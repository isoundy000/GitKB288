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
using System.Text.RegularExpressions;
using System.Net;
using System.Text;

/// <summary>
/// 增加消费日志显示下注日期
/// 黄国军 20160601
/// 修改上证即时显示指数
/// 黄国军20160524
/// 上证修改消费记录
/// 黄国军 20160312
/// 活跃抽奖入口额度控制
/// 姚志光20160621
/// 蒙宗将 20161123 新排版新  功能
///        20161125 增加ID每期每种玩法限额
/// </summary>
public partial class bbs_game_stkguess : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/stkguess.xml";
    protected string GameName = ub.GetSub("StkName", "/Controls/stkguess.xml");
    protected string strText = string.Empty;
    protected string strName = string.Empty;
    protected string strType = string.Empty;
    protected string strValu = string.Empty;
    protected string strEmpt = string.Empty;
    protected string strIdea = string.Empty;
    protected string strOthe = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        //维护提示
        if (ub.GetSub("StkStatus", xmlPath) == "1")
        {
            Utils.Safe("此游戏");
        }

        //内测判断 0,内测是否开启1，是否为内测账号
        if (ub.GetSub("StkStatus", xmlPath) == "2")//内测
        {
            string StkCeshihao = ub.GetSub("StkCeshihao", "/Controls/stkguess.xml");
            string[] sNum = Regex.Split(StkCeshihao, "#");
            int sbsy = 0;
            for (int a = 0; a < sNum.Length; a++)
            {
                if (new BCW.User.Users().GetUsId() == Convert.ToInt32(sNum[a].Trim()))
                {
                    sbsy++;
                }
            }
            if (sbsy == 0)
            {
                Utils.Error("内测中..你没获得测试的资格，谢谢。", "");
            }
        }

        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "case":
                CasePage();
                break;
            case "caseok":
                CaseOkPage();
                break;
            case "casepost":
                CasePostPage();
                break;
            case "pay":
                PayPage();
                break;
            case "paysave":
                PaySavePage();
                break;
            case "mylist":
                MyListPage();
                break;
            case "help":
                HelpPage();
                break;
            case "list":
                ListPage();
                break;
            case "top":
                TopPage();//排行榜
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        ub xml = new ub();
        xml.ReloadSub(xmlPath); //加载配置

        Master.Title = xml.dss["StkName"].ToString();
        string Logo = xml.dss["StkLogo"].ToString();
        if (Logo != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<img src=\"" + Logo + "\" alt=\"load\"/>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        //拾物随机
        builder.Append(BCW.User.Game.GiftFlows.ShowGiftFlows(6));

        string Notes = xml.dss["StkNotes"].ToString();
        if (Notes != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.SysUBB(Notes) + "");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("stkguess.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        DateTime dt = Convert.ToDateTime("15:30");
        DateTime dt2 = Convert.ToDateTime("9:00");
        //if (DateTime.Now > dt2 && DateTime.Now < dt)
        //{
        string stats = new BCW.Service.GetStk().GetStkXML().Split("+".ToCharArray())[0];
        if (stats.Contains("-"))
            stats = stats.Split("-".ToCharArray())[0];

        builder.Append("即时上证:" + stats + "<br />");
        //}
        //上期开奖
        DataSet ds = new BCW.BLL.Game.Stklist().GetList("WinNum,EndTime", "State=1 ORDER BY ID DESC");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            builder.Append(DT.FormatDate(DateTime.Parse(ds.Tables[0].Rows[0]["EndTime"].ToString()), 4) + "收盘" + ds.Tables[0].Rows[0]["WinNum"].ToString() + "点<br />");
        }
        builder.Append("您目前自带" + Utils.ConvertGold(new BCW.BLL.User().GetGold(meid)) + "" + ub.Get("SiteBz") + "");
        if (Isbz())
            builder.Append("/" + Utils.ConvertGold(new BCW.BLL.User().GetMoney(meid)) + "" + ub.Get("SiteBz2") + "");

        builder.Append("<br /><a href=\"" + Utils.getUrl("stkguess.aspx?act=case") + "\">兑奖</a> | ");
        builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?act=mylist") + "\">记录</a> | ");
        builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?act=help") + "\">规则</a> | ");
        builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?act=top") + "\">排行</a><br />");
        // builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx") + "\">刷新</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?act=list") + "\">上证指数每日收盘</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        BCW.Model.Game.Stklist stk = new BCW.BLL.Game.Stklist().GetStklist();
        //自动开奖
        new BCW.User.Game.Stkguess().StkPage(stk.ID, stk.EndTime, 0, 0);
        //赔率浮动
        OddsPage();

        builder.Append(Out.Tab("<div>", ""));
        if (stk.ID == 0)
        {
            builder.Append("请等待开通下期。。。<br />");
        }
        else
        {
            builder.Append(DT.FormatDate(stk.EndTime, 1) + "上证趣味竞猜<br />");
            //builder.Append("竞猜收盘后个位数,小数点后不计<br />");
            builder.Append("本局押注总量:" + Utils.ConvertGold(stk.Pool) + "" + ub.Get("SiteBz") + "");
            if (Isbz())
                builder.Append("|" + Utils.ConvertGold(stk.WinPool) + "" + ub.Get("SiteBz2") + "");

            if (DateTime.Now < stk.EndTime)
            {
                builder.Append("<br />截止时间:" + DT.FormatDate(stk.EndTime, 1) + "<br />");
                //builder.Append("<table columns=\"2\">");
                //builder.Append("<tr><td>玩法</td><td>赔率</td></tr>");
                //builder.Append("<tr><td>单</td><td><a href=\"" + Utils.getUrl("stkguess.aspx?act=pay&amp;ptype=10") + "\">" + xml.dss["StkOdds10"] + "[竞猜]</a></td></tr>");
                //builder.Append("<tr><td>双</td><td><a href=\"" + Utils.getUrl("stkguess.aspx?act=pay&amp;ptype=11") + "\">" + xml.dss["StkOdds11"] + "[竞猜]</a></td></tr>");
                //builder.Append("<tr><td>大</td><td><a href=\"" + Utils.getUrl("stkguess.aspx?act=pay&amp;ptype=12") + "\">" + xml.dss["StkOdds12"] + "[竞猜]</a></td></tr>");
                //builder.Append("<tr><td>小</td><td><a href=\"" + Utils.getUrl("stkguess.aspx?act=pay&amp;ptype=13") + "\">" + xml.dss["StkOdds13"] + "[竞猜]</a></td></tr>");
                //builder.Append("<tr><td>0</td><td><a href=\"" + Utils.getUrl("stkguess.aspx?act=pay&amp;ptype=0") + "\">" + xml.dss["StkOdds0"] + "[竞猜]</a></td></tr>");
                //builder.Append("<tr><td>1</td><td><a href=\"" + Utils.getUrl("stkguess.aspx?act=pay&amp;ptype=1") + "\">" + xml.dss["StkOdds1"] + "[竞猜]</a></td></tr>");
                //builder.Append("<tr><td>2</td><td><a href=\"" + Utils.getUrl("stkguess.aspx?act=pay&amp;ptype=2") + "\">" + xml.dss["StkOdds2"] + "[竞猜]</a></td></tr>");
                //builder.Append("<tr><td>3</td><td><a href=\"" + Utils.getUrl("stkguess.aspx?act=pay&amp;ptype=3") + "\">" + xml.dss["StkOdds3"] + "[竞猜]</a></td></tr>");
                //builder.Append("<tr><td>4</td><td><a href=\"" + Utils.getUrl("stkguess.aspx?act=pay&amp;ptype=4") + "\">" + xml.dss["StkOdds4"] + "[竞猜]</a></td></tr>");
                //builder.Append("<tr><td>5</td><td><a href=\"" + Utils.getUrl("stkguess.aspx?act=pay&amp;ptype=5") + "\">" + xml.dss["StkOdds5"] + "[竞猜]</a></td></tr>");
                //builder.Append("<tr><td>6</td><td><a href=\"" + Utils.getUrl("stkguess.aspx?act=pay&amp;ptype=6") + "\">" + xml.dss["StkOdds6"] + "[竞猜]</a></td></tr>");
                //builder.Append("<tr><td>7</td><td><a href=\"" + Utils.getUrl("stkguess.aspx?act=pay&amp;ptype=7") + "\">" + xml.dss["StkOdds7"] + "[竞猜]</a></td></tr>");
                //builder.Append("<tr><td>8</td><td><a href=\"" + Utils.getUrl("stkguess.aspx?act=pay&amp;ptype=8") + "\">" + xml.dss["StkOdds8"] + "[竞猜]</a></td></tr>");
                //builder.Append("<tr><td>9</td><td><a href=\"" + Utils.getUrl("stkguess.aspx?act=pay&amp;ptype=9") + "\">" + xml.dss["StkOdds9"] + "[竞猜]</a></td></tr>");
                //builder.Append("<tr><td>合单</td><td><a href=\"" + Utils.getUrl("stkguess.aspx?act=pay&amp;ptype=14") + "\">" + xml.dss["StkOdds14"] + "[竞猜]</a></td></tr>");
                //builder.Append("<tr><td>合双</td><td><a href=\"" + Utils.getUrl("stkguess.aspx?act=pay&amp;ptype=15") + "\">" + xml.dss["StkOdds15"] + "[竞猜]</a></td></tr>");
                //builder.Append("<tr><td>合大</td><td><a href=\"" + Utils.getUrl("stkguess.aspx?act=pay&amp;ptype=16") + "\">" + xml.dss["StkOdds16"] + "[竞猜]</a></td></tr>");
                //builder.Append("<tr><td>合小</td><td><a href=\"" + Utils.getUrl("stkguess.aspx?act=pay&amp;ptype=17") + "\">" + xml.dss["StkOdds17"] + "[竞猜]</a></td></tr>");
                //builder.Append("</table>");


                builder.Append("<table columns=\"2\">");
                // builder.Append("<tr><td></td><td>玩法</td><td>赔率</td></tr>");
                builder.Append("<tr><td>[竞猜]</td><td><a href=\"" + Utils.getUrl("stkguess.aspx?act=pay&amp;ptype=10") + "\">单" + xml.dss["StkOdds10"] + "</a></td>");
                builder.Append("<td>&nbsp;</td><td><a href=\"" + Utils.getUrl("stkguess.aspx?act=pay&amp;ptype=11") + "\">双" + xml.dss["StkOdds11"] + "</a></td></tr>");
                builder.Append("<tr><td>[竞猜]</td><td><a href=\"" + Utils.getUrl("stkguess.aspx?act=pay&amp;ptype=12") + "\">大" + xml.dss["StkOdds12"] + "</a></td>");
                builder.Append("<td>&nbsp;</td><td><a href=\"" + Utils.getUrl("stkguess.aspx?act=pay&amp;ptype=13") + "\">小" + xml.dss["StkOdds13"] + "</a></td></tr>");
                builder.Append("<tr><td>[竞猜]</td><td><a href=\"" + Utils.getUrl("stkguess.aspx?act=pay&amp;ptype=0") + "\">[0]" + xml.dss["StkOdds0"] + "</a></td>");
                builder.Append("<td>&nbsp;</td><td><a href=\"" + Utils.getUrl("stkguess.aspx?act=pay&amp;ptype=1") + "\">[1]" + xml.dss["StkOdds1"] + "</a></td></tr>");
                builder.Append("<tr><td>[竞猜]</td><td><a href=\"" + Utils.getUrl("stkguess.aspx?act=pay&amp;ptype=2") + "\">[2]" + xml.dss["StkOdds2"] + "</a></td>");
                builder.Append("<td>&nbsp;</td><td><a href=\"" + Utils.getUrl("stkguess.aspx?act=pay&amp;ptype=3") + "\">[3]" + xml.dss["StkOdds3"] + "</a></td></tr>");
                builder.Append("<tr><td>[竞猜]</td><td><a href=\"" + Utils.getUrl("stkguess.aspx?act=pay&amp;ptype=4") + "\">[4]" + xml.dss["StkOdds4"] + "</a></td>");
                builder.Append("<td>&nbsp;</td><td><a href=\"" + Utils.getUrl("stkguess.aspx?act=pay&amp;ptype=5") + "\">[5]" + xml.dss["StkOdds5"] + "</a></td></tr>");
                builder.Append("<tr><td>[竞猜]</td><td><a href=\"" + Utils.getUrl("stkguess.aspx?act=pay&amp;ptype=6") + "\">[6]" + xml.dss["StkOdds6"] + "</a></td>");
                builder.Append("<td>&nbsp;</td><td><a href=\"" + Utils.getUrl("stkguess.aspx?act=pay&amp;ptype=7") + "\">[7]" + xml.dss["StkOdds7"] + "</a></td></tr>");
                builder.Append("<tr><td>[竞猜]</td><td><a href=\"" + Utils.getUrl("stkguess.aspx?act=pay&amp;ptype=8") + "\">[8]" + xml.dss["StkOdds8"] + "</a></td>");
                builder.Append("<td>&nbsp;</td><td><a href=\"" + Utils.getUrl("stkguess.aspx?act=pay&amp;ptype=9") + "\">[9]" + xml.dss["StkOdds9"] + "</a></td></tr>");
                builder.Append("<tr><td>[竞猜]</td><td><a href=\"" + Utils.getUrl("stkguess.aspx?act=pay&amp;ptype=14") + "\">合单" + xml.dss["StkOdds14"] + "</a></td>");
                builder.Append("<td>&nbsp;</td><td><a href=\"" + Utils.getUrl("stkguess.aspx?act=pay&amp;ptype=15") + "\">合双" + xml.dss["StkOdds15"] + "</a></td></tr>");
                builder.Append("<tr><td>[竞猜]</td><td><a href=\"" + Utils.getUrl("stkguess.aspx?act=pay&amp;ptype=16") + "\">合大" + xml.dss["StkOdds16"] + "</a></td>");
                builder.Append("<td>&nbsp;</td><td><a href=\"" + Utils.getUrl("stkguess.aspx?act=pay&amp;ptype=17") + "\">合小" + xml.dss["StkOdds17"] + "</a></td></tr>");
                builder.Append("</table>");
            }
            else
            {
                builder.Append("<br />截止时间:今日已截止");
            }
        }
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("〓玩家动态〓");
        builder.Append(Out.Tab("</div>", ""));
        // 开始读取动态列表
        int SizeNum_Action = 3;
        string strWhere_Action = "Types=11";
        IList<BCW.Model.Action> listAction = new BCW.BLL.Action().GetActions(SizeNum_Action, strWhere_Action);
        if (listAction.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Action n in listAction)
            {
                builder.Append(Out.Tab("<div>", "<br />"));
                string ForNotes = Regex.Replace(n.Notes, @"\[url=\/bbs\/game\/([\s\S]+?)\]([\s\S]+?)\[\/url\]", "$2", RegexOptions.IgnoreCase);
                builder.AppendFormat("{0}前{1}", DT.DateDiff2(DateTime.Now, n.AddTime), Out.SysUBB(ForNotes));
                builder.Append(Out.Tab("</div>", ""));
                k++;
            }
            if (k > SizeNum_Action)
            {
                builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("/bbs/action.aspx?ptype=11") + "\">更多动态&gt;&gt;</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("暂无动态记录.");
            builder.Append(Out.Tab("</div>", ""));
        }


        //闲聊显示
        builder.Append(Out.SysUBB(BCW.User.Users.ShowSpeak(12, "stkguess.aspx", 5, 0)));
        //游戏底部Ubb
        string Foot = xml.dss["StkFoot"].ToString();
        if (Foot != "")
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(Foot)));
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void PayPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Master.Title = "上证竞猜押注";
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 2, @"^[0-9]\d*$", "押注类型错误"));
        if (ptype > 17)
        {
            Utils.Error("投注类型错误", "");
        }
        BCW.Model.Game.Stklist stk = new BCW.BLL.Game.Stklist().GetStklist();
        if (stk.ID == 0)
        {
            Utils.Error("请等待开通下期。。。", "");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx") + "\">" + GameName + "</a>&gt;押注");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("" + DT.FormatDate(stk.EndTime, 4) + "上证指数竞猜<br />竞猜:" + OutType(ptype) + "<br />赔率" + ub.GetSub("StkOdds" + ptype + "", xmlPath) + "倍<br />");



        //浮动限制
        long SCent = 0;
        if (ptype >= 0 && ptype <= 9)
        {
            SCent = OutOddsc(ptype);
            long Cent = new BCW.BLL.Game.Stkpay().GetCent(stk.ID, ptype);
            builder.Append("提示:" + ptype + "尾还可以下注" + (SCent - Cent) + "" + ub.Get("SiteBz") + "<br />");
        }
        else if (ptype == 10 || ptype == 11)//单双浮动
        {
            SCent = OutOddsc(10);
            long Cent = new BCW.BLL.Game.Stkpay().GetCent(stk.ID, 10);
            long Cent2 = new BCW.BLL.Game.Stkpay().GetCent(stk.ID, 11);
            if (ptype == 10)
            {
                builder.Append("提示:单数还可以下注" + (SCent + Cent2 - Cent) + "" + ub.Get("SiteBz") + "<br />");
            }
            else
            {
                builder.Append("提示:双数还可以下注" + (Cent + SCent - Cent2) + "" + ub.Get("SiteBz") + "<br />");
            }
        }
        else if (ptype == 12 || ptype == 13)//大小浮动
        {
            SCent = OutOddsc(12);
            long Cent = new BCW.BLL.Game.Stkpay().GetCent(stk.ID, 12);
            long Cent2 = new BCW.BLL.Game.Stkpay().GetCent(stk.ID, 13);
            if (ptype == 12)
            {
                builder.Append("提示:大还可以下注" + (Cent2 + SCent - Cent) + "" + ub.Get("SiteBz") + "<br />");
            }
            else
            {
                builder.Append("提示:小还可以下注" + (Cent + SCent - Cent2) + "" + ub.Get("SiteBz") + "<br />");
            }
        }
        else if (ptype == 14 || ptype == 15)//合单双浮动
        {
            SCent = OutOddsc(14);
            long Cent = new BCW.BLL.Game.Stkpay().GetCent(stk.ID, 14);
            long Cent2 = new BCW.BLL.Game.Stkpay().GetCent(stk.ID, 15);
            if (ptype == 14)
            {
                builder.Append("提示:合单还可以下注" + (Cent2 + SCent - Cent) + "" + ub.Get("SiteBz") + "<br />");
            }
            else
            {
                builder.Append("提示:合双还可以下注" + (Cent + SCent - Cent2) + "" + ub.Get("SiteBz") + "<br />");
            }
        }
        else if (ptype == 16 || ptype == 17)//合大小浮动
        {
            SCent = OutOddsc(16);
            long Cent = new BCW.BLL.Game.Stkpay().GetCent(stk.ID, 16);
            long Cent2 = new BCW.BLL.Game.Stkpay().GetCent(stk.ID, 17);
            if (ptype == 16)
            {
                builder.Append("提示:合大还可以下注" + (Cent2 + SCent - Cent) + "" + ub.Get("SiteBz") + "<br />");
            }
            else
            {
                builder.Append("提示:合小还可以下注" + (Cent + SCent - Cent2) + "" + ub.Get("SiteBz") + "<br />");
            }
        }


        if (Isbz())
            builder.Append("您目前自带:" + Utils.ConvertGold(new BCW.BLL.User().GetGold(meid)) + "" + ub.Get("SiteBz") + "/" + Utils.ConvertGold(new BCW.BLL.User().GetMoney(meid)) + "" + ub.Get("SiteBz2") + "");
        else
            builder.Append("您目前自带:" + Utils.ConvertGold(new BCW.BLL.User().GetGold(meid)) + "" + ub.Get("SiteBz") + "");

        builder.Append(Out.Tab("</div>", "<br />"));
        //快捷键
        string Ps = string.Empty;
        string PsCent = ub.GetSub("BbsPsCent", "/Controls/bbs.xml");
        if (PsCent != "")
        {
            string[] sTemp = PsCent.Split("|".ToCharArray());
            for (int i = 0; i < sTemp.Length; i++)
            {
                if (i % 2 == 0)
                {
                    strName = "paycent,ptype,act,backurl";
                    strValu = "" + sTemp[i] + "'" + ptype + "'paysave'" + Utils.PostPage(1) + "";
                    strOthe = "" + sTemp[i + 1] + ",stkguess.aspx,post,0,other";
                    builder.Append(Out.wapform(strName, strValu, strOthe));
                }
                if ((i + 1) % 8 == 0)
                    builder.Append(Out.Tab("", "<br />"));
                else if (i % 2 != 0 && (i + 1 != sTemp.Length))
                    builder.Append(Out.Tab("", ". "));
            }
            Ps = "或";
        }
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("" + Ps + "输入金额(限" + ub.GetSub("StkSmallPay", xmlPath) + "-" + ub.GetSub("StkBigPay", xmlPath) + ")");
        builder.Append(Out.Tab("</div>", ""));
        strText = ",,,";
        strName = "paycent,ptype,act,backurl";
        strType = "num,hidden,hidden,hidden";
        strValu = "'" + ptype + "'paysave'" + Utils.PostPage(1) + "";
        strEmpt = "false,false,false,false";
        strIdea = "";
        //if (Isbz())
        // strOthe = "押" + ub.Get("SiteBz") + "|押" + ub.Get("SiteBz2") + ",stkguess.aspx,post,0,red|blue";
        //else
        strOthe = "押" + ub.Get("SiteBz") + ",stkguess.aspx,post,0,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx") + "\">返回" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void PaySavePage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string ac = Utils.GetRequest("ac", "post", 1, "", "");
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 2, @"^[0-9]\d*$", "押注类型错误"));
        if (ptype > 17)
        {
            Utils.Error("投注类型错误", "");
        }
        long paycent = Int64.Parse(Utils.GetRequest("paycent", "post", 4, @"^[0-9]\d*$", "押注金额错误"));
        //得到赔率
        decimal odds = Convert.ToDecimal(ub.GetSub("StkOdds" + ptype + "", xmlPath));

        //是否刷屏
        string appName = "LIGHT_STKGUESS";
        int Expir = Utils.ParseInt(ub.GetSub("StkExpir", xmlPath));


        BCW.Model.Game.Stklist stk = new BCW.BLL.Game.Stklist().GetStklist();
        if (stk.ID == 0)
        {
            Utils.Error("请等待开通下期。。。", "");
        }

        //时间限制
        if (DateTime.Now >= stk.EndTime)
        {
            Utils.Error("投注截止时间已到，欢迎下期再押注", "");
        }


        int bzType = 0;
        string bzText = string.Empty;
        long gold = 0;
        if (Utils.ToSChinese(ac).Contains(Utils.ToSChinese(ub.Get("SiteBz2"))))
        {
            bzType = 1;
            bzText = ub.Get("SiteBz2");
            gold = new BCW.BLL.User().GetMoney(meid);
            BCW.User.Users.IsFresh(appName, Expir);
            if (paycent < Convert.ToInt64(ub.GetSub("StkSmallPay", xmlPath)) || paycent > Convert.ToInt64(ub.GetSub("StkBigPay", xmlPath)))
            {
                Utils.Error("押注金额限" + ub.GetSub("StkSmallPay", xmlPath) + "-" + ub.GetSub("StkBigPay", xmlPath) + "" + bzText + "", "");
            }
        }
        else
        {

            //支付安全提示
            string[] p_pageArr = { "act", "ptype", "paycent", "ac" };
            BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr);

            bzType = 0;
            bzText = ub.Get("SiteBz");
            gold = new BCW.BLL.User().GetGold(meid);
            long small = Convert.ToInt64(ub.GetSub("StkSmallPay", xmlPath));
            long big = Convert.ToInt64(ub.GetSub("StkBigPay", xmlPath));
            BCW.User.Users.IsFresh(appName, Expir, paycent, small, big);
        }
        if (paycent > gold)
        {
            Utils.Error("你的" + bzText + "不足", Utils.getPage("stkguess.aspx"));
        }

        long xPrices = Utils.ParseInt64(ub.GetSub("StkMaxpay", xmlPath));
        if (xPrices > 0)
        {
            long oPrices = new BCW.BLL.Game.Stkpay().GetSumPrices(meid, stk.ID);
            if (oPrices + paycent > xPrices)
            {
                if (oPrices >= xPrices)
                    Utils.Error("您本期下注已达上限，请等待下期...", "");
                else
                    Utils.Error("您本期最多还可以下注" + (xPrices - oPrices) + "" + ub.Get("SiteBz") + "", "");
            }
        }

        #region 每期每玩法每ID投注上限
        long ptyPrices = 0;
        try
        {
            ptyPrices = Convert.ToInt64(OutOddscid(ptype));
        }
        catch { ptyPrices = 0; }
        if (ptyPrices > 0)
        {
            long oPrices = new BCW.BLL.Game.Stkpay().GetSumPrices(meid, stk.ID, ptype);
            if (oPrices + paycent > ptyPrices)
            {
                if (oPrices >= ptyPrices)
                    Utils.Error("您本期竞猜" + OutType(ptype) + "下注已达上限，请选择其他下注或等待下期...", "");
                else
                    Utils.Error("您本期竞猜" + OutType(ptype) + "最多还可以下注" + (ptyPrices - oPrices) + "" + ub.Get("SiteBz") + "测试", "");
            }
        }
        #endregion

        #region //浮动限制
        long xPricesc = 0;
        if (ptype < 10)
        {
            xPricesc = OutOddsc(ptype);
            if (xPricesc > 0)
            {
                long oPricesc = new BCW.BLL.Game.Stkpay().GetSumPricesbytype(ptype, stk.ID);
                if (oPricesc + paycent > xPricesc)
                {
                    if (oPricesc > xPricesc)
                        Utils.Error("本期" + OutType(ptype) + "下注已达上限，请等待下期或者选择其他投注...", "");
                    else
                        Utils.Error("本期" + OutType(ptype) + "最多还可以下注" + (xPricesc - oPricesc) + ub.Get("SiteBz") + "", "");
                }
            }
        }
        else if (ptype == 10 || ptype == 11)//单双
        {
            xPricesc = OutOddsc(10);
            long Cent = new BCW.BLL.Game.Stkpay().GetSumPricesbytype(10, stk.ID);
            long Cent2 = new BCW.BLL.Game.Stkpay().GetSumPricesbytype(11, stk.ID);

            if (ptype == 10)
            {
                if (Math.Abs(Cent + paycent - Cent2) > xPricesc)
                {
                    long hk = xPricesc + Cent2 - Cent;
                    Utils.Error("单数超出系统投注币额，押单还可投注" + hk + ub.Get("SiteBz") + "，或者请选择其他投注", "");
                }
            }
            if (ptype == 11)
            {
                if (Math.Abs(Cent2 + paycent - Cent) > xPricesc)
                {
                    long hk = xPricesc + Cent - Cent2;
                    Utils.Error("双数超出系统投注币额，押双还可投注" + hk + ub.Get("SiteBz") + "，或者请选择其他投注", "");
                }
            }
        }
        else if (ptype == 12 || ptype == 13)//大小
        {
            xPricesc = OutOddsc(12);
            long Cent = new BCW.BLL.Game.Stkpay().GetSumPricesbytype(12, stk.ID);
            long Cent2 = new BCW.BLL.Game.Stkpay().GetSumPricesbytype(13, stk.ID);

            if (ptype == 12)
            {
                if (Math.Abs(Cent + paycent - Cent2) > xPricesc)
                {
                    long hk = xPricesc + Cent2 - Cent;
                    Utils.Error("大超出系统投注币额，押大还可投注" + hk + ub.Get("SiteBz") + "，或者请选择其他投注", "");
                }
            }
            if (ptype == 13)
            {
                if (Math.Abs(Cent2 + paycent - Cent) > xPricesc)
                {
                    long hk = xPricesc + Cent - Cent2;
                    Utils.Error("小超出系统投注币额，押小还可投注" + hk + ub.Get("SiteBz") + "，或者请选择其他投注", "");
                }
            }
        }
        else if (ptype == 14 || ptype == 15)//合单双
        {
            xPricesc = OutOddsc(14);
            long Cent = new BCW.BLL.Game.Stkpay().GetSumPricesbytype(14, stk.ID);
            long Cent2 = new BCW.BLL.Game.Stkpay().GetSumPricesbytype(15, stk.ID);

            if (ptype == 14)
            {
                if (Math.Abs(Cent + paycent - Cent2) > xPricesc)
                {
                    long hk = xPricesc + Cent2 - Cent;
                    Utils.Error("合单数超出系统投注币额，押合单还可投注" + hk + ub.Get("SiteBz") + "，或者请选择其他投注", "");
                }
            }
            if (ptype == 15)
            {
                if (Math.Abs(Cent2 + paycent - Cent) > xPricesc)
                {
                    long hk = xPricesc + Cent - Cent2;
                    Utils.Error("合双数超出系统投注币额，押合双还可投注" + hk + ub.Get("SiteBz") + "，或者请选择其他投注", "");
                }
            }
        }
        else if (ptype == 16 || ptype == 17)//合大小
        {
            xPricesc = OutOddsc(16);
            long Cent = new BCW.BLL.Game.Stkpay().GetSumPricesbytype(16, stk.ID);
            long Cent2 = new BCW.BLL.Game.Stkpay().GetSumPricesbytype(17, stk.ID);

            if (ptype == 16)
            {
                if (Math.Abs(Cent + paycent - Cent2) > xPricesc)
                {
                    long hk = xPricesc + Cent2 - Cent;
                    Utils.Error("合大超出系统投注币额，押合大还可投注" + hk + ub.Get("SiteBz") + "，或者请选择其他投注", "");
                }
            }
            if (ptype == 17)
            {
                if (Math.Abs(Cent2 + paycent - Cent) > xPricesc)
                {
                    long hk = xPricesc + Cent - Cent2;
                    Utils.Error("合小超出系统投注币额，押合小还可投注" + hk + ub.Get("SiteBz") + "，或者请选择其他投注", "");
                }
            }
        }
        #endregion

        string mename = new BCW.BLL.User().GetUsName(meid);

        BCW.Model.Game.Stkpay model = new BCW.Model.Game.Stkpay();
        model.StkId = stk.ID;
        model.UsID = meid;
        model.UsName = mename;
        model.Types = ptype;
        model.WinNum = 0;
        model.Odds = odds;
        model.BuyCent = paycent;
        model.WinCent = 0;
        model.AddTime = DateTime.Now;//stk.EndTime
        model.State = 0;
        model.bzType = bzType;
        model.isSpier = 0;
        int StkpayID = 0;
        int id = 0;
        //if (!new BCW.BLL.Game.Stkpay().Exists(stk.ID, meid, bzType, ptype, odds))
        //    id = StkpayID = new BCW.BLL.Game.Stkpay().Add(model);
        //else
        //    new BCW.BLL.Game.Stkpay().Update(model);
        id = StkpayID = new BCW.BLL.Game.Stkpay().Add(model);

        //加总押注额
        if (bzType == 0)
        {
            new BCW.BLL.Game.Stklist().UpdatePool(stk.ID, paycent);
            new BCW.BLL.User().UpdateiGold(meid, mename, -paycent, "在-[url=/bbs/game/stkguess.aspx]" + GameName + "[/url]" + stk.ID + "(" + stk.EndTime.ToString("MM-dd") + ")-|押" + OutType(ptype) + "|赔率:" + odds + "|标识ID:" + StkpayID + "");//
            if (new BCW.BLL.User().GetIsSpier(meid) != 1)
                new BCW.BLL.User().UpdateiGold(109, new BCW.BLL.User().GetUsName(109), paycent, "ID:[url=forumlog.aspx?act=xview&amp;uid=" + meid + "]" + meid + "[/url]" + GameName + "第[url=./game/stkguess.aspx?act=view&amp;id=" + stk.ID + "]" + stk.ID + "(" + stk.EndTime.ToString("MM-dd") + ")" + "[/url]期竞猜[" + OutType(ptype) + "]下注" + paycent + bzText + "|赔率" + odds + "-标识ID" + id + "");// model.ID

        }
        else
        {
            new BCW.BLL.Game.Stklist().UpdateWinPool(stk.ID, paycent);
            new BCW.BLL.User().UpdateiMoney(meid, mename, -paycent, "在-[url=/bbs/game/stkguess.aspx]" + GameName + "[/url]" + stk.ID + "(" + stk.EndTime.ToString("MM-dd") + ")-|押" + OutType(ptype) + "|赔率:" + odds + "|标识ID:" + StkpayID + "");
            if (new BCW.BLL.User().GetIsSpier(meid) != 1)
                new BCW.BLL.User().UpdateiMoney(109, new BCW.BLL.User().GetUsName(109), paycent, "ID:[url=forumlog.aspx?act=xview&amp;uid=" + meid + "]" + meid + "[/url]" + GameName + "第[url=./game/stkguess.aspx?act=view&amp;id=" + stk.ID + "]" + stk.ID + "(" + stk.EndTime.ToString("MM-dd") + ")" + "[/url]期竞猜[" + OutType(ptype) + "]下注" + paycent + bzText + "|赔率" + odds + "-标识ID" + id + "");

        }

        string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/stkguess.aspx]" + GameName + "[/url]竞猜押注" + "**" + "" + bzText + "";//[" + OutType(ptype) + "]
        new BCW.BLL.Action().Add(11, id, meid, "", wText);
        //活跃抽奖入口_20160621姚志光
        try
        {
            //表中存在记录
            if (new BCW.BLL.tb_WinnersGame().ExistsGameName("上证指数"))
            {
                //投注是否大于设定的限额，是则有抽奖机会
                if (paycent > new BCW.BLL.tb_WinnersGame().GetPrice("上证指数"))
                {
                    string TextForUbb = (ub.GetSub("TextForUbb", "/Controls/winners.xml"));//设置内线提示的文字
                    string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", "/Controls/winners.xml"));//1发内线2不发内线 
                    int hit = new BCW.winners.winners().CheckActionForAll(1, 1, meid, mename, "上证", 3);
                    if (hit == 1)
                    {
                        //内线开关 1开
                        if (WinnersGuessOpen == "1")
                        {
                            //发内线到该ID
                            new BCW.BLL.Guest().Add(0, meid, mename, TextForUbb);
                        }
                    }
                }
            }
        }
        catch { }
        Utils.Success("押注", "押注成功，花费了" + paycent + "" + bzText + "<br /><a href=\"" + Utils.getPage("stkguess.aspx?act=pay&amp;ptype=" + ptype + "") + "\">&gt;继续下注</a>", Utils.getUrl("stkguess.aspx"), "2");
    }

    private void HelpPage()
    {
        Master.Title = "" + GameName + "规则";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("stkguess.aspx") + "\">" + GameName + "</a>&gt;规则");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.SysUBB(ub.GetSub("StkRule", xmlPath)));
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void MyListPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[1-2]$", "1"));
        string strTitle = "";
        if (ptype == 1)
            strTitle = "未开押注";
        else
            strTitle = "历史押注";

        Master.Title = "我的" + strTitle;

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx") + "\">" + GameName + "</a>&gt;" + strTitle + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = 10;
        string strWhere = string.Empty;
        strWhere = "UsID=" + meid + "";
        if (ptype == 1)
            strWhere += " and State=0";
        else
            strWhere += " and State>0";

        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Game.Stkpay> listStkpay = new BCW.BLL.Game.Stkpay().GetStkpays(pageIndex, pageSize, strWhere, out recordCount);
        if (listStkpay.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Stkpay n in listStkpay)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".");
                builder.Append(DT.FormatDate(n.AddTime, 4) + "上证指数竞猜");

                builder.Append("/竞猜:" + OutType(n.Types) + "/标识ID:" + n.ID);
                builder.Append("/赔率:" + Convert.ToDouble(n.Odds) + "");
                string bzTypes = string.Empty;
                if (n.bzType == 0)
                    bzTypes = ub.Get("SiteBz");
                else
                    bzTypes = ub.Get("SiteBz2");

                builder.Append("/下注:" + n.BuyCent + "" + bzTypes + "");
                builder.Append("/开奖结果:");
                if (n.State == 0)
                {
                    builder.Append("未开奖");
                }
                else
                {
                    builder.Append("收盘" + n.WinNum + "点");
                    if (n.WinCent > 0)
                    {
                        builder.Append("/赢" + n.WinCent + "" + bzTypes + "");
                    }
                }
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?act=mylist&amp;ptype=1") + "\">未开押注</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?act=mylist&amp;ptype=2") + "\">历史押注</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append(" <a href=\"" + Utils.getUrl("stkguess.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ListPage()
    {
        Master.Title = "上证指数每日收盘";

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("stkguess.aspx") + "\">" + GameName + "</a>&gt;上证指数每日收盘");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("上证指数每日收盘");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        strWhere = "State=1";

        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Game.Stklist> listStklist = new BCW.BLL.Game.Stklist().GetStklists(pageIndex, pageSize, strWhere, out recordCount);
        if (listStklist.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Stklist n in listStklist)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }
                builder.Append(DT.FormatDate(n.EndTime, 4) + " 收盘" + n.WinNum + "点");
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?act=mylist&amp;ptype=1") + "\">未开投注</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?act=mylist&amp;ptype=2") + "\">历史投注</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append(" <a href=\"" + Utils.getUrl("stkguess.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void CaseOkPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int pid = Utils.ParseInt(Utils.GetRequest("pid", "get", 2, @"^[0-9]\d*$", "选择兑奖无效"));

        BCW.Model.Game.Stkpay n = new BCW.BLL.Game.Stkpay().GetStkpay(pid);
        int stklistid = n.StkId;
        DateTime endtime = new BCW.BLL.Game.Stklist().GetEndTime(stklistid);//上证截止日期
        BCW.Model.Game.Stklist stk = new BCW.BLL.Game.Stklist().GetStklist(stklistid);
        BCW.User.Users.IsFresh("stkguess", 1);//防刷

        if (new BCW.BLL.Game.Stkpay().ExistsState(pid, meid))
        {
            new BCW.BLL.Game.Stkpay().UpdateState(pid);
            //操作币
            long winMoney = Convert.ToInt64(new BCW.BLL.Game.Stkpay().GetWinCent(pid));
            int bzType = new BCW.BLL.Game.Stkpay().GetbzType(pid);
            //税率
            long SysTax = 0;
            int Tax = Utils.ParseInt(ub.GetSub("StkTax", xmlPath));
            if (Tax > 0)
            {
                SysTax = Convert.ToInt64(winMoney * Tax * 0.01);
            }
            winMoney = winMoney - SysTax;
            if (bzType == 0)
            {
                new BCW.BLL.User().UpdateiGold(meid, winMoney, "[url=./game/stkguess.aspx]" + GameName + "[/url](" + endtime.ToString("MM-dd") + ")兑奖-标识ID" + pid + "");
                if (new BCW.BLL.User().GetIsSpier(meid) != 1)
                    new BCW.BLL.User().UpdateiGold(109, new BCW.BLL.User().GetUsName(109), -winMoney, "ID:[url=forumlog.aspx?act=xview&amp;uid=" + meid + "]" + meid + "[/url]" + GameName + "第[url=./game/stkguess.aspx?act=view&amp;id=" + stk.ID + "&amp;ptype=2]" + stk.ID + "(" + endtime.ToString("MM-dd") + ")" + "[/url]期兑奖" + winMoney + "|（标识ID" + pid + "）");
                Utils.Success("兑奖", "恭喜，成功兑奖" + winMoney + "" + ub.Get("SiteBz") + "", Utils.getUrl("stkguess.aspx?act=case"), "1");
            }
            else
            {
                new BCW.BLL.User().UpdateiMoney(meid, winMoney, "[url=./game/stkguess.aspx]" + GameName + "[/url](" + endtime.ToString("MM-dd") + ")兑奖-标识ID" + pid + "");
                if (new BCW.BLL.User().GetIsSpier(meid) != 1)
                    new BCW.BLL.User().UpdateiMoney(109, new BCW.BLL.User().GetUsName(109), -winMoney, "ID:[url=forumlog.aspx?act=xview&amp;uid=" + meid + "]" + meid + "[/url]" + GameName + "第[url=./game/stkguess.aspx?act=view&amp;id=" + stk.ID + "&amp;ptype=2]" + stk.ID + "(" + endtime.ToString("MM-dd") + ")" + "[/url]期兑奖" + winMoney + "|（标识ID" + pid + "）");
                Utils.Success("兑奖", "恭喜，成功兑奖" + winMoney + "" + ub.Get("SiteBz2") + "", Utils.getUrl("stkguess.aspx?act=case"), "1");
            }

        }
        else
        {
            Utils.Success("兑奖", "恭喜，重复兑奖或没有可以兑奖的记录", Utils.getUrl("stkguess.aspx?act=case"), "1");
        }
    }

    private void CasePostPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string arrId = "";
        arrId = Utils.GetRequest("arrId", "post", 1, "", "");
        if (!Utils.IsRegex(arrId.Replace(",", ""), @"^[0-9]\d*$"))
        {
            Utils.Error("选择本页兑奖出错", "");
        }
        BCW.User.Users.IsFresh("stkguess", 1);//防刷
        string[] strArrId = arrId.Split(",".ToCharArray());
        long winMoney = 0;
        long winMoney2 = 0;
        for (int i = 0; i < strArrId.Length; i++)
        {
            int pid = Convert.ToInt32(strArrId[i]);
            if (new BCW.BLL.Game.Stkpay().ExistsState(pid, meid))
            {
                new BCW.BLL.Game.Stkpay().UpdateState(pid);

                BCW.Model.Game.Stkpay n = new BCW.BLL.Game.Stkpay().GetStkpay(pid);
                int stklistid = n.StkId;
                DateTime endtime = new BCW.BLL.Game.Stklist().GetEndTime(stklistid);//上证截止日期
                BCW.Model.Game.Stklist stk = new BCW.BLL.Game.Stklist().GetStklist(stklistid);

                //操作币
                long win = Convert.ToInt64(new BCW.BLL.Game.Stkpay().GetWinCent(pid));
                int bzType = new BCW.BLL.Game.Stkpay().GetbzType(pid);
                //税率
                long SysTax = 0;
                int Tax = Utils.ParseInt(ub.GetSub("StkTax", xmlPath));
                if (Tax > 0)
                {
                    SysTax = Convert.ToInt64(win * Tax * 0.01);
                }
                win = win - SysTax;
                if (bzType == 0)
                {
                    winMoney += win - SysTax;
                    new BCW.BLL.User().UpdateiGold(meid, win, "[url=./game/stkguess.aspx]" + GameName + "[/url](" + endtime.ToString("MM-dd") + ")兑奖-标识ID" + pid + "");
                    if (new BCW.BLL.User().GetIsSpier(meid) != 1)
                        new BCW.BLL.User().UpdateiGold(109, new BCW.BLL.User().GetUsName(109), -win, "ID:[url=forumlog.aspx?act=xview&amp;uid=" + meid + "]" + meid + "[/url]" + GameName + "第[url=./game/stkguess.aspx?act=view&amp;id=" + stk.ID + "&amp;ptype=2]" + stk.ID + "(" + endtime.ToString("MM-dd") + ")" + "[/url]期兑奖" + win + "|（标识ID" + pid + "）");

                }
                else
                {
                    winMoney2 += win - SysTax;
                    new BCW.BLL.User().UpdateiMoney(meid, win, "[url=./game/stkguess.aspx]" + GameName + "[/url](" + endtime.ToString("MM-dd") + ")兑奖-标识ID" + pid + "");
                    if (new BCW.BLL.User().GetIsSpier(meid) != 1)
                        new BCW.BLL.User().UpdateiMoney(109, new BCW.BLL.User().GetUsName(109), -win, "ID:[url=forumlog.aspx?act=xview&amp;uid=" + meid + "]" + meid + "[/url]" + GameName + "第[url=./game/stkguess.aspx?act=view&amp;id=" + stk.ID + "&amp;ptype=2]" + stk.ID + "(" + endtime.ToString("MM-dd") + ")" + "[/url]期兑奖" + win + "|（标识ID" + pid + "）");

                }
            }
        }
        Utils.Success("兑奖", "恭喜，成功兑奖" + winMoney + "" + ub.Get("SiteBz") + "/" + winMoney2 + "" + ub.Get("SiteBz2") + "", Utils.getUrl("stkguess.aspx?act=case"), "1");
    }

    private void CasePage()
    {
        Master.Title = "兑奖中心";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("stkguess.aspx") + "\">" + GameName + "</a>&gt;兑奖中心");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("您现在有" + Utils.ConvertGold(new BCW.BLL.User().GetGold(meid)) + "" + ub.Get("SiteBz") + "");
        if (Isbz())
            builder.Append("/" + Utils.ConvertGold(new BCW.BLL.User().GetMoney(meid)) + "" + ub.Get("SiteBz2") + "");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = 10;
        string strWhere = string.Empty;
        strWhere = "UsID=" + meid + " and WinCent>0 and State=1";
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        string arrId = "";
        // 开始读取列表
        IList<BCW.Model.Game.Stkpay> listStkpay = new BCW.BLL.Game.Stkpay().GetStkpays(pageIndex, pageSize, strWhere, out recordCount);
        if (listStkpay.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Stkpay n in listStkpay)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".");
                builder.Append(DT.FormatDate(n.AddTime, 4) + "上证指数竞猜");
                builder.AppendFormat("/标识ID:" + n.ID);
                builder.Append("/竞猜:" + OutType(n.Types) + "");
                builder.Append("/赔率:" + Convert.ToDouble(n.Odds) + "");
                string bzTypes = string.Empty;
                if (n.bzType == 0)
                    bzTypes = ub.Get("SiteBz");
                else
                    bzTypes = ub.Get("SiteBz2");

                builder.Append("/下注:" + n.BuyCent + "" + bzTypes + "");
                builder.Append("/开奖结果:收盘" + n.WinNum + "点");
                builder.Append("/赢" + n.WinCent + "" + bzTypes + "");
                builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?act=caseok&amp;pid=" + n.ID + "") + "\">兑奖</a>");
                arrId = arrId + " " + n.ID;
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        if (!string.IsNullOrEmpty(arrId))
        {
            builder.Append(Out.Tab("", "<br />"));
            arrId = arrId.Trim();
            arrId = arrId.Replace(" ", ",");
            string strName = "arrId,act";
            string strValu = "" + arrId + "'casepost";
            string strOthe = "本页兑奖,stkguess.aspx,post,0,red";
            builder.Append(Out.wapform(strName, strValu, strOthe));
        }
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?act=mylist&amp;ptype=1") + "\">未开投注</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?act=mylist&amp;ptype=2") + "\">历史投注</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append(" <a href=\"" + Utils.getUrl("stkguess.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    #region 排行榜 TopPage
    /// <summary>
    /// 排行榜
    /// </summary>
    private void TopPage()
    {
        Master.Title = "" + GameName + "排行榜";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx") + "\">" + GameName + "</a>&gt;排行榜");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("" + GameName + "排行榜");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;

        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-2]\d*$", "0"));
        builder.Append(Out.Tab("<div>", ""));
        if (ptype == 0)
        {
            strWhere = " State>0 ";
            builder.Append("总榜 | <a href=\"" + Utils.getUrl("stkguess.aspx?act=top&amp;ptype=1") + "\">周榜</a> | <a href=\"" + Utils.getUrl("stkguess.aspx?act=top&amp;ptype=2") + "\">日榜</a>");
        }
        else if (ptype == 1)
        {
            strWhere = " datediff(WEEK,AddTime,getdate())=0 and  State>0 ";
            builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?act=top&amp;ptype=0") + "\">总榜</a> | 周榜 | <a href=\"" + Utils.getUrl("stkguess.aspx?act=top&amp;ptype=2") + "\">日榜</a>");
        }
        else
        {
            strWhere = " datediff(DAY,AddTime,getdate())=0  and State>0 ";
            builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx?act=top&amp;ptype=0") + "\">总榜</a> | <a href=\"" + Utils.getUrl("stkguess.aspx?act=top&amp;ptype=1") + "\">周榜</a> | 日榜");
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        if (pageIndex >= 10)
            pageIndex = 10;
        // 开始读取列表
        IList<BCW.Model.Game.Stkpay> listStkpay = new BCW.BLL.Game.Stkpay().GetStkpaysTop(pageIndex, pageSize, strWhere, out recordCount);
        if (listStkpay.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Stkpay n in listStkpay)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }
                builder.Append("[第" + ((pageIndex - 1) * pageSize + k) + "名]<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(n.UsID) + "(" + n.UsID + ")</a>赢" + n.WinCent + "" + ub.Get("SiteBz") + "");
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }

            int Pcount = 0;
            if (recordCount <= 100)
            {
                Pcount = recordCount;
            }
            else
            {
                Pcount = 100;
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, Pcount, Utils.getPageUrl(), pageValUrl, "page", 0));

        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("stkguess.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 获得类型对应名字 OutType
    private string OutType(int Types)
    {
        string Retn = string.Empty;
        if (Types >= 10)
        {
            if (Types == 10)
                Retn = "单";
            else if (Types == 11)
                Retn = "双";
            else if (Types == 12)
                Retn = "大";
            else if (Types == 13)
                Retn = "小";
            else if (Types == 14)
                Retn = "合单";
            else if (Types == 15)
                Retn = "合双";
            else if (Types == 16)
                Retn = "合大";
            else if (Types == 17)
                Retn = "合小";
        }
        else
        {
            Retn = Types.ToString();
        }
        return Retn;
    }
    #endregion

    #region 投注上限 OutOddsc
    /// <summary>
    /// 投注上限
    /// </summary>
    /// <param name="Types"></param>
    /// <returns></returns>
    private long OutOddsc(int Types)
    {
        string toppay = ub.GetSub("StkToppay", xmlPath);
        long pText = 0;
        string[] toppay1 = toppay.Split('|');
        pText = Convert.ToInt64(toppay1[Types]);

        return pText;
    }
    #endregion

    #region 投注上限 OutOddscid
    /// <summary>
    /// 投注上限
    /// </summary>
    /// <param name="Types"></param>
    /// <returns></returns>
    private long OutOddscid(int Types)
    {
        string toppay = ub.GetSub("StkPayID", xmlPath);
        long pText = 0;
        string[] toppay1 = toppay.Split('|');
        pText = Convert.ToInt64(toppay1[Types]);

        return pText;
    }
    #endregion

    //大小单双赔率浮动
    private void OddsPage()
    {
        #region 大小单双赔率浮动
        try
        {
            ub xml = new ub();
            Application.Remove(xmlPath);//清缓存
            xml.ReloadSub(xmlPath); //加载配置
            int lk = Convert.ToInt32(ub.GetSub("Stkfushu", xmlPath));
            string strWheres = string.Empty;
            strWheres += "State = 1 order by ID desc";
            DataSet ds = new BCW.BLL.Game.Stklist().GetList("WinNum", strWheres);

            string result1 = ds.Tables[0].Rows[0]["WinNum"].ToString();
            int re1 = Convert.ToInt32(result1.Substring(0, 1));//千位
            int re2 = Convert.ToInt32(result1.Substring(1, 1));//百位
            int re3 = Convert.ToInt32(result1.Substring(2, 1));//十位
            int re4 = Convert.ToInt32(result1.Substring(3, 1));//个位

            string sum1 = (re1 + re2 + re3 + re4).ToString();//和
            int sum = Convert.ToInt32(sum1.Substring(sum1.Length - 1, 1));

            #region 大小、单双
            #region 大
            if (re4 > 4)
            {
                bool s1 = true;
                int count1 = 1;
                while (s1)
                {
                    string Result = ds.Tables[0].Rows[count1]["WinNum"].ToString();

                    if (Convert.ToInt32(Result.Substring(3, 1)) > 4)
                        count1++;
                    else
                        s1 = false;
                }
                if (count1 <= lk)
                {
                    string ptype = ub.GetSub("StkDX", xmlPath);
                    string[] ptype2f = ptype.Split('|');
                    xml.dss["StkOdds12"] = ptype2f[0];
                    xml.dss["StkOdds13"] = ptype2f[0];
                }
                else
                {
                    string ptype = ub.GetSub("StkDX", xmlPath);
                    string[] ptype2f = ptype.Split('|');
                    double oddschushi = Convert.ToDouble(ptype2f[0]);
                    double Odds = Convert.ToDouble(ptype2f[1]);
                    double oddsmax = Convert.ToDouble(ptype2f[2]);
                    double oddsmin = Convert.ToDouble(ptype2f[3]);
                    double oddschushi2 = oddschushi * 2;
                    string Da = string.Empty; string Xiao = string.Empty;
                    if (oddschushi + Odds * (count1 - lk) > oddsmax || oddschushi + Odds * (count1 - lk) < oddsmin)
                    {
                        if (oddschushi + Odds * (count1 - lk) > oddsmax) { Da = oddsmax.ToString(); Xiao = (oddschushi2 - oddsmax).ToString(); }
                        if (oddschushi + Odds * (count1 - lk) < oddsmin) { Da = oddsmin.ToString(); Xiao = (oddschushi2 - oddsmin).ToString(); }
                        xml.dss["StkOdds12"] = Da;
                        xml.dss["StkOdds13"] = Xiao;
                    }
                    else
                    {
                        Da = (oddschushi + Odds * (count1 - lk)).ToString();
                        Xiao = (oddschushi2 - (oddschushi + Odds * (count1 - lk))).ToString();
                        xml.dss["StkOdds12"] = Da;
                        xml.dss["StkOdds13"] = Xiao;
                    }
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #region 小
            else
            {
                bool s1 = true;
                int count1 = 1;
                while (s1)
                {
                    string Result = ds.Tables[0].Rows[count1]["WinNum"].ToString();

                    if (Convert.ToInt32(Result.Substring(3, 1)) > 4)
                        s1 = false;
                    else
                        count1++;
                }
                if (count1 <= lk)
                {
                    string ptype = ub.GetSub("StkDX", xmlPath);
                    string[] ptype2f = ptype.Split('|');
                    xml.dss["StkOdds12"] = ptype2f[0];
                    xml.dss["StkOdds13"] = ptype2f[0];
                }
                else
                {
                    string ptype = ub.GetSub("StkDX", xmlPath);
                    string[] ptype2f = ptype.Split('|');
                    double oddschushi = Convert.ToDouble(ptype2f[0]);
                    double Odds = Convert.ToDouble(ptype2f[1]);
                    double oddsmax = Convert.ToDouble(ptype2f[2]);
                    double oddsmin = Convert.ToDouble(ptype2f[3]);
                    double oddschushi2 = oddschushi * 2;
                    string Da = string.Empty; string Xiao = string.Empty;
                    if (oddschushi + Odds * (count1 - lk) > oddsmax || oddschushi + Odds * (count1 - lk) < oddsmin)
                    {
                        if (oddschushi + Odds * (count1 - lk) > oddsmax) { Xiao = oddsmax.ToString(); Da = (oddschushi2 - oddsmax).ToString(); }
                        if (oddschushi + Odds * (count1 - lk) < oddsmin) { Xiao = oddsmin.ToString(); Da = (oddschushi2 - oddsmin).ToString(); }
                        xml.dss["StkOdds12"] = Da;
                        xml.dss["StkOdds13"] = Xiao;
                    }
                    else
                    {
                        Xiao = (oddschushi + Odds * (count1 - lk)).ToString();
                        Da = (oddschushi2 - (oddschushi + Odds * (count1 - lk))).ToString();
                        xml.dss["StkOdds12"] = Da;
                        xml.dss["StkOdds13"] = Xiao;
                    }
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #region 单
            if (re3 % 2 != 0)
            {
                bool s2 = true;
                int count2 = 1;
                while (s2)
                {
                    string Result = ds.Tables[0].Rows[count2]["WinNum"].ToString();
                    if (Convert.ToInt32(Result.Substring(3, 1)) % 2 != 0)
                        count2++;
                    else
                        s2 = false;
                }
                if (count2 <= lk)
                {
                    string ptype = ub.GetSub("StkDS", xmlPath);
                    string[] ptype2f = ptype.Split('|');
                    xml.dss["StkOdds10"] = ptype2f[0];
                    xml.dss["StkOdds11"] = ptype2f[0];
                }
                else
                {
                    string ptype = ub.GetSub("StkDS", xmlPath);
                    string[] ptype2f = ptype.Split('|');
                    double oddschushi = Convert.ToDouble(ptype2f[0]);
                    double Odds = Convert.ToDouble(ptype2f[1]);
                    double oddsmax = Convert.ToDouble(ptype2f[2]);
                    double oddsmin = Convert.ToDouble(ptype2f[3]);
                    double oddschushi2 = oddschushi * 2;
                    string Da = string.Empty; string Xiao = string.Empty;
                    if (oddschushi + Odds * (count2 - lk) > oddsmax || oddschushi + Odds * (count2 - lk) < oddsmin)
                    {
                        if (oddschushi + Odds * (count2 - lk) > oddsmax) { Da = oddsmax.ToString(); Xiao = (oddschushi2 - oddsmax).ToString(); }
                        if (oddschushi + Odds * (count2 - lk) < oddsmin) { Da = oddsmin.ToString(); Xiao = (oddschushi2 - oddsmin).ToString(); }
                        xml.dss["StkOdds10"] = Da;
                        xml.dss["StkOdds11"] = Xiao;
                    }
                    else
                    {
                        Da = (oddschushi + Odds * (count2 - lk)).ToString();
                        Xiao = (oddschushi2 - (oddschushi + Odds * (count2 - lk))).ToString();
                        xml.dss["StkOdds10"] = Da;
                        xml.dss["StkOdds11"] = Xiao;
                    }
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #region 双
            else
            {
                bool s2 = true;
                int count2 = 1;
                while (s2)
                {
                    string Result = ds.Tables[0].Rows[count2]["WinNum"].ToString();
                    if (Convert.ToInt32(Result.Substring(3, 1)) % 2 != 0)
                        s2 = false;
                    else
                        count2++;
                }
                if (count2 <= lk)
                {
                    string ptype = ub.GetSub("StkDS", xmlPath);
                    string[] ptype2f = ptype.Split('|');
                    xml.dss["StkOdds10"] = ptype2f[0];
                    xml.dss["StkOdds11"] = ptype2f[0];
                }
                else
                {
                    string ptype = ub.GetSub("StkDS", xmlPath);
                    string[] ptype2f = ptype.Split('|');
                    double oddschushi = Convert.ToDouble(ptype2f[0]);
                    double Odds = Convert.ToDouble(ptype2f[1]);
                    double oddsmax = Convert.ToDouble(ptype2f[2]);
                    double oddsmin = Convert.ToDouble(ptype2f[3]);
                    double oddschushi2 = oddschushi * 2;
                    string Da = string.Empty; string Xiao = string.Empty;
                    if (oddschushi + Odds * (count2 - lk) > oddsmax || oddschushi + Odds * (count2 - lk) < oddsmin)
                    {
                        if (oddschushi + Odds * (count2 - lk) > oddsmax) { Xiao = oddsmax.ToString(); Da = (oddschushi2 - oddsmax).ToString(); }
                        if (oddschushi + Odds * (count2 - lk) < oddsmin) { Xiao = oddsmin.ToString(); Da = (oddschushi2 - oddsmin).ToString(); }
                        xml.dss["StkOdds10"] = Da;
                        xml.dss["StkOdds11"] = Xiao;
                    }
                    else
                    {
                        Xiao = (oddschushi + Odds * (count2 - lk)).ToString();
                        Da = (oddschushi2 - (oddschushi + Odds * (count2 - lk))).ToString();
                        xml.dss["StkOdds10"] = Da;
                        xml.dss["StkOdds11"] = Xiao;
                    }
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #endregion

            #region 合大小、单双
            #region 合大
            if (sum > 4)
            {
                bool s1 = true;
                int count1 = 1;
                while (s1)
                {
                    string Result = ds.Tables[0].Rows[count1]["WinNum"].ToString();
                    int r1 = Convert.ToInt32(Result.Substring(0, 1));//千位
                    int r2 = Convert.ToInt32(Result.Substring(1, 1));//百位
                    int r3 = Convert.ToInt32(Result.Substring(2, 1));//十位
                    int r4 = Convert.ToInt32(Result.Substring(3, 1));//个位

                    string sm1 = (re1 + re2 + re3 + re4).ToString();//和
                    int sm = Convert.ToInt32(sum1.Substring(sum1.Length - 1, 1));

                    if (sm > 4)
                        count1++;
                    else
                        s1 = false;
                }
                if (count1 <= lk)
                {
                    string ptype = ub.GetSub("StkHDX", xmlPath);
                    string[] ptype2f = ptype.Split('|');
                    xml.dss["StkOdds16"] = ptype2f[0];
                    xml.dss["StkOdds17"] = ptype2f[0];
                }
                else
                {
                    string ptype = ub.GetSub("StkHDX", xmlPath);
                    string[] ptype2f = ptype.Split('|');
                    double oddschushi = Convert.ToDouble(ptype2f[0]);
                    double Odds = Convert.ToDouble(ptype2f[1]);
                    double oddsmax = Convert.ToDouble(ptype2f[2]);
                    double oddsmin = Convert.ToDouble(ptype2f[3]);
                    double oddschushi2 = oddschushi * 2;
                    string Da = string.Empty; string Xiao = string.Empty;
                    if (oddschushi + Odds * (count1 - lk) > oddsmax || oddschushi + Odds * (count1 - lk) < oddsmin)
                    {
                        if (oddschushi + Odds * (count1 - lk) > oddsmax) { Da = oddsmax.ToString(); Xiao = (oddschushi2 - oddsmax).ToString(); }
                        if (oddschushi + Odds * (count1 - lk) < oddsmin) { Da = oddsmin.ToString(); Xiao = (oddschushi2 - oddsmin).ToString(); }
                        xml.dss["StkOdds16"] = Da;
                        xml.dss["StkOdds17"] = Xiao;
                    }
                    else
                    {
                        Da = (oddschushi + Odds * (count1 - lk)).ToString();
                        Xiao = (oddschushi2 - (oddschushi + Odds * (count1 - lk))).ToString();
                        xml.dss["StkOdds16"] = Da;
                        xml.dss["StkOdds17"] = Xiao;
                    }
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #region 合小
            else
            {
                bool s1 = true;
                int count1 = 1;
                while (s1)
                {
                    string Result = ds.Tables[0].Rows[count1]["WinNum"].ToString();
                    int r1 = Convert.ToInt32(Result.Substring(0, 1));//千位
                    int r2 = Convert.ToInt32(Result.Substring(1, 1));//百位
                    int r3 = Convert.ToInt32(Result.Substring(2, 1));//十位
                    int r4 = Convert.ToInt32(Result.Substring(3, 1));//个位

                    string sm1 = (re1 + re2 + re3 + re4).ToString();//和
                    int sm = Convert.ToInt32(sum1.Substring(sum1.Length - 1, 1));

                    if (sm > 4)
                        s1 = false;
                    else
                        count1++;
                }
                if (count1 <= lk)
                {
                    string ptype = ub.GetSub("StkHDX", xmlPath);
                    string[] ptype2f = ptype.Split('|');
                    xml.dss["StkOdds16"] = ptype2f[0];
                    xml.dss["StkOdds17"] = ptype2f[0];
                }
                else
                {
                    string ptype = ub.GetSub("StkHDX", xmlPath);
                    string[] ptype2f = ptype.Split('|');
                    double oddschushi = Convert.ToDouble(ptype2f[0]);
                    double Odds = Convert.ToDouble(ptype2f[1]);
                    double oddsmax = Convert.ToDouble(ptype2f[2]);
                    double oddsmin = Convert.ToDouble(ptype2f[3]);
                    double oddschushi2 = oddschushi * 2;
                    string Da = string.Empty; string Xiao = string.Empty;
                    if (oddschushi + Odds * (count1 - lk) > oddsmax || oddschushi + Odds * (count1 - lk) < oddsmin)
                    {
                        if (oddschushi + Odds * (count1 - lk) > oddsmax) { Xiao = oddsmax.ToString(); Da = (oddschushi2 - oddsmax).ToString(); }
                        if (oddschushi + Odds * (count1 - lk) < oddsmin) { Xiao = oddsmin.ToString(); Da = (oddschushi2 - oddsmin).ToString(); }
                        xml.dss["StkOdds16"] = Da;
                        xml.dss["StkOdds17"] = Xiao;
                    }
                    else
                    {
                        Xiao = (oddschushi + Odds * (count1 - lk)).ToString();
                        Da = (oddschushi2 - (oddschushi + Odds * (count1 - lk))).ToString();
                        xml.dss["StkOdds16"] = Da;
                        xml.dss["StkOdds17"] = Xiao;
                    }
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #region 合单
            if (sum % 2 != 0)
            {
                bool s2 = true;
                int count2 = 1;
                while (s2)
                {
                    string Result = ds.Tables[0].Rows[count2]["WinNum"].ToString();
                    int r1 = Convert.ToInt32(Result.Substring(0, 1));//千位
                    int r2 = Convert.ToInt32(Result.Substring(1, 1));//百位
                    int r3 = Convert.ToInt32(Result.Substring(2, 1));//十位
                    int r4 = Convert.ToInt32(Result.Substring(3, 1));//个位

                    string sm1 = (re1 + re2 + re3 + re4).ToString();//和
                    int sm = Convert.ToInt32(sum1.Substring(sum1.Length - 1, 1));
                    if (sm % 2 != 0)
                        count2++;
                    else
                        s2 = false;
                }
                if (count2 <= lk)
                {
                    string ptype = ub.GetSub("StkHDS", xmlPath);
                    string[] ptype2f = ptype.Split('|');
                    xml.dss["StkOdds14"] = ptype2f[0];
                    xml.dss["StkOdds15"] = ptype2f[0];
                }
                else
                {
                    string ptype = ub.GetSub("StkHDS", xmlPath);
                    string[] ptype2f = ptype.Split('|');
                    double oddschushi = Convert.ToDouble(ptype2f[0]);
                    double Odds = Convert.ToDouble(ptype2f[1]);
                    double oddsmax = Convert.ToDouble(ptype2f[2]);
                    double oddsmin = Convert.ToDouble(ptype2f[3]);
                    double oddschushi2 = oddschushi * 2;
                    string Da = string.Empty; string Xiao = string.Empty;
                    if (oddschushi + Odds * (count2 - lk) > oddsmax || oddschushi + Odds * (count2 - lk) < oddsmin)
                    {
                        if (oddschushi + Odds * (count2 - lk) > oddsmax) { Da = oddsmax.ToString(); Xiao = (oddschushi2 - oddsmax).ToString(); }
                        if (oddschushi + Odds * (count2 - lk) < oddsmin) { Da = oddsmin.ToString(); Xiao = (oddschushi2 - oddsmin).ToString(); }
                        xml.dss["StkOdds14"] = Da;
                        xml.dss["StkOdds15"] = Xiao;
                    }
                    else
                    {
                        Da = (oddschushi + Odds * (count2 - lk)).ToString();
                        Xiao = (oddschushi2 - (oddschushi + Odds * (count2 - lk))).ToString();
                        xml.dss["StkOdds14"] = Da;
                        xml.dss["StkOdds15"] = Xiao;
                    }
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #region 合双
            else
            {
                bool s2 = true;
                int count2 = 1;
                while (s2)
                {
                    string Result = ds.Tables[0].Rows[count2]["WinNum"].ToString();
                    int r1 = Convert.ToInt32(Result.Substring(0, 1));//千位
                    int r2 = Convert.ToInt32(Result.Substring(1, 1));//百位
                    int r3 = Convert.ToInt32(Result.Substring(2, 1));//十位
                    int r4 = Convert.ToInt32(Result.Substring(3, 1));//个位

                    string sm1 = (re1 + re2 + re3 + re4).ToString();//和
                    int sm = Convert.ToInt32(sum1.Substring(sum1.Length - 1, 1));
                    if (sm % 2 != 0)
                        s2 = false;
                    else
                        count2++;
                }
                if (count2 <= lk)
                {
                    string ptype = ub.GetSub("StkHDS", xmlPath);
                    string[] ptype2f = ptype.Split('|');
                    xml.dss["StkOdds14"] = ptype2f[0];
                    xml.dss["StkOdds15"] = ptype2f[0];
                }
                else
                {
                    string ptype = ub.GetSub("StkHDS", xmlPath);
                    string[] ptype2f = ptype.Split('|');
                    double oddschushi = Convert.ToDouble(ptype2f[0]);
                    double Odds = Convert.ToDouble(ptype2f[1]);
                    double oddsmax = Convert.ToDouble(ptype2f[2]);
                    double oddsmin = Convert.ToDouble(ptype2f[3]);
                    double oddschushi2 = oddschushi * 2;
                    string Da = string.Empty; string Xiao = string.Empty;
                    if (oddschushi + Odds * (count2 - lk) > oddsmax || oddschushi + Odds * (count2 - lk) < oddsmin)
                    {
                        if (oddschushi + Odds * (count2 - lk) > oddsmax) { Xiao = oddsmax.ToString(); Da = (oddschushi2 - oddsmax).ToString(); }
                        if (oddschushi + Odds * (count2 - lk) < oddsmin) { Xiao = oddsmin.ToString(); Da = (oddschushi2 - oddsmin).ToString(); }
                        xml.dss["StkOdds14"] = Da;
                        xml.dss["StkOdds15"] = Xiao;
                    }
                    else
                    {
                        Xiao = (oddschushi + Odds * (count2 - lk)).ToString();
                        Da = (oddschushi2 - (oddschushi + Odds * (count2 - lk))).ToString();
                        xml.dss["StkOdds14"] = Da;
                        xml.dss["StkOdds15"] = Xiao;
                    }
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #endregion

        }
        catch
        {

        }
        #endregion
    }

    private bool Isbz()
    {
        if (Utils.GetDomain().Contains("kubao") || Utils.GetDomain().Contains("tuhao") || Utils.GetDomain().Contains("th") || Utils.GetDomain().Contains("kb288"))
            return true;
        else
            return false;
    }

}
