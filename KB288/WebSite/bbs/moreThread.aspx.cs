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
/// <summary>
/// 修改查询条件：推荐 64行
/// </summary>
public partial class bbs_moreThread : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[0-9]\d*$", "0"));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        if (uid == 0)
        {
            uid = meid;
        }
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]\d*$", "1"));
        int showtype = int.Parse(Utils.GetRequest("showtype", "all", 1, @"^[0-9]\d*$", "0"));
        int ordertype = int.Parse(Utils.GetRequest("ordertype", "all", 1, @"^[0-9]\d*$", "1"));
        if (ptype == 1)
        {
        
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + uid + "") + "\">Ta的空间</a>&gt;帖子");
            builder.Append(Out.Tab("</div>", "<br />"));
            Master.Title = "查看主题帖子";

            //string strText = "排序：,,,,";
            //string strName = "ordertype,ptype,uid,showtype,backurl";
            //string strType = "select,hidden,hidden,hidden,hidden";
            //string strValu = "" + ordertype + "'" + ptype + "'" + uid + "'" + showtype + "'" + Utils.getPage(0) + "";
            //string strEmpt = "1|按发帖时间|2|按最后跟贴|3|按帖子浏览量|4|按帖子跟贴量,,,,";
            //string strIdea = "";
            //string strOthe = "确定,moreThread.aspx,post,3,red";
            //builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string strWhere = string.Empty;
            string strOrder = string.Empty;
            string[] pageValUrl = { "uid", "ptype", "showtype", "ordertype", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            //查询条件
            strWhere = "UsID=" + uid + "";
            if (showtype == 1)
                strWhere += " and IsGood=1";
            else if (showtype == 2)
                strWhere += " and IsRecom=1";

            strWhere += " and IsDel=0";

            //if (ordertype == 1)
            strOrder = "AddTime Desc";
            //else if (ordertype == 2)
            //    strOrder = "ReTime Desc";
            //else if (ordertype == 3)
            //    strOrder = "ReadNum Desc";
            //else if (ordertype == 4)
            //    strOrder = "ReplyNum Desc";

            // 开始读取列表
            IList<BCW.Model.Text> listText = new BCW.BLL.Text().GetTextsMe(pageIndex, pageSize, strWhere, strOrder, out recordCount);
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
                    builder.AppendFormat("<a href=\"" + Utils.getUrl("topic.aspx?forumid=" + n.ForumId + "&amp;bid={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{1}.{2}</a>{3}", n.ID, (pageIndex - 1) * pageSize + k, n.Title, DT.FormatDate(n.AddTime, 2));

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
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            if (showtype != 0)
                builder.Append("<a href=\"" + Utils.getUrl("moreThread.aspx?ptype=" + ptype + "&amp;uid=" + uid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">全部帖</a> ");
            if (showtype != 1)
                builder.Append("<a href=\"" + Utils.getUrl("moreThread.aspx?ptype=" + ptype + "&amp;uid=" + uid + "&amp;showtype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">精华帖</a> ");
            if (showtype != 2)
                builder.Append("<a href=\"" + Utils.getUrl("moreThread.aspx?ptype=" + ptype + "&amp;uid=" + uid + "&amp;showtype=2&amp;backurl=" + Utils.getPage(0) + "") + "\">推荐帖</a>");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx?uid=" + uid + "") + "\">上级</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("moreThread.aspx?ptype=2&amp;uid=" + uid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">回帖</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else if (ptype == 2)
        {
            Master.Title = "查看回帖";
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string strWhere = string.Empty;
            string strOrder = string.Empty;
            string[] pageValUrl = { "uid", "ptype", "forumid", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + uid + "") + "\">Ta的空间</a>&gt;回帖");
            builder.Append(Out.Tab("</div>", "<br />"));

            //查询条件
            strWhere = "UsID=" + uid + "";
            if (meid != uid)
                strWhere += " and ForumId<>88 and  ForumId<>100 and  ForumId<>14 and  ForumId<>99 and ForumId<>77";


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

                    builder.AppendFormat("<a href=\"" + Utils.getUrl("reply.aspx?act=view&amp;forumid=" + n.ForumId + "&amp;bid=" + n.Bid + "&amp;reid=" + n.Floor + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">{1}.{2}</a>{3}", n.ID, (pageIndex - 1) * pageSize + k, Out.USB(TrueStrLength.cutTrueLength(n.Content, 20, "…")), DT.FormatDate(n.AddTime, 2));

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
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("moreThread.aspx?ptype=3&amp;uid=" + uid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">按论坛查看回帖&gt;&gt;</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx?uid=" + uid + "") + "\">上级</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("moreThread.aspx?ptype=2&amp;uid=" + uid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">回帖</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else if (ptype == 3)
        {
            Master.Title = "查看回帖";
            int pageIndex;
            int recordCount;
            int pageSize = 4;
            string strWhere = "IsActive=0";
            string strOrder = string.Empty;
            string[] pageValUrl = { "uid", "ptype", "forumid", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            int forumid = int.Parse(Utils.GetRequest("forumid", "get", 1, @"^[0-9]\d*$", "0"));
            if (forumid == 0)
            {
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + uid + "") + "\">Ta的空间</a>&gt;回帖");
                builder.Append(Out.Tab("</div>", "<br />"));

                // 开始读取论坛
                IList<BCW.Model.Forum> listForum = new BCW.BLL.Forum().GetForums(pageIndex, pageSize, strWhere, out recordCount);
                if (listForum.Count > 0)
                {
                    int k = 1;
                    foreach (BCW.Model.Forum n in listForum)
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
                        builder.AppendFormat("<a href=\"" + Utils.getUrl("moreThread.aspx?ptype=" + ptype + "&amp;uid=" + uid + "&amp;forumid={0}&amp;backurl=" + Utils.getPage(0) + "") + "\">[{1}]</a>", n.ID, n.Title);
                        builder.Append("<br />参与" + new BCW.BLL.Reply().GetCount(uid, n.ID) + "回帖");
                        k++;
                        builder.Append(Out.Tab("</div>", ""));

                    }

                    // 分页
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                }
                else
                {
                    builder.Append(Out.Div("text", "没有相关记录"));
                }
                builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
                builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
                builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx?uid=" + uid + "") + "\">上级</a>-");
                builder.Append("<a href=\"" + Utils.getUrl("moreThread.aspx?ptype=1&amp;uid=" + uid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">帖子</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
                string BbsName = new BCW.BLL.Forum().GetTitle(forumid);
                if (BbsName == "")
                {
                    Utils.Error("不存在的论坛", "");
                }
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + uid + "") + "\">Ta的空间</a>&gt;" + BbsName + "回帖");
                builder.Append(Out.Tab("</div>", "<br />"));

                //查询条件
                strWhere = "UsID=" + uid + " and forumid=" + forumid + "";
                if (meid != uid)
                    strWhere += " and ForumId<>88 and  ForumId<>100 and  ForumId<>14 and  ForumId<>99 and ForumId<>77";

                if (showtype == 1)
                    strWhere += " and IsGood=1";

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
                builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("moreThread.aspx?ptype=" + ptype + "&amp;uid=" + uid + "&amp;forumid=" + forumid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">全部回帖</a> ");
                builder.Append("<a href=\"" + Utils.getUrl("moreThread.aspx?ptype=" + ptype + "&amp;uid=" + uid + "&amp;forumid=" + forumid + "&amp;showtype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">精华回帖</a> ");
                builder.Append(Out.Tab("</div>", ""));
                builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
                builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
                builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx?uid=" + uid + "") + "\">上级</a>-");
                builder.Append("<a href=\"" + Utils.getUrl("moreThread.aspx?ptype=2&amp;uid=" + uid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">回帖</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
    }
}