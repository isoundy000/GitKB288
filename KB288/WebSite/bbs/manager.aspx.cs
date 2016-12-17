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

public partial class bbs_manager : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "admin":
                AdminPage();
                break;
            case "admintext":
                AdminTextPage();
                break;
            case "uidtext":
                UIDTextPage();
                break;
            case "flow":
                FlowPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        Master.Title = "管理团队";
        builder.Append(Out.Tab("<div class=\"title\">管理团队</div>", ""));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("manager.aspx?act=admin&amp;ptype=1") + "\">系统管理员</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("manager.aspx?act=admin&amp;ptype=2") + "\">论坛总版</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("manager.aspx?act=admin&amp;ptype=3") + "\">论坛版主</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("manager.aspx?act=admin&amp;ptype=4") + "\">聊吧总管理</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("manager.aspx?act=admin&amp;ptype=5") + "\">闲聊总管理</a>");

        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("manager.aspx?act=flow") + "\">社区滚动列表</a>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("manager.aspx?act=admintext") + "\">管理员发帖回帖</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("manager.aspx?act=uidtext") + "\">统计会员发帖回帖</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("blacklist.aspx") + "\">查看社区监狱&gt;&gt;</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx") + "\">空间</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void AdminPage()
    {
        Master.Title = "管理团队";
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-5]$", "1"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        if (ptype == 1)
            builder.Append("系统管理员");
        else if (ptype == 2)
            builder.Append("论坛总版");
        else if (ptype == 3)
            builder.Append("论坛版主");
        else if (ptype == 4)
            builder.Append("聊吧总管理");
        else if (ptype == 5)
            builder.Append("闲聊总管理");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "ptype" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        if (ptype == 1)
            strWhere += "ForumID=-1 and ";
        else if (ptype == 2)
            strWhere += "ForumID=0 and ";
        else if (ptype == 3)
            strWhere += "ForumID>0 and ";
        else if (ptype == 4)
            strWhere += "Rolece collate Chinese_PRC_CS_AS_WS like '%E%' and ";
        else if (ptype == 5)
            strWhere += "Rolece collate Chinese_PRC_CS_AS_WS like '%N%' and ";

        strWhere += "(OverTime>='" + DateTime.Now + "' OR OverTime='1990-1-1 00:00:00') and Status=0";


        // 开始读取列表
        IList<BCW.Model.Role> listRole = new BCW.BLL.Role().GetRoles(pageIndex, pageSize, strWhere, out recordCount);
        if (listRole.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Role n in listRole)
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

                string sInclude = string.Empty;
                if (n.Include == 1)
                    sInclude = "(含下级版块)";

                builder.AppendFormat("<a href=\"" + Utils.getUrl("uinfo.aspx?uid={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{1}</a>/<a href=\"" + Utils.getUrl("usermanage.aspx?act=role&amp;id=" + n.ID + "&amp;hid={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}</a>", n.UsID, BCW.User.Users.SetUser(n.UsID, 3), n.RoleName);

                if (n.ForumID == 0)
                    builder.Append("/管辖:全区版块");
                else if (n.ForumID > 0)
                    builder.Append("/管辖:<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + n.ForumID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.ForumName + "" + sInclude + "</a>");

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
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("manager.aspx") + "\">管理</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void AdminTextPage()
    {
        Master.Title = "发帖回帖清单";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("各管理发回帖清单");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = 5;
        string strWhere = "";
        string[] pageValUrl = { "act" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件

        strWhere += "(OverTime>='" + DateTime.Now + "' OR OverTime='1990-1-1 00:00:00') and Status=0";

        // 开始读取列表
        IList<BCW.Model.Role> listRole = new BCW.BLL.Role().GetRoles(pageIndex, pageSize, strWhere, out recordCount);
        if (listRole.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Role n in listRole)
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

                builder.AppendFormat("<a href=\"" + Utils.getUrl("uinfo.aspx?uid={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{1}(" + n.UsID + ")</a>", n.UsID, BCW.User.Users.SetUser(n.UsID, 3));

                builder.Append("<br />本月发帖：" + new BCW.BLL.Forumstat().GetMonthCount(n.UsID, 1) + "");
                builder.Append("<br />本月回帖：" + new BCW.BLL.Forumstat().GetMonthCount(n.UsID, 2) + "");

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
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("manager.aspx") + "\">管理</a>");
        builder.Append(Out.Tab("</div>", ""));
    }


    private void UIDTextPage()
    {
        Master.Title = "会员发回帖统计";
        int uid = int.Parse(Utils.GetRequest("uid", "post", 1, @"^[1-9]\d*$", "0"));

        if (uid > 0)
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("统计对象:<a href=\"" + Utils.getUrl("/default.aspx") + "\">" + BCW.User.Users.SetUser(uid) + "</a>");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("今日帖子数量:" + new BCW.BLL.Forumstat().GetCount(uid, 0, 1, 1) + "<br />");
            builder.Append("今日回帖数量:" + new BCW.BLL.Forumstat().GetCount(uid, 0, 2, 1) + "");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("昨日帖子数量:" + new BCW.BLL.Forumstat().GetCount(uid, 0, 1, 2) + "<br />");
            builder.Append("昨日回帖数量:" + new BCW.BLL.Forumstat().GetCount(uid, 0, 2, 2) + "");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("本周帖子数量:" + new BCW.BLL.Forumstat().GetCount(uid, 0, 1, 3) + "<br />");
            builder.Append("本周回帖数量:" + new BCW.BLL.Forumstat().GetCount(uid, 0, 2, 3) + "");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("本月帖子数量:" + new BCW.BLL.Forumstat().GetCount(uid, 0, 1, 4) + "<br />");
            builder.Append("本月回帖数量:" + new BCW.BLL.Forumstat().GetCount(uid, 0, 2, 4) + "");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("上月帖子数量:" + new BCW.BLL.Forumstat().GetCount(uid, 0, 1, 5) + "<br />");
            builder.Append("上月回帖数量:" + new BCW.BLL.Forumstat().GetCount(uid, 0, 2, 5) + "");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("帖子总数量:" + new BCW.BLL.Forumstat().GetCount(uid, 0, 1, 0) + "<br />");
            builder.Append("回帖总数量:" + new BCW.BLL.Forumstat().GetCount(uid, 0, 2, 0) + "");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("manager.aspx?act=uidtext") + "\">&lt;&lt;继续搜索</a>");
            builder.Append(Out.Tab("</div>", ""));

        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("会员发回帖统计");
            builder.Append(Out.Tab("</div>", ""));
            string strText = "输入会员ID:/,,,,";
            string strName = "uid,act,backurl";
            string strType = "num,hidden,hidden";
            string strValu = "'uidtext'" + Utils.PostPage(1) + "";
            string strEmpt = "false,false,false";
            string strIdea = "/";
            string strOthe = "确定查询,manager.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("manager.aspx") + "\">管理</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void FlowPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        int forumid = int.Parse(Utils.GetRequest("forumid", "get", 1, @"^[1-9]\d*$", "0"));
        int bid = int.Parse(Utils.GetRequest("bid", "get", 1, @"^[1-9]\d*$", "0"));

        Master.Title = "社区滚动列表";
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-1]$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("社区滚动列表");
        builder.Append(Out.Tab("</div>", "<br />"));
        bool IsFlow = IsGdID(meid);


        if (bid > 0)
        {
            BCW.Model.Text flow = new BCW.BLL.Text().GetText(bid);
            if (flow != null)
            {
                if (flow.IsFlow == 2 && flow.ForumId == forumid)
                {
                    builder.Append(Out.Tab("<div class=\"text\">", ""));
                    builder.Append("<a href=\"" + Utils.getUrl("topic.aspx?forumid=" + flow.ForumId + "&amp;bid=" + bid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + Out.TitleUBB(flow.Title) + "</a>");
                    if (IsFlow)
                    {
                        builder.Append("[<a href=\"" + Utils.getUrl("textmanage.aspx?act=delflow&amp;forumid=" + flow.ForumId + "&amp;bid=" + bid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">撤</a>]");
                    }
                    builder.Append(Out.Tab("</div>", Out.Hr()));
                }
            }
        }
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string strOrder = "";
        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        if (ptype == 1 && IsFlow == true)
            strWhere = "IsFlow=1";
        else
            strWhere = "IsFlow=2 and FlowTime>'" + DateTime.Now + "'";

        strOrder = "AddTime Desc";

        // 开始读取列表
        IList<BCW.Model.Text> listText = new BCW.BLL.Text().GetTexts(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listText.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Text n in listText)
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
                builder.Append("<a href=\"" + Utils.getUrl("topic.aspx?forumid=" + n.ForumId + "&amp;bid=" + n.ID + "") + "&amp;backurl=" + Utils.PostPage(1) + "\">" + ((pageIndex - 1) * pageSize + k) + "." + n.Title + "</a>");
                if (ptype == 1 && IsFlow == true)
                {
                    builder.Append("[<a href=\"" + Utils.getUrl("textmanage.aspx?act=flowset&amp;forumid=" + n.ForumId + "&amp;bid=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">设</a>]");
                }
                else
                {
                    if (IsFlow)
                    {
                        builder.Append("[<a href=\"" + Utils.getUrl("textmanage.aspx?act=delflow&amp;forumid=" + n.ForumId + "&amp;bid=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">撤</a>]");
                    }
                }
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            if (ptype == 1 && IsFlow == true)
                builder.Append(Out.Div("div", "没有任何版块滚动可以选择.."));
            else
                builder.Append(Out.Div("div", "没有相关记录.."));
        }
        if (IsFlow)
        {
            builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
            if (ptype == 1 && IsFlow == true)
                builder.Append("<a href=\"" + Utils.getUrl("manager.aspx?act=flow") + "\">&gt;&gt;返回社区滚动</a>");
            else
                builder.Append("<a href=\"" + Utils.getUrl("manager.aspx?act=flow&amp;ptype=1") + "\">设置社区滚动&gt;&gt;</a>");

            builder.Append(Out.Tab("</div>", ""));

        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("manager.aspx") + "\">管理</a>");
        builder.Append(Out.Tab("</div>", ""));

    }

    /// <summary>
    /// 滚动管理员ID
    /// </summary>
    private bool IsGdID(int meid)
    {
        bool Isvi = false;
        //能够穿透的ID
        string GdID = "#" + ub.GetSub("FreshGdID", "/Controls/fresh.xml") + "#";
        if (GdID.IndexOf("#" + meid + "#") != -1)
        {
            Isvi = true;
        }

        return Isvi;
    }
}