using System;
using System.Collections.Generic;
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

public partial class Manage_channel : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    private string M_Str_mindate = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "add":
                AddPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }
    private void ReloadPage()
    {
        Master.Title = "快速添加";
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-9]\d*$", "1"));
        int leibie = int.Parse(Utils.GetRequest("leibie", "get", 1, @"^[0-2]$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("请选择菜单类型");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("类型:");
        if (leibie == 0)
            builder.Append("首页|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("channel.aspx?leibie=0&amp;ptype=" + ptype + "") + "\">首页</a>|");

        if (leibie == 1)
            builder.Append("社区|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("channel.aspx?leibie=1&amp;ptype=" + ptype + "") + "\">社区</a>|");

        if (leibie == 2)
            builder.Append("论坛");
        else
            builder.Append("<a href=\"" + Utils.getUrl("channel.aspx?leibie=2&amp;ptype=" + ptype + "") + "\">论坛</a>");

        builder.Append(Out.Tab("</div>", "<br />~~~~~~"));
        int pageIndex;
        int recordCount;
        //int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        int pageSize = 20;
        string strWhere = "";
        string[] pageValUrl = { "leibie", "ptype" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "Types=" + ptype + " and Leibie=" + leibie + "";

        // 开始读取专题
        IList<BCW.Model.Topics> listTopics = new BCW.BLL.Topics().GetTopicss(pageIndex, pageSize, strWhere, out recordCount);
        if (listTopics.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Topics n in listTopics)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                    builder.Append(Out.Tab("<div>", "<br />"));

                string IsHidden = string.Empty;
                if (n.Hidden == 1)
                    IsHidden = "＾";

                builder.AppendFormat("<a href=\"" + Utils.getUrl("class.aspx?act=view&amp;id={0}") + "\">[" + BCW.User.AppCase.CaseTopics(n.Types) + "]&gt;" + IsHidden + "</a>{1}.{2}", n.ID, n.Paixu, n.Title);

                k++;
                builder.Append(Out.Tab("</div>", ""));

            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Tab("", "<br />"));
            builder.Append(Out.Div("text", "没有菜单记录"));
        }

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void AddPage()
    { 
    
    }
}
