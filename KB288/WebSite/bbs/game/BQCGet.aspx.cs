using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text.RegularExpressions;
using System.Net;
using BCW.Common;
using System.Timers;

/// <summary>
/// 蒙宗将 20160924 规划上期无开奖，下期停滞
/// 蒙宗将 20161006 修复投入无回收
/// 20161007 蒙宗将 优化获取上期
/// 蒙宗将 20161112 优化开奖
/// </summary>

public partial class bbs_game_6CBGet : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/BQC.xml";
    protected string GameName = ub.GetSub("BQCName", "/Controls/BQC.xml");//游戏名字
    protected void Page_Load(object sender, EventArgs e)
    {
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();
        try
        {
            ZQPage();
        }
        catch
        {
            Response.Write("抓取失败,error1.<br/>");
        }

        CasePage();
        Response.Write("ok1" + "<br />");
        Response.Write("<b>6场半数据正常获取中！！</b><br />");
        Response.Write("<b>上次获取时间：</b>" + DateTime.Now + "<br />");
        stopwatch.Stop();
        Response.Write("<font color=\"black\">" + "总耗时:" + stopwatch.Elapsed.TotalSeconds + "秒");
    }
    //抓取页面
    public void ZQPage()
    {

        List<Matchs> list = TranList();

        list.Reverse();//集合反序

        BCW.BQC.Model.BQCList bqcList = new BCW.BQC.Model.BQCList();

        BCW.BQC.BLL.BQCList bqcListBll = new BCW.BQC.BLL.BQCList();


        foreach (Matchs a in list)
        {
            if (!String.IsNullOrEmpty(a.sale_start.ToString()))//判断开卖时间是否有空，数据接口有时会有期数但没有开卖时间
            {
                if (bqcListBll.Exists(a.phase))//如果存在该期数，更新该期的比赛得分
                {
                    string result = bqcListBll.FindResultByPhase(a.phase);

                    if (String.IsNullOrEmpty(result))//判断比赛总结果，如果总结果为空，证明该期的比赛还没有比完。
                    {
                        bqcList.CID = a.phase;
                        bqcList.Result = a.result;
                        bqcList.Score = a.score;
                        bqcList.Sale_StartTime = a.sale_start;
                        bqcList.EndTime = (DateTime)a.sale_end;

                        bqcList.Match = a.matchs;
                        bqcList.Team_Home = a.team_home;
                        bqcList.Team_Away = a.team_away;
                        bqcList.Start_time = a.start_time;


                        if (new BCW.BQC.BLL.BQCList().getState((a.phase - 1)) != 0)//上一场没有开奖不开下一场，
                        {
                            if (bqcListBll.UpdateMatchs(bqcList))//更新赛事结果跟比分
                            {

                                Response.Write("更新第" + a.phase + "期数据成功！ok1<br />");
                            }
                            else
                            {
                                Response.Write("更新第" + a.phase + "期数据失败！error1<br />");
                            }
                        }
                    }
                    else
                    {
                        if (bqcListBll.UpdateState(1, a.phase))
                        {
                            Response.Write("更新第" + a.phase + "期开奖状态成功！ok1<br />");
                        }
                        else
                        {
                            Response.Write("更新第" + a.phase + "期开奖状态失败！error1<br />");
                        }
                    }
                    //  Response.Write("更新" + a.phase + "数据成功ok1<br />");
                }

                else//插入新的比赛赛事
                {
                    int CID = a.phase;
                    bqcList.CID = a.phase;
                    bqcList.Result = a.result;
                    bqcList.Sale_StartTime = a.sale_start;
                    bqcList.EndTime = (DateTime)a.sale_end;
                    bqcList.Match = a.matchs;
                    bqcList.Team_Home = a.team_home;
                    bqcList.Team_Away = a.team_away;
                    bqcList.Score = a.score;
                    bqcList.Start_time = a.start_time;
                    bqcList.State = 0;
                    bqcList.PayCent = 0;
                    bqcList.PayCount = 0;
                    bqcList.nowprize = 0;
                    bqcList.other = "";
                    bqcList.nextprize = 0;
                    if (new BCW.BQC.DAL.BQCList().getState(CID - 1) == 0)//未开奖上一期
                    {
                        bqcList.sysprize = 0;
                        bqcList.sysprizestatue = 0;
                    }
                    else//上一期已经开奖
                    {
                        if (new BCW.BQC.DAL.BQCList().getnextprize(CID - 1) > Convert.ToInt64(ub.GetSub("BQCMin", "/Controls/BQC.xml")))
                        {
                            bqcList.sysprize = 0;
                            bqcList.sysprizestatue = 0;
                        }
                        else
                        {
                            bqcList.sysprize = Convert.ToInt64(ub.GetSub("BQCSend", "/Controls/BQC.xml"));
                            bqcList.sysprizestatue = 1;//表示系统有投入
                        }
                    }
                    bqcList.sysdayprize = 0;
                    new BCW.BQC.BLL.BQCList().Add(bqcList);

                    if (new BCW.BQC.DAL.BQCList().getState(CID - 1) == 1)
                    {
                        if (new BCW.BQC.DAL.BQCList().getnextprize(CID - 1) > Convert.ToInt64(ub.GetSub("BQCMin", "/Controls/BQC.xml")))
                        {
                            //把记录加到奖池表
                            BCW.BQC.Model.BQCJackpot mo = new BCW.BQC.Model.BQCJackpot();
                            mo.usID = 0;//1表示系统无投入
                            mo.WinPrize = 0;
                            mo.Prize = 0;
                            mo.other = "上期滚存" + new BCW.BQC.DAL.BQCList().getnextprize(CID - 1);
                            mo.allmoney = new BCW.BQC.DAL.BQCList().getnextprize(CID - 1);
                            mo.AddTime = DateTime.Now;
                            mo.CID = CID;
                            new BCW.BQC.BLL.BQCJackpot().Add(mo);
                        }
                        else
                        {
                            //把记录加到奖池表
                            BCW.BQC.Model.BQCJackpot mo = new BCW.BQC.Model.BQCJackpot();
                            mo.usID = 1;//1表示系统有投入
                            mo.WinPrize = 0;
                            mo.Prize = Convert.ToInt64(ub.GetSub("BQCSend", "/Controls/BQC.xml")) + new BCW.BQC.DAL.BQCList().getnextprize(CID - 1);
                            mo.other = "系统投入" + Convert.ToInt64(ub.GetSub("BQCSend", "/Controls/BQC.xml")) + "上期滚存" + new BCW.BQC.DAL.BQCList().getnextprize(CID - 1);
                            mo.allmoney = new BCW.BQC.DAL.BQCList().getnextprize(CID - 1) + Convert.ToInt64(ub.GetSub("BQCSend", "/Controls/BQC.xml"));
                            mo.AddTime = DateTime.Now;
                            mo.CID = CID;
                            new BCW.BQC.BLL.BQCJackpot().Add(mo);
                        }
                    }

                    Response.Write("获取第" + a.phase + "期数据成功！ok1<br>");

                }
            }

        }
        DataSet ds = new BCW.BQC.BLL.BQCList().GetList("CID", " State=1 Order by CID Desc ");
        try
        {
            if (new BCW.BQC.BLL.BQCList().getsysstate((Convert.ToInt32(ds.Tables[0].Rows[0][0]) + 1)) == 0)
            {
                if (ds.Tables[0].Rows.Count > 0 && ds != null)
                {
                    int CIDs = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                    int sta = new BCW.BQC.BLL.BQCList().getsysstate(CIDs);
                    if (sta == 0)
                    {
                        if ((CIDs + 1) > 0)
                        {
                            if (!new BCW.BQC.BLL.BQCJackpot().Exists4())
                            {
                                BCW.BQC.Model.BQCList sq = new BCW.BQC.Model.BQCList();
                                sq.sysprize = Convert.ToInt64(ub.GetSub("BQCMin", "/Controls/BQC.xml"));
                                sq.sysprizestatue = 3;//表示首期系统有投入
                                new BCW.BQC.BLL.BQCList().UpdateSysstaprize((CIDs + 1), sq.sysprizestatue, sq.sysprize);

                                //把记录加到奖池表
                                BCW.BQC.Model.BQCJackpot mo = new BCW.BQC.Model.BQCJackpot();
                                mo.usID = 8;//8表示系统首期投入
                                mo.WinPrize = 0;
                                mo.Prize = Convert.ToInt64(ub.GetSub("BQCMin", "/Controls/BQC.xml"));
                                mo.other = "系统首期(" + (CIDs + 1) + "期)投入" + Convert.ToInt64(ub.GetSub("BQCMin", "/Controls/BQC.xml")) + "|结余" + Convert.ToInt64(ub.GetSub("BQCMin", "/Controls/BQC.xml")) + ub.Get("SiteBz");
                                mo.allmoney = Convert.ToInt64(ub.GetSub("BQCMin", "/Controls/BQC.xml"));
                                mo.AddTime = DateTime.Now;
                                mo.CID = (CIDs + 1);
                                new BCW.BQC.BLL.BQCJackpot().Add(mo);
                            }
                        }
                    }
                }
            }
        }
        catch
        {

        }

    }
    //开奖页面
    public void CasePage()
    {
        #region 更新中奖

        #region 开奖结果
        //获取最新开奖结果
        DataSet qi = new BCW.BQC.BLL.BQCList().GetList("CID,Result", " Result!='" + null + "' Order by CID Desc ");
        int CID = 0; string Result = string.Empty;
        CID = Convert.ToInt32(qi.Tables[0].Rows[0][0]);//最新开奖的期号  
        Result = Convert.ToString(qi.Tables[0].Rows[0][1]);//最新开奖结果
        #endregion

        #region 判断中奖
        string[] resultnum = Result.Split(',');
        //遍历表BQCPay，更新中奖
        DataSet dspay = new BCW.BQC.BLL.BQCPay().GetList("CID,usID,vote,payCents,change,id", "CID=" + CID + " and State=0");
        if (dspay != null && dspay.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dspay.Tables[0].Rows.Count; i++)
            {
                //对比数据：对比两数组相同数据出现的次数
                int count = 0;
                //中奖标记,中奖注数
                int num1 = 0;
                int num2 = 0;

                int pid = int.Parse(dspay.Tables[0].Rows[i]["id"].ToString());
                int UsID = int.Parse(dspay.Tables[0].Rows[i]["UsID"].ToString());
                string Vote = dspay.Tables[0].Rows[i]["Vote"].ToString();
                long PayCents = Int64.Parse(dspay.Tables[0].Rows[i]["PayCents"].ToString());
                int change = int.Parse(dspay.Tables[0].Rows[i]["change"].ToString());
                bool IsWin = false;

                string[] buyvote = Vote.Split(',');//数据库存储的投注记录格式：(3-3/3-1),(3-3/1-3),(3-3),(3-3),(3-3),(3-3)
                string[] vote1 = buyvote[0].Replace("(", "").Replace(")", "").Split('/');//第一场比赛投注结果：胜胜3-3和胜平3-1
                string[] vote2 = buyvote[1].Replace("(", "").Replace(")", "").Split('/');
                string[] vote3 = buyvote[2].Replace("(", "").Replace(")", "").Split('/');
                string[] vote4 = buyvote[3].Replace("(", "").Replace(")", "").Split('/');
                string[] vote5 = buyvote[4].Replace("(", "").Replace(")", "").Split('/');
                string[] vote6 = buyvote[5].Replace("(", "").Replace(")", "").Split('/');

                List<string[]> list1 = new List<string[]>();
                list1.Add(vote1);
                list1.Add(vote2);
                list1.Add(vote3);
                list1.Add(vote4);
                list1.Add(vote5);
                list1.Add(vote6);
                string[] totalResult;
                //找到投注数据
                totalResult = bianli(list1);

                for (int iresult = 0; iresult < totalResult.Length; iresult++)
                {
                    string[] result = totalResult[iresult].Split(',');
                    for (int j = 0; j < result.Length; j++)
                    {
                        //遍历开奖数据是否相同，相同则count+1
                        if (result[j].Equals(resultnum[j]))
                            count++;
                        if (resultnum[j].Contains("*"))
                            count++;
                    }
                    //如果count出现的次数等于5，证明是二等奖
                    if (count == 5)
                    {
                        num2++;
                        IsWin = true;
                    }
                    //如果count出现的次数等于6，证明是一等奖
                    if (count == 6)
                    {
                        num1++;
                        IsWin = true;

                    }
                    count = 0;
                }

                if (IsWin == true)
                {
                    if (num2 > 0)
                        BCW.Data.SqlHelper.ExecuteSql("update tb_BQCPay set ISPrize=2 ,ISPrize2=1, Prize2Num=" + num2 + " Where id=" + pid + "");//ISPrize2=1代表中二等奖，Prize2Num代表中二等奖的注数
                    if (num1 > 0)
                        BCW.Data.SqlHelper.ExecuteSql("update tb_BQCPay set ISPrize=1 Where id=" + pid + "");//一等奖仅有一注

                }

            }
        }
        #endregion

        #region 奖池更新 （回收）
        if (Result != null)
        {
            if (new BCW.BQC.BLL.BQCList().getState(CID) == 0)
            {
                //判断奖池，小于200万就投入，大于不投入
                if (AllPrize(CID) < Convert.ToInt64(ub.GetSub("BQCMin", "/Controls/BQC.xml")))
                {
                    if (new BCW.BQC.BLL.BQCList().getsysstate(CID) == 0)
                    {
                        long all = AllPrize(CID) + Convert.ToInt64(ub.GetSub("BQCMin", "/Controls/BQC.xml"));
                        //更新奖池
                        new BCW.BQC.BLL.BQCList().updateNowprize(all, CID);
                        new BCW.BQC.BLL.BQCList().UpdateSysstaprize(CID, 3, Convert.ToInt64(ub.GetSub("BQCMin", "/Controls/BQC.xml")));

                        //把记录加到奖池表
                        BCW.BQC.Model.BQCJackpot mo = new BCW.BQC.Model.BQCJackpot();
                        mo.usID = 8;//8表示系统首期有投入
                        mo.WinPrize = 0;
                        mo.Prize = Convert.ToInt64(ub.GetSub("BQCMin", "/Controls/BQC.xml"));
                        mo.other = "系统首期(" + CID + "期)投入" + Convert.ToInt64(ub.GetSub("BQCMin", "/Controls/BQC.xml")) + "|结余" + Convert.ToInt64(ub.GetSub("BQCMin", "/Controls/BQC.xml")) + ub.Get("SiteBz");
                        mo.allmoney = all;
                        mo.AddTime = DateTime.Now.AddMilliseconds(-10);
                        mo.CID = CID;
                        new BCW.BQC.BLL.BQCJackpot().Add(mo);
                    }
                    else
                    {
                        //更新奖池
                        new BCW.BQC.BLL.BQCList().updateNowprize(AllPrize(CID), CID);
                    }

                }
                else if (AllPrize(CID) > Convert.ToInt64(ub.GetSub("BQCMin", "/Controls/BQC.xml")) && AllPrize(CID) < Convert.ToInt64(ub.GetSub("BQCMax", "/Controls/BQC.xml")))
                {
                    //更新奖池
                    new BCW.BQC.BLL.BQCList().updateNowprize(AllPrize(CID), CID);
                }
                //判断大于400万，有投入回收，无投入不回收
                //if (AllPrize(model.CID) > Convert.ToInt64(ub.GetSub("BQCMax", "/Controls/BQC.xml")))
                else
                {
                    //如果之前期数有投入为回收就回收，无则不做操作
                    if (new BCW.BQC.BLL.BQCList().Existsysprize(CID))
                    {
                        //未开奖当前投注期号
                        DataSet ds = new BCW.BQC.BLL.BQCList().GetList("TOP 1 CID", "State=1 and sysprizestatue=1 order by CID ASC");
                        int CIDh = 0;
                        CIDh = int.Parse(ds.Tables[0].Rows[0][0].ToString());
                        new BCW.BQC.BLL.BQCList().UpdateSysstaprize(CID, 2, Convert.ToInt64(ub.GetSub("BQCSend", "/Controls/BQC.xml")));//当期回收
                        long all = AllPrize(CID) - Convert.ToInt64(ub.GetSub("BQCSend", "/Controls/BQC.xml"));
                        //更新奖池
                        new BCW.BQC.BLL.BQCList().updateNowprize(all, CID);

                        //把记录加到奖池表
                        BCW.BQC.Model.BQCJackpot mo = new BCW.BQC.Model.BQCJackpot();
                        mo.usID = 7;//7表示系统有回收
                        mo.WinPrize = Convert.ToInt64(ub.GetSub("BQCSend", "/Controls/BQC.xml"));
                        mo.Prize = 0;
                        mo.other = "系统回收" + Convert.ToInt64(ub.GetSub("BQCSend", "/Controls/BQC.xml"));
                        mo.allmoney = all;
                        mo.AddTime = DateTime.Now.AddMilliseconds(-500);
                        mo.CID = CID;
                        new BCW.BQC.BLL.BQCJackpot().Add(mo);

                        if (CID != CIDh)
                        {
                            new BCW.BQC.BLL.BQCList().UpdateSysprizestatue(CIDh, 3);//表示被回收
                        }
                    }
                    else
                    {
                        //更新奖池
                        new BCW.BQC.BLL.BQCList().updateNowprize(AllPrize(CID), CID);
                    }
                }
            }
        }
        #endregion

        #region 奖池手续更新
        long Allmoney = new BCW.BQC.BLL.BQCList().nowprize(CID);
        //判断该期数有无中奖
        if (!new BCW.BQC.BLL.BQCPay().Exists(CID, 0))//无中奖
        {
            #region 无中奖奖池更新

            if (!new BCW.BQC.BLL.BQCList().ExistsSysprize(CID))//如果不存在系统投注
            {
                if (!new BCW.BQC.BLL.BQCJackpot().Exists3(CID))//开奖当前还没有数据
                {
                    BCW.BQC.Model.BQCJackpot mo = new BCW.BQC.Model.BQCJackpot();
                    mo.usID = 0;
                    mo.WinPrize = Convert.ToInt64(Allmoney * Convert.ToInt64(ub.GetSub("BQCsys", "/Controls/BQC.xml")) * 0.01);//系统收回%10
                    mo.Prize = 0;
                    long a = Convert.ToInt64(Allmoney * Convert.ToInt64(ub.GetSub("BQCsys", "/Controls/BQC.xml")) * 0.01);
                    long b = Convert.ToInt64(Allmoney) - a;
                    mo.other = "系统收取手续" + a;
                    mo.allmoney = b;
                    mo.AddTime = DateTime.Now;
                    mo.CID = CID;
                    new BCW.BQC.BLL.BQCJackpot().Add(mo);
                }

            }
            else//如果存在系统投注
            {

                if (Allmoney > Convert.ToInt64(ub.GetSub("BQCMax", "/Controls/BQC.xml")))//最大奖池数
                {

                    if (!new BCW.BQC.BLL.BQCJackpot().Exists3(CID))
                    {
                        BCW.BQC.Model.BQCJackpot mo = new BCW.BQC.Model.BQCJackpot();
                        mo.usID = 2;//表示回收投入的金额
                        mo.WinPrize = Convert.ToInt64(Allmoney * Convert.ToInt64(ub.GetSub("BQCsys", "/Controls/BQC.xml")) * 0.01);
                        mo.Prize = 0;
                        long a = Convert.ToInt64(Allmoney * Convert.ToInt64(ub.GetSub("BQCsys", "/Controls/BQC.xml")) * 0.01) + Convert.ToInt64(ub.GetSub("BQCMin", "/Controls/BQC.xml"));
                        long b = Convert.ToInt64(Allmoney) - Convert.ToInt64(Allmoney * Convert.ToInt64(ub.GetSub("BQCsys", "/Controls/BQC.xml")) * 0.01);
                        mo.other = "系统收取手续" + Convert.ToInt64(Allmoney * Convert.ToInt64(ub.GetSub("BQCsys", "/Controls/BQC.xml")) * 0.01);
                        mo.allmoney = b;
                        mo.AddTime = DateTime.Now;
                        mo.CID = CID;

                        new BCW.BQC.BLL.BQCJackpot().Add(mo);
                    }

                }
                else//奖池与最小奖池数
                {
                    if (!new BCW.BQC.BLL.BQCJackpot().Exists3(CID))
                    {
                        BCW.BQC.Model.BQCJackpot mo = new BCW.BQC.Model.BQCJackpot();
                        mo.usID = 2;
                        mo.WinPrize = Convert.ToInt64(Allmoney * Convert.ToInt64(ub.GetSub("BQCsys", "/Controls/BQC.xml")) * 0.01);//系统收回%10
                        mo.Prize = 0;
                        long a = Convert.ToInt64(Allmoney * Convert.ToInt64(ub.GetSub("BQCsys", "/Controls/BQC.xml")) * 0.01);
                        long b = Convert.ToInt64(Allmoney) - a;
                        mo.other = "系统收取手续" + a;
                        mo.allmoney = b;
                        mo.AddTime = DateTime.Now;
                        mo.CID = CID;
                        new BCW.BQC.BLL.BQCJackpot().Add(mo);
                    }

                    //更新系统投注状态为3，表示小于最小额数不需要回收
                    // new BCW.BQC.BLL.BQCList().UpdateSysprizestatue(CID, 3);
                }
            }
            #endregion
        }
        else
        {
            #region 中奖奖池更新
            if (!new BCW.BQC.BLL.BQCList().ExistsSysprize(CID))//如果不存在系统投注
            {
                if (!new BCW.BQC.BLL.BQCJackpot().Exists3(CID))//开奖当前还没有数据
                {
                    BCW.BQC.Model.BQCJackpot mo = new BCW.BQC.Model.BQCJackpot();
                    mo.usID = 2;
                    mo.WinPrize = Convert.ToInt64(Allmoney * Convert.ToInt64(ub.GetSub("BQCsys", "/Controls/BQC.xml")) * 0.01);//系统收回%10
                    mo.Prize = 0;
                    long a = Convert.ToInt64(Allmoney * Convert.ToInt64(ub.GetSub("BQCsys", "/Controls/BQC.xml")) * 0.01);
                    long b = Convert.ToInt64(Allmoney) - a;
                    mo.other = "系统收取手续" + a;
                    mo.allmoney = b;
                    mo.AddTime = DateTime.Now;
                    mo.CID = CID;
                    new BCW.BQC.BLL.BQCJackpot().Add(mo);
                }
            }
            else//如果存在系统投注
            {
                if (Allmoney > Convert.ToInt64(ub.GetSub("BQCMax", "/Controls/BQC.xml")))//最大奖池数
                {

                    if (!new BCW.BQC.BLL.BQCJackpot().Exists3(CID))
                    {
                        BCW.BQC.Model.BQCJackpot mo = new BCW.BQC.Model.BQCJackpot();
                        mo.usID = 2;//表示回收投入的金额
                        mo.WinPrize = Convert.ToInt64(Allmoney * Convert.ToInt64(ub.GetSub("BQCsys", "/Controls/BQC.xml")) * 0.01);
                        mo.Prize = 0;
                        long a = Convert.ToInt64(Allmoney * Convert.ToInt64(ub.GetSub("BQCsys", "/Controls/BQC.xml")) * 0.01) + Convert.ToInt64(ub.GetSub("BQCMin", "/Controls/BQC.xml"));
                        long b = Convert.ToInt64(Allmoney) - Convert.ToInt64(Allmoney * Convert.ToInt64(ub.GetSub("BQCsys", "/Controls/BQC.xml")) * 0.01);
                        mo.other = "系统收取手续" + Convert.ToInt64(Allmoney * Convert.ToInt64(ub.GetSub("BQCsys", "/Controls/BQC.xml")) * 0.01);
                        mo.allmoney = b;
                        mo.AddTime = DateTime.Now;
                        mo.CID = CID;

                        new BCW.BQC.BLL.BQCJackpot().Add(mo);
                    }

                }
                else//奖池与最小奖池数
                {
                    if (!new BCW.BQC.BLL.BQCJackpot().Exists3(CID))
                    {
                        BCW.BQC.Model.BQCJackpot mo = new BCW.BQC.Model.BQCJackpot();
                        mo.usID = 2;
                        mo.WinPrize = Convert.ToInt64(Allmoney * Convert.ToInt64(ub.GetSub("BQCsys", "/Controls/BQC.xml")) * 0.01);//系统收回%10
                        mo.Prize = 0;
                        long a = Convert.ToInt64(Allmoney * Convert.ToInt64(ub.GetSub("BQCsys", "/Controls/BQC.xml")) * 0.01);
                        long b = Convert.ToInt64(Allmoney) - a;
                        mo.other = "系统收取手续" + a;
                        mo.allmoney = b;
                        mo.AddTime = DateTime.Now;
                        mo.CID = CID;
                        new BCW.BQC.BLL.BQCJackpot().Add(mo);
                    }

                    //更新系统投注状态为3，表示小于最小额数不需要回收
                    // new BCW.BQC.BLL.BQCList().UpdateSysprizestatue(CID, 3);
                }
            }
            #endregion
        }
        #endregion

        #region 派奖
        if (new BCW.BQC.DAL.BQCList().getState(CID) == 0)
        {
            DataSet pay = new BCW.BQC.BLL.BQCPay().GetList("usID,ISPrize,VoteNum,id", "CID=" + CID + " and State=0");
            if (dspay != null && dspay.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < pay.Tables[0].Rows.Count; i++)
                {
                    int pid = int.Parse(pay.Tables[0].Rows[i]["id"].ToString());
                    int ISPrize = int.Parse(pay.Tables[0].Rows[i]["ISPrize"].ToString());
                    int UsID = int.Parse(pay.Tables[0].Rows[i]["usID"].ToString());
                    int VoteNum = int.Parse(pay.Tables[0].Rows[i]["VoteNum"].ToString());
                    int overridebyid1 = new BCW.BQC.BLL.BQCPay().VoteNum1(pid, CID);
                    int overridebyid2 = new BCW.BQC.BLL.BQCPay().VoteNum2(pid, CID);
                    //得到当前奖池
                    long All = new BCW.BQC.BLL.BQCList().nowprize(CID);
                    long Now = NextPrize(CID);
                    if (VoteNum == 1)//单式派奖
                    {
                        //一等奖
                        if (new BCW.BQC.BLL.BQCPay().Exists1(pid, 1))
                        {
                            //注数
                            long zhu = new BCW.BQC.BLL.BQCPay().countPrize(CID, 1);
                            //费率
                            double lv = Convert.ToDouble(ub.GetSub("BQCOne", "/Controls/BQC.xml")) * 0.01;
                            double allr = Math.Round(Convert.ToDouble(All * lv / Convert.ToDouble(zhu)), 2);
                            long all = Convert.ToInt64(allr * overridebyid1);

                            //添加奖池数据
                            BCW.BQC.Model.BQCJackpot mo = new BCW.BQC.Model.BQCJackpot();
                            mo.usID = UsID;
                            mo.WinPrize = all;
                            mo.Prize = 0;
                            mo.other = "中一等奖" + Convert.ToString(all);
                            mo.allmoney = (Now - all);
                            mo.AddTime = DateTime.Now;
                            mo.CID = CID;
                            new BCW.BQC.BLL.BQCJackpot().Add(mo);

                            //动态
                            string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + new BCW.BLL.User().GetUsName(UsID) + "[/url]在第" + CID + "期[url=/bbs/game/BQC.aspx]" + GameName + "[/url]中一等奖" + all + "" + ub.Get("SiteBz") + "";
                            new BCW.BLL.Action().Add(1017, pid, UsID, "", wText);

                            BCW.Data.SqlHelper.ExecuteSql("update tb_BQCPay set WinCent=(( select WinCent from tb_BQCPay where id=" + pid + ")+" + all + ") Where id=" + pid + "");
                            //发送内线
                            new BCW.BLL.Guest().Add(1, UsID, "", "恭喜您在第" + CID + "期" + "[URL=/bbs/game/BQC.aspx]" + GameName + "[/URL]" + "投注，中一等奖奖" + all + "" + ub.Get("SiteBz") + "，开奖为" + Result + "[URL=/bbs/game/BQC.aspx?act=case]马上兑奖[/URL]");
                        }
                        //二等奖
                        if (new BCW.BQC.BLL.BQCPay().Exists2(pid, 2))
                        {
                            //注数
                            int zhu = new BCW.BQC.BLL.BQCPay().countPrize2(CID);
                            //费率
                            double lv = Convert.ToDouble(ub.GetSub("BQCTwo", "/Controls/BQC.xml")) * 0.01;
                            double allr = Math.Round(Convert.ToDouble(All * lv / Convert.ToDouble(zhu)), 2);
                            long all = Convert.ToInt64(allr * overridebyid2);
                            //添加奖池数据
                            BCW.BQC.Model.BQCJackpot mo = new BCW.BQC.Model.BQCJackpot();
                            mo.usID = UsID;
                            mo.WinPrize = all;
                            mo.Prize = 0;
                            mo.other = "中二等奖" + Convert.ToString(all);
                            mo.allmoney = (Now - all);
                            mo.AddTime = DateTime.Now;
                            mo.CID = CID;
                            new BCW.BQC.BLL.BQCJackpot().Add(mo);
                            BCW.Data.SqlHelper.ExecuteSql("update tb_BQCPay set WinCent=(( select WinCent from tb_BQCPay where id=" + pid + ")+" + all + ") Where id=" + pid + "");

                            //动态
                            string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + new BCW.BLL.User().GetUsName(UsID) + "[/url]在第" + CID + "期[url=/bbs/game/BQC.aspx]" + GameName + "[/url]中二等奖" + all + "" + ub.Get("SiteBz") + "";
                            new BCW.BLL.Action().Add(1017, pid, UsID, "", wText);

                            //发送内线
                            new BCW.BLL.Guest().Add(1, UsID, "", "恭喜您在第" + CID + "期" + "[URL=/bbs/game/BQC.aspx]" + GameName + "[/URL]" + "投注，中二等奖奖" + all + "" + ub.Get("SiteBz") + "，开奖为" + Result + "[URL=/bbs/game/BQC.aspx?act=case]马上兑奖[/URL]");
                        }
                    }
                    else//复式派奖
                    {
                        //一等奖
                        if (new BCW.BQC.BLL.BQCPay().Exists1(pid, 1))
                        {
                            //注数
                            long zhu = new BCW.BQC.BLL.BQCPay().countPrize(CID, 1);
                            //费率
                            double lv = Convert.ToDouble(ub.GetSub("BQCOne", "/Controls/BQC.xml")) * 0.01;
                            double allr = Math.Round(Convert.ToDouble(All * lv / Convert.ToDouble(zhu)), 2);
                            long all = Convert.ToInt64(allr * overridebyid1);

                            //添加奖池数据
                            BCW.BQC.Model.BQCJackpot mo = new BCW.BQC.Model.BQCJackpot();
                            mo.usID = UsID;
                            mo.WinPrize = all;
                            mo.Prize = 0;
                            mo.other = "中一等奖" + Convert.ToString(all);
                            mo.allmoney = (Now - all);
                            mo.AddTime = DateTime.Now;
                            mo.CID = CID;
                            new BCW.BQC.BLL.BQCJackpot().Add(mo);

                            //动态
                            string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + new BCW.BLL.User().GetUsName(UsID) + "[/url]在第" + CID + "期[url=/bbs/game/BQC.aspx]" + GameName + "[/url]中一等奖" + all + "" + ub.Get("SiteBz") + "";
                            new BCW.BLL.Action().Add(1017, pid, UsID, "", wText);

                            BCW.Data.SqlHelper.ExecuteSql("update tb_BQCPay set WinCent=(( select WinCent from tb_BQCPay where id=" + pid + ")+" + all + ") Where id=" + pid + "");
                            //发送内线
                            new BCW.BLL.Guest().Add(1, UsID, "", "恭喜您在第" + CID + "期" + "[URL=/bbs/game/BQC.aspx]" + GameName + "[/URL]" + "投注，中一等奖奖" + all + "" + ub.Get("SiteBz") + "，开奖为" + Result + "[URL=/bbs/game/BQC.aspx?act=case]马上兑奖[/URL]");
                        }
                        //二等奖
                        if (new BCW.BQC.BLL.BQCPay().Exists2(pid, 2))
                        {
                            //注数
                            int zhu = new BCW.BQC.BLL.BQCPay().countPrize2(CID);
                            //费率
                            double lv = Convert.ToDouble(ub.GetSub("BQCTwo", "/Controls/BQC.xml")) * 0.01;
                            double allr = Math.Round(Convert.ToDouble(All * lv / Convert.ToDouble(zhu)), 2);
                            long all = Convert.ToInt64(allr * overridebyid2);
                            //添加奖池数据
                            BCW.BQC.Model.BQCJackpot mo = new BCW.BQC.Model.BQCJackpot();
                            mo.usID = UsID;
                            mo.WinPrize = all;
                            mo.Prize = 0;
                            mo.other = "中二等奖" + Convert.ToString(all);
                            mo.allmoney = (new BCW.BQC.BLL.BQCJackpot().Getallmoney(CID) - all);
                            mo.AddTime = DateTime.Now;
                            mo.CID = CID;
                            new BCW.BQC.BLL.BQCJackpot().Add(mo);
                            BCW.Data.SqlHelper.ExecuteSql("update tb_BQCPay set WinCent=(( select WinCent from tb_BQCPay where id=" + pid + ")+" + all + ") Where id=" + pid + "");

                            //动态
                            string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + new BCW.BLL.User().GetUsName(UsID) + "[/url]在第" + CID + "期[url=/bbs/game/BQC.aspx]" + GameName + "[/url]中二等奖" + all + "" + ub.Get("SiteBz") + "";
                            new BCW.BLL.Action().Add(1017, pid, UsID, "", wText);

                            //发送内线
                            new BCW.BLL.Guest().Add(1, UsID, "", "恭喜您在第" + CID + "期" + "[URL=/bbs/game/BQC.aspx]" + GameName + "[/URL]" + "投注，中二等奖奖" + all + "" + ub.Get("SiteBz") + "，开奖为" + Result + "[URL=/bbs/game/BQC.aspx?act=case]马上兑奖[/URL]");
                        }
                    }
                }
            }
        }
        #endregion

        #region 更新奖池结余，更新系统收取手续
        if (new BCW.BQC.DAL.BQCList().getState(CID) == 0)
        {
            //更新当期系统结余奖池
            new BCW.BQC.DAL.BQCList().UpdateNextprize(CID, NextPrize(CID));
            //更新当期系统收取手续
            new BCW.BQC.DAL.BQCList().Updatesysdayprize(CID, Convert.ToInt64(Allmoney * Convert.ToInt64(ub.GetSub("BQCsys", "/Controls/BQC.xml")) * 0.01));
        }
        #endregion

        #region 奖池滚存
        int newcid = 0;
        if (new BCW.BQC.DAL.BQCList().ExistsCID(CID + 1))
        {
            newcid = (CID + 1);
        }
        else
        {
            newcid = FirstNewCID();
        }
        if (!new BCW.BQC.DAL.BQCJackpot().Existsgun(CID))
        {
            //把奖池滚存记录下来
            BCW.BQC.Model.BQCJackpot mos = new BCW.BQC.Model.BQCJackpot();
            mos.usID = 5;
            mos.WinPrize = 0;
            mos.Prize = 0;
            mos.other = "第" + CID + "期滚存" + NextPrize(CID) + ub.Get("SiteBz") + "到" + newcid + "期|结余0" + ub.Get("SiteBz");
            mos.allmoney = 0;
            mos.AddTime = DateTime.Now;
            mos.CID = CID;
            new BCW.BQC.BLL.BQCJackpot().Add(mos);
        }
        if (!new BCW.BQC.DAL.BQCJackpot().Existsgun1(newcid))
        {
            //把奖池滚存记录下来
            BCW.BQC.Model.BQCJackpot mos = new BCW.BQC.Model.BQCJackpot();
            mos.usID = 6;
            mos.WinPrize = 0;
            mos.Prize = 0;
            mos.other = "得到第" + CID + "期滚存" + NextPrize(CID) + ub.Get("SiteBz") + "|结余" + NextPrize(CID) + "" + ub.Get("SiteBz");
            mos.allmoney = 0;
            mos.AddTime = Convert.ToDateTime("2000-10-10 10:10:10");
            mos.CID = (newcid);
            new BCW.BQC.BLL.BQCJackpot().Add(mos);
        }
        #endregion

        #region 遍历奖池表 （BQCJackpot）,更新预售期的奖池
        //遍历表BQCJackpot，更新预售期的奖池
        if (new BCW.BQC.DAL.BQCList().ExistsCID((newcid)))
        {
            DataSet nextPP = new BCW.BQC.BLL.BQCJackpot().GetList("id,usID,Prize,WinPrize,other,allmoney,AddTime,CID", "CID=" + (newcid) + " ");

            if (nextPP != null && nextPP.Tables[0].Rows.Count > 0)
            {

                for (int i = 0; i < nextPP.Tables[0].Rows.Count; i++)
                {
                    int pid = int.Parse(nextPP.Tables[0].Rows[i]["id"].ToString());
                    int UsID = int.Parse(nextPP.Tables[0].Rows[i]["usID"].ToString());
                    long Prize = Int64.Parse(nextPP.Tables[0].Rows[i]["Prize"].ToString());
                    long WinPrize = Int64.Parse(nextPP.Tables[0].Rows[i]["WinPrize"].ToString());
                    string other = nextPP.Tables[0].Rows[i]["other"].ToString();
                    long allmoney = Int64.Parse(nextPP.Tables[0].Rows[i]["allmoney"].ToString());
                    DateTime AddTime = DateTime.Parse(nextPP.Tables[0].Rows[i]["AddTime"].ToString());
                    int CID1 = int.Parse(nextPP.Tables[0].Rows[i]["CID"].ToString());

                    long PP = 0;
                    if (new BCW.BQC.BLL.BQCList().getsysstate((newcid)) == 1)
                    {
                        PP = (allmoney + NextPrize(CID) + Convert.ToInt64(ub.GetSub("BQCMin", "/Controls/BQC.xml")));
                    }
                    else
                    {
                        PP = (allmoney + NextPrize(CID));
                    }
                    //更新奖池
                    if (other.Contains("预售"))
                    {
                        BCW.Data.SqlHelper.ExecuteSql("update tb_BQCJackpot set allmoney='" + PP + "' Where id='" + pid + "' ");
                        BCW.Data.SqlHelper.ExecuteSql("update tb_BQCJackpot set other=replace(other,'预售','') where id='" + pid + "' ");
                    }
                }

            }
            else
            {

            }
        }
        #endregion

        #region 系统投入
        if (new BCW.BQC.DAL.BQCList().getState(CID) == 0)
        {
            if (new BCW.BQC.BLL.BQCList().Exists((newcid)))
            {
                long paycent = new BCW.BQC.BLL.BQCList().GetPrice("sum(PayCent)", " CID =" + (newcid) + " ");//消费
                if ((NextPrize(CID) + paycent) < Convert.ToInt64(ub.GetSub("BQCMin", "/Controls/BQC.xml")))
                {
                    new BCW.BQC.BLL.BQCList().UpdateSysstaprize((newcid), 1, Convert.ToInt64(ub.GetSub("BQCSend", "/Controls/BQC.xml")));

                    //把记录加到奖池表
                    BCW.BQC.Model.BQCJackpot mo = new BCW.BQC.Model.BQCJackpot();
                    mo.usID = 1;//1表示系统有投入
                    mo.WinPrize = 0;
                    mo.Prize = Convert.ToInt64(ub.GetSub("BQCSend", "/Controls/BQC.xml"));
                    mo.other = "系统投入" + Convert.ToInt64(ub.GetSub("BQCSend", "/Controls/BQC.xml"));
                    mo.allmoney = (Convert.ToInt64(ub.GetSub("BQCSend", "/Controls/BQC.xml")) + NextPrize(CID));
                    mo.AddTime = DateTime.Now;
                    mo.CID = (newcid);
                    new BCW.BQC.BLL.BQCJackpot().Add(mo);
                }
            }

        }
        #endregion

        #region 完成返彩后正式更新该期为结束
        if (new BCW.BQC.BLL.BQCList().getState(CID) == 0)
        {
            BCW.Data.SqlHelper.ExecuteSql("update tb_BQCList set State=1 Where CID=" + CID + "");
            BCW.Data.SqlHelper.ExecuteSql("update tb_BQCPay set State=1 Where CID=" + CID + "");
        }
        #endregion

        #endregion
    }
    /// <summary>
    /// 遍历数组，组合投注结果
    /// </summary>
    /// <param name="al"></param>
    /// <returns></returns>
    private string[] bianli(List<string[]> al)
    {
        if (al.Count == 0)
            return null;
        int size = 1;
        for (int i = 0; i < al.Count; i++)
        {
            size = size * al[i].Length;
        }
        string[] str = new string[size];
        for (long j = 0; j < size; j++)
        {
            for (int m = 0; m < al.Count; m++)
            {
                str[j] = str[j] + al[m][(j * jisuan(al, m) / size) % al[m].Length] + ",";
            }
            str[j] = str[j].Trim(',');
        }
        return str;
    }
    /// <summary>
    /// 计算当前产生的结果数
    /// </summary>
    /// <param name="al"></param>
    /// <param name="m"></param>
    /// <returns></returns>
    private int jisuan(List<string[]> al, int m)
    {
        int result = 1;
        for (int i = 0; i < al.Count; i++)
        {
            if (i <= m)
            {
                result = result * al[i].Length;
            }
            else
            {
                break;
            }
        }
        return result;
    }
    /// <summary>
    /// 将xml解析成list集合
    /// </summary>
    /// <returns>返回list集合</returns>
    public List<Matchs> TranList()
    {
        List<Matchs> list = new List<Matchs>();
        XmlDocument dom = GetXmlData();
        XmlNodeList nodelist = dom.SelectSingleNode("/xml").ChildNodes;

        foreach (XmlNode node in nodelist)//遍历xml节点
        {
            Matchs matchs = new Matchs();
            XmlElement xe = (XmlElement)node;//将子节点类型转换为xmlelement类型

            matchs.phase = int.Parse(xe.GetAttribute("phase"));
            string[] result = xe.GetAttribute("result").ToString().Split(',');
            string newresult = "";
            for (int i = 0; i < result.Length; i++)
            {
                if (i % 2 == 0)
                    newresult += result[i] + "-";
                else
                    newresult += result[i] + ",";
            }
            matchs.result = newresult.Substring(0, newresult.Length - 1);
            if (!String.IsNullOrEmpty(xe.GetAttribute("sale_start")) && !String.IsNullOrEmpty(xe.GetAttribute("sale_end")))
            {
                matchs.sale_start = DateTime.Parse(xe.GetAttribute("sale_start"));
                matchs.sale_end = DateTime.Parse(xe.GetAttribute("sale_end"));
            }
            else
            {
                matchs.sale_start = null;
                matchs.sale_end = null;
            }


            StringBuilder sbMatchs = new StringBuilder();
            StringBuilder sbTeamHome = new StringBuilder();
            StringBuilder sbTeamAway = new StringBuilder();
            StringBuilder sbStartTime = new StringBuilder();
            StringBuilder sbScore = new StringBuilder();

            foreach (XmlNode node2 in node)
            {
                foreach (XmlNode node3 in node2)
                {
                    XmlElement xe2 = (XmlElement)node3;
                    sbMatchs.Append(xe2.GetAttribute("match").ToString() + ",");
                    sbTeamHome.Append(xe2.GetAttribute("team_home").ToString() + ",");
                    sbTeamAway.Append(xe2.GetAttribute("team_away").ToString() + ",");
                    sbStartTime.Append(xe2.GetAttribute("start_time").ToString() + ",");
                    sbScore.Append(xe2.GetAttribute("score").ToString() + ",");
                }
            }

            matchs.matchs = sbMatchs.ToString().TrimEnd(',');
            matchs.team_home = sbTeamHome.ToString().TrimEnd(',');
            matchs.team_away = sbTeamAway.ToString().TrimEnd(',');
            matchs.start_time = sbStartTime.ToString().TrimEnd(',');
            matchs.score = sbScore.ToString().TrimEnd(',');

            list.Add(matchs);
        }
        return list;
    }

    /// <summary>
    /// 获得xml接口数据
    /// </summary>
    /// <returns>返回xmldocument格式数据</returns>
    public XmlDocument GetXmlData()
    {
        string url = "http://s.apiplus.cn/bq/?token=2bbadaee337d9127";//xml接口
        XmlDocument xml = new XmlDocument();
        try
        {
            xml.Load(url);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }

        return xml;
    }
    /// <summary>
    /// 每期比赛
    /// </summary>
    public class Matchs
    {
        public int phase;
        public DateTime? sale_start;
        public DateTime? sale_end;
        public string matchs;
        public string team_home;
        public string team_away;
        public string start_time;
        public string result;
        public string score;
    }
    // 获得奖池币
    private long AllPrize(int resultCID)
    {
        //获取当前期数投注总额
        long AllPrice = new BCW.BQC.BLL.BQCPay().AllPrice(resultCID);
        //获取已经派出奖金
        long _Price = new BCW.BQC.BLL.BQCPay().AllWinCentbyCID(resultCID);
        //获取当前期数系统投注总额
        long Sysprize = new BCW.BQC.DAL.BQCList().getsysprize(resultCID);
        //获取当期系统投注状态
        int Sysprizestatue = new BCW.BQC.DAL.BQCList().getsysprizestatue(resultCID);
        //获取上一期滚存下来的奖池
        int lastcid = 0;
        if (new BCW.BQC.BLL.BQCList().Exists(resultCID - 1))
        {
            lastcid = (resultCID - 1);
        }
        else
        {
            lastcid = LastOpenCID();
        }
        long Nextprize = new BCW.BQC.DAL.BQCList().getnextprize(lastcid);

        //获取当前期数系统回收总额
        long SysWin = new BCW.BQC.BLL.BQCJackpot().SysWin(resultCID);
        //奖池总额
        long Prices = 0;
        if (Sysprizestatue == 3 || Sysprizestatue == 1)
        {
            Prices = (AllPrice + Nextprize + Sysprize);
        }
        else
        {
            Prices = (AllPrice + Nextprize);
        }
        return Prices;
    }
    // 获得当期奖池结余
    private long NextPrize(int resultCID)
    {
        long nowprize = new BCW.BQC.BLL.BQCList().nowprize(resultCID);
        //获取已经派出奖金
        long _Price = new BCW.BQC.BLL.BQCPay().AllWinCentbyCID(resultCID);
        long sysprizeshouxu = Convert.ToInt64(nowprize * Convert.ToInt64(ub.GetSub("BQCsys", "/Controls/BQC.xml")) * 0.01);
        long Prices = nowprize - _Price - sysprizeshouxu;//当期结余=当期奖池-当期系统收取-当期派奖
        return Prices;
    }
    // 获得当期剩余奖池（为每一次减去中奖额减去系统回收）
    private long NowPrize(int resultCID)
    {
        long nowprize1 = new BCW.BQC.BLL.BQCList().nowprize(resultCID);
        long sysprizeshouxu = Convert.ToInt64(nowprize1 * Convert.ToInt64(ub.GetSub("BQCsys", "/Controls/BQC.xml")) * 0.01);
        long Prices = nowprize1 - sysprizeshouxu;//当期结余=当期奖池-当期系统收取-当期派奖
        return Prices;
    }
    //获取数据库最新已经开奖期号
    private int LastOpenCID()
    {
        try
        {
            int CID = 0;
            DataSet ds = new BCW.BQC.BLL.BQCList().GetList("CID", " State=1 Order by CID Desc ");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                CID = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
            }
            return CID;
        }
        catch { return 0; }
    }
    //获取数据库最新未开奖期号
    private int FirstNewCID()
    {
        try
        {
            int CID = 0;
            DataSet ds = new BCW.BQC.BLL.BQCList().GetList("CID", " State=0 Order by CID Asc ");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                CID = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
            }
            return CID;
        }
        catch { return 0; }
    }
}

