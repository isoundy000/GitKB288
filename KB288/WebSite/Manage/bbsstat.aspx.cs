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
using BCW.Data;
using BCW.Common;

public partial class Manage_bbsstat : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "top":
                TopPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        Master.Title = "社区统计";

        int TodayReg = Convert.ToInt32(SqlHelper.GetSingle("Select Count(ID) from tb_User where Year(RegTime)=" + DateTime.Now.Year + " AND Month(RegTime) = " + DateTime.Now.Month + " and Day(RegTime) = " + DateTime.Now.Day + " "));
        int YesterdayReg = Convert.ToInt32(SqlHelper.GetSingle("Select Count(ID) from tb_User where Year(RegTime)=" + DateTime.Now.AddDays(-1).Year + " AND Month(RegTime) = " + DateTime.Now.AddDays(-1).Month + " and Day(RegTime) = " + DateTime.Now.AddDays(-1).Day + " "));
        int AllReg = Convert.ToInt32(SqlHelper.GetSingle("Select Count(ID) from tb_User"));

        int TodayDiary = Convert.ToInt32(SqlHelper.GetSingle("Select Count(ID) from tb_Diary where Year(AddTime)=" + DateTime.Now.Year + " AND Month(AddTime) = " + DateTime.Now.Month + " and Day(AddTime) = " + DateTime.Now.Day + " "));
        int YesterdayDiary = Convert.ToInt32(SqlHelper.GetSingle("Select Count(ID) from tb_Diary where Year(AddTime)=" + DateTime.Now.AddDays(-1).Year + " AND Month(AddTime) = " + DateTime.Now.AddDays(-1).Month + " and Day(AddTime) = " + DateTime.Now.AddDays(-1).Day + " "));
        int AllDiary = Convert.ToInt32(SqlHelper.GetSingle("Select Count(ID) from tb_Diary"));

        int TodayUpfile = Convert.ToInt32(SqlHelper.GetSingle("Select Count(ID) from tb_Upfile where Year(AddTime)=" + DateTime.Now.Year + " AND Month(AddTime) = " + DateTime.Now.Month + " and Day(AddTime) = " + DateTime.Now.Day + " "));
        int YesterdayUpfile = Convert.ToInt32(SqlHelper.GetSingle("Select Count(ID) from tb_Upfile where Year(AddTime)=" + DateTime.Now.AddDays(-1).Year + " AND Month(AddTime) = " + DateTime.Now.AddDays(-1).Month + " and Day(AddTime) = " + DateTime.Now.AddDays(-1).Day + " "));
        int AllUpfile = Convert.ToInt32(SqlHelper.GetSingle("Select Count(ID) from tb_Upfile"));

        long gold = Convert.ToInt64(SqlHelper.GetSingle("Select Sum(iGold) from tb_User where IsSpier=0"));
        long bank = Convert.ToInt64(SqlHelper.GetSingle("Select Sum(iBank) from tb_User where IsSpier=0"));
        long money = Convert.ToInt64(SqlHelper.GetSingle("Select Sum(iMoney) from tb_User where IsSpier=0"));

        long gold2 = Convert.ToInt64(SqlHelper.GetSingle("Select Sum(iGold) from tb_User where IsSpier=1"));
        long bank2 = Convert.ToInt64(SqlHelper.GetSingle("Select Sum(iBank) from tb_User where IsSpier=1"));
        long money2 = Convert.ToInt64(SqlHelper.GetSingle("Select Sum(iMoney) from tb_User where IsSpier=1"));


        long score = Convert.ToInt64(SqlHelper.GetSingle("Select Sum(iScore) from tb_User"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("社区统计");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("今天注册：" + TodayReg + "<br />");
        builder.Append("昨天注册：" + YesterdayReg + "<br />");
        builder.Append("总注册数：" + AllReg + "<br />");

        builder.Append("今日帖子：" + new BCW.BLL.Forumstat().GetCount(0, 1, 1) + "<br />");
        builder.Append("今日回帖：" + new BCW.BLL.Forumstat().GetCount(0, 2, 1) + "<br />");
        builder.Append("昨日帖子：" + new BCW.BLL.Forumstat().GetCount(0, 1, 2) + "<br />");
        builder.Append("昨日回帖：" + new BCW.BLL.Forumstat().GetCount(0, 2, 2) + "<br />");
        builder.Append("本周帖子：" + new BCW.BLL.Forumstat().GetCount(0, 1, 3) + "<br />");
        builder.Append("本周回帖：" + new BCW.BLL.Forumstat().GetCount(0, 2, 3) + "<br />");
        builder.Append("本月帖子：" + new BCW.BLL.Forumstat().GetCount(0, 1, 4) + "<br />");
        builder.Append("本月回帖：" + new BCW.BLL.Forumstat().GetCount(0, 2, 4) + "<br />");
        builder.Append("帖子总数：" + new BCW.BLL.Forumstat().GetCount(0, 1, 0) + "<br />");
        builder.Append("回帖总数：" + new BCW.BLL.Forumstat().GetCount(0, 2, 0) + "<br />");

        builder.Append("今天日记：" + TodayDiary + "<br />");
        builder.Append("昨天日记：" + YesterdayDiary + "<br />");
        builder.Append("总日记数：" + AllDiary + "<br />");

        builder.Append("今天上传：" + TodayUpfile + "<br />");
        builder.Append("昨天上传：" + YesterdayUpfile + "<br />");
        builder.Append("总文件数：" + AllUpfile + "<br />");

        builder.Append("<a href=\"" + Utils.getUrl("bbsstat.aspx?act=top&amp;ptype=1") + "\">" + ub.Get("SiteBz") + "总计</a>：主" + gold + "/机" + gold2 + "<br />");
        builder.Append("<a href=\"" + Utils.getUrl("bbsstat.aspx?act=top&amp;ptype=2") + "\">银行里" + ub.Get("SiteBz") + "</a>：主" + bank + "/机" + bank2 + "<br />");
        builder.Append("<a href=\"" + Utils.getUrl("bbsstat.aspx?act=top&amp;ptype=3") + "\">" + ub.Get("SiteBz2") + "总计</a>：主" + money + "/机" + money2 + "<br />");
        builder.Append("<a href=\"" + Utils.getUrl("bbsstat.aspx?act=top&amp;ptype=4") + "\">积分总计</a>：" + score + "");

        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void TopPage()
    {
        Master.Title = "社区统计";
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-4]$", "1"));
        int showtype = int.Parse(Utils.GetRequest("showtype", "get", 1, @"^[0-1]$", "0"));
        int p = int.Parse(Utils.GetRequest("p", "get", 1, @"^[0-1]$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        if (ptype == 1)
            builder.Append(ub.Get("SiteBz") + "排行榜");
        else if (ptype == 2)
            builder.Append("银行"+ub.Get("SiteBz") + "排行榜");
        else if (ptype == 3)
            builder.Append(ub.Get("SiteBz2") + "排行榜");
        else if (ptype == 4)
            builder.Append("积分排行榜");

        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        if (showtype == 0)
            builder.Append("主号排行|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("bbsstat.aspx?act=top&amp;ptype=" + ptype + "&amp;showtype=0") + "\">主号排行</a>|");

        if (showtype == 1)
            builder.Append("机器排行");
        else
            builder.Append("<a href=\"" + Utils.getUrl("bbsstat.aspx?act=top&amp;ptype=" + ptype + "&amp;showtype=1") + "\">机器排行</a>");

        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        string strOrder = string.Empty;
        string[] pageValUrl = { "act", "ptype", "p", "showtype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        if (showtype == 0)
            strWhere = "IsSpier=0";
        else
            strWhere = "IsSpier=1";

        string sr = "";
        if (p == 0)
            sr = " Desc";
        else
            sr = " Asc";

        if (ptype == 1)
            strOrder = "iGold" + sr + "";
        else if (ptype == 2)
            strOrder = "iBank" + sr + "";
        else if (ptype == 3)
            strOrder = "iMoney" + sr + "";
        else if (ptype == 4)
            strOrder = "iScore" + sr + "";

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
                if (ptype == 1)
                    OutText = "(" +  Utils.ConvertGold(n.iGold) + "" + ub.Get("SiteBz") + ")";
                else if (ptype == 2)
                    OutText = "(" + Utils.ConvertGold(n.iBank) + "" + ub.Get("SiteBz") + ")";
                else if (ptype == 3)
                    OutText = "(" + ub.Get("SiteBz2") + "" + n.iMoney + ")";
                else if (ptype == 4)
                    OutText = "(积分" + n.iScore + ")";

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
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("排序:");
        if (p == 0)
            builder.Append("倒序|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("bbsstat.aspx?act=top&amp;ptype=" + ptype + "&amp;showtype=" + showtype + "&amp;p=0") + "\">倒序</a>|");

        if (p == 1)
            builder.Append("正序");
        else
            builder.Append("<a href=\"" + Utils.getUrl("bbsstat.aspx?act=top&amp;ptype=" + ptype + "&amp;showtype=" + showtype + "&amp;p=1") + "\">正序</a>");

        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("bbsstat.aspx") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
}
