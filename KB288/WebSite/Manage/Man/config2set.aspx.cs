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

public partial class Manage_Man_config2set : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "系统基本设置";
        builder.Append(Out.Tab("", ""));
        int ptype = 1;
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        Application.Remove("xml_wap");//清缓存
        xml.Reload(); //加载网站配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            if (ptype == 0)
            {
                string Name = Utils.GetRequest("Name", "post", 2, @"^[^\^]{1,100}$", "系统名称限1-100字内");
                string Logo = Utils.GetRequest("Logo", "post", 3, @"^[^\^]{1,200}$", "Logo地址限200字内");
                string Status = Utils.GetRequest("Status", "post", 2, @"^[0-2]$", "系统状态选择出错");
                string WapBb = Utils.GetRequest("WapBb", "post", 2, @"^[0-3]$", "默认版本选择出错");
                string IsPC = Utils.GetRequest("IsPC", "post", 2, @"^[0-1]$", "浏览限制选择出错");
                string Align = Utils.GetRequest("Align", "post", 2, @"^left|center|right$", "网站对齐选择错误");
                string VCountType = Utils.GetRequest("VCountType", "post", 2, @"^[0-2]$", "流量统计类型选择错误");
                string bbsName = Utils.GetRequest("bbsName", "post", 2, @"^[^\^]{1,20}$", "社区名称限1-20字内");
                string bbsLogo = Utils.GetRequest("bbsLogo", "post", 3, @"^[^\^]{1,200}$", "社区Logo地址限200字内");
                string bbsStatus = Utils.GetRequest("bbsStatus", "post", 2, @"^[0-2]$", "社区状态选择出错");
                string forumName = Utils.GetRequest("forumName", "post", 2, @"^[^\^]{1,20}$", "论坛名称限1-20字内");
                string forumLogo = Utils.GetRequest("forumLogo", "post", 3, @"^[^\^]{1,200}$", "论坛Logo地址限200字内");
                string forumStatus = Utils.GetRequest("forumStatus", "post", 2, @"^[0-2]$", "论坛状态选择出错");
                string ExTime = Utils.GetRequest("ExTime", "post", 2, @"^[0-9]\d*$", "会员离线时间填写出错");
                string IsModel = Utils.GetRequest("IsModel", "post", 2, @"^[0-1]$", "是否开启机型适配选择出错");
                string Bz = Utils.GetRequest("Bz", "post", 2, @"^[A-Za-zＡ-Ｚ\d０-９\u4E00-\u9FA5]{1,3}$", "请输入不超过3字虚拟币种名称");
                string Bz2 = Utils.GetRequest("Bz2", "post", 2, @"^[A-Za-zＡ-Ｚ\d０-９\u4E00-\u9FA5]{1,3}$", "请输入不超过3字充值币种名称");
                string ListNo = Utils.GetRequest("ListNo", "post", 2, @"^[0-9]\d*$", "列表数必为数字");
                string Hr = Utils.GetRequest("Hr", "post", 2, @"^[\s\S]{1,100}", "分隔线限100字符");
                string Sensitive = Utils.GetRequest("Sensitive", "post", 3, @"^[^\#]{2,50}(?:\#[^\#]{2,50}){0,500}$", "敏感词请用#分开，并且每个敏感词须两字以上");
                string WelNotes = Utils.GetRequest("WelNotes", "post", 2, @"^[\s\S]{1,5000}", "欢迎语限5000字内");
                string ManIDS = Utils.GetRequest("ManIDS", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "系统前台管理员用#分开");
                string ManExpir = Utils.GetRequest("ManExpir", "post", 3, @"^[0-9]\d*$", "系统前台管理超时分钟填写出错");
                xml.ds["SiteName"] = Name;
                xml.ds["SiteLogo"] = Logo;
                xml.ds["SiteStatus"] = Status;
                xml.ds["SiteWapBb"] = WapBb;
                xml.ds["SiteIsPC"] = IsPC;
                xml.ds["SitebbsName"] = bbsName;
                xml.ds["SitebbsLogo"] = bbsLogo;
                xml.ds["SitebbsStatus"] = bbsStatus;
                xml.ds["SiteforumName"] = forumName;
                xml.ds["SiteforumLogo"] = forumLogo;
                xml.ds["SiteforumStatus"] = forumStatus;
                xml.ds["SiteExTime"] = ExTime;
                xml.ds["SiteAlign"] = Align;
                xml.ds["SiteVCountType"] = VCountType;
                xml.ds["SiteIsModel"] = IsModel;
                xml.ds["SiteBz"] = Bz;
                xml.ds["SiteBz2"] = Bz2;
                xml.ds["SiteListNo"] = ListNo;
                xml.ds["SiteHr"] = Hr;
                xml.ds["SiteSensitive"] = Sensitive;
                xml.ds["SiteWelNotes"] = WelNotes;
                xml.ds["SiteManIDS"] = ManIDS;
                xml.ds["SiteManExpir"] = ManExpir;
            }
            else
            {
                string IndexTopUbb = Utils.GetRequest("IndexTopUbb", "post", 3, @"^[\s\S]{1,5000}", "网站页面顶部UBB限5000字符");
                string BbsTopUbb = Utils.GetRequest("BbsTopUbb", "post", 3, @"^[\s\S]{1,5000}", "社区页面顶部UBB限5000字符");
                string GameTopUbb = Utils.GetRequest("GameTopUbb", "post", 3, @"^[\s\S]{1,5000}", "游戏页面顶部UBB限5000字符");
                string IndexUbb = Utils.GetRequest("IndexUbb", "post", 3, @"^[\s\S]{1,5000}", "网站首页底部UBB限5000字符");
                string WapUbb = Utils.GetRequest("WapUbb", "post", 2, @"^[\s\S]{1,5000}", "网站底部UBB限5000字符");
                string BbsIndexUbb = Utils.GetRequest("BbsIndexUbb", "post", 3, @"^[\s\S]{1,5000}", "社区首页底部UBB限5000字符");
                string BbsUbb = Utils.GetRequest("BbsUbb", "post", 2, @"^[\s\S]{1,5000}", "社区底部UBB限5000字符");
                string GameUbb = Utils.GetRequest("GameUbb", "post", 2, @"^[\s\S]{1,5000}", "游戏底部UBB限5000字符");

                xml.ds["SiteIndexTopUbb"] = IndexTopUbb;
                xml.ds["SiteBbsTopUbb"] = BbsTopUbb;
                xml.ds["SiteGameTopUbb"] = GameTopUbb;
                xml.ds["SiteIndexUbb"] = IndexUbb;
                xml.ds["SiteWapUbb"] = WapUbb;
                xml.ds["SiteBbsIndexUbb"] = BbsIndexUbb;
                xml.ds["SiteBbsUbb"] = BbsUbb;
                xml.ds["SiteGameUbb"] = GameUbb;
            }
            System.IO.File.WriteAllText(Server.MapPath("~/Controls/wap.xml"), xml.Post(xml.ds), System.Text.Encoding.UTF8);

            Utils.Success("设置参数", "设置成功，正在返回..", Utils.getUrl("config2set.aspx?ptype=" + ptype + ""), "1");
        }
        else
        {

            builder.Append(Out.Div("title", "系统参数设置"));
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            if (ptype == 0)
            {
                builder.Append("基本设置|");
                builder.Append("<a href=\"" + Utils.getUrl("config.aspx?ptype=1") + "\">顶部与底部</a>");
            }
            else
            {
                //builder.Append("<a href=\"" + Utils.getUrl("config.aspx?ptype=0") + "\">基本设置</a>|");
                builder.Append("顶部与底部");
            }
            builder.Append(Out.Tab("</div>", ""));
            if (ptype == 0)
            {
                string strText = "系统名称:/,系统Logo(可留空):/,系统状态:/,默认版本:/,浏览限制:/,网站对齐:/,流量统计:/,社区名称:/,社区Logo:/,社区状态:/,论坛名称:/,论坛Logo:/,论坛状态:/,会员离线时间(分钟):/,是否开启机型适配:/,虚拟币种:/,充值币种:/,页面列表条数:/,分隔线:/,敏感词屏蔽(用#分开，敏感词两字以上):/,书签登录欢迎语(支持Ubb):/,后台二级管理员(如填写1即为1号管理员，多个用#分开):/,系统前台管理超时(分钟):/";
                string strName = "Name,Logo,Status,WapBb,IsPC,Align,VCountType,bbsName,bbsLogo,bbsStatus,forumName,forumLogo,forumStatus,ExTime,IsModel,Bz,Bz2,ListNo,Hr,Sensitive,WelNotes,ManIDS,ManExpir";
                string strType = "text,text,select,select,select,select,select,text,text,select,text,text,select,num,select,text,text,num,text,text,textarea,text,hidden";
                string strValu = "" + xml.ds["SiteName"] + "'" + xml.ds["SiteLogo"] + "'" + xml.ds["SiteStatus"] + "'" + xml.ds["SiteWapBb"] + "'" + xml.ds["SiteIsPC"] + "'" + xml.ds["SiteAlign"] + "'" + xml.ds["SiteVCountType"] + "'" + xml.ds["SitebbsName"] + "'" + xml.ds["SitebbsLogo"] + "'" + xml.ds["SitebbsStatus"] + "'" + xml.ds["SiteforumName"] + "'" + xml.ds["SiteforumLogo"] + "'" + xml.ds["SiteforumStatus"] + "'" + xml.ds["SiteExTime"] + "'" + xml.ds["SiteIsModel"] + "'" + xml.ds["SiteBz"] + "'" + xml.ds["SiteBz2"] + "'" + xml.ds["SiteListNo"] + "'" + xml.ds["SiteHr"] + "'" + xml.ds["SiteSensitive"] + "'" + xml.ds["SiteWelNotes"] + "'" + xml.ds["SiteManIDS"] + "'" + xml.ds["SiteManExpir"] + "";
                string strEmpt = "false,true,0|正常|1|维护|2|登录可进,0|自动适配|1|默认2.0|2|禁止1.0|3|禁止2.0,0|不限|1|限手机,left|居左|center|居中|right|居右,0|统计UV(推荐)|1|统计PV(耗资源)|2|关闭统计(省资源),false,true,0|正常|1|维护|2|登录可进,false,true,0|正常|1|维护|2|登录可进,false,0|不开启|1|已开启,false,flase,false,flase,true,true,true,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,config.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
            else
            {
                string strText = "网站页面顶部Ubb:/,社区页面顶部Ubb:/,游戏页面顶部Ubb:/,网站首页底部Ubb:/,网站底部Ubb:/,社区首页底部Ubb:/,社区底部Ubb:/,游戏底部Ubb:/,";
                string strName = "IndexTopUbb,BbsTopUbb,GameTopUbb,IndexUbb,WapUbb,BbsIndexUbb,BbsUbb,GameUbb,ptype";
                string strType = "textarea,textarea,textarea,textarea,textarea,textarea,textarea,textarea,hidden";
                string strValu = "" + xml.ds["SiteIndexTopUbb"] + "'" + xml.ds["SiteBbsTopUbb"] + "'" + xml.ds["SiteGameTopUbb"] + "'" + xml.ds["SiteIndexUbb"] + "'" + xml.ds["SiteWapUbb"] + "'" + xml.ds["SiteBbsIndexUbb"] + "'" + xml.ds["SiteBbsUbb"] + "'" + xml.ds["SiteGameUbb"] + "'" + ptype + "";
                string strEmpt = "true,true,true,true,true,true,true,true,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,config2set.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
}
