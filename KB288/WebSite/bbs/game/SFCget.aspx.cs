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
using System.Threading;

/// <summary>
/// 抓取全新版本 20160831 蒙宗将
/// 修复抓取bug 20160908 蒙宗将
/// 蒙宗将 20160924 开奖规划，上期无开奖下期停滞
/// 20161007 蒙宗将 优化获取上期
/// 蒙宗将 20161010 抓取场次不够继续抓取
/// 蒙宗将 20161112 优化开奖
/// </summary>


public partial class bbs_game_SFCget : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/SFC.xml";
    protected string GameName = ub.GetSub("SFName", "/Controls/SFC.xml");//游戏名字
    protected void Page_Load(object sender, EventArgs e)
    {
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();
        try
        {
            ZQPage();
        }
        catch { }
        try
        {
            CasePage();
        }
        catch { }
        builder.Append("ok1" + "<br />");
        builder.Append("<b>" + ub.GetSub("SFName", "/Controls/SFC.xml") + "数据正常获取中！！</b><br />");
        builder.Append("<b>上次获取时间：</b>" + DateTime.Now + "<br />");
        stopwatch.Stop();
        builder.Append("<font color=\"black\">" + "总耗时:" + stopwatch.Elapsed.TotalSeconds + "秒");
    }
    //抓取页面
    public void ZQPage()
    {

        List<Matchs> list = TranList();

        list.Reverse();//集合反序

        BCW.SFC.Model.SfList sfcList = new BCW.SFC.Model.SfList();
        BCW.SFC.BLL.SfList sfcListBll = new BCW.SFC.BLL.SfList();


        foreach (Matchs a in list)
        {
            if (!String.IsNullOrEmpty(a.sale_start.ToString()))//判断开卖时间是否有空，数据接口有时会有期数但没有开卖时间
            {
                if (sfcListBll.ExistsCID(a.phase))//如果存在该期数，更新该期的比赛得分
                {
                    string result = string.Empty;
                    //   int newQishu = Convert.ToInt32(new BCW.SFC.GetSFC().GetResult(2));

                    //if (newQishu == a.phase)
                    //{
                    //    try { result = sfcListBll.FindResultByPhase(a.phase); }
                    //    catch { result = new BCW.SFC.GetSFC().GetResult(1); }
                    //}
                    //else
                    //{
                    result = sfcListBll.FindResultByPhase(a.phase);
                    //}
                    BCW.SFC.Model.SfList SFCCUN = new BCW.SFC.BLL.SfList().GetSfList1(a.phase);
                    if (SFCCUN.Match.Split(',').Length != 14 || SFCCUN.Team_Home.Split(',').Length != 14 || SFCCUN.Team_Away.Split(',').Length != 14 || SFCCUN.Start_time.Split(',').Length != 14)
                    {
                        new BCW.SFC.BLL.SfList().UpdateXinXi(a.matchs, a.team_home, a.team_away, a.start_time, a.phase);
                    }

                    if (String.IsNullOrEmpty(result))//判断比赛总结果，如果总结果为空，证明该期的比赛还没有比完。
                    {
                        sfcList.CID = a.phase;
                        sfcList.Result = a.result;
                        sfcList.Score = a.score;
                        sfcList.Sale_StartTime = a.sale_start;
                        sfcList.EndTime = (DateTime)a.sale_end;

                        sfcList.Match = a.matchs;
                        sfcList.Team_Home = a.team_home;
                        sfcList.Team_Away = a.team_away;
                        sfcList.Start_time = a.start_time;

                        if (new BCW.SFC.BLL.SfList().getState((a.phase - 1)) != 0)//上一场未开奖不会执行下一场的开奖
                        {
                            if (sfcListBll.UpdateMatchs(sfcList))//更新赛事结果跟比分
                            {

                                builder.Append("更新第" + a.phase + "期数据成功！ok1<br />");
                            }
                            else
                            {
                                builder.Append("更新第" + a.phase + "期数据失败！error1<br />");
                            }
                        }
                    }
                    else
                    {
                        if (sfcListBll.UpdateState(1, a.phase))
                        {
                            builder.Append("更新第" + a.phase + "期开奖状态成功！ok1<br />");
                        }
                        else
                        {
                            builder.Append("更新第" + a.phase + "期开奖状态失败！error1<br />");
                        }

                    }
                    // builder.Append("更新" + a.phase + "数据成功ok1<br />");
                }

                else//插入新的比赛赛事
                {
                    int CID = a.phase;
                    sfcList.CID = a.phase;
                    sfcList.Result = a.result;
                    sfcList.Sale_StartTime = a.sale_start;
                    sfcList.EndTime = (DateTime)a.sale_end;
                    sfcList.Match = a.matchs;
                    sfcList.Team_Home = a.team_home;
                    sfcList.Team_Away = a.team_away;
                    sfcList.Score = a.score;
                    sfcList.Start_time = a.start_time;
                    sfcList.State = 0;
                    sfcList.PayCent = 0;
                    sfcList.PayCount = 0;
                    sfcList.nowprize = 0;
                    sfcList.other = "";
                    sfcList.nextprize = 0;
                    if (new BCW.SFC.DAL.SfList().getState(CID - 1) == 0)//未开奖上一期
                    {
                        sfcList.sysprize = 0;
                        sfcList.sysprizestatue = 0;
                    }
                    else//上一期已经开奖
                    {
                        if (new BCW.SFC.DAL.SfList().getnextprize(CID - 1) > Convert.ToInt64(ub.GetSub("SFMin", "/Controls/SFC.xml")))
                        {
                            sfcList.sysprize = 0;
                            sfcList.sysprizestatue = 0;
                        }
                        else
                        {
                            sfcList.sysprize = Convert.ToInt64(ub.GetSub("SFCSend", "/Controls/SFC.xml"));
                            sfcList.sysprizestatue = 1;//表示系统有投入
                        }
                    }
                    sfcList.sysdayprize = 0;
                    new BCW.SFC.BLL.SfList().Add(sfcList);

                    if (new BCW.SFC.DAL.SfList().getState(CID - 1) == 1)
                    {
                        if (new BCW.SFC.DAL.SfList().getnextprize(CID - 1) > Convert.ToInt64(ub.GetSub("SFMin", "/Controls/SFC.xml")))
                        {
                            //把记录加到奖池表
                            BCW.SFC.Model.SfJackpot mo = new BCW.SFC.Model.SfJackpot();
                            mo.usID = 0;//1表示系统无投入
                            mo.WinPrize = 0;
                            mo.Prize = 0;
                            mo.other = "上期滚存" + new BCW.SFC.DAL.SfList().getnextprize(CID - 1);
                            mo.allmoney = new BCW.SFC.DAL.SfList().getnextprize(CID - 1);
                            mo.AddTime = DateTime.Now;
                            mo.CID = CID;
                            new BCW.SFC.BLL.SfJackpot().Add(mo);
                        }
                        else
                        {
                            //把记录加到奖池表
                            BCW.SFC.Model.SfJackpot mo = new BCW.SFC.Model.SfJackpot();
                            mo.usID = 1;//1表示系统有投入
                            mo.WinPrize = 0;
                            mo.Prize = Convert.ToInt64(ub.GetSub("SFCSend", "/Controls/SFC.xml")) + new BCW.SFC.DAL.SfList().getnextprize(CID - 1);
                            mo.other = "系统投入" + Convert.ToInt64(ub.GetSub("SFCSend", "/Controls/SFC.xml")) + "上期滚存" + new BCW.SFC.DAL.SfList().getnextprize(CID - 1);
                            mo.allmoney = new BCW.SFC.DAL.SfList().getnextprize(CID - 1) + Convert.ToInt64(ub.GetSub("SFCSend", "/Controls/SFC.xml"));
                            mo.AddTime = DateTime.Now;
                            mo.CID = CID;
                            new BCW.SFC.BLL.SfJackpot().Add(mo);
                        }
                    }

                    builder.Append("获取第" + a.phase + "期数据成功！ok1<br>");

                }
            }

        }
        DataSet ds = new BCW.SFC.BLL.SfList().GetList("CID", " State=1 Order by CID Desc ");
        try
        {
            if (new BCW.SFC.BLL.SfList().getsysstate((Convert.ToInt32(ds.Tables[0].Rows[0][0]) + 1)) == 0)
            {
                if (ds.Tables[0].Rows.Count > 0 && ds != null)
                {
                    int CIDs = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                    int sta = new BCW.SFC.BLL.SfList().getsysstate(CIDs);

                    if ((CIDs + 1) > 0)
                    {
                        if (!new BCW.SFC.BLL.SfJackpot().Exists4())
                        {
                            BCW.SFC.Model.SfList sq = new BCW.SFC.Model.SfList();
                            sq.sysprize = Convert.ToInt64(ub.GetSub("SFMin", "/Controls/SFC.xml"));
                            sq.sysprizestatue = 3;//表示首期系统有投入
                            new BCW.SFC.BLL.SfList().UpdateSysstaprize((CIDs + 1), sq.sysprizestatue, sq.sysprize);

                            //把记录加到奖池表
                            BCW.SFC.Model.SfJackpot mo = new BCW.SFC.Model.SfJackpot();
                            mo.usID = 8;//8表示系统首期投入
                            mo.WinPrize = 0;
                            mo.Prize = Convert.ToInt64(ub.GetSub("SFMin", "/Controls/SFC.xml"));
                            mo.other = "系统首期(" + (CIDs + 1) + "期)投入" + Convert.ToInt64(ub.GetSub("SFMin", "/Controls/SFC.xml")) + "|结余" + Convert.ToInt64(ub.GetSub("SFMin", "/Controls/SFC.xml")) + ub.Get("SiteBz");
                            mo.allmoney = Convert.ToInt64(ub.GetSub("SFMin", "/Controls/SFC.xml"));
                            mo.AddTime = DateTime.Now;
                            mo.CID = (CIDs + 1);
                            new BCW.SFC.BLL.SfJackpot().Add(mo);
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
        DataSet qi = new BCW.SFC.BLL.SfList().GetList("CID,Result", " Result!='" + null + "' Order by CID Desc ");
        int CID = 0; string Result = string.Empty;
        CID = Convert.ToInt32(qi.Tables[0].Rows[0][0]);//最新开奖的期号  
        int resultCID = CID;
        Result = Convert.ToString(qi.Tables[0].Rows[0][1]);//最新开奖结果
        #endregion

        #region 更新中奖判断（判断几等奖）
        //中奖
        int count = 0;
        string[] resultnum = Result.Split(',');
        //遍历表SfPay，更新中奖
        DataSet dspay = new BCW.SFC.BLL.SfPay().GetList("CID,usID,vote,VoteNum,payCents,change,id", "CID=" + CID + " and State=0");
        if (dspay != null && dspay.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dspay.Tables[0].Rows.Count; i++)
            {
                int pid = int.Parse(dspay.Tables[0].Rows[i]["id"].ToString());
                int UsID = int.Parse(dspay.Tables[0].Rows[i]["UsID"].ToString());
                string Vote = dspay.Tables[0].Rows[i]["Vote"].ToString();
                int VoteNum = int.Parse(dspay.Tables[0].Rows[i]["VoteNum"].ToString());
                string[] votenum = Vote.Split(',');
                long PayCents = Int64.Parse(dspay.Tables[0].Rows[i]["PayCents"].ToString());
                string change = dspay.Tables[0].Rows[i]["change"].ToString();
                bool IsWin = false;

                int num1 = 0; int num2 = 0;
                int num3 = 0; int num4 = 0;
                int num5 = 0; int num6 = 0;
                int num7 = 0;
                bool IsWinf = false;

                #region 单式中奖算法
                if (VoteNum == 1)//单式算法
                {
                    for (int k = 0; k < votenum.Length; k++)
                    {

                        string aa = string.Empty;
                        string bb = string.Empty;
                        try
                        {
                            aa = votenum[k];
                        }
                        catch
                        {
                            // Response.Write(k);
                            break;
                        }
                        try
                        {
                            bb = resultnum[k];
                        }
                        catch
                        {
                            // Response.Write(k + "rrr");
                            break;
                        }
                        if (True(aa, bb))
                        {
                            IsWin = true;
                            count++;
                        }


                    }
                    #region 单式中奖更新数据库
                    if (IsWin == true)
                    {
                        switch (count)
                        {
                            case 8:
                                {
                                    BCW.Data.SqlHelper.ExecuteSql("update tb_SFPay set ISPrize=7 Where id=" + pid + "");
                                }
                                break;
                            case 9:
                                {
                                    BCW.Data.SqlHelper.ExecuteSql("update tb_SFPay set ISPrize=6 Where id=" + pid + "");
                                }
                                break;
                            case 10:
                                {
                                    BCW.Data.SqlHelper.ExecuteSql("update tb_SFPay set ISPrize=5 Where id=" + pid + "");
                                }
                                break;
                            case 11:
                                {
                                    BCW.Data.SqlHelper.ExecuteSql("update tb_SFPay set ISPrize=4 Where id=" + pid + "");
                                }
                                break;
                            case 12:
                                {
                                    BCW.Data.SqlHelper.ExecuteSql("update tb_SFPay set ISPrize=3 Where id=" + pid + "");
                                }
                                break;
                            case 13:
                                {
                                    BCW.Data.SqlHelper.ExecuteSql("update tb_SFPay set ISPrize=2 Where id=" + pid + "");
                                }
                                break;
                            case 14:
                                {
                                    BCW.Data.SqlHelper.ExecuteSql("update tb_SFPay set ISPrize=1 Where id=" + pid + "");
                                }
                                break;
                        }
                        count = 0;
                    }
                    #endregion

                }
                #endregion

                #region 复式算法
                else // if (VoteNum > 1)//复式算法
                {
                    string resultnumf = Result;

                    string[] buyvote = Vote.Split(',');//数据库存储的投注记录格式：3/1/0,1,0,3,0,1,0,3,0,1,1,0,1,3
                    string[] vote1 = buyvote[0].Split('/');
                    string[] vote2 = buyvote[1].Split('/');
                    string[] vote3 = buyvote[2].Split('/');
                    string[] vote4 = buyvote[3].Split('/');
                    string[] vote5 = buyvote[4].Split('/');
                    string[] vote6 = buyvote[5].Split('/');
                    string[] vote7 = buyvote[6].Split('/');
                    string[] vote8 = buyvote[7].Split('/');
                    string[] vote9 = buyvote[8].Split('/');
                    string[] vote10 = buyvote[9].Split('/');
                    string[] vote11 = buyvote[10].Split('/');
                    string[] vote12 = buyvote[11].Split('/');
                    string[] vote13 = buyvote[12].Split('/');
                    string[] vote14 = buyvote[13].Split('/');

                    List<string[]> list = new List<string[]>();
                    list.Add(vote1);
                    list.Add(vote2);
                    list.Add(vote3);
                    list.Add(vote4);
                    list.Add(vote5);
                    list.Add(vote6);
                    list.Add(vote7);
                    list.Add(vote8);
                    list.Add(vote9);
                    list.Add(vote10);
                    list.Add(vote11);
                    list.Add(vote12);
                    list.Add(vote13);
                    list.Add(vote14);
                    string[] totalResult;
                    string[] Finalresult = resultnumf.Split(',');
                    //到投注数据
                    totalResult = bianli(list);

                    for (int iresult = 0; iresult < totalResult.Length; iresult++)
                    {
                        string[] result = totalResult[iresult].Split(',');
                        for (int j = 0; j < result.Length; j++)
                        {
                            //遍历开奖数据是否相同，相同则count+1
                            if (result[j].Equals(Finalresult[j]))
                            {
                                count++;
                            }
                            if (Finalresult[j].Contains("*"))
                            {
                                count++;
                            }

                        }

                        //如果count出现的次数等于8，证明是七等奖
                        if (count == 8)
                        {
                            num7++;
                            IsWinf = true;

                        }
                        //如果count出现的次数等于9，证明是六等奖
                        if (count == 9)
                        {
                            num6++;
                            IsWinf = true;
                        }
                        //如果count出现的次数等于10，证明是五等奖
                        if (count == 10)
                        {
                            num5++;
                            IsWinf = true;

                        }
                        //如果count出现的次数等于11，证明是四等奖
                        if (count == 11)
                        {
                            num4++;
                            IsWinf = true;
                        }
                        //如果count出现的次数等于12，证明是三等奖
                        if (count == 12)
                        {
                            num3++;
                            IsWinf = true;

                        }
                        //如果count出现的次数等于13，证明是二等奖
                        if (count == 13)
                        {
                            num2++;
                            IsWinf = true;
                        }
                        //如果count出现的次数等于14，证明是一等奖
                        if (count == 14)
                        {
                            num1++;
                            IsWinf = true;
                        }
                        count = 0;
                    }
                    #region 复式中奖更新数据库
                    if (IsWinf == true)
                    {
                        BCW.Data.SqlHelper.ExecuteSql("update tb_SFPay set IsPrize=10 Where id=" + pid + "");
                        if (num1 > 0)
                        {
                            builder.Append("一等奖" + num1);
                            new BCW.SFC.BLL.SfPay().UpdateChange(pid, "一/" + num1 + "#");
                        }
                        if (num2 > 0)
                        {
                            builder.Append("二等奖" + num2);
                            new BCW.SFC.BLL.SfPay().UpdateChange(pid, "二/" + num2 + "#");
                        }
                        if (num3 > 0)
                        {
                            builder.Append("三等奖" + num3);
                            new BCW.SFC.BLL.SfPay().UpdateChange(pid, "三/" + num3 + "#");
                        }
                        if (num4 > 0)
                        {
                            builder.Append("四等奖" + num4);
                            new BCW.SFC.BLL.SfPay().UpdateChange(pid, "四/" + num4 + "#");
                        }
                        if (num5 > 0)
                        {
                            builder.Append("五等奖" + num5);
                            new BCW.SFC.BLL.SfPay().UpdateChange(pid, "五/" + num5 + "#");
                        }
                        if (num6 > 0)
                        {
                            builder.Append("六等奖" + num6);
                            new BCW.SFC.BLL.SfPay().UpdateChange(pid, "六/" + num6 + "#");
                        }
                        if (num7 > 0)
                        {
                            builder.Append("七等奖" + num7);
                            new BCW.SFC.BLL.SfPay().UpdateChange(pid, "七/" + num7 + "");
                        }
                        //  break;
                    }
                    #endregion
                }
                #endregion
            }
        }

        #endregion

        #region 奖池更新 （包括与回收）
        if (Result != null)
        {
            if (new BCW.SFC.BLL.SfList().ExistsCID(resultCID))
            {
                if (new BCW.SFC.BLL.SfList().getState(resultCID) == 0)
                {
                    //判断奖池，小于200万就投入，大于不投入
                    if (AllPrize(resultCID) < Convert.ToInt64(ub.GetSub("SFMin", "/Controls/SFC.xml")))
                    {
                        if (new BCW.SFC.BLL.SfList().getsysstate(resultCID) == 0)
                        {
                            long all = AllPrize(resultCID) + Convert.ToInt64(ub.GetSub("SFMin", "/Controls/SFC.xml"));
                            //更新奖池
                            new BCW.SFC.BLL.SfList().updateNowprize(all, resultCID);
                            new BCW.SFC.BLL.SfList().UpdateSysstaprize(resultCID, 3, Convert.ToInt64(ub.GetSub("SFMin", "/Controls/SFC.xml")));//3表示首期投入
                            //把记录加到奖池表
                            BCW.SFC.Model.SfJackpot mo = new BCW.SFC.Model.SfJackpot();

                            mo.WinPrize = 0;
                            mo.Prize = Convert.ToInt64(ub.GetSub("SFMin", "/Controls/SFC.xml"));
                            mo.usID = 8;//8表示系统首期有投入
                            mo.other = "系统首期(" + resultCID + "期)投入" + Convert.ToInt64(ub.GetSub("SFMin", "/Controls/SFC.xml")) + "|结余" + Convert.ToInt64(ub.GetSub("SFMin", "/Controls/SFC.xml")) + ub.Get("SiteBz");
                            mo.allmoney = all;
                            mo.AddTime = DateTime.Now.AddMilliseconds(-10);
                            mo.CID = resultCID;
                            new BCW.SFC.BLL.SfJackpot().Add(mo);
                        }
                        else
                        {
                            //更新奖池
                            new BCW.SFC.BLL.SfList().updateNowprize(AllPrize(resultCID), resultCID);

                        }

                    }
                    else if (AllPrize(resultCID) > Convert.ToInt64(ub.GetSub("SFMin", "/Controls/SFC.xml")) && AllPrize(resultCID) < Convert.ToInt64(ub.GetSub("SFMax", "/Controls/SFC.xml")))
                    {
                        //更新奖池
                        new BCW.SFC.BLL.SfList().updateNowprize(AllPrize(resultCID), resultCID);
                    }
                    //判断大于400万，有投入回收，无投入不回收
                    //if (AllPrize(model.CID) > Convert.ToInt64(ub.GetSub("SFCMax", "/Controls/SFC.xml")))
                    else
                    {
                        //如果之前期数有投入为回收就回收，无则不做操作
                        if (new BCW.SFC.BLL.SfList().Existsysprize(resultCID))
                        {
                            //未开奖当前投注期号
                            DataSet dsh = new BCW.SFC.BLL.SfList().GetList("TOP 1 CID", "State=1 and sysprizestatue=1 order by CID ASC");
                            int CIDh = 0;
                            CIDh = int.Parse(dsh.Tables[0].Rows[0][0].ToString());
                            new BCW.SFC.BLL.SfList().UpdateSysstaprize(resultCID, 2, Convert.ToInt64(ub.GetSub("SFCSend", "/Controls/SFC.xml")));//当期回收
                            long all = (AllPrize(resultCID)) - Convert.ToInt64(ub.GetSub("SFCSend", "/Controls/SFC.xml"));
                            //更新奖池
                            new BCW.SFC.BLL.SfList().updateNowprize(all, resultCID);
                            //把记录加到奖池表
                            BCW.SFC.Model.SfJackpot mo = new BCW.SFC.Model.SfJackpot();
                            mo.usID = 7;//7表示系统有回收
                            mo.WinPrize = Convert.ToInt64(ub.GetSub("SFCSend", "/Controls/SFC.xml"));
                            mo.Prize = 0;
                            mo.other = "系统回收" + Convert.ToInt64(ub.GetSub("SFCSend", "/Controls/SFC.xml"));
                            mo.allmoney = all;
                            mo.AddTime = DateTime.Now.AddMilliseconds(-500);
                            mo.CID = resultCID;
                            new BCW.SFC.BLL.SfJackpot().Add(mo);

                            if (resultCID != CIDh)
                            {
                                new BCW.SFC.BLL.SfList().UpdateSysprizestatue(CIDh, 3);//表示被回收
                            }
                        }
                        else
                        {
                            //更新奖池
                            new BCW.SFC.BLL.SfList().updateNowprize(AllPrize(resultCID), resultCID);
                        }
                    }
                }
            }
        }
        #endregion

        #region 更新奖池手续更新
        long Allmoney = new BCW.SFC.BLL.SfList().nowprize(resultCID);
        //判断该期数有无中奖
        if (!new BCW.SFC.BLL.SfPay().Exists(resultCID, 0))
        {
            #region 无中奖奖池更新

            if (!new BCW.SFC.BLL.SfList().ExistsSysprize(resultCID))//如果不存在系统投注
            {
                //是否存在
                bool qishu = new BCW.SFC.BLL.SfList().ExistsCID(resultCID);

                if (qishu)
                {
                    if (!new BCW.SFC.BLL.SfJackpot().Exists3(resultCID))//开奖当前还没有数据
                    {
                        BCW.SFC.Model.SfJackpot mo = new BCW.SFC.Model.SfJackpot();
                        mo.usID = 0;
                        mo.WinPrize = Convert.ToInt64(Allmoney * Convert.ToInt64(ub.GetSub("SFsys", "/Controls/SFC.xml")) * 0.01);//系统收回%10
                        mo.Prize = 0;
                        long a = Convert.ToInt64(Allmoney * Convert.ToInt64(ub.GetSub("SFsys", "/Controls/SFC.xml")) * 0.01);
                        long b = Convert.ToInt64(Allmoney) - a;
                        mo.other = "系统收取手续" + a;
                        mo.allmoney = b;
                        mo.AddTime = DateTime.Now;
                        mo.CID = resultCID;
                        new BCW.SFC.BLL.SfJackpot().Add(mo);
                    }
                }
            }
            else//如果存在系统投注
            {

                if (Allmoney > Convert.ToInt64(ub.GetSub("SFMax", "/Controls/SFC.xml")))//最大奖池数
                {
                    if (!new BCW.SFC.BLL.SfJackpot().Exists3(resultCID))
                    {
                        BCW.SFC.Model.SfJackpot mo = new BCW.SFC.Model.SfJackpot();
                        mo.usID = 2;//表示回收投入的金额
                        mo.WinPrize = Convert.ToInt64(Allmoney * Convert.ToInt64(ub.GetSub("SFsys", "/Controls/SFC.xml")) * 0.01);
                        mo.Prize = 0;
                        long a = Convert.ToInt64(Allmoney * Convert.ToInt64(ub.GetSub("SFsys", "/Controls/SFC.xml")) * 0.01) + Convert.ToInt64(ub.GetSub("SFMin", "/Controls/SFC.xml"));
                        long b = Convert.ToInt64(Allmoney) - Convert.ToInt64(Allmoney * Convert.ToInt64(ub.GetSub("SFsys", "/Controls/SFC.xml")) * 0.01);
                        mo.other = "系统收取手续" + Convert.ToInt64(Allmoney * Convert.ToInt64(ub.GetSub("SFsys", "/Controls/SFC.xml")) * 0.01);
                        mo.allmoney = b;
                        mo.AddTime = DateTime.Now;
                        mo.CID = resultCID;

                        new BCW.SFC.BLL.SfJackpot().Add(mo);
                    }

                }
                else//奖池与最小奖池数
                {
                    if (!new BCW.SFC.BLL.SfJackpot().Exists3(resultCID))
                    {
                        BCW.SFC.Model.SfJackpot mo = new BCW.SFC.Model.SfJackpot();
                        mo.usID = 2;
                        mo.WinPrize = Convert.ToInt64(Allmoney * Convert.ToInt64(ub.GetSub("SFsys", "/Controls/SFC.xml")) * 0.01);//系统收回%10
                        mo.Prize = 0;
                        long a = Convert.ToInt64(Allmoney * Convert.ToInt64(ub.GetSub("SFsys", "/Controls/SFC.xml")) * 0.01);
                        long b = Convert.ToInt64(Allmoney) - a;
                        mo.other = "系统收取手续" + a;
                        mo.allmoney = b;
                        mo.AddTime = DateTime.Now;
                        mo.CID = resultCID;
                        new BCW.SFC.BLL.SfJackpot().Add(mo);
                    }

                }
            }
            #endregion
        }
        else//有中奖
        {
            #region 中奖奖池更新
            if (!new BCW.SFC.BLL.SfList().ExistsSysprize(resultCID))//如果不存在系统投注
            {
                if (!new BCW.SFC.BLL.SfJackpot().Exists3(resultCID))//开奖当前还没有数据
                {
                    BCW.SFC.Model.SfJackpot mo = new BCW.SFC.Model.SfJackpot();
                    mo.usID = 2;
                    mo.WinPrize = Convert.ToInt64(Allmoney * Convert.ToInt64(ub.GetSub("SFsys", "/Controls/SFC.xml")) * 0.01);//系统收回%10
                    mo.Prize = 0;
                    long a = Convert.ToInt64(Allmoney * Convert.ToInt64(ub.GetSub("SFsys", "/Controls/SFC.xml")) * 0.01);
                    long b = Convert.ToInt64(Allmoney) - a;
                    mo.other = "系统收取手续" + a;
                    mo.allmoney = b;
                    mo.AddTime = DateTime.Now;
                    mo.CID = resultCID;
                    new BCW.SFC.BLL.SfJackpot().Add(mo);
                }

            }
            else//如果存在系统投注
            {

                if (Allmoney > Convert.ToInt64(ub.GetSub("SFMax", "/Controls/SFC.xml")))//最大奖池数
                {
                    if (!new BCW.SFC.BLL.SfJackpot().Exists3(resultCID))
                    {
                        BCW.SFC.Model.SfJackpot mo = new BCW.SFC.Model.SfJackpot();
                        mo.usID = 2;//表示回收投入的金额
                        mo.WinPrize = Convert.ToInt64(Allmoney * Convert.ToInt64(ub.GetSub("SFsys", "/Controls/SFC.xml")) * 0.01) + Convert.ToInt64(ub.GetSub("SFMin", "/Controls/SFC.xml"));
                        mo.Prize = 0;
                        long a = Convert.ToInt64(Allmoney * Convert.ToInt64(ub.GetSub("SFsys", "/Controls/SFC.xml")) * 0.01);
                        long b = Convert.ToInt64(Allmoney) - Convert.ToInt64(Allmoney * Convert.ToInt64(ub.GetSub("SFsys", "/Controls/SFC.xml")) * 0.01);
                        mo.other = "系统收取手续" + Convert.ToInt64(Allmoney * Convert.ToInt64(ub.GetSub("SFsys", "/Controls/SFC.xml")) * 0.01);
                        mo.allmoney = b;
                        mo.AddTime = DateTime.Now;
                        new BCW.SFC.BLL.SfJackpot().Add(mo);
                    }

                }
                else//奖池与最小奖池数
                {
                    if (!new BCW.SFC.BLL.SfJackpot().Exists3(resultCID))
                    {
                        BCW.SFC.Model.SfJackpot mo = new BCW.SFC.Model.SfJackpot();
                        mo.usID = 2;
                        mo.WinPrize = Convert.ToInt64(Allmoney * Convert.ToInt64(ub.GetSub("SFsys", "/Controls/SFC.xml")) * 0.01);//系统收回%10
                        mo.Prize = 0;
                        long a = Convert.ToInt64(Allmoney * Convert.ToInt64(ub.GetSub("SFsys", "/Controls/SFC.xml")) * 0.01);
                        long b = Convert.ToInt64(Allmoney) - a;
                        mo.other = "系统收取手续" + a;
                        mo.allmoney = b;
                        mo.AddTime = DateTime.Now;
                        mo.CID = resultCID;
                        new BCW.SFC.BLL.SfJackpot().Add(mo);
                    }
                    //else

                }
            }
            #endregion
        }
        #endregion

        #region 给中奖会员派奖
        if (new BCW.SFC.DAL.SfList().getState(resultCID) == 0)
        {
            DataSet pay = new BCW.SFC.BLL.SfPay().GetList("usID,ISPrize,VoteNum,id,change", "CID=" + resultCID + " and State=0");
            if (pay != null && pay.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < pay.Tables[0].Rows.Count; i++)
                {
                    int pid = int.Parse(pay.Tables[0].Rows[i]["id"].ToString());
                    int ISPrize = int.Parse(pay.Tables[0].Rows[i]["ISPrize"].ToString());
                    int UsID = int.Parse(pay.Tables[0].Rows[i]["usID"].ToString());
                    int VoteNum = int.Parse(pay.Tables[0].Rows[i]["VoteNum"].ToString());
                    string change = pay.Tables[0].Rows[i]["change"].ToString();
                    int overridebyid = new BCW.SFC.BLL.SfPay().VoteNum(pid, resultCID);
                    //得到当前奖池
                    long All = new BCW.SFC.BLL.SfList().nowprize(resultCID);
                    long Now = NextPrize(resultCID);

                    #region 单式中奖
                    //一等奖
                    if (new BCW.SFC.BLL.SfPay().Exists1(pid, 1))
                    {
                        //注数
                        int zhu = new BCW.SFC.BLL.SfPay().countPrize(resultCID, 1) + getzhu(1, resultCID);
                        //费率
                        double lv = Convert.ToDouble(ub.GetSub("SFOne", "/Controls/SFC.xml")) * 0.01;
                        double allr = Math.Round(Convert.ToDouble(All * lv / Convert.ToDouble(zhu)), 2);
                        long all = Convert.ToInt64(allr * overridebyid);

                        //添加奖池数据
                        BCW.SFC.Model.SfJackpot mo = new BCW.SFC.Model.SfJackpot();
                        mo.usID = UsID;
                        mo.WinPrize = all;
                        mo.Prize = 0;
                        mo.other = "中一等奖" + Convert.ToString(all);
                        mo.allmoney = Now - all;
                        mo.AddTime = DateTime.Now;
                        mo.CID = resultCID;
                        new BCW.SFC.BLL.SfJackpot().Add(mo);
                        BCW.Data.SqlHelper.ExecuteSql("update tb_SFPay set WinCent=" + all + " Where id=" + pid + "");
                        //动态
                        string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + new BCW.BLL.User().GetUsName(UsID) + "[/url]在第" + resultCID + "期[url=/bbs/game/SFC.aspx]" + GameName + "[/url]中一等奖" + all + "" + ub.Get("SiteBz") + "";
                        new BCW.BLL.Action().Add(1016, pid, UsID, "", wText);
                        //发送内线
                        new BCW.BLL.Guest().Add(1, UsID, "", "恭喜您在第" + resultCID + "期" + "[URL=/bbs/game/SFC.aspx]" + GameName + "[/URL]" + "投注，中一等奖奖" + all + "" + ub.Get("SiteBz") + "，开奖为" + Result + "[URL=/bbs/game/SFC.aspx?act=case]马上兑奖[/URL]");
                    }
                    //二等奖
                    if (new BCW.SFC.BLL.SfPay().Exists1(pid, 2))
                    {
                        //注数
                        int zhu = new BCW.SFC.BLL.SfPay().countPrize(resultCID, 2) + getzhu(2, resultCID);
                        //费率
                        double lv = Convert.ToDouble(ub.GetSub("SFTwo", "/Controls/SFC.xml")) * 0.01;
                        double allr = Math.Round(Convert.ToDouble(All * lv / Convert.ToDouble(zhu)), 2);
                        long all = Convert.ToInt64(allr * overridebyid);
                        //添加奖池数据
                        BCW.SFC.Model.SfJackpot mo = new BCW.SFC.Model.SfJackpot();
                        mo.usID = UsID;
                        mo.WinPrize = all;
                        mo.Prize = 0;
                        mo.other = "中二等奖" + Convert.ToString(all);
                        mo.allmoney = Now - all;
                        mo.AddTime = DateTime.Now;
                        mo.CID = resultCID;
                        new BCW.SFC.BLL.SfJackpot().Add(mo);
                        BCW.Data.SqlHelper.ExecuteSql("update tb_SFPay set WinCent=" + all + " Where id=" + pid + "");
                        //动态
                        string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + new BCW.BLL.User().GetUsName(UsID) + "[/url]在第" + resultCID + "期[url=/bbs/game/SFC.aspx]" + GameName + "[/url]中二等奖" + all + "" + ub.Get("SiteBz") + "";
                        new BCW.BLL.Action().Add(1016, pid, UsID, "", wText);
                        //发送内线
                        new BCW.BLL.Guest().Add(1, UsID, "", "恭喜您在第" + resultCID + "期" + "[URL=/bbs/game/SFC.aspx]" + ub.GetSub("SFName", "/Controls/SFC.xml") + "[/URL]" + "投注，中二等奖奖" + all + "" + ub.Get("SiteBz") + "，开奖为" + Result + "[URL=/bbs/game/SFC.aspx?act=case]马上兑奖[/URL]");
                    }
                    //三等奖
                    if (new BCW.SFC.BLL.SfPay().Exists1(pid, 3))
                    {
                        //注数
                        int zhu = new BCW.SFC.BLL.SfPay().countPrize(resultCID, 3) + getzhu(3, resultCID);
                        //费率
                        double lv = Convert.ToDouble(ub.GetSub("SFThree", "/Controls/SFC.xml")) * 0.01;
                        double allr = Math.Round(Convert.ToDouble(All * lv / Convert.ToDouble(zhu)), 2);
                        long all = Convert.ToInt64(allr * overridebyid);
                        //添加奖池数据
                        BCW.SFC.Model.SfJackpot mo = new BCW.SFC.Model.SfJackpot();
                        mo.usID = UsID;
                        mo.WinPrize = all;
                        mo.Prize = 0;
                        mo.other = "中三等奖" + Convert.ToString(all);
                        mo.allmoney = Now - all;
                        mo.AddTime = DateTime.Now;
                        mo.CID = resultCID;
                        new BCW.SFC.BLL.SfJackpot().Add(mo);
                        BCW.Data.SqlHelper.ExecuteSql("update tb_SFPay set WinCent=" + all + " Where id=" + pid + "");
                        //动态
                        string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + new BCW.BLL.User().GetUsName(UsID) + "[/url]在第" + resultCID + "期[url=/bbs/game/SFC.aspx]" + GameName + "[/url]中三等奖" + all + "" + ub.Get("SiteBz") + "";
                        new BCW.BLL.Action().Add(1016, pid, UsID, "", wText);
                        //发送内线
                        new BCW.BLL.Guest().Add(1, UsID, "", "恭喜您在第" + resultCID + "期" + "[URL=/bbs/game/SFC.aspx]" + ub.GetSub("SFName", "/Controls/SFC.xml") + "[/URL]" + "投注，中三等奖奖" + all + "" + ub.Get("SiteBz") + "，开奖为" + Result + "[URL=/bbs/game/SFC.aspx?act=case]马上兑奖[/URL]");
                    }
                    //四等奖
                    if (new BCW.SFC.BLL.SfPay().Exists1(pid, 4))
                    {
                        //注数
                        int zhu = new BCW.SFC.BLL.SfPay().countPrize(resultCID, 4) + getzhu(4, resultCID);
                        //费率
                        double lv = Convert.ToDouble(ub.GetSub("SFForc", "/Controls/SFC.xml")) * 0.01;
                        double allr = Math.Round(Convert.ToDouble(All * lv / Convert.ToDouble(zhu)), 2);
                        long all = Convert.ToInt64(allr * overridebyid);
                        //添加奖池数据
                        BCW.SFC.Model.SfJackpot mo = new BCW.SFC.Model.SfJackpot();
                        mo.usID = UsID;
                        mo.WinPrize = all;
                        mo.Prize = 0;
                        mo.other = "中四等奖" + Convert.ToString(all);
                        mo.allmoney = Now - all;
                        mo.AddTime = DateTime.Now;
                        mo.CID = resultCID;
                        new BCW.SFC.BLL.SfJackpot().Add(mo);
                        BCW.Data.SqlHelper.ExecuteSql("update tb_SFPay set WinCent=" + all + " Where id=" + pid + "");
                        //动态
                        string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + new BCW.BLL.User().GetUsName(UsID) + "[/url]在第" + resultCID + "期[url=/bbs/game/SFC.aspx]" + GameName + "[/url]中四等奖" + all + "" + ub.Get("SiteBz") + "";
                        new BCW.BLL.Action().Add(1016, pid, UsID, "", wText);
                        //发送内线
                        new BCW.BLL.Guest().Add(1, UsID, "", "恭喜您在第" + resultCID + "期" + "[URL=/bbs/game/SFC.aspx]" + ub.GetSub("SFName", "/Controls/SFC.xml") + "[/URL]" + "投注，中四等奖奖" + all + "" + ub.Get("SiteBz") + "，开奖为" + Result + "[URL=/bbs/game/SFC.aspx?act=case]马上兑奖[/URL]");
                    }
                    //五等奖
                    if (new BCW.SFC.BLL.SfPay().Exists1(pid, 5))
                    {
                        //注数
                        int zhu = new BCW.SFC.BLL.SfPay().countPrize(resultCID, 5) + getzhu(5, resultCID);
                        //费率
                        double lv = Convert.ToDouble(ub.GetSub("SFFive", "/Controls/SFC.xml")) * 0.01;
                        double allr = Math.Round(Convert.ToDouble(All * lv / Convert.ToDouble(zhu)), 2);
                        long all = Convert.ToInt64(allr * overridebyid);
                        //添加奖池数据
                        BCW.SFC.Model.SfJackpot mo = new BCW.SFC.Model.SfJackpot();
                        mo.usID = UsID;
                        mo.WinPrize = all;
                        mo.Prize = 0;
                        mo.other = "中五等奖" + Convert.ToString(all);
                        mo.allmoney = Now - all;
                        mo.AddTime = DateTime.Now;
                        mo.CID = resultCID;
                        new BCW.SFC.BLL.SfJackpot().Add(mo);
                        BCW.Data.SqlHelper.ExecuteSql("update tb_SFPay set WinCent=" + all + " Where id=" + pid + "");
                        //动态
                        string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + new BCW.BLL.User().GetUsName(UsID) + "[/url]在第" + resultCID + "期[url=/bbs/game/SFC.aspx]" + GameName + "[/url]中五等奖" + all + "" + ub.Get("SiteBz") + "";
                        new BCW.BLL.Action().Add(1016, pid, UsID, "", wText);
                        //发送内线
                        new BCW.BLL.Guest().Add(1, UsID, "", "恭喜您在第" + resultCID + "期" + "[URL=/bbs/game/SFC.aspx]" + ub.GetSub("SFName", "/Controls/SFC.xml") + "[/URL]" + "投注，中五等奖奖" + all + "" + ub.Get("SiteBz") + "，开奖为" + Result + "[URL=/bbs/game/SFC.aspx?act=case]马上兑奖[/URL]");
                    }
                    //六等奖
                    if (new BCW.SFC.BLL.SfPay().Exists1(pid, 6))
                    {
                        //注数
                        int zhu = new BCW.SFC.BLL.SfPay().countPrize(resultCID, 6) + getzhu(6, resultCID);
                        //费率
                        double lv = Convert.ToDouble(ub.GetSub("SFSix", "/Controls/SFC.xml")) * 0.01;
                        double allr = Math.Round(Convert.ToDouble(All * lv / Convert.ToDouble(zhu)), 2);
                        long all = Convert.ToInt64(allr * overridebyid);
                        //添加奖池数据
                        BCW.SFC.Model.SfJackpot mo = new BCW.SFC.Model.SfJackpot();
                        mo.usID = UsID;
                        mo.WinPrize = all;
                        mo.Prize = 0;
                        mo.other = "中六等奖" + Convert.ToString(all);
                        mo.allmoney = Now - all;
                        mo.AddTime = DateTime.Now;
                        mo.CID = resultCID;
                        new BCW.SFC.BLL.SfJackpot().Add(mo);
                        BCW.Data.SqlHelper.ExecuteSql("update tb_SFPay set WinCent=" + all + " Where id=" + pid + "");
                        //动态
                        string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + new BCW.BLL.User().GetUsName(UsID) + "[/url]在第" + resultCID + "期[url=/bbs/game/SFC.aspx]" + GameName + "[/url]中六等奖" + all + "" + ub.Get("SiteBz") + "";
                        new BCW.BLL.Action().Add(1016, pid, UsID, "", wText);
                        //发送内线
                        new BCW.BLL.Guest().Add(1, UsID, "", "恭喜您在第" + resultCID + "期" + "[URL=/bbs/game/SFC.aspx]" + ub.GetSub("SFName", "/Controls/SFC.xml") + "[/URL]" + "投注，中六等奖奖" + all + "" + ub.Get("SiteBz") + "，开奖为" + Result + "[URL=/bbs/game/SFC.aspx?act=case]马上兑奖[/URL]");
                    }
                    //七等奖
                    if (new BCW.SFC.BLL.SfPay().Exists1(pid, 7))
                    {
                        //注数
                        int zhu = new BCW.SFC.BLL.SfPay().countPrize(resultCID, 7) + getzhu(7, resultCID);
                        //费率
                        double lv = Convert.ToDouble(ub.GetSub("SFSeven", "/Controls/SFC.xml")) * 0.01;
                        double allr = Math.Round(Convert.ToDouble(All * lv / Convert.ToDouble(zhu)), 2);
                        long all = Convert.ToInt64(allr * overridebyid);
                        //添加奖池数据
                        BCW.SFC.Model.SfJackpot mo = new BCW.SFC.Model.SfJackpot();
                        mo.usID = UsID;
                        mo.WinPrize = all;
                        mo.Prize = 0;
                        mo.other = "中七等奖" + Convert.ToString(all);
                        mo.allmoney = Now - all;
                        mo.AddTime = DateTime.Now;
                        mo.CID = resultCID;
                        new BCW.SFC.BLL.SfJackpot().Add(mo);
                        BCW.Data.SqlHelper.ExecuteSql("update tb_SFPay set WinCent=" + all + " Where id=" + pid + "");
                        //动态
                        string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + new BCW.BLL.User().GetUsName(UsID) + "[/url]在第" + resultCID + "期[url=/bbs/game/SFC.aspx]" + GameName + "[/url]中七等奖" + all + "" + ub.Get("SiteBz") + "";
                        new BCW.BLL.Action().Add(1016, pid, UsID, "", wText);
                        //发送内线
                        new BCW.BLL.Guest().Add(1, UsID, "", "恭喜您在第" + resultCID + "期" + "[URL=/bbs/game/SFC.aspx]" + ub.GetSub("SFName", "/Controls/SFC.xml") + "[/URL]" + "投注，中七等奖奖" + all + "" + ub.Get("SiteBz") + "，开奖为" + Result + "[URL=/bbs/game/SFC.aspx?act=case]马上兑奖[/URL]");
                    }
                    #endregion

                    #region 复式派奖
                    //复式派奖
                    if (new BCW.SFC.BLL.SfPay().Exists1(pid, 10))
                    {
                        string[] ch = change.Split('#');
                        for (int w = 0; w < ch.Length; w++)
                        {
                            if (ch[w].Contains("一"))
                            {
                                //注数
                                int zhu = new BCW.SFC.BLL.SfPay().countPrize(resultCID, 1) + getzhu(1, resultCID);
                                //费率
                                double lv = Convert.ToDouble(ub.GetSub("SFOne", "/Controls/SFC.xml")) * 0.01;
                                double allr = Math.Round(Convert.ToDouble(All * lv / Convert.ToDouble(zhu)), 2);
                                long all = Convert.ToInt64(allr * getzhu(1, resultCID));
                                //添加奖池数据
                                BCW.SFC.Model.SfJackpot mo = new BCW.SFC.Model.SfJackpot();
                                mo.usID = UsID;
                                mo.WinPrize = all;
                                mo.Prize = 0;
                                mo.other = "中一等奖" + Convert.ToString(all);
                                mo.allmoney = new BCW.SFC.BLL.SfJackpot().Getallmoney(resultCID) - all;
                                mo.AddTime = DateTime.Now;
                                mo.CID = resultCID;
                                new BCW.SFC.BLL.SfJackpot().Add(mo);
                                BCW.Data.SqlHelper.ExecuteSql("update tb_SFPay set WinCent=(( select WinCent from tb_SfPay where id=" + pid + ")+" + all + ") Where id=" + pid + "");
                                //动态
                                string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + new BCW.BLL.User().GetUsName(UsID) + "[/url]在第" + resultCID + "期[url=/bbs/game/SFC.aspx]" + GameName + "[/url]中一等奖" + all + "" + ub.Get("SiteBz") + "";
                                new BCW.BLL.Action().Add(1016, pid, UsID, "", wText);
                                //发送内线
                                new BCW.BLL.Guest().Add(1, UsID, "", "恭喜您在第" + resultCID + "期" + "[URL=/bbs/game/SFC.aspx]" + ub.GetSub("SFName", "/Controls/SFC.xml") + "[/URL]" + "投注，中一等奖奖" + all + "" + ub.Get("SiteBz") + "，开奖为" + Result + "[URL=/bbs/game/SFC.aspx?act=case]马上兑奖[/URL]");

                            }
                            if (ch[w].Contains("二"))
                            {
                                //注数
                                int zhu = new BCW.SFC.BLL.SfPay().countPrize(resultCID, 2) + getzhu(2, resultCID);
                                //费率
                                double lv = Convert.ToDouble(ub.GetSub("SFTwo", "/Controls/SFC.xml")) * 0.01;
                                double allr = Math.Round(Convert.ToDouble(All * lv / Convert.ToDouble(zhu)), 2);
                                long all = Convert.ToInt64(allr * getzhu(2, resultCID));
                                //添加奖池数据
                                BCW.SFC.Model.SfJackpot mo = new BCW.SFC.Model.SfJackpot();
                                mo.usID = UsID;
                                mo.WinPrize = all;
                                mo.Prize = 0;
                                mo.other = "中二等奖" + Convert.ToString(all);
                                mo.allmoney = new BCW.SFC.BLL.SfJackpot().Getallmoney(resultCID) - all;
                                mo.AddTime = DateTime.Now;
                                mo.CID = resultCID;
                                new BCW.SFC.BLL.SfJackpot().Add(mo);
                                BCW.Data.SqlHelper.ExecuteSql("update tb_SFPay set WinCent=(( select WinCent from tb_SfPay where id=" + pid + ")+" + all + ") Where id=" + pid + "");
                                //动态
                                string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + new BCW.BLL.User().GetUsName(UsID) + "[/url]在第" + resultCID + "期[url=/bbs/game/SFC.aspx]" + GameName + "[/url]中二等奖" + all + "" + ub.Get("SiteBz") + "";
                                new BCW.BLL.Action().Add(1016, pid, UsID, "", wText);
                                //发送内线
                                new BCW.BLL.Guest().Add(1, UsID, "", "恭喜您在第" + resultCID + "期" + "[URL=/bbs/game/SFC.aspx]" + ub.GetSub("SFName", "/Controls/SFC.xml") + "[/URL]" + "投注，中二等奖奖" + all + "" + ub.Get("SiteBz") + "，开奖为" + Result + "[URL=/bbs/game/SFC.aspx?act=case]马上兑奖[/URL]");

                            }
                            if (ch[w].Contains("三"))
                            {
                                //注数
                                int zhu = new BCW.SFC.BLL.SfPay().countPrize(resultCID, 3) + getzhu(3, resultCID);
                                //费率
                                double lv = Convert.ToDouble(ub.GetSub("SFThree", "/Controls/SFC.xml")) * 0.01;
                                double allr = Math.Round(Convert.ToDouble(All * lv / Convert.ToDouble(zhu)), 2);
                                long all = Convert.ToInt64(allr * getzhu(3, resultCID));
                                //添加奖池数据
                                BCW.SFC.Model.SfJackpot mo = new BCW.SFC.Model.SfJackpot();
                                mo.usID = UsID;
                                mo.WinPrize = all;
                                mo.Prize = 0;
                                mo.other = "中三等奖" + Convert.ToString(all);
                                mo.allmoney = new BCW.SFC.BLL.SfJackpot().Getallmoney(resultCID) - all;
                                mo.AddTime = DateTime.Now;
                                mo.CID = resultCID;
                                new BCW.SFC.BLL.SfJackpot().Add(mo);
                                BCW.Data.SqlHelper.ExecuteSql("update tb_SFPay set WinCent=(( select WinCent from tb_SfPay where id=" + pid + ")+" + all + ") Where id=" + pid + "");
                                //动态
                                string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + new BCW.BLL.User().GetUsName(UsID) + "[/url]在第" + resultCID + "期[url=/bbs/game/SFC.aspx]" + GameName + "[/url]中三等奖" + all + "" + ub.Get("SiteBz") + "";
                                new BCW.BLL.Action().Add(1016, pid, UsID, "", wText);
                                //发送内线
                                new BCW.BLL.Guest().Add(1, UsID, "", "恭喜您在第" + resultCID + "期" + "[URL=/bbs/game/SFC.aspx]" + ub.GetSub("SFName", "/Controls/SFC.xml") + "[/URL]" + "投注，中三等奖奖" + all + "" + ub.Get("SiteBz") + "，开奖为" + Result + "[URL=/bbs/game/SFC.aspx?act=case]马上兑奖[/URL]");

                            }
                            if (ch[w].Contains("四"))
                            {
                                //注数
                                int zhu = new BCW.SFC.BLL.SfPay().countPrize(resultCID, 4) + getzhu(4, resultCID);
                                //费率
                                double lv = Convert.ToDouble(ub.GetSub("SFForc", "/Controls/SFC.xml")) * 0.01;
                                double allr = Math.Round(Convert.ToDouble(All * lv / Convert.ToDouble(zhu)), 2);
                                long all = Convert.ToInt64(allr * getzhu(4, resultCID));
                                //添加奖池数据
                                BCW.SFC.Model.SfJackpot mo = new BCW.SFC.Model.SfJackpot();
                                mo.usID = UsID;
                                mo.WinPrize = all;
                                mo.Prize = 0;
                                mo.other = "中四等奖" + Convert.ToString(all);
                                mo.allmoney = new BCW.SFC.BLL.SfJackpot().Getallmoney(resultCID) - all;
                                mo.AddTime = DateTime.Now;
                                mo.CID = resultCID;
                                new BCW.SFC.BLL.SfJackpot().Add(mo);
                                BCW.Data.SqlHelper.ExecuteSql("update tb_SFPay set WinCent=(( select WinCent from tb_SfPay where id=" + pid + ")+" + all + ") Where id=" + pid + "");  //动态
                                string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + new BCW.BLL.User().GetUsName(UsID) + "[/url]在第" + resultCID + "期[url=/bbs/game/SFC.aspx]" + GameName + "[/url]中四等奖" + all + "" + ub.Get("SiteBz") + "";
                                new BCW.BLL.Action().Add(1016, pid, UsID, "", wText);
                                //发送内线
                                new BCW.BLL.Guest().Add(1, UsID, "", "恭喜您在第" + resultCID + "期" + "[URL=/bbs/game/SFC.aspx]" + ub.GetSub("SFName", "/Controls/SFC.xml") + "[/URL]" + "投注，中四等奖奖" + all + "" + ub.Get("SiteBz") + "，开奖为" + Result + "[URL=/bbs/game/SFC.aspx?act=case]马上兑奖[/URL]");

                            }
                            if (ch[w].Contains("五"))
                            {
                                //注数
                                int zhu = new BCW.SFC.BLL.SfPay().countPrize(resultCID, 5) + getzhu(5, resultCID);
                                //费率
                                double lv = Convert.ToDouble(ub.GetSub("SFFive", "/Controls/SFC.xml")) * 0.01;
                                double allr = Math.Round(Convert.ToDouble(All * lv / Convert.ToDouble(zhu)), 2);
                                long all = Convert.ToInt64(allr * getzhu(5, resultCID));
                                //添加奖池数据
                                BCW.SFC.Model.SfJackpot mo = new BCW.SFC.Model.SfJackpot();
                                mo.usID = UsID;
                                mo.WinPrize = all;
                                mo.Prize = 0;
                                mo.other = "中五等奖" + Convert.ToString(all);
                                mo.allmoney = new BCW.SFC.BLL.SfJackpot().Getallmoney(resultCID) - all;
                                mo.AddTime = DateTime.Now;
                                mo.CID = resultCID;
                                new BCW.SFC.BLL.SfJackpot().Add(mo);
                                BCW.Data.SqlHelper.ExecuteSql("update tb_SFPay set WinCent=(( select WinCent from tb_SfPay where id=" + pid + ")+" + all + ") Where id=" + pid + "");
                                //动态
                                string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + new BCW.BLL.User().GetUsName(UsID) + "[/url]在第" + resultCID + "期[url=/bbs/game/SFC.aspx]" + GameName + "[/url]中五等奖" + all + "" + ub.Get("SiteBz") + "";
                                new BCW.BLL.Action().Add(1016, pid, UsID, "", wText);
                                //发送内线
                                new BCW.BLL.Guest().Add(1, UsID, "", "恭喜您在第" + resultCID + "期" + "[URL=/bbs/game/SFC.aspx]" + ub.GetSub("SFName", "/Controls/SFC.xml") + "[/URL]" + "投注，中五等奖奖" + all + "" + ub.Get("SiteBz") + "，开奖为" + Result + "[URL=/bbs/game/SFC.aspx?act=case]马上兑奖[/URL]");

                            }
                            if (ch[w].Contains("六"))
                            {
                                //注数
                                int zhu = new BCW.SFC.BLL.SfPay().countPrize(resultCID, 6) + getzhu(6, resultCID);
                                //费率
                                double lv = Convert.ToDouble(ub.GetSub("SFSix", "/Controls/SFC.xml")) * 0.01;
                                double allr = Math.Round(Convert.ToDouble(All * lv / Convert.ToDouble(zhu)), 2);
                                long all = Convert.ToInt64(allr * getzhu(6, resultCID));
                                //添加奖池数据
                                BCW.SFC.Model.SfJackpot mo = new BCW.SFC.Model.SfJackpot();
                                mo.usID = UsID;
                                mo.WinPrize = all;
                                mo.Prize = 0;
                                mo.other = "中六等奖" + Convert.ToString(all);
                                mo.allmoney = new BCW.SFC.BLL.SfJackpot().Getallmoney(resultCID) - all;
                                mo.AddTime = DateTime.Now;
                                mo.CID = resultCID;
                                new BCW.SFC.BLL.SfJackpot().Add(mo);
                                BCW.Data.SqlHelper.ExecuteSql("update tb_SFPay set WinCent=(( select WinCent from tb_SfPay where id=" + pid + ")+" + all + ") Where id=" + pid + "");
                                //动态
                                string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + new BCW.BLL.User().GetUsName(UsID) + "[/url]在第" + resultCID + "期[url=/bbs/game/SFC.aspx]" + GameName + "[/url]中六等奖" + all + "" + ub.Get("SiteBz") + "";
                                new BCW.BLL.Action().Add(1016, pid, UsID, "", wText);
                                //发送内线
                                new BCW.BLL.Guest().Add(1, UsID, "", "恭喜您在第" + resultCID + "期" + "[URL=/bbs/game/SFC.aspx]" + ub.GetSub("SFName", "/Controls/SFC.xml") + "[/URL]" + "投注，中六等奖奖" + all + "" + ub.Get("SiteBz") + "，开奖为" + Result + "[URL=/bbs/game/SFC.aspx?act=case]马上兑奖[/URL]");

                            }
                            if (ch[w].Contains("七"))
                            {
                                //注数
                                int zhu = new BCW.SFC.BLL.SfPay().countPrize(resultCID, 7) + getzhu(7, resultCID);
                                //费率
                                double lv = Convert.ToDouble(ub.GetSub("SFSeven", "/Controls/SFC.xml")) * 0.01;
                                double allr = Math.Round(Convert.ToDouble(All * lv / Convert.ToDouble(zhu)), 2);
                                long all = Convert.ToInt64(allr * getzhu(7, resultCID));
                                //添加奖池数据
                                BCW.SFC.Model.SfJackpot mo = new BCW.SFC.Model.SfJackpot();
                                mo.usID = UsID;
                                mo.WinPrize = all;
                                mo.Prize = 0;
                                mo.other = "中七等奖" + Convert.ToString(all);
                                mo.allmoney = new BCW.SFC.BLL.SfJackpot().Getallmoney(resultCID) - all;
                                mo.AddTime = DateTime.Now;
                                mo.CID = resultCID;
                                new BCW.SFC.BLL.SfJackpot().Add(mo);
                                BCW.Data.SqlHelper.ExecuteSql("update tb_SFPay set WinCent=(( select WinCent from tb_SfPay where id=" + pid + ")+" + all + ") Where id=" + pid + "");
                                //动态
                                string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + new BCW.BLL.User().GetUsName(UsID) + "[/url]在第" + resultCID + "期[url=/bbs/game/SFC.aspx]" + GameName + "[/url]中七等奖" + all + "" + ub.Get("SiteBz") + "";
                                new BCW.BLL.Action().Add(1016, pid, UsID, "", wText);
                                //发送内线
                                new BCW.BLL.Guest().Add(1, UsID, "", "恭喜您在第" + resultCID + "期" + "[URL=/bbs/game/SFC.aspx]" + ub.GetSub("SFName", "/Controls/SFC.xml") + "[/URL]" + "投注，中七等奖奖" + all + "" + ub.Get("SiteBz") + "，开奖为" + Result + "[URL=/bbs/game/SFC.aspx?act=case]马上兑奖[/URL]");

                            }
                        }
                    }
                    #endregion
                }
            }
        }
        #endregion

        #region 更新奖池结余与系统收取手续
        if (new BCW.SFC.DAL.SfList().getState(resultCID) == 0)
        {
            //更新当期系统结余奖池
            new BCW.SFC.DAL.SfList().UpdateNextprize(resultCID, NextPrize(resultCID));

            //更新当期系统收取手续
            new BCW.SFC.DAL.SfList().Updatesysdayprize(resultCID, Convert.ToInt64(Allmoney * Convert.ToInt64(ub.GetSub("SFsys", "/Controls/SFC.xml")) * 0.01));
        }
        #endregion

        #region 奖池滚存
        if (!new BCW.SFC.DAL.SfJackpot().Existsgun(resultCID))
        {
            //把奖池滚存记录下来
            BCW.SFC.Model.SfJackpot mos = new BCW.SFC.Model.SfJackpot();
            mos.usID = 5;
            mos.WinPrize = 0;
            mos.Prize = 0;
            mos.other = "第" + resultCID + "期滚存" + NextPrize(resultCID) + ub.Get("SiteBz") + "到" + (resultCID + 1) + "期|结余0" + ub.Get("SiteBz");
            mos.allmoney = 0;
            mos.AddTime = DateTime.Now;
            mos.CID = resultCID;
            new BCW.SFC.BLL.SfJackpot().Add(mos);
        }
        if (!new BCW.SFC.DAL.SfJackpot().Existsgun1(resultCID + 1))
        {
            //把奖池滚存记录下来
            BCW.SFC.Model.SfJackpot mos = new BCW.SFC.Model.SfJackpot();
            mos.usID = 6;
            mos.WinPrize = 0;
            mos.Prize = 0;
            mos.other = "得到第" + resultCID + "期滚存" + NextPrize(resultCID) + ub.Get("SiteBz") + "|结余" + NextPrize(resultCID) + "" + ub.Get("SiteBz");
            mos.allmoney = NextPrize(resultCID);
            mos.AddTime = Convert.ToDateTime("2000-10-10 10:10:10");
            mos.CID = (resultCID + 1);
            new BCW.SFC.BLL.SfJackpot().Add(mos);
        }
        #endregion

        #region 遍历奖池表 （SFJackpot）,更新预售期的奖池
        //遍历表SFJackpot，更新预售期的奖池
        if (new BCW.SFC.DAL.SfList().GetMaxId() != resultCID)
        {
            DataSet nextPP = new BCW.SFC.BLL.SfJackpot().GetList("id,usID,Prize,WinPrize,other,allmoney,AddTime,CID", "CID=" + (resultCID + 1) + " ");

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
                    //int CID = int.Parse(nextPP.Tables[0].Rows[i]["CID"].ToString());

                    long PP = 0;
                    if (new BCW.SFC.BLL.SfList().getsysstate((resultCID + 1)) == 1)
                    {
                        PP = (allmoney + NextPrize(resultCID) + Convert.ToInt64(ub.GetSub("SFCSend", "/Controls/SFC.xml")));
                    }
                    else
                    {
                        PP = (allmoney + NextPrize(resultCID));
                    }

                    //更新奖池
                    //new BCW.SFC.DAL.SfJackpot().updateallmoney(PP, (resultCID + 1), UsID,id);
                    if (other.Contains("预售"))
                    {
                        BCW.Data.SqlHelper.ExecuteSql("update tb_SfJackpot set allmoney='" + PP + "' Where id='" + pid + "' ");
                        BCW.Data.SqlHelper.ExecuteSql("update tb_SfJackpot set other=replace(other,'预售','') where id='" + pid + "' ");
                    }
                }

            }
            else
            {

            }
        }
        #endregion

        #region 判断系统投入
        if (new BCW.SFC.DAL.SfList().getState(resultCID) == 0)
        {
            if (new BCW.SFC.BLL.SfList().ExistsCID((resultCID + 1)))
            {
                long paycent = new BCW.SFC.BLL.SfList().GetPrice("sum(PayCent)", " CID =" + (resultCID + 1) + " ");//消费
                if ((NextPrize(resultCID) + paycent) < Convert.ToInt64(ub.GetSub("SFMin", "/Controls/SFC.xml")))
                {
                    new BCW.SFC.BLL.SfList().UpdateSysstaprize((resultCID + 1), 1, Convert.ToInt64(ub.GetSub("SFCSend", "/Controls/SFC.xml")));

                    //把记录加到奖池表
                    BCW.SFC.Model.SfJackpot mo = new BCW.SFC.Model.SfJackpot();
                    mo.usID = 1;//1表示系统有投入
                    mo.WinPrize = 0;
                    mo.Prize = Convert.ToInt64(ub.GetSub("SFCSend", "/Controls/SFC.xml"));
                    mo.other = "系统投入" + Convert.ToInt64(ub.GetSub("SFCSend", "/Controls/SFC.xml"));
                    mo.allmoney = (Convert.ToInt64(ub.GetSub("SFCSend", "/Controls/SFC.xml")) + NextPrize(resultCID)) + paycent;
                    mo.AddTime = DateTime.Now;
                    mo.CID = (resultCID + 1);
                    new BCW.SFC.BLL.SfJackpot().Add(mo);
                }
            }
        }
        #endregion

        #region 结束更新
        if (new BCW.SFC.DAL.SfList().getState(resultCID) == 0)
        {
            //完成返彩后正式更新该期为结束
            BCW.Data.SqlHelper.ExecuteSql("update tb_SfList set State=1 Where CID=" + resultCID + "");
            BCW.Data.SqlHelper.ExecuteSql("update tb_SFPay set State=1 Where CID=" + resultCID + "");
        }
        #endregion
        #endregion
    }

    public bool True(string votenum, string resultnum)
    {
        if ((votenum).Contains(resultnum))
            return true;
        if (resultnum == "*")
            return true;
        return false;
    }
    //几等奖总注数
    private int getzhu(int j, int CID)
    {
        string str = string.Empty;
        int zhu = 0;
        int sum = 0; int sum2 = 0; int sum3 = 0; int sum4 = 0; int sum5 = 0; int sum6 = 0; int sum7 = 0;
        DataSet pay = new BCW.SFC.BLL.SfPay().GetList("usID,ISPrize,VoteNum,OverRide,id,change", "CID=" + CID + " and State=0");
        if (pay != null && pay.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < pay.Tables[0].Rows.Count; i++)
            {
                int OverRide = 0; int OverRide2 = 0; int OverRide3 = 0; int OverRide4 = 0; int OverRide5 = 0; int OverRide6 = 0; int OverRide7 = 0;
                int num1 = 0; int num2 = 0; int num3 = 0; int num4 = 0; int num5 = 0; int num6 = 0; int num7 = 0;
                int pid = int.Parse(pay.Tables[0].Rows[i]["id"].ToString());
                int ISPrize = int.Parse(pay.Tables[0].Rows[i]["ISPrize"].ToString());
                int UsID = int.Parse(pay.Tables[0].Rows[i]["usID"].ToString());
                int VoteNum = int.Parse(pay.Tables[0].Rows[i]["VoteNum"].ToString());

                string change = pay.Tables[0].Rows[i]["change"].ToString();
                int overridebyid = new BCW.SFC.BLL.SfPay().VoteNum(pid, CID);

                if (change != null || change != " ")
                {
                    string[] ch = change.Split('#');
                    for (int w = 0; w < ch.Length; w++)
                    {
                        if (ch[w].Contains("一"))
                        {
                            OverRide = int.Parse(pay.Tables[0].Rows[i]["OverRide"].ToString());
                            str = ch[w];
                            string[] nn = str.Split('/');
                            num1 = Convert.ToInt32(nn[1]);
                        }
                        if (ch[w].Contains("二"))
                        {
                            OverRide2 = int.Parse(pay.Tables[0].Rows[i]["OverRide"].ToString());
                            str = ch[w];
                            string[] nn = str.Split('/');
                            num2 = Convert.ToInt32(nn[1]);
                        }
                        if (ch[w].Contains("三"))
                        {
                            OverRide3 = int.Parse(pay.Tables[0].Rows[i]["OverRide"].ToString());
                            str = ch[w];
                            string[] nn = str.Split('/');
                            num3 = Convert.ToInt32(nn[1]);
                        }
                        if (ch[w].Contains("四"))
                        {
                            OverRide4 = int.Parse(pay.Tables[0].Rows[i]["OverRide"].ToString());
                            str = ch[w];
                            string[] nn = str.Split('/');
                            num4 = Convert.ToInt32(nn[1]);
                        }
                        if (ch[w].Contains("五"))
                        {
                            OverRide5 = int.Parse(pay.Tables[0].Rows[i]["OverRide"].ToString());
                            str = ch[w];
                            string[] nn = str.Split('/');
                            num5 = Convert.ToInt32(nn[1]);
                        }
                        if (ch[w].Contains("六"))
                        {
                            OverRide6 = int.Parse(pay.Tables[0].Rows[i]["OverRide"].ToString());
                            str = ch[w];
                            string[] nn = str.Split('/');
                            num6 = Convert.ToInt32(nn[1]);
                        }
                        if (ch[w].Contains("七"))
                        {
                            OverRide7 = int.Parse(pay.Tables[0].Rows[i]["OverRide"].ToString());
                            str = ch[w];
                            string[] nn = str.Split('/');
                            num7 = Convert.ToInt32(nn[1]);
                        }
                    }

                }
                if (j == 1)
                {
                    zhu = (num1 * OverRide);
                    sum += zhu;
                }
                if (j == 2)
                {
                    zhu = (num2 * OverRide2);
                    sum2 += zhu;
                }
                if (j == 3)
                {
                    zhu = (num3 * OverRide3);
                    sum3 += zhu;
                }
                if (j == 4)
                {
                    zhu = (num4 * OverRide4);
                    sum4 += zhu;
                }
                if (j == 5)
                {
                    zhu = (num5 * OverRide5);
                    sum5 += zhu;
                }
                if (j == 6)
                {
                    zhu = (num6 * OverRide6);
                    sum6 += zhu;
                }
                if (j == 7)
                {
                    zhu = (num7 * OverRide7);
                    sum7 += zhu;
                }
            }
        }
        if (j == 1)
        {
            zhu = sum;
        }
        if (j == 2)
        {
            zhu = sum2;
        }
        if (j == 3)
        {
            zhu = sum3;
        }
        if (j == 4)
        {
            zhu = sum4;
        }
        if (j == 5)
        {
            zhu = sum5;
        }
        if (j == 6)
        {
            zhu = sum6;
        }
        if (j == 7)
        {
            zhu = sum7;
        }
        return zhu;

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
                matchs.sale_start = Convert.ToDateTime("");
                matchs.sale_end = Convert.ToDateTime("");
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
        string url = "http://s.apiplus.cn/sf/?token=2bbadaee337d9127";//xml接口
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
        public DateTime sale_start;
        public DateTime sale_end;
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
        long AllPrice = new BCW.SFC.BLL.SfPay().AllPrice(resultCID);
        //获取已经派出奖金
        long _Price = new BCW.SFC.BLL.SfPay().AllWinCentbyCID(resultCID);
        //获取当前期数系统投注总额
        //long SysPrice = new BCW.SFC.BLL.SfJackpot().SysPrice();
        long Sysprize = new BCW.SFC.DAL.SfList().getsysprize(resultCID);
        //获取当期系统投注状态
        int Sysprizestatue = new BCW.SFC.DAL.SfList().getsysprizestatue(resultCID);
        //获取上一期滚存下来的奖池
        int lastcid = 0;
        if (new BCW.SFC.BLL.SfList().ExistsCID(resultCID - 1))
        {
            lastcid = (resultCID - 1);
        }
        else
        {
            lastcid = LastOpenCID();
        }
        long Nextprize = new BCW.SFC.DAL.SfList().getnextprize(lastcid);

        //获取当前期数系统回收总额
        long SysWin = new BCW.SFC.BLL.SfJackpot().SysWin(resultCID);
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
        long nowprize = new BCW.SFC.BLL.SfList().nowprize(resultCID);
        //获取已经派出奖金
        long _Price = new BCW.SFC.BLL.SfPay().AllWinCentbyCID(resultCID);
        long sysprizeshouxu = Convert.ToInt64(nowprize * Convert.ToInt64(ub.GetSub("SFsys", "/Controls/SFC.xml")) * 0.01);
        long Prices = nowprize - _Price - sysprizeshouxu;//当期结余=当期奖池-当期系统收取-当期派奖
        return Prices;
    }
    // 获得当期剩余奖池（为每一次减去中奖额减去系统回收）
    private long NowPrize(int resultCID)
    {
        long nowprize1 = new BCW.SFC.BLL.SfList().nowprize(resultCID);
        long sysprizeshouxu = Convert.ToInt64(nowprize1 * Convert.ToInt64(ub.GetSub("SFsys", "/Controls/SFC.xml")) * 0.01);
        long Prices = nowprize1 - sysprizeshouxu;//当期结余=当期奖池-当期系统收取-当期派奖
        return Prices;
    }
    //获取数据库最新已经开奖期号
    private int LastOpenCID()
    {
        try
        {
            int CID = 0;
            DataSet ds = new BCW.SFC.BLL.SfList().GetList("CID", " State=1 Order by CID Desc ");
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
            DataSet ds = new BCW.SFC.BLL.SfList().GetList("CID", " State=0 Order by CID Asc ");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                CID = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
            }
            return CID;
        }
        catch { return 0; }
    }
}
