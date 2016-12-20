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
using System.Collections.Generic;
using BCW.Common;
using System.Text.RegularExpressions;

public partial class Manage_game_xk3swset : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/xinkuai3_TRIAL_GAME.xml";
    protected string GameName = ub.GetSub("XinKuai3Name", "/Controls/xinkuai3_TRIAL_GAME.xml");//游戏名字
    protected string klb = ub.GetSub("klb", "/Controls/xinkuai3_TRIAL_GAME.xml");//快乐币
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
        if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
        {
            //Utils.Success("温馨提示", "使用彩版，页面更直观，更快捷！正在切换进入...", "xk3swset.aspx?ve=2a&amp;u=" + Utils.getstrU() + "", "1");
        }

        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "view":
                ViewPage();//显示详细的投注信息
                break;
            case "del":
                DelPage();//删除一条投注记录
                break;
            case "reset":
                ResetPage();//重置新快3
                break;
            case "stat":
                StatPage();//赢利分析
                break;
            case "peizhi":
                PeizhiPage();//配置管理
                break;
            case "weihu":
                WeihuPage();//游戏维护
                break;
            case "back":
                BackPage();//返赢返负
                break;
            case "backsave":
            case "backsave2":
                BackSavePage(act);
                break;
            case "paihang":
                PaihangPage();//用户排行
                break;
            case "fenxi":
                FenxiPage();//用户购买情况和获奖数据情况
                break;
            case "add":
                AddPage();//手动添加开奖
                break;
            case "robot":
                RobotPage();//传说中的机器人
                break;
            case "Top_add":
                Top_addPage();//紧急添加开奖数据
                break;
            case "ReWard":
                ReWard();
                break;
            case "ReWardCase":
                ReWardCase();
                break;
            case "shiwan":
                ShiwanPage();
                break;
            case "del_sw":
                Del_swPage();//删除试玩ID
                break;
            case "edit":
                EditPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        Master.Title = "" + GameName + "_后台管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Default.aspx") + "\">游戏</a>&gt;");
        builder.Append("" + GameName + "");
        builder.Append(Out.Tab("</div>", ""));

        string searchday = (Utils.GetRequest("inputdate", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8])))$", ""));
        string strText = "输入开奖日期：格式（20160214）/,";
        string strName = "inputdate";
        string strType = "num";
        string strValu = searchday;
        string strEmpt = "";
        string strIdea = "";
        string strOthe = "搜开奖记录,xk3swset.aspx?act=analyze,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        //builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("" + GameName + "开奖信息：");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo2", xmlPath));
        string strWhere = string.Empty;
        if (searchday == "")
        {
            strWhere = "";
        }
        else
        {
            strWhere = "convert(varchar(10),Lottery_time,120)='" + (DateTime.ParseExact(searchday, "yyyyMMdd", null).ToString("yyyy-MM-dd")) + "'";
        }
        string[] pageValUrl = { "act", "inputdate", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.XinKuai3.Model.XK3_Internet_Data> listXK3 = new BCW.XinKuai3.BLL.XK3_Internet_Data().GetXK3_Internet_Datas(pageIndex, pageSize, strWhere, out recordCount);
        if (listXK3.Count > 0)
        {
            int k = 1;
            foreach (BCW.XinKuai3.Model.XK3_Internet_Data n in listXK3)
            {
                if (k % 2 == 0)
                {
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                }
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }
                BCW.XinKuai3.Model.XK3_Bet_SWB model_get = new BCW.XinKuai3.BLL.XK3_Bet_SWB().GetXK3_Bet_SWB_hounum(n.Lottery_issue);
                //" + ((pageIndex - 1) * pageSize + k) + "&nbsp;.&nbsp;
                builder.Append("" + n.Lottery_issue + "期");
                if (n.Lottery_num != "")
                {
                    builder.Append(".<b>[" + n.Lottery_num + "]</b>.[" + string.Format("{0:T}", n.Lottery_time) + "].");
                    //builder.Append("&nbsp;&nbsp;|&nbsp;&nbsp;(时间：" + n.Lottery_time + ")");
                    if (model_get.aa > 0)
                    {
                        builder.Append("(<h style=\"color:red\"><a href=\"" + Utils.getUrl("xk3swset.aspx?act=view&amp;id=" + n.Lottery_issue + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model_get.aa + "</a></h>人投注)");
                        //builder.Append("&nbsp;&nbsp;|&nbsp;&nbsp;<a href=\"" + Utils.getUrl("xk3swset.aspx?act=view&amp;id=" + n.Lottery_issue + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">查看</a>");
                    }
                    else
                    {
                        builder.Append("(" + model_get.aa + "人投注)");
                    }
                }
                else
                {
                    //builder.Append("&nbsp;&nbsp;|&nbsp;&nbsp;<a href=\"" + Utils.getUrl("xk3swset.aspx?act=add&amp;id=" + n.Lottery_issue + "") + "\">手动填入开奖号码</a>");
                    builder.Append(".此为新快3试玩版系统，请到新快3系统手动填入开奖号码");
                }
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

        //builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", Out.Hr()));

        builder.Append("<a href=\"" + Utils.getUrl("xk3swset.aspx?act=fenxi") + "\">投注记录</a><br/>");

        builder.Append("<a href=\"" + Utils.getUrl("xk3swset.aspx?act=stat") + "\">赢利分析</a><br/>");

        builder.Append("<a href=\"" + Utils.getUrl("xk3swset.aspx?act=paihang") + "\">用户排行</a><br/>");

        builder.Append("<a href=\"" + Utils.getUrl("xk3swset.aspx?act=peizhi") + "\">游戏配置</a><br/>");

        builder.Append("<a href=\"" + Utils.getUrl("xk3swset.aspx?act=reset") + "\">重置游戏</a><br/>");

        builder.Append("<a href=\"" + Utils.getUrl("xk3swset.aspx?act=weihu") + "\">游戏维护</a><br/>");

        builder.Append("<a href=\"" + Utils.getUrl("xk3swset.aspx?act=shiwan") + "\">试玩ID</a><br/>");
        builder.Append(Out.Tab("</div>", ""));


        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //=============赢利分析=======================
    private void StatPage()
    {
        Master.Title = "" + GameName + "_赢利分析";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("xk3swset.aspx") + "\">" + GameName + "</a>&gt;赢利分析");
        builder.Append(Out.Tab("</div>", "<br/>"));
        //今天投入数+今天兑奖数
        long TodayTou = new BCW.XinKuai3.BLL.XK3_Bet_SWB().GetPrice("PutGold", "DateDiff(dd,Input_Time,getdate())=0 AND isRobot='0'");
        long TodayDui = new BCW.XinKuai3.BLL.XK3_Bet_SWB().GetPrice("GetMoney", "DateDiff(dd,Input_Time,getdate())=0 AND isRobot='0'");
        //昨天投入数+昨天兑奖数
        long yesTou = new BCW.XinKuai3.BLL.XK3_Bet_SWB().GetPrice("PutGold", "DateDiff(dd,Input_Time,getdate()-1)=0 AND isRobot='0'");
        long yesDui = new BCW.XinKuai3.BLL.XK3_Bet_SWB().GetPrice("GetMoney", "DateDiff(dd,Input_Time,getdate()-1)=0 AND isRobot='0'");
        //本月投入数+本月兑奖数
        long MonthTou = new BCW.XinKuai3.BLL.XK3_Bet_SWB().GetPrice("PutGold", "datediff(month,[Input_Time],getdate())=0 AND isRobot='0'");
        long MonthDui = new BCW.XinKuai3.BLL.XK3_Bet_SWB().GetPrice("GetMoney", "datediff(month,[Input_Time],getdate())=0 AND isRobot='0' AND State!='3'");
        //上月投入数+上月兑奖数
        long Month2Tou = new BCW.XinKuai3.BLL.XK3_Bet_SWB().GetPrice("PutGold", "datediff(month,[Input_Time],getdate())=1 AND isRobot='0'");
        long Month2Dui = new BCW.XinKuai3.BLL.XK3_Bet_SWB().GetPrice("GetMoney", "datediff(month,[Input_Time],getdate())=1 AND isRobot='0' AND State!='3'");
        //今年投入+今年兑奖
        long yearTou = new BCW.XinKuai3.BLL.XK3_Bet_SWB().GetPrice("PutGold", "datediff(YEAR,[Input_Time],getdate())=0 AND isRobot='0'");
        long yearDui = new BCW.XinKuai3.BLL.XK3_Bet_SWB().GetPrice("GetMoney", "datediff(YEAR,[Input_Time],getdate())=0 AND isRobot='0' AND State!='3'");
        //总投入+总兑奖
        long allTou = new BCW.XinKuai3.BLL.XK3_Bet_SWB().GetPrice("PutGold", "State>0 AND isRobot='0'");
        long allDui = new BCW.XinKuai3.BLL.XK3_Bet_SWB().GetPrice("GetMoney", "State>0 AND isRobot='0' AND State!='3'");


        builder.Append(Out.Tab("<div>", ""));
        builder.Append("今天赢利：" + (TodayTou - TodayDui) + "" + klb + "<br/>今天收支：收" + TodayTou + "，支" + TodayDui + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("昨天赢利：" + (yesTou - yesDui) + "" + klb + "<br/>昨天收支：收" + yesTou + "，支" + yesDui + "");
        //builder.Append("<hr/>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("本月赢利：" + (MonthTou - MonthDui) + "" + klb + "<br/>本月收支：收" + MonthTou + "，支" + MonthDui + "");
        builder.Append(Out.Tab("</div>", "<br />"));


        builder.Append(Out.Tab("<div>", ""));
        builder.Append("上月赢利：" + (Month2Tou - Month2Dui) + "" + klb + "<br/>上月收支：收" + Month2Tou + "，支" + Month2Dui + "");
        //builder.Append("<hr/>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("今年赢利：" + (yearTou - yearDui) + "" + klb + "<br/>今年收支：收" + yearTou + "，支" + yearDui + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b>总赢利：" + (allTou - allDui) + "" + klb + "<br/>总收支：收" + allTou + "，支" + allDui + "</b>");
        //builder.Append("<hr/>");
        builder.Append(Out.Tab("</div>", Out.Hr()));

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        if (Utils.ToSChinese(ac) == "马上查询")
        {
            //string searchday1 = string.Empty;
            //string searchday2 = string.Empty;
            //searchday1 = Utils.GetRequest("sTime", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8])))$", DateTime.Now.ToString("yyyyMMdd"));
            //searchday2 = Utils.GetRequest("oTime", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8])))$", DateTime.Now.ToString("yyyyMMdd"));

            //if (searchday1 == "")
            //{
            //    searchday1 = DateTime.Now.ToString("yyyyMMdd");
            //}
            //if (searchday2 == "")
            //{
            //    searchday2 = DateTime.Now.AddDays(1).ToString("yyyyMMdd");
            //}

            DateTime searchday1 = Utils.ParseTime(Utils.GetRequest("sTime", "all", 2, DT.RegexTime, "开始时间填写无效"));
            DateTime searchday2 = Utils.ParseTime(Utils.GetRequest("oTime", "all", 2, DT.RegexTime, "结束时间填写无效"));

            long dateTou = new BCW.XinKuai3.BLL.XK3_Bet_SWB().GetPrice("PutGold", "Input_Time>='" + searchday1 + "' and Input_Time<='" + searchday2 + "'AND isRobot='0'AND State!='3'");
            long dateDui = new BCW.XinKuai3.BLL.XK3_Bet_SWB().GetPrice("GetMoney", "Input_Time>='" + searchday1 + "' and Input_Time<='" + searchday2 + "'AND isRobot='0'AND State!='3'");

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<b>盈利" + (dateTou - dateDui) + "" + klb + ".</b>");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "开始日期：,结束日期：,";
            string strName = "sTime,oTime,act";
            string strType = "date,date,hidden";
            string strValu = "" + searchday1.ToString("yyyy-MM-dd HH:mm:ss") + "'" + searchday2.ToString("yyyy-MM-dd HH:mm:ss") + "'''backsave";
            string strEmpt = "false,false,false";
            string strIdea = "/";
            string strOthe = "马上查询,xk3swset.aspx?act=stat,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<b>盈利0" + klb + ".</b>");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "开始日期：,结束日期：,";
            string strName = "sTime,oTime,act";
            string strType = "date,date,hidden";
            string strValu = "" + DT.FormatDate(DateTime.Now.AddDays(-1), 0) + "'" + DT.FormatDate(DateTime.Now, 0) + "'''backsave";
            string strEmpt = "false,false,false";
            string strIdea = "/";
            string strOthe = "马上查询,xk3swset.aspx?act=stat,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("xk3swset.aspx") + "\">&lt;&lt;&lt;返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //==========根据期号，显示详细的投注信息======
    private void ViewPage()
    {
        Master.Title = "" + GameName + "_投注情况";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("xk3swset.aspx") + "\">" + GameName + "</a>&gt;投注情况");
        builder.Append(Out.Tab("</div>", "<br/>"));

        string Lottery_issue = (Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.XinKuai3.Model.XK3_Bet_SWB model = new BCW.XinKuai3.BLL.XK3_Bet_SWB().GetXK3_Bet_SWB_num(Lottery_issue);
        if (model == null)
        {
            Utils.Error("不存在投注记录", "");
        }
        BCW.XinKuai3.Model.XK3_Internet_Data model_num = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3kainum(Lottery_issue);
        if (model_num == null)
        {
            Utils.Error("该" + Lottery_issue + "期的开奖号码不存在", "");
        }
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 1)
            builder.Append("查看方式：总下注|");
        else
            builder.Append("查看方式：<a href=\"" + Utils.getUrl("xk3swset.aspx?act=view&amp;id=" + Lottery_issue + "&amp;ptype=1") + "\">总下注</a>|");
        if (ptype == 2)
            builder.Append("中奖情况");
        else
            builder.Append("<a href=\"" + Utils.getUrl("xk3swset.aspx?act=view&amp;id=" + Lottery_issue + "&amp;ptype=2") + "\">中奖情况</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo2", xmlPath));
        string strWhere = string.Empty;
        if (ptype == 1)
        {
            strWhere += "Lottery_issue='" + Lottery_issue + "'";
        }
        else if (ptype == 2)
        {
            strWhere = "Lottery_issue='" + Lottery_issue + "' and state=2";
        }


        string[] pageValUrl = { "act", "id", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.XinKuai3.Model.XK3_Bet_SWB> listXK3pay = new BCW.XinKuai3.BLL.XK3_Bet_SWB().GetXK3_Bet_SWBs(pageIndex, pageSize, strWhere, out recordCount);

        if (listXK3pay.Count > 0)
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("第" + Lottery_issue + "期开出：" + model_num.Lottery_num + "；");

            builder.Append("共" + recordCount + "注");

            builder.Append(Out.Tab("</div>", "<br />"));

            int k = 1;
            foreach (BCW.XinKuai3.Model.XK3_Bet_SWB n in listXK3pay)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br/>"));
                }

                string Getnum = string.Empty;
                if (n.Play_Way == 1)
                {
                    Getnum = n.Sum;//和值
                }
                else if (n.Play_Way == 2)
                {
                    Getnum = n.Three_Same_All;//三同号通选
                }
                else if (n.Play_Way == 3)
                {
                    Getnum = n.Three_Same_Single;//三同号单选
                }
                else if (n.Play_Way == 4)
                {
                    Getnum = n.Three_Same_Not;//三不同号
                }
                else if (n.Play_Way == 5)
                {
                    Getnum = n.Three_Continue_All;//三连号通选
                }
                else if (n.Play_Way == 6)
                {
                    Getnum = n.Two_Same_All;//二同号复选
                }
                else if (n.Play_Way == 7)
                {
                    Getnum = n.Two_Same_Single;//二同号单选
                }
                else if (n.Play_Way == 8)
                {
                    Getnum = n.Two_dissame;//二不同号
                }
                else if (n.Play_Way == 9)//大小
                {
                    //1大2小、1单2双
                    if (n.DaXiao == "1")
                    {
                        Getnum = "大";
                    }
                    else if (n.DaXiao == "2")
                    {
                        Getnum = "小";
                    }
                }
                else if (n.Play_Way == 10)//双单
                {
                    if (n.DanShuang == "2")
                    {
                        Getnum = "双";
                    }
                    else if (n.DanShuang == "1")
                    {
                        Getnum = "单";
                    }
                }

                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(n.UsID) + "</a>");

                builder.Append("" + OutType(n.Play_Way) + "{" + Getnum + "}每注" + n.Zhu_money + "" + klb + "/共" + n.Zhu + "注/共投" + n.PutGold + klb + ".标识ID:" + n.ID + ".[" + n.Input_Time + "]");//DT.FormatDate(n.Input_Time, 1)
                if (n.GetMoney > 0)
                {
                    if (n.State == 2)
                    {
                        builder.Append("<h style=\"color:red\">[赢" + n.GetMoney + "" + klb + "]</h>.(已领奖)");
                    }
                    else if (n.State == 3)
                    {
                        builder.Append("<h style=\"color:red\">[赢" + n.GetMoney + "" + klb + "]</h>.(过期未领奖)");
                    }
                    else
                        builder.Append("<h style=\"color:red\">[赢" + n.GetMoney + "" + klb + "]</h>.(未领奖)");
                    //builder.Append("<b style=\"color:blue\">共赢了" + n.GetMoney + " " + klb + "</b>.");
                }
                builder.Append(".<a href=\"" + Utils.getUrl("xk3swset.aspx?act=del&amp;id=" + n.ID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[删]</a>");
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

        }
        else
        {
            builder.Append(Out.Div("div", "没有返彩或无下注记录.."));
        }
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("xk3swset.aspx") + "\">&lt;&lt;&lt;返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //==========显示标题的投注方式================
    private string OutType(int Types)
    {
        string pText = string.Empty;
        if (Types == 1)
            pText = "总和复选";//和值投注
        else if (Types == 2)
            pText = "任意豹子";//三同号通选
        else if (Types == 3)
            pText = "直选豹子";//三同号单选
        else if (Types == 4)
            pText = "复选任三";//三不同号
        else if (Types == 5)
            pText = "三连通选";//三连号通选
        else if (Types == 6)
            pText = "对子复选";//二同号复选
        else if (Types == 7)
            pText = "组二单选";//二同号单选
        else if (Types == 8)
            pText = "复选任二";//二不同号
        else if (Types == 9)
            pText = "押注大小";//大小投注
        else if (Types == 10)
            pText = "押注单双";//单双投注
        return pText;
    }

    //==========删除一条投注记录==================
    private void DelPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        if (info == "")
        {
            Master.Title = "删除一条投注记录";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除该记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("xk3swset.aspx?info=ok&amp;act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("xk3swset.aspx") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.XinKuai3.BLL.XK3_Bet_SWB().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            else
            {
                //根据id查询-购买表
                BCW.XinKuai3.Model.XK3_Bet_SWB model = new BCW.XinKuai3.BLL.XK3_Bet_SWB().GetXK3_Bet_SWB(id);
                int meid = model.UsID;//用户名
                string mename = new BCW.BLL.User().GetUsName(meid);//获得id对应的用户名
                int state_get = model.State;//用户购买情况
                //排行榜
                //BCW.XinKuai3.Model.XK3_Toplist_SWB aa = new BCW.XinKuai3.BLL.XK3_Toplist_SWB().GetXK3_meid(meid);
                //long price_put = aa.PutGold - model.PutGold;//排行榜-投入
                //long Price2 = aa.WinGold - model.GetMoney;//排行榜-所得
                long Price = 0;
                //如果未开奖，退回本金
                if (state_get == 0)
                {
                    Price = model.PutGold;
                    //new BCW.BLL.User().UpdateiGold(model.UsID, Price, "系统退回新快3_试玩版第" + model.Lottery_issue + "期未开奖的" + model.PutGold + "酷币！");//减少系统总的酷币
                    new BCW.XinKuai3.BLL.SWB().UpdateiGold(model.UsID, Price);
                    new BCW.BLL.Guest().Add(1, meid, mename, "系统退回您的新快3_试玩版：第" + model.Lottery_issue + "期未开奖的" + model.PutGold + "" + klb + "！");
                }
                //未中奖或已中奖，退回本金-所得
                //else if ((state_get == 2) || (state_get == 1))
                else
                {
                    Price = model.PutGold - model.GetMoney;//系统-(本金-所得)
                    //new BCW.BLL.User().UpdateiGold(model.UsID, Price, "无效购奖或非法操作，系统退回新快3_试玩版第" + model.Lottery_issue + "期的" + Price + "酷币！");//减少系统总的酷币
                    new BCW.XinKuai3.BLL.SWB().UpdateiGold(model.UsID, Price);
                    new BCW.BLL.Guest().Add(1, meid, mename, "无效购奖或非法操作，系统退回新快3_试玩版第" + model.Lottery_issue + "期未开奖的" + Price + "" + klb + "！");
                }
                ////如果过期不兑奖，退回本金
                //else if (state_get == 3)
                //{
                //    Price = model.PutGold;
                //    //new BCW.BLL.User().UpdateiGold(model.UsID, Price, "系统退回新快3_试玩版第" + model.Lottery_issue + "期未兑奖的"+model.PutGold+"酷币！");//减少系统总的酷币
                //    new BCW.XinKuai3.BLL.SWB().UpdateiGold(model.UsID, Price);
                //    new BCW.BLL.Guest().Add(1, meid, mename, "系统退回新快3_试玩版第" + model.Lottery_issue + "期未兑奖的" + model.PutGold + "" + klb + "！");
                //}


                //new BCW.XinKuai3.BLL.XK3_Toplist_SWB().Update_getgold(meid, Price2);//减少排行榜所得酷币
                //new BCW.XinKuai3.BLL.XK3_Toplist_SWB().Update_gold(meid, price_put);//减少排行投入的酷币
                new BCW.XinKuai3.BLL.XK3_Bet_SWB().Delete(id);
                if (state_get == 0)
                    Utils.Success("删除投注记录", "删除记录成功..", Utils.getPage("xk3swset.aspx?act=fenxi&amp;ptype=5"), "2");//未开奖
                else if ((state_get == 1))
                    Utils.Success("删除投注记录", "删除记录成功..", Utils.getPage("xk3swset.aspx?act=fenxi&amp;ptype=4"), "2");//不中奖
                else if (state_get == 2)
                    Utils.Success("删除投注记录", "删除记录成功..", Utils.getPage("xk3swset.aspx?act=fenxi&amp;ptype=2"), "2");//中奖
                else if (state_get == 3)
                    Utils.Success("删除投注记录", "删除记录成功..", Utils.getPage("xk3swset.aspx?act=fenxi&amp;ptype=3"), "2");//系统回收
            }
        }
    }

    //==========重置新快3=========================
    private void ResetPage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId != 1 && ManageId != 9)
        {
            Utils.Error("权限不足", "");
        }
        Master.Title = "" + GameName + "_重置游戏";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("xk3swset.aspx") + "\">" + GameName + "</a>&gt;重置游戏");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("1、<a href=\"" + Utils.getUrl("xk3swset.aspx?info=1&amp;act=reset") + "\">[<b>一键全部重置</b>]</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("2、<a href=\"" + Utils.getUrl("xk3swset.aspx?info=2&amp;act=reset") + "\">[单独重置投注数据]</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("3、<a href=\"" + Utils.getUrl("xk3swset.aspx?info=3&amp;act=reset") + "\">[单独重置排行榜]</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        //builder.Append(Out.Tab("<div>", ""));
        //builder.Append("4、<a href=\"" + Utils.getUrl("xk3swset.aspx?act=del_date") + "\">根据日期删除记录。</a>");
        //builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        //builder.Append("<br/>");
        builder.Append("<h style=\"color:red\">注意：重置后，数据无法恢复。</h><br />");
        builder.Append("<a href=\"" + Utils.getUrl("xk3swset.aspx") + "\">&lt;&lt;&lt;返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "1")
        {
            new BCW.XinKuai3.BLL.XK3_Bet_SWB().ClearTable("tb_XK3_Bet_SWB");
            new BCW.XinKuai3.BLL.XK3_Bet_SWB().ClearTable("tb_XK3_Toplist_SWB");
            Utils.Success("重置游戏", "重置[所有数据]成功..", Utils.getUrl("xk3swset.aspx?act=reset"), "2");
        }
        else if (info == "2")
        {
            new BCW.XinKuai3.BLL.XK3_Bet_SWB().ClearTable("tb_XK3_Bet_SWB");
            Utils.Success("重置游戏", "重置[投注数据]成功..", Utils.getUrl("xk3swset.aspx?act=reset"), "2");
        }
        else if (info == "3")
        {
            new BCW.XinKuai3.BLL.XK3_Bet_SWB().ClearTable("tb_XK3_Toplist_SWB");
            Utils.Success("重置游戏", "重置[排行榜数据]成功..", Utils.getUrl("xk3swset.aspx?act=reset"), "2");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //==========新快3系统配置=====================
    private void PeizhiPage()
    {
        Master.Title = "" + GameName + "_游戏配置";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("xk3swset.aspx") + "\">" + GameName + "</a>&gt;游戏配置");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-1]$", "0"));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        string xmlPath = "/Controls/xinkuai3_TRIAL_GAME.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            if (ptype == 0)
            {
                string Name = Utils.GetRequest("Name", "post", 2, @"^[^\^]{1,20}$", "游戏名称限1-20字内");
                string klb = Utils.GetRequest("klb", "post", 2, @"^[^\^]{1,20}$", "游戏币名称限1-20字内");
                string XinKuai3top1 = Utils.GetRequest("XinKuai3top1", "post", 3, @"^[\s\S]{1,2000}$", "头部Ubb限2000字内");
                string Logo = Utils.GetRequest("logo", "post", 3, @"^[^\^]{1,200}$", "Logo地址限200字内");
                string Sec = Utils.GetRequest("xSec", "post", 2, @"^[0-9]\d*$", "秒数填写出错");//等待几秒开奖
                string SiteListNo = Utils.GetRequest("SiteListNo", "post", 2, @"^[0-9]\d*$", "前台分页数填写出错");
                string SiteListNo2 = Utils.GetRequest("SiteListNo2", "post", 2, @"^[0-9]\d*$", "后台分页数填写出错");
                string SmallPay = Utils.GetRequest("xSmallPay", "post", 2, @"^[0-9]\d*$", "最小下注" + klb + "填写错误");
                string BigPay = Utils.GetRequest("xBigPay", "post", 2, @"^[0-9]\d*$", "最大下注" + klb + "填写错误");
                string Price = Utils.GetRequest("XK3Price", "post", 2, @"^[0-9]\d*$", "每期每ID限购多少" + klb + "填写错误");
                string Maxnum = Utils.GetRequest("Maxnum", "post", 2, @"^[0-9]\d*$", "大小单双最大差值" + klb + "填写错误");
                string Expir = Utils.GetRequest("xExpir", "post", 2, @"^[0-9]\d*$", "防刷秒数填写出错");
                string OnTime = Utils.GetRequest("xOnTime", "post", 3, @"^[0-9]{2}:[0-9]{2}-[0-9]{2}:[0-9]{2}$", "游戏开放时间填写错误，可留空");
                string date = Utils.GetRequest("Xdate", "post", 2, @"^[0-9]\d*$", "系统回收快乐币填写错误");
                string XinKuai3Foot = Utils.GetRequest("XinKuai3Foot", "post", 3, @"^[\s\S]{1,2000}$", "底部Ubb限2000字内");
                string XIsBot = Utils.GetRequest("XIsBot", "post", 2, @"^[0-1]$", "机器人选择出错");
                string Xupdatetime = Utils.GetRequest("Xupdatetime", "post", 2, @"^[0-9]\d*$", "试玩版可领取间隔多少分钟填写出错");
                string Xjine_add = Utils.GetRequest("Xjine_add", "post", 2, @"^[0-9]\d*$", "每次领取的" + klb + "填写错误");
                string Xjine_da = Utils.GetRequest("Xjine_da", "post", 2, @"^[0-9]\d*$", "限定最大的" + klb + "填写错误");

                //继续验证时间
                if (OnTime != "")
                {
                    string[] temp = OnTime.Split("-".ToCharArray());
                    DateTime dt1 = Utils.ParseTime(temp[0]);
                    DateTime dt2 = Utils.ParseTime(temp[1]);
                }

                xml.dss["XinKuai3Name"] = Name;
                xml.dss["XinKuai3top1"] = XinKuai3top1;
                xml.dss["logo"] = Logo;
                xml.dss["xSec"] = Sec;
                xml.dss["SiteListNo"] = SiteListNo;
                xml.dss["SiteListNo2"] = SiteListNo2;
                xml.dss["xSmallPay"] = SmallPay;
                xml.dss["xBigPay"] = BigPay;
                xml.dss["Maxnum"] = Maxnum;
                xml.dss["XK3Price"] = Price;
                xml.dss["xExpir"] = Expir;
                xml.dss["Xdate"] = date;
                xml.dss["xOnTime"] = OnTime;
                xml.dss["XinKuai3Foot"] = XinKuai3Foot;
                xml.dss["XIsBot"] = XIsBot;
                xml.dss["Xupdatetime"] = Xupdatetime;
                xml.dss["Xjine_add"] = Xjine_add;
                xml.dss["Xjine_da"] = Xjine_da;
                xml.dss["klb"] = klb;
            }
            else
            {
                string XinKuai3Sum2 = Utils.GetRequest("XinKuai3Sum2", "post", 2, @"^[0-9]\d*$", "和值4、17赔率错误");
                string XSum1 = Utils.GetRequest("XSum1", "post", 2, @"^[0-9]\d*$", "和值5、16赔率错误");
                string XSum2 = Utils.GetRequest("XSum2", "post", 2, @"^[0-9]\d*$", "和值6、15赔率错误");
                string XSum3 = Utils.GetRequest("XSum3", "post", 2, @"^[0-9]\d*$", "和值7、14赔率错误");
                string XSum4 = Utils.GetRequest("XSum4", "post", 2, @"^[0-9]\d*$", "和值8、13赔率错误");
                string XSum5 = Utils.GetRequest("XSum5", "post", 2, @"^[0-9]\d*$", "和值9、12赔率错误");
                string XinKuai3Sum1 = Utils.GetRequest("XinKuai3Sum1", "post", 2, @"^[0-9]\d*$", "和值10、11赔率错误");
                string XinKuai3Three_Same_All = Utils.GetRequest("XinKuai3Three_Same_All", "post", 2, @"^[0-9]\d*$", "三同号通选赔率错误");
                string XinKuai3Three_Same_Single = Utils.GetRequest("XinKuai3Three_Same_Single", "post", 2, @"^[0-9]\d*$", "三同号单选赔率错误");
                string XinKuai3Three_Same_Not = Utils.GetRequest("XinKuai3Three_Same_Not", "post", 2, @"^[0-9]\d*$", "三不同号赔率错误");
                string XinKuai3Three_Continue_All = Utils.GetRequest("XinKuai3Three_Continue_All", "post", 2, @"^[0-9]\d*$", "三连号通选赔率错误");
                string XinKuai3Two_Same_All = Utils.GetRequest("XinKuai3Two_Same_All", "post", 2, @"^[0-9]\d*$", "二同号通选赔率错误");
                string XinKuai3Two_Same_Single = Utils.GetRequest("XinKuai3Two_Same_Single", "post", 2, @"^[0-9]\d*$", "二同号单选赔率错误");
                string XinKuai3Two_dissame = Utils.GetRequest("XinKuai3Two_dissame", "post", 2, @"^[0-9]\d*$", "二不同号赔率错误");
                string Xda = Utils.GetRequest("Xda", "post", 2, @"^(\d)*(\.(\d){1,2})?$", "大的赔率错误");
                string Xxiao = Utils.GetRequest("Xxiao", "post", 2, @"^(\d)*(\.(\d){1,2})?$", "小的赔率错误");
                string Xdan = Utils.GetRequest("Xdan", "post", 2, @"^(\d)*(\.(\d){1,2})?$", "单的赔率错误");
                string Xshuang = Utils.GetRequest("Xshuang", "post", 2, @"^(\d)*(\.(\d){1,2})?$", "双的赔率错误");
                string Xfudong = Utils.GetRequest("Xfudong", "post", 2, @"^(\d)*(\.(\d){1,2})?$", "浮动的赔率错误");
                string Xda1 = Utils.GetRequest("Xda1", "post", 2, @"^(\d)*(\.(\d){1,2})?$", "实时大的赔率错误");
                string Xxiao1 = Utils.GetRequest("Xxiao1", "post", 2, @"^(\d)*(\.(\d){1,2})?$", "实时小的赔率错误");
                string Xdan1 = Utils.GetRequest("Xdan1", "post", 2, @"^(\d)*(\.(\d){1,2})?$", "实时单的赔率错误");
                string Xshuang1 = Utils.GetRequest("Xshuang1", "post", 2, @"^(\d)*(\.(\d){1,2})?$", "实时双的赔率错误");
                string Xoverpeilv = Utils.GetRequest("Xoverpeilv", "post", 2, @"^(\d)*(\.(\d){1,2})?$", "限定连开最大的赔率错误");

                xml.dss["XinKuai3Sum2"] = XinKuai3Sum2;
                xml.dss["XSum1"] = XSum1;
                xml.dss["XSum2"] = XSum2;
                xml.dss["XSum3"] = XSum3;
                xml.dss["XSum4"] = XSum4;
                xml.dss["XSum5"] = XSum5;
                xml.dss["XinKuai3Sum1"] = XinKuai3Sum1;
                xml.dss["XinKuai3Three_Same_All"] = XinKuai3Three_Same_All;
                xml.dss["XinKuai3Three_Same_Single"] = XinKuai3Three_Same_Single;
                xml.dss["XinKuai3Three_Same_Not"] = XinKuai3Three_Same_Not;
                xml.dss["XinKuai3Three_Continue_All"] = XinKuai3Three_Continue_All;
                xml.dss["XinKuai3Two_Same_All"] = XinKuai3Two_Same_All;
                xml.dss["XinKuai3Two_Same_Single"] = XinKuai3Two_Same_Single;
                xml.dss["XinKuai3Two_dissame"] = XinKuai3Two_dissame;
                xml.dss["Xda"] = Xda;
                xml.dss["Xxiao"] = Xxiao;
                xml.dss["Xdan"] = Xdan;
                xml.dss["Xshuang"] = Xshuang;
                xml.dss["Xfudong"] = Xfudong;
                xml.dss["Xda1"] = Xda1;
                xml.dss["Xxiao1"] = Xxiao1;
                xml.dss["Xdan1"] = Xdan1;
                xml.dss["Xshuang1"] = Xshuang1;
                xml.dss["Xoverpeilv"] = Xoverpeilv;
            }

            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("新快3_试玩版设置", "设置成功，正在返回..", Utils.getUrl("xk3swset.aspx?act=peizhi&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            if (ptype == 0)
            {
                builder.Append("新快3_试玩版设置|");
                builder.Append("<a href=\"" + Utils.getUrl("xk3swset.aspx?act=peizhi&amp;ptype=1") + "\">赔率设置</a>");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("xk3swset.aspx?act=peizhi&amp;ptype=0") + "\">新快3_试玩版设置</a>");
                builder.Append("|赔率设置");
            }
            builder.Append(Out.Tab("</div>", ""));
            if (ptype == 0)
            {
                string strText = "游戏名称：/,游戏币的名称：/,头部Ubb：/,游戏Logo路径：/,等待几秒开奖：/,前台分页行数：/,后台分页行数：/,最小下注" + klb + "：/,最大下注" + klb + "：/,每期每ID限购多少" + klb + "：（0为不限投）/,大小单双最大" + klb + "差值：（0为没有差值）/,下注防刷(秒)：/,试玩币间隔分钟领取：（不能为0）/,试玩版每次领取金额：/,试玩版最高可领取金额：/,游戏开奖时间：/,超过几天系统自动回收" + klb + "：/,底部Ubb：/,机器人：/,";
                string strName = "Name,klb,XinKuai3top1,Logo,xSec,SiteListNo,SiteListNo2,xSmallPay,xBigPay,XK3Price,Maxnum,xExpir,Xupdatetime,Xjine_add,Xjine_da,xOnTime,Xdate,XinKuai3Foot,XIsBot,backurl";
                string strType = "text,text,textarea,text,num,num,num,num,num,num,num,num,num,num,num,text,text,textarea,select,hidden";
                string strValu = "" + xml.dss["XinKuai3Name"] + "'" + xml.dss["klb"] + "'" + xml.dss["XinKuai3top1"] + "'" + xml.dss["logo"] + "'" + xml.dss["xSec"] + "'" + xml.dss["SiteListNo"] + "'" + xml.dss["SiteListNo2"] + "'" + xml.dss["xSmallPay"] + "'" + xml.dss["xBigPay"] + "'" + xml.dss["XK3Price"] + "'" + xml.dss["Maxnum"] + "'" + xml.dss["xExpir"] + "'" + xml.dss["Xupdatetime"] + "'" + xml.dss["Xjine_add"] + "'" + xml.dss["Xjine_da"] + "'" + xml.dss["xOnTime"] + "'" + xml.dss["Xdate"] + "'" + xml.dss["XinKuai3Foot"] + "'" + xml.dss["XIsBot"] + "'" + Utils.getPage(0) + "";
                string strEmpt = "false,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,true,true,0|关闭|1|开启,false";
                string strIdea = "/";
                string strOthe = "确定修改,xk3swset.aspx?act=peizhi,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("温馨提示:<br />游戏开放时间填写格式为:09:26-22:26.<br/>大小单双最大差值不能大于最大下注" + klb + "。");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                string strText = "和值4、17：,和值5、16：,和值6、15：,和值7、14：,和值8、13：,和值9、12：,和值10、11：,三同号通选：,三同号单选：,三不同号：,三连号通选：,二同号通选：,二同号单选：,二不同号：,大：,小：,单：,双：,实时大：,实时小：,实时单：,实时双：,大小单双浮动赔率：/,大小单双限定连开最大的赔率：/,,,";
                string strName = "XinKuai3Sum2,XSum1,XSum2,XSum3,XSum4,XSum5,XinKuai3Sum1,XinKuai3Three_Same_All,XinKuai3Three_Same_Single,XinKuai3Three_Same_Not,XinKuai3Three_Continue_All,XinKuai3Two_Same_All,XinKuai3Two_Same_Single,XinKuai3Two_dissame,Xda,Xxiao,Xdan,Xshuang,Xda1,Xxiao1,Xdan1,Xshuang1,Xfudong,Xoverpeilv,ptype,backurl";
                string strType = "num,num,num,num,num,num,num,num,num,num,num,num,num,num,text,text,text,text,text,text,text,text,text,text,hidden,hidden";
                string strValu = "" + xml.dss["XinKuai3Sum2"] + "'" + xml.dss["XSum1"] + "'" + xml.dss["XSum2"] + "'" + xml.dss["XSum3"] + "'" + xml.dss["XSum4"] + "'" + xml.dss["XSum5"] + "'" + xml.dss["XinKuai3Sum1"] + "'" + xml.dss["XinKuai3Three_Same_All"] + "'" + xml.dss["XinKuai3Three_Same_Single"] + "'" + xml.dss["XinKuai3Three_Same_Not"] + "'" + xml.dss["XinKuai3Three_Continue_All"] + "'" + xml.dss["XinKuai3Two_Same_All"] + "'" + xml.dss["XinKuai3Two_Same_Single"] + "'" + xml.dss["XinKuai3Two_dissame"] + "'" + xml.dss["Xda"] + "'" + xml.dss["Xxiao"] + "'" + xml.dss["Xdan"] + "'" + xml.dss["Xshuang"] + "'" + xml.dss["Xda1"] + "'" + xml.dss["Xxiao1"] + "'" + xml.dss["Xdan1"] + "'" + xml.dss["Xshuang1"] + "'" + xml.dss["Xfudong"] + "'" + xml.dss["Xoverpeilv"] + "'1'" + Utils.getPage(0) + "";
                string strEmpt = "false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确定修改,xk3swset.aspx?act=peizhi,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("温馨提示:<br />实时的大小单双赔率没有特殊情况不建议手动修改。");
                builder.Append(Out.Tab("</div>", ""));
            }

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("xk3swset.aspx") + "\">返回上级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }

    //==========游戏维护==========================
    private void WeihuPage()
    {
        Master.Title = "" + GameName + "_游戏维护";
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/xinkuai3_TRIAL_GAME.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置

        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string Status = Utils.GetRequest("Status", "post", 2, @"^[0-1]$", "0");
            string XIstest = Utils.GetRequest("XIstest", "post", 2, @"^[0-1]$", "0");
            xml.dss["xk3Status"] = Status;
            xml.dss["XIstest"] = XIstest;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            if (Status == "0")
            {
                if (XIstest == "0")
                    Utils.Success("" + GameName + "_游戏维护", "游戏已开放，其他玩家可以正常游戏。正在返回..", Utils.getUrl("xk3swset.aspx?act=weihu&amp;backurl=" + Utils.getPage(0) + ""), "3");
                else
                    Utils.Success("" + GameName + "_游戏维护", "游戏已开放，进入测试阶段，其他人暂停游戏。正在返回..", Utils.getUrl("xk3swset.aspx?act=weihu&amp;backurl=" + Utils.getPage(0) + ""), "3");
            }

            else
                Utils.Success("" + GameName + "_游戏维护", "游戏已进入维护模式，正在返回..", Utils.getUrl("xk3swset.aspx?act=weihu&amp;backurl=" + Utils.getPage(0) + ""), "2");
        }
        else
        {
            builder.Append(Out.Div("title", "<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;<a href=\"" + Utils.getUrl("xk3swset.aspx") + "\">" + GameName + "</a>&gt;游戏维护"));
            string strText = "游戏状态:/,游戏测试模式:/,";
            string strName = "Status,XIstest,backurl";
            string strType = "select,select,hidden";
            string strValu = "" + xml.dss["xk3Status"] + "'" + xml.dss["XIstest"] + "'" + Utils.getPage(0) + "";
            string strEmpt = "0|正常|1|维护,0|关闭|1|开启,false";
            string strIdea = "/";
            string strOthe = "确定修改,xk3swset.aspx?act=weihu,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }

        builder.Append(Out.Tab("<div>", "<br/>"));
        //builder.Append("<hr/>");
        builder.Append("<a href=\"" + Utils.getUrl("xk3swset.aspx") + "\">&lt;&lt;&lt;返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //==========返赢返负===界面=========================试玩版不用
    private void BackPage()
    {
        Master.Title = "" + GameName + "_返赢返负";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("xk3swset.aspx") + "\">" + GameName + "</a>&gt;返赢返负");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("返赢点操作");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "开始时间:,结束时间:,返赢千分比:,至少赢多少才返:,";
        string strName = "sTime,oTime,iTar,iPrice,act";
        string strType = "date,date,num,num,hidden";
        string strValu = "" + DT.FormatDate(DateTime.Now.AddDays(-10), 0) + "'" + DT.FormatDate(DateTime.Now, 0) + "'''backsave";
        string strEmpt = "false,false,false,false,false";
        string strIdea = "/";
        string strOthe = "马上返赢,xk3swset.aspx,post,1,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("返负点操作");
        builder.Append(Out.Tab("</div>", ""));

        strText = "开始时间:,结束时间:,返负千分比:,至少负多少才返:,";
        strName = "sTime,oTime,iTar,iPrice,act";
        strType = "date,date,num,num,hidden";
        strValu = "" + DT.FormatDate(DateTime.Now.AddDays(-10), 0) + "'" + DT.FormatDate(DateTime.Now, 0) + "'''backsave2";
        strEmpt = "false,false,false,false,false";
        strIdea = "/";
        strOthe = "马上返负,xk3swset.aspx,post,1,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<hr/>");
        builder.Append("<a href=\"" + Utils.getPage("xk3swset.aspx") + "\">&lt;&lt;&lt;返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //==========返赢返负===执行=========================试玩版不用
    private void BackSavePage(string act)
    {
        DateTime sTime = Utils.ParseTime(Utils.GetRequest("sTime", "post", 2, DT.RegexTime, "开始时间填写无效"));
        DateTime oTime = Utils.ParseTime(Utils.GetRequest("oTime", "post", 2, DT.RegexTime, "结束时间填写无效"));
        int iTar = Utils.ParseInt(Utils.GetRequest("iTar", "post", 2, @"^[0-9]\d*$", "千分比填写错误"));
        int iPrice = Utils.ParseInt(Utils.GetRequest("iPrice", "post", 2, @"^[0-9]\d*$", "至少多少币才返填写错误"));
        if (act == "backsave")
        {
            DataSet ds = new BCW.XinKuai3.BLL.XK3_Bet_SWB().GetList("UsID,sum(GetMoney-PutGold) as WinCents", "Input_Time>='" + sTime + "'and Input_Time<'" + oTime + "' group by UsID");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                long Cents = Convert.ToInt64(ds.Tables[0].Rows[i]["WinCents"]);
                if (Cents > 0 && Cents >= iPrice)
                {
                    int usid = Convert.ToInt32(ds.Tables[0].Rows[i]["UsID"]);
                    long cent = Convert.ToInt64(Cents * (iTar * 0.001));
                    new BCW.BLL.User().UpdateiGold(usid, cent, "新快3_试玩版返赢");
                    //发内线
                    string strLog = "根据你上期新快3_试玩版排行榜上的赢利情况，系统自动返还了" + cent + "" + klb + "[url=/bbs/game/xk3sw.aspx]进入新快3_试玩版[/url]";

                    new BCW.BLL.Guest().Add(0, usid, new BCW.BLL.User().GetUsName(usid), strLog);
                }
            }
            Utils.Success("返赢操作", "返赢操作成功", Utils.getUrl("xk3swset.aspx"), "1");
        }
        else
        {
            DataSet ds = new BCW.XinKuai3.BLL.XK3_Bet_SWB().GetList("UsID,sum(GetMoney-PutGold) as WinCents", "Input_Time>='" + sTime + "'and Input_Time<'" + oTime + "' group by UsID");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                long Cents = Convert.ToInt64(ds.Tables[0].Rows[i]["WinCents"]);
                if (Cents < 0 && Cents < (-iPrice))
                {
                    int usid = Convert.ToInt32(ds.Tables[0].Rows[i]["UsID"]);
                    long cent = Convert.ToInt64((-Cents) * (iTar * 0.001));
                    new BCW.BLL.User().UpdateiGold(usid, cent, "新快3_试玩版返负");
                    //发内线
                    string strLog = "根据你上期新快3_试玩版排行榜上的亏损情况，系统自动返还了" + cent + "" + klb + "[url=/bbs/game/xk3sw.aspx]进入新快3_试玩版[/url]";
                    new BCW.BLL.Guest().Add(0, usid, new BCW.BLL.User().GetUsName(usid), strLog);
                }
            }
            Utils.Success("返负操作", "返负操作成功", Utils.getUrl("xk3swset.aspx"), "1");
        }
    }

    //==========用户排行==========================
    private void PaihangPage()
    {
        Master.Title = "" + GameName + "_用户排行";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("xk3swset.aspx") + "\">" + GameName + "</a>&gt;用户排行");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));
        if (ptype == 1)
            builder.Append("<h style=\"color:red\">净 赚 排 行" + "</h>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("xk3swset.aspx?act=paihang&amp;ptype=1&amp;id=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">净 赚 排 行</a>" + "|");

        if (ptype == 2)
            builder.Append("<h style=\"color:red\">赚 币 排 行" + "</h>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("xk3swset.aspx?act=paihang&amp;ptype=2&amp;id=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">赚 币 排 行</a>" + "|");
        if (ptype == 3)
            builder.Append("<h style=\"color:red\">游 戏 狂 人" + "</h>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("xk3swset.aspx?act=paihang&amp;ptype=3&amp;id=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">游 戏 狂 人</a>" + "|");
        builder.Append(Out.Tab("</div>", "<br/>"));

        //builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));

        //string startstate = (Utils.GetRequest("startstate", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8])))$", "20150101"));
        //string endstate = (Utils.GetRequest("endstate", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8])))$", "20501231"));

        DateTime startstate = Utils.ParseTime(Utils.GetRequest("startstate2", "all", 1, DT.RegexTime, "2014-01-01 00:00:00"));
        DateTime endstate = Utils.ParseTime(Utils.GetRequest("endstate2", "all", 1, DT.RegexTime, "2115-01-01 00:00:00"));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");

        string rewardid = "";
        int pageIndex = 1;
        if (ptype == 1)
        {
            int recordCount;
            string strWhere = string.Empty;
            string strWhere2 = string.Empty;
            string strWhere3 = string.Empty;
            string strWhere4 = string.Empty;
            int pageSize = 10;
            string[] pageValUrl = { "act", "startstate", "endstate", "ptype", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            if (startstate.ToString() == "2014-01-01 00:00:00")
            {
                strWhere = "State>0 GROUP BY UsID ORDER BY Sum(GetMoney-PutGold) DESC";
            }
            else
                strWhere = "State>0 and Input_Time>='" + startstate + "' and Input_Time<='" + endstate + "' GROUP BY UsID ORDER BY Sum(GetMoney-PutGold) DESC";

            strWhere2 = "TOP(100) UsID,Sum(GetMoney-PutGold) as bb";

            strWhere3 = "UsID,sum(GetMoney-PutGold) AS'bb' into #bang3";
            strWhere4 = "WHERE " + strWhere + "";

            DataSet rowbang = new BCW.XinKuai3.BLL.XK3_Bet_SWB().GetList(strWhere2, strWhere);
            recordCount = rowbang.Tables[0].Rows.Count;
            DataSet bang = new BCW.XinKuai3.BLL.XK3_Bet_SWB().GetListByPage2(0, recordCount, strWhere3, strWhere4);
            if (recordCount > 0)
            {
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

                for (int soms = 0; soms < skt; soms++)
                {
                    int usid;
                    long usmoney;
                    usid = Convert.ToInt32(bang.Tables[0].Rows[koo + soms][1]);
                    usmoney = Convert.ToInt64(bang.Tables[0].Rows[koo + soms][2]);
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br/>"));
                    }
                    string mename = new BCW.BLL.User().GetUsName(usid);
                    int wd = (pageIndex - 1) * 10 + k;
                    builder.Append("[<h style=\"color:red\">第" + wd + "名</h>]<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + mename + "</a>净赢<h style=\"color:red\">" + usmoney + "</h>" + klb + "");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                    rewardid = rewardid + usid.ToString() + "#";
                }
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                //builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));

            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录.."));
            }
        }

        else if (ptype == 2)
        {
            int recordCount;
            string strWhere = string.Empty;
            string strWhere2 = string.Empty;
            string strWhere3 = string.Empty;
            string strWhere4 = string.Empty;
            int pageSize = 10;
            string[] pageValUrl = { "act", "startstate", "endstate", "ptype", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            if (startstate.ToString() == "2014-01-01 00:00:00")
            {
                strWhere = "GetMoney>0 GROUP BY UsID ORDER BY Sum(GetMoney) DESC";
            }
            else
                strWhere = "GetMoney>0 and Input_Time>='" + startstate + "' and Input_Time<='" + endstate + "' GROUP BY UsID ORDER BY Sum(GetMoney) DESC";

            strWhere2 = "TOP(100) UsID,Sum(GetMoney) as bb";

            strWhere3 = "UsID,sum(GetMoney) AS'bb' into #bang3";

            strWhere4 = "WHERE " + strWhere + "";

            DataSet rowbang = new BCW.XinKuai3.BLL.XK3_Bet_SWB().GetList(strWhere2, strWhere);
            recordCount = rowbang.Tables[0].Rows.Count;
            DataSet bang = new BCW.XinKuai3.BLL.XK3_Bet_SWB().GetListByPage2(0, recordCount, strWhere3, strWhere4);
            if (recordCount > 0)
            {
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

                for (int soms = 0; soms < skt; soms++)
                {
                    int usid;
                    long usmoney;
                    usid = Convert.ToInt32(bang.Tables[0].Rows[koo + soms][1]);
                    usmoney = Convert.ToInt64(bang.Tables[0].Rows[koo + soms][2]);
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br/>"));
                    }
                    string mename = new BCW.BLL.User().GetUsName(usid);
                    int wd = (pageIndex - 1) * 10 + k;
                    builder.Append("[<h style=\"color:red\">第" + wd + "名</h>]<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + mename + "</a>赚币<h style=\"color:red\">" + usmoney + "</h>" + klb + "");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                    rewardid = rewardid + usid.ToString() + "#";
                }
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                //builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录.."));
            }
        }

        else if (ptype == 3)
        {
            int recordCount;
            string strWhere = string.Empty;
            string strWhere2 = string.Empty;
            string strWhere3 = string.Empty;
            string strWhere4 = string.Empty;
            int pageSize = 10;
            string[] pageValUrl = { "act", "startstate", "endstate", "ptype", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            if (startstate.ToString() == "2014-01-01 00:00:00")
            {
                strWhere = "GROUP BY UsID ORDER BY count(UsID) DESC";
            }
            else
                strWhere = "Input_Time>='" + startstate + "' and Input_Time<='" + endstate + "' GROUP BY UsID ORDER BY count(UsID) DESC";

            strWhere2 = "TOP(100) UsID,count(UsID) as bb";

            strWhere3 = "UsID,count(UsID) AS'bb' into #bang3";

            strWhere4 = "WHERE " + strWhere + "";

            DataSet rowbang = new BCW.XinKuai3.BLL.XK3_Bet_SWB().GetList(strWhere2, strWhere);
            recordCount = rowbang.Tables[0].Rows.Count;
            DataSet bang = new BCW.XinKuai3.BLL.XK3_Bet_SWB().GetListByPage2(0, recordCount, strWhere3, strWhere4);
            if (recordCount > 0)
            {
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

                for (int soms = 0; soms < skt; soms++)
                {
                    int usid;
                    long usmoney;
                    usid = Convert.ToInt32(bang.Tables[0].Rows[koo + soms][1]);
                    usmoney = Convert.ToInt64(bang.Tables[0].Rows[koo + soms][2]);
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br/>"));
                    }
                    string mename = new BCW.BLL.User().GetUsName(usid);
                    int wd = (pageIndex - 1) * 10 + k;
                    builder.Append("[<h style=\"color:red\">第" + wd + "名</h>]<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + mename + "</a>共玩<h style=\"color:red\">" + usmoney + "</h>次");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                    rewardid = rewardid + usid.ToString() + "#";
                }
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                //builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
                //builder.Append(Out.Tab("<div></div>", Out.Hr()));
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录.."));
            }
        }
        //builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        string strText = "开始日期：/,结束日期：/,";
        string strName = "startstate2,endstate2,backurl";
        string strType = "date,date,hidden";
        string strValu = string.Empty;
        if (Utils.ToSChinese(ac) != "马上查询")
        {
            strValu = "" + DT.FormatDate(DateTime.Now.AddDays(-1), 0) + "'" + DT.FormatDate(DateTime.Now, 0) + "'" + Utils.getPage(0) + "";
        }
        else
        {
            strValu = "" + DT.FormatDate(startstate, 0) + "'" + DT.FormatDate(endstate, 0) + "'" + Utils.getPage(0) + "";
        }
        string strEmpt = "false,false,false";
        string strIdea = "/";
        string strOthe = "马上查询,xk3swset.aspx?act=paihang&amp;ptype=" + ptype + ",post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));


        //builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));


        if (Utils.ToSChinese(ac) != "马上查询")
        {
            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append("排行榜奖励提示：<br/>");
            builder.Append("如需发放奖励，请按日期查询.");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            string wdy = "";
            if (pageIndex == 1)
                wdy = "TOP10";
            else
                wdy = "TOP" + (pageIndex - 1).ToString() + "1-" + pageIndex.ToString() + "0";

            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append(wdy + " 的用户ID分别是：" + rewardid);
            builder.Append(Out.Tab("</div>", ""));
            string strText2 = ",,,,";
            string strName2 = "startstate,endstate,pageIndex,rewardid,backurl";
            string strType2 = "hidden,hidden,hidden,hidden,hidden";
            string strValu2 = DT.FormatDate(startstate, 0) + "'" + DT.FormatDate(endstate, 0) + "'" + pageIndex + "'" + rewardid + "'" + Utils.getPage(0) + "";
            string strEmpt2 = "true,true,false";
            string strIdea2 = "/";
            string strOthe2 = wdy + "奖励发放,xk3swset.aspx?act=ReWard&amp;ptype=" + ptype + ",post,1,red";
            builder.Append(Out.wapform(strText2, strName2, strType2, strValu2, strEmpt2, strIdea2, strOthe2));
        }

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("xk3swset.aspx") + "\">&lt;&lt;&lt;返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

    }

    //==========用户购买情况和获奖数据情况========
    private void FenxiPage()
    {
        Master.Title = "" + GameName + "_用户下注情况";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("xk3swset.aspx") + "\">" + GameName + "</a>&gt;用户下注情况");
        builder.Append(Out.Tab("</div>", "<br />"));

        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-5]$", "1"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 1)
        {
            builder.Append("<h style=\"color:red\">全部购买情况" + "</h>" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("xk3swset.aspx?act=fenxi&amp;ptype=1&amp;id=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">全部购买情况</a>" + "|");
        }
        if (ptype == 2)
        {
            builder.Append("<h style=\"color:red\">获奖数据" + "</h>" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("xk3swset.aspx?act=fenxi&amp;ptype=2&amp;id=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">获奖数据</a>" + "|");
        }
        if (ptype == 3)
        {
            builder.Append("<h style=\"color:red\">系统没收数据" + "</h>" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("xk3swset.aspx?act=fenxi&amp;ptype=3&amp;id=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">系统没收数据</a>" + "|");
        }
        if (ptype == 4)
        {
            builder.Append("<h style=\"color:red\">不中奖" + "</h>" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("xk3swset.aspx?act=fenxi&amp;ptype=4&amp;id=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">不中奖</a>" + "|");
        }
        if (ptype == 5)
        {
            builder.Append("<h style=\"color:red\">未开奖" + "</h>" + "");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("xk3swset.aspx?act=fenxi&amp;ptype=5&amp;id=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">未开奖</a>" + "");
        }
        builder.Append(Out.Tab("</div>", "<br />"));


        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        int qihaos = int.Parse(Utils.GetRequest("qihaos", "all", 1, @"^[1-9]\d*$", "0"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo2", xmlPath));
        string strWhere = " ";
        string[] pageValUrl = { "act", "uid", "qihaos", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //string strOrder = "";

        if (ptype == 1)
        {
            if (uid > 0)
            {
                if (qihaos > 0)
                {
                    strWhere = "UsID=" + uid + " and Lottery_issue=" + qihaos + "";
                }
                else
                    strWhere = "UsID=" + uid + "";
            }
            else if (qihaos > 0)
            {
                strWhere = "Lottery_issue=" + qihaos + "";
            }
            else
                strWhere = "";
        }
        else if (ptype == 2)
        {
            if (uid > 0)
            {
                if (qihaos > 0)
                {
                    strWhere = "State=2 and UsID=" + uid + " and Lottery_issue=" + qihaos + "";
                }
                else
                    strWhere = "State=2 and UsID=" + uid + "";
            }
            else if (qihaos > 0)
            {
                strWhere = "State=2 and Lottery_issue=" + qihaos + "";
            }
            else
                strWhere = "State=2";
        }
        else if (ptype == 3)
        {
            if (uid > 0)
            {
                if (qihaos > 0)
                {
                    strWhere = "State=3 and UsID=" + uid + " and Lottery_issue=" + qihaos + "";
                }
                else
                    strWhere = "State=3 and UsID=" + uid + "";
            }
            else if (qihaos > 0)
            {
                strWhere = "State=3 and Lottery_issue=" + qihaos + "";
            }
            else
                strWhere = "State=3";
        }
        else if (ptype == 4)
        {
            if (uid > 0)
            {
                if (qihaos > 0)
                {
                    strWhere = "State=1 and UsID=" + uid + " and Lottery_issue=" + qihaos + "";
                }
                else
                    strWhere = "State=1 and UsID=" + uid + "";
            }
            else if (qihaos > 0)
            {
                strWhere = "State=1 and Lottery_issue=" + qihaos + "";
            }
            else
                strWhere = "State=1";
        }
        else if (ptype == 5)
        {
            if (uid > 0)
            {
                if (qihaos > 0)
                {
                    strWhere = "State=0 and UsID=" + uid + " and Lottery_issue=" + qihaos + "";
                }
                else
                    strWhere = "State=0 and UsID=" + uid + "";
            }
            else if (qihaos > 0)
            {
                strWhere = "State=0 and Lottery_issue=" + qihaos + "";
            }
            else
                strWhere = "State=0";
        }

        // 开始读取列表
        IList<BCW.XinKuai3.Model.XK3_Bet_SWB> listXK3pay = new BCW.XinKuai3.BLL.XK3_Bet_SWB().GetXK3_Bet_SWBs(pageIndex, pageSize, strWhere, out recordCount);

        if (listXK3pay.Count > 0)
        {
            int k = 1;
            foreach (BCW.XinKuai3.Model.XK3_Bet_SWB n in listXK3pay)
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
                string Gettou = string.Empty;
                if (n.Play_Way == 1)
                {
                    Gettou = n.Sum;
                }
                if (n.Play_Way == 2)
                {
                    Gettou = n.Three_Same_All;
                }
                if (n.Play_Way == 3)
                {
                    Gettou = n.Three_Same_Single;
                }
                if (n.Play_Way == 4)
                {
                    Gettou = n.Three_Same_Not;
                }
                if (n.Play_Way == 5)
                {
                    Gettou = n.Three_Continue_All;
                }
                if (n.Play_Way == 6)
                {
                    Gettou = n.Two_Same_All;
                }
                if (n.Play_Way == 7)
                {
                    Gettou = n.Two_Same_Single;
                }
                if (n.Play_Way == 8)
                {
                    Gettou = n.Two_dissame;
                }
                if (n.Play_Way == 9)
                {
                    //1大2小、1单2双
                    if (n.DaXiao == "1")
                    {
                        Gettou = "大";
                    }
                    else if (n.DaXiao == "2")
                    {
                        Gettou = "小";
                    }
                }
                if (n.Play_Way == 10)
                {
                    if (n.DanShuang == "2")
                    {
                        Gettou = "双";
                    }
                    else if (n.DanShuang == "1")
                    {
                        Gettou = "单";
                    }
                }
                //获取id对应的用户名
                string mename = new BCW.BLL.User().GetUsName(n.UsID);
                builder.Append("[" + ((pageIndex - 1) * pageSize + k) + "].<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + mename + "</a>." + (n.Lottery_issue) + "期");
                BCW.XinKuai3.Model.XK3_Internet_Data model_num1 = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3kainum(n.Lottery_issue);
                if (model_num1.Lottery_num != "")
                {
                    builder.Append("&lt;" + model_num1.Lottery_num + ">");
                }
                builder.Append("[" + OutType(n.Play_Way) + "]为:{" + Gettou + "}每注" + n.Zhu_money + "" + klb + "/共" + n.Zhu + "注/共投" + n.PutGold + klb + "[" + n.Input_Time + "].");//DT.FormatDate(n.Input_Time, 1)//号码
                if (n.GetMoney > 0)
                {
                    if (n.State == 2)
                    {
                        builder.Append("<h style=\"color:red\">[赢" + n.GetMoney + "" + klb + "]</h>.(已领奖)");
                        builder.AppendFormat("<a href=\"" + Utils.getUrl("xk3swset.aspx?act=fenxi&amp;id=" + ptype + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>");
                    }
                    else if (n.State == 3)
                    {
                        builder.Append("<h style=\"color:red\">[赢" + n.GetMoney + "" + klb + "]</h>.(过期未领奖)");
                        builder.AppendFormat("<a href=\"" + Utils.getUrl("xk3swset.aspx?act=fenxi&amp;id=" + ptype + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>");
                    }
                    else
                    {
                        builder.Append("<h style=\"color:red\">[赢" + n.GetMoney + "" + klb + "]</h>.(未领奖)");
                        builder.AppendFormat("<a href=\"" + Utils.getUrl("xk3swset.aspx?act=fenxi&amp;id=" + ptype + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>");
                    }
                }
                if (n.GetMoney == 0)
                {
                    if (n.State == 0)
                    {
                        builder.Append("<h style=\"color:blue\">(未开奖)</h>");
                        builder.AppendFormat("<a href=\"" + Utils.getUrl("xk3swset.aspx?act=fenxi&amp;id=" + ptype + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>");
                    }
                    else if (n.State == 1)
                    {
                        builder.Append("<h style=\"color:green\">(不中奖)</h>");
                        builder.AppendFormat("<a href=\"" + Utils.getUrl("xk3swset.aspx?act=fenxi&amp;id=" + ptype + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>");
                    }
                }
                builder.Append(".<a href=\"" + Utils.getUrl("xk3swset.aspx?act=del&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[删]</a>");
                if (ptype == 3)
                {
                    builder.Append(".<a href=\"" + Utils.getUrl("xk3swset.aspx?act=edit&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[改]</a>");
                }
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


        //builder.Append(Out.Tab("<div>", ""));
        //builder.Append("<hr/>");
        builder.Append(Out.Tab("", Out.Hr()));
        string strText = "输入用户ID(可为空):/,输入彩票期号(可为空):/,";
        string strName = "uid,qihaos,backurl";
        string strType = "num,num,hidden";
        string strValu = "'" + "'" + Utils.getPage(0) + "";
        string strEmpt = "true,true,false";
        string strIdea = "/";
        string strOthe = "搜一搜,xk3swset.aspx?act=fenxi&amp;ptype=" + ptype + ",post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div>", Out.Hr()));
        //builder.Append("<hr/>");
        builder.Append("<a href=\"" + Utils.getPage("xk3swset.aspx") + "\">&lt;&lt;&lt;返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //==========手动添加开奖数字========================试玩版不用
    private void AddPage()
    {
        Master.Title = "" + GameName + "_人工开奖";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("xk3swset.aspx") + "\">" + GameName + "</a>&gt;人工开奖");
        builder.Append(Out.Tab("</div>", "<br />"));

        string Lottery_issue = (Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("需要手动开奖的期号为：" + Lottery_issue + "<br/>");
        builder.Append(Out.Tab("</div>", "<br />"));

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            BCW.XinKuai3.Model.XK3_Internet_Data model = new BCW.XinKuai3.Model.XK3_Internet_Data();
            string num = Utils.GetRequest("num", "post", 2, @"^[1-6][1-6][1-6]$", "填写出错");

            //加分隔符,
            string a = num[0] + "," + num[1] + "," + num[2];

            string[] str1 = a.Split(',');
            string t1 = str1[0];
            string t2 = str1[1];
            string t3 = str1[2];

            string _where2 = string.Empty;
            _where2 = "'" + Lottery_issue + "' Order by id desc";

            BCW.XinKuai3.Model.XK3_Internet_Data model_getTime = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3all_num2(_where2);
            try
            {
                DateTime getTime = model_getTime.Lottery_time;
                model.Lottery_time = getTime;
            }
            catch
            {
                model.Lottery_time = DateTime.Now;
            }
            model.Lottery_issue = Lottery_issue;
            model.Lottery_num = a;
            //大小单双
            if (((t1 == t2) && (t1 == t3)))//大小双单通食
            {
                model.DaXiao = "0";
                model.DanShuang = "0";
            }
            else
            {
                if ((Int32.Parse(str1[0])) + (Int32.Parse(str1[1])) + (Int32.Parse(str1[2])) <= 10)
                {
                    model.DaXiao = "1";//和值开出是4-10,即为小

                }
                else
                {
                    model.DaXiao = "2";//和值开出是11-17,即为大

                }
                if (((Int32.Parse(str1[0])) + (Int32.Parse(str1[1])) + (Int32.Parse(str1[2]))) % 2 == 1)
                {
                    model.DanShuang = "1";//单数
                }
                else if (((Int32.Parse(str1[0])) + (Int32.Parse(str1[1])) + (Int32.Parse(str1[2]))) % 2 == 0)
                {
                    model.DanShuang = "2";//双数
                }
            }

            //和值
            string sum1 = (Int32.Parse(str1[0]) + Int32.Parse(str1[1]) + Int32.Parse(str1[2])).ToString();
            model.Sum = sum1;
            //三同号通选+三同号单选
            if (((t1 == t2) && (t1 == t3)))
            {
                model.Three_Same_All = "1";
                model.Three_Same_Single = t1 + t2 + t3;
                //model.Three_Continue_All = "1";
            }
            else
            {
                model.Three_Same_All = "0";
                model.Three_Same_Single = "0";
                //model.Three_Continue_All = "0";
            }
            //三不同号
            if ((t1 != t2) && (t1 != t3) && (t2 != t3))
            {
                model.Three_Same_Not = t1 + t2 + t3;
            }
            else
            {
                model.Three_Same_Not = "0";
            }
            //三连号通选
            //if ((Int32.Parse(str1[1]) == (Int32.Parse(str1[0]) + 1)) && (Int32.Parse(str1[2]) == (Int32.Parse(str1[1]) + 1)))
            if ((Int32.Parse(str1[1]) == (Int32.Parse(str1[0]) + 1)) && (Int32.Parse(str1[1]) == (Int32.Parse(str1[2]) - 1)))
            {
                //model.Three_Continue_All = t1 + t2 + t3;
                model.Three_Continue_All = "1";
            }
            else
            {
                model.Three_Continue_All = "0";
            }
            //二同号复选
            if ((t1 == t2) || (t2 == t3))
            {
                model.Two_Same_All = t2 + t2;

            }
            else
            {
                model.Two_Same_All = "0";

            }
            //二同号单选
            if ((t1 == t2) || (t2 == t3))
            {
                model.Two_Same_Single = t1 + t2 + t3;
            }
            else if ((t1 == t2) && (t1 == t3))
            {
                model.Two_Same_Single = "0";
            }
            else
            {
                model.Two_Same_Single = "0";
            }
            //二不同号
            if ((t1 != t2) && (t2 != t3))
            {
                model.Two_dissame = (t1 + t2) + "," + (t1 + t3) + "," + (t2 + t3);
            }
            else if ((t1 == t2) && (t2 != t3))
            {
                model.Two_dissame = t2 + t3;
            }
            else if ((t1 != t2) && (t2 == t3))
            {
                model.Two_dissame = t1 + t2;
            }
            else
            {
                model.Two_dissame = "0";
            }
            model.UpdateTime = DateTime.Now;
            new BCW.XinKuai3.BLL.XK3_Internet_Data().Update_num2(model);
            Open_price();//返奖
            change_peilv();//根据最近几期的大小单双开奖情况，实时变动赔率。
            Utils.Success("" + GameName + "_人工开奖", "人工开奖成功，正在返回..", Utils.getUrl("xk3swset.aspx?backurl=" + Utils.getPage(0) + ""), "1");
            //builder.Append("================" + a);
        }
        else
        {
            string strText = "输入开奖号码：/,";
            string strName = "num," + Utils.getPage(0) + "";
            string strType = "num,hidden";
            string strValu = "'" + Utils.getPage(0) + "";
            string strEmpt = "true,false";
            string strIdea = "/";
            string strOthe = "确定修改,xk3swset.aspx?act=add&amp;id=" + Lottery_issue + ",post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }


        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<hr/>");
        builder.Append("温馨提示：请在输入框输入需要开奖的号码。如：123<br/>");
        builder.Append("<a href=\"" + Utils.getUrl("xk3swset.aspx") + "\">&lt;&lt;&lt;返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //==========开始返彩================================试玩版不用
    public void Open_price()
    {
        //检查数据库最后一期开奖号码
        BCW.XinKuai3.Model.XK3_Internet_Data model_1 = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3listLast2();//网络开奖数据
        string qihao = model_1.Lottery_issue;
        string kai = model_1.Lottery_num;
        //Response.Write("===============数据库最后一期的开奖数据===============" + qihao + "：" + kai + "<br/>");
        if (kai != "")//如果开奖号码为空，则不返奖
        {
            int sum = Int32.Parse(model_1.Sum);
            int Odds = 0;//和值的倍数
            if ((sum == 4) || (sum == 17))
            {
                Odds = Utils.ParseInt(ub.GetSub("XinKuai3Sum2", xmlPath));
            }
            else if ((sum == 5) || (sum == 16))
            {
                Odds = Utils.ParseInt(ub.GetSub("XSum1", xmlPath));
            }
            else if ((sum == 6) || (sum == 15))
            {
                Odds = Utils.ParseInt(ub.GetSub("XSum2", xmlPath));
            }
            else if ((sum == 7) || (sum == 14))
            {
                Odds = Utils.ParseInt(ub.GetSub("XSum3", xmlPath));
            }
            else if ((sum == 8) || (sum == 13))
            {
                Odds = Utils.ParseInt(ub.GetSub("XSum4", xmlPath));
            }
            else if ((sum == 9) || (sum == 12))
            {
                Odds = Utils.ParseInt(ub.GetSub("XSum5", xmlPath));
            }
            else if ((sum == 10) || (sum == 11))
            {
                Odds = Utils.ParseInt(ub.GetSub("XinKuai3Sum1", xmlPath));
            }


            //检查投注表是否存在没开奖数据
            if (new BCW.XinKuai3.BLL.XK3_Bet_SWB().Exists_num(qihao))
            {
                Response.Write("<br />&nbsp;&nbsp;&nbsp;&nbsp;==============oh shit  有未兑奖的==========<b></b><br /><br />");
                DataSet ds = new BCW.XinKuai3.BLL.XK3_Bet_SWB().GetList("*", "State=0");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        //本地投注数据
                        int ID = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                        int UsID = int.Parse(ds.Tables[0].Rows[i]["UsID"].ToString());
                        int Play_Way = int.Parse(ds.Tables[0].Rows[i]["Play_Way"].ToString());
                        string Sum = ds.Tables[0].Rows[i]["Sum"].ToString();
                        string Three_Same_All = ds.Tables[0].Rows[i]["Three_Same_All"].ToString();
                        string Three_Same_Single = ds.Tables[0].Rows[i]["Three_Same_Single"].ToString();
                        string Three_Same_Not = ds.Tables[0].Rows[i]["Three_Same_Not"].ToString();
                        string Three_Continue_All = ds.Tables[0].Rows[i]["Three_Continue_All"].ToString();
                        string Two_Same_All = ds.Tables[0].Rows[i]["Two_Same_All"].ToString();
                        string Two_Same_Single = ds.Tables[0].Rows[i]["Two_Same_Single"].ToString();
                        string Two_dissame = ds.Tables[0].Rows[i]["Two_dissame"].ToString();
                        string Lottery_issue = ds.Tables[0].Rows[i]["Lottery_issue"].ToString();
                        string DanTuo = ds.Tables[0].Rows[i]["DanTuo"].ToString();
                        int Zhu = int.Parse(ds.Tables[0].Rows[i]["Zhu"].ToString());
                        int State = int.Parse(ds.Tables[0].Rows[i]["State"].ToString());
                        long Zhu_money = Int64.Parse(ds.Tables[0].Rows[i]["Zhu_money"].ToString());
                        long PutGold = Int64.Parse(ds.Tables[0].Rows[i]["PutGold"].ToString());
                        long GetMoney = Int64.Parse(ds.Tables[0].Rows[i]["GetMoney"].ToString());
                        string DaXiao = (ds.Tables[0].Rows[i]["DaXiao"].ToString());
                        string DanShuang = (ds.Tables[0].Rows[i]["DanShuang"].ToString());
                        float _odds = float.Parse(ds.Tables[0].Rows[i]["Odds"].ToString());
                        string mename = new BCW.BLL.User().GetUsName(UsID);//获得id对应的用户名

                        if (Play_Way == 1)//和值
                        {
                            if (Sum.Contains(model_1.Sum))
                            {
                                long WinCent = Convert.ToInt64(Zhu_money * Odds);
                                new BCW.XinKuai3.BLL.XK3_Bet_SWB().Update_win(ID, WinCent);
                                new BCW.BLL.Guest().Add(1, UsID, mename, "恭喜您在" + GameName + "_试玩版第" + Lottery_issue + "期获得了" + WinCent + "" + klb + "[url=/bbs/game/xk3sw.aspx?act=case]马上兑奖[/url]");//开奖提示信息,1表示开奖信息

                                string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + mename + "[/url]在[url=/bbs/game/xk3sw.aspx]" + GameName + "[/url]《" + Lottery_issue + "期.和值投注》获得了" + WinCent + "" + klb + "";
                                new BCW.BLL.Action().Add(1002, ID, UsID, "", wText);
                            }
                        }
                        if (Play_Way == 2)//三同号通选
                        {
                            if (model_1.Three_Same_All == "1")
                            {
                                Odds = Utils.ParseInt(ub.GetSub("XinKuai3Three_Same_All", xmlPath));
                                long WinCent = Convert.ToInt64(Zhu_money * Odds);
                                new BCW.XinKuai3.BLL.XK3_Bet_SWB().Update_win(ID, WinCent);
                                new BCW.BLL.Guest().Add(1, UsID, mename, "恭喜您在" + GameName + "_试玩版第" + Lottery_issue + "期获得了" + WinCent + "" + klb + "[url=/bbs/game/xk3sw.aspx?act=case]马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                                string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + mename + "[/url]在[url=/bbs/game/xk3sw.aspx]" + GameName + "[/url]《" + Lottery_issue + "期.三同号通选投注》获得了" + WinCent + "" + klb + "";
                                new BCW.BLL.Action().Add(1002, ID, UsID, "", wText);
                            }
                        }
                        if (Play_Way == 3)//三同号单选
                        {
                            if (Three_Same_Single.Contains(model_1.Three_Same_Single))
                            {
                                Odds = Utils.ParseInt(ub.GetSub("XinKuai3Three_Same_Single", xmlPath));
                                long WinCent = Convert.ToInt64(Zhu_money * Odds);
                                new BCW.XinKuai3.BLL.XK3_Bet_SWB().Update_win(ID, WinCent);
                                new BCW.BLL.Guest().Add(1, UsID, mename, "恭喜您在" + GameName + "_试玩版第" + Lottery_issue + "期获得了" + WinCent + "" + klb + "[url=/bbs/game/xk3sw.aspx?act=case]马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                                string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + mename + "[/url]在[url=/bbs/game/xk3sw.aspx]" + GameName + "[/url]《" + Lottery_issue + "期.三同号单选投注》获得了" + WinCent + "" + klb + "";
                                new BCW.BLL.Action().Add(1002, ID, UsID, "", wText);
                            }
                        }
                        if (Play_Way == 4)//三不同号
                        {
                            if (Three_Same_Not.Contains(model_1.Three_Same_Not))
                            {
                                Odds = Utils.ParseInt(ub.GetSub("XinKuai3Three_Same_Not", xmlPath));
                                long WinCent = Convert.ToInt64(Zhu_money * Odds);
                                new BCW.XinKuai3.BLL.XK3_Bet_SWB().Update_win(ID, WinCent);
                                new BCW.BLL.Guest().Add(1, UsID, mename, "恭喜您在" + GameName + "_试玩版第" + Lottery_issue + "期获得了" + WinCent + "" + klb + "[url=/bbs/game/xk3sw.aspx?act=case]马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                                string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + mename + "[/url]在[url=/bbs/game/xk3sw.aspx]" + GameName + "[/url]《" + Lottery_issue + "期.三不同号投注》获得了" + WinCent + "" + klb + "";
                                new BCW.BLL.Action().Add(1002, ID, UsID, "", wText);
                            }
                        }
                        if (Play_Way == 5)//三连号通选
                        {
                            if (model_1.Three_Continue_All == "1")
                            {
                                Odds = Utils.ParseInt(ub.GetSub("XinKuai3Three_Continue_All", xmlPath));
                                long WinCent = Convert.ToInt64(Zhu_money * Odds);
                                new BCW.XinKuai3.BLL.XK3_Bet_SWB().Update_win(ID, WinCent);
                                new BCW.BLL.Guest().Add(1, UsID, mename, "恭喜您在" + GameName + "_试玩版第" + Lottery_issue + "期获得了" + WinCent + "" + klb + "[url=/bbs/game/xk3sw.aspx?act=case]马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                                string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + mename + "[/url]在[url=/bbs/game/xk3sw.aspx]" + GameName + "[/url]《" + Lottery_issue + "期.三连号通选投注》获得了" + WinCent + "" + klb + "";
                                new BCW.BLL.Action().Add(1002, ID, UsID, "", wText);
                            }
                        }
                        if (Play_Way == 6)//二同号复选
                        {
                            if (Two_Same_All.Contains(model_1.Two_Same_All))
                            {
                                Odds = Utils.ParseInt(ub.GetSub("XinKuai3Two_Same_All", xmlPath));
                                long WinCent = Convert.ToInt64(Zhu_money * Odds);
                                new BCW.XinKuai3.BLL.XK3_Bet_SWB().Update_win(ID, WinCent);
                                new BCW.BLL.Guest().Add(1, UsID, mename, "恭喜您在" + GameName + "_试玩版第" + Lottery_issue + "期获得了" + WinCent + "" + klb + "[url=/bbs/game/xk3sw.aspx?act=case]马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                                string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + mename + "[/url]在[url=/bbs/game/xk3sw.aspx]" + GameName + "[/url]《" + Lottery_issue + "期.二同号复选投注》获得了" + WinCent + "" + klb + "";
                                new BCW.BLL.Action().Add(1002, ID, UsID, "", wText);
                            }
                        }
                        if (Play_Way == 7)//二同号单选
                        {
                            string[] a1 = Two_Same_Single.Split(',');//分割购买的数据 {221,223,224,551,553,554}
                            int e = 0;
                            for (int p = 0; p < a1.Length; p++)
                            {
                                //各自赋值给y
                                int y = Convert.ToInt32(a1[p]);
                                int y1 = y / 100;
                                int y2 = (y - y1 * 100) / 10;
                                int y3 = (y - y1 * 100 - y2 * 10);
                                int[] num3 = { y1, y2, y3 };

                                //冒泡排序 从小到大
                                for (int t = 0; t < 3; t++)
                                {
                                    for (int j = t + 1; j < 3; j++)
                                    {
                                        if (num3[j] < num3[t])
                                        {
                                            int temp = num3[t];
                                            num3[t] = num3[j];
                                            num3[j] = temp;
                                        }
                                    }
                                }
                                string num22 = string.Empty;
                                for (int w = 0; w < 3; w++)//遍历数组显示结果
                                {
                                    num22 = num22 + num3[w];
                                }
                                if (num22.Contains(model_1.Two_Same_Single))
                                {
                                    e++;
                                }
                                if (e > 0)
                                {
                                    break;
                                }
                            }
                            //if (Two_Same_Single.Contains(model_1.Two_Same_Single))
                            if (e > 0)
                            {
                                Odds = Utils.ParseInt(ub.GetSub("XinKuai3Two_Same_Single", xmlPath));
                                long WinCent = Convert.ToInt64(Zhu_money * Odds);
                                new BCW.XinKuai3.BLL.XK3_Bet_SWB().Update_win(ID, WinCent);
                                new BCW.BLL.Guest().Add(1, UsID, mename, "恭喜您在" + GameName + "_试玩版第" + Lottery_issue + "期获得了" + WinCent + "" + klb + "[url=/bbs/game/xk3sw.aspx?act=case]马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                                string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + mename + "[/url]在[url=/bbs/game/xk3sw.aspx]" + GameName + "[/url]《" + Lottery_issue + "期.二同号单选投注》获得了" + WinCent + "" + klb + "";
                                new BCW.BLL.Action().Add(1002, ID, UsID, "", wText);
                            }
                        }
                        if (Play_Way == 8)//二不同号
                        {
                            string[] tt = (model_1.Two_dissame).Split(',');//开奖的数据
                            string[] bb = Two_dissame.Split(',');
                            int a = tt.Length;
                            int b = bb.Length;
                            int j = 0;

                            for (int y = 0; y <= tt.Length - 1; y++)
                            {
                                for (int ii = 0; ii <= bb.Length - 1; ii++)
                                {
                                    if (bb[ii].Contains(tt[y]))
                                    {
                                        j++;
                                    }
                                }
                            }
                            Odds = Utils.ParseInt(ub.GetSub("XinKuai3Two_dissame", xmlPath));
                            long WinCent = Convert.ToInt64(Zhu_money * Odds * j);
                            new BCW.XinKuai3.BLL.XK3_Bet_SWB().Update_win(ID, WinCent);
                            new BCW.BLL.Guest().Add(1, UsID, mename, "恭喜您在" + GameName + "_试玩版第" + Lottery_issue + "期获得了" + WinCent + "" + klb + "[url=/bbs/game/xk3sw.aspx?act=case]马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                            string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + mename + "[/url]在[url=/bbs/game/xk3sw.aspx]" + GameName + "[/url]《" + Lottery_issue + "期.二不同号投注》获得了" + WinCent + "" + klb + "";
                            new BCW.BLL.Action().Add(1002, ID, UsID, "", wText);

                        }
                        if (Play_Way == 9)//大小
                        {
                            if (DaXiao == (model_1.DaXiao))
                            {
                                long WinCent = Convert.ToInt64(Zhu_money * _odds);
                                new BCW.XinKuai3.BLL.XK3_Bet_SWB().Update_win(ID, WinCent);
                                new BCW.BLL.Guest().Add(1, UsID, mename, "恭喜您在" + GameName + "_试玩版第" + Lottery_issue + "期获得了" + WinCent + "" + klb + "[url=/bbs/game/xk3sw.aspx?act=case]马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                                string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + mename + "[/url]在[url=/bbs/game/xk3sw.aspx]" + GameName + "[/url]《" + Lottery_issue + "期.大小投注》获得了" + WinCent + "" + klb + "";
                                new BCW.BLL.Action().Add(1002, ID, UsID, "", wText);
                            }
                        }
                        if (Play_Way == 10)//单双
                        {
                            if (DanShuang == (model_1.DanShuang))
                            {
                                long WinCent = Convert.ToInt64(Zhu_money * _odds);
                                new BCW.XinKuai3.BLL.XK3_Bet_SWB().Update_win(ID, WinCent);
                                new BCW.BLL.Guest().Add(1, UsID, mename, "恭喜您在" + GameName + "_试玩版第" + Lottery_issue + "期获得了" + WinCent + "" + klb + "[url=/bbs/game/xk3sw.aspx?act=case]马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                                string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + mename + "[/url]在[url=/bbs/game/xk3sw.aspx]" + GameName + "[/url]《" + Lottery_issue + "期.单双投注》获得了" + WinCent + "" + klb + "";
                                new BCW.BLL.Action().Add(1002, ID, UsID, "", wText);
                            }
                        }
                        new BCW.XinKuai3.BLL.XK3_Bet_SWB().UpdateState(ID, 1);
                    }
                }

            }
        }
        else
        {
            //此次应该通知管理员：第几期有期号，而没开奖号码。
            Response.Write("<br/><br/><b>《《《请注意：第" + qihao + "期因没有开奖号码，返奖失败。。。》》》</b><br/><br/>");
        }
    }

    //根据最近几期的大小单双开奖情况，实时变动赔率。====试玩版不用
    public void change_peilv()
    {
        BCW.XinKuai3.Model.XK3_Internet_Data model_1 = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3listLast3();//网络开奖数据
        string qihao = model_1.Lottery_issue;
        string kai = model_1.Lottery_num;

        ub xml = new ub();
        string xmlPath_update = "/Controls/xinkuai3_TRIAL_GAME.xml";
        Application.Remove(xmlPath_update);//清缓存
        xml.ReloadSub(xmlPath_update); //加载配置

        if (kai != "" && qihao != "")
        {
            string issue3 = Utils.Right(model_1.Lottery_issue.ToString(), 3);//本期开奖期号的后3位
            if (issue3 != "001")
            {
                DataSet d1, d2;
                d1 = new BCW.XinKuai3.BLL.XK3_Internet_Data().GetList("TOP 1 *", "DaXiao!='' ORDER BY Lottery_time DESC");
                d2 = new BCW.XinKuai3.BLL.XK3_Internet_Data().GetList2("TOP 1 *", "id!='' ORDER BY Lottery_time ASC ");

                string Cents1 = "";
                string Cents2 = "";
                string Cents3 = "";
                string Cents4 = "";
                for (int i = 0; i < d1.Tables[0].Rows.Count; i++)
                {
                    try
                    {
                        Cents1 = Convert.ToString(d1.Tables[0].Rows[i]["DaXiao"]);//最后一期的大小
                        Cents2 = Convert.ToString(d1.Tables[0].Rows[i]["DanShuang"]);//最后一期的单双
                    }
                    catch
                    {
                        Cents1 = "";
                        Cents2 = "";
                    }
                }
                for (int i = 0; i < d2.Tables[0].Rows.Count; i++)
                {
                    try
                    {
                        Cents3 = Convert.ToString(d2.Tables[0].Rows[i]["DaXiao"]);//倒数第2期的大小
                        Cents4 = Convert.ToString(d2.Tables[0].Rows[i]["DanShuang"]);//倒数第2期的单双
                    }
                    catch
                    {
                        Cents3 = "";
                        Cents4 = "";
                    }
                }
                if (Cents1 != Cents3)//如果连续2期不相等，还原赔率----大小
                {
                    xml.dss["Xda1"] = xml.dss["Xda"];
                    xml.dss["Xxiao1"] = xml.dss["Xxiao"];
                    System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                }
                else
                {
                    if (Cents1 == "1" && Cents3 == "1")//如果连续2期开小
                    {
                        xml.dss["Xxiao1"] = Convert.ToDouble(xml.dss["Xxiao1"]) + Convert.ToDouble(xml.dss["Xfudong"]);//小的赔率增加
                        xml.dss["Xda1"] = Convert.ToDouble(xml.dss["Xda1"]) - Convert.ToDouble(xml.dss["Xfudong"]);//大的赔率减少
                        System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                    }
                    else if (Cents1 == "2" && Cents3 == "2")//大
                    {
                        xml.dss["Xxiao1"] = Convert.ToDouble(xml.dss["Xxiao1"]) - Convert.ToDouble(xml.dss["Xfudong"]);//小的赔率减少
                        xml.dss["Xda1"] = Convert.ToDouble(xml.dss["Xda1"]) + Convert.ToDouble(xml.dss["Xfudong"]);//大的赔率增加
                        System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                    }
                }
                if (Cents2 != Cents4)//如果连续2期不相等，还原赔率----单双
                {
                    xml.dss["Xdan1"] = xml.dss["Xdan"];
                    xml.dss["Xshuang1"] = xml.dss["Xshuang"];
                    System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                }
                else
                {
                    if (Cents2 == "1" && Cents4 == "1")//如果连续2期开单
                    {
                        xml.dss["Xdan1"] = Convert.ToDouble(xml.dss["Xdan1"]) + Convert.ToDouble(xml.dss["Xfudong"]);//单的赔率增加
                        xml.dss["Xshuang1"] = Convert.ToDouble(xml.dss["Xshuang1"]) - Convert.ToDouble(xml.dss["Xfudong"]);//双的赔率减少
                        System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                    }
                    else if (Cents2 == "2" && Cents4 == "2")//双
                    {
                        xml.dss["Xdan1"] = Convert.ToDouble(xml.dss["Xdan1"]) - Convert.ToDouble(xml.dss["Xfudong"]);//单的赔率减少
                        xml.dss["Xshuang1"] = Convert.ToDouble(xml.dss["Xshuang1"]) + Convert.ToDouble(xml.dss["Xfudong"]);//双的赔率增加
                        System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                    }
                }
            }
        }
        //如果最后一期没有开奖期号和开奖号码，则大小单双的赔率重置
        else
        {
            xml.dss["Xda1"] = xml.dss["Xda"];
            xml.dss["Xxiao1"] = xml.dss["Xxiao"];
            xml.dss["Xdan1"] = xml.dss["Xdan"];
            xml.dss["Xshuang1"] = xml.dss["Xshuang"];
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
        }
    }

    //==========传说中的机器人==========================试玩版不用
    private void RobotPage()
    {
        Master.Title = "" + GameName + "_机器人管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("xk3swset.aspx") + "\">" + GameName + "</a>&gt;机器人管理");
        builder.Append(Out.Tab("</div>", "<br />"));

        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置

        string RoBotID = Utils.GetRequest("RoBotID", "post", 1, "", xml.dss["XK3ROBOTID"].ToString());
        string IsBot = Utils.GetRequest("IsBot", "post", 1, @"^[0-1]$", xml.dss["XIsBot"].ToString());
        string RoBotCost = Utils.GetRequest("RoBotCost", "post", 1, @"^[0-9]\d*$", xml.dss["XK3ROBOTCOST"].ToString());
        string RoBotCount = Utils.GetRequest("RoBotCount", "post", 1, @"^[0-9]\d*$", xml.dss["XK3ROBOTBUY"].ToString());

        xml.dss["XK3ROBOTID"] = RoBotID;
        xml.dss["XIsBot"] = IsBot;
        xml.dss["XK3ROBOTCOST"] = RoBotCost;
        xml.dss["XK3ROBOTBUY"] = RoBotCount;
        System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);

        string strText = "机器人ID:/,机器人状态:/,机器人单注投注金额:（不能为0）/,机器人每期购买订单数:（0为不限制）/";
        string strName = "RoBotID,IsBot,RoBotCost,RoBotCount";
        string strType = "textarea,select,text,text";
        string strValu = xml.dss["XK3ROBOTID"].ToString() + "'" + xml.dss["XIsBot"].ToString() + "'" + xml.dss["XK3ROBOTCOST"].ToString() + "'" + xml.dss["XK3ROBOTBUY"].ToString();
        string strEmpt = "true,0|关闭|1|开启,true,false";
        string strIdea = "/";
        string strOthe = "确定修改,xk3swset.aspx?act=robot&ve=2a,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        //显示
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<hr/>");
        builder.Append("温馨提示：多个机器人ID请用#分隔。<br />");

        string aa = xml.dss["XK3ROBOTID"].ToString();
        string[] sNum = Regex.Split(aa, "#");
        int bb = sNum.Length;



        builder.Append("当前机器人ID为：<h style=\"color:red\">" + xml.dss["XK3ROBOTID"].ToString() + "</h>--------(提示：共有《" + bb + "》个机器人)<br />");
        if (xml.dss["XIsBot"].ToString() == "0")
        {
            builder.Append("机器人状态：<h style=\"color:red\">关闭</h><br />");
        }
        else
        {
            builder.Append("当前机器人状态：<h style=\"color:red\">开启</h><br />");
        }
        builder.Append("机器人单注投注金额：<h style=\"color:red\">" + xml.dss["XK3ROBOTCOST"].ToString() + "</h><br />");
        builder.Append("机器人每期购买彩票数：<h style=\"color:red\">" + xml.dss["XK3ROBOTBUY"].ToString() + "</h><br />");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<hr/>");
        builder.Append("<a href=\"" + Utils.getUrl("xk3swset.aspx") + "\">&lt;&lt;&lt;返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //==========紧急添加开奖数据========================试玩版不用
    private void Top_addPage()
    {
        Master.Title = "" + GameName + "_紧急添加开奖数据";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("xk3swset.aspx") + "\">" + GameName + "</a>&gt;紧急添加开奖数据");
        builder.Append(Out.Tab("</div>", "<br />"));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");

        if (Utils.ToSChinese(ac) == "确定添加")
        {
            string num1 = Utils.GetRequest("num1", "post", 2, @"^[1-9]\d{8,8}$", "填写开奖期号出错");//开奖期号
            string num2 = Utils.GetRequest("num2", "post", 2, @"^[1-6][1-6][1-6]$", "填写开奖号码出错");//开奖号码
            string num22 = num2[0] + "," + num2[1] + "," + num2[2];
            string _where = string.Empty;
            _where = "where Lottery_issue='" + num1 + "'";
            int tpye1 = 0;

            BCW.XinKuai3.Model.XK3_Internet_Data model_select = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3listLast(_where);
            BCW.XinKuai3.Model.XK3_Internet_Data model = new BCW.XinKuai3.Model.XK3_Internet_Data();
            if (model_select.Lottery_issue == "0")
            {
                model.Lottery_issue = num1;
                model.Lottery_num = num22;
                model.Lottery_time = DateTime.Now;
                model.UpdateTime = DateTime.Now;
                new BCW.XinKuai3.BLL.XK3_Internet_Data().Add_num(model);
                tpye1 = 1;
            }
            else if ((model_select.Lottery_issue != "0") && (model_select.Lottery_num == ""))
            {
                model.Lottery_issue = num1;
                model.Lottery_num = num22;
                new BCW.XinKuai3.BLL.XK3_Internet_Data().update_num2(model);
                tpye1 = 2;
            }
            //开奖
            string[] str1 = num22.Split(',');
            string t1 = str1[0];
            string t2 = str1[1];
            string t3 = str1[2];

            string _where2 = string.Empty;
            _where2 = "'" + num1 + "' Order by id desc";

            BCW.XinKuai3.Model.XK3_Internet_Data model_getTime = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3all_num2(_where2);
            try
            {
                DateTime getTime = model_getTime.Lottery_time;
                model.Lottery_time = getTime;
            }
            catch
            {
                model.Lottery_time = DateTime.Now;
            }
            model.Lottery_issue = num1;
            model.Lottery_num = num22;
            //大小单双
            if (((t1 == t2) && (t1 == t3)))//大小双单通食
            {
                model.DaXiao = "0";
                model.DanShuang = "0";
            }
            else
            {
                if ((Int32.Parse(str1[0])) + (Int32.Parse(str1[1])) + (Int32.Parse(str1[2])) <= 10)
                {
                    model.DaXiao = "1";//和值开出是4-10,即为小

                }
                else
                {
                    model.DaXiao = "2";//和值开出是11-17,即为大

                }
                if (((Int32.Parse(str1[0])) + (Int32.Parse(str1[1])) + (Int32.Parse(str1[2]))) % 2 == 1)
                {
                    model.DanShuang = "1";//单数
                }
                else if (((Int32.Parse(str1[0])) + (Int32.Parse(str1[1])) + (Int32.Parse(str1[2]))) % 2 == 0)
                {
                    model.DanShuang = "2";//双数
                }
            }

            //和值
            string sum1 = (Int32.Parse(str1[0]) + Int32.Parse(str1[1]) + Int32.Parse(str1[2])).ToString();
            model.Sum = sum1;
            //三同号通选+三同号单选
            if (((t1 == t2) && (t1 == t3)))
            {
                model.Three_Same_All = "1";
                model.Three_Same_Single = t1 + t2 + t3;
                //model.Three_Continue_All = "1";
            }
            else
            {
                model.Three_Same_All = "0";
                model.Three_Same_Single = "0";
                //model.Three_Continue_All = "0";
            }
            //三不同号
            if ((t1 != t2) && (t1 != t3) && (t2 != t3))
            {
                model.Three_Same_Not = t1 + t2 + t3;
            }
            else
            {
                model.Three_Same_Not = "0";
            }
            //三连号通选
            if ((Int32.Parse(str1[1]) == (Int32.Parse(str1[0]) + 1)) && (Int32.Parse(str1[1]) == (Int32.Parse(str1[2]) - 1)))
            {
                //model.Three_Continue_All = t1 + t2 + t3;
                model.Three_Continue_All = "1";
            }
            else
            {
                model.Three_Continue_All = "0";
            }
            //二同号复选
            if ((t1 == t2) || (t2 == t3))
            {
                model.Two_Same_All = t2 + t2;

            }
            else
            {
                model.Two_Same_All = "0";

            }
            //二同号单选
            if ((t1 == t2) || (t2 == t3))
            {
                model.Two_Same_Single = t1 + t2 + t3;
            }
            else if ((t1 == t2) && (t1 == t3))
            {
                model.Two_Same_Single = "0";
            }
            else
            {
                model.Two_Same_Single = "0";
            }
            //二不同号
            if ((t1 != t2) && (t2 != t3))
            {
                model.Two_dissame = (t1 + t2) + "," + (t1 + t3) + "," + (t2 + t3);
            }
            else if ((t1 == t2) && (t2 != t3))
            {
                model.Two_dissame = t2 + t3;
            }
            else if ((t1 != t2) && (t2 == t3))
            {
                model.Two_dissame = t1 + t2;
            }
            else
            {
                model.Two_dissame = "0";
            }
            model.UpdateTime = DateTime.Now;
            new BCW.XinKuai3.BLL.XK3_Internet_Data().Update_num2(model);
            change_peilv();//根据最近几期的大小单双开奖情况，实时变动赔率。
            Open_price();//返彩
            if (tpye1 == 1)
            {
                Utils.Success("" + GameName + "_紧急添加开奖数据", "开奖数据《已添加》成功，正在返回..", Utils.getUrl("xk3swset.aspx?act=Top_add"), "1");
            }
            else if (tpye1 == 2)
            {
                Utils.Success("" + GameName + "_紧急添加开奖数据", "开奖数据《已更新开奖号码》成功，正在返回..", Utils.getUrl("xk3swset.aspx?act=Top_add"), "1");
            }
            else
            {
                Utils.Success("" + GameName + "_紧急添加开奖数据", "添加失败：不能对有开奖号码的期号进行修改，正在返回..", Utils.getUrl("xk3swset.aspx?act=Top_add"), "2");
            }
        }
        else
        {
            strText = "开奖期号：/,开奖号码：/,,";
            strName = "num1,num2,hid,act";
            strType = "num,num,hidden,hidden";
            strValu = "'''" + Utils.getPage(0) + "";
            strEmpt = "false,false,false,false";
            strIdea = "/";
            strOthe = "确定添加,xk3swset.aspx?act=Top_add,post,0,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<hr/>");
        builder.Append("温馨提示：请在输入框输入需要开奖的号码。如：123<br/>对于已有开奖号码的期号，不能对其修改，也不能删除。<br/>");
        builder.Append("<a href=\"" + Utils.getUrl("xk3swset.aspx") + "\">&lt;&lt;&lt;返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //==========排行榜奖励发放界面================
    private void ReWard()
    {
        Master.Title = "" + GameName + "_奖励发放";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("xk3swset.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("xk3swset.aspx?act=paihang") + "\">用户排行</a>&gt;奖励发放");
        builder.Append(Out.Tab("</div>", ""));


        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));
        //string startstate = (Utils.GetRequest("startstate", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8])))$", "20150101"));
        //string endstate = (Utils.GetRequest("endstate", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8])))$", "20501231"));
        DateTime startstate = Utils.ParseTime(Utils.GetRequest("startstate", "all", 2, DT.RegexTime, "开始时间填写无效"));
        DateTime endstate = Utils.ParseTime(Utils.GetRequest("endstate", "all", 2, DT.RegexTime, "结束时间填写无效"));
        int pageIndex = int.Parse(Utils.GetRequest("pageIndex", "all", 1, @"^[1-9]\d*$", "1"));
        string rewardid = Utils.GetRequest("rewardid", "post", 2, @"^[\s\S]{1,500}$", "当前页无可操作用户");
        //builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        string wdy = "";
        if (pageIndex == 1)
            wdy = "TOP10";
        else
            wdy = "TOP" + (pageIndex - 1).ToString() + "1-" + pageIndex.ToString() + "0";
        builder.Append(Out.Tab("<div>", "<br/>"));
        switch (ptype)
        {
            case 1:
                builder.Append("《净赚排行》" + wdy + "奖励发放：");
                break;
            case 2:
                builder.Append("《赚币排行》" + wdy + "奖励发放：");
                break;
            case 3:
                builder.Append("《游戏狂人》" + wdy + "奖励发放：");
                break;
        }
        builder.Append(Out.Tab("</div>", ""));

        int mzj = (pageIndex - 1) * 10;
        string[] IdRe = rewardid.Split('#');
        try
        {
            string strText2 = ",,,,TOP" + (mzj + 1) + "：" + IdRe[0] + "&nbsp;&nbsp;,,TOP" + (mzj + 2) + "：" + IdRe[1] + "&nbsp;&nbsp;,,TOP" + (mzj + 3) + "：" + IdRe[2] + "&nbsp;&nbsp;,,TOP" + (mzj + 4) + "：" + IdRe[3] + "&nbsp;&nbsp;,,TOP" + (mzj + 5) + "：" + IdRe[4] + "&nbsp;&nbsp;,,TOP" + (mzj + 6) + "：" + IdRe[5] + "&nbsp;&nbsp;,,TOP" + (mzj + 7) + "：" + IdRe[6] + "&nbsp;&nbsp;,,TOP" + (mzj + 8) + "：" + IdRe[7] + "&nbsp;&nbsp;,,TOP" + (mzj + 9) + "：" + IdRe[8] + "&nbsp;&nbsp;,,TOP" + pageIndex * 10 + "：" + IdRe[9] + "&nbsp;&nbsp;,";
            string strName2 = "act,ptype,startstate,endstate,top1,IdRe1,top2,IdRe2,top3,IdRe3,top4,IdRe4,top5,IdRe5,top6,IdRe6,top7,IdRe7,top8,IdRe8,top9,IdRe9,top10,IdRe10";
            string strType2 = "hidden,hidden,hidden,hidden,num,hidden,num,hidden,num,hidden,num,hidden,num,hidden,num,hidden,num,hidden,num,hidden,num,hidden,num,hidden";
            string strValu2 = "ReWardCase'" + ptype + "'" + DT.FormatDate(startstate, 0) + "'" + DT.FormatDate(endstate, 0) + "'" + "0'" + IdRe[0] + "'0'" + IdRe[1] + "'0'" + IdRe[2] + "'0'" + IdRe[3] + "'0'" + IdRe[4] + "'0'" + IdRe[5] + "'0'" + IdRe[6] + "'0'" + IdRe[7] + "'0'" + IdRe[8] + "'0'" + IdRe[9] + "'0";
            string strEmpt2 = "false,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true";
            string strIdea2 = "/";
            string strOthe2 = "提交,xk3swset.aspx,post,1,red";
            builder.Append(Out.wapform(strText2, strName2, strType2, strValu2, strEmpt2, strIdea2, strOthe2));
        }
        catch
        {
            builder.Append(Out.Tab("<div>", "<br/>"));
            builder.Append("当页少于10人，无法发放！");
            builder.Append(Out.Tab("</div>", ""));

        }
        builder.Append(Out.Tab("</div>", Out.Hr()));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("xk3swset.aspx?act=paihang") + "\">&lt;&lt;&lt;返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //==========排行榜奖励发放====================
    private void ReWardCase()
    {
        int[] IdRe = new int[11];
        long[] Top = new long[11];
        IdRe[1] = int.Parse(Utils.GetRequest("IdRe1", "post", 1, "", "3004"));
        IdRe[2] = int.Parse(Utils.GetRequest("IdRe2", "post", 1, "", "3004"));
        IdRe[3] = int.Parse(Utils.GetRequest("IdRe3", "post", 1, "", "3004"));
        IdRe[4] = int.Parse(Utils.GetRequest("IdRe4", "post", 1, "", "3004"));
        IdRe[5] = int.Parse(Utils.GetRequest("IdRe5", "post", 1, "", "3004"));
        IdRe[6] = int.Parse(Utils.GetRequest("IdRe6", "post", 1, "", "3004"));
        IdRe[7] = int.Parse(Utils.GetRequest("IdRe7", "post", 1, "", "3004"));
        IdRe[8] = int.Parse(Utils.GetRequest("IdRe8", "post", 1, "", "3004"));
        IdRe[9] = int.Parse(Utils.GetRequest("IdRe9", "post", 1, "", "3004"));
        IdRe[10] = int.Parse(Utils.GetRequest("IdRe10", "post", 1, "", "3004"));
        Top[1] = Convert.ToInt64(Utils.GetRequest("top1", "post", 1, "", ""));
        Top[2] = Convert.ToInt64(Utils.GetRequest("top2", "post", 1, "", ""));
        Top[3] = Convert.ToInt64(Utils.GetRequest("top3", "post", 1, "", ""));
        Top[4] = Convert.ToInt64(Utils.GetRequest("top4", "post", 1, "", ""));
        Top[5] = Convert.ToInt64(Utils.GetRequest("top5", "post", 1, "", ""));
        Top[6] = Convert.ToInt64(Utils.GetRequest("top6", "post", 1, "", ""));
        Top[7] = Convert.ToInt64(Utils.GetRequest("top7", "post", 1, "", ""));
        Top[8] = Convert.ToInt64(Utils.GetRequest("top8", "post", 1, "", ""));
        Top[9] = Convert.ToInt64(Utils.GetRequest("top9", "post", 1, "", ""));
        Top[10] = Convert.ToInt64(Utils.GetRequest("top10", "post", 1, "", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));
        //string startstate = (Utils.GetRequest("startstate", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8])))$", "20150101"));
        //string endstate = (Utils.GetRequest("endstate", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8])))$", "20501231"));
        DateTime startstate = Utils.ParseTime(Utils.GetRequest("startstate", "all", 2, DT.RegexTime, "开始时间填写无效"));
        DateTime endstate = Utils.ParseTime(Utils.GetRequest("endstate", "all", 2, DT.RegexTime, "结束时间填写无效"));
        string wdy = "";
        switch (ptype)
        {
            case 1:
                wdy = "净赚排行榜";
                break;
            case 2:
                wdy = "赚币排行榜";
                break;
            case 3:
                wdy = "游戏狂人榜";
                break;
        }
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        if (Utils.ToSChinese(ac) == "确定发放")
        {
            for (int i = 1; i <= 10; i++)
            {
                if (Top[i] != 0)
                {
                    new BCW.BLL.User().UpdateiGold(IdRe[i], Top[i], "新快3_试玩版排行榜奖励");
                    //发内线
                    string strLog = "您在 " + startstate + " 至 " + endstate + " 里在游戏《新快3》" + wdy + "上取得了第" + i + "名的好成绩，系统奖励了" + Top[i] + "" + klb + "[url=/bbs/game/xk3.aspx]进入《新快3》[/url]";
                    new BCW.BLL.Guest().Add(0, IdRe[i], new BCW.BLL.User().GetUsName(IdRe[i]), strLog);
                    //动态
                    string mename = new BCW.BLL.User().GetUsName(IdRe[i]);
                    string wText = "[url=/bbs/uinfo.aspx?uid=" + IdRe[i] + "]" + mename + "[/url]在[url=/bbs/game/xk3.aspx]《新快3》[/url]" + wdy + "上取得了第" + i + "名的好成绩，系统奖励了" + Top[i] + "" + klb;
                    new BCW.BLL.Action().Add(1002, 0, IdRe[i], "", wText);
                }
            }
            Utils.Success("奖励操作", "奖励操作成功", Utils.getUrl("xk3swset.aspx?act=paihang"), "1");
        }
        else
        {
            Master.Title = "" + GameName + "_奖励发放";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
            builder.Append("<a href=\"" + Utils.getUrl("xk3swset.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("xk3swset.aspx?act=paihang") + "\">用户排行</a>&gt;奖励发放");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("正在发放《" + wdy + "》奖励：");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("时间从：" + startstate + "到" + endstate + "<br/>");
            for (int j = 1; j <= 10; j++)
            {
                builder.Append("TOP" + j + "：" + IdRe[j] + ".[" + Top[j] + "快乐币]<br/>");
            }

            string strText2 = ",,,,,,,,,,,,,,,,,,,,,,,";
            string strName2 = "act,ptype,startstate,endstate,top1,IdRe1,top2,IdRe2,top3,IdRe3,top4,IdRe4,top5,IdRe5,top6,IdRe6,top7,IdRe7,top8,IdRe8,top9,IdRe9,top10,IdRe10";
            string strType2 = "hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden";
            string strValu2 = "ReWardCase'" + ptype + "'" + DT.FormatDate(startstate, 0) + "'" + DT.FormatDate(endstate, 0) + "'" + Top[1] + "'" + IdRe[1] + "'" + Top[2] + "'" + IdRe[2] + "'" + Top[3] + "'" + IdRe[3] + "'" + Top[4] + "'" + IdRe[4] + "'" + Top[5] + "'" + IdRe[5] + "'" + Top[6] + "'" + IdRe[6] + "'" + Top[7] + "'" + IdRe[7] + "'" + Top[8] + "'" + IdRe[8] + "'" + Top[9] + "'" + IdRe[9] + "'" + Top[10] + "'" + IdRe[10];
            string strEmpt2 = "false,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true";
            string strIdea2 = "/";
            string strOthe2 = "确定发放,xk3swset.aspx,post,1,red";
            builder.Append(Out.wapform(strText2, strName2, strType2, strValu2, strEmpt2, strIdea2, strOthe2));

            builder.Append(Out.Tab("</div>", ""));


            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getPage("xk3swset.aspx?act=paihang") + "\">&lt;&lt;再看看吧</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }


    }

    //==========临时添加试玩ID====================
    private void ShiwanPage()
    {
        Master.Title = "" + GameName + "_添加试玩ID";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("xk3swset.aspx") + "\">" + GameName + "</a>&gt;添加试玩ID");
        builder.Append(Out.Tab("</div>", ""));

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        if (Utils.ToSChinese(ac) == "确定增加")
        {
            int Status = int.Parse(Utils.GetRequest("Status", "post", 1, @"^[0-9]\d*$", "0"));
            if (Status != 0)
            {

                if (!new BCW.XinKuai3.BLL.SWB().Exists(Status))
                {
                    BCW.XinKuai3.Model.SWB model = new BCW.XinKuai3.Model.SWB();
                    model.UserID = Status;
                    try
                    {
                        new BCW.XinKuai3.BLL.SWB().Add_num(model);
                    }
                    catch
                    {
                        Utils.Error("请输入会员ID，请重新添加。", "");
                    }
                    Utils.Success("温馨提示", "添加试玩ID成功。正在返回....", Utils.getUrl("xk3swset.aspx?act=shiwan&amp;backurl=" + Utils.getPage(0) + ""), "1");
                }
                else
                {
                    Utils.Success("温馨提示", "该ID已存在。正在返回....", Utils.getUrl("xk3swset.aspx?act=shiwan&amp;backurl=" + Utils.getPage(0) + ""), "1");
                }

            }
            else
                Utils.Error("添加试玩ID失败，请重新添加。", "");
        }
        else
        {
            string strText = "请输入试玩的ID：/,";
            string strName = "Status,backurl";
            string strType = "num,hidden";
            string strValu = "'" + Utils.getPage(0) + "";
            string strEmpt = "false,false";
            string strIdea = "/";
            string strOthe = "确定增加,xk3swset.aspx?act=shiwan,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }


        DataSet get_num = new BCW.XinKuai3.BLL.SWB().GetList("COUNT(UserID) as aa", "UserID != '0'");
        string a_1 = string.Empty;
        for (int i = 0; i < get_num.Tables[0].Rows.Count; i++)
        {
            a_1 = Convert.ToString(get_num.Tables[0].Rows[i]["aa"]);//最后一期的大小
        }
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("《共有" + a_1 + "人在试玩》");
        builder.Append(Out.Tab("</div>", "<br/>"));

        int pageIndex;
        int recordCount;
        int pageSize = 20;
        string strWhere = " UserID!='' ";
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        // 开始读取列表
        IList<BCW.XinKuai3.Model.SWB> listXK3pay = new BCW.XinKuai3.BLL.SWB().GetSWBs(pageIndex, pageSize, strWhere, out recordCount);
        if (listXK3pay.Count > 0)
        {
            int k = 1;
            foreach (BCW.XinKuai3.Model.SWB n in listXK3pay)
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
                //获取id对应的用户名
                string mename = new BCW.BLL.User().GetUsName(n.UserID);
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + "、ID:" + n.UserID + "------用户名:" + mename + "");
                builder.Append(".<a href=\"" + Utils.getUrl("xk3swset.aspx?act=del_sw&amp;id=" + n.UserID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[删]</a>");
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

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<h style=\"color:red\">注意：只能添加已注册的会员。</h><br />");
        builder.Append("<a href=\"" + Utils.getUrl("xk3swset.aspx") + "\">&lt;&lt;&lt;返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //删除试玩ID
    private void Del_swPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        if (info == "")
        {
            Master.Title = "删除一条投注记录";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除该记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("xk3swset.aspx?info=ok&amp;act=del_sw&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("xk3swset.aspx?act=shiwan") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.XinKuai3.BLL.SWB().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            else
            {
                new BCW.XinKuai3.BLL.SWB().Delete(id);
                //BCW.XinKuai3.Model.XK3_Toplist_SWB a = new BCW.XinKuai3.BLL.XK3_Toplist_SWB().GetXK3_meid(id);
                //new BCW.XinKuai3.BLL.XK3_Toplist_SWB().Delete(a.ID);
                Utils.Success("删除试玩ID", "删除试玩ID成功..", Utils.getPage("xk3swset.aspx?act=shiwan"), "2");
            }
        }
    }

    //把过期改为不过期
    private void EditPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        if (info == "")
        {
            Master.Title = "修改一条投注记录";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定修改该记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("xk3swset.aspx?info=ok&amp;act=edit&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定修改</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("xk3swset.aspx?act=fenxi&amp;ptype=3") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.XinKuai3.BLL.XK3_Bet_SWB().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            else
            {
                int a = 1;
                new BCW.XinKuai3.BLL.XK3_Bet_SWB().UpdateState(id, a);
                Utils.Success("修改过期领奖数据", "修改过期领奖数据成功..", Utils.getPage("xk3swset.aspx?act=fenxi&amp;ptype=3"), "2");
            }
        }
    }



}
