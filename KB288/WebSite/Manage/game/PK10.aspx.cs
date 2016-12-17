using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BCW.Common;
using BCW.PK10;
using BCW.PK10.Model;
using System.Data;
public partial class Manage_game_PK10 : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string myFileName = "PK10.aspx";
    protected string xmlPath = "/Controls/PK10.xml";
    protected PK10 _logic;
    protected void Page_Load(object sender, EventArgs e)
    {
        _logic = new PK10();
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "back":
                BackPage();
                break;
            case "backok1":
            case "backok2":
                BackOKPage();
                break;
            case "top":
                TopPage();
                break;
            case "backsave":
            case "backsave2":
                BackSavePage(act);
                break;
            case "buydetail":
                ShowBuyDetail();
                break;
            case "calc":
                CalcPage();
                break;
            case "calcall":
                CalcAllPage();
                break;
            case "view":
                ViewPage();
                break;
            case "edit":
                EditPage();
                break;
            case "editsave":
                EditSavePage();
                break;
            case "del":
                DelPage();
                break;
            case "record":
                ViewBuysPage();
                break;
            case "reset":
                ResetPage();
                break;
            case "winreport":
                WinReportPage();
                break;
            case "userAction":
                UserActionPage();
                break;
            case "userGoldLog":
                UserGoldLogPage();
                break;
            case "showlist":
                ShowListPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }
    private void ReloadPage()
    {
        Master.Title = "PK拾.管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理</a>&gt;");
        builder.Append("PK拾");
        builder.Append(Out.Tab("</div>", "<br />"));
        #region 显示数据记录
        DateTime today = DateTime.Parse(DateTime.Now.ToShortDateString());
        DateTime showdate = today; //要显示的数据日期
        int pageindex = 1;//默认显示第一页
        int pagesize =  Convert.ToInt32(ub.Get("SiteListNo")); ;//每页显示的行数
        string cshowday = Utils.GetRequest("showdate", "get", 1, "", today.ToShortDateString().Trim());
        string cpageindex = Utils.GetRequest("pageindex", "get", 1, "", "1");
        DateTime.TryParse(cshowday, out showdate);
        int.TryParse(cpageindex, out pageindex);
        #region 显示日期导航
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href =\"" + Utils.getUrl(myFileName + "?showdate=" + showdate.AddDays(1).ToShortDateString().Trim()) + "\">" + showdate.AddDays(1).ToShortDateString().Trim() + " |" + "</a>");
        builder.Append(showdate.ToShortDateString().Trim() + " |");
        builder.Append("<a href =\"" + Utils.getUrl(myFileName + "?showdate=" + showdate.AddDays(-1).ToShortDateString().Trim()) + "\">" + showdate.AddDays(-1).ToShortDateString().Trim() + "</a>");
        builder.Append(Out.Tab("</div>", Out.Hr()));
        #endregion
        #region 读取指定日期的数据页并显示
        int recordCount = 0;
        List<PK10_List> lists = _logic.GetListDatasByDate(showdate, showdate, pagesize, pageindex, out recordCount);
        string[] pageValUrl = { "act", "showdate", "backurl" };
        if (lists != null && lists.Count > 0)
        {
            int k = 1;
            foreach (PK10_List item in lists)
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
                //
                builder.Append("<a href=\"" + Utils.getUrl(myFileName+"?act=edit&amp;id=" + item.ID.ToString().Trim() + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[管理]&gt;</a>");
                string cbuy = (item.PayCount > 0) ? "(" + item.PayCount.ToString() + "下注)" : "";
                if (item.OpenFlag == 0)
                    builder.Append("第" + item.No.Trim() + "期开出:<a href=\"" + Utils.getUrl(myFileName + "?act=view&amp;id=" + item.ID.ToString().Trim()+ "&amp;backurl=" + Utils.PostPage(1) + "") + "\">未开" + cbuy + "</a>");
                else
                {
                    builder.Append("第" + item.No.Trim() + "期开出:<a href=\"" + Utils.getUrl(myFileName + "?act=view&amp;id=" + item.ID.ToString().Trim()+ "&amp;backurl=" + Utils.PostPage(1) + "") + "\">");
                    string[] anums = item.Nums.Split(',');
                    string cnums = "";
                    for (int i = 0; i < anums.Length; i++)
                    {
                        cnums += anums[i].Trim() + " ";
                    }
                    builder.Append(cnums.Trim());
                    builder.Append(cbuy + "</a>");
                }
                //builder.Append("(会员投注："+item.PayMoney+")");
                builder.Append("(" + item.PayMoney + ")");//会员投注
                if(item.OpenFlag==1 && item.CalcFlag==0)
                    builder.Append(" "+"<a href =\"" + Utils.getUrl(myFileName + "?act=calc&amp;id=" + item.ID.ToString().Trim() + "") + "\">" + "派奖" + "</a>");
                k++;
                builder.Append(Out.Tab("</div>", ""));
                //
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageindex, pagesize, recordCount, Utils.getPageUrl(), pageValUrl, "pageindex", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        #endregion
        #endregion
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("PK10.aspx?act=showlist") + "\">历史开奖</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("PK10.aspx?act=record") + "\">购买记录</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("PK10.aspx?act=calcall") + "\">全部派奖</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("PK10.aspx?act=winreport") + "\">赢利分析</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("PK10.aspx?act=top") + "\">排行榜单</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("PK10.aspx?act=back") + "\">返赢返负</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("PK10.aspx?act=reset") + "\">重置游戏</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("../xml/PK10set.aspx?backurl=" + Utils.PostPage(1) + "") + "\">游戏配置</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    private void ShowListPage()
    {
        Master.Title = "PK拾.历史开奖";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("PK拾历史开奖");
        builder.Append(Out.Tab("</div>", "<br />"));
        //
        #region 读取参数
        string cShowType = Utils.GetRequest("showtype", "all", 1, "", "0");
        int showtype = 0;
        int.TryParse(cShowType, out showtype);
        //
        string cdate1 = Utils.GetRequest("begindate", "all", 1, "", "");
        string cdate2 = Utils.GetRequest("enddate", "all", 1, "", "");
        DateTime dDate1 = DateTime.Parse(DateTime.Now.AddDays(-1).ToShortDateString());
        DateTime dDate2 = dDate1.AddDays(1);
        try
        {
            if (cdate1 != "")
                DateTime.TryParse(cdate1, out dDate1);
            if (cdate2 != "")
                DateTime.TryParse(cdate2, out dDate2);
        }
        catch { };
        //
        DateTime d1 = dDate1, d2 = dDate2;
        if(showtype==0)
        {
            d1 = DateTime.MinValue;
            d2 = DateTime.MaxValue;
        }
        #endregion
        #region 显示数据记录
        int pageindex = 1;//默认显示第一页
        int pagesize = Convert.ToInt32(ub.Get("SiteListNo")); ;//每页显示的行数
        string cpageindex = Utils.GetRequest("pageindex", "get", 1, "", "1");
        int.TryParse(cpageindex, out pageindex);
        #region 读取指定日期的数据页并显示
        int recordCount = 0;
        List<PK10_List> lists = _logic.GetOpenDatas(d1,d2, pagesize, pageindex, out recordCount);
        string[] pageValUrl = { "act", "showtype", "begindate", "enddate", "backurl" };
        if (lists != null && lists.Count > 0)
        {
            int k = 1;
            DateTime dd = DateTime.MinValue;
            foreach (PK10_List item in lists)
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
                //
                if (dd != item.Date)
                {
                    dd = item.Date;
                    builder.Append("<font color=\"red\">日期：" + item.Date.ToShortDateString() + "</font><br />");
                }
                string cbuy = (item.PayCount > 0) ? "(" + item.PayCount.ToString() + "下注)" : "";
                if (item.OpenFlag == 0)
                    builder.Append("第" + item.No.Trim() + "期开出:<a href=\"" + Utils.getUrl(myFileName + "?act=view&amp;id=" + item.ID.ToString().Trim() + "&amp;backurl="+ Utils.PostPage(1)) + "\">未开" + cbuy + "</a>");
                else
                {
                    builder.Append("第" + item.No.Trim() + "期开出:<a href=\"" + Utils.getUrl(myFileName + "?act=view&amp;id=" + item.ID.ToString().Trim() + "&amp;backurl=" + Utils.PostPage(1)) + "\">");
                    string[] anums = item.Nums.Split(',');
                    string cnums = "";
                    for (int i = 0; i < anums.Length; i++)
                    {
                        cnums += anums[i].Trim() + " ";
                    }
                    builder.Append(cnums.Trim());
                    builder.Append(cbuy + "</a>");
                }
                //builder.Append("(会员投注：" + item.PayMoney + ")");
                builder.Append("(" + item.PayMoney + ")");//会员投注
                if (item.OpenFlag == 1 && item.CalcFlag == 0)
                    builder.Append(" " + "<a href =\"" + Utils.getUrl(myFileName + "?act=calc&amp;id=" + item.ID.ToString().Trim() + "&amp;backurl=" + Utils.PostPage(1)) + "\">" + "派奖" + "</a>");
                k++;
                builder.Append(Out.Tab("</div>", ""));
                //
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageindex, pagesize, recordCount, Utils.getPageUrl(), pageValUrl, "pageindex", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        #endregion
        #endregion
        #region 显示日期查询控件
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl(myFileName + "?act=showlist&amp;showtype=0") + "\">查询全部数据</a>");
        builder.Append(Out.Tab("</div>", ""));
        string strText = "开始日期：/,截止日期：/,/,/,";
        string strName = "begindate,enddate,act,showtype";
        string strType = "text,text,hidden,hidden";
        string strValu = dDate1.ToShortDateString() + "'" + dDate2.ToShortDateString() + "'" + "showlist" + "'1";
        string strEmpt = "false,false,false，false";
        string strIdea = "/";
        string strOthe = "按日期查询," + Utils.getUrl(myFileName) + ",post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        #endregion
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage(myFileName) + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    private void BackPage()
    {
        Master.Title = "PK拾.返赢返负";
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("返赢返负操作");
        builder.Append(Out.Tab("</div>", "<br />"));
        //
        #region 参数
        string csTime = Utils.GetRequest("sTime", "get", 1, DT.RegexTime, "");
        string  coTime =Utils.GetRequest("oTime", "get", 1, DT.RegexTime, "");
        string  ciTar = Utils.GetRequest("iTar", "get", 1, @"^[0-9]\d*$", "");
        string  ciPrice = Utils.GetRequest("iPrice", "get", 1, @"^[0-9]\d*$", "");
        string cshowtype= Utils.GetRequest("showtype", "get", 1, @"^[0-9]\d*$", "");
        DateTime sTime= DateTime.Parse(DateTime.Now.AddDays(-10).ToShortDateString());
        DateTime oTime = sTime.AddDays(10);
        if (csTime != "")
            DateTime.TryParse(csTime, out sTime);
        if (coTime != "")
            DateTime.TryParse(coTime, out oTime);
        int iTar = 0, iPrice = 0, showtype = 0;
        if (ciTar != "")
            int.TryParse(ciTar, out iTar);
        if (ciPrice != "")
            int.TryParse(ciPrice, out iPrice);
        if (cshowtype != "")
            int.TryParse(cshowtype, out showtype);
        //
        #endregion
        //
        #region 显示内容
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("返赢点操作");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "开始时间:/,结束时间:/,返赢千分比:/,至少赢多少才返:/,";
        string strName = "sTime,oTime,iTar,iPrice,act";
        string strType = "date,date,num,num,hidden";
        string strValu = "" + DT.FormatDate(sTime, 0) + "'" + DT.FormatDate(oTime, 0) + "'" + iTar + "'" + iPrice + "'backok1";
        string strEmpt = "false,false,false,false,false";
        string strIdea = "/";
        string strOthe = "返赢,PK10.aspx,post,1,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("返负点操作");
        builder.Append(Out.Tab("</div>", ""));

        strText = "开始时间:/,结束时间:/,返负千分比:/,至少负多少才返:/,";
        strName = "sTime,oTime,iTar,iPrice,act";
        strType = "date,date,num,num,hidden";
        strValu = "" + DT.FormatDate(sTime, 0) + "'" + DT.FormatDate(oTime, 0) + "'" + iTar + "'" + iPrice + "'backok2";
        strEmpt = "false,false,false,false,false";
        strIdea = "/";
        strOthe = "返负,PK10.aspx,post,1,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        #endregion
        #region 底部
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage(myFileName) + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        #endregion
    }
    private void BackOKPage()
    {
        DateTime sTime = Utils.ParseTime(Utils.GetRequest("sTime", "post", 2, DT.RegexTime, "开始时间填写无效"));
        DateTime oTime = Utils.ParseTime(Utils.GetRequest("oTime", "post", 2, DT.RegexTime, "结束时间填写无效"));
        int iTar = Utils.ParseInt(Utils.GetRequest("iTar", "post", 2, @"^[0-9]\d*$", "千分比填写错误"));
        int iPrice = Utils.ParseInt(Utils.GetRequest("iPrice", "post", 2, @"^[0-9]\d*$", "至少多少币才返填写错误"));
        string act = Utils.GetRequest("act", "post", 1, "", "");
        if (act == "backok1")
        {
            //
            int backCount = 0, backMoney = 0;
            string cFlag = _logic.AddUserMoney(false, true, true, sTime, oTime, iTar, iPrice, Utils.getPageUrl(), out backCount, out backMoney);
            if (cFlag != "")
                Utils.Error("计算返赢失败：" + cFlag, "");
            else
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("本次返赢的开始时间： " + DT.FormatDate(sTime, 0) + "<br />");
                builder.Append("本次返赢的截止时间： " + DT.FormatDate(oTime, 0) + "<br />");
                builder.Append("本次返赢的千分比为： " + iTar.ToString().Trim() + "<br />");
                builder.Append("本次至少赢多少才返： " + iPrice.ToString().Trim() + "<br />");
                builder.Append("<font color=\"red\">"+"本次返赢的用户数为： " + backCount.ToString().Trim() + "</font><br />");
                builder.Append("<font color=\"red\">" + "本次返赢的总金额为： " + backMoney.ToString().Trim() + "</font><br />");
                builder.Append(Out.Tab("</div>", ""));
            }
            //
            string strText = "开始时间:,结束时间:,返赢千分比:,至少赢多少才返:,";
            string strName = "sTime,oTime,iTar,iPrice,act";
            string strType = "hidden,hidden,hidden,hidden,hidden";
            string strValu = "" + DT.FormatDate(sTime, 0) + "'" + DT.FormatDate(oTime, 0) + "'"+iTar.ToString().Trim()+"'"+iPrice.ToString().Trim()+"'backsave";
            string strEmpt = "false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定返赢,PK10.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        else
        {
            //
            int backCount = 0, backMoney = 0;
            string cFlag = _logic.AddUserMoney(false, true, false, sTime, oTime, iTar, iPrice, Utils.getPageUrl(), out backCount, out backMoney);
            if (cFlag != "")
                Utils.Error("计算返负失败：" + cFlag, "");
            else
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("本次返负的开始时间： " + DT.FormatDate(sTime, 0) + "<br />");
                builder.Append("本次返负的截止时间： " + DT.FormatDate(oTime, 0) + "<br />");
                builder.Append("本次返负的千分比为： " + iTar.ToString().Trim() + "<br />");
                builder.Append("本次至少负多少才返： " + iPrice.ToString().Trim() + "<br />");
                builder.Append("本次返负的用户数为： "+backCount.ToString().Trim()+"<br />");
                builder.Append("本次返负的总金额为： " + backMoney.ToString().Trim() + "<br />");
                builder.Append(Out.Tab("</div>", ""));
            }
            //
            string strText = "开始时间:,结束时间:,返负千分比:,至少负多少才返:,";
            string strName = "sTime,oTime,iTar,iPrice,act";
            string strType = "hidden,hidden,hidden,hidden,hidden";
            string strValu = "" + DT.FormatDate(sTime, 0) + "'" + DT.FormatDate(oTime, 0) + "'" + iTar.ToString().Trim() + "'" + iPrice.ToString().Trim() + "'backsave2";
            string strEmpt = "false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定返负,PK10.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        //
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage(myFileName+"?act=back&amp;sTime=" + DT.FormatDate(sTime, 0)+"&amp;oTime=" + DT.FormatDate(oTime, 0)+"&amp;iTar="+iTar+"&amp;iPrice="+iPrice) + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    private void BackSavePage(string act)
    {

        DateTime sTime = Utils.ParseTime(Utils.GetRequest("sTime", "post", 2, DT.RegexTime, "开始时间填写无效"));
        DateTime oTime = Utils.ParseTime(Utils.GetRequest("oTime", "post", 2, DT.RegexTime, "结束时间填写无效"));
        int iTar = Utils.ParseInt(Utils.GetRequest("iTar", "post", 2, @"^[0-9]\d*$", "千分比填写错误"));
        int iPrice = Utils.ParseInt(Utils.GetRequest("iPrice", "post", 2, @"^[0-9]\d*$", "至少多少币才返填写错误"));
        string GameStatus = ub.GetSub("GameStatus", xmlPath);
        bool isTest = false;
        if (GameStatus == "2" || GameStatus == "3")
            isTest = true;
        //
        if (act == "backsave")
        {
            int backCount = 0,backMoney = 0;
            string cFlag = _logic.AddUserMoney(true,isTest, true, sTime, oTime, iTar, iPrice, Utils.getPageUrl(),out backCount,out backMoney);
            if (cFlag != "")
                Utils.Error("返赢失败："+cFlag,"");
            else
                Utils.Success("返赢操作", "返赢操作成功，共返赢用户数为" + backCount.ToString().Trim(), Utils.getUrl(myFileName), "2");
        }
        else
        {
            int backCount = 0, backMoney = 0;
            string cFlag = _logic.AddUserMoney(true,isTest, false, sTime, oTime, iTar, iPrice, Utils.getPageUrl(), out backCount,out backMoney);
            if (cFlag != "")
                Utils.Error("返负失败：" + cFlag, "");
            else
                Utils.Success("返负操作", "返负操作成功，共返负用户数为"+backCount.ToString().Trim(), Utils.getUrl(myFileName), "2");
        }
        //
    }
    private void ViewBuysPage()
    {
        Master.Title = "PK拾.记录查询";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理</a>&gt;");
        builder.Append("PK拾");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        string cShowType = Utils.GetRequest("showtype", "all", 1, "", "0");
        int showtype = 0;
        int.TryParse(cShowType, out showtype);
        //
        #region 显示导航
        string cfont0="<font>",cfont1 = "<font>",cfont2="<font>", cfont3 = "<font>", cfont4 = "<font>"; ;
        if (showtype == 0)
            cfont0 = "<font color=\"red\">";
        if (showtype == 1)
            cfont1 = "<font color=\"red\">";
        if (showtype == 2)
            cfont2 = "<font color=\"red\">";
        if (showtype == 3)
            cfont3 = "<font color=\"red\">";
        if (showtype == 4)
            cfont4 = "<font color=\"red\">";
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href =\"" + Utils.getUrl(myFileName + "?act=record&amp;showtype=0") + "\">" +cfont0 + "全部" + "</font></a>" + " | ");
        builder.Append("<a href =\"" + Utils.getUrl(myFileName + "?act=record&amp;showtype=1") + "\">" +cfont1 + "未开" + "</font></a>" + " | ");
        builder.Append("<a href =\"" + Utils.getUrl(myFileName + "?act=record&amp;showtype=2") + "\">" +cfont2 + "未兑" + "</font></a>" + " | ");
        builder.Append("<a href =\"" + Utils.getUrl(myFileName + "?act=record&amp;showtype=3") + "\">" +cfont3 + "已兑" + "</font></a>" + " | ");
        builder.Append("<a href =\"" + Utils.getUrl(myFileName + "?act=record&amp;showtype=4") + "\">" +cfont4 + "过期" + "</font></a>");
        builder.Append(Out.Tab("</div>", Out.Hr()));
        #endregion
        ShowAllBuyPage(showtype);
        //
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage(myFileName) + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    private void ShowAllBuyPage(int showtype)
    {
        string listno = (Utils.GetRequest("listno", "all", 1, "", ""));
        int listid = 0;
        if (listno != "")
        {
            PK10_List list = _logic.GetListByNo(listno);
            if (list != null)
                listid = list.ID;
            else
                listid = -1;
        }
        int userid = Utils.ParseInt(Utils.GetRequest("userid", "all", 1, @"^[0-9]\d*$", "0"));
        //
        int pageindex = 1;//默认显示第一页
        int pagesize = 10;//每页显示的行数
        int recordCount = 0;
        string cpageindex = Utils.GetRequest("pageindex", "all", 1, "", "1");
        int.TryParse(cpageindex, out pageindex);
        List<PK10_Buy> lists = _logic.GetBuyDatas(listid,userid,showtype, pagesize, pageindex, out recordCount);
        ShowBuyPage(showtype,lists, pagesize, pageindex, recordCount);
        //
        #region 显示查询
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        string strText = "输入期号查询(空表示所有期号)：/,输入用户ID查询(0表示所有用户)：/,/,";
        string strName = "listno,userid,act,showtype";
        string strType = "text,num,hidden,hidden";
        string strValu = listno.Trim()+ "'" +userid.ToString().Trim()+ "'" + "record"+"'"+showtype.ToString().Trim();
        string strEmpt = "true,true,false,false";
        string strIdea = "/";
        string strOthe = "查询记录," + myFileName + ",post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        #endregion
        //
    }
    private void ShowBuyPage(int showtype,List<PK10_Buy> lists,int pagesize,int pageindex, int recordCount)
    {
        #region 显示下注记录页面

        string[] pageValUrl = { "act", "showtype","listno","userid","backurl" };
        if (lists != null && lists.Count > 0)
        {
            int k = 1;
            string showlistno = "";
            int showid = 0;
            foreach (PK10_Buy item in lists)
            {
                if (showlistno == item.ListNo.Trim())
                {
                    showid++;
                }
                else
                {
                    if(k==1)
                        builder.Append(Out.Tab("<div class=\"text\">", ""));
                    else
                        builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
                    builder.Append("第" + item.ListNo.Trim() + "期开出：" + item.ListNums.Trim());
                    builder.Append(Out.Tab("</div>", ""));
                    showid = 1;
                    showlistno = item.ListNo.Trim();
                }
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append(showid.ToString().Trim()+".");
                builder.Append("<a href=\"" + Utils.getUrl("../forumlog.aspx?act=xview&amp;uid=" + item.uID + "&amp;backurl=" + Utils.PostPage(1)) + "\">" + item.uName.Trim() + "</a>");
                //builder.Append("<a href=\"" + Utils.getUrl(myFileName + "?act=userAction&amp;hid=" + item.uID + "&amp;backurl=" + Utils.PostPage(1)) + "\">" + item.uName.Trim() + "</a>");
                builder.Append("买" + item.BuyDescript.Trim());
                if (item.BuyCount > 1)
                {
                    builder.Append(",每注下" + item.BuyPrice.ToString().Trim() + _logic.GetGoldName(item.isTest == 1 ? true : false));
                    builder.Append(",总下注" + item.PayMoney.ToString().Trim() + _logic.GetGoldName(item.isTest == 1 ? true : false));
                }
                else
                {
                    builder.Append(",下注" + item.PayMoney.ToString().Trim() + _logic.GetGoldName(item.isTest == 1 ? true : false));
                }
                if(item.WinMoney>0)
                    builder.Append("<font color=\"red\">(赢" + item.WinMoney.ToString().Trim() + "" + _logic.GetGoldName(item.isTest == 1 ? true : false) + ")</font>");
                if (showtype == 0) //查询全部
                {
                    #region 显示状态
                    if (item.ListNums.Trim() == "")
                        builder.Append("<font color=\"green\">(未开奖)</font>");
                    else
                    {
                        if (item.WinMoney > 0)
                        {
                            if (item.CaseFlag == 1)
                                builder.Append("已兑奖");
                            else
                            {
                                if (item.ValidFlag == 1)
                                    builder.Append("未兑奖");
                                else
                                    builder.Append("<font color=\"green\">已过期</font>");
                            }
                        }
                    }
                    #endregion
                }
                builder.Append("<a href=\"" + Utils.getPage(myFileName + "?act=buydetail&amp;id=" + item.ID+"&amp;backurl=" + Utils.PostPage(1)) + "\">" + "详细" + "</a>");
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageindex, pagesize, recordCount, Utils.getPageUrl(), pageValUrl, "pageindex", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        #endregion
    }
    private void ShowBuyDetail() //显示下注号码的中奖明细
    {
        //
        #region 显示导航
        int id = Utils.ParseInt(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        PK10_Buy obuy = _logic.GetBuyByID(id);
        if (obuy == null)
        {
            Utils.Error("不存在的下注记录", "");
        }
        PK10_BuyType obuyType = _logic.GetBuyTypeByID(obuy.BuyType);
        if (obuyType == null)
            Utils.Error("不存在的下注类型", "");
        Master.Title = "PK拾.下注明细";
        builder.Append(Out.Tab("<div >", ""));
        string backurl = Utils.GetRequest("backurl", "get", 1, "", "");
        //if (backurl != "")
        backurl = Utils.getUrl(backurl);
        builder.Append("<a href=\"" + backurl + "\">返回上一页</a>");
        builder.Append(Out.Tab("</div>", ""));
        #endregion
        builder.Append(Out.Tab("<div class=\"hr\" ></div> ", Out.Hr()));
        #region 显示明细
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("期号：" + obuy.ListNo + "<br />");
        builder.Append("买家：" + obuy.uName + "<br />");
        builder.Append("时间：" + obuy.BuyTime + "<br />");
        builder.Append("下注：" + obuy.BuyDescript + "<br />");
        builder.Append("总注数：" + obuy.BuyCount + "<br />");
        builder.Append("每一注单价：" + obuy.BuyPrice + "<br />");
        builder.Append("总支付金额：" + obuy.PayMoney + "<br />");
        builder.Append("开奖号码：" + obuy.ListNums + "<br />");
        string winnums = obuy.WinNums.Trim(); //号码、任选、和值，保存的是中奖号，大小单双龙虎保存的是0、1
        int wincount = 0;
        if (obuy.WinNums.Trim() != "")
        {
            wincount = winnums.Split('#').Length;
        }
        #region 取到中奖的明细
        switch (obuyType.ParentID)
        {
            case 2://大小
                int select = 0;
                int.TryParse(winnums, out select);
                if (select == 1)
                    winnums = "大";
                if (select == 0)
                    winnums = "小";
                break;
            case 3://单双
                select = 0;
                int.TryParse(winnums, out select);
                if (select == 1)
                    winnums = "双";
                if (select == 0)
                    winnums = "单";
                break;
            case 4://龙虎
                select = 0;
                int.TryParse(winnums, out select);
                if (select == 1)
                    winnums = "龙";
                if (select == 0)
                    winnums = "虎";
                break;
            case 6://任选
                switch (obuyType.Type)
                {
                    case 2://大小
                        select = 0;
                        int.TryParse(winnums, out select);
                        if (select == 1)
                            winnums = "大";
                        if (select == 0)
                            winnums = "小";
                        break;
                    case 3://单双
                        select = 0;
                        int.TryParse(winnums, out select);
                        if (select == 1)
                            winnums = "双";
                        if (select == 0)
                            winnums = "单";
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
        #endregion
        #region 显示中奖的明细
        builder.Append("中奖号码：");
        if (winnums != "")
        {
            string[] awinnums = winnums.Split('#');
            for (int i = 0; i < wincount; i++)
            {
                string[] anums = awinnums[i].Split(',');
                if (anums.Length > 0)
                {
                    builder.Append(anums[0]); //前二前三前四，复式时候，WinNums会记录所有中奖的组合，格式为“中奖号码，匹配奖号个数，赔率”
                    if (anums.Length > 1)
                        builder.Append("中出个数：" + anums[1]);
                    if (anums.Length > 2)
                        builder.Append("赔率：" + anums[2]);
                }
                builder.Append("<br />");
            }
        }
        else
        {
            builder.Append("<br />");
        }
        #endregion
        builder.Append("中奖注数：" + wincount + "<br />");
        if(wincount<=1)
            builder.Append("中奖赔率：" + obuy.WinPrice + "<br />");
        else
            builder.Append("平均赔率：" + obuy.WinPrice + "<br />");
        builder.Append("中奖金额：" + obuy.WinMoney + "<br />");
        builder.Append("手续费：" + obuy.Charges + "<br />");
        builder.Append("兑奖金额：" + obuy.CaseMoney + "<br />");
        builder.Append("有效期至：" + obuy.ValidDate + "<br />");
        string status = "";
        #region 状态
        if (obuy.ListNums.Trim() == "")
            status = "未开奖";
        else
        {
            if (obuy.WinMoney > 0)
            {
                if (obuy.CaseFlag == 0)
                {
                    if (obuy.ValidFlag == 0)
                        status = "已过期";
                    else
                        status = "未兑奖";
                }
                else
                {
                    status = "已兑奖";
                }
            }
        }
        #endregion
        builder.Append("状态：" + status + "<br />");
        builder.Append(Out.Tab("</div", "<br />"));
        #endregion
        //

    }
    private void ViewPage()//显示某一期的开奖情况
    {
        #region 显示某一期的开奖情况
        int id = Utils.ParseInt(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        PK10_List model = _logic.GetListByID(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "第" + model.No.Trim() + "期" ;
        string backurl = Utils.GetRequest("backurl", "all", 1, "", "");
        //
        builder.Append(Out.Tab("<div class=text>", ""));
        builder.Append("第" + model.No.Trim() + "期开奖:" + "<br/>");
        builder.Append(model.Nums.Trim()+ "<br/>");
        builder.Append("(共有" + model.PayCount.ToString().Trim() + "人次参与)" + " <br /> ");
        builder.Append("共下注：" + model.PayMoney.ToString().Trim()  + " <br /> ");
        builder.Append("共赢取：" + model.WinMoney.ToString().Trim() + " <br />");
        builder.Append("封盘时间：" + model.EndTime.ToString().Trim());
        builder.Append(Out.Tab("</div>", Out.Hr()));
        //
        #region 显示导航
        builder.Append(Out.Tab("<div>", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-2]\d*$", "1"));
        builder.Append("查看:");
        if (ptype == 1)
            builder.Append("<font color=\"red\">下注</font>"+"|");
        else
            builder.Append("<a href=\"" + Utils.getUrl(myFileName +"?act=view&amp;ptype=1&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">下注</a>|");

        if (ptype == 2)
            builder.Append("<font color=\"red\">中奖</font>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl(myFileName +"?act=view&amp;ptype=2&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">中奖</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        #endregion
        #region 显示所有下注/中奖情况
        bool showwin = true;
        string ctitle = "中奖";
        if (ptype == 1)
        {
            showwin = false;
            ctitle = "下注";
        }
        int pageindex = 1;//默认显示第一页
        int pagesize = Convert.ToInt32(ub.Get("SiteListNo"));//每页显示的行数
        string cpageindex = Utils.GetRequest("pageindex", "get", 1, "", "1");
        int.TryParse(cpageindex, out pageindex);
        int recordCount = 0;
        List<PK10_Buy> lists = _logic.GetWinDatas(id,showwin,pagesize, pageindex, out recordCount);
        string[] pageValUrl = { "act", "ptype", "id" ,"backurl"};
        if (lists != null && lists.Count > 0)
        {
            builder.Append(Out.Div("","共" + recordCount + "注"+ctitle+ "<br />"));
            int k = 1;
            foreach (PK10_Buy item in lists)
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
                builder.Append(k.ToString().Trim() + ".");
                builder.Append("<a href=\"" + Utils.getUrl("../forumlog.aspx?act=xview&amp;uid=" + item.uID + "&amp;backurl=" + Utils.getPage(0)) + "\">" + item.uName.Trim() + "</a>");
                builder.Append(",买" + item.BuyDescript.Trim());
                if (item.BuyCount > 1)
                {
                    builder.Append(",每注下" + item.BuyPrice.ToString().Trim() + _logic.GetGoldName(item.isTest == 1 ? true : false));
                    builder.Append(",总下注" + item.PayMoney.ToString().Trim() + _logic.GetGoldName(item.isTest == 1 ? true : false));
                }
                else
                {
                    builder.Append(",下注" + item.PayMoney.ToString().Trim() + _logic.GetGoldName(item.isTest == 1 ? true : false));
                }
                builder.Append("<font color=\"blue\">");
                builder.Append(" ID"+item.ID.ToString().Trim());
                builder.Append("</font>");
                builder.Append("[" + item.BuyTime.ToShortDateString().Substring(5) + " " + item.BuyTime.ToShortTimeString() + "]");

                if (item.ListNums.Trim() == "")
                    builder.Append("<font color=\"green\">(未开奖)</font>");
                else
                {
                    if (item.WinMoney > 0)
                    {
                        builder.Append("<font color=\"red\">(赢" + item.WinMoney.ToString().Trim() + "" + _logic.GetGoldName(item.isTest == 1 ? true : false) + ")</font>");
                    }
                }
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageindex, pagesize, recordCount, Utils.getPageUrl(), pageValUrl, "pageindex", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有"+ctitle+"记录.."));
        }
        #endregion
        //
        #endregion
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl(backurl) + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    private void EditPage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        #region 权限判断
        //if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th"))
        //{
        //    if (ManageId != 1 && ManageId != 2)
        //    {
        //        Utils.Error("权限不足", "");
        //    }
        //}
        //else if (Utils.GetTopDomain().Contains("kb288.net"))
        //{
        //    if (ManageId != 1 && ManageId != 3 && ManageId != 4 && ManageId != 5)
        //    {
        //        Utils.Error("权限不足", "");
        //    }
        //}
        //else
        //{
        //    if (ManageId != 1 && ManageId != 9)
        //    {
        //        Utils.Error("权限不足", "");
        //    }
        //}
        #endregion
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        string cnums = Utils.GetRequest("Nums", "all", 1, "", "");
        Master.Title = "编辑PK拾";
        PK10_List model = _logic.GetListByID(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (cnums == "")
            cnums = model.Nums.Trim();
        string cCalc = model.CalcFlag == 1 ? "(已派奖)" : "";
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("编辑第" + model.No.Trim() + "期PK拾<font color=\"green\">"+cCalc+"</font><br/>");
        builder.Append("<font color=\"red\">"+"注：已经派奖的期号，不能修改；同时为避免更改过程中被机器人自动派奖，先停用机器人的自动派奖功能！"+"</font>");
        builder.Append(Out.Tab("</div>", ""));
        if (model.CalcFlag == 0)
        {
            string strText = "开奖号码(开奖号码为10位，用，分隔，例如01，02，03，04，05，06，07，08，09，10）:/,开盘时间:/,开奖时间:/,,,";
            string strName = "Nums,BeginTime,EndTime,id,act,backurl";
            string strType = "text,date,date,hidden,hidden,hidden";
            string strValu = "" + cnums + "'" + DT.FormatDate(model.BeginTime, 0) + "'" + DT.FormatDate(model.EndTime, 0)+ "'" + id + "'editsave'" + Utils.getPage(0) + "";
            string strEmpt = "false,false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定编辑|reset,PK10.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl(myFileName + "?act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">删除本期</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage(myFileName) + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void EditSavePage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        #region 权限判断
        //if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th"))
        //{
        //    if (ManageId != 1 && ManageId != 2)
        //    {
        //        Utils.Error("权限不足", "");
        //    }
        //}
        //else if (Utils.GetTopDomain().Contains("kb288.net"))
        //{
        //    if (ManageId != 1 && ManageId != 3 && ManageId != 4 && ManageId != 5)
        //    {
        //        Utils.Error("权限不足", "");
        //    }
        //}
        //else
        //{
        //    if (ManageId != 1 && ManageId != 9)
        //    {
        //        Utils.Error("权限不足", "");
        //    }
        //}
        #endregion
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
        string cnums =Utils.GetRequest("Nums", "all", 1, "", "");
        DateTime BeginTime = Utils.ParseTime(Utils.GetRequest("BeginTime", "all", 2, DT.RegexTime, "开盘时间格式填写出错,正确格式如" + DT.FormatDate(DateTime.Now, 0) + ""));
        DateTime EndTime = Utils.ParseTime(Utils.GetRequest("EndTime", "all", 2, DT.RegexTime, "开奖时间格式填写出错,正确格式如" + DT.FormatDate(DateTime.Now, 0) + ""));
        string backurl = Utils.GetRequest("backurl", "all", 1, "", "");
        PK10_List model = _logic.GetListByID(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        #region 判断开奖号码，并生成数据
        if (cnums.Trim()=="")
            Utils.Error("号码不能为空！", "");
        string[] anums = cnums.Split(',');
        if(anums.Length<10)
            Utils.Error("号码格式不正常，开奖号码为10位，用“,”分隔，例如01,02,03,04,05,06,07,08,09,10", "");
        List<string> lnums = new List<string>();
        for(int i=0;i<anums.Length;i++)
        {
            int num = 0;
            int.TryParse(anums[i].ToString(), out num);
            if (num > 0 && num <= 10)
            {
                string cnum = num.ToString().Trim();
                if (cnum.Length == 1)
                    cnum = "0" + cnum;
                else
                    cnum = cnum.Substring(0, 2);
                //
                if (!lnums.Contains(cnum))
                    lnums.Add(cnum);
            }
        }
        if(lnums.Count<10)
            Utils.Error("号码格式不正常，开奖号码为10位，用“,”分隔，例如01,02,03,04,05,06,07,08,09,10", "");
        model.Nums = "";
        for (int i = 0; i < 10; i++)
        {
            if (i == 0)
                model.Nums += lnums[i].Trim();
            else
                model.Nums += " , " + lnums[i].Trim();
        }
        #endregion
        #region 判断开奖时间，并生成数据

        #endregion
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "")
        {
            #region 显示修改确认提示页面
            Master.Title = "更改第" + model.No.Trim() + "期";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("第" + model.No.Trim() + "期的开奖号码为："+cnums);
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl(myFileName + "?info=ok&amp;act=editsave&amp;id=" + id + "&amp;Nums="+cnums+"&amp;BeginTime="+DT.FormatDate(BeginTime,0)+"&amp;EndTime="+ DT.FormatDate(EndTime,0)+"&amp;backurl=" + backurl  + "") + "\">确定更改</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl(myFileName + "?act=edit&amp;id=" + id + "&amp;Nums=" + cnums + "&amp;BeginTime=" + DT.FormatDate(BeginTime, 0) + "&amp;EndTime=" + DT.FormatDate(EndTime, 0) + "&amp;backurl=" + backurl  + "") + "\">再看看..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            #endregion
        }
        else
        {
            #region 保存更改
            string cSave = _logic.SaveOpenData(model);
            if (cSave == "")
            {
                //记录日志
                if (Utils.GetDomain().Contains("kubao") || Utils.GetDomain().Contains("tuhao") || Utils.GetDomain().Contains("th") || Utils.GetDomain().Contains("PK10"))
                {
                    String sLogFilePath = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "/logstr/" + DateTime.Now.ToString("MM-dd") + ".txt";
                    LogHelper.Write(sLogFilePath, "操作管理员:" + ManageId + "号/编辑PK拾期数:" + id + "|是否预设:" + cnums + "");
                }
                Utils.Success("编辑第" + id + "期", "编辑第" + id + "期成功..", Utils.getUrl(myFileName + "?act=edit&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
            }
            else
                Utils.Error("保存失败！" + cSave, "");
            #endregion
        }
    }

    private void DelPage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        #region 权限判断
        if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th"))
        {
            if (ManageId != 1 && ManageId != 2)
            {
                Utils.Error("权限不足", "");
            }
        }
        else if (Utils.GetTopDomain().Contains("kb288.net"))
        {
            if (ManageId != 1 && ManageId != 3 && ManageId != 4 && ManageId != 5)
            {
                Utils.Error("权限不足", "");
            }
        }
        else
        {
            if (ManageId != 1 && ManageId != 9)
            {
                Utils.Error("权限不足", "");
            }
        }
        #endregion
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        if (info == "")
        {
            #region 删除提示页面
            PK10_List model = _logic.GetListByID(id);
            if (model == null)
            {
                Utils.Error("不存在的记录", "");
            }
            Master.Title = "删除第" + model.No.Trim() + "期";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定第" + model.No.Trim() + "期记录吗.删除同时将会删除该期的下注记录.");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl(myFileName+"?info=ok&amp;act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl(myFileName+"?act=edit&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            #endregion
        }
        else
        {
            #region 删除数据
            PK10_List model = _logic.GetListByID(id);
            if (model == null)
            {
                Utils.Error("不存在的记录", "");
            }
            string cDelete = _logic.DeleteList(model);
            if (cDelete == "")
            {
                //记录日志
                if (Utils.GetDomain().Contains("kubao") || Utils.GetDomain().Contains("tuhao") || Utils.GetDomain().Contains("th") || Utils.GetDomain().Contains("PK10"))
                {
                    String sLogFilePath = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "/logstr/" + DateTime.Now.ToString("MM-dd") + ".txt";
                    LogHelper.Write(sLogFilePath, "操作管理员:" + ManageId + "号/删除PK拾期数:" + model.No.Trim() + "");
                }
                Utils.Success("删除第" + id + "期", "删除第" + model.No.Trim() + "期成功..", Utils.getPage(myFileName), "1");
            }
            else
                Utils.Error("删除失败！" + cDelete, "");
            #endregion
        }
    }

    private void ResetPage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        //
        #region 权限判断
        if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th"))
        {
            if (ManageId != 1 && ManageId != 2)
            {
                Utils.Error("权限不足", "");
            }
        }
        else if (Utils.GetTopDomain().Contains("kb288.net"))
        {
            if (ManageId != 1 && ManageId != 3 && ManageId != 4 && ManageId != 5)
            {
                Utils.Error("权限不足", "");
            }
        }
        else
        {
            if (ManageId != 1 && ManageId != 9)
            {
                Utils.Error("权限不足", "");
            }
        }
        #endregion
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "")
        {
            Master.Title = "重置游戏";
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("确定重置PK拾游戏吗？(重置后，将重新开始，以前所有记录将会期数和下注记录全被删除)");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl(myFileName+"?info=ok&amp;act=reset") + "\">确定重置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl(myFileName) + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div></div>", "<br />"));
        }
        else
        {
            string cDelete = _logic.DeleteAll();
            if(cDelete=="")
                Utils.Success("重置游戏", "重置游戏成功..", Utils.getUrl(myFileName), "1");
            else
                Utils.Error("重置失败！" + cDelete, "");
        }
    }

    private void TopPage()
    {
        Master.Title = "PK拾.排行榜";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("PK拾排行榜");
        builder.Append(Out.Tab("</div>", "<br />"));
        //
        #region 读取参数
        string cShowType = Utils.GetRequest("showtype", "all", 1, "", "0");
        int showtype = 0;
        int.TryParse(cShowType, out showtype);
        //
        string cdate1 = Utils.GetRequest("begindate", "all", 1, "", "");
        string cdate2 = Utils.GetRequest("enddate", "all", 1, "", "");
        DateTime dDate1 = DateTime.Parse(DateTime.Now.AddDays(-1).ToShortDateString());
        DateTime dDate2 = dDate1.AddDays(1);
        try
        {
            if (cdate1 != "")
                DateTime.TryParse(cdate1, out dDate1);
            if (cdate2 != "")
                DateTime.TryParse(cdate2, out dDate2);
        }
        catch { };
        //
        string listno = (Utils.GetRequest("listno", "all", 1, "", ""));
        int listid = 0;
        if (listno != "")
        {
            PK10_List list = _logic.GetListByNo(listno);
            if (list != null)
                listid = list.ID;
            else
                listid = -1;
        }
        //
        DateTime d1 = dDate1, d2 = dDate2;
        switch (showtype)
        {
            case 0: //全部
                d1 = DateTime.MinValue;
                d2 = DateTime.MaxValue;
                listid = 0;
                break;
            case 1: //按日期查询
                listid = 0;
                break;
            case 2: //按期号查询
                d1 = DateTime.MinValue;
                d2 = DateTime.MaxValue;
                break;
        }
        #endregion
        #region 显示排行数据
        int pageindex = 1;//默认显示第一页
        int pagesize = Convert.ToInt32(ub.Get("SiteListNo")); //每页显示的行数
        string cpageindex = Utils.GetRequest("pageindex", "get", 1, "", "1");
        int.TryParse(cpageindex, out pageindex);
        int recordCount = 0;
        List<PK10_Top> lists= _logic.GetWinTopDatas(d1,d2,listid,0, pagesize, pageindex, out recordCount);
        string[] pageValUrl = { "act", "showtype","begindate","enddate","listno", "backurl" };
        if (lists != null && lists.Count > 0)
        {
            int k = 1;
            foreach (PK10_Top item in lists)
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
                //builder.Append("[第" + item.No.ToString().Trim() + "名]<a href=\"" + Utils.getUrl(myFileName + "?act=userGoldLog&amp;hid=" + item.UsID.ToString().Trim() + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(item.UsID) + "</a>赢" + item.iGold.ToString().Trim() + "" );
                builder.Append("[第" + item.No.ToString().Trim() + "名]<a href=\"" + Utils.getUrl("../forumlog.aspx?act=xview&amp;uid=" + item.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(item.UsID)+"("+item.UsID+")" + "</a>赢" + item.iGold.ToString().Trim() + "");
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageindex, pagesize, recordCount, Utils.getPageUrl(), pageValUrl, "pageindex", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        #endregion
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl(myFileName + "?act=top&amp;showtype=0") + "\">查询全部数据</a>");
        builder.Append(Out.Tab("</div>", ""));
        #region 显示日期查询控件
        string strText = "开始日期：/,截止日期：/,/,/,";
        string strName = "begindate,enddate,act,showtype";
        string strType = "text,text,hidden,hidden";
        string strValu = dDate1.ToString() + "'" + dDate2.ToString() + "'" + "top" + "'1";
        string strEmpt = "false,false,false，false";
        string strIdea = "/";
        string strOthe = "按日期查询," + Utils.getUrl(myFileName) + ",post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        #endregion
        #region 显示期号查询
        strText = "输入期号（空表示所有期号）：/,/,/,";
        strName = "listno,act,showtype";
        strType = "text,hidden,hidden";
        strValu = listno.Trim() + "'" + "top" + "'2";
        strEmpt = "true,false,false";
        strIdea = "/";
        strOthe = "按期号查询," + Utils.getUrl(myFileName) + ",post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        #endregion
        //
        #region 游戏底部导航
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage(myFileName) + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        #endregion
    }
    private void CalcPage()
    {
        int id = Utils.ParseInt(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "选择派奖无效"));
        PK10_List list = _logic.GetListByID(id);
        if (list == null)
            Utils.Error("找不到派奖的期号记录", "");
        //
        List<PK10_List> lists = new List<PK10_List>();
        lists.Add(list);
        //
        string cFlag = _logic.CalcOpenData(lists);
        if (string.IsNullOrEmpty(cFlag))
        {
            Utils.Success("派奖", "恭喜，成功派奖！" , Utils.getUrl(myFileName), "1");
        }
        else
        {
            Utils.Success("派奖失败", cFlag, Utils.getUrl(myFileName), "2");
        }
    }
    private void CalcAllPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "")
        {
            Master.Title = "全部派奖";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定把全部没派奖的期数进行派奖吗？派奖后，会员可以兑奖，而且已派奖的期数不能更改！");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl(myFileName + "?info=ok&amp;act=calcall") + "\">确定全部派奖</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl(myFileName) + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            //
            string cFlag = _logic.CalcOpenData();
            if (string.IsNullOrEmpty(cFlag))
            {
                Utils.Success("派奖", "恭喜，成功派奖！", Utils.getUrl(myFileName), "1");
            }
            else
            {
                Utils.Success("派奖失败", cFlag, Utils.getUrl(myFileName), "2");
            }
        }
    }
    private void WinReportPage()
    {
        #region 顶部
        Master.Title = "PK拾.赢利分析";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理</a>&gt;");
        builder.Append("PK拾");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        #endregion
        #region 读取参数
        string cdate1 = Utils.GetRequest("begindate", "all", 1, "", "");
        string cdate2 = Utils.GetRequest("enddate", "all", 1, "", "");
        DateTime dDate1 = DateTime.Parse(DateTime.Now.AddDays(-1).ToShortDateString());
        DateTime dDate2 = dDate1.AddDays(1);
        try
        {
            if(cdate1!="")
                DateTime.TryParse(cdate1, out dDate1);
            if(cdate2!="")
                DateTime.TryParse(cdate2, out dDate2);
        }
        catch { };
        bool hasRobot = false;
        bool hasSpier = false;
        string robotflag = Utils.GetRequest("hasRobot", "all", 1, "", "");
        string spierflag = Utils.GetRequest("hasSpier", "all", 1, "", "");
        int nrobotflag = 0, nspierflag = 0;
        int.TryParse(robotflag, out nrobotflag);
        int.TryParse(spierflag, out nspierflag);
        if (nrobotflag==1)
            hasRobot = true;
        if (nspierflag == 1)
            hasSpier = true;
        #endregion
        PK10_WinReport oreport;
        #region  显示日月年的赢利情况
        DateTime today = DateTime.Parse(DateTime.Now.ToShortDateString());
        oreport = _logic.GetWinReport(today, today, hasRobot, hasSpier);
        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        builder.Append("【今日的赢利情况】");
        builder.Append(Out.Tab("</div>", "<br/>"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("总收入：" + (oreport.PayMoney + oreport.Charges).ToString().Trim() + "(销售收入" + oreport.PayMoney.ToString().PadRight(30) + "+ 手续费收入" + oreport.Charges.ToString().PadRight(30) + ")<br/>"); //
        builder.Append("总支出：" + oreport.WinMoney.ToString().PadRight(30) + "<br/>"); //
        builder.Append("总赢利：" + (oreport.PayMoney + oreport.Charges - oreport.WinMoney).ToString().PadRight(30)); //
        builder.Append(Out.Tab("</div>", ""));
        //
        oreport = _logic.GetWinReport(today.AddDays(-1), today.AddDays(-1), hasRobot, hasSpier);
        builder.Append(Out.Tab("<div class=\"text\">", "<br /> "));
        builder.Append("【上日的赢利情况】");
        builder.Append(Out.Tab("</div>", "<br/>"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("总收入：" + (oreport.PayMoney + oreport.Charges).ToString().Trim() + "(销售收入" + oreport.PayMoney.ToString().PadRight(30) + "+ 手续费收入" + oreport.Charges.ToString().PadRight(30) + ")<br/>"); //
        builder.Append("总支出：" + oreport.WinMoney.ToString().PadRight(30) + "<br/>"); //
        builder.Append("总赢利：" + (oreport.PayMoney + oreport.Charges - oreport.WinMoney).ToString().PadRight(30)); //
        builder.Append(Out.Tab("</div>", ""));
        //
        oreport = _logic.GetWinReport(GetCurrentMonthFirstDay(today), GetCurrentMonthLastDay(today), hasRobot, hasSpier);
        builder.Append(Out.Tab("<div class=\"text\">", "<br /> "));
        builder.Append("【当月的赢利情况】");
        builder.Append(Out.Tab("</div>", "<br/>"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("总收入：" + (oreport.PayMoney + oreport.Charges).ToString().Trim() + "(销售收入" + oreport.PayMoney.ToString().PadRight(30) + "+ 手续费收入" + oreport.Charges.ToString().PadRight(30) + ")<br/>"); //
        builder.Append("总支出：" + oreport.WinMoney.ToString().PadRight(30) + "<br/>"); //
        builder.Append("总赢利：" + (oreport.PayMoney + oreport.Charges - oreport.WinMoney).ToString().PadRight(30)); //
        builder.Append(Out.Tab("</div>", ""));
        //
        oreport = _logic.GetWinReport(GetPrevMonthFirstDay(today), GetPrevMonthLastDay(today), hasRobot, hasSpier);
        builder.Append(Out.Tab("<div class=\"text\">", "<br /> "));
        builder.Append("【上月的赢利情况】");
        builder.Append(Out.Tab("</div>", "<br/>"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("总收入：" + (oreport.PayMoney + oreport.Charges).ToString().Trim() + "(销售收入" + oreport.PayMoney.ToString().PadRight(30) + "+ 手续费收入" + oreport.Charges.ToString().PadRight(30) + ")<br/>"); //
        builder.Append("总支出：" + oreport.WinMoney.ToString().PadRight(30) + "<br/>"); //
        builder.Append("总赢利：" + (oreport.PayMoney + oreport.Charges - oreport.WinMoney).ToString().PadRight(30)); //
        builder.Append(Out.Tab("</div>", ""));
        //
        oreport = _logic.GetWinReport(GetCurrentYearFirstDay(today),GetCurrentYearLastDay(today), hasRobot, hasSpier);
        builder.Append(Out.Tab("<div class=\"text\">", "<br /> "));
        builder.Append("【今年的赢利情况】");
        builder.Append(Out.Tab("</div>", "<br/>"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("总收入：" + (oreport.PayMoney + oreport.Charges).ToString().Trim() + "(销售收入" + oreport.PayMoney.ToString().PadRight(30) + "+ 手续费收入" + oreport.Charges.ToString().PadRight(30) + ")<br/>"); //
        builder.Append("总支出：" + oreport.WinMoney.ToString().PadRight(30) + "<br/>"); //
        builder.Append("总赢利：" + (oreport.PayMoney + oreport.Charges - oreport.WinMoney).ToString().PadRight(30)); //
        builder.Append(Out.Tab("</div>", ""));
        #endregion
        #region 显示按日期查询出的赢利情况
        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        builder.Append("【从" + dDate1.ToShortDateString() + "到" + dDate2.ToShortDateString() + "的赢利情况】");
        builder.Append(Out.Tab("</div>", ""));
        oreport = _logic.GetWinReport(dDate1, dDate2, hasRobot, hasSpier);
        if (oreport != null)
        {
            builder.Append(Out.Tab("<div>", "<br/>"));
            builder.Append("总收入：" + (oreport.PayMoney + oreport.Charges).ToString().Trim() + "(销售收入" + oreport.PayMoney.ToString().PadRight(30) + "+ 手续费收入" + oreport.Charges.ToString().PadRight(30) + ")<br/>"); //
            builder.Append("总支出：" + oreport.WinMoney.ToString().PadRight(30) + "<br/>"); //
            builder.Append("总赢利：" + (oreport.PayMoney + oreport.Charges - oreport.WinMoney).ToString().PadRight(30)); //
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            builder.Append(Out.Tab("<div>", "<br/>"));
            builder.Append("不能读取从" + dDate1.ToShortDateString() + "到" + dDate2.ToShortDateString() + "的赢利情况！");
            builder.Append(Out.Tab("</div>", ""));
        }
        #endregion
        #region 显示日期查询控件
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        string strText = "机器人：/,系统号：/,输入开始日期：/,输入截止日期：/,/,";
        string strName = "hasRobot,hasSpier,begindate,enddate,act";
        string strType = "select,select,text,text,hidden";
        string strValu = nrobotflag.ToString()+"'"+nspierflag.ToString()+"'"+ dDate1.ToShortDateString() + "'" + dDate2.ToShortDateString() + "'" + "winreport";
        string strEmpt = "0|不包|1|包含"+","+ "0|不包|1|包含" + ","+"false,false,false";
        string strIdea = "/";
        string strOthe = "查询," +Utils.getUrl(myFileName+"?act=winreport") + ",post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        #endregion

        #region 底部
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage(myFileName) + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        #endregion
    }
    private void UserActionPage()//用户动态记录页
    {
        #region 显示导航
        int hid = Utils.ParseInt(Utils.GetRequest("hid", "get", 2, @"^[1-9]\d*$", "ID错误"));
        if (hid == 0)
        {
            Utils.Error("不存在的记录", "");
        }
        int type= Utils.ParseInt(Utils.GetRequest("type", "get", 1, @"^[1-9]\d*$", "0"));

        Master.Title = "PK拾.用户动态";
        string backurl = Utils.GetRequest("backurl", "get", 1, "", "");
        backurl = Utils.getUrl(backurl);
        string cfont0 = "<font>", cfont1 = "<font>" ;
        if (type == 0)
            cfont0 = "<font color=\"red\">";
        else
            cfont1 = "<font color=\"red\">";
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("【会员动态】" + "<br/>");
        builder.Append("<a href =\"" + Utils.getUrl(myFileName + "?act=userAction&amp;hid=" + hid + "&amp;backurl=" + backurl) + "\">" + cfont0 + "PK10" + "</font></a>" + " | ");
        builder.Append("<a href =\"" + Utils.getUrl(myFileName + "?act=userAction&amp;hid=" + hid + "&amp;backurl=" + backurl + "&amp;type=1") + "\">" + cfont1 + "全部" + "</font></a>");
        builder.Append(Out.Tab("</div>", ""));

        #endregion
        #region 显示数据
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string[] pageValUrl = { "act", "hid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        // 开始读取列表
        IList<BCW.Model.Action> listAction = null;
        string cwhere = "usid=" + hid + "";
        if (type == 0)
            cwhere += " and types=30 ";
        listAction = new BCW.BLL.Action().GetActions(pageIndex, pageSize, cwhere , out recordCount);
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
                builder.AppendFormat("{0}前<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid={1}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}</a>{3}", DT.DateDiff2(DateTime.Now, n.AddTime), n.UsId, n.UsName, Out.SysUBB(n.Notes));
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
        #endregion
        #region 游戏底部导航
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + backurl + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        #endregion
    }
    private void UserGoldLogPage() //用户消费记录
    {
        #region 显示导航
        int hid = Utils.ParseInt(Utils.GetRequest("hid", "get", 2, @"^[1-9]\d*$", "ID错误"));
        if (hid == 0)
        {
            Utils.Error("不存在的记录", "");
        }
        int type = Utils.ParseInt(Utils.GetRequest("type", "get", 1, @"^[1-9]\d*$", "0"));
        Master.Title = "PK拾.消费记录";
        string backurl = Utils.GetRequest("backurl", "get", 1, "", "");
        backurl = Utils.getUrl(backurl);
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("【会员消费记录】" + "<br/>");
        builder.Append(Out.Tab("</div>", ""));
        #endregion
        #region
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string[] pageValUrl = { "act", "hid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        // 开始读取列表
        string cwhere = "UsID=" + hid + " and Types=" + type + " and BbTag<=1";
        IList<BCW.Model.Goldlog> listGoldlog = new BCW.BLL.Goldlog().GetGoldlogs(pageIndex, pageSize, cwhere, out recordCount);
        if (listGoldlog.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Goldlog n in listGoldlog)
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
                builder.AppendFormat("{0}.{1}|操作{2}|结{3}({4})", (pageIndex - 1) * pageSize + k, Out.SysUBB(n.AcText), n.AcGold, n.AfterGold, DT.FormatDate(n.AddTime, 1));

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
        #endregion
        #region 游戏底部导航
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + backurl + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        #endregion
    }
    #region 其他函数
    private DateTime GetCurrentMonthFirstDay(DateTime date)
    {
        string cDate = date.Year.ToString().Trim() + "-" + date.Month.ToString().Trim() + "-01";
        return DateTime.Parse(cDate);
    }
    private DateTime GetCurrentMonthLastDay(DateTime date)
    {
        string cDate = "";
        if (date.Month == 12)
            cDate = (date.Year + 1).ToString().Trim() + "-01-01";
        else
            cDate = date.Year.ToString().Trim() + "-" + (date.Month + 1).ToString().Trim() + "-01";
        return DateTime.Parse(cDate).AddDays(-1);
    }
    private DateTime GetPrevMonthFirstDay(DateTime date)
    {
        string cDate = "";
        if (date.Month == 1)
            cDate = (date.Year - 1).ToString().Trim() + "-12-01";
        else
            cDate = date.Year.ToString().Trim() +"-"+(date.Month-1).ToString().Trim() +"-01";
        return DateTime.Parse(cDate);
    }
    private DateTime GetPrevMonthLastDay(DateTime date)
    {
        return GetCurrentMonthFirstDay(date).AddDays(-1);
    }
    private DateTime GetCurrentYearFirstDay(DateTime date)
    {
        string cDate = date.Year.ToString().Trim() + "-1-01";
        return DateTime.Parse(cDate);
    }
    private DateTime GetCurrentYearLastDay(DateTime date)
    {
        string cDate = (date.Year + 1).ToString().Trim() + "-01-01";
        return DateTime.Parse(cDate).AddDays(-1);
    }
    #endregion
}