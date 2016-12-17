using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BCW.Common;
using BCW.PK10;
using BCW.PK10.Model;

public partial class bbs_game_PK10Get : System.Web.UI.Page
{
    protected string xmlPath = "/Controls/PK10.xml";
    protected int myValidDays = 15;//当期的兑奖有效日期,默认15天
    protected int mySaleTimes = 5;//每期游戏开售时长分钟数,默认5分钟
    protected int myOpenTimes = 40;//每期游戏开奖公布延时秒数（默认在停售后1分钟后开奖）
    protected string cDefaultOpenTime = "09:07:30"; //每天第一期默认开奖时间
    protected int defaultStopSec = 60;//默认开奖时间提前X秒截至销售
    protected int defaultStopSecMin = 30;//开奖时间提前X秒截至销售（最小值）
    protected int defaultOpenTimes = 179;//每人销售的期数
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string GameName = ub.GetSub("GameName", xmlPath);
            DateTime GameBeginTime, GameEndTime, CurrentTime;
            int GameOpenTimes, SaleTimes, OpenTimes, ValidDays;
            int StopSec = defaultStopSec;
            bool AutoCalc = false;
            #region 读取配置项，并初始化变量
            try
            {
                int.TryParse(ub.GetSub("StopSaleSec", xmlPath),out StopSec);
                if (StopSec <= defaultStopSecMin)
                    StopSec = defaultStopSec;
                //
                string ct1 = ub.GetSub("GameBeginTime", xmlPath);
                string ct2 = ub.GetSub("GameEndTime", xmlPath);
                GameBeginTime = Convert.ToDateTime(ct1);
                GameEndTime = Convert.ToDateTime(ct2);
                CurrentTime = DateTime.Now;
                GameOpenTimes = int.Parse(ub.GetSub("GameOpenTimes", xmlPath)); //游戏每天开奖期数
                SaleTimes = int.Parse(ub.GetSub("SaleTimes", xmlPath));//每期游戏开售分钟数
                OpenTimes = int.Parse(ub.GetSub("OpenTimes", xmlPath));//每期游戏开奖分钟数（在上期停售后N秒后开奖）
                ValidDays = int.Parse(ub.GetSub("ValidDays", xmlPath)); //当期的兑奖有效日期天数
                myOpenTimes = OpenTimes;
                mySaleTimes = SaleTimes;
                myValidDays = ValidDays;
                int autocalc = int.Parse(ub.GetSub("AutoCalc", xmlPath));
                if (autocalc == 1)
                    AutoCalc = true;
            }
            catch (Exception ex)
            {
                Response.Write("error1“读取配置项并初始化变量”出错！！！" + "</br>");
                Response.Write(ex.Message.Replace("\n", "</br>"));
                return;
            }
            #endregion
            Response.Write("PK10每天的销售时间为" + GameBeginTime.ToShortTimeString() + "至" + GameEndTime.ToShortTimeString() + "，每" + SaleTimes.ToString() + "分钟一期，每天" + GameOpenTimes.ToString().Trim() + "期" + "<br/>");
            //
            PK10_Base _base = new PK10().GetSaleBase();
            #region 判断是否已有初始化(tb_PK10_Base表添加一行ID=0的记录)
            if (_base == null)
            {
                Response.Write("ok1 没有初始化游戏数据" + "<br/>");
                return;
            }
            #endregion

            if (_base.CurrentSaleDate < DateTime.Parse(CurrentTime.ToShortDateString()))
            {
                int newNo = new PK10().GetTodayFistCreateNo();
                if (newNo>0)
                {
                    
                    #region 初始化新的一天数据
                    //新的一天,从网页抓取正在开售的期
                    Response.Write("初始化新的一天数据...." + "<br/>");
                    string html = new PK10().GetHtmlByURL();
                    PK10_List list = new PK10().GetCurrentSaleDataByHtml(html);
                    if (list != null)
                    {
                        //初始化当天销售数据（生成将要开售的期号）
                        string cInitFlag = new PK10().InitSaleData(GameBeginTime,newNo, mySaleTimes, myValidDays, GameOpenTimes, StopSec);
                        if (cInitFlag == "")
                        {
                            GetLatestOpenDatas(html, "0");//获取当天已经开奖的数据（补填当天的销售记录）(从百度彩票抓，可以抓到整日的数据）
                        }
                        else
                            Response.Write("error1 :初始化当天销售数据失败！" + "<br/>");
                    }
                    else
                        Response.Write("error1 :抓取网页数据失败！" + "<br/>");
                    #endregion
                }
                else
                {
                    Response.Write("error1“数据隔了1天以上没抓取过，请先设置今天的开始期号！”" + "</br>");
                    //向上一天插入一跳最后一期的记录，例如：insert into tb_PK10_List(No,date) values('584063','2016-10-30')，不用设置begintime,endtime等。
                }
                return;
            }
            else
            {
                Response.Write("<a href =\"" + Utils.getPage("PK10Get.aspx" + "?act=getall") + "\">" + "手动抓取最近所有开奖数据" + "</a><br/>");
                Response.Write("<a href =\"" + Utils.getPage("PK10Get.aspx") + "\">" + "自动抓取开奖数据" + "</a><br/>");
                Response.Write("<br/>");
                string act = Utils.GetRequest("act", "get", 1, "", "");
                if (act == "getall")
                {
                    Response.Write("【手动抓取最近所有开奖数据...】" + " <br/>");
                    string html = new PK10().GetHtmlByURL();
                    RefreshLatestOpenData(html);
                    GetLatestOpenDatas(html, "0"); //(从百度彩票抓，可以抓到整日的数据）
                }
                else
                {
                    Response.Write("【自动抓取第" + _base.GetOpenDataNo.ToString() + "期】" + "<br/>");
                    #region 读取开奖记录
                    if (CurrentTime < _base.GetOpenDataBeginTime) //未到公布开奖时间
                    {
                        Response.Write("等待开奖公布..." + "<br/>");
                    }
                    else
                    {

                        if (CurrentTime <= _base.GetOpenDataEndTime) //处于本期公布开奖时间,读取开奖记录，并重置下次读取的时间段
                        {
                            string html = new PK10().GetHtmlByURL2();
                            RefreshLatestOpenData(html);
                        }
                        else //超过指定的开奖公布时间，读取最新开奖记录，并重置下次读取的时间段；然后读取已开奖的记录更新开奖（刷新机可能停止运作了一段长的时间）
                        {
                            string html = new PK10().GetHtmlByURL2();
                            RefreshLatestOpenData(html);
                            html = new PK10().GetHtmlByURL();
                            GetLatestOpenDatas(html, _base.GetOpenDataNo); //(从百度彩票抓，可以抓到整日的数据）
                        }
                    }
                    #endregion
                }
            }
            if (AutoCalc)
                CalcOpenData(); //派奖计算
            Response.Write("ok1");
        }
        catch(Exception ex)
        {
            Response.Write("error1" + "<br/>" + ex.Message);
            return;
        }
    }

    //
    #region 最新开奖信息
    private void RefreshLatestOpenData(string html)
    {
        Response.Write("-----最新开奖-----" + "<br/>");
        try
        {
            PK10_List list = new PK10().GetCurrentOpenDataByHtml2(html); //从付费网站，比较快开奖
            if (list != null)
            {
                list.ValidDate = list.Date.AddDays(myValidDays);
                Response.Write("第" + list.No + "期" + "<br/>");
                Response.Write("开奖时间：" + list.EndTime.ToString() + "<br/>");
                Response.Write("开奖号码：" + list.Nums + "<br/>");
                //更新开奖
                string cSave=new PK10().SaveOpenData(list);
                if (cSave == "")
                    new PK10().SetNextGetOpenData(myOpenTimes, mySaleTimes);
                else
                    Response.Write("【更新开奖出错】:" + cSave);
            }
        }
        catch (Exception ex)
        {
            Response.Write("[最新开奖信息]出错：" + ex.Message.Replace("\n", "</br>") + "<br/>");
        }
    }
    #endregion
    #region 最近开奖记录
    private void GetLatestOpenDatas(string html,string beginNo)
    {
        Response.Write("-----最近开奖-----" + "<br/>");
        try
        {
            IList<PK10_List> openList = new PK10().GetLatestOpenDataByHtml(html);

            if (openList.Count > 0)
            {
                int beginno = int.Parse(beginNo);
                for (int i = openList.Count - 1; i >= 0; i--)
                {
                    int no = int.Parse(openList[i].No.ToString());
                    if (no >= beginno)//已经读取过的开奖记录，不计算
                    {
                        openList[i].ValidDate = openList[i].Date.AddDays(myValidDays);
                        Response.Write("第" + openList[i].No + "期" + openList[i].Nums + "<br/>");
                    }
                }
            }
            else
            {
                Response.Write("无开奖记录" + "<br/>");
                return;
            }
            //更新最近开奖记录
            string cSave = new PK10().SaveOpenDatas(openList,beginNo);
            if(cSave == "")
                new PK10().SetNextGetOpenData(myOpenTimes, mySaleTimes);
            else
                Response.Write("【更新开奖出错】:" + cSave);
        }
        catch (Exception ex)
        {
            Response.Write("[最近开奖记录]出错：" + ex.Message.Replace("\n", "</br>") + "<br/>");
        }
    }
    #endregion
    #region 派奖计算
    private void CalcOpenData()
    {
        Response.Write("【派奖计算】" + "<br/>");
        try
        {
            string cFlag = new PK10().CalcOpenData();
            if (cFlag != "")
                Response.Write("[派奖计算]出错：" + cFlag.Replace("\n", "</br>") + "<br/>");
            else
                Response.Write("派奖完成！");
        }
        catch (Exception ex)
        {
            Response.Write("[派奖计算]出错：" + ex.Message.Replace("\n", "</br>") + "<br/>");
        }
    }
    #endregion
    //
}