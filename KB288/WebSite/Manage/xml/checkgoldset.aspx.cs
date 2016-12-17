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

public partial class Manage_xml_checkgoldset : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "前台查币/竞猜管理设置";
        builder.Append(Out.Tab("", ""));

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/bbs.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string CheckID = Utils.GetRequest("CheckID", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "多个查币ID请用#分隔，可以留空");
            string GuessAdminID = Utils.GetRequest("GuessAdminID", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "多个竞猜管理ID请用#分隔，可以留空");
            xml.dss["BbsCheckID"] = CheckID;
            xml.dss["BbsGuessAdminID"] = GuessAdminID;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("设置", "设置成功，正在返回..", Utils.getUrl("checkgoldset.aspx"), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "查币/竞猜管理设置"));

            string strText = "查币ID(多个请用#分隔):/,竞猜管理ID:/";
            string strName = "CheckID,GuessAdminID";
            string strType = "big,big";
            string strValu = "" + xml.dss["BbsCheckID"] + "'" + xml.dss["BbsGuessAdminID"] + "";
            string strEmpt = "true,true";
            string strIdea = "/";
            string strOthe = "确定修改|reset,checkgoldset.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
}
