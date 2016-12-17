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
using System.Text.RegularExpressions;
using BCW.Common;

/// <summary>
/// 球赛确认设置 黄国军 20161128
/// 超额提醒设置 黄国军 20161122
/// 增加数据来源设置 黄国军 20161107
/// 修正足篮球限制下注配置 黄国军 20161105
/// 修改抓取配置地址和内容 取消刷新机独立的文件读取 黄国军 20160929
/// 修改更新代理模式,取消刷新机模式 黄国军 20160509
/// 增加 是否开启实时波胆更新 选项 黄国军 20160227
/// 增加 自定义滚球下注确认时间 黄国军 20160323
/// 增加新旧版控制 xml控制 姚志光 20160910
/// 邵广林 20160910 增加getip的xml字段为SiteViewStatus_1
/// </summary>
public partial class Manage_xml_guessset2 : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "球彩系统设置";
        string act = Utils.GetRequest("act", "all", 1, "", "");

        switch (act)
        {
            case "betleven":            //足球分级
                BetLevenPage();
                break;
            case "betleven2":           //篮球分级
                BetLeven2Page();
                break;
            case "betleven3":           //足球半场
                BetLeven3Page();
                break;
            case "betleven4":           //篮球半场单节
                BetLeven4Page();
                break;
            case "score":               //波胆配置页
                ScorePage();
                break;
            case "ubb":
                UbbPage();
                break;
            case "guessjc":             //球赛联赛抓取配置
                GuessjcPage();
                break;
            case "source":              //数据抓取来源配置
                SourcePage();
                break;
            case "overcall":            //超额提醒设置
                OverCallPage();
                break;
            case "confirmset":          //确认设置
                ConfirmSetPage();
                break;
            default:                    //球赛设置
                ReloadPage();
                break;
        }
    }

    #region 球赛设置 0 ReloadPage
    /// <summary>
    /// 球赛设置
    /// </summary>
    private void ReloadPage()
    {
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/guess2.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            #region 保存变量
            string Name = Utils.GetRequest("Name", "post", 2, @"^[\u4e00-\u9fa5A-Za-z0-9]{1,20}$", "系统名称应为中文、英文、数字的组合,长度限20字内");
            string Logo = Utils.GetRequest("Logo", "post", 3, @"^[\s\S]{0,500}$", "Logo地址不能超过500字");
            string BigPay = Utils.GetRequest("BigPay", "post", 2, @"^[0-9]\d*$", "最大下注数量填写错误");
            string SmallPay = Utils.GetRequest("SmallPay", "post", 2, @"^[0-9]\d*$", "最小下注填写错误");
            string PayNum = Utils.GetRequest("PayNum", "post", 2, @"^\d+?$", "每场每项下注次数填写错误");
            string MaxNum = Utils.GetRequest("MaxNum", "post", 2, @"^\d+?$", "每场下注上限填写错误，不限制请填写0");
            string MaxNumGid = Utils.GetRequest("MaxNumGid", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,1000}$", "每场下注上限赛事ID填写错误");
            string vice = Utils.GetRequest("vice", "post", 3, @"^[\s\S]{1,2000}$", "顶部Ubb不能超过2000字");
            string foot = Utils.GetRequest("foot", "post", 3, @"^[\s\S]{1,2000}$", "底部Ubb不能超过2000字");

            //string Isbz = Utils.GetRequest("Isbz", "post", 2, @"^[0-1]$", "是否开标准盘选择错误");
            //string Isyc = Utils.GetRequest("Isyc", "post", 2, @"^[0-1]$", "抓取方式选择错误");
            //string zqstat = Utils.GetRequest("zqstat", "post", 3, @"^[(\u4e00-\u9fa5A-Za-z0-9)# -]+$", "联赛名称必须简体中文或者英文、数字，并用#分开"));
            //string lqstat = Utils.GetRequest("lqstat", "post", 3, @"^[(\u4e00-\u9fa5A-Za-z0-9)# -]+$", "联赛名称必须简体中文或者英文、数字，并用#分开"));
            //string gqstat = Utils.GetRequest("gqstat", "post", 3, @"^[(\u4e00-\u9fa5A-Za-z0-9)# -]+$", "联赛名称必须简体中文或者英文、数字，并用#分开"));
            //string gqstat2 = Utils.GetRequest("gqstat2", "post", 3, @"^[(\u4e00-\u9fa5A-Za-z0-9)# -]+$", "联赛名称必须简体中文或者英文、数字，并用#分开"));

            string Expir = Utils.GetRequest("Expir", "post", 2, @"^[0-9]\d*$", "防刷秒数填写错误");

            string BigPay2 = Utils.GetRequest("BigPay2", "post", 2, @"^[0-9]\d*$", "串串最大下注数量填写错误");
            string SmallPay2 = Utils.GetRequest("SmallPay2", "post", 2, @"^[0-9]\d*$", "串串最小下注填写错误");
            string LostPrice = Utils.GetRequest("LostPrice", "post", 2, @"^[0-9]\d*$", "每期输多少作为返币填写错误");
            string LostPL = Utils.GetRequest("LostPL", "post", 2, @"^[0-9]\d*$", "返输币百分比填写错误,必须为整数");
            string onceType = Utils.GetRequest("onceType", "post", 2, @"^[0-1]$", "走地返彩方式选择错误");
            string Once = Utils.GetRequest("Once", "post", 2, @"^[0-9]\d*$", "走地平盘前后秒钟数填写错误");
            string Stemp = Utils.GetRequest("Stemp", "post", 2, @"^[0-9]\d*$", "走地比分变化后N秒钟可下注填写错误");
            string Stempb = Utils.GetRequest("Stempb", "post", 2, @"^[0-9]\d*$", "走地比分变化后N秒钟可下注填写错误");
            string zdType = Utils.GetRequest("zdType", "post", 2, @"^[0-1]$", "抓取走地选择错误");
            string SmallPay3 = Utils.GetRequest("SmallPay3", "post", 2, @"^[0-9]\d*$", "最小下注填写错误");
            string BigPay3 = Utils.GetRequest("BigPay3", "post", 2, @"^[0-9]\d*$", "最大下注填写错误");
            string IDMaxPay = Utils.GetRequest("IDMaxPay", "post", 2, @"^[0-9]\d*$", "每ID每场最大下注填写错误");
            //-------------------------------------------------------------------
            string checkgidsec = Utils.GetRequest("checkgidsec", "post", 2, @"^[0-9]\d*$", "中断N秒则隐藏填写错误");
            string checkgid = Utils.GetRequest("checkgid", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,1000}$", "指定参照赛事ID填写错误");
            xml.dss["Sitecheckgidsec"] = checkgidsec;
            xml.dss["Sitecheckgid"] = checkgid;
            xml.dss["SitecheckgidID"] = "";
            //-------------------------------------------------------------------

            string SuperVoteSec = Utils.GetRequest("SuperVoteSec", "post", 2, @"^[0-9]\d*$", "串关选择后限时投注(秒)填写错误");
            xml.dss["SiteSuperVoteSec"] = SuperVoteSec;
            string SuperSubCent = Utils.GetRequest("SuperSubCent", "post", 2, @"^[0-9]\d*$", "串关同一场同一盘口每ID下注上限填写错误");
            xml.dss["SiteSuperSubCent"] = SuperSubCent;
            string xCent = Utils.GetRequest("xCent", "post", 2, @"^[0-9]\d*$", "最低多少币才能开庄填写错误");
            xml.dss["SitexCent"] = xCent;
            string txIDS = Utils.GetRequest("txIDS", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,1000}$", "特别检查ID填写错误");
            string rqPCent = Utils.GetRequest("rqPCent", "post", 3, @"^[0-9]\d*$", "篮球让球盘限额比例填写错误");
            string dxPCent = Utils.GetRequest("dxPCent", "post", 3, @"^[0-9]\d*$", "篮球大小盘限额比例填写错误");
            string IsSiteView = Utils.GetRequest("IsSiteView", "post", 3, @"^[\s\S]{1,2000}$", "代理填写错误");
            string IsSiteView_1 = Utils.GetRequest("IsSiteView_1", "post", 3, @"^[\s\S]{1,2000}$", "代理2填写错误");
            string zdIDS = Utils.GetRequest("zdIDS", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,1000}$", "自定义的下注ID填写错误");
            string Zdtime = Utils.GetRequest("Zdtime", "post", 2, @"^[0-9]\d*$", "自定义滚球下注确认时间(秒)填写错误");
            string SiteUpdateOpen = Utils.GetRequest("SiteUpdateOpen", "post", 2, @"^[0-9]\d*$", "新旧刷新机选着错误");


            xml.dss["SiteZdtime"] = Zdtime;
            xml.dss["SitezdIDS"] = zdIDS;
            xml.dss["SitetxIDS"] = txIDS;
            xml.dss["SiteRQPCent"] = rqPCent;
            xml.dss["SiteDXPCent"] = dxPCent;
            //写入XML
            xml.dss["SiteName"] = Name;
            xml.dss["SiteLogo"] = Logo;
            xml.dss["SiteBigPay"] = BigPay;
            xml.dss["SiteSmallPay"] = SmallPay;
            xml.dss["SitePayNum"] = PayNum;
            xml.dss["SiteMaxNum"] = MaxNum;
            xml.dss["SiteMaxNumGid"] = MaxNumGid;
            xml.dss["Sitevice"] = vice;
            xml.dss["SiteFoot"] = foot;
            //xml.dss["SiteIsbz"] = Isbz;
            //xml.dss["SiteIsyc"] = Isyc;
            //xml.dss["Sitezqstat"] = zqstat;
            //xml.dss["Sitelqstat"] = lqstat;
            //xml.dss["Sitegqstat"] = gqstat;
            //xml.dss["Sitegqstat2"] = gqstat2;
            xml.dss["SiteExpir"] = Expir;
            xml.dss["SiteBigPay2"] = BigPay2;
            xml.dss["SiteSmallPay2"] = SmallPay2;
            xml.dss["SiteLostPrice"] = LostPrice;
            xml.dss["SiteLostPL"] = LostPL;
            xml.dss["SiteonceType"] = onceType;
            xml.dss["SiteOnce"] = Once;
            xml.dss["SiteStemp"] = Stemp;
            xml.dss["SiteStempb"] = Stempb;
            xml.dss["SitezdType"] = zdType;
            xml.dss["SiteSmallPay3"] = SmallPay3;
            xml.dss["SiteBigPay3"] = BigPay3;
            xml.dss["SiteIDMaxPay"] = IDMaxPay;
            xml.dss["SiteViewStatus"] = IsSiteView;
            xml.dss["SiteUpdateOpen"] = SiteUpdateOpen;
            xml.dss["SiteViewStatus_1"] = IsSiteView_1;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("球彩系统设置", "设置成功，正在返回..", Utils.getUrl("guessset2.aspx?backurl=" + Utils.getPage(0) + ""), "1");
            #endregion
        }
        else
        {
            #region 输入表单
            builder.Append(Out.Div("title", "球彩系统设置"));
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            GuessSetNav(0);
            builder.Append(Out.Tab("</div>", ""));
            //string strText = "*系统名称:/,系统LOGO:/,最小下注:/,最大下注:/,每场每项每ID下注限次数:(0为不限制)/,每场下注上限(0为不限制):/,每场下注上限赛事ID:/,下注防刷秒数:/,顶部Ubb:/,底部Ubb:/,是否开标准盘:/,抓取方式:/,要开的足球联赛:/,要开的篮球联赛:/,要开的足球" + ub.Get("SiteGqText") + "联赛:/,要开的篮球" + ub.Get("SiteGqText") + "联赛:/,以下豪华版才有的功能/最小下注(串串):/,最大下注(串串):/,每期输多少作为返币:/,返输币百分比%:/,走地返彩方式:/,走地平盘前后秒钟:/,走地比分变化后N秒钟可下注:/,抓取走地,";
            //string strName = "Name,Logo,SmallPay,BigPay,PayNum,MaxNum,MaxNumGid,Expir,vice,foot,Isbz,Isyc,zqstat,lqstat,gqstat,gqstat2,SmallPay2,BigPay2,LostPrice,LostPL,onceType,Once,Stemp,zdType,backurl";
            //string strType = "text,text,num,num,num,num,text,num,text,textarea,select,select,textarea,textarea,textarea,textarea,num,num,num,num,select,num,num,select,hidden";
            //string strValu = "" + xml.dss["SiteName"] + "'" + xml.dss["SiteLogo"] + "'" + xml.dss["SiteSmallPay"] + "'" + xml.dss["SiteBigPay"] + "'" + xml.dss["SitePayNum"] + "'" + xml.dss["SiteMaxNum"] + "'" + xml.dss["SiteMaxNumGid"] + "'" + xml.dss["SiteExpir"] + "'" + xml.dss["Sitevice"] + "'" + xml.dss["SiteFoot"] + "'" + xml.dss["SiteIsbz"] + "'" + xml.dss["SiteIsyc"] + "'" + xml.dss["Sitezqstat"] + "'" + xml.dss["Sitelqstat"] + "'" + xml.dss["Sitegqstat"] + "'" + xml.dss["Sitegqstat2"] + "'" + xml.dss["SiteSmallPay2"] + "'" + xml.dss["SiteBigPay2"] + "'" + xml.dss["SiteLostPrice"] + "'" + xml.dss["SiteLostPL"] + "'" + xml.dss["SiteonceType"] + "'" + xml.dss["SiteOnce"] + "'" + xml.dss["SiteStemp"] + "'" + xml.dss["SitezdType"] + "'" + Utils.getPage(0) + "";
            //string strEmpt = "false,true,false,false,false,false,true,false,true,true,0|开启|1|关闭,0|抓取即显示|1|抓取先隐藏,true,true,true,true,false,false,true,true,0|自动开奖|1|手工开奖,false,false,0|开启抓取|1|关闭抓取,false";
            //string strIdea = "/温馨提示:/每场下注上限赛事ID用#分开，留空则全部赛事有效/联赛名称必须简体中文或者英文、数字，并用#分开/";
            //string strOthe = "确定修改,guessset2.aspx,post,1,red";

            string strText = "*系统名称:/,系统LOGO:/,最小下注:/,最大下注:/,每场每项每ID下注限次数:(0为不限制)/,每场下注上限(0为不限制):/,每场下注上限赛事ID:/,下注防刷秒数:/,顶部Ubb:/,底部Ubb:/,最小下注(串串):/,最大下注(串串):/,每期输多少作为返币:/,返输币百分比%:/," + ub.Get("SiteGqText") + "返彩方式:/," + ub.Get("SiteGqText") + "平盘前后秒钟:/,足球" + ub.Get("SiteGqText") + "比分变化后N秒钟可下注:/,篮球" + ub.Get("SiteGqText") + "比分变化后N秒钟可下注:/,抓取" + ub.Get("SiteGqText") + ",";
            string strName = "Name,Logo,SmallPay,BigPay,PayNum,MaxNum,MaxNumGid,Expir,vice,foot,SmallPay2,BigPay2,LostPrice,LostPL,onceType,Once,Stemp,Stempb,zdType,backurl";
            string strType = "text,text,num,num,num,num,text,num,text,textarea,num,num,num,num,select,num,num,num,select,hidden";
            string strValu = "" + xml.dss["SiteName"] + "'" + xml.dss["SiteLogo"] + "'" + xml.dss["SiteSmallPay"] + "'" + xml.dss["SiteBigPay"] + "'" + xml.dss["SitePayNum"] + "'" + xml.dss["SiteMaxNum"] + "'" + xml.dss["SiteMaxNumGid"] + "'" + xml.dss["SiteExpir"] + "'" + xml.dss["Sitevice"] + "'" + xml.dss["SiteFoot"] + "'" + xml.dss["SiteSmallPay2"] + "'" + xml.dss["SiteBigPay2"] + "'" + xml.dss["SiteLostPrice"] + "'" + xml.dss["SiteLostPL"] + "'" + xml.dss["SiteonceType"] + "'" + xml.dss["SiteOnce"] + "'" + xml.dss["SiteStemp"] + "'" + xml.dss["SiteStempb"] + "'" + xml.dss["SitezdType"] + "'" + Utils.getPage(0) + "";
            string strEmpt = "false,true,false,false,false,false,true,false,true,true,false,false,true,true,0|自动开奖|1|手工开奖,false,false,false,0|开启抓取|1|关闭抓取,false";
            string strIdea = "/温馨提示:/每场下注上限赛事ID用#分开，留空则全部赛事有效/联赛名称必须简体中文或者英文、数字，并用#分开/";
            string strOthe = "确定修改,guessset2.aspx,post,1,red";

            strText += ",最小下注(" + ub.Get("SiteBz2") + "):/,最大下注(" + ub.Get("SiteBz2") + "):/,每ID每场最大下注(" + ub.Get("SiteBz2") + ")填0则不限制:/";
            strName += ",SmallPay3,BigPay3,IDMaxPay";
            strType += ",num,num,num";
            strValu += "'" + xml.dss["SiteSmallPay3"] + "'" + xml.dss["SiteBigPay3"] + "'" + xml.dss["SiteIDMaxPay"] + "";
            strEmpt += ",false,false,false";

            strText += ",==" + ub.Get("SiteGqText") + "中断立即隐藏全部" + ub.Get("SiteGqText") + "设置==/指定参照赛事ID(多个赛事ID用#分开，留空则随机抽取一场" + ub.Get("SiteGqText") + "):/,中断N秒则隐藏(填0则不开放此功能):/";
            strName += ",checkgid,checkgidsec";
            strType += ",text,num";
            strValu += "'" + xml.dss["Sitecheckgid"] + "'" + xml.dss["Sitecheckgidsec"] + "";
            strEmpt += ",true,true";


            strText += ",串关选择后限时投注(秒):/,最低多少币才能开庄:/,串关同一场同一盘口每ID下注上限(" + ub.Get("SiteBz") + "):/";
            strName += ",SuperVoteSec,xCent,SuperSubCent";
            strType += ",num,num,num";
            strValu += "'" + xml.dss["SiteSuperVoteSec"] + "'" + xml.dss["SitexCent"] + "'" + xml.dss["SiteSuperSubCent"] + "";
            strEmpt += ",true,true,true";

            strText += ",特别检查ID(用#分开):/,让球盘限额比例(%):/,大小盘限额比例(%):/,自定义" + ub.Get("SiteGqText") + "下注确认时间(默认60秒):/,自定义的下注ID(设置的ID将按自定义时间执行下注确认):/,刷新代理(http开头&#47;结尾，留空则停用):/,新方式抓取刷新地址:/";
            strName += ",txIDS,rqPCent,dxPCent,Zdtime,zdIDS,IsSiteView,IsSiteView_1";
            strType += ",big,num,num,num,textarea,text,text";
            strValu += "'" + xml.dss["SitetxIDS"] + "'" + xml.dss["SiteRQPCent"] + "'" + xml.dss["SiteDXPCent"] + "'" + xml.dss["SiteZdtime"] + "'" + xml.dss["SitezdIDS"] + "'" + xml.dss["SiteViewStatus"] + "'" + xml.dss["SiteViewStatus_1"] + "";
            strEmpt += ",false,false,true,false,false,false,false";

            strText += ",捉取方式(默认旧版):/"; //0旧刷新机开 1旧刷新机关闭
            strName += ",SiteUpdateOpen";
            strType += ",select";
            strValu += "'" + xml.dss["SiteUpdateOpen"] + "";
            strEmpt += ",0|旧版|1|新版|2|WCF版本";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl(@"..\guess2\default.aspx") + "\">返回上级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
            #endregion
        }
    }
    #endregion

    #region 足球分级 1
    private void BetLevenPage()
    {
        ub xml = new ub();
        string xmlPath = "/Controls/guess2.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            #region 确定修改

            #region 获取表单
            string Leven1 = Utils.GetRequest("Leven1", "post", 3, @"^[(\u4e00-\u9fa5A-Za-z0-9)# -]+$", "一级赛填写错误，联赛名称必须简体中文或者英文、数字，并用#分开");
            string Leven2 = Utils.GetRequest("Leven2", "post", 3, @"^[(\u4e00-\u9fa5A-Za-z0-9)# -]+$", "二级赛填写错误，联赛名称必须简体中文或者英文、数字，并用#分开");
            string Leven3 = Utils.GetRequest("Leven3", "post", 3, @"^[(\u4e00-\u9fa5A-Za-z0-9)# -]+$", "三级赛填写错误，联赛名称必须简体中文或者英文、数字，并用#分开");
            string Leven4 = Utils.GetRequest("Leven4", "post", 3, @"^[(\u4e00-\u9fa5A-Za-z0-9)# -]+$", "四级赛填写错误，联赛名称必须简体中文或者英文、数字，并用#分开");
            string Leven5 = Utils.GetRequest("Leven5", "post", 3, @"^[(\u4e00-\u9fa5A-Za-z0-9)# -]+$", "" + ub.Get("SiteGqText") + "一级赛填写错误，联赛名称必须简体中文或者英文、数字，并用#分开");
            string Leven6 = Utils.GetRequest("Leven6", "post", 3, @"^[(\u4e00-\u9fa5A-Za-z0-9)# -]+$", "" + ub.Get("SiteGqText") + "二级赛填写错误，联赛名称必须简体中文或者英文、数字，并用#分开");
            string Leven7 = Utils.GetRequest("Leven7", "post", 3, @"^[(\u4e00-\u9fa5A-Za-z0-9)# -]+$", "" + ub.Get("SiteGqText") + "三级赛填写错误，联赛名称必须简体中文或者英文、数字，并用#分开");
            string Leven8 = Utils.GetRequest("Leven8", "post", 3, @"^[(\u4e00-\u9fa5A-Za-z0-9)# -]+$", "" + ub.Get("SiteGqText") + "四级赛填写错误，联赛名称必须简体中文或者英文、数字，并用#分开");
            string Scent1 = Utils.GetRequest("Scent1", "post", 2, @"^[0-9]\d*$", "让球币填写错误");
            string Scent2 = Utils.GetRequest("Scent2", "post", 2, @"^[0-9]\d*$", "让球币填写错误");
            string Scent3 = Utils.GetRequest("Scent3", "post", 2, @"^[0-9]\d*$", "让球币填写错误");
            string Scent4 = Utils.GetRequest("Scent4", "post", 2, @"^[0-9]\d*$", "让球币填写错误");
            string Scent5 = Utils.GetRequest("Scent5", "post", 2, @"^[0-9]\d*$", "让球币填写错误");
            string Scent6 = Utils.GetRequest("Scent6", "post", 2, @"^[0-9]\d*$", "让球币填写错误");
            string Scent7 = Utils.GetRequest("Scent7", "post", 2, @"^[0-9]\d*$", "让球币填写错误");
            string Scent8 = Utils.GetRequest("Scent8", "post", 2, @"^[0-9]\d*$", "让球币填写错误");
            string Dcent1 = Utils.GetRequest("Dcent1", "post", 2, @"^[0-9]\d*$", "大小币填写错误");
            string Dcent2 = Utils.GetRequest("Dcent2", "post", 2, @"^[0-9]\d*$", "大小币填写错误");
            string Dcent3 = Utils.GetRequest("Dcent3", "post", 2, @"^[0-9]\d*$", "大小币填写错误");
            string Dcent4 = Utils.GetRequest("Dcent4", "post", 2, @"^[0-9]\d*$", "大小币填写错误");
            string Dcent5 = Utils.GetRequest("Dcent5", "post", 2, @"^[0-9]\d*$", "大小币填写错误");
            string Dcent6 = Utils.GetRequest("Dcent6", "post", 2, @"^[0-9]\d*$", "大小币填写错误");
            string Dcent7 = Utils.GetRequest("Dcent7", "post", 2, @"^[0-9]\d*$", "大小币填写错误");
            string Dcent8 = Utils.GetRequest("Dcent8", "post", 2, @"^[0-9]\d*$", "大小币填写错误");
            string Bcent1 = Utils.GetRequest("Bcent1", "post", 2, @"^[0-9]\d*$", "标准币填写错误");
            string Bcent2 = Utils.GetRequest("Bcent2", "post", 2, @"^[0-9]\d*$", "标准币填写错误");
            string Bcent3 = Utils.GetRequest("Bcent3", "post", 2, @"^[0-9]\d*$", "标准币填写错误");
            string Bcent4 = Utils.GetRequest("Bcent4", "post", 2, @"^[0-9]\d*$", "标准币填写错误");
            string Bcent5 = Utils.GetRequest("Bcent5", "post", 2, @"^[0-9]\d*$", "标准币填写错误");
            string Bcent6 = Utils.GetRequest("Bcent6", "post", 2, @"^[0-9]\d*$", "标准币填写错误");
            string Bcent7 = Utils.GetRequest("Bcent7", "post", 2, @"^[0-9]\d*$", "标准币填写错误");
            string Bcent8 = Utils.GetRequest("Bcent8", "post", 2, @"^[0-9]\d*$", "标准币填写错误");
            #endregion

            #region 判断#号开头结尾
            if (Utils.Left(Leven1, 1) == "#" || Utils.Right(Leven1, 1) == "#")
            {
                Utils.Error("一级赛名称不能以#开头和#结尾..", "");
            }
            if (Utils.Left(Leven2, 1) == "#" || Utils.Right(Leven2, 1) == "#")
            {
                Utils.Error("二级赛名称不能以#开头和#结尾..", "");
            }
            if (Utils.Left(Leven3, 1) == "#" || Utils.Right(Leven3, 1) == "#")
            {
                Utils.Error("三级赛名称不能以#开头和#结尾..", "");
            }
            if (Utils.Left(Leven4, 1) == "#" || Utils.Right(Leven4, 1) == "#")
            {
                Utils.Error("四级赛名称不能以#开头和#结尾..", "");
            }
            if (Utils.Left(Leven5, 1) == "#" || Utils.Right(Leven5, 1) == "#")
            {
                Utils.Error("" + ub.Get("SiteGqText") + "一级赛名称不能以#开头和#结尾..", "");
            }
            if (Utils.Left(Leven6, 1) == "#" || Utils.Right(Leven6, 1) == "#")
            {
                Utils.Error("" + ub.Get("SiteGqText") + "二级赛名称不能以#开头和#结尾..", "");
            }
            if (Utils.Left(Leven7, 1) == "#" || Utils.Right(Leven7, 1) == "#")
            {
                Utils.Error("" + ub.Get("SiteGqText") + "三级赛名称不能以#开头和#结尾..", "");
            }
            if (Utils.Left(Leven8, 1) == "#" || Utils.Right(Leven8, 1) == "#")
            {
                Utils.Error("" + ub.Get("SiteGqText") + "四级赛名称不能以#开头和#结尾..", "");
            }
            string Levens = "";

            Levens = "" + Leven1 + "#" + Leven2 + "#" + Leven3 + "#" + Leven4 + "";
            string[] Temp = Regex.Split(Levens, "#");
            for (int i = 0; i < Temp.Length; i++)
            {
                int Num = Utils.GetStringNum("#" + Levens + "#", "#" + Temp[i] + "#");
                if (Num >= 2)
                {
                    Utils.Error("联赛名称“" + Temp[i] + "”在四级赛中重复输入", "");
                }
            }

            string Levens2 = "";

            Levens2 = "" + Leven5 + "#" + Leven6 + "#" + Leven7 + "#" + Leven8 + "";
            string[] Temp2 = Regex.Split(Levens2, "#");
            for (int i = 0; i < Temp2.Length; i++)
            {
                int Num = Utils.GetStringNum("#" + Levens2 + "#", "#" + Temp2[i] + "#");
                if (Num >= 2)
                {
                    Utils.Error("联赛名称“" + Temp2[i] + "”在" + ub.Get("SiteGqText") + "四级赛中重复输入", "");
                }
            }
            #endregion

            #region 联赛设置
            xml.dss["SiteLeven1"] = Leven1;
            xml.dss["SiteLeven2"] = Leven2;
            xml.dss["SiteLeven3"] = Leven3;
            xml.dss["SiteLeven4"] = Leven4;
            xml.dss["SiteLeven5"] = Leven5;
            xml.dss["SiteLeven6"] = Leven6;
            xml.dss["SiteLeven7"] = Leven7;
            xml.dss["SiteLeven8"] = Leven8;
            #endregion

            xml.dss["SiteScent1"] = Scent1;
            xml.dss["SiteScent2"] = Scent2;
            xml.dss["SiteScent3"] = Scent3;
            xml.dss["SiteScent4"] = Scent4;
            xml.dss["SiteScent5"] = Scent5;
            xml.dss["SiteScent6"] = Scent6;
            xml.dss["SiteScent7"] = Scent7;
            xml.dss["SiteScent8"] = Scent8;


            xml.dss["SiteDcent1"] = Dcent1;
            xml.dss["SiteDcent2"] = Dcent2;
            xml.dss["SiteDcent3"] = Dcent3;
            xml.dss["SiteDcent4"] = Dcent4;
            xml.dss["SiteDcent5"] = Dcent5;
            xml.dss["SiteDcent6"] = Dcent6;
            xml.dss["SiteDcent7"] = Dcent7;
            xml.dss["SiteDcent8"] = Dcent8;

            xml.dss["SiteBcent1"] = Bcent1;
            xml.dss["SiteBcent2"] = Bcent2;
            xml.dss["SiteBcent3"] = Bcent3;
            xml.dss["SiteBcent4"] = Bcent4;
            xml.dss["SiteBcent5"] = Bcent5;
            xml.dss["SiteBcent6"] = Bcent6;
            xml.dss["SiteBcent7"] = Bcent7;
            xml.dss["SiteBcent8"] = Bcent8;

            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("球彩系统设置", "设置成功，正在返回..", Utils.getUrl("guessset2.aspx?act=betleven&amp;backurl=" + Utils.getPage(0) + ""), "1");
            #endregion
        }
        else
        {
            #region 设置
            builder.Append(Out.Div("title", "足球联赛分级设置"));
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            GuessSetNav(1);
            builder.Append(Out.Tab("</div>", ""));

            string strText = "=一级赛=/联赛名:,让球币:,大小币:,标准币:,=二级赛=/联赛名:,让球币:,大小币:,标准币:,=三级赛=/联赛名:,让球币:,大小币:,标准币:,=四级赛=/联赛名:,让球币:,大小币:,标准币:,=" + ub.Get("SiteGqText") + "一级赛=/联赛名:,让球币:,大小币:,标准币:,=" + ub.Get("SiteGqText") + "二级赛=/联赛名:,让球币:,大小币:,标准币:,=" + ub.Get("SiteGqText") + "三级赛=/联赛名:,让球币:,大小币:,标准币:,=" + ub.Get("SiteGqText") + "四级赛=/联赛名:,让球币:,大小币:,标准币:,,";
            string strName = "Leven1,Scent1,Dcent1,Bcent1,Leven2,Scent2,Dcent2,Bcent2,Leven3,Scent3,Dcent3,Bcent3,Leven4,Scent4,Dcent4,Bcent4,Leven5,Scent5,Dcent5,Bcent5,Leven6,Scent6,Dcent6,Bcent6,Leven7,Scent7,Dcent7,Bcent7,Leven8,Scent8,Dcent8,Bcent8,act,backurl";
            string strType = "big,num,num,num,big,num,num,num,big,num,num,num,big,num,num,num,big,num,num,num,big,num,num,num,big,num,num,num,big,num,num,num,hidden,hidden";
            string strValu = "" + xml.dss["SiteLeven1"] + "'" + xml.dss["SiteScent1"] + "'" + xml.dss["SiteDcent1"] + "'" + xml.dss["SiteBcent1"] + "'" + xml.dss["SiteLeven2"] + "'" + xml.dss["SiteScent2"] + "'" + xml.dss["SiteDcent2"] + "'" + xml.dss["SiteBcent2"] + "'" + xml.dss["SiteLeven3"] + "'" + xml.dss["SiteScent3"] + "'" + xml.dss["SiteDcent3"] + "'" + xml.dss["SiteBcent3"] + "'" + xml.dss["SiteLeven4"] + "'" + xml.dss["SiteScent4"] + "'" + xml.dss["SiteDcent4"] + "'" + xml.dss["SiteBcent4"] + "'" + xml.dss["SiteLeven5"] + "'" + xml.dss["SiteScent5"] + "'" + xml.dss["SiteDcent5"] + "'" + xml.dss["SiteBcent5"] + "'" + xml.dss["SiteLeven6"] + "'" + xml.dss["SiteScent6"] + "'" + xml.dss["SiteDcent6"] + "'" + xml.dss["SiteBcent6"] + "'" + xml.dss["SiteLeven7"] + "'" + xml.dss["SiteScent7"] + "'" + xml.dss["SiteDcent7"] + "'" + xml.dss["SiteBcent7"] + "'" + xml.dss["SiteLeven8"] + "'" + xml.dss["SiteScent8"] + "'" + xml.dss["SiteDcent8"] + "'" + xml.dss["SiteBcent8"] + "'betleven'" + Utils.getPage(0) + "";
            string strEmpt = "true,false,false,false,true,false,false,false,true,false,false,false,true,false,false,false,true,false,false,false,true,false,false,false,true,false,false,false,true,false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定修改|reset,guessset2.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示:<br />联赛名称请用#分开，不限制币请填写0");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl(@"..\guess2\default.aspx") + "\">返回上级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
            #endregion
        }
    }
    #endregion

    #region 篮球联赛分级设置 2
    private void BetLeven2Page()
    {
        ub xml = new ub();
        string xmlPath = "/Controls/guess2.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            #region 确定修改
            string Levenb1 = Utils.GetRequest("Levenb1", "post", 3, @"^[(\u4e00-\u9fa5A-Za-z0-9)# -]+$", "一级赛填写错误，联赛名称必须简体中文或者英文、数字，并用#分开");
            string Levenb2 = Utils.GetRequest("Levenb2", "post", 3, @"^[(\u4e00-\u9fa5A-Za-z0-9)# -]+$", "二级赛填写错误，联赛名称必须简体中文或者英文、数字，并用#分开");
            string Levenb3 = Utils.GetRequest("Levenb3", "post", 3, @"^[(\u4e00-\u9fa5A-Za-z0-9)# -]+$", "三级赛填写错误，联赛名称必须简体中文或者英文、数字，并用#分开");
            string Levenb4 = Utils.GetRequest("Levenb4", "post", 3, @"^[(\u4e00-\u9fa5A-Za-z0-9)# -]+$", "四级赛填写错误，联赛名称必须简体中文或者英文、数字，并用#分开");
            string Levenb5 = Utils.GetRequest("Levenb5", "post", 3, @"^[(\u4e00-\u9fa5A-Za-z0-9)# -]+$", "" + ub.Get("SiteGqText") + "一级赛填写错误，联赛名称必须简体中文或者英文、数字，并用#分开");
            string Levenb6 = Utils.GetRequest("Levenb6", "post", 3, @"^[(\u4e00-\u9fa5A-Za-z0-9)# -]+$", "" + ub.Get("SiteGqText") + "二级赛填写错误，联赛名称必须简体中文或者英文、数字，并用#分开");
            string Levenb7 = Utils.GetRequest("Levenb7", "post", 3, @"^[(\u4e00-\u9fa5A-Za-z0-9)# -]+$", "" + ub.Get("SiteGqText") + "三级赛填写错误，联赛名称必须简体中文或者英文、数字，并用#分开");
            string Levenb8 = Utils.GetRequest("Levenb8", "post", 3, @"^[(\u4e00-\u9fa5A-Za-z0-9)# -]+$", "" + ub.Get("SiteGqText") + "四级赛填写错误，联赛名称必须简体中文或者英文、数字，并用#分开");

            string Scentb1 = Utils.GetRequest("Scentb1", "post", 2, @"^[0-9]\d*$", "让球币填写错误");
            string Scentb2 = Utils.GetRequest("Scentb2", "post", 2, @"^[0-9]\d*$", "让球币填写错误");
            string Scentb3 = Utils.GetRequest("Scentb3", "post", 2, @"^[0-9]\d*$", "让球币填写错误");
            string Scentb4 = Utils.GetRequest("Scentb4", "post", 2, @"^[0-9]\d*$", "让球币填写错误");
            string Scentb5 = Utils.GetRequest("Scentb5", "post", 2, @"^[0-9]\d*$", "让球币填写错误");
            string Scentb6 = Utils.GetRequest("Scentb6", "post", 2, @"^[0-9]\d*$", "让球币填写错误");
            string Scentb7 = Utils.GetRequest("Scentb7", "post", 2, @"^[0-9]\d*$", "让球币填写错误");
            string Scentb8 = Utils.GetRequest("Scentb8", "post", 2, @"^[0-9]\d*$", "让球币填写错误");

            string Dcentb1 = Utils.GetRequest("Dcentb1", "post", 2, @"^[0-9]\d*$", "大小币填写错误");
            string Dcentb2 = Utils.GetRequest("Dcentb2", "post", 2, @"^[0-9]\d*$", "大小币填写错误");
            string Dcentb3 = Utils.GetRequest("Dcentb3", "post", 2, @"^[0-9]\d*$", "大小币填写错误");
            string Dcentb4 = Utils.GetRequest("Dcentb4", "post", 2, @"^[0-9]\d*$", "大小币填写错误");
            string Dcentb5 = Utils.GetRequest("Dcentb5", "post", 2, @"^[0-9]\d*$", "大小币填写错误");
            string Dcentb6 = Utils.GetRequest("Dcentb6", "post", 2, @"^[0-9]\d*$", "大小币填写错误");
            string Dcentb7 = Utils.GetRequest("Dcentb7", "post", 2, @"^[0-9]\d*$", "大小币填写错误");
            string Dcentb8 = Utils.GetRequest("Dcentb8", "post", 2, @"^[0-9]\d*$", "大小币填写错误");

            if (Utils.Left(Levenb1, 1) == "#" || Utils.Right(Levenb1, 1) == "#")
            {
                Utils.Error("一级赛名称不能以#开头和#结尾..", "");
            }
            if (Utils.Left(Levenb2, 1) == "#" || Utils.Right(Levenb2, 1) == "#")
            {
                Utils.Error("二级赛名称不能以#开头和#结尾..", "");
            }
            if (Utils.Left(Levenb3, 1) == "#" || Utils.Right(Levenb3, 1) == "#")
            {
                Utils.Error("三级赛名称不能以#开头和#结尾..", "");
            }
            if (Utils.Left(Levenb4, 1) == "#" || Utils.Right(Levenb4, 1) == "#")
            {
                Utils.Error("四级赛名称不能以#开头和#结尾..", "");
            }
            if (Utils.Left(Levenb5, 1) == "#" || Utils.Right(Levenb5, 1) == "#")
            {
                Utils.Error("" + ub.Get("SiteGqText") + "一级赛名称不能以#开头和#结尾..", "");
            }
            if (Utils.Left(Levenb6, 1) == "#" || Utils.Right(Levenb6, 1) == "#")
            {
                Utils.Error("" + ub.Get("SiteGqText") + "二级赛名称不能以#开头和#结尾..", "");
            }
            if (Utils.Left(Levenb7, 1) == "#" || Utils.Right(Levenb7, 1) == "#")
            {
                Utils.Error("" + ub.Get("SiteGqText") + "三级赛名称不能以#开头和#结尾..", "");
            }
            if (Utils.Left(Levenb8, 1) == "#" || Utils.Right(Levenb8, 1) == "#")
            {
                Utils.Error("" + ub.Get("SiteGqText") + "四级赛名称不能以#开头和#结尾..", "");
            }



            string Levenbs = "";

            Levenbs = "" + Levenb1 + "#" + Levenb2 + "#" + Levenb3 + "#" + Levenb4 + "";



            string[] Temp = Regex.Split(Levenbs, "#");
            for (int i = 0; i < Temp.Length; i++)
            {
                int Num = Utils.GetStringNum("#" + Levenbs + "#", "#" + Temp[i] + "#");
                if (Num >= 2)
                {
                    Utils.Error("联赛名称“" + Temp[i] + "”在四级赛中重复输入", "");
                }
            }

            string Levenbs2 = "";

            Levenbs2 = "" + Levenb5 + "#" + Levenb6 + "#" + Levenb7 + "#" + Levenb8 + "";



            string[] Temp2 = Regex.Split(Levenbs2, "#");
            for (int i = 0; i < Temp2.Length; i++)
            {
                int Num = Utils.GetStringNum("#" + Levenbs2 + "#", "#" + Temp2[i] + "#");
                if (Num >= 2)
                {
                    Utils.Error("联赛名称“" + Temp2[i] + "”在" + ub.Get("SiteGqText") + "四级赛中重复输入", "");
                }
            }

            xml.dss["SiteLevenb1"] = Levenb1;
            xml.dss["SiteLevenb2"] = Levenb2;
            xml.dss["SiteLevenb3"] = Levenb3;
            xml.dss["SiteLevenb4"] = Levenb4;
            xml.dss["SiteLevenb5"] = Levenb5;
            xml.dss["SiteLevenb6"] = Levenb6;
            xml.dss["SiteLevenb7"] = Levenb7;
            xml.dss["SiteLevenb8"] = Levenb8;

            xml.dss["SiteScentb1"] = Scentb1;
            xml.dss["SiteScentb2"] = Scentb2;
            xml.dss["SiteScentb3"] = Scentb3;
            xml.dss["SiteScentb4"] = Scentb4;
            xml.dss["SiteScentb5"] = Scentb5;
            xml.dss["SiteScentb6"] = Scentb6;
            xml.dss["SiteScentb7"] = Scentb7;
            xml.dss["SiteScentb8"] = Scentb8;

            xml.dss["SiteDcentb1"] = Dcentb1;
            xml.dss["SiteDcentb2"] = Dcentb2;
            xml.dss["SiteDcentb3"] = Dcentb3;
            xml.dss["SiteDcentb4"] = Dcentb4;
            xml.dss["SiteDcentb5"] = Dcentb5;
            xml.dss["SiteDcentb6"] = Dcentb6;
            xml.dss["SiteDcentb7"] = Dcentb7;
            xml.dss["SiteDcentb8"] = Dcentb8;


            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("球彩系统设置", "设置成功，正在返回..", Utils.getUrl("guessset2.aspx?act=betleven2&amp;backurl=" + Utils.getPage(0) + ""), "1");
            #endregion
        }
        else
        {
            #region 设置
            builder.Append(Out.Div("title", "篮球联赛分级设置"));
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            GuessSetNav(2);
            builder.Append(Out.Tab("</div>", ""));
            string strText = "=一级赛=/联赛名:,让球币:,大小币:,=二级赛=/联赛名:,让球币:,大小币:,=三级赛=/联赛名:,让球币:,大小币:,=四级赛=/联赛名:,让球币:,大小币:,,";
            string strName = "Levenb1,Scentb1,Dcentb1,Levenb2,Scentb2,Dcentb2,Levenb3,Scentb3,Dcentb3,Levenb4,Scentb4,Dcentb4,act,backurl";
            string strType = "big,num,num,big,num,num,big,num,num,big,num,num,hidden,hidden";
            string strValu = "" + xml.dss["SiteLevenb1"] + "'" + xml.dss["SiteScentb1"] + "'" + xml.dss["SiteDcentb1"] + "'" + xml.dss["SiteLevenb2"] + "'" + xml.dss["SiteScentb2"] + "'" + xml.dss["SiteDcentb2"] + "'" + xml.dss["SiteLevenb3"] + "'" + xml.dss["SiteScentb3"] + "'" + xml.dss["SiteDcentb3"] + "'" + xml.dss["SiteLevenb4"] + "'" + xml.dss["SiteScentb4"] + "'" + xml.dss["SiteDcentb4"] + "'betleven2'" + Utils.getPage(0) + "";
            string strEmpt = "true,false,false,true,false,false,true,false,false,true,false,false,false,false";

            strText += ",=" + ub.Get("SiteGqText") + "一级赛=/联赛名:,让球币:,大小币:,=" + ub.Get("SiteGqText") + "二级赛=/联赛名:,让球币:,大小币:,=" + ub.Get("SiteGqText") + "三级赛=/联赛名:,让球币:,大小币:,=" + ub.Get("SiteGqText") + "四级赛=/联赛名:,让球币:,大小币:";
            strName += ",Levenb5,Scentb5,Dcentb5,Levenb6,Scentb6,Dcentb6,Levenb7,Scentb7,Dcentb7,Levenb8,Scentb8,Dcentb8";
            strType += ",big,num,num,big,num,num,big,num,num,big,num,num";
            strValu += "'" + xml.dss["SiteLevenb5"] + "'" + xml.dss["SiteScentb5"] + "'" + xml.dss["SiteDcentb5"] + "'" + xml.dss["SiteLevenb6"] + "'" + xml.dss["SiteScentb6"] + "'" + xml.dss["SiteDcentb6"] + "'" + xml.dss["SiteLevenb7"] + "'" + xml.dss["SiteScentb7"] + "'" + xml.dss["SiteDcentb7"] + "'" + xml.dss["SiteLevenb8"] + "'" + xml.dss["SiteScentb8"] + "'" + xml.dss["SiteDcentb8"] + "";
            strEmpt += ",true,false,false,true,false,false,true,false,false,true,false,false";

            string strIdea = "/";
            string strOthe = "确定修改|reset,guessset2.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示:<br />联赛名称请用#分开，不限制币请填写0");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl(@"..\guess2\default.aspx") + "\">返回上级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
            #endregion
        }
    }
    #endregion

    #region 半场单节分级设置 3
    private void BetLeven3Page()
    {
        ub xml = new ub();
        string xmlPath = "/Controls/guess2.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            #region 确定修改
            string Leven1 = Utils.GetRequest("Leven1", "post", 3, @"^[(\u4e00-\u9fa5A-Za-z0-9)# -]+$", "一级赛填写错误，联赛名称必须简体中文或者英文、数字，并用#分开");
            string Leven2 = Utils.GetRequest("Leven2", "post", 3, @"^[(\u4e00-\u9fa5A-Za-z0-9)# -]+$", "二级赛填写错误，联赛名称必须简体中文或者英文、数字，并用#分开");
            string Leven3 = Utils.GetRequest("Leven3", "post", 3, @"^[(\u4e00-\u9fa5A-Za-z0-9)# -]+$", "三级赛填写错误，联赛名称必须简体中文或者英文、数字，并用#分开");
            string Leven4 = Utils.GetRequest("Leven4", "post", 3, @"^[(\u4e00-\u9fa5A-Za-z0-9)# -]+$", "四级赛填写错误，联赛名称必须简体中文或者英文、数字，并用#分开");
            string Scent1 = Utils.GetRequest("Scent1", "post", 2, @"^[0-9]\d*$", "让球币填写错误");
            string Scent2 = Utils.GetRequest("Scent2", "post", 2, @"^[0-9]\d*$", "让球币填写错误");
            string Scent3 = Utils.GetRequest("Scent3", "post", 2, @"^[0-9]\d*$", "让球币填写错误");
            string Scent4 = Utils.GetRequest("Scent4", "post", 2, @"^[0-9]\d*$", "让球币填写错误");
            string Dcent1 = Utils.GetRequest("Dcent1", "post", 2, @"^[0-9]\d*$", "大小币填写错误");
            string Dcent2 = Utils.GetRequest("Dcent2", "post", 2, @"^[0-9]\d*$", "大小币填写错误");
            string Dcent3 = Utils.GetRequest("Dcent3", "post", 2, @"^[0-9]\d*$", "大小币填写错误");
            string Dcent4 = Utils.GetRequest("Dcent4", "post", 2, @"^[0-9]\d*$", "大小币填写错误");
            string Bcent1 = Utils.GetRequest("Bcent1", "post", 2, @"^[0-9]\d*$", "标准币填写错误");
            string Bcent2 = Utils.GetRequest("Bcent2", "post", 2, @"^[0-9]\d*$", "标准币填写错误");
            string Bcent3 = Utils.GetRequest("Bcent3", "post", 2, @"^[0-9]\d*$", "标准币填写错误");
            string Bcent4 = Utils.GetRequest("Bcent4", "post", 2, @"^[0-9]\d*$", "标准币填写错误");

            #region 判断#号开头结尾
            if (Utils.Left(Leven1, 1) == "#" || Utils.Right(Leven1, 1) == "#")
            {
                Utils.Error("一级赛名称不能以#开头和#结尾..", "");
            }
            if (Utils.Left(Leven2, 1) == "#" || Utils.Right(Leven2, 1) == "#")
            {
                Utils.Error("二级赛名称不能以#开头和#结尾..", "");
            }
            if (Utils.Left(Leven3, 1) == "#" || Utils.Right(Leven3, 1) == "#")
            {
                Utils.Error("三级赛名称不能以#开头和#结尾..", "");
            }
            if (Utils.Left(Leven4, 1) == "#" || Utils.Right(Leven4, 1) == "#")
            {
                Utils.Error("四级赛名称不能以#开头和#结尾..", "");
            }
            string Levens = "";

            Levens = "" + Leven1 + "#" + Leven2 + "#" + Leven3 + "#" + Leven4 + "";
            string[] Temp = Regex.Split(Levens, "#");
            for (int i = 0; i < Temp.Length; i++)
            {
                int Num = Utils.GetStringNum("#" + Levens + "#", "#" + Temp[i] + "#");
                if (Num >= 2)
                {
                    Utils.Error("联赛名称“" + Temp[i] + "”在四级赛中重复输入", "");
                }
            }
            #endregion

            xml.dss["SiteBLeven1"] = Leven1;
            xml.dss["SiteBLeven2"] = Leven2;
            xml.dss["SiteBLeven3"] = Leven3;
            xml.dss["SiteBLeven4"] = Leven4;

            xml.dss["SiteBScent1"] = Scent1;
            xml.dss["SiteBScent2"] = Scent2;
            xml.dss["SiteBScent3"] = Scent3;
            xml.dss["SiteBScent4"] = Scent4;

            xml.dss["SiteBDcent1"] = Dcent1;
            xml.dss["SiteBDcent2"] = Dcent2;
            xml.dss["SiteBDcent3"] = Dcent3;
            xml.dss["SiteBDcent4"] = Dcent4;

            xml.dss["SiteBBcent1"] = Bcent1;
            xml.dss["SiteBBcent2"] = Bcent2;
            xml.dss["SiteBBcent3"] = Bcent3;
            xml.dss["SiteBBcent4"] = Bcent4;


            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("球彩系统设置", "设置成功，正在返回..", Utils.getUrl("guessset2.aspx?act=betleven3&amp;backurl=" + Utils.getPage(0) + ""), "1");
            #endregion
        }
        else
        {
            #region 设置
            builder.Append(Out.Div("title", "足球半场联赛分级设置"));
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            GuessSetNav(3);
            builder.Append(Out.Tab("</div>", ""));
            string strText = "=一级赛=/联赛名:,让球币:,大小币:,标准币:,=二级赛=/联赛名:,让球币:,大小币:,标准币:,=三级赛=/联赛名:,让球币:,大小币:,标准币:,=四级赛=/联赛名:,让球币:,大小币:,标准币:,,";
            string strName = "Leven1,Scent1,Dcent1,Bcent1,Leven2,Scent2,Dcent2,Bcent2,Leven3,Scent3,Dcent3,Bcent3,Leven4,Scent4,Dcent4,Bcent4,act,backurl";
            string strType = "big,num,num,num,big,num,num,num,big,num,num,num,big,num,num,num,hidden,hidden";
            string strValu = "" + xml.dss["SiteBLeven1"] + "'" + xml.dss["SiteBScent1"] + "'" + xml.dss["SiteBDcent1"] + "'" + xml.dss["SiteBBcent1"] + "'" + xml.dss["SiteBLeven2"] + "'" + xml.dss["SiteBScent2"] + "'" + xml.dss["SiteBDcent2"] + "'" + xml.dss["SiteBBcent2"] + "'" + xml.dss["SiteBLeven3"] + "'" + xml.dss["SiteBScent3"] + "'" + xml.dss["SiteBDcent3"] + "'" + xml.dss["SiteBBcent3"] + "'" + xml.dss["SiteBLeven4"] + "'" + xml.dss["SiteBScent4"] + "'" + xml.dss["SiteBDcent4"] + "'" + xml.dss["SiteBBcent4"] + "'betleven3'" + Utils.getPage(0) + "";
            string strEmpt = "true,false,false,false,true,false,false,false,true,false,false,false,true,false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定修改|reset,guessset2.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示:<br />联赛名称请用#分开，不限制币请填写0");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl(@"..\guess2\default.aspx") + "\">返回上级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
            #endregion
        }

    }
    #endregion

    #region 单节分级 4
    private void BetLeven4Page()
    {
        ub xml = new ub();
        string xmlPath = "/Controls/guess2.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            #region 确定修改
            string Levenb1 = Utils.GetRequest("Levenb1", "post", 3, @"^[(\u4e00-\u9fa5A-Za-z0-9)# -]+$", "一级赛填写错误，联赛名称必须简体中文或者英文、数字，并用#分开");
            string Levenb2 = Utils.GetRequest("Levenb2", "post", 3, @"^[(\u4e00-\u9fa5A-Za-z0-9)# -]+$", "二级赛填写错误，联赛名称必须简体中文或者英文、数字，并用#分开");
            string Levenb3 = Utils.GetRequest("Levenb3", "post", 3, @"^[(\u4e00-\u9fa5A-Za-z0-9)# -]+$", "三级赛填写错误，联赛名称必须简体中文或者英文、数字，并用#分开");
            string Levenb4 = Utils.GetRequest("Levenb4", "post", 3, @"^[(\u4e00-\u9fa5A-Za-z0-9)# -]+$", "四级赛填写错误，联赛名称必须简体中文或者英文、数字，并用#分开");
            string Scentb1 = Utils.GetRequest("Scentb1", "post", 2, @"^[0-9]\d*$", "让球币填写错误");
            string Scentb2 = Utils.GetRequest("Scentb2", "post", 2, @"^[0-9]\d*$", "让球币填写错误");
            string Scentb3 = Utils.GetRequest("Scentb3", "post", 2, @"^[0-9]\d*$", "让球币填写错误");
            string Scentb4 = Utils.GetRequest("Scentb4", "post", 2, @"^[0-9]\d*$", "让球币填写错误");
            string Dcentb1 = Utils.GetRequest("Dcentb1", "post", 2, @"^[0-9]\d*$", "大小币填写错误");
            string Dcentb2 = Utils.GetRequest("Dcentb2", "post", 2, @"^[0-9]\d*$", "大小币填写错误");
            string Dcentb3 = Utils.GetRequest("Dcentb3", "post", 2, @"^[0-9]\d*$", "大小币填写错误");
            string Dcentb4 = Utils.GetRequest("Dcentb4", "post", 2, @"^[0-9]\d*$", "大小币填写错误");

            #region 判断#号开头结尾
            if (Utils.Left(Levenb1, 1) == "#" || Utils.Right(Levenb1, 1) == "#")
            {
                Utils.Error("一级赛名称不能以#开头和#结尾..", "");
            }
            if (Utils.Left(Levenb2, 1) == "#" || Utils.Right(Levenb2, 1) == "#")
            {
                Utils.Error("二级赛名称不能以#开头和#结尾..", "");
            }
            if (Utils.Left(Levenb3, 1) == "#" || Utils.Right(Levenb3, 1) == "#")
            {
                Utils.Error("三级赛名称不能以#开头和#结尾..", "");
            }
            if (Utils.Left(Levenb4, 1) == "#" || Utils.Right(Levenb4, 1) == "#")
            {
                Utils.Error("四级赛名称不能以#开头和#结尾..", "");
            }
            string Levens = "";

            Levens = "" + Levenb1 + "#" + Levenb2 + "#" + Levenb3 + "#" + Levenb4 + "";
            string[] Temp = Regex.Split(Levens, "#");
            for (int i = 0; i < Temp.Length; i++)
            {
                int Num = Utils.GetStringNum("#" + Levens + "#", "#" + Temp[i] + "#");
                if (Num >= 2)
                {
                    Utils.Error("联赛名称“" + Temp[i] + "”在四级赛中重复输入", "");
                }
            }
            #endregion

            xml.dss["SiteBLevenb1"] = Levenb1;
            xml.dss["SiteBLevenb2"] = Levenb2;
            xml.dss["SiteBLevenb3"] = Levenb3;
            xml.dss["SiteBLevenb4"] = Levenb4;

            xml.dss["SiteBScentb1"] = Scentb1;
            xml.dss["SiteBScentb2"] = Scentb2;
            xml.dss["SiteBScentb3"] = Scentb3;
            xml.dss["SiteBScentb4"] = Scentb4;

            xml.dss["SiteBDcentb1"] = Dcentb1;
            xml.dss["SiteBDcentb2"] = Dcentb2;
            xml.dss["SiteBDcentb3"] = Dcentb3;
            xml.dss["SiteBDcentb4"] = Dcentb4;

            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("球彩系统设置", "设置成功，正在返回..", Utils.getUrl("guessset2.aspx?act=betleven4&amp;backurl=" + Utils.getPage(0) + ""), "1");
            #endregion
        }
        else
        {
            #region 设置
            builder.Append(Out.Div("title", "篮球单节联赛分级设置"));
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            GuessSetNav(4);
            builder.Append(Out.Tab("</div>", ""));
            string strText = "=一级赛=/联赛名:,让球币:,大小币:,=二级赛=/联赛名:,让球币:,大小币:,=三级赛=/联赛名:,让球币:,大小币:,=四级赛=/联赛名:,让球币:,大小币:,,";
            string strName = "Levenb1,Scentb1,Dcentb1,Levenb2,Scentb2,Dcentb2,Levenb3,Scentb3,Dcentb3,Levenb4,Scentb4,Dcentb4,act,backurl";
            string strType = "big,num,num,big,num,num,big,num,num,big,num,num,hidden,hidden";
            string strValu = "" + xml.dss["SiteBLevenb1"] + "'" + xml.dss["SiteBScentb1"] + "'" + xml.dss["SiteBDcentb1"] + "'" + xml.dss["SiteBLevenb2"] + "'" + xml.dss["SiteBScentb2"] + "'" + xml.dss["SiteBDcentb2"] + "'" + xml.dss["SiteBLevenb3"] + "'" + xml.dss["SiteBScentb3"] + "'" + xml.dss["SiteBDcentb3"] + "'" + xml.dss["SiteBLevenb4"] + "'" + xml.dss["SiteBScentb4"] + "'" + xml.dss["SiteBDcentb4"] + "'betleven4'" + Utils.getPage(0) + "";
            string strEmpt = "true,false,false,true,false,false,true,false,false,true,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定修改|reset,guessset2.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示:<br />联赛名称请用#分开，不限制币请填写0");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl(@"..\guess2\default.aspx") + "\">返回上级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
            #endregion
        }
    }
    #endregion

    #region 波胆 5
    private void ScorePage()
    {
        ub xml = new ub();
        string xmlPath = "/Controls/guess2.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            #region 确认修改
            //string bdstat = Utils.GetRequest("bdstat", "post", 3, @"^[(\u4e00-\u9fa5A-Za-z0-9)# -]+$", "波胆联赛名称必须简体中文或者英文、数字，并用#分开"));
            decimal score10 = Convert.ToDecimal(Utils.GetRequest("score10", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写1赔率,小数点后保留1-2位"));
            decimal score01 = Convert.ToDecimal(Utils.GetRequest("score01", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写2赔率,小数点后保留1-2位"));
            decimal score00 = Convert.ToDecimal(Utils.GetRequest("score00", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写3赔率,小数点后保留1-2位"));
            decimal score20 = Convert.ToDecimal(Utils.GetRequest("score20", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写4赔率,小数点后保留1-2位"));
            decimal score02 = Convert.ToDecimal(Utils.GetRequest("score02", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写5赔率,小数点后保留1-2位"));
            decimal score11 = Convert.ToDecimal(Utils.GetRequest("score11", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写6赔率,小数点后保留1-2位"));
            decimal score30 = Convert.ToDecimal(Utils.GetRequest("score30", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写7赔率,小数点后保留1-2位"));
            decimal score03 = Convert.ToDecimal(Utils.GetRequest("score03", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写8赔率,小数点后保留1-2位"));
            decimal score22 = Convert.ToDecimal(Utils.GetRequest("score22", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写9赔率,小数点后保留1-2位"));
            decimal score40 = Convert.ToDecimal(Utils.GetRequest("score40", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写10赔率,小数点后保留1-2位"));
            decimal score04 = Convert.ToDecimal(Utils.GetRequest("score04", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写11赔率,小数点后保留1-2位"));
            decimal score33 = Convert.ToDecimal(Utils.GetRequest("score33", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写12赔率,小数点后保留1-2位"));
            decimal score21 = Convert.ToDecimal(Utils.GetRequest("score21", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写13赔率,小数点后保留1-2位"));
            decimal score12 = Convert.ToDecimal(Utils.GetRequest("score12", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写14赔率,小数点后保留1-2位"));
            decimal score44 = Convert.ToDecimal(Utils.GetRequest("score44", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写15赔率,小数点后保留1-2位"));
            decimal score31 = Convert.ToDecimal(Utils.GetRequest("score31", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写16赔率,小数点后保留1-2位"));
            decimal score13 = Convert.ToDecimal(Utils.GetRequest("score13", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写17赔率,小数点后保留1-2位"));
            decimal score41 = Convert.ToDecimal(Utils.GetRequest("score41", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写18赔率,小数点后保留1-2位"));
            decimal score14 = Convert.ToDecimal(Utils.GetRequest("score14", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写19赔率,小数点后保留1-2位"));
            decimal score32 = Convert.ToDecimal(Utils.GetRequest("score32", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写20赔率,小数点后保留1-2位"));
            decimal score23 = Convert.ToDecimal(Utils.GetRequest("score23", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写21赔率,小数点后保留1-2位"));
            decimal score42 = Convert.ToDecimal(Utils.GetRequest("score42", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写22赔率,小数点后保留1-2位"));
            decimal score24 = Convert.ToDecimal(Utils.GetRequest("score24", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写23赔率,小数点后保留1-2位"));
            decimal score43 = Convert.ToDecimal(Utils.GetRequest("score43", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写24赔率,小数点后保留1-2位"));
            decimal score34 = Convert.ToDecimal(Utils.GetRequest("score34", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写25赔率,小数点后保留1-2位"));
            decimal score5z = Convert.ToDecimal(Utils.GetRequest("score5z", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写26赔率,小数点后保留1-2位"));
            decimal score5k = Convert.ToDecimal(Utils.GetRequest("score5k", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写27赔率,小数点后保留1-2位"));
            decimal scoreot = Convert.ToDecimal(Utils.GetRequest("scoreot", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写27赔率,小数点后保留1-2位"));

            //xml.dss["Sitebdstat"] = bdstat;
            xml.dss["SiteScore10"] = score10;
            xml.dss["SiteScore01"] = score01;
            xml.dss["SiteScore00"] = score00;
            xml.dss["SiteScore20"] = score20;
            xml.dss["SiteScore02"] = score02;
            xml.dss["SiteScore11"] = score11;
            xml.dss["SiteScore30"] = score30;
            xml.dss["SiteScore03"] = score03;
            xml.dss["SiteScore22"] = score22;
            xml.dss["SiteScore40"] = score40;
            xml.dss["SiteScore04"] = score04;
            xml.dss["SiteScore33"] = score33;
            xml.dss["SiteScore21"] = score21;
            xml.dss["SiteScore12"] = score12;
            xml.dss["SiteScore44"] = score44;
            xml.dss["SiteScore31"] = score31;
            xml.dss["SiteScore13"] = score13;
            xml.dss["SiteScore41"] = score41;
            xml.dss["SiteScore14"] = score14;
            xml.dss["SiteScore32"] = score32;
            xml.dss["SiteScore23"] = score23;
            xml.dss["SiteScore42"] = score42;
            xml.dss["SiteScore24"] = score24;
            xml.dss["SiteScore43"] = score43;
            xml.dss["SiteScore34"] = score34;
            xml.dss["SiteScore5z"] = score5z;
            xml.dss["SiteScore5k"] = score5k;
            xml.dss["SiteScoreot"] = scoreot;

            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("球彩系统设置", "设置成功，正在返回..", Utils.getUrl("guessset2.aspx?act=score&amp;backurl=" + Utils.getPage(0) + ""), "1");
            #endregion
        }
        else
        {
            #region 设置
            builder.Append(Out.Div("title", "波胆投注上限设置"));
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            GuessSetNav(5);
            builder.Append(Out.Tab("</div>", "<br />"));


            //string strText = "=波胆联赛设置(备用)=/,/=投注上限设置=/,1:0,0:1,0:0,/,2:0,0:2,1:1,/,3:0,0:3,2:2,/,4:0,0:4,3:3,/,2:1,1:2,4:4,/,3:1,1:3,/,4:1,1:4,/,3:2,2:3,/,4:2,2:4,/,4:3,3:4,/,主净胜5球或以上,/,客净胜5球或以上,/,其他盘/,/,,,,,";
            //string strName = "bdstat,score10,score01,score00,,score20,score02,score11,,score30,score03,score22,,score40,score04,score33,,score21,score12,score44,,score31,score13,,score41,score14,,score32,score23,,score42,score24,,score43,score34,,score5z,,score5k,,scoreot,,act";
            //string strType = "big,small,small,small,hr,small,small,small,hr,small,small,small,hr,small,small,small,hr,small,small,small,hr,small,small,hr,small,small,hr,small,small,hr,small,small,hr,small,small,hr,small,hr,small,hr,small,hr,hidden,hidden,hidden";
            //string strValu = "" + xml.dss["Sitebdstat"] + "'" + xml.dss["SiteScore10"] + "'" + xml.dss["SiteScore01"] + "'" + xml.dss["SiteScore00"] + "''" + xml.dss["SiteScore20"] + "'" + xml.dss["SiteScore02"] + "'" + xml.dss["SiteScore11"] + "''" + xml.dss["SiteScore30"] + "'" + xml.dss["SiteScore03"] + "'" + xml.dss["SiteScore22"] + "''" + xml.dss["SiteScore40"] + "'" + xml.dss["SiteScore04"] + "'" + xml.dss["SiteScore33"] + "''" + xml.dss["SiteScore21"] + "'" + xml.dss["SiteScore12"] + "'" + xml.dss["SiteScore44"] + "''" + xml.dss["SiteScore31"] + "'" + xml.dss["SiteScore13"] + "''" + xml.dss["SiteScore41"] + "'" + xml.dss["SiteScore14"] + "''" + xml.dss["SiteScore32"] + "'" + xml.dss["SiteScore23"] + "''" + xml.dss["SiteScore42"] + "'" + xml.dss["SiteScore24"] + "''" + xml.dss["SiteScore43"] + "'" + xml.dss["SiteScore34"] + "''" + xml.dss["SiteScore5z"] + "''" + xml.dss["SiteScore5k"] + "''" + xml.dss["SiteScoreot"] + "''score'" + Utils.getPage(0) + "";
            //string strEmpt = "true,true,true,true,,true,true,true,,true,true,true,,true,true,true,,true,true,true,,true,true,,true,true,,true,true,,true,true,,true,true,,true,,true,,false,false";
            //string strIdea = "'" + ub.Get("SiteBz") + " '" + ub.Get("SiteBz") + " '" + ub.Get("SiteBz") + " ''" + ub.Get("SiteBz") + " '" + ub.Get("SiteBz") + " '" + ub.Get("SiteBz") + " ''" + ub.Get("SiteBz") + " '" + ub.Get("SiteBz") + " '" + ub.Get("SiteBz") + " ''" + ub.Get("SiteBz") + " '" + ub.Get("SiteBz") + " '" + ub.Get("SiteBz") + " ''" + ub.Get("SiteBz") + " '" + ub.Get("SiteBz") + " '" + ub.Get("SiteBz") + " ''" + ub.Get("SiteBz") + " '" + ub.Get("SiteBz") + " ''" + ub.Get("SiteBz") + " '" + ub.Get("SiteBz") + " ''" + ub.Get("SiteBz") + " '" + ub.Get("SiteBz") + " ''" + ub.Get("SiteBz") + " '" + ub.Get("SiteBz") + " ''" + ub.Get("SiteBz") + " '" + ub.Get("SiteBz") + " ''" + ub.Get("SiteBz") + " ''" + ub.Get("SiteBz") + " ''" + ub.Get("SiteBz") + " ''|/";

            string strText = "/=投注上限设置=/,1:0,0:1,0:0,/,2:0,0:2,1:1,/,3:0,0:3,2:2,/,4:0,0:4,3:3,/,2:1,1:2,4:4,/,3:1,1:3,/,4:1,1:4,/,3:2,2:3,/,4:2,2:4,/,4:3,3:4,/,主净胜5球或以上,/,客净胜5球或以上,/,其他盘/,/,,,,,";
            string strName = "score10,score01,score00,,score20,score02,score11,,score30,score03,score22,,score40,score04,score33,,score21,score12,score44,,score31,score13,,score41,score14,,score32,score23,,score42,score24,,score43,score34,,score5z,,score5k,,scoreot,,act";
            string strType = "small,small,small,hr,small,small,small,hr,small,small,small,hr,small,small,small,hr,small,small,small,hr,small,small,hr,small,small,hr,small,small,hr,small,small,hr,small,small,hr,small,hr,small,hr,small,hr,hidden,hidden,hidden";
            string strValu = xml.dss["SiteScore10"] + "'" + xml.dss["SiteScore01"] + "'" + xml.dss["SiteScore00"] + "''" + xml.dss["SiteScore20"] + "'" + xml.dss["SiteScore02"] + "'" + xml.dss["SiteScore11"] + "''" + xml.dss["SiteScore30"] + "'" + xml.dss["SiteScore03"] + "'" + xml.dss["SiteScore22"] + "''" + xml.dss["SiteScore40"] + "'" + xml.dss["SiteScore04"] + "'" + xml.dss["SiteScore33"] + "''" + xml.dss["SiteScore21"] + "'" + xml.dss["SiteScore12"] + "'" + xml.dss["SiteScore44"] + "''" + xml.dss["SiteScore31"] + "'" + xml.dss["SiteScore13"] + "''" + xml.dss["SiteScore41"] + "'" + xml.dss["SiteScore14"] + "''" + xml.dss["SiteScore32"] + "'" + xml.dss["SiteScore23"] + "''" + xml.dss["SiteScore42"] + "'" + xml.dss["SiteScore24"] + "''" + xml.dss["SiteScore43"] + "'" + xml.dss["SiteScore34"] + "''" + xml.dss["SiteScore5z"] + "''" + xml.dss["SiteScore5k"] + "''" + xml.dss["SiteScoreot"] + "''score'" + Utils.getPage(0) + "";
            string strEmpt = "true,true,true,,true,true,true,,true,true,true,,true,true,true,,true,true,true,,true,true,,true,true,,true,true,,true,true,,true,true,,true,,true,,false,false";
            string strIdea = ub.Get("SiteBz") + " '" + ub.Get("SiteBz") + " '" + ub.Get("SiteBz") + " ''" + ub.Get("SiteBz") + " '" + ub.Get("SiteBz") + " '" + ub.Get("SiteBz") + " ''" + ub.Get("SiteBz") + " '" + ub.Get("SiteBz") + " '" + ub.Get("SiteBz") + " ''" + ub.Get("SiteBz") + " '" + ub.Get("SiteBz") + " '" + ub.Get("SiteBz") + " ''" + ub.Get("SiteBz") + " '" + ub.Get("SiteBz") + " '" + ub.Get("SiteBz") + " ''" + ub.Get("SiteBz") + " '" + ub.Get("SiteBz") + " ''" + ub.Get("SiteBz") + " '" + ub.Get("SiteBz") + " ''" + ub.Get("SiteBz") + " '" + ub.Get("SiteBz") + " ''" + ub.Get("SiteBz") + " '" + ub.Get("SiteBz") + " ''" + ub.Get("SiteBz") + " '" + ub.Get("SiteBz") + " ''" + ub.Get("SiteBz") + " ''" + ub.Get("SiteBz") + " ''" + ub.Get("SiteBz") + " ''|/";

            string strOthe = "确定修改,guessset2.aspx,post,3,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示:<br />不限制币请填写0");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl(@"..\guess2\default.aspx") + "\">返回上级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
            #endregion
        }
    }
    #endregion

    #region 超额提醒设置 6
    /// <summary>
    /// 超额提醒设置
    /// </summary>
    private void OverCallPage()
    {
        Master.Title = "超额提醒设置";
        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId != 1 && ManageId != 9 && ManageId != 26)
        {
            Utils.Error("权限不足", "");
        }
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/guess2.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            #region 确定修改

            string ListCent = Utils.GetRequest("ListCent", "post", 2, @"^[0-9]\d*$", "检查金额填写错误");
            string KillCent = Utils.GetRequest("KillCent", "post", 2, @"^[0-9]\d*$", "超额金额填写错误");
            string SiteAdminIDS = Utils.GetRequest("SiteAdminIDS", "post", 2, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,1000}$", "接收内线ID填写错误");

            //写入XML
            xml.dss["SiteListCent"] = ListCent;
            xml.dss["SiteKillCent"] = KillCent;
            xml.dss["SiteAdminIDS"] = SiteAdminIDS;

            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("超额提醒设置", "设置成功，正在返回..", Utils.getUrl("guessset2.aspx?act=overcall"), "1");
            #endregion
        }
        else
        {
            #region 设置
            builder.Append(Out.Div("title", "超额提醒设置"));
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            GuessSetNav(6);
            builder.Append(Out.Tab("</div>", ""));

            string strText = "检查金额:/,超额金额:/,接收内线ID:/,";
            string strName = "ListCent,KillCent,SiteAdminIDS,backurl";
            string strType = "num,num,big,hidden";
            string strValu = xml.dss["SiteListCent"] + "'" + xml.dss["SiteKillCent"] + "'" + xml.dss["SiteAdminIDS"] + "'" + Utils.getPage(0) + "";
            string strEmpt = "false,false,false,false";

            string strIdea = "/温馨提示:/下注金额超过设置设置值则收到信息提醒,内线ID用#号分隔!/";
            string strOthe = "确定修改,guessset2.aspx?act=overcall,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("", "<br />"));

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl(@"..\guess2\default.aspx") + "\">返回上级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
            #endregion
        }
    }
    #endregion

    #region 确认设置 7
    /// <summary>
    /// 确认设置
    /// </summary>
    private void ConfirmSetPage()
    {
        Master.Title = "确认设置";
        //int ManageId = new BCW.User.Manage().IsManageLogin();
        //if (ManageId != 1 && ManageId != 9 && ManageId != 26)
        //{
        //    Utils.Error("权限不足", "");
        //}
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/guess2.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            #region 确定修改
            string rqRang = Utils.GetRequest("rqRang", "post", 2, @"^(\d)*(\.(\d){0,1})?$", "让球盘初盘变水范围填写错误");
            string dxRang = Utils.GetRequest("dxRang", "post", 2, @"^(\d)*(\.(\d){0,1})?$", "大小盘初盘变水范围填写错误");
            string bzRang = Utils.GetRequest("bzRang", "post", 2, @"^(\d)*(\.(\d){0,1})?$", "标准盘初盘变水范围填写错误");
            string lqZdpkRang = Utils.GetRequest("lqZdpkRang", "post", 2, @"^(\d)*(\.(\d){0,1})?$", "篮球" + ub.Get("SiteGqText") + "让球盘变盘范围填写错误");
            string lqZddxRang = Utils.GetRequest("lqZddxRang", "post", 2, @"^(\d)*(\.(\d){0,1})?$", "篮球" + ub.Get("SiteGqText") + "大小盘变盘范围填写错误");

            //写入XML
            xml.dss["SiterqRang"] = rqRang;
            xml.dss["SitedxRang"] = dxRang;
            xml.dss["SitedbzRang"] = bzRang;
            xml.dss["SitedlqZdpkRang"] = lqZdpkRang;
            xml.dss["SitedlqZddxRang"] = lqZddxRang;

            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("确认设置", "设置成功，正在返回..", Utils.getUrl("guessset2.aspx?act=confirmset"), "1");
            #endregion
        }
        else
        {
            #region 设置
            builder.Append(Out.Div("title", "确认设置"));
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            GuessSetNav(7);
            builder.Append(Out.Tab("</div>", ""));

            string strText = "让球盘初盘变水范围(原始:0.1):/,大小盘初盘变水范围:/,标准盘初盘变水范围:/,篮球" + ub.Get("SiteGqText") + "让球盘变盘范围(原始2.5球):/,篮球" + ub.Get("SiteGqText") + "大小盘变盘范围(原始2.5球):/,";
            string strName = "rqRang,dxRang,bzRang,lqZdpkRang,lqZddxRang,backurl";
            string strType = "text,text,text,text,text,hidden";
            string strValu = xml.dss["SiterqRang"] + "'" + xml.dss["SitedxRang"] + "'" + xml.dss["SitedbzRang"] + "'" + xml.dss["SitedlqZdpkRang"] + "'" + xml.dss["SitedlqZddxRang"] + "'" + Utils.getPage(0) + "";
            string strEmpt = "false,false,false,false,false,false";

            string strIdea = "/";
            string strOthe = "确定修改,guessset2.aspx?act=confirmset,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("", "<br />"));

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl(@"..\guess2\default.aspx") + "\">返回上级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
            #endregion
        }
    }
    #endregion

    #region 导航菜单
    /// <summary>
    /// 导航菜单 0球彩设置,1足球分级,2篮球分级,3半场分级,4单节分级,5波胆,6超额设置,7确认设置
    /// </summary>
    /// <param name="n"></param>
    private void GuessSetNav(int n)
    {
        if (n == 0) builder.Append("球彩设置"); else builder.Append("<a href=\"" + Utils.getUrl("guessset2.aspx?backurl=" + Utils.getPage(0) + "") + "\">球彩设置</a>");
        if (n == 1) builder.Append("|足球分级"); else builder.Append("|<a href=\"" + Utils.getUrl("guessset2.aspx?act=betleven&amp;backurl=" + Utils.getPage(0) + "") + "\">足球分级</a>");
        if (n == 2) builder.Append("|篮球分级"); else builder.Append("|<a href=\"" + Utils.getUrl("guessset2.aspx?act=betleven2&amp;backurl=" + Utils.getPage(0) + "") + "\">篮球分级</a>");
        if (n == 3) builder.Append("|半场分级"); else builder.Append("|<a href=\"" + Utils.getUrl("guessset2.aspx?act=betleven3&amp;backurl=" + Utils.getPage(0) + "") + "\">半场分级</a>");
        if (n == 4) builder.Append("|单节分级"); else builder.Append("|<a href=\"" + Utils.getUrl("guessset2.aspx?act=betleven4&amp;backurl=" + Utils.getPage(0) + "") + "\">单节分级</a>");
        if (n == 5) builder.Append("|波胆"); else builder.Append("|<a href=\"" + Utils.getUrl("guessset2.aspx?act=score&amp;backurl=" + Utils.getPage(0) + "") + "\">波胆</a>");
        if (n == 6) builder.Append("|超额设置"); else builder.Append("|<a href=\"" + Utils.getUrl("guessset2.aspx?act=overcall&amp;backurl=" + Utils.getPage(0) + "") + "\">超额设置</a>");
        if (n == 7) builder.Append("|确认设置"); else builder.Append("|<a href=\"" + Utils.getUrl("guessset2.aspx?act=confirmset&amp;backurl=" + Utils.getPage(0) + "") + "\">确认设置</a>");
        //builder.Append("|<a href=\"" + Utils.getUrl("spkadminset.aspx?ptype=5&amp;backurl=" + Utils.PostPage(true) + "") + "\">管理员</a>");
    }
    #endregion

    #region UBB配置,貌似没用了
    private void UbbPage()
    {
        ub xml = new ub();
        string xmlPath = "/Controls/guess2.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string SystemUbb = Utils.GetRequest("SystemUbb", "post", 3, @"^[\s\S]{1,10000}$", "排版限10000字内");
            string IsUbb = Utils.GetRequest("IsUbb", "post", 2, @"^[0-1]$", "是否启用选择错误");

            xml.dss["SiteSystemUbb"] = SystemUbb;
            xml.dss["SiteIsUbb"] = IsUbb;

            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("球彩系统设置", "设置成功，正在返回..", Utils.getUrl("guessset2.aspx?act=ubb&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "球彩Ubb排版设计"));
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("guessset2.aspx?backurl=" + Utils.getPage(0) + "") + "\">球彩设置</a>");
            builder.Append(Out.Tab("</div>", ""));
            string strText = "请用彩版进行Ubb设计:/,是否启用:/,,";
            string strName = "SystemUbb,IsUbb,act,backurl";
            string strType = "big,select,hidden,hidden";
            string strValu = "" + xml.dss["SiteSystemUbb"] + "'" + xml.dss["SiteIsUbb"] + "'ubb'" + Utils.getPage(0) + "";
            string strEmpt = "true,0|不启用|1|启用,false,false";
            string strIdea = "/";
            string strOthe = "确定修改|reset,guessset2.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示:<br />不启用时请用以下地址查看效果<br />http://" + Utils.GetDomain() + "/bbs/guess2/default.aspx?act=demo");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("game.aspx") + "\">返回上级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
    #endregion

    #region 抓取配置
    /// <summary>
    /// 抓取配置
    /// </summary>
    private void GuessjcPage()
    {
        Master.Title = "球彩抓取设置";

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/guess2.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            #region 确定修改
            string zqstat = Utils.GetRequest("zqstat", "post", 2, @"^[(\u4e00-\u9fa5A-Za-z0-9)# -]+$", "联赛名称必须简体中文或者英文、数字，并用#分开");
            string lqstat = Utils.GetRequest("lqstat", "post", 2, @"^[(\u4e00-\u9fa5A-Za-z0-9)# -]+$", "联赛名称必须简体中文或者英文、数字，并用#分开");
            string gqstat = Utils.GetRequest("gqstat", "post", 3, @"^[(\u4e00-\u9fa5A-Za-z0-9)# -]+$", "联赛名称必须简体中文或者英文、数字，并用#分开");
            string gqstat2 = Utils.GetRequest("gqstat2", "post", 3, @"^[(\u4e00-\u9fa5A-Za-z0-9)# -]+$", "联赛名称必须简体中文或者英文、数字，并用#分开");
            string bdstat = Utils.GetRequest("bdstat", "post", 3, @"^[(\u4e00-\u9fa5A-Za-z0-9)# -]+$", "联赛名称必须简体中文或者英文、数字，并用#分开");
            string onceType = Utils.GetRequest("onceType", "post", 2, @"^[0-1]$", "走地返彩方式选择错误");

            if (Utils.Left(zqstat, 1) == "#" || Utils.Right(zqstat, 1) == "#")
            {
                Utils.Error("足球联赛不能以#开头和#结尾..", "");
            }
            if (Utils.Left(lqstat, 1) == "#" || Utils.Right(lqstat, 1) == "#")
            {
                Utils.Error("篮球联赛不能以#开头和#结尾..", "");
            }
            if (gqstat != "")
            {
                if (Utils.Left(gqstat, 1) == "#" || Utils.Right(gqstat, 1) == "#")
                {
                    Utils.Error("足球滚球联赛不能以#开头和#结尾..", "");
                }
            }
            if (gqstat2 != "")
            {
                if (Utils.Left(gqstat2, 1) == "#" || Utils.Right(gqstat2, 1) == "#")
                {
                    Utils.Error("篮球滚球联赛不能以#开头和#结尾..", "");
                }
            }
            string[] zqstatTemp = Regex.Split(zqstat, "#");
            for (int i = 0; i < zqstatTemp.Length; i++)
            {
                int Num = Utils.GetStringNum("#" + zqstat + "#", "#" + zqstatTemp[i] + "#");
                if (Num >= 2)
                {
                    Utils.Error("足球联赛名称“" + zqstatTemp[i] + "”重复输入", "");
                }
            }
            string[] lqstatTemp = Regex.Split(lqstat, "#");
            for (int i = 0; i < lqstatTemp.Length; i++)
            {
                int Num = Utils.GetStringNum("#" + lqstat + "#", "#" + lqstatTemp[i] + "#");
                if (Num >= 2)
                {
                    Utils.Error("篮球联赛名称“" + lqstatTemp[i] + "”重复输入", "");
                }
            }
            string[] gqstatTemp = Regex.Split(gqstat, "#");
            for (int i = 0; i < gqstatTemp.Length; i++)
            {
                int Num = Utils.GetStringNum("#" + gqstat + "#", "#" + gqstatTemp[i] + "#");
                if (Num >= 2)
                {
                    Utils.Error("足球滚球联赛名称“" + gqstatTemp[i] + "”重复输入", "");
                }
            }
            string[] gqstat2Temp = Regex.Split(gqstat2, "#");
            for (int i = 0; i < gqstat2Temp.Length; i++)
            {
                int Num = Utils.GetStringNum("#" + gqstat2 + "#", "#" + gqstat2Temp[i] + "#");
                if (Num >= 2)
                {
                    Utils.Error("篮球滚球联赛名称“" + gqstat2Temp[i] + "”重复输入", "");
                }
            }

            string zqhalf = Utils.GetRequest("zqhalf", "post", 3, @"^[(\u4e00-\u9fa5A-Za-z0-9)# -]+$", "联赛名称必须简体中文或者英文、数字，并用#分开");
            string lqhalf = Utils.GetRequest("lqhalf", "post", 3, @"^[(\u4e00-\u9fa5A-Za-z0-9)# -]+$", "联赛名称必须简体中文或者英文、数字，并用#分开");
            //篮球单节无数据暂用空值替换全部抓取
            zqhalf = "";
            lqhalf = "";
            xml.dss["Sitezqhalf"] = zqhalf;
            xml.dss["Sitelqhalf"] = lqhalf;
            string gqstat3 = Utils.GetRequest("gqstat3", "post", 3, @"^[(\u4e00-\u9fa5A-Za-z0-9)# -]+$", "联赛名称必须简体中文或者英文、数字，并用#分开");
            xml.dss["Sitegqstat3"] = gqstat3;

            //写入XML
            xml.dss["Sitezqstat"] = zqstat;
            xml.dss["Sitelqstat"] = lqstat;
            xml.dss["Sitegqstat"] = gqstat;
            xml.dss["Sitegqstat2"] = gqstat2;
            xml.dss["Sitebdstat"] = bdstat;
            xml.dss["SiteonceType"] = onceType;

            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("球彩系统设置", "设置成功，正在返回..", Utils.getUrl("guessset2.aspx?act=guessjc"), "1");
            #endregion
        }
        else
        {
            #region 设置
            builder.Append(Out.Div("title", "球彩抓取设置"));

            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("guessset2.aspx?act=source") + "\">来源配置</a>");
            builder.Append("|抓取配置");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "要开的足球联赛:/,要开的篮球联赛:/,要开的足球滚球联赛:/,要开的篮球滚球联赛:/,要开的波胆联赛:/,走地返彩方式:/,";
            string strName = "zqstat,lqstat,gqstat,gqstat2,bdstat,onceType,backurl";
            string strType = "big,big,big,big,big,select,hidden";
            string strValu = "" + xml.dss["Sitezqstat"] + "'" + xml.dss["Sitelqstat"] + "'" + xml.dss["Sitegqstat"] + "'" + xml.dss["Sitegqstat2"] + "'" + xml.dss["Sitebdstat"] + "'" + xml.dss["SiteonceType"] + "'" + Utils.getPage(0) + "";
            string strEmpt = "true,true,true,true,true,0|自动开奖|1|手工开奖,false";

            strText += ",要开的足球半场联赛(已禁用):/,要开的篮球单节联赛(已禁用):/";
            strName += ",zqhalf,lqhalf";
            strType += ",big,big";
            strValu += "'" + xml.dss["Sitezqhalf"] + "'" + xml.dss["Sitelqhalf"] + "";
            strEmpt += ",true,true";


            strText += ",要开的足球半场滚球联赛:/";
            strName += ",gqstat3";
            strType += ",big";
            strValu += "'" + xml.dss["Sitegqstat3"] + "";
            strEmpt += ",true";

            string strIdea = "/温馨提示:/联赛名称必须简体中文或者英文、数字，并用#分开/";
            string strOthe = "确定修改,guessset2.aspx?act=guessjc,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("", "<br />"));

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl(@"..\guess2\default.aspx") + "\">返回上级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
            #endregion
        }
    }
    #endregion

    #region 球彩来源设置
    /// <summary>
    /// 球彩来源设置
    /// </summary>
    private void SourcePage()
    {
        Master.Title = "球彩来源设置";

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/guess2.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            #region 确定修改
            string zqfull = Utils.GetRequest("zqfull", "post", 2, @"^[0-2]$", "足球全场源选择错误");
            string zqhalf = Utils.GetRequest("zqhalf", "post", 2, @"^[0-2]$", "足球半场源选择错误");
            string lqfull = Utils.GetRequest("lqfull", "post", 2, @"^[0-2]$", "篮球全场源选择错误");
            string lqhalf = Utils.GetRequest("lqhalf", "post", 2, @"^[0-2]$", "篮球半场源选择错误");
            string lqsinger = Utils.GetRequest("lqsinger", "post", 2, @"^[0-2]$", "篮球单节源选择错误");
            string zqsorce = Utils.GetRequest("zqsorce", "post", 2, @"^[0-2]$", "足球波胆源选择错误");


            //写入XML
            xml.dss["Sitezqfull"] = zqfull;
            xml.dss["Sitezqhalf"] = zqhalf;
            xml.dss["Sitelqfull"] = lqfull;
            xml.dss["Sitelqhalf"] = lqhalf;
            xml.dss["Sitelqsinger"] = lqsinger;
            xml.dss["Sitezqsorce"] = zqsorce;

            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("球彩系统设置", "设置成功，正在返回..", Utils.getUrl("guessset2.aspx?act=source"), "1");
            #endregion
        }
        else
        {
            #region 设置
            builder.Append(Out.Div("title", "球彩来源设置"));

            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("来源配置");
            builder.Append("|<a href=\"" + Utils.getUrl("guessset2.aspx?act=guessjc") + "\">抓取配置</a>");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "足球全场:/,足球半场:/,篮球全场:/,篮球半场:/,篮球单节:/,波胆:/,";
            string strName = "zqfull,zqhalf,lqfull,lqhalf,lqSinger,zqsorce,backurl";
            string strType = "select,select,select,select,select,select,hidden";
            string strValu = "" + xml.dss["Sitezqfull"] + "'" + xml.dss["Sitezqhalf"] + "'" + xml.dss["Sitelqfull"] + "'" + xml.dss["Sitelqhalf"] + "'" + xml.dss["Sitelqsinger"] + "'" + xml.dss["Sitezqsorce"] + "'" + Utils.getPage(0) + "";
            string strEmpt = "0|电脑8波|1|球探网,0|电脑8波|1|球探网,0|电脑8波,0|球探网,0|球探网,0|电脑8波,false";

            string strIdea = "/温馨提示:/联赛来源修改后前台列表将按配置的来源显示!/";
            string strOthe = "确定修改,guessset2.aspx?act=source,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("", "<br />"));

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl(@"..\guess2\default.aspx") + "\">返回上级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
            #endregion
        }
    }
    #endregion
}