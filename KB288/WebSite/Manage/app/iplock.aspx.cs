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

public partial class Manage_app_iplock : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "设置拒绝IP";
        builder.Append(Out.Tab("", ""));

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        Application.Remove("xml_wap");//清缓存
        xml.Reload(); //加载网站配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string NoIP = Utils.GetRequest("NoIP", "post", 3, @"^[^\@]{1,15}(?:\@[^\@]{1,15}){0,1000}$", "IP段填写错误");
            string NoIPMsg = Utils.GetRequest("NoIPMsg", "post", 2, @"^[^\^]{1,500}$", "提示信息限1-500字内");
            string NoIPStat = Utils.GetRequest("NoIPStat", "post", 2, @"^[0-1]$", "状态选择错误");

            string[] arrNoIP = NoIP.Split("@".ToCharArray());
            for (int i = 0; i < arrNoIP.Length; i++)
            {
                if (!Ipaddr.IsIPAddress2(arrNoIP[i].ToString()))
                {
                    Utils.Error("IP段填写错误", "");
                }
            }

            xml.ds["SiteNoIP"] = NoIP;
            xml.ds["SiteNoIPMsg"] = NoIPMsg;
            xml.ds["SiteNoIPStat"] = NoIPStat;
            System.IO.File.WriteAllText(Server.MapPath("~/Controls/wap.xml"), xml.Post(xml.ds), System.Text.Encoding.UTF8);
            Utils.Success("设置拒绝IP", "设置拒绝IP成功，正在返回..", Utils.getUrl("iplock.aspx"), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "设置拒绝IP"));

            string strText = "IP(请用“@”作分隔):/,提示信息:/,状态:/";
            string strName = "NoIP,NoIPMsg,NoIPStat";
            string strType = "textarea,text,select";
            string strValu = "" + xml.ds["SiteNoIP"] + "'"+xml.ds["SiteNoIPMsg"]+"'" + xml.ds["SiteNoIPStat"] + "";
            string strEmpt = "true,true,0|启用|1|关闭";
            string strIdea = "/";
            string strOthe = "确定修改|reset,iplock.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("设置的IP在启用状态下将不允许访问网站，多个IP请用“@”作分隔。<br />示例：192.168.1.1@192.3.*.*@202.31.97.*<br />如拒绝202.31.97.0-255整个IP段可以写成202.31.97.*<br />如果设置为空或关闭，将允许所有的IP都可以访问。");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">系统服务中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
}
