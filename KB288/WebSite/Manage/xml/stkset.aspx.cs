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

public partial class Manage_xml_stkset : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "上证指数竞猜设置";
        builder.Append(Out.Tab("", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-1]$", "0"));
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
                string Status = Utils.GetRequest("Status", "post", 2, @"^[0-1]$", "系统状态选择出错");
                string OpenType = Utils.GetRequest("OpenType", "post", 2, @"^[0-2]$", "开奖类型选择出错");
                string OpenTime = Utils.GetRequest("OpenTime", "post", 2, @"^(?:[0]?\d|1\d|2[0123]):(?:[0-5]\d)$", "每天开奖时间填写错误，正确格式如16:00");
                string Tax = Utils.GetRequest("Tax", "post", 2, @"^[0-9]\d*$", "税率填写错误");
                string SmallPay = Utils.GetRequest("SmallPay", "post", 2, @"^[0-9]\d*$", "最小下注填写错误");
                string BigPay = Utils.GetRequest("BigPay", "post", 2, @"^[0-9]\d*$", "最大下注填写错误");
                string Expir = Utils.GetRequest("Expir", "post", 2, @"^[0-9]\d*$", "防刷秒数填写错误");
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
                xml.dss["StkExpir"] = Expir;
                xml.dss["StkFoot"] = Foot;
                xml.dss["StkGuestSet"] = GuestSet;
            }
            else
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
                string Odds6 =Utils.GetRequest("Odds6", "post", 2, @"^(0|([1-9]+[0-9]*))(.[0-9]+)?$", "赔率填写错误");
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
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("上证指数竞猜设置", "设置成功，正在返回..", Utils.getUrl("stkset.aspx?ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "上证指数竞猜设置"));
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            if (ptype == 0)
            {
                builder.Append("上证设置|");
                builder.Append("<a href=\"" + Utils.getUrl("stkset.aspx?ptype=1") + "\">赔率</a>");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("stkset.aspx?ptype=0") + "\">上证设置</a>");
                builder.Append("|赔率");
            }
            builder.Append("|<a href=\"" + Utils.getUrl("spkadminset.aspx?ptype=11&amp;backurl=" + Utils.PostPage(true) + "") + "\">管理员</a>");
            builder.Append(Out.Tab("</div>", ""));
            if (ptype == 0)
            {
                string strText = "游戏名称:/,游戏口号(可留空):/,游戏Logo(可留空):/,游戏状态:/,开奖类型:/,每天自动开奖时间:/,系统收税(%):/,最小下注:/,最大下注:/,下注防刷(秒):/,底部Ubb:/,兑奖内线:/,";
                string strName = "Name,Notes,Logo,Status,OpenType,OpenTime,Tax,SmallPay,BigPay,Expir,Foot,GuestSet,backurl";
                string strType = "text,text,text,select,select,text,num,num,num,num,textarea,select,hidden";
                string strValu = "" + xml.dss["StkName"] + "'" + xml.dss["StkNotes"] + "'" + xml.dss["StkLogo"] + "'" + xml.dss["StkStatus"] + "'" + xml.dss["StkOpenType"] + "'" + xml.dss["StkOpenTime"] + "'" + xml.dss["StkTax"] + "'" + xml.dss["StkSmallPay"] + "'" + xml.dss["StkBigPay"] + "'" + xml.dss["StkExpir"] + "'" + xml.dss["StkFoot"] + "'" + xml.dss["StkGuestSet"] + "'" + Utils.getPage(0) + "";
                string strEmpt = "false,true,true,0|正常|1|维护,0|自动开奖|1|手工开奖,false,false,false,false,false,true,0|开启|1|关闭,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,stkset.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("温馨提示:<br />选择自动开奖则自动抓取指数开奖并自动开通下一期上证.<br />选择手工开奖时请在上证管理中开奖，开奖后请开通新的一期.<br />上证指数一般在15:30就不再变动，故每天自动开奖时间默认为16:00");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                string strText = "单,双,大,小,合单,合双,合大,合小,0,1,2,3,4,5,6,7,8,9,,";
                string strName = "Odds10,Odds11,Odds12,Odds13,Odds14,Odds15,Odds16,Odds17,Odds0,Odds1,Odds2,Odds3,Odds4,Odds5,Odds6,Odds7,Odds8,Odds9,ptype,backurl";
                string strType = "small,small,small,small,small,small,small,small,small,small,small,small,small,small,small,small,small,small,hidden,hidden";
                string strValu = "" + xml.dss["StkOdds10"] + "'" + xml.dss["StkOdds11"] + "'" + xml.dss["StkOdds12"] + "'" + xml.dss["StkOdds13"] + "'" + xml.dss["StkOdds14"] + "'" + xml.dss["StkOdds15"] + "'" + xml.dss["StkOdds16"] + "'" + xml.dss["StkOdds17"] + "'" + xml.dss["StkOdds0"] + "'" + xml.dss["StkOdds1"] + "'" + xml.dss["StkOdds2"] + "'" + xml.dss["StkOdds3"] + "'" + xml.dss["StkOdds4"] + "'" + xml.dss["StkOdds5"] + "'" + xml.dss["StkOdds6"] + "'" + xml.dss["StkOdds7"] + "'" + xml.dss["StkOdds8"] + "'" + xml.dss["StkOdds9"] + "'1'" + Utils.getPage(0) + "";
                string strEmpt = "false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,stkset.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("温馨提示:<br />赔率支持1-2位小数，如1.85");
                builder.Append(Out.Tab("</div>", ""));
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