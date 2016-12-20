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

public partial class Manage_xml_diaryset : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "日记系统设置";
        builder.Append(Out.Tab("", ""));

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/diary.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string Name = Utils.GetRequest("Name", "post", 2, @"^[^\^]{1,20}$", "系统名称限1-20字内");
            string Logo = Utils.GetRequest("Logo", "post", 3, @"^[^\^]{1,200}$", "Logo地址限200字内");
            string Status = Utils.GetRequest("Status", "post", 2, @"^[0-1]$", "系统状态选择出错");
            string TopNum = Utils.GetRequest("TopNum", "post", 2, @"^[1-9]\d*$", "人气日记显示条数填写错误");
            string NewNum = Utils.GetRequest("NewNum", "post", 2, @"^[1-9]\d*$", "最新日记显示条数填写错误");
            string AddNum = Utils.GetRequest("AddNum", "post", 2, @"^[0-9]\d*$", "发表N篇日记填写错误");
            string TitleNum = Utils.GetRequest("TitleNum", "post", 2, @"^[1-9]\d*$", "标题限字数填写错误");
            string ContentNum = Utils.GetRequest("ContentNum", "post", 2, @"^[1-9]\d*$", "内容限字数填写错误");
            string AddExpir = Utils.GetRequest("AddExpir", "post", 2, @"^[1-9]\d*$", "发表防刷秒数填写错误");
            string Expir = Utils.GetRequest("Expir", "post", 2, @"^[0-9]\d*$", "防刷秒数填写错误");
            if (Convert.ToInt32(TitleNum) > 50)
                TitleNum = "50";

            xml.dss["DiaryName"] = Name;
            xml.dss["DiaryLogo"] = Logo;
            xml.dss["DiaryStatus"] = Status;
            xml.dss["DiaryTopNum"] = TopNum;
            xml.dss["DiaryNewNum"] = NewNum;
            xml.dss["DiaryAddNum"] = AddNum;
            xml.dss["DiaryTitleNum"] = TitleNum;
            xml.dss["DiaryContentNum"] = ContentNum;
            xml.dss["DiaryAddExpir"] = AddExpir;
            xml.dss["DiaryExpir"] = Expir;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("日记系统设置", "设置成功，正在返回..", Utils.getUrl("diaryset.aspx"), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "日记系统设置"));

            string strText = "日记名称:/,日记Logo(可留空):/,系统状态:/,人气日记显示条数:/,最新日记显示条数:/,每人每天限发表N篇日记(填0则不限):/,标题限多少字:/,内容限多少字:/,发表日记防刷(秒):/,评论防刷(秒):/";
            string strName = "Name,Logo,Status,TopNum,NewNum,AddNum,TitleNum,ContentNum,AddExpir,Expir";
            string strType = "text,text,select,snum,snum,snum,snum,snum,snum,snum";
            string strValu = "" + xml.dss["DiaryName"] + "'" + xml.dss["DiaryLogo"] + "'" + xml.dss["DiaryStatus"] + "'" + xml.dss["DiaryTopNum"] + "'" + xml.dss["DiaryNewNum"] + "'" + xml.dss["DiaryAddNum"] + "'" + xml.dss["DiaryTitleNum"] + "'" + xml.dss["DiaryContentNum"] + "'" + xml.dss["DiaryAddExpir"] + "'" + xml.dss["DiaryExpir"] + "";
            string strEmpt = "false,true,0|正常|1|维护,false,false,false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定修改|reset,diaryset.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
}

