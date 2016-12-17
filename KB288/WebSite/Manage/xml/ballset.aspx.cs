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

public partial class Manage_xml_ballset : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "疯狂彩球游戏设置";
        builder.Append(Out.Tab("", ""));

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/ball.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string Name = Utils.GetRequest("Name", "post", 2, @"^[^\^]{1,20}$", "系统名称限1-20字内");
            string Notes = Utils.GetRequest("Notes", "post", 3, @"^[^\^]{1,50}$", "Ball口号限50字内");
            string Logo = Utils.GetRequest("Logo", "post", 3, @"^[^\^]{1,200}$", "Logo地址限200字内");
            string Status = Utils.GetRequest("Status", "post", 2, @"^[0-1]$", "系统状态选择出错");
            string OpenType = Utils.GetRequest("OpenType", "post", 2, @"^[0-1]$", "开奖类型选择出错");
            string Tax = Utils.GetRequest("Tax", "post", 2, @"^[0-9]\d*$", "税率填写错误");
            string SysNum = Utils.GetRequest("SysNum", "post", 2, @"^[1-9]$|^[1-4]([0-9])?$", "彩球数字最大填写错误,限1-49");
            string Odds = Utils.GetRequest("Odds", "post", 2, @"^[1-9]\d*$", "赔率填写错误");
            string OutNum = Utils.GetRequest("OutNum", "post", 2, @"^[1-9]\d*$", "每期限购多少份填写错误");
            string OutIDNum = Utils.GetRequest("OutIDNum", "post", 2, @"^[1-9]\d*$", "每期每ID限购多少份填写错误");
            string iCent = Utils.GetRequest("iCent", "post", 2, @"^[1-9]\d*$", "每份价格填写错误");
            string SysPay = Utils.GetRequest("SysPay", "post", 2, @"^[0-9]\d*$", "系统每期投入多少币填写错误");
            string CycleMin = Utils.GetRequest("CycleMin", "post", 2, @"^[0-9]\d*$", "游戏周期填写错误");
            string Expir = Utils.GetRequest("Expir", "post", 2, @"^[0-9]\d*$", "游戏防刷填写错误");
            string OnTime = Utils.GetRequest("OnTime", "post", 3, @"^[0-9]{2}:[0-9]{2}-[0-9]{2}:[0-9]{2}$", "游戏开放时间填写错误，可留空");
            string Foot = Utils.GetRequest("Foot", "post", 3, @"^[\s\S]{1,2000}$", "底部Ubb限2000字内");
            string Rule = Utils.GetRequest("Rule", "post", 3, @"^[\s\S]{1,5000}$", "规则最限5000字内");
            //继续验证时间
            if (OnTime != "")
            {
                string[] temp = OnTime.Split("-".ToCharArray());
                DateTime dt1 = Utils.ParseTime(temp[0]);
                DateTime dt2 = Utils.ParseTime(temp[1]);
            }

            xml.dss["BallName"] = Name;
            xml.dss["BallNotes"] = Notes;
            xml.dss["BallLogo"] = Logo;
            xml.dss["BallStatus"] = Status;
            xml.dss["BallOpenType"] = OpenType;
            xml.dss["BallTax"] = Tax;
            xml.dss["BallSysNum"] = SysNum;
            xml.dss["BallOutNum"] = OutNum;
            xml.dss["BallOdds"] = Odds;
            xml.dss["BalliCent"] = iCent;
            xml.dss["BallOutIDNum"] = OutIDNum;
            xml.dss["BallSysPay"] = SysPay;
            xml.dss["BallCycleMin"] = CycleMin;
            xml.dss["BallExpir"] = Expir;
            xml.dss["BallOnTime"] = OnTime;
            xml.dss["BallFoot"] = Foot;
            xml.dss["BallRule"] = Rule;

            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("疯狂彩球游戏设置", "设置成功，正在返回..", Utils.getUrl("ballset.aspx?backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "疯狂彩球游戏设置"));
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("彩球设置|<a href=\"" + Utils.getUrl("spkadminset.aspx?ptype=8&amp;backurl=" + Utils.PostPage(true) + "") + "\">管理员</a>");
            builder.Append(Out.Tab("</div>", ""));
            string strText = "游戏名称:/,游戏口号(可留空):/,游戏Logo(可留空):/,游戏状态:/,开奖类型:/,系统收税(%):/,彩球数字最大:/,赔率(倍):/,每期限购多少份:/,每份价格:/,每期每ID限购多少份:/,系统每期投入多少币:/,游戏周期(分钟):/,游戏下注防刷(秒):/,游戏开放时间(可留空):/,底部Ubb:/,游戏规则:(支持UBB)/,";
            string strName = "Name,Notes,Logo,Status,OpenType,Tax,SysNum,Odds,OutNum,iCent,OutIDNum,SysPay,CycleMin,Expir,OnTime,Foot,Rule,backurl";
            string strType = "text,text,text,select,select,num,num,num,num,num,num,num,num,num,text,text,textarea,hidden";
            string strValu = "" + xml.dss["BallName"] + "'" + xml.dss["BallNotes"] + "'" + xml.dss["BallLogo"] + "'" + xml.dss["BallStatus"] + "'" + xml.dss["BallOpenType"] + "'" + xml.dss["BallTax"] + "'" + xml.dss["BallSysNum"] + "'" + xml.dss["BallOdds"] + "'" + xml.dss["BallOutNum"] + "'" + xml.dss["BalliCent"] + "'" + xml.dss["BallOutIDNum"] + "'" + xml.dss["BallSysPay"] + "'" + xml.dss["BallCycleMin"] + "'" + xml.dss["BallExpir"] + "'" + xml.dss["BallOnTime"] + "'" + xml.dss["BallFoot"] + "'" + xml.dss["BallRule"] + "'" + Utils.getPage(0) + "";
            string strEmpt = "false,true,true,0|正常|1|维护,0|随机开奖|1|不输开奖|2|手工开奖,false,false,false,false,false,false,false,false,false,true,true,true,false";
            string strIdea = "/";
            string strOthe = "确定修改|reset,ballset.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示:<br />随机开奖指正常的随机开出数字，不输开奖指自动开出最少购买的数字赢.<br />当选择手工开奖时，请记得在游戏管理中开奖.<br />彩球数字最大为49，如填写20，则开奖数字段为1-20<br />当修改赔率、每期限购份数与每份价格时，将在下期生效.<br />游戏开放时间填写格式为:09:00-18:00，留空则全天开放.");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("game.aspx") + "\">返回上级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
}
