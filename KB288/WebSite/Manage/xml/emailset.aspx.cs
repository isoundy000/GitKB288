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

public partial class Manage_xml_emailset : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "邮箱系统设置";
        builder.Append(Out.Tab("", ""));

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/email.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string From = Utils.GetRequest("From", "post", 3, @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", "请输入正确的邮箱地址");
            string FromUser = Utils.GetRequest("FromUser", "post", 3, @"^[\s\S]{1,30}$", "登录帐号限1-30字内");
            string FromPwd = Utils.GetRequest("FromPwd", "post", 3, @"^[\s\S]{1,30}$", "登录密码限1-30字内");
            string FromHost = Utils.GetRequest("FromHost", "post", 3, @"^[\s\S]{1,50}$", "主机地址限1-50字内");
            string FromPort = Utils.GetRequest("FromPort", "post", 3, @"^[0-9]\d*$", "主机端口填写错误");

            xml.dss["EmailFrom"] = From;
            xml.dss["EmailFromUser"] = FromUser;
            xml.dss["EmailFromPwd"] = FromPwd;
            xml.dss["EmailFromHost"] = FromHost;
            xml.dss["EmailFromPort"] = FromPort;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("邮箱系统设置", "设置成功，正在返回..", Utils.getUrl("emailset.aspx"), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "邮箱系统设置"));

            string strText = "邮箱地址:/,登录帐号:/,登录密码:/,邮件主机地址:/,邮件主机端口:/";
            string strName = "From,FromUser,FromPwd,FromHost,FromPort";
            string strType = "text,text,text,text,num";
            string strValu = "" + xml.dss["EmailFrom"] + "'" + xml.dss["EmailFromUser"] + "'" + xml.dss["EmailFromPwd"] + "'" + xml.dss["EmailFromHost"] + "'" + xml.dss["EmailFromPort"] + "";
            string strEmpt = "true,true,true,true,true";
            string strIdea = "/";
            string strOthe = "确定修改|reset,emailset.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示:<br />邮件功能作为用户找回密码、将附件转向用户邮箱的一个转发服务，以后将会应用更多的网站服务中.<br />所有选项留空时，系统将自动调用默认帐号nowtx.net@gmail.com作为转发邮箱.");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
}
