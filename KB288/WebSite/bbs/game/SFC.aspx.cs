using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BCW.Common;
using System.Collections;
using System.Configuration;
using System.Data;
using BCW.SFC;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Text.RegularExpressions;

/// <summary>
/// 20160924  蒙宗将  无开奖状态
/// 20161007 蒙宗将 优化获取上期
///  蒙宗将 20161011 奖池修改显示
///  蒙宗将 20161025 新倒计时
///  蒙宗将 首页重新排版 快捷下注 20161027
///  蒙宗将 快捷下注转换成万 20161028
///  蒙宗将 下注验证期号选号信息 20161107
/// 蒙宗将 20161112 优化开奖
/// </summary>

public partial class bbs_game_SFC : System.Web.UI.Page
{
    protected string xmlPath = "/Controls/SFC.xml";
    protected string SFStatus = ub.GetSub("SFStatus", "/Controls/SFC.xml");//测试状态
    protected string SFCDemoIDS = ub.GetSub("SFCDemoIDS", "/Controls/SFC.xml");
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    int meid = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        //维护提示
        if (ub.GetSub("SFStatus", xmlPath) == "1")
        {
            Utils.Safe("此游戏");
        }

        //内测判断 0,内测是否开启1，是否为内测账号
        if (ub.GetSub("SFStatus", xmlPath) == "2")//内测
        {
            string[] sNum = Regex.Split(SFCDemoIDS, "#");
            int sbsy = 0;
            for (int a = 0; a < sNum.Length; a++)
            {
                if (new BCW.User.Users().GetUsId() == int.Parse(sNum[a].Trim()))
                {
                    sbsy++;
                }
            }
            if (sbsy == 0)
            {
                Utils.Error("内测中..你没获得测试的资格，谢谢。", "");
            }
        }

        Master.Title = ub.GetSub("SFName", xmlPath);
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "rule":
                RulePage();
                break;
            case "info":
                InfoPage();
                break;
            case "pay":
                PayPage();
                break;
            case "list":
                ListPage();
                break;
            case "mylist":
                MyListPage();
                break;
            case "listview":
                ListViewPage();
                break;
            case "case":
                CasePage();
                break;
            case "caseok":
                CaseOkPage();
                break;
            case "casepost":
                CasePostPage();
                break;
            case "top":
                TopPage();
                break;
            case "prizelist":
                PrizeListPage();
                break;
            case "prize":
                PrizePage();
                break;
            case "trends":
                TrendsPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    #region 胜负彩首页
    private void ReloadPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href =\"" + Utils.getUrl("SFC.aspx") + "\">" + ub.GetSub("SFName", xmlPath) + "</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        string TopUbb = ub.GetSub("SFTopUbb", xmlPath);
        if (TopUbb != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.SysUBB(TopUbb) + "");
            builder.Append(Out.Tab("</div>", "<br />"));
        }

        builder.Append(Out.Tab("<div>", ""));//【" + ub.GetSub("SFName", xmlPath) + "】
        builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx?act=rule&amp;backurl=" + Utils.PostPage(1)) + "\">规则</a>" + " | <a href =\"" + Utils.getUrl("SFC.aspx?act=case") + "\">兑奖</a>");
        builder.Append(" | <a href =\"" + Utils.getUrl("SFC.aspx?act=mylist&amp;id=0") + "\">记录</a>");   //     builder.Append("|<a href =\"" + Utils.getUrl("SFC.aspx?act=mylist&amp;id=0") + "\">未开</a>");
        builder.Append(" | <a href =\"" + Utils.getUrl("SFC.aspx?act=top&amp;id=0") + "\">排行</a>");//        builder.Append("|<a href =\"" + Utils.getUrl("SFC.aspx?act=top&amp;id=1") + "\">好运榜</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        //未开奖当前投注期号
        DataSet ds = new BCW.SFC.BLL.SfList().GetList("TOP 1 CID", "State=0 order by CID ASC");
        DataSet dsas = new BCW.SFC.BLL.SfList().GetList("TOP 1 CID", "State=1 order by CID DESC");
        int CID = 0;
        DateTime AddTime = DateTime.Now;

        try
        {
            CID = int.Parse(ds.Tables[0].Rows[0][0].ToString());
        }
        catch
        {
            CID = int.Parse(dsas.Tables[0].Rows[0][0].ToString());
        }
        //if (CID == 0)
        //{
        //    //未开奖当前投注期号
        //    CID = new BCW.SFC.DAL.SfPay().GetMaxCID() + 1;
        //}

        builder.Append("当期(" + CID + ")奖池滚存：" + AllPrize(CID) + "" + ub.Get("SiteBz") + "<br />");//<a href=\"" + Utils.getUrl("SFC.aspx?act=prize") + "\"></a>

        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", ""));
        DataSet qi = new BCW.SFC.BLL.SfList().GetList("CID", " State=0 and Sale_StartTime < '" + DateTime.Now + "' and EndTime > '" + DateTime.Now + "'");
        for (int i2 = 0; i2 < qi.Tables[0].Rows.Count; i2++)
        {
            int qishu = Convert.ToInt32(qi.Tables[0].Rows[i2][0]);
            BCW.SFC.Model.SfList mo = new BCW.SFC.BLL.SfList().GetSfList1(qishu);
            //builder.Append(qishu);
            if (mo.Sale_StartTime < DateTime.Now && mo.EndTime > DateTime.Now)
            {
                if (i2 == (qi.Tables[0].Rows.Count - 1))
                {
                    builder.Append("<a href =\"" + Utils.getUrl("SFC.aspx?act=info&amp;id=" + i2 + "") + "\"><b>" + qishu + "期</b></a>");
                }
                else
                {
                    builder.Append("<a href =\"" + Utils.getUrl("SFC.aspx?act=info&amp;id=" + i2 + "") + "\"><b>" + qishu + "期</b></a>|");
                }
            }
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        #region 期数倒计时
        //builder.Append(Out.Tab("<div>", ""));
        //for (int i2 = 0; i2 < qi.Tables[0].Rows.Count; i2++)
        //{
        //    int qishu = Convert.ToInt32(qi.Tables[0].Rows[i2][0]);
        //    BCW.SFC.Model.SfList mo = new BCW.SFC.BLL.SfList().GetSfList1(qishu);
        //    if (i2 == (qi.Tables[0].Rows.Count - 1))
        //    {
        //        if (mo.Sale_StartTime < DateTime.Now)
        //        {
        //            if (mo.EndTime < DateTime.Now)
        //            {
        //             //   builder.Append("第" + mo.CID + "期投注截止请等待系统开奖...<br />");
        //            }
        //            else
        //            {
        //                    string SFC = new BCW.JS.somejs().newDaojishi("" + i2 + "", mo.EndTime.AddMinutes(10).AddSeconds(-10));
        //                    builder.Append("第" + mo.CID + "期投注进行中,<br />距离截止时间还有" + SFC + "<br />");
        //                    //本期已下注" + mo.PayCent + "" + ub.Get("SiteBz") + "(" + mo.PayCount + "注)<br />
        //            }
        //        }
        //        else
        //        {
        //          //  builder.Append("第" + mo.CID + "期请等待投注开始...<br />");
        //        }
        //    }
        //    else
        //    {
        //        if (mo.Sale_StartTime < DateTime.Now)
        //        {
        //            if (mo.EndTime < DateTime.Now)
        //            {
        //             //   builder.Append("第" + mo.CID + "期投注截止请等待系统开奖...<br />");
        //            }
        //            else
        //            {

        //                    string SFC = new BCW.JS.somejs().newDaojishi("" + i2 + "", mo.EndTime.AddMinutes(10).AddSeconds(-10));
        //                    builder.Append("第" + mo.CID + "期投注进行中,<br />距离截止时间还有" + SFC + "<br />");//本期已下注" + mo.PayCent + "" + ub.Get("SiteBz") + "(" + mo.PayCount + "注)<br />
        //                    builder.Append("" + "-----------" + "<br />");

        //            }
        //        }
        //        else
        //        {
        //          //  builder.Append("第" + mo.CID + "期请等待投注开始...<br />");
        //        }
        //    }
        //}
        # endregion

        #region
        builder.Append(Out.Tab("<div>", ""));
        //开奖最新期号
        builder.Append("【最新开奖】<br />");
        string act = Utils.GetRequest("act", "post", 1, @"^", "");
        int CID3c = int.Parse(Utils.GetRequest("number1", "post", 1, @"^", "0"));
        if (act == "ok")
        {
            if (CID3c == 0 || CID3c == null)
            {
                CID3c = new BCW.SFC.BLL.SfList().CID();
            }
            builder.Append("第" + CID3c + "期开奖情况<br />");
            builder.Append("该期奖池：" + new BCW.SFC.BLL.SfList().nowprize(CID3c) + ub.Get("SiteBz") + "<br />");
            //得到当前奖池
            long All = new BCW.SFC.BLL.SfList().nowprize(CID3c);

            int zhu1 = (new BCW.SFC.BLL.SfPay().countPrize(CID3c, 1) + getzhuh(1, CID3c));
            int zhu2 = (new BCW.SFC.BLL.SfPay().countPrize(CID3c, 2) + getzhuh(2, CID3c));
            int zhu3 = (new BCW.SFC.BLL.SfPay().countPrize(CID3c, 3) + getzhuh(3, CID3c));
            int zhu4 = (new BCW.SFC.BLL.SfPay().countPrize(CID3c, 4) + getzhuh(4, CID3c));
            int zhu5 = (new BCW.SFC.BLL.SfPay().countPrize(CID3c, 5) + getzhuh(5, CID3c));
            int zhu6 = (new BCW.SFC.BLL.SfPay().countPrize(CID3c, 6) + getzhuh(6, CID3c));
            int zhu7 = (new BCW.SFC.BLL.SfPay().countPrize(CID3c, 7) + getzhuh(7, CID3c));
            if (zhu1 == 0)
            {
                builder.Append("[一等奖]：" + "<a href =\"" + Utils.getUrl("SFC.aspx?act=prizelist&amp;IsPrize=" + 1 + "&amp;CID=" + CID3c + "") + "\">" + getzhuh(1, CID3c) + "</a> " + "注 每注0 " + ub.Get("SiteBz") + "<br />");

            }
            else
            {
                //费率
                double lv = Convert.ToDouble(ub.GetSub("SFOne", "/Controls/SFC.xml")) * 0.01;
                double allr = Math.Round(Convert.ToDouble(All * lv / Convert.ToDouble(zhu1)), 2);
                builder.Append("[一等奖]：" + "<a href =\"" + Utils.getUrl("SFC.aspx?act=prizelist&amp;IsPrize=" + 1 + "&amp;CID=" + CID3c + "") + "\">" + (getzhuh(1, CID3c) + new BCW.SFC.BLL.SfPay().countPrize(CID3c, 1)) + "</a> " + "注 每注 " + allr + " " + ub.Get("SiteBz") + "<br />");

            }
            if (zhu2 == 0)
            {
                builder.Append("[二等奖]：" + "<a href =\"" + Utils.getUrl("SFC.aspx?act=prizelist&amp;IsPrize=" + 2 + "&amp;CID=" + CID3c + "") + "\">" + getzhuh(2, CID3c) + "</a> " + "注 每注0 " + ub.Get("SiteBz") + "<br />");

            }
            else
            {
                //费率
                double lv = Convert.ToDouble(ub.GetSub("SFTwo", "/Controls/SFC.xml")) * 0.01;
                double allr = Math.Round(Convert.ToDouble(All * lv / Convert.ToDouble(zhu2)), 2);
                builder.Append("[二等奖]：" + "<a href =\"" + Utils.getUrl("SFC.aspx?act=prizelist&amp;IsPrize=" + 2 + "&amp;CID=" + CID3c + "") + "\">" + (getzhuh(2, CID3c) + new BCW.SFC.BLL.SfPay().countPrize(CID3c, 2)) + "</a> " + "注 每注 " + allr + " " + ub.Get("SiteBz") + "<br />");

            }
            if (zhu3 == 0)
            {
                builder.Append("[三等奖]：" + "<a href =\"" + Utils.getUrl("SFC.aspx?act=prizelist&amp;IsPrize=" + 3 + "&amp;CID=" + CID3c + "") + "\">" + getzhuh(3, CID3c) + "</a> " + "注 每注0 " + ub.Get("SiteBz") + "<br />");

            }
            else
            {
                //费率
                double lv = Convert.ToDouble(ub.GetSub("SFThree", "/Controls/SFC.xml")) * 0.01;
                double allr = Math.Round(Convert.ToDouble(All * lv / Convert.ToDouble(zhu3)), 2);
                builder.Append("[三等奖]：" + "<a href =\"" + Utils.getUrl("SFC.aspx?act=prizelist&amp;IsPrize=" + 3 + "&amp;CID=" + CID3c + "") + "\">" + (getzhuh(3, CID3c) + new BCW.SFC.BLL.SfPay().countPrize(CID3c, 3)) + "</a> " + "注 每注 " + allr + " " + ub.Get("SiteBz") + "<br />");

            }
            if (zhu4 == 0)
            {
                builder.Append("[四等奖]：" + "<a href =\"" + Utils.getUrl("SFC.aspx?act=prizelist&amp;IsPrize=" + 4 + "&amp;CID=" + CID3c + "") + "\">" + getzhuh(4, CID3c) + "</a> " + "注 每注0 " + ub.Get("SiteBz") + "<br />");

            }
            else
            {
                //费率
                double lv = Convert.ToDouble(ub.GetSub("SFForc", "/Controls/SFC.xml")) * 0.01;
                double allr = Math.Round(Convert.ToDouble(All * lv / Convert.ToDouble(zhu4)), 2);
                builder.Append("[四等奖]：" + "<a href =\"" + Utils.getUrl("SFC.aspx?act=prizelist&amp;IsPrize=" + 4 + "&amp;CID=" + CID3c + "") + "\">" + (getzhuh(4, CID3c) + new BCW.SFC.BLL.SfPay().countPrize(CID3c, 4)) + "</a> " + "注 每注 " + allr + " " + ub.Get("SiteBz") + "<br />");

            }
            if (zhu5 == 0)
            {
                builder.Append("[五等奖]：" + "<a href =\"" + Utils.getUrl("SFC.aspx?act=prizelist&amp;IsPrize=" + 5 + "&amp;CID=" + CID3c + "") + "\">" + getzhuh(5, CID3c) + "</a> " + "注 每注0 " + ub.Get("SiteBz") + "<br />");
            }
            else
            {
                //费率
                double lv = Convert.ToDouble(ub.GetSub("SFFive", "/Controls/SFC.xml")) * 0.01;
                double allr = Math.Round(Convert.ToDouble(All * lv / Convert.ToDouble(zhu5)), 2);
                builder.Append("[五等奖]：" + "<a href =\"" + Utils.getUrl("SFC.aspx?act=prizelist&amp;IsPrize=" + 5 + "&amp;CID=" + CID3c + "") + "\">" + (getzhuh(5, CID3c) + new BCW.SFC.BLL.SfPay().countPrize(CID3c, 5)) + "</a> " + "注 每注 " + allr + " " + ub.Get("SiteBz") + "<br />");

            }
            if (zhu6 == 0)
            {
                builder.Append("[六等奖]：" + "<a href =\"" + Utils.getUrl("SFC.aspx?act=prizelist&amp;IsPrize=" + 6 + "&amp;CID=" + CID3c + "") + "\">" + getzhuh(6, CID3c) + "</a> " + "注 每注0 " + ub.Get("SiteBz") + "<br />");
            }
            else
            {
                //费率
                double lv = Convert.ToDouble(ub.GetSub("SFSix", "/Controls/SFC.xml")) * 0.01;
                double allr = Math.Round(Convert.ToDouble(All * lv / Convert.ToDouble(zhu6)), 2);
                builder.Append("[六等奖]：" + "<a href =\"" + Utils.getUrl("SFC.aspx?act=prizelist&amp;IsPrize=" + 6 + "&amp;CID=" + CID3c + "") + "\">" + (getzhuh(6, CID3c) + new BCW.SFC.BLL.SfPay().countPrize(CID3c, 6)) + "</a> " + "注 每注 " + allr + " " + ub.Get("SiteBz") + "<br />");

            }
            if (zhu7 == 0)
            {
                builder.Append("[七等奖]：" + "<a href =\"" + Utils.getUrl("SFC.aspx?act=prizelist&amp;IsPrize=" + 7 + "&amp;CID=" + CID3c + "") + "\">" + getzhuh(7, CID3c) + "</a> " + "注 每注0 " + ub.Get("SiteBz") + "");
            }
            else
            {
                //费率
                double lv = Convert.ToDouble(ub.GetSub("SFSeven", "/Controls/SFC.xml")) * 0.01;
                double allr = Math.Round(Convert.ToDouble(All * lv / Convert.ToDouble(zhu7)), 2);
                builder.Append("[七等奖]：" + "<a href =\"" + Utils.getUrl("SFC.aspx?act=prizelist&amp;IsPrize=" + 7 + "&amp;CID=" + CID3c + "") + "\">" + (getzhuh(7, CID3c) + new BCW.SFC.BLL.SfPay().countPrize(CID3c, 7)) + "</a> " + "注 每注 " + allr + " " + ub.Get("SiteBz") + "");

            }

        }
        else
        {
            int CID3 = new BCW.SFC.BLL.SfList().CID();
            builder.Append("第" + CID3 + "期开奖情况<br />");
            builder.Append("该期奖池：" + new BCW.SFC.BLL.SfList().nowprize(CID3) + ub.Get("SiteBz") + "<br />");
            //得到当前奖池
            long All = new BCW.SFC.BLL.SfList().nowprize(CID3);

            int zhu1 = (new BCW.SFC.BLL.SfPay().countPrize(CID3, 1) + getzhuh(1, CID3));
            int zhu2 = (new BCW.SFC.BLL.SfPay().countPrize(CID3, 2) + getzhuh(2, CID3));
            int zhu3 = (new BCW.SFC.BLL.SfPay().countPrize(CID3, 3) + getzhuh(3, CID3));
            int zhu4 = (new BCW.SFC.BLL.SfPay().countPrize(CID3, 4) + getzhuh(4, CID3));
            int zhu5 = (new BCW.SFC.BLL.SfPay().countPrize(CID3, 5) + getzhuh(5, CID3));
            int zhu6 = (new BCW.SFC.BLL.SfPay().countPrize(CID3, 6) + getzhuh(6, CID3));
            int zhu7 = (new BCW.SFC.BLL.SfPay().countPrize(CID3, 7) + getzhuh(7, CID3));
            if (zhu1 == 0)
            {
                builder.Append("[一等奖]：" + "<a href =\"" + Utils.getUrl("SFC.aspx?act=prizelist&amp;IsPrize=" + 1 + "&amp;CID=" + CID3 + "") + "\">" + getzhuh(1, CID3) + "</a> " + "注 每注0 " + ub.Get("SiteBz") + "<br />");

            }
            else
            {
                //费率
                double lv = Convert.ToDouble(ub.GetSub("SFOne", "/Controls/SFC.xml")) * 0.01;
                double allr = Math.Round(Convert.ToDouble(All * lv / Convert.ToDouble(zhu1)), 2);
                builder.Append("[一等奖]：" + "<a href =\"" + Utils.getUrl("SFC.aspx?act=prizelist&amp;IsPrize=" + 1 + "&amp;CID=" + CID3 + "") + "\">" + (getzhuh(1, CID3) + new BCW.SFC.BLL.SfPay().countPrize(CID3, 1)) + "</a> " + "注 每注 " + allr + " " + ub.Get("SiteBz") + "<br />");

            }
            if (zhu2 == 0)
            {
                builder.Append("[二等奖]：" + "<a href =\"" + Utils.getUrl("SFC.aspx?act=prizelist&amp;IsPrize=" + 2 + "&amp;CID=" + CID3 + "") + "\">" + getzhuh(2, CID3) + "</a> " + "注 每注0 " + ub.Get("SiteBz") + "<br />");

            }
            else
            {
                //费率
                double lv = Convert.ToDouble(ub.GetSub("SFTwo", "/Controls/SFC.xml")) * 0.01;
                double allr = Math.Round(Convert.ToDouble(All * lv / Convert.ToDouble(zhu2)), 2);
                builder.Append("[二等奖]：" + "<a href =\"" + Utils.getUrl("SFC.aspx?act=prizelist&amp;IsPrize=" + 2 + "&amp;CID=" + CID3 + "") + "\">" + (getzhuh(2, CID3) + new BCW.SFC.BLL.SfPay().countPrize(CID3, 2)) + "</a> " + "注 每注 " + allr + " " + ub.Get("SiteBz") + "<br />");

            }
            if (zhu3 == 0)
            {
                builder.Append("[三等奖]：" + "<a href =\"" + Utils.getUrl("SFC.aspx?act=prizelist&amp;IsPrize=" + 3 + "&amp;CID=" + CID3 + "") + "\">" + getzhuh(3, CID3) + "</a> " + "注 每注0 " + ub.Get("SiteBz") + "<br />");

            }
            else
            {
                //费率
                double lv = Convert.ToDouble(ub.GetSub("SFThree", "/Controls/SFC.xml")) * 0.01;
                double allr = Math.Round(Convert.ToDouble(All * lv / Convert.ToDouble(zhu3)), 2);
                builder.Append("[三等奖]：" + "<a href =\"" + Utils.getUrl("SFC.aspx?act=prizelist&amp;IsPrize=" + 3 + "&amp;CID=" + CID3 + "") + "\">" + (getzhuh(3, CID3) + new BCW.SFC.BLL.SfPay().countPrize(CID3, 3)) + "</a> " + "注 每注 " + allr + " " + ub.Get("SiteBz") + "<br />");

            }
            if (zhu4 == 0)
            {
                builder.Append("[四等奖]：" + "<a href =\"" + Utils.getUrl("SFC.aspx?act=prizelist&amp;IsPrize=" + 4 + "&amp;CID=" + CID3 + "") + "\">" + getzhuh(4, CID3) + "</a> " + "注 每注0 " + ub.Get("SiteBz") + "<br />");

            }
            else
            {
                //费率
                double lv = Convert.ToDouble(ub.GetSub("SFForc", "/Controls/SFC.xml")) * 0.01;
                double allr = Math.Round(Convert.ToDouble(All * lv / Convert.ToDouble(zhu4)), 2);
                builder.Append("[四等奖]：" + "<a href =\"" + Utils.getUrl("SFC.aspx?act=prizelist&amp;IsPrize=" + 4 + "&amp;CID=" + CID3 + "") + "\">" + (getzhuh(4, CID3) + new BCW.SFC.BLL.SfPay().countPrize(CID3, 4)) + "</a> " + "注 每注 " + allr + " " + ub.Get("SiteBz") + "<br />");

            }
            if (zhu5 == 0)
            {
                builder.Append("[五等奖]：" + "<a href =\"" + Utils.getUrl("SFC.aspx?act=prizelist&amp;IsPrize=" + 5 + "&amp;CID=" + CID3 + "") + "\">" + getzhuh(5, CID3) + "</a> " + "注 每注0 " + ub.Get("SiteBz") + "<br />");
            }
            else
            {
                //费率
                double lv = Convert.ToDouble(ub.GetSub("SFFive", "/Controls/SFC.xml")) * 0.01;
                double allr = Math.Round(Convert.ToDouble(All * lv / Convert.ToDouble(zhu5)), 2);
                builder.Append("[五等奖]：" + "<a href =\"" + Utils.getUrl("SFC.aspx?act=prizelist&amp;IsPrize=" + 5 + "&amp;CID=" + CID3 + "") + "\">" + (getzhuh(5, CID3) + new BCW.SFC.BLL.SfPay().countPrize(CID3, 5)) + "</a> " + "注 每注 " + allr + " " + ub.Get("SiteBz") + "<br />");

            }
            if (zhu6 == 0)
            {
                builder.Append("[六等奖]：" + "<a href =\"" + Utils.getUrl("SFC.aspx?act=prizelist&amp;IsPrize=" + 6 + "&amp;CID=" + CID3 + "") + "\">" + getzhuh(6, CID3) + "</a> " + "注 每注0 " + ub.Get("SiteBz") + "<br />");
            }
            else
            {
                //费率
                double lv = Convert.ToDouble(ub.GetSub("SFSix", "/Controls/SFC.xml")) * 0.01;
                double allr = Math.Round(Convert.ToDouble(All * lv / Convert.ToDouble(zhu6)), 2);
                builder.Append("[六等奖]：" + "<a href =\"" + Utils.getUrl("SFC.aspx?act=prizelist&amp;IsPrize=" + 6 + "&amp;CID=" + CID3 + "") + "\">" + (getzhuh(6, CID3) + new BCW.SFC.BLL.SfPay().countPrize(CID3, 6)) + "</a> " + "注 每注 " + allr + " " + ub.Get("SiteBz") + "<br />");

            }
            if (zhu7 == 0)
            {
                builder.Append("[七等奖]：" + "<a href =\"" + Utils.getUrl("SFC.aspx?act=prizelist&amp;IsPrize=" + 7 + "&amp;CID=" + CID3 + "") + "\">" + getzhuh(7, CID3) + "</a> " + "注 每注0 " + ub.Get("SiteBz") + "");
            }
            else
            {
                //费率
                double lv = Convert.ToDouble(ub.GetSub("SFSeven", "/Controls/SFC.xml")) * 0.01;
                double allr = Math.Round(Convert.ToDouble(All * lv / Convert.ToDouble(zhu7)), 2);
                builder.Append("[七等奖]：" + "<a href =\"" + Utils.getUrl("SFC.aspx?act=prizelist&amp;IsPrize=" + 7 + "&amp;CID=" + CID3 + "") + "\">" + (getzhuh(7, CID3) + new BCW.SFC.BLL.SfPay().countPrize(CID3, 7)) + "</a> " + "注 每注 " + allr + " " + ub.Get("SiteBz") + "");

            }

        } builder.Append(Out.Tab("</div>", ""));
        #endregion

        string strText = "输入期数查询:/,";
        string strName = "number1,act";
        string strType = "num,hidden";
        string strValu = "" + CID3c + "'" + Utils.getPage(0) + "ok";
        string strEmpt = "true,true,false";
        string strIdea = "/";
        string strOthe = "确认搜索,SFC.aspx?,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));


        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("【开奖公告】 " + "<a href =\"" + Utils.getUrl("SFC.aspx?act=list") + "\">更多开奖</a><br />");

        try
        {
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string strWhere = string.Empty;
            strWhere = "EndTime<'" + DateTime.Now + "' ";

            string[] pageValUrl = { "act", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            // 开始读取列表
            IList<BCW.SFC.Model.SfList> listSFCList = new BCW.SFC.BLL.SfList().GetSfLists(pageIndex, pageSize, strWhere, out recordCount);
            if (listSFCList.Count > 0)
            {
                int k = 1;
                foreach (BCW.SFC.Model.SfList n in listSFCList)
                {
                    if (k <= 3)
                    {
                        if (k % 2 == 0)
                            builder.Append(Out.Tab("<div>", "<br />"));
                        else
                        {
                            if (k == 1)
                                builder.Append(Out.Tab("<div>", ""));
                            else
                                builder.Append(Out.Tab("<div>", "<br />"));
                        }
                        if (n.State == 1)
                        {
                            builder.Append("第" + n.CID + "期开出:<a href=\"" + Utils.getUrl("SFC.aspx?act=listview&amp;id=" + n.id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"><b>" + n.Result + "" + "</b></a>");
                        }
                        if (n.State == 0)
                        {
                            builder.Append("第" + n.CID + "期开出:<b>投注截止请等待系统开奖...</b>");
                        }
                        k++;
                        builder.Append(Out.Tab("</div>", ""));
                    }
                }

                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, 3, Utils.getPageUrl(), pageValUrl, "page", 0));

            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录.."));
            }
        }
        catch { }

        builder.Append(Out.Tab("</div>", "<br/>"));

        //3条实时动态
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("【游戏动态】");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        try
        {
            DataSet ds5 = new BCW.SFC.BLL.SfPay().GetList("top 3 *", " usID>10 order by id desc");
            if (ds5 != null && ds5.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds5.Tables[0].Rows.Count; i++)
                {
                    int UsID = int.Parse(ds5.Tables[0].Rows[i]["usID"].ToString());
                    string UsName = new BCW.BLL.User().GetUsName(UsID);
                    //string MyGoods = ds.Tables[0].Rows[i]["MyGoods"].ToString();
                    string addTime = ds5.Tables[0].Rows[i]["AddTime"].ToString();
                    int qishu = int.Parse(ds5.Tables[0].Rows[i]["CID"].ToString());
                    //int R = int.Parse(ds.Tables[0].Rows[i]["R"].ToString());
                    int Num = int.Parse(ds5.Tables[0].Rows[i]["PayCents"].ToString());
                    TimeSpan time = DateTime.Now - Convert.ToDateTime(addTime);

                    int d1 = time.Days;
                    int d = time.Hours;
                    int e = time.Minutes;
                    int f = time.Seconds;
                    if (d1 == 0)
                    {
                        if (d == 0 && e == 0 && f == 0)
                        {
                            builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + UsID + "") + "\">" + UsName + "</a>" + "在" + ub.GetSub("SFName", xmlPath) + "第" + CID + "期下注**" + ub.Get("SiteBz") + "<br />");//+Num
                        }
                        else if (d == 0 && e == 0)
                            builder.Append(f + "秒前" + " " + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + UsID + "") + "\">" + UsName + "</a>" + "在" + ub.GetSub("SFName", xmlPath) + "第" + CID + "期下注**" + ub.Get("SiteBz") + "<br />");// + Num 
                        else if (d == 0)
                            builder.Append(e + "分" + f + "秒前" + " " + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + UsID + "") + "\">" + UsName + "</a>" + "在" + ub.GetSub("SFName", xmlPath) + "第" + CID + "期下注**" + ub.Get("SiteBz") + "<br />");//+ Num 
                        else
                            builder.Append(d + "小时" + e + "分" + f + "秒前" + " " + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + UsID + "") + "\">" + UsName + "</a>" + "在" + ub.GetSub("SFName", xmlPath) + "第" + CID + "期下注**" + ub.Get("SiteBz") + "<br />");//+ Num
                    }
                    else
                        builder.Append(d1 + "天前" + " " + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + UsID + "") + "\">" + UsName + "</a>" + "在" + ub.GetSub("SFName", xmlPath) + "第" + CID + "期下注**" + ub.Get("SiteBz") + "<br />");//+ Num 

                }
            }
            else
            {
                builder.Append("没有更多数据...<br />");
            }
        }
        catch
        {
            builder.Append("没有数据");
        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx?act=trends") + "\">>>更多动态</a>");
        builder.Append(Out.Tab("</div>", ""));

        string FootUbb = ub.GetSub("SFFootUbb", xmlPath);
        if (FootUbb != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.SysUBB(FootUbb) + "");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        //闲聊显示
        builder.Append(Out.SysUBB(BCW.User.Users.ShowSpeak(34, "SFC.aspx", 5, 0)));
        builder.Append(Out.Tab("", Out.Hr()));
        GameFoot();
    }
    #endregion

    #region 中奖详细页面
    private void PrizeListPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href =\"" + Utils.getUrl("SFC.aspx") + "\">" + ub.GetSub("SFName", xmlPath) + "</a>&gt;开奖情况");
        builder.Append(Out.Tab("</div>", "<br />"));
        //期数
        int CID = int.Parse(Utils.GetRequest("CID", "all", 1, @"^", "0"));
        //等奖
        int IsPrize = int.Parse(Utils.GetRequest("IsPrize", "all", 1, @"^", "0"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("第" + CID + "期<br/>");
        builder.Append(Out.Tab("</div>", ""));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "CID=" + CID + " and ((IsPrize=10 and change like '%" + Han(IsPrize) + "%') or IsPrize=" + IsPrize + ")";
        string[] pageValUrl = { "act", "CID", "IsPrize", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        builder.Append(Out.Tab("<div>", ""));
        IList<BCW.SFC.Model.SfPay> listSfPay = new BCW.SFC.BLL.SfPay().GetSfPays(pageIndex, pageSize, strWhere, out recordCount);
        if (listSfPay.Count > 0)
        {
            int k = 1;
            foreach (BCW.SFC.Model.SfPay n in listSfPay)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }

                if (n.IsPrize == 10)
                {
                    if (n.change.Contains("" + Han(IsPrize) + ""))
                    {
                        builder.Append(k + "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.usID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(Convert.ToInt32(n.usID)) + "</a>" + "复式中奖(" + n.change + ")*" + n.OverRide + "倍共获得" + n.WinCent + ub.Get("SiteBz"));
                    }
                    else
                    {

                    }
                }
                else
                {
                    builder.Append(k + "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.usID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(Convert.ToInt32(n.usID)) + "</a>" + "中" + Han(IsPrize) + "等奖（" + n.VoteNum + "注*" + n.OverRide + "倍）共获得" + n.WinCent + ub.Get("SiteBz"));
                }
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("说明：复式中奖( 一/1#二/2#)表示中一等奖1注，二等奖2注 ");
            builder.Append(Out.Tab("</div>", ""));


        }
        else
        {
            builder.Append("没有相关记录.. ");
        }
        builder.Append(Out.Tab("</div>", "<br/>"));

        GameFoot();
    }
    #endregion

    #region 添加奖池数据
    private void Jackpot()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        BCW.SFC.Model.SfJackpot mo = new BCW.SFC.Model.SfJackpot();
        mo.usID = meid;
        mo.WinPrize = 0;
        mo.Prize = 0;
        mo.other = string.Empty;
        mo.AddTime = DateTime.Now;
        mo.CID = 0;
        new BCW.SFC.BLL.SfJackpot().Add(mo);
    }
    #endregion

    #region 投注界面
    private void InfoPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href =\"" + Utils.getUrl("SFC.aspx") + "\">" + ub.GetSub("SFName", xmlPath) + "</a>&gt;投注");
        builder.Append(Out.Tab("</div>", "<br />"));
        int yu = Utils.ParseInt(Utils.GetRequest("yu", "post", 1, @"^[0-5]$", "0"));

        #region 手机浏览器投注
        string agent = (Request.UserAgent + "").ToLower().Trim();
        if (agent == "" ||
            agent.IndexOf("mobile") != -1 ||
            agent.IndexOf("mobi") != -1 ||
            agent.IndexOf("nokia") != -1 ||
            agent.IndexOf("samsung") != -1 ||
            agent.IndexOf("sonyericsson") != -1 ||
            agent.IndexOf("mot") != -1 ||
            agent.IndexOf("blackberry") != -1 ||
            agent.IndexOf("lg") != -1 ||
            agent.IndexOf("htc") != -1 ||
            agent.IndexOf("j2me") != -1 ||
            agent.IndexOf("ucweb") != -1 ||
            agent.IndexOf("opera mini") != -1 ||
            agent.IndexOf("mobi") != -1 ||
            agent.IndexOf("android") != -1 ||
            agent.IndexOf("iphone") != -1)
        {
            builder.Append("<style>table{border-collapse:collapse;align-text:center;border:solid 1px black;}table tr td{padding:10px;border:solid 1px black;text-align:center;}</style>");
            int meid = new BCW.User.Users().GetUsId();
            if (meid == 0)
                Utils.Login();

            int id = Utils.ParseInt(Utils.GetRequest("id", "all", 1, @"^[0-5]$", "0"));
            DataSet qi = new BCW.SFC.BLL.SfList().GetList("CID", " State=0 and Sale_StartTime < '" + DateTime.Now + "' and EndTime > '" + DateTime.Now + "'");
            int qishu = Convert.ToInt32(qi.Tables[0].Rows[id][0]);

            BCW.SFC.Model.SfList mo = new BCW.SFC.BLL.SfList().GetSfList1(qishu);
            if (mo == null)
            {
                Utils.Error("请等待管理员开通下期...", "");
            }

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("" + ub.GetSub("SFName", xmlPath) + " 第<b>" + mo.CID + "</b>期");
            builder.Append(" 当前奖池：" + AllPrize(qishu) + "" + ub.Get("SiteBz") + "");
            builder.Append(Out.Tab("</div>", "<br />"));
            //builder.Append(Out.Tab("<div>", ""));
            if (mo.Sale_StartTime > DateTime.Now)
            {
                Utils.Error("当前投注未开始，请等待下注开始...", "");
            }
            if (mo.EndTime < DateTime.Now)
                Utils.Error("当前投注截止系统正在开奖...", "");
            else
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("起售时间：" + Convert.ToDateTime(mo.Sale_StartTime).ToString("yyyy-MM-dd HH:mm:ss") + "<br />");
                builder.Append("截止时间：" + Convert.ToDateTime(mo.EndTime).ToString("yyyy-MM-dd HH:mm:ss") + "<br />");
                builder.Append(Out.Tab("</div>", "<br />"));

                string SFC = new BCW.JS.somejs().newDaojishi("SFC", mo.EndTime.AddMinutes(10).AddSeconds(-10));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("距离截止还有" + SFC + "<br />");
                builder.Append(Out.Tab("</div>", ""));

                builder.Append("<form id=\"form1\" method=\"post\" action=\"SFC.aspx\">");
                BCW.SFC.Model.SfList model = new BCW.SFC.BLL.SfList().GetSfList1(qishu);
                string[] Match = model.Match.Split(",".ToCharArray());//赛事
                string[] Team_Home = model.Team_Home.Split(',');//主场
                string[] Team_way = model.Team_Away.Split(',');//客场
                string[] Start_Time = model.Start_time.Split(',');//开始时间
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("<table>");
                builder.Append("<tr><td>[场次]</td><td>[赛事]</td><td>[比赛时间]</td><td>[主场球队]VS[客场球队]</td><td>[选号区]</td></tr>");
                for (int j = 0; j < 14; j++)
                {
                    builder.Append("<tr><td>" + (j + 1) + "</td><td>" + Match[j] + "</td><td>" + Start_Time[j] + "</td><td>" + Team_Home[j] + "VS" + Team_way[j] + "</td><td>");
                    builder.Append("胜<input type=\"checkbox\" name=\"Num" + j + "\" value=\"3\" />");
                    builder.Append("平<input type=\"checkbox\" name=\"Num" + j + "\" value=\"1\" />");
                    builder.Append("负<input type=\"checkbox\" name=\"Num" + j + "\" value=\"0\" /></td></tr>");

                }
                builder.Append("</table>");
                builder.Append(Out.Tab("</div>", ""));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<input type=\"hidden\" name=\"act\" Value=\"pay\"/>");
                builder.Append("<input type=\"hidden\" name=\"ptype\" Value=\"" + id + "\"/>");
                builder.Append("<input type=\"hidden\" name=\"CID\" value=\"" + qishu + "\" />");
                string VE = ConfigHelper.GetConfigString("VE");
                string SID = ConfigHelper.GetConfigString("SID");
                builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
                builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
                builder.Append("<input class=\"btn-red\" type=\"submit\" value=\"下一步\"/><br />");


                builder.Append(Out.Tab("</div>", ""));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx?act=info&amp;id=" + id + "") + "\">清空选号</a>");
                builder.Append(Out.Tab("</div>", ""));
                builder.Append("</form>");
            }

            GameFoot();
        }
        #endregion
        #region 电脑浏览器投注
        else//
        {
            int meid = new BCW.User.Users().GetUsId();
            if (meid == 0)
                Utils.Login();

            int id = Utils.ParseInt(Utils.GetRequest("id", "all", 1, @"^[0-5]$", "0"));
            DataSet qi = new BCW.SFC.BLL.SfList().GetList("CID", " State=0 and Sale_StartTime < '" + DateTime.Now + "' and EndTime > '" + DateTime.Now + "'");
            int qishu = Convert.ToInt32(qi.Tables[0].Rows[id][0]);

            BCW.SFC.Model.SfList mo = new BCW.SFC.BLL.SfList().GetSfList1(qishu);
            if (mo == null)
            {
                Utils.Error("请等待管理员开通下期...", "");
            }

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("" + ub.GetSub("SFName", xmlPath) + " 第<b>" + mo.CID + "</b>期");
            builder.Append(" 当前奖池：" + AllPrize(qishu) + "" + ub.Get("SiteBz") + "");
            builder.Append(Out.Tab("</div>", "<br />"));
            //builder.Append(Out.Tab("<div>", ""));
            if (mo.Sale_StartTime > DateTime.Now)
            {
                Utils.Error("当前投注未开始，请等待下注开始...", "");
            }
            if (mo.EndTime < DateTime.Now)
                Utils.Error("当前投注截止系统正在开奖...", "");
            else
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("起售时间：" + Convert.ToDateTime(mo.Sale_StartTime).ToString("yyyy-MM-dd HH:mm:ss") + "<br />");
                builder.Append("截止时间：" + Convert.ToDateTime(mo.EndTime).ToString("yyyy-MM-dd HH:mm:ss") + "<br />");
                builder.Append(Out.Tab("</div>", "<br />"));

                string SFC = new BCW.JS.somejs().newDaojishi("SFC", mo.EndTime.AddMinutes(10).AddSeconds(-10));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("距离截止还有" + SFC + "<br />");
                builder.Append(Out.Tab("</div>", ""));

                builder.Append("<form id=\"form1\" method=\"post\" action=\"SFC.aspx\">");
                BCW.SFC.Model.SfList model = new BCW.SFC.BLL.SfList().GetSfList1(qishu);
                string[] Match = model.Match.Split(",".ToCharArray());//赛事
                string[] Team_Home = model.Team_Home.Split(',');//主场
                string[] Team_way = model.Team_Away.Split(',');//客场
                string[] Start_Time = model.Start_time.Split(',');//开始时间
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("<table>");
                builder.Append("<tr><td>[场次]</td><td>[赛事]</td><td>[比赛时间]</td><td>[主场球队]VS[客场球队]</td><td>[选号区]</td></tr>");
                for (int j = 0; j < 14; j++)
                {
                    builder.Append("<tr><td>" + (j + 1) + "</td><td>" + Match[j] + "</td><td>" + Start_Time[j] + "</td><td>" + Team_Home[j] + "VS" + Team_way[j] + "</td><td>");
                    builder.Append("胜<input type=\"checkbox\" name=\"Num" + j + "\" value=\"3\" />");
                    builder.Append("平<input type=\"checkbox\" name=\"Num" + j + "\" value=\"1\" />");
                    builder.Append("负<input type=\"checkbox\" name=\"Num" + j + "\" value=\"0\" /></td></tr>");

                }
                builder.Append("</table>");
                builder.Append(Out.Tab("</div>", ""));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<input type=\"hidden\" name=\"act\" Value=\"pay\"/>");
                builder.Append("<input type=\"hidden\" name=\"ptype\" Value=\"" + id + "\"/>");
                builder.Append("<input type=\"hidden\" name=\"CID\" value=\"" + qishu + "\" />");
                string VE = ConfigHelper.GetConfigString("VE");
                string SID = ConfigHelper.GetConfigString("SID");
                builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
                builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
                builder.Append("<input class=\"btn-red\" type=\"submit\" value=\"下一步\"/><br />");


                builder.Append(Out.Tab("</div>", ""));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx?act=info&amp;id=" + id + "") + "\">清空选号</a>");
                builder.Append(Out.Tab("</div>", ""));
                builder.Append("</form>");
            }


            GameFoot();
        }
        #endregion
    }
    #endregion

    #region 下一步投注
    private void PayPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = Utils.ParseInt(Utils.GetRequest("id", "all", 1, @"^[0-5]$", "0"));
        string info = Utils.GetRequest("info", "all", 1, "", "");
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href =\"" + Utils.getUrl("SFC.aspx") + "\">" + ub.GetSub("SFName", xmlPath) + "</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        string Num0 = Utils.GetRequest("Num0", "all", 2, @"^[\d((,)\d)?]+$", "第1场未投注");
        string Num1 = Utils.GetRequest("Num1", "all", 2, @"^[\d((,)\d)?]+$", "第2场未投注");
        string Num2 = Utils.GetRequest("Num2", "all", 2, @"^[\d((,)\d)?]+$", "第3场未投注");
        string Num3 = Utils.GetRequest("Num3", "all", 2, @"^[\d((,)\d)?]+$", "第4场未投注");
        string Num4 = Utils.GetRequest("Num4", "all", 2, @"^[\d((,)\d)?]+$", "第5场未投注");
        string Num5 = Utils.GetRequest("Num5", "all", 2, @"^[\d((,)\d)?]+$", "第6场未投注");
        string Num6 = Utils.GetRequest("Num6", "all", 2, @"^[\d((,)\d)?]+$", "第7场未投注");
        string Num7 = Utils.GetRequest("Num7", "all", 2, @"^[\d((,)\d)?]+$", "第8场未投注");
        string Num8 = Utils.GetRequest("Num8", "all", 2, @"^[\d((,)\d)?]+$", "第9场未投注");
        string Num9 = Utils.GetRequest("Num9", "all", 2, @"^[\d((,)\d)?]+$", "第10场未投注");
        string Num10 = Utils.GetRequest("Num10", "all", 2, @"^[\d((,)\d)?]+$", "第11场未投注");
        string Num11 = Utils.GetRequest("Num11", "all", 2, @"^[\d((,)\d)?]+$", "第12场未投注");
        string Num12 = Utils.GetRequest("Num12", "all", 2, @"^[\d((,)\d)?]+$", "第13场未投注");
        string Num13 = Utils.GetRequest("Num13", "all", 2, @"^[\d((,)\d)?]+$", "第14场未投注");
        int CID = Utils.ParseInt(Utils.GetRequest("CID", "all", 2, @"^[\d]{7}", "期数出错"));



        string[] str0 = Num0.Split(',');
        string[] str1 = Num1.Split(',');
        string[] str2 = Num2.Split(',');
        string[] str3 = Num3.Split(',');
        string[] str4 = Num4.Split(',');
        string[] str5 = Num5.Split(',');
        string[] str6 = Num6.Split(',');
        string[] str7 = Num7.Split(',');
        string[] str8 = Num8.Split(',');
        string[] str9 = Num9.Split(',');
        string[] str10 = Num10.Split(',');
        string[] str11 = Num11.Split(',');
        string[] str12 = Num12.Split(',');
        string[] str13 = Num13.Split(',');

        //投注个数
        int[] Temp = { str0.Length, str1.Length, str2.Length, str3.Length, str4.Length, str5.Length, str6.Length, str7.Length, str8.Length, str9.Length, str10.Length, str11.Length, str12.Length, str13.Length };

        if (info == "ok2")
        {
            BCW.SFC.Model.SfList mok = new BCW.SFC.BLL.SfList().GetSfList1(CID);
            if (mok == null)
            {
                Utils.Error("当前期数不存在...", "");
            }
            if (mok.Sale_StartTime > DateTime.Now)
            {
                Utils.Error("当前投注未开始，请等待下注开始...", "");
            }
            if (mok.EndTime < DateTime.Now)
                Utils.Error("当前投注截止...", "");

            if (mok.Sale_StartTime < DateTime.Now && DateTime.Now < mok.EndTime && mok.State != 1)//验证期数信息
            {

                long Price = Utils.ParseInt64(Utils.GetRequest("Price", "all", 1, @"^\d", "0"));
                int peilv = Utils.ParseInt(Utils.GetRequest("peilv", "all", 2, @"^[1-9]\d*$", "倍率输入错误"));
                if (peilv == 0)
                {
                    peilv = 1;
                }
                int Price1 = Utils.ParseInt(Utils.GetRequest("Price1", "all", 1, @"^\d", "0"));
                string SFprice = ub.GetSub("SFprice", xmlPath);

                long gold = new BCW.BLL.User().GetGold(meid);

                //是否刷屏
                long small = Convert.ToInt64(ub.GetSub("SFmallPay", xmlPath));
                long big = Convert.ToInt64(ub.GetSub("SFBigPay", xmlPath));
                string appName = "SFC";
                int Expir = Utils.ParseInt(ub.GetSub("SFExpir", xmlPath));//5
                BCW.User.Users.IsFresh(appName, Expir);

                if (gold < Price1)
                {
                    Utils.Error("您的" + ub.Get("SiteBz") + "不足", "");
                }
                string GameName = ub.GetSub("SFName", xmlPath);
                //个人每期限投
                long xPrices = 1;
                xPrices = Utils.ParseInt64(ub.GetSub("SFCprice", xmlPath));

                string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名
                string votenum1 = vote(str0) + "," + vote(str1) + "," + vote(str2) + "," + vote(str3) + "," + vote(str4) + "," + vote(str5) + "," + vote(str6) + "," + vote(str7) + "," + vote(str8) + "," + vote(str9) + "," + vote(str10) + "," + vote(str11) + "," + vote(str12) + "," + vote(str13) + ",";
                //  builder.Append(votenum1);

                if (xPrices > 0)
                {
                    int oPrices = 0;
                    DataSet ds = null;
                    try
                    {
                        ds = new BCW.SFC.BLL.SfPay().GetList("PayCents", "UsID=" + meid + " and CID=" + CID + "");
                    }
                    catch
                    {
                        Utils.Error("！", "");
                    }
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow dr in dt.Rows)
                    {
                        int drs = int.Parse(dr[0].ToString());
                        oPrices = oPrices + drs;
                    }
                    if (oPrices + Price1 > xPrices)
                    {
                        if (oPrices >= xPrices)
                            Utils.Error("您本期下注已达上限，请等待下期...", "");
                        else
                            Utils.Error("您本期最多还可以下注" + (xPrices - oPrices) + "" + ub.Get("SiteBz") + "", "");
                    }
                }
                string votenum = string.Empty;
                votenum = vote(str0) + "," + vote(str1) + "," + vote(str2) + "," + vote(str3) + "," + vote(str4) + "," + vote(str5) + "," + vote(str6) + "," + vote(str7) + "," + vote(str8) + "," + vote(str9) + "," + vote(str10) + "," + vote(str11) + "," + vote(str12) + "," + vote(str13);

                //支付安全提示
                string[] p_pageArr = { "act", "CID", "id", "votenum", "Price", "peilv", "Price1", "Num0", "Num1", "Num2", "Num3", "Num4", "Num5", "Num6", "Num7", "Num8", "Num9", "Num10", "Num11", "Num12", "Num13", "info" };
                BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr);

                BCW.SFC.Model.SfPay modelpay = new BCW.SFC.Model.SfPay();
                modelpay.usID = meid;
                modelpay.Vote = votenum;//投注情况
                modelpay.VoteNum = Convert.ToInt32(Zhu(Temp));//投注总数
                modelpay.IsPrize = 0;//中奖情况
                modelpay.IsSpier = 0;//机器人
                modelpay.AddTime = DateTime.Now;
                modelpay.CID = CID;//期号
                modelpay.OverRide = peilv;//投注倍率
                modelpay.PayCents = Price1;//投注总额
                modelpay.State = 0;//兑奖
                modelpay.PayCent = Convert.ToInt32(ub.GetSub("SFmallPay", xmlPath));
                modelpay.change = " ";
                new BCW.SFC.BLL.SfPay().Add(modelpay);
                //添加奖池数据
                BCW.SFC.Model.SfJackpot mo = new BCW.SFC.Model.SfJackpot();
                mo.usID = meid;
                mo.WinPrize = 0;
                mo.Prize = Price1;
                if (new BCW.SFC.BLL.SfList().getState((CID - 1)) == 1)
                {
                    mo.other = "下注" + Price1;
                }
                else
                {
                    mo.other = "预售下注" + Price1;
                }

                mo.allmoney = AllPrize(CID);
                mo.AddTime = DateTime.Now;
                mo.CID = CID;
                new BCW.SFC.BLL.SfJackpot().Add(mo);

                int maxid = new BCW.SFC.BLL.SfPay().GetMaxId(meid);
                new BCW.BLL.User().UpdateiGold(meid, mename, -Price1, "" + GameName + "第" + CID + "期投注" + votenum + "标识id" + (maxid - 1));//胜负彩----更新排行榜与扣钱
                //更新每期下注额度
                new BCW.SFC.BLL.SfList().UpdatePayCent(new BCW.SFC.BLL.SfPay().PayCents(CID), CID);
                //更新每期下注数
                new BCW.SFC.BLL.SfList().UpdatePayCount(new BCW.SFC.BLL.SfPay().VoteNum(CID), CID);
                string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在第" + CID + "期[url=/bbs/game/SFC.aspx]" + GameName + "[/url]下注**" + "" + ub.Get("SiteBz") + "";//+ Price1
                new BCW.BLL.Action().Add(1016, id, meid, "", wText);
                Utils.Success("下注", "下注成功，花费了" + Price1 + "" + ub.Get("SiteBz") + "<br /><a href=\"" + Utils.getUrl("SFC.aspx?act=info") + "\">&gt;继续下注</a>", Utils.getUrl("SFC.aspx"), "3");
            }
        }
        else if (info == "ok1")
        {
            #region 下注选号验证
            if (Num0 == "3" || Num0 == "1" || Num0 == "0" || Num0 == "3,1" || Num0 == "3,0" || Num0 == "1,0" || Num0 == "3,1,0")
            {
            }
            else { Utils.Error("下注错误，请不要更改下注选号", ""); }

            if (Num1 == "3" || Num1 == "1" || Num1 == "0" || Num1 == "3,1" || Num1 == "3,0" || Num1 == "1,0" || Num1 == "3,1,0")
            {
            }
            else { Utils.Error("下注错误，请不要更改下注选号", ""); }

            if (Num2 == "3" || Num2 == "1" || Num2 == "0" || Num2 == "3,1" || Num2 == "3,0" || Num2 == "1,0" || Num2 == "3,1,0")
            {
            }
            else { Utils.Error("下注错误，请不要更改下注选号", ""); }

            if (Num3 == "3" || Num3 == "1" || Num3 == "0" || Num3 == "3,1" || Num3 == "3,0" || Num3 == "1,0" || Num3 == "3,1,0")
            {
            }
            else { Utils.Error("下注错误，请不要更改下注选号", ""); }

            if (Num4 == "3" || Num4 == "1" || Num4 == "0" || Num4 == "3,1" || Num4 == "3,0" || Num4 == "1,0" || Num4 == "3,1,0")
            {
            }
            else { Utils.Error("下注错误，请不要更改下注选号", ""); }

            if (Num5 == "3" || Num5 == "1" || Num5 == "0" || Num5 == "3,1" || Num5 == "3,0" || Num5 == "1,0" || Num5 == "3,1,0")
            {
            }
            else { Utils.Error("下注错误，请不要更改下注选号", ""); }

            if (Num6 == "3" || Num6 == "1" || Num6 == "0" || Num6 == "3,1" || Num6 == "3,0" || Num6 == "1,0" || Num6 == "3,1,0")
            {
            }
            else { Utils.Error("下注错误，请不要更改下注选号", ""); }

            if (Num7 == "3" || Num7 == "1" || Num7 == "0" || Num7 == "3,1" || Num7 == "3,0" || Num7 == "1,0" || Num7 == "3,1,0")
            {
            }
            else { Utils.Error("下注错误，请不要更改下注选号", ""); }

            if (Num8 == "3" || Num8 == "1" || Num8 == "0" || Num8 == "3,1" || Num8 == "3,0" || Num8 == "1,0" || Num8 == "3,1,0")
            {
            }
            else { Utils.Error("下注错误，请不要更改下注选号", ""); }

            if (Num9 == "3" || Num9 == "1" || Num9 == "0" || Num9 == "3,1" || Num9 == "3,0" || Num9 == "1,0" || Num9 == "3,1,0")
            {
            }
            else { Utils.Error("下注错误，请不要更改下注选号", ""); }

            if (Num10 == "3" || Num10 == "1" || Num10 == "0" || Num10 == "3,1" || Num10 == "3,0" || Num10 == "1,0" || Num10 == "3,1,0")
            {
            }
            else { Utils.Error("下注错误，请不要更改下注选号", ""); }

            if (Num11 == "3" || Num11 == "1" || Num11 == "0" || Num11 == "3,1" || Num11 == "3,0" || Num11 == "1,0" || Num11 == "3,1,0")
            {
            }
            else { Utils.Error("下注错误，请不要更改下注选号", ""); }

            if (Num12 == "3" || Num12 == "1" || Num12 == "0" || Num12 == "3,1" || Num12 == "3,0" || Num12 == "1,0" || Num12 == "3,1,0")
            {
            }
            else { Utils.Error("下注错误，请不要更改下注选号", ""); }

            if (Num13 == "3" || Num13 == "1" || Num13 == "0" || Num13 == "3,1" || Num13 == "3,0" || Num13 == "1,0" || Num13 == "3,1,0")
            {
            }
            else { Utils.Error("下注错误，请不要更改下注选号", ""); }
            #endregion

            string SFprice = ub.GetSub("SFprice", xmlPath);
            int peilv = Utils.ParseInt(Utils.GetRequest("peilv", "post", 2, @"^[1-9]\d*$", "倍率输入错误"));
            int Price = Utils.ParseInt(Utils.GetRequest("Price", "post", 1, @"^\d", "0"));
            builder.Append(Out.Tab("<div>", ""));
            long gold = new BCW.BLL.User().GetGold(meid);
            builder.Append("您自带：" + Utils.ConvertGold(gold) + "" + ub.Get("SiteBz") + "<br />");
            builder.Append("投注总数：" + Zhu(Temp) + "注<br />");
            builder.Append("投注倍率：" + peilv + "倍<br />");
            builder.Append("每注金额：" + SFprice + "" + ub.Get("SiteBz") + "<br/>");
            Price = Price * peilv;
            builder.Append("总花费：" + Price + "" + ub.Get("SiteBz") + "");
            builder.Append(Out.Tab("</div>", ""));
            string strText = ",,,,,,,,,,,,,,,,,,,,";
            string strName = "peilv,CID,Price1,Num0,Num1,Num2,Num3,Num4,Num5,Num6,Num7,Num8,Num9,Num10,Num11,Num12,Num13,id,act,info";
            string strType = "hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden";
            string strValu = "" + peilv + "'" + CID + "'" + Price + "'" + Num0 + "'" + Num1 + "'" + Num2 + "'" + Num3 + "'" + Num4 + "'" + Num5 + "'" + Num6 + "'" + Num7 + "'" + Num8 + "'" + Num9 + "'" + Num10 + "'" + Num11 + "'" + Num12 + "'" + Num13 + "'" + id + "'pay'ok2";
            string strEmpt = "false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定投注,SFC.aspx,post,0,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx?act=info&amp;id=" + id + "") + "\">&lt;&lt;返回再看看</a>");
            builder.Append(Out.Tab("</div>", "<br />"));

        }
        else
        {
            int peilv = Utils.ParseInt(Utils.GetRequest("peilv", "all", 1, @"^[1-9]\d*$", "1"));
            string SFprice = ub.GetSub("SFprice", xmlPath);
            long gold = new BCW.BLL.User().GetGold(meid);
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("您自带：" + Utils.ConvertGold(gold) + "" + ub.Get("SiteBz") + "<br />");
            builder.Append("投注总数：" + Zhu(Temp) + "注");
            int Price = 0;
            Price = Convert.ToInt32(Zhu(Temp) * Convert.ToDouble(SFprice));
            // builder.Append("总花费："+ Price + ""+ ub.Get("SiteBz")+"<br />");
            builder.Append(Out.Tab("</div>", ""));

            #region 快捷下注
            try
            {
                int ptype = CID;
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("快捷下注（倍）<br />∟");
                kuai(meid, 5, ptype, peilv, Num0, Num1, Num2, Num3, Num4, Num5, Num6, Num7, Num8, Num9, Num10, Num11, Num12, Num13, id);//用户，游戏5，下注类型,传值1.2
                builder.Append(Out.Tab("</div>", ""));
            }
            catch { }
            #endregion

            string strText = "投注倍率：/,,,,,,,,,,,,,,,,,,,";
            string strName = "peilv,CID,Price,Num0,Num1,Num2,Num3,Num4,Num5,Num6,Num7,Num8,Num9,Num10,Num11,Num12,Num13,id,act,info";
            string strType = "num,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden";
            string strValu = "" + peilv + "'" + CID + "'" + Price + "'" + Num0 + "'" + Num1 + "'" + Num2 + "'" + Num3 + "'" + Num4 + "'" + Num5 + "'" + Num6 + "'" + Num7 + "'" + Num8 + "'" + Num9 + "'" + Num10 + "'" + Num11 + "'" + Num12 + "'" + Num13 + "'" + id + "'pay'ok1";
            string strEmpt = "true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定投注,SFC.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx?act=info&amp;id=" + id + "") + "\">&lt;&lt;返回再看看</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        GameFoot();
    }
    #endregion

    #region 奖池记录
    private void JackpotListPage()
    {
        GameTop();

        GameFoot();
    }
    #endregion

    #region 规则
    private void RulePage()
    {

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href =\"" + Utils.getUrl("SFC.aspx") + "\">" + ub.GetSub("SFName", xmlPath) + "</a>&gt;游戏规则");
        builder.Append(Out.Tab("</div>", "<br />"));
        //builder.Append(Out.Tab("<div>", ""));
        //builder.Append("=玩法规则=<br />");
        //builder.Append("一、玩法类型<br />    14场胜负彩是一种大盘玩法游戏，属竞技型彩票范畴。<br/>");
        //builder.Append("二、玩法说明<br />    14场胜负彩玩法竟猜规定的14场比赛90分钟内(含伤停补时)的胜平负结果。<br />");
        //builder.Append("三、标准投注<br />    对14场比赛各选1种比赛结果为1注，每场比赛最多可选3种结果。<br />");
        //builder.Append("四、设奖及中奖<br />   设一等奖（中14场）、二等奖（中13场）、三等奖（中12场）、四等奖（中11场）、五等奖（中10场）、六等奖（中9场）、七等奖（中8场）  <br />");
        //builder.Append("一等奖为当期奖金总额的" + ub.GetSub("SFOne", "/Controls/SFC.xml") + "%；<br/>二等奖为当期奖金总额的" + ub.GetSub("SFTwo", "/Controls/SFC.xml") + " %；<br/>三等奖为当期奖金总额的" + ub.GetSub("SFThree", "/Controls/SFC.xml") + " %；<br/>四等奖为当期奖金总额的" + ub.GetSub("SFForc", "/Controls/SFC.xml") + " %；<br/>五等奖为当期奖金总额的" + ub.GetSub("SFFive", "/Controls/SFC.xml") + " %；<br/>六等奖为当期奖金总额的" + ub.GetSub("SFSix", "/Controls/SFC.xml") + " %；<br/>七等奖为当期奖金总额的" + ub.GetSub("SFSeven", "/Controls/SFC.xml") + " %；<br/>");
        //builder.Append(Out.Tab("</div>", ""));

        string rule = ub.GetSub("SFrule", xmlPath);
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.SysUBB(rule));
        builder.Append(Out.Tab("</div>", "<br />"));

        GameFoot();
    }
    #endregion

    #region 动态
    private void TrendsPage()
    {

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href =\"" + Utils.getUrl("SFC.aspx") + "\">" + ub.GetSub("SFName", xmlPath) + "</a>&gt;游戏动态");
        builder.Append(Out.Tab("</div>", "<br />"));


        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b> 动态：</b>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        int pageIndex = 0;//当前页
        int recordCount;//记录总条数
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));//分页大小
        string strWhere = "";
        string strOrder = "";
        strWhere = " usID>10 ";
        strOrder = "id desc";
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        IList<BCW.SFC.Model.SfPay> GetPay = new BCW.SFC.BLL.SfPay().GetSfPays1(pageIndex, pageSize, strWhere, strOrder, out recordCount);

        if (GetPay.Count > 0)
        {
            int k = 1;
            foreach (BCW.SFC.Model.SfPay model1 in GetPay)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }
                int d = (pageIndex - 1) * 10 + k;
                string sText = string.Empty;
                string UsName = new BCW.BLL.User().GetUsName(Convert.ToInt32(model1.usID));
                sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model1.usID + "") + "\">" + UsName + "</a>" + "在" + ub.GetSub("SFName", xmlPath) + "第" + model1.CID + "期下注**" + ub.Get("SiteBz") + "（" + Convert.ToDateTime(model1.AddTime).ToString("yyyy-MM-dd HH:mm:ss") + "）<br />";//+ model1.PayCents 
                builder.AppendFormat("<a href=\"" + Utils.getUrl("SFC.aspx?act=trends&amp;id=" + model1.CID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>" + d + sText);


                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
        }

        else
        {
            builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
        }
        builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0) + "");
        builder.Append(Out.Tab("</div>", "<br />"));


        GameFoot();
    }
    #endregion

    #region 游戏顶部
    private void GameTop()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href =\"" + Utils.getUrl("SFC.aspx") + "\">" + ub.GetSub("SFName", xmlPath) + "</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    #endregion

    #region 游戏底部
    private void GameFoot()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏中心</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx") + "\">" + ub.GetSub("SFName", xmlPath) + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 往期开奖
    private void ListPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href =\"" + Utils.getUrl("SFC.aspx") + "\">" + ub.GetSub("SFName", xmlPath) + "</a>&gt;开奖历史");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        strWhere = "EndTime<'" + DateTime.Now + "' ";

        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.SFC.Model.SfList> listSFCList = new BCW.SFC.BLL.SfList().GetSfLists(pageIndex, pageSize, strWhere, out recordCount);
        if (listSFCList.Count > 0)
        {
            int k = 1;
            foreach (BCW.SFC.Model.SfList n in listSFCList)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }
                if (n.State == 1)
                {
                    builder.Append("第" + n.CID + "期开出:<a href=\"" + Utils.getUrl("SFC.aspx?act=listview&amp;id=" + n.id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"><b>" + n.Result + "" + "</b></a>");
                }
                if (n.State == 0)
                {
                    builder.Append("第" + n.CID + "期开出:<b>投注截止请等待系统开奖...</b>");
                }
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx?act=mylist&amp;id=0") + "\">未开投注</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx?act=mylist&amp;id=1") + "\">历史投注</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        GameFoot();
    }
    #endregion

    #region 开奖详细记录
    private void ListViewPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href =\"" + Utils.getUrl("SFC.aspx") + "\">" + ub.GetSub("SFName", xmlPath) + "</a>&gt;期数详情");
        builder.Append(Out.Tab("</div>", "<br />"));
        meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = Utils.ParseInt(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.SFC.Model.SfList model = new BCW.SFC.BLL.SfList().GetSfList(id);

        if (!new BCW.SFC.BLL.SfList().ExistsCID(model.CID))
        {
            Utils.Error("不存在的记录", "");
        }

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("期数详情 | <a href=\"" + Utils.getUrl("SFC.aspx?act=prize&amp;id=" + id + "") + "\">奖池记录</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        strWhere += "CID=" + model.CID + " and WinCent>0";

        string[] pageValUrl = { "act", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.SFC.Model.SfPay> listSFPay = new BCW.SFC.BLL.SfPay().GetSfPays(pageIndex, pageSize, strWhere, out recordCount);
        if (listSFPay.Count > 0)
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("第" + model.CID + "期开出:<b>" + model.Result + "</b><br/>");
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("详细记录：<br/>");
            builder.Append("该期奖池：" + new BCW.SFC.BLL.SfList().nowprize(model.CID) + ub.Get("SiteBz") + "<br />");

            string[] Match = model.Match.Split(',');
            string[] Team_Home = model.Team_Home.Split(',');
            string[] Team_Away = model.Team_Away.Split(',');
            string[] Result = model.Result.Split(',');
            string[] MatchTime = model.Start_time.Split(',');
            for (int i = 0; i < Match.Length - 1; i++)
            {
                if (model.Result == "无开奖")
                {
                    builder.Append(Match[i] + "|" + Team_Home[i] + "(<b>" + "无开奖" + "</b>)" + Team_Away[i] + "|比赛时间" + MatchTime[i] + "<br/>");
                }
                else
                    builder.Append(Match[i] + "|" + Team_Home[i] + "(<b>" + SPF(Result[i]) + "</b>)" + Team_Away[i] + "|比赛时间" + MatchTime[i] + "<br/>");
            }
            builder.Append(Out.Tab("</div>", ""));

            //builder.Append("共" + new BCW.SFC.BLL.SfPay().VoteNum(model.CID) + "注中奖");
            builder.Append("中奖情况如下:");

            builder.Append(Out.Tab("</div>", "<br />"));

            int k = 1;
            foreach (BCW.SFC.Model.SfPay n in listSFPay)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }
                string mename = new BCW.BLL.User().GetUsName(Convert.ToInt32(n.usID));
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".");
                if (n.IsPrize == 10)
                {
                    builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.usID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + mename + "</a>复式中" + "奖(" + n.change + ")注获得" + n.WinCent + "" + ub.Get("SiteBz") + "");
                }
                else
                    builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.usID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + mename + "</a>中" + Han(Convert.ToInt32(n.IsPrize)) + "等奖(" + n.VoteNum + ")注获得" + n.WinCent + "" + ub.Get("SiteBz") + "");

                k++;
                builder.Append(Out.Tab("</div>", ""));

            }
            builder.Append("<br />");

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

        }
        else
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("第" + model.CID + "期开出:<b>" + model.Result + "</b><br/>");
            builder.Append("详细记录：<br/>");
            builder.Append("该期奖池：" + new BCW.SFC.BLL.SfList().nowprize(model.CID) + ub.Get("SiteBz") + "<br />");

            string[] Match = model.Match.Split(',');
            string[] Team_Home = model.Team_Home.Split(',');
            string[] Team_Away = model.Team_Away.Split(',');
            string[] Result = model.Result.Split(',');
            string[] MatchTime = model.Start_time.Split(',');
            for (int i = 0; i < Match.Length - 1; i++)
            {
                if (model.Result == "无开奖")
                {
                    builder.Append(Match[i] + "|" + Team_Home[i] + "(<b>" + "无开奖" + "</b>)" + Team_Away[i] + "|比赛时间" + MatchTime[i] + "<br/>");
                }
                else
                    builder.Append(Match[i] + "|" + Team_Home[i] + "(<b>" + SPF(Result[i]) + "</b>)" + Team_Away[i] + "|比赛时间" + MatchTime[i] + "<br/>");
            }

            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("共0注中奖<br />");
            builder.Append(Out.Tab("</div>", ""));

        }

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("说明：复式中奖如（一/2#三/5#）表示中一等奖2注，三等奖5注");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"\">", ""));
        builder.Append("<a href=\"" + Utils.getPage("SFC.aspx?act=list") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx?act=mylist&amp;id=0") + "\">未开投注</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx?act=mylist&amp;id=1") + "\">历史投注</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        GameFoot();
    }
    #endregion

    #region 我的历史投注
    private void MyListPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href =\"" + Utils.getUrl("SFC.aspx") + "\">" + ub.GetSub("SFName", xmlPath) + "</a>&gt;投注记录");
        builder.Append(Out.Tab("</div>", "<br />"));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int ptype = Utils.ParseInt(Utils.GetRequest("id", "All", 2, @"^[0-1]$", "0"));

        string strTitle = "";
        if (ptype == 0)
            strTitle = "<b>我的未开投注</b>";
        else
            strTitle = "<b>我的历史投注</b>";

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx?act=mylist&amp;id=0") + "\">未开投注</a> |");
        builder.Append(" <a href=\"" + Utils.getUrl("SFC.aspx?act=mylist&amp;id=1") + "\">历史投注</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(strTitle);
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        strWhere = "UsID=" + meid + "";
        if (ptype == 0)
        {
            strWhere += " and State=0";
        }
        else
            strWhere += " and State!=0";

        string[] pageValUrl = { "act", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        int SFCqi = 0;

        // 开始读取列表
        IList<BCW.SFC.Model.SfPay> listSFPay = new BCW.SFC.BLL.SfPay().GetSfPays(pageIndex, pageSize, strWhere, out recordCount);
        if (listSFPay.Count > 0)
        {
            int k = 1;
            foreach (BCW.SFC.Model.SfPay n in listSFPay)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }
                if (n.State == 0)
                {


                    builder.Append("[" + ((pageIndex - 1) * pageSize + k) + "].");
                    string[] vote = n.Vote.Split(',');
                    builder.Append("第" + n.CID + "期押" + n.Vote + "/共" + n.VoteNum + "注*" + n.OverRide + "倍/下注" + n.PayCents + "" + ub.Get("SiteBz") + "[" + Convert.ToDateTime(n.AddTime).ToString("yyy-MM-dd HH:mm:ss") + "]");
                }
                else
                {
                    if (n.CID != SFCqi)
                        builder.Append("=第" + n.CID + "期=开出" + new BCW.SFC.BLL.SfList().result(n.CID) + "<br/>");

                    builder.Append("[" + ((pageIndex - 1) * pageSize + k) + "].");
                    builder.Append("押" + n.Vote + "/共" + n.VoteNum + "注*" + n.OverRide + "倍/下注" + n.PayCents + "" + ub.Get("SiteBz") + "[" + Convert.ToDateTime(n.AddTime).ToString("yyy-MM-dd HH:mm:ss") + "]");
                    if (n.WinCent > 0)
                    {
                        if (n.IsPrize == 10)
                        {
                            builder.Append("赢" + n.WinCent + "" + ub.Get("SiteBz") + "（" + n.change + "奖级）");
                        }
                        else
                        {
                            builder.Append("赢" + n.WinCent + "" + ub.Get("SiteBz") + "（" + n.IsPrize + "等奖）");
                        }
                    }
                }
                SFCqi = n.CID;
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("说明：（一/1#二/2#奖级）表示一等奖中1注，二等奖中2注");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append(Out.Tab("</div>", ""));
        GameFoot();
    }
    #endregion

    #region 排行榜
    private void TopPage()
    {
        Master.Title = "排行榜";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href =\"" + Utils.getUrl("SFC.aspx") + "\">" + ub.GetSub("SFName", xmlPath) + "</a>&gt;排行榜");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        int ptype = Utils.ParseInt(Utils.GetRequest("id", "all", 1, @"^[0-1]$", "0"));
        if (ptype == 0)
        {
            builder.Append("土豪榜|");
            builder.Append("<a href =\"" + Utils.getUrl("SFC.aspx?act=top&amp;id=1") + "\">好运榜</a><br />");
        }
        else
        {
            builder.Append("<a href =\"" + Utils.getUrl("SFC.aspx?act=top&amp;id=0") + "\">土豪榜|</a>");
            builder.Append("好运榜<br />");
        }

        builder.Append(Out.Tab("</div>", ""));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        if (ptype == 1)
        {
            strWhere = "PayCents>0 and State !=0 ";

            string[] pageValUrl = { "act", "ptype", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            IList<BCW.SFC.Model.SfPay> listSFPay = new BCW.SFC.BLL.SfPay().GetSFPaysTop(pageIndex, pageSize, strWhere, out recordCount);
            if (listSFPay.Count > 0)
            {
                int k = 1;
                foreach (BCW.SFC.Model.SfPay n in listSFPay)
                {
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }
                    if (n.WinCent > 0)
                    {
                        builder.Append("[第" + ((pageIndex - 1) * pageSize + k) + "名]<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.usID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(Convert.ToInt32(n.usID)) + "</a>" + "净赚" + n.WinCent + "" + ub.Get("SiteBz") + "");
                        k++;
                    }
                    else
                    {
                        builder.Append("没有相关记录..."); break;
                    }
                    builder.Append(Out.Tab("</div>", ""));
                }

                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录.."));
            }
        }
        else
        {
            strWhere = "PayCents>0";

            string[] pageValUrl = { "act", "ptype", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            DataSet ds = new BCW.SFC.BLL.SfPay().GetList("top 50 usID,sum(PayCents) as aa", "" + strWhere + " group by UsID ORDER BY aa DESC");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                recordCount = ds.Tables[0].Rows.Count;
                int k = 1;
                int koo = (pageIndex - 1) * pageSize;
                int skt = pageSize;
                if (recordCount > koo + pageSize)
                {
                    skt = pageSize;
                }
                else
                {
                    skt = recordCount - koo;
                }
                for (int i = 0; i < skt; i++)
                {
                    int UsID = Convert.ToInt32(ds.Tables[0].Rows[koo + i]["usid"]);//用户id
                    int id = Convert.ToInt32(ds.Tables[0].Rows[koo + i]["aa"]);//币额
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }
                    builder.Append("[第" + ((pageIndex - 1) * pageSize + k) + "名]<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(UsID) + "</a>投注" + id + "" + ub.Get("SiteBz") + "");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录."));
            }
        }
        builder.Append("<br />");
        GameFoot();
    }
    #endregion

    #region 奖池记录
    private void PrizePage()
    {
        Master.Title = "" + ub.GetSub("SFName", xmlPath) + "奖池记录";

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        string sfc = ub.GetSub("SFName", xmlPath);
        builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx") + "\">" + sfc + "</a>&gt;奖池记录");
        builder.Append(Out.Tab("</div>", "<br />"));

        int id = Utils.ParseInt(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.SFC.Model.SfList model = new BCW.SFC.BLL.SfList().GetSfList(id);

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx?act=listview&amp;id=" + id + "") + "\">期数详情</a> | 奖池记录");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b>" + model.CID + "期</b>");
        builder.Append(Out.Tab("</div>", "<br />"));

        if (!new BCW.SFC.BLL.SfList().ExistsCID(model.CID))
        {
            Utils.Error("不存在的记录", "");
        }


        string[] pageValUrl = { "act", "id", "backurl" };
        int pageIndex = 0;//当前页
        int recordCount;//记录总条数
        int pageSize = 10;//分页大小
        string strOrder = "";
        string strWhere = string.Empty;

        strWhere = " CID=" + model.CID + " ";
        strOrder = " CID Desc, AddTime Desc";

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("" + ub.GetSub("SFName", xmlPath) + "奖池记录");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("<b style=\"color:red\">" + model.CID + "</b> 期奖池：" + AllPrize(model.CID) + "" + ub.Get("SiteBz") + "<br/>");
        builder.Append(Out.Tab("</div>", ""));

        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        // 开始读取列表
        IList<BCW.SFC.Model.SfJackpot> listSfjackpot = new BCW.SFC.BLL.SfJackpot().GetSfJackpots(pageIndex, pageSize, strWhere, strOrder, out recordCount);

        if (listSfjackpot.Count > 0)
        {
            int k = 1;
            foreach (BCW.SFC.Model.SfJackpot n in listSfjackpot)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }

                int d = (pageIndex - 1) * 10 + k;
                string sText = string.Empty;
                if (BCW.User.Users.SetUser(n.usID) == "不存在的会员")
                {
                    if (n.usID == 5)
                    {
                        sText = "." + "<h style=\"color:red\">" + n.other + "" + "(" + Convert.ToDateTime(n.AddTime).ToString("yyyy-MM-dd HH:mm:ss") + ")</h>";
                    }
                    else if (n.usID == 6)
                    {
                        sText = "." + "<h style=\"color:red\">" + "第" + n.CID + "期" + n.other + "</h>";
                    }
                    else if (n.usID == 7)//系统回收
                    {
                        sText = "." + "<h style=\"color:red\">" + "第" + n.CID + "期" + n.other + ub.Get("SiteBz") + "|结余" + n.allmoney + ub.Get("SiteBz") + "(" + Convert.ToDateTime(n.AddTime).ToString("yyyy-MM-dd HH:mm:ss") + ")</h>";
                    }
                    else if (n.usID == 8)//当期滚完金额
                    {
                        sText = "." + "<h style=\"color:red\">" + n.other + "" + "(" + Convert.ToDateTime(n.AddTime).ToString("yyyy-MM-dd HH:mm:ss") + ")</h>";
                    }
                    else
                    {
                        if (n.Prize == 0)
                        {
                            sText = "." + "<h style=\"color:red\">系统在第" + n.CID + "期" + n.other + "" + ub.Get("SiteBz") + "|结余" + n.allmoney + ub.Get("SiteBz") + "(" + Convert.ToDateTime(n.AddTime).ToString("yyyy-MM-dd HH:mm:ss") + ")</h>";
                        }
                        else
                        {
                            sText = "." + "<h style=\"color:red\">系统在第" + n.CID + "期投入" + n.Prize + "" + ub.Get("SiteBz") + "|结余" + n.allmoney + ub.Get("SiteBz") + "(" + Convert.ToDateTime(n.AddTime).ToString("yyyy-MM-dd HH:mm:ss") + ")</h>";
                        }
                    }
                }
                else
                {
                    if (n.WinPrize == 0)
                    {
                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.usID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(n.usID) + "</a>在第" + n.CID + "期投注消费" + n.Prize + "" + ub.Get("SiteBz") + "|结余" + n.allmoney + ub.Get("SiteBz") + "(" + Convert.ToDateTime(n.AddTime).ToString("yyyy-MM-dd HH:mm:ss") + ")";
                    }
                    else
                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.usID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(n.usID) + "</a><h style=\"color:red\">在第" + n.CID + "期" + n.other + "" + ub.Get("SiteBz") + "|结余" + n.allmoney + ub.Get("SiteBz") + "(" + Convert.ToDateTime(n.AddTime).ToString("yyyy-MM-dd HH:mm:ss") + ")</h>";
                }

                builder.AppendFormat("<a href=\"" + Utils.getUrl("SFC.aspx?act=prize&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">  </a>" + d + sText + "   ");

                k++;

                builder.Append(Out.Tab("</div>", ""));
            }
        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("没有相关记录..");
            builder.Append(Out.Tab("</div>", ""));
        }
        // 分页
        builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0) + "");

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<br />说明：当前期奖池是系统当前期及所有过往期的记录，预售期记录仅为预售期的下注情况");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("SFC.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div>", ""));
        GameFoot();
    }
    #endregion

    #region 兑奖
    //兑奖确认
    private void CaseOkPage()
    {
        string sfc = ub.GetSub("SFName", xmlPath);
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int pid = Utils.ParseInt(Utils.GetRequest("pid", "get", 2, @"^[0-9]\d*$", "选择兑奖无效"));

        if (new BCW.SFC.BLL.SfPay().ExistsState(pid, meid))
        {
            new BCW.SFC.BLL.SfPay().UpdateState(pid);
            //操作币
            long winMoney = Convert.ToInt64(new BCW.SFC.BLL.SfPay().GetWinCent(pid));
            //税率
            long SysTax = 0;
            //期号
            BCW.SFC.Model.SfPay model = new BCW.SFC.BLL.SfPay().GetSfPay(pid);
            long number = Convert.ToInt64(model.CID);
            winMoney = winMoney - SysTax;
            BCW.User.Users.IsFresh("sfc", 1);//防刷
            if (model.IsPrize == 10)
            {
                new BCW.BLL.User().UpdateiGold(meid, winMoney, "" + sfc + "兑奖-期号-" + number + "-标识ID" + pid + "(" + model.change + "奖)");
            }
            else
            {
                new BCW.BLL.User().UpdateiGold(meid, winMoney, "" + sfc + "兑奖-期号-" + number + "-标识ID" + pid + "(" + model.IsPrize + "等奖)");
            }
            Utils.Success("兑奖", "恭喜，成功兑奖" + winMoney + "" + ub.Get("SiteBz") + "", Utils.getUrl("SFC.aspx?act=case"), "1");
        }
        else
        {
            Utils.Success("兑奖", "恭喜，重复兑奖或没有可以兑奖的记录", Utils.getUrl("SFC.aspx?act=case"), "1");
        }
    }
    //兑奖传输
    private void CasePostPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string arrId = "";
        arrId = Utils.GetRequest("arrId", "post", 1, "", "");
        if (!Utils.IsRegex(arrId.Replace(",", ""), @"^[0-9]\d*$"))
        {
            Utils.Error("选择本页兑奖出错", "");
        }
        string[] strArrId = arrId.Split(",".ToCharArray());
        long getwinMoney = 0;
        long winMoney = 0;
        BCW.User.Users.IsFresh("sfc", 1);//防刷
        for (int i = 0; i < strArrId.Length; i++)
        {
            int pid = Convert.ToInt32(strArrId[i]);
            if (new BCW.SFC.BLL.SfPay().ExistsState(pid, meid))
            {
                new BCW.SFC.BLL.SfPay().UpdateState(pid);
                //操作币
                winMoney = Convert.ToInt64(new BCW.SFC.BLL.SfPay().GetWinCent(pid));
                //税率
                long SysTax = 0;
                winMoney = winMoney - SysTax;
                //期号
                BCW.SFC.Model.SfPay model = new BCW.SFC.BLL.SfPay().GetSfPay(pid);
                long number = Convert.ToInt64(model.CID);
                new BCW.BLL.User().UpdateiGold(meid, winMoney, "胜负彩兑奖-期号-" + number + "-标识ID" + pid + "");
            }
            getwinMoney = getwinMoney + winMoney;
        }
        Utils.Success("兑奖", "恭喜，成功兑奖" + getwinMoney + "" + ub.Get("SiteBz") + "", Utils.getUrl("SFC.aspx?act=case"), "2");
    }
    //兑奖中心
    private void CasePage()
    {
        Master.Title = "兑奖中心";

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href =\"" + Utils.getUrl("SFC.aspx") + "\">" + ub.GetSub("SFName", xmlPath) + "</a>&gt;兑奖中心");
        builder.Append(Out.Tab("</div>", "<br />"));

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        //builder.Append(Out.Tab("<div class=\"title\">", ""));
        //builder.Append("您现在有" + new BCW.BLL.User().GetGold(meid) + "" + ub.Get("SiteBz") + "");
        //builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = 10;
        string strWhere = string.Empty;
        strWhere = "UsID=" + meid + " and WinCent>0 and State=1";
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        string arrId = "";
        // 开始读取列表
        IList<BCW.SFC.Model.SfPay> listSfPay = new BCW.SFC.BLL.SfPay().GetSfPays(pageIndex, pageSize, strWhere, out recordCount);
        if (listSfPay.Count > 0)
        {
            int k = 1;
            foreach (BCW.SFC.Model.SfPay n in listSfPay)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }
                builder.Append("[第" + n.CID + "期].");
                builder.Append("押" + n.Vote + "/下注" + n.PayCents + "" + ub.Get("SiteBz") + "/赢" + n.WinCent + "" + ub.Get("SiteBz") + "[" + DT.FormatDate(Convert.ToDateTime(n.AddTime), 13) + "]<a href=\"" + Utils.getUrl("SFC.aspx?act=caseok&amp;pid=" + n.id + "") + "\">兑奖</a>");

                arrId = arrId + " " + n.id;
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        if (!string.IsNullOrEmpty(arrId))
        {
            builder.Append(Out.Tab("", "<br />"));
            arrId = arrId.Trim();
            arrId = arrId.Replace(" ", ",");
            string strName = "arrId,act";
            string strValu = "" + arrId + "'casepost";
            string strOthe = "本页兑奖,SFC.aspx,post,0,red";
            builder.Append(Out.wapform(strName, strValu, strOthe));
        }
        //builder.Append(Out.Tab("<div>", Out.Hr()));
        //builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx?act=mylist&amp;ptype=0") + "\">未开投注</a> ");
        //builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx?act=mylist&amp;ptype=1") + "\">历史投注</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx") + "\">胜负彩</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 胜平负
    private string SPF(string Types)
    {
        string TyName = string.Empty;
        if (Types == "3")
            TyName = "胜";
        else if (Types == "1")
            TyName = "平";
        else if (Types == "0")
            TyName = "负";
        else if (Types == "*")
            TyName = "*";

        return TyName;
    }
    #endregion

    /// <summary>
    /// 计算注数
    /// </summary>
    /// <param name="temp"></param>
    /// <returns></returns>
    private double Zhu(int[] temp)
    {
        int er = 0;
        int yi = 0;
        int san = 0;
        double zhu = 0;
        for (int i = 0; i < temp.Length; i++)
        {
            if (temp[i] == 1)
                yi++;
            else if (temp[i] == 2)
                er++;
            else
                san++;
        }
        zhu = (Math.Pow(1, Convert.ToDouble(yi))) * (Math.Pow(2, Convert.ToDouble(er))) * (Math.Pow(3, Convert.ToDouble(san)));
        return zhu;
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
    //几等奖总注数开奖后
    private int getzhuh(int j, int CID)
    {
        string str = string.Empty;
        int zhu = 0;
        int sum = 0; int sum2 = 0; int sum3 = 0; int sum4 = 0; int sum5 = 0; int sum6 = 0; int sum7 = 0;
        DataSet pay = new BCW.SFC.BLL.SfPay().GetList("usID,ISPrize,VoteNum,OverRide,id,change", "CID=" + CID + "");
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

    private string vote(string[] aa)
    {
        string Vote = string.Empty;
        int i = 0;
        while (i < aa.Length)
        {
            if (string.IsNullOrEmpty(Vote))
            {
                Vote = Vote + aa[i];
            }
            else
            {
                Vote = Vote + "/" + aa[i];
            }
            i++;
        }
        return Vote;
    }
    //首页几等奖显示
    private string Han(int aa)
    {
        string han = "";
        switch (aa)
        {
            case 1:
                han = "一";
                break;
            case 2:
                han = "二";
                break;
            case 3:
                han = "三";
                break;
            case 4:
                han = "四";
                break;
            case 5:
                han = "五";
                break;
            case 6:
                han = "六";
                break;
            case 7:
                han = "七";
                break;
        }
        return han;
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

    /// <summary>
    /// 快捷下注转换成X万
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private string ChangeToWan(string str)
    {
        string CW = string.Empty;
        try
        {
            if (str != "")
            {
                long first = 0;
                first = Convert.ToInt64(str.Trim());
                if (first >= 10000)
                {
                    if (first % 10000 == 0)
                    {
                        CW = (first / 10000) + "万";
                    }
                    else
                    {
                        CW = (first / 10000) + ".X万";
                    }
                }
                else
                {
                    CW = first.ToString();
                }
            }
        }
        catch { }
        return CW;
    }

    #region 快捷下注1
    private void kuai(int uid, int type, int ptype, int peilv, string Num0, string Num1, string Num2, string Num3, string Num4, string Num5, string Num6, string Num7, string Num8, string Num9, string Num10, string Num11, string Num12, string Num13, int id)//用户，游戏编号，下注类型|特殊传值1.2
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string ac = Utils.GetRequest("ac", "all", 1, "", "");

        if (new BCW.QuickBet.BLL.QuickBet().ExistsUsID(meid))
        {

        }
        else//给会员自动添加默认的快捷下注
        {
            BCW.QuickBet.Model.QuickBet model = new BCW.QuickBet.Model.QuickBet();
            model.UsID = meid;
            model.Game = new BCW.QuickBet.BLL.QuickBet().GetGame();//十个编号的游戏|1:时时彩|2快乐十分|3:快乐扑克3|4:6场半|5:胜负彩
            model.Bet = new BCW.QuickBet.BLL.QuickBet().GetBety();
            new BCW.QuickBet.BLL.QuickBet().Add(model);
        }

        #region 快捷下注
        try
        {
            string game = new BCW.QuickBet.BLL.QuickBet().GetGame(meid);
            string bet = new BCW.QuickBet.BLL.QuickBet().GetBet(meid);
            string[] game1 = game.Split('#');
            string[] bet1 = bet.Split('#');
            for (int i = 0; i < game1.Length; i++)
            {
            }

            int j = 0;
            for (int i = 0; i < game1.Length; i++)
            {
                if (Convert.ToInt32(game1[i]) == type)//取出对应的游戏
                {
                    j = i;
                }
            }
            string str = string.Empty;
            string gold = string.Empty;
            string st = string.Empty;
            string[] kuai = bet1[j].Split('|');//取出对应的快捷下注
            for (int i = 0; i < kuai.Length; i++)
            {
                if (kuai[i] != "0")
                {
                    //if (Convert.ToInt64(kuai[i]) >= 10000)
                    //{
                    //    if (Convert.ToInt64(kuai[i]) % 10000 == 0)
                    //    {
                    //        gold = Utils.ConvertGold(Convert.ToInt64(kuai[i]));//
                    //    }
                    //    else
                    //    {
                    //      st = (Convert.ToInt64(kuai[i]) / 10000) + ".X万";
                    //        gold = st;
                    //    }
                    //}
                    //else
                    //{
                    //    gold = Utils.ConvertGold(Convert.ToInt64(kuai[i]));//
                    //}

                    gold = ChangeToWan(kuai[i]);

                    builder.Append("<a href =\"" + Utils.getUrl("SFC.aspx?act=pay&amp;CID=" + ptype + "&amp;id=" + id + "&amp;peilv=" + Convert.ToInt64(kuai[i]) + "&amp;Num0=" + Num0 + "&amp;Num1=" + Num1 + "&amp;Num2=" + Num2 + "&amp;Num3=" + Num3 + "&amp;Num4=" + Num4 + "&amp;Num5=" + Num5 + "&amp;Num6=" + Num6 + "&amp;Num7=" + Num7 + "&amp;Num8=" + Num8 + "&amp;Num9=" + Num9 + "&amp;Num10=" + Num10 + "&amp;Num11=" + Num11 + "&amp;Num12=" + Num12 + "&amp;Num13=" + Num13 + "") + "\">" + gold + "</a>" + "|");//    
                }
            }

            builder.Append("<a href=\"" + Utils.getUrl("QuickBet.aspx?act=edit&amp;type=" + type + "&amp;ptype=" + ptype + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">添删</a>");

        }
        catch { }
        #endregion
    }
    #endregion
}