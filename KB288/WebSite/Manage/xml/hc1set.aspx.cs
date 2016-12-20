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

public partial class Manage_xml_hc1set : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
       
        Master.Title = "好彩一游戏设置";
        builder.Append(Out.Tab("", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-1]$", "0"));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/hc1.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            if (ptype == 0)
            {
                string Name = Utils.GetRequest("Name", "post", 2, @"^[^\^]{1,20}$", "系统名称限1-20字内");
                string Logo = Utils.GetRequest("Logo", "post", 3, @"^[^\^]{1,200}$", "Logo地址限200字内");
                string Status = Utils.GetRequest("Status", "post", 2, @"^[0-1]$", "系统状态选择出错");
                string Expir = Utils.GetRequest("Expir", "post", 2, @"^[0-9]\d*$", "游戏防刷填写错误");
                string SmallPay = Utils.GetRequest("SmallPay", "post", 3, @"^[0-9]\d*$", "最小下注填写错误");
                string BigPay = Utils.GetRequest("BigPay", "post", 3, @"^[0-9]\d*$", "最大下注填写错误");
                string TopUbb = Utils.GetRequest("TopUbb", "post", 3, @"^[\s\S]{1,2000}$", "顶部Ubb限2000字内");
                string FootUbb = Utils.GetRequest("FootUbb", "post", 3, @"^[\s\S]{1,2000}$", "底部Ubb限2000字内");
                string DemoIDS = Utils.GetRequest("DemoIDS", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "内测ID填写错误，可留空");
                string Rule = Utils.GetRequest("Rule", "post", 3, @"^[\s\S]{1,5000}$", "规则最限5000字内");
                string Max = Utils.GetRequest("Max", "post", 4, @"^[0-9]\d*$", "每ID每期下注金币上限填写错误");
                string Max2 = Utils.GetRequest("Max2", "post", 4, @"^[0-9]\d*$", "每期总下注金币上限填写错误");

                xml.dss["Hc1Name"] = Name;
                xml.dss["Hc1Logo"] = Logo;
                xml.dss["Hc1Status"] = Status;
                xml.dss["Hc1Expir"] = Expir;
                xml.dss["Hc1SmallPay"] = SmallPay;
                xml.dss["Hc1BigPay"] = BigPay;
                xml.dss["Hc1TopUbb"] = TopUbb;
                xml.dss["Hc1FootUbb"] = FootUbb;
                xml.dss["Hc1DemoIDS"] = DemoIDS;
                xml.dss["Hc1Rule"] = Rule;
                xml.dss["Hc1Max"] = Max;
                xml.dss["Hc1Max2"] = Max2;
            }
            else
            {
                string odds1 = Utils.GetRequest("odds1", "post", 2, @"^(\d)*(\.(\d){1,2})?$", "赔率填写错误");
                string odds2 = Utils.GetRequest("odds2", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "赔率填写错误");
                string odds3 = Utils.GetRequest("odds3", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "赔率填写错误");
                string odds4 = Utils.GetRequest("odds4", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "赔率填写错误");
                string odds5 = Utils.GetRequest("odds5", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "赔率填写错误");
                string odds6= Utils.GetRequest("odds6", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "赔率填写错误");
                string odds7 = Utils.GetRequest("odds7", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "赔率填写错误");
                string odds8 = Utils.GetRequest("odds8", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "赔率填写错误");
                string odds9 = Utils.GetRequest("odds9", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "赔率填写错误");
                string odds10 = Utils.GetRequest("odds10", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "赔率填写错误");
                string odds11= Utils.GetRequest("odds11", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "赔率填写错误");
                string odds12= Utils.GetRequest("odds12", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "赔率填写错误");
                string odds13 = Utils.GetRequest("odds13", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "赔率填写错误");
                string odds14 = Utils.GetRequest("odds14", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "赔率填写错误");
                string odds15 = Utils.GetRequest("odds15", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "赔率填写错误");
                string odds16 = Utils.GetRequest("odds16", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "赔率填写错误");
                string odds17 = Utils.GetRequest("odds17", "post", 2, @"^(\d)*(\.(\d){0,2})?$", "赔率填写错误");
                xml.dss["Hc1odds1"] = odds1;
                xml.dss["Hc1odds2"] = odds2;
                xml.dss["Hc1odds3"] = odds3;
                xml.dss["Hc1odds4"] = odds4;
                xml.dss["Hc1da"] = odds5;
                xml.dss["Hc1xiao"] = odds6;
                xml.dss["Hc1dan"] = odds7;
                xml.dss["Hc1shuang"] = odds8;
                xml.dss["Hc1odds9"] = odds9;
                xml.dss["Hc1wsda"] = odds10;
                xml.dss["Hc1wsxiao"] = odds11;
                xml.dss["Hc1wsdan"] = odds12;
                xml.dss["Hc1wsshuang"] = odds13;
                xml.dss["Hc1odds14"] = odds14;
                xml.dss["Hc1odds15"] = odds15;
                xml.dss["Hc1overpeilv"] = odds16;
                xml.dss["Hc1fudong"] = odds17;
            }
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("好彩一游戏设置", "设置成功，正在返回..", Utils.getUrl("hc1set.aspx?ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            if (ptype == 0)
            {
                builder.Append(Out.Div("title", "好彩一游戏设置"));
                builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                builder.Append("好彩一设置|<a href=\"" + Utils.getUrl("hc1set.aspx?ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">赔率</a>");
                builder.Append(Out.Tab("</div>", ""));
                string strText = "游戏名称:/,游戏Logo(可留空):/,游戏状态:/,游戏下注防刷(秒):/,最小下注:/,最大下注:/,顶部Ubb:/,底部Ubb:/,内测ID(留空则全部开放):/,游戏规则:(支持UBB)/,每ID每期下注" + ub.Get("SiteBz") + "上限:/,每期总下注" + ub.Get("SiteBz") + "上限:/,";
                string strName = "Name,Logo,Status,Expir,SmallPay,BigPay,TopUbb,FootUbb,DemoIDS,Rule,Max,Max2,backurl";
                string strType = "text,text,select,num,num,num,textarea,textarea,textarea,textarea,num,num,hidden";
                string strValu = "" + xml.dss["Hc1Name"] + "'" + xml.dss["Hc1Logo"] + "'" + xml.dss["Hc1Status"] + "'" + xml.dss["Hc1Expir"] + "'" + xml.dss["Hc1SmallPay"] + "'" + xml.dss["Hc1BigPay"] + "'" + xml.dss["Hc1TopUbb"] + "'" + xml.dss["Hc1FootUbb"] + "'" + xml.dss["Hc1DemoIDS"] + "'" + xml.dss["Hc1Rule"] + "'" + xml.dss["Hc1Max"] + "'" + xml.dss["Hc1Max2"] + "'" + Utils.getPage(0) + "";
                string strEmpt = "false,true,0|正常|1|维护,false,false,false,true,true,true,true,false,false,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,hc1set.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
            else
            {
                if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
                {
                    Utils.Success("温馨提示", "使用彩版，页面更直观，更快捷！正在切换进入...", "hc1set.aspx?ve=2a&amp;u=" + Utils.getstrU() + "", "1");
                }
                builder.Append(Out.Div("title", "好彩一赔率设置"));
                builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("hc1set.aspx?ptype=0&amp;backurl=" + Utils.getPage(0) + "") + "\">好彩一设置</a>|赔率</a>");
                builder.Append(Out.Tab("</div>", ""));
                string strText = "=赔率设置(支持小数)=/号码赔率:/,生肖赔率:/,方位赔率:/,四季赔率:/,大数赔率:/,小数赔率:/,单数赔率:/,双数赔率:/,六肖赔率:/,尾数大数赔率:/,尾数小数赔率:/,尾数单数赔率:/,尾数双数赔率:/,家禽赔率:/,野兽赔率:/,大小双单最大赔率:/,大小双单浮动赔率:/,七不中赔率:/,八不中赔率:/,九不中赔率:/,十不中赔率:/,,";
                string strName = "odds1,odds2,odds3,odds4,odds5,odds6,odds7,odds8,odds9,odds10,odds11,odds12,odds13,odds14,odds15,odds16,odds17,odds18,odds19,odds20,odds21,ptype,backurl";
                string strType = "text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,hidden,hidden,hidden,hidden,hidden,hidden";
                string strValu = "" + xml.dss["Hc1odds1"] + "'" + xml.dss["Hc1odds2"] + "'" + xml.dss["Hc1odds3"] + "'" + xml.dss["Hc1odds4"] + "'" + xml.dss["Hc1da"] + "'" + xml.dss["Hc1xiao"] + "'" + xml.dss["Hc1dan"] + "'" + xml.dss["Hc1shuang"] + "'" + xml.dss["Hc1odds9"] + "'" + xml.dss["Hc1wsda"] + "'" + xml.dss["Hc1wsxiao"] + "'" + xml.dss["Hc1wsdan"] + "'" + xml.dss["Hc1wsshuang"] + "'" + xml.dss["Hc1odds14"] + "'" + xml.dss["Hc1odds15"] + "'" + xml.dss["Hc1overpeilv"] + "'" + xml.dss["Hc1fudong"] + "'" + xml.dss["Hc1odds18"] + "'" + xml.dss["Hc1odds19"] + "'" + xml.dss["Hc1odds20"] + "'" + xml.dss["Hc1odds21"] + "'"+ptype+"'" + Utils.getPage(0) + "";
                string strEmpt = "false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,hc1set.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("game.aspx") + "\">返回上级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
}
