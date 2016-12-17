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

public partial class Man_default : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "快捷管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("快捷管理");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("config2set.aspx") + "\">顶部和底部设置</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("fresh2set.aspx") + "\">滚动设置</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("spkadmin2set.aspx") + "\">默认闲聊管理</a>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("../default.aspx") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
}
