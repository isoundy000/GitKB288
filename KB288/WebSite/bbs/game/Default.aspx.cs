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

public partial class bbs_game_Default : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Utils.GetDomain().Contains("dyj6")) 
        {
            Response.Redirect(Utils.getUrl("http://" + Utils.GetDomain() + "/bbs/game/six49.aspx").Replace("&amp;", "&"));
        }
        else if (Utils.GetDomain().Contains("boyi929"))
        {
            Response.Redirect(Utils.getUrl("http://" + Utils.GetDomain() + "/default.aspx").Replace("&amp;", "&"));
        }
        else
        {
            Response.Redirect(Utils.getUrl("http://" + Utils.GetDomain() + "/default.aspx?id=13667").Replace("&amp;", "&"));
        }

        if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th"))
        {
            Response.Redirect(Utils.getUrl("http://" + Utils.GetDomain() + "/default.aspx?id=13371").Replace("&amp;", "&"));
        }
        else if (Utils.GetTopDomain().Contains("kb288.net"))
        {

        }

        Master.Title = "游戏中心";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("游戏中心");
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
        int pageSize = 20;
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

                builder.Append("<a href=\"" + Utils.getUrl(sUrl[i].ToString() + ".aspx") + "\">" + sName[i].ToString() + "</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            if (k == endIndex)
                break;
            k++;
        }

        // 分页
        builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        if (Utils.GetDomain().Contains("kubao") || Utils.GetDomain().Contains("tuhao") || Utils.GetDomain().Contains("th") || Utils.GetDomain().Contains("kb288"))
        {
            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("../finance.aspx?act=paysafe&amp;backurl=" + Utils.getPage(0) + "") + "\">设置支付安全</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("../default.aspx") + "\">社区</a>-");
        builder.Append("<a href=\"" + Utils.getPage("../finance.aspx") + "\">金融</a>");
        builder.Append(Out.Tab("</div>", ""));
      //-----------友链链入开始
        int kid = int.Parse(Utils.GetRequest("kid", "get", 1, @"^[0-9]\d*$", "0"));
        if (kid != 0)
        {
            if (new BCW.BLL.Link().Exists(kid))
            {
                //统计链入
                string xmlPath = "/Controls/link.xml";
                if (ub.GetSub("LinkIsPc", xmlPath) == "0")
                {
                    if (Utils.IsMobileUa())
                    {
                        new BCW.BLL.Link().UpdateLinkIn(kid);
                        if (ub.GetSub("LinkGoUrl", xmlPath) != "")
                        {
                            Response.Redirect(ub.GetSub("LinkGoUrl", xmlPath));
                        }
                    }
                }
                else
                {
                    new BCW.BLL.Link().UpdateLinkIn(kid);
                    if (ub.GetSub("LinkGoUrl", xmlPath) != "")
                    {
                        Response.Redirect(ub.GetSub("LinkGoUrl", xmlPath));
                    }
                }
            }
        }
        //-----------友链链入结束
    }
}
