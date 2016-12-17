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

public partial class Manage_xml_financeset : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "金融服务设置";
        builder.Append(Out.Tab("", ""));

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/finance.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string Status = Utils.GetRequest("Status", "post", 2, @"^[0-1]$", "系统状态选择错误");
            string BankKh = Utils.GetRequest("BankKh", "post", 2, @"^[0-9]\d*$", "银行开户手续费填写错误");
            string BankType = Utils.GetRequest("BankType", "post", 2, @"^[0-1]$", "银行利息周期选择错误");
            string BankTar = Utils.GetRequest("BankTar", "post", 2, @"^[0-9]\d*$", "银行每周利率填写错误，必须为整数");
            string Paybig = Utils.GetRequest("Paybig", "post", 2, @"^[0-9]\d*$", "每次最多存储多少填写错误");
            string BzTar = Utils.GetRequest("BzTar", "post", 2, @"^[1-9]\d*$", "兑换比例填写错误");
            string PayTar = Utils.GetRequest("PayTar", "post", 3, @"^(\d)*(\.(\d){0,1})?$", "" + ub.Get("SiteBz") + "过户手续费填写错误");
            string BzTar2 = Utils.GetRequest("BzTar2", "post", 2, @"^[1-9]\d*$", "兑换比例填写错误");
            string PayTar2 = Utils.GetRequest("PayTar2", "post", 3, @"^(\d)*(\.(\d){0,1})?$", "" + ub.Get("SiteBz2") + "过户手续费填写错误");
            string BzMoveSet = Utils.GetRequest("BzMoveSet", "post", 2, @"^[0-3]$", "兑换开关选择错误");
            string PayExpir = Utils.GetRequest("PayExpir", "post", 2, @"^[0-9]\d*$", "过户防刷填写错误");
            string SZXType = Utils.GetRequest("SZXType", "post", 2, @"^[0-1]$", "充入币种填写错误");
            string SZXTar = Utils.GetRequest("SZXTar", "post", 2, @"^[0-9]\d*$", "1元等于多少充值币填写错误");
            string Amt1 = Utils.GetRequest("Amt1", "post", 3, @"^[^\^]{1,200}$", "神州行开放面额填写错误");
            string Amt2 = Utils.GetRequest("Amt2", "post", 3, @"^[^\^]{1,200}$", "联通开放面额填写错误");
            string Amt3 = Utils.GetRequest("Amt3", "post", 3, @"^[^\^]{1,200}$", "电信开放面额填写错误");
            string AmtType= Utils.GetRequest("AmtType", "post", 2, @"^[0-1]$", "充值渠道选择错误");
            string SZXNo = Utils.GetRequest("SZXNo", "post", 3, @"^[\s\S]{1,200}$", "易宝商户编号填写错误");
            string SZXPass = Utils.GetRequest("SZXPass", "post", 3, @"^[s\S]{1,200}$", "易宝商户密钥填写错误");

            if (BzTar != "1" && BzTar2 != "1")
            {
                Utils.Error("兑换比例填写错误，兑换比例例子1:xx或xx:1", "");
            }

            xml.dss["FinanceStatus"] = Status;
            xml.dss["FinanceBankKh"] = BankKh;
            xml.dss["FinanceBankType"] = BankType;
            xml.dss["FinanceBankTar"] = BankTar;
            xml.dss["FinanceBzTar"] = BzTar;
            xml.dss["FinancePayTar"] = PayTar;
            xml.dss["FinanceBzTar2"] = BzTar2;
            xml.dss["FinancePayTar2"] = PayTar2;
            xml.dss["FinanceBzMoveSet"] = BzMoveSet;
            xml.dss["FinancePayExpir"] = PayExpir;
            xml.dss["FinancePaybig"] = Paybig;
            xml.dss["FinanceSZXType"] = SZXType;
            xml.dss["FinanceSZXTar"] = SZXTar;
            xml.dss["FinanceAmt1"] = Amt1;
            xml.dss["FinanceAmt2"] = Amt2;
            xml.dss["FinanceAmt3"] = Amt3;
            xml.dss["FinanceAmtType"] = AmtType;
            xml.dss["FinanceSZXNo"] = SZXNo;
            xml.dss["FinanceSZXPass"] = SZXPass;

            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("金融服务设置", "设置成功，正在返回..", Utils.getUrl("financeset.aspx"), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "金融服务设置"));

            string strText = "金融服务状态:/," + ub.Get("SiteBz") + "银行开户手续费:/," + ub.Get("SiteBz") + "银行利息周期:/," + ub.Get("SiteBz") + "银行利率(‰):/,每次最多存取" + ub.Get("SiteBz") + " :/," + ub.Get("SiteBz") + "-" + ub.Get("SiteBz2") + "兑换比例:/,比,兑换开关:/," + ub.Get("SiteBz") + "过户手续费(%|留空不开放过户):/," + ub.Get("SiteBz2") + "过户手续费(%|留空不开放过户):/,过户防刷(秒):/,充入币种:/,1元等于多少充值币:/,神州行开放充值的面额:/,联通卡开放充值的面额:/,电信卡开放充值的面额:/,充值渠道(选择易宝支付则以下需填写):/,易宝商户编号:/,易宝商户密钥:/";
            string strName = "Status,BankKh,BankType,BankTar,Paybig,BzTar,BzTar2,BzMoveSet,PayTar,PayTar2,PayExpir,SZXType,SZXTar,Amt1,Amt2,Amt3,AmtType,SZXNo,SZXPass";
            string strType = "select,num,select,num,num,snum,snum,select,text,text,num,select,num,text,text,text,select,text,text";
            string strValu = "" + xml.dss["FinanceStatus"] + "'" + xml.dss["FinanceBankKh"] + "'" + xml.dss["FinanceBankType"] + "'" + xml.dss["FinanceBankTar"] + "'" + xml.dss["FinancePaybig"] + "'" + xml.dss["FinanceBzTar"] + "'" + xml.dss["FinanceBzTar2"] + "'" + xml.dss["FinanceBzMoveSet"] + "'" + xml.dss["FinancePayTar"] + "'" + xml.dss["FinancePayTar2"] + "'" + xml.dss["FinancePayExpir"] + "'" + xml.dss["FinanceSZXType"] + "'" + xml.dss["FinanceSZXTar"] + "'" + xml.dss["FinanceAmt1"] + "'" + xml.dss["FinanceAmt2"] + "'" + xml.dss["FinanceAmt3"] + "'" + xml.dss["FinanceAmtType"] + "'" + xml.dss["FinanceSZXNo"] + "'" + xml.dss["FinanceSZXPass"] + "";
            string strEmpt = "0|正常|1|维护,false,0|按周计息|1|按日计息,false,false,false,false,0|" + ub.Get("SiteBz") + "兑换" + ub.Get("SiteBz2") + "|1|" + ub.Get("SiteBz2") + "兑换" + ub.Get("SiteBz") + "|2|全部开放|3|全部关闭,true,true,false,0|充值" + ub.Get("SiteBz") + "|1|充值" + ub.Get("SiteBz2") + ",false,true,true,true,0|系统自带|1|易宝支付,true,true";
            string strIdea = "/";
            string strOthe = "确定修改|reset,financeset.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("温馨提示:<br />过户手续费支持填写1位小数，如0.1，填0则免手续费<br />充值面额填写格式为：30#50#100，用#分开<br />以下是各运营商的卡面值参考:<br  />");
            builder.Append("神州行:10#30#50#100#300#500<br />联通卡:20#30#40#100#300<br />电信:50#100<br  />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));

        }
    }
}
