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

public partial class Manage_xml_verifyset : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "非验证会员权限设置";
        builder.Append(Out.Tab("", ""));

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/verify.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string Verifytxt = Utils.GetRequest("Verifytxt", "post", 3, @"^[\w((;|,)\w)?]+$", "选择权限选项错误");
            Verifytxt = Verifytxt.Replace(",", ";");

            xml.dss["Verifytxt"] = Verifytxt;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("非验证会员权限设置", "非验证会员权限设置成功，正在返回..", Utils.getUrl("verifyset.aspx"), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "非验证会员权限设置"));

            string strText = "请选择开启的选项:/";
            string strName = "Verifytxt";
            string strType = "multiple";
            string strValu = "" + xml.dss["Verifytxt"] + "";
            string strEmpt = "a|不能发贴|b|不能回帖|c|不能玩游戏|d|不能过户币|e|不能写日记|f|不能上传文件|g|不能聊室发言|h|不能闲聊|i|不能建圈|j|不能自建聊室|k|不能空间留言|l|不能前台评论|m|不能社区评论|n|不奖励" + ub.Get("SiteBz") + "|o|不能发内线|p|不奖励" + ub.Get("SiteBz2") + "";
            string strIdea = "/";
            string strOthe = "确定修改|reset,verifyset.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示:<br />非验证会员指的是通过前台手工注册的会员.<br />选项可以全留空，则非验证会员拥有所有的权限");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
}