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

public partial class Manage_xml_raceset : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "竞拍系统设置";
        builder.Append(Out.Tab("", ""));

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/race.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string Name = Utils.GetRequest("Name", "post", 2, @"^[^\^]{1,20}$", "系统名称限1-20字内");
            string Notes = Utils.GetRequest("Notes", "post", 3, @"^[^\^]{1,50}$", "竞拍口号限50字内");
            string Logo = Utils.GetRequest("Logo", "post", 3, @"^[^\^]{1,200}$", "Logo地址限200字内");
            string Status = Utils.GetRequest("Status", "post", 2, @"^[0-1]$", "系统状态选择出错");
            string Tax = Utils.GetRequest("Tax", "post", 2, @"^[0-9]\d*$", "税率填写错误");
            string Startlow = Utils.GetRequest("Startlow", "post", 2, @"^[0-9]\d*$", "竞拍最低起拍价填写错误");
            string Starthigh = Utils.GetRequest("Starthigh", "post", 2, @"^[0-9]\d*$", "竞拍最高起拍价填写错误");
            string ZfPrice = Utils.GetRequest("ZfPrice", "post", 2, @"^[0-9]\d*$", "拍卖至少增幅填写错误");
            string Startlow2 = Utils.GetRequest("Startlow2", "post", 2, @"^[0-9]\d*$", "竞拍最低起拍价填写错误");
            string Starthigh2 = Utils.GetRequest("Starthigh2", "post", 2, @"^[0-9]\d*$", "竞拍最高起拍价填写错误");
            string ZfPrice2 = Utils.GetRequest("ZfPrice2", "post", 2, @"^[0-9]\d*$", "拍卖至少增幅填写错误");
            string DayNum = Utils.GetRequest("DayNum", "post", 2, @"^[0-9]\d*$", "竞拍每天每ID发布数量填写错误");
            string Rule = Utils.GetRequest("Rule", "post", 3, @"^[^\^]{1,5000}$", "规则最限5000字内");

            xml.dss["RaceName"] = Name;
            xml.dss["RaceNotes"] = Notes;
            xml.dss["RaceLogo"] = Logo;
            xml.dss["RaceStatus"] = Status;
            xml.dss["RaceTax"] = Tax;
            xml.dss["RaceStartlow"] = Startlow;
            xml.dss["RaceStarthigh"] = Starthigh;
            xml.dss["RaceZfPrice"] = ZfPrice;
            xml.dss["RaceStartlow2"] = Startlow2;
            xml.dss["RaceStarthigh2"] = Starthigh2;
            xml.dss["RaceZfPrice2"] = ZfPrice2;
            xml.dss["RaceDayNum"] = DayNum;
            xml.dss["RaceRule"] = Rule;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("竞拍系统设置", "设置成功，正在返回..", Utils.getUrl("raceset.aspx?backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "竞拍系统设置"));
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("竞拍设置|<a href=\"" + Utils.getUrl("spkadminset.aspx?ptype=4&amp;backurl=" + Utils.PostPage(true) + "") + "\">管理员</a>");
            builder.Append(Out.Tab("</div>", ""));
            string strText = "游戏名称:/,游戏口号(可留空):/,游戏Logo(可留空):/,游戏状态:/,系统收税(%):/,竞拍最低起拍" + ub.Get("SiteBz") + ":/,竞拍最高起拍" + ub.Get("SiteBz") + ":/,拍卖至少增幅" + ub.Get("SiteBz") + ":/,竞拍最低起拍" + ub.Get("SiteBz2") + ":/,竞拍最高起拍" + ub.Get("SiteBz2") + ":/,拍卖至少增幅" + ub.Get("SiteBz2") + ":/,竞拍每天每ID发布数量:/,游戏规则(支持UBB):/,";
            string strName = "Name,Notes,Logo,Status,Tax,Startlow,Starthigh,ZfPrice,Startlow2,Starthigh2,ZfPrice2,DayNum,Rule,backurl";
            string strType = "text,text,text,select,num,num,num,num,num,num,num,num,textarea,hidden";
            string strValu = "" + xml.dss["RaceName"] + "'" + xml.dss["RaceNotes"] + "'" + xml.dss["RaceLogo"] + "'" + xml.dss["RaceStatus"] + "'" + xml.dss["RaceTax"] + "'" + xml.dss["RaceStartlow"] + "'" + xml.dss["RaceStarthigh"] + "'" + xml.dss["RaceZfPrice"] + "'" + xml.dss["RaceStartlow2"] + "'" + xml.dss["RaceStarthigh2"] + "'" + xml.dss["RaceZfPrice2"] + "'" + xml.dss["RaceDayNum"] + "'" + xml.dss["RaceRule"] + "'" + Utils.getPage(0) + "";
            string strEmpt = "false,true,true,0|正常|1|维护,false,false,false,false,false,false,false,false,true,false";
            string strIdea = "/";
            string strOthe = "确定修改|reset,raceset.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("game.aspx") + "\">返回上级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
}
