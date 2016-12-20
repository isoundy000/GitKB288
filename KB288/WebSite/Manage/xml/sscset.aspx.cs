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

public partial class Manage_xml_sscset : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "时时彩设置";
        builder.Append(Out.Tab("", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-1]$", "0"));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/ssc.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            if (ptype == 0)
            {
                string Name = Utils.GetRequest("Name", "post", 2, @"^[^\^]{1,20}$", "系统名称限1-20字内");
                string Notes = Utils.GetRequest("Notes", "post", 3, @"^[^\^]{1,5000}$", "口号限5000字内");
                string Logo = Utils.GetRequest("Logo", "post", 3, @"^[^\^]{1,200}$", "Logo地址限200字内");
                string Status = Utils.GetRequest("Status", "post", 2, @"^[0-1]$", "系统状态选择出错");
                string Sec = Utils.GetRequest("Sec", "post", 2, @"^[0-9]\d*$", "秒数填写出错");
                string SmallPay = Utils.GetRequest("SmallPay", "post", 2, @"^[0-9]\d*$", "最小下注" + ub.Get("SiteBz") + "填写错误");
                string BigPay = Utils.GetRequest("BigPay", "post", 2, @"^[0-9]\d*$", "最大下注" + ub.Get("SiteBz") + "填写错误");
                string Price = Utils.GetRequest("Price", "post", 2, @"^[0-9]\d*$", "每期每ID限购多少" + ub.Get("SiteBz") + "填写错误");
                string Expir = Utils.GetRequest("Expir", "post", 2, @"^[0-9]\d*$", "防刷秒数填写出错");
                string OnTime = Utils.GetRequest("OnTime", "post", 3, @"^[0-9]{2}:[0-9]{2}-[0-9]{2}:[0-9]{2}$", "游戏开放时间填写错误，可留空");
                string Foot = Utils.GetRequest("Foot", "post", 3, @"^[\s\S]{1,2000}$", "底部Ubb限2000字内");
                string GuestSet = Utils.GetRequest("GuestSet", "post", 2, @"^[0-1]$", "兑奖内线选择出错");
                string IsBot = Utils.GetRequest("IsBot", "post", 2, @"^[0-1]$", "机器人选择出错");

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
                xml.dss["SSCIsBot"] = IsBot;
            }
            else
            {

                for (int i = 1; i <= 15; i++)
                {
                    string Odds = "";
                    if (i == 15)
                        Odds = Utils.GetRequest("Odds" + i + "", "post", 2, @"^(\d)*(\.(\d){1,2})?$", "赔率错误");
                    else
                        Odds = Utils.GetRequest("Odds" + i + "", "post", 2, @"^[0-9]\d*$", "赔率错误");

                    xml.dss["SSCOdds" + i + ""] = Odds;
                }

            }

            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("时时彩设置", "设置成功，正在返回..", Utils.getUrl("sscset.aspx?ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "时时彩设置"));
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            if (ptype == 0)
            {
                builder.Append("时时彩设置|");
                builder.Append("<a href=\"" + Utils.getUrl("sscset.aspx?ptype=1") + "\">赔率</a>");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("sscset.aspx?ptype=0") + "\">时时彩设置</a>");
                builder.Append("|赔率");
            }
            builder.Append("|<a href=\"" + Utils.getUrl("spkadminset.aspx?ptype=22&amp;backurl=" + Utils.PostPage(true) + "") + "\">管理员</a>");
            builder.Append(Out.Tab("</div>", ""));
            if (ptype == 0)
            {
                string strText = "游戏名称:/,游戏口号(可留空):/,游戏Logo(可留空):/,游戏状态:/,离截止时间N秒前不能下注/,最小下注" + ub.Get("SiteBz") + ":/,最大下注" + ub.Get("SiteBz") + ":/,每期每ID限购多少" + ub.Get("SiteBz") + "(填0则不限制):/,下注防刷(秒):/,游戏开放时间(可留空):/,底部Ubb:/,兑奖内线:/,机器人:/,";
                string strName = "Name,Notes,Logo,Status,Sec,SmallPay,BigPay,Price,Expir,OnTime,Foot,GuestSet,IsBot,backurl";
                string strType = "text,text,text,select,num,num,num,num,num,text,textarea,select,select,hidden";
                string strValu = "" + xml.dss["SSCName"] + "'" + xml.dss["SSCNotes"] + "'" + xml.dss["SSCLogo"] + "'" + xml.dss["SSCStatus"] + "'" + xml.dss["SSCSec"] + "'" + xml.dss["SSCSmallPay"] + "'" + xml.dss["SSCBigPay"] + "'" + xml.dss["SSCPrice"] + "'" + xml.dss["SSCExpir"] + "'" + xml.dss["SSCOnTime"] + "'" + xml.dss["SSCFoot"] + "'" + xml.dss["SSCGuestSet"] + "'" + xml.dss["SSCIsBot"] + "'" + Utils.getPage(0) + "";
                string strEmpt = "false,true,true,0|正常|1|维护,false,false,false,false,false,true,true,0|开启|1|关闭,0|关闭|1|开启,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,sscset.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("温馨提示:<br />游戏开放时间填写格式为:09:00-18:00，留空则全天开放.");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                string strText = "五星直选:,五星通选:,五星通选前三后三对应:,五星通选前二后二对应:,四星直选:,四星三位按位相符:,三星直选:,三星组三:,三星组六:,二星直选:,二星组选:,一星直选:,任选一:,任选二:,大小单双:,,";
                string strName = "Odds1,Odds2,Odds3,Odds4,Odds5,Odds6,Odds7,Odds8,Odds9,Odds10,Odds11,Odds12,Odds13,Odds14,Odds15,ptype,backurl";
                string strType = "num,num,num,num,num,num,num,num,num,num,num,num,num,num,text,hidden,hidden";
                string strValu = "" + xml.dss["SSCOdds1"] + "'" + xml.dss["SSCOdds2"] + "'" + xml.dss["SSCOdds3"] + "'" + xml.dss["SSCOdds4"] + "'" + xml.dss["SSCOdds5"] + "'" + xml.dss["SSCOdds6"] + "'" + xml.dss["SSCOdds7"] + "'" + xml.dss["SSCOdds8"] + "'" + xml.dss["SSCOdds9"] + "'" + xml.dss["SSCOdds10"] + "'" + xml.dss["SSCOdds11"] + "'" + xml.dss["SSCOdds12"] + "'" + xml.dss["SSCOdds13"] + "'" + xml.dss["SSCOdds14"] + "'" + xml.dss["SSCOdds15"] + "'1'" + Utils.getPage(0) + "";
                string strEmpt = "false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,sscset.aspx,post,1,red|blue";
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
