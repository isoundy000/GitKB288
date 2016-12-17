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

public partial class Manage_app_ualock : System.Web.UI.Page
{

    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "设置拒绝UA";
        builder.Append(Out.Tab("", ""));

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        Application.Remove("xml_wap");//清缓存
        xml.Reload(); //加载网站配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string NoUA = Utils.GetRequest("NoUA", "post", 3, @"^[^\#]{1,500}(?:\#[^\#]{1,500}){0,10000}$", "UA填写错误");
            string NoUA2 = Utils.GetRequest("NoUA2", "post", 3, @"^[^\#]{1,500}(?:\#[^\#]{1,500}){0,10000}$", "UA2填写错误");
            string NoUAMsg = Utils.GetRequest("NoUAMsg", "post", 2, @"^[^\^]{1,500}$", "提示信息限1-500字内");
            string NoUAStat = Utils.GetRequest("NoUAStat", "post", 2, @"^[0-1]$", "状态选择错误");

            xml.ds["SiteNoUA"] = NoUA;
            xml.ds["SiteNoUA2"] = NoUA2;
            xml.ds["SiteNoUAMsg"] = NoUAMsg;
            xml.ds["SiteNoUAStat"] = NoUAStat;
            System.IO.File.WriteAllText(Server.MapPath("~/Controls/wap.xml"), xml.Post(xml.ds), System.Text.Encoding.UTF8);
            Utils.Success("设置拒绝UA", "设置拒绝UA成功，正在返回..", Utils.getUrl("ualock.aspx"), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "设置拒绝UA"));

            string strText = "存在UA(请用“#”作分隔):/,等于UA(请用“#”作分隔):/,提示信息:/,状态:/";
            string strName = "NoUA,NoUA2,NoUAMsg,NoUAStat";
            string strType = "textarea,textarea,text,select";
            string strValu = "" + xml.ds["SiteNoUA"] + "'" + xml.ds["SiteNoUA2"] + "'" + xml.ds["SiteNoUAMsg"] + "'" + xml.ds["SiteNoUAStat"] + "";
            string strEmpt = "true,true,true,0|启用|1|关闭";
            string strIdea = "/";
            string strOthe = "确定修改|reset,ualock.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("设置的UA在启用状态下将不允许访问网站，多个UA请用“#”作分隔。<br />您的短UA:" + Utils.GetBrowser() + "<br />您的长UA:" + Utils.GetUA() + "<br />如果设置其中一个UA你将不能访问前台。");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">系统服务中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
}
