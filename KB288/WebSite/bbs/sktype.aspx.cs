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

public partial class bbs_sktype : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        int forumid = int.Parse(Utils.GetRequest("forumid", "get", 1, @"^[0-9]\d*$", "0"));
        if (forumid > 0)
        {
            if (!new BCW.BLL.Forum().Exists2(forumid))
            {
                Utils.Success("访问论坛", "该论坛不存在或已暂停使用", Utils.getUrl("forum.aspx"), "1");
            }
        }
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "reply":
                ReplyPage(act, forumid, meid);
                break;
            default:
                ReloadPage(act, forumid, meid);
                break;
        }
    }
    private void ReloadPage(string act, int forumid, int uid)
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-5]\d*$", "1"));
        if (forumid > 0)
        {
            Master.Title = "查看模式";
            builder.Append(Out.Tab("<div class=\"title\">查看模式</div>", ""));
        }
        else
        {
            Master.Title = "查看帖子";
            builder.Append(Out.Tab("<div class=\"title\">查看帖子</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"text\">", ""));

        if (forumid > 0)
        {
            if (act != "text")
            {
                if (ptype == 1)
                    builder.Append("精华|");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("sktype.aspx?forumid=" + forumid + "&amp;ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">精华</a>|");

                if (ptype == 2)
                    builder.Append("推荐|");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("sktype.aspx?forumid=" + forumid + "&amp;ptype=2&amp;backurl=" + Utils.getPage(0) + "") + "\">推荐</a>|");

                if (ptype == 3)
                    builder.Append("新帖|");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("sktype.aspx?forumid=" + forumid + "&amp;ptype=3&amp;backurl=" + Utils.getPage(0) + "") + "\">新帖</a>|");

                if (ptype == 4)
                    builder.Append("锁定|");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("sktype.aspx?forumid=" + forumid + "&amp;ptype=4&amp;backurl=" + Utils.getPage(0) + "") + "\">锁定</a>|");

                if (ptype == 5)
                    builder.Append("固底");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("sktype.aspx?forumid=" + forumid + "&amp;ptype=5&amp;backurl=" + Utils.getPage(0) + "") + "\">固底</a>");
            }
            else
            {
                builder.Append("本版发贴|<a href=\"" + Utils.getUrl("sktype.aspx?act=reply&amp;forumid=" + forumid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">本版回帖</a>");
            }
        }
        else
        {
            if (ptype == 1 || ptype == 5)
                builder.Append("精华|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("sktype.aspx?ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">精华</a>|");

            if (ptype == 2)
                builder.Append("推荐|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("sktype.aspx?ptype=2&amp;backurl=" + Utils.getPage(0) + "") + "\">推荐</a>|");

            if (ptype == 3)
                builder.Append("新帖|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("sktype.aspx?ptype=3&amp;backurl=" + Utils.getPage(0) + "") + "\">新帖</a>|");

            if (ptype == 4)
                builder.Append("热门");
            else
                builder.Append("<a href=\"" + Utils.getUrl("sktype.aspx?ptype=4&amp;backurl=" + Utils.getPage(0) + "") + "\">热门</a>");

        }
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        string strOrder = string.Empty;
        string[] pageValUrl = { "act", "forumid", "tsid", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        strWhere = "IsDel=0 and HideType<>9";
        //查询条件
        if (forumid > 0)
        {
            if (act == "text")
                strWhere += " and UsID=" + uid + "";

            strWhere += " and ForumId=" + forumid + "";
        }
        if (act != "text")
        {
            strWhere += " and ";
            if (ptype == 1)
                strWhere += "IsGood=1";
            else if (ptype == 2)
                strWhere += "IsRecom=1";
            if (ptype == 3)
                strWhere += "AddTime>='" + DateTime.Now.AddDays(-2) + "'";
            else if (ptype == 4)
                strWhere += "IsLock=1";
            else if (ptype == 5)
                strWhere += "IsTop=-1";
        }
        //排序条件
        strOrder = "ID Desc";

        if (forumid == 0)
        {
            if (ptype == 4 || ptype == 5)
                strWhere = "";
            if (ptype == 4)
                strOrder = "ReadNum Desc";
        }


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

                builder.Append(BCW.User.AppCase.CaseIsTop(n.IsTop));
                builder.Append(BCW.User.AppCase.CaseIsGood(n.IsGood));
                builder.Append(BCW.User.AppCase.CaseIsRecom(n.IsRecom));
                builder.Append(BCW.User.AppCase.CaseIsLock(n.IsLock));
                builder.Append(BCW.User.AppCase.CaseIsOver(n.IsOver));
                builder.Append(BCW.User.AppCase.CaseText(n.Types));

                builder.AppendFormat("<a href=\"" + Utils.getUrl("/bbs/topic.aspx?forumid=" + n.ForumId + "&amp;bid={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{1}.{2}</a><br />", n.ID, (pageIndex - 1) * pageSize + k, n.Title);
                builder.Append("" + n.UsName);
                builder.AppendFormat("/阅{0}/回<a href=\"" + Utils.getUrl("/bbs/reply.aspx?forumid=" + n.ForumId + "&amp;bid={1}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}</a>", n.ReadNum, n.ID, n.ReplyNum);

                k++;
                builder.Append(Out.Tab("</div>", ""));
            }

            // 分页
            builder.Append(BasePage.ForumMultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }

        builder.Append(Out.Tab("<div>", Out.Hr()));
        if (forumid > 0)
        {
            builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "") + "\">返回" + new BCW.BLL.Forum().GetTitle(forumid) + "</a><br />");
        }
        builder.Append("<a href=\"" + Utils.getUrl("moreThread.aspx?ptype=1&amp;backurl=" + Utils.PostPage(1) + "") + "\">我的帖子</a>-");
        builder.Append("<a href=\"" + Utils.getPage("moreThread.aspx?ptype=2&amp;backurl=" + Utils.PostPage(1) + "") + "\">回帖</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("forumstat.aspx?act=top&amp;forumid=" + forumid + "") + "\">排行榜</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ReplyPage(string act, int forumid, int uid)
    {
        Master.Title = "查看模式";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("sktype.aspx?act=text&amp;forumid=" + forumid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">本版发贴</a>|本版回帖");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        string strOrder = string.Empty;
        string[] pageValUrl = { "act", "forumid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        strWhere = "UsID=" + uid + " and forumid=" + forumid + "";

        strWhere += " and IsDel=0";

        strOrder = "ID Desc";

        // 开始读取列表
        IList<BCW.Model.Reply> listReply = new BCW.BLL.Reply().GetReplysMe(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listReply.Count > 0)
        {

            int k = 1;
            foreach (BCW.Model.Reply n in listReply)
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

                builder.AppendFormat("<a href=\"" + Utils.getUrl("reply.aspx?act=view&amp;forumid=" + forumid + "&amp;bid=" + n.Bid + "&amp;reid=" + n.Floor + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">{1}.{2}</a>{3}", n.ID, (pageIndex - 1) * pageSize + k, n.Content, DT.FormatDate(n.AddTime, 2));

                k++;
                builder.Append(Out.Tab("</div>", ""));

            }

            // 分页
            builder.Append(BasePage.ForumMultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "") + "\">" + new BCW.BLL.Forum().GetTitle(forumid) + "</a>");
        builder.Append(Out.Tab("</div>", ""));

    }
}



