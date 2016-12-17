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

public partial class bbs_blacklist : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        ReloadPage();
    }
    private void ReloadPage()
    {
        Master.Title = "社区监狱";
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-4]$", "1"));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("社区监狱");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));

        if (ptype == 1)
            builder.Append("全局黑名单|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("blacklist.aspx?uid=" + uid + "&amp;ptype=1") + "\">全局黑名单</a>|");

        if (ptype == 2)
            builder.Append("版块|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("blacklist.aspx?uid=" + uid + "&amp;ptype=2") + "\">版块</a>|");

        if (ptype == 3)
            builder.Append("聊室|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("blacklist.aspx?uid=" + uid + "&amp;ptype=3") + "\">聊室</a>|");

        if (ptype == 4)
            builder.Append("闲聊");
        else
            builder.Append("<a href=\"" + Utils.getUrl("blacklist.aspx?uid=" + uid + "&amp;ptype=4") + "\">闲聊</a>");

        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "ptype", "uid" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        if (uid > 0)
            strWhere = "usid=" + uid + " and ";

        if (ptype == 1)
            strWhere += " ForumId=0 and ";
        else if (ptype == 2)
            strWhere += " ForumId>0 and ";

       
        // 开始读取列表
        if (ptype == 1 || ptype == 2)
        {
            strWhere += "(ExitTime>='" + DateTime.Now + "' OR BlackDay=0)";

            IList<BCW.Model.Blacklist> listBlacklist = new BCW.BLL.Blacklist().GetBlacklists(pageIndex, pageSize, strWhere, out recordCount);
            if (listBlacklist.Count > 0)
            {
                int k = 1;
                foreach (BCW.Model.Blacklist n in listBlacklist)
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

                    string sInclude = string.Empty;
                    if (n.Include == 1)
                        sInclude = "(含下级版块)";

                    string sRole = string.Empty;
                    if (n.ForumID == 0)
                        sRole = BCW.User.Limits.OutLimitString(n.BlackRole);
                    else
                        sRole = BCW.User.FLimits.OutFLimitString(n.BlackRole);

                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")</a>");
                    if (n.BlackDay == 0)
                        builder.Append("永久锁定/理由:" + n.BlackWhy + "[" + DT.FormatDate(n.AddTime, 5) + "]<br />");
                    else
                        builder.Append("" + sRole + ":" + n.BlackDay + "天/理由:" + n.BlackWhy + "[" + DT.FormatDate(n.AddTime, 5) + "]<br />");

                    if (n.ForumID > 0)
                    {
                        builder.Append("范围:<a href=\"" + Utils.getUrl("/bbs/forum.aspx?forumid=" + n.ForumID + "") + "\">" + n.ForumName + "" + sInclude + "</a>");
                    }
                    if (n.AdminUsID > 0)
                    {
                        builder.Append("-操作ID:<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.AdminUsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.AdminUsID + "</a>");
                    }
                    else
                    {
                        builder.Append("-操作ID:系统管理员");
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
        }
        else if (ptype == 3)
        {
            strWhere += "ExitTime>='" + DateTime.Now + "'";
            // 开始读取列表
            IList<BCW.Model.Chatblack> listChatblack = new BCW.BLL.Chatblack().GetChatblacks(pageIndex, pageSize, strWhere, out recordCount);
            if (listChatblack.Count > 0)
            {
                int k = 1;
                foreach (BCW.Model.Chatblack n in listChatblack)
                {

                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));

                    builder.Append(((pageIndex - 1) * pageSize + k) + ".");

                    string Why = n.BlackWhy;
                    if (Why == "")
                        Why = "无";

                    if (ptype == 1)
                        builder.AppendFormat("<a href=\"" + Utils.getUrl("uinfo.aspx?uid={0}&amp;backurl=" + Utils.getPage(0) + "") + "\">{1}({0})</a>在聊室<a href=\"" + Utils.getUrl("/bbs/chatroom.aspx?id=" + n.ChatId + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.ChatName + "</a>被禁言{2}天/原因:" + Why + "[{3}]", n.UsID, n.UsName, n.BlackDay, DT.FormatDate(n.AddTime, 5));
                    else
                        builder.AppendFormat("<a href=\"" + Utils.getUrl("uinfo.aspx?uid={0}&amp;backurl=" + Utils.getPage(0) + "") + "\">{1}({0})</a>在聊室<a href=\"" + Utils.getUrl("/bbs/chatroom.aspx?id=" + n.ChatId + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.ChatName + "</a>被踢出{2}分钟/原因:" + Why + "[{3}]", n.UsID, n.UsName, n.BlackDay, DT.FormatDate(n.AddTime, 5));

                    builder.Append("<br />-操作ID:<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.AdminUsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.AdminUsID + "</a>");
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

        }
        else
        {
            strWhere += "ExitTime>='" + DateTime.Now + "'";
            // 开始读取列表
            IList<BCW.Model.Spkblack> listSpkblack = new BCW.BLL.Spkblack().GetSpkblacks(pageIndex, pageSize, strWhere, out recordCount);
            if (listSpkblack.Count > 0)
            {
                int k = 1;
                foreach (BCW.Model.Spkblack n in listSpkblack)
                {

                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));

                    builder.Append(((pageIndex - 1) * pageSize + k) + ".");

                    string Why = n.BlackWhy;
                    if (Why == "")
                        Why = "无";

                    builder.AppendFormat("<a href=\"" + Utils.getUrl("uinfo.aspx?uid={0}&amp;backurl=" + Utils.getPage(0) + "") + "\">{1}({0})</a>在" + BCW.User.AppCase.CaseAction(n.Types) + "游戏被加黑{2}天/原因:" + Why + "[{3}]", n.UsID, n.UsName, n.BlackDay, DT.FormatDate(n.AddTime, 5));

                    builder.Append("<br />-操作ID:<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.AdminUsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.AdminUsID + "</a>");
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
        }
        string strText = "输入用户ID:/,";
        string strName = "uid,ptype";
        string strType = "num,hidden";
        string strValu = "'" + ptype + "";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "搜黑名单,blacklist.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("manager.aspx") + "\">社区管理团队&gt;&gt;</a>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx") + "\">空间</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
}
