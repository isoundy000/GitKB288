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
using System.Drawing;

public partial class Manage_app_stat : System.Web.UI.Page
{
    //所占百分比
    private float Percent(int i, int total)
    {
        float P_Fl_percent = 0;
        if (total != 0)
        {
            P_Fl_percent = Convert.ToSingle(i) / Convert.ToSingle(total);
        }
        return P_Fl_percent;
    }
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    private string M_Str_mindate = string.Empty;
    private string M_Str_maxdate = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "网站统计分析";
        string act = Utils.GetRequest("act", "get", 1, "", "");
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-9]$", "1"));
        if (act == "help")
        {
            HelpPage();
        }
        else if (act == "clear")
        {
            ClearPage();
        }
        else if (act == "clearok")
        {
            ClearOkPage();
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("网站统计分析");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div class=\"text\">", ""));
            if (ptype == 1)
                builder.Append("基本|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("stat.aspx?ptype=1") + "\">基本</a>|");

            if (ptype == 2)
                builder.Append("本日|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("stat.aspx?ptype=2") + "\">本日</a>|");

            if (ptype == 3)
                builder.Append("本月|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("stat.aspx?ptype=3") + "\">本月</a>|");

            if (ptype == 4)
                builder.Append("本年|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("stat.aspx?ptype=4") + "\">本年</a>|");

            if (ptype == 5)
                builder.Append("IP|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("stat.aspx?ptype=5") + "\">IP</a>|");

            if (ptype == 6)
                builder.Append("UA|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("stat.aspx?ptype=6") + "\">UA</a>|");

            if (ptype == 7)
                builder.Append("OS|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("stat.aspx?ptype=7") + "\">OS</a>|");

            if (ptype == 8)
                builder.Append("页面");
            else
                builder.Append("<a href=\"" + Utils.getUrl("stat.aspx?ptype=8") + "\">页面</a>");

            //if (ptype == 9)
            //    builder.Append("图表");
            //else
            //    builder.Append("<a href=\"" + Utils.getUrl("stat.aspx?ptype=9") + "\">图表</a>");

            builder.Append(Out.Tab("</div>", ""));

            switch (ptype)
            {
                case 1:
                    ReloadPage();
                    break;
                case 2:
                    DayPage();
                    break;
                case 3:
                    MonthPage();
                    break;
                case 4:
                    YearPage();
                    break;
                case 5:
                    IpPage();
                    break;
                case 6:
                    BrowserPage();
                    break;
                case 7:
                    SystemPage();
                    break;
                case 8:
                    PUrlPage();
                    break;
                case 9:
                    ImagePage();
                    break;
                default:
                    ReloadPage();
                    break;
            }
        }
        builder.Append(Out.Tab("", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        if (act == "")
        {
            builder.Append("<a href=\"" + Utils.getUrl("stat.aspx?act=help") + "\">什么叫IP/UV/PV?</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("stat.aspx?act=clear") + "\">管理统计</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("stat.aspx?ptype=9") + "\">看即时图表</a><br />");

        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("stat.aspx") + "\">返回上一级</a><br />");
        }
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">系统服务中心</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    private void ReloadPage()
    {
        string M_Str_mindate;//用于存储最小日期
        string M_Str_maxdate;//用于存储最大日期
        //显示统计时间
        string sCountDate = DateTime.Now.ToString();
        //本日访问人数的计算
        M_Str_mindate = DateTime.Now.ToShortDateString() + " 0:00:00";
        M_Str_maxdate = DateTime.Now.AddDays(1).ToShortDateString() + " 0:00:00";

        string sCountDay = new BCW.BLL.Statinfo().GetCount("AddTime>='" + M_Str_mindate + "'and AddTime<'" + M_Str_maxdate + "'").ToString();

        string sCountDayIP = new BCW.BLL.Statinfo().GetIPCount("AddTime>='" + M_Str_mindate + "'and AddTime<'" + M_Str_maxdate + "'").ToString();

        //本周访问人数
        switch (DateTime.Now.DayOfWeek)
        {
            case DayOfWeek.Monday:
                M_Str_mindate = DateTime.Now.AddDays(0).ToShortDateString() + " 0:00:00";
                M_Str_maxdate = DateTime.Now.AddDays(6).ToShortDateString() + " 0:00:00";
                break;
            case DayOfWeek.Tuesday:
                M_Str_mindate = DateTime.Now.AddDays(-1).ToShortDateString() + " 0:00:00";
                M_Str_maxdate = DateTime.Now.AddDays(5).ToShortDateString() + " 0:00:00";
                break;
            case DayOfWeek.Wednesday:
                M_Str_mindate = DateTime.Now.AddDays(-2).ToShortDateString() + " 0:00:00";
                M_Str_maxdate = DateTime.Now.AddDays(4).ToShortDateString() + " 0:00:00";
                break;
            case DayOfWeek.Thursday:
                M_Str_mindate = DateTime.Now.AddDays(-3).ToShortDateString() + " 0:00:00";
                M_Str_maxdate = DateTime.Now.AddDays(3).ToShortDateString() + " 0:00:00";
                break;
            case DayOfWeek.Friday:
                M_Str_mindate = DateTime.Now.AddDays(-4).ToShortDateString() + " 0:00:00";
                M_Str_maxdate = DateTime.Now.AddDays(2).ToShortDateString() + " 0:00:00";
                break;
            case DayOfWeek.Saturday:
                M_Str_mindate = DateTime.Now.AddDays(-5).ToShortDateString() + " 0:00:00";
                M_Str_maxdate = DateTime.Now.AddDays(1).ToShortDateString() + " 0:00:00";
                break;
            case DayOfWeek.Sunday:
                M_Str_mindate = DateTime.Now.AddDays(-6).ToShortDateString() + " 0:00:00";
                M_Str_maxdate = DateTime.Now.AddDays(0).ToShortDateString() + " 0:00:00";
                break;
        }
        string sCountWeek = new BCW.BLL.Statinfo().GetCount("AddTime>='" + M_Str_mindate + "'and AddTime<'" + M_Str_maxdate + "'").ToString();
        string sCountWeekIP = new BCW.BLL.Statinfo().GetIPCount("AddTime>='" + M_Str_mindate + "'and AddTime<'" + M_Str_maxdate + "'").ToString();
        //本月访问人数
        string sCountMonth = new BCW.BLL.Statinfo().GetCount("Year(AddTime)=" + DateTime.Now.Year + " and Month(AddTime)=" + DateTime.Now.Month.ToString()).ToString();
        string sCountMonthIP = new BCW.BLL.Statinfo().GetIPCount("Year(AddTime)=" + DateTime.Now.Year + " and Month(AddTime)=" + DateTime.Now.Month.ToString()).ToString();
        //最高日访问量
        DataSet ds = new BCW.BLL.Statinfo().GetList("Select COUNT(ID) AS count, MAX(AddTime) AS date From tb_Statinfo GROUP BY YEAR(AddTime), MONTH(AddTime), DAY(AddTime)");
        int P_Int_max = 0;//最大值
        string P_Str_date = "";
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            if (!dr.IsNull(0))
            {
                if (P_Int_max <= Convert.ToInt32(dr[0]))
                {
                    P_Int_max = Convert.ToInt32(dr[0]);
                    P_Str_date = Convert.ToDateTime(dr[1]).ToShortDateString();
                }
            }
        }
        string sMaxCountDay = P_Int_max.ToString();
        //最高日访问日期
        string sMaxCountDayDate="";
        if (P_Str_date != "")
        {
            DateTime P_Date_date = Convert.ToDateTime(P_Str_date);
            sMaxCountDayDate = P_Date_date.Year + "年" + P_Date_date.Month + "月" + P_Date_date.Day + "日";
        }

        //最高月访问量
        P_Int_max = 0;//最大值
        P_Str_date = "";
        ds = new BCW.BLL.Statinfo().GetList("SELECT YEAR(AddTime) FROM tb_Statinfo GROUP BY YEAR(AddTime)");
        foreach (DataRow drYear in ds.Tables[0].Rows)
        {
            drYear[0].ToString();
            DataSet dsMonth = new BCW.BLL.Statinfo().GetList("SELECT COUNT(*) as count, MAX(Month(AddTime)) as month FROM tb_Statinfo where YEAR(AddTime)=" + drYear[0].ToString() + " GROUP BY Month(AddTime)");
            foreach (DataRow drMonth in dsMonth.Tables[0].Rows)
            {
                if (!drMonth.IsNull(0))
                {
                    if (P_Int_max <= Convert.ToInt32(drMonth[0]))
                    {
                        P_Int_max = Convert.ToInt32(drMonth[0]);
                        P_Str_date = drYear[0].ToString() + "年" + drMonth[1].ToString() + "月";
                    }
                }
            }
        }
        string sMaxCountMonth = P_Int_max.ToString();
        //最高月访问日期
        string sMaxCountMonthDate = P_Str_date;

        //最高年访问量
        ds = new BCW.BLL.Statinfo().GetList("SELECT COUNT(ID), MAX(Year(AddTime)) FROM tb_Statinfo GROUP BY Year(AddTime)");
        P_Int_max = 0;//最大值
        P_Str_date = "";
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            if (!dr.IsNull(0))
            {
                if (P_Int_max <= Convert.ToInt32(dr[0]))
                {
                    P_Int_max = Convert.ToInt32(dr[0]);
                    P_Str_date = dr[1].ToString() + "年";
                }
            }
        }
        string sMaxCountYear = P_Int_max.ToString();
        //最高年访问日期
        string sMaxCountYearDate = P_Str_date;

        //总访问人数
        string sTotalCount = new BCW.BLL.Statinfo().GetCount("").ToString();
        string sTotalCountIP = new BCW.BLL.Statinfo().GetIPCount("").ToString();

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("本日访问:" + sCountDay + " IP:" + sCountDayIP + "");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("本周访问:" + sCountWeek + " IP:" + sCountWeekIP + "");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("本月访问:" + sCountMonth + " IP:" + sCountMonthIP + "");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("总访问:" + sTotalCount + " IP:" + sTotalCountIP + "");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("最高日访问:" + sMaxCountDay + "");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("最高日访问日期:" + sMaxCountDayDate + "");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("最高月访问:" + sMaxCountMonth + "");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("最高月访问日期:" + sMaxCountMonthDate + "");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("最高年访问:" + sMaxCountYear + "");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("最高年访问日期:" + sMaxCountYearDate + "");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        string VCountTypeName = string.Empty;
        if (ub.Get("SiteVCountType") == "0")
            VCountTypeName = "按UV";
        else if (ub.Get("SiteVCountType") == "1")
            VCountTypeName = "按PV";
        else
            VCountTypeName = "已关闭";

        builder.Append("温馨提示:统计按自然日、周、月等计算，当前" + VCountTypeName + "统计");
        builder.Append(Out.Tab("</div>", ""));

    }
    /// <summary>
    /// 本日统计
    /// </summary>
    private void DayPage()
    {
        string DayText = DateTime.Now.ToShortDateString();//当日日期
        M_Str_mindate = DateTime.Now.ToShortDateString() + " 0:00:00";
        M_Str_maxdate = DateTime.Now.AddDays(1).ToShortDateString() + " 0:00:00";
        //计算总量
        int total = new BCW.BLL.Statinfo().GetCount("AddTime>='" + M_Str_mindate + "'and AddTime<'" + M_Str_maxdate + "'");

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("日期:" + DayText + " 累计流量" + total + "");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("时间|访问量|比例");
        builder.Append(Out.Tab("</div>", ""));

        int[] P_Int_hour = new int[24];
        for (int i = 0; i < 24; i++)
        {
            if (i % 2 == 0)
                builder.Append(Out.Tab("<div>", "<br />"));
            else
                builder.Append(Out.Tab("<div>", "<br />"));

            if (!Utils.Isie())
                builder.Append(Time(i) + "—" + HourCount(i) + "—" + (Convert.ToInt32(Percent(HourCount(i), total) * 100)) + "%");
            else
                builder.Append(Time(i) + "—" + HourCount(i) + "—<img src=\"/Files/sys/bar.gif\" height=\"10\" width=\"" + Percent(HourCount(i), total) * 100 + "\" />" + Convert.ToInt32(Percent(HourCount(i), total) * 100) + "%");

            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private string Time(int i)
    {
        string P_Str_time = "";
        if (i < 24 & i >= 0)
        {
            P_Str_time = i.ToString() + ":00--" + Convert.ToString(i + 1) + ":00";
        }
        return P_Str_time;
    }
    //各时间段的统计人数
    private int HourCount(int i)
    {
        int P_Int_count = 0;
        P_Int_count = new BCW.BLL.Statinfo().GetCount("AddTime>='" + M_Str_mindate + "'and AddTime<'" + M_Str_maxdate + "' and DATEPART(hh,AddTime)=" + i + " GROUP BY DATEPART(hh, AddTime)");

        return P_Int_count;
    }

    /// <summary>
    /// 本月统计
    /// </summary>
    private void MonthPage()
    {
        int P_Int_DaysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);//指定年和月中的天数
        string MonthText = DateTime.Now.Year + "-" + DateTime.Now.Month;
        M_Str_mindate = MonthText + "-1 0:00:00";
        M_Str_maxdate = MonthText + "-" + P_Int_DaysInMonth + " 23:59:59";
        //计算总量
        int total = new BCW.BLL.Statinfo().GetCount("AddTime>='" + M_Str_mindate + "'and AddTime<'" + M_Str_maxdate + "'");

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("月份:" + MonthText + " 累计流量" + total + "");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("日期|访问量|比例");
        builder.Append(Out.Tab("</div>", ""));


        int[] P_Int_Day = new int[P_Int_DaysInMonth];
        for (int i = 0; i < P_Int_DaysInMonth; i++)
        {
            if (i % 2 == 0)
                builder.Append(Out.Tab("<div>", "<br />"));
            else
                builder.Append(Out.Tab("<div>", "<br />"));

            if (!Utils.Isie())
                builder.Append(Day(i) + "—" + DayCount(i) + "—" + (Convert.ToInt32(Percent(DayCount(i), total) * 100)) + "%");
            else
                builder.Append(Day(i) + "—" + DayCount(i) + "—<img src=\"/Files/sys/bar.gif\" height=\"10\" width=\"" + Percent(DayCount(i), total) * 100 + "\" />" + Convert.ToInt32(Percent(DayCount(i), total) * 100) + "%");

            builder.Append(Out.Tab("</div>", ""));
        }
    }

    public string Day(int i)
    {
        int P_Int_DaysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);//指定年和月中的天数
        string P_Str_day = "";
        if (i < P_Int_DaysInMonth & i >= 0)
        {
            P_Str_day = (i + 1).ToString();
        }
        return P_Str_day;
    }

    //每日统计人数
    public int DayCount(int i)
    {
        int P_Int_count = 0;
        P_Int_count = new BCW.BLL.Statinfo().GetCount("AddTime<'" + M_Str_maxdate + "' and DATEPART(dd,AddTime)=" + (i + 1) + " GROUP BY DATEPART(dd, AddTime)");

        return P_Int_count;
    }

    /// <summary>
    /// 本年统计
    /// </summary>
    private void YearPage()
    {
        string YearText = DateTime.Now.Year + "年";

        //计算总量
        int total = new BCW.BLL.Statinfo().GetCount("Year(AddTime)=" + DateTime.Now.Year);

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("年份:" + YearText + " 累计流量" + total + "");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("月份|访问量|比例");
        builder.Append(Out.Tab("</div>", ""));

        int[] P_Int_month = new int[12];
        for (int i = 0; i < 12; i++)
        {
            if (i % 2 == 0)
                builder.Append(Out.Tab("<div>", "<br />"));
            else
                builder.Append(Out.Tab("<div>", "<br />"));

            if (!Utils.Isie())
                builder.Append(Year(i) + "—" + MonthCount(i) + "—" + (Convert.ToInt32(Percent(MonthCount(i), total) * 100)) + "%");
            else
                builder.Append(Year(i) + "—" + MonthCount(i) + "—<img src=\"/Files/sys/bar.gif\" height=\"10\" width=\"" + Percent(MonthCount(i), total) * 100 + "\" />" + Convert.ToInt32(Percent(MonthCount(i), total) * 100) + "%");

            builder.Append(Out.Tab("</div>", ""));
        }
    }

    public string Year(int i)
    {
        string P_Str_year = "";
        if (i < 12 & i >= 0)
        {
            P_Str_year = Convert.ToString(i + 1);
        }
        return P_Str_year;
    }

    //每月统计人数
    public int MonthCount(int i)
    {
        int P_Int_count = 0;
        P_Int_count = new BCW.BLL.Statinfo().GetCount("Year(AddTime)=" + DateTime.Now.Year + "and Month(AddTime)=" + (i + 1) + " Group By Month(AddTime)");

        return P_Int_count;
    }

    /// <summary>
    /// IP统计
    /// </summary>
    private void IpPage()
    {

        //计算总量
        int total = new BCW.BLL.Statinfo().GetCount("");

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("累计流量" + total + "");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("IP|访问量|比例");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string[] pageValUrl = { "ptype" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Statinfo> listStatinfo = new BCW.BLL.Statinfo().GetIPs(pageIndex, pageSize, out recordCount);
        if (listStatinfo.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Statinfo n in listStatinfo)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div>", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }

                string ipCity = new IPSearch().GetAddressWithIP(n.IP);
                if (!string.IsNullOrEmpty(ipCity))
                {
                    ipCity = ipCity.Replace("IANA保留地址  CZ88.NET", "本机地址").Replace("CZ88.NET", "") + ":";
                }
                if (!Utils.Isie())
                    builder.Append(ipCity + "" + n.IP + "—" + n.IpCount + "—" + (Convert.ToInt32(Percent(n.IpCount, total) * 100)) + "%");
                else
                    builder.Append(ipCity + "" + n.IP + "—" + n.IpCount + "—<img src=\"/Files/sys/bar.gif\" height=\"10\" width=\"" + Percent(n.IpCount, total) * 100 + "\" />" + Convert.ToInt32(Percent(n.IpCount, total) * 100) + "%");

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
    }

    /// <summary>
    /// Browser统计
    /// </summary>
    private void BrowserPage()
    {
        //计算总量
        int total = new BCW.BLL.Statinfo().GetCount("");

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("累计流量" + total + "");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("IP|访问量|比例");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string[] pageValUrl = { "ptype" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Statinfo> listStatinfo = new BCW.BLL.Statinfo().GetBrowsers(pageIndex, pageSize, out recordCount);
        if (listStatinfo.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Statinfo n in listStatinfo)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div>", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }

                if (!Utils.Isie())
                    builder.Append(n.Browser + "—" + n.BrowserCount + "—" + (Convert.ToInt32(Percent(n.BrowserCount, total) * 100)) + "%");
                else
                    builder.Append(n.Browser + "—" + n.BrowserCount + "—<img src=\"/Files/sys/bar.gif\" height=\"10\" width=\"" + Percent(n.BrowserCount, total) * 100 + "\" />" + Convert.ToInt32(Percent(n.BrowserCount, total) * 100) + "%");

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
    }
    /// <summary>
    /// System统计
    /// </summary>
    private void SystemPage()
    {
        //计算总量
        int total = new BCW.BLL.Statinfo().GetCount("");

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("累计流量" + total + "");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("IP|访问量|比例");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string[] pageValUrl = { "ptype" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Statinfo> listStatinfo = new BCW.BLL.Statinfo().GetSystems(pageIndex, pageSize, out recordCount);
        if (listStatinfo.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Statinfo n in listStatinfo)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div>", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }

                if (!Utils.Isie())
                    builder.Append(n.System + "—" + n.SystemCount + "—" + (Convert.ToInt32(Percent(n.SystemCount, total) * 100)) + "%");
                else
                    builder.Append(n.System + "—" + n.SystemCount + "—<img src=\"/Files/sys/bar.gif\" height=\"10\" width=\"" + Percent(n.SystemCount, total) * 100 + "\" />" + Convert.ToInt32(Percent(n.SystemCount, total) * 100) + "%");

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
    }

    /// <summary>
    /// PUrl统计
    /// </summary>
    private void PUrlPage()
    {
        //计算总量
        int total = new BCW.BLL.Statinfo().GetCount("");

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("累计流量" + total + "");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("IP|访问量|比例");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string[] pageValUrl = { "ptype" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Statinfo> listStatinfo = new BCW.BLL.Statinfo().GetPUrls(pageIndex, pageSize, out recordCount);
        if (listStatinfo.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Statinfo n in listStatinfo)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div>", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }

                if (!Utils.Isie())
                    builder.Append(Utils.Mid(n.PUrl, 1, n.PUrl.Length) + "—" + n.PUrlCount + "—" + (Convert.ToInt32(Percent(n.PUrlCount, total) * 100)) + "%");
                else
                    builder.Append(Utils.Mid(n.PUrl, 1, n.PUrl.Length) + "—" + n.PUrlCount + "—<img src=\"/Files/sys/bar.gif\" height=\"10\" width=\"" + Percent(n.PUrlCount, total) * 100 + "\" />" + Convert.ToInt32(Percent(n.PUrlCount, total) * 100) + "%");

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
    }

    private void HelpPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("什么叫IP/UV/PV");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("IP（Internet Protocol）：指独立IP数。00:00-24:00内相同IP地址只被计算一次<br />");
        builder.Append("UV（UniqueVisitor）：独立访客，将每台独立上网电脑（以cookie为依据）视为一位访客，一天之内（00:00-24:00），访问您网站的访客数量。一天之内相同cookie的访问只被计算1次。<br />");
        builder.Append("PV（PageView）：访问量,即页面浏览量或者点击量,用户每次对网站的访问均被记录1次。用户对同一页面的多次访问，访问量值累计。");
        
        builder.Append(Out.Tab("</div>", ""));
    }

    /// <summary>
    /// 清空统计记录
    /// </summary>
    private void ClearPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("请选择清空选项");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("stat.aspx?act=clearok&amp;ptype=1") + "\">清空本日前</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("stat.aspx?act=clearok&amp;ptype=2") + "\">清空本周前</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("stat.aspx?act=clearok&amp;ptype=3") + "\">清空本月前</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("stat.aspx?act=clearok&amp;ptype=4") + "\">清空全部</a>");
        builder.Append(Out.Tab("</div>", ""));
    }


    /// <summary>
    /// 清空统计记录
    /// </summary>
    private void ClearOkPage()
    {
        string act = Utils.GetRequest("act", "get", 1, "", "");
        string info = Utils.GetRequest("info", "get", 1, "", "");
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-4]$", "0"));
        if (info != "ok" && info != "acok")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定要清空吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("stat.aspx?info=ok&amp;act=" + act + "&amp;ptype=" + ptype + "") + "\">确定清空</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("stat.aspx?act=clear") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));

        }
        else
        {
            if (info == "ok")
            {
                Utils.Success("正在清空", "正在清空,请稍后..", Utils.getPage("stat.aspx?info=acok&amp;act=" + act + "&amp;ptype=" + ptype + ""), "2");
            }
            else
            {
                if (ptype == 1)
                {
                    //保留本日计算
                    M_Str_mindate = DateTime.Now.ToShortDateString() + " 0:00:00";
                    new BCW.BLL.Statinfo().Delete("AddTime<'" + M_Str_mindate + "'");
                }
                else if (ptype == 2)
                {
                    //保留本周计算
                    switch (DateTime.Now.DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            M_Str_mindate = DateTime.Now.AddDays(0).ToShortDateString() + " 0:00:00";
                            break;
                        case DayOfWeek.Tuesday:
                            M_Str_mindate = DateTime.Now.AddDays(-1).ToShortDateString() + " 0:00:00";
                            break;
                        case DayOfWeek.Wednesday:
                            M_Str_mindate = DateTime.Now.AddDays(-2).ToShortDateString() + " 0:00:00";
                            break;
                        case DayOfWeek.Thursday:
                            M_Str_mindate = DateTime.Now.AddDays(-3).ToShortDateString() + " 0:00:00";
                            break;
                        case DayOfWeek.Friday:
                            M_Str_mindate = DateTime.Now.AddDays(-4).ToShortDateString() + " 0:00:00";
                            break;
                        case DayOfWeek.Saturday:
                            M_Str_mindate = DateTime.Now.AddDays(-5).ToShortDateString() + " 0:00:00";
                            break;
                        case DayOfWeek.Sunday:
                            M_Str_mindate = DateTime.Now.AddDays(-6).ToShortDateString() + " 0:00:00";
                            break;
                    }
                    new BCW.BLL.Statinfo().Delete("AddTime<'" + M_Str_mindate + "'");
                }
                else if (ptype == 3)
                {
                    //保留本月计算
                    string MonthText = DateTime.Now.Year + "-" + DateTime.Now.Month;
                    M_Str_mindate = MonthText + "-1 0:00:00";
                    new BCW.BLL.Statinfo().Delete("AddTime<'" + M_Str_mindate + "'");
                }
                else if (ptype == 4)
                {
                    //new BCW.BLL.Statinfo().Delete("");
                    new BCW.Data.SqlUp().ClearTable("tb_Statinfo");
                }
                Utils.Success("清空成功", "清空操作成功..", Utils.getPage("stat.aspx?act=clear"), "2");
            }
        }
    }

    /// <summary>
    /// 今日/本月24小时流量分析图
    /// </summary>
    private void ImagePage()
    {
        statFulx();
    }

    protected void statFulx()
    {
        //编写SQL语句，查询日网站的访问量的总人数（PV）
        M_Str_mindate = DateTime.Now.ToShortDateString() + " 0:00:00";
        M_Str_maxdate = DateTime.Now.AddDays(1).ToShortDateString() + " 0:00:00";
        int sumFlux = new BCW.BLL.Statinfo().GetCount("AddTime>='" + M_Str_mindate + "'and AddTime<'" + M_Str_maxdate + "'");

        //调用自定义方法绘制柱子形图，表示日时段的流量
        CreateImage(sumFlux, DateTime.Now.Year + "年" + DateTime.Now.Month + "月" + DateTime.Now.Day + "日网站24小时流量分析图");
    }

    /// <summary>
    /// 自定义方法绘制柱形图，表示日或月时段的流量
    /// </summary>
    /// <param name="sumFlux">int类型的参数，表示日或月的总访问人数</param>
    /// <param name="title">标题文字</param>
    private void CreateImage(int sumFlux, string title)
    {
        //随机生成24个颜色
        ArrayList colors = new ArrayList();
        Random rnd = new Random();
        for (int len = 0; len < 24; len++)
        {
            colors.Add(new SolidBrush(Color.FromArgb(rnd.Next(255), rnd.Next(255), rnd.Next(255))));
        }

        //设置宽和高
        int height = 400, width = 830;
        //初始化并指定图像的大小
        Bitmap image = new System.Drawing.Bitmap(width, height);
        //创建Graphics类对象
        Graphics g = Graphics.FromImage(image);

        try
        {
            //清空图片背景色
            g.Clear(Color.White);
            //设置字体
            Font font = new System.Drawing.Font("Arial", 9, FontStyle.Regular);
            Font font1 = new System.Drawing.Font("宋体", 20, FontStyle.Regular);
            //
            System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.Blue, 1.2f, true);
            //绘制一个矩形
            g.FillRectangle(Brushes.WhiteSmoke, 0, 0, width, height);
            //设置标题文字的颜色
            Brush brush1 = new SolidBrush(Color.Blue);
            //绘制标题文字
            g.DrawString(title, font1, brush1, new PointF(200, 30));
            //画图片的边框线
            g.DrawRectangle(new Pen(Color.Blue), 0, 0, image.Width - 1, image.Height - 1);

            Pen mypen = new Pen(brush, 1);

            //绘制10个横向线条
            int x = 90;
            for (int i = 0; i < 24; i++)
            {
                g.DrawLine(mypen, x, 80, x, 370);
                x = x + 30;
            }
            Pen mypen1 = new Pen(Color.Blue, 2);
            g.DrawLine(mypen1, x - 750, 80, x - 750, 370);

            //绘制10个纵向线条
            int y = 80;
            for (int i = 0; i < 10; i++)
            {
                g.DrawLine(mypen, 60, y, 780, y);
                y = y + 29;
            }
            g.DrawLine(mypen1, 60, y, 780, y);

            //设置x轴的小时刻度
            string[] n ={"0","1","2","3","4","5","6","7","8","9",
            "10","11","12","13","14","15","16","17","18","19",
            "20","21","22","23"};
            x = 84;
            for (int i = 0; i < 24; i++)
            {
                //将小时刻度绘制到指定的位置上
                g.DrawString(n[i].ToString(), font, Brushes.Red, x, 374); //设置文字内容及输出位置
                x = x + 30;
            }

            //设置y轴的百分比
            String[] m = {"100%", " 90%", " 80%", " 70%", " 60%", " 50%", " 40%", " 30%",
                     " 20%", " 10%", "  0%"};
            y = 72;
            for (int i = 0; i < 11; i++)
            {
                //将百分比绘制到指定的位置上
                g.DrawString(m[i].ToString(), font, Brushes.Red, 25, y); //设置文字内容及输出位置
                y = y + 29;
            }
            //创建一个数组用来存放，没小时访问人数的百分比
            int[] Count = new int[24];
            int j = 0;
            int number = sumFlux;
            if(number == 0)   //防除数为0
               number = 1;
            for (j = 0; j < 23; j++)
            {
                string st = HourCount(j).ToString();
                Count[j + 1] = HourCount(j) * 100 / number;
            }
            //将数组第一个位，存放23点到0点的访问人数百分比
            Count[0] = HourCount(j) * 100 / number;


            //显示柱状效果
            x = 70;
            //设置表示23点到0点访问人数百分比柱形的颜色
            SolidBrush mbrush = (SolidBrush)colors[23];
            //绘制23点到0点的访问人数提示文字
            g.DrawString(HourCount(23).ToString(), font, Brushes.Black, x - 3, (370 - Count[0] * 29 / 10) - 20);
            //绘制表示23点到0点访问人数百分比柱形
            g.FillRectangle(mbrush, x, 370 - Count[0] * 29 / 10, 10, Count[0] * 29 / 10);
            for (int i = 0; i < 23; i++)
            {
                //设置两个柱形之间的颜色
                x = x + 30;
                //设置柱形的颜色
                SolidBrush mybrush = (SolidBrush)colors[i];
                //绘制柱形
                g.FillRectangle(mybrush, x, 370 - Count[i + 1] * 29 / 10, 10, Count[i + 1] * 29 / 10);
                //绘制访问人数提示文字
                g.DrawString(HourCount(i).ToString(), font, Brushes.Black, x - 3, (370 - Count[i + 1] * 29 / 10) - 20);
            }
            //将图片保存到指定路径下        
            image.Save(Server.MapPath("/Files/temp/") + "\\stat.jpeg");
            //使用Image控件显示图片
            //Image1.ImageUrl = "Images\\Z.jpeg?id=" + rnd.Next(9999);
            new BCW.Graph.ImageHelper().ResponseImage(Server.MapPath("/Files/temp/stat.jpeg"));
            //builder.Append("<img src=\"/Files/temp/stat.jpeg?rd=" + rnd.Next(9999) + "\" alt=\"load\"/>");

        }
        finally
        {
            g.Dispose();
            image.Dispose();
        }
    }
}