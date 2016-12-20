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

public partial class Manage_xml_freeset : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "猜猜乐游戏设置";
        builder.Append(Out.Tab("", ""));

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/free.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string Name = Utils.GetRequest("Name", "post", 2, @"^[^\^]{1,20}$", "系统名称限1-20字内");
            string Notes = Utils.GetRequest("Notes", "post", 3, @"^[^\^]{1,50}$", "猜猜乐口号限50字内");
            string Logo = Utils.GetRequest("Logo", "post", 3, @"^[^\^]{1,200}$", "Logo地址限200字内");
            string Status = Utils.GetRequest("Status", "post", 2, @"^[0-1]$", "系统状态选择出错");
            string OpenId = Utils.GetRequest("OpenId", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,1000}$", "开庄会员ID请用#分开，可以留空");

            string CpsId = Utils.GetRequest("CpsId", "post", 2, @"^[1-9]\d*$", "接收投诉ID填写错误");
            string Tax = Utils.GetRequest("Tax", "post", 2, @"^[0-9]\d*$", "税率填写错误");
            string SmallPay = Utils.GetRequest("SmallPay", "post", 2, @"^[0-9]\d*$", "单份猜猜价格最小填写错误");
            string BigPay = Utils.GetRequest("BigPay", "post", 2, @"^[0-9]\d*$", "单份猜猜价格最大填写错误");
            string MaxNum = Utils.GetRequest("MaxNum", "post", 2, @"^[0-9]\d*$", "最多选项数填写错误");
            string XsTime = Utils.GetRequest("XsTime", "post", 2, @"^[0-9]\d*$", "猜客限时确认时间填写错误");
            string Rule = Utils.GetRequest("Rule", "post", 3, @"^[^\^]{1,5000}$", "规则最限5000字内");

            xml.dss["FreeName"] = Name;
            xml.dss["FreeNotes"] = Notes;
            xml.dss["FreeLogo"] = Logo;
            xml.dss["FreeStatus"] = Status;
            xml.dss["FreeOpenId"] = OpenId;
            xml.dss["FreeCpsId"] = CpsId;
            xml.dss["FreeTax"] = Tax;
            xml.dss["FreeSmallPay"] = SmallPay;
            xml.dss["FreeBigPay"] = BigPay;
            xml.dss["FreeMaxNum"] = MaxNum;
            xml.dss["FreeXsTime"] = XsTime;
            xml.dss["FreeRule"] = Rule;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("猜猜乐游戏设置", "设置成功，正在返回..", Utils.getUrl("freeset.aspx"), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "猜猜乐游戏设置"));
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("猜猜乐设置|<a href=\"" + Utils.getUrl("spkadminset.aspx?ptype=3&amp;backurl=" + Utils.PostPage(true) + "") + "\">管理员</a>");
            builder.Append(Out.Tab("</div>", ""));
            string strText = "游戏名称:/,游戏口号(可留空):/,游戏Logo(可留空):/,游戏状态:/,设置开庄ID(多个用#分开):/,接受投诉ID:/,系统收税(%):/,单份猜猜价格最小:/,单份猜猜价格最大:/,最多选项数:/,猜客限时确认时间(小时):/,游戏规则(支持UBB):/";
            string strName = "Name,Notes,Logo,Status,OpenId,CpsId,Tax,SmallPay,BigPay,MaxNum,XsTime,Rule";
            string strType = "text,text,text,select,text,num,num,num,num,num,num,textarea";
            string strValu = "" + xml.dss["FreeName"] + "'" + xml.dss["FreeNotes"] + "'" + xml.dss["FreeLogo"] + "'" + xml.dss["FreeStatus"] + "'" + xml.dss["FreeOpenId"] + "'" + xml.dss["FreeCpsId"] + "'" + xml.dss["FreeTax"] + "'" + xml.dss["FreeSmallPay"] + "'" + xml.dss["FreeBigPay"] + "'" + xml.dss["FreeMaxNum"] + "'" + xml.dss["FreeXsTime"] + "'" + xml.dss["FreeRule"] + "";
            string strEmpt = "false,true,true,0|正常|1|维护,true,false,false,false,false,false,false,true";
            string strIdea = "/";
            string strOthe = "确定修改|reset,freeset.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("温馨提示:<br />开庄会员ID多个用#分开,此项留空则人人可以开庄.<br />接收投诉ID拥有改变猜猜开奖结果与删除猜猜的权限.<br />猜客限时确认时间指结束的猜猜,限猜客在规定时间内进行确认,如超出时间则系统自动确认为满意,建议填写48小时<br />");
            builder.Append("<a href=\"" + Utils.getUrl("game.aspx") + "\">游戏配置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
}
