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
/// 蒙宗将 20161123 新增  功能
///        20161125 增加每一期每一种玩法ID限额
/// </summary>

public partial class Manage_game_stkguess : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string GameName = ub.GetSub("StkName", "/Controls/stkguess.xml");
    protected string xmlPath = "/Controls/stkguess.xml";

    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "set":
                SetPage();
                break;
            case "open":
                OpenPage();
                break;
            case "opensave":
                OpenSavePage();
                break;
            case "add":
                AddPage();
                break;
            case "addsave":
                AddSavePage();
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
            case "payback":
                PayBackPage();
                break;
            case "del":
                DelPage();
                break;
            case "reset":
                ResetPage();
                break;
            case "top":
                TopPage();//排行榜
                break;
            case "rewards":
                RewardPage();//前十奖励
                break;
            case "stat":
                StatPage();//盈利分析
                break;
            case "back":
                BackPage();//返赢返负
                break;
            case "backsave":
            case "backsave2":
                BackSavePage(act);
                break;
            case "backsave1":
            case "backsave3":
                BackSave1Page(act);
                break;
            case "chaxun":
                ChaxunPage();//期数查询
                break;
            case "usidcx":
                UsidcxPage();//会员查询
                break;
            case "case":
                CasePage();//未/已兑奖
                break;
            case "caseok":
                CaseokPage();//帮他兑奖
                break;
            case "setceshi":
                SetStatueCeshi();//内测管理
                break;
            case "delpay":
                DelpayPage();//退注
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
        builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx") + "\">" + GameName + "</a>");
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
        IList<BCW.Model.Game.Stklist> listStklist = new BCW.BLL.Game.Stklist().GetStklists(pageIndex, pageSize, strWhere, out recordCount);
        if (listStklist.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Stklist n in listStklist)
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
                builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?act=edit&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[管理]&gt;</a>");

                if (n.State == 0)
                    builder.Append("" + DT.FormatDate(n.EndTime, 1) + "开出指数:<a href=\"" + Utils.getUrl("stkguess.aspx?act=view&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">未开</a> <a href=\"" + Utils.getUrl("stkguess.aspx?act=open&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">开奖</a>");
                else
                    builder.Append("" + DT.FormatDate(n.EndTime, 1) + "开出指数:<a href=\"" + Utils.getUrl("stkguess.aspx?act=view&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.WinNum + "</a>");
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
        builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?act=add") + "\">开通下期</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?act=set") + "\">游戏配置</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?act=setceshi") + "\">测试管理</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?act=top") + "\">游戏排行</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?act=chaxun") + "\">游戏查询</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?act=stat") + "\">盈利分析</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?act=back") + "\">返赢返负</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?act=reset") + "\">重置游戏</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

    }
    //游戏管理
    protected void SetPage()
    {
        Master.Title = "" + GameName + "设置";

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx") + "\">" + GameName + "</a>&gt;游戏配置");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-4]$", "0"));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/stkguess.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            if (ptype == 0)
            {
                string Name = Utils.GetRequest("Name", "post", 2, @"^[^\^]{1,20}$", "系统名称限1-20字内");
                string Notes = Utils.GetRequest("Notes", "post", 3, @"^[^\^]{1,50}$", "Stk口号限50字内");
                string Logo = Utils.GetRequest("Logo", "post", 3, @"^[^\^]{1,200}$", "Logo地址限200字内");
                string Status = Utils.GetRequest("Status", "post", 2, @"^[0-2]$", "系统状态选择出错");
                string OpenType = Utils.GetRequest("OpenType", "post", 2, @"^[0-2]$", "开奖类型选择出错");
                string OpenTime = Utils.GetRequest("OpenTime", "post", 2, @"^(?:[0]?\d|1\d|2[0123]):(?:[0-5]\d)$", "每天开奖时间填写错误，正确格式如16:00");
                string Tax = Utils.GetRequest("Tax", "post", 2, @"^[0-9]\d*$", "税率填写错误");
                string SmallPay = Utils.GetRequest("SmallPay", "post", 2, @"^[0-9]\d*$", "最小下注填写错误");
                string BigPay = Utils.GetRequest("BigPay", "post", 2, @"^[0-9]\d*$", "最大下注填写错误");
                string StkMaxpay = Utils.GetRequest("StkMaxpay", "post", 2, @"^[0-9]\d*$", "每ID每期限投下注填写错误");
                string Expir = Utils.GetRequest("Expir", "post", 2, @"^[0-9]\d*$", "防刷秒数填写错误");
                string Rule = Utils.GetRequest("Rule", "post", 3, @"^[\s\S]{1,20000}$", "游戏规则限20000字内");
                string Foot = Utils.GetRequest("Foot", "post", 3, @"^[\s\S]{1,2000}$", "底部Ubb限2000字内");
                string GuestSet = Utils.GetRequest("GuestSet", "post", 2, @"^[0-1]$", "兑奖内线选择出错");

                xml.dss["StkName"] = Name;
                xml.dss["StkNotes"] = Notes;
                xml.dss["StkLogo"] = Logo;
                xml.dss["StkStatus"] = Status;
                xml.dss["StkOpenType"] = OpenType;
                xml.dss["StkOpenTime"] = OpenTime;
                xml.dss["StkTax"] = Tax;
                xml.dss["StkSmallPay"] = SmallPay;
                xml.dss["StkBigPay"] = BigPay;
                xml.dss["StkMaxpay"] = StkMaxpay;
                xml.dss["StkExpir"] = Expir;
                xml.dss["StkRule"] = Rule;
                xml.dss["StkFoot"] = Foot;
                xml.dss["StkGuestSet"] = GuestSet;
            }
            else if (ptype == 1)
            {
                string Odds10 = Utils.GetRequest("Odds10", "post", 2, @"^(0|([1-9]+[0-9]*))(.[0-9]+)?$", "赔率填写错误");
                string Odds11 = Utils.GetRequest("Odds11", "post", 2, @"^(0|([1-9]+[0-9]*))(.[0-9]+)?$", "赔率填写错误");
                string Odds12 = Utils.GetRequest("Odds12", "post", 2, @"^(0|([1-9]+[0-9]*))(.[0-9]+)?$", "赔率填写错误");
                string Odds13 = Utils.GetRequest("Odds13", "post", 2, @"^(0|([1-9]+[0-9]*))(.[0-9]+)?$", "赔率填写错误");
                string Odds14 = Utils.GetRequest("Odds14", "post", 2, @"^(0|([1-9]+[0-9]*))(.[0-9]+)?$", "赔率填写错误");
                string Odds15 = Utils.GetRequest("Odds15", "post", 2, @"^(0|([1-9]+[0-9]*))(.[0-9]+)?$", "赔率填写错误");
                string Odds16 = Utils.GetRequest("Odds16", "post", 2, @"^(0|([1-9]+[0-9]*))(.[0-9]+)?$", "赔率填写错误");
                string Odds17 = Utils.GetRequest("Odds17", "post", 2, @"^(0|([1-9]+[0-9]*))(.[0-9]+)?$", "赔率填写错误");
                string Odds0 = Utils.GetRequest("Odds0", "post", 2, @"^(0|([1-9]+[0-9]*))(.[0-9]+)?$", "赔率填写错误");
                string Odds1 = Utils.GetRequest("Odds1", "post", 2, @"^(0|([1-9]+[0-9]*))(.[0-9]+)?$", "赔率填写错误");
                string Odds2 = Utils.GetRequest("Odds2", "post", 2, @"^(0|([1-9]+[0-9]*))(.[0-9]+)?$", "赔率填写错误");
                string Odds3 = Utils.GetRequest("Odds3", "post", 2, @"^(0|([1-9]+[0-9]*))(.[0-9]+)?$", "赔率填写错误");
                string Odds4 = Utils.GetRequest("Odds4", "post", 2, @"^(0|([1-9]+[0-9]*))(.[0-9]+)?$", "赔率填写错误");
                string Odds5 = Utils.GetRequest("Odds5", "post", 2, @"^(0|([1-9]+[0-9]*))(.[0-9]+)?$", "赔率填写错误");
                string Odds6 = Utils.GetRequest("Odds6", "post", 2, @"^(0|([1-9]+[0-9]*))(.[0-9]+)?$", "赔率填写错误");
                string Odds7 = Utils.GetRequest("Odds7", "post", 2, @"^(0|([1-9]+[0-9]*))(.[0-9]+)?$", "赔率填写错误");
                string Odds8 = Utils.GetRequest("Odds8", "post", 2, @"^(0|([1-9]+[0-9]*))(.[0-9]+)?$", "赔率填写错误");
                string Odds9 = Utils.GetRequest("Odds9", "post", 2, @"^(0|([1-9]+[0-9]*))(.[0-9]+)?$", "赔率填写错误");

                xml.dss["StkOdds10"] = Odds10;
                xml.dss["StkOdds11"] = Odds11;
                xml.dss["StkOdds12"] = Odds12;
                xml.dss["StkOdds13"] = Odds13;
                xml.dss["StkOdds14"] = Odds14;
                xml.dss["StkOdds15"] = Odds15;
                xml.dss["StkOdds16"] = Odds16;
                xml.dss["StkOdds17"] = Odds17;
                xml.dss["StkOdds0"] = Odds0;
                xml.dss["StkOdds1"] = Odds1;
                xml.dss["StkOdds2"] = Odds2;
                xml.dss["StkOdds3"] = Odds3;
                xml.dss["StkOdds4"] = Odds4;
                xml.dss["StkOdds5"] = Odds5;
                xml.dss["StkOdds6"] = Odds6;
                xml.dss["StkOdds7"] = Odds7;
                xml.dss["StkOdds8"] = Odds8;
                xml.dss["StkOdds9"] = Odds9;

            }
            else if (ptype == 2)
            {
                string STKfushu = Utils.GetRequest("Stkfushu", "post", 2, @"^[1-9]\d*$", "多少期开始浮动填写错误");
                string DS1 = Utils.GetRequest("DS1", "post", 2, @"^(0|([1-9]+[0-9]*))(.[0-9]+)?$", "赔率填写错误");
                string DS2 = Utils.GetRequest("DS2", "post", 2, @"^(0|([1-9]+[0-9]*))(.[0-9]+)?$", "赔率填写错误");
                string DS3 = Utils.GetRequest("DS3", "post", 2, @"^(0|([1-9]+[0-9]*))(.[0-9]+)?$", "赔率填写错误");
                string DS4 = Utils.GetRequest("DS4", "post", 2, @"^(0|([1-9]+[0-9]*))(.[0-9]+)?$", "赔率填写错误");
                string DX1 = Utils.GetRequest("DX1", "post", 2, @"^(0|([1-9]+[0-9]*))(.[0-9]+)?$", "赔率填写错误");
                string DX2 = Utils.GetRequest("DX2", "post", 2, @"^(0|([1-9]+[0-9]*))(.[0-9]+)?$", "赔率填写错误");
                string DX3 = Utils.GetRequest("DX3", "post", 2, @"^(0|([1-9]+[0-9]*))(.[0-9]+)?$", "赔率填写错误");
                string DX4 = Utils.GetRequest("DX4", "post", 2, @"^(0|([1-9]+[0-9]*))(.[0-9]+)?$", "赔率填写错误");
                string HDS1 = Utils.GetRequest("HDS1", "post", 2, @"^(0|([1-9]+[0-9]*))(.[0-9]+)?$", "赔率填写错误");
                string HDS2 = Utils.GetRequest("HDS2", "post", 2, @"^(0|([1-9]+[0-9]*))(.[0-9]+)?$", "赔率填写错误");
                string HDS3 = Utils.GetRequest("HDS3", "post", 2, @"^(0|([1-9]+[0-9]*))(.[0-9]+)?$", "赔率填写错误");
                string HDS4 = Utils.GetRequest("HDS4", "post", 2, @"^(0|([1-9]+[0-9]*))(.[0-9]+)?$", "赔率填写错误");
                string HDX1 = Utils.GetRequest("HDX1", "post", 2, @"^(0|([1-9]+[0-9]*))(.[0-9]+)?$", "赔率填写错误");
                string HDX2 = Utils.GetRequest("HDX2", "post", 2, @"^(0|([1-9]+[0-9]*))(.[0-9]+)?$", "赔率填写错误");
                string HDX3 = Utils.GetRequest("HDX3", "post", 2, @"^(0|([1-9]+[0-9]*))(.[0-9]+)?$", "赔率填写错误");
                string HDX4 = Utils.GetRequest("HDX4", "post", 2, @"^(0|([1-9]+[0-9]*))(.[0-9]+)?$", "赔率填写错误");

                string DS = DS1.Replace(" ", "").Replace("|", "") + "|" + DS2.Replace(" ", "").Replace("|", "") + "|" + DS3.Replace(" ", "").Replace("|", "") + "|" + DS4.Replace(" ", "").Replace("|", "");
                string DX = DX1.Replace(" ", "").Replace("|", "") + "|" + DX2.Replace(" ", "").Replace("|", "") + "|" + DX3.Replace(" ", "").Replace("|", "") + "|" + DX4.Replace(" ", "").Replace("|", "");
                string HDS = HDS1.Replace(" ", "").Replace("|", "") + "|" + HDS2.Replace(" ", "").Replace("|", "") + "|" + HDS3.Replace(" ", "").Replace("|", "") + "|" + HDS4.Replace(" ", "").Replace("|", "");
                string HDX = HDX1.Replace(" ", "").Replace("|", "") + "|" + HDX2.Replace(" ", "").Replace("|", "") + "|" + HDX3.Replace(" ", "").Replace("|", "") + "|" + HDX4.Replace(" ", "").Replace("|", "");

                xml.dss["Stkfushu"] = STKfushu.Replace(" ", "");
                xml.dss["StkDS"] = DS;
                xml.dss["StkDX"] = DX;
                xml.dss["StkHDS"] = HDS;
                xml.dss["StkHDX"] = HDX;
            }
            else if (ptype == 3)
            {
                string Toppay0 = Utils.GetRequest("Toppay0", "post", 2, @"^[1-9]\d*$", "投注上限填写错误");
                string Toppay1 = Utils.GetRequest("Toppay1", "post", 2, @"^[1-9]\d*$", "投注上限填写错误");
                string Toppay2 = Utils.GetRequest("Toppay2", "post", 2, @"^[1-9]\d*$", "投注上限填写错误");
                string Toppay3 = Utils.GetRequest("Toppay3", "post", 2, @"^[1-9]\d*$", "投注上限填写错误");
                string Toppay4 = Utils.GetRequest("Toppay4", "post", 2, @"^[1-9]\d*$", "投注上限填写错误");
                string Toppay5 = Utils.GetRequest("Toppay5", "post", 2, @"^[1-9]\d*$", "投注上限填写错误");
                string Toppay6 = Utils.GetRequest("Toppay6", "post", 2, @"^[1-9]\d*$", "投注上限填写错误");
                string Toppay7 = Utils.GetRequest("Toppay7", "post", 2, @"^[1-9]\d*$", "投注上限填写错误");
                string Toppay8 = Utils.GetRequest("Toppay8", "post", 2, @"^[1-9]\d*$", "投注上限填写错误");
                string Toppay9 = Utils.GetRequest("Toppay9", "post", 2, @"^[1-9]\d*$", "投注上限填写错误");
                string Toppay10 = Utils.GetRequest("Toppay10", "post", 2, @"^[1-9]\d*$", "投注上限填写错误");
                string Toppay11 = Utils.GetRequest("Toppay11", "post", 2, @"^[1-9]\d*$", "投注上限填写错误");
                string Toppay12 = Utils.GetRequest("Toppay12", "post", 2, @"^[1-9]\d*$", "投注上限填写错误");
                string Toppay13 = Utils.GetRequest("Toppay13", "post", 2, @"^[1-9]\d*$", "投注上限填写错误");
                string Toppay14 = Utils.GetRequest("Toppay14", "post", 2, @"^[1-9]\d*$", "投注上限填写错误");
                string Toppay15 = Utils.GetRequest("Toppay15", "post", 2, @"^[1-9]\d*$", "投注上限填写错误");
                string Toppay16 = Utils.GetRequest("Toppay16", "post", 2, @"^[1-9]\d*$", "投注上限填写错误");
                string Toppay17 = Utils.GetRequest("Toppay17", "post", 2, @"^[1-9]\d*$", "投注上限填写错误");

                string Toppay = Toppay0.Replace(" ", "").Replace("|", "") + "|" + Toppay1.Replace(" ", "").Replace("|", "") + "|" + Toppay2.Replace(" ", "").Replace("|", "") + "|" + Toppay3.Replace(" ", "").Replace("|", "") + "|" + Toppay4.Replace(" ", "").Replace("|", "") + "|" + Toppay5.Replace(" ", "").Replace("|", "") + "|" + Toppay6.Replace(" ", "").Replace("|", "") + "|" + Toppay7.Replace(" ", "").Replace("|", "") + "|" + Toppay8.Replace(" ", "").Replace("|", "") + "|" + Toppay9.Replace(" ", "").Replace("|", "") + "|" + Toppay10.Replace(" ", "").Replace("|", "") + "|" + Toppay11.Replace(" ", "").Replace("|", "") + "|" + Toppay12.Replace(" ", "").Replace("|", "") + "|" + Toppay13.Replace(" ", "").Replace("|", "") + "|" + Toppay14.Replace(" ", "").Replace("|", "") + "|" + Toppay15.Replace(" ", "").Replace("|", "") + "|" + Toppay16.Replace(" ", "").Replace("|", "") + "|" + Toppay17.Replace(" ", "").Replace("|", "");
                xml.dss["StkToppay"] = Toppay;
            }
            else if (ptype == 4)
            {
                string Toppay0 = Utils.GetRequest("Toppay0", "post", 2, @"^[0-9]\d*$", "ID投注上限填写错误");
                string Toppay1 = Utils.GetRequest("Toppay1", "post", 2, @"^[0-9]\d*$", "ID投注上限填写错误");
                string Toppay2 = Utils.GetRequest("Toppay2", "post", 2, @"^[0-9]\d*$", "ID投注上限填写错误");
                string Toppay3 = Utils.GetRequest("Toppay3", "post", 2, @"^[0-9]\d*$", "ID投注上限填写错误");
                string Toppay4 = Utils.GetRequest("Toppay4", "post", 2, @"^[0-9]\d*$", "ID投注上限填写错误");
                string Toppay5 = Utils.GetRequest("Toppay5", "post", 2, @"^[0-9]\d*$", "ID投注上限填写错误");
                string Toppay6 = Utils.GetRequest("Toppay6", "post", 2, @"^[0-9]\d*$", "ID投注上限填写错误");
                string Toppay7 = Utils.GetRequest("Toppay7", "post", 2, @"^[0-9]\d*$", "ID投注上限填写错误");
                string Toppay8 = Utils.GetRequest("Toppay8", "post", 2, @"^[0-9]\d*$", "ID投注上限填写错误");
                string Toppay9 = Utils.GetRequest("Toppay9", "post", 2, @"^[0-9]\d*$", "ID投注上限填写错误");
                string Toppay10 = Utils.GetRequest("Toppay10", "post", 2, @"^[0-9]\d*$", "ID投注上限填写错误");
                string Toppay11 = Utils.GetRequest("Toppay11", "post", 2, @"^[0-9]\d*$", "ID投注上限填写错误");
                string Toppay12 = Utils.GetRequest("Toppay12", "post", 2, @"^[0-9]\d*$", "ID投注上限填写错误");
                string Toppay13 = Utils.GetRequest("Toppay13", "post", 2, @"^[0-9]\d*$", "ID投注上限填写错误");
                string Toppay14 = Utils.GetRequest("Toppay14", "post", 2, @"^[0-9]\d*$", "ID投注上限填写错误");
                string Toppay15 = Utils.GetRequest("Toppay15", "post", 2, @"^[0-9]\d*$", "ID投注上限填写错误");
                string Toppay16 = Utils.GetRequest("Toppay16", "post", 2, @"^[0-9]\d*$", "ID投注上限填写错误");
                string Toppay17 = Utils.GetRequest("Toppay17", "post", 2, @"^[0-9]\d*$", "ID投注上限填写错误");

                string Toppay = Toppay0.Replace(" ", "").Replace("|", "") + "|" + Toppay1.Replace(" ", "").Replace("|", "") + "|" + Toppay2.Replace(" ", "").Replace("|", "") + "|" + Toppay3.Replace(" ", "").Replace("|", "") + "|" + Toppay4.Replace(" ", "").Replace("|", "") + "|" + Toppay5.Replace(" ", "").Replace("|", "") + "|" + Toppay6.Replace(" ", "").Replace("|", "") + "|" + Toppay7.Replace(" ", "").Replace("|", "") + "|" + Toppay8.Replace(" ", "").Replace("|", "") + "|" + Toppay9.Replace(" ", "").Replace("|", "") + "|" + Toppay10.Replace(" ", "").Replace("|", "") + "|" + Toppay11.Replace(" ", "").Replace("|", "") + "|" + Toppay12.Replace(" ", "").Replace("|", "") + "|" + Toppay13.Replace(" ", "").Replace("|", "") + "|" + Toppay14.Replace(" ", "").Replace("|", "") + "|" + Toppay15.Replace(" ", "").Replace("|", "") + "|" + Toppay16.Replace(" ", "").Replace("|", "") + "|" + Toppay17.Replace(" ", "").Replace("|", "");
                xml.dss["StkPayID"] = Toppay;
            }

            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("" + GameName + "设置", "设置成功，正在返回..", Utils.getUrl("stkguess.aspx?act=set&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            if (ptype == 0)
            {
                builder.Append("上证设置 | ");
                builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?act=set&amp;ptype=1") + "\">赔率</a> | <a href=\"" + Utils.getUrl("stkguess.aspx?act=set&amp;ptype=2") + "\">浮动赔率</a> | <a href=\"" + Utils.getUrl("stkguess.aspx?act=set&amp;ptype=3") + "\">投注上限</a> | <a href=\"" + Utils.getUrl("stkguess.aspx?act=set&amp;ptype=4") + "\">每期每种投注ID上限</a>");
            }
            else if (ptype == 1)
            {
                builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?act=set&amp;ptype=0") + "\">上证设置</a>");
                builder.Append(" | 赔率 | <a href=\"" + Utils.getUrl("stkguess.aspx?act=set&amp;ptype=2") + "\">浮动赔率</a> | <a href=\"" + Utils.getUrl("stkguess.aspx?act=set&amp;ptype=3") + "\">投注上限</a> | <a href=\"" + Utils.getUrl("stkguess.aspx?act=set&amp;ptype=4") + "\">每期每种投注ID上限</a>");
            }
            else if (ptype == 2)
            {
                builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?act=set&amp;ptype=0") + "\">上证设置</a> | ");
                builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?act=set&amp;ptype=1") + "\">赔率</a> | 浮动赔率 | ");
                builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?act=set&amp;ptype=3") + "\">投注上限</a> | <a href=\"" + Utils.getUrl("stkguess.aspx?act=set&amp;ptype=4") + "\">每期每种投注ID上限</a>");
            }
            else if (ptype == 3)
            {
                builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?act=set&amp;ptype=0") + "\">上证设置</a> | ");
                builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?act=set&amp;ptype=1") + "\">赔率</a> | ");
                builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?act=set&amp;ptype=2") + "\">浮动赔率</a>");
                builder.Append(" | 投注上限 | <a href=\"" + Utils.getUrl("stkguess.aspx?act=set&amp;ptype=4") + "\">每期每种投注ID上限</a>");
            }
            else if (ptype == 4)
            {
                builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?act=set&amp;ptype=0") + "\">上证设置</a> | ");
                builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?act=set&amp;ptype=1") + "\">赔率</a> | ");
                builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?act=set&amp;ptype=2") + "\">浮动赔率</a>");
                builder.Append(" | <a href=\"" + Utils.getUrl("stkguess.aspx?act=set&amp;ptype=3") + "\">投注上限</a> | 每期每种投注ID上限");
            }
            // builder.Append("|<a href=\"" + Utils.getUrl("../xml/spkadminset.aspx?ptype=11&amp;backurl=" + Utils.PostPage(true) + "") + "\">管理员</a>");
            builder.Append(Out.Tab("</div>", ""));
            if (ptype == 0)
            {
                string strText = "游戏名称:/,游戏口号(可留空):/,游戏Logo(可留空):/,游戏状态:/,开奖类型:/,每天自动开奖时间:/,系统收税(%):/,最小下注:/,最大下注:/,每期每ID限购多少酷币(填0则不限制):/,下注防刷(秒):/,游戏规则:/,底部Ubb:/,兑奖内线:/,";
                string strName = "Name,Notes,Logo,Status,OpenType,OpenTime,Tax,SmallPay,BigPay,StkMaxpay,Expir,Rule,Foot,GuestSet,backurl";
                string strType = "text,text,text,select,select,text,num,num,num,num,num,textarea,textarea,select,hidden";
                string strValu = "" + xml.dss["StkName"] + "'" + xml.dss["StkNotes"] + "'" + xml.dss["StkLogo"] + "'" + xml.dss["StkStatus"] + "'" + xml.dss["StkOpenType"] + "'" + xml.dss["StkOpenTime"] + "'" + xml.dss["StkTax"] + "'" + xml.dss["StkSmallPay"] + "'" + xml.dss["StkBigPay"] + "'" + xml.dss["StkMaxpay"] + "'" + xml.dss["StkExpir"] + "'" + xml.dss["StkRule"] + "'" + xml.dss["StkFoot"] + "'" + xml.dss["StkGuestSet"] + "'" + Utils.getPage(0) + "";
                string strEmpt = "false,true,true,0|正常|1|维护|2|内测,0|自动开奖|1|手工开奖,false,false,false,false,false,false,false,true,0|开启|1|关闭,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,stkguess.aspx?act=set,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("温馨提示:<br />选择自动开奖则自动抓取指数开奖并自动开通下一期上证.<br />选择手工开奖时请在上证管理中开奖，开奖后请开通新的一期.<br />上证指数一般在15:30就不再变动，故每天自动开奖时间默认为16:00");
                builder.Append(Out.Tab("</div>", ""));
            }
            else if (ptype == 1)
            {
                string strText = "实时单,实时双,实时大,实时小,实时合单,实时合双,实时合大,实时合小,0 ,1 ,2 ,3 ,4 ,5 ,6 ,7 ,8 ,9 ,,";
                string strName = "Odds10,Odds11,Odds12,Odds13,Odds14,Odds15,Odds16,Odds17,Odds0,Odds1,Odds2,Odds3,Odds4,Odds5,Odds6,Odds7,Odds8,Odds9,ptype,backurl";
                string strType = "small,small,small,small,small,small,small,small,small,small,small,small,small,small,small,small,small,small,hidden,hidden";
                string strValu = "" + xml.dss["StkOdds10"] + "'" + xml.dss["StkOdds11"] + "'" + xml.dss["StkOdds12"] + "'" + xml.dss["StkOdds13"] + "'" + xml.dss["StkOdds14"] + "'" + xml.dss["StkOdds15"] + "'" + xml.dss["StkOdds16"] + "'" + xml.dss["StkOdds17"] + "'" + xml.dss["StkOdds0"] + "'" + xml.dss["StkOdds1"] + "'" + xml.dss["StkOdds2"] + "'" + xml.dss["StkOdds3"] + "'" + xml.dss["StkOdds4"] + "'" + xml.dss["StkOdds5"] + "'" + xml.dss["StkOdds6"] + "'" + xml.dss["StkOdds7"] + "'" + xml.dss["StkOdds8"] + "'" + xml.dss["StkOdds9"] + "'1'" + Utils.getPage(0) + "";
                string strEmpt = "false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,stkguess.aspx?act=set,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("温馨提示:<br />赔率支持1-2位小数，如1.85");
                builder.Append(Out.Tab("</div>", ""));
            }
            else if (ptype == 2)
            {
                string STKfushu = ub.GetSub("Stkfushu", xmlPath);
                string DS = ub.GetSub("StkDS", xmlPath); string[] DSf = DS.Split('|');
                string DX = ub.GetSub("StkDX", xmlPath); string[] DXf = DX.Split('|');
                string HDS = ub.GetSub("StkHDS", xmlPath); string[] HDSf = HDS.Split('|');
                string HDX = ub.GetSub("StkHDX", xmlPath); string[] HDXf = HDX.Split('|');
                string DS1 = DSf[0]; string DS2 = DSf[1]; string DS3 = DSf[2]; string DS4 = DSf[3];
                string DX1 = DXf[0]; string DX2 = DXf[1]; string DX3 = DXf[2]; string DX4 = DXf[3];
                string HDS1 = HDSf[0]; string HDS2 = HDSf[1]; string HDS3 = HDSf[2]; string HDS4 = HDSf[3];
                string HDX1 = HDXf[0]; string HDX2 = HDXf[1]; string HDX3 = HDXf[2]; string HDX4 = HDXf[3];

                string strText = "连开多少期浮动（整数）,-------------/单双（固定赔率）,单双（浮动赔率）,单双上限赔率,单双下限赔率,-------------/大小（固定赔率）,大小（浮动赔率）,大小上限赔率,大小下限赔率,-------------/合单双（固定赔率）,合单双（浮动赔率）,合单双上限赔率,合单双下限赔率,-------------/合大小（固定赔率）,合大小（浮动赔率）,合大小上限赔率,合大小下限赔率 ,,";
                string strName = "STKfushu,DS1,DS2,DS3,DS4,DX1,DX2,DX3,DX4,HDS1,HDS2,HDS3,HDS4,HDX1,HDX2,HDX3,HDX4,ptype,backurl";
                string strType = "num,text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,hidden,hidden";
                string strValu = "" + STKfushu + "'" + DS1 + "'" + DS2 + "'" + DS3 + "'" + DS4 + "'" + DX1 + "'" + DX2 + "'" + DX3 + "'" + DX4 + "'" + HDS1 + "'" + HDS2 + "'" + HDS3 + "'" + HDS4 + "'" + HDX1 + "'" + HDX2 + "'" + HDX3 + "'" + HDX4 + "'2'" + Utils.getPage(0) + "";
                string strEmpt = "false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,stkguess.aspx?act=set,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("温馨提示:<br />赔率支持1-2位小数，如1.85");
                builder.Append(Out.Tab("</div>", ""));
            }
            else if (ptype == 3)
            {
                string StkToppay = ub.GetSub("StkToppay", xmlPath);
                string[] toppay = StkToppay.Split('|');
                string strText = "单双额度 ,,大小额度 ,,合单双额度 ,,合大小额度 ,,0 ,1 ,2 ,3 ,4 ,5 ,6 ,7 ,8 ,9 ,,";
                string strName = "Toppay10,Toppay11,Toppay12,Toppay13,Toppay14,Toppay15,Toppay16,Toppay17,Toppay0,Toppay1,Toppay2,Toppay3,Toppay4,Toppay5,Toppay6,Toppay7,Toppay8,Toppay9,ptype,backurl";
                string strType = "num,hidden,num,hidden,num,hidden,num,hidden,num,num,num,num,num,num,num,num,num,num,hidden,hidden";
                string strValu = "" + toppay[10] + "'" + toppay[11] + "'" + toppay[12] + "'" + toppay[13] + "'" + toppay[14] + "'" + toppay[15] + "'" + toppay[16] + "'" + toppay[17] + "'" + toppay[0] + "'" + toppay[1] + "'" + toppay[2] + "'" + toppay[3] + "'" + toppay[4] + "'" + toppay[5] + "'" + toppay[6] + "'" + toppay[7] + "'" + toppay[8] + "'" + toppay[9] + "'3'" + Utils.getPage(0) + "";
                string strEmpt = "false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,stkguess.aspx?act=set,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
            else if (ptype == 4)
            {
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("<f style=\"color:red\">此配置为每一期没一种投注方式每一个用户ID投注限额，填0不限额</f>");
                builder.Append(Out.Tab("</div>", ""));
                string StkToppay = ub.GetSub("StkPayID", xmlPath);
                string[] toppay = StkToppay.Split('|');
                string strText = "竞猜单ID限额 ,竞猜双ID限额 ,竞猜大ID限额 ,竞猜小ID限额 ,竞猜合单ID限额 ,竞猜合双ID限额 ,竞猜合大ID限额 ,竞猜合小ID限额 ,竞猜 0 ID限额 ,竞猜 1 ID限额 ,竞猜 2 ID限额 ,竞猜 3 ID限额 ,竞猜 4 ID限额 ,竞猜 5 ID限额 ,竞猜 6 ID限额 ,竞猜 7 ID限额 ,竞猜 8 ID限额 ,竞猜 9 ID限额 ,,";
                string strName = "Toppay10,Toppay11,Toppay12,Toppay13,Toppay14,Toppay15,Toppay16,Toppay17,Toppay0,Toppay1,Toppay2,Toppay3,Toppay4,Toppay5,Toppay6,Toppay7,Toppay8,Toppay9,ptype,backurl";
                string strType = "num,num,num,num,num,num,num,num,num,num,num,num,num,num,num,num,num,num,hidden,hidden";
                string strValu = "" + toppay[10] + "'" + toppay[11] + "'" + toppay[12] + "'" + toppay[13] + "'" + toppay[14] + "'" + toppay[15] + "'" + toppay[16] + "'" + toppay[17] + "'" + toppay[0] + "'" + toppay[1] + "'" + toppay[2] + "'" + toppay[3] + "'" + toppay[4] + "'" + toppay[5] + "'" + toppay[6] + "'" + toppay[7] + "'" + toppay[8] + "'" + toppay[9] + "'4'" + Utils.getPage(0) + "";
                string strEmpt = "false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,stkguess.aspx?act=set,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }


            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            //  builder.Append("<a href=\"" + Utils.getPage("game.aspx") + "\">返回上级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a><br />");
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
        builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx") + "\">" + GameName + "</a>&gt;测试配置");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b>" + GameName + "内测管理：</b>");
        builder.Append(Out.Tab("</div>", "<br />"));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string StkStatus = Utils.GetRequest("StkStatus", "post", 2, @"^[0-9]\d*$", "测试权限管理输入出错");
            string StkCeshihao = Utils.GetRequest("StkCeshihao", "all", 2, @"^[^\^]{1,2000}$", "请输入测试号");
            xml.dss["StkStatus"] = StkStatus;
            xml.dss["StkCeshihao"] = StkCeshihao.Replace("\r\n", "").Replace(" ", "");
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("" + GameName + "设置", "内测设置成功，正在返回..", Utils.getUrl("stkguess.aspx?act=setceshi"), "2");
        }
        else
        {
            string strText = "测试权限管理:/,添加测试号(多账号用#号分隔):/,";
            string strName = "StkStatus,StkCeshihao,backurl";
            string strType = "select,textarea,hidden";
            string strValu = xml.dss["StkStatus"] + "'" + xml.dss["StkCeshihao"] + "'" + Utils.getPage(0) + "";
            string strEmpt = "0|开放|1|维护|2|内测,true";
            string strIdea = "/";
            string strOthe = "确定修改,stkguess.aspx?act=setceshi,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            string StkCeshihao = Convert.ToString(ub.GetSub("StkCeshihao", xmlPath));
            string[] name = StkCeshihao.Split('#');
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
                string StkRoBotID = Utils.GetRequest("StkRoBotID", "post", 1, "", xml.dss["StkRoBotID"].ToString());
                string StkIsBot = Utils.GetRequest("StkIsBot", "post", 1, @"^[0-1]$", xml.dss["StkIsBot"].ToString());
                string StkRoBotCent = Utils.GetRequest("StkRoBotCent", "post", 1, "", xml.dss["StkRoBotCent"].ToString());
                string StkRoBotbuyCount = Utils.GetRequest("StkRoBotbuyCount", "post", 1, @"^[0-9]\d*$", xml.dss["StkRoBotbuyCount"].ToString());
                xml.dss["StkRoBotID"] = StkRoBotID.Replace("\r\n", "").Replace(" ", "");
                xml.dss["StkIsBot"] = StkIsBot;
                xml.dss["StkRoBotCent"] = StkRoBotCent.Replace("\r\n", "").Replace(" ", "");
                xml.dss["StkRoBotbuyCount"] = StkRoBotbuyCount;
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                Utils.Success("" + GameName + "设置", "机器人管理成功，正在返回..", Utils.getUrl("stkguess.aspx?act=setceshi"), "2");
            }
            else
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<b>机器人管理：</b>");
                builder.Append(Out.Tab("</div>", "<br />"));

                string strText1 = "机器人ID(多个机器人用#号分隔):/,机器人状态:/,机器人投注金额设置:/,机器人每期购买次数:/";
                string strName1 = "StkRoBotID,StkIsBot,StkRoBotCent,StkRoBotbuyCount";
                string strType1 = "textarea,select,text,text";
                string strValu1 = xml.dss["StkRoBotID"].ToString() + "'" + xml.dss["StkIsBot"].ToString() + "'" + xml.dss["StkRoBotCent"].ToString() + "'" + xml.dss["StkRoBotbuyCount"].ToString();
                string strEmpt1 = "true,0|关闭|1|开启,true,false";
                string strIdea1 = "/";
                string strOthe1 = "确定设置,stkguess.aspx?act=setceshi,post,1,red";
                builder.Append(Out.wapform(strText1, strName1, strType1, strValu1, strEmpt1, strIdea1, strOthe1));
                builder.Append(Out.Tab("<div>", "<br />"));

                string StkRoBotIDS = Convert.ToString(ub.GetSub("StkRoBotID", xmlPath));
                string[] name1 = StkRoBotIDS.Split('#');
                string name2 = string.Empty;
                for (int n = 0; n < name1.Length; n++)
                {
                    if ((n + 1) % 5 == 0)
                        name2 = name2 + name1[n] + "," + "<br />";
                    else
                        name2 = name2 + name1[n] + ",";
                }
                builder.Append("当前机器人ID为：<br /><b style=\"color:red\">" + name2 + "</b><br />");
                if (xml.dss["StkIsBot"].ToString() == "0")
                {
                    builder.Append("机器人状态：<b style=\"color:red\">关闭</b><br />");
                }
                else
                {
                    builder.Append("当前机器人状态：<b style=\"color:red\">开启</b><br />");
                }
                builder.Append("当前机器人单注投注金额(随机投注)：<b style=\"color:red\">" + xml.dss["StkRoBotCent"].ToString() + "</b><br />");
                builder.Append("当前机器人每期限购买彩票次数：<b style=\"color:red\">" + xml.dss["StkRoBotbuyCount"].ToString() + "</b><br />");
                builder.Append("<b>温馨提示:请注意内测设置与机器人设置分开设置</b>");
                builder.Append(Out.Tab("</div>", "<br />"));
            }

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("" + "-----------" + "<br />");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx") + "\">返回" + GameName + "管理</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }

    private void OpenPage()
    {
        //int ManageId = new BCW.User.Manage().IsManageLogin();
        //if (ManageId != 1 && ManageId != 9)
        //{
        //    Utils.Error("权限不足", "");
        //}
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
        Master.Title = "" + GameName + "手工开奖";
        BCW.Model.Game.Stklist model = new BCW.BLL.Game.Stklist().GetStklist(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx") + "\">" + GameName + "</a>&gt;" + DT.FormatDate(model.EndTime, 4) + "" + GameName + "开奖");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("第" + DT.FormatDate(model.EndTime, 4) + "" + GameName + "开奖");
        builder.Append(Out.Tab("</div>", ""));

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        if (ac == "")
        {
            if (model.State == 1)
            {
                builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                builder.Append("本期已开奖，要重新开奖吗");
                builder.Append(Out.Tab("</div>", ""));
            }
            string strText = "开出指数:/,,,,";
            string strName = "WinNum,id,act,ac,backurl";
            string strType = "num,hidden,hidden,hidden,hidden";
            string strValu = "" + model.WinNum + "'" + id + "'open'ac'" + Utils.getPage(0) + "";
            string strEmpt = "false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定开奖,stkguess.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        else
        {
            int WinNum = int.Parse(Utils.GetRequest("WinNum", "all", 2, @"^[0-9]{4}$", "开出指数填写错误，格式如2599"));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("确认开出指数：<b style=\"color:red\">" + WinNum + "</b>");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "开出指数:/,,,";
            string strName = "WinNum,id,act,backurl";
            string strType = "hidden,hidden,hidden,hidden";
            string strValu = "" + WinNum + "'" + id + "'opensave'" + Utils.getPage(0) + "";
            string strEmpt = "false,false,false,false";
            string strIdea = "/";
            string strOthe = "确认开奖,stkguess.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }


        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("stkguess.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    private void OpenSavePage()
    {
        //int ManageId = new BCW.User.Manage().IsManageLogin();
        //if (ManageId != 1 && ManageId != 9)
        //{
        //    Utils.Error("权限不足", "");
        //}
        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[1-9]\d*$", "ID错误"));
        int WinNum = int.Parse(Utils.GetRequest("WinNum", "all", 2, @"^[0-9]{4}$", "开出指数填写错误，格式如2599"));
        DateTime EndTime = new BCW.BLL.Game.Stklist().GetEndTime(id);
        if (!new BCW.BLL.Game.Stklist().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        //开奖
        new BCW.User.Game.Stkguess().StkPage(id, EndTime, WinNum, 1);

        Utils.Success("" + DT.FormatDate(EndTime, 4) + "" + GameName + "开奖", "" + DT.FormatDate(EndTime, 4) + "" + GameName + "开奖成功..", Utils.getUrl("stkguess.aspx"), "1");
    }

    private void ViewPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]\d*$", "1"));
        BCW.Model.Game.Stklist model = new BCW.BLL.Game.Stklist().GetStklist(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "" + DT.FormatDate(model.EndTime, 4) + "" + GameName + "";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx") + "\">" + GameName + "</a>&gt;" + DT.FormatDate(model.EndTime, 4) + "" + GameName + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("查看下注/开奖记录");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("查看:");
        if (ptype == 1)
            builder.Append("下注|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?act=view&amp;ptype=1&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">下注</a>|");

        if (ptype == 2)
            builder.Append("中奖");
        else
            builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?act=view&amp;ptype=2&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">中奖</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        if (ptype == 1)
            strWhere += "StkId=" + id + "";
        else
            strWhere += "StkId=" + id + " and state>0 and winCent>0";

        string[] pageValUrl = { "act", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Game.Stkpay> listStkpay = new BCW.BLL.Game.Stkpay().GetStkpays(pageIndex, pageSize, strWhere, out recordCount);
        if (listStkpay.Count > 0)
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("" + DT.FormatDate(model.EndTime, 4) + "开出:<b>" + model.WinNum + "</b>");
            if (ptype == 1)
                builder.Append("<br />共" + recordCount + "注下注");
            else
                builder.Append("<br />共" + recordCount + "注中奖");

            builder.Append(Out.Tab("</div>", "<br />"));

            int k = 1;
            foreach (BCW.Model.Game.Stkpay n in listStkpay)
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
                string bzTypes = string.Empty;
                if (n.bzType == 0)
                    bzTypes = ub.Get("SiteBz");
                else
                    bzTypes = ub.Get("SiteBz2");

                builder.Append("/竞猜:" + OutType(n.Types) + "/下注" + n.BuyCent + "" + bzTypes + "");
                builder.Append("/赔率:" + Convert.ToDouble(n.Odds) + "");

                builder.Append("/结果:");
                if (n.State == 0)
                {
                    builder.Append("未开奖");
                }
                else
                {
                    if (n.WinCent > 0)
                    {
                        builder.Append("赢" + n.WinCent + "" + bzTypes + "");
                    }
                    else
                    {
                        builder.Append("未中");
                    }
                }

                builder.Append("/标识ID:" + n.ID + "[" + DT.FormatDate(n.AddTime, 5) + "]");
                builder.Append(".<a href=\"" + Utils.getUrl("stkguess.aspx?act=delpay&amp;listID=" + model.ID + "&amp;payID=" + n.ID + "&amp;ptype=" + ptype + "") + "\">[退]</a>");
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

        }
        else
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("" + DT.FormatDate(model.EndTime, 4) + "开出:<b>" + model.WinNum + "</b>");
            builder.Append("<br />共0注中奖");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("stkguess.aspx") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    //==========删除一条投注记录==================
    private void DelpayPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "")
        {
            int listID = int.Parse(Utils.GetRequest("listID", "all", 2, @"^[1-9]\d*$", "ID错误"));
            int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]\d*$", "1"));
            int payID = int.Parse(Utils.GetRequest("payID", "all", 1, @"^[0-9]\d*$", "1"));
            BCW.Model.Game.Stkpay n = new BCW.BLL.Game.Stkpay().GetStkpay(payID);
            BCW.Model.Game.Stklist m = new BCW.BLL.Game.Stklist().GetStklist(listID);

            Master.Title = "" + GameName + "第" + DT.FormatDate(m.EndTime, 4) + "期删除一条投注记录";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
            builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx") + "\">" + GameName + "</a>");
            builder.Append("&gt;第" + DT.FormatDate(m.EndTime, 4) + "期");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("确定删除该记录吗?");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("标识ID:" + n.ID + ".<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")</a>");
            string bzTypes = string.Empty;
            if (n.bzType == 0)
                bzTypes = ub.Get("SiteBz");
            else
                bzTypes = ub.Get("SiteBz2");

            builder.Append("/竞猜:" + OutType(n.Types) + "/" + n.BuyCent + "" + bzTypes + "");
            builder.Append("/赔率:" + Convert.ToDouble(n.Odds) + "");

            builder.Append("结果:");
            if (n.State == 0)
            {
                builder.Append("未开奖");
            }
            else
            {
                if (n.WinCent > 0)
                {
                    builder.Append("赢" + n.WinCent + "" + bzTypes + "");
                }
                else
                {
                    builder.Append("未中");
                }
            }

            builder.Append("[" + DT.FormatDate(n.AddTime, 5) + "]");
            builder.Append("<br />");

            if (n.State == 2)
            {
                if (n.WinCent > 0)
                {
                    builder.Append("确认退还删除记录将收回会员下注赢的" + n.WinCent + ub.Get("SiteBz") + ",退还下注" + n.BuyCent + ub.Get("SiteBz") + "<br />");
                }
                else
                {
                    builder.Append("确认退还删除记录将退还会员下注" + n.BuyCent + ub.Get("SiteBz") + "<br />");
                }
            }
            else
            {
                builder.Append("确认退还删除记录将退还会员下注" + n.BuyCent + ub.Get("SiteBz") + "<br />");
            }
            builder.Append(Out.Tab("</div>", ""));

            string strText = ",,退注原因（可为空）:/,,";
            string strName = "listID,payID,text";
            string strType = "hidden,hidden,text";
            string strValu = "" + listID + "'" + payID + "'" + "" + "'" + ptype + "'";
            string strEmpt = "false,false,true";
            string strIdea = "/";
            string strOthe = "确定删除,stkguess.aspx?info=ok&amp;act=delpay,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?act=view&amp;id=" + m.ID + "&amp;ptype=" + ptype + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?act=view&amp;id=" + m.ID + "&amp;ptype=" + ptype + "") + "\">返回上一级</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            int listID = int.Parse(Utils.GetRequest("listID", "all", 2, @"^[1-9]\d*$", "ID错误"));
            int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]\d*$", "1"));
            int payID = int.Parse(Utils.GetRequest("payID", "all", 1, @"^[0-9]\d*$", "1"));
            BCW.Model.Game.Stkpay n = new BCW.BLL.Game.Stkpay().GetStkpay(payID);
            BCW.Model.Game.Stklist m = new BCW.BLL.Game.Stklist().GetStklist(listID);

            string tets = Utils.GetRequest("text", "all", 1, @"^[^\^]{0,20000}$", "");
            string why = string.Empty;
            if (tets != "")
            {
                why = "退注原因：" + tets + "";
            }
            else { why = ""; }

            if (!new BCW.BLL.Game.Stkpay().Exists(n.ID))
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
                    new BCW.BLL.User().UpdateiGold(meid, n.BuyCent, "系统退回" + GameName + "第" + DT.FormatDate(m.EndTime, 4) + "期下注的" + n.BuyCent + "" + ub.Get("SiteBz") + "|标识ID:" + n.ID);//减少系统总的酷币
                    new BCW.BLL.Guest().Add(1, meid, mename, "系统退回您的" + GameName + "：第" + DT.FormatDate(m.EndTime, 4) + "期下注的" + n.BuyCent + "" + ub.Get("SiteBz") + "." + why + "");//
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
                        cMoney = n.WinCent - gold + n.BuyCent;
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
                        owe.Content = "" + GameName + DT.FormatDate(m.EndTime, 4) + "期" + OutType(n.Types) + "下注" + n.BuyCent + "" + ub.Get("SiteBz") + ".系统管理员" + new BCW.User.Manage().IsManageLogin() + "删除.";
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
                    new BCW.BLL.User().UpdateiGold(n.UsID, n.BuyCent - n.WinCent, "系统退回" + GameName + "第" + DT.FormatDate(m.EndTime, 4) + "期下注的" + n.BuyCent + "" + ub.Get("SiteBz") + "." + oop + "" + ui + "|标识ID:" + n.ID);//减少系统总的酷币
                    new BCW.BLL.Guest().Add(1, meid, mename, "系统退回" + GameName + "第" + DT.FormatDate(m.EndTime, 4) + "期下注的" + n.BuyCent + "" + ub.Get("SiteBz") + "." + oop + "" + ui + "" + why + "");//
                }
                ////如果过期不兑奖，退回本金
                //else if (state_get == 3)
                //{
                //    Price = model.PutGold;
                //    new BCW.BLL.User().UpdateiGold(model.UsID, Price, "系统退回新快3第" + model.Lottery_issue + "期未兑奖的" + model.PutGold + "酷币！");//减少系统总的酷币
                //    new BCW.BLL.Guest().Add(1, meid, mename, "系统退回新快3第" + model.Lottery_issue + "期未兑奖的" + model.PutGold + "酷币！");
                //}

                new BCW.BLL.Game.Stkpay().Delete(n.ID);

                Utils.Success("删除投注记录", "删除记录成功..", Utils.getUrl("stkguess.aspx?act=view&amp;id=" + m.ID + "&amp;ptype=" + ptype + ""), "2");

            }
        }
    }

    private void AddPage()
    {
        Master.Title = "开通新一期" + GameName + "";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx") + "\">" + GameName + "</a>&gt;开通新一期" + GameName + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        BCW.Model.Game.Stklist model = new BCW.BLL.Game.Stklist().GetStklist();
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("开通新一期" + GameName + "");
        builder.Append(Out.Tab("</div>", ""));
        if (model.ID != 0)
        {
            Utils.Error("上一期未开奖，不能开通新的一期", "");
        }

        string strText = "开盘时间:/,截止投注时间:/,,";
        string strName = "BeginTime,EndTime,act,backurl";
        string strType = "date,date,hidden,hidden";
        string strValu = "" + DT.FormatDate(DateTime.Now, 0) + "'" + DateTime.Now.AddDays(1).ToString("yyy-MM-dd") + "'addsave'" + Utils.getPage(0) + "";
        string strEmpt = "false,false,false,false";
        string strIdea = "/";
        string strOthe = "确定开通|reset,stkguess.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("stkguess.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void AddSavePage()
    {
        DateTime BeginTime = Utils.ParseTime(Utils.GetRequest("BeginTime", "post", 2, DT.RegexTime, "开盘时间格式填写出错,正确格式如" + DT.FormatDate(DateTime.Now, 0) + ""));
        DateTime EndTime = Utils.ParseTime(Utils.GetRequest("EndTime", "post", 2, DT.RegexDate, "截止投注时间填写出错,正确格式如2011-1-1"));
        if (new BCW.BLL.Game.Stklist().Exists(EndTime))
        {
            Utils.Error("此截止投注日期已在数据库中存在", "");
        }
        BCW.Model.Game.Stklist model = new BCW.Model.Game.Stklist();
        model.EndTime = EndTime;
        model.WinNum = 0;
        model.Pool = 0;
        model.WinPool = 0;
        model.WinCount = 0;
        model.State = 0;
        model.BeginTime = BeginTime;
        DateTime dtTime = Convert.ToDateTime(EndTime.Year + "-" + EndTime.Month + "-" + EndTime.Day + " 14:00:00");
        model.EndTime = dtTime;

        new BCW.BLL.Game.Stklist().Add(model);
        Utils.Success("开通" + GameName + "", "开通" + GameName + "成功..", Utils.getUrl("stkguess.aspx"), "1");
    }

    private void EditPage()
    {
        //int ManageId = new BCW.User.Manage().IsManageLogin();
        //if (ManageId != 1 && ManageId != 9)
        //{
        //Utils.Error("权限不足", "");
        //}
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "上证ID错误"));
        Master.Title = "编辑" + GameName + "";
        BCW.Model.Game.Stklist model = new BCW.BLL.Game.Stklist().GetStklist(id);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx") + "\">" + GameName + "</a>&gt;" + DT.FormatDate(model.EndTime, 4) + "" + GameName + "编辑");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("编辑第" + DT.FormatDate(model.EndTime, 4) + "期" + GameName + "");
        builder.Append(Out.Tab("</div>", ""));
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.State == 0)
        {
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("当开出数字非0时，开奖时则使用该数字作为开奖结果");
            builder.Append(Out.Tab("</div>", ""));
        }
        string strText = "开盘时间:/,截止投注时间:/,,,";
        string strName = "BeginTime,EndTime,id,act,backurl";
        string strType = "date,date,hidden,hidden,hidden";
        string strValu = "" + model.BeginTime + "'" + model.EndTime + "'" + id + "'editsave'" + Utils.getPage(0) + "";
        string strEmpt = "false,false,false,false,false";
        string strIdea = "/";
        string strOthe = "确定编辑|reset,stkguess.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("stkguess.aspx") + "\">返回上一级</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?act=payback&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">退还币币</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">删除本期</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void EditSavePage()
    {
        //int ManageId = new BCW.User.Manage().IsManageLogin();
        //if (ManageId != 1 && ManageId != 9)
        //{
        //  Utils.Error("权限不足", "");
        // }
        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[1-9]\d*$", "ID错误"));
        DateTime BeginTime = Utils.ParseTime(Utils.GetRequest("BeginTime", "post", 2, DT.RegexTime, "开盘时间格式填写出错,正确格式如" + DT.FormatDate(DateTime.Now, 0) + ""));
        DateTime EndTime = Utils.ParseTime(Utils.GetRequest("EndTime", "post", 2, DT.RegexTime, "开奖时间格式填写出错,正确格式如" + DT.FormatDate(DateTime.Now, 0) + ""));

        if (!new BCW.BLL.Game.Stklist().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        BCW.Model.Game.Stklist model = new BCW.Model.Game.Stklist();
        model.ID = id;
        model.BeginTime = BeginTime;
        model.EndTime = EndTime;
        new BCW.BLL.Game.Stklist().Update(model);
        Utils.Success("编辑" + GameName + "", "编辑" + GameName + "成功..", Utils.getUrl("stkguess.aspx?act=edit&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
    }

    private void PayBackPage()
    {
        //int ManageId = new BCW.User.Manage().IsManageLogin();
        //if (ManageId != 1 && ManageId != 9)
        //{
        // Utils.Error("权限不足", "");
        //}
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));

        BCW.Model.Game.Stklist model = new BCW.BLL.Game.Stklist().GetStklist(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.State == 1)
        {
            Utils.Error("已开奖的记录不能退还", "");
        }
        if (info == "")
        {
            Master.Title = "退还上证币币";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定退还" + DT.FormatDate(model.EndTime, 4) + "上证的购买记录吗.此功能将该期的所有下注币币归还到会员上并通过内线通知.");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?info=ok&amp;act=payback&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定退还</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?act=edit&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            int k = 0;
            DataSet ds = new BCW.BLL.Game.Stkpay().GetList("ID,bzType,UsID,UsName,BuyCent", "StkId=" + id + " ORDER BY ID DESC");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    int pid = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                    int bzType = int.Parse(ds.Tables[0].Rows[i]["bzType"].ToString());
                    int UsID = int.Parse(ds.Tables[0].Rows[i]["UsID"].ToString());
                    string UsName = ds.Tables[0].Rows[i]["UsName"].ToString();
                    long BuyCent = Int64.Parse(ds.Tables[0].Rows[i]["BuyCent"].ToString());

                    string bzTypes = string.Empty;
                    if (bzType == 0)
                        bzTypes = ub.Get("SiteBz");
                    else
                        bzTypes = ub.Get("SiteBz2");

                    //删除记录
                    new BCW.BLL.Game.Stkpay().Delete(pid);
                    //操作币
                    new BCW.BLL.User().UpdateiGold(UsID, UsName, BuyCent, "" + GameName + "退还");
                    //发送系统内线
                    new BCW.BLL.Guest().Add(0, UsID, UsName, "因" + GameName + "开奖日期推迟，系统退还您在[URL=/bbs/game/stkguess.aspx]" + GameName + "[/URL]的下注额，共" + BuyCent + "" + bzTypes + "");
                    k++;
                }
            }
            Utils.Success("退还" + DT.FormatDate(model.EndTime, 4) + "上证币", "退还" + k + "份下注成功..", Utils.getPage("stkguess.aspx"), "1");
        }
    }

    private void DelPage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId != 1 && ManageId != 9)
        {
            Utils.Error("权限不足", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Game.Stklist().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        DateTime EndTime = new BCW.BLL.Game.Stklist().GetEndTime(id);
        if (info == "")
        {
            Master.Title = "删除" + DT.FormatDate(EndTime, 4) + "" + GameName + "";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定" + DT.FormatDate(EndTime, 4) + "上证记录吗.删除同时将会删除下注记录.");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?info=ok&amp;act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?act=edit&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            new BCW.BLL.Game.Stklist().Delete(id);
            new BCW.BLL.Game.Stkpay().Delete("StkId=" + id + "");
            Utils.Success("删除" + DT.FormatDate(EndTime, 4) + "" + GameName + "", "删除" + DT.FormatDate(EndTime, 4) + "" + GameName + "成功..", Utils.getPage("stkguess.aspx"), "1");
        }
    }

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
            builder.Append("确定重置" + GameName + "游戏吗，重置后，上证指数记录和下注记录将会全被删除");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?info=ok&amp;act=reset") + "\">确定重置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            new BCW.Data.SqlUp().ClearTable("tb_Stklist");
            new BCW.Data.SqlUp().ClearTable("tb_Stkpay");
            Utils.Success("重置游戏", "重置游戏成功..", Utils.getUrl("stkguess.aspx"), "1");
        }
    }
    private string OutType(int Types)
    {
        string Retn = string.Empty;
        if (Types >= 10)
        {
            if (Types == 10)
                Retn = "单";
            else if (Types == 11)
                Retn = "双";
            else if (Types == 12)
                Retn = "大";
            else if (Types == 13)
                Retn = "小";
            else if (Types == 14)
                Retn = "合单";
            else if (Types == 15)
                Retn = "合双";
            else if (Types == 16)
                Retn = "合大";
            else if (Types == 17)
                Retn = "合小";
        }
        else
        {
            Retn = Types.ToString();
        }
        return Retn;
    }

    /// <summary>
    /// 排行榜
    /// </summary>
    private void TopPage()
    {

        Master.Title = "" + GameName + "排行榜";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx") + "\">" + GameName + "</a>&gt;排行榜");
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
            strWhere = " State>0 ";
            builder.Append("总榜 | <a href=\"" + Utils.getUrl("stkguess.aspx?act=top&amp;ptype=1") + "\">周榜</a> | <a href=\"" + Utils.getUrl("stkguess.aspx?act=top&amp;ptype=2") + "\">日榜</a>");
        }
        else if (ptype == 1)
        {
            strWhere = " datediff(WEEK,AddTime,getdate())=0 and State>0 ";
            builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?act=top&amp;ptype=0") + "\">总榜</a> | 周榜 | <a href=\"" + Utils.getUrl("stkguess.aspx?act=top&amp;ptype=2") + "\">日榜</a>");
        }
        else
        {
            strWhere = " datediff(DAY,AddTime,getdate())=0 and State>0 ";
            builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?act=top&amp;ptype=0") + "\">总榜</a> | <a href=\"" + Utils.getUrl("stkguess.aspx?act=top&amp;ptype=1") + "\">周榜</a> | 日榜");
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        // 开始读取列表
        IList<BCW.Model.Game.Stkpay> listStkpay = new BCW.BLL.Game.Stkpay().GetStkpaysTop(pageIndex, pageSize, strWhere, out recordCount);
        if (listStkpay.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Stkpay n in listStkpay)
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
        string strOthe = "时间查询排行,stkguess.aspx,post,1,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("温馨提示：查询排行可对排行前十用户手动发放奖励<br />");
        builder.Append("<a href=\"" + Utils.getPage("stkguess.aspx") + "\">返回上一级</a><br />");
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
        builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx") + "\">" + GameName + "</a>");
        builder.Append("&gt;排行查询结果");
        builder.Append(Out.Tab("</div>", "<br />"));

        DateTime sTime = Utils.ParseTime(Utils.GetRequest("sTime", "all", 2, DT.RegexTime, "开始时间填写无效"));
        DateTime oTime = Utils.ParseTime(Utils.GetRequest("oTime", "all", 2, DT.RegexTime, "结束时间填写无效"));
        string state = Utils.GetRequest("State", "all", 2, @"^[^\^]{1,5}$", "状态填写无效");

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("" + GameName + sTime + "-" + oTime + "排行榜");
        builder.Append(Out.Tab("</div>", "<br />"));

        string strWheres = string.Empty;
        strWheres += "AddTime > '" + sTime + "' and AddTime <'" + oTime + "' and State>0 group by UsID Order by sum(WinCent-BuyCent) desc";
        DataSet ds = new BCW.BLL.Game.Stkpay().GetList("UsID,sum(WinCent-BuyCent) as WinCent", strWheres);

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


            Utils.Success("排行榜奖励", "排行榜奖励发放成功", Utils.getUrl("stkguess.aspx"), "1");
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
            IList<BCW.Model.Game.Stkpay> listStkpay = new BCW.BLL.Game.Stkpay().GetStkpaysTop(pageIndex, pageSize, strWhere, out recordCount);
            if (listStkpay.Count > 0)
            {
                int k = 1;
                foreach (BCW.Model.Game.Stkpay n in listStkpay)
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
                string strOthe = "发放奖励,stkguess.aspx,post,1,red";

                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append("温馨提示：如果要奖励前十必须全部奖励，奖励值为" + ub.Get("SiteBz") + "<br />");
            builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?act=top") + "\">返回上一级</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        #endregion
    }
    // 盈利分析
    private void StatPage()
    {
        Master.Title = "" + GameName + "赢利分析";

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        string stk = GameName;
        builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx") + "\">" + stk + "</a>&gt;盈利分析");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("=赢利分析=");
        builder.Append(Out.Tab("</div>", "<br />"));

        int tax = Convert.ToInt32(ub.GetSub("StkTax", "/Controls/stkguess.xml"));

        //上期本金与赢利
        long TodayBuyCent = new BCW.BLL.Game.Stkpay().GetPayCentlast();
        long TodayWinCent = new BCW.BLL.Game.Stkpay().GetWinCentlast();
        long TodaySysCent = Convert.ToInt64(TodayBuyCent * (tax * 0.01));


        //近五期本金与赢利
        long TodayBuyCent5 = new BCW.BLL.Game.Stkpay().GetPayCentlast5();
        long TodayWinCent5 = new BCW.BLL.Game.Stkpay().GetWinCentlast5();
        long TodaySysCent5 = Convert.ToInt64(TodayBuyCent5 * (tax * 0.01));


        //总本金与赢利
        long BuyCent = new BCW.BLL.Game.Stkpay().GetPayCent("2000-01-01 12:12:12", "9000-01-01 12:12:12");
        long WinCent = new BCW.BLL.Game.Stkpay().GetWinCent("2000-01-01 12:12:12", "9000-01-01 12:12:12");
        long SysCent = Convert.ToInt64(BuyCent * (tax * 0.01));


        builder.Append(Out.Tab("<div>", ""));
        builder.Append("上期会员下注:" + TodayBuyCent + "" + ub.Get("SiteBz") + "<br />");
        builder.Append("上期游戏返彩:" + TodayWinCent + "" + ub.Get("SiteBz") + "<br />");
        builder.Append("上期系统收税:" + TodaySysCent + ub.Get("SiteBz") + "<br />");
        builder.Append("上期游戏盈利:" + (TodayBuyCent - TodayWinCent + TodaySysCent) + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", Out.Hr()));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("近5期会员下注:" + TodayBuyCent5 + "" + ub.Get("SiteBz") + "<br />");
        builder.Append("近5期游戏返彩:" + TodayWinCent5 + "" + ub.Get("SiteBz") + "<br />");
        builder.Append("近5期系统收税:" + TodaySysCent5 + ub.Get("SiteBz") + "<br />");
        builder.Append("近5期游戏盈利:" + (TodayBuyCent5 - TodayWinCent5 + TodaySysCent5) + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", Out.Hr()));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("会员总下注:" + BuyCent + "" + ub.Get("SiteBz") + "<br />");
        builder.Append("游戏总返彩:" + WinCent + "" + ub.Get("SiteBz") + "<br />");
        builder.Append("系统总收税:" + SysCent + "" + ub.Get("SiteBz") + "<br />");
        builder.Append("游戏总盈利:" + (BuyCent - WinCent + SysCent) + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", Out.Hr()));


        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        if (Utils.ToSChinese(ac) == "马上查询")
        {
            DateTime phase1 = Convert.ToDateTime(Utils.GetRequest("sTime", "all", 1, DT.RegexTime, ""));
            DateTime phase2 = Convert.ToDateTime(Utils.GetRequest("sTime2", "all", 1, DT.RegexTime, ""));

            long q_1 = new BCW.BLL.Game.Stkpay().GetPrice("sum(BuyCent)", "(AddTime between'" + phase1 + "' AND '" + phase2 + "') AND IsSpier=0  and UsID in (select ID from tb_User where IsSpier!=1) ");//消费
            long q_2 = new BCW.BLL.Game.Stkpay().GetPrice("sum(WinCent)", "(AddTime between'" + phase1 + "' AND '" + phase2 + "') AND IsSpier=0  and UsID in (select ID from tb_User where IsSpier!=1) ");//收入

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<hr/>" + phase1 + "到" + phase2 + "<br />下注：" + q_1 + ub.Get("SiteBz") + "<br/>" + "返奖：" + q_2 + ub.Get("SiteBz") + "<br/>" + "收税：" + q_2 * (tax * 0.01) + "<br />");
            builder.Append("系统盈利：" + (q_1 - q_2 + (q_2 * (tax * 0.01))) + ub.Get("SiteBz") + "");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "开始期号：,结束期号：";
            string strName = "sTime,sTime2";
            string strType = "text,text";
            string strValu = "" + phase1.ToString("yyyy-MM-dd HH:mm:ss") + "'" + phase2.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            string strEmpt = "false,false";
            string strIdea = "/";
            string strOthe = "马上查询,stkguess.aspx?act=stat,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        else
        {
            builder.Append(Out.Tab("<div>", "<br/>"));
            builder.Append("<hr/>请输入需要查询的期号：");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "开始时间：,结束时间：";
            string strName = "sTime,sTime2";
            string strType = "text,text";
            string strValu = "" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss") + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            string strEmpt = "false,false";
            string strIdea = "/";
            string strOthe = "马上查询,stkguess.aspx?act=stat,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("说明：盈利分析是不包括测试机器人的已经开奖的记录，盈利=会员投入－会员兑奖+系统收税");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("stkguess.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //反赢反负
    private void BackPage()
    {
        Master.Title = "" + GameName + "返赢返负";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx") + "\">" + GameName + "</a>");
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
        string strOthe = "马上返赢,stkguess.aspx,post,1,red";

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
        strOthe = "马上返负,stkguess.aspx,post,1,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("stkguess.aspx") + "\">返回上一级</a>");
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
            DataSet ds = new BCW.BLL.Game.Stkpay().GetList("UsID,sum(WinCent-BuyCent) as WinCents", "AddTime>='" + sTime + "'and AddTime<='" + oTime + "' group by UsID");
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
                    strLog = "根据你近期" + GameName + "排行榜上的盈利情况，系统自动返还了" + cent + "" + ub.Get("SiteBz") + "[url=/bbs/game/stkguess.aspx]进入" + GameName + "[/url]";

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
            new BCW.BLL.User().UpdateiGold(109, new BCW.BLL.User().GetUsName(109), 0, "" + GameName + sTime + "-" + oTime + "返赢|千分比" + iTar + "|至少赢" + iPrice + "|共计人次" + sumi + "|共返" + sum + "|此记录为返赢记录,不操作109的币数");


            Utils.Success("返赢操作", "返赢操作成功", Utils.getUrl("stkguess.aspx"), "1");
        }
        else
        {
            DataSet ds = new BCW.BLL.Game.Stkpay().GetList("UsID,sum(WinCent-BuyCent) as WinCents", "AddTime>='" + sTime + "'and AddTime<='" + oTime + "' group by UsID");
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
                    strLog = "根据你近期" + GameName + "排行榜上的亏损情况，系统自动返还了" + cent + "" + ub.Get("SiteBz") + "[url=/bbs/game/stkguess.aspx]进入" + GameName + "[/url]";

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
            new BCW.BLL.User().UpdateiGold(109, new BCW.BLL.User().GetUsName(109), 0, "" + GameName + sTime + "-" + oTime + "返负|千分比" + iTar + "|至少负" + iPrice + "|共计人次" + sumi + "|共返" + sum + "|此记录为返负记录,不操作109的币数");

            Utils.Success("返负操作", "返负操作成功", Utils.getUrl("stkguess.aspx"), "1");
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
            DataSet ds = new BCW.BLL.Game.Stkpay().GetList("UsID,sum(WinCent-BuyCent) as WinCents", "AddTime>='" + sTime + "'and AddTime<='" + oTime + "' group by UsID");
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
            string strOthe = "马上返赢,stkguess.aspx,post,1,red";
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
            DataSet ds = new BCW.BLL.Game.Stkpay().GetList("UsID,sum(WinCent-BuyCent) as WinCents", "AddTime>='" + sTime + "'and AddTime<='" + oTime + "' group by UsID ");
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
            string strOthe = "马上返负,stkguess.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("", "<br />"));

        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?act=back") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    /// <summry>
    /// 游戏查询
    /// </summry>
    //查询期号信息（下注与中奖）
    private void ChaxunPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[1-9]\d*$", DateTime.Now.ToString("yyyyMMdd")));
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-2]\d*$", "1"));

        Master.Title = "期号详情";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx") + "\">" + GameName + "</a>&gt;期号详情");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("期号查询|" + "<a href=\"" + Utils.getUrl("stkguess.aspx?act=usidcx") + "\">会员查询</a>" + "|<a href=\"" + Utils.getUrl("stkguess.aspx?act=case") + "\">查兑奖</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("查看下注/中奖记录");
        builder.Append(Out.Tab("</div>", ""));

        DataSet ds = new BCW.BLL.Game.Stklist().GetList("Top(1) EndTime", "State>0 Order by ID DESC");
        DateTime dt = Convert.ToDateTime(ds.Tables[0].Rows[0]["EndTime"]);
        //开始期数
        int number1 = int.Parse(Utils.GetRequest("number1", "all", 1, @"^(\d\d\d\d\d\d\d\d)$", dt.ToString("yyyyMMdd")));

        string strText = "查询期数（期数为2016年的11-17期，查询格式20161117）:/,";
        string strName = "number1,act";
        string strType = "num,hidden";
        string strValu = "" + number1 + "'" + Utils.getPage(0) + "";
        string strEmpt = "true,true,false";
        string strIdea = "/";
        string strOthe = "确认搜索,stkguess.aspx?act=chaxun&amp;ptype=" + ptype + ",post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        string stk = number1.ToString().Substring(0, 4) + "-" + number1.ToString().Substring(4, 2) + "-" + number1.ToString().Substring(6, 2) + " 14:00:00.000";
        int StkId = new BCW.BLL.Game.Stklist().GetIDbyDate(stk);
        BCW.Model.Game.Stklist model = new BCW.BLL.Game.Stklist().GetStklist(StkId);
        BCW.Model.Game.Stkpay nodel = new BCW.BLL.Game.Stkpay().GetStkpaybystkid(StkId);

        id = StkId;

        if (model == null)
        {
            Utils.Error("不存在的记录，输入的期号未开启或者不存在，请检查输入信息是否正确", "");
        }

        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        builder.Append("查看:");
        if (ptype == 1)
            builder.Append("下注|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?act=chaxun&amp;ptype=1&amp;id=" + id + "&amp;number1=" + number1 + "&amp;backurl=" + Utils.getPage(0) + "") + "\">下注</a>|");

        if (ptype == 2)
            builder.Append("中奖");
        else
            builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?act=chaxun&amp;ptype=2&amp;id=" + id + "&amp;number1=" + number1 + "&amp;backurl=" + Utils.getPage(0) + "") + "\">中奖</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        if (ptype == 1)
            strWhere += "StkId=" + model.ID + "";
        else
            strWhere += "StkId=" + model.ID + " and WinCent>0 ";

        string[] pageValUrl = { "act", "id", "ptype", "number1", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        // 开始读取列表
        IList<BCW.Model.Game.Stkpay> listStkpay = new BCW.BLL.Game.Stkpay().GetStkpays(pageIndex, pageSize, strWhere, out recordCount);
        if (listStkpay.Count > 0)
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("第" + model.EndTime.ToString("MM-dd") + "期开出:<b>" + model.WinNum + "</b>");
            if (ptype == 1)
                builder.Append("<br />共" + recordCount + "注下注");
            else
                builder.Append("<br />共" + recordCount + "注中奖");

            builder.Append(Out.Tab("</div>", "<br />"));

            int k = 1;
            foreach (BCW.Model.Game.Stkpay n in listStkpay)
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

                string bzTypes = string.Empty;
                if (n.bzType == 0)
                    bzTypes = ub.Get("SiteBz");
                else
                    bzTypes = ub.Get("SiteBz2");

                builder.Append("/竞猜:" + OutType(n.Types) + "/" + n.BuyCent + "" + bzTypes + "");
                builder.Append("/赔率:" + Convert.ToDouble(n.Odds) + "");

                builder.Append("结果:");
                if (n.State == 0)
                {
                    builder.Append("未开奖");
                }
                else
                {
                    if (n.WinCent > 0)
                    {
                        builder.Append("赢" + n.WinCent + "" + bzTypes + "");
                    }
                    else
                    {
                        builder.Append("未中");
                    }
                }

                builder.Append("/标识ID:" + n.ID + "[" + DT.FormatDate(n.AddTime, 5) + "]");
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
                builder.Append("第" + model.EndTime.ToString("MM-dd") + "期开出:<b>" + model.WinNum + "</b>");
                builder.Append("<br />共0注投注");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Div("div", "没有相关记录..."));
            }
            else if (ptype == 2)
            {
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("第" + model.EndTime.ToString("MM-dd") + "期开出:<b>" + model.WinNum + "</b>");
                builder.Append("<br />共0注中奖");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Div("div", "没有相关记录..."));
            }

        }

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx") + "\">返回上一级</a>");
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
        builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx") + "\">" + GameName + "</a>&gt;会员查询");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?act=chaxun") + "\">期号查询</a>" + "|会员查询" + "|<a href=\"" + Utils.getUrl("stkguess.aspx?act=case") + "\">查兑奖</a>");
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
        string strOthe = "确认搜索,stkguess.aspx?act=usidcx&amp;ptype=" + ptype + ",post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));


        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        builder.Append("查看:");
        if (ptype == 1)
            builder.Append("下注|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?act=usidcx&amp;ptype=1&amp;UsID=" + UsID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">下注</a>|");

        if (ptype == 2)
            builder.Append("中奖");
        else
            builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?act=usidcx&amp;ptype=2&amp;UsID=" + UsID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">中奖</a>");

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
        IList<BCW.Model.Game.Stkpay> listStkpay = new BCW.BLL.Game.Stkpay().GetStkpays(pageIndex, pageSize, strWhere, out recordCount);
        if (listStkpay.Count > 0)
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
            foreach (BCW.Model.Game.Stkpay n in listStkpay)
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

                string bzTypes = string.Empty;
                if (n.bzType == 0)
                    bzTypes = ub.Get("SiteBz");
                else
                    bzTypes = ub.Get("SiteBz2");

                DateTime endtime = new BCW.BLL.Game.Stklist().GetEndTime(n.StkId);

                builder.Append("/" + endtime.ToString("MM-dd") + "期竞猜:" + OutType(n.Types) + "/" + n.BuyCent + "" + bzTypes + "");
                builder.Append("/赔率:" + Convert.ToDouble(n.Odds) + "");

                builder.Append("结果:");
                if (n.State == 0)
                {
                    builder.Append("未开奖");
                }
                else
                {
                    if (n.WinCent > 0)
                    {
                        builder.Append("赢" + n.WinCent + "" + bzTypes + "");
                    }
                    else
                    {
                        builder.Append("未中");
                    }
                }

                builder.Append("/标识ID:" + n.ID + "[" + DT.FormatDate(n.AddTime, 5) + "]");
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
        builder.Append("<a href=\"" + Utils.getPage("stkguess.aspx") + "\">返回上一级</a>");
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
        builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx") + "\">" + GameName + "</a>");
        builder.Append("&gt;兑奖查看");
        builder.Append(Out.Tab("</div>", "<br />"));

        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-1]$", "0"));
        //用户ID
        int usid = int.Parse(Utils.GetRequest("usid", "all", 1, @"^[1-9]\d*$", "0"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?act=chaxun") + "\">期号查询</a>" + "|<a href=\"" + Utils.getUrl("stkguess.aspx?act=usidcx") + "\">会员查询</a>" + "|查兑奖");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("〓查兑奖〓");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        if (ptype == 0)
        {
            builder.Append("未兑奖 | <a href=\"" + Utils.getUrl("stkguess.aspx?act=case&amp;ptype=1&amp;usid=" + usid + "") + "\">已兑奖</a>");
        }
        else
        {
            builder.Append(" <a href=\"" + Utils.getUrl("stkguess.aspx?act=case&amp;ptype=0&amp;usid=" + usid + "") + "\">未兑奖</a> | 已兑奖");
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
        string Stkqi = "";
        // 开始读取列表
        IList<BCW.Model.Game.Stkpay> listStkpay = new BCW.BLL.Game.Stkpay().GetStkpays(pageIndex, pageSize, strWhere, out recordCount);
        if (listStkpay.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Stkpay n in listStkpay)
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
                if (n.StkId.ToString() != Stkqi)
                {
                    DateTime endtime = new BCW.BLL.Game.Stklist().GetEndTime(n.StkId);
                    if (n.WinNum == 0)
                        builder.Append("=第" + endtime.ToString("MM-dd") + "期=<br />");
                    else
                        builder.Append("=第" + endtime.ToString("MM-dd") + "期=开出:<f style=\"color:red\">" + n.WinNum + "</f><br />");
                }

                builder.Append(Out.Tab("<div>", ""));
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>");

                string bzTypes = string.Empty;
                if (n.bzType == 0)
                    bzTypes = ub.Get("SiteBz");
                else
                    bzTypes = ub.Get("SiteBz2");

                DateTime endtime1 = new BCW.BLL.Game.Stklist().GetEndTime(n.StkId);

                builder.Append("/" + endtime1.ToString("MM-dd") + "期竞猜:" + OutType(n.Types) + "/" + n.BuyCent + "" + bzTypes + "");
                builder.Append("/赔率:" + Convert.ToDouble(n.Odds) + "");

                builder.Append("结果:");
                if (n.State == 0)
                {
                    builder.Append("未开奖");
                }
                else
                {
                    if (n.WinCent > 0)
                    {
                        builder.Append("赢" + n.WinCent + "" + bzTypes + "");
                    }
                    else
                    {
                        builder.Append("未中");
                    }
                }

                builder.Append("/标识ID:" + n.ID + "[" + DT.FormatDate(n.AddTime, 5) + "]");
                if (n.WinCent > 0)
                {
                    if (ptype == 0)
                    {
                        builder.Append("<f style=\"color:blue\"><a href=\"" + Utils.getUrl("stkguess.aspx?act=caseok&amp;id=" + n.ID + "") + "\">帮他兑奖</a></f>");
                    }
                    else
                    {
                        builder.Append("<f style=\"color:black\">已兑奖</f>");
                    }
                }
                builder.Append(Out.Tab("</div>", ""));
                Stkqi = n.StkId.ToString();
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
        string strOthe = "确认搜索,stkguess.aspx?act=case&amp;ptype=" + ptype + ",post,1,red";
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
        builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx") + "\">" + GameName + "</a>");
        builder.Append("&gt;帮他兑奖");
        builder.Append(Out.Tab("</div>", "<br />"));

        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[1-9]\d*$", "0"));
        string info = Utils.GetRequest("info", "all", 1, "", "");
        BCW.Model.Game.Stkpay n = new BCW.BLL.Game.Stkpay().GetStkpay(id);
        DateTime endtime = new BCW.BLL.Game.Stklist().GetEndTime(n.StkId);
        string bzTypes = string.Empty;
        if (n.bzType == 0)
            bzTypes = ub.Get("SiteBz");
        else
            bzTypes = ub.Get("SiteBz2");

        if (info == "ok")
        {
            if (new BCW.BLL.Game.Stkpay().ExistsState(id, n.UsID))
            {
                int guestset = Utils.ParseInt(ub.GetSub("GuestSet", xmlPath));
                new BCW.BLL.Game.Stkpay().UpdateState(id);

                BCW.User.Users.IsFresh("stk", 1);//防刷
                BCW.Model.Game.Stklist idd = new BCW.BLL.Game.Stklist().GetStklist(n.StkId);

                //操作币
                long win = n.WinCent;
                //税率
                long SysTax = 0;
                int Tax = Utils.ParseInt(ub.GetSub("StkTax", xmlPath));
                if (Tax > 0)
                {
                    SysTax = Convert.ToInt64(win * Tax * 0.01);
                }
                win = win - SysTax;

                if (n.bzType == 0)
                {
                    new BCW.BLL.User().UpdateiGold(n.UsID, new BCW.BLL.User().GetUsName(n.UsID), win, "" + GameName + "兑奖-" + "[url=./game/stkguess.aspx?act=view&amp;id=" + idd.ID + "&amp;ptype=2]" + endtime.ToString("MM-dd") + "[/url]" + "-标识ID" + n.ID + "");
                    if (new BCW.BLL.User().GetIsSpier(n.UsID) != 1)
                        new BCW.BLL.User().UpdateiGold(109, new BCW.BLL.User().GetUsName(109), -win, "ID:[url=forumlog.aspx?act=xview&amp;uid=" + n.UsID + "]" + n.UsID + "[/url]" + GameName + "第[url=./game/stkguess.aspx?act=view&amp;id=" + idd.ID + "&amp;ptype=2]" + endtime.ToString("MM-dd") + "[/url]期兑奖" + win + "|标识ID" + n.ID + "");
                    if (guestset == 0)
                        new BCW.BLL.Guest().Add(1, n.UsID, BCW.User.Users.SetUser(n.UsID), "您在[URL=/bbs/game/stkguess.aspx]" + GameName + "[/URL]第" + endtime.ToString("MM-dd") + "期竞猜" + OutType(n.Types) + "系统已经帮您兑奖，获得了" + win + ub.Get("SiteBz") + "。");//开奖提示信息,1表示开奖信息

                }
                else
                {
                    new BCW.BLL.User().UpdateiMoney(n.UsID, new BCW.BLL.User().GetUsName(n.UsID), win, "" + GameName + "兑奖-" + "[url=./game/stkguess.aspx?act=view&amp;id=" + idd.ID + "&amp;ptype=2]" + endtime.ToString("MM-dd") + "[/url]" + "-标识ID" + n.ID + "");
                    if (new BCW.BLL.User().GetIsSpier(n.UsID) != 1)
                        new BCW.BLL.User().UpdateiMoney(109, new BCW.BLL.User().GetUsName(109), -win, "ID:[url=forumlog.aspx?act=xview&amp;uid=" + n.UsID + "]" + n.UsID + "[/url]" + GameName + "第[url=./game/stkguess.aspx?act=view&amp;id=" + idd.ID + "&amp;ptype=2]" + endtime.ToString("MM-dd") + "[/url]期兑奖" + win + "|标识ID" + n.ID + "");
                    if (guestset == 0)
                        new BCW.BLL.Guest().Add(1, n.UsID, BCW.User.Users.SetUser(n.UsID), "您在[URL=/bbs/game/stkguess.aspx]" + GameName + "[/URL]第" + endtime.ToString("MM-dd") + "期竞猜" + OutType(n.Types) + "系统已经帮您兑奖，获得了" + win + ub.Get("SiteBz2") + "。");//开奖提示信息,1表示开奖信息

                }

                Utils.Success("兑奖", "恭喜，成功帮他兑奖" + win + "" + ub.Get("SiteBz") + "", Utils.getUrl("stkguess.aspx?act=case&amp;ptype=0"), "1");
            }
            else
            {
                Utils.Success("兑奖", "重复兑奖或没有可以兑奖的记录", Utils.getUrl("stkguess.aspx?act=case&amp;ptype=0"), "2");
            }
        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("=第" + endtime.ToString("MM-dd") + "期=开出:<f style=\"color:red\">" + n.WinNum + "</f><br />");

            builder.Append("用户：<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(n.UsID) + " (" + n.UsID + ")" + "</a><br /> ");
            builder.Append("兑奖ID：" + id + "<br />竞猜：<b>" + OutType(n.Types) + "</b>" + "<br />投注：" + n.BuyCent + "" + ub.Get("SiteBz") + "<br />赔率：" + n.Odds + "<br />投注时间：" + DT.FormatDate(n.AddTime, 1) + "<br />");

            if (n.WinCent > 0)
            {
                builder.Append("结果：赢" + n.WinCent + "" + bzTypes + "");

                builder.Append("<br /><a href=\"" + Utils.getUrl("stkguess.aspx?act=caseok&amp;info=ok&amp;id=" + n.ID + "") + "\">确定帮他兑奖</a>");
            }
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
}
