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

public partial class Manage_xml_game : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "游戏配置中心";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("游戏配置中心");
        builder.Append(Out.Tab("</div>", ""));

        string endName = string.Empty;
        string endUrl = string.Empty;

        string GameName = new BCW.User.Game.GameRole().GameName();
        string[] gTemp = GameName.Split('#');

        endName = gTemp[0];
        endUrl = gTemp[1];

        string[] sName = endName.Split(",".ToCharArray());
        string[] sUrl = endUrl.Split(",".ToCharArray());

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string[] pageValUrl = { };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //总记录数
        recordCount = sName.Length;

        int stratIndex = (pageIndex - 1) * pageSize;
        int endIndex = pageIndex * pageSize;
        int k = 0;
        for (int i = 0; i < sName.Length; i++)
        {
            if (k >= stratIndex && k < endIndex)
            {
                if ((k + 1) % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                    builder.Append(Out.Tab("<div>", "<br />"));

                builder.Append("" + (i + 1) + ".<a href=\"" + Utils.getUrl(sUrl[i].ToString().Replace("stkguess", "stk").Replace("/bbs/guess/default", "guess") + "set.aspx").Replace("/bbs/guess2/defaultset.aspx", "guessset2.aspx").Replace("/bbs/guessbc/defaultset.aspx", "guessbcset.aspx") + "\">" + sName[i].ToString() + "</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            if (k == endIndex)
                break;
            k++;
        }

        // 分页
        builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br /> "));
    }
}
