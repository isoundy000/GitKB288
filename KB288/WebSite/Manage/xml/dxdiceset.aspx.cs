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

public partial class Manage_xml_dxdiceset : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "掷骰游戏设置";
        builder.Append(Out.Tab("", ""));

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/dxdice.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string Name = Utils.GetRequest("Name", "post", 2, @"^[^\^]{1,20}$", "系统名称限1-20字内");
            string Notes = Utils.GetRequest("Notes", "post", 3, @"^[^\^]{1,2000}$", "口号限2000字内");
            string Logo = Utils.GetRequest("Logo", "post", 3, @"^[^\^]{1,200}$", "Logo地址限200字内");
            string Status = Utils.GetRequest("Status", "post", 2, @"^[0-1]$", "系统状态选择出错");
            string SmallPay = Utils.GetRequest("SmallPay", "post", 4, @"^[1-9]\d*$", "掷骰最小" + ub.Get("SiteBz") + "填写错误");
            string BigPay = Utils.GetRequest("BigPay", "post", 4, @"^[1-9]\d*$", "掷骰最大" + ub.Get("SiteBz") + "填写错误");
            string Tar = Utils.GetRequest("Tar", "post", 2, @"^(\d)*(\.(\d){0,1})?$", "赢利手续费填写错误");
            string Foot = Utils.GetRequest("Foot", "post", 3, @"^[\s\S]{1,2000}$", "底部Ubb限2000字内");
            string F1 = Utils.GetRequest("F1", "post", 2, @"^[0-9]\d*$", "大骰金额(" + ub.Get("SiteBz") + ")填写错误");
            string TNum = Utils.GetRequest("TNum", "post", 2, @"^[0-9]\d*$", "应答限制填写错误");
            string Expir = Utils.GetRequest("Expir", "post", 2, @"^[0-9]\d*$", "防刷秒数填写错误");
            string IsBot = Utils.GetRequest("IsBot", "post", 2, @"^[0-1]$", "机器人选择出错");


            if (Utils.GetDomain().Contains("tkss"))
            {
                string KzID = Utils.GetRequest("KzID", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "多个控制ID请用#分隔，可以留空");
                string BL1 = Utils.GetRequest("BL1", "post", 3, @"^[1-9]\d*$", "比例填写错误");
                string BL2 = Utils.GetRequest("BL2", "post", 3, @"^[1-9]\d*$", "比例填写错误");
                xml.dss["DxdiceKzID"] = KzID;
                xml.dss["DxdiceBL1"] = BL1;
                xml.dss["DxdiceBL2"] = BL2;
            }

            else if (Utils.GetDomain().Contains("kubao") || Utils.GetDomain().Contains("tuhao") || Utils.GetDomain().Contains("th") || Utils.GetDomain().Contains("kb288") || Utils.GetDomain().Contains("127.0.0.6"))
            {
                string SmallPay2 = Utils.GetRequest("SmallPay2", "post", 4, @"^[1-9]\d*$", "掷骰最小" + ub.Get("SiteBz") + "填写错误");
                string BigPay2 = Utils.GetRequest("BigPay2", "post", 4, @"^[1-9]\d*$", "掷骰最大" + ub.Get("SiteBz") + "填写错误");
                string F2 = Utils.GetRequest("F2", "post", 3, @"^[0-9]\d*$", "大骰金额(" + ub.Get("SiteBz2") + ")填写错误");
                xml.dss["DxdiceSmallPay2"] = SmallPay2;
                xml.dss["DxdiceBigPay2"] = BigPay2;
                xml.dss["DxdiceF2"] = F2;
                xml.dss["DxdiceIsBot"] = IsBot;
            }
            else
            {
                xml.dss["DxdiceSmallPay2"] = 0;
                xml.dss["DxdiceBigPay2"] = 0;
                xml.dss["DxdiceF2"] = 0;
            }
            xml.dss["DxdiceName"] = Name;
            xml.dss["DxdiceNotes"] = Notes;
            xml.dss["DxdiceLogo"] = Logo;
            xml.dss["DxdiceStatus"] = Status;
            xml.dss["DxdiceSmallPay"] = SmallPay;
            xml.dss["DxdiceBigPay"] = BigPay;
            xml.dss["DxdiceTar"] = Tar;
            xml.dss["DxdiceFoot"] = Foot;
            xml.dss["DxdiceF1"] = F1;
            xml.dss["DxdiceTNum"] = TNum;
            xml.dss["DxdiceExpir"] = Expir;

            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("掷骰游戏设置", "设置成功，正在返回..", Utils.getUrl("dxdiceset.aspx?backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "掷骰游戏设置"));
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("掷骰设置|<a href=\"" + Utils.getUrl("spkadminset.aspx?ptype=18&amp;backurl=" + Utils.PostPage(true) + "") + "\">管理员</a>");
            builder.Append(Out.Tab("</div>", ""));

            if (Utils.GetDomain().Contains("tkss"))
            {
                string strText = "游戏名称:/,游戏口号(可留空):/,游戏Logo(可留空):/,游戏状态:/,掷骰最小" + ub.Get("SiteBz") + ":/,掷骰最大" + ub.Get("SiteBz") + ":/,获胜手续费(%):/,底部Ubb:/,大骰金额(" + ub.Get("SiteBz") + "):/,每人每天应答N次:/,发掷骰防刷(秒):/,控制ID(多个用#分开):/,赢输比例:,:,";
                string strName = "Name,Notes,Logo,Status,SmallPay,BigPay,Tar,Foot,F1,TNum,Expir,KzID,BL1,BL2,backurl";
                string strType = "text,text,text,select,num,num,text,textarea,num,num,num,text,snum,snum,hidden";
                string strValu = "" + xml.dss["DxdiceName"] + "'" + xml.dss["DxdiceNotes"] + "'" + xml.dss["DxdiceLogo"] + "'" + xml.dss["DxdiceStatus"] + "'" + xml.dss["DxdiceSmallPay"] + "'" + xml.dss["DxdiceBigPay"] + "'" + xml.dss["DxdiceTar"] + "'" + xml.dss["DxdiceFoot"] + "'" + xml.dss["DxdiceF1"] + "'" + xml.dss["DxdiceTNum"] + "'" + xml.dss["DxdiceExpir"] + "'" + xml.dss["DxdiceKzID"] + "'" + xml.dss["DxdiceBL1"] + "'" + xml.dss["DxdiceBL2"] + "'" + Utils.getPage(0) + "";
                string strEmpt = "false,true,true,0|正常|1|维护,false,false,false,true,false,false,false,true,true,true,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,dxdiceset.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
            else
            {
                string strText = "游戏名称:/,游戏口号(可留空):/,游戏Logo(可留空):/,游戏状态:/,掷骰最小" + ub.Get("SiteBz") + ":/,掷骰最大" + ub.Get("SiteBz") + ":/,获胜手续费(%):/,底部Ubb:/,大骰金额(" + ub.Get("SiteBz") + "):/,每人每天应答N次:/,发掷骰防刷(秒):/,";
                string strName = "Name,Notes,Logo,Status,SmallPay,BigPay,Tar,Foot,F1,TNum,Expir,backurl";
                string strType = "text,text,text,select,num,num,text,textarea,num,num,num,hidden";
                string strValu = "" + xml.dss["DxdiceName"] + "'" + xml.dss["DxdiceNotes"] + "'" + xml.dss["DxdiceLogo"] + "'" + xml.dss["DxdiceStatus"] + "'" + xml.dss["DxdiceSmallPay"] + "'" + xml.dss["DxdiceBigPay"] + "'" + xml.dss["DxdiceTar"] + "'" + xml.dss["DxdiceFoot"] + "'" + xml.dss["DxdiceF1"] + "'" + xml.dss["DxdiceTNum"] + "'" + xml.dss["DxdiceExpir"] + "'" + Utils.getPage(0) + "";
                string strEmpt = "false,true,true,0|正常|1|维护,false,false,false,true,false,false,false,false";

                if (Utils.GetDomain().Contains("kubao") || Utils.GetDomain().Contains("tuhao") || Utils.GetDomain().Contains("th") || Utils.GetDomain().Contains("kb288") || Utils.GetDomain().Contains("127.0.0.6"))
                {
                    strText += ",掷骰最小" + ub.Get("SiteBz2") + ":/,掷骰最大" + ub.Get("SiteBz2") + ":/,大骰金额(" + ub.Get("SiteBz2") + "):/,机器人:/";
                    strName += ",SmallPay2,BigPay2,F2,IsBot";
                    strType += ",num,num,num,select";
                    strValu += "'" + xml.dss["DxdiceSmallPay2"] + "'" + xml.dss["DxdiceBigPay2"] + "'" + xml.dss["DxdiceF2"] + "'" + xml.dss["DxdiceIsBot"] + "";
                    strEmpt += ",false,false,false,0|关闭|1|开启";
                }
                string strIdea = "/";
                string strOthe = "确定修改|reset,dxdiceset.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
       

            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示:<br />每人每天应答N次指每人每天可以先应答N次,之后必须自己开盘一次才能再应答N次<br />如果设置0则全部不限制");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("game.aspx") + "\">返回上级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
}
