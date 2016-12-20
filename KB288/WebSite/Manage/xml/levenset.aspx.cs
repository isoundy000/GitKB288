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

public partial class Manage_xml_levenset : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "等级设置";
        builder.Append(Out.Tab("", ""));

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/leven.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定")
        {
            int j = int.Parse(Utils.GetRequest("i", "post", 2, @"^[0-9]\d*$", "操作出错"));
            for (int i = j + 1; i <= (j + 10); i++)
            {
                if (!Utils.IsRegex(Request["L" + i + ""], @"^[0-9]\d*$"))
                {
                    Utils.Error("积分填写出错", "");
                }
                if (!Utils.IsRegex(Request["R" + i + ""], @"^[0-9]\d*$"))
                {
                    Utils.Error("体力填写出错", "");
                }
                xml.dss["LevenL" + i + ""] = Request["L" + i + ""];
                xml.dss["LevenR" + i + ""] = Request["R" + i + ""];
            }

            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("等级设置", "设置成功，正在返回..", Utils.getUrl("levenset.aspx?page=" + (j / 10 + 1) + ""), "1");
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("等级升级花费设置");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("级别->积分->体力");
            builder.Append(Out.Tab("</div>", "<br />"));

            int pageIndex;
            int recordCount;
            int pageSize = 10;
            string[] pageValUrl = {  };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            //总记录数
            recordCount = 100;
            int i = (pageIndex - 1) * 10;
            if (i > 90)
                i = 90;
            string strText = "等级" + (1 + i) + ",-,/等级" + (2 + i) + ",-,/等级" + (3 + i) + ",-,/等级" + (4 + i) + ",-,/等级" + (5 + i) + ",-,/等级" + (6 + i) + ",-,/等级" + (7 + i) + ",-,/等级" + (8 + i) + ",-,/等级" + (9 + i) + ",-,/等级" + (10 + i) + ",,";
            string strName = "L" + (1 + i) + ",R" + (1 + i) + ",L" + (2 + i) + ",R" + (2 + i) + ",L" + (3 + i) + ",R" + (3 + i) + ",L" + (4 + i) + ",R" + (4 + i) + ",L" + (5 + i) + ",R" + (5 + i) + ",L" + (6 + i) + ",R" + (6 + i) + ",L" + (7 + i) + ",R" + (7 + i) + ",L" + (8 + i) + ",R" + (8 + i) + ",L" + (9 + i) + ",R" + (9 + i) + ",L" + (10 + i) + ",R" + (10 + i) + ",i";
            string strType = "stext,stext,stext,stext,stext,stext,stext,stext,stext,stext,stext,stext,stext,stext,stext,stext,stext,stext,stext,stext,hidden";
            string strValu = "" + xml.dss["LevenL" + (1 + i) + ""] + "'" + xml.dss["LevenR" + (1 + i) + ""] + "'" + xml.dss["LevenL" + (2 + i) + ""] + "'" + xml.dss["LevenR" + (2 + i) + ""] + "'" + xml.dss["LevenL" + (3 + i) + ""] + "'" + xml.dss["LevenR" + (3 + i) + ""] + "'" + xml.dss["LevenL" + (4 + i) + ""] + "'" + xml.dss["LevenR" + (4 + i) + ""] + "'" + xml.dss["LevenL" + (5 + i) + ""] + "'" + xml.dss["LevenR" + (5 + i) + ""] + "'" + xml.dss["LevenL" + (6 + i) + ""] + "'" + xml.dss["LevenR" + (6 + i) + ""] + "'" + xml.dss["LevenL" + (7 + i) + ""] + "'" + xml.dss["LevenR" + (7 + i) + ""] + "'" + xml.dss["LevenL" + (8 + i) + ""] + "'" + xml.dss["LevenR" + (8 + i) + ""] + "'" + xml.dss["LevenL" + (9 + i) + ""] + "'" + xml.dss["LevenR" + (9 + i) + ""] + "'" + xml.dss["LevenL" + (10 + i) + ""] + "'" + xml.dss["LevenR" + (10 + i) + ""] + "'" + i + "";
            string strEmpt = "false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定,levenset.aspx,post,3,red";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("提示:无限等级但目前提供100级设置,请分页设置升级至某等级所花费的积分和体力.");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
}