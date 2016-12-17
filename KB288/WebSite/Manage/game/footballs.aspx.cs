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
using System.Net;
using System.Text;
using System.IO;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Xml;
using System.Collections.Generic;
using BCW.Common;
using TPR2.Common;
using System.Threading;
public partial class Manage_game_footballs : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/ball.xml";
    //篮球收藏开关 1开 0关闭
    string BasketBallCollect = (ub.GetSub("BasketBallCollect", "/Controls/footballs.xml"));
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "all":
                AllSet();
                break;
            case "balllist":
                BalllistPage();
                break;
            case "allballlist":
                AllBalllistPage();
                break;
            case "find":
                FindBalllistPage();//赛事速查
                break;
            case "onelist":
                GetAGameData();
                break;
            case "style":
                GetAStyleData();
                break;
            case "team":
                GetATeamData();
                break;
            case "day":
                GetADayData();
                break;
            case "update":
                UpdateList();
                break;
            case "view":
                ViewPage();
                break;
            case "reset":
                ResetPage();
                break;
            case "del":
                DeleBallId();
                break;
            //篮球开始
            case "28"://篮球即时比分
                BasketNow();
                break;
            case "29"://篮球一场比分查看
                BasketMatch();
                break;
            case "30"://分类查看
                SplitBaskGame();
                break;
            case "31"://篮球收藏
                FavoriteBacketGame();
                break;
            case "32"://修改篮球数据
                UpdateListB();
                break;
            case "33"://修改篮球数据
                DeleBaskId();
                break;
            case "34":
                ViewPageBask();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        Master.Title = "即时足球比分管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("即时比分管理");
        builder.Append(Out.Tab("</div>", "<br />"));
        string type = (Utils.GetRequest("type", "all", 1, "", "1"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("全局管理");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("footballs.aspx?act=all&amp;") + "\">全局设置</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
        builder.Append("足球管理 ");
        //if (type == "1")
        //{
        //  
        //}
        //else
        //{
        //    builder.Append("<a href=\"" + Utils.getUrl("footballs.aspx?type=1&amp;") + "\">足球管理</a> |");
        //}
        //if (type == "2")
        //{
        //    builder.Append(" 篮球管理");
        //}
        //else
        //{
        //    builder.Append("<a href=\"" + Utils.getUrl("footballs.aspx?type=2&amp;") + "\"> 篮球管理</a>");
        //}
        builder.Append(Out.Tab("</div>", "<br />"));
        // if (type == "1")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("footballs.aspx?act=balllist&amp;State=1&amp;") + "\">比分管理</a><br />");
            //builder.Append("<a href=\"" + Utils.getUrl("footballs.aspx?act=balllist&amp;State=2&amp;") + "\">完场比分</a><br />");
            //builder.Append("<a href=\"" + Utils.getUrl("footballs.aspx?act=balllist&amp;State=0&amp;") + "\">未开比分</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("footballs.aspx?act=find&amp;State=9&amp;") + "\">赛事速查</a><br />");

            //builder.Append(Out.Tab("<div>", ""));
            //builder.Append("#配置管理#");
            //builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("footballs.aspx?act=view") + "\">足球配置</a><br />");
            //builder.Append("<a href=\"" + Utils.getUrl("footballs.aspx?act=reset") + "\">重置游戏</a><br />"); 
            builder.Append(Out.Tab("</div>", ""));
        }
        //  else
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("篮球管理 ");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", "<br/>"));
            builder.Append("<a href=\"" + Utils.getUrl("footballs.aspx?act=28&amp;State=1&amp;") + "\">比分管理</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("footballs.aspx?act=34&amp;State=1&amp;") + "\">篮球配置</a><br />");
            builder.Append(Out.Tab("</div>", ""));

        }
        // builder.Append("<a href=\"" + Utils.getUrl("../toplist.aspx?act=top&amp;ptype=6&amp;backurl=" + Utils.PostPage(1) + "") + "\">排行榜单</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //all 全局配置
    private void AllSet()
    {
        Master.Title = "游戏配置";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append(Out.waplink(Utils.getUrl("footballs.aspx"), "比分管理-"));
        builder.Append("全局管理");
        builder.Append(Out.Tab("</div>", "<br />"));
        string info = (Utils.GetRequest("info", "all", 1, @"", ""));
        //  int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-2]\d*$", "1"));
        ub xml = new ub();
        string xmlPath = "/Controls/footballs.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("显示设置");
        builder.Append(Out.Tab("</div>", ""));
        if (info == "ok")
        {
            //BallQiuChangBallOpenSpeakBallJinQiu
            string Open = Utils.GetRequest("Open", "all", 1, "", "");
            string ZQOpen = Utils.GetRequest("ZQOpen", "all", 1, "", "");
            string getZQOpen = Utils.GetRequest("getZQOpen", "all", 1, "", "");
            string LQOpen = Utils.GetRequest("LQOpen", "all", 1, "", "");
            string getLQOpen = Utils.GetRequest("getLQOpen", "all", 1, "", "");
            xml.dss["Open"] = Open;//0关闭1开启
            xml.dss["ZQOpen"] = ZQOpen;////0关闭1开启
            xml.dss["getZQOpen"] = getZQOpen;//0隐藏1显示
            xml.dss["LQOpen"] = LQOpen;////0关闭1开启
            xml.dss["getLQOpen"] = getLQOpen;//0隐藏1显示
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("足球直播设置", "设置成功，正在返回..", Utils.getUrl("footballs.aspx?act=all&amp;ve=2a;backurl=" + Utils.getPage(0) + ""), "1");
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("暂无相关修改");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            string strText1 = "即时比分状态:/,足球开关:/,足球捉取开关:/,篮球开关:/,篮球捉取开关:/,";
            string strName1 = "Open,ZQOpen,getZQOpen,LQOpen,getLQOpen,backurl";
            string strType1 = "select,select,select,select,select,hidden";
            string strValu1 = xml.dss["Open"] + "'" + xml.dss["ZQOpen"] + "'" + xml.dss["getZQOpen"] + "'" + xml.dss["LQOpen"] + "'" + xml.dss["getLQOpen"] + "'" + Utils.getPage(0) + "";
            string strEmpt1 = "0|维护|1|正常,0|关闭|1|开启,0|关闭|1|开启,0|关闭|1|开启,0|关闭|1|开启,false";
            string strIdea1 = "/";
            string strOthe1 = "确定修改,footballs.aspx?act=all&amp;info=ok&amp;,post,1,red";
            builder.Append(Out.wapform(strText1, strName1, strType1, strValu1, strEmpt1, strIdea1, strOthe1));
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("footballs.aspx") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    #region 篮球开始 现函数

    //获取状态
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
                return "加时2";
            case "6":
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

    //view 游戏配置
    private void ViewPageBask()
    {
        Master.Title = "游戏配置";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append(Out.waplink(Utils.getUrl("footballs.aspx"), "比分管理-"));
        builder.Append("篮球配置");
        builder.Append(Out.Tab("</div>", "<br />"));
        string info = (Utils.GetRequest("info", "all", 1, @"", ""));
        //  int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-2]\d*$", "1"));
        ub xml = new ub();
        string xmlPath = "/Controls/footballs.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("显示设置");
        builder.Append(Out.Tab("</div>", ""));
        if (info == "ok")
        {
            //BallQiuChangBallOpenSpeakBallJinQiu
            string hidden = Utils.GetRequest("hidden", "all", 1, "", "");
            string collect = Utils.GetRequest("collect", "all", 1, "", "");
            string ouzhi = Utils.GetRequest("ouzhi", "all", 1, "", "");
            xml.dss["BasketBallHidden"] = hidden;//0关闭1开启
            xml.dss["BasketBallCollect"] = collect;////0关闭1开启
            xml.dss["BasketBallOutZhi"] = ouzhi;//0隐藏1显示
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("足球直播设置", "设置成功，正在返回..", Utils.getUrl("footballs.aspx?act=34&amp;ve=2a;backurl=" + Utils.getPage(0) + ""), "1");
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("暂无相关修改");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            string strText1 = "自动关联(隐藏)状态:/,收藏状态:/,欧指状态:/,";
            string strName1 = "hidden,collect,ouzhi,backurl";
            string strType1 = "select,select,select,hidden";
            string strValu1 = xml.dss["BasketBallHidden"] + "'" + xml.dss["BasketBallCollect"] + "'" + xml.dss["BasketBallOutZhi"] + "'" + Utils.getPage(0) + "";
            string strEmpt1 = "0|关闭|1|开启,0|隐藏|1|开启,0|隐藏|1|显示,false";
            string strIdea1 = "/";
            string strOthe1 = "确定修改,footballs.aspx?act=34&amp;info=ok&amp;,post,1,red";
            builder.Append(Out.wapform(strText1, strName1, strType1, strValu1, strEmpt1, strIdea1, strOthe1));
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("footballs.aspx") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //30  篮球联赛筛选
    private void SplitBaskGame()
    {
        string Style = Utils.GetRequest("Style", "all", 1, "", "");
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(Out.waplink(Utils.getUrl("footballs.aspx"), "游戏-"));
        builder.Append(Out.waplink(Utils.getUrl("footballs.aspx"), "比分管理-"));
        builder.Append(Out.waplink(Utils.getUrl("footballs.aspx?act=28&amp;"), "篮球比分-"));
        builder.Append("赛事筛选");
        builder.Append(Out.Tab("</div>", "<br/>"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("请选择赛事(可多选)" + "<br/>");
        string strWhere = " Id>0 ";
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
                //if (i % 2 == 0)
                //{ builder.Append(Out.Tab("<div >", "")); }
                //else
                //{
                //    if (i == 1)
                //        builder.Append(Out.Tab("<div class=\"text\">", ""));
                //    else
                //        builder.Append(Out.Tab("<div class=\"text\">", ""));
                //}
                builder.Append(Out.waplink(Utils.getUrl("footballs.aspx?act=30&amp;Style=" + Style + ds.Tables[0].Rows[koo + i]["classType"].ToString() + "." + ""), "[" + ds.Tables[0].Rows[koo + i]["classType"].ToString() + "]"));
                // builder.Append(Out.Tab("</div>", ""));
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
        builder.Append(Out.Tab("</div>", "<br/>"));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //28 篮球即时比分
    private void BasketNow()
    {
        int meid = new BCW.User.Users().GetUsId();
        //if (meid == 0)
        //    Utils.Login();
        string Style = Utils.GetRequest("Style", "all", 1, "", "");
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(Out.waplink(Utils.getUrl("footballs.aspx"), "游戏-"));
        builder.Append(Out.waplink(Utils.getUrl("footballs.aspx"), "比分管理-"));
        builder.Append("篮球比分");
        // builder.Append("-即时比分");
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
        string lwhere = "  ";
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
            title = "即 时";//and isHidden=1 
            strWhere = "  convert(int,matchstate)>-1  and    DateDiff(dd,matchtime,getdate())=0 " + lwhere + "  ORDER BY matchtime ASC";
            textout = "直播";
            builder.Append("即 时| ");
            textwhere = "暂无更多进行赛事.";
            //  strWhere = "p_active=0 and p_del=0 and p_ison=1 and p_isondel=0 and p_basketve=0";
        }
        else
        {
            builder.Append(Out.waplink(Utils.getUrl("footballs.aspx?ptype=1&amp;act=28&amp;kind=" + kind + "&amp;State=1"), "即 时| ") + "");
        }
        if ((State == 2))
        {
            builder.Append("赛 果| ");
            DateTime dt = Convert.ToDateTime(date);
            title = "比赛结果"; //datediff(dd, matchtime, GETDATE()) = 0 and  isHidden=1 
            strWhere = " matchstate=-1 and matchtime <'" + dt.AddDays(1).ToString("yyyyMMdd 00:00:00") + "' and matchtime >'" + dt.ToString("yyyyMMdd 00:00:00") + "' ORDER BY convert(datetime,matchtime,120) desc";
            textwhere = "暂无更多完场赛事.";
        }
        else
        {
            builder.Append(Out.waplink(Utils.getUrl("footballs.aspx?ptype=1&amp;act=28&amp;kind=" + kind + "&amp;State=2&amp;date=" + DateTime.Now.AddDays(-2).ToString() + "&amp;"), "赛 果| ") + "");
        }
        if (State == 3)
        {
            title = "未开赛事";//and isHidden=1 
            strWhere = " matchstate>-10  ORDER BY convert(datetime,matchtime,120) desc";
            textout = "未";
            builder.Append("赛 程| ");
            textwhere = "暂无更多未开赛事.";
            // strWhere = "p_once!='完' and p_type=1";
        }
        else
        {
            builder.Append(Out.waplink(Utils.getUrl("footballs.aspx?ptype=1&amp;act=28&amp;kind=" + kind + "&amp;State=3&amp;week=" + n + "&amp;"), "赛 程| ") + "");
        }
        if (State == 4)
        {
            //   if (BasketBallCollect == "1")
            {
                title = "收藏比赛";
                strWhere = " matchstate=-1 and datediff(dd,matchtime,GETDATE())=0  ORDER BY convert(datetime,matchtime,120) desc";
                textout = "未";
                builder.Append("收 藏");
                textwhere = "暂无更多未开赛事.";
                // strWhere = "p_once!='完' and p_type=1";
            }
        }
        else
        {
            //if (BasketBallCollect == "1")
            {
                builder.Append(Out.waplink(Utils.getUrl("footballs.aspx?ptype=1&amp;act=28&amp;kind=" + kind + "&amp;State=4"), "收 藏 ") + "");
            }
        }
        builder.Append(Out.Tab("</div>", ""));

        if ((State == 2))
        {
            builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
            builder.Append(Out.waplink(Utils.getUrl("footballs.aspx?ptype=2&amp;act=28&amp;kind=" + kind + "&amp;State=2&amp;date=" + Convert.ToDateTime(date).AddDays(-1).ToString() + "&amp;"), Convert.ToDateTime(date).AddDays(-1).ToString("yyyy-MM-dd")) + " | ");
            builder.Append(Convert.ToDateTime(date).ToString("yyyy-MM-dd") + " | ");
            if (Convert.ToDateTime(date) > DateTime.Now)
            {
                builder.Append(Convert.ToDateTime(date).AddDays(1).ToString("yyyy-MM-dd"));
            }
            else
                builder.Append(Out.waplink(Utils.getUrl("footballs.aspx?ptype=2&amp;act=28&amp;kind=" + kind + "&amp;State=2&amp;date=" + Convert.ToDateTime(date).AddDays(1).ToString() + "&amp;"), Convert.ToDateTime(date).AddDays(1).ToString("yyyy-MM-dd")));
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
                    // and "+dt.AddDays(1).ToString("yyyyMMdd 00:00:00")+"
                    DateTime dd = DateTime.Now.AddDays(i + 1);
                    DateTime dd2 = DateTime.Now.AddDays((i));
                    builder.Append(weekDays[n++]);
                    strWhere = " matchstate>-5 and matchtime <'" + dd.ToString("yyyyMMdd 00:00:00") + "' and matchtime >'" + dd2.ToString("yyyyMMdd 00:00:00") + "' ORDER BY convert(datetime,matchtime,120) desc";
                }
                else
                {
                    builder.Append(Out.waplink(Utils.getUrl("footballs.aspx?ptype=2&amp;act=28&amp;kind=" + kind + "&amp;State=3&amp;week=" + n + "&amp;"), weekDays[n++]));
                }
                builder.Append(" | ");
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
                string strWhereC = "ID>0 order by AddTime DESC";
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
                            builder.Append((DT.DateDiff2(DateTime.Now, Convert.ToDateTime(ds.Tables[0].Rows[koo + i]["AddTime"]))) + "前");
                            builder.Append("<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + ds.Tables[0].Rows[koo + i]["UsId"]) + "\">" + ds.Tables[0].Rows[koo + i]["UsName"] + "</a>");
                            builder.Append("收藏了");
                            builder.Append("<a  href=\"" + Utils.getUrl("footballs.aspx?act=29&amp;Id=" + ds.Tables[0].Rows[koo + i]["BasketBallId"] + "") + "\">");
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
                                Utils.Success("温馨提示", "使用彩版，页面更直观，更快捷！正在切换进入...", "footballs.aspx?act=26&amp;ve=2a&amp;page=" + pageIndex + "&amp;u=" + Utils.getstrU() + "", "1");
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
                builder.Append(Out.waplink(Utils.getUrl("footballs.aspx?act=1&amp;Id=" + 1 + ""), "再看看吧"));
                //   builder.Append(Out.waplink(Utils.getUrl("footballs.aspx?act=25&amp;Id=" + 1 + ""), "查看我的收藏") + "<br/>");
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
            string[] pageValUrl = { "act", "ptype", "id", "backurl", "State", "date", "week" };
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
                                //if (BasketBallCollect == "1")
                                //{
                                //    if (new BCW.BLL.tb_BasketBallCollect().ExistsUsIdAndBaskId(meid, Convert.ToInt32(ds.Tables[0].Rows[koo + i]["Id"])))
                                //    {
                                //        //  builder.Append(Out.waplink(Utils.getUrl("footballs.aspx?act=25&amp;Id=" + Id + ""), "[已收藏]") + "");
                                //        builder.Append("<font color=\"BLUE\">" + "[已收藏]" + "</font>");
                                //    }
                                //    else
                                //    {
                                //        builder.Append(Out.waplink(Utils.getUrl("footballs.aspx?act=31&amp;Id=" + ds.Tables[0].Rows[koo + i]["ID"] + ""), "[收藏]") + "");
                                //    }
                                //}
                                if (ds.Tables[0].Rows[koo + i]["isHidden"].ToString() == "0")
                                {
                                    builder.Append("[已隐藏]");
                                }
                                else { builder.Append("[已显示]"); }
                                builder.Append("<a  href=\"" + Utils.getUrl("footballs.aspx?act=29&amp;Id=" + ds.Tables[0].Rows[koo + i]["ID"] + "&amp;") + "\">" + Convert.ToDateTime(ds.Tables[0].Rows[koo + i]["matchtime"]).ToString("HH:mm") + "<font color=\"" + ds.Tables[0].Rows[koo + i]["addTechnic"] + "\">" + "[" + ds.Tables[0].Rows[koo + i]["classType"] + "</font>" + "]");
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
                            Utils.Success("温馨提示", "使用彩版，页面更直观，更快捷！正在切换进入...", "footballs.aspx?act=29&amp;ve=2a&amp;u=" + Utils.getstrU() + "", "1");
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
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //29 篮球一场比赛
    private void BasketMatch()
    {
        int meid = new BCW.User.Users().GetUsId();
        //if (meid == 0)
        //    Utils.Login();
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(Out.waplink(Utils.getUrl("default.aspx"), "游戏-"));
        builder.Append(Out.waplink(Utils.getUrl("footballs.aspx"), "比分管理-"));
        builder.Append(Out.waplink(Utils.getUrl("footballs.aspx?act=28&amp;"), "篮球比分-"));
        builder.Append("即时比分");
        builder.Append(Out.Tab("</div>", "<br />"));
        int Id = Utils.ParseInt(Utils.GetRequest("Id", "all", 1, "", "0"));
        int type = Utils.ParseInt(Utils.GetRequest("type", "all", 1, "", "1"));
        BCW.Model.tb_BasketBallList model = new BCW.BLL.tb_BasketBallList().Gettb_BasketBallList(Id);
        if (!new BCW.BLL.tb_BasketBallList().Exists(Id))
        {
            Utils.Success("不存在的记录！", "出错啦！不存在该球赛记录！1s后返回..", Utils.getUrl("footballs.aspx?act=1&amp;Id=" + Id + ""), "1");
        }
        builder.Append(Out.Tab("<div>", ""));
        Master.Title = model.hometeam + "VS" + model.guestteam;
        //if (model.connectId > 0)
        //{ builder.Append("[已关联ID " + model.connectId + "]"); }
        //else
        //    builder.Append("[无关联" + "]");

        //if (model.isHidden == 0)
        //{
        //    builder.Append("[已隐藏" + "]");
        //}

        builder.Append("<a href=\"" + Utils.getUrl("footballs.aspx?act=28&amp;Style=" + model.classType + "") + "\">" + "<font color=\"" + model.addTechnic + "\">" + model.classType + "</font>" + " </a> " + "@ " + Convert.ToDateTime(model.matchtime).ToString("MM月dd日 HH:mm") + "&nbsp; ");
        //收藏开关
        //if (BasketBallCollect == "1")
        //{
        //    if (new BCW.BLL.tb_BasketBallCollect().ExistsUsIdAndBaskId(meid, Id))
        //    {
        //        builder.Append("<font color=\"BLUE\">" + "[已收藏]" + "</font>");
        //    }
        //    else
        //    {
        //        builder.Append(Out.waplink(Utils.getUrl("footballs.aspx?act=31&amp;Id=" + Id + ""), "[收藏]") + "");
        //    }
        //}
        builder.Append("<br />主客队:");
        builder.Append("<a>" + model.hometeam + " </a>");
        builder.Append("-");
        builder.Append("<a>" + model.guestteam + " </a>");
        builder.Append("<br />比赛状态:" + getCount(model.matchstate.Trim()));
        builder.Append(Out.waplink(Utils.getUrl("footballs.aspx?act=29&amp;Id=" + Id + ""), " [刷]") + "");
        if (model.isDone.Contains(":"))
            builder.Append("<br/>" + "剩余时间:" + model.isDone);
        //  if (model.matchstate.Trim() != "0")
        {
            if (model.matchstate.Length > 2)
            {
                if (model.matchstate.Trim() == "-1")
                { builder.Append("<br/>" + "完场比分:" + "<font color=\"red\">" + model.homescore + "-" + model.guestscore + "</font><br/>"); }
                else
                {
                    builder.Append("<br/>" + "即时比分:" + "<font color=\"red\">" + model.homescore + "-" + model.guestscore + "</font><br/>");
                }
            }
            if (model.homeEurope.Contains(",") && model.homeEurope.Trim().Length > 5)
            {
                string[] ouzhi = model.homeEurope.Split(',');
                builder.Append("欧指:");
                builder.Append("[主] " + ouzhi[0] + " [客] " + ouzhi[1] + "<br/>");
            }
            builder.Append("〓管理〓<br/>");
            builder.Append(" " + Out.waplink(Utils.getUrl("footballs.aspx?act=32&amp;gid=" + model.ID + "&amp;Id=" + model.ID + "&amp;"), "修改"));
            builder.Append(" " + Out.waplink(Utils.getUrl("footballs.aspx?Id=" + model.ID + "&amp;act=33"), "删除"));

            builder.Append(" " + Out.waplink(Utils.getUrl("footballs.aspx?act=32&amp;Id=" + model.ID + ""), "关联"));

            if (model.isHidden == 0)
                builder.Append(" " + Out.waplink(Utils.getUrl("footballs.aspx?act=32&amp;Id=" + model.ID + "&amp;"), "隐藏"));
            else
                builder.Append(" " + Out.waplink(Utils.getUrl("footballs.aspx?act=32&amp;Id=" + model.ID + "&amp;"), "显示"));

            if (model.connectId == 0)
                builder.Append("<br />关联状态：[无关联]<br />" + Out.waplink(Utils.getUrl("footballs.aspx?Id=" + model.ID + "&amp;act=32&amp;"), "开启关联"));
            else
                builder.Append("<br />关联状态：<b>[已关联]</b><br />" + Out.waplink(Utils.getUrl("footballs.aspx?Id=" + model.ID + "&amp;act=32&amp;"), "停止关联"));

            if (model.isHidden == 1)
                builder.Append("<br />" + "显示状态:" + "<b>[已显示]</b>" + "");
            else
                builder.Append("<br />" + "显示状态:" + "[已隐藏]");
            builder.Append("" + Out.waplink(Utils.getUrl("footballs.aspx?Id=" + model.ID + "&amp;act=32&amp;"), "修改"));

            builder.Append("<br />球探网:" + model.name_en + "<br />");
            if (type == 1)
            {
                builder.Append("<b>比分直播</b> |");
            }
            else
            {
                builder.Append(Out.waplink(Utils.getUrl("footballs.aspx?ptype=1&amp;act=29&amp;Id=" + Id + "&amp;type=1"), "比分直播 ") + "|");
            }
            if (type == 2)
            {
                builder.Append(" <b>文字直播</b>");
            }
            else
            {
                builder.Append(Out.waplink(Utils.getUrl("footballs.aspx?ptype=1&amp;act=29&amp;Id=" + Id + "&amp;type=2"), " 文字直播 ") + "");
            }
            builder.Append("<br/>");
            if (type == 1)
            {
                if (model.homeone.Trim() != "")
                    builder.Append("第一节:" + "<font color=\"red\">" + model.homeone.Trim() + "-" + model.guestone.Trim() + "</font><br/>");
                if (model.hometwo.Trim() != "")
                    builder.Append("第二节:" + "<font color=\"red\">" + model.hometwo.Trim() + "-" + model.guesttwo.Trim() + "</font><br/>");
                if (model.homethree.Trim() != "")
                    builder.Append("第三节:" + "<font color=\"red\">" + model.homethree.Trim() + "-" + model.guestthree.Trim() + "</font><br/>");
                if (model.homefour.Trim() != "")
                    builder.Append("第四节:" + "<font color=\"red\">" + model.homefour.Trim() + "-" + model.guestfour.Trim() + "</font><br/>");
                if (model.explain.Length > 10)
                    builder.Append("球场数据:" + "<br/>" + model.explain + "<br/>" + model.explain2);
            }
            else
            {
                #region 文字直播开始
                //存在文字直播数据
                //直播
                //开始
                if (new BCW.BLL.tb_BasketBallWord().ExistsName(246308))
                {
                    string swhere = " name_enId = " + 246308 + " and ID<491 order by last desc";
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
                                builder.Append("[主]");
                            }
                            else if (ds.Tables[0].Rows[koo + i]["whichTeam"].ToString().Trim() == "2")
                            {
                                builder.Append("[客]");
                            }
                            else
                                builder.Append("[全]");
                            builder.Append(ds.Tables[0].Rows[koo + i]["hometeam"].ToString().Trim() + ":");
                            builder.Append(ds.Tables[0].Rows[koo + i]["guestteam"] + "");
                            builder.Append(ds.Tables[0].Rows[koo + i]["listContent"].ToString().Trim());
                            if (ds.Tables[0].Rows[koo + i]["isSame"].ToString().Trim() != "")
                            {
                                builder.Append("[" + ds.Tables[0].Rows[koo + i]["isSame"].ToString().Trim() + "]");
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
                else
                { builder.Append("暂无文字直播!" + "<br/>" + "..." + "<br/>"); }
                if (model.tv.Contains("a"))
                {
                    if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
                    {
                        Utils.Success("温馨提示", "使用彩版，页面更直观，更快捷！正在切换进入...", "footballs.aspx?act=29&amp;type=2&amp;ve=2a&amp;Id=" + Id + "&amp;u=" + Utils.getstrU() + "", "2");
                    }
                    try
                    {
                        builder.Append("存在视频直播!" + "<br/>" + model.tv);
                    }
                    catch
                    {

                    }
                }
                else
                    builder.Append("[无tv直播]");
                #endregion
            }
        }
        builder.Append(Out.Tab("</div>", "<br/>"));
        //builder.Append(Out.SysUBB(BCW.User.Users.ShowSpeak(6, "footballs.aspx?act=29&amp;Id=" + Id + "", 5, 0)));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
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
        builder.Append(Out.waplink(Utils.getUrl("footballs.aspx"), "游戏-"));
        builder.Append(Out.waplink(Utils.getUrl("footballs.aspx?act=28&amp;"), "篮球比分-"));
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
            builder.Append(Out.waplink(Utils.getUrl("footballs.aspx?act=28&amp;Id=" + Id + "&amp;State=4&amp;"), "查看我的收藏") + "<br/>");
            builder.Append(Out.waplink(Utils.getUrl("footballs.aspx?act=29&amp;Id=" + Id + ""), "返回球赛"));
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
                builder.Append(Out.waplink(Utils.getUrl("footballs.aspx?act=31&amp;Id=" + Id + "&amp;favorite=yes&amp;"), "确定收藏") + "<br/>");
            }
            builder.Append(Out.waplink(Utils.getUrl("footballs.aspx?act=28&amp;Id=" + Id + ""), "再看看吧") + "<br/>");
            builder.Append(Out.waplink(Utils.getUrl("footballs.aspx?act=28&amp;Id=" + Id + "&amp;State=4&amp;"), "查看我的收藏"));
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div>", "<br/>"));
        builder.Append("温馨提示:收藏后若比分变化将发内线通知您");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //32 更新篮球一场球赛
    private void UpdateListB()
    {
        int Id = Utils.ParseInt(Utils.GetRequest("Id", "all", 1, "", ""));
        Master.Title = "修改一场球赛的数据";
        builder.Append(Out.Tab("", ""));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        //ub xml = new ub();
        //string xmlPath = "/Controls/myyg.xml";
        //Application.Remove(xmlPath);//清缓存
        //xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            BCW.Model.tb_BasketBallList model = new BCW.BLL.tb_BasketBallList().Gettb_BasketBallList(Id);
            string classType = Utils.GetRequest("classType", "post", 2, @"^[^\^]{1,20}$", "球队类型限1-20字内");
            string matchtime = Utils.GetRequest("matchtime", "post", 2, @"^[^\^]{1,2000}$", "比赛日期输入错误");
            string name_en = Utils.GetRequest("name_en", "post", 2, @"^[^\^]{1,200}$", "球探ID输入错误");
            string home = Utils.GetRequest("home", "post", 2, @"^[^\^]{1,2000}$", "主队名称输入错误");
            string guest = Utils.GetRequest("guest", "post", 2, @"^[^\^]{1,2000}$", "客队名称输入错误");
            string matchstate = Utils.GetRequest("matchstate", "post", 2, @"^[^\^]{1,200}$", "比赛状态填写错误");
            string homescore = Utils.GetRequest("homescore", "post", 2, @"^[^\^]{1,200}$", "主队比分填写错误");
            string guestscore = Utils.GetRequest("guestscore", "post", 2, @"^[^\^]{1,2000}$", "客队比分填写错误");
            string oerzhi = Utils.GetRequest("oerzhi", "post", 2, @"^[^\^]{1,2000}$", "欧指填写错误");
            string isdone = Utils.GetRequest("isdone", "post", 2, @"^[^\^]{1,2000}$", "剩余时间填写错误");
            string home1 = Utils.GetRequest("home1", "post", 2, @"^[^\^]{1,200}$", "主队第一节比分入错误");
            string guest1 = Utils.GetRequest("guest1", "post", 2, @"^[^\^]{1,200}$", "客队第一节比分入错误");
            string home2 = Utils.GetRequest("home2", "post", 2, @"^[^\^]{1,200}$", "主队第二节比分入错误");
            string guest2 = Utils.GetRequest("guest2", "post", 2, @"^[^\^]{1,200}$", "客队第二节比分入错误");
            string home3 = Utils.GetRequest("home3", "post", 2, @"^[^\^]{1,200}$", "主队第三节比分入错误");
            string guest3 = Utils.GetRequest("guest3", "post", 2, @"^[^\^]{1,200}$", "客队第三节比分入错误");
            string home4 = Utils.GetRequest("home4", "post", 2, @"^[^\^]{1,200}$", "主队第四节比分入错误");
            string guest4 = Utils.GetRequest("guest4", "post", 2, @"^[^\^]{1,200}$", "客队第四节比分入错误");
            string tv = Utils.GetRequest("tv", "post", 1, "", "");
            string connectId = Utils.GetRequest("connectId", "post", 1, "", "");
            string isHidden = Utils.GetRequest("isHidden", "post", 1, "", "");
            DateTime dt = DateTime.Now;
            model.classType = classType;
            model.matchtime = Convert.ToDateTime(matchtime);
            model.name_en = Convert.ToInt32(name_en);
            model.hometeam = home;
            model.homeone = home1;
            model.guestone = guest1;
            model.hometwo = home2;
            model.guesttwo = guest2;
            model.homethree = home3;
            model.guestthree = guest3;
            model.homefour = home4;
            model.guestfour = guest4;
            model.guestteam = guest;
            model.matchstate = matchstate;
            model.homescore = Convert.ToInt32(homescore);
            model.guestscore = Convert.ToInt32(guestscore);
            model.homeEurope = oerzhi;
            model.isDone = isdone;
            model.tv = tv;
            model.connectId = Convert.ToInt32(connectId);
            model.isHidden = Convert.ToInt32(isHidden);
            new BCW.BLL.tb_BasketBallList().Update(model);
            //  System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("修改球队比赛数据", home + "VS" + guest + "修改成功，3s后返回..", Utils.getUrl("footballs.aspx?act=32&amp;Id=" + Id + "&amp;backurl=" + Utils.getPage(1) + ""), "3");
        }
        else
        {
            BCW.Model.tb_BasketBallList model = new BCW.BLL.tb_BasketBallList().Gettb_BasketBallList(Id);
            builder.Append(Out.Div("title", "" + GetAGameText(model.hometeam) + "VS" + GetAGameText(model.guestteam) + "-比赛数据"));
            string strText = "球队类型:/,比赛日期:/,球探编号:/,主队:/,客队:/,比赛状态:/,主队 全 场比分:,客 队全 场比分:,主队第一节比分:,客队第一节比分:,主队第二节比分:,客队第二节比分:,主队第三节比分:,客队第三节比分:,主队第四节比分:,客队第四节比分:,欧指:/,剩余时间:/,TV直播:/,关联ID:/,显示状态:/,";
            string strName = "classType,matchtime,name_en,home,guest,matchstate,homescore,guestscore,home1,guest1,home2,guest2,home3,guest3,home4,guest4,oerzhi,isdone,tv,connectId,isHidden";
            string strType = "text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,select";
            string strValu = model.classType + "'" + Convert.ToDateTime(model.matchtime).ToString("yyyy-MM-dd HH:mm:ss") + "'" + model.name_en + "'" + model.hometeam + "'" + model.guestteam + "'" + model.matchstate + "'" + model.homescore + "'" + model.guestscore + "'" + model.homeone + "'" + model.guestone + "'" + model.hometwo + "'" + model.guesttwo + "'" + model.homethree + "'" + model.homethree + "'" + model.homefour + "'" + model.guestfour + "'" + model.homeEurope + "'" + model.isDone + "'" + model.tv + "'" + model.connectId + "'" + model.isHidden + "";
            string strEmpt = "true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,0|隐藏|1|显示";
            string strIdea = "/";
            string strOthe = "确定修改|reset,footballs.aspx?act=32&amp;Id=" + Id + "&amp;,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示:TV直播为对应的视频直播地址.前台不显示.<br/>开启关联请填写对应的球赛ID,停止关联默认为0<br />");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("footballs.aspx?act=29&amp;Id=" + Id + "") + "\">返回上级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }

    //33 删除一场球赛
    private void DeleBaskId()
    {
        int Id = Utils.ParseInt(Utils.GetRequest("Id", "all", 1, "", ""));
        Master.Title = "删除一场球赛的数据";
        builder.Append(Out.Tab("", ""));
        string ac = Utils.GetRequest("ac", "post", 1, "", "");
        if (ac == "yes")
        {
            Utils.Error("" + ac + "", "");
            new BCW.BLL.tb_BasketBallList().Delete(Id);
            Utils.Success("删除球队比赛数据", "删除成功，3s后返回..", Utils.getUrl("footballs.aspx?act=28&amp;Id=" + Id + "&amp;"), "3");
        }
        else
        {
            BCW.Model.tb_BasketBallList model = new BCW.BLL.tb_BasketBallList().Gettb_BasketBallList(Id);
            builder.Append(Out.Div("title", "" + GetAGameText(model.hometeam) + "VS" + GetAGameText(model.guestteam) + "-比赛数据"));
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("确定删除吗？" + "<br/>");
            builder.Append("<a href=\"" + Utils.getUrl("footballs.aspx?act=33&amp;Id=" + Id + "&amp;ac=yes") + "\">是</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("footballs.aspx?act=29&amp;Id=" + Id + "&amp;ac=no") + "\">再看看吧</a><br />");
            builder.Append(Out.Tab("</div>", ""));
            //builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("footballs.aspx?act=29&amp;Id=" + Id + "") + "\">返回上级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
    #endregion

    #region 足球管理
    //view 游戏配置
    private void ViewPage()
    {
        Master.Title = "游戏配置";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append(Out.waplink(Utils.getUrl("footballs.aspx"), "比分管理-"));
        builder.Append("游戏配置");
        builder.Append(Out.Tab("</div>", "<br />"));
        string info = (Utils.GetRequest("info", "all", 1, @"", ""));
        //  int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-2]\d*$", "1"));
        ub xml = new ub();
        string xmlPath = "/Controls/footballs.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("显示设置");
        builder.Append(Out.Tab("</div>", ""));
        if (info == "ok")
        {
            //BallQiuChangBallOpenSpeakBallJinQiu
            string BallPeiLu = Utils.GetRequest("peilv", "all", 1, "", "");
            //   string Open = Utils.GetRequest("open", "all", 1, "", "");
            string BallJinQiu = Utils.GetRequest("jinqiu", "all", 1, "", "");
            string BallSpeak = Utils.GetRequest("speakStyle", "all", 1, "", "");
            string BallQiuChang = Utils.GetRequest("data", "all", 1, "", "");
            string BallOpenSpeak = Utils.GetRequest("speak", "all", 1, "", "");
            string speakSecond = Utils.GetRequest("speakSecond", "all", 1, "", "");
            string deletetext = Utils.GetRequest("deletetext", "all", 1, "", "");
            //  xml.dss["Open"] = Open;
            xml.dss["BallPeiLu"] = BallPeiLu;
            xml.dss["BallJinQiu"] = BallJinQiu;
            xml.dss["BallQiuChang"] = BallQiuChang;
            xml.dss["BallSpeak"] = BallSpeak;
            xml.dss["BallOpenSpeak"] = BallOpenSpeak;
            xml.dss["deletetext"] = deletetext;
            xml.dss["BallSpeakTime"] = speakSecond;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("足球直播设置", "设置成功，正在返回..", Utils.getUrl("footballs.aspx?act=view&amp;ve=2a;backurl=" + Utils.getPage(0) + ""), "1");
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("暂无相关修改");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            string strText1 = "赔率数据:/,进球时刻:/,球场数据:/,发言开关(新):/,闲聊方式:/,发言间隔(秒):/,敏感文字检测(#分隔):/,";
            string strName1 = "peilv,jinqiu,data,speak,speakStyle,speakSecond,deletetext,backurl";
            string strType1 = "select,select,select,select,select,num,text,hidden";
            string strValu1 = xml.dss["BallPeiLu"] + "'" + xml.dss["BallJinQiu"] + "'" + xml.dss["BallQiuChang"] + "'" + xml.dss["BallOpenSpeak"] + "'" + xml.dss["BallSpeak"] + "'" + xml.dss["BallSpeakTime"] + "'" + "'" + Utils.getPage(0) + "";
            string strEmpt1 = "0|隐藏|1|显示,0|隐藏|1|显示,0|隐藏|1|显示,0|隐藏|1|显示,0|统一聊天|1|单一聊天|2|使用原闲聊,true,true,false";
            string strIdea1 = "/";
            string strOthe1 = "确定修改,footballs.aspx?act=view&amp;info=ok&amp;,post,1,red";
            builder.Append(Out.wapform(strText1, strName1, strType1, strValu1, strEmpt1, strIdea1, strOthe1));
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("footballs.aspx") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //allballlist 全部记录
    private void AllBalllistPage()
    {
        Master.Title = "比分管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(Out.waplink(Utils.getUrl("default.aspx"), "游戏-"));
        builder.Append(Out.waplink(Utils.getUrl("footballs.aspx"), "比分管理-"));
        builder.Append("全部足球");
        builder.Append(Out.Tab("</div>", "<br/>"));
        //builder.Append(Out.Tab("<div class=\"text\">", ""));
        int kind = 1;
        string strWhere = "";
        int State = Utils.ParseInt(Utils.GetRequest("State", "all", 1, "", "1"));
        string date = (Utils.GetRequest("date", "all", 1, "", ""));
        if (date == "")
            date = DateTime.Now.AddHours(5).ToString("yyyy-MM-dd");
        strWhere = "convert(datetime, ft_time+convert(datetime, ft_caipan, 8), 120) > '" + date + "'" + "order by convert(datetime, ft_time+convert(datetime, ft_caipan, 8), 120) asc";
        string datetimeout = "";
        string textwhere = "";
        DataSet ds = new BCW.BLL.tb_ZQLists().GetList(" * ", strWhere);
        // builder.Append(ds.Tables[0].Rows.Count);
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string[] pageValUrl = { "act", "ptype", "id", "backurl", "State", "date" };
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
                        { builder.Append(Out.Tab("<div >", "")); }
                        else
                        {
                            if (i == 1)
                                builder.Append(Out.Tab("<div class=\"text\">", ""));
                            else
                                builder.Append(Out.Tab("<div class=\"text\">", ""));
                        }
                        if (datetimeout != Convert.ToDateTime(ds.Tables[0].Rows[koo + i]["ft_time"]).ToString("MM月dd日"))
                        {
                            datetimeout = Convert.ToDateTime(ds.Tables[0].Rows[koo + i]["ft_time"]).ToString("MM月dd日");
                            builder.Append("<b>" + datetimeout + "</b><br/>");
                        }
                        if (ds.Tables[0].Rows[koo + i]["Identification"].ToString() == "0")
                        {
                            builder.Append("[已隐藏]");
                        }
                        else
                        {
                            builder.Append("<font color=\"red\">" + "[显示中]" + "</font>");
                        }
                        //TagsChecker.fix(str1)
                        //  builder.Append(Convert.ToDateTime(ds.Tables[0].Rows[koo + i]["ft_time"]).ToString("MM月dd日"));
                        builder.Append("<a  href=\"" + Utils.getUrl("footballs.aspx?act=onelist&amp;Id=" + ds.Tables[0].Rows[koo + i]["Id"] + "&amp;") + "\">" + ds.Tables[0].Rows[koo + i]["ft_caipan"] + "[" + ds.Tables[0].Rows[koo + i]["ft_teamStyle"] + "]");
                        //   builder.Append(ds.Tables[0].Rows[koo + i]["ft_caipan"] + "[" + ds.Tables[0].Rows[koo + i]["ft_teamStyle"] + "]");
                        builder.Append(BCW.Footballs.checkCodeker.fix(ds.Tables[0].Rows[koo + i]["ft_team1"].ToString()));
                        if (State == 0)
                        {
                            builder.Append("VS");
                        }
                        else
                        {
                            builder.Append("(" + "<font color=\"red\">" + ds.Tables[0].Rows[koo + i]["ft_result"] + "</font>" + ")");
                        }
                        builder.Append(BCW.Footballs.checkCodeker.fix(ds.Tables[0].Rows[koo + i]["ft_team2"].ToString()) + "</a>");
                        // if (State == 1)
                        {
                            builder.Append(ds.Tables[0].Rows[koo + i]["ft_state"]);
                        }
                        // builder.Append(ds.Tables[0].Rows[koo + i]["ft_state"]);
                        // builder.Append("<br/>");
                        // builder.Append(Out.waplink(Utils.getUrl("footballs.aspx?act=onelist&amp;Id=" + ds.Tables[0].Rows[koo + i]["Id"] + "&amp;"), "[" + textout + "]") + "");
                        k++;
                        datetimeout = Convert.ToDateTime(ds.Tables[0].Rows[koo + i]["ft_time"]).ToString("MM月dd日");
                        if (i == 9)
                        {
                            builder.Append(Out.Tab("</div>", ""));
                        }
                        else
                            builder.Append(Out.Tab("</div>", "<br/>"));
                    }
                }
                catch
                {
                    if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))//u=" + Utils.getstrU() + "&amp;
                    {
                        Utils.Success("温馨提示", "使用彩版，页面更直观，更快捷！正在切换进入...", "footballs.aspx?act=balllist&amp;ve=2a&amp;u=" + Utils.getstrU() + "", "1");
                    }
                }
            }
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Tab("<div>", "<br/>"));
            builder.Append(textwhere);
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows.Count < 3)
        {
            builder.Append(Out.Tab("<div>", "<br/>"));
            builder.Append(textwhere);
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        string strText1 = "【赛事速查】/日期(格式如:2016-07-08):/,";
        string strName1 = "date,backurl";
        string strType1 = "date,hidden";
        string strValu1 = date + "'" + Utils.getPage(0) + "";
        string strEmpt1 = "true,false";
        string strIdea1 = "";
        string strOthe1 = "确定搜索,footballs.aspx?act=allballlist&amp;,post,1,red";
        builder.Append(Out.wapform(strText1, strName1, strType1, strValu1, strEmpt1, strIdea1, strOthe1));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("footballs.aspx") + "\">返回上级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //搜索球队记录
    private void FindBalllistPage()
    {
        Master.Title = "赛事速查";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append(Out.waplink(Utils.getUrl("footballs.aspx"), "比分管理-"));
        builder.Append("赛事速查");
        builder.Append(Out.Tab("</div>", "<br />"));
        string date = (Utils.GetRequest("date", "all", 1, "", ""));
        int State = Utils.ParseInt(Utils.GetRequest("State", "all", 1, "", ""));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("选择查找的命令");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        if (State == 0)
        {
            builder.Append("<b>按ID</b>");
            string strText1 = "ID:,";
            string strName1 = "ID,backurl";
            string strType1 = "text,hidden";
            string strValu1 = "'" + Utils.getPage(0) + "";
            string strEmpt1 = "true,false";
            string strIdea1 = "";
            string strOthe1 = "确定搜索,footballs.aspx?act=onelist&amp;,post,1,red";
            builder.Append(Out.wapform(strText1, strName1, strType1, strValu1, strEmpt1, strIdea1, strOthe1));
            builder.Append("<br/>");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("footballs.aspx?act=find&amp;State=0&amp;") + "\">按ID</a><br />");
        }
        if (State == 1)
        {
            if (date == "")
            {
                date = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            }
            builder.Append("<b>按时间</b>");
            string strText1 = "日期:,";
            string strName1 = "date,backurl";
            string strType1 = "date,hidden";
            string strValu1 = date + "'" + Utils.getPage(0) + "";
            string strEmpt1 = "true,false";
            string strIdea1 = "";
            string strOthe1 = "确定搜索,footballs.aspx?act=day&amp;,post,1,red";
            builder.Append(Out.wapform(strText1, strName1, strType1, strValu1, strEmpt1, strIdea1, strOthe1));
            builder.Append("<br/>");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("footballs.aspx?act=find&amp;State=1&amp;") + "\">按时间</a><br />");
        }
        if (State == 2)
        {
            builder.Append("<b>按球队</b>");
            string strText1 = "球队名称:,";
            string strName1 = "team,backurl";
            string strType1 = "text,hidden";
            string strValu1 = "'" + Utils.getPage(0) + "";
            string strEmpt1 = "true,false";
            string strIdea1 = "";
            string strOthe1 = "确定搜索,footballs.aspx?act=team&amp;,post,1,red";
            builder.Append(Out.wapform(strText1, strName1, strType1, strValu1, strEmpt1, strIdea1, strOthe1));
            builder.Append("<br/>");
        }
        else
            builder.Append("<a href=\"" + Utils.getUrl("footballs.aspx?act=find&amp;State=2&amp;") + "\">按球队</a><br />");
        if (State == 3)
        {
            builder.Append("<b>按类型</b>");
            string strText1 = "比赛类型:,";
            string strName1 = "style,backurl";
            string strType1 = "text,hidden";
            string strValu1 = "欧锦赛" + "'" + Utils.getPage(0) + "";
            string strEmpt1 = "true,false";
            string strIdea1 = "";
            string strOthe1 = "确定搜索,footballs.aspx?act=style&amp;,post,1,red";
            builder.Append(Out.wapform(strText1, strName1, strType1, strValu1, strEmpt1, strIdea1, strOthe1));
            builder.Append("<br/>");
        }
        else
            builder.Append("<a href=\"" + Utils.getUrl("footballs.aspx?act=find&amp;State=3&amp;") + "\">按类型</a><br />");
        if (State == 4)
        {
            builder.Append("<b>按点击</b>");
            //   builder.Append("<br/>");//, convert(datetime,ft_time,120) ASC,convert(datetime,ft_caipan,120)
            string strWhere = " isDone=1 ORDER BY ft_Hit  DESC";
            DataSet ds = new BCW.BLL.tb_ZQLists().GetList(" * ", strWhere);
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string[] pageValUrl = { "act", "ptype", "id", "backurl", "Style", "State" };
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
                        if (i % 2 == 0)
                        { builder.Append(Out.Tab("<div class=\"text\">", "<br/>")); }
                        else
                        {
                            if (i == 1)
                                builder.Append(Out.Tab("<div>", "<br/>"));
                            else
                                builder.Append(Out.Tab("<div>", "<br/>"));
                        }
                        //if (datetimeout != Convert.ToDateTime(ds.Tables[0].Rows[koo + i]["ft_time"]).ToString("MM月dd日"))
                        //{
                        //    datetimeout = Convert.ToDateTime(ds.Tables[0].Rows[koo + i]["ft_time"]).ToString("MM月dd日");
                        //    builder.Append("<b>" + datetimeout + "</b><br/>");
                        //}
                        builder.Append("[" + "<font color=\"blue\">" + ds.Tables[0].Rows[koo + i]["ft_Hit"] + "</font>" + "]");
                        //              builder.Append("<a  href=\"" + Utils.getUrl("footballs.aspx?act=21&amp;Id=" + ds.Tables[0].Rows[koo + i]["Id"] + "&amp;") + "\">" + ds.Tables[0].Rows[koo + i]["ft_caipan"] + "[" + GetRed(ds.Tables[0].Rows[koo + i]["ft_teamStyle"].ToString(), Style) + "]");
                        builder.Append("<a  href =\"" + Utils.getUrl("footballs.aspx?act=onelist&amp;Id=" + ds.Tables[0].Rows[koo + i]["Id"] + "&amp;") + "\">" + ds.Tables[0].Rows[koo + i]["ft_team1"]);
                        if (ds.Tables[0].Rows[koo + i]["ft_result"].ToString() == "")
                        {
                            builder.Append("<font color=\"red\">" + "VS" + "</font>");
                        }
                        else
                        {
                            builder.Append("(" + "<font color=\"red\">" + ds.Tables[0].Rows[koo + i]["ft_result"] + "</font>" + ")");
                        }
                        builder.Append(ds.Tables[0].Rows[koo + i]["ft_team2"].ToString() + "</a>");
                        // builder.Append("<br/>");
                        // builder.Append(Out.waplink(Utils.getUrl("footballs.aspx?act=21&amp;Id=" + ds.Tables[0].Rows[koo + i]["Id"] + "&amp;"), "[" + textout + "]") + "");
                        k++;
                        //  datetimeout = Convert.ToDateTime(ds.Tables[0].Rows[koo + i]["ft_time"]).ToString("MM月dd日");
                        builder.Append(Out.Tab("</div>", ""));
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
            builder.Append("<br/>");
        }
        else
            builder.Append("<a href=\"" + Utils.getUrl("footballs.aspx?act=find&amp;State=4&amp;") + "\">按点击</a><br />");
        if (State == 5)
        {
            builder.Append("<b>按收藏</b>");
            //builder.Append("<br/>");
            ////SELECT count(FootBallId) as a from dbo.tb_ZQCollection GROUP BY(FootBallId) order by a desc
            string datetimeout = "";
            string strWhere = " ID>0  GROUP by FootBallId order by a desc  ";
            DataSet ds = new BCW.BLL.tb_ZQCollection().GetList(" count(FootBallId) as a,FootBallId ", strWhere);
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string[] pageValUrl = { "act", "ptype", "id", "backurl", "Style", "State" };
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
                    if (ds.Tables[0].Rows[koo + i]["a"].ToString() != "" && ds.Tables[0].Rows[koo + i]["a"].ToString() != "")
                    {
                        BCW.Model.tb_ZQLists model = new BCW.BLL.tb_ZQLists().Gettb_ZQLists(Convert.ToInt32(ds.Tables[0].Rows[koo + i]["FootBallId"]));
                        if (i % 2 == 0)
                        { builder.Append(Out.Tab("<div class=\"text\">", "<br/>")); }
                        else
                        {
                            if (i == 1)
                                builder.Append(Out.Tab("<div>", "<br/>"));
                            else
                                builder.Append(Out.Tab("<div>", "<br/>"));
                        }
                        //if (datetimeout != Convert.ToDateTime(ds.Tables[0].Rows[koo + i]["ft_time"]).ToString("MM月dd日"))
                        //{
                        //    datetimeout = Convert.ToDateTime(ds.Tables[0].Rows[koo + i]["ft_time"]).ToString("MM月dd日");
                        //    builder.Append("<b>" + datetimeout + "</b><br/>");
                        //}
                        builder.Append("[" + "<font color=\"blue\">" + ds.Tables[0].Rows[koo + i]["a"] + "</font>" + "]");
                        //              builder.Append("<a  href=\"" + Utils.getUrl("footballs.aspx?act=21&amp;Id=" + ds.Tables[0].Rows[koo + i]["Id"] + "&amp;") + "\">" + ds.Tables[0].Rows[koo + i]["ft_caipan"] + "[" + GetRed(ds.Tables[0].Rows[koo + i]["ft_teamStyle"].ToString(), Style) + "]");
                        builder.Append("<a href =\"" + Utils.getUrl("footballs.aspx?act=onelist&amp;Id=" + model.Id + "&amp;") + "\">" + model.ft_team1);
                        if (model.ft_result.ToString() == "")
                        {
                            builder.Append("<font color=\"red\">" + "VS" + "</font>");
                        }
                        else
                        {
                            builder.Append("(" + "<font color=\"red\">" + model.ft_result.ToString() + "</font>" + ")");
                        }
                        builder.Append(model.ft_team2 + "</a>");
                        // builder.Append("<br/>");
                        // builder.Append(Out.waplink(Utils.getUrl("footballs.aspx?act=21&amp;Id=" + ds.Tables[0].Rows[koo + i]["Id"] + "&amp;"), "[" + textout + "]") + "");
                        k++;
                        //  datetimeout = Convert.ToDateTime(ds.Tables[0].Rows[koo + i]["ft_time"]).ToString("MM月dd日");
                        builder.Append(Out.Tab("</div>", ""));
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
            builder.Append("<br/>");
        }
        else
            builder.Append("<a href=\"" + Utils.getUrl("footballs.aspx?act=find&amp;State=5&amp;") + "\">按收藏</a><br />");
        if (State == 6)
        {
            builder.Append("<b>按讨论</b>");
            //builder.Append("<br/>");
            ////SELECT count(FootBallId) as a from dbo.tb_ZQCollection GROUP BY(FootBallId) order by a desc
            string strWhere = "ID>0 GROUP by toFootID order by a desc ";
            DataSet ds = new BCW.BLL.tb_ZQChact().GetList(" COUNT(toFootID) as a,toFootID ", strWhere);
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string[] pageValUrl = { "act", "ptype", "id", "backurl", "Style", "State" };
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
                    BCW.Model.tb_ZQLists model = new BCW.BLL.tb_ZQLists().Gettb_ZQLists(Convert.ToInt32(ds.Tables[0].Rows[koo + i]["toFootID"]));
                    if (ds.Tables[0].Rows[koo + i]["toFootID"].ToString() != "" && ds.Tables[0].Rows[koo + i]["toFootID"].ToString() != "")
                    {
                        if (i % 2 == 0)
                        { builder.Append(Out.Tab("<div class=\"text\">", "<br/>")); }
                        else
                        {
                            if (i == 1)
                                builder.Append(Out.Tab("<div>", "<br/>"));
                            else
                                builder.Append(Out.Tab("<div>", "<br/>"));
                        }
                        //if (datetimeout != Convert.ToDateTime(ds.Tables[0].Rows[koo + i]["ft_time"]).ToString("MM月dd日"))
                        //{
                        //    datetimeout = Convert.ToDateTime(ds.Tables[0].Rows[koo + i]["ft_time"]).ToString("MM月dd日");
                        //    builder.Append("<b>" + datetimeout + "</b><br/>");
                        //}
                        builder.Append("[" + "<font color=\"blue\">" + ds.Tables[0].Rows[koo + i]["a"] + "</font>" + "]");
                        builder.Append("<a href=\"" + Utils.getUrl("footballs.aspx?act=onelist&amp;Id=" + model.Id + "&amp;") + "\">");
                        builder.Append(GetAGameText(model.ft_team1));
                        if (model.ft_result.ToString() == "")
                        {
                            builder.Append("<font color=\"red\">" + "VS" + "</font>");
                        }
                        else
                        {
                            builder.Append("(" + "<font color=\"red\">" + model.ft_result.ToString() + "</font>" + ")");
                        }
                        builder.Append(GetAGameText(model.ft_team2) + "</a>");
                        // builder.Append("<br/>");
                        // builder.Append(Out.waplink(Utils.getUrl("footballs.aspx?act=21&amp;Id=" + ds.Tables[0].Rows[koo + i]["Id"] + "&amp;"), "[" + textout + "]") + "");
                        k++;
                        //  datetimeout = Convert.ToDateTime(ds.Tables[0].Rows[koo + i]["ft_time"]).ToString("MM月dd日");
                        builder.Append(Out.Tab("</div>", ""));
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
        else if (State == 7)
            builder.Append("<a href=\"" + Utils.getUrl("footballs.aspx?act=find&amp;State=6&amp;") + "\">按讨论</a><br />");
        else
            builder.Append("<a href=\"" + Utils.getUrl("footballs.aspx?act=allballlist&amp;State=6&amp;") + "\">按全部</a><br />");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
        builder.Append("查找完毕");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //balllist 今日|完场|未开
    private void BalllistPage()
    {
        //  FrashAll();
        Master.Title = "比分管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(Out.waplink(Utils.getUrl("default.aspx"), "游戏-"));
        builder.Append(Out.waplink(Utils.getUrl("footballs.aspx"), "比分管理-"));
        builder.Append("足球比分");
        builder.Append(Out.Tab("</div>", "<br/>"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        int kind = 1;
        int State = Utils.ParseInt(Utils.GetRequest("State", "all", 1, "", "1"));
        string date = (Utils.GetRequest("date", "all", 1, "", ""));
        string title = "";
        string textout = "";
        string strWhere = "";
        string textwhere = "";
        string textresult = "";
        if (State == 1)
        {
            title = "进行赛事"; //and convert(datetime, ft_time+convert(datetime, ft_caipan, 8), 120)> DATEADD(hour, -1, GETDATE())  and convert(datetime,ft_time+convert(datetime,ft_caipan,8),120)< DATEADD(hour,24,GETDATE())
            strWhere = " ft_state!='完' and ft_state!='推迟'  and ft_state!='待定' and convert(datetime,ft_time+convert(datetime,ft_caipan,8),120)< DATEADD(hour,7,GETDATE()) and convert(datetime,ft_time+convert(datetime,ft_caipan,8),120)> DATEADD(hour,-3,GETDATE()) ORDER BY convert(datetime,ft_time,120) ASC,convert(datetime,ft_caipan,120) ASC ";
            textout = "直播";
            builder.Append(" 进 行 |");
            textwhere = "暂无进行赛事.";
        }
        else
        {
            builder.Append(Out.waplink(Utils.getUrl("footballs.aspx?ptype=1&amp;act=balllist&amp;kind=" + kind + "&amp;State=1"), " 进 行 |") + "");
        }
        if ((State == 2))
        {
            builder.Append(" 完 场 |");
            title = "完场赛事";
            strWhere = " ft_state='完'  ORDER BY convert(datetime,ft_time,120) DESC,convert(datetime,ft_caipan,120) DESC";
            textout = "完";
            textwhere = "暂无完场赛事.";
        }
        else
        {
            builder.Append(Out.waplink(Utils.getUrl("footballs.aspx?ptype=1&amp;act=balllist&amp;kind=" + kind + "&amp;State=2"), " 完 场 |") + "");
        }
        if (State == 0)
        {
            //   FrashAll(); 刷新全部进行中赛事
            //and convert(datetime,ft_time,120)> DATEADD(day,-1,GETDATE())  时间比较
            title = "未开赛事";
            strWhere = "ft_state='未' and convert(datetime, ft_time+convert(datetime, ft_caipan, 8), 120)> DATEADD(hour, -1, GETDATE())  ORDER BY convert(datetime,ft_time,120) ASC,convert(datetime,ft_caipan,120) ASC";
            textout = "未";
            builder.Append(" 未 开 ");
            textwhere = "暂无未开赛事.";
        }
        else
        {
            builder.Append(Out.waplink(Utils.getUrl("footballs.aspx?ptype=1&amp;act=balllist&amp;kind=" + kind + "&amp;State=0"), " 未 开 ") + "");
        }
        if (State == 4)
        {
            // FrashAll();  刷新全部进行中的赛事
            builder.Append("| 收 藏 ");
            textwhere = "暂无未开赛事.";
        }
        else
        {
            builder.Append(Out.waplink(Utils.getUrl("footballs.aspx?ptype=1&amp;act=balllist&amp;kind=" + kind + "&amp;State=4"), "| 收 藏 ") + "");
        }
        builder.Append(Out.Tab("</div>", ""));

        #region
        //时间拼接  getDate()< convert(datetime,ft_time,120)+convert(datetime,ft_caipan,120)
        //builder.Append(Out.Tab("<div class=\"text\">", ""));
        //builder.Append("足球" + title + "");
        //builder.Append(Out.Tab("</div>", "<br />"));
        //builder.Append(Out.Tab("<div>", "<br />"));
        //builder.Append("<b>" + DateTime.Now.ToString("MM月dd日") + "</b>");
        //builder.Append(Out.Tab("</div>", "<br />"));
        #endregion

        if (State == 4)
        {
            string strWherel = " UsId>0 order by AddTime DESC";
            DataSet ds = new BCW.BLL.tb_ZQCollection().GetList(" * ", strWherel);
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
                        string name = new BCW.BLL.User().GetUsName(Convert.ToInt32(ds.Tables[0].Rows[koo + i]["UsId"]));
                        builder.Append(DT.DateDiff2(DateTime.Now, Convert.ToDateTime(ds.Tables[0].Rows[koo + i]["AddTime"])) + "前");
                        builder.Append("<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + ds.Tables[0].Rows[koo + i]["UsId"]) + "\">" + name + "</a>");
                        builder.Append("收藏了");
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
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            //else
            //{
            //    builder.Append(Out.Tab("<div>", "<br/>"));
            //    builder.Append("暂无收藏！");
            //    builder.Append(Out.Tab("</div>", "<br/>"));
            //}
        }
        else
        {
            string datetimeout = "";
            DataSet ds = new BCW.BLL.tb_ZQLists().GetList(" * ", strWhere);
            // builder.Append(ds.Tables[0].Rows.Count);
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
                            if (ds.Tables[0].Rows[koo + i]["Identification"].ToString() == "0")
                            {
                                builder.Append("[已隐藏]");
                            }
                            else
                            {
                                builder.Append("<font color=\"red\">" + "[显示中]" + "</font>");
                            }
                            //TagsChecker.fix(str1)
                            //  builder.Append(Convert.ToDateTime(ds.Tables[0].Rows[koo + i]["ft_time"]).ToString("MM月dd日"));
                            builder.Append("<a  href=\"" + Utils.getUrl("footballs.aspx?act=onelist&amp;Id=" + ds.Tables[0].Rows[koo + i]["Id"] + "&amp;") + "\">" + ds.Tables[0].Rows[koo + i]["ft_caipan"] + "[" + ds.Tables[0].Rows[koo + i]["ft_teamStyle"] + "]");
                            //   builder.Append(ds.Tables[0].Rows[koo + i]["ft_caipan"] + "[" + ds.Tables[0].Rows[koo + i]["ft_teamStyle"] + "]");
                            builder.Append(BCW.Footballs.checkCodeker.fix(ds.Tables[0].Rows[koo + i]["ft_team1"].ToString()));
                            if (ds.Tables[0].Rows[koo + i]["ft_state"].ToString().Trim() == "完")
                            {
                                builder.Append("(" + "<font color=\"red\">" + ds.Tables[0].Rows[koo + i]["ft_result"] + "</font>" + ")");
                            }
                            else
                            {
                                builder.Append("VS");
                            }
                            builder.Append(BCW.Footballs.checkCodeker.fix(ds.Tables[0].Rows[koo + i]["ft_team2"].ToString()) + "</a>");
                            if (State == 1)
                            {
                                builder.Append(ds.Tables[0].Rows[koo + i]["ft_state"]);
                            }
                            // builder.Append("<br/>");
                            // builder.Append(Out.waplink(Utils.getUrl("footballs.aspx?act=onelist&amp;Id=" + ds.Tables[0].Rows[koo + i]["Id"] + "&amp;"), "[" + textout + "]") + "");
                            k++;
                            datetimeout = Convert.ToDateTime(ds.Tables[0].Rows[koo + i]["ft_time"]).ToString("MM月dd日");
                            builder.Append(Out.Tab("</div>", ""));
                        }
                    }
                    catch
                    {
                        if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))//u=" + Utils.getstrU() + "&amp;
                        {
                            Utils.Success("温馨提示", "使用彩版，页面更直观，更快捷！正在切换进入...", "footballs.aspx?act=balllist&amp;ve=2a&amp;u=" + Utils.getstrU() + "", "1");
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
            if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows.Count < 3)
            {
                builder.Append(Out.Tab("<div>", "<br/>"));
                builder.Append(textwhere);
                builder.Append(Out.Tab("</div>", "<br/>"));
            }
            if ((State == 2))//&& ds.Tables[0].Rows.Count > 0
            {
                if (date == "")
                {
                    date = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                }
                //builder.Append("【赛事速查】");
                string strText1 = "【赛事速查】/日期:,";
                string strName1 = "date,backurl";
                string strType1 = "date,hidden";
                string strValu1 = date + "'" + Utils.getPage(0) + "";
                string strEmpt1 = "true,false";
                string strIdea1 = "";
                string strOthe1 = "确定搜索,footballs.aspx?act=day&amp;,post,1,red";
                builder.Append(Out.wapform(strText1, strName1, strType1, strValu1, strEmpt1, strIdea1, strOthe1));
            }
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("footballs.aspx") + "\">返回上级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //检查数据 打开新页面
    private void Check(int Id)
    {
        BCW.Model.tb_ZQLists model = new BCW.BLL.tb_ZQLists().Gettb_ZQLists(Id);
        string url = "http://3g.8bo.com/3g/football/score/history.aspx?date=" + Convert.ToDateTime(model.ft_time).ToString("yyyy/MM/dd").ToString() + "&st=hasCompletedField&by=detail&eid=" + new BCW.BLL.tb_ZQLists().GetBianhaoFromId(Id);
        if (model.ft_state.Contains("未"))
        {
            url = "http://3g.8bo.com/3g/football/score/today.aspx?st=notStart&by=detail&eid=" + model.ft_bianhao;
        }
        else if (Convert.ToDateTime(model.ft_time).ToString("yyyyMMdd") == DateTime.Now.ToString("yyyyMMdd"))
        {
            url = "http://3g.8bo.com/3g/football/score/today.aspx?st=hasCompletedField&by=detail&eid=" + model.ft_bianhao;
        }
        string html = GetHtmlSource(url, Encoding.UTF8);
        int i = -1;
        if (html.Contains("暂无数据"))
        {
            url = "http://3g.8bo.com/3g/football/score/history.aspx?date=" + Convert.ToDateTime(model.ft_time).AddDays(i).ToString("yyyy/MM/dd").ToString() + "&st=hasCompletedField&by=detail&eid=" + new BCW.BLL.tb_ZQLists().GetBianhaoFromId(Id);
            html = GetHtmlSource(url, Encoding.UTF8);
            i--;
        }
        //if (html.Contains("暂无数据"))
        //{
        //    url = "http://3g.8bo.com/3g/football/score/history.aspx?date=" + Convert.ToDateTime(model.ft_time).AddDays(i).ToString("yyyy/MM/dd").ToString() + "&st=hasCompletedField&by=detail&eid=" + new BCW.BLL.tb_ZQLists().GetBianhaoFromId(Id);
        //    html = GetHtmlSource(url, Encoding.UTF8);
        //    i--;
        //}
        builder.Append("<script language='javascript'>window.open('" + url + "');</script>");
    }

    //刷新一场记录数据
    private void Frash(int Id)
    {
        string jinqiu = "";
        string gunqiu = "";
        ////取进行中球赛更新
        // http://3g.8bo.com/3g/football/score/today.aspx?st=hasStart&by=detail&eid=865666
        BCW.Model.tb_ZQLists model = new BCW.BLL.tb_ZQLists().Gettb_ZQLists(Id);
        if (model.ft_state.Contains("未") && DateTime.Now < Convert.ToDateTime(model.ft_time))
        {
            Utils.Success("未到开赛时间", "未到开赛时间", Utils.getUrl("footballs.aspx?act=onelist&amp;Id=" + Id + "&amp;backurl=" + Utils.getPage(1) + ""), "2");

        }
        string url2 = "http://3g.8bo.com/3g/football/score/history.aspx?date=" + Convert.ToDateTime(model.ft_time).ToString("yyyy/MM/dd").ToString() + "&st=hasCompletedField&by=detail&eid=" + model.ft_bianhao.ToString();
        if (model.ft_state != "未" && model.ft_state != "推迟" && model.ft_state != "腰斩")
        {
            url2 = "http://3g.8bo.com/3g/football/score/today.aspx?st=hasStart&by=detail&eid=" + model.ft_bianhao;
        }
        else
        {
            url2 = "http://3g.8bo.com/3g/football/score/history.aspx?date=" + Convert.ToDateTime(model.ft_time).ToString("yyyy/MM/dd").ToString() + "&type=0&st=hasCompletedField&by=detail&eid=" + new BCW.BLL.tb_ZQLists().GetBianhaoFromId(Id);
        }
        string result1 = "";
        string state1 = "";
        DateTime dt;
        string htmlText = GetHtmlSource(url2, Encoding.UTF8);
        if (htmlText.Contains("暂无数据"))
        {
            url2 = "http://3g.8bo.com/3g/football/score/history.aspx?date=" + Convert.ToDateTime(model.ft_time).AddDays(-1).ToString("yyyy/MM/dd").ToString() + "&type=0&st=hasCompletedField&by=detail&eid=" + new BCW.BLL.tb_ZQLists().GetBianhaoFromId(Id);
            htmlText = GetHtmlSource(url2, Encoding.UTF8);
        }
        //if (htmlText.Contains("暂无数据"))http://3g.8bo.com/3g/football/score/history.aspx?date=2016/07/29&type=0&lid=&by=detail&eid=865126
        //{http://3g.8bo.com/3g/football/score/history.aspx?date=2016/07/29&type=0&lid=432,522,219,251,364,196,232,45,150,1511&by=detail&eid=863746
        //    url2 = "http://3g.8bo.com/3g/football/score/history.aspx?date=" + Convert.ToDateTime(model.ft_time).AddDays(-2).ToString("yyyy/MM/dd").ToString() + "&st=hasCompletedField&by=detail&eid=" + new BCW.BLL.tb_ZQLists().GetBianhaoFromId(Id);
        //    htmlText = GetHtmlSource(url2, Encoding.UTF8);
        //}
        //if (htmlText.Contains("暂无数据"))
        //{
        //    url2 = "http://3g.8bo.com/3g/football/score/history.aspx?date=" + Convert.ToDateTime(model.ft_time).AddDays(-23).ToString("yyyy/MM/dd").ToString() + "&st=hasCompletedField&by=detail&eid=" + new BCW.BLL.tb_ZQLists().GetBianhaoFromId(Id);
        //    htmlText = GetHtmlSource(url2, Encoding.UTF8);
        //}
        gunqiu = getPeilvForLianSai(htmlText);
        state1 = getStateForLianSai(htmlText);
        result1 = getResult(htmlText);
        Regex regex = new Regex(@"<table.*?>[\s\S]*?<\/table>");
        MatchCollection mc = regex.Matches(htmlText);
        //获取集合类中自己需要的某个table
        string newHtmlStr = "";
        try
        {
            if (mc[0].ToString().Length > 5)
            {
                newHtmlStr = mc[0].ToString();
                //  Response.Write(mc[0].ToString());
            }
        }
        catch (Exception ee)
        {
            //Response.Write("<br />" +ee.ToString()+ "****************" + "我取table1" + "<br />");
        }
        //将数据返回
        string[] hr = Regex.Split(newHtmlStr, (@"<hr size=""1"""));
        int isNum = 1;//默认mc1捉到
        string hitball = "";
        //  Response.Write("<br />" + "****************" + "我取table1" + "<br />");
        try
        {
            if (mc[1].ToString().Length > 5)
            {
                hitball = mc[1].ToString();
                //  Response.Write(mc[1].ToString());
            }
            else
            { isNum = 2; }
        }
        catch
        {
            isNum = 2;//默认mc1捉不到,null
        }

        int idd = new BCW.BLL.tb_ZQLists().GetIdFromBianhao(Convert.ToInt32(model.ft_bianhao));
        if (state1 != "" && result1 != "")
        {
            // Response.Write(FootbzlistHtml(htmlText));

            BCW.Model.tb_ZQLists ftt = new BCW.BLL.tb_ZQLists().Gettb_ZQLists(idd);
            //  ftt.ft_addTime = DateTime.Now;
            // ftt.ft_bianhao = Convert.ToInt32(ds.Tables[0].Rows[i]["ft_bianhao"]);
            //  ftt.ft_beiyong = title;
            //  ftt.ft_teamStyle = title;
            //  ftt.ft_caipan = tttime;
            //  ftt.ft_didian = "0";
            //  ftt.ft_glod = 0;
            //  ftt.ft_hit = 0;
            //  ftt.ft_news = infomat;
            //  ftt.ft_otherNews = "0";
            //  ftt.ft_otherNews = jinqiu;
            ftt.ft_overTime = DateTime.Now;
            ftt.ft_result = result1;
            ftt.ft_state = state1;
            if (hr.Length > 3)
            {
                ftt.ft_team1Explain = getStringNew(hr[1].ToString());
            }
            if (hr.Length > 3)
            {
                ftt.ft_team2Explain = getStringNew(hr[2].ToString());
            }
            if (hr.Length > 3)
            {
                ftt.ft_state1 = getStringNew(hr[3].ToString());
            }
            ftt.ft_state2 = (hitball);
            //  ftt.ft_team1 = p_one;   
            //  ftt.ft_team2 = p_two;                
            //  ftt.ft_time = Convert.ToDateTime(Date);
            //  ftt.Identification = 1;
            //  ftt.isDone = 1;
            try
            {
                new BCW.BLL.tb_ZQLists().Update(ftt);
            }
            catch (WebException ex)
            {
                //  Response.Write(ex.ToString());
                //using (StreamReader sr = new StreamReader(ex.Response.GetResponseStream()))
                //{
                //    Response.Write(sr.ReadToEnd());
                //}
            }
            //catch ( ee)
            //{
            //     Response.Write(ee.ToString());
            //}
        }
        //   Response.Write(idd + "[" + ftt.ft_time + "]" + ftt.ft_teamStyle + ftt.ft_team1 + "--" + ftt.ft_team2 + "赛事重更成功！" + "当前" + ftt.ft_result + "状态" + ftt.ft_state + "<br/>");

    }

    //全部刷新
    private void FrashAll()
    {
        string jinqiu = "";
        string gunqiu = "";
        ////取进行中球赛更新
        string strWhere = "ft_state!='完' and ft_state!='推迟'  and ft_state!='待定' ORDER BY convert(datetime,ft_time,120) DESC,convert(datetime,ft_caipan,120) ASC";
        DataSet ds = new BCW.BLL.tb_ZQLists().GetList(" * ", strWhere);
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string[] pageValUrl = { "act", "ptype", "id", "backurl", "State" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        string timedate = "";
        string timehour = "";
        string alltime = "";
        string result1 = "";
        string state1 = "";
        DateTime dt;
        string url2 = "http://3g.8bo.com/3g/football/score/history.aspx?date=2016/06/28&st=allEvents&by=detail&eid=857322";//历史
        string htmlText = "";
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            Response.Write("<br/>" + "当前进行球赛" + ds.Tables[0].Rows.Count);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                timedate = ds.Tables[0].Rows[i]["ft_time"].ToString();
                timehour = ds.Tables[0].Rows[i]["ft_caipan"].ToString();
                alltime = timedate + " " + timehour;
                //  dt = Convert.ToDateTime(alltime);
                // if (dt.AddHours(3) < DateTime.Now)
                {
                    url2 = "http://3g.8bo.com/3g/football/score/history.aspx?date=" + Convert.ToDateTime(timedate).ToString("yyyy/MM/dd").ToString() + "&st=hasCompletedField&by=detail&eid=" + ds.Tables[0].Rows[i]["ft_bianhao"].ToString();
                    htmlText = GetHtmlSource(url2, Encoding.UTF8);
                    gunqiu = getPeilvForLianSai(htmlText);
                    //  Response.Write(htmlText);
                    state1 = getStateForLianSai(htmlText);
                    result1 = getResult(htmlText);
                    Regex regex = new Regex(@"<table.*?>[\s\S]*?<\/table>");
                    MatchCollection mc = regex.Matches(htmlText);
                    //获取集合类中自己需要的某个table
                    String newHtmlStr = "";
                    try
                    {
                        newHtmlStr = mc[0].ToString();
                        //  Response.Write(mc[0].ToString());
                    }
                    catch { }
                    //将数据返回
                    string[] hr = Regex.Split(newHtmlStr, (@"<hr size=""1"""));
                    int isNum = 1;//默认mc1捉到
                    string hitball = "";
                    try
                    {
                        if (mc[1].ToString().Length > 5)
                        {
                            hitball = mc[1].ToString();
                        }
                        else
                        { isNum = 2; }
                    }
                    catch
                    {
                        isNum = 2;//默认mc1捉不到,null
                    }
                    int idd = new BCW.BLL.tb_ZQLists().GetIdFromBianhao(Convert.ToInt32(ds.Tables[0].Rows[i]["ft_bianhao"]));
                    if (state1 != "" && result1 != "")
                    {
                        // Response.Write(FootbzlistHtml(htmlText));

                        BCW.Model.tb_ZQLists ftt = new BCW.BLL.tb_ZQLists().Gettb_ZQLists(idd);
                        //  ftt.ft_addTime = DateTime.Now;
                        // ftt.ft_bianhao = Convert.ToInt32(ds.Tables[0].Rows[i]["ft_bianhao"]);
                        //  ftt.ft_beiyong = title;
                        //  ftt.ft_teamStyle = title;
                        //  ftt.ft_caipan = tttime;
                        //  ftt.ft_didian = "0";
                        //  ftt.ft_glod = 0;
                        //  ftt.ft_hit = 0;
                        //  ftt.ft_news = infomat;
                        //  ftt.ft_otherNews = "0";
                        //  ftt.ft_otherNews = jinqiu;
                        ftt.ft_overTime = DateTime.Now;
                        ftt.ft_result = result1;
                        ftt.ft_state = state1;
                        if (hr.Length > 3)
                        {
                            ftt.ft_team1Explain = getStringNew(hr[1].ToString());
                        }
                        if (hr.Length > 3)
                        {
                            ftt.ft_team2Explain = getStringNew(hr[2].ToString());
                        }
                        if (hr.Length > 3)
                        {
                            ftt.ft_state1 = getStringNew(hr[3].ToString());
                        }
                        ftt.ft_state2 = (hitball);
                        //  ftt.ft_team1 = p_one;   
                        //  ftt.ft_team2 = p_two;                
                        //  ftt.ft_time = Convert.ToDateTime(Date);
                        //  ftt.Identification = 1;
                        //  ftt.isDone = 1;
                        try
                        {
                            new BCW.BLL.tb_ZQLists().Update(ftt);
                        }
                        catch (Exception ee)
                        {
                            // Response.Write(ee.ToString());
                        }
                        //   Response.Write(idd + "[" + ftt.ft_time + "]" + ftt.ft_teamStyle + ftt.ft_team1 + "--" + ftt.ft_team2 + "赛事重更成功！" + "当前" + ftt.ft_result + "状态" + ftt.ft_state + "<br/>");
                    }
                }
            }
        }
        else
        {
            //Response.Write("赛事重更失败！" + "<br/>");

        }
    }

    //onelist 一场比赛的具体数据
    private void GetAGameData()
    {
        int meid = new BCW.User.Users().GetUsId();
        //if (meid == 0)
        //    Utils.Login();
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(Out.waplink(Utils.getUrl("default.aspx"), "游戏-"));
        builder.Append(Out.waplink(Utils.getUrl("footballs.aspx"), "比分管理-"));
        builder.Append("足球即时比分");
        builder.Append(Out.Tab("</div>", "<br />"));
        int Id = Utils.ParseInt(Utils.GetRequest("Id", "all", 1, "", ""));
        string check = Utils.GetRequest("check", "all", 1, "", "no");
        string frash = Utils.GetRequest("frash", "all", 1, "", "no");
        if (!new BCW.BLL.tb_ZQLists().Exists(Id))
        {
            Utils.Success("不存在的记录！", "出错啦！不存在该球赛记录！1s后返回..", Utils.getUrl("footballs.aspx?act=balllist&amp;Id=" + Id + ""), "1");
        }
        if (frash == "yes")
        {
            Frash(Id);
            frash = "no";
        }
        else
        { frash = "yes"; }
        if (check == "yes")
        {
            Check(Id);
            check = "no";
            //builder.Append(check);
        }
        builder.Append(Out.Tab("<div>", ""));

        BCW.Model.tb_ZQLists model = new BCW.BLL.tb_ZQLists().Gettb_ZQLists(Id);
        //  Master.Title = model.ft_team1+"VS"+model.ft_team2;
        builder.Append("[阅读量<font color=\"blue\">" + model.ft_hit + "</font>]<br/>");
        builder.Append("[收藏量<font color=\"blue\">" + new BCW.BLL.tb_ZQCollection().CountUsIdAndFootId(Id) + "</font>]<br/>");
        builder.Append("[闲聊量<font color=\"blue\">" + new BCW.BLL.tb_ZQChact().GetCountForId(Id) + "</font>]<br/>");
        builder.Append("[当前ID<font color=\"blue\">" + model.Id + "</font>]<br/>");
        builder.Append("[最后更新" + DT.DateDiff2(DateTime.Now, Convert.ToDateTime(model.ft_overTime)) + "前]<br/>");
        // builder.AppendFormat("{0}前<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid={1}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}</a>{3}", DT.DateDiff2(DateTime.Now, Convert.ToDateTime(n.AddTime)), n.UserId, mename, Out.SysUBB(n.Remarks));
        builder.Append("<a href=\"" + Utils.getUrl("footballs.aspx?act=style&amp;Style=" + model.ft_teamStyle + "") + "\">" + model.ft_teamStyle + " </a> " + "@ " + Convert.ToDateTime(model.ft_time).ToString("MM月dd日") + "&nbsp; " + model.ft_caipan);
        builder.Append(Out.waplink(Utils.getUrl("footballs.aspx?act=onelist&amp;Id=" + Id + "&amp;frash=" + frash + "&amp;"), "[刷]") + "");
        builder.Append("<br />主客队:");
        builder.Append("<a href=\"" + Utils.getUrl("footballs.aspx?act=team&amp;team=" + GetAGameText(model.ft_team1) + "") + "\">" + model.ft_team1 + " </a>");
        builder.Append("-");
        builder.Append("<a href =\"" + Utils.getUrl("footballs.aspx?act=team&amp;team=" + GetAGameText(model.ft_team2) + "") + "\">" + model.ft_team2 + " </a>");
        builder.Append("<br />比赛状态:" + Convertp_once(model.ft_state) + "");

        if (model.ft_result != "")
            builder.Append("<br/>即时比分:" + "<font color=\"red\">" + model.ft_result + "</font>");

        builder.Append("<a href=\"" + Utils.getUrl("footballs.aspx?act=update&amp;Id=" + (model.Id) + "") + "\">" + "[改]" + " </a>");
        builder.Append("<br />8bo:" + model.ft_bianhao + "<br/>");
        builder.Append("〓管理〓<br/>");
        builder.Append(" " + Out.waplink(Utils.getUrl("footballs.aspx?act=update&amp;Id=" + model.Id + "&amp;"), "修改"));
        builder.Append(" " + Out.waplink(Utils.getUrl("footballs.aspx?Id=" + model.Id + "&amp;act=del"), "删除"));
        builder.Append("<a href=\"" + Utils.getUrl("footballs.aspx?act=onelist&amp;Id=" + Id + "&amp;check=yes&amp;") + "\">" + " 检查" + " </a>");
        builder.Append(" " + Out.waplink(Utils.getUrl("footballs.aspx?act=update&amp;Id=" + model.Id + ""), "关联"));
        builder.Append(Out.Tab("</div>", ""));
        try
        {
            if (model.ft_otherNews.Length > 5)
            {
                // builder.Append("<br/>");
                builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
                builder.Append("赔率数据:");
                builder.Append(Out.Tab("</div>", ""));
                //  builder.Append("<div>" + model.ft_otherNews+ "</div>");
                //  builder.Append("<div>" + model.ft_team1Explain + "</div>");
                builder.Append("<div>");
                string[] ss = model.ft_team1Explain.Split('↑');
                for (int i = 0; i < ss.Length; i++)
                {
                    builder.Append(ss[i] + "<br/>");
                }
                // <b>初盘</b>↑让球0.91 <i>一/球半</i> 0.85↑大小0.85 <i>3</i> 0.91↑标准1.39 4.50 5.60</table>
                // builder.Append("<div>" + model.ft_team2Explain + "</div>");
                string[] ss1 = model.ft_team2Explain.Split('↑');
                for (int i = 0; i < ss.Length; i++)
                {
                    builder.Append(ss1[i] + "<br/>");
                }
                //  builder.Append("<div>" + model.ft_state1 + "</div>");
                string[] ss2 = model.ft_state1.Split('↑');
                for (int i = 0; i < ss.Length; i++)
                {
                    builder.Append(ss2[i] + "<br/>");
                }
                builder.Append("</div>");
            }
            if (model.ft_state2.Length > 5)
            {
                builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
                builder.Append("进球时刻:");
                builder.Append(Out.Tab("</div>", ""));
                builder.Append("<div>" + model.ft_state2 + "</div>");
            }
            //else { builder.Append("<br/>" + "进球时刻:" + "" + "暂无收录." + ""); }
            if (model.ft_team1Explain.Length > 3)
            {
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("球场数据:");
                builder.Append(Out.Tab("</div>", ""));
                builder.Append("<div>" + model.ft_news + "</div>");
            }
        }
        catch
        {
            if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))//u=" + Utils.getstrU() + "&amp;
            {
                Utils.Success("温馨提示", "使用彩版，页面更直观，更快捷！正在切换进入...", "footballs.aspx?act=onelist&amp;ve=2a&amp;u=" + Utils.getstrU() + "", "1");
            }
        }
        //else
        //{
        //    //builder.Append("<div>" + "球场数据:" + "" + "暂无收录." + "</div>"); 
        //}
        if (new BCW.BLL.tb_ZQChact().GetCountForId(Id) > 0)
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("球赛讨论:");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        string swhere = "toFootId=" + Id + "order by  AddTime DESC";
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
                    builder.Append("<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + Convert.ToInt32(ds.Tables[0].Rows[koo + i]["UsId"]) + "") + "\">" + new BCW.BLL.User().GetUsName(Convert.ToInt32(ds.Tables[0].Rows[koo + i]["UsId"])) + "</a>说: ");
                    // builder.Append(new BCW.BLL.User().GetUsName(Convert.ToInt32(ds.Tables[0].Rows[koo + i]["UsId"])) + ":");
                }
                builder.Append((ds.Tables[0].Rows[koo + i]["TextContent"]).ToString().Trim());
                builder.Append("&nbsp; [" + Convert.ToDateTime(ds.Tables[0].Rows[koo + i]["Addtime"]).ToString("MM-dd HH:mm") + "]");
                builder.Append(Out.Tab("</div>", ""));
                //  builder.Append("<br/>");
                k++;
            }

            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {

        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("footballs.aspx") + "\">返回上级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));

        //builder.Append(Out.Tab("<div>", "<br/>"));
        //builder.Append("<a href=\"" + Utils.getUrl("footballs.aspx?act=1&amp;ptype=4&amp;fly=" + 3 + "") + "\">" + "返回上级" + "</a>");
        //builder.Append(Out.Tab("</div>", ""));
        //builder.Append(Out.Tab("<div>", "<br />"));
        //builder.Append("比分数据采集于互联网，仅用于球迷参考或虚拟游戏，如侵犯您的利益请联系本站工作人员.");
        //builder.Append(Out.Tab("</div>", ""));
        // 闲聊显示
        //builder.Append(Out.Tab("<div>", ""));
        //builder.Append(Out.SysUBB(BCW.User.Users.ShowSpeak(17, "footballs.aspx?act=onelist&amp;Id="+Id+"", 5, 0)));
        //builder.Append(Out.Tab("</div>", ""));
    }

    //style 一类比赛的具体数据
    private void GetAStyleData()
    {
        string Style = Utils.GetRequest("Style", "all", 1, "", "");
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(Out.waplink(Utils.getUrl("default.aspx"), "游戏-"));
        builder.Append(Out.waplink(Utils.getUrl("footballs.aspx"), "比分管理-"));
        builder.Append(Style);
        builder.Append(Out.Tab("</div>", ""));
        string datetimeout = "";
        string strWhere = "ft_teamStyle like '" + Style + "' ORDER BY convert(datetime,ft_time,120) ASC,convert(datetime,ft_caipan,120) ASC";
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
                    builder.Append("<a  href=\"" + Utils.getUrl("footballs.aspx?act=onelist&amp;Id=" + ds.Tables[0].Rows[koo + i]["Id"] + "&amp;") + "\">" + ds.Tables[0].Rows[koo + i]["ft_caipan"] + "[" + GetRed(ds.Tables[0].Rows[koo + i]["ft_teamStyle"].ToString(), Style) + "]");
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
                    // builder.Append(Out.waplink(Utils.getUrl("footballs.aspx?act=onelist&amp;Id=" + ds.Tables[0].Rows[koo + i]["Id"] + "&amp;"), "[" + textout + "]") + "");
                    k++;
                    datetimeout = Convert.ToDateTime(ds.Tables[0].Rows[koo + i]["ft_time"]).ToString("MM月dd日");
                    builder.Append(Out.Tab("</div>", ""));
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
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("footballs.aspx") + "\">返回上级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //day 一天比赛的具体数据
    private void GetADayData()
    {
        string date = Utils.GetRequest("date", "all", 1, @"^[^\^]{0,2000}$", "0");
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(Out.waplink(Utils.getUrl("default.aspx"), "游戏-"));
        builder.Append(Out.waplink(Utils.getUrl("footballs.aspx"), "比分管理-"));
        builder.Append(date);
        builder.Append(Out.Tab("</div>", ""));
        string datetimeout = "";
        try
        {
            string strWhere = "ft_time ='" + Convert.ToDateTime(date) + "' ORDER BY convert(datetime,ft_time,120) DESC,convert(datetime,ft_caipan,120) DESC";
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
                        builder.Append("<a  href=\"" + Utils.getUrl("footballs.aspx?act=onelist&amp;Id=" + ds.Tables[0].Rows[koo + i]["Id"] + "&amp;") + "\">" + ds.Tables[0].Rows[koo + i]["ft_caipan"] + "[" + ds.Tables[0].Rows[koo + i]["ft_teamStyle"] + "]");
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
                        // builder.Append(Out.waplink(Utils.getUrl("footballs.aspx?act=onelist&amp;Id=" + ds.Tables[0].Rows[koo + i]["Id"] + "&amp;"), "[" + textout + "]") + "");
                        k++;
                        datetimeout = Convert.ToDateTime(ds.Tables[0].Rows[koo + i]["ft_time"]).ToString("MM月dd日");
                        builder.Append(Out.Tab("</div>", ""));
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
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("footballs.aspx") + "\">返回上级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //字体变红
    private string GetRed(string str, string s)
    {
        str = Regex.Replace(str, @"" + s + "", "<font color=\"red\">" + s + "</font>");
        return str;
    }

    //team 一支球队的具体数据
    private void GetATeamData()
    {
        string team = Utils.GetRequest("team", "all", 1, @"^[^\^]{0,2000}$", "0");
        //string strpattern1 = @"[\u4e00-\u9fa5]+";
        //Match mtitle12 = Regex.Match(team, strpattern1, RegexOptions.IgnoreCase);
        //team = mtitle12.Groups[0].Value.Trim();
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(Out.waplink(Utils.getUrl("default.aspx"), "游戏-"));
        builder.Append(Out.waplink(Utils.getUrl("footballs.aspx"), "比分管理-"));
        builder.Append(team);
        builder.Append(Out.Tab("</div>", ""));
        string datetimeout = "";
        try
        {
            string strWhere = "ft_team1 like '%" + team + "%' or ft_team2 like '%" + team + "%' ORDER BY convert(datetime,ft_time,120) DESC,convert(datetime,ft_caipan,120) DESC";
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
                        builder.Append("<a  href=\"" + Utils.getUrl("footballs.aspx?act=onelist&amp;Id=" + ds.Tables[0].Rows[koo + i]["Id"] + "&amp;") + "\">" + ds.Tables[0].Rows[koo + i]["ft_caipan"] + "[" + ds.Tables[0].Rows[koo + i]["ft_teamStyle"] + "]");
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
                        // builder.Append(Out.waplink(Utils.getUrl("footballs.aspx?act=onelist&amp;Id=" + ds.Tables[0].Rows[koo + i]["Id"] + "&amp;"), "[" + textout + "]") + "");
                        k++;
                        datetimeout = Convert.ToDateTime(ds.Tables[0].Rows[koo + i]["ft_time"]).ToString("MM月dd日");
                        builder.Append(Out.Tab("</div>", ""));
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
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("footballs.aspx") + "\">返回上级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //update 一场球赛的数据
    private void UpdateList()
    {
        int Id = Utils.ParseInt(Utils.GetRequest("Id", "all", 1, "", ""));
        Master.Title = "修改一场球赛的数据";
        builder.Append(Out.Tab("", ""));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        //ub xml = new ub();
        //string xmlPath = "/Controls/myyg.xml";
        //Application.Remove(xmlPath);//清缓存
        //xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            BCW.Model.tb_ZQLists model = new BCW.BLL.tb_ZQLists().Gettb_ZQLists(Id);
            string ft_teamStyle = Utils.GetRequest("ft_teamStyle", "post", 2, @"^[^\^]{1,20}$", "球队类型限1-20字内");
            string ft_time = Utils.GetRequest("ft_time", "post", 2, @"^[^\^]{1,2000}$", "比赛日期输入错误");
            string ft_caipan = Utils.GetRequest("ft_caipan", "post", 2, @"^[^\^]{1,200}$", "比赛时间输入错误");
            string ft_team1 = Utils.GetRequest("ft_team1", "post", 2, @"^[^\^]{1,2000}$", "主队名称输入错误");
            string ft_team2 = Utils.GetRequest("ft_team2", "post", 2, @"^[^\^]{1,2000}$", "客队名称输入错误");
            string ft_state = Utils.GetRequest("ft_state", "post", 2, @"^[^\^]{1,200}$", "比赛状态填写错误");
            string ft_result = Utils.GetRequest("ft_result", "all", 2, @"^[^\^]{1,200}$", "即时比分填写错误");
            string ft_team1Explain = Utils.GetRequest("ft_team1Explain", "post", 2, @"^[^\^]{1,2000}$", "滚球赔率填写错误");
            string ft_team2Explain = Utils.GetRequest("ft_team2Explain", "post", 2, @"^[^\^]{1,2000}$", "即时赔率填写错误");
            string ft_state1 = Utils.GetRequest("ft_state1", "post", 2, @"^[^\^]{1,2000}$", "初盘赔率填写错误");
            string ft_news = Utils.GetRequest("ft_news", "post", 1, "", "");
            string Identification = Utils.GetRequest("indet", "all", 2, @"^[0-1]$", "1");
            //    Utils.Error(""+ Identification + "","");
            DateTime dt = DateTime.Now;
            try
            {
                dt = Convert.ToDateTime(ft_time);
            }
            catch
            { Utils.Error("输入的时间格式有误！", ""); }
            model.ft_teamStyle = ft_teamStyle;
            model.ft_time = dt;
            model.ft_caipan = ft_caipan;
            model.ft_team1 = ft_team1;
            model.ft_team2 = ft_team2;
            model.ft_state = ft_state;
            model.ft_result = ft_result;
            model.ft_team1Explain = ft_team1Explain;
            model.ft_team2Explain = ft_team2Explain;
            model.ft_state1 = ft_state1;
            model.ft_news = ft_news;
            model.Identification = Convert.ToInt32(Identification);
            new BCW.BLL.tb_ZQLists().Update(model);
            //  System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("修改球队比赛数据", ft_team1 + "VS" + ft_team2 + "修改成功，3s后返回..", Utils.getUrl("footballs.aspx?act=update&amp;Id=" + Id + "&amp;backurl=" + Utils.getPage(1) + ""), "3");
        }
        else
        {
            BCW.Model.tb_ZQLists model = new BCW.BLL.tb_ZQLists().Gettb_ZQLists(Id);
            builder.Append(Out.Div("title", "" + GetAGameText(model.ft_teamStyle) + "VS" + GetAGameText(model.ft_team2) + "-比赛数据"));
            string strText = "球队类型:/,比赛日期:/,比赛时间:/,主队:/,客队:/,比赛状态:/,即时比分:/,滚球:/,即时:/,初盘:/,球场数据:/,显示与隐藏";
            string strName = "ft_teamStyle,ft_time,ft_caipan,ft_team1,ft_team2,ft_state,ft_result,ft_team1Explain,ft_team2Explain,ft_state1,ft_news,indet";
            string strType = "text,text,text,text,text,text,text,textarea,textarea,textarea,textarea,select";
            string strValu = model.ft_teamStyle + "'" + Convert.ToDateTime(model.ft_time).ToString("yyyy-MM-dd") + "'" + model.ft_caipan + "'" + model.ft_team1 + "'" + model.ft_team2 + "'" + deleteStr(model.ft_state) + "'" + model.ft_result.ToString() + "'" + model.ft_team1Explain + "'" + model.ft_team2Explain + "'" + model.ft_state1 + "'" + model.ft_news + "'" + +model.Identification + "";
            string strEmpt = "true,true,true,true,true,true,true,true,true,true,true,0|隐藏|1|显示";
            string strIdea = "/";
            string strOthe = "确定修改|reset,footballs.aspx?act=update&amp;Id=" + Id + "&amp;,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            //  builder.Append("温馨提示:"+ model.Identification + "<br />");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("footballs.aspx?act=onelist&amp;Id=" + Id + "") + "\">返回上级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }

    //del 删除一场球赛
    private void DeleBallId()
    {
        int Id = Utils.ParseInt(Utils.GetRequest("Id", "all", 1, "", ""));
        Master.Title = "删除一场球赛的数据";
        builder.Append(Out.Tab("", ""));
        string ac = Utils.GetRequest("ac", "post", 1, "", "");
        if (ac == "yes")
        {
            //   Utils.Error("" + ac + "", "");
            new BCW.BLL.tb_ZQLists().Delete(Id);
            Utils.Success("删除球队比赛数据", "删除成功，3s后返回..", Utils.getUrl("footballs.aspx?act=balllist&amp;Id=" + Id + "&amp;"), "3");
        }
        else
        {
            BCW.Model.tb_ZQLists model = new BCW.BLL.tb_ZQLists().Gettb_ZQLists(Id);
            builder.Append(Out.Div("title", "" + GetAGameText(model.ft_team1) + "VS" + GetAGameText(model.ft_team2) + "-比赛数据"));
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("确定删除吗？" + "<br/>");
            builder.Append("<a href=\"" + Utils.getUrl("footballs.aspx?act=del&amp;Id=" + Id + "&amp;ac=yes") + "\">是</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("footballs.aspx?act=onelist&amp;Id=" + Id + "&amp;ac=no") + "\">再看看吧</a><br />");
            builder.Append(Out.Tab("</div>", ""));
            //builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("footballs.aspx?act=onelist&amp;Id=" + Id + "") + "\">返回上级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }

    #endregion

    #region 捉取管理
    //去除’号显示
    private string deleteStr(string p_once)
    {
        p_once = Regex.Replace(p_once, @"'", "");
        return p_once;

    }

    //取文字显示
    private string GetAGameText2(string text)
    {
        string strpattern1 = @"[\u4e00-\u9fa5]+";
        Match mtitle12 = Regex.Match(text, strpattern1, RegexOptions.IgnoreCase);
        text = mtitle12.Groups[0].Value.Trim();
        return text;
    }
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
                        once = (min - 0) + "'";
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

    //取文字显示
    private string GetAGameText(string text)
    {
        string strpattern1 = @"[\u4e00-\u9fa5]+";
        Match mtitle12 = Regex.Match(text, strpattern1, RegexOptions.IgnoreCase);
        text = mtitle12.Groups[0].Value.Trim();
        return text;
    }

    //这里即时完场比分
    private string getResult(string _html)
    {
        string Result = "";
        string strpattern = @"<b class=""score"">((\d){1,2}-(\d){1,2})</b>";
        Match mtitle = Regex.Match(_html, strpattern, RegexOptions.IgnoreCase);
        if (mtitle.Success)
        {
            //取即时比分
            Result = mtitle.Groups[1].Value;
            int p_id = 811960183;
            string strState = getStateForLianSai(_html);
            if (Result.Contains("-"))
            {
                try
                {
                    string[] p_result = Result.Split('-');
                    if (strState == "完")
                    {
                        int p_result_one = Convert.ToInt32(p_result[0]);
                        int p_result_two = Convert.ToInt32(p_result[1]);
                        //  new TPR2.BLL.guess.BaList().UpdateBoResult(p_id, p_result_one, p_result_two);

                    }
                    else
                    {
                        int p_result_temp1 = Convert.ToInt32(p_result[0]);
                        int p_result_temp2 = Convert.ToInt32(p_result[1]);
                        TPR2.Model.guess.BaList bf = new TPR2.BLL.guess.BaList().GetTemp(p_id);
                        if (bf != null)
                        {
                            if (bf.p_result_temp1 != p_result_temp1 || bf.p_result_temp2 != p_result_temp2) { }
                            //   new TPR2.BLL.guess.BaList().UpdateBoResult2(p_id, p_result_temp1, p_result_temp2);

                        }
                        //更新半场即时比分
                        bf = new TPR2.BLL.guess.BaList().GetTemp(p_id, 9);
                        if (bf != null)
                        {
                            if (bf.p_result_temp1 != p_result_temp1 || bf.p_result_temp2 != p_result_temp2)
                            {
                                //    new TPR2.BLL.guess.BaList().UpdateBoResultHalf(p_id, p_result_temp1, p_result_temp2);

                            }

                        }
                    }
                }
                catch { }
            }
        }
        return Result;
    }

    /// <summary>
    /// 获取网页HTML源码
    /// </summary>
    /// <param name="url">链接 eg:http://www.baidu.com/ </param>
    /// <param name="charset">编码 eg:Encoding.UTF8</param>
    /// <returns>HTML源码</returns>
    public static string GetHtmlSource(string url, Encoding charset)
    {

        string _html = string.Empty;
        try
        {
            HttpWebRequest _request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse _response = (HttpWebResponse)_request.GetResponse();
            using (Stream _stream = _response.GetResponseStream())
            {
                using (StreamReader _reader = new StreamReader(_stream, charset))
                {
                    _html = _reader.ReadToEnd();
                }
            }
        }
        catch (WebException ex)
        {

            //using (StreamReader sr = new StreamReader(ex.Response.GetResponseStream()))
            //{
            //    _html = sr.ReadToEnd();
            //}
            _html = ex.Message;
        }
        catch (Exception ex)
        {
            _html = ex.Message;
        }
        return _html;
    }

    public string getStringNew(string message)
    {
        //过滤\n 转换成空
        String result = message.Replace("\n", "");
        //过滤\r 转换成空
        result = result.Replace("\r", "");
        //过滤\t 转换成空
        result = result.Replace("\t", "");
        //过滤\ 转换成空
        result = result.Replace("\\", "");
        //获取html中的body标签
        //   String result = Regex.Match(newString, @"<body.*>.*</body>").ToString();
        //过滤注释
        result = Regex.Replace(result, @"<!--(?s).*?-->", "", RegexOptions.IgnoreCase);
        //过滤nbsp标签
        result = Regex.Replace(result, @"&nbsp;", "", RegexOptions.IgnoreCase);
        //过滤nbsp标签   /></td></tr><tr><td rowspan="3" align="center">
        //<b>滾球</b></td><td>↑让球</td><td>0.77 <i>平手</i> 1.12
        //</td></tr><tr><td>↑大小</td><td>0.95 <i>1.5/2</i> 0.91
        //</td></tr><tr><td>↑标准</td><td>1.21 4.75 15.00</td></tr><tr>
        //  />↑让球0.75 <i>平手</i> 1.14↑大小0.80 <i>2.5</i> 1.06↑标准1.02 11.50 28.00
        //<table cellspacing="3" cellpadding="0" border="0" class="detail">
        //<td align="right" class="type">进球<td align="right" class="name">Valdez<td align="center">
        //[18']<td align="right" class="type">进球<td align="right" class="name">Sambueza<td align="center">[67']
        //</table>
        result = Regex.Replace(result, @"</td>", "", RegexOptions.IgnoreCase);
        result = Regex.Replace(result, @"//", "", RegexOptions.IgnoreCase);
        result = Regex.Replace(result, @"</tr>;", "", RegexOptions.IgnoreCase);
        result = Regex.Replace(result, @"<td>", "", RegexOptions.IgnoreCase);
        result = Regex.Replace(result, @"<tr>", "", RegexOptions.IgnoreCase);
        result = Regex.Replace(result, @"</tr>", "", RegexOptions.IgnoreCase);
        result = Regex.Replace(result, @"/>", "", RegexOptions.IgnoreCase);
        result = Regex.Replace(result, @"<td rowspan=""3"" align=""center"">", "", RegexOptions.IgnoreCase);
        result = Regex.Replace(result, @"<td colspan=""3"">", "", RegexOptions.IgnoreCase);
        result = Regex.Replace(result, @"</table>", "", RegexOptions.IgnoreCase);
        result = Regex.Replace(result, @"</spa>", "", RegexOptions.IgnoreCase);
        // result = Regex.Replace(result, @"<b>滾球</b>", "", RegexOptions.IgnoreCase);

        return result;
    }
    //取联赛名称
    private string getNameForLianSai(string _html)
    {
        string title = "";
        string strpattern = @"([\s\S]+?)</td><td class=""W2"">";
        Match mtitle = Regex.Match(_html, strpattern, RegexOptions.IgnoreCase);
        if (mtitle.Success)
        {
            title = mtitle.Groups[1].Value;
            title = Regex.Replace(title, @"<.+?>", "");
        }
        return title;
    }
    //取比赛信息
    private string getInformationForLianSai(string _html)
    {
        string strState = "";
        string strpattern = @"<td colspan=""2"" class=""info"">([\s\S]+?)</td>";
        Match mtitle = Regex.Match(_html, strpattern, RegexOptions.IgnoreCase);
        if (mtitle.Success)
        {
            strState = mtitle.Groups[1].Value.Trim();
        }
        return strState;
    }
    //取比赛状态
    private string getStateForLianSai(string _html)
    {
        string strState = "";
        string strpattern = @"<td align=""center"">([\s\S]{1,10})</td>";//未、点(这个是代表点球)、完、待定、腰斩、推迟（无这些选项则取的比赛进行分钟数）
        Match mtitle = Regex.Match(_html, strpattern, RegexOptions.IgnoreCase);
        if (mtitle.Success)
        {
            strState = mtitle.Groups[1].Value.Trim();
        }
        return strState;
    }
    //取比赛日期
    private string getDateForLianSai(string _html)
    {
        string Date = "";
        string strpattern = @"<td class=""W2"">((\d){2}-(\d){2})</td><td class=""teamname"">";
        Match mtitle = Regex.Match(_html, strpattern, RegexOptions.IgnoreCase);
        if (mtitle.Success)
        {
            Date = mtitle.Groups[1].Value;
            //HttpContext.Current.Response.Write(Date + "<br />");
        }
        return Date;
    }
    //取主队名称
    private string getTeam1ForLianSai(string _html)
    {
        string p_one = "";
        string strpattern = @"<td class=""teamname"">([\s\S]+)<td class=""teamname"">";
        Match mtitle = Regex.Match(_html, strpattern, RegexOptions.IgnoreCase);
        if (mtitle.Success)
        {
            p_one = mtitle.Groups[1].Value.Trim();
            p_one = Regex.Replace(p_one, @"<small>\[[\s\S]+\]</small>", "");
            p_one = Regex.Replace(p_one, @"<span class=""rc"">[\w\d]+</span>", "");
            p_one = Regex.Replace(p_one, @"<.+?>", "");
            p_one = Regex.Replace(p_one, @"析", "");
            p_one = Regex.Replace(p_one, @"^[1-9]\d*$", "");
            p_one = Regex.Replace(p_one, @"\d", "");
            p_one = Regex.Replace(p_one, @"'", "");
            p_one = Regex.Replace(p_one, @":", "");
            p_one = Regex.Replace(p_one, @"完", "");
        }
        return p_one;
    }
    //取客队名称
    private string getTeam2ForLianSai(string _html)
    {
        //取客队名称
        string p_two = "";
        string strpattern = @"<td>(\d){2}:(\d){2}</td><td class=""teamname"">([\s\S]+)</td></tr>";
        Match mtitle = Regex.Match(_html, strpattern, RegexOptions.IgnoreCase);
        if (mtitle.Success)
        {
            p_two = mtitle.Groups[0].Value.Trim();
            string[] p_twoTemp = Regex.Split(p_two, @"<tr class=""alternation"">");

            p_two = Regex.Replace(p_twoTemp[0], @"<small>\[[\s\S]+\]</small>", "");
            p_two = Regex.Replace(p_two, @"<span class=""rc"">[\w\d]+</span>", "");
            if (p_two.Contains("↑"))
            {
                p_two = Regex.Split(p_two, "↑")[0];
            }
            p_two = Regex.Replace(p_two, @"<td>(\d){2}:(\d){2}</td>", "");
            p_two = Regex.Replace(p_two, @"<td class=""teamname"">", "");
            p_two = Regex.Replace(p_two, @"<td>", "");
            p_two = Regex.Replace(p_two, @"<tr>", "");
            p_two = Regex.Replace(p_two, @"</td>", "");
            p_two = Regex.Replace(p_two, @"</tr>", "");
            p_two = Regex.Replace(p_two, @"<b class=""score"">\[[\s\S]+\]</b>", "");
            p_two = Regex.Replace(p_two, @"<td", "");
            p_two = Regex.Replace(p_two, @"colspan=""(\d){2}"" class=", "");
            p_two = Regex.Replace(p_two, @"<td colspan = ""(\d)"" class=""info"">", "");
            //  p_two = Regex.Replace(p_two, @"/^[a-z\d] +$/", "");
            string strpattern1 = @"[\u4e00-\u9fa5]+";
            Match mtitle12 = Regex.Match(p_two, strpattern1, RegexOptions.IgnoreCase);
            p_two = mtitle12.Groups[0].Value.Trim();
            //  string strText = System.Text.RegularExpressions.Regex.Replace(p_two, "<[^>]+>", "");
            //  p_two = System.Text.RegularExpressions.Regex.Replace(strText, "&[^;]+;", "");
            // p_two = Regex.Replace(p_two, @"<b class=""score"">\[[\s\S]+\]</b>", "");
            // 平顺 < b class="score">0-0</b>
        }
        return p_two;
    }
    //取赔率
    private string getPeilvForLianSai(string _html)
    {
        //取赔率
        //滾球0.87 <i>平/半</i> 1.01<br/><tr class="alternation">↑大小1.02 <i>2.5/3</i> 0.84<br/><tr class="alternation">↑标准4.75 3.55 1.70<br/><tr class="alternation"></td>
        //滾球1.01 <i>受平手</i> 0.81<br/>↑大小0.90 <i>1</i> 0.90<br/>↑标准3.30 2.12 2.88<br/>
        //↑让球0.80 <i>受半球</i> 0.96<br/>↑大小0.87 <i>3.5</i> 0.89<br/>↑标准2.65 3.80 1.96<br/>
        string strState = "";
        string strpattern = @"<td>↑([\s\S]+?)<td colspan=""2"" class=""info"">";
        Match mtitle = Regex.Match(_html, strpattern, RegexOptions.IgnoreCase);
        if (mtitle.Success)
        {
            strState = mtitle.Groups[1].Value.Trim();
            strState = Regex.Replace(strState, @"</td><td>", "");
            strState = Regex.Replace(strState, @"</td></tr>", "<br/>");
            strState = Regex.Replace(strState, @"<td class=""alternation"">", "");
            strState = Regex.Replace(strState, @"<tr class=""alternation"">", "");
            strState = Regex.Replace(strState, @"<td>", "");
            strState = Regex.Replace(strState, @"<tr>", "");
            strState = Regex.Replace(strState, @"</tr>", "");
            strState = Regex.Replace(strState, @"</td>", "");
        }
        return ("↑" + strState);
    }

    #endregion

    #region 重置
    //重置所有记录（3个表）
    private void ResetPage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId != 1 && ManageId != 9)
        {
            Utils.Error("权限不足", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "")
        {
            Master.Title = "重置游戏";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定重置即时比分戏吗，重置后，所有记录将会被删除");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("footballs.aspx?info=ok&amp;act=reset") + "\">确定重置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("footballs.aspx") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            new BCW.Data.SqlUp().ClearTable("tb_BasketBallCollect");
            new BCW.Data.SqlUp().ClearTable("tb_BasketBallList");
            new BCW.Data.SqlUp().ClearTable("tb_BasketBallWord");
            new BCW.Data.SqlUp().ClearTable("tb_ZQLists");
            new BCW.Data.SqlUp().ClearTable("tb_ZQChact");
            new BCW.Data.SqlUp().ClearTable("tb_ZQColloction");
            Utils.Success("重置游戏", "重置游戏成功..", Utils.getUrl("footballs.aspx"), "1");
        }
    }

    #endregion





}