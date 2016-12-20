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

public partial class Manage_xml_networkset : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "广播系统设置";
        builder.Append(Out.Tab("", ""));

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/network.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string Name = Utils.GetRequest("Name", "post", 2, @"^[^\^]{1,20}$", "系统名称限1-20字内");
            string Notes = Utils.GetRequest("Notes", "post", 3, @"^[^\^]{1,50}$", "广播口号限50字内");
            string Logo = Utils.GetRequest("Logo", "post", 3, @"^[^\^]{1,200}$", "Logo地址限200字内");
            string Status = Utils.GetRequest("Status", "post", 2, @"^[0-1]$", "系统状态选择出错");
            string sLength = Utils.GetRequest("sLength", "post", 2, @"^[0-9]\d*$", "内容最小字数填写错误");
            string bLength = Utils.GetRequest("bLength", "post", 2, @"^[0-9]\d*$", "内容最大字数填写错误");
            string bMinute = Utils.GetRequest("bMinute", "post", 2, @"^[0-9]\d*$", "显示时长最大时间填写错误");
            string iGold = Utils.GetRequest("iGold", "post", 2, @"^[0-9]\d*$", "每分钟扣币填写错误");
            string Expir = Utils.GetRequest("Expir", "post", 2, @"^[0-9]\d*$", "防刷时间填写错误");
            string Grade = Utils.GetRequest("Grade", "post", 2, @"^[0-9]\d*$", "限等级数填写错误");
            string RegDay = Utils.GetRequest("RegDay", "post", 2, @"^[0-9]\d*$", "限注册天数填写错误");

            xml.dss["NetworkName"] = Name;
            xml.dss["NetworkNotes"] = Notes;
            xml.dss["NetworkLogo"] = Logo;
            xml.dss["NetworkStatus"] = Status;
            xml.dss["NetworksLength"] = sLength;
            xml.dss["NetworkbLength"] = bLength;
            xml.dss["NetworkbMinute"] = bMinute;
            xml.dss["NetworkiGold"] = iGold;
            xml.dss["NetworkExpir"] = Expir;
            xml.dss["NetworkGrade"] = Grade;
            xml.dss["NetworkRegDay"] = RegDay;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("广播系统设置", "设置成功，正在返回..", Utils.getUrl("networkset.aspx"), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "广播系统设置"));

            string strText = "广播名称:/,广播口号(可留空):/,广播Logo(可留空):/,系统状态:/,内容最小字数:/,内容最大字数(最大500字):/,显示时长最大时间(分钟):/,每分钟扣" + ub.Get("SiteBz") + ":/,防刷时间(秒):/,发布限等级(级):/,发布限注册天数(天):/";
            string strName = "Name,Notes,Logo,Status,sLength,bLength,bMinute,iGold,Expir,Grade,RegDay";
            string strType = "text,text,text,select,num,num,num,num,num,num,num";
            string strValu = "" + xml.dss["NetworkName"] + "'" + xml.dss["NetworkNotes"] + "'" + xml.dss["NetworkLogo"] + "'" + xml.dss["NetworkStatus"] + "'" + xml.dss["NetworksLength"] + "'" + xml.dss["NetworkbLength"] + "'" + xml.dss["NetworkbMinute"] + "'" + xml.dss["NetworkiGold"] + "'" + xml.dss["NetworkExpir"] + "'" + xml.dss["NetworkGrade"] + "'" + xml.dss["NetworkRegDay"] + "";
            string strEmpt = "false,true,true,0|正常|1|维护,false,false,false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定修改|reset,networkset.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
}
