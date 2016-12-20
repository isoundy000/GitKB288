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
public partial class Manage_xml_vbookset : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "留言系统设置";
        builder.Append(Out.Tab("", ""));

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/vbook.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string Name = Utils.GetRequest("Name", "post", 2, @"^[^\^]{1,20}$", "系统名称限1-20字内");
            string Notes = Utils.GetRequest("Notes", "post", 3, @"^[^\^]{1,50}$", "留言口号限50字内");
            string Logo = Utils.GetRequest("Logo", "post", 3, @"^[^\^]{1,200}$", "Logo地址限200字内");
            string Status = Utils.GetRequest("Status", "post", 2, @"^[0-1]$", "系统状态选择出错");
            string IsAc = Utils.GetRequest("IsAc", "post", 2, @"^[0-1]$", "留言性质选择错误");
            string IsUser = Utils.GetRequest("IsUser", "post", 2, @"^[0-1]$", "发表限制选择错误");
            string Expir = Utils.GetRequest("Expir", "post", 2, @"^[0-9]\d*$", "防刷时间填写错误");

            xml.dss["VBookName"] = Name;
            xml.dss["VBookNotes"] = Notes;
            xml.dss["VBookLogo"] = Logo;
            xml.dss["VBookStatus"] = Status;
            xml.dss["VBookIsAc"] = IsAc;
            xml.dss["VBookIsUser"] = IsUser;
            xml.dss["VBookExpir"] = Expir;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("留言系统设置", "设置成功，正在返回..", Utils.getUrl("vbookset.aspx"), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "留言系统设置"));

            string strText = "留言名称:/,留言口号(可留空):/,留言Logo(可留空):/,系统状态:/,留言性质:/,发表限制:/,防刷时间(秒):/";
            string strName = "Name,Notes,Logo,Status,IsAc,IsUser,Expir";
            string strType = "text,text,text,select,select,select,num";
            string strValu = "" + xml.dss["VBookName"] + "'" + xml.dss["VBookNotes"] + "'" + xml.dss["VBookLogo"] + "'" + xml.dss["VBookStatus"] + "'" + xml.dss["VBookIsAc"] + "'" + xml.dss["VBookIsUser"] + "'" + xml.dss["VBookExpir"] + "";
            string strEmpt = "false,true,true,0|正常|1|维护,0|立即显示|1|回复后显示,0|不作限制|1|仅限会员,false";
            string strIdea = "/";
            string strOthe = "确定修改|reset,vbookset.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
}
