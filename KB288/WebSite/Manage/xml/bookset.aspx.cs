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

public partial class Manage_xml_bookset : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "书城系统设置";
        builder.Append(Out.Tab("", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-2]$", "0"));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/book.xml";
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
                string AdminIDS = Utils.GetRequest("AdminIDS", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "前台审核ID请用#分开，可以留空");
                string AdminIDS2 = Utils.GetRequest("AdminIDS2", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "前台管理员ID请用#分开，可以留空");
                string IsShuMu = Utils.GetRequest("IsShuMu", "post", 2, @"^[0-1]$", "书本审核选择出错");
                string IsContents = Utils.GetRequest("IsContents", "post", 2, @"^[0-1]$", "章节审核选择出错");
                string IsShuMu2 = Utils.GetRequest("IsShuMu2", "post", 2, @"^[0-1]$", "书本审核选择出错");
                string IsContents2 = Utils.GetRequest("IsContents2", "post", 2, @"^[0-1]$", "章节审核选择出错");
                string PwNum = Utils.GetRequest("PwNum", "post", 2, @"^[0-9]\d*$", "书页字数填写出错");
                string Expir = Utils.GetRequest("Expir", "post", 2, @"^[0-9]\d*$", "防刷秒数填写错误");
                xml.dss["BookName"] = Name;
                xml.dss["BookNotes"] = Notes;
                xml.dss["BookLogo"] = Logo;
                xml.dss["BookStatus"] = Status;
                xml.dss["BookAdminIDS"] = AdminIDS;
                xml.dss["BookAdminIDS2"] = AdminIDS2;
                xml.dss["BookIsShuMu"] = IsShuMu;
                xml.dss["BookIsContents"] = IsContents;
                xml.dss["BookIsShuMu2"] = IsShuMu2;
                xml.dss["BookIsContents2"] = IsContents2;
                xml.dss["BookPwNum"] = PwNum;
                xml.dss["BookExpir"] = Expir;
            }
            else if(ptype==1)
            {
                string SystemUbb = Utils.GetRequest("SystemUbb", "post", 3, @"^[\s\S]{1,10000}$", "首页排版Ubb限10000字内");
                xml.dss["BookSystemUbb"] = SystemUbb;
            }
            else
            {
                string SystemUbb2 = Utils.GetRequest("SystemUbb2", "post", 3, @"^[\s\S]{1,10000}$", "分类排版Ubb限10000字内");
                xml.dss["BookSystemUbb2"] = SystemUbb2;
            }
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("书城系统设置", "设置成功，正在返回..", Utils.getUrl("bookset.aspx?ptype=" + ptype + ""), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "书城系统设置"));
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            if (ptype == 0)
            {
                builder.Append("基本设置|");
                builder.Append("<a href=\"" + Utils.getUrl("bookset.aspx?ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">首页排版</a>|");
                builder.Append("<a href=\"" + Utils.getUrl("bookset.aspx?ptype=2&amp;backurl=" + Utils.getPage(0) + "") + "\">分类排版</a>");
            }
            else if (ptype == 1)
            {
                builder.Append("<a href=\"" + Utils.getUrl("bookset.aspx?ptype=0&amp;backurl=" + Utils.getPage(0) + "") + "\">基本设置</a>");
                builder.Append("|首页排版");
                builder.Append("<a href=\"" + Utils.getUrl("bookset.aspx?ptype=2&amp;backurl=" + Utils.getPage(0) + "") + "\">分类排版</a>");

            }
            else
            {
                builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("bookset.aspx?ptype=0&amp;backurl=" + Utils.getPage(0) + "") + "\">基本设置</a>");
                builder.Append("<a href=\"" + Utils.getUrl("bookset.aspx?ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">首页排版</a>|");
                builder.Append("|分类排版");
            }
            builder.Append(Out.Tab("</div>", ""));
            if (ptype == 0)
            {
                string strText = "书城名称:/,书城口号(可留空):/,书城Logo(可留空):/,书城状态:/,前台审核ID(多个用#分开):/,前台管理员ID(多个用#分开):/,添加书本需要审核:/,添加章节需要审核:/,编辑书本需要审核:/,编辑章节需要审核:/,书页默认字数:/,书评防刷秒数(秒):/,";
                string strName = "Name,Notes,Logo,Status,AdminIDS,AdminIDS2,IsShuMu,IsContents,IsShuMu2,IsContents2,PwNum,Expir,backurl";
                string strType = "text,text,text,select,textarea,textarea,select,select,select,select,num,num,hidden";
                string strValu = "" + xml.dss["BookName"] + "'" + xml.dss["BookNotes"] + "'" + xml.dss["BookLogo"] + "'" + xml.dss["BookStatus"] + "'" + xml.dss["BookAdminIDS"] + "'" + xml.dss["BookAdminIDS2"] + "'" + xml.dss["BookIsShuMu"] + "'" + xml.dss["BookIsContents"] + "'" + xml.dss["BookIsShuMu2"] + "'" + xml.dss["BookIsContents2"] + "'" + xml.dss["BookPwNum"] + "'" + xml.dss["BookExpir"] + "'" + Utils.getPage(0) + "";
                string strEmpt = "false,true,true,0|正常|1|维护,true,true,0|需要审核|1|不用审核,0|需要审核|1|不用审核,0|需要审核|1|不用审核,0|需要审核|1|不用审核,false,false,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,bookset.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
            else if (ptype == 1)
            {
                builder.Append(Out.Tab("</div>", ""));
                string strText = "首页UBB排版:/,,";
                string strName = "SystemUbb,ptype,backurl";
                string strType = "big,hidden,hidden";
                string strValu = "" + xml.dss["BookSystemUbb"] + "'"+ptype+"'" + Utils.getPage(0) + "";
                string strEmpt = "true,false,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,bookset.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
            else if (ptype == 2)
            {
                builder.Append(Out.Tab("</div>", ""));
                string strText = "分类排版(留空则默认系统排版):/,,";
                string strName = "SystemUbb2,ptype,backurl";
                string strType = "big,hidden,hidden";
                string strValu = "" + xml.dss["BookSystemUbb2"] + "'" + ptype + "'" + Utils.getPage(0) + "";
                string strEmpt = "true,false,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,bookset.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("../book.aspx") + "\">书城管理</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
}
