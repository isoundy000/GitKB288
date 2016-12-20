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

public partial class Manage_xml_bigsmallset : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "大小庄游戏设置";
        builder.Append(Out.Tab("", ""));

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/bigsmall.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string Name = Utils.GetRequest("Name", "post", 2, @"^[^\^]{1,20}$", "系统名称限1-20字内");
            string Notes = Utils.GetRequest("Notes", "post", 3, @"^[^\^]{1,2000}$", "口号限2000字内");
            string Logo = Utils.GetRequest("Logo", "post", 3, @"^[^\^]{1,200}$", "Logo地址限200字内");
            string Status = Utils.GetRequest("Status", "post", 2, @"^[0-1]$", "系统状态选择出错");
            string SmallPay = Utils.GetRequest("SmallPay", "post", 4, @"^[1-9]\d*$", "开庄最小" + ub.Get("SiteBz") + "填写错误");
            string BigPay = Utils.GetRequest("BigPay", "post", 4, @"^[1-9]\d*$", "开庄最大" + ub.Get("SiteBz") + "填写错误");
            string vMoney = Utils.GetRequest("vMoney", "post", 4, @"^[1-9]\d*$", "达多少" + ub.Get("SiteBz") + "显示填写错误");
            string Odds = Utils.GetRequest("Odds", "post", 2, @"^[1-9]\d*$", "赔率填写错误");
            string ZTar = Utils.GetRequest("ZTar", "post", 2, @"^(\d)*(\.(\d){0,1})?$", "庄胜手续费填写错误");
            string XTar = Utils.GetRequest("XTar", "post", 2, @"^(\d)*(\.(\d){0,1})?$", "闲胜手续费填写错误");
            string Foot = Utils.GetRequest("Foot", "post", 3, @"^[\s\S]{1,2000}$", "底部Ubb限2000字内");
            string Expir = Utils.GetRequest("Expir", "post", 2, @"^[0-9]\d*$", "防刷秒数填写错误");
            string IsBot = Utils.GetRequest("IsBot", "post", 2, @"^[0-1]$", "机器人选择出错");

            xml.dss["BsName"] = Name;
            xml.dss["BsNotes"] = Notes;
            xml.dss["BsLogo"] = Logo;
            xml.dss["BsStatus"] = Status;
            xml.dss["BsSmallPay"] = SmallPay;
            xml.dss["BsBigPay"] = BigPay;
            xml.dss["BsvMoney"] = vMoney;
    
                string SmallPay2 = Utils.GetRequest("SmallPay2", "post", 4, @"^[1-9]\d*$", "开庄最小" + ub.Get("SiteBz2") + "填写错误");
                string BigPay2 = Utils.GetRequest("BigPay2", "post", 4, @"^[1-9]\d*$", "开庄最大" + ub.Get("SiteBz2") + "填写错误");
                string vMoney2 = Utils.GetRequest("vMoney2", "post", 4, @"^[1-9]\d*$", "达多少" + ub.Get("SiteBz2") + "显示填写错误");
                string sCent = Utils.GetRequest("sCent", "post", 4, @"^[1-9]\d*$", "最小下注下限" + ub.Get("SiteBz") + "填写错误");
                string bCent = Utils.GetRequest("bCent", "post", 4, @"^[1-9]\d*$", "最大下注上限" + ub.Get("SiteBz") + "填写错误");
                string sCent2 = Utils.GetRequest("sCent2", "post", 4, @"^[1-9]\d*$", "最小下注下限" + ub.Get("SiteBz2") + "填写错误");
                string bCent2 = Utils.GetRequest("bCent2", "post", 4, @"^[1-9]\d*$", "最大下注上限" + ub.Get("SiteBz2") + "填写错误");
                xml.dss["BsSmallPay2"] = SmallPay2;
                xml.dss["BsBigPay2"] = BigPay2;
                xml.dss["BssCent"] = sCent;
                xml.dss["BsbCent"] = bCent;
                xml.dss["BssCent2"] = sCent2;
                xml.dss["BsbCent2"] = bCent2;
                xml.dss["BsvMoney2"] = vMoney2;
    
            xml.dss["BsOdds"] = Odds;
            xml.dss["BsZTar"] = ZTar;
            xml.dss["BsXTar"] = XTar;
            xml.dss["BsFoot"] = Foot;
            xml.dss["BsExpir"] = Expir;
            xml.dss["BsIsBot"] = IsBot;
            xml.dss["BsBotTime"] = DT.FormatDate(DateTime.Now, 0);
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("大小庄游戏设置", "设置成功，正在返回..", Utils.getUrl("bigsmallset.aspx?backurl=" + Utils.getPage(0) + ""), "1");
        }
        else if (Utils.ToSChinese(ac) == "确定设置")
        {
            int ManageId = new BCW.User.Manage().IsManageLogin();
            if (ManageId != 1 && ManageId != 11)
            {
                Utils.Error("不存在的页面", "");
            }
            string WinID = Utils.GetRequest("WinID", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "多个ID请用#分隔，可以留空");
            string LostID = Utils.GetRequest("LostID", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "多个ID请用#分隔，可以留空");
            string ZWinID = Utils.GetRequest("ZWinID", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "多个ID请用#分隔，可以留空");
            string ZLostID = Utils.GetRequest("ZLostID", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "多个ID请用#分隔，可以留空");
            string IsWinBot = Utils.GetRequest("IsWinBot", "post", 2, @"^[0-1]$", "机器人选择出错");
            xml.dss["BsWinID"] = WinID;
            xml.dss["BsLostID"] = LostID;
            xml.dss["BsZWinID"] = ZWinID;
            xml.dss["BsZLostID"] = ZLostID;
            xml.dss["BsIsWinBot"] = IsWinBot;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("大小庄游戏设置", "设置成功，正在返回..", Utils.getUrl("bigsmallset.aspx?act=ok&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            if (Request["act"] == "ok")
            {
                int ManageId = new BCW.User.Manage().IsManageLogin();
                if (ManageId != 1 && ManageId != 11)
                {
                    Utils.Error("不存在的页面", "");
                }
                builder.Append(Out.Div("title", "大小庄游戏设置"));
                string strText = "闲家上帝ID(多个用#分开):/,闲家地狱ID(多个用#分开):/,庄家上帝ID(多个用#分开):/,庄家地狱ID(多个用#分开):/,机器人上帝:/,";
                string strName = "WinID,LostID,ZWinID,ZLostID,IsWinBot,backurl";
                string strType = "textarea,textarea,textarea,textarea,select,hidden";
                string strValu = "" + xml.dss["BsWinID"] + "'" + xml.dss["BsLostID"] + "'" + xml.dss["BsZWinID"] + "'" + xml.dss["BsZLostID"] + "'" + xml.dss["BsIsWinBot"] + "'" + Utils.getPage(0) + "";
                string strEmpt = "true,true,true,true,0|开|1|关,false";
                string strIdea = "/";
                string strOthe = "确定设置|reset,bigsmallset.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
            else
            {
                builder.Append(Out.Div("title", "大小庄游戏设置"));
                builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                builder.Append("大小庄设置|<a href=\"" + Utils.getUrl("spkadminset.aspx?ptype=13&amp;backurl=" + Utils.PostPage(true) + "") + "\">管理员</a>");
                builder.Append(Out.Tab("</div>", ""));

                string strText = "游戏名称:/,游戏口号(可留空):/,游戏Logo(可留空):/,游戏状态:/,开庄最小启动" + ub.Get("SiteBz") + ":/,开庄最大启动" + ub.Get("SiteBz") + ":/,达多少" + ub.Get("SiteBz") + "显示：/,开庄最小启动" + ub.Get("SiteBz2") + ":/,开庄最大启动" + ub.Get("SiteBz2") + ":/,达多少" + ub.Get("SiteBz2") + "显示：/,赔率1赔多少:/,庄胜手续费(%):/,闲胜手续费(%):/,底部Ubb:/,下注防刷(秒):/,机器人:/,最小下注下限" + ub.Get("SiteBz") + ":/,最大下注上限" + ub.Get("SiteBz") + ":/,最小下注下限" + ub.Get("SiteBz2") + ":/,最大下注上限" + ub.Get("SiteBz2") + ":/,";
                string strName = "Name,Notes,Logo,Status,SmallPay,BigPay,vMoney,SmallPay2,BigPay2,vMoney2,Odds,ZTar,XTar,Foot,Expir,IsBot,sCent,bCent,sCent2,bCent2,backurl";
                string strType = "text,text,text,select,num,num,num,num,num,num,num,text,text,textarea,num,select,num,num,num,num,hidden";
                string strValu = "" + xml.dss["BsName"] + "'" + xml.dss["BsNotes"] + "'" + xml.dss["BsLogo"] + "'" + xml.dss["BsStatus"] + "'" + xml.dss["BsSmallPay"] + "'" + xml.dss["BsBigPay"] + "'" + xml.dss["BsvMoney"] + "'" + xml.dss["BsSmallPay2"] + "'" + xml.dss["BsBigPay2"] + "'" + xml.dss["BsvMoney2"] + "'" + xml.dss["BsOdds"] + "'" + xml.dss["BsZTar"] + "'" + xml.dss["BsXTar"] + "'" + xml.dss["BsFoot"] + "'" + xml.dss["BsExpir"] + "'" + xml.dss["BsIsBot"] + "'" + xml.dss["BssCent"] + "'" + xml.dss["BsbCent"] + "'" + xml.dss["BssCent2"] + "'" + xml.dss["BsbCent2"] + "'" + Utils.getPage(0) + "";
                string strEmpt = "false,true,true,0|正常|1|维护,false,false,false,false,false,false,false,false,false,true,false,0|关闭|1|开启,true,true,true,true,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,bigsmallset.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            }
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("game.aspx") + "\">返回上级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }

    }
}
