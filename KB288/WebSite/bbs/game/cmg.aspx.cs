using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text.RegularExpressions;
using BCW.Common;

/// <summary>
/// 姚志光 20160621 活跃抽奖控制入口
/// 邵广林 20160730 增加兑奖防刷
/// </summary>

public partial class bbs_game_cmg : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string strnum = string.Empty;
    public static string xmlPath = "/Controls/BYDR.xml";
    protected int buyuti = Convert.ToInt32(ub.GetSub("buyuti", xmlPath));
    protected int buyuti1 = Convert.ToInt32(ub.GetSub("buyuti1", xmlPath));
    protected int Ti = Convert.ToInt32(ub.GetSub("bydrbuyuTi", xmlPath));
    protected string bydrBotTime = Convert.ToString(ub.GetSub("bydrBotTime", xmlPath));
    protected double buyuprofit1 = Convert.ToDouble(ub.GetSub("bydrbuyuprofit", xmlPath));
    protected string bydrname = Convert.ToString(ub.GetSub("bydrName", xmlPath));//游戏名字
    protected static long T = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        //维护提示
        if (ub.GetSub("bydrStatus", xmlPath) == "1")
        {
            Utils.Safe("此游戏");
        }

        int meid = new BCW.User.Users().GetUsId();

        int test = Convert.ToInt32(ub.GetSub("bydrtest", "/Controls/BYDR.xml"));

        string act = Utils.GetRequest("act", "all", 1, "", "");

        //if (meid == 0)
        //    Utils.Login();
        switch (act)
        {
            case "buyuRule":
                buyuRulePage();   //玩法说明
                break;
            case "Sign":
                SignPage();        //签到
                break;
            case "addT":
                addTPage();       //兑换体力
                break;
            case "Record":
                RecordPage();     //我的记录
                break;
            case "addzanzhu":
                addzanzhuPage();   //捐款
                break;
            case "resetzz":
                resetzzPage();//捐款成功页面
                break;
            case "resetjudge":
                resetjudgePage();   //确认捐款
                break;
            case "recordj":
                RecordjPage();    //奖池记录
                break;
            case "addJ":
                addJPage();       //兑奖
                break;
            case "listadd":
                listaddPage();     //本页兑换
                break;
            case "alllistadd":
                alllistaddPage();  //单个兑换
                break;
            case "Top":
                TopPage();        //排行榜
                break;
            case "Topji":
                TopjiPage();      //等级榜
                break;
            case "reset":
                restPage();
                break;
            case "dongtai":
                dongtaiPage();   //捕鱼动态
                break;
            case "list":
                listPage();        //捕鱼详情表
                break;
            case "buyu":
                buyuPage();        //捕鱼开始
                break;
            case "Buyujudge":
                buyujudgePage();    //进入捕鱼场景时的提醒
                break;
            case "Wait":
                waitPage();         //未捕完提示      
                break;
            case "fangshua":
                fangshua();
                break;
            case "by":
                byPage();
                break;
            default:
                ReloadPage();       //游戏首页
                break;
        }
    }

    //游戏首页
    private void ReloadPage()
    {
        Master.Title = bydrname;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx") + "\">捕鱼</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        #region 一些游戏列表前的处理

        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[0-5]$", "0"));
        int maxID = new BCW.bydr.BLL.Cmg_Top().GetMaxId();

        int meid = new BCW.User.Users().GetUsId();
        //if (meid == 0)
        //    Utils.Login();//首页可看
        if (meid != 0)
        {
            //tb_Cmg_Top 表是否存在用户ID
            bool meid6 = new BCW.bydr.BLL.Cmg_Top().ExistsusID(meid);
            //如不存在,则增加一条数据
            if (meid6 == false)
                addbuyub();
            BCW.bydr.Model.CmgToplist model2 = new BCW.bydr.BLL.CmgToplist().GetCmgToplistusID(meid);
        }
        //提示公告UBB
        string Notes = ub.GetSub("bydrNotes", xmlPath);
        if (Notes != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.SysUBB(Notes) + "");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        //图片gif
        string Logo = ub.GetSub("bydrLogo", xmlPath);
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<img src=\"" + Logo + "bydr.gif" + "\" alt=\"load\"/>");
        builder.Append(Out.Tab("</div>", "<br />"));
        //奖池统计
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("奖池：" + "<a href=\"" + Utils.getUrl("cmg.aspx?act=recordj") + "\">" + (new BCW.bydr.BLL.CmgToplist().GettoplistYcolletGoldsum() - new BCW.bydr.BLL.Cmg_Top().GetCmg_AllcolletGold2()) + "</a>" + " " + ub.Get("SiteBz") + " " + "<a href=\"" + "  " + Utils.getUrl("cmg.aspx?act=addzanzhu") + "\">捐款</a>" + "<br />");
        if (meid != 0)
        {
            builder.Append("<a href=\"" + Utils.getUrl("../finance.aspx") + "\">我的财产</a>." + Utils.ConvertGold(new BCW.BLL.User().GetGold(meid)) + ub.Get("SiteBz") + "<br />");
        }
        //体力
        if (meid == 0)
        {
            builder.Append("体力：" + 0 + "<a href=\"" + Utils.getUrl("cmg.aspx?act=addT") + "\">补充</a>." + "<a href=\"" + Utils.getUrl("cmg.aspx?act=Sign") + "\">签到</a>." + "<a href=\"" + Utils.getUrl("cmg.aspx?act=ReloadPage") + "\">刷新</a><br />");
        }
        else
        {
            builder.Append("体力：" + new BCW.bydr.BLL.CmgToplist().Getvit(meid) + "<a href=\"" + Utils.getUrl("cmg.aspx?act=addT") + "\">补充</a>." + "<a href=\"" + Utils.getUrl("cmg.aspx?act=Sign") + "\">签到</a>." + "<a href=\"" + Utils.getUrl("cmg.aspx?act=ReloadPage") + "\">刷新</a><br />");
        }
        builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx?act=buyuRule") + "\">规则</a>." + "<a href=\"" + Utils.getUrl("cmg.aspx?act=Top") + "\">排行</a>." + "<a href=\"" + Utils.getUrl("cmg.aspx?act=Topji") + "\">等级</a>." + "<a href=\"" + Utils.getUrl("cmg.aspx?act=Record") + "\">记录</a>." + "<a href=\"" + Utils.getUrl("cmg.aspx?act=addJ") + "\">兑奖</a><br />");
        builder.Append("准备好了吧？ ");
        builder.Append("选择场景出发吧！<br />");
        #endregion
        #region 查询场景
        //依次取出场景的名称id
        DataSet dtchangj = new BCW.bydr.BLL.CmgDaoju().GetList("ID", "Xiaoxi=0 and Tianyuan=0 ORDER BY changj2 ASC");
        try
        {
            //查询场景 
            for (ptype = 0; ptype < 6; ptype++)
            {
                int ID1 = Convert.ToInt32(dtchangj.Tables[0].Rows[ptype][0]);
                BCW.bydr.Model.CmgDaoju model0 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(ID1);
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx?act=Buyujudge&amp;ptype=" + ptype) + "\">" + model0.Changjing + "&gt;" + "</a><br />");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        catch
        {
            builder.Append("游戏场景出错！<br />");
        }
        #endregion
        builder.Append("<b style=\"color:black\">=捕鱼动态=</b><br />");
        #region 捕鱼动态 
        //获取最新数据
        DataSet notes = new BCW.bydr.BLL.Cmg_notes().GetList("Top(5) ID", "Vit=0  ORDER BY Stime desc");
        if (notes != null && notes.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < notes.Tables[0].Rows.Count; i++)
            {
                int maxID1 = Convert.ToInt32(notes.Tables[0].Rows[i][0]);
                BCW.bydr.Model.Cmg_notes model1 = new BCW.bydr.BLL.Cmg_notes().GetCmg_notes(maxID1);
                string name = new BCW.BLL.User().GetUsName(model1.usID);
                if (model1 == null)
                    continue;
                else
                {
                    TimeSpan time = DateTime.Now - model1.Stime;
                    int d1 = time.Days;
                    int d = time.Hours;
                    int e = time.Minutes;
                    int f = time.Seconds;
                    string daoju = "";
                    #region 获得道具名称
                    BCW.bydr.Model.CmgDaoju modeldao = null;
                    if (model1.random == 0)
                        daoju = "超级大奖";
                    else
                    {
                        modeldao = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(model1.random);
                        daoju = modeldao.Daoju;
                    }
                    #endregion
                    #region 钓鱼日志显示

                    if (d1 == 0)
                    {
                        if (d == 0 && e == 0 && f == 0)
                        {
                            builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model1.usID + "") + "\">" + name + "</a>" + "在" + model1.changj + "钓中了" + daoju + "" + model1.AcolletGold + "币" + "<br />");
                        }
                        else if (d == 0 && e == 0)
                        {
                            builder.Append(f + "秒前" + " " + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model1.usID + "") + "\">" + name + "</a>" + "在" + model1.changj + "钓中了" + daoju + "" + model1.AcolletGold + "币" + "<br />");
                        }
                        else if (d == 0)
                        {
                            builder.Append(e + "分前" + " " + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model1.usID + "") + "\">" + name + "</a>" + "在" + model1.changj + "钓中了" + daoju + "" + model1.AcolletGold + "币" + "<br />");
                        }
                        else
                        {
                            builder.Append(d + "小时前" + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model1.usID + "") + "\">" + name + "</a>" + "在" + model1.changj + "钓中了" + daoju + "" + model1.AcolletGold + "币" + "<br />");
                        }
                    }
                    else
                    {

                        builder.Append(d1 + "天前" + " " + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model1.usID + "") + "\">" + name + "</a>" + "在" + model1.changj + "钓中了" + daoju + "" + model1.AcolletGold + "币" + "<br />");
                    }
                    #endregion
                }
            }
            builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx?act=dongtai") + "\">更多动态>></a>");
        }
        else
        {
            builder.Append("暂无捕鱼数据.");
        }

        #endregion
        //闲聊显示
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.SysUBB(BCW.User.Users.ShowSpeak(26, "cmg.aspx", 5, 0)));
        //游戏底部Ubb
        string Foot = ub.GetSub("bydrFoot", xmlPath);
        if (Foot != "")
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(Foot)));
            builder.Append(Out.Tab("</div>", ""));
        }
        DUBBPage();
    }

    private void dongtaiPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx") + "\">捕鱼</a>&gt;捕鱼动态");
        Master.Title = "" + bydrname + "_捕鱼动态";
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex = 0;//当前页
        int recordCount;//记录总条数
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));//分页大小
        string strWhere = "AcolletGold!=0";
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        // 开始读取列表
        IList<BCW.bydr.Model.Cmg_notes> listcmglist = new BCW.bydr.BLL.Cmg_notes().GetCmg_notess(pageIndex, pageSize, strWhere, out recordCount);
        if (listcmglist.Count > 0)
        {
            int k = 1;
            foreach (BCW.bydr.Model.Cmg_notes n in listcmglist)
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
                string mename = new BCW.BLL.User().GetUsName(Convert.ToInt32(n.usID));
                string daoju = "";
                BCW.bydr.Model.CmgDaoju modeldao = null;
                try
                {
                    modeldao = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(n.random);
                    daoju = modeldao.Daoju;
                }
                catch
                {
                    daoju = "超级大奖";
                }
                TimeSpan time = DateTime.Now - n.Stime;
                int d1 = time.Days;
                int d = time.Hours;
                int e = time.Minutes;
                int f = time.Seconds;


                if (d1 == 0)
                {
                    if (d == 0 && e == 0 && f == 0)
                    {
                        builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.usID + "") + "\">" + mename + "</a>" + "在" + n.changj + "钓中了" + daoju + n.AcolletGold + "币" + "");
                    }
                    else if (d == 0 && e == 0)
                        builder.Append(f + "秒前" + " " + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.usID + "") + "\">" + mename + "</a>" + "在" + n.changj + "钓中了" + daoju + n.AcolletGold + "币" + "");
                    else if (d == 0)
                        builder.Append(e + "分前" + " " + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.usID + "") + "\">" + mename + "</a>" + "在" + n.changj + "钓中了" + daoju + n.AcolletGold + "币" + "");
                    else
                        builder.Append(d + "小时前" + " " + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.usID + "") + "\">" + mename + "</a>" + "在:" + n.changj + "钓中了" + daoju + n.AcolletGold + "币" + "");
                }
                else
                {
                    builder.Append(d1 + "天前" + " " + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.usID + "") + "\">" + mename + "</a>" + "在" + n.changj + "钓中了" + daoju + n.AcolletGold + "币" + "");
                }

                builder.Append(Out.Tab("</div>", ""));
                k++;
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0) + "");
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录..") + "");
        }
        DUBBPage();
    }

    private void addbuyub()
    {
        int meid = new BCW.User.Users().GetUsId();
        BCW.bydr.Model.Cmg_Top m1 = new BCW.bydr.Model.Cmg_Top();
        m1.AllcolletGold = 0;
        m1.Bid = 0;
        m1.Changj = "";
        m1.ColletGold = 0;
        m1.DcolletGold = 10;
        m1.jID = 0;
        m1.McolletGold = 0;
        m1.randdaoju = "";
        m1.randgoldnum = "";
        m1.randnum = 0;
        m1.randyuID = "";
        m1.Time = DateTime.Now;
        m1.updatetime = DateTime.Now;
        m1.usID = meid;
        m1.YcolletGold = 0;
        m1.randten = "";
        m1.isrobot = 0;
        new BCW.bydr.BLL.Cmg_Top().Add(m1);

        BCW.bydr.Model.CmgToplist m2 = new BCW.bydr.Model.CmgToplist();
        m2.AllcolletGold = 0;
        m2.DcolletGold = 0;
        m2.McolletGold = 0;
        m2.stype = 0;
        m2.YcolletGold = 0;
        m2.Time = DateTime.Now;
        m2.usID = meid;
        m2.sid = 0;
        m2.updatetime = DateTime.Now;
        m2.vit = 0;
        m2.Signtime = Convert.ToDateTime("2015-09-07 08:51:52.347");
        new BCW.bydr.BLL.CmgToplist().Add(m2);

        BCW.bydr.Model.Cmg_buyuDonation m3 = new BCW.bydr.Model.Cmg_buyuDonation();
        m3.Ctime = DateTime.Now;
        m3.Donation = 0;
        m3.stype = 0;
        m3.time = DateTime.Now;
        m3.usID = meid;
        new BCW.bydr.BLL.Cmg_buyuDonation().Add(m3);
    }

    //奖池记录
    private void RecordjPage()
    {
        Master.Title = "" + bydrname + "_奖池记录";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx") + "\">捕鱼&gt;</a>");
        builder.Append("奖池记录");
        builder.Append(Out.Tab("</div>", "<br />"));
        //if (meid == 0)
        //    Utils.Login();
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));//分页大小;
        string strWhere = "jID=1 and DcolletGold=10";
        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.bydr.Model.Cmg_Top> listcmglist = new BCW.bydr.BLL.Cmg_Top().GetCmg_Tops2(pageIndex, pageSize, strWhere, out recordCount);
        if (listcmglist.Count > 0)
        {
            int k = 1;
            foreach (BCW.bydr.Model.Cmg_Top n in listcmglist)
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
                string sText = string.Empty;
                int wd = (pageIndex - 1) * 10 + k;
                sText = wd + "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.usID + "") + "\">" + new BCW.BLL.User().GetUsName(n.usID) + "</a>" + "在" + Convert.ToDateTime(n.updatetime).ToString("yyyy-MM-dd HH:mm:ss") + "," + "捕了" + Convert.ToInt64(n.ColletGold) + ub.Get("SiteBz");
                builder.AppendFormat(sText);
                builder.Append(Out.Tab("</div>", ""));
                k++;
            }
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0) + "");
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录..") + "");
        }
        DUBBPage();
    }

    //玩法说明
    private void buyuRulePage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx") + "\">捕鱼</a>&gt;捕鱼规则");
        Master.Title = "" + bydrname + "_捕鱼规则";
        double buyuprofit = 0;
        try
        {
            buyuprofit = buyuprofit1 / 1000;
        }
        catch
        {
            buyuprofit = 0;
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"\">", ""));
        builder.Append("捕鱼达人是会员与会员之间娱乐的游戏,将倒霉鬼的币币分配给幸运儿,奖池的币是会员输赢剩下和捐款得来,每一笔消费记录都能在奖池查看,保证奖池真实性.<br />");
        builder.Append("1.捕鱼共六个场景,每个场景入场费用都不一样,不一样的场景捕到的道具价值都不一样.<br />");

        builder.Append("2." + "<a href=\"" + Utils.getUrl("../finance.aspx?act=addvip") + "\">vip</a>" + "每天免费参与一次" + "<a href=\"" + Utils.getUrl("cmg.aspx?act=Buyujudge&amp;ptype=0") + "\">山涧小溪</a>" + ".<br />");
        builder.Append("3.捕鱼兑奖请在游戏里点击兑奖键,即可获奖.<br />");
        builder.Append("4.每进入一个场景,可以参与10次捕鱼,捕完后才能去其它场景.<br />");
        builder.Append("5.等级升级:每进行10个场景捕鱼即升1级,捕的次数越多,级别越高.<br />");
        builder.Append("6.每玩一场景获得的奖励系统会收" + buyuprofit + "手续费.不足1币时四舍五入,其它不收取任何费用.<br />");
        builder.Append("7.捕鱼一共六个场景,每个场景入场费用都是不一样的,不一样的场景捕到的鱼种价值也是不一样的.<br />");
        builder.Append("8.每进行一场次,会消耗1点体力,当体力为0时将不能捕鱼,每天签到送10点体力," + "<a href=\"" + Utils.getUrl("../finance.aspx?act=addvip") + "\">vip</a>" + "送20点体力,体力签到累计100时为满.<br />");
        builder.Append("注意：已扣币未玩完的请在24小时内捕完，超时作废.");
        builder.Append(Out.Tab("</div>", ""));
        //游戏底部
        DUBBPage();
    }

    //体力兑换
    private void addTPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Master.Title = "" + bydrname + "_体力兑换";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx") + "\">捕鱼</a>&gt;");
        builder.Append("体力兑换");
        builder.Append(Out.Tab("</div>", "<br />"));

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));

        builder.Append(Out.Tab("<div class=\"\">", ""));
        builder.Append("您目前自带" + Utils.ConvertGold(new BCW.BLL.User().GetGold(meid)) + ub.Get("SiteBz") + "<br />");
        builder.Append("1体力=" + Ti + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "输入购买体力数:/,";
        string strName = "uid,backurl";
        string strType = "num,hidden";
        string strValu = "'" + Utils.getPage(0) + "";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "确认购买,cmg.aspx?act=resetjudge,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div>", "<br/>"));
        builder.Append("<a href=\"" + Utils.getPage("cmg.aspx?act=add") + "\">再看看吧&gt;&gt;</a>");
        builder.Append(Out.Tab("</div>", ""));
        DUBBPage();
    }

    //捐款
    private void addzanzhuPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx") + "\">捕鱼</a>&gt;");
        builder.Append("捐款<br />");
        Master.Title = "" + bydrname + "_捐款";
        builder.Append(Out.Tab("</div>", ""));

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        builder.Append(Out.Tab("<div>", ""));
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[0-4]$", "1"));
        if (ptype == 1)
            builder.Append("<h style=\"color:red\">我的捐款" + "</h>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx?act=addzanzhu&amp;ptype=1&amp;id=" + ptype + "") + "\">我的捐款</a>" + "|");
        if (ptype == 2)
            builder.Append("<h style=\"color:red\">捐款记录" + "</h>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx?act=addzanzhu&amp;ptype=2&amp;id=" + ptype + "") + "\">捐款记录</a>" + "|");
        if (ptype == 3)
            builder.Append("<h style=\"color:red\">捐款排行" + "</h>" + "<br />");
        else
            builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx?act=addzanzhu&amp;ptype=3&amp;id=" + ptype + "") + "\">捐款排行</a>" + "<br />");
        builder.Append(Out.Tab("</div>", ""));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));//分页大小;
        string strWhere = string.Empty;
        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        if (ptype == 1 || ptype == 2)
        {
            if (ptype == 1)
                strWhere = "stype=1 and usID=" + meid;
            else if (ptype == 2)
                strWhere = "stype=1";

            // 开始读取列表
            IList<BCW.bydr.Model.Cmg_buyuDonation> listcmglist1 = new BCW.bydr.BLL.Cmg_buyuDonation().GetCmg_buyuDonations(pageIndex, pageSize, strWhere, out recordCount);
            {
                if (listcmglist1.Count > 0)
                {
                    int k = 1;
                    foreach (BCW.bydr.Model.Cmg_buyuDonation n in listcmglist1)
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
                        string sText = string.Empty;
                        int wd = (pageIndex - 1) * 10 + k;
                        string mename = new BCW.BLL.User().GetUsName(Convert.ToInt32(n.usID));
                        sText = wd + "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.usID + "") + "\">" + mename + "(" + n.usID + ")" + "</a>" + "捐助了" + n.Donation + ub.Get("SiteBz") + ".[" + Convert.ToDateTime(n.Ctime).ToString("yyyy-MM-dd hh:mm:ss") + "]";
                        builder.AppendFormat("<a href=\"" + Utils.getUrl("cmg.aspx?act=Top&amp;id=" + ptype + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>" + sText);
                        builder.Append(Out.Tab("</div>", ""));
                        k++;
                    }
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0) + "");
                }
                else
                {
                    builder.Append(Out.Div("div", "没有相关记录..") + "");
                }
            }
        }
        else
        {
            string Logo = ub.GetSub("bydrLogo", xmlPath);
            strWhere = "YcolletGold != 0";
            string strOrder = "YcolletGold Desc";

            // 开始读取列表
            IList<BCW.bydr.Model.CmgToplist> listcmglist = new BCW.bydr.BLL.CmgToplist().GetCmgToplists1(pageIndex, pageSize, strWhere, strOrder, out recordCount);
            if (listcmglist.Count > 0)
            {
                int k = 1;

                foreach (BCW.bydr.Model.CmgToplist n in listcmglist)
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
                    string mename = new BCW.BLL.User().GetUsName(Convert.ToInt32(n.usID));
                    string sText = string.Empty;
                    int wd = (pageIndex - 1) * 10 + k;
                    sText = ".<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.usID + "") + "\">" + mename + "(" + n.usID + ")" + "</a>" + "捐助了" + "<b style=\"color:red\">" + n.YcolletGold + "</b>" + ub.Get("SiteBz");
                    if (wd == 1)
                        builder.AppendFormat("<img src=\"" + Logo + "top1.gif" + "\" alt=\"load\"/>" + "<a href=\"" + Utils.getUrl("cmg.aspx?act=Top&amp;id=" + ptype + "") + "\"></a>" + sText);
                    else if (wd == 2)
                        builder.AppendFormat("<img src=\"" + Logo + "top1.gif" + "\" alt=\"load\"/>" + "<a href=\"" + Utils.getUrl("cmg.aspx?act=Top&amp;id=" + ptype + "") + "\"></a>" + sText);
                    else if (wd == 3)
                        builder.AppendFormat("<img src=\"" + Logo + "top1.gif" + "\" alt=\"load\"/>" + "<a href=\"" + Utils.getUrl("cmg.aspx?act=Top&amp;id=" + ptype + "") + "\"></a>" + sText);
                    else
                        builder.AppendFormat("<a href=\"" + Utils.getUrl("cmg.aspx?act=Top&amp;id=" + ptype + "") + "\"></a>" + wd + sText);
                    builder.Append(Out.Tab("</div>", ""));
                    k++;

                }
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0) + "");
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录..") + "");
            }
        }

        int uid1 = int.Parse(Utils.GetRequest("uid1", "all", 1, @"^[1-9]\d*$", "0"));
        string strText = "输入捐款数额:/,";
        string strName = "uid1,backurl";
        string strType = "num,hidden";
        string strValu = "'" + Utils.getPage(0) + "";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "确认,cmg.aspx?act=resetjudge,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        DUBBPage();
    }

    //确认捐款
    private void resetjudgePage()
    {
        Master.Title = "" + bydrname + "_确认支付";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx") + "\">捕鱼</a>&gt;<a href=\"" + Utils.getUrl("cmg.aspx?act=addzanzhu") + "\">捐款</a>&gt;确认");
        builder.Append(Out.Tab("</div>", "<br />"));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string info = Utils.GetRequest("info", "all", 1, "", "");
        long uid1 = int.Parse(Utils.GetRequest("uid1", "all", 1, @"^[1-9]\d*$", "0"));
        long uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        long prices = Convert.ToInt64(uid * Ti);
        long Gold = new BCW.BLL.User().GetGold(meid);
        if (Gold < prices)
        {
            Utils.Error("您的" + ub.Get("SiteBz") + "不足", "");
        }
        else if (Gold < uid1)
        {
            Utils.Error("您的" + ub.Get("SiteBz") + "不足", "");
        }
        //支付安全提示
        if (info == "")
        {
            //支付安全提示
            string[] p_pageArr = { "Price", "uid", "uid1", "act", "info" };
            BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr);
            if (uid == 0)
            {
                if (uid1 == 0)
                {
                    Utils.Error("温馨提示请输入具体" + ub.Get("SiteBz") + "！！！马上返回...", "");
                }
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("您即将捐款" + uid1 + ub.Get("SiteBz") + ".<br />");
                builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx?act=resetzz&amp;uid1=" + uid1) + "\">确认捐款</a>.");
                builder.Append("<a href=\"" + Utils.getPage("cmg.aspx?act=addzanzhu") + "\">再看看吧</a>");
                builder.Append(Out.Tab("</div>", ""));
                DUBBPage();
            }
            else
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("您即将购买" + uid + "体力，" + "需要花费" + (prices) + ub.Get("SiteBz") + "<br />");
                builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx?act=reset&amp;uid=" + uid) + "\">确认购买</a>");
                builder.Append("<br /><a href=\"" + Utils.getPage("cmg.aspx?act=add") + "\">再看看吧>></a>");
                builder.Append(Out.Tab("</div>", ""));
                DUBBPage();
            }
        }
    }

    private void resetzzPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        long Gold = new BCW.BLL.User().GetGold(meid);
        long uid1 = int.Parse(Utils.GetRequest("uid1", "all", 2, @"^[1-9]\d*$", "输入金额非法，请重新输入."));

        string info = Utils.GetRequest("info", "all", 1, "", "");
        Master.Title = "" + bydrname + "_捐款成功";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx") + "\">捕鱼</a>&gt;<a href=\"" + Utils.getUrl("cmg.aspx?act=addzanzhu") + "\">捐款</a>&gt;捐款成功");
        builder.Append(Out.Tab("</div>", "<br />"));
        if (info == "")
        {
            if (Gold < uid1)
            {
                Utils.Error("您的" + ub.Get("SiteBz") + "不足", "");
            }
            else
            {
                new BCW.bydr.BLL.CmgToplist().UpdateYcolletGold(meid, +uid1);
                string mename = new BCW.BLL.User().GetUsName(meid);
                new BCW.BLL.User().UpdateiGold(meid, -uid1, "捕鱼捐款消费");

                new BCW.BLL.Action().Add(1005, 0, meid, mename, "在[URL=/bbs/game/cmg.aspx]捕鱼[/URL]捐助了**" + ub.Get("SiteBz") + ".");//动态" + uid1 + "//26

                DataSet rows = new BCW.bydr.BLL.Cmg_buyuDonation().GetList("ID", "usID=" + meid);
                int id1 = Convert.ToInt32(rows.Tables[0].Rows[0][0]);
                BCW.bydr.Model.Cmg_buyuDonation model2 = new BCW.bydr.BLL.Cmg_buyuDonation().GetCmg_buyuDonation(id1);
                BCW.bydr.Model.Cmg_buyuDonation model1 = new BCW.bydr.Model.Cmg_buyuDonation();
                model1.Ctime = DateTime.Now;
                model1.Donation = uid1;
                model1.stype = 1;
                model1.usID = meid;
                model1.time = model2.time;
                new BCW.bydr.BLL.Cmg_buyuDonation().Add(model1);
                //活跃抽奖入口_20160621姚志光
                try
                {
                    //表中存在记录
                    if (new BCW.BLL.tb_WinnersGame().ExistsGameName("捕鱼达人"))
                    {
                        //投注是否大于设定的限额，是则有抽奖机会
                        if (uid1 > new BCW.BLL.tb_WinnersGame().GetPrice("捕鱼达人"))
                        {
                            string TextForUbb = (ub.GetSub("TextForUbb", "/Controls/winners.xml"));//设置内线提示的文字
                            string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", "/Controls/winners.xml"));//1发内线2不发内线 
                            int hit = new BCW.winners.winners().CheckActionForAll(1, 1, meid, mename, "捕鱼捐款", 3);
                            if (hit == 1)
                            {
                                //内线开关 1开
                                if (WinnersGuessOpen == "1")
                                {
                                    //发内线到该ID
                                    new BCW.BLL.Guest().Add(0, meid, mename, TextForUbb);
                                }
                            }
                        }
                    }
                }
                catch { }
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("您已赞助" + uid1 + ub.Get("SiteBz") + "<br />");
                builder.Append("已花费:" + uid1 + ub.Get("SiteBz") + "<br />");
                builder.Append(Out.Tab("</div>", ""));
            }
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("赞助成功！祝您玩的开心！");
            builder.Append("<br /><a href=\"" + Utils.getPage("cmg.aspx?act=addzanzhu") + "\">返回&gt;&gt;</a>");
            builder.Append(Out.Tab("</div>", ""));
            DUBBPage();
        }
    }

    private void RecordPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx") + "\">捕鱼</a>&gt;");
        builder.Append("记录");
        Master.Title = "" + bydrname + "_我的捕鱼记录";
        builder.Append(Out.Tab("</div>", "<br />"));

        int meid = new BCW.User.Users().GetUsId();
        builder.Append(Out.Tab("<div class=\"\">", ""));

        if (meid == 0)
            Utils.Login();
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[0-4]$", "0"));
        builder.Append("我的捕鱼记录");
        builder.Append(Out.Tab("</div>", "<br />"));
        Utils.getUrl("cmg.aspx?act=Record&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "");

        if (ptype == 0)
        {
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));//分页大小;
            string strWhere = string.Empty;
            strWhere = "usID=" + meid + "and jID=1 and DcolletGold=10";
            string[] pageValUrl = { "act", "ptype", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            // 开始读取列表
            IList<BCW.bydr.Model.Cmg_Top> listcmglist = new BCW.bydr.BLL.Cmg_Top().GetCmg_Tops(pageIndex, pageSize, strWhere, out recordCount);
            if (listcmglist.Count > 0)
            {
                int k = 1;

                foreach (BCW.bydr.Model.Cmg_Top n in listcmglist)
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
                    string sText = string.Empty;
                    string st = string.Empty;
                    int wd = (pageIndex - 1) * 10 + k;
                    if (n.Bid == 1)
                        st = "(未兑奖)";
                    else
                        st = "(已兑奖)";
                    sText = wd + "." + "在" + n.Changj + "捕到了" + Convert.ToInt64((n.ColletGold)) + "" + ub.Get("SiteBz") + st + "," + "编号" + n.ID + ".";

                    builder.AppendFormat("<a href=\"" + Utils.getUrl("cmg.aspx?act=Top&amp;id=" + ptype + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>" + sText);
                    builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx?act=list&amp;id=" + n.ID + "") + "&amp;backurl=" + Utils.PostPage(1) + "" + "\">查看&gt;</a>");
                    builder.Append(Out.Tab("</div>", ""));
                    k++;
                }
                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0) + "");
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录..") + "");
            }
        }
        DUBBPage();
    }

    //等级排行
    private void TopjiPage()
    {
        Master.Title = "" + bydrname + "_等级榜";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx") + "\">捕鱼</a>&gt;等级榜");
        builder.Append(Out.Tab("</div>", ""));

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string Logo = ub.GetSub("bydrLogo", xmlPath);
        builder.Append("<img src=\"" + Logo + "bydr1.gif" + "\" alt=\"load\"/><br />");
        BCW.bydr.Model.CmgToplist model2 = new BCW.bydr.BLL.CmgToplist().GetCmgToplistusID(meid);
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("我的等级：" + model2.stype / 10 + "级<br />");
        builder.Append(Out.Tab("</div>", ""));
        int pageIndex = 0;//当前页
        int recordCount;//记录总条数
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));//分页大小
        string strWhere = "";
        string strOrder = "";
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        strOrder = "stype Desc";
        // 开始读取列表
        IList<BCW.bydr.Model.CmgToplist> listcmglist = new BCW.bydr.BLL.CmgToplist().GetCmgToplists1(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listcmglist.Count > 0)
        {
            int k = 1;

            foreach (BCW.bydr.Model.CmgToplist n in listcmglist)
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
                string mename = new BCW.BLL.User().GetUsName(Convert.ToInt32(n.usID));
                int wd = (pageIndex - 1) * 10 + k;
                if (wd == 1)
                {
                    builder.Append("<img src=\"" + Logo + "top1.gif" + "\" alt=\"load\"/>" + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.usID + "") + "\">" + mename + "(" + n.usID + ")" + "</a>" + " " + n.stype / 10 + "级");
                }
                else if (wd == 2)
                    builder.Append("<img src=\"" + Logo + "top2.gif" + "\" alt=\"load\"/>" + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.usID + "") + "\">" + mename + "(" + n.usID + ")" + "</a>" + " " + n.stype / 10 + "级");
                else if (wd == 3)
                    builder.Append("<img src=\"" + Logo + "top3.gif" + "\" alt=\"load\"/>" + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.usID + "") + "\">" + mename + "(" + n.usID + ")" + "</a>" + " " + n.stype / 10 + "级");
                else
                    builder.Append(wd + "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.usID + "") + "\">" + mename + "(" + n.usID + ")" + "</a>" + " " + n.stype / 10 + "级");
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0) + "");
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录..") + "");
        }
        DUBBPage();
    }

    //我的捕鱼记录——详细
    private void listPage()
    {
        Master.Title = "" + bydrname + "_我的捕鱼记录";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx") + "\">捕鱼</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx?act=Record") + "\">记录</a>&gt;记录详细<br />");
        builder.Append(Out.Tab("</div>", ""));

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string name = new BCW.BLL.User().GetUsName(meid);
        int id = Utils.ParseInt(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.bydr.Model.Cmg_Top model = new BCW.bydr.BLL.Cmg_Top().GetCmg_Top(id, meid);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        BCW.bydr.Model.Cmg_Top model1 = new BCW.bydr.BLL.Cmg_Top().GetCmg_Top(id, meid);
        string[] randaoju = model1.randdaoju.Split(',');
        string[] randyu = model1.randyuID.Split(',');
        string[] randgoldnum = model1.randgoldnum.Split(',');
        int s = 0;
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("==<b>捕鱼详情</b>==" + "<br />");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"\">", ""));
        builder.Append(" " + "编号：" + id + "<br />");
        builder.Append(" " + "场景：" + model1.Changj + "<br />");
        builder.Append(" " + "结束时间：" + Convert.ToDateTime(model1.updatetime).ToString("yyyy-MM-dd HH:mm:ss") + "<br />");
        if (Convert.ToInt32(randgoldnum[model1.randnum]) == 0)
        {
            builder.Append(" " + "采到超级好运币：" + model1.ColletGold + "<br />");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            builder.Append(" " + "共采" + ub.Get("SiteBz") + "：" + model1.ColletGold + "<br />");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("==<b>采集到的物品</b>==" + "<br />");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"\">", ""));
            try
            {
                for (int i = 0; i < 10; i++)
                {
                    s += Convert.ToInt32(randaoju[i]);
                }
            }
            catch
            {
                try
                {

                    for (int i = 1; i < 11; i++)
                    {
                        DataSet rows91 = null;
                        if (i == 10)
                        {
                            rows91 = new BCW.bydr.BLL.Cmg_notes().GetList("ID", "usID=" + meid + "and stype=0 and cxid=" + model.Bid);
                        }
                        else
                        {
                            rows91 = new BCW.bydr.BLL.Cmg_notes().GetList("ID", "usID=" + meid + "and stype=" + i + "and cxid=" + model.Bid);
                        }

                        BCW.bydr.Model.Cmg_notes model2 = null;
                        int id21 = 0;
                        try
                        {
                            id21 = Convert.ToInt32(rows91.Tables[0].Rows[0][0]);
                            model2 = new BCW.bydr.BLL.Cmg_notes().GetCmg_notes(Convert.ToInt32(id21));
                        }
                        catch
                        {
                            DataSet rows9 = new BCW.bydr.BLL.Cmg_notes().GetList("ID", "usID=" + meid + "and stype=11 and cxid=" + model.Bid);
                            id21 = Convert.ToInt32(rows9.Tables[0].Rows[0][0]);
                            model2 = new BCW.bydr.BLL.Cmg_notes().GetCmg_notes(Convert.ToInt32(id21));
                        }

                        if (model2 == null)
                            Utils.Error("不存在的记录", "");
                        else
                        {
                            builder.Append(model2.coID + "+" + (model2.AcolletGold) + ub.Get("SiteBz") + "<br />");
                        }
                    }
                }
                catch
                {
                    builder.Append("");
                }
            }
            try
            {
                for (int i = 0; i < 10; i++)
                {
                    if (Convert.ToInt32(randaoju[i]) == 0)
                        continue;
                    builder.Append(new BCW.bydr.BLL.CmgDaoju().GetyuName(Convert.ToInt32(randyu[i])) + "+" + randaoju[i] + ub.Get("SiteBz") + "<br />");
                }
            }
            catch
            {
                builder.Append("没有数据.");
            }
        }
        builder.Append(Out.Tab("</div>", ""));
        DUBBPage();
    }

    //兑奖页面
    private void addJPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx") + "\">捕鱼</a>&gt;");
        Master.Title = "" + bydrname + "_捕鱼兑奖";
        builder.Append("兑奖");
        builder.Append(Out.Tab("</div>", "<br />"));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        double buyuprofit = 0;
        try
        {
            buyuprofit = buyuprofit1 / 1000;
        }
        catch
        {
            buyuprofit = 0;
        }
        builder.Append(Out.Tab("<div class=\"\">", ""));
        builder.Append("您现在有" + Utils.ConvertGold(new BCW.BLL.User().GetGold(meid)) + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));//分页大小
        string strWhere = string.Empty;
        strWhere = "usID=" + meid + "" + " and Bid=1 and DcolletGold=10";
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        string arrId = "";
        string BYDYqi = "";
        // 开始读取列表
        IList<BCW.bydr.Model.Cmg_Top> listcmglist = new BCW.bydr.BLL.Cmg_Top().GetCmg_Tops(pageIndex, pageSize, strWhere, out recordCount);
        if (listcmglist.Count > 0)
        {
            int k = 1;

            foreach (BCW.bydr.Model.Cmg_Top n in listcmglist)
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
                int wd = (pageIndex - 1) * 10 + k;
                builder.Append(wd + "." + "采到了" + Convert.ToInt64(n.ColletGold * (1 - buyuprofit)) + "" + ub.Get("SiteBz") + "," + "编号" + (n.ID) + "<a href=\"" + Utils.getUrl("cmg.aspx?act=alllistadd&amp;pid=" + (n.ID) + "") + "\">兑奖</a>");
                BYDYqi = n.Bid.ToString();
                arrId = arrId + " " + (n.ID);
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0) + "");
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录..") + "");
        }
        if (!string.IsNullOrEmpty(arrId))
        {
            builder.Append(Out.Tab("", "<br />"));
            arrId = arrId.Trim();
            arrId = arrId.Replace(" ", ",");
            string strName = "arrId,act";
            string strValu = "" + arrId + "'listaddPage";
            string strOthe = "本页兑奖,cmg.aspx?act=listadd,post,0,red";
            builder.Append(Out.wapform(strName, strValu, strOthe) + "");
        }
        DUBBPage();
    }
    //单个兑奖
    private void listaddPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        double buyuprofit = 0;
        try
        {
            buyuprofit = buyuprofit1 / 1000;
        }
        catch
        {
            buyuprofit = 0;
        }
        string arrId = "";
        arrId = Utils.GetRequest("arrId", "post", 1, "", "");

        if (!Utils.IsRegex(arrId.Replace(",", ""), @"^[0-9]\d*$"))
        {
            Utils.Error("选择本页兑奖出错", "");
        }
        string[] strArrId = arrId.Split(",".ToCharArray());
        long getwinMoney = 0;
        long winMoney = 0;
        BCW.User.Users.IsFresh("1cmg", 2);//防刷
        for (int i = 0; i < strArrId.Length; i++)
        {
            int pid = Convert.ToInt32(strArrId[i]);
            BCW.bydr.Model.Cmg_Top model = new BCW.bydr.BLL.Cmg_Top().GetCmg_Top(pid);
            if (model.Expiry == 0)
            {
                new BCW.bydr.BLL.Cmg_Top().UpdateExpiry(1, pid);
                if (new BCW.bydr.BLL.Cmg_Top().ExistsBid(pid, meid))
                {
                    new BCW.bydr.BLL.Cmg_Top().UpdateBid(0, pid, meid);
                    string mename = new BCW.BLL.User().GetUsName(meid);
                    winMoney = Convert.ToInt64(model.ColletGold * (1 - buyuprofit));
                    new BCW.BLL.User().UpdateiGold(meid, +winMoney, "捕鱼兑奖-标识id" + pid + "");
                }
                getwinMoney = getwinMoney + winMoney;
                new BCW.bydr.BLL.Cmg_Top().UpdateExpiry(0, pid);
            }
        }
        if (getwinMoney == 0)
        {
            Utils.Success("兑奖", "重复兑奖或没有可以兑奖的记录.", Utils.getUrl("cmg.aspx?act=addJ"), "2");
        }
        else
        {
            Utils.Success("兑奖", "恭喜，成功兑奖" + getwinMoney + " " + ub.Get("SiteBz") + "", Utils.getUrl("cmg.aspx?act=addJ"), "2");
        }
    }
    //全部兑奖
    private void alllistaddPage()
    {
        double buyuprofit = 0;
        try
        {
            buyuprofit = buyuprofit1 / 1000;
        }
        catch
        {
            buyuprofit = 0;
        }
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        int pid = Utils.ParseInt(Utils.GetRequest("pid", "get", 2, @"^[0-9]\d*$", "选择兑奖无效"));
        BCW.bydr.Model.Cmg_Top model = new BCW.bydr.BLL.Cmg_Top().GetCmg_Top(pid);

        if (model.Bid == 1 && model.usID == meid)
        {
            if (model.Expiry == 0)//重复兑奖标识
            {
                BCW.User.Users.IsFresh("1cmg", 2);//防刷
                new BCW.bydr.BLL.Cmg_Top().UpdateExpiry(1, pid);//入场改变重复兑奖标识
                //string mename = new BCW.BLL.User().GetUsName(meid);
                new BCW.BLL.User().UpdateiGold(meid, +Convert.ToInt64((model.ColletGold) * (1 - buyuprofit)), "捕鱼兑奖—标识id" + pid + "");
                new BCW.bydr.BLL.Cmg_Top().UpdateBid(0, pid, meid);
                new BCW.bydr.BLL.Cmg_Top().UpdateExpiry(0, pid);//出场还原重复兑奖标识
                Utils.Success("兑奖", "恭喜，成功兑奖" + Convert.ToInt64((model.ColletGold) * (1 - buyuprofit)) + "" + ub.Get("SiteBz") + "", Utils.getUrl("cmg.aspx?act=addJ"), "2");
            }
            else
                Utils.Error("抱歉,兑奖失败,请重新核查.", "");
        }
        else
        {
            Utils.Success("兑奖", "重复兑奖或没有可以兑奖的记录", Utils.getUrl("cmg.aspx?act=addJ"), "2");
        }
    }

    //排行榜
    private void TopPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Master.Title = "" + bydrname + "_排行榜";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx") + "\">捕鱼</a>&gt;排行榜");
        builder.Append(Out.Tab("</div>", "<br/>"));

        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[0-4]$", "1"));
        builder.Append(Out.Tab("<div>", ""));
        if (ptype == 1)
            builder.Append("<b style=\"color:red\">今日" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx?act=Top&amp;ptype=1&amp;id=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">今日</a>" + "|");
        if (ptype == 2)
            builder.Append("<b style=\"color:red\">本月" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx?act=Top&amp;ptype=2&amp;id=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">本月</a>" + "|");
        if (ptype == 3)
            builder.Append("<b style=\"color:red\">总榜" + "</b>" + "<br />");
        else
            builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx?act=Top&amp;ptype=3&amp;id=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">总榜</a>" + "<br />");
        builder.Append(Out.Tab("</div>", ""));

        int pageIndex = 0;//当前页
        int recordCount;//记录总条数
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));//分页
        string monthwdy1 = "";
        string monthwdy2 = "";
        //查询条件
        if (ptype == 1)
        {
            monthwdy1 = DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00";
            monthwdy2 = DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59";
        }
        else if (ptype == 2)
        {
            monthwdy1 = DateTime.Now.ToString("yyyy-MM") + "-01 00:00:00";
            monthwdy2 = DateTime.Parse(monthwdy1).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd") + " 23:59:59";
        }
        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        DataSet bang = new BCW.bydr.BLL.Cmg_Top().GetListByPage(monthwdy1, monthwdy2);
        recordCount = Convert.ToInt32(bang.Tables[1].Rows[0][0]);
        if (recordCount > 100)
            recordCount = 100;//显示前100名
        if (recordCount >= 0)
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
                usid = Convert.ToInt32(bang.Tables[0].Rows[koo + soms][0]);
                usmoney = Convert.ToInt64(bang.Tables[0].Rows[koo + soms][1]);
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }
                string mename = new BCW.BLL.User().GetUsName(usid);
                int wd = (pageIndex - 1) * 10 + k;
                builder.Append("" + wd + ".<a  href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + mename + "(" + usid + ")</a>净捕了" + "" + usmoney + "" + ub.Get("SiteBz") + "");
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }


        BCW.bydr.Model.CmgToplist model = new BCW.bydr.BLL.CmgToplist().GetCmgToplistusID(meid);
        builder.Append(Out.Tab("<div>", "<br/>"));
        builder.Append("我的战况：" + "今天" + (new BCW.bydr.BLL.Cmg_Top().GetCmg_AllcolletGold(meid, DateTime.Now.ToString("yyyy-MM-dd"))) + ";");
        string strdatetime1 = DateTime.Now.ToString("yyyy-MM") + "-01 00:00:00";
        string strdatetime2 = DateTime.Parse(strdatetime1).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd") + " 23:59:59";
        builder.Append("本月" + (new BCW.bydr.BLL.Cmg_Top().GetCmg_AllcolletGoldmonth(meid, strdatetime1, strdatetime2)) + ";");
        builder.Append("总榜" + new BCW.bydr.BLL.Cmg_Top().GetCmg_AllcolletGold1(meid) + "");
        builder.Append(Out.Tab("</div>", ""));
        DUBBPage();
    }

    private void restPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        long Gold = new BCW.BLL.User().GetGold(meid);
        long uid = int.Parse(Utils.GetRequest("uid", "all", 2, @"^[1-9]\d*$", "您输入的数字为非法数字，购买失败！"));
        long prices = Convert.ToInt64(uid * Ti);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "")
        {
            Master.Title = "" + bydrname + "_确认购买";
            if (Gold < prices)
            {
                Utils.Error("您的" + ub.Get("SiteBz") + "不足", "");
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx") + "\">捕鱼首页</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                builder.Append("您已购买" + uid + "体力<br />");
                builder.Append("已花费:" + (prices) + ub.Get("SiteBz") + "<br />");
                new BCW.bydr.BLL.CmgToplist().Updatevit(meid, Convert.ToInt32(uid));
                string mename = new BCW.BLL.User().GetUsName(meid);
                new BCW.BLL.User().UpdateiGold(meid, -prices, "捕鱼购买体力花费");
                //活跃抽奖入口_20160621姚志光
                try
                {
                    //表中存在记录
                    if (new BCW.BLL.tb_WinnersGame().ExistsGameName("捕鱼达人"))
                    {
                        //投注是否大于设定的限额，是则有抽奖机会
                        if (prices > new BCW.BLL.tb_WinnersGame().GetPrice("捕鱼达人"))
                        {
                            string TextForUbb = (ub.GetSub("TextForUbb", "/Controls/winners.xml"));//设置内线提示的文字
                            string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", "/Controls/winners.xml"));//1发内线2不发内线 
                            int hit = new BCW.winners.winners().CheckActionForAll(1, 1, meid, mename, "捕鱼购买体力", 3);
                            if (hit == 1)
                            {
                                //内线开关 1开
                                if (WinnersGuessOpen == "1")
                                {
                                    //发内线到该ID
                                    new BCW.BLL.Guest().Add(0, meid, mename, TextForUbb);
                                }
                            }
                        }
                    }
                }
                catch { }
                //动态
                new BCW.BLL.Action().Add(1005, 0, meid, mename, "在[URL=/bbs/game/cmg.aspx]捕鱼[/URL]购买体力花费**" + ub.Get("SiteBz") + "");//" + prices + "//26
            }
            builder.Append("购买成功！祝您玩的开心！");
            builder.Append("<br /><a href=\"" + Utils.getPage("cmg.aspx") + "\">返回&gt;&gt;</a>");
        }
        builder.Append(Out.Tab("</div>", ""));

    }

    private void SignPage()
    {
        //获取当前时间
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string strt1 = bydrBotTime;
        string strT = DateTime.Now.ToString("yyyy-MM-dd");
        BCW.bydr.Model.CmgToplist model2 = new BCW.bydr.BLL.CmgToplist().GetCmgToplistusID(meid);

        try
        {
            strt1 = Convert.ToDateTime(new BCW.bydr.BLL.CmgToplist().GetSigntime(meid)).ToString("yyyy-MM-dd");
        }
        catch
        {
            strt1 = bydrBotTime;
        }
        if (strT == strt1)
        {
            Utils.Success("温馨提示", "今日已签到！！！" + "马上返回...", Utils.getUrl("cmg.aspx"), "1");
        }
        else
        {
            //识别会员vip
            DataSet rows = new BCW.BLL.User().GetList("VipDate", "ID=" + meid);
            DateTime viptime = DateTime.Now;
            try
            {
                viptime = Convert.ToDateTime(rows.Tables[0].Rows[0][0]);
            }
            catch
            {
                viptime = Convert.ToDateTime(bydrBotTime);
            }
            if (DateTime.Now < viptime)
            {
                if (model2.vit < 100)
                {
                    new BCW.bydr.BLL.CmgToplist().Updatevit(meid, 20);
                    new BCW.bydr.BLL.CmgToplist().UpdateSigntime(meid, DateTime.Now);
                    Utils.Success("温馨提示", "签到成功，体力+20" + "马上返回...", Utils.getUrl("cmg.aspx"), "1");
                }
                else
                    Utils.Success("温馨提示", "您的体力大于100点签到失败， 马上返回...", Utils.getUrl("cmg.aspx"), "2");

            }
            else
            {
                if (model2.vit < 100)
                {
                    new BCW.bydr.BLL.CmgToplist().Updatevit(meid, 10);
                    new BCW.bydr.BLL.CmgToplist().UpdateSigntime(meid, DateTime.Now);
                    Utils.Success("温馨提示", "签到成功，体力+10" + "马上返回...", Utils.getUrl("cmg.aspx"), "1");
                }
                else
                {
                    Utils.Success("温馨提示", "您的体力大于100点签到失败， 马上返回...", Utils.getUrl("cmg.aspx"), "2");
                }
            }
        }
    }

    #region 捕鱼开始 buyuPage

    /// <summary>
    /// 捕鱼开始
    /// </summary>
    private void buyuPage()
    {
        Master.Title = "" + bydrname + "_捕鱼游戏中";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 1, @"^[0-5]$", "0"));
        int n = Utils.ParseInt(Utils.GetRequest("n", "get", 1, @"^[0-9]$", "0"));
        int id = Utils.ParseInt(Utils.GetRequest("id", "get", 1, @"^[0-5]$", "0"));
        int time = Convert.ToInt32(ub.GetSub("bydrExpir1", "/Controls/BYDR.xml"));
        long Gold = new BCW.BLL.User().GetGold(meid);
        BCW.bydr.Model.CmgDaoju model0 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(ptype + 1);
        //读取加入的10条数据
        //读取用户id最后一条数据
        DataSet rows11 = new BCW.bydr.BLL.Cmg_Top().GetList("Top 1 ID ", "usID=" + meid + " order by Time desc");
        int id11 = Convert.ToInt32(rows11.Tables[0].Rows[0][0]);
        BCW.bydr.Model.Cmg_Top modle11 = new BCW.bydr.BLL.Cmg_Top().GetCmg_Top(id11);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx") + "\">捕鱼</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        string[] numgoldf = new string[] { };
        builder.Append(Out.Tab("<div>", ""));

        string[] randgoldnum = modle11.randgoldnum.Split(',');
        string[] randaoju = modle11.randdaoju.Split(',');
        string[] randyu = modle11.randyuID.Split(',');
        string[] randten = modle11.randten.Split(',');
        //鱼id
        int[] daojunum1 = new int[] { 15, 19, 22, 23, 24, 24, 37, 38, 59, 67, 73, 79, 84, 85, 92, 13 };//最低价值鱼类
        int[] daojunum2 = new int[] { 16, 18, 29, 32, 34, 40, 48, 49, 51, 54, 56, 60, 63, 68, 70, 71, 75, 76, 77, 81, 72, 82, 91, 92, 93, 94, 95 };//中间价值鱼类
        int[] daojunum3 = new int[] { 100, 101, 102, 97, 98, 99, 96, 14, 17, 20, 25, 21, 27, 28, 30, 31, 33, 36, 39, 42, 43, 44, 45, 46, 47, 50, 52, 53, 57, 58, 64, 65, 66, 69, 74, 78, 80, 83, 86, 87, 88, 89, 90 };//最高价值鱼类
        int randgold = 0;
        try
        {
            randgold = Convert.ToInt32(randgoldnum[modle11.randnum]);
        }
        catch
        {
            randgold = 1;
        }
        #region 防刷判断
        if (((DateTime.Now.AddSeconds(10) - modle11.updatetime.AddSeconds(10)).TotalSeconds > time))
        {
            switch (ptype)
            {
                #region 场景1操作
                case 0:
                    {
                        //读取场景的价格包
                        BCW.bydr.Model.CmgDaoju modeldaoju0 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(300);
                        BCW.bydr.Model.CmgDaoju modeldaoju1 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(301);
                        BCW.bydr.Model.CmgDaoju modeldaoju2 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(302);
                        BCW.bydr.Model.CmgDaoju modeldaoju3 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(303);
                        BCW.bydr.Model.CmgDaoju modeldaoju4 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(304);
                        string[] numgold3 = modeldaoju3.Changjing.Split(',');
                        string[] numgold2 = modeldaoju2.Changjing.Split(',');
                        string[] numgold1 = modeldaoju1.Changjing.Split(',');
                        string[] numgold0 = modeldaoju0.Changjing.Split(',');
                        string[] numgold4 = modeldaoju4.Changjing.Split(',');


                        if (randgold == 0)
                        {
                            #region 获得超级大奖的处理
                            //判断随机给定的场景价格包
                            string[] numgoldo = new string[] { };
                            //判断随机给定的场景价格包
                            if (modle11.McolletGold == 0)
                                numgoldo = numgold0;
                            else if (modle11.McolletGold == 1)
                                numgoldo = numgold1;
                            else if (modle11.McolletGold == 2)
                                numgoldo = numgold2;
                            else if (modle11.McolletGold == 3)
                                numgoldo = numgold3;
                            else if (modle11.McolletGold == 4)
                                numgoldo = numgold4;
                            else
                                Utils.Error("选择场景出错！您属于非法操作！请返回游戏主页重新选择该场景", "");
                            //获得超级大奖的处理
                            int goldnumder = Convert.ToInt32(numgold0[0]);
                            DataSet row11s = new BCW.BLL.User().GetList("VipDate", "ID=" + meid);
                            DateTime viptime2 = DateTime.Now;
                            try
                            {
                                viptime2 = Convert.ToDateTime(row11s.Tables[0].Rows[0][0]);
                            }
                            catch
                            {
                                viptime2 = Convert.ToDateTime(bydrBotTime);
                            }
                            if (DateTime.Now < viptime2)
                            {
                                if (model0.changj2 == 200)
                                {
                                    int count = 0;
                                    count = new BCW.bydr.BLL.Cmg_Top().GetCmgcount(meid);
                                    string swhere = DateTime.Now.ToString("yyyy-MM-dd");
                                    bool viptime11 = new BCW.bydr.BLL.CmgToplist().Existsusvip(meid, swhere);
                                    if (!viptime11 || count == 1)
                                    {
                                        string Logo = ub.GetSub("bydrLogo", xmlPath);
                                        builder.Append(Out.Tab("<div><br />", ""));
                                        builder.Append("<img src=\"" + Logo + "jiang.gif" + "\" alt=\"load\"/><br />");
                                        builder.Append(Out.Tab("</div><br />", "<br />"));
                                        builder.Append("亲，你太好运了！获得了惊喜大奖，，奖励" + goldnumder + "" + ub.Get("SiteBz") + "，惊喜只能捕鱼一次哦，请重新再选择场景继续抓捕<br />");
                                        new BCW.bydr.BLL.Cmg_Top().UpdateAllcolletGold(id11, goldnumder);//更新实际所得币
                                        new BCW.bydr.BLL.Cmg_Top().UpdateColletGold(id11, goldnumder);//更新本次游戏所得的游戏币
                                    }
                                    else
                                    {
                                        string Logo = ub.GetSub("bydrLogo", xmlPath);
                                        builder.Append(Out.Tab("<div>", ""));
                                        builder.Append("<img src=\"" + Logo + "jiang.gif" + "\" alt=\"load\"/><br />");
                                        builder.Append(Out.Tab("</div><br />", "<br />"));
                                        builder.Append("亲，你太好运了！获得了惊喜大奖，，奖励" + goldnumder + "" + ub.Get("SiteBz") + "，惊喜只能捕鱼一次哦，请重新再选择场景继续抓捕<br />");
                                        new BCW.bydr.BLL.Cmg_Top().UpdateAllcolletGold(id11, goldnumder - 200);//更新实际所得币
                                        new BCW.bydr.BLL.Cmg_Top().UpdateColletGold(id11, goldnumder);//更新本次游戏所得的游戏币                                              
                                    }
                                }
                                else
                                {
                                    string Logo = ub.GetSub("bydrLogo", xmlPath);
                                    builder.Append(Out.Tab("<div>", ""));
                                    builder.Append("<img src=\"" + Logo + "jiang.gif" + "\" alt=\"load\"/><br />");
                                    builder.Append(Out.Tab("</div><br />", "<br />"));
                                    builder.Append("亲，你太好运了！获得了惊喜大奖，，奖励" + goldnumder + "" + ub.Get("SiteBz") + "，惊喜只能捕鱼一次哦，请重新再选择场景继续抓捕<br />");
                                    new BCW.bydr.BLL.Cmg_Top().UpdateAllcolletGold(id11, goldnumder - 200);//更新实际所得币
                                    new BCW.bydr.BLL.Cmg_Top().UpdateColletGold(id11, goldnumder);//更新本次游戏所得的游戏币                     
                                }
                            }
                            else
                            {
                                string Logo = ub.GetSub("bydrLogo", xmlPath);
                                builder.Append(Out.Tab("<div>", ""));
                                builder.Append("<img src=\"" + Logo + "jiang.gif" + "\" alt=\"load\"/><br />");
                                builder.Append(Out.Tab("</div><br />", "<br />"));
                                builder.Append("亲，你太好运了！获得了惊喜大奖，，奖励" + goldnumder + "" + ub.Get("SiteBz") + "，惊喜只能捕鱼一次哦，请重新再选择场景继续抓捕<br />");
                                new BCW.bydr.BLL.Cmg_Top().UpdateAllcolletGold(id11, goldnumder - 200);//更新实际所得币
                                new BCW.bydr.BLL.Cmg_Top().UpdateColletGold(id11, goldnumder);//更新本次游戏所得的游戏币            
                            }
                            //读取Cmg_nots 表中Usid 的最后一条数据的id
                            DataSet rownotesid = new BCW.bydr.BLL.Cmg_notes().GetList("Top 1 ID", "usID=" + meid + "  order by Stime desc ");
                            int maxID = Convert.ToInt32(rownotesid.Tables[0].Rows[0][0]);
                            BCW.bydr.Model.Cmg_notes mo = new BCW.bydr.BLL.Cmg_notes().GetCmg_notes(maxID);
                            if (modle11.DcolletGold == 0)
                            {
                                BCW.bydr.Model.Cmg_notes m1 = new BCW.bydr.Model.Cmg_notes();
                                m1.AllGold = 0;
                                m1.AcolletGold = goldnumder;
                                m1.changj = modle11.Changj;
                                m1.coID = "";
                                m1.cxid = 1;
                                m1.random = 0;
                                m1.Stime = DateTime.Now;
                                m1.Signtime = DateTime.Now;
                                m1.usID = meid;
                                m1.Vit = 0;
                                new BCW.bydr.BLL.Cmg_notes().Add(m1);

                                new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, 10);
                                new BCW.bydr.BLL.CmgToplist().UpdateAllcolletGold(meid, new BCW.bydr.BLL.Cmg_Top().GetCmg_AllcolletGold1(meid));//更新排行榜总收集币                   
                                new BCW.bydr.BLL.CmgToplist().Updatetime(meid, DateTime.Now);
                                new BCW.bydr.BLL.Cmg_Top().updatetime1(id11, DateTime.Now);
                            }
                            builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx") + "\">继续捕鱼</a><br/>");
                            builder.Append(Out.Tab("<br />", ""));
                            builder.Append(Out.Tab("</div>", ""));
                            DUBBPage();
                            #endregion
                        }
                        else
                        {
                            #region 普通奖品派发
                            #region 更新鱼的价格
                            if (modle11.randdaoju == "")//更新随机鱼的价格
                            {
                                string randdaoju = string.Empty;
                                string[] numgoldo = new string[] { };
                                //判断随机给定的场景价格包
                                if (modle11.McolletGold == 0)
                                    numgoldo = numgold0;
                                else if (modle11.McolletGold == 1)
                                    numgoldo = numgold1;
                                else if (modle11.McolletGold == 2)
                                    numgoldo = numgold2;
                                else if (modle11.McolletGold == 3)
                                    numgoldo = numgold3;
                                else if (modle11.McolletGold == 4)
                                    numgoldo = numgold4;
                                else
                                    Utils.Error("选择场景出错！您属于非法操作！请返回游戏主页重新选择该场景", "");
                                int goldnum = 0;
                                try
                                {
                                    goldnum = Convert.ToInt32(numgoldo[Convert.ToInt32(randgoldnum[modle11.randnum/*根据次数读取鱼的价格*/])]);
                                }
                                catch
                                {
                                    builder.Append(".");
                                    goldnum = 200;
                                }
                                if (goldnum == 0)
                                {
                                    //发我内线
                                    new BCW.BLL.Guest().Add(51259, "", "随机次数：" + modle11.randnum + "||随机价格" + randgoldnum[modle11.randnum] + "||鱼的价格：" + goldnum + "当前时间：" + DateTime.Now);
                                    goldnum = 201;
                                }
                                int a = 0;
                                while (a < 10)
                                {
                                    int r = num(modle11.randten, goldnum)[a];//根据随机数的权值计算出每条鱼的价格
                                    randdaoju += Convert.ToString(r);
                                    randdaoju += ",";
                                    a++;
                                }
                                new BCW.bydr.BLL.Cmg_Top().Updateranddaoju(randdaoju, id11);//更新每条鱼的价格
                                new BCW.bydr.BLL.Cmg_Top().UpdateColletGold(id11, goldnum);//更新本次游戏所得的游戏币
                                                                                           //识别会员vip
                                DataSet row1s = new BCW.BLL.User().GetList("VipDate", "ID=" + meid);
                                DateTime viptime = DateTime.Now;
                                try
                                {
                                    viptime = Convert.ToDateTime(row1s.Tables[0].Rows[0][0]);
                                }
                                catch
                                {
                                    viptime = Convert.ToDateTime(bydrBotTime);
                                }
                                if (DateTime.Now < viptime)
                                {
                                    //判断是否为场景1
                                    if (model0.changj2 == 200)
                                    {
                                        int count = 0;
                                        count = new BCW.bydr.BLL.Cmg_Top().GetCmgcount(meid);
                                        string swhere = DateTime.Now.ToString("yyyy-MM-dd");
                                        bool viptime1 = new BCW.bydr.BLL.CmgToplist().Existsusvip(meid, swhere);
                                        if (!viptime1 || count == 1)
                                        {
                                            builder.Append("<b style=\"color:red\"> 您是vip，首次进入" + model0.Changjing + "免费</b><br />");
                                            builder.Append("您的体力-1<br />");
                                            new BCW.bydr.BLL.Cmg_Top().UpdateAllcolletGold(id11, goldnum);//更新实际所得币
                                            new BCW.bydr.BLL.Cmg_Top().UpdateColletGold(id11, goldnum);//更新本次游戏所得的游戏币
                                        }
                                        else
                                        {
                                            new BCW.bydr.BLL.Cmg_Top().UpdateAllcolletGold(id11, goldnum - model0.changj2);//更新实际所得币
                                            new BCW.bydr.BLL.Cmg_Top().UpdateColletGold(id11, goldnum);//更新本次游戏所得的游戏币                          
                                            builder.Append("您的体力-1" + "您的" + ub.Get("SiteBz") + "-" + model0.changj2 + "<br />");
                                        }
                                    }
                                    else
                                    {
                                        new BCW.bydr.BLL.Cmg_Top().UpdateAllcolletGold(id11, goldnum - model0.changj2);//更新实际所得币
                                        new BCW.bydr.BLL.Cmg_Top().UpdateColletGold(id11, goldnum);//更新本次游戏所得的游戏币
                                        builder.Append("您的体力-1" + "您的" + ub.Get("SiteBz") + "-" + model0.changj2 + "<br />");
                                    }
                                }
                                else
                                {
                                    new BCW.bydr.BLL.Cmg_Top().UpdateAllcolletGold(id11, goldnum - model0.changj2);//更新实际所得币
                                    new BCW.bydr.BLL.Cmg_Top().UpdateColletGold(id11, goldnum);//更新本次游戏所得的游戏币
                                    builder.Append("您的体力-1" + "您的" + ub.Get("SiteBz") + "-" + model0.changj2 + "<br />");
                                }
                                new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, modle11.DcolletGold);
                                builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx?act=buyu&amp;id=1buyu&amp;ptype=" + ptype) + "\">继续捕鱼</a><br/>");
                                builder.Append(Out.Tab("</div>", ""));
                                DUBBPage();
                            }
                            #endregion
                            else
                            {

                                if (modle11.DcolletGold < 10)
                                {
                                    #region 判断第9次加数据
                                    if (modle11.DcolletGold == 9)
                                    {
                                        new BCW.bydr.BLL.CmgToplist().UpdateAllcolletGold(meid, new BCW.bydr.BLL.Cmg_Top().GetCmg_AllcolletGold1(meid));//更新排行榜总收集币                      
                                        new BCW.bydr.BLL.CmgToplist().Updatetime(meid, DateTime.Now);
                                        //new BCW.bydr.BLL.Cmg_Top().updatetime1(id11, DateTime.Now);
                                    }
                                    #endregion
                                    string rand = "";
                                    string rand1 = "";
                                    #region 根据价格取不同的鱼类                  
                                    if (Convert.ToInt32(randaoju[modle11.DcolletGold]) < 20)
                                    {
                                        int r = 0;
                                        r = R(0, daojunum1.Length - 1);
                                        rand = Convert.ToString(daojunum1[r]);
                                        rand1 += modle11.randyuID + rand + ",";
                                        new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);
                                    }
                                    else if (Convert.ToInt32(randaoju[modle11.DcolletGold]) >= 20 && Convert.ToInt32(randaoju[modle11.DcolletGold]) < 100)
                                    {
                                        int r = 0;
                                        r = R(0, daojunum2.Length - 1);
                                        rand = Convert.ToString(daojunum2[r]);
                                        rand1 += modle11.randyuID + rand + ",";
                                        new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);
                                    }
                                    else
                                    {
                                        int r = 0;
                                        r = R(0, daojunum3.Length - 1);
                                        rand = Convert.ToString(daojunum3[r]);
                                        rand1 += modle11.randyuID + rand + ",";
                                        new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);
                                    }
                                    #endregion
                                    if (modle11.YcolletGold == 1)
                                    {
                                        if (((DateTime.Now.AddSeconds(10) - modle11.updatetime.AddSeconds(10)).TotalSeconds > time))
                                        {
                                            new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, modle11.DcolletGold + 1);
                                            new BCW.bydr.BLL.Cmg_Top().updatetime(id11, DateTime.Now);
                                            new BCW.bydr.BLL.Cmg_Top().UpdateYcolletGold(id11, 0);
                                            Utils.Success("", "捕鱼成功！", Utils.getUrl("cmg.aspx?act=by"), "1");
                                        }
                                        else
                                        {
                                            new BCW.bydr.BLL.Cmg_Top().UpdateYcolletGold(id11, 0);
                                        }
                                    }
                                    string yuju = Convert.ToString(ub.GetSub("bydryuju", "/Controls/BYDR.xml"));
                                    string[] yu = yuju.Split('#');
                                    int n1 = R(0, yu.Length);
                                    builder.Append(yu[n1] + "<br />");
                                    builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx?act=by&amp;ptype=" + ptype + "") + "\">继续抓捕</a><br />");

                                    new BCW.bydr.BLL.Cmg_Top().updatetime(id11, DateTime.Now);                                                                           //是否刷屏
                                    string we = "";
                                    int Expir = Utils.ParseInt(ub.GetSub("bydrExpir", xmlPath));
                                    BCW.User.Users.IsFresh(we, Expir);
                                    builder.Append(Out.Tab("</div>", ""));
                                    DUBBPage();
                                }
                                else
                                {
                                    builder.Append(Out.Tab("<div>", ""));
                                    builder.Append("本场次数已捕完。继续选择场景捕鱼<br />");
                                    builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx?") + "\">继续抓捕</a><br />");
                                    builder.Append(Out.Tab("</div>", ""));
                                    DUBBPage();
                                }
                            }
                            #endregion
                        }
                    }; break;
                #endregion
                #region 场景2操作
                case 1:
                    {
                        BCW.bydr.Model.CmgDaoju modeldaoju5 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(305);
                        string[] numgold5 = modeldaoju5.Changjing.Split(',');
                        BCW.bydr.Model.CmgDaoju modeldaoju6 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(306);
                        string[] numgold6 = modeldaoju6.Changjing.Split(',');
                        BCW.bydr.Model.CmgDaoju modeldaoju7 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(307);
                        string[] numgold7 = modeldaoju7.Changjing.Split(',');
                        BCW.bydr.Model.CmgDaoju modeldaoju8 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(308);
                        string[] numgold8 = modeldaoju8.Changjing.Split(',');
                        BCW.bydr.Model.CmgDaoju modeldaoju9 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(309);
                        string[] numgold9 = modeldaoju9.Changjing.Split(',');
                        if (randgold == 0)
                        {
                            if (modle11.McolletGold == 5)
                                numgoldf = numgold5;
                            else if (modle11.McolletGold == 6)
                                numgoldf = numgold6;
                            else if (modle11.McolletGold == 7)
                                numgoldf = numgold7;
                            else if (modle11.McolletGold == 8)
                                numgoldf = numgold8;
                            else if (modle11.McolletGold == 9)
                                numgoldf = numgold9;
                            else
                                Utils.Error("选择场景出错！您属于非法操作！请返回游戏主页重新选择该场景", "");
                            int goldnumder = Convert.ToInt32(numgold5[0]);
                            string Logo = ub.GetSub("bydrLogo", xmlPath);
                            builder.Append(Out.Tab("<div>", ""));
                            builder.Append("<img src=\"" + Logo + "jiang.gif" + "\" alt=\"load\"/><br />");
                            builder.Append(Out.Tab("</div><br />", "<br />"));
                            builder.Append("亲，你太好运了！获得了惊喜大奖，，奖励" + goldnumder + "" + ub.Get("SiteBz") + "，惊喜只能捕鱼一次哦，请重新再选择场景继续抓捕<br />");
                            if (modle11.DcolletGold == 0)
                            {
                                BCW.bydr.Model.Cmg_notes m1 = new BCW.bydr.Model.Cmg_notes();
                                m1.AllGold = 0;
                                m1.AcolletGold = goldnumder;
                                m1.changj = modle11.Changj;
                                m1.coID = "";
                                m1.cxid = 1;
                                m1.random = 0;
                                m1.Stime = DateTime.Now;
                                m1.Signtime = DateTime.Now;
                                m1.usID = meid;
                                m1.Vit = 0;
                                new BCW.bydr.BLL.Cmg_notes().Add(m1);
                                new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, 10);
                                new BCW.bydr.BLL.CmgToplist().UpdateAllcolletGold(meid, new BCW.bydr.BLL.Cmg_Top().GetCmg_AllcolletGold1(meid));//更新排行榜总收集币                 
                                new BCW.bydr.BLL.CmgToplist().Updatetime(meid, DateTime.Now);
                                new BCW.bydr.BLL.Cmg_Top().updatetime1(id11, DateTime.Now);
                                new BCW.bydr.BLL.Cmg_Top().UpdateAllcolletGold(id11, goldnumder - 1000);//更新实际所得币
                                new BCW.bydr.BLL.Cmg_Top().UpdateColletGold(id11, goldnumder);//更新本次游戏所得的游戏币
                            }
                            builder.Append("本场次不用捕鱼<br />");
                            builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx") + "\">继续捕鱼</a><br/>");
                            builder.Append(Out.Tab("<br />", ""));
                            builder.Append(Out.Tab("</div>", ""));
                            DUBBPage();
                        }
                        else
                        {
                            if (modle11.randdaoju == "")//更新随机鱼的价格
                            {
                                int a = 0;
                                string randdaoju = string.Empty;
                                Random ran = new Random();
                                if (modle11.McolletGold == 5)
                                    numgoldf = numgold5;
                                else if (modle11.McolletGold == 6)
                                    numgoldf = numgold6;
                                else if (modle11.McolletGold == 7)
                                    numgoldf = numgold7;
                                else if (modle11.McolletGold == 8)
                                    numgoldf = numgold8;
                                else if (modle11.McolletGold == 9)
                                    numgoldf = numgold9;
                                else
                                    Utils.Error("选择场景出错！您属于非法操作！请返回游戏主页重新选择该场景", "");
                                int goldnum = 0;
                                try
                                {
                                    goldnum = Convert.ToInt32(numgoldf[Convert.ToInt32(randgoldnum[modle11.randnum/*根据次数读取鱼的价格*/])]);
                                }
                                catch
                                {
                                    builder.Append(".");
                                    goldnum = 1000;
                                }
                                if (goldnum == 0)
                                    goldnum = 1000;
                                while (a < 10)
                                {
                                    int r = num(modle11.randten, goldnum)[a];//根据随机数的权值计算出每条鱼的价格
                                    randdaoju += Convert.ToString(r);
                                    randdaoju += ",";
                                    a++;
                                }
                                new BCW.bydr.BLL.Cmg_Top().Updateranddaoju(randdaoju, id11);
                                new BCW.bydr.BLL.Cmg_Top().UpdateColletGold(id11, goldnum);//更新本次游戏所得的游戏币
                                new BCW.bydr.BLL.Cmg_Top().UpdateAllcolletGold(id11, goldnum - model0.changj2);//更新实际所得币
                                builder.Append("您的体力-1" + "您的" + ub.Get("SiteBz") + "-" + model0.changj2 + "<br />");
                                new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, modle11.DcolletGold);
                                builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx?act=buyu&amp;id=1&amp;ptype=" + ptype) + "\">继续捕鱼</a><br/>");
                                builder.Append(Out.Tab("<br />", ""));
                                builder.Append(Out.Tab("</div>", ""));
                                DUBBPage();
                            }
                            else
                            {
                                if (modle11.DcolletGold < 10)
                                {
                                    if (modle11.DcolletGold == 9)
                                    {
                                        new BCW.bydr.BLL.CmgToplist().UpdateAllcolletGold(meid, new BCW.bydr.BLL.Cmg_Top().GetCmg_AllcolletGold1(meid)); //更新排行榜总收集币
                                        new BCW.bydr.BLL.CmgToplist().Updatetime(meid, DateTime.Now);
                                        // new BCW.bydr.BLL.Cmg_Top().updatetime1(id11, DateTime.Now);
                                    }
                                    string rand = "";
                                    string rand1 = "";
                                    //根据价格取不同的鱼类
                                    if (Convert.ToInt32(randaoju[modle11.DcolletGold]) < 100)
                                    {
                                        int r = 0;
                                        r = R(0, daojunum1.Length - 1);
                                        rand = Convert.ToString(daojunum1[r]);
                                        rand1 += modle11.randyuID + rand + ",";
                                        new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);
                                    }
                                    else if (Convert.ToInt32(randaoju[modle11.DcolletGold]) >= 100 && Convert.ToInt32(randaoju[modle11.DcolletGold]) < 10000)
                                    {
                                        int r = 0;
                                        r = R(0, daojunum2.Length - 1);
                                        rand = Convert.ToString(daojunum2[r]);
                                        rand1 += modle11.randyuID + rand + ",";
                                        new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);
                                    }
                                    else
                                    {
                                        int r = 0;
                                        r = R(0, daojunum3.Length - 1);
                                        rand = Convert.ToString(daojunum3[r]);
                                        rand1 += modle11.randyuID + rand + ",";
                                        new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);
                                    }
                                    if (modle11.YcolletGold == 1)
                                    {

                                        if (((DateTime.Now.AddSeconds(10) - modle11.updatetime.AddSeconds(10)).TotalSeconds > time))
                                        {
                                            new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, modle11.DcolletGold + 1);
                                            new BCW.bydr.BLL.Cmg_Top().updatetime(id11, DateTime.Now);
                                            new BCW.bydr.BLL.Cmg_Top().UpdateYcolletGold(id11, 0);
                                            Utils.Success("", "捕鱼成功！", Utils.getUrl("cmg.aspx?act=by"), "1");
                                        }
                                        else
                                        {
                                            new BCW.bydr.BLL.Cmg_Top().UpdateYcolletGold(id11, 0);
                                        }
                                    }
                                    string yuju = Convert.ToString(ub.GetSub("bydryuju", "/Controls/BYDR.xml"));
                                    string[] yu = yuju.Split('#');
                                    int n1 = R(0, yu.Length);
                                    builder.Append(yu[n1] + "<br />");
                                    builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx?act=by&amp;ptype=" + ptype + "") + "\">继续抓捕</a><br />");

                                    new BCW.bydr.BLL.Cmg_Top().updatetime(id11, DateTime.Now);//更新防刷时间 
                                                                                              //是否刷屏
                                    string we = "";
                                    int Expir = Utils.ParseInt(ub.GetSub("bydrExpir", xmlPath));
                                    BCW.User.Users.IsFresh(we, Expir);
                                    builder.Append(Out.Tab("</div>", ""));
                                    DUBBPage();
                                }
                                else
                                {
                                    builder.Append(Out.Tab("<div>", ""));
                                    builder.Append("本场次数已捕完。继续选择场景捕鱼<br />");
                                    builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx?") + "\">继续抓捕</a><br />");
                                    builder.Append(Out.Tab("</div>", ""));

                                    DUBBPage();
                                }
                            }
                        }

                    }; break;
                #endregion
                #region 场景3操作
                case 2:
                    {

                        BCW.bydr.Model.CmgDaoju modeldaoju10 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(310);
                        string[] numgold10 = modeldaoju10.Changjing.Split(',');
                        BCW.bydr.Model.CmgDaoju modeldaoju11 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(311);
                        string[] numgold11 = modeldaoju11.Changjing.Split(',');
                        BCW.bydr.Model.CmgDaoju modeldaoju12 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(312);
                        string[] numgold12 = modeldaoju12.Changjing.Split(',');
                        BCW.bydr.Model.CmgDaoju modeldaoju13 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(313);
                        string[] numgold13 = modeldaoju13.Changjing.Split(',');
                        BCW.bydr.Model.CmgDaoju modeldaoju14 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(314);
                        string[] numgold14 = modeldaoju14.Changjing.Split(',');
                        if (randgold == 0)
                        {
                            if (modle11.McolletGold == 10)
                                numgoldf = numgold10;
                            else if (modle11.McolletGold == 11)
                                numgoldf = numgold11;
                            else if (modle11.McolletGold == 12)
                                numgoldf = numgold12;
                            else if (modle11.McolletGold == 13)
                                numgoldf = numgold13;
                            else if (modle11.McolletGold == 14)
                                numgoldf = numgold14;
                            else
                                Utils.Error("选择场景出错！您属于非法操作！请返回游戏主页重新选择该场景", "");
                            int goldnumder = Convert.ToInt32(numgold10[0]);
                            string Logo = ub.GetSub("bydrLogo", xmlPath);
                            builder.Append(Out.Tab("<div>", ""));
                            builder.Append("<img src=\"" + Logo + "jiang.gif" + "\" alt=\"load\"/><br />");
                            builder.Append(Out.Tab("</div><br />", "<br />"));

                            builder.Append("亲，你太好运了！获得了惊喜大奖，，奖励" + goldnumder + "" + ub.Get("SiteBz") + "，惊喜只能捕鱼一次哦，请重新再选择场景继续抓捕<br />");
                            if (modle11.DcolletGold == 0)
                            {
                                BCW.bydr.Model.Cmg_notes m1 = new BCW.bydr.Model.Cmg_notes();
                                m1.AllGold = 0;
                                m1.AcolletGold = goldnumder;
                                m1.changj = modle11.Changj;
                                m1.coID = "";
                                m1.cxid = 1;
                                m1.random = 0;
                                m1.Stime = DateTime.Now;
                                m1.Signtime = DateTime.Now;
                                m1.usID = meid;
                                m1.Vit = 0;
                                new BCW.bydr.BLL.Cmg_notes().Add(m1);

                                new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, 10);
                                new BCW.bydr.BLL.CmgToplist().UpdateAllcolletGold(meid, new BCW.bydr.BLL.Cmg_Top().GetCmg_AllcolletGold1(meid));//更新排行榜总收集币                   
                                new BCW.bydr.BLL.CmgToplist().Updatetime(meid, DateTime.Now);
                                new BCW.bydr.BLL.Cmg_Top().updatetime1(id11, DateTime.Now);
                                new BCW.bydr.BLL.Cmg_Top().UpdateAllcolletGold(id11, goldnumder - 5000);//更新实际所得币
                                new BCW.bydr.BLL.Cmg_Top().UpdateColletGold(id11, goldnumder);//更新本次游戏所得的游戏币

                            }
                            builder.Append("本场次不用捕鱼<br />");
                            builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx") + "\">继续捕鱼</a><br/>");
                            builder.Append(Out.Tab("<br />", ""));
                            builder.Append(Out.Tab("</div>", ""));
                            DUBBPage();
                        }
                        else
                        {
                            if (modle11.randdaoju == "")//更新随机鱼的价格
                            {
                                int a = 0;
                                string randdaoju = string.Empty;

                                Random ran = new Random();

                                if (modle11.McolletGold == 10)
                                    numgoldf = numgold10;
                                else if (modle11.McolletGold == 11)
                                    numgoldf = numgold11;
                                else if (modle11.McolletGold == 12)
                                    numgoldf = numgold12;
                                else if (modle11.McolletGold == 13)
                                    numgoldf = numgold13;
                                else if (modle11.McolletGold == 14)
                                    numgoldf = numgold14;
                                else
                                    Utils.Error("选择场景出错！您属于非法操作！请返回游戏主页重新选择该场景", "");
                                int goldnum = 0;
                                try
                                {
                                    goldnum = Convert.ToInt32(numgoldf[Convert.ToInt32(randgoldnum[modle11.randnum/*根据次数读取鱼的价格*/])]);
                                }
                                catch
                                {
                                    builder.Append(".");
                                    goldnum = 5000;
                                }
                                if (goldnum == 0)
                                    goldnum = 5000;
                                while (a < 10)
                                {
                                    int r = num(modle11.randten, goldnum)[a];//根据随机数的权值计算出每条鱼的价格
                                    randdaoju += Convert.ToString(r);
                                    randdaoju += ",";
                                    a++;
                                }
                                new BCW.bydr.BLL.Cmg_Top().Updateranddaoju(randdaoju, id11);
                                new BCW.bydr.BLL.Cmg_Top().UpdateColletGold(id11, goldnum);//更新本次游戏所得的游戏币
                                new BCW.bydr.BLL.Cmg_Top().UpdateAllcolletGold(id11, goldnum - model0.changj2);//更新实际所得币
                                builder.Append("您的体力-1" + "您的" + ub.Get("SiteBz") + "-" + model0.changj2 + "<br />");
                                new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, modle11.DcolletGold);
                                builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx?act=buyu&amp;id=1&amp;ptype=" + ptype) + "\">继续捕鱼</a><br/>");
                                builder.Append(Out.Tab("<br />", ""));
                                builder.Append(Out.Tab("</div>", ""));
                                DUBBPage();
                            }
                            else
                            {
                                if (modle11.DcolletGold < 10)
                                {
                                    if (modle11.DcolletGold == 9)
                                    {
                                        new BCW.bydr.BLL.CmgToplist().UpdateAllcolletGold(meid, new BCW.bydr.BLL.Cmg_Top().GetCmg_AllcolletGold1(meid));//更新排行榜总收集币
                                        new BCW.bydr.BLL.CmgToplist().Updatetime(meid, DateTime.Now);
                                        //   new BCW.bydr.BLL.Cmg_Top().updatetime1(id11, DateTime.Now);
                                    }
                                    string rand = "";
                                    string rand1 = "";
                                    //根据价格取不同的鱼类
                                    if (Convert.ToInt32(randaoju[modle11.DcolletGold]) < 200)
                                    {
                                        int r = 0;
                                        r = R(0, daojunum1.Length - 1);
                                        rand = Convert.ToString(daojunum1[r]);
                                        rand1 += modle11.randyuID + rand + ",";
                                        new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);

                                    }
                                    else if (Convert.ToInt32(randaoju[modle11.DcolletGold]) >= 200 && Convert.ToInt32(randaoju[modle11.DcolletGold]) < 500)
                                    {
                                        int r = 0;
                                        r = R(0, daojunum2.Length - 1);
                                        rand = Convert.ToString(daojunum2[r]);
                                        rand1 += modle11.randyuID + rand + ",";
                                        new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);

                                    }
                                    else
                                    {
                                        int r = 0;
                                        r = R(0, daojunum3.Length - 1);
                                        rand = Convert.ToString(daojunum3[r]);
                                        rand1 += modle11.randyuID + rand + ",";
                                        new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);

                                    }
                                    if (modle11.YcolletGold == 1)
                                    {

                                        if (((DateTime.Now.AddSeconds(10) - modle11.updatetime.AddSeconds(10)).TotalSeconds > time))
                                        {
                                            new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, modle11.DcolletGold + 1);
                                            new BCW.bydr.BLL.Cmg_Top().updatetime(id11, DateTime.Now);
                                            new BCW.bydr.BLL.Cmg_Top().UpdateYcolletGold(id11, 0);
                                            Utils.Success("", "捕鱼成功！", Utils.getUrl("cmg.aspx?act=by"), "1");
                                        }
                                        else
                                        {
                                            new BCW.bydr.BLL.Cmg_Top().UpdateYcolletGold(id11, 0);
                                        }
                                    }
                                    string yuju = Convert.ToString(ub.GetSub("bydryuju", "/Controls/BYDR.xml"));
                                    string[] yu = yuju.Split('#');
                                    int n1 = R(0, yu.Length);
                                    builder.Append(yu[n1] + "<br />");
                                    builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx?act=by&amp;ptype=" + ptype + "") + "\">继续抓捕</a><br />");

                                    new BCW.bydr.BLL.Cmg_Top().updatetime(id11, DateTime.Now);//更新防刷时间 
                                                                                              //是否刷屏
                                    string we = "";
                                    int Expir = Utils.ParseInt(ub.GetSub("bydrExpir", xmlPath));
                                    BCW.User.Users.IsFresh(we, Expir);
                                    builder.Append(Out.Tab("</div>", ""));
                                    DUBBPage();

                                }
                                else
                                {
                                    builder.Append(Out.Tab("<div>", ""));
                                    builder.Append("本场次数已捕完。继续选择场景捕鱼<br />");
                                    builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx?") + "\">继续抓捕</a><br />");
                                    builder.Append(Out.Tab("</div>", ""));

                                    DUBBPage();
                                }
                            }
                        }
                    }; break;
                #endregion
                #region 场景4操作
                case 3:
                    {
                        BCW.bydr.Model.CmgDaoju modeldaoju15 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(315);
                        string[] numgold15 = modeldaoju15.Changjing.Split(',');
                        BCW.bydr.Model.CmgDaoju modeldaoju16 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(316);
                        string[] numgold16 = modeldaoju16.Changjing.Split(',');
                        BCW.bydr.Model.CmgDaoju modeldaoju17 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(317);
                        string[] numgold17 = modeldaoju17.Changjing.Split(',');
                        BCW.bydr.Model.CmgDaoju modeldaoju18 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(318);
                        string[] numgold18 = modeldaoju18.Changjing.Split(',');
                        BCW.bydr.Model.CmgDaoju modeldaoju19 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(319);
                        string[] numgold19 = modeldaoju19.Changjing.Split(',');
                        if (randgold == 0)
                        {
                            if (modle11.McolletGold == 15)
                                numgoldf = numgold15;
                            else if (modle11.McolletGold == 16)
                                numgoldf = numgold16;
                            else if (modle11.McolletGold == 17)
                                numgoldf = numgold17;
                            else if (modle11.McolletGold == 18)
                                numgoldf = numgold18;
                            else if (modle11.McolletGold == 19)
                                numgoldf = numgold19;
                            else
                                Utils.Error("选择场景出错！您属于非法操作！请返回游戏主页重新选择该场景", "");
                            int goldnumder = Convert.ToInt32(numgold15[0]);
                            string Logo = ub.GetSub("bydrLogo", xmlPath);
                            builder.Append(Out.Tab("<div>", ""));
                            builder.Append("<img src=\"" + Logo + "jiang.gif" + "\" alt=\"load\"/><br />");
                            builder.Append(Out.Tab("</div><br />", "<br />"));
                            builder.Append("亲，你太好运了！获得了惊喜大奖，，奖励" + goldnumder + "" + ub.Get("SiteBz") + "，惊喜只能捕鱼一次哦，请重新再选择场景继续抓捕<br />");
                            if (modle11.DcolletGold == 0)
                            {
                                BCW.bydr.Model.Cmg_notes m1 = new BCW.bydr.Model.Cmg_notes();
                                m1.AllGold = 0;
                                m1.AcolletGold = goldnumder;
                                m1.changj = modle11.Changj;
                                m1.coID = "";
                                m1.cxid = 1;
                                m1.random = 0;
                                m1.Stime = DateTime.Now;
                                m1.Signtime = DateTime.Now;
                                m1.usID = meid;
                                m1.Vit = 0;
                                new BCW.bydr.BLL.Cmg_notes().Add(m1);

                                new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, 10);
                                new BCW.bydr.BLL.CmgToplist().UpdateAllcolletGold(meid, new BCW.bydr.BLL.Cmg_Top().GetCmg_AllcolletGold1(meid));//更新排行榜总收集币                            
                                new BCW.bydr.BLL.CmgToplist().Updatetime(meid, DateTime.Now);
                                new BCW.bydr.BLL.Cmg_Top().updatetime1(id11, DateTime.Now);
                                new BCW.bydr.BLL.Cmg_Top().UpdateAllcolletGold(id11, goldnumder - 10000);//更新实际所得币
                                new BCW.bydr.BLL.Cmg_Top().UpdateColletGold(id11, goldnumder);//更新本次游戏所得的游戏币
                            }
                            builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx") + "\">继续捕鱼</a><br/>");
                            builder.Append(Out.Tab("<br />", ""));
                            builder.Append(Out.Tab("</div>", ""));
                            DUBBPage();
                        }
                        else
                        {
                            if (modle11.randdaoju == "")//更新随机鱼的价格
                            {
                                int a = 0;
                                string randdaoju = string.Empty;

                                Random ran = new Random();

                                if (modle11.McolletGold == 15)
                                    numgoldf = numgold15;
                                else if (modle11.McolletGold == 16)
                                    numgoldf = numgold16;
                                else if (modle11.McolletGold == 17)
                                    numgoldf = numgold17;
                                else if (modle11.McolletGold == 18)
                                    numgoldf = numgold18;
                                else if (modle11.McolletGold == 19)
                                    numgoldf = numgold19;
                                else
                                    Utils.Error("选择场景出错！您属于非法操作！请返回游戏主页重新选择该场景", "");
                                int goldnum = 0;
                                try
                                {
                                    goldnum = Convert.ToInt32(numgoldf[Convert.ToInt32(randgoldnum[modle11.randnum/*根据次数读取鱼的价格*/])]);
                                }
                                catch
                                {
                                    builder.Append(".");
                                    goldnum = 10000;
                                }
                                if (goldnum == 0)
                                    goldnum = 10000;
                                while (a < 10)
                                {
                                    int r = num(modle11.randten, goldnum)[a];//根据随机数的权值计算出每条鱼的价格
                                    randdaoju += Convert.ToString(r);
                                    randdaoju += ",";
                                    a++;
                                }
                                new BCW.bydr.BLL.Cmg_Top().Updateranddaoju(randdaoju, id11);
                                new BCW.bydr.BLL.Cmg_Top().UpdateColletGold(id11, goldnum);//更新本次游戏所得的游戏币
                                new BCW.bydr.BLL.Cmg_Top().UpdateAllcolletGold(id11, goldnum - model0.changj2);//更新实际所得币
                                builder.Append("您的体力-1" + "您的" + ub.Get("SiteBz") + "-" + model0.changj2 + "<br />");
                                new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, modle11.DcolletGold);
                                builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx?act=buyu&amp;id=1&amp;ptype=" + ptype) + "\">继续捕鱼</a><br/>");
                                builder.Append(Out.Tab("<br />", ""));
                                builder.Append(Out.Tab("</div>", ""));
                                DUBBPage();
                            }
                            else
                            {
                                if (modle11.DcolletGold < 10)
                                {
                                    if (modle11.DcolletGold == 9)
                                    {
                                        new BCW.bydr.BLL.CmgToplist().UpdateAllcolletGold(meid, new BCW.bydr.BLL.Cmg_Top().GetCmg_AllcolletGold1(meid));//更新排行榜总收集币
                                        new BCW.bydr.BLL.CmgToplist().Updatetime(meid, DateTime.Now);
                                        //  new BCW.bydr.BLL.Cmg_Top().updatetime1(id11, DateTime.Now);
                                    }
                                    string rand = "";
                                    string rand1 = "";
                                    //根据价格取不同的鱼类
                                    if (Convert.ToInt32(randaoju[modle11.DcolletGold]) < 200)
                                    {
                                        int r = 0;
                                        r = R(0, daojunum1.Length - 1);
                                        rand = Convert.ToString(daojunum1[r]);
                                        rand1 += modle11.randyuID + rand + ",";
                                        new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);

                                    }
                                    else if (Convert.ToInt32(randaoju[modle11.DcolletGold]) >= 200 && Convert.ToInt32(randaoju[modle11.DcolletGold]) < 500)
                                    {
                                        int r = 0;
                                        r = R(0, daojunum2.Length - 1);
                                        rand = Convert.ToString(daojunum2[r]);
                                        rand1 += modle11.randyuID + rand + ",";
                                        new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);

                                    }
                                    else
                                    {
                                        int r = 0;
                                        r = R(0, daojunum3.Length - 1);
                                        rand = Convert.ToString(daojunum3[r]);
                                        rand1 += modle11.randyuID + rand + ",";
                                        new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);

                                    }
                                    if (modle11.YcolletGold == 1)
                                    {

                                        if (((DateTime.Now.AddSeconds(10) - modle11.updatetime.AddSeconds(10)).TotalSeconds > time))
                                        {
                                            new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, modle11.DcolletGold + 1);
                                            new BCW.bydr.BLL.Cmg_Top().updatetime(id11, DateTime.Now);
                                            new BCW.bydr.BLL.Cmg_Top().UpdateYcolletGold(id11, 0);
                                            Utils.Success("", "捕鱼成功！", Utils.getUrl("cmg.aspx?act=by"), "1");
                                        }
                                        else
                                        {
                                            new BCW.bydr.BLL.Cmg_Top().UpdateYcolletGold(id11, 0);
                                        }
                                    }
                                    string yuju = Convert.ToString(ub.GetSub("bydryuju", "/Controls/BYDR.xml"));
                                    string[] yu = yuju.Split('#');
                                    int n1 = R(0, yu.Length);
                                    builder.Append(yu[n1] + "<br />");
                                    builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx?act=by&amp;ptype=" + ptype + "") + "\">继续抓捕</a><br />");

                                    new BCW.bydr.BLL.Cmg_Top().updatetime(id11, DateTime.Now);//更新防刷时间   
                                                                                              //是否刷屏
                                    string we = "";
                                    int Expir = Utils.ParseInt(ub.GetSub("bydrExpir", xmlPath));
                                    BCW.User.Users.IsFresh(we, Expir);
                                    builder.Append(Out.Tab("</div>", ""));
                                    DUBBPage();

                                }
                                else
                                {

                                    builder.Append(Out.Tab("<div>", ""));
                                    builder.Append("本场次数已捕完。继续选择场景捕鱼<br />");
                                    builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx?") + "\">继续抓捕</a><br />");
                                    builder.Append(Out.Tab("</div>", ""));

                                    DUBBPage();
                                }
                            }
                        }
                    }; break;
                #endregion
                #region 场景5操作
                case 4:
                    {
                        BCW.bydr.Model.CmgDaoju modeldaoju20 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(320);
                        string[] numgold20 = modeldaoju20.Changjing.Split(',');
                        BCW.bydr.Model.CmgDaoju modeldaoju21 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(321);
                        string[] numgold21 = modeldaoju21.Changjing.Split(',');
                        BCW.bydr.Model.CmgDaoju modeldaoju22 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(322);
                        string[] numgold22 = modeldaoju22.Changjing.Split(',');
                        BCW.bydr.Model.CmgDaoju modeldaoju23 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(323);
                        string[] numgold23 = modeldaoju23.Changjing.Split(',');
                        BCW.bydr.Model.CmgDaoju modeldaoju24 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(324);
                        string[] numgold24 = modeldaoju24.Changjing.Split(',');
                        if (randgold == 0)
                        {
                            if (modle11.McolletGold == 20)
                                numgoldf = numgold20;
                            else if (modle11.McolletGold == 21)
                                numgoldf = numgold21;
                            else if (modle11.McolletGold == 22)
                                numgoldf = numgold22;
                            else if (modle11.McolletGold == 23)
                                numgoldf = numgold23;
                            else if (modle11.McolletGold == 24)
                                numgoldf = numgold24;
                            else
                                Utils.Error("选择场景出错！您属于非法操作！请返回游戏主页重新选择该场景", "");
                            int goldnumder = Convert.ToInt32(numgold20[0]);
                            string Logo = ub.GetSub("bydrLogo", xmlPath);
                            builder.Append(Out.Tab("<div>", ""));
                            builder.Append("<img src=\"" + Logo + "jiang.gif" + "\" alt=\"load\"/><br />");
                            builder.Append(Out.Tab("</div><br />", "<br />"));
                            builder.Append("亲，你太好运了！获得了惊喜大奖，，奖励" + goldnumder + "" + ub.Get("SiteBz") + "，惊喜只能捕鱼一次哦，请重新再选择场景继续抓捕<br />");
                            if (modle11.DcolletGold == 0)
                            {
                                BCW.bydr.Model.Cmg_notes m1 = new BCW.bydr.Model.Cmg_notes();
                                m1.AllGold = 0;
                                m1.AcolletGold = goldnumder;
                                m1.changj = modle11.Changj;
                                m1.coID = "";
                                m1.cxid = 1;
                                m1.random = 0;
                                m1.Stime = DateTime.Now;
                                m1.Signtime = DateTime.Now;
                                m1.usID = meid;
                                m1.Vit = 0;
                                new BCW.bydr.BLL.Cmg_notes().Add(m1);

                                new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, 10);
                                new BCW.bydr.BLL.CmgToplist().UpdateAllcolletGold(meid, new BCW.bydr.BLL.Cmg_Top().GetCmg_AllcolletGold1(meid));//更新排行榜总收集币                     
                                new BCW.bydr.BLL.CmgToplist().Updatetime(meid, DateTime.Now);
                                new BCW.bydr.BLL.Cmg_Top().updatetime1(id11, DateTime.Now);
                                new BCW.bydr.BLL.Cmg_Top().UpdateAllcolletGold(id11, goldnumder - 20000);//更新实际所得币
                                new BCW.bydr.BLL.Cmg_Top().UpdateColletGold(id11, goldnumder);//更新本次游戏所得的游戏币
                            }
                            builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx") + "\">继续捕鱼</a><br/>");
                            builder.Append(Out.Tab("<br />", ""));
                            builder.Append(Out.Tab("</div>", ""));
                            DUBBPage();
                        }
                        else
                        {
                            if (modle11.randdaoju == "")//更新随机鱼的价格
                            {
                                int a = 0;
                                string randdaoju = string.Empty;

                                Random ran = new Random();

                                if (modle11.McolletGold == 20)
                                    numgoldf = numgold20;
                                else if (modle11.McolletGold == 21)
                                    numgoldf = numgold21;
                                else if (modle11.McolletGold == 22)
                                    numgoldf = numgold22;
                                else if (modle11.McolletGold == 23)
                                    numgoldf = numgold23;
                                else if (modle11.McolletGold == 24)
                                    numgoldf = numgold24;
                                else
                                    Utils.Error("选择场景出错！您属于非法操作！请返回游戏主页重新选择该场景", "");
                                int goldnum = 0;
                                try
                                {
                                    goldnum = Convert.ToInt32(numgoldf[Convert.ToInt32(randgoldnum[modle11.randnum/*根据次数读取鱼的价格*/])]);
                                }
                                catch
                                {
                                    builder.Append(".");
                                    goldnum = 20000;
                                }
                                if (goldnum == 0)
                                    goldnum = 20000;
                                while (a < 10)
                                {
                                    int r = num(modle11.randten, goldnum)[a];//根据随机数的权值计算出每条鱼的价格
                                    randdaoju += Convert.ToString(r);
                                    randdaoju += ",";
                                    a++;
                                }
                                new BCW.bydr.BLL.Cmg_Top().Updateranddaoju(randdaoju, id11);
                                new BCW.bydr.BLL.Cmg_Top().UpdateColletGold(id11, goldnum);//更新本次游戏所得的游戏币
                                new BCW.bydr.BLL.Cmg_Top().UpdateAllcolletGold(id11, goldnum - model0.changj2);//更新实际所得币
                                builder.Append("您的体力-1" + "您的" + ub.Get("SiteBz") + "-" + model0.changj2 + "<br />");
                                new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, modle11.DcolletGold);
                                builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx?act=buyu&amp;id=1&amp;ptype=" + ptype) + "\">继续捕鱼</a><br/>");
                                builder.Append(Out.Tab("<br />", ""));
                                builder.Append(Out.Tab("</div>", ""));
                                DUBBPage();
                            }
                            else
                            {
                                if (modle11.DcolletGold < 10)
                                {
                                    if (modle11.DcolletGold == 9)
                                    {
                                        new BCW.bydr.BLL.CmgToplist().UpdateAllcolletGold(meid, new BCW.bydr.BLL.Cmg_Top().GetCmg_AllcolletGold1(meid));//更新排行榜总收集币                  
                                        new BCW.bydr.BLL.CmgToplist().Updatetime(meid, DateTime.Now);
                                        // new BCW.bydr.BLL.Cmg_Top().updatetime1(id11, DateTime.Now);
                                    }
                                    string rand = "";
                                    string rand1 = "";
                                    //根据价格取不同的鱼类
                                    if (Convert.ToInt32(randaoju[modle11.DcolletGold]) < 500)
                                    {
                                        int r = 0;
                                        r = R(0, daojunum1.Length - 1);
                                        rand = Convert.ToString(daojunum1[r]);
                                        rand1 += modle11.randyuID + rand + ",";
                                        new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);

                                    }
                                    else if (Convert.ToInt32(randaoju[modle11.DcolletGold]) >= 500 && Convert.ToInt32(randaoju[modle11.DcolletGold]) < 1000)
                                    {
                                        int r = 0;
                                        r = R(0, daojunum2.Length - 1);
                                        rand = Convert.ToString(daojunum2[r]);
                                        rand1 += modle11.randyuID + rand + ",";
                                        new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);

                                    }
                                    else
                                    {
                                        int r = 0;
                                        r = R(0, daojunum3.Length - 1);
                                        rand = Convert.ToString(daojunum3[r]);
                                        rand1 += modle11.randyuID + rand + ",";
                                        new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);

                                    }
                                    if (modle11.YcolletGold == 1)
                                    {

                                        if (((DateTime.Now.AddSeconds(10) - modle11.updatetime.AddSeconds(10)).TotalSeconds > time))
                                        {
                                            new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, modle11.DcolletGold + 1);
                                            new BCW.bydr.BLL.Cmg_Top().updatetime(id11, DateTime.Now);
                                            new BCW.bydr.BLL.Cmg_Top().UpdateYcolletGold(id11, 0);
                                            Utils.Success("", "捕鱼成功！", Utils.getUrl("cmg.aspx?act=by"), "1");
                                        }
                                        else
                                        {
                                            new BCW.bydr.BLL.Cmg_Top().UpdateYcolletGold(id11, 0);
                                        }
                                    }
                                    string yuju = Convert.ToString(ub.GetSub("bydryuju", "/Controls/BYDR.xml"));
                                    string[] yu = yuju.Split('#');
                                    int n1 = R(0, yu.Length);
                                    builder.Append(yu[n1] + "<br />");
                                    builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx?act=by&amp;ptype=" + ptype + "") + "\">继续抓捕</a><br />");

                                    new BCW.bydr.BLL.Cmg_Top().updatetime(id11, DateTime.Now);//更新防刷时间   
                                                                                              //是否刷屏
                                    string we = "";
                                    int Expir = Utils.ParseInt(ub.GetSub("bydrExpir", xmlPath));
                                    BCW.User.Users.IsFresh(we, Expir);
                                    builder.Append(Out.Tab("</div>", ""));
                                    DUBBPage();
                                }
                                else
                                {

                                    builder.Append(Out.Tab("<div>", ""));
                                    builder.Append("本场次数已捕完。继续选择场景捕鱼<br />");
                                    builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx?") + "\">继续抓捕</a><br />");
                                    builder.Append(Out.Tab("</div>", ""));

                                    DUBBPage();
                                }
                            }
                        }
                    }; break;
                #endregion
                #region 场景6操作
                case 5:
                    {
                        BCW.bydr.Model.CmgDaoju modeldaoju25 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(325);
                        string[] numgold25 = modeldaoju25.Changjing.Split(',');
                        BCW.bydr.Model.CmgDaoju modeldaoju26 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(326);
                        string[] numgold26 = modeldaoju26.Changjing.Split(',');
                        BCW.bydr.Model.CmgDaoju modeldaoju27 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(327);
                        string[] numgold27 = modeldaoju27.Changjing.Split(',');
                        BCW.bydr.Model.CmgDaoju modeldaoju28 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(328);
                        string[] numgold28 = modeldaoju28.Changjing.Split(',');
                        BCW.bydr.Model.CmgDaoju modeldaoju29 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(329);
                        string[] numgold29 = modeldaoju29.Changjing.Split(',');
                        if (randgold == 0)
                        {
                            if (modle11.McolletGold == 25)
                                numgoldf = numgold25;
                            else if (modle11.McolletGold == 26)
                                numgoldf = numgold26;
                            else if (modle11.McolletGold == 27)
                                numgoldf = numgold27;
                            else if (modle11.McolletGold == 28)
                                numgoldf = numgold28;
                            else if (modle11.McolletGold == 29)
                                numgoldf = numgold29;
                            else
                                Utils.Error("选择场景出错！您属于非法操作！请返回游戏主页重新选择该场景", "");
                            int goldnumder = Convert.ToInt32(numgold25[0]);
                            string Logo = ub.GetSub("bydrLogo", xmlPath);
                            builder.Append(Out.Tab("<div>", ""));
                            builder.Append("<img src=\"" + Logo + "jiang.gif" + "\" alt=\"load\"/><br />");
                            builder.Append(Out.Tab("</div><br />", "<br />"));
                            builder.Append("亲，你太好运了！获得了惊喜大奖，，奖励" + goldnumder + "" + ub.Get("SiteBz") + "，惊喜只能捕鱼一次哦，请重新再选择场景继续抓捕<br />");
                            if (modle11.DcolletGold == 0)
                            {
                                BCW.bydr.Model.Cmg_notes m1 = new BCW.bydr.Model.Cmg_notes();
                                m1.AllGold = 0;
                                m1.AcolletGold = goldnumder;
                                m1.changj = modle11.Changj;
                                m1.coID = "";
                                m1.cxid = 1;
                                m1.random = 0;
                                m1.Stime = DateTime.Now;
                                m1.Signtime = DateTime.Now;
                                m1.usID = meid;
                                m1.Vit = 0;
                                new BCW.bydr.BLL.Cmg_notes().Add(m1);

                                new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, 10);
                                new BCW.bydr.BLL.CmgToplist().UpdateAllcolletGold(meid, new BCW.bydr.BLL.Cmg_Top().GetCmg_AllcolletGold1(meid));//更新排行榜总收集币                     
                                new BCW.bydr.BLL.CmgToplist().Updatetime(meid, DateTime.Now);
                                new BCW.bydr.BLL.Cmg_Top().updatetime1(id11, DateTime.Now);
                                new BCW.bydr.BLL.Cmg_Top().UpdateAllcolletGold(id11, goldnumder - 50000);//更新实际所得币
                                new BCW.bydr.BLL.Cmg_Top().UpdateColletGold(id11, goldnumder);//更新本次游戏所得的游戏币
                            }
                            builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx") + "\">继续捕鱼</a><br/>");
                            builder.Append(Out.Tab("<br />", ""));
                            builder.Append(Out.Tab("</div>", ""));
                            DUBBPage();
                        }
                        else
                        {
                            if (modle11.randdaoju == "")//更新随机鱼的价格
                            {
                                int a = 0;
                                string randdaoju = string.Empty;
                                Random ran = new Random();
                                if (modle11.McolletGold == 25)
                                    numgoldf = numgold25;
                                else if (modle11.McolletGold == 26)
                                    numgoldf = numgold26;
                                else if (modle11.McolletGold == 27)
                                    numgoldf = numgold27;
                                else if (modle11.McolletGold == 28)
                                    numgoldf = numgold28;
                                else if (modle11.McolletGold == 29)
                                    numgoldf = numgold29;
                                else
                                    Utils.Error("选择场景出错！您属于非法操作！请返回游戏主页重新选择该场景", "");

                                int goldnum = 0;
                                try
                                {
                                    goldnum = Convert.ToInt32(numgoldf[Convert.ToInt32(randgoldnum[modle11.randnum/*根据次数读取鱼的价格*/])]);
                                }
                                catch
                                {
                                    builder.Append(".");
                                    goldnum = 50000;
                                }
                                if (goldnum == 0)
                                    goldnum = 50000;
                                while (a < 10)
                                {
                                    int r = num(modle11.randten, goldnum)[a];//根据随机数的权值计算出每条鱼的价格
                                    randdaoju += Convert.ToString(r);
                                    randdaoju += ",";
                                    a++;
                                }
                                new BCW.bydr.BLL.Cmg_Top().Updateranddaoju(randdaoju, id11);
                                new BCW.bydr.BLL.Cmg_Top().UpdateColletGold(id11, goldnum);//更新本次游戏所得的游戏币
                                new BCW.bydr.BLL.Cmg_Top().UpdateAllcolletGold(id11, goldnum - model0.changj2);//更新实际所得币

                                builder.Append("您的体力-1" + "您的" + ub.Get("SiteBz") + "-" + model0.changj2 + "<br />");
                                new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, modle11.DcolletGold);
                                builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx?act=buyu&amp;id=1&amp;ptype=" + ptype) + "\">继续捕鱼</a><br/>");
                                builder.Append(Out.Tab("<br />", ""));
                                builder.Append(Out.Tab("</div>", ""));
                                DUBBPage();
                            }
                            else
                            {
                                if (modle11.DcolletGold < 10)
                                {
                                    if (modle11.DcolletGold == 9)
                                    {
                                        new BCW.bydr.BLL.CmgToplist().UpdateAllcolletGold(meid, new BCW.bydr.BLL.Cmg_Top().GetCmg_AllcolletGold1(meid));//更新排行榜总收集币
                                        new BCW.bydr.BLL.CmgToplist().Updatetime(meid, DateTime.Now);
                                        // new BCW.bydr.BLL.Cmg_Top().updatetime1(id11, DateTime.Now);
                                    }

                                    string rand = "";
                                    string rand1 = "";
                                    //根据价格取不同的鱼类
                                    if (Convert.ToInt32(randaoju[modle11.DcolletGold]) < 500)
                                    {
                                        int r = 0;
                                        r = R(0, daojunum1.Length - 1);
                                        rand = Convert.ToString(daojunum1[r]);
                                        rand1 += modle11.randyuID + rand + ",";
                                        new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);

                                    }
                                    else if (Convert.ToInt32(randaoju[modle11.DcolletGold]) >= 500 && Convert.ToInt32(randaoju[modle11.DcolletGold]) < 2000)
                                    {
                                        int r = 0;
                                        r = R(0, daojunum2.Length - 1);
                                        rand = Convert.ToString(daojunum2[r]);
                                        rand1 += modle11.randyuID + rand + ",";
                                        new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);

                                    }
                                    else
                                    {
                                        int r = 0;
                                        r = R(0, daojunum3.Length - 1);
                                        rand = Convert.ToString(daojunum3[r]);
                                        rand1 += modle11.randyuID + rand + ",";
                                        new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);

                                    }
                                    if (modle11.YcolletGold == 1)
                                    {

                                        if (((DateTime.Now.AddSeconds(10) - modle11.updatetime.AddSeconds(10)).TotalSeconds > time))
                                        {
                                            new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, modle11.DcolletGold + 1);
                                            new BCW.bydr.BLL.Cmg_Top().updatetime(id11, DateTime.Now);
                                            new BCW.bydr.BLL.Cmg_Top().UpdateYcolletGold(id11, 0);
                                            Utils.Success("", "捕鱼成功！", Utils.getUrl("cmg.aspx?act=by"), "1");
                                        }
                                        else
                                        {
                                            new BCW.bydr.BLL.Cmg_Top().UpdateYcolletGold(id11, 0);
                                        }
                                    }
                                    string yuju = Convert.ToString(ub.GetSub("bydryuju", "/Controls/BYDR.xml"));
                                    string[] yu = yuju.Split('#');
                                    int n1 = R(0, yu.Length);
                                    builder.Append(yu[n1] + "<br />");
                                    builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx?act=by&amp;ptype=" + ptype + "") + "\">继续抓捕</a><br />");

                                    new BCW.bydr.BLL.Cmg_Top().updatetime(id11, DateTime.Now);//更新防刷时间 
                                                                                              //是否刷屏
                                    string we = "";
                                    int Expir = Utils.ParseInt(ub.GetSub("bydrExpir", xmlPath));
                                    BCW.User.Users.IsFresh(we, Expir);
                                    builder.Append(Out.Tab("</div>", ""));
                                    DUBBPage();
                                }
                                else
                                {

                                    builder.Append(Out.Tab("<div>", ""));
                                    builder.Append("本场次数已捕完。继续选择场景捕鱼<br />");
                                    builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx?") + "\">继续抓捕</a><br />");
                                    builder.Append(Out.Tab("</div>", ""));

                                    DUBBPage();
                                }
                            }
                        }
                    }; break;
                default:
                    break;
                    #endregion
            }
        }
        else
        {
            if (modle11.DcolletGold == 10)
            {

                builder.Append(Out.Tab("<div>", ""));
                builder.Append("本场次数已捕完。继续选择场景捕鱼<br />");
                builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx?") + "\">继续抓捕</a><br />");
                builder.Append(Out.Tab("</div>", ""));

                DUBBPage();
            }
            else
            {
                string yuju = Convert.ToString(ub.GetSub("bydryuju", "/Controls/BYDR.xml"));
                string[] yu = yuju.Split('#');
                int n1 = R(0, yu.Length);
                builder.Append(yu[n1] + "<br />");
                builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx?act=buyu&amp;id=1&amp;ptype=" + ptype + "") + "\">继续抓捕</a><br />");

                //是否刷屏
                string we = "";
                int Expir = Utils.ParseInt(ub.GetSub("bydrExpir", xmlPath));
                BCW.User.Users.IsFresh(we, Expir);
                builder.Append(Out.Tab("</div>", ""));
                DUBBPage();
            }

        }
        #endregion

    }
    #endregion
    /// <summary>
    /// 底部界面
    /// </summary>


    #region 捕鱼跳转页面
    private void byPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx") + "\">捕鱼</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        bool meid6 = new BCW.bydr.BLL.Cmg_Top().ExistsusID(meid);
        //如不存在,则增加一条数据
        if (meid6 == false)
            addbuyub();
        DataSet rows11 = new BCW.bydr.BLL.Cmg_Top().GetList("Top 1 ID ", "usID=" + meid + " order by Time desc");


        int id11 = Convert.ToInt32(rows11.Tables[0].Rows[0][0]);

        BCW.bydr.Model.Cmg_Top modle11 = new BCW.bydr.BLL.Cmg_Top().GetCmg_Top(id11);
        if (modle11.DcolletGold == 10)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("本场次数已捕完。继续选择场景捕鱼<br />");
            builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx?") + "\">继续抓捕</a><br />");
            builder.Append(Out.Tab("</div>", ""));
            DUBBPage();
        }
        else
        {
            int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 1, @"^[0-5]$", "0"));
            string[] randyu = modle11.randyuID.Split(',');
            string[] randaoju = modle11.randdaoju.Split(',');
            BCW.bydr.Model.CmgDaoju modeldao = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(Convert.ToInt32(randyu[modle11.DcolletGold]));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("=捕鱼补贴家用=" + "<br />");
            builder.Append("次数：" + (Convert.ToInt32(modle11.DcolletGold) + 1) + "/10");
            builder.Append(Out.Tab("</div>", "<br />"));
            string Logo = ub.GetSub("bydrLogo", xmlPath);
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<img src=\"" + Logo + modeldao.changj1 + "\" alt=\"load\"/>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(modeldao.Changjing + modeldao.Daoju + ",价值" + randaoju[modle11.DcolletGold] + ub.Get("SiteBz") + "<br/>");
            builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx?act=buyu&amp;id=1&amp;ptype=" + ptype) + "\">哇～好激动!继续捕鱼</a><br/>");
            if (modle11.YcolletGold == 0)
            {
                new BCW.bydr.BLL.Cmg_Top().UpdateYcolletGold(id11, 1);
                BCW.bydr.Model.Cmg_notes m1 = new BCW.bydr.Model.Cmg_notes();//添加动态记录
                m1.AllGold = 0;
                m1.AcolletGold = Convert.ToInt64(randaoju[modle11.DcolletGold]);
                m1.changj = modle11.Changj;
                m1.coID = "";
                m1.cxid = 0;
                m1.random = Convert.ToInt32(randyu[modle11.DcolletGold]);
                m1.Stime = DateTime.Now;
                m1.Signtime = DateTime.Now;
                m1.usID = meid;
                m1.Vit = 0;
                new BCW.bydr.BLL.Cmg_notes().Add(m1);
            }
            builder.Append(Out.Tab("</div>", ""));
            DUBBPage();
        }
    }
    #endregion
    private void fangshua()
    {
        Master.Title = "捕鱼游戏中";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx") + "\">捕鱼</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 1, @"^[0-5]$", "0"));
        string yuju = Convert.ToString(ub.GetSub("bydryuju", "/Controls/BYDR.xml"));
        string[] yu = yuju.Split('#');
        int n1 = R(0, yu.Length);
        builder.Append(yu[n1] + "<br />");
        builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx?act=buyu&amp;id=1&amp;ptype=" + ptype + "") + "\">继续抓捕</a><br />");

        //是否刷屏
        string we = "";
        int Expir = Utils.ParseInt(ub.GetSub("bydrExpir", xmlPath));
        BCW.User.Users.IsFresh(we, Expir);
        builder.Append(Out.Tab("</div>", ""));
        DUBBPage();
    }

    //底部界面
    private void DUBBPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx") + "\">捕鱼</a>");
        builder.Append(Out.Tab("</div>", ""));
    }


    #region 未捕完提示 waitPage
    /// <summary>
    /// 未捕完提示
    /// </summary>
    private void waitPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 1, @"^[0-5]$", "0"));
        //等待语句
        string yuju = Convert.ToString(ub.GetSub("bydryuju", "/Controls/BYDR.xml"));
        string[] yu = yuju.Split('#');
        int n1 = R(0, yu.Length);
        //刷新时间
        int time = Convert.ToInt32(ub.GetSub("bydrExpir1", "/Controls/BYDR.xml"));
        //获取用户最后一条数据
        DataSet rows9 = new BCW.bydr.BLL.Cmg_Top().GetList("Top 1 ID", "usID=" + meid + "  order by Time desc");
        int id2 = Convert.ToInt32(rows9.Tables[0].Rows[0][0]);
        BCW.bydr.Model.Cmg_Top modle3 = new BCW.bydr.BLL.Cmg_Top().GetCmg_Top(id2);

        #region 上帝之手用,暂无效果
        int n = 0;
        int maxID = new BCW.bydr.BLL.CmgDaoju().GetMaxId();
        //特殊ID识别 
        if (new BCW.bydr.BLL.CmgToplist().ExistsusID1(meid, 1))
            n = R(7, 90);
        else
            n = R(13, maxID);
        #endregion

        if (new BCW.bydr.BLL.CmgToplist().Getvit(meid) < 1)
        {
            Utils.Error("您的体力不足", "");
        }
        long Gold = new BCW.BLL.User().GetGold(meid);
        int no = 0;
        //读取场景
        DataSet dtchangj = new BCW.bydr.BLL.CmgDaoju().GetList("ID", "Xiaoxi=0 and Tianyuan=0 ORDER BY changj2 ASC");
        int ID1 = Convert.ToInt32(dtchangj.Tables[0].Rows[ptype][0]);
        BCW.bydr.Model.CmgDaoju model10 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(ID1);

        //是否存在未捕完的数据
        bool sss = new BCW.bydr.BLL.Cmg_Top().ExistsusID1(meid);
        if (sss)
        {
            BCW.User.Users.IsFresh("cmg1", 1);
            //判断未捕完的次数时间是否超过24小时
            if (((DateTime.Now.AddHours(10) - Convert.ToDateTime(modle3.Time).AddHours(10)).TotalHours > 24))
            {
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
                builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx") + "\">捕鱼</a><br />");
                builder.Append(Out.Tab("</div>", ""));

                int s = 0;
                int ss = Convert.ToInt32(modle3.DcolletGold) + 1;
                if (modle3.DcolletGold != 10)
                {
                    DataSet ds = new BCW.bydr.BLL.Cmg_notes().GetList("Top " + ss + " AcolletGold as aa", "usID=" + meid + "  order by Stime desc");
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int a = 0; a < ds.Tables[0].Rows.Count; a++)//遍历数据库Cmg_notes表的记录集
                        {
                            s += Convert.ToInt32(ds.Tables[0].Rows[a]["aa"]);
                        }
                    }
                    new BCW.bydr.BLL.Cmg_Top().UpdateAllcolletGold(id2, s - model10.changj2);//更新实际所得币
                    new BCW.bydr.BLL.CmgToplist().UpdateAllcolletGold(meid, new BCW.bydr.BLL.Cmg_Top().GetCmg_AllcolletGold1(meid));//更新排行榜总收集币                     
                    new BCW.bydr.BLL.CmgToplist().Updatetime(meid, DateTime.Now);
                    new BCW.bydr.BLL.Cmg_Top().updatetime1(id2, DateTime.Now);
                    new BCW.bydr.BLL.Cmg_Top().UpdateColletGold(id2, s);//更新本次游戏所得的游戏币
                    new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id2, 10);

                }
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("由于您在改场景钓鱼的时间超过了24个小时，鱼儿早就脱钩跑掉了！真是竹篮打水一场空啊，下次注意了！<br />");
                builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx?") + "\">返回继续抓捕</a><br />");
                builder.Append(Out.Tab("</div>", ""));
                DUBBPage();
            }
            else
            {
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
                builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx") + "\">捕鱼</a><br />");
                builder.Append(Out.Tab("</div>", ""));
                builder.Append(Out.Tab("<div class=\"\">", ""));
                builder.Append("您还有未捕完的次数，请继续。。。<br />" + "<a href=\"" + Utils.getUrl("cmg.aspx?act=buyu&amp;ptype=" + ptype + "") + "\">继续</a>&gt;");
                builder.Append(Out.Tab("</div>", ""));
                DUBBPage();
            }
        }
        else
        {
            if (Gold < model10.changj2)
                Utils.Error("您的" + ub.Get("SiteBz") + "不够进入该场景！请选择合适的场景钓鱼！祝您游戏愉快！", "");
            //支付安全提示
            string[] p_pageArr = { "ptype", "act", "sid" };
            BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr);
            int sid = 0;
            DataSet ds = new BCW.bydr.BLL.CmgToplist().GetList("sid", "usid=" + meid);
            try
            {
                sid = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
            }
            catch
            {
                sid = 0;
            }
            BCW.User.Users.IsFresh("cmg1", 1);
            if (((DateTime.Now.AddSeconds(10) - Convert.ToDateTime(modle3.Time).AddSeconds(10)).TotalSeconds > 60))//超时判断，更新防止同时入场字段sid
            {
                new BCW.bydr.BLL.CmgToplist().Updatesid(meid, 0);
            }
            if (sid == 0)
            {
                new BCW.bydr.BLL.CmgToplist().Updatesid(meid, 1);
                int k = 0;//控制价格变动！！
                BCW.bydr.Model.Cmg_Top m2 = new BCW.bydr.Model.Cmg_Top();
                //读取随机次数
                BCW.bydr.Model.CmgDaoju modeldaoju = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(300);
                int i = 0;
                string randgoldnum = "0";
                int randnum = 0;//每个相同场景增加1到30次一轮回
                long WinnersGold = 0;
                if (sss)
                {
                    Utils.Error("您不能同时进入不同的场景！", "");
                }
                else
                {
                    switch (ptype)
                    {
                        #region 场景1判断
                        case 0:
                            {
                                no = ptype + 1;
                                k = R(0, 5);//每个场景价格均有一个唯一对应的值！！！
                                BCW.bydr.Model.CmgDaoju model0 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(no);
                                BCW.bydr.Model.CmgDaoju modelchang1 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(1);
                                DataSet changj1 = new BCW.bydr.BLL.Cmg_Top().GetList("Top 1 ID", "Changj='" + modelchang1.Changjing + "' order by Time desc ");
                                int changid1 = 1;
                                try
                                {
                                    changid1 = Convert.ToInt32(changj1.Tables[0].Rows[0][0]);
                                }
                                catch (Exception)
                                {
                                }
                                BCW.bydr.Model.Cmg_Top modelchangj1 = new BCW.bydr.BLL.Cmg_Top().GetCmg_Top(changid1);

                                m2.Changj = model0.Changjing;
                                m2.usID = meid;
                                m2.AllcolletGold = 0;
                                m2.Changj = model0.Changjing;
                                m2.ColletGold = 0;
                                m2.DcolletGold = 0;
                                m2.Time = DateTime.Now;
                                m2.YcolletGold = 0;//每个场景n次标识
                                m2.Bid = 1;
                                m2.jID = 1;
                                BCW.bydr.Model.CmgDaoju modeldaoju111 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(300);
                                string[] randgoldnum1 = modelchangj1.randgoldnum.Split(',');
                                if ((model0.Changjing == modelchang1.Changjing && randgoldnum1.Length - 1 == modeldaoju111.changj2))
                                {
                                    if ((model0.Changjing == modelchang1.Changjing && modelchangj1.randnum > (Convert.ToInt32(modeldaoju.changj2) - 2)))
                                    {
                                        randnum = 0;
                                        Random ran = new Random();
                                        while (i < modeldaoju.changj2)
                                        {
                                            randgoldnum += Convert.ToString(GetRandomNum(Convert.ToInt32(modeldaoju.changj2), 0, Convert.ToInt32(modeldaoju.changj2) - 1)[i]);
                                            randgoldnum += ",";
                                            i++;
                                        }
                                        m2.randgoldnum = randgoldnum;
                                    }
                                    else
                                    {
                                        randnum = modelchangj1.randnum + 1;
                                        m2.randgoldnum = modelchangj1.randgoldnum;
                                        k = Convert.ToInt32(modelchangj1.McolletGold);
                                    }
                                }
                                else
                                {
                                    randnum = 0;
                                    Random ran = new Random();
                                    while (i < modeldaoju.changj2)
                                    {
                                        randgoldnum += Convert.ToString(GetRandomNum(Convert.ToInt32(modeldaoju.changj2), 0, Convert.ToInt32(modeldaoju.changj2) - 1)[i]);
                                        randgoldnum += ",";
                                        i++;
                                    }
                                    m2.randgoldnum = randgoldnum;
                                }
                                m2.randnum = randnum;
                                m2.McolletGold = k;
                                m2.randdaoju = "";
                                m2.randyuID = "";

                                int j1 = 0;
                                string randte1 = string.Empty;
                                while (j1 < 10)
                                {
                                    int r = GetRandomNum(10, 3, 25)[j1];
                                    randte1 += Convert.ToString(r);
                                    randte1 += ",";
                                    j1++;
                                }
                                m2.randten = randte1;
                                m2.updatetime = DateTime.Now;
                                m2.isrobot = 0;
                                //增加兑换表数据
                                new BCW.bydr.BLL.Cmg_Top().Add(m2);
                                //更新体力值
                                new BCW.bydr.BLL.CmgToplist().Updatevit1(meid, new BCW.bydr.BLL.CmgToplist().Getvit(meid) - 1);
                                //等级更新判断是否捕完一次游戏，等级经验加1    
                                BCW.bydr.Model.CmgToplist model20 = new BCW.bydr.BLL.CmgToplist().GetCmgToplistusID(meid);
                                new BCW.bydr.BLL.CmgToplist().Updatestype(meid, Convert.ToInt32(model20.stype) + 1);
                                //读取最后一条数据的id
                                DataSet rows10 = new BCW.bydr.BLL.Cmg_Top().GetList("Top 1 ID", "YcolletGold=0  order by Time desc");
                                int maxID2 = Convert.ToInt32(rows10.Tables[0].Rows[0][0]);
                                BCW.bydr.Model.Cmg_Top modle4 = new BCW.bydr.BLL.Cmg_Top().GetCmg_Top(maxID2);//读取数据的最后一条id    
                                DataSet rows = new BCW.BLL.User().GetList("VipDate", "ID=" + meid);

                                DateTime viptime = DateTime.Now;
                                try
                                {
                                    viptime = Convert.ToDateTime(rows.Tables[0].Rows[0][0]);
                                }
                                catch
                                {
                                    viptime = Convert.ToDateTime(bydrBotTime);
                                }
                                if (DateTime.Now < viptime)
                                {
                                    if (model10.changj2 == 200)
                                    {
                                        int count = 0;
                                        count = new BCW.bydr.BLL.Cmg_Top().GetCmgcount(meid);
                                        string swhere = DateTime.Now.ToString("yyyy-MM-dd");
                                        bool viptime1 = new BCW.bydr.BLL.CmgToplist().Existsusvip(meid, swhere);
                                        if (!viptime1 || count == 1)
                                        {
                                            builder.Append("VIP每日第一次免费！<br />");
                                        }
                                        else
                                        {
                                            builder.Append("VIP每日第一次免费！<br />");
                                            WinnersGold = model10.changj2;
                                            new BCW.BLL.User().UpdateiGold(meid, -model10.changj2, "捕鱼入场费-标识id" + (modle4.ID));
                                        }
                                    }
                                    else
                                    {
                                        builder.Append("VIP免费！<br />");
                                        WinnersGold = model10.changj2;
                                        new BCW.BLL.User().UpdateiGold(meid, -model10.changj2, "捕鱼入场费-标识id" + (modle4.ID));
                                    }
                                }
                                else
                                {
                                    builder.Append("VIP免费！<br />");
                                    WinnersGold = model10.changj2;
                                    new BCW.BLL.User().UpdateiGold(meid, -model10.changj2, "捕鱼入场费-标识id" + (modle4.ID));
                                }
                                new BCW.bydr.BLL.CmgToplist().Updatesid(meid, 0);

                                Utils.Success("", "钓鱼啊钓鱼，钓到一条大鱼。", Utils.getUrl("cmg.aspx?act=buyu&amp;ptype=" + ptype + "") + "", "1");
                            }
                            break;
                        #endregion
                        #region 场景2
                        case 1:
                            {
                                no = ptype + 1;
                                k = R(5, 10);//每个场景价格均有一个唯一对应的值！！！
                                BCW.bydr.Model.CmgDaoju model0 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(no);

                                BCW.bydr.Model.CmgDaoju modelchang2 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(2);
                                DataSet changj2 = new BCW.bydr.BLL.Cmg_Top().GetList("Top 1 ID", "Changj='" + modelchang2.Changjing + "' order by Time desc ");
                                int changid2 = 1;
                                try
                                {
                                    changid2 = Convert.ToInt32(changj2.Tables[0].Rows[0][0]);
                                }
                                catch (Exception)
                                {
                                }

                                BCW.bydr.Model.Cmg_Top modelchangj2 = new BCW.bydr.BLL.Cmg_Top().GetCmg_Top(changid2);
                                m2.usID = meid;
                                m2.AllcolletGold = 0;
                                m2.Changj = model0.Changjing;
                                m2.ColletGold = 0;
                                m2.DcolletGold = 0;
                                m2.Time = DateTime.Now;
                                m2.YcolletGold = 0;//每个场景n次标识
                                m2.Bid = 1;
                                m2.jID = 1;
                                string[] randgoldnum2 = modelchangj2.randgoldnum.Split(',');
                                if ((model0.Changjing == modelchang2.Changjing && randgoldnum2.Length - 1 == modeldaoju.changj2))
                                {
                                    //每个场合循环判断
                                    if ((model0.Changjing == modelchang2.Changjing && modelchangj2.randnum > (Convert.ToInt32(modeldaoju.changj2) - 2)))
                                    {
                                        randnum = 0;
                                        Random ran = new Random();
                                        while (i < modeldaoju.changj2)
                                        {
                                            randgoldnum += Convert.ToString(GetRandomNum(Convert.ToInt32(modeldaoju.changj2), 0, Convert.ToInt32(modeldaoju.changj2) - 1)[i]);
                                            randgoldnum += ",";
                                            i++;
                                        }
                                        m2.randgoldnum = randgoldnum;
                                    }
                                    else
                                    {
                                        randnum = modelchangj2.randnum + 1;
                                        m2.randgoldnum = modelchangj2.randgoldnum;
                                        k = Convert.ToInt32(modelchangj2.McolletGold);
                                    }
                                }
                                else
                                {
                                    randnum = 0;
                                    Random ran = new Random();
                                    while (i < modeldaoju.changj2)
                                    {
                                        randgoldnum += Convert.ToString(GetRandomNum(Convert.ToInt32(modeldaoju.changj2), 0, Convert.ToInt32(modeldaoju.changj2) - 1)[i]);
                                        randgoldnum += ",";
                                        i++;
                                    }
                                    m2.randgoldnum = randgoldnum;
                                }
                                m2.randnum = randnum;
                                m2.McolletGold = k;
                                m2.randdaoju = "";
                                m2.randyuID = "";

                                int j = 0;
                                string randte = string.Empty;
                                while (j < 10)
                                {
                                    int r = GetRandomNum(10, 3, 25)[j];
                                    randte += Convert.ToString(r);
                                    randte += ",";
                                    j++;
                                }
                                m2.randten = randte;
                                m2.updatetime = DateTime.Now;
                                m2.isrobot = 0;
                                //增加兑换表数据
                                new BCW.bydr.BLL.Cmg_Top().Add(m2);
                                //更新体力值
                                new BCW.bydr.BLL.CmgToplist().Updatevit1(meid, new BCW.bydr.BLL.CmgToplist().Getvit(meid) - 1);
                                //等级更新判断是否捕完一次游戏，等级经验加1    
                                BCW.bydr.Model.CmgToplist model21 = new BCW.bydr.BLL.CmgToplist().GetCmgToplistusID(meid);
                                new BCW.bydr.BLL.CmgToplist().Updatestype(meid, Convert.ToInt32(model21.stype) + 1);
                                //读取最后一条数据的id
                                DataSet rows10 = new BCW.bydr.BLL.Cmg_Top().GetList("Top 1 ID", "YcolletGold=0  order by Time desc");
                                int maxID2 = Convert.ToInt32(rows10.Tables[0].Rows[0][0]);
                                BCW.bydr.Model.Cmg_Top modle4 = new BCW.bydr.BLL.Cmg_Top().GetCmg_Top(maxID2);//读取数据的最后一条id    
                                new BCW.BLL.User().UpdateiGold(meid, -model10.changj2, "捕鱼入场费-标识id" + (modle4.ID));
                                WinnersGold = model10.changj2;
                                new BCW.bydr.BLL.CmgToplist().Updatesid(meid, 0);
                                Utils.Success("", "钓鱼啊钓鱼，钓到一条大鱼。", Utils.getUrl("cmg.aspx?act=buyu&amp;ptype=" + ptype + "") + "", "1");
                            }
                            break;
                        #endregion
                        #region 场景3
                        case 2:
                            {
                                no = ptype + 1;
                                k = R(10, 15);//每个场景价格均有一个唯一对应的值！！！
                                BCW.bydr.Model.CmgDaoju model0 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(no);
                                BCW.bydr.Model.CmgDaoju modelchang3 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(3);
                                DataSet changj3 = new BCW.bydr.BLL.Cmg_Top().GetList("Top 1 ID", "Changj='" + modelchang3.Changjing + "' order by Time desc ");
                                int changid3 = 1;
                                try
                                {
                                    changid3 = Convert.ToInt32(changj3.Tables[0].Rows[0][0]);
                                }
                                catch (Exception)
                                {
                                }
                                BCW.bydr.Model.Cmg_Top modelchangj3 = new BCW.bydr.BLL.Cmg_Top().GetCmg_Top(changid3);
                                m2.usID = meid;
                                m2.AllcolletGold = 0;
                                m2.Changj = model0.Changjing;
                                m2.ColletGold = 0;
                                m2.DcolletGold = 0;
                                m2.Time = DateTime.Now;
                                m2.YcolletGold = 0;//每个场景n次标识
                                m2.Bid = 1;
                                m2.jID = 1;
                                string[] randgoldnum3 = modelchangj3.randgoldnum.Split(',');
                                if ((model0.Changjing == modelchang3.Changjing && randgoldnum3.Length - 1 == modeldaoju.changj2))
                                {
                                    if ((model0.Changjing == modelchang3.Changjing && modelchangj3.randnum > (Convert.ToInt32(modeldaoju.changj2) - 2)))
                                    {
                                        randnum = 0;
                                        Random ran = new Random();
                                        while (i < modeldaoju.changj2)
                                        {
                                            randgoldnum += Convert.ToString(GetRandomNum(Convert.ToInt32(modeldaoju.changj2), 0, Convert.ToInt32(modeldaoju.changj2) - 1)[i]);
                                            randgoldnum += ",";
                                            i++;
                                        }
                                        m2.randgoldnum = randgoldnum;
                                    }
                                    else
                                    {
                                        randnum = modelchangj3.randnum + 1;
                                        m2.randgoldnum = modelchangj3.randgoldnum;
                                        k = Convert.ToInt32(modelchangj3.McolletGold);
                                    }
                                }
                                else
                                {
                                    randnum = 0;
                                    Random ran = new Random();
                                    while (i < modeldaoju.changj2)
                                    {
                                        randgoldnum += Convert.ToString(GetRandomNum(Convert.ToInt32(modeldaoju.changj2), 0, Convert.ToInt32(modeldaoju.changj2) - 1)[i]);
                                        randgoldnum += ",";
                                        i++;
                                    }
                                    m2.randgoldnum = randgoldnum;
                                }
                                m2.randnum = randnum;
                                m2.McolletGold = k;
                                m2.randdaoju = "";
                                m2.randyuID = "";

                                int j = 0;
                                string randte = string.Empty;
                                while (j < 10)
                                {
                                    int r = GetRandomNum(10, 3, 25)[j];
                                    randte += Convert.ToString(r);
                                    randte += ",";
                                    j++;
                                }
                                m2.randten = randte;
                                m2.updatetime = DateTime.Now;
                                m2.isrobot = 0;
                                //增加兑换表数据
                                new BCW.bydr.BLL.Cmg_Top().Add(m2);
                                //更新体力值
                                new BCW.bydr.BLL.CmgToplist().Updatevit1(meid, new BCW.bydr.BLL.CmgToplist().Getvit(meid) - 1);
                                //等级更新判断是否捕完一次游戏，等级经验加1    
                                BCW.bydr.Model.CmgToplist model21 = new BCW.bydr.BLL.CmgToplist().GetCmgToplistusID(meid);
                                new BCW.bydr.BLL.CmgToplist().Updatestype(meid, Convert.ToInt32(model21.stype) + 1);
                                //读取最后一条数据的id
                                DataSet rows10 = new BCW.bydr.BLL.Cmg_Top().GetList("Top 1 ID", "YcolletGold=0  order by Time desc");
                                int maxID2 = Convert.ToInt32(rows10.Tables[0].Rows[0][0]);
                                BCW.bydr.Model.Cmg_Top modle4 = new BCW.bydr.BLL.Cmg_Top().GetCmg_Top(maxID2);//读取数据的最后一条id    
                                new BCW.BLL.User().UpdateiGold(meid, -model10.changj2, "捕鱼入场费-标识id" + (modle4.ID));
                                WinnersGold = model10.changj2;
                                new BCW.bydr.BLL.CmgToplist().Updatesid(meid, 0);
                                Utils.Success("", "钓鱼啊钓鱼，钓到一条大鱼。", Utils.getUrl("cmg.aspx?act=buyu&amp;ptype=" + ptype + "") + "", "1");
                            }
                            break;
                        #endregion
                        #region 场景4
                        case 3:
                            {
                                no = ptype + 1;
                                k = R(15, 20);//每个场景价格均有一个唯一对应的值！！！
                                BCW.bydr.Model.CmgDaoju model0 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(no);
                                BCW.bydr.Model.CmgDaoju modelchang4 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(4);
                                DataSet changj4 = new BCW.bydr.BLL.Cmg_Top().GetList("Top 1 ID", "Changj='" + modelchang4.Changjing + "' order by Time desc ");
                                int changid4 = 1;
                                try
                                {
                                    changid4 = Convert.ToInt32(changj4.Tables[0].Rows[0][0]);
                                }
                                catch (Exception)
                                {
                                }
                                BCW.bydr.Model.Cmg_Top modelchangj4 = new BCW.bydr.BLL.Cmg_Top().GetCmg_Top(changid4);
                                m2.usID = meid;
                                m2.AllcolletGold = 0;
                                m2.Changj = model0.Changjing;
                                m2.ColletGold = 0;
                                m2.DcolletGold = 0;
                                m2.Time = DateTime.Now;
                                m2.YcolletGold = 0;//每个场景n次标识
                                m2.Bid = 1;
                                m2.jID = 1;
                                string[] randgoldnum4 = modelchangj4.randgoldnum.Split(',');
                                if ((model0.Changjing == modelchang4.Changjing && randgoldnum4.Length - 1 == modeldaoju.changj2))
                                {
                                    if ((model0.Changjing == modelchang4.Changjing && modelchangj4.randnum > (Convert.ToInt32(modeldaoju.changj2) - 2)))
                                    {
                                        randnum = 0;
                                        Random ran = new Random();
                                        while (i < modeldaoju.changj2)
                                        {
                                            randgoldnum += Convert.ToString(GetRandomNum(Convert.ToInt32(modeldaoju.changj2), 0, Convert.ToInt32(modeldaoju.changj2) - 1)[i]);
                                            randgoldnum += ",";
                                            i++;
                                        }
                                        m2.randgoldnum = randgoldnum;
                                    }
                                    else
                                    {
                                        randnum = modelchangj4.randnum + 1;
                                        m2.randgoldnum = modelchangj4.randgoldnum;
                                        k = Convert.ToInt32(modelchangj4.McolletGold);
                                    }
                                }
                                else
                                {
                                    randnum = 0;
                                    Random ran = new Random();
                                    while (i < modeldaoju.changj2)
                                    {
                                        randgoldnum += Convert.ToString(GetRandomNum(Convert.ToInt32(modeldaoju.changj2), 0, Convert.ToInt32(modeldaoju.changj2) - 1)[i]);
                                        randgoldnum += ",";
                                        i++;
                                    }
                                    m2.randgoldnum = randgoldnum;
                                }
                                m2.randnum = randnum;
                                m2.McolletGold = k;
                                m2.randdaoju = "";
                                m2.randyuID = "";

                                int j = 0;
                                string randte = string.Empty;
                                while (j < 10)
                                {
                                    int r = GetRandomNum(10, 3, 25)[j];
                                    randte += Convert.ToString(r);
                                    randte += ",";
                                    j++;
                                }
                                m2.randten = randte;
                                m2.updatetime = DateTime.Now;
                                m2.isrobot = 0;
                                //增加兑换表数据
                                new BCW.bydr.BLL.Cmg_Top().Add(m2);
                                //更新体力值
                                new BCW.bydr.BLL.CmgToplist().Updatevit1(meid, new BCW.bydr.BLL.CmgToplist().Getvit(meid) - 1);
                                //等级更新判断是否捕完一次游戏，等级经验加1    
                                BCW.bydr.Model.CmgToplist model21 = new BCW.bydr.BLL.CmgToplist().GetCmgToplistusID(meid);
                                new BCW.bydr.BLL.CmgToplist().Updatestype(meid, Convert.ToInt32(model21.stype) + 1);
                                //读取最后一条数据的id
                                DataSet rows10 = new BCW.bydr.BLL.Cmg_Top().GetList("Top 1 ID", "YcolletGold=0  order by Time desc");
                                int maxID2 = Convert.ToInt32(rows10.Tables[0].Rows[0][0]);
                                BCW.bydr.Model.Cmg_Top modle4 = new BCW.bydr.BLL.Cmg_Top().GetCmg_Top(maxID2);//读取数据的最后一条id    
                                new BCW.BLL.User().UpdateiGold(meid, -model10.changj2, "捕鱼入场费-标识id" + (modle4.ID));
                                WinnersGold = model10.changj2;
                                new BCW.bydr.BLL.CmgToplist().Updatesid(meid, 0);
                                Utils.Success("", "钓鱼啊钓鱼，钓到一条大鱼。", Utils.getUrl("cmg.aspx?act=buyu&amp;ptype=" + ptype + "") + "", "1");
                            }
                            break;
                        #endregion
                        #region 场景5
                        case 4:
                            {
                                no = ptype + 1;
                                k = R(20, 25);//每个场景价格均有一个唯一对应的值！！！
                                BCW.bydr.Model.CmgDaoju model0 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(no);
                                BCW.bydr.Model.CmgDaoju modelchang5 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(5);
                                DataSet changj5 = new BCW.bydr.BLL.Cmg_Top().GetList("Top 1 ID", "Changj='" + modelchang5.Changjing + "' order by Time desc ");
                                int changid5 = 1;
                                try
                                {
                                    changid5 = Convert.ToInt32(changj5.Tables[0].Rows[0][0]);
                                }
                                catch (Exception)
                                {
                                }
                                BCW.bydr.Model.Cmg_Top modelchangj5 = new BCW.bydr.BLL.Cmg_Top().GetCmg_Top(changid5);
                                m2.usID = meid;
                                m2.AllcolletGold = 0;
                                m2.Changj = model0.Changjing;
                                m2.ColletGold = 0;
                                m2.DcolletGold = 0;
                                m2.Time = DateTime.Now;
                                m2.YcolletGold = 0;//每个场景n次标识
                                m2.Bid = 1;
                                m2.jID = 1;
                                string[] randgoldnum5 = modelchangj5.randgoldnum.Split(',');
                                if ((model0.Changjing == modelchang5.Changjing && randgoldnum5.Length - 1 == modeldaoju.changj2))
                                {
                                    if ((model0.Changjing == modelchang5.Changjing && modelchangj5.randnum > (Convert.ToInt32(modeldaoju.changj2) - 2)))
                                    {
                                        randnum = 0;
                                        Random ran = new Random();
                                        while (i < modeldaoju.changj2)
                                        {
                                            randgoldnum += Convert.ToString(GetRandomNum(Convert.ToInt32(modeldaoju.changj2), 0, Convert.ToInt32(modeldaoju.changj2) - 1)[i]);
                                            randgoldnum += ",";
                                            i++;
                                        }
                                        m2.randgoldnum = randgoldnum;
                                    }
                                    else
                                    {
                                        randnum = modelchangj5.randnum + 1;
                                        m2.randgoldnum = modelchangj5.randgoldnum;
                                        k = Convert.ToInt32(modelchangj5.McolletGold);
                                    }
                                }
                                else
                                {
                                    randnum = 0;
                                    Random ran = new Random();
                                    while (i < modeldaoju.changj2)
                                    {
                                        randgoldnum += Convert.ToString(GetRandomNum(Convert.ToInt32(modeldaoju.changj2), 0, Convert.ToInt32(modeldaoju.changj2) - 1)[i]);
                                        randgoldnum += ",";
                                        i++;
                                    }
                                    m2.randgoldnum = randgoldnum;
                                }
                                m2.randnum = randnum;
                                m2.McolletGold = k;
                                m2.randdaoju = "";
                                m2.randyuID = "";

                                int j = 0;
                                string randte = string.Empty;
                                while (j < 10)
                                {
                                    int r = GetRandomNum(10, 3, 25)[j];
                                    randte += Convert.ToString(r);
                                    randte += ",";
                                    j++;
                                }
                                m2.randten = randte;
                                m2.updatetime = DateTime.Now;
                                m2.isrobot = 0;
                                //增加兑换表数据
                                new BCW.bydr.BLL.Cmg_Top().Add(m2);
                                //更新体力值
                                new BCW.bydr.BLL.CmgToplist().Updatevit1(meid, new BCW.bydr.BLL.CmgToplist().Getvit(meid) - 1);
                                //等级更新判断是否捕完一次游戏，等级经验加1    
                                BCW.bydr.Model.CmgToplist model21 = new BCW.bydr.BLL.CmgToplist().GetCmgToplistusID(meid);
                                new BCW.bydr.BLL.CmgToplist().Updatestype(meid, Convert.ToInt32(model21.stype) + 1);
                                //读取最后一条数据的id
                                DataSet rows10 = new BCW.bydr.BLL.Cmg_Top().GetList("Top 1 ID", "YcolletGold=0  order by Time desc");
                                int maxID2 = Convert.ToInt32(rows10.Tables[0].Rows[0][0]);
                                BCW.bydr.Model.Cmg_Top modle4 = new BCW.bydr.BLL.Cmg_Top().GetCmg_Top(maxID2);//读取数据的最后一条id    
                                new BCW.BLL.User().UpdateiGold(meid, -model10.changj2, "捕鱼入场费-标识id" + (modle4.ID));
                                WinnersGold = model10.changj2;
                                new BCW.bydr.BLL.CmgToplist().Updatesid(meid, 0);
                                Utils.Success("", "钓鱼啊钓鱼，钓到一条大鱼。", Utils.getUrl("cmg.aspx?act=buyu&amp;ptype=" + ptype + "") + "", "1");
                            }
                            break;
                        #endregion
                        #region 场景6
                        case 5:
                            {
                                no = ptype + 1;
                                k = R(25, 30);//每个场景价格均有一个唯一对应的值！！！
                                BCW.bydr.Model.CmgDaoju model0 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(no);
                                BCW.bydr.Model.CmgDaoju modelchang6 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(6);
                                DataSet changj6 = new BCW.bydr.BLL.Cmg_Top().GetList("Top 1 ID", "Changj='" + modelchang6.Changjing + "' order by Time desc ");
                                int changid6 = 1;
                                try
                                {
                                    changid6 = Convert.ToInt32(changj6.Tables[0].Rows[0][0]);
                                }
                                catch
                                {
                                }
                                BCW.bydr.Model.Cmg_Top modelchangj6 = new BCW.bydr.BLL.Cmg_Top().GetCmg_Top(changid6);
                                m2.usID = meid;
                                m2.AllcolletGold = 0;
                                m2.Changj = model0.Changjing;
                                m2.ColletGold = 0;
                                m2.DcolletGold = 0;
                                m2.Time = DateTime.Now;
                                m2.YcolletGold = 0;//每个场景n次标识
                                m2.Bid = 1;
                                m2.jID = 1;
                                string[] randgoldnum6 = modelchangj6.randgoldnum.Split(',');
                                if ((model0.Changjing == modelchang6.Changjing && randgoldnum6.Length - 1 == modeldaoju.changj2))
                                {
                                    if ((model0.Changjing == modelchang6.Changjing && modelchangj6.randnum > (Convert.ToInt32(modeldaoju.changj2) - 2)))
                                    {
                                        randnum = 0;
                                        Random ran = new Random();
                                        while (i < modeldaoju.changj2)
                                        {
                                            randgoldnum += Convert.ToString(GetRandomNum(Convert.ToInt32(modeldaoju.changj2), 0, Convert.ToInt32(modeldaoju.changj2) - 1)[i]);
                                            randgoldnum += ",";
                                            i++;
                                        }
                                        m2.randgoldnum = randgoldnum;
                                    }
                                    else
                                    {
                                        randnum = modelchangj6.randnum + 1;
                                        m2.randgoldnum = modelchangj6.randgoldnum;
                                        k = Convert.ToInt32(modelchangj6.McolletGold);
                                    }
                                }
                                else
                                {
                                    randnum = 0;
                                    Random ran = new Random();
                                    while (i < modeldaoju.changj2)
                                    {
                                        randgoldnum += Convert.ToString(GetRandomNum(Convert.ToInt32(modeldaoju.changj2), 0, Convert.ToInt32(modeldaoju.changj2) - 1)[i]);
                                        randgoldnum += ",";
                                        i++;
                                    }
                                    m2.randgoldnum = randgoldnum;
                                }
                                m2.randnum = randnum;
                                m2.McolletGold = k;
                                m2.randdaoju = "";
                                m2.randyuID = "";

                                int j = 0;
                                string randte = string.Empty;
                                while (j < 10)
                                {
                                    int r = GetRandomNum(10, 3, 25)[j];
                                    randte += Convert.ToString(r);
                                    randte += ",";
                                    j++;
                                }
                                m2.randten = randte;
                                m2.updatetime = DateTime.Now;
                                m2.isrobot = 0;
                                //增加兑换表数据
                                new BCW.bydr.BLL.Cmg_Top().Add(m2);
                                //更新体力值
                                new BCW.bydr.BLL.CmgToplist().Updatevit1(meid, new BCW.bydr.BLL.CmgToplist().Getvit(meid) - 1);
                                //等级更新判断是否捕完一次游戏，等级经验加1    
                                BCW.bydr.Model.CmgToplist model21 = new BCW.bydr.BLL.CmgToplist().GetCmgToplistusID(meid);
                                new BCW.bydr.BLL.CmgToplist().Updatestype(meid, Convert.ToInt32(model21.stype) + 1);
                                //读取最后一条数据的id
                                DataSet rows10 = new BCW.bydr.BLL.Cmg_Top().GetList("Top 1 ID", "YcolletGold=0  order by Time desc");
                                int maxID2 = Convert.ToInt32(rows10.Tables[0].Rows[0][0]);
                                BCW.bydr.Model.Cmg_Top modle4 = new BCW.bydr.BLL.Cmg_Top().GetCmg_Top(maxID2);//读取数据的最后一条id    
                                new BCW.BLL.User().UpdateiGold(meid, -model10.changj2, "捕鱼入场费-标识id" + (modle4.ID));
                                WinnersGold = model10.changj2;
                                new BCW.bydr.BLL.CmgToplist().Updatesid(meid, 0);
                                Utils.Success("", "钓鱼啊钓鱼，钓到一条大鱼。", Utils.getUrl("cmg.aspx?act=buyu&amp;ptype=" + ptype + "") + "", "1");
                            }
                            break;
                            #endregion
                    }
                    //活跃抽奖入口_20160621姚志光
                    try
                    {
                        //表中存在记录
                        if (new BCW.BLL.tb_WinnersGame().ExistsGameName("捕鱼达人"))
                        {
                            //投注是否大于设定的限额，是则有抽奖机会
                            if (WinnersGold > new BCW.BLL.tb_WinnersGame().GetPrice("捕鱼达人"))
                            {
                                string mename = new BCW.BLL.User().GetUsName(meid);
                                string TextForUbb = (ub.GetSub("TextForUbb", "/Controls/winners.xml"));//设置内线提示的文字
                                string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", "/Controls/winners.xml"));//1发内线2不发内线 
                                int hit = new BCW.winners.winners().CheckActionForAll(1, 1, meid, mename, "捕鱼", 3);
                                if (hit == 1)
                                {
                                    //内线开关 1开
                                    if (WinnersGuessOpen == "1")
                                    {
                                        //发内线到该ID
                                        new BCW.BLL.Guest().Add(0, meid, mename, TextForUbb);
                                    }
                                }
                            }
                        }
                    }
                    catch { }
                }
            }
            else
            {
                Utils.Error("您不能同时进入不同的场景！", "");
            }
        }
    }
    #endregion


    #region 进入捕鱼场景时的提醒 buyujudgePage
    /// <summary>
    /// 进入捕鱼场景时的提醒
    /// </summary>
    private void buyujudgePage()
    {
        #region 导航
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx") + "\">捕鱼</a>");
        Master.Title = "鱼儿上钩中。。。";
        builder.Append(Out.Tab("</div>", "<br />"));
        #endregion

        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 1, @"^[0-5]$", "0"));

        BCW.bydr.Model.CmgDaoju model0 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(ptype + 1);
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        DataSet rows = new BCW.BLL.User().GetList("VipDate", "ID=" + meid);
        DateTime viptime = DateTime.Now;
        try
        {
            viptime = Convert.ToDateTime(rows.Tables[0].Rows[0][0]);
        }
        catch
        {
            viptime = Convert.ToDateTime(bydrBotTime);
        }
        if (DateTime.Now < viptime)
        {
            #region VIP处理
            if (model0.changj2 == 200)
            {
                #region 进入200场景的提示处理
                string swhere = DateTime.Now.ToString("yyyy-MM-dd");
                bool viptime1 = new BCW.bydr.BLL.CmgToplist().Existsusvip(meid, swhere);
                if (!viptime1)
                {
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("将要花费" + model0.changj2 + "" + ub.Get("SiteBz") + "进入" + model0.Changjing + "<br />");
                    builder.Append("" + "钓鱼啊钓鱼，祝您钓到大鱼！<br />" + "<a href=\"" + Utils.getUrl("cmg.aspx?act=Wait&amp;id=1&amp;ptype=" + ptype + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">开始钓鱼</a><br />");
                    builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx") + "\">再看看吧</a>-");
                    builder.Append(Out.Tab("</div>", ""));
                    DUBBPage();

                }
                else
                {
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("将要花费" + model0.changj2 + "" + ub.Get("SiteBz") + "进入" + model0.Changjing + "<br />");
                    builder.Append("" + "钓鱼啊钓鱼，祝您钓到大鱼！<br />" + "<a href=\"" + Utils.getUrl("cmg.aspx?act=Wait&amp;id=1&amp;ptype=" + ptype + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">开始钓鱼</a><br />");

                    builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx") + "\">再看看吧</a>-");
                    builder.Append(Out.Tab("</div>", ""));
                    DUBBPage();
                }
                #endregion
            }
            else
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("将要花费" + model0.changj2 + "" + ub.Get("SiteBz") + "进入" + model0.Changjing + "<br />");
                builder.Append("" + "钓鱼啊钓鱼，祝您钓到大鱼！<br />" + "<a href=\"" + Utils.getUrl("cmg.aspx?act=Wait&amp;id=1&amp;ptype=" + ptype + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">开始钓鱼</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx") + "\">再看看吧</a>-");
                builder.Append(Out.Tab("</div>", ""));
                DUBBPage();
            }
            #endregion
        }
        else
        {
            #region 非VIP处理
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("将要花费" + model0.changj2 + "" + ub.Get("SiteBz") + "进入" + model0.Changjing + "<br />");
            builder.Append("" + "钓鱼啊钓鱼，祝您钓到大鱼！<br />" + "<a href=\"" + Utils.getUrl("cmg.aspx?act=Wait&amp;id=1&amp;ptype=" + ptype + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">开始钓鱼</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("cmg.aspx") + "\">再看看吧</a>-");
            builder.Append(Out.Tab("</div>", ""));
            DUBBPage();
            #endregion
        }
    }
    #endregion

    #region 随机函数 R
    /// <summary>
    /// 随机函数
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    protected int R(int x, int y)
    {
        Random ran = new Random();
        int RandKey = ran.Next(x, y);
        return RandKey;
    }
    #endregion
    //生成随机不重复的数
    private int[] GetRandomNum(int n, int min, int max)
    {
        Dictionary<int, int> dict = new Dictionary<int, int>();
        Random r = new Random();
        int[] result = new int[n];
        int num = 0;
        for (int i = 0; i < n; i++)
        {
            num = r.Next(min, max + 1);
            while (dict.ContainsKey(num))
            {
                num = r.Next(min, max + 1);
            }
            dict.Add(num, 1);
        }
        dict.Keys.CopyTo(result, 0);
        return result;
    }

    //根据随机数的权值算出每条鱼的价格
    private int[] num(string m, int x)
    {
        string[] randten = m.Split(',');
        int i = 0;
        int n = 0;
        int[] num1 = new int[10];
        for (i = 0; i < randten.Length - 1; i++)
            n += Convert.ToInt32(randten[i]);//数组求和
        int k = 0;
        for (i = 0; i < 9; i++)
        {

            double m1 = 0;
            m1 = Convert.ToDouble(randten[i]) / n;//数组中的权值
            k += Convert.ToInt32(m1 * x);
            num1[i] = Convert.ToInt32(m1 * x);
        }
        num1[9] = x - k;//最后一个价格
        return num1;
    }
}
