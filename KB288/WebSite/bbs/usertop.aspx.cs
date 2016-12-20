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

public partial class bbs_usertop : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "list":
                ListPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        Master.Title = "会员排行榜";
        builder.Append(Out.Tab("<div class=\"title\">会员排行榜</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("usertop.aspx?act=list&amp;ptype=0") + "\">最新会员</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("usertop.aspx?act=list&amp;ptype=1") + "\">签到次数</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("usertop.aspx?act=list&amp;ptype=2") + "\">在线时长</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("usertop.aspx?act=list&amp;ptype=3") + "\">空间人气</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("usertop.aspx?act=list&amp;ptype=4") + "\">会员" + ub.Get("SiteBz") + "</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("usertop.aspx?act=list&amp;ptype=5") + "\">会员等级</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("usertop.aspx?act=list&amp;ptype=6") + "\">VIP成长排行</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("recomuser.aspx") + "\">推荐会员排行</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ListPage()
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-6]$", "0"));
        Master.Title = "会员排行榜";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        if (ptype == 0)
            builder.Append("欢迎新人");
        else if (ptype == 1)
            builder.Append("签到次数排行");
        else if (ptype == 2)
            builder.Append("在线时长排行");
        else if (ptype == 3)
            builder.Append("空间人气排行");
        else if (ptype == 4)
            builder.Append("会员" + ub.Get("SiteBz") + "排行");
        else if (ptype == 5)
            builder.Append("会员等级排行");
        else if (ptype == 6)
            builder.Append("VIP成长排行");
        else
            builder.Append("推荐会员排行");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = 20;//Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        string strOrder = string.Empty;
        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        if (ptype == 4)
        {
            strWhere = "ForumSet not LIKE '%财产显示|1%'";
        }
        if (ptype == 0)
            strOrder = "RegTime Desc";
        else if (ptype == 1)
            strOrder = "SignTotal Desc";
        else if (ptype == 2)
            strOrder = "OnTime Desc";
        else if (ptype == 3)
            strOrder = "Click Desc";
        else if (ptype == 4)
            strOrder = "iGold Desc";
        else if (ptype == 5)
            strOrder = "Leven Desc";
        else if (ptype == 6)
            strOrder = "VipGrow Desc";
        // 开始读取列表
        IList<BCW.Model.User> listUser = new BCW.BLL.User().GetUsers(pageIndex, pageSize, strWhere, strOrder, out recordCount);
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
                string OutText = string.Empty;
                if (ptype == 0)
                    OutText = "(" + n.ID + ")";
                else if (ptype == 1)
                    OutText = "(" + n.Click + "次)";
                else if (ptype == 2)
                    OutText = "(" + BCW.User.Users.ChangeDayff(n.Click) + ")";
                else if (ptype == 3 || ptype == 6)
                    OutText = "(" + n.Click + "点)";
                else if (ptype == 4)
                    OutText = "(" + Utils.ConvertGold(n.iGold) + "" + ub.Get("SiteBz") + ")";
                else if (ptype == 5)
                    OutText = "(" + n.Click + "级)";

                n.UsName = BCW.User.Users.SetVipName(n.ID, n.UsName, false);

                builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + ((pageIndex - 1) * pageSize + k) + "." + n.UsName + "" + OutText + "</a>");

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
        builder.Append("<a href=\"" + Utils.getUrl("usertop.aspx?backurl=" + Utils.getPage(0) + "") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

}
