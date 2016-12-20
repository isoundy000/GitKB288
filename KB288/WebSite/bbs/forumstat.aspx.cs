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
/// 修复搜索排行问题
/// 陈志基  2016/4/9
/// 陈志基  2016/08/11
/// 修改统计方式
/// </summary>
public partial class bbs_forumstat : System.Web.UI.Page
{
    
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {

        int forumid = int.Parse(Utils.GetRequest("forumid", "all", 1, @"^[0-9]\d*$", "0"));

        int meid = new BCW.User.Users().GetUsId();

        if (meid == 0)
            Utils.Login();

        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "forumtop":
                ForumTopPage();
                break;
            case "top":
            case "top2":
                TopPage(act, meid, forumid);
                break;
            case "toplist":
            case "toplist2":
                TopListPage(act, meid, forumid);
                break;
            case "praisetoplist":
                PraiseTopListPage(act, meid, forumid);
                break;
            case "forumtopsr":
                ForumTopSearchPage();
                break;
            default:
                ReloadPage(meid, forumid);
                break;
        }
    }
    /// <summary>
    /// 陈志基 2016/08/16
    /// 修改统计方式
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="forumid"></param>
    #region 论坛统计 ReloadPage
    private void ReloadPage(int uid, int forumid)
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-2]$", "1"));
        string forum = "";
        string ForumName = "主题论坛";
        if (ptype == 1)
        {
            forum = "(Select GroupId FROM tb_Forum where ID=ForumID)=0 and ";
        }
        else
        {
            forum = "(Select GroupId FROM tb_Forum where ID=ForumID)<>0 and ";
        }
        if (forumid > 0)
        {
            ForumName = new BCW.BLL.Forum().GetTitle(forumid);
            ptype = 0;
        }
        else
        {
            if (ptype == 2)
                ForumName = "圈子论坛";
        }
     
        //string strWhe = "";
        //DateTime nowtime = DateTime.Now;
        //string time = nowtime.ToShortDateString();
        //string time1 = time + "  00:00:00 ";
        //string time2 = time + "  23:59:59 ";
        ////builder.Append("time:" + time1 + "<br/>");
        ////builder.Append("time:" + time2 + "<br/>");
        //strWhe = forum + " AddTime between '" + time1 + "' and '" + time2 + "' and IsDel = 0";
        //DataSet data = new BCW.BLL.Text().GetList("ID", strWhe);//当天发帖
        //DataSet data1 = new BCW.BLL.Reply().GetList("ID", strWhe);//当天发帖
        ////builder.Append("data：当天？" + data.Tables[0].Rows.Count + "<br/>");
        ////builder.Append("data：当天回帖？" + data1.Tables[0].Rows.Count + "<br/>");

        Master.Title = "" + ForumName + "统计";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("=" + ForumName + "统计=");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("今日帖子数量:" + new BCW.BLL.Forumstat().GetCount2(ptype,forumid,1,1) + "<br />");
        builder.Append("今日回帖数量:" + new BCW.BLL.Forumstat().GetCount2(ptype, forumid, 2, 1) + "");
        builder.Append(Out.Tab("</div>", "<br />"));
    
     

        //DateTime yesterday = nowtime.AddDays(-1);
        //time = yesterday.ToShortDateString();
        //time1 = time + "  00:00:00 ";
        //time2 = time + "  23:59:59 ";
        ////builder.Append("yesterday:" + time1 + "<br/>");
        ////builder.Append("yesterday:" + time2 + "<br/>");
        //strWhe = forum + " AddTime between '" + time1 + "' and '" + time2 + "' and IsDel = 0";
        //data = new BCW.BLL.Text().GetList("ID", strWhe);//昨天发帖
        //data1 = new BCW.BLL.Reply().GetList("ID", strWhe);//昨天发帖
        ////builder.Append("data：昨天？" + data.Tables[0].Rows.Count + "<br/>");
        ////builder.Append("data：昨天回帖？" + data1.Tables[0].Rows.Count + "<br/>");

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("昨日帖子数量:" + new BCW.BLL.Forumstat().GetCount2(ptype, forumid, 1, 2) + "<br />");
        builder.Append("昨日回帖数量:" + new BCW.BLL.Forumstat().GetCount2(ptype, forumid, 2, 2) + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        //string M_Str_mindate = string.Empty;

        //switch (DateTime.Now.DayOfWeek)
        //{
        //    case DayOfWeek.Monday:
        //        M_Str_mindate = DateTime.Now.AddDays(0).ToShortDateString() + "";
        //        break;
        //    case DayOfWeek.Tuesday:
        //        M_Str_mindate = DateTime.Now.AddDays(-1).ToShortDateString() + "";
        //        break;
        //    case DayOfWeek.Wednesday:
        //        M_Str_mindate = DateTime.Now.AddDays(-2).ToShortDateString() + "";
        //        break;
        //    case DayOfWeek.Thursday:
        //        M_Str_mindate = DateTime.Now.AddDays(-3).ToShortDateString() + "";
        //        break;
        //    case DayOfWeek.Friday:
        //        M_Str_mindate = DateTime.Now.AddDays(-4).ToShortDateString() + "";
        //        break;
        //    case DayOfWeek.Saturday:
        //        M_Str_mindate = DateTime.Now.AddDays(-5).ToShortDateString() + "";
        //        break;
        //    case DayOfWeek.Sunday:
        //        M_Str_mindate = DateTime.Now.AddDays(-6).ToShortDateString() + "";
        //        break;
        //}
        //strWhe = forum + " AddTime>='" + M_Str_mindate + "' and IsDel =0";
        //data = new BCW.BLL.Text().GetList("ID", strWhe);//本周发帖的
        ////builder.Append("data：本周？" + data.Tables[0].Rows.Count + "<br/>");
        //data1 = new BCW.BLL.Reply().GetList("ID", strWhe);//本周发帖的
        //builder.Append("data：本周回帖？" + data1.Tables[0].Rows.Count + "<br/>");
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("本周帖子数量:" + new BCW.BLL.Forumstat().GetCount2(ptype, forumid, 1, 3) + "<br />");
        builder.Append("本周回帖数量:" + new BCW.BLL.Forumstat().GetCount2(ptype, forumid, 2, 3) + "");
        builder.Append(Out.Tab("</div>", "<br />"));



        //DateTime ForDate = DateTime.Parse(DateTime.Now.AddDays(-7).ToShortDateString());
        //M_Str_mindate = string.Empty;
        //string M_Str_Maxdate = string.Empty;

        //switch (ForDate.DayOfWeek)
        //{
        //    case DayOfWeek.Monday:
        //        M_Str_mindate = ForDate.AddDays(0).ToShortDateString() + "";
        //        break;
        //    case DayOfWeek.Tuesday:
        //        M_Str_mindate = ForDate.AddDays(-1).ToShortDateString() + "";
        //        break;
        //    case DayOfWeek.Wednesday:
        //        M_Str_mindate = ForDate.AddDays(-2).ToShortDateString() + "";
        //        break;
        //    case DayOfWeek.Thursday:
        //        M_Str_mindate = ForDate.AddDays(-3).ToShortDateString() + "";
        //        break;
        //    case DayOfWeek.Friday:
        //        M_Str_mindate = ForDate.AddDays(-4).ToShortDateString() + "";
        //        break;
        //    case DayOfWeek.Saturday:
        //        M_Str_mindate = ForDate.AddDays(-5).ToShortDateString() + "";
        //        break;
        //    case DayOfWeek.Sunday:
        //        M_Str_mindate = ForDate.AddDays(-6).ToShortDateString() + "";
        //        break;
        //}
        //M_Str_Maxdate = DateTime.Parse(M_Str_mindate).AddDays(6).ToShortDateString();
        //strWhe = forum + " AddTime between '" + M_Str_mindate + " 00:00:00' AND '" + M_Str_Maxdate + " 23:59:59'  and IsDel =0";
        ////builder.Append("M_Str_mindate:" + M_Str_mindate + "<br/>");
        ////builder.Append("M_Str_Maxdate:" + M_Str_Maxdate + "<br/>");
        //data = new BCW.BLL.Text().GetList("ID", strWhe);//本月发帖的
        ////builder.Append("data：上周？" + data.Tables[0].Rows.Count + "<br/>");
        //data1 = new BCW.BLL.Reply().GetList("ID", strWhe);//本月发帖的
       // builder.Append("data：上周回帖？" + data1.Tables[0].Rows.Count + "<br/>");
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("上周帖子数量:" + new BCW.BLL.Forumstat().GetCount2(ptype, forumid, 1, 6) + "<br />");
        builder.Append("上周回帖数量:" + new BCW.BLL.Forumstat().GetCount2(ptype, forumid, 2, 6) + "");
        builder.Append(Out.Tab("</div>", "<br />"));


       // strWhe = forum + " Year(AddTime)=" + DateTime.Now.Year + " and Month(AddTime)=" + DateTime.Now.Month + "  and IsDel =0";
       // data = new BCW.BLL.Text().GetList("ID", strWhe);//本月发帖的
       //// builder.Append("data：本月？" + data.Tables[0].Rows.Count + "<br/>");
       // data1 = new BCW.BLL.Reply().GetList("ID", strWhe);//本月发帖的
        //builder.Append("data：本月回帖？" + data1.Tables[0].Rows.Count + "<br/>");

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("本月帖子数量:" + new BCW.BLL.Forumstat().GetCount2(ptype, forumid, 1, 4) + "<br />");
        builder.Append("本月回帖数量:" + new BCW.BLL.Forumstat().GetCount2(ptype, forumid, 2, 4) + "");
        builder.Append(Out.Tab("</div>", "<br />"));


       // DateTime ForDate1 = DateTime.Parse(DateTime.Now.AddMonths(-1).ToShortDateString());
       // int ForYear = ForDate1.Year;
       // int ForMonth = ForDate1.Month;
       // strWhe = forum + " Year(AddTime) = " + (ForYear) + " AND Month(AddTime) = " + (ForMonth) + "  and IsDel =0";
       // data = new BCW.BLL.Text().GetList("ID", strWhe);//上月发帖的
       //// builder.Append("data：上月？" + data.Tables[0].Rows.Count + "<br/>");
       // data1 = new BCW.BLL.Reply().GetList("ID", strWhe);//上月发帖的
        //builder.Append("data：上月回帖？" + data1.Tables[0].Rows.Count + "<br/>");
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("上月帖子数量:" + new BCW.BLL.Forumstat().GetCount2(ptype, forumid, 1, 5) + "<br />");
        builder.Append("上月回帖数量:" + new BCW.BLL.Forumstat().GetCount2(ptype, forumid, 2, 5) + "");
        builder.Append(Out.Tab("</div>", "<br />"));


       // strWhe = forum + "IsDel =0";
       // data = new BCW.BLL.Text().GetList("ID", strWhe);//上月发帖的
       //// builder.Append("data：帖子总数量？" + data.Tables[0].Rows.Count + "<br/>");
       // data1 = new BCW.BLL.Reply().GetList("ID", strWhe);//上月发帖的
        //builder.Append("data：回帖总数量？" + data1.Tables[0].Rows.Count + "<br/>");

        //builder.Append(forumid+"asdfsda");

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("帖子总数量:" + new BCW.BLL.Forumstat().GetCount2(ptype, forumid, 1, 0) + "<br />");
        builder.Append("回帖总数量:" + new BCW.BLL.Forumstat().GetCount2(ptype, forumid, 2, 0) + "");
        builder.Append(Out.Tab("</div>", ""));
        if (forumid > 0)
        {
            BCW.Model.Forum m = new BCW.BLL.Forum().GetForumBasic(forumid);
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("/bbs/online.aspx?forumid=" + forumid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">当前在线" + m.Line + "人.最高" + m.TopLine + "人</a><br />");
            builder.Append("最高记录发生在" + DT.FormatDate(m.TopTime, 5) + "");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            if (ptype == 2)
                builder.Append("<a href=\"" + Utils.getUrl("forumstat.aspx?ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">主题论坛统计&gt;&gt;</a>");
            else
                builder.Append("<a href=\"" + Utils.getUrl("forumstat.aspx?ptype=2&amp;backurl=" + Utils.getPage(0) + "") + "\">圈子论坛统计&gt;&gt;</a>");

            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?backurl=" + Utils.getPage(0) + "") + "\">论坛</a>");
        builder.Append(Out.Tab("</div>", ""));

    }
    #endregion

    #region 排行榜入口 TopPage
    private void TopPage(string act, int uid, int forumid)
    {
        // int bid = int.Parse(Utils.GetRequest("bid", "all", 1, @"^[0-9]\d*$", "0"));
        string ForumName = "主题论坛";
        if (act == "top")
        {
            act = "toplist";
        }
        else
        {
            act = "toplist2";
            ForumName = "圈子论坛";
        }
        if (forumid > 0)
        {
            ForumName = new BCW.BLL.Forum().GetTitle(forumid);
            int GroupId = new BCW.BLL.Forum().GetGroupId(forumid);
            if (GroupId > 0)
                act = "toplist2";
            else
                act = "toplist";
        }
        // BCW.Model.Forumstat a = new BCW.Model.Forumstat();
        // a.
        Master.Title = "" + ForumName + "排行";
        //
        //builder.Append(act + "<br/>");
        //builder.Append(forumid + "<br/>");
        //builder.Append(bid + "<br/>");
        //

        builder.Append(Out.Tab("<div class=\"title\">" + ForumName + "排行</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("+|总排行榜<br />");
        builder.Append("<a href=\"" + Utils.getUrl("forumstat.aspx?act=" + act + "&amp;forumid=" + forumid + "&amp;ptype=1&amp;showtype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">发帖最多</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("forumstat.aspx?act=" + act + "&amp;forumid=" + forumid + "&amp;ptype=2&amp;showtype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">回帖最多</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("forumstat.aspx?act=" + act + "&amp;forumid=" + forumid + "&amp;ptype=3&amp;showtype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">精华最多</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("forumstat.aspx?act=" + act + "&amp;forumid=" + forumid + "&amp;ptype=4&amp;showtype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">推荐最多</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("+|本周排行榜<br />");
        builder.Append("<a href=\"" + Utils.getUrl("forumstat.aspx?act=" + act + "&amp;forumid=" + forumid + "&amp;ptype=1&amp;showtype=2&amp;backurl=" + Utils.getPage(0) + "") + "\">发帖最多</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("forumstat.aspx?act=" + act + "&amp;forumid=" + forumid + "&amp;ptype=2&amp;showtype=2&amp;backurl=" + Utils.getPage(0) + "") + "\">回帖最多</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("forumstat.aspx?act=" + act + "&amp;forumid=" + forumid + "&amp;ptype=3&amp;showtype=2&amp;backurl=" + Utils.getPage(0) + "") + "\">精华最多</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("forumstat.aspx?act=" + act + "&amp;forumid=" + forumid + "&amp;ptype=4&amp;showtype=2&amp;backurl=" + Utils.getPage(0) + "") + "\">推荐最多</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("+|上周排行榜<br />");
        builder.Append("<a href=\"" + Utils.getUrl("forumstat.aspx?act=" + act + "&amp;forumid=" + forumid + "&amp;ptype=1&amp;showtype=5&amp;backurl=" + Utils.getPage(0) + "") + "\">发帖最多</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("forumstat.aspx?act=" + act + "&amp;forumid=" + forumid + "&amp;ptype=2&amp;showtype=5&amp;backurl=" + Utils.getPage(0) + "") + "\">回帖最多</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("forumstat.aspx?act=" + act + "&amp;forumid=" + forumid + "&amp;ptype=3&amp;showtype=5&amp;backurl=" + Utils.getPage(0) + "") + "\">精华最多</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("forumstat.aspx?act=" + act + "&amp;forumid=" + forumid + "&amp;ptype=4&amp;showtype=5&amp;backurl=" + Utils.getPage(0) + "") + "\">推荐最多</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("+|本月排行榜<br />");
        builder.Append("<a href=\"" + Utils.getUrl("forumstat.aspx?act=" + act + "&amp;forumid=" + forumid + "&amp;ptype=1&amp;showtype=3&amp;backurl=" + Utils.getPage(0) + "") + "\">发帖最多</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("forumstat.aspx?act=" + act + "&amp;forumid=" + forumid + "&amp;ptype=2&amp;showtype=3&amp;backurl=" + Utils.getPage(0) + "") + "\">回帖最多</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("forumstat.aspx?act=" + act + "&amp;forumid=" + forumid + "&amp;ptype=3&amp;showtype=3&amp;backurl=" + Utils.getPage(0) + "") + "\">精华最多</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("forumstat.aspx?act=" + act + "&amp;forumid=" + forumid + "&amp;ptype=4&amp;showtype=3&amp;backurl=" + Utils.getPage(0) + "") + "\">推荐最多</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("+|上月排行榜<br />");
        builder.Append("<a href=\"" + Utils.getUrl("forumstat.aspx?act=" + act + "&amp;forumid=" + forumid + "&amp;ptype=1&amp;showtype=4&amp;backurl=" + Utils.getPage(0) + "") + "\">发帖最多</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("forumstat.aspx?act=" + act + "&amp;forumid=" + forumid + "&amp;ptype=2&amp;showtype=4&amp;backurl=" + Utils.getPage(0) + "") + "\">回帖最多</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("forumstat.aspx?act=" + act + "&amp;forumid=" + forumid + "&amp;ptype=3&amp;showtype=4&amp;backurl=" + Utils.getPage(0) + "") + "\">精华最多</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("forumstat.aspx?act=" + act + "&amp;forumid=" + forumid + "&amp;ptype=4&amp;showtype=4&amp;backurl=" + Utils.getPage(0) + "") + "\">推荐最多</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        if (act == "toplist")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("+|" + ForumName + "点赞排行榜<br />");
            builder.Append("<a href=\"" + Utils.getUrl("forumstat.aspx?act=praisetoplist&amp;forumid=" + forumid + "&amp;ptype=5&amp;showtype=6&amp;backurl=" + Utils.getPage(0) + "") + "\">本月</a>.");
            builder.Append("<a href=\"" + Utils.getUrl("forumstat.aspx?act=praisetoplist&amp;forumid=" + forumid + "&amp;ptype=6&amp;showtype=6&amp;backurl=" + Utils.getPage(0) + "") + "\">本年</a>");
            builder.Append(Out.Tab("</div>", "<br />"));

            if (!(forumid > 0))
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("+|圈子论坛点赞排行榜<br />");
                builder.Append("<a href=\"" + Utils.getUrl("forumstat.aspx?act=praisetoplist&amp;forumid=" + forumid + "&amp;ptype=5&amp;showtype=7&amp;backurl=" + Utils.getPage(0) + "") + "\">本月</a>.");
                builder.Append("<a href=\"" + Utils.getUrl("forumstat.aspx?act=praisetoplist&amp;forumid=" + forumid + "&amp;ptype=6&amp;showtype=7&amp;backurl=" + Utils.getPage(0) + "") + "\">本年</a>");

                builder.Append(Out.Tab("</div>", "<br />"));
            }
        }
        if (act == "toplist2")//是圈子
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("+|" + ForumName + "点赞排行榜<br />");
            builder.Append("<a href=\"" + Utils.getUrl("forumstat.aspx?act=praisetoplist&amp;forumid=" + forumid + "&amp;ptype=5&amp;showtype=7&amp;backurl=" + Utils.getPage(0) + "") + "\">本月</a>.");
            builder.Append("<a href=\"" + Utils.getUrl("forumstat.aspx?act=praisetoplist&amp;forumid=" + forumid + "&amp;ptype=6&amp;showtype=7&amp;backurl=" + Utils.getPage(0) + "") + "\">本年</a>");
            builder.Append(Out.Tab("</div>", "<br />"));

        }
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("注:本周和本月排行榜指帖子发布时间在本周或本月的帖子");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("", Out.Hr()));

        if (forumid > 0)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "") + "\">返回" + ForumName + "</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("sktype.aspx?forumid=" + forumid + "") + "\">更多帖子</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion


    #region 点赞统计列表PraiseTopListPage
    /// <summary>
    /// 点赞统计列表
    /// </summary>
    /// <param name="act"></param>
    /// <param name="uid"></param>
    /// <param name="forumid"></param>
    private void PraiseTopListPage(string act, int uid, int forumid)
    {
        string ForumName = "论坛";
        if (forumid > 0)
            ForumName = new BCW.BLL.Forum().GetTitle(forumid);

        Master.Title = "" + ForumName + "TOP100";
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-6]$", "5"));// 获取失败时 默认为1
        int showtype = int.Parse(Utils.GetRequest("showtype", "get", 1, @"^[1-7]$", "6"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        if (showtype == 6)
        {
            builder.Append("主题论坛");
        }
        else if (showtype == 7)
        {
            builder.Append("圈子论坛");
        }
        if (ptype == 5)
        {
            builder.Append("本月");
        }
        else if (ptype == 6)
        {
            builder.Append("本年");
        }

        builder.Append("排行");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        string strOrder = string.Empty;
        string[] pageValUrl = { "act", "forumid", "ptype", "showtype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        if (showtype == 6)  //判断是主题还是圈子
            strWhere = "(Select GroupId FROM tb_Forum where ID=ForumID)=0";
        else if (showtype == 7)
            strWhere = "(Select GroupId FROM tb_Forum where ID=ForumID)<>0";

        if (forumid > 0)
            strWhere += " and forumid=" + forumid + "";

        //if (ptype == 5)
        strOrder = "sum(Praise)";


        //修改strOrder ="sum(pTotal)";

        // 开始读取列表
        IList<BCW.Model.Text> listForumstat = new BCW.BLL.Text().GetForumstats(pageIndex, pageSize, strWhere, strOrder, ptype, out recordCount);
        if (listForumstat.Count > 0)
        {

            int k = 1;
            foreach (BCW.Model.Text n in listForumstat)
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

                //builder.AppendFormat("{0}.", (pageIndex - 1) * pageSize + k);

                builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(n.UsID) + "(" + n.UsID + ")(" + n.Praise + "个点赞)</a>");

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
        if (forumid > 0)
            builder.Append("<a href=\"" + Utils.getPage("forum.aspx?forumid=" + forumid + "") + "\">上级</a>");
        else
            builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>");

        builder.Append("-<a href=\"" + Utils.getUrl("forumstat.aspx?act=top&amp;forumid=" + forumid + "") + "\">排行榜</a>");
        builder.Append(Out.Tab("</div>", ""));


    }
    #endregion
    #region 总榜统计列表TopListPage
    /// <summary>
    /// 总榜统计列表
    /// 陈志基 2016/08/11
    /// 修改统计方法
    /// </summary>
    /// <param name="act"></param>
    /// <param name="uid"></param>
    /// <param name="forumid"></param>
    private void TopListPage(string act, int uid, int forumid)
    {
        string ForumName = "论坛";
        if (forumid > 0)
            ForumName = new BCW.BLL.Forum().GetTitle(forumid);

        Master.Title = "" + ForumName + "TOP100";
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-4]$", "1"));// 获取失败时 默认为1
        int showtype = int.Parse(Utils.GetRequest("showtype", "get", 1, @"^[1-5]$", "1"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));

        if (showtype == 1)
        {
            builder.Append("总计");
        }
        else if (showtype == 2)
        {
            builder.Append("本周");
        }
        else if (showtype == 3)
        {
            builder.Append("本月");
        }
        else if (showtype == 4)
        {
            builder.Append("上月");
        }
        else if (showtype == 5)
        {
            builder.Append("上周");
        }
        if (ptype == 1)
        {
            builder.Append("发帖");
        }
        else if (ptype == 2)
        {
            builder.Append("回帖");
        }
        else if (ptype == 3)
        {
            builder.Append("精华");
        }
        else if (ptype == 4)
        {
            builder.Append("推荐");
        }

        builder.Append("排行");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        string strOrder = string.Empty;
        string[] pageValUrl = { "act", "forumid", "ptype", "showtype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        if (act == "toplist")
            strWhere = "(Select GroupId FROM tb_Forum where ID=ForumID)=0";
        else
            strWhere = "(Select GroupId FROM tb_Forum where ID=ForumID)<>0";

        if (forumid > 0)
            strWhere += " and forumid=" + forumid + "";

        if (ptype == 1)
            strOrder = "sum(tTotal)";
        else if (ptype == 2)
            strOrder = "sum(rTotal)";
        else if (ptype == 3)
            strOrder = "sum(gTotal)";
        else if (ptype == 4)
            strOrder = "sum(jTotal)";
        #region
        ////  ceshi
        //if (ptype == 1)  //发帖
        //    strWhere += " and IsDel=0 ";
        //else if (ptype == 2)//回帖
        //    strWhere += " and IsDel=0  ";
        //else if (ptype == 3)//精华
        //    strWhere += " and IsGood=1 ";
        //else if (ptype == 4)//推荐
        //    strWhere += " and IsRecom=1 ";
        //if (ptype != 2)   //不是回帖的
        //{
        //    IList<BCW.Model.Text> list = new BCW.BLL.Text().GetForumstats1(pageIndex, pageSize, strWhere, showtype, out recordCount);
        //    if (list.Count > 0)
        //    {
        //        int k = 1;
        //        foreach (BCW.Model.Text n in list)
        //        {
        //            if (k % 2 == 0)
        //                builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        //            else
        //            {
        //                if (k == 1)
        //                    builder.Append(Out.Tab("<div>", ""));
        //                else
        //                    builder.Append(Out.Tab("<div>", "<br />"));
        //            }

        //            //builder.AppendFormat("{0}.", (pageIndex - 1) * pageSize + k);

        //            builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(n.UsID) + "(" + n.UsID + ")(" + n.ReadNum + "帖)</a>");

        //            k++;
        //            builder.Append(Out.Tab("</div>", ""));

        //        }
        //        // 分页
        //        builder.Append(BasePage.ForumMultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        //    }
        //    else
        //    {
        //        builder.Append(Out.Div("div", "没有相关记录.."));
        //    }
        //}
        //else
        //{
        //    IList<BCW.Model.Reply> list = new BCW.BLL.Reply().GetForumstats1(pageIndex, pageSize, strWhere, showtype, out recordCount);
        //    if (list.Count > 0)
        //    {
        //        int k = 1;
        //        foreach (BCW.Model.Reply n in list)
        //        {
        //            if (k % 2 == 0)
        //                builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        //            else
        //            {
        //                if (k == 1)
        //                    builder.Append(Out.Tab("<div>", ""));
        //                else
        //                    builder.Append(Out.Tab("<div>", "<br />"));
        //            }

        //            //builder.AppendFormat("{0}.", (pageIndex - 1) * pageSize + k);

        //            builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(n.UsID) + "(" + n.UsID + ")(" + n.Floor + "帖)</a>");

        //            k++;
        //            builder.Append(Out.Tab("</div>", ""));

        //        }
        //        // 分页
        //        builder.Append(BasePage.ForumMultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        //    }
        //    else
        //    {
        //        builder.Append(Out.Div("div", "没有相关记录.."));
        //    }
        //}
        //if (list.Count > 0)
        //{
        //    int k = 1;
        //    foreach (BCW.Model.Text n in list)
        //    {
        //        if (k % 2 == 0)
        //            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        //        else
        //        {
        //            if (k == 1)
        //                builder.Append(Out.Tab("<div>", ""));
        //            else
        //                builder.Append(Out.Tab("<div>", "<br />"));
        //        }

        //        //builder.AppendFormat("{0}.", (pageIndex - 1) * pageSize + k);

        //        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(n.UsID) + "(" + n.UsID + ")(" + n.ReadNum + "帖)</a>");

        //        k++;
        //        builder.Append(Out.Tab("</div>", ""));

        //    }
        //}
        //else
        //{
        //    builder.Append(Out.Div("div", "没有相关记录.."));
        //}
        //ceshi
        #endregion
        //// 开始读取列表
        ////     IList<BCW.Model.Text> list = new BCW.BLL.Text().GetForumstats1pageIndex, pageSize, strWhere, strOrder, showtype, out recordCount);
        IList<BCW.Model.Forumstat> listForumstat = new BCW.BLL.Forumstat().GetForumstats(pageIndex, pageSize, strWhere, strOrder, showtype, out recordCount);
        if (listForumstat.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Forumstat n in listForumstat)
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

                //builder.AppendFormat("{0}.", (pageIndex - 1) * pageSize + k);

                builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(n.UsID) + "(" + n.UsID + ")(" + n.tTotal + "帖)</a>");

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
        if (forumid > 0)
            builder.Append("<a href=\"" + Utils.getPage("forum.aspx?forumid=" + forumid + "") + "\">上级</a>");
        else
            builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>");

        builder.Append("-<a href=\"" + Utils.getUrl("forumstat.aspx?act=top&amp;forumid=" + forumid + "") + "\">排行榜</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 排行 ForumTopPage
    private void ForumTopPage()
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-1]$", "0"));
        int year = int.Parse(Utils.GetRequest("year", "all", 1, @"^[0-9]\d*$", "" + DateTime.Now.Year + ""));
        int month = int.Parse(Utils.GetRequest("month", "all", 1, @"^[0-9]\d*$", "" + DateTime.Now.Month + ""));

        if (ptype == 0)
        {
            Master.Title = "主题论坛排行";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("=主题论坛排行=");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {

            Master.Title = "圈子论坛排行";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("=圈子论坛排行=");
            builder.Append(Out.Tab("</div>", "<br />"));
        }

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("forumstat.aspx?act=forumtop&amp;ptype=" + ptype + "") + "\">本月</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("forumstat.aspx?act=forumtop&amp;year=1&amp;month=1&amp;ptype=" + ptype + "") + "\">总排行</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("forumstat.aspx?act=forumtopsr&amp;ptype=" + ptype + "") + "\">搜索排行</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string[] pageValUrl = { "act", "ptype", "year", "month", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;


        string strWhere = "";
        if (year != 1 && month != 1)
        {
            strWhere = " Year(AddTime)=" + year + " AND Month(AddTime) = " + month + " ";

            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("=" + year + "年" + month + "月排行=");
            builder.Append(Out.Tab("</div>", "<br />"));
        }


        // 开始读取列表
        IList<BCW.Model.Forumstat> listForumstat = new BCW.BLL.Forumstat().GetForumstats(pageIndex, pageSize, ptype, strWhere, out recordCount);
        if (listForumstat.Count > 0)
        {

            int k = 1;
            foreach (BCW.Model.Forumstat n in listForumstat)
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

                builder.AppendFormat("[第{0}名]", (pageIndex - 1) * pageSize + k);

                builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + n.ForumID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.Forum().GetTitle(n.ForumID) + "(发帖+回帖共" + n.tTotal + ")</a>");

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
        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        if (ptype == 0)
        {
            builder.Append("<a href=\"" + Utils.getUrl("forumstat.aspx?act=forumtop&amp;ptype=1&amp;year=" + year + "&amp;month=" + month + "") + "\">圈子论坛排行&gt;&gt;</a>");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("forumstat.aspx?act=forumtop&amp;ptype=0&amp;year=" + year + "&amp;month=" + month + "") + "\">主题论坛排行&gt;&gt;</a>");
        }
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">社区</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 搜索排行 ForumTopSearchPage
    private void ForumTopSearchPage()
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-1]$", "0"));

        Master.Title = "搜索论坛排行";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("=搜索论坛排行=");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "类型:,年份:,月份:,,";
        string strName = "pptype,year,month,ptype,act";
        string strType = "select,select,select,hidden,hidden";
        string strValu = "0'" + DateTime.Now.Year + "'" + DateTime.Now.Month + "'" + ptype + "'forumtop";
        string strEmpt = "0|主题论坛|1|圈子论坛,2011|2011|2012|2012|2013|2013|2014|2014|2015|2015|2016|2016|2017|2017|2018|2018,1|1月|2|2月|3|3月|4|4月|5|5月|6|6月|7|7月|8|8月|9|9月|10|10月|11|11月|12|12月,false,false";
        string strIdea = "/";
        string strOthe = "搜索,forumstat.aspx,post,1,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        if (ptype == 0)
        {
            builder.Append("<a href=\"" + Utils.getUrl("forumstat.aspx?act=forumtop&amp;ptype=1") + "\">圈子论坛排行&gt;&gt;</a>");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("forumstat.aspx?act=forumtop&amp;ptype=0") + "\">主题论坛排行&gt;&gt;</a>");
        }
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">社区</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

}