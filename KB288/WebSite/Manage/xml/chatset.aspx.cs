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

public partial class Manage_xml_chatset : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "聊吧系统设置";
        builder.Append(Out.Tab("", ""));

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/chat.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string Name = Utils.GetRequest("Name", "post", 2, @"^[^\^]{1,20}$", "系统名称限1-20字内");
            string Logo = Utils.GetRequest("Logo", "post", 3, @"^[^\^]{1,200}$", "Logo地址限200字内");
            string Status = Utils.GetRequest("Status", "post", 2, @"^[0-1]$", "系统状态选择出错");
            string ContentNum = Utils.GetRequest("ContentNum", "post", 2, @"^[1-9]\d*$", "内容限字数填写错误");
            string Expir = Utils.GetRequest("Expir", "post", 2, @"^[0-9]\d*$", "防刷秒数填写错误");
            string IsAdd = Utils.GetRequest("IsAdd", "post", 2, @"^[0-1]$", "是否允许自建聊室选择错误");
            string AddLeven = Utils.GetRequest("AddLeven", "post", 2, @"^[0-9]\d*$", "自建需要等级填写错误");
            string AddPrice = Utils.GetRequest("AddPrice", "post", 2, @"^[0-9]\d*$", "自建聊室手续费填写错误");
            string iPrice = Utils.GetRequest("iPrice", "post", 2, @"^[0-9]\d*$", "每小时需付多少币填写错误");

            if (Convert.ToInt32(ContentNum) > 500)
                ContentNum = "500";

            xml.dss["ChatName"] = Name;
            xml.dss["ChatLogo"] = Logo;
            xml.dss["ChatStatus"] = Status;
            xml.dss["ChatContentNum"] = ContentNum;
            xml.dss["ChatExpir"] = Expir;
            xml.dss["ChatIsAdd"] = IsAdd;
            xml.dss["ChatAddLeven"] = AddLeven;
            xml.dss["ChatAddPrice"] = AddPrice;
            xml.dss["ChatiPrice"] = iPrice;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("聊吧系统设置", "设置成功，正在返回..", Utils.getUrl("chatset.aspx"), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "聊吧系统设置"));

            string strText = "聊吧名称:/,聊吧Logo(可留空):/,系统状态:/,发言内容最大限多少字:/,发言防刷(秒):/,是否允许自建聊室:/,自建需要等级(级):/,自建聊室手续费(币):/,每小时需付多少币:/";
            string strName = "Name,Logo,Status,ContentNum,Expir,IsAdd,AddLeven,AddPrice,iPrice";
            string strType = "text,text,select,snum,snum,select,num,num,num";
            string strValu = "" + xml.dss["ChatName"] + "'" + xml.dss["ChatLogo"] + "'" + xml.dss["ChatStatus"] + "'" + xml.dss["ChatContentNum"] + "'" + xml.dss["ChatExpir"] + "'" + xml.dss["ChatIsAdd"] + "'" + xml.dss["ChatAddLeven"] + "'" + xml.dss["ChatAddPrice"] + "'" + xml.dss["ChatiPrice"] + "";
            string strEmpt = "false,true,0|正常|1|维护,false,false,0|允许|1|禁止,false,false,false";
            string strIdea = "/";
            string strOthe = "确定修改|reset,chatset.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
}

