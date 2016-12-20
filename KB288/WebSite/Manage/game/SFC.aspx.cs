using System;
using System.Collections.Generic;
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
using BCW.Common;

/// <summary>
/// 2016/8/12 [管理]功能数据未完全添加
/// 2016/9/24 无开奖
/// /// 20161007 蒙宗将 优化获取上期
/// 蒙宗将 20161011 增加退
/// 蒙宗将 20161111 状态修复，增加系统投注
/// 蒙宗将 20161112 优化开奖 1118 返赢反负
/// </summary>

public partial class Manage_game_SFC : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/SFC.xml";
    ub xml = new ub();
    string GameName = ub.GetSub("SFName", "/Controls/SFC.xml");

    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "back":
                BackPage();
                break;
            case "backsave":
            case "backsave2":
                BackSavePage(act);
                break;
            case "backsave1":
            case "backsave3":
                BackSave1Page(act);
                break;
            case "add":
                AddPage();
                break;
            case "addsave":
                AddSavePage();
                break;
            case "open":
                OpenPage();
                break;
            case "opensave":
                OpenSavePage();
                break;
            case "edit":
                EditPage();
                break;
            case "editsave":
                EditSavePage();
                break;
            case "del":
                DelPage();
                break;
            case "view":
                ViewPage();
                break;
            case "jieguo":
                JieguoPage();
                break;
            case "chaxun":
                ChaxunPage();//查询期号下注与中奖信息
                break;
            case "usidcx":
                UsidcxPage();//根据用户ＩＤ查询
                break;
            case "jiangcx":
                JiangcxPage();//奖池查询
                break;
            case "reset":
                ResetPage();
                break;
            case "top":
                TopPage();
                break;
            case "rebot":
                ReBot();
                break;
            case "stat":
                StatPage();
                break;
            case "prizelist":
                PrizeListPage();
                break;
            case "set":
                SetPage();
                break;
            case "setceshi":
                SetStatueCeshi();//内测设置
                break;
            case "teshu":
                TeShuPage();//特殊开奖
                break;
            case "tui":
                TuiPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }
    //管理首页
    private void ReloadPage()
    {
        Master.Title = "" + ub.GetSub("SFName", xmlPath) + "管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        builder.Append(ub.GetSub("SFName", xmlPath));
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        strWhere = "";
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.SFC.Model.SfList> listSFList = new BCW.SFC.BLL.SfList().GetSfLists(pageIndex, pageSize, strWhere, out recordCount);
        if (listSFList.Count > 0)
        {
            int k = 1;
            foreach (BCW.SFC.Model.SfList n in listSFList)
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
                builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx?act=edit&amp;id=" + n.id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[管理]&gt;</a>");

                if (n.State == 0)
                {
                    builder.Append("第" + n.CID + "期开出:<a href=\"" + Utils.getUrl("SFC.aspx?act=view&amp;id=" + n.id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">未开</a> <a href=\"" + Utils.getUrl("SFC.aspx?act=open&amp;id=" + n.id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">开奖</a>");
                }
                else
                {
                    builder.Append("第" + n.CID + "期开出:<a href=\"" + Utils.getUrl("SFC.aspx?act=view&amp;id=" + n.id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.Result + "" + "</a>");
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
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));

        builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx?act=add") + "\">开通下期</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx?act=set") + "\">游戏配置</a><br />");
        builder.Append("<a href =\"" + Utils.getUrl("SFC.aspx?act=setceshi") + "\">测试配置</a><br />");
        //builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx?act=rebot") + "\">管机器人</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx?act=top") + "\">游戏排行</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx?act=prizelist") + "\">奖池记录</a><br />");
        builder.Append("<a href =\"" + Utils.getUrl("SFC.aspx?act=chaxun") + "\">游戏查询</a><br />");
        //builder.Append("<a href =\"" + Utils.getUrl("SFC.aspx?act=usidcx") + "\">会员查询</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx?act=stat") + "\">盈利分析</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx?act=back") + "\">返赢返负</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx?act=reset") + "\">重置游戏</a>");

        builder.Append(Out.Tab("</div>", Out.Hr()));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    // 反赢反负
    private void BackPage()
    {
        Master.Title = "" + ub.GetSub("SFName", xmlPath) + "返赢返负";

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        string sfc = ub.GetSub("SFName", xmlPath);
        builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx") + "\">" + sfc + "</a>&gt;返赢返负");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("返赢返负操作");
        builder.Append(Out.Tab("</div>", "<br />"));


        builder.Append(Out.Tab("<div>", ""));
        builder.Append("返赢点操作");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "开始时间:,结束时间:,返赢千分比:,至少赢多少才返:,";
        string strName = "sTime,oTime,iTar,iPrice,act";
        string strType = "date,date,num,num,hidden";
        string strValu = "" + DT.FormatDate(DateTime.Now.AddDays(-10), 0) + "'" + DT.FormatDate(DateTime.Now, 0) + "'''backsave";
        string strEmpt = "false,false,false,false,false";
        string strIdea = "/";
        string strOthe = "马上返赢,SFC.aspx,post,1,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("返负点操作");
        builder.Append(Out.Tab("</div>", ""));

        strText = "开始时间:,结束时间:,返负千分比:,至少负多少才返:,";
        strName = "sTime,oTime,iTar,iPrice,act";
        strType = "date,date,num,num,hidden";
        strValu = "" + DT.FormatDate(DateTime.Now.AddDays(-10), 0) + "'" + DT.FormatDate(DateTime.Now, 0) + "'''backsave2";
        strEmpt = "false,false,false,false,false";
        strIdea = "/";
        strOthe = "马上返负,SFC.aspx,post,1,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("SFC.aspx") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    // 反负确认
    private void BackSavePage(string act)
    {
        DateTime sTime = Utils.ParseTime(Utils.GetRequest("sTime", "post", 2, DT.RegexTime, "开始时间填写无效"));
        DateTime oTime = Utils.ParseTime(Utils.GetRequest("oTime", "post", 2, DT.RegexTime, "结束时间填写无效"));
        int iTar = Utils.ParseInt(Utils.GetRequest("iTar", "post", 2, @"^[0-9]\d*$", "千分比填写错误"));
        int iPrice = Utils.ParseInt(Utils.GetRequest("iPrice", "post", 2, @"^[0-9]\d*$", "至少多少币才返填写错误"));

        if (act == "backsave")
        {

            builder.Append(Out.Tab("<div>", "<br />"));

            builder.Append("返赢时间段：" + sTime + "到" + oTime + "<br />");
            builder.Append("返赢千分比：" + iTar + "<br />");
            builder.Append("至少赢：" + iPrice + "币返<br />");

            DataSet ds = new BCW.SFC.BLL.SfPay().GetList("UsID,sum(WinCent-PayCents) as WinCents", "AddTime>='" + sTime + "'and AddTime<='" + oTime + "' group by UsID");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                long sum = 0; int sumi = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    long Cents = Convert.ToInt64(ds.Tables[0].Rows[i]["WinCents"]);
                    long cent = 0;
                    if (Cents > 0 && Cents >= iPrice)
                    {
                        int usid = Convert.ToInt32(ds.Tables[0].Rows[i]["UsID"]);
                        cent = Convert.ToInt64(Cents * (iTar * 0.001));
                        sumi++;
                    }
                    sum += cent;
                }
                builder.Append("本次返赢人次：" + sumi + "<br />");
                builder.Append("本次返赢金额：" + sum + "<br />");
            }

            builder.Append(Out.Tab("</div>", ""));
            string strText = "开始时间:,结束时间:,返赢千分比:,至少赢多少才返:,";
            string strName = "sTime,oTime,iTar,iPrice,act";
            string strType = "hidden,hidden,hidden,hidden,hidden";
            string strValu = "" + DT.FormatDate(sTime, 0) + "'" + DT.FormatDate(oTime, 0) + "'" + iTar + "'" + iPrice + "'backsave1";
            string strEmpt = "false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "马上返赢,SFC.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));

        }
        else
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("返负时间段：" + sTime + "到" + oTime + "<br />");
            builder.Append("返负千分比：" + iTar + "<br />");
            builder.Append("至少：" + iTar + "币返<br />");

            DataSet ds = new BCW.SFC.BLL.SfPay().GetList("UsID,sum(WinCent-PayCents) as WinCents", "AddTime>='" + sTime + "'and AddTime<='" + oTime + "' group by UsID");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                long sum = 0; int sumi = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    long Cents = Convert.ToInt64(ds.Tables[0].Rows[i]["WinCents"]);
                    long cent = 0;
                    if (Cents < 0 && Cents <= (-iPrice))
                    {
                        int usid = Convert.ToInt32(ds.Tables[0].Rows[i]["UsID"]);
                        cent = Convert.ToInt64((-Cents) * (iTar * 0.001));
                        sumi++;
                    }
                    sum += cent;
                }
                builder.Append("本次返负人次：" + sumi + "<br />");
                builder.Append("本次返负金额：" + sum + "<br />");
            }

            builder.Append(Out.Tab("</div>", ""));
            string strText = "开始时间:,结束时间:,返负千分比:,至少负多少才返:,";
            string strName = "sTime,oTime,iTar,iPrice,act";
            string strType = "hidden,hidden,hidden,hidden,hidden";
            string strValu = "" + DT.FormatDate(sTime, 0) + "'" + DT.FormatDate(oTime, 0) + "'" + iTar + "'" + iPrice + "'backsave3";
            string strEmpt = "false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "马上返负,SFC.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));

        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("SFC.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    private void BackSave1Page(string act)
    {
        DateTime sTime = Utils.ParseTime(Utils.GetRequest("sTime", "post", 2, DT.RegexTime, "开始时间填写无效"));
        DateTime oTime = Utils.ParseTime(Utils.GetRequest("oTime", "post", 2, DT.RegexTime, "结束时间填写无效"));
        int iTar = Utils.ParseInt(Utils.GetRequest("iTar", "post", 2, @"^[0-9]\d*$", "千分比填写错误"));
        int iPrice = Utils.ParseInt(Utils.GetRequest("iPrice", "post", 2, @"^[0-9]\d*$", "至少多少币才返填写错误"));
        if (act == "backsave1")
        {
            DataSet ds = new BCW.SFC.BLL.SfPay().GetList("UsID,sum(WinCent-PayCents) as WinCents", "AddTime>='" + sTime + "'and AddTime<='" + oTime + "' group by UsID");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                long Cents = Convert.ToInt64(ds.Tables[0].Rows[i]["WinCents"]);
                if (Cents > 0 && Cents >= iPrice)
                {
                    int usid = Convert.ToInt32(ds.Tables[0].Rows[i]["UsID"]);
                    long cent = Convert.ToInt64(Cents * (iTar * 0.001));
                    new BCW.BLL.User().UpdateiGold(usid, cent, "" + GameName + "返赢");
                    //发内线
                    string strLog = "根据您近期" + ub.GetSub("SFName", xmlPath) + "排行榜上的赢利情况，系统自动返还了" + cent + "" + ub.Get("SiteBz") + "[url=/bbs/game/SFC.aspx]进入" + ub.GetSub("SFName", xmlPath) + "[/url]";

                    new BCW.BLL.Guest().Add(0, usid, new BCW.BLL.User().GetUsName(usid), strLog);
                }
            }

            long sum = 0; int sumi = 0;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                long Cents = Convert.ToInt64(ds.Tables[0].Rows[i]["WinCents"]);
                long cent = 0;
                if (Cents > 0 && Cents >= iPrice)
                {
                    int usid = Convert.ToInt32(ds.Tables[0].Rows[i]["UsID"]);
                    cent = Convert.ToInt64(Cents * (iTar * 0.001));
                    sumi++;
                }
                sum += cent;
            }
            new BCW.BLL.User().UpdateiGold(726, new BCW.BLL.User().GetUsName(726), 0, "" + GameName + sTime + "-" + oTime + "返赢|千分比" + iTar + "|至少赢" + iPrice + "|共计人次" + sumi + "|共返" + sum + "|此记录为返赢记录,不操作726的币数");


            Utils.Success("返赢操作", "返赢操作成功", Utils.getUrl("SFC.aspx"), "1");
        }
        else
        {
            DataSet ds = new BCW.SFC.BLL.SfPay().GetList("UsID,sum(WinCent-PayCents) as WinCents", "AddTime>='" + sTime + "'and AddTime<='" + oTime + "' group by UsID");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                long Cents = Convert.ToInt64(ds.Tables[0].Rows[i]["WinCents"]);
                if (Cents < 0 && Cents < (-iPrice))
                {
                    int usid = Convert.ToInt32(ds.Tables[0].Rows[i]["UsID"]);
                    long cent = Convert.ToInt64((-Cents) * (iTar * 0.001));
                    new BCW.BLL.User().UpdateiGold(usid, cent, "" + GameName + "返负");
                    //发内线
                    string strLog = "根据你近期" + ub.GetSub("SFName", xmlPath) + "排行榜上的亏损情况，系统自动返还了" + cent + "" + ub.Get("SiteBz") + "[url=/bbs/game/SFC.aspx]进入" + ub.GetSub("SFName", xmlPath) + "[/url]";
                    new BCW.BLL.Guest().Add(0, usid, new BCW.BLL.User().GetUsName(usid), strLog);
                }
            }

            long sum = 0; int sumi = 0;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                long Cents = Convert.ToInt64(ds.Tables[0].Rows[i]["WinCents"]);
                long cent = 0;
                if (Cents < 0 && Cents <= (-iPrice))
                {
                    int usid = Convert.ToInt32(ds.Tables[0].Rows[i]["UsID"]);
                    cent = Convert.ToInt64((-Cents) * (iTar * 0.001));
                    sumi++;
                }
                sum += cent;
            }
            new BCW.BLL.User().UpdateiGold(726, new BCW.BLL.User().GetUsName(726), 0, "" + GameName + sTime + "-" + oTime + "返负|千分比" + iTar + "|至少负" + iPrice + "|共计人次" + sumi + "|共返" + sum + "|此记录为返负记录,不操作726的币数");

            Utils.Success("返负操作", "返负操作成功", Utils.getUrl("SFC.aspx"), "1");
        }

    }
    // 游戏设置
    private void SetPage()
    {
        Master.Title = "游戏设置";
        string xmlPath = "/Controls/SFC.xml";

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        string sfc = ub.GetSub("SFName", xmlPath);
        builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx") + "\">" + sfc + "</a>&gt;游戏设置");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-1]$", "0"));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();

        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            if (ptype == 0)
            {
                string SFName = Utils.GetRequest("SFName", "post", 2, @"^[^\^]{1,20}$", "系统名称限1-20字内");

                string SFStatus = Utils.GetRequest("SFStatus", "post", 2, @"^[0-2]$", "系统状态选择出错");
                string SFExpir = Utils.GetRequest("SFExpir", "post", 2, @"^[0-9]\d*$", "游戏防刷填写错误");
                string SFmallPay = Utils.GetRequest("SFmallPay", "post", 3, @"^[0-9]\d*$", "最小下注填写错误");
                string SFBigPay = Utils.GetRequest("SFBigPay", "post", 3, @"^[0-9]\d*$", "最大下注填写错误");
                string SFTopUbb = Utils.GetRequest("SFTopUbb", "post", 3, @"^[\s\S]{1,2000}$", "顶部Ubb限2000字内");
                string SFFootUbb = Utils.GetRequest("SFFootUbb", "post", 3, @"^[\s\S]{1,2000}$", "底部Ubb限2000字内");
                //string SFCDemoIDS = Utils.GetRequest("SFCDemoIDS", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "内测ID填写格式错误");
                string SFrule = Utils.GetRequest("SFrule", "post", 3, @"^[\s\S]{1,5000}$", "规则最限5000字内");
                string SFMax = Utils.GetRequest("SFMax", "post", 4, @"^[0-9]\d*$", "最大奖池填写错误");
                string SFMin = Utils.GetRequest("SFMin", "post", 4, @"^[0-9]\d*$", "最小奖池填写错误");
                string SFCSend = Utils.GetRequest("SFCSend", "post", 2, @"^[0-9]\d*$", "系统投入金额填写错误");
                string SFprice = Utils.GetRequest("SFprice", "post", 2, @"^[0-9]\d*$", "每注金额填写错误");
                string SFCprice = Utils.GetRequest("SFCprice", "post", 2, @"^[0-9]\d*$", "每期个人最多投注金额填写错误");

                xml.dss["SFName"] = SFName;
                xml.dss["SFStatus"] = SFStatus;
                xml.dss["SFExpir"] = SFExpir;
                xml.dss["SFmallPay"] = SFmallPay;
                xml.dss["SFBigPay"] = SFBigPay;
                xml.dss["SFTopUbb"] = SFTopUbb;
                xml.dss["SFFootUbb"] = SFFootUbb;
                //xml.dss["SFCDemoIDS"] = SFCDemoIDS;
                xml.dss["SFrule"] = SFrule;
                xml.dss["SFMax"] = SFMax;
                xml.dss["SFMin"] = SFMin;
                xml.dss["SFCSend"] = SFCSend;
                xml.dss["SFprice"] = SFprice;
                xml.dss["SFCprice"] = SFCprice;
            }
            else
            {
                string odds1 = Utils.GetRequest("odds1", "post", 2, @"^[0-9]\d*$", "一等奖赔率填写错误");
                string odds2 = Utils.GetRequest("odds2", "post", 2, @"^[0-9]\d*$", "二等奖赔率填写错误");
                string odds3 = Utils.GetRequest("odds3", "post", 2, @"^[0-9]\d*$", "三等奖赔率填写错误");
                string odds4 = Utils.GetRequest("odds4", "post", 2, @"^[0-9]\d*$", "四等奖赔率填写错误");
                string odds5 = Utils.GetRequest("odds5", "post", 2, @"^[0-9]\d*$", "五等奖赔率填写错误");
                string odds6 = Utils.GetRequest("odds6", "post", 2, @"^[0-9]\d*$", "六等奖赔率填写错误");
                string odds7 = Utils.GetRequest("odds7", "post", 2, @"^[0-9]\d*$", "七等奖赔率填写错误");
                string odds8 = Utils.GetRequest("odds8", "post", 2, @"^[0-9]\d*$", "系统每次收费填写错误");

                xml.dss["SFOne"] = odds1;
                xml.dss["SFTwo"] = odds2;
                xml.dss["SFThree"] = odds3;
                xml.dss["SFforc"] = odds4;
                xml.dss["SFFive"] = odds5;
                xml.dss["SFSix"] = odds6;
                xml.dss["SFSeven"] = odds7;
                xml.dss["SFsys"] = odds8;

            }
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("游戏设置", "设置成功，正在返回..", Utils.getUrl("SFC.aspx?act=set&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            if (ptype == 0)
            {
                builder.Append(Out.Div("", "游戏设置"));
                builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                builder.Append("设置|<a href=\"" + Utils.getUrl("SFC.aspx?act=set&amp;ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">赔率</a>");
                builder.Append(Out.Tab("</div>", ""));
                string strText = "游戏名称:/,游戏状态:/,游戏下注防刷(秒):/,最小下注:/,最大下注:/,顶部Ubb:/,底部Ubb:/,游戏规则:(支持UBB)/,奖池上限:（系统有投入时系统回收上限，即奖池达到额度有系统投入则回收）/,奖池下限:（系统投入下限，即当奖池不足额度时系统投入）/,系统投入金额:/,单注金额:/,每期个人限投金额:/,";
                string strName = "SFName,SFStatus,SFExpir,SFmallPay,SFBigPay,SFTopUbb,SFFootUbb,SFrule,SFMax,SFMin,SFCSend,SFprice,SFCprice,backurl";
                string strType = "text,select,num,hidden,hidden,textarea,textarea,textarea,num,num,num,num,num,hidden";
                string strValu = "" + xml.dss["SFName"] + "'" + xml.dss["SFStatus"] + "'" + xml.dss["SFExpir"] + "'" + xml.dss["SFmallPay"] + "'" + xml.dss["SFBigPay"] + "'" + xml.dss["SFTopUbb"] + "'" + xml.dss["SFFootUbb"] + "'" + xml.dss["SFrule"] + "'" + xml.dss["SFMax"] + "'" + xml.dss["SFMin"] + "'" + xml.dss["SFCSend"] + "'" + xml.dss["SFprice"] + "'" + xml.dss["SFCprice"] + "'" + Utils.getPage(0) + "";
                string strEmpt = "false,0|正常|1|维护|2|内测,false,false,false,true,true,true,false,false,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,SFC.aspx?act=set,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
            else
            {
                builder.Append(Out.Div("", "赔率设置"));
                builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx?act=set&amp;ptype=0&amp;backurl=" + Utils.getPage(0) + "") + "\">设置</a>|赔率");
                builder.Append(Out.Tab("</div>", ""));
                string strText = "=赔率设置=/一等奖赔率%:/,二等奖赔率%:/,三等奖赔率%:/,四等奖赔率%:/,五等奖赔率%:/,六等奖赔率%:/,七等奖赔率%:/,系统每期收费%:/,,";
                string strName = "odds1,odds2,odds3,odds4,odds5,odds6,odds7,odds8,ptype,backurl";
                string strType = "text,text,text,text,text,text,text,text,hidden,hidden";
                string strValu = "" + xml.dss["SFOne"] + "'" + xml.dss["SFTwo"] + "'" + xml.dss["SFThree"] + "'" + xml.dss["SFForc"] + "'" + xml.dss["SFFive"] + "'" + xml.dss["SFSix"] + "'" + xml.dss["SFSeven"] + "'" + xml.dss["SFsys"] + "'" + ptype + "'" + Utils.getPage(0) + "";
                string strEmpt = "false,false,false,false,false,false,false,false,false,false,";
                string strIdea = "/";
                string strOthe = "确定修改|reset,SFC.aspx?act=set,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("SFC.aspx") + "\">返回上级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
    //游戏内测管理
    private void SetStatueCeshi()
    {
        Master.Title = "" + GameName + "设置测试状态";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx") + "\">" + GameName + "管理</a>&gt;");
        builder.Append("设置测试状态");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b>" + GameName + "内测管理：</b>");
        builder.Append(Out.Tab("</div>", "<br />"));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/SFC.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string SFStatus = Utils.GetRequest("SFStatus", "post", 2, @"^[0-9]\d*$", "测试权限管理输入出错");
            string SFCDemoIDS = Utils.GetRequest("SFCDemoIDS", "all", 2, @"^[^\^]{1,2000}$", "请输入测试号");
            xml.dss["SFStatus"] = SFStatus;
            xml.dss["SFCDemoIDS"] = SFCDemoIDS.Replace("\r\n", "").Replace(" ", "");
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("" + GameName + "设置", "内测设置成功，正在返回..", Utils.getUrl("SFC.aspx?act=setceshi"), "2");
        }
        else
        {
            string strText = "测试权限管理:/,添加测试号(多账户用#分隔):/,";
            string strName = "SFStatus,SFCDemoIDS,backurl";
            string strType = "select,textarea,hidden";
            string strValu = xml.dss["SFStatus"] + "'" + xml.dss["SFCDemoIDS"] + "'" + Utils.getPage(0) + "";
            string strEmpt = "0|开放|1|维护|2|内测,true";
            string strIdea = "/";
            string strOthe = "确定修改,SFC.aspx?act=setceshi,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            string SFCDemoIDS = Convert.ToString(ub.GetSub("SFCDemoIDS", xmlPath));
            string[] name = SFCDemoIDS.Split('#');
            // foreach (string n in imgNum)
            builder.Append("当前测试号:<br />");
            for (int n = 0; n < name.Length; n++)
            {
                if ((n + 1) % 5 == 0)
                {
                    builder.Append(name[n] + "," + "<br />");
                }
                else
                {
                    builder.Append(name[n] + ",");
                }
            }
            builder.Append(Out.Tab("</div>", "<br/>"));


            builder.Append(Out.Tab("<div>", ""));
            builder.Append("" + "-----------" + "<br />");
            builder.Append(Out.Tab("</div>", ""));

            string ac1 = Utils.GetRequest("ac", "all", 1, "", "");
            Application.Remove(xmlPath);//清缓存
            xml.ReloadSub(xmlPath); //加载配置
            if (Utils.ToSChinese(ac1) == "确定设置")
            {
                string RoBotID = Utils.GetRequest("RoBotID", "post", 1, "", xml.dss["RoBotID"].ToString());
                string IsBot = Utils.GetRequest("IsBot", "post", 1, @"^[0-1]$", xml.dss["IsBot"].ToString());
                string RoBotCost = Utils.GetRequest("RoBotCost", "post", 1, "", xml.dss["RoBotCost"].ToString());
                string RoBotCount = Utils.GetRequest("RoBotCount", "post", 1, @"^[0-9]\d*$", xml.dss["RoBotCount"].ToString());
                xml.dss["RoBotID"] = RoBotID;
                xml.dss["IsBot"] = IsBot;
                xml.dss["RoBotCost"] = RoBotCost;
                xml.dss["RoBotCount"] = RoBotCount;
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                Utils.Success("" + GameName + "设置", "机器人管理成功，正在返回..", Utils.getUrl("SFC.aspx?act=setceshi"), "2");
            }
            else
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<b>机器人管理：</b>");
                builder.Append(Out.Tab("</div>", "<br />"));

                string strText1 = "机器人ID(多个机器人用#分隔):/,机器人状态:/,机器人单注投注倍数:/,机器人每期购买次数:/";
                string strName1 = "RoBotID,IsBot,RoBotCost,RoBotCount";
                string strType1 = "textarea,select,text,text";
                string strValu1 = xml.dss["RoBotID"].ToString() + "'" + xml.dss["IsBot"].ToString() + "'" + xml.dss["RoBotCost"].ToString() + "'" + xml.dss["RoBotCount"].ToString();
                string strEmpt1 = "true,0|关闭|1|开启,true,false";
                string strIdea1 = "/";
                string strOthe1 = "确定设置,SFC.aspx?act=setceshi,post,1,red";
                builder.Append(Out.wapform(strText1, strName1, strType1, strValu1, strEmpt1, strIdea1, strOthe1));
                builder.Append(Out.Tab("<div>", "<br />"));
                string SFCRobotIDS = Convert.ToString(ub.GetSub("RoBotID", xmlPath));
                string[] name1 = SFCRobotIDS.Split('#');
                string name2 = string.Empty;
                for (int n = 0; n < name1.Length; n++)
                {
                    if ((n + 1) % 5 == 0)
                        name2 = name2 + name1[n] + "," + "<br />";
                    else
                        name2 = name2 + name1[n] + ",";
                }
                builder.Append("当前机器人ID为：<br /><b style=\"color:red\">" + name2 + "</b><br />");
                if (xml.dss["IsBot"].ToString() == "0")
                {
                    builder.Append("机器人状态：<b style=\"color:red\">关闭</b><br />");
                }
                else
                {
                    builder.Append("当前机器人状态：<b style=\"color:red\">开启</b><br />");
                }
                builder.Append("当前机器人单注投注金额：<b style=\"color:red\">" + xml.dss["RoBotCost"].ToString() + "</b><br />");
                builder.Append("当前机器人每期限购买彩票次数：<b style=\"color:red\">" + xml.dss["RoBotCount"].ToString() + "</b><br />");
                builder.Append("<b>温馨提示:请注意内测设置与机器人设置分开设置</b>");
                builder.Append(Out.Tab("</div>", "<br />"));
            }

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("" + "-----------" + "<br />");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx") + "\">返回" + GameName + "管理</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
    //机器人ID设置
    private void ReBot()
    {
        Master.Title = "机器人设置";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx") + "\">" + GameName + "</a>&gt;");
        builder.Append("机器人设置");
        builder.Append(Out.Tab("</div>", "<br />"));

        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        string RoBotID = Utils.GetRequest("RoBotID", "post", 1, "", xml.dss["RoBotID"].ToString());
        string IsBot = Utils.GetRequest("IsBot", "post", 1, @"^[0-1]$", xml.dss["IsBot"].ToString());
        string RoBotCost = Utils.GetRequest("RoBotCost", "post", 1, @"^[0-9]\d*$", xml.dss["RoBotCost"].ToString());
        string RoBotCount = Utils.GetRequest("RoBotCount", "post", 1, @"^[0-9]\d*$", xml.dss["RoBotCount"].ToString());
        xml.dss["RoBotID"] = RoBotID.Replace("\r\n", "").Replace(" ", "");
        xml.dss["IsBot"] = IsBot;
        xml.dss["RoBotCost"] = RoBotCost.Replace("\r\n", "").Replace(" ", "");
        xml.dss["RoBotCount"] = RoBotCount;
        System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
        string strText = "机器人ID:/,机器人状态:/,机器人单注投注倍数范围:/,机器人每期购买次数:/";
        string strName = "RoBotID,IsBot,RoBotCost,RoBotCount";
        string strType = "big,select,text,text";
        string strValu = xml.dss["RoBotID"].ToString() + "'" + xml.dss["IsBot"].ToString() + "'" + xml.dss["RoBotCost"].ToString() + "'" + xml.dss["RoBotCount"].ToString();
        string strEmpt = "true,0|关闭|1|开启,true,false";
        string strIdea = "/";
        string strOthe = "确定修改,SFC.aspx?act=rebot,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<br />温馨提示:多个机器人ID请用#分隔。<br />");
        builder.Append("当前机器人ID为：<b style=\"color:red\">" + xml.dss["RoBotID"].ToString() + "</b><br />");
        if (xml.dss["IsBot"].ToString() == "0")
        {
            builder.Append("机器人状态：<b style=\"color:red\">关闭</b><br />");
        }
        else
        {
            builder.Append("当前机器人状态：<b style=\"color:red\">开启</b><br />");
        }
        builder.Append("当前机器人单注投注金额：<b style=\"color:red\">" + xml.dss["RoBotCost"].ToString() + "</b><br />");
        builder.Append("当前机器人每期限购买彩票次数：<b style=\"color:red\">" + xml.dss["RoBotCount"].ToString() + "</b><br />");
        builder.Append(Out.Tab("</div class=\"hr\">", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    // 开通下一期
    private void AddPage()
    {
        Master.Title = "开通";

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        string sfc = ub.GetSub("SFName", xmlPath);
        builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx") + "\">" + sfc + "</a>&gt;开通期数");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("开通期数");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "期数:/,赛事:(14场)/,主场:(14场)/,客场:(14场)/,比赛时间:(14场)/,开售时间:（开始下注时间，格式" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "）/,截止时间:（下注截止时间，格式" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "）/,,";
        string strName = "CID,Match,Home,Away,Stime,StartTime,EndTime,act,backurl";
        string strType = "num,text,text,text,text,date,date,hidden,hidden";
        string strValu = "'''''" + "'" + "'" + "''" + "'addsave'" + Utils.getPage(0) + "";
        string strEmpt = "ture,ture,ture,ture,ture,ture,ture,false,false,false,false,false,false,false";
        string strIdea = "/";
        string strOthe = "确定开通|reset,SFC.aspx?act=addsave,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("填写说明：<br />");
        builder.Append("赛事如：瑞超,瑞超,瑞超,瑞超,挪超,挪超,挪超,挪超,挪超,巴甲,巴甲,巴甲,巴甲,巴甲,<br />");
        builder.Append("主场如：索尔纳,哈马比,埃夫斯堡,厄勒布鲁,斯特罗姆,博德闪耀,莫尔德,斯塔贝克,萨普斯堡,米内罗,巴竞技,科林蒂安,费古埃伦,弗鲁米嫩,<br />");
        builder.Append("客场如：马尔默,法尔肯堡,哥德堡,赫根,特罗姆瑟,奥德,斯达,布兰,利勒斯特,圣克鲁斯,维多利亚,圣保罗,沙佩科,克鲁塞罗,<br />");
        builder.Append("比赛时间如：2016-07-17 23:00:00,2016-07-17 23:30:00,2016-07-17 21:00:00,2016-07-17 23:30:00,2016-07-17 21:30:00,2016-07-18 00:00:00,2016-07-18 00:00:00,2016-07-18 00:00:00,2016-07-18 02:00:00,2016-07-17 22:00:00,2016-07-18 03:00:00,2016-07-18 03:00:00,2016-07-18 03:00:00,2016-07-18 03:00:00,<br />");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("SFC.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    private void AddSavePage()
    {

        string Match = Utils.GetRequest("Match", "post", 2, @"^", "赛事填写错误！");
        string Home = Utils.GetRequest("Home", "post", 2, @"^", "主场填写错误！");
        string Away = Utils.GetRequest("Away", "post", 2, @"^", "客场填写错误！");
        string Stime = Utils.GetRequest("Stime", "post", 2, @"^", "比赛时间填写错误！");
        string StartTime = (Utils.GetRequest("StartTime", "post", 2, DT.RegexTime, "开始时间格式填写出错,正确格式如" + DT.FormatDate(DateTime.Now, 0) + ""));
        int CID = int.Parse(Utils.GetRequest("CID", "post", 2, @"^[0-9]\d*$", "期数填写错误"));
        string EndTime = (Utils.GetRequest("EndTime", "post", 2, DT.RegexTime, "截止时间格式填写出错,正确格式如" + DT.FormatDate(DateTime.Now, 0) + ""));
        BCW.SFC.Model.SfList model = new BCW.SFC.Model.SfList();
        DataSet ds1 = new BCW.SFC.BLL.SfList().GetList("TOP 1 CID", "State=0 order by CID DESC");
        DataSet ds2 = new BCW.SFC.BLL.SfList().GetList("Top 1 CID", "State=1 order by CID DESC");
        DataSet ds3 = new BCW.SFC.BLL.SfList().GetList("Top 1 State", "id>0 order by CID DESC");
        int CID1 = 0;
        int CID2 = 0;
        int state = 3;
        try
        {
            CID1 = int.Parse(ds1.Tables[0].Rows[0][0].ToString());
        }
        catch
        {
            builder.Append("<b>.</b>");
        }
        try
        {
            CID2 = int.Parse(ds2.Tables[0].Rows[0][0].ToString());
        }
        catch
        {
            builder.Append("<b>.</b>");
        }
        try
        {
            state = int.Parse(ds3.Tables[0].Rows[0][0].ToString());
        }
        catch
        {
            builder.Append("<b>.</b>");
        }
        if (CID < CID1)
            Utils.Error("开出的最新期数应比上一期数大", "");
        else
        {
            if (new BCW.SFC.DAL.SfList().Existsjilu())
            {
                if (state == 0)//如果最新一期未开奖
                {
                    model.CID = CID;
                    model.EndTime = Convert.ToDateTime(EndTime);
                    model.Match = Match;
                    model.Team_Home = Home;
                    model.Team_Away = Away;
                    model.Result = "";
                    model.Sale_StartTime = Convert.ToDateTime(StartTime);
                    model.Start_time = Stime;
                    model.Score = "";
                    model.other = "";
                    model.State = 0;
                    model.PayCent = 0;
                    model.PayCount = 0;
                    model.nowprize = 0;
                    model.nextprize = 0;
                    model.sysprize = 0;
                    model.sysprizestatue = 0;
                    model.sysdayprize = 0;
                    new BCW.SFC.BLL.SfList().Add(model);
                }
                else if (state == 1)
                {
                    model.CID = CID;
                    model.EndTime = Convert.ToDateTime(EndTime);
                    model.Match = Match;
                    model.Team_Home = Home;
                    model.Team_Away = Away;
                    model.Result = "";
                    model.Sale_StartTime = Convert.ToDateTime(StartTime);
                    model.Start_time = Stime;
                    model.Score = "";
                    model.other = "";
                    model.State = 0;
                    model.PayCent = 0;
                    model.PayCount = 0;
                    model.nowprize = 0;
                    model.nextprize = 0;
                    if (new BCW.SFC.DAL.SfList().getnextprize(CID2) > Convert.ToInt64(ub.GetSub("SFMin", "/Controls/SFC.xml")))
                    {
                        model.sysprize = 0;
                        model.sysprizestatue = 0;
                    }
                    else
                    {
                        model.sysprize = Convert.ToInt64(ub.GetSub("SFMin", "/Controls/SFC.xml"));
                        model.sysprizestatue = 1;//表示系统有投入
                    }
                    model.sysdayprize = 0;
                    new BCW.SFC.BLL.SfList().Add(model);

                    if (new BCW.SFC.DAL.SfList().getnextprize(CID2) > Convert.ToInt64(ub.GetSub("SFMin", "/Controls/SFC.xml")))
                    {
                        //把记录加到奖池表
                        BCW.SFC.Model.SfJackpot mo = new BCW.SFC.Model.SfJackpot();
                        mo.usID = 0;//1表示系统有投入
                        mo.WinPrize = 0;
                        mo.Prize = 0;
                        mo.other = "上期滚存" + new BCW.SFC.DAL.SfList().getnextprize(CID2);
                        mo.allmoney = new BCW.SFC.DAL.SfList().getnextprize(CID2);
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
                        mo.Prize = Convert.ToInt64(ub.GetSub("SFMin", "/Controls/SFC.xml")) + new BCW.SFC.DAL.SfList().getnextprize(CID2);
                        mo.other = "系统投入" + Convert.ToInt64(ub.GetSub("SFMin", "/Controls/SFC.xml")) + "上期滚存" + new BCW.SFC.DAL.SfList().getnextprize(CID2);
                        mo.allmoney = new BCW.SFC.DAL.SfList().getnextprize(CID2) + Convert.ToInt64(ub.GetSub("SFMin", "/Controls/SFC.xml"));
                        mo.AddTime = DateTime.Now;
                        mo.CID = CID;
                        new BCW.SFC.BLL.SfJackpot().Add(mo);
                    }
                }
            }
            else//没有上一期，作为第一期
            {
                model.CID = CID;
                model.EndTime = Convert.ToDateTime(EndTime);
                model.Match = Match;
                model.Team_Home = Home;
                model.Team_Away = Away;
                model.Result = "";
                model.Sale_StartTime = Convert.ToDateTime(StartTime);
                model.Start_time = Stime;
                model.Score = "";
                model.other = "";
                model.State = 0;
                model.PayCent = 0;
                model.PayCount = 0;
                model.nowprize = 0;
                model.nextprize = 0;
                model.sysprize = Convert.ToInt64(ub.GetSub("SFMin", "/Controls/SFC.xml"));//系统初始值
                model.sysprizestatue = 3;//0表示没有操作，1表示系统有投入，2表示系统已回收投入,3代表首期，不回收
                model.sysdayprize = 0;
                new BCW.SFC.BLL.SfList().Add(model);

                //把第一条记录加到奖池表
                BCW.SFC.Model.SfJackpot mo = new BCW.SFC.Model.SfJackpot();
                mo.usID = 0;//1表示系统有投入
                mo.WinPrize = 0;
                mo.Prize = Convert.ToInt64(ub.GetSub("SFMin", "/Controls/SFC.xml"));//系统初始值
                mo.other = "" + Convert.ToInt64(ub.GetSub("SFMin", "/Controls/SFC.xml"));
                mo.allmoney = Convert.ToInt64(ub.GetSub("SFMin", "/Controls/SFC.xml"));
                mo.AddTime = DateTime.Now;
                mo.CID = model.CID;
                new BCW.SFC.BLL.SfJackpot().Add(mo);
            }

            Utils.Success("添加第" + CID + "期", "添加第" + CID + "期成功..", Utils.getUrl("SFC.aspx"), "2");
        }
    }
    // 手动开奖
    private void OpenPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
        string Result = Utils.GetRequest("Result", "all", 1, @"^[0,1,3,*][,][0,1,3,*][,][0,1,3,*][,][0,1,3,*][,][0,1,3,*][,][0,1,3,*][,][0,1,3,*][,][0,1,3,*][,][0,1,3,*][,][0,1,3,*][,][0,1,3,*][,][0,1,3,*][,][0,1,3,*][,][0,1,3,*]\d*$", "");
        string info = Utils.GetRequest("info", "all", 1, "", "");
        BCW.SFC.Model.SfList model = new BCW.SFC.BLL.SfList().GetSfList(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "" + ub.GetSub("SFName", xmlPath) + "开奖";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        string sfc = ub.GetSub("SFName", xmlPath);
        builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx") + "\">" + sfc + "</a>&gt;开奖");
        builder.Append(Out.Tab("</div>", "<br />"));

        if (info == "opens")
        {
            if (Result != "")
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("第" + model.CID + "期" + ub.GetSub("SFName", xmlPath) + "开奖");
                builder.Append(Out.Tab("</div>", "<br/>"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("手动开奖为：" + Result + "");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx?act=opensave&amp;id=" + id + "&amp;Result=" + Result + "") + "\">确定开奖</a>");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx?act=open&amp;id=" + id + "") + "\">返回修改</a>");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            else
            {
                Utils.Error("请输入正确的开奖结果..", "");
            }
        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("第" + model.CID + "期" + ub.GetSub("SFName", xmlPath) + "开奖<br/>");
            builder.Append("开奖小提示：开奖数字如1,0,3,1,1,0,1,1,1,1,1,1,1,1共14个号码，每个号码在3 0 1中选取，用英文逗号隔开(特殊情况可开*)");
            builder.Append(Out.Tab("</div>", ""));

            if (model.State == 1)
            {
                int ManageId = new BCW.User.Manage().IsManageLogin();
                if (ManageId != 1)
                {
                    Utils.Error("本期已开奖", "");
                }
                builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                builder.Append("本期已开奖，要重新开奖吗?");
                builder.Append(Out.Tab("</div>", ""));
            }
            string strText = "开出数字:/,,,";
            string strName = "Result,id,act,backurl";
            string strType = "textarea,hidden,hidden,hidden";
            string strValu = "" + Result + "'" + id + "'open'opens" + Utils.getPage(0) + "";
            string strEmpt = "false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定开奖,SFC.aspx?act=open&amp;info=opens,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx?act=teshu&amp;id=" + model.CID + "") + "\">返退开奖</a><br />");
        builder.Append("温馨提示：<b style=\"color:red\">返退开奖为手动开奖，主要是应对某一期不开奖，百度彩票也不开奖，那么返退开奖将是返还会员下注币，结束当期</b>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("SFC.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    // 手动开奖确认
    private void OpenSavePage()
    {
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
        string Result = Utils.GetRequest("Result", "all", 2, @"^[0,1,3,*][,][0,1,3,*][,][0,1,3,*][,][0,1,3,*][,][0,1,3,*][,][0,1,3,*][,][0,1,3,*][,][0,1,3,*][,][0,1,3,*][,][0,1,3,*][,][0,1,3,*][,][0,1,3,*][,][0,1,3,*][,][0,1,3,*]\d*$", "输入开奖号码不正确！");
        BCW.SFC.Model.SfList model = new BCW.SFC.BLL.SfList().GetSfList(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        #region 更新中奖
        //更新开奖结果
        if (Result.Length == 27)
        {
            new BCW.SFC.BLL.SfList().UpdateResult(id, Result);
        }
        else
        {
            Utils.Error("请输入正确的比赛结果", "");
        }

        #region 奖池更新 （包括投入与回收）
        if (new BCW.SFC.BLL.SfList().getState(model.CID) == 0)
        {
            //判断奖池，小于200万就投入，大于不投入
            if (AllPrize(model.CID) < Convert.ToInt64(ub.GetSub("SFMin", "/Controls/SFC.xml")))
            {
                //long all = AllPrize(model.CID) + Convert.ToInt64(ub.GetSub("SFMin", "/Controls/SFC.xml"));
                ////更新奖池
                //new BCW.SFC.BLL.SfList().updateNowprize(all, model.CID);

                //new BCW.SFC.BLL.SfList().UpdateSysstaprize(model.CID, 1, Convert.ToInt64(ub.GetSub("SFMin", "/Controls/SFC.xml")));

                ////把记录加到奖池表
                //BCW.SFC.Model.SfJackpot mo = new BCW.SFC.Model.SfJackpot();
                //mo.usID = 1;//1表示系统有投入
                //mo.WinPrize = 0;
                //mo.Prize = Convert.ToInt64(ub.GetSub("SFMin", "/Controls/SFC.xml"));
                //mo.other = "系统投入" + Convert.ToInt64(ub.GetSub("SFMin", "/Controls/SFC.xml"));
                //mo.allmoney = all;
                //mo.AddTime = DateTime.Now.AddMilliseconds(-10);
                //mo.CID = model.CID;
                //new BCW.SFC.BLL.SfJackpot().Add(mo);

                //更新奖池
                new BCW.SFC.BLL.SfList().updateNowprize(AllPrize(model.CID), model.CID);
            }
            else if (AllPrize(model.CID) > Convert.ToInt64(ub.GetSub("SFMin", "/Controls/SFC.xml")) && AllPrize(model.CID) < Convert.ToInt64(ub.GetSub("SFMax", "/Controls/SFC.xml")))
            {
                //更新奖池
                new BCW.SFC.BLL.SfList().updateNowprize(AllPrize(model.CID), model.CID);
            }
            //判断大于400万，有投入回收，无投入不回收
            //if (AllPrize(model.CID) > Convert.ToInt64(ub.GetSub("SFCMax", "/Controls/SFC.xml")))
            else
            {
                //如果之前期数有投入为回收就回收，无则不做操作
                if (new BCW.SFC.BLL.SfList().Existsysprize(model.CID))
                {
                    //未开奖当前投注期号
                    DataSet dsh = new BCW.SFC.BLL.SfList().GetList("TOP 1 CID", "State=1 and sysprizestatue=1 order by CID ASC");
                    int CIDh = 0;
                    CIDh = int.Parse(dsh.Tables[0].Rows[0][0].ToString());
                    new BCW.SFC.BLL.SfList().UpdateSysstaprize(model.CID, 2, Convert.ToInt64(ub.GetSub("SFCSend", "/Controls/SFC.xml")));//当期回收
                    long all = AllPrize(model.CID) - Convert.ToInt64(ub.GetSub("SFCSend", "/Controls/SFC.xml"));
                    //更新奖池
                    new BCW.SFC.BLL.SfList().updateNowprize(all, model.CID);
                    //把记录加到奖池表
                    BCW.SFC.Model.SfJackpot mo = new BCW.SFC.Model.SfJackpot();
                    mo.usID = 7;//7表示系统有回收
                    mo.WinPrize = Convert.ToInt64(ub.GetSub("SFCSend", "/Controls/SFC.xml"));
                    mo.Prize = 0;
                    mo.other = "系统回收" + Convert.ToInt64(ub.GetSub("SFCSend", "/Controls/SFC.xml"));
                    mo.allmoney = all;
                    mo.AddTime = DateTime.Now.AddMilliseconds(-500);
                    mo.CID = model.CID;
                    new BCW.SFC.BLL.SfJackpot().Add(mo);

                    if (model.CID != CIDh)
                    {
                        new BCW.SFC.BLL.SfList().UpdateSysprizestatue(CIDh, 3);//表示被回收
                    }
                }
                else
                {
                    //更新奖池
                    new BCW.SFC.BLL.SfList().updateNowprize(AllPrize(model.CID), model.CID);
                }
            }
        }
        #endregion

        //中奖 
        #region 遍历中奖，得到中奖额，计算出奖池
        int count = 0;
        string[] resultnum = Result.Split(',');

        //遍历表SfPay，更新中奖
        DataSet dspay = new BCW.SFC.BLL.SfPay().GetList("CID,usID,vote,VoteNum,payCents,change,id", "CID=" + model.CID + " and State=0");
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
                for (int k = 0; k < votenum.Length; k++)
                {
                    if (VoteNum == 1)//单式算法
                    {
                        string aa = string.Empty;
                        string bb = string.Empty;
                        try
                        {
                            aa = votenum[k];
                        }
                        catch
                        {
                            //  Response.Write(k);
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
                        if (True(aa, bb))//需要特殊判断*号问题。*买胜平负都算赢
                        {
                            IsWin = true;
                            count++;
                        }


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
                #endregion

                #region 复式算法
                if (VoteNum > 1)//复式算法
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
                            if (result[j].Equals(Finalresult[j]))//需要特殊判断*号问题。*买胜平负都算赢
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
                        builder.Append(count + ",");
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
                    }
                    #endregion
                }
                #endregion
            }
        }

        #endregion

        long Allmoney = new BCW.SFC.BLL.SfList().nowprize(model.CID);
        //判断该期数有无中奖
        if (!new BCW.SFC.BLL.SfPay().Exists(model.CID, 0))
        {
            #region 无中奖奖池更新

            if (!new BCW.SFC.BLL.SfList().ExistsSysprize(model.CID))//如果不存在系统投注
            {
                if (!new BCW.SFC.BLL.SfJackpot().Exists3(model.CID))//开奖当前还没有数据
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
                    mo.CID = model.CID;
                    new BCW.SFC.BLL.SfJackpot().Add(mo);
                }

            }
            else//如果存在系统投注
            {

                if (Allmoney > Convert.ToInt64(ub.GetSub("SFMax", "/Controls/SFC.xml")))//最大奖池数
                {

                    if (!new BCW.SFC.BLL.SfJackpot().Exists3(model.CID))
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
                        mo.CID = model.CID;

                        new BCW.SFC.BLL.SfJackpot().Add(mo);
                    }

                }
                else//奖池与最小奖池数
                {
                    if (!new BCW.SFC.BLL.SfJackpot().Exists3(model.CID))
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
                        mo.CID = model.CID;
                        new BCW.SFC.BLL.SfJackpot().Add(mo);
                    }

                }
            }
            #endregion
        }
        else//有中奖
        {
            #region 中奖奖池更新
            if (!new BCW.SFC.BLL.SfList().ExistsSysprize(model.CID))//如果不存在系统投注
            {
                if (!new BCW.SFC.BLL.SfJackpot().Exists3(model.CID))//开奖当前还没有数据
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
                    mo.CID = model.CID;
                    new BCW.SFC.BLL.SfJackpot().Add(mo);
                }


            }
            else//如果存在系统投注
            {

                if (Allmoney > Convert.ToInt64(ub.GetSub("SFMax", "/Controls/SFC.xml")))//最大奖池数
                {

                    if (!new BCW.SFC.BLL.SfJackpot().Exists3(model.CID))
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
                        mo.CID = model.CID;

                        new BCW.SFC.BLL.SfJackpot().Add(mo);
                    }

                }
                else//奖池与最小奖池数
                {
                    if (!new BCW.SFC.BLL.SfJackpot().Exists3(model.CID))
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
                        mo.CID = model.CID;
                        new BCW.SFC.BLL.SfJackpot().Add(mo);
                    }

                }
            }
            #endregion
        }

        #region 给中奖会员派奖
        if (new BCW.SFC.DAL.SfList().getState(model.CID) == 0)
        {
            DataSet pay = new BCW.SFC.BLL.SfPay().GetList("usID,ISPrize,VoteNum,id,change", "CID=" + model.CID + " and State=0");
            if (pay != null && pay.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < pay.Tables[0].Rows.Count; i++)
                {
                    int pid = int.Parse(pay.Tables[0].Rows[i]["id"].ToString());
                    int ISPrize = int.Parse(pay.Tables[0].Rows[i]["ISPrize"].ToString());
                    int UsID = int.Parse(pay.Tables[0].Rows[i]["usID"].ToString());
                    int VoteNum = int.Parse(pay.Tables[0].Rows[i]["VoteNum"].ToString());
                    string change = pay.Tables[0].Rows[i]["change"].ToString();
                    int overridebyid = new BCW.SFC.BLL.SfPay().VoteNum(pid, model.CID);
                    //得到当前奖池
                    long All = new BCW.SFC.BLL.SfList().nowprize(model.CID);
                    long Now = NextPrize(model.CID);

                    #region 单式中奖
                    //一等奖
                    if (new BCW.SFC.BLL.SfPay().Exists1(pid, 1))
                    {
                        //注数
                        int zhu = new BCW.SFC.BLL.SfPay().countPrize(model.CID, 1) + getzhu(1, model.CID);
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
                        mo.CID = model.CID;
                        new BCW.SFC.BLL.SfJackpot().Add(mo);
                        BCW.Data.SqlHelper.ExecuteSql("update tb_SFPay set WinCent=" + all + " Where id=" + pid + "");
                        //动态
                        string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + new BCW.BLL.User().GetUsName(UsID) + "[/url]在第" + model.CID + "期[url=/bbs/game/SFC.aspx]" + GameName + "[/url]中一等奖" + all + "" + ub.Get("SiteBz") + "";
                        new BCW.BLL.Action().Add(1016, id, UsID, "", wText);
                        //发送内线
                        new BCW.BLL.Guest().Add(1, UsID, "", "恭喜您在第" + model.CID + "期" + "[URL=/bbs/game/SFC.aspx]" + GameName + "[/URL]" + "投注，中一等奖奖" + all + "" + ub.Get("SiteBz") + "，开奖为" + Result + "[URL=/bbs/game/SFC.aspx?act=case]马上兑奖[/URL]");
                    }
                    //二等奖
                    if (new BCW.SFC.BLL.SfPay().Exists1(pid, 2))
                    {
                        //注数
                        int zhu = new BCW.SFC.BLL.SfPay().countPrize(model.CID, 2) + getzhu(2, model.CID);
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
                        mo.CID = model.CID;
                        new BCW.SFC.BLL.SfJackpot().Add(mo);
                        BCW.Data.SqlHelper.ExecuteSql("update tb_SFPay set WinCent=" + all + " Where id=" + pid + "");
                        //动态
                        string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + new BCW.BLL.User().GetUsName(UsID) + "[/url]在第" + model.CID + "期[url=/bbs/game/SFC.aspx]" + GameName + "[/url]中二等奖" + all + "" + ub.Get("SiteBz") + "";
                        new BCW.BLL.Action().Add(1016, id, UsID, "", wText);
                        //发送内线
                        new BCW.BLL.Guest().Add(1, UsID, "", "恭喜您在第" + model.CID + "期" + "[URL=/bbs/game/SFC.aspx]" + GameName + "[/URL]" + "投注，中二等奖奖" + all + "" + ub.Get("SiteBz") + "，开奖为" + Result + "[URL=/bbs/game/SFC.aspx?act=case]马上兑奖[/URL]");
                    }
                    //三等奖
                    if (new BCW.SFC.BLL.SfPay().Exists1(pid, 3))
                    {
                        //注数
                        int zhu = new BCW.SFC.BLL.SfPay().countPrize(model.CID, 3) + getzhu(3, model.CID);
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
                        mo.CID = model.CID;
                        new BCW.SFC.BLL.SfJackpot().Add(mo);
                        BCW.Data.SqlHelper.ExecuteSql("update tb_SFPay set WinCent=" + all + " Where id=" + pid + "");
                        //动态
                        string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + new BCW.BLL.User().GetUsName(UsID) + "[/url]在第" + model.CID + "期[url=/bbs/game/SFC.aspx]" + GameName + "[/url]中三等奖" + all + "" + ub.Get("SiteBz") + "";
                        new BCW.BLL.Action().Add(1016, id, UsID, "", wText);
                        //发送内线
                        new BCW.BLL.Guest().Add(1, UsID, "", "恭喜您在第" + model.CID + "期" + "[URL=/bbs/game/SFC.aspx]" + GameName + "[/URL]" + "投注，中三等奖奖" + all + "" + ub.Get("SiteBz") + "，开奖为" + Result + "[URL=/bbs/game/SFC.aspx?act=case]马上兑奖[/URL]");
                    }
                    //四等奖
                    if (new BCW.SFC.BLL.SfPay().Exists1(pid, 4))
                    {
                        //注数
                        int zhu = new BCW.SFC.BLL.SfPay().countPrize(model.CID, 4) + getzhu(4, model.CID);
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
                        mo.CID = model.CID;
                        new BCW.SFC.BLL.SfJackpot().Add(mo);
                        BCW.Data.SqlHelper.ExecuteSql("update tb_SFPay set WinCent=" + all + " Where id=" + pid + "");
                        //动态
                        string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + new BCW.BLL.User().GetUsName(UsID) + "[/url]在第" + model.CID + "期[url=/bbs/game/SFC.aspx]" + GameName + "[/url]中四等奖" + all + "" + ub.Get("SiteBz") + "";
                        new BCW.BLL.Action().Add(1016, id, UsID, "", wText);
                        //发送内线
                        new BCW.BLL.Guest().Add(1, UsID, "", "恭喜您在第" + model.CID + "期" + "[URL=/bbs/game/SFC.aspx]" + GameName + "[/URL]" + "投注，中四等奖奖" + all + "" + ub.Get("SiteBz") + "，开奖为" + Result + "[URL=/bbs/game/SFC.aspx?act=case]马上兑奖[/URL]");
                    }
                    //五等奖
                    if (new BCW.SFC.BLL.SfPay().Exists1(pid, 5))
                    {
                        //注数
                        int zhu = new BCW.SFC.BLL.SfPay().countPrize(model.CID, 5) + getzhu(5, model.CID);
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
                        mo.CID = model.CID;
                        new BCW.SFC.BLL.SfJackpot().Add(mo);
                        BCW.Data.SqlHelper.ExecuteSql("update tb_SFPay set WinCent=" + all + " Where id=" + pid + "");
                        //动态
                        string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + new BCW.BLL.User().GetUsName(UsID) + "[/url]在第" + model.CID + "期[url=/bbs/game/SFC.aspx]" + GameName + "[/url]中五等奖" + all + "" + ub.Get("SiteBz") + "";
                        new BCW.BLL.Action().Add(1016, id, UsID, "", wText);
                        //发送内线
                        new BCW.BLL.Guest().Add(1, UsID, "", "恭喜您在第" + model.CID + "期" + "[URL=/bbs/game/SFC.aspx]" + GameName + "[/URL]" + "投注，中五等奖奖" + all + "" + ub.Get("SiteBz") + "，开奖为" + Result + "[URL=/bbs/game/SFC.aspx?act=case]马上兑奖[/URL]");
                    }
                    //六等奖
                    if (new BCW.SFC.BLL.SfPay().Exists1(pid, 6))
                    {
                        //注数
                        int zhu = new BCW.SFC.BLL.SfPay().countPrize(model.CID, 6) + getzhu(6, model.CID);
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
                        mo.CID = model.CID;
                        new BCW.SFC.BLL.SfJackpot().Add(mo);
                        BCW.Data.SqlHelper.ExecuteSql("update tb_SFPay set WinCent=" + all + " Where id=" + pid + "");
                        //动态
                        string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + new BCW.BLL.User().GetUsName(UsID) + "[/url]在第" + model.CID + "期[url=/bbs/game/SFC.aspx]" + GameName + "[/url]中六等奖" + all + "" + ub.Get("SiteBz") + "";
                        new BCW.BLL.Action().Add(1016, id, UsID, "", wText);
                        //发送内线
                        new BCW.BLL.Guest().Add(1, UsID, "", "恭喜您在第" + model.CID + "期" + "[URL=/bbs/game/SFC.aspx]" + GameName + "[/URL]" + "投注，中六等奖奖" + all + "" + ub.Get("SiteBz") + "，开奖为" + Result + "[URL=/bbs/game/SFC.aspx?act=case]马上兑奖[/URL]");
                    }
                    //七等奖
                    if (new BCW.SFC.BLL.SfPay().Exists1(pid, 7))
                    {
                        //注数
                        int zhu = new BCW.SFC.BLL.SfPay().countPrize(model.CID, 7) + getzhu(7, model.CID);
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
                        mo.CID = model.CID;
                        new BCW.SFC.BLL.SfJackpot().Add(mo);
                        BCW.Data.SqlHelper.ExecuteSql("update tb_SFPay set WinCent=" + all + " Where id=" + pid + "");
                        //动态
                        string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + new BCW.BLL.User().GetUsName(UsID) + "[/url]在第" + model.CID + "期[url=/bbs/game/SFC.aspx]" + GameName + "[/url]中七等奖" + all + "" + ub.Get("SiteBz") + "";
                        new BCW.BLL.Action().Add(1016, id, UsID, "", wText);
                        //发送内线
                        new BCW.BLL.Guest().Add(1, UsID, "", "恭喜您在第" + model.CID + "期" + "[URL=/bbs/game/SFC.aspx]" + GameName + "[/URL]" + "投注，中七等奖奖" + all + "" + ub.Get("SiteBz") + "，开奖为" + Result + "[URL=/bbs/game/SFC.aspx?act=case]马上兑奖[/URL]");
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
                                int zhu = new BCW.SFC.BLL.SfPay().countPrize(model.CID, 1) + getzhu(1, model.CID);
                                //费率
                                double lv = Convert.ToDouble(ub.GetSub("SFOne", "/Controls/SFC.xml")) * 0.01;
                                double allr = Math.Round(Convert.ToDouble(All * lv / Convert.ToDouble(zhu)), 2);
                                long all = Convert.ToInt64(allr * getzhu(1, model.CID));
                                //添加奖池数据
                                BCW.SFC.Model.SfJackpot mo = new BCW.SFC.Model.SfJackpot();
                                mo.usID = UsID;
                                mo.WinPrize = all;
                                mo.Prize = 0;
                                mo.other = "中一等奖" + Convert.ToString(all);
                                mo.allmoney = new BCW.SFC.BLL.SfJackpot().Getallmoney(model.CID) - all;
                                mo.AddTime = DateTime.Now;
                                mo.CID = model.CID;
                                new BCW.SFC.BLL.SfJackpot().Add(mo);
                                BCW.Data.SqlHelper.ExecuteSql("update tb_SFPay set WinCent=(( select WinCent from tb_SfPay where id=" + pid + ")+" + all + ") Where id=" + pid + "");
                                //动态
                                string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + new BCW.BLL.User().GetUsName(UsID) + "[/url]在第" + model.CID + "期[url=/bbs/game/SFC.aspx]" + GameName + "[/url]中一等奖" + all + "" + ub.Get("SiteBz") + "";
                                new BCW.BLL.Action().Add(1016, id, UsID, "", wText);
                                //发送内线
                                new BCW.BLL.Guest().Add(1, UsID, "", "恭喜您在第" + model.CID + "期" + "[URL=/bbs/game/SFC.aspx]" + GameName + "[/URL]" + "投注，中一等奖奖" + all + "" + ub.Get("SiteBz") + "，开奖为" + Result + "[URL=/bbs/game/SFC.aspx?act=case]马上兑奖[/URL]");

                            }
                            if (ch[w].Contains("二"))
                            {
                                //注数
                                int zhu = new BCW.SFC.BLL.SfPay().countPrize(model.CID, 2) + getzhu(2, model.CID);
                                //费率
                                double lv = Convert.ToDouble(ub.GetSub("SFTwo", "/Controls/SFC.xml")) * 0.01;
                                double allr = Math.Round(Convert.ToDouble(All * lv / Convert.ToDouble(zhu)), 2);
                                long all = Convert.ToInt64(allr * getzhu(2, model.CID));
                                //添加奖池数据
                                BCW.SFC.Model.SfJackpot mo = new BCW.SFC.Model.SfJackpot();
                                mo.usID = UsID;
                                mo.WinPrize = all;
                                mo.Prize = 0;
                                mo.other = "中二等奖" + Convert.ToString(all);
                                mo.allmoney = new BCW.SFC.BLL.SfJackpot().Getallmoney(model.CID) - all;
                                mo.AddTime = DateTime.Now;
                                mo.CID = model.CID;
                                new BCW.SFC.BLL.SfJackpot().Add(mo);
                                BCW.Data.SqlHelper.ExecuteSql("update tb_SFPay set WinCent=(( select WinCent from tb_SfPay where id=" + pid + ")+" + all + ") Where id=" + pid + "");
                                //动态
                                string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + new BCW.BLL.User().GetUsName(UsID) + "[/url]在第" + model.CID + "期[url=/bbs/game/SFC.aspx]" + GameName + "[/url]中二等奖" + all + "" + ub.Get("SiteBz") + "";
                                new BCW.BLL.Action().Add(1016, id, UsID, "", wText);
                                //发送内线
                                new BCW.BLL.Guest().Add(1, UsID, "", "恭喜您在第" + model.CID + "期" + "[URL=/bbs/game/SFC.aspx]" + GameName + "[/URL]" + "投注，中二等奖奖" + all + "" + ub.Get("SiteBz") + "，开奖为" + Result + "[URL=/bbs/game/SFC.aspx?act=case]马上兑奖[/URL]");

                            }
                            if (ch[w].Contains("三"))
                            {
                                //注数
                                int zhu = new BCW.SFC.BLL.SfPay().countPrize(model.CID, 3) + getzhu(3, model.CID);
                                //费率
                                double lv = Convert.ToDouble(ub.GetSub("SFThree", "/Controls/SFC.xml")) * 0.01;
                                double allr = Math.Round(Convert.ToDouble(All * lv / Convert.ToDouble(zhu)), 2);
                                long all = Convert.ToInt64(allr * getzhu(3, model.CID));
                                //添加奖池数据
                                BCW.SFC.Model.SfJackpot mo = new BCW.SFC.Model.SfJackpot();
                                mo.usID = UsID;
                                mo.WinPrize = all;
                                mo.Prize = 0;
                                mo.other = "中三等奖" + Convert.ToString(all);
                                mo.allmoney = new BCW.SFC.BLL.SfJackpot().Getallmoney(model.CID) - all;
                                mo.AddTime = DateTime.Now;
                                mo.CID = model.CID;
                                new BCW.SFC.BLL.SfJackpot().Add(mo);
                                BCW.Data.SqlHelper.ExecuteSql("update tb_SFPay set WinCent=(( select WinCent from tb_SfPay where id=" + pid + ")+" + all + ") Where id=" + pid + "");
                                //动态
                                string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + new BCW.BLL.User().GetUsName(UsID) + "[/url]在第" + model.CID + "期[url=/bbs/game/SFC.aspx]" + GameName + "[/url]中三等奖" + all + "" + ub.Get("SiteBz") + "";
                                new BCW.BLL.Action().Add(1016, id, UsID, "", wText);
                                //发送内线
                                new BCW.BLL.Guest().Add(1, UsID, "", "恭喜您在第" + model.CID + "期" + "[URL=/bbs/game/SFC.aspx]" + GameName + "[/URL]" + "投注，中三等奖奖" + all + "" + ub.Get("SiteBz") + "，开奖为" + Result + "[URL=/bbs/game/SFC.aspx?act=case]马上兑奖[/URL]");

                            }
                            if (ch[w].Contains("四"))
                            {
                                //注数
                                int zhu = new BCW.SFC.BLL.SfPay().countPrize(model.CID, 4) + getzhu(4, model.CID);
                                //费率
                                double lv = Convert.ToDouble(ub.GetSub("SFForc", "/Controls/SFC.xml")) * 0.01;
                                double allr = Math.Round(Convert.ToDouble(All * lv / Convert.ToDouble(zhu)), 2);
                                long all = Convert.ToInt64(allr * getzhu(4, model.CID));
                                //添加奖池数据
                                BCW.SFC.Model.SfJackpot mo = new BCW.SFC.Model.SfJackpot();
                                mo.usID = UsID;
                                mo.WinPrize = all;
                                mo.Prize = 0;
                                mo.other = "中四等奖" + Convert.ToString(all);
                                mo.allmoney = new BCW.SFC.BLL.SfJackpot().Getallmoney(model.CID) - all;
                                mo.AddTime = DateTime.Now;
                                mo.CID = model.CID;
                                new BCW.SFC.BLL.SfJackpot().Add(mo);
                                BCW.Data.SqlHelper.ExecuteSql("update tb_SFPay set WinCent=(( select WinCent from tb_SfPay where id=" + pid + ")+" + all + ") Where id=" + pid + "");
                                //动态
                                string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + new BCW.BLL.User().GetUsName(UsID) + "[/url]在第" + model.CID + "期[url=/bbs/game/SFC.aspx]" + GameName + "[/url]中四等奖" + all + "" + ub.Get("SiteBz") + "";
                                new BCW.BLL.Action().Add(1016, id, UsID, "", wText);
                                //发送内线
                                new BCW.BLL.Guest().Add(1, UsID, "", "恭喜您在第" + model.CID + "期" + "[URL=/bbs/game/SFC.aspx]" + GameName + "[/URL]" + "投注，中四等奖奖" + all + "" + ub.Get("SiteBz") + "，开奖为" + Result + "[URL=/bbs/game/SFC.aspx?act=case]马上兑奖[/URL]");

                            }
                            if (ch[w].Contains("五"))
                            {
                                //注数
                                int zhu = new BCW.SFC.BLL.SfPay().countPrize(model.CID, 5) + getzhu(5, model.CID);
                                //费率
                                double lv = Convert.ToDouble(ub.GetSub("SFFive", "/Controls/SFC.xml")) * 0.01;
                                double allr = Math.Round(Convert.ToDouble(All * lv / Convert.ToDouble(zhu)), 2);
                                long all = Convert.ToInt64(allr * getzhu(5, model.CID));
                                //添加奖池数据
                                BCW.SFC.Model.SfJackpot mo = new BCW.SFC.Model.SfJackpot();
                                mo.usID = UsID;
                                mo.WinPrize = all;
                                mo.Prize = 0;
                                mo.other = "中五等奖" + Convert.ToString(all);
                                mo.allmoney = new BCW.SFC.BLL.SfJackpot().Getallmoney(model.CID) - all;
                                mo.AddTime = DateTime.Now;
                                mo.CID = model.CID;
                                new BCW.SFC.BLL.SfJackpot().Add(mo);
                                BCW.Data.SqlHelper.ExecuteSql("update tb_SFPay set WinCent=(( select WinCent from tb_SfPay where id=" + pid + ")+" + all + ") Where id=" + pid + "");
                                //动态
                                string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + new BCW.BLL.User().GetUsName(UsID) + "[/url]在第" + model.CID + "期[url=/bbs/game/SFC.aspx]" + GameName + "[/url]中五等奖" + all + "" + ub.Get("SiteBz") + "";
                                new BCW.BLL.Action().Add(1016, id, UsID, "", wText);
                                //发送内线
                                new BCW.BLL.Guest().Add(1, UsID, "", "恭喜您在第" + model.CID + "期" + "[URL=/bbs/game/SFC.aspx]" + GameName + "[/URL]" + "投注，中五等奖奖" + all + "" + ub.Get("SiteBz") + "，开奖为" + Result + "[URL=/bbs/game/SFC.aspx?act=case]马上兑奖[/URL]");

                            }
                            if (ch[w].Contains("六"))
                            {
                                //注数
                                int zhu = new BCW.SFC.BLL.SfPay().countPrize(model.CID, 6) + getzhu(6, model.CID);
                                //费率
                                double lv = Convert.ToDouble(ub.GetSub("SFSix", "/Controls/SFC.xml")) * 0.01;
                                double allr = Math.Round(Convert.ToDouble(All * lv / Convert.ToDouble(zhu)), 2);
                                long all = Convert.ToInt64(allr * getzhu(6, model.CID));
                                //添加奖池数据
                                BCW.SFC.Model.SfJackpot mo = new BCW.SFC.Model.SfJackpot();
                                mo.usID = UsID;
                                mo.WinPrize = all;
                                mo.Prize = 0;
                                mo.other = "中六等奖" + Convert.ToString(all);
                                mo.allmoney = new BCW.SFC.BLL.SfJackpot().Getallmoney(model.CID) - all;
                                mo.AddTime = DateTime.Now;
                                mo.CID = model.CID;
                                new BCW.SFC.BLL.SfJackpot().Add(mo);
                                BCW.Data.SqlHelper.ExecuteSql("update tb_SFPay set WinCent=(( select WinCent from tb_SfPay where id=" + pid + ")+" + all + ") Where id=" + pid + "");
                                //动态
                                string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + new BCW.BLL.User().GetUsName(UsID) + "[/url]在第" + model.CID + "期[url=/bbs/game/SFC.aspx]" + GameName + "[/url]中六等奖" + all + "" + ub.Get("SiteBz") + "";
                                new BCW.BLL.Action().Add(1016, id, UsID, "", wText);
                                //发送内线
                                new BCW.BLL.Guest().Add(1, UsID, "", "恭喜您在第" + model.CID + "期" + "[URL=/bbs/game/SFC.aspx]" + GameName + "[/URL]" + "投注，中六等奖奖" + all + "" + ub.Get("SiteBz") + "，开奖为" + Result + "[URL=/bbs/game/SFC.aspx?act=case]马上兑奖[/URL]");

                            }
                            if (ch[w].Contains("七"))
                            {
                                //注数
                                int zhu = new BCW.SFC.BLL.SfPay().countPrize(model.CID, 7) + getzhu(7, model.CID);
                                //费率
                                double lv = Convert.ToDouble(ub.GetSub("SFSeven", "/Controls/SFC.xml")) * 0.01;
                                double allr = Math.Round(Convert.ToDouble(All * lv / Convert.ToDouble(zhu)), 2);
                                long all = Convert.ToInt64(allr * getzhu(7, model.CID));
                                //添加奖池数据
                                BCW.SFC.Model.SfJackpot mo = new BCW.SFC.Model.SfJackpot();
                                mo.usID = UsID;
                                mo.WinPrize = all;
                                mo.Prize = 0;
                                mo.other = "中七等奖" + Convert.ToString(all);
                                mo.allmoney = new BCW.SFC.BLL.SfJackpot().Getallmoney(model.CID) - all;
                                mo.AddTime = DateTime.Now;
                                mo.CID = model.CID;
                                new BCW.SFC.BLL.SfJackpot().Add(mo);
                                BCW.Data.SqlHelper.ExecuteSql("update tb_SFPay set WinCent=(( select WinCent from tb_SfPay where id=" + pid + ")+" + all + ") Where id=" + pid + "");
                                //动态
                                string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + new BCW.BLL.User().GetUsName(UsID) + "[/url]在第" + model.CID + "期[url=/bbs/game/SFC.aspx]" + GameName + "[/url]中七等奖" + all + "" + ub.Get("SiteBz") + "";
                                new BCW.BLL.Action().Add(1016, id, UsID, "", wText);
                                //发送内线
                                new BCW.BLL.Guest().Add(1, UsID, "", "恭喜您在第" + model.CID + "期" + "[URL=/bbs/game/SFC.aspx]" + GameName + "[/URL]" + "投注，中七等奖奖" + all + "" + ub.Get("SiteBz") + "，开奖为" + Result + "[URL=/bbs/game/SFC.aspx?act=case]马上兑奖[/URL]");

                            }
                        }
                    }
                    #endregion
                }
            }
        }
        #endregion

        if (new BCW.SFC.DAL.SfList().getState(model.CID) == 0)
        {
            //更新当期系统结余奖池
            new BCW.SFC.DAL.SfList().UpdateNextprize(model.CID, NextPrize(model.CID));
            //更新当期系统收取手续
            new BCW.SFC.DAL.SfList().Updatesysdayprize(model.CID, Convert.ToInt64(Allmoney * Convert.ToInt64(ub.GetSub("SFsys", "/Controls/SFC.xml")) * 0.01));
        }

        #region 奖池滚存
        int newcid = 0;
        if (new BCW.SFC.BLL.SfList().ExistsCID(model.CID + 1))
        {
            newcid = (model.CID + 1);
        }
        else
        {
            newcid = FirstNewCID();
        }
        if (!new BCW.SFC.DAL.SfJackpot().Existsgun(model.CID))
        {
            //把奖池滚存记录下来
            BCW.SFC.Model.SfJackpot mos = new BCW.SFC.Model.SfJackpot();
            mos.usID = 5;
            mos.WinPrize = 0;
            mos.Prize = 0;
            mos.other = "第" + model.CID + "期滚存" + NextPrize(model.CID) + ub.Get("SiteBz") + "到" + (newcid) + "期|结余0" + ub.Get("SiteBz");
            mos.allmoney = 0;
            mos.AddTime = DateTime.Now;
            mos.CID = model.CID;
            new BCW.SFC.BLL.SfJackpot().Add(mos);
        }
        if (!new BCW.SFC.DAL.SfJackpot().Existsgun1(newcid))
        {
            //把奖池滚存记录下来
            BCW.SFC.Model.SfJackpot mos = new BCW.SFC.Model.SfJackpot();
            mos.usID = 6;
            mos.WinPrize = 0;
            mos.Prize = 0;
            mos.other = "得到第" + model.CID + "期滚存" + NextPrize(model.CID) + ub.Get("SiteBz") + "|结余" + NextPrize(model.CID) + "" + ub.Get("SiteBz");
            mos.allmoney = 0;
            mos.AddTime = Convert.ToDateTime("2000-10-10 10:10:10");
            mos.CID = (newcid);
            new BCW.SFC.BLL.SfJackpot().Add(mos);
        }
        #endregion

        #region 遍历奖池表 （SFJackpot）,更新预售期的奖池
        //遍历表SFJackpot，更新预售期的奖池
        if (new BCW.SFC.DAL.SfList().ExistsCID((newcid)))
        {
            DataSet nextPP = new BCW.SFC.BLL.SfJackpot().GetList("id,usID,Prize,WinPrize,other,allmoney,AddTime,CID", "CID=" + (newcid) + " ");

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
                    int CID = int.Parse(nextPP.Tables[0].Rows[i]["CID"].ToString());

                    long PP = 0;
                    if (new BCW.SFC.BLL.SfList().getsysstate((model.CID + 1)) == 1)
                    {
                        PP = (allmoney + NextPrize(model.CID) + Convert.ToInt64(ub.GetSub("SFCSend", "/Controls/SFC.xml")));
                    }
                    else
                    {
                        PP = (allmoney + NextPrize(model.CID));
                    }
                    //更新奖池
                    //new BCW.SFC.DAL.SfJackpot().updateallmoney(PP, (model.CID + 1), UsID,id);
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

        #region 更新奖池结余与系统收取手续
        if (new BCW.SFC.DAL.SfList().getState(model.CID) == 0)
        {
            if (new BCW.SFC.BLL.SfList().ExistsCID((model.CID + 1)))
            {
                long paycent = new BCW.SFC.BLL.SfList().GetPrice("sum(PayCent)", " CID =" + (model.CID + 1) + " ");//消费
                if ((NextPrize(model.CID) + paycent) < Convert.ToInt64(ub.GetSub("SFMin", "/Controls/SFC.xml")))
                {
                    new BCW.SFC.BLL.SfList().UpdateSysstaprize((model.CID + 1), 1, Convert.ToInt64(ub.GetSub("SFCSend", "/Controls/SFC.xml")));

                    //把记录加到奖池表
                    BCW.SFC.Model.SfJackpot mo = new BCW.SFC.Model.SfJackpot();
                    mo.usID = 1;//1表示系统有投入
                    mo.WinPrize = 0;
                    mo.Prize = Convert.ToInt64(ub.GetSub("SFCSend", "/Controls/SFC.xml"));
                    mo.other = "系统投入" + Convert.ToInt64(ub.GetSub("SFCSend", "/Controls/SFC.xml"));
                    mo.allmoney = (Convert.ToInt64(ub.GetSub("SFCSend", "/Controls/SFC.xml")) + NextPrize(model.CID));
                    mo.AddTime = DateTime.Now;
                    mo.CID = (model.CID + 1);
                    new BCW.SFC.BLL.SfJackpot().Add(mo);
                }
            }
        }
        #endregion

        //完成返彩后正式更新该期为结束
        if (new BCW.SFC.DAL.SfList().getState(model.CID) == 0)
        {
            BCW.Data.SqlHelper.ExecuteSql("update tb_SfList set State=1 Where CID=" + model.CID + "");
            BCW.Data.SqlHelper.ExecuteSql("update tb_SFPay set State=1 Where CID=" + model.CID + "");
        }
        Utils.Success("第" + model.CID + "期手动开奖", "第" + model.CID + "期手动开奖成功..", Utils.getUrl("SFC.aspx"), "3");
        #endregion
    }
    //特殊开奖
    private void TeShuPage()
    {

        int ManageId = new BCW.User.Manage().IsManageLogin();
        string Title = GameName;
        Master.Title = (Title + "管理中心");
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "期号错误"));
        ub xml = new ub();
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;<a href=\"" + Utils.getUrl("SFC.aspx") + "\">" + Title + "管理</a>&gt;返退开奖");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("第" + id + "期" + ub.GetSub("SFName", xmlPath) + "开奖");
        builder.Append(Out.Tab("</div>", "<br/>"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx?info=1&amp;act=teshu&amp;id=" + id + "") + "\">[确定返退开奖]</a>");
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "1")
        {
            builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx?act=teshu&amp;id=" + id + "&amp;info=" + 2 + "") + "\">&nbsp;-->确认开奖</a>");

        }
        if (info == "2")
        {
            if (!new BCW.SFC.BLL.SfList().ExistsCID(id))
            {
                Utils.Error("不存在的记录", "");
            }
            else
            {
                //根据id查询-购买表
                DataSet ds = new BCW.SFC.BLL.SfPay().GetList(" UsID,PayCents,State", " State=0 and CID=" + id + " ");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        int meid = Convert.ToInt32(ds.Tables[0].Rows[i][0]);//用户名
                        long Price = Convert.ToInt32(ds.Tables[0].Rows[i][1]); ;//获得id对应的用户名
                        int state_get = Convert.ToInt32(ds.Tables[0].Rows[i][2]);//用户购买情况
                        //如果特殊开奖，退回本金
                        if (state_get == 0)
                        {
                            new BCW.BLL.User().UpdateiGold(meid, Price, "" + GameName + "第" + id + "期取消开奖，系统返还当期下注" + Price + ub.Get("SiteBz") + "！");//退还酷币
                            new BCW.BLL.Guest().Add(1, meid, new BCW.BLL.User().GetUsName(meid), "由于" + GameName + "第" + id + "期取消开奖，系统返还您当期下注的" + Price + ub.Get("SiteBz") + "！");
                        }
                    }

                    //  BCW.Data.SqlHelper.ExecuteSql("update tb_SfPay set State=1 Where CID=" + id + "");//更新当期为开奖状态

                    if (new BCW.SFC.BLL.SfList().getState(id) == 0)
                    {
                        //获取当前期数投注总额
                        long AllPrice = new BCW.SFC.BLL.SfPay().AllPrice(id);
                        new BCW.SFC.BLL.SfList().updateNowprize((AllPrize(id) - AllPrice), id);//更新当期奖池

                        long Allmoney = new BCW.SFC.BLL.SfList().nowprize(id);
                        //更新当期系统收取手续
                        new BCW.SFC.DAL.SfList().Updatesysdayprize(id, Convert.ToInt64(Allmoney * Convert.ToInt64(ub.GetSub("SFsys", "/Controls/SFC.xml")) * 0.01));//更新当期收费

                        new BCW.SFC.DAL.SfList().UpdateNextprize(id, (Allmoney - Convert.ToInt64(Allmoney * Convert.ToInt64(ub.GetSub("SFsys", "/Controls/SFC.xml")) * 0.01)));//更新当期结余

                        if (!new BCW.SFC.BLL.SfJackpot().Exists3(id))//开奖当前还没有数据
                        {
                            BCW.SFC.Model.SfJackpot mo = new BCW.SFC.Model.SfJackpot();
                            mo.usID = 0;
                            mo.WinPrize = Convert.ToInt64(Allmoney * Convert.ToInt64(ub.GetSub("SFsys", "/Controls/SFC.xml")) * 0.01);//系统收回%10
                            mo.Prize = 0;
                            long a = Convert.ToInt64(Allmoney * Convert.ToInt64(ub.GetSub("SFsys", "/Controls/SFC.xml")) * 0.01);
                            long b = Convert.ToInt64(Allmoney) - a;
                            mo.other = "当期无开奖，会员下注全部返还,系统收取手续" + a;
                            mo.allmoney = b;
                            mo.AddTime = DateTime.Now.AddSeconds(-1);
                            mo.CID = id;
                            new BCW.SFC.BLL.SfJackpot().Add(mo);
                        }

                        #region 奖池滚存
                        if (!new BCW.SFC.DAL.SfJackpot().Existsgun(id))
                        {
                            //把奖池滚存记录下来
                            BCW.SFC.Model.SfJackpot mos = new BCW.SFC.Model.SfJackpot();
                            mos.usID = 5;
                            mos.WinPrize = 0;
                            mos.Prize = 0;
                            mos.other = "第" + id + "期滚存" + NextPrize(id) + ub.Get("SiteBz") + "到" + (id + 1) + "期|结余0" + ub.Get("SiteBz");
                            mos.allmoney = 0;
                            mos.AddTime = DateTime.Now;
                            mos.CID = id;
                            new BCW.SFC.BLL.SfJackpot().Add(mos);
                        }
                        if (!new BCW.SFC.DAL.SfJackpot().Existsgun1(id + 1))
                        {
                            //把奖池滚存记录下来
                            BCW.SFC.Model.SfJackpot mos = new BCW.SFC.Model.SfJackpot();
                            mos.usID = 6;
                            mos.WinPrize = 0;
                            mos.Prize = 0;
                            mos.other = "得到第" + id + "期滚存" + NextPrize(id) + ub.Get("SiteBz") + "|结余" + NextPrize(id) + "" + ub.Get("SiteBz");
                            mos.allmoney = 0;
                            mos.AddTime = Convert.ToDateTime("2000-10-10 10:10:10");
                            mos.CID = (id + 1);
                            new BCW.SFC.BLL.SfJackpot().Add(mos);
                        }
                        #endregion


                        #region 遍历奖池表 （SfJackpot）,更新预售期的奖池
                        //遍历表SfJackpot，更新预售期的奖池
                        if (new BCW.SFC.DAL.SfList().ExistsCID((id + 1)))
                        {
                            DataSet nextPP = new BCW.SFC.BLL.SfJackpot().GetList("id,usID,Prize,WinPrize,other,allmoney,AddTime,CID", "CID=" + (id + 1) + " ");

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
                                    if (new BCW.SFC.BLL.SfList().getsysstate((id + 1)) == 1)
                                    {
                                        PP = (allmoney + NextPrize(id) + Convert.ToInt64(ub.GetSub("SFMin", "/Controls/SFC.xml")));
                                    }
                                    else
                                    {
                                        PP = (allmoney + NextPrize(id));
                                    }
                                    //更新奖池
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



                    }

                }
                //完成返彩后正式更新该期为结束
                new BCW.SFC.BLL.SfList().UpdateResult(new BCW.SFC.BLL.SfList().id(id), "无开奖");
                BCW.Data.SqlHelper.ExecuteSql("update tb_SfList set State=1 Where CID=" + id + "");
                BCW.Data.SqlHelper.ExecuteSql("update tb_SfPay set State=1 Where CID=" + id + "");
            }


            Utils.Success("" + GameName + "特殊开奖", "" + GameName + "第" + id + "期特殊开奖成功..", Utils.getUrl("SFC.aspx"), "3");
        }
        builder.Append(Out.Tab("</div>", "<br />"));


        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b style=\"color:red\">注意：特殊开奖，将当期所有下注返还</b><br />");
        builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx") + "\">[再看看吧..]</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    // 开奖号码对比
    public bool True(string votenum, string resultnum)
    {
        if ((votenum).Contains(resultnum))
            return true;
        if (resultnum == "*")
            return true;
        return false;
    }
    // 编辑
    private void EditPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));

        BCW.SFC.Model.SfList model = new BCW.SFC.BLL.SfList().GetSfList(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "编辑";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        string sfc = ub.GetSub("SFName", xmlPath);
        builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx") + "\">" + sfc + "</a>&gt;编辑");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("编辑第<b>" + model.CID + "</b>期");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "期数:/,赛事:(14场)/,主场:(14场)/,客场:(14场)/,比赛时间:(14场)/,开始时间:/,截止时间:/,比赛结果:/,,,,,,,";
        string strName = "CID,Match,Home,Away,Stime,StartTime,EndTime,result,State,PayCent,PayCount,nowprize,id,act,backurl";
        string strType = "num,textarea,textarea,textarea,textarea,date,date,textarea,hidden,hidden,hidden,hidden,hidden,hidden,hidden";
        string strValu = "" + model.CID + "'" + model.Match + "'" + model.Team_Home + "'" + model.Team_Away + "'" + model.Start_time + "'" + DT.FormatDate(model.Sale_StartTime, 0) + "'" + DT.FormatDate(model.EndTime, 0) + "'" + model.Result + "'" + model.State + "'" + model.PayCent + "'" + model.PayCount + "'" + model.nowprize + "'" + id + "'editsave'" + Utils.getPage(0) + "";
        string strEmpt = "false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false";
        string strIdea = "/";
        string strOthe = "确定编辑|reset,SFC.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx") + "\">返回上一级</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx?act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">删除本期</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    private void EditSavePage()
    {
        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[1-9]\d*$", "ID错误"));
        string Match = Utils.GetRequest("Match", "post", 2, @"^", "1填写错误！");
        string Home = Utils.GetRequest("Home", "post", 2, @"^", "2填写错误！");
        string Away = Utils.GetRequest("Away", "post", 2, @"^", "3填写错误！");
        string Stime = Utils.GetRequest("Stime", "post", 2, @"^", "4填写错误！");
        DateTime StartTime = Utils.ParseTime(Utils.GetRequest("StartTime", "post", 2, DT.RegexTime, "截止时间格式填写出错,正确格式如" + DT.FormatDate(DateTime.Now, 0) + ""));
        int CID = int.Parse(Utils.GetRequest("CID", "post", 2, @"^[0-9]\d*$", "期数填写错误"));
        string result = Utils.GetRequest("result", "post", 1, @"^", "");
        DateTime EndTime = Utils.ParseTime(Utils.GetRequest("EndTime", "post", 2, DT.RegexTime, "截止时间格式填写出错,正确格式如" + DT.FormatDate(DateTime.Now, 0) + ""));
        int State = int.Parse(Utils.GetRequest("State", "post", 2, @"^[0-1]\d*$", "ID错误"));
        long PayCent = Convert.ToInt64(Utils.GetRequest("PayCent", "post", 2, @"^[0-9]\d*$", "2填写错误！"));
        int PayCount = int.Parse(Utils.GetRequest("PayCount", "post", 2, @"^[0-9]\d*$", "2填写错误！"));
        long nowprize = Convert.ToInt64(Utils.GetRequest("nowprize", "post", 2, @"^[0-9]\d*$", "2填写错误！"));
        BCW.SFC.Model.SfList model = new BCW.SFC.Model.SfList();
        BCW.SFC.Model.SfList model1 = new BCW.SFC.BLL.SfList().GetSfList(id);
        model.id = id;
        model.CID = CID;
        model.EndTime = EndTime;
        model.Match = Match;
        model.Team_Home = Home;
        model.Team_Away = Away;
        model.Result = result;
        model.Sale_StartTime = StartTime;
        model.Start_time = Stime;
        model.Score = model1.Score;
        model.other = model1.other;
        model.State = model1.State;
        model.PayCent = model1.PayCent;
        model.PayCount = model1.PayCount;
        model.nowprize = model1.nowprize;
        model.nextprize = model1.nextprize;
        model.sysprize = model1.sysprize;
        model.sysprizestatue = model1.sysprizestatue;
        model.sysdayprize = model1.sysdayprize;
        new BCW.SFC.BLL.SfList().Update(model);
        Utils.Success("编辑第" + CID + "期", "编辑第" + CID + "期成功..", Utils.getUrl("SFC.aspx?act=edit&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
    }
    // 删除
    private void DelPage()
    {
        //int ManageId = new BCW.User.Manage().IsManageLogin();
        //if (ManageId != 1)
        //{
        //    Utils.Error("权限不足", "");
        //}

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        string sfc = ub.GetSub("SFName", xmlPath);
        builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx") + "\">" + sfc + "</a>&gt;期数删除");
        builder.Append(Out.Tab("</div>", "<br />"));

        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.SFC.Model.SfList model = new BCW.SFC.BLL.SfList().GetSfList(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }

        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "")
        {
            Master.Title = "删除第" + model.CID + "期";
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("确定第" + model.CID + "期记录吗.删除同时将会删除该期的下注记录.");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx?info=ok&amp;act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx?act=edit&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            new BCW.SFC.BLL.SfList().Delete(id);
            new BCW.SFC.BLL.SfPay().Delete("CID=" + model.CID + "");
            new BCW.SFC.BLL.SfJackpot().Delete("CID=" + model.CID + "");
            Utils.Success("删除第" + model.CID + "期", "删除第" + model.CID + "期成功..", Utils.getPage("SFC.aspx"), "3");
        }
    }
    //期号详情
    private void ViewPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-2]\d*$", "1"));
        BCW.SFC.Model.SfList model = new BCW.SFC.BLL.SfList().GetSfList(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "第" + model.CID + "期";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        string sfc = ub.GetSub("SFName", xmlPath);
        builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx") + "\">" + sfc + "</a>&gt;第" + model.CID + "期");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("查看下注/开奖记录");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("查看:");
        if (ptype == 1)
            builder.Append("下注|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx?act=view&amp;ptype=1&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">下注</a>|");

        if (ptype == 2)
            builder.Append("中奖");
        else
            builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx?act=view&amp;ptype=2&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">中奖</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        int zhucounts = new BCW.SFC.DAL.SfPay().AllVoteNum(model.CID);
        long Money = new BCW.SFC.DAL.SfPay().getAllPrice(model.CID);
        long wincent = new BCW.SFC.DAL.SfPay().AllWinCentbyCID(model.CID);

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        if (ptype == 1)
            strWhere += "CID=" + model.CID + "";
        else
            strWhere += "CID=" + model.CID + " and state>0 and winCent>0";

        string[] pageValUrl = { "act", "id", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.SFC.Model.SfPay> listSFPay = new BCW.SFC.BLL.SfPay().GetSfPays(pageIndex, pageSize, strWhere, out recordCount);
        if (listSFPay.Count > 0)
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            //builder.Append("第" + model.CID + "期<br />");
            builder.Append("<a href =\"" + Utils.getUrl("SFC.aspx?act=jieguo&amp;id=" + id + "") + "\">第" + model.CID + "期</a><br />");
            builder.Append("起底奖池：" + new BCW.SFC.DAL.SfList().getnextprize(model.CID - 1) + "<br />");
            builder.Append("总下注额：" + Money + "<br />");
            builder.Append("系统投入额：" + new BCW.SFC.BLL.SfJackpot().SysPrice(model.CID) + "<br />");
            builder.Append("当期滚存奖池：" + AllPrize(model.CID) + "<br />");
            builder.Append("系统回收：" + new BCW.SFC.BLL.SfJackpot().SysWin(model.CID) + "<br />");
            builder.Append("当期中奖：" + wincent + "<br />");
            builder.Append("结余奖池(滚入下期)：" + new BCW.SFC.DAL.SfList().getnextprize(model.CID) + "<br />");

            if (model.State == 1)
            {
                builder.Append("第" + model.CID + "期开出:<b>" + model.Result + "</b>");
            }
            else
            {
                builder.Append("第" + model.CID + "期未开奖");
            }
            if (ptype == 1)
            {
                builder.Append("<br />共" + recordCount + "次下注");
                builder.Append("/总计下注" + Money + ub.Get("SiteBz") + "");
            }
            else
            {
                builder.Append("<br />共" + recordCount + "次中奖");
                builder.Append("/总计中奖" + wincent + ub.Get("SiteBz") + "");
            }
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

                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.usID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(Convert.ToInt32(n.usID)) + "</a>");
                if (n.State == 0)
                    builder.Append("押" + n.Vote + "/共" + n.VoteNum + "注*" + n.OverRide + "倍/下注" + n.PayCents + "" + ub.Get("SiteBz") + "[" + DT.FormatDate(Convert.ToDateTime(n.AddTime), 5) + "]" + " 标识ID" + n.id);
                else if (n.State == 1)
                {
                    builder.Append("押" + n.Vote + "/共" + n.VoteNum + "注*" + n.OverRide + "倍/下注" + n.PayCents + "" + ub.Get("SiteBz") + "[" + DT.FormatDate(Convert.ToDateTime(n.AddTime), 5) + "]" + " 标识ID" + n.id);
                    if (n.WinCent > 0)
                    {
                        if (n.IsPrize == 10)
                        {
                            builder.Append("赢" + n.WinCent + "" + ub.Get("SiteBz") + "（" + n.change + "奖）" + "");
                        }
                        else
                            builder.Append("赢" + n.WinCent + "" + ub.Get("SiteBz") + "（" + n.IsPrize + "等奖）" + "");

                        if (n.State == 2) { builder.Append("[已兑奖]"); }
                        else { builder.Append("[未兑奖]"); }
                    }
                }
                else
                {
                    if (n.IsPrize == 10)
                    {
                        builder.Append("押" + n.Vote + "/共" + n.VoteNum + "注*" + n.OverRide + "倍/下注" + n.PayCents + "" + ub.Get("SiteBz") + "，赢" + n.WinCent + ub.Get("SiteBz") + "（" + n.change + "奖）" + "[" + DT.FormatDate(Convert.ToDateTime(n.AddTime), 1) + "]" + " 标识ID" + n.id);
                    }
                    else
                        builder.Append("押" + n.Vote + "/共" + n.VoteNum + "注*" + n.OverRide + "倍/下注" + n.PayCents + "" + ub.Get("SiteBz") + "，赢" + n.WinCent + ub.Get("SiteBz") + "（" + n.IsPrize + "等奖）" + "[" + DT.FormatDate(Convert.ToDateTime(n.AddTime), 1) + "]" + " 标识ID" + n.id);
                    if (n.State == 2) { builder.Append("[已兑奖]"); }
                    else { builder.Append("[未兑奖]"); }
                }
                builder.Append(".<a href=\"" + Utils.getUrl("SFC.aspx?act=tui&amp;bqcid=" + n.id + "&amp;id=" + id + "&amp;ptype" + ptype + "") + "\">[退]</a>");
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

        }
        else
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            //builder.Append("第" + model.CID + "期<br />");
            builder.Append("<a href =\"" + Utils.getUrl("SFC.aspx?act=jieguo&amp;id=" + id + "") + "\">第" + model.CID + "期</a><br />");
            builder.Append("起底奖池：" + new BCW.SFC.DAL.SfList().getnextprize(model.CID - 1) + "<br />");
            builder.Append("总下注额：" + Money + "<br />");
            builder.Append("系统投入额：" + new BCW.SFC.BLL.SfJackpot().SysPrice(model.CID) + "<br />");
            if (new BCW.SFC.BLL.SfList().getState(model.CID) == 0)
            {
                builder.Append("当期滚存奖池：" + AllPrize(model.CID) + "<br />");
            }
            else
            {
                builder.Append("当期滚存奖池：" + new BCW.SFC.BLL.SfList().nowprize(model.CID) + "<br />");
            }
            builder.Append("系统回收：" + new BCW.SFC.BLL.SfJackpot().SysWin(model.CID) + "<br />");
            builder.Append("当期中奖：" + wincent + "<br />");
            builder.Append("结余奖池(滚入下期)：" + new BCW.SFC.DAL.SfList().getnextprize(model.CID) + "<br />");

            if (model.State == 1)
            {
                builder.Append("第" + model.CID + "期开出号码:<b>" + model.Result + "</b>");
            }
            else
            {
                builder.Append("第" + model.CID + "期未开奖");
            }
            builder.Append("<br />共0注中奖");
            builder.Append("/总计中奖" + wincent + ub.Get("SiteBz") + "");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("说明：复式中奖（ 一/1#二/2#奖）表示中一等奖1注，二等奖2注");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("SFC.aspx") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //==========删除一条投注记录==================
    private void TuiPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "")
        {
            int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
            int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-9]\d*$", "1"));
            int bqcid = int.Parse(Utils.GetRequest("bqcid", "all", 1, @"^[0-9]\d*$", "1"));
            BCW.SFC.Model.SfPay n = new BCW.SFC.BLL.SfPay().GetSfPay(bqcid);

            Master.Title = "" + GameName + "第" + n.CID + "期删除一条投注记录";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
            builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx") + "\">" + GameName + "</a>");
            builder.Append("&gt;第" + n.CID + "期");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("确定删除该记录吗?");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("ID:" + n.id + ".<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.usID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(Convert.ToInt32(n.usID)) + "</a>");
            if (n.State == 0)
                builder.Append("押" + n.Vote + "/共" + n.VoteNum + "注*" + n.OverRide + "倍/下注" + n.PayCents + "" + ub.Get("SiteBz") + "[" + DT.FormatDate(Convert.ToDateTime(n.AddTime), 5) + "]" + " 标识ID" + n.id);
            else if (n.State == 1)
            {
                builder.Append("押" + n.Vote + "/共" + n.VoteNum + "注*" + n.OverRide + "倍/下注" + n.PayCents + "" + ub.Get("SiteBz") + "[" + DT.FormatDate(Convert.ToDateTime(n.AddTime), 5) + "]" + " 标识ID" + n.id);
                if (n.WinCent > 0)
                {
                    if (n.IsPrize == 10)
                    {
                        builder.Append("赢" + n.WinCent + "" + ub.Get("SiteBz") + "（" + n.change + "奖）" + "");
                    }
                    else
                        builder.Append("赢" + n.WinCent + "" + ub.Get("SiteBz") + "（" + n.IsPrize + "等奖）" + "");

                    if (n.State == 2) { builder.Append("[已兑奖]"); }
                    else { builder.Append("[未兑奖]"); }
                }
            }
            else
            {
                if (n.IsPrize == 10)
                {
                    builder.Append("押" + n.Vote + "/共" + n.VoteNum + "注*" + n.OverRide + "倍/下注" + n.PayCents + "" + ub.Get("SiteBz") + "，赢" + n.WinCent + ub.Get("SiteBz") + "（" + n.change + "奖）" + "[" + DT.FormatDate(Convert.ToDateTime(n.AddTime), 1) + "]" + " 标识ID" + n.id);
                }
                else
                    builder.Append("押" + n.Vote + "/共" + n.VoteNum + "注*" + n.OverRide + "倍/下注" + n.PayCents + "" + ub.Get("SiteBz") + "，赢" + n.WinCent + ub.Get("SiteBz") + "（" + n.IsPrize + "等奖）" + "[" + DT.FormatDate(Convert.ToDateTime(n.AddTime), 1) + "]" + " 标识ID" + n.id);
                if (n.State == 2) { builder.Append("[已兑奖]"); }
                else { builder.Append("[未兑奖]"); }
            }
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            if (n.State == 2)
            {
                if (n.WinCent > 0)
                {
                    builder.Append("确认退还删除记录将收回会员下注赢的" + n.WinCent + ub.Get("SiteBz") + ",退还下注" + n.PayCents + ub.Get("SiteBz") + "<br />");
                }
                else
                {
                    builder.Append("确认退还删除记录将退还会员下注" + n.PayCents + ub.Get("SiteBz") + "<br />");
                }
            }
            else
            {
                builder.Append("确认退还删除记录将退还会员下注" + n.PayCents + ub.Get("SiteBz") + "<br />");
            }
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx?info=ok&amp;act=tui&amp;id=" + id + "&amp;bqcid=" + n.id + "&amp;ptype=" + ptype + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx?act=view&amp;id=" + id + "&amp;ptype=" + ptype + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx?act=view&amp;id=" + id + "&amp;ptype=" + ptype + "") + "\">返回上一级</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
            int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-9]\d*$", "1"));
            int bqcid = int.Parse(Utils.GetRequest("bqcid", "all", 1, @"^[0-9]\d*$", "1"));
            BCW.SFC.Model.SfPay n = new BCW.SFC.BLL.SfPay().GetSfPay(bqcid);
            if (!new BCW.BQC.BLL.BQCPay().Exists(bqcid))
            {
                Utils.Error("不存在的记录", "");
            }
            else
            {
                //根据id查询-购买表
                int meid = Convert.ToInt32(n.usID);//用户名
                string mename = new BCW.BLL.User().GetUsName(meid);//获得id对应的用户名
                int state_get = Convert.ToInt32(n.State);//用户购买情况

                //如果未开奖，退回本金
                if (state_get < 2)
                {
                    new BCW.BLL.User().UpdateiGold(meid, n.PayCents, "系统退回" + GameName + "第" + n.CID + "期下注的" + n.PayCents + "" + ub.Get("SiteBz") + "！");//减少系统总的酷币
                    new BCW.BLL.Guest().Add(1, meid, mename, "系统退回您的" + GameName + "：第" + n.CID + "期下注的" + n.PayCents + "" + ub.Get("SiteBz") + "！");
                }
                //未中奖或已中奖，退回本金-所得
                //else if ((state_get == 2) || (state_get == 1))
                else
                {
                    //Price = model.PutGold - model.GetMoney;//系统-(本金-所得)
                    long gold = 0;//个人酷币
                    long cMoney = 0;//差多少
                    long sMoney = 0;//实扣
                    string ui = string.Empty;
                    gold = new BCW.BLL.User().GetGold(meid);//个人酷币
                    if (n.WinCent > gold)
                    {
                        cMoney = n.WinCent - gold + n.PayCents;
                        sMoney = n.WinCent;
                    }
                    else
                    {
                        sMoney = n.WinCent;
                    }

                    //如果币不够扣则记录日志并冻结IsFreeze
                    if (cMoney > 0)
                    {
                        BCW.Model.Gameowe owe = new BCW.Model.Gameowe();
                        owe.Types = 1;
                        owe.UsID = meid;
                        owe.UsName = mename;
                        owe.Content = "" + GameName + n.CID + "期" + "(" + n.Vote + ")" + "下注" + n.PayCents + "" + ub.Get("SiteBz") + ".系统管理员" + new BCW.User.Manage().IsManageLogin() + "删除.";
                        owe.OweCent = cMoney;
                        owe.BzType = 10;
                        owe.EnId = n.id;
                        owe.AddTime = DateTime.Now;
                        new BCW.BLL.Gameowe().Add(owe);
                        new BCW.BLL.User().UpdateIsFreeze(meid, 1);
                        ui = "实扣" + sMoney + ",还差" + (cMoney) + ",系统已自动将您帐户冻结.";
                    }
                    string oop = string.Empty;
                    if (n.WinCent > 0)
                    {
                        oop = "并扣除所得的" + n.WinCent + "。";
                    }
                    new BCW.BLL.User().UpdateiGold(meid, n.PayCents - n.WinCent, "无效购奖或非法操作，系统退回" + GameName + "第" + n.CID + "期下注的" + n.PayCents + "" + ub.Get("SiteBz") + "." + oop + "" + ui);//减少系统总的酷币
                    new BCW.BLL.Guest().Add(1, meid, mename, "无效购奖或非法操作，系统退回" + GameName + "第" + n.CID + "期下注的" + n.PayCents + "" + ub.Get("SiteBz") + "." + oop + "" + ui);
                }
                ////如果过期不兑奖，退回本金
                //else if (state_get == 3)
                //{
                //    Price = model.PutGold;
                //    new BCW.BLL.User().UpdateiGold(model.UsID, Price, "系统退回新快3第" + model.Lottery_issue + "期未兑奖的" + model.PutGold + "酷币！");//减少系统总的酷币
                //    new BCW.BLL.Guest().Add(1, meid, mename, "系统退回新快3第" + model.Lottery_issue + "期未兑奖的" + model.PutGold + "酷币！");
                //}

                new BCW.SFC.BLL.SfPay().Delete(n.id);

                Utils.Success("删除投注记录", "删除记录成功..", Utils.getPage("SFC.aspx?act=view&amp;id=" + id + "&amp;ptype=" + ptype + ""), "2");

            }
        }
    }
    //开奖结果显示
    private void JieguoPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href =\"" + Utils.getUrl("SFC.aspx") + "\">" + ub.GetSub("SFName", xmlPath) + "</a>&gt;期数详情");
        builder.Append(Out.Tab("</div>", "<br />"));

        int id = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.SFC.Model.SfList model = new BCW.SFC.BLL.SfList().GetSfList(id);

        int State = new BCW.SFC.DAL.SfList().getState(model.CID);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }

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

            string[] Match = model.Match.Split(',');
            string[] Team_Home = model.Team_Home.Split(',');
            string[] Team_Away = model.Team_Away.Split(',');
            string[] Result = model.Result.Split(',');
            string[] MatchTime = model.Start_time.Split(',');
            for (int i = 0; i < Match.Length - 1; i++)
            {
                if (model.Result == "无开奖") { builder.Append(Match[i] + "|" + Team_Home[i] + "(<b>无开奖</b>)" + Team_Away[i] + "|比赛时间" + MatchTime[i] + "<br/>"); }
                else
                {
                    if (State == 0)
                    {
                        builder.Append(Match[i] + "|" + Team_Home[i] + "(<b>未开</b>)" + Team_Away[i] + "|比赛时间" + MatchTime[i] + "<br/>");
                    }
                    else
                        builder.Append(Match[i] + "|" + Team_Home[i] + "(<b>" + SPF(Result[i]) + "</b>)" + Team_Away[i] + "|比赛时间" + MatchTime[i] + "<br/>");
                }
            }
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("第" + model.CID + "期开出:<b>" + model.Result + "</b><br/>");
            builder.Append("详细记录：<br/>");

            string[] Match = model.Match.Split(',');
            string[] Team_Home = model.Team_Home.Split(',');
            string[] Team_Away = model.Team_Away.Split(',');
            string[] Result = model.Result.Split(',');
            string[] MatchTime = model.Start_time.Split(',');
            for (int i = 0; i < Match.Length - 1; i++)
            {
                if (model.Result == "无开奖") { builder.Append(Match[i] + "|" + Team_Home[i] + "(<b>无开奖</b>)" + Team_Away[i] + "|比赛时间" + MatchTime[i] + "<br/>"); }
                else
                {
                    if (State == 0)
                    {
                        builder.Append(Match[i] + "|" + Team_Home[i] + "(<b>未开</b>)" + Team_Away[i] + "|比赛时间" + MatchTime[i] + "<br/>");
                    }
                    else
                        builder.Append(Match[i] + "|" + Team_Home[i] + "(<b>" + SPF(Result[i]) + "</b>)" + Team_Away[i] + "|比赛时间" + MatchTime[i] + "<br/>");
                }
            }

            builder.Append(Out.Tab("</div>", ""));

        }

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("SFC.aspx?act=view&amp;id=" + id + "") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("SFC.aspx") + "\">" + GameName + "管理</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    //查询期号信息（下注与中奖）
    private void ChaxunPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[1-9]\d*$", "0"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]\d*$", "1"));

        Master.Title = "期号详情";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        string sfc = ub.GetSub("SFName", xmlPath);
        builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx") + "\">" + sfc + "</a>&gt;期号详情");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("期号查询|" + "<a href=\"" + Utils.getUrl("SFC.aspx?act=usidcx") + "\">会员查询</a>" + "|<a href=\"" + Utils.getUrl("SFC.aspx?act=jiangcx") + "\">奖池查询</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("查看下注/中奖记录");
        builder.Append(Out.Tab("</div>", ""));

        //开始期数
        int number1 = int.Parse(Utils.GetRequest("number1", "all", 1, @"^[1-9]\d*$", "0"));

        string strText = "查询期数:/,";
        string strName = "number1,act";
        string strType = "num,hidden";
        string strValu = "" + number1 + "'" + Utils.getPage(0) + "";
        string strEmpt = "true,true,false";
        string strIdea = "/";
        string strOthe = "确认搜索,SFC.aspx?act=chaxun&amp;ptype=" + ptype + ",post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        if (number1 == null || number1 == 0)
        {
            number1 = new BCW.SFC.DAL.SfList().CID();
        }
        id = new BCW.SFC.DAL.SfList().id(number1);
        if (id == 0)
        {
            id = 1;
        }

        BCW.SFC.Model.SfList model = new BCW.SFC.BLL.SfList().GetSfList(id);
        if (model == null)
        {
            Utils.Error("不存在的记录，输入的期号未开启或者不存在，请检查输入信息是否正确", "");
        }

        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        builder.Append("查看:");
        if (ptype == 1)
            builder.Append("下注|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx?act=chaxun&amp;ptype=1&amp;id=" + id + "&amp;number1=" + number1 + "&amp;backurl=" + Utils.getPage(0) + "") + "\">下注</a>|");

        if (ptype == 2)
            builder.Append("中奖");
        else
            builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx?act=chaxun&amp;ptype=2&amp;id=" + id + "&amp;number1=" + number1 + "&amp;backurl=" + Utils.getPage(0) + "") + "\">中奖</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        int zhucounts = new BCW.SFC.DAL.SfPay().AllVoteNum(model.CID);
        long Money = new BCW.SFC.DAL.SfPay().getAllPrice(model.CID);
        long wincent = new BCW.SFC.DAL.SfPay().AllWinCentbyCID(model.CID);

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        if (ptype == 1)
            strWhere += "CID=" + model.CID + "";
        else
            strWhere += "CID=" + model.CID + " and state>0 and winCent>0";

        string[] pageValUrl = { "act", "id", "ptype", "number1", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.SFC.Model.SfPay> listSFPay = new BCW.SFC.BLL.SfPay().GetSfPays(pageIndex, pageSize, strWhere, out recordCount);
        if (listSFPay.Count > 0)
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("第" + model.CID + "期<br />");
            builder.Append("起底奖池：" + new BCW.SFC.DAL.SfList().getnextprize(model.CID - 1) + "<br />");
            builder.Append("总下注额：" + Money + "<br />");
            builder.Append("系统投入额：" + new BCW.SFC.BLL.SfJackpot().SysPrice(model.CID) + "<br />");
            builder.Append("当期滚存奖池：" + AllPrize(model.CID) + "<br />");
            builder.Append("系统回收：" + new BCW.SFC.BLL.SfJackpot().SysWin(model.CID) + "<br />");
            builder.Append("当期中奖：" + wincent + "<br />");
            builder.Append("结余奖池(滚入下期)：" + new BCW.SFC.DAL.SfList().getnextprize(model.CID) + "<br />");



            if (model.State != 0)
            {
                builder.Append("第" + model.CID + "期开出:<b>" + model.Result + "</b>");
            }
            else
            {
                builder.Append("第" + model.CID + "期未开奖");
            }
            if (ptype == 1)
            {
                builder.Append("<br />共" + recordCount + "次下注");
                builder.Append("/总计下注" + Money + ub.Get("SiteBz") + "");
            }
            else
            {
                builder.Append("<br />共" + recordCount + "次中奖");
                builder.Append("/总计中奖" + wincent + ub.Get("SiteBz") + "");
            }
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

                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.usID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(Convert.ToInt32(n.usID)) + "</a>");
                if (n.State == 0)
                    builder.Append("押" + n.Vote + "/共" + n.VoteNum + "注*" + n.OverRide + "倍/下注" + n.PayCents + "" + ub.Get("SiteBz") + "[" + DT.FormatDate(Convert.ToDateTime(n.AddTime), 13) + "]" + " 标识ID" + n.id);
                else if (n.State == 1)
                {
                    builder.Append("押" + n.Vote + "/共" + n.VoteNum + "注*" + n.OverRide + "倍/下注" + n.PayCents + "" + ub.Get("SiteBz") + "[" + DT.FormatDate(Convert.ToDateTime(n.AddTime), 1) + "]" + " 标识ID" + n.id);
                    if (n.WinCent > 0)
                    {
                        if (n.IsPrize == 10)
                            builder.Append("（" + n.change + "）" + "赢" + n.WinCent + "" + ub.Get("SiteBz") + "");
                        else
                            builder.Append("（" + n.IsPrize + "等奖）" + "赢" + n.WinCent + "" + ub.Get("SiteBz") + "");
                    }
                }
                else
                {
                    if (n.IsPrize == 10)
                        builder.Append("押" + n.Vote + "/共" + n.VoteNum + "注*" + n.OverRide + "倍/下注" + n.PayCents + "" + ub.Get("SiteBz") + "（" + n.change + "）" + "，赢" + n.WinCent + "" + ub.Get("SiteBz") + "[" + DT.FormatDate(Convert.ToDateTime(n.AddTime), 1) + "]" + " 标识ID" + n.id);
                    else
                        builder.Append("押" + n.Vote + "/共" + n.VoteNum + "注*" + n.OverRide + "倍/下注" + n.PayCents + "" + ub.Get("SiteBz") + "（" + n.IsPrize + "等奖）" + "，赢" + n.WinCent + "" + ub.Get("SiteBz") + "[" + DT.FormatDate(Convert.ToDateTime(n.AddTime), 1) + "]" + " 标识ID" + n.id);

                }
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

        }
        else
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            if (model.State != 0)
            {
                builder.Append("第" + model.CID + "期开出:<b>" + model.Result + "</b>");
            }
            else
            {
                builder.Append("第" + model.CID + "期未开奖");
            }

            builder.Append("<br />共0注中奖");
            builder.Append("/总计中奖" + wincent + ub.Get("SiteBz") + "");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("说明：复式中奖( 一/1#二/2#)表示中一等奖1注，二等奖2注 ");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("SFC.aspx") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    //根据会员ＩＤ查询信息
    private void UsidcxPage()
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]\d*$", "1"));

        Master.Title = "会员查询";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        string sfc = ub.GetSub("SFName", xmlPath);
        builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx") + "\">" + sfc + "</a>&gt;会员查询");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx?act=chaxun") + "\">期号查询</a>" + "|会员查询" + "|<a href=\"" + Utils.getUrl("SFC.aspx?act=jiangcx") + "\">奖池查询</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("查看下注/中奖记录");
        builder.Append(Out.Tab("</div>", ""));

        int UsID = int.Parse(Utils.GetRequest("UsID", "all", 1, @"^[1-9]\d*$", "0"));

        string strText = "查询会员ID:/,";
        string strName = "UsID,act";
        string strType = "num,hidden";
        string strValu = "" + UsID + "'" + Utils.getPage(0) + "";
        string strEmpt = "true,true,false";
        string strIdea = "/";
        string strOthe = "确认搜索,SFC.aspx?act=usidcx&amp;ptype=" + ptype + ",post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));



        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        builder.Append("查看:");
        if (ptype == 1)
            builder.Append("下注|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx?act=usidcx&amp;ptype=1&amp;UsID=" + UsID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">下注</a>|");

        if (ptype == 2)
            builder.Append("中奖");
        else
            builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx?act=usidcx&amp;ptype=2&amp;UsID=" + UsID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">中奖</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        int zhucounts = new BCW.SFC.DAL.SfPay().AllVoteNumbyusID(UsID);
        long Money = new BCW.SFC.DAL.SfPay().getAllPricebyusID(UsID);
        long wincent = new BCW.SFC.DAL.SfPay().AllWinCentbyusID(UsID);

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;


        if (ptype == 1)
            strWhere += "UsID=" + UsID + "";
        else
            strWhere += "UsID=" + UsID + " and state>0 and winCent>0";

        string[] pageValUrl = { "act", "id", "ptype", "UsID", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.SFC.Model.SfPay> listSFPay = new BCW.SFC.BLL.SfPay().GetSfPays(pageIndex, pageSize, strWhere, out recordCount);
        if (listSFPay.Count > 0)
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            if (UsID == 0)
            {
                builder.Append("请输入会员ID");
            }
            else
            {
                builder.Append("会员ID" + UsID);
            }
            if (ptype == 1)
            {
                builder.Append("<br />共" + recordCount + "次下注");
                builder.Append("/总计下注" + Money + ub.Get("SiteBz") + "");
            }
            else
            {
                builder.Append("<br />共" + recordCount + "次中奖");
                builder.Append("/总计中奖" + wincent + ub.Get("SiteBz") + "");
            }
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

                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.usID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(Convert.ToInt32(n.usID)) + "</a>");
                if (n.State == 0)
                    builder.Append("在" + n.CID + "期押" + n.Vote + "/共" + n.VoteNum + "注*" + n.OverRide + "倍/下注" + n.PayCents + "" + ub.Get("SiteBz") + "[" + DT.FormatDate(Convert.ToDateTime(n.AddTime), 13) + "]" + " 标识ID" + n.id);
                else if (n.State == 1)
                {
                    builder.Append("在" + n.CID + "期押" + n.Vote + "/共" + n.VoteNum + "注*" + n.OverRide + "倍/下注" + n.PayCents + "" + ub.Get("SiteBz") + "[" + DT.FormatDate(Convert.ToDateTime(n.AddTime), 1) + "]" + " 标识ID" + n.id);
                    if (n.WinCent > 0)
                    {
                        builder.Append("赢" + n.WinCent + "" + ub.Get("SiteBz") + "");
                    }
                }
                else
                    builder.Append("在" + n.CID + "期押" + n.Vote + "/共" + n.VoteNum + "注*" + n.OverRide + "倍/下注" + n.PayCents + "" + ub.Get("SiteBz") + "，赢" + n.WinCent + "" + ub.Get("SiteBz") + "[" + DT.FormatDate(Convert.ToDateTime(n.AddTime), 1) + "]" + " 标识ID" + n.id);

                k++;
                builder.Append(Out.Tab("</div>", ""));
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

        }
        else
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            if (UsID == 0)
            {
                builder.Append("请输入会员ID");
            }
            else
            {
                builder.Append("会员ID" + UsID);
            }

            builder.Append("<br />共0注中奖");
            builder.Append("/总计中奖" + wincent + ub.Get("SiteBz") + "");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("SFC.aspx") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    //奖池查询信息
    private void JiangcxPage()
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]\d*$", "1"));

        Master.Title = "奖池查询";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        string sfc = ub.GetSub("SFName", xmlPath);
        builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx") + "\">" + sfc + "</a>&gt;奖池查询");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx?act=chaxun") + "\">期号查询</a>" + "|<a href=\"" + Utils.getUrl("SFC.aspx?act=usidcx") + "\">会员查询</a>" + "|奖池查询");
        builder.Append(Out.Tab("</div>", "<br />"));

        int CID = int.Parse(Utils.GetRequest("CID", "all", 1, @"^[1-9]\d*$", "0"));

        string strText = "输入期数:/,";
        string strName = "CID,act";
        string strType = "num,hidden";
        string strValu = "" + CID + "'" + Utils.getPage(0) + "";
        string strEmpt = "true,true,false";
        string strIdea = "/";
        string strOthe = "确认搜索,SFC.aspx?act=jiangcx&amp;ptype=" + ptype + ",post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));

        string[] pageValUrl = { "act", "ptype", "CID", "backurl" };
        int pageIndex = 0;//当前页
        int recordCount;//记录总条数
        int pageSize = 10;//分页大小
        string strOrder = "";
        string strWhere = string.Empty;

        //查询条件
        strWhere = " CID=" + CID + " ";
        strOrder = "CID Desc, AddTime Desc";

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("第" + CID + "期" + ub.GetSub("SFName", xmlPath) + "奖池记录");
        builder.Append(Out.Tab("</div>", "<br />"));


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
                    else
                    {
                        if (n.Prize == 0)
                        {
                            sText = "." + "<h style=\"color:red\">系统在第" + n.CID + "期" + n.other + "" + ub.Get("SiteBz") + "|结余" + n.allmoney + ub.Get("SiteBz") + "|标识id:" + n.id + "(" + Convert.ToDateTime(n.AddTime).ToString("yyyy-MM-dd HH:mm:ss") + ")</h>";
                        }
                        else
                        {
                            sText = "." + "<h style=\"color:red\">系统在第" + n.CID + "期投入" + n.Prize + "" + ub.Get("SiteBz") + "|结余" + n.allmoney + ub.Get("SiteBz") + "|标识id:" + n.id + "(" + Convert.ToDateTime(n.AddTime).ToString("yyyy-MM-dd HH:mm:ss") + ")</h>";
                        }
                    }
                }
                else
                {
                    if (n.WinPrize == 0)
                    {
                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.usID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(n.usID) + "</a>在第" + n.CID + "期投注消费" + n.Prize + "" + ub.Get("SiteBz") + "|结余" + n.allmoney + ub.Get("SiteBz") + "|标识id:" + n.id + "(" + Convert.ToDateTime(n.AddTime).ToString("yyyy-MM-dd HH:mm:ss") + ")";
                    }
                    else
                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.usID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(n.usID) + "</a><h style=\"color:red\">在第" + n.CID + "期" + n.other + "" + ub.Get("SiteBz") + "|结余" + n.allmoney + ub.Get("SiteBz") + "|标识id:" + n.id + "(" + Convert.ToDateTime(n.AddTime).ToString("yyyy-MM-dd HH:mm:ss") + ")</h>";
                }

                builder.AppendFormat("<a href=\"" + Utils.getUrl("SFC.aspx?act=prizelist&amp;ptype=" + ptype + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">  </a>" + d + sText + "   ");

                k++;

                builder.Append(Out.Tab("</div>", ""));
            }
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录..") + "");
        }
        // 分页
        builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0) + "<br />");


        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("SFC.aspx") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    //重置游戏
    private void ResetPage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId != 1 && ManageId != 9)
        {
            //   Utils.Error("权限不足", "");
        }

        string Title = GameName;
        Master.Title = (Title + "管理中心");
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        //string xmlPath = "/Controls/Dawnlife.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;<a href=\"" + Utils.getUrl("SFC.aspx") + "\">" + Title + "管理</a>&gt;游戏重置");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx?info=1&amp;act=reset") + "\">[一键全部重置]</a>");
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "1")
        {
            builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx?act=reset&amp;info=" + 2 + "") + "\">&nbsp;-->确认重置</a>");

        }
        if (info == "2")
        {
            new BCW.Draw.BLL.DrawUser().ClearTable("tb_SfList");
            new BCW.Draw.BLL.DrawUser().ClearTable("tb_SfPay");
            new BCW.Draw.BLL.DrawUser().ClearTable("tb_SfJackpot");

            Utils.Success("重置" + GameName + "", "重置" + GameName + "成功..", Utils.getUrl("SFC.aspx?act=reset"), "3");
        }
        builder.Append(Out.Tab("</div>", "<br />"));


        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b style=\"color:red\">注意：重置后，数据无法恢复。</b><br />");
        builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx") + "\">[再看看吧..]</a>");
        builder.Append(Out.Tab("</div>", "<br />"));


        //builder.Append(Out.Tab("<div>", ""));
        //builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx") + "\">" + Title + "管理中心</a>");
        //builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    // 排行榜
    private void TopPage()
    {
        Master.Title = "" + ub.GetSub("SFName", xmlPath) + "排行榜";

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        string sfc = ub.GetSub("SFName", xmlPath);
        builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx") + "\">" + sfc + "</a>&gt;排行榜");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-1]$", "0"));
        if (ptype == 0)
        {
            builder.Append("<b>期数查询|</b>");
            builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx?act=top&amp;ptype=1") + "\"><b>日期查询</b></a>");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx?act=top&amp;ptype=0") + "\"><b>期数查询</b></a><b>|</b>");
            builder.Append("<b>日期查询</b>");
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("" + ub.GetSub("SFName", xmlPath) + "排行榜");
        builder.Append(Out.Tab("</div>", "<br />"));

        if (ptype == 0)
        {
            string ac = Utils.GetRequest("ac", "all", 1, "", "");
            //开始期数
            int number1 = int.Parse(Utils.GetRequest("number1", "all", 1, @"^[1-9]\d*$", "0"));
            //结束期数
            int number2 = int.Parse(Utils.GetRequest("number2", "all", 1, @"^[1-9]\d*$", "0"));

            if (Utils.ToSChinese(ac) == "确认搜索")
            {
                string strText = "开始期数:/,结束期数:/,";
                string strName = "number1,number2,act";
                string strType = "num,num,hidden";
                string strValu = "" + number1 + "'" + number2 + "'" + Utils.getPage(0) + "";
                string strEmpt = "true,true,true,false";
                string strIdea = "/";
                string strOthe = "确认搜索,SFC.aspx?act=top&amp;ptype=0,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

                builder.Append("<br />");
                int pageIndex;
                int recordCount;
                int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
                string strWhere = "";
                strWhere = "PayCents>0 and State!=0 and CID>=" + number1 + " and CID<=" + number2;

                string[] pageValUrl = { "act", "ac", "backurl" };
                pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                if (pageIndex == 0)
                    pageIndex = 1;
                DataSet ds = new BCW.SFC.BLL.SfPay().GetList(" UsID,sum(WinCent-PayCents) as aa", "" + strWhere + " group by UsID ORDER BY aa DESC");
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
                        if (id > 0)
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

                            builder.Append("[第" + ((pageIndex - 1) * pageSize + k) + "名]<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(UsID) + "</a>赢" + id + "" + ub.Get("SiteBz") + "");

                            k++;
                        }
                        else
                        {
                            //builder.Append(Out.Tab("<div>", ""));
                            //builder.Append("没有相关记录...");
                            //builder.Append(Out.Tab("</div>", "<br />"));
                            //break;
                        }
                        builder.Append(Out.Tab("</div>", ""));
                    }
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                }
                else
                {
                    builder.Append(Out.Div("div", "暂无数据!"));
                }
            }
            else
            {
                string strText = "开始期数:/,结束期数:/,";
                string strName = "number1,number2,act";
                string strType = "num,num,hidden";
                string strValu = "" + number1 + "'" + number2 + "'" + Utils.getPage(0) + "";
                string strEmpt = "true,true,true,false";
                string strIdea = "/";
                string strOthe = "确认搜索,SFC.aspx?act=top&amp;ptype=0,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

                builder.Append("<br />");
                int pageIndex;
                int recordCount;
                int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
                string strWhere = "PayCents>0  and State!=0 ";
                string[] pageValUrl = { "act", "backurl" };
                pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                if (pageIndex == 0)
                    pageIndex = 1;
                DataSet ds = new BCW.SFC.BLL.SfPay().GetList(" UsID,sum(WinCent-PayCents) as aa", "" + strWhere + " group by UsID ORDER BY aa DESC");
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
                        int id = Convert.ToInt32(ds.Tables[0].Rows[koo + i]["aa"]);//用户id
                        if (id > 0)
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

                            builder.Append("[第" + ((pageIndex - 1) * pageSize + k) + "名]<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(UsID) + "</a>赢" + id + "" + ub.Get("SiteBz") + "");

                            k++;
                        }
                        else
                        {
                            //builder.Append(Out.Tab("<div>", ""));
                            //builder.Append("没有相关记录...");
                            //builder.Append(Out.Tab("</div>", "<br />"));
                            //break;
                        }
                        builder.Append(Out.Tab("</div>", ""));
                    }
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                }
                else
                {
                    builder.Append(Out.Div("div", "暂无数据!"));
                }
            }
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
        }
        else
        {
            string ac = Utils.GetRequest("ac", "all", 1, "", "");

            //开始时间
            string searchday1 = Utils.GetRequest("sTime", "all", 1, @"^(?:(?!0000)[0-9]{4}(?:(?:0[1-9]|1[0-2])(?:0[1-9]|1[0-9]|2[0-8])|(?:0[13-9]|1[0-2])(?:29|30)|(?:0[13578]|1[02])31)|(?:[0-9]{2}(?:0[48]|[2468][048]|[13579][26])|(?:0[48]|[2468][048]|[13579][26])00)0229)$", "0");
            //结束时间
            string searchday2 = Utils.GetRequest("oTime", "all", 1, @"^(?:(?!0000)[0-9]{4}(?:(?:0[1-9]|1[0-2])(?:0[1-9]|1[0-9]|2[0-8])|(?:0[13-9]|1[0-2])(?:29|30)|(?:0[13578]|1[02])31)|(?:[0-9]{2}(?:0[48]|[2468][048]|[13579][26])|(?:0[48]|[2468][048]|[13579][26])00)0229)$", "0");

            if (Utils.ToSChinese(ac) == "确认搜索")
            {
                //查询条件  

                if (searchday1 == "")
                {
                    searchday1 = DateTime.Now.ToString("yyyyMMdd") + "00:00:00";
                }
                if (searchday2 == "")
                {
                    searchday2 = DateTime.Now.AddDays(1).ToString("yyyyMMdd") + "23:59:59";
                }

                string strText = "开始日期(日期输入如20160110):/,结束日期:/,";
                string strName = "stime,otime,act";
                string strType = "num,num,hidden";
                string strValu = "" + searchday1 + "'" + searchday2 + "'" + Utils.getPage(0) + "";
                string strEmpt = "true,true,false";
                string strIdea = "/";
                string strOthe = "确认搜索,SFC.aspx?act=top&amp;ptype=1,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

                builder.Append("<br />");
                int pageIndex;
                int recordCount;
                int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
                string strWhere = "";

                //判断是否输入正确的时间格式
                if (Convert.ToInt32(searchday1) == 0 || Convert.ToInt32(searchday2) == 0)
                {
                    Utils.Error("输入时间格式错误！！", "");
                }
                else
                {
                    searchday1 = DateTime.ParseExact(searchday1, "yyyyMMdd", null).ToString("yyyy-MM-dd") + " 00:00:00";
                    searchday2 = DateTime.ParseExact(searchday2, "yyyyMMdd", null).ToString("yyyy-MM-dd") + " 23:59:59";
                    strWhere = "PayCents>0 and State>0 and AddTime between '" + searchday1 + "' and  '" + searchday2 + "'";
                }

                string[] pageValUrl = { "act", "ac", "backurl" };
                pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                if (pageIndex == 0)
                    pageIndex = 1;
                DataSet ds = new BCW.SFC.BLL.SfPay().GetList(" UsID,sum(WinCent-PayCents) as aa", "" + strWhere + " group by UsID ORDER BY aa DESC");
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
                        if (id > 0)
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

                            builder.Append("[第" + ((pageIndex - 1) * pageSize + k) + "名]<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(UsID) + "</a>赢" + id + "" + ub.Get("SiteBz") + "");

                            k++;
                        }
                        else
                        {
                            //builder.Append(Out.Tab("<div>", ""));
                            //builder.Append("没有相关记录1...");
                            //builder.Append(Out.Tab("</div>", "<br />"));
                            //break;
                        }
                        builder.Append(Out.Tab("</div>", ""));
                    }
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                }
                else
                {
                    builder.Append(Out.Div("div", "暂无数据!"));
                }
            }
            else
            {
                string strText = "开始日期(日期输入如20160110):/,结束日期:/,";
                string strName = "stime,otime,act";
                string strType = "num,num,hidden";
                string strValu = "" + searchday1 + "'" + searchday2 + "'" + Utils.getPage(0) + "";
                string strEmpt = "true,true,false";
                string strIdea = "/";
                string strOthe = "确认搜索,SFC.aspx?act=top&amp;ptype=1,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append("<br />");
                int pageIndex;
                int recordCount;
                int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
                string strWhere = "PayCents>0  and State!=0 ";
                string[] pageValUrl = { "act", "backurl" };
                pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                if (pageIndex == 0)
                    pageIndex = 1;
                DataSet ds = new BCW.SFC.BLL.SfPay().GetList(" UsID,sum(WinCent-PayCents) as aa", "" + strWhere + " group by UsID ORDER BY aa DESC");
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
                        int id = Convert.ToInt32(ds.Tables[0].Rows[koo + i]["aa"]);//用户id
                        if (id > 0)
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

                            builder.Append("[第" + ((pageIndex - 1) * pageSize + k) + "名]<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(UsID) + "</a>赢" + id + "" + ub.Get("SiteBz") + "");

                            k++;
                        }
                        else
                        {
                            //builder.Append(Out.Tab("<div>", ""));
                            //builder.Append("没有相关记录2...");
                            //builder.Append(Out.Tab("</div>", "<br />"));
                            //break;
                        }
                        builder.Append(Out.Tab("</div>", ""));
                    }
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                }
                else
                {
                    builder.Append(Out.Div("div", "暂无数据!"));
                }
            }
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
        }


        builder.Append("<a href=\"" + Utils.getPage("SFC.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    // 盈利分析
    private void StatPage()
    {
        Master.Title = "" + ub.GetSub("SFName", xmlPath) + "赢利分析";

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        string sfc = ub.GetSub("SFName", xmlPath);
        builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx") + "\">" + sfc + "</a>&gt;盈利分析");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("=赢利分析=");
        builder.Append(Out.Tab("</div>", "<br />"));

        //上期本金与赢利
        long TodayBuyCent = new BCW.SFC.BLL.SfPay().GetPayCentlast();
        long TodayWinCent = new BCW.SFC.BLL.SfPay().GetWinCentlast();
        long TodaySysCent = new BCW.SFC.BLL.SfList().GetSysPaylast();
        long TodaySysdayprizelast = new BCW.SFC.BLL.SfList().GetSysdayprizelast();
        long TodaySysWinCent = 0;
        try
        {
            TodaySysWinCent = new BCW.SFC.BLL.SfJackpot().GetWinCentlast();
        }
        catch { }

        //近五期本金与赢利
        long TodayBuyCent5 = new BCW.SFC.BLL.SfPay().GetPayCentlast5();
        long TodayWinCent5 = new BCW.SFC.BLL.SfPay().GetWinCentlast5();
        long TodaySysCent5 = new BCW.SFC.BLL.SfList().GetSysPaylast5();
        long TodaySysdayprizelast5 = new BCW.SFC.BLL.SfList().GetSysdayprizelast5();
        long TodaySysWinCent5 = 0;
        try
        {
            TodaySysWinCent5 = new BCW.SFC.BLL.SfJackpot().GetWinCentlast5();
        }
        catch { }

        //总本金与赢利
        long BuyCent = new BCW.SFC.BLL.SfPay().GetPayCent("2000-01-01 12:12:12", "9000-01-01 12:12:12");
        long WinCent = new BCW.SFC.BLL.SfPay().GetWinCent("2000-01-01 12:12:12", "9000-01-01 12:12:12");
        long SysCent = new BCW.SFC.DAL.SfList().GetSyspayall();
        long Sysdayprizeall = new BCW.SFC.DAL.SfList().GetSysdayprizeall();
        long SysWinCent = new BCW.SFC.DAL.SfJackpot().GetWinCentall();


        builder.Append(Out.Tab("<div>", ""));
        builder.Append("上期会员下注:" + TodayBuyCent + "" + ub.Get("SiteBz") + "<br />");
        builder.Append("上期游戏返彩:" + TodayWinCent + "" + ub.Get("SiteBz") + "<br />");
        builder.Append("上期系统投入:" + TodaySysCent + ub.Get("SiteBz") + "<br />");
        builder.Append("上期系统回收:" + TodaySysWinCent + ub.Get("SiteBz") + "<br />");
        builder.Append("上期系统收税:" + TodaySysdayprizelast + ub.Get("SiteBz") + "<br />");
        builder.Append("上期游戏盈利:" + (TodayBuyCent + TodaySysWinCent + TodaySysdayprizelast - TodayWinCent - TodaySysCent) + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", Out.Hr()));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("近5期会员下注:" + TodayBuyCent5 + "" + ub.Get("SiteBz") + "<br />");
        builder.Append("近5期游戏返彩:" + TodayWinCent5 + "" + ub.Get("SiteBz") + "<br />");
        builder.Append("近5期系统投入:" + TodaySysCent5 + ub.Get("SiteBz") + "<br />");
        builder.Append("近5期系统回收:" + TodaySysWinCent5 + ub.Get("SiteBz") + "<br />");
        builder.Append("近5期系统收税:" + TodaySysdayprizelast5 + ub.Get("SiteBz") + "<br />");
        builder.Append("近5期游戏盈利:" + (TodayBuyCent5 + TodaySysWinCent5 + TodaySysdayprizelast5 - TodayWinCent5 - TodaySysCent5) + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", Out.Hr()));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("会员总下注:" + BuyCent + "" + ub.Get("SiteBz") + "<br />");
        builder.Append("游戏总返彩:" + WinCent + "" + ub.Get("SiteBz") + "<br />");
        builder.Append("系统总投入:" + SysCent + "" + ub.Get("SiteBz") + "<br />");
        builder.Append("系统总回收:" + SysWinCent + "" + ub.Get("SiteBz") + "<br />");
        builder.Append("系统总收税:" + Sysdayprizeall + "" + ub.Get("SiteBz") + "<br />");
        builder.Append("游戏总盈利:" + (BuyCent + SysWinCent + Sysdayprizeall - SysCent - WinCent) + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", Out.Hr()));


        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        if (Utils.ToSChinese(ac) == "马上查询")
        {
            string phase1 = (Utils.GetRequest("sTime", "all", 1, @"^[0-9]\d*$", ""));
            string phase2 = (Utils.GetRequest("sTime2", "all", 1, @"^[0-9]\d*$", ""));

            long q_1 = new BCW.SFC.BLL.SfPay().GetPrice("sum(PayCents)", "(CID between'" + phase1 + "' AND '" + phase2 + "') AND IsSpier=0");//消费
            long q_2 = new BCW.SFC.BLL.SfPay().GetPrice("sum(WinCent)", "(CID between'" + phase1 + "' AND '" + phase2 + "') AND IsSpier=0");//收入
            long q_3 = new BCW.SFC.BLL.SfList().GetPrice("sum(sysprize)", "(CID between'" + phase1 + "' AND '" + phase2 + "') AND sysprizestatue!=2");//系统投入
            long q_4 = new BCW.SFC.BLL.SfList().GetPrice("sum(sysprize)", "(CID between'" + phase1 + "' AND '" + phase2 + "') AND sysprizestatue=2");//系统取出
            long q_5 = new BCW.SFC.BLL.SfList().GetPrice("sum(sysdayprize)", "(CID between'" + phase1 + "' AND '" + phase2 + "')  ");//系统手续

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<hr/>" + phase1 + "期到" + phase2 + "期下注：" + q_1 + "<br/>" + phase1 + "期到" + phase2 + "期返奖：" + q_2 + "<br/>");
            builder.Append("" + phase1 + "期到" + phase2 + "期系统投入：" + q_3 + "<br/>" + phase1 + "期到" + phase2 + "期系统取出：" + q_4 + "<br/>");
            builder.Append("" + phase1 + "期到" + phase2 + "期系统收税：" + q_5 + "<br/>" + phase1 + "期到" + phase2 + "期系统盈利：" + (q_1 + q_4 + q_5 - q_2 - q_3) + "");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "开始期号：,结束期号：";
            string strName = "sTime,sTime2";
            string strType = "text,text";
            string strValu = "" + phase1 + "'" + phase2 + "'";
            string strEmpt = "false,false";
            string strIdea = "/";
            string strOthe = "马上查询,SFC.aspx?act=stat,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        else
        {
            builder.Append(Out.Tab("<div>", "<br/>"));
            builder.Append("<hr/>请输入需要查询的期号：");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "开始期号：,结束期号：";
            string strName = "sTime,sTime2";
            string strType = "text,text";
            string strValu = "" + new BCW.SFC.BLL.SfList().CID() + "'" + new BCW.SFC.BLL.SfList().CID() + "'";
            string strEmpt = "false,false";
            string strIdea = "/";
            string strOthe = "马上查询,SFC.aspx?act=stat,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("说明：盈利分析是不包括测试机器人的已经开奖的记录，【盈利】=【下注】+【收税】+【回收】-【投入】-【返彩】");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("SFC.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    // 奖池记录
    private void PrizeListPage()
    {
        Master.Title = "" + ub.GetSub("SFName", xmlPath) + "奖池记录";

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        string sfc = ub.GetSub("SFName", xmlPath);
        builder.Append("<a href=\"" + Utils.getUrl("SFC.aspx") + "\">" + sfc + "</a>&gt;奖池记录");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-5]$", "0"));
        DataSet qi = new BCW.SFC.BLL.SfList().GetList("CID", " State=0 ");//and Sale_StartTime < '" + DateTime.Now + "' and EndTime > '" + DateTime.Now + "'
        for (int i2 = 0; i2 < qi.Tables[0].Rows.Count; i2++)
        {
            int qishu = Convert.ToInt32(qi.Tables[0].Rows[i2][0]);
            //builder.Append(qishu);
            if (i2 == (qi.Tables[0].Rows.Count - 1))
            {
                builder.Append("<a href =\"" + Utils.getUrl("SFC.aspx?act=prizelist&amp;ptype=" + i2 + "") + "\"><b>" + qishu + "期</b></a>");
            }
            else
                builder.Append("<a href =\"" + Utils.getUrl("SFC.aspx?act=prizelist&amp;ptype=" + i2 + "") + "\"><b>" + qishu + "期</b>|</a>");
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        if (!new BCW.SFC.BLL.SfList().Existsjilu())
        {
            Utils.Error("系统错误...", "");
        }
        int CID = 0;
        DataSet dqi = new BCW.SFC.BLL.SfList().GetList("CID", " State=1 Order by CID Desc ");
        try
        {
            CID = Convert.ToInt32(qi.Tables[0].Rows[ptype][0]);
        }
        catch
        {
            CID = Convert.ToInt32(dqi.Tables[0].Rows[0][0]);
        }

        string[] pageValUrl = { "act", "ptype", "backurl" };
        int pageIndex = 0;//当前页
        int recordCount;//记录总条数
        int pageSize = 10;//分页大小
        string strOrder = "";
        string strWhere = string.Empty;

        //查询条件
        if (ptype == 0)
        {
            strWhere = " CID<=" + CID + " ";
        }
        else
        {
            strWhere = " CID=" + CID + " ";
        }
        strOrder = "CID Desc,AddTime Desc,id Desc";

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("" + ub.GetSub("SFName", xmlPath) + "奖池记录");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 0)
        {
            builder.Append("系统当前奖池（<b style=\"color:red\">" + CID + "</b> 期）：" + AllPrize(CID) + "" + ub.Get("SiteBz") + "<br/>");
        }
        else
        {
            builder.Append("系统预售奖池（<b style=\"color:red\">" + CID + "</b> 期）：" + AllPrize(CID) + "" + ub.Get("SiteBz") + "<br/>");
        }
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
                    else if (n.usID == 7)
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
                            sText = "." + "<h style=\"color:red\">系统在第" + n.CID + "期" + n.other + "" + ub.Get("SiteBz") + "|结余" + n.allmoney + ub.Get("SiteBz") + "|标识id:" + n.id + "(" + Convert.ToDateTime(n.AddTime).ToString("yyyy-MM-dd HH:mm:ss") + ")</h>";
                        }
                        else
                        {
                            sText = "." + "<h style=\"color:red\">系统在第" + n.CID + "期投入" + n.Prize + "" + ub.Get("SiteBz") + "|结余" + n.allmoney + ub.Get("SiteBz") + "|标识id:" + n.id + "(" + Convert.ToDateTime(n.AddTime).ToString("yyyy-MM-dd HH:mm:ss") + ")</h>";
                        }
                    }
                }
                else
                {
                    if (n.WinPrize == 0)
                    {
                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.usID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(n.usID) + "</a>在第" + n.CID + "期投注消费" + n.Prize + "" + ub.Get("SiteBz") + "|结余" + n.allmoney + ub.Get("SiteBz") + "|标识id:" + n.id + "(" + Convert.ToDateTime(n.AddTime).ToString("yyyy-MM-dd HH:mm:ss") + ")";
                    }
                    else
                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.usID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(n.usID) + "</a><h style=\"color:red\">在第" + n.CID + "期" + n.other + "" + ub.Get("SiteBz") + "|结余" + n.allmoney + ub.Get("SiteBz") + "|标识id:" + n.id + "(" + Convert.ToDateTime(n.AddTime).ToString("yyyy-MM-dd HH:mm:ss") + ")</h>";
                }

                builder.AppendFormat("<a href=\"" + Utils.getUrl("SFC.aspx?act=prizelist&amp;ptype=" + ptype + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">  </a>" + d + sText + "   ");

                k++;

                builder.Append(Out.Tab("</div>", ""));
            }
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录..") + "");
        }
        // 分页
        builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0) + "<br />");

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("说明：当前期奖池是系统当前期及所有过往期的记录，预售期记录仅为预售期的下注情况");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("SFC.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    //几等奖总注数开奖前
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
    // 遍历数组，组合投注结果
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
    // 计算当前产生的结果数
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
    //胜平负
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