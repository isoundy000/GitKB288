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
/// 姚志光 增加前10奖励派送 20160906 
/// </summary>

public partial class Manage_guess2_topGuess : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string Strbuilder = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "get", 1, "", "");

        if (act == "seach")
        {
            #region 搜索排行榜
            Master.Title = "搜索排行榜";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("搜索排行榜");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "球类:,类型:,开始时间:,结束时间:,";
            string strName = "p_type,ptype,StartTime,OverTime,act";
            string strType = "select,select,date,date,hidden";
            string strValu = "0'0'" + DT.FormatDate(DateTime.Now.AddDays(-10), 0) + "'" + DT.FormatDate(DateTime.Now, 0) + "'seachok";
            string strEmpt = "0|全部|1|足球|2|篮球,1|赌神榜|2|狂人榜,false,false,false";
            string strIdea = "/";
            string strOthe = "搜索排行,topguess.aspx,get,1,red";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            #endregion
        }
        else if (act == "todaytop")
        {
            #region 昨日神猜
            Master.Title = "昨日神猜";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("昨日神猜Top10");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            DateTime dt = DateTime.Parse(DateTime.Now.ToLongDateString());
            string dt1 = dt.AddDays(-1).AddHours(12).ToString();
            string dt2 = dt.AddHours(12).ToString();

            DataSet ds = new TPR2.BLL.guess.BaPay().GetBaPayList("TOP 10 payusid,Count(DISTINCT bcid) as payCount", "p_TPRtime >= '" + dt1 + "' AND p_TPRtime <= '" + dt2 + "' and itypes=0 and types = 0 and p_active>0 and p_getMoney>paycent group by payusid order by Count(DISTINCT bcid) desc");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    builder.Append("[第" + (i + 1) + "名]<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + ds.Tables[0].Rows[i]["payusid"].ToString() + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(int.Parse(ds.Tables[0].Rows[i]["payusid"].ToString())) + "</a>(获胜" + ds.Tables[0].Rows[i]["payCount"] + "场)<br />");
                }
            }
            else
                builder.Append("暂无数据..<br />");

            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.waplink(Utils.getUrl("topGuess.aspx"), "返回排行首页") + "");
            builder.Append("<br />注：一场比赛获胜多场时只算胜一场，平盘、走盘不计，赢半算赢一场");
            builder.Append("<br />昨日时间计算：上一天（昨天）12:00~当天中午12:00");
            builder.Append(Out.Tab("</div>", ""));
            #endregion
        }
        else if (act == "todaytop2")
        {
            #region 一周神猜
            Master.Title = "一周神猜";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("一周神猜Top10");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));

            string dt3 = Convert.ToDateTime(DT.GetWeekStart()).AddHours(12).ToString();
            string dt4 = Convert.ToDateTime(DT.GetWeekOver()).AddHours(12).ToString();

            DataSet ds = new TPR2.BLL.guess.BaPay().GetBaPayList("TOP 10 payusid,Count(DISTINCT bcid) as payCount", "p_TPRtime >= '" + dt3 + "' AND p_TPRtime <= '" + dt4 + "' and itypes=0 and types = 0 and p_active>0 and p_getMoney>paycent group by payusid order by Count(DISTINCT bcid) desc");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    builder.Append("[第" + (i + 1) + "名]<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + ds.Tables[0].Rows[i]["payusid"].ToString() + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(int.Parse(ds.Tables[0].Rows[i]["payusid"].ToString())) + "</a>(获胜" + ds.Tables[0].Rows[i]["payCount"] + "场)<br />");
                }
            }
            else
                builder.Append("暂无数据..<br />");

            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.waplink(Utils.getUrl("topGuess.aspx"), "返回排行首页") + "");
            builder.Append("<br />注：一场比赛获胜多场时只算胜一场，平盘、走盘不计，赢半算赢一场");
            builder.Append("<br />一周时间计算：按自然周，从本周星期一中午12:00~当天中午12:00");
            builder.Append(Out.Tab("</div>", ""));
            #endregion
        }
        else if (act == "todaystar")
        {
            #region 昨日之星
            Master.Title = "昨日之星";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("昨日之星Top10");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));

            DateTime dt = DateTime.Parse(DateTime.Now.ToLongDateString());
            string dt1 = dt.AddDays(-1).AddHours(12).ToString();
            string dt2 = dt.AddHours(12).ToString();

            DataSet ds = new TPR2.BLL.guess.BaPay().GetBaPayList("TOP 10 payusid,sum(p_getMoney-payCent) as payCents", "p_TPRtime >= '" + dt1 + "' AND p_TPRtime <= '" + dt2 + "' and itypes=0 and types = 0 and p_active>0 group by payusid order by sum(p_getMoney-payCent) desc");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    builder.Append("[第" + (i + 1) + "名]<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + ds.Tables[0].Rows[i]["payusid"].ToString() + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(int.Parse(ds.Tables[0].Rows[i]["payusid"].ToString())) + "</a>(净赚" + Convert.ToDouble(ds.Tables[0].Rows[i]["payCents"]) + "" + ub.Get("SiteBz") + ")<br />");
                }
            }
            else
                builder.Append("暂无数据..<br />");

            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.waplink(Utils.getUrl("topGuess.aspx"), "返回排行首页") + "");
            builder.Append("<br />注:昨日时间计算：上一天（昨天）12:00~当天中午12:00");
            builder.Append(Out.Tab("</div>", ""));
            #endregion
        }
        else if (act == "todaystar2")
        {
            #region 一周之星
            Master.Title = "一周之星";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("一周之星Top10");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));

            string dt3 = Convert.ToDateTime(DT.GetWeekStart()).AddHours(12).ToString();
            string dt4 = Convert.ToDateTime(DT.GetWeekOver()).AddHours(12).ToString();

            DataSet ds = new TPR2.BLL.guess.BaPay().GetBaPayList("TOP 10 payusid,sum(p_getMoney-payCent) as payCents", "p_TPRtime >= '" + dt3 + "' AND p_TPRtime <= '" + dt4 + "' and itypes=0 and types = 0 and p_active>0 group by payusid order by sum(p_getMoney-payCent) desc");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    builder.Append("[第" + (i + 1) + "名]<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + ds.Tables[0].Rows[i]["payusid"].ToString() + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(int.Parse(ds.Tables[0].Rows[i]["payusid"].ToString())) + "</a>(净赚" + Convert.ToDouble(ds.Tables[0].Rows[i]["payCents"]) + "" + ub.Get("SiteBz") + ")<br />");
                }
            }
            else
                builder.Append("暂无数据..<br />");

            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.waplink(Utils.getUrl("topGuess.aspx"), "返回排行首页") + "");
            builder.Append("<br />注:一周时间计算：按自然周，从本周星期一中午12:00~当天中午12:00");
            builder.Append(Out.Tab("</div>", ""));
            #endregion
        }
        //else if (act == "ReWardSent")//奖励发放页
        //{
        //    builder.Append(act);

        //}
        else
        {
            if (act == "ReWard")//奖励发放确认
            {
                string bcing = Utils.GetRequest("bcing", "get", 1, "", "");

                if (bcing == "ok")
                {

                    string ae = Utils.GetRequest("ae", "get", 1, "", "");

                    #region  发放前输出
                    Master.Title = "" + "奖励发放";
                    builder.Append(Out.Tab("<div class=\"title\">", ""));
                    builder.Append("<a href=\"" + Utils.getUrl("../game/default.aspx") + "\">游戏</a>&gt;");
                    builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">球彩竞猜</a>&gt;");
                    builder.Append("<a href=\"" + Utils.getUrl("topGuess.aspx") + "\">" + "排行榜" + "</a>&gt;奖励发放");
                    builder.Append(Out.Tab("</div>", "<br />"));
                    int[] IdRe1 = new int[11];
                    long[] Top = new long[11];
                    IdRe1[1] = int.Parse(Utils.GetRequest("IdRe1", "all", 1, "", "10086"));
                    IdRe1[2] = int.Parse(Utils.GetRequest("IdRe2", "all", 1, "", "10086"));
                    IdRe1[3] = int.Parse(Utils.GetRequest("IdRe3", "all", 1, "", "10086"));
                    IdRe1[4] = int.Parse(Utils.GetRequest("IdRe4", "all", 1, "", "10086"));
                    IdRe1[5] = int.Parse(Utils.GetRequest("IdRe5", "all", 1, "", "10086"));
                    IdRe1[6] = int.Parse(Utils.GetRequest("IdRe6", "all", 1, "", "10086"));
                    IdRe1[7] = int.Parse(Utils.GetRequest("IdRe7", "all", 1, "", "10086"));
                    IdRe1[8] = int.Parse(Utils.GetRequest("IdRe8", "all", 1, "", "10086"));
                    IdRe1[9] = int.Parse(Utils.GetRequest("IdRe9", "all", 1, "", "10086"));
                    IdRe1[10] = int.Parse(Utils.GetRequest("IdRe10", "all", 1, "", "10086"));
                    Top[1] = Convert.ToInt64(Utils.GetRequest("top1", "all", 1, "", ""));
                    //  Utils.Error(""+Top[1]+"","");
                    Top[2] = Convert.ToInt64(Utils.GetRequest("top2", "all", 1, "", ""));
                    Top[3] = Convert.ToInt64(Utils.GetRequest("top3", "all", 1, "", ""));
                    Top[4] = Convert.ToInt64(Utils.GetRequest("top4", "all", 1, "", ""));
                    Top[5] = Convert.ToInt64(Utils.GetRequest("top5", "all", 1, "", ""));
                    Top[6] = Convert.ToInt64(Utils.GetRequest("top6", "all", 1, "", ""));
                    Top[7] = Convert.ToInt64(Utils.GetRequest("top7", "all", 1, "", ""));
                    Top[8] = Convert.ToInt64(Utils.GetRequest("top8", "all", 1, "", ""));
                    Top[9] = Convert.ToInt64(Utils.GetRequest("top9", "all", 1, "", ""));
                    Top[10] = Convert.ToInt64(Utils.GetRequest("top10", "all", 1, "", ""));
                    int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-3]$", "1"));
                    //  int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));
                    DateTime startstate = Utils.ParseTime(Utils.GetRequest("startstate", "all", 2, DT.RegexTime, "开始时间填写无效"));
                    DateTime endstate = Utils.ParseTime(Utils.GetRequest("endstate", "all", 2, DT.RegexTime, "结束时间填写无效"));
                    int pageIndex = int.Parse(Utils.GetRequest("pageIndex", "all", 1, @"^[1-9]\d*$", "1"));
                    //  string rewardid = Utils.GetRequest("rewardid", "post", 2, @"^[\s\S]{1,500}$", "当前页无可操作用户");

                    string wdy = "";
                    if (pageIndex == 1)
                        wdy = "TOP10";
                    else
                        wdy = "TOP" + (pageIndex - 1).ToString() + "1-" + pageIndex.ToString() + "0";
                    // Utils.Error(""+ wdy + "","");
                    builder.Append(Out.Tab("<div class=\"text\">", ""));
                    builder.Append("正在发放");
                    switch (ptype)
                    {
                        case 1:
                            builder.Append("<font color =\"red\">《赌神榜》" + wdy + "奖励</font>");
                            wdy = "《赌神榜》" + wdy;
                            break;
                        case 2:
                            builder.Append("<font color =\"red\">《狂人榜》" + wdy + "奖励</font>");
                            wdy = "《狂人榜》" + wdy;
                            break;
                    }
                    builder.Append(",是否确认发放?");
                    builder.Append(Out.Tab("</div>", "<br/>"));
                    #endregion

                    if (ae == "ok")
                    {
                        #region 开始发放
                        for (int i = 1; i <= 10; i++)
                        {
                            if (Top[i] != 0)
                            {
                                //  Utils.Error(""+Top[3]+"=="+ IdRe1[1] + "","");
                                new BCW.BLL.User().UpdateiGold(IdRe1[i], Top[i], "竞猜排行榜奖励");
                                //发内线
                                string strLog = "您在" + startstate.ToString("yyyy年MM月dd日 HH:MM") + "至" + endstate.ToString("yyyy年MM月dd日 HH:MM") + "里在游戏《虚拟竞猜》" + wdy + "上取得了第" + i + "名的好成绩，系统奖励了" + Top[i] + "" + ub.Get("SiteBz") + "[url=/bbs/guess2/default.aspx]进入《虚拟竞猜》[/url]";
                                new BCW.BLL.Guest().Add(0, IdRe1[i], new BCW.BLL.User().GetUsName(IdRe1[i]), strLog);
                                //动态
                                string mename = new BCW.BLL.User().GetUsName(IdRe1[i]);
                                string wText = "[url=/bbs/uinfo.aspx?uid=" + IdRe1[i] + "]" + mename + "[/url]在[url=/bbs/guess2/default.aspx]竞猜[/url]" + wdy + "上取得了第" + i + "名的好成绩,系统奖励了" + Top[i] + "" + ub.Get("SiteBz");
                                new BCW.BLL.Action().Add(1, 0, IdRe1[i], "", wText);
                            }
                        }
                        Utils.Success("奖励操作", "TOP奖励操作成功,内线动态已发送！3s后返回...", Utils.getUrl("topGuess.aspx"), "3");
                        #endregion
                    }
                    else
                    {
                        // Utils.Error("" + acing + "---" + "", "");
                        #region 确认发放

                        builder.Append(Out.Tab("<div>", ""));
                        builder.Append("时间从:" + startstate.ToString("yyy-MM-dd HH:mm:ss") + "到" + endstate.ToString("yyy-MM-dd HH:mm:ss") + "<br/>");
                        for (int j = 1; j <= 10; j++)
                        {
                            if (j == 10)
                            {
                                builder.Append("TOP" + j + "：" + IdRe1[j] + ". " + "奖励<font color=\"red\"> " + Top[j] + "" + "</font> " + ub.Get("SiteBz") + " ");
                            }
                            else
                            {
                                builder.Append("TOP" + j + "：" + IdRe1[j] + ". " + "奖励<font color=\"red\"> " + Top[j] + "" + "</font> " + ub.Get("SiteBz") + " <br/>");
                            }
                        }

                        string strText2 = ",,,,,,,,,,,,,,,,,,,,,,,,,,,";
                        string strName2 = "ptype,pageIndex,ae,endstate,startstate,bcing,act,top1,top2,top3,top4,top5,top6,top7,top8,top9,top10,IdRe1,IdRe2,IdRe3,IdRe4,IdRe5,IdRe6,IdRe7,IdRe8,IdRe9,IdRe10";
                        string strType2 = "hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,";
                        string strValu2 = ptype + "'" + pageIndex + "'" + "ok'" + endstate.ToString("yyy-MM-dd HH:mm:ss") + "'" + startstate.ToString("yyy-MM-dd HH:mm:ss") + "'" + "ok'" + "ReWard'" + Top[1] + "'" + Top[2] + "'" + Top[3] + "'" + Top[4] + "'" + Top[5] + "'" + Top[6] + "'" + Top[7] + "'" + Top[8] + "'" + Top[9] + "'" + Top[10] + "'" + IdRe1[1] + "'" + IdRe1[2] + "'" + IdRe1[3] + "'" + IdRe1[4] + "'" + IdRe1[5] + "'" + IdRe1[6] + "'" + IdRe1[7] + "'" + IdRe1[8] + "'" + IdRe1[9] + "'" + IdRe1[10];
                        string strEmpt2 = "true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true";
                        string strIdea2 = "/";
                        string strOthe2 = "确定发放,topGuess.aspx, post,1,red";
                        builder.Append(Out.wapform(strText2, strName2, strType2, strValu2, strEmpt2, strIdea2, strOthe2));
                        builder.Append("<br/><a href=\"" + Utils.getUrl("topGuess.aspx") + "\">" + "再看看吧" + "</a>");

                        builder.Append(Out.Tab("</div>", ""));
                        #endregion
                    }

                }
                else
                {

                    #region 提交发放奖励
                    Master.Title = "" + "奖励发放";
                    builder.Append(Out.Tab("<div class=\"title\">", ""));
                    builder.Append("<a href=\"" + Utils.getUrl("../game/default.aspx") + "\">游戏</a>&gt;");
                    builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">球彩竞猜</a>&gt;");
                    builder.Append("<a href=\"" + Utils.getUrl("topGuess.aspx") + "\">" + "排行榜" + "</a>&gt;奖励发放");
                    builder.Append(Out.Tab("</div>", "<br />"));


                    int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));
                    DateTime startstate = Utils.ParseTime(Utils.GetRequest("startstate", "all", 2, DT.RegexTime, "开始时间填写无效"));
                    DateTime endstate = Utils.ParseTime(Utils.GetRequest("endstate", "all", 2, DT.RegexTime, "结束时间填写无效"));
                    //string startstate = (Utils.GetRequest("startstate", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8])))$", "20150101"));
                    //string endstate = (Utils.GetRequest("endstate", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8])))$", "20501231"));
                    int pageIndex = int.Parse(Utils.GetRequest("pageIndex", "all", 1, @"^[1-9]\d*$", "1"));
                    string rewardid = Utils.GetRequest("rewardid", "post", 2, @"^[\s\S]{1,500}$", "当前页无可操作用户");//"727#727#727#727#727#727#727#727#727#727#";


                    //builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
                    string wdy = "";
                    if (pageIndex == 1)
                        wdy = "TOP10";
                    else
                        wdy = "TOP" + (pageIndex - 1).ToString() + "1-" + pageIndex.ToString() + "0";
                    builder.Append(Out.Tab("<div>", ""));
                    switch (ptype)
                    {
                        case 1:
                            builder.Append("<font color =\"red\">《赌神榜》" + wdy + "奖励发放：</font>");
                            break;
                        case 2:
                            builder.Append("<font color =\"red\">《狂人榜》" + wdy + "奖励发放：</font>");
                            break;
                    }
                    builder.Append(Out.Tab("</div>", ""));

                    int mzj = (pageIndex - 1) * 10;
                    string[] IdRe = rewardid.Split('#');
                    // Utils.Error("" + ptype + "", "");
                    //  Utils.Error(""+rewardid+"","");
                    try
                    {
                        string strText2 = ",,,,TOP" + (mzj + 1) + "：" + IdRe[0] + "&nbsp;&nbsp;,,TOP" + (mzj + 2) + "：" + IdRe[1] + "&nbsp;&nbsp;,,TOP" + (mzj + 3) + "：" + IdRe[2] + "&nbsp;&nbsp;,,TOP" + (mzj + 4) + "：" + IdRe[3] + "&nbsp;&nbsp;,,TOP" + (mzj + 5) + "：" + IdRe[4] + "&nbsp;&nbsp;,,TOP" + (mzj + 6) + "：" + IdRe[5] + "&nbsp;&nbsp;,,TOP" + (mzj + 7) + "：" + IdRe[6] + "&nbsp;&nbsp;,,TOP" + (mzj + 8) + "：" + IdRe[7] + "&nbsp;&nbsp;,,TOP" + (mzj + 9) + "：" + IdRe[8] + "&nbsp;&nbsp;,,TOP" + pageIndex * 10 + "：" + IdRe[9] + "&nbsp;&nbsp;,";
                        string strName2 = "pageIndex,ptype,startstate,endstate,top1,IdRe1,top2,IdRe2,top3,IdRe3,top4,IdRe4,top5,IdRe5,top6,IdRe6,top7,IdRe7,top8,IdRe8,top9,IdRe9,top10,IdRe10";
                        string strType2 = "hidden,hidden,hidden,hidden,num,hidden,num,hidden,num,hidden,num,hidden,num,hidden,num,hidden,num,hidden,num,hidden,num,hidden,num,hidden";
                        string strValu2 = pageIndex + "'" + ptype + "'" + DT.FormatDate(startstate, 0) + "'" + DT.FormatDate(endstate, 0) + "'" + "0'" + IdRe[0] + "'0'" + IdRe[1] + "'0'" + IdRe[2] + "'0'" + IdRe[3] + "'0'" + IdRe[4] + "'0'" + IdRe[5] + "'0'" + IdRe[6] + "'0'" + IdRe[7] + "'0'" + IdRe[8] + "'0'" + IdRe[9] + "'0";
                        string strEmpt2 = "true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true";
                        string strIdea2 = "/";
                        string strOthe2 = "提交,topGuess.aspx?act=ReWard&amp;bcing=ok&amp;ae=not&amp;,post,1,red";
                        builder.Append(Out.wapform(strText2, strName2, strType2, strValu2, strEmpt2, strIdea2, strOthe2));
                    }
                    catch
                    {
                        builder.Append(Out.Tab("<div>", "<br/>"));
                        builder.Append("当页少于10人，无法发放！");
                        builder.Append(Out.Tab("</div>", ""));

                    }
                    builder.Append(Out.Tab("</div>", Out.Hr()));
                    #endregion
                }

            }
            else//正常模式
            {

                #region 查询开始
                int p_type = Utils.ParseInt(Utils.GetRequest("p_type", "get", 1, @"^[0-2]$", "0"));
                int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[1-2]$", "1"));
                int showtype = Utils.ParseInt(Utils.GetRequest("showtype", "get", 1, @"^[0-2]$", "0"));
                DateTime StartTime = DateTime.Now.AddMonths(-1);
                DateTime OverTime = DateTime.Now;
                //组件查询条件
                string strWhere = "";

                if (act == "seachok")
                {
                    StartTime = Utils.ParseTime(Utils.GetRequest("StartTime", "get", 2, DT.RegexTime, "开始时间格式填写出错,正确格式如" + DateTime.Now.ToString("yyy-MM-dd HH:mm:ss") + ""));
                    OverTime = Utils.ParseTime(Utils.GetRequest("OverTime", "get", 2, DT.RegexTime, "结束时间格式填写出错,正确格式如" + DateTime.Now.ToString("yyy-MM-dd HH:mm:ss") + ""));
                }


                #region 选项
                Master.Title = "竞猜排行榜";
                builder.Append(Out.Tab("<div class=\"title\">", ""));

                if (showtype == 0)
                    builder.Append("全部日期 ");
                else
                    builder.Append(Out.waplink(Utils.getUrl("topGuess.aspx?showtype=0"), "全部日期") + " ");


                if (showtype == 1)
                    builder.Append("本周 ");
                else
                    builder.Append(Out.waplink(Utils.getUrl("topGuess.aspx?showtype=1"), "本周") + " ");


                if (showtype == 2)
                    builder.Append("本月 ");
                else
                    builder.Append(Out.waplink(Utils.getUrl("topGuess.aspx?showtype=2"), "本月") + " ");

                builder.Append(Out.waplink(Utils.getUrl("topGuess.aspx?act=seach"), "更多") + " ");

                builder.Append("<br />榜:");
                if (p_type == 0)
                    builder.Append("总榜 ");
                else
                    builder.Append(Out.waplink(Utils.getUrl("topGuess.aspx?p_type=0&amp;ptype=" + ptype + "&amp;showtype=" + showtype + ""), "总榜") + " ");
                if (p_type == 1)
                    builder.Append("足球 ");
                else
                    builder.Append(Out.waplink(Utils.getUrl("topGuess.aspx?p_type=1&amp;ptype=" + ptype + "&amp;showtype=" + showtype + ""), "足球") + " ");

                if (p_type == 2)
                    builder.Append("篮球 ");
                else
                    builder.Append(Out.waplink(Utils.getUrl("topGuess.aspx?p_type=2&amp;ptype=" + ptype + "&amp;showtype=" + showtype + ""), "篮球") + " ");

                builder.Append("<br />单:");

                if (ptype == 1)
                    builder.Append("赌神榜 ");
                else
                    builder.Append(Out.waplink(Utils.getUrl("topGuess.aspx?p_type=" + p_type + "&amp;ptype=1&amp;showtype=" + showtype + ""), "赌神榜") + " ");
                if (ptype == 2)
                    builder.Append("狂人榜 ");
                else
                    builder.Append(Out.waplink(Utils.getUrl("topGuess.aspx?p_type=" + p_type + "&amp;ptype=2&amp;showtype=" + showtype + ""), "狂人榜") + " ");

                builder.Append(Out.Tab("</div>", "<br />"));
                #endregion



                if (p_type > 0)
                    strWhere += "pType=" + p_type + " and ";

                strWhere += " p_active>0 and types = 0 and itypes=0";

                if (showtype == 1)
                {

                    string dt3 = Convert.ToDateTime(DT.GetWeekStart()).AddDays(-1).AddHours(12).ToString();
                    string dt4 = Convert.ToDateTime(DT.GetWeekOver()).AddHours(12).ToString();

                    strWhere += " and p_TPRtime>='" + dt3 + "'and p_TPRtime<='" + dt4 + "' ";
                }
                else if (showtype == 2)
                {
                    strWhere += " and Year(p_TPRtime) = " + (DateTime.Now.Year) + " AND Month(p_TPRtime) = " + (DateTime.Now.Month) + "";
                }
                else
                {
                    if (act == "seachok")
                    {
                        strWhere += " and p_TPRtime>='" + StartTime + "'and p_TPRtime<'" + OverTime + "' ";
                    }
                }
                if (act == "timespan")//增加时间段查询前10ID
                {
                    StartTime = Utils.ParseTime(Utils.GetRequest("StartTime", "all", 1, "", "开始时间格式填写出错,正确格式如" + DateTime.Now.ToString("yyy-MM-dd HH:mm:ss") + ""));
                    OverTime = Utils.ParseTime(Utils.GetRequest("OverTime", "all", 1, "", "结束时间格式填写出错,正确格式如" + DateTime.Now.ToString("yyy-MM-dd HH:mm:ss") + ""));
                    // Utils.Error("" + StartTime + "", "");
                    strWhere += " and p_TPRtime>='" + StartTime + "'and p_TPRtime<'" + OverTime + "' ";
                }
                string rewardid = "";
                int pageSize = 10;
                int pageIndex;
                int recordCount;
                string[] pageValUrl = { "act", "p_type", "ptype", "showtype", "StartTime", "OverTime" };
                pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                if (pageIndex == 0)
                    pageIndex = 1;

                // 开始读取排行榜
                IList<TPR2.Model.guess.BaPay> listBaPay = new TPR2.BLL.guess.BaPay().GetBaPayTop2(pageIndex, pageSize, strWhere, ptype, out recordCount);
                if (listBaPay.Count > 0)
                {
                    int k = 1;
                    foreach (TPR2.Model.guess.BaPay n in listBaPay)
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
                            builder.AppendFormat("[第{0}名]" + Out.waplink(Utils.getUrl("../uinfo.aspx?uid={1}&amp;backurl=" + Utils.PostPage(1)) + "", "{2}(" + n.payusid + ")") + "盈利{3}币", (pageIndex - 1) * 10 + k, n.payusid, new BCW.BLL.User().GetUsName(Convert.ToInt32(n.payusid)), Convert.ToDouble(n.payCount));
                        else if (ptype == 2)
                            builder.AppendFormat("[第{0}名]" + Out.waplink(Utils.getUrl("../uinfo.aspx?uid={1}&amp;backurl=" + Utils.PostPage(1)) + "", "{2}(" + n.payusid + ")") + "净胜{3}场", (pageIndex - 1) * 10 + k, n.payusid, new BCW.BLL.User().GetUsName(Convert.ToInt32(n.payusid)), Convert.ToDouble(n.payCount));
                        if (act == "timespan")
                        { rewardid = rewardid + n.payusid.ToString() + "#"; }
                        builder.Append(Out.Tab("</div>", ""));

                        k++;
                    }

                    // 分页
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                }
                else
                {
                    builder.Append(Out.Div("div", "没有相关记录.."));
                }
                #endregion


                if (p_type == 0 && showtype == 0)//总榜
                {

                    //top 奖励发放
                    string strText = "开始日期：/,结束日期：/,";
                    string strName = "StartTime,OverTime,backurl";
                    string strType = "date,date,hidden";
                    string strValu = string.Empty;
                    //if (Utils.ToSChinese(ac) != "马上查询")
                    {
                        //strValu = "" + DT.FormatDate(DateTime.Now.AddDays(-1), 0) + "'" + DT.FormatDate(DateTime.Now, 0) + "'" + Utils.getPage(0) + "";
                    }
                    //else
                    {
                        strValu = StartTime.ToString("yyy-MM-dd HH:mm:ss") + "'" + OverTime.ToString("yyy-MM-dd HH:mm:ss") + "'" + Utils.getPage(0) + "";
                    }
                    string strEmpt = "false,false,false";
                    string strIdea = "/";
                    string strOthe = "马上查询,topGuess.aspx?act=timespan&amp;ptype=" + ptype + "&amp;,post,1,red";
                    builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

                    if (Utils.ToSChinese(act) != "timespan")
                    {
                        builder.Append(Out.Tab("<div>", Out.Hr()));
                        builder.Append("排行榜奖励提示：<br/>");
                        builder.Append("如需发放奖励，请按日期查询.");
                        builder.Append(Out.Tab("</div>", ""));
                    }
                    else
                    {
                        string wdy = "";
                        string type = "";
                        if (ptype == 1)
                        {
                            type = "赌神榜";
                        }
                        else
                        {
                            type = "狂人榜";
                        }
                        if (pageIndex == 1)
                            wdy = "TOP10";
                        else
                            wdy = "TOP" + (pageIndex - 1).ToString() + "1-" + pageIndex.ToString() + "0";
                        if (rewardid != "")
                        {
                            builder.Append(Out.Tab("<div>", Out.Hr()));
                            builder.Append("<font color =\"red\">" + type + wdy + " </font>的用户ID分别是：" + rewardid);
                            builder.Append(Out.Tab("</div>", ""));
                            string strText2 = ",,,,";
                            string strName2 = "startstate,endstate,pageIndex,rewardid,backurl";
                            string strType2 = "hidden,hidden,hidden,hidden,hidden";
                            string strValu2 = DT.FormatDate(StartTime, 0) + "'" + DT.FormatDate(OverTime, 0) + "'" + pageIndex + "'" + rewardid + "'" + Utils.getPage(0) + "";
                            string strEmpt2 = "true,true,false";
                            string strIdea2 = "/";
                            string strOthe2 = wdy + "奖励发放,topGuess.aspx?act=ReWard&amp;ptype=" + ptype + "&amp;showtype=0&amp;,post,1,red";
                            builder.Append(Out.wapform(strText2, strName2, strType2, strValu2, strEmpt2, strIdea2, strOthe2));
                        }
                    }
                }



                int meid = new BCW.User.Users().GetUsId();
                if (meid > 0)
                {
                    builder.Append(Out.Tab("<div>", Out.Hr()));
                    DataSet ds = new TPR2.BLL.guess.BaPay().GetBaPayList("sum(p_getMoney-payCent) as WinCents", " payusid=" + meid + " and types=0 and itypes=0 and p_active>0");
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        string win = ds.Tables[0].Rows[0]["WinCents"].ToString();
                        if (!string.IsNullOrEmpty(win))
                            builder.Append("我的战绩:盈利" + Convert.ToDouble(win) + "" + ub.Get("SiteBz") + "");
                        else
                            builder.Append("我的战绩:盈利0" + ub.Get("SiteBz") + "");
                    }
                    else
                    {
                        builder.Append("我的战绩:盈利0" + ub.Get("SiteBz") + "");
                    }
                    builder.Append(Out.Tab("</div>", ""));
                }
            }
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.waplink(Utils.getUrl("default.aspx"), "返回上一级"));
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
}
