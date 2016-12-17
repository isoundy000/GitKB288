using System;
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
using TPR.Model.Http;
using BCW.Common;
using TPR.Common;
using System.Collections.Generic;

public partial class bbs_guess2_live : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    //篮球收藏开关 1开 0关闭
    string BasketBallCollect = (ub.GetSub("BasketBallCollect", "/Controls/footballs.xml"));
    string Open = (ub.GetSub("Open", "/Controls/footballs.xml"));
    string ZQOpen = (ub.GetSub("ZQOpen", "/Controls/footballs.xml"));
    string LQOpen = (ub.GetSub("LQOpen", "/Controls/footballs.xml"));
    protected string Strbuilder = string.Empty;
    protected string strText = string.Empty;
    protected string strName = string.Empty;
    protected string strType = string.Empty;
    protected string strValu = string.Empty;
    protected string strEmpt = string.Empty;
    protected string strIdea = string.Empty;
    protected string strOthe = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "比分直播中心";

        int act = Utils.ParseInt(Utils.GetRequest("act", "all", 1, "", ""));
        int kind = Utils.ParseInt(Utils.GetRequest("kind", "get", 1, @"^[0-1]$", "1"));
        int State = Utils.ParseInt(Utils.GetRequest("State", "get", 1, @"^[0-2]$", ""));
        int SClassID = Utils.ParseInt(Utils.GetRequest("SClassID", "get", 1, @"^[0-9]\d*$", ""));
        int itype = Utils.ParseInt(Utils.GetRequest("type", "get", 1, @"^[0-9]\d*$", "1"));
        string SClass = Utils.GetRequest("SClass", "get", 1, "", "");
        string day = Utils.GetRequest("day", "all", 1, "", "");
        switch (act)
        {
            case 1:
                Getzlist(kind);
                break;
            case 2:
                Getzlist2(1);
                break;
            case 3:
                Getzlist2(2);
                break;
            case 4:
                GetScheduleAll();
                break;
            case 5:
                GetSchedule(kind, SClassID, SClass);
                break;
            case 6:
                GetSclassseach(day, 1);
                break;
            case 7:
                GetSclassseach(day, 2);
                break;
            case 8:
                GetSclassseachInfo(day, act, SClassID);
                break;
            case 9:
                GetSclassseachInfo(day, act, SClassID);
                break;
            case 10:
                Getllist();
                break;
            case 11:
                Getvlist(SClassID, SClass);
                break;
            case 12:
                Gettlist();
                break;
            case 13:
                Getllist2(itype);
                break;
            case 14:
                GetnbaSclassseach(day, itype);
                break;
            case 15:
                GetnbaScheduleseach(day, itype, SClassID, SClass);
                break;
            case 16:
                GetNbaWord();
                break;
            case 17:
                GetNbaWordPage();
                break;
            case 18:
                GetNbaWordView();
                break;
            case 19:
                GetWNbaWordPage();
                break;
            case 20:
                GetNba2Page();
                break;
            case 21://具体某场比赛记录
                GetAGameData();
                break;
            case 22://具体类场比赛记录
                GetAStyleData();
                break;
            case 23://具体某天比赛记录
                GetADayData();
                break;
            case 24://具体某天比赛记录
                GetATeamData();
                break;
            case 25://收藏某场比赛记录
                FavoriteGame();
                break;
            case 26://收藏某场比赛记录
                MyFavoriteGame();
                break;
            case 27://分类查看
                SplitGame();
                break;
            //足球结束
            //篮球开始
            case 28://篮球即时比分
                BasketNow();
                break;
            case 29://篮球一场比分查看
                BasketMatch();
                break;
            case 30://分类查看
                SplitBaskGame();
                break;
            case 31://篮球收藏
                FavoriteBacketGame();
                break;
            case 32://取消收藏  邵广林 20161004
                quxiaosc();
                break;
            default:
                ReloadPage();
                break;
        }
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append(Out.waplink(Utils.getUrl("myGuess.aspx?ptype=1"), "未开投注") + " ");
        builder.Append(Out.waplink(Utils.getUrl("myGuess.aspx?ptype=2"), "历史投注") + "<br />");
        if (act != 0)
        {
            builder.Append(Out.waplink(Utils.getUrl("live.aspx"), "比分直播中心") + "<br />");
        }
        builder.Append(Out.waplink(Utils.getUrl("default.aspx"), "返回球彩首页") + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        //FaveriteTop(1);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(Out.waplink(Utils.getUrl("/default.aspx"), "首页") + "-");
        builder.Append(Out.waplink(Utils.getPage("live.aspx"), "上级") + "-");
        builder.Append(Out.waplink(Utils.getUrl("default.aspx"), "球彩") + "");
        builder.Append(Out.Tab("</div>", ""));
    }

    //live 主页
    private void ReloadPage()
    {
        //维护提示 0维护 1正常
        if (Open == "0")
        {
            Utils.Safe("此游戏");
        }
        DateTime now = DateTime.Now;
        int n = (int)now.DayOfWeek;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(Out.waplink(Utils.getUrl("default.aspx"), "竞猜-") + "");
        builder.Append("比分直播中心");
        builder.Append(Out.Tab("</div>", "<br />"));
        if (ZQOpen == "1")
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("<b>足球</b>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("" + Out.waplink(Utils.getUrl("live.aspx?act=1"), "即时比分"));
            builder.Append("<br />" + Out.waplink(Utils.getUrl("live.aspx?act=1&amp;State=2&amp;"), "完场赛事"));
            builder.Append("<br />" + Out.waplink(Utils.getUrl("live.aspx?act=1&amp;State=0&amp;"), "一周赛程") + "");
            builder.Append("<br />" + Out.waplink(Utils.getUrl("live.aspx?act=27&amp;State=0&amp;"), "赛事筛选") + "");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        if (LQOpen == "1")
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("<b>篮球</b>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("" + Out.waplink(Utils.getUrl("live.aspx?act=16"), "NBA直播"));
            builder.Append("<br />" + Out.waplink(Utils.getUrl("live.aspx?act=28"), "即时比分"));
            builder.Append("<br />" + Out.waplink(Utils.getUrl("live.aspx?act=30&amp;type=1"), "赛事筛选"));
            builder.Append("<br />" + Out.waplink(Utils.getUrl("live.aspx?act=28&amp;type=2&amp;State=3&amp;week=" + n + "&amp;"), "一周赛程") + "");
            builder.Append(Out.Tab("</div>", ""));
        }
        //builder.Append(Out.Tab("<div>", "<br />"));
        //builder.Append("比分数据采集于互联网，仅用于球迷参考或虚拟游戏，如侵犯您的利益请联系本站工作人员.");
        //builder.Append(Out.Tab("</div>", ""));
    }

    #region 足球开始
    //联赛名字
    private void SplitGame()
    {
        string Style = Utils.GetRequest("Style", "all", 1, "", "");
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(Out.waplink(Utils.getUrl("live.aspx"), "直播中心-"));
        builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=1&amp;"), "足球比分-"));
        builder.Append("分类选择");
        builder.Append(Out.Tab("</div>", "<br/>"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        string strWhere = " Id>0 and  Identification=1 ";
        DataSet ds = new BCW.BLL.tb_ZQLists().GetList(" distinct ft_teamStyle ", strWhere);
        int pageIndex;
        int recordCount;
        int pageSize = 30;// Convert.ToInt32(ub.Get("SiteListNo"));
        string[] pageValUrl = { "act", "ptype", "id", "backurl", "Style" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            recordCount = ds.Tables[0].Rows.Count;
            int k = 1;
            int koo = (pageIndex - 1) * pageSize;
            int skt = pageSize;
            if (recordCount > koo + pageSize)
            {
                skt = pageSize;
            }
            else
            {
                skt = recordCount - koo;
            }
            for (int i = 0; i < skt; i++)
            {
                //if (i % 2 == 0)
                //{ builder.Append(Out.Tab("<div >", "")); }
                //else
                //{
                //    if (i == 1)
                //        builder.Append(Out.Tab("<div class=\"text\">", ""));
                //    else
                //        builder.Append(Out.Tab("<div class=\"text\">", ""));
                //}
                builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=22&amp;Style=" + ds.Tables[0].Rows[koo + i]["ft_teamStyle"].ToString() + ""), "[" + ds.Tables[0].Rows[koo + i]["ft_teamStyle"].ToString() + "]"));
                // builder.Append(Out.Tab("</div>", ""));
                builder.Append("&nbsp;&nbsp;");
                if ((i + 1) % 3 == 0)
                    builder.Append("<br/>");


            }
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Tab("<div>", "<br/><br/>"));
            builder.Append("无相关球赛记录");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        // builder.Append("分类选择");
        builder.Append(Out.Tab("</div>", ""));
    }

    //收藏的最新几条消息
    private void FaveriteTop(int max)
    {
        int idd = new BCW.BLL.tb_ZQCollection().GetMaxId() - 1;
        for (int i = 0; i < max; i++)
        {
            BCW.Model.tb_ZQCollection cc = new BCW.BLL.tb_ZQCollection().Gettb_ZQCollection(idd--);
            builder.Append(Out.Tab("<div>", "<br/>"));
            builder.Append(DT.DateDiff2(DateTime.Now, Convert.ToDateTime(cc.AddTime)) + "前" + new BCW.BLL.User().GetUsName(Convert.ToInt32(cc.UsId)) + "");
            builder.Append("收藏了" + "<a  href=\"" + Utils.getUrl("live.aspx?act=21&amp;Id=" + cc.FootBallId + "&amp;") + "\">" + cc.team1 + "VS" + cc.team2 + "</a>");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
    }

    //1进行 未开 完场赛事
    private void Getzlist(int kind)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(Out.waplink(Utils.getUrl("default.aspx"), "体育竞猜-") + "");
        builder.Append(Out.waplink(Utils.getUrl("live.aspx"), "直播中心-"));
        builder.Append("足球比分");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        int State = Utils.ParseInt(Utils.GetRequest("State", "all", 1, "", "1"));
        //  string date = (Utils.GetRequest("date", "all", 1, "", DateTime.Now.ToString("yyyyMMdd HH:mm:ss")));
        string date = (Utils.GetRequest("date", "all", 1, "", ""));
        string title = "";
        string textout = "";
        string strWhere = "";
        string textwhere = "";
        string textresult = "";
        if (State == 1)
        {
            title = "进行赛事";//identification 1显示 0隐藏
            strWhere = " Identification=1 and ft_state!='完' and ft_state!='推迟'  and ft_state!='待定' and convert(datetime,ft_time+convert(datetime,ft_caipan,8),120)< DATEADD(hour,7,GETDATE()) and convert(datetime,ft_time+convert(datetime,ft_caipan,8),120)> DATEADD(hour,-3,GETDATE()) ORDER BY convert(datetime,ft_time,120) ASC,convert(datetime,ft_caipan,120) ASC ";
            textout = "直播";
            builder.Append("即 时 |");
            textwhere = "暂无更多进行赛事.";
        }
        else
        {
            builder.Append(Out.waplink(Utils.getUrl("live.aspx?ptype=1&amp;act=1&amp;kind=" + kind + "&amp;State=1"), "即 时 |") + "");
        }
        if ((State == 2))
        {
            builder.Append(" 赛 果 |");
            title = "完场赛事";
            strWhere = "Identification=1 and ft_state='完' ORDER BY convert(datetime,ft_time,120) DESC,convert(datetime,ft_caipan,120) DESC";
            textout = "完";
            textwhere = "暂无更多完场赛事.";
        }
        else
        {
            builder.Append(Out.waplink(Utils.getUrl("live.aspx?ptype=1&amp;act=1&amp;kind=" + kind + "&amp;State=2&amp;date=" + DateTime.Now.AddDays(-2).ToString() + "&amp;"), " 赛 果 |") + "");
        }
        if (State == 0)
        {
            title = "未开赛事";
            strWhere = " Identification=1 and  ft_state='未' and convert(datetime,ft_time+convert(datetime,ft_caipan,8),120)> DATEADD(hour,-1,GETDATE()) ORDER BY convert(datetime,ft_time,120) ASC,convert(datetime,ft_caipan,120) ASC";
            textout = " 赛 程 ";
            builder.Append(" 赛 程");
            textwhere = "暂无更多未开赛事.";
            // strWhere = "p_once!='完' and p_type=1";
        }
        else
        {
            builder.Append(Out.waplink(Utils.getUrl("live.aspx?ptype=1&amp;act=1&amp;kind=" + kind + "&amp;State=0"), " 赛 程") + "");
        }
        if (State == 4)
        {
            if (BasketBallCollect == "1")
            {
                title = "收藏比赛";
                strWhere = "  convert(int,matchstate)=-1 and datediff(dd,matchtime,GETDATE())=0  ORDER BY convert(datetime,matchtime,120) desc";
                textout = "未";
                builder.Append("| 收 藏");
                textwhere = "暂无更多未开赛事.";
            }
        }
        else
        {
            if (BasketBallCollect == "1")
            {
                builder.Append(Out.waplink(Utils.getUrl("live.aspx?ptype=1&amp;act=1&amp;kind=" + kind + "&amp;State=4"), "| 收 藏 ") + "");
            }
        }

        builder.Append(Out.Tab("</div>", ""));

        #region 二级导航
        //if ((State == 2))
        //{
        //    builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
        //    builder.Append(Out.waplink(Utils.getUrl("live.aspx?ptype=2&amp;act=28&amp;kind=" + kind + "&amp;State=2&amp;date=" + Convert.ToDateTime(date).AddDays(-1).ToString() + "&amp;"), Convert.ToDateTime(date).AddDays(-1).ToString("yyyy-MM-dd")) + " | ");
        //    builder.Append("<b>" + Convert.ToDateTime(date).ToString("yyyy-MM-dd") + "</b>" + " | ");
        //    if (Convert.ToDateTime(date) > DateTime.Now.AddDays(-1))
        //    {
        //        builder.Append(Convert.ToDateTime(date).AddDays(1).ToString("yyyy-MM-dd"));
        //    }
        //    else
        //        builder.Append(Out.waplink(Utils.getUrl("live.aspx?ptype=2&amp;act=28&amp;kind=" + kind + "&amp;State=2&amp;date=" + Convert.ToDateTime(date).AddDays(1).ToString() + "&amp;"), Convert.ToDateTime(date).AddDays(1).ToString("yyyy-MM-dd")));
        //    builder.Append(Out.Tab("</div>", ""));
        //}
        //DateTime now = DateTime.Now;
        //int n = (int)now.DayOfWeek;
        //if (State == 0)
        //{
        //    int week = Utils.ParseInt(Utils.GetRequest("week", "all", 1, "", "1"));
        //    string[] weekDays = { "周日", "周一", "周二", "周三", "周四", "周五", "周六" };
        //    builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
        //    for (int i = 0; i < 7; i++)
        //    {
        //        if (n > 6)
        //            n = 0;
        //        if (n == week)
        //        {
        //            DateTime dd = DateTime.Now.AddDays(i + 1);
        //            DateTime dd2 = DateTime.Now.AddDays((i));
        //            builder.Append("<b>" + weekDays[n++] + "</b>");
        //            strWhere = "ft_state='未' and convert(datetime,ft_time+convert(datetime,ft_caipan,8),120)> DATEADD(hour,-1,GETDATE()) ORDER BY convert(datetime,ft_time,120) ASC,convert(datetime,ft_caipan,120) ASC";
        //        }
        //        else
        //        {
        //            builder.Append(Out.waplink(Utils.getUrl("live.aspx?ptype=2&amp;act=1&amp;kind=" + kind + "&amp;State=0amp;week=" + n + "&amp;"), weekDays[n++]));
        //        }
        //        if (i < 6)
        //        {
        //            builder.Append(" | ");
        //        }
        //    }
        //    builder.Append(Out.Tab("</div>", ""));
        //}

        #region 时间
        //时间拼接  getDate()< convert(datetime,ft_time,120)+convert(datetime,ft_caipan,120)
        //builder.Append(Out.Tab("<div class=\"text\">", ""));
        //builder.Append("足球" + title + "");
        //builder.Append(Out.Tab("</div>", "<br />"));
        //builder.Append(Out.Tab("<div>", "<br />"));
        //builder.Append("<b>" + DateTime.Now.ToString("MM月dd日") + "</b>");
        //builder.Append(Out.Tab("</div>", "<br />"));
        #endregion

        #region 旧版已注释
        //int pageIndex=0;
        //int recordCount;
        //int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        //pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        //string[] pageValUrl = { "act", "ptype", "id", "backurl", "State" };
        ////读取的SQL
        ////  strWhere = "";
        //int sp = 2;
        //if (sp == 1)
        //{
        //    strWhere += "  AND p_title= '欧锦赛'";
        //}
        //string  strOrder = "p_TPRtime ASC,ID ASC";
        //#endregion
        //if (pageIndex == 0)
        //        pageIndex = 1;
        //    #region 开始读取竞猜表
        //    // 开始读取竞猜表 tb_TBaList 按 p_TPRtime 时间排列
        //IList<TPR2.Model.guess.BaList> listBaList = new TPR2.BLL.guess.BaList().GetBaLists(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        //if (listBaList.Count > 0)
        //{
        //    #region 循环
        //    int k = 1;
        //    string strDay = "";
        //    foreach (TPR2.Model.guess.BaList n in listBaList)
        //    {
        //        if (k % 2 == 0)
        //        {
        //            builder.Append(Out.Tab("<div>", "<br />"));
        //        }
        //        else
        //        {
        //            builder.Append(Out.Tab("<div>", "<br />"));
        //        }

        //        if (DT.GetTime(n.p_TPRtime.ToString(), "MM月dd日").ToString() != strDay.ToString())
        //            builder.Append(DT.GetTime(n.p_TPRtime.ToString(), "MM月dd日") + "<br />");

        //        string Sonce = string.Empty;
        //        string VS = "VS";
        //        string hp_one = "";
        //        string hp_two = "";

        //        if (n.p_ison == 1)
        //        {
        //            Sonce = "(" + ub.Get("SiteGqText") + ")";
        //            if (n.p_result_temp1 == null && n.p_result_temp2 == null)
        //                VS = "(0-0)";
        //            else
        //                VS = "(" + n.p_result_temp1 + "-" + n.p_result_temp2 + ")";

        //            if (n.p_type == 1)
        //            {
        //                if (n.p_hp_one > 0)
        //                    hp_one = "<img src=\"/Files/sys/guess/redcard" + n.p_hp_one + ".gif\" alt=\"红" + n.p_hp_one + "\"/>";

        //                if (n.p_hp_two > 0)
        //                    hp_two = "<img src=\"/Files/sys/guess/redcard" + n.p_hp_two + ".gif\" alt=\"红" + n.p_hp_two + "\"/>";
        //            }

        //        }
        //        string actUrl = string.Empty;
        //        //if (ptype == 6)
        //        //    actUrl = "act=score&amp;";
        //       // <a  href=\"" + Utils.getUrl("live.aspx?act=21&amp;Id=" + ds.Tables[0].Rows[koo + i]["Id"] + "&amp;") + "\">"
        //        builder.AppendFormat("<a href=\"" + Utils.getUrl("showGuess.aspx?" + actUrl + "gid={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{1}[{2}]{3}" + hp_one + "" + VS + "{4}" + hp_two + "{5}</a>", n.ID, DT.FormatDate(Convert.ToDateTime(n.p_TPRtime), 13), n.p_title, n.p_one, n.p_two, Sonce);
        //        builder.Append("" + Convertp_once(n.p_once) + "" + Out.waplink(Utils.getUrl("showGuess.aspx?act=analysis&amp;gid=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + ""), "[析]"));
        //        builder.Append(Out.Tab("</div>", ""));

        //        strDay = DT.GetTime(n.p_TPRtime.ToString(), "MM月dd日").ToString();
        //        k++;
        //    }
        //    builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        //    #endregion

        //    #region 分页
        //    if (!Utils.GetTopDomain().Contains("tuhao") && !Utils.GetTopDomain().Contains("th"))
        //    {
        //        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        //        builder.Append("<b><a href=\"" + Utils.getUrl("default.aspx?showtype=1&amp;ptype=5") + "\">更多滚球竞猜记录&gt;&gt;</a></b>");
        //        builder.Append(Out.Tab("</div>", ""));
        //    }
        //    #endregion
        //}
        #endregion
        #endregion

        if (State != 4)
        {
            #region 当前使用最新版
            string datetimeout = "";
            DataSet ds = new BCW.BLL.tb_ZQLists().GetList(" * ", strWhere);
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string[] pageValUrl = { "act", "ptype", "id", "backurl", "State" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                recordCount = ds.Tables[0].Rows.Count;
                int k = 1;
                int koo = (pageIndex - 1) * pageSize;
                int skt = pageSize;
                if (recordCount > koo + pageSize)
                {
                    skt = pageSize;
                }
                else
                {
                    skt = recordCount - koo;
                }
                for (int i = 0; i < skt; i++)
                {
                    try
                    {
                        //官网是否有该场球赛，有则显示
                        // if (new TPR2.BLL.guess.BaList().ExistsByp_id(Convert.ToInt32(ds.Tables[0].Rows[koo + i]["ft_bianhao"])))
                        {
                            if (ds.Tables[0].Rows[koo + i]["ft_team1"].ToString() != "" && ds.Tables[0].Rows[koo + i]["ft_team2"].ToString() != "")
                            {
                                if (i % 2 == 0)
                                { builder.Append(Out.Tab("<div >", "<br/>")); }
                                else
                                {
                                    if (i == 1)
                                        builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
                                    else
                                        builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
                                }
                                if (datetimeout != Convert.ToDateTime(ds.Tables[0].Rows[koo + i]["ft_time"]).ToString("MM月dd日"))
                                {
                                    datetimeout = Convert.ToDateTime(ds.Tables[0].Rows[koo + i]["ft_time"]).ToString("MM月dd日");
                                    builder.Append("<b>" + datetimeout + "</b><br/>");
                                }
                                if (new BCW.BLL.tb_ZQCollection().ExistsUsIdAndFootId(meid, Convert.ToInt32(ds.Tables[0].Rows[koo + i]["Id"])))
                                {
                                    //  builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=25&amp;Id=" + Id + ""), "[已收藏]") + "");
                                    builder.Append("[已收藏].");
                                    builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=32&amp;Id=" + ds.Tables[0].Rows[koo + i]["Id"] + ""), "[取消收藏]") + "");
                                }
                                else
                                {
                                    builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=25&amp;Id=" + ds.Tables[0].Rows[koo + i]["Id"] + ""), "[收藏]") + "");
                                }
                                //TagsChecker.fix(str1)
                                //  builder.Append(Convert.ToDateTime(ds.Tables[0].Rows[koo + i]["ft_time"]).ToString("MM月dd日"));
                                builder.Append("<a  href=\"" + Utils.getUrl("live.aspx?act=21&amp;Id=" + ds.Tables[0].Rows[koo + i]["Id"] + "&amp;") + "\">" + ds.Tables[0].Rows[koo + i]["ft_caipan"] + "[" + ds.Tables[0].Rows[koo + i]["ft_teamStyle"] + "]");
                                //   builder.Append(ds.Tables[0].Rows[koo + i]["ft_caipan"] + "[" + ds.Tables[0].Rows[koo + i]["ft_teamStyle"] + "]");
                                builder.Append(BCW.Footballs.checkCodeker.fix(ds.Tables[0].Rows[koo + i]["ft_team1"].ToString()));
                                if (State == 0 || ds.Tables[0].Rows[koo + i]["ft_state"].ToString().Trim() == "未")
                                {
                                    builder.Append("VS");
                                }
                                else
                                {
                                    builder.Append("(" + "<font color=\"red\">" + ds.Tables[0].Rows[koo + i]["ft_result"] + "</font>" + ")");
                                }
                                builder.Append(BCW.Footballs.checkCodeker.fix(ds.Tables[0].Rows[koo + i]["ft_team2"].ToString()) + "</a>");
                                if (State == 1 || State == 2)
                                {
                                    builder.Append(ds.Tables[0].Rows[koo + i]["ft_state"]);
                                }
                                else
                                    if (State == 0)
                                {
                                    builder.Append("未开赛");
                                }
                                // builder.Append("<br/>");
                                // builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=21&amp;Id=" + ds.Tables[0].Rows[koo + i]["Id"] + "&amp;"), "[" + textout + "]") + "");
                                k++;
                                datetimeout = Convert.ToDateTime(ds.Tables[0].Rows[koo + i]["ft_time"]).ToString("MM月dd日");
                                builder.Append(Out.Tab("</div>", ""));
                            }
                        }
                    }
                    catch
                    {
                        if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))//u=" + Utils.getstrU() + "&amp;
                        {
                            Utils.Success("温馨提示", "使用彩版，页面更直观，更快捷！正在切换进入...", "live.aspx?act=1&amp;ve=2a&amp;u=" + Utils.getstrU() + "", "1");
                        }
                    }
                }
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Tab("<div>", "<br/><br/>"));
                builder.Append(textwhere);
                builder.Append(Out.Tab("</div>", "<br/>"));
            }
            //if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows.Count < 3)
            //{
            //    builder.Append(Out.Tab("<div>", "<br/>"));
            //    builder.Append(textwhere);
            //    builder.Append(Out.Tab("</div>", "<br/>"));
            //}
            if ((State == 2))//&& ds.Tables[0].Rows.Count > 0
            {
                if (date == "")
                {
                    date = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                }
                date = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                //builder.Append("【赛事速查】");
                string strText1 = "【赛事速查】/日期:,";
                string strName1 = "date,backurl";
                string strType1 = "date,hidden";
                string strValu1 = date + "'" + Utils.getPage(0) + "";
                string strEmpt1 = "true,false";
                string strIdea1 = "";
                string strOthe1 = "确定搜索,live.aspx?act=23&amp;,post,1,red";
                builder.Append(Out.wapform(strText1, strName1, strType1, strValu1, strEmpt1, strIdea1, strOthe1));
            }
            #endregion
        }
        else
        {
            //足球收藏列表
            int UsId = new BCW.User.Users().GetUsId();
            if (UsId == 0)
                Utils.Login();
            strWhere = "UsId=" + UsId + " order by AddTime DESC";
            DataSet ds = new BCW.BLL.tb_ZQCollection().GetList(" * ", strWhere);
            int pageIndex;
            int recordCount = 1;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string[] pageValUrl = { "act", "ptype", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                recordCount = ds.Tables[0].Rows.Count;
                int k = 1;
                int koo = (pageIndex - 1) * pageSize;
                int skt = pageSize;
                if (recordCount > koo + pageSize)
                {
                    skt = pageSize;
                }
                else
                {
                    skt = recordCount - koo;
                }
                for (int i = 0; i < skt; i++)
                {
                    // try
                    {
                        if (i % 2 == 0)
                        {
                            builder.Append(Out.Tab("<div >", "<br/>"));
                        }
                        else
                        {
                            if (i == 1)
                                builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
                            else
                                builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
                        }
                        builder.Append(DT.DateDiff2(DateTime.Now, Convert.ToDateTime(ds.Tables[0].Rows[koo + i]["AddTime"])) + "前收藏了");
                        builder.Append("<a  href=\"" + Utils.getUrl("live.aspx?act=21&amp;Id=" + ds.Tables[0].Rows[koo + i]["FootBallId"] + "") + "\">");
                        builder.Append(BCW.Footballs.checkCodeker.fix(ds.Tables[0].Rows[koo + i]["team1"].ToString()));
                        string result = new BCW.BLL.tb_ZQLists().GetResultFromId(Convert.ToInt32(ds.Tables[0].Rows[koo + i]["FootBallId"]));
                        if (result.Contains("-"))
                            builder.Append("(" + "<font color=\"red\">" + result + "</font>)");
                        else
                            builder.Append("<font color=\"red\">" + "VS" + "</font>");
                        builder.Append(BCW.Footballs.checkCodeker.fix(ds.Tables[0].Rows[koo + i]["team2"].ToString()) + "</a>");
                        if (!result.Contains("-"))
                        { builder.Append("未开赛"); }
                        k++;
                        builder.Append(Out.Tab("</div>", ""));
                    }
                    //catch
                    //{
                    //    if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
                    //    {
                    //        Utils.Success("温馨提示", "使用彩版，页面更直观，更快捷！正在切换进入...", "live.aspx?act=26&amp;ve=2a&amp;page=" + pageIndex + "&amp;u=" + Utils.getstrU() + "", "1");
                    //    }
                    //}
                }
            }
            else
            {
                builder.Append(Out.Tab("<div>", "<br/>"));
                builder.Append("暂无收藏哦！快去参与球赛收藏吧！");
                builder.Append(Out.Tab("</div>", "<br/>"));
            }
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }

        #region 原来分页
        //int pageIndex;
        //int recordCount;
        //int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));

        //string[] pageValUrl = { "act", "backurl", "State" };
        //pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        //if (pageIndex == 0)
        //    pageIndex = 1;
        //// 开始读取列表
        //IList<BCW.Model.tb_ZQLists> listSSCpay = new BCW.BLL.tb_ZQLists().Gettb_ZQListss2(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        //if (listSSCpay.Count > 0)
        //{
        //    int k = 1;
        //    foreach (BCW.Model.tb_ZQLists n in listSSCpay)
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

        //        builder.Append(Convert.ToDateTime(n.ft_time).ToString("MM月dd日") +"<a  href=\"" + Utils.getUrl("live.aspx?act=21&amp;Id=" + n.Id + "&amp;") + "\">" + n.ft_caipan + "[" + n.ft_teamStyle + "]" + n.ft_team1 + "(" + n.ft_result + ")VS" + n.ft_team2.Trim() + "</a>");
        //        builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=21&amp;Id=" + n.Id + "&amp;"), "[" + textout + "]") + "");
        //        builder.Append(Out.Tab("</div>", "<br />"));
        //    }
        //    // 分页
        //    builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        //}
        //else
        //{
        //    builder.Append(Out.Tab("<div>", ""));
        //    builder.Append("暂无进行赛事.");
        //    builder.Append(Out.Tab("</div>", "<br />"));
        //}
        #endregion

    }

    //正则取中文
    private string GetAGameText(string text)
    {
        string strpattern1 = @"[\u4e00-\u9fa5]+";
        Match mtitle12 = Regex.Match(text, strpattern1, RegexOptions.IgnoreCase);
        text = mtitle12.Groups[0].Value.Trim();
        return text;
    }

    //发言保存
    private void SaveSpeak(string Content, int img, int Id)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        int BallSpeakTime = Convert.ToInt32((ub.GetSub("BallSpeakTime", "/Controls/footballs.xml")));
        //< IMG alt = load src = "/Files/Face/9.gif" >
        if (Content.Length > 1 || img > 0)
        {
            string img2 = Content;
            if (img > 0)
            {
                if (Content == "")
                { img2 = "<IMG alt =\"load\"" + " src =\"/Files/Face/" + img + ".gif" + "\"/>"; }
                else
                {
                    img2 = Content + " <IMG alt =\"load\"" + " src =\"/Files/Face/" + img + ".gif" + "\"/>";
                }
            }
            if (img2.Length > BallSpeakTime)
            {
                Utils.Success("发言失败！", "一口气发言过长啦！发言失败！1s后返回..", Utils.getUrl("live.aspx?act=21&amp;Id=" + Id + ""), "1");
            }
            if (new BCW.BLL.tb_ZQChact().GetSecond(meid) < 30)
            {
                Utils.Success("发言失败！", "一口气发言过快啦！先踹口气！1s后返回..", Utils.getUrl("live.aspx?act=21&amp;Id=" + Id + ""), "1");
            }

            BCW.Model.tb_ZQChact model = new BCW.Model.tb_ZQChact();
            model.AddTime = DateTime.Now;
            model.ident = "1";
            model.isHit = 1;//1发言成功 0发言失败
            model.TextContent = img2;
            model.toFootID = Id;
            model.toUsId = 0;
            model.UsId = meid;
            if (model.TextContent.Trim() != "")
            {
                new BCW.BLL.tb_ZQChact().Add(model);
                Utils.Success("发言成功！", "发言成功！2s后返回..", Utils.getUrl("live.aspx?act=21&amp;Id=" + Id + ""), "1");
            }
            else
            { Utils.Success("发言失败！", "发言不能为空！发言失败！2s后返回..", Utils.getUrl("live.aspx?act=21&amp;Id=" + Id + ""), "1"); }
        }
        else
        { Utils.Success("发言失败！", "发言不能为空！发言失败！2s后返回..", Utils.getUrl("live.aspx?act=21&amp;Id=" + Id + ""), "1"); }
    }

    //21 一场比赛的具体数据
    private void GetAGameData()
    {
        // builder.Append( new BCW.BLL.tb_ZQChact().GetSecond(727));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(Out.waplink(Utils.getUrl("live.aspx"), "直播中心-"));
        builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=1&amp;"), "足球比分-"));
        builder.Append("即时比分");
        builder.Append(Out.Tab("</div>", "<br />"));
        string xmlPath = "/Controls/footballs.xml";
        //聊天0全部1每场
        string BallSpeak = (ub.GetSub("BallSpeak", "/Controls/footballs.xml"));
        //赔率开关0关1开
        string BallPeiLu = (ub.GetSub("BallPeiLu", "/Controls/footballs.xml"));
        //球场数据开关0关1开
        string BallQiuChang = (ub.GetSub("BallQiuChang", "/Controls/footballs.xml"));
        //进球时刻开关0关1开
        string BallJinQiu = (ub.GetSub("BallJinQiu", "/Controls/footballs.xml"));
        //聊天开关0关1开
        string BallOpenSpeak = (ub.GetSub("BallOpenSpeak", "/Controls/footballs.xml"));

        int Id = Utils.ParseInt(Utils.GetRequest("Id", "all", 1, "", ""));
        string speak = Utils.GetRequest("speak", "all", 1, "", "no");
        // string Content = Utils.GetRequest("Content", "all", 2, @"^[^\^]{0,30}$", "发言内容限30字");
        string Content = Utils.GetRequest("Content", "all", 1, "", "");
        int img = Utils.ParseInt(Utils.GetRequest("img", "all", 1, "", "0"));
        if (!new BCW.BLL.tb_ZQLists().Exists(Id))
        {
            Utils.Success("不存在的记录！", "出错啦！不存在该球赛记录！1s后返回..", Utils.getUrl("live.aspx?act=1&amp;Id=" + Id + ""), "1");
        }
        builder.Append(Out.Tab("<div>", ""));
        BCW.Model.tb_ZQLists model = new BCW.BLL.tb_ZQLists().Gettb_ZQLists(Id);
        new BCW.BLL.tb_ZQLists().UpdateHit(Id);
        if (model.Identification == 0)
        {
            Utils.Success("不存在的记录！", "出错啦！不存在该球赛记录！1s后返回..", Utils.getUrl("live.aspx?act=1&amp;Id=" + Id + ""), "1");
        }
        if (speak == "yes")
        {
            if (Content != "" || img > 0)
            {
                SaveSpeak(Content, img, Id);
            }
        }
        //  Master.Title = model.ft_team1+"VS"+model.ft_team2;
        builder.Append("<a href=\"" + Utils.getUrl("live.aspx?act=22&amp;Style=" + model.ft_teamStyle + "") + "\">" + model.ft_teamStyle + " </a> " + "@ " + Convert.ToDateTime(model.ft_time).ToString("MM月dd日") + "&nbsp; " + model.ft_caipan);
        if (new BCW.BLL.tb_ZQCollection().ExistsUsIdAndFootId(meid, Id))
        {
            //  builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=25&amp;Id=" + Id + ""), "[已收藏]") + "");
            builder.Append("[已收藏].");
            builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=32&amp;Id=" + Id + ""), "[取消收藏]") + "");
        }
        else
        {
            builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=25&amp;Id=" + Id + ""), "[收藏]") + "");
        }
        builder.Append("<br />主客队:");
        builder.Append("<a href=\"" + Utils.getUrl("live.aspx?act=24&amp;team=" + GetAGameText(model.ft_team1) + "") + "\">" + model.ft_team1 + " </a>");
        builder.Append("-");
        builder.Append("<a href =\"" + Utils.getUrl("live.aspx?act=24&amp;team=" + GetAGameText(model.ft_team2) + "") + "\">" + model.ft_team2 + " </a>");
        builder.Append("<br />比赛状态:" + Convertp_once(model.ft_state) + "");
        builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=21&amp;Id=" + Id + ""), " [刷]") + "");

        if (model.ft_result != "")
            builder.Append("<br />即时比分:" + "<font color=\"red\">" + model.ft_result + "</font>");
        builder.Append(Out.Tab("</div>", "<br/>"));
        if (BallPeiLu == "1")
        {
            if (model.ft_team1Explain.Length > 5)
            {
                // builder.Append("<br/>");
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("【赔率数据】");
                builder.Append(Out.Tab("</div>", "<br/>"));
                //  builder.Append("<div>" + model.ft_otherNews+ "</div>");
                //  builder.Append("<div>" + model.ft_team1Explain + "</div>");
                builder.Append(Out.Tab("<div>", ""));
                string[] ss = model.ft_team1Explain.Split('↑');
                try
                {
                    for (int i = 0; i < ss.Length; i++)
                    {
                        builder.Append(ss[i] + " <br/>");
                    }
                }
                catch
                {
                    if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))//u=" + Utils.getstrU() + "&amp;
                    {
                        Utils.Success("温馨提示", "使用彩版，页面更直观，更快捷！正在切换进入...", "live.aspx?act=21&amp;Id=" + Id + "&amp;ve=2a&amp;u=" + Utils.getstrU() + "", "1");
                    }
                }
                // <b>初盘</b>↑让球0.91 <i>一/球半</i> 0.85↑大小0.85 <i>3</i> 0.91↑标准1.39 4.50 5.60</table>
                // builder.Append("<div>" + model.ft_team2Explain + "</div>");
                string[] ss1 = model.ft_team2Explain.Split('↑');
                try
                {
                    for (int i = 0; i < ss.Length; i++)
                    {
                        builder.Append(ss1[i] + "<br/>");
                    }
                }
                catch
                {
                    if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))//u=" + Utils.getstrU() + "&amp;
                    {
                        Utils.Success("温馨提示", "使用彩版，页面更直观，更快捷！正在切换进入...", "live.aspx?act=21&amp;Id=" + Id + "&amp;ve=2a&amp;u=" + Utils.getstrU() + "", "1");
                    }
                }
                //  builder.Append("<div>" + model.ft_state1 + "</div>");
                string[] ss2 = model.ft_state1.Split('↑');
                try
                {
                    for (int i = 0; i < ss.Length; i++)
                    {
                        builder.Append(ss2[i] + "<br/>");
                    }
                }
                catch
                {
                    if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))//u=" + Utils.getstrU() + "&amp;
                    {
                        Utils.Success("温馨提示", "使用彩版，页面更直观，更快捷！正在切换进入...", "live.aspx?act=21&amp;Id=" + Id + "&amp;ve=2a&amp;u=" + Utils.getstrU() + "", "1");
                    }
                }
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        if (BallJinQiu == "1")
        {
            if (model.ft_state2.Length > 5)
            {
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("【进球时刻】");
                builder.Append(Out.Tab("</div>", "<br/>"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append(model.ft_state2);
                builder.Append(Out.Tab("</div>", ""));
            }
            //else { builder.Append("<br/>" + "进球时刻:" + "" + "暂无收录." + ""); }
        }
        if (BallQiuChang == "1")
        {
            if (model.ft_news.Length > 5)
            {
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("【球场数据】");
                builder.Append(Out.Tab("</div>", "<br/>"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append(model.ft_news);
                builder.Append(Out.Tab("</div>", "<br/>"));
            }
            //else
            //{
            //    //builder.Append("<div>" + "球场数据:" + "" + "暂无收录." + "</div>"); 
            //}
        }
        //聊天开关
        if (BallOpenSpeak == "1")
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("【球赛讨论】");
            builder.Append(Out.Tab("</div>", "<br/>"));
            strText = ",,,,";
            strName = "Content,img,Id,act,backurl";
            strType = "text,select,hidden,hidden,hidden";
            strValu = "" + "'" + "'" + Id + "'" + 21 + "'" + Utils.getPage(0) + "";
            strEmpt = "true,0|表情|1|微笑|2|哭泣|3|调皮|4|脸红|5|害羞|6|生气|7|偷笑|8|流汗|9|我倒|10|得意|11|呲牙|12|疑问|13|睡觉|14|好色|15|口水|16|敲头|17|可爱|18|猪头|19|胜利|20|玫瑰|21|吃药|22|白酒|23|可乐|24|囧囧|25|亲亲|26|抱抱,false,false,false";
            strIdea = "";
            strOthe = "快速发言,live.aspx?&amp;speak=yes,post,3,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            string swhere = "";
            if (BallSpeak == "0")//0全部1每队
            {
                swhere = " isHit=1 order by  AddTime DESC";
            }
            else
            {
                swhere = "toFootId=" + Id + " and isHit=1 order by  AddTime DESC";
            }
            DataSet ds = new BCW.BLL.tb_ZQChact().GetList(" * ", swhere);
            int pageIndex;
            int recordCount;
            int pageSize = 5;
            string[] pageValUrl = { "act", "Id", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                recordCount = ds.Tables[0].Rows.Count;
                int k = 1;
                int koo = (pageIndex - 1) * pageSize;
                int skt = pageSize;
                if (recordCount > koo + pageSize)
                {
                    skt = pageSize;
                }
                else
                {
                    skt = recordCount - koo;
                }

                int tt = 0;
                for (int i = 0; i < skt; i++)
                {
                    if (skt < pageSize)
                    {
                        tt = skt - i;
                    }
                    else
                    {
                        tt = ds.Tables[0].Rows.Count - skt * (pageIndex - 1) - i;
                    }
                    builder.Append(Out.Tab("<div float=\"left\">", "<br/>"));
                    builder.Append(tt + "楼.");
                    if (Convert.ToInt32(ds.Tables[0].Rows[koo + i]["UsId"]) == meid)
                    { builder.Append("<b>" + "我" + "说:</b>"); }
                    else
                    {
                        builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + Convert.ToInt32(ds.Tables[0].Rows[koo + i]["UsId"]) + "") + "\">" + new BCW.BLL.User().GetUsName(Convert.ToInt32(ds.Tables[0].Rows[koo + i]["UsId"])) + "</a>");
                        builder.Append(":");
                    }
                    builder.Append((ds.Tables[0].Rows[koo + i]["TextContent"]).ToString().Trim());
                    builder.Append("&nbsp; [" + Convert.ToDateTime(ds.Tables[0].Rows[koo + i]["Addtime"]).ToString("HH:mm") + "]");
                    builder.Append(Out.Tab("</div>", ""));
                    //  builder.Append("<br/>");
                    k++;
                }

                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                //无记录
            }
        }
        else
            if (BallSpeak == "2")
        {
            // 闲聊显示
            // builder.Append(Out.Tab("<div>", ""));
            //builder.Append(Out.SysUBB(BCW.User.Users.ShowSpeak(6,"live.aspx",5, 0)));
            builder.Append((BCW.User.Users.ShowSpeak(21, "live.aspx?act=21&amp;Id=" + Id + "", 5, 0)));
            // builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div>", "<br/>"));
        builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=26&amp;Id=" + Id + ""), "我的收藏") + "<br/>");
        builder.Append("<a href=\"" + Utils.getUrl("live.aspx?act=1&amp;ptype=4&amp;fly=" + 3 + "") + "\">" + "返回上级" + "</a>");
        builder.Append(Out.Tab("</div>", ""));
        //builder.Append(Out.Tab("<div>", "<br />"));
        //builder.Append("比分数据采集于互联网，仅用于球迷参考或虚拟游戏，如侵犯您的利益请联系本站工作人员.");
        //builder.Append(Out.Tab("</div>", ""));


    }

    //22 一类比赛的具体数据
    private void GetAStyleData()
    {
        string Style = Utils.GetRequest("Style", "all", 1, "", "");
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(Out.waplink(Utils.getUrl("live.aspx"), "直播中心-"));
        builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=1&amp;"), "足球比分-"));
        builder.Append(Style);
        builder.Append(Out.Tab("</div>", ""));
        string datetimeout = "";
        string strWhere = "Identification=1 and ft_teamStyle like '" + Style + "' and convert(datetime,ft_time,120)>(getDate()-1) ORDER BY convert(datetime,ft_time,120) ASC,convert(datetime,ft_caipan,120) ASC";
        DataSet ds = new BCW.BLL.tb_ZQLists().GetList(" * ", strWhere);
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string[] pageValUrl = { "act", "ptype", "id", "backurl", "Style" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            recordCount = ds.Tables[0].Rows.Count;
            int k = 1;
            int koo = (pageIndex - 1) * pageSize;
            int skt = pageSize;
            if (recordCount > koo + pageSize)
            {
                skt = pageSize;
            }
            else
            {
                skt = recordCount - koo;
            }
            for (int i = 0; i < skt; i++)
            {
                if (ds.Tables[0].Rows[koo + i]["ft_team1"].ToString() != "" && ds.Tables[0].Rows[koo + i]["ft_team2"].ToString() != "")
                {
                    try
                    {
                        if (i % 2 == 0)
                        { builder.Append(Out.Tab("<div >", "<br/>")); }
                        else
                        {
                            if (i == 1)
                                builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
                            else
                                builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
                        }
                        if (datetimeout != Convert.ToDateTime(ds.Tables[0].Rows[koo + i]["ft_time"]).ToString("MM月dd日"))
                        {
                            datetimeout = Convert.ToDateTime(ds.Tables[0].Rows[koo + i]["ft_time"]).ToString("MM月dd日");
                            builder.Append("<b>" + datetimeout + "</b><br/>");
                        }

                        builder.Append("<a  href=\"" + Utils.getUrl("live.aspx?act=21&amp;Id=" + ds.Tables[0].Rows[koo + i]["Id"] + "&amp;") + "\">" + ds.Tables[0].Rows[koo + i]["ft_caipan"] + "[" + GetRed(ds.Tables[0].Rows[koo + i]["ft_teamStyle"].ToString(), Style) + "]");
                        builder.Append(ds.Tables[0].Rows[koo + i]["ft_team1"]);
                        if (ds.Tables[0].Rows[koo + i]["ft_result"].ToString() == "")
                        {
                            builder.Append("VS");
                        }
                        else
                        {
                            builder.Append("(" + "<font color=\"red\">" + ds.Tables[0].Rows[koo + i]["ft_result"] + "</font>" + ")");
                        }
                        builder.Append(ds.Tables[0].Rows[koo + i]["ft_team2"].ToString() + "</a>");

                        // builder.Append("<br/>");
                        // builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=21&amp;Id=" + ds.Tables[0].Rows[koo + i]["Id"] + "&amp;"), "[" + textout + "]") + "");
                        k++;
                        datetimeout = Convert.ToDateTime(ds.Tables[0].Rows[koo + i]["ft_time"]).ToString("MM月dd日");

                        builder.Append(Out.Tab("</div>", ""));
                    }
                    catch
                    {
                        if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))//u=" + Utils.getstrU() + "&amp;
                        {
                            Utils.Success("温馨提示", "使用彩版，页面更直观，更快捷！正在切换进入...", "live.aspx?act=22&amp;ve=2a&amp;Style=" + Style + "&amp;u=" + Utils.getstrU() + "", "1");
                        }
                    }
                }
            }
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Tab("<div>", "<br/><br/>"));
            builder.Append("无相关球赛记录");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        builder.Append(Out.Tab("<div >", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("live.aspx?act=1&amp;") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div >", ""));
    }

    //23 一天比赛的具体数据
    private void GetADayData()
    {
        string date = Utils.GetRequest("date", "all", 1, @"^[^\^]{0,2000}$", "0");
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(Out.waplink(Utils.getUrl("live.aspx"), "直播中心-"));
        builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=1&amp;"), "足球比分-"));
        try
        {
            builder.Append(Convert.ToDateTime(date).ToString("yyyy-MM-dd"));
        }
        catch { Utils.Error("输入的时间有误！请重新输入！", ""); }
        builder.Append(Out.Tab("</div>", ""));
        string datetimeout = "";
        try
        {
            string strWhere = "Identification=1 and ft_time ='" + Convert.ToDateTime(date) + "' ORDER BY convert(datetime,ft_time,120) DESC,convert(datetime,ft_caipan,120) DESC";
            DataSet ds = new BCW.BLL.tb_ZQLists().GetList(" * ", strWhere);
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string[] pageValUrl = { "act", "ptype", "id", "backurl", "date" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                recordCount = ds.Tables[0].Rows.Count;
                int k = 1;
                int koo = (pageIndex - 1) * pageSize;
                int skt = pageSize;
                if (recordCount > koo + pageSize)
                {
                    skt = pageSize;
                }
                else
                {
                    skt = recordCount - koo;
                }
                for (int i = 0; i < skt; i++)
                {
                    try
                    {
                        if (ds.Tables[0].Rows[koo + i]["ft_team1"].ToString() != "" && ds.Tables[0].Rows[koo + i]["ft_team2"].ToString() != "")
                        {
                            if (i % 2 == 0)
                            { builder.Append(Out.Tab("<div >", "<br/>")); }
                            else
                            {
                                if (i == 1)
                                    builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
                                else
                                    builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
                            }
                            if (datetimeout != Convert.ToDateTime(ds.Tables[0].Rows[koo + i]["ft_time"]).ToString("MM月dd日"))
                            {
                                datetimeout = Convert.ToDateTime(ds.Tables[0].Rows[koo + i]["ft_time"]).ToString("MM月dd日");
                                builder.Append("<b>" + datetimeout + "</b><br/>");
                            }
                            builder.Append("<a  href=\"" + Utils.getUrl("live.aspx?act=21&amp;Id=" + ds.Tables[0].Rows[koo + i]["Id"] + "&amp;") + "\">" + ds.Tables[0].Rows[koo + i]["ft_caipan"] + "[" + ds.Tables[0].Rows[koo + i]["ft_teamStyle"] + "]");
                            builder.Append(ds.Tables[0].Rows[koo + i]["ft_team1"]);
                            if (ds.Tables[0].Rows[koo + i]["ft_result"].ToString() == "")
                            {
                                builder.Append("VS");
                            }
                            else
                            {
                                builder.Append("(" + ds.Tables[0].Rows[koo + i]["ft_result"] + ")");
                            }
                            builder.Append(ds.Tables[0].Rows[koo + i]["ft_team2"].ToString() + "</a>");

                            // builder.Append("<br/>");
                            // builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=21&amp;Id=" + ds.Tables[0].Rows[koo + i]["Id"] + "&amp;"), "[" + textout + "]") + "");
                            k++;
                            datetimeout = Convert.ToDateTime(ds.Tables[0].Rows[koo + i]["ft_time"]).ToString("MM月dd日");
                            builder.Append(Out.Tab("</div>", ""));
                        }
                    }
                    catch
                    {
                        if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))//u=" + Utils.getstrU() + "&amp;
                        {
                            Utils.Success("温馨提示", "使用彩版，页面更直观，更快捷！正在切换进入...", "live.aspx?act=23&amp;ve=2a&amp;page=" + pageIndex + "&amp;date=" + date + "&amp;u=" + Utils.getstrU() + "", "1");
                        }
                    }
                }
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Tab("<div>", "<br/><br/>"));
                builder.Append("无相关球赛记录");
                builder.Append(Out.Tab("</div>", "<br/>"));
            }
        }
        catch
        {
            builder.Append(Out.Tab("<div>", "<br/><br/>"));
            builder.Append("输入日期错误.");
            builder.Append(Out.Tab("</div>", "<br/>"));
            //if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))//u=" + Utils.getstrU() + "&amp;
            //{
            //    Utils.Success("温馨提示", "使用彩版，页面更直观，更快捷！正在切换进入...", "live.aspx?act=21&amp;Id=" + Id + "&amp;ve=2a&amp;u=" + Utils.getstrU() + "", "1");
            //}
        }
        builder.Append(Out.Tab("<div >", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("live.aspx?act=1&amp;") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div >", ""));
    }

    //24 一支球队的具体数据
    private void GetATeamData()
    {
        string team = Utils.GetRequest("team", "all", 1, @"^[^\^]{0,2000}$", "0");
        //string strpattern1 = @"[\u4e00-\u9fa5]+";
        //Match mtitle12 = Regex.Match(team, strpattern1, RegexOptions.IgnoreCase);
        //team = mtitle12.Groups[0].Value.Trim();
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(Out.waplink(Utils.getUrl("live.aspx"), "直播中心-"));
        builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=1&amp;"), "足球比分-"));
        builder.Append(team);
        builder.Append(Out.Tab("</div>", ""));
        string datetimeout = "";
        try
        {
            string strWhere = "Identification=1 and ft_team1 like '%" + team + "%' or ft_team2 like '%" + team + "%' and convert(datetime,ft_time,120)>(getDate()-1) ORDER BY convert(datetime,ft_time,120) DESC,convert(datetime,ft_caipan,120) DESC";
            DataSet ds = new BCW.BLL.tb_ZQLists().GetList(" * ", strWhere);
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string[] pageValUrl = { "act", "ptype", "id", "backurl", "date" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                recordCount = ds.Tables[0].Rows.Count;
                int k = 1;
                int koo = (pageIndex - 1) * pageSize;
                int skt = pageSize;
                if (recordCount > koo + pageSize)
                {
                    skt = pageSize;
                }
                else
                {
                    skt = recordCount - koo;
                }
                for (int i = 0; i < skt; i++)
                {
                    try
                    {
                        if (ds.Tables[0].Rows[koo + i]["ft_team1"].ToString() != "" && ds.Tables[0].Rows[koo + i]["ft_team2"].ToString() != "")
                        {
                            if (i % 2 == 0)
                            { builder.Append(Out.Tab("<div >", "<br/>")); }
                            else
                            {
                                if (i == 1)
                                    builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
                                else
                                    builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
                            }
                            if (datetimeout != Convert.ToDateTime(ds.Tables[0].Rows[koo + i]["ft_time"]).ToString("MM月dd日"))
                            {
                                datetimeout = Convert.ToDateTime(ds.Tables[0].Rows[koo + i]["ft_time"]).ToString("MM月dd日");
                                builder.Append("<b>" + datetimeout + "</b><br/>");
                            }
                            builder.Append("<a  href=\"" + Utils.getUrl("live.aspx?act=21&amp;Id=" + ds.Tables[0].Rows[koo + i]["Id"] + "&amp;") + "\">" + ds.Tables[0].Rows[koo + i]["ft_caipan"] + "[" + ds.Tables[0].Rows[koo + i]["ft_teamStyle"] + "]");
                            builder.Append(GetRed(ds.Tables[0].Rows[koo + i]["ft_team1"].ToString(), team));
                            if (ds.Tables[0].Rows[koo + i]["ft_result"].ToString() == "")
                            {
                                builder.Append("VS");
                            }
                            else
                            {
                                builder.Append("(" + ds.Tables[0].Rows[koo + i]["ft_result"] + ")");
                            }
                            builder.Append(GetRed(ds.Tables[0].Rows[koo + i]["ft_team2"].ToString(), team) + "</a>");

                            // builder.Append("<br/>");
                            // builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=21&amp;Id=" + ds.Tables[0].Rows[koo + i]["Id"] + "&amp;"), "[" + textout + "]") + "");
                            k++;
                            datetimeout = Convert.ToDateTime(ds.Tables[0].Rows[koo + i]["ft_time"]).ToString("MM月dd日");
                            builder.Append(Out.Tab("</div>", ""));
                        }
                    }
                    catch
                    {
                        if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))//u=" + Utils.getstrU() + "&amp;
                        {
                            Utils.Success("温馨提示", "使用彩版，页面更直观，更快捷！正在切换进入...", "live.aspx?act=23&amp;ve=2a&amp;page=" + pageIndex + "&amp;team=" + team + "&amp;u=" + Utils.getstrU() + "", "1");
                        }
                    }
                }
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Tab("<div>", "<br/><br/>"));
                builder.Append("无相关球赛记录");
                builder.Append(Out.Tab("</div>", "<br/>"));
            }
        }
        catch
        {
            builder.Append(Out.Tab("<div>", "<br/><br/>"));
            builder.Append("输入日期错误.");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        builder.Append(Out.Tab("<div >", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("live.aspx?act=1&amp;") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div >", ""));
    }

    //25 收藏某场比赛发内线
    private void FavoriteGame()
    {
        int Id = Utils.ParseInt(Utils.GetRequest("Id", "all", 1, "", ""));
        int UsId = new BCW.User.Users().GetUsId();
        if (UsId == 0)
            Utils.Login();
        BCW.Model.tb_ZQLists zq = new BCW.BLL.tb_ZQLists().Gettb_ZQLists(Id);
        string favorite = (Utils.GetRequest("favorite", "all", 1, "", ""));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(Out.waplink(Utils.getUrl("live.aspx"), "直播中心-"));
        builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=1&amp;"), "足球比分-"));
        builder.Append("收藏球赛");
        builder.Append(Out.Tab("</div>", "<br/>"));
        if (favorite == "yes")
        {
            if (new BCW.BLL.tb_ZQCollection().ExistsUsIdAndFootId(UsId, Id))
            {
                Utils.Error("你已收藏过该比赛了,再看看其他的吧.", "");
            }
            BCW.Model.tb_ZQCollection Collect = new BCW.Model.tb_ZQCollection();
            Collect.FootBallId = Id;
            Collect.UsId = UsId;
            Collect.Bianhao = zq.ft_bianhao;
            Collect.team1 = zq.ft_team1;
            Collect.team2 = zq.ft_team2;
            Collect.sendCount = 0;
            Collect.result = zq.ft_result;
            Collect.AddTime = DateTime.Now;
            Collect.ident = 0;
            Collect.Remark = "0";
            new BCW.BLL.tb_ZQCollection().Add(Collect);
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("收藏成功！" + "<br/>");
            // builder.Append("当前比分" + zq.ft_result+ "<br/>");
            builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=21&amp;Id=" + Id + ""), "返回上级"));
            builder.Append("<br/>");
            builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=26&amp;Id=" + Id + ""), "查看我的收藏") + "<br/>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append(zq.ft_team1 + "VS" + zq.ft_team2);
            if (zq.ft_result.Length > 2)
            { builder.Append("<br/>" + "当前比分:" + "<font color=\"red\">" + zq.ft_result + "</font>"); }
            //if (zq.ft_result.Length > 2)
            { builder.Append("<br/>" + "比赛状态:" + "<font color=\"red\">" + zq.ft_state + "</font>"); }
            if (new BCW.BLL.tb_ZQCollection().ExistsUsIdAndFootId(UsId, Id))
            {
                builder.Append("<br/>" + "<font color=\"red\">" + "你已收藏过该比赛了,再看看其他的吧." + "</font>" + "<br/>");
            }
            else
            {
                builder.Append("<br/>" + "确定收藏该球赛吗?");
                builder.Append(Out.Tab("</div>", ""));
                builder.Append(Out.Tab("<div>", "<br/>"));
                builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=25&amp;Id=" + Id + "&amp;favorite=yes&amp;"), "确定收藏") + "<br/>");
            }
            builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=21&amp;Id=" + Id + ""), "再看看吧") + "<br/>");
            builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=26&amp;Id=" + Id + ""), "查看我的收藏"));
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div>", "<br/>"));
        builder.Append("温馨提示:收藏后若比分变化将发内线通知您");
        builder.Append(Out.Tab("</div>", ""));
    }

    //26 我的收藏
    private void MyFavoriteGame()
    {
        int UsId = new BCW.User.Users().GetUsId();
        if (UsId == 0)
            Utils.Login();
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(Out.waplink(Utils.getUrl("live.aspx"), "直播中心-"));
        builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=1&amp;"), "足球比分-"));
        builder.Append("我的收藏");
        builder.Append(Out.Tab("</div>", ""));
        string strWhere = "UsId=" + UsId + " order by AddTime DESC";
        DataSet ds = new BCW.BLL.tb_ZQCollection().GetList(" * ", strWhere);
        int pageIndex;
        int recordCount = 1;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            recordCount = ds.Tables[0].Rows.Count;
            int k = 1;
            int koo = (pageIndex - 1) * pageSize;
            int skt = pageSize;
            if (recordCount > koo + pageSize)
            {
                skt = pageSize;
            }
            else
            {
                skt = recordCount - koo;
            }
            for (int i = 0; i < skt; i++)
            {
                try
                {
                    if (i % 2 == 0)
                    {
                        builder.Append(Out.Tab("<div >", "<br/>"));
                    }
                    else
                    {
                        if (i == 1)
                            builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
                        else
                            builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
                    }
                    builder.Append(DT.DateDiff2(DateTime.Now, Convert.ToDateTime(ds.Tables[0].Rows[koo + i]["AddTime"])) + "前收藏了");
                    builder.Append("<a  href=\"" + Utils.getUrl("live.aspx?act=21&amp;Id=" + ds.Tables[0].Rows[koo + i]["FootBallId"] + "") + "\">");
                    builder.Append(BCW.Footballs.checkCodeker.fix(ds.Tables[0].Rows[koo + i]["team1"].ToString()));
                    string result = new BCW.BLL.tb_ZQLists().GetResultFromId(Convert.ToInt32(ds.Tables[0].Rows[koo + i]["FootBallId"]));
                    if (result.Contains("-"))
                        builder.Append("(" + "<font color=\"red\">" + result + "</font>)");
                    else
                        builder.Append("<font color=\"red\">" + "VS" + "</font>");

                    builder.Append(BCW.Footballs.checkCodeker.fix(ds.Tables[0].Rows[koo + i]["team2"].ToString()) + "</a>");
                    //if(new BCW.BLL.tb_ZQLists().GetResultFromId(Convert.ToInt32(ds.Tables[0].Rows[koo + i]["Id"]).ToString())

                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                catch
                {
                    if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
                    {
                        Utils.Success("温馨提示", "使用彩版，页面更直观，更快捷！正在切换进入...", "live.aspx?act=26&amp;ve=2a&amp;page=" + pageIndex + "&amp;u=" + Utils.getstrU() + "", "1");
                    }
                }
            }
        }
        else
        {
            builder.Append(Out.Tab("<div>", "<br/>"));
            builder.Append("暂无收藏哦！快去参与球赛收藏吧！");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        builder.Append(Out.Tab("<div>", "<br/>"));
        builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=1&amp;Id=" + 1 + ""), "再看看吧"));
        //   builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=25&amp;Id=" + 1 + ""), "查看我的收藏") + "<br/>");
        builder.Append(Out.Tab("</div>", "<br/>"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("温馨提示:收藏后若比分变化将发内线通知您");
        builder.Append(Out.Tab("</div>", ""));
    }

    //减缓3min显示
    private string Convertp_once(string p_once)
    {
        string once = "";
        if (!string.IsNullOrEmpty(p_once))
        {
            if (p_once.Contains("'") && !p_once.Contains("+"))
            {
                try
                {
                    int min = Convert.ToInt32(p_once.Replace("'", ""));
                    if (min > 5)
                        once = (min - 3) + "'";
                    else
                        once = p_once;
                }
                catch
                {
                    once = p_once;
                }
            }
            else
            {
                once = p_once;
            }
        }
        return once;

        //return p_once;
    }

    //搜索字体变红
    private string GetRed(string str, string s)
    {
        str = Regex.Replace(str, @"" + s + "", "<font color=\"red\">" + s + "</font>");
        return str;
    }

    private void Getzlist2(int itype)
    {
        string title = "";
        if (itype == 1)
        {
            title = "足球完场比分";
        }
        else
        {
            title = "足球一周赛程";
        }
        TPR.Http.GetLive2 p = new TPR.Http.GetLive2();
        TPR.Model.Http.Live2 objzlist2 = p.Getzlist2(title, itype);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("" + title + "");
        builder.Append(Out.Tab("</div>", ""));
        // null 超时
        if (objzlist2 != null)
        {
            // 为空 遇到问题 或 没有该信息
            if (!string.IsNullOrEmpty(objzlist2.txtLive2zlist))
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.AppendFormat("{0}", ReplaceStr(objzlist2.txtLive2zlist));
                builder.Append(Out.Tab("</div>", "<br />"));
                if (itype == 1)
                {
                    string strText = "查询完场比分:/,";
                    string strName = "day,act";
                    string strType = "text,hidden";
                    string strValu = "'6";
                    string strEmpt = "false";
                    string strIdea = "/查询格式：20100115/";
                    string strOthe = "执行查询,live.aspx,post,1,red";

                    builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                }
            }
            else
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("没有任何记录");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("操作超时, 请重试");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.waplink(Utils.getUrl("live.aspx"), "返回上一级"));
        builder.Append(Out.Tab("</div>", ""));
    }

    private void GetSclassseach(string day, int itype)
    {
        string title = "";
        int Ltype = 0;
        if (itype == 1)
        {
            title = "足球完场比分";
            Ltype = 2;
        }
        else
        {
            title = "足球一周赛程";
            Ltype = 3;
        }
        TPR.Http.GetLive2 p = new TPR.Http.GetLive2();
        TPR.Model.Http.Live2 objzlists2 = p.Getzlists2(day, itype);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("" + title + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        // null 超时
        if (objzlists2 != null)
        {
            // 为空 遇到问题 或 没有该信息
            if (!string.IsNullOrEmpty(objzlists2.txtLive2zlists))
            {
                builder.AppendFormat("{0}", ReplaceStr(objzlists2.txtLive2zlists));
            }
            else
                builder.Append("没有任何记录");
        }
        else
        {
            builder.Append("操作超时, 请重试");
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=" + Ltype + ""), "返回上一级"));
        builder.Append(Out.Tab("</div>", ""));
    }


    private void GetSclassseachInfo(string day, int act, int SClassID)
    {
        string title = "";
        int Ltype = 0;
        if (act == 8)
        {
            title = "足球完场比分";
            Ltype = 6;
        }
        else
        {
            title = "足球一周赛程";
            Ltype = 7;
        }
        TPR.Http.GetLive2 p = new TPR.Http.GetLive2();
        TPR.Model.Http.Live2 objview = p.GetView(day, act, SClassID);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("" + title + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        // null 超时
        if (objview != null)
        {
            // 为空 遇到问题 或 没有该信息
            if (!string.IsNullOrEmpty(objview.txtLive2View))
            {
                builder.AppendFormat("{0}", ReplaceStr(objview.txtLive2View));
            }
            else
                builder.Append("没有任何记录");
        }
        else
        {
            builder.Append("操作超时, 请重试");
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=" + Ltype + "&amp;day=" + day + ""), "返回上一级"));
        builder.Append(Out.Tab("</div>", ""));
    }

    //4 不用
    private void GetScheduleAll()//未开 开 完
    {
        int State = Utils.ParseInt(Utils.GetRequest("State", "all", 1, "", ""));
        string title = "";
        string textout = "";
        string strWhere = "";
        if (State == 0)
        {
            title = "未开赛事";
            strWhere = "ft_state='未'";
            textout = "未";
        }
        else if (State == 1)
        {
            title = "进行赛事";
            strWhere = "ft_state!='未'";
            textout = "直播";
        }
        else
        {
            title = "完场赛事";
            strWhere = "ft_state='完'";
            textout = "完";
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("足球" + title + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<b>" + DateTime.Now.ToString("MM月dd日") + "</b>");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));

        string[] pageValUrl = { "act", "backurl", "State" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        // 开始读取列表
        IList<BCW.Model.tb_ZQLists> listSSCpay = new BCW.BLL.tb_ZQLists().Gettb_ZQListss(pageIndex, pageSize, strWhere, out recordCount);
        if (listSSCpay.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.tb_ZQLists n in listSSCpay)
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

                builder.Append("<a  href=\"" + Utils.getUrl("live.aspx?act=21&amp;Id=" + n.Id + "&amp;") + "\">" + n.ft_caipan + "[" + n.ft_teamStyle + "]" + n.ft_team1 + "(" + n.ft_result + ")VS" + n.ft_team2.Trim() + "</a>");
                builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=21&amp;Id=" + n.Id + "&amp;"), "[" + textout + "]") + "");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }


















        // builder.Append(Out.Tab("<div>", ""));
        // null 超时
        //if (objmlist != null)
        //{
        //    // 为空 遇到问题 或 没有该信息
        //    if (!string.IsNullOrEmpty(objmlist.txtLiveState))
        //    {
        //        builder.AppendFormat("{0}", ReplaceStr(objmlist.txtLiveState));

        //    }
        //    else
        //        builder.Append("没有任何记录");
        //}
        //else
        //{
        //    builder.Append("操作超时, 请重试");
        //}
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        // builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=1&amp;kind=" + + ""), "返回上一级"));
        builder.Append(Out.Tab("</div>", ""));
    }

    private void GetSchedule(int kind, int SClassID, string SClass)
    {

        TPR.Http.GetLive p = new TPR.Http.GetLive();
        TPR.Model.Http.Live objvlist = p.Getvlist(kind, SClassID, SClass);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("" + SClass + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        // null 超时
        if (objvlist != null)
        {
            // 为空 遇到问题 或 没有该信息
            if (!string.IsNullOrEmpty(objvlist.txtLiveView))
            {
                string str = ReplaceStr(objvlist.txtLiveView);

                builder.AppendFormat("{0}", str);

            }
            else
                builder.Append("没有任何记录");
        }
        else
        {
            builder.Append("操作超时, 请重试");
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=1&amp;kind=" + kind + ""), "返回上一级"));
        builder.Append(Out.Tab("</div>", ""));
    }

    #endregion

    #region 篮球开始 现函数
    private string getCount(string count)
    {
        switch (count)
        {
            case "1":
                return "第一节";
            case "2":
                return "第二节";
            case "3":
                return "第三节";
            case "4":
                return "第四节";
            case "5":
                return "加时1";
            case "-1":
                return "完";
            case "0":
                return "未开赛";
            case "50":
                return "中场";
            case "-2":
                return "待定";
            case "-5":
                return "推迟";
            default:
                return "完";
        }
    }

    //30  篮球联赛筛选
    private void SplitBaskGame()
    {
        string Style = Utils.GetRequest("Style", "all", 1, "", "");
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(Out.waplink(Utils.getUrl("live.aspx"), "直播中心-"));
        builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=28&amp;"), "篮球比分-"));
        builder.Append("赛事筛选");
        builder.Append(Out.Tab("</div>", "<br/>"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("请选择赛事(可多选)" + "<br/>");
        string strWhere = " Id>0  and isHidden=1";
        DataSet ds = new BCW.BLL.tb_BasketBallList().GetList(" distinct classType ", strWhere);
        int pageIndex;
        int recordCount;
        int pageSize = 30;// Convert.ToInt32(ub.Get("SiteListNo"));
        string[] pageValUrl = { "act", "ptype", "id", "backurl", "Style" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            recordCount = ds.Tables[0].Rows.Count;
            int k = 1;
            int koo = (pageIndex - 1) * pageSize;
            int skt = pageSize;
            if (recordCount > koo + pageSize)
            {
                skt = pageSize;
            }
            else
            {
                skt = recordCount - koo;
            }
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (!Style.Contains(ds.Tables[0].Rows[koo + i]["classType"].ToString()))
                {
                    builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=30&amp;Style=" + Style + ds.Tables[0].Rows[koo + i]["classType"].ToString() + "." + ""), "[" + ds.Tables[0].Rows[koo + i]["classType"].ToString() + "]"));
                }
                else
                {
                    builder.Append(ds.Tables[0].Rows[koo + i]["classType"].ToString());
                }
                builder.Append("&nbsp;&nbsp;");
                if ((i + 1) % 3 == 0)
                    builder.Append("<br/>");
            }
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Tab("<div>", "<br/>"));
            builder.Append("无相关球赛记录");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        // builder.Append("分类选择");
        builder.Append(Out.Tab("</div>", ""));
        if (Style != "")
        {
            builder.Append(Out.Tab("<div>", "<br/>"));
            builder.Append("当前已选择：" + "<br/>");
            //  builder.Append(Style);
            string[] str = Style.Split('.');
            for (int i = 0; i < str.Length; i++)
            {
                builder.Append(str[i] + "&nbsp;&nbsp;");
                if (i > 0 && i % 3 == 0)
                { builder.Append("<br/>"); }
            }
            builder.Append("<br/>");
            builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=28&amp;Style=" + Style + ""), "确定筛选"));
            builder.Append("<br/>");
            builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=30&amp;"), "重新选择"));
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
    }

    //28 篮球即时比分
    private void BasketNow()
    {
        Master.Refresh = 15;
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string Style = Utils.GetRequest("Style", "all", 1, "", "");
        string lwhere = "  ";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(Out.waplink(Utils.getUrl("default.aspx"), "竞猜-") + "");
        builder.Append(Out.waplink(Utils.getUrl("live.aspx"), "直播中心-"));
        builder.Append("篮球比分");
        builder.Append(Out.Tab("</div>", "<br/>"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        int State = Utils.ParseInt(Utils.GetRequest("State", "all", 1, "", "1"));
        string date = (Utils.GetRequest("date", "all", 1, "", DateTime.Now.ToString("yyyyMMdd HH:mm:ss")));
        DateTime now = DateTime.Now;
        int n = (int)now.DayOfWeek;
        int kind = 1;
        string title = "";
        string textout = "";
        string strWhere = "";
        string textwhere = "";
        string textresult = "";
        if (State == 1)
        {
            if (Style.Length > 2)
            {
                lwhere += " and ( ";
                string[] sty = Style.Split('.');
                int k = 0;
                foreach (string styl in sty)
                {
                    if (styl != "")
                    {
                        lwhere += " classType like  '%" + styl + "%' ";
                        if (k < sty.Length - 2)
                            lwhere += " or ";
                    }
                    k++;
                }
                lwhere += " ) ";
            }

            title = "即 时";//and isHidden=1  datediff(dd,matchtime,GETDATE())>0
            strWhere = " isHidden=1 and  convert(int,matchstate)>-1  and    DateDiff(dd,matchtime,getdate())=0 " + lwhere + "  ORDER BY matchtime ASC";
            //  builder.Append(strWhere);
            textout = "直播";
            builder.Append("即 时| ");
            textwhere = "暂无更多进行赛事.";
            //  strWhere = "p_active=0 and p_del=0 and p_ison=1 and p_isondel=0 and p_basketve=0";
        }
        else
        {
            builder.Append(Out.waplink(Utils.getUrl("live.aspx?ptype=1&amp;act=28&amp;kind=" + kind + "&amp;State=1"), "即 时| ") + "");
        }
        if ((State == 2))
        {
            builder.Append("赛 果| ");
            DateTime dt = Convert.ToDateTime(date);
            title = "比赛结果"; //datediff(dd, matchtime, GETDATE()) = 0 and  isHidden=1 
            strWhere = " isHidden=1 and matchstate=-1 and matchtime <'" + dt.AddDays(1).ToString("yyyyMMdd 00:00:00") + "' and matchtime >'" + dt.ToString("yyyyMMdd 00:00:00") + "' ORDER BY convert(datetime,matchtime,120) desc";
            textwhere = "暂无更多完场赛事.";
        }
        else
        {
            builder.Append(Out.waplink(Utils.getUrl("live.aspx?ptype=1&amp;act=28&amp;kind=" + kind + "&amp;State=2&amp;date=" + DateTime.Now.AddDays(-2).ToString() + "&amp;"), "赛 果| ") + "");
        }
        if (State == 3)
        {
            title = "未开赛事";//and isHidden=1 
            strWhere = " isHidden=1 and convert(int,matchstate)>-1  ORDER BY convert(datetime,matchtime,120) desc";
            textout = "未";
            builder.Append("赛 程| ");
            textwhere = "暂无更多未开赛事.";
        }
        else
        {
            builder.Append(Out.waplink(Utils.getUrl("live.aspx?ptype=1&amp;act=28&amp;kind=" + kind + "&amp;State=3&amp;week=" + n + "&amp;"), "赛 程| ") + "");
        }
        if (State == 4)
        {
            if (BasketBallCollect == "1")
            {
                title = "收藏比赛";
                strWhere = "  convert(int,matchstate)=-1 and datediff(dd,matchtime,GETDATE())=0  ORDER BY convert(datetime,matchtime,120) desc";
                textout = "未";
                builder.Append("收 藏");
                textwhere = "暂无更多未开赛事.";
            }
        }
        else
        {
            if (BasketBallCollect == "1")
            {
                builder.Append(Out.waplink(Utils.getUrl("live.aspx?ptype=1&amp;act=28&amp;kind=" + kind + "&amp;State=4"), "收 藏 ") + "");
            }
        }
        builder.Append(Out.Tab("</div>", ""));

        if ((State == 2))
        {
            builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
            builder.Append(Out.waplink(Utils.getUrl("live.aspx?ptype=2&amp;act=28&amp;kind=" + kind + "&amp;State=2&amp;date=" + Convert.ToDateTime(date).AddDays(-1).ToString() + "&amp;"), Convert.ToDateTime(date).AddDays(-1).ToString("yyyy-MM-dd")) + " | ");
            builder.Append("<b>" + Convert.ToDateTime(date).ToString("yyyy-MM-dd") + "</b>" + " | ");
            if (Convert.ToDateTime(date) > DateTime.Now.AddDays(-1))
            {
                builder.Append(Convert.ToDateTime(date).AddDays(1).ToString("yyyy-MM-dd"));
            }
            else
                builder.Append(Out.waplink(Utils.getUrl("live.aspx?ptype=2&amp;act=28&amp;kind=" + kind + "&amp;State=2&amp;date=" + Convert.ToDateTime(date).AddDays(1).ToString() + "&amp;"), Convert.ToDateTime(date).AddDays(1).ToString("yyyy-MM-dd")));
            builder.Append(Out.Tab("</div>", ""));
        }
        else if (State == 3)
        {
            int week = Utils.ParseInt(Utils.GetRequest("week", "all", 1, "", "1"));
            string[] weekDays = { "周日", "周一", "周二", "周三", "周四", "周五", "周六" };
            builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
            for (int i = 0; i < 7; i++)
            {
                if (n > 6)
                    n = 0;
                if (n == week)
                {
                    DateTime dd = DateTime.Now.AddDays(i + 1);
                    DateTime dd2 = DateTime.Now.AddDays((i));
                    builder.Append("<b>" + weekDays[n++] + "</b>");
                    strWhere = " isHidden=1 and convert(int,matchstate)>-5 and matchtime <'" + dd.ToString("yyyyMMdd 00:00:00") + "' and matchtime >'" + dd2.ToString("yyyyMMdd 00:00:00") + "' ORDER BY convert(datetime,matchtime,120) desc";
                }
                else
                {
                    builder.Append(Out.waplink(Utils.getUrl("live.aspx?ptype=2&amp;act=28&amp;kind=" + kind + "&amp;State=3&amp;week=" + n + "&amp;"), weekDays[n++]));
                }
                if (i < 6)
                {
                    builder.Append(" | ");
                }
            }
            builder.Append(Out.Tab("</div>", ""));
        }

        if (State == 4)
        {
            #region 收藏显示与隐藏
            //篮球收藏开关 1开 0关
            if (BasketBallCollect == "1")
            {
                //收藏列表
                string strWhereC = "UsId=" + meid + " order by AddTime DESC";
                DataSet ds = new BCW.BLL.tb_BasketBallCollect().GetList(" * ", strWhereC);
                int pageIndex;
                int recordCount = 1;
                int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
                string[] pageValUrl = { "act", "ptype", "backurl", "State" };
                pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                if (pageIndex == 0)
                    pageIndex = 1;
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    recordCount = ds.Tables[0].Rows.Count;
                    int k = 1;
                    int koo = (pageIndex - 1) * pageSize;
                    int skt = pageSize;
                    if (recordCount > koo + pageSize)
                    {
                        skt = pageSize;
                    }
                    else
                    {
                        skt = recordCount - koo;
                    }
                    for (int i = 0; i < skt; i++)
                    {
                        try
                        {
                            if (i % 2 == 0)
                            {
                                builder.Append(Out.Tab("<div >", "<br/>"));
                            }
                            else
                            {
                                if (i == 1)
                                    builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
                                else
                                    builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
                            }
                            builder.Append(DT.DateDiff2(DateTime.Now, Convert.ToDateTime(ds.Tables[0].Rows[koo + i]["AddTime"])) + "前收藏了");
                            builder.Append("<a  href=\"" + Utils.getUrl("live.aspx?act=29&amp;Id=" + ds.Tables[0].Rows[koo + i]["BasketBallId"] + "") + "\">");
                            builder.Append((ds.Tables[0].Rows[koo + i]["team1"].ToString().Trim()));
                            BCW.Model.tb_BasketBallList mo = new BCW.BLL.tb_BasketBallList().Gettb_BasketBallList(Convert.ToInt32(ds.Tables[0].Rows[koo + i]["BasketBallId"]));
                            if (mo.matchstate.Length > 2)
                            {
                                if (mo.matchstate.Trim() == "-1" || getCount(mo.matchstate.Trim()).Contains("节"))
                                { builder.Append("(<font color=\"red\">" + mo.homescore + "-" + mo.guestscore + "</font>)"); }
                                else
                                {
                                    builder.Append("<font color=\"red\">" + "VS" + "</font>");
                                }
                            }
                            //   builder.Append("<font color=\"red\">" + "VS" + "</font>");

                            builder.Append((ds.Tables[0].Rows[koo + i]["team2"].ToString().Trim()) + "</a>");
                            if (mo.matchstate.Trim() != "")
                            { builder.Append(getCount(mo.matchstate.Trim())); }
                            k++;
                            builder.Append(Out.Tab("</div>", ""));
                        }
                        catch
                        {
                            if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
                            {
                                Utils.Success("温馨提示", "使用彩版，页面更直观，更快捷！正在切换进入...", "live.aspx?act=26&amp;ve=2a&amp;page=" + pageIndex + "&amp;u=" + Utils.getstrU() + "", "1");
                            }
                        }
                    }
                }
                else
                {
                    builder.Append(Out.Tab("<div>", "<br/>"));
                    builder.Append("暂无收藏哦！快去参与球赛收藏吧！");
                    builder.Append(Out.Tab("</div>", "<br/>"));
                }
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                builder.Append(Out.Tab("<div>", "<br/>"));
                builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=28&amp;Id=" + 1 + ""), "再看看吧"));
                //   builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=25&amp;Id=" + 1 + ""), "查看我的收藏") + "<br/>");
                builder.Append(Out.Tab("</div>", "<br/>"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("温馨提示:收藏后若比分变化将发内线通知您");
                builder.Append(Out.Tab("</div>", ""));
            }
            #endregion
        }
        else
        {
            #region 最新版_篮球即时 赛果 赛程
            DataSet ds = new BCW.BLL.tb_BasketBallList().GetList(" * ", strWhere);
            string datetimeout = "";
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string[] pageValUrl = { "act", "ptype", "id", "backurl", "State", "date", "week", "Style" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                recordCount = ds.Tables[0].Rows.Count;
                int k = 1;
                int koo = (pageIndex - 1) * pageSize;
                int skt = pageSize;
                if (recordCount > koo + pageSize)
                {
                    skt = pageSize;
                }
                else
                {
                    skt = recordCount - koo;
                }
                for (int i = 0; i < skt; i++)
                {
                    try
                    {
                        //官网是否有该场球赛，有则显示
                        // if (new TPR2.BLL.guess.BaList().ExistsByp_id(Convert.ToInt32(ds.Tables[0].Rows[koo + i]["ft_bianhao"])))
                        {
                            {
                                if (i % 2 == 0)
                                { builder.Append(Out.Tab("<div >", "<br/>")); }
                                else
                                {
                                    if (i == 1)
                                        builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
                                    else
                                        builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
                                }
                                if (datetimeout != Convert.ToDateTime(ds.Tables[0].Rows[koo + i]["matchtime"]).ToString("MM月dd日"))
                                {
                                    datetimeout = Convert.ToDateTime(ds.Tables[0].Rows[koo + i]["matchtime"]).ToString("MM月dd日");
                                    builder.Append("<b>" + datetimeout + "</b><br/>");
                                }
                                if (BasketBallCollect == "1")
                                {
                                    if (new BCW.BLL.tb_BasketBallCollect().ExistsUsIdAndBaskId(meid, Convert.ToInt32(ds.Tables[0].Rows[koo + i]["Id"])))
                                    {
                                        //  builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=25&amp;Id=" + Id + ""), "[已收藏]") + "");
                                        builder.Append("<font color=\"BLUE\">" + "[已收藏]" + "</font>");
                                    }
                                    else
                                    {
                                        builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=31&amp;Id=" + ds.Tables[0].Rows[koo + i]["ID"] + ""), "[收藏]") + "");
                                    }
                                }
                                builder.Append("<a  href=\"" + Utils.getUrl("live.aspx?act=29&amp;Id=" + ds.Tables[0].Rows[koo + i]["ID"] + "&amp;") + "\">" + Convert.ToDateTime(ds.Tables[0].Rows[koo + i]["matchtime"]).ToString("HH:mm") + "<font color=\"" + ds.Tables[0].Rows[koo + i]["addTechnic"] + "\">" + "[" + ds.Tables[0].Rows[koo + i]["classType"] + "</font>" + "]");
                                builder.Append((ds.Tables[0].Rows[koo + i]["hometeam"].ToString()));

                                {
                                    if (ds.Tables[0].Rows[koo + i]["matchstate"].ToString().Trim() == "0")
                                    {
                                        builder.Append("<font color=\"red\">" + "VS" + "</font>");
                                    }
                                    else
                                        builder.Append("(" + "<font color=\"red\">" + ds.Tables[0].Rows[koo + i]["homescore"] + "-" + ds.Tables[0].Rows[koo + i]["guestscore"] + "</font>" + ")");
                                    builder.Append(ds.Tables[0].Rows[koo + i]["guestteam"]);
                                }
                                builder.Append("</a>");
                                if (Convert.ToInt32(ds.Tables[0].Rows[koo + i]["matchstate"]) > -10 && Convert.ToInt32(ds.Tables[0].Rows[koo + i]["matchstate"]) < 100)
                                {
                                    builder.Append(getCount(ds.Tables[0].Rows[koo + i]["matchstate"].ToString().Trim()));
                                }
                                k++;
                                datetimeout = Convert.ToDateTime(ds.Tables[0].Rows[koo + i]["matchtime"]).ToString("MM月dd日");
                                builder.Append(Out.Tab("</div>", ""));
                            }
                        }
                    }
                    catch
                    {
                        if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))//u=" + Utils.getstrU() + "&amp;
                        {
                            Utils.Success("温馨提示", "使用彩版，页面更直观，更快捷！正在切换进入...", "live.aspx?act=29&amp;ve=2a&amp;u=" + Utils.getstrU() + "", "1");
                        }
                    }
                }
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                // builder.Append("<br/>");
            }
            else
            {
                builder.Append(Out.Tab("<div>", "<br/><br/>"));
                builder.Append("无更多赛果");
                builder.Append(Out.Tab("</div>", "<br/>"));
            }
            #endregion
        }
    }

    //29 篮球一场比赛
    private void BasketMatch()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(Out.waplink(Utils.getUrl("live.aspx"), "直播中心-"));
        builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=28&amp;"), "篮球比分-"));
        builder.Append("即时比分");
        builder.Append(Out.Tab("</div>", "<br />"));
        int Id = Utils.ParseInt(Utils.GetRequest("Id", "all", 1, "", "0"));
        int type = Utils.ParseInt(Utils.GetRequest("type", "all", 1, "", "1"));
        int shunxu = Utils.ParseInt(Utils.GetRequest("shunxu", "all", 1, "", "1"));
        string BasketBallOutZhi = (ub.GetSub("BasketBallOutZhi", "/Controls/footballs.xml"));
        BCW.Model.tb_BasketBallList model = new BCW.BLL.tb_BasketBallList().Gettb_BasketBallList(Id);
        //   builder.Append(model.contentList);
        if (!new BCW.BLL.tb_BasketBallList().Exists(Id))
        {
            Utils.Success("不存在的记录！", "出错啦！不存在该球赛记录！1s后返回..", Utils.getUrl("live.aspx?act=100&amp;Id=" + Id + ""), "1");
        }
        Master.Title = model.hometeam + "VS" + model.guestteam;
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("live.aspx?act=28&amp;Style=" + model.classType + "") + "\">" + "<font color=\"" + model.addTechnic + "\">" + model.classType + "</font>" + " </a> " + "@ " + Convert.ToDateTime(model.matchtime).ToString("MM月dd日 HH:mm") + "&nbsp; ");
        //收藏开关
        if (BasketBallCollect == "1")
        {
            if (new BCW.BLL.tb_BasketBallCollect().ExistsUsIdAndBaskId(meid, Id))
            {
                builder.Append("<font color=\"BLUE\">" + "[已收藏]" + "</font>");
                builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=32&amp;Id=" + Id + ""), "[取消收藏]") + "");
            }
            else
            {
                builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=31&amp;Id=" + Id + ""), "[收藏]") + "");
            }
        }
        builder.Append("<br />主客队:");//href=\"" + Utils.getUrl("live.aspx?act=28&amp;Style=" + GetAGameText(model.hometeam) +  href =\"" + Utils.getUrl("live.aspx?act=28&amp;Style=" + GetAGameText(model.guestteam) + "") + "\
        builder.Append("<a>" + model.hometeam + " </a>");
        builder.Append(" - ");
        builder.Append("<a>" + model.guestteam + " </a>");
        builder.Append("<br />比赛状态:" + getCount(model.matchstate.Trim()));
        builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=29&amp;Id=" + Id + "&amp;type=" + type + ""), " [刷]") + "");
        if (model.isDone.Contains(":"))
            builder.Append("<br/>" + "剩余时间:" + model.isDone);
        if (model.matchstate.Trim() != "0")
        {
            if (model.matchstate.Length > 2)
            {
                if (model.matchstate.Trim() == "-1")
                { builder.Append("<br/>" + "完场比分:" + "<font color=\"red\">" + model.homescore + "-" + model.guestscore + "</font>"); }
                else
                {
                    builder.Append("<br/>" + "即时比分:" + "<font color=\"red\">" + model.homescore + "-" + model.guestscore + "</font>");
                }
                if (new TPR2.BLL.guess.BaList().Exists(model.connectId))
                {
                    builder.Append(Out.waplink(Utils.getUrl("showGuess.aspx?gid=" + model.connectId + ""), "[析]"));
                }
            }

            if (model.homeEurope.Contains(",") && model.homeEurope.Trim().Length > 3)
            {
                string[] ouzhi = model.homeEurope.Trim().Split(',');
                builder.Append("<br/>" + "欧指:" + " [主] " + ouzhi[0] + "   [客] " + ouzhi[1]);
            }
            builder.Append("<br/>");
            if (type == 1)
            {
                builder.Append("<u>比分直播</u> |");
            }
            else
            {
                builder.Append(Out.waplink(Utils.getUrl("live.aspx?ptype=1&amp;act=29&amp;Id=" + Id + "&amp;type=1"), "比分直播 ") + "|");
            }
            if (type == 2)
            {
                builder.Append("  <u>文字直播</u>");
            }
            else
            {
                builder.Append(Out.waplink(Utils.getUrl("live.aspx?ptype=1&amp;act=29&amp;Id=" + Id + "&amp;type=2"), "  文字直播 ") + "");
            }
        }
        builder.Append(Out.Tab("</div>", ""));
        //    builder.Append("<br/>");

        if (type == 1)
        {
            builder.Append(Out.Tab("<div>", "<br/>"));
            if (model.homeone.Trim() != "")
                builder.Append("第一节:" + "<font color=\"red\">" + model.homeone.Trim() + "-" + model.guestone.Trim() + "</font><br/>");
            if (model.hometwo.Trim() != "")
                builder.Append("第二节:" + "<font color=\"red\">" + model.hometwo.Trim() + "-" + model.guesttwo.Trim() + "</font><br/>");
            if (model.homethree.Trim() != "")
                builder.Append("第三节:" + "<font color=\"red\">" + model.homethree.Trim() + "-" + model.guestthree.Trim() + "</font><br/>");
            if (model.homefour.Trim() != "")
                builder.Append("第四节:" + "<font color=\"red\">" + model.homefour.Trim() + "-" + model.guestfour.Trim() + "</font><br/>");
            if (model.contentList.Length > 3)
            {
                //  if (!model.contentList.Trim().Contains(model.explain.Trim()))
                {
                    builder.Append("<font color=\"green\">最新直播:</font>" + model.contentList);
                }
            }
            if (model.explain.Length > 10 && !(model.contentList.Trim().Contains(model.explain.Trim())))
                builder.Append("<font color=\"green\">球场数据:</font>" + "<br/>" + model.explain + "<br/>" + model.explain2);

            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            // builder.Append(model.contentList);
            #region 文字直播开始
            //存在文字直播数据
            if (new BCW.BLL.tb_BasketBallWord().ExistsName(model.name_en))
            {
                builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
                string strwhere = "desc";
                if (shunxu == 2)
                {
                    builder.Append("  <u>倒序</u> | ");
                    strwhere = " desc ";
                }
                else
                {
                    builder.Append(Out.waplink(Utils.getUrl("live.aspx?ptype=1&amp;act=29&amp;Id=" + Id + "&amp;type=2&amp;shunxu=2&amp;"), "  倒序 ") + " | ");
                }
                if (shunxu == 1)
                {
                    builder.Append("<u>正序</u> ");
                    strwhere = " asc ";
                }
                else
                {
                    builder.Append(Out.waplink(Utils.getUrl("live.aspx?ptype=1&amp;act=29&amp;Id=" + Id + "&amp;type=2&amp;shunxu=1&amp;"), "正序 ") + "");
                }
                builder.Append(Out.Tab("", ""));

                builder.Append(Out.Tab("</div>", ""));
                string swhere = " name_enId = " + model.name_en + "  order by last  " + strwhere;
                DataSet ds = new BCW.BLL.tb_BasketBallWord().GetList(" * ", swhere);
                int pageIndex;
                int recordCount;
                int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
                string[] pageValUrl = { "act", "ptype", "Id", "backurl", "State", "date", "type" };
                pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                if (pageIndex == 0)
                    pageIndex = 1;
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    recordCount = ds.Tables[0].Rows.Count;
                    int k = 1;
                    int koo = (pageIndex - 1) * pageSize;
                    int skt = pageSize;
                    if (recordCount > koo + pageSize)
                    {
                        skt = pageSize;
                    }
                    else
                    {
                        skt = recordCount - koo;
                    }
                    for (int i = 0; i < skt; i++)
                    {
                        if (i % 2 == 0)
                        {
                            builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
                        }
                        else
                        {
                            if (i == 1)
                                builder.Append(Out.Tab("<div>", "<br/>"));
                            else
                                builder.Append(Out.Tab("<div>", "<br/>"));
                        }
                        if (ds.Tables[0].Rows[koo + i]["whichTeam"].ToString().Trim() == "1")
                        {
                            builder.Append("<font color=\"green\">[主]</font>");
                        }
                        else if (ds.Tables[0].Rows[koo + i]["whichTeam"].ToString().Trim() == "2")
                        {
                            builder.Append("[客]");
                        }
                        else
                            builder.Append("[全]");
                        //输出比分
                        builder.Append("<font color=\"red\">" + ds.Tables[0].Rows[koo + i]["hometeam"].ToString().Trim() + ":");
                        builder.Append(ds.Tables[0].Rows[koo + i]["guestteam"] + "</font>");
                        builder.Append(ds.Tables[0].Rows[koo + i]["listContent"].ToString().Trim() + "");
                        if (ds.Tables[0].Rows[koo + i]["isSame"].ToString().Trim() != "")
                        {
                            builder.Append("<font color =\"green\">[" + ds.Tables[0].Rows[koo + i]["isSame"].ToString().Trim() + "]</font>");
                        }
                        k++;
                        builder.Append(Out.Tab("</div>", ""));
                        //catch
                        //{
                        //    if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))//u=" + Utils.getstrU() + "&amp;
                        //    {
                        //        Utils.Success("温馨提示", "使用彩版，页面更直观，更快捷！正在切换进入...", "live.aspx?act=29&amp;ve=2a&amp;u=" + Utils.getstrU() + "", "1");
                        //    }
                        //}
                    }
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                }
                else
                {
                    builder.Append(Out.Tab("<div>", "<br/>"));
                    builder.Append("无更多赛果");
                    builder.Append(Out.Tab("</div>", "<br/>"));
                }   
            }
            else if (model.contentList != "")
            {
                builder.Append(Out.Tab("<div>", "<br/>"));
                builder.Append(model.contentList);
                builder.Append(Out.Tab("</div>", "<br/>"));
            }
            else
            {
                builder.Append(Out.Tab("<div>", "<br/>"));
                builder.Append("暂无文字直播!");
                builder.Append(Out.Tab("</div>", "<br/>"));
            }
            //if (model.tv.Contains("</a>"))
            //{
            //    builder.Append(Out.Tab("<div>", "<br/>"));
            //    try
            //    {
            //        builder.Append("视频直播:" + "" + model.tv);
            //    }
            //    catch
            //    {

            //    }
            //    builder.Append(Out.Tab("</div>", "<br/>"));
            //}
            #endregion
        }
        //  builder.Append((Out.Hr()));
        builder.Append(Out.SysUBB(BCW.User.Users.ShowSpeak(21, "live.aspx?act=29&amp;Id=" + Id + "&amp;type=" + type + "&amp;backurl=" + Utils.getPage(0) + "", 5, 0)));
    }

    //31 收藏比赛
    private void FavoriteBacketGame()
    {
        int Id = Utils.ParseInt(Utils.GetRequest("Id", "all", 1, "", ""));
        int UsId = new BCW.User.Users().GetUsId();
        string usname = new BCW.BLL.User().GetUsName(UsId);
        if (UsId == 0)
            Utils.Login();
        BCW.Model.tb_BasketBallList zq = new BCW.BLL.tb_BasketBallList().Gettb_BasketBallList(Id);
        string favorite = (Utils.GetRequest("favorite", "all", 1, "", ""));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(Out.waplink(Utils.getUrl("live.aspx"), "直播中心-"));
        builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=28&amp;"), "篮球比分-"));
        builder.Append("收藏球赛");
        builder.Append(Out.Tab("</div>", "<br/>"));
        if (favorite == "yes")
        {
            if (new BCW.BLL.tb_BasketBallCollect().ExistsUsIdAndBaskId(UsId, Id))
            {
                Utils.Error("你已收藏过该比赛了,再看看其他的吧.", "");
            }
            BCW.Model.tb_BasketBallCollect Collect = new BCW.Model.tb_BasketBallCollect();
            Collect.BasketBallId = Id;
            Collect.UsId = UsId;
            Collect.UsName = usname;
            Collect.Bianhao = zq.name_en;
            Collect.team1 = zq.hometeam;
            Collect.team2 = zq.guestteam;
            Collect.sendCount = 0;
            Collect.result = zq.homescore + "-" + zq.guestscore;
            Collect.AddTime = DateTime.Now;
            Collect.ident = 0;
            Collect.Remark = "0";
            new BCW.BLL.tb_BasketBallCollect().Add(Collect);

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("收藏成功！" + "<br/>");
            builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=28&amp;Id=" + Id + "&amp;State=4&amp;"), "查看我的收藏") + "<br/>");
            builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=29&amp;Id=" + Id + ""), "返回球赛"));
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append(zq.hometeam + "VS" + zq.guestteam);
            if (zq.matchstate.Trim() == "-1")
            { builder.Append("<br/>" + "完场比分:" + "<font color=\"red\">" + zq.homescore + "-" + zq.guestscore + "</font>"); }
            else if (getCount(zq.matchstate.Trim()).Contains("节"))
            { builder.Append("<br/>" + "即时比分:" + "<font color=\"red\">" + zq.homescore + "-" + zq.guestscore + "</font>"); }
            if (zq.matchstate.Trim() != "")
            {
                builder.Append("<br/>" + "比赛状态:" + "<font color=\"red\">" + getCount(zq.matchstate.Trim()) + "</font>");
            }
            if (new BCW.BLL.tb_BasketBallCollect().ExistsUsIdAndBaskId(UsId, Id))
            {
                builder.Append("<br/>" + "<font color=\"red\">" + "你已收藏过该比赛了,再看看其他的吧." + "</font>" + "<br/>");
            }
            else
            {
                builder.Append("<br/>" + "确定收藏该球赛吗?");
                builder.Append(Out.Tab("</div>", ""));
                builder.Append(Out.Tab("<div>", "<br/>"));
                builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=31&amp;Id=" + Id + "&amp;favorite=yes&amp;"), "确定收藏") + "<br/>");
            }
            builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=28&amp;Id=" + Id + ""), "再看看吧") + "<br/>");
            builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=28&amp;Id=" + Id + "&amp;State=4&amp;"), "查看我的收藏"));
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div>", "<br/>"));
        builder.Append("温馨提示:收藏后若比分变化将发内线通知您");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 原函数
    //篮球开始 原函数
    private void Getllist()
    {
        TPR.Http.GetLive3 p = new TPR.Http.GetLive3();
        TPR.Model.Http.Live3 objllist = p.Getllist("篮球即时比分");
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("篮球即时比分");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        // null 超时
        if (objllist != null)
        {
            // 为空 遇到问题 或 没有该信息
            if (!string.IsNullOrEmpty(objllist.txtLivellist))
            {
                builder.AppendFormat("{0}", ReplaceStr(objllist.txtLivellist));

            }
            else
                builder.Append("没有任何记录");
        }
        else
        {
            builder.Append("操作超时, 请重试");
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.waplink(Utils.getUrl("live.aspx"), "返回上一级"));
        builder.Append(Out.Tab("</div>", ""));
    }

    //原函数
    private void Gettlist()
    {

        TPR.Http.GetLive3 p = new TPR.Http.GetLive3();
        TPR.Model.Http.Live3 objtlist = p.Gettlist("今天篮球全部赛事");
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("今天篮球全部赛事");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        // null 超时
        if (objtlist != null)
        {
            // 为空 遇到问题 或 没有该信息
            if (!string.IsNullOrEmpty(objtlist.txtLivetlist))
            {
                builder.AppendFormat("{0}", ReplaceStr(objtlist.txtLivetlist));

            }
            else
                builder.Append("没有任何记录");
        }
        else
        {
            builder.Append("操作超时, 请重试");
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=10"), "返回上一级"));
        builder.Append(Out.Tab("<div>", ""));
    }
    //原函数
    private void Getvlist(int SClassID, string SClass)
    {
        TPR.Http.GetLive3 p = new TPR.Http.GetLive3();
        TPR.Model.Http.Live3 objvlist = p.Getvlist(SClassID, SClass);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("篮球即时比分");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        // null 超时
        if (objvlist != null)
        {
            // 为空 遇到问题 或 没有该信息
            if (!string.IsNullOrEmpty(objvlist.txtLiveView))
            {
                builder.AppendFormat("{0}", ReplaceStr(objvlist.txtLiveView));

            }
            else
                builder.Append("没有任何记录");
        }
        else
        {
            builder.Append("操作超时, 请重试");
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=10"), "返回上一级"));
        builder.Append(Out.Tab("</div>", ""));
    }
    //原函数
    private void Getllist2(int itype)
    {
        string title = "";
        if (itype == 1)
        {
            title = "篮球完场比分";
        }
        else
        {
            title = "篮球一周赛程";
        }
        TPR.Http.GetLive4 p = new TPR.Http.GetLive4();
        TPR.Model.Http.Live4 objllist2 = p.Getllist2(title, itype);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("" + title + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        // null 超时
        if (objllist2 != null)
        {
            // 为空 遇到问题 或 没有该信息
            if (!string.IsNullOrEmpty(objllist2.txtLive2llist))
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.AppendFormat("{0}", ReplaceStr(objllist2.txtLive2llist));
                builder.Append(Out.Tab("</div>", "<br />"));
                if (itype == 1)
                {
                    string strText = "查询完场比分:/,";
                    string strName = "day,act";
                    string strType = "text,hidden";
                    string strValu = "'14";
                    string strEmpt = "false";
                    string strIdea = "/查询格式：20100115/";
                    string strOthe = "执行查询,live.aspx,post,1,red";

                    builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                }
            }
            else
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("没有任何记录");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("操作超时, 请重试");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.waplink(Utils.getUrl("live.aspx"), "返回上一级"));
        builder.Append(Out.Tab("</div>", ""));
    }
    //原函数
    private void GetnbaSclassseach(string day, int itype)
    {
        string title = "";
        if (itype == 1)
        {
            title = "足球完场比分";
        }
        else
        {
            title = "足球一周赛程";
        }
        TPR.Http.GetLive4 p = new TPR.Http.GetLive4();
        TPR.Model.Http.Live4 objllists2 = p.Getllists2(day, itype);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("" + title + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        // null 超时
        if (objllists2 != null)
        {
            // 为空 遇到问题 或 没有该信息
            if (!string.IsNullOrEmpty(objllists2.txtLive2llists))
            {
                builder.AppendFormat("{0}", ReplaceStr(objllists2.txtLive2llists));
            }
            else
                builder.Append("没有任何记录");
        }
        else
        {
            builder.Append("操作超时, 请重试");
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=13&amp;type=" + itype + ""), "返回上一级"));
        builder.Append(Out.Tab("</div>", ""));
    }
    //原函数
    private void GetnbaScheduleseach(string p_day, int itype, int SClassID, string SClass)
    {

        TPR.Http.GetLive4 p = new TPR.Http.GetLive4();
        TPR.Model.Http.Live4 objvlist4 = p.GetView(p_day, itype, SClassID, SClass);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("" + SClass + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        // null 超时
        if (objvlist4 != null)
        {
            // 为空 遇到问题 或 没有该信息
            if (!string.IsNullOrEmpty(objvlist4.txtLive2View))
            {
                builder.AppendFormat("{0}", ReplaceStr(objvlist4.txtLive2View));

            }
            else
                builder.Append("没有任何记录");
        }
        else
        {
            builder.Append("操作超时, 请重试");
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=14&amp;day=" + p_day + "&amp;type=" + itype + ""), "返回上一级"));
        builder.Append(Out.Tab("</div>", ""));
    }
    //原函数 16
    private void GetNbaWord()
    {

        TPR.Http.GetNbaword p = new TPR.Http.GetNbaword();
        TPR.Model.Http.Nba objnbalist = p.Getnbalist();
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/game/default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">球彩</a>&gt;直播");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        // null 超时
        if (objnbalist != null)
        {
            // 为空 遇到问题 或 没有该信息
            if (!string.IsNullOrEmpty(objnbalist.txtLivenbalist))
            {
                if (objnbalist.txtLivenbalist.Contains("今日没有比赛!"))
                {
                    builder.Append("前往>>" + Out.waplink(Utils.getUrl("live.aspx?act=28"), "即时比分") + "");
                }
                else
                {
                    builder.AppendFormat("{0}", objnbalist.txtLivenbalist);
                }
            }
            else
                builder.Append("没有任何记录<br />");
        }
        else
        {
            builder.Append("操作超时, 请重试<br />");
        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append(Out.waplink(Utils.getUrl("live.aspx"), "返回上一级"));
        builder.Append(Out.Tab("</div>", ""));
        //闲聊显示
        builder.Append(BCW.User.Users.ShowSpeak(21, "live.aspx?act=16", 5, 0));


        //builder.Append(Out.Tab("<div>", "<br />"));
        //builder.Append("比分数据采集于互联网，仅用于球迷参考或虚拟游戏，如侵犯您的利益请联系本站工作人员.");
        //builder.Append(Out.Tab("</div>", ""));
    }
    //原函数
    private void GetNbaWordPage()
    {
        int nbaid = Utils.ParseInt(Utils.GetRequest("nbaid", "get", 2, @"^[0-9]\d*$", "ID错误"));
        Master.Refresh = 15;
        Master.Gourl = Utils.getUrl("live.aspx?act=17&amp;nbaid=" + nbaid + "");

        TPR.Http.GetNbaword p = new TPR.Http.GetNbaword();
        TPR.Model.Http.Nba objnbapage = p.Getnbapage(nbaid);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/game/default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">球彩</a>&gt;<a href=\"" + Utils.getUrl("live.aspx?act=16") + "\">直播</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        // null 超时
        if (objnbapage != null)
        {
            // 为空 遇到问题 或 没有该信息
            if (!string.IsNullOrEmpty(objnbapage.txtLivenbapage))
            {
                builder.AppendFormat("{0}", objnbapage.txtLivenbapage.Replace("|----------<br/>", "<a href=\"" + Utils.getUrl("live.aspx?act=20&amp;nbaid=" + nbaid + "") + "\">查看更多&gt;&gt;</a>"));

            }
            else
                builder.Append("没有任何记录");
        }
        else
        {
            builder.Append("操作超时, 请重试");
        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=16"), "返回NBA直播"));
        builder.Append(Out.Tab("</div>", ""));
        //闲聊显示
        builder.Append(BCW.User.Users.ShowSpeak(21, "live.aspx?act=17&amp;nbaid=" + nbaid + "", 5, 0));


    }
    //原函数
    private void GetNba2Page()
    {
        int nbaid = Utils.ParseInt(Utils.GetRequest("nbaid", "get", 2, @"^[0-9]\d*$", "ID错误"));
        Master.Refresh = 15;
        Master.Gourl = Utils.getUrl("live.aspx?act=20&amp;nbaid=" + nbaid + "");

        TPR.Http.GetNbaword p = new TPR.Http.GetNbaword();
        TPR.Model.Http.Nba objnbapage = p.Getnbapage2(nbaid);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/game/default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">球彩</a>&gt;<a href=\"" + Utils.getUrl("live.aspx?act=16") + "\">直播</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        // null 超时
        if (objnbapage != null)
        {
            // 为空 遇到问题 或 没有该信息
            if (!string.IsNullOrEmpty(objnbapage.txtLivenbapage))
            {
                builder.AppendFormat("{0}", objnbapage.txtLivenbapage.Replace("|<br/><br/>----------<br/>", ""));

            }
            else
                builder.Append("没有任何记录");
        }
        else
        {
            builder.Append("操作超时, 请重试");
        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=17&amp;nbaid=" + nbaid + ""), "返回上级"));
        builder.Append(Out.Tab("</div>", ""));
    }
    //原函数
    private void GetNbaWordView()
    {
        int nbaid = Utils.ParseInt(Utils.GetRequest("nbaid", "get", 2, @"^[0-9]\d*$", "ID错误"));
        int page = Utils.ParseInt(Utils.GetRequest("page", "get", 1, @"^[0-9]\d*$", "1"));
        Master.Refresh = 15;
        Master.Gourl = Utils.getUrl("live.aspx?act=18&amp;nbaid=" + nbaid + "&amp;page=" + page + "");

        TPR.Http.GetNbaword p = new TPR.Http.GetNbaword();
        TPR.Model.Http.Nba objnbaview = p.Getnbaview(nbaid, page);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/game/default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">球彩</a>&gt;<a href=\"" + Utils.getUrl("live.aspx?act=16") + "\">直播</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        // null 超时
        if (objnbaview != null)
        {
            // 为空 遇到问题 或 没有该信息
            if (!string.IsNullOrEmpty(objnbaview.txtLivenbaview))
            {
                builder.AppendFormat("{0}", objnbaview.txtLivenbaview.Replace("<br/>----------<br/>", "<br />"));

            }
            else
                builder.Append("没有任何记录");
        }
        else
        {
            builder.Append("操作超时, 请重试");
        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=16"), "返回NBA比分直播"));
        builder.Append(Out.Tab("</div>", ""));
        //闲聊显示
        builder.Append(BCW.User.Users.ShowSpeak(21, "live.aspx?act=18&amp;nbaid=" + nbaid + "", 5, 0));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=16"), "NBA") + " ");
        builder.Append(Out.waplink(Utils.getUrl("live.aspx?act=17&amp;nbaid=" + nbaid + ""), "比分直播"));
        builder.Append(Out.Tab("</div>", ""));
    }
    //原函数
    private void GetWNbaWordPage()
    {
        Master.Refresh = 15;
        Master.Gourl = Utils.getUrl("live.aspx?act=19");

        string WNbaList = new TPR.Http.GetWNbaword().Getwnbalist();

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/game/default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">球彩</a>&gt;<a href=\"" + Utils.getUrl("live.aspx?act=16") + "\">直播</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        if (WNbaList != "")
        {
            string getWNBA = string.Empty;
            WNbaList = WNbaList.Replace("],[", "####");
            string[] wnba = Regex.Split(WNbaList, "####");
            for (int i = 0; i < wnba.Length; i++)
            {

                if (wnba[i].Contains("WNBA"))
                {
                    //builder.Append("@" + wnba[i] + "<br />");
                    getWNBA += "@" + wnba[i];
                    string[] temp = Regex.Split(wnba[i], ",");
                    //得到文字直播的页面ID
                    string wnbaid = temp[2].Replace(char.ConvertFromUtf32(34), "");
                    //得到主队名称


                    builder.Append(wnbaid + "<br />");
                }
            }

        }
        else
        {
            builder.Append("操作超时, 请重试");
        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.waplink(Utils.getUrl("live.aspx"), "返回比分直播"));
        builder.Append(Out.Tab("</div>", ""));
        //闲聊显示
        builder.Append(BCW.User.Users.ShowSpeak(21, "live.aspx?act=19", 5, 0));


    }

    private string ReplaceStr(string str)
    {
        if (!string.IsNullOrEmpty(str))
        {
            str = Regex.Replace(str, @"<a.href=.http://168yy.cc[\s\S]+?>[\s\S]+?</a>", "");

        }
        return str;
    }
    #endregion

    //取消收藏 邵广林 20161004
    private void quxiaosc()
    {
        int Id = Utils.ParseInt(Utils.GetRequest("Id", "all", 1, "", ""));
        int UsId = new BCW.User.Users().GetUsId();
        if (UsId == 0)
            Utils.Login();
        BCW.User.Users.IsFresh("live", 2);//防刷
        new BCW.BLL.tb_BasketBallCollect().Delete(Id, UsId);

        Utils.Success("取消收藏", "取消收藏成功，正在返回..", Utils.getUrl("live.aspx?act=28"), "1");
    }

}
