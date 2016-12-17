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

public partial class Manage_xml_flowset : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "百花谷设置";
        builder.Append(Out.Tab("", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-9]\d*$", "0"));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/flow.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {

            string Name = Utils.GetRequest("Name", "post", 2, @"^[^\^]{1,20}$", "系统名称限1-20字内");
            string Notes = Utils.GetRequest("Notes", "post", 3, @"^[^\^]{1,2000}$", "百花谷口号限2000字内");
            string Logo = Utils.GetRequest("Logo", "post", 3, @"^[^\^]{1,200}$", "Logo地址限200字内");
            string Status = Utils.GetRequest("Status", "post", 2, @"^[0-1]$", "系统状态选择出错");
            string Foot = Utils.GetRequest("Foot", "post", 3, @"^[\s\S]{1,2000}$", "底部Ubb限2000字内");
            string zPrice = Utils.GetRequest("zPrice", "post", 2, @"^[1-9]\d*$", "种子单价填写错误");
            string dPrice = Utils.GetRequest("dPrice", "post", 2, @"^[1-9]\d*$", "道具1单价填写错误");
            string dPrice2 = Utils.GetRequest("dPrice2", "post", 2, @"^[1-9]\d*$", "道具2单价填写错误");


            xml.dss["FlowName"] = Name;
            xml.dss["FlowNotes"] = Notes;
            xml.dss["FlowLogo"] = Logo;
            xml.dss["FlowStatus"] = Status;
            xml.dss["FlowFoot"] = Foot;
            xml.dss["FlowzPrice"] = zPrice;
            xml.dss["FlowdPrice"] = dPrice;
            xml.dss["FlowdPrice2"] = dPrice2;


            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("百花谷设置", "设置成功，正在返回..", Utils.getUrl("Flowset.aspx?ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "百花谷设置"));
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));

            builder.Append("百花谷设置");


            builder.Append("|<a href=\"" + Utils.getUrl("spkadminset.aspx?ptype=25&amp;backurl=" + Utils.PostPage(true) + "") + "\">管理员</a>");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "游戏名称:/,游戏口号(可留空):/,游戏Logo(可留空):/,游戏状态:/,底部Ubb:/,种子单价:/,道具1单价:/,道具2单价:/,";
            string strName = "Name,Notes,Logo,Status,Foot,zPrice,dPrice,dPrice2,backurl";
            string strType = "text,text,text,select,text,num,num,num,hidden";
            string strValu = "" + xml.dss["FlowName"] + "'" + xml.dss["FlowNotes"] + "'" + xml.dss["FlowLogo"] + "'" + xml.dss["FlowStatus"] + "'" + xml.dss["FlowFoot"] + "'" + xml.dss["FlowzPrice"] + "'" + xml.dss["FlowdPrice"] + "'" + xml.dss["FlowdPrice2"] + "'" + Utils.getPage(0) + "";
            string strEmpt = "false,true,true,0|正常|1|维护,true,false,false,false,false";


            string strIdea = "/";
            string strOthe = "确定修改|reset,flowset.aspx,post,1,red|blue";
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