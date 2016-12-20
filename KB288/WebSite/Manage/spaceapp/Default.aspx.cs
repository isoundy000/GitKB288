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

public partial class Manage_spaceapp_Default : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        string gestName = Convert.ToString(ub.GetSub("gestName", "/Controls/guestlist.xml"));
        Master.Title = "社区应用管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("社区应用列表");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("1、<a href=\"" + Utils.getUrl("sellnum.aspx") + "\">兑换靓号管理</a><br />");
        builder.Append("2、<a href=\"" + Utils.getUrl("changesim.aspx") + "\">兑换话费管理</a><br />");
        builder.Append("3、<a href=\"" + Utils.getUrl("changeqb.aspx") + "\">兑换Q币管理</a><br />");
        builder.Append("4、<a href=\"" + Utils.getUrl("changeqqvip.aspx") + "\">QQ服务开通管理</a><br />");
        builder.Append("5、<a href=\"" + Utils.getUrl("Gsadmin.aspx") + "\">高手系统管理</a><br />");
        builder.Append("6、<a href=\"" + Utils.getUrl("../guest.aspx?act=manage") + "\">" + "" + gestName + "管理" + " </a><br />");
        builder.Append("7、<a href=\"" + Utils.getUrl("../guest.aspx?act=msg") + "\">" + "短信管理" + " </a><br />");
        builder.Append(Out.Tab("</div>", " "));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br /> "));
    }
}
