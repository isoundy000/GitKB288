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

public partial class Manage_xml_centrole : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {

        Master.Title = "" + ub.Get("SiteBz") + "转帐ID设置";
        builder.Append(Out.Tab("", ""));
        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId != 1 && ManageId != 11 )
        {
            Utils.Error("只有系统管理员1号才可以进行此操作", "");
        }
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/finance.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string SysID = Utils.GetRequest("SysID", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "内部互转ID请用#分隔，可以留空");
            string SysID2 = Utils.GetRequest("SysID2", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "内部禁转ID请用#分隔，可以留空");
            xml.dss["FinanceSysID"] = SysID;
            xml.dss["FinanceSysID2"] = SysID2;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("设置", "设置成功，正在返回..", Utils.getUrl("centrole.aspx"), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "" + ub.Get("SiteBz") + "转帐ID设置"));

            string strText = "内部互转ID(多个用#分隔|对外限20000一天):/,内部禁转ID(多个用#分隔):/";
            string strName = "SysID,SysID2";
            string strType = "big,big";
            string strValu = "" + xml.dss["FinanceSysID"] + "'" + xml.dss["FinanceSysID2"] + "";
            string strEmpt = "true,true";
            string strIdea = "/";
            string strOthe = "确定修改|reset,centrole.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
}
