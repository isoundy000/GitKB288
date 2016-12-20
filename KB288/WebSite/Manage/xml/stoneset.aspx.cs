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

public partial class Manage_xml_stoneset : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "剪刀游戏设置";
        builder.Append(Out.Tab("", ""));

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/stone.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string Name = Utils.GetRequest("Name", "post", 2, @"^[^\^]{1,20}$", "系统名称限1-20字内");
            string Notes = Utils.GetRequest("Notes", "post", 3, @"^[^\^]{1,50}$", "剪刀口号限50字内");
            string Logo = Utils.GetRequest("Logo", "post", 3, @"^[^\^]{1,200}$", "Logo地址限200字内");
            string Status = Utils.GetRequest("Status", "post", 2, @"^[0-1]$", "系统状态选择出错");
            string Tax = Utils.GetRequest("Tax", "post", 2, @"^[0-9]\d*$", "税率填写错误");
            string Rule = Utils.GetRequest("Rule", "post", 3, @"^[^\^]{1,5000}$", "规则最限5000字内");
            string CheatID = Utils.GetRequest("CheatID", "post", 3, @"^[^\#]{1,10}(?:\#[^\#]{1,10}){0,500}$", "作弊ID请用#分开");

            xml.dss["StoneName"] = Name;
            xml.dss["StoneNotes"] = Notes;
            xml.dss["StoneLogo"] = Logo;
            xml.dss["StoneStatus"] = Status;
            xml.dss["StoneTax"] = Tax;
            xml.dss["StoneRule"] = Rule;
            xml.dss["StoneCheatID"] = CheatID;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("剪刀游戏设置", "设置成功，正在返回..", Utils.getUrl("stoneset.aspx?backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "剪刀游戏设置"));
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("剪刀设置|<a href=\"" + Utils.getUrl("spkadminset.aspx?ptype=1&amp;backurl=" + Utils.PostPage(true) + "") + "\">管理员</a>");
            builder.Append(Out.Tab("</div>", ""));
            string sText = string.Empty;
            string sName = string.Empty;
            string sType = string.Empty;
            string sValu = string.Empty;
            string sEmpt = string.Empty;
            if (Utils.GetTopDomain().Equals("qyh.cc"))
            {
                sText = ",作弊ID(多个用#分开):/";
                sName = ",CheatID";
                sType = ",text";
                sValu = "'" + xml.dss["StoneCheatID"] + "";
                sEmpt = ",true";
            }
            string strText = "游戏名称:/,游戏口号(可留空):/,游戏Logo(可留空):/,游戏状态:/,系统收税(%):/,游戏规则(支持UBB):/" + sText + ",";
            string strName = "Name,Notes,Logo,Status,Tax,Rule" + sName + ",backurl";
            string strType = "text,text,text,select,num,textarea" + sType + ",hidden";
            string strValu = "" + xml.dss["StoneName"] + "'" + xml.dss["StoneNotes"] + "'" + xml.dss["StoneLogo"] + "'" + xml.dss["StoneStatus"] + "'" + xml.dss["StoneTax"] + "'" + xml.dss["StoneRule"] + "" + sValu + "'" + Utils.getPage(0) + "";
            string strEmpt = "false,true,true,0|正常|1|维护,false,true" + sEmpt + ",false";
            string strIdea = "/";
            string strOthe = "确定修改|reset,stoneset.aspx,post,1,red|blue";
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