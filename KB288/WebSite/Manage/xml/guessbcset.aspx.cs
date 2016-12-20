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
using BCW.Common;

public partial class Manage_xml_guessbcset : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "球彩系统设置";
        string act = Utils.GetRequest("act", "all", 1, "", "");

        switch (act)
        {
            case "betleven":
                BetLevenPage();
                break;
            case "betleven2":
                BetLeven2Page();
                break;
            case "jc":
                JcPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/guessbc.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {

            string Name = Utils.GetRequest("Name", "post", 2, @"^[\u4e00-\u9fa5A-Za-z0-9]{1,20}$", "系统名称应为中文、英文、数字的组合,长度限20字内");
            string Logo = Utils.GetRequest("Logo", "post", 3, @"^[\s\S]{0,500}$", "Logo地址不能超过500字");
            string BigPay = Utils.GetRequest("BigPay", "post", 2, @"^[0-9]\d*$", "最大下注数量填写错误");
            string SmallPay = Utils.GetRequest("SmallPay", "post", 2, @"^[0-9]\d*$", "最小下注填写错误");
            string PayNum = Utils.GetRequest("PayNum", "post", 2, @"^\d+?$", "每场每项下注次数填写错误");
            string PayCent = Utils.GetRequest("PayCent", "post", 3, @"^\d+?$", "每场每ID下注上限填写错误");
            string MaxNum = Utils.GetRequest("MaxNum", "post", 2, @"^\d+?$", "每场下注上限填写错误，不限制请填写0");
            string MaxNumGid = Utils.GetRequest("MaxNumGid", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,1000}$", "每场下注上限赛事ID填写错误");
            string vice = Utils.GetRequest("vice", "post", 3, @"^[\s\S]{1,2000}$", "顶部Ubb不能超过2000字");
            string foot = Utils.GetRequest("foot", "post", 3, @"^[\s\S]{1,2000}$", "底部Ubb不能超过2000字");
            string Isbz = Utils.GetRequest("Isbz", "post", 2, @"^[0-1]$", "是否开标准盘选择错误");
            string Isyc = Utils.GetRequest("Isyc", "post", 2, @"^[0-1]$", "抓取方式选择错误");
            string Expir = Utils.GetRequest("Expir", "post", 2, @"^[0-9]\d*$", "防刷秒数填写错误");
            string DemoIDS = Utils.GetRequest("DemoIDS", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,5000}$", "内测ID填写错误");

            string SmallPay3 = Utils.GetRequest("SmallPay3", "post", 2, @"^[0-9]\d*$", "最小下注填写错误");
            string BigPay3 = Utils.GetRequest("BigPay3", "post", 2, @"^[0-9]\d*$", "最大下注填写错误");
            string IDMaxPay = Utils.GetRequest("IDMaxPay", "post", 2, @"^[0-9]\d*$", "每ID每场最大下注填写错误");

            //写入XML
            xml.dss["SiteName"] = Name;
            xml.dss["SiteLogo"] = Logo;
            xml.dss["SiteBigPay"] = BigPay;
            xml.dss["SiteSmallPay"] = SmallPay;
            xml.dss["SitePayNum"] = PayNum;
            xml.dss["SitePayCent"] = PayCent;
            xml.dss["SiteMaxNum"] = MaxNum;
            xml.dss["SiteMaxNumGid"] = MaxNumGid;
            xml.dss["Sitevice"] = vice;
            xml.dss["SiteFoot"] = foot;
            xml.dss["SiteIsbz"] = Isbz;
            xml.dss["SiteIsyc"] = Isyc;
            xml.dss["SiteExpir"] = Expir;
            xml.dss["SiteDemoIDS"] = DemoIDS;
            xml.dss["SiteSmallPay3"] = SmallPay3;
            xml.dss["SiteBigPay3"] = BigPay3;
            xml.dss["SiteIDMaxPay"] = IDMaxPay;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("球彩系统设置", "设置成功，正在返回..", Utils.getUrl("guessbcset.aspx?backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "球彩系统设置"));
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("球彩设置|<a href=\"" + Utils.getUrl("spkadminset.aspx?ptype=23&amp;backurl=" + Utils.PostPage(true) + "") + "\">管理员</a>");
            builder.Append("|<a href=\"" + Utils.getUrl("guessbcset.aspx?act=jc&amp;backurl=" + Utils.getPage(0) + "") + "\">抓取配置</a>");
            if (Utils.GetDomain().Contains("kb288") || Utils.GetDomain().Contains("kubao") || Utils.GetDomain().Contains("tuhao") || Utils.GetDomain().Contains("th") || Utils.GetDomain().Contains("127.0.0.6"))
            {
                builder.Append("|<a href=\"" + Utils.getUrl("guessbcset.aspx?act=betleven&amp;backurl=" + Utils.getPage(0) + "") + "\">足球分级</a>");
                builder.Append("|<a href=\"" + Utils.getUrl("guessbcset.aspx?act=betleven2&amp;backurl=" + Utils.getPage(0) + "") + "\">篮球分级</a>");
            }
            builder.Append(Out.Tab("</div>", ""));
            string strText = "*系统名称:/,系统LOGO:/,最小下注:/,最大下注:/,每场每项每ID下注限次数:(0为不限制)/,每场每ID下注上限:(0为不限制)/,每场下注上限(0为不限制):/,每场下注上限赛事ID:/,下注防刷秒数:/,顶部Ubb:/,底部Ubb:/,是否开标准盘:/,抓取方式:/,内测ID(留空则全部开放):/,";
            string strName = "Name,Logo,SmallPay,BigPay,PayNum,PayCent,MaxNum,MaxNumGid,Expir,vice,foot,Isbz,Isyc,DemoIDS,backurl";
            string strType = "text,text,num,num,num,num,num,text,num,text,textarea,hidden,select,text,hidden";
            string strValu = "" + xml.dss["SiteName"] + "'" + xml.dss["SiteLogo"] + "'" + xml.dss["SiteSmallPay"] + "'" + xml.dss["SiteBigPay"] + "'" + xml.dss["SitePayNum"] + "'" + xml.dss["SitePayCent"] + "'" + xml.dss["SiteMaxNum"] + "'" + xml.dss["SiteMaxNumGid"] + "'" + xml.dss["SiteExpir"] + "'" + xml.dss["Sitevice"] + "'" + xml.dss["SiteFoot"] + "'" + xml.dss["SiteIsbz"] + "'" + xml.dss["SiteIsyc"] + "'" + xml.dss["SiteDemoIDS"] + "'" + Utils.getPage(0) + "";
            string strEmpt = "false,true,false,false,false,false,false,true,false,true,true,0|开启|1|关闭,0|抓取即显示|1|抓取先隐藏,true,false";
            string strIdea = "/温馨提示:/每场下注上限赛事ID用#分开，留空则全部赛事有效/";
            string strOthe = "确定修改,guessbcset.aspx,post,1,red";

            strText += ",最小下注(" + ub.Get("SiteBz2") + "):/,最大下注(" + ub.Get("SiteBz2") + "):/,每ID每场最大下注(" + ub.Get("SiteBz2") + ")填0则不限制:/";
            strName += ",SmallPay3,BigPay3,IDMaxPay";
            strType += ",num,num,num";
            strValu += "'" + xml.dss["SiteSmallPay3"] + "'" + xml.dss["SiteBigPay3"] + "'" + xml.dss["SiteIDMaxPay"] + "";
            strEmpt += ",false,false,false";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("game.aspx") + "\">返回上级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }

    private void BetLevenPage()
    {
        ub xml = new ub();
        string xmlPath = "/Controls/guessbc.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string Leven1 = Utils.ToSChinese(Utils.GetRequest("Leven1", "post", 3, @"^[(\u4e00-\u9fa5A-Za-z0-9)# -]+$", "一级赛填写错误，联赛名称必须简体中文或者英文、数字，并用#分开"));
            string Leven2 = Utils.ToSChinese(Utils.GetRequest("Leven2", "post", 3, @"^[(\u4e00-\u9fa5A-Za-z0-9)# -]+$", "二级赛填写错误，联赛名称必须简体中文或者英文、数字，并用#分开"));
            string Leven3 = Utils.ToSChinese(Utils.GetRequest("Leven3", "post", 3, @"^[(\u4e00-\u9fa5A-Za-z0-9)# -]+$", "三级赛填写错误，联赛名称必须简体中文或者英文、数字，并用#分开"));
            string Leven4 = Utils.ToSChinese(Utils.GetRequest("Leven4", "post", 3, @"^[(\u4e00-\u9fa5A-Za-z0-9)# -]+$", "四级赛填写错误，联赛名称必须简体中文或者英文、数字，并用#分开"));
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

            xml.dss["SiteLeven1"] = Leven1;
            xml.dss["SiteLeven2"] = Leven2;
            xml.dss["SiteLeven3"] = Leven3;
            xml.dss["SiteLeven4"] = Leven4;

            xml.dss["SiteScent1"] = Scent1;
            xml.dss["SiteScent2"] = Scent2;
            xml.dss["SiteScent3"] = Scent3;
            xml.dss["SiteScent4"] = Scent4;

            xml.dss["SiteDcent1"] = Dcent1;
            xml.dss["SiteDcent2"] = Dcent2;
            xml.dss["SiteDcent3"] = Dcent3;
            xml.dss["SiteDcent4"] = Dcent4;

            xml.dss["SiteBcent1"] = Bcent1;
            xml.dss["SiteBcent2"] = Bcent2;
            xml.dss["SiteBcent3"] = Bcent3;
            xml.dss["SiteBcent4"] = Bcent4;


            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("球彩系统设置", "设置成功，正在返回..", Utils.getUrl("guessbcset.aspx?act=betleven&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "足球联赛分级设置"));
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("guessbcset.aspx?backurl=" + Utils.getPage(0) + "") + "\">球彩设置</a>");
            builder.Append("|<a href=\"" + Utils.getUrl("spkadminset.aspx?ptype=5&amp;backurl=" + Utils.PostPage(true) + "") + "\">管理员</a>");
            builder.Append("|足球分级");
            builder.Append("|<a href=\"" + Utils.getUrl("guessbcset.aspx?act=betleven2&amp;backurl=" + Utils.getPage(0) + "") + "\">篮球分级</a>");

            builder.Append(Out.Tab("</div>", ""));
            string strText = "=一级赛=/联赛名:,让球币:,大小币:,标准币:,=二级赛=/联赛名:,让球币:,大小币:,标准币:,=三级赛=/联赛名:,让球币:,大小币:,标准币:,=四级赛=/联赛名:,让球币:,大小币:,标准币:,,";
            string strName = "Leven1,Scent1,Dcent1,Bcent1,Leven2,Scent2,Dcent2,Bcent2,Leven3,Scent3,Dcent3,Bcent3,Leven4,Scent4,Dcent4,Bcent4,act,backurl";
            string strType = "textarea,num,num,num,textarea,num,num,num,textarea,num,num,num,textarea,num,num,num,hidden,hidden";
            string strValu = "" + xml.dss["SiteLeven1"] + "'" + xml.dss["SiteScent1"] + "'" + xml.dss["SiteDcent1"] + "'" + xml.dss["SiteBcent1"] + "'" + xml.dss["SiteLeven2"] + "'" + xml.dss["SiteScent2"] + "'" + xml.dss["SiteDcent2"] + "'" + xml.dss["SiteBcent2"] + "'" + xml.dss["SiteLeven3"] + "'" + xml.dss["SiteScent3"] + "'" + xml.dss["SiteDcent3"] + "'" + xml.dss["SiteBcent3"] + "'" + xml.dss["SiteLeven4"] + "'" + xml.dss["SiteScent4"] + "'" + xml.dss["SiteDcent4"] + "'" + xml.dss["SiteBcent4"] + "'betleven'" + Utils.getPage(0) + "";
            string strEmpt = "true,false,false,false,true,false,false,false,true,false,false,false,true,false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定修改|reset,guessbcset.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示:<br />联赛名称请用#分开，不限制币请填写0");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("game.aspx") + "\">返回上级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }

    }


    private void BetLeven2Page()
    {
        ub xml = new ub();
        string xmlPath = "/Controls/guessbc.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string Levenb1 = Utils.ToSChinese(Utils.GetRequest("Levenb1", "post", 3, @"^[(\u4e00-\u9fa5A-Za-z0-9)# -]+$", "一级赛填写错误，联赛名称必须简体中文或者英文、数字，并用#分开"));
            string Levenb2 = Utils.ToSChinese(Utils.GetRequest("Levenb2", "post", 3, @"^[(\u4e00-\u9fa5A-Za-z0-9)# -]+$", "二级赛填写错误，联赛名称必须简体中文或者英文、数字，并用#分开"));
            string Levenb3 = Utils.ToSChinese(Utils.GetRequest("Levenb3", "post", 3, @"^[(\u4e00-\u9fa5A-Za-z0-9)# -]+$", "三级赛填写错误，联赛名称必须简体中文或者英文、数字，并用#分开"));
            string Levenb4 = Utils.ToSChinese(Utils.GetRequest("Levenb4", "post", 3, @"^[(\u4e00-\u9fa5A-Za-z0-9)# -]+$", "四级赛填写错误，联赛名称必须简体中文或者英文、数字，并用#分开"));
            string Scentb1 = Utils.GetRequest("Scentb1", "post", 2, @"^[0-9]\d*$", "让球币填写错误");
            string Scentb2 = Utils.GetRequest("Scentb2", "post", 2, @"^[0-9]\d*$", "让球币填写错误");
            string Scentb3 = Utils.GetRequest("Scentb3", "post", 2, @"^[0-9]\d*$", "让球币填写错误");
            string Scentb4 = Utils.GetRequest("Scentb4", "post", 2, @"^[0-9]\d*$", "让球币填写错误");
            string Dcentb1 = Utils.GetRequest("Dcentb1", "post", 2, @"^[0-9]\d*$", "大小币填写错误");
            string Dcentb2 = Utils.GetRequest("Dcentb2", "post", 2, @"^[0-9]\d*$", "大小币填写错误");
            string Dcentb3 = Utils.GetRequest("Dcentb3", "post", 2, @"^[0-9]\d*$", "大小币填写错误");
            string Dcentb4 = Utils.GetRequest("Dcentb4", "post", 2, @"^[0-9]\d*$", "大小币填写错误");


            xml.dss["SiteLevenb1"] = Levenb1;
            xml.dss["SiteLevenb2"] = Levenb2;
            xml.dss["SiteLevenb3"] = Levenb3;
            xml.dss["SiteLevenb4"] = Levenb4;

            xml.dss["SiteScentb1"] = Scentb1;
            xml.dss["SiteScentb2"] = Scentb2;
            xml.dss["SiteScentb3"] = Scentb3;
            xml.dss["SiteScentb4"] = Scentb4;

            xml.dss["SiteDcentb1"] = Dcentb1;
            xml.dss["SiteDcentb2"] = Dcentb2;
            xml.dss["SiteDcentb3"] = Dcentb3;
            xml.dss["SiteDcentb4"] = Dcentb4;




            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("球彩系统设置", "设置成功，正在返回..", Utils.getUrl("guessbcset.aspx?act=betleven2&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "篮球联赛分级设置"));
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("guessbcset.aspx?backurl=" + Utils.getPage(0) + "") + "\">球彩设置</a>");
            builder.Append("|<a href=\"" + Utils.getUrl("spkadminset.aspx?ptype=5&amp;backurl=" + Utils.PostPage(true) + "") + "\">管理员</a>");
            builder.Append("|<a href=\"" + Utils.getUrl("guessbcset.aspx?act=betleven&amp;backurl=" + Utils.getPage(0) + "") + "\">足球分级</a>");
            builder.Append("|篮球分级");
            builder.Append(Out.Tab("</div>", ""));
            string strText = "=一级赛=/联赛名:,让球币:,大小币:,=二级赛=/联赛名:,让球币:,大小币:,=三级赛=/联赛名:,让球币:,大小币:,=四级赛=/联赛名:,让球币:,大小币:,,";
            string strName = "Levenb1,Scentb1,Dcentb1,Levenb2,Scentb2,Dcentb2,Levenb3,Scentb3,Dcentb3,Levenb4,Scentb4,Dcentb4,act,backurl";
            string strType = "textarea,num,num,textarea,num,num,textarea,num,num,textarea,num,num,hidden,hidden";
            string strValu = "" + xml.dss["SiteLevenb1"] + "'" + xml.dss["SiteScentb1"] + "'" + xml.dss["SiteDcentb1"] + "'" + xml.dss["SiteLevenb2"] + "'" + xml.dss["SiteScentb2"] + "'" + xml.dss["SiteDcentb2"] + "'" + xml.dss["SiteLevenb3"] + "'" + xml.dss["SiteScentb3"] + "'" + xml.dss["SiteDcentb3"] + "'" + xml.dss["SiteLevenb4"] + "'" + xml.dss["SiteScentb4"] + "'" + xml.dss["SiteDcentb4"] + "'betleven2'" + Utils.getPage(0) + "";
            string strEmpt = "true,false,false,true,false,false,true,false,false,true,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定修改|reset,guessbcset.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示:<br />联赛名称请用#分开，不限制币请填写0");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("game.aspx") + "\">返回上级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }


    private void JcPage()
    {
        ub xml = new ub();
        string xmlPath = "/Controls/guessbc.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        if (Utils.ToSChinese(ac) == "确定修改")
        {
          
            string gqstat3 = Utils.ToSChinese(Utils.GetRequest("gqstat3", "post", 3, @"^[(\u4e00-\u9fa5A-Za-z0-9)# -]+$", "" + ub.Get("SiteGqText") + "联赛名称必须简体中文或者英文、数字，并用#分开"));

            string zq1 = Utils.ToSChinese(Utils.GetRequest("zq1", "post", 3, @"^[(\u4e00-\u9fa5A-Za-z0-9)# -]+$", "半场联赛名称必须简体中文或者英文、数字，并用#分开"));
            string lq1 = Utils.ToSChinese(Utils.GetRequest("lq1", "post", 3, @"^[(\u4e00-\u9fa5A-Za-z0-9)# -]+$", "单节联赛名称必须简体中文或者英文、数字，并用#分开"));
            string lq2 = Utils.ToSChinese(Utils.GetRequest("lq2", "post", 3, @"^[(\u4e00-\u9fa5A-Za-z0-9)# -]+$", "单节联赛名称必须简体中文或者英文、数字，并用#分开"));
            string lq3 = Utils.ToSChinese(Utils.GetRequest("lq3", "post", 3, @"^[(\u4e00-\u9fa5A-Za-z0-9)# -]+$", "单节联赛名称必须简体中文或者英文、数字，并用#分开"));
            string lq4 = Utils.ToSChinese(Utils.GetRequest("lq4", "post", 3, @"^[(\u4e00-\u9fa5A-Za-z0-9)# -]+$", "单节联赛名称必须简体中文或者英文、数字，并用#分开"));

            string DsSet = Utils.GetRequest("DsSet", "post", 1, @"^[0-3]$", "3");
            string DsOdds = Utils.GetRequest("DsOdds", "post", 3, @"^(\d)*(\.(\d){1,2})?$", "单双默认赔率填写错误");

            xml.dss["Sitegqstat3"] = gqstat3;
            xml.dss["Sitezq1"] = zq1;
            xml.dss["Sitelq1"] = lq1;
            xml.dss["Sitelq2"] = lq2;
            xml.dss["Sitelq3"] = lq3;
            xml.dss["Sitelq4"] = lq4;
            xml.dss["SiteDsSet"] = DsSet;
            xml.dss["SiteDsOdds"] = DsOdds;

            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("球彩系统设置", "设置成功，正在返回..", Utils.getUrl("guessbcset.aspx?act=jc&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "半场单节抓取配置"));
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("guessbcset.aspx?backurl=" + Utils.getPage(0) + "") + "\">球彩设置</a>|抓取配置");
            builder.Append(Out.Tab("</div>", ""));
            string strText = "=足球联赛=/上半场:/,=篮球联赛=/第一节:/,第二节:/,上半场:/,第三节:/,,";
            string strName = "zq1,lq1,lq2,lq3,lq4,act,backurl";
            string strType = "big,textarea,textarea,textarea,textarea,hidden,hidden";
            string strValu = "" + xml.dss["Sitezq1"] + "'" + xml.dss["Sitelq1"] + "'" + xml.dss["Sitelq2"] + "'" + xml.dss["Sitelq3"] + "'" + xml.dss["Sitelq4"] + "'jc'" + Utils.getPage(0) + "";
            string strEmpt = "true,true,true,true,true,false,false";

            strText += ",=走地联赛=/要开的足球半场走地联赛:/";
            strName += ",gqstat3";
            strType += ",big";
            strValu += "'" + xml.dss["Sitegqstat3"] + "";
            strEmpt += ",true";

            //strText += ",单双盘开关:/,单双盘默认赔率:/";
            //strName += ",DsSet,DsOdds";
            //strType += ",select,text";
            //strValu += "'" + xml.dss["SiteDsSet"] + "'" + xml.dss["SiteDsOdds"] + "";
            //strEmpt += ",0|全部写入|1|写入篮球|2|写入足球|3|全部关闭,true";
            string strIdea = "/";
            string strOthe = "确定修改|reset,guessbcset.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示:<br />联赛名称请用#分开");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("game.aspx") + "\">返回上级</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
}