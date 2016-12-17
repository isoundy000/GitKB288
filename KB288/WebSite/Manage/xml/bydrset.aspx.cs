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


public partial class Manage_xml_bydrset : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "捕鱼游戏设置";
        builder.Append(Out.Tab("", ""));

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-1]$", "0"));
        string xmlPath = "/Controls/BYDR.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置

        if (Utils.ToSChinese(ac) == "确定修改")
        {
            if (ptype == 0)
            {
                string Name = Utils.GetRequest("Name", "post", 2, @"^[^\^]{1,20}$", "系统名称限1-20字内");
                string Notes = Utils.GetRequest("Notes", "post", 3, @"^[^\^]{1,2000}$", "口号限2000字内");
                string Logo = Utils.GetRequest("Logo", "post", 3, @"^[^\^]{1,200}$", "Logo地址限200字内");
                string Status = Utils.GetRequest("Status", "post", 2, @"^[0-1]$", "系统状态选择出错");
                string buyuprofit = Utils.GetRequest("buyuprofit", "post", 2, @"^(\d)*(\.(\d){0,4})?$", "捕鱼赢利手续费填写错误");
                string Foot = Utils.GetRequest("Foot", "post", 3, @"^[\s\S]{1,2000}$", "底部Ubb限2000字内");
                string buyuTi = Utils.GetRequest("buyuTi", "post", 2, @"^[0-9]\d*$", "体力价值填写错误");
                string Expir = Utils.GetRequest("bydrExpir", "post", 2, @"^[0-9]\d*$", "防刷秒数填写出错");
                string Expir1 = Utils.GetRequest("bydrExpir1", "post", 2, @"^[0-9]\d*$", "防刷秒数填写出错");
                string bydryuju = Utils.GetRequest("bydryuju", "post", 3, @"^[^\^]{1,2000000}$", "钓鱼等待语句限2000000字内");
                string bydrtest = Utils.GetRequest("bydrtest", "post", 2, @"^[0-1]$", "系统状态选择出错");
                string bydrxiaoxi = Utils.GetRequest("bydrxiaoxi", "post", 3, @"^[\s\S]{1,2000}$", "底部Ubb限2000字内");
                xml.dss["bydrName"] = Name;
                xml.dss["bydrNotes"] = Notes;
                xml.dss["bydrLogo"] = Logo;
                xml.dss["bydrStatus"] = Status;
                xml.dss["bydrbuyuprofit"] = buyuprofit;
                xml.dss["bydrFoot"] = Foot;
                xml.dss["bydrbuyuTi"] = buyuTi;
                xml.dss["bydrExpir"] = Expir;
                xml.dss["bydrExpir1"] = Expir1;
                xml.dss["bydryuju"] = bydryuju;
                xml.dss["bydrtest"] = bydrtest;
                xml.dss["bydrxiaoxi"] = bydrxiaoxi;
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                Utils.Success("捕鱼游戏设置", "设置成功，正在返回..", Utils.getUrl("bydrset.aspx?backurl=" + Utils.getPage(0) + ""), "1");
            }
        }
        else
        {
            if (ptype == 0)
            {
                builder.Append(Out.Div("title", "捕鱼游戏设置"));
                string strText = "游戏名称:/,游戏口号(可留空):/,游戏Logo(可留空):/,游戏状态:/,捕鱼盈利手续费千分之几（如输入1）:/,底部Ubb:/,体力价值（1体力=？" + ub.Get("SiteBz") + "）:/,防刷时间:/,防刷时间(加入数据时间):/,钓鱼等待语句（每个句子前面加#号分割）:/,游戏测试开放:/,返负消息编辑:/,";
                string strName = "Name,Notes,Logo,Status,buyuprofit,Foot,buyuTi,bydrExpir,bydrExpir1,bydryuju,bydrtest,bydrxiaoxi,backurl";
                string strType = "text,text,text,select,num,textarea,num,num,num,text,select,text,hidden";
                string strValu = "" + xml.dss["bydrName"] + "'" + xml.dss["bydrNotes"] + "'" + xml.dss["bydrLogo"] + "'" + xml.dss["bydrStatus"] + "'" + xml.dss["bydrbuyuprofit"] + "'" + xml.dss["bydrFoot"] + "'" + xml.dss["bydrbuyuTi"] + "'" + xml.dss["bydrExpir"] + "'" + xml.dss["bydrExpir1"] + "'" + xml.dss["bydryuju"] + "'" + xml.dss["bydrtest"] + "'" + xml.dss["bydrxiaoxi"] + "'" + Utils.getPage(0) + "";
                string strEmpt = "false,true,true,0|正常|1|维护,false,false,false,false,false,false,0|关闭|1|开放,false,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,bydrset.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getPage("../game/bydr.aspx") + "\">返回上级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
}
