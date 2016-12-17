using System;
using System.Data;
using BCW.Common;
using System.Collections.Generic;

/// <summary>
/// 蒙宗将20160902 增加马上兑奖链接
/// 蒙宗将 20160906 排版开奖单双出错修改
/// 蒙宗将 20160908 增加龙虎配赔率与手动开奖龙虎计算
/// 蒙宗将 20160913 修复连二组选算法（9.22 修）
/// 蒙宗将 20160930 赔率调整
/// 蒙宗将 20161007 增加未|已兑奖
/// 蒙宗将 20161010 增加重置游戏权限
/// 蒙宗将 20161011 增加退还功能，修改赔率返奖出错
/// 蒙宗将 20161019 排行榜增加用户ID
/// 蒙宗将 20161111 状态修复
/// 蒙宗将 20161118 返赢反负修复
///        20161121 盈利分析排除系统号
/// </summary>

public partial class Manage_game_klsf : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/klsf.xml";
    protected int gid = 32;
    protected string GameName = ub.GetSub("klsfName", "/Controls/klsf.xml");

    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "top":
                TopPage();
                break;
            case "back":
                BackPage();
                break;
            case "backsave":
            case "backsave2":
                BackSavePage(act);
                break;
            case "backsave1":
            case "backsave3":
                BackSave1Page(act);
                break;
            case "view":
                ViewPage();
                break;
            case "reset":
                ResetPage();
                break;
            case "stat":
                StatPage();
                break;
            case "open":
                OpenPage();
                break;
            case "addStage":
                AddNewStage();
                break;
            case "rewards":
                RewardPage();
                break;
            case "chaxun":
                ChaxunPage();
                break;
            case "usidcx":
                UsidcxPage();
                break;
            case "jiangcx":
                JiangcxPage();
                break;
            case "tstate":
                TimeStatPage();
                break;
            case "shezhi":
                ShezhiPage();
                break;
            case "setceshi":
                SetStatueCeshi();//内测设置
                break;
            case "case":
                CasePage();//未/已兑奖
                break;
            case "caseok":
                CaseokPage();//帮他兑奖
                break;
            case "del":
                DelPage();//退还
                break;
            case "backerror":
                backerror();//反负出错扣回
                break;
            case "backmessage":
                BackMessagePage();//返赢返负记录
                break;
            default:
                ReloadPage();
                break;
        }
    }

    /// <summary>
    /// 主要管理页面
    /// </summary>
    private void ReloadPage()
    {
        Master.Title = "" + GameName + "管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        builder.Append(GameName);
        builder.Append(Out.Tab("</div>", "<br />"));

        //builder.Append(Out.Tab("<div>", ""));
        //builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=addStage") + "\">手动添加新一期</a>");
        //builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        strWhere = "";
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        // 开始读取列表
        IList<BCW.Model.klsflist> listSSClist = new BCW.BLL.klsflist().Getklsflists(pageIndex, pageSize, strWhere, out recordCount);
        if (listSSClist.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.klsflist n in listSSClist)
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

                if (n.State == 0)
                {
                    builder.Append("第" + n.klsfId + "期开出:<a href=\"" + Utils.getUrl("klsf.aspx?act=view&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">未开</a>");
                    builder.Append("|<a href=\"" + Utils.getUrl("klsf.aspx?act=open&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">开奖</a>");
                }
                else
                    builder.Append("第" + n.klsfId + "期开出:<a href=\"" + Utils.getUrl("klsf.aspx?act=view&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.Result + "</a>");
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

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=shezhi") + "\">游戏配置</a><br />");
        builder.Append("<a href =\"" + Utils.getUrl("klsf.aspx?act=setceshi") + "\">测试配置</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=top") + "\">游戏排行</a><br />");
        builder.Append("<a href =\"" + Utils.getUrl("klsf.aspx?act=chaxun") + "\">游戏查询</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=stat") + "\">赢利分析</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=back") + "\">返赢返负</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=reset") + "\">重置游戏</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

    }
    /// <summary>
    /// 游戏设置
    /// </summary>
    private void ShezhiPage()
    {

        Master.Title = "" + GameName + "游戏配置";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx") + "\">" + GameName + "</a>");
        builder.Append("&gt;游戏配置");
        builder.Append(Out.Tab("</div>", "<br />"));

        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-2]$", "0"));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/klsf.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            if (ptype == 0)
            {
                string Name = Utils.GetRequest("Name", "post", 2, @"^[^\^]{1,20}$", "系统名称限1-20字内");
                string Notes = Utils.GetRequest("Notes", "post", 3, @"^[^\^]{1,5000}$", "口号限5000字内");
                string Logo = Utils.GetRequest("Logo", "post", 3, @"^[^\^]{1,200}$", "Logo地址限200字内");
                string Status = Utils.GetRequest("Status", "post", 2, @"^[0-2]$", "系统状态选择出错");
                string Sec = Utils.GetRequest("Sec", "post", 2, @"^[0-9]\d*$", "秒数填写出错");
                string SmallPay = Utils.GetRequest("SmallPay", "post", 2, @"^[0-9]\d*$", "最小下注" + ub.Get("SiteBz") + "填写错误");
                string BigPay = Utils.GetRequest("BigPay", "post", 2, @"^[0-9]\d*$", "最大下注" + ub.Get("SiteBz") + "填写错误");
                string Price = Utils.GetRequest("Price", "post", 2, @"^[0-9]\d*$", "每期每ID限购多少" + ub.Get("SiteBz") + "填写错误");
                string Expir = Utils.GetRequest("Expir", "post", 2, @"^[0-9]\d*$", "防刷秒数填写出错");
                string OnTime = Utils.GetRequest("OnTime", "post", 3, @"^[0-9]{2}:[0-9]{2}-[0-9]{2}:[0-9]{2}$", "游戏开放时间填写错误，可留空");
                string Foot = Utils.GetRequest("Foot", "post", 3, @"^[\s\S]{1,2000}$", "底部Ubb限2000字内");
                string Rule = Utils.GetRequest("Rule", "post", 3, @"^[\s\S]{1,5000}$", "规则最限5000字内");
                string GuestSet = Utils.GetRequest("GuestSet", "post", 2, @"^[0-1]$", "兑奖内线选择出错");

                //继续验证时间
                if (OnTime != "")
                {
                    string[] temp = OnTime.Split("-".ToCharArray());
                    DateTime dt1 = Utils.ParseTime(temp[0]);
                    DateTime dt2 = Utils.ParseTime(temp[1]);
                }
                xml.dss["klsfName"] = Name;
                xml.dss["klsfNotes"] = Notes;
                xml.dss["klsfLogo"] = Logo;
                xml.dss["klsfStatus"] = Status;
                xml.dss["klsfSec"] = Sec;
                xml.dss["klsfSmallPay"] = SmallPay;
                xml.dss["klsfBigPay"] = BigPay;
                xml.dss["klsfPrice"] = Price;
                xml.dss["klsfExpir"] = Expir;
                xml.dss["klsfOnTime"] = OnTime;
                xml.dss["klsfFoot"] = Foot;
                xml.dss["klsfRule"] = Rule;
                xml.dss["klsfGuestSet"] = GuestSet;


            }
            if (ptype == 1)
            {
                for (int i = 1; i <= 25; i++)
                {
                    string Odds = "";
                    Odds = Utils.GetRequest("Odds" + i + "", "all", 2, @"^[0-9]\d*\.?\d{0,2}$", "赔率错误");

                    xml.dss["klsfOdds" + i + ""] = Odds;
                }
                string Oddschishi = Utils.GetRequest("Oddschushi", "all", 2, @"^[0-9]\d*\.?\d{0,2}$", "赔率错误");
                string Oddsmax = Utils.GetRequest("Oddsmax", "all", 2, @"^[0-9]\d*\.?\d{0,2}$", "赔率错误");
                string Oddsmin = Utils.GetRequest("Oddsmin", "all", 2, @"^[0-9]\d*\.?\d{0,2}$", "赔率错误");
                xml.dss["klsfOddschushi"] = Oddschishi;
                xml.dss["klsfOddsmax"] = Oddsmax;
                xml.dss["klsfOddsmin"] = Oddsmin;
            }
            if (ptype == 2)
            {
                for (int i = 1; i <= 18; i++)
                {
                    string Oddsc = "";
                    Oddsc = Utils.GetRequest("Oddsc" + i + "", "all", 2, @"^[0-9]\d*$", "下注方式上限错误" + i + "");

                    xml.dss["klsfOddsc" + i + ""] = Oddsc;
                }
            }

            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("" + GameName + "设置", "设置成功，正在返回..", Utils.getUrl("klsf.aspx?act=shezhi&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            if (ptype == 0)
            {
                builder.Append("设置");
                builder.Append("|<a href=\"" + Utils.getUrl("klsf.aspx?act=shezhi&amp;ptype=1") + "\">赔率</a>");
                builder.Append("|<a href=\"" + Utils.getUrl("klsf.aspx?act=shezhi&amp;ptype=2") + "\">投注方式投注额上限</a>");
            }
            if (ptype == 1)
            {
                builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=shezhi&amp;ptype=0") + "\">设置</a>");
                builder.Append("|赔率");
                builder.Append("|<a href=\"" + Utils.getUrl("klsf.aspx?act=shezhi&amp;ptype=2") + "\">投注方式投注额上限</a>");
            }
            if (ptype == 2)
            {
                builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=shezhi&amp;ptype=0") + "\">设置</a>");
                builder.Append("|<a href=\"" + Utils.getUrl("klsf.aspx?act=shezhi&amp;ptype=1") + "\">赔率</a>");
                builder.Append("|投注方式投注额上限");
            }

            builder.Append(Out.Tab("</div>", ""));
            if (ptype == 0)
            {
                string strText = "游戏名称:/,游戏口号(可留空):/,游戏Logo(可留空):/,游戏状态:/,离截止时间N秒前不能下注/,最小下注" + ub.Get("SiteBz") + ":/,最大下注" + ub.Get("SiteBz") + ":/,每期每ID限购多少" + ub.Get("SiteBz") + "(填0则不限制):/,下注防刷(秒):/,游戏开放时间(可留空):/,底部Ubb:/,游戏规则:/,兑奖内线:/,";
                string strName = "Name,Notes,Logo,Status,Sec,SmallPay,BigPay,Price,Expir,OnTime,Foot,Rule,GuestSet,backurl";
                string strType = "text,text,text,select,num,num,num,num,num,text,textarea,textarea,select,hidden";
                string strValu = "" + xml.dss["klsfName"] + "'" + xml.dss["klsfNotes"] + "'" + xml.dss["klsfLogo"] + "'" + xml.dss["klsfStatus"] + "'" + xml.dss["klsfSec"] + "'" + xml.dss["klsfSmallPay"] + "'" + xml.dss["klsfBigPay"] + "'" + xml.dss["klsfPrice"] + "'" + xml.dss["klsfExpir"] + "'" + xml.dss["klsfOnTime"] + "'" + xml.dss["klsfFoot"] + "'" + xml.dss["klsfRule"] + "'" + xml.dss["klsfGuestSet"] + "'" + Utils.getPage(0) + "";
                string strEmpt = "false,true,true,0|正常|1|维护|2|内测,false,false,false,false,false,true,true,true,0|开启|1|关闭,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,klsf.aspx?act=shezhi,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                //builder.Append(Out.Tab("<div>", "<br />"));
                //builder.Append("温馨提示:<br />游戏开放时间填写格式为:09:00-18:00，留空则全天开放.");
                //builder.Append(Out.Tab("</div>", ""));
            }
            if (ptype == 1)
            {
                string strText = "任选五胆拖:,任选五普通:,任选四胆拖:,任选四普通:,任选三胆拖:,任选三普通:,任选二胆拖:,任选二普通:,连二直选:,连二组选:,前一红投:,前一数投:,大:,小:,单:,双:,龙（1与8位）:/,虎（1与8位）:/,龙（2与7位）:/,虎（2与7位）:/,龙（3与6位）:/,虎（3与6位）:/,龙（4与5位）:/,虎（4与5位）:/,浮动赔率:,初始赔率:,上限赔率:,下限赔率:,,";
                string strName = "Odds1,Odds2,Odds3,Odds4,Odds5,Odds6,Odds7,Odds8,Odds9,Odds10,Odds11,Odds12,Odds13,Odds14,Odds15,Odds16,Odds18,Odds19,Odds20,Odds21,Odds22,Odds23,Odds24,Odds25,Odds17,Oddschushi,Oddsmax,Oddsmin,ptype,backurl";
                string strType = "text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,hidden,hidden";
                string strValu = "" + xml.dss["klsfOdds1"] + "'" + xml.dss["klsfOdds2"] + "'" + xml.dss["klsfOdds3"] + "'" + xml.dss["klsfOdds4"] + "'" + xml.dss["klsfOdds5"] + "'" + xml.dss["klsfOdds6"] + "'" + xml.dss["klsfOdds7"] + "'" + xml.dss["klsfOdds8"] + "'" + xml.dss["klsfOdds9"] + "'" + xml.dss["klsfOdds10"] + "'" + xml.dss["klsfOdds11"] + "'" + xml.dss["klsfOdds12"] + "'" + xml.dss["klsfOdds13"] + "'" + xml.dss["klsfOdds14"] + "'" + xml.dss["klsfOdds15"] + "'" + xml.dss["klsfOdds16"] + "'" + xml.dss["klsfOdds18"] + "'" + xml.dss["klsfOdds19"] + "'" + xml.dss["klsfOdds20"] + "'" + xml.dss["klsfOdds21"] + "'" + xml.dss["klsfOdds22"] + "'" + xml.dss["klsfOdds23"] + "'" + xml.dss["klsfOdds24"] + "'" + xml.dss["klsfOdds25"] + "'" + xml.dss["klsfOdds17"] + "'" + xml.dss["klsfOddschushi"] + "'" + xml.dss["klsfOddsmax"] + "'" + xml.dss["klsfOddsmin"] + "'1'" + Utils.getPage(1) + "";
                string strEmpt = "false,false,false,false,false,false,false,false,false,false,false,false,false,false,,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,klsf.aspx?act=shezhi,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
            if (ptype == 2)
            {
                string strText = "任选五胆拖:,任选五普通:,任选四胆拖:,任选四普通:,任选三胆拖:,任选三普通:,任选二胆拖:,任选二普通:,连二直选:,连二组选:,前一红投:,前一数投:,大:,小:,单:,双:,龙:/,虎:/,,";
                string strName = "Oddsc1,Oddsc2,Oddsc3,Oddsc4,Oddsc5,Oddsc6,Oddsc7,Oddsc8,Oddsc9,Oddsc10,Oddsc11,Oddsc12,Oddsc13,Oddsc14,Oddsc15,Oddsc16,Oddsc17,Oddsc18,ptype,backurl";
                string strType = "text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,hidden,hidden";
                string strValu = "" + xml.dss["klsfOddsc1"] + "'" + xml.dss["klsfOddsc2"] + "'" + xml.dss["klsfOddsc3"] + "'" + xml.dss["klsfOddsc4"] + "'" + xml.dss["klsfOddsc5"] + "'" + xml.dss["klsfOddsc6"] + "'" + xml.dss["klsfOddsc7"] + "'" + xml.dss["klsfOddsc8"] + "'" + xml.dss["klsfOddsc9"] + "'" + xml.dss["klsfOddsc10"] + "'" + xml.dss["klsfOddsc11"] + "'" + xml.dss["klsfOddsc12"] + "'" + xml.dss["klsfOddsc13"] + "'" + xml.dss["klsfOddsc14"] + "'" + xml.dss["klsfOddsc15"] + "'" + xml.dss["klsfOddsc16"] + "'" + xml.dss["klsfOddsc17"] + "'" + xml.dss["klsfOddsc18"] + "'2'" + Utils.getPage(1) + "";
                string strEmpt = "false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,klsf.aspx?act=shezhi,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("说明：【投注方式投注额上限】是当期当投注方式所有投注最大限额");
                builder.Append(Out.Tab("</div>", ""));
            }

            builder.Append(Out.Tab("<div>", Out.Hr()));
            if (ptype == 1)
            {
                builder.Append("温馨提示：大小单双龙虎赔率是实时赔率，它们都是在初始赔率上根据浮动赔率更新的，如果赔率超过上限赔率，那么它们的赔率将取上限赔率，下限赔率同理<br />");
            }
            builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx") + "\">返回上一级</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));

        }
    }

    //游戏内测管理
    private void SetStatueCeshi()
    {
        Master.Title = "" + GameName + "设置测试状态";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx") + "\">" + GameName + "管理</a>&gt;");
        builder.Append("设置测试状态");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b>" + GameName + "内测管理：</b>");
        builder.Append(Out.Tab("</div>", "<br />"));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/klsf.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string klsfStatus = Utils.GetRequest("klsfStatus", "post", 2, @"^[0-9]\d*$", "测试权限管理输入出错");
            string klsfDemoIDS = Utils.GetRequest("klsfDemoIDS", "all", 2, @"^[^\^]{1,2000}$", "请输入测试号");
            xml.dss["klsfStatus"] = klsfStatus;
            xml.dss["klsfDemoIDS"] = klsfDemoIDS.Replace("\r\n", "").Replace(" ", "");
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("" + GameName + "设置", "内测设置成功，正在返回..", Utils.getUrl("klsf.aspx?act=setceshi"), "2");
        }
        else
        {
            string strText = "测试权限管理:/,添加测试号(多账号用#号分隔):/,";
            string strName = "klsfStatus,klsfDemoIDS,backurl";
            string strType = "select,textarea,hidden";
            string strValu = xml.dss["klsfStatus"] + "'" + xml.dss["klsfDemoIDS"] + "'" + Utils.getPage(0) + "";
            string strEmpt = "0|开放|1|维护|2|内测,true";
            string strIdea = "/";
            string strOthe = "确定修改,klsf.aspx?act=setceshi,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            string klsfDemoIDS = Convert.ToString(ub.GetSub("klsfDemoIDS", xmlPath));
            string[] name = klsfDemoIDS.Split('#');
            // foreach (string n in imgNum)
            builder.Append("当前测试号:<br />");
            for (int n = 0; n < name.Length; n++)
            {
                if ((n + 1) % 5 == 0)
                {
                    builder.Append(name[n] + "," + "<br />");
                }
                else
                {
                    builder.Append(name[n] + ",");
                }
            }
            builder.Append(Out.Tab("</div>", "<br/>"));


            builder.Append(Out.Tab("<div>", ""));
            builder.Append("" + "-----------" + "<br />");
            builder.Append(Out.Tab("</div>", ""));

            string ac1 = Utils.GetRequest("ac", "all", 1, "", "");
            Application.Remove(xmlPath);//清缓存
            xml.ReloadSub(xmlPath); //加载配置
            if (Utils.ToSChinese(ac1) == "确定设置")
            {
                string klsfRobotId = Utils.GetRequest("klsfRobotId", "post", 1, "", xml.dss["klsfRobotId"].ToString());
                string klsfIsBot = Utils.GetRequest("klsfIsBot", "post", 1, @"^[0-1]$", xml.dss["klsfIsBot"].ToString());
                string klsfRobotBuyCent = Utils.GetRequest("klsfRobotBuyCent", "post", 1, "", xml.dss["klsfRobotBuyCent"].ToString());
                string klsfRobotBuy = Utils.GetRequest("klsfRobotBuy", "post", 1, @"^[0-9]\d*$", xml.dss["klsfRobotBuy"].ToString());
                xml.dss["klsfRobotId"] = klsfRobotId.Replace("\r\n", "").Replace(" ", "");
                xml.dss["klsfIsBot"] = klsfIsBot;
                xml.dss["klsfRobotBuyCent"] = klsfRobotBuyCent.Replace("\r\n", "").Replace(" ", "");
                xml.dss["klsfRobotBuy"] = klsfRobotBuy;
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                Utils.Success("" + GameName + "设置", "机器人管理成功，正在返回..", Utils.getUrl("klsf.aspx?act=setceshi"), "2");
            }
            else
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<b>机器人管理：</b>");
                builder.Append(Out.Tab("</div>", "<br />"));

                string strText1 = "机器人ID(多个机器人用#号分隔):/,机器人状态:/,机器人投注金额设置:/,机器人每期购买次数:/";
                string strName1 = "klsfRobotId,klsfIsBot,klsfRobotBuyCent,klsfRobotBuy";
                string strType1 = "textarea,select,text,text";
                string strValu1 = xml.dss["klsfRobotId"].ToString() + "'" + xml.dss["klsfIsBot"].ToString() + "'" + xml.dss["klsfRobotBuyCent"].ToString() + "'" + xml.dss["klsfRobotBuy"].ToString();
                string strEmpt1 = "true,0|关闭|1|开启,true,false";
                string strIdea1 = "/";
                string strOthe1 = "确定设置,klsf.aspx?act=setceshi,post,1,red";
                builder.Append(Out.wapform(strText1, strName1, strType1, strValu1, strEmpt1, strIdea1, strOthe1));
                builder.Append(Out.Tab("<div>", "<br />"));

                string klsfRobotIDS = Convert.ToString(ub.GetSub("klsfRobotId", xmlPath));
                string[] name1 = klsfRobotIDS.Split('#');
                string name2 = string.Empty;
                for (int n = 0; n < name1.Length; n++)
                {
                    if ((n + 1) % 5 == 0)
                        name2 = name2 + name1[n] + ",<br />";
                    else
                        name2 = name2 + name1[n] + ",";
                }
                builder.Append("当前机器人ID为：<br />" + name2 + "<br />");
                if (xml.dss["klsfIsBot"].ToString() == "0")
                {
                    builder.Append("机器人状态：<b style=\"color:red\">关闭</b><br />");
                }
                else
                {
                    builder.Append("当前机器人状态：<b style=\"color:red\">开启</b><br />");
                }
                builder.Append("当前机器人单注投注金额(随机投注)：<b style=\"color:red\">" + xml.dss["klsfRobotBuyCent"].ToString() + "</b><br />");
                builder.Append("当前机器人每期限购买彩票次数：<b style=\"color:red\">" + xml.dss["klsfRobotBuy"].ToString() + "</b><br />");
                builder.Append("<b>温馨提示:请注意内测设置与机器人设置分开设置</b>");
                builder.Append(Out.Tab("</div>", "<br />"));
            }

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("" + "-----------" + "<br />");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx") + "\">返回" + GameName + "管理</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
    /// <summary>
    /// 排行榜
    /// </summary>
    private void TopPage()
    {
        Master.Title = "" + GameName + "排行榜";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx") + "\">" + GameName + "</a>");
        builder.Append("&gt;排行榜");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("" + GameName + "排行榜");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "Prices>0 and State>0";
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        // 开始读取列表
        IList<BCW.Model.klsfpay> listSSCpay = new BCW.BLL.klsfpay().GetklsfpaysTop(pageIndex, pageSize, strWhere, out recordCount);
        if (listSSCpay.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.klsfpay n in listSSCpay)
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
                #region 根据版本选择快乐币和酷币的获取和显示
                int IsSWB = Utils.ParseInt(ub.GetSub("klsfSWB", xmlPath));
                if (IsSWB == 0)
                {
                    builder.Append("[第" + ((pageIndex - 1) * pageSize + k) + "名]<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(n.UsID) + "</a>赢" + n.WinCent + "快乐币");
                }
                else
                {
                    builder.Append("[第" + ((pageIndex - 1) * pageSize + k) + "名]<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(n.UsID) + "(" + n.UsID + ")</a>赢" + n.WinCent + "" + ub.Get("SiteBz") + "");
                }
                #endregion

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
        builder.Append(Out.Tab("</div>", "<br />"));
        string strText = "开始时间:/,结束时间:/,,";
        string strName = "sTime,oTime,State,act";
        string strType = "date,date,hidden,hidden";
        string strValu = "" + DT.FormatDate(DateTime.Now.AddDays(-10), 0) + "'" + DT.FormatDate(DateTime.Now, 0) + "'first'rewards";
        string strEmpt = "false,false,false,false";
        string strIdea = "/";
        string strOthe = "时间查询排行,klsf.aspx,post,1,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("温馨提示：查询排行可对排行前十用户手动发放奖励<br />");
        builder.Append("<a href=\"" + Utils.getPage("klsf.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    ///<summary>
    ///未/已兑奖
    /// </summary>
    private void CasePage()
    {
        Master.Title = "" + GameName + "兑奖查看";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx") + "\">" + GameName + "</a>");
        builder.Append("&gt;兑奖查看");
        builder.Append(Out.Tab("</div>", "<br />"));

        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-1]$", "0"));
        //用户ID
        int usid = int.Parse(Utils.GetRequest("usid", "all", 1, @"^[1-9]\d*$", "0"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=chaxun") + "\">期号查询</a>" + "|<a href=\"" + Utils.getUrl("klsf.aspx?act=usidcx") + "\">会员查询</a>" + "|<a href=\"" + Utils.getUrl("klsf.aspx?act=jiangcx") + "\">往期分析</a>|查兑奖");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("〓查兑奖〓");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        if (ptype == 0)
        {
            builder.Append("未兑奖 | <a href=\"" + Utils.getUrl("klsf.aspx?act=case&amp;ptype=1&amp;usid=" + usid + "") + "\">已兑奖</a>");
        }
        else
        {
            builder.Append(" <a href=\"" + Utils.getUrl("klsf.aspx?act=case&amp;ptype=0&amp;usid=" + usid + "") + "\">未兑奖</a> | 已兑奖");
        }
        builder.Append(Out.Tab("</div>", "<br />"));


        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        if (ptype == 0)
        {
            if (usid == 0)
                strWhere += " State=1 and WinCent >0 ";
            else
                strWhere += " State=1 and UsID=" + usid + " and WinCent >0 ";
        }
        else
        {
            if (usid == 0)
                strWhere += " State=2 ";
            else
                strWhere += " State=2 and UsID=" + usid + " ";
        }
        string[] pageValUrl = { "act", "ptype", "usid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        string SSCqi = "";
        // 开始读取列表
        IList<BCW.Model.klsfpay> listSSCpay = new BCW.BLL.klsfpay().Getklsfpays(pageIndex, pageSize, strWhere, out recordCount);
        if (listSSCpay.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.klsfpay n in listSSCpay)
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
                if (n.klsfId.ToString() != SSCqi)
                {
                    if (n.Result == "")
                        builder.Append("=第" + n.klsfId + "期=<br />");
                    else
                        builder.Append("=第" + n.klsfId + "期=开出:<f style=\"color:red\">" + n.Result + "|和:" + GetHe(n.Result) + "</f><br />");
                }

                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<b>" + ((pageIndex - 1) * pageSize + k) + ".</b>");
                builder.Append("<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(n.UsID) + "</a> <b>[" + OutType(n.Types) + "]</b>位号:" + n.Notes + "/每注" + n.Price + "" + ub.Get("SiteBz") + "/共" + n.iCount + "注/赔率" + n.Odds + "[" + DT.FormatDate(n.AddTime, 1) + "]");

                if (n.WinCent > 0)
                {
                    builder.Append("中" + n.iWin + "注/赢" + n.WinCent + "" + ub.Get("SiteBz") + "");
                    if (ptype == 0)
                    {
                        builder.Append("|<f style=\"color:blue\"><a href=\"" + Utils.getUrl("klsf.aspx?act=caseok&amp;id=" + n.ID + "") + "\">帮他兑奖</a></f>");
                    }
                    else
                    {
                        builder.Append("|<f style=\"color:black\">已兑奖</f>");
                    }
                }
                builder.Append(Out.Tab("</div>", ""));
                SSCqi = n.klsfId.ToString();
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

        builder.Append(Out.Tab("<div class=\"hr\"></div>", "<br />-----------"));

        string strText = "用户ID查询:/,";
        string strName = "usid,act";
        string strType = "num,hidden";
        string strValu = "" + usid + "'" + Utils.getPage(0) + "";
        string strEmpt = "true,true,false";
        string strIdea = "/";
        string strOthe = "确认搜索,klsf.aspx?act=case&amp;ptype=" + ptype + ",post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("温馨提示：查询用户ID为0时，默认查询所有用户记录<br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    ///<summary>
    ///帮他兑奖
    /// </summary>
    private void CaseokPage()
    {
        Master.Title = "" + GameName + "帮他兑奖";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx") + "\">" + GameName + "</a>");
        builder.Append("&gt;帮他兑奖");
        builder.Append(Out.Tab("</div>", "<br />"));

        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[1-9]\d*$", "0"));
        string info = Utils.GetRequest("info", "all", 1, "", "");
        BCW.Model.klsfpay n = new BCW.BLL.klsfpay().Getklsfpay(id);

        if (info == "ok")
        {
            if (new BCW.BLL.klsfpay().ExistsState(id, n.UsID))
            {
                int guestset = Utils.ParseInt(ub.GetSub("klsfStatus", xmlPath));
                new BCW.BLL.klsfpay().UpdateState(id, 2);

                BCW.User.Users.IsFresh("klsf", 1);//防刷
                BCW.Model.klsflist idd = new BCW.BLL.klsflist().Getklsflistbyklsfid(n.klsfId);
                new BCW.BLL.User().UpdateiGold(n.UsID, new BCW.BLL.User().GetUsName(n.UsID), n.WinCent, "" + GameName + "兑奖-" + "[url=./game/klsf.aspx?act=view&amp;id=" + idd.ID + "&amp;ptype=2]" + n.klsfId + "[/url]" + "-标识ID" + n.ID + "");
                if (new BCW.BLL.User().GetIsSpier(n.UsID) != 1)
                    new BCW.BLL.User().UpdateiGold(105, new BCW.BLL.User().GetUsName(105), -n.WinCent, "ID:[url=forumlog.aspx?act=xview&amp;uid=" + n.UsID + "]" + n.UsID + "[/url]" + GameName + "第[url=./game/klsf.aspx?act=view&amp;id=" + idd.ID + "&amp;ptype=2]" + n.klsfId + "[/url]期兑奖" + n.WinCent + "|(标识ID" + n.ID + ")");
                if (guestset == 0)
                    new BCW.BLL.Guest().Add(1, n.UsID, BCW.User.Users.SetUser(n.UsID), "您在[URL=/bbs/game/klsf.aspx]" + GameName + "[/URL]第" + n.klsfId + "期的投注：" + n.Notes + "系统已经帮您兑奖，获得了" + n.WinCent + ub.Get("SiteBz") + "。");//开奖提示信息,1表示开奖信息
                Utils.Success("兑奖", "恭喜，成功帮他兑奖" + n.WinCent + "" + ub.Get("SiteBz") + "", Utils.getUrl("klsf.aspx?act=case&amp;ptype=0"), "1");

            }
            else
            {
                Utils.Success("兑奖", "重复兑奖或没有可以兑奖的记录", Utils.getUrl("klsf.aspx?act=case&amp;ptype=0"), "1");
            }
        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("=第" + n.klsfId + "期=开出:<f style=\"color:red\">" + n.Result + "|和:" + GetHe(n.Result) + "</f><br />");

            builder.Append("用户：<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(n.UsID) + " (" + n.UsID + ")" + "</a><br /> ");
            builder.Append("兑奖ID：" + id + "<br />投注方式：<b>[" + OutType(n.Types) + "]</b>位号:" + n.Notes + "<br />每注：" + n.Price + "" + ub.Get("SiteBz") + "<br />共投：" + n.iCount + "注<br />赔率：" + n.Odds + "<br />投注时间：" + DT.FormatDate(n.AddTime, 1) + "<br />");

            if (n.WinCent > 0)
            {
                builder.Append("中奖：" + n.iWin + "注/赢" + n.WinCent + "" + ub.Get("SiteBz") + "");

                builder.Append("<br /><a href=\"" + Utils.getUrl("klsf.aspx?act=caseok&amp;info=ok&amp;id=" + n.ID + "") + "\">确定帮他兑奖</a>");
            }
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    /// <summary>
    /// 排行榜奖励
    /// </summary>
    private void RewardPage()
    {
        Master.Title = "" + GameName + "排行查询";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx") + "\">" + GameName + "</a>");
        builder.Append("&gt;排行查询结果");
        builder.Append(Out.Tab("</div>", "<br />"));

        DateTime sTime = Utils.ParseTime(Utils.GetRequest("sTime", "all", 2, DT.RegexTime, "开始时间填写无效"));
        DateTime oTime = Utils.ParseTime(Utils.GetRequest("oTime", "all", 2, DT.RegexTime, "结束时间填写无效"));
        string state = Utils.GetRequest("State", "all", 2, @"^[^\^]{1,5}$", "状态填写无效");
        int IsSWB = Utils.ParseInt(ub.GetSub("klsfSWB", xmlPath));


        builder.Append(Out.Tab("<div>", ""));
        builder.Append("" + GameName + sTime + "-" + oTime + "排行榜");
        builder.Append(Out.Tab("</div>", "<br />"));

        string strWheres = string.Empty;
        strWheres += "AddTime > '" + sTime + "' and AddTime <'" + oTime + "' and State>0 group by UsID Order by sum(WinCent-Prices) desc";
        DataSet ds = new BCW.BLL.klsfpay().GetList("UsID,sum(WinCent-Prices) as WinCent", strWheres);

        #region 发放奖励
        if (state == "ok")
        {
            int fPrize = Utils.ParseInt(Utils.GetRequest("fPrize", "post", 2, @"^[0-9]\d*$", "第一名奖励填写错误"));
            int sPrize = Utils.ParseInt(Utils.GetRequest("sPrize", "post", 2, @"^[0-9]\d*$", "第二名奖励填写错误"));
            int tPrize = Utils.ParseInt(Utils.GetRequest("tPrize", "post", 2, @"^[0-9]\d*$", "第三名奖励填写错误"));
            int fourPrize = Utils.ParseInt(Utils.GetRequest("fourPrize", "post", 2, @"^[0-9]\d*$", "第四名奖励填写错误"));
            int fivePrize = Utils.ParseInt(Utils.GetRequest("fivePrize", "post", 2, @"^[0-9]\d*$", "第五名奖励填写错误"));
            int sixPrize = Utils.ParseInt(Utils.GetRequest("sixPrize", "post", 2, @"^[0-9]\d*$", "第六名奖励填写错误"));
            int sevenPrize = Utils.ParseInt(Utils.GetRequest("sevenPrize", "post", 2, @"^[0-9]\d*$", "第七名奖励填写错误"));
            int ePrize = Utils.ParseInt(Utils.GetRequest("ePrize", "post", 2, @"^[0-9]\d*$", "第八名奖励填写错误"));
            int nPrize = Utils.ParseInt(Utils.GetRequest("nPrize", "post", 2, @"^[0-9]\d*$", "第就名奖励填写错误"));
            int tenPrize = Utils.ParseInt(Utils.GetRequest("tenPrize", "post", 2, @"^[0-9]\d*$", "第十名奖励填写错误"));

            int[] UsId = new int[10];
            string[] UsName = new string[10];

            for (int i = 0; i < 10; i++)
            {
                UsId[i] = Utils.ParseInt(ds.Tables[0].Rows[i]["UsID"].ToString());
                UsName[i] = new BCW.BLL.User().GetUsName(UsId[i]);
            }

            //if (IsSWB == 0)
            //{
            //    //第一名
            //    new BCW.SWB.BLL().UpdateMoney(UsId[0], fPrize, gid); //快乐币
            //    new BCW.BLL.Guest().Add(1, UsId[0], UsName[0], "您在" + GameName + "的排行榜奖励已经发放，获得了" + fPrize + "快乐币。");//开奖提示信息,1表示开奖信息

            //    //第二名
            //    new BCW.SWB.BLL().UpdateMoney(UsId[1], sPrize, gid); //快乐币
            //    new BCW.BLL.Guest().Add(1, UsId[1], UsName[1], "您在" + GameName + "的排行榜奖励已经发放，获得了" + sPrize + "快乐币。");//开奖提示信息,1表示开奖信息

            //    //第三名
            //    new BCW.SWB.BLL().UpdateMoney(UsId[2], tPrize, gid); //快乐币
            //    new BCW.BLL.Guest().Add(1, UsId[2], UsName[2], "您在" + GameName + "的排行榜奖励已经发放，获得了" + tPrize + "快乐币。");//开奖提示信息,1表示开奖信息

            //    //第四名到第十名
            //    for (int i = 3; i < 10; i++)
            //    {
            //        new BCW.SWB.BLL().UpdateMoney(UsId[i], ePrize, gid); //快乐币
            //        new BCW.BLL.Guest().Add(1, UsId[i], UsName[i], "您在" + GameName + "的排行榜奖励已经发放，获得了" + ePrize + "快乐币。");//开奖提示信息,1表示开奖信息
            //    }

            //    Utils.Success("排行榜奖励", "排行榜奖励发放成功", Utils.getUrl("klsf.aspx"), "1");
            //}
            //else
            {

                //第一名
                new BCW.BLL.User().UpdateiGold(UsId[0], fPrize, "" + GameName + sTime.ToShortDateString() + "-" + oTime.ToShortDateString() + "排行榜第一名奖励"); //酷币
                new BCW.BLL.Guest().Add(1, UsId[0], UsName[0], "您在" + GameName + sTime.ToShortDateString() + "-" + oTime.ToShortDateString() + "排行榜第一名奖励已经发放，获得了" + fPrize + ub.Get("SiteBz") + "。");//开奖提示信息,1表示开奖信息

                //第二名
                new BCW.BLL.User().UpdateiGold(UsId[1], sPrize, "" + GameName + sTime.ToShortDateString() + "-" + oTime.ToShortDateString() + "排行榜第二名奖励"); //酷币
                new BCW.BLL.Guest().Add(1, UsId[1], UsName[1], "您在" + GameName + sTime.ToShortDateString() + "-" + oTime.ToShortDateString() + "排行榜第二名奖励已经发放，获得了" + sPrize + ub.Get("SiteBz") + "。");//开奖提示信息,1表示开奖信息

                //第三名
                new BCW.BLL.User().UpdateiGold(UsId[2], tPrize, "" + GameName + sTime.ToShortDateString() + "-" + oTime.ToShortDateString() + "排行榜第三名奖励"); //酷币
                new BCW.BLL.Guest().Add(1, UsId[2], UsName[2], "您在" + GameName + sTime.ToShortDateString() + "-" + oTime.ToShortDateString() + "排行榜第三名奖励已经发放，获得了" + tPrize + ub.Get("SiteBz") + "。");//开奖提示信息,1表示开奖信息

                //第四名
                new BCW.BLL.User().UpdateiGold(UsId[3], fourPrize, "" + GameName + sTime.ToShortDateString() + "-" + oTime.ToShortDateString() + "排行榜第四名奖励"); //酷币
                new BCW.BLL.Guest().Add(1, UsId[3], UsName[3], "您在" + GameName + sTime.ToShortDateString() + "-" + oTime.ToShortDateString() + "排行榜第四名奖励已经发放，获得了" + fourPrize + ub.Get("SiteBz") + "。");//开奖提示信息,1表示开奖信息

                //第五名
                new BCW.BLL.User().UpdateiGold(UsId[4], fivePrize, "" + GameName + sTime.ToShortDateString() + "-" + oTime.ToShortDateString() + "排行榜第五名奖励"); //酷币
                new BCW.BLL.Guest().Add(1, UsId[4], UsName[4], "您在" + GameName + sTime.ToShortDateString() + "-" + oTime.ToShortDateString() + "排行榜第五名奖励已经发放，获得了" + fivePrize + ub.Get("SiteBz") + "。");//开奖提示信息,1表示开奖信息

                //第六名
                new BCW.BLL.User().UpdateiGold(UsId[5], sixPrize, "" + GameName + sTime.ToShortDateString() + "-" + oTime.ToShortDateString() + "排行榜第六名奖励"); //酷币
                new BCW.BLL.Guest().Add(1, UsId[5], UsName[5], "您在" + GameName + sTime.ToShortDateString() + "-" + oTime.ToShortDateString() + "排行榜第六名奖励已经发放，获得了" + sixPrize + ub.Get("SiteBz") + "。");//开奖提示信息,1表示开奖信息

                //第七名
                new BCW.BLL.User().UpdateiGold(UsId[6], sevenPrize, "" + GameName + sTime.ToShortDateString() + "-" + oTime.ToShortDateString() + "排行榜第七名奖励"); //酷币
                new BCW.BLL.Guest().Add(1, UsId[6], UsName[6], "您在" + GameName + sTime.ToShortDateString() + "-" + oTime.ToShortDateString() + "排行榜第七名奖励已经发放，获得了" + sevenPrize + ub.Get("SiteBz") + "。");//开奖提示信息,1表示开奖信息

                //第八名
                new BCW.BLL.User().UpdateiGold(UsId[7], ePrize, "" + GameName + sTime.ToShortDateString() + "-" + oTime.ToShortDateString() + "排行榜第八名奖励"); //酷币
                new BCW.BLL.Guest().Add(1, UsId[7], UsName[7], "您在" + GameName + sTime.ToShortDateString() + "-" + oTime.ToShortDateString() + "排行榜第八名奖励已经发放，获得了" + ePrize + ub.Get("SiteBz") + "。");//开奖提示信息,1表示开奖信息

                //第九名
                new BCW.BLL.User().UpdateiGold(UsId[8], nPrize, "" + GameName + sTime.ToShortDateString() + "-" + oTime.ToShortDateString() + "排行榜第九名奖励"); //酷币
                new BCW.BLL.Guest().Add(1, UsId[8], UsName[8], "您在" + GameName + sTime.ToShortDateString() + "-" + oTime.ToShortDateString() + "排行榜第九名奖励已经发放，获得了" + nPrize + ub.Get("SiteBz") + "。");//开奖提示信息,1表示开奖信息

                //第十名
                new BCW.BLL.User().UpdateiGold(UsId[9], tenPrize, "" + GameName + sTime.ToShortDateString() + "-" + oTime.ToShortDateString() + "排行榜第十名奖励"); //酷币
                new BCW.BLL.Guest().Add(1, UsId[9], UsName[9], "您在" + GameName + sTime.ToShortDateString() + "-" + oTime.ToShortDateString() + "排行榜第十名奖励已经发放，获得了" + tenPrize + ub.Get("SiteBz") + "。");//开奖提示信息,1表示开奖信息


                Utils.Success("排行榜奖励", "排行榜奖励发放成功", Utils.getUrl("klsf.aspx"), "1");
            }
        }
        #endregion

        #region 填写奖励
        else
        {
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    int[] UsId = new int[ds.Tables[0].Rows.Count];
            //    builder.Append(Out.Tab("<div>", ""));
            //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //    {
            //        UsId[i] = Utils.ParseInt(ds.Tables[0].Rows[i]["UsID"].ToString());
            //        if (IsSWB == 0)
            //            builder.Append("第" + (i + 1) + "名：<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + UsId[i] + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(UsId[i]) + "</a>赢" + ds.Tables[0].Rows[i]["WinCent"] + "快乐币<br/>");
            //        else
            //            builder.Append("第" + (i + 1) + "名：<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + UsId[i] + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(UsId[i]) + "</a>赢" + ds.Tables[0].Rows[i]["WinCent"] + ub.Get("SiteBz") + "<br/>");
            //    }
            //    builder.Append(Out.Tab("</div>", ""));
            //}
            //else
            //{
            //    builder.Append(Out.Tab("<div>", ""));
            //    builder.Append("没有相关记录...");
            //    builder.Append(Out.Tab("</div>", ""));
            //}
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string strWhere = "AddTime > '" + sTime + "' and AddTime <'" + oTime + "' and State>0 ";
            string[] pageValUrl = { "act", "State", "sTime", "oTime", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            // 开始读取列表
            IList<BCW.Model.klsfpay> listSSCpay = new BCW.BLL.klsfpay().GetklsfpaysTop(pageIndex, pageSize, strWhere, out recordCount);
            if (listSSCpay.Count > 0)
            {
                int k = 1;
                foreach (BCW.Model.klsfpay n in listSSCpay)
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
                    #region 根据版本选择快乐币和酷币的获取和显示
                    if (IsSWB == 0)
                    {
                        builder.Append("[第" + ((pageIndex - 1) * pageSize + k) + "名]<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(n.UsID) + "</a>赢" + n.WinCent + "快乐币");
                    }
                    else
                    {
                        builder.Append("[第" + ((pageIndex - 1) * pageSize + k) + "名]<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(n.UsID) + "(" + n.UsID + ")</a>赢" + n.WinCent + "" + ub.Get("SiteBz") + "");
                    }
                    #endregion

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
            builder.Append(Out.Tab("</div>", "<br />"));

            if (ds.Tables[0].Rows.Count < 10)
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<br />当前排行榜人数少于10人无法进行奖励");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                int[] UsId = new int[10];
                string[] UsName = new string[10];

                for (int i = 0; i < 10; i++)
                {
                    UsId[i] = Utils.ParseInt(ds.Tables[0].Rows[i]["UsID"].ToString());
                    UsName[i] = new BCW.BLL.User().GetUsName(UsId[i]);
                }
                string strText = "第一名(" + UsId[0] + ")的奖励:/,第二名(" + UsId[1] + ")的奖励:/,第三名(" + UsId[2] + ")的奖励:/,第四名(" + UsId[3] + ")的奖励:/,第五名(" + UsId[4] + ")的奖励:/,第六名(" + UsId[5] + ")的奖励:/,第七名(" + UsId[6] + ")的奖励:/,第八名(" + UsId[7] + ")的奖励:/,第九名(" + UsId[8] + ")的奖励:/,第十名(" + UsId[9] + ")的奖励:/,,,,";
                string strName = "fPrize,sPrize,tPrize,fourPrize,fivePrize,sixPrize,sevenPrize,ePrize,nPrize,tenPrize,state,act,sTime,oTime";
                string strType = "num,num,num,num,num,num,num,num,num,num,hidden,hidden,hidden,hidden";
                string strValu = "''''''''''ok'rewards'" + DT.FormatDate(sTime, 0) + "'" + DT.FormatDate(oTime, 0);
                string strEmpt = "false,false,false,false,false,false,false,false,false,false,false,false,false";
                string strIdea = "/";
                string strOthe = "发放奖励,klsf.aspx,post,1,red";

                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append("温馨提示：如果要奖励前十必须全部奖励，奖励值为" + ub.Get("SiteBz") + "<br />");
            builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=top") + "\">返回上一级</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        #endregion
    }

    /// <summary>
    /// 人工加上新一期
    /// </summary>
    private void AddNewStage()
    {
        Master.Title = "" + GameName + "手动添加新一期";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx") + "\">" + GameName + "</a>");
        builder.Append("&gt;手动添加新一期");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=addStage&amp;info=1") + "\">手动添加新一期</a>");
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "1")
        {
            builder.Append("&gt;<a href=\"" + Utils.getUrl("klsf.aspx?act=addStage&amp;info=2") + "\">确认添加新一期</a>");
        }
        if (info == "2")
        {
            BCW.tbasic.klsf.NewLastest2();
            Utils.Success("添加成功..", "手动添加新一期成功", Utils.getUrl("klsf.aspx"), "3");
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("说明：手动添加新一期即为添加新一期投注，一旦添加成功则添加的这一期开始投注");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    /// <summary>
    /// 返赢返负页面
    /// </summary>
    private void BackPage()
    {
        Master.Title = "" + GameName + "返赢返负";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx") + "\">" + GameName + "</a>");
        builder.Append("&gt;返赢返负");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("返赢返负操作");
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
        string strOthe = "马上返赢,klsf.aspx,post,1,red";

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
        strOthe = "马上返负,klsf.aspx,post,1,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("klsf.aspx?act=backmessage") + "\">返赢反负记录</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("klsf.aspx") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    /// <summary>
    /// 返赢返负操作
    /// </summary>
    /// <param name="act"></param>
    private void BackSave1Page(string act)
    {

        DateTime sTime = Utils.ParseTime(Utils.GetRequest("sTime", "post", 2, DT.RegexTime, "开始时间填写无效"));
        DateTime oTime = Utils.ParseTime(Utils.GetRequest("oTime", "post", 2, DT.RegexTime, "结束时间填写无效"));
        int iTar = Utils.ParseInt(Utils.GetRequest("iTar", "post", 2, @"^[0-9]\d*$", "千分比填写错误"));
        int iPrice = Utils.ParseInt(Utils.GetRequest("iPrice", "post", 2, @"^[0-9]\d*$", "至少多少币才返填写错误"));
        int IsSWB = Utils.ParseInt(ub.GetSub("klsfSWB", xmlPath));

        if (act == "backsave1")
        {
            DataSet ds = new BCW.BLL.klsfpay().GetList("UsID,sum(WinCent-Prices) as WinCents", "AddTime>='" + sTime + "'and AddTime<='" + oTime + "' group by UsID");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                long Cents = Convert.ToInt64(ds.Tables[0].Rows[i]["WinCents"]);
                if (Cents > 0 && Cents >= iPrice)
                {
                    int usid = Convert.ToInt32(ds.Tables[0].Rows[i]["UsID"]);
                    long cent = Convert.ToInt64(Cents * (iTar * 0.001));
                    string strLog = string.Empty;
                    if (IsSWB == 0)
                    {
                        new BCW.SWB.BLL().UpdateMoney(usid, cent, gid);
                        //发内线
                        strLog = "根据你近期" + GameName + "排行榜上的盈利情况，系统自动返还了" + cent + "快乐币[url=/bbs/game/klsf.aspx]进入" + GameName + "[/url]";
                    }
                    else
                    {
                        new BCW.BLL.User().UpdateiGold(usid, cent, "" + GameName + "返赢");
                        //发内线
                        strLog = "根据你近期" + GameName + "排行榜上的盈利情况，系统自动返还了" + cent + "" + ub.Get("SiteBz") + "[url=/bbs/game/klsf.aspx]进入" + GameName + "[/url]";
                    }

                    new BCW.BLL.Guest().Add(0, usid, new BCW.BLL.User().GetUsName(usid), strLog);
                }
            }
            long sum = 0; int sumi = 0;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                long Cents = Convert.ToInt64(ds.Tables[0].Rows[i]["WinCents"]);
                long cent = 0;
                if (Cents > 0 && Cents >= iPrice)
                {
                    int usid = Convert.ToInt32(ds.Tables[0].Rows[i]["UsID"]);
                    cent = Convert.ToInt64(Cents * (iTar * 0.001));
                    sumi++;
                }
                sum += cent;
            }
            new BCW.BLL.User().UpdateiGold(105, new BCW.BLL.User().GetUsName(105), 0, "" + GameName + sTime + "-" + oTime + "返赢|千分比" + iTar + "|至少赢" + iPrice + "|共计人次" + sumi + "|共返" + sum + "|此记录为返赢记录,不操作105的币数");

            Utils.Success("返赢操作", "返赢操作成功", Utils.getUrl("klsf.aspx"), "1");
        }
        else
        {
            DataSet ds = new BCW.BLL.klsfpay().GetList("UsID,sum(WinCent-Prices) as WinCents", "AddTime>='" + sTime + "'and AddTime<='" + oTime + "'  group by UsID");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                long Cents = Convert.ToInt64(ds.Tables[0].Rows[i]["WinCents"]);
                if (Cents < 0 && Cents <= (-iPrice))
                {
                    int usid = Convert.ToInt32(ds.Tables[0].Rows[i]["UsID"]);
                    long cent = Convert.ToInt64((-Cents) * (iTar * 0.001));
                    string strLog = string.Empty;
                    if (IsSWB == 0)
                    {
                        new BCW.SWB.BLL().UpdateMoney(usid, cent, gid);
                        //发内线
                        strLog = "根据你近期" + GameName + "排行榜上的亏损情况，系统自动返还了" + cent + "快乐币[url=/bbs/game/klsf.aspx]进入" + GameName + "[/url]";
                    }
                    else
                    {
                        new BCW.BLL.User().UpdateiGold(usid, cent, "" + GameName + "返负");
                        //发内线
                        strLog = "根据你近期" + GameName + "排行榜上的亏损情况，系统自动返还了" + cent + "" + ub.Get("SiteBz") + "[url=/bbs/game/klsf.aspx]进入" + GameName + "[/url]";
                    }

                    new BCW.BLL.Guest().Add(0, usid, new BCW.BLL.User().GetUsName(usid), strLog);
                }
            }
            long sum = 0; int sumi = 0;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                long Cents = Convert.ToInt64(ds.Tables[0].Rows[i]["WinCents"]);
                long cent = 0;
                if (Cents < 0 && Cents <= (-iPrice))
                {
                    int usid = Convert.ToInt32(ds.Tables[0].Rows[i]["UsID"]);
                    cent = Convert.ToInt64((-Cents) * (iTar * 0.001));
                    sumi++;
                }
                sum += cent;
            }
            new BCW.BLL.User().UpdateiGold(105, new BCW.BLL.User().GetUsName(105), 0, "" + GameName + sTime + "-" + oTime + "返负|千分比" + iTar + "|至少负" + iPrice + "|共计人次" + sumi + "|共返" + sum + "|此记录为返负记录,不操作105的币数");

            Utils.Success("返负操作", "返负操作成功", Utils.getUrl("klsf.aspx"), "1");

        }
    }
    private void BackSavePage(string act)
    {
        DateTime sTime = Utils.ParseTime(Utils.GetRequest("sTime", "post", 2, DT.RegexTime, "开始时间填写无效"));
        DateTime oTime = Utils.ParseTime(Utils.GetRequest("oTime", "post", 2, DT.RegexTime, "结束时间填写无效"));
        int iTar = Utils.ParseInt(Utils.GetRequest("iTar", "post", 2, @"^[0-9]\d*$", "千分比填写错误"));
        int iPrice = Utils.ParseInt(Utils.GetRequest("iPrice", "post", 2, @"^[0-9]\d*$", "至少多少币才返填写错误"));

        if (act == "backsave")
        {

            builder.Append(Out.Tab("<div>", "<br />"));

            builder.Append("返赢时间段：" + sTime + "到" + oTime + "<br />");
            builder.Append("返赢千分比：" + iTar + "<br />");
            builder.Append("至少赢：" + iPrice + "币返<br />");
            DataSet ds = new BCW.BLL.klsfpay().GetList("UsID,sum(WinCent-Prices) as WinCents", "AddTime>='" + sTime + "'and AddTime<='" + oTime + "' group by UsID");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                long sum = 0; int sumi = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    long Cents = Convert.ToInt64(ds.Tables[0].Rows[i]["WinCents"]);
                    long cent = 0;
                    if (Cents > 0 && Cents >= iPrice)
                    {
                        int usid = Convert.ToInt32(ds.Tables[0].Rows[i]["UsID"]);
                        cent = Convert.ToInt64(Cents * (iTar * 0.001));
                        sumi++;
                    }
                    sum += cent;
                }
                builder.Append("本次返赢人次：" + sumi + "<br />");
                builder.Append("本次返赢金额：" + sum + "<br />");
            }

            builder.Append(Out.Tab("</div>", ""));
            string strText = "开始时间:,结束时间:,返赢千分比:,至少赢多少才返:,";
            string strName = "sTime,oTime,iTar,iPrice,act";
            string strType = "hidden,hidden,hidden,hidden,hidden";
            string strValu = "" + DT.FormatDate(sTime, 0) + "'" + DT.FormatDate(oTime, 0) + "'" + iTar + "'" + iPrice + "'backsave1";
            string strEmpt = "false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "马上返赢,klsf.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("", "<br />"));

        }
        else
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("返负时间段：" + sTime + "到" + oTime + "<br />");
            builder.Append("返负千分比：" + iTar + "<br />");
            builder.Append("至少：" + iPrice + "币返<br />");
            DataSet ds = new BCW.BLL.klsfpay().GetList("UsID,sum(WinCent-Prices) as WinCents", "AddTime>='" + sTime + "'and AddTime<='" + oTime + "' group by UsID");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                long sum = 0; int sumi = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    long Cents = Convert.ToInt64(ds.Tables[0].Rows[i]["WinCents"]);
                    long cent = 0;
                    if (Cents < 0 && Cents <= (-iPrice))
                    {
                        int usid = Convert.ToInt32(ds.Tables[0].Rows[i]["UsID"]);
                        cent = Convert.ToInt64((-Cents) * (iTar * 0.001));
                        sumi++;
                    }
                    sum += cent;
                }
                builder.Append("本次返负人次：" + sumi + "<br />");
                builder.Append("本次返负金额：" + sum + "<br />");
            }

            builder.Append(Out.Tab("</div>", ""));
            string strText = "开始时间:,结束时间:,返负千分比:,至少负多少才返:,";
            string strName = "sTime,oTime,iTar,iPrice,act";
            string strType = "hidden,hidden,hidden,hidden,hidden";
            string strValu = "" + DT.FormatDate(sTime, 0) + "'" + DT.FormatDate(oTime, 0) + "'" + iTar + "'" + iPrice + "'backsave3";
            string strEmpt = "false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "马上返负,klsf.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("", "<br />"));

        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=back") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    /// <summary>
    /// 返赢返负记录
    /// </summary>
    private void BackMessagePage()
    {
        Master.Title = "" + GameName + "返赢返负记录";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx") + "\">" + GameName + "</a>");
        builder.Append("&gt;<a href=\"" + Utils.getUrl("klsf.aspx?act=back") + "\">返赢返负</a>&gt;返赢返负记录");
        builder.Append(Out.Tab("</div>", "<br />"));

        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-1]$", "0"));
        builder.Append(Out.Tab("<div>", ""));
        if (ptype == 0)
            builder.Append("返赢记录 | <a href=\"" + Utils.getUrl("klsf.aspx?act=backmessage&amp;ptype=1") + "\">返负记录</a>");
        else
            builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=backmessage&amp;ptype=0") + "\">返赢记录</a> | 返负记录");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 0)
            builder.Append("返赢记录");
        else
            builder.Append("返负记录");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        if (ptype == 0)
        {
            strWhere = " AcText LIKE '%" + GameName + "返赢%' ";
        }
        else
        {
            strWhere = " AcText LIKE '%" + GameName + "返负%' ";
        }

        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //  string arrId = "";
        // string klsfqi = "";
        // 开始读取列表
        IList<BCW.Model.Goldlog> listgodlog = new BCW.BLL.Goldlog().GetGoldlogs(pageIndex, pageSize, strWhere, out recordCount);
        if (listgodlog.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Goldlog n in listgodlog)
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
                //if (n.AddTime.ToString() != klsfqi)
                //{
                //        builder.Append("<f style=\"color:red\">="+n.AddTime+"=</f><br />");
                //}
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.UsId + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsId + ")" + "</a>" + n.AcText + n.AcGold + ub.Get("SiteBz") + "[" + n.AddTime + "]");


                // klsfqi = n.AddTime.ToString();
                //  arrId = arrId + " " + n.ID;
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

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("klsf.aspx?act=back") + "\">返赢返负</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    /// <summary>
    /// 开奖页面
    /// </summary>
    private void OpenPage()
    {
        Master.Title = "" + GameName + "手动开奖";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx") + "\">" + GameName + "</a>");
        builder.Append("&gt;手动开奖");
        builder.Append(Out.Tab("</div>", "<br />"));

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));

        BCW.Model.klsflist model = new BCW.BLL.klsflist().Getklsflist(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.State == 1)
        {
            Utils.Error("该期已开奖", "");
        }

        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {
            string rResult = Utils.GetRequest("rResult", "all", 2, @"^[0-9]\d\s[0-9]\d\s[0-9]\d\s[0-9]\d\s[0-9]\d\s[0-9]\d\s[0-9]\d\s[0-9]\d$", "中奖号码填写错误");
            rResult = rResult.Replace(" ", ",");
            new BCW.BLL.klsflist().UpdateResult(model.klsfId.ToString(), rResult);
            new BCW.BLL.klsfpay().UpdateResult(model.klsfId.ToString(), rResult);

            try
            {
                klsffc();
            }
            catch { }

            Utils.Success("第" + model.klsfId + "期开奖", "第" + model.klsfId + "期开奖成功..", Utils.getUrl("klsf.aspx"), "2");
        }
        if (info == "ok1")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("第" + model.klsfId + "期手动开奖");
            builder.Append(Out.Tab("</div>", "<br />"));
            string rResult = Utils.GetRequest("rResult", "all", 2, @"^[0-9]\d\s[0-9]\d\s[0-9]\d\s[0-9]\d\s[0-9]\d\s[0-9]\d\s[0-9]\d\s[0-9]\d$", "中奖号码填写错误");
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("开奖结果为：" + rResult + "<br />");
            builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=open&amp;info=ok&amp;id=" + id + "&amp;rResult=" + rResult + "") + "\">确定开奖</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getPage("klsf.aspx") + "\">返回上一级</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("第" + model.klsfId + "期手动开奖");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "中奖号码（用空格分开，如07 18 19 01 15 10 11 17）:/,,,,";
            string strName = "rResult,id,act,info,backurl";
            string strType = "text,hidden,hidden,hidden,hidden";
            string strValu = "'" + id + "'open'ok1'" + Utils.getPage(0) + "";
            string strEmpt = "true,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定开奖|reset,klsf.aspx,post,1,red|blue";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getPage("klsf.aspx") + "\">返回上一级</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }

    /// <summary>
    /// 显示数据库内所有记录
    /// </summary>
    private void ViewPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-2]\d*$", "1"));
        BCW.Model.klsflist model = null;
        model = new BCW.BLL.klsflist().Getklsflist(id);

        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "" + GameName + "第" + model.klsfId + "期";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx") + "\">" + GameName + "</a>");
        builder.Append("&gt;第" + model.klsfId + "期");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("查看下注/开奖记录");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("查看:");
        if (ptype == 1)
            builder.Append("下注|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=view&amp;ptype=1&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">下注</a>|");

        if (ptype == 2)
            builder.Append("中奖");
        else
            builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=view&amp;ptype=2&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">中奖</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        if (ptype == 1)
            strWhere += "klsfId=" + model.klsfId + "";
        else
            strWhere += "klsfId=" + model.klsfId + " and state>0 and winCent>0";

        string[] pageValUrl = { "act", "id", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.klsfpay> listSSCpay = new BCW.BLL.klsfpay().Getklsfpays(pageIndex, pageSize, strWhere, out recordCount);
        if (listSSCpay.Count > 0)
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("第" + model.klsfId + "期开出:<b>" + model.Result + "|和:" + GetHe(model.Result) + "</b>");
            if (ptype == 1)
                builder.Append("<br />共" + recordCount + "注下注");
            else
                builder.Append("<br />共" + recordCount + "注中奖");

            builder.Append(Out.Tab("</div>", "<br />"));

            int k = 1;
            foreach (BCW.Model.klsfpay n in listSSCpay)
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

                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "</a>");

                #region 根据版本选择快乐币和酷币的获取和显示
                int IsSWB = Utils.ParseInt(ub.GetSub("klsfSWB", xmlPath));
                if (IsSWB == 0)
                {
                    builder.Append("<b>[" + OutType(n.Types) + "]</b>位号:" + n.Notes + "/每注" + n.Price + "快乐币/共买" + n.iCount + "注[" + DT.FormatDate(n.AddTime, 1) + "]");
                }
                else
                {
                    builder.Append("<b>[" + OutType(n.Types) + "]</b>位号:" + n.Notes + "/每注" + n.Price + "" + ub.Get("SiteBz") + "/共买" + n.iCount + "注/赔率" + n.Odds + "/标识ID：" + n.ID + " [" + DT.FormatDate(n.AddTime, 1) + "]");
                }
                #endregion

                if (n.WinCent > 0)
                {
                    #region 根据版本选择快乐币和酷币的获取和显示
                    if (IsSWB == 0)
                    {
                        builder.Append("&nbsp;中" + n.iWin + "注/ 赢" + n.WinCent + "快乐币");
                    }
                    else
                    {
                        builder.Append("&nbsp; 中" + n.iWin + "注/赢" + n.WinCent + "" + ub.Get("SiteBz") + "");
                    }
                    #endregion

                }

                builder.Append(".<a href=\"" + Utils.getUrl("klsf.aspx?act=del&amp;klsfid=" + n.ID + "&amp;id=" + id + "&amp;ptype" + ptype + "") + "\">[退]</a>");
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

        }
        else
        {
            if (ptype == 1)
            {
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("第" + model.klsfId + "期开出:<b>" + model.Result + "|和:" + GetHe(model.Result) + "</b>");
                builder.Append("<br />共0注投注");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Div("div", "没有相关记录..."));
            }
            else if (ptype == 2)
            {
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("第" + model.klsfId + "期开出:<b>" + model.Result + "|和:" + GetHe(model.Result) + "</b>");
                builder.Append("<br />共0注中奖");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Div("div", "没有相关记录..."));
            }

        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("klsf.aspx") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //==========删除一条投注记录==================
    private void DelPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "")
        {
            int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
            int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-9]\d*$", "1"));
            int klsfid = int.Parse(Utils.GetRequest("klsfid", "all", 1, @"^[0-9]\d*$", "1"));
            BCW.Model.klsfpay n = new BCW.BLL.klsfpay().Getklsfpay(klsfid);

            Master.Title = "" + GameName + "第" + n.klsfId + "期删除一条投注记录";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
            builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx") + "\">" + GameName + "</a>");
            builder.Append("&gt;第" + n.klsfId + "期");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("确定删除该记录吗?");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("ID:" + n.ID + ".<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "</a>");
            builder.Append("<b>[" + OutType(n.Types) + "]</b>位号:" + n.Notes + "/每注" + n.Price + "" + ub.Get("SiteBz") + "/共买" + n.iCount + "注/总下注" + n.Prices + ub.Get("SiteBz") + "/赔率" + n.Odds + "/标识ID：" + n.ID + " [" + DT.FormatDate(n.AddTime, 1) + "]");

            if (n.WinCent > 0)
            {
                builder.Append("&nbsp; 中" + n.iWin + "注/赢" + n.WinCent + "" + ub.Get("SiteBz") + "");
            }
            builder.Append("<br />");

            if (n.State == 2)
            {
                if (n.WinCent > 0)
                {
                    builder.Append("确认退还删除记录将收回会员下注赢的" + n.WinCent + ub.Get("SiteBz") + ",退还下注" + n.Prices + ub.Get("SiteBz") + "<br />");
                }
                else
                {
                    builder.Append("确认退还删除记录将退还会员下注" + n.Prices + ub.Get("SiteBz") + "<br />");
                }
            }
            else
            {
                builder.Append("确认退还删除记录将退还会员下注" + n.Prices + ub.Get("SiteBz") + "<br />");
            }
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?info=ok&amp;act=del&amp;id=" + id + "&amp;klsfid=" + n.ID + "&amp;ptype=" + ptype + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=view&amp;id=" + id + "&amp;ptype=" + ptype + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=view&amp;id=" + id + "&amp;ptype=" + ptype + "") + "\">返回上一级</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
            int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-9]\d*$", "1"));
            int klsfid = int.Parse(Utils.GetRequest("klsfid", "all", 1, @"^[0-9]\d*$", "1"));
            BCW.Model.klsfpay n = new BCW.BLL.klsfpay().Getklsfpay(klsfid);
            if (!new BCW.BLL.klsfpay().Exists(klsfid))
            {
                Utils.Error("不存在的记录", "");
            }
            else
            {
                //根据id查询-购买表
                int meid = n.UsID;//用户名
                string mename = new BCW.BLL.User().GetUsName(meid);//获得id对应的用户名
                int state_get = n.State;//用户购买情况

                //如果未开奖，退回本金
                if (state_get < 2)
                {
                    new BCW.BLL.User().UpdateiGold(meid, n.Prices, "系统退回" + GameName + "第" + n.klsfId + "期下注的" + n.Prices + "" + ub.Get("SiteBz") + "！");//减少系统总的酷币
                    new BCW.BLL.Guest().Add(1, meid, mename, "系统退回您的" + GameName + "：第" + n.klsfId + "期下注的" + n.Prices + "" + ub.Get("SiteBz") + "！");
                }
                //未中奖或已中奖，退回本金-所得
                //else if ((state_get == 2) || (state_get == 1))
                else
                {
                    //Price = model.PutGold - model.GetMoney;//系统-(本金-所得)
                    long gold = 0;//个人酷币
                    long cMoney = 0;//差多少
                    long sMoney = 0;//实扣
                    string ui = string.Empty;
                    gold = new BCW.BLL.User().GetGold(n.UsID);//个人酷币
                    if (n.WinCent > gold)
                    {
                        cMoney = n.WinCent - gold + n.Prices;
                        sMoney = n.WinCent;
                    }
                    else
                    {
                        sMoney = n.WinCent;
                    }

                    //如果币不够扣则记录日志并冻结IsFreeze
                    if (cMoney > 0)
                    {
                        BCW.Model.Gameowe owe = new BCW.Model.Gameowe();
                        owe.Types = 1;
                        owe.UsID = n.UsID;
                        owe.UsName = mename;
                        owe.Content = "" + GameName + n.klsfId + "期" + OutType(n.Types) + "下注" + n.Prices + "" + ub.Get("SiteBz") + ".系统管理员" + new BCW.User.Manage().IsManageLogin() + "删除.";
                        owe.OweCent = cMoney;
                        owe.BzType = 10;
                        owe.EnId = n.ID;
                        owe.AddTime = DateTime.Now;
                        new BCW.BLL.Gameowe().Add(owe);
                        new BCW.BLL.User().UpdateIsFreeze(n.UsID, 1);
                        ui = "实扣" + sMoney + ",还差" + (cMoney) + ",系统已自动将您帐户冻结.";
                    }
                    string oop = string.Empty;
                    if (n.WinCent > 0)
                    {
                        oop = "并扣除所得的" + n.WinCent + "。";
                    }
                    new BCW.BLL.User().UpdateiGold(n.UsID, n.Prices - n.WinCent, "无效购奖或非法操作，系统退回" + GameName + "第" + n.klsfId + "期下注的" + n.Prices + "" + ub.Get("SiteBz") + "." + oop + "" + ui);//减少系统总的酷币
                    new BCW.BLL.Guest().Add(1, meid, mename, "无效购奖或非法操作，系统退回" + GameName + "第" + n.klsfId + "期下注的" + n.Prices + "" + ub.Get("SiteBz") + "." + oop + "" + ui);
                }
                ////如果过期不兑奖，退回本金
                //else if (state_get == 3)
                //{
                //    Price = model.PutGold;
                //    new BCW.BLL.User().UpdateiGold(model.UsID, Price, "系统退回新快3第" + model.Lottery_issue + "期未兑奖的" + model.PutGold + "酷币！");//减少系统总的酷币
                //    new BCW.BLL.Guest().Add(1, meid, mename, "系统退回新快3第" + model.Lottery_issue + "期未兑奖的" + model.PutGold + "酷币！");
                //}

                new BCW.BLL.klsfpay().Delete(n.ID);

                Utils.Success("删除投注记录", "删除记录成功..", Utils.getPage("klsf.aspx?act=view&amp;id=" + id + "&amp;ptype=" + ptype + ""), "2");

            }
        }
    }

    /// <summary>
    /// 重置游戏
    /// </summary>
    private void ResetPage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId != 1 && ManageId != 9)
        {
            Utils.Error("权限不足", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {
            new BCW.BLL.klsflist().ClearTable("tb_klsflist");
            new BCW.BLL.klsflist().ClearTable("tb_klsfpay");

            Utils.Success("重置游戏", "重置游戏成功..", Utils.getUrl("klsf.aspx"), "2");
        }
        if (info == "ok1")
        {
            Master.Title = "" + GameName + "重置游戏";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
            builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx") + "\">" + GameName + "</a>");
            builder.Append("&gt;重置游戏");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("确定重置" + GameName + "游戏吗，重置后，将重新从第一期开始，所有记录将会期数和下注记录全被删除");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=reset&amp;info=ok1") + "\">重置游戏</a>&gt;");
            builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=reset&amp;info=ok") + "\">确定重置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            Master.Title = "" + GameName + "重置游戏";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
            builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx") + "\">" + GameName + "</a>");
            builder.Append("&gt;重置游戏");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("确定重置" + GameName + "游戏吗，重置后，将重新从第一期开始，所有记录将会期数和下注记录全被删除");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=reset&amp;info=ok1") + "\">重置游戏</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }

    /// <summary>
    /// 投注方式
    /// </summary>
    /// <param name="Types">输入投注类型编号</param>
    /// <returns>投注类型的名称</returns>
    private string OutType(int Types)
    {
        string pText = string.Empty;
        if (Types == 1)
            pText = "任选五胆拖投注";
        else if (Types == 2)
            pText = "任选五普通复式";
        else if (Types == 3)
            pText = "任选四胆拖投注";
        else if (Types == 4)
            pText = "任选四普通复式";
        else if (Types == 5)
            pText = "任选三胆拖投注";
        else if (Types == 6)
            pText = "任选三普通复式";
        else if (Types == 7)
            pText = "任选二胆拖投注";
        else if (Types == 8)
            pText = "任选二普通复式";
        else if (Types == 9)
            pText = "连二直选";
        else if (Types == 10)
            pText = "连二组选";
        else if (Types == 11)
            pText = "前一红投";
        else if (Types == 12)
            pText = "前一数投";
        else if (Types == 13)
            pText = "大小";
        else if (Types == 14)
            pText = "单双";
        else if (Types == 15)
            pText = "龙虎";
        return pText;
    }

    /// <summary>
    /// 赢利分析
    /// </summary>
    private void StatPage()
    {
        Master.Title = "" + GameName + "赢利分析";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx") + "\">" + GameName + "</a>");
        builder.Append("&gt;赢利分析");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("=赢利分析=");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-2]$", "1"));
        builder.Append("=用户分析=");
        //if (ptype == 0)
        //    builder.Append("所有分析|<a href=\"" + Utils.getUrl("klsf.aspx?act=stat&amp;ptype=1") + "\">用户分析</a>|<a href=\"" + Utils.getUrl("klsf.aspx?act=stat&amp;ptype=2") + "\">机器人分析</a>");
        //if (ptype == 1)
        //    builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=stat&amp;ptype=0") + "\">所有分析</a>|用户分析|<a href=\"" + Utils.getUrl("klsf.aspx?act=stat&amp;ptype=2") + "\">机器人分析</a>");
        //if (ptype == 2)
        //    builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=stat&amp;ptype=0") + "\">所有分析</a>|<a href=\"" + Utils.getUrl("klsf.aspx?act=stat&amp;ptype=1") + "\">用户分析</a>|机器人分析");
        builder.Append(Out.Tab("</div>", "<br />"));

        long TodayBuyCent = 0;
        long TodayWinCent = 0;
        long YesBuyCent = 0; long YesWinCent = 0;
        long MonthBuyCent = 0; long MonthWinCent = 0;
        long Month2BuyCent = 0; long Month2WinCent = 0;
        long BuyCent = 0; long WinCent = 0;
        if (ptype == 0)
        {
            //今天本金与赢利
            TodayBuyCent = new BCW.BLL.klsfpay().GetSumPrices("State>0  and Year(AddTime) = " + DateTime.Now.Year + " AND Month(AddTime) = " + DateTime.Now.Month + " and Day(AddTime) = " + DateTime.Now.Day + "");
            TodayWinCent = new BCW.BLL.klsfpay().GetSumWinCent("State>0  and Year(AddTime) = " + DateTime.Now.Year + " AND Month(AddTime) = " + DateTime.Now.Month + " and Day(AddTime) = " + DateTime.Now.Day + "");

            //昨天本金与赢利
            YesBuyCent = new BCW.BLL.klsfpay().GetSumPrices("State>0  and Year(AddTime) = " + DateTime.Now.AddDays(-1).Year + " AND Month(AddTime) = " + DateTime.Now.AddDays(-1).Month + " and Day(AddTime) = " + DateTime.Now.AddDays(-1).Day + " ");
            YesWinCent = new BCW.BLL.klsfpay().GetSumWinCent("State>0   and Year(AddTime) = " + DateTime.Now.AddDays(-1).Year + " AND Month(AddTime) = " + DateTime.Now.AddDays(-1).Month + " and Day(AddTime) = " + DateTime.Now.AddDays(-1).Day + " ");

            //本月本金与赢利
            MonthBuyCent = new BCW.BLL.klsfpay().GetSumPrices("State>0  and Year(AddTime) = " + (DateTime.Now.Year) + " AND Month(AddTime) = " + (DateTime.Now.Month) + " ");
            MonthWinCent = new BCW.BLL.klsfpay().GetSumWinCent("State>0   and Year(AddTime) = " + (DateTime.Now.Year) + " AND Month(AddTime) = " + (DateTime.Now.Month) + " ");

            //上月本金与赢利
            Month2BuyCent = new BCW.BLL.klsfpay().GetSumPrices("State>0  and Year(AddTime) = " + (DateTime.Now.AddMonths(-1).Year) + " AND Month(AddTime) = " + (DateTime.Now.AddMonths(-1).Month) + " ");
            Month2WinCent = new BCW.BLL.klsfpay().GetSumWinCent("State>0  and Year(AddTime) = " + (DateTime.Now.AddMonths(-1).Year) + " AND Month(AddTime) = " + (DateTime.Now.AddMonths(-1).Month) + " ");

            //总本金与赢利
            BuyCent = new BCW.BLL.klsfpay().GetSumPrices("State>0  ");
            WinCent = new BCW.BLL.klsfpay().GetSumWinCent("State>0 ");
        }
        if (ptype == 1)
        {
            //今天本金与赢利
            TodayBuyCent = new BCW.BLL.klsfpay().GetSumPrices("State>0 and isRoBot!=1  and UsID in (select ID from tb_User where IsSpier!=1)  and Year(AddTime) = " + DateTime.Now.Year + " AND Month(AddTime) = " + DateTime.Now.Month + " and Day(AddTime) = " + DateTime.Now.Day + "");
            TodayWinCent = new BCW.BLL.klsfpay().GetSumWinCent("State>0 and isRoBot!=1  and UsID in (select ID from tb_User where IsSpier!=1)  and Year(AddTime) = " + DateTime.Now.Year + " AND Month(AddTime) = " + DateTime.Now.Month + " and Day(AddTime) = " + DateTime.Now.Day + "");

            //昨天本金与赢利
            YesBuyCent = new BCW.BLL.klsfpay().GetSumPrices("State>0 and isRoBot!=1  and UsID in (select ID from tb_User where IsSpier!=1)  and Year(AddTime) = " + DateTime.Now.AddDays(-1).Year + " AND Month(AddTime) = " + DateTime.Now.AddDays(-1).Month + " and Day(AddTime) = " + DateTime.Now.AddDays(-1).Day + " ");
            YesWinCent = new BCW.BLL.klsfpay().GetSumWinCent("State>0 and isRoBot!=1  and UsID in (select ID from tb_User where IsSpier!=1)  and Year(AddTime) = " + DateTime.Now.AddDays(-1).Year + " AND Month(AddTime) = " + DateTime.Now.AddDays(-1).Month + " and Day(AddTime) = " + DateTime.Now.AddDays(-1).Day + " ");

            //本月本金与赢利
            MonthBuyCent = new BCW.BLL.klsfpay().GetSumPrices("State>0 and isRoBot!=1  and UsID in (select ID from tb_User where IsSpier!=1)  and Year(AddTime) = " + (DateTime.Now.Year) + " AND Month(AddTime) = " + (DateTime.Now.Month) + " ");
            MonthWinCent = new BCW.BLL.klsfpay().GetSumWinCent("State>0 and isRoBot!=1  and UsID in (select ID from tb_User where IsSpier!=1)  and Year(AddTime) = " + (DateTime.Now.Year) + " AND Month(AddTime) = " + (DateTime.Now.Month) + " ");

            //上月本金与赢利
            Month2BuyCent = new BCW.BLL.klsfpay().GetSumPrices("State>0 and isRoBot!=1  and UsID in (select ID from tb_User where IsSpier!=1)  and Year(AddTime) = " + (DateTime.Now.AddMonths(-1).Year) + " AND Month(AddTime) = " + (DateTime.Now.AddMonths(-1).Month) + " ");
            Month2WinCent = new BCW.BLL.klsfpay().GetSumWinCent("State>0 and isRoBot!=1  and UsID in (select ID from tb_User where IsSpier!=1)  and Year(AddTime) = " + (DateTime.Now.AddMonths(-1).Year) + " AND Month(AddTime) = " + (DateTime.Now.AddMonths(-1).Month) + " ");

            //总本金与赢利
            BuyCent = new BCW.BLL.klsfpay().GetSumPrices("State>0 and isRoBot!=1  and UsID in (select ID from tb_User where IsSpier!=1)  ");
            WinCent = new BCW.BLL.klsfpay().GetSumWinCent("State>0 and isRoBot!=1  and UsID in (select ID from tb_User where IsSpier!=1)  ");
        }
        if (ptype == 2)
        {
            //今天本金与赢利
            TodayBuyCent = new BCW.BLL.klsfpay().GetSumPrices("State>0 and isRoBot=1 and Year(AddTime) = " + DateTime.Now.Year + " AND Month(AddTime) = " + DateTime.Now.Month + " and Day(AddTime) = " + DateTime.Now.Day + "");
            TodayWinCent = new BCW.BLL.klsfpay().GetSumWinCent("State>0 and isRoBot=1  and Year(AddTime) = " + DateTime.Now.Year + " AND Month(AddTime) = " + DateTime.Now.Month + " and Day(AddTime) = " + DateTime.Now.Day + "");

            //昨天本金与赢利
            YesBuyCent = new BCW.BLL.klsfpay().GetSumPrices("State>0 and isRoBot=1  and Year(AddTime) = " + DateTime.Now.AddDays(-1).Year + " AND Month(AddTime) = " + DateTime.Now.AddDays(-1).Month + " and Day(AddTime) = " + DateTime.Now.AddDays(-1).Day + " ");
            YesWinCent = new BCW.BLL.klsfpay().GetSumWinCent("State>0 and isRoBot=1  and Year(AddTime) = " + DateTime.Now.AddDays(-1).Year + " AND Month(AddTime) = " + DateTime.Now.AddDays(-1).Month + " and Day(AddTime) = " + DateTime.Now.AddDays(-1).Day + " ");

            //本月本金与赢利
            MonthBuyCent = new BCW.BLL.klsfpay().GetSumPrices("State>0 and isRoBot=1  and Year(AddTime) = " + (DateTime.Now.Year) + " AND Month(AddTime) = " + (DateTime.Now.Month) + " ");
            MonthWinCent = new BCW.BLL.klsfpay().GetSumWinCent("State>0 and isRoBot=1  and Year(AddTime) = " + (DateTime.Now.Year) + " AND Month(AddTime) = " + (DateTime.Now.Month) + " ");

            //上月本金与赢利
            Month2BuyCent = new BCW.BLL.klsfpay().GetSumPrices("State>0 and isRoBot=1  and Year(AddTime) = " + (DateTime.Now.AddMonths(-1).Year) + " AND Month(AddTime) = " + (DateTime.Now.AddMonths(-1).Month) + " ");
            Month2WinCent = new BCW.BLL.klsfpay().GetSumWinCent("State>0 and isRoBot=1  and Year(AddTime) = " + (DateTime.Now.AddMonths(-1).Year) + " AND Month(AddTime) = " + (DateTime.Now.AddMonths(-1).Month) + " ");

            //总本金与赢利
            BuyCent = new BCW.BLL.klsfpay().GetSumPrices("State>0 and isRoBot=1  ");
            WinCent = new BCW.BLL.klsfpay().GetSumWinCent("State>0 and isRoBot=1  ");
        }


        #region 根据版本选择快乐币和酷币的获取和显示
        int IsSWB = Utils.ParseInt(ub.GetSub("klsfSWB", xmlPath));
        if (IsSWB == 0)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("今天下注:" + TodayBuyCent + "快乐币<br />");
            builder.Append("今天返彩:" + TodayWinCent + "快乐币<br />");
            builder.Append("今天净赢:" + (TodayBuyCent - TodayWinCent) + "快乐币");
            builder.Append(Out.Tab("</div>", Out.Hr()));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("昨天下注:" + YesBuyCent + "快乐币<br />");
            builder.Append("昨天返彩:" + YesWinCent + "快乐币<br />");
            builder.Append("昨天净赢:" + (YesBuyCent - YesWinCent) + "快乐币");
            builder.Append(Out.Tab("</div>", Out.Hr()));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("本月下注:" + MonthBuyCent + "快乐币<br />");
            builder.Append("本月返彩:" + MonthWinCent + "快乐币<br />");
            builder.Append("本月净赢:" + (MonthBuyCent - MonthWinCent) + "快乐币");
            builder.Append(Out.Tab("</div>", Out.Hr()));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("上月下注:" + Month2BuyCent + "快乐币<br />");
            builder.Append("上月返彩:" + Month2WinCent + "快乐币<br />");
            builder.Append("上月净赢:" + (Month2BuyCent - Month2WinCent) + "快乐币");
            builder.Append(Out.Tab("</div>", Out.Hr()));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("总计下注:" + BuyCent + "快乐币<br />");
            builder.Append("总计返彩:" + WinCent + "快乐币<br />");
            builder.Append("总计净赢:" + (BuyCent - WinCent) + "快乐币");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("今天下注:" + TodayBuyCent + "" + ub.Get("SiteBz") + "<br />");
            builder.Append("今天返彩:" + TodayWinCent + "" + ub.Get("SiteBz") + "<br />");
            builder.Append("今天净赢:" + (TodayBuyCent - TodayWinCent) + "" + ub.Get("SiteBz") + "");
            builder.Append(Out.Tab("</div>", Out.Hr()));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("昨天下注:" + YesBuyCent + "" + ub.Get("SiteBz") + "<br />");
            builder.Append("昨天返彩:" + YesWinCent + "" + ub.Get("SiteBz") + "<br />");
            builder.Append("昨天净赢:" + (YesBuyCent - YesWinCent) + "" + ub.Get("SiteBz") + "");
            builder.Append(Out.Tab("</div>", Out.Hr()));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("本月下注:" + MonthBuyCent + "" + ub.Get("SiteBz") + "<br />");
            builder.Append("本月返彩:" + MonthWinCent + "" + ub.Get("SiteBz") + "<br />");
            builder.Append("本月净赢:" + (MonthBuyCent - MonthWinCent) + "" + ub.Get("SiteBz") + "");
            builder.Append(Out.Tab("</div>", Out.Hr()));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("上月下注:" + Month2BuyCent + "" + ub.Get("SiteBz") + "<br />");
            builder.Append("上月返彩:" + Month2WinCent + "" + ub.Get("SiteBz") + "<br />");
            builder.Append("上月净赢:" + (Month2BuyCent - Month2WinCent) + "" + ub.Get("SiteBz") + "");
            builder.Append(Out.Tab("</div>", Out.Hr()));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("总计下注:" + BuyCent + "" + ub.Get("SiteBz") + "<br />");
            builder.Append("总计返彩:" + WinCent + "" + ub.Get("SiteBz") + "<br />");
            builder.Append("总计净赢:" + (BuyCent - WinCent) + "" + ub.Get("SiteBz") + "");
            builder.Append(Out.Tab("</div>", ""));
        }
        #endregion

        builder.Append(Out.Tab("<div class=\"hr\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=tstate") + "\">按时间段进行盈利分析</a>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("说明：【用户分析】是会员用户记录分析");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("klsf.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    /// <summary>
    /// 根据时间查询盈利
    /// </summary>
    private void TimeStatPage()
    {
        Master.Title = "" + GameName + "指定时间段赢利分析";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx") + "\">" + GameName + "</a>");
        builder.Append("&gt;指定时间段赢利分析");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("=赢利分析=");
        builder.Append(Out.Tab("</div>", "<br />"));

        DateTime sTime = Utils.ParseTime(Utils.GetRequest("stTime", "all", 1, DT.RegexTime, "" + DateTime.Now + ""));
        DateTime oTime = Utils.ParseTime(Utils.GetRequest("ovTime", "all", 1, DT.RegexTime, "" + DateTime.Now + ""));

        builder.Append(Out.Tab("<div>", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-2]$", "1"));
        builder.Append("=用户分析=");
        //if (ptype == 0)
        //    builder.Append("所有分析|<a href=\"" + Utils.getUrl("klsf.aspx?act=tstate&amp;ptype=1") + "\">用户分析</a>|<a href=\"" + Utils.getUrl("klsf.aspx?act=tstate&amp;ptype=2") + "\">机器人分析</a>");
        //if (ptype == 1)
        //    builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=tstate&amp;ptype=0") + "\">所有分析</a>|用户分析|<a href=\"" + Utils.getUrl("klsf.aspx?act=tstate&amp;ptype=2") + "\">机器人分析</a>");
        //if (ptype == 2)
        //    builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=tstate&amp;ptype=0") + "\">所有分析</a>|<a href=\"" + Utils.getUrl("klsf.aspx?act=tstate&amp;ptype=1") + "\">用户分析</a>|机器人分析");
        builder.Append(Out.Tab("</div>", "<br />"));

        string strText = "开始时间:,结束时间:,";
        string strName = "stTime,ovTime,act";
        string strType = "date,date,hidden";
        string strValu = "" + sTime.ToString("yyyy-MM-dd HH:mm:ss") + "'" + oTime.ToString("yyyy-MM-dd HH:mm:ss") + "'";
        string strEmpt = "false,false,false";
        string strIdea = "/";
        string strOthe = "进行查询,klsf.aspx?act=tstate&amp;ptype=" + ptype + ",post,1,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        long TimeBuyCent = 0;
        long TimeWinCent = 0;

        if (ptype == 0)
        {
            TimeBuyCent = new BCW.BLL.klsfpay().GetSumPrices("State>0 and AddTime > '" + sTime + "' and AddTime <'" + oTime + "'");
            TimeWinCent = new BCW.BLL.klsfpay().GetSumWinCent("State>0 and AddTime > '" + sTime + "' and AddTime <'" + oTime + "'");
        }
        if (ptype == 1)
        {
            TimeBuyCent = new BCW.BLL.klsfpay().GetSumPrices("State>0  and isRoBot=0  and UsID in (select ID from tb_User where IsSpier!=1)  and AddTime > '" + sTime + "' and AddTime <'" + oTime + "'");
            TimeWinCent = new BCW.BLL.klsfpay().GetSumWinCent("State>0 and isRoBot=0  and UsID in (select ID from tb_User where IsSpier!=1)  and AddTime > '" + sTime + "' and AddTime <'" + oTime + "'");
        }
        if (ptype == 2)
        {
            TimeBuyCent = new BCW.BLL.klsfpay().GetSumPrices("State>0 and isRoBot=1  and AddTime > '" + sTime + "' and AddTime <'" + oTime + "'");
            TimeWinCent = new BCW.BLL.klsfpay().GetSumWinCent("State>0  and isRoBot=1 and AddTime > '" + sTime + "' and AddTime <'" + oTime + "'");
        }

        int IsSWB = Utils.ParseInt(ub.GetSub("klsfSWB", xmlPath));
        string str = string.Empty;
        if (ptype == 0)
            str = "全部分析";
        if (ptype == 1)
            str = "用户分析";
        if (ptype == 2)
            str = "机器人分析";
        if (IsSWB == 0)
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("从" + sTime.ToString("yyyy-MM-dd HH:mm:ss") + "到" + oTime.ToString("yyyy-MM-dd HH:mm:ss") + str + "<br />");
            builder.Append("下注:" + TimeBuyCent + "快乐币<br />");
            builder.Append("返彩:" + TimeWinCent + "快乐币<br />");
            builder.Append("净赢:" + (TimeBuyCent - TimeWinCent) + "快乐币");
            builder.Append(Out.Tab("</div>", Out.Hr()));
        }
        else
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("从" + sTime.ToString("yyyy-MM-dd HH:mm:ss") + "到" + oTime.ToString("yyyy-MM-dd HH:mm:ss") + str + "<br />");
            builder.Append("下注:" + TimeBuyCent + "" + ub.Get("SiteBz") + "<br />");
            builder.Append("返彩:" + TimeWinCent + "" + ub.Get("SiteBz") + "<br />");
            builder.Append("净赢:" + (TimeBuyCent - TimeWinCent) + "" + ub.Get("SiteBz") + "");
            builder.Append(Out.Tab("</div>", Out.Hr()));
        }

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("说明：【用户分析】是会员用户记录分析");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("klsf.aspx?act=stat") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    /// <summry>
    /// 游戏查询
    /// </summry>
    //查询期号信息（下注与中奖）
    private void ChaxunPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[1-9]\d*$", "0"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-2]\d*$", "1"));

        Master.Title = "期号详情";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx") + "\">" + GameName + "</a>&gt;期号详情");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("期号查询|" + "<a href=\"" + Utils.getUrl("klsf.aspx?act=usidcx") + "\">会员查询</a>" + "|<a href=\"" + Utils.getUrl("klsf.aspx?act=jiangcx") + "\">往期分析</a>|<a href=\"" + Utils.getUrl("klsf.aspx?act=case") + "\">查兑奖</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("查看下注/中奖记录");
        builder.Append(Out.Tab("</div>", ""));

        //开始期数
        int number1 = int.Parse(Utils.GetRequest("number1", "all", 1, @"^[1-9]\d*$", "0"));

        string strText = "查询期数:/,";
        string strName = "number1,act";
        string strType = "num,hidden";
        string strValu = "" + number1 + "'" + Utils.getPage(0) + "";
        string strEmpt = "true,true,false";
        string strIdea = "/";
        string strOthe = "确认搜索,klsf.aspx?act=chaxun&amp;ptype=" + ptype + ",post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        id = new BCW.DAL.klsflist().id(number1);
        if (id == 0)
        {
            BCW.Model.klsflist mm = new BCW.BLL.klsflist().GetklsflistLast2();
            try
            {
                id = mm.ID;
            }
            catch
            {
                Utils.Error("不存在的记录，输入的期号未开启或者不存在，请检查输入信息是否正确", "");
            }
        }

        BCW.Model.klsflist model = new BCW.BLL.klsflist().Getklsflist(id);

        if (model == null)
        {
            Utils.Error("不存在的记录，输入的期号未开启或者不存在，请检查输入信息是否正确", "");
        }

        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        builder.Append("查看:");
        if (ptype == 1)
            builder.Append("下注|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=chaxun&amp;ptype=1&amp;id=" + id + "&amp;number1=" + number1 + "&amp;backurl=" + Utils.getPage(0) + "") + "\">下注</a>|");

        if (ptype == 2)
            builder.Append("中奖");
        else
            builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=chaxun&amp;ptype=2&amp;id=" + id + "&amp;number1=" + number1 + "&amp;backurl=" + Utils.getPage(0) + "") + "\">中奖</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        if (ptype == 1)
            strWhere += "klsfId=" + model.klsfId + "";
        else
            strWhere += "klsfId=" + model.klsfId + " and WinCent>0 and iWin>0";

        string[] pageValUrl = { "act", "id", "ptype", "number1", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        // 开始读取列表
        IList<BCW.Model.klsfpay> listSSCpay = new BCW.BLL.klsfpay().Getklsfpays(pageIndex, pageSize, strWhere, out recordCount);
        if (listSSCpay.Count > 0)
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("第" + model.klsfId + "期开出:<b>" + model.Result + "|和:" + GetHe(model.Result) + "</b>");
            if (ptype == 1)
                builder.Append("<br />共" + recordCount + "注下注");
            else
                builder.Append("<br />共" + recordCount + "注中奖");

            builder.Append(Out.Tab("</div>", "<br />"));

            int k = 1;
            foreach (BCW.Model.klsfpay n in listSSCpay)
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

                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "</a>");

                #region 根据版本选择快乐币和酷币的获取和显示
                int IsSWB = Utils.ParseInt(ub.GetSub("klsfSWB", xmlPath));
                if (IsSWB == 0)
                {
                    builder.Append("<b>[" + OutType(n.Types) + "]</b>位号:" + n.Notes + "/每注" + n.Price + "快乐币/共买" + n.iCount + "注[" + DT.FormatDate(n.AddTime, 5) + "]");
                }
                else
                {
                    builder.Append("<b>[" + OutType(n.Types) + "]</b>位号:" + n.Notes + "/每注" + n.Price + "" + ub.Get("SiteBz") + "/共买" + n.iCount + "注/赔率" + n.Odds + "[" + DT.FormatDate(n.AddTime, 5) + "]");
                }
                #endregion

                if (n.WinCent > 0)
                {
                    #region 根据版本选择快乐币和酷币的获取和显示
                    if (IsSWB == 0)
                    {
                        builder.Append("&nbsp;中" + n.iWin + "注/ 赢" + n.WinCent + "快乐币");
                    }
                    else
                    {
                        builder.Append("&nbsp; 中" + n.iWin + "注/赢" + n.WinCent + "" + ub.Get("SiteBz") + "");
                    }
                    #endregion

                }
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

        }
        else
        {
            if (ptype == 1)
            {
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("第" + model.klsfId + "期开出:<b>" + model.Result + "|和:" + GetHe(model.Result) + "</b>");
                builder.Append("<br />共0注投注");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Div("div", "没有相关记录..."));
            }
            else if (ptype == 2)
            {
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("第" + model.klsfId + "期开出:<b>" + model.Result + "|和:" + GetHe(model.Result) + "</b>");
                builder.Append("<br />共0注中奖");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Div("div", "没有相关记录..."));
            }

        }

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    //根据会员ＩＤ查询信息
    private void UsidcxPage()
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-2]\d*$", "1"));

        Master.Title = "会员查询";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx") + "\">" + GameName + "</a>&gt;会员查询");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=chaxun") + "\">期号查询</a>" + "|会员查询" + "|<a href=\"" + Utils.getUrl("klsf.aspx?act=jiangcx") + "\">往期分析</a>|<a href=\"" + Utils.getUrl("klsf.aspx?act=case") + "\">查兑奖</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("查看下注/中奖记录");
        builder.Append(Out.Tab("</div>", ""));

        int UsID = int.Parse(Utils.GetRequest("UsID", "all", 1, @"^[1-9]\d*$", "0"));

        string strText = "查询会员ID:/,";
        string strName = "UsID,act";
        string strType = "num,hidden";
        string strValu = "" + UsID + "'" + Utils.getPage(0) + "";
        string strEmpt = "true,true,false";
        string strIdea = "/";
        string strOthe = "确认搜索,klsf.aspx?act=usidcx&amp;ptype=" + ptype + ",post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));


        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        builder.Append("查看:");
        if (ptype == 1)
            builder.Append("下注|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=usidcx&amp;ptype=1&amp;UsID=" + UsID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">下注</a>|");

        if (ptype == 2)
            builder.Append("中奖");
        else
            builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=usidcx&amp;ptype=2&amp;UsID=" + UsID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">中奖</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;


        if (ptype == 1)
            strWhere += "UsID=" + UsID + "";
        else
            strWhere += "UsID=" + UsID + " and iWin>0 and winCent>0";

        string[] pageValUrl = { "act", "id", "ptype", "UsID", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.klsfpay> listSSCpay = new BCW.BLL.klsfpay().Getklsfpays(pageIndex, pageSize, strWhere, out recordCount);
        if (listSSCpay.Count > 0)
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            if (UsID == 0)
            {
                builder.Append("请输入会员ID");
            }
            else
            {
                builder.Append("会员ID" + UsID + "<br />");
            }
            int k = 1;
            foreach (BCW.Model.klsfpay n in listSSCpay)
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

                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "</a>");

                #region 根据版本选择快乐币和酷币的获取和显示
                int IsSWB = Utils.ParseInt(ub.GetSub("klsfSWB", xmlPath));
                if (IsSWB == 0)
                {
                    builder.Append("<b>" + n.klsfId + "期[" + OutType(n.Types) + "]</b>位号:" + n.Notes + "/每注" + n.Price + "快乐币/共买" + n.iCount + "注[" + DT.FormatDate(n.AddTime, 5) + "]");
                }
                else
                {
                    builder.Append("<b>" + n.klsfId + "期[" + OutType(n.Types) + "]</b>位号:" + n.Notes + "/每注" + n.Price + "" + ub.Get("SiteBz") + "/共买" + n.iCount + "注/赔率" + n.Odds + "[" + DT.FormatDate(n.AddTime, 5) + "]");
                }
                #endregion

                if (n.WinCent > 0)
                {
                    #region 根据版本选择快乐币和酷币的获取和显示
                    if (IsSWB == 0)
                    {
                        builder.Append("&nbsp;中" + n.iWin + "注/ 赢" + n.WinCent + "快乐币");
                    }
                    else
                    {
                        builder.Append("&nbsp; 中" + n.iWin + "注/赢" + n.WinCent + "" + ub.Get("SiteBz") + "");
                    }
                    #endregion

                }
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

        }
        else
        {
            if (ptype == 1)
            {
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("<br />共0注投注");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Div("div", "没有相关记录..."));
            }
            else if (ptype == 2)
            {
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("<br />共0注中奖");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Div("div", "没有相关记录..."));
            }

        }

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("klsf.aspx") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    //奖池查询信息
    private void JiangcxPage()
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]\d*$", "1"));

        Master.Title = "奖池查询";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx") + "\">" + GameName + "</a>&gt;往期分析");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx?act=chaxun") + "\">期号查询</a>" + "|<a href=\"" + Utils.getUrl("klsf.aspx?act=usidcx") + "\">会员查询</a>" + "|往期分析|<a href=\"" + Utils.getUrl("klsf.aspx?act=case") + "\">查兑奖</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("〓往期分析〓");
        builder.Append(Out.Tab("</div>", ""));

        #region 表格的提交和确认
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        string searchDate = DateTime.Now.ToString("yyyyMMdd");

        if (Utils.ToSChinese(ac) == "确定修改")
        {
            searchDate = Utils.GetRequest("date", "all", 2, @"^(\d\d\d\d\d\d\d\d)$", "请输入正确的时间格式");
        }

        //输入框
        string strText = "查询日期(格式20160808):/,";
        string strName = "date,backurl";
        string strType = "text,hidden";
        string strValu = "" + searchDate + "'" + Utils.getPage(1) + "";
        string strEmpt = "false,false";
        string strIdea = "/";
        string strOthe = "确定修改|reset,klsf.aspx?act=jiangcx,post,1,red|blue";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        #endregion

        #region 从数据库中调出符合条件的数据并统计
        string strWhere = string.Empty;
        strWhere += "klsfId like \'" + searchDate.Remove(0, 2) + "%\' and State <> 0";
        DataSet ds = new BCW.BLL.klsflist().GetList("Result", strWhere);
        int[] num = new int[33];

        //数据总结
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            string[] Result = ds.Tables[0].Rows[i]["Result"].ToString().Split(',');
            int temp = 0;
            int sum = 0;
            string temps = string.Empty;
            for (int j = 0; j < Result.Length; j++)
            {
                temp = Convert.ToInt32(Result[j]);
                num[temp]++;
                sum += temp;
            }
            temps = Convert.ToString(sum);
            temps = temps.Substring(temps.Length - 1, 1);
            temp = Convert.ToInt32(temps);
            if (temp > 4)
                num[21]++;
            else
                num[22]++;
            if (sum % 2 != 0)
                num[23]++;
            else
                num[24]++;

            if (GetLHF(ds.Tables[0].Rows[i]["Result"].ToString(), 1) == 1)
            {
                num[25]++;
            }
            else//   if (Convert.ToInt32(resu[0]) < Convert.ToInt32(resu[7]))
            {
                num[26]++;
            }
            if (GetLHF(ds.Tables[0].Rows[i]["Result"].ToString(), 2) == 1)
            {
                num[27]++;
            }
            else// if (Convert.ToInt32(resu[6]) < Convert.ToInt32(resu[1]))
            {
                num[28]++;
            }
            if (GetLHF(ds.Tables[0].Rows[i]["Result"].ToString(), 3) == 1)
            {
                num[29]++;
            }
            else//  if (Convert.ToInt32(resu[5]) < Convert.ToInt32(resu[2]))
            {
                num[30]++;
            }
            if (GetLHF(ds.Tables[0].Rows[i]["Result"].ToString(), 4) == 1)
            {
                num[31]++;
            }
            else//  if (Convert.ToInt32(resu[4]) < Convert.ToInt32(resu[3]))
            {
                num[32]++;
            }
        }
        #endregion

        #region 显示统计结果
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<h class=\"text\" style=\"color:#FF7F00\">冷热奖号统计</h>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", Out.Hr()));
        for (int i = 1; i < 21; i++)
        {
            if (i % 4 == 0 && i != 0)
                builder.Append("<b>" + i + "：<b style=\"color:red\">" + num[i] + "</b>次</b><br />");
            else
            {
                if (i < 10)
                    builder.Append("<b>" + i + "：<b style=\"color:red\">&nbsp;&nbsp;" + num[i] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
                else if (i > 9 && i < 13)
                    builder.Append("<b>" + i + "：<b style=\"color:red\">" + num[i] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
                else
                    builder.Append("<b>" + i + "：<b style=\"color:red\">" + num[i] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
            }
        }
        builder.Append(Out.Tab("</div>", Out.RHr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<h class=\"text\" style=\"color:#FF7F00\">大小单双统计</h>");
        builder.Append(Out.Tab("</div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b>大：<b style=\"color:red\">" + num[21] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>小：<b style=\"color:red\">" + num[22] + "</b>次</b><br />");
        builder.Append("<b>单：<b style=\"color:red\">" + num[23] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>双：<b style=\"color:red\">" + num[24] + "</b>次</b><br />");
        builder.Append(Out.Tab("</div>", Out.RHr()));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<h class=\"text\" style=\"color:#FF7F00\">龙虎统计</h>");
        builder.Append(Out.Tab("</div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b>龙（1与8位）：<b style=\"color:red\">" + num[25] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>虎（1与8位）：<b style=\"color:red\">" + num[26] + "</b>次</b><br />");
        builder.Append("<b>龙（2与7位）：<b style=\"color:red\">" + num[27] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>虎（2与7位）：<b style=\"color:red\">" + num[28] + "</b>次</b><br />");
        builder.Append("<b>龙（3与6位）：<b style=\"color:red\">" + num[29] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>虎（3与6位）：<b style=\"color:red\">" + num[30] + "</b>次</b><br />");
        builder.Append("<b>龙（4与5位）：<b style=\"color:red\">" + num[31] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>虎（4与5位）：<b style=\"color:red\">" + num[32] + "</b>次</b>");
        builder.Append(Out.Tab("</div>", ""));
        #endregion

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    /// <summary>
    /// 返彩的方法
    /// </summary>
    private void klsffc()
    {
        //开始返彩
        DataSet ds = new BCW.BLL.klsfpay().GetList("ID,Types,klsfId,UsID,UsName,Price,Notes,Result,Prices,Odds", "State=0 and Result<>''");  //选取没有返彩的数据 state=0未返彩
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                int ID = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());   //该条数据的ID
                int Types = int.Parse(ds.Tables[0].Rows[i]["Types"].ToString()); //该条记录的投注类型
                int klsfId = int.Parse(ds.Tables[0].Rows[i]["klsfId"].ToString()); //该条记录的期数
                string Notes = ds.Tables[0].Rows[i]["Notes"].ToString();  //该用户在该期的投注
                string[] Temp = Notes.Split(',');
                int UsID = int.Parse(ds.Tables[0].Rows[i]["UsID"].ToString()); //该用户的ID
                string UsName = ds.Tables[0].Rows[i]["UsName"].ToString(); //该用户的用户名
                string Result = ds.Tables[0].Rows[i]["Result"].ToString().Replace(" ", ""); //该期的开奖结果
                long Price = long.Parse(ds.Tables[0].Rows[i]["Price"].ToString()); //每注的单价
                decimal Odds = decimal.Parse(ds.Tables[0].Rows[i]["Odds"].ToString()); //赔率
                int IsSWB = Utils.ParseInt(ub.GetSub("klsfState", xmlPath));
                long WinCent = 0;
                //int Prices = int.Parse(ds.Tables[0].Rows[i]["Prices"].ToString()); //这一次投注的总价



                if (Types == 1 || Types == 3 || Types == 5 || Types == 7)  //胆拖类型的返彩
                {
                    string[] iNum_kj = Result.Split(',');  //存放结果
                    string[] iNum_kj_hm = Result.Split(',');  //存放结果
                    string[] iNum_zj = Notes.Split('|');  //把胆码和拖码分开存储
                    string[] iNum_zj_d = iNum_zj[0].Split(','); //胆码
                    string[] iNum_zj_t = iNum_zj[1].Split(',');  //拖码

                    int zj_ds = 0; //中奖的胆码数
                    int zj_ts = 0; //中奖的拖码数
                    int zj = 0; //中奖的注数

                    //统计胆码的中奖数
                    for (int i1 = 0; i1 < iNum_zj_d.Length; i1++)
                    {
                        for (int p = 0; p < iNum_kj.Length; p++)
                        {
                            if (iNum_zj_d[i1] == iNum_kj[p])
                            {
                                zj_ds += 1;
                            }
                        }
                    }

                    if (zj_ds == iNum_zj_d.Length) //判断胆码是否全中
                    {
                        //统计拖码的中奖数
                        for (int i1 = 0; i1 < iNum_zj_t.Length; i1++)
                        {
                            for (int p = 0; p < iNum_kj.Length; p++)
                            {
                                if (iNum_zj_t[i1] == iNum_kj[p])
                                {
                                    zj_ts += 1;
                                }
                            }
                        }

                        switch (Types)
                        {
                            case 1: //五胆拖类型
                                {
                                    if (zj_ds + zj_ts >= 5)
                                    {
                                        zj = C(zj_ts, 5 - iNum_zj_d.Length);
                                        WinCent = Convert.ToInt64(zj * (Odds * Price));
                                    }
                                }
                                break;
                            case 3: //四胆拖类型
                                {
                                    if (zj_ds + zj_ts >= 4)
                                    {
                                        zj = C(zj_ts, 4 - iNum_zj_d.Length);
                                        WinCent = Convert.ToInt64(zj * (Odds * Price));
                                    }
                                }
                                break;
                            case 5: //三胆拖类型
                                {
                                    if (zj_ds + zj_ts >= 3)
                                    {
                                        zj = C(zj_ts, 3 - iNum_zj_d.Length);
                                        WinCent = Convert.ToInt64(zj * (Odds * Price));
                                    }
                                }
                                break;
                            default: //二胆拖类型
                                {
                                    if (zj_ds + zj_ts >= 2)
                                    {
                                        zj = C(zj_ts, 2 - iNum_zj_d.Length);
                                        WinCent = Convert.ToInt64(zj * (Odds * Price));
                                    }
                                }
                                break;
                        }
                    }
                    else  //胆码没有全中则没有中奖
                    {
                        zj = 0;
                        WinCent = 0;
                    }
                    //把中奖结果更新到数据库
                    string str_SQL = string.Format(@"UPDATE [tb_klsfpay]  SET [WinCent] = {0},[iWin]={1}  WHERE klsfId={2} and ID = {3}", WinCent, zj, klsfId, ID);
                    //如果中奖了就发一条内线
                    if (zj > 0)
                    {
                        BCW.Data.SqlHelper.ExecuteSql(str_SQL);
                        #region 根据版本选择快乐币和酷币的获取和显示
                        if (IsSWB == 0)
                        {
                            new BCW.BLL.Guest().Add(1, UsID, UsName, "您的[url=/bbs/game/klsf.aspx]" + GameName + "[/url]:" + klsfId + "期已经开奖，获得了" + WinCent + "快乐币。");//开奖提示信息,1表示开奖信息
                        }
                        else
                        {
                            new BCW.BLL.Guest().Add(1, UsID, UsName, "您的[url=/bbs/game/klsf.aspx]" + GameName + "[/url]:" + klsfId + "期已经开奖，获得了" + WinCent + ub.Get("SiteBz") + "[url=/bbs/game/klsf.aspx?act=case]>>马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                        }
                        #endregion
                    }
                }
                else if (Types == 2 || Types == 4 || Types == 6 || Types == 8)  //任选普通类型的返彩
                {
                    string[] iNum_kj = Result.Split(',');  //该期开奖的结果
                    string[] iNum_zj = Notes.Split(',');  //该期该用户购买的彩票

                    int zj_zs = 0; //统计中奖注数

                    for (int fs = 0; fs < iNum_zj.Length; fs++)
                        for (int p = 0; p < iNum_kj.Length; p++)
                            if (string.Compare(iNum_zj[fs], iNum_kj[p]) == 0)
                                zj_zs += 1;


                    switch (Types)
                    {
                        case 2: //任选五普通
                            {
                                if (zj_zs >= 5)
                                {
                                    zj_zs = C(zj_zs, 5);
                                    WinCent = Convert.ToInt64(zj_zs * (Odds * Price));
                                }
                                else
                                    zj_zs = 0;
                            }
                            break;
                        case 4: //任选四普通
                            {
                                if (zj_zs >= 4)
                                {
                                    zj_zs = C(zj_zs, 4);
                                    WinCent = Convert.ToInt64(zj_zs * (Odds * Price));
                                }
                                else
                                    zj_zs = 0;
                            }
                            break;
                        case 6: //任选三普通
                            {
                                if (zj_zs >= 3)
                                {
                                    zj_zs = C(zj_zs, 3);
                                    WinCent = Convert.ToInt64(zj_zs * (Odds * Price));
                                }
                                else
                                    zj_zs = 0;
                            }
                            break;
                        default: //任选二普通
                            {

                                if (zj_zs >= 2)
                                {
                                    zj_zs = C(zj_zs, 2);
                                    WinCent = Convert.ToInt64(zj_zs * (Odds * Price));
                                }
                                else
                                    zj_zs = 0;
                            }
                            break;
                    }
                    //把中奖结果更新到数据库
                    string str_SQL = string.Format(@"UPDATE [tb_klsfpay]  SET [WinCent] = {0},[iWin]={1}  WHERE klsfId={2} and ID = {3}", WinCent, zj_zs, klsfId, ID);
                    //如果中奖了就发一条内线通知
                    if (zj_zs > 0)
                    {
                        BCW.Data.SqlHelper.ExecuteSql(str_SQL);
                        #region 根据版本选择快乐币和酷币的获取和显示
                        if (IsSWB == 0)
                        {
                            new BCW.BLL.Guest().Add(1, UsID, UsName, "您的[url=/bbs/game/klsf.aspx]" + GameName + "[/url]:" + klsfId + "期已经开奖，获得了" + WinCent + "快乐币。");//开奖提示信息,1表示开奖信息
                        }
                        else
                        {
                            new BCW.BLL.Guest().Add(1, UsID, UsName, "您的[url=/bbs/game/klsf.aspx]" + GameName + "[/url]:" + klsfId + "期已经开奖，获得了" + WinCent + ub.Get("SiteBz") + "[url=/bbs/game/klsf.aspx?act=case]>>马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                        }
                        #endregion
                    }
                }
                else if (Types == 9) //连二直选
                {
                    string[] iNum_kj = Result.Split(',');
                    string[] iNum_kj_q = Result.Split(',');
                    string[] iNum_kj_h = Result.Split(',');
                    string[] iNum_zj = Notes.Split('|');
                    string[] iNum_zj_q = iNum_zj[0].Split(',');
                    string[] iNum_zj_h = iNum_zj[1].Split(',');

                    for (int c = 0; c < iNum_kj.Length; c++) //把对比投注组设为0，作判断依据
                    {
                        iNum_kj_q[c] = "0";
                        iNum_kj_h[c] = "0";
                    }

                    int zj_zs = 0; //中奖的注数

                    for (int j = 0; j < iNum_zj_q.Length; j++) //寻找到匹配的数字，替换掉对比组内相对应位置的"0"
                        for (int p = 0; p < iNum_kj.Length; p++)
                            if (iNum_zj_q[j] == iNum_kj[p])
                                iNum_kj_q[p] = iNum_kj[p];

                    for (int j = 0; j < iNum_zj_h.Length; j++) //寻找到匹配的数字，替换掉对比组内相对应位置的"0"
                        for (int p = 0; p < iNum_kj.Length; p++)
                            if (iNum_zj_h[j] == iNum_kj[p])
                                iNum_kj_h[p] = iNum_kj[p];

                    for (int h = 0; h < iNum_kj.Length - 1; h++) //如果对应位置不是"0"则是中奖
                    {
                        if ((iNum_kj_q[h] != "0") && (iNum_kj_h[h + 1] != "0"))
                        {
                            zj_zs += 1;
                        }
                    }
                    WinCent = Convert.ToInt64(zj_zs * (Odds * Price));
                    //把中奖结果更新到数据库
                    string str_SQL = string.Format(@"UPDATE [tb_klsfpay]  SET [WinCent] = {0},[iWin]={1}  WHERE klsfId={2} and ID = {3}", WinCent, zj_zs, klsfId, ID);
                    //如果中奖了就发一条内线通知
                    if (zj_zs > 0)
                    {
                        BCW.Data.SqlHelper.ExecuteSql(str_SQL);
                        #region 根据版本选择快乐币和酷币的获取和显示
                        if (IsSWB == 0)
                        {
                            new BCW.BLL.Guest().Add(1, UsID, UsName, "您的[url=/bbs/game/klsf.aspx]" + GameName + "[/url]:" + klsfId + "期已经开奖，获得了" + WinCent + "快乐币。");//开奖提示信息,1表示开奖信息
                        }
                        else
                        {
                            new BCW.BLL.Guest().Add(1, UsID, UsName, "您的[url=/bbs/game/klsf.aspx]" + GameName + "[/url]:" + klsfId + "期已经开奖，获得了" + WinCent + ub.Get("SiteBz") + "[url=/bbs/game/klsf.aspx?act=case]>>马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                        }
                        #endregion
                    }
                }
                else if (Types == 10) //连二组选
                {
                    string[] iNum_kj = Result.Split(',');
                    string[] iNum_kj_hm = Result.Split(',');
                    string[] iNum_zj = Notes.Split(',');

                    for (int c = 0; c < iNum_kj_hm.Length; c++)
                    {
                        iNum_kj_hm[c] = "9";
                    }

                    int zj_zs = 0;

                    for (int j = 0; j < iNum_zj.Length; j++)
                        for (int p = 0; p < iNum_kj.Length; p++)
                            if (iNum_zj[j] == iNum_kj[p])
                                iNum_kj_hm[p] = p.ToString();

                    for (int h = 0; h < iNum_kj_hm.Length - 1; h++)
                        if ((iNum_kj_hm[h] != "9") && (iNum_kj_hm[h + 1] != "9"))
                            zj_zs += 1;


                    WinCent = Convert.ToInt64(zj_zs * (Odds * Price));
                    //把中奖结果更新到数据库
                    string str_SQL = string.Format(@"UPDATE [tb_klsfpay]  SET [WinCent] = {0},[iWin]={1}  WHERE klsfId={2} and ID = {3}", WinCent, zj_zs, klsfId, ID);
                    //如果中奖了就发一条内线通知
                    if (zj_zs > 0)
                    {
                        BCW.Data.SqlHelper.ExecuteSql(str_SQL);
                        #region 根据版本选择快乐币和酷币的获取和显示
                        if (IsSWB == 0)
                        {
                            new BCW.BLL.Guest().Add(1, UsID, UsName, "您的[url=/bbs/game/klsf.aspx]" + GameName + "[/url]:" + klsfId + "期已经开奖，获得了" + WinCent + "快乐币。");//开奖提示信息,1表示开奖信息
                        }
                        else
                        {
                            new BCW.BLL.Guest().Add(1, UsID, UsName, "您的[url=/bbs/game/klsf.aspx]" + GameName + "[/url]:" + klsfId + "期已经开奖，获得了" + WinCent + ub.Get("SiteBz") + "[url=/bbs/game/klsf.aspx?act=case]>>马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                        }
                        #endregion
                    }
                }
                else if (Types == 11)//前一红投
                {
                    string[] iNum_kj = Result.Split(',');
                    string[] iNum_zj = Notes.Split(',');
                    int zj_zs = 0;
                    for (int j = 0; j < iNum_zj.Length; j++)
                    {
                        if (iNum_zj[j] == iNum_kj[0])
                        {
                            zj_zs += 1;
                        }
                    }

                    WinCent = Convert.ToInt64(zj_zs * (Odds * Price));
                    //把中奖结果更新到数据库
                    string str_SQL = string.Format(@"UPDATE [tb_klsfpay]  SET [WinCent] = {0},[iWin]={1}  WHERE klsfId={2} and ID = {3}", WinCent, zj_zs, klsfId, ID);
                    //如果中奖了就发一条内线通知
                    if (zj_zs > 0)
                    {
                        BCW.Data.SqlHelper.ExecuteSql(str_SQL);
                        #region 根据版本选择快乐币和酷币的获取和显示
                        if (IsSWB == 0)
                        {
                            new BCW.BLL.Guest().Add(1, UsID, UsName, "您的[url=/bbs/game/klsf.aspx]" + GameName + "[/url]:" + klsfId + "期已经开奖，获得了" + WinCent + "快乐币。");//开奖提示信息,1表示开奖信息
                        }
                        else
                        {
                            new BCW.BLL.Guest().Add(1, UsID, UsName, "您的[url=/bbs/game/klsf.aspx]" + GameName + "[/url]:" + klsfId + "期已经开奖，获得了" + WinCent + ub.Get("SiteBz") + "[url=/bbs/game/klsf.aspx?act=case]>>马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                        }
                        #endregion
                    }

                }
                else if (Types == 12)//前一数投
                {
                    string[] iNum_kj = Result.Split(',');
                    string[] iNum_zj = Notes.Split(',');
                    int zj_zs = 0;
                    for (int j = 0; j < iNum_zj.Length; j++)
                        if (iNum_zj[j] == iNum_kj[0])
                            zj_zs += 1;

                    WinCent = Convert.ToInt64(zj_zs * (Odds * Price));
                    //把中奖结果更新到数据库
                    string str_SQL = string.Format(@"UPDATE [tb_klsfpay]  SET [WinCent] = {0},[iWin]={1}  WHERE klsfId={2} and ID = {3}", WinCent, zj_zs, klsfId, ID);
                    //如果中奖了就发一条内线通知
                    if (zj_zs > 0)
                    {
                        //builder.Append(Out.Tab("<div class=\"title\">", ""));
                        //builder.Append(Out.Tab("中奖金额:" + str_SQL + " ", ""));
                        //builder.Append(Out.Tab("</div>", "<br />"));//在前台显示变量得值 测试用
                        BCW.Data.SqlHelper.ExecuteSql(str_SQL);
                        #region 根据版本选择快乐币和酷币的获取和显示
                        if (IsSWB == 0)
                        {
                            new BCW.BLL.Guest().Add(1, UsID, UsName, "您的[url=/bbs/game/klsf.aspx]" + GameName + "[/url]:" + klsfId + "期已经开奖，获得了" + WinCent + "快乐币。");//开奖提示信息,1表示开奖信息
                        }
                        else
                        {
                            new BCW.BLL.Guest().Add(1, UsID, UsName, "您的[url=/bbs/game/klsf.aspx]" + GameName + "[/url]:" + klsfId + "期已经开奖，获得了" + WinCent + ub.Get("SiteBz") + "[url=/bbs/game/klsf.aspx?act=case]>>马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                        }
                        #endregion
                    }
                }
                else if (Types == 13)
                {
                    string[] iNum_kj = Result.Split(',');
                    string zj = Notes;
                    int sum = 0;
                    int zj_zs = 0;

                    try
                    {
                        for (int j = 0; j < iNum_kj.Length; j++)
                        {
                            sum += Convert.ToInt32(iNum_kj[j]);
                        }
                        string temp = Convert.ToString(sum);
                        temp = temp.Substring(temp.Length - 1, 1);
                        sum = Convert.ToInt32(temp);
                        if (sum >= 5 && zj == "大")
                        {
                            zj_zs = 1;
                            WinCent = Convert.ToInt64(zj_zs * (Odds * Price));
                        }
                        else if (sum <= 4 && zj == "小")
                        {
                            zj_zs = 1;
                            WinCent = Convert.ToInt64(zj_zs * (Odds * Price));
                        }
                    }
                    catch
                    { }
                    //把中奖结果更新到数据库
                    string str_SQL = string.Format(@"UPDATE [tb_klsfpay]  SET [WinCent] = {0},[iWin]={1}  WHERE klsfId={2} and ID = {3}", WinCent, zj_zs, klsfId, ID);
                    //如果中奖了就发一条内线通知
                    if (zj_zs > 0)
                    {
                        BCW.Data.SqlHelper.ExecuteSql(str_SQL);
                        #region 根据版本选择快乐币和酷币的获取和显示
                        if (IsSWB == 0)
                        {
                            new BCW.BLL.Guest().Add(1, UsID, UsName, "您的[url=/bbs/game/klsf.aspx]" + GameName + "[/url]:" + klsfId + "期已经开奖，获得了" + WinCent + "快乐币。");//开奖提示信息,1表示开奖信息
                        }
                        else
                        {
                            new BCW.BLL.Guest().Add(1, UsID, UsName, "您的[url=/bbs/game/klsf.aspx]" + GameName + "[/url]:" + klsfId + "期已经开奖，获得了" + WinCent + ub.Get("SiteBz") + "[url=/bbs/game/klsf.aspx?act=case]>>马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                        }
                        #endregion
                    }
                }
                else if (Types == 14)
                {
                    string[] iNum_kj = Result.Split(',');
                    string zj = Notes;
                    int sum = 0;
                    int zj_zs = 0;

                    try
                    {
                        for (int j = 0; j < iNum_kj.Length; j++)
                        {
                            sum += Convert.ToInt32(iNum_kj[j]);
                        }
                        if (sum % 2 != 0 && zj == "单")
                        {
                            zj_zs = 1;
                            WinCent = Convert.ToInt64(zj_zs * (Odds * Price));
                        }
                        else if (sum % 2 == 0 && zj == "双")
                        {
                            zj_zs = 1;
                            WinCent = Convert.ToInt64(zj_zs * (Odds * Price));
                        }
                    }
                    catch
                    { }
                    //把中奖结果更新到数据库
                    string str_SQL = string.Format(@"UPDATE [tb_klsfpay]  SET [WinCent] = {0},[iWin]={1}  WHERE klsfId={2} and ID = {3}", WinCent, zj_zs, klsfId, ID);
                    //如果中奖了就发一条内线通知
                    if (zj_zs > 0)
                    {
                        BCW.Data.SqlHelper.ExecuteSql(str_SQL);
                        #region 根据版本选择快乐币和酷币的获取和显示
                        string PlayUsID = ub.GetSub("klsfklsfRobotId", "/Controls/klsf.xml");
                        bool IsRob = false;
                        if (PlayUsID != "")
                        {
                            string[] sNum = PlayUsID.Split('#');
                            int[] iNum = new int[sNum.Length];
                            for (int j = 0; j < sNum.Length; j++)
                            {
                                iNum[j] = int.Parse(sNum[j]);
                                if (iNum[j] == UsID)
                                {
                                    IsRob = true;
                                    break;
                                }
                            }
                        }
                        if (!IsRob)
                        {
                            if (IsSWB == 0)
                            {
                                new BCW.BLL.Guest().Add(1, UsID, UsName, "您的[url=/bbs/game/klsf.aspx]" + GameName + "[/url]:" + klsfId + "期已经开奖，获得了" + WinCent + "快乐币。");//开奖提示信息,1表示开奖信息
                            }
                            else
                            {
                                new BCW.BLL.Guest().Add(1, UsID, UsName, "您的[url=/bbs/game/klsf.aspx]" + GameName + "[/url]:" + klsfId + "期已经开奖，获得了" + WinCent + ub.Get("SiteBz") + "[url=/bbs/game/klsf.aspx?act=case]>>马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                            }
                        }
                        #endregion

                    }
                }
                else if (Types == 15)//龙虎
                {
                    string[] iNum_kj = Result.Split(',');
                    int num1, num2, num3, num4, num5, num6, num7, num8;
                    num1 = Convert.ToInt32(iNum_kj[0]);
                    num2 = Convert.ToInt32(iNum_kj[1]);
                    num3 = Convert.ToInt32(iNum_kj[2]);
                    num4 = Convert.ToInt32(iNum_kj[3]);
                    num5 = Convert.ToInt32(iNum_kj[4]);
                    num6 = Convert.ToInt32(iNum_kj[5]);
                    num7 = Convert.ToInt32(iNum_kj[6]);
                    num8 = Convert.ToInt32(iNum_kj[7]);
                    string zj = Notes;
                    int zhug = 0;

                    try
                    {
                        if (num1 > num8 && zj == "龙（1与8位）")
                        {
                            zhug = 1;
                            WinCent = Convert.ToInt64(zhug * (Odds * Price));
                        }
                        if (num8 > num1 && zj == "虎（1与8位）")
                        {
                            zhug = 1;
                            WinCent = Convert.ToInt64(zhug * (Odds * Price));
                        }
                        if (num2 > num7 && zj == "龙（2与7位）")
                        {
                            zhug = 1;
                            WinCent = Convert.ToInt64(zhug * (Odds * Price));
                        }
                        if (num7 > num2 && zj == "虎（2与7位）")
                        {
                            zhug = 1;
                            WinCent = Convert.ToInt64(zhug * (Odds * Price));
                        }
                        if (num3 > num6 && zj == "龙（3与6位）")
                        {
                            zhug = 1;
                            WinCent = Convert.ToInt64(zhug * (Odds * Price));
                        }
                        if (num6 > num3 && zj == "虎（3与6位）")
                        {
                            zhug = 1;
                            WinCent = Convert.ToInt64(zhug * (Odds * Price));
                        }
                        if (num4 > num5 && zj == "龙（4与5位）")
                        {
                            zhug = 1;
                            WinCent = Convert.ToInt64(zhug * (Odds * Price));
                        }
                        if (num5 > num4 && zj == "虎（4与5位）")
                        {
                            zhug = 1;
                            WinCent = Convert.ToInt64(zhug * (Odds * Price));
                        }

                    }
                    catch
                    { }
                    //把中奖结果更新到数据库
                    string str_SQL = string.Format(@"UPDATE [tb_klsfpay]  SET [WinCent] = {0},[iWin]={1}  WHERE klsfId={2} and ID = {3}", WinCent, zhug, klsfId, ID);
                    //如果中奖了就发一条内线通知
                    if (zhug > 0)
                    {
                        BCW.Data.SqlHelper.ExecuteSql(str_SQL);
                        #region 根据版本选择快乐币和酷币的获取和显示
                        string PlayUsID = ub.GetSub("klsfklsfRobotId", "/Controls/klsf.xml");
                        bool IsRob = false;
                        if (PlayUsID != "")
                        {
                            string[] sNum = PlayUsID.Split('#');
                            int[] iNum = new int[sNum.Length];
                            for (int j = 0; j < sNum.Length; j++)
                            {
                                iNum[j] = int.Parse(sNum[j]);
                                if (iNum[j] == UsID)
                                {
                                    IsRob = true;
                                    break;
                                }
                            }
                        }
                        if (!IsRob)
                        {
                            if (IsSWB == 0)
                            {
                                new BCW.BLL.Guest().Add(1, UsID, UsName, "您的[url=/bbs/game/klsf.aspx]" + GameName + "[/url]:" + klsfId + "期已经开奖，获得了" + WinCent + "快乐币。");//开奖提示信息,1表示开奖信息
                            }
                            else
                            {
                                new BCW.BLL.Guest().Add(1, UsID, UsName, "您的[url=/bbs/game/klsf.aspx]" + GameName + "[/url]:" + klsfId + "期" + OutType(Types) + "已经开奖，获得了" + WinCent + ub.Get("SiteBz") + "[url=/bbs/game/klsf.aspx?act=case]>>马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                            }
                        }
                        #endregion

                    }
                }
                //更新已开奖
                new BCW.BLL.klsfpay().UpdateState(ID, 1);
            }
        }
    }

    #region 计算组合的数量
    static long jc(int N)//阶乘
    {
        long t = 1;

        for (int i = 1; i <= N; i++)
        {
            t *= i;
        }
        return t;
    }
    static long P(int N, int R)//组合的计算公式
    {
        long t = jc(N) / jc(N - R);

        return t;
    }
    static int C(int N, int R)//组合
    {
        long i = P(N, R) / jc(R);
        int t = Convert.ToInt32(i);
        return t;
    }
    #endregion

    #region 计算开奖和值
    public int GetHe(string Result)
    {
        int He = 0;
        string[] num = Result.Split(',');
        for (int i = 0; i < num.Length; i++)
        {
            try
            {
                He += Convert.ToInt32(num[i]);
            }
            catch
            {

            }
        }
        return He;
    }
    #endregion

    #region 计算开奖龙虎分析
    public int GetLHF(string Result, int i)
    {
        int LH = 0;
        string kai1 = string.Empty;
        string kai2 = string.Empty;
        string kai3 = string.Empty;
        string kai4 = string.Empty;
        string[] num = Result.Split(',');
        try
        {
            if (i == 1)
            {
                if (Convert.ToInt32(num[0]) > Convert.ToInt32(num[7]))
                {
                    LH = 1;
                }
                else//   if (Convert.ToInt32(num[0]) < Convert.ToInt32(num[7]))
                {
                    LH = 2;
                }
            }
            if (i == 2)
            {
                if (Convert.ToInt32(num[1]) > Convert.ToInt32(num[6]))
                {
                    LH = 1;
                }
                else// if (Convert.ToInt32(num[6]) < Convert.ToInt32(num[1]))
                {
                    LH = 2;
                }
            }
            if (i == 3)
            {
                if (Convert.ToInt32(num[2]) > Convert.ToInt32(num[5]))
                {
                    LH = 1;
                }
                else//  if (Convert.ToInt32(num[5]) < Convert.ToInt32(num[2]))
                {
                    LH = 2;
                }
            }
            if (i == 4)
            {
                if (Convert.ToInt32(num[3]) > Convert.ToInt32(num[4]))
                {
                    LH = 1;
                }
                else//  if (Convert.ToInt32(num[4]) < Convert.ToInt32(num[3]))
                {
                    LH = 2;
                }
            }

        }
        catch
        {
            return 0;
        }

        return LH;
    }
    #endregion

    //返赢返负出错处理扣回代码
    private void backerror()
    {
        //DataSet ds = new BCW.BLL.Goldlog().GetList("ID, Types, PUrl, UsId, UsName, AcGold, AfterGold, AcText, AddTime, BbTag, IsCheck", "(AcText LIKE '%快乐十分返负%') AND (CONVERT(varchar(20), AddTime, 120) LIKE '2016-11-17%')ORDER BY AddTime DESC");
        //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //{
        //    long Cents = Convert.ToInt64(ds.Tables[0].Rows[i]["AcGold"]);

        //        int usid = Convert.ToInt32(ds.Tables[0].Rows[i]["UsId"]);
        //        long cent = Cents;
        //        string strLog = string.Empty;

        //        {
        //            new BCW.BLL.User().UpdateiGold(usid, -cent, "" + GameName + "返负出错");
        //            //发内线
        //            strLog = "系统自动反负出错扣回";
        //        }

        //        new BCW.BLL.Guest().Add(0, usid, new BCW.BLL.User().GetUsName(usid), strLog);

        //}
        //Utils.Success("返负操作", "返负操作成功", Utils.getUrl("klsf.aspx"), "1");
    }
}
