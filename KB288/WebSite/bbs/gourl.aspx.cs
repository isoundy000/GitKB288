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

public partial class bbs_gourl : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string tt = Utils.GetRequest("tt", "get", 1, @"http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", "错误的地址");
        new Out().head(Utils.ForWordType("温馨提示"));
        Response.Write(Out.Tab("<div class=\"text\">", ""));
        Response.Write("您即将离开" + ub.Get("SiteName") + "...");
        Response.Write(Out.Tab("</div>", "<br />"));
        Response.Write(Out.Tab("<div class=\"title\">", ""));
        Response.Write("<a href=\"" + tt + "\">马上进入</a>");
        Response.Write(Out.Tab("</div>", "<br />"));
        Response.Write(Out.Tab("<div>", ""));
        Response.Write(Out.back("取消返回"));
        Response.Write(Out.Tab("</div>", ""));
        Response.Write(new Out().foot());
        Response.End();
    }
}
