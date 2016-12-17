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

public partial class bbs_Visit : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "我的足迹";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "list":
                ListPage(meid);
                break;
            default:
                ReloadPage(meid);
                break;
        }
    }

    private void ReloadPage(int uid)
    {
        string VisitHy = new BCW.BLL.User().GetVisitHy(uid);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("我的历史足迹..");
        builder.Append(Out.Tab("</div>", ""));
        if (!string.IsNullOrEmpty(VisitHy))
        {
            string[] sName = Regex.Split(VisitHy, "##");

            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string[] pageValUrl = { };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            //总记录数
            recordCount = sName.Length;

            int stratIndex = (pageIndex - 1) * pageSize;
            int endIndex = pageIndex * pageSize;
            int k = 0;
            for (int i = 0; i < sName.Length; i++)
            {
                if (k >= stratIndex && k < endIndex)
                {
                    if ((k + 1) % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));

                    builder.Append("" + (i + 1) + "." + Out.SysUBB(sName[i].ToString()) + "");
                    builder.Append(Out.Tab("</div>", ""));
                }
                if (k == endIndex)
                    break;
                k++;
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx?uid=" + uid + "") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));

    }


    private void ListPage(int uid)
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-2]$", "0"));
        int hid = int.Parse(Utils.GetRequest("hid", "get", 1, @"^[1-9]\d*$", "0"));
        if (hid == 0)
            hid = uid;

        string NameType = string.Empty;
        string sText = string.Empty;
        if (uid == hid)
            NameType = "我";
        else
            NameType = "TA";
        if (ptype == 2)
            sText = "谁来拜访过" + NameType + "";
        else
            sText = "" + NameType + "去拜访过谁";

        Master.Title = sText;
        builder.Append(Out.Tab("<div class=\"title\">" + sText + "</div>", ""));

        builder.Append(Out.Tab("<div class=\"text\">", ""));

        builder.Append("<a href=\"" + Utils.getUrl("mebook.aspx?hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">留言</a>|");

        if (ptype == 1)
            builder.Append("拜访|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("visit.aspx?act=list&amp;ptype=1&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">拜访</a>|");

        if (ptype == 2)
            builder.Append("访客|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("visit.aspx?act=list&amp;ptype=2&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">访客</a>|");

        builder.Append("<a href=\"" + Utils.getUrl("action.aspx?act=friend&amp;hid=" + hid + "") + "\">好友</a>|");

        if (uid == hid)
            builder.Append("<a href=\"" + Utils.getUrl("action.aspx?act=fans&amp;hid=" + hid + "") + "\">关注</a>");
        else
            builder.Append("<a href=\"" + Utils.getUrl("action.aspx?act=me&amp;hid=" + hid + "") + "\">动态</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "uid", "ptype", "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        if (ptype == 2)
            strWhere = "UsID=" + hid + "";
        else
            strWhere = "VisitId=" + hid + "";

        // 开始读取列表
        IList<BCW.Model.Visitor> listVisitor = new BCW.BLL.Visitor().GetVisitors(pageIndex, pageSize, strWhere, out recordCount);
        if (listVisitor.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Visitor n in listVisitor)
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
                if (ptype == 1)
                    builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + ((pageIndex - 1) * pageSize + k) + "." + n.UsName + "</a>");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.VisitId + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + ((pageIndex - 1) * pageSize + k) + "." + n.VisitName + "</a>");

                builder.Append("(" + DT.DateDiff2(DateTime.Now, n.AddTime) + "前)");
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
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx?uid=" + uid + "") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
}
