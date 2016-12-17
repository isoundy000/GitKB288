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

public partial class Manage_xml_Gsset : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "高手系统设置";
        builder.Append(Out.Tab("", ""));

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/bbs.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string GsDemoID = Utils.GetRequest("GsDemoID", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "高手内测ID请用#分隔，可以留空");
            string GsAdminID = Utils.GetRequest("GsAdminID", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "高手前台管理ID请用#分隔，可以留空");
            string GsAdminID2 = Utils.GetRequest("GsAdminID2", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "高手前台设置期数ID请用#分隔，可以留空");

            xml.dss["BbsGsDemoID"] = GsDemoID;
            xml.dss["BbsGsAdminID"] = GsAdminID;
            xml.dss["BbsGsAdminID2"] = GsAdminID2;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("设置", "设置成功，正在返回..", Utils.getUrl("Gsset.aspx?backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "高手系统设置"));

            string strText = "高手内测ID(多个请用#分隔，留空则开放所有人使用):/,高手前台开奖管理ID:/,高手前台设置期数ID:/,";
            string strName = "GsDemoID,GsAdminID,GsAdminID2,backurl";
            string strType = "big,big,big,hidden";
            string strValu = "" + xml.dss["BbsGsDemoID"] + "'" + xml.dss["BbsGsAdminID"] + "'" + xml.dss["BbsGsAdminID2"] + "'" + Utils.getPage(0) + "";
            string strEmpt = "true,true,true,false";
            string strIdea = "/";
            string strOthe = "确定修改|reset,Gsset.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">返回上级</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
}
