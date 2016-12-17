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

public partial class Manage_game_DawnlifeManage : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/Dawnlife.xml";
    protected string GameName = ub.GetSub("DawnlifeName", "/Controls/Dawnlife.xml");

    protected double jcd = Convert.ToInt32(ub.GetSub("jcd", "/Controls/Dawnlife.xml"));
    protected double jcx = Convert.ToInt32(ub.GetSub("jcx", "/Controls/Dawnlife.xml"));
    protected double drd = Convert.ToInt32(ub.GetSub("drd", "/Controls/Dawnlife.xml"));
    protected double drq = Convert.ToInt32(ub.GetSub("drq", "/Controls/Dawnlife.xml"));
    protected double q1 = Convert.ToInt32(ub.GetSub("q1", "/Controls/Dawnlife.xml"));
    protected double q2 = Convert.ToInt32(ub.GetSub("q2", "/Controls/Dawnlife.xml"));
    protected double q3 = Convert.ToInt32(ub.GetSub("q3", "/Controls/Dawnlife.xml"));
    protected double q4 = Convert.ToInt32(ub.GetSub("q4", "/Controls/Dawnlife.xml"));
    protected double q5 = Convert.ToInt32(ub.GetSub("q5", "/Controls/Dawnlife.xml"));
    protected string dq = Convert.ToString(ub.GetSub("dq", "/Controls/Dawnlife.xml"));
    protected double juankuan = Convert.ToInt32(ub.GetSub("juankuan", "/Controls/Dawnlife.xml"));

    protected int r;
    protected string strText = string.Empty;
    protected string strName = string.Empty;
    protected string strType = string.Empty;
    protected string strValu = string.Empty;
    protected string strEmpt = string.Empty;
    protected string strIdea = string.Empty;
    protected string strOthe = string.Empty;
    ub xml = new ub(); 
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "award":
                AwardPage();//游戏派奖管理
                break;
            case "hj":
                HjPage();//日期查询获奖情况
                break;
            case "awardr":
                AwardrPage();//游戏派奖管理
                break;
            case "caseok":
                CaseOkPage();
                break;
            case "caseok1"://
                CaseOk1Page();
                break;
            case "jilu":
                JiluPage();//奖池详细记录
                break;
            case "top":
                TopPage();//排行榜管理
                break;
            case "cdjl":
                CdjlPage();//闯荡记录
                break;
            case "cdjilu":
                cdjiluPage();
                break;
            case "paijiang":
                PaijiangPage();
                break;
            case "paijiang1":
                PaijiangPage1();
                break;
            case "rpaijiang":
                RPaijiangPage();
                break;
            case "rpaijiang1":
                RPaijiangPage1();
                break;
            case "top1":
                Top1Page();//排行榜按日期管理
                break;
            case "gift":
                GiftPage();//奖池管理
                break;
            case "juankuan":
                JuankuanPage();//捐款排行榜
                break;
            case "xitongpaijiang":
                XTPJPage();//系统派奖详细
                break;
            case "give":
                GivePage();
                break;
            case "weihu":
                WeihuPage();//游戏维护
                break;
            case "reset":
                ResetPage();//游戏重置
                break;
            case "peizhi":
               PeizhiPage();//游戏配置
                break;
            case "neice":
                SetStatueCeshi();
                break;
            case "profit":
                ProfitPage();//盈利分析
                break;
            default:
                ReloadPage();
                break;
        }
    }
    //首页
    protected void ReloadPage()
    {
        Master.Title = ""+GameName+"管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">管理中心</a>&gt;" + GameName + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=top") + "\">排行榜管理</a>" + "<br />");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=cdjl") + "\">闯荡记录</a>" + "<br />");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=gift") + "\">奖池管理</a>" + "<br />");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=award") + "\">派奖管理</a>" + "<br />");
        builder.Append(Out.Tab("</div>", ""));
        //builder.Append(Out.Tab("<div>", ""));
        //builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=awardr") + "\">自动派奖</a>" + "<br />");
        //builder.Append(Out.Tab("</div>", ""));
        //builder.Append(Out.Tab("<div>", ""));
        //builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=profit") + "\">盈利分析</a>" + "<br />");
        //builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=weihu") + "\">游戏配置</a>" + "<br />");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=peizhi") + "\">奖励配置</a>" + "<br />");
        builder.Append(Out.Tab("</div>", "")); 
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=neice") + "\">内测ID配置</a>" + "<br />");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=reset") + "\">游戏重置</a>" + "<br />");
        builder.Append(Out.Tab("</div>", ""));
      
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    //排行榜管理
    protected void TopPage()
    {
        Master.Title = "管理中心";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx") + "\">" + GameName + "管理</a>&gt;排行榜管理");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("特别提醒：" + "<br />" + "排行榜管理可以查看排行榜记录，也可以返奖，派奖操作请到派奖管理进行操作，此处的奖励为管理员给予酷友的额外奖励，请慎重操作");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("" + "-----------" + "<br />");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=top1") + "\">按日期查看排行</a>" + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("" + "-----------" + "<br />");
        builder.Append(Out.Tab("</div>", ""));
        int ptypec = Utils.ParseInt(Utils.GetRequest("ptypec", "get", 1, @"^[0-4]$", "4"));
        int ptyped = Utils.ParseInt(Utils.GetRequest("ptyped", "get", 1, @"^[0-4]$", "4"));

        int pageIndex = 0;//当前页
        int recordCount;//记录总条数
        int pageSize = 10;//分页大小
        string strWhere = "";
        string strOrder = "";
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("【富人排行榜】");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        if (ptyped == 4)
            builder.Append("<b style=\"color:black\">总" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=top&amp;ptyped=4&amp;id=" + ptyped + "&amp;ptypec=" + ptypec + "&amp;backurl=" + Utils.getPage(0) + "") + "\">总</a>" + "|");
        if (ptyped == 1)
            builder.Append("<b style=\"color:black\">日" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=top&amp;ptyped=1&amp;id=" + ptyped + "&amp;ptypec=" + ptypec + "&amp;backurl=" + Utils.getPage(0) + "") + "\">日</a>" + "|");
        if (ptyped == 2)
            builder.Append("<b style=\"color:black\">周" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=top&amp;ptyped=2&amp;id=" + ptyped + "&amp;ptypec=" + ptypec + "&amp;backurl=" + Utils.getPage(0) + "") + "\">周</a>" + "|");
        if (ptyped == 3)
            builder.Append("<b style=\"color:black\">月" + "</b>" + "");
        else
            builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=top&amp;ptyped=3&amp;id=" + ptyped + "&amp;ptypec=" + ptypec + "&amp;backurl=" + Utils.getPage(0) + "") + "\">月</a>" + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        if (ptypec == 4)
            builder.Append("<b style=\"color:black\">全国" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=top&amp;ptypec=4&amp;id1=" + ptypec + "&amp;ptyped=" + ptyped + "&amp;backurl=" + Utils.getPage(0) + "") + "\">全国</a>" + "|");
        if (ptypec == 1)
            builder.Append("<b style=\"color:black\">广州" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=top&amp;ptypec=1&amp;id1=" + ptypec + "&amp;ptyped=" + ptyped + "&amp;backurl=" + Utils.getPage(0) + "") + "\">广州</a>" + "|");

        if (ptypec == 2)
            builder.Append("<b style=\"color:black\">北京" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=top&amp;ptypec=2&amp;id1=" + ptypec + "&amp;ptyped=" + ptyped + "&amp;backurl=" + Utils.getPage(0) + "") + "\">北京</a>" + "|");
        if (ptypec == 3)
            builder.Append("<b style=\"color:black\">上海" + "</b>" + "");
        else
            builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=top&amp;ptypec=3&amp;id1=" + ptypec + "&amp;ptyped=" + ptyped + "&amp;backurl=" + Utils.getPage(0) + "") + "\">上海</a>" + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        //查询条件

        if (ptypec == 1 && ptyped == 1)
        {
            strWhere = "DateDiff(dd,date,getdate())=0 and city='1'";
            strOrder = "money Desc";
        }
        if (ptypec == 1 && ptyped == 2)
        {
            strWhere = "DateDiff(week,date,getdate())=0 and city='1'";
            strOrder = "money Desc";
        }
        if (ptypec == 1 && ptyped == 3)
        {
            strWhere = "DateDiff(month,date,getdate())=0 and city='1'";
            strOrder = "money Desc";
        }
        if (ptypec == 1 && ptyped == 4)
        {
            strWhere = "city='1'";
            strOrder = "money Desc";
        }
        if (ptypec == 2 && ptyped == 1)
        {
            strWhere = "DateDiff(dd,date,getdate())=0 and city='2'";
            strOrder = "money Desc";
        }
        if (ptypec == 2 && ptyped == 2)
        {
            strWhere = "DateDiff(week,date,getdate())=0 and city='2'";
            strOrder = "money Desc";
        }
        if (ptypec == 2 && ptyped == 3)
        {
            strWhere = "DateDiff(month,date,getdate())=0 and city='2'";
            strOrder = "money Desc";
        }
        if (ptypec == 2 && ptyped == 4)
        {
            strWhere = "city='2'";
            strOrder = "money Desc";
        }
        if (ptypec == 3 && ptyped == 1)
        {
            strWhere = "DateDiff(dd,date,getdate())=0 and city='3'";
            strOrder = "money Desc";
        }
        if (ptypec == 3 && ptyped == 2)
        {
            strWhere = "DateDiff(week,date,getdate())=0 and city='3'";
            strOrder = "money Desc";
        }
        if (ptypec == 3 && ptyped == 3)
        {
            strWhere = "DateDiff(month,date,getdate())=0 and city='3'";
            strOrder = "money Desc";
        }
        if (ptypec == 3 && ptyped == 4)
        {
            strWhere = "city='3'";
            strOrder = "money Desc";
        }
        if (ptypec == 4 && ptyped == 1)
        {
            strWhere = "DateDiff(dd,date,getdate())=0";
            strOrder = "money Desc";
        }
        if (ptypec == 4 && ptyped == 2)
        {
            strWhere = "DateDiff(week,date,getdate())=0 ";
            strOrder = "money Desc";
        }
        if (ptypec == 4 && ptyped == 3)
        {
            strWhere = "DateDiff(month,date,getdate())=0 ";
            strOrder = "money Desc";
        }
        if (ptypec == 4 && ptyped == 4)
        {
            strWhere = "";
            strOrder = "money Desc";
        }

        string[] pageValUrl = { "act", "ptyped", "ptypec", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        // 开始读取列表
        IList<BCW.Model.dawnlifeTop> listdawnlifeTop = new BCW.BLL.dawnlifeTop().GetdawnlifeTops(pageIndex, 10, strWhere, strOrder, out recordCount);

        if (listdawnlifeTop.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.dawnlifeTop n in listdawnlifeTop)
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
                int usid = int.Parse(Utils.GetRequest("usid", "all", 1, @"^[1-9]\d*$", "0"));
                usid = Convert.ToInt32(n.UsID);
                int d = (pageIndex - 1) * 10 + k;
                string sText = string.Empty;
                sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "于" + Convert.ToDateTime(n.date).ToString("yyyy-MM-dd HH:mm:ss") + " 赚得" + n.money + "银两";
                builder.AppendFormat("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=top&amp;ptyped=" + ptyped + "&amp;ptypec=" + ptypec + "&amp;id=" + ptyped + "&amp;id1=" + ptypec + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">  </a>" + d + sText + "   ");
                //builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=paijiang&amp;usid=" + usid + "&amp;ptyped=" + ptyped + "&amp;ptypec=" + ptypec + "&amp;id=" + ptyped + "&amp;id1=" + ptypec + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">奖励</a>");
                k++;

                builder.Append(Out.Tab("</div>", ""));
            }
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
        }
        // 分页
        builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0) + "");

        builder.Append("<br />");
        builder.Append(Out.Tab("<div class=\"\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx") + "\">返回" + GameName + "管理</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    //排行榜按日期管理
    protected void Top1Page()
    {
        Master.Title = "" + GameName + "管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx") + "\">" + GameName + "管理</a>&gt;排行榜管理");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("" + "-----------" + "");
        builder.Append(Out.Tab("</div>", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        string searchday1 = string.Empty;
        string searchday2 = string.Empty;

        int pageIndex;
        int recordCount;
        int pageSize = 10;
        //int iSCounts = Convert.ToInt32(1000000000);
        string strWhere = " ";
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        if (Utils.ToSChinese(ac) == "马上查询")
        {
            searchday1 = Utils.GetRequest("sTime", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8])))$", DateTime.Now.ToString("yyyyMMdd"));
            searchday2 = Utils.GetRequest("oTime", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8])))$", DateTime.Now.ToString("yyyyMMdd"));

            string strText = "开始日期：,结束日期：,";
            string strName = "sTime,oTime,act";
            string strType = "date,date,hidden";
            string strValu = "" + searchday1 + "'" + searchday2 + "'''backsave";
            string strEmpt = "false,false,false";
            string strIdea = "/";
            string strOthe = "马上查询,DawnlifeManage.aspx?act=top1&amp;ptype=" + ptype + ",post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            //strWhere = "State>0 and Input_Time>='" + (DateTime.ParseExact(searchday1, "yyyyMMdd", null).ToString("yyyy-MM-dd")) + "' and Input_Time<='" + (DateTime.ParseExact(searchday2, "yyyyMMdd", null).AddDays(1).ToString("yyyy-MM-dd")) + "' GROUP BY UsID ORDER BY money DESC";

            builder.Append(Out.Tab("<div>", "<br />"));
            //查询条件
            strWhere = " date>='" + (DateTime.ParseExact(searchday1, "yyyyMMdd", null).ToString("yyyy-MM-dd")) + "' and date<='" + (DateTime.ParseExact(searchday2, "yyyyMMdd", null).AddDays(1).ToString("yyyy-MM-dd")) + "'";
            string strOrder = "money DESC";
            int ptypec = Utils.ParseInt(Utils.GetRequest("ptypec", "get", 1, @"^[0-4]$", "4"));
            int ptyped = Utils.ParseInt(Utils.GetRequest("ptyped", "get", 1, @"^[0-4]$", "4"));
            int usid = int.Parse(Utils.GetRequest("usid", "all", 1, @"^[1-9]\d*$", "0"));
            string[] pageValUrl = { "act", "ptype", "sTime", "oTime", "ptypec", "ptyped", "backurl", "ac" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            // 开始读取列表
            IList<BCW.Model.dawnlifeTop> listdawnlifeTop = new BCW.BLL.dawnlifeTop().GetdawnlifeTops(pageIndex, 10, strWhere, strOrder, out recordCount);
            if (listdawnlifeTop.Count > 0)
            {
                int k = 1;
                foreach (BCW.Model.dawnlifeTop n in listdawnlifeTop)
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
                    int d = (pageIndex - 1) * 10 + k;
                    string sText = string.Empty;
                    string city = n.city;
                    usid = Convert.ToInt32(n.UsID);
                    sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "于" + Convert.ToDateTime(n.date).ToString("yyyy-MM-dd HH:mm:ss") + " 赚得" + n.money + "银两";
                    builder.AppendFormat("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=top1DawnlifeManage.aspx?act=top1&amp;usid=" + usid + "&amp;ptype=" + ptype + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">  </a>" + d + sText + "   ");
                    //builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=rpaijiang&amp;usid=" + usid + "&amp;searchday1=" + searchday1 + "&amp;searchday2=" + searchday2 + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">奖励</a>");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0) + "<br />");
            builder.Append(Out.Tab("</div>", ""));


        }
        else
        {
            string strText = "开始日期：,结束日期：,";
            string strName = "sTime,oTime,act";
            string strType = "date,date,hidden";
            string strValu = "" + DateTime.Now.ToString("yyyyMMdd") + "'" + DateTime.Now.ToString("yyyyMMdd") + "'''backsave";
            string strEmpt = "false,false,false";
            string strIdea = "/";
            string strOthe = "马上查询,DawnlifeManage.aspx?act=top1&amp;ptype=" + ptype + ",post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("" + "-----------" + "");
            builder.Append(Out.Tab("</div>", "<br />"));

            strWhere = "State>0 GROUP BY UsID ORDER BY money DESC";
        }

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=top") + "\">返回排行榜管理</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx") + "\">返回" + GameName + "管理</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    //排行榜派奖
    private void PaijiangPage()
    {
        Master.Title = "管理中心";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx") + "\">" + GameName + "管理</a>&gt;排行榜管理");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("手动派奖:派奖酷币请手动输入，");
        builder.Append("此奖励与派奖奖励不同，管理员慎重考虑");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        int ptypec = Utils.ParseInt(Utils.GetRequest("ptypec", "get", 1, @"^[0-4]$", "4"));
        int ptyped = Utils.ParseInt(Utils.GetRequest("ptyped", "get", 1, @"^[0-4]$", "4"));
        int usid = int.Parse(Utils.GetRequest("usid", "all", 1, @"^[1-9]\d*$", "0"));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        string strText = "请输入奖励酷币：/,";
        string strName = "uid,backurl";
        string strType = "num,hidden";
        string strValu = "'" + Utils.getPage(0) + "";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "奖励,DawnlifeManage.aspx?act=paijiang1&amp;usid=" + usid + "&amp;ptyped=" + ptyped + "&amp;ptypec=" + ptypec + "&amp;id=" + ptyped + "&amp;id1=" + ptypec + "&amp;backurl=" + Utils.PostPage(1) + ",post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=top&amp;ptyped=" + ptyped + "&amp;ptypec=" + ptypec + "&amp;id=" + ptyped + "&amp;id1=" + ptypec + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">返回排行榜管理</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx") + "\">返回" + GameName + "管理</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    //派奖1
    private void PaijiangPage1()
    {
        Master.Title = "管理中心";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx") + "\">" + GameName + "管理</a>&gt;排行榜管理");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        int ptypec = Utils.ParseInt(Utils.GetRequest("ptypec", "get", 1, @"^[0-4]$", "4"));
        int ptyped = Utils.ParseInt(Utils.GetRequest("ptyped", "get", 1, @"^[0-4]$", "4"));
        int usid = int.Parse(Utils.GetRequest("usid", "all", 1, @"^[1-9]\d*$", "0"));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        if (uid > 0)
        {
            new BCW.BLL.User().UpdateiGold(usid, uid,"" + GameName + "奖励");
            //发内线
            string strLog = "根据你" + GameName + "排行榜上的赢利情况，系统自动返还了" + uid + "酷币" + "[url=/bbs/game/Dawnlife.aspx]进入" + GameName + "[/url]";
            new BCW.BLL.Guest().Add(0, usid, new BCW.BLL.User().GetUsName(usid), strLog);
            Utils.Success("奖励成功", "成功奖励，正在返回", Utils.getUrl("DawnlifeManage.aspx?act=top&amp;ptyped=" + ptyped + "&amp;ptypec=" + ptypec + "&amp;id=" + ptyped + "&amp;id1=" + ptypec + "&amp;backurl=" + Utils.PostPage(1) + ""), "1");

        }
        else
        {
            Utils.Success("", "请输入大于0的正整数", Utils.getUrl("DawnlifeManage.aspx?act=paijiang&amp;ptyped=" + ptyped + "&amp;ptypec=" + ptypec + "&amp;id=" + ptyped + "&amp;id1=" + ptypec + "&amp;backurl=" + Utils.PostPage(1) + ""), "1");
        }
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    //日期排行榜派奖
    private void RPaijiangPage()
    {
        Master.Title = "管理中心";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx") + "\">" + GameName + "管理</a>&gt;排行榜管理");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("手动派奖:派奖酷币请手动输入，");
        builder.Append("此奖励与派奖奖励不同，管理员慎重考虑");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        int ptypec = Utils.ParseInt(Utils.GetRequest("ptypec", "get", 1, @"^[0-4]$", "4"));
        int ptyped = Utils.ParseInt(Utils.GetRequest("ptyped", "get", 1, @"^[0-4]$", "4"));
        int usid = int.Parse(Utils.GetRequest("usid", "all", 1, @"^[1-9]\d*$", "0"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        string searchday1 = Utils.GetRequest("sTime", "post", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8])))$", DateTime.Now.ToString("yyyyMMdd"));
        string searchday2 = Utils.GetRequest("oTime", "post", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8])))$", DateTime.Now.ToString("yyyyMMdd"));

        string strText = "请输入奖励酷币：/,";
        string strName = "uid,backurl";
        string strType = "num,hidden";
        string strValu = "'" + Utils.getPage(0) + "";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "奖励,DawnlifeManage.aspx?act=rpaijiang1&amp;usid=" + usid + "&amp;ptype=" + ptype + "&amp;searchday1=" + searchday1 + "&amp;searchday2=" + searchday2 + "&amp;backurl=" + Utils.PostPage(1) + ",post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=top&amp;ptyped=" + ptyped + "&amp;searchday1=" + searchday1 + "&amp;searchday2=" + searchday2 + "&amp;ptypec=" + ptypec + "&amp;id=" + ptyped + "&amp;id1=" + ptypec + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">返回排行榜管理</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx") + "\">返回" + GameName + "管理</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    //日期派奖1
    private void RPaijiangPage1()
    {
        Master.Title = "管理中心";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx") + "\">" + GameName + "管理</a>&gt;排行榜管理");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        int ptypec = Utils.ParseInt(Utils.GetRequest("ptypec", "get", 1, @"^[0-4]$", "4"));
        int ptyped = Utils.ParseInt(Utils.GetRequest("ptyped", "get", 1, @"^[0-4]$", "4"));
        int usid = int.Parse(Utils.GetRequest("usid", "all", 1, @"^[1-9]\d*$", "0"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        string searchday1 = Utils.GetRequest("sTime", "get", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8])))$", DateTime.Now.ToString("yyyyMMdd"));
        string searchday2 = Utils.GetRequest("oTime", "get", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8])))$", DateTime.Now.ToString("yyyyMMdd"));

        if (uid > 0)
        {
            new BCW.BLL.User().UpdateiGold(usid, uid,"" + GameName + "奖励");
            //发内线
            string strLog = "根据你" + GameName + "排行榜上的赢利情况，系统自动返还了" + uid + "酷币" + "[url=/bbs/game/Dawnlife.aspx]进入" + GameName + "[/url]";
            new BCW.BLL.Guest().Add(0, usid, new BCW.BLL.User().GetUsName(usid), strLog);
            Utils.Success("奖励成功", "成功奖励，正在返回", Utils.getUrl("DawnlifeManage.aspx?act=top1&amp;usid=" + usid + "&amp;ptype=" + ptype + "&amp;searchday1=" + searchday1 + "&amp;searchday2=" + searchday2 + "&amp;backurl=" + Utils.PostPage(1) + ""), "1");

        }
        else
        {
            Utils.Success("", "请输入大于0的正整数", Utils.getUrl("DawnlifeManage.aspx?act=rpaijiang&amp;searchday1=" + searchday1 + "&amp;searchday2=" + searchday2 + "&amp;ptyped=" + ptyped + "&amp;ptypec=" + ptypec + "&amp;id=" + ptyped + "&amp;id1=" + ptypec + "&amp;backurl=" + Utils.PostPage(1) + ""), "1");
        }
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    //奖池管理
    protected void GiftPage()
    {
        Master.Title = "管理中心";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx") + "\">" + GameName + "管理</a>&gt;奖池管理");
        builder.Append(Out.Tab("</div>", "<br />"));

        int meid = new BCW.User.Users().GetUsId();
        string name = new BCW.BLL.User().GetUsName(meid);

        BCW.Model.dawnlifegift give = new BCW.Model.dawnlifegift();
        int r = new BCW.BLL.dawnlifegift().GetMaxId();
        BCW.Model.dawnlifegift re = new BCW.BLL.dawnlifegift().Getdawnlifegift(r - 1);
        int gift2 = Convert.ToInt32(re.gift * jcx / 100) + Convert.ToInt32(re.giftj * juankuan / 100);
        string _time = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
        int _time_num = Convert.ToInt32(_time);
        if (_time_num == 0)
        {
            give.date = DateTime.Now;
            give.gift = gift2;
            give.giftj = Convert.ToInt32(re.giftj * (1 - (juankuan / 100)));
            give.UsID = 1;
            give.UsName = "1";
            give.coin = gift2;
            give.state = 3;
            new BCW.BLL.dawnlifegift().Add(give);
        }
        r = new BCW.BLL.dawnlifegift().GetMaxId();
        BCW.Model.dawnlifegift gift = new BCW.BLL.dawnlifegift().Getdawnlifegift(r - 1);
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b style=\"color:black\">捐款总池：</b>");
        builder.Append("" + gift.giftj + "酷币");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=juankuan") + "\">捐款排行榜</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\""+Utils.getUrl("DawnlifeManage.aspx?act=jilu")+"\">奖池记录</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b style=\"color:red\">今日奖池：</b>");
        builder.Append("" + gift.gift + "酷币");
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=gift") + "\">刷新</a>");
        builder.Append(Out.Tab("</div>", ""));

        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        string strText = "更改奖池酷币为:/,";
        string strName = "uid,backurl";
        string strType = "num,hidden";
        string strValu = "'" + Utils.getPage(0) + "";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "确认,DawnlifeManage.aspx?act=give,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("特别说明：今日奖池为当日的奖池酷币，"+jcd+"%用于当日排行的酷币奖励，"+jcx+"%用于第二日奖池的投入，管理员可直接更改奖池的酷币值，注意玩家随时进入游戏与捐款奖池都会变更，进行奖池更改时慎重操作！" + "<br />");
        builder.Append("特别提醒：每日凌晨00:00:00到00:01:00这一时间段内奖池更新，请管理员慎重操作" + "<br />");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx") + "\">返回" + GameName + "管理</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    //奖池详细记录
    private void JiluPage()
    {
        Master.Title = "管理中心";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx") + "\">" + GameName + "管理</a>&gt;奖池记录");
        builder.Append(Out.Tab("</div>", "<br />"));

        int ptypec = Utils.ParseInt(Utils.GetRequest("ptypec", "get", 1, @"^[0-4]$", "1"));

        builder.Append(Out.Tab("<div>",""));
        string searchday = (Utils.GetRequest("searchday", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8])))$", ""));
        string strText = "输入查询日期：格式（20160307）/,";
        string strName = "searchday";
        string strType = "num";
        string strValu = searchday;
        string strEmpt = "";
        string strIdea = "";
        string strOthe = "搜奖池记录,DawnlifeManage.aspx?act=jilu,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("</div>", "<br />"));

        string[] pageValUrl = { "act", "ptyped", "ptypec", "searchday", "backurl" };
        int pageIndex = 0;//当前页
        int recordCount;//记录总条数
        int pageSize = 10;//分页大小
        string strOrder = "";
        string strWhere = string.Empty;
        string strWhere1 = string.Empty;
        if (searchday == "")
        {
            strWhere1 = "DateDiff(month,date,getdate())=0";
        }
        else
        {
            strWhere1 = "convert(varchar(10),date,120)='" + (DateTime.ParseExact(searchday, "yyyyMMdd", null).ToString("yyyy-MM-dd")) + "'";
        }
 
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("【奖池记录】");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        if (searchday == "")
        {
            builder.Append("【当月酷币记录】");
        }
        else
        {
            builder.Append("【"+(DateTime.ParseExact(searchday, "yyyyMMdd", null).ToString("yyyy年MM月dd日")) + "酷币记录" + "】");
        }
        builder.Append(Out.Tab("</div>", "<br />"));
   

        //查询条件

            strWhere = strWhere1+" and (state='0' or state='3'or state='4') ";
            strOrder = "date Desc";
      
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            // 开始读取列表
            IList<BCW.Model.dawnlifegift> listdawnlifeTop = new BCW.BLL.dawnlifegift().Getdawnlifegifts(pageIndex, 10, strWhere, strOrder, out recordCount);

            if (listdawnlifeTop.Count > 0)
            {
                int k = 1;
                foreach (BCW.Model.dawnlifegift n in listdawnlifeTop)
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
                    int usid = int.Parse(Utils.GetRequest("usid", "all", 1, @"^[1-9]\d*$", "0"));
                    usid = Convert.ToInt32(n.UsID);
                    int d = (pageIndex - 1) * 10 + k;
                    string sText = string.Empty;
                    if (n.state == 0)
                    {
                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "入场消费" + n.coin + "|奖池结余：" + n.gift + "|"+"("+ Convert.ToDateTime(n.date).ToString("yyyy-MM-dd HH:mm:ss") +")";
                    }
                    if (n.state == 3)
                    {
                        sText = "." + "<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=xitongpaijiang&amp;id="+n.ID+"&amp;backurl=" + Utils.PostPage(1) + "") + "\">系统派奖</a>-" + n.coin + "|系统更新（捐款奖池赠）" + n.UsName + "|奖池结余：" + n.gift + "|" + "(" + Convert.ToDateTime(n.date).ToString("yyyy-MM-dd HH:mm:ss") + ")";
                    }
                    if (n.state == 4)
                    {
                        sText = "." + "管理员" + " 直接修改奖池|奖池余：" + n.gift + ""+"|(" + Convert.ToDateTime(n.date).ToString("yyyy-MM-dd HH:mm:ss") + ")";
                    }
                    builder.AppendFormat("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=top&amp;ptypec=" + ptypec + "&amp;id1=" + ptypec + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">  </a>" + d + sText + "   ");

                    k++;

                    builder.Append(Out.Tab("</div>", ""));
                }
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0) + "");


        builder.Append(Out.Tab("<div>", "<br /><br />"));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx") + "\">返回" + GameName + "管理</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    //系统派奖详细
    private void XTPJPage()
    {
        Master.Title = "管理中心";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx") + "\">" + GameName + "管理</a>&gt;派奖记录");
        builder.Append(Out.Tab("</div>", "<br />"));
        int id = Utils.ParseInt(Utils.GetRequest("id", "get", 1, @"^[1-9]\d*$", "1"));
        BCW.Model.dawnlifegift dss = new BCW.BLL.dawnlifegift().Getdawnlifegift(id);
        DateTime dsa =Convert .ToDateTime( dss.date);
        string dtr =dsa.ToString("yyyy-MM-dd HH:mm:ss");
        DateTime dsds = Convert.ToDateTime(dss.date).AddDays(-1);
        string abc = dsds.ToString("yyyy-MM-dd");

        //兑奖页面
        builder.Append(Out.Tab("<div>", ""));
        string day = dsds.ToString("yyyy年MM月dd日");
        builder.Append("" + day + "获奖名单：" + "<br />");
        builder.Append(Out.Tab("</div>", ""));

        int pageIndex = 0;//当前页
        int recordCount;//记录总条数
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));//分页大小
        string strWhere = "";
        string strOrder = "";

        strWhere = " Convert(varchar,date,120) like '%" + abc + "%'";
        strOrder = "gift Desc";
        IList<BCW.Model.dawnlifegift> listdawnlifeTop3 = new BCW.BLL.dawnlifegift().Getdawnlifegifts(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listdawnlifeTop3.Count > 0)
        {
            int l = 1;
            foreach (BCW.Model.dawnlifegift n3 in listdawnlifeTop3)
            {
                if (l <= 1)
                {
                    int gift = Convert.ToInt32(n3.gift);
                    //查询条件
                 
                    strOrder = "money Desc";
                    string[] pageValUrl = { "act", "ptypet", "ptyped", "backurl" };
                    pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                    if (pageIndex == 0)
                        pageIndex = 1;
                    // 开始读取列表
                    IList<BCW.Model.dawnlifeTop> listdawnlifeTop = new BCW.BLL.dawnlifeTop().GetdawnlifeTops(pageIndex, pageSize, strWhere, strOrder, out recordCount);
                    if (listdawnlifeTop.Count > 0)
                    {
                        int k = 1;
                        int j = 1;
                        //int h = 1;
                        int h = Utils.ParseInt(Utils.GetRequest("h", "get", 1, @"^[0-5]$", "1"));
                        foreach (BCW.Model.dawnlifeTop n in listdawnlifeTop)
                        {
                            if (j <= 5)
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
                                int d = (pageIndex - 1) * 10 + k;
                                int coin = Convert.ToInt16(n.coin);
                                int money = Convert.ToInt32(n.money);
                                string sText = string.Empty;
                                    int award = 0;
                                    if (h == 1) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q1 / 100); }
                                    if (h == 2) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q2 / 100); }
                                    if (h == 3) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q3 / 100); }
                                    if (h == 4) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q4 / 100); }
                                    if (h == 5) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q5 / 100); }
                                    if (n.sum == 0 && n.money > 0)
                                    {
                                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "赚得" + n.money + "银两|" + "全国榜奖励" + award + "酷币" + "|派奖时间"+dtr;
                                    }
                                    else
                                    {
                                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "赚得" + n.money + "银两|" + "全国榜奖励" + award + "酷币" + "|派奖时间" + dtr;
                                    }
                        
                                builder.AppendFormat("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=award&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>" + d + sText);
                                k++;
                                h++;
                                builder.Append(Out.Tab("</div>", ""));
                            }
                            else
                            {
                                ;
                            }
                            j++;
                        }
                    }
                    else
                    {
                        builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
                    }

                }
                else
                {
                    ;
                }
                l++;
            }
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
        }
        builder.Append(Out.Tab("<br />", Out.Hr()));

        string strWhere1 = string.Empty;
        strWhere1 = " Convert(varchar,date,120) like '%" + abc + "%'";
        strOrder = "gift Desc";
        IList<BCW.Model.dawnlifegift> listdawnlifeTopg = new BCW.BLL.dawnlifegift().Getdawnlifegifts(pageIndex, pageSize, strWhere1, strOrder, out recordCount);
        if (listdawnlifeTopg.Count > 0)
        {
            int l = 1;
            foreach (BCW.Model.dawnlifegift n3 in listdawnlifeTopg)
            {
                if (l <= 1)
                {
                    int gift = Convert.ToInt32(n3.gift);
                    //查询条件
                    strWhere = " Convert(varchar,date,120) like '%" + abc + "%' and city ='1' ";
                    strOrder = "money Desc";
                    string[] pageValUrl = { "act", "ptypet", "ptyped", "backurl" };
                    pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                    if (pageIndex == 0)
                        pageIndex = 1;
                    // 开始读取列表
                    IList<BCW.Model.dawnlifeTop> listdawnlifeTop = new BCW.BLL.dawnlifeTop().GetdawnlifeTops(pageIndex, pageSize, strWhere, strOrder, out recordCount);
                    if (listdawnlifeTop.Count > 0)
                    {
                        int k = 1;
                        int j = 1;
                        //int h = 1;
                        int h = Utils.ParseInt(Utils.GetRequest("h", "get", 1, @"^[0-5]$", "1"));
                        foreach (BCW.Model.dawnlifeTop n in listdawnlifeTop)
                        {
                            if (j <= 5)
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
                                int d = (pageIndex - 1) * 10 + k;
                                int coin = Convert.ToInt16(n.coin);
                                int money = Convert.ToInt32(n.money);
                                string sText = string.Empty;
                                int award = 0;
                                if (h == 1) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                if (h == 2) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                if (h == 3) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                if (h == 4) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                if (h == 5) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                if (n.sum == 0 && n.money > 0)
                                {
                                    sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "赚得" + n.money + "银两|" + "广州榜奖励" + award + "酷币" + "|派奖时间" + dtr;
                                }
                                else
                                {
                                    sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "赚得" + n.money + "银两|" + "广州榜奖励" + award + "酷币" + "|派奖时间" + dtr;
                                }

                                builder.AppendFormat("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=award&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>" + d + sText);
                                k++;
                                h++;
                                builder.Append(Out.Tab("</div>", ""));
                            }
                            else
                            {
                                ;
                            }
                            j++;
                        }
                    }
                    else
                    {
                        builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
                    }

                }
                else
                {
                    ;
                }
                l++;
            }
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
        }
        builder.Append(Out.Tab("<br />", Out.Hr()));


        string strWhere2 = string.Empty;
        strWhere2 = " Convert(varchar,date,120) like '%" + abc + "%'";
        strOrder = "gift Desc";
        IList<BCW.Model.dawnlifegift> listdawnlifeTopb = new BCW.BLL.dawnlifegift().Getdawnlifegifts(pageIndex, pageSize, strWhere2, strOrder, out recordCount);
        if (listdawnlifeTopb.Count > 0)
        {
            int l = 1;
            foreach (BCW.Model.dawnlifegift n3 in listdawnlifeTopb)
            {
                if (l <= 1)
                {
                    int gift = Convert.ToInt32(n3.gift);
                    //查询条件
                    strWhere = " Convert(varchar,date,120) like '%" + abc + "%' and city ='2' ";
                    strOrder = "money Desc";
                    string[] pageValUrl = { "act", "ptypet", "ptyped", "backurl" };
                    pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                    if (pageIndex == 0)
                        pageIndex = 1;
                    // 开始读取列表
                    IList<BCW.Model.dawnlifeTop> listdawnlifeTop = new BCW.BLL.dawnlifeTop().GetdawnlifeTops(pageIndex, pageSize, strWhere, strOrder, out recordCount);
                    if (listdawnlifeTop.Count > 0)
                    {
                        int k = 1;
                        int j = 1;
                        //int h = 1;
                        int h = Utils.ParseInt(Utils.GetRequest("h", "get", 1, @"^[0-5]$", "1"));
                        foreach (BCW.Model.dawnlifeTop n in listdawnlifeTop)
                        {
                            if (j <= 5)
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
                                int d = (pageIndex - 1) * 10 + k;
                                int coin = Convert.ToInt16(n.coin);
                                int money = Convert.ToInt32(n.money);
                                string sText = string.Empty;
                                int award = 0;
                                if (h == 1) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                if (h == 2) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                if (h == 3) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                if (h == 4) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                if (h == 5) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                if (n.sum == 0 && n.money > 0)
                                {
                                    sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "赚得" + n.money + "银两|" + "北京榜奖励" + award + "酷币" + "|派奖时间" + dtr;
                                }
                                else
                                {
                                    sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "赚得" + n.money + "银两|" + "北京榜奖励" + award + "酷币" + "|派奖时间" + dtr;
                                }

                                builder.AppendFormat("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=award&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>" + d + sText);
                                k++;
                                h++;
                                builder.Append(Out.Tab("</div>", ""));
                            }
                            else
                            {
                                ;
                            }
                            j++;
                        }
                    }
                    else
                    {
                        builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
                    }

                }
                else
                {
                    ;
                }
                l++;
            }
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
        }
        builder.Append(Out.Tab("<br />", Out.Hr()));

        string strWhere3 = string.Empty;
        strWhere3 = " Convert(varchar,date,120) like '%" + abc + "%'";
        strOrder = "gift Desc";
        IList<BCW.Model.dawnlifegift> listdawnlifeTops = new BCW.BLL.dawnlifegift().Getdawnlifegifts(pageIndex, pageSize, strWhere3, strOrder, out recordCount);
        if (listdawnlifeTops.Count > 0)
        {
            int l = 1;
            foreach (BCW.Model.dawnlifegift n3 in listdawnlifeTops)
            {
                if (l <= 1)
                {
                    int gift = Convert.ToInt32(n3.gift);
                    //查询条件
                    strWhere = " Convert(varchar,date,120) like '%" + abc + "%' and city ='3' ";
                    strOrder = "money Desc";
                    string[] pageValUrl = { "act", "ptypet", "ptyped", "backurl" };
                    pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                    if (pageIndex == 0)
                        pageIndex = 1;
                    // 开始读取列表
                    IList<BCW.Model.dawnlifeTop> listdawnlifeTop = new BCW.BLL.dawnlifeTop().GetdawnlifeTops(pageIndex, pageSize, strWhere, strOrder, out recordCount);
                    if (listdawnlifeTop.Count > 0)
                    {
                        int k = 1;
                        int j = 1;
                        //int h = 1;
                        int h = Utils.ParseInt(Utils.GetRequest("h", "get", 1, @"^[0-5]$", "1"));
                        foreach (BCW.Model.dawnlifeTop n in listdawnlifeTop)
                        {
                            if (j <= 5)
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
                                int d = (pageIndex - 1) * 10 + k;
                                int coin = Convert.ToInt16(n.coin);
                                int money = Convert.ToInt32(n.money);
                                string sText = string.Empty;
                                int award = 0;
                                if (h == 1) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                if (h == 2) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                if (h == 3) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                if (h == 4) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                if (h == 5) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                if (n.sum == 0 && n.money > 0)
                                {
                                    sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "赚得" + n.money + "银两|" + "上海榜奖励" + award + "酷币" + "|派奖时间" + dtr;
                                }
                                else
                                {
                                    sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "赚得" + n.money + "银两|" + "上海榜奖励" + award + "酷币" + "|派奖时间" + dtr;
                                }

                                builder.AppendFormat("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=award&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>" + d + sText);
                                k++;
                                h++;
                                builder.Append(Out.Tab("</div>", ""));
                            }
                            else
                            {
                                ;
                            }
                            j++;
                        }
                    }
                    else
                    {
                        builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
                    }

                }
                else
                {
                    ;
                }
                l++;
            }
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
        }
        builder.Append(Out.Tab("<br />", Out.Hr()));

        builder.Append(Out.Tab("<div>",""));
       builder.Append("<a href=\"" + Utils.getPage("game.aspx&amp;backurl=" + Utils.PostPage(1)+"") + "\">返回上级</a><br />");
       builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx") + "\">返回" + GameName + "管理</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    //闯荡记录
    private void CdjlPage()
    {

        Master.Title = "管理中心";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx") + "\">" + GameName + "管理</a>&gt;闯荡记录");
        builder.Append(Out.Tab("</div>", "<br />"));


        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        if (Utils.ToSChinese(ac) == "确认搜索")
        {
            int usid = int.Parse(Utils.GetRequest("UsID", "all", 1, @"^[1-9]\d*$", "0"));
            string strText = "输入用户ID:/,,";
            string strName = "usid,act";
            string strType = "num,hidden";
            string strValu = "" + usid + "'" + Utils.getPage(0) + "";
            string strEmpt = "true,false";
            string strIdea = "/";
            string strOthe = "确认搜索,DawnlifeManage.aspx?act=cdjl,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append("<br />");
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));//分页大小;
            string strWhere = string.Empty;
            strWhere = "UsID=" + usid ;
            string strOrder;
            strOrder = " date DESC ";
            string[] pageValUrl = { "act", "usid", "ac", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
                  // 开始读取列表
        IList<BCW.Model.dawnlifeTop> listdawnlifeTop = new BCW.BLL.dawnlifeTop().GetdawnlifeTops(pageIndex, 10, strWhere, strOrder, out recordCount);

        if (usid!= 0)
        {
            if (listdawnlifeTop.Count > 0)
            {
                int k = 1;
                foreach (BCW.Model.dawnlifeTop n in listdawnlifeTop)
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

                    usid = Convert.ToInt32(n.UsID);
                    int d = (pageIndex - 1) * 10 + k;
                    string sText = string.Empty;
                    string ccity = string.Empty;
                    if (n.city == "1") { ccity = "广州"; }
                    if (n.city == "2") { ccity = "北京"; }
                    if (n.city == "3") { ccity = "上海"; }
                    sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "在" + ccity + "赚得" + n.money + "银两|" + "编号ID " + n.coin + "|(" + Convert.ToDateTime(n.date).ToString("yyyy-MM-dd HH:mm:ss") + ")<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=cdjilu&amp;id=" + n.coin + "&amp;usid="+n.UsID+"") + "\">查看</a>";

                    builder.AppendFormat("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=cdjl&amp;backurl=" + Utils.PostPage(1) + "") + "\">  </a>" + d + sText + "   ");

                    k++;

                    builder.Append(Out.Tab("</div>", ""));
                }
            }

            else
            {
                builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
            }
        }
        else
        {
            strWhere = "";
            strOrder = " date DESC ";
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            // 开始读取列表
            IList<BCW.Model.dawnlifeTop> listdawnlifeTop1 = new BCW.BLL.dawnlifeTop().GetdawnlifeTops(pageIndex, 10, strWhere, strOrder, out recordCount);

                int k = 1;
                foreach (BCW.Model.dawnlifeTop n in listdawnlifeTop1)
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

                    usid = Convert.ToInt32(n.UsID);
                    int d = (pageIndex - 1) * 10 + k;
                    string sText = string.Empty;
                    string ccity = string.Empty;
                    if (n.city == "1") { ccity = "广州"; }
                    if (n.city == "2") { ccity = "北京"; }
                    if (n.city == "3") { ccity = "上海"; }
                    sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "在" + ccity + "赚得" + n.money + "银两|" + "编号ID " + n.coin + "|(" + Convert.ToDateTime(n.date).ToString("yyyy-MM-dd HH:mm:ss") + ")<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=cdjilu&amp;id=" + n.coin + "&amp;usid=" + n.UsID + "") + "\">查看</a>";

                    builder.AppendFormat("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=cdjl&amp;backurl=" + Utils.PostPage(1) + "") + "\">  </a>" + d + sText + "   ");

                    k++;

                    builder.Append(Out.Tab("</div>", ""));
                }

        }
        // 分页
        builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0) + "");
   
        }
        else
        {
        
            string strText = "输入用户ID:/,,";
            string strName = "usid,act";
            string strType = "num,hidden";
            string strValu = "'" + Utils.getPage(0) + "";
            string strEmpt = "true,false";
            string strIdea = "/";
            string strOthe = "确认搜索,DawnlifeManage.aspx?act=cdjl,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append("<br />");

            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));//分页大小;
            string strWhere = string.Empty;
            strWhere = " ";
            string strOrder;
            strOrder = " date DESC ";
            string[] pageValUrl = { "act", "usid", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
                  // 开始读取列表
        IList<BCW.Model.dawnlifeTop> listdawnlifeTop = new BCW.BLL.dawnlifeTop().GetdawnlifeTops(pageIndex, 10, strWhere, strOrder, out recordCount);
        if (listdawnlifeTop.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.dawnlifeTop n in listdawnlifeTop)
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
                int usid = int.Parse(Utils.GetRequest("usid", "all", 1, @"^[1-9]\d*$", "0"));
                usid = Convert.ToInt32(n.UsID);
                int d = (pageIndex - 1) * 10 + k;
                string sText = string.Empty;
                string ccity = string.Empty;
                if (n.city == "1") { ccity = "广州"; }
                if (n.city == "2") { ccity = "北京"; }
                if (n.city == "3") { ccity = "上海"; }
                sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "在" + ccity + "赚得" + n.money + "银两|" + "编号ID " + n.coin + "|(" + Convert.ToDateTime(n.date).ToString("yyyy-MM-dd HH:mm:ss") + ")<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=cdjilu&amp;id=" + n.coin + "&amp;usid=" + n.UsID + "") + "\">查看</a>";
          
                builder.AppendFormat("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=cdjl&amp;backurl=" + Utils.PostPage(1) + "") + "\">  </a>" + d + sText + "   ");

                k++;

                builder.Append(Out.Tab("</div>", ""));
            }
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
        }
        // 分页
        builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0) + "");
        }


        builder.Append(Out.Tab("<div>", "<br /><br />"));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx") + "\">返回" + GameName + "管理</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    //闯荡详细记录
    private void cdjiluPage()
    {
        string Logo = ub.GetSub("DawnlifeLogo", xmlPath);
        int id = Utils.ParseInt(Utils.GetRequest("id", "all", 1, @"^[0-9]\d*$", "0"));
        int usid = Utils.ParseInt(Utils.GetRequest("usid", "all", 1, @"^[0-9]\d*$", "0"));
        string name = new BCW.BLL.User().GetUsName(usid);
        Master.Title = "管理中心";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx") + "\">" + GameName + "管理</a>&gt;闯荡详细记录");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex = 0;//当前页
        int recordCount;//记录总条数
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));//分页大小
        string strWhere = "";
        string strOrder = "";
        //查询条件   
        strWhere = "UsID=" + usid + " and coin =" + id + " ";
        strOrder = " day , date , money";
        string[] pageValUrl = { "act", "ptype", "id","usid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        // 开始读取列表
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("闯荡编号：" + id + "<br />");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("用户：" + name +"("+usid+")"+ "<br />");
        builder.Append(Out.Tab("</div>", ""));
        IList<BCW.Model.dawnlifenotes> listdawnlifeTop1 = new BCW.BLL.dawnlifenotes().Getdawnlifenotess2(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listdawnlifeTop1.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.dawnlifenotes n1 in listdawnlifeTop1)
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
                int d = (pageIndex - 1) * 10 + k;
                string sText = string.Empty;
                string city = Convert.ToString(n1.city);
                //string area = string.Empty;
                string caozuo = string.Empty;
                if (n1.buy == "buy" && n1.sell == "sell") { caozuo = "无买卖"; }
                if (n1.buy != "buy" && n1.sell == "sell") { caozuo = "购买." + n1.buy; }
                if (n1.buy == "buy" && n1.sell != "sell") { caozuo = "出售." + n1.sell; }
                if (n1.buy == "如来神掌" && n1.sell == "sellr") { caozuo = "赠送." + n1.buy; }
                if (n1.buy == "走私汽车" && n1.sell == "sellz") { caozuo = "赠送." + n1.buy; }
                if (n1.buy == "buy0.1" && n1.sell == "sell") { caozuo = "银两减少10%"; }
                if (n1.buy == "buy0.05" && n1.sell == "sell") { caozuo = "银两减少5%"; }
                if (n1.buy == "buy" && n1.sell == "sell0.05") { caozuo = "欠款增加5%"; }
                if (n1.buy == "buy" && n1.sell == "sell9968") { caozuo = "欠款增加9968"; }
                if (n1.buy == "buyq" && n1.sell == "sell") { caozuo = "还钱"; }
                if (n1.buy == "sell") { caozuo = "出售" + n1.sell; }
                if (n1.buy == "yiyuanq" && n1.sell == "yiyuanq") { caozuo = "恢复健康"; }
                if (n1.buy == "yiyuan" && n1.sell == "yiyuan") { caozuo = "恢复健康"; }
                if (n1.buy == "mingshengq" && n1.sell == "mingshengq") { caozuo = "恢复名声"; }
                if (n1.buy == "mingsheng" && n1.sell == "mingsheng") { caozuo = "恢复名声"; }
                if (n1.buy == "cksj" && n1.sell == "cksj") { caozuo = "仓库升级"; }

                BCW.Model.dawnlifedaoju rg = new BCW.BLL.dawnlifedaoju().Getdawnlifedaoju(1);
                BCW.Model.dawnlifedaoju rb = new BCW.BLL.dawnlifedaoju().Getdawnlifedaoju(2);
                BCW.Model.dawnlifedaoju rs = new BCW.BLL.dawnlifedaoju().Getdawnlifedaoju(3);
                //string[] area = rg.area.Split('/');
                if (n1.city == 1)
                {
                    city = "广州";
                    string[] area = rg.area.Split('/');
                    if (n1.day == 0) { sText = "." + "[第" + n1.day + "天." + city + "" + "][" + caozuo + ".数量." + n1.num + "|价钱" + n1.price + "][" + "银两." + n1.money + "|" + "欠款." + n1.debt + "](" + Convert.ToDateTime(n1.date).ToString("yyyy-MM-dd HH:mm:ss") + ")"; }
                  else if (n1.day == 31)
                    {
                      sText = "." + "[第" + n1.day + "天." + city + "" + "][" + caozuo + "][" + "银两." + n1.money + "|" + "欠款." + n1.debt + "](" + Convert.ToDateTime(n1.date).ToString("yyyy-MM-dd HH:mm:ss") + ")"; 
                    }
                   
                    else
                    {
                        sText = "." + "[第" + n1.day + "天." + city + "." + area[n1.area - 1] + "][" + caozuo + ".数量." + n1.num + "|价钱" + n1.price + "][" + "银两." + n1.money + "|" + "欠款." + n1.debt + "](" + Convert.ToDateTime(n1.date).ToString("yyyy-MM-dd HH:mm:ss") + ")";
                    }
                }
                if (n1.city == 2)
                {
                    city = "北京";
                    string[] area = rb.area.Split('/');
                    if (n1.day == 0) { sText = "." + "[第" + n1.day + "天." + city + "" + "][" + caozuo + ".数量." + n1.num + "|价钱" + n1.price + "][" + "银两." + n1.money + "|" + "欠款." + n1.debt + "](" + Convert.ToDateTime(n1.date).ToString("yyyy-MM-dd HH:mm:ss") + ")"; }
                    else if (n1.day == 31) { sText = "." + "[第" + n1.day + "天." + city + "" + "][" + caozuo + "][" + "银两." + n1.money + "|" + "欠款." + n1.debt + "](" + Convert.ToDateTime(n1.date).ToString("yyyy-MM-dd HH:mm:ss") + ")"; }
                  
                    else
                    {
                        sText = "." + "[第" + n1.day + "天." + city + "." + area[n1.area - 1] + "][" + caozuo + ".数量." + n1.num + "|价钱" + n1.price + "][" + "银两." + n1.money + "|" + "欠款." + n1.debt + "](" + Convert.ToDateTime(n1.date).ToString("yyyy-MM-dd HH:mm:ss") + ")";
                    }
                }
                if (n1.city == 3)
                {
                    city = "上海";
                    string[] area = rs.area.Split('/');
                    if (n1.day == 0) { sText = "." + "[第" + n1.day + "天." + city + "" + "][" + caozuo + ".数量." + n1.num + "|价钱" + n1.price + "][" + "银两." + n1.money + "|" + "欠款." + n1.debt + "](" + Convert.ToDateTime(n1.date).ToString("yyyy-MM-dd HH:mm:ss") + ")"; }
                    else if (n1.day == 31) { sText = "." + "[第" + n1.day + "天." + city + "" + "][" + caozuo + "][" + "银两." + n1.money + "|" + "欠款." + n1.debt + "](" + Convert.ToDateTime(n1.date).ToString("yyyy-MM-dd HH:mm:ss") + ")"; }
                  
                    else
                    {
                        sText = "." + "[第" + n1.day + "天." + city + "." + area[n1.area - 1] + "][" + caozuo + ".数量." + n1.num + "|价钱" + n1.price + "][" + "银两." + n1.money + "|" + "欠款." + n1.debt + "](" + Convert.ToDateTime(n1.date).ToString("yyyy-MM-dd HH:mm:ss") + ")";
                    }
                }
                //sText = "." + "[第" + n1.day + "天." + city + "." +area [ n1.area ]+ "][" + caozuo + ".数量." + n1.num + ".价钱" + n1.price + "][" + "银两." + n1.money + "|" + "欠款." + n1.debt + "](" + Convert.ToDateTime(n1.date).ToString("yyyy-MM-dd HH:mm:ss")+")";

                builder.AppendFormat("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=join&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>" + d + sText);
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
        }

        else
        {
            builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
        }
        builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0) + "");

        //游戏底部Ubb
        string Foot = ub.GetSub("DawnlifeFoot", xmlPath);
        if (Foot != "")
        {
            builder.Append("<br />");
            builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(Foot)));
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("" + "-----------" + "<br />");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>",""));
        builder.Append("特别说明：<br/>购买减少的银两=数量×价钱<br />卖出增加的银两=数量×价钱<br />还钱、银两减少百分之几银两直接减少<br />恢复健康/恢复名声减少银两=价钱，数量为增加的值<br />仓库升级减少银两=价钱，数量为升级后的仓库库存大小");
        builder.Append(Out.Tab("</div>","<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=cdjl") + "\">返回闯荡记录</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx") + "\">返回" + GameName + "管理</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    //捐款排行榜
    private void JuankuanPage()
    {
        Master.Title = "管理中心";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx") + "\">" + GameName + "管理</a>&gt;捐款排行榜");
        builder.Append(Out.Tab("</div>", "<br />"));

        int meid = new BCW.User.Users().GetUsId();
        string name = new BCW.BLL.User().GetUsName(meid);

        BCW.Model.dawnlifegift give = new BCW.Model.dawnlifegift();
        int r = new BCW.BLL.dawnlifegift().GetMaxId();
        BCW.Model.dawnlifegift re = new BCW.BLL.dawnlifegift().Getdawnlifegift(r - 1);
        int gift2 = Convert.ToInt32(re.gift * jcx / 100) + Convert.ToInt32(re.giftj * juankuan / 100);
        string _time = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
        int _time_num = Convert.ToInt32(_time);
        if (_time_num == 0)
        {
            give.date = DateTime.Now;
            give.gift = gift2;
            give.giftj = Convert.ToInt32(re.giftj * (1 - (juankuan / 100)));
            give.UsID = 1;
            give.UsName = "1";
            give.coin = gift2;
            give.state =3;
            new BCW.BLL.dawnlifegift().Add(give);
        }
        r = new BCW.BLL.dawnlifegift().GetMaxId();
        BCW.Model.dawnlifegift gift = new BCW.BLL.dawnlifegift().Getdawnlifegift(r - 1);
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b style=\"color:black\">捐款总池：</b>");
        builder.Append("" + gift.giftj + "酷币");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex = 0;//当前页
        int recordCount;//记录总条数
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));//分页大小
        string strWhere = "";
        string strOrder = "";
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("" + GameName + "捐款记录：" + "<br />");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[0-4]$", "3"));
        int ptyped = Utils.ParseInt(Utils.GetRequest("ptyped", "get", 1, @"^[0-4]$", "2"));
        if (ptyped == 1)
            builder.Append("<b style=\"color:black\">今日" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=juankuan&amp;ptyped=1&amp;id=" + ptyped + "&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">今日</a>" + "|");
        if (ptyped == 2)
            builder.Append("<b style=\"color:black\">历史" + "</b>" + "");
        else
            builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=juankuan&amp;ptyped=2&amp;id=" + ptyped + "&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">历史</a>" + "");
        builder.Append("<br />");
        builder.Append(Out.Tab("</div>", "")); builder.Append(Out.Tab("<div>", ""));
        //if (ptype == 1)
        //    builder.Append("<b style=\"color:black\">我的捐款记录" + "</b>" + "|");
        //else
        //    builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=gift&amp;ptype=1&amp;id1=" + ptype + "&amp;ptyped=" + ptyped + "&amp;backurl=" + Utils.getPage(0) + "") + "\">我的捐款记录</a>" + "|");
        if (ptype == 2)
            builder.Append("<b style=\"color:black\">捐款记录" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=juankuan&amp;ptype=2&amp;id1=" + ptype + "&amp;ptyped=" + ptyped + "&amp;backurl=" + Utils.getPage(0) + "") + "\">捐款记录</a>" + "|");
        if (ptype == 3)
            builder.Append("<b style=\"color:black\">捐款排行" + "</b>" + "<br />");
        else
            builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=juankuan&amp;ptype=3&amp;id1=" + ptype + "&amp;ptyped=" + ptyped + "&amp;backurl=" + Utils.getPage(0) + "") + "\">捐款排行</a>" + "<br />");
        builder.Append(Out.Tab("</div>", ""));
        if (ptyped == 1 && ptype == 1)
        {
            strWhere = "state=1 and DateDiff(dd,date,getdate())=0 and UsID=" + meid;
            strOrder = "date Desc";
        }
        if (ptyped == 1 && ptype == 2)
        {
            strWhere = "state=1 and DateDiff(dd,date,getdate())=0";
            strOrder = "date Desc";
        }
        if (ptyped == 1 && ptype == 3)
        {
            strWhere = "state=1 and DateDiff(dd,date,getdate())=0";
            strOrder = "coin Desc , date Desc ";
        }
        if (ptyped == 2 && ptype == 1)
        {
            strWhere = "state=1 and UsID=" + meid;
            strOrder = "date Desc";
        }
        if (ptyped == 2 && ptype == 2)
        {
            strWhere = "state=1 ";
            strOrder = "date Desc";
        }
        if (ptyped == 2 && ptype == 3)
        {
            strWhere = "state=1 ";
            strOrder = "coin Desc , date Desc ";
        }
        string[] pageValUrl = { "act", "ptype", "ptyped", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        // 开始读取列表
        IList<BCW.Model.dawnlifegift> listdawnlifeTop = new BCW.BLL.dawnlifegift().Getdawnlifegifts(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listdawnlifeTop.Count > 0)
        {
            int k = 1;

            foreach (BCW.Model.dawnlifegift n in listdawnlifeTop)
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
                int d = (pageIndex - 1) * 10 + k;
                string sText = string.Empty;

                sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")</a> " + "于" + Convert.ToDateTime(n.date).ToString("yyyy-MM-dd HH:mm:ss") + "捐助了" + n.coin + "酷币";
                builder.AppendFormat("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=juankuan&amp;ptype=" + ptype + "&amp;ptyped=" + ptyped + "&amp;id1=" + ptype + "&amp;id=" + ptyped + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>" + d + sText);
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
        }
        // 分页
        builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0) + "<br />");

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx") + "\">返回" + GameName + "管理</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    //管理员确认更改奖池数据
    protected void GivePage()
    {
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        BCW.Model.dawnlifegift give = new BCW.Model.dawnlifegift();
        int r = new BCW.BLL.dawnlifegift().GetMaxId();
        BCW.Model.dawnlifegift re = new BCW.BLL.dawnlifegift().Getdawnlifegift(r - 1);
        if (uid > 0)
        {
            give.date = DateTime.Now;
            give.coin = 0;
            give.UsID = 1;
            give.UsName = "1";
            give.state = 4;
            give.gift = uid;
            give.giftj = re.giftj;
            new BCW.BLL.dawnlifegift().Add(give);
            Utils.Success("奖池更新成功", "奖池更新成功", Utils.getUrl("DawnlifeManage.aspx?act=gift"), "1");

        }
        else
        {
            Utils.Success("", "请输入大于0的正整数", Utils.getUrl("DawnlifeManage.aspx?act=gift"), "1");
        }

    }
    //自动派奖与奖池更新面页
    private void AwardrPage()
    {
        Master.Title = "管理中心";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx") + "\">" + GameName + "管理</a>&gt;自动派奖与奖池更新管理");
        builder.Append(Out.Tab("</div>", "<br />"));

        //兑奖页面
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("说明：本面页用于奖池自动更新与自动派奖，请管理员请勿随意操作");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<br />", Out.Hr()));
        builder.Append(Out.Tab("</div>", ""));
        string day = DateTime.Now.AddDays(-1).ToString("yyyy年MM月dd日");
        int pageIndex = 0;//当前页
        int recordCount;//记录总条数
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));//分页大小
        string strWhere = "";
        string strOrder = "";
        strWhere = " DateDiff(day,date,getdate())=1";
        strOrder = "date Desc";
        /////////////////////////////////////////////////////////
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("全国前五：");
        builder.Append(Out.Tab("</div>", "<br />"));
        IList<BCW.Model.dawnlifegift> listdawnlifeTop4 = new BCW.BLL.dawnlifegift().Getdawnlifegifts(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listdawnlifeTop4.Count > 0)
        {
            int l = 1;
            foreach (BCW.Model.dawnlifegift n4 in listdawnlifeTop4)
            {
                if (l <= 1)
                {
                    int gift = Convert.ToInt32(n4.gift);
                    //查询条件
                    strWhere = "DateDiff(day,date,getdate())=1";
                    strOrder = "money Desc";
                    strOrder = "money Desc";
                    string[] pageValUrl = { "act", "ptypet", "ptyped", "backurl" };
                    pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                    if (pageIndex == 0)
                        pageIndex = 1;
                    string _time = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                    int _time_num = Convert.ToInt32(_time);
                    //if (_time_num == 0)
                    //{
                        // 开始读取列表
                        IList<BCW.Model.dawnlifeTop> listdawnlifeTop = new BCW.BLL.dawnlifeTop().GetdawnlifeTops(pageIndex, pageSize, strWhere, strOrder, out recordCount);
                        if (listdawnlifeTop.Count > 0)
                        {
                            int k = 1;
                            int j = 1;
                            int h = 1;
                            foreach (BCW.Model.dawnlifeTop n in listdawnlifeTop)
                            {
                                if (j <= 5)
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
                                    int d = (pageIndex - 1) * 10 + k;
                                    int coin = Convert.ToInt16(n.coin);
                                    int money = Convert.ToInt32(n.money);
                                    string sText = string.Empty;
                                    int award = 0;
                                    if (h == 1) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q1 / 100); }
                                    if (h == 2) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q2 / 100); }
                                    if (h == 3) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q3 / 100); }
                                    if (h == 4) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q4 / 100); }
                                    if (h == 5) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q5 / 100); }
                                    if (n.sum == 0 && n.money > 0)
                                    {
                                        int usid = Convert.ToInt32(n.UsID);
                                        BCW.Model.dawnlifeTop update = new BCW.Model.dawnlifeTop();
                                        r = new BCW.BLL.dawnlifeTop().GetRowByUsID(usid, coin, money);
                                        new BCW.BLL.dawnlifeTop().Updatesum(r, 1);
                                        new BCW.BLL.User().UpdateiGold(usid, award, "闯荡" + DateTime.Now.AddDays(-1).ToString("MM月dd日") + "全国榜单兑奖|标识ID" + n.coin + "");
                                        //发内线
                                        string strLog = "根据你上期" + GameName + "全国排行榜上的赢利情况，你的闯荡获得第" + h + "名，闯荡君奖励你" + award + "酷币" + "[url=/bbs/game/Dawnlife.aspx]进入" + GameName + "[/url]";
                                        new BCW.BLL.Guest().Add(0, usid, new BCW.BLL.User().GetUsName(usid), strLog);

                                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "获得第" + h + "名，" + "奖励" + award + "酷币:" + "派奖";
                                    }
                                    else
                                    {
                                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "获得第" + h + "名，" + "奖励" + award + "酷币:" + "已派奖";
                                    }

                                    builder.AppendFormat("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=awardr&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>" + d + sText);
                                    k++;
                                    h++;
                                    builder.Append(Out.Tab("</div>", ""));
                                }
                                else
                                { ;  }
                                j++;
                            }
                        }
                        else
                        {
                            builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
                        }
                    //}
                    //else
                    //{
                    //    string day1 = DateTime.Now.AddDays(0).ToString("yyyy年MM月dd日");
                    //    builder.Append("" + day + "获奖名单已经全部奖励，请前去游戏领奖查看" + "<br />");
                    //    builder.Append("等待" + day1 + "奖励时间开启中...");
                    //}
                }
                else
                {
                    ;
                }
                l++;
            }
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
        }
        builder.Append(Out.Tab("<br />", Out.Hr()));
        ///////////////////////////////////////////////////////////////////
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("广州地区前五：");
        builder.Append(Out.Tab("</div>", "<br />"));
        strWhere = " DateDiff(day,date,getdate())=1";
        strOrder = "date Desc";
        IList<BCW.Model.dawnlifegift> listdawnlifeTop1 = new BCW.BLL.dawnlifegift().Getdawnlifegifts(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listdawnlifeTop1.Count > 0)
        {
            int l = 1;
            foreach (BCW.Model.dawnlifegift n1 in listdawnlifeTop1)
            {
                if (l <= 1)
                {
                    int gift = Convert.ToInt32(n1.gift);
                    //查询条件
                    strWhere = "DateDiff(day,date,getdate())=1 and city ='1' ";
                    strOrder = "money Desc";

                    string[] pageValUrl = { "act", "ptypet", "ptyped", "backurl" };
                    pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                    if (pageIndex == 0)
                        pageIndex = 1;
                    string _time = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                    int _time_num = Convert.ToInt32(_time);
                    //if (_time_num == 0)
                    //{
                        // 开始读取列表
                        IList<BCW.Model.dawnlifeTop> listdawnlifeTop11 = new BCW.BLL.dawnlifeTop().GetdawnlifeTops(pageIndex, pageSize, strWhere, strOrder, out recordCount);
                        if (listdawnlifeTop11.Count > 0)
                        {
                            int k = 1;
                            int j = 1;
                            int h = 1;
                            foreach (BCW.Model.dawnlifeTop n in listdawnlifeTop11)
                            {
                                if (j <= 5)
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
                                    int d = (pageIndex - 1) * 10 + k;
                                    int coin = Convert.ToInt16(n.coin);
                                    int money = Convert.ToInt32(n.money);
                                    string sText = string.Empty;
                                    int award = 0;
                                    if (h == 1) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 2) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 3) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 4) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 5) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (n.cum == 0 && n.money > 0)
                                    {
                                        int usid = Convert.ToInt32(n.UsID);
                                        BCW.Model.dawnlifeTop update = new BCW.Model.dawnlifeTop();
                                        r = new BCW.BLL.dawnlifeTop().GetRowByUsID(usid, coin, money);
                                        new BCW.BLL.dawnlifeTop().Updatecum(r, 1);
                                        new BCW.BLL.User().UpdateiGold(usid, award, "闯荡" + DateTime.Now.AddDays(-1).ToString("MM月dd日") + "广州榜单兑奖|标识ID" + n.coin + "");
                                        //发内线
                                        string strLog = "根据你上期" + GameName + "广州排行榜上的赢利情况，你的闯荡获得第" + h + "名，闯荡君奖励你" + award + "酷币" + "[url=/bbs/game/Dawnlife.aspx]进入" + GameName + "[/url]";
                                        new BCW.BLL.Guest().Add(0, usid, new BCW.BLL.User().GetUsName(usid), strLog);

                                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "获得第" + h + "名，" + "奖励" + award + "酷币:" + "派奖";
                                    }
                                    else
                                    {
                                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "获得第" + h + "名，" + "奖励" + award + "酷币:" + "已派奖";
                                    }

                                    builder.AppendFormat("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=awardr&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>" + d + sText);
                                    k++;
                                    h++;
                                    builder.Append(Out.Tab("</div>", ""));
                                }
                                else
                                { ;  }
                                j++;
                            }
                        }
                        else
                        {
                            builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
                        }
                    //}
                    //else
                    //{
                    //    string day1 = DateTime.Now.AddDays(0).ToString("yyyy年MM月dd日");
                    //    builder.Append("" + day + "获奖名单已经全部奖励，请前去游戏领奖查看" + "<br />");
                    //    builder.Append("等待" + day1 + "奖励时间开启中...");
                    //}
                }
                else
                {
                    ;
                }
                l++;
            }
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
        }
        builder.Append(Out.Tab("<br />", Out.Hr()));

        ///////////////////////////////////////////////////////////////////
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("北京地区前五：");
        builder.Append(Out.Tab("</div>", "<br />"));
        strWhere = " DateDiff(day,date,getdate())=1";
        strOrder = "date Desc";
        IList<BCW.Model.dawnlifegift> listdawnlifeTop2 = new BCW.BLL.dawnlifegift().Getdawnlifegifts(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listdawnlifeTop2.Count > 0)
        {
            int l = 1;
            foreach (BCW.Model.dawnlifegift n1 in listdawnlifeTop2)
            {
                if (l <= 1)
                {
                    int gift = Convert.ToInt32(n1.gift);
                    //查询条件
                    strWhere = "DateDiff(day,date,getdate())=1 and city ='2' ";
                    strOrder = "money Desc";

                    string[] pageValUrl = { "act", "ptypet", "ptyped", "backurl" };
                    pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                    if (pageIndex == 0)
                        pageIndex = 1;
                    string _time = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                    int _time_num = Convert.ToInt32(_time);
                    //if (_time_num == 0)
                    //{
                        // 开始读取列表
                        IList<BCW.Model.dawnlifeTop> listdawnlifeTop11 = new BCW.BLL.dawnlifeTop().GetdawnlifeTops(pageIndex, pageSize, strWhere, strOrder, out recordCount);
                        if (listdawnlifeTop11.Count > 0)
                        {
                            int k = 1;
                            int j = 1;
                            int h = 1;
                            foreach (BCW.Model.dawnlifeTop n in listdawnlifeTop11)
                            {
                                if (j <= 5)
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
                                    int d = (pageIndex - 1) * 10 + k;
                                    int coin = Convert.ToInt16(n.coin);
                                    int money = Convert.ToInt32(n.money);
                                    string sText = string.Empty;
                                    int award = 0;
                                    if (h == 1) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 2) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 3) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 4) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 5) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (n.cum == 0 && n.money > 0)
                                    {
                                        int usid = Convert.ToInt32(n.UsID);
                                        BCW.Model.dawnlifeTop update = new BCW.Model.dawnlifeTop();
                                        r = new BCW.BLL.dawnlifeTop().GetRowByUsID(usid, coin, money);
                                        new BCW.BLL.dawnlifeTop().Updatecum(r, 1);
                                        new BCW.BLL.User().UpdateiGold(usid, award, "闯荡" + DateTime.Now.AddDays(-1).ToString("MM月dd日") + "北京榜单兑奖|标识ID" + n.coin + "");
                                        //发内线
                                        string strLog = "根据你上期" + GameName + "北京排行榜上的赢利情况，你的闯荡获得第" + h + "名，闯荡君奖励你" + award + "酷币" + "[url=/bbs/game/Dawnlife.aspx]进入" + GameName + "[/url]";
                                        new BCW.BLL.Guest().Add(0, usid, new BCW.BLL.User().GetUsName(usid), strLog);

                                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "获得第" + h + "名，" + "奖励" + award + "酷币:" + "派奖";
                                    }
                                    else
                                    {
                                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "获得第" + h + "名，" + "奖励" + award + "酷币:" + "已派奖";
                                    }

                                    builder.AppendFormat("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=awardr&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>" + d + sText);
                                    k++;
                                    h++;
                                    builder.Append(Out.Tab("</div>", ""));
                                }
                                else
                                { ;  }
                                j++;
                            }
                        }
                        else
                        {
                            builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
                        }
                    //}
                    //else
                    //{
                    //    string day1 = DateTime.Now.AddDays(0).ToString("yyyy年MM月dd日");
                    //    builder.Append("" + day + "获奖名单已经全部奖励，请前去游戏领奖查看" + "<br />");
                    //    builder.Append("等待" + day1 + "奖励时间开启中...");
                    //}
                }
                else
                {
                    ;
                }
                l++;
            }
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
        }
        builder.Append(Out.Tab("<br />", Out.Hr()));

        ///////////////////////////////////////////////////////////////////
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("上海地区前五：");
        builder.Append(Out.Tab("</div>", "<br />"));
        strWhere = " DateDiff(day,date,getdate())=1";
        strOrder = "date Desc";
        IList<BCW.Model.dawnlifegift> listdawnlifeTop3 = new BCW.BLL.dawnlifegift().Getdawnlifegifts(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listdawnlifeTop3.Count > 0)
        {
            int l = 1;
            foreach (BCW.Model.dawnlifegift n1 in listdawnlifeTop3)
            {
                if (l <= 1)
                {
                    int gift = Convert.ToInt32(n1.gift);
                    //查询条件
                    strWhere = "DateDiff(day,date,getdate())=1 and city ='3' ";
                    strOrder = "money Desc";

                    string[] pageValUrl = { "act", "ptypet", "ptyped", "backurl" };
                    pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                    if (pageIndex == 0)
                        pageIndex = 1;
                    string _time = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                    int _time_num = Convert.ToInt32(_time);
                    //if (_time_num == 0)
                    //{
                        // 开始读取列表
                        IList<BCW.Model.dawnlifeTop> listdawnlifeTop11 = new BCW.BLL.dawnlifeTop().GetdawnlifeTops(pageIndex, pageSize, strWhere, strOrder, out recordCount);
                        if (listdawnlifeTop11.Count > 0)
                        {
                            int k = 1;
                            int j = 1;
                            int h = 1;
                            foreach (BCW.Model.dawnlifeTop n in listdawnlifeTop11)
                            {
                                if (j <= 5)
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
                                    int d = (pageIndex - 1) * 10 + k;
                                    int coin = Convert.ToInt16(n.coin);
                                    int money = Convert.ToInt32(n.money);
                                    string sText = string.Empty;
                                    int award = 0;
                                    if (h == 1) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 2) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 3) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 4) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 5) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (n.cum == 0 && n.money > 0)
                                    {
                                        int usid = Convert.ToInt32(n.UsID);
                                        BCW.Model.dawnlifeTop update = new BCW.Model.dawnlifeTop();
                                        r = new BCW.BLL.dawnlifeTop().GetRowByUsID(usid, coin, money);
                                        new BCW.BLL.dawnlifeTop().Updatecum(r, 1);
                                        new BCW.BLL.User().UpdateiGold(usid, award, "闯荡" + DateTime.Now.AddDays(-1).ToString("MM月dd日") + "上海榜单兑奖|标识ID" + n.coin + "");
                                        //发内线
                                        string strLog = "根据你上期" + GameName + "上海排行榜上的赢利情况，你的闯荡获得第" + h + "名，闯荡君奖励你" + award + "酷币" + "[url=/bbs/game/Dawnlife.aspx]进入" + GameName + "[/url]";
                                        new BCW.BLL.Guest().Add(0, usid, new BCW.BLL.User().GetUsName(usid), strLog);

                                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "获得第" + h + "名，" + "奖励" + award + "酷币:" + "派奖";
                                    }
                                    else
                                    {
                                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "获得第" + h + "名，" + "奖励" + award + "酷币:" + "已派奖";
                                    }

                                    builder.AppendFormat("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=awardr&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>" + d + sText);
                                    k++;
                                    h++;
                                    builder.Append(Out.Tab("</div>", ""));
                                }
                                else
                                { ;  }
                                j++;
                            }
                        }
                        else
                        {
                            builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
                        }
                    //}
                    //else
                    //{
                    //    string day1 = DateTime.Now.AddDays(0).ToString("yyyy年MM月dd日");
                    //    builder.Append("" + day + "获奖名单已经全部奖励，请前去游戏领奖查看" + "<br />");
                    //    builder.Append("等待" + day1 + "奖励时间开启中...");
                    //}
                }
                else
                {
                    ;
                }
                l++;
            }
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
        }
        builder.Append(Out.Tab("<br />", Out.Hr()));


        ////////////////////////////////


        BCW.Model.dawnlifegift give = new BCW.Model.dawnlifegift();
        r = new BCW.BLL.dawnlifegift().GetMaxId();
        BCW.Model.dawnlifegift re = new BCW.BLL.dawnlifegift().Getdawnlifegift(r - 1);
        int gift2 = Convert.ToInt32(re.gift * jcx / 100) + Convert.ToInt32(re.giftj * juankuan / 100);
        string _time3 = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
        int _time_num3 = Convert.ToInt32(_time3);
        //if (_time_num3 == 0)
        //{
            int coin1 = Convert.ToInt32(re.giftj * juankuan / 100);
            string un = coin1.ToString();
            give.date = DateTime.Now;
            give.gift = gift2;
            give.giftj = Convert.ToInt32(re.giftj * (1 - (juankuan / 100)));
            give.UsID = 1;
            give.UsName = un;
            give.coin = Convert.ToInt32(re.gift * jcd / 100);
            give.state = 3;
            new BCW.BLL.dawnlifegift().Add(give);
    //}

            Utils.Success("更新", "完成奖池更新与派奖，返回派奖管理", Utils.getUrl("DawnlifeManage.aspx?act=award"), "2");
    }
    //派奖管理
    protected void AwardPage()
    {
        Master.Title = "管理中心";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx") + "\">" + GameName + "管理</a>&gt;派奖管理");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("特别说明：" + "<br />" + "具体奖励规则请查看获奖细则，此操作为管理员对前两日获奖酷友进行派奖，" + "<br />" + "当游戏维护或者自动派奖没有执行时使得前两日获奖的酷友不能自己领奖时的管理员派奖操作，" + "<br />" + "且派奖酷币由奖池与获奖规则规定，管理员只能操作酷币的发放!");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("【注意】：请不要擅自给获奖酷友派奖，派奖时自动给获奖酷友发内线");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("" + "-----------" + "<br />");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\""+Utils.getUrl("DawnlifeManage.aspx?act=hj&amp;backurl="+Utils.getPage(1)+"")+"\">查看具体某一天获奖情况</a>");
        builder.Append(Out.Tab("</div>", "<br />"));


        builder.Append(Out.Tab("<div>", ""));
        builder.Append("" + "-----------" + "<br />");
        builder.Append(Out.Tab("</div>", ""));
        DataSet ds = new BCW.BLL.dawnlifegift().GetList(" *", "DateDiff(day,date,getdate())=0 and state='3'");
        if (ds.Tables[0].Rows.Count> 0)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<b style=\"color:red\">昨日奖池已更新，完成派奖</b>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=awardr") + "\">确认手动派奖并更新奖池</a>");
            builder.Append("<br /><b style=\"color:red\">【说明】此操作为管理员当系统自动派奖出错时的手动操作，操作上一日派奖，可完成奖池更新和派奖功能</b>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("" + "-----------" + "<br />");
        builder.Append(Out.Tab("</div>", ""));

        //兑奖页面
        builder.Append(Out.Tab("<div>", ""));
        string day = DateTime.Now.AddDays(-1).ToString("yyyy年MM月dd日");
        builder.Append("" + day + "获奖名单：" + "<br />");
        builder.Append(Out.Tab("</div>", ""));
        int ptypet = Utils.ParseInt(Utils.GetRequest("ptypet", "get", 1, @"^[0-4]$", "4"));
        int ptyped = Utils.ParseInt(Utils.GetRequest("ptyped", "get", 1, @"^[0-4]$", "4"));
        builder.Append(Out.Tab("<div>", ""));
        if (ptypet == 4)
            builder.Append("<b style=\"color:black\">全国" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=award&amp;ptypet=4&amp;id=" + ptypet + "&amp;ptyped=" + ptyped + "&amp;backurl=" + Utils.getPage(0) + "") + "\">全国</a>" + "|");
        if (ptypet == 1)
            builder.Append("<b style=\"color:black\">广州" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=award&amp;ptypet=1&amp;id=" + ptypet + "&amp;ptyped=" + ptyped + "&amp;backurl=" + Utils.getPage(0) + "") + "\">广州</a>" + "|");

        if (ptypet == 2)
            builder.Append("<b style=\"color:black\">北京" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=award&amp;ptypet=2&amp;id=" + ptypet + "&amp;ptyped=" + ptyped + "&amp;backurl=" + Utils.getPage(0) + "") + "\">北京</a>" + "|");
        if (ptypet == 3)
            builder.Append("<b style=\"color:black\">上海" + "</b>" + "");
        else
            builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=award&amp;ptypet=3&amp;id=" + ptypet + "&amp;ptyped=" + ptyped + "&amp;backurl=" + Utils.getPage(0) + "") + "\">上海</a>" + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex = 0;//当前页
        int recordCount;//记录总条数
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));//分页大小
        string strWhere = "";
        string strOrder = "";

        strWhere = " DateDiff(day,date,getdate())=1";
        strOrder = "gift Desc";
        IList<BCW.Model.dawnlifegift> listdawnlifeTop3 = new BCW.BLL.dawnlifegift().Getdawnlifegifts(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listdawnlifeTop3.Count > 0)
        {
            int l = 1;
            foreach (BCW.Model.dawnlifegift n3 in listdawnlifeTop3)
            {
                if (l <= 1)
                {
                    int gift = Convert.ToInt32(n3.gift);
                    //查询条件
                    if (ptypet == 1)
                    {
                        strWhere = "DateDiff(day,date,getdate())=1 and city='1'";
                        strOrder = "money Desc";
                    }
                    if (ptypet == 2)
                    {
                        strWhere = "DateDiff(day,date,getdate())=1 and city='2'";
                        strOrder = "money Desc";
                    }
                    if (ptypet == 3)
                    {
                        strWhere = "DateDiff(day,date,getdate())=1 and city='3'";
                        strOrder = "money Desc";
                    }
                    else if (ptypet == 4)
                        strWhere = "DateDiff(day,date,getdate())=1";
                    strOrder = "money Desc";
                    string[] pageValUrl = { "act", "ptypet", "ptyped", "backurl" };
                    pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                    if (pageIndex == 0)
                        pageIndex = 1;
                    // 开始读取列表
                    IList<BCW.Model.dawnlifeTop> listdawnlifeTop = new BCW.BLL.dawnlifeTop().GetdawnlifeTops(pageIndex, pageSize, strWhere, strOrder, out recordCount);
                    if (listdawnlifeTop.Count > 0)
                    {
                        int k = 1;
                        int j = 1;
                        //int h = 1;
                        int h = Utils.ParseInt(Utils.GetRequest("h", "get", 1, @"^[0-5]$", "1"));
                        foreach (BCW.Model.dawnlifeTop n in listdawnlifeTop)
                        {
                            if (j <= 5)
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
                                int d = (pageIndex - 1) * 10 + k;
                                int coin = Convert.ToInt16(n.coin);
                                int money = Convert.ToInt32(n.money);
                                string sText = string.Empty;

                                if (ptypet == 4)
                                {
                                    int award = 0;
                                    if (h == 1) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q1 / 100); }
                                    if (h == 2) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q2 / 100); }
                                    if (h == 3) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q3 / 100); }
                                    if (h == 4) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q4 / 100); }
                                    if (h == 5) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q5 / 100); }
                                    if (n.sum == 0 && n.money > 0)
                                    {
                                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "获得第" + h + "名，" + "奖励" + award + "酷币:" + "<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=caseok&amp;ptypet=" + ptypet + "&amp;ptyped=" + ptyped + "&amp;pid=" + n.UsID + "&amp;coin=" + coin + "&amp;money=" + money + "&amp;h=" + h + "&amp;award=" + award + "") + "\">派奖</a>";
                                    }
                                    else
                                    {
                                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "获得第" + h + "名，" + "奖励" + award + "酷币:" + "已派奖";
                                    }
                                }
                                else
                                {
                                    int award = 0;
                                    if (h == 1) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 2) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 3) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 4) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 5) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (n.cum == 0 && n.money > 0)
                                    {
                                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "获得第" + h + "名，" + "奖励" + award + "酷币:" + "<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=caseok&amp;ptypet=" + ptypet + "&amp;ptyped=" + ptyped + "&amp;pid=" + n.UsID + "&amp;coin=" + coin + "&amp;money=" + money + "&amp;h=" + h + "&amp;award=" + award + "") + "\">派奖</a>";
                                    }
                                    else
                                    {
                                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "获得第" + h + "名，" + "奖励" + award + "酷币:" + "已派奖";
                                    }
                                }
                                builder.AppendFormat("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=award&amp;id=" + ptypet + "&amp;id2=" + ptyped + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>" + d + sText);
                                k++;
                                h++;
                                builder.Append(Out.Tab("</div>", ""));
                            }
                            else
                            {
                                ;
                            }
                            j++;
                        }
                    }
                    else
                    {
                        builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
                    }

                }
                else
                {
                    ;
                }
                l++;
            }
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
        }
        builder.Append(Out.Tab("<br />", Out.Hr()));

        builder.Append(Out.Tab("<div>",""));
        string day2 = DateTime.Now.AddDays(-2).ToString("yyyy年MM月dd日");
        builder.Append("" + day2 + "获奖名单：" + "<br />");

        if (ptyped == 4)
            builder.Append("<b style=\"color:black\">全国" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=award&amp;ptyped=4&amp;id2=" + ptyped + "&amp;ptypet=" + ptypet + "&amp;backurl=" + Utils.getPage(0) + "") + "\">全国</a>" + "|");
        if (ptyped == 1)
            builder.Append("<b style=\"color:black\">广州" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=award&amp;ptyped=1&amp;id2=" + ptyped + "&amp;ptypet=" + ptypet + "&amp;backurl=" + Utils.getPage(0) + "") + "\">广州</a>" + "|");

        if (ptyped == 2)
            builder.Append("<b style=\"color:black\">北京" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=award&amp;ptyped=2&amp;id2=" + ptyped + "&amp;ptypet=" + ptypet + "&amp;backurl=" + Utils.getPage(0) + "") + "\">北京</a>" + "|");
        if (ptyped == 3)
            builder.Append("<b style=\"color:black\">上海" + "</b>" + "");
        else
            builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=award&amp;ptyped=3&amp;id2=" + ptyped + "&amp;ptypet=" + ptypet + "&amp;backurl=" + Utils.getPage(0) + "") + "\">上海</a>" + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        strWhere = " DateDiff(day,date,getdate())=2";
        strOrder = "gift Desc";
        IList<BCW.Model.dawnlifegift> listdawnlifeTop4 = new BCW.BLL.dawnlifegift().Getdawnlifegifts(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listdawnlifeTop4.Count > 0)
        {
            int l = 1;
            foreach (BCW.Model.dawnlifegift n4 in listdawnlifeTop4)
            {
                if (l <= 1)
                {
                    int gift = Convert.ToInt32(n4.gift);
                    //查询条件
                    if (ptyped == 1)
                    {
                        strWhere = "DateDiff(day,date,getdate())=2 and city='1'";
                        strOrder = "money Desc";
                    }
                    if (ptyped == 2)
                    {
                        strWhere = "DateDiff(day,date,getdate())=2 and city='2'";
                        strOrder = "money Desc";
                    }
                    if (ptyped == 3)
                    {
                        strWhere = "DateDiff(day,date,getdate())=2 and city='3'";
                        strOrder = "money Desc";
                    }
                    else if (ptyped == 4)
                        strWhere = "DateDiff(day,date,getdate())=2";
                    strOrder = "money Desc";
                    string[] pageValUrl = { "act", "ptypet", "ptyped", "backurl" };
                    pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                    if (pageIndex == 0)
                        pageIndex = 1;
                    // 开始读取列表
                    IList<BCW.Model.dawnlifeTop> listdawnlifeTop2 = new BCW.BLL.dawnlifeTop().GetdawnlifeTops(pageIndex, pageSize, strWhere, strOrder, out recordCount);
                    if (listdawnlifeTop2.Count > 0)
                    {
                        //DataSet dg = new BCW.BLL.dawnlifegift().GetList("Top 1 UsID,gift", "DateDiff(day,date,getdate())=1 Order by gift Desc");

                        //int gift = 5;
                        int k = 1;
                        int j = 1;
                        //int h = 1;
                        int h = Utils.ParseInt(Utils.GetRequest("h", "get", 1, @"^[0-5]$", "1"));
                        foreach (BCW.Model.dawnlifeTop n2 in listdawnlifeTop2)
                        {
                            if (j <= 5)
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
                                int d = (pageIndex - 1) * 10 + k;
                                int coin = Convert.ToInt16(n2.coin);
                                int money = Convert.ToInt32(n2.money);
                                string sText = string.Empty;

                                if (ptyped == 4)
                                {
                                    int award = 0;
                                    if (h == 1) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q1 / 100); }
                                    if (h == 2) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q2 / 100); }
                                    if (h == 3) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q3 / 100); }
                                    if (h == 4) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q4 / 100); }
                                    if (h == 5) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q5 / 100); }
                                    if (n2.sum == 0 && n2.money > 0)
                                    {
                                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n2.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n2.UsName + "(" + n2.UsID + ")" + "</a>" + "获得第" + h + "名，" + "奖励" + award + "酷币:" + "<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=caseok1&amp;ptypet=" + ptypet + "&amp;ptyped=" + ptyped + "&amp;pid=" + n2.UsID + "&amp;coin=" + coin + "&amp;money=" + money + "&amp;h=" + h + "&amp;award=" + award + "") + "\">派奖</a>";
                                    }
                                    else
                                    {
                                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n2.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n2.UsName + "(" + n2.UsID + ")" + "</a>" + "获得第" + h + "名，" + "奖励" + award + "酷币:" + "已派奖";
                                    }
                                }
                                else if (ptyped == 1 || ptyped == 2 || ptyped == 3)
                                {
                                    int award = 0;
                                    if (h == 1) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 2) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 3) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 4) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 5) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (n2.cum == 0 && n2.money > 0)
                                    {
                                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n2.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n2.UsName + "(" + n2.UsID + ")" + "</a>" + "获得第" + h + "名，" + "奖励" + award + "酷币:" + "<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=caseok1&amp;ptypet=" + ptypet + "&amp;ptyped=" + ptyped + "&amp;pid=" + n2.UsID + "&amp;coin=" + coin + "&amp;money=" + money + "&amp;h=" + h + "&amp;award=" + award + "") + "\">派奖</a>";
                                    }
                                    else
                                    {
                                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n2.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n2.UsName + "(" + n2.UsID + ")" + "</a>" + "获得第" + h + "名，" + "奖励" + award + "酷币:" + "已派奖";
                                    }
                                }
                                builder.AppendFormat("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=award&amp;id=" + ptypet + "&amp;id2=" + ptyped + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>" + d + sText);
                                k++;
                                h++;
                                builder.Append(Out.Tab("</div>", ""));
                            }
                            else
                            {
                                ;
                            }
                            j++;
                        }
                    }
                    else
                    {
                        builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
                    }

                }
                else
                {
                    ;
                }
                l++;
            }
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
        }

        builder.Append(Out.Tab("<br />", Out.Hr()));
        builder.Append(Out.Tab("<div class=\"\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx") + "\">返回" + GameName + "管理</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    //日期查询获奖情况
    protected void HjPage()
    {
        Master.Title = "管理中心";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx") + "\">" + GameName + "管理</a>&gt;<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=award") + "\">派奖管理</a>&gt;按日期查看获奖情况");
        builder.Append(Out.Tab("</div>", "<br />"));

        string[] pageValUrl = { "act", "ptypet", "ptyped", "searchday", "backurl" };

        builder.Append(Out.Tab("<div>", ""));
        string searchday = (Utils.GetRequest("searchday", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8])))$", ""));
        string strText = "输入查询日期：格式（20160307）/,";
        string strName = "searchday";
        string strType = "num";
        string strValu = searchday;
        string strEmpt = "";
        string strIdea = "";
        string strOthe = "搜获奖记录,DawnlifeManage.aspx?act=hj&amp;ptyped="+strValu+",post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("</div>", "<br />"));

        //兑奖页面
        builder.Append(Out.Tab("<div>", ""));
        if (searchday == "")
        {
            string day = DateTime.Now.AddDays(-1).ToString("yyyy年MM月dd日");
            builder.Append("" + day + "获奖名单：" + "<br />");
        }
        else
        {
            builder.Append("" + (DateTime.ParseExact(searchday, "yyyyMMdd", null).ToString("yyyy年MM月dd日")) + "获奖名单：" + "<br />");
        }

        builder.Append(Out.Tab("</div>", ""));
        int ptypet = Utils.ParseInt(Utils.GetRequest("ptypet", "all", 1, @"^[0-4]$", "4"));
        int pageIndex = 0;//当前页
        int recordCount;//记录总条数
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));//分页大小
        string strWhere1 = string.Empty;
        string strWhere = string.Empty;
        string strOrder = "";

        if (searchday == "")
        {
            strWhere1 = "DateDiff(day,date,getdate())=1";
        }
        else
        {
            strWhere1 = "convert(varchar(10),date,120)='" + (DateTime.ParseExact(searchday, "yyyyMMdd", null).ToString("yyyy-MM-dd")) + "'";
        }
        strOrder = "gift Desc";

        builder.Append(Out.Tab("<div>", ""));
        if (ptypet == 4)
            builder.Append("<b style=\"color:black\">全国" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=hj&amp;ptypet=4&amp;searchday=" + searchday + "&amp;backurl=" + Utils.getPage(0) + "") + "\">全国</a>" + "|");
        if (ptypet == 1)
            builder.Append("<b style=\"color:black\">广州" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=hj&amp;ptypet=1&amp;searchday=" + searchday + "&amp;backurl=" + Utils.getPage(0) + "") + "\">广州</a>" + "|");

        if (ptypet == 2)
            builder.Append("<b style=\"color:black\">北京" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=hj&amp;ptypet=2&amp;searchday=" + searchday + "&amp;backurl=" + Utils.getPage(0) + "") + "\">北京</a>" + "|");
        if (ptypet == 3)
            builder.Append("<b style=\"color:black\">上海" + "</b>" + "");
        else
            builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=hj&amp;ptypet=3&amp;searchday=" + searchday + "&amp;backurl=" + Utils.getPage(0) + "") + "\">上海</a>" + "");
        builder.Append(Out.Tab("</div>", "<br />"));

      
        

        IList<BCW.Model.dawnlifegift> listdawnlifeTop3 = new BCW.BLL.dawnlifegift().Getdawnlifegifts(pageIndex, pageSize, strWhere1, strOrder, out recordCount);
        if (listdawnlifeTop3.Count > 0)
        {
            int l = 1;
            foreach (BCW.Model.dawnlifegift n3 in listdawnlifeTop3)
            {
                if (l <= 1)
                {
                    int gift = Convert.ToInt32(n3.gift);
                    //查询条件
                    if (ptypet == 1)
                    {
                        strWhere =strWhere1+ " and city='1'";
                        strOrder = "money Desc";
                    }
                    if (ptypet == 2)
                    {
                        strWhere =strWhere1 +" and city='2'";
                        strOrder = "money Desc";
                    }
                    if (ptypet == 3)
                    {
                        strWhere = strWhere1 + " and city='3'";
                        strOrder = "money Desc";
                    }
                    else if (ptypet == 4)
                        strWhere = strWhere1 ;
                    strOrder = "money Desc";
                 
                    pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                    if (pageIndex == 0)
                        pageIndex = 1;
                    // 开始读取列表
                    IList<BCW.Model.dawnlifeTop> listdawnlifeTop = new BCW.BLL.dawnlifeTop().GetdawnlifeTops(pageIndex, pageSize, strWhere, strOrder, out recordCount);
                    if (listdawnlifeTop.Count > 0)
                    {
                        int k = 1;
                        int j = 1;
                        //int h = 1;
                        int h = Utils.ParseInt(Utils.GetRequest("h", "get", 1, @"^[0-5]$", "1"));
                        foreach (BCW.Model.dawnlifeTop n in listdawnlifeTop)
                        {
                            if (j <= 5)
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
                                int d = (pageIndex - 1) * 10 + k;
                                int coin = Convert.ToInt16(n.coin);
                                int money = Convert.ToInt32(n.money);
                                string sText = string.Empty;

                                if (ptypet == 4)
                                {
                                    int award = 0;
                                    if (h == 1) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q1 / 100); }
                                    if (h == 2) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q2 / 100); }
                                    if (h == 3) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q3 / 100); }
                                    if (h == 4) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q4 / 100); }
                                    if (h == 5) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q5 / 100); }
                                    if (n.sum == 0 && n.money > 0)
                                    {
                                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "获得第" + h + "名，" + "奖励" + award + "酷币" + "";
                                    }
                                    else
                                    {
                                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "获得第" + h + "名，" + "奖励" + award + "酷币" + "";
                                    }
                                }
                                else
                                {
                                    int award = 0;
                                    if (h == 1) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 2) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 3) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 4) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (h == 5) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                    if (n.cum == 0 && n.money > 0)
                                    {
                                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "获得第" + h + "名，" + "奖励" + award + "酷币";
                                    }
                                    else
                                    {
                                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "获得第" + h + "名，" + "奖励" + award + "酷币" + "";
                                    }
                                }
                                builder.AppendFormat("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=award&amp;ptypet=" + ptypet + "&amp;searchday=" + searchday + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>" + d + sText);
                                k++;
                                h++;
                                builder.Append(Out.Tab("</div>", ""));
                            }
                            else
                            {
                                ;
                            }
                            j++;
                        }
                    }
                    else
                    {
                        builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
                    }

                }
                else
                {
                    ;
                }
                l++;
            }
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
        }
        builder.Append(Out.Tab("<br />", Out.Hr()));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?act=award") + "\">返回派奖管理</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx") + "\">返回" + GameName + "管理</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    //上一日每单领奖
    private void CaseOkPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        //if (meid == 0)
        //    Utils.Login();
        int pid = Utils.ParseInt(Utils.GetRequest("pid", "get", 2, @"^[0-9]\d*$", "选择领奖无效"));
        int coin = Utils.ParseInt(Utils.GetRequest("coin", "get", 2, @"^[0-9]\d*$", ""));
        int money = Utils.ParseInt(Utils.GetRequest("money", "get", 2, @"^[0-9]\d*$", ""));
        int award = Utils.ParseInt(Utils.GetRequest("award", "get", 2, @"^[0-9]\d*$", ""));
        int ptypet = Utils.ParseInt(Utils.GetRequest("ptypet", "get", 1, @"^[0-4]$", "4"));
        int ptyped = Utils.ParseInt(Utils.GetRequest("ptyped", "get", 1, @"^[0-4]$", "4"));
        int h = Utils.ParseInt(Utils.GetRequest("h", "get", 1, @"^[0-5]$", "1"));
        r = new BCW.BLL.dawnlifeTop().GetRowByUsID(pid, coin, money);
        BCW.Model.dawnlifeTop re = new BCW.BLL.dawnlifeTop().GetdawnlifeTop(r);
        int UsID = Convert.ToInt32(re.UsID);
        if (ptyped != 0)
        {
            if (ptypet == 4)
            {
                if (re.sum == 0)
                {
                    BCW.Model.dawnlifeTop update = new BCW.Model.dawnlifeTop();
                    update.sum = 1;
                    new BCW.BLL.dawnlifeTop().Updatesum(r, 1);
                    new BCW.BLL.User().UpdateiGold(UsID, award, "闯荡" + DateTime.Now.AddDays(-1).ToString("MM月dd日") + "全国榜单兑奖|标识ID" + re.coin + "");
                    //发内线
                    string strLog = "根据你上期" + GameName + "全国排行榜上的赢利情况，你的闯荡获得第" + h + "名，闯荡君奖励你" + award + "酷币" + "[url=/bbs/game/Dawnlife.aspx]进入" + GameName + "[/url]";
                    new BCW.BLL.Guest().Add(0, UsID, new BCW.BLL.User().GetUsName(UsID), strLog);
                    Utils.Success("领奖", "成功派奖" + award + "" + ub.Get("SiteBz") + "", Utils.getUrl("DawnlifeManage.aspx?act=award&amp;ptypet=" + ptypet + "&amp;ptyped=" + ptyped + ""), "1");
                }
                else if (re.sum == 1)
                {
                    Utils.Success("领奖", "重复派奖或没有可以派奖的记录", Utils.getUrl("DawnlifeManage.aspx?act=award&amp;ptypet=" + ptypet + "&amp;ptyped=" + ptyped + ""), "1");
                }
            }
            else if (ptypet == 1 || ptypet == 3 || ptypet == 2)
            {
                if (re.cum == 0)
                {
                    BCW.Model.dawnlifeTop update = new BCW.Model.dawnlifeTop();
                    update.cum = 1;
                    new BCW.BLL.dawnlifeTop().Updatecum(r, 1);
                    new BCW.BLL.User().UpdateiGold(UsID, award, "闯荡" + DateTime.Now.AddDays(-1).ToString("MM月dd日") + "地区榜单兑奖|标识ID" + re.coin + "");
                    //发内线
                    if (ptypet == 1)
                    {
                        string strLog = "根据你上期" + GameName + "广州排行榜上的赢利情况，你的闯荡获得第" + h + "名，闯荡君奖励你" + award + "酷币" + "[url=/bbs/game/Dawnlife.aspx]进入" + GameName + "[/url]";
                        new BCW.BLL.Guest().Add(0, UsID, new BCW.BLL.User().GetUsName(UsID), strLog);
                    }
                    if (ptypet == 2)
                    {
                        string strLog = "根据你上期" + GameName + "北京排行榜上的赢利情况，你的闯荡获得第" + h + "名，闯荡君奖励你" + award + "酷币" + "[url=/bbs/game/Dawnlife.aspx]进入" + GameName + "[/url]";
                        new BCW.BLL.Guest().Add(0, UsID, new BCW.BLL.User().GetUsName(UsID), strLog);
                    }
                    if (ptypet == 3)
                    {
                        string strLog = "根据你上期" + GameName + "上海排行榜上的赢利情况，你的闯荡获得第" + h + "名，闯荡君奖励你" + award + "酷币" + "[url=/bbs/game/Dawnlife.aspx]进入" + GameName + "[/url]";
                        new BCW.BLL.Guest().Add(0, UsID, new BCW.BLL.User().GetUsName(UsID), strLog);
                    }

                    Utils.Success("派奖", "成功派奖" + award + "" + ub.Get("SiteBz") + "", Utils.getUrl("DawnlifeManage.aspx?act=award&amp;ptypet=" + ptypet + "&amp;ptyped=" + ptyped + ""), "1");
                }
                else if (re.cum == 1)
                {
                    Utils.Success("派奖", "重复派奖或没有可以派奖的记录", Utils.getUrl("DawnlifeManage.aspx?act=award&amp;ptypet=" + ptypet + "&amp;ptyped=" + ptyped + ""), "1");
                }
            }
        }
        else
        {
            if (ptypet == 4)
            {
                if (re.sum == 0)
                {
                    BCW.Model.dawnlifeTop update = new BCW.Model.dawnlifeTop();
                    update.sum = 1;
                    new BCW.BLL.dawnlifeTop().Updatesum(r, 1);
                    new BCW.BLL.User().UpdateiGold(UsID, award, "闯荡" + DateTime.Now.AddDays(-1).ToString("MM月dd日") + "全国榜单兑奖|标识ID" + re.coin + "");
                    //发内线
                    string strLog = "根据你上期" + GameName + "全国排行榜上的赢利情况，你的闯荡获得第" + h + "名，闯荡君奖励你" + award + "酷币" + "[url=/bbs/game/Dawnlife.aspx]进入" + GameName + "[/url]";
                    new BCW.BLL.Guest().Add(0, UsID, new BCW.BLL.User().GetUsName(UsID), strLog);
                    Utils.Success("派奖", "成功派奖" + award + "" + ub.Get("SiteBz") + "", Utils.getUrl("DawnlifeManage.aspx?act=award&amp;ptypet=" + ptypet + ""), "1");
                }
                else if (re.sum == 1)
                {
                    Utils.Success("派奖", "重复派奖或没有可以派奖的记录", Utils.getUrl("DawnlifeManage.aspx?act=award&amp;ptypet=" + ptypet + ""), "1");
                }
            }
            else if (ptypet == 1 || ptypet == 3 || ptypet == 2)
            {
                if (re.cum == 0)
                {
                    BCW.Model.dawnlifeTop update = new BCW.Model.dawnlifeTop();
                    update.cum = 1;
                    new BCW.BLL.dawnlifeTop().Updatecum(r, 1);
                    new BCW.BLL.User().UpdateiGold(UsID, award, "闯荡" + DateTime.Now.AddDays(-1).ToString("MM月dd日") + "地区榜单兑奖|标识ID" + re.coin + "");
                    //发内线
                    if (ptypet == 1)
                    {
                        string strLog = "根据你上期" + GameName + "广州排行榜上的赢利情况，你的闯荡获得第" + h + "名，闯荡君奖励你" + award + "酷币" + "[url=/bbs/game/Dawnlife.aspx]进入" + GameName + "[/url]";
                        new BCW.BLL.Guest().Add(0, UsID, new BCW.BLL.User().GetUsName(UsID), strLog);
                    }
                    if (ptypet == 2)
                    {
                        string strLog = "根据你上期" + GameName + "北京排行榜上的赢利情况，你的闯荡获得第" + h + "名，闯荡君奖励你" + award + "酷币" + "[url=/bbs/game/Dawnlife.aspx]进入" + GameName + "[/url]";
                        new BCW.BLL.Guest().Add(0, UsID, new BCW.BLL.User().GetUsName(UsID), strLog);
                    }
                    if (ptypet == 3)
                    {
                        string strLog = "根据你上期" + GameName + "上海排行榜上的赢利情况，你的闯荡获得第" + h + "名，闯荡君奖励你" + award + "酷币" + "[url=/bbs/game/Dawnlife.aspx]进入" + GameName + "[/url]";
                        new BCW.BLL.Guest().Add(0, UsID, new BCW.BLL.User().GetUsName(UsID), strLog);
                    }
                    Utils.Success("派奖", "成功派奖" + award + "" + ub.Get("SiteBz") + "", Utils.getUrl("DawnlifeManage.aspx?act=award&amp;ptypet=" + ptypet + ""), "1");
                }
                else if (re.cum == 1)
                {
                    Utils.Success("派奖", "重复派奖或没有可以派奖的记录", Utils.getUrl("DawnlifeManage.aspx?act=award&amp;ptypet=" + ptypet + ""), "1");
                }
            }
        }
    }
    //每单领奖前日
    private void CaseOk1Page()
    {
        int meid = new BCW.User.Users().GetUsId();
        //if (meid == 0)
        //    Utils.Login();
        int pid = Utils.ParseInt(Utils.GetRequest("pid", "get", 2, @"^[0-9]\d*$", "选择领奖无效"));
        int coin = Utils.ParseInt(Utils.GetRequest("coin", "get", 2, @"^[0-9]\d*$", ""));
        int money = Utils.ParseInt(Utils.GetRequest("money", "get", 2, @"^[0-9]\d*$", ""));
        int award = Utils.ParseInt(Utils.GetRequest("award", "get", 2, @"^[0-9]\d*$", ""));
        int ptypet = Utils.ParseInt(Utils.GetRequest("ptypet", "get", 1, @"^[0-4]$", "4"));
        int ptyped = Utils.ParseInt(Utils.GetRequest("ptyped", "get", 1, @"^[0-4]$", "4"));
        int h = Utils.ParseInt(Utils.GetRequest("h", "get", 1, @"^[0-5]$", "1"));
        r = new BCW.BLL.dawnlifeTop().GetRowByUsID(pid, coin, money);
        BCW.Model.dawnlifeTop re = new BCW.BLL.dawnlifeTop().GetdawnlifeTop(r);
        int UsID = Convert.ToInt32(re.UsID);
        if (ptyped == 4)
        {
            if (re.sum == 0)
            {
                BCW.Model.dawnlifeTop update = new BCW.Model.dawnlifeTop();
                update.sum = 1;
                new BCW.BLL.dawnlifeTop().Updatesum(r, 1);
                new BCW.BLL.User().UpdateiGold(UsID, award, "闯荡" + DateTime.Now.AddDays(-2).ToString("MM月dd日") + "全国榜单兑奖|标识ID" + re.coin + "");
                //发内线
                string strLog = "根据你上期" + GameName + "全国排行榜上的赢利情况，你的闯荡获得第" + h + "名，闯荡君奖励你" + award + "酷币" + "[url=/bbs/game/Dawnlife.aspx]进入" + GameName + "[/url]";
                new BCW.BLL.Guest().Add(0, UsID, new BCW.BLL.User().GetUsName(UsID), strLog);
                Utils.Success("派奖", "成功派奖" + award + "" + ub.Get("SiteBz") + "", Utils.getUrl("DawnlifeManage.aspx?act=award&amp;ptyped=" + ptyped + "&amp;ptypet=" + ptypet + ""), "1");
            }
            else if (re.sum == 1)
            {
                Utils.Success("派奖", "没有可以派奖的记录", Utils.getUrl("DawnlifeManage.aspx?act=award&amp;ptyped=" + ptyped + "&amp;ptypet=" + ptypet + ""), "1");
            }
        }
        else if (ptyped == 1 || ptyped == 2 || ptyped == 3)
        {
            if (re.cum == 0)
            {
                BCW.Model.dawnlifeTop update = new BCW.Model.dawnlifeTop();
                update.cum = 1;
                new BCW.BLL.dawnlifeTop().Updatecum(r, 1);
                new BCW.BLL.User().UpdateiGold(UsID, award, "闯荡" + DateTime.Now.AddDays(-2).ToString("MM月dd日") + "地区榜单兑奖|标识ID" + re.coin + "");
                //发内线
                if (ptypet == 1)
                {
                    string strLog = "根据你上期" + GameName + "广州排行榜上的赢利情况，你的闯荡获得第" + h + "名，闯荡君奖励你" + award + "酷币" + "[url=/bbs/game/Dawnlife.aspx]进入" + GameName + "[/url]";
                    new BCW.BLL.Guest().Add(0, UsID, new BCW.BLL.User().GetUsName(UsID), strLog);
                }
                if (ptypet == 2)
                {
                    string strLog = "根据你上期" + GameName + "北京排行榜上的赢利情况，你的闯荡获得第" + h + "名，闯荡君奖励你" + award + "酷币" + "[url=/bbs/game/Dawnlife.aspx]进入" + GameName + "[/url]";
                    new BCW.BLL.Guest().Add(0, UsID, new BCW.BLL.User().GetUsName(UsID), strLog);
                }
                if (ptypet == 3)
                {
                    string strLog = "根据你上期" + GameName + "上海排行榜上的赢利情况，你的闯荡获得第" + h + "名，闯荡君奖励你" + award + "酷币" + "[url=/bbs/game/Dawnlife.aspx]进入" + GameName + "[/url]";
                    new BCW.BLL.Guest().Add(0, UsID, new BCW.BLL.User().GetUsName(UsID), strLog);
                }
                Utils.Success("派奖", "成功派奖" + award + "" + ub.Get("SiteBz") + "", Utils.getUrl("DawnlifeManage.aspx?act=award&amp;ptyped=" + ptyped + "&amp;ptypet=" + ptypet + ""), "1");
            }
            else if (re.cum == 1)
            {
                Utils.Success("派奖", "没有可以派奖的记录", Utils.getUrl("DawnlifeManage.aspx?act=award&amp;ptyped=" + ptyped + "&amp;ptypet=" + ptypet + ""), "1");
            }
        }


    }
    //盈利分析
    protected void ProfitPage()
    {
        Master.Title = "管理中心";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx") + "\">" + GameName + "管理</a>&gt;盈利分析");
        builder.Append(Out.Tab("</div>", ""));
        //今天投入酷币
        long todaycoin = new BCW.BLL.dawnlifegift().GetPrice("coin", "DateDiff(day,date,getdate())=0");
        //昨天投入
        long yesterdaycoin = new BCW.BLL.dawnlifegift().GetPrice("coin", "DateDiff(day,date,getdate()-1)=0");
        //本月投入
        long monthcoin = new BCW.BLL.dawnlifegift().GetPrice("coin", "DateDiff(month,date,getdate())=0");
        //上月投入
        long lastmonthcoin = new BCW.BLL.dawnlifegift().GetPrice("coin", "DateDiff(month,date,getdate())=1");
        //今年投入
        long yearcoin = new BCW.BLL.dawnlifegift().GetPrice("coin", "DateDiff(YEAR,date,getdate())=0");
        //总投入
        long allcoin = new BCW.BLL.dawnlifegift().GetPrice("coin", "state>=0");
        builder.Append(Out.Tab("", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("今天赢利：" + todaycoin * 0.2 + ub.Get("SiteBz"));
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("昨天赢利：" + yesterdaycoin * 0.2 + ub.Get("SiteBz"));
        builder.Append(Out.Tab("</div>", Out.Hr()));
        builder.Append("<hr/>");

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("本月赢利：" + monthcoin * 0.2 + ub.Get("SiteBz"));
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("上月赢利：" + lastmonthcoin * 0.2 + ub.Get("SiteBz"));
        builder.Append(Out.Tab("</div>", Out.Hr()));
        builder.Append("<hr/>");

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("今年赢利：" + yearcoin * 0.2 + ub.Get("SiteBz"));
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("总赢利：" + allcoin * 0.2 + ub.Get("SiteBz"));
        builder.Append(Out.Tab("</div>", Out.Hr()));
        builder.Append("<hr/>");

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        if (Utils.ToSChinese(ac) == "马上查询")
        {
            string searchday1 = string.Empty;
            string searchday2 = string.Empty;
            searchday1 = Utils.GetRequest("sTime", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8])))$", DateTime.Now.ToString("yyyyMMdd"));
            searchday2 = Utils.GetRequest("oTime", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8])))$", DateTime.Now.ToString("yyyyMMdd"));

            if (searchday1 == "")
            {
                searchday1 = DateTime.Now.ToString("yyyyMMdd");
            }
            if (searchday2 == "")
            {
                searchday2 = DateTime.Now.AddDays(1).ToString("yyyyMMdd");
            }

            long coin = new BCW.BLL.dawnlifegift().GetPrice("coin", "date>='" + (DateTime.ParseExact(searchday1, "yyyyMMdd", null).ToString("yyyy-MM-dd")) + "' and date<='" + (DateTime.ParseExact(searchday2, "yyyyMMdd", null).AddDays(1).ToString("yyyy-MM-dd")) + "'AND state>='0'");

            builder.Append(Out.Tab("<div>", ""));
            builder.Append(">>>盈利" + coin * 0.2 + "酷币&lt;&lt;&lt;");
            string strText = "开始日期：,结束日期：,";
            string strName = "sTime,oTime,act";
            string strType = "date,date,hidden";
            string strValu = "" + searchday1 + "'" + searchday2 + "'''backsave";
            string strEmpt = "false,false,false";
            string strIdea = "/";
            string strOthe = "马上查询,DawnlifeManage.aspx?act=profit,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(">>>盈利 0 酷币&lt;&lt;&lt;");
            string strText = "开始日期：,结束日期：,";
            string strName = "sTime,oTime,act";
            string strType = "date,date,hidden";
            string strValu = "" + DateTime.Now.ToString("yyyyMMdd") + "'" + DateTime.Now.ToString("yyyyMMdd") + "'''backsave";
            string strEmpt = "false,false,false";
            string strIdea = "/";
            string strOthe = "马上查询,DawnlifeManage.aspx?act=profit,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        builder.Append(Out.Tab("</div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx") + "\">返回" + GameName + "管理</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    //游戏配置
    protected void WeihuPage()
    {
        Master.Title = "" + GameName + "管理中心";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx") + "\">" + GameName + "管理</a>&gt;游戏配置");
        builder.Append(Out.Tab("</div>", "<br />"));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/Dawnlife.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置

        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string Name = Utils.GetRequest("Name", "post", 2, @"^[^\^]{1,20}$", "系统名称限1-20字内");
            string Notes = Utils.GetRequest("Notes", "post", 3, @"^[^\^]{1,2000}$", "口号限2000字内");
            string Logo = Utils.GetRequest("Logo", "post", 3, @"^[^\^]{1,200}$", "Logo地址限200字内");
            string Status = Utils.GetRequest("Status", "post", 2, @"^[0-1]$", "系统状态选择出错");
            string SmallPay = Utils.GetRequest("SmallPay", "post", 4, @"^[0-9]\d*$", "闯荡游戏每次游戏酷币填写错误");
            string Head = Utils.GetRequest("Head", "post", 3, @"^[\s\S]{1,2000}$", "头部Ubb限2000字内");
            string Foot = Utils.GetRequest("Foot", "post", 3, @"^[\s\S]{1,2000}$", "底部Ubb限2000字内");

            xml.dss["DawnlifeName"] = Name;
            xml.dss["DawnlifeNotes"] = Notes;
            xml.dss["DawnlifeLogo"] = Logo;
            xml.dss["DawnlifeStatus"] = Status;
            xml.dss["DawnlifePay"] = SmallPay;
            xml.dss["DawnlifeHead"] = Head;
            xml.dss["DawnlifeFoot"] = Foot;

            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("" + GameName + "游戏设置", "设置成功，正在返回..", Utils.getUrl("DawnlifeManage.aspx?act=weihu&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            string strText = "游戏名称:/,游戏口号(可留空):/,游戏Logo(可留空):/,游戏状态:/," + GameName + "每次游戏花费酷币:/,头部Ubb:/,底部Ubb:/,";
            string strName = "Name,Notes,Logo,Status,SmallPay,Head,Foot,backurl";
            string strType = "text,text,text,select,num,text,text,hidden";
            string strValu = "" + xml.dss["DawnlifeName"] + "'" + xml.dss["DawnlifeNotes"] + "'" + xml.dss["DawnlifeLogo"] + "'" + xml.dss["DawnlifeStatus"] + "'" + xml.dss["DawnlifePay"] + "'" + xml.dss["DawnlifeHead"] + "'" + xml.dss["DawnlifeFoot"] + "'" + Utils.getPage(0) + "";
            string strEmpt = "false,true,true,0|正常|1|维护,false,true,true,false";
            string strIdea = "/";
            string strOthe = "确定修改|reset,DawnlifeManage.aspx?act=weihu,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));


            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("温馨提示:每次进入游戏所花费酷币默认为100酷币，管理员可根据情况慎重更改每次游戏所需花费酷币");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx") + "\">" + GameName + "管理中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
    //奖励配置
    protected void PeizhiPage()
    {
        Master.Title = "" + GameName + "管理中心";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx") + "\">" + GameName + "管理</a>&gt;奖励配置");
        builder.Append(Out.Tab("</div>", "<br />"));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/Dawnlife.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置

        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string jcd = Utils.GetRequest("jcd", "all", 4, @"^[0-9]\d*$", "当日奖池奖励填写出错");
            string jcx = Utils.GetRequest("jcx", "all", 4, @"^[0-9]\d*$", "下日奖池填写出错");
            string drq = Utils.GetRequest("drq", "all", 4, @"^[0-9]\d*$", "全国榜奖励填写出错");
            string drd = Utils.GetRequest("drd", "all", 4, @"^[0-9]\d*$", "地区奖励填写出错");
            string q1 = Utils.GetRequest("q1", "all", 4, @"^[0-9]\d*$", "全国榜第一奖励填写出错");
            string q2 = Utils.GetRequest("q2", "all", 4, @"^[0-9]\d*$", "全国榜第二奖励填写出错");
            string q3 = Utils.GetRequest("q3", "all", 4, @"^[0-9]\d*$", "全国榜第三奖励填写出错");
            string q4 = Utils.GetRequest("q4", "all", 4, @"^[0-9]\d*$", "全国榜第四奖励填写出错");
            string q5 = Utils.GetRequest("q5", "all", 4, @"^[0-9]\d*$", "全国榜第五奖励填写出错");
            string juankuan = Utils.GetRequest("juankuan", "all", 4, @"^[0-9]\d*$", "捐款奖池多少存入下一日奖池填写出错");
         

            xml.dss["jcd"] = jcd;
            xml.dss["jcx"] = jcx;
            xml.dss["drq"] = drq;
            xml.dss["drd"] = drd;
            xml.dss["juankuan"] = juankuan;
            xml.dss["q1"] = q1;
            xml.dss["q2"] = q2;
            xml.dss["q3"] = q3;
            xml.dss["q4"] = q4;
            xml.dss["q5"] = q5;

            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("" + GameName + "游戏设置", "设置成功，正在返回..", Utils.getUrl("DawnlifeManage.aspx?act=peizhi&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            string strText = "当日奖池奖励（%）:/,下一日奖池（%）:/,全国榜奖励（%）:/,地区榜奖励（%）:/,全国榜第一奖励（%）:/,全国榜第二奖励（%）:/,全国榜第三奖励（%）:/,全国榜第四奖励（%）:/,全国榜第五奖励（%）:/,捐款奖池多少存入下一日奖池（%）:/,";
            string strName = "jcd,jcx,drq,drd,q1,q2,q3,q4,q5,juankuan";
            string strType = "num,num,num,num,num,num,num,num,num,num";
            string strValu = "" + xml.dss["jcd"] + "'" + xml.dss["jcx"] + "'" + xml.dss["drq"] + "'" + xml.dss["drd"] + "'" + xml.dss["q1"] + "'" + xml.dss["q2"] + "'" + xml.dss["q3"] + "'" + xml.dss["q4"] + "'" + xml.dss["q5"] + "'" + xml.dss["juankuan"] + "'" + Utils.getPage(0) + "";
            string strEmpt = "false,false,false,false,false,false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定修改|reset,DawnlifeManage.aspx?act=peizhi,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));


            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("温馨提示:<br />当日奖池奖励与下一日奖池的百分比和必须为100%"+"<br />"+"全国榜奖励与地区榜奖励的百分比和必须为100%"+"<br />"+"全国第一、二、三、四、五奖励百分比和必须为100%"+"<br />"+"捐款奖池多少存入下一日独立存在，可根据老板要求填写百分比");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx") + "\">" + GameName + "管理中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
    //游戏内测管理
    //测试设置测试状态
    private void SetStatueCeshi()
    {
        Master.Title = "" + GameName + "设置测试状态";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx") + "\">" + GameName + "管理</a>&gt;");
        builder.Append("" + GameName + "设置测试状态");
        builder.Append(Out.Tab("</div>", ""));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/Dawnlife.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string ceshi = Utils.GetRequest("ceshi", "post", 2, @"^[0-9]\d*$", "测试权限管理隔输入出错");
            string CeshiQualification = Utils.GetRequest("CeshiQualification", "all", 2, @"^[^\^]{1,2000}$", "请输入测试号");      
            xml.dss["ceshi"] = ceshi;
            xml.dss["CeshiQualification"] = CeshiQualification;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("" + GameName + "设置", "设置成功，正在返回..", Utils.getUrl("DawnlifeManage.aspx?act=neice"), "2");
        }
        else
        {

            string strText = "测试权限管理:/,添加测试号(#号结束):/,";
            string strName = "ceshi,CeshiQualification,backurl";
            string strType = "select,text,hidden";
            string strValu = xml.dss["ceshi"] + "'" + xml.dss["CeshiQualification"] + "'" + Utils.getPage(0) + "";
            string strEmpt = "0|开放|1|内测,true";
            string strIdea = "/";
            string strOthe = "确定修改|reset,DawnlifeManage.aspx?act=neice,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            string CeshiQualification = Convert.ToString(ub.GetSub("CeshiQualification", xmlPath));
            string[] name = CeshiQualification.Split('#');
            // foreach (string n in imgNum)
            builder.Append("当前测试号:<br />");
            for (int n = 0; n < name.Length - 1; n++)
            {
                if ((n+1) % 5 == 0)
                { 
                    builder.Append(name[n] + "," + "<br />");
                }
                else
                {
                    builder.Append(name[n] + ",");
                }
            }
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", "<br/>"));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx") + "\">返回" + GameName + "管理</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
    //游戏重置
    protected void ResetPage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId != 1 && ManageId != 9)
        {
            Utils.Error("权限不足", "");
        }

        Master.Title = "" + GameName + "管理中心";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx") + "\">" + GameName + "管理</a>&gt;游戏重置");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?info=1&amp;act=reset") + "\">[一键全部重置]</a><b>（包括清空排行榜、奖池、闯荡用户表、切换天数表、购买卖出表）</b>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?info=2&amp;act=reset") + "\">[单独重置排行榜]</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?info=3&amp;act=reset") + "\">[单独重置奖池]</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?info=4&amp;act=reset") + "\">[单独重置用户表]</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?info=5&amp;act=reset") + "\">[单独重置天数表]</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx?info=6&amp;act=reset") + "\">[单独重置购买卖出表]</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        //builder.Append(Out.Tab("<div class=\"text\">", ""));
        //builder.Append("特别说明：" + "<br />" + "重置排行榜（tb_dawnlifeTop）：(排行榜为用户每次游戏所挣钱的排行，主要用于奖励挣钱前五的用户)" + "<br />" + "重置奖池（tb_dawnlifegift）：（奖池是每日更新的，清空奖池表需给表添加第一条数据（此操作已自动添加第一条数据））" + "<br />" + "重置用户表（tb_dawnlifeUser）：（用户表为" + GameName + "用户游戏的信息表，重置用户表需给表添加第一条数据（此操作已自动添加第一条数据））" + "<br />" + "重置天数表（tb_dawnlifeDays）：（天数表为游戏时切换地区存取数据的表，重置天数表需给表添加第一条数据（此操作已自动添加第一条数据））" + "<br />" + "重置购买卖出表（tb_dawnlifeDo）：（购买卖出表为游戏购买卖出数据表，用于物品的操作）" + "<br />");
        //builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b style=\"color:red\">注意：重置后，数据无法恢复。</b><br />");
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx") + "\">[再看看吧..]</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "1")
        {
            new BCW.BLL.dawnlifeUser().ClearTable("tb_dawnlifeTop");
            new BCW.BLL.dawnlifeUser().ClearTable("tb_dawnlifegift");
            new BCW.BLL.dawnlifeUser().ClearTable("tb_dawnlifeDo");
            new BCW.BLL.dawnlifeUser().ClearTable("tb_dawnlifeDays");
            new BCW.BLL.dawnlifeUser().ClearTable("tb_dawnlifeUser");

            BCW.Model.dawnlifeUser addcs = new BCW.Model.dawnlifeUser();
            addcs.UsID = 53212;
            addcs.coin = 0;
            addcs.debt = 0;
            addcs.money = 520;
            addcs.health = 88;
            addcs.stock = " 0";
            addcs.storehouse = "0";
            addcs.reputation = 88;
            addcs.UsName = "YE";
            addcs.city = "1";
            new BCW.BLL.dawnlifeUser().Add(addcs);

            BCW.Model.dawnlifeDays addday = new BCW.Model.dawnlifeDays();
            addday.UsID = 53212;
            addday.UsName = "YE";
            addday.price = "0";
            addday.news = "0";
            addday.n = 0;
            addday.goods = "0";
            addday.area = "0";
            addday.city = " 0";
            addday.coin = 0;
            addday.day = 0;
            new BCW.BLL.dawnlifeDays().Add(addday);

            BCW.Model.dawnlifegift addj = new BCW.Model.dawnlifegift();
            addj.date = DateTime.Now;
            addj.gift = 1000;
            addj.giftj = 2000;
            addj.UsID = 53212;
            addj.UsName = "YE";
            addj.coin = 0;
            addj.state = 5;
            new BCW.BLL.dawnlifegift().Add(addj);

            Utils.Success("重置游戏", "重置[所有数据]成功..", Utils.getUrl("DawnlifeManage.aspx?act=reset"), "1");
        }
        else if (info == "2")
        {
            new BCW.BLL.dawnlifeUser().ClearTable("tb_dawnlifeTop");
            Utils.Success("重置游戏", "重置[开奖数据]成功..", Utils.getUrl("DawnlifeManage.aspx?act=reset"), "2");
        }
        else if (info == "3")
        {
            new BCW.BLL.dawnlifeUser().ClearTable("tb_dawnlifegift");
            BCW.Model.dawnlifegift addj = new BCW.Model.dawnlifegift();
            addj.date = DateTime.Now;
            addj.gift = 1000;
            addj.giftj = 2000;
            addj.UsID = 53212;
            addj.UsName = "YE";
            addj.coin = 0;
            addj.state = 5;
            new BCW.BLL.dawnlifegift().Add(addj);
            Utils.Success("重置游戏", "重置[投注数据]成功..", Utils.getUrl("DawnlifeManage.aspx?act=reset"), "2");
        }
        else if (info == "4")
        {
            new BCW.BLL.dawnlifeUser().ClearTable("tb_dawnlifeUser");
            BCW.Model.dawnlifeUser addcs = new BCW.Model.dawnlifeUser();
            addcs.UsID = 53212;
            addcs.coin = 0;
            addcs.debt = 0;
            addcs.money = 520;
            addcs.health = 88;
            addcs.stock = " 0";
            addcs.storehouse = "0";
            addcs.reputation = 88;
            addcs.UsName = "YE";
            addcs.city = "1";
            new BCW.BLL.dawnlifeUser().Add(addcs);
            Utils.Success("重置游戏", "重置[排行榜数据]成功..", Utils.getUrl("DawnlifeManage.aspx?act=reset"), "2");
        }
        else if (info == "5")
        {
            new BCW.BLL.dawnlifeUser().ClearTable("tb_dawnlifeDays");
            BCW.Model.dawnlifeDays addday = new BCW.Model.dawnlifeDays();
            addday.UsID = 53212;
            addday.UsName = "YE";
            addday.price = "0";
            addday.news = "0";
            addday.n = 0;
            addday.goods = "0";
            addday.area = "0";
            addday.city = " 0";
            addday.coin = 0;
            addday.day = 0;
            new BCW.BLL.dawnlifeDays().Add(addday);
            Utils.Success("重置游戏", "重置[投注数据]成功..", Utils.getUrl("DawnlifeManage.aspx?act=reset"), "2");
        }
        else if (info == "6")
        {
            new BCW.BLL.dawnlifeUser().ClearTable("tb_dawnlifeDo");
            Utils.Success("重置游戏", "重置[排行榜数据]成功..", Utils.getUrl("DawnlifeManage.aspx?act=reset"), "2");
        }
        builder.Append(Out.Tab("<div class=\"\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("DawnlifeManage.aspx") + "\">返回" + GameName + "管理</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

}
