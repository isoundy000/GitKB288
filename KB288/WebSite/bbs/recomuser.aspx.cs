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

public partial class bbs_recomuser : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "推荐注册排行榜";
        int meid = new BCW.User.Users().GetUsId();
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-2]\d*$", "0"));
        int Year = int.Parse(Utils.GetRequest("Year", "get", 1, @"^(?:19[3-9]\d|20[01]\d)$", "0"));
        int Month = int.Parse(Utils.GetRequest("Month", "get", 1, @"^(?:[0]?[1-9]|1[012])$", "0"));
        if (Year < 2010)
            Year = 0;

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        if (Year == 0 || Month == 0)
        {
            if (ptype == 0)
                builder.Append("总榜|");
            else
                builder.Append("<a href=\"" + Utils.getPage("recomuser.aspx?backurl=" + Utils.getPage(0) + "") + "\">总榜</a>|");

            if (ptype == 1)
                builder.Append("本月|");
            else
                builder.Append("<a href=\"" + Utils.getPage("recomuser.aspx?ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">本月</a>|");

            if (ptype == 2)
                builder.Append("上月");
            else
                builder.Append("<a href=\"" + Utils.getPage("recomuser.aspx?ptype=2&amp;backurl=" + Utils.getPage(0) + "") + "\">上月</a>");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getPage("recomuser.aspx?backurl=" + Utils.getPage(0) + "") + "\">总榜</a>|" + Year + "年" + Month + "月");
            ptype = 3;
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = 20;// Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = {"ptype", "Year", "Month", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        if (ptype == 1)
            strWhere = "Year(RegTime)=" + DateTime.Now.Year + " and Month(RegTime)=" + DateTime.Now.Month + " and ";
        else if (ptype == 2)
        {
            DateTime ForDate = DateTime.Parse(DateTime.Now.AddMonths(-1).ToShortDateString());
            int ForYear = ForDate.Year;
            int ForMonth = ForDate.Month;
            strWhere = "Year(RegTime) = " + (ForYear) + " AND Month(RegTime) = " + (ForMonth) + " and ";
        }
        else if (ptype == 3)
            strWhere = "(Year(RegTime) = " + Year + ") AND (Month(RegTime) = " + Month + ") and ";

        strWhere += "InviteNum>0 and IsVerify=1";

        // 开始读取列表
        IList<BCW.Model.User> listUser = new BCW.BLL.User().GetInvites(pageIndex, pageSize, strWhere, out recordCount);
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

                builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(n.ID) + "(" + n.InviteNum + "人)</a>");

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
        if (pageIndex == 1)
        {
            builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
            builder.Append("按月份查询：");
            builder.Append(Out.Tab("</div>", "<br />"));
            string strText = "年,月:,";
            string strName = "Year,Month,backurl";
            string strType = "snum,snum,hidden";
            string strValu = "" + DateTime.Now.Year + "'" + DateTime.Now.Month + "'" + Utils.getPage(0) + "";
            string strEmpt = "true,true,false";
            string strIdea = "";
            string strOthe = "查询,recomuser.aspx,get,3,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            if (meid > 0)
            {
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("您的推荐地址:<br /><a href=\"http://" + Utils.GetDomain() + "/reg-" + meid + ".aspx\">http://" + Utils.GetDomain() + "/reg-" + meid + ".aspx</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
}
