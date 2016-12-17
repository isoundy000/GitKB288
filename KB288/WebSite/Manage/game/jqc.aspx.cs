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
using System.Collections.Generic;
using BCW.Common;
using System.Text.RegularExpressions;

public partial class Manage_game_jqc : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/jqc.xml";
    protected string GameName = ub.GetSub("GameName", "/Controls/jqc.xml");//游戏名字
    protected string ROBOTID = (ub.GetSub("ROBOTID", "/Controls/jqc.xml"));//机器人ID
    protected string testID = (ub.GetSub("testID", "/Controls/jqc.xml"));//测试ID
    protected long jiangchi_guding = Convert.ToInt64(ub.GetSub("jiangchi_guding", "/Controls/jqc.xml"));//xml奖池最大值
    protected long jianchi_zuidi = Convert.ToInt64(ub.GetSub("jianchi_zuidi", "/Controls/jqc.xml"));//奖池最低回收
    protected long jiangchi = Convert.ToInt64(ub.GetSub("jiangchi", "/Controls/jqc.xml"));//奖池金额
    protected long jqcOne = Convert.ToInt64(ub.GetSub("jqcOne", "/Controls/jqc.xml"));//1等奖
    protected long jqcTwo = Convert.ToInt64(ub.GetSub("jqcTwo", "/Controls/jqc.xml"));//2等奖
    protected long jqcThree = Convert.ToInt64(ub.GetSub("jqcThree", "/Controls/jqc.xml"));//3等奖
    protected long jqcFour = Convert.ToInt64(ub.GetSub("jqcFour", "/Controls/jqc.xml"));//4等奖
    protected long huishou = Convert.ToInt64(ub.GetSub("huishou", "/Controls/jqc.xml"));//系统回收金额


    ub xml = new ub();
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "view":
                ViewPage();//某期投注信息
                break;
            case "guanli_qihao":
                guanli_qihaoPage();//管理每期信息
                break;
            case "editsave":
                editsavePage();//修改信息
                break;
            case "del":
                DelPage();//删除某期投注记录
                break;
            case "del_sj":
                del_sjPage();//删除一条开奖数据
                break;
            case "reset":
                ResetPage();//重置
                break;
            case "jiangchi":
                jiangchiPage();//奖池
                break;
            case "stat":
                StatPage();//赢利分析
                break;
            case "peizhi":
                PeizhiPage();//游戏配置
                break;
            case "weihu":
                WeihuPage();//游戏维护
                break;
            case "back":
                BackPage();//返赢返负
                break;
            case "backsave":
            case "backsave2":
                BackSavePage(act);
                break;
            case "backsave3":
            case "backsave5":
                BackSavePage2(act);
                break;
            case "paihang":
                PaihangPage();//用户排行
                break;
            case "fenxi":
                FenxiPage();//购买情况和获奖情况
                break;
            case "Top_add":
                Top_addPage();//添加开奖数据
                break;
            case "add_bisai":
                add_bisaiPage();//添加比赛
                break;
            case "ReWard":
                ReWard();//排行榜奖励发放--界面
                break;
            case "ReWardCase":
                ReWardCase();//排行榜奖励发放--操作
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        Master.Title = "" + GameName + "";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));

        string phase = (Utils.GetRequest("phase", "all", 1, @"^[0-9]\d*$", ""));
        string strText = "输入开奖期号：例如(2016088)/,";
        string strName = "phase";
        string strType = "num";
        string strValu = phase;
        string strEmpt = "";
        string strIdea = "";
        string strOthe = "搜开奖记录,jqc.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        //builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        //builder.Append("<h style=\"color:blue\">" + GameName + "开奖信息：</h>");
        builder.Append(Out.Tab("<div></div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
        string strWhere = string.Empty;
        if (phase != "")
        {
            strWhere = "phase=" + phase + "";
        }
        string[] pageValUrl = { "act", "phase", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.JQC.Model.JQC_Internet> listXK3 = new BCW.JQC.BLL.JQC_Internet().GetJQC_Internets(pageIndex, pageSize, strWhere, out recordCount);
        if (listXK3.Count > 0)
        {
            int k = 1;
            foreach (BCW.JQC.Model.JQC_Internet n in listXK3)
            {
                if (k % 2 == 0)
                {
                    builder.Append(Out.Tab("<div>", "<br />"));
                }
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }
                BCW.JQC.Model.JQC_Bet model_get = new BCW.JQC.BLL.JQC_Bet().Get_tounum(n.phase);
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("jqc.aspx?act=guanli_qihao&amp;id=" + n.ID + "") + "\">[管理]></a>" + n.phase + "期");
                if (n.Result != "")
                {
                    builder.Append(".[" + n.Result + "].");
                    if (model_get.a > 0)
                    {
                        builder.Append("(<h style=\"color:red\"><a href=\"" + Utils.getUrl("jqc.aspx?act=view&amp;id=" + n.phase + "") + "\">" + model_get.a + "</a></h>条.奖池:<a href=\"" + Utils.getUrl("jqc.aspx?act=jiangchi&amp;phase=" + n.phase + "") + "\">" + n.nowprize + "</a>)");
                    }
                    else
                    {
                        builder.Append("(<a href=\"" + Utils.getUrl("jqc.aspx?act=view&amp;id=" + n.phase + "") + "\">0</a>条.奖池:<a href=\"" + Utils.getUrl("jqc.aspx?act=jiangchi&amp;phase=" + n.phase + "") + "\">" + n.nowprize + "</a>)");
                    }
                }
                else
                {
                    builder.Append("|<a href=\"" + Utils.getUrl("jqc.aspx?act=Top_add&amp;phase=" + n.phase + "") + "\">开奖</a>");
                    builder.Append("|<a href=\"" + Utils.getUrl("jqc.aspx?act=jiangchi&amp;phase=" + n.phase + "") + "\">奖池</a>");
                }
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("【管理操作】<br/>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=jiangchi") + "\">奖池记录</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=stat") + "\">赢利分析</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=paihang") + "\">用户排行</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=fenxi") + "\">投注记录</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=add_bisai") + "\">添加开奖</a><br/>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("【系统操作】<br/>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=peizhi") + "\">游戏配置</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=weihu") + "\">游戏维护</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=back") + "\">返赢返负</a>");
        builder.Append(Out.Tab("</div>", Out.Hr()));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //奖池
    private void jiangchiPage()
    {
        Master.Title = "" + GameName + "_奖池";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx") + "\">" + GameName + "</a>&gt;奖池记录");
        builder.Append(Out.Tab("</div>", "<br/>"));

        builder.Append(Out.Tab("<div>", ""));
        string phase = (Utils.GetRequest("phase", "all", 1, @"^[0-9]\d*$", "0"));
        if (phase == "0")
        {
            //如果当前没有开售：即phase=0
            phase = (new BCW.JQC.BLL.JQC_Internet().Get_oldID()).ToString();
            if (phase == "0")
            {
                phase = (new BCW.JQC.BLL.JQC_Internet().Get_oldID2()).ToString();//查询最近一期已开奖数据
            }
        }
        builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=jiangchi&amp;phase=" + (int.Parse(phase) + 1) + "") + "\">上一期</a> | ");
        builder.Append("" + phase + " | ");
        builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=jiangchi&amp;phase=" + (int.Parse(phase) - 1) + "") + "\">下一期</a>");
        builder.Append(Out.Tab("</div>", "<br/>"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
        string strWhere = string.Empty;
        string strOrder = "AddTime desc";
        string[] pageValUrl = { "act", "phase", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        strWhere = "phase=" + phase + "";

        // 开始读取列表
        IList<BCW.JQC.Model.JQC_Jackpot> listjqcpay = new BCW.JQC.BLL.JQC_Jackpot().GetJQC_Jackpots(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listjqcpay.Count > 0)
        {
            int k = 1;
            foreach (BCW.JQC.Model.JQC_Jackpot n in listjqcpay)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div>", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".");
                if (n.UsID != 10086)
                    builder.Append("<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(n.UsID) + "(" + n.UsID + ")</a>");
                else
                    builder.Append("<b style=\"color:red\">系统</b>");
                string ah = string.Empty;
                string bi = string.Empty;
                if (n.type == 0)
                {
                    ah = "在第" + n.phase + "期下注";
                    bi = n.InPrize + ub.Get("SiteBz");
                }
                if (n.type == 1)
                {
                    ah = "在第" + n.phase + "期派奖";
                    bi = n.OutPrize + ub.Get("SiteBz");
                }
                if (n.type == 2)
                {
                    ah = "在第" + n.phase + "期过期回收";
                    bi = n.InPrize + ub.Get("SiteBz");
                }
                if (n.type == 3)
                {
                    ah = "在第" + n.phase + "期系统扣税";
                    bi = n.OutPrize + ub.Get("SiteBz");
                }
                if (n.type == 4)
                {
                    if (n.InPrize > 0)
                    {
                        ah = "在第" + n.phase + "期系统增加";
                        bi = n.InPrize + ub.Get("SiteBz");
                    }
                    if (n.OutPrize > 0)
                    {
                        ah = "在第" + n.phase + "期系统回收";
                        bi = n.OutPrize + ub.Get("SiteBz");
                    }
                }
                if (n.type == 5)
                {
                    if (n.InPrize > 0)
                    {
                        bi = n.InPrize + ub.Get("SiteBz") + "到" + n.phase + "期";
                        ah = "在第" + (n.phase - 1) + "期滚存";
                    }
                    if (n.OutPrize > 0)
                    {
                        bi = n.OutPrize + ub.Get("SiteBz") + "到" + (n.phase + 1) + "期";
                        ah = "在第" + n.phase + "期滚存";
                    }

                }
                if (n.type == 3 || n.type == 4 || n.type == 5)
                {
                    builder.Append("<b style=\"color:red\">");
                    builder.Append(ah + bi);
                    builder.Append(".结余:" + n.Jackpot + "");
                    builder.Append("</b>");
                }
                else
                {
                    builder.Append(ah + bi);
                    builder.Append(".结余:" + n.Jackpot + "");
                }
                builder.Append("[" + DT.FormatDate(n.AddTime, 0) + "]");
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有奖池记录.."));
        }

        string strText = "输入期号：例如(2016088)/,";
        string strName = "phase";
        string strType = "num";
        string strValu = phase;
        string strEmpt = "";
        string strIdea = "/";
        string strOthe = "搜一搜,jqc.aspx?act=jiangchi,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        GameFoot();
    }

    //赢利分析
    private void StatPage()
    {
        Master.Title = "" + GameName + "_赢利分析";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx") + "\">" + GameName + "</a>&gt;赢利分析");
        builder.Append(Out.Tab("</div>", "<br />"));
        //上期下注（最新开奖这一期）
        long shang_1 = new BCW.JQC.BLL.JQC_Bet().GetPrice("sum(PutGold)", "Lottery_issue=(SELECT TOP(1)phase FROM tb_JQC_Internet WHERE Result!='' ORDER BY ID DESC) AND isRobot=0");
        //上期返奖
        long shang_2 = new BCW.JQC.BLL.JQC_Bet().GetPrice("sum(GetMoney)", "Lottery_issue=(SELECT TOP(1)phase FROM tb_JQC_Internet WHERE Result!='' ORDER BY ID DESC) AND isRobot=0");
        //上期系统投入
        long shang_3 = new BCW.JQC.BLL.JQC_Jackpot().GetPrice("sum(InPrize)", "phase=(SELECT TOP(1)phase FROM tb_JQC_Internet WHERE Result!='' ORDER BY ID DESC) AND type=4");
        //上期系统取出
        long shang_4 = new BCW.JQC.BLL.JQC_Jackpot().GetPrice("sum(OutPrize)", "phase=(SELECT TOP(1)phase FROM tb_JQC_Internet WHERE Result!='' ORDER BY ID DESC) AND type=4");
        //上期系统收税
        long shang_5 = new BCW.JQC.BLL.JQC_Jackpot().GetPrice("sum(OutPrize)", "phase=(SELECT TOP(1)phase FROM tb_JQC_Internet WHERE Result!='' ORDER BY ID DESC) AND type=5");

        //近5期下注
        long jin_1 = new BCW.JQC.BLL.JQC_Bet().GetPrice("sum(PutGold)", "Lottery_issue in (SELECT TOP(5)phase FROM tb_JQC_Internet WHERE Result!='' ORDER BY ID DESC) AND isRobot=0");
        //近5期返奖
        long jin_2 = new BCW.JQC.BLL.JQC_Bet().GetPrice("sum(GetMoney)", "Lottery_issue in (SELECT TOP(5)phase FROM tb_JQC_Internet WHERE Result!='' ORDER BY ID DESC) AND isRobot=0");
        //近5期系统投入
        long jin_3 = new BCW.JQC.BLL.JQC_Jackpot().GetPrice("sum(InPrize)", "phase in (SELECT TOP(5)phase FROM tb_JQC_Internet WHERE Result!='' ORDER BY ID DESC) AND type=4");
        //近5期系统取出
        long jin_4 = new BCW.JQC.BLL.JQC_Jackpot().GetPrice("sum(OutPrize)", "phase in (SELECT TOP(5)phase FROM tb_JQC_Internet WHERE Result!='' ORDER BY ID DESC) AND type=4");
        //近5期系统收税
        long jin_5 = new BCW.JQC.BLL.JQC_Jackpot().GetPrice("sum(OutPrize)", "phase in (SELECT TOP(5)phase FROM tb_JQC_Internet WHERE Result!='' ORDER BY ID DESC) AND type=5");

        //总下注
        long zong_1 = new BCW.JQC.BLL.JQC_Bet().GetPrice("sum(PutGold)", "Lottery_issue in (SELECT TOP(100000)phase FROM tb_JQC_Internet WHERE Result!='' ORDER BY ID DESC) AND isRobot=0");
        //总返奖
        long zong_2 = new BCW.JQC.BLL.JQC_Bet().GetPrice("sum(GetMoney)", "Lottery_issue in (SELECT TOP(100000)phase FROM tb_JQC_Internet WHERE Result!='' ORDER BY ID DESC) AND isRobot=0");
        //总系统投入
        long zong_3 = new BCW.JQC.BLL.JQC_Jackpot().GetPrice("sum(InPrize)", "phase in (SELECT TOP(100000)phase FROM tb_JQC_Internet WHERE Result!='' ORDER BY ID DESC) AND type=4");
        //总系统取出
        long zong_4 = new BCW.JQC.BLL.JQC_Jackpot().GetPrice("sum(OutPrize)", "phase in (SELECT TOP(100000)phase FROM tb_JQC_Internet WHERE Result!='' ORDER BY ID DESC) AND type=4");
        //总系统收税
        long zong_5 = new BCW.JQC.BLL.JQC_Jackpot().GetPrice("sum(OutPrize)", "phase in (SELECT TOP(100000)phase FROM tb_JQC_Internet WHERE Result!='' ORDER BY ID DESC) AND type=5");

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("上期下注：" + shang_1 + "<br/>上期返奖：" + shang_2 + "<br/>");
        builder.Append("上期系统投入：" + shang_3 + "<br/>上期系统取出：" + shang_4 + "<br/>");
        builder.Append("上期系统收税：" + shang_5 + "<br/>上期系统盈利：" + (shang_1 + shang_4 + shang_5 - shang_2 - shang_3) + "");
        builder.Append(Out.Tab("</div>", Out.Hr()));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("近5期下注：" + jin_1 + "<br/>近5期返奖：" + jin_2 + "<br/>");
        builder.Append("近5期系统投入：" + jin_3 + "<br/>近5期系统取出：" + jin_4 + "<br/>");
        builder.Append("近5期系统收税：" + jin_5 + "<br/>近5期系统盈利：" + (jin_1 + jin_4 + jin_5 - jin_2 - jin_3) + "");
        builder.Append(Out.Tab("</div>", Out.Hr()));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("总下注：" + zong_1 + "<br/>总返奖：" + zong_2 + "<br/>");
        builder.Append("总系统投入：" + zong_3 + "<br/>总系统取出：" + zong_4 + "<br/>");
        builder.Append("总系统收税：" + zong_5 + "<br/>总系统盈利：" + (zong_1 + zong_4 + zong_5 - zong_2 - zong_3) + "");
        builder.Append(Out.Tab("</div>", ""));

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        if (Utils.ToSChinese(ac) == "马上查询")
        {
            string phase1 = (Utils.GetRequest("sTime", "all", 1, @"^[0-9]\d*$", ""));
            string phase2 = (Utils.GetRequest("sTime2", "all", 1, @"^[0-9]\d*$", ""));

            long q_1 = new BCW.JQC.BLL.JQC_Bet().GetPrice("sum(PutGold)", "(Lottery_issue between'" + phase1 + "' AND '" + phase2 + "') AND isRobot=0");
            long q_2 = new BCW.JQC.BLL.JQC_Bet().GetPrice("sum(GetMoney)", "(Lottery_issue between'" + phase1 + "' AND '" + phase2 + "') AND isRobot=0");
            long q_3 = new BCW.JQC.BLL.JQC_Jackpot().GetPrice("sum(InPrize)", "(phase between'" + phase1 + "' AND '" + phase2 + "') AND type=4");
            long q_4 = new BCW.JQC.BLL.JQC_Jackpot().GetPrice("sum(OutPrize)", "(phase between'" + phase1 + "' AND '" + phase2 + "') AND type=4");
            long q_5 = new BCW.JQC.BLL.JQC_Jackpot().GetPrice("sum(OutPrize)", "(phase between'" + phase1 + "' AND '" + phase2 + "') AND type=5");

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<hr/>" + phase1 + "期到" + phase2 + "期下注：" + q_1 + "<br/>" + phase1 + "期到" + phase2 + "期返奖：" + q_2 + "<br/>");
            builder.Append("" + phase1 + "期到" + phase2 + "期系统投入：" + q_3 + "<br/>" + phase1 + "期到" + phase2 + "期系统取出：" + q_4 + "<br/>");
            builder.Append("" + phase1 + "期到" + phase2 + "期系统收税：" + q_5 + "<br/>" + phase1 + "期到" + phase2 + "期系统盈利：" + (q_1 + q_4 + q_5 - q_2 - q_3) + "");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "开始期号：,结束期号：";
            string strName = "sTime,sTime2";
            string strType = "text,text";
            string strValu = "" + phase1 + "'" + phase2 + "'";
            string strEmpt = "false,false";
            string strIdea = "/";
            string strOthe = "马上查询,jqc.aspx?act=stat,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        else
        {
            builder.Append(Out.Tab("<div>", "<br/>"));
            builder.Append("<hr/>请输入需要查询的期号：");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "开始期号：,结束期号：";
            string strName = "sTime,sTime2";
            string strType = "text,text";
            string strValu = "" + new BCW.JQC.BLL.JQC_Internet().Get_kaiID() + "'" + new BCW.JQC.BLL.JQC_Internet().Get_kaiID() + "'";
            string strEmpt = "false,false";
            string strIdea = "/";
            string strOthe = "马上查询,jqc.aspx?act=stat,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        GameFoot();
    }

    //用户排行
    private void PaihangPage()
    {
        Master.Title = "" + GameName + "_用户排行";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx") + "\">" + GameName + "</a>&gt;用户排行");
        builder.Append(Out.Tab("</div>", "<br />"));

        DateTime startstate = Utils.ParseTime(Utils.GetRequest("startstate2", "all", 1, DT.RegexTime, DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd HH:mm:ss")));
        DateTime endstate = Utils.ParseTime(Utils.GetRequest("endstate2", "all", 1, DT.RegexTime, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

        builder.Append(Out.Tab("<div>", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));
        int aa = int.Parse(Utils.GetRequest("aa", "all", 1, @"^[0-1]$", "0"));
        if (ptype == 1)
            builder.Append("<h style=\"color:red\">净赚排行" + "</h>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=paihang&amp;ptype=1&amp;id=" + ptype + "") + "\">净赚排行</a>" + "|");
        if (ptype == 2)
            builder.Append("<h style=\"color:red\">赚币排行" + "</h>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=paihang&amp;ptype=2&amp;id=" + ptype + "") + "\">赚币排行</a>" + "|");
        if (ptype == 3)
            builder.Append("<h style=\"color:red\">购买次数" + "</h>" + "");
        else
            builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=paihang&amp;ptype=3&amp;id=" + ptype + "") + "\">购买次数</a>" + "");
        builder.Append(Out.Tab("</div>", "<br/>"));

        string ac = Utils.GetRequest("ac", "all", 1, "", "");

        string rewardid = "";
        int pageIndex = 1;
        int recordCount;
        string strWhere = string.Empty;
        string strWhere2 = string.Empty;
        string strWhere3 = string.Empty;
        string strWhere4 = string.Empty;
        int pageSize = 10;
        string[] pageValUrl = { "act", "aa", "startstate2", "endstate2", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        if (aa == 0)//0全部
        {
            strWhere = "";
        }
        else//1查询
        {
            strWhere = "and Input_Time>= '" + startstate + "' and Input_Time<= '" + endstate + "'";
        }

        if (ptype == 1)
        {
            strWhere = "State>0 " + strWhere + " GROUP BY UsID ORDER BY Sum(GetMoney-PutGold) DESC";
            strWhere2 = "TOP(100) UsID,Sum(GetMoney-PutGold) as bb";
            strWhere3 = "UsID,sum(GetMoney-PutGold) AS'bb' into #bang3";
            strWhere4 = "WHERE " + strWhere + "";

            DataSet rowbang = new BCW.JQC.BLL.JQC_Bet().GetList(strWhere2, strWhere);
            recordCount = rowbang.Tables[0].Rows.Count;
            DataSet bang = new BCW.JQC.BLL.JQC_Bet().GetListByPage2(0, recordCount, strWhere3, strWhere4);
            if (recordCount > 0)
            {
                int k = 1;
                int koo = (pageIndex - 1) * pageSize;
                int skt = pageSize;
                if (recordCount > koo + pageSize)
                {
                    skt = pageSize;
                }
                else
                {
                    skt = recordCount - koo;
                }

                for (int soms = 0; soms < skt; soms++)
                {
                    int usid;
                    long usmoney;
                    usid = Convert.ToInt32(bang.Tables[0].Rows[koo + soms][1]);
                    usmoney = Convert.ToInt64(bang.Tables[0].Rows[koo + soms][2]);
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br/>"));
                    }
                    string mename = new BCW.BLL.User().GetUsName(usid);
                    int wd = (pageIndex - 1) * 10 + k;
                    builder.Append("[<h style=\"color:red\">第" + wd + "名</h>]<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + mename + "(" + usid + ")</a>净赢<h style=\"color:red\">" + usmoney + "</h>" + ub.Get("SiteBz") + "");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                    rewardid = rewardid + usid.ToString() + "#";
                }
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录.."));
            }
        }
        else if (ptype == 2)
        {
            strWhere = "State>0 and GetMoney>0 " + strWhere + " GROUP BY UsID ORDER BY Sum(GetMoney) DESC";
            strWhere2 = "TOP(100) UsID,Sum(GetMoney) as bb";
            strWhere3 = "UsID,sum(GetMoney) AS'bb' into #bang3";
            strWhere4 = "WHERE " + strWhere + "";

            DataSet rowbang = new BCW.JQC.BLL.JQC_Bet().GetList(strWhere2, strWhere);
            recordCount = rowbang.Tables[0].Rows.Count;
            DataSet bang = new BCW.JQC.BLL.JQC_Bet().GetListByPage2(0, recordCount, strWhere3, strWhere4);
            if (recordCount > 0)
            {
                int k = 1;
                int koo = (pageIndex - 1) * pageSize;
                int skt = pageSize;
                if (recordCount > koo + pageSize)
                {
                    skt = pageSize;
                }
                else
                {
                    skt = recordCount - koo;
                }
                for (int soms = 0; soms < skt; soms++)
                {
                    int usid;
                    long usmoney;
                    usid = Convert.ToInt32(bang.Tables[0].Rows[koo + soms][1]);
                    usmoney = Convert.ToInt64(bang.Tables[0].Rows[koo + soms][2]);
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br/>"));
                    }
                    string mename = new BCW.BLL.User().GetUsName(usid);
                    int wd = (pageIndex - 1) * 10 + k;
                    builder.Append("[<h style=\"color:red\">第" + wd + "名</h>]<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + mename + "(" + usid + ")</a>赚币<h style=\"color:red\">" + usmoney + "</h>" + ub.Get("SiteBz") + "");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                    rewardid = rewardid + usid.ToString() + "#";
                }
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录.."));
            }
        }
        else if (ptype == 3)
        {
            strWhere = "id>0" + strWhere + " GROUP BY UsID ORDER BY count(UsID) DESC";
            strWhere2 = "TOP(100) UsID,count(UsID) as bb";
            strWhere3 = "UsID,count(UsID) AS'bb' into #bang3";
            strWhere4 = "WHERE " + strWhere + "";

            DataSet rowbang = new BCW.JQC.BLL.JQC_Bet().GetList(strWhere2, strWhere);
            recordCount = rowbang.Tables[0].Rows.Count;
            DataSet bang = new BCW.JQC.BLL.JQC_Bet().GetListByPage2(0, recordCount, strWhere3, strWhere4);
            if (recordCount > 0)
            {
                int k = 1;
                int koo = (pageIndex - 1) * pageSize;
                int skt = pageSize;
                if (recordCount > koo + pageSize)
                {
                    skt = pageSize;
                }
                else
                {
                    skt = recordCount - koo;
                }

                for (int soms = 0; soms < skt; soms++)
                {
                    int usid;
                    long usmoney;
                    usid = Convert.ToInt32(bang.Tables[0].Rows[koo + soms][1]);
                    usmoney = Convert.ToInt64(bang.Tables[0].Rows[koo + soms][2]);
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br/>"));
                    }
                    string mename = new BCW.BLL.User().GetUsName(usid);
                    int wd = (pageIndex - 1) * 10 + k;
                    builder.Append("[<h style=\"color:red\">第" + wd + "名</h>]<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + mename + "(" + usid + ")</a>共玩<h style=\"color:red\">" + usmoney + "</h>次");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                    rewardid = rewardid + usid.ToString() + "#";
                }
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录.."));
            }
        }

        string strText = "开始日期：/,结束日期：/,";
        string strName = "startstate2,endstate2,backurl";
        string strType = "date,date,hidden";
        string strValu = "" + DT.FormatDate(startstate, 0) + "'" + DT.FormatDate(endstate, 0) + "'" + Utils.getPage(0) + "";
        string strEmpt = "false,false,false";
        string strIdea = "/";
        string strOthe = "马上查询,jqc.aspx?act=paihang&amp;ptype=" + ptype + "&amp;aa=1,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        if (Utils.ToSChinese(ac) != "马上查询")
        {
            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append("排行榜奖励提示：<br/>");
            builder.Append("如需发放奖励，请按日期查询.");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            string wdy = "";
            if (pageIndex == 1)
                wdy = "TOP10";
            else
                wdy = "TOP" + (pageIndex - 1).ToString() + "1-" + pageIndex.ToString() + "0";

            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append(wdy + " 的用户ID分别是：" + rewardid);
            builder.Append(Out.Tab("</div>", ""));
            string strText2 = ",,,,";
            string strName2 = "startstate,endstate,pageIndex,rewardid,backurl";
            string strType2 = "hidden,hidden,hidden,hidden,hidden";
            string strValu2 = DT.FormatDate(startstate, 0) + "'" + DT.FormatDate(endstate, 0) + "'" + pageIndex + "'" + rewardid + "'" + Utils.getPage(0) + "";
            string strEmpt2 = "true,true,false";
            string strIdea2 = "/";
            string strOthe2 = wdy + "奖励发放,jqc.aspx?act=ReWard&amp;ptype=" + ptype + ",post,1,red";
            builder.Append(Out.wapform(strText2, strName2, strType2, strValu2, strEmpt2, strIdea2, strOthe2));
        }

        GameFoot();
    }

    //购买情况和获奖情况
    private void FenxiPage()
    {
        Master.Title = "" + GameName + "_购彩情况";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx") + "\">" + GameName + "</a>&gt;购彩情况");
        builder.Append(Out.Tab("</div>", "<br />"));

        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-5]$", "1"));
        int ptype2 = int.Parse(Utils.GetRequest("ptype2", "all", 1, @"^[1-2]$", "1"));
        int isRobot = int.Parse(Utils.GetRequest("isRobot", "all", 1, @"^[0-1]$", "0"));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        int qihaos = int.Parse(Utils.GetRequest("qihaos", "all", 1, @"^[1-9]\d*$", "0"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
        string strWhere = " ";
        string[] pageValUrl = { "act", "uid", "qihaos", "ptype", "isRobot", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        string strOrder = "Input_Time desc";

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 1)
        {
            builder.Append("<h style=\"color:red\">购彩" + "</h>" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=fenxi&amp;ptype=1&amp;id=" + ptype + "&amp;uid=" + uid + "&amp;qihaos=" + qihaos + "&amp;isRobot=" + isRobot + "") + "\">购彩</a>" + "|");
        }
        if (ptype == 2)
        {
            builder.Append("<h style=\"color:red\">中奖" + "</h>" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=fenxi&amp;ptype=2&amp;id=" + ptype + "&amp;uid=" + uid + "&amp;qihaos=" + qihaos + "&amp;isRobot=" + isRobot + "") + "\">中奖</a>" + "|");
        }
        if (ptype == 3)
        {
            builder.Append("<h style=\"color:red\">回收" + "</h>" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=fenxi&amp;ptype=3&amp;id=" + ptype + "&amp;uid=" + uid + "&amp;qihaos=" + qihaos + "&amp;isRobot=" + isRobot + "") + "\">回收</a>" + "|");
        }
        if (ptype == 4)
        {
            builder.Append("<h style=\"color:red\">不中" + "</h>" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=fenxi&amp;ptype=4&amp;id=" + ptype + "&amp;uid=" + uid + "&amp;qihaos=" + qihaos + "&amp;isRobot=" + isRobot + "") + "\">不中</a>" + "|");
        }
        if (ptype == 5)
        {
            builder.Append("<h style=\"color:red\">未开" + "</h>" + "");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=fenxi&amp;ptype=5&amp;id=" + ptype + "&amp;uid=" + uid + "&amp;qihaos=" + qihaos + "&amp;isRobot=" + isRobot + "") + "\">未开</a>" + "");
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        if (ptype == 2)
        {
            builder.Append(Out.Tab("<div>", ""));
            if (ptype2 == 1)
            {
                builder.Append("<h style=\"color:red\">已兑奖" + "</h>" + "|");
                builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=fenxi&amp;ptype=2&amp;id=" + ptype + "&amp;uid=" + uid + "&amp;qihaos=" + qihaos + "&amp;isRobot=" + isRobot + "&amp;ptype2=2") + "\">未兑奖</a>" + "");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=fenxi&amp;ptype=2&amp;id=" + ptype + "&amp;uid=" + uid + "&amp;qihaos=" + qihaos + "&amp;isRobot=" + isRobot + "&amp;ptype2=1") + "\">已兑奖</a>" + "|");
                builder.Append("<h style=\"color:red\">未兑奖" + "</h>" + "");
            }
            builder.Append(Out.Tab("</div>", "<br />"));
        }

        if (ptype == 1)
        {
            if (uid > 0)
            {
                if (qihaos > 0)
                {
                    strWhere = "UsID=" + uid + " and Lottery_issue=" + qihaos + " and isRobot=" + isRobot + "";
                }
                else
                    strWhere = "UsID=" + uid + " and isRobot=" + isRobot + "";
            }
            else if (qihaos > 0)
            {
                strWhere = "Lottery_issue=" + qihaos + " and isRobot=" + isRobot + "";
            }
            else
                strWhere = "isRobot=" + isRobot + "";
        }
        else if (ptype == 2)
        {
            if (uid > 0)
            {
                if (qihaos > 0)
                {
                    if (ptype2 == 1)
                    {
                        strWhere = "(State=3) and UsID=" + uid + " and Lottery_issue=" + qihaos + " and isRobot=" + isRobot + "";
                    }
                    else
                        strWhere = "(State=1) and UsID=" + uid + " and Lottery_issue=" + qihaos + " and isRobot=" + isRobot + "";
                }
                else
                {
                    if (ptype2 == 1)
                    {
                        strWhere = "(State=3) and UsID=" + uid + " and isRobot=" + isRobot + "";
                    }
                    else
                        strWhere = "(State=1) and UsID=" + uid + " and isRobot=" + isRobot + "";
                }
            }
            else if (qihaos > 0)
            {
                if (ptype2 == 1)
                {
                    strWhere = "(State=3) and Lottery_issue=" + qihaos + " and isRobot=" + isRobot + "";
                }
                else
                    strWhere = "(State=1) and Lottery_issue=" + qihaos + " and isRobot=" + isRobot + "";
            }
            else
            {
                if (ptype2 == 1)
                {
                    strWhere = "(State=3) and isRobot=" + isRobot + "";
                }
                else
                    strWhere = "(State=1) and isRobot=" + isRobot + "";
            }
        }
        else if (ptype == 3)
        {
            if (uid > 0)
            {
                if (qihaos > 0)
                {
                    strWhere = "State=4 and UsID=" + uid + " and Lottery_issue=" + qihaos + " and isRobot=" + isRobot + "";
                }
                else
                    strWhere = "State=4 and UsID=" + uid + " and isRobot=" + isRobot + "";
            }
            else if (qihaos > 0)
            {
                strWhere = "State=4 and Lottery_issue=" + qihaos + " and isRobot=" + isRobot + "";
            }
            else
                strWhere = "State=4 and isRobot=" + isRobot + "";
        }
        else if (ptype == 4)
        {
            if (uid > 0)
            {
                if (qihaos > 0)
                {
                    strWhere = "State=2 and UsID=" + uid + " and Lottery_issue=" + qihaos + " and isRobot=" + isRobot + "";
                }
                else
                    strWhere = "State=2 and UsID=" + uid + " and isRobot=" + isRobot + "";
            }
            else if (qihaos > 0)
            {
                strWhere = "State=2 and Lottery_issue=" + qihaos + " and isRobot=" + isRobot + "";
            }
            else
                strWhere = "State=2 and isRobot=" + isRobot + "";
        }
        else if (ptype == 5)
        {
            if (uid > 0)
            {
                if (qihaos > 0)
                {
                    strWhere = "State=0 and UsID=" + uid + " and Lottery_issue=" + qihaos + " and isRobot=" + isRobot + "";
                }
                else
                    strWhere = "State=0 and UsID=" + uid + " and isRobot=" + isRobot + "";
            }
            else if (qihaos > 0)
            {
                strWhere = "State=0 and Lottery_issue=" + qihaos + " and isRobot=" + isRobot + "";
            }
            else
                strWhere = "State=0 and isRobot=" + isRobot + "";
        }

        // 开始读取列表
        IList<BCW.JQC.Model.JQC_Bet> listjqcpay = new BCW.JQC.BLL.JQC_Bet().GetJQC_Bets(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listjqcpay.Count > 0)
        {
            int k = 1;
            foreach (BCW.JQC.Model.JQC_Bet n in listjqcpay)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div>", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }
                //获取id对应的用户名
                string mename = new BCW.BLL.User().GetUsName(n.UsID);
                builder.Append("[" + ((pageIndex - 1) * pageSize + k) + "]<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + mename + "</a>." + (n.Lottery_issue) + "期");
                BCW.JQC.Model.JQC_Internet model_num1 = new BCW.JQC.BLL.JQC_Internet().Get_kainum(n.Lottery_issue);
                if (model_num1.Result != "")
                {
                    builder.Append("&lt;" + model_num1.Result + ">");
                }
                builder.Append("投注[" + n.VoteNum + "]");
                builder.Append("每注" + n.Zhu_money + "" + ub.Get("SiteBz") + "/共" + n.Zhu + "注" + n.VoteRate + "倍/共投" + n.PutGold + ub.Get("SiteBz") + "[" + n.Input_Time + "].");
                if (n.GetMoney > 0)
                {
                    string oj = string.Empty;
                    if (n.Prize1 > 0)
                    {
                        oj = "一等:" + n.Prize1 + "注.";
                    }
                    if (n.Prize2 > 0)
                    {
                        oj = oj + "二等:" + n.Prize2 + "注.";
                    }
                    if (n.Prize3 > 0)
                    {
                        oj = oj + "三等:" + n.Prize3 + "注.";
                    }
                    if (n.Prize4 > 0)
                    {
                        oj = oj + "四等:" + n.Prize4 + "注";
                    }
                    if (n.State == 3)
                    {
                        builder.Append("<h style=\"color:red\">[中" + (n.Prize1 + n.Prize2 + n.Prize3 + n.Prize4) + "注(" + oj + ").赢" + n.GetMoney + "" + ub.Get("SiteBz") + "]</h>.(已领奖)");
                    }
                    else if (n.State == 4)
                    {
                        builder.Append("<h style=\"color:red\">[中" + (n.Prize1 + n.Prize2 + n.Prize3 + n.Prize4) + "注(" + oj + ").赢" + n.GetMoney + "" + ub.Get("SiteBz") + "]</h>.(过期未领奖)");
                    }
                    else
                    {
                        builder.Append("<h style=\"color:red\">[中" + (n.Prize1 + n.Prize2 + n.Prize3 + n.Prize4) + "注(" + oj + ").赢" + n.GetMoney + "" + ub.Get("SiteBz") + "]</h>.(未领奖)");
                    }
                }
                else
                {
                    if (n.State == 0)
                    {
                        builder.Append("<h style=\"color:blue\">(未开奖)</h>");
                    }
                    else
                    {
                        builder.Append("<h style=\"color:green\">(不中奖)</h>");
                    }
                }

                if (Convert.ToInt32(ub.GetSub("JQCStatus", xmlPath)) == 1)//维护
                {
                    builder.Append(".<a href=\"" + Utils.getUrl("jqc.aspx?act=del&amp;id=" + n.ID + "&amp;a=1&amp;b=" + ptype + "") + "\">[删]</a>");
                }
                builder.Append(Out.Tab("</div>", ""));
                k++;
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }

        string strText = "输入用户ID(可为空):/,输入彩票期号(可为空):/,机器人投注显示：/,";
        string strName = "uid,qihaos,isRobot,backurl";
        string strType = "num,num,select,hidden";
        string strValu = string.Empty;
        if (uid == 0)
        {
            if (qihaos == 0)
            {
                strValu = "''" + isRobot + "'" + Utils.getPage(0) + "";
            }
            else
            {
                strValu = "'" + qihaos + "'" + isRobot + "'" + Utils.getPage(0) + "";
            }
        }
        else
        {
            if (qihaos == 0)
            {
                strValu = "" + uid + "''" + isRobot + "'" + Utils.getPage(0) + "";
            }
            else
            {
                strValu = "" + uid + "'" + qihaos + "'" + isRobot + "'" + Utils.getPage(0) + "";
            }
        }
        string strEmpt = "true,true,0|关|1|开,false";
        string strIdea = "/";
        string strOthe = "搜一搜,jqc.aspx?act=fenxi&amp;ptype=" + ptype + ",post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        GameFoot();
    }

    //根据期号，显示详细的投注信息
    private void ViewPage()
    {
        int Lottery_issue = Convert.ToInt32(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));
        int type = int.Parse(Utils.GetRequest("type", "all", 1, @"^[1-2]$", "1"));

        Master.Title = "" + GameName + "_" + Lottery_issue + "期投注情况";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        if (type == 2)
        {
            builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("jqc.aspx?act=jiangchi2") + "\">奖池</a>&gt;" + Lottery_issue + "期投注情况");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx") + "\">" + GameName + "</a>&gt;" + Lottery_issue + "期投注情况");
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
        string strWhere = string.Empty;
        string strOrder = string.Empty;
        string[] pageValUrl = { "act", "id", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        strOrder = "Input_Time desc";


        BCW.JQC.Model.JQC_Internet model_num = new BCW.JQC.BLL.JQC_Internet().Get_kainum(Lottery_issue);
        if (model_num.Result != "")
        {
            //Utils.Error("" + Lottery_issue + "期的开奖号码不存在.请输入开奖号码.", "");
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            if (ptype == 1)
                builder.Append("总下注|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=view&amp;id=" + Lottery_issue + "&amp;ptype=1") + "\">总下注|</a>");
            if (ptype == 2)
                builder.Append("中奖情况");
            else
                builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=view&amp;id=" + Lottery_issue + "&amp;ptype=2") + "\">中奖情况</a>");
            builder.Append(Out.Tab("</div>", "<br />"));

            if (ptype == 1)
            {
                strWhere = "Lottery_issue='" + Lottery_issue + "'";
            }
            else
            {
                strWhere = "Lottery_issue='" + Lottery_issue + "' and (State=1 or State=3 or State=4)";
            }

            // 开始读取列表
            IList<BCW.JQC.Model.JQC_Bet> listjqcpay = new BCW.JQC.BLL.JQC_Bet().GetJQC_Bets(pageIndex, pageSize, strWhere, strOrder, out recordCount);
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("第" + Lottery_issue + "期开出:" + model_num.Result + " ");
            builder.Append("共" + recordCount + "条购彩记录.<br/>");
            builder.Append("当期奖池:" + new BCW.JQC.BLL.JQC_Internet().get_jiangchi(Lottery_issue) + ",派出:" + new BCW.JQC.BLL.JQC_Bet().Get_paijiang(Lottery_issue) + ",系统收取:" + new BCW.JQC.BLL.JQC_Jackpot().Get_xtshouqu(Lottery_issue) + ".滚存：" + (new BCW.JQC.BLL.JQC_Internet().get_jiangchi(Lottery_issue) - new BCW.JQC.BLL.JQC_Bet().Get_paijiang(Lottery_issue) - new BCW.JQC.BLL.JQC_Jackpot().Get_xtshouqu(Lottery_issue)) + "<br/>");
            //根据期号查询每注中奖金额
            string zhu_meney = new BCW.JQC.BLL.JQC_Internet().get_zhumeney(Lottery_issue);
            int zm1 = 0; int zm2 = 0; int zm3 = 0; int zm4 = 0;
            if (zhu_meney != "")
            {
                string[] ab = zhu_meney.Split(',');
                try { zm1 = int.Parse(ab[0]); } catch { }
                try { zm2 = int.Parse(ab[1]); } catch { }
                try { zm3 = int.Parse(ab[2]); } catch { }
                try { zm4 = int.Parse(ab[3]); } catch { }
            }
            builder.Append("[一等奖]：" + new BCW.JQC.BLL.JQC_Bet().count_renshu((Lottery_issue), "Prize1") + "注，每注金额：" + zm1 + "<br/>");
            builder.Append("[二等奖]：" + new BCW.JQC.BLL.JQC_Bet().count_renshu((Lottery_issue), "Prize2") + "注，每注金额：" + zm2 + "<br/>");
            builder.Append("[三等奖]：" + new BCW.JQC.BLL.JQC_Bet().count_renshu((Lottery_issue), "Prize3") + "注，每注金额：" + zm3 + "<br/>");
            builder.Append("[四等奖]：" + new BCW.JQC.BLL.JQC_Bet().count_renshu((Lottery_issue), "Prize4") + "注，每注金额：" + zm4 + "<br/>");
            builder.Append(Out.Tab("</div>", ""));
            if (listjqcpay.Count > 0)
            {
                int k = 1;
                foreach (BCW.JQC.Model.JQC_Bet n in listjqcpay)
                {
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div>", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }
                    //根据投注id，查出对应的奖池
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(n.UsID) + "</a>");
                    builder.Append(":[" + n.VoteNum + "]每注" + n.Zhu_money + ",共" + n.Zhu + "注" + n.VoteRate + "倍,共投" + n.PutGold + ub.Get("SiteBz") + ",结余" + new BCW.JQC.BLL.JQC_Jackpot().Get_BetID(n.ID) + ".[" + n.Input_Time + "].");
                    if (n.GetMoney > 0 && ptype == 2)
                    {
                        string oj = string.Empty;
                        if (n.Prize1 > 0)
                        {
                            oj = "一等:" + n.Prize1 * n.VoteRate + "注.";
                        }
                        if (n.Prize2 > 0)
                        {
                            oj = oj + "二等:" + n.Prize2 * n.VoteRate + "注.";
                        }
                        if (n.Prize3 > 0)
                        {
                            oj = oj + "三等:" + n.Prize3 * n.VoteRate + "注.";
                        }
                        if (n.Prize4 > 0)
                        {
                            oj = oj + "四等:" + n.Prize4 * n.VoteRate + "注";
                        }
                        if (n.State == 3)
                        {
                            builder.Append("<h style=\"color:red\">[中" + (n.Prize1 * n.VoteRate + n.Prize2 * n.VoteRate + n.Prize3 * n.VoteRate + n.Prize4 * n.VoteRate) + "注(" + oj + ").赢" + n.GetMoney + "" + ub.Get("SiteBz") + "]</h>.(已领奖).");
                        }
                        else if (n.State == 4)
                        {
                            builder.Append("<h style=\"color:red\">[中" + (n.Prize1 * n.VoteRate + n.Prize2 * n.VoteRate + n.Prize3 * n.VoteRate + n.Prize4 * n.VoteRate) + "注(" + oj + ").赢" + n.GetMoney + "" + ub.Get("SiteBz") + "]</h>.(过期未领奖).");
                        }
                        else
                            builder.Append("<h style=\"color:red\">[中" + (n.Prize1 * n.VoteRate + n.Prize2 * n.VoteRate + n.Prize3 * n.VoteRate + n.Prize4 * n.VoteRate) + "注(" + oj + ").赢" + n.GetMoney + "" + ub.Get("SiteBz") + "]</h>.(未领奖).");
                    }
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "没有返彩或无下注记录.."));
            }
        }
        else
        {
            int a_putgoid = 0; int a_zhushu = 0;
            //该期下注注数
            DataSet zhu = new BCW.JQC.BLL.JQC_Bet().GetList("SUM(Zhu*VoteRate) as Zhu", "Lottery_issue='" + Lottery_issue + "'");
            try
            {
                a_zhushu = int.Parse(zhu.Tables[0].Rows[0]["Zhu"].ToString());
            }
            catch
            {
                a_zhushu = 0;
            }
            //该期下注金额
            DataSet putgoid = new BCW.JQC.BLL.JQC_Bet().GetList("SUM(PutGold) as PutGold", "Lottery_issue='" + Lottery_issue + "'");
            try
            {
                a_putgoid = int.Parse(putgoid.Tables[0].Rows[0]["PutGold"].ToString());
            }
            catch
            {
                a_putgoid = 0;
            }

            strWhere = "Lottery_issue='" + Lottery_issue + "'";
            IList<BCW.JQC.Model.JQC_Bet> listjqcpay = new BCW.JQC.BLL.JQC_Bet().GetJQC_Bets(pageIndex, pageSize, strWhere, strOrder, out recordCount);
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("本期已下注:" + a_putgoid + "" + ub.Get("SiteBz") + "(" + a_zhushu + "注)");
            builder.Append(Out.Tab("</div>", "<br/>"));
            if (listjqcpay.Count > 0)
            {
                int k = 1;
                foreach (BCW.JQC.Model.JQC_Bet n in listjqcpay)
                {
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div>", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(n.UsID) + "</a>");
                    builder.Append(":[" + n.VoteNum + "]每注" + n.Zhu_money + ",共" + n.Zhu + "注" + n.VoteRate + "倍,共投" + n.PutGold + ub.Get("SiteBz") + ",结余" + new BCW.JQC.BLL.JQC_Jackpot().Get_BetID(n.ID) + "");
                    //根据下注id查找奖池记录
                    builder.Append("[" + n.Input_Time + "].");

                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "没有购彩记录.."));
            }
        }
        GameFoot();
    }

    //管理每期信息
    private void guanli_qihaoPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));

        BCW.JQC.Model.JQC_Internet model = new BCW.JQC.BLL.JQC_Internet().GetJQC_Internet(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "" + GameName + "_编辑开奖信息";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx") + "\">" + GameName + "</a>&gt;编辑开奖信息");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "期数：/,赛事(4场)：/,主场(4场)：/,客场(4场)：/,比赛时间(4场)：/,开始时间：/,截止时间：/,比赛结果：/,比赛比分：/,当期奖池：/,投注注数：/,每注金额：/,,,";
        string strName = "phase,Match,Team_Home,Team_Away,Start_Time,Sale_Start,Sale_End,Result,Score,nowprize,zhu,zhu_money,id,act,backurl";
        string strType = "num,textarea,textarea,textarea,big,date,date,textarea,textarea,num,textarea,textarea,hidden,hidden,hidden";
        string strValu = "" + model.phase + "'" + model.Match + "'" + model.Team_Home + "'" + model.Team_Away + "'" + model.Start_Time + "'" + DT.FormatDate(model.Sale_Start, 0) + "'" + DT.FormatDate(model.Sale_End, 0) + "'" + model.Result + "'" + model.Score + "'" + model.nowprize + "'" + model.zhu + "'" + model.zhu_money + "'" + id + "'editsave'" + Utils.getPage(0) + "";
        string strEmpt = "false,false,false,false,false,false,false,false,false,true,true,true,true,false,false,false";
        string strIdea = "/";
        string strOthe = "确定编辑,jqc.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div>", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=del_sj&amp;ID=" + id + "") + "\">[删除该期]</a>");
        //builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=ck_sj&amp;id=" + id + "") + "\">[重开该期]</a>");
        builder.Append(Out.Tab("</div>", ""));

        GameFoot();
    }

    //修改每期信息
    private void editsavePage()
    {
        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[1-9]\d*$", "ID错误"));
        int phase = int.Parse(Utils.GetRequest("phase", "post", 2, @"^[1-9]\d*$", "期数错误"));
        string Match = Utils.GetRequest("Match", "post", 2, @"^", "赛事填写错误！");
        string Team_Home = Utils.GetRequest("Team_Home", "post", 2, @"^", "主场填写错误！");
        string Team_Away = Utils.GetRequest("Team_Away", "post", 2, @"^", "客场填写错误！");
        string Start_Time = Utils.GetRequest("Start_Time", "post", 2, @"^", "比赛时间填写错误！");
        DateTime Sale_Start = Utils.ParseTime(Utils.GetRequest("Sale_Start", "post", 2, DT.RegexTime, "开始时间格式填写出错,正确格式如" + DT.FormatDate(DateTime.Now, 0) + ""));
        DateTime Sale_End = Utils.ParseTime(Utils.GetRequest("Sale_End", "post", 2, DT.RegexTime, "截止时间格式填写出错,正确格式如" + DT.FormatDate(DateTime.Now, 0) + ""));
        string Result = Utils.GetRequest("Result", "post", 1, @"^", "");
        string Score = (Utils.GetRequest("Score", "post", 2, @"^", "比赛比分填写错误"));
        string zhu = (Utils.GetRequest("zhu", "post", 1, "", ""));
        string zhu_money = (Utils.GetRequest("zhu_money", "post", 1, "", ""));
        int nowprize = Convert.ToInt32(Utils.GetRequest("nowprize", "post", 2, @"^[0-9]\d*$", "奖池填写错误！"));

        BCW.JQC.Model.JQC_Internet model = new BCW.JQC.Model.JQC_Internet();
        model.phase = phase;
        model.Match = Match;
        model.Team_Home = Team_Home;
        model.Team_Away = Team_Away;
        model.Sale_Start = Sale_Start;
        model.Sale_End = Sale_End;
        model.Start_Time = Start_Time;
        model.Result = Result;
        model.Score = Score;
        model.zhu = zhu;
        model.zhu_money = zhu_money;
        model.nowprize = nowprize;
        model.ID = id;
        new BCW.JQC.BLL.JQC_Internet().Update_ht(model);
        Utils.Success("编辑第" + phase + "期", "编辑第" + phase + "期成功.正在返回.", Utils.getUrl("jqc.aspx?act=guanli_qihao&amp;id=" + id + ""), "1");
    }

    //删除一条投注记录
    private void DelPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "")
        {
            int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
            int b = int.Parse(Utils.GetRequest("b", "all", 1, @"^[0-9]\d*$", "1"));
            Master.Title = "删除一条投注记录";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除该记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?info=ok&amp;act=del&amp;id=" + id + "&amp;b=" + b + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=fenxi&amp;id=" + id + "&amp;ptype=" + b + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            int id2 = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
            int b2 = int.Parse(Utils.GetRequest("b", "all", 1, @"^[0-9]\d*$", "1"));

            if (!new BCW.JQC.BLL.JQC_Bet().Exists(id2))
            {
                Utils.Error("不存在的记录", "");
            }
            else
            {
                //根据id查询-购买表
                BCW.JQC.Model.JQC_Bet model = new BCW.JQC.BLL.JQC_Bet().GetJQC_Bet(id2);
                int meid = model.UsID;//用户名

                //如果未开奖，退回本金
                if (model.State == 0)
                {
                    new BCW.BLL.User().UpdateiGold(model.UsID, model.PutGold, "系统退回进球彩第" + model.Lottery_issue + "期未开奖的" + model.PutGold + "" + ub.Get("SiteBz") + "！");
                    new BCW.BLL.Guest().Add(1, meid, new BCW.BLL.User().GetUsName(meid), "系统退回您的进球彩：第" + model.Lottery_issue + "期未开奖的" + model.PutGold + "" + ub.Get("SiteBz") + "！");
                    //奖池增加
                    BCW.JQC.Model.JQC_Jackpot bb = new BCW.JQC.Model.JQC_Jackpot();
                    bb.AddTime = DateTime.Now;
                    bb.BetID = model.ID;
                    bb.InPrize = model.PutGold;
                    bb.phase = model.Lottery_issue;
                    bb.type = 2;
                    bb.UsID = model.UsID;
                    bb.Jackpot = new BCW.JQC.BLL.JQC_Jackpot().GetGold_phase(model.Lottery_issue) - model.PutGold;
                    bb.OutPrize = 0;
                    new BCW.JQC.BLL.JQC_Jackpot().Add(bb);
                }
                else
                {
                    long gold = 0;//个人酷币
                    long cMoney = 0;//差多少
                    long sMoney = 0;//实扣
                    string ui = string.Empty;
                    gold = new BCW.BLL.User().GetGold(model.UsID);//个人酷币
                    if (model.GetMoney > gold)
                    {
                        cMoney = model.GetMoney - gold + model.PutGold;
                        sMoney = model.GetMoney;
                    }
                    else
                    {
                        sMoney = model.GetMoney;
                    }

                    //如果币不够扣则记录日志并冻结IsFreeze
                    if (cMoney > 0)
                    {
                        BCW.Model.Gameowe owe = new BCW.Model.Gameowe();
                        owe.Types = 1;
                        owe.UsID = model.UsID;
                        owe.UsName = new BCW.BLL.User().GetUsName(meid);
                        owe.Content = "进球彩第" + model.Lottery_issue + "期下注" + model.PutGold + "" + ub.Get("SiteBz") + ".系统管理员" + new BCW.User.Manage().IsManageLogin() + "删除.";
                        owe.OweCent = cMoney;
                        owe.BzType = 10;
                        owe.EnId = model.ID;
                        owe.AddTime = DateTime.Now;
                        new BCW.BLL.Gameowe().Add(owe);
                        new BCW.BLL.User().UpdateIsFreeze(model.UsID, 1);
                        ui = "实扣" + sMoney + ",还差" + (cMoney) + ",系统已自动将您帐户冻结.";
                    }
                    string oop = string.Empty;
                    if (model.GetMoney > 0)
                    {
                        oop = "并扣除所得的" + model.GetMoney + "。";
                    }

                    //奖池增加
                    BCW.JQC.Model.JQC_Jackpot bb = new BCW.JQC.Model.JQC_Jackpot();
                    bb.AddTime = DateTime.Now;
                    bb.BetID = model.ID;
                    bb.InPrize = 0;
                    bb.phase = model.Lottery_issue;
                    bb.type = 2;
                    bb.UsID = model.UsID;
                    bb.Jackpot = new BCW.JQC.BLL.JQC_Jackpot().GetGold_phase(model.Lottery_issue) - model.PutGold + model.GetMoney;
                    bb.OutPrize = model.GetMoney - model.PutGold;
                    new BCW.JQC.BLL.JQC_Jackpot().Add(bb);

                    new BCW.BLL.User().UpdateiGold(model.UsID, model.PutGold - model.GetMoney, "无效购奖或非法操作，系统退回进球彩第" + model.Lottery_issue + "期下注的" + model.PutGold + "" + ub.Get("SiteBz") + "." + oop + "" + ui);
                    new BCW.BLL.Guest().Add(1, meid, new BCW.BLL.User().GetUsName(meid), "无效购奖或非法操作，系统退回进球彩第" + model.Lottery_issue + "期下注的" + model.PutGold + "" + ub.Get("SiteBz") + "." + oop + "" + ui);
                }

                new BCW.JQC.BLL.JQC_Bet().Delete(id2);
                Utils.Success("删除投注记录", "删除记录成功..", Utils.getPage("jqc.aspx?act=fenxi&amp;ptype=" + b2 + ""), "2");
            }
        }
    }

    //删除一条开奖数据(刷新机获取的数据)
    private void del_sjPage()
    {
        int id = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "选择ID出错"));
        Master.Title = "" + GameName + "_删除数据";
        string info = Utils.GetRequest("info", "all", 1, "", "");

        if (info != "")
        {
            int id2 = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "选择ID出错"));
            new BCW.JQC.BLL.JQC_Internet().Delete(id2);
            Utils.Success("删除数据", "删除成功，正在返回..", Utils.getUrl("jqc.aspx"), "1");
        }
        else
        {
            BCW.JQC.Model.JQC_Internet aaa = new BCW.JQC.BLL.JQC_Internet().GetJQC_Internet(id);
            builder.Append(Out.Div("title", "<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;<a href=\"" + Utils.getUrl("jqc.aspx") + "\">" + GameName + "</a>&gt;删除数据"));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示：确定要删除数据吗？（只删除开奖数据,对用户购买的数据没影响.）<br/>");
            try
            {
                builder.Append("数据内容：第" + aaa.phase + "期.<br/>");
            }
            catch
            {
                Utils.Error("该ID或期号不存在.", "");
            }
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?info=ok&amp;act=del_sj&amp;id=" + id + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        GameFoot();
    }

    //游戏配置
    private void PeizhiPage()
    {
        Master.Title = "" + GameName + "_游戏配置";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx") + "\">" + GameName + "</a>&gt;游戏配置");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("", ""));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        string xmlPath = "/Controls/jqc.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string Name = Utils.GetRequest("GameName", "post", 2, @"^[^\^]{1,20}$", "游戏名称限1-20字内");
            string JQCTop = Utils.GetRequest("JQCTop", "post", 3, @"^[\s\S]{1,2000}$", "头部Ubb限2000字内");
            string JQCFoot = Utils.GetRequest("JQCFoot", "post", 3, @"^[\s\S]{1,2000}$", "底部Ubb限2000字内");
            string SiteListNo = Utils.GetRequest("SiteListNo", "post", 2, @"^[0-9]\d*$", "分页条数填写出错");
            string hsdatetime = Utils.GetRequest("hsdatetime", "post", 2, @"^[0-9]\d*$", "系统过期回收" + ub.Get("SiteBz") + "的天数填写错误");
            string Expir = Utils.GetRequest("Expir", "post", 2, @"^[0-9]\d*$", "防刷秒数填写出错");
            string jqcOne = Utils.GetRequest("jqcOne", "post", 2, @"^[0-9]\d*$", "一等奖奖励填写错误");
            string jqcTwo = Utils.GetRequest("jqcTwo", "post", 2, @"^[0-9]\d*$", "二等奖奖励填写错误");
            string jqcThree = Utils.GetRequest("jqcThree", "post", 2, @"^[0-9]\d*$", "三等奖奖励填写错误");
            string jqcFour = Utils.GetRequest("jqcFour", "post", 2, @"^[0-9]\d*$", "四等奖奖励填写错误");
            string huishou = Utils.GetRequest("huishou", "post", 2, @"^[0-9]\d*$", "系统回收填写错误");
            string zhuPrice = Utils.GetRequest("zhuPrice", "post", 2, @"^[0-9]\d*$", "每注" + ub.Get("SiteBz") + "填写错误");
            string BigPrice = Utils.GetRequest("BigPrice", "post", 2, @"^[0-9]\d*$", "每期每ID限购多少" + ub.Get("SiteBz") + "填写错误");
            string jiangchi_guding = Utils.GetRequest("jiangchi_guding", "post", 2, @"^[0-9]\d*$", "奖池超过多少才回收系统投入填写错误");
            string jiangchi = Utils.GetRequest("jiangchi", "post", 2, @"^[0-9]\d*$", "奖池启动金和系统投入填写错误");
            string jianchi_zuidi = Utils.GetRequest("jianchi_zuidi", "post", 2, @"^[0-9]\d*$", "奖池最低派送金填写错误");
            string guize = Utils.GetRequest("guize", "post", 3, @"^[\s\S]{1,4000}$", "规则限4000字内");
            xml.dss["GameName"] = Name;
            xml.dss["JQCTop"] = JQCTop;
            xml.dss["JQCFoot"] = JQCFoot;
            xml.dss["SiteListNo"] = SiteListNo;
            xml.dss["hsdatetime"] = hsdatetime;
            xml.dss["Expir"] = Expir;
            xml.dss["jqcOne"] = jqcOne;
            xml.dss["jqcTwo"] = jqcTwo;
            xml.dss["jqcThree"] = jqcThree;
            xml.dss["jqcFour"] = jqcFour;
            xml.dss["huishou"] = huishou;
            xml.dss["zhuPrice"] = zhuPrice;
            xml.dss["BigPrice"] = BigPrice;
            xml.dss["jiangchi_guding"] = jiangchi_guding;
            xml.dss["jiangchi"] = jiangchi;//jianchi_zuidi
            xml.dss["jianchi_zuidi"] = jianchi_zuidi;
            xml.dss["guize"] = guize;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("" + GameName + "设置", "设置成功，正在返回..", Utils.getUrl("jqc.aspx?act=peizhi"), "1");
        }
        else
        {
            string strText = "游戏名称：/,头部Ubb：/,底部Ubb：/,游戏规则：(在原有的基础上添加)/,分页条数：/,兑奖天数有效期：/,防刷秒数：/,一等奖奖励(%)：/,二等奖奖励(%)：/,三等奖奖励(%)：/,四等奖奖励(%)：/,每期系统回收(%)：/,每注默认投注的" + ub.Get("SiteBz") + "：/,每期每ID限购：/,奖池超过多少才回收系统的投入：/,奖池启动金和系统每次投入：/,奖池最低派送金：/,";
            string strName = "GameName,JQCTop,JQCFoot,guize,SiteListNo,hsdatetime,Expir,jqcOne,jqcTwo,jqcThree,jqcFour,huishou,zhuPrice,BigPrice,jiangchi_guding,jiangchi,jianchi_zuidi,backurl";
            string strType = "text,textarea,textarea,big,text,text,text,text,text,text,text,text,text,text,text,text,text,hidden";
            string strValu = "" + xml.dss["GameName"] + "'" + xml.dss["JQCTop"] + "'" + xml.dss["JQCFoot"] + "'" + xml.dss["guize"] + "'" + xml.dss["SiteListNo"] + "'" + xml.dss["hsdatetime"] + "'"
                + xml.dss["Expir"] + "'" + xml.dss["jqcOne"] + "'" + xml.dss["jqcTwo"] + "'"
                + xml.dss["jqcThree"] + "'" + xml.dss["jqcFour"] + "'" + xml.dss["huishou"] + "'" + xml.dss["zhuPrice"] + "'" + xml.dss["BigPrice"] + "'" + xml.dss["jiangchi_guding"] + "'" + xml.dss["jiangchi"] + "'"
                + xml.dss["jianchi_zuidi"] + "'" + Utils.getPage(0) + "";
            string strEmpt = "false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定修改,jqc.aspx?act=peizhi,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            GameFoot();
        }
    }

    //游戏维护
    private void WeihuPage()
    {
        Master.Title = "" + GameName + "_游戏维护";
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/jqc.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置

        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string JQCStatus = Utils.GetRequest("JQCStatus", "post", 1, @"^[0-2]$", "0");
            string testID = Utils.GetRequest("testID", "all", 1, @"^[^\^]{1,2000}$", "");
            string ROBOTID = Utils.GetRequest("ROBOTID", "post", 1, @"^[^\^]{1,2000}$", "");
            string IsBot = Utils.GetRequest("IsBot", "post", 1, @"^[0-1]$", "0");
            string ROBOTbeilv = Utils.GetRequest("ROBOTbeilv", "post", 1, @"^[^\^]{1,2000}$", "");
            string ROBOTBUY = Utils.GetRequest("ROBOTBUY", "post", 1, @"^[0-9]\d*$", "1");
            string robotmiao = Utils.GetRequest("robotmiao", "all", 1, @"^[0-9]$", "5");

            xml.dss["ROBOTID"] = ROBOTID.Replace("\r\n", "").Replace(" ", "");
            xml.dss["IsBot"] = IsBot;
            xml.dss["ROBOTbeilv"] = ROBOTbeilv.Replace("\r\n", "").Replace(" ", "");
            xml.dss["ROBOTBUY"] = ROBOTBUY;
            xml.dss["testID"] = testID.Replace("\r\n", "").Replace(" ", "");
            xml.dss["JQCStatus"] = JQCStatus;
            xml.dss["robotmiao"] = robotmiao;
            if (JQCStatus == "0")
            {
                xml.dss["XIsBot"] = 1;
            }
            else
            {
                xml.dss["XIsBot"] = 0;
            }
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            if (JQCStatus == "0")
                Utils.Success("" + GameName + "_游戏维护", "游戏已开放，正在返回..", Utils.getUrl("jqc.aspx?act=weihu"), "1");
            else if (JQCStatus == "1")
                Utils.Success("" + GameName + "_游戏维护", "游戏已进入维护模式，正在返回..", Utils.getUrl("jqc.aspx?act=weihu"), "1");
            else if (JQCStatus == "2")
                Utils.Success("" + GameName + "_游戏维护", "游戏已进入内测模式，正在返回..", Utils.getUrl("jqc.aspx?act=weihu"), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;<a href=\"" + Utils.getUrl("jqc.aspx") + "\">" + GameName + "</a>&gt;游戏维护"));
            string strText = "游戏状态:/,机器人状态:/,测试ID:/,机器人ID:/,机器人投注倍率:(下注金额=倍率*每注" + ub.Get("SiteBz") + "数)#号分隔购买/,机器人每期购买订单数:（0为不限制）/,刷新间隔:/";
            string strName = "JQCStatus,IsBot,testID,ROBOTID,ROBOTbeilv,ROBOTBUY,robotmiao";
            string strType = "select,select,big,big,big,text,text";
            string strValu = "" + xml.dss["JQCStatus"] + "'" + xml.dss["IsBot"].ToString() + "'" + xml.dss["testID"] + "'" + xml.dss["ROBOTID"].ToString() + "'" + xml.dss["ROBOTbeilv"].ToString() + "'" + xml.dss["ROBOTBUY"].ToString() + "'" + xml.dss["robotmiao"].ToString() + "";
            string strEmpt = "0|正常|1|维护|2|内测,0|关闭|1|开启,false,true,false,false,false";
            string strIdea = "/";
            string strOthe = "确定修改,jqc.aspx?act=weihu,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }

        builder.Append(Out.Tab("<div>", Out.Hr()));
        string aa = xml.dss["testID"].ToString();
        string aa1 = xml.dss["ROBOTID"].ToString();
        string[] sNum = Regex.Split(aa, "#");
        string[] sNum1 = Regex.Split(aa1, "#");

        string[] name = aa.Split('#');
        string[] name1 = aa1.Split('#');

        if (aa.Length > 0)
        {
            builder.Append("测试ID：《" + sNum.Length + "》个<br/>");
            for (int n = 0; n < name.Length; n++)
            {
                if ((n + 1) % 5 == 0)
                {
                    builder.Append(name[n] + "," + "<br />");
                }
                else
                {
                    builder.Append(name[n] + ",");
                }
            }
        }

        if (aa1.Length > 0)
        {
            builder.Append("<br/>机器人：《" + sNum1.Length + "》个<br/>");
            for (int n = 0; n < name1.Length; n++)
            {
                if ((n + 1) % 5 == 0)
                {
                    builder.Append(name1[n] + "," + "<br />");
                }
                else
                {
                    builder.Append(name1[n] + ",");
                }
            }
        }
        builder.Append(Out.Tab("</div>", ""));


        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("温馨提示：<br/>1.多个测试ID和机器人ID请用#分隔.<br/>");
        builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=reset") + "\">【游戏重置】</a>");
        builder.Append(Out.Tab("</div>", ""));

        GameFoot();
    }

    //游戏重置
    private void ResetPage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId != 1 && ManageId != 9)
        {
            //Utils.Error("权限不足", "");
        }

        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "11")
        {
            new BCW.JQC.BLL.JQC_Internet().ClearTable("tb_JQC_Bet");
            new BCW.JQC.BLL.JQC_Internet().ClearTable("tb_JQC_Internet");
            new BCW.JQC.BLL.JQC_Internet().ClearTable("tb_JQC_Jackpot");
            Utils.Success("重置游戏", "重置[所有数据]成功..", Utils.getUrl("jqc.aspx?act=reset"), "2");
        }
        else if (info == "1")
        {
            Master.Title = "重置所有表吗？";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定重置所有表吗？将会把所有数据清空哦.");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=reset&amp;info=11") + "\">确定重置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=reset") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        else if (info == "22")
        {
            new BCW.JQC.BLL.JQC_Internet().ClearTable("tb_JQC_Internet");
            Utils.Success("重置游戏", "重置[开奖数据表]成功..", Utils.getUrl("jqc.aspx?act=reset"), "2");
        }
        else if (info == "2")
        {
            Master.Title = "重置开奖数据表吗？";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定重置开奖数据表吗？将会把所有数据清空哦.");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=reset&amp;info=22") + "\">确定重置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=reset") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        else if (info == "33")
        {
            new BCW.JQC.BLL.JQC_Internet().ClearTable("tb_JQC_Bet");
            Utils.Success("重置游戏", "重置[投注数据表]成功..", Utils.getUrl("jqc.aspx?act=reset"), "2");
        }
        else if (info == "3")
        {
            Master.Title = "重置投注数据表吗？";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定重置投注数据表吗？将会把所有数据清空哦.");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=reset&amp;info=33") + "\">确定重置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=reset") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        else if (info == "44")
        {
            new BCW.JQC.BLL.JQC_Internet().ClearTable("tb_JQC_Jackpot");
            Utils.Success("重置游戏", "重置[奖池表]成功..", Utils.getUrl("jqc.aspx?act=reset"), "2");
        }
        else if (info == "4")
        {
            Master.Title = "重置奖池表吗？";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定重置奖池表吗？将会把所有数据清空哦.");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=reset&amp;info=44") + "\">确定重置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=reset") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        else
        {
            Master.Title = "" + GameName + "_重置游戏";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
            builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx") + "\">" + GameName + "</a>&gt;重置游戏");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?info=1&amp;act=reset") + "\">[一键全部重置]</a><br/>");
            builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?info=2&amp;act=reset") + "\">[单独重置开奖表]</a><br/>");
            builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?info=3&amp;act=reset") + "\">[单独重置投注数据]</a><br/>");
            builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?info=4&amp;act=reset") + "\">[单独重置奖池表]</a>");
            builder.Append(Out.Tab("</div>", ""));

            GameFoot();
        }
    }

    //模拟开奖+手动开奖
    private void Top_addPage()
    {
        int ah = Convert.ToInt32(Utils.GetRequest("ah", "all", 1, "", "0"));
        if (ah == 2)
        {
            int phase = int.Parse(Utils.GetRequest("phase", "all", 2, @"^[1-9]\d*$", "填写开奖期号出错"));//开奖期号
            string Result = Utils.GetRequest("Result", "all", 2, @"^[0-3][,][0-3][,][0-3][,][0-3][,][0-3][,][0-3][,][0-3][,][0-3]", "填写开奖号码出错");//开奖号码
            string where = "where phase='" + phase + "'";
            BCW.JQC.Model.JQC_Internet model = new BCW.JQC.BLL.JQC_Internet().GetJQC_Internet_model(where);
            if (model.phase != 0)//有期号
            {
                //如果已开奖，则不能再开奖
                if (model.Result != "")
                {
                    Utils.Error("已有开奖号码.不能再开奖.", "");
                }
                //返奖
                if (Result != "")//如果开奖号码为空，则不返奖
                {
                    //取出当前期的奖池金额
                    long get_jiangchi_kou = new BCW.JQC.BLL.JQC_Jackpot().GetGold_phase((phase));//该期号最后一个id

                    #region 判断系统是否有投入和开奖是否够币
                    //如果奖池大于400w，则判断是否有系统投入，若有，则先扣除系统投入，再拿去派奖和扣税
                    if (get_jiangchi_kou > jiangchi_guding)
                    {
                        //通过比较type=4、InPrize次数和OutPrize次数对比
                        if (new BCW.JQC.BLL.JQC_Jackpot().Getxitong_toujin() > new BCW.JQC.BLL.JQC_Jackpot().Getxitong_huishou())
                        {
                            BCW.JQC.Model.JQC_Jackpot bb = new BCW.JQC.Model.JQC_Jackpot();
                            bb.AddTime = DateTime.Now;
                            bb.BetID = 0;
                            bb.InPrize = 0;
                            bb.Jackpot = get_jiangchi_kou - jiangchi;
                            bb.OutPrize = jiangchi;
                            bb.phase = (phase);//数据库最新一期
                            bb.type = 4;//系统
                            bb.UsID = 10086;
                            new BCW.JQC.BLL.JQC_Jackpot().Add(bb);
                        }
                    }
                    //判断该期最后的奖池是否大于最低的200w，如果是，则增加200w
                    if (get_jiangchi_kou < jianchi_zuidi)
                    {
                        BCW.JQC.Model.JQC_Jackpot aa = new BCW.JQC.Model.JQC_Jackpot();
                        aa.AddTime = DateTime.Now;
                        aa.BetID = 0;
                        aa.InPrize = jiangchi;
                        aa.Jackpot = jiangchi + get_jiangchi_kou;
                        aa.OutPrize = 0;
                        aa.phase = (phase);//数据库最新一期
                        aa.type = 4;//系统
                        aa.UsID = 10086;
                        new BCW.JQC.BLL.JQC_Jackpot().Add(aa);
                    }
                    #endregion

                    //取出当前期的奖池金额——系统增加或扣除后
                    long get_jiangchi = new BCW.JQC.BLL.JQC_Jackpot().GetGold_phase((phase));
                    //把金额存到相应的期号
                    new BCW.JQC.BLL.JQC_Internet().Update_Result("nowprize='" + get_jiangchi + "'", "phase='" + phase + "'");
                    //系统没收的金额并奖池减少
                    long moshou = Convert.ToInt64(get_jiangchi * huishou * 0.01);
                    BCW.JQC.Model.JQC_Jackpot aa1 = new BCW.JQC.Model.JQC_Jackpot();
                    aa1.AddTime = DateTime.Now;
                    aa1.BetID = 0;
                    aa1.InPrize = 0;
                    aa1.Jackpot = get_jiangchi - moshou;
                    aa1.OutPrize = moshou;
                    aa1.phase = (phase);
                    aa1.type = 3;//系统扣税10%
                    aa1.UsID = 10086;
                    new BCW.JQC.BLL.JQC_Jackpot().Add(aa1);

                    //检查投注表是否存在没开奖数据
                    //if (new BCW.JQC.BLL.JQC_Bet().Exists_num(model.phase))
                    {
                        //得到每个奖的金额
                        long z4 = Convert.ToInt64(get_jiangchi * jqcFour * 0.01);
                        long z3 = Convert.ToInt64(get_jiangchi * jqcThree * 0.01);
                        long z2 = Convert.ToInt64(get_jiangchi * jqcTwo * 0.01);
                        long z1 = Convert.ToInt64(get_jiangchi * jqcOne * 0.01);

                        DataSet ds = new BCW.JQC.BLL.JQC_Bet().GetList("*", "State=0 and Lottery_issue='" + model.phase + "'");
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            int count = 0;
                            string[] resultnum = Result.Split(',');

                            //中奖判断
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                //投注数据
                                int ID = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                                int UsID = int.Parse(ds.Tables[0].Rows[i]["UsID"].ToString());
                                string Lottery_issue = ds.Tables[0].Rows[i]["Lottery_issue"].ToString();
                                int State = int.Parse(ds.Tables[0].Rows[i]["State"].ToString());
                                long Zhu_money = Int64.Parse(ds.Tables[0].Rows[i]["Zhu_money"].ToString());
                                long PutGold = Int64.Parse(ds.Tables[0].Rows[i]["PutGold"].ToString());
                                long GetMoney = Int64.Parse(ds.Tables[0].Rows[i]["GetMoney"].ToString());
                                int VoteRate = int.Parse(ds.Tables[0].Rows[i]["VoteRate"].ToString());//投注倍率
                                DateTime Input_Time = Convert.ToDateTime(ds.Tables[0].Rows[i]["Input_Time"]);
                                int isRobot = int.Parse(ds.Tables[0].Rows[i]["isRobot"].ToString());
                                string Vote = ds.Tables[0].Rows[i]["VoteNum"].ToString();//投注号码
                                int VoteNum = int.Parse(ds.Tables[0].Rows[i]["Zhu"].ToString());//投注注数

                                if (VoteNum == 1)//单注
                                {
                                    #region 单式中奖算法
                                    string[] votenum = Vote.Split('#');
                                    if (Vote != "" && Result != "")
                                    {
                                        for (int k = 0; k < 8; k++)
                                        {
                                            string aa = votenum[k];
                                            string bb = resultnum[k];
                                            if (True(aa, bb))
                                            {
                                                count++;
                                            }
                                        }
                                    }
                                    #endregion

                                    #region 单式中奖更新数据库
                                    if (count == 5)//4等奖----State=4
                                    {
                                        new BCW.JQC.BLL.JQC_Bet().update_GetMoney("Prize4=1", "ID='" + ID + "'");
                                        new BCW.JQC.BLL.JQC_Bet().Update_win(ID, 1);//0未开1中2不中3已领4过期
                                    }
                                    else if (count == 6)//3等奖----State=3
                                    {
                                        new BCW.JQC.BLL.JQC_Bet().update_GetMoney("Prize3=1", "ID='" + ID + "'");
                                        new BCW.JQC.BLL.JQC_Bet().Update_win(ID, 1);
                                    }
                                    else if (count == 7)//2等奖----State=2
                                    {
                                        new BCW.JQC.BLL.JQC_Bet().update_GetMoney("Prize2=1", "ID='" + ID + "'");
                                        new BCW.JQC.BLL.JQC_Bet().Update_win(ID, 1);
                                    }
                                    else if (count == 8)//1等奖----State=1
                                    {
                                        new BCW.JQC.BLL.JQC_Bet().update_GetMoney("Prize1=1", "ID='" + ID + "'");
                                        new BCW.JQC.BLL.JQC_Bet().Update_win(ID, 1);
                                    }
                                    else//不中奖
                                    {
                                        new BCW.JQC.BLL.JQC_Bet().Update_win(ID, 2);
                                    }
                                    count = 0;
                                    #endregion
                                }
                                else//复式
                                {
                                    #region 复式中奖算法
                                    int num1 = 0; int num2 = 0; int num3 = 0; int num4 = 0;
                                    string[] buyvote = Vote.Split('#');
                                    string[] vote1 = buyvote[0].Split(',');
                                    string[] vote2 = buyvote[1].Split(',');
                                    string[] vote3 = buyvote[2].Split(',');
                                    string[] vote4 = buyvote[3].Split(',');
                                    string[] vote5 = buyvote[4].Split(',');
                                    string[] vote6 = buyvote[5].Split(',');
                                    string[] vote7 = buyvote[6].Split(',');
                                    string[] vote8 = buyvote[7].Split(',');

                                    List<string[]> list = new List<string[]>();
                                    list.Add(vote1);
                                    list.Add(vote2);
                                    list.Add(vote3);
                                    list.Add(vote4);
                                    list.Add(vote5);
                                    list.Add(vote6);
                                    list.Add(vote7);
                                    list.Add(vote8);
                                    string[] totalResult;
                                    string[] Finalresult = Result.Split(',');

                                    totalResult = bianli(list);//得到投注数据

                                    for (int iresult = 0; iresult < totalResult.Length; iresult++)
                                    {
                                        //builder.Append(totalResult[iresult].Replace("#", ",") + "<br/>");
                                        string[] results = totalResult[iresult].Split('#');
                                        for (int j = 0; j < results.Length; j++)
                                        {
                                            if (results[j].Equals(Finalresult[j]))
                                            {
                                                count++;//遍历开奖数据是否相同，相同则count+1
                                            }
                                        }
                                        if (count == 5)//如果count出现的次数等于5，证明是四等奖
                                        {
                                            num4++;
                                        }
                                        if (count == 6)//如果count出现的次数等于6，证明是三等奖
                                        {
                                            num3++;

                                        }
                                        if (count == 7)//如果count出现的次数等于7，证明是二等奖
                                        {
                                            num2++;
                                        }
                                        if (count == 8)//如果count出现的次数等于8，证明是一等奖
                                        {
                                            num1 = 1;
                                        }
                                        count = 0;
                                    }
                                    #endregion

                                    #region 单式中奖更新数据库
                                    //更新各中几等奖
                                    if (num1 > 0)
                                    {
                                        new BCW.JQC.BLL.JQC_Bet().update_GetMoney("Prize1=1", "ID='" + ID + "'");
                                    }
                                    if (num2 > 0)
                                    {
                                        new BCW.JQC.BLL.JQC_Bet().update_GetMoney("Prize2=" + num2 + "", "ID='" + ID + "'");
                                    }
                                    if (num3 > 0)
                                    {
                                        new BCW.JQC.BLL.JQC_Bet().update_GetMoney("Prize3=" + num3 + "", "ID='" + ID + "'");
                                    }
                                    if (num4 > 0)
                                    {
                                        new BCW.JQC.BLL.JQC_Bet().update_GetMoney("Prize4=" + num4 + "", "ID='" + ID + "'");
                                    }
                                    //更新中奖状态
                                    if (num1 == 0 && num2 == 0 && num3 == 0 && num4 == 0)
                                    {
                                        new BCW.JQC.BLL.JQC_Bet().Update_win(ID, 2);
                                    }
                                    else
                                    {
                                        new BCW.JQC.BLL.JQC_Bet().Update_win(ID, 1);
                                    }
                                    #endregion
                                }
                            }
                        }

                        //计算当期各等奖总注数
                        int q4 = new BCW.JQC.BLL.JQC_Bet().count_renshu(model.phase, "Prize4");
                        int q3 = new BCW.JQC.BLL.JQC_Bet().count_renshu(model.phase, "Prize3");
                        int q2 = new BCW.JQC.BLL.JQC_Bet().count_renshu(model.phase, "Prize2");
                        int q1 = new BCW.JQC.BLL.JQC_Bet().count_renshu(model.phase, "Prize1");
                        //得到每注金额
                        long z_price4, z_price3, z_price2, z_price1 = 0;
                        if (q1 != 0) { z_price1 = z1 / q1; } else { z_price1 = z1; }
                        if (q2 != 0) { z_price2 = z2 / q2; } else { z_price2 = z2; }
                        if (q3 != 0) { z_price3 = z3 / q3; } else { z_price3 = z3; }
                        if (q4 != 0) { z_price4 = z4 / q4; } else { z_price4 = z4; }

                        string ai = q1 + "," + q2 + "," + q3 + "," + q4;
                        string aii = z_price1 + "," + z_price2 + "," + z_price3 + "," + z_price4;

                        string[] sc = Result.Split(',');
                        string s = "";
                        for (int jj = 0; jj < sc.Length; jj++)
                        {
                            if (jj == 1 || jj == 3 || jj == 5 || jj == 7)
                            {
                                s += ":";
                                s += sc[jj];
                            }
                            else if (jj == 2 || jj == 4 || jj == 6)
                            {
                                s += "#";
                                s += sc[jj];
                            }
                            else
                                s += sc[jj];
                        }
                        //把中奖注数和每注金额存到数据库对应的那期
                        new BCW.JQC.BLL.JQC_Internet().Update_Result("Score='" + s + "',Result='" + Result + "',zhu='" + ai + "',zhu_money='" + aii + "'", "phase='" + phase + "'");

                        //派奖
                        DataSet ds1 = new BCW.JQC.BLL.JQC_Bet().GetList("*", " State>0 and Lottery_issue='" + model.phase + "'");
                        for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                        {
                            int ID = int.Parse(ds1.Tables[0].Rows[i]["ID"].ToString());
                            int UsID = int.Parse(ds1.Tables[0].Rows[i]["UsID"].ToString());
                            int VoteRate = int.Parse(ds1.Tables[0].Rows[i]["VoteRate"].ToString());//投注倍率
                            int isRobot = int.Parse(ds1.Tables[0].Rows[i]["isRobot"].ToString());
                            int Prize1 = int.Parse(ds1.Tables[0].Rows[i]["Prize1"].ToString());
                            int Prize2 = int.Parse(ds1.Tables[0].Rows[i]["Prize2"].ToString());
                            int Prize3 = int.Parse(ds1.Tables[0].Rows[i]["Prize3"].ToString());
                            int Prize4 = int.Parse(ds1.Tables[0].Rows[i]["Prize4"].ToString());
                            long qian = 0;

                            //奖池减少
                            BCW.JQC.Model.JQC_Jackpot bb = new BCW.JQC.Model.JQC_Jackpot();
                            bb.AddTime = DateTime.Now;
                            bb.BetID = ID;
                            bb.InPrize = 0;
                            bb.phase = (phase);
                            bb.type = 1;
                            bb.UsID = UsID;
                            if (Prize1 > 0 || Prize2 > 0 || Prize3 > 0 || Prize4 > 0)
                            {
                                qian = z_price4 * VoteRate * Prize4 + z_price3 * VoteRate * Prize3 + z_price2 * VoteRate * Prize2 + z_price1 * VoteRate * Prize1;
                                new BCW.JQC.BLL.JQC_Bet().update_GetMoney("GetMoney='" + qian + "'", "ID=" + ID + "");
                                //奖池
                                bb.Jackpot = new BCW.JQC.BLL.JQC_Jackpot().GetGold_phase(phase) - qian;
                                bb.OutPrize = qian;
                                new BCW.JQC.BLL.JQC_Jackpot().Add(bb);

                                if (isRobot == 0)
                                    new BCW.BLL.Guest().Add(1, UsID, new BCW.BLL.User().GetUsName(UsID), "恭喜您在" + GameName + "第" + model.phase + "期获得了" + qian + "" + ub.Get("SiteBz") + "[url=/bbs/game/jqc.aspx?act=case]马上兑奖[/url]");
                                string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + new BCW.BLL.User().GetUsName(UsID) + "[/url]在[url=/bbs/game/jqc.aspx]" + GameName + "[/url]《" + model.phase + "期》获得了" + qian + "" + ub.Get("SiteBz") + "";
                                new BCW.BLL.Action().Add(1017, ID, UsID, "", wText);
                                System.Threading.Thread.Sleep(5);
                            }
                        }

                        System.Threading.Thread.Sleep(5);

                        //把剩余的奖池，加到下一期去,该期奖池清空
                        BCW.JQC.Model.JQC_Jackpot aa3 = new BCW.JQC.Model.JQC_Jackpot();
                        aa3.AddTime = DateTime.Now;
                        aa3.BetID = 0;
                        aa3.InPrize = 0;
                        aa3.Jackpot = 0;
                        aa3.OutPrize = get_jiangchi - moshou - new BCW.JQC.BLL.JQC_Bet().Get_paijiang(phase);
                        aa3.phase = (phase);
                        aa3.type = 5;//5是系统滚存
                        aa3.UsID = 10086;
                        new BCW.JQC.BLL.JQC_Jackpot().Add(aa3);

                        BCW.JQC.Model.JQC_Jackpot aa2 = new BCW.JQC.Model.JQC_Jackpot();
                        aa2.AddTime = DateTime.Now;
                        aa2.BetID = 0;
                        aa2.InPrize = get_jiangchi - moshou - new BCW.JQC.BLL.JQC_Bet().Get_paijiang(phase);
                        aa2.Jackpot = new BCW.JQC.BLL.JQC_Jackpot().GetGold_phase((phase + 1)) + get_jiangchi - moshou - new BCW.JQC.BLL.JQC_Bet().Get_paijiang(phase);
                        aa2.OutPrize = 0;
                        aa2.phase = (phase + 1);
                        aa2.type = 5;//5是系统滚存
                        aa2.UsID = 10086;
                        new BCW.JQC.BLL.JQC_Jackpot().Add(aa2);

                        //滚存后，判断下一期是否够200w，如果不够则加200w
                        //查询滚存后的那期奖池
                        if (new BCW.JQC.BLL.JQC_Jackpot().GetGold_phase(((phase) + 1)) < jianchi_zuidi)
                        {
                            BCW.JQC.Model.JQC_Jackpot aa = new BCW.JQC.Model.JQC_Jackpot();
                            aa.AddTime = DateTime.Now;
                            aa.BetID = 0;
                            aa.InPrize = jiangchi;
                            aa.Jackpot = jiangchi + new BCW.JQC.BLL.JQC_Jackpot().GetGold_phase(((phase) + 1));
                            aa.OutPrize = 0;
                            aa.phase = ((phase) + 1);
                            aa.type = 4;//系统
                            aa.UsID = 10086;
                            new BCW.JQC.BLL.JQC_Jackpot().Add(aa);
                        }

                        //机器人兑奖
                        DataSet ds5 = new BCW.JQC.BLL.JQC_Bet().GetList("*", "GetMoney>0 AND isRobot=1");
                        if (ds5 != null && ds5.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds5.Tables[0].Rows.Count; i++)
                            {
                                int ID = int.Parse(ds5.Tables[0].Rows[i]["ID"].ToString());
                                int UsID = int.Parse(ds5.Tables[0].Rows[i]["UsID"].ToString());
                                long GetMoney = Int64.Parse(ds5.Tables[0].Rows[i]["GetMoney"].ToString());

                                new BCW.JQC.BLL.JQC_Bet().Update_win(ID, 3);//6为中奖
                                new BCW.BLL.User().UpdateiGold(UsID, GetMoney, "" + GameName + "兑奖-标识ID" + ID + "");
                            }
                        }

                        Utils.Success("派奖操作", "手动添加开奖成功.", Utils.getUrl("jqc.aspx"), "1");
                    }
                    //else
                    //{
                    //    Utils.Error("该期已开奖.", "");
                    //}
                }
            }
            else
            {
                //Utils.Error("抱歉,请先添加比赛信息.", "");
                BCW.JQC.Model.JQC_Internet model_add = new BCW.JQC.Model.JQC_Internet();
                model_add.phase = phase;
                model_add.Result = Result;
                model_add.Match = "超级联赛1,超级联赛2,超级联赛3,超级联赛4";
                model_add.Start_Time = "2016-07-03 20:40:00";
                model_add.Sale_End = Convert.ToDateTime("2026-07-01 20:40:00");
                model_add.Sale_Start = Convert.ToDateTime("2016-07-01 20:40:00");
                model_add.Team_Home = "A1,A2,A3,A4";
                model_add.Team_Away = "B1,B2,B3,B4";
                model_add.Score = "0:3#0:0#2:1#0:2";
                new BCW.JQC.BLL.JQC_Internet().Add(model_add);
            }
        }
        else if (ah == 1)
        {
            int phase1 = int.Parse(Utils.GetRequest("phase", "all", 2, @"^[1-9]\d*$", "填写开奖期号出错"));//开奖期号
            string Result1 = Utils.GetRequest("Result", "all", 2, @"^[0-3][,][0-3][,][0-3][,][0-3][,][0-3][,][0-3][,][0-3][,][0-3]", "填写开奖号码出错");//开奖号码

            Master.Title = "对第：" + phase1 + "期开奖吗？";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定对第：" + phase1 + "期开奖吗？开奖号码为：" + Result1 + "");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=Top_add&amp;ah=2&amp;phase=" + phase1 + "&amp;Result=" + Result1 + "") + "\">确定开奖</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx?act=Top_add&amp;ah=0&amp;phase=" + phase1 + "&amp;Result=" + Result1 + "") + "\">先看看吧..</a>");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        else
        {
            Master.Title = "" + GameName + "_添加开奖数据";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
            builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx") + "\">" + GameName + "</a>&gt;添加开奖数据");
            builder.Append(Out.Tab("</div>", ""));


            string phase2 = (Utils.GetRequest("phase", "all", 1, @"^[1-9]\d*$", ""));
            string Result2 = Utils.GetRequest("Result", "all", 1, @"^[0-3][,][0-3][,][0-3][,][0-3][,][0-3][,][0-3][,][0-3][,][0-3]", "");//开奖号码
            string strText = "开奖期号：/,开奖号码：/,,";
            string strName = "phase,Result,hid,act";
            string strType = "text,text,hidden,hidden";
            string strValu = "" + phase2 + "'" + Result2 + "'" + Utils.getPage(0) + "'";
            string strEmpt = "false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定添加,jqc.aspx?act=Top_add&amp;ah=1,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append("温馨提示：<br/>1.开奖格式：0,1,2,3,0,1,2,3 (英文的逗号)<br/>2.若已有开奖号码则不能修改");
            builder.Append(Out.Tab("</div>", ""));

            GameFoot();
        }
    }


    //添加比赛
    private void add_bisaiPage()
    {
        int ah = Convert.ToInt32(Utils.GetRequest("ah", "all", 1, "", "0"));
        if (ah == 1)
        {
            int phase = int.Parse(Utils.GetRequest("phase", "post", 2, @"^[1-9]\d*$", "期数错误"));
            if (new BCW.JQC.BLL.JQC_Internet().Exists_phase(phase))
            {
                Utils.Error("抱歉，该期已存在.", "");
            }
            string Match = Utils.GetRequest("Match", "post", 2, @"^", "赛事填写错误！");
            string Team_Home = Utils.GetRequest("Team_Home", "post", 2, @"^", "主场填写错误！");
            string Team_Away = Utils.GetRequest("Team_Away", "post", 2, @"^", "客场填写错误！");
            string Start_Time = Utils.GetRequest("Start_Time", "post", 2, @"^", "比赛时间填写错误！");
            DateTime Sale_Start = Utils.ParseTime(Utils.GetRequest("Sale_Start", "post", 2, DT.RegexTime, "开始时间格式填写出错,正确格式如" + DT.FormatDate(DateTime.Now, 0) + ""));
            DateTime Sale_End = Utils.ParseTime(Utils.GetRequest("Sale_End", "post", 2, DT.RegexTime, "截止时间格式填写出错,正确格式如" + DT.FormatDate(DateTime.Now, 0) + ""));

            BCW.JQC.Model.JQC_Internet model = new BCW.JQC.Model.JQC_Internet();
            model.phase = phase;
            model.Match = Match;
            model.Team_Home = Team_Home;
            model.Team_Away = Team_Away;
            model.Sale_Start = Sale_Start;
            model.Sale_End = Sale_End;
            model.Start_Time = Start_Time;
            model.Result = "";
            model.Score = "";
            model.zhu = "";
            model.zhu_money = "";
            model.nowprize = 0;
            new BCW.JQC.BLL.JQC_Internet().Add(model);
            Utils.Success("添加第" + phase + "期", "添加第" + phase + "期成功.正在返回.", Utils.getUrl("jqc.aspx"), "1");
        }
        else
        {
            Master.Title = "" + GameName + "_添加开奖信息";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
            builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx") + "\">" + GameName + "</a>&gt;添加开奖信息");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "期数：/,赛事(4场)：/,主场(4场)：/,客场(4场)：/,比赛时间(4场)：/,开始时间：(格式" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ")/,截止时间：(格式" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ")/,,";
            string strName = "phase,Match,Team_Home,Team_Away,Start_Time,Sale_Start,Sale_End,id,backurl";
            string strType = "num,textarea,textarea,textarea,big,date,date,hidden,hidden";
            string strValu = "''''''''" + Utils.getPage(0) + "";
            string strEmpt = "false,false,false,false,false,false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定添加,jqc.aspx?act=add_bisai&amp;ah=1,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append("填写说明：<br />");
            builder.Append("赛事如：A联赛,B联赛,C联赛,D联赛<br />");
            builder.Append("主场如：A1,B1,C1,D1<br />");
            builder.Append("客场如：A2,B2,C2,D2<br />");
            builder.Append("比赛时间如：2016-07-24 00:00:00,2016-07-24 00:00:00,2016-07-24 00:00:00,2016-07-25 00:00:00");
            builder.Append(Out.Tab("</div>", ""));

            GameFoot();
        }
    }

    //排行榜奖励发放--界面
    private void ReWard()
    {
        Master.Title = "" + GameName + "_奖励发放";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("jqc.aspx?act=paihang") + "\">用户排行</a>&gt;奖励发放");
        builder.Append(Out.Tab("</div>", "<br />"));

        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));
        DateTime startstate = Utils.ParseTime(Utils.GetRequest("startstate", "all", 2, DT.RegexTime, "开始时间填写无效"));
        DateTime endstate = Utils.ParseTime(Utils.GetRequest("endstate", "all", 2, DT.RegexTime, "结束时间填写无效"));
        int pageIndex = int.Parse(Utils.GetRequest("pageIndex", "all", 1, @"^[1-9]\d*$", "1"));
        string rewardid = Utils.GetRequest("rewardid", "post", 2, @"^[\s\S]{1,500}$", "当前页无可操作用户");

        string wdy = "";
        if (pageIndex == 1)
            wdy = "TOP10";
        else
            wdy = "TOP" + (pageIndex - 1).ToString() + "1-" + pageIndex.ToString() + "0";
        builder.Append(Out.Tab("<div>", ""));
        switch (ptype)
        {
            case 1:
                builder.Append("《净赚排行》" + wdy + "奖励发放：");
                break;
            case 2:
                builder.Append("《赚币排行》" + wdy + "奖励发放：");
                break;
            case 3:
                builder.Append("《购买次数》" + wdy + "奖励发放：");
                break;
        }
        builder.Append(Out.Tab("</div>", ""));

        int mzj = (pageIndex - 1) * 10;
        string[] IdRe = rewardid.Split('#');
        try
        {
            string strText2 = ",,,,TOP" + (mzj + 1) + "：" + IdRe[0] + "&nbsp;&nbsp;,,TOP" + (mzj + 2) + "：" + IdRe[1] + "&nbsp;&nbsp;,,TOP" + (mzj + 3) + "：" + IdRe[2] + "&nbsp;&nbsp;,,TOP" + (mzj + 4) + "：" + IdRe[3] + "&nbsp;&nbsp;,,TOP" + (mzj + 5) + "：" + IdRe[4] + "&nbsp;&nbsp;,,TOP" + (mzj + 6) + "：" + IdRe[5] + "&nbsp;&nbsp;,,TOP" + (mzj + 7) + "：" + IdRe[6] + "&nbsp;&nbsp;,,TOP" + (mzj + 8) + "：" + IdRe[7] + "&nbsp;&nbsp;,,TOP" + (mzj + 9) + "：" + IdRe[8] + "&nbsp;&nbsp;,,TOP" + pageIndex * 10 + "：" + IdRe[9] + "&nbsp;&nbsp;,";
            string strName2 = "act,ptype,startstate,endstate,top1,IdRe1,top2,IdRe2,top3,IdRe3,top4,IdRe4,top5,IdRe5,top6,IdRe6,top7,IdRe7,top8,IdRe8,top9,IdRe9,top10,IdRe10";
            string strType2 = "hidden,hidden,hidden,hidden,num,hidden,num,hidden,num,hidden,num,hidden,num,hidden,num,hidden,num,hidden,num,hidden,num,hidden,num,hidden";
            string strValu2 = "ReWardCase'" + ptype + "'" + DT.FormatDate(startstate, 0) + "'" + DT.FormatDate(endstate, 0) + "'" + "0'" + IdRe[0] + "'0'" + IdRe[1] + "'0'" + IdRe[2] + "'0'" + IdRe[3] + "'0'" + IdRe[4] + "'0'" + IdRe[5] + "'0'" + IdRe[6] + "'0'" + IdRe[7] + "'0'" + IdRe[8] + "'0'" + IdRe[9] + "'0";
            string strEmpt2 = "false,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true";
            string strIdea2 = "/";
            string strOthe2 = "提交,jqc.aspx,post,1,red";
            builder.Append(Out.wapform(strText2, strName2, strType2, strValu2, strEmpt2, strIdea2, strOthe2));
        }
        catch
        {
            builder.Append(Out.Tab("<div>", "<br/>"));
            builder.Append("当页少于10人，无法发放！");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("</div>", ""));

        GameFoot();
    }
    //排行榜奖励发放--执行
    private void ReWardCase()
    {
        int[] IdRe = new int[11];
        long[] Top = new long[11];
        IdRe[1] = int.Parse(Utils.GetRequest("IdRe1", "post", 1, "", "10086"));
        IdRe[2] = int.Parse(Utils.GetRequest("IdRe2", "post", 1, "", "10086"));
        IdRe[3] = int.Parse(Utils.GetRequest("IdRe3", "post", 1, "", "10086"));
        IdRe[4] = int.Parse(Utils.GetRequest("IdRe4", "post", 1, "", "10086"));
        IdRe[5] = int.Parse(Utils.GetRequest("IdRe5", "post", 1, "", "10086"));
        IdRe[6] = int.Parse(Utils.GetRequest("IdRe6", "post", 1, "", "10086"));
        IdRe[7] = int.Parse(Utils.GetRequest("IdRe7", "post", 1, "", "10086"));
        IdRe[8] = int.Parse(Utils.GetRequest("IdRe8", "post", 1, "", "10086"));
        IdRe[9] = int.Parse(Utils.GetRequest("IdRe9", "post", 1, "", "10086"));
        IdRe[10] = int.Parse(Utils.GetRequest("IdRe10", "post", 1, "", "10086"));
        Top[1] = Convert.ToInt64(Utils.GetRequest("top1", "post", 1, "", ""));
        Top[2] = Convert.ToInt64(Utils.GetRequest("top2", "post", 1, "", ""));
        Top[3] = Convert.ToInt64(Utils.GetRequest("top3", "post", 1, "", ""));
        Top[4] = Convert.ToInt64(Utils.GetRequest("top4", "post", 1, "", ""));
        Top[5] = Convert.ToInt64(Utils.GetRequest("top5", "post", 1, "", ""));
        Top[6] = Convert.ToInt64(Utils.GetRequest("top6", "post", 1, "", ""));
        Top[7] = Convert.ToInt64(Utils.GetRequest("top7", "post", 1, "", ""));
        Top[8] = Convert.ToInt64(Utils.GetRequest("top8", "post", 1, "", ""));
        Top[9] = Convert.ToInt64(Utils.GetRequest("top9", "post", 1, "", ""));
        Top[10] = Convert.ToInt64(Utils.GetRequest("top10", "post", 1, "", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));
        DateTime startstate = Utils.ParseTime(Utils.GetRequest("startstate", "all", 2, DT.RegexTime, "开始时间填写无效"));
        DateTime endstate = Utils.ParseTime(Utils.GetRequest("endstate", "all", 2, DT.RegexTime, "结束时间填写无效"));

        string wdy = "";
        switch (ptype)
        {
            case 1:
                wdy = "净赚排行榜";
                break;
            case 2:
                wdy = "赚币排行榜";
                break;
            case 3:
                wdy = "游戏狂人榜";
                break;
        }
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        if (Utils.ToSChinese(ac) == "确定发放")
        {
            for (int i = 1; i <= 10; i++)
            {
                if (Top[i] != 0)
                {
                    new BCW.BLL.User().UpdateiGold(IdRe[i], Top[i], "" + GameName + "" + wdy + "奖励");
                    //发内线
                    string strLog = "您在 " + startstate + " 至 " + endstate + " 里在游戏《" + GameName + "》" + wdy + "上取得了第" + i + "名的好成绩，系统奖励了" + Top[i] + "" + ub.Get("SiteBz") + "[url=/bbs/game/jqc.aspx]进入《" + GameName + "》[/url]";
                    new BCW.BLL.Guest().Add(0, IdRe[i], new BCW.BLL.User().GetUsName(IdRe[i]), strLog);
                    //动态
                    string mename = new BCW.BLL.User().GetUsName(IdRe[i]);
                    string wText = "[url=/bbs/uinfo.aspx?uid=" + IdRe[i] + "]" + mename + "[/url]在[url=/bbs/game/jqc.aspx]《" + GameName + "》[/url]" + wdy + "上取得了第" + i + "名的好成绩，系统奖励了" + Top[i] + "" + ub.Get("SiteBz");
                    new BCW.BLL.Action().Add(1001, 0, IdRe[i], "", wText);
                }
            }
            Utils.Success("奖励操作", "奖励操作成功", Utils.getUrl("jqc.aspx?act=paihang"), "1");
        }
        else
        {
            Master.Title = "" + GameName + "_奖励发放";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
            builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("jqc.aspx?act=paihang") + "\">用户排行</a>&gt;奖励发放");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("正在发放《" + wdy + "》奖励：");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("时间从：" + startstate + "到" + endstate + "<br/>");
            for (int j = 1; j <= 10; j++)
            {
                if (j == 10)
                {
                    builder.Append("TOP" + j + "：" + IdRe[j] + ".[" + Top[j] + "" + ub.Get("SiteBz") + "]");
                }
                else
                {
                    builder.Append("TOP" + j + "：" + IdRe[j] + ".[" + Top[j] + "" + ub.Get("SiteBz") + "]<br/>");
                }
            }

            string strText2 = ",,,,,,,,,,,,,,,,,,,,,,,";
            string strName2 = "act,ptype,startstate,endstate,top1,IdRe1,top2,IdRe2,top3,IdRe3,top4,IdRe4,top5,IdRe5,top6,IdRe6,top7,IdRe7,top8,IdRe8,top9,IdRe9,top10,IdRe10";
            string strType2 = "hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden";
            string strValu2 = "ReWardCase'" + ptype + "'" + DT.FormatDate(startstate, 0) + "'" + DT.FormatDate(endstate, 0) + "'" + Top[1] + "'" + IdRe[1] + "'" + Top[2] + "'" + IdRe[2] + "'" + Top[3] + "'" + IdRe[3] + "'" + Top[4] + "'" + IdRe[4] + "'" + Top[5] + "'" + IdRe[5] + "'" + Top[6] + "'" + IdRe[6] + "'" + Top[7] + "'" + IdRe[7] + "'" + Top[8] + "'" + IdRe[8] + "'" + Top[9] + "'" + IdRe[9] + "'" + Top[10] + "'" + IdRe[10];
            string strEmpt2 = "false,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true";
            string strIdea2 = "/";
            string strOthe2 = "确定发放,jqc.aspx,post,1,red";
            builder.Append(Out.wapform(strText2, strName2, strType2, strValu2, strEmpt2, strIdea2, strOthe2));
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getPage("jqc.aspx?act=paihang") + "\">再看看吧>></a>");
            builder.Append(Out.Tab("</div>", ""));
            GameFoot();
        }
    }



    //返赢返负===界面
    private void BackPage()
    {
        Master.Title = "" + GameName + "_返赢返负";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx") + "\">" + GameName + "</a>&gt;返赢返负");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("返赢点操作");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "开始时间:,结束时间:,返赢千分比:,至少赢多少才返:,消息通知:,";
        string strName = "sTime,oTime,iTar,iPrice,text,act";
        string strType = "date,date,num,num,text,hidden";
        string strValu = "" + DT.FormatDate(DateTime.Now.AddDays(-10), 0) + "'" + DT.FormatDate(DateTime.Now, 0) + "''''backsave3";
        string strEmpt = "false,false,false,false,false,false";
        string strIdea = "/";
        string strOthe = "马上返赢,jqc.aspx,post,1,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("返负点操作");
        builder.Append(Out.Tab("</div>", ""));

        strText = "开始时间:,结束时间:,返负千分比:,至少负多少才返:,消息通知:,";
        strName = "sTime,oTime,iTar,iPrice,text,act";
        strType = "date,date,num,num,text,hidden";
        strValu = "" + DT.FormatDate(DateTime.Now.AddDays(-10), 0) + "'" + DT.FormatDate(DateTime.Now, 0) + "''''backsave5";
        strEmpt = "false,false,false,false,false,false";
        strIdea = "/";
        strOthe = "马上返负,jqc.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        GameFoot();
    }
    //返赢返负===确定界面
    private void BackSavePage2(string act)
    {
        DateTime sTime = Utils.ParseTime(Utils.GetRequest("sTime", "all", 2, DT.RegexTime, "开始时间填写无效"));
        DateTime oTime = Utils.ParseTime(Utils.GetRequest("oTime", "all", 2, DT.RegexTime, "结束时间填写无效"));
        int iTar = Utils.ParseInt(Utils.GetRequest("iTar", "all", 2, @"^[0-9]\d*$", "千分比填写错误"));
        int iPrice = Utils.ParseInt(Utils.GetRequest("iPrice", "all", 2, @"^[0-9]\d*$", "返币填写错误"));
        string text = Utils.GetRequest("text", "all", 2, @"^[^\^]{1,5000}$", "消息填写太多了");

        Master.Title = "" + GameName + "_返赢返负";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");

        if (act == "backsave3")
        {
            builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("jqc.aspx?act=back") + "\">返赢返负</a>&gt;返赢确认");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("返赢时间段：<b>" + sTime + "</b>到<b>" + oTime + "</b><br />");
            builder.Append("返赢千分比：" + iTar + "<br />");
            builder.Append("至少赢：" + iPrice + "币返还.<br />");
            builder.Append("消息通知：" + text + ".");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "开始时间:,结束时间:,返赢千分比:,至少赢多少才返:,消息通知:,";
            string strName = "sTime,oTime,iTar,iPrice,text,act";
            string strType = "hidden,hidden,hidden,hidden,hidden,hidden";
            string strValu = "" + DT.FormatDate(sTime, 0) + "'" + DT.FormatDate(oTime, 0) + "'" + iTar + "'" + iPrice + "'" + text + "'backsave";
            string strEmpt = "false,false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定返赢,jqc.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        else if (act == "backsave5")
        {
            builder.Append("<a href=\"" + Utils.getUrl("jqc.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("jqc.aspx?act=back") + "\">返赢返负</a>&gt;返负确认");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("返负时间段：<b>" + sTime + "</b>到<b>" + oTime + "</b><br />");
            builder.Append("返负千分比：" + iTar + "<br />");
            builder.Append("至少负：" + iPrice + "币返还.<br />");
            builder.Append("消息通知：" + text + ".");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "开始时间:,结束时间:,返负千分比:,至少负多少才返:,消息通知:,";
            string strName = "sTime,oTime,iTar,iPrice,text,act";
            string strType = "hidden,hidden,hidden,hidden,hidden,hidden";
            string strValu = "" + DT.FormatDate(sTime, 0) + "'" + DT.FormatDate(oTime, 0) + "'" + iTar + "'" + iPrice + "'" + text + "'backsave2";
            string strEmpt = "false,false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定返负,jqc.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }

        GameFoot();
    }
    //返赢返负===执行
    private void BackSavePage(string act)
    {
        DateTime sTime = Utils.ParseTime(Utils.GetRequest("sTime", "all", 2, DT.RegexTime, "开始时间填写无效"));
        DateTime oTime = Utils.ParseTime(Utils.GetRequest("oTime", "all", 2, DT.RegexTime, "结束时间填写无效"));
        int iTar = Utils.ParseInt(Utils.GetRequest("iTar", "all", 2, @"^[0-9]\d*$", "千分比填写错误"));
        int iPrice = Utils.ParseInt(Utils.GetRequest("iPrice", "all", 2, @"^[0-9]\d*$", "返币填写错误"));
        string text = Utils.GetRequest("text", "all", 2, @"^[^\^]{1,5000}$", "消息填写太多了");

        if (act == "backsave")
        {
            DataSet ds = new BCW.JQC.BLL.JQC_Bet().GetList("UsID,sum(GetMoney-PutGold) as WinCents", "State>0 and Input_Time>='" + sTime + "'and Input_Time<'" + oTime + "' group by UsID");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                long Cents = Convert.ToInt64(ds.Tables[0].Rows[i]["WinCents"]);
                if (Cents > 0 && Cents >= iPrice)
                {
                    int usid = Convert.ToInt32(ds.Tables[0].Rows[i]["UsID"]);
                    long cent = Convert.ToInt64(Cents * (iTar * 0.001));
                    new BCW.BLL.User().UpdateiGold(usid, cent, "" + GameName + "返赢");
                    //发内线
                    string strLog = text + "返还了：" + cent + "" + ub.Get("SiteBz") + "[url=/bbs/game/jqc.aspx]进入" + GameName + "[/url]";

                    new BCW.BLL.Guest().Add(0, usid, new BCW.BLL.User().GetUsName(usid), strLog);
                }
            }
            Utils.Success("返赢操作", "返赢操作成功", Utils.getUrl("jqc.aspx?act=back"), "1");
        }
        else if (act == "backsave2")
        {
            DataSet ds = new BCW.JQC.BLL.JQC_Bet().GetList("UsID,sum(GetMoney-PutGold) as WinCents", "State>0 and Input_Time>='" + sTime + "'and Input_Time<'" + oTime + "' group by UsID");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                long Cents = Convert.ToInt64(ds.Tables[0].Rows[i]["WinCents"]);
                if (Cents < 0 && Cents < (-iPrice))
                {
                    int usid = Convert.ToInt32(ds.Tables[0].Rows[i]["UsID"]);
                    long cent = Convert.ToInt64((-Cents) * (iTar * 0.001));
                    new BCW.BLL.User().UpdateiGold(usid, cent, "" + GameName + "返负");
                    //发内线
                    string strLog = text + "返还了：" + cent + "" + ub.Get("SiteBz") + "[url=/bbs/game/jqc.aspx]进入" + GameName + "[/url]";
                    new BCW.BLL.Guest().Add(0, usid, new BCW.BLL.User().GetUsName(usid), strLog);
                }
            }
            Utils.Success("返负操作", "返负操作成功", Utils.getUrl("jqc.aspx?act=back"), "1");
        }
    }



    //游戏底部
    private void GameFoot()
    {
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("jqc.aspx") + "\">返回首页>></a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    //中奖类型
    private string OutType(int Types)
    {
        string pText = string.Empty;
        if (Types == 1)
            pText = "一等奖";
        else if (Types == 2)
            pText = "二等奖";
        else if (Types == 3)
            pText = "三等奖";
        else if (Types == 4)
            pText = "四等奖";
        return pText;
    }
    //开奖号码对比
    public bool True(string votenum, string resultnum)
    {
        if ((votenum).Contains(resultnum))
            return true;
        return false;
    }
    //遍历数组，组合投注结果
    private string[] bianli(List<string[]> al)
    {
        if (al.Count == 0)
            return null;
        int size = 1;
        for (int i = 0; i < al.Count; i++)
        {
            size = size * al[i].Length;
        }
        string[] str = new string[size];
        for (long j = 0; j < size; j++)
        {
            for (int m = 0; m < al.Count; m++)
            {
                str[j] = str[j] + al[m][(j * jisuan(al, m) / size) % al[m].Length] + "#";
            }
            str[j] = str[j].Trim('#');
        }
        return str;
    }
    //计算当前产生的结果数
    private int jisuan(List<string[]> al, int m)
    {
        int result = 1;
        for (int i = 0; i < al.Count; i++)
        {
            if (i <= m)
            {
                result = result * al[i].Length;
            }
            else
            {
                break;
            }
        }
        return result;
    }





}
