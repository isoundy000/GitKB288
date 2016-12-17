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
/// 蒙宗将 20161027 新开发
/// 蒙宗将 20161029 优化管理
/// 蒙宗将 20161031 返奖修复
/// 蒙宗将 20161101 返奖优化任选 顺子开奖修复 总和大小单双 
/// 蒙宗将 20161104 增加退注原因
/// 蒙宗将 20161111 修复任选2、3普通开奖
/// 蒙宗将 20161112 手动开奖判定时间
///        20161116 牛牛算法完善
///        20161118 优化返赢返负
///        20161121 周榜修复、盈利分析排除系统号、有牛无牛赔率改为固定 22
///        20161125 增加ID限额、增加开奖时间 26 上月盈利分析
/// </summary>

public partial class Manage_game_ssc : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/ssc.xml";
    protected string GameName = ub.GetSub("SSCName", "/Controls/ssc.xml");
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
            case "opentwo":
                OpentwoPage();//重开奖
                break;
            case "set":
                SetPage();//游戏配置
                break;
            case "setceshi":
                SetStatueCeshi();//测试配置
                break;
            case "rewards":
                RewardPage();//前十奖励
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
            case "case":
                CasePage();//未/已兑奖
                break;
            case "caseok":
                CaseokPage();//帮他兑奖
                break;
            case "tstate":
                TimeStatPage();//时间排行查询
                break;
            case "del":
                DelPage();//退还
                break;
            case "backmessage":
                BackMessagePage();//返赢返负记录
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        Master.Title = "" + GameName + "管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

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
        IList<BCW.ssc.Model.SSClist> listSSClist = new BCW.ssc.BLL.SSClist().GetSSClists(pageIndex, pageSize, strWhere, out recordCount);
        if (listSSClist.Count > 0)
        {
            int k = 1;
            foreach (BCW.ssc.Model.SSClist n in listSSClist)
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
                    builder.Append("第" + n.SSCId + "期开出:<a href=\"" + Utils.getUrl("ssc.aspx?act=view&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">未开</a>");
                    builder.Append("|<a href=\"" + Utils.getUrl("ssc.aspx?act=open&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">开奖</a>");

                }
                else
                {
                    builder.Append("第" + n.SSCId + "期开出:<a href=\"" + Utils.getUrl("ssc.aspx?act=view&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.Result + "</a>" + " （会员总投" + GetPayAllbySSCId(n.SSCId) + ub.Get("SiteBz") + "）");// 
                    try
                    {
                        string sta = new BCW.ssc.BLL.SSClist().GetStateTime(n.SSCId);
                        string[] sta1 = sta.Split('#');
                        string sta2 = sta1[sta1.Length - 1];
                        string[] sta3 = sta2.Split('|');
                        int x = Convert.ToInt32(sta3[0]);
                        string relt = sta3[2];
                        string time = sta3[1];
                        if (x == 1)
                        {
                            builder.Append(" <f style=\"color:red\">人工开奖</f>");
                        }
                        if (x >= 2)
                        {
                            builder.Append(" <f style=\"color:red\">第" + (x - 1) + "次重开奖</f>");
                        }
                    }
                    catch { }
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
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=set&amp;backurl=" + Utils.PostPage(1) + "") + "\">游戏配置</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=setceshi&amp;backurl=" + Utils.PostPage(1) + "") + "\">测试配置</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=top") + "\">游戏排行</a><br />");
        builder.Append("<a href =\"" + Utils.getUrl("ssc.aspx?act=chaxun") + "\">游戏查询</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=stat") + "\">盈利分析</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=back") + "\">返赢返负</a><br />");

        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=reset&amp;backurl=" + Utils.PostPage(1) + "") + "\">游戏重置</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

    }
    //游戏配置
    protected void SetPage()
    {
        Master.Title = "" + GameName + "设置";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx") + "\">" + GameName + "</a>&gt;配置");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("", ""));
        int pei = int.Parse(Utils.GetRequest("pei", "all", 1, @"^[0-1]$", "0"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^5[0-6]$|^[1-4]\d$|^[1-9]$", "1"));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            if (pei == 0)
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
                string GuestSet = Utils.GetRequest("GuestSet", "post", 2, @"^[0-1]$", "兑奖内线选择出错");
                string Rule = Utils.GetRequest("Rule", "post", 2, @"^[^\^]{1,10000}$", "游戏规则限10000字内");
                string GetRefresh = Utils.GetRequest("GetRefresh", "post", 2, @"^[0-9]\d*$", "刷新机抓取秒数填写出错");
                string RoBotRefresh = Utils.GetRequest("RoBotRefresh", "post", 2, @"^[0-9]\d*$", "刷新机机器人秒数填写出错");
                string SSCLianTing = Utils.GetRequest("SSCLianTing", "post", 2, @"^[0-9]\d*$", "连停期数填写出错");
                string lkstart = Utils.GetRequest("lkstart", "post", 2, @"^[0-9]\d*$", "连开期数填写出错");

                //继续验证时间
                if (OnTime != "")
                {
                    string[] temp = OnTime.Split("-".ToCharArray());
                    DateTime dt1 = Utils.ParseTime(temp[0]);
                    DateTime dt2 = Utils.ParseTime(temp[1]);
                }
                xml.dss["SSCName"] = Name;
                xml.dss["SSCNotes"] = Notes;
                xml.dss["SSCLogo"] = Logo;
                xml.dss["SSCStatus"] = Status;
                xml.dss["SSCSec"] = Sec;
                xml.dss["SSCSmallPay"] = SmallPay;
                xml.dss["SSCBigPay"] = BigPay;
                xml.dss["SSCPrice"] = Price;
                xml.dss["SSCExpir"] = Expir;
                xml.dss["SSCOnTime"] = OnTime;
                xml.dss["SSCFoot"] = Foot;
                xml.dss["SSCGuestSet"] = GuestSet;
                xml.dss["SSCRule"] = Rule;
                xml.dss["SSCGetRefresh"] = GetRefresh;
                xml.dss["SSCRoBotRefresh"] = RoBotRefresh;
                xml.dss["SSCLianTing"] = SSCLianTing;
                xml.dss["lkstart"] = lkstart;
            }
            if (pei == 1)
            {
                string ptypey = string.Empty;
                string payname = Utils.GetRequest("payname", "all", 2, @"^[^\^]{1,1000}$", "玩法名称xml属性出错");
                string odds = Utils.GetRequest("odds", "all", 2, @"^[^\^]{1,1000}$", "赔率xml属性出错");
                string oddsc = Utils.GetRequest("oddsc", "all", 2, @"^[^\^]{1,1000}$", "投注额度xml属性出错");
                string rule = Utils.GetRequest("rule", "all", 2, @"^[^\^]{1,1000}$", "规则提示xml属性出错");

                if (ptype == 17 || ptype == 28 || ptype == 2 || ptype == 3 || ptype == 5 || ptype == 6 || ptype == 8 || ptype == 9 || ptype == 11 || ptype == 12 || ptype == 14 || ptype == 15 || ptype == 25 || ptype == 26 || ptype == 36 || ptype == 37 || ptype == 43 || ptype == 44 || ptype == 50 || ptype == 51)// || ptype == 23
                {
                    string oddsg = Utils.GetRequest("oddsg", "all", 2, @"^[^\^]{1,1000}$", "固定赔率提示xml属性出错");
                    string oddss = Utils.GetRequest("oddss", "all", 2, @"^[^\^]{1,1000}$", "最小赔率提示xml属性出错");
                    string oddsm = Utils.GetRequest("oddsm", "all", 2, @"^[^\^]{1,1000}$", "最大赔率提示xml属性出错");
                    string oddsf = Utils.GetRequest("oddsf", "all", 2, @"^[^\^]{1,1000}$", "浮动赔率提示xml属性出错");
                    string idxet = Utils.GetRequest("idxet", "all", 1, @"^[0-9]\d*$", "0");
                    ptypey = payname + "#" + odds + "#" + oddsc + "#" + rule + "#" + oddsg + "#" + oddsf + "#" + oddss + "#" + oddsm + "#" + idxet; //Utils.Error(""+ptypey,"");
                }
                else
                {
                    string idxe = Utils.GetRequest("idxe", "all", 1, @"^[0-9]\d*$", "0");
                    if (Convert.ToInt64(idxe) > Convert.ToInt64(xml.dss["SSCPrice"]) && Convert.ToInt64(xml.dss["SSCPrice"]) != 0)
                    {
                        Utils.Error("每期每种玩法每用户ID限额不能大于每期每ID限额" + Convert.ToInt64(xml.dss["SSCPrice"]) + "", "");
                    }
                    ptypey = payname + "#" + odds + "#" + oddsc + "#" + rule + "#" + idxe;
                }

                xml.dss["ptype" + ptype + ""] = ptypey;
            }

            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            if (pei == 0)
                Utils.Success("" + GameName + "设置", "设置成功，正在返回..", Utils.getUrl("ssc.aspx?act=set&amp;pei=" + pei + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
            else
                Utils.Success("" + GameName + "设置", "设置成功，正在返回..", Utils.getUrl("ssc.aspx?act=set&amp;pei=" + pei + "&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            if (pei == 0)
            {
                builder.Append("" + GameName + "设置 | ");
                builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=set&amp;pei=1") + "\">玩法属性</a>");
            }
            if (pei == 1)
            {
                builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=set&amp;pei=0") + "\">" + GameName + "设置</a>");
                builder.Append(" | 玩法属性");
            }
            builder.Append(Out.Tab("</div>", ""));
            if (pei == 0)
            {
                string strText = "游戏名称:/,游戏口号(可留空):/,游戏Logo(可留空):/,游戏状态:/,离截止时间N秒前不能下注/,最小下注" + ub.Get("SiteBz") + ":/,最大下注" + ub.Get("SiteBz") + ":/,每期每ID限购多少" + ub.Get("SiteBz") + "(填0则不限制):/,下注防刷(秒):/,游戏开放时间(可留空):/,游戏规则:/,底部Ubb:/,兑奖内线:/,刷新机开奖抓取时间（秒）:/,刷新机机器人刷新时间（秒）:/,最大连停期数:/,X期连开才开起浮动:/,";
                string strName = "Name,Notes,Logo,Status,Sec,SmallPay,BigPay,Price,Expir,OnTime,Rule,Foot,GuestSet,GetRefresh,RoBotRefresh,SSCLianTing,lkstart,backurl";
                string strType = "text,text,text,select,num,num,num,num,num,text,textarea,textarea,select,num,num,num,num,hidden";
                string strValu = "" + xml.dss["SSCName"] + "'" + xml.dss["SSCNotes"] + "'" + xml.dss["SSCLogo"] + "'" + xml.dss["SSCStatus"] + "'" + xml.dss["SSCSec"] + "'" + xml.dss["SSCSmallPay"] + "'" + xml.dss["SSCBigPay"] + "'" + xml.dss["SSCPrice"] + "'" + xml.dss["SSCExpir"] + "'" + xml.dss["SSCOnTime"] + "'" + xml.dss["SSCRule"] + "'" + xml.dss["SSCFoot"] + "'" + xml.dss["SSCGuestSet"] + "'" + xml.dss["SSCGetRefresh"] + "'" + xml.dss["SSCRoBotRefresh"] + "'" + xml.dss["SSCLianTing"] + "'" + xml.dss["lkstart"] + "'" + Utils.getPage(0) + "";
                string strEmpt = "false,true,true,0|正常|1|维护|2|内测,false,false,false,false,false,true,true,true,0|开启|1|关闭,false,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,ssc.aspx?act=set,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", ""));
                //builder.Append("温馨提示:<br />");
                builder.Append(Out.Tab("</div>", ""));
            }
            if (pei == 1)
            {
                string ptypey = string.Empty;
                string payname = string.Empty;
                string odds = string.Empty;
                string oddsc = string.Empty;
                string rule = string.Empty;
                string payname1 = string.Empty;
                string odds1 = string.Empty;
                string oddsc1 = string.Empty;
                string rule1 = string.Empty;
                string oddsg = string.Empty;
                string oddsf = string.Empty;
                string oddss = string.Empty;
                string oddsm = string.Empty;
                string idxeyy = string.Empty;
                string idxeyyt = string.Empty;
                for (int i = 1; i < 57; i++)
                {
                    ptypey = ub.GetSub("ptype" + i + "", xmlPath);
                    string[] ptypef = ptypey.Split('#');
                    payname1 += "#" + ptypef[0];
                    odds1 += "#" + ptypef[1];
                    oddsc1 += "#" + ptypef[2];
                    rule1 += "#" + ptypef[3];
                    try { idxeyy += "#" + ptypef[4]; }
                    catch { idxeyy += "#" + " "; }
                    if (i == 17 || i == 28 || i == 2 || i == 3 || i == 5 || i == 6 || i == 8 || i == 9 || i == 11 || i == 12 || i == 14 || i == 15 || i == 25 || i == 26 || i == 36 || i == 37 || i == 43 || i == 44 || i == 50 || i == 51)// || i == 23 
                    {
                        oddsg += "#" + ptypef[4];
                        oddsf += "#" + ptypef[5];
                        oddss += "#" + ptypef[6];
                        oddsm += "#" + ptypef[7];
                        try { idxeyyt += "#" + ptypef[8]; }
                        catch { idxeyyt += "#" + " "; }
                    }
                }
                string[] payname2 = payname1.Split('#');
                string[] odds2 = odds1.Split('#');
                string[] oddsc2 = oddsc1.Split('#');
                string[] rule2 = rule1.Split('#');

                builder.Append(Out.Tab("<div>", "<br />----------<br />"));
                for (int i = 1; i < 57; i++)
                {
                    payname = payname2[i];
                    string name = string.Empty;
                    if (ptype == i) { name = "<b style=\"color:red\">" + payname + "</b>"; } else { name = payname; }
                    if (i == 8 || i == 17 || i == 28 || i == 35 || i == 42 || i == 49)
                        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=set&amp;pei=" + pei + "&amp;ptype=" + i + "") + "\">" + name + "</a><br />----------<br />");
                    else
                    {
                        if (i != 56)
                            builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=set&amp;pei=" + pei + "&amp;ptype=" + i + "") + "\">" + name + "</a> | ");
                        else
                            builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=set&amp;pei=" + pei + "&amp;ptype=" + i + "") + "\">" + name + "</a>");
                    }
                }
                string strText = string.Empty;
                if (ptype == 17)
                {
                    strText = "玩法名称:/,赔率:（龙|虎|和）/,投注额度:/,规则提示:/,固定赔率（赔率浮动只有龙与虎）:/,浮动赔率:/,下限赔率:/,上限赔率:/,每期每用户ID限额（填0则不限制）:/,,,";
                }
                else if (ptype == 23)
                {
                    strText = "玩法名称:/,赔率:（有牛|无牛）/,投注额度:（有牛|无牛）/,规则提示:/,每期每用户ID限额（填0则不限制）:/,,,";//固定赔率:/浮动赔率:/下限赔率:/上限赔率:/
                }
                else if (ptype == 27)
                {
                    strText = "玩法名称:/,赔率:（一门|二门|三门|四门|五门）/,投注额度:（一门|二门|三门|四门|五门）/,规则提示:/,每期每用户ID限额（填0则不限制）:/,,,";
                }
                else if (ptype == 28)
                {
                    strText = "玩法名称:/,赔率:（大单、小双|大双、小单）/,浮动额度:/,规则提示:/,固定赔率:/,浮动赔率:/,下限赔率:/,上限赔率:/,每期每用户ID限额（填0则不限制）:/,,,";
                }
                else
                {
                    if (ptype == 2 || ptype == 3 || ptype == 5 || ptype == 6 || ptype == 8 || ptype == 9 || ptype == 11 || ptype == 12 || ptype == 14 || ptype == 15 || ptype == 25 || ptype == 26 || ptype == 36 || ptype == 37 || ptype == 43 || ptype == 44 || ptype == 50 || ptype == 51)
                    {
                        if (ptype == 2 || ptype == 5 || ptype == 8 || ptype == 11 || ptype == 14 || ptype == 25 || ptype == 36 || ptype == 43 || ptype == 50)
                        {
                            strText = "玩法名称:/,赔率（大|小 ）:/,浮动额度:/,规则提示:/,固定赔率:/,浮动赔率:/,下限赔率:/,上限赔率:/,每期每用户ID限额（填0则不限制）:/,,,";
                        }
                        else
                        {
                            strText = "玩法名称:/,赔率（单|双 ）:/,浮动额度:/,规则提示:/,固定赔率:/,浮动赔率:/,下限赔率:/,上限赔率:/,每期每用户ID限额（填0则不限制）:/,,,";
                        }
                    }
                    else
                    {
                        strText = "玩法名称:/,赔率:/,投注额度:（玩法的总投注上限）/,规则提示:/,每期每用户ID限额（填0则不限制）:/,,,";
                    }
                }
                string strName = string.Empty;
                string strType = string.Empty;
                string strValu = string.Empty;
                string strEmpt = string.Empty;
                if (ptype == 17 || ptype == 28 || ptype == 2 || ptype == 3 || ptype == 5 || ptype == 6 || ptype == 8 || ptype == 9 || ptype == 11 || ptype == 12 || ptype == 14 || ptype == 15 || ptype == 25 || ptype == 26 || ptype == 36 || ptype == 37 || ptype == 43 || ptype == 44 || ptype == 50 || ptype == 51)// || ptype == 23
                {
                    string[] oddsg2 = oddsg.Split('#'); string oddsg1 = string.Empty;
                    string[] oddsf2 = oddsf.Split('#'); string oddsf1 = string.Empty;
                    string[] oddss2 = oddss.Split('#'); string oddss1 = string.Empty;
                    string[] oddsm2 = oddsm.Split('#'); string oddsm1 = string.Empty;
                    string[] idxeyyyt = idxeyyt.Split('#'); string idxeyt = string.Empty;

                    if (ptype == 2) { oddsg1 = oddsg2[1]; oddsf1 = oddsf2[1]; oddss1 = oddss2[1]; oddsm1 = oddsm2[1]; idxeyt = idxeyyyt[1]; }

                    if (ptype == 3) { oddsg1 = oddsg2[2]; oddsf1 = oddsf2[2]; oddss1 = oddss2[2]; oddsm1 = oddsm2[2]; idxeyt = idxeyyyt[2]; }
                    if (ptype == 5) { oddsg1 = oddsg2[3]; oddsf1 = oddsf2[3]; oddss1 = oddss2[3]; oddsm1 = oddsm2[3]; idxeyt = idxeyyyt[3]; }
                    if (ptype == 6) { oddsg1 = oddsg2[4]; oddsf1 = oddsf2[4]; oddss1 = oddss2[4]; oddsm1 = oddsm2[4]; idxeyt = idxeyyyt[4]; }
                    if (ptype == 8) { oddsg1 = oddsg2[5]; oddsf1 = oddsf2[5]; oddss1 = oddss2[5]; oddsm1 = oddsm2[5]; idxeyt = idxeyyyt[5]; }
                    if (ptype == 9) { oddsg1 = oddsg2[6]; oddsf1 = oddsf2[6]; oddss1 = oddss2[6]; oddsm1 = oddsm2[6]; idxeyt = idxeyyyt[6]; }
                    if (ptype == 11) { oddsg1 = oddsg2[7]; oddsf1 = oddsf2[7]; oddss1 = oddss2[7]; oddsm1 = oddsm2[7]; idxeyt = idxeyyyt[7]; }
                    if (ptype == 12) { oddsg1 = oddsg2[8]; oddsf1 = oddsf2[8]; oddss1 = oddss2[8]; oddsm1 = oddsm2[8]; idxeyt = idxeyyyt[8]; }
                    if (ptype == 14) { oddsg1 = oddsg2[9]; oddsf1 = oddsf2[9]; oddss1 = oddss2[9]; oddsm1 = oddsm2[9]; idxeyt = idxeyyyt[9]; }
                    if (ptype == 15) { oddsg1 = oddsg2[10]; oddsf1 = oddsf2[10]; oddss1 = oddss2[10]; oddsm1 = oddsm2[10]; idxeyt = idxeyyyt[10]; }
                    if (ptype == 17) { oddsg1 = oddsg2[11]; oddsf1 = oddsf2[11]; oddss1 = oddss2[11]; oddsm1 = oddsm2[11]; idxeyt = idxeyyyt[11]; }
                    // if (ptype == 23) { oddsg1 = oddsg2[12]; oddsf1 = oddsf2[12]; oddss1 = oddss2[12]; oddsm1 = oddsm2[12]; }
                    if (ptype == 25) { oddsg1 = oddsg2[12]; oddsf1 = oddsf2[12]; oddss1 = oddss2[12]; oddsm1 = oddsm2[12]; idxeyt = idxeyyyt[12]; }
                    if (ptype == 26) { oddsg1 = oddsg2[13]; oddsf1 = oddsf2[13]; oddss1 = oddss2[13]; oddsm1 = oddsm2[13]; idxeyt = idxeyyyt[13]; }
                    if (ptype == 28) { oddsg1 = oddsg2[14]; oddsf1 = oddsf2[14]; oddss1 = oddss2[14]; oddsm1 = oddsm2[14]; idxeyt = idxeyyyt[14]; }
                    if (ptype == 36) { oddsg1 = oddsg2[15]; oddsf1 = oddsf2[15]; oddss1 = oddss2[15]; oddsm1 = oddsm2[15]; idxeyt = idxeyyyt[15]; }
                    if (ptype == 37) { oddsg1 = oddsg2[16]; oddsf1 = oddsf2[16]; oddss1 = oddss2[16]; oddsm1 = oddsm2[16]; idxeyt = idxeyyyt[16]; }
                    if (ptype == 43) { oddsg1 = oddsg2[17]; oddsf1 = oddsf2[17]; oddss1 = oddss2[17]; oddsm1 = oddsm2[17]; idxeyt = idxeyyyt[17]; }
                    if (ptype == 44) { oddsg1 = oddsg2[18]; oddsf1 = oddsf2[18]; oddss1 = oddss2[18]; oddsm1 = oddsm2[18]; idxeyt = idxeyyyt[18]; }
                    if (ptype == 50) { oddsg1 = oddsg2[19]; oddsf1 = oddsf2[19]; oddss1 = oddss2[19]; oddsm1 = oddsm2[19]; idxeyt = idxeyyyt[19]; }
                    if (ptype == 51) { oddsg1 = oddsg2[20]; oddsf1 = oddsf2[20]; oddss1 = oddss2[20]; oddsm1 = oddsm2[20]; idxeyt = idxeyyyt[20]; }

                    strName = "payname,odds,oddsc,rule,oddsg,oddsf,oddss,oddsm,idxet,pei,ptype,backurl";
                    strType = "text,text,text,textarea,text,text,text,text,num,hidden,hidden,hidden";
                    strValu = "" + payname2[ptype] + "'" + odds2[ptype] + "'" + oddsc2[ptype] + "'" + rule2[ptype] + "'" + oddsg1 + "'" + oddsf1 + "'" + oddss1 + "'" + oddsm1 + "'" + idxeyt + "'1'" + ptype + "'" + Utils.getPage(0) + "";
                    strEmpt = "false,false,false,false,false,false,false,false,false,false,false,false";
                }
                else
                {
                    string[] idxey = idxeyy.Split('#');
                    strName = "payname,odds,oddsc,rule,idxe,pei,ptype,backurl";
                    strType = "text,text,text,textarea,num,hidden,hidden,hidden";
                    strValu = "" + payname2[ptype] + "'" + odds2[ptype] + "'" + oddsc2[ptype] + "'" + rule2[ptype] + "'" + idxey[ptype] + "'1'" + ptype + "'" + Utils.getPage(0) + "";
                    strEmpt = "false,false,false,false,false,false,false,false";
                }

                string strIdea = "/";
                string strOthe = "确定修改|reset,ssc.aspx?act=set,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            if (pei == 1)
            {
                builder.Append("温馨提示：<b style=\"color:red\">所有属性不能填写#</b>。玩法赔率有多个赔率时，请查看仔细赔率提示的位号，赔率之间用“|”分隔<br />");
            }
            builder.Append("<a href=\"" + Utils.getPage("ssc.aspx") + "\">返回上级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
    //游戏内测管理
    private void SetStatueCeshi()
    {
        Master.Title = "" + GameName + "设置测试状态";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx") + "\">" + GameName + "</a>&gt;测试配置");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b>" + GameName + "内测管理：</b>");
        builder.Append(Out.Tab("</div>", "<br />"));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/ssc.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string SSCStatus = Utils.GetRequest("SSCStatus", "post", 2, @"^[0-9]\d*$", "测试权限管理输入出错");
            string SSCCeshihao = Utils.GetRequest("SSCCeshihao", "all", 2, @"^[^\^]{1,2000}$", "请输入测试号");
            xml.dss["SSCStatus"] = SSCStatus;
            xml.dss["SSCCeshihao"] = SSCCeshihao.Replace("\r\n", "").Replace(" ", "");
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("" + GameName + "设置", "内测设置成功，正在返回..", Utils.getUrl("ssc.aspx?act=setceshi"), "2");
        }
        else
        {
            string strText = "测试权限管理:/,添加测试号(多账号用#号分隔):/,";
            string strName = "SSCStatus,SSCCeshihao,backurl";
            string strType = "select,textarea,hidden";
            string strValu = xml.dss["SSCStatus"] + "'" + xml.dss["SSCCeshihao"] + "'" + Utils.getPage(0) + "";
            string strEmpt = "0|开放|1|维护|2|内测,true";
            string strIdea = "/";
            string strOthe = "确定修改,ssc.aspx?act=setceshi,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            string SSCCeshihao = Convert.ToString(ub.GetSub("SSCCeshihao", xmlPath));
            string[] name = SSCCeshihao.Split('#');
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
                string SSCRoBotID = Utils.GetRequest("SSCRoBotID", "post", 1, "", xml.dss["SSCRoBotID"].ToString());
                string SSCIsBot = Utils.GetRequest("SSCIsBot", "post", 1, @"^[0-1]$", xml.dss["SSCIsBot"].ToString());
                string SSCRoBotCent = Utils.GetRequest("SSCRoBotCent", "post", 1, "", xml.dss["SSCRoBotCent"].ToString());
                string SSCRoBotbuyCount = Utils.GetRequest("SSCRoBotbuyCount", "post", 1, @"^[0-9]\d*$", xml.dss["SSCRoBotbuyCount"].ToString());
                xml.dss["SSCRoBotID"] = SSCRoBotID.Replace("\r\n", "").Replace(" ", "");
                xml.dss["SSCIsBot"] = SSCIsBot;
                xml.dss["SSCRoBotCent"] = SSCRoBotCent.Replace("\r\n", "").Replace(" ", "");
                xml.dss["SSCRoBotbuyCount"] = SSCRoBotbuyCount;
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                Utils.Success("" + GameName + "设置", "机器人管理成功，正在返回..", Utils.getUrl("ssc.aspx?act=setceshi"), "2");
            }
            else
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<b>机器人管理：</b>");
                builder.Append(Out.Tab("</div>", "<br />"));

                string strText1 = "机器人ID(多个机器人用#号分隔):/,机器人状态:/,机器人投注金额设置:/,机器人每期购买次数:/";
                string strName1 = "SSCRoBotID,SSCIsBot,SSCRoBotCent,SSCRoBotbuyCount";
                string strType1 = "textarea,select,text,text";
                string strValu1 = xml.dss["SSCRoBotID"].ToString() + "'" + xml.dss["SSCIsBot"].ToString() + "'" + xml.dss["SSCRoBotCent"].ToString() + "'" + xml.dss["SSCRoBotbuyCount"].ToString();
                string strEmpt1 = "true,0|关闭|1|开启,true,false";
                string strIdea1 = "/";
                string strOthe1 = "确定设置,ssc.aspx?act=setceshi,post,1,red";
                builder.Append(Out.wapform(strText1, strName1, strType1, strValu1, strEmpt1, strIdea1, strOthe1));
                builder.Append(Out.Tab("<div>", "<br />"));

                string SSCRoBotIDS = Convert.ToString(ub.GetSub("SSCRoBotID", xmlPath));
                string[] name1 = SSCRoBotIDS.Split('#');
                string name2 = string.Empty;
                for (int n = 0; n < name1.Length; n++)
                {
                    if ((n + 1) % 5 == 0)
                        name2 = name2 + name1[n] + "," + "<br />";
                    else
                        name2 = name2 + name1[n] + ",";
                }
                builder.Append("当前机器人ID为：<br /><b style=\"color:red\">" + name2 + "</b><br />");
                if (xml.dss["SSCIsBot"].ToString() == "0")
                {
                    builder.Append("机器人状态：<b style=\"color:red\">关闭</b><br />");
                }
                else
                {
                    builder.Append("当前机器人状态：<b style=\"color:red\">开启</b><br />");
                }
                builder.Append("当前机器人单注投注金额(随机投注)：<b style=\"color:red\">" + xml.dss["SSCRoBotCent"].ToString() + "</b><br />");
                builder.Append("当前机器人每期限购买彩票次数：<b style=\"color:red\">" + xml.dss["SSCRoBotbuyCount"].ToString() + "</b><br />");
                builder.Append("<b>温馨提示:请注意内测设置与机器人设置分开设置</b>");
                builder.Append(Out.Tab("</div>", "<br />"));
            }

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("" + "-----------" + "<br />");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx") + "\">返回" + GameName + "管理</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
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
            BCW.Data.SqlHelper.ExecuteSql(" truncate table tb_SSClist ");
            BCW.Data.SqlHelper.ExecuteSql(" truncate table tb_SSCpay ");

            Utils.Success("重置游戏", "重置游戏成功..", Utils.getUrl("ssc.aspx"), "2");
        }
        if (info == "ok1")
        {
            Master.Title = "" + GameName + "重置游戏";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
            builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx") + "\">" + GameName + "</a>");
            builder.Append("&gt;重置游戏");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("确定重置" + GameName + "游戏吗，重置后，将重新从新的一期开始，所有期数记录将会和下注记录全被删除");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=reset&amp;info=ok1") + "\">重置游戏</a>&gt;");
            builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=reset&amp;info=ok") + "\">确定重置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            Master.Title = "" + GameName + "重置游戏";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
            builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx") + "\">" + GameName + "</a>");
            builder.Append("&gt;重置游戏");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("确定重置" + GameName + "游戏吗，重置后，将重新从第一期开始，所有记录将会期数和下注记录全被删除");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=reset&amp;info=ok1") + "\">重置游戏</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx") + "\">再看看吧..</a>");
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
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx") + "\">" + GameName + "</a>&gt;排行榜");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("=排行榜=");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;

        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-2]\d*$", "0"));
        builder.Append(Out.Tab("<div>", ""));
        if (ptype == 0)
        {
            strWhere = "Prices>0 and State>0 ";
            builder.Append("总榜 | <a href=\"" + Utils.getUrl("ssc.aspx?act=top&amp;ptype=1") + "\">周榜</a> | <a href=\"" + Utils.getUrl("ssc.aspx?act=top&amp;ptype=2") + "\">日榜</a>");
        }
        else if (ptype == 1)
        {
            strWhere = " datediff(WEEK,AddTime,getdate())=0 and Prices>0 and State>0 ";
            builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=top&amp;ptype=0") + "\">总榜</a> | 周榜 | <a href=\"" + Utils.getUrl("ssc.aspx?act=top&amp;ptype=2") + "\">日榜</a>");
        }
        else
        {
            strWhere = " datediff(DAY,AddTime,getdate())=0 and Prices>0 and State>0 ";
            builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=top&amp;ptype=0") + "\">总榜</a> | <a href=\"" + Utils.getUrl("ssc.aspx?act=top&amp;ptype=1") + "\">周榜</a> | 日榜");
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        // 开始读取列表
        IList<BCW.ssc.Model.SSCpay> listSSCpay = new BCW.ssc.BLL.SSCpay().GetSSCpaysTop(pageIndex, pageSize, strWhere, out recordCount);
        if (listSSCpay.Count > 0)
        {
            int k = 1;
            foreach (BCW.ssc.Model.SSCpay n in listSSCpay)
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
                builder.Append("[第" + ((pageIndex - 1) * pageSize + k) + "名]<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(n.UsID) + "(" + n.UsID + ")</a>赢" + n.WinCent + "" + ub.Get("SiteBz") + "");
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
        string strOthe = "时间查询排行,ssc.aspx,post,1,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("温馨提示：查询排行可对排行前十用户手动发放奖励<br />");
        builder.Append("<a href=\"" + Utils.getPage("ssc.aspx") + "\">返回上一级</a><br />");
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
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx") + "\">" + GameName + "</a>");
        builder.Append("&gt;排行查询结果");
        builder.Append(Out.Tab("</div>", "<br />"));

        DateTime sTime = Utils.ParseTime(Utils.GetRequest("sTime", "all", 2, DT.RegexTime, "开始时间填写无效"));
        DateTime oTime = Utils.ParseTime(Utils.GetRequest("oTime", "all", 2, DT.RegexTime, "结束时间填写无效"));
        string state = Utils.GetRequest("State", "all", 2, @"^[^\^]{1,5}$", "状态填写无效");

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("" + GameName + sTime + "-" + oTime + "排行榜");
        builder.Append(Out.Tab("</div>", "<br />"));

        string strWheres = string.Empty;
        strWheres += "AddTime > '" + sTime + "' and AddTime <'" + oTime + "' and State>0 group by UsID Order by sum(WinCent-Prices) desc";
        DataSet ds = new BCW.ssc.BLL.SSCpay().GetList("UsID,sum(WinCent-Prices) as WinCent", strWheres);

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


            Utils.Success("排行榜奖励", "排行榜奖励发放成功", Utils.getUrl("ssc.aspx"), "1");
        }
        #endregion

        #region 填写奖励
        else
        {
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string strWhere = "AddTime > '" + sTime + "' and AddTime <'" + oTime + "' and State>0 ";
            string[] pageValUrl = { "act", "State", "sTime", "oTime", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            // 开始读取列表
            IList<BCW.ssc.Model.SSCpay> listSSCpay = new BCW.ssc.BLL.SSCpay().GetSSCpaysTop(pageIndex, pageSize, strWhere, out recordCount);
            if (listSSCpay.Count > 0)
            {
                int k = 1;
                foreach (BCW.ssc.Model.SSCpay n in listSSCpay)
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

                    builder.Append("[第" + ((pageIndex - 1) * pageSize + k) + "名]<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(n.UsID) + "(" + n.UsID + ")" + "</a>赢" + n.WinCent + "" + ub.Get("SiteBz") + "");

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
                string strOthe = "发放奖励,ssc.aspx,post,1,red";

                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append("温馨提示：如果要奖励前十必须全部奖励，奖励值为" + ub.Get("SiteBz") + "<br />");
            builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=top") + "\">返回上一级</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        #endregion
    }
    //反赢反负
    private void BackPage()
    {
        Master.Title = "" + GameName + "返赢返负";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx") + "\">" + GameName + "</a>");
        builder.Append("&gt;返赢返负");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
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
        string strOthe = "马上返赢,ssc.aspx,post,1,red";

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
        strOthe = "马上返负,ssc.aspx,post,1,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("ssc.aspx?act=backmessage") + "\">返赢反负记录</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("ssc.aspx") + "\">返回上一级</a>");
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

        if (act == "backsave1")
        {
            DataSet ds = new BCW.ssc.BLL.SSCpay().GetList("UsID,sum(WinCent-Prices) as WinCents", "AddTime>='" + sTime + "'and AddTime<='" + oTime + "' group by UsID");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                long Cents = Convert.ToInt64(ds.Tables[0].Rows[i]["WinCents"]);
                if (Cents > 0 && Cents >= iPrice)
                {
                    int usid = Convert.ToInt32(ds.Tables[0].Rows[i]["UsID"]);
                    long cent = Convert.ToInt64(Cents * (iTar * 0.001));
                    string strLog = string.Empty;

                    new BCW.BLL.User().UpdateiGold(usid, cent, "" + GameName + "返赢");
                    //发内线
                    strLog = "根据你近期" + GameName + "排行榜上的盈利情况，系统自动返还了" + cent + "" + ub.Get("SiteBz") + "[url=/bbs/game/ssc.aspx]进入" + GameName + "[/url]";

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
            new BCW.BLL.User().UpdateiGold(108, new BCW.BLL.User().GetUsName(108), 0, "" + GameName + sTime + "-" + oTime + "返赢|千分比" + iTar + "|至少赢" + iPrice + "|共计人次" + sumi + "|共返" + sum + "|此记录为返赢记录,不操作108的币数");

            Utils.Success("返赢操作", "返赢操作成功", Utils.getUrl("ssc.aspx"), "1");
        }
        else
        {
            DataSet ds = new BCW.ssc.BLL.SSCpay().GetList("UsID,sum(WinCent-Prices) as WinCents", "AddTime>='" + sTime + "'and AddTime<='" + oTime + "' group by UsID");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                long Cents = Convert.ToInt64(ds.Tables[0].Rows[i]["WinCents"]);
                if (Cents < 0 && Cents < (-iPrice))
                {
                    int usid = Convert.ToInt32(ds.Tables[0].Rows[i]["UsID"]);
                    long cent = Convert.ToInt64((-Cents) * (iTar * 0.001));
                    string strLog = string.Empty;

                    new BCW.BLL.User().UpdateiGold(usid, cent, "" + GameName + "返负");
                    //发内线
                    strLog = "根据你近期" + GameName + "排行榜上的亏损情况，系统自动返还了" + cent + "" + ub.Get("SiteBz") + "[url=/bbs/game/ssc.aspx]进入" + GameName + "[/url]";

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
            new BCW.BLL.User().UpdateiGold(108, new BCW.BLL.User().GetUsName(108), 0, "" + GameName + sTime + "-" + oTime + "返负|千分比" + iTar + "|至少负" + iPrice + "|共计人次" + sumi + "|共返" + sum + "|此记录为返负记录,不操作108的币数");

            Utils.Success("返负操作", "返负操作成功", Utils.getUrl("ssc.aspx"), "1");
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

            builder.Append("返赢开始时间：" + sTime + "<br />");
            builder.Append("返赢截止时间：" + oTime + "<br />");
            builder.Append("返赢千分比：" + iTar + "<br />");
            builder.Append("至少赢：" + iPrice + "币返<br />");
            DataSet ds = new BCW.ssc.BLL.SSCpay().GetList("UsID,sum(WinCent-Prices) as WinCents", "AddTime>='" + sTime + "'and AddTime<='" + oTime + "' group by UsID");
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
            string strOthe = "马上返赢,ssc.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("", "<br />"));

        }
        else
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("返负开始时间：" + sTime + "<br />");
            builder.Append("返负截止时间：" + oTime + "<br />");
            builder.Append("返负千分比：" + iTar + "<br />");
            builder.Append("至少：" + iPrice + "币返<br />");
            DataSet ds = new BCW.ssc.BLL.SSCpay().GetList("UsID,sum(WinCent-Prices) as WinCents", "AddTime>='" + sTime + "'and AddTime<='" + oTime + "' group by UsID ");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                long sum = 0; int sumi = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    long Cents = Convert.ToInt64(ds.Tables[0].Rows[i]["WinCents"]);
                    long cent = 0;
                    if (Cents < 0 && Cents < (-iPrice))
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
            string strOthe = "马上返负,ssc.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("", "<br />"));

        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=back") + "\">返回上一级</a><br />");
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
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx") + "\">" + GameName + "</a>");
        builder.Append("&gt;<a href=\"" + Utils.getUrl("ssc.aspx?act=back") + "\">返赢返负</a>&gt;返赢返负记录");
        builder.Append(Out.Tab("</div>", "<br />"));

        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-1]$", "0"));
        builder.Append(Out.Tab("<div>", ""));
        if (ptype == 0)
            builder.Append("返赢记录 | <a href=\"" + Utils.getUrl("ssc.aspx?act=backmessage&amp;ptype=1") + "\">返负记录</a>");
        else
            builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=backmessage&amp;ptype=0") + "\">返赢记录</a> | 返负记录");
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
        builder.Append("<a href=\"" + Utils.getPage("ssc.aspx?act=back") + "\">返赢返负</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
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
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx") + "\">" + GameName + "</a>&gt;期号详情");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("期号查询|" + "<a href=\"" + Utils.getUrl("ssc.aspx?act=usidcx") + "\">会员查询</a>" + "|<a href=\"" + Utils.getUrl("ssc.aspx?act=jiangcx") + "\">往期分析</a>|<a href=\"" + Utils.getUrl("ssc.aspx?act=case") + "\">查兑奖</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("查看下注/中奖记录");
        builder.Append(Out.Tab("</div>", ""));

        BCW.ssc.Model.SSClist mm = new BCW.ssc.BLL.SSClist().GetSSClistLast2();
        //开始期数
        int number1 = int.Parse(Utils.GetRequest("number1", "all", 1, @"^[1-9]\d*$", mm.SSCId.ToString()));

        string strText = "查询期数:/,";
        string strName = "number1,act";
        string strType = "num,hidden";
        string strValu = "" + number1 + "'" + Utils.getPage(0) + "";
        string strEmpt = "true,true,false";
        string strIdea = "/";
        string strOthe = "确认搜索,ssc.aspx?act=chaxun&amp;ptype=" + ptype + ",post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        id = new BCW.ssc.BLL.SSClist().id(number1);
        if (id == 0)
        {
            try
            {
                id = mm.ID;
            }
            catch
            {
                Utils.Error("不存在的记录，输入的期号未开启或者不存在，请检查输入信息是否正确", "");
            }
        }

        BCW.ssc.Model.SSClist model = new BCW.ssc.BLL.SSClist().GetSSClist(id);

        if (model == null)
        {
            Utils.Error("不存在的记录，输入的期号未开启或者不存在，请检查输入信息是否正确", "");
        }

        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        builder.Append("查看:");
        if (ptype == 1)
            builder.Append("下注|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=chaxun&amp;ptype=1&amp;id=" + id + "&amp;number1=" + number1 + "&amp;backurl=" + Utils.getPage(0) + "") + "\">下注</a>|");

        if (ptype == 2)
            builder.Append("中奖");
        else
            builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=chaxun&amp;ptype=2&amp;id=" + id + "&amp;number1=" + number1 + "&amp;backurl=" + Utils.getPage(0) + "") + "\">中奖</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        if (ptype == 1)
            strWhere += "SSCId=" + model.SSCId + "";
        else
            strWhere += "SSCId=" + model.SSCId + " and WinCent>0 ";

        string[] pageValUrl = { "act", "id", "ptype", "number1", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        // 开始读取列表
        IList<BCW.ssc.Model.SSCpay> listSSCpay = new BCW.ssc.BLL.SSCpay().GetSSCpays(pageIndex, pageSize, strWhere, out recordCount);
        if (listSSCpay.Count > 0)
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("第" + model.SSCId + "期开出:<b>" + model.Result + " (" + Message(model.Result) + ")</b>");

            try
            {
                string sta = new BCW.ssc.BLL.SSClist().GetStateTime(model.SSCId);
                string[] sta1 = sta.Split('#');
                string sta2 = sta1[sta1.Length - 1];
                string[] sta3 = sta2.Split('|');
                int x = Convert.ToInt32(sta3[0]);
                string relt = sta3[2];
                string time = sta3[1];
                if (x == 0)
                {
                    builder.Append("<br />开奖时间:" + time + "");
                }
                if (x == 1)
                {
                    builder.Append("<br /><f style=\"color:red\">开奖类型:人工开奖 开奖时间:" + time + "</f>");
                }
                if (x >= 2)
                {
                    builder.Append("<br /><f style=\"color:red\">开奖类型:第" + (x - 1) + "次重开奖 开奖时间:" + time + "</f>");
                }
            }
            catch { }

            if (ptype == 1)
                builder.Append("<br />共" + recordCount + "注下注 会员下注（非机器系统号）总计" + GetPayAllbySSCId(model.SSCId) + ub.Get("SiteBz"));
            else
                builder.Append("<br />共" + recordCount + "注中奖 会员中奖（非机器系统号）总计" + GetWincentbySSCId(model.SSCId) + ub.Get("SiteBz"));

            builder.Append(Out.Tab("</div>", "<br />"));

            int k = 1;
            foreach (BCW.ssc.Model.SSCpay n in listSSCpay)
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

                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>");

                builder.Append("<b>[" + OutType(n.Types) + "]</b>位号:" + n.Notes + "/每注" + n.Price + "" + ub.Get("SiteBz") + "/共买" + n.iCount + "注/赔率" + n.Odds + "/标识ID:" + n.ID + "[" + DT.FormatDate(n.AddTime, 5) + "]");

                if (n.WinCent > 0)
                {
                    builder.Append("&nbsp; 中" + GetZj_zs(n.WinNotes) + "注/赢" + n.WinCent + "" + ub.Get("SiteBz") + "");
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
                builder.Append("第" + model.SSCId + "期开出:<b>" + model.Result + "</b>");

                try
                {
                    string sta = new BCW.ssc.BLL.SSClist().GetStateTime(model.SSCId);
                    string[] sta1 = sta.Split('#');
                    string sta2 = sta1[sta1.Length - 1];
                    string[] sta3 = sta2.Split('|');
                    int x = Convert.ToInt32(sta3[0]);
                    string relt = sta3[2];
                    string time = sta3[1];
                    if (x == 0)
                    {
                        builder.Append("<br />开奖时间:" + time + "");
                    }
                    if (x == 1)
                    {
                        builder.Append("<br /><f style=\"color:red\">开奖类型:人工开奖 开奖时间:" + time + "</f>");
                    }
                    if (x >= 2)
                    {
                        builder.Append("<br /><f style=\"color:red\">开奖类型:第" + (x - 1) + "次重开奖 开奖时间:" + time + "</f>");
                    }
                }
                catch { }

                builder.Append("<br />共0注投注");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Div("div", "没有相关记录..."));
            }
            else if (ptype == 2)
            {
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("第" + model.SSCId + "期开出:<b>" + model.Result + "</b>");

                try
                {
                    string sta = new BCW.ssc.BLL.SSClist().GetStateTime(model.SSCId);
                    string[] sta1 = sta.Split('#');
                    string sta2 = sta1[sta1.Length - 1];
                    string[] sta3 = sta2.Split('|');
                    int x = Convert.ToInt32(sta3[0]);
                    string relt = sta3[2];
                    string time = sta3[1];
                    if (x == 0)
                    {
                        builder.Append("<br />开奖时间:" + time + "");
                    }
                    if (x == 1)
                    {
                        builder.Append("<br /><f style=\"color:red\">开奖类型:人工开奖 开奖时间:" + time + "</f>");
                    }
                    if (x >= 2)
                    {
                        builder.Append("<br /><f style=\"color:red\">开奖类型:第" + (x - 1) + "次重开奖 开奖时间:" + time + "</f>");
                    }
                }
                catch { }

                builder.Append("<br />共0注中奖");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Div("div", "没有相关记录..."));
            }

        }

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx") + "\">返回上一级</a>");
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
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx") + "\">" + GameName + "</a>&gt;会员查询");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=chaxun") + "\">期号查询</a>" + "|会员查询" + "|<a href=\"" + Utils.getUrl("ssc.aspx?act=jiangcx") + "\">往期分析</a>|<a href=\"" + Utils.getUrl("ssc.aspx?act=case") + "\">查兑奖</a>");
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
        string strOthe = "确认搜索,ssc.aspx?act=usidcx&amp;ptype=" + ptype + ",post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));


        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        builder.Append("查看:");
        if (ptype == 1)
            builder.Append("下注|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=usidcx&amp;ptype=1&amp;UsID=" + UsID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">下注</a>|");

        if (ptype == 2)
            builder.Append("中奖");
        else
            builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=usidcx&amp;ptype=2&amp;UsID=" + UsID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">中奖</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;


        if (ptype == 1)
        {
            if (UsID == 0)
            {
                strWhere += "UsID>0";
            }
            else
            {
                strWhere += "UsID=" + UsID + "";
            }
        }
        else
        {
            if (UsID == 0)
            {
                strWhere += "UsID>0 and winCent>0 ";
            }
            else
            {
                strWhere += "UsID=" + UsID + " and winCent>0";
            }
        }

        string[] pageValUrl = { "act", "id", "ptype", "UsID", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.ssc.Model.SSCpay> listSSCpay = new BCW.ssc.BLL.SSCpay().GetSSCpays(pageIndex, pageSize, strWhere, out recordCount);
        if (listSSCpay.Count > 0)
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            if (UsID == 0)
            {
                builder.Append("当前无查询会员ID，请输入会员ID<br />");
            }
            else
            {
                builder.Append("会员ID" + UsID + "<br />");
            }
            int k = 1;
            foreach (BCW.ssc.Model.SSCpay n in listSSCpay)
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

                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>");


                builder.Append("<b>" + n.SSCId + "期[" + OutType(n.Types) + "]</b>位号:" + n.Notes + "/每注" + n.Price + "" + ub.Get("SiteBz") + "/共买" + n.iCount + "注/赔率" + n.Odds + "/标识ID:" + n.ID + "[" + DT.FormatDate(n.AddTime, 5) + "]");

                if (n.WinCent > 0)
                {

                    builder.Append("&nbsp; 中" + GetZj_zs(n.WinNotes) + "注/赢" + n.WinCent + "" + ub.Get("SiteBz") + "");

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
        builder.Append("<a href=\"" + Utils.getPage("ssc.aspx") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    //往期分析
    private void JiangcxPage()
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]\d*$", "1"));

        Master.Title = "奖池查询";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx") + "\">" + GameName + "</a>&gt;往期分析");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=chaxun") + "\">期号查询</a>" + "|<a href=\"" + Utils.getUrl("ssc.aspx?act=usidcx") + "\">会员查询</a>" + "|往期分析|<a href=\"" + Utils.getUrl("ssc.aspx?act=case") + "\">查兑奖</a>");
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
        string strOthe = "确定修改,ssc.aspx?act=jiangcx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        #endregion

        #region 从数据库中调出符合条件的数据并统计
        string strWhere = string.Empty;
        strWhere += "SSCId like \'" + searchDate.Remove(0, 2) + "%\' and State <> 0";
        DataSet ds = new BCW.ssc.BLL.SSClist().GetList("Result", strWhere);
        int[] num = new int[93];

        //数据总结
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            string result = ds.Tables[0].Rows[i]["Result"].ToString();
            string[] Result = ds.Tables[0].Rows[i]["Result"].ToString().Split(' ');
            int Re1 = Convert.ToInt32(Result[0]);//万位
            int Re2 = Convert.ToInt32(Result[1]);
            int Re3 = Convert.ToInt32(Result[2]);
            int Re4 = Convert.ToInt32(Result[3]);
            int Re5 = Convert.ToInt32(Result[4]);
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

            #region 大小单双 10-50
            if (Re1 > 4)
                num[11]++;//万位大
            else
                num[12]++;//万位小
            if (Re1 % 2 != 0)
                num[13]++;//万位单
            else
                num[14]++;//万位双

            if (Re2 > 4)
                num[15]++;//千位大
            else
                num[16]++;//位小
            if (Re2 % 2 != 0)
                num[17]++;//位单
            else
                num[18]++;//位双

            if (Re3 > 4)
                num[19]++;//百位大
            else
                num[20]++;//位小
            if (Re3 % 2 != 0)
                num[21]++;//位单
            else
                num[22]++;//位双

            if (Re4 > 4)
                num[23]++;//十位大
            else
                num[24]++;//位小
            if (Re4 % 2 != 0)
                num[25]++;//位单
            else
                num[26]++;//位双

            if (Re5 > 4)
                num[27]++;//个位大
            else
                num[28]++;//位小
            if (Re5 % 2 != 0)
                num[29]++;//位单
            else
                num[30]++;//位双

            int sum5 = 0;
            sum5 = Re1 + Re2 + Re3 + Re4 + Re5;
            if (sum5 > 22)
            {
                num[31]++;//总和位大
                if (sum5 % 2 != 0)//大单
                {
                    num[47]++;
                }
                else//大双
                {
                    num[48]++;
                }
            }
            else
            {
                num[32]++;//位小
                if (sum5 % 2 != 0)//小单
                {
                    num[49]++;
                }
                else//小双
                {
                    num[50]++;
                }
            }
            if (sum5 % 2 != 0)
            {
                num[33]++;//位单
            }
            else
            {
                num[34]++;//位双
            }

            int sumq3 = 0;
            sumq3 = Re1 + Re2 + Re3;
            if (sumq3 > 13)
                num[35]++;//前三位大
            else
                num[36]++;//位小
            if (sumq3 % 2 != 0)
                num[37]++;//位单
            else
                num[38]++;//位双

            int sumz3 = 0;
            sumz3 = Re4 + Re2 + Re3;
            if (sumz3 > 13)
                num[39]++;//中三位大
            else
                num[40]++;//位小
            if (sumz3 % 2 != 0)
                num[41]++;//位单
            else
                num[42]++;//位双

            int sumh3 = 0;
            sumh3 = Re4 + Re5 + Re3;
            if (sumh3 > 13)
                num[43]++;//后三位大
            else
                num[44]++;//位小
            if (sumh3 % 2 != 0)
                num[45]++;//位单
            else
                num[46]++;//位双
            #endregion

            #region 龙虎和
            if (Re1 > Re5)//龙
                num[51]++;
            if (Re1 < Re5)//虎
                num[52]++;
            if (Re1 == Re5)//和
                num[53]++;
            #endregion

            #region 牛牛，总和五门
            if (Niu(result) != "")
                num[54]++;//有牛
            else
                num[55]++;//无牛
            string a = Niu(result);
            int c = 0;
            if (a != "")
            {
                c = Convert.ToInt32(a.Substring(a.Length - 1, 1));
            }
            if (c == 0)
                num[56]++;//牛0
            if (c == 1)
                num[57]++;
            if (c == 2)
                num[58]++;
            if (c == 3)
                num[59]++;
            if (c == 4)
                num[60]++;
            if (c == 5)
                num[61]++;
            if (c == 6)
                num[62]++;
            if (c == 7)
                num[63]++;
            if (c == 8)
                num[64]++;
            if (c == 9)
                num[65]++;//牛9

            if (sum5 < 10)
                num[66]++;//一门
            if (sum5 > 9 && sum5 < 20)
                num[67]++;
            if (sum5 > 19 && sum5 < 30)
                num[68]++;
            if (sum5 > 29 && sum5 < 40)
                num[69]++;
            if (sum5 > 39 && sum5 < 46)
                num[70]++;
            #endregion

            #region 前三
            if (Qiansan(3, result) == 1)//豹子
                num[71]++;
            if (Qiansan(4, result) == 1)//顺子
                num[72]++;
            if (Qiansan(5, result) == 1)//对子
                num[73]++;
            if (Qiansan(6, result) == 1)//半顺
                num[74]++;
            if (Qiansan(7, result) != 1)//杂六
                num[75]++;
            #endregion

            #region 中三
            if (Zhongsan(3, result) == 1)//豹子
                num[76]++;
            if (Zhongsan(4, result) == 1)//顺子
                num[77]++;
            if (Zhongsan(5, result) == 1)//对子
                num[78]++;
            if (Zhongsan(6, result) == 1)//半顺
                num[79]++;
            if (Zhongsan(7, result) != 1)//杂六
                num[80]++;
            #endregion

            #region 后三
            if (Housan(3, result) == 1)//豹子
                num[81]++;
            if (Housan(4, result) == 1)//顺子
                num[82]++;
            if (Housan(5, result) == 1)//对子
                num[83]++;
            if (Housan(6, result) == 1)//半顺
                num[84]++;
            if (Housan(7, result) != 1)//杂六
                num[85]++;
            #endregion

            #region 梭哈
            if (Zhadan(result) >= 4)
                num[86]++;
            if (HuLu(result) == 1)
                num[87]++;
            if (SHShunzi(result) == 1)
                num[88]++;
            if (SHSantiao(result) == 1)
                num[89]++;
            if (SHLiangdui(result) == 1)
                num[90]++;
            if (SHDandui(result) == 1)
                num[91]++;
            if (SHSanpai(result) == 1)
                num[92]++;
            #endregion

        }
        #endregion

        #region 显示统计结果
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<h class=\"text\" style=\"color:#FF7F00\">冷热奖号统计</h>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", Out.Hr()));
        for (int i = 0; i < 10; i++)
        {
            if ((i + 1) % 5 == 0)
                builder.Append("<b>" + i + "：<b style=\"color:red\">" + num[i] + "</b>次</b><br />");
            else
            {
                builder.Append("<b>" + i + "：<b style=\"color:red\">&nbsp;&nbsp;" + num[i] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
            }
        }
        builder.Append(Out.Tab("</div>", Out.RHr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<h class=\"text\" style=\"color:#FF7F00\">大小单双统计</h>");
        builder.Append(Out.Tab("</div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<h class=\"text\" style=\"color:#FF7F00\">万位 </h>");
        builder.Append("<b>大：<b style=\"color:red\">" + num[11] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>小：<b style=\"color:red\">" + num[12] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>单：<b style=\"color:red\">" + num[13] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>双：<b style=\"color:red\">" + num[14] + "</b>次</b><br />");

        builder.Append("<h class=\"text\" style=\"color:#FF7F00\">千位 </h>");
        builder.Append("<b>大：<b style=\"color:red\">" + num[15] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>小：<b style=\"color:red\">" + num[16] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>单：<b style=\"color:red\">" + num[17] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>双：<b style=\"color:red\">" + num[18] + "</b>次</b><br />");

        builder.Append("<h class=\"text\" style=\"color:#FF7F00\">百位 </h>");
        builder.Append("<b>大：<b style=\"color:red\">" + num[19] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>小：<b style=\"color:red\">" + num[20] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>单：<b style=\"color:red\">" + num[21] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>双：<b style=\"color:red\">" + num[22] + "</b>次</b><br />");

        builder.Append("<h class=\"text\" style=\"color:#FF7F00\">个位 </h>");
        builder.Append("<b>大：<b style=\"color:red\">" + num[23] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>小：<b style=\"color:red\">" + num[24] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>单：<b style=\"color:red\">" + num[25] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>双：<b style=\"color:red\">" + num[26] + "</b>次</b><br />");

        builder.Append("<h class=\"text\" style=\"color:#FF7F00\">个位 </h>");
        builder.Append("<b>大：<b style=\"color:red\">" + num[27] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>小：<b style=\"color:red\">" + num[28] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>单：<b style=\"color:red\">" + num[29] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>双：<b style=\"color:red\">" + num[30] + "</b>次</b><br />");

        builder.Append("<h class=\"text\" style=\"color:#FF7F00\">总和 </h>");
        builder.Append("<b>大：<b style=\"color:red\">" + num[31] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>小：<b style=\"color:red\">" + num[32] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>单：<b style=\"color:red\">" + num[33] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>双：<b style=\"color:red\">" + num[34] + "</b>次</b><br />");

        builder.Append("<h class=\"text\" style=\"color:#FF7F00\">前三 </h>");
        builder.Append("<b>大：<b style=\"color:red\">" + num[35] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>小：<b style=\"color:red\">" + num[36] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>单：<b style=\"color:red\">" + num[37] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>双：<b style=\"color:red\">" + num[38] + "</b>次</b><br />");

        builder.Append("<h class=\"text\" style=\"color:#FF7F00\">中三 </h>");
        builder.Append("<b>大：<b style=\"color:red\">" + num[39] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>小：<b style=\"color:red\">" + num[40] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>单：<b style=\"color:red\">" + num[41] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>双：<b style=\"color:red\">" + num[42] + "</b>次</b><br />");

        builder.Append("<h class=\"text\" style=\"color:#FF7F00\">后三 </h>");
        builder.Append("<b>大：<b style=\"color:red\">" + num[43] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>小：<b style=\"color:red\">" + num[44] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>单：<b style=\"color:red\">" + num[45] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>双：<b style=\"color:red\">" + num[46] + "</b>次</b><br />");

        builder.Append("<h class=\"text\" style=\"color:#FF7F00\">总和 </h>");
        builder.Append("<b>大单：<b style=\"color:red\">" + num[48] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>大双：<b style=\"color:red\">" + num[49] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>小单：<b style=\"color:red\">" + num[49] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>小双：<b style=\"color:red\">" + num[50] + "</b>次</b><br />");
        builder.Append(Out.Tab("</div>", Out.RHr()));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<h class=\"text\" style=\"color:#FF7F00\">龙虎和统计</h>");
        builder.Append(Out.Tab("</div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b>龙：<b style=\"color:red\">" + num[51] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>虎：<b style=\"color:red\">" + num[52] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>和：<b style=\"color:red\">" + num[53] + "</b>次</b><br />");
        builder.Append(Out.Tab("</div>", Out.RHr()));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<h class=\"text\" style=\"color:#FF7F00\">牛牛统计</h>");
        builder.Append(Out.Tab("</div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b>有牛：<b style=\"color:red\">" + num[54] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>无牛：<b style=\"color:red\">" + num[55] + "</b>次</b><br />");
        for (int i = 56; i < 66; i++)
        {
            if (((i - 56) + 1) % 5 == 0)
                builder.Append("<b>牛" + (i - 56) + "：<b style=\"color:red\">" + num[i] + "</b>次</b><br />");
            else
            {
                builder.Append("<b>牛" + (i - 56) + "：<b style=\"color:red\">&nbsp;&nbsp;" + num[i] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
            }
        }
        builder.Append(Out.Tab("</div>", Out.RHr()));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<h class=\"text\" style=\"color:#FF7F00\">总和五门统计</h>");
        builder.Append(Out.Tab("</div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b>一门：<b style=\"color:red\">" + num[66] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>二门：<b style=\"color:red\">" + num[67] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>三门：<b style=\"color:red\">" + num[68] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>四门：<b style=\"color:red\">" + num[69] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>五门：<b style=\"color:red\">" + num[70] + "</b>次</b><br />");
        builder.Append(Out.Tab("</div>", Out.RHr()));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<h class=\"text\" style=\"color:#FF7F00\">取三统计</h>");
        builder.Append(Out.Tab("</div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<h class=\"text\" style=\"color:#FF7F00\">前三 </h>");
        builder.Append("<b>豹子：<b style=\"color:red\">" + num[71] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>顺子：<b style=\"color:red\">" + num[72] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>对子：<b style=\"color:red\">" + num[73] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>半顺：<b style=\"color:red\">" + num[74] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>杂六：<b style=\"color:red\">" + num[75] + "</b>次</b><br />");

        builder.Append("<h class=\"text\" style=\"color:#FF7F00\">中三 </h>");
        builder.Append("<b>豹子：<b style=\"color:red\">" + num[76] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>顺子：<b style=\"color:red\">" + num[77] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>对子：<b style=\"color:red\">" + num[78] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>半顺：<b style=\"color:red\">" + num[79] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>杂六：<b style=\"color:red\">" + num[80] + "</b>次</b><br />");

        builder.Append("<h class=\"text\" style=\"color:#FF7F00\">后三 </h>");
        builder.Append("<b>豹子：<b style=\"color:red\">" + num[81] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>顺子：<b style=\"color:red\">" + num[82] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>对子：<b style=\"color:red\">" + num[83] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>半顺：<b style=\"color:red\">" + num[84] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>杂六：<b style=\"color:red\">" + num[85] + "</b>次</b><br />");
        builder.Append(Out.Tab("</div>", Out.RHr()));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<h class=\"text\" style=\"color:#FF7F00\">梭哈统计</h>");
        builder.Append(Out.Tab("</div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b>炸弹：<b style=\"color:red\">" + num[86] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>葫芦：<b style=\"color:red\">" + num[87] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>顺子：<b style=\"color:red\">" + num[88] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>三条：<b style=\"color:red\">" + num[89] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>两对：<b style=\"color:red\">" + num[90] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>单对：<b style=\"color:red\">" + num[91] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>散牌：<b style=\"color:red\">" + num[92] + "</b>次</b>");
        builder.Append(Out.Tab("</div>", ""));
        #endregion

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
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
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx") + "\">" + GameName + "</a>");
        builder.Append("&gt;兑奖查看");
        builder.Append(Out.Tab("</div>", "<br />"));

        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-1]$", "0"));
        //用户ID
        int usid = int.Parse(Utils.GetRequest("usid", "all", 1, @"^[1-9]\d*$", "0"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=chaxun") + "\">期号查询</a>" + "|<a href=\"" + Utils.getUrl("ssc.aspx?act=usidcx") + "\">会员查询</a>" + "|<a href=\"" + Utils.getUrl("ssc.aspx?act=jiangcx") + "\">往期分析</a>|查兑奖");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("〓查兑奖〓");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        if (ptype == 0)
        {
            builder.Append("未兑奖 | <a href=\"" + Utils.getUrl("ssc.aspx?act=case&amp;ptype=1&amp;usid=" + usid + "") + "\">已兑奖</a>");
        }
        else
        {
            builder.Append(" <a href=\"" + Utils.getUrl("ssc.aspx?act=case&amp;ptype=0&amp;usid=" + usid + "") + "\">未兑奖</a> | 已兑奖");
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
        IList<BCW.ssc.Model.SSCpay> listSSCpay = new BCW.ssc.BLL.SSCpay().GetSSCpays(pageIndex, pageSize, strWhere, out recordCount);
        if (listSSCpay.Count > 0)
        {
            int k = 1;
            foreach (BCW.ssc.Model.SSCpay n in listSSCpay)
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
                if (n.SSCId.ToString() != SSCqi)
                {
                    if (n.Result == "")
                        builder.Append("=第" + n.SSCId + "期=<br />");
                    else
                        builder.Append("=第" + n.SSCId + "期=开出:<f style=\"color:red\">" + n.Result + " (" + Message(n.Result) + ")" + "</f><br />");
                }

                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<b>" + ((pageIndex - 1) * pageSize + k) + ".</b>");
                builder.Append("<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(n.UsID) + "(" + n.UsID + ")" + "</a> <b>[" + OutType(n.Types) + "]</b>位号:" + n.Notes + "/每注" + n.Price + "" + ub.Get("SiteBz") + "/共" + n.iCount + "注/赔率" + n.Odds + "/标识ID:" + n.ID + "[" + DT.FormatDate(n.AddTime, 1) + "]");

                if (n.WinCent > 0)
                {
                    builder.Append("中" + GetZj_zs(n.WinNotes) + "注/赢" + n.WinCent + "" + ub.Get("SiteBz") + "");
                    if (ptype == 0)
                    {
                        builder.Append("|<f style=\"color:blue\"><a href=\"" + Utils.getUrl("ssc.aspx?act=caseok&amp;id=" + n.ID + "") + "\">帮他兑奖</a></f>");
                    }
                    else
                    {
                        builder.Append("|<f style=\"color:black\">已兑奖</f>");
                    }
                }
                builder.Append(Out.Tab("</div>", ""));
                SSCqi = n.SSCId.ToString();
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
        string strOthe = "确认搜索,ssc.aspx?act=case&amp;ptype=" + ptype + ",post,1,red";
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
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx") + "\">" + GameName + "</a>");
        builder.Append("&gt;帮他兑奖");
        builder.Append(Out.Tab("</div>", "<br />"));

        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[1-9]\d*$", "0"));
        string info = Utils.GetRequest("info", "all", 1, "", "");
        BCW.ssc.Model.SSCpay n = new BCW.ssc.BLL.SSCpay().GetSSCpay(id);

        if (info == "ok")
        {
            if (new BCW.ssc.BLL.SSCpay().ExistsState(id, n.UsID))
            {
                int guestset = Utils.ParseInt(ub.GetSub("SSCGuestSet", xmlPath));
                new BCW.ssc.BLL.SSCpay().UpdateState(id, 2);

                BCW.User.Users.IsFresh("ssc", 1);//防刷
                BCW.ssc.Model.SSClist idd = new BCW.ssc.BLL.SSClist().GetSSClistbySSCId(n.SSCId);
                new BCW.BLL.User().UpdateiGold(n.UsID, new BCW.BLL.User().GetUsName(n.UsID), n.WinCent, "" + GameName + "兑奖-" + "[url=./game/ssc.aspx?act=view&amp;id=" + idd.ID + "&amp;ptype=2]" + n.SSCId + "[/url]" + "-标识ID" + n.ID + "");
                if (new BCW.BLL.User().GetIsSpier(n.UsID) != 1)
                    new BCW.BLL.User().UpdateiGold(108, new BCW.BLL.User().GetUsName(108), -n.WinCent, "ID:[url=forumlog.aspx?act=xview&amp;uid=" + n.UsID + "]" + n.UsID + "[/url]" + GameName + "第[url=./game/ssc.aspx?act=view&amp;id=" + idd.ID + "&amp;ptype=2]" + n.SSCId + "[/url]期兑奖" + n.WinCent + "|(标识ID" + n.ID + ")");
                if (guestset == 0)
                    new BCW.BLL.Guest().Add(1, n.UsID, BCW.User.Users.SetUser(n.UsID), "您在[URL=/bbs/game/ssc.aspx]" + GameName + "[/URL]第" + n.SSCId + "期的投注：" + n.Notes + "系统已经帮您兑奖，获得了" + n.WinCent + ub.Get("SiteBz") + "。");//开奖提示信息,1表示开奖信息
                Utils.Success("兑奖", "恭喜，成功帮他兑奖" + n.WinCent + "" + ub.Get("SiteBz") + "", Utils.getUrl("ssc.aspx?act=case&amp;ptype=0"), "1");

            }
            else
            {
                Utils.Success("兑奖", "重复兑奖或没有可以兑奖的记录", Utils.getUrl("ssc.aspx?act=case&amp;ptype=0"), "1");
            }
        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("=第" + n.SSCId + "期=开出:<f style=\"color:red\">" + n.Result + "</f><br />");

            builder.Append("用户：<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(n.UsID) + " (" + n.UsID + ")" + "</a><br /> ");
            builder.Append("兑奖ID：" + id + "<br />投注方式：<b>[" + OutType(n.Types) + "]</b>位号:" + n.Notes + "<br />每注：" + n.Price + "" + ub.Get("SiteBz") + "<br />共投：" + n.iCount + "注<br />赔率：" + n.Odds + "<br />投注时间：" + DT.FormatDate(n.AddTime, 1) + "<br />");

            if (n.WinCent > 0)
            {
                builder.Append("中奖：" + GetZj_zs(n.WinNotes) + "注/赢" + n.WinCent + "" + ub.Get("SiteBz") + "");

                builder.Append("<br /><a href=\"" + Utils.getUrl("ssc.aspx?act=caseok&amp;info=ok&amp;id=" + n.ID + "") + "\">确定帮他兑奖</a>");
            }
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    //盈利分析
    private void StatPage()
    {
        Master.Title = "" + GameName + "赢利分析";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx") + "\">" + GameName + "</a>&gt;盈利分析");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("=赢利分析=");
        builder.Append(Out.Tab("</div>", "<br />"));

        //今天本金与赢利
        long TodayBuyCent = new BCW.ssc.BLL.SSCpay().GetSumPrices("State>0 and IsSpier=0  and UsID in (select ID from tb_User where IsSpier!=1)  and Year(AddTime) = " + DateTime.Now.Year + " AND Month(AddTime) = " + DateTime.Now.Month + " and Day(AddTime) = " + DateTime.Now.Day + "");
        long TodayWinCent = new BCW.ssc.BLL.SSCpay().GetSumWinCent("State>0 and IsSpier=0  and UsID in (select ID from tb_User where IsSpier!=1)  and Year(AddTime) = " + DateTime.Now.Year + " AND Month(AddTime) = " + DateTime.Now.Month + " and Day(AddTime) = " + DateTime.Now.Day + "");

        //昨天本金与赢利
        long YesBuyCent = new BCW.ssc.BLL.SSCpay().GetSumPrices("State>0 and IsSpier=0  and UsID in (select ID from tb_User where IsSpier!=1)  and Year(AddTime) = " + DateTime.Now.AddDays(-1).Year + " AND Month(AddTime) = " + DateTime.Now.AddDays(-1).Month + " and Day(AddTime) = " + DateTime.Now.AddDays(-1).Day + " ");
        long YesWinCent = new BCW.ssc.BLL.SSCpay().GetSumWinCent("State>0 and IsSpier=0  and UsID in (select ID from tb_User where IsSpier!=1)  and Year(AddTime) = " + DateTime.Now.AddDays(-1).Year + " AND Month(AddTime) = " + DateTime.Now.AddDays(-1).Month + " and Day(AddTime) = " + DateTime.Now.AddDays(-1).Day + " ");

        //本月本金与赢利
        long MonthBuyCent = new BCW.ssc.BLL.SSCpay().GetSumPrices("State>0 and IsSpier=0  and UsID in (select ID from tb_User where IsSpier!=1)  and Year(AddTime) = " + (DateTime.Now.Year) + " AND Month(AddTime) = " + (DateTime.Now.Month) + " ");
        long MonthWinCent = new BCW.ssc.BLL.SSCpay().GetSumWinCent("State>0 and IsSpier=0  and UsID in (select ID from tb_User where IsSpier!=1)  and Year(AddTime) = " + (DateTime.Now.Year) + " AND Month(AddTime) = " + (DateTime.Now.Month) + " ");

        //上月本金与赢利
        long Month2BuyCent = new BCW.ssc.BLL.SSCpay().GetSumPrices("State>0 and IsSpier=0  and UsID in (select ID from tb_User where IsSpier!=1)  and Year(AddTime) = " + (DateTime.Now.AddMonths(-1).Year) + " AND Month(AddTime) = " + (DateTime.Now.AddMonths(-1).Month) + " ");
        long Month2WinCent = new BCW.ssc.BLL.SSCpay().GetSumWinCent("State>0 and IsSpier=0  and UsID in (select ID from tb_User where IsSpier!=1)  and Year(AddTime) = " + (DateTime.Now.AddMonths(-1).Year) + " AND Month(AddTime) = " + (DateTime.Now.AddMonths(-1).Month) + " ");

        //总本金与赢利
        long BuyCent = new BCW.ssc.BLL.SSCpay().GetSumPrices("State>0 and IsSpier=0  and UsID in (select ID from tb_User where IsSpier!=1) ");
        long WinCent = new BCW.ssc.BLL.SSCpay().GetSumWinCent("State>0 and IsSpier=0  and UsID in (select ID from tb_User where IsSpier!=1) ");


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

        builder.Append(Out.Tab("<div class=\"hr\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=tstate") + "\">按时间段进行盈利分析</a>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("ssc.aspx") + "\">返回上一级</a><br />");
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
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx") + "\">" + GameName + "</a>");
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
        //    builder.Append("所有分析|<a href=\"" + Utils.getUrl("ssc.aspx?act=tstate&amp;ptype=1") + "\">用户分析</a>|<a href=\"" + Utils.getUrl("ssc.aspx?act=tstate&amp;ptype=2") + "\">机器人分析</a>");
        //if (ptype == 1)
        //    builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=tstate&amp;ptype=0") + "\">所有分析</a>|用户分析|<a href=\"" + Utils.getUrl("ssc.aspx?act=tstate&amp;ptype=2") + "\">机器人分析</a>");
        //if (ptype == 2)
        //    builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=tstate&amp;ptype=0") + "\">所有分析</a>|<a href=\"" + Utils.getUrl("ssc.aspx?act=tstate&amp;ptype=1") + "\">用户分析</a>|机器人分析");
        builder.Append(Out.Tab("</div>", "<br />"));

        string strText = "开始时间:,结束时间:,";
        string strName = "stTime,ovTime,act";
        string strType = "date,date,hidden";
        string strValu = "" + sTime.ToString("yyyy-MM-dd HH:mm:ss") + "'" + oTime.ToString("yyyy-MM-dd HH:mm:ss") + "'";
        string strEmpt = "false,false,false";
        string strIdea = "/";
        string strOthe = "进行查询,ssc.aspx?act=tstate&amp;ptype=" + ptype + ",post,1,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        long TimeBuyCent = 0;
        long TimeWinCent = 0;

        if (ptype == 0)
        {
            TimeBuyCent = new BCW.ssc.BLL.SSCpay().GetSumPrices("State>0 and AddTime > '" + sTime + "' and AddTime <'" + oTime + "'");
            TimeWinCent = new BCW.ssc.BLL.SSCpay().GetSumWinCent("State>0 and AddTime > '" + sTime + "' and AddTime <'" + oTime + "'");
        }
        if (ptype == 1)
        {
            TimeBuyCent = new BCW.ssc.BLL.SSCpay().GetSumPrices("State>0  and IsSpier=0  and UsID in (select ID from tb_User where IsSpier!=1)  and AddTime > '" + sTime + "' and AddTime <'" + oTime + "'");
            TimeWinCent = new BCW.ssc.BLL.SSCpay().GetSumWinCent("State>0 and IsSpier=0  and UsID in (select ID from tb_User where IsSpier!=1)  and AddTime > '" + sTime + "' and AddTime <'" + oTime + "'");
        }
        if (ptype == 2)
        {
            TimeBuyCent = new BCW.ssc.BLL.SSCpay().GetSumPrices("State>0 and IsSpier=1  and AddTime > '" + sTime + "' and AddTime <'" + oTime + "'");
            TimeWinCent = new BCW.ssc.BLL.SSCpay().GetSumWinCent("State>0  and IsSpier=1 and AddTime > '" + sTime + "' and AddTime <'" + oTime + "'");
        }

        string str = string.Empty;
        if (ptype == 0)
            str = "全部分析";
        if (ptype == 1)
            str = "用户分析";
        if (ptype == 2)
            str = "机器人分析";

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("从" + sTime.ToString("yyyy-MM-dd HH:mm:ss") + "到" + oTime.ToString("yyyy-MM-dd HH:mm:ss") + str + "<br />");
        builder.Append("下注:" + TimeBuyCent + "" + ub.Get("SiteBz") + "<br />");
        builder.Append("返彩:" + TimeWinCent + "" + ub.Get("SiteBz") + "<br />");
        builder.Append("净赢:" + (TimeBuyCent - TimeWinCent) + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", Out.Hr()));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("说明：【用户分析】是会员用户记录分析");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("ssc.aspx?act=stat") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    //期数详情
    private void ViewPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]\d*$", "1"));
        BCW.ssc.Model.SSClist model = new BCW.ssc.BLL.SSClist().GetSSClist(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "第" + model.SSCId + "期" + GameName + "";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx") + "\">" + GameName + "</a>&gt;第" + model.SSCId + "期");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("查看下注/开奖记录");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("查看:");
        if (ptype == 1)
            builder.Append("下注|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=view&amp;ptype=1&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">下注</a>|");

        if (ptype == 2)
            builder.Append("中奖");
        else
            builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=view&amp;ptype=2&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">中奖</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        if (ptype == 1)
            strWhere += "SSCId=" + model.SSCId + "";
        else
            strWhere += "SSCId=" + model.SSCId + " and state>0 and winCent>0";

        string[] pageValUrl = { "act", "id", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.ssc.Model.SSCpay> listSSCpay = new BCW.ssc.BLL.SSCpay().GetSSCpays(pageIndex, pageSize, strWhere, out recordCount);
        if (listSSCpay.Count > 0)
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("第" + model.SSCId + "期开出:<b>" + model.Result + " (" + Message(model.Result) + ")</b>");
            if (model.Result != "")
                builder.Append(" <a href=\"" + Utils.getUrl("ssc.aspx?act=opentwo&amp;&amp;id=" + id + "") + "\">[重开奖]</a>");

            try
            {
                string sta = new BCW.ssc.BLL.SSClist().GetStateTime(model.SSCId);
                string[] sta1 = sta.Split('#');
                string sta2 = sta1[sta1.Length - 1];
                string[] sta3 = sta2.Split('|');
                int x = Convert.ToInt32(sta3[0]);
                string relt = sta3[2];
                string time = sta3[1];
                if (x == 0)
                {
                    builder.Append("<br />开奖时间:" + time + "");
                }
                if (x == 1)
                {
                    builder.Append("<br /><f style=\"color:red\">开奖类型:人工开奖 开奖时间:" + time + "</f>");
                }
                if (x >= 2)
                {
                    builder.Append("<br /><f style=\"color:red\">开奖类型:第" + (x - 1) + "次重开奖 开奖时间:" + time + "</f>");
                }
            }
            catch { }

            if (ptype == 1)
                builder.Append("<br />共" + recordCount + "注下注 会员下注（非机器系统号）总计" + GetPayAllbySSCId(model.SSCId) + ub.Get("SiteBz") + "");
            else
                builder.Append("<br />共" + recordCount + "注中奖 会员中奖（非机器系统号）总计" + GetWincentbySSCId(model.SSCId) + ub.Get("SiteBz"));

            builder.Append(Out.Tab("</div>", "<br />"));

            int k = 1;
            foreach (BCW.ssc.Model.SSCpay n in listSSCpay)
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

                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>");

                builder.Append("<b>[" + OutType(n.Types) + "]</b>位号:" + n.Notes + "/每注" + n.Price + "" + ub.Get("SiteBz") + "/共" + n.iCount + "注/赔率" + n.Odds + "/标识ID:" + n.ID + "[" + DT.FormatDate(n.AddTime, 1) + "]");
                if (n.WinCent > 0)
                {
                    builder.Append("赢" + n.WinCent + "" + ub.Get("SiteBz") + "（中" + GetZj_zs(n.WinNotes) + "注）");
                }
                builder.Append(".<a href=\"" + Utils.getUrl("ssc.aspx?act=del&amp;SSCid=" + n.ID + "&amp;id=" + id + "&amp;ptype=" + ptype + "") + "\">[退]</a>");
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

        }
        else
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("第" + model.SSCId + "期开出:<b>" + model.Result + "</b>");

            try
            {
                string sta = new BCW.ssc.BLL.SSClist().GetStateTime(model.SSCId);
                string[] sta1 = sta.Split('#');
                string sta2 = sta1[sta1.Length - 1];
                string[] sta3 = sta2.Split('|');
                int x = Convert.ToInt32(sta3[0]);
                string relt = sta3[2];
                string time = sta3[1];
                if (x == 0)
                {
                    builder.Append("<br />开奖时间:" + time + "");
                }
                if (x == 1)
                {
                    builder.Append("<br /><f style=\"color:red\">开奖类型:人工开奖 开奖时间:" + time + "</f>");
                }
                if (x >= 2)
                {
                    builder.Append("<br /><f style=\"color:red\">开奖类型:第" + (x - 1) + "次重开奖 开奖时间:" + time + "</f>");
                }
            }
            catch { }

            builder.Append("<br />共0注中奖");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("ssc.aspx") + "\">返回上一级</a>");
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
            int SSCid = int.Parse(Utils.GetRequest("SSCid", "all", 1, @"^[0-9]\d*$", "1"));
            BCW.ssc.Model.SSCpay n = new BCW.ssc.BLL.SSCpay().GetSSCpay(SSCid);

            Master.Title = "" + GameName + "第" + n.SSCId + "期删除一条投注记录";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
            builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx") + "\">" + GameName + "</a>");
            builder.Append("&gt;第" + n.SSCId + "期");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("确定删除该记录吗?");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("ID:" + n.ID + ".<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")</a>");
            builder.Append("<b>[" + OutType(n.Types) + "]</b>位号:" + n.Notes + "/每注" + n.Price + "" + ub.Get("SiteBz") + "/共买" + n.iCount + "注/总下注" + n.Prices + ub.Get("SiteBz") + "/赔率" + n.Odds + "/标识ID:" + n.ID + " [" + DT.FormatDate(n.AddTime, 1) + "]");

            if (n.WinCent > 0)
            {
                builder.Append("&nbsp; 中" + GetZj_zs(n.WinNotes) + "注/赢" + n.WinCent + "" + ub.Get("SiteBz") + "");
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
            builder.Append(Out.Tab("</div>", ""));

            string strText = ",,退注原因（可为空）:/,,";
            string strName = "id,SSCid,text";
            string strType = "hidden,hidden,text";
            string strValu = "" + id + "'" + n.ID + "'" + "" + "'" + ptype + "'";
            string strEmpt = "false,false,true";
            string strIdea = "/";
            string strOthe = "确定删除,ssc.aspx?info=ok&amp;act=del,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", "<br />"));
            //builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?info=ok&amp;act=del&amp;id=" + id + "&amp;SSCid=" + n.ID + "&amp;ptype=" + ptype + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=view&amp;id=" + id + "&amp;ptype=" + ptype + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=view&amp;id=" + id + "&amp;ptype=" + ptype + "") + "\">返回上一级</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
            int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-9]\d*$", "1"));
            int SSCid = int.Parse(Utils.GetRequest("SSCid", "all", 1, @"^[0-9]\d*$", "1"));

            string tets = Utils.GetRequest("text", "all", 1, @"^[^\^]{0,20000}$", "");
            string why = string.Empty;
            if (tets != "")
            {
                why = "退注原因：" + tets + "";
            }
            else { why = ""; }

            BCW.ssc.Model.SSCpay n = new BCW.ssc.BLL.SSCpay().GetSSCpay(SSCid);
            if (!new BCW.ssc.BLL.SSCpay().Exists(SSCid))
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
                    new BCW.BLL.User().UpdateiGold(meid, n.Prices, "系统退回" + GameName + "第" + n.SSCId + "期下注的" + n.Prices + "" + ub.Get("SiteBz") + "！");//减少系统总的酷币
                    new BCW.BLL.Guest().Add(1, meid, mename, "系统退回您的" + GameName + "：第" + n.SSCId + "期下注的" + n.Prices + "" + ub.Get("SiteBz") + "！" + why + "");//
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
                        owe.Content = "" + GameName + n.SSCId + "期" + OutType(n.Types) + "下注" + n.Prices + "" + ub.Get("SiteBz") + ".系统管理员" + new BCW.User.Manage().IsManageLogin() + "删除.";
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
                    new BCW.BLL.User().UpdateiGold(n.UsID, n.Prices - n.WinCent, "无效购奖或非法操作，系统退回" + GameName + "第" + n.SSCId + "期下注的" + n.Prices + "" + ub.Get("SiteBz") + "." + oop + "" + ui);//减少系统总的酷币
                    new BCW.BLL.Guest().Add(1, meid, mename, "无效购奖或非法操作，系统退回" + GameName + "第" + n.SSCId + "期下注的" + n.Prices + "" + ub.Get("SiteBz") + "." + oop + "" + ui + "" + why + "");//
                }
                ////如果过期不兑奖，退回本金
                //else if (state_get == 3)
                //{
                //    Price = model.PutGold;
                //    new BCW.BLL.User().UpdateiGold(model.UsID, Price, "系统退回新快3第" + model.Lottery_issue + "期未兑奖的" + model.PutGold + "酷币！");//减少系统总的酷币
                //    new BCW.BLL.Guest().Add(1, meid, mename, "系统退回新快3第" + model.Lottery_issue + "期未兑奖的" + model.PutGold + "酷币！");
                //}

                new BCW.ssc.BLL.SSCpay().Delete(n.ID);

                Utils.Success("删除投注记录", "删除记录成功..", Utils.getPage("ssc.aspx?act=view&amp;id=" + id + "&amp;ptype=" + ptype + ""), "2");

            }
        }
    }

    //手动开奖
    private void OpenPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));

        BCW.ssc.Model.SSClist model = new BCW.ssc.BLL.SSClist().GetSSClist(id);

        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.State == 1)
        {
            //Utils.Error("该期已开奖", "");
        }

        Master.Title = "" + GameName + "第" + model.SSCId + "手动开奖";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx") + "\">" + GameName + "</a>");
        builder.Append("&gt;第" + model.SSCId + "期手动开奖");
        builder.Append(Out.Tab("</div>", "<br />"));

        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {

            string rResult = Utils.GetRequest("rResult", "all", 2, @"^\d\s\d\s\d\s\d\s\d$", "中奖号码填写错误");

            new BCW.ssc.BLL.SSClist().UpdateResult(model.SSCId, rResult);
            new BCW.ssc.BLL.SSCpay().UpdateResult(model.SSCId, rResult);
            //更新开奖类型时间
            string StateTime = string.Empty;
            try
            {
                string staet = new BCW.ssc.BLL.SSClist().GetStateTime(model.SSCId);
                if (staet == " " || staet == null || staet == "")
                {
                    StateTime = "1|" + DateTime.Now + "|" + rResult;//手动开奖
                }
                else
                {
                    StateTime = staet + "#" + "1|" + DateTime.Now + "|" + rResult;
                }
            }
            catch
            { StateTime = "1|" + DateTime.Now + "|" + rResult; }
            new BCW.ssc.BLL.SSClist().UpdateStateTime(model.SSCId, StateTime);//0自动开奖|time0时间#1|time1手动开奖#2|time2重开奖.....

            //开始返彩
            DataSet ds = new BCW.ssc.BLL.SSCpay().GetList("ID,Types,SSCId,UsID,UsName,Price,Notes,Result,Odds", "State=0 and Result<>''");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    int ID = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                    int Types = int.Parse(ds.Tables[0].Rows[i]["Types"].ToString());
                    int SSCId = int.Parse(ds.Tables[0].Rows[i]["SSCId"].ToString());
                    string Notes = ds.Tables[0].Rows[i]["Notes"].ToString();
                    int UsID = int.Parse(ds.Tables[0].Rows[i]["UsID"].ToString());
                    string UsName = ds.Tables[0].Rows[i]["UsName"].ToString();
                    string Result = ds.Tables[0].Rows[i]["Result"].ToString();
                    long Price = Int64.Parse(ds.Tables[0].Rows[i]["Price"].ToString());
                    decimal Odds = decimal.Parse(ds.Tables[0].Rows[i]["Odds"].ToString()); //赔率

                    #region 返奖
                    if (Types == 18)//任一 
                    {
                        string[] iNum_kj = Result.Split(' ');  //该期开奖的结果
                        string[] iNum_zj = Notes.Split(',');  //该期该用户购买的彩票

                        int zj_zs = 0; //统计中奖注数

                        if (BackLikeNum(Result) == 5)//开奖结果五个结果不同
                        {
                            for (int fs = 0; fs < iNum_zj.Length; fs++)
                                for (int p = 0; p < iNum_kj.Length; p++)
                                    if (string.Compare(iNum_zj[fs], iNum_kj[p]) == 0)
                                        zj_zs += 1;
                        }
                        else//开奖结果有相同的，把相同的号码合并为一个号码，再比对开奖
                        {
                            string ret = string.Empty;
                            ArrayList list = new ArrayList();
                            for (int j = 0; j < iNum_kj.Length; j++)
                            {
                                if (!list.Contains(iNum_kj[j]))
                                {
                                    list.Add(iNum_kj[j]);
                                }
                            }
                            for (int k = 0; k < list.Count; k++)
                            {
                                ret += list[k] + " ";
                            }

                            string[] kj = ret.Split(' ');
                            for (int fs = 0; fs < iNum_zj.Length; fs++)
                                for (int p = 0; p < kj.Length; p++)
                                    if (string.Compare(iNum_zj[fs], kj[p]) == 0)
                                        zj_zs += 1;

                        }


                        if (zj_zs > 0)//中奖
                        {

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 16)//任选号码
                    {
                        string[] iNum_kj = Result.Split(' ');  //该期开奖的结果
                        string[] iNum_zj = Notes.Split(',');  //该期该用户购买的彩票

                        int zj_zs = 0; //统计中奖注数

                        for (int fs = 0; fs < iNum_zj.Length; fs++)
                            for (int p = 0; p < iNum_kj.Length; p++)
                                if (string.Compare(iNum_zj[fs], iNum_kj[p]) == 0)
                                    zj_zs += 1;
                        if (zj_zs > 0)//中奖
                        {

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖倍数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 1 || Types == 4 || Types == 7 || Types == 10 || Types == 13)// 万、千、百、十、个位直选
                    {
                        string[] iNum_kj = Result.Split(' ');  //该期开奖的结果
                        string[] iNum_zj = Notes.Split(',');  //该期该用户购买的彩票

                        int zj_zs = 0; //统计中奖注数

                        int p = 0;
                        if (Types == 1)//万位 
                        {
                            for (int fs = 0; fs < iNum_zj.Length; fs++)
                            {
                                if (string.Compare(iNum_zj[fs], iNum_kj[0]) == 0)
                                    zj_zs += 1;
                            }
                        }
                        else if (Types == 4)//千位 
                        {
                            for (int fs = 0; fs < iNum_zj.Length; fs++)
                            {
                                if (string.Compare(iNum_zj[fs], iNum_kj[1]) == 0)
                                    zj_zs += 1;
                            }
                        }
                        else if (Types == 7)//百位
                        {
                            for (int fs = 0; fs < iNum_zj.Length; fs++)
                            {
                                if (string.Compare(iNum_zj[fs], iNum_kj[2]) == 0)
                                    zj_zs += 1;
                            }
                        }
                        else if (Types == 10)//十位
                        {
                            for (int fs = 0; fs < iNum_zj.Length; fs++)
                            {
                                if (string.Compare(iNum_zj[fs], iNum_kj[3]) == 0)
                                    zj_zs += 1;
                            }
                        }
                        else if (Types == 13)//个位 
                        {
                            for (int fs = 0; fs < iNum_zj.Length; fs++)
                            {
                                if (string.Compare(iNum_zj[fs], iNum_kj[4]) == 0)
                                    zj_zs += 1;
                            }
                        }
                        if (zj_zs > 0)//中奖
                        {

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 19 || Types == 20 || Types == 21 || Types == 22)//任二、三、四、五
                    {
                        string[] iNum_kj = Result.Split(' ');  //存放结果
                        string[] iNum_zj = Notes.Split('#');  //把胆码和拖码分开存储
                        string[] iNum_zj_d = iNum_zj[0].Split(','); //胆码
                        string[] iNum_zj_t = iNum_zj[1].Split(',');  //拖码

                        if (iNum_zj[0] == "")//无胆码，为普通选
                        {
                            int zj_zs = 0; //统计中奖注数

                            if (BackLikeNum(Result) == 5)//开奖的五个号不同
                            {
                                for (int fs = 0; fs < iNum_zj_t.Length; fs++)
                                    for (int p = 0; p < iNum_kj.Length; p++)
                                        if (string.Compare(iNum_zj_t[fs], iNum_kj[p]) == 0)
                                            zj_zs += 1;
                            }
                            else//开奖号有相同的
                            {
                                string ret = string.Empty;
                                ArrayList list = new ArrayList();
                                for (int j = 0; j < iNum_kj.Length; j++)
                                {
                                    if (!list.Contains(iNum_kj[j]))
                                    {
                                        list.Add(iNum_kj[j]);
                                    }
                                }
                                for (int k = 0; k < list.Count; k++)
                                {
                                    ret += list[k] + " ";
                                }

                                string[] kj = ret.Split(' ');
                                for (int fs = 0; fs < iNum_zj_t.Length; fs++)
                                    for (int p = 0; p < kj.Length; p++)
                                        if (string.Compare(iNum_zj_t[fs], kj[p]) == 0)
                                            zj_zs += 1;
                            }


                            switch (Types)
                            {
                                case 22: //任选五普通
                                    {
                                        if (zj_zs >= 5 && BackLikeNum(Result) == 5)
                                        {
                                            zj_zs = C(zj_zs, 5);
                                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                                            string WinNotes = "中奖注数:" + zj_zs;
                                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                                        }
                                        else
                                            zj_zs = 0;
                                    }
                                    break;
                                case 21: //任选四普通
                                    {
                                        if (zj_zs >= 4 && BackLikeNum(Result) >= 4)
                                        {
                                            if (BackLikeNum(Result) == 5)
                                            {
                                                zj_zs = C(zj_zs, 4);
                                            }
                                            if (BackLikeNum(Result) == 4)
                                            {
                                                zj_zs = 1;
                                            }

                                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                                            string WinNotes = "中奖注数:" + zj_zs;
                                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                                        }
                                        else
                                            zj_zs = 0;
                                    }
                                    break;
                                case 20: //任选三普通
                                    {
                                        if (zj_zs >= 3 && BackLikeNum(Result) >= 3)
                                        {
                                            if (BackLikeNum(Result) == 5)
                                            {
                                                zj_zs = C(zj_zs, 3);
                                            }
                                            if (BackLikeNum(Result) == 4)
                                            {
                                                zj_zs = C(zj_zs, 3);
                                            }
                                            if (BackLikeNum(Result) == 3)
                                            {
                                                zj_zs = 1;
                                            }

                                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                                            string WinNotes = "中奖注数:" + zj_zs;
                                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                                        }
                                        else
                                            zj_zs = 0;
                                    }
                                    break;
                                default: //任选二普通
                                    {
                                        if (zj_zs >= 2 && BackLikeNum(Result) >= 2)
                                        {
                                            if (BackLikeNum(Result) == 5)
                                            {
                                                zj_zs = C(zj_zs, 2);
                                            }
                                            if (BackLikeNum(Result) == 4)
                                            {
                                                zj_zs = C(zj_zs, 2);
                                            }
                                            if (BackLikeNum(Result) == 3)
                                            {
                                                zj_zs = C(zj_zs, 2);
                                            }
                                            if (BackLikeNum(Result) == 2)
                                            {
                                                zj_zs = 1;
                                            }

                                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                                            string WinNotes = "中奖注数:" + zj_zs;
                                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                                        }
                                        else
                                            zj_zs = 0;
                                    }
                                    break;
                            }
                        }
                        else//有胆码，为胆拖
                        {
                            int zj_ds = 0; //中奖的胆码数
                            int zj_ts = 0; //中奖的拖码数
                            int zj = 0; //中奖的注数

                            if (BackLikeNum(Result) == 5)//开奖的五个号不同
                            {
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
                            }
                            else//开奖号有相同的
                            {
                                string ret = string.Empty;
                                ArrayList list = new ArrayList();
                                for (int j = 0; j < iNum_kj.Length; j++)
                                {
                                    if (!list.Contains(iNum_kj[j]))
                                    {
                                        list.Add(iNum_kj[j]);
                                    }
                                }
                                for (int k = 0; k < list.Count; k++)
                                {
                                    ret += list[k] + " ";
                                }

                                string[] kj = ret.Split(' ');
                                for (int fs = 0; fs < iNum_zj_d.Length; fs++)
                                    for (int p = 0; p < kj.Length; p++)
                                        if (string.Compare(iNum_zj_d[fs], kj[p]) == 0)
                                            zj_ds += 1;
                            }
                            if (zj_ds == iNum_zj_d.Length) //判断胆码是否全中
                            {
                                if (BackLikeNum(Result) == 5)
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
                                }
                                else//开奖结果有相同的，把相同的号码合并为一个号码，再比对开奖
                                {
                                    string ret = string.Empty;
                                    ArrayList list = new ArrayList();
                                    for (int j = 0; j < iNum_kj.Length; j++)
                                    {
                                        if (!list.Contains(iNum_kj[j]))
                                        {
                                            list.Add(iNum_kj[j]);
                                        }
                                    }
                                    for (int k = 0; k < list.Count; k++)
                                    {
                                        ret += list[k] + " ";
                                    }

                                    string[] kj = ret.Split(' ');
                                    for (int fs = 0; fs < iNum_zj_t.Length; fs++)
                                        for (int p = 0; p < kj.Length; p++)
                                            if (string.Compare(iNum_zj_t[fs], kj[p]) == 0)
                                                zj_ts += 1;

                                }

                                switch (Types)
                                {
                                    case 22: //五胆拖类型
                                        {
                                            if (zj_ds + zj_ts >= 5)
                                            {
                                                zj = C(zj_ts, 5 - iNum_zj_d.Length);

                                                long WinCent = Convert.ToInt64(Price * Odds * zj);
                                                string WinNotes = "中奖注数:" + zj;
                                                new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                                            }
                                        }
                                        break;
                                    case 21: //四胆拖类型
                                        {
                                            if (zj_ds + zj_ts >= 4)
                                            {
                                                zj = C(zj_ts, 4 - iNum_zj_d.Length);

                                                long WinCent = Convert.ToInt64(Price * Odds * zj);
                                                string WinNotes = "中奖注数:" + zj;
                                                new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                                            }
                                        }
                                        break;
                                    case 20: //三胆拖类型
                                        {
                                            if (zj_ds + zj_ts >= 3)
                                            {
                                                zj = C(zj_ts, 3 - iNum_zj_d.Length);

                                                long WinCent = Convert.ToInt64(Price * Odds * zj);
                                                string WinNotes = "中奖注数:" + zj;
                                                new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                                            }
                                        }
                                        break;
                                    default: //二胆拖类型
                                        {
                                            if (zj_ds + zj_ts >= 2)
                                            {
                                                zj = C(zj_ts, 2 - iNum_zj_d.Length);

                                                long WinCent = Convert.ToInt64(Price * Odds * zj);
                                                string WinNotes = "中奖注数:" + zj;
                                                new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                                            }
                                        }
                                        break;
                                }
                            }
                            else  //胆码没有全中则没有中奖
                            {
                                zj = 0;
                            }
                        }
                    }
                    else if (Types == 25)//总和大小
                    {
                        string[] iNum_kj = Result.Split(' ');
                        string zj = Notes;
                        int sum = 0;
                        int zj_zs = 0;

                        for (int j = 0; j < iNum_kj.Length; j++)
                        {
                            sum += Convert.ToInt32(iNum_kj[j]);
                        }
                        string temp = Convert.ToString(sum);
                        sum = Convert.ToInt32(temp);
                        if (sum >= 23 && zj == "大")
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                        else if (sum <= 22 && zj == "小")
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 26)//总和单双
                    {
                        string[] iNum_kj = Result.Split(' ');
                        string zj = Notes;
                        int sum = 0;
                        int zj_zs = 0;

                        for (int j = 0; j < iNum_kj.Length; j++)
                        {
                            sum += Convert.ToInt32(iNum_kj[j]);
                        }
                        string temp = Convert.ToString(sum);
                        temp = temp.Substring(temp.Length - 1, 1);
                        sum = Convert.ToInt32(temp);
                        if (sum % 2 != 0 && zj == "单")
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                        else if (sum % 2 == 0 && zj == "双")
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 2 || Types == 5 || Types == 8 || Types == 11 || Types == 14)//万千百十个大小
                    {
                        string[] iNum_kj = Result.Split(' ');
                        string zj = Notes;
                        int sum = 0;
                        int zj_zs = 0;
                        int j = 0;
                        if (Types == 2) { sum = Convert.ToInt32(iNum_kj[0]); }
                        if (Types == 5) { sum = Convert.ToInt32(iNum_kj[1]); }
                        if (Types == 8) { sum = Convert.ToInt32(iNum_kj[2]); }
                        if (Types == 11) { sum = Convert.ToInt32(iNum_kj[3]); }
                        if (Types == 14) { sum = Convert.ToInt32(iNum_kj[4]); }

                        if (sum >= 5 && zj == "大")
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                        else if (sum <= 4 && zj == "小")
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 3 || Types == 6 || Types == 9 || Types == 12 || Types == 15)//万千百十个单双
                    {
                        string[] iNum_kj = Result.Split(' ');
                        string zj = Notes;
                        int sum = 0;
                        int zj_zs = 0;
                        int j = 0;
                        if (Types == 3) { j = 0; }
                        if (Types == 6) { j = 1; }
                        if (Types == 9) { j = 2; }
                        if (Types == 12) { j = 3; }
                        if (Types == 15) { j = 4; }
                        sum = Convert.ToInt32(iNum_kj[j]);
                        if (sum % 2 != 0 && zj == "单")
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                        else if (sum % 2 == 0 && zj == "双")
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 17)//龙虎和
                    {
                        string[] iNum_kj = Result.Split(' ');
                        string zj = Notes;
                        int zj_zs = 0;

                        if (Convert.ToInt32(iNum_kj[0]) > Convert.ToInt32(iNum_kj[4]) && zj == "龙")
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                        if (Convert.ToInt32(iNum_kj[0]) < Convert.ToInt32(iNum_kj[4]) && zj == "虎")
                        {
                            zj_zs = 1;
                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                        if (Convert.ToInt32(iNum_kj[0]) == Convert.ToInt32(iNum_kj[4]) && zj == "和")
                        {
                            zj_zs = 1;
                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 23)//有牛无牛
                    {
                        string result = string.Empty;
                        if (Niu(Result) == "")
                        {
                            result = "无牛";
                        }
                        else
                        {
                            result = "有牛";
                        }
                        int zj_zs = 0;
                        if (result == Notes)
                        {
                            zj_zs = 1;
                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 24)//特定牛牛
                    {
                        string a = Niu(Result);
                        int c = 0;
                        if (a != "")
                        {
                            c = Convert.ToInt32(a.Substring(a.Length - 1, 1));

                            string[] iNum_zj = Notes.Split(',');  //该期该用户购买的彩票

                            int zj_zs = 0; //统计中奖注数

                            for (int fs = 0; fs < iNum_zj.Length; fs++)
                                if (string.Compare(iNum_zj[fs], c.ToString()) == 0)
                                    zj_zs += 1;

                            if (zj_zs > 0)
                            {
                                long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                                string WinNotes = "中奖号：牛" + c + "|中奖注数:" + zj_zs;
                                new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                            }
                        }
                    }
                    else if (Types == 27)//总和五门
                    {
                        string[] iNum_kj = Result.Split(' ');
                        string zj = Notes;
                        int sum = 0;
                        int zj_zs = 0;

                        for (int j = 0; j < iNum_kj.Length; j++)
                        {
                            sum += Convert.ToInt32(iNum_kj[j]);
                        }
                        string temp = Convert.ToString(sum);
                        sum = Convert.ToInt32(temp);
                        if (sum <= 9 && zj == "一门")
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                        else if (sum > 9 && sum <= 19 && zj == "二门")
                        {
                            zj_zs = 1;
                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                        else if (sum > 19 && sum <= 29 && zj == "三门")
                        {
                            zj_zs = 1;
                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                        else if (sum > 29 && sum <= 39 && zj == "四门")
                        {
                            zj_zs = 1;
                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                        else if (sum > 39 && sum <= 45 && zj == "五门")
                        {
                            zj_zs = 1;
                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 28)//总和大小单双
                    {
                        string[] iNum_kj = Result.Split(' ');
                        string zj = Notes;
                        int sum = 0;
                        int zj_zs = 0;

                        for (int j = 0; j < iNum_kj.Length; j++)
                        {
                            sum += Convert.ToInt32(iNum_kj[j]);
                        }
                        string temp = Convert.ToString(sum);
                        sum = Convert.ToInt32(temp);
                        if (sum < 23)//小
                        {
                            if (sum % 2 == 0 && zj == "小双")
                            {
                                zj_zs = 1;

                                long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                                string WinNotes = "中奖注数:" + zj_zs;
                                new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                            }
                            if (sum % 2 != 0 && zj == "小单")
                            {
                                zj_zs = 1;
                                long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                                string WinNotes = "中奖注数:" + zj_zs;
                                new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                            }
                        }
                        else//大
                        {
                            if (sum % 2 == 0 && zj == "大双")
                            {
                                zj_zs = 1;
                                long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                                string WinNotes = "中奖注数:" + zj_zs;
                                new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                            }
                            if (sum % 2 != 0 && zj == "大单")
                            {
                                zj_zs = 1;

                                long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                                string WinNotes = "中奖注数:" + zj_zs;
                                new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                            }
                        }
                    }
                    else if (Types == 29)//梭哈炸弹
                    {
                        int zj_zs = 0;
                        if (Zhadan(Result) >= 4)
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 30)//梭哈葫芦
                    {
                        int zj_zs = 0;
                        if (HuLu(Result) == 1)
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 31)//梭哈顺子
                    {
                        int zj_zs = 0;
                        if (SHShunzi(Result) == 1)
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 32)//梭哈三条
                    {
                        int zj_zs = 0;
                        if (SHSantiao(Result) == 1)
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 33)//梭哈两对
                    {
                        int zj_zs = 0;
                        if (SHLiangdui(Result) == 1)
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 34)//梭哈单对
                    {
                        int zj_zs = 0;
                        if (SHDandui(Result) == 1)
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 35)//梭哈散牌
                    {
                        int zj_zs = 0;
                        if (SHSanpai(Result) == 1)
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 36)//前三大小
                    {
                        int zj_zs = 0;
                        if (Qiansan(1, Result) == 1 && Notes == "大")
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                        if (Qiansan(1, Result) == 2 && Notes == "小")
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 37)//前三单双
                    {
                        int zj_zs = 0;
                        if (Qiansan(2, Result) == 1 && Notes == "单")
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                        if (Qiansan(2, Result) == 2 && Notes == "双")
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 38)//前三豹子
                    {
                        int zj_zs = 0;
                        if (Qiansan(3, Result) == 1)
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 39)//前三顺子
                    {
                        int zj_zs = 0;
                        if (Qiansan(4, Result) == 1)
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 40)//前三对子
                    {
                        int zj_zs = 0;
                        if (Qiansan(5, Result) == 1)
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 41)//前三半顺
                    {
                        int zj_zs = 0;
                        if (Qiansan(6, Result) == 1)
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 42)//前三杂六
                    {
                        int zj_zs = 0;
                        if (Qiansan(7, Result) != 1)
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 43)//中三大小
                    {
                        int zj_zs = 0;
                        if (Zhongsan(1, Result) == 1 && Notes == "大")
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                        if (Zhongsan(1, Result) == 2 && Notes == "小")
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 44)//中三单双
                    {
                        int zj_zs = 0;
                        if (Zhongsan(2, Result) == 1 && Notes == "单")
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                        if (Zhongsan(2, Result) == 2 && Notes == "双")
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 45)//中三豹子
                    {
                        int zj_zs = 0;
                        if (Zhongsan(3, Result) == 1)
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 46)//中三顺子
                    {
                        int zj_zs = 0;
                        if (Zhongsan(4, Result) == 1)
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 47)//中三对子
                    {
                        int zj_zs = 0;
                        if (Zhongsan(5, Result) == 1)
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 48)//中三半顺
                    {
                        int zj_zs = 0;
                        if (Zhongsan(6, Result) == 1)
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 49)//中三杂六
                    {
                        int zj_zs = 0;
                        if (Zhongsan(7, Result) != 1)
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }

                    else if (Types == 50)//后三大小
                    {
                        int zj_zs = 0;
                        if (Housan(1, Result) == 1 && Notes == "大")
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                        if (Housan(1, Result) == 2 && Notes == "小")
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 51)//后三单双
                    {
                        int zj_zs = 0;
                        if (Housan(2, Result) == 1 && Notes == "单")
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                        if (Housan(2, Result) == 2 && Notes == "双")
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 52)//后三豹子
                    {
                        int zj_zs = 0;
                        if (Housan(3, Result) == 1)
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 53)//后三顺子
                    {
                        int zj_zs = 0;
                        if (Housan(4, Result) == 1)
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 54)//后三对子
                    {
                        int zj_zs = 0;
                        if (Housan(5, Result) == 1)
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 55)//后三半顺
                    {
                        int zj_zs = 0;
                        if (Housan(6, Result) == 1)
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 56)//后三杂六
                    {
                        int zj_zs = 0;
                        if (Housan(7, Result) != 1)
                        {
                            zj_zs = 1;
                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    #endregion

                    //更新已开奖
                    new BCW.ssc.BLL.SSCpay().UpdateState(ID, 1);
                }
            }

            Utils.Success("第" + model.SSCId + "期开奖", "第" + model.SSCId + "期开奖成功..", Utils.getUrl("ssc.aspx"), "2");
        }
        else if (info == "ok1")
        {
            string rResult = Utils.GetRequest("rResult", "all", 2, @"^\d\s\d\s\d\s\d\s\d$", "中奖号码填写错误");
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("第" + model.SSCId + "期开奖确认<br />");
            builder.Append("开奖号码：<b style=\"color:red\">" + rResult + "</b>");
            builder.Append(Out.Tab("</div>", ""));

            string strText = ",,,,";
            string strName = "rResult,id,act,info,backurl";
            string strType = "hidden,hidden,hidden,hidden,hidden";
            string strValu = "" + rResult + "'" + id + "'open'ok'" + Utils.getPage(0) + "";
            string strEmpt = "false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定开奖,ssc.aspx,post,1,red";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("ssc.aspx") + "\">返回上一级</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("第" + model.SSCId + "期开奖");
            builder.Append(Out.Tab("</div>", ""));

            if (model.EndTime < DateTime.Now)
            {
                string strText = "开奖号码(用空格分开，如1 2 3 4 5):/,,,,";
                string strName = "rResult,id,act,info,backurl";
                string strType = "text,hidden,hidden,hidden,hidden";
                string strValu = "'" + id + "'open'ok1'" + Utils.getPage(0) + "";
                string strEmpt = "false,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确定开奖,ssc.aspx,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
            else
            {
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("当前期数正在投注，请于投注结束再进行开奖操作！");
                builder.Append(Out.Tab("</div>", ""));
            }

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("ssc.aspx") + "\">返回上一级</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }

    //重开奖
    private void OpentwoPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));

        BCW.ssc.Model.SSClist model = new BCW.ssc.BLL.SSClist().GetSSClist(id);

        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.State == 1)
        {
            //  Utils.Error("该期已开奖", "");
        }
        if (model.EndTime > DateTime.Now)
        {
            Utils.Error("该期未到开奖时间", "");
        }

        Master.Title = "" + GameName + "第" + model.SSCId + "重开奖";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx") + "\">" + GameName + "</a>");
        builder.Append("&gt;第" + model.SSCId + "期重开奖");
        builder.Append(Out.Tab("</div>", "<br />"));

        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {
            string rResult = Utils.GetRequest("rResult", "all", 2, @"^\d\s\d\s\d\s\d\s\d$", "中奖号码填写错误");

            #region 退回上次开奖

            //开始返退
            DataSet ds1 = new BCW.ssc.BLL.SSCpay().GetList("ID,Types,SSCId,UsID,UsName,Price,Notes,Result,State,Odds,WinCent,IsSpier", " SSCId=" + model.SSCId + "");
            if (ds1 != null && ds1.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    int ID = int.Parse(ds1.Tables[0].Rows[i]["ID"].ToString());
                    int Types = int.Parse(ds1.Tables[0].Rows[i]["Types"].ToString());
                    int SSCId = int.Parse(ds1.Tables[0].Rows[i]["SSCId"].ToString());
                    string Notes = ds1.Tables[0].Rows[i]["Notes"].ToString();
                    int UsID = int.Parse(ds1.Tables[0].Rows[i]["UsID"].ToString());
                    string UsName = ds1.Tables[0].Rows[i]["UsName"].ToString();
                    string Result = ds1.Tables[0].Rows[i]["Result"].ToString();
                    long Price = Int64.Parse(ds1.Tables[0].Rows[i]["Price"].ToString());
                    decimal Odds = decimal.Parse(ds1.Tables[0].Rows[i]["Odds"].ToString()); //赔率
                    int State = int.Parse(ds1.Tables[0].Rows[i]["State"].ToString());
                    long WinCent = Int64.Parse(ds1.Tables[0].Rows[i]["WinCent"].ToString());
                    int IsSpier = int.Parse(ds1.Tables[0].Rows[i]["IsSpier"].ToString());

                    // int SSCid = model.SSCId;
                    // BCW.ssc.Model.SSCpay n = new BCW.ssc.BLL.SSCpay().GetSSCpay(ID);
                    if (!new BCW.ssc.BLL.SSCpay().Exists(ID))
                    {
                        Utils.Error("不存在的记录", "");
                    }
                    else
                    {
                        //根据id查询-购买表
                        int meid = UsID;//用户名
                        string mename = new BCW.BLL.User().GetUsName(meid);//获得id对应的用户名
                        int state_get = State;//用户购买情况

                        //如果未开奖，退回本金
                        if (state_get < 2)
                        {
                            //new BCW.BLL.User().UpdateiGold(meid, n.Prices, "系统退回" + GameName + "第" + n.SSCId + "期下注的" + n.Prices + "" + ub.Get("SiteBz") + "！");//减少系统总的酷币
                            //new BCW.BLL.Guest().Add(1, meid, mename, "系统退回您的" + GameName + "：第" + n.SSCId + "期下注的" + n.Prices + "" + ub.Get("SiteBz") + "！");
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
                            gold = new BCW.BLL.User().GetGold(UsID);//个人酷币
                            if (WinCent > gold)
                            {
                                cMoney = WinCent - gold;
                                sMoney = WinCent;
                            }
                            else
                            {
                                sMoney = WinCent;
                            }

                            //如果币不够扣则记录日志并冻结IsFreeze
                            if (cMoney > 0)
                            {
                                BCW.Model.Gameowe owe = new BCW.Model.Gameowe();
                                owe.Types = 1;
                                owe.UsID = UsID;
                                owe.UsName = mename;
                                owe.Content = "" + GameName + SSCId + "期" + OutType(Types) + "下注标识ID" + ID + ".系统管理员" + new BCW.User.Manage().IsManageLogin() + "重开奖.";
                                owe.OweCent = cMoney;
                                owe.BzType = 10;
                                owe.EnId = ID;
                                owe.AddTime = DateTime.Now;
                                new BCW.BLL.Gameowe().Add(owe);
                                new BCW.BLL.User().UpdateIsFreeze(UsID, 1);
                                ui = "实扣" + sMoney + ",还差" + (cMoney) + ",系统已自动将您帐户冻结.";
                            }
                            string oop = string.Empty;
                            if (WinCent > 0)
                            {
                                oop = "并扣除所得的" + WinCent + "。";
                            }
                            new BCW.BLL.User().UpdateiGold(UsID, -WinCent, "重开奖操作，系统扣回" + GameName + "第" + SSCId + "期上次开奖赢的" + WinCent + "" + ub.Get("SiteBz") + "." + oop + "" + ui + "标识ID" + ID);//减少系统总的酷币
                            if (ub.GetSub("SSCGuestSet", "/Controls/ssc.xml") == "0" && new BCW.ssc.BLL.SSCpay().GetIsSpier(ID) != 1)
                            {
                                new BCW.BLL.Guest().Add(1, meid, mename, "重开奖操作，系统扣回" + GameName + "第" + SSCId + "期上次开奖赢的" + WinCent + "" + ub.Get("SiteBz") + "." + oop + "" + ui);
                            }
                            if (IsSpier != 1 && new BCW.BLL.User().GetIsSpier(UsID) != 1) //
                            {
                                new BCW.BLL.User().UpdateiGold(108, new BCW.BLL.User().GetUsName(108), WinCent, "ID:[url=forumlog.aspx?act=xview&amp;uid=" + UsID + "]" + UsID + "[/url]" + GameName + "第[url=./game/ssc.aspx?act=view&amp;id=" + model.ID + "]" + model.SSCId + "[/url]期重开奖收回上次开奖赢取" + WinCent + "-标识ID" + ID + "");
                            }
                        }

                        new BCW.ssc.BLL.SSCpay().UpdateResult1(model.SSCId, "");//重置开奖结果Result
                        new BCW.ssc.BLL.SSCpay().UpdateState1(ID, 0);//重置开奖状态State
                        new BCW.ssc.BLL.SSCpay().UpdateWincent(ID, 0);//重置开奖赢WinCent
                        new BCW.ssc.BLL.SSCpay().UpdateWinNotes(ID, "");//重置开奖注数WinNotes

                        new BCW.ssc.BLL.SSClist().UpdateResult1(model.SSCId, rResult);
                    }
                }
            }


            #endregion

            #region 重开奖
            //new BCW.ssc.BLL.SSClist().UpdateResult(model.SSCId, rResult);
            new BCW.ssc.BLL.SSCpay().UpdateResult(model.SSCId, rResult);
            //更新开奖类型时间
            string StateTime = string.Empty;
            try
            {
                string staet = new BCW.ssc.BLL.SSClist().GetStateTime(model.SSCId);
                string[] statime1 = staet.Split('#');
                string statime2 = statime1[statime1.Length - 1];
                string[] statime3 = statime2.Split('|');
                int x = Convert.ToInt32(statime3[0]);
                if (x == 0) x = 1;
                if (staet == " " || staet == null || staet == "")
                {
                    StateTime = "" + (x + 1) + "|" + DateTime.Now + "|" + rResult;//重开奖
                }
                else
                {
                    StateTime = staet + "#" + "" + (x + 1) + "|" + DateTime.Now + "|" + rResult;
                }
            }
            catch { StateTime = "2|" + DateTime.Now + "|" + rResult; }
            new BCW.ssc.BLL.SSClist().UpdateStateTime(model.SSCId, StateTime);//0自动开奖|time0时间#1|time1手动开奖#2|time2重开奖.....

            //开始返彩
            DataSet ds = new BCW.ssc.BLL.SSCpay().GetList("ID,Types,SSCId,UsID,UsName,Price,Notes,Result,Odds", "State=0 and Result<>''");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    int ID = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                    int Types = int.Parse(ds.Tables[0].Rows[i]["Types"].ToString());
                    int SSCId = int.Parse(ds.Tables[0].Rows[i]["SSCId"].ToString());
                    string Notes = ds.Tables[0].Rows[i]["Notes"].ToString();
                    int UsID = int.Parse(ds.Tables[0].Rows[i]["UsID"].ToString());
                    string UsName = ds.Tables[0].Rows[i]["UsName"].ToString();
                    string Result = ds.Tables[0].Rows[i]["Result"].ToString();
                    long Price = Int64.Parse(ds.Tables[0].Rows[i]["Price"].ToString());
                    decimal Odds = decimal.Parse(ds.Tables[0].Rows[i]["Odds"].ToString()); //赔率

                    #region 返奖
                    if (Types == 18)//任一 
                    {
                        string[] iNum_kj = Result.Split(' ');  //该期开奖的结果
                        string[] iNum_zj = Notes.Split(',');  //该期该用户购买的彩票

                        int zj_zs = 0; //统计中奖注数

                        if (BackLikeNum(Result) == 5)//开奖结果五个结果不同
                        {
                            for (int fs = 0; fs < iNum_zj.Length; fs++)
                                for (int p = 0; p < iNum_kj.Length; p++)
                                    if (string.Compare(iNum_zj[fs], iNum_kj[p]) == 0)
                                        zj_zs += 1;
                        }
                        else//开奖结果有相同的，把相同的号码合并为一个号码，再比对开奖
                        {
                            string ret = string.Empty;
                            ArrayList list = new ArrayList();
                            for (int j = 0; j < iNum_kj.Length; j++)
                            {
                                if (!list.Contains(iNum_kj[j]))
                                {
                                    list.Add(iNum_kj[j]);
                                }
                            }
                            for (int k = 0; k < list.Count; k++)
                            {
                                ret += list[k] + " ";
                            }

                            string[] kj = ret.Split(' ');
                            for (int fs = 0; fs < iNum_zj.Length; fs++)
                                for (int p = 0; p < kj.Length; p++)
                                    if (string.Compare(iNum_zj[fs], kj[p]) == 0)
                                        zj_zs += 1;

                        }

                        if (zj_zs > 0)//中奖
                        {

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 16)//任选号码
                    {
                        string[] iNum_kj = Result.Split(' ');  //该期开奖的结果
                        string[] iNum_zj = Notes.Split(',');  //该期该用户购买的彩票

                        int zj_zs = 0; //统计中奖注数

                        for (int fs = 0; fs < iNum_zj.Length; fs++)
                            for (int p = 0; p < iNum_kj.Length; p++)
                                if (string.Compare(iNum_zj[fs], iNum_kj[p]) == 0)
                                    zj_zs += 1;
                        if (zj_zs > 0)//中奖
                        {

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖倍数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 1 || Types == 4 || Types == 7 || Types == 10 || Types == 13)// 万、千、百、十、个位直选
                    {
                        string[] iNum_kj = Result.Split(' ');  //该期开奖的结果
                        string[] iNum_zj = Notes.Split(',');  //该期该用户购买的彩票

                        int zj_zs = 0; //统计中奖注数

                        int p = 0;
                        if (Types == 1)//万位 
                        {
                            for (int fs = 0; fs < iNum_zj.Length; fs++)
                            {
                                if (string.Compare(iNum_zj[fs], iNum_kj[0]) == 0)
                                    zj_zs += 1;
                            }
                        }
                        else if (Types == 4)//千位 
                        {
                            for (int fs = 0; fs < iNum_zj.Length; fs++)
                            {
                                if (string.Compare(iNum_zj[fs], iNum_kj[1]) == 0)
                                    zj_zs += 1;
                            }
                        }
                        else if (Types == 7)//百位
                        {
                            for (int fs = 0; fs < iNum_zj.Length; fs++)
                            {
                                if (string.Compare(iNum_zj[fs], iNum_kj[2]) == 0)
                                    zj_zs += 1;
                            }
                        }
                        else if (Types == 10)//十位
                        {
                            for (int fs = 0; fs < iNum_zj.Length; fs++)
                            {
                                if (string.Compare(iNum_zj[fs], iNum_kj[3]) == 0)
                                    zj_zs += 1;
                            }
                        }
                        else if (Types == 13)//个位 
                        {
                            for (int fs = 0; fs < iNum_zj.Length; fs++)
                            {
                                if (string.Compare(iNum_zj[fs], iNum_kj[4]) == 0)
                                    zj_zs += 1;
                            }
                        }
                        if (zj_zs > 0)//中奖
                        {

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 19 || Types == 20 || Types == 21 || Types == 22)//任二、三、四、五
                    {
                        string[] iNum_kj = Result.Split(' ');  //存放结果
                        string[] iNum_zj = Notes.Split('#');  //把胆码和拖码分开存储
                        string[] iNum_zj_d = iNum_zj[0].Split(','); //胆码
                        string[] iNum_zj_t = iNum_zj[1].Split(',');  //拖码

                        if (iNum_zj[0] == "")//无胆码，为普通选
                        {
                            int zj_zs = 0; //统计中奖注数

                            if (BackLikeNum(Result) == 5)//开奖的五个号不同
                            {
                                for (int fs = 0; fs < iNum_zj_t.Length; fs++)
                                    for (int p = 0; p < iNum_kj.Length; p++)
                                        if (string.Compare(iNum_zj_t[fs], iNum_kj[p]) == 0)
                                            zj_zs += 1;
                            }
                            else//开奖号有相同的
                            {
                                string ret = string.Empty;
                                ArrayList list = new ArrayList();
                                for (int j = 0; j < iNum_kj.Length; j++)
                                {
                                    if (!list.Contains(iNum_kj[j]))
                                    {
                                        list.Add(iNum_kj[j]);
                                    }
                                }
                                for (int k = 0; k < list.Count; k++)
                                {
                                    ret += list[k] + " ";
                                }

                                string[] kj = ret.Split(' ');
                                for (int fs = 0; fs < iNum_zj_t.Length; fs++)
                                    for (int p = 0; p < kj.Length; p++)
                                        if (string.Compare(iNum_zj_t[fs], kj[p]) == 0)
                                            zj_zs += 1;
                            }


                            switch (Types)
                            {
                                case 22: //任选五普通
                                    {
                                        if (zj_zs >= 5 && BackLikeNum(Result) == 5)
                                        {
                                            zj_zs = C(zj_zs, 5);
                                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                                            string WinNotes = "中奖注数:" + zj_zs;
                                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                                        }
                                        else
                                            zj_zs = 0;
                                    }
                                    break;
                                case 21: //任选四普通
                                    {
                                        if (zj_zs >= 4 && BackLikeNum(Result) >= 4)
                                        {
                                            if (BackLikeNum(Result) == 5)
                                            {
                                                zj_zs = C(zj_zs, 4);
                                            }
                                            if (BackLikeNum(Result) == 4)
                                            {
                                                zj_zs = 1;
                                            }

                                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                                            string WinNotes = "中奖注数:" + zj_zs;
                                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                                        }
                                        else
                                            zj_zs = 0;
                                    }
                                    break;
                                case 20: //任选三普通
                                    {
                                        if (zj_zs >= 3 && BackLikeNum(Result) >= 3)
                                        {
                                            if (BackLikeNum(Result) == 5)
                                            {
                                                zj_zs = C(zj_zs, 3);
                                            }
                                            if (BackLikeNum(Result) == 4)
                                            {
                                                zj_zs = C(zj_zs, 3);
                                            }
                                            if (BackLikeNum(Result) == 3)
                                            {
                                                zj_zs = 1;
                                            }

                                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                                            string WinNotes = "中奖注数:" + zj_zs;
                                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                                        }
                                        else
                                            zj_zs = 0;
                                    }
                                    break;
                                default: //任选二普通
                                    {
                                        if (zj_zs >= 2 && BackLikeNum(Result) >= 2)
                                        {
                                            if (BackLikeNum(Result) == 5)
                                            {
                                                zj_zs = C(zj_zs, 2);
                                            }
                                            if (BackLikeNum(Result) == 4)
                                            {
                                                zj_zs = C(zj_zs, 2);
                                            }
                                            if (BackLikeNum(Result) == 3)
                                            {
                                                zj_zs = C(zj_zs, 2);
                                            }
                                            if (BackLikeNum(Result) == 2)
                                            {
                                                zj_zs = 1;
                                            }

                                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                                            string WinNotes = "中奖注数:" + zj_zs;
                                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                                        }
                                        else
                                            zj_zs = 0;
                                    }
                                    break;
                            }
                        }
                        else//有胆码，为胆拖
                        {
                            int zj_ds = 0; //中奖的胆码数
                            int zj_ts = 0; //中奖的拖码数
                            int zj = 0; //中奖的注数

                            if (BackLikeNum(Result) == 5)//开奖的五个号不同
                            {
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
                            }
                            else//开奖号有相同的
                            {
                                string ret = string.Empty;
                                ArrayList list = new ArrayList();
                                for (int j = 0; j < iNum_kj.Length; j++)
                                {
                                    if (!list.Contains(iNum_kj[j]))
                                    {
                                        list.Add(iNum_kj[j]);
                                    }
                                }
                                for (int k = 0; k < list.Count; k++)
                                {
                                    ret += list[k] + " ";
                                }

                                string[] kj = ret.Split(' ');
                                for (int fs = 0; fs < iNum_zj_d.Length; fs++)
                                    for (int p = 0; p < kj.Length; p++)
                                        if (string.Compare(iNum_zj_d[fs], kj[p]) == 0)
                                            zj_ds += 1;
                            }
                            if (zj_ds == iNum_zj_d.Length) //判断胆码是否全中
                            {
                                if (BackLikeNum(Result) == 5)
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
                                }
                                else//开奖结果有相同的，把相同的号码合并为一个号码，再比对开奖
                                {
                                    string ret = string.Empty;
                                    ArrayList list = new ArrayList();
                                    for (int j = 0; j < iNum_kj.Length; j++)
                                    {
                                        if (!list.Contains(iNum_kj[j]))
                                        {
                                            list.Add(iNum_kj[j]);
                                        }
                                    }
                                    for (int k = 0; k < list.Count; k++)
                                    {
                                        ret += list[k] + " ";
                                    }

                                    string[] kj = ret.Split(' ');
                                    for (int fs = 0; fs < iNum_zj_t.Length; fs++)
                                        for (int p = 0; p < kj.Length; p++)
                                            if (string.Compare(iNum_zj_t[fs], kj[p]) == 0)
                                                zj_ts += 1;

                                }

                                switch (Types)
                                {
                                    case 22: //五胆拖类型
                                        {
                                            if (zj_ds + zj_ts >= 5)
                                            {
                                                zj = C(zj_ts, 5 - iNum_zj_d.Length);

                                                long WinCent = Convert.ToInt64(Price * Odds * zj);
                                                string WinNotes = "中奖注数:" + zj;
                                                new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                                            }
                                        }
                                        break;
                                    case 21: //四胆拖类型
                                        {
                                            if (zj_ds + zj_ts >= 4)
                                            {
                                                zj = C(zj_ts, 4 - iNum_zj_d.Length);

                                                long WinCent = Convert.ToInt64(Price * Odds * zj);
                                                string WinNotes = "中奖注数:" + zj;
                                                new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                                            }
                                        }
                                        break;
                                    case 20: //三胆拖类型
                                        {
                                            if (zj_ds + zj_ts >= 3)
                                            {
                                                zj = C(zj_ts, 3 - iNum_zj_d.Length);

                                                long WinCent = Convert.ToInt64(Price * Odds * zj);
                                                string WinNotes = "中奖注数:" + zj;
                                                new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                                            }
                                        }
                                        break;
                                    default: //二胆拖类型
                                        {
                                            if (zj_ds + zj_ts >= 2)
                                            {
                                                zj = C(zj_ts, 2 - iNum_zj_d.Length);

                                                long WinCent = Convert.ToInt64(Price * Odds * zj);
                                                string WinNotes = "中奖注数:" + zj;
                                                new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                                            }
                                        }
                                        break;
                                }
                            }
                            else  //胆码没有全中则没有中奖
                            {
                                zj = 0;
                            }
                        }
                    }
                    else if (Types == 25)//总和大小
                    {
                        string[] iNum_kj = Result.Split(' ');
                        string zj = Notes;
                        int sum = 0;
                        int zj_zs = 0;

                        for (int j = 0; j < iNum_kj.Length; j++)
                        {
                            sum += Convert.ToInt32(iNum_kj[j]);
                        }
                        string temp = Convert.ToString(sum);
                        sum = Convert.ToInt32(temp);
                        if (sum >= 23 && zj == "大")
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                        else if (sum <= 22 && zj == "小")
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 26)//总和单双
                    {
                        string[] iNum_kj = Result.Split(' ');
                        string zj = Notes;
                        int sum = 0;
                        int zj_zs = 0;

                        for (int j = 0; j < iNum_kj.Length; j++)
                        {
                            sum += Convert.ToInt32(iNum_kj[j]);
                        }
                        string temp = Convert.ToString(sum);
                        temp = temp.Substring(temp.Length - 1, 1);
                        sum = Convert.ToInt32(temp);
                        if (sum % 2 != 0 && zj == "单")
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                        else if (sum % 2 == 0 && zj == "双")
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 2 || Types == 5 || Types == 8 || Types == 11 || Types == 14)//万千百十个大小
                    {
                        string[] iNum_kj = Result.Split(' ');
                        string zj = Notes;
                        int sum = 0;
                        int zj_zs = 0;
                        int j = 0;
                        if (Types == 2) { sum = Convert.ToInt32(iNum_kj[0]); }
                        if (Types == 5) { sum = Convert.ToInt32(iNum_kj[1]); }
                        if (Types == 8) { sum = Convert.ToInt32(iNum_kj[2]); }
                        if (Types == 11) { sum = Convert.ToInt32(iNum_kj[3]); }
                        if (Types == 14) { sum = Convert.ToInt32(iNum_kj[4]); }

                        if (sum >= 5 && zj == "大")
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                        else if (sum <= 4 && zj == "小")
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 3 || Types == 6 || Types == 9 || Types == 12 || Types == 15)//万千百十个单双
                    {
                        string[] iNum_kj = Result.Split(' ');
                        string zj = Notes;
                        int sum = 0;
                        int zj_zs = 0;
                        int j = 0;
                        if (Types == 3) { j = 0; }
                        if (Types == 6) { j = 1; }
                        if (Types == 9) { j = 2; }
                        if (Types == 12) { j = 3; }
                        if (Types == 15) { j = 4; }
                        sum = Convert.ToInt32(iNum_kj[j]);
                        if (sum % 2 != 0 && zj == "单")
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                        else if (sum % 2 == 0 && zj == "双")
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 17)//龙虎和
                    {
                        string[] iNum_kj = Result.Split(' ');
                        string zj = Notes;
                        int zj_zs = 0;

                        if (Convert.ToInt32(iNum_kj[0]) > Convert.ToInt32(iNum_kj[4]) && zj == "龙")
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                        if (Convert.ToInt32(iNum_kj[0]) < Convert.ToInt32(iNum_kj[4]) && zj == "虎")
                        {
                            zj_zs = 1;
                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                        if (Convert.ToInt32(iNum_kj[0]) == Convert.ToInt32(iNum_kj[4]) && zj == "和")
                        {
                            zj_zs = 1;
                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 23)//有牛无牛
                    {
                        string result = string.Empty;
                        if (Niu(Result) == "")
                        {
                            result = "无牛";
                        }
                        else
                        {
                            result = "有牛";
                        }
                        int zj_zs = 0;
                        if (result == Notes)
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 24)//特定牛牛
                    {
                        string a = Niu(Result);
                        int c = 0;
                        if (a != "")
                        {
                            c = Convert.ToInt32(a.Substring(a.Length - 1, 1));

                            string[] iNum_zj = Notes.Split(',');  //该期该用户购买的彩票

                            int zj_zs = 0; //统计中奖注数

                            for (int fs = 0; fs < iNum_zj.Length; fs++)
                                if (string.Compare(iNum_zj[fs], c.ToString()) == 0)
                                    zj_zs += 1;

                            if (zj_zs > 0)
                            {
                                long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                                string WinNotes = "中奖号：牛" + c + "|中奖注数:" + zj_zs;
                                new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                            }
                        }
                    }
                    else if (Types == 27)//总和五门
                    {
                        string[] iNum_kj = Result.Split(' ');
                        string zj = Notes;
                        int sum = 0;
                        int zj_zs = 0;

                        for (int j = 0; j < iNum_kj.Length; j++)
                        {
                            sum += Convert.ToInt32(iNum_kj[j]);
                        }
                        string temp = Convert.ToString(sum);
                        sum = Convert.ToInt32(temp);
                        if (sum <= 9 && zj == "一门")
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                        else if (sum > 9 && sum <= 19 && zj == "二门")
                        {
                            zj_zs = 1;
                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                        else if (sum > 19 && sum <= 29 && zj == "三门")
                        {
                            zj_zs = 1;
                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                        else if (sum > 29 && sum <= 39 && zj == "四门")
                        {
                            zj_zs = 1;
                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                        else if (sum > 39 && sum <= 45 && zj == "五门")
                        {
                            zj_zs = 1;
                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 28)//总和大小单双
                    {
                        string[] iNum_kj = Result.Split(' ');
                        string zj = Notes;
                        int sum = 0;
                        int zj_zs = 0;

                        for (int j = 0; j < iNum_kj.Length; j++)
                        {
                            sum += Convert.ToInt32(iNum_kj[j]);
                        }
                        string temp = Convert.ToString(sum);
                        sum = Convert.ToInt32(temp);
                        if (sum < 23)//小
                        {
                            if (sum % 2 == 0 && zj == "小双")
                            {
                                zj_zs = 1;

                                long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                                string WinNotes = "中奖注数:" + zj_zs;
                                new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                            }
                            if (sum % 2 != 0 && zj == "小单")
                            {
                                zj_zs = 1;
                                long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                                string WinNotes = "中奖注数:" + zj_zs;
                                new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                            }
                        }
                        else//大
                        {
                            if (sum % 2 == 0 && zj == "大双")
                            {
                                zj_zs = 1;
                                long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                                string WinNotes = "中奖注数:" + zj_zs;
                                new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                            }
                            if (sum % 2 != 0 && zj == "大单")
                            {
                                zj_zs = 1;

                                long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                                string WinNotes = "中奖注数:" + zj_zs;
                                new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                            }
                        }
                    }
                    else if (Types == 29)//梭哈炸弹
                    {
                        int zj_zs = 0;
                        if (Zhadan(Result) >= 4)
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 30)//梭哈葫芦
                    {
                        int zj_zs = 0;
                        if (HuLu(Result) == 1)
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 31)//梭哈顺子
                    {
                        int zj_zs = 0;
                        if (SHShunzi(Result) == 1)
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 32)//梭哈三条
                    {
                        int zj_zs = 0;
                        if (SHSantiao(Result) == 1)
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 33)//梭哈两对
                    {
                        int zj_zs = 0;
                        if (SHLiangdui(Result) == 1)
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 34)//梭哈单对
                    {
                        int zj_zs = 0;
                        if (SHDandui(Result) == 1)
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 35)//梭哈散牌
                    {
                        int zj_zs = 0;
                        if (SHSanpai(Result) == 1)
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 36)//前三大小
                    {
                        int zj_zs = 0;
                        if (Qiansan(1, Result) == 1 && Notes == "大")
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                        if (Qiansan(1, Result) == 2 && Notes == "小")
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 37)//前三单双
                    {
                        int zj_zs = 0;
                        if (Qiansan(2, Result) == 1 && Notes == "单")
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                        if (Qiansan(2, Result) == 2 && Notes == "双")
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 38)//前三豹子
                    {
                        int zj_zs = 0;
                        if (Qiansan(3, Result) == 1)
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 39)//前三顺子
                    {
                        int zj_zs = 0;
                        if (Qiansan(4, Result) == 1)
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 40)//前三对子
                    {
                        int zj_zs = 0;
                        if (Qiansan(5, Result) == 1)
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 41)//前三半顺
                    {
                        int zj_zs = 0;
                        if (Qiansan(6, Result) == 1)
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 42)//前三杂六
                    {
                        int zj_zs = 0;
                        if (Qiansan(7, Result) != 1)
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 43)//中三大小
                    {
                        int zj_zs = 0;
                        if (Zhongsan(1, Result) == 1 && Notes == "大")
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                        if (Zhongsan(1, Result) == 2 && Notes == "小")
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 44)//中三单双
                    {
                        int zj_zs = 0;
                        if (Zhongsan(2, Result) == 1 && Notes == "单")
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                        if (Zhongsan(2, Result) == 2 && Notes == "双")
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 45)//中三豹子
                    {
                        int zj_zs = 0;
                        if (Zhongsan(3, Result) == 1)
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 46)//中三顺子
                    {
                        int zj_zs = 0;
                        if (Zhongsan(4, Result) == 1)
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 47)//中三对子
                    {
                        int zj_zs = 0;
                        if (Zhongsan(5, Result) == 1)
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 48)//中三半顺
                    {
                        int zj_zs = 0;
                        if (Zhongsan(6, Result) == 1)
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 49)//中三杂六
                    {
                        int zj_zs = 0;
                        if (Zhongsan(7, Result) != 1)
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }

                    else if (Types == 50)//后三大小
                    {
                        int zj_zs = 0;
                        if (Housan(1, Result) == 1 && Notes == "大")
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                        if (Housan(1, Result) == 2 && Notes == "小")
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 51)//后三单双
                    {
                        int zj_zs = 0;
                        if (Housan(2, Result) == 1 && Notes == "单")
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                        if (Housan(2, Result) == 2 && Notes == "双")
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 52)//后三豹子
                    {
                        int zj_zs = 0;
                        if (Housan(3, Result) == 1)
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 53)//后三顺子
                    {
                        int zj_zs = 0;
                        if (Housan(4, Result) == 1)
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 54)//后三对子
                    {
                        int zj_zs = 0;
                        if (Housan(5, Result) == 1)
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 55)//后三半顺
                    {
                        int zj_zs = 0;
                        if (Housan(6, Result) == 1)
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    else if (Types == 56)//后三杂六
                    {
                        int zj_zs = 0;
                        if (Housan(7, Result) != 1)
                        {
                            zj_zs = 1;

                            long WinCent = Convert.ToInt64(Price * Odds * zj_zs);
                            string WinNotes = "中奖注数:" + zj_zs;
                            new BCW.ssc.BLL.SSCpay().UpdateWinCent(ID, SSCId, UsID, UsName, Types, WinCent, WinNotes);
                        }
                    }
                    #endregion

                    //更新已开奖
                    new BCW.ssc.BLL.SSCpay().UpdateState(ID, 1);
                }
            }
            #endregion

            Utils.Success("第" + model.SSCId + "期开奖", "第" + model.SSCId + "期重开奖成功..", Utils.getUrl("ssc.aspx"), "2");

        }
        else if (info == "ok1")
        {
            string rResult = Utils.GetRequest("rResult", "all", 2, @"^\d\s\d\s\d\s\d\s\d$", "中奖号码填写错误");
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("第" + model.SSCId + "期重开奖确认<br />");
            builder.Append("开奖号码：<b style=\"color:red\">" + rResult + "</b>");
            builder.Append(Out.Tab("</div>", ""));

            string strText = ",,,,";
            string strName = "rResult,id,act,info,backurl";
            string strType = "hidden,hidden,hidden,hidden,hidden";
            string strValu = "" + rResult + "'" + id + "'opentwo'ok'" + Utils.getPage(0) + "";
            string strEmpt = "false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定开奖,ssc.aspx,post,1,red";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<b style=\"color:red\">注意：重开奖操作会收回上次开奖所有会员赢取的" + ub.Get("SiteBz") + "，然后再进行新的开奖号码的开奖。请管理员注意，刷新机开奖程序运行，且这一期是抓取到开奖号码时最好不要重开奖</b><br />");
            builder.Append("<a href=\"" + Utils.getPage("ssc.aspx") + "\">返回上一级</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("第" + model.SSCId + "期重开奖");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "中奖号码(用空格分开，如1 2 3 4 5):/,,,,";
            string strName = "rResult,id,act,info,backurl";
            string strType = "text,hidden,hidden,hidden,hidden";
            string strValu = "'" + id + "'opentwo'ok1'" + Utils.getPage(0) + "";
            string strEmpt = "false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定开奖,ssc.aspx,post,1,red";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<b style=\"color:red\">注意：重开奖操作会收回上次开奖所有会员赢取的" + ub.Get("SiteBz") + "，然后再进行新的开奖号码的开奖。请管理员注意，刷新机开奖程序运行，且这一期是抓取到开奖号码时最好不要重开奖</b><br />");
            builder.Append("<a href=\"" + Utils.getPage("ssc.aspx") + "\">返回上一级</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }

    #region 前三
    private int Qiansan(int i, string Result)
    {
        string[] iNum_kj = Result.Split(' ');
        int zj_zs = 0;

        if (i == 1)//大小
        {
            int sum = 0;
            sum = Convert.ToInt32(iNum_kj[0]) + Convert.ToInt32(iNum_kj[1]) + Convert.ToInt32(iNum_kj[2]);
            if (sum > 13 && sum < 28)//大
            {
                zj_zs = 1;
            }
            if (sum < 14)//小
            {
                zj_zs = 2;
            }
        }
        if (i == 2)//单双
        {
            int sum = 0;
            sum = Convert.ToInt32(iNum_kj[0]) + Convert.ToInt32(iNum_kj[1]) + Convert.ToInt32(iNum_kj[2]);
            if (sum % 2 != 0)//单
            {
                zj_zs = 1;
            }
            if (sum % 2 == 0)//双
            {
                zj_zs = 2;
            }
        }
        if (i == 3)//豹子
        {
            if (Convert.ToInt32(iNum_kj[0]) == Convert.ToInt32(iNum_kj[1]) && Convert.ToInt32(iNum_kj[1]) == Convert.ToInt32(iNum_kj[2]))
            {
                zj_zs = 1;
            }
        }
        if (i == 4)//顺子
        {
            int n1 = Convert.ToInt32(iNum_kj[0]); if (n1 == 0) n1 = 10;
            int n2 = Convert.ToInt32(iNum_kj[1]); if (n2 == 0) n2 = 10;
            int n3 = Convert.ToInt32(iNum_kj[2]); if (n3 == 0) n3 = 10;
            int a = n1 - n2;
            int b = n1 - n3;
            int c = n2 - n3;

            int d = 0;
            if (n1 != n2 && n2 != n3 && n1 != n3)
            {
                d = Math.Abs(a) + Math.Abs(b) + Math.Abs(c);
            }

            if (d == 4)
            {
                zj_zs = 1;
            }

            if (n1 == 10 && n2 == 1 && n3 == 2) zj_zs = 1;//012
            if (n1 == 10 && n2 == 2 && n3 == 1) zj_zs = 1;
            if (n1 == 1 && n2 == 10 && n3 == 2) zj_zs = 1;
            if (n1 == 1 && n2 == 2 && n3 == 10) zj_zs = 1;
            if (n1 == 2 && n2 == 1 && n3 == 10) zj_zs = 1;
            if (n1 == 2 && n2 == 10 && n3 == 1) zj_zs = 1;

            if (n1 == 10 && n2 == 1 && n3 == 9) zj_zs = 1;//901
            if (n1 == 10 && n2 == 9 && n3 == 1) zj_zs = 1;
            if (n1 == 1 && n2 == 10 && n3 == 9) zj_zs = 1;
            if (n1 == 1 && n2 == 9 && n3 == 10) zj_zs = 1;
            if (n1 == 9 && n2 == 1 && n3 == 10) zj_zs = 1;
            if (n1 == 9 && n2 == 10 && n3 == 1) zj_zs = 1;
        }
        if (i == 5)//对子
        {
            int n1 = Convert.ToInt32(iNum_kj[0]);
            int n2 = Convert.ToInt32(iNum_kj[1]);
            int n3 = Convert.ToInt32(iNum_kj[2]);

            if (n1 == n2 && n2 != n3)
            {
                zj_zs = 1;
            }
            if (n1 == n3 && n3 != n2)
            {
                zj_zs = 1;
            }
            if (n2 == n3 && n3 != n1)
            {
                zj_zs = 1;
            }
        }
        if (i == 6)//半顺
        {
            int n1 = Convert.ToInt32(iNum_kj[0]); if (n1 == 0) n1 = 10;
            int n2 = Convert.ToInt32(iNum_kj[1]); if (n2 == 0) n2 = 10;
            int n3 = Convert.ToInt32(iNum_kj[2]); if (n3 == 0) n3 = 10;

            int a = n1 - n2;
            int b = n1 - n3;
            int c = n2 - n3;

            if (Math.Abs(a) == 1 && Math.Abs(b) > 1 && Math.Abs(c) > 1)
            {
                zj_zs = 1;
            }
            if (Math.Abs(b) == 1 && Math.Abs(a) > 1 && Math.Abs(c) > 1)
            {
                zj_zs = 1;
            }
            if (Math.Abs(c) == 1 && Math.Abs(b) > 1 && Math.Abs(a) > 1)
            {
                zj_zs = 1;
            }
            string n123 = iNum_kj[0] + iNum_kj[1] + iNum_kj[2];
            {
                if (n1 != n2 && n2 != n3 && n1 != n3)
                {
                    if (n123.Contains("0") && n123.Contains("1") && !n123.Contains("2"))
                        zj_zs = 1;
                }
            }
            if (Qiansan(4, Result) == 1) zj_zs = 0;
        }
        if (i == 7)//杂六
        {
            if (Qiansan(3, Result) == 1) //豹子
                zj_zs = 1;
            if (Qiansan(4, Result) == 1) //顺子
                zj_zs = 1;
            if (Qiansan(5, Result) == 1) //对子
                zj_zs = 1;
            if (Qiansan(6, Result) == 1) //半顺
                zj_zs = 1;
        }
        return zj_zs;
    }
    #endregion

    #region 中三
    private int Zhongsan(int i, string Result)
    {
        string[] iNum_kj = Result.Split(' ');
        int zj_zs = 0;

        if (i == 1)//大小
        {
            int sum = 0;
            sum = Convert.ToInt32(iNum_kj[1]) + Convert.ToInt32(iNum_kj[2]) + Convert.ToInt32(iNum_kj[3]);
            if (sum > 13 && sum < 28)//大
            {
                zj_zs = 1;
            }
            if (sum < 14)//小
            {
                zj_zs = 2;
            }
        }
        if (i == 2)//单双
        {
            int sum = 0;
            sum = Convert.ToInt32(iNum_kj[1]) + Convert.ToInt32(iNum_kj[2]) + Convert.ToInt32(iNum_kj[3]);
            if (sum % 2 != 0)//单
            {
                zj_zs = 1;
            }
            if (sum % 2 == 0)//双
            {
                zj_zs = 2;
            }
        }
        if (i == 3)//豹子
        {
            if (Convert.ToInt32(iNum_kj[1]) == Convert.ToInt32(iNum_kj[2]) && Convert.ToInt32(iNum_kj[2]) == Convert.ToInt32(iNum_kj[3]))
            {
                zj_zs = 1;
            }
        }
        if (i == 4)//顺子
        {
            int n1 = Convert.ToInt32(iNum_kj[1]); if (n1 == 0) n1 = 10;
            int n2 = Convert.ToInt32(iNum_kj[2]); if (n2 == 0) n2 = 10;
            int n3 = Convert.ToInt32(iNum_kj[3]); if (n3 == 0) n3 = 10;
            int a = n1 - n2;
            int b = n1 - n3;
            int c = n2 - n3;

            int d = 0;
            if (n1 != n2 && n2 != n3 && n1 != n3)
            {
                d = Math.Abs(a) + Math.Abs(b) + Math.Abs(c);
            }

            if (d == 4)
            {
                zj_zs = 1;
            }
            if (n1 == 10 && n2 == 1 && n3 == 2) zj_zs = 1;//012
            if (n1 == 10 && n2 == 2 && n3 == 1) zj_zs = 1;
            if (n1 == 1 && n2 == 10 && n3 == 2) zj_zs = 1;
            if (n1 == 1 && n2 == 2 && n3 == 10) zj_zs = 1;
            if (n1 == 2 && n2 == 1 && n3 == 10) zj_zs = 1;
            if (n1 == 2 && n2 == 10 && n3 == 1) zj_zs = 1;

            if (n1 == 10 && n2 == 1 && n3 == 9) zj_zs = 1;//901
            if (n1 == 10 && n2 == 9 && n3 == 1) zj_zs = 1;
            if (n1 == 1 && n2 == 10 && n3 == 9) zj_zs = 1;
            if (n1 == 1 && n2 == 9 && n3 == 10) zj_zs = 1;
            if (n1 == 9 && n2 == 1 && n3 == 10) zj_zs = 1;
            if (n1 == 9 && n2 == 10 && n3 == 1) zj_zs = 1;
        }
        if (i == 5)//对子
        {
            int n1 = Convert.ToInt32(iNum_kj[1]);
            int n2 = Convert.ToInt32(iNum_kj[2]);
            int n3 = Convert.ToInt32(iNum_kj[3]);

            if (n1 == n2 && n2 != n3)
            {
                zj_zs = 1;
            }
            if (n1 == n3 && n3 != n2)
            {
                zj_zs = 1;
            }
            if (n2 == n3 && n3 != n1)
            {
                zj_zs = 1;
            }
        }
        if (i == 6)//半顺
        {
            int n1 = Convert.ToInt32(iNum_kj[1]); if (n1 == 0) n1 = 10;
            int n2 = Convert.ToInt32(iNum_kj[2]); if (n2 == 0) n2 = 10;
            int n3 = Convert.ToInt32(iNum_kj[3]); if (n3 == 0) n3 = 10;

            int a = n1 - n2;
            int b = n1 - n3;
            int c = n2 - n3;

            if (Math.Abs(a) == 1 && Math.Abs(b) > 1 && Math.Abs(c) > 1)
            {
                zj_zs = 1;
            }
            if (Math.Abs(b) == 1 && Math.Abs(a) > 1 && Math.Abs(c) > 1)
            {
                zj_zs = 1;
            }
            if (Math.Abs(c) == 1 && Math.Abs(b) > 1 && Math.Abs(a) > 1)
            {
                zj_zs = 1;
            }
            string n123 = iNum_kj[1] + iNum_kj[2] + iNum_kj[3];
            {
                if (n1 != n2 && n2 != n3 && n1 != n3)
                {
                    if (n123.Contains("0") && n123.Contains("1") && !n123.Contains("2"))
                        zj_zs = 1;
                }
            }
            if (Zhongsan(4, Result) == 1) zj_zs = 0;
        }
        if (i == 7)//杂六
        {
            if (Zhongsan(3, Result) == 1) //豹子
                zj_zs = 1;
            if (Zhongsan(4, Result) == 1) //顺子
                zj_zs = 1;
            if (Zhongsan(5, Result) == 1) //对子
                zj_zs = 1;
            if (Zhongsan(6, Result) == 1) //半顺
                zj_zs = 1;

        }
        return zj_zs;
    }
    #endregion

    #region 后三
    private int Housan(int i, string Result)
    {
        string[] iNum_kj = Result.Split(' ');
        int zj_zs = 0;

        if (i == 1)//大小
        {
            int sum = 0;
            sum = Convert.ToInt32(iNum_kj[2]) + Convert.ToInt32(iNum_kj[3]) + Convert.ToInt32(iNum_kj[4]);
            if (sum > 13 && sum < 28)//大
            {
                zj_zs = 1;
            }
            if (sum < 14)//小
            {
                zj_zs = 2;
            }
        }
        if (i == 2)//单双
        {
            int sum = 0;
            sum = Convert.ToInt32(iNum_kj[2]) + Convert.ToInt32(iNum_kj[3]) + Convert.ToInt32(iNum_kj[4]);
            if (sum % 2 != 0)//单
            {
                zj_zs = 1;
            }
            if (sum % 2 == 0)//双
            {
                zj_zs = 2;
            }
        }
        if (i == 3)//豹子
        {
            if (Convert.ToInt32(iNum_kj[2]) == Convert.ToInt32(iNum_kj[3]) && Convert.ToInt32(iNum_kj[3]) == Convert.ToInt32(iNum_kj[4]))
            {
                zj_zs = 1;
            }
        }
        if (i == 4)//顺子
        {
            int n1 = Convert.ToInt32(iNum_kj[2]); if (n1 == 0) n1 = 10;
            int n2 = Convert.ToInt32(iNum_kj[3]); if (n2 == 0) n2 = 10;
            int n3 = Convert.ToInt32(iNum_kj[4]); if (n3 == 0) n3 = 10;
            int d = 0;
            if (n1 != n2 && n2 != n3 && n1 != n3)
            {
                int a = n1 - n2;
                int b = n1 - n3;
                int c = n2 - n3;
                d = Math.Abs(a) + Math.Abs(b) + Math.Abs(c);
            }

            if (d == 4)
            {
                zj_zs = 1;
            }

            if (n1 == 10 && n2 == 1 && n3 == 2) zj_zs = 1;//012
            if (n1 == 10 && n2 == 2 && n3 == 1) zj_zs = 1;
            if (n1 == 1 && n2 == 10 && n3 == 2) zj_zs = 1;
            if (n1 == 1 && n2 == 2 && n3 == 10) zj_zs = 1;
            if (n1 == 2 && n2 == 1 && n3 == 10) zj_zs = 1;
            if (n1 == 2 && n2 == 10 && n3 == 1) zj_zs = 1;

            if (n1 == 10 && n2 == 1 && n3 == 9) zj_zs = 1;//901
            if (n1 == 10 && n2 == 9 && n3 == 1) zj_zs = 1;
            if (n1 == 1 && n2 == 10 && n3 == 9) zj_zs = 1;
            if (n1 == 1 && n2 == 9 && n3 == 10) zj_zs = 1;
            if (n1 == 9 && n2 == 1 && n3 == 10) zj_zs = 1;
            if (n1 == 9 && n2 == 10 && n3 == 1) zj_zs = 1;
        }
        if (i == 5)//对子
        {
            int n1 = Convert.ToInt32(iNum_kj[2]);
            int n2 = Convert.ToInt32(iNum_kj[3]);
            int n3 = Convert.ToInt32(iNum_kj[4]);

            if (n1 == n2 && n2 != n3)
            {
                zj_zs = 1;
            }
            if (n1 == n3 && n3 != n2)
            {
                zj_zs = 1;
            }
            if (n2 == n3 && n3 != n1)
            {
                zj_zs = 1;
            }
        }
        if (i == 6)//半顺
        {
            int n1 = Convert.ToInt32(iNum_kj[2]); if (n1 == 0) n1 = 10;
            int n2 = Convert.ToInt32(iNum_kj[3]); if (n2 == 0) n2 = 10;
            int n3 = Convert.ToInt32(iNum_kj[4]); if (n3 == 0) n3 = 10;

            int a = n1 - n2;
            int b = n1 - n3;
            int c = n2 - n3;

            if (Math.Abs(a) == 1 && Math.Abs(b) > 1 && Math.Abs(c) > 1)
            {
                zj_zs = 1;
            }
            if (Math.Abs(b) == 1 && Math.Abs(a) > 1 && Math.Abs(c) > 1)
            {
                zj_zs = 1;
            }
            if (Math.Abs(c) == 1 && Math.Abs(b) > 1 && Math.Abs(a) > 1)
            {
                zj_zs = 1;
            }
            string n123 = iNum_kj[2] + iNum_kj[3] + iNum_kj[4];
            {
                if (n1 != n2 && n2 != n3 && n1 != n3)
                {
                    if (n123.Contains("0") && n123.Contains("1") && !n123.Contains("2"))
                        zj_zs = 1;
                }
            }
            if (Housan(4, Result) == 1) zj_zs = 0;
        }
        if (i == 7)//杂六
        {
            if (Housan(3, Result) == 1) //豹子
                zj_zs = 1;
            if (Housan(4, Result) == 1) //顺子
                zj_zs = 1;
            if (Housan(5, Result) == 1) //对子
                zj_zs = 1;
            if (Housan(6, Result) == 1) //半顺
                zj_zs = 1;
        }
        return zj_zs;
    }
    #endregion

    #region 梭哈散牌
    private int SHSanpai(string Result)
    {
        string[] iNum_kj = Result.Split(' ');
        int zj_zs = 0;
        int[] a = { Convert.ToInt32(iNum_kj[0]), Convert.ToInt32(iNum_kj[1]), Convert.ToInt32(iNum_kj[2]), Convert.ToInt32(iNum_kj[3]), Convert.ToInt32(iNum_kj[4]) };
        int equal = 0;
        for (int i = 0; i < iNum_kj.Length - 1; i++)
        {
            for (int j = i + 1; j < iNum_kj.Length; j++)
            {
                if (a[i] == a[j])
                {
                    equal = 1;
                    break;
                }
            }
        }
        if (equal == 0)//数全不相等;
        {
            zj_zs = 1;
        }

        if (SHShunzi(Result) == 1) zj_zs = 0;//不能为顺子

        return zj_zs;
    }
    #endregion

    #region 梭哈单对
    private int SHDandui(string Result)
    {
        string[] iNum_kj = Result.Split(' ');
        int zj_zs = 0;

        int count0 = Result.Replace("*", "").Replace("<img", "*").Split('0').Length - 1;
        int count1 = Result.Replace("*", "").Replace("<img", "*").Split('1').Length - 1;
        int count2 = Result.Replace("*", "").Replace("<img", "*").Split('2').Length - 1;
        int count3 = Result.Replace("*", "").Replace("<img", "*").Split('3').Length - 1;
        int count4 = Result.Replace("*", "").Replace("<img", "*").Split('4').Length - 1;
        int count5 = Result.Replace("*", "").Replace("<img", "*").Split('5').Length - 1;
        int count6 = Result.Replace("*", "").Replace("<img", "*").Split('6').Length - 1;
        int count7 = Result.Replace("*", "").Replace("<img", "*").Split('7').Length - 1;
        int count8 = Result.Replace("*", "").Replace("<img", "*").Split('8').Length - 1;
        int count9 = Result.Replace("*", "").Replace("<img", "*").Split('9').Length - 1;

        if (count0 == 2) { if (count1 == 1 || count2 == 1 || count3 == 1 || count4 == 1 || count5 == 1 || count6 == 1 || count7 == 1 || count8 == 1 || count9 == 1) zj_zs = 1; }
        if (count1 == 2) { if (count0 == 1 || count2 == 1 || count3 == 1 || count4 == 1 || count5 == 1 || count6 == 1 || count7 == 1 || count8 == 1 || count9 == 1) zj_zs = 1; }
        if (count2 == 2) { if (count1 == 1 || count0 == 1 || count3 == 1 || count4 == 1 || count5 == 1 || count6 == 1 || count7 == 1 || count8 == 1 || count9 == 1) zj_zs = 1; }
        if (count3 == 2) { if (count1 == 1 || count2 == 1 || count0 == 1 || count4 == 1 || count5 == 1 || count6 == 1 || count7 == 1 || count8 == 1 || count9 == 1) zj_zs = 1; }
        if (count4 == 2) { if (count1 == 1 || count2 == 1 || count3 == 1 || count0 == 1 || count5 == 1 || count6 == 1 || count7 == 1 || count8 == 1 || count9 == 1) zj_zs = 1; }
        if (count5 == 2) { if (count1 == 1 || count2 == 1 || count3 == 1 || count4 == 1 || count0 == 1 || count6 == 1 || count7 == 1 || count8 == 1 || count9 == 1) zj_zs = 1; }
        if (count6 == 2) { if (count1 == 1 || count2 == 1 || count3 == 1 || count4 == 1 || count5 == 1 || count0 == 1 || count7 == 1 || count8 == 1 || count9 == 1) zj_zs = 1; }
        if (count7 == 2) { if (count1 == 1 || count2 == 1 || count3 == 1 || count4 == 1 || count5 == 1 || count6 == 1 || count0 == 1 || count8 == 1 || count9 == 1) zj_zs = 1; }
        if (count8 == 2) { if (count1 == 1 || count2 == 1 || count3 == 1 || count4 == 1 || count5 == 1 || count6 == 1 || count7 == 1 || count0 == 1 || count9 == 1) zj_zs = 1; }
        if (count9 == 2) { if (count1 == 1 || count2 == 1 || count3 == 1 || count4 == 1 || count5 == 1 || count6 == 1 || count7 == 1 || count8 == 1 || count0 == 1) zj_zs = 1; }

        if (HuLu(Result) == 1) zj_zs = 0;//不能为葫芦
        if (SHLiangdui(Result) == 1) zj_zs = 0;//不能为两对

        return zj_zs;
    }
    #endregion

    #region 梭哈两对
    private int SHLiangdui(string Result)
    {
        string[] iNum_kj = Result.Split(' ');
        int zj_zs = 0;

        int count0 = Result.Replace("*", "").Replace("<img", "*").Split('0').Length - 1;
        int count1 = Result.Replace("*", "").Replace("<img", "*").Split('1').Length - 1;
        int count2 = Result.Replace("*", "").Replace("<img", "*").Split('2').Length - 1;
        int count3 = Result.Replace("*", "").Replace("<img", "*").Split('3').Length - 1;
        int count4 = Result.Replace("*", "").Replace("<img", "*").Split('4').Length - 1;
        int count5 = Result.Replace("*", "").Replace("<img", "*").Split('5').Length - 1;
        int count6 = Result.Replace("*", "").Replace("<img", "*").Split('6').Length - 1;
        int count7 = Result.Replace("*", "").Replace("<img", "*").Split('7').Length - 1;
        int count8 = Result.Replace("*", "").Replace("<img", "*").Split('8').Length - 1;
        int count9 = Result.Replace("*", "").Replace("<img", "*").Split('9').Length - 1;

        if (count0 == 2) { if (count1 == 2 || count2 == 2 || count3 == 2 || count4 == 2 || count5 == 2 || count6 == 2 || count7 == 2 || count8 == 2 || count9 == 2) zj_zs = 1; }
        if (count1 == 2) { if (count0 == 2 || count2 == 2 || count3 == 2 || count4 == 2 || count5 == 2 || count6 == 2 || count7 == 2 || count8 == 2 || count9 == 2) zj_zs = 1; }
        if (count2 == 2) { if (count1 == 2 || count0 == 2 || count3 == 2 || count4 == 2 || count5 == 2 || count6 == 2 || count7 == 2 || count8 == 2 || count9 == 2) zj_zs = 1; }
        if (count3 == 2) { if (count1 == 2 || count2 == 2 || count0 == 2 || count4 == 2 || count5 == 2 || count6 == 2 || count7 == 2 || count8 == 2 || count9 == 2) zj_zs = 1; }
        if (count4 == 2) { if (count1 == 2 || count2 == 2 || count3 == 2 || count0 == 2 || count5 == 2 || count6 == 2 || count7 == 2 || count8 == 2 || count9 == 2) zj_zs = 1; }
        if (count5 == 2) { if (count1 == 2 || count2 == 2 || count3 == 2 || count4 == 2 || count0 == 2 || count6 == 2 || count7 == 2 || count8 == 2 || count9 == 2) zj_zs = 1; }
        if (count6 == 2) { if (count1 == 2 || count2 == 2 || count3 == 2 || count4 == 2 || count5 == 2 || count0 == 2 || count7 == 2 || count8 == 2 || count9 == 2) zj_zs = 1; }
        if (count7 == 2) { if (count1 == 2 || count2 == 2 || count3 == 2 || count4 == 2 || count5 == 2 || count6 == 2 || count0 == 2 || count8 == 2 || count9 == 2) zj_zs = 1; }
        if (count8 == 2) { if (count1 == 2 || count2 == 2 || count3 == 2 || count4 == 2 || count5 == 2 || count6 == 2 || count7 == 2 || count0 == 2 || count9 == 2) zj_zs = 1; }
        if (count9 == 2) { if (count1 == 2 || count2 == 2 || count3 == 2 || count4 == 2 || count5 == 2 || count6 == 2 || count7 == 2 || count8 == 2 || count0 == 2) zj_zs = 1; }

        if (HuLu(Result) == 1) zj_zs = 0;//不能为葫芦

        return zj_zs;
    }
    #endregion

    #region 梭哈三条
    private int SHSantiao(string Result)
    {
        string[] iNum_kj = Result.Split(' ');
        int zj_zs = 0;

        int count0 = Result.Replace("*", "").Replace("<img", "*").Split('0').Length - 1;
        int count1 = Result.Replace("*", "").Replace("<img", "*").Split('1').Length - 1;
        int count2 = Result.Replace("*", "").Replace("<img", "*").Split('2').Length - 1;
        int count3 = Result.Replace("*", "").Replace("<img", "*").Split('3').Length - 1;
        int count4 = Result.Replace("*", "").Replace("<img", "*").Split('4').Length - 1;
        int count5 = Result.Replace("*", "").Replace("<img", "*").Split('5').Length - 1;
        int count6 = Result.Replace("*", "").Replace("<img", "*").Split('6').Length - 1;
        int count7 = Result.Replace("*", "").Replace("<img", "*").Split('7').Length - 1;
        int count8 = Result.Replace("*", "").Replace("<img", "*").Split('8').Length - 1;
        int count9 = Result.Replace("*", "").Replace("<img", "*").Split('9').Length - 1;

        if (count0 == 3) { if (count1 == 1 || count2 == 1 || count3 == 1 || count4 == 1 || count5 == 1 || count6 == 1 || count7 == 1 || count8 == 1 || count9 == 1) zj_zs = 1; }
        if (count1 == 3) { if (count0 == 1 || count2 == 1 || count3 == 1 || count4 == 1 || count5 == 1 || count6 == 1 || count7 == 1 || count8 == 1 || count9 == 1) zj_zs = 1; }
        if (count2 == 3) { if (count1 == 1 || count0 == 1 || count3 == 1 || count4 == 1 || count5 == 1 || count6 == 1 || count7 == 1 || count8 == 1 || count9 == 1) zj_zs = 1; }
        if (count3 == 3) { if (count1 == 1 || count2 == 1 || count0 == 1 || count4 == 1 || count5 == 1 || count6 == 1 || count7 == 1 || count8 == 1 || count9 == 1) zj_zs = 1; }
        if (count4 == 3) { if (count1 == 1 || count2 == 1 || count3 == 1 || count0 == 1 || count5 == 1 || count6 == 1 || count7 == 1 || count8 == 1 || count9 == 1) zj_zs = 1; }
        if (count5 == 3) { if (count1 == 1 || count2 == 1 || count3 == 1 || count4 == 1 || count0 == 1 || count6 == 1 || count7 == 1 || count8 == 1 || count9 == 1) zj_zs = 1; }
        if (count6 == 3) { if (count1 == 1 || count2 == 1 || count3 == 1 || count4 == 1 || count5 == 1 || count0 == 1 || count7 == 1 || count8 == 1 || count9 == 1) zj_zs = 1; }
        if (count7 == 3) { if (count1 == 1 || count2 == 1 || count3 == 1 || count4 == 1 || count5 == 1 || count6 == 1 || count0 == 1 || count8 == 1 || count9 == 1) zj_zs = 1; }
        if (count8 == 3) { if (count1 == 1 || count2 == 1 || count3 == 1 || count4 == 1 || count5 == 1 || count6 == 1 || count7 == 1 || count0 == 1 || count9 == 1) zj_zs = 1; }
        if (count9 == 3) { if (count1 == 1 || count2 == 1 || count3 == 1 || count4 == 1 || count5 == 1 || count6 == 1 || count7 == 1 || count8 == 1 || count0 == 1) zj_zs = 1; }

        return zj_zs;
    }
    #endregion

    #region 梭哈顺子
    private int SHShunzi(string Result)
    {
        int a = 0;
        if (Result == "0 1 2 3 4" || Result == "1 2 3 4 5" || Result == "2 3 4 5 6" || Result == "3 4 5 6 7" || Result == "4 5 6 7 8" || Result == "5 6 7 8 9" || Result == "0 6 7 8 9" || Result == "0 1 7 8 9" || Result == "0 1 2 8 9" || Result == "0 1 2 3 9")
        {
            a = 1;
        }

        return a;
    }
    #endregion

    #region 炸弹算法
    private int Zhadan(string Result)
    {
        string[] iNum_kj = Result.Split(' ');
        int zj_zs = 0;
        int a = 0;
        int b = 0;
        int c = 0;
        int d = 0;
        int f = 0;
        for (int j = 0; j < iNum_kj.Length; j++)
        {
            if (Convert.ToInt32(iNum_kj[j]) == Convert.ToInt32(iNum_kj[0]))
                a += 1;
            if (Convert.ToInt32(iNum_kj[j]) == Convert.ToInt32(iNum_kj[1]))
                b += 1;
            if (Convert.ToInt32(iNum_kj[j]) == Convert.ToInt32(iNum_kj[2]))
                c += 1;
            if (Convert.ToInt32(iNum_kj[j]) == Convert.ToInt32(iNum_kj[3]))
                d += 1;
            if (Convert.ToInt32(iNum_kj[j]) == Convert.ToInt32(iNum_kj[4]))
                f += 1;
        }
        if (a >= 4)
            zj_zs = a;
        if (b >= 4)
            zj_zs = b;
        if (c >= 4)
            zj_zs = c;
        if (d >= 4)
            zj_zs = d;
        if (f >= 4)
            zj_zs = f;
        return zj_zs;
    }
    #endregion

    #region 葫芦算法
    private int HuLu(string Result)
    {
        string[] iNum_kj = Result.Split(' ');
        int zj_zs = 0;

        int count0 = Result.Replace("*", "").Replace("<img", "*").Split('0').Length - 1;
        int count1 = Result.Replace("*", "").Replace("<img", "*").Split('1').Length - 1;
        int count2 = Result.Replace("*", "").Replace("<img", "*").Split('2').Length - 1;
        int count3 = Result.Replace("*", "").Replace("<img", "*").Split('3').Length - 1;
        int count4 = Result.Replace("*", "").Replace("<img", "*").Split('4').Length - 1;
        int count5 = Result.Replace("*", "").Replace("<img", "*").Split('5').Length - 1;
        int count6 = Result.Replace("*", "").Replace("<img", "*").Split('6').Length - 1;
        int count7 = Result.Replace("*", "").Replace("<img", "*").Split('7').Length - 1;
        int count8 = Result.Replace("*", "").Replace("<img", "*").Split('8').Length - 1;
        int count9 = Result.Replace("*", "").Replace("<img", "*").Split('9').Length - 1;

        if (count0 == 3) { if (count1 == 2 || count2 == 2 || count3 == 2 || count4 == 2 || count5 == 2 || count6 == 2 || count7 == 2 || count8 == 2 || count9 == 2) zj_zs = 1; }
        if (count1 == 3) { if (count0 == 2 || count2 == 2 || count3 == 2 || count4 == 2 || count5 == 2 || count6 == 2 || count7 == 2 || count8 == 2 || count9 == 2) zj_zs = 1; }
        if (count2 == 3) { if (count1 == 2 || count0 == 2 || count3 == 2 || count4 == 2 || count5 == 2 || count6 == 2 || count7 == 2 || count8 == 2 || count9 == 2) zj_zs = 1; }
        if (count3 == 3) { if (count1 == 2 || count2 == 2 || count0 == 2 || count4 == 2 || count5 == 2 || count6 == 2 || count7 == 2 || count8 == 2 || count9 == 2) zj_zs = 1; }
        if (count4 == 3) { if (count1 == 2 || count2 == 2 || count3 == 2 || count0 == 2 || count5 == 2 || count6 == 2 || count7 == 2 || count8 == 2 || count9 == 2) zj_zs = 1; }
        if (count5 == 3) { if (count1 == 2 || count2 == 2 || count3 == 2 || count4 == 2 || count0 == 2 || count6 == 2 || count7 == 2 || count8 == 2 || count9 == 2) zj_zs = 1; }
        if (count6 == 3) { if (count1 == 2 || count2 == 2 || count3 == 2 || count4 == 2 || count5 == 2 || count0 == 2 || count7 == 2 || count8 == 2 || count9 == 2) zj_zs = 1; }
        if (count7 == 3) { if (count1 == 2 || count2 == 2 || count3 == 2 || count4 == 2 || count5 == 2 || count6 == 2 || count0 == 2 || count8 == 2 || count9 == 2) zj_zs = 1; }
        if (count8 == 3) { if (count1 == 2 || count2 == 2 || count3 == 2 || count4 == 2 || count5 == 2 || count6 == 2 || count7 == 2 || count0 == 2 || count9 == 2) zj_zs = 1; }
        if (count9 == 3) { if (count1 == 2 || count2 == 2 || count3 == 2 || count4 == 2 || count5 == 2 || count6 == 2 || count7 == 2 || count8 == 2 || count0 == 2) zj_zs = 1; }

        return zj_zs;
    }
    #endregion

    #region 牛牛算法
    ///<summary>
    ///牛牛算法
    ///返回result ，为空则是无牛
    /// </summary>
    private string Niu(string Result)
    {
        string result = string.Empty;
        string a = string.Empty;
        string b = string.Empty;
        string[] num = Result.Split(' ');

        if ((Convert.ToInt32(num[0]) + Convert.ToInt32(num[1]) + Convert.ToInt32(num[2])) % 10 == 0)//012
        {
            a = "牛";
            b = ((Convert.ToInt32(num[3]) + Convert.ToInt32(num[4])) % 10).ToString();
            result = a + b;
        }
        else if ((Convert.ToInt32(num[0]) + Convert.ToInt32(num[1]) + Convert.ToInt32(num[3])) % 10 == 0)//013
        {
            a = "牛";
            b = ((Convert.ToInt32(num[2]) + Convert.ToInt32(num[4])) % 10).ToString();
            result = a + b;
        }
        else if ((Convert.ToInt32(num[0]) + Convert.ToInt32(num[1]) + Convert.ToInt32(num[4])) % 10 == 0)//014
        {
            a = "牛";
            b = ((Convert.ToInt32(num[2]) + Convert.ToInt32(num[3])) % 10).ToString();
            result = a + b;
        }
        else if ((Convert.ToInt32(num[0]) + Convert.ToInt32(num[2]) + Convert.ToInt32(num[3])) % 10 == 0)//023
        {
            a = "牛";
            b = ((Convert.ToInt32(num[1]) + Convert.ToInt32(num[4])) % 10).ToString();
            result = a + b;
        }
        else if ((Convert.ToInt32(num[0]) + Convert.ToInt32(num[2]) + Convert.ToInt32(num[4])) % 10 == 0)//024
        {
            a = "牛";
            b = ((Convert.ToInt32(num[1]) + Convert.ToInt32(num[3])) % 10).ToString();
            result = a + b;
        }
        else if ((Convert.ToInt32(num[0]) + Convert.ToInt32(num[3]) + Convert.ToInt32(num[4])) % 10 == 0)//034
        {
            a = "牛";
            b = ((Convert.ToInt32(num[1]) + Convert.ToInt32(num[2])) % 10).ToString();
            result = a + b;
        }
        else if ((Convert.ToInt32(num[1]) + Convert.ToInt32(num[2]) + Convert.ToInt32(num[3])) % 10 == 0)//123
        {
            a = "牛";
            b = ((Convert.ToInt32(num[0]) + Convert.ToInt32(num[4])) % 10).ToString();
            result = a + b;
        }
        else if ((Convert.ToInt32(num[1]) + Convert.ToInt32(num[2]) + Convert.ToInt32(num[4])) % 10 == 0)//124
        {
            a = "牛";
            b = ((Convert.ToInt32(num[0]) + Convert.ToInt32(num[3])) % 10).ToString();
            result = a + b;
        }
        else if ((Convert.ToInt32(num[1]) + Convert.ToInt32(num[3]) + Convert.ToInt32(num[4])) % 10 == 0)//134
        {
            a = "牛";
            b = ((Convert.ToInt32(num[0]) + Convert.ToInt32(num[2])) % 10).ToString();
            result = a + b;
        }
        else if ((Convert.ToInt32(num[2]) + Convert.ToInt32(num[3]) + Convert.ToInt32(num[4])) % 10 == 0)//234
        {
            a = "牛";
            b = ((Convert.ToInt32(num[0]) + Convert.ToInt32(num[1])) % 10).ToString();
            result = a + b;
        }

        return result;
    }
    #endregion

    #region 结果信息显示
    /// <summary>
    /// 结果信息 总和-牛牛-梭哈
    /// </summary>
    /// <param name="Result"></param>
    /// <returns></returns>
    private string Message(string Result)
    {
        try
        {
            string message = string.Empty;

            string[] result = Result.Split(' ');
            int sum = 0;
            for (int i = 0; i < result.Length; i++)
            {
                sum += Convert.ToInt32(result[i]);
            }

            string niu = Niu(Result);
            if (niu != "")
            {
                if (Niu(Result) == "牛0")
                {
                    niu = "牛牛";
                }
                else
                {
                    niu = Niu(Result);
                }
            }
            else
            {
                niu = "无牛";
            }

            string suoha = string.Empty;
            if (Zhadan(Result) >= 4)
            {
                suoha = "炸弹";
            }
            if (HuLu(Result) == 1)
            {
                suoha = "葫芦";
            }
            if (SHShunzi(Result) == 1)
            {
                suoha = "顺子";
            }
            if (SHSantiao(Result) == 1)
            {
                suoha = "三条";
            }
            if (SHLiangdui(Result) == 1)
            {
                suoha = "两对";
            }
            if (SHDandui(Result) == 1)
            {
                suoha = "单对";
            }
            if (SHSanpai(Result) == 1)
            {
                suoha = "散牌";
            }

            message = sum.ToString() + "-" + niu + "-" + suoha;
            return message;
        }
        catch { return "未开奖"; }
    }
    #endregion

    #region 下注类型 OutType
    /// <summary>
    /// 下注类型
    /// </summary>
    /// <param name="Types"></param>
    /// <returns></returns>
    private string OutType(int Types)
    {
        string ptypey = string.Empty;
        string payname1 = string.Empty;
        string odds1 = string.Empty;
        string oddsc1 = string.Empty;
        string rule1 = string.Empty;
        for (int i = 1; i < 57; i++)
        {
            ptypey = ub.GetSub("ptype" + i + "", xmlPath);
            string[] ptypef = ptypey.Split('#');
            payname1 += "#" + ptypef[0];
            odds1 += "#" + ptypef[1];
            oddsc1 += "#" + ptypef[2];
            rule1 += "#" + ptypef[3];
        }
        string[] payname2 = payname1.Split('#');
        string[] odds2 = odds1.Split('#');
        string[] oddsc2 = oddsc1.Split('#');
        string[] rule2 = rule1.Split('#');
        string pText = string.Empty;

        for (int i = 1; i < 57; i++)
        {
            if (Types == i)
                pText = payname2[i];
        }

        return pText;
    }
    #endregion

    #region 玩法提示 OutRule
    /// <summary>
    /// 玩法提示
    /// </summary>
    /// <param name="Types"></param>
    /// <returns></returns>
    private string OutRule(int Types)
    {
        string ptypey = string.Empty;
        string payname1 = string.Empty;
        string odds1 = string.Empty;
        string oddsc1 = string.Empty;
        string rule1 = string.Empty;
        for (int i = 1; i < 57; i++)
        {
            ptypey = ub.GetSub("ptype" + i + "", xmlPath);
            string[] ptypef = ptypey.Split('#');
            payname1 += "#" + ptypef[0];
            odds1 += "#" + ptypef[1];
            oddsc1 += "#" + ptypef[2];
            rule1 += "#" + ptypef[3];
        }
        string[] payname2 = payname1.Split('#');
        string[] odds2 = odds1.Split('#');
        string[] oddsc2 = oddsc1.Split('#');
        string[] rule2 = rule1.Split('#');
        string pText = string.Empty;

        for (int i = 1; i < 57; i++)
        {
            if (Types == i)
                pText = rule2[i];
        }

        return pText;
    }
    #endregion

    #region 赔率 OutOdds
    /// <summary>
    /// 赔率 如果赔率只有一个，n就是1位，赔率取第二位，n就是2，赔率n位，取N位
    /// </summary>
    /// <param name="Types"></param>
    /// <returns></returns>
    private string OutOdds(int Types, int n)
    {
        string ptypey = string.Empty;
        string payname1 = string.Empty;
        string odds1 = string.Empty;
        string oddsc1 = string.Empty;
        string rule1 = string.Empty;
        for (int i = 1; i < 57; i++)
        {
            ptypey = ub.GetSub("ptype" + i + "", xmlPath);
            string[] ptypef = ptypey.Split('#');
            payname1 += "#" + ptypef[0];
            odds1 += "#" + ptypef[1];
            oddsc1 += "#" + ptypef[2];
            rule1 += "#" + ptypef[3];
        }
        string[] payname2 = payname1.Split('#');
        string[] odds2 = odds1.Split('#');
        string[] oddsc2 = oddsc1.Split('#');
        string[] rule2 = rule1.Split('#');
        string pText = string.Empty;

        for (int i = 1; i < 57; i++)
        {
            string[] odds = odds2[i].Split('|');

            if (odds.Length == 1)
            {
                if (Types == i)
                    pText = odds2[i];
            }
            else
            {
                for (int m = 0; m < odds.Length; m++)
                {
                    if (Types == i && m == (n - 1))
                    {
                        pText = odds[m];
                    }
                }
            }
        }

        return pText;
    }
    #endregion

    #region 投注额度 OutOddsc
    /// <summary>
    /// 投注额度
    /// </summary>
    /// <param name="Types"></param>
    /// <returns></returns>
    private string OutOddsc(int Types)
    {
        string ptypey = string.Empty;
        string payname1 = string.Empty;
        string odds1 = string.Empty;
        string oddsc1 = string.Empty;
        string rule1 = string.Empty;
        for (int i = 1; i < 57; i++)
        {
            ptypey = ub.GetSub("ptype" + i + "", xmlPath);
            string[] ptypef = ptypey.Split('#');
            payname1 += "#" + ptypef[0];
            odds1 += "#" + ptypef[1];
            oddsc1 += "#" + ptypef[2];
            rule1 += "#" + ptypef[3];
        }
        string[] payname2 = payname1.Split('#');
        string[] odds2 = odds1.Split('#');
        string[] oddsc2 = oddsc1.Split('#');
        string[] rule2 = rule1.Split('#');
        string pText = string.Empty;

        for (int i = 1; i < 57; i++)
        {
            if (Types == i)
                pText = oddsc2[i];
        }

        return pText;
    }
    #endregion

    #region 中奖注数
    private int GetZj_zs(string WinNotes)
    {
        int a = 0;
        if (WinNotes != " " || WinNotes != "" || WinNotes != null)
        {
            string[] b = WinNotes.Split(':');
            try
            {
                a = Convert.ToInt32(b[1]);
            }
            catch { }
        }
        return a;
    }
    #endregion

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
        string[] num = Result.Split(' ');
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

    /// <summary>
    /// 将两个数字转化成三个数字的两组集合
    /// </summary>
    /// <param name="sNum"></param>
    /// <returns></returns>
    private string OutStrNum(string sNum)
    {
        string[] Temp = sNum.Split(',');
        string strNum1 = Temp[0] + "," + sNum;
        string strNum2 = sNum + "," + Temp[1];
        string strNum = strNum1 + "，" + strNum2;
        return strNum.Replace(",", "");
    }
    /// <summary>
    /// 两个数是否相似
    /// </summary>
    private bool IsLike(string Num1, string Num2)
    {
        bool like = true;
        string getNum1 = Utils.ConvertSeparated(Num1, 1, ",");
        string getNum2 = Utils.ConvertSeparated(Num2, 1, ",");

        string[] str1 = getNum1.Split(',');
        string[] str2 = getNum2.Split(',');

        for (int i = 0; i < str1.Length; i++)
        {
            int cNum = Utils.GetStringNum(Num1, str1[i]);
            int cNum2 = Utils.GetStringNum(Num2, str1[i]);

            if (cNum != cNum2)
            {
                like = false;
                break;
            }

        }

        return like;
    }

    /// <summary>
    /// 统计有多少个不同数字
    /// </summary>
    /// <param name="Result"></param>
    /// <returns></returns>
    private int BackLikeNum(string Result)
    {
        int num = 0;

        if (Result.Contains("0"))
            num = num + 1;
        if (Result.Contains("1"))
            num = num + 1;
        if (Result.Contains("2"))
            num = num + 1;
        if (Result.Contains("3"))
            num = num + 1;
        if (Result.Contains("4"))
            num = num + 1;
        if (Result.Contains("5"))
            num = num + 1;
        if (Result.Contains("6"))
            num = num + 1;
        if (Result.Contains("7"))
            num = num + 1;
        if (Result.Contains("8"))
            num = num + 1;
        if (Result.Contains("9"))
            num = num + 1;

        return num;
    }
    //统计每一期总投入
    private long GetPayAllbySSCId(int SSCId)
    {
        long allpay = 0;
        try
        {
            long sum = 0;
            DataSet ds = new BCW.ssc.BLL.SSCpay().GetList("Sum(Prices) as Price", " SSCId=" + SSCId + " and IsSpier!=1 and UsID in (select ID from tb_User where IsSpier!=1) ");//
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    long Cents = Convert.ToInt64(ds.Tables[0].Rows[i]["Price"]);
                    sum += Cents;
                }
                allpay = sum;
            }
        }
        catch { }
        return allpay;
    }
    //统计每一期中奖总数
    private long GetWincentbySSCId(int SSCId)
    {
        long wincent = 0;
        try
        {
            long sum = new BCW.ssc.BLL.SSCpay().GetSumWinCent(" SSCId=" + SSCId + " and IsSpier!=1  and UsID in (select ID from tb_User where IsSpier!=1) ");
            if (sum > 0)
                wincent = sum;
        }
        catch { }
        return wincent;
    }
}
