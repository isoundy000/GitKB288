using System;
using System.Text;
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

public partial class Manage_guess3_stats : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "完场数据分析";
 
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[1-2]$", "1"));
        int showtype = Utils.ParseInt(Utils.GetRequest("showtype", "get", 1, @"^[0-3]$", "0"));
        if (showtype == 0)
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("完场数据分析");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            if (ptype == 1)
            {
                builder.Append("当前统计:足球 " + Out.waplink(Utils.getUrl("stats.aspx?ptype=2"), "切换篮球"));
            }
            else
            {
                builder.Append("当前统计:篮球 " + Out.waplink(Utils.getUrl("stats.aspx?ptype=1"), "切换足球"));
            }
            builder.Append(Out.Tab("</div>", "<br />"));
            //统计今天
            int todaycsNum, todayzsNum, todaywinNum;
            long todayzsCent, todaywinCent;
            todaycsNum = new TPR3.BLL.guess.BaList().GetBaListCount("p_type=" + ptype + " and  Year(p_addtime)=" + DateTime.Now.Year + " AND Month(p_addtime) = " + DateTime.Now.Month + " and Day(p_addtime) = " + DateTime.Now.Day + " and p_active<>0");

            todayzsNum = new TPR3.BLL.guess.BaPay().GetBaPayCount("ptype=" + ptype + " and   Year(paytimes) = " + DateTime.Now.Year + " AND Month(paytimes) = " + DateTime.Now.Month + " and Day(paytimes) = " + DateTime.Now.Day + "  and p_active<>0");

            todaywinNum = new TPR3.BLL.guess.BaPay().GetBaPayCount("ptype=" + ptype + " and  Year(paytimes) = " + DateTime.Now.Year + " AND Month(paytimes) = " + DateTime.Now.Month + " and Day(paytimes) = " + DateTime.Now.Day + "  and p_getMoney>0 and p_active<>0");

            todayzsCent = new TPR3.BLL.guess.BaPay().GetBaPaypayCent("ptype=" + ptype + " and  Year(paytimes) = " + DateTime.Now.Year + " AND Month(paytimes) = " + DateTime.Now.Month + " and Day(paytimes) = " + DateTime.Now.Day + " and p_active<>0");

            todaywinCent = new TPR3.BLL.guess.BaPay().GetBaPaygetMoney("ptype=" + ptype + " and  Year(paytimes) = " + DateTime.Now.Year + " AND Month(paytimes) = " + DateTime.Now.Month + " and Day(paytimes) = " + DateTime.Now.Day + "  and p_getMoney>0 and p_active<>0");

            //统计昨天
            int yestcsNum, yestzsNum, yestwinNum;
            long yestzsCent, yestwinCent;
            yestcsNum = new TPR3.BLL.guess.BaList().GetBaListCount("p_type=" + ptype + " and  Year(p_addtime)=" + DateTime.Now.AddDays(-1).Year + " AND Month(p_addtime) = " + DateTime.Now.AddDays(-1).Month + " and Day(p_addtime) = " + DateTime.Now.AddDays(-1).Day + " and p_active<>0");

            yestzsNum = new TPR3.BLL.guess.BaPay().GetBaPayCount("ptype=" + ptype + " and   Year(paytimes) = " + DateTime.Now.AddDays(-1).Year + " AND Month(paytimes) = " + DateTime.Now.AddDays(-1).Month + " and Day(paytimes) = " + DateTime.Now.AddDays(-1).Day + "  and p_active<>0");

            yestwinNum = new TPR3.BLL.guess.BaPay().GetBaPayCount("ptype=" + ptype + " and  Year(paytimes) = " + DateTime.Now.AddDays(-1).Year + " AND Month(paytimes) = " + DateTime.Now.AddDays(-1).Month + " and Day(paytimes) = " + DateTime.Now.AddDays(-1).Day + "  and p_getMoney>0 and p_active<>0");

            yestzsCent = new TPR3.BLL.guess.BaPay().GetBaPaypayCent("ptype=" + ptype + " and  Year(paytimes) = " + DateTime.Now.AddDays(-1).Year + " AND Month(paytimes) = " + DateTime.Now.AddDays(-1).Month + " and Day(paytimes) = " + DateTime.Now.AddDays(-1).Day + " and p_active<>0");

            yestwinCent = new TPR3.BLL.guess.BaPay().GetBaPaygetMoney("ptype=" + ptype + " and  Year(paytimes) = " + DateTime.Now.AddDays(-1).Year + " AND Month(paytimes) = " + DateTime.Now.AddDays(-1).Month + " and Day(paytimes) = " + DateTime.Now.AddDays(-1).Day + "  and p_getMoney>0 and p_active<>0");

            //统计本月
            int bmcsNum, bmzsNum, bmwinNum;
            long bmzsCent, bmwinCent;
            bmcsNum = new TPR3.BLL.guess.BaList().GetBaListCount("p_type=" + ptype + " and  Year(p_addtime)=" + (DateTime.Now.Year) + " AND Month(p_addtime) = " + (DateTime.Now.Month) + " and p_active<>0");

            bmzsNum = new TPR3.BLL.guess.BaPay().GetBaPayCount("ptype=" + ptype + " and   Year(paytimes) = " + (DateTime.Now.Year) + " AND Month(paytimes) = " + (DateTime.Now.Month) + " and p_active<>0");

            bmwinNum = new TPR3.BLL.guess.BaPay().GetBaPayCount("ptype=" + ptype + " and  Year(paytimes) = " + (DateTime.Now.Year) + " AND Month(paytimes) = " + (DateTime.Now.Month) + " and p_getMoney>0 and p_active<>0");

            bmzsCent = new TPR3.BLL.guess.BaPay().GetBaPaypayCent("ptype=" + ptype + " and  Year(paytimes) = " + (DateTime.Now.Year) + " AND Month(paytimes) = " + (DateTime.Now.Month) + " and p_active<>0");

            bmwinCent = new TPR3.BLL.guess.BaPay().GetBaPaygetMoney("ptype=" + ptype + " and  Year(paytimes) = " + (DateTime.Now.Year) + " AND Month(paytimes) = " + (DateTime.Now.Month) + " and p_getMoney>0 and p_active<>0");

            //统计上月
            int smcsNum, smzsNum, smwinNum;
            long smzsCent, smwinCent;
            smcsNum = new TPR3.BLL.guess.BaList().GetBaListCount("p_type=" + ptype + " and  Year(p_addtime)=" + (DateTime.Now.Year - DateTime.Now.Day) + " AND Month(p_addtime) = " + (DateTime.Now.Month - DateTime.Now.Day) + " and p_active<>0");

            smzsNum = new TPR3.BLL.guess.BaPay().GetBaPayCount("ptype=" + ptype + " and   Year(paytimes) = " + (DateTime.Now.Year - DateTime.Now.Day) + " AND Month(paytimes) = " + (DateTime.Now.Month - DateTime.Now.Day) + " and p_active<>0");

            smwinNum = new TPR3.BLL.guess.BaPay().GetBaPayCount("ptype=" + ptype + " and  Year(paytimes) = " + (DateTime.Now.Year - DateTime.Now.Day) + " AND Month(paytimes) = " + (DateTime.Now.Month - DateTime.Now.Day) + " and p_getMoney>0 and p_active<>0");

            smzsCent = new TPR3.BLL.guess.BaPay().GetBaPaypayCent("ptype=" + ptype + " and  Year(paytimes) = " + (DateTime.Now.Year - DateTime.Now.Day) + " AND Month(paytimes) = " + (DateTime.Now.Month - DateTime.Now.Day) + " and p_active<>0");

            smwinCent = new TPR3.BLL.guess.BaPay().GetBaPaygetMoney("ptype=" + ptype + " and  Year(paytimes) = " + (DateTime.Now.Year - DateTime.Now.Day) + " AND Month(paytimes) = " + (DateTime.Now.Month - DateTime.Now.Day) + " and p_getMoney>0 and p_active<>0");

            //总计
            int csNum, zsNum, winNum;
            long zsCent, winCent;
            csNum = new TPR3.BLL.guess.BaList().GetBaListCount("p_type=" + ptype + " and p_active<>0");

            zsNum = new TPR3.BLL.guess.BaPay().GetBaPayCount("ptype=" + ptype + " and p_active<>0");

            winNum = new TPR3.BLL.guess.BaPay().GetBaPayCount("ptype=" + ptype + " and p_getMoney>0 and p_active<>0");

            zsCent = new TPR3.BLL.guess.BaPay().GetBaPaypayCent("ptype=" + ptype + " and p_active<>0");

            winCent = new TPR3.BLL.guess.BaPay().GetBaPaygetMoney("ptype=" + ptype + " and p_getMoney>0 and p_active<>0");

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<table border=\"1\" cellpadding=\"3\" cellspacing=\"0\">");
            builder.Append("<tr>");
            builder.Append("<td>类别</td><td>场次</td><td>下注数</td><td>下注额</td><td>用户赢注数</td><td>用户赢金额</td><td>本站盈利</td>");
            builder.Append("</tr>");
            builder.Append("<tr>");
            builder.Append("<td>今天</td><td>" + todaycsNum + "</td><td>" + todayzsNum + "</td><td>" + todayzsCent + "</td><td>" + todaywinNum + "</td><td>" + todaywinCent + "</td><td>" + (todayzsCent - todaywinCent) + "</td>");
            builder.Append("</tr>");
            builder.Append("<tr>");
            builder.Append("<td>昨天</td><td>" + yestcsNum + "</td><td>" + yestzsNum + "</td><td>" + yestzsCent + "</td><td>" + yestwinNum + "</td><td>" + yestwinCent + "</td><td>" + (yestzsCent - yestwinCent) + "</td>");
            builder.Append("</tr>");
            builder.Append("<tr>");
            builder.Append("<td>本月</td><td>" + bmcsNum + "</td><td>" + bmzsNum + "</td><td>" + bmzsCent + "</td><td>" + bmwinNum + "</td><td>" + bmwinCent + "</td><td>" + (bmzsCent - bmwinCent) + "</td>");
            builder.Append("</tr>");
            builder.Append("<tr>");
            builder.Append("<td>上月</td><td>" + smcsNum + "</td><td>" + smzsNum + "</td><td>" + smzsCent + "</td><td>" + smwinNum + "</td><td>" + smwinCent + "</td><td>" + (smzsCent - smwinCent) + "</td>");
            builder.Append("</tr>");
            builder.Append("<tr>");
            builder.Append("<td>总计</td><td>" + csNum + "</td><td>" + zsNum + "</td><td>" + zsCent + "</td><td>" + winNum + "</td><td>" + winCent + "</td><td>" + (zsCent - winCent) + "</td>");
            builder.Append("</tr>");
            builder.Append("</table>");
            builder.Append(Out.Tab("</div>", ""));
        }
       
        else if (showtype > 1)
        {
            Master.Title = "未场数据分析";
            DataSet ds = null;
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            if (showtype == 2)
            {
                builder.Append("未结束赛事下注数TOP10");
                ds = new TPR3.BLL.guess.BaPay().GetBaPayList("top 10 bcid,count(bcid) as payCount", "p_active=0 group by bcid", "count(bcid) desc");
            }
            else
            {
                builder.Append("未结束赛事下注额TOP10");
                ds = new TPR3.BLL.guess.BaPay().GetBaPayList("top 10 bcid,sum(payCent) as payCount", "p_active=0 group by bcid", "sum(payCent) desc");
            }
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<table border=\"1\" cellpadding=\"3\" cellspacing=\"0\">");
            builder.Append("<tr>");
            builder.Append("<td>名次</td><td>1</td><td>2</td><td>3</td><td>4</td><td>5</td><td>6</td><td>7</td><td>8</td><td>9</td><td>10</td>");
            builder.Append("</tr>");
            builder.Append("<tr>");
            builder.Append("<td>赛事ID</td>");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                builder.Append("<td>" + Out.waplink(Utils.getUrl("ShowGuess.aspx?gid=" + ds.Tables[0].Rows[i]["bcid"].ToString()), ds.Tables[0].Rows[i]["bcid"].ToString()) + "</td>");
            }
            if (ds.Tables[0].Rows.Count < 10)
            {
                for (int i = 0; i < 10 - ds.Tables[0].Rows.Count; i++)
                {
                    builder.Append("<td>0</td>");
                }
            }
            builder.Append("</tr>");
            builder.Append("<tr>");
            if (showtype == 2)
                builder.Append("<td>下注数</td>");
            else
                builder.Append("<td>下注额</td>");

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                builder.Append("<td>" + Convert.ToDouble(ds.Tables[0].Rows[i]["payCount"]) + "</td>");
            }
            if (ds.Tables[0].Rows.Count < 10)
            {
                for (int i = 0; i < 10 - ds.Tables[0].Rows.Count; i++)
                {
                    builder.Append("<td>0</td>");
                }
            }
            builder.Append("</tr>");
            builder.Append("</table>");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div>", ""));
            if (showtype == 2)
            {
                builder.Append(Out.waplink(Utils.getUrl("stats.aspx?showtype=3"), "&gt;未结束赛事下注额TOP10"));
            }
            else
            {
                builder.Append(Out.waplink(Utils.getUrl("stats.aspx?showtype=2"), "&gt;未结束赛事下注数TOP10"));
            }
            builder.Append(Out.Tab("</div>", ""));
        
        }

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.waplink(Utils.getUrl("default.aspx"), "返回上一级"));
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
}
