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

public partial class Manage_xml_bbsshopset : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "社区商城设置";
        builder.Append(Out.Tab("", ""));

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/bbsshop.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string Name = Utils.GetRequest("Name", "post", 2, @"^[^\^]{1,20}$", "系统名称限1-20字内");
            string Logo = Utils.GetRequest("Logo", "post", 3, @"^[^\^]{1,200}$", "Logo地址限200字内");
            string Status = Utils.GetRequest("Status", "post", 2, @"^[0-1]$", "系统状态选择出错");
            string NewNum = Utils.GetRequest("NewNum", "post", 2, @"^[1-9]\d*$", "新品上架显示条数填写错误");
            string GoodNum = Utils.GetRequest("GoodNum", "post", 2, @"^[1-9]\d*$", "精品推荐显示条数填写错误");//热销商品
            string HotNum = Utils.GetRequest("HotNum", "post", 2, @"^[0-9]\d*$", "热销商品显示条数填写错误");
            string ActNum = Utils.GetRequest("ActNum", "post", 2, @"^[1-9]\d*$", "动态显示条数填写错误");
            string VipSec = Utils.GetRequest("VipSec", "post", 2, @"^(\d)*(\.(\d){0,1})?$", "VIP打折填写错误");

            string MfTotal = Utils.GetRequest("MfTotal", "post", 2, @"^[1-9]\d*$", "免费区单个礼物每天每ID限购填写错误");
            string BgTotal = Utils.GetRequest("BgTotal", "post", 2, @"^[1-9]\d*$", "" + ub.Get("SiteBz2") + "单个礼物每天每ID限购填写错误");

            xml.dss["BbsshopName"] = Name;
            xml.dss["BbsshopLogo"] = Logo;
            xml.dss["BbsshopStatus"] = Status;
            xml.dss["BbsshopGoodNum"] = GoodNum;
            xml.dss["BbsshopNewNum"] = NewNum;
            xml.dss["BbsshopActNum"] = ActNum;
            xml.dss["BbsshopHotNum"] = HotNum;
            xml.dss["BbsshopVipSec"] = VipSec;
            xml.dss["BbsshopMfTotal"] = MfTotal;
            xml.dss["BbsshopBgTotal"] = BgTotal;

            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("商城系统设置", "设置成功，正在返回..", Utils.getUrl("bbsshopset.aspx"), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "商城系统设置"));

            string strText = "商城名称:/,商城Logo(可留空):/,系统状态:/,新品上架显示条数:/,精品推荐显示条数:/,热销商品显示条数:/,动态显示条数:/,VIP打折(折):/,免费区单个礼物每天每ID限购(份):/," + ub.Get("SiteBz2") + "类单个礼物每天每ID限购(份):/";
            string strName = "Name,Logo,Status,NewNum,GoodNum,HotNum,ActNum,VipSec,MfTotal,BgTotal";
            string strType = "text,text,select,snum,snum,snum,snum,text,num,num";
            string strValu = "" + xml.dss["BbsshopName"] + "'" + xml.dss["BbsshopLogo"] + "'" + xml.dss["BbsshopStatus"] + "'" + xml.dss["BbsshopNewNum"] + "'" + xml.dss["BbsshopGoodNum"] + "'" + xml.dss["BbsshopHotNum"] + "'" + xml.dss["BbsshopActNum"] + "'" + xml.dss["BbsshopVipSec"] + "'" + xml.dss["BbsshopMfTotal"] + "'" + xml.dss["BbsshopBgTotal"] + "";
            string strEmpt = "false,true,0|正常|1|维护,false,false,false,false,text,num,num";
            string strIdea = "/VIP打折支持小数，如填写9.5，即VIP会员购买时可以打9.5折/";
            string strOthe = "确定修改|reset,bbsshopset.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
}

