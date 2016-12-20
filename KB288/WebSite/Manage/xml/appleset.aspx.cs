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

public partial class Manage_xml_appleset : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "苹果机游戏设置";
        builder.Append(Out.Tab("", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-1]$", "0"));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/apple.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            if (ptype == 0)
            {
                string Name = Utils.GetRequest("Name", "post", 2, @"^[^\^]{1,20}$", "系统名称限1-20字内");
                string Notes = Utils.GetRequest("Notes", "post", 3, @"^[^\^]{1,50}$", "Apple口号限50字内");
                string Logo = Utils.GetRequest("Logo", "post", 3, @"^[^\^]{1,200}$", "Logo地址限200字内");
                string Status = Utils.GetRequest("Status", "post", 2, @"^[0-1]$", "系统状态选择出错");
                string OpenType = Utils.GetRequest("OpenType", "post", 2, @"^[0-1]$", "开奖类型选择出错");
                string Tax = Utils.GetRequest("Tax", "post", 2, @"^[0-9]\d*$", "税率填写错误");
                string OutIDNum = Utils.GetRequest("OutIDNum", "post", 2, @"^[1-9]\d*$", "每期每ID限购多少注填写错误");
                string Price = Utils.GetRequest("Price", "post", 2, @"^[1-9]\d*$", "每注价格填写错误");
                string Minutes = Utils.GetRequest("Minutes", "post", 2, @"^[0-9]\d*$", "游戏周期填写错误");
                string OnTime = Utils.GetRequest("OnTime", "post", 3, @"^[0-9]{2}:[0-9]{2}-[0-9]{2}:[0-9]{2}$", "游戏开放时间填写错误，可留空");
                string Foot = Utils.GetRequest("Foot", "post", 3, @"^[\s\S]{1,2000}$", "底部Ubb限2000字内");
                string GuestSet = Utils.GetRequest("GuestSet", "post", 2, @"^[0-1]$", "兑奖内线选择出错");
                //继续验证时间
                if (OnTime != "")
                {
                    string[] temp = OnTime.Split("-".ToCharArray());
                    DateTime dt1 = Utils.ParseTime(temp[0]);
                    DateTime dt2 = Utils.ParseTime(temp[1]);
                }
                xml.dss["AppleName"] = Name;
                xml.dss["AppleNotes"] = Notes;
                xml.dss["AppleLogo"] = Logo;
                xml.dss["AppleStatus"] = Status;
                xml.dss["AppleOpenType"] = OpenType;
                xml.dss["AppleTax"] = Tax;
                xml.dss["ApplePrice"] = Price;
                xml.dss["AppleOutIDNum"] = OutIDNum;
                xml.dss["AppleMinutes"] = Minutes;
                xml.dss["AppleOnTime"] = OnTime;
                xml.dss["AppleFoot"] = Foot;
            }
            else {
                string Odds1 = Utils.GetRequest("Odds1", "post", 2, @"^[0-9]\d*$", "苹果赔率错误");
                string Odds2 = Utils.GetRequest("Odds2", "post", 2, @"^[0-9]\d*$", "木瓜赔率错误");
                string Odds3 = Utils.GetRequest("Odds3", "post", 2, @"^[0-9]\d*$", "西瓜赔率错误");
                string Odds4 = Utils.GetRequest("Odds4", "post", 2, @"^[0-9]\d*$", "芒果赔率错误");
                string Odds5 = Utils.GetRequest("Odds5", "post", 2, @"^[0-9]\d*$", "双星赔率错误");
                string Odds6 = Utils.GetRequest("Odds6", "post", 2, @"^[0-9]\d*$", "金钟赔率错误");
                string Odds7 = Utils.GetRequest("Odds7", "post", 2, @"^[0-9]\d*$", "双七赔率错误");
                string Odds8 = Utils.GetRequest("Odds8", "post", 2, @"^[0-9]\d*$", "元宝赔率错误");
                string OddsSmall = Utils.GetRequest("OddsSmall", "post", 2, @"^[0-9]\d*$", "开小赔率错误");
                xml.dss["AppleOdds1"] = Odds1;
                xml.dss["AppleOdds2"] = Odds2;
                xml.dss["AppleOdds3"] = Odds3;
                xml.dss["AppleOdds4"] = Odds4;
                xml.dss["AppleOdds5"] = Odds5;
                xml.dss["AppleOdds6"] = Odds6;
                xml.dss["AppleOdds7"] = Odds7;
                xml.dss["AppleOdds8"] = Odds8;
                xml.dss["AppleOddsSmall"] = OddsSmall;
            }

            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("苹果机游戏设置", "设置成功，正在返回..", Utils.getUrl("appleset.aspx?ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "苹果机游戏设置"));
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            if (ptype == 0)
            {
                builder.Append("苹果机设置|");
                builder.Append("<a href=\"" + Utils.getUrl("appleset.aspx?ptype=1") + "\">赔率</a>");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("appleset.aspx?ptype=0") + "\">苹果机设置</a>");
                builder.Append("|赔率");
            }
            builder.Append("|<a href=\"" + Utils.getUrl("spkadminset.aspx?ptype=17&amp;backurl=" + Utils.PostPage(true) + "") + "\">管理员</a>");
            builder.Append(Out.Tab("</div>", ""));
            if (ptype == 0)
            {
                string strText = "游戏名称:/,游戏口号(可留空):/,游戏Logo(可留空):/,游戏状态:/,开奖类型:/,系统收税(%):/,每注价格:/,每期每ID限购多少注:/,游戏周期(分钟):/,游戏开放时间(可留空):/,底部Ubb:/,兑奖内线:,";
                string strName = "Name,Notes,Logo,Status,OpenType,Tax,Price,OutIDNum,Minutes,OnTime,Foot,GuestSet,backurl";
                string strType = "text,text,text,select,select,num,num,num,num,text,textarea,select,hidden";
                string strValu = "" + xml.dss["AppleName"] + "'" + xml.dss["AppleNotes"] + "'" + xml.dss["AppleLogo"] + "'" + xml.dss["AppleStatus"] + "'" + xml.dss["AppleOpenType"] + "'" + xml.dss["AppleTax"] + "'" + xml.dss["ApplePrice"] + "'" + xml.dss["AppleOutIDNum"] + "'" + xml.dss["AppleMinutes"] + "'" + xml.dss["AppleOnTime"] + "'" + xml.dss["AppleFoot"] + "'" + xml.dss["AppleRule"] + "'" + xml.dss["AppleGuestSet"] + "'" + Utils.getPage(0) + "";
                string strEmpt = "false,true,true,0|正常|1|维护,0|随机开奖|1|不输开奖,false,false,false,false,true,true,0|开启|1|关闭,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,appleset.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("温馨提示:<br />随机开奖指正常的随机开出，不输开奖指自动开出最低赔付的类型赢.<br />游戏开放时间填写格式为:09:00-18:00，留空则全天开放.");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                string strText = "苹果:,木瓜:,西瓜:,芒果:,双星:,金钟:,双七:,元宝:,开小:,,";
                string strName = "Odds1,Odds2,Odds3,Odds4,Odds5,Odds6,Odds7,Odds8,OddsSmall,ptype,backurl";
                string strType = "num,num,num,num,num,num,num,num,num,hidden,hidden";
                string strValu = "" + xml.dss["AppleOdds1"] + "'" + xml.dss["AppleOdds2"] + "'" + xml.dss["AppleOdds3"] + "'" + xml.dss["AppleOdds4"] + "'" + xml.dss["AppleOdds5"] + "'" + xml.dss["AppleOdds6"] + "'" + xml.dss["AppleOdds7"] + "'" + xml.dss["AppleOdds8"] + "'" + xml.dss["AppleOddsSmall"] + "'1'" + Utils.getPage(0) + "";
                string strEmpt = "false,false,false,false,false,false,false,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,appleset.aspx,post,1,red|blue";
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
