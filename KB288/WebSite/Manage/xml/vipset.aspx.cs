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

public partial class Manage_xml_vipset : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "VIP系统设置";
        builder.Append(Out.Tab("", ""));

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/vip.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string Cent1 = Utils.GetRequest("Cent1", "post", 2, @"^[0-9]\d*$", "费用填写错误");
            string Cent2 = Utils.GetRequest("Cent2", "post", 2, @"^[0-9]\d*$", "费用填写错误");
            string Cent3 = Utils.GetRequest("Cent3", "post", 2, @"^[0-9]\d*$", "费用填写错误");
            string Cent4 = Utils.GetRequest("Cent4", "post", 2, @"^[0-9]\d*$", "费用填写错误");
            string bzType = Utils.GetRequest("bzType", "post", 2, @"^[0-1]\d*$", "消费币种选择错误");
            string Grow1 = Utils.GetRequest("Grow1", "post", 2, @"^[0-9]\d*$", "每天成长点填写错误");
            string Grow2 = Utils.GetRequest("Grow2", "post", 2, @"^[0-9]\d*$", "每天成长点填写错误");
            string Grow3 = Utils.GetRequest("Grow3", "post", 2, @"^[0-9]\d*$", "每天成长点填写错误");
            string Grow4 = Utils.GetRequest("Grow4", "post", 2, @"^[0-9]\d*$", "每天成长点填写错误");
            string VGrow1 = Utils.GetRequest("VGrow1", "post", 2, @"^[0-9]\d*$", "VIP需成长点填写错误");
            string VGrow2 = Utils.GetRequest("VGrow2", "post", 2, @"^[0-9]\d*$", "VIP需成长点填写错误");
            string VGrow3 = Utils.GetRequest("VGrow3", "post", 2, @"^[0-9]\d*$", "VIP需成长点填写错误");
            string VGrow4 = Utils.GetRequest("VGrow4", "post", 2, @"^[0-9]\d*$", "VIP需成长点填写错误");
            string VGrow5 = Utils.GetRequest("VGrow5", "post", 2, @"^[0-9]\d*$", "VIP需成长点填写错误");
            string VGrow6 = Utils.GetRequest("VGrow6", "post", 2, @"^[0-9]\d*$", "VIP需成长点填写错误");
            string VGrow7 = Utils.GetRequest("VGrow7", "post", 2, @"^[0-9]\d*$", "VIP需成长点填写错误");
            string VGrow8 = Utils.GetRequest("VGrow8", "post", 2, @"^[0-9]\d*$", "VIP需成长点填写错误");
            string Rule = Utils.GetRequest("Rule", "post", 3, @"^[^\^]{1,5000}$", "服务条款限1-5000字符，可留空");

            xml.dss["VipCent1"] = Cent1;
            xml.dss["VipCent2"] = Cent2;
            xml.dss["VipCent3"] = Cent3;
            xml.dss["VipCent4"] = Cent4;
            xml.dss["VipbzType"] = bzType;
            xml.dss["VipGrow1"] = Grow1;
            xml.dss["VipGrow2"] = Grow2;
            xml.dss["VipGrow3"] = Grow3;
            xml.dss["VipGrow4"] = Grow4;
            xml.dss["VipVGrow1"] = VGrow1;
            xml.dss["VipVGrow2"] = VGrow2;
            xml.dss["VipVGrow3"] = VGrow3;
            xml.dss["VipVGrow4"] = VGrow4;
            xml.dss["VipVGrow5"] = VGrow5;
            xml.dss["VipVGrow6"] = VGrow6;
            xml.dss["VipVGrow7"] = VGrow7;
            xml.dss["VipVGrow8"] = VGrow8;
            xml.dss["VipRule"] = Rule;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("VIP系统设置", "设置成功，正在返回..", Utils.getUrl("vipset.aspx"), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "VIP系统设置"));

            string strText = "=费用与每天成长点=/开通或续费一个月费用/,开通或续费三个月费用/,开通或续费六个月费用/,开通或续费一年费用/,消费币种:/,开通或续费一个月(成长)/,开通或续费三个月(成长)/,开通或续费六个月(成长)/,开通或续费一年(成长)/,=等级制度设置=/VIP一级需成长点:/,VIP二级需成长点:/,VIP三级需成长点:/,VIP四级需成长点:/,VIP五级需成长点:/,VIP六级需成长点:/,VIP七级需成长点:/,VIP八级需成长点:/,VIP会员服务条款:/";
            string strName = "Cent1,Cent2,Cent3,Cent4,bzType,Grow1,Grow2,Grow3,Grow4,VGrow1,VGrow2,VGrow3,VGrow4,VGrow5,VGrow6,VGrow7,VGrow8,Rule";
            string strType = "num,num,num,num,select,num,num,num,num,num,num,num,num,num,num,num,num,textarea";
            string strValu = "" + xml.dss["VipCent1"] + "'" + xml.dss["VipCent2"] + "'" + xml.dss["VipCent3"] + "'" + xml.dss["VipCent4"] + "'" + xml.dss["VipbzType"] + "'" + xml.dss["VipGrow1"] + "'" + xml.dss["VipGrow2"] + "'" + xml.dss["VipGrow3"] + "'" + xml.dss["VipGrow4"] + "'" + xml.dss["VipVGrow1"] + "'" + xml.dss["VipVGrow2"] + "'" + xml.dss["VipVGrow3"] + "'" + xml.dss["VipVGrow4"] + "'" + xml.dss["VipVGrow5"] + "'" + xml.dss["VipVGrow6"] + "'" + xml.dss["VipVGrow7"] + "'" + xml.dss["VipVGrow8"] + "'" + xml.dss["VipRule"] + "";
            string strEmpt = "false,false,false,false,0|" + ub.Get("SiteBz") + "|1|" + ub.Get("SiteBz2") + ",false,false,false,false,false,false,false,false,false,false,false,false,true";
            string strIdea = "/";
            string strOthe = "确定修改|reset,vipset.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
}
