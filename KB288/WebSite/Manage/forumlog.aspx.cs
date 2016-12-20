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
using BCW.Common;
using System.Text.RegularExpressions;

/// <summary>
/// 非1号后台无法查看609ID充值记录 黄国军 20161015
/// 增加取款记录 黄国军 20160718
/// 增加支付统计功能 黄国军 20160611 
/// 增加环迅支付显示 黄国军 20160512 
/// 增加过户日志时间查询 姚志光 20160826
/// </summary>
public partial class Manage_forumlog : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    private string M_Str_mindate = string.Empty;
    private string M_Str_maxdate = string.Empty;

    #region 加载页面参数
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "日志管理";
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "list":
                ListPage();
                break;
            case "bview":
                BViewPage(act);
                break;
            case "xview":
                XViewPage(act);
                break;
            case "gview":
                GViewPage(act);
                break;
            case "gview2":
                GView2Page(act);
                break;
            case "yview":
                YViewPage(act);
                break;
            case "yedit":
                YEditPage(act);
                break;
            case "yeditsave":
                YEditSavePage(act);
                break;
            case "ydel":
                YDelPage();
                break;
            case "vview":           //充值日志
                VViewPage(act);
                break;
            case "vedit":
                VEditPage(act);
                break;
            case "veditsave":
                VEditSavePage(act);
                break;
            //case "vdel":
            //    VDelPage();
            //    break;
            case "clear":
                ClearPage();
                break;
            case "clearok":
                ClearOkPage();
                break;
            case "cleartext":
                ClearTextPage();
                break;
            case "gamelog":
                GameLogPage();
                break;
            case "gdel":
                GDelPage();
                break;
            case "gameowe":
                GameOwePage();
                break;
            case "gameowedel":
                GameOweDelPage();
                break;
            case "gameowedel2":
                GameOweDel2Page();
                break;
            case "searchgold":          //重复消费日志
                SearchGoldPage(act);
                break;
            case "BuyHistory":          //商城记录
                BuyHistoryPage();
                break;
            case "pool":                //统计
                poolPage();
                break;
            case "addpool":             //支付数据添加
                addpoolPage();
                break;
            case "ShopPool":            //商城统计
                ShopPoolPage();
                break;
            case "ShowPool":            //查看提现明细
                ShowPoolPage();
                break;
            case "addlog":
                addlogPage();           //新增记录
                break;
            case "addlog_Shop":         //新增商城记录
                addlog_ShopPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }
    #endregion

    #region 日志管理 ReloadPage
    private void ReloadPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("日志管理");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=bview&amp;ptype=11") + "\">管理日志</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=list") + "\">版务日志</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=bview&amp;ptype=10") + "\">论坛加黑</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=bview&amp;ptype=12") + "\">闲聊加黑</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=xview") + "\">消费日志</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=gamelog") + "\">竞猜日志</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=gview") + "\">过户日志</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=gview2") + "\">内部过户</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=yview") + "\">页面订单</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=vview") + "\">充值日志</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=xview&amp;showtype=1") + "\">编币日志</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=BuyHistory") + "\">商城记录</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=gameowe") + "\">会员欠币</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=xview&amp;showtype=2") + "\">机器人日志</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=gamelog&amp;ptype=2") + "\">" + ub.Get("SiteGqText") + "失败</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=gamelog&amp;ptype=1") + "\">新竞猜日志</a><br />");

        builder.Append("<a href=\"" + Utils.getUrl("app/goldlog.aspx") + "\">新消费日志</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=searchgold") + "\">消费重复查询</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    #endregion

    #region 商城记录 BuyHistoryPage 黄国军 20161015
    /// <summary>
    /// 商城记录
    /// </summary>
    private void BuyHistoryPage()
    {
        Master.Title = "商城记录";
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        int type = int.Parse(Utils.GetRequest("type", "all", 1, @"^[1-3]\d*$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        if (uid > 0)
            builder.Append("ID:" + uid + "");

        builder.Append("商城记录<br />");
        if (type == 0)
            builder.Append("全部|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=BuyHistory&amp;type=0") + "\">全部</a>|");

        if (type == 1)
            builder.Append("成功|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=BuyHistory&amp;type=1") + "\">成功</a>|");

        if (type == 2)
            builder.Append("失败");
        else
            builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=BuyHistory&amp;type=2") + "\">失败</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "NodeId = 28";
        string[] pageValUrl = { "act", "uid", "type" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        if (uid > 0)
            strWhere = " AND UsID=" + uid + "";
        if (type == 1)
        {
            strWhere += " AND (State = 1) ";
        }
        if (type == 2)
        {
            strWhere += " AND (State = 2) ";
        }

        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId != 1)
        {
            strWhere += " AND (UsID <>8888) AND (UsID<>666888) AND (UsID<>1119) AND (UsID<>111) ";
        }
        IList<BCW.Model.Shopkeep> listShopkeep = new BCW.BLL.Shopkeep().GetShopkeeps(pageIndex, pageSize, strWhere, out recordCount);
        if (listShopkeep.Count > 0)
        {
            #region 商品列表
            int k = 1;
            foreach (BCW.Model.Shopkeep n in listShopkeep)
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
                builder.Append("<img src=\"" + n.PrevPic + "\" alt=\"load\"/>");
                builder.Append("<br /><a href=\"" + Utils.getUrl("bbsshop.aspx?act=giftinfo&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.Title + "x" + n.Total + "</a>");

                if (n.NodeId == 28)
                {
                    builder.Append("<br />" + k + ".订单号:" + n.MerBillNo);
                    if (n.State == 0)
                    {
                        cn.com.ips.newpay.WSOrderQuery WSorder = new cn.com.ips.newpay.WSOrderQuery();
                        string SignatureOrder = "", bodystr = "", Resultstr = ""; ;
                        bodystr = BCW.IPSPay.IPSPayMent.GetSignatureByChkOrderByShop(n, ref SignatureOrder);
                        string pGateWayReqstr = BCW.IPSPay.IPSPayMent.IPSPayMentPost_ByOrder(DateTime.Now.ToString("yyyyMMddHHmmss"), SignatureOrder, bodystr);
                        Resultstr = WSorder.getOrderByMerBillNo(pGateWayReqstr);
                        BCW.IPSPay.IPSPayMent.updateorder(Resultstr, false);
                        builder.Append("<br />订单待支付");
                    }
                    else if (n.State == 2)
                    {
                        builder.Append("<br />[已失败]");
                    }
                    else
                    {
                        builder.Append("<br />[交易成功]");
                        string nBankName = BCW.IPSPay.IPSPayMent.GetBankNameByCode(n.BankCode);
                        builder.Append("(" + nBankName + ")");
                    }
                    builder.Append("|" + n.UsName + "(" + n.UsID + ")|" + n.AddTime.ToString());
                }
                else
                {
                    builder.Append("<br /><a href=\"" + Utils.getUrl("friend.aspx?act=online&amp;backurl=" + Server.UrlEncode("/bbs/bbsshop.aspx?act=proxy&amp;id=" + n.ID + "") + "") + "\">[送礼]</a>");
                }
                k++;
                builder.Append(Out.Tab("</div>", ""));

            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            #endregion
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }

        //string strText = "输入用户ID:/,,";
        //string strName = "uid,act";
        //string strType = "num,hidden";
        //string strValu = "'" + act + "";
        //string strEmpt = "true,false";
        //string strIdea = "/";
        //string strOthe = "搜日志,forumlog.aspx,post,1,red";
        //builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=ShopPool&amp;backurl=" + Utils.PostPage(1) + "") + "\">记录统计</a><br />");
        builder.Append("<a href=\"" + Utils.getPage("forumlog.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    #endregion

    #region 版务日志管理 ListPage
    private void ListPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("版务日志管理");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=bview") + "\">=全部日志=</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=bview&amp;ptype=1") + "\">精华帖日志</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=bview&amp;ptype=2") + "\">推荐帖日志</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=bview&amp;ptype=3") + "\">置顶帖日志</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=bview&amp;ptype=4") + "\">固底帖日志</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=bview&amp;ptype=5") + "\">锁定帖日志</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=bview&amp;ptype=6") + "\">删除帖日志</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=bview&amp;ptype=7") + "\">编辑帖日志</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=bview&amp;ptype=8") + "\">转移帖日志</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=bview&amp;ptype=9") + "\">设专题日志</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=bview&amp;ptype=10") + "\">黑名单日志</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=bview&amp;ptype=13") + "\">设粉丝日志</a>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx") + "\">日志管理</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    #endregion

    #region 管理/版务日志 BViewPage
    private void BViewPage(string act)
    {
        Master.Title = "管理/版务日志";
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-9]\d*$", "0"));
        int forumid = int.Parse(Utils.GetRequest("forumid", "all", 1, @"^[0-9]\d*$", "0"));
        int bid = int.Parse(Utils.GetRequest("bid", "all", 1, @"^[0-9]\d*$", "0"));
        string sText = string.Empty;
        if (ptype == 1)
            sText = "精华帖日志";
        else if (ptype == 2)
            sText = "推荐帖日志";
        else if (ptype == 3)
            sText = "置顶帖日志";
        else if (ptype == 4)
            sText = "固底帖日志";
        else if (ptype == 5)
            sText = "锁定帖日志";
        else if (ptype == 6)
            sText = "删除帖日志";
        else if (ptype == 7)
            sText = "编辑帖日志";
        else if (ptype == 8)
            sText = "转移帖日志";
        else if (ptype == 9)
            sText = "设专题日志";
        else if (ptype == 10)
            sText = "黑名单日志";
        else if (ptype == 11)
            sText = "管理日志";
        else if (ptype == 12)
            sText = "闲聊日志";
        else if (ptype == 13)
            sText = "设粉丝日志";
        else
            sText = "全部日志";
        if (bid > 0)
        {
            sText = "帖子日志(含回帖)";
        }
        Master.Title = sText;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("" + sText + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "ptype", "forumid", "bid" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        if (bid > 0)
        {
            strWhere = " BID=" + bid + "";
        }
        else
        {
            if (ptype != 0)
            {
                if (ptype != 11)
                    strWhere = "Types=" + ptype + "";
                else
                    strWhere = "Types=0";

                if (forumid > 0)
                    strWhere += " and ForumID=" + forumid + "";
            }
            else
            {
                if (forumid > 0)
                    strWhere = "ForumID=" + forumid + "";
            }
        }

        // 开始读取列表
        IList<BCW.Model.Forumlog> listForumlog = new BCW.BLL.Forumlog().GetForumlogs(pageIndex, pageSize, strWhere, out recordCount);
        if (listForumlog.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Forumlog n in listForumlog)
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
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".");
                if (n.ForumID > 0)
                {
                    if (n.Types == 12)
                        builder.Append("[" + BCW.User.AppCase.CaseAction(n.ForumID) + "]");
                    else
                    {
                        string ForumName = new BCW.BLL.Forum().GetTitle(n.ForumID);
                        builder.Append("[<a href=\"" + Utils.getUrl("/bbs/forum.aspx?forumid=" + n.ForumID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + ((ForumName == "") ? "未知论坛" : ForumName) + "</a>]");
                    }
                }
                builder.Append("" + Out.SysUBB(n.Content) + "(" + DT.FormatDate(n.AddTime, 6) + ")");
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
        if (bid == 0)
        {
            string strText = "输入(论坛|游戏)ID:/,,";
            string strName = "forumid,ptype,act";
            string strType = "num,hidden,hidden";
            string strValu = "'" + ptype + "'" + act + "";
            string strEmpt = "true,false,false";
            string strIdea = "/";
            string strOthe = "搜日志,forumlog.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        if (bid == 0)
            builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=clear&amp;deltype=" + act + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">清空记录</a><br />");
        else
            builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=cleartext&amp;id=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">清空此帖日志</a><br />");

        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=list") + "\">论坛日志</a><br />");
        builder.Append("<a href=\"" + Utils.getPage("forumlog.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    #endregion

    #region 消费日志 XViewPage
    private void XViewPage(string act)
    {
        Master.Title = "消费日志";
        int showtype = int.Parse(Utils.GetRequest("showtype", "get", 1, @"^[0-2]$", "0"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-1]$", "0"));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        if (uid > 0)
            builder.Append("ID:" + uid + "");

        if (showtype > 0)
            builder.Append("后台");

        builder.Append("消费日志");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 1)
            builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=" + act + "&amp;uid=" + uid + "&amp;showtype=" + showtype + "&amp;ptype=0") + "\">" + ub.Get("SiteBz") + "记录</a>&gt;" + ub.Get("SiteBz2") + "记录");
        else
            builder.Append("" + ub.Get("SiteBz") + "记录&gt;<a href=\"" + Utils.getUrl("forumlog.aspx?act=" + act + "&amp;uid=" + uid + "&amp;showtype=" + showtype + "&amp;ptype=1") + "\">" + ub.Get("SiteBz2") + "记录</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "ptype", "showtype", "uid" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        if (showtype == 0)
            strWhere += "Types=" + ptype + " and BbTag<=1";
        else if (showtype == 1)
            strWhere += "Types=" + ptype + " and (BbTag>=1 and BbTag<=2)";
        else
            strWhere += "Types=" + ptype + " and BbTag=3";

        if (uid > 0)
            strWhere += " and UsID=" + uid + "";

        // 开始读取列表
        IList<BCW.Model.Goldlog> listGoldlog = new BCW.BLL.Goldlog().GetGoldlogs(pageIndex, pageSize, strWhere, out recordCount);
        if (listGoldlog.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Goldlog n in listGoldlog)
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
                builder.AppendFormat("{0}.<a href=\"" + Utils.getUrl("uinfo.aspx?uid={1}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}</a>{3}|操作{4}|结{5}({6})", (pageIndex - 1) * pageSize + k, n.UsId, n.UsName, Out.SysUBB(n.AcText.Replace("/bbs/guess2/", "guess2/")), n.AcGold, n.AfterGold, DT.FormatDate(n.AddTime, 0));

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
        string strText = "输入用户ID:/,,,";
        string strName = "uid,act,ptype,showtpye";
        string strType = "num,hidden,hidden,hidden";
        string strValu = "'" + act + "'" + ptype + "'" + showtype + "";
        string strEmpt = "true,false,false,false";
        string strIdea = "/";
        string strOthe = "搜日志," + Utils.getUrl("forumlog.aspx") + ",post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=clear&amp;deltype=" + act + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">清空记录</a><br />");
        builder.Append("<a href=\"" + Utils.getPage("forumlog.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    #endregion

    #region 新版过户日志 阿光 20160823
    private void GViewPage(string act)
    {
        Master.Title = "过户日志";
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-9]\d*$", "1"));
        int showtype = int.Parse(Utils.GetRequest("showtype", "all", 1, @"^[0-1]$", "0"));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        int toid = int.Parse(Utils.GetRequest("toid", "all", 1, @"^[1-9]\d*$", "0"));
        string start = Utils.GetRequest("start", "all", 1, @"^[^\^]{0,2000}$", "0");
        string over = Utils.GetRequest("over", "all", 1, @"^[^\^]{0,2000}$", "0");
        builder.Append(Out.Tab("<div class=\"title\">", ""));

        if (uid > 0)
            builder.Append("ID:" + uid + "");

        builder.Append("过户日志");
        builder.Append(Out.Tab("</div>", "<br/>"));

        builder.Append(Out.Tab("<div>", ""));
        if (showtype == 0)
            builder.Append("<b>" + ub.Get("SiteBz") + "记录</b> | ");
        else
            builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=gview&amp;uid=" + uid + "&amp;showtype=0&amp;ptype=" + ptype + "") + "\">" + ub.Get("SiteBz") + "记录</a> | ");

        if (showtype == 1)
            builder.Append("<b>" + ub.Get("SiteBz2") + "记录</b>");
        else
            builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=gview&amp;uid=" + uid + "&amp;showtype=1&amp;ptype=" + ptype + "") + "\">" + ub.Get("SiteBz2") + "记录</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        if (uid > 0)
        {
            builder.Append(Out.Tab("<div>", ""));
            if (ptype == 1)
                builder.Append("<b>转入记录</b> | ");
            else
                builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=gview&amp;toid=" + toid + "&amp;uid=" + uid + "&amp;showtype=" + showtype + "&amp;ptype=1") + "\">转入记录</a> | ");

            if (ptype == 2)
                builder.Append("<b>转出记录</b>");
            else
                builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=gview&amp;uid=" + uid + "&amp;showtype=" + showtype + "&amp;ptype=2&amp;toid=" + toid + "&amp;") + "\">转出记录</a>");

            builder.Append(Out.Tab("</div>", "<br />"));
        }
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string strWhereTime = "";
        string[] pageValUrl = { "act", "ptype", "showtype", "uid", "toid", "start", "over" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        if (showtype == 0)
        {
            strWhere = "( Types=" + showtype + " or Types=3  or Types=4 ) ";
            strWhereTime = "( Types=" + showtype + " or Types=3  or Types=4 ) ";
        }
        else
        {
            strWhere = " Types=" + showtype + "";
            strWhereTime = " Types=" + showtype + "";
        }
        DataSet ds = null;
        DataSet dsTime = null;
        string strtext = "";
        string text = "";
        //查询条件 uid（0） 转入toid
        if (ptype == 1)//转入记录
        {
            strtext = "转入";
            text = "来源ID";
            if (uid > 0)
            {
                strWhere += " and ToId=" + uid + "";
                strWhereTime += " and ToId=" + uid + "";
                strtext = toid + "向" + uid + "转入";
            }
            else
            {
                strtext = toid + "全部转入";
            }
            if (toid > 0)
            {
                strWhere += " and FromId=" + toid + " ";
                strWhereTime += " and FromId=" + toid + " ";
            }
            //  strtext = toid + "向" + uid + "转入";
        }
        else//转出记录  uid(0)转出 toid
        {
            strtext = "转出";
            text = "转出ID";
            if (uid > 0)
            {
                strWhere += " and FromId=" + uid + "";
                strWhereTime += " and FromId=" + uid + "";
            }
            if (toid > 0)
            {
                strWhere += " and ToId=" + toid + " ";
                strWhereTime += " and ToId=" + toid + " ";
                strtext = uid + "向" + toid + "转出";
            }
        }
        if (start.Length > 1)
        {
            try
            {
                strWhere += " and AddTime> '" + Convert.ToDateTime(start) + "' and AddTime< '" + Convert.ToDateTime(over) + "'";
                // strWhereTime += " and AddTime> '" + Convert.ToDateTime(start) + "' and AddTime< '" + Convert.ToDateTime(over) + "'";
            }
            catch
            {
                Utils.Error("开始或结束时间格式输入有误.", "");
            }
        }
        else
        {
            start = DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd HH:mm:ss");
            over = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        ds = new BCW.BLL.Transfer().GetList(" sum(AcCent) as gold ", strWhere);
        dsTime = new BCW.BLL.Transfer().GetList(" sum(AcCent) as gold ", strWhereTime);
        string goldtext = "";
        if (showtype == 0)
        { goldtext = ub.Get("SiteBz"); }
        else
            if (showtype == 1)
            { goldtext = ub.Get("SiteBz2"); }
        if (dsTime != null && ds.Tables[0].Rows.Count > 0 && dsTime.Tables[0].Rows[0]["gold"].ToString() != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            if (uid > 0 && toid > 0)
            { builder.Append(strtext); }
            builder.Append("全部总额: " + "<b>" + dsTime.Tables[0].Rows[0]["gold"] + "</b>" + " " + goldtext);
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        if (dsTime != null && toid >= 0)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("从" + start + "到" + over + "用户ID" + strtext + "总额: " + "<b>" + ds.Tables[0].Rows[0]["gold"] + "</b>" + " " + goldtext);
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        // 开始读取列表
        IList<BCW.Model.Transfer> listTransfer = new BCW.BLL.Transfer().GetTransfers(pageIndex, pageSize, strWhere, out recordCount);
        if (listTransfer.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Transfer n in listTransfer)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }
                string bzText = string.Empty;
                if (n.Types == 0)
                    bzText = ub.Get("SiteBz");
                else
                    bzText = ub.Get("SiteBz2");

                if (ptype == 1)
                {
                    builder.AppendFormat("{0}.<a href=\"" + Utils.getUrl("uinfo.aspx?uid={1}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}({1})</a>向<a href=\"" + Utils.getUrl("uinfo.aspx?uid={3}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{4}({3})</a>转入{5}" + bzText + "({6})", (pageIndex - 1) * pageSize + k, n.FromId, n.FromName, n.ToId, n.ToName, n.AcCent, DT.FormatDate(n.AddTime, 0));
                }
                else
                {
                    builder.AppendFormat("{0}.<a href=\"" + Utils.getUrl("uinfo.aspx?uid={1}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}({1})</a>向<a href=\"" + Utils.getUrl("uinfo.aspx?uid={3}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{4}({3})</a>转出{5}" + bzText + "({6})", (pageIndex - 1) * pageSize + k, n.FromId, n.FromName, n.ToId, n.ToName, n.AcCent, DT.FormatDate(n.AddTime, 0));
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
        // if (ptype == 1)//转入
        //{
        //    string strText = "输入用户ID:/,,,";
        //    string strName = "uid,showtype,ptype,act";
        //    string strType = "num,hidden,hidden,hidden";
        //    string strValu = "'" + showtype + "'" + ptype + "'" + act + "";
        //    string strEmpt = "true,false,false,false";
        //    string strIdea = "/";
        //    string strOthe = "搜日志,forumlog.aspx,post,1,red";
        //    builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        //}
        //else
        {
            string strText = "用户ID:/," + text + "(无则为0):/,开始时间:/,结束时间:/,,,";
            string strName = "uid,toid,start,over,showtype,ptype,act";
            string strType = "num,text,text,text,hidden,hidden,hidden";
            string strValu = uid + "'" + toid + "'" + start + "'" + over + "'" + showtype + "'" + ptype + "'" + act + "";
            string strEmpt = "true,true,true,true,false,false,false";
            string strIdea = "/";
            string strOthe = "搜过户日志,forumlog.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=clear&amp;deltype=" + act + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">清空记录</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=gview&amp;uid=" + uid + "&amp;showtype=" + 0 + "&amp;ptype=2&amp;toid=" + toid + "&amp;") + "\">搜转出记录</a><br/>");

        if (ptype > 0)
        {
            builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=gview") + "\">回过户日志</a><br/>");
        }
        builder.Append("<a href=\"" + Utils.getPage("forumlog.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    #endregion

    #region 屏蔽过户日志 GViewPage
    //private void GViewPage(string act)
    //{
    //    Master.Title = "过户日志";
    //    int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-9]\d*$", "1"));
    //    int showtype = int.Parse(Utils.GetRequest("showtype", "all", 1, @"^[0-1]$", "0"));
    //    int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
    //    builder.Append(Out.Tab("<div class=\"title\">", ""));
    //    if (uid > 0)
    //        builder.Append("ID:" + uid + "");

    //    builder.Append("过户日志");
    //    builder.Append(Out.Tab("</div>", "<br />"));

    //    builder.Append(Out.Tab("<div>", ""));
    //    if (showtype == 0)
    //        builder.Append("" + ub.Get("SiteBz") + "记录/");
    //    else
    //        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=gview&amp;uid=" + uid + "&amp;showtype=0&amp;ptype=" + ptype + "") + "\">" + ub.Get("SiteBz") + "</a>/");

    //    if (showtype == 1)
    //        builder.Append("" + ub.Get("SiteBz2") + "记录");
    //    else
    //        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=gview&amp;uid=" + uid + "&amp;showtype=1&amp;ptype=" + ptype + "") + "\">" + ub.Get("SiteBz2") + "</a>");

    //    builder.Append(Out.Tab("</div>", "<br />"));

    //    if (uid > 0)
    //    {
    //        builder.Append(Out.Tab("<div>", ""));
    //        if (ptype == 1)
    //            builder.Append("转入记录/");
    //        else
    //            builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=gview&amp;uid=" + uid + "&amp;showtype=" + showtype + "&amp;ptype=1") + "\">转入</a>/");

    //        if (ptype == 2)
    //            builder.Append("转出记录");
    //        else
    //            builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=gview&amp;uid=" + uid + "&amp;showtype=" + showtype + "&amp;ptype=2") + "\">转出</a>");

    //        builder.Append(Out.Tab("</div>", "<br />"));
    //    }
    //    int pageIndex;
    //    int recordCount;
    //    int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
    //    string strWhere = "";
    //    string[] pageValUrl = { "act", "ptype", "showtype", "uid" };
    //    pageIndex = Utils.ParseInt(Request.QueryString["page"]);
    //    if (pageIndex == 0)
    //        pageIndex = 1;

    //    strWhere = "Types=" + showtype + "";

    //    //查询条件
    //    if (uid > 0)
    //    {
    //        if (ptype == 1)
    //            strWhere += " and ToId=" + uid + "";
    //        else
    //            strWhere += " and FromId=" + uid + "";
    //    }

    //    // 开始读取列表
    //    IList<BCW.Model.Transfer> listTransfer = new BCW.BLL.Transfer().GetTransfers(pageIndex, pageSize, strWhere, out recordCount);
    //    if (listTransfer.Count > 0)
    //    {
    //        int k = 1;
    //        foreach (BCW.Model.Transfer n in listTransfer)
    //        {
    //            if (k % 2 == 0)
    //                builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
    //            else
    //            {
    //                if (k == 1)
    //                    builder.Append(Out.Tab("<div>", ""));
    //                else
    //                    builder.Append(Out.Tab("<div>", "<br />"));
    //            }
    //            string bzText = string.Empty;
    //            if (n.Types == 0)
    //                bzText = ub.Get("SiteBz");
    //            else
    //                bzText = ub.Get("SiteBz2");

    //            if (ptype == 1)
    //            {
    //                builder.AppendFormat("{0}.<a href=\"" + Utils.getUrl("uinfo.aspx?uid={1}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}({1})</a>向<a href=\"" + Utils.getUrl("uinfo.aspx?uid={3}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{4}({3})</a>转入{5}" + bzText + "({6})", (pageIndex - 1) * pageSize + k, n.FromId, n.FromName, n.ToId, n.ToName, n.AcCent, DT.FormatDate(n.AddTime, 0));
    //            }
    //            else
    //            {
    //                builder.AppendFormat("{0}.<a href=\"" + Utils.getUrl("uinfo.aspx?uid={1}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}({1})</a>向<a href=\"" + Utils.getUrl("uinfo.aspx?uid={3}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{4}({3})</a>转出{5}" + bzText + "({6})", (pageIndex - 1) * pageSize + k, n.FromId, n.FromName, n.ToId, n.ToName, n.AcCent, DT.FormatDate(n.AddTime, 0));
    //            }

    //            k++;
    //            builder.Append(Out.Tab("</div>", ""));

    //        }

    //        // 分页
    //        builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

    //    }
    //    else
    //    {
    //        builder.Append(Out.Div("div", "没有相关记录.."));
    //    }
    //    string strText = "输入用户ID:/,,,";
    //    string strName = "uid,showtype,ptype,act";
    //    string strType = "num,hidden,hidden,hidden";
    //    string strValu = "'" + showtype + "'" + ptype + "'" + act + "";
    //    string strEmpt = "true,false,false,false";
    //    string strIdea = "/";
    //    string strOthe = "搜日志," + Utils.getUrl("forumlog.aspx") + ",post,1,red";
    //    builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

    //    builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
    //    builder.Append(Out.Tab("<div>", ""));
    //    builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=clear&amp;deltype=" + act + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">清空记录</a><br />");
    //    builder.Append("<a href=\"" + Utils.getPage("forumlog.aspx") + "\">返回上一级</a><br />");
    //    builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
    //    builder.Append(Out.Tab("</div>", "<br />"));
    //}
    #endregion

    #region 内部ID过户日志 GView2Page
    private void GView2Page(string act)
    {
        Master.Title = "内部ID过户日志";
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-9]\d*$", "1"));
        int showtype = int.Parse(Utils.GetRequest("showtype", "all", 1, @"^[0-1]$", "0"));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        if (uid > 0)
            builder.Append("ID:" + uid + "");

        builder.Append("" + ub.Get("SiteBz") + "过户日志");
        builder.Append(Out.Tab("</div>", "<br />"));

        if (uid > 0)
        {
            builder.Append(Out.Tab("<div>", ""));
            if (ptype == 1)
                builder.Append("转入记录/");
            else
                builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=gview2&amp;uid=" + uid + "&amp;showtype=" + showtype + "&amp;ptype=1") + "\">转入</a>/");

            if (ptype == 2)
                builder.Append("转出记录");
            else
                builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=gview2&amp;uid=" + uid + "&amp;showtype=" + showtype + "&amp;ptype=2") + "\">转出</a>");

            builder.Append(Out.Tab("</div>", "<br />"));
        }
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "ptype", "showtype", "uid" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        strWhere = "Types>=3";

        //查询条件
        if (uid > 0)
        {
            if (ptype == 1)
                strWhere += " and ToId=" + uid + "";
            else
                strWhere += " and FromId=" + uid + "";
        }

        // 开始读取列表
        IList<BCW.Model.Transfer> listTransfer = new BCW.BLL.Transfer().GetTransfers(pageIndex, pageSize, strWhere, out recordCount);
        if (listTransfer.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Transfer n in listTransfer)
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
                string bzText = ub.Get("SiteBz");

                if (ptype == 1)
                {
                    builder.AppendFormat("{0}.<a href=\"" + Utils.getUrl("uinfo.aspx?uid={1}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}({1})</a>向<a href=\"" + Utils.getUrl("uinfo.aspx?uid={3}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{4}({3})</a>转入{5}" + bzText + "({6})", (pageIndex - 1) * pageSize + k, n.FromId, n.FromName, n.ToId, n.ToName, n.AcCent, DT.FormatDate(n.AddTime, 0));
                }
                else
                {
                    builder.AppendFormat("{0}.<a href=\"" + Utils.getUrl("uinfo.aspx?uid={1}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}({1})</a>向<a href=\"" + Utils.getUrl("uinfo.aspx?uid={3}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{4}({3})</a>转出{5}" + bzText + "({6})", (pageIndex - 1) * pageSize + k, n.FromId, n.FromName, n.ToId, n.ToName, n.AcCent, DT.FormatDate(n.AddTime, 0));
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
        string strText = "输入用户ID:/,,,";
        string strName = "uid,showtype,ptype,act";
        string strType = "num,hidden,hidden,hidden";
        string strValu = "'" + showtype + "'" + ptype + "'" + act + "";
        string strEmpt = "true,false,false,false";
        string strIdea = "/";
        string strOthe = "搜日志,forumlog.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=clear&amp;deltype=" + act + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">清空记录</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    #endregion

    #region 充值记录 VViewPage
    /// <summary>
    /// 充值记录
    /// </summary>
    /// <param name="act"></param>
    private void VViewPage(string act)
    {
        Master.Title = "充值记录";
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        int type = int.Parse(Utils.GetRequest("type", "all", 1, @"^[1-9]\d*$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        if (uid > 0)
            builder.Append("ID:" + uid + "");

        #region 分页传值
        builder.Append("充值日志<br />");
        if (type == 0)
            builder.Append("全部|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=vview&amp;type=0") + "\">全部</a>|");

        if (type == 1)
            builder.Append("卡类|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=vview&amp;type=1") + "\">卡类</a>|");

        if (type == 2)
            builder.Append("成功|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=vview&amp;type=2") + "\">成功</a>|");

        if (type == 3)
            builder.Append("待付|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=vview&amp;type=3") + "\">待付</a>|");

        if (type == 4)
            builder.Append("网银");
        else
            builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=vview&amp;type=4") + "\">网银</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "uid", "type" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        if (uid > 0)
            strWhere = "UsID=" + uid + "";

        if (type == 1)
        {
            if (strWhere != "") strWhere += " AND ";
            strWhere += "types <> 100";
        }
        else if (type == 2)
        {
            if (strWhere != "") strWhere += " AND ";
            strWhere += "types = 100 AND State=1";
        }
        else if (type == 3)
        {
            if (strWhere != "") strWhere += " AND ";
            strWhere += "types = 100 AND State=0";
        }
        else if (type == 4)
        {
            if (strWhere != "") strWhere += " AND ";
            strWhere += "types = 100";
        }
        #endregion

        #region 读取列表
        // 开始读取列表
        IList<BCW.Model.Payrmb> listPayrmb = new BCW.BLL.Payrmb().GetPayrmbs(pageIndex, pageSize, strWhere, out recordCount);
        if (listPayrmb.Count > 0)
        {
            #region 读取列表
            int k = 1;
            foreach (BCW.Model.Payrmb n in listPayrmb)
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

                string sType = string.Empty;
                if (n.Types == 1)
                    sType = "神州行";
                else if (n.Types == 2)
                    sType = "联通";
                else if (n.Types == 3)
                    sType = "电信";
                else if (n.Types == 100)
                    sType = "网上支付";

                string sState = string.Empty;
                if (n.State == 0)
                {
                    if (n.Types == 100)
                    {
                        sState = "待付款";
                        //ChkUrl = Out.SysUBB("[url=finance.aspx?act=MerBillPay&amp;MsgId=" + n.ID + "][红]付款[/红][/url]");
                    }
                    else
                        sState = "未回应";
                }
                else if (n.State == 1)
                    sState = "已成功";
                else
                    sState = "已失败";

                int p = (pageIndex - 1) * pageSize + k;

                if (n.Types == 100)
                {
                    builder.AppendFormat("{0}.[{1}]订单:<b>{2}</b>-{3} {4}({5})", p, sState, n.MerBillNo, n.GoodsName, n.Amount.ToString("0.00"), DT.FormatDate(n.AddTime, 1));
                    if (n.State == 1)
                    {
                        builder.Append("<br />");
                        if (n.BankCode != "")
                        {
                            string nBankName = BCW.IPSPay.IPSPayMent.GetBankNameByCode(n.BankCode);
                            builder.Append("(" + nBankName + ")");
                        }
                        if (n.GoodsName != "")
                        {
                            builder.Append(n.GoodsName);
                        }
                    }
                    else
                    {
                        builder.Append("<br />订单待支付");
                    }
                }
                else
                {
                    builder.AppendFormat("<a href=\"" + Utils.getUrl("forumlog.aspx?act=vedit&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[管理]&gt;</a>{0}.[" + sState + "]" + sType + "{1}卡({2})", (pageIndex - 1) * pageSize + k, n.CardAmt, DT.FormatDate(n.AddTime, 1));
                    builder.Append("<br />序列号" + n.CardNum + "|密码|" + n.CardPwd + "|订单号:" + n.CardOrder + "");
                }
                builder.Append("<br /><a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")</a> ip:" + n.AddUsIP + "");
                k++;
                builder.Append(Out.Tab("</div>", ""));

            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            #endregion
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        string strText = "输入用户ID:/,,";
        string strName = "uid,act";
        string strType = "num,hidden";
        string strValu = "'" + act + "";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "搜日志," + Utils.getUrl("forumlog.aspx") + ",post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=pool&amp;backurl=" + Utils.PostPage(1) + "") + "\">记录统计</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=clear&amp;deltype=" + act + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">清空记录</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
        #endregion
    }
    #endregion

    #region 页面订单记录 YViewPage
    private void YViewPage(string act)
    {
        Master.Title = "页面订单记录";
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-4]$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        if (uid > 0)
            builder.Append("ID:" + uid + "");

        builder.Append("页面订单日志");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        if (ptype == 0)
            builder.Append("全部|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=yview&amp;uid=" + uid + "&amp;ptype=0") + "\">全部</a>|");

        if (ptype == 1)
            builder.Append("包周|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=yview&amp;uid=" + uid + "&amp;ptype=1") + "\">包周</a>|");

        if (ptype == 2)
            builder.Append("包月|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=yview&amp;uid=" + uid + "&amp;ptype=2") + "\">包月</a>|");

        if (ptype == 3)
            builder.Append("未过期|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=yview&amp;uid=" + uid + "&amp;ptype=3") + "\">未过期</a>|");

        if (ptype == 4)
            builder.Append("过期");
        else
            builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=yview&amp;uid=" + uid + "&amp;ptype=4") + "\">过期</a>");

        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "uid" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        if (uid > 0)
            strWhere = "UsID=" + uid + "";

        if (strWhere != "" && ptype > 0)
            strWhere += " and ";

        if (ptype == 1)
            strWhere += "SellTypes=1";
        else if (ptype == 2)
            strWhere += "SellTypes=2";
        else if (ptype == 3)
            strWhere += "ExTime>='" + DateTime.Now + "'";
        else if (ptype == 4)
            strWhere += "ExTime<'" + DateTime.Now + "'";

        // 开始读取列表
        IList<BCW.Model.Order> listOrder = new BCW.BLL.Order().GetOrders(pageIndex, pageSize, strWhere, out recordCount);
        if (listOrder.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Order n in listOrder)
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
                string sText = string.Empty;
                if (n.SellTypes == 1)
                    sText = "包周计费";
                else if (n.SellTypes == 2)
                    sText = "包月计费";

                builder.AppendFormat("<a href=\"" + Utils.getUrl("forumlog.aspx?act=yedit&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[管理]&gt;</a>{0}.[" + sText + "]<a href=\"" + Utils.getUrl("/default.aspx?id=" + n.TopicId + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">{1}</a>({2})", (pageIndex - 1) * pageSize + k, n.Title, DT.FormatDate(n.AddTime, 1));
                builder.Append("<br /><a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.UsId + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsId + ")</a>到期:" + n.AddTime + "");
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
        string strText = "输入用户ID:/,,";
        string strName = "uid,act";
        string strType = "num,hidden";
        string strValu = "'" + act + "";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "搜日志,forumlog.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=ydel&amp;backurl=" + Utils.PostPage(1) + "") + "\">清空记录</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    #endregion

    #region 竞猜操作日志 GameLogPage
    private void GameLogPage()
    {
        Master.Title = "竞猜操作日志";
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-2]$", "0"));
        int gid = int.Parse(Utils.GetRequest("gid", "all", 1, @"^[0-9]\d*$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("竞猜操作日志");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "ptype", "gid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        strWhere += "Types=" + ptype + "";
        if (gid != 0)
            strWhere += " and EnId=" + gid + "";

        // 开始读取列表
        IList<BCW.Model.Gamelog> listGamelog = new BCW.BLL.Gamelog().GetGamelogs(pageIndex, pageSize, strWhere, out recordCount);
        if (listGamelog.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Gamelog n in listGamelog)
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
                builder.AppendFormat("{0}.{1}备注:{2}[{3}]", (pageIndex - 1) * pageSize + k, Out.SysUBB(n.Content.Replace("/bbs/guess2/", "guess2/")), n.Notes, DT.FormatDate(n.AddTime, 1));

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
        string strText = "输入赛事ID:/,,,";
        string strName = "gid,ptype,act,backurl";
        string strType = "num,hidden,hidden,hidden";
        string strValu = "'" + ptype + "'gamelog'" + Utils.getPage(0) + "";
        string strEmpt = "true,false,false,false";
        string strIdea = "/";
        string strOthe = "搜日志,forumlog.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=gdel&amp;ptype=" + ptype + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">清空记录</a><br />");
        builder.Append("<a href=\"" + Utils.getPage("forumlog.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    #endregion

    #region 编辑订单 YEditPage
    private void YEditPage(string act)
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId != 1 && ManageId != 11)
        {
            Utils.Error("只有系统管理员1号才可以进行此操作", "");
        }
        Master.Title = "编辑订单";
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.Order model = new BCW.BLL.Order().GetOrder(id);
        if (model == null)
        {
            Utils.Error("不存在的订单记录", "");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("编辑订单");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("页面《" + model.Title + "》<br />");
        builder.Append("页面ID：" + model.TopicId + "<br />");
        if (model.SellTypes == 1)
            builder.Append("类型：包周");
        else if (model.SellTypes == 2)
            builder.Append("类型：包月");

        builder.Append(Out.Tab("</div>", ""));

        string strText = "用户ID:/,用户昵称:/,开始时间:/,截止时间:/,,";
        string strName = "UsID,UsName,AddTime,ExTime,id,act";
        string strType = "num,text,date,date,hidden,hidden";
        string strValu = "" + model.UsId + "'" + model.UsName + "'" + DT.FormatDate(model.AddTime, 0) + "'" + DT.FormatDate(model.ExTime, 0) + "'" + id + "'yeditsave";
        string strEmpt = "false,false,false,false,false,false";
        string strIdea = "/";
        string strOthe = "确定编辑,forumlog.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(" <a href=\"" + Utils.getPage("forumlog.aspx?act=yview") + "\">取消</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=ydel&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">删除订单</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?acct=yview") + "\">订单管理</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    #endregion

    #region 编辑订单成功 YEditSavePage
    private void YEditSavePage(string act)
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId != 1 && ManageId != 11)
        {
            Utils.Error("只有系统管理员1号才可以进行此操作", "");
        }
        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[1-9]\d*$", "ID错误"));
        int UsID = int.Parse(Utils.GetRequest("UsID", "post", 2, @"^[1-9]\d*$", "用户ID错误"));
        string UsName = Utils.GetRequest("UsName", "post", 2, @"^[^\^]{1,50}$", "昵称不超50字");
        DateTime AddTime = Utils.ParseTime(Utils.GetRequest("AddTime", "post", 2, DT.RegexTime, "开始时间填写出错"));
        DateTime ExTime = Utils.ParseTime(Utils.GetRequest("ExTime", "post", 2, DT.RegexTime, "截止时间填写出错"));
        if (!new BCW.BLL.Order().Exists(id))
        {
            Utils.Error("不存在的订单记录", "");
        }
        if (!new BCW.BLL.User().Exists(UsID))
        {
            Utils.Error("不存在的用户ID", "");
        }

        BCW.Model.Order model = new BCW.Model.Order();
        model.ID = id;
        model.UsId = UsID;
        model.UsName = UsName;
        model.AddTime = AddTime;
        model.ExTime = ExTime;
        new BCW.BLL.Order().Update2(model);
        Utils.Success("编辑订单", "编辑订单成功..", Utils.getPage("forumlog.aspx?act=yview"), "1");
    }
    #endregion

    #region 删除订单 YDelPage
    private void YDelPage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId != 1 && ManageId != 11)
        {
            Utils.Error("只有系统管理员1号才可以进行此操作", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 1, @"^[0-9]\d*$", "0"));
        string Title = string.Empty;
        if (id > 0)
        {
            Title = new BCW.BLL.Order().GetTitle(id);
            if (Title == "")
            {
                Utils.Error("不存在的订单记录", "");
            }
        }
        if (info == "")
        {
            Master.Title = "删除订单";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            if (id > 0)
            {
                builder.Append("确定删除《" + Title + "》订单记录吗");
            }
            else
            {
                builder.Append("不可恢复！请谨慎删除订单：");
            }
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            if (id > 0)
            {
                builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?info=ok1&amp;act=ydel&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=yedit&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?info=ok2&amp;act=ydel&amp;backurl=" + Utils.getPage(0) + "") + "\">删除全部过期</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?info=ok3&amp;act=ydel&amp;backurl=" + Utils.getPage(0) + "") + "\">删除所有(含未过期)</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=yview") + "\">先留着吧..</a>");
            }
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            //删除
            if (info == "ok1")
                new BCW.BLL.Order().Delete("ID=" + id + "");
            else if (info == "ok2")
                new BCW.BLL.Order().Delete("ExTime<'" + DateTime.Now + "'");
            else if (info == "ok3")
                new BCW.BLL.Order().Delete("");

            Utils.Success("删除订单", "删除订单成功..", Utils.getPage("forumlog.aspx?act=yview"), "1");
        }
    }
    #endregion

    #region 编辑充值 VEditPage
    private void VEditPage(string act)
    {
        Master.Title = "编辑充值";
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.Payrmb model = new BCW.BLL.Payrmb().GetPayrmb(id);
        if (model == null)
        {
            Utils.Error("不存在的充值记录", "");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("编辑充值");
        builder.Append(Out.Tab("</div>", ""));

        //string strText = "用户ID:/,用户昵称:/,卡类型:/,卡面额:/,序列号:/,卡密码:/,订单号:/,充值状态:/,操作IP:/,充值时间:/,,";
        //string strName = "UsID,UsName,Types,CardAmt,CardNum,CardPwd,CardOrder,State,AddUsIP,AddTime,id,act";
        //string strType = "num,text,select,text,text,text,text,select,text,date,hidden,hidden";
        //string strValu = "" + model.UsID + "'" + model.UsName + "'" + model.Types + "'" + model.CardAmt + "'" + model.CardNum + "'" + model.CardPwd + "'" + model.CardOrder + "'"+model.State+"'"+model.AddUsIP+"'" + DT.FormatDate(model.AddTime, 0) + "'" + id + "'veditsave";
        //string strEmpt = "false,false,1|神州行|2|联通|3|电信,false,false,false,false,0|未回应|1|已成功|2|已失败,false,false";
        //string strIdea = "/";
        //string strOthe = "确定编辑,forumlog.aspx,post,1,red";
        //builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        string strText = "卡面额(元):/,充值状态:/,,";
        string strName = "CardAmt,State,id,act";
        string strType = "num,select,hidden,hidden";
        string strValu = "" + model.CardAmt + "'" + model.State + "'" + id + "'veditsave";
        string strEmpt = "false,0|未回应|1|已成功|2|已失败,false,false";
        string strIdea = "/";
        string strOthe = "确定状态,forumlog.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));


        builder.Append(Out.Tab("<div>", ""));
        builder.Append(" <a href=\"" + Utils.getPage("forumlog.aspx?act=vview") + "\">取消</a><br />确定状态后，系统将以内线方式将充值结果通知会员");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        //builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=vdel&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">删除充值</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=vview") + "\">充值管理</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    #endregion

    #region 保存充值状态 VEditSavePage
    private void VEditSavePage(string act)
    {
        //int ManageId = new BCW.User.Manage().IsManageLogin();
        //if (ManageId != 1 && ManageId != 11 )
        //{
        //    Utils.Error("只有系统管理员1号才可以进行此操作", "");
        //}
        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.Payrmb model = new BCW.BLL.Payrmb().GetPayrmb(id);
        if (model == null)
        {
            Utils.Error("不存在的充值记录", "");
        }
        if (model.State > 0)
        {
            Utils.Error("该充值记录已处理", "");
        }
        //int UsID = int.Parse(Utils.GetRequest("UsID", "post", 2, @"^[1-9]\d*$", "用户ID错误"));
        //string UsName = Utils.GetRequest("UsName", "post", 2, @"^[^\^]{1,50}$", "昵称不超50字");
        //int Types = int.Parse(Utils.GetRequest("Types", "post", 2, @"^[1-3]$", "卡类型选择错误"));
        int CardAmt = int.Parse(Utils.GetRequest("CardAmt", "post", 2, @"^[0-9]\d*$", "卡面额填写错误"));
        //string CardNum = Utils.GetRequest("CardNum", "post", 2, @"^[0-9]{1,50}$", "序列号填写错误");
        //string CardPwd = Utils.GetRequest("CardPwd", "post", 2, @"^[0-9]{1,50}$", "卡密码填写错误");
        //string CardOrder = Utils.GetRequest("CardOrder", "post", 2, @"^[0-9]{1,50}$", "订单号填写错误");
        int State = int.Parse(Utils.GetRequest("State", "post", 2, @"^[1-2]$", "充值状态选择错误"));
        //string AddUsIP = Utils.GetRequest("AddUsIP", "post", 2, @"^[^\^]{1,50}$", "操作IP填写错误");
        //if (!Ipaddr.IsIPAddress(AddUsIP))
        //{
        //    Utils.Error("操作IP填写错误", "");
        //}
        //DateTime AddTime = Utils.ParseTime(Utils.GetRequest("AddTime", "post", 2, DT.RegexTime, "充值时间填写出错"));
        //if (!new BCW.BLL.Payrmb().Exists(id))
        //{
        //    Utils.Error("不存在的充值记录", "");
        //}
        //if (!new BCW.BLL.User().Exists(UsID))
        //{
        //    Utils.Error("不存在的用户ID", "");
        //}

        //BCW.Model.Payrmb model = new BCW.Model.Payrmb();
        //model.ID = id;
        //model.UsID = UsID;
        //model.UsName = UsName;
        //model.Types = Types;
        //model.CardAmt = CardAmt;
        //model.CardNum = CardNum;
        //model.CardPwd = CardPwd;
        //model.CardOrder = CardOrder;
        //model.State = State;
        //model.AddUsIP = AddUsIP;
        //model.AddTime = AddTime;
        //new BCW.BLL.Payrmb().Update2(model);
        //Utils.Success("编辑充值", "编辑充值成功..", Utils.getPage("forumlog.aspx?act=vview"), "1");

        //new BCW.BLL.Payrmb();
        //比例
        if (State == 1)
        {
            string xmlPath = "/Controls/finance.xml";
            int Tar = Utils.ParseInt(ub.GetSub("FinanceSZXTar", xmlPath));
            if (Tar == 0)
                Tar = 1;
            //充入币种
            if (ub.GetSub("FinanceSZXType", xmlPath) == "0")
                new BCW.BLL.User().UpdateiGold(model.UsID, model.UsName, Convert.ToInt64(CardAmt * Tar), "充值");
            else
                new BCW.BLL.User().UpdateiMoney(model.UsID, model.UsName, Convert.ToInt64(CardAmt * Tar), "充值");

            model.ID = id;
            model.CardAmt = CardAmt;
            model.State = 1;
            new BCW.BLL.Payrmb().Update3(model);
            //发内线给会员
            new BCW.BLL.Guest().Add(model.UsID, model.UsName, "您的充值已成功，订单号:" + model.CardOrder + "[br][url=/bbs/finance.aspx?act=viplist]查看充值记录[/url]");
            Utils.Success("确认充值状态", "已确认充值状态为“成功”", Utils.getPage("forumlog.aspx?act=vview"), "1");
        }
        else if (State == 2)
        {
            model.ID = id;
            model.CardAmt = CardAmt;
            model.State = 2;
            new BCW.BLL.Payrmb().Update3(model);
            //发内线给会员
            new BCW.BLL.Guest().Add(model.UsID, model.UsName, "您的充值已失败，订单号:" + model.CardOrder + "[br][url=/bbs/finance.aspx?act=viplist]查看充值记录[/url]");
            Utils.Success("确认充值状态", "已确认充值状态为“失败”", Utils.getPage("forumlog.aspx?act=vview"), "1");
        }

    }
    #endregion

    #region 删除订单 VDelPage
    private void VDelPage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId != 1 && ManageId != 11)
        {
            Utils.Error("只有系统管理员1号才可以进行此操作", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 1, @"^[0-9]\d*$", "0"));
        if (info == "")
        {
            Master.Title = "删除订单";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("不可恢复！请谨慎删除充值记录");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?info=ok&amp;act=vdel&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=vedit&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            //删除
            if (!new BCW.BLL.Payrmb().Exists(id))
            {
                Utils.Error("不存在的充值记录", "");
            }
            new BCW.BLL.Payrmb().Delete("id=" + id + "");
            Utils.Success("删除充值", "删除充值记录成功..", Utils.getPage("forumlog.aspx?act=vview"), "1");
        }
    }
    #endregion

    #region 清空竞猜日志 GDelPage
    private void GDelPage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId != 1 && ManageId != 11)
        {
            Utils.Error("只有系统管理员1号才可以进行此操作", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-9]\d*$", "0"));
        if (info == "")
        {
            Master.Title = "清空竞猜日志";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("不可恢复！请谨慎清空竞猜日志");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?info=ok&amp;act=gdel&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=gamelog&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            //删除
            new BCW.BLL.Gamelog().Delete("Types=" + ptype + "");
            Utils.Success("清空竞猜日志", "清空竞猜日志成功..", Utils.getPage("forumlog.aspx?act=game&amp;ptype=" + ptype + ""), "1");
        }
    }
    #endregion

    #region 会员欠币日志 GameOwePage
    private void GameOwePage()
    {
        Master.Title = "会员欠币日志";
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-1]$", "0"));
        int UsID = int.Parse(Utils.GetRequest("UsID", "all", 1, @"^[0-9]\d*$", "0"));
        int gid = int.Parse(Utils.GetRequest("gid", "all", 1, @"^[0-9]\d*$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("会员欠币日志");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "ptype", "UsID", "gid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        strWhere += "Types=" + ptype + "";
        if (UsID != 0)
            strWhere += " and UsID=" + UsID + "";

        if (gid != 0)
            strWhere += " and EnId=" + gid + "";

        // 开始读取列表
        IList<BCW.Model.Gameowe> listGameowe = new BCW.BLL.Gameowe().GetGameowes(pageIndex, pageSize, strWhere, out recordCount);
        if (listGameowe.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Gameowe n in listGameowe)
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
                string bzText = string.Empty;
                if (n.BzType == 0)
                    bzText = ub.Get("SiteBz");
                else
                    bzText = ub.Get("SiteBz2");

                builder.AppendFormat("{0}.<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "</a>在{1}欠下:{2}" + bzText + "[{3}]", (pageIndex - 1) * pageSize + k, Out.SysUBB(n.Content.Replace("showGuess.aspx", "guess/showGuess.aspx")), n.OweCent, DT.FormatDate(n.AddTime, 1));
                if (ptype == 0)
                    builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=gameowedel&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[扣除]</a>");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=gameowedel2&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[扣除]</a>");

                builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?act=freeze&amp;id=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[冻结]</a>");

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
        string strText = "输入会员ID:/,,,";
        string strName = "UsID,act,ptype,backurl";
        string strType = "num,hidden,hidden,hidden";
        string strValu = "'gameowe'" + ptype + "'" + Utils.getPage(0) + "";
        string strEmpt = "true,false,false,false";
        string strIdea = "/";
        string strOthe = "搜欠币,forumlog.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("forumlog.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));

    }
    #endregion

    #region 扣除欠币 GameOweDelPage
    private void GameOweDelPage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.Gameowe model = new BCW.BLL.Gameowe().GetGameowe(0, id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        long gold = 0;
        string bzText = string.Empty;
        if (model.BzType == 0)
        {
            bzText = ub.Get("SiteBz");
            gold = new BCW.BLL.User().GetGold(model.UsID);
        }
        else
        {
            bzText = ub.Get("SiteBz2");
            gold = new BCW.BLL.User().GetMoney(model.UsID);
        }
        if (info == "")
        {
            Master.Title = "扣除欠币";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("对象:<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.UsName + "(" + model.UsID + ")</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("当前自带:" + gold + "" + bzText + "<br />");
            builder.Append("欠币:" + model.OweCent + "" + bzText + "");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?info=ok&amp;act=gameowedel&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定扣除</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("forumlog.aspx?act=gameowe") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            //if (gold < model.OweCent)
            //{
            //    Utils.Error("" + bzText + "不足扣除", "");
            //}
            //删除
            if (model.BzType == 0)
            {
                new BCW.BLL.User().UpdateiGold(model.UsID, model.UsName, -model.OweCent, "后台管理员" + ManageId + "号扣除欠币");
            }
            else
            {
                new BCW.BLL.User().UpdateiMoney(model.UsID, model.UsName, -model.OweCent, "后台管理员" + ManageId + "号扣除欠币");

            }
            new BCW.BLL.Gameowe().Delete(0, id);
            Utils.Success("扣除欠币", "扣除欠币成功..", Utils.getPage("forumlog.aspx?act=gameowe"), "1");
        }
    }
    #endregion

    #region 扣除欠币 GameOweDel2Page
    private void GameOweDel2Page()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.Gameowe model = new BCW.BLL.Gameowe().GetGameowe(1, id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        long gold = 0;
        string bzText = string.Empty;
        if (model.BzType == 0)
        {
            bzText = ub.Get("SiteBz");
            gold = new BCW.BLL.User().GetGold(model.UsID);
        }
        else
        {
            bzText = ub.Get("SiteBz2");
            gold = new BCW.BLL.User().GetMoney(model.UsID);
        }
        if (info == "")
        {
            Master.Title = "扣除欠币";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("对象:<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.UsName + "(" + model.UsID + ")</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("当前自带:" + gold + "" + bzText + "<br />");
            builder.Append("欠币:" + model.OweCent + "" + bzText + "");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?info=ok&amp;act=gameowedel2&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定扣除</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("forumlog.aspx?act=gameowe") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            //if (gold < model.OweCent)
            //{
            //    Utils.Error("" + bzText + "不足扣除", "");
            //}
            //删除
            if (model.BzType == 0)
            {
                new BCW.BLL.User().UpdateiGold(model.UsID, model.UsName, -model.OweCent, "后台管理员" + ManageId + "号扣除欠币");
            }
            else
            {
                new BCW.BLL.User().UpdateiMoney(model.UsID, model.UsName, -model.OweCent, "后台管理员" + ManageId + "号扣除欠币");

            }
            new BCW.BLL.Gameowe().Delete(1, id);
            Utils.Success("扣除欠币", "扣除欠币成功..", Utils.getPage("forumlog.aspx?act=gameowe"), "1");
        }
    }
    #endregion

    #region 清空记录 ClearPage
    /// <summary>
    /// 清空记录
    /// </summary>
    private void ClearPage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId != 1 && ManageId != 11)
        {
            Utils.Error("只有系统管理员1号才可以进行此操作", "");
        }
        string deltype = Utils.GetRequest("deltype", "get", 1, "", "");
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        if (deltype == "bview")
            builder.Append("清空管理/版务日志");
        else if (deltype == "xview")
            builder.Append("清空消费日志");
        else if (deltype == "gview")
            builder.Append("清空过户日志");
        else if (deltype == "gview2")
            builder.Append("清空内部过户日志");
        else if (deltype == "vview")
            builder.Append("清空充值日志");

        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=clearok&amp;deltype=" + deltype + "&amp;ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">清空本日前</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=clearok&amp;deltype=" + deltype + "&amp;ptype=2&amp;backurl=" + Utils.getPage(0) + "") + "\">清空本周前</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=clearok&amp;deltype=" + deltype + "&amp;ptype=3&amp;backurl=" + Utils.getPage(0) + "") + "\">清空本月前</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=clearok&amp;deltype=" + deltype + "&amp;ptype=4&amp;backurl=" + Utils.getPage(0) + "") + "\">清空全部</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("forumlog.aspx?act=" + deltype + "") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    #endregion

    #region 清空记录确认 ClearOkPage
    /// <summary>
    /// 清空记录
    /// </summary>
    private void ClearOkPage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId != 1 && ManageId != 11)
        {
            Utils.Error("只有系统管理员1号才可以进行此操作", "");
        }
        string act = Utils.GetRequest("act", "get", 1, "", "");
        string deltype = Utils.GetRequest("deltype", "get", 1, "", "");
        string info = Utils.GetRequest("info", "get", 1, "", "");
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-4]$", "0"));
        if (info != "ok" && info != "acok")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定要清空吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?info=ok&amp;act=" + act + "&amp;deltype=" + deltype + "&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定清空</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("forumlog.aspx?act=" + deltype + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=" + deltype + "") + "\">返回上一级</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (info == "ok")
            {
                Utils.Success("正在清空", "正在清空,请稍后..", Utils.getUrl("forumlog.aspx?info=acok&amp;act=" + act + "&amp;deltype=" + deltype + "&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + ""), "2");
            }
            else
            {
                if (ptype == 1)
                {
                    //保留本日计算
                    M_Str_mindate = DateTime.Now.ToShortDateString() + " 0:00:00";
                    if (deltype == "bview")
                        new BCW.BLL.Forumlog().Delete("AddTime<'" + M_Str_mindate + "'");
                    else if (deltype == "xview")
                        new BCW.BLL.Goldlog().Delete("AddTime<'" + M_Str_mindate + "'");
                    else if (deltype == "gview")
                        new BCW.BLL.Transfer().Delete("Types<>3 and AddTime<'" + M_Str_mindate + "'");
                    else if (deltype == "gview2")
                        new BCW.BLL.Transfer().Delete("Types=3 and AddTime<'" + M_Str_mindate + "'");
                    else if (deltype == "vview")
                        new BCW.BLL.Payrmb().Delete("AddTime<'" + M_Str_mindate + "'");

                }
                else if (ptype == 2)
                {
                    //保留本周计算
                    switch (DateTime.Now.DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            M_Str_mindate = DateTime.Now.AddDays(0).ToShortDateString() + " 0:00:00";
                            break;
                        case DayOfWeek.Tuesday:
                            M_Str_mindate = DateTime.Now.AddDays(-1).ToShortDateString() + " 0:00:00";
                            break;
                        case DayOfWeek.Wednesday:
                            M_Str_mindate = DateTime.Now.AddDays(-2).ToShortDateString() + " 0:00:00";
                            break;
                        case DayOfWeek.Thursday:
                            M_Str_mindate = DateTime.Now.AddDays(-3).ToShortDateString() + " 0:00:00";
                            break;
                        case DayOfWeek.Friday:
                            M_Str_mindate = DateTime.Now.AddDays(-4).ToShortDateString() + " 0:00:00";
                            break;
                        case DayOfWeek.Saturday:
                            M_Str_mindate = DateTime.Now.AddDays(-5).ToShortDateString() + " 0:00:00";
                            break;
                        case DayOfWeek.Sunday:
                            M_Str_mindate = DateTime.Now.AddDays(-6).ToShortDateString() + " 0:00:00";
                            break;
                    }
                    if (deltype == "bview")
                        new BCW.BLL.Forumlog().Delete("AddTime<'" + M_Str_mindate + "'");
                    else if (deltype == "xview")
                        new BCW.BLL.Goldlog().Delete("AddTime<'" + M_Str_mindate + "'");
                    else if (deltype == "gview")
                        new BCW.BLL.Transfer().Delete("Types<>3 and AddTime<'" + M_Str_mindate + "'");
                    else if (deltype == "gview2")
                        new BCW.BLL.Transfer().Delete("Types=3 and AddTime<'" + M_Str_mindate + "'");
                    else if (deltype == "vview")
                        new BCW.BLL.Payrmb().Delete("AddTime<'" + M_Str_mindate + "'");

                }
                else if (ptype == 3)
                {
                    //保留本月计算
                    string MonthText = DateTime.Now.Year + "-" + DateTime.Now.Month;
                    M_Str_mindate = MonthText + "-1 0:00:00";
                    if (deltype == "bview")
                        new BCW.BLL.Forumlog().Delete("AddTime<'" + M_Str_mindate + "'");
                    else if (deltype == "xview")
                        new BCW.BLL.Goldlog().Delete("AddTime<'" + M_Str_mindate + "'");
                    else if (deltype == "gview")
                        new BCW.BLL.Transfer().Delete("Types<>3 and AddTime<'" + M_Str_mindate + "'");
                    else if (deltype == "gview2")
                        new BCW.BLL.Transfer().Delete("Types=3 and AddTime<'" + M_Str_mindate + "'");
                    else if (deltype == "vview")
                        new BCW.BLL.Payrmb().Delete("AddTime<'" + M_Str_mindate + "'");
                }
                else if (ptype == 4)
                {
                    if (deltype == "bview")
                        new BCW.BLL.Forumlog().Delete("");
                    else if (deltype == "xview")
                        new BCW.BLL.Goldlog().Delete("");
                    else if (deltype == "gview")
                        new BCW.BLL.Transfer().Delete("Types<>3");
                    else if (deltype == "gview2")
                        new BCW.BLL.Transfer().Delete("Types=3");
                    else if (deltype == "vview")
                        new BCW.BLL.Payrmb().Delete("");
                }
                Utils.Success("清空成功", "清空操作成功..", Utils.getPage("forumlog.aspx?act=" + deltype + ""), "2");
            }
        }
    }
    #endregion

    #region 清空帖子日志 ClearTextPage
    private void ClearTextPage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId != 1 && ManageId != 11)
        {
            Utils.Error("只有系统管理员1号才可以进行此操作", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        string Title = new BCW.BLL.Text().GetTitle(id);
        if (Title == "")
        {
            Utils.Error("不存在的帖子记录", "");
        }
        if (info == "")
        {
            Master.Title = "清空帖子日志";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定清空《" + Title + "》的帖子日志记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?info=ok1&amp;act=cleartext&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定清空(含回帖日志)</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?info=ok2&amp;act=cleartext&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定清空二(不含回帖日志)</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=bview&amp;bid=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            //删除
            if (info == "ok1")
                new BCW.BLL.Forumlog().Delete("BID=" + id + "");
            else
                new BCW.BLL.Forumlog().Delete("BID=" + id + " and ReID=0");

            Utils.Success("清空帖子日志", "清空帖子日志成功..", Utils.getUrl("forumlog.aspx?act=bview&amp;bid=" + id + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
    }
    #endregion

    #region 消息日志重复查询 SearchGoldPage
    /// <summary>
    /// 消息日志重复查询
    /// </summary>
    /// <param name="act"></param>
    private void SearchGoldPage(string act)
    {
        Master.Title = "消息日志重复查询";
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        if (uid > 0)
            builder.Append("ID:" + uid + "");

        builder.Append("重复日志");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "uid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        if (uid > 0)
            strWhere += "UsID=" + uid + " and ";

        strWhere += "AddTime>'2013-06-25 18:30:00' ";


        // 开始读取列表
        IList<BCW.Model.Goldlog> listGoldlog = new BCW.BLL.Goldlog().GetGoldlogsCF(pageIndex, pageSize, strWhere, out recordCount);
        if (listGoldlog.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Goldlog n in listGoldlog)
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

                builder.AppendFormat("{0}.<a href=\"" + Utils.getUrl("uinfo.aspx?uid={1}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}</a>{3}|操作{4}({5})", (pageIndex - 1) * pageSize + k, n.UsId, "会员ID" + n.UsId, Out.SysUBB(n.AcText.Replace("/bbs/guess2/", "guess2/")), n.AcGold, DT.FormatDate(n.AddTime, 0));

                k++;
                builder.Append(Out.Tab("</div>", ""));

            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 1));

        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        string strText = "输入用户ID:/,";
        string strName = "uid,act";
        string strType = "num,hidden";
        string strValu = "'" + act + "";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "搜重复日志,forumlog.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    #endregion

    #region 充值统计 poolPage
    /// <summary>
    /// 充值统计
    /// </summary>
    private void poolPage()
    {
        Master.Title = "充值统计";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("充值统计");
        builder.Append(Out.Tab("</div>", "<br />"));

        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId != 1 && ManageId != 9 && ManageId != 26)
        {
            Utils.Error("权限不足", "");
        }
        //中介收益列表
        List<string> Bills = new List<string>();
        //系统收益列表
        List<string> SysBills = new List<string>();
        //总额
        double alls = 0;
        //费率
        double arges = 0;
        //余额
        double bals = 0;
        //已支付
        double isgives = 0;

        #region 循环统计中介的收益
        // 开始读取列表
        DataSet ds = new BCW.BLL.Payrmb().GetList("*", " (MerBillNo LIKE '%AGENC%') AND State=1 and Types=100");
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    //builder.Append(ds.Tables[0].Rows[i]["MerBillNo"].ToString());
                    string[] sArray = Regex.Split(ds.Tables[0].Rows[i]["MerBillNo"].ToString(), "AGENC", RegexOptions.IgnoreCase);
                    double dsmount = double.Parse(ds.Tables[0].Rows[i]["Amount"].ToString());
                    //扣取千分6的手续费
                    double arge = Math.Round(dsmount * 0.006, 2);
                    if (Bills.Count <= 0)
                    {
                        //USID+充值金额+手续费
                        Bills.Add(sArray[0] + "," + dsmount.ToString("0.00") + "," + arge.ToString());
                    }
                    else
                    {
                        for (int j = 0; j < Bills.Count; j++)
                        {
                            if (Bills[j].Split(',')[0] == sArray[0])
                            {
                                //累计合并
                                double amount = double.Parse(Bills[j].Split(',')[1]);
                                double p_arge = double.Parse(Bills[j].Split(',')[2]);
                                amount = amount + dsmount;
                                arge = p_arge + arge;
                                Bills[j] = sArray[0] + "," + amount.ToString("0.00") + "," + arge.ToString();
                                break;
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region 循环统计系统的收益
        // 开始读取列表
        DataSet ds1 = new BCW.BLL.Payrmb().GetList("*", " (MerBillNo not LIKE '%AGENC%') AND State=1 and Types=100");
        if (ds1.Tables.Count > 0)
        {
            if (ds1.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    double dsmount = double.Parse(ds1.Tables[0].Rows[i]["Amount"].ToString());
                    //扣取千分6的手续费
                    double arge = Math.Round(dsmount * 0.006, 2);
                    string ArName = "系统";
                    if (SysBills.Count <= 0)
                    {
                        //USID+充值金额+手续费
                        SysBills.Add(ArName + "," + dsmount.ToString("0.00") + "," + arge.ToString());
                    }
                    else
                    {
                        for (int j = 0; j < SysBills.Count; j++)
                        {
                            if (SysBills[j].Split(',')[0] == ArName)
                            {
                                //累计合并
                                double amount = double.Parse(SysBills[j].Split(',')[1]);
                                double p_arge = double.Parse(SysBills[j].Split(',')[2]);
                                amount = amount + dsmount;
                                arge = p_arge + arge;
                                SysBills[j] = ArName + "," + amount.ToString("0.00") + "," + arge.ToString();
                                break;
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region 显示收益到界面
        //计算系统的
        if (SysBills.Count > 0)
        {
            int k = 1;
            for (int i = 0; i < SysBills.Count; i++)
            {

                if (k == 1)
                    builder.Append(Out.Tab("<div>", ""));
                else
                    builder.Append(Out.Tab("<div>", "<br />"));
                string[] bls = SysBills[i].Split(',');
                double isgive = new BCW.IPSPay.BLL.tb_IPSGiveLog().GetTotal(0);
                double all = double.Parse(bls[1]);
                double arge = double.Parse(bls[2]);
                alls += all;
                arges += arge;
                isgives += isgive;
                bals += (all - isgive);
                builder.Append("系统充值<br />总额:" + all + " 扣费:" + arge + "<br />");
                builder.Append("已取:" + isgive + "<br />");
                builder.Append("余额:" + (all - isgive));
                k++;
                builder.Append(Out.Tab("</div>", "<br />"));
            }
        }
        //计算中介的
        if (Bills.Count > 0)
        {
            int k = 1;
            for (int i = 0; i < Bills.Count; i++)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", "<br />"));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }
                string[] bls = Bills[i].Split(',');
                double isgive = double.Parse(new BCW.BLL.User().GetUsISGive(int.Parse(bls[0])));
                double all = double.Parse(bls[1]);
                double arge = double.Parse(bls[2]);
                alls += all;
                arges += arge;
                isgives += isgive;
                bals += (all - isgive);
                builder.Append(k + "." + new BCW.BLL.User().GetUsName(int.Parse(bls[0])) + "(" + bls[0] + ")<br />总额:" + all + " 扣费:" + arge + "<br />");
                builder.Append("已付:" + isgive + " <a href=\"" + Utils.getUrl("forumlog.aspx?act=ShowPool&amp;p=0") + "\">详细</a><br />");
                builder.Append("余额:" + (all - isgive).ToString("0.00"));
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
        }

        #endregion

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("系统总金额:" + alls + "<br />系统总扣费" + arges + "<br />已支付:" + isgives + "  <a href=\"" + Utils.getUrl("forumlog.aspx?act=ShowPool&amp;p=0") + "\">详细</a><br />余额总量:" + bals);
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        if (ManageId == 1 || ManageId == 9 || ManageId == 26)
        {
            builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=addpool&amp;p=0") + "\">取款编辑</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=addlog&amp;p=0") + "\">新增支付</a><br />");
        }
        builder.Append("<a href=\"" + Utils.getPage("forumlog.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    #endregion

    #region 编辑支付金额 addpoolPage
    private void addpoolPage()
    {
        int GType = int.Parse(Utils.GetRequest("p", "get", 2, @"^[0-1]\d*$", "类别错误"));
        string nm = "";
        if (GType == 0) { nm = "网上"; }
        else if (GType == 1) { nm = "商城"; }
        Master.Title = nm + "充值统计";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(nm + "充值统计");
        builder.Append(Out.Tab("</div>", "<br />"));
        string info = Utils.GetRequest("info", "post", 1, "", "");
        if (info == "ok")
        {
            string bz = Utils.GetRequest("bz", "post", 2, "", "备注不能为空");
            double isgive = double.Parse(Utils.GetRequest("isgive", "post", 1, @"^\d+(.\d+)?$", "0"));
            int ManageID = new BCW.User.Manage().IsManageLogin();
            if (isgive <= 10)
            {
                Utils.Error("取款金额必须大于10以上", "");
            }
            BCW.IPSPay.Model.tb_IPSGiveLog model = new BCW.IPSPay.Model.tb_IPSGiveLog();
            model.ManageID = ManageID;                          //操作ID
            model.BzNote = bz;                                  //备注信息
            model.GetMoney = decimal.Parse(isgive.ToString());  //取款金额
            model.G_type = GType;                               //操作类型
            model.addtime = DateTime.Now;                       //操作时间
            model.G_arge = 10;                                  //每一笔手续费
            new BCW.IPSPay.BLL.tb_IPSGiveLog().Add(model);
            if (GType == 0)
            {
                Utils.Success("提交成功", "填写支付金额成功..", Utils.getPage("forumlog.aspx?act=pool"), "1");
            }
            if (GType == 1)
            {
                Utils.Success("提交成功", "填写支付金额成功..", Utils.getPage("forumlog.aspx?act=ShopPool"), "1");
            }
        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            string strText = "取款金额(大于10元，系统每次扣取10元手续费)/,备注/,,";
            string strName = "isgive,bz,act,info";
            string strType = "text,text,hidden,hidden,";
            string strValu = "''addpool'ok";
            string strEmpt = "true,true,false";
            string strIdea = "/";
            string strOthe = "提交数据," + Utils.getUrl("forumlog.aspx?p=" + GType) + ",post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=pool") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    #endregion

    #region 商城统计 ShopPoolPage 黄国军 20161015
    /// <summary>
    /// 商城统计
    /// </summary>
    private void ShopPoolPage()
    {
        Master.Title = "商城统计";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("商城统计");
        builder.Append(Out.Tab("</div>", "<br />"));

        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId != 1 & ManageId != 9 && ManageId != 26)
        {
            Utils.Error("权限不足", "");
        }

        //中介收益列表
        List<string> Bills = new List<string>();
        //系统收益列表
        List<string> SysBills = new List<string>();
        //总额
        double alls = 0;
        //费率
        double arges = 0;
        //余额
        double bals = 0;
        //已支付
        double isgives = 0;

        #region 循环统计收益
        // 开始读取列表
        DataSet ds = new BCW.BLL.Shopkeep().GetList("*", " (NodeId = 28) AND (State = 1)");
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    double dsmount = double.Parse(ds.Tables[0].Rows[i]["Amount"].ToString());
                    //扣取千分6的手续费
                    double arge = Math.Round(dsmount * 0.006, 2);
                    string ArName = "系统";
                    if (SysBills.Count <= 0)
                    {
                        //USID+充值金额+手续费
                        SysBills.Add(ArName + "," + dsmount.ToString("0.00") + "," + arge.ToString());
                    }
                    else
                    {
                        for (int j = 0; j < SysBills.Count; j++)
                        {
                            if (SysBills[j].Split(',')[0] == ArName)
                            {
                                //累计合并
                                double amount = double.Parse(SysBills[j].Split(',')[1]);
                                double p_arge = double.Parse(SysBills[j].Split(',')[2]);
                                amount = amount + dsmount;
                                arge = p_arge + arge;
                                SysBills[j] = ArName + "," + amount.ToString("0.00") + "," + arge.ToString();
                                break;
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region 显示收益到界面
        //计算系统的
        if (SysBills.Count > 0)
        {
            int k = 1;
            for (int i = 0; i < SysBills.Count; i++)
            {

                if (k == 1)
                    builder.Append(Out.Tab("<div>", ""));
                else
                    builder.Append(Out.Tab("<div>", "<br />"));
                string[] bls = SysBills[i].Split(',');
                double isgive = new BCW.IPSPay.BLL.tb_IPSGiveLog().GetTotal(1);
                double all = double.Parse(bls[1]);
                double arge = double.Parse(bls[2]);
                alls += all;
                arges += arge;
                isgives += isgive;
                bals += (all - isgive);
                builder.Append("系统充值<br />总额:" + all + " 扣费:" + arge + "<br />");
                builder.Append("已取:" + isgive + "<br />");
                builder.Append("余额:" + (all - isgive));
                k++;
                builder.Append(Out.Tab("</div>", "<br />"));
            }
        }

        #endregion

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("系统总金额:" + alls + "<br />系统总扣费" + arges + "<br />已支付:" + isgives + " <a href=\"" + Utils.getUrl("forumlog.aspx?act=ShowPool&amp;p=1") + "\">详细</a><br />余额总量:" + bals);
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        if (ManageId == 1 || ManageId == 9 || ManageId == 26)
        {
            builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=addpool&amp;p=1") + "\">支付编辑</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=addlog_Shop&amp;p=1") + "\">新增支付</a><br />");
        }
        builder.Append("<a href=\"" + Utils.getPage("forumlog.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    #endregion

    #region 显示取款详细 ShowPoolPage
    private void ShowPoolPage()
    {
        Master.Title = "取款详细";
        int ptype = int.Parse(Utils.GetRequest("p", "all", 1, @"^[0-1]$", "0"));
        int gid = int.Parse(Utils.GetRequest("gid", "all", 1, @"^[0-9]\d*$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("取款详细");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "ptype", "gid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        strWhere += "G_type =" + ptype + "";

        // 开始读取列表
        IList<BCW.IPSPay.Model.tb_IPSGiveLog> listGiveLog = new BCW.IPSPay.BLL.tb_IPSGiveLog().GetIPSGiveLogs(pageIndex, pageSize, strWhere, out recordCount);
        if (listGiveLog.Count > 0)
        {
            int k = 1;
            foreach (BCW.IPSPay.Model.tb_IPSGiveLog n in listGiveLog)
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
                builder.AppendFormat("{0}.管理员ID:{1} 备注:{2} <br />提款金额:{3} 手续费:{4} <br />操作时间:{5}", (pageIndex - 1) * pageSize + k, n.ManageID, n.BzNote, n.GetMoney, n.G_arge, DT.FormatDate(n.addtime, 1));

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
        if (ptype == 0)
            builder.Append("<a href=\"" + Utils.getPage("forumlog.aspx?act=pool") + "\">返回上一级</a><br />");
        else
            builder.Append("<a href=\"" + Utils.getPage("forumlog.aspx?act=ShopPool") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    #endregion

    #region 新增记录 addlogPage
    private void addlogPage()
    {
        int GType = int.Parse(Utils.GetRequest("p", "get", 2, @"^[0-1]\d*$", "类别错误"));
        string nm = "";
        if (GType == 0) { nm = "网上"; }
        else if (GType == 1) { nm = "商城"; }
        Master.Title = nm + "充值统计";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(nm + "新增记录");
        builder.Append(Out.Tab("</div>", "<br />"));
        string info = Utils.GetRequest("info", "post", 1, "", "");
        if (info == "ok")
        {
            string UsiD = Utils.GetRequest("Usid", "post", 2, "", "充值ID不能为空");
            string BillNo = Utils.GetRequest("BillNo", "post", 2, "", "订单号不能为空");
            string Billtime = Utils.GetRequest("Billtime", "post", 2, "", "订单号时间不能为空");
            double isgive = double.Parse(Utils.GetRequest("isgive", "post", 1, @"^\d+(.\d+)?$", "0"));
            int bank = int.Parse(Utils.GetRequest("bank", "post", 1, @"^\d+(.\d+)?$", "0"));
            int statue = int.Parse(Utils.GetRequest("statue", "post", 1, @"^\d+(.\d+)?$", "0"));
            int ManageID = new BCW.User.Manage().IsManageLogin();

            BCW.Model.Payrmb rmb = new BCW.Model.Payrmb();
            rmb.UsName = new BCW.BLL.User().GetUsName(int.Parse(UsiD));
            rmb.MerBillNo = BillNo;                               //商户订单号 必填字母及数字
            rmb.State = 0;                                        //未处理
            rmb.Types = 100;                                      //100标记
            rmb.Amount = decimal.Parse(isgive.ToString());        //充值金额 2位小数
            rmb.GatewayType = "156";                                 //默认未支付为空
            rmb.Attach = "";                                      //默认未支付为空
            rmb.BillEXP = 3;                                      //默认3小时自动取消订单
            rmb.GoodsName = ub.Get("SiteBz");                     //商品名称            
            rmb.IsCredit = "1";                                   //直连选项 1
            rmb.BankCode = bank.ToString();                         //IPS唯一标识指定的银行编号 00018
            rmb.ProductType = "1";                               //产品类型 -1未填 1个人银行 2企业银行
            rmb.AddUsIP = Utils.GetUsIP();                        //提交IP
            rmb.AddTime = DateTime.Now;                           //提交时间
            rmb.CardAmt = 0;
            rmb.CardNum = "";
            rmb.CardPwd = "";
            rmb.CardOrder = "";
            int MsgId = 0;
            MsgId = new BCW.BLL.Payrmb().Add(rmb);

            if (MsgId >= 0)
            {
                BCW.Model.Payrmb model = new BCW.BLL.Payrmb().GetPayrmb(MsgId);
                model.GatewayType = "156";
                model.Attach = "";
                model.BankCode = bank.ToString();
                model.ProductType = "1";
                model.State = statue;
                new BCW.BLL.Payrmb().Update_ips(model);
                if (GType == 0)
                {
                    Utils.Success("提交成功", "新增订单完成", Utils.getPage("forumlog.aspx?act=vview"), "1");
                }
            }
        }
        else
        {
            string BankCodestr = "";
            for (int i = 0; i < BCW.IPSPay.IPSPayMent.BankCodes.Length; i++)
            {
                if (BankCodestr != "") { BankCodestr += "|"; }
                BankCodestr += BCW.IPSPay.IPSPayMent.BankCodes[i] + "|" + BCW.IPSPay.IPSPayMent.BankNames[i];
            }

            builder.Append(Out.Tab("<div>", ""));
            string strText = "充值ID/,随机订单号/,充值金额/,时间(格式:1990-01-01 23:59:59)/,银行/,订单状态/,,";
            string strName = "Usid,BillNo,isgive,Billtime,bank,statue,act,info";
            string strType = "text,text,text,text,select,select,hidden,hidden,";
            string strValu = "'" + DT.getDateTimeNum() + "''" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'0'0'addlog'ok";
            string strEmpt = "true,true,true,true," + BankCodestr + ",0|待支付|1|成功|2|失效,false";
            string strIdea = "/";
            string strOthe = "提交数据," + Utils.getUrl("forumlog.aspx?p=" + GType) + ",post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=pool") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    #endregion

    #region 新增商城记录 addlog_ShopPage
    private void addlog_ShopPage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId == 1 & ManageId != 9 && ManageId != 26)
        {
            Utils.Error("权限不足", "");
        }
        int GType = int.Parse(Utils.GetRequest("p", "get", 2, @"^[0-1]\d*$", "类别错误"));
        string nm = "";
        if (GType == 0) { nm = "网上"; }
        else if (GType == 1) { nm = "商城"; }
        Master.Title = nm + "充值统计";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(nm + "新增记录");
        builder.Append(Out.Tab("</div>", "<br />"));
        string info = Utils.GetRequest("info", "post", 1, "", "");
        if (info == "ok")
        {
            string keepid = Utils.GetRequest("keepid", "post", 2, "", "商品ID不能为空");
            string UsiD = Utils.GetRequest("Usid", "post", 2, "", "充值ID不能为空");
            string BillNo = Utils.GetRequest("BillNo", "post", 2, "", "订单号不能为空");
            string Billtime = Utils.GetRequest("Billtime", "post", 2, "", "订单号时间不能为空");
            double isgive = double.Parse(Utils.GetRequest("isgive", "post", 1, @"^\d+(.\d+)?$", "0"));
            int bank = int.Parse(Utils.GetRequest("bank", "post", 1, @"^\d+(.\d+)?$", "0"));
            int statue = int.Parse(Utils.GetRequest("statue", "post", 1, @"^\d+(.\d+)?$", "0"));
            int ManageID = new BCW.User.Manage().IsManageLogin();
            int id = int.Parse(keepid);
            BCW.Model.Shopkeep keep = new BCW.Model.Shopkeep();
            BCW.Model.Shopgift model = new BCW.BLL.Shopgift().GetShopgift(id);
            if (model == null)
            {
                Utils.Error("不存在的商品记录", "");
            }

            //操作币
            long AcPrice = Convert.ToInt64(model.Price * int.Parse(isgive.ToString()));

            keep.GiftId = id;
            keep.Title = model.Title.Trim();
            keep.Pic = model.Pic;
            keep.PrevPic = model.PrevPic;
            keep.Notes = model.Notes;
            keep.IsSex = model.IsSex;
            keep.Para = model.Para;
            keep.UsID = int.Parse(UsiD);
            keep.UsName = new BCW.BLL.User().GetUsName(int.Parse(UsiD));
            keep.Total = int.Parse(isgive.ToString());
            keep.TopTotal = int.Parse(isgive.ToString());
            keep.AddTime = DateTime.Now;

            //产品分类ID28为在线支付商品
            keep.NodeId = model.NodeId;
            keep.MerBillNo = "Shop" + DT.getDateTimeNum();              //商户订单号 必填字母及数字
            keep.State = 0;                                             //未处理
            keep.Amount = decimal.Parse(AcPrice.ToString("0.00"));      //充值金额 2位小数
            keep.GatewayType = "";                                      //默认未支付为空
            keep.Attach = "";                                           //默认未支付为空
            keep.BillEXP = 3;                                           //默认3小时自动取消订单
            keep.GoodsName = model.Title.Trim();                        //商品名称            
            keep.IsCredit = "1";                                        //直连选项 1
            keep.BankCode = bank.ToString();                                         //IPS唯一标识指定的银行编号 00018
            keep.ProductType = "1";                                    //产品类型 -1未填 1个人银行 2企业银行
            keep.AddTime = DateTime.Now;
            keep.State = statue;
            int KeepId = new BCW.BLL.Shopkeep().Add(keep);

            if (KeepId >= 0)
            {
                Utils.Success("提交成功", "新增订单完成", Utils.getPage("forumlog.aspx?act=BuyHistory"), "1");
            }
        }
        else
        {
            string BankCodestr = "";
            for (int i = 0; i < BCW.IPSPay.IPSPayMent.BankCodes.Length; i++)
            {
                if (BankCodestr != "") { BankCodestr += "|"; }
                BankCodestr += BCW.IPSPay.IPSPayMent.BankCodes[i] + "|" + BCW.IPSPay.IPSPayMent.BankNames[i];
            }

            builder.Append(Out.Tab("<div>", ""));
            string strText = "商品ID/,充值ID/,随机订单号/,商品购买个数/,时间(格式:1990-01-01 23:59:59)/,银行/,订单状态/,,";
            string strName = "keepid,Usid,BillNo,isgive,Billtime,bank,statue,act,info";
            string strType = "num,num,text,text,text,select,select,hidden,hidden,";
            string strValu = "609''" + DT.getDateTimeNum() + "''" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'0'0'addlog_Shop'ok";
            string strEmpt = "true,true,true,true,true," + BankCodestr + ",0|待支付|1|成功|2|失效,false";
            string strIdea = "/";
            string strOthe = "提交数据," + Utils.getUrl("forumlog.aspx?p=" + GType) + ",post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=pool") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    #endregion
}