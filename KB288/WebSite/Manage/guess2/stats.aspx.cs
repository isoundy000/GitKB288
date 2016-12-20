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

/// <summary>
/// 邵广林 20160817 增加足球机器人
/// </summary>

public partial class Manage_guess2_stats : System.Web.UI.Page
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
            builder.Append("完场数据分析(不含串串、已排除机器人投注)");
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
            todaycsNum = new TPR2.BLL.guess.BaList().GetBaListCount("p_type=" + ptype + " and  Year(p_addtime)=" + DateTime.Now.Year + " AND Month(p_addtime) = " + DateTime.Now.Month + " and Day(p_addtime) = " + DateTime.Now.Day + " and p_active<>0");

            todayzsNum = new TPR2.BLL.guess.BaPay().GetBaPayCount("isrobot=0 and ptype=" + ptype + " and   Year(paytimes) = " + DateTime.Now.Year + " AND Month(paytimes) = " + DateTime.Now.Month + " and Day(paytimes) = " + DateTime.Now.Day + "  and p_active<>0");

            todaywinNum = new TPR2.BLL.guess.BaPay().GetBaPayCount("isrobot=0 and ptype=" + ptype + " and  Year(paytimes) = " + DateTime.Now.Year + " AND Month(paytimes) = " + DateTime.Now.Month + " and Day(paytimes) = " + DateTime.Now.Day + "  and p_getMoney>0 and p_active<>0");

            todayzsCent = new TPR2.BLL.guess.BaPay().GetBaPaypayCent("isrobot=0 and ptype=" + ptype + " and  Year(paytimes) = " + DateTime.Now.Year + " AND Month(paytimes) = " + DateTime.Now.Month + " and Day(paytimes) = " + DateTime.Now.Day + " and p_active<>0");

            todaywinCent = new TPR2.BLL.guess.BaPay().GetBaPaygetMoney("isrobot=0 and ptype=" + ptype + " and  Year(paytimes) = " + DateTime.Now.Year + " AND Month(paytimes) = " + DateTime.Now.Month + " and Day(paytimes) = " + DateTime.Now.Day + "  and p_getMoney>0 and p_active<>0");

            //统计昨天
            int yestcsNum, yestzsNum, yestwinNum;
            long yestzsCent, yestwinCent;
            yestcsNum = new TPR2.BLL.guess.BaList().GetBaListCount("p_type=" + ptype + " and  Year(p_addtime)=" + DateTime.Now.AddDays(-1).Year + " AND Month(p_addtime) = " + DateTime.Now.AddDays(-1).Month + " and Day(p_addtime) = " + DateTime.Now.AddDays(-1).Day + " and p_active<>0");

            yestzsNum = new TPR2.BLL.guess.BaPay().GetBaPayCount("isrobot=0 and ptype=" + ptype + " and   Year(paytimes) = " + DateTime.Now.AddDays(-1).Year + " AND Month(paytimes) = " + DateTime.Now.AddDays(-1).Month + " and Day(paytimes) = " + DateTime.Now.AddDays(-1).Day + "  and p_active<>0");

            yestwinNum = new TPR2.BLL.guess.BaPay().GetBaPayCount("isrobot=0 and ptype=" + ptype + " and  Year(paytimes) = " + DateTime.Now.AddDays(-1).Year + " AND Month(paytimes) = " + DateTime.Now.AddDays(-1).Month + " and Day(paytimes) = " + DateTime.Now.AddDays(-1).Day + "  and p_getMoney>0 and p_active<>0");

            yestzsCent = new TPR2.BLL.guess.BaPay().GetBaPaypayCent("isrobot=0 and ptype=" + ptype + " and  Year(paytimes) = " + DateTime.Now.AddDays(-1).Year + " AND Month(paytimes) = " + DateTime.Now.AddDays(-1).Month + " and Day(paytimes) = " + DateTime.Now.AddDays(-1).Day + " and p_active<>0");

            yestwinCent = new TPR2.BLL.guess.BaPay().GetBaPaygetMoney("isrobot=0 and ptype=" + ptype + " and  Year(paytimes) = " + DateTime.Now.AddDays(-1).Year + " AND Month(paytimes) = " + DateTime.Now.AddDays(-1).Month + " and Day(paytimes) = " + DateTime.Now.AddDays(-1).Day + "  and p_getMoney>0 and p_active<>0");

            //统计本月
            int bmcsNum, bmzsNum, bmwinNum;
            long bmzsCent, bmwinCent;
            bmcsNum = new TPR2.BLL.guess.BaList().GetBaListCount("p_type=" + ptype + " and  Year(p_addtime)=" + (DateTime.Now.Year) + " AND Month(p_addtime) = " + (DateTime.Now.Month) + " and p_active<>0");

            bmzsNum = new TPR2.BLL.guess.BaPay().GetBaPayCount("isrobot=0 and ptype=" + ptype + " and   Year(paytimes) = " + (DateTime.Now.Year) + " AND Month(paytimes) = " + (DateTime.Now.Month) + " and p_active<>0");

            bmwinNum = new TPR2.BLL.guess.BaPay().GetBaPayCount("isrobot=0 and ptype=" + ptype + " and  Year(paytimes) = " + (DateTime.Now.Year) + " AND Month(paytimes) = " + (DateTime.Now.Month) + " and p_getMoney>0 and p_active<>0");

            bmzsCent = new TPR2.BLL.guess.BaPay().GetBaPaypayCent("isrobot=0 and ptype=" + ptype + " and  Year(paytimes) = " + (DateTime.Now.Year) + " AND Month(paytimes) = " + (DateTime.Now.Month) + " and p_active<>0");

            bmwinCent = new TPR2.BLL.guess.BaPay().GetBaPaygetMoney("isrobot=0 and ptype=" + ptype + " and  Year(paytimes) = " + (DateTime.Now.Year) + " AND Month(paytimes) = " + (DateTime.Now.Month) + " and p_getMoney>0 and p_active<>0");

            //统计上月
            int smcsNum, smzsNum, smwinNum;
            long smzsCent, smwinCent;
            smcsNum = new TPR2.BLL.guess.BaList().GetBaListCount("p_type=" + ptype + " and  Year(p_addtime)=" + (DateTime.Now.Year - DateTime.Now.Day) + " AND Month(p_addtime) = " + (DateTime.Now.Month - DateTime.Now.Day) + " and p_active<>0");

            smzsNum = new TPR2.BLL.guess.BaPay().GetBaPayCount("isrobot=0 and ptype=" + ptype + " and   Year(paytimes) = " + (DateTime.Now.Year - DateTime.Now.Day) + " AND Month(paytimes) = " + (DateTime.Now.Month - DateTime.Now.Day) + " and p_active<>0");

            smwinNum = new TPR2.BLL.guess.BaPay().GetBaPayCount("isrobot=0 and ptype=" + ptype + " and  Year(paytimes) = " + (DateTime.Now.Year - DateTime.Now.Day) + " AND Month(paytimes) = " + (DateTime.Now.Month - DateTime.Now.Day) + " and p_getMoney>0 and p_active<>0");

            smzsCent = new TPR2.BLL.guess.BaPay().GetBaPaypayCent("isrobot=0 and ptype=" + ptype + " and  Year(paytimes) = " + (DateTime.Now.Year - DateTime.Now.Day) + " AND Month(paytimes) = " + (DateTime.Now.Month - DateTime.Now.Day) + " and p_active<>0");

            smwinCent = new TPR2.BLL.guess.BaPay().GetBaPaygetMoney("isrobot=0 and ptype=" + ptype + " and  Year(paytimes) = " + (DateTime.Now.Year - DateTime.Now.Day) + " AND Month(paytimes) = " + (DateTime.Now.Month - DateTime.Now.Day) + " and p_getMoney>0 and p_active<>0");

            //总计
            int csNum, zsNum, winNum;
            long zsCent, winCent;
            csNum = new TPR2.BLL.guess.BaList().GetBaListCount("p_type=" + ptype + " and p_active<>0");

            zsNum = new TPR2.BLL.guess.BaPay().GetBaPayCount("isrobot=0 and ptype=" + ptype + " and p_active<>0");

            winNum = new TPR2.BLL.guess.BaPay().GetBaPayCount("isrobot=0 and ptype=" + ptype + " and p_getMoney>0 and p_active<>0");

            zsCent = new TPR2.BLL.guess.BaPay().GetBaPaypayCent("isrobot=0 and ptype=" + ptype + " and p_active<>0");

            winCent = new TPR2.BLL.guess.BaPay().GetBaPaygetMoney("isrobot=0 and ptype=" + ptype + " and p_getMoney>0 and p_active<>0");

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
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.waplink(Utils.getUrl("stats.aspx?showtype=1"), "&gt;串串数据分析"));
            builder.Append(Out.Tab("</div>", ""));
        }
        else if(showtype==1)
        {
            //统计今天
            int todayzsNum2, todaywinNum2;
            long todayzsCent2, todaywinCent2;

            todayzsNum2 = new TPR2.BLL.guess.Super().GetSuperCount("Year(AddTime) = " + DateTime.Now.Year + " AND Month(AddTime) = " + DateTime.Now.Month + " and Day(AddTime) = " + DateTime.Now.Day + "  and Status<>0");

            todaywinNum2 = new TPR2.BLL.guess.Super().GetSuperCount("Year(AddTime) = " + DateTime.Now.Year + " AND Month(AddTime) = " + DateTime.Now.Month + " and Day(AddTime) = " + DateTime.Now.Day + "  and getMoney>0 and Status<>0");

            todayzsCent2 = new TPR2.BLL.guess.Super().GetSuperpayCent("Year(AddTime) = " + DateTime.Now.Year + " AND Month(AddTime) = " + DateTime.Now.Month + " and Day(AddTime) = " + DateTime.Now.Day + " and Status<>0");

            todaywinCent2 = new TPR2.BLL.guess.Super().GetSupergetMoney("Year(AddTime) = " + DateTime.Now.Year + " AND Month(AddTime) = " + DateTime.Now.Month + " and Day(AddTime) = " + DateTime.Now.Day + "  and getMoney>0 and Status<>0");

            //统计昨天
            int yestzsNum2, yestwinNum2;
            long yestzsCent2, yestwinCent2;

            yestzsNum2 = new TPR2.BLL.guess.Super().GetSuperCount("Year(AddTime) = " + DateTime.Now.AddDays(-1).Year + " AND Month(AddTime) = " + DateTime.Now.AddDays(-1).Month + " and Day(AddTime) = " + DateTime.Now.AddDays(-1).Day + "  and Status<>0");

            yestwinNum2 = new TPR2.BLL.guess.Super().GetSuperCount("Year(AddTime) = " + DateTime.Now.AddDays(-1).Year + " AND Month(AddTime) = " + DateTime.Now.AddDays(-1).Month + " and Day(AddTime) = " + DateTime.Now.AddDays(-1).Day + "  and getMoney>0 and Status<>0");

            yestzsCent2 = new TPR2.BLL.guess.Super().GetSuperpayCent("Year(AddTime) = " + DateTime.Now.AddDays(-1).Year + " AND Month(AddTime) = " + DateTime.Now.AddDays(-1).Month + " and Day(AddTime) = " + DateTime.Now.AddDays(-1).Day + " and Status<>0");

            yestwinCent2 = new TPR2.BLL.guess.Super().GetSupergetMoney("Year(AddTime) = " + DateTime.Now.AddDays(-1).Year + " AND Month(AddTime) = " + DateTime.Now.AddDays(-1).Month + " and Day(AddTime) = " + DateTime.Now.AddDays(-1).Day + "  and getMoney>0 and Status<>0");

            //统计本月
            int bmzsNum2, bmwinNum2;
            long bmzsCent2, bmwinCent2;

            bmzsNum2 = new TPR2.BLL.guess.Super().GetSuperCount("Year(AddTime) = " + (DateTime.Now.Year) + " AND Month(AddTime) = " + (DateTime.Now.Month) + " and Status<>0");

            bmwinNum2 = new TPR2.BLL.guess.Super().GetSuperCount("Year(AddTime) = " + (DateTime.Now.Year) + " AND Month(AddTime) = " + (DateTime.Now.Month) + " and getMoney>0 and Status<>0");

            bmzsCent2 = new TPR2.BLL.guess.Super().GetSuperpayCent("Year(AddTime) = " + (DateTime.Now.Year) + " AND Month(AddTime) = " + (DateTime.Now.Month) + " and Status<>0");

            bmwinCent2 = new TPR2.BLL.guess.Super().GetSupergetMoney("Year(AddTime) = " + (DateTime.Now.Year) + " AND Month(AddTime) = " + (DateTime.Now.Month) + " and getMoney>0 and Status<>0");

            //统计上月
            int smzsNum2, smwinNum2;
            long smzsCent2, smwinCent2;

            smzsNum2 = new TPR2.BLL.guess.Super().GetSuperCount("Year(AddTime) = " + (DateTime.Now.Year - DateTime.Now.Day) + " AND Month(AddTime) = " + (DateTime.Now.Month - DateTime.Now.Day) + " and Status<>0");

            smwinNum2 = new TPR2.BLL.guess.Super().GetSuperCount("Year(AddTime) = " + (DateTime.Now.Year - DateTime.Now.Day) + " AND Month(AddTime) = " + (DateTime.Now.Month - DateTime.Now.Day) + " and getMoney>0 and Status<>0");

            smzsCent2 = new TPR2.BLL.guess.Super().GetSuperpayCent("Year(AddTime) = " + (DateTime.Now.Year - DateTime.Now.Day) + " AND Month(AddTime) = " + (DateTime.Now.Month - DateTime.Now.Day) + " and Status<>0");

            smwinCent2 = new TPR2.BLL.guess.Super().GetSupergetMoney("Year(AddTime) = " + (DateTime.Now.Year - DateTime.Now.Day) + " AND Month(AddTime) = " + (DateTime.Now.Month - DateTime.Now.Day) + " and getMoney>0 and Status<>0");

            //总计
            int zsNum2, winNum2;
            long zsCent2, winCent2;

            zsNum2 = new TPR2.BLL.guess.Super().GetSuperCount("Status<>0");

            winNum2 = new TPR2.BLL.guess.Super().GetSuperCount("getMoney>0 and Status<>0");

            zsCent2 = new TPR2.BLL.guess.Super().GetSuperpayCent("Status<>0");

            winCent2 = new TPR2.BLL.guess.Super().GetSupergetMoney("getMoney>0 and Status<>0");

            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("完场串串数据分析");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<table border=\"1\" cellpadding=\"3\" cellspacing=\"0\">");
            builder.Append("<tr>");
            builder.Append("<td>类别</td><td>下注数</td><td>下注额</td><td>用户赢注数</td><td>用户赢金额</td><td>本站盈利</td>");
            builder.Append("</tr>");
            builder.Append("<tr>");
            builder.Append("<td>今天</td><td>" + todayzsNum2 + "</td><td>" + todayzsCent2 + "</td><td>" + todaywinNum2 + "</td><td>" + todaywinCent2 + "</td><td>" + (todayzsCent2 - todaywinCent2) + "</td>");
            builder.Append("</tr>");
            builder.Append("<tr>");
            builder.Append("<td>昨天</td><td>" + yestzsNum2 + "</td><td>" + yestzsCent2 + "</td><td>" + yestwinNum2 + "</td><td>" + yestwinCent2 + "</td><td>" + (yestzsCent2 - yestwinCent2) + "</td>");
            builder.Append("</tr>");
            builder.Append("<tr>");
            builder.Append("<td>本月</td><td>" + bmzsNum2 + "</td><td>" + bmzsCent2 + "</td><td>" + bmwinNum2 + "</td><td>" + bmwinCent2 + "</td><td>" + (bmzsCent2 - bmwinCent2) + "</td>");
            builder.Append("</tr>");
            builder.Append("<tr>");
            builder.Append("<td>上月</td><td>" + smzsNum2 + "</td><td>" + smzsCent2 + "</td><td>" + smwinNum2 + "</td><td>" + smwinCent2 + "</td><td>" + (smzsCent2 - smwinCent2) + "</td>");
            builder.Append("</tr>");
            builder.Append("<tr>");
            builder.Append("<td>总计</td><td>" + zsNum2 + "</td><td>" + zsCent2 + "</td><td>" + winNum2 + "</td><td>" + winCent2 + "</td><td>" + (zsCent2 - winCent2) + "</td>");
            builder.Append("</tr>");
            builder.Append("</table>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.waplink(Utils.getUrl("stats.aspx"), "&gt;普通数据分析"));
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
                ds = new TPR2.BLL.guess.BaPay().GetBaPayList("top 10 bcid,count(bcid) as payCount", "p_active=0 group by bcid", "count(bcid) desc");
            }
            else
            {
                builder.Append("未结束赛事下注额TOP10");
                ds = new TPR2.BLL.guess.BaPay().GetBaPayList("top 10 bcid,sum(payCent) as payCount", "p_active=0 group by bcid", "sum(payCent) desc");
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
