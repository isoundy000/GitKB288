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

public partial class bbs_online : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "add":
                AddPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-2]$", "0"));
        int forumid = int.Parse(Utils.GetRequest("forumid", "get", 1, @"^[1-9]\d*$", "0"));
        BCW.Model.Forum m = null;

        Master.Title = "在线会员";
        if (forumid > 0)
        {
            if (ub.GetSub("BbsIsOnline", "/Controls/bbs.xml") != "0")
            {
                Utils.Error("未开放此功能", "");
            }
            m = new BCW.BLL.Forum().GetForumBasic(forumid);
            if (m == null)
                Utils.Error("不存在的论坛记录", "");

            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "") + "\">" + m.Title + "</a>&gt;在线会员");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            if (ptype == 1)
                builder.Append("全站美女在线");
            else if (ptype == 2)
                builder.Append("全站帅哥在线");
            else
                builder.Append("全站在线会员");

            builder.Append(Out.Tab("</div>", "<br />"));
        }
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "ptype", "forumid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        if (forumid > 0)
            strWhere += "EndForumID=" + forumid + " and EndTime>='" + DateTime.Now.AddMinutes(-Convert.ToInt32(ub.Get("SiteExTime"))) + "'";
        else
        {
            //查询条件
            if (ptype == 1)
                strWhere = "Sex<=1 and";
            else if (ptype == 2)
                strWhere = "Sex=2 and";

            strWhere += " EndTime>='" + DateTime.Now.AddMinutes(-Convert.ToInt32(ub.Get("SiteExTime"))) + "'";
        }
        // 开始读取列表
        IList<BCW.Model.User> listUser = new BCW.BLL.User().GetUsers(pageIndex, pageSize, strWhere, out recordCount);
        if (listUser.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.User n in listUser)
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
                //if (n.State == 1)
                //    builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".隐身会员");

                builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(n.ID) + "(" + n.ID + ")</a>");

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
        builder.Append(Out.Tab("<div>", "<br />"));
        if (forumid > 0)
        {
            builder.Append("最高" + m.TopLine + "人，发生在" + DT.FormatDate(m.TopTime, 5) + "<br />");
            builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "") + "\">返回" + m.Title + "</a>");
        }
        else
        {
            builder.Append("帅哥<a href=\"" + Utils.getUrl("online.aspx?ptype=2&amp;backurl=" + Utils.getPage(0) + "") + "\">" + new BCW.BLL.User().GetNum(2) + "</a>|美女<a href=\"" + Utils.getUrl("online.aspx?ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">" + new BCW.BLL.User().GetNum(1) + "</a>");
        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void AddPage()
    { 
    
    }
}