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

public partial class Manage_xml_marryset : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "婚姻系统设置";
        builder.Append(Out.Tab("", ""));

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/marry.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string Name = Utils.GetRequest("Name", "post", 2, @"^[^\^]{1,20}$", "系统名称限1-20字内");
            string Logo = Utils.GetRequest("Logo", "post", 3, @"^[^\^]{1,200}$", "Logo地址限200字内");
            string Status = Utils.GetRequest("Status", "post", 2, @"^[0-1]$", "系统状态选择出错");
            string Notes = Utils.GetRequest("Notes", "post", 3, @"^[\s\S]{1,2000}$", "口号限2000字，可留空");
            string GiftNum = Utils.GetRequest("GiftNum", "post", 2, @"^[0-9]\d*$", "送礼次数填写错误");
            string Price = Utils.GetRequest("Price", "post", 2, @"^[0-9]\d*$", "结婚登记费填写错误");
            string LostPrice = Utils.GetRequest("LostPrice", "post", 2, @"^[0-9]\d*$", "离婚手续费填写错误");
            string LostPrice2 = Utils.GetRequest("LostPrice2", "post", 2, @"^[0-9]\d*$", "强制离婚手续费填写错误");

            xml.dss["MarryName"] = Name;
            xml.dss["MarryLogo"] = Logo;
            xml.dss["MarryStatus"] = Status;
            xml.dss["MarryNotes"] = Notes;
            xml.dss["MarryGiftNum"] = GiftNum;
            xml.dss["MarryPrice"] = Price;
            xml.dss["MarryLostPrice"] = LostPrice;
            xml.dss["MarryLostPrice2"] = LostPrice2;

            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("婚姻系统设置", "设置成功，正在返回..", Utils.getUrl("marryset.aspx?backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "婚姻系统设置"));

            string strText = "婚姻名称:/,婚姻Logo(可留空):/,系统状态:/,婚姻口号:/,求婚需送礼次数(不限制请填0):/,结婚登记费(" + ub.Get("SiteBz") + "):/,离婚手续费(" + ub.Get("SiteBz") + "):/,强制离婚手续费(" + ub.Get("SiteBz") + "):/,";
            string strName = "Name,Logo,Status,Notes,GiftNum,Price,LostPrice,LostPrice2,backurl";
            string strType = "text,text,select,textarea,num,num,num,num,hidden";
            string strValu = "" + xml.dss["MarryName"] + "'" + xml.dss["MarryLogo"] + "'" + xml.dss["MarryStatus"] + "'" + xml.dss["MarryNotes"] + "'" + xml.dss["MarryGiftNum"] + "'" + xml.dss["MarryPrice"] + "'" + xml.dss["MarryLostPrice"] + "'" + xml.dss["MarryLostPrice2"] + "'"+Utils.getPage(0)+"";
            string strEmpt = "false,true,0|正常|1|维护,true,false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定修改|reset,marryset.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">返回上级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
}
