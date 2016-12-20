using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using BCW.Common;
using System.Timers;
public partial class bbs_game_hc1Get : System.Web.UI.Page
{
    /// <summary>
    /// wdy 20160524 1
    /// </summary>
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/hc1.xml";
    protected string GameName = ub.GetSub("Hc1Name", "/Controls/hc1.xml");//游戏名字
    protected void Page_Load(object sender, EventArgs e)
    {
        string st1 = "01:00";
        string st2 = "23:59";
        DateTime dt1 = Convert.ToDateTime(st1);
        DateTime dt2 = Convert.ToDateTime(st2);
        DateTime dt3 = DateTime.Now;
        string _time = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
        if ((DateTime.Compare(dt1, dt3) > 0) || ((DateTime.Compare(dt2, dt3) < 0)))
        {
            Response.Write("<br /><b>程序好彩1_没到开奖时间，请等待。。。close1</b><br /><br />");
            Response.Write("<br /><b>开奖时间：" + st1 + "到" + st2 + "<br /><br />");
        }
        else
        {
            hc1Get();//获取开奖数据
            RobotCase(); //机器人兑奖
        }
    }
    public void hc1Get()
    {
        if (GetStage() != "" || GetTime() != "")//判断获取期数是否为空
        {
            int qishu = Convert.ToInt32(GetStage());
            //判断该期数是否存在
            bool s = new BCW.BLL.Game.HcList().Existsm(qishu);
            switch (s)
            {
                case false:
                    {
                        #region 如果该开奖期号不存在，则增加    
                        //存期号，开奖号码，截止时间
                        BCW.Model.Game.HcList model = new BCW.Model.Game.HcList();
                        model.CID = qishu;
                        model.Result = "";
                        model.Notes = "";
                        model.payCent = 0;
                        model.payCount = 0;
                        model.EndTime = Convert.ToDateTime(GetTime());
                        model.State = 0;
                        model.payCent2 = 0;
                        model.payCount2 = 0;
                        new BCW.BLL.Game.HcList().Add(model);
                        Response.Write("第" + qishu + "期添加期号成功。。。。ok1<br/>");
                        #endregion
                    }
                    break;
                case true:
                    {
                        string qs = GetStageS();//得到接口的期数
                        string jieguo = Getnumber();//得到接口的开奖结果
                        if (qs != "")//是否获取到上期开奖期号
                        {
                            DataSet ds1 = new BCW.BLL.Game.HcList().GetList("Top 1 id", "State=0 Order BY ID ASC");
                            int id = 0;
                            try
                            {
                                id = int.Parse(ds1.Tables[0].Rows[0][0].ToString());
                            }
                            catch { }
                            if (id == 0)
                            {
                                Response.Write("已全部开奖.还没有加入新期号.ok1<br/>");
                                Response.End();
                            }
                            BCW.Model.Game.HcList m1 = new BCW.BLL.Game.HcList().GetHcList(id);
                            if (qs == Convert.ToString(m1.CID))//判断上期开奖期号是否与存储期号相等
                            {
                                if (jieguo != "")//是否获取上期开奖号码
                                {
                                    #region  遍历当期所有投注
                                    //开奖号码
                                    int Result = Convert.ToInt32(jieguo);
                                    DataSet ds = new BCW.BLL.Game.HcPay().GetList("*", "CID=" + m1.CID + " and State=0");
                                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                                    {
                                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                        {
                                            int pid = int.Parse(ds.Tables[0].Rows[i]["id"].ToString());
                                            int Types = int.Parse(ds.Tables[0].Rows[i]["Types"].ToString());
                                            int UsID = int.Parse(ds.Tables[0].Rows[i]["UsID"].ToString());
                                            string UsName = ds.Tables[0].Rows[i]["UsName"].ToString();
                                            string Vote = ds.Tables[0].Rows[i]["Vote"].ToString();
                                            long PayCent = Int64.Parse(ds.Tables[0].Rows[i]["PayCent"].ToString());
                                            double Odds = 0;//赔率
                                            bool IsWin = false;
                                            if (Types == 1)//选号开奖
                                            {
                                                if (("," + Vote + ",").Contains("," + Result + ","))
                                                {
                                                    IsWin = true;
                                                    Odds = Convert.ToDouble(ub.GetSub("Hc1odds1", xmlPath));
                                                }
                                            }
                                            else if (Types == 2)//生肖开奖
                                            {
                                                string[] VoteTemp = Vote.Split(",".ToCharArray());
                                                for (int k = 0; k < VoteTemp.Length; k++)
                                                {
                                                    string sVote = OutSxNum(VoteTemp[k]);
                                                    if (("," + sVote + ",").Contains("," + Result + ","))
                                                    {
                                                        IsWin = true;
                                                        Odds = Convert.ToDouble(ub.GetSub("Hc1odds2", xmlPath));
                                                        break;
                                                    }
                                                }
                                            }
                                            else if (Types == 3)//方位开奖
                                            {
                                                string[] VoteTemp = Vote.Split(",".ToCharArray());
                                                for (int k = 0; k < VoteTemp.Length; k++)
                                                {
                                                    string sVote = OutFwNum(VoteTemp[k]);
                                                    if (("," + sVote + ",").Contains("," + Result + ","))
                                                    {
                                                        IsWin = true;
                                                        Odds = Convert.ToDouble(ub.GetSub("Hc1odds3", xmlPath));
                                                        break;
                                                    }
                                                }
                                            }
                                            else if (Types == 4)//四季开奖
                                            {
                                                string[] VoteTemp = Vote.Split(",".ToCharArray());
                                                for (int k = 0; k < VoteTemp.Length; k++)
                                                {
                                                    string sVote = OutSjNum(VoteTemp[k]);
                                                    if (("," + sVote + ",").Contains("," + Result + ","))
                                                    {
                                                        IsWin = true;
                                                        Odds = Convert.ToDouble(ub.GetSub("Hc1odds4", xmlPath));
                                                        break;
                                                    }
                                                }
                                            }
                                            else if (Types == 5)//大小单双
                                            {
                                                if (("," + OutDXDS(Result) + ",").Contains("," + Vote + ","))
                                                {
                                                    IsWin = true;
                                                    if (Vote == "大")
                                                        Odds = Convert.ToDouble(ub.GetSub("Hc1da", xmlPath));
                                                    else if (Vote == "小")
                                                        Odds = Convert.ToDouble(ub.GetSub("Hc1xiao", xmlPath));
                                                    else if (Vote == "单")
                                                        Odds = Convert.ToDouble(ub.GetSub("Hc1dan", xmlPath));
                                                    else if (Vote == "双")
                                                        Odds = Convert.ToDouble(ub.GetSub("Hc1shuang", xmlPath));
                                                }
                                            }
                                            else if (Types == 6)//六肖
                                            {
                                                if (("," + Vote + ",").Contains("," + OutSx(Result.ToString()) + ","))
                                                {
                                                    IsWin = true;
                                                    Odds = Convert.ToDouble(ub.GetSub("Hc1odds9", xmlPath));
                                                }
                                            }
                                            else if (Types == 7)//尾数大小
                                            {
                                                string wsdx = "";
                                                int ws = Convert.ToInt32(Utils.Right(Result.ToString(), 1));
                                                if (ws < 5)
                                                    wsdx = "小";
                                                else
                                                    wsdx = "大";
                                                if (wsdx == Vote)
                                                {

                                                    IsWin = true;
                                                    if (Vote == "大")
                                                        Odds = Convert.ToDouble(ub.GetSub("Hc1wsda", xmlPath));
                                                    else
                                                        Odds = Convert.ToDouble(ub.GetSub("Hc1wsxiao", xmlPath));

                                                }
                                            }
                                            else if (Types == 8)//尾数单双
                                            {
                                                string wsds = "";
                                                int ws = Convert.ToInt32(Utils.Right(Result.ToString(), 1));
                                                if (ws % 2 == 0)
                                                    wsds = "双";
                                                else
                                                    wsds = "单";
                                                if (wsds == Vote)
                                                {
                                                    IsWin = true;
                                                    if (Vote == "单")
                                                        Odds = Convert.ToDouble(ub.GetSub("Hc1wsdan", xmlPath));
                                                    else
                                                        Odds = Convert.ToDouble(ub.GetSub("Hc1wsshuang", xmlPath));
                                                }
                                            }
                                            else if (Types == 9)//家禽野兽
                                            {
                                                string jq = "牛,马,羊,鸡,狗,猪";
                                                string ys = "鼠,猴,兔,虎,龙,蛇";
                                                string sVote = "";
                                                if (Vote == "家禽")
                                                    sVote = jq;
                                                else
                                                    sVote = ys;

                                                if (("," + sVote + ",").Contains("," + OutSx(Result.ToString()) + ","))
                                                {
                                                    IsWin = true;
                                                    if (Vote == "家禽")
                                                        Odds = Convert.ToDouble(ub.GetSub("Hc1odds14", xmlPath));
                                                    else
                                                        Odds = Convert.ToDouble(ub.GetSub("Hc1odds15", xmlPath));
                                                }
                                            }
                                            //else if (Types == 10)//自选不中
                                            //{
                                            //    if (!("," + Vote + ",").Contains("," + Result + ","))
                                            //    {
                                            //        IsWin = true;
                                            //        int cNum = Utils.GetStringNum(Vote, ",") + 1;
                                            //        if (cNum == 5)
                                            //            Odds = Convert.ToDouble(ub.GetSub("Hc1odds16", xmlPath));
                                            //        else if (cNum == 6)
                                            //            Odds = Convert.ToDouble(ub.GetSub("Hc1odds17", xmlPath));
                                            //        else if (cNum == 7)
                                            //            Odds = Convert.ToDouble(ub.GetSub("Hc1odds18", xmlPath));
                                            //        else if (cNum == 8)
                                            //            Odds = Convert.ToDouble(ub.GetSub("Hc1odds19", xmlPath));
                                            //        else if (cNum == 9)
                                            //            Odds = Convert.ToDouble(ub.GetSub("Hc1odds20", xmlPath));
                                            //        else if (cNum == 10)
                                            //            Odds = Convert.ToDouble(ub.GetSub("Hc1odds21", xmlPath));
                                            //    }
                                            //}
                                            if (IsWin == true)
                                            {
                                                BCW.Data.SqlHelper.ExecuteSql("update tb_HcPay set WinCent=" + Convert.ToInt64(PayCent * Odds) + " Where id=" + pid + "");
                                                //发送内线
                                                new BCW.BLL.Guest().Add(1, UsID, UsName, "恭喜您在第" + m1.CID + "期好彩一《" + ForType(Types) + "》投注，中奖" + Convert.ToInt64(PayCent * Odds) + "" + ub.Get("SiteBz") + "，开奖为" + Result + "-" + OutSx(Result.ToString()) + "-" + OutSj(Result.ToString()) + "-" + OutFw(Result.ToString()) + "[URL=/bbs/game/hc1.aspx?act=case]马上兑奖[/URL]");
                                            }
                                        }
                                    }
                                    //完成返彩后正式更新该期为结束
                                    BCW.Data.SqlHelper.ExecuteSql("update tb_HcList set Result='" + Result + "',State=1 Where CID=" + m1.CID + "");
                                    BCW.Data.SqlHelper.ExecuteSql("update tb_HcPay set State=1 Where CID=" + m1.CID + "");
                                    change_peilv();//赔率浮动
                                    #endregion
                                }
                                else
                                    Response.Write("第" + m1.CID + "期获取开奖号码失败。。。。error1<br/>");
                            }
                            else
                                Response.Write("期数获取成功！<br/>上期开奖期数：" + qs + ".<br/>上期开奖结果：" + jieguo + ".<br/>等待开奖期号：" + m1.CID + ".<br/>现在未到开奖时间！请等待开奖.ok1<br/>");
                        }
                    }
                    break;
            }
        }
        else
            Response.Write("<b>《《《期号或开奖时间获取失败。。。。error1》》》</b>");
    }

    //取得好彩一 (抓取百度彩票网页数据)
    public string GetNewsUrl()
    {
        string strUrl = "http://baidu.lecai.com/lottery/draw/view/555";
        string str = "";
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strUrl);
        request.Timeout = 30000;
        request.AllowAutoRedirect = false;
        request.KeepAlive = false;
        request.ProtocolVersion = HttpVersion.Version11;
        request.Headers["Accept-Language"] = "zh-cn";
        request.UserAgent = "mozilla/4.0 (compatible; msie 6.0; windows nt 5.1; sv1; .net clr 1.0.3705; .net clr 2.0.50727; .net clr 1.1.4322)";
        try
        {
            WebResponse response = request.GetResponse();
            System.IO.Stream stream = response.GetResponseStream();
            System.IO.StreamReader sr = new System.IO.StreamReader(stream, Encoding.GetEncoding("UTF-8"));
            str = sr.ReadToEnd();
            stream.Close();
            sr.Close();
        }
        catch (Exception e)
        {
            str = e.ToString().Replace("\n", "<BR>");
        }
        return str;
    }

    //得到最新期开奖期数(抓取百度彩票网页数据)
    public string GetStage()
    {
        String s = GetNewsUrl();
        String stage = @"\""phase""\:\""[\d]{7}";
        Match stages = Regex.Match(s, stage);
        String stageto = @"[\d]{7}";
        Match stageok = Regex.Match(stages.Value, stageto);
        if (stageok.Value != null)
        {
            return stageok.Value;
        }
        else
        {
            return "";
        }
    }

    //得到开奖时间(抓取百度彩票网页数据)
    public string GetTime()
    {
        String s = GetNewsUrl();
        String stage = @"\""time_endsale""\:\""[\d]{4}-[\d]{2}-[\d]{1,2} [\d]{2}:[\d]{2}:[\d]{2}";
        Match stages = Regex.Match(s, stage);
        String stageto = @"[\d]{4}-[\d]{2}-[\d]{1,2} [\d]{2}:[\d]{2}:[\d]{2}";
        Match stageok = Regex.Match(stages.Value, stageto);
        if (stageok.Value != null)
        {
            return stageok.Value;
        }
        else
        {
            return "";
        }
    }

    //取得好彩一 (接口)
    public string GetNewsUrl1()
    {
        string strUrl = "http://c.apiplus.net/newly.do?token=5a8ebe52461354bc&code=gdfchc1&rows=1";
        string str = "";
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strUrl);
        request.Timeout = 30000;
        request.AllowAutoRedirect = false;
        request.KeepAlive = false;
        request.ProtocolVersion = HttpVersion.Version11;
        request.Headers["Accept-Language"] = "zh-cn";
        request.UserAgent = "mozilla/4.0 (compatible; msie 6.0; windows nt 5.1; sv1; .net clr 1.0.3705; .net clr 2.0.50727; .net clr 1.1.4322)";
        try
        {
            WebResponse response = request.GetResponse();
            System.IO.Stream stream = response.GetResponseStream();
            System.IO.StreamReader sr = new System.IO.StreamReader(stream, Encoding.GetEncoding("UTF-8"));
            str = sr.ReadToEnd();
            stream.Close();
            sr.Close();
        }
        catch (Exception e)
        {
            str = e.ToString().Replace("\n", "<BR>");
        }
        return str;
    }

    //得到上期开奖期数(接口)
    public string GetStageS()
    {
        String s = GetNewsUrl1();
        String stage = @"expect=""[\d]{7}";
        Match stages = Regex.Match(s, stage);
        String stageto = @"[\d]{7}";
        Match stageok = Regex.Match(stages.Value, stageto);
        if (stageok.Value != null && stageok.Value != "")
        {
            return stageok.Value;
        }
        else
        {
            return "";
        }
    }

    //得到上期开奖号码(接口)
    public string Getnumber()
    {
        String s = GetNewsUrl1();
        String stage = @"opencode=""[\d]{2}";
        Match stages = Regex.Match(s, stage);
        String stageto = @"[\d]{2}";
        Match stageok = Regex.Match(stages.Value, stageto);
        if (stageok.Value != null && stageok.Value != "")
        {
            return stageok.Value;
        }
        else
        {
            return "";
        }
    }


    private string OutSxNum(string sx)
    {
        string sNum = "";
        if (sx == "鼠")
            sNum = ",1,13,25,";
        else if (sx == "牛")
            sNum = ",2,14,26,";
        else if (sx == "虎")
            sNum = ",3,15,27,";
        else if (sx == "兔")
            sNum = ",4,16,28,";
        else if (sx == "龙")
            sNum = ",5,17,29,";
        else if (sx == "蛇")
            sNum = ",6,18,30,";
        else if (sx == "马")
            sNum = ",7,19,31,";
        else if (sx == "羊")
            sNum = ",8,20,32,";
        else if (sx == "猴")
            sNum = ",9,21,33,";
        else if (sx == "鸡")
            sNum = ",10,22,34,";
        else if (sx == "狗")
            sNum = ",11,23,35,";
        else if (sx == "猪")
            sNum = ",12,24,36,";
        return sNum;
    }
    private string OutFwNum(string fw)
    {
        string sNum = "";
        if (fw == "东")
            sNum = ",1,3,5,7,9,11,13,15,17,";
        else if (fw == "南")
            sNum = ",2,4,6,8,10,12,14,16,18,";
        else if (fw == "西")
            sNum = ",19,21,23,25,27,29,31,33,35,";
        else if (fw == "北")
            sNum = ",20,22,24,26,28,30,32,34,36,";
        return sNum;
    }
    private string OutSjNum(string sj)
    {
        string sNum = "";
        if (sj == "春")
            sNum = ",1,2,3,4,5,6,7,8,9,";
        else if (sj == "夏")
            sNum = ",10,11,12,13,14,15,16,17,18,";
        else if (sj == "秋")
            sNum = ",19,20,21,22,23,24,25,26,27,";
        else if (sj == "冬")
            sNum = ",28,29,30,31,32,33,34,35,36,";
        return sNum;
    }
    private string OutSx(string Result)
    {
        string sx = "";
        if ((",1,13,25,").Contains("," + Result + ","))
            sx = "鼠";
        else if ((",2,14,26,").Contains("," + Result + ","))
            sx = "牛";
        else if ((",3,15,27,").Contains("," + Result + ","))
            sx = "虎";
        else if ((",4,16,28,").Contains("," + Result + ","))
            sx = "兔";
        else if ((",5,17,29,").Contains("," + Result + ","))
            sx = "龙";
        else if ((",6,18,30,").Contains("," + Result + ","))
            sx = "蛇";
        else if ((",7,19,31,").Contains("," + Result + ","))
            sx = "马";
        else if ((",8,20,32,").Contains("," + Result + ","))
            sx = "羊";
        else if ((",9,21,33,").Contains("," + Result + ","))
            sx = "猴";
        else if ((",10,22,34,").Contains("," + Result + ","))
            sx = "鸡";
        else if ((",11,23,35,").Contains("," + Result + ","))
            sx = "狗";
        else if ((",12,24,36,").Contains("," + Result + ","))
            sx = "猪";
        return sx;
    }
    private string OutFw(string Result)
    {
        string fw = "";
        if ((",1,3,5,7,9,11,13,15,17,").Contains("," + Result + ","))
            fw = "东";
        else if ((",2,4,6,8,10,12,14,16,18,").Contains("," + Result + ","))
            fw = "南";
        else if ((",19,21,23,25,27,29,31,33,35,").Contains("," + Result + ","))
            fw = "西";
        else if ((",20,22,24,26,28,30,32,34,36,").Contains("," + Result + ","))
            fw = "北";
        return fw;
    }
    private string OutSj(string Result)
    {
        string sj = "";
        if ((",1,2,3,4,5,6,7,8,9,").Contains("," + Result + ","))
            sj = "春";
        if ((",10,11,12,13,14,15,16,17,18,").Contains("," + Result + ","))
            sj = "夏";
        if ((",19,20,21,22,23,24,25,26,27,").Contains("," + Result + ","))
            sj = "秋";
        if ((",28,29,30,31,32,33,34,35,36,").Contains("," + Result + ","))
            sj = "冬";
        return sj;
    }
    private string OutDXDS(int Result)
    {
        string dxds = "";
        int i = Result;
        if (i <= 18)
            dxds += "小,";
        else
            dxds += "大,";

        if (i % 2 == 0)
            dxds += "双";
        else
            dxds += "单";
        return dxds;
    }
    private string OutDS(int Result)
    {
        string dxds = "";
        int i = Result;
        if (i % 2 == 0)
            dxds += "双";
        else
            dxds += "单";
        return dxds;

    }
    private string OutDX(int Result)
    {
        string dxds = "";

        int i = Result;
        if (i <= 18)
            dxds += "小";
        else
            dxds += "大";
        return dxds;
    }
    private string OutwsDX(int Result)
    {
        string wsdx = "";
        int ws = Convert.ToInt32(Utils.Right(Result.ToString(), 1));
        if (ws < 5)
            wsdx = "小";
        else
            wsdx = "大";
        return wsdx;
    }
    private string OutwsDS(int Result)
    {
        string wsds = "";
        int ws = Convert.ToInt32(Utils.Right(Result.ToString(), 1));
        if (ws % 2 == 0)
            wsds = "双";
        else
            wsds = "单";
        return wsds;
    }
    //大小单双和尾数单双赔率浮动
    public void change_peilv()
    {
        BCW.Model.Game.HcList model_2 = new BCW.BLL.Game.HcList().GetHcListNew(1);//开奖的的最新数据                                                                     
        string qihao = Convert.ToString(model_2.CID);
        string kai = model_2.Result;//开奖结果
        Response.Write("开奖数据：" + model_2.CID + "<br />");
        ub xml = new ub();
        string xmlPath_update = "/Controls/hc1.xml";
        Application.Remove(xmlPath_update);//清缓存
        xml.ReloadSub(xmlPath_update); //加载配置
        #region 期号获取赔率浮动   
        if (kai != "" && qihao != "")
        {
            string issue3 = Utils.Right(model_2.CID.ToString(), 3);//本期开奖期号的后3位
            if (issue3 != "001")
            {
                DataSet d1, d2;
                //获取最后一期期号
                d1 = new BCW.BLL.Game.HcList().GetList("TOP 1 *", "CID!='' and State=1 ORDER BY ID DESC");
                //获取倒数第二期期号
                int ID = new BCW.BLL.Game.HcList().CID();
                BCW.Model.Game.HcList mo = new BCW.BLL.Game.HcList().GetHcList(ID);
                OutDX(Convert.ToInt32(model_2.Result));//最后一期大小
                OutDS(Convert.ToInt32(model_2.Result));//最后一期单双
                OutwsDX(Convert.ToInt32(model_2.Result));//最后一期尾数大小
                OutwsDS(Convert.ToInt32(model_2.Result));//最后一期尾数单双

                Response.Write("最后一期大小:" + "<b>" + OutDX(Convert.ToInt32(model_2.Result)) + "</b><br />");
                string Cents1 = "";
                string Cents2 = "";
                string Cents3 = "";
                string Cents4 = "";
                string Cents5 = "";
                string Cents6 = "";
                string Cents7 = "";
                string Cents8 = "";
                Cents1 = OutDX(Convert.ToInt32(model_2.Result));//最后一期的大小
                Cents2 = OutDS(Convert.ToInt32(model_2.Result));//最后一期的单双
                Cents5 = OutwsDX(Convert.ToInt32(model_2.Result));//最后一期尾数大小              
                Cents6 = OutwsDS(Convert.ToInt32(model_2.Result));//最后一期尾数单双

                Response.Write("最后一期大小:" + "<b>" + Cents1 + "</b><br />");

                try
                {
                    Cents3 = OutDX(Convert.ToInt32(mo.Result));//倒数第2期的大小
                    Cents4 = OutDS(Convert.ToInt32(mo.Result));//倒数第2期的单双
                    Cents7 = OutwsDX(Convert.ToInt32(mo.Result));//倒数第2期尾数的大小
                    Cents8 = OutwsDS(Convert.ToInt32(mo.Result));//倒数第2期尾数的单双
                }
                catch
                {
                    Cents3 = "";
                    Cents4 = "";
                    Cents7 = "";
                    Cents8 = "";
                }

                Response.Write("倒数第二期大小:" + "<b>" + Cents3 + "</b><br />");
                Response.Write("倒数第二期单双:" + "<b>" + Cents4 + "</b><br />");
                Response.Write("倒数第二期尾数大小:" + "<b>" + Cents7 + "</b><br />");
                Response.Write("倒数第二期尾数单双:" + "<b>" + Cents8 + "</b><br />");
                if (Cents1 != Cents3)//如果连续2期不相等，还原赔率----大小
                {
                    xml.dss["Hc1da"] = xml.dss["Hc1odds5"];//da
                    xml.dss["Hc1xiao"] = xml.dss["Hc1odds6"];//xiao
                    System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                }
                else
                {
                    //1大2小、1单2双
                    if (Cents1 == "小" && Cents3 == "小")//如果连续2期开小
                    {
                        if (Convert.ToDouble(xml.dss["Hc1xiao"]) >= Convert.ToDouble(xml.dss["Hc1overpeilv"]))//如果增加到一定的赔率，则不再增加。
                        {
                            if (Convert.ToDouble(xml.dss["Hc1da"]) - 1.2 > 0)
                            {
                                xml.dss["Hc1da"] = Convert.ToDouble(xml.dss["Hc1da"]) - Convert.ToDouble(xml.dss["Hc1fudong"]);//大的赔率减少
                            }
                            else
                            {
                                xml.dss["Hc1da"] = Convert.ToDouble(xml.dss["Hc1odds7"]);
                            }
                        }
                        else
                        {
                            xml.dss["Hc1xiao"] = Convert.ToDouble(xml.dss["Hc1xiao"]) + Convert.ToDouble(xml.dss["Hc1fudong"]);//小的赔率增加
                            xml.dss["Hc1da"] = Convert.ToDouble(xml.dss["Hc1da"]) - Convert.ToDouble(xml.dss["Hc1fudong"]);//大的赔率减少
                        }
                        System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                    }
                    else if (Cents1 == "大" && Cents3 == "大")//大
                    {
                        if (Convert.ToDouble(xml.dss["Hc1da"]) >= Convert.ToDouble(xml.dss["Hc1overpeilv"]))
                        {
                            if (Convert.ToDouble(xml.dss["Hc1xiao"]) - 1.2 > 0)
                                xml.dss["Hc1xiao"] = Convert.ToDouble(xml.dss["Hc1xiao"]) - Convert.ToDouble(xml.dss["Hc1fudong"]);//小的赔率减少
                            else
                                xml.dss["Hc1xiao"] = Convert.ToDouble(xml.dss["Hc1odds7"]);
                        }
                        else
                        {
                            xml.dss["Hc1xiao"] = Convert.ToDouble(xml.dss["Hc1xiao"]) - Convert.ToDouble(xml.dss["Hc1fudong"]);//小的赔率减少
                            xml.dss["Hc1da"] = Convert.ToDouble(xml.dss["Hc1da"]) + Convert.ToDouble(xml.dss["Hc1fudong"]);//大的赔率增加
                        }
                        System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                    }
                }
                if (Cents5 != Cents7)//如果连续2期不相等，还原赔率----尾数大小
                {
                    xml.dss["Hc1wsda"] = xml.dss["Hc1odds10"];//da
                    xml.dss["Hc1wsxiao"] = xml.dss["Hc1odds11"];//xiao
                    System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                }
                else
                {

                    if (Cents5 == "小" && Cents7 == "小")//如果连续2期开小
                    {
                        if (Convert.ToDouble(xml.dss["Hc1wsxiao"]) >= Convert.ToDouble(xml.dss["Hc1overpeilv"]))//如果增加到一定的赔率，则不再增加。
                        {
                            if (Convert.ToDouble(xml.dss["Hc1wsda"]) - 1.2 > 0)
                                xml.dss["Hc1wsda"] = Convert.ToDouble(xml.dss["Hc1wsda"]) - Convert.ToDouble(xml.dss["Hc1fudong"]);//大的赔率减少
                            else
                                xml.dss["Hc1wsda"] = xml.dss["Hc1odds11"];
                        }
                        else
                        {
                            xml.dss["Hc1wsxiao"] = Convert.ToDouble(xml.dss["Hc1wsxiao"]) + Convert.ToDouble(xml.dss["Hc1fudong"]);//小的赔率增加
                            xml.dss["Hc1wsda"] = Convert.ToDouble(xml.dss["Hc1wsda"]) - Convert.ToDouble(xml.dss["Hc1fudong"]);//大的赔率减少
                        }
                        System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                    }
                    else if (Cents5 == "大" && Cents7 == "大")//大
                    {
                        if (Convert.ToDouble(xml.dss["Hc1wsda"]) >= Convert.ToDouble(xml.dss["Hc1overpeilv"]))
                        {
                            if (Convert.ToDouble(xml.dss["Hc1wsxiao"]) - 1.2 > 0)
                                xml.dss["Hc1wsxiao"] = Convert.ToDouble(xml.dss["Hc1wsxiao"]) - Convert.ToDouble(xml.dss["Hc1fudong"]);//小的赔率减少
                            else
                                xml.dss["Hc1wsxiao"] = xml.dss["Hc1odds11"];
                        }
                        else
                        {
                            xml.dss["Hc1wsxiao"] = Convert.ToDouble(xml.dss["Hc1wsxiao"]) - Convert.ToDouble(xml.dss["Hc1fudong"]);//小的赔率减少
                            xml.dss["Hc1wsda"] = Convert.ToDouble(xml.dss["Hc1wsda"]) + Convert.ToDouble(xml.dss["Hc1fudong"]);//大的赔率增加
                        }
                        System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                    }
                }
                if (Cents2 != Cents4)//如果连续2期不相等，还原赔率----单双
                {
                    xml.dss["Hc1dan"] = xml.dss["Hc1odds7"];
                    xml.dss["Hc1shuang"] = xml.dss["Hc1odds8"];
                    System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                }
                else
                {
                    //1大2小、1单2双
                    if (Cents2 == "单" && Cents4 == "单")//如果连续2期开单
                    {
                        if (Convert.ToDouble(xml.dss["Hc1dan"]) >= Convert.ToDouble(xml.dss["Hc1overpeilv"]))
                        {
                            if (Convert.ToDouble(xml.dss["Hc1shuang"]) - 1.2 > 0)
                                xml.dss["Hc1shuang"] = Convert.ToDouble(xml.dss["Hc1shuang"]) - Convert.ToDouble(xml.dss["Hc1fudong"]);//双的赔率减少
                            else
                                xml.dss["Hc1shuang"] = xml.dss["Hc1odds8"];
                        }
                        else
                        {
                            xml.dss["Hc1dan"] = Convert.ToDouble(xml.dss["Hc1dan"]) + Convert.ToDouble(xml.dss["Hc1fudong"]);//单的赔率增加
                            xml.dss["Hc1shuang"] = Convert.ToDouble(xml.dss["Hc1shuang"]) - Convert.ToDouble(xml.dss["Hc1fudong"]);//双的赔率减少
                        }
                        System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                    }
                    else if (Cents2 == "双" && Cents4 == "双")//双
                    {
                        if (Convert.ToDouble(xml.dss["Hc1shuang"]) >= Convert.ToDouble(xml.dss["Hc1overpeilv"]))
                        {
                            xml.dss["Hc1dan"] = Convert.ToDouble(xml.dss["Hc1dan"]) - Convert.ToDouble(xml.dss["Hc1fudong"]);//单的赔率减少
                        }
                        else
                        {
                            xml.dss["Hc1dan"] = Convert.ToDouble(xml.dss["Hc1dan"]) - Convert.ToDouble(xml.dss["Hc1fudong"]);//单的赔率减少
                            xml.dss["Hc1shuang"] = Convert.ToDouble(xml.dss["Hc1shuang"]) + Convert.ToDouble(xml.dss["Hc1fudong"]);//双的赔率增加
                        }
                        System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                    }
                }
                if (Cents6 != Cents8)//如果连续2期不相等，还原赔率----尾数单双
                {
                    xml.dss["Hc1wsdan"] = xml.dss["Hc1odds12"];
                    xml.dss["Hc1wsshuang"] = xml.dss["Hc1odds13"];
                    System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                }
                else
                {
                    //1大2小、1单2双
                    if (Cents2 == "单" && Cents4 == "单")//如果连续2期开单
                    {
                        if (Convert.ToDouble(xml.dss["Hc1wsdan"]) >= Convert.ToDouble(xml.dss["Hc1overpeilv"]))
                        {
                            xml.dss["Hc1wsshuang"] = Convert.ToDouble(xml.dss["Hc1wsshuang"]) - Convert.ToDouble(xml.dss["Hc1fudong"]);//双的赔率减少
                        }
                        else
                        {
                            xml.dss["Hc1wsdan"] = Convert.ToDouble(xml.dss["Hc1wsdan"]) + Convert.ToDouble(xml.dss["Hc1fudong"]);//单的赔率增加
                            xml.dss["Hc1wsshuang"] = Convert.ToDouble(xml.dss["Hc1wsshuang"]) - Convert.ToDouble(xml.dss["Hc1fudong"]);//双的赔率减少
                        }
                        System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                    }
                    else if (Cents2 == "双" && Cents4 == "双")//双
                    {
                        if (Convert.ToDouble(xml.dss["Hc1wsshuang"]) >= Convert.ToDouble(xml.dss["Hc1overpeilv"]))
                        {
                            xml.dss["Hc1wsdan"] = Convert.ToDouble(xml.dss["Hc1wsdan"]) - Convert.ToDouble(xml.dss["Hc1fudong"]);//单的赔率减少
                        }
                        else
                        {
                            xml.dss["Hc1wsdan"] = Convert.ToDouble(xml.dss["Hc1wsdan"]) - Convert.ToDouble(xml.dss["Hc1fudong"]);//单的赔率减少
                            xml.dss["Hc1wsshuang"] = Convert.ToDouble(xml.dss["Hc1wsshuang"]) + Convert.ToDouble(xml.dss["Hc1fudong"]);//双的赔率增加
                        }
                        System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                    }
                }
            }
        }
        //如果最后一期没有开奖期号和开奖号码，则大小单双的赔率重置
        else
        {
            xml.dss["Hc1da"] = xml.dss["Hc1odds5"];
            xml.dss["Hc1xiao"] = xml.dss["Hc1odds6"];
            xml.dss["Hc1dan"] = xml.dss["Hc1odds7"];
            xml.dss["Hc1shuang"] = xml.dss["Hc1odds8"];
            xml.dss["Hc1wsda"] = xml.dss["Hc1odds10"];
            xml.dss["Hc1wsxiao"] = xml.dss["Hc1odds11"];
            xml.dss["Hc1wsdan"] = xml.dss["Hc1odds12"];
            xml.dss["Hc1wsshuang"] = xml.dss["Hc1odds13"];
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
        }
        #endregion
    }
    //大小单双和尾数单双赔率浮动
    public void change_peilv1()
    {
        BCW.Model.Game.HcList model_2 = new BCW.BLL.Game.HcList().GetHcListNew(1);//开奖的的最新数据                                                                     
        string qihao = Convert.ToString(model_2.CID);
        string kai = model_2.Result;//开奖结果
                                    // Response.Write("开奖数据：" + model_2.CID + "<br />");
        ub xml = new ub();
        string xmlPath_update = "/Controls/hc1.xml";
        Application.Remove(xmlPath_update);//清缓存
        xml.ReloadSub(xmlPath_update); //加载配置
        #region 期号获取赔率浮动   
        if (kai != "" && qihao != "")
        {
            string issue3 = Utils.Right(model_2.CID.ToString(), 3);//本期开奖期号的后3位
            if (issue3 != "001")
            {
                DataSet d1, d2;
                //获取最后一期期号
                d1 = new BCW.BLL.Game.HcList().GetList("TOP 1 *", "CID!='' and State=1 ORDER BY ID DESC");
                //获取倒数第二期期号
                d2 = new BCW.BLL.Game.HcList().GetList1("TOP 1 *", "id!='' and State=1 ORDER BY id ASC ");
                OutDX(Convert.ToInt32(model_2.Result));//最后一期大小
                OutDS(Convert.ToInt32(model_2.Result));//最后一期单双
                OutwsDX(Convert.ToInt32(model_2.Result));//最后一期尾数大小
                OutwsDS(Convert.ToInt32(model_2.Result));//最后一期尾数单双

                //  Response.Write("倒数第二期期号:" + "<b>" + OutDX(Convert.ToInt32(model_2.Result)) + "</b><br />");
                string Cents1 = "";
                string Cents2 = "";
                string Cents3 = "";
                string Cents4 = "";
                string Cents5 = "";
                string Cents6 = "";
                string Cents7 = "";
                string Cents8 = "";
                Cents1 = OutDX(Convert.ToInt32(model_2.Result));//最后一期的大小
                Cents2 = OutDS(Convert.ToInt32(model_2.Result));//最后一期的单双
                Cents5 = OutwsDX(Convert.ToInt32(model_2.Result));//最后一期尾数大小              
                Cents6 = OutwsDS(Convert.ToInt32(model_2.Result));//最后一期尾数单双

                //Response.Write("最后一期大小:" + "<b>" + Cents1 + "</b><br />");
                //    Response.Write("最后一期单双:" + "<b>" + Cents2 + "</b><br />");
                //    Response.Write("最后一期尾数大小:" + "<b>" + Cents5 + "</b><br />");
                //    Response.Write("最后一期尾数单双:" + "<b>" + Cents6 + "</b><br />");
                for (int i = 0; i < d2.Tables[0].Rows.Count; i++)
                {
                    try
                    {
                        Cents3 = OutDX(Convert.ToInt32(d2.Tables[0].Rows[i][2]));//倒数第2期的大小
                        Cents4 = OutDS(Convert.ToInt32(d2.Tables[0].Rows[i][2]));//倒数第2期的单双
                        Cents7 = OutwsDX(Convert.ToInt32(d2.Tables[0].Rows[i][2]));//倒数第2期尾数的大小
                        Cents8 = OutwsDS(Convert.ToInt32(d2.Tables[0].Rows[i][2]));//倒数第2期尾数的单双
                    }
                    catch
                    {
                        Cents3 = "";
                        Cents4 = "";
                        Cents7 = "";
                        Cents8 = "";
                    }
                }
                //Response.Write("倒数第二期大小:" + "<b>" + Cents3 + "</b><br />");
                //Response.Write("倒数第二期单双:" + "<b>" + Cents4 + "</b><br />");
                //Response.Write("倒数第二期尾数大小:" + "<b>" + Cents7 + "</b><br />");
                //Response.Write("倒数第二期尾数单双:" + "<b>" + Cents8 + "</b><br />");
                if (Cents1 != Cents3)//如果连续2期不相等，还原赔率----大小
                {
                    xml.dss["Hc1da"] = xml.dss["Hc1odds5"];//da
                    xml.dss["Hc1xiao"] = xml.dss["Hc1odds6"];//xiao
                    System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                }
                else
                {
                    //1大2小、1单2双
                    if (Cents1 == "小" && Cents3 == "小")//如果连续2期开小
                    {
                        if (Convert.ToDouble(xml.dss["Hc1xiao"]) >= Convert.ToDouble(xml.dss["Hc1overpeilv"]))//如果增加到一定的赔率，则不再增加。
                        {

                            xml.dss["Hc1da"] = Convert.ToDouble(xml.dss["Hc1da"]) - Convert.ToDouble(xml.dss["Hc1fudong"]);//大的赔率减少
                        }
                        else
                        {
                            xml.dss["Hc1xiao"] = Convert.ToDouble(xml.dss["Hc1xiao"]) + Convert.ToDouble(xml.dss["Hc1fudong"]);//小的赔率增加
                            xml.dss["Hc1da"] = Convert.ToDouble(xml.dss["Hc1da"]) - Convert.ToDouble(xml.dss["Hc1fudong"]);//大的赔率减少
                        }
                        System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                    }
                    else if (Cents1 == "大" && Cents3 == "大")//大
                    {
                        if (Convert.ToDouble(xml.dss["Hc1da"]) >= Convert.ToDouble(xml.dss["Hc1overpeilv"]))
                        {
                            xml.dss["Hc1xiao"] = Convert.ToDouble(xml.dss["Hc1xiao"]) - Convert.ToDouble(xml.dss["Hc1fudong"]);//小的赔率减少        
                        }
                        else
                        {
                            xml.dss["Hc1xiao"] = Convert.ToDouble(xml.dss["Hc1xiao"]) - Convert.ToDouble(xml.dss["Hc1fudong"]);//小的赔率减少
                            xml.dss["Hc1da"] = Convert.ToDouble(xml.dss["Hc1da"]) + Convert.ToDouble(xml.dss["Hc1fudong"]);//大的赔率增加
                        }
                        System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                    }
                }
                if (Cents5 != Cents7)//如果连续2期不相等，还原赔率----尾数大小
                {
                    xml.dss["Hc1wsda"] = xml.dss["Hc1odds10"];//da
                    xml.dss["Hc1wsxiao"] = xml.dss["Hc1odds11"];//xiao
                    System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                }
                else
                {

                    if (Cents5 == "小" && Cents7 == "小")//如果连续2期开小
                    {
                        if (Convert.ToDouble(xml.dss["Hc1wsxiao"]) >= Convert.ToDouble(xml.dss["Hc1overpeilv"]))//如果增加到一定的赔率，则不再增加。
                        {

                            xml.dss["Hc1wsda"] = Convert.ToDouble(xml.dss["Hc1wsda"]) - Convert.ToDouble(xml.dss["Hc1fudong"]);//大的赔率减少

                        }
                        else
                        {
                            xml.dss["Hc1wsxiao"] = Convert.ToDouble(xml.dss["Hc1wsxiao"]) + Convert.ToDouble(xml.dss["Hc1fudong"]);//小的赔率增加
                            xml.dss["Hc1wsda"] = Convert.ToDouble(xml.dss["Hc1wsda"]) - Convert.ToDouble(xml.dss["Hc1fudong"]);//大的赔率减少
                        }
                        System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                    }
                    else if (Cents5 == "大" && Cents7 == "大")//大
                    {
                        if (Convert.ToDouble(xml.dss["Hc1wsda"]) >= Convert.ToDouble(xml.dss["Hc1overpeilv"]))
                        {

                            xml.dss["Hc1wsxiao"] = Convert.ToDouble(xml.dss["Hc1wsxiao"]) - Convert.ToDouble(xml.dss["Hc1fudong"]);//小的赔率减少

                        }
                        else
                        {
                            xml.dss["Hc1wsxiao"] = Convert.ToDouble(xml.dss["Hc1wsxiao"]) - Convert.ToDouble(xml.dss["Hc1fudong"]);//小的赔率减少
                            xml.dss["Hc1wsda"] = Convert.ToDouble(xml.dss["Hc1wsda"]) + Convert.ToDouble(xml.dss["Hc1fudong"]);//大的赔率增加
                        }
                        System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                    }
                }
                if (Cents2 != Cents4)//如果连续2期不相等，还原赔率----单双
                {
                    xml.dss["Hc1dan"] = xml.dss["Hc1odds7"];
                    xml.dss["Hc1shuang"] = xml.dss["Hc1odds8"];
                    System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                }
                else
                {
                    //1大2小、1单2双
                    if (Cents2 == "单" && Cents4 == "单")//如果连续2期开单
                    {
                        if (Convert.ToDouble(xml.dss["Hc1dan"]) >= Convert.ToDouble(xml.dss["Hc1overpeilv"]))
                        {
                            xml.dss["Hc1shuang"] = Convert.ToDouble(xml.dss["Hc1shuang"]) - Convert.ToDouble(xml.dss["Hc1fudong"]);//双的赔率减少                         
                        }
                        else
                        {
                            xml.dss["Hc1dan"] = Convert.ToDouble(xml.dss["Hc1dan"]) + Convert.ToDouble(xml.dss["Hc1fudong"]);//单的赔率增加
                            xml.dss["Hc1shuang"] = Convert.ToDouble(xml.dss["Hc1shuang"]) - Convert.ToDouble(xml.dss["Hc1fudong"]);//双的赔率减少
                        }
                        System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                    }
                    else if (Cents2 == "双" && Cents4 == "双")//双
                    {
                        if (Convert.ToDouble(xml.dss["Hc1shuang"]) >= Convert.ToDouble(xml.dss["Hc1overpeilv"]))
                        {
                            xml.dss["Hc1dan"] = Convert.ToDouble(xml.dss["Hc1dan"]) - Convert.ToDouble(xml.dss["Hc1fudong"]);//单的赔率减少
                        }
                        else
                        {
                            xml.dss["Hc1dan"] = Convert.ToDouble(xml.dss["Hc1dan"]) - Convert.ToDouble(xml.dss["Hc1fudong"]);//单的赔率减少
                            xml.dss["Hc1shuang"] = Convert.ToDouble(xml.dss["Hc1shuang"]) + Convert.ToDouble(xml.dss["Hc1fudong"]);//双的赔率增加
                        }
                        System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                    }
                }
                if (Cents6 != Cents8)//如果连续2期不相等，还原赔率----尾数单双
                {
                    xml.dss["Hc1wsdan"] = xml.dss["Hc1odds12"];
                    xml.dss["Hc1wsshuang"] = xml.dss["Hc1odds13"];
                    System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                }
                else
                {
                    //1大2小、1单2双
                    if (Cents2 == "单" && Cents4 == "单")//如果连续2期开单
                    {
                        if (Convert.ToDouble(xml.dss["Hc1wsdan"]) >= Convert.ToDouble(xml.dss["Hc1overpeilv"]))
                        {
                            xml.dss["Hc1wsshuang"] = Convert.ToDouble(xml.dss["Hc1wsshuang"]) - Convert.ToDouble(xml.dss["Hc1fudong"]);//双的赔率减少
                        }
                        else
                        {
                            xml.dss["Hc1wsdan"] = Convert.ToDouble(xml.dss["Hc1wsdan"]) + Convert.ToDouble(xml.dss["Hc1fudong"]);//单的赔率增加
                            xml.dss["Hc1wsshuang"] = Convert.ToDouble(xml.dss["Hc1wsshuang"]) - Convert.ToDouble(xml.dss["Hc1fudong"]);//双的赔率减少
                        }
                        System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                    }
                    else if (Cents2 == "双" && Cents4 == "双")//双
                    {
                        if (Convert.ToDouble(xml.dss["Hc1wsshuang"]) >= Convert.ToDouble(xml.dss["Hc1overpeilv"]))
                        {
                            xml.dss["Hc1wsdan"] = Convert.ToDouble(xml.dss["Hc1wsdan"]) - Convert.ToDouble(xml.dss["Hc1fudong"]);//单的赔率减少
                        }
                        else
                        {
                            xml.dss["Hc1wsdan"] = Convert.ToDouble(xml.dss["Hc1wsdan"]) - Convert.ToDouble(xml.dss["Hc1fudong"]);//单的赔率减少
                            xml.dss["Hc1wsshuang"] = Convert.ToDouble(xml.dss["Hc1wsshuang"]) + Convert.ToDouble(xml.dss["Hc1fudong"]);//双的赔率增加
                        }
                        System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                    }
                }
            }
        }
        //如果最后一期没有开奖期号和开奖号码，则大小单双的赔率重置
        else
        {
            xml.dss["Hc1da"] = xml.dss["Hc1odds5"];
            xml.dss["Hc1xiao"] = xml.dss["Hc1odds6"];
            xml.dss["Hc1dan"] = xml.dss["Hc1odds7"];
            xml.dss["Hc1shuang"] = xml.dss["Hc1odds8"];
            xml.dss["Hc1wsda"] = xml.dss["Hc1odds10"];
            xml.dss["Hc1wsxiao"] = xml.dss["Hc1odds11"];
            xml.dss["Hc1wsdan"] = xml.dss["Hc1odds12"];
            xml.dss["Hc1wsshuang"] = xml.dss["Hc1odds13"];
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
        }

        #endregion
    }
    private string ForType(int Types)
    {

        string TyName = string.Empty;
        if (Types == 1)
            TyName = "选号玩法";
        else if (Types == 2)
            TyName = "生肖玩法";
        else if (Types == 3)
            TyName = "方位玩法";
        else if (Types == 4)
            TyName = "四季玩法";
        else if (Types == 5)
            TyName = "大小单双";
        else if (Types == 6)
            TyName = "六肖中奖";
        else if (Types == 7)
            TyName = "尾数大小";
        else if (Types == 8)
            TyName = "尾数单双";
        else if (Types == 9)
            TyName = "家禽野兽";
        else if (Types == 10)
            TyName = "自选不中";

        return TyName;
    }
    //机器人兑奖
    public void RobotCase()
    {
        string RoBot = ub.GetSub("hc1ROBOTID", xmlPath);
        string RoBotCost = ub.GetSub("hc1ROBOTCOST", xmlPath);
        string[] RoBotID = RoBot.Split('#');
        MatchCollection leyr = Regex.Matches(RoBot, "#");
        int ele = leyr.Count + 1;
        for (int i = 0; i < ele; i++)
        {
            int meid = Convert.ToInt32(RoBotID[i]);
            DataSet model1 = new BCW.BLL.Game.HcPay().GetList("ID", "UsID=" + meid + "and IsSpier=1");
            for (int i2 = 0; i2 < model1.Tables[0].Rows.Count; i2++)
            {
                try
                {
                    int pid = Convert.ToInt32(model1.Tables[0].Rows[0][0]);
                    if (new BCW.BLL.Game.HcPay().ExistsState(pid, meid))
                    {
                        new BCW.BLL.Game.HcPay().UpdateState(pid);
                        //操作币
                        long winMoney = Convert.ToInt64(new BCW.BLL.Game.HcPay().GetWinCent(pid));
                        //税率
                        long SysTax = 0;
                        winMoney = winMoney - SysTax;
                        new BCW.BLL.User().UpdateiGold(meid, winMoney, "好彩一机器人兑奖-标识ID" + pid + "");
                        Response.Write("机器人自动兑奖!<br />");
                    }
                }
                catch
                {
                    Response.Write("机器人兑奖失败!<br />");
                }
            }
        }
    }


}