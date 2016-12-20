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

public partial class Manage_app_Default : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "系统服务中心";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("系统服务中心");
        builder.Append(Out.Tab("</div>", ""));

        string[] sName = { "数据管理", "空间占用", "流量统计", "绿色浏览器", "在线FTP", "设置拒绝IP", "设置拒绝UA", "管理网络爬虫", "系统缓存管理", "执行sql语句", "历史消费日志", "验证手机号" };
        string[] sUrl = { "databackup", "space", "stat", "browser", "filemanager", "iplock", "ualock", "webbug", "cache", "datasql","goldlog", "rzmobile" };
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string[] pageValUrl = { };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //总记录数
        recordCount = sName.Length;

        int stratIndex = (pageIndex - 1) * pageSize;
        int endIndex = pageIndex * pageSize;
        int k = 0;
        for (int i = 0; i < sName.Length; i++)
        {
            if (k >= stratIndex && k < endIndex)
            {
                if ((k + 1) % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                    builder.Append(Out.Tab("<div>", "<br />"));

                builder.Append("" + (i + 1) + ".<a href=\"" + Utils.getUrl(sUrl[i].ToString() + ".aspx") + "\">" + sName[i].ToString() + "</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            if (k == endIndex)
                break;
            k++;
        }

        // 分页
        builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        if (Utils.GetDomain().Contains("kubao") || Utils.GetDomain().Contains("tuhao") || Utils.GetDomain().Contains("th") || Utils.GetDomain().Contains("kb288"))
        {
            builder.Append("<a href=\"" + Utils.getUrl("setline.aspx") + "\">更新会员在线</a><br />");
        }
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br /> "));
    }
}
