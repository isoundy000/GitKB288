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

/// <summary>
/// 闲聊奖励 黄国军 20160813
/// </summary>
public partial class Manage_xml_spkadminset : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "闲聊管理员设置";
        builder.Append(Out.Tab("", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-9]\d*$", "0"));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/spkadmin.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string SpkAdmin = Utils.GetRequest("SpkAdmin", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "多个管理员ID请用#分隔，可以留空");
            string SpkLimit = Utils.GetRequest("SpkLimit", "post", 2, @"^[0-9]\d*$", "奖币上限填写错误");
            string SpkOne = Utils.GetRequest("SpkOne", "post", 2, @"^[0-9]\d*$", "发言奖币数填写错误");

            xml.dss["SpkAdmin" + ptype + ""] = SpkAdmin;
            xml.dss["SpkLimit"] = SpkLimit;
            xml.dss["SpkOne"] = SpkOne;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("闲聊管理员设置", "设置闲聊管理员成功，正在返回..", Utils.getUrl("spkadminset.aspx?ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "" + BCW.User.AppCase.CaseAction(ptype) + "&gt;管理员设置"));

            string strText = "管理员ID:/,奖币上限(条):/,发言奖币数(每条):/,,";
            string strName = "SpkAdmin,SpkLimit,SpkOne,ptype,backurl";
            string strType = "text,num,num,hidden,hidden";
            string strValu = "" + xml.dss["SpkAdmin" + ptype + ""] + "'" + xml.dss["SpkLimit"] + "'" + xml.dss["SpkOne"] + "'" + ptype + "'" + Utils.getPage(0) + "";
            string strEmpt = "true,true,true,false,false";
            string strIdea = "/";
            string strOthe = "确定修改|reset,spkadminset.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示:多个管理员ID请用#分隔");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">返回上级</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
}
