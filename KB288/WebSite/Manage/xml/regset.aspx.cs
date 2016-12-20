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

public partial class Manage_xml_regset : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "注册系统设置";
        builder.Append(Out.Tab("", ""));

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/reg.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string Leibie = Utils.GetRequest("Leibie", "post", 2, @"^[0-3]$", "注册类型选择错误");
            string EmailPost = Utils.GetRequest("EmailPost", "post", 2, @"^[0-1]$", "邮件验证选择错误");
            string Types = Utils.GetRequest("Types", "post", 2, @"^[0-1]$", "注册分配ID选择错误");
            int StartID = int.Parse(Utils.GetRequest("StartID", "post", 2, @"^[1-9]\d*$", "注册起始ID填写错误"));
            string Name = Utils.GetRequest("Name", "post", 2, @"^[\s\S]{1,10}$", "新会员昵称限1-10字");
            string KeepID = Utils.GetRequest("KeepID", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "保留ID配置填写错误");
            string KeepName = Utils.GetRequest("KeepName", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "保留昵称填写错误");
            string KeepChar = Utils.GetRequest("KeepChar", "post", 3, @"^[\s\S]{1,500}$", "昵称允许修改的特殊字符限1-500字符，可留空");
            string Expir = Utils.GetRequest("Expir", "post", 2, @"^[0-9]\d*$", "防刷时间填写错误");
            string NameSame = Utils.GetRequest("NameSame", "post", 2, @"^[0-1]$", "昵称是否允许相同选择错误");
            string EditSex = Utils.GetRequest("EditSex", "post", 2, @"^[0-1]$", "修改性别设置选择错误");
            string Guest = Utils.GetRequest("Guest", "post", 2, @"^[\s\S]{1,1000}$", "注册发送内线内容限1-1000字");
            string Agreement = Utils.GetRequest("Agreement", "post", 3, @"^[\s\S]{1,5000}$", "注册协议限1-5000字符，可留空");
            string SmsContent = Utils.GetRequest("SmsContent", "post", 3, @"^[\s\S]{1,2000}$", "短信注册内容限1-2000字符，可留空");
            if (StartID > 99999)
            {
                Utils.Error("起始ID最大99999", "");
            }
            xml.dss["RegLeibie"] = Leibie;
            xml.dss["RegEmailPost"] = EmailPost;
            xml.dss["RegTypes"] = Types;
            xml.dss["RegStartID"] = StartID;
            xml.dss["RegName"] = Name;
            xml.dss["RegKeepID"] = KeepID;
            xml.dss["RegKeepName"] = KeepName;
            xml.dss["RegKeepChar"] = KeepChar;
            xml.dss["RegExpir"] = Expir;
            xml.dss["RegNameSame"] = NameSame;
            xml.dss["RegEditSex"] = EditSex;
            xml.dss["RegGuest"] = Guest;
            xml.dss["RegAgreement"] = Agreement;
            xml.dss["RegSmsContent"] = SmsContent;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("注册系统设置", "设置成功，正在返回..", Utils.getUrl("regset.aspx"), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "注册系统设置"));

            string strText = "注册类型:/,是否启用邮件验证:/,注册分配ID:/,注册起始ID:/,新会员昵称:/,保留ID(多个用#分开):/,保留昵称(多个用#分开):/,昵称允许修改的特殊字符:/,昵称是否允许相同:/,修改性别设置:/,手工注册防刷(秒):/,注册后发送内线内容(支持Ubb):/,注册协议(支持Ubb):/,短信注册内容(支持Ubb):/";
            string strName = "Leibie,EmailPost,Types,StartID,Name,KeepID,KeepName,KeepChar,NameSame,EditSex,Expir,Guest,Agreement,SmsContent";
            string strType = "select,select,select,num,text,textarea,textarea,textarea,select,select,num,textarea,textarea,textarea";
            string strValu = "" + xml.dss["RegLeibie"] + "'" + xml.dss["RegEmailPost"] + "'" + xml.dss["RegTypes"] + "'" + xml.dss["RegStartID"] + "'" + xml.dss["RegName"] + "'" + xml.dss["RegKeepID"] + "'" + xml.dss["RegKeepName"] + "'" + xml.dss["RegKeepChar"] + "'" + xml.dss["RegNameSame"] + "'" + xml.dss["RegEditSex"] + "'" + xml.dss["RegExpir"] + "'" + xml.dss["RegGuest"] + "'" + xml.dss["RegAgreement"] + "'" + xml.dss["RegSmsContent"] + "";
            string strEmpt = "0|短信注册|1|手工注册|2|全部开放|3|全部关闭,0|不启用|1|启用,0|随机|1|递增,false,false,true,true,true,0|允许相同|1|禁止相同,0|允许修改一次|1|不限制次数,false,false,true,true";
            string strIdea = "/";
            string strOthe = "确定修改|reset,regset.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示：<br />邮件验证只适用于手工注册<br />当启用时,注册需填写邮箱,成功后会员可登录邮箱得到注册信息.<br />注册起始ID限1-99999<br />保留ID填写例子:1111#2222<br />保留昵称填写例子:管理员#总版<br />昵称允许特殊字符填写为空时则不限制.");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
}
