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

public partial class Manage_xml_freshset : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "滚动设置";
        builder.Append(Out.Tab("", ""));

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/fresh.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-9]\d*$", "0"));
        if (ptype != 0 && ptype <= 7)
        {
            if (Utils.ToSChinese(ac) == "确定修改")
            {
                xml.dss["FreshC" + ptype + ""] = Request["C" + ptype + ""];
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                Utils.Success("滚动设置", "" + CaseType(ptype) + "滚动设置成功，正在返回..", Utils.getUrl("freshset.aspx?ptype=" + ptype + ""), "1");
            }
            else
            {
                builder.Append(Out.Div("title", "滚动设置"));

                string strText = "" + CaseType(ptype) + "滚动:/,";
                string strName = "C" + ptype + ",ptype";
                string strType = "big,hidden";
                string strValu = "";
                if (xml.dss["FreshC" + ptype + ""] != null)
                    strValu = "" + Out.UBB(xml.dss["FreshC" + ptype + ""].ToString()) + "'" + ptype + "";
                else
                    strValu = "'" + ptype + "";

                string strEmpt = "true,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,freshset.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", " "));
                builder.Append("<a href=\"" + Utils.getUrl("freshset.aspx") + "\">取消</a><br />");
                builder.Append("温馨提示:完美支持使用智能UBB配置<br />滚动UBB：[@随机数据=数据1#数据2#数据3]");
                builder.Append(Out.Tab("</div>", ""));

            }
        }
        else if (ptype == 8)
        {
            if (Utils.ToSChinese(ac) == "确定修改")
            {
                xml.dss["FreshGdID"] = Request["GdID"];
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                Utils.Success("滚动设置", "滚动设置成功，正在返回..", Utils.getUrl("freshset.aspx?ptype=" + ptype + ""), "1");
            }
            else
            {
                builder.Append(Out.Div("title", "管理员设置"));
                string strText = "滚动管理ID(用#分开):/,";
                string strName = "GdID,ptype";
                string strType = "big,hidden";
                string strValu = "" + xml.dss["FreshGdID"] + "'" + ptype + "";
                string strEmpt = "true,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,freshset.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", " "));
                builder.Append("<a href=\"" + Utils.getUrl("freshset.aspx") + "\">取消</a>"); ;
                builder.Append(Out.Tab("</div>", ""));
            }

        }
        else
        {
            builder.Append(Out.Div("title", "滚动设置"));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("0.<a href=\"" + Utils.getUrl("freshset.aspx?ptype=8") + "\">管理员设置</a><br />");
            builder.Append("1.<a href=\"" + Utils.getUrl("freshset.aspx?ptype=1") + "\">首页社区</a><br />");
            builder.Append("2.<a href=\"" + Utils.getUrl("freshset.aspx?ptype=2") + "\">论坛首页</a><br />");
            builder.Append("3.<a href=\"" + Utils.getUrl("freshset.aspx?ptype=3") + "\">论坛区域</a><br />");
            builder.Append("4.<a href=\"" + Utils.getUrl("freshset.aspx?ptype=4") + "\">游戏区域</a><br />");
            builder.Append("5.<a href=\"" + Utils.getUrl("freshset.aspx?ptype=5") + "\">日记区域</a><br />");
            builder.Append("6.<a href=\"" + Utils.getUrl("freshset.aspx?ptype=6") + "\">相册区域</a><br />");
            builder.Append("7.<a href=\"" + Utils.getUrl("freshset.aspx?ptype=7") + "\">圈子区域</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private string CaseType(int Types)
    {
        string sText = string.Empty;
        if (Types == 1)
            sText = "首页社区";
        else if (Types == 2)
            sText = "论坛首页";
        else if (Types == 3)
            sText = "论坛区域";
        else if (Types == 4)
            sText = "游戏区域";
        else if (Types == 5)
            sText = "日记区域";
        else if (Types == 6)
            sText = "相册区域";
        else if (Types == 7)
            sText = "圈子区域";

        return sText;
    }
}
