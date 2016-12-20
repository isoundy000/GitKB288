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

public partial class bbs_guess2_topGuess : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string Strbuilder = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "get", 1, "", "");
        if (act == "seach")
        {
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
        }
        else if (act == "todaytop")
        {
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
                    builder.Append("[第" + (i + 1) + "名]<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + ds.Tables[0].Rows[i]["payusid"].ToString() + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(int.Parse(ds.Tables[0].Rows[i]["payusid"].ToString())) + "</a>(获胜" + ds.Tables[0].Rows[i]["payCount"] + "场)<br />");
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
        }
        else if (act == "todaytop2")
        {
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
                    builder.Append("[第" + (i + 1) + "名]<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + ds.Tables[0].Rows[i]["payusid"].ToString() + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(int.Parse(ds.Tables[0].Rows[i]["payusid"].ToString())) + "</a>(获胜" + ds.Tables[0].Rows[i]["payCount"] + "场)<br />");
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
        }
        else if (act == "todaystar")
        {
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
                    builder.Append("[第" + (i + 1) + "名]<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + ds.Tables[0].Rows[i]["payusid"].ToString() + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(int.Parse(ds.Tables[0].Rows[i]["payusid"].ToString())) + "</a>(净赚" + Convert.ToDouble(ds.Tables[0].Rows[i]["payCents"]) + "" + ub.Get("SiteBz") + ")<br />");
                }
            }
            else
                builder.Append("暂无数据..<br />");

            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.waplink(Utils.getUrl("topGuess.aspx"), "返回排行首页") + "");
            builder.Append("<br />注:昨日时间计算：上一天（昨天）12:00~当天中午12:00");
            builder.Append(Out.Tab("</div>", ""));
        }
        else if (act == "todaystar2")
        {
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
                    builder.Append("[第" + (i + 1) + "名]<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + ds.Tables[0].Rows[i]["payusid"].ToString() + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(int.Parse(ds.Tables[0].Rows[i]["payusid"].ToString())) + "</a>(净赚" + Convert.ToDouble(ds.Tables[0].Rows[i]["payCents"]) + "" + ub.Get("SiteBz") + ")<br />");
                }
            }
            else
                builder.Append("暂无数据..<br />");

            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.waplink(Utils.getUrl("topGuess.aspx"), "返回排行首页") + "");
            builder.Append("<br />注:一周时间计算：按自然周，从本周星期一中午12:00~当天中午12:00");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            int p_type = Utils.ParseInt(Utils.GetRequest("p_type", "get", 1, @"^[0-2]$", "0"));
            int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[1-2]$", "1"));
            int showtype = Utils.ParseInt(Utils.GetRequest("showtype", "get", 1, @"^[0-2]$", "0"));
            DateTime StartTime = DateTime.Now;
            DateTime OverTime = DateTime.Now;
            if (act == "seachok")
            {
                StartTime = Utils.ParseTime(Utils.GetRequest("StartTime", "get", 2, DT.RegexTime, "开始时间格式填写出错,正确格式如" + DateTime.Now.ToString("yyy-MM-dd HH:mm:ss") + ""));
                OverTime = Utils.ParseTime(Utils.GetRequest("OverTime", "get", 2, DT.RegexTime, "结束时间格式填写出错,正确格式如" + DateTime.Now.ToString("yyy-MM-dd HH:mm:ss") + ""));
            }
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

            //组件查询条件
            string strWhere = "";

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
                        builder.AppendFormat("[第{0}名]" + Out.waplink(Utils.getUrl("/bbs/uinfo.aspx?uid={1}&amp;backurl=" + Utils.PostPage(1)) + "", "{2}(" + n.payusid + ")") + "盈利{3}币", (pageIndex - 1) * 10 + k, n.payusid, new BCW.BLL.User().GetUsName(Convert.ToInt32(n.payusid)), Convert.ToDouble(n.payCount));
                    else if (ptype == 2)
                        builder.AppendFormat("[第{0}名]" + Out.waplink(Utils.getUrl("/bbs/uinfo.aspx?uid={1}&amp;backurl=" + Utils.PostPage(1)) + "", "{2}(" + n.payusid + ")") + "净胜{3}场", (pageIndex - 1) * 10 + k, n.payusid, new BCW.BLL.User().GetUsName(Convert.ToInt32(n.payusid)), Convert.ToDouble(n.payCount));

                    builder.Append(Out.Tab("</div>", ""));

                    k++;
                }

                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, 100, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录.."));
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
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append(Out.waplink(Utils.getUrl("myGuess.aspx?ptype=1"), "未开投注") + " ");
        builder.Append(Out.waplink(Utils.getUrl("myGuess.aspx?ptype=2"), "历史投注") + "<br />");
        builder.Append(Out.waplink(Utils.getUrl("default.aspx"), "返回球彩首页") + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(Out.waplink(Utils.getUrl("/default.aspx"), "首页") + "-");
        builder.Append(Out.waplink(Utils.getPage("default.aspx"), "上级") + "-");
        builder.Append(Out.waplink(Utils.getUrl("default.aspx"), "球彩") + "");
        builder.Append(Out.Tab("</div>", ""));
    }
}
