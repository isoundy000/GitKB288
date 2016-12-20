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

public partial class Manage_app_webbug : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "网络爬虫设置";
        string robots = "/robots.txt";
        
        string act = Utils.GetRequest("act", "all", 1, "", "");
        if (act == "ok")
        {
            string fileContent = Utils.GetRequest("fileContent", "post", 1, "", "");
            string getEncode = Utils.GetRequest("getEncode", "post", 1, "", "UTF-8");

            string str = new BCW.Files.FileManagerProcessor().SaveTextFile(Server.MapPath(robots), fileContent, getEncode);
            Utils.Success("爬虫设置", "" + str + "，正在返回..", Utils.getUrl("webbug.aspx"), "1");    
        }
        else
        {
            if (act == "help")
            {
                builder.Append(Out.Div("title", "网络爬虫设置示例"));
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("一、拒绝网络爬虫对整个网站的抓取<br />User-agent: * <br />Disallow: /<br />二、 允许网络爬虫对整个网站的抓取<br />User-agent: * <br />Disallow:<br />三、 拒绝网络爬虫对网站指定的目录抓取<br />User-agent: * <br />Disallow: /bbs/ <br />Disallow: /manage/ <br />四、 允许某些搜索引擎的网络爬虫对网站抓取<br />User-agent: Baiduspider<br />Disallow:<br />User-agent: Googlebot<br />Disallow: <br />User-agent: MSNBot<br />Allow: /<br />五、 限制网络爬虫对网站中页面的抓取<br />User-agent: * <br />Disallow: /bbs/fourm.aspx<br />Disallow: /mange/default.aspx");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                builder.Append(Out.Div("title", "网络爬虫设置"));
                string fileContent;
                string getEncode;
                new BCW.Files.FileManagerProcessor().ReadTextFile(Server.MapPath(robots), out fileContent, out getEncode);

                string strText = "robots.txt内容:/,,";
                string strName = "fileContent,getEncode,act";
                string strType = "textarea,hidden,hidden";
                string strValu = "" + fileContent + "'" + getEncode + "'ok";
                string strEmpt = "true,false,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,webbug.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("由于搜索引擎的泛滥,网站避免不了被几千几万个爬虫每天爬来爬去<br />并且可能并发几十上百个请求循环重复抓取，对网站往往是毁灭性打击<br />设置robots.txt可以引导搜索引擎对网站的抓取<br />但如果恶性搜索引擎不遵循robots协议恶意抓取，你只能杀掉它的UA。");
                builder.Append(Out.Tab("</div>", ""));
            }
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            if (act == "help")
                builder.Append("<a href=\"" + Utils.getUrl("webbug.aspx") + "\">网络爬虫</a><br />");
            else
                builder.Append("<a href=\"" + Utils.getUrl("webbug.aspx?act=help") + "\">设置方法</a><br />");

            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">系统服务中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));

        }
    }
}
