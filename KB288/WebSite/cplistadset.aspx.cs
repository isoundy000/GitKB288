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

public partial class cplistadset : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "免费料广告设置";
        builder.Append(Out.Tab("", ""));

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/cplist.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string ListTop = Utils.GetRequest("ListTop", "post", 1, @"^[\s\S^]{1,5000}$", "");
            string ListFoot = Utils.GetRequest("ListFoot", "post", 1, @"^[\s\S^]{1,5000}$", "");
            string DetailTop = Utils.GetRequest("DetailTop", "post", 1, @"^[\s\S^]{1,5000}$", "");
            string DetailFoot = Utils.GetRequest("DetailFoot", "post", 1, @"^[\s\S^]{1,5000}$", "");
            string BmTop = Utils.GetRequest("BmTop", "post", 1, @"^[\s\S^]{1,5000}$", "");
            string BmFoot = Utils.GetRequest("BmFoot", "post", 1, @"^[\s\S^]{1,5000}$", "");

            xml.dss["CPListTop"] = ListTop;
            xml.dss["CPListFoot"] = ListFoot;
            xml.dss["CPDetailTop"] = DetailTop;
            xml.dss["CPDetailFoot"] = DetailFoot;
            xml.dss["CPBmTop"] = BmTop;
            xml.dss["CPBmFoot"] = BmFoot;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("设置", "设置成功，正在返回..", Utils.getUrl("cplistadset.aspx"), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "免费料广告设置"));

            string strText = "列表顶部Ubb:/,列表底部Ubb:/,内容顶部Ubb:/,内容底部Ubb:/,报码顶部Ubb:/,报码底部Ubb:/";
            string strName = "ListTop,ListFoot,DetailTop,DetailFoot,BmTop,BmFoot";
            string strType = "text,text,text,text,text,text";
            string strValu = "" + xml.dss["CPListTop"] + "'" + xml.dss["CPListFoot"] + "'" + xml.dss["CPDetailTop"] + "'" + xml.dss["CPDetailFoot"] + "'" + xml.dss["CPBmTop"] + "'" + xml.dss["CPBmFoot"] + "";
            string strEmpt = "true,true,true,true,true,true";
            string strIdea = "/";
            string strOthe = "确定修改|reset,cplistadset.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

       
        }
    }
}
