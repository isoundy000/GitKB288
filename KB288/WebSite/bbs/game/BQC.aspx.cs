using System;
using System.Collections.Generic;
using BCW.Common;
using System.Data;
using System.Text.RegularExpressions;

/// <summary>
/// 蒙宗将 20160924 规划无开奖
/// 蒙宗将 20161006 历史投注显示
///  20161007 蒙宗将 优化获取上期
///  蒙宗将 20161011 奖池修改显示
///  蒙宗将 20161025 新倒计时
///  蒙宗将 首页重新排版 快捷下注 20161027
///  蒙宗将 快捷下注转换成万 20161028
///  蒙宗将 下注验证期号和选号 20161107
///  蒙宗将 20161112 优化开奖
/// </summary>

public partial class bbs_game_6changban : System.Web.UI.Page
{
    protected static string xmlPath = "/Controls/BQC.xml";
    protected string GameName = ub.GetSub("BQCName", "/Controls/BQC.xml");
    //  protected string BQCStatus = ub.GetSub("BQCStatus", xmlPath);//测试状态
    protected string BQCDemoIDS = ub.GetSub("BQCDemoIDS", xmlPath);
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    int meid = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        //维护提示
        if (ub.GetSub("BQCStatus", xmlPath) == "1")
        {
            Utils.Safe("此游戏");
        }

        //内测判断 0,内测是否开启1，是否为内测账号
        if (ub.GetSub("BQCStatus", xmlPath) == "2")//内测
        {
            string[] sNum = Regex.Split(BQCDemoIDS, "#");
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

        Master.Title = ub.GetSub("BQCName", xmlPath);
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

    #region 6场半首页
    private void ReloadPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href =\"" + Utils.getUrl("BQC.aspx") + "\">" + ub.GetSub("BQCName", xmlPath) + "</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        string TopUbb = ub.GetSub("BQCTopUbb", xmlPath);
        if (TopUbb != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.SysUBB(TopUbb) + "");
            builder.Append(Out.Tab("</div>", "<br />"));
        }

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("BQC.aspx?act=rule&amp;backurl=" + Utils.PostPage(1)) + "\">规则</a>");
        builder.Append(" | <a href =\"" + Utils.getUrl("BQC.aspx?act=case") + "\">兑奖</a>");
        builder.Append(" | <a href =\"" + Utils.getUrl("BQC.aspx?act=mylist&amp;id=0") + "\">记录</a>");
        builder.Append(" | <a href =\"" + Utils.getUrl("BQC.aspx?act=top&amp;id=0") + "\">排行</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        //未开奖当前投注期号
        DataSet ds = new BCW.BQC.BLL.BQCList().GetList("TOP 1 CID", "State=0 order by CID ASC");
        DataSet dsas = new BCW.BQC.BLL.BQCList().GetList("TOP 1 CID", "State=1 order by CID DESC");
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
        //    CID = new BCW.BQC.DAL.BQCPay().GetMaxCID() + 1;
        //}

        builder.Append("当期(" + CID + ")奖池滚存：" + AllPrize(CID) + "" + ub.Get("SiteBz") + "<br />");//<a href=\"" + Utils.getUrl("BQC.aspx?act=prize") + "\">奖池</a>

        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", ""));
        DataSet qi = new BCW.BQC.BLL.BQCList().GetList("CID", " State=0  and Sale_StartTime < '" + DateTime.Now + "' and EndTime > '" + DateTime.Now + "'");
        for (int i2 = 0; i2 < qi.Tables[0].Rows.Count; i2++)
        {
            int qishu = Convert.ToInt32(qi.Tables[0].Rows[i2][0]);
            BCW.BQC.Model.BQCList mo = new BCW.BQC.BLL.BQCList().GetBQCList1(qishu);
            //builder.Append(qishu);
            if (mo.Sale_StartTime < DateTime.Now && mo.EndTime > DateTime.Now)
            {
                if (i2 == (qi.Tables[0].Rows.Count - 1))
                {
                    builder.Append("<a href =\"" + Utils.getUrl("BQC.aspx?act=info&amp;id=" + i2 + "") + "\"><b>" + qishu + "期</b></a>");
                }
                else
                    builder.Append("<a href =\"" + Utils.getUrl("BQC.aspx?act=info&amp;id=" + i2 + "") + "\"><b>" + qishu + "期</b>|</a>");
            }
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        #region 期数倒计时
        //for (int i2 = 0; i2 < qi.Tables[0].Rows.Count; i2++)
        //{
        //    int qishu = Convert.ToInt32(qi.Tables[0].Rows[i2][0]);
        //    BCW.BQC.Model.BQCList mo = new BCW.BQC.BLL.BQCList().GetBQCList1(qishu);
        //    if (i2 == (qi.Tables[0].Rows.Count - 1))
        //    {
        //        if (mo.Sale_StartTime < DateTime.Now)
        //        {
        //            if (mo.EndTime < DateTime.Now)
        //            {
        //               // builder.Append("第" + mo.CID + "期投注截止请等待系统开奖...<br />");
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
        //            //builder.Append("第" + mo.CID + "期请等待投注开始...<br />");
        //        }
        //    }
        //    else
        //    {
        //        if (mo.Sale_StartTime < DateTime.Now)
        //        {
        //            if (mo.EndTime < DateTime.Now)
        //            {
        //               // builder.Append("第" + mo.CID + "期投注截止请等待系统开奖...<br />");
        //            }
        //            else
        //            {
        //                    string SFC = new BCW.JS.somejs().newDaojishi("" + i2 + "", mo.EndTime.AddMinutes(10).AddSeconds(-10));
        //                    builder.Append("第" + mo.CID + "期投注进行中,<br />距离截止时间还有" + SFC + "<br />");
        //                    //本期已下注" + mo.PayCent + "" + ub.Get("SiteBz") + "(" + mo.PayCount + "注)<br />
        //                    builder.Append("" + "-----------" + "<br />");

        //            }
        //        }
        //        else
        //        {
        //            //builder.Append("第" + mo.CID + "期请等待投注开始...<br />");
        //        }
        //    }
        //}
        #endregion

        builder.Append(Out.Tab("<div>", ""));
        //开奖最新期号
        builder.Append("【最新开奖】<br />");
        string act = Utils.GetRequest("act", "post", 1, @"^", "");
        int CID3c = int.Parse(Utils.GetRequest("number1", "post", 1, @"^", "0"));
        if (act == "ok")
        {
            if (CID3c == 0)
            {
                CID3c = new BCW.BQC.BLL.BQCList().CID();
            }

            builder.Append("第" + CID3c + "期开奖情况<br />");
            builder.Append("该期奖池：" + new BCW.BQC.BLL.BQCList().nowprize(CID3c) + ub.Get("SiteBz") + "<br />");

            //得到当前奖池
            long All = new BCW.BQC.BLL.BQCList().nowprize(CID3c);
            //费率
            double lv1 = Convert.ToDouble(ub.GetSub("BQCOne", "/Controls/BQC.xml")) * 0.01;
            double lv2 = Convert.ToDouble(ub.GetSub("BQCTwo", "/Controls/BQC.xml")) * 0.01;
            int zhu1 = new BCW.BQC.BLL.BQCPay().countPrize(CID3c, 1);
            int zhu2 = new BCW.BQC.BLL.BQCPay().countPrize2(CID3c);
            double allr1 = Math.Round(Convert.ToDouble(All * lv1 / Convert.ToDouble(zhu1)), 2);
            double allr2 = Math.Round(Convert.ToDouble(All * lv2 / Convert.ToDouble(zhu2)), 2);
            if (zhu1 == 0)
            {
                builder.Append("[一等奖]：" + "<a href =\"" + Utils.getUrl("BQC.aspx?act=prizelist&amp;IsPrize=" + 1 + "&amp;CID=" + CID3c + "") + "\">0</a>注 每注0" + ub.Get("SiteBz") + "<br />");
            }
            else
            {
                builder.Append("[一等奖]：" + "<a href =\"" + Utils.getUrl("BQC.aspx?act=prizelist&amp;IsPrize=" + 1 + "&amp;CID=" + CID3c + "") + "\">" + zhu1 + "</a> " + "注 每注 " + allr1 + ub.Get("SiteBz") + "<br />");
            }
            if (zhu2 == 0)
            {
                builder.Append("[二等奖]：" + "<a href =\"" + Utils.getUrl("BQC.aspx?act=prizelist&amp;IsPrize=" + 2 + "&amp;CID=" + CID3c + "") + "\">0</a>注 每注0" + ub.Get("SiteBz") + "");
            }
            else
            {
                builder.Append("[二等奖]：" + "<a href =\"" + Utils.getUrl("BQC.aspx?act=prizelist&amp;IsPrize=" + 2 + "&amp;CID=" + CID3c + "") + "\">" + zhu2 + "</a> " + "注 每注 " + allr2 + ub.Get("SiteBz") + "");
            }
        }
        else
        {
            int CID3 = new BCW.BQC.BLL.BQCList().CID();
            builder.Append("第" + CID3 + "期开奖情况<br />");
            builder.Append("该期奖池：" + new BCW.BQC.BLL.BQCList().nowprize(CID3) + ub.Get("SiteBz") + "<br />");

            //得到当前奖池
            long All = new BCW.BQC.BLL.BQCList().nowprize(CID3);
            //费率
            double lv1 = Convert.ToDouble(ub.GetSub("BQCOne", "/Controls/BQC.xml")) * 0.01;
            double lv2 = Convert.ToDouble(ub.GetSub("BQCTwo", "/Controls/BQC.xml")) * 0.01;
            int zhu1 = new BCW.BQC.BLL.BQCPay().countPrize(CID3, 1);
            int zhu2 = new BCW.BQC.BLL.BQCPay().countPrize2(CID3);
            double allr1 = Math.Round(Convert.ToDouble(All * lv1 / Convert.ToDouble(zhu1)), 2);
            double allr2 = Math.Round(Convert.ToDouble(All * lv2 / Convert.ToDouble(zhu2)), 2);
            if (zhu1 == 0)
            {
                builder.Append("[一等奖]：" + "<a href =\"" + Utils.getUrl("BQC.aspx?act=prizelist&amp;IsPrize=" + 1 + "&amp;CID=" + CID3 + "") + "\">0</a>注 每注0" + ub.Get("SiteBz") + "<br />");
            }
            else
            {
                builder.Append("[一等奖]：" + "<a href =\"" + Utils.getUrl("BQC.aspx?act=prizelist&amp;IsPrize=" + 1 + "&amp;CID=" + CID3 + "") + "\">" + zhu1 + "</a> " + "注 每注 " + allr1 + ub.Get("SiteBz") + "<br />");
            }
            if (zhu2 == 0)
            {
                builder.Append("[二等奖]：" + "<a href =\"" + Utils.getUrl("BQC.aspx?act=prizelist&amp;IsPrize=" + 2 + "&amp;CID=" + CID3 + "") + "\">0</a>注 每注0" + ub.Get("SiteBz") + "");
            }
            else
            {
                builder.Append("[二等奖]：" + "<a href =\"" + Utils.getUrl("BQC.aspx?act=prizelist&amp;IsPrize=" + 2 + "&amp;CID=" + CID3 + "") + "\">" + zhu2 + "</a> " + "注 每注 " + allr2 + ub.Get("SiteBz") + "");
            }
        }
        builder.Append(Out.Tab("</div>", ""));

        string strText = "输入期数查询:/,";
        string strName = "number1,act";
        string strType = "num,hidden";
        string strValu = "" + CID3c + "'" + Utils.getPage(0) + "ok";
        string strEmpt = "true,true,false";
        string strIdea = "/";
        string strOthe = "确认搜索,BQC.aspx?,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));


        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("【开奖公告】 " + "<a href =\"" + Utils.getUrl("BQC.aspx?act=list") + "\">更多开奖<br /></a>");

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
        IList<BCW.BQC.Model.BQCList> listSFCList = new BCW.BQC.BLL.BQCList().GetBQCLists(pageIndex, pageSize, strWhere, out recordCount);
        if (listSFCList.Count > 0)
        {
            int k = 1;
            foreach (BCW.BQC.Model.BQCList n in listSFCList)
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
                        builder.Append("第" + n.CID + "期开出:<a href=\"" + Utils.getUrl("BQC.aspx?act=listview&amp;id=" + n.id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"><b>" + n.Result + "</b></a>");
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

        builder.Append(Out.Tab("</div>", "<br />"));

        //5条实时动态
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("【游戏动态】");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));

        DataSet ds5 = new BCW.BQC.BLL.BQCPay().GetList("top 3 *", " usID>10 order by id Desc");
        if (ds5 != null && ds5.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds5.Tables[0].Rows.Count; i++)
            {
                int UsID = int.Parse(ds5.Tables[0].Rows[i]["usID"].ToString());
                string UsName = new BCW.BLL.User().GetUsName(UsID);
                string addTime = ds5.Tables[0].Rows[i]["AddTime"].ToString();
                int qishu = int.Parse(ds5.Tables[0].Rows[i]["CID"].ToString());
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
                        builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + UsID + "") + "\">" + UsName + "</a>" + "在" + ub.GetSub("BQCName", xmlPath) + "第" + qishu + "期下注**" + ub.Get("SiteBz") + "<br />");//+ Num
                    }
                    else if (d == 0 && e == 0)
                        builder.Append(f + "秒前" + " " + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + UsID + "") + "\">" + UsName + "</a>" + "在" + ub.GetSub("BQCName", xmlPath) + "第" + qishu + "期下注**" + ub.Get("SiteBz") + "<br />");//+ Num
                    else if (d == 0)
                        builder.Append(e + "分" + f + "秒前" + " " + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + UsID + "") + "\">" + UsName + "</a>" + "在" + ub.GetSub("BQCName", xmlPath) + "第" + qishu + "期下注**" + ub.Get("SiteBz") + "<br />");//+ Num 
                    else
                        builder.Append(d + "小时" + e + "分" + f + "秒前" + " " + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + UsID + "") + "\">" + UsName + "</a>" + "在" + ub.GetSub("BQCName", xmlPath) + "第" + qishu + "期下注**" + ub.Get("SiteBz") + "<br />");// + Num
                }
                else
                    builder.Append(d1 + "天前" + " " + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + UsID + "") + "\">" + UsName + "</a>" + "在" + ub.GetSub("BQCName", xmlPath) + "第" + qishu + "期下注**" + ub.Get("SiteBz") + "<br />");//+ Num

            }
        }
        else
        {
            builder.Append("没有更多数据...<br />");
        }

        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("BQC.aspx?act=trends") + "\">>>更多动态</a>");
        builder.Append(Out.Tab("</div>", ""));

        string FootUbb = ub.GetSub("BQCFootUbb", xmlPath);
        if (FootUbb != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.SysUBB(FootUbb) + "");
            builder.Append(Out.Tab("</div>", ""));
        }
        //闲聊显示
        builder.Append(Out.SysUBB(BCW.User.Users.ShowSpeak(36, "BQC.aspx", 5, 0)));
        builder.Append(Out.Tab("", Out.Hr()));
        GameFoot();
    }
    #endregion

    #region 中奖详细页面
    private void PrizeListPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href =\"" + Utils.getUrl("BQC.aspx") + "\">" + ub.GetSub("BQCName", xmlPath) + "</a>&gt;开奖情况");
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
        string strWhere = string.Empty;
        if (IsPrize == 1)
        {
            strWhere = "CID=" + CID + " and  IsPrize=" + 1 + "";
        }
        if (IsPrize == 2)
        {
            strWhere = "CID=" + CID + " and  IsPrize2>0 ";
        }
        string[] pageValUrl = { "act", "CID", "IsPrize", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        builder.Append(Out.Tab("<div>", ""));
        IList<BCW.BQC.Model.BQCPay> listBQCPay = new BCW.BQC.BLL.BQCPay().GetBQCPays(pageIndex, pageSize, strWhere, out recordCount);
        if (listBQCPay.Count > 0)
        {
            int k = 1;
            foreach (BCW.BQC.Model.BQCPay n in listBQCPay)
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
                if (n.IsPrize == 1 && n.IsPrize2 > 0)
                {
                    builder.Append(k + "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.usID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(Convert.ToInt32(n.usID)) + "</a>" + "复式中奖(" + n.IsPrize + "等奖1注/2等奖" + n.Prize2Num + "注)*" + n.OverRide + "倍|共获得" + n.WinCent + ub.Get("SiteBz"));
                }
                if (n.IsPrize == 1 && n.IsPrize2 == 0)
                {
                    builder.Append(k + "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.usID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(Convert.ToInt32(n.usID)) + "</a>" + "中奖(" + n.IsPrize + "等奖1注)*" + n.OverRide + "倍|共获得" + n.WinCent + ub.Get("SiteBz"));
                }
                if (n.IsPrize == 2 && n.IsPrize2 > 0)
                {
                    builder.Append(k + "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.usID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(Convert.ToInt32(n.usID)) + "</a>" + "中奖(2等奖" + n.Prize2Num + "注)*" + n.OverRide + "倍|共获得" + n.WinCent + ub.Get("SiteBz"));
                }
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("说明：复式中奖显示当条总中奖" + ub.Get("SiteBz") + "记录");
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
        BCW.BQC.Model.BQCJackpot mo = new BCW.BQC.Model.BQCJackpot();
        mo.usID = meid;
        mo.WinPrize = 0;
        mo.Prize = 0;
        mo.other = string.Empty;
        mo.AddTime = DateTime.Now;
        mo.CID = 0;
        new BCW.BQC.BLL.BQCJackpot().Add(mo);
    }
    #endregion

    #region 投注界面
    private void InfoPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href =\"" + Utils.getUrl("BQC.aspx") + "\">" + ub.GetSub("BQCName", xmlPath) + "</a>&gt;投注");
        builder.Append(Out.Tab("</div>", "<br />"));
        int yu = Utils.ParseInt(Utils.GetRequest("yu", "post", 1, @"^[0-5]$", "0"));
        builder.Append("<style>table{border-collapse:collapse;align-text:center;border:solid 1px black;}table tr td{padding:10px;border:solid 1px black;text-align:center;}</style>");

        #region 电脑版下注
        {
            int meid = new BCW.User.Users().GetUsId();
            if (meid == 0)
                Utils.Login();

            int id = Utils.ParseInt(Utils.GetRequest("id", "all", 1, @"^[0-5]$", "0"));
            DataSet qi = new BCW.BQC.BLL.BQCList().GetList("CID", " State=0 and Sale_StartTime < '" + DateTime.Now + "' and EndTime > '" + DateTime.Now + "'");
            int qishu = Convert.ToInt32(qi.Tables[0].Rows[id][0]);

            BCW.BQC.Model.BQCList mo = new BCW.BQC.BLL.BQCList().GetBQCList1(qishu);
            if (mo == null)
            {
                Utils.Error("请等待管理员开通下期...", "");
            }

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("" + ub.GetSub("BQCName", xmlPath) + " 第<b>" + mo.CID + "</b>期");
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
                builder.Append("截止时间：" + Convert.ToDateTime(mo.EndTime).ToString("yyyy-MM-dd HH:mm:ss") + "");
                builder.Append(Out.Tab("</div>", "<br />"));

                string SFC = new BCW.JS.somejs().newDaojishi("SFC", mo.EndTime.AddMinutes(10).AddSeconds(-10));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("距离截止还有" + SFC + "<br />");
                builder.Append(Out.Tab("</div>", ""));


                builder.Append("<form id=\"form1\" method=\"post\" action=\"BQC.aspx\">");
                int CID = qishu;
                BCW.BQC.Model.BQCList model = new BCW.BQC.BLL.BQCList().GetBQCListByCID(CID);
                string[] Match = model.Match.Split(",".ToCharArray());//赛事
                string[] Team_Home = model.Team_Home.Split(',');//主场
                string[] Team_way = model.Team_Away.Split(',');//客场
                string[] Start_Time = model.Start_time.Split(',');//开始时间
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("<table>");
                builder.Append("<tr><td>[场次]</td><td>[赛事]</td><td>[比赛时间]</td><td>[主场球队]VS[客场球队]</td><td>[选号区]</td></tr>");
                if (model.Match.Split(',').Length == 6)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        builder.Append("<tr><td>" + (j + 1) + "</td><td>" + Match[j] + "</td><td>" + Start_Time[j] + "</td><td>" + Team_Home[j] + "VS" + Team_way[j] + "</td><td>");
                        builder.Append("胜胜<input type=\"checkbox\" name=\"Num" + j + "\" value=\"3-3\" /> ");
                        builder.Append("胜平<input type=\"checkbox\" name=\"Num" + j + "\" value=\"3-1\" /> ");
                        builder.Append("胜负<input type=\"checkbox\" name=\"Num" + j + "\" value=\"3-0\" /> <br/>");

                        builder.Append("平胜<input type=\"checkbox\" name=\"Num" + j + "\" value=\"1-3\" /> ");
                        builder.Append("平平<input type=\"checkbox\" name=\"Num" + j + "\" value=\"1-1\" /> ");
                        builder.Append("平负<input type=\"checkbox\" name=\"Num" + j + "\" value=\"1-0\" /> <br/>");

                        builder.Append("负胜<input type=\"checkbox\" name=\"Num" + j + "\" value=\"0-3\" /> ");
                        builder.Append("负平<input type=\"checkbox\" name=\"Num" + j + "\" value=\"0-1\" /> ");
                        builder.Append("负负<input type=\"checkbox\" name=\"Num" + j + "\" value=\"0-0\" /> </td></tr>");

                    }
                }
                else
                {
                    builder.Append("场次出错，请等待开启正确场次...");
                }
                builder.Append("</table>");
                builder.Append(Out.Tab("</div>", ""));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<input type=\"hidden\" name=\"act\" Value=\"pay\"/>");
                builder.Append("<input type=\"hidden\" name=\"ptype\" Value=\"" + id + "\"/>");
                builder.Append("<input type=\"hidden\" name=\"CID\" value=\"" + CID + "\" />");
                string VE = ConfigHelper.GetConfigString("VE");
                string SID = ConfigHelper.GetConfigString("SID");
                builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
                builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
                builder.Append("<input class=\"btn-red\" type=\"submit\" value=\"下一步\"/><br />");
                builder.Append(Out.Tab("</div>", ""));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getUrl("BQC.aspx?act=info&amp;id=" + id + "") + "\">清空选号</a>");
                builder.Append(Out.Tab("</div>", ""));
                builder.Append("</form>");
                GameFoot();
            }
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
        builder.Append("<a href =\"" + Utils.getUrl("BQC.aspx") + "\">" + ub.GetSub("BQCName", xmlPath) + "</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        string Num0 = Utils.GetRequest("Num0", "all", 2, @"^[\s\S((,)\s\S)?]+$", "第1场未投注");
        string Num1 = Utils.GetRequest("Num1", "all", 2, @"^[\s\S((,)\s\S)?]+$", "第2场未投注");
        string Num2 = Utils.GetRequest("Num2", "all", 2, @"^[\s\S((,)\s\S)?]+$", "第3场未投注");
        string Num3 = Utils.GetRequest("Num3", "all", 2, @"^[\s\S((,)\s\S)?]+$", "第4场未投注");
        string Num4 = Utils.GetRequest("Num4", "all", 2, @"^[\s\S((,)\s\S)?]+$", "第5场未投注");
        string Num5 = Utils.GetRequest("Num5", "all", 2, @"^[\s\S((,)\s\S)?]+$", "第6场未投注");
        int CID = Utils.ParseInt(Utils.GetRequest("CID", "all", 2, @"^[\d]{7}", "期数出错"));
        string[] str0 = Num0.Split(',');
        string[] str1 = Num1.Split(',');
        string[] str2 = Num2.Split(',');
        string[] str3 = Num3.Split(',');
        string[] str4 = Num4.Split(',');
        string[] str5 = Num5.Split(',');

        //投注个数
        int[] Temp = { str0.Length, str1.Length, str2.Length, str3.Length, str4.Length, str5.Length };

        if (info == "ok2")
        {
            BCW.BQC.Model.BQCList mok = new BCW.BQC.BLL.BQCList().GetBQCList1(CID);
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
                string BQCprice = ub.GetSub("BQCprice", xmlPath);

                long gold = new BCW.BLL.User().GetGold(meid);


                //是否刷屏
                long small = Convert.ToInt64(ub.GetSub("BQCmallPay", xmlPath));
                long big = Convert.ToInt64(ub.GetSub("BQCBigPay", xmlPath));
                string appName = "SFC";
                int Expir = Utils.ParseInt(ub.GetSub("BQCExpir", xmlPath));//5
                BCW.User.Users.IsFresh(appName, Expir);

                if (gold < Price1)
                {
                    Utils.Error("您的" + ub.Get("SiteBz") + "不足", "");
                }
                string GameName = ub.GetSub("BQCName", xmlPath);
                //个人每期限投
                long xPrices = 0;
                xPrices = Utils.ParseInt64(ub.GetSub("BQCCprice", xmlPath));

                string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名
                string votenum1 = vote(str0) + "," + vote(str1) + "," + vote(str2) + "," + vote(str3) + "," + vote(str4) + "," + vote(str5);
                //builder.Append(votenum1);
                if (xPrices > 0)
                {
                    int oPrices = 0;
                    DataSet ds = null;
                    try
                    {
                        ds = new BCW.BQC.BLL.BQCPay().GetList("PayCents", "UsID=" + meid + " and CID=" + CID + "");
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
                        {
                            Utils.Error("您本期下注已达上限，请等待下期...", "");
                        }
                        else
                            Utils.Error("您本期最多还可以下注" + (xPrices - oPrices) + "" + ub.Get("SiteBz") + "", "");
                    }
                }
                string votenum = string.Empty;
                votenum = vote(str0) + "," + vote(str1) + "," + vote(str2) + "," + vote(str3) + "," + vote(str4) + "," + vote(str5);

                #region 下注验证
                string yanzheng = string.Empty;
                yanzheng = votenum.Replace("(", "").Replace(")", "").Replace(",", "").Replace("-", "").Replace("/", "");
                if (bolNum(yanzheng))
                {
                    int yza = 0;
                    string[] yz0 = vote(str0).Replace("(", "").Replace(")", "").Split('/');
                    for (int i = 0; i < yz0.Length; i++)
                    {
                        if (yz0[i].Length != 3)
                        {
                            yza += 1;
                        }
                        if (yz0[i].Contains("2") || yz0[i].Contains("4") || yz0[i].Contains("5") || yz0[i].Contains("6") || yz0[i].Contains("7") || yz0[i].Contains("8") || yz0[i].Contains("9") || yz0[i].Contains(" "))
                        {
                            yza += 1;
                        }
                    }
                    string[] yz1 = vote(str1).Replace("(", "").Replace(")", "").Split('/');
                    for (int i = 0; i < yz1.Length; i++)
                    {
                        if (yz1[i].Length != 3)
                        {
                            yza += 1;
                        }
                        if (yz1[i].Contains("2") || yz1[i].Contains("4") || yz1[i].Contains("5") || yz1[i].Contains("6") || yz1[i].Contains("7") || yz1[i].Contains("8") || yz1[i].Contains("9") || yz1[i].Contains(" "))
                        {
                            yza += 1;
                        }
                    }
                    string[] yz2 = vote(str2).Replace("(", "").Replace(")", "").Split('/');
                    for (int i = 0; i < yz2.Length; i++)
                    {
                        if (yz2[i].Length != 3)
                        {
                            yza += 1;
                        }
                        if (yz2[i].Contains("2") || yz2[i].Contains("4") || yz2[i].Contains("5") || yz2[i].Contains("6") || yz2[i].Contains("7") || yz2[i].Contains("8") || yz2[i].Contains("9") || yz2[i].Contains(" "))
                        {
                            yza += 1;
                        }
                    }
                    string[] yz3 = vote(str3).Replace("(", "").Replace(")", "").Split('/');
                    for (int i = 0; i < yz3.Length; i++)
                    {
                        if (yz3[i].Length != 3)
                        {
                            yza += 1;
                        }
                        if (yz3[i].Contains("2") || yz3[i].Contains("4") || yz3[i].Contains("5") || yz3[i].Contains("6") || yz3[i].Contains("7") || yz3[i].Contains("8") || yz3[i].Contains("9") || yz3[i].Contains(" "))
                        {
                            yza += 1;
                        }
                    }
                    string[] yz4 = vote(str4).Replace("(", "").Replace(")", "").Split('/');
                    for (int i = 0; i < yz4.Length; i++)
                    {
                        if (yz4[i].Length != 3)
                        {
                            yza += 1;
                        }
                        if (yz4[i].Contains("2") || yz4[i].Contains("4") || yz4[i].Contains("5") || yz4[i].Contains("6") || yz4[i].Contains("7") || yz4[i].Contains("8") || yz4[i].Contains("9") || yz4[i].Contains(" "))
                        {
                            yza += 1;
                        }
                    }
                    if (yza > 0)
                    {
                        Utils.Error("下注出错，请检查下注信息", "");
                    }
                }
                else
                {
                    Utils.Error("下注出错，请检查下注信息", "");
                }
                #endregion

                //支付安全提示
                string[] p_pageArr = { "act", "CID", "id", "votenum", "Price", "peilv", "Price1", "Num0", "Num1", "Num2", "Num3", "Num4", "Num5", "info" };
                BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr);

                BCW.BQC.Model.BQCPay modelpay = new BCW.BQC.Model.BQCPay();
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
                modelpay.PayCent = Convert.ToInt32(ub.GetSub("BQCmallPay", xmlPath));
                modelpay.change = 0;
                new BCW.BQC.BLL.BQCPay().Add(modelpay);
                //添加奖池数据
                BCW.BQC.Model.BQCJackpot mo = new BCW.BQC.Model.BQCJackpot();
                mo.usID = meid;
                mo.WinPrize = 0;
                mo.Prize = Price1;
                if (CID == FirstNewCID())//new BCW.BQC.BLL.BQCList().getState((CID-1))==1(旧方法)
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
                new BCW.BQC.BLL.BQCJackpot().Add(mo);


                int maxid = new BCW.BQC.BLL.BQCPay().GetMaxId(meid);
                new BCW.BLL.User().UpdateiGold(meid, mename, -Price1, "" + GameName + "第" + CID + "期投注" + votenum + "标识id" + (maxid - 1));//半全场----更新排行榜与扣钱
                //更新每期下注额度
                new BCW.BQC.BLL.BQCList().UpdatePayCent(new BCW.BQC.BLL.BQCPay().PayCents(CID), CID);
                //更新每期下注数
                new BCW.BQC.BLL.BQCList().UpdatePayCount(new BCW.BQC.BLL.BQCPay().VoteNum(CID), CID);
                string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在第" + CID + "期[url=/bbs/game/BQC.aspx]" + GameName + "[/url]下注**" + "" + ub.Get("SiteBz") + "";// + Price1 
                new BCW.BLL.Action().Add(1017, id, meid, "", wText);
                Utils.Success("下注", "下注成功，花费了" + Price1 + "" + ub.Get("SiteBz") + "<br /><a href=\"" + Utils.getUrl("BQC.aspx?act=info") + "\">&gt;继续下注</a>", Utils.getUrl("BQC.aspx"), "3");
            }
        }
        else if (info == "ok1")
        {

            string BQCprice = ub.GetSub("BQCprice", xmlPath);
            int peilv = Utils.ParseInt(Utils.GetRequest("peilv", "post", 2, @"^[1-9]\d*$", "倍率输入错误"));
            int Price = Utils.ParseInt(Utils.GetRequest("Price", "post", 1, @"^\d", "0"));
            builder.Append(Out.Tab("<div>", ""));
            long gold = new BCW.BLL.User().GetGold(meid);
            builder.Append("您自带：" + Utils.ConvertGold(gold) + "" + ub.Get("SiteBz") + "<br />");
            builder.Append("投注总数：" + Zhu(Temp) + "注<br />");
            builder.Append("投注倍率：" + peilv + "倍<br />");
            builder.Append("每注金额：" + BQCprice + "" + ub.Get("SiteBz") + "<br/>");
            Price = Price * peilv;
            builder.Append("总花费：" + Price + "" + ub.Get("SiteBz") + "");
            builder.Append(Out.Tab("</div>", ""));
            string strText = ",,,,,,,,,,,,,,,,,,";
            string strName = "peilv,CID,Price1,Num0,Num1,Num2,Num3,Num4,Num5,id,act,info";
            string strType = "hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden";
            string strValu = "" + peilv + "'" + CID + "'" + Price + "'" + Num0 + "'" + Num1 + "'" + Num2 + "'" + Num3 + "'" + Num4 + "'" + Num5 + "'" + id + "'pay'ok2";
            string strEmpt = "false,false,false,false,false,false,false,false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定投注,BQC.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("BQC.aspx?act=info&amp;id=" + id + "") + "\">&lt;&lt;返回再看看</a>");
            builder.Append(Out.Tab("</div>", "<br />"));

        }
        else
        {
            int peilv = Utils.ParseInt(Utils.GetRequest("peilv", "all", 1, @"^[1-9]\d*$", "1"));
            string BQCprice = ub.GetSub("BQCprice", xmlPath);
            long gold = new BCW.BLL.User().GetGold(meid);

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("您自带：" + Utils.ConvertGold(gold) + "" + ub.Get("SiteBz") + "<br />");
            builder.Append("投注总数：" + Zhu(Temp) + "注");
            long Price = 0;
            Price = Convert.ToInt64(Zhu(Temp) * Convert.ToDouble(BQCprice));
            // builder.Append("总花费："+ Price + ""+ ub.Get("SiteBz")+"<br />");
            builder.Append(Out.Tab("</div>", ""));

            #region 快捷下注
            try
            {
                int ptype = CID;
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("快捷下注（倍）<br />∟");
                kuai(meid, 4, ptype, peilv, Price, Num0, Num1, Num2, Num3, Num4, Num5, id);//用户，游戏4，下注类型,传值1.2
                builder.Append(Out.Tab("</div>", ""));
            }
            catch { }
            #endregion

            string strText = "投注倍率：/,,,,,,,,,,,";
            string strName = "peilv,CID,Price,Num0,Num1,Num2,Num3,Num4,Num5,id,act,info";
            string strType = "num,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden";
            string strValu = "" + peilv + "'" + CID + "'" + Price + "'" + Num0 + "'" + Num1 + "'" + Num2 + "'" + Num3 + "'" + Num4 + "'" + Num5 + "'" + id + "'pay'ok1";
            string strEmpt = "true,false,false,false,false,false,false,false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定投注,BQC.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));


            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("BQC.aspx?act=info&amp;id=" + id + "") + "\">&lt;&lt;返回再看看</a>");
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
        builder.Append("<a href =\"" + Utils.getUrl("BQC.aspx") + "\">" + ub.GetSub("BQCName", xmlPath) + "</a>&gt;游戏规则");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        //builder.Append("=玩法规则=<br />");
        //builder.Append("一、玩法类型<br />&nbsp;半全场是足彩胜平负游戏中竞猜上半场胜负和全场胜负的一种玩法。<br/>");
        //builder.Append("二、玩法说明<br />&nbsp;玩家选定1场比赛，对主队在上半场45分钟（含伤停补时）和全场90分钟（含伤停补时）的“胜”、“平”、“负”结果分别进行投注。如果玩家能竞猜正确，则中奖。<br />&nbsp;共有9个投注选项：<br/>");
        //builder.Append("&nbsp;3 - 3胜胜   主队上半场胜 + 主队全场胜<br/>&nbsp;3 - 1胜平   主队上半场胜 + 主队全场平<br/>&nbsp;3 - 0胜负   主队上半场胜 + 主队全场负<br/>&nbsp;1 - 3平胜   主队上半场平 + 主队全场胜<br/>"); ;
        //builder.Append("&nbsp;1 - 1平平 主队上半场平 + 主队全场平<br/>&nbsp;1 - 0平负   主队上半场平 + 主队全场负<br/>&nbsp;0 - 3负胜   主队上半场负 + 主队全场胜<br/>&nbsp;0 - 1负平   主队上半场负 + 主队全场平<br/>&nbsp;0 - 0负负   主队上半场负 + 主队全场负<br/>");
        //builder.Append("三、标准投注<br /> &nbsp;6场半全场是对给定的6场赛事进行半全场竞猜的方法。<br />");
        //builder.Append("四、设奖及中奖<br /> &nbsp;设一等奖（中6场）、二等奖（中5场）  <br />");
        //builder.Append("&nbsp;一等奖为当期奖金总额的" + ub.GetSub("SFOne", "/Controls/6changban.xml") + "%；<br/>&nbsp;二等奖为当期奖金总额的" + ub.GetSub("SFTwo", "/Controls/6changban.xml") + "%；");
        //builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.SysUBB(ub.GetSub("BQCrule", xmlPath)));
        builder.Append(Out.Tab("</div>", "<br />"));
        GameFoot();
    }
    #endregion

    #region 游戏顶部
    private void GameTop()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href =\"" + Utils.getUrl("BQC.aspx") + "\">" + ub.GetSub("BQCName", xmlPath) + "</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    #endregion

    #region 游戏底部
    private void GameFoot()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏中心</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("BQC.aspx") + "\">" + ub.GetSub("BQCName", xmlPath) + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 往期开奖
    private void ListPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href =\"" + Utils.getUrl("BQC.aspx") + "\">" + ub.GetSub("BQCName", xmlPath) + "</a>&gt;开奖历史");
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
        IList<BCW.BQC.Model.BQCList> listSFCList = new BCW.BQC.BLL.BQCList().GetBQCLists(pageIndex, pageSize, strWhere, out recordCount);
        if (listSFCList.Count > 0)
        {
            int k = 1;
            foreach (BCW.BQC.Model.BQCList n in listSFCList)
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
                    builder.Append("第" + n.CID + "期开出:<a href=\"" + Utils.getUrl("BQC.aspx?act=listview&amp;id=" + n.id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"><b>" + n.Result + "</b></a>");
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
        builder.Append("<a href=\"" + Utils.getUrl("BQC.aspx?act=mylist&amp;id=0") + "\">未开投注</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("BQC.aspx?act=mylist&amp;id=1") + "\">历史投注</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        GameFoot();
    }
    #endregion

    #region 动态
    private void TrendsPage()
    {

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href =\"" + Utils.getUrl("BQC.aspx") + "\">" + ub.GetSub("BQCName", xmlPath) + "</a>&gt;游戏动态");
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
        IList<BCW.BQC.Model.BQCPay> GetPay = new BCW.BQC.BLL.BQCPay().GetBQCPays1(pageIndex, pageSize, strWhere, strOrder, out recordCount);

        if (GetPay.Count > 0)
        {
            int k = 1;
            foreach (BCW.BQC.Model.BQCPay model1 in GetPay)
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
                sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model1.usID + "") + "\">" + UsName + "</a>" + "在" + ub.GetSub("BQCName", xmlPath) + "第" + model1.CID + "期下注**" + ub.Get("SiteBz") + "（" + Convert.ToDateTime(model1.AddTime).ToString("yyyy-MM-dd HH:mm:ss") + "）<br />";//+ model1.PayCents
                builder.AppendFormat("<a href=\"" + Utils.getUrl("BQC.aspx?act=trends&amp;id=" + model1.CID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>" + d + sText);


                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0) + "");
        }

        else
        {
            builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
        }

        builder.Append(Out.Tab("</div>", "<br />"));


        GameFoot();
    }
    #endregion

    #region 我的历史投注
    private void MyListPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href =\"" + Utils.getUrl("BQC.aspx") + "\">" + ub.GetSub("BQCName", xmlPath) + "</a>&gt;投注记录");
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
        builder.Append("<a href=\"" + Utils.getUrl("BQC.aspx?act=mylist&amp;id=0") + "\">未开投注</a> | ");
        builder.Append("<a href=\"" + Utils.getUrl("BQC.aspx?act=mylist&amp;id=1") + "\">历史投注</a>");
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
            strWhere += " and State=0";
        else
            strWhere += " and State!=0";

        string[] pageValUrl = { "act", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        int SFCqi = 0;

        // 开始读取列表
        IList<BCW.BQC.Model.BQCPay> listBQCPay = new BCW.BQC.BLL.BQCPay().GetBQCPays(pageIndex, pageSize, strWhere, out recordCount);
        if (listBQCPay.Count > 0)
        {
            int k = 1;
            foreach (BCW.BQC.Model.BQCPay n in listBQCPay)
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
                    //   if (n.CID != SFCqi)
                    builder.Append("[" + ((pageIndex - 1) * pageSize + k) + "].");
                    string[] vote = n.Vote.Split(',');
                    builder.Append("第" + n.CID + "期押" + n.Vote + "/共" + n.VoteNum + "注*" + n.OverRide + "倍/下注" + n.PayCents + "" + ub.Get("SiteBz"));//+ "[" + Convert.ToDateTime(n.AddTime).ToString("yyyy-MM-dd HH:mm:ss") + "]"
                }
                else
                {
                    if (n.CID != SFCqi)
                        builder.Append("=第" + n.CID + "期=开出" + new BCW.BQC.BLL.BQCList().result(n.CID) + "<br/>");

                    builder.Append("[" + ((pageIndex - 1) * pageSize + k) + "].");
                    builder.Append("押" + n.Vote + "/共" + n.VoteNum + "注*" + n.OverRide + "倍/下注" + n.PayCents + "" + ub.Get("SiteBz"));//+ "[" + Convert.ToDateTime(n.AddTime).ToString("yyyy-MM-dd HH:mm:ss") + "]"
                    if (n.WinCent > 0)
                    {
                        string ying = string.Empty;
                        if (n.IsPrize == 1 && n.IsPrize2 > 0)
                        {
                            ying = "复式中奖(" + n.IsPrize + "等奖1注/2等奖" + n.Prize2Num + "注)*" + n.OverRide + "倍";
                        }
                        if (n.IsPrize == 1 && n.IsPrize2 == 0)
                        {
                            ying = "中奖(" + n.IsPrize + "等奖1注)*" + n.OverRide + "倍";
                        }
                        if (n.IsPrize == 2 && n.IsPrize2 > 0)
                        {
                            ying = "中奖(2等奖" + n.Prize2Num + "注)*" + n.OverRide + "倍";
                        }
                        builder.Append("|" + ying + "|赢" + n.WinCent + "" + ub.Get("SiteBz") + "");
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
        builder.Append("<a href =\"" + Utils.getUrl("BQC.aspx") + "\">" + ub.GetSub("BQCName", xmlPath) + "</a>&gt;排行榜");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        int ptype = Utils.ParseInt(Utils.GetRequest("id", "all", 1, @"^[0-1]$", "0"));
        if (ptype == 0)
        {
            builder.Append("土豪榜|");
            builder.Append("<a href =\"" + Utils.getUrl("BQC.aspx?act=top&amp;id=1") + "\">好运榜</a><br />");
        }
        if (ptype == 1)
        {
            builder.Append("<a href =\"" + Utils.getUrl("BQC.aspx?act=top&amp;id=0") + "\">土豪榜|</a>");
            builder.Append("好运榜<br />");
        }

        builder.Append(Out.Tab("</div>", ""));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        if (ptype == 1)
        {
            strWhere = "WinCent>0 and State != 0 ";

            string[] pageValUrl = { "act", "ptype", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            IList<BCW.BQC.Model.BQCPay> listBQCPay = new BCW.BQC.BLL.BQCPay().GetBQCPaysTop(pageIndex, pageSize, strWhere, out recordCount);
            if (listBQCPay.Count > 0)
            {
                int k = 1;
                foreach (BCW.BQC.Model.BQCPay n in listBQCPay)
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
            DataSet ds = new BCW.BQC.BLL.BQCPay().GetList("top 50 usID,sum(PayCents) as aa", "" + strWhere + " group by UsID ORDER BY aa DESC");
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

    #region 开奖详细记录
    private void ListViewPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href =\"" + Utils.getUrl("BQC.aspx") + "\">" + ub.GetSub("BQCName", xmlPath) + "</a>&gt;期数详情");
        builder.Append(Out.Tab("</div>", "<br />"));
        meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = Utils.ParseInt(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.BQC.Model.BQCList model = new BCW.BQC.BLL.BQCList().GetBQCList(id);

        if (!new BCW.BQC.BLL.BQCList().Exists(model.CID))
        {
            Utils.Error("不存在的记录", "");
        }

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("期数详情 | <a href=\"" + Utils.getUrl("BQC.aspx?act=prize&amp;id=" + id + "") + "\">奖池记录</a>");
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
        IList<BCW.BQC.Model.BQCPay> listBQCPay = new BCW.BQC.BLL.BQCPay().GetBQCPays(pageIndex, pageSize, strWhere, out recordCount);
        if (listBQCPay.Count > 0)
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("第" + model.CID + "期开出:<b>" + model.Result + "</b><br/>");
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("详细记录：<br/>");
            builder.Append("该期奖池：" + new BCW.BQC.BLL.BQCList().nowprize(model.CID) + ub.Get("SiteBz") + "<br />");

            string[] Match = model.Match.Split(',');
            string[] Team_Home = model.Team_Home.Split(',');
            string[] Team_Away = model.Team_Away.Split(',');
            string[] Result = model.Result.Split(',');
            string[] MatchTime = model.Start_time.Split(',');
            for (int i = 0; i < 6; i++)
            {

                if (model.Result == "无开奖")
                {
                    builder.Append(Match[i] + "|" + Team_Home[i] + "(<b>" + "无开奖" + "</b>)" + Team_Away[i] + "|比赛时间" + MatchTime[i] + "<br/>");
                }
                else

                    builder.Append(Match[i] + "|" + Team_Home[i] + "(<b>" + SPF(Result[i]) + "</b>)" + Team_Away[i] + "|比赛时间" + MatchTime[i] + "<br/>");
            }
            builder.Append(Out.Tab("</div>", ""));

            builder.Append("共" + new BCW.BQC.BLL.BQCPay().PrizeNum(model.CID) + "注中奖");

            builder.Append(Out.Tab("</div>", "<br />"));

            int k = 1;
            foreach (BCW.BQC.Model.BQCPay n in listBQCPay)
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

                if (n.IsPrize == 1 && n.IsPrize2 > 0)
                {
                    builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.usID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(Convert.ToInt32(n.usID)) + "</a>" + "复式中奖(" + n.IsPrize + "等奖1注/2等奖" + n.Prize2Num + "注)*" + n.OverRide + "倍|共获得" + n.WinCent + ub.Get("SiteBz"));
                }
                if (n.IsPrize == 1 && n.IsPrize2 == 0)
                {
                    builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.usID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(Convert.ToInt32(n.usID)) + "</a>" + "中奖(" + n.IsPrize + "等奖1注)*" + n.OverRide + "倍|共获得" + n.WinCent + ub.Get("SiteBz"));
                }
                if (n.IsPrize == 2 && n.IsPrize2 > 0)
                {
                    builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.usID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(Convert.ToInt32(n.usID)) + "</a>" + "中奖(2等奖" + n.Prize2Num + "注)*" + n.OverRide + "倍|共获得" + n.WinCent + ub.Get("SiteBz"));
                }
                k++;
                builder.Append(Out.Tab("</div>", "<br />"));

            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

        }
        else
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("第" + model.CID + "期开出:<b>" + model.Result + "</b><br/>");
            builder.Append("详细记录：<br/>");
            builder.Append("该期奖池：" + new BCW.BQC.BLL.BQCList().nowprize(model.CID) + ub.Get("SiteBz") + "<br />");

            string[] Match = model.Match.Split(',');
            string[] Team_Home = model.Team_Home.Split(',');
            string[] Team_Away = model.Team_Away.Split(',');
            string[] Result = model.Result.Split(',');
            string[] MatchTime = model.Start_time.Split(',');
            for (int i = 0; i < Match.Length; i++)
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
        builder.Append("说明：复式中奖显示当条总中奖" + ub.Get("SiteBz") + "记录");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"\">", ""));
        builder.Append("<a href=\"" + Utils.getPage("BQC.aspx?act=list") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("BQC.aspx?act=mylist&amp;id=0") + "\">未开投注</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("BQC.aspx?act=mylist&amp;id=1") + "\">历史投注</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        GameFoot();
    }
    #endregion

    #region 奖池记录
    private void PrizePage()
    {
        Master.Title = "" + ub.GetSub("BQCName", xmlPath) + "奖池记录";

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        string sfc = ub.GetSub("BQCName", xmlPath);
        builder.Append("<a href=\"" + Utils.getUrl("BQC.aspx") + "\">" + sfc + "</a>&gt;奖池记录");
        builder.Append(Out.Tab("</div>", "<br />"));

        int id = Utils.ParseInt(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.BQC.Model.BQCList model = new BCW.BQC.BLL.BQCList().GetBQCList(id);

        if (!new BCW.BQC.BLL.BQCList().Exists(model.CID))
        {
            Utils.Error("不存在的记录", "");
        }

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("BQC.aspx?act=listview&amp;id=" + id + "") + "\">期数详情</a> | 奖池记录");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b>" + model.CID + "期</b>");
        builder.Append(Out.Tab("</div>", "<br />"));

        if (!new BCW.BQC.BLL.BQCList().Existsjilu())
        {
            Utils.Error("系统错误...", "");
        }

        string[] pageValUrl = { "act", "id", "backurl" };
        int pageIndex = 0;//当前页
        int recordCount;//记录总条数
        int pageSize = 10;//分页大小
        string strOrder = "";
        string strWhere = string.Empty;

        //查询条件

        strWhere = " CID=" + model.CID + " ";
        strOrder = "CID Desc, AddTime Desc";

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("" + ub.GetSub("BQCName", xmlPath) + "奖池记录");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("<b style=\"color:red\">" + model.CID + "</b> 期奖池：" + AllPrize(model.CID) + "" + ub.Get("SiteBz") + "<br/>");

        builder.Append(Out.Tab("</div>", ""));

        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        // 开始读取列表
        IList<BCW.BQC.Model.BQCJackpot> listBQCJackpot = new BCW.BQC.BLL.BQCJackpot().GetBQCJackpots(pageIndex, pageSize, strWhere, strOrder, out recordCount);

        if (listBQCJackpot.Count > 0)
        {
            int k = 1;
            foreach (BCW.BQC.Model.BQCJackpot n in listBQCJackpot)
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
                if (BCW.User.Users.SetUser(Convert.ToInt32(n.usID)) == "不存在的会员")
                {
                    if (n.usID == 5)//当期滚完金额
                    {
                        sText = "." + "<h style=\"color:red\">" + n.other + "" + "(" + Convert.ToDateTime(n.AddTime).ToString("yyyy-MM-dd HH:mm:ss") + ")</h>";
                    }
                    else if (n.usID == 6)//得到滚存金额
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
                            if (n.usID == 1)
                            {
                                sText = "." + "<h style=\"color:red\">系统在第" + n.CID + "期" + n.other + "" + ub.Get("SiteBz") + "|结余" + n.allmoney + ub.Get("SiteBz") + "(" + Convert.ToDateTime(n.AddTime).ToString("yyyy-MM-dd HH:mm:ss") + ")</h>";
                            }
                            else
                                sText = "." + "<h style=\"color:red\">系统在第" + n.CID + "期投入" + n.Prize + "" + ub.Get("SiteBz") + "|结余" + n.allmoney + ub.Get("SiteBz") + "(" + Convert.ToDateTime(n.AddTime).ToString("yyyy-MM-dd HH:mm:ss") + ")</h>";
                        }
                    }
                }
                else
                {
                    if (n.WinPrize == 0)
                    {
                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.usID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(Convert.ToInt32(n.usID)) + "</a>在第" + n.CID + "期投注消费" + n.Prize + "" + ub.Get("SiteBz") + "|结余" + n.allmoney + ub.Get("SiteBz") + "(" + Convert.ToDateTime(n.AddTime).ToString("yyyy-MM-dd HH:mm:ss") + ")";
                    }
                    else
                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.usID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(Convert.ToInt32(n.usID)) + "</a><h style=\"color:red\">在第" + n.CID + "期" + n.other + "" + ub.Get("SiteBz") + "|结余" + n.allmoney + ub.Get("SiteBz") + "(" + Convert.ToDateTime(n.AddTime).ToString("yyyy-MM-dd HH:mm:ss") + ")</h>";
                }

                builder.AppendFormat("<a href=\"" + Utils.getUrl("BQC.aspx?act=prize&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">  </a>" + d + sText + "   ");

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


        builder.Append(Out.Tab("<div class=\"hr\"></div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("BQC.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div>", ""));
        GameFoot();
    }
    #endregion

    //判断单式复式
    private string IsFS(int? voteNum, int? isPrize, int prize2Num)
    {

        int totalresult;
        if (isPrize == 1)
            totalresult = (int)isPrize + prize2Num;
        if (isPrize == 2)
            totalresult = prize2Num;
        else
            totalresult = 0;
        if (voteNum > 1)
            return "复式" + voteNum + "注中" + totalresult;
        else
            return "单式" + voteNum;
    }

    #region 兑奖中心
    private void CasePage()
    {
        Master.Title = "兑奖中心";

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href =\"" + Utils.getUrl("BQC.aspx") + "\">" + ub.GetSub("BQCName", xmlPath) + "</a>&gt;兑奖中心");
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
        IList<BCW.BQC.Model.BQCPay> listBQCPay = new BCW.BQC.BLL.BQCPay().GetBQCPays(pageIndex, pageSize, strWhere, out recordCount);
        if (listBQCPay.Count > 0)
        {
            int k = 1;
            foreach (BCW.BQC.Model.BQCPay n in listBQCPay)
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
                builder.Append("押" + n.Vote + "/下注" + n.PayCents + "" + ub.Get("SiteBz") + "/赢" + n.WinCent + "" + ub.Get("SiteBz") + "[" + DT.FormatDate(Convert.ToDateTime(n.AddTime), 13) + "]<a href=\"" + Utils.getUrl("BQC.aspx?act=caseok&amp;pid=" + n.id + "") + "\">兑奖</a>");

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
            string strOthe = "本页兑奖,BQC.aspx,post,0,red";
            builder.Append(Out.wapform(strName, strValu, strOthe));
        }
        //builder.Append(Out.Tab("<div>", Out.Hr()));
        //builder.Append("<a href=\"" + Utils.getUrl("BQC.aspx?act=mylist&amp;ptype=0") + "\">未开投注</a> ");
        //builder.Append("<a href=\"" + Utils.getUrl("BQC.aspx?act=mylist&amp;ptype=1") + "\">历史投注</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("BQC.aspx") + "\">" + ub.GetSub("BQCName", xmlPath) + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    private void CaseOkPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int pid = Utils.ParseInt(Utils.GetRequest("pid", "get", 2, @"^[0-9]\d*$", "选择兑奖无效"));

        if (new BCW.BQC.BLL.BQCPay().ExistsState(pid, meid))
        {
            //操作币
            long winMoney = Convert.ToInt64(new BCW.BQC.BLL.BQCPay().GetWinCent(pid));
            //税率
            long SysTax = 0;
            //期号
            BCW.BQC.Model.BQCPay model = new BCW.BQC.BLL.BQCPay().GetBQCPay(pid);
            long number = Convert.ToInt64(model.CID);
            winMoney = winMoney - SysTax;
            BCW.User.Users.IsFresh("BQC", 1);//防刷
            new BCW.BQC.BLL.BQCPay().UpdateState(pid);
            new BCW.BLL.User().UpdateiGold(meid, winMoney, "" + GameName + "兑奖-期号-" + number + "-标识ID" + pid + "");
            Utils.Success("兑奖", "恭喜，成功兑奖" + winMoney + "" + ub.Get("SiteBz") + "", Utils.getUrl("BQC.aspx?act=case"), "1");
        }
        else
        {
            Utils.Success("兑奖", "恭喜，重复兑奖或没有可以兑奖的记录", Utils.getUrl("BQC.aspx?act=case"), "1");
        }
    }
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
        BCW.User.Users.IsFresh("BQC", 1);//防刷
        for (int i = 0; i < strArrId.Length; i++)
        {
            int pid = Convert.ToInt32(strArrId[i]);
            if (new BCW.BQC.BLL.BQCPay().ExistsState(pid, meid))
            {
                new BCW.BQC.BLL.BQCPay().UpdateState(pid);
                //操作币
                winMoney = Convert.ToInt64(new BCW.BQC.BLL.BQCPay().GetWinCent(pid));
                //税率
                long SysTax = 0;
                winMoney = winMoney - SysTax;
                //期号
                BCW.BQC.Model.BQCPay model = new BCW.BQC.BLL.BQCPay().GetBQCPay(pid);
                long number = Convert.ToInt64(model.CID);
                new BCW.BLL.User().UpdateiGold(meid, winMoney, "" + GameName + "兑奖-期号-" + number + "-标识ID" + pid + "");
            }
            getwinMoney = getwinMoney + winMoney;
        }
        Utils.Success("兑奖", "恭喜，成功兑奖" + getwinMoney + "" + ub.Get("SiteBz") + "", Utils.getUrl("BQC.aspx?act=case"), "2");
    }
    #endregion

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
        long Nextprize = new BCW.BQC.DAL.BQCList().getnextprize(lastcid);//LastOpenCID()

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

    private string SPF(string Types)
    {
        string TyName = string.Empty;

        if (Types == "3-3")
            TyName = "胜胜";
        else if (Types == "3-1")
            TyName = "胜平";
        else if (Types == "3-0")
            TyName = "胜负";

        else if (Types == "1-3")
            TyName = "平胜";
        else if (Types == "1-1")
            TyName = "平平";
        else if (Types == "1-0")
            TyName = "平负";

        else if (Types == "0-3")
            TyName = "负胜";
        else if (Types == "0-1")
            TyName = "负平";
        else if (Types == "0-0")
            TyName = "负负";

        else
            TyName = "*";

        return TyName;
    }

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
        int si = 0; int wu = 0; int liu = 0; int qi = 0; int ba = 0; int jiu = 0;
        double zhu = 0;
        for (int i = 0; i < temp.Length; i++)
        {
            if (temp[i] == 1)
                yi++;
            else if (temp[i] == 2)
                er++;
            else if (temp[i] == 3)
                san++;
            else if (temp[i] == 4)
                si++;
            else if (temp[i] == 5)
                wu++;
            else if (temp[i] == 6)
                liu++;
            else if (temp[i] == 7)
                qi++;
            else if (temp[i] == 8)
                ba++;
            else
                jiu++;
        }
        zhu = (Math.Pow(1, Convert.ToDouble(yi))) * (Math.Pow(2, Convert.ToDouble(er))) * (Math.Pow(3, Convert.ToDouble(san)) * Math.Pow(4, Convert.ToDouble(si))) * (Math.Pow(5, Convert.ToDouble(wu))) * (Math.Pow(6, Convert.ToDouble(liu)) * Math.Pow(7, Convert.ToDouble(qi))) * (Math.Pow(8, Convert.ToDouble(ba))) * (Math.Pow(9, Convert.ToDouble(jiu)));
        return zhu;
    }

    private string vote(string[] aa)
    {
        string Vote = "(";
        int i = 0;
        while (i < aa.Length)
        {
            Vote += aa[i] + "/";
            i++;
        }
        Vote = Vote.TrimEnd('/') + ")";
        return Vote;
    }

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
    private void kuai(int uid, int type, int ptype, int peilv, long Price, string Num0, string Num1, string Num2, string Num3, string Num4, string Num5, int id)//用户，游戏编号，下注类型|特殊传值1.2
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

                    builder.Append("<a style=\"hidden\" href =\"" + Utils.getUrl("BQC.aspx?act=pay&amp;CID=" + ptype + "&amp;id=" + id + "&amp;peilv=" + Convert.ToInt64(kuai[i]) + "&amp;Num0=" + Num0 + "&amp;Num1=" + Num1 + "&amp;Num2=" + Num2 + "&amp;Num3=" + Num3 + "&amp;Num4=" + Num4 + "&amp;Num5=" + Num5 + "") + "\">" + gold + "</a>" + "|");//    
                }
            }

            builder.Append("<a href=\"" + Utils.getUrl("QuickBet.aspx?act=edit&amp;type=" + type + "&amp;ptype=" + ptype + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">添删</a>");

        }
        catch { }
        #endregion
    }
    #endregion

    //判断数字
    public bool bolNum(string temp)
    {
        byte tempByte = 0;
        for (int i = 0; i < temp.Length; i++)
        {
            try
            {
                tempByte = Convert.ToByte(temp[i]);
            }
            catch { }
            if ((tempByte < 48) || (tempByte > 57))
                return false;
        }
        return true;
    }
}