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

public partial class Manage_xml_moraset : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "猜拳游戏设置";
        builder.Append(Out.Tab("", ""));

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/mora.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string Name = Utils.GetRequest("Name", "post", 2, @"^[^\^]{1,20}$", "系统名称限1-20字内");
            string Notes = Utils.GetRequest("Notes", "post", 3, @"^[^\^]{1,2000}$", "口号限2000字内");
            string Logo = Utils.GetRequest("Logo", "post", 3, @"^[^\^]{1,200}$", "Logo地址限200字内");
            string Status = Utils.GetRequest("Status", "post", 2, @"^[0-1]$", "系统状态选择出错");
            string SmallPay = Utils.GetRequest("SmallPay", "post", 4, @"^[1-9]\d*$", "猜拳最小" + ub.Get("SiteBz") + "填写错误");
            string BigPay = Utils.GetRequest("BigPay", "post", 4, @"^[1-9]\d*$", "猜拳最大" + ub.Get("SiteBz") + "填写错误");
            string Tar1 = Utils.GetRequest("Tar1", "post", 2, @"^(\d)*(\.(\d){0,1})?$", "蜗牛赢利手续费填写错误");
            string Tar2 = Utils.GetRequest("Tar2", "post", 2, @"^(\d)*(\.(\d){0,1})?$", "水牛赢利手续费填写错误");
            string Foot = Utils.GetRequest("Foot", "post", 3, @"^[\s\S]{1,2000}$", "底部Ubb限2000字内");
            string TNum = Utils.GetRequest("TNum", "post", 2, @"^[0-9]\d*$", "应答限制填写错误");
            string F1 = Utils.GetRequest("F1", "post", 2, @"^[0-9]\d*$", "水牛金额(" + ub.Get("SiteBz") + ")填写错误");
            string Expir = Utils.GetRequest("Expir", "post", 2, @"^[0-9]\d*$", "防刷秒数填写错误");
            string WinID = Utils.GetRequest("WinID", "post", 1, "", "");
            string ZWinID = Utils.GetRequest("ZWinID", "post", 1, "", "");
            string WinTar = Utils.GetRequest("WinTar", "post", 1, @"^[0-9]\d*$", "0");
            if (int.Parse(WinTar) > 1000)
                Utils.Error("上帝比例不能大于1000", "");

            if (Utils.GetDomain().Contains("kubao") || Utils.GetDomain().Contains("tuhao") || Utils.GetDomain().Contains("th") || Utils.GetDomain().Contains("kb288"))
            {
                string SmallPay2 = Utils.GetRequest("SmallPay2", "post", 4, @"^[1-9]\d*$", "猜拳最小" + ub.Get("SiteBz2") + "填写错误");
                string BigPay2 = Utils.GetRequest("BigPay2", "post", 4, @"^[1-9]\d*$", "猜拳最大" + ub.Get("SiteBz2") + "填写错误");
                string F2 = Utils.GetRequest("F2", "post", 2, @"^[0-9]\d*$", "犀牛金额(" + ub.Get("SiteBz") + ")填写错误");
                xml.dss["MoraSmallPay2"] = SmallPay2;
                xml.dss["MoraBigPay2"] = BigPay2;
                xml.dss["MoraF2"] = F2;
            }
            xml.dss["MoraName"] = Name;
            xml.dss["MoraNotes"] = Notes;
            xml.dss["MoraLogo"] = Logo;
            xml.dss["MoraStatus"] = Status;
            xml.dss["MoraSmallPay"] = SmallPay;
            xml.dss["MoraBigPay"] = BigPay;
            xml.dss["MoraTar1"] = Tar1;
            xml.dss["MoraTar2"] = Tar2;
            xml.dss["MoraFoot"] = Foot;
            xml.dss["MoraTNum"] = TNum;
            xml.dss["MoraF1"] = F1;
            xml.dss["MoraExpir"] = Expir;
            xml.dss["MoraWinID"] = WinID;
            xml.dss["MoraZWinID"] = ZWinID;
            xml.dss["MoraWinTar"] = WinTar;

            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("猜拳游戏设置", "设置成功，正在返回..", Utils.getUrl("moraset.aspx?backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "猜拳游戏设置"));
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("猜拳设置|<a href=\"" + Utils.getUrl("spkadminset.aspx?ptype=15&amp;backurl=" + Utils.PostPage(true) + "") + "\">管理员</a>");
            builder.Append(Out.Tab("</div>", ""));
            if (Utils.GetDomain().Contains("kubao") || Utils.GetDomain().Contains("tuhao") || Utils.GetDomain().Contains("th") || Utils.GetDomain().Contains("kb288"))
            {
                string strText = "游戏名称:/,游戏口号(可留空):/,游戏Logo(可留空):/,游戏状态:/,猜拳最小" + ub.Get("SiteBz") + ":/,猜拳最大" + ub.Get("SiteBz") + ":/,猜拳最小" + ub.Get("SiteBz2") + ":/,猜拳最大" + ub.Get("SiteBz2") + ":/,小局获胜手续费(%):/,大局获胜手续费(%):/,底部Ubb:/,大局金额(" + ub.Get("SiteBz") + "):/,大局金额(" + ub.Get("SiteBz2") + "):/,每人每天应答N次:/,开猜拳防刷(秒):/,";
                string strName = "Name,Notes,Logo,Status,SmallPay,BigPay,SmallPay2,BigPay2,Tar1,Tar2,Foot,F1,F2,TNum,Expir,backurl";
                string strType = "text,text,text,select,num,num,num,num,text,text,textarea,num,num,num,num,hidden";
                string strValu = "" + xml.dss["MoraName"] + "'" + xml.dss["MoraNotes"] + "'" + xml.dss["MoraLogo"] + "'" + xml.dss["MoraStatus"] + "'" + xml.dss["MoraSmallPay"] + "'" + xml.dss["MoraBigPay"] + "'" + xml.dss["MoraSmallPay2"] + "'" + xml.dss["MoraBigPay2"] + "'" + xml.dss["MoraTar1"] + "'" + xml.dss["MoraTar2"] + "'" + xml.dss["MoraFoot"] + "'" + xml.dss["MoraF1"] + "'" + xml.dss["MoraF2"] + "'" + xml.dss["MoraTNum"] + "'" + xml.dss["MoraExpir"] + "'" + Utils.getPage(0) + "";
                string strEmpt = "false,true,true,0|正常|1|维护,false,false,false,false,false,false,true,false,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,moraset.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
            else
            {
                string strText = "游戏名称:/,游戏口号(可留空):/,游戏Logo(可留空):/,游戏状态:/,猜拳最小" + ub.Get("SiteBz") + ":/,猜拳最大" + ub.Get("SiteBz") + ":/,小局获胜手续费(%):/,大局获胜手续费(%):/,底部Ubb:/,多少金额为大局(" + ub.Get("SiteBz") + "):/,每人每天应答N次:/,开猜拳防刷(秒):/,";
                string strName = "Name,Notes,Logo,Status,SmallPay,BigPay,Tar1,Tar2,Foot,F1,TNum,Expir,backurl";
                string strType = "text,text,text,select,num,num,text,text,textarea,num,num,num,hidden";
                string strValu = "" + xml.dss["MoraName"] + "'" + xml.dss["MoraNotes"] + "'" + xml.dss["MoraLogo"] + "'" + xml.dss["MoraStatus"] + "'" + xml.dss["MoraSmallPay"] + "'" + xml.dss["MoraBigPay"] + "'" + xml.dss["MoraTar1"] + "'" + xml.dss["MoraTar2"] + "'" + xml.dss["MoraFoot"] + "'" + xml.dss["MoraF1"] + "'" + xml.dss["MoraTNum"] + "'" + xml.dss["MoraExpir"] + "'" + Utils.getPage(0) + "";
                string strEmpt = "false,true,true,0|正常|1|维护,false,false,false,false,true,false,false,false,false";
                if (Utils.GetDomain().Contains("168yy") || Utils.GetDomain().Contains("tl88") || Utils.GetDomain().Contains("127.0.0.6"))
                {
                    strText += ",闲家上帝ID(多个用#分隔):/,庄家上帝ID(多个用#分隔):/,上帝比例:";
                    strName += ",WinID,ZWinID,WinTar";
                    strType += ",text,text,snum";
                    strValu += "'" + xml.dss["MoraWinID"] + "'" + xml.dss["MoraZWinID"] + "'" + xml.dss["MoraWinTar"] + "";
                    strEmpt += ",true,true,false";
                }
                string strIdea = "/";
                string strOthe = "确定修改|reset,moraset.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示:<br />每人每天应答N次指每人每天可以先应战N次,之后必须自己开一次盘才能再应战N次<br />如果设置0则全部不限制");
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
