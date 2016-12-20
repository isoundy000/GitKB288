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

public partial class Manage_xml_guestset : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "短消息系统设置";
        builder.Append(Out.Tab("", ""));

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/guest.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string Status = Utils.GetRequest("Status", "post", 2, @"^[0-1]\d*$", "短消息状态选择错误");
            string Expir = Utils.GetRequest("Expir", "post", 2, @"^[1-9]\d*$", "发消息防刷秒数填写错误");
            string Add = Utils.GetRequest("Add", "post", 2, @"^[0-1]$", "群发消息性质选择错误");
            string All = Utils.GetRequest("All", "post", 2, @"^[0-9]\d*$", "群发收费填写错误");
            string Share = Utils.GetRequest("Share", "post", 2, @"^[0-9]\d*$", "分享收费填写错误");
            string ReportID = Utils.GetRequest("ReportID", "post", 2, @"^[1-9]\d*$", "接收举报ID填写错误");
            xml.dss["GuestStatus"] = Status;
            xml.dss["GuestExpir"] = Expir;
            xml.dss["GuestAdd"] = Add;
            xml.dss["GuestAll"] = All;
            xml.dss["GuestShare"] = Share;
            xml.dss["GuestReportID"] = ReportID;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("短消息系统设置", "设置成功，正在返回..", Utils.getUrl("guestset.aspx"), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "短消息系统设置"));

            string strText = "会员短消息状态:/,发消息防刷秒数:/,群发消息:/,群发消息收费(按每人收费):/,分享收费:/,接收举报ID:/:";
            string strName = "Status,Expir,Add,All,Share,ReportID";
            string strType = "select,num,select,num,num,num";
            string strValu = "" + xml.dss["GuestStatus"] + "'" + xml.dss["GuestExpir"] + "'" + xml.dss["GuestAdd"] + "'" + xml.dss["GuestAll"] + "'" + xml.dss["GuestShare"] + "'" + xml.dss["GuestReportID"] + "";
            string strEmpt = "0|正常|1|维护,false,0|开启|1|关闭,false,false,false";
            string strIdea = "/";
            string strOthe = "确定修改|reset,guestset.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
}