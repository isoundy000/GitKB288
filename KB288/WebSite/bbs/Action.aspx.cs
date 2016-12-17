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
/// 最新版20160423
/// </summary>
public partial class bbs_Action : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "me":
            case "friend":
            case "fans":
                FriendPage(act);
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {        
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-9]\d*$", "0"));
        Master.Title = "大家都在忙啥呢";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("大家都在忙啥呢");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        ///取消显示球赛动态
        string strWhere = "(Types <> 5) AND (Types <> 23) ";
        string[] pageValUrl = { "ptype", "uid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        if (ptype > 0)
        {
            if (ptype == 999)
                strWhere = "(Types=999 or Types=997 or Types=998)";
            else
                strWhere = "Types=" + ptype + "";
        }

        //指定不显示609 中介充值的商品
        strWhere += "AND (NodeId <> 609)";

        // 开始读取列表
        IList<BCW.Model.Action> listAction = new BCW.BLL.Action().GetActions(pageIndex, pageSize, strWhere, out recordCount);
        if (listAction.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Action n in listAction)
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
                if (n.UsId == 0)
                    builder.AppendFormat("{0}前{1}", DT.DateDiff2(DateTime.Now, n.AddTime), Out.SysUBB(n.Notes));
                else
                    builder.AppendFormat("{0}前<a href=\"" + Utils.getUrl("uinfo.aspx?uid={1}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}</a>{3}", DT.DateDiff2(DateTime.Now, n.AddTime), n.UsId, n.UsName, Out.SysUBB(n.Notes));
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
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void FriendPage(string act)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int hid = int.Parse(Utils.GetRequest("hid", "get", 2, @"^[1-9]\d*$", "0"));
        if (hid == 0)
            hid = meid;

        string NameType = string.Empty;
        string sTitle = string.Empty;
        if (hid == meid)
            NameType = "我";
        else
            NameType = "TA";

        if (act == "fans")
            sTitle = "关注的友友";
        else if (act == "friend")
            sTitle = "的好友动态";
        else
            sTitle = "的动态记录";

        Master.Title = "" + NameType + "" + sTitle + "";
        builder.Append(Out.Tab("<div class=\"title\">" + NameType + "" + sTitle + "</div>", ""));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("mebook.aspx?hid=" + hid + "") + "\">留言</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("visit.aspx?act=list&amp;ptype=1&amp;hid=" + hid + "") + "\">拜访</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("visit.aspx?act=list&amp;ptype=2&amp;hid=" + hid + "") + "\">访客</a>|");
        if (act == "friend")
            builder.Append("好友|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("action.aspx?act=friend&amp;hid=" + hid + "") + "\">好友</a>|");

        if (meid == hid)
        {
            if (act == "fans")
            {
                builder.Append("关注");
                builder.Append("<br /><a href=\"" + Utils.getUrl("friend.aspx?act=fans&amp;backurl=" + Utils.PostPage(1) + "") + "\">管理我关注的友友</a>");
            }
            else
                builder.Append("<a href=\"" + Utils.getUrl("action.aspx?act=fans&amp;hid=" + hid + "") + "\">关注</a>");
        }
        else
        {
            if (act == "me")
                builder.Append("动态");
            else
                builder.Append("<a href=\"" + Utils.getUrl("action.aspx?act=me&amp;hid=" + hid + "") + "\">动态</a>");
        }

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string[] pageValUrl = { "act", "hid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        int Types = 0;
        if (act == "fans")
            Types = 2;

        // 开始读取列表
        IList<BCW.Model.Action> listAction = null;
        if (act == "me")
            listAction = new BCW.BLL.Action().GetActions(pageIndex, pageSize, "usid=" + hid + " and notes not like '%guess2%' and notes not like '%bbsshop%' ", out recordCount);
        else
            listAction = new BCW.BLL.Action().GetActionsFriend(Types, hid, pageIndex, pageSize, out recordCount);

        if (listAction.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Action n in listAction)
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
                builder.AppendFormat("{0}前<a href=\"" + Utils.getUrl("uinfo.aspx?uid={1}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}</a>{3}", DT.DateDiff2(DateTime.Now, n.AddTime), n.UsId, n.UsName, Out.SysUBB(n.Notes));
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
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
}