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

public partial class Manage_game_Default : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "游戏管理中心";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("游戏管理中心");
        builder.Append(Out.Tab("</div>", ""));

        string endName = string.Empty;
        string endUrl = string.Empty;

        #region 后台游戏管理列表 20160121修改

        //管理后台游戏列表，已弃用 20151228
        //string GameName = new BCW.User.Game.GameRole().GameName();
        string GameName = "球彩竞猜,虚拟49选1,半场/单节竞猜,北京赛车PK10,胜负彩,6场半,进球彩,快乐十分,点值抽奖,活跃抽奖,即时比分,百家乐,每日云购,捕鱼达人,新快三,新快三(试玩),快乐扑克3,德州扑克,开心农场,时时彩,大小庄,挖宝竞猜,跑马风云,闯荡人生,好彩一,";
        GameName += "上证指数,疯狂吹牛,大小掷骰,幸运二八,竞拍系统,拾物活动,系统号自动补币,新上证【酷币版】,新上证【金币版】#";
        GameName += "/bbs/guess2/default,six49,/bbs/guessbc/default,PK10,SFC,BQC,jqc,klsf,draw,winners,footballs,bjl,kbyg,bydr,xk3,xk3swset,HP3,dzpk,farm,ssc,bigsmall,dice,horse,dawnlifeManage,hc1,";
        GameName += "stkguess,brag,dxdice,luck28,race,flows,RobotGold,sse.aspx?sseVe=0&amp;,sse.aspx?sseVe=1&amp;";
        string[] gTemp = GameName.Split('#');

        #endregion

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

                builder.Append("" + (i + 1) + ".<a href=\"" + Utils.getUrl(sUrl[i].ToString().Replace("/bbs/guess/default", "../guess/default").Replace("/bbs/guess2/default", "../guess2/default").Replace("/bbs/guessbc/default", "../guessbc/default") + ".aspx") + "\">" + sName[i].ToString() + "</a>");
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
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br /> "));
    }
}
