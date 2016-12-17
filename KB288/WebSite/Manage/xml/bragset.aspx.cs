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

public partial class Manage_xml_bragset : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "吹牛游戏设置";
        builder.Append(Out.Tab("", ""));

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/brag.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string Name = Utils.GetRequest("Name", "post", 2, @"^[^\^]{1,20}$", "系统名称限1-20字内");
            string Notes = Utils.GetRequest("Notes", "post", 3, @"^[^\^]{1,2000}$", "口号限2000字内");
            string Logo = Utils.GetRequest("Logo", "post", 3, @"^[^\^]{1,200}$", "Logo地址限200字内");
            string Status = Utils.GetRequest("Status", "post", 2, @"^[0-1]$", "系统状态选择出错");
            string SmallPay = Utils.GetRequest("SmallPay", "post", 4, @"^[1-9]\d*$", "吹牛最小" + ub.Get("SiteBz") + "填写错误");
            string BigPay = Utils.GetRequest("BigPay", "post", 4, @"^[1-9]\d*$", "吹牛最大" + ub.Get("SiteBz") + "填写错误");
            string Tar1 = Utils.GetRequest("Tar1", "post", 2, @"^(\d)*(\.(\d){0,1})?$", "蜗牛赢利手续费填写错误");
            string Tar2 = Utils.GetRequest("Tar2", "post", 2, @"^(\d)*(\.(\d){0,1})?$", "水牛赢利手续费填写错误");
            string Tar3 = Utils.GetRequest("Tar3", "post", 2, @"^(\d)*(\.(\d){0,1})?$", "犀牛赢利手续费填写错误");
            string Foot = Utils.GetRequest("Foot", "post", 3, @"^[\s\S]{1,2000}$", "底部Ubb限2000字内");
            string TNum = Utils.GetRequest("TNum", "post", 2, @"^[0-9]\d*$", "应答限制填写错误");
            string F1 = Utils.GetRequest("F1", "post", 2, @"^[0-9]\d*$", "水牛金额(" + ub.Get("SiteBz") + ")填写错误");
            string F2 = Utils.GetRequest("F2", "post", 2, @"^[0-9]\d*$", "犀牛金额(" + ub.Get("SiteBz") + ")填写错误");
            string Expir = Utils.GetRequest("Expir", "post", 2, @"^[0-9]\d*$", "防刷秒数填写错误");
            string IsBot = Utils.GetRequest("IsBot", "post", 2, @"^[0-1]$", "机器人选择出错");

                string SmallPay2 = Utils.GetRequest("SmallPay2", "post", 4, @"^[1-9]\d*$", "吹牛最小" + ub.Get("SiteBz2") + "填写错误");
                string BigPay2 = Utils.GetRequest("BigPay2", "post", 4, @"^[1-9]\d*$", "吹牛最大" + ub.Get("SiteBz2") + "填写错误");
                string F3 = Utils.GetRequest("F3", "post", 2, @"^[0-9]\d*$", "水牛金额(" + ub.Get("SiteBz2") + ")填写错误");
                string F4 = Utils.GetRequest("F4", "post", 2, @"^[0-9]\d*$", "犀牛金额(" + ub.Get("SiteBz2") + ")填写错误");
                xml.dss["BragSmallPay2"] = SmallPay2;
                xml.dss["BragBigPay2"] = BigPay2;
                xml.dss["BragF3"] = F3;
                xml.dss["BragF4"] = F4;
                xml.dss["BragIsBot"] = IsBot;

            xml.dss["BragName"] = Name;
            xml.dss["BragNotes"] = Notes;
            xml.dss["BragLogo"] = Logo;
            xml.dss["BragStatus"] = Status;
            xml.dss["BragSmallPay"] = SmallPay;
            xml.dss["BragBigPay"] = BigPay;
            xml.dss["BragTar1"] = Tar1;
            xml.dss["BragTar2"] = Tar2;
            xml.dss["BragTar3"] = Tar3;
            xml.dss["BragFoot"] = Foot;
            xml.dss["BragTNum"] = TNum;
            xml.dss["BragF1"] = F1;
            xml.dss["BragF2"] = F2;
            xml.dss["BragExpir"] = Expir;

            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("吹牛游戏设置", "设置成功，正在返回..", Utils.getUrl("bragset.aspx?backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "吹牛游戏设置"));
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("吹牛设置|<a href=\"" + Utils.getUrl("spkadminset.aspx?ptype=14&amp;backurl=" + Utils.PostPage(true) + "") + "\">管理员</a>");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "游戏名称:/,游戏口号(可留空):/,游戏Logo(可留空):/,游戏状态:/,吹牛最小" + ub.Get("SiteBz") + ":/,吹牛最大" + ub.Get("SiteBz") + ":/,吹牛最小" + ub.Get("SiteBz2") + ":/,吹牛最大" + ub.Get("SiteBz2") + ":/,蜗牛获胜手续费(%):/,水牛获胜手续费(%):/,犀牛获胜手续费(%):/,底部Ubb:/,水牛金额(" + ub.Get("SiteBz") + "):/,犀牛金额(" + ub.Get("SiteBz") + "):/,水牛金额(" + ub.Get("SiteBz2") + "):/,犀牛金额(" + ub.Get("SiteBz2") + "):/,每人每天应答N次:/,发吹牛防刷(秒):/,机器人:/,";
            string strName = "Name,Notes,Logo,Status,SmallPay,BigPay,SmallPay2,BigPay2,Tar1,Tar2,Tar3,Foot,F1,F2,F3,F4,TNum,Expir,IsBot,backurl";
            string strType = "text,text,text,select,num,num,num,num,text,text,text,textarea,num,num,num,num,num,num,select,hidden";
            string strValu = "" + xml.dss["BragName"] + "'" + xml.dss["BragNotes"] + "'" + xml.dss["BragLogo"] + "'" + xml.dss["BragStatus"] + "'" + xml.dss["BragSmallPay"] + "'" + xml.dss["BragBigPay"] + "'" + xml.dss["BragSmallPay2"] + "'" + xml.dss["BragBigPay2"] + "'" + xml.dss["BragTar1"] + "'" + xml.dss["BragTar2"] + "'" + xml.dss["BragTar3"] + "'" + xml.dss["BragFoot"] + "'" + xml.dss["BragF1"] + "'" + xml.dss["BragF2"] + "'" + xml.dss["BragF3"] + "'" + xml.dss["BragF4"] + "'" + xml.dss["BragTNum"] + "'" + xml.dss["BragExpir"] + "'" + xml.dss["BragIsBot"] + "'" + Utils.getPage(0) + "";
            string strEmpt = "false,true,true,0|正常|1|维护,false,false,false,false,false,false,false,true,false,false,false,false,false,false,0|关闭|1|开启,false";
            string strIdea = "/";
            string strOthe = "确定修改|reset,bragset.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示:<br />每人每天应答N次指每人每天可以先应答N次,之后必须自己吹一个牛才能再应答N次<br />如果设置0则全部不限制");
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
