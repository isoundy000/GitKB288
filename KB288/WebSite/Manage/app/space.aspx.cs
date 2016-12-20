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
using BCW.Files;

public partial class Manage_app_space : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "空间占用";

        //计算全站占用
        long AllFtp = FileTool.GetPathLength(Server.MapPath("/"));

        //计算文件存储占用
        long FilesFtp = FileTool.GetPathLength(Server.MapPath("/Files"));

        //数据库备份占用
        long DataFtp = FileTool.GetPathLength(Server.MapPath("/App_Data"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("空间占用管理");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("空间总占用:" + FileTool.GetContentLength(AllFtp) + "");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("", "<br />----------<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("文件存储:");
        builder.Append(Out.Tab("<img src=\"/Files/sys/bar.gif\" height=\"10\" width=\"" + Utils.Percent(FilesFtp, AllFtp).Replace("%", "") + "\" />", ""));
        builder.Append(Utils.Percent(FilesFtp, AllFtp) + "<br />占用" + FileTool.GetContentLength(FilesFtp) + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("备份数据:");
        builder.Append(Out.Tab("<img src=\"/Files/sys/bar.gif\" height=\"10\" width=\"" + Utils.Percent(DataFtp, AllFtp).Replace("%", "") + "\" />", ""));
        builder.Append(Utils.Percent(DataFtp, AllFtp) + "<br />占用" + FileTool.GetContentLength(DataFtp) + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("程序文件:");
        builder.Append(Out.Tab("<img src=\"/Files/sys/bar.gif\" height=\"10\" width=\"" + Utils.Percent((AllFtp - FilesFtp - DataFtp), AllFtp).Replace("%", "") + "\" />", ""));
        builder.Append(Utils.Percent((AllFtp - FilesFtp - DataFtp), AllFtp) + "<br />占用" + FileTool.GetContentLength(AllFtp - FilesFtp - DataFtp) + "");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">系统服务中心</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
}