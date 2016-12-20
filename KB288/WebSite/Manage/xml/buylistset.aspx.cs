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

public partial class Manage_xml_buylistset : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "订单系统设置";

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/buylist.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string pText = Utils.GetRequest("pText", "post", 3, @"^[^\^]{1,2000}$", "付款方式限2000字");
            string PayNum = Utils.GetRequest("PayNum", "post", 2, @"^[0-9]\d*$", "每天限购填写错误");
            string Expir = Utils.GetRequest("Expir", "post", 2, @"^[0-9]\d*$", "防刷时间填写错误");
            string ReTar = Utils.GetRequest("ReTar", "post", 1, @"^[0-9]\d*$", "0");
            string ReMax = Utils.GetRequest("ReMax", "post", 1, @"^[0-9]\d*$", "0");
            string pText2 = Utils.GetRequest("pText2", "post", 3, @"^[^\^]{1,2000}$", "推荐说明限2000字");

            xml.dss["BuylistpText"] = pText;
            xml.dss["BuylistPayNum"] = PayNum;
            xml.dss["BuylistExpir"] = Expir;
            xml.dss["BuylistReTar"] = ReTar;
            xml.dss["BuylistReMax"] = ReMax;
            xml.dss["BuylistpText2"] = pText2;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("订单系统设置", "设置成功，正在返回..", Utils.getUrl("buylistset.aspx"), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "订单系统设置"));

            //商品推广功能
            if (Utils.GetDomain().Contains("127.0.0.6") || Utils.GetDomain().Contains("xgbxj.net"))
            {

                string strText = "付款方式(支持UBB):/,每ID每天限购买订单多少个:/,防刷时间(秒):/,推荐分成(%):/,多少元可以提款:/,推荐说明(支持UBB):/";
                string strName = "pText,PayNum,Expir,ReTar,ReMax,pText2";
                string strType = "textarea,num,num,num,num,textarea";
                string strValu = "" + xml.dss["BuylistpText"] + "'" + xml.dss["BuylistPayNum"] + "'" + xml.dss["BuylistExpir"] + "'" + xml.dss["BuylistReTar"] + "'" + xml.dss["BuylistReMax"] + "'" + xml.dss["BuylistpText2"] + "";
                string strEmpt = "true,false,false,false,false,true";
                string strIdea = "/";
                string strOthe = "确定修改|reset,buylistset.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
            else
            {
                string strText = "付款方式(支持UBB):/,每ID每天限购买订单多少个:/,防刷时间(秒):/";
                string strName = "pText,PayNum,Expir";
                string strType = "textarea,num,num";
                string strValu = "" + xml.dss["BuylistpText"] + "'" + xml.dss["BuylistPayNum"] + "'" + xml.dss["BuylistExpir"] + "";
                string strEmpt = "true,false,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,buylistset.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            
            }
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
}
