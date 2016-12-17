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
using System.Text.RegularExpressions;
using BCW.Common;
using System.Net;
using System.Text;

/// <summary>
/// 时时彩修改消费记录
/// 
/// 黄国军20160312
/// 
/// 姚志光20160621 活跃抽奖控制入口
/// 
/// 蒙宗将 20161027 新开发时时彩 10.28 快捷下注转换成万
/// 蒙宗将 20161029 优化订单加赔率，处理买空号
/// 蒙宗将 20161101 优化历史投注
/// 蒙宗将 20161103 下注默认为空 动态修复
/// 蒙宗将 20161114 任选345下注修复无胆码不足位号 1115
///       20161116 牛牛算法完善
///       26161121 修复周排行榜 投注浮动 22
///       20161124 动态去掉玩法 期数中奖显示简略 25 增加ID限额 26 优化显示
/// </summary>
public partial class bbs_game_ssc : System.Web.UI.Page
{
    protected string xmlPath = "/Controls/ssc.xml";
    protected string GameName = ub.GetSub("SSCName", "/Controls/ssc.xml");
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        //维护提示
        if (ub.GetSub("SSCStatus", xmlPath) == "1")
        {
            Utils.Safe("此游戏");
        }
        //内测判断 0,内测是否开启1，是否为内测账号
        if (ub.GetSub("SSCStatus", xmlPath) == "2")//内测
        {
            string SSCCeshihao = ub.GetSub("SSCCeshihao", "/Controls/ssc.xml");
            string[] sNum = Regex.Split(SSCCeshihao, "#");
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

        Master.Title = "" + GameName + "";
        string act = Utils.GetRequest("act", "all", 1, "", "");

        #region 页面判定 act
        switch (act)
        {
            case "case":
                CasePage();         //兑奖中心
                break;
            case "caseok":
                CaseOkPage();       //成功兑奖提示
                break;
            case "casepost":
                CasePostPage();     //时时彩兑奖保存页
                break;
            case "info":
                int meid = new BCW.User.Users().GetUsId();
                if (meid == 0)
                    Utils.Login();
                InfoPage(meid);         //下注信息页
                break;
            case "pay":
                PayPage();          //下注页面
                break;
            case "mylist":
                MyListPage();       //我的历史下注
                break;
            case "mylistview":
                MyListViewPage();   //我的历史投注详情
                break;
            case "top":
                TopPage();          //排行榜
                break;
            case "rule":
                RulePage();         //时时彩中奖规则
                break;
            case "list":
                ListPage();         //历史开奖
                break;
            case "listview":
                ListViewPage();     //投注详情
                break;
            case "jiangcx":
                JiangcxPage();//往期分析
                break;
            default:
                ReloadPage();       //时时彩首页
                break;
        }
        #endregion
    }

    #region 时时彩首页 ReloadPage
    /// <summary>
    /// 时时彩首页
    /// </summary>
    private void ReloadPage()
    {
        //读取最后一期没有则初始化
        BCW.ssc.Model.SSClist model = new BCW.ssc.Model.SSClist();
        model = new BCW.ssc.BLL.SSClist().GetSSClistLast();
        if (model.ID == 0)
        {
            Utils.Error("系统尚未初始化...", "");
        }
        int meid = new BCW.User.Users().GetUsId();
        string Logo = ub.GetSub("SSCLogo", xmlPath);
        if (Logo != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<img src=\"" + Logo + "\" alt=\"load\"/>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        string Notes = ub.GetSub("SSCNotes", xmlPath);
        if (Notes != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.SysUBB(Notes) + "");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        try
        {
            SSCLISTPAGE();
        }
        catch { }

        builder.Append(Out.Tab("<div>", ""));
        if (IsOpen() == true)
        {
            int Sec = Utils.ParseInt(ub.GetSub("SSCSec", xmlPath));
            if (model.EndTime < DateTime.Now.AddSeconds(Sec))
            {

                if (DateTime.Now < model.EndTime)
                    builder.Append("第" + model.SSCId + "期投注截止<br />等待下一期开售还有:" + new BCW.JS.somejs().newDaojishi("aaa", model.EndTime) + "");
                else
                {
                    if (Utils.Right(model.SSCId.ToString(), 3) == "120")
                    {
                        builder.Append("每天120期,今天已开120期");
                    }
                    else
                    {
                        builder.Append("正在获取下一期信息...");

                    }
                }
            }
            else
            {
                string SSC = new BCW.JS.somejs().newDaojishi("a", model.EndTime.AddSeconds(-Sec));
                builder.Append("第" + model.SSCId + "期投注进行中...<br />距离截止时间还有" + SSC + "");

            }
            builder.Append("|<a href=\"" + Utils.getUrl("ssc.aspx") + "\">刷新</a><br />");


            BCW.ssc.Model.SSClist m = new BCW.ssc.BLL.SSClist().GetSSClistLast2();
            if (m != null)
            {
                builder.Append("第" + m.SSCId + "期开奖:<a href=\"" + Utils.getUrl("ssc.aspx?act=listview&amp;id=" + m.ID + "") + "\">" + m.Result + "</a><br />");
            }
        }
        else
        {
            builder.Append("游戏开放时间:" + ub.GetSub("SSCOnTime", xmlPath) + "<br />");
        }
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=case") + "\">兑奖</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=mylist&amp;ptype=1") + "\">未开</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=mylist&amp;ptype=2") + "\">历史</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=rule") + "\">规则</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=top") + "\">排行</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=jiangcx") + "\">分析</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("你目前自带" + Utils.ConvertGold(new BCW.BLL.User().GetGold(meid)) + "" + ub.Get("SiteBz") + "<br />");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("万位 <a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=1") + "\">直选</a> | <a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=2") + "\">大小</a> | <a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=3") + "\">单双</a><br />");
        builder.Append("千位 <a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=4") + "\">直选</a> | <a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=5") + "\">大小</a> | <a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=6") + "\">单双</a><br />");
        builder.Append("百位 <a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=7") + "\">直选</a> | <a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=8") + "\">大小</a> | <a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=9") + "\">单双</a><br />");
        builder.Append("十位 <a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=10") + "\">直选</a> | <a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=11") + "\">大小</a> | <a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=12") + "\">单双</a><br />");
        builder.Append("个位 <a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=13") + "\">直选</a> | <a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=14") + "\">大小</a> | <a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=15") + "\">单双</a><br />");
        builder.Append("任选 <a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=16") + "\">号码</a> | <a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=17") + "\">龙 虎 和 值</a><br />");
        //  builder.Append(Out.Tab("------<br />", "------<br />"));

        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=18") + "\">任一</a> | <a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=19") + "\">任二</a> | <a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=20") + "\">任三</a> | <a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=21") + "\">任四</a> | <a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=22") + "\">任五</a><br />");
        // builder.Append(Out.Tab("------<br />", "------<br />"));

        builder.Append("牛牛玩法 <a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=23") + "\">有无牛牛</a> | <a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=24") + "\">特定牛牛</a><br />");

        builder.Append("总和 <a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=25") + "\">大小</a> | <a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=26") + "\">单双</a> | <a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=27") + "\">五门</a> | <a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=28") + "\">大小单双</a><br />");
        //   builder.Append(Out.Tab("------<br />", "------<br />"));

        builder.Append("梭哈玩法<br />&nbsp;∟<a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=29") + "\">炸弹</a> | <a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=30") + "\">葫芦</a> | ");
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=31") + "\">顺子</a> | <a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=32") + "\">三条</a> | <a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=33") + "\">两对</a> | <a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=34") + "\">单对</a> | <a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=35") + "\">散牌</a><br />");

        builder.Append("前三特殊玩法<br />");
        builder.Append("&nbsp;∟<a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=36") + "\">大小</a> | <a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=37") + "\">单双</a> | <a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=38") + "\">豹子</a> | <a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=39") + "\">顺子</a> | <a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=40") + "\">对子</a> | <a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=41") + "\">半顺</a> | <a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=42") + "\">杂六</a><br />");
        builder.Append("中三特殊玩法<br />");
        builder.Append("&nbsp;∟<a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=43") + "\">大小</a> | <a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=44") + "\">单双</a> | <a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=45") + "\">豹子</a> | <a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=46") + "\">顺子</a> | <a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=47") + "\">对子</a> | <a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=48") + "\">半顺</a> | <a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=49") + "\">杂六</a><br />");
        builder.Append("后三特殊玩法<br />");
        builder.Append("&nbsp;∟<a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=50") + "\">大小</a> | <a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=51") + "\">单双</a> | <a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=52") + "\">豹子</a> | <a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=53") + "\">顺子</a> | <a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=54") + "\">对子</a> | <a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=55") + "\">半顺</a> | <a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=56") + "\">杂六</a><br />");

        builder.Append(Out.Tab("</div>", ""));


        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        builder.Append("〓历史开奖〓");
        builder.Append(Out.Tab("</div>", ""));
        // 开始读取历史开奖
        int SizeNum = 3;
        string strWhere = "";
        strWhere = "State=1";
        IList<BCW.ssc.Model.SSClist> listSSClist = new BCW.ssc.BLL.SSClist().GetSSClists(SizeNum, strWhere);
        if (listSSClist.Count > 0)
        {
            int k = 1;
            foreach (BCW.ssc.Model.SSClist n in listSSClist)
            {
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.AppendFormat("<a href=\"" + Utils.getUrl("ssc.aspx?act=listview&amp;id=" + n.ID + "") + "\">{0}期:{1} ({2})</a>", n.SSCId, n.Result, Message(n.Result));
                builder.Append(Out.Tab("</div>", ""));
                k++;
            }
            if (k > SizeNum)
            {
                builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=list") + "\">更多开奖记录&gt;&gt;</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
        }

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("〓玩家动态〓");
        builder.Append(Out.Tab("</div>", ""));
        // 开始读取动态列表
        int SizeNum_Action = 3;
        string strWhere_Action = "Types=1019";
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
                builder.Append("<a href=\"" + Utils.getUrl("/bbs/action.aspx?ptype=1019") + "\">更多动态&gt;&gt;</a>");
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
        builder.Append(Out.SysUBB(BCW.User.Users.ShowSpeak(22, "ssc.aspx", 5, 0)));

        //游戏底部Ubb
        string Foot = ub.GetSub("SSCFoot", xmlPath);
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
    #endregion

    #region 历史开奖 ListPage
    /// <summary>
    /// 历史开奖
    /// </summary>
    private void ListPage()
    {
        Master.Title = "历史开奖";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx") + "\">" + GameName + "</a>&gt;历史开奖");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("〓历史开奖〓");
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
        IList<BCW.ssc.Model.SSClist> listSSClist = new BCW.ssc.BLL.SSClist().GetSSClists(pageIndex, pageSize, strWhere, out recordCount);
        if (listSSClist.Count > 0)
        {
            int k = 1;
            foreach (BCW.ssc.Model.SSClist n in listSSClist)
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

                builder.AppendFormat("<a href=\"" + Utils.getUrl("ssc.aspx?act=listview&amp;id=" + n.ID + "") + "\">{0}期:{1} ({2})</a>", n.SSCId, n.Result, Message(n.Result));


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
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 投注详情 ListViewPage
    /// <summary>
    /// 投注详情
    /// </summary>
    private void ListViewPage()
    {
        int id = Utils.ParseInt(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.ssc.Model.SSClist model = new BCW.ssc.BLL.SSClist().GetSSClist(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "第" + model.SSCId + "期投注";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx") + "\">" + GameName + "</a>&gt;第" + model.SSCId + "期");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        strWhere += "SSCId=" + model.SSCId + " and winCent>0";

        string[] pageValUrl = { "act", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        // 开始读取列表
        IList<BCW.ssc.Model.SSCpay> listSSCpay = new BCW.ssc.BLL.SSCpay().GetSSCpays(pageIndex, pageSize, strWhere, out recordCount);
        if (listSSCpay.Count > 0)
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("第" + model.SSCId + "期开出:<b>" + model.Result + " (" + Message(model.Result) + ")</b>");

            builder.Append("<br />共" + recordCount + "注中奖");

            builder.Append(Out.Tab("</div>", "<br />"));

            int k = 1;
            foreach (BCW.ssc.Model.SSCpay n in listSSCpay)
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

                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "</a>");

                //  builder.Append("<b>[" + OutType(n.Types) + "]</b>位号:" + n.Notes + "/每注" + n.Price + "" + ub.Get("SiteBz") + "/共" + n.iCount + "注/赔率" + n.Odds + "[" + DT.FormatDate(n.AddTime, 1) + "]");
                if (n.WinCent > 0)
                {
                    builder.Append(" 赢" + n.WinCent + "" + ub.Get("SiteBz") + "（中" + GetZj_zs(n.WinNotes) + "注）");
                }
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

        }
        else
        {
            builder.Append(Out.Div("div", "没有返彩或无下注记录.."));
        }
        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=list") + "\">历史开奖</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 我的历史下注 MyListPage
    /// <summary>
    /// 我的历史下注
    /// </summary>
    private void MyListPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[1-2]$", "1"));
        string strTitle = "";
        if (ptype == 1)
            strTitle = "我的未开下注";
        else
            strTitle = "我的历史下注";

        Master.Title = strTitle;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx") + "\">" + GameName + "</a>&gt;" + strTitle.Replace("我的", "") + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append(strTitle);
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
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

        string SSCqi = "";
        // 开始读取列表
        IList<BCW.ssc.Model.SSCpay> listSSCpay = new BCW.ssc.BLL.SSCpay().GetSSCpays(pageIndex, pageSize, strWhere, out recordCount);
        if (listSSCpay.Count > 0)
        {
            int k = 1;
            foreach (BCW.ssc.Model.SSCpay n in listSSCpay)
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
                if (n.SSCId.ToString() != SSCqi)
                {
                    try
                    {
                        builder.Append("=第" + n.SSCId + "期=<f style=\"color:red\">" + n.Result + " (" + Message(n.Result) + ")</f><br />");
                    }
                    catch
                    {
                        builder.Append("=第" + n.SSCId + "期=<br />");
                    }
                }

                builder.Append("<b>" + ((pageIndex - 1) * pageSize + k) + ".</b>");

                builder.Append("<b>[" + OutType(n.Types) + "]</b>位号:" + n.Notes + "/每注" + n.Price + "" + ub.Get("SiteBz") + "/共" + n.iCount + "注/赔率" + n.Odds + "[" + DT.FormatDate(n.AddTime, 1) + "]");

                if (n.WinCent > 0)
                {
                    builder.Append("赢" + n.WinCent + "" + ub.Get("SiteBz") + "（中" + GetZj_zs(n.WinNotes) + "注）");

                }
                builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=mylistview&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">详情&gt;&gt;</a>");

                SSCqi = n.SSCId.ToString();
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
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=mylist&amp;ptype=1") + "\">未开下注</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=mylist&amp;ptype=2") + "\">历史下注</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 我的历史投注详情 MyListViewPage
    /// <summary>
    /// 我的历史投注详情
    /// </summary>
    private void MyListViewPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = Utils.ParseInt(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.ssc.Model.SSCpay n = new BCW.ssc.BLL.SSCpay().GetSSCpay(id);
        if (n == null || n.UsID != meid)
        {
            Utils.Error("不存在的记录", "");
        }

        Master.Title = "第" + n.SSCId + "期";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx") + "\">" + GameName + "</a>&gt;第" + n.SSCId + "期");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("第" + n.SSCId + "期");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (n.Result == "")
            builder.Append("开奖号码:未开奖");
        else
            builder.Append("开奖号码:" + n.Result + "");

        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("类型:<b>" + OutType(n.Types) + "</b><br />位号:" + n.Notes + "<br />每注:" + n.Price + "" + ub.Get("SiteBz") + "<br />注数:" + n.iCount + "注<br />花费:" + n.Prices + "" + ub.Get("SiteBz") + "");
        builder.Append("<br />下注:" + DT.FormatDate(n.AddTime, 0) + "<br />赔率:" + n.Odds + "");
        if (n.WinCent > 0)
        {
            builder.Append("<br />结果:赢:" + n.WinCent + "" + ub.Get("SiteBz") + "（中" + GetZj_zs(n.WinNotes) + "注）");
        }
        else
        {
            if (n.State == 0)
                builder.Append("<br />结果:未开奖");
            else
                builder.Append("<br />结果:未中奖");

        }
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("ssc.aspx?act=mylist&amp;ptype=2") + "\">返回上一级</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx") + "\">返回" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));

    }
    #endregion

    #region 排行榜 TopPage
    /// <summary>
    /// 排行榜
    /// </summary>
    private void TopPage()
    {
        Master.Title = "" + GameName + "排行榜";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx") + "\">" + GameName + "</a>&gt;排行榜");
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
            strWhere = "Prices>0 and State>0 ";
            builder.Append("总榜 | <a href=\"" + Utils.getUrl("ssc.aspx?act=top&amp;ptype=1") + "\">周榜</a> | <a href=\"" + Utils.getUrl("ssc.aspx?act=top&amp;ptype=2") + "\">日榜</a>");
        }
        else if (ptype == 1)
        {
            strWhere = " datediff(WEEK,AddTime,getdate())=0 and Prices>0 and State>0 ";
            builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=top&amp;ptype=0") + "\">总榜</a> | 周榜 | <a href=\"" + Utils.getUrl("ssc.aspx?act=top&amp;ptype=2") + "\">日榜</a>");
        }
        else
        {
            strWhere = " datediff(DAY,AddTime,getdate())=0 and Prices>0 and State>0 ";
            builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=top&amp;ptype=0") + "\">总榜</a> | <a href=\"" + Utils.getUrl("ssc.aspx?act=top&amp;ptype=1") + "\">周榜</a> | 日榜");
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        if (pageIndex >= 10)
            pageIndex = 10;
        // 开始读取列表
        IList<BCW.ssc.Model.SSCpay> listSSCpay = new BCW.ssc.BLL.SSCpay().GetSSCpaysTop(pageIndex, pageSize, strWhere, out recordCount);
        if (listSSCpay.Count > 0)
        {
            int k = 1;
            foreach (BCW.ssc.Model.SSCpay n in listSSCpay)
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
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 往期分析
    //往期分析
    private void JiangcxPage()
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]\d*$", "1"));

        Master.Title = "" + GameName + "往期分析";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx") + "\">" + GameName + "</a>&gt;往期分析");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("〓往期分析〓");
        builder.Append(Out.Tab("</div>", ""));

        #region 表格的提交和确认
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        string searchDate = DateTime.Now.ToString("yyyyMMdd");

        if (Utils.ToSChinese(ac) == "确定修改")
        {
            searchDate = Utils.GetRequest("date", "all", 2, @"^(\d\d\d\d\d\d\d\d)$", "请输入正确的时间格式");
        }

        //输入框
        string strText = "查询日期(格式20160808):/,";
        string strName = "date,backurl";
        string strType = "text,hidden";
        string strValu = "" + searchDate + "'" + Utils.getPage(1) + "";
        string strEmpt = "false,false";
        string strIdea = "/";
        string strOthe = "确定修改,ssc.aspx?act=jiangcx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        #endregion

        #region 从数据库中调出符合条件的数据并统计
        string strWhere = string.Empty;
        strWhere += "SSCId like \'" + searchDate.Remove(0, 2) + "%\' and State <> 0";
        DataSet ds = new BCW.ssc.BLL.SSClist().GetList("Result", strWhere);
        int[] num = new int[93];

        //数据总结
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            string result = ds.Tables[0].Rows[i]["Result"].ToString();
            string[] Result = ds.Tables[0].Rows[i]["Result"].ToString().Split(' ');
            int Re1 = Convert.ToInt32(Result[0]);//万位
            int Re2 = Convert.ToInt32(Result[1]);
            int Re3 = Convert.ToInt32(Result[2]);
            int Re4 = Convert.ToInt32(Result[3]);
            int Re5 = Convert.ToInt32(Result[4]);
            int temp = 0;
            int sum = 0;
            string temps = string.Empty;
            for (int j = 0; j < Result.Length; j++)
            {
                temp = Convert.ToInt32(Result[j]);
                num[temp]++;
                sum += temp;
            }
            temps = Convert.ToString(sum);
            temps = temps.Substring(temps.Length - 1, 1);
            temp = Convert.ToInt32(temps);

            #region 大小单双 10-50
            if (Re1 > 4)
                num[11]++;//万位大
            else
                num[12]++;//万位小
            if (Re1 % 2 != 0)
                num[13]++;//万位单
            else
                num[14]++;//万位双

            if (Re2 > 4)
                num[15]++;//千位大
            else
                num[16]++;//位小
            if (Re2 % 2 != 0)
                num[17]++;//位单
            else
                num[18]++;//位双

            if (Re3 > 4)
                num[19]++;//百位大
            else
                num[20]++;//位小
            if (Re3 % 2 != 0)
                num[21]++;//位单
            else
                num[22]++;//位双

            if (Re4 > 4)
                num[23]++;//十位大
            else
                num[24]++;//位小
            if (Re4 % 2 != 0)
                num[25]++;//位单
            else
                num[26]++;//位双

            if (Re5 > 4)
                num[27]++;//个位大
            else
                num[28]++;//位小
            if (Re5 % 2 != 0)
                num[29]++;//位单
            else
                num[30]++;//位双

            int sum5 = 0;
            sum5 = Re1 + Re2 + Re3 + Re4 + Re5;
            if (sum5 > 22)
            {
                num[31]++;//总和位大
                if (sum5 % 2 != 0)//大单
                {
                    num[47]++;
                }
                else//大双
                {
                    num[48]++;
                }
            }
            else
            {
                num[32]++;//位小
                if (sum5 % 2 != 0)//小单
                {
                    num[49]++;
                }
                else//小双
                {
                    num[50]++;
                }
            }
            if (sum5 % 2 != 0)
            {
                num[33]++;//位单
            }
            else
            {
                num[34]++;//位双
            }

            int sumq3 = 0;
            sumq3 = Re1 + Re2 + Re3;
            if (sumq3 > 13)
                num[35]++;//前三位大
            else
                num[36]++;//位小
            if (sumq3 % 2 != 0)
                num[37]++;//位单
            else
                num[38]++;//位双

            int sumz3 = 0;
            sumz3 = Re4 + Re2 + Re3;
            if (sumz3 > 13)
                num[39]++;//中三位大
            else
                num[40]++;//位小
            if (sumz3 % 2 != 0)
                num[41]++;//位单
            else
                num[42]++;//位双

            int sumh3 = 0;
            sumh3 = Re4 + Re5 + Re3;
            if (sumh3 > 13)
                num[43]++;//后三位大
            else
                num[44]++;//位小
            if (sumh3 % 2 != 0)
                num[45]++;//位单
            else
                num[46]++;//位双
            #endregion

            #region 龙虎和
            if (Re1 > Re5)//龙
                num[51]++;
            if (Re1 < Re5)//虎
                num[52]++;
            if (Re1 == Re5)//和
                num[53]++;
            #endregion

            #region 牛牛，总和五门
            if (Niu(result) != "")
                num[54]++;//有牛
            else
                num[55]++;//无牛
            string a = Niu(result);
            int c = 0;
            if (a != "")
            {
                c = Convert.ToInt32(a.Substring(a.Length - 1, 1));
            }
            if (c == 0)
                num[56]++;//牛0
            if (c == 1)
                num[57]++;
            if (c == 2)
                num[58]++;
            if (c == 3)
                num[59]++;
            if (c == 4)
                num[60]++;
            if (c == 5)
                num[61]++;
            if (c == 6)
                num[62]++;
            if (c == 7)
                num[63]++;
            if (c == 8)
                num[64]++;
            if (c == 9)
                num[65]++;//牛9

            if (sum5 < 10)
                num[66]++;//一门
            if (sum5 > 9 && sum5 < 20)
                num[67]++;
            if (sum5 > 19 && sum5 < 30)
                num[68]++;
            if (sum5 > 29 && sum5 < 40)
                num[69]++;
            if (sum5 > 39 && sum5 < 46)
                num[70]++;
            #endregion

            #region 前三
            if (Qiansan(3, result) == 1)//豹子
                num[71]++;
            if (Qiansan(4, result) == 1)//顺子
                num[72]++;
            if (Qiansan(5, result) == 1)//对子
                num[73]++;
            if (Qiansan(6, result) == 1)//半顺
                num[74]++;
            if (Qiansan(7, result) != 1)//杂六
                num[75]++;
            #endregion

            #region 中三
            if (Zhongsan(3, result) == 1)//豹子
                num[76]++;
            if (Zhongsan(4, result) == 1)//顺子
                num[77]++;
            if (Zhongsan(5, result) == 1)//对子
                num[78]++;
            if (Zhongsan(6, result) == 1)//半顺
                num[79]++;
            if (Zhongsan(7, result) != 1)//杂六
                num[80]++;
            #endregion

            #region 后三
            if (Housan(3, result) == 1)//豹子
                num[81]++;
            if (Housan(4, result) == 1)//顺子
                num[82]++;
            if (Housan(5, result) == 1)//对子
                num[83]++;
            if (Housan(6, result) == 1)//半顺
                num[84]++;
            if (Housan(7, result) != 1)//杂六
                num[85]++;
            #endregion

            #region 梭哈
            if (Zhadan(result) >= 4)
                num[86]++;
            if (HuLu(result) == 1)
                num[87]++;
            if (SHShunzi(result) == 1)
                num[88]++;
            if (SHSantiao(result) == 1)
                num[89]++;
            if (SHLiangdui(result) == 1)
                num[90]++;
            if (SHDandui(result) == 1)
                num[91]++;
            if (SHSanpai(result) == 1)
                num[92]++;
            #endregion

        }
        #endregion

        #region 显示统计结果
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<h class=\"text\" style=\"color:#FF7F00\">冷热奖号统计</h>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", Out.Hr()));
        for (int i = 0; i < 10; i++)
        {
            if ((i + 1) % 5 == 0)
                builder.Append("<b>" + i + "：<b style=\"color:red\">" + num[i] + "</b>次</b><br />");
            else
            {
                builder.Append("<b>" + i + "：<b style=\"color:red\">&nbsp;&nbsp;" + num[i] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
            }
        }
        builder.Append(Out.Tab("</div>", Out.RHr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<h class=\"text\" style=\"color:#FF7F00\">大小单双统计</h>");
        builder.Append(Out.Tab("</div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<h class=\"text\" style=\"color:#FF7F00\">万位 </h>");
        builder.Append("<b>大：<b style=\"color:red\">" + num[11] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>小：<b style=\"color:red\">" + num[12] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>单：<b style=\"color:red\">" + num[13] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>双：<b style=\"color:red\">" + num[14] + "</b>次</b><br />");

        builder.Append("<h class=\"text\" style=\"color:#FF7F00\">千位 </h>");
        builder.Append("<b>大：<b style=\"color:red\">" + num[15] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>小：<b style=\"color:red\">" + num[16] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>单：<b style=\"color:red\">" + num[17] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>双：<b style=\"color:red\">" + num[18] + "</b>次</b><br />");

        builder.Append("<h class=\"text\" style=\"color:#FF7F00\">百位 </h>");
        builder.Append("<b>大：<b style=\"color:red\">" + num[19] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>小：<b style=\"color:red\">" + num[20] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>单：<b style=\"color:red\">" + num[21] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>双：<b style=\"color:red\">" + num[22] + "</b>次</b><br />");

        builder.Append("<h class=\"text\" style=\"color:#FF7F00\">个位 </h>");
        builder.Append("<b>大：<b style=\"color:red\">" + num[23] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>小：<b style=\"color:red\">" + num[24] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>单：<b style=\"color:red\">" + num[25] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>双：<b style=\"color:red\">" + num[26] + "</b>次</b><br />");

        builder.Append("<h class=\"text\" style=\"color:#FF7F00\">个位 </h>");
        builder.Append("<b>大：<b style=\"color:red\">" + num[27] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>小：<b style=\"color:red\">" + num[28] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>单：<b style=\"color:red\">" + num[29] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>双：<b style=\"color:red\">" + num[30] + "</b>次</b><br />");

        builder.Append("<h class=\"text\" style=\"color:#FF7F00\">总和 </h>");
        builder.Append("<b>大：<b style=\"color:red\">" + num[31] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>小：<b style=\"color:red\">" + num[32] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>单：<b style=\"color:red\">" + num[33] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>双：<b style=\"color:red\">" + num[34] + "</b>次</b><br />");

        builder.Append("<h class=\"text\" style=\"color:#FF7F00\">前三 </h>");
        builder.Append("<b>大：<b style=\"color:red\">" + num[35] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>小：<b style=\"color:red\">" + num[36] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>单：<b style=\"color:red\">" + num[37] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>双：<b style=\"color:red\">" + num[38] + "</b>次</b><br />");

        builder.Append("<h class=\"text\" style=\"color:#FF7F00\">中三 </h>");
        builder.Append("<b>大：<b style=\"color:red\">" + num[39] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>小：<b style=\"color:red\">" + num[40] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>单：<b style=\"color:red\">" + num[41] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>双：<b style=\"color:red\">" + num[42] + "</b>次</b><br />");

        builder.Append("<h class=\"text\" style=\"color:#FF7F00\">后三 </h>");
        builder.Append("<b>大：<b style=\"color:red\">" + num[43] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>小：<b style=\"color:red\">" + num[44] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>单：<b style=\"color:red\">" + num[45] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>双：<b style=\"color:red\">" + num[46] + "</b>次</b><br />");

        builder.Append("<h class=\"text\" style=\"color:#FF7F00\">总和 </h>");
        builder.Append("<b>大单：<b style=\"color:red\">" + num[48] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>大双：<b style=\"color:red\">" + num[49] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>小单：<b style=\"color:red\">" + num[49] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>小双：<b style=\"color:red\">" + num[50] + "</b>次</b><br />");
        builder.Append(Out.Tab("</div>", Out.RHr()));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<h class=\"text\" style=\"color:#FF7F00\">龙虎和统计</h>");
        builder.Append(Out.Tab("</div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b>龙：<b style=\"color:red\">" + num[51] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>虎：<b style=\"color:red\">" + num[52] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>和：<b style=\"color:red\">" + num[53] + "</b>次</b><br />");
        builder.Append(Out.Tab("</div>", Out.RHr()));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<h class=\"text\" style=\"color:#FF7F00\">牛牛统计</h>");
        builder.Append(Out.Tab("</div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b>有牛：<b style=\"color:red\">" + num[54] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>无牛：<b style=\"color:red\">" + num[55] + "</b>次</b><br />");
        for (int i = 56; i < 66; i++)
        {
            if (((i - 56) + 1) % 5 == 0)
                builder.Append("<b>牛" + (i - 56) + "：<b style=\"color:red\">" + num[i] + "</b>次</b><br />");
            else
            {
                builder.Append("<b>牛" + (i - 56) + "：<b style=\"color:red\">&nbsp;&nbsp;" + num[i] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
            }
        }
        builder.Append(Out.Tab("</div>", Out.RHr()));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<h class=\"text\" style=\"color:#FF7F00\">总和五门统计</h>");
        builder.Append(Out.Tab("</div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b>一门：<b style=\"color:red\">" + num[66] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>二门：<b style=\"color:red\">" + num[67] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>三门：<b style=\"color:red\">" + num[68] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>四门：<b style=\"color:red\">" + num[69] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>五门：<b style=\"color:red\">" + num[70] + "</b>次</b><br />");
        builder.Append(Out.Tab("</div>", Out.RHr()));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<h class=\"text\" style=\"color:#FF7F00\">取三统计</h>");
        builder.Append(Out.Tab("</div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<h class=\"text\" style=\"color:#FF7F00\">前三 </h>");
        builder.Append("<b>豹子：<b style=\"color:red\">" + num[71] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>顺子：<b style=\"color:red\">" + num[72] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>对子：<b style=\"color:red\">" + num[73] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>半顺：<b style=\"color:red\">" + num[74] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>杂六：<b style=\"color:red\">" + num[75] + "</b>次</b><br />");

        builder.Append("<h class=\"text\" style=\"color:#FF7F00\">中三 </h>");
        builder.Append("<b>豹子：<b style=\"color:red\">" + num[76] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>顺子：<b style=\"color:red\">" + num[77] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>对子：<b style=\"color:red\">" + num[78] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>半顺：<b style=\"color:red\">" + num[79] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>杂六：<b style=\"color:red\">" + num[80] + "</b>次</b><br />");

        builder.Append("<h class=\"text\" style=\"color:#FF7F00\">后三 </h>");
        builder.Append("<b>豹子：<b style=\"color:red\">" + num[81] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>顺子：<b style=\"color:red\">" + num[82] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>对子：<b style=\"color:red\">" + num[83] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>半顺：<b style=\"color:red\">" + num[84] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>杂六：<b style=\"color:red\">" + num[85] + "</b>次</b><br />");
        builder.Append(Out.Tab("</div>", Out.RHr()));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<h class=\"text\" style=\"color:#FF7F00\">梭哈统计</h>");
        builder.Append(Out.Tab("</div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b>炸弹：<b style=\"color:red\">" + num[86] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>葫芦：<b style=\"color:red\">" + num[87] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>顺子：<b style=\"color:red\">" + num[88] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>三条：<b style=\"color:red\">" + num[89] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>两对：<b style=\"color:red\">" + num[90] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>单对：<b style=\"color:red\">" + num[91] + "</b>次</b>&nbsp;&nbsp;&nbsp;");
        builder.Append("<b>散牌：<b style=\"color:red\">" + num[92] + "</b>次</b>");
        builder.Append(Out.Tab("</div>", ""));
        #endregion

        //builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 成功兑奖提示 CaseOkPage
    /// <summary>
    /// 成功兑奖提示
    /// </summary>
    private void CaseOkPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);
        int pid = Utils.ParseInt(Utils.GetRequest("pid", "get", 2, @"^[0-9]\d*$", "选择兑奖无效"));

        if (new BCW.ssc.BLL.SSCpay().ExistsState(pid, meid))
        {
            BCW.User.Users.IsFresh("ssc", 1);//防刷
            new BCW.ssc.BLL.SSCpay().UpdateState(pid, 2);
            //操作币
            long winMoney = Convert.ToInt64(new BCW.ssc.BLL.SSCpay().GetWinCent(pid));
            BCW.ssc.Model.SSCpay model = new BCW.ssc.BLL.SSCpay().GetSSCpay(pid);
            BCW.ssc.Model.SSClist idd = new BCW.ssc.BLL.SSClist().GetSSClistbySSCId(model.SSCId);
            new BCW.BLL.User().UpdateiGold(meid, mename, winMoney, "" + GameName + "兑奖-" + "[url=./game/ssc.aspx?act=view&amp;id=" + idd.ID + "&amp;ptype=2]" + model.SSCId + "[/url]" + "-标识ID" + pid + "");
            if (new BCW.BLL.User().GetIsSpier(meid) != 1)
                new BCW.BLL.User().UpdateiGold(108, new BCW.BLL.User().GetUsName(108), -winMoney, "ID:[url=forumlog.aspx?act=xview&amp;uid=" + meid + "]" + meid + "[/url]" + GameName + "第[url=./game/ssc.aspx?act=view&amp;id=" + idd.ID + "&amp;ptype=2]" + model.SSCId + "[/url]期兑奖" + winMoney + "|（标识ID" + pid + "）");

            if (ub.GetSub("SSCGuestSet", "/Controls/ssc.xml") == "0")
                new BCW.BLL.Guest().Add(1, meid, mename, "您在[url=./game/ssc.aspx]" + GameName + "[/url]第" + model.SSCId + "期的投注：[" + OutType(model.Types) + "]位号:" + model.Notes + "已经兑奖，获得了" + winMoney + ub.Get("SiteBz") + "。");//开奖提示信息,1表示开奖信息

            Utils.Success("兑奖", "恭喜，成功兑奖" + winMoney + "" + ub.Get("SiteBz") + "", Utils.getUrl("ssc.aspx?act=case"), "2");
        }
        else
        {
            Utils.Success("兑奖", "重复兑奖或没有可以兑奖的记录", Utils.getUrl("ssc.aspx?act=case"), "1");
        }
    }
    #endregion

    #region 时时彩兑奖保存页 CasePostPage
    /// <summary>
    /// 时时彩兑奖保存页
    /// </summary>
    private void CasePostPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);
        string arrId = "";
        arrId = Utils.GetRequest("arrId", "post", 1, "", "");
        if (!Utils.IsRegex(arrId.Replace(",", ""), @"^[0-9]\d*$"))
        {
            Utils.Error("选择本页兑奖出错", "");
        }
        BCW.User.Users.IsFresh("ssc", 1);//防刷
        string[] strArrId = arrId.Split(",".ToCharArray());
        long getwinMoney = 0;
        long winMoney = 0;
        for (int i = 0; i < strArrId.Length; i++)
        {
            int pid = Convert.ToInt32(strArrId[i]);
            if (new BCW.ssc.BLL.SSCpay().ExistsState(pid, meid))
            {

                new BCW.ssc.BLL.SSCpay().UpdateState(pid, 2);
                //操作币
                winMoney = Convert.ToInt64(new BCW.ssc.BLL.SSCpay().GetWinCent(pid));

                BCW.ssc.Model.SSCpay model = new BCW.ssc.BLL.SSCpay().GetSSCpay(pid);
                BCW.ssc.Model.SSClist idd = new BCW.ssc.BLL.SSClist().GetSSClistbySSCId(model.SSCId);
                new BCW.BLL.User().UpdateiGold(meid, mename, winMoney, "" + GameName + "兑奖-" + "[url=./game/ssc.aspx?act=view&amp;id=" + idd.ID + "&amp;ptype=2]" + model.SSCId + "[/url]" + "-标识ID" + pid + "");
                if (new BCW.BLL.User().GetIsSpier(meid) != 1)
                    new BCW.BLL.User().UpdateiGold(108, new BCW.BLL.User().GetUsName(108), -winMoney, "ID:[url=forumlog.aspx?act=xview&amp;uid=" + meid + "]" + meid + "[/url]" + GameName + "第[url=./game/ssc.aspx?act=view&amp;id=" + idd.ID + "&amp;ptype=2]" + model.SSCId + "[/url]期兑奖" + winMoney + "|（标识ID" + pid + "）");
                if (ub.GetSub("SSCGuestSet", "/Controls/ssc.xml") == "0")
                    new BCW.BLL.Guest().Add(1, meid, mename, "您在[url=./game/ssc.aspx]" + GameName + "[/url]第" + model.SSCId + "期的投注：[" + OutType(model.Types) + "]位号:" + model.Notes + "已经兑奖，获得了" + winMoney + ub.Get("SiteBz") + "。");//开奖提示信息,1表示开奖信息

            }
            getwinMoney = getwinMoney + winMoney;
        }
        Utils.Success("兑奖", "恭喜，成功兑奖" + getwinMoney + "" + ub.Get("SiteBz") + "", Utils.getUrl("ssc.aspx?act=case"), "1");
    }
    #endregion

    #region 兑奖中心 CasePage
    /// <summary>
    /// 兑奖中心
    /// </summary>
    private void CasePage()
    {
        Master.Title = "兑奖中心";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx") + "\">" + GameName + "</a>&gt;兑奖中心");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("您现在有" + new BCW.BLL.User().GetGold(meid) + "" + ub.Get("SiteBz") + "");
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
        string SSCqi = "";
        // 开始读取列表
        IList<BCW.ssc.Model.SSCpay> listSSCpay = new BCW.ssc.BLL.SSCpay().GetSSCpays(pageIndex, pageSize, strWhere, out recordCount);
        if (listSSCpay.Count > 0)
        {
            int k = 1;
            foreach (BCW.ssc.Model.SSCpay n in listSSCpay)
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
                if (n.SSCId.ToString() != SSCqi)
                {
                    if (n.Result == "")
                    {
                        builder.Append("=第" + n.SSCId + "期=<br />");
                    }
                    else
                    {
                        builder.Append("=第" + n.SSCId + "期=开出: <f style=\"color:red\">" + n.Result + "</f><br />");
                    }
                }
                builder.Append("<b>" + ((pageIndex - 1) * pageSize + k) + ".</b>");

                builder.Append("<b>[" + OutType(n.Types) + "]</b>位号:" + n.Notes + "/每注" + n.Price + "" + ub.Get("SiteBz") + "/共" + n.iCount + "注/赔率" + n.Odds + "/标识ID:" + n.ID + "[" + DT.FormatDate(n.AddTime, 1) + "]");

                builder.Append("赢" + n.WinCent + "" + ub.Get("SiteBz") + "（中" + GetZj_zs(n.WinNotes) + "注）");

                builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=caseok&amp;pid=" + n.ID + "") + "\">兑奖</a>");

                SSCqi = n.SSCId.ToString();
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
            string strOthe = "本页兑奖,ssc.aspx,post,0,red";
            builder.Append(Out.wapform(strName, strValu, strOthe));
        }
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=mylist&amp;ptype=1") + "\">未开投注</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=mylist&amp;ptype=2") + "\">历史投注</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 时时彩中奖规则 RulePage
    /// <summary>
    /// 时时彩中奖规则
    /// </summary>
    private void RulePage()
    {
        Master.Title = "" + GameName + "中奖规则";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href =\"" + Utils.getUrl("ssc.aspx") + "\">" + GameName + "</a>&gt;规则");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("=" + GameName + "中奖规则=");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));

        //builder.Append("<b>本" + GameName + "根据福利彩票(重庆" + GameName + ")转换成虚拟游戏,开奖时间、结果均与" + GameName + "相同.</b><br />" + GameName + "投注区分为万位、千位、百位、十位和个位，各位号码范围为0-9。<br />每期从各位上开出1个号码作为中奖号码，即开奖号码为5位数。<br />" + GameName + "玩法是竞猜5位开奖号码的全部号码、部分号码。<br />" + GameName + "全天120期，10:00 - 22:00每10分钟一期、22:00 - 01:55每5分钟一期，游戏开奖时间：10:00～01:55。<br />");

        //builder.Append("<b>1." + OutRule(1).Split("===".ToCharArray())[0]).Replace("提示：", "</b>");
        //builder.Append("<b>2." + OutRule(2).Split("===".ToCharArray())[0]).Replace("提示：", "</b>");
        //builder.Append("<b>3." + OutRule(3).Split("===".ToCharArray())[0]).Replace("提示：", "</b>");
        //builder.Append("<b>4." + OutRule(4).Split("===".ToCharArray())[0]).Replace("提示：", "</b>");
        //builder.Append("<b>5." + OutRule(5).Split("===".ToCharArray())[0]).Replace("提示：", "</b>");
        //builder.Append("<b>6." + OutRule(6).Split("===".ToCharArray())[0]).Replace("提示：", "</b>");
        //builder.Append("<b>7." + OutRule(7).Split("===".ToCharArray())[0]).Replace("提示：", "</b>");
        //builder.Append("<b>8." + OutRule(8).Split("===".ToCharArray())[0]).Replace("提示：", "</b>");
        //builder.Append("<b>9." + OutRule(9).Split("===".ToCharArray())[0]).Replace("提示：", "</b>");
        //builder.Append("<b>10." + OutRule(10).Split("===".ToCharArray())[0]).Replace("提示：", "</b>");
        //builder.Append("<b>11." + OutRule(11).Split("===".ToCharArray())[0]).Replace("提示：", "</b>");
        //builder.Append("<b>12." + OutRule(12).Split("===".ToCharArray())[0]).Replace("提示：", "</b>");

        builder.Append(Out.SysUBB(ub.GetSub("SSCRule", xmlPath)));

        builder.Append(Out.Tab("</div>", "<br />"));


        builder.Append(Out.Tab("<div class=\"title\">", Out.RHr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 更新期数 SSCLISTPAGE
    /// <summary>
    /// 更新期数 20160928
    /// </summary>
    public string SSCLISTPAGE()
    {
        string tmpid = "";
        DateTime EndTime = DateTime.Now;
        try
        {
            string tmpStartTime = string.Empty;
            if (DateTime.Now > Convert.ToDateTime("10:00:00") && DateTime.Now < Convert.ToDateTime("22:00:00"))//10：00:00到22:00:00 025--096期 10分钟每期
            {
                tmpStartTime = (EndTime.Year).ToString() + "-" + EndTime.Month.ToString("00") + "-" + EndTime.Day.ToString("00") + " 10:00:00";
            }
            if (DateTime.Now > Convert.ToDateTime("00:00:00") && DateTime.Now < Convert.ToDateTime("01:55:00"))// 00:00:00-01:55:00 001-023 期  5分钟每期
            {
                tmpStartTime = (EndTime.Year).ToString() + "-" + EndTime.Month.ToString("00") + "-" + EndTime.Day.ToString("00") + " 00:00:00";
            }
            if (DateTime.Now > Convert.ToDateTime("22:00:00") && DateTime.Now <= Convert.ToDateTime("23:59:59"))// 22:00:00-00:00:00 097-120 5分钟每期
            {
                tmpStartTime = (EndTime.Year).ToString() + "-" + EndTime.Month.ToString("00") + "-" + EndTime.Day.ToString("00") + " 22:00:00";
            }
            if (DateTime.Now > Convert.ToDateTime("01:55:00") && DateTime.Now <= Convert.ToDateTime("10:00:00"))// 22:00:00-00:00:00 097-120 5分钟每期
            {
                tmpStartTime = (EndTime.Year).ToString() + "-" + EndTime.Month.ToString("00") + "-" + EndTime.Day.ToString("00") + " 10:00:00";
            }
            DateTime StartTime = Convert.ToDateTime(tmpStartTime);
            int d = 1;
            if (DateTime.Now > Convert.ToDateTime("10:00:00") && DateTime.Now < Convert.ToDateTime("22:00:00"))//10：00:00到22:00:00 025--096期 10分钟每期
            {
                if (DateTime.Compare(EndTime, StartTime) > 0)
                {
                    DateTime dt1 = Convert.ToDateTime("10:00:00");
                    string dt3 = DateTime.Now.AddMinutes(5).Subtract(dt1).Duration().TotalMinutes.ToString();
                    decimal dt4 = Convert.ToDecimal(dt3);
                    d = Convert.ToInt32(dt4 / 10);
                }
            }
            if (DateTime.Now > Convert.ToDateTime("00:00:00") && DateTime.Now < Convert.ToDateTime("01:55:00"))// 00:00:00-01:55:00 001-023 期  5分钟每期
            {
                if (DateTime.Compare(EndTime, StartTime) > 0)
                {
                    DateTime dt1 = Convert.ToDateTime("00:00:00");
                    string dt3 = DateTime.Now.AddMinutes(2.5).Subtract(dt1).Duration().TotalMinutes.ToString();
                    decimal dt4 = Convert.ToDecimal(dt3);
                    d = Convert.ToInt32(dt4 / 5);
                }
            }
            if (DateTime.Now > Convert.ToDateTime("22:00:00") && DateTime.Now <= Convert.ToDateTime("23:59:59"))// 22:00:00-00:00:00 097-120 5分钟每期
            {
                if (DateTime.Compare(EndTime, StartTime) > 0)
                {
                    DateTime dt1 = Convert.ToDateTime("22:00:00");
                    string dt3 = DateTime.Now.AddMinutes(2.5).Subtract(dt1).Duration().TotalMinutes.ToString();
                    decimal dt4 = Convert.ToDecimal(dt3);
                    d = Convert.ToInt32(dt4 / 5);
                }
            }
            if (DateTime.Now > Convert.ToDateTime("01:55:00") && DateTime.Now <= Convert.ToDateTime("10:00:00"))// 22:00:00-00:00:00 097-120 5分钟每期
            {
            }


            if (DateTime.Now > Convert.ToDateTime("10:00:00") && DateTime.Now < Convert.ToDateTime("22:00:00"))//10：00:00到22:00:00 025--096期 10分钟每期
            {
                tmpid = (EndTime.Year % 100).ToString() + EndTime.Month.ToString("00") + EndTime.Day.ToString("00") + "0" + (d + 24);
                EndTime = Convert.ToDateTime(tmpStartTime).AddMinutes(d * 10);
            }
            if (DateTime.Now > Convert.ToDateTime("01:55:00") && DateTime.Now < Convert.ToDateTime("10:00:00"))//1:55:00-10:00:00 为 024 期
            {
                tmpid = (EndTime.Year % 100).ToString() + EndTime.Month.ToString("00") + EndTime.Day.ToString("00") + "0" + 24;
                EndTime = Convert.ToDateTime(tmpStartTime);
            }
            if (DateTime.Now > Convert.ToDateTime("22:00:00") && DateTime.Now <= Convert.ToDateTime("23:59:59"))// 22:00:00-00:00:00 097-120 5分钟每期
            {
                if (d < 4)
                {
                    tmpid = (EndTime.Year % 100).ToString() + EndTime.Month.ToString("00") + EndTime.Day.ToString("00") + "0" + (d + 96);
                }
                else
                {
                    tmpid = (EndTime.Year % 100).ToString() + EndTime.Month.ToString("00") + EndTime.Day.ToString("00") + (d + 96);
                }
                EndTime = Convert.ToDateTime(tmpStartTime).AddMinutes(d * 5);
            }
            if (DateTime.Now > Convert.ToDateTime("00:00:00") && DateTime.Now < Convert.ToDateTime("01:55:00"))// 00:00:00-01:55:00 001-023 期  5分钟每期
            {
                if (d < 10)
                {
                    tmpid = (EndTime.Year % 100).ToString() + EndTime.Month.ToString("00") + EndTime.Day.ToString("00") + "00" + d;
                }
                else
                {
                    tmpid = (EndTime.Year % 100).ToString() + EndTime.Month.ToString("00") + EndTime.Day.ToString("00") + "0" + d;
                }
                EndTime = Convert.ToDateTime(tmpStartTime).AddMinutes(d * 5);
            }

            if (!new BCW.ssc.BLL.SSClist().ExistsSSCId(int.Parse(tmpid)))
            {
                if (d <= 120)
                {
                    BCW.ssc.Model.SSClist model = new BCW.ssc.Model.SSClist();
                    model.SSCId = int.Parse(tmpid);
                    model.Result = "";
                    model.Notes = "";
                    model.EndTime = EndTime;
                    model.State = 0;
                    model.StateTime = " ";
                    new BCW.ssc.BLL.SSClist().Add(model);
                }
            }
        }
        catch
        {
        }
        ////   builder.Append("更新最新期数" + tmpid + "--截止时间" + EndTime);
        return "更新最新期数" + tmpid;
    }
    #endregion

    #region 下注信息页 InfoPage
    /// <summary>
    /// 下注信息页
    /// </summary>
    private void InfoPage(int uid)
    {

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        BCW.ssc.Model.SSClist model = new BCW.ssc.BLL.SSClist().GetSSClistLast();
        if (model.ID == 0)
        {
            Utils.Error("正在处理开奖，请稍后...", Utils.getUrl("ssc.aspx"));
        }
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 2, @"^5[0-6]$|^[1-4]\d$|^[1-9]$", "类型选择错误"));
        string Num5 = Utils.GetRequest("Num5", "post", 1, @"^[\d((,)\d)?]+$", "");
        string Num4 = Utils.GetRequest("Num4", "post", 1, @"^[\d((,)\d)?]+$", "");
        string Num3 = Utils.GetRequest("Num3", "post", 1, @"^[\d((,)\d)?]+$", "");
        string Num2 = Utils.GetRequest("Num2", "post", 1, @"^[\d((,)\d)?]+$", "");
        string Num1 = Utils.GetRequest("Num1", "post", 1, @"^[\d((,)\d)?]+$", "");
        long Pric = Utils.ParseInt64(Utils.GetRequest("Pric", "all", 1, @"^[1-9]\d*$", "0"));

        if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
        {
            //  Utils.Success("温馨提示", "使用彩版，下注更直观，更快捷！正在进入...", "ssc.aspx?act=info&amp;ptype=" + ptype + "&amp;ve=2a&amp;u=" + Utils.getstrU() + "", "1");
        }

        int Sec = Utils.ParseInt(ub.GetSub("SSCSec", xmlPath));
        if (model.EndTime < DateTime.Now.AddSeconds(Sec))
        {
            Utils.Error("第" + model.SSCId + "期已截止下注,等待下期开启...", Utils.getUrl("ssc.aspx"));
        }

        string TypeTitle = OutType(ptype);
        Master.Title = TypeTitle;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx") + "\">" + GameName + "</a>&gt;" + TypeTitle + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("第" + model.SSCId + "期");

        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));

        string SSC = new BCW.JS.somejs().newDaojishi("a", model.EndTime.AddSeconds(-Sec));
        builder.Append("距离截止时间还有" + SSC + "");

        builder.Append("|<a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=" + ptype + "") + "\">刷新</a><br />");
        builder.Append("类型：<b>" + TypeTitle + "</b>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append("<form id=\"form1\"  method=\"post\"  action=\"ssc.aspx\" style=\"line-height:30px;\">");
        if (ptype == 1 || ptype == 4 || ptype == 7 || ptype == 10 || ptype == 13 || ptype == 18)
        {
            string name = string.Empty;
            string Num = string.Empty;
            if (ptype == 1) { name = "万位"; Num = "Num5"; }
            if (ptype == 4) { name = "千位"; Num = "Num4"; }
            if (ptype == 7) { name = "百位"; Num = "Num3"; }
            if (ptype == 10) { name = "十位"; Num = "Num2"; }
            if (ptype == 13) { name = "个位"; Num = "Num1"; }
            if (ptype == 18) { name = "任意号码"; Num = "Num5"; }
            //Function(Num);
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("" + name + "：<b>0</b><input type=\"checkbox\" name=\"" + Num + "\" value=\"0\" /> ");//onclick=\"aaa()\"
            builder.Append("<b>1</b><input type=\"checkbox\" name=\"" + Num + "\" value=\"1\"  /> ");
            builder.Append("<b>2</b><input type=\"checkbox\" name=\"" + Num + "\" value=\"2\"/> ");
            builder.Append("<b>3</b><input type=\"checkbox\" name=\"" + Num + "\" value=\"3\" /> ");
            builder.Append("<b>4</b><input type=\"checkbox\" name=\"" + Num + "\" value=\"4\"/> ");
            builder.Append("<b>5</b><input type=\"checkbox\" name=\"" + Num + "\" value=\"5\"/> ");
            builder.Append("<b>6</b><input type=\"checkbox\" name=\"" + Num + "\" value=\"6\"/> ");
            builder.Append("<b>7</b><input type=\"checkbox\" name=\"" + Num + "\" value=\"7\"/> ");
            builder.Append("<b>8</b><input type=\"checkbox\" name=\"" + Num + "\" value=\"8\"/> ");
            builder.Append("<b>9</b><input type=\"checkbox\" name=\"" + Num + "\" value=\"9\"/>");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("每注投注：<input type=\"text\" name=\"Price\" value=\"\" style =\"width:100px;height:25px;\" /> " + ub.Get("SiteBz") + " ");
            builder.Append(" <input class=\"btn-red\" type=\"submit\" name=\"ac99\"  value=\"确定\" style =\"height:25px;\" /> ");
            try
            {
                builder.Append("<br />∟");
                kuai(meid, 1, ptype);//快捷下注|用户ID|快捷下注游戏编号：时时彩为1|下注类型
            }
            catch { }
            builder.Append(Out.Tab("</div>", ""));
        }
        else if (ptype == 19 || ptype == 20 || ptype == 21 || ptype == 22)
        {
            string name = string.Empty;
            string name1 = string.Empty;
            if (ptype == 19) { name = "（0-1个）"; name1 = "（1-10个，不与胆码重复）"; }
            if (ptype == 20) { name = "（0-2个）"; name1 = "（1-10个，不与胆码重复）"; }
            if (ptype == 21) { name = "（0-3个）"; name1 = "（1-10个，不与胆码重复）"; }
            if (ptype == 22) { name = "（0-4个）"; name1 = "（1-10个，不与胆码重复）"; }
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("选择<f style=\"color:red\">胆码</f>" + name + "<br /><b>0</b><input type=\"checkbox\" name=\"Num2\" value=\"0\" /> ");
            builder.Append("<b>1</b><input type=\"checkbox\" name=\"Num2\" value=\"1\" /> ");
            builder.Append("<b>2</b><input type=\"checkbox\" name=\"Num2\" value=\"2\" /> ");
            builder.Append("<b>3</b><input type=\"checkbox\" name=\"Num2\" value=\"3\" /> ");
            builder.Append("<b>4</b><input type=\"checkbox\" name=\"Num2\" value=\"4\" /> ");
            builder.Append("<b>5</b><input type=\"checkbox\" name=\"Num2\" value=\"5\" /> ");
            builder.Append("<b>6</b><input type=\"checkbox\" name=\"Num2\" value=\"6\" /> ");
            builder.Append("<b>7</b><input type=\"checkbox\" name=\"Num2\" value=\"7\" /> ");
            builder.Append("<b>8</b><input type=\"checkbox\" name=\"Num2\" value=\"8\" /> ");
            builder.Append("<b>9</b><input type=\"checkbox\" name=\"Num2\" value=\"9\" />");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("选择<f style=\"color:red\">拖码</f>" + name1 + "<br /><b>0</b><input type=\"checkbox\" name=\"Num1\" value=\"0\" /> ");
            builder.Append("<b>1</b><input type=\"checkbox\" name=\"Num1\" value=\"1\" /> ");
            builder.Append("<b>2</b><input type=\"checkbox\" name=\"Num1\" value=\"2\" /> ");
            builder.Append("<b>3</b><input type=\"checkbox\" name=\"Num1\" value=\"3\" /> ");
            builder.Append("<b>4</b><input type=\"checkbox\" name=\"Num1\" value=\"4\" /> ");
            builder.Append("<b>5</b><input type=\"checkbox\" name=\"Num1\" value=\"5\" /> ");
            builder.Append("<b>6</b><input type=\"checkbox\" name=\"Num1\" value=\"6\" /> ");
            builder.Append("<b>7</b><input type=\"checkbox\" name=\"Num1\" value=\"7\" /> ");
            builder.Append("<b>8</b><input type=\"checkbox\" name=\"Num1\" value=\"8\" /> ");
            builder.Append("<b>9</b><input type=\"checkbox\" name=\"Num1\" value=\"9\" />");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("每注投注：<input type=\"text\" name=\"Price\" value=\"\" style =\"width:100px;height:25px;\" /> " + ub.Get("SiteBz") + " ");
            builder.Append("<input class=\"btn-red\" type=\"submit\"  name=\"ac99\"  value=\"确定\" style =\"height:25px;\" />");
            try
            {
                builder.Append("<br />∟");
                kuai(meid, 1, ptype);//快捷下注|用户ID|快捷下注游戏编号：时时彩为1|下注类型
            }
            catch { }
            builder.Append(Out.Tab("</div>", ""));

        }
        else if (ptype == 16)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("选择<f style=\"color:red\">任意一个号码</f><br /><b>0</b><input type=\"radio\" name=\"Num5\" value=\"0\" /> ");
            builder.Append("<b>1</b><input type=\"radio\" name=\"Num5\" value=\"1\" /> ");
            builder.Append("<b>2</b><input type=\"radio\" name=\"Num5\" value=\"2\" /> ");
            builder.Append("<b>3</b><input type=\"radio\" name=\"Num5\" value=\"3\" /> ");
            builder.Append("<b>4</b><input type=\"radio\" name=\"Num5\" value=\"4\" /> ");
            builder.Append("<b>5</b><input type=\"radio\" name=\"Num5\" value=\"5\" /> ");
            builder.Append("<b>6</b><input type=\"radio\" name=\"Num5\" value=\"6\" /> ");
            builder.Append("<b>7</b><input type=\"radio\" name=\"Num5\" value=\"7\" /> ");
            builder.Append("<b>8</b><input type=\"radio\" name=\"Num5\" value=\"8\" /> ");
            builder.Append("<b>9</b><input type=\"radio\" name=\"Num5\" value=\"9\" />");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("每注投注：<input type=\"text\" name=\"Price\" value=\"\" style =\"width:100px;height:25px;\" /> " + ub.Get("SiteBz") + " ");
            builder.Append("<input class=\"btn-red\" type=\"submit\"  name=\"ac99\"  value=\"确定\" style =\"height:25px;\" /> ");
            try
            {
                builder.Append("<br />∟");
                kuai(meid, 1, ptype);//快捷下注|用户ID|快捷下注游戏编号：时时彩为1|下注类型
            }
            catch { }
            builder.Append(Out.Tab("</div>", ""));
        }
        else if (ptype == 24)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("选择<f style=\"color:red\">牛几</f><br /><b>0</b><input type=\"checkbox\" name=\"Num5\" value=\"0\" /> ");
            builder.Append("<b>1</b><input type=\"checkbox\" name=\"Num5\" value=\"1\" /> ");
            builder.Append("<b>2</b><input type=\"checkbox\" name=\"Num5\" value=\"2\" /> ");
            builder.Append("<b>3</b><input type=\"checkbox\" name=\"Num5\" value=\"3\" /> ");
            builder.Append("<b>4</b><input type=\"checkbox\" name=\"Num5\" value=\"4\" /> ");
            builder.Append("<b>5</b><input type=\"checkbox\" name=\"Num5\" value=\"5\" /> ");
            builder.Append("<b>6</b><input type=\"checkbox\" name=\"Num5\" value=\"6\" /> ");
            builder.Append("<b>7</b><input type=\"checkbox\" name=\"Num5\" value=\"7\" /> ");
            builder.Append("<b>8</b><input type=\"checkbox\" name=\"Num5\" value=\"8\" /> ");
            builder.Append("<b>9</b><input type=\"checkbox\" name=\"Num5\" value=\"9\" />");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("每注投注：<input type=\"text\" name=\"Price\" value=\"\" style =\"width:100px;height:25px;\" /> " + ub.Get("SiteBz") + " ");
            builder.Append("<input class=\"btn-red\" type=\"submit\"  name=\"ac99\"  value=\"确定\" style =\"height:25px;\" /> ");
            try
            {
                builder.Append("<br />∟");
                kuai(meid, 1, ptype);//快捷下注|用户ID|快捷下注游戏编号：时时彩为1|下注类型
            }
            catch { }
            builder.Append(Out.Tab("</div>", ""));
        }
        else if (ptype == 27)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("选择<f style=\"color:red\">几门</f><br /><b>一门（ 0 - 9 ）</b><input type=\"radio\" name=\"Num5\" value=\"0\" /><br />");
            builder.Append("<b>二门（10-19）</b><input type=\"radio\" name=\"Num5\" value=\"1\" /><br />");
            builder.Append("<b>三门（20-29）</b><input type=\"radio\" name=\"Num5\" value=\"2\" /><br />");
            builder.Append("<b>四门（30-39）</b><input type=\"radio\" name=\"Num5\" value=\"3\" /><br />");
            builder.Append("<b>五门（40-45）</b><input type=\"radio\" name=\"Num5\" value=\"4\" /> ");

            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("每注投注：<input type=\"text\" name=\"Price\" value=\"\" style =\"width:100px;height:25px;\" /> " + ub.Get("SiteBz") + " ");
            builder.Append("<input class=\"btn-red\" type=\"submit\" name=\"ac99\"   value=\"确定\" style =\"height:25px;\" />  ");
            try
            {
                builder.Append("<br />∟");
                kuai(meid, 1, ptype);//快捷下注|用户ID|快捷下注游戏编号：时时彩为1|下注类型
            }
            catch { }
            builder.Append(Out.Tab("</div>", ""));
        }
        else if (ptype >= 29 && ptype <= 35)//梭哈玩法
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("投注：<input type=\"text\" name=\"Price\" value=\"\" style =\"width:100px;height:25px;\" /> " + ub.Get("SiteBz") + " ");
            builder.Append("<input class=\"btn-red\" type=\"submit\" name=\"ac99\"   value=\"确定\" style =\"height:25px;\" /> ");
            try
            {
                builder.Append("<br />∟");
                kuai(meid, 1, ptype);//快捷下注|用户ID|快捷下注游戏编号：时时彩为1|下注类型
            }
            catch { }
            builder.Append(Out.Tab("</div>", ""));
        }
        else if (ptype >= 38 && ptype <= 42)//前三特殊玩法
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("投注：<input type=\"text\" name=\"Price\" value=\"\" style =\"width:100px;height:25px;\" /> " + ub.Get("SiteBz") + " ");
            builder.Append("<input class=\"btn-red\" type=\"submit\"  name=\"ac99\"  value=\"确定\" style =\"height:25px;\" /> ");
            try
            {
                builder.Append("<br />∟");
                kuai(meid, 1, ptype);//快捷下注|用户ID|快捷下注游戏编号：时时彩为1|下注类型
            }
            catch { }
            builder.Append(Out.Tab("</div>", ""));
        }
        else if (ptype >= 45 && ptype <= 49)//中三特殊玩法
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("投注：<input type=\"text\" name=\"Price\" value=\"\" style =\"width:100px;height:25px;\" /> " + ub.Get("SiteBz") + " ");
            builder.Append("<input class=\"btn-red\" type=\"submit\"  name=\"ac99\"  value=\"确定\" style =\"height:25px;\" /> ");
            try
            {
                builder.Append("<br />∟");
                kuai(meid, 1, ptype);//快捷下注|用户ID|快捷下注游戏编号：时时彩为1|下注类型
            }
            catch { }
            builder.Append(Out.Tab("</div>", ""));
        }
        else if (ptype >= 52 && ptype <= 56)//后三特殊玩法
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("投注：<input type=\"text\" name=\"Price\" value=\"\" style =\"width:100px;height:25px;\" /> " + ub.Get("SiteBz") + " ");
            builder.Append("<input class=\"btn-red\" type=\"submit\"  name=\"ac99\"  value=\"确定\" style =\"height:25px;\" /> ");
            try
            {
                builder.Append("<br />∟");
                kuai(meid, 1, ptype);//快捷下注|用户ID|快捷下注游戏编号：时时彩为1|下注类型
            }
            catch { }
            builder.Append(Out.Tab("</div>", ""));
        }

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<input type=\"hidden\" name=\"act\" Value=\"pay\"/>");
        builder.Append("<input type=\"hidden\" name=\"ptype\" Value=\"" + ptype + "\"/>");
        string VE = ConfigHelper.GetConfigString("VE");
        string SID = ConfigHelper.GetConfigString("SID");
        builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
        builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
        builder.Append(Out.Tab("</div>", ""));
        builder.Append("</form>");


        if (ptype == 2 || ptype == 5 || ptype == 8 || ptype == 11 || ptype == 14 || ptype == 25 || ptype == 36 || ptype == 43 || ptype == 50 || ptype == 3 || ptype == 6 || ptype == 9 || ptype == 12 || ptype == 15 || ptype == 26 || ptype == 37 || ptype == 44 || ptype == 51 || ptype == 23)
        {
            //string buyname1 = string.Empty;
            //string buyname2 = string.Empty;
            if (ptype == 2 || ptype == 5 || ptype == 8 || ptype == 11 || ptype == 14 || ptype == 25 || ptype == 36 || ptype == 43 || ptype == 50)
            {
                //buyname1 = "押大"; buyname2 = "押小";
                builder.Append("<form id=\"form12\" method=\"post\" action=\"ssc.aspx\" style=\"line-height:30px;\">");
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("投注：<input type=\"text\" name=\"Price1\" value=\"\" style =\"width:100px;height:25px;\" /> " + ub.Get("SiteBz") + " ");
                builder.Append(" <input class=\"btn-red\" type=\"submit\" name=\"ddn10\" value=\"押大\" style =\"height:25px;\" />");
                try { builder.Append("<br />押大："); kuait(meid, 1, ptype, 1); }
                catch { }
                builder.Append(Out.Tab("</div>", "<br />"));

                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<input type=\"hidden\" name=\"act\" Value=\"pay\"/>");
                builder.Append("<input type=\"hidden\" name=\"ptype\" Value=\"" + ptype + "\"/>");
                VE = ConfigHelper.GetConfigString("VE");
                SID = ConfigHelper.GetConfigString("SID");
                builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
                builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
                builder.Append(Out.Tab("</div>", ""));
                builder.Append("</form>");


                builder.Append("<form id=\"form13\" method=\"post\" action=\"ssc.aspx\" style=\"line-height:30px;\">");
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("投注：<input type=\"text\" name=\"Price2\" value=\"\" style =\"width:100px;height:25px;\" /> " + ub.Get("SiteBz") + " ");
                builder.Append(" <input class=\"btn-red\" type=\"submit\" name=\"xsn10\" value=\"押小\" style =\"height:25px;\" />");
                try { builder.Append("<br />押小："); kuait(meid, 1, ptype, 2); }
                catch { }
                builder.Append(Out.Tab("</div>", ""));

                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<input type=\"hidden\" name=\"act\" Value=\"pay\"/>");
                builder.Append("<input type=\"hidden\" name=\"ptype\" Value=\"" + ptype + "\"/>");
                VE = ConfigHelper.GetConfigString("VE");
                SID = ConfigHelper.GetConfigString("SID");
                builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
                builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
                builder.Append(Out.Tab("</div>", ""));
                builder.Append("</form>");
            }
            if (ptype == 3 || ptype == 6 || ptype == 9 || ptype == 12 || ptype == 15 || ptype == 26 || ptype == 37 || ptype == 44 || ptype == 51)
            {
                //  buyname1 = "押单"; buyname2 = "押双";
                builder.Append("<form id=\"form14\" method=\"post\" action=\"ssc.aspx\" style=\"line-height:30px;\">");
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("投注：<input type=\"text\" name=\"Price1\" value=\"\" style =\"width:100px;height:25px;\" /> " + ub.Get("SiteBz") + " ");
                builder.Append(" <input class=\"btn-red\" type=\"submit\" name=\"ddnds10\" value=\"押单\" style =\"height:25px;\" />");
                try { builder.Append("<br />押单："); kuait(meid, 1, ptype, 1); }
                catch { }
                builder.Append(Out.Tab("</div>", "<br />"));

                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<input type=\"hidden\" name=\"act\" Value=\"pay\"/>");
                builder.Append("<input type=\"hidden\" name=\"ptype\" Value=\"" + ptype + "\"/>");
                VE = ConfigHelper.GetConfigString("VE");
                SID = ConfigHelper.GetConfigString("SID");
                builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
                builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
                builder.Append(Out.Tab("</div>", ""));
                builder.Append("</form>");


                builder.Append("<form id=\"form15\" method=\"post\" action=\"ssc.aspx\" style=\"line-height:30px;\">");
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("投注：<input type=\"text\" name=\"Price2\" value=\"\" style =\"width:100px;height:25px;\" /> " + ub.Get("SiteBz") + " ");
                builder.Append(" <input class=\"btn-red\" type=\"submit\" name=\"xsnds10\" value=\"押双\" style =\"height:25px;\" />");
                try { builder.Append("<br />押双："); kuait(meid, 1, ptype, 2); }
                catch { }
                builder.Append(Out.Tab("</div>", ""));

                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<input type=\"hidden\" name=\"act\" Value=\"pay\"/>");
                builder.Append("<input type=\"hidden\" name=\"ptype\" Value=\"" + ptype + "\"/>");
                VE = ConfigHelper.GetConfigString("VE");
                SID = ConfigHelper.GetConfigString("SID");
                builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
                builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
                builder.Append(Out.Tab("</div>", ""));
                builder.Append("</form>");
            }
            if (ptype == 23)
            {
                //  buyname1 = "有牛"; buyname2 = "无牛";
                builder.Append("<form id=\"form16\" method=\"post\" action=\"ssc.aspx\" style=\"line-height:30px;\">");
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("投注：<input type=\"text\" name=\"Price1\" value=\"\" style =\"width:100px;height:25px;\" /> " + ub.Get("SiteBz") + " ");
                builder.Append(" <input class=\"btn-red\" type=\"submit\" name=\"ddnyn10\" value=\"有牛\" style =\"height:25px;\" />");
                try { builder.Append("<br />有牛："); kuait(meid, 1, ptype, 1); }
                catch { }
                builder.Append(Out.Tab("</div>", "<br />"));

                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<input type=\"hidden\" name=\"act\" Value=\"pay\"/>");
                builder.Append("<input type=\"hidden\" name=\"ptype\" Value=\"" + ptype + "\"/>");
                VE = ConfigHelper.GetConfigString("VE");
                SID = ConfigHelper.GetConfigString("SID");
                builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
                builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
                builder.Append(Out.Tab("</div>", ""));
                builder.Append("</form>");


                builder.Append("<form id=\"form17\" method=\"post\" action=\"ssc.aspx\" style=\"line-height:30px;\">");
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("投注：<input type=\"text\" name=\"Price2\" value=\"\" style =\"width:100px;height:25px;\" /> " + ub.Get("SiteBz") + " ");
                builder.Append(" <input class=\"btn-red\" type=\"submit\" name=\"xsnyn10\" value=\"无牛\" style =\"height:25px;\" />");
                try { builder.Append("<br />无牛："); kuait(meid, 1, ptype, 2); }
                catch { }
                builder.Append(Out.Tab("</div>", ""));

                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<input type=\"hidden\" name=\"act\" Value=\"pay\"/>");
                builder.Append("<input type=\"hidden\" name=\"ptype\" Value=\"" + ptype + "\"/>");
                VE = ConfigHelper.GetConfigString("VE");
                SID = ConfigHelper.GetConfigString("SID");
                builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
                builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
                builder.Append(Out.Tab("</div>", ""));
                builder.Append("</form>");
            }

        }
        if (ptype == 17)//龙虎和
        {
            builder.Append("<form id=\"form2\" method=\"post\" action=\"ssc.aspx\" style=\"line-height:30px;\">");
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("投注：<input type=\"text\" name=\"Price1\" value=\"\" style =\"width:100px;height:25px;\" /> " + ub.Get("SiteBz") + " ");
            builder.Append(" <input class=\"btn-red\" type=\"submit\" name=\"yl10\" value=\"押龙\" style =\"height:25px;\" /> ");
            try
            {
                builder.Append("<br />押龙：");
                kuait(meid, 1, ptype, 1);//用户ID|游戏ID|下注类型|特殊下注的编号：龙
            }
            catch { }
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<input type=\"hidden\" name=\"act\" Value=\"pay\"/>");
            builder.Append("<input type=\"hidden\" name=\"ptype\" Value=\"" + ptype + "\"/>");
            VE = ConfigHelper.GetConfigString("VE");
            SID = ConfigHelper.GetConfigString("SID");
            builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
            builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
            builder.Append(Out.Tab("</div>", ""));
            builder.Append("</form>");

            builder.Append("<form id=\"form3\" method=\"post\" action=\"ssc.aspx\" style=\"line-height:30px;\">");
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("投注：<input type=\"text\" name=\"Price2\" value=\"\" style =\"width:100px;height:25px;\" /> " + ub.Get("SiteBz") + " ");
            builder.Append(" <input class=\"btn-red\" type=\"submit\" name=\"yh10\"  value=\"押虎\" style =\"height:25px;\" />");
            try
            {
                builder.Append("<br />押虎：");
                kuait(meid, 1, ptype, 2);
            }
            catch { }
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<input type=\"hidden\" name=\"act\" Value=\"pay\"/>");
            builder.Append("<input type=\"hidden\" name=\"ptype\" Value=\"" + ptype + "\"/>");
            VE = ConfigHelper.GetConfigString("VE");
            SID = ConfigHelper.GetConfigString("SID");
            builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
            builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
            builder.Append(Out.Tab("</div>", ""));
            builder.Append("</form>");

            builder.Append("<form id=\"form4\" method=\"post\" action=\"ssc.aspx\" style=\"line-height:30px;\">");
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("投注：<input type=\"text\" name=\"Price3\" value=\"\" style =\"width:100px;height:25px;\" /> " + ub.Get("SiteBz") + " ");
            builder.Append(" <input class=\"btn-red\" type=\"submit\" name=\"yhe10\"  value=\"押和\" style =\"height:25px;\" />");
            try
            {
                builder.Append("<br />押和：");
                kuait(meid, 1, ptype, 3);
            }
            catch { }
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<input type=\"hidden\" name=\"act\" Value=\"pay\"/>");
            builder.Append("<input type=\"hidden\" name=\"ptype\" Value=\"" + ptype + "\"/>");
            VE = ConfigHelper.GetConfigString("VE");
            SID = ConfigHelper.GetConfigString("SID");
            builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
            builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
            builder.Append(Out.Tab("</div>", ""));
            builder.Append("</form>");
        }
        if (ptype == 28)//总和大小单双
        {
            builder.Append("<form id=\"form2\" method=\"post\" action=\"ssc.aspx\" style=\"line-height:30px;\">");
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("投注：<input type=\"text\" name=\"Price1\" value=\"\" style =\"width:100px;height:25px;\" /> " + ub.Get("SiteBz") + " ");
            builder.Append(" <input class=\"btn-red\" type=\"submit\" name=\"dd10\" value=\"大单\" style =\"height:25px;\" />");
            try { builder.Append("<br />大单："); kuait(meid, 1, ptype, 1); }
            catch { }
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<input type=\"hidden\" name=\"act\" Value=\"pay\"/>");
            builder.Append("<input type=\"hidden\" name=\"ptype\" Value=\"" + ptype + "\"/>");
            VE = ConfigHelper.GetConfigString("VE");
            SID = ConfigHelper.GetConfigString("SID");
            builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
            builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
            builder.Append(Out.Tab("</div>", ""));
            builder.Append("</form>");

            builder.Append("<form id=\"form3\" method=\"post\" action=\"ssc.aspx\" style=\"line-height:30px;\">");
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("投注：<input type=\"text\" name=\"Price2\" value=\"\" style =\"width:100px;height:25px;\" /> " + ub.Get("SiteBz") + " ");
            builder.Append(" <input class=\"btn-red\" type=\"submit\" name=\"ds10\"  value=\"大双\" style =\"height:25px;\" />");
            try { builder.Append("<br />大双："); kuait(meid, 1, ptype, 2); }
            catch { }
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<input type=\"hidden\" name=\"act\" Value=\"pay\"/>");
            builder.Append("<input type=\"hidden\" name=\"ptype\" Value=\"" + ptype + "\"/>");
            VE = ConfigHelper.GetConfigString("VE");
            SID = ConfigHelper.GetConfigString("SID");
            builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
            builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
            builder.Append(Out.Tab("</div>", ""));
            builder.Append("</form>");

            builder.Append("<form id=\"form4\" method=\"post\" action=\"ssc.aspx\" style=\"line-height:30px;\">");
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("投注：<input type=\"text\" name=\"Price3\" value=\"\" style =\"width:100px;height:25px;\" /> " + ub.Get("SiteBz") + " ");
            builder.Append(" <input class=\"btn-red\" type=\"submit\"  name=\"xd10\" value=\"小单\" style =\"height:25px;\" />");
            try { builder.Append("<br />小单："); kuait(meid, 1, ptype, 3); }
            catch { }
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<input type=\"hidden\" name=\"act\" Value=\"pay\"/>");
            builder.Append("<input type=\"hidden\" name=\"ptype\" Value=\"" + ptype + "\"/>");
            VE = ConfigHelper.GetConfigString("VE");
            SID = ConfigHelper.GetConfigString("SID");
            builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
            builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
            builder.Append(Out.Tab("</div>", ""));
            builder.Append("</form>");

            builder.Append("<form id=\"form5\" method=\"post\" action=\"ssc.aspx\" style=\"line-height:30px;\">");
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("投注：<input type=\"text\" name=\"Price4\" value=\"\" style =\"width:100px;height:25px;\" /> " + ub.Get("SiteBz") + " ");
            builder.Append(" <input class=\"btn-red\" type=\"submit\"  name=\"xs10\" value=\"小双\" style =\"height:25px;\" />");
            try { builder.Append("<br />小双："); kuait(meid, 1, ptype, 4); }
            catch { }
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<input type=\"hidden\" name=\"act\" Value=\"pay\"/>");
            builder.Append("<input type=\"hidden\" name=\"ptype\" Value=\"" + ptype + "\"/>");
            VE = ConfigHelper.GetConfigString("VE");
            SID = ConfigHelper.GetConfigString("SID");
            builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
            builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
            builder.Append(Out.Tab("</div>", ""));
            builder.Append("</form>");
        }

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=" + ptype + "") + "\">清空选号</a><br />");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", ""));
        if (ptype == 17)
        {
            builder.Append("<b>玩法提示  </b>" + "赔率：龙:" + Out.SysUBB(OutOdds(ptype, 1)) + "倍 虎:" + Out.SysUBB(OutOdds(ptype, 2)) + "倍 和:" + Out.SysUBB(OutOdds(ptype, 3)) + "倍 <br />" + Out.SysUBB(OutRule(ptype)) + "");
        }
        else if (ptype == 23)
        {
            builder.Append("<b>玩法提示  </b>" + "有牛赔率：1:" + Out.SysUBB(OutOdds(ptype, 1)) + "  无牛赔率：1:" + Out.SysUBB(OutOdds(ptype, 2)) + "<br />" + Out.SysUBB(OutRule(ptype)) + "");
        }
        else if (ptype == 27)
        {
            builder.Append("<b>玩法提示  </b><br />" + "一门： 0 - 9  赔率：1:" + Out.SysUBB(OutOdds(ptype, 1)) + "<br />二门：10-19 赔率：1:" + Out.SysUBB(OutOdds(ptype, 2)) + "<br />三门：20-29 赔率：1:" + Out.SysUBB(OutOdds(ptype, 3)) + "<br />四门：30-39 赔率：1:" + Out.SysUBB(OutOdds(ptype, 4)) + "<br />五门：40-45 赔率：1:" + Out.SysUBB(OutOdds(ptype, 5)) + "<br />" + Out.SysUBB(OutRule(ptype)) + "");
        }
        else if (ptype == 28)
        {
            builder.Append("<b>玩法提示  </b>" + "大单、小双赔率：1:" + Out.SysUBB(OutOdds(ptype, 1)) + "。大双、小单赔率：1:" + Out.SysUBB(OutOdds(ptype, 2)) + "<br />" + Out.SysUBB(OutRule(ptype)) + "");
        }
        else
        {
            if (ptype == 2 || ptype == 3 || ptype == 5 || ptype == 6 || ptype == 8 || ptype == 9 || ptype == 11 || ptype == 12 || ptype == 14 || ptype == 15 || ptype == 25 || ptype == 26 || ptype == 36 || ptype == 37 || ptype == 43 || ptype == 44 || ptype == 50 || ptype == 51)
            {
                if (ptype == 2 || ptype == 5 || ptype == 8 || ptype == 11 || ptype == 14 || ptype == 25 || ptype == 36 || ptype == 43 || ptype == 50)
                {
                    builder.Append("<b>玩法提示  </b>" + "押大赔率：1:" + Out.SysUBB(OutOdds(ptype, 1)) + " 押小赔率：1:" + Out.SysUBB(OutOdds(ptype, 2)) + "<br />" + Out.SysUBB(OutRule(ptype)) + "");
                }
                else
                {
                    builder.Append("<b>玩法提示  </b>" + "押单赔率：1:" + Out.SysUBB(OutOdds(ptype, 1)) + " 押双赔率：1:" + Out.SysUBB(OutOdds(ptype, 2)) + "<br />" + Out.SysUBB(OutRule(ptype)) + "");
                }
            }
            else
            {
                builder.Append("<b>玩法提示  </b>" + "赔率：1:" + Out.SysUBB(OutOdds(ptype, 1)) + "<br />" + Out.SysUBB(OutRule(ptype)) + "");
            }
        }
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 下注页面 PayPage
    /// <summary>
    /// 下注页面
    /// </summary>
    private void PayPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();


        int LianTing = Convert.ToInt32(ub.GetSub("SSCLianTing", xmlPath));//连停期数 只考虑ID，不考虑数据库里面的期数
        int IDlastopen = 0;
        int IDlastnew = 0;
        DataSet dslastopen = new BCW.ssc.BLL.SSClist().GetList("Top(1) ID,SSCId,Result,State", " State=1 Order by ID DESC");
        if (dslastopen != null && dslastopen.Tables[0].Rows.Count > 0)
        {
            IDlastopen = Convert.ToInt32(dslastopen.Tables[0].Rows[0]["ID"]);//最后一条开奖的ID
        }
        DataSet dslastnew = new BCW.ssc.BLL.SSClist().GetList("Top(1) ID,SSCId,Result,State", " State=0 Order by ID DESC");
        if (dslastnew != null && dslastnew.Tables[0].Rows.Count > 0)
        {
            IDlastnew = Convert.ToInt32(dslastnew.Tables[0].Rows[0]["ID"]);//最新期数
        }

        if (dslastnew.Tables[0].Rows.Count >= LianTing)
        {
            if (IDlastnew - IDlastopen >= LianTing)//判断连停
            {
                Utils.Error("由于程序出现多期未开奖情况，现暂停投注，请等待程序恢复正常再来下注！谢谢，祝您愉快！", "");
            }
        }

        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 2, @"^5[0-6]$|^[1-4]\d$|^[1-9]$", "类型选择错误"));
        long Price = Utils.ParseInt64(Utils.GetRequest("Price", "all", 1, @"^[1-9]\d*$", "投注额填写错误"));
        long Price1 = Utils.ParseInt64(Utils.GetRequest("Price1", "all", 1, @"^[1-9]\d*$", "投注额填写错误"));
        long Price2 = Utils.ParseInt64(Utils.GetRequest("Price2", "all", 1, @"^[1-9]\d*$", "投注额填写错误"));
        long Price3 = Utils.ParseInt64(Utils.GetRequest("Price3", "all", 1, @"^[1-9]\d*$", "投注额填写错误"));
        long Price4 = Utils.ParseInt64(Utils.GetRequest("Price4", "all", 1, @"^[1-9]\d*$", "投注额填写错误"));

        string yhe = Utils.GetRequest("yhe", "all", 1, "", "");
        string yheq = string.Empty;
        for (int i = 0; i <= 11; i++)
        {
            yheq = Utils.GetRequest("yhe" + i + "", "all", 1, "", "");
            if (yheq != "") yhe = yheq;
        }

        string dd = Utils.GetRequest("dd", "all", 1, "", "");
        string ddq = string.Empty;
        for (int i = 0; i <= 11; i++)
        {
            ddq = Utils.GetRequest("dd" + i + "", "all", 1, "", "");
            if (ddq != "") dd = ddq;
        }
        string ds = Utils.GetRequest("ds", "all", 1, "", "");
        string dsq = string.Empty;
        for (int i = 0; i <= 11; i++)
        {
            dsq = Utils.GetRequest("ds" + i + "", "all", 1, "", "");
            if (dsq != "") ds = dsq;
        }
        string xd = Utils.GetRequest("xd", "all", 1, "", "");
        string xdq = string.Empty;
        for (int i = 0; i <= 11; i++)
        {
            xdq = Utils.GetRequest("xd" + i + "", "all", 1, "", "");
            if (xdq != "") xd = xdq;
        }
        string xs = Utils.GetRequest("xs", "all", 1, "", "");
        string xsq = string.Empty;
        for (int i = 0; i <= 11; i++)
        {
            xsq = Utils.GetRequest("xs" + i + "", "all", 1, "", "");
            if (xsq != "") xs = xsq;
        }
        string ddn = Utils.GetRequest("ddn", "all", 1, "", "");
        string ddnq = string.Empty;
        for (int i = 0; i <= 11; i++)
        {
            ddnq = Utils.GetRequest("ddn" + i + "", "all", 1, "", "");
            if (ddnq != "") ddn = ddnq;
        }

        string ddnds = Utils.GetRequest("ddnds", "all", 1, "", "");
        string ddndsq = string.Empty;
        for (int i = 0; i <= 11; i++)
        {
            ddndsq = Utils.GetRequest("ddnds" + i + "", "all", 1, "", "");
            if (ddndsq != "") ddnds = ddndsq;
        }
        string ddnyn = Utils.GetRequest("ddnyn", "all", 1, "", "");
        string ddnynq = string.Empty;
        for (int i = 0; i <= 11; i++)
        {
            ddnynq = Utils.GetRequest("ddnyn" + i + "", "all", 1, "", "");
            if (ddnynq != "") ddnyn = ddnynq;
        }

        string xsn = Utils.GetRequest("xsn", "all", 1, "", "");
        string xsnq = string.Empty;
        for (int i = 0; i <= 11; i++)
        {
            xsnq = Utils.GetRequest("xsn" + i + "", "all", 1, "", "");
            if (xsnq != "") xsn = xsnq;
        }

        string xsnds = Utils.GetRequest("xsnds", "all", 1, "", "");
        string xsndsq = string.Empty;
        for (int i = 0; i <= 11; i++)
        {
            xsndsq = Utils.GetRequest("xsnds" + i + "", "all", 1, "", "");
            if (xsndsq != "") xsnds = xsndsq;
        }
        string xsnyn = Utils.GetRequest("xsnyn", "all", 1, "", "");
        string xsnynq = string.Empty;
        for (int i = 0; i <= 11; i++)
        {
            xsnynq = Utils.GetRequest("xsnyn" + i + "", "all", 1, "", "");
            if (xsnynq != "") xsnyn = xsnynq;
        }
        string ace = Utils.GetRequest("ace", "all", 1, "", "");
        string aceq = string.Empty;
        for (int i = 0; i < 11; i++)
        {
            aceq = Utils.GetRequest("ace" + i + "", "all", 1, "", "");
            if (aceq != "") ace = aceq;
        }
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        string acq = string.Empty;
        for (int i = 0; i < 11; i++)
        {
            acq = Utils.GetRequest("ac" + i + "", "all", 1, "", "");
            if (acq != "") ac = acq;
        }
        string yl = Utils.GetRequest("yl", "all", 1, "", "");
        string ylq = string.Empty;
        for (int i = 0; i <= 11; i++)
        {
            ylq = Utils.GetRequest("yl" + i + "", "all", 1, "", "");
            if (ylq != "") yl = ylq;
        }
        string yh = Utils.GetRequest("yh", "all", 1, "", "");
        string yhq = string.Empty;
        for (int i = 0; i <= 11; i++)
        {
            yhq = Utils.GetRequest("yh" + i + "", "all", 1, "", "");
            if (yhq != "") yh = yhq;
        }



        //Utils.Error("" + ace, "");

        if (ptype == 1 || ptype == 4 || ptype == 7 || ptype == 10 || ptype == 13 || ptype == 18 || ptype == 19 || ptype == 20 || ptype == 21 || ptype == 22 || ptype == 16 || ptype == 24 || ptype == 27 || (ptype >= 29 && ptype <= 35) || (ptype >= 38 && ptype <= 42) || (ptype >= 45 && ptype <= 49) || (ptype >= 52 && ptype <= 56))
        {
            if (ac != "确定")
            {
                if (ac.Contains("万"))
                {
                    //if (ac.Contains(".X"))
                    //{
                    //    string str = string.Empty;
                    //    str = ace;
                    //    Price = Convert.ToInt64(str);
                    //}
                    //else
                    {
                        string str = string.Empty;
                        str = ac.Replace("万", "");
                        Price = Convert.ToInt64(Convert.ToDouble(str) * 10000);
                    }
                }
                else
                {
                    try
                    {
                        Price = Convert.ToInt64(ac);
                    }
                    catch { }
                }
            }
        }

        string info = Utils.GetRequest("info", "post", 1, "", "");
        string TypeTitle = OutType(ptype);
        Master.Title = TypeTitle;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx") + "\">" + GameName + "</a>&gt;" + TypeTitle + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        BCW.ssc.Model.SSClist model = new BCW.ssc.BLL.SSClist().GetSSClistLast();
        if (model.ID == 0)
        {
            Utils.Error("正在处理开奖，请稍后...", Utils.getUrl("ssc.aspx"));
        }
        int Sec = Utils.ParseInt(ub.GetSub("SSCSec", xmlPath));
        if (model.EndTime < DateTime.Now.AddSeconds(Sec))
        {
            Utils.Error("第" + model.SSCId + "期已截止下注,等待开奖...", Utils.getUrl("ssc.aspx"));
        }

        string Num5 = Utils.GetRequest("Num5", "all", 1, @"^[\d((,)\d)?]+$", "");
        string Num4 = Utils.GetRequest("Num4", "all", 1, @"^[\d((,)\d)?]+$", "");
        string Num3 = Utils.GetRequest("Num3", "all", 1, @"^[\d((,)\d)?]+$", "");
        string Num2 = Utils.GetRequest("Num2", "all", 1, @"^[\d((,)\d)?]+$", "");
        string Num1 = Utils.GetRequest("Num1", "all", 1, @"^[\d((,)\d)?]+$", "");

        // string accNum = string.Empty;
        string accNum = Utils.GetRequest("accNum", "all", 1, @"^[^\^]{1,20000}$", "");
        string accNum2 = string.Empty;
        string[] strTemp = { };
        int iZhu = 0;
        string[] str5 = Num5.Split(',');
        string[] str4 = Num4.Split(',');
        string[] str3 = Num3.Split(',');
        string[] str2 = Num2.Split(',');
        string[] str1 = Num1.Split(',');

        #region 位号
        if (ptype == 1)//万位直选
        {
            if (Num5 == "")
            {
                Utils.Error("至少选择1个号码", "");
            }
            if (Price == 0)
                Utils.Error("投注金额必须大于0", "");
            strTemp = new Combination().GetCombinationStr5(str5, str4, str3, str2, str1);

            accNum = Num5;
        }
        else if (ptype == 4)//千位直选
        {
            if (Num4 == "")
            {
                Utils.Error("至少选择1个号码", "");
            }
            if (Price == 0)
                Utils.Error("投注金额必须大于0", "");
            strTemp = new Combination().GetCombinationStr5(str5, str4, str3, str2, str1);

            accNum = Num4;
        }
        else if (ptype == 7)//百位直选
        {
            if (Num3 == "")
            {
                Utils.Error("至少选择1个号码", "");
            }
            if (Price == 0)
                Utils.Error("投注金额必须大于0", "");
            strTemp = new Combination().GetCombinationStr5(str5, str4, str3, str2, str1);

            accNum = Num3;
        }
        else if (ptype == 10)//十位直选
        {
            if (Num2 == "")
            {
                Utils.Error("至少选择1个号码", "");
            }
            if (Price == 0)
                Utils.Error("投注金额必须大于0", "");
            strTemp = new Combination().GetCombinationStr5(str5, str4, str3, str2, str1);

            accNum = Num2;
        }
        else if (ptype == 13)//个位直选
        {
            if (Num1 == "")
            {
                Utils.Error("至少选择1个号码", "");
            }
            if (Price == 0)
                Utils.Error("投注金额必须大于0", "");
            strTemp = new Combination().GetCombinationStr5(str5, str4, str3, str2, str1);

            accNum = Num1;
        }
        else if (ptype == 18)//任一
        {
            if (Num5 == "")
                Utils.Error("至少选择1个号码", "");
            if (Price == 0)
                Utils.Error("投注金额必须大于0", "");
            strTemp = new Combination().GetCombinationStr5(str5, str4, str3, str2, str1);
            accNum = Num5;
        }
        else if (ptype == 19)//任二
        {
            if (Price == 0)
                Utils.Error("投注金额必须大于0", "");
            if (Num1 == "")
                Utils.Error("至少选择一个拖码", "");
            if (Num1.Length + Num2.Length < 2)
                Utils.Error("胆码和拖码总个数不小于2", "");
            if (str2.Length > 1)
                Utils.Error("胆码最多选择1个（0-1个）", "");
            strTemp = new Combination().GetCombinationStr5(str5, str4, str3, str2, str1);
            accNum = Num2 + "#" + Num1;
        }
        else if (ptype == 20)//任三
        {
            if (Price == 0)
                Utils.Error("投注金额必须大于0", "");
            if (Num1 == "")
                Utils.Error("至少选择一个拖码", "");
            if (Num1.Split(',').Length + Num2.Length < 3)
                Utils.Error("胆码和拖码总个数不小于3", "");
            if (str2.Length > 2)
                Utils.Error("胆码最多选择2个（0-2个）", "");
            strTemp = new Combination().GetCombinationStr5(str5, str4, str3, str2, str1);
            accNum = Num2 + "#" + Num1;
        }
        else if (ptype == 21)//任四
        {
            if (Price == 0)
                Utils.Error("投注金额必须大于0", "");
            if (Num1 == "")
                Utils.Error("至少选择一个拖码", "");
            if (Num1.Split(',').Length + Num2.Length < 4)
                Utils.Error("胆码和拖码总个数不小于4", "");
            if (str2.Length > 3)
                Utils.Error("胆码最多选择3个（0-3个）", "");
            strTemp = new Combination().GetCombinationStr5(str5, str4, str3, str2, str1);
            accNum = Num2 + "#" + Num1;
        }
        else if (ptype == 22)//任五
        {
            if (Price == 0)
                Utils.Error("投注金额必须大于0", "");
            if (Num1 == "")
                Utils.Error("至少选择一个拖码", "");
            if (Num1.Split(',').Length + Num2.Length < 5)
                Utils.Error("胆码和拖码总个数不小于5", "");
            if (str2.Length > 4)
                Utils.Error("胆码最多选择4个（0-4个）", "");
            strTemp = new Combination().GetCombinationStr5(str5, str4, str3, str2, str1);
            accNum = Num2 + "#" + Num1;
        }
        else if (ptype == 16)//任选号码
        {
            if (Num5 == "")
                Utils.Error("至少选择一个号码", "");
            if (Price == 0)
                Utils.Error("投注金额必须大于0", "");
            strTemp = new Combination().GetCombinationStr5(str5, str4, str3, str2, str1);
            accNum = Num5;
        }
        else if (ptype == 27)//总和五门
        {
            if (Num5 == "")
                Utils.Error("至少选择几门", "");
            if (Price == 0)
                Utils.Error("投注金额必须大于0", "");
            strTemp = new Combination().GetCombinationStr5(str5, str4, str3, str2, str1);
            if (Num5 == "0")
                accNum = "一门";
            if (Num5 == "1")
                accNum = "二门";
            if (Num5 == "2")
                accNum = "三门";
            if (Num5 == "3")
                accNum = "四门";
            if (Num5 == "4")
                accNum = "五门";
        }
        else if (ptype == 24)
        {
            if (Num5 == "")
                Utils.Error("至少选择一个号码", "");
            if (Price == 0)
                Utils.Error("投注金额必须大于0", "");
            strTemp = new Combination().GetCombinationStr5(str5, str4, str3, str2, str1);
            accNum = Num5;
        }
        else if (ptype == 2 || ptype == 5 || ptype == 8 || ptype == 11 || ptype == 14 || ptype == 25 || ptype == 36 || ptype == 43 || ptype == 50)
        {
            if (ddn != "")//押大Price2 == 0
            {
                if (ddn == "押大")
                {
                    if (Price1 == 0)
                        Utils.Error("押大投注金额必须大于0", "");
                }
                accNum = "大";
            }
            if (xsn != "")//押小Price1 == 0
            {
                if (xsn == "押小")
                {
                    if (Price2 == 0)
                        Utils.Error("押小投注金额必须大于0", "");
                }
                accNum = "小";
            }
        }
        else if (ptype == 3 || ptype == 6 || ptype == 9 || ptype == 12 || ptype == 15 || ptype == 26 || ptype == 37 || ptype == 44 || ptype == 51)
        {
            if (ddnds != "")//押单Price2 == 0
            {
                if (ddnds == "押单")
                {
                    if (Price1 == 0)
                        Utils.Error("押单投注金额必须大于0", "");
                }
                accNum = "单";
            }
            if (xsnds != "")//押双Price1 == 0
            {
                if (xsnds == "押双")
                {
                    if (Price2 == 0)
                        Utils.Error("押双投注金额必须大于0", "");
                }
                accNum = "双";
            }
        }
        else if (ptype == 23)//有无牛牛
        {
            if (ddnyn != "")//有牛Price2 == 0
            {
                if (ddnyn == "有牛")
                {
                    if (Price1 == 0)
                        Utils.Error("有牛投注金额必须大于0", "");
                }
                accNum = "有牛";
            }
            if (xsnyn != "")//无牛Price1 == 0
            {
                if (xsnyn == "无牛")
                {
                    if (Price2 == 0)
                        Utils.Error("无牛投注金额必须大于0", "");
                }
                accNum = "无牛";
            }
        }
        else if (ptype == 17)//龙虎和
        {
            if (yl != "")
            {
                if (yl == "押龙")
                {
                    if (Price1 == 0)
                        Utils.Error("押龙投注金额必须大于0", "");
                }
                accNum = "龙";

            }
            if (yh != "")
            {
                if (yh == "押虎")
                {
                    if (Price2 == 0)
                        Utils.Error("押虎投注金额必须大于0", "");
                }
                accNum = "虎";
            }
            if (yhe != "")
            {
                if (yhe == "押和")
                {
                    if (Price3 == 0)
                        Utils.Error("押和投注金额必须大于0", "");
                }
                accNum = "和";
            }
        }
        else if (ptype == 28)//总和大小单双
        {
            if (dd != "")//Price2 == 0 && Price3 == 0 && Price4 == 0
            {
                if (dd == "大单")
                {
                    if (Price1 == 0)
                        Utils.Error("押总和大小单双投注大单金额必须大于0", "");
                }
                accNum = "大单";
            }
            if (ds != "")//Price1 == 0 && Price3 == 0 && Price4 == 0
            {
                if (ds == "大双")
                {
                    if (Price2 == 0)
                        Utils.Error("押总和大小单双投注大双金额必须大于0", "");
                }
                accNum = "大双";
            }
            if (xd != "")//Price1 == 0 && Price2 == 0 && Price4 == 0
            {
                if (xd == "小单")
                {
                    if (Price3 == 0)
                        Utils.Error("押总和大小单双投注小单金额必须大于0", "");
                }
                accNum = "小单";
            }
            if (xs != "")//Price1 == 0 && Price2 == 0 && Price3 == 0
            {
                if (xs == "小双")
                {
                    if (Price4 == 0)
                        Utils.Error("押总和大小单双投注小双金额必须大于0", "");
                }
                accNum = "小双";
            }
        }
        else if (ptype >= 29 && ptype <= 35)//梭哈玩法
        {
            if (Price == 0)
                Utils.Error("投注金额必须大于0", "");
            strTemp = new Combination().GetCombinationStr5(str5, str4, str3, str2, str1);
            if (ptype == 29)
                accNum = "炸弹";
            if (ptype == 30)
                accNum = "葫芦";
            if (ptype == 31)
                accNum = "顺子";
            if (ptype == 32)
                accNum = "三条";
            if (ptype == 33)
                accNum = "两对";
            if (ptype == 34)
                accNum = "单对";
            if (ptype == 35)
                accNum = "散牌";
        }
        else if (ptype >= 38 && ptype <= 42)//前三特殊玩法
        {
            if (Price == 0)
                Utils.Error("投注金额必须大于0", "");
            if (ptype == 38)
                accNum = "豹子";
            if (ptype == 39)
                accNum = "顺子";
            if (ptype == 40)
                accNum = "对子";
            if (ptype == 41)
                accNum = "半顺";
            if (ptype == 42)
                accNum = "杂六";
        }
        else if (ptype >= 45 && ptype <= 49)//中三特殊玩法
        {
            if (Price == 0)
                Utils.Error("投注金额必须大于0", "");
            if (ptype == 45)
                accNum = "豹子";
            if (ptype == 46)
                accNum = "顺子";
            if (ptype == 47)
                accNum = "对子";
            if (ptype == 48)
                accNum = "半顺";
            if (ptype == 49)
                accNum = "杂六";
        }
        else if (ptype >= 52 && ptype <= 56)//中三特殊玩法
        {
            if (Price == 0)
                Utils.Error("投注金额必须大于0", "");
            if (ptype == 52)
                accNum = "豹子";
            if (ptype == 53)
                accNum = "顺子";
            if (ptype == 54)
                accNum = "对子";
            if (ptype == 55)
                accNum = "半顺";
            if (ptype == 56)
                accNum = "杂六";
        }
        #endregion

        #region 判断胆拖不同号
        if (ptype >= 19 && ptype <= 22)//判断胆拖不同号
        {
            //查看胆码和拖码有没有相同的
            for (int i = 0; i < str1.Length; i++)
            {
                for (int p = 0; p < str2.Length; p++)
                {
                    if (str1[i] == str2[p])
                    {
                        Utils.Error("拖码不能和胆码相同,请重新选择号码", "");
                    }
                }
            }
        }
        #endregion

        #region 注数计算
        if (ptype == 1 || ptype == 4 || ptype == 7 || ptype == 10 || ptype == 13 || ptype == 18)//万、千、百、十、个位直选、任一
        {
            if (iZhu == 0)
                iZhu = strTemp.Length;
        }
        if (ptype == 19)//任二
        {
            if (Num2.Length == 1)
            {
                iZhu = str1.Length;//有一个胆码，注数为拖码的个数
            }
            else
            {
                iZhu = C(str1.Length, 2);//无胆码，注数为拖码的组合选2
            }
        }
        if (ptype == 20)//任三
        {
            if (Num2 == "")//无胆码，注数为拖码的组合选3
            {
                iZhu = C(str1.Length, 3);
            }
            else
            {
                if (str2.Length == 2)//2个胆码，注数为拖码的个数
                {
                    iZhu = str1.Length;
                }
                else if (str2.Length == 1)//1个胆码，注数为拖码的组合选2
                {
                    iZhu = C(str1.Length, 2);
                }
            }
        }
        if (ptype == 21)//任四
        {
            if (Num2 == "")//无胆码，注数为拖码的组合选4
            {
                iZhu = C(str1.Length, 4);
            }
            else
            {
                if (str2.Length == 3)//3个胆码，注数为拖码的个数
                {
                    iZhu = str1.Length;
                }
                else if (str2.Length == 2)//2个胆码，注数为拖码的组合选2
                {
                    iZhu = C(str1.Length, 2);
                }
                else if (str2.Length == 1)//1个胆码，注数为拖码的组合选3
                {
                    iZhu = C(str1.Length, 3);
                }
            }
        }
        if (ptype == 22)//任五
        {
            if (Num2 == "")//无胆码，注数为拖码的组合选5
            {
                iZhu = C(str1.Length, 5);
            }
            else
            {
                if (str2.Length == 4)//4个胆码，注数为拖码的个数
                {
                    iZhu = str1.Length;
                }
                else if (str2.Length == 3)//3个胆码，注数为拖码的组合选2
                {
                    iZhu = C(str1.Length, 2);
                }
                else if (str2.Length == 2)//2个胆码，注数为拖码的组合选3
                {
                    iZhu = C(str1.Length, 3);
                }
                else if (str2.Length == 1)//1个胆码，注数为拖码的组合选4
                {
                    iZhu = C(str1.Length, 4);
                }
            }
        }
        if (ptype == 16)//任选位数
        {
            iZhu = str5.Length;
        }
        if (ptype == 24)//特定牛牛
        {
            iZhu = str5.Length;
        }
        if (ptype == 23)//有无牛牛
        {
            iZhu = 1;
        }
        if (ptype == 2 || ptype == 5 || ptype == 8 || ptype == 11 || ptype == 14 || ptype == 25 || ptype == 36 || ptype == 43 || ptype == 50)//万千百十个 大小、总和大小、前三、中三，后三大小
        {
            iZhu = 1;
        }
        if (ptype == 3 || ptype == 6 || ptype == 9 || ptype == 12 || ptype == 15 || ptype == 26 || ptype == 37 || ptype == 44 || ptype == 51)//万千百十个 单双、总和单双、前三、中三，后三单双
        {
            iZhu = 1;
        }
        if (ptype == 17 || ptype == 27 || ptype == 28)//龙虎和,总和五门,总和大小单双
        {
            iZhu = 1;
        }
        if (ptype >= 29 && ptype <= 35)//梭哈玩法
        {
            iZhu = 1;
        }
        if (ptype >= 38 && ptype <= 42)//前三
        {
            iZhu = 1;
        }
        if (ptype >= 45 && ptype <= 49)//中三
        {
            iZhu = 1;
        }
        if (ptype >= 52 && ptype <= 56)//后三
        {
            iZhu = 1;
        }
        #endregion

        if (info == "ok2")
        {

            if (Price == 0) Utils.Error("投注金额不能为0！", "");
            //支付安全提示
            string[] p_pageArr = { "Price", "Price1", "Price2", "Price3", "Price4", "Num5", "Num4", "Num3", "Num2", "Num1", "accNum", "ptype", "act", "info" };
            BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr);

            long gold = new BCW.BLL.User().GetGold(meid);
            long prices = Convert.ToInt64(Price * iZhu);

            //是否刷屏
            long small = Convert.ToInt64(ub.GetSub("SSCSmallPay", xmlPath));
            long big = Convert.ToInt64(ub.GetSub("SSCBigPay", xmlPath));
            string appName = "LIGHT_SSC";
            int Expir = Utils.ParseInt(ub.GetSub("SSCExpir", xmlPath));
            BCW.User.Users.IsFresh(appName, Expir, Price, small, big);

            if (model.EndTime < DateTime.Now.AddSeconds(Sec))
            {
                Utils.Error("第" + model.SSCId + "期已截止下注,等待开奖...", Utils.getUrl("ssc.aspx"));
            }

            if (gold < prices)
            {
                Utils.Error("您的" + ub.Get("SiteBz") + "不足", "");
            }

            long xPrices = Utils.ParseInt64(ub.GetSub("SSCPrice", xmlPath));
            if (xPrices > 0)
            {
                long oPrices = new BCW.ssc.BLL.SSCpay().GetSumPrices(meid, model.SSCId);
                if (oPrices + prices > xPrices)
                {
                    if (oPrices >= xPrices)
                        Utils.Error("您本期下注已达上限，请等待下期...", "");
                    else
                        Utils.Error("您本期最多还可以下注" + (xPrices - oPrices) + "" + ub.Get("SiteBz") + "", "");
                }
            }

            #region 每期每玩法每ID投注上限
            if (ptype == 17 || ptype == 28 || ptype == 2 || ptype == 3 || ptype == 5 || ptype == 6 || ptype == 8 || ptype == 9 || ptype == 11 || ptype == 12 || ptype == 14 || ptype == 15 || ptype == 25 || ptype == 26 || ptype == 36 || ptype == 37 || ptype == 43 || ptype == 44 || ptype == 50 || ptype == 51)// || ptype == 23
            {
                long ptyPrices = 0;
                try
                {
                    ptyPrices = Convert.ToInt64(OutOddscTid(ptype));
                }
                catch { ptyPrices = 0; }
                if (ptyPrices > 0)
                {
                    long oPrices = new BCW.ssc.BLL.SSCpay().GetSumPrices(meid, model.SSCId, ptype);
                    if (oPrices + prices > ptyPrices)
                    {
                        if (oPrices >= ptyPrices)
                            Utils.Error("您本期" + OutType(ptype) + "下注已达上限，请等待下期...", "");
                        else
                            Utils.Error("您本期" + OutType(ptype) + "最多还可以下注" + (ptyPrices - oPrices) + "" + ub.Get("SiteBz") + "", "");
                    }
                }
            }
            else
            {
                long ptyPrices = 0;
                try
                {
                    ptyPrices = Convert.ToInt64(OutOddscid(ptype));
                }
                catch { ptyPrices = 0; }
                if (ptyPrices > 0)
                {
                    long oPrices = new BCW.ssc.BLL.SSCpay().GetSumPrices(meid, model.SSCId, ptype);
                    if (oPrices + prices > ptyPrices)
                    {
                        if (oPrices >= ptyPrices)
                            Utils.Error("您本期" + OutType(ptype) + "下注已达上限，请等待下期...", "");
                        else
                            Utils.Error("您本期" + OutType(ptype) + "最多还可以下注" + (ptyPrices - oPrices) + "" + ub.Get("SiteBz") + "", "");
                    }
                }
            }
            #endregion

            #region 投注上限
            long xPricesc = 0;
            //投注上限
            if (ptype == 1 || ptype == 4 || ptype == 7 || ptype == 10 || ptype == 13 || (ptype >= 16 && ptype <= 22) || ptype == 24 || (ptype >= 29 && ptype <= 35) || (ptype >= 38 && ptype <= 42) || (ptype >= 45 && ptype <= 49) || (ptype >= 52 && ptype <= 56))
            {
                xPricesc = Convert.ToInt64(OutOddsc(ptype));
                if (xPricesc > 0)
                {
                    long oPricesc = new BCW.ssc.BLL.SSCpay().GetSumPricebyTypes(ptype, model.SSCId);
                    if (oPricesc + prices > xPricesc)
                    {
                        if (oPricesc > xPricesc)
                            Utils.Error("本期" + OutType(ptype) + "下注已达上限，请等待下期或者选择其他投注...", "");
                        else
                            Utils.Error("本期" + OutType(ptype) + "最多还可以下注" + (xPricesc - oPricesc) + ub.Get("SiteBz") + "", "");
                    }
                }
            }
            else if (ptype == 23)//有牛无牛上限
            {
                string yn = OutOddsc(ptype);
                string[] yny = yn.Split('|');
                if (accNum == "有牛")
                {
                    xPricesc = Convert.ToInt64(yny[0]);
                    long oPricesc = new BCW.ssc.BLL.SSCpay().GetSumPriceby23(23, model.SSCId, 1);
                    if (oPricesc + prices > xPricesc)
                    {
                        if (oPricesc > xPricesc)
                            Utils.Error("本期" + OutType(ptype) + "有牛下注已达上限，请等待下期或者选择其他投注...", "");
                        else
                            Utils.Error("本期" + OutType(ptype) + "有牛最多还可以下注" + (xPricesc - oPricesc) + ub.Get("SiteBz") + "", "");
                    }
                }
                if (accNum == "无牛")
                {
                    xPricesc = Convert.ToInt64(yny[1]);
                    long oPricesc = new BCW.ssc.BLL.SSCpay().GetSumPriceby23(23, model.SSCId, 2);
                    if (oPricesc + prices > xPricesc)
                    {
                        if (oPricesc > xPricesc)
                            Utils.Error("本期" + OutType(ptype) + "无牛下注已达上限，请等待下期或者选择其他投注...", "");
                        else
                            Utils.Error("本期" + OutType(ptype) + "无牛最多还可以下注" + (xPricesc - oPricesc) + ub.Get("SiteBz") + "", "");
                    }
                }
            }
            else if (ptype == 27)//五门上限
            {
                string yn = OutOddsc(ptype);
                string[] yny = yn.Split('|');
                if (accNum == "一门")
                {
                    xPricesc = Convert.ToInt64(yny[0]);
                    long oPricesc = new BCW.ssc.BLL.SSCpay().GetSumPriceby27(27, model.SSCId, 1);
                    if (oPricesc + prices > xPricesc)
                    {
                        if (oPricesc > xPricesc)
                            Utils.Error("本期" + OutType(ptype) + "[一门]下注已达上限，请等待下期或者选择其他投注...", "");
                        else
                            Utils.Error("本期" + OutType(ptype) + "[一门]最多还可以下注" + (xPricesc - oPricesc) + ub.Get("SiteBz") + "", "");
                    }
                }
                if (accNum == "二门")
                {
                    xPricesc = Convert.ToInt64(yny[1]);
                    long oPricesc = new BCW.ssc.BLL.SSCpay().GetSumPriceby27(27, model.SSCId, 2);
                    if (oPricesc + prices > xPricesc)
                    {
                        if (oPricesc > xPricesc)
                            Utils.Error("本期" + OutType(ptype) + "[二门]下注已达上限，请等待下期或者选择其他投注...", "");
                        else
                            Utils.Error("本期" + OutType(ptype) + "[二门]最多还可以下注" + (xPricesc - oPricesc) + ub.Get("SiteBz") + "", "");
                    }
                }
                if (accNum == "三门")
                {
                    xPricesc = Convert.ToInt64(yny[2]);
                    long oPricesc = new BCW.ssc.BLL.SSCpay().GetSumPriceby27(27, model.SSCId, 3);
                    if (oPricesc + prices > xPricesc)
                    {
                        if (oPricesc > xPricesc)
                            Utils.Error("本期" + OutType(ptype) + "[三门]下注已达上限，请等待下期或者选择其他投注...", "");
                        else
                            Utils.Error("本期" + OutType(ptype) + "[三门]最多还可以下注" + (xPricesc - oPricesc) + ub.Get("SiteBz") + "", "");
                    }
                }
                if (accNum == "四门")
                {
                    xPricesc = Convert.ToInt64(yny[3]);
                    long oPricesc = new BCW.ssc.BLL.SSCpay().GetSumPriceby27(27, model.SSCId, 4);
                    if (oPricesc + prices > xPricesc)
                    {
                        if (oPricesc > xPricesc)
                            Utils.Error("本期" + OutType(ptype) + "[四门]下注已达上限，请等待下期或者选择其他投注...", "");
                        else
                            Utils.Error("本期" + OutType(ptype) + "[四门]最多还可以下注" + (xPricesc - oPricesc) + ub.Get("SiteBz") + "", "");
                    }
                }
                if (accNum == "五门")
                {
                    xPricesc = Convert.ToInt64(yny[4]);
                    long oPricesc = new BCW.ssc.BLL.SSCpay().GetSumPriceby27(27, model.SSCId, 5);
                    if (oPricesc + prices > xPricesc)
                    {
                        if (oPricesc > xPricesc)
                            Utils.Error("本期" + OutType(ptype) + "[五门]下注已达上限，请等待下期或者选择其他投注...", "");
                        else
                            Utils.Error("本期" + OutType(ptype) + "[五门]最多还可以下注" + (xPricesc - oPricesc) + ub.Get("SiteBz") + "", "");
                    }
                }
            }
            else//浮动额度
            {
                xPricesc = Convert.ToInt64(OutOddsc(ptype));
                if (ptype == 2 || ptype == 5 || ptype == 8 || ptype == 11 || ptype == 14 || ptype == 25 || ptype == 36 || ptype == 43 || ptype == 50)//大小浮动
                {
                    long Cent = new BCW.ssc.BLL.SSCpay().GetSumPricebyDX(ptype, model.SSCId, 1);
                    long Cent2 = new BCW.ssc.BLL.SSCpay().GetSumPricebyDX(ptype, model.SSCId, 2);

                    if (accNum == "大")
                    {
                        if (Math.Abs(Cent + prices - Cent2) > xPricesc)
                        {
                            long hk = xPricesc + Cent2 - Cent;
                            Utils.Error("大数超出系统投注币额，押大还可投注" + hk + ub.Get("SiteBz") + "，或者请选择其他投注", "");
                        }
                    }
                    if (accNum == "小")
                    {
                        if (Math.Abs(Cent2 + prices - Cent) > xPricesc)
                        {
                            long hk = xPricesc + Cent - Cent2;
                            Utils.Error("小数超出系统投注币额，押小还可投注" + hk + ub.Get("SiteBz") + "，或者请选择其他投注", "");
                        }
                    }

                }
                else if (ptype == 3 || ptype == 6 || ptype == 9 || ptype == 12 || ptype == 15 || ptype == 26 || ptype == 37 || ptype == 44 || ptype == 51)//单双浮动
                {
                    long Cent = new BCW.ssc.BLL.SSCpay().GetSumPricebyDS(ptype, model.SSCId, 1);
                    long Cent2 = new BCW.ssc.BLL.SSCpay().GetSumPricebyDS(ptype, model.SSCId, 2);

                    if (accNum == "单")
                    {
                        if (Math.Abs(Cent + prices - Cent2) > xPricesc)
                        {
                            long hk = xPricesc + Cent2 - Cent;
                            Utils.Error("单数超出系统投注币额，押单还可投注" + hk + ub.Get("SiteBz") + "，或者请选择其他投注", "");
                        }
                    }
                    if (accNum == "双")
                    {
                        if (Math.Abs(Cent2 + prices - Cent) > xPricesc)
                        {
                            long hk = xPricesc + Cent - Cent2;
                            Utils.Error("双数超出系统投注币额，押双还可投注" + hk + ub.Get("SiteBz") + "，或者请选择其他投注", "");
                        }
                    }
                }
                else if (ptype == 28)
                {
                    if (accNum == "大单" || accNum == "大双")
                    {
                        long Cent = new BCW.ssc.BLL.SSCpay().GetSumPricebyHD(ptype, model.SSCId, 1);
                        long Cent2 = new BCW.ssc.BLL.SSCpay().GetSumPricebyHD(ptype, model.SSCId, 2);
                        if (accNum == "大单")
                        {
                            if (Math.Abs(Cent + prices - Cent2) > xPricesc)
                            {
                                long hk = xPricesc + Cent2 - Cent;
                                Utils.Error("大单数超出系统投注币额，押大单还可投注" + hk + ub.Get("SiteBz") + "，或者请选择其他投注", "");
                            }
                        }
                        if (accNum == "大双")
                        {
                            if (Math.Abs(Cent2 + prices - Cent) > xPricesc)
                            {
                                long hk = xPricesc + Cent - Cent2;
                                Utils.Error("大双数超出系统投注币额，押大双还可投注" + hk + ub.Get("SiteBz") + "，或者请选择其他投注", "");
                            }
                        }
                    }
                    if (accNum == "小单" || accNum == "小双")
                    {
                        long Cent = new BCW.ssc.BLL.SSCpay().GetSumPricebyHDx(ptype, model.SSCId, 1);
                        long Cent2 = new BCW.ssc.BLL.SSCpay().GetSumPricebyHDx(ptype, model.SSCId, 2);
                        if (accNum == "小单")
                        {
                            if (Math.Abs(Cent + prices - Cent2) > xPricesc)
                            {
                                long hk = xPricesc + Cent2 - Cent;
                                Utils.Error("小单数超出系统投注币额，押小单还可投注" + hk + ub.Get("SiteBz") + "，或者请选择其他投注", "");
                            }
                        }
                        if (accNum == "小双")
                        {
                            if (Math.Abs(Cent2 + prices - Cent) > xPricesc)
                            {
                                long hk = xPricesc + Cent - Cent2;
                                Utils.Error("小双数超出系统投注币额，押小双还可投注" + hk + ub.Get("SiteBz") + "，或者请选择其他投注", "");
                            }
                        }
                    }
                }
            }
            #endregion

            string mename = new BCW.BLL.User().GetUsName(meid);

            #region 赔率判断
            decimal Odds = 0;
            if (ptype == 17 || ptype == 23 || ptype == 27 || ptype == 28)
            {
                if (ptype == 17)//龙虎和
                {
                    if (accNum == "龙")
                        Odds = Convert.ToDecimal(OutOdds(17, 1));
                    if (accNum == "虎")
                        Odds = Convert.ToDecimal(OutOdds(17, 2));
                    if (accNum == "和")
                        Odds = Convert.ToDecimal(OutOdds(17, 3));
                }
                if (ptype == 23)//有无牛牛
                {
                    if (accNum == "有牛")
                        Odds = Convert.ToDecimal(OutOdds(23, 1));
                    if (accNum == "无牛")
                        Odds = Convert.ToDecimal(OutOdds(23, 2));
                }
                if (ptype == 27)//总和五门
                {
                    if (accNum == "一门")
                        Odds = Convert.ToDecimal(OutOdds(27, 1));
                    if (accNum == "二门")
                        Odds = Convert.ToDecimal(OutOdds(27, 2));
                    if (accNum == "三门")
                        Odds = Convert.ToDecimal(OutOdds(27, 3));
                    if (accNum == "四门")
                        Odds = Convert.ToDecimal(OutOdds(27, 4));
                    if (accNum == "五门")
                        Odds = Convert.ToDecimal(OutOdds(27, 5));
                }
                if (ptype == 28)//总和大小单双
                {
                    if (accNum == "大单")
                        Odds = Convert.ToDecimal(OutOdds(28, 1));
                    if (accNum == "小双")
                        Odds = Convert.ToDecimal(OutOdds(28, 1));
                    if (accNum == "大双")
                        Odds = Convert.ToDecimal(OutOdds(28, 2));
                    if (accNum == "小单")
                        Odds = Convert.ToDecimal(OutOdds(28, 2));
                }
            }
            else
            {
                if (ptype == 2 || ptype == 3 || ptype == 5 || ptype == 6 || ptype == 8 || ptype == 9 || ptype == 11 || ptype == 12 || ptype == 14 || ptype == 15 || ptype == 25 || ptype == 26 || ptype == 36 || ptype == 37 || ptype == 43 || ptype == 44 || ptype == 50 || ptype == 51)
                {
                    if (accNum == "大" || accNum == "单")
                    {
                        Odds = Convert.ToDecimal(OutOdds(ptype, 1));
                    }
                    if (accNum == "小" || accNum == "双")
                    {
                        Odds = Convert.ToDecimal(OutOdds(ptype, 2));
                    }
                }
                else
                {
                    Odds = Convert.ToDecimal(OutOdds(ptype, 1));
                }
            }
            #endregion

            BCW.ssc.Model.SSCpay modelpay = new BCW.ssc.Model.SSCpay();
            modelpay.SSCId = model.SSCId;
            modelpay.Types = ptype;
            modelpay.UsID = meid;
            modelpay.UsName = mename;
            modelpay.iCount = iZhu;
            modelpay.Price = Price;
            modelpay.State = 0;
            modelpay.Prices = prices;
            modelpay.WinCent = 0;
            modelpay.Result = "";
            modelpay.Notes = accNum;
            modelpay.Odds = Odds;
            modelpay.WinNotes = "";
            modelpay.AddTime = DateTime.Now;

            int id = 0;
            if (accNum != "")
            {
                id = new BCW.ssc.BLL.SSCpay().Add(modelpay);


                new BCW.BLL.User().UpdateiGold(meid, mename, -prices, 11, "" + GameName + "第[url=./game/ssc.aspx?act=view&amp;id=" + model.ID + "]" + model.SSCId + "[/url]期[" + OutType(ptype) + "]位号:" + accNum + "|赔率" + modelpay.Odds + "|标识ID" + id + ""); //酷币
                if (new BCW.BLL.User().GetIsSpier(meid) != 1)
                    new BCW.BLL.User().UpdateiGold(108, new BCW.BLL.User().GetUsName(108), prices, "ID:[url=forumlog.aspx?act=xview&amp;uid=" + meid + "]" + meid + "[/url]" + GameName + "第[url=./game/ssc.aspx?act=view&amp;id=" + model.ID + "]" + model.SSCId + "[/url]期买[" + OutType(ptype) + ":" + accNum + "]共" + prices + ub.Get("SiteBz") + "|赔率" + modelpay.Odds + "-标识ID" + id + "");

                //动态
                string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/ssc.aspx]" + GameName + "[/url]第" + model.SSCId + "期下注**" + ub.Get("SiteBz") + "";//prices
                new BCW.BLL.Action().Add(1019, id, meid, "", wText);
                //活跃抽奖入口_20160621姚志光
                try
                {
                    //表中存在记录
                    if (new BCW.BLL.tb_WinnersGame().ExistsGameName("时时彩票"))
                    {
                        //投注是否大于设定的限额，是则有抽奖机会
                        if (prices > new BCW.BLL.tb_WinnersGame().GetPrice("时时彩票"))
                        {
                            string TextForUbb = (ub.GetSub("TextForUbb", "/Controls/winners.xml"));//设置内线提示的文字
                            string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", "/Controls/winners.xml"));//1发内线2不发内线 
                            int hit = new BCW.winners.winners().CheckActionForAll(1, 1, meid, mename, "时时彩", 3);
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
                Utils.Success("下注", "下注《" + TypeTitle + "》位号:" + accNum + "成功，花费了" + prices + "" + ub.Get("SiteBz") + "(共" + iZhu + "注)<br /><a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=" + ptype + "") + "\">&gt;继续下注</a>", Utils.getUrl("ssc.aspx"), "3");
            }
            else
            {
                Utils.Success("下注", "下注《" + TypeTitle + "》失败!<br /><a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=" + ptype + "") + "\">&gt;继续下注</a>", Utils.getUrl("ssc.aspx?act=info&amp;ptype=" + ptype + ""), "2");
            }
        }
        else
        {
            #region 特殊判断快捷下注
            if (ptype == 2 || ptype == 5 || ptype == 8 || ptype == 11 || ptype == 14 || ptype == 25 || ptype == 36 || ptype == 43 || ptype == 50)//万千百十个、总和、前中后三 大小，单双
            {
                if (ddn != "")//押大/单Price2 == 0
                {
                    if (ddn != "押大")
                    {
                        if (ddn.Contains("万"))
                        {
                            string str = string.Empty;
                            str = ddn.Replace("万", "");
                            Price = (Convert.ToInt64(str) * 10000);
                        }
                        else
                        {
                            Price = Convert.ToInt64(ddn);
                        }
                    }
                    else
                    {
                        Price = Price1;
                    }
                }
                if (xsn != "")//押小/双Price1 == 0
                {
                    if (xsn != "押小")
                    {
                        if (xsn.Contains("万"))
                        {
                            string str = string.Empty;
                            str = xsn.Replace("万", "");
                            Price = (Convert.ToInt64(str) * 10000);
                        }
                        else
                        {
                            Price = Convert.ToInt64(xsn);
                        }
                    }
                    else
                    {
                        Price = Price2;
                    }
                }
            }
            if (ptype == 3 || ptype == 6 || ptype == 9 || ptype == 12 || ptype == 15 || ptype == 26 || ptype == 37 || ptype == 44 || ptype == 51)//万千百十个、总和、前中后三 大小，单双
            {
                if (ddnds != "")//押大/单Price2 == 0
                {
                    if (ddnds != "押单")
                    {
                        if (ddnds.Contains("万"))
                        {
                            string str = string.Empty;
                            str = ddnds.Replace("万", "");
                            Price = (Convert.ToInt64(str) * 10000);
                        }
                        else
                        {
                            Price = Convert.ToInt64(ddnds);
                        }
                    }
                    else
                    {
                        Price = Price1;
                    }
                }
                if (xsnds != "")//押小/双Price1 == 0
                {
                    if (xsnds != "押双")
                    {
                        if (xsnds.Contains("万"))
                        {
                            string str = string.Empty;
                            str = xsnds.Replace("万", "");
                            Price = (Convert.ToInt64(str) * 10000);
                        }
                        else
                        {
                            Price = Convert.ToInt64(xsnds);
                        }
                    }
                    else
                    {
                        Price = Price2;
                    }
                }
            }
            if (ptype == 23)//有无牛牛
            {
                if (ddnyn != "")//有牛Price2 == 0
                {
                    if (ddnyn != "有牛")
                    {
                        if (ddnyn.Contains("万"))
                        {
                            string str = string.Empty;
                            str = ddnyn.Replace("万", "");
                            Price = (Convert.ToInt64(str) * 10000);
                        }
                        else
                        {
                            Price = Convert.ToInt64(ddnyn);
                        }
                    }
                    else
                    {
                        Price = Price1;
                    }
                }
                if (xsnyn != "")//无牛Price1 == 0
                {
                    if (xsnyn != "无牛")
                    {
                        if (xsnyn.Contains("万"))
                        {
                            string str = string.Empty;
                            str = xsnyn.Replace("万", "");
                            Price = (Convert.ToInt64(str) * 10000);
                        }
                        else
                        {
                            Price = Convert.ToInt64(xsnyn);
                        }
                    }
                    else
                    {
                        Price = Price2;
                    }
                }
            }
            if (ptype == 17)//龙虎和
            {
                if (yl != "")//龙
                {
                    if (yl != "押龙")
                    {
                        if (yl.Contains("万"))
                        {
                            string str = string.Empty;
                            str = yl.Replace("万", "");
                            Price = (Convert.ToInt64(str) * 10000);
                        }
                        else
                        {
                            Price = Convert.ToInt64(yl);
                        }
                    }
                    else
                    {
                        Price = Price1;
                    }
                }
                if (yh != "")//虎     
                {
                    if (yh != "押虎")
                    {
                        if (yh.Contains("万"))
                        {
                            string str = string.Empty;
                            str = yh.Replace("万", "");
                            Price = (Convert.ToInt64(str) * 10000);
                        }
                        else
                        {
                            Price = Convert.ToInt64(yh);
                        }
                    }
                    else
                    {
                        Price = Price2;
                    }
                }
                if (yhe != "")//和
                {
                    if (yhe != "押和")
                    {
                        if (yhe.Contains("万"))
                        {
                            string str = string.Empty;
                            str = yhe.Replace("万", "");
                            Price = (Convert.ToInt64(str) * 10000);
                        }
                        else
                        {
                            Price = Convert.ToInt64(yhe);
                        }
                    }
                    else
                    {
                        Price = Price3;
                    }
                }
            }
            if (ptype == 28)//总和大小单双
            {
                if (dd != "")//大单Price2 == 0 && Price3 == 0 && Price4 == 0
                {
                    if (dd != "大单")
                    {
                        if (dd.Contains("万"))
                        {
                            string str = string.Empty;
                            str = dd.Replace("万", "");
                            Price = (Convert.ToInt64(str) * 10000);
                        }
                        else
                        {
                            Price = Convert.ToInt64(dd);
                        }
                    }
                    else
                    {
                        Price = Price1;
                    }
                }
                if (ds != "")//大双 Price1 == 0 && Price3 == 0 && Price4 == 0   
                {
                    if (ds != "大双")
                    {
                        if (ds.Contains("万"))
                        {
                            string str = string.Empty;
                            str = ds.Replace("万", "");
                            Price = (Convert.ToInt64(str) * 10000);
                        }
                        else
                        {
                            Price = Convert.ToInt64(ds);
                        }
                    }
                    else
                    {
                        Price = Price2;
                    }
                }
                if (xd != "")//小单Price1 == 0 && Price2 == 0 && Price4 == 0
                {
                    if (xd != "小单")
                    {
                        if (xd.Contains("万"))
                        {
                            string str = string.Empty;
                            str = xd.Replace("万", "");
                            Price = (Convert.ToInt64(str) * 10000);
                        }
                        else
                        {
                            Price = Convert.ToInt64(xd);
                        }
                    }
                    else
                    {
                        Price = Price3;
                    }
                }
                if (xs != "")//小双Price1 == 0 && Price2 == 0 && Price3 == 0
                {
                    if (xs != "小双")
                    {
                        if (xs.Contains("万"))
                        {
                            string str = string.Empty;
                            str = xs.Replace("万", "");
                            Price = (Convert.ToInt64(str) * 10000);
                        }
                        else
                        {
                            Price = Convert.ToInt64(xs);
                        }
                    }
                    else
                    {
                        Price = Price4;
                    }
                }
            }
            #endregion

            if (accNum != "")//判断下注位号不能为空
            {

                #region 赔率判断
                decimal Odds = 0;
                if (ptype == 17 || ptype == 23 || ptype == 27 || ptype == 28)
                {
                    if (ptype == 17)//龙虎和
                    {
                        if (accNum == "龙")
                            Odds = Convert.ToDecimal(OutOdds(17, 1));
                        if (accNum == "虎")
                            Odds = Convert.ToDecimal(OutOdds(17, 2));
                        if (accNum == "和")
                            Odds = Convert.ToDecimal(OutOdds(17, 3));
                    }
                    if (ptype == 23)//有无牛牛
                    {
                        if (accNum == "有牛")
                            Odds = Convert.ToDecimal(OutOdds(23, 1));
                        if (accNum == "无牛")
                            Odds = Convert.ToDecimal(OutOdds(23, 2));
                    }
                    if (ptype == 27)//总和五门
                    {
                        if (accNum == "一门")
                            Odds = Convert.ToDecimal(OutOdds(27, 1));
                        if (accNum == "二门")
                            Odds = Convert.ToDecimal(OutOdds(27, 2));
                        if (accNum == "三门")
                            Odds = Convert.ToDecimal(OutOdds(27, 3));
                        if (accNum == "四门")
                            Odds = Convert.ToDecimal(OutOdds(27, 4));
                        if (accNum == "五门")
                            Odds = Convert.ToDecimal(OutOdds(27, 5));
                    }
                    if (ptype == 28)//总和大小单双
                    {
                        if (accNum == "大单")
                            Odds = Convert.ToDecimal(OutOdds(28, 1));
                        if (accNum == "小双")
                            Odds = Convert.ToDecimal(OutOdds(28, 1));
                        if (accNum == "大双")
                            Odds = Convert.ToDecimal(OutOdds(28, 2));
                        if (accNum == "小单")
                            Odds = Convert.ToDecimal(OutOdds(28, 2));
                    }
                }
                else
                {
                    if (ptype == 2 || ptype == 3 || ptype == 5 || ptype == 6 || ptype == 8 || ptype == 9 || ptype == 11 || ptype == 12 || ptype == 14 || ptype == 15 || ptype == 25 || ptype == 26 || ptype == 36 || ptype == 37 || ptype == 43 || ptype == 44 || ptype == 50 || ptype == 51)
                    {
                        if (accNum == "大" || accNum == "单")
                        {
                            Odds = Convert.ToDecimal(OutOdds(ptype, 1));
                        }
                        if (accNum == "小" || accNum == "双")
                        {
                            Odds = Convert.ToDecimal(OutOdds(ptype, 2));
                        }
                    }
                    else
                    {
                        Odds = Convert.ToDecimal(OutOdds(ptype, 1));
                    }
                }
                #endregion

                builder.Append(Out.Tab("<div>", ""));
                builder.Append("第" + model.SSCId + "期");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));

                string SSC = new BCW.JS.somejs().newDaojishi("a", model.EndTime.AddSeconds(-Sec));
                builder.Append("距离截止时间还有" + SSC + "<br />");

                builder.Append("类型：<b>" + TypeTitle + "</b>");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("位号：" + accNum + "<br />");
                builder.Append("注数：" + iZhu + "注<br />");
                builder.Append("赔率：" + Odds + "<br />");
                builder.Append("每注：" + Price + "" + ub.Get("SiteBz") + "<br />");
                builder.Append("需花费：" + (Price * iZhu) + "" + ub.Get("SiteBz") + "<br />");
                long gold = new BCW.BLL.User().GetGold(meid);
                builder.Append("您自带：" + Utils.ConvertGold(gold) + "" + ub.Get("SiteBz") + "<br />");
                builder.Append(Out.Tab("</div>", ""));

                if (ptype == 2 || ptype == 5 || ptype == 8 || ptype == 11 || ptype == 14 || ptype == 25 || ptype == 36 || ptype == 43 || ptype == 50 || ptype == 3 || ptype == 6 || ptype == 9 || ptype == 12 || ptype == 15 || ptype == 26 || ptype == 37 || ptype == 44 || ptype == 51 || ptype == 23)//万千百十个、总和、前中后三 大小、单双\有无牛牛
                {
                    string strName = "Price,Price1,Price2,Num5,Num4,Num3,Num2,Num1,ptype,accNum,act,info";
                    string strValu = "" + Price + "'" + Price1 + "'" + Price2 + "'" + Num5 + "'" + Num4 + "'" + Num3 + "'" + Num2 + "'" + Num1 + "'" + ptype + "'" + accNum + "'pay'ok2";
                    string strOthe = "确定投注,ssc.aspx,post,0,red";
                    builder.Append(Out.wapform(strName, strValu, strOthe));
                }
                else if (ptype == 17)
                {
                    string strName = "Price,Price1,Price2,Price3,Num5,Num4,Num3,Num2,Num1,ptype,accNum,act,info";
                    string strValu = "" + Price + "'" + Price1 + "'" + Price2 + "'" + Price3 + "'" + Num5 + "'" + Num4 + "'" + Num3 + "'" + Num2 + "'" + Num1 + "'" + ptype + "'" + accNum + "'pay'ok2";
                    string strOthe = "确定投注,ssc.aspx,post,0,red";
                    builder.Append(Out.wapform(strName, strValu, strOthe));
                }
                else if (ptype == 28)
                {
                    string strName = "Price,Price1,Price2,Price3,Price4,Num5,Num4,Num3,Num2,Num1,ptype,accNum,act,info";
                    string strValu = "" + Price + "'" + Price1 + "'" + Price2 + "'" + Price3 + "'" + Price4 + "'" + Num5 + "'" + Num4 + "'" + Num3 + "'" + Num2 + "'" + Num1 + "'" + ptype + "'" + accNum + "'pay'ok2";
                    string strOthe = "确定投注,ssc.aspx,post,0,red";
                    builder.Append(Out.wapform(strName, strValu, strOthe));
                }
                else
                {
                    string strName = "Price,Num5,Num4,Num3,Num2,Num1,ptype,act,info";
                    string strValu = "" + Price + "'" + Num5 + "'" + Num4 + "'" + Num3 + "'" + Num2 + "'" + Num1 + "'" + ptype + "'pay'ok2";
                    string strOthe = "确定投注,ssc.aspx,post,0,red";
                    builder.Append(Out.wapform(strName, strValu, strOthe));
                }
                builder.Append(Out.Tab("<div>", Out.Hr()));
                builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx?act=info&amp;ptype=" + ptype + "") + "\">返回重新选号</a>");
                builder.Append(Out.Tab("</div>", ""));

                builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
                builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>-");
                builder.Append("<a href=\"" + Utils.getUrl("ssc.aspx") + "\">" + GameName + "</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
    }
    #endregion

    #region 两个数是否相似 IsLike
    /// <summary>
    /// 两个数是否相似
    /// </summary>
    private bool IsLike(string Num1, string Num2)
    {
        bool like = true;
        string getNum1 = Utils.ConvertSeparated(Num1, 1, ",");
        string getNum2 = Utils.ConvertSeparated(Num2, 1, ",");

        string[] str1 = getNum1.Split(',');
        string[] str2 = getNum2.Split(',');

        for (int i = 0; i < str1.Length; i++)
        {
            int cNum = Utils.GetStringNum(Num1, str1[i]);
            int cNum2 = Utils.GetStringNum(Num2, str1[i]);

            if (cNum != cNum2)
            {
                like = false;
                break;
            }

        }

        return like;
    }
    #endregion

    #region 将两个数字转化成三个数字的两组集合 OutStrNum

    /// <summary>
    /// 将两个数字转化成三个数字的两组集合
    /// </summary>
    /// <param name="sNum"></param>
    /// <returns></returns>
    private string OutStrNum(string sNum)
    {
        string[] Temp = sNum.Split(',');
        string strNum1 = Temp[0] + "," + sNum;
        string strNum2 = sNum + "," + Temp[1];
        string strNum = strNum1 + "，" + strNum2;
        return strNum.Replace(",", "");
    }
    #endregion

    #region 下注类型 OutType
    /// <summary>
    /// 下注类型
    /// </summary>
    /// <param name="Types"></param>
    /// <returns></returns>
    private string OutType(int Types)
    {
        string ptypey = string.Empty;
        string payname1 = string.Empty;
        string odds1 = string.Empty;
        string oddsc1 = string.Empty;
        string rule1 = string.Empty;
        for (int i = 1; i < 57; i++)
        {
            ptypey = ub.GetSub("ptype" + i + "", xmlPath);
            string[] ptypef = ptypey.Split('#');
            payname1 += "#" + ptypef[0];
            odds1 += "#" + ptypef[1];
            oddsc1 += "#" + ptypef[2];
            rule1 += "#" + ptypef[3];
        }
        string[] payname2 = payname1.Split('#');
        string[] odds2 = odds1.Split('#');
        string[] oddsc2 = oddsc1.Split('#');
        string[] rule2 = rule1.Split('#');
        string pText = string.Empty;

        for (int i = 1; i < 57; i++)
        {
            if (Types == i)
                pText = payname2[i];
        }

        return pText;
    }
    #endregion

    #region 玩法提示 OutRule
    /// <summary>
    /// 玩法提示
    /// </summary>
    /// <param name="Types"></param>
    /// <returns></returns>
    private string OutRule(int Types)
    {
        string ptypey = string.Empty;
        string payname1 = string.Empty;
        string odds1 = string.Empty;
        string oddsc1 = string.Empty;
        string rule1 = string.Empty;
        for (int i = 1; i < 57; i++)
        {
            ptypey = ub.GetSub("ptype" + i + "", xmlPath);
            string[] ptypef = ptypey.Split('#');
            payname1 += "#" + ptypef[0];
            odds1 += "#" + ptypef[1];
            oddsc1 += "#" + ptypef[2];
            rule1 += "#" + ptypef[3];
        }
        string[] payname2 = payname1.Split('#');
        string[] odds2 = odds1.Split('#');
        string[] oddsc2 = oddsc1.Split('#');
        string[] rule2 = rule1.Split('#');
        string pText = string.Empty;

        for (int i = 1; i < 57; i++)
        {
            if (Types == i)
                pText = rule2[i];
        }

        return pText;
    }
    #endregion

    #region 赔率 OutOdds
    /// <summary>
    /// 赔率 如果赔率只有一个，n就是1位，赔率取第二位，n就是2，赔率n位，取N位
    /// </summary>
    /// <param name="Types"></param>
    /// <returns></returns>
    private string OutOdds(int Types, int n)
    {
        string ptypey = string.Empty;
        string payname1 = string.Empty;
        string odds1 = string.Empty;
        string oddsc1 = string.Empty;
        string rule1 = string.Empty;
        for (int i = 1; i < 57; i++)
        {
            ptypey = ub.GetSub("ptype" + i + "", xmlPath);
            string[] ptypef = ptypey.Split('#');
            payname1 += "#" + ptypef[0];
            odds1 += "#" + ptypef[1];
            oddsc1 += "#" + ptypef[2];
            rule1 += "#" + ptypef[3];
        }
        string[] payname2 = payname1.Split('#');
        string[] odds2 = odds1.Split('#');
        string[] oddsc2 = oddsc1.Split('#');
        string[] rule2 = rule1.Split('#');
        string pText = string.Empty;

        for (int i = 1; i < 57; i++)
        {
            string[] odds = odds2[i].Split('|');

            if (odds.Length == 1)
            {
                if (Types == i)
                    pText = odds2[i];
            }
            else
            {
                for (int m = 0; m < odds.Length; m++)
                {
                    if (Types == i && m == (n - 1))
                    {
                        pText = odds[m];
                    }
                }
            }
        }

        return pText;
    }
    #endregion

    #region 投注上限 OutOddsc
    /// <summary>
    /// 投注上限
    /// </summary>
    /// <param name="Types"></param>
    /// <returns></returns>
    private string OutOddsc(int Types)
    {
        string ptypey = string.Empty;
        string payname1 = string.Empty;
        string odds1 = string.Empty;
        string oddsc1 = string.Empty;
        string rule1 = string.Empty;
        for (int i = 1; i < 57; i++)
        {
            ptypey = ub.GetSub("ptype" + i + "", xmlPath);
            string[] ptypef = ptypey.Split('#');
            payname1 += "#" + ptypef[0];
            odds1 += "#" + ptypef[1];
            oddsc1 += "#" + ptypef[2];
            rule1 += "#" + ptypef[3];
        }
        string[] payname2 = payname1.Split('#');
        string[] odds2 = odds1.Split('#');
        string[] oddsc2 = oddsc1.Split('#');
        string[] rule2 = rule1.Split('#');
        string pText = string.Empty;

        for (int i = 1; i < 57; i++)
        {
            if (Types == i)
                pText = oddsc2[i];
        }

        return pText;
    }
    #endregion

    #region 投注上限 OutOddscTid
    /// <summary>
    /// 投注上限
    /// </summary>
    /// <param name="Types"></param>
    /// <returns></returns>
    private string OutOddscTid(int Types)
    {
        string ptypey = string.Empty;
        string oddsc1 = string.Empty;
        for (int i = 1; i < 57; i++)
        {
            ptypey = ub.GetSub("ptype" + i + "", xmlPath);
            string[] ptypef = ptypey.Split('#');
            string p8 = string.Empty;
            try { p8 = ptypef[8]; }
            catch { p8 = "0"; }
            oddsc1 += "#" + p8;

        }
        string[] oddsc2 = oddsc1.Split('#');
        string pText = string.Empty;

        for (int i = 1; i < 57; i++)
        {
            if (Types == i)
                pText = oddsc2[i];
        }

        return pText;
    }
    #endregion

    #region 投注上限 OutOddscid
    /// <summary>
    /// 投注上限
    /// </summary>
    /// <param name="Types"></param>
    /// <returns></returns>
    private string OutOddscid(int Types)
    {
        string ptypey = string.Empty;
        string oddsc1 = string.Empty;
        for (int i = 1; i < 57; i++)
        {
            ptypey = ub.GetSub("ptype" + i + "", xmlPath);
            string[] ptypef = ptypey.Split('#');
            oddsc1 += "#" + ptypef[4];

        }
        string[] oddsc2 = oddsc1.Split('#');
        string pText = string.Empty;

        for (int i = 1; i < 57; i++)
        {
            if (Types == i)
                pText = oddsc2[i];
        }

        return pText;
    }
    #endregion

    #region 中奖注数
    private int GetZj_zs(string WinNotes)
    {
        int a = 0;
        if (WinNotes != " " || WinNotes != "" || WinNotes != null)
        {
            string[] b = WinNotes.Split(':');
            try
            {
                a = Convert.ToInt32(b[1]);
            }
            catch { }
        }
        return a;
    }
    #endregion

    #region 计算组合的数量
    static long jc(int N)//阶乘
    {
        long t = 1;

        for (int i = 1; i <= N; i++)
        {
            t *= i;
        }
        return t;
    }
    static long P(int N, int R)//组合的计算公式
    {
        long t = jc(N) / jc(N - R);

        return t;
    }
    static int C(int N, int R)//组合
    {
        long i = P(N, R) / jc(R);
        int t = Convert.ToInt32(i);
        return t;
    }
    #endregion

    #region 前三
    private int Qiansan(int i, string Result)
    {
        string[] iNum_kj = Result.Split(' ');
        int zj_zs = 0;

        if (i == 1)//大小
        {
            int sum = 0;
            sum = Convert.ToInt32(iNum_kj[0]) + Convert.ToInt32(iNum_kj[1]) + Convert.ToInt32(iNum_kj[2]);
            if (sum > 13 && sum < 28)//大
            {
                zj_zs = 1;
            }
            if (sum < 14)//小
            {
                zj_zs = 2;
            }
        }
        if (i == 2)//单双
        {
            int sum = 0;
            sum = Convert.ToInt32(iNum_kj[0]) + Convert.ToInt32(iNum_kj[1]) + Convert.ToInt32(iNum_kj[2]);
            if (sum % 2 != 0)//单
            {
                zj_zs = 1;
            }
            if (sum % 2 == 0)//双
            {
                zj_zs = 2;
            }
        }
        if (i == 3)//豹子
        {
            if (Convert.ToInt32(iNum_kj[0]) == Convert.ToInt32(iNum_kj[1]) && Convert.ToInt32(iNum_kj[1]) == Convert.ToInt32(iNum_kj[2]))
            {
                zj_zs = 1;
            }
        }
        if (i == 4)//顺子
        {
            int n1 = Convert.ToInt32(iNum_kj[0]); if (n1 == 0) n1 = 10;
            int n2 = Convert.ToInt32(iNum_kj[1]); if (n2 == 0) n2 = 10;
            int n3 = Convert.ToInt32(iNum_kj[2]); if (n3 == 0) n3 = 10;
            int a = n1 - n2;
            int b = n1 - n3;
            int c = n2 - n3;

            int d = 0;
            if (n1 != n2 && n2 != n3 && n1 != n3)
            {
                d = Math.Abs(a) + Math.Abs(b) + Math.Abs(c);
            }

            if (d == 4)
            {
                zj_zs = 1;
            }

            if (n1 == 10 && n2 == 1 && n3 == 2) zj_zs = 1;//012
            if (n1 == 10 && n2 == 2 && n3 == 1) zj_zs = 1;
            if (n1 == 1 && n2 == 10 && n3 == 2) zj_zs = 1;
            if (n1 == 1 && n2 == 2 && n3 == 10) zj_zs = 1;
            if (n1 == 2 && n2 == 1 && n3 == 10) zj_zs = 1;
            if (n1 == 2 && n2 == 10 && n3 == 1) zj_zs = 1;

            if (n1 == 10 && n2 == 1 && n3 == 9) zj_zs = 1;//901
            if (n1 == 10 && n2 == 9 && n3 == 1) zj_zs = 1;
            if (n1 == 1 && n2 == 10 && n3 == 9) zj_zs = 1;
            if (n1 == 1 && n2 == 9 && n3 == 10) zj_zs = 1;
            if (n1 == 9 && n2 == 1 && n3 == 10) zj_zs = 1;
            if (n1 == 9 && n2 == 10 && n3 == 1) zj_zs = 1;
        }
        if (i == 5)//对子
        {
            int n1 = Convert.ToInt32(iNum_kj[0]);
            int n2 = Convert.ToInt32(iNum_kj[1]);
            int n3 = Convert.ToInt32(iNum_kj[2]);

            if (n1 == n2 && n2 != n3)
            {
                zj_zs = 1;
            }
            if (n1 == n3 && n3 != n2)
            {
                zj_zs = 1;
            }
            if (n2 == n3 && n3 != n1)
            {
                zj_zs = 1;
            }
        }
        if (i == 6)//半顺
        {
            int n1 = Convert.ToInt32(iNum_kj[0]); if (n1 == 0) n1 = 10;
            int n2 = Convert.ToInt32(iNum_kj[1]); if (n2 == 0) n2 = 10;
            int n3 = Convert.ToInt32(iNum_kj[2]); if (n3 == 0) n3 = 10;

            int a = n1 - n2;
            int b = n1 - n3;
            int c = n2 - n3;

            if (Math.Abs(a) == 1 && Math.Abs(b) > 1 && Math.Abs(c) > 1)
            {
                zj_zs = 1;
            }
            if (Math.Abs(b) == 1 && Math.Abs(a) > 1 && Math.Abs(c) > 1)
            {
                zj_zs = 1;
            }
            if (Math.Abs(c) == 1 && Math.Abs(b) > 1 && Math.Abs(a) > 1)
            {
                zj_zs = 1;
            }
            string n123 = iNum_kj[0] + iNum_kj[1] + iNum_kj[2];
            {
                if (n1 != n2 && n2 != n3 && n1 != n3)
                {
                    if (n123.Contains("0") && n123.Contains("1") && !n123.Contains("2"))
                        zj_zs = 1;
                }
            }
            if (Qiansan(4, Result) == 1) zj_zs = 0;
        }
        if (i == 7)//杂六
        {
            if (Qiansan(3, Result) == 1) //豹子
                zj_zs = 1;
            if (Qiansan(4, Result) == 1) //顺子
                zj_zs = 1;
            if (Qiansan(5, Result) == 1) //对子
                zj_zs = 1;
            if (Qiansan(6, Result) == 1) //半顺
                zj_zs = 1;
        }
        return zj_zs;
    }
    #endregion

    #region 中三
    private int Zhongsan(int i, string Result)
    {
        string[] iNum_kj = Result.Split(' ');
        int zj_zs = 0;

        if (i == 1)//大小
        {
            int sum = 0;
            sum = Convert.ToInt32(iNum_kj[1]) + Convert.ToInt32(iNum_kj[2]) + Convert.ToInt32(iNum_kj[3]);
            if (sum > 13 && sum < 28)//大
            {
                zj_zs = 1;
            }
            if (sum < 14)//小
            {
                zj_zs = 2;
            }
        }
        if (i == 2)//单双
        {
            int sum = 0;
            sum = Convert.ToInt32(iNum_kj[1]) + Convert.ToInt32(iNum_kj[2]) + Convert.ToInt32(iNum_kj[3]);
            if (sum % 2 != 0)//单
            {
                zj_zs = 1;
            }
            if (sum % 2 == 0)//双
            {
                zj_zs = 2;
            }
        }
        if (i == 3)//豹子
        {
            if (Convert.ToInt32(iNum_kj[1]) == Convert.ToInt32(iNum_kj[2]) && Convert.ToInt32(iNum_kj[2]) == Convert.ToInt32(iNum_kj[3]))
            {
                zj_zs = 1;
            }
        }
        if (i == 4)//顺子
        {
            int n1 = Convert.ToInt32(iNum_kj[1]); if (n1 == 0) n1 = 10;
            int n2 = Convert.ToInt32(iNum_kj[2]); if (n2 == 0) n2 = 10;
            int n3 = Convert.ToInt32(iNum_kj[3]); if (n3 == 0) n3 = 10;
            int a = n1 - n2;
            int b = n1 - n3;
            int c = n2 - n3;

            int d = 0;
            if (n1 != n2 && n2 != n3 && n1 != n3)
            {
                d = Math.Abs(a) + Math.Abs(b) + Math.Abs(c);
            }

            if (d == 4)
            {
                zj_zs = 1;
            }
            if (n1 == 10 && n2 == 1 && n3 == 2) zj_zs = 1;//012
            if (n1 == 10 && n2 == 2 && n3 == 1) zj_zs = 1;
            if (n1 == 1 && n2 == 10 && n3 == 2) zj_zs = 1;
            if (n1 == 1 && n2 == 2 && n3 == 10) zj_zs = 1;
            if (n1 == 2 && n2 == 1 && n3 == 10) zj_zs = 1;
            if (n1 == 2 && n2 == 10 && n3 == 1) zj_zs = 1;

            if (n1 == 10 && n2 == 1 && n3 == 9) zj_zs = 1;//901
            if (n1 == 10 && n2 == 9 && n3 == 1) zj_zs = 1;
            if (n1 == 1 && n2 == 10 && n3 == 9) zj_zs = 1;
            if (n1 == 1 && n2 == 9 && n3 == 10) zj_zs = 1;
            if (n1 == 9 && n2 == 1 && n3 == 10) zj_zs = 1;
            if (n1 == 9 && n2 == 10 && n3 == 1) zj_zs = 1;
        }
        if (i == 5)//对子
        {
            int n1 = Convert.ToInt32(iNum_kj[1]);
            int n2 = Convert.ToInt32(iNum_kj[2]);
            int n3 = Convert.ToInt32(iNum_kj[3]);

            if (n1 == n2 && n2 != n3)
            {
                zj_zs = 1;
            }
            if (n1 == n3 && n3 != n2)
            {
                zj_zs = 1;
            }
            if (n2 == n3 && n3 != n1)
            {
                zj_zs = 1;
            }
        }
        if (i == 6)//半顺
        {
            int n1 = Convert.ToInt32(iNum_kj[1]); if (n1 == 0) n1 = 10;
            int n2 = Convert.ToInt32(iNum_kj[2]); if (n2 == 0) n2 = 10;
            int n3 = Convert.ToInt32(iNum_kj[3]); if (n3 == 0) n3 = 10;

            int a = n1 - n2;
            int b = n1 - n3;
            int c = n2 - n3;

            if (Math.Abs(a) == 1 && Math.Abs(b) > 1 && Math.Abs(c) > 1)
            {
                zj_zs = 1;
            }
            if (Math.Abs(b) == 1 && Math.Abs(a) > 1 && Math.Abs(c) > 1)
            {
                zj_zs = 1;
            }
            if (Math.Abs(c) == 1 && Math.Abs(b) > 1 && Math.Abs(a) > 1)
            {
                zj_zs = 1;
            }
            string n123 = iNum_kj[1] + iNum_kj[2] + iNum_kj[3];
            {
                if (n1 != n2 && n2 != n3 && n1 != n3)
                {
                    if (n123.Contains("0") && n123.Contains("1") && !n123.Contains("2"))
                        zj_zs = 1;
                }
            }
            if (Zhongsan(4, Result) == 1) zj_zs = 0;
        }
        if (i == 7)//杂六
        {
            if (Zhongsan(3, Result) == 1) //豹子
                zj_zs = 1;
            if (Zhongsan(4, Result) == 1) //顺子
                zj_zs = 1;
            if (Zhongsan(5, Result) == 1) //对子
                zj_zs = 1;
            if (Zhongsan(6, Result) == 1) //半顺
                zj_zs = 1;

        }
        return zj_zs;
    }
    #endregion

    #region 后三
    private int Housan(int i, string Result)
    {
        string[] iNum_kj = Result.Split(' ');
        int zj_zs = 0;

        if (i == 1)//大小
        {
            int sum = 0;
            sum = Convert.ToInt32(iNum_kj[2]) + Convert.ToInt32(iNum_kj[3]) + Convert.ToInt32(iNum_kj[4]);
            if (sum > 13 && sum < 28)//大
            {
                zj_zs = 1;
            }
            if (sum < 14)//小
            {
                zj_zs = 2;
            }
        }
        if (i == 2)//单双
        {
            int sum = 0;
            sum = Convert.ToInt32(iNum_kj[2]) + Convert.ToInt32(iNum_kj[3]) + Convert.ToInt32(iNum_kj[4]);
            if (sum % 2 != 0)//单
            {
                zj_zs = 1;
            }
            if (sum % 2 == 0)//双
            {
                zj_zs = 2;
            }
        }
        if (i == 3)//豹子
        {
            if (Convert.ToInt32(iNum_kj[2]) == Convert.ToInt32(iNum_kj[3]) && Convert.ToInt32(iNum_kj[3]) == Convert.ToInt32(iNum_kj[4]))
            {
                zj_zs = 1;
            }
        }
        if (i == 4)//顺子
        {
            int n1 = Convert.ToInt32(iNum_kj[2]); if (n1 == 0) n1 = 10;
            int n2 = Convert.ToInt32(iNum_kj[3]); if (n2 == 0) n2 = 10;
            int n3 = Convert.ToInt32(iNum_kj[4]); if (n3 == 0) n3 = 10;
            int d = 0;
            if (n1 != n2 && n2 != n3 && n1 != n3)
            {
                int a = n1 - n2;
                int b = n1 - n3;
                int c = n2 - n3;
                d = Math.Abs(a) + Math.Abs(b) + Math.Abs(c);
            }

            if (d == 4)
            {
                zj_zs = 1;
            }

            if (n1 == 10 && n2 == 1 && n3 == 2) zj_zs = 1;//012
            if (n1 == 10 && n2 == 2 && n3 == 1) zj_zs = 1;
            if (n1 == 1 && n2 == 10 && n3 == 2) zj_zs = 1;
            if (n1 == 1 && n2 == 2 && n3 == 10) zj_zs = 1;
            if (n1 == 2 && n2 == 1 && n3 == 10) zj_zs = 1;
            if (n1 == 2 && n2 == 10 && n3 == 1) zj_zs = 1;

            if (n1 == 10 && n2 == 1 && n3 == 9) zj_zs = 1;//901
            if (n1 == 10 && n2 == 9 && n3 == 1) zj_zs = 1;
            if (n1 == 1 && n2 == 10 && n3 == 9) zj_zs = 1;
            if (n1 == 1 && n2 == 9 && n3 == 10) zj_zs = 1;
            if (n1 == 9 && n2 == 1 && n3 == 10) zj_zs = 1;
            if (n1 == 9 && n2 == 10 && n3 == 1) zj_zs = 1;
        }
        if (i == 5)//对子
        {
            int n1 = Convert.ToInt32(iNum_kj[2]);
            int n2 = Convert.ToInt32(iNum_kj[3]);
            int n3 = Convert.ToInt32(iNum_kj[4]);

            if (n1 == n2 && n2 != n3)
            {
                zj_zs = 1;
            }
            if (n1 == n3 && n3 != n2)
            {
                zj_zs = 1;
            }
            if (n2 == n3 && n3 != n1)
            {
                zj_zs = 1;
            }
        }
        if (i == 6)//半顺
        {
            int n1 = Convert.ToInt32(iNum_kj[2]); if (n1 == 0) n1 = 10;
            int n2 = Convert.ToInt32(iNum_kj[3]); if (n2 == 0) n2 = 10;
            int n3 = Convert.ToInt32(iNum_kj[4]); if (n3 == 0) n3 = 10;

            int a = n1 - n2;
            int b = n1 - n3;
            int c = n2 - n3;

            if (Math.Abs(a) == 1 && Math.Abs(b) > 1 && Math.Abs(c) > 1)
            {
                zj_zs = 1;
            }
            if (Math.Abs(b) == 1 && Math.Abs(a) > 1 && Math.Abs(c) > 1)
            {
                zj_zs = 1;
            }
            if (Math.Abs(c) == 1 && Math.Abs(b) > 1 && Math.Abs(a) > 1)
            {
                zj_zs = 1;
            }
            string n123 = iNum_kj[2] + iNum_kj[3] + iNum_kj[4];
            {
                if (n1 != n2 && n2 != n3 && n1 != n3)
                {
                    if (n123.Contains("0") && n123.Contains("1") && !n123.Contains("2"))
                        zj_zs = 1;
                }
            }
            if (Housan(4, Result) == 1) zj_zs = 0;
        }
        if (i == 7)//杂六
        {
            if (Housan(3, Result) == 1) //豹子
                zj_zs = 1;
            if (Housan(4, Result) == 1) //顺子
                zj_zs = 1;
            if (Housan(5, Result) == 1) //对子
                zj_zs = 1;
            if (Housan(6, Result) == 1) //半顺
                zj_zs = 1;
        }
        return zj_zs;
    }
    #endregion

    #region 梭哈散牌
    private int SHSanpai(string Result)
    {
        string[] iNum_kj = Result.Split(' ');
        int zj_zs = 0;
        int[] a = { Convert.ToInt32(iNum_kj[0]), Convert.ToInt32(iNum_kj[1]), Convert.ToInt32(iNum_kj[2]), Convert.ToInt32(iNum_kj[3]), Convert.ToInt32(iNum_kj[4]) };
        int equal = 0;
        for (int i = 0; i < iNum_kj.Length - 1; i++)
        {
            for (int j = i + 1; j < iNum_kj.Length; j++)
            {
                if (a[i] == a[j])
                {
                    equal = 1;
                    break;
                }
            }
        }
        if (equal == 0)//数全不相等;
        {
            zj_zs = 1;
        }

        if (SHShunzi(Result) == 1) zj_zs = 0;//不能为顺子

        return zj_zs;
    }
    #endregion

    #region 梭哈单对
    private int SHDandui(string Result)
    {
        string[] iNum_kj = Result.Split(' ');
        int zj_zs = 0;

        int count0 = Result.Replace("*", "").Replace("<img", "*").Split('0').Length - 1;
        int count1 = Result.Replace("*", "").Replace("<img", "*").Split('1').Length - 1;
        int count2 = Result.Replace("*", "").Replace("<img", "*").Split('2').Length - 1;
        int count3 = Result.Replace("*", "").Replace("<img", "*").Split('3').Length - 1;
        int count4 = Result.Replace("*", "").Replace("<img", "*").Split('4').Length - 1;
        int count5 = Result.Replace("*", "").Replace("<img", "*").Split('5').Length - 1;
        int count6 = Result.Replace("*", "").Replace("<img", "*").Split('6').Length - 1;
        int count7 = Result.Replace("*", "").Replace("<img", "*").Split('7').Length - 1;
        int count8 = Result.Replace("*", "").Replace("<img", "*").Split('8').Length - 1;
        int count9 = Result.Replace("*", "").Replace("<img", "*").Split('9').Length - 1;

        if (count0 == 2) { if (count1 == 1 || count2 == 1 || count3 == 1 || count4 == 1 || count5 == 1 || count6 == 1 || count7 == 1 || count8 == 1 || count9 == 1) zj_zs = 1; }
        if (count1 == 2) { if (count0 == 1 || count2 == 1 || count3 == 1 || count4 == 1 || count5 == 1 || count6 == 1 || count7 == 1 || count8 == 1 || count9 == 1) zj_zs = 1; }
        if (count2 == 2) { if (count1 == 1 || count0 == 1 || count3 == 1 || count4 == 1 || count5 == 1 || count6 == 1 || count7 == 1 || count8 == 1 || count9 == 1) zj_zs = 1; }
        if (count3 == 2) { if (count1 == 1 || count2 == 1 || count0 == 1 || count4 == 1 || count5 == 1 || count6 == 1 || count7 == 1 || count8 == 1 || count9 == 1) zj_zs = 1; }
        if (count4 == 2) { if (count1 == 1 || count2 == 1 || count3 == 1 || count0 == 1 || count5 == 1 || count6 == 1 || count7 == 1 || count8 == 1 || count9 == 1) zj_zs = 1; }
        if (count5 == 2) { if (count1 == 1 || count2 == 1 || count3 == 1 || count4 == 1 || count0 == 1 || count6 == 1 || count7 == 1 || count8 == 1 || count9 == 1) zj_zs = 1; }
        if (count6 == 2) { if (count1 == 1 || count2 == 1 || count3 == 1 || count4 == 1 || count5 == 1 || count0 == 1 || count7 == 1 || count8 == 1 || count9 == 1) zj_zs = 1; }
        if (count7 == 2) { if (count1 == 1 || count2 == 1 || count3 == 1 || count4 == 1 || count5 == 1 || count6 == 1 || count0 == 1 || count8 == 1 || count9 == 1) zj_zs = 1; }
        if (count8 == 2) { if (count1 == 1 || count2 == 1 || count3 == 1 || count4 == 1 || count5 == 1 || count6 == 1 || count7 == 1 || count0 == 1 || count9 == 1) zj_zs = 1; }
        if (count9 == 2) { if (count1 == 1 || count2 == 1 || count3 == 1 || count4 == 1 || count5 == 1 || count6 == 1 || count7 == 1 || count8 == 1 || count0 == 1) zj_zs = 1; }

        if (HuLu(Result) == 1) zj_zs = 0;//不能为葫芦
        if (SHLiangdui(Result) == 1) zj_zs = 0;//不能为两对

        return zj_zs;
    }
    #endregion

    #region 梭哈两对
    private int SHLiangdui(string Result)
    {
        string[] iNum_kj = Result.Split(' ');
        int zj_zs = 0;

        int count0 = Result.Replace("*", "").Replace("<img", "*").Split('0').Length - 1;
        int count1 = Result.Replace("*", "").Replace("<img", "*").Split('1').Length - 1;
        int count2 = Result.Replace("*", "").Replace("<img", "*").Split('2').Length - 1;
        int count3 = Result.Replace("*", "").Replace("<img", "*").Split('3').Length - 1;
        int count4 = Result.Replace("*", "").Replace("<img", "*").Split('4').Length - 1;
        int count5 = Result.Replace("*", "").Replace("<img", "*").Split('5').Length - 1;
        int count6 = Result.Replace("*", "").Replace("<img", "*").Split('6').Length - 1;
        int count7 = Result.Replace("*", "").Replace("<img", "*").Split('7').Length - 1;
        int count8 = Result.Replace("*", "").Replace("<img", "*").Split('8').Length - 1;
        int count9 = Result.Replace("*", "").Replace("<img", "*").Split('9').Length - 1;

        if (count0 == 2) { if (count1 == 2 || count2 == 2 || count3 == 2 || count4 == 2 || count5 == 2 || count6 == 2 || count7 == 2 || count8 == 2 || count9 == 2) zj_zs = 1; }
        if (count1 == 2) { if (count0 == 2 || count2 == 2 || count3 == 2 || count4 == 2 || count5 == 2 || count6 == 2 || count7 == 2 || count8 == 2 || count9 == 2) zj_zs = 1; }
        if (count2 == 2) { if (count1 == 2 || count0 == 2 || count3 == 2 || count4 == 2 || count5 == 2 || count6 == 2 || count7 == 2 || count8 == 2 || count9 == 2) zj_zs = 1; }
        if (count3 == 2) { if (count1 == 2 || count2 == 2 || count0 == 2 || count4 == 2 || count5 == 2 || count6 == 2 || count7 == 2 || count8 == 2 || count9 == 2) zj_zs = 1; }
        if (count4 == 2) { if (count1 == 2 || count2 == 2 || count3 == 2 || count0 == 2 || count5 == 2 || count6 == 2 || count7 == 2 || count8 == 2 || count9 == 2) zj_zs = 1; }
        if (count5 == 2) { if (count1 == 2 || count2 == 2 || count3 == 2 || count4 == 2 || count0 == 2 || count6 == 2 || count7 == 2 || count8 == 2 || count9 == 2) zj_zs = 1; }
        if (count6 == 2) { if (count1 == 2 || count2 == 2 || count3 == 2 || count4 == 2 || count5 == 2 || count0 == 2 || count7 == 2 || count8 == 2 || count9 == 2) zj_zs = 1; }
        if (count7 == 2) { if (count1 == 2 || count2 == 2 || count3 == 2 || count4 == 2 || count5 == 2 || count6 == 2 || count0 == 2 || count8 == 2 || count9 == 2) zj_zs = 1; }
        if (count8 == 2) { if (count1 == 2 || count2 == 2 || count3 == 2 || count4 == 2 || count5 == 2 || count6 == 2 || count7 == 2 || count0 == 2 || count9 == 2) zj_zs = 1; }
        if (count9 == 2) { if (count1 == 2 || count2 == 2 || count3 == 2 || count4 == 2 || count5 == 2 || count6 == 2 || count7 == 2 || count8 == 2 || count0 == 2) zj_zs = 1; }

        if (HuLu(Result) == 1) zj_zs = 0;//不能为葫芦

        return zj_zs;
    }
    #endregion

    #region 梭哈三条
    private int SHSantiao(string Result)
    {
        string[] iNum_kj = Result.Split(' ');
        int zj_zs = 0;

        int count0 = Result.Replace("*", "").Replace("<img", "*").Split('0').Length - 1;
        int count1 = Result.Replace("*", "").Replace("<img", "*").Split('1').Length - 1;
        int count2 = Result.Replace("*", "").Replace("<img", "*").Split('2').Length - 1;
        int count3 = Result.Replace("*", "").Replace("<img", "*").Split('3').Length - 1;
        int count4 = Result.Replace("*", "").Replace("<img", "*").Split('4').Length - 1;
        int count5 = Result.Replace("*", "").Replace("<img", "*").Split('5').Length - 1;
        int count6 = Result.Replace("*", "").Replace("<img", "*").Split('6').Length - 1;
        int count7 = Result.Replace("*", "").Replace("<img", "*").Split('7').Length - 1;
        int count8 = Result.Replace("*", "").Replace("<img", "*").Split('8').Length - 1;
        int count9 = Result.Replace("*", "").Replace("<img", "*").Split('9').Length - 1;

        if (count0 == 3) { if (count1 == 1 || count2 == 1 || count3 == 1 || count4 == 1 || count5 == 1 || count6 == 1 || count7 == 1 || count8 == 1 || count9 == 1) zj_zs = 1; }
        if (count1 == 3) { if (count0 == 1 || count2 == 1 || count3 == 1 || count4 == 1 || count5 == 1 || count6 == 1 || count7 == 1 || count8 == 1 || count9 == 1) zj_zs = 1; }
        if (count2 == 3) { if (count1 == 1 || count0 == 1 || count3 == 1 || count4 == 1 || count5 == 1 || count6 == 1 || count7 == 1 || count8 == 1 || count9 == 1) zj_zs = 1; }
        if (count3 == 3) { if (count1 == 1 || count2 == 1 || count0 == 1 || count4 == 1 || count5 == 1 || count6 == 1 || count7 == 1 || count8 == 1 || count9 == 1) zj_zs = 1; }
        if (count4 == 3) { if (count1 == 1 || count2 == 1 || count3 == 1 || count0 == 1 || count5 == 1 || count6 == 1 || count7 == 1 || count8 == 1 || count9 == 1) zj_zs = 1; }
        if (count5 == 3) { if (count1 == 1 || count2 == 1 || count3 == 1 || count4 == 1 || count0 == 1 || count6 == 1 || count7 == 1 || count8 == 1 || count9 == 1) zj_zs = 1; }
        if (count6 == 3) { if (count1 == 1 || count2 == 1 || count3 == 1 || count4 == 1 || count5 == 1 || count0 == 1 || count7 == 1 || count8 == 1 || count9 == 1) zj_zs = 1; }
        if (count7 == 3) { if (count1 == 1 || count2 == 1 || count3 == 1 || count4 == 1 || count5 == 1 || count6 == 1 || count0 == 1 || count8 == 1 || count9 == 1) zj_zs = 1; }
        if (count8 == 3) { if (count1 == 1 || count2 == 1 || count3 == 1 || count4 == 1 || count5 == 1 || count6 == 1 || count7 == 1 || count0 == 1 || count9 == 1) zj_zs = 1; }
        if (count9 == 3) { if (count1 == 1 || count2 == 1 || count3 == 1 || count4 == 1 || count5 == 1 || count6 == 1 || count7 == 1 || count8 == 1 || count0 == 1) zj_zs = 1; }

        return zj_zs;
    }
    #endregion

    #region 梭哈顺子
    private int SHShunzi(string Result)
    {
        int a = 0;
        if (Result == "0 1 2 3 4" || Result == "1 2 3 4 5" || Result == "2 3 4 5 6" || Result == "3 4 5 6 7" || Result == "4 5 6 7 8" || Result == "5 6 7 8 9" || Result == "0 6 7 8 9" || Result == "0 1 7 8 9" || Result == "0 1 2 8 9" || Result == "0 1 2 3 9")
        {
            a = 1;
        }

        return a;
    }
    #endregion

    #region 炸弹算法
    private int Zhadan(string Result)
    {
        string[] iNum_kj = Result.Split(' ');
        int zj_zs = 0;
        int a = 0;
        int b = 0;
        int c = 0;
        int d = 0;
        int f = 0;
        for (int j = 0; j < iNum_kj.Length; j++)
        {
            if (Convert.ToInt32(iNum_kj[j]) == Convert.ToInt32(iNum_kj[0]))
                a += 1;
            if (Convert.ToInt32(iNum_kj[j]) == Convert.ToInt32(iNum_kj[1]))
                b += 1;
            if (Convert.ToInt32(iNum_kj[j]) == Convert.ToInt32(iNum_kj[2]))
                c += 1;
            if (Convert.ToInt32(iNum_kj[j]) == Convert.ToInt32(iNum_kj[3]))
                d += 1;
            if (Convert.ToInt32(iNum_kj[j]) == Convert.ToInt32(iNum_kj[4]))
                f += 1;
        }
        if (a >= 4)
            zj_zs = a;
        if (b >= 4)
            zj_zs = b;
        if (c >= 4)
            zj_zs = c;
        if (d >= 4)
            zj_zs = d;
        if (f >= 4)
            zj_zs = f;
        return zj_zs;
    }
    #endregion

    #region 葫芦算法
    private int HuLu(string Result)
    {
        string[] iNum_kj = Result.Split(' ');
        int zj_zs = 0;

        int count0 = Result.Replace("*", "").Replace("<img", "*").Split('0').Length - 1;
        int count1 = Result.Replace("*", "").Replace("<img", "*").Split('1').Length - 1;
        int count2 = Result.Replace("*", "").Replace("<img", "*").Split('2').Length - 1;
        int count3 = Result.Replace("*", "").Replace("<img", "*").Split('3').Length - 1;
        int count4 = Result.Replace("*", "").Replace("<img", "*").Split('4').Length - 1;
        int count5 = Result.Replace("*", "").Replace("<img", "*").Split('5').Length - 1;
        int count6 = Result.Replace("*", "").Replace("<img", "*").Split('6').Length - 1;
        int count7 = Result.Replace("*", "").Replace("<img", "*").Split('7').Length - 1;
        int count8 = Result.Replace("*", "").Replace("<img", "*").Split('8').Length - 1;
        int count9 = Result.Replace("*", "").Replace("<img", "*").Split('9').Length - 1;

        if (count0 == 3) { if (count1 == 2 || count2 == 2 || count3 == 2 || count4 == 2 || count5 == 2 || count6 == 2 || count7 == 2 || count8 == 2 || count9 == 2) zj_zs = 1; }
        if (count1 == 3) { if (count0 == 2 || count2 == 2 || count3 == 2 || count4 == 2 || count5 == 2 || count6 == 2 || count7 == 2 || count8 == 2 || count9 == 2) zj_zs = 1; }
        if (count2 == 3) { if (count1 == 2 || count0 == 2 || count3 == 2 || count4 == 2 || count5 == 2 || count6 == 2 || count7 == 2 || count8 == 2 || count9 == 2) zj_zs = 1; }
        if (count3 == 3) { if (count1 == 2 || count2 == 2 || count0 == 2 || count4 == 2 || count5 == 2 || count6 == 2 || count7 == 2 || count8 == 2 || count9 == 2) zj_zs = 1; }
        if (count4 == 3) { if (count1 == 2 || count2 == 2 || count3 == 2 || count0 == 2 || count5 == 2 || count6 == 2 || count7 == 2 || count8 == 2 || count9 == 2) zj_zs = 1; }
        if (count5 == 3) { if (count1 == 2 || count2 == 2 || count3 == 2 || count4 == 2 || count0 == 2 || count6 == 2 || count7 == 2 || count8 == 2 || count9 == 2) zj_zs = 1; }
        if (count6 == 3) { if (count1 == 2 || count2 == 2 || count3 == 2 || count4 == 2 || count5 == 2 || count0 == 2 || count7 == 2 || count8 == 2 || count9 == 2) zj_zs = 1; }
        if (count7 == 3) { if (count1 == 2 || count2 == 2 || count3 == 2 || count4 == 2 || count5 == 2 || count6 == 2 || count0 == 2 || count8 == 2 || count9 == 2) zj_zs = 1; }
        if (count8 == 3) { if (count1 == 2 || count2 == 2 || count3 == 2 || count4 == 2 || count5 == 2 || count6 == 2 || count7 == 2 || count0 == 2 || count9 == 2) zj_zs = 1; }
        if (count9 == 3) { if (count1 == 2 || count2 == 2 || count3 == 2 || count4 == 2 || count5 == 2 || count6 == 2 || count7 == 2 || count8 == 2 || count0 == 2) zj_zs = 1; }

        return zj_zs;
    }
    #endregion

    #region 牛牛算法
    ///<summary>
    ///牛牛算法
    ///返回result ，为空则是无牛
    /// </summary>
    private string Niu(string Result)
    {
        string result = string.Empty;
        string a = string.Empty;
        string b = string.Empty;
        string[] num = Result.Split(' ');

        if ((Convert.ToInt32(num[0]) + Convert.ToInt32(num[1]) + Convert.ToInt32(num[2])) % 10 == 0)//012
        {
            a = "牛";
            b = ((Convert.ToInt32(num[3]) + Convert.ToInt32(num[4])) % 10).ToString();
            result = a + b;
        }
        else if ((Convert.ToInt32(num[0]) + Convert.ToInt32(num[1]) + Convert.ToInt32(num[3])) % 10 == 0)//013
        {
            a = "牛";
            b = ((Convert.ToInt32(num[2]) + Convert.ToInt32(num[4])) % 10).ToString();
            result = a + b;
        }
        else if ((Convert.ToInt32(num[0]) + Convert.ToInt32(num[1]) + Convert.ToInt32(num[4])) % 10 == 0)//014
        {
            a = "牛";
            b = ((Convert.ToInt32(num[2]) + Convert.ToInt32(num[3])) % 10).ToString();
            result = a + b;
        }
        else if ((Convert.ToInt32(num[0]) + Convert.ToInt32(num[2]) + Convert.ToInt32(num[3])) % 10 == 0)//023
        {
            a = "牛";
            b = ((Convert.ToInt32(num[1]) + Convert.ToInt32(num[4])) % 10).ToString();
            result = a + b;
        }
        else if ((Convert.ToInt32(num[0]) + Convert.ToInt32(num[2]) + Convert.ToInt32(num[4])) % 10 == 0)//024
        {
            a = "牛";
            b = ((Convert.ToInt32(num[1]) + Convert.ToInt32(num[3])) % 10).ToString();
            result = a + b;
        }
        else if ((Convert.ToInt32(num[0]) + Convert.ToInt32(num[3]) + Convert.ToInt32(num[4])) % 10 == 0)//034
        {
            a = "牛";
            b = ((Convert.ToInt32(num[1]) + Convert.ToInt32(num[2])) % 10).ToString();
            result = a + b;
        }
        else if ((Convert.ToInt32(num[1]) + Convert.ToInt32(num[2]) + Convert.ToInt32(num[3])) % 10 == 0)//123
        {
            a = "牛";
            b = ((Convert.ToInt32(num[0]) + Convert.ToInt32(num[4])) % 10).ToString();
            result = a + b;
        }
        else if ((Convert.ToInt32(num[1]) + Convert.ToInt32(num[2]) + Convert.ToInt32(num[4])) % 10 == 0)//124
        {
            a = "牛";
            b = ((Convert.ToInt32(num[0]) + Convert.ToInt32(num[3])) % 10).ToString();
            result = a + b;
        }
        else if ((Convert.ToInt32(num[1]) + Convert.ToInt32(num[3]) + Convert.ToInt32(num[4])) % 10 == 0)//134
        {
            a = "牛";
            b = ((Convert.ToInt32(num[0]) + Convert.ToInt32(num[2])) % 10).ToString();
            result = a + b;
        }
        else if ((Convert.ToInt32(num[2]) + Convert.ToInt32(num[3]) + Convert.ToInt32(num[4])) % 10 == 0)//234
        {
            a = "牛";
            b = ((Convert.ToInt32(num[0]) + Convert.ToInt32(num[1])) % 10).ToString();
            result = a + b;
        }

        return result;
    }
    #endregion

    #region 结果信息显示
    /// <summary>
    /// 结果信息 总和-牛牛-梭哈
    /// </summary>
    /// <param name="Result"></param>
    /// <returns></returns>
    private string Message(string Result)
    {
        try
        {
            string message = string.Empty;

            string[] result = Result.Split(' ');
            int sum = 0;
            for (int i = 0; i < result.Length; i++)
            {
                sum += Convert.ToInt32(result[i]);
            }

            string niu = Niu(Result);
            if (niu != "")
            {
                if (Niu(Result) == "牛0")
                {
                    niu = "牛牛";
                }
                else
                {
                    niu = Niu(Result);
                }
            }
            else
            {
                niu = "无牛";
            }

            string suoha = string.Empty;
            if (Zhadan(Result) >= 4)
            {
                suoha = "炸弹";
            }
            if (HuLu(Result) == 1)
            {
                suoha = "葫芦";
            }
            if (SHShunzi(Result) == 1)
            {
                suoha = "顺子";
            }
            if (SHSantiao(Result) == 1)
            {
                suoha = "三条";
            }
            if (SHLiangdui(Result) == 1)
            {
                suoha = "两对";
            }
            if (SHDandui(Result) == 1)
            {
                suoha = "单对";
            }
            if (SHSanpai(Result) == 1)
            {
                suoha = "散牌";
            }

            message = sum.ToString() + "-" + niu + "-" + suoha;
            return message;
        }
        catch { return "未开奖"; }
    }
    #endregion

    #region 开放时间计算 IsOpen
    /// <summary>
    /// 开放时间计算
    /// </summary>
    /// <returns></returns>
    private bool IsOpen()
    {
        bool IsOpen = true;
        string OnTime = ub.GetSub("SSCOnTime", xmlPath);
        if (OnTime != "")
        {
            if (Utils.IsRegex(OnTime, @"^[0-9]{2}:[0-9]{2}-[0-9]{2}:[0-9]{2}$"))
            {
                string[] temp = OnTime.Split("-".ToCharArray());
                DateTime dt1 = Convert.ToDateTime(temp[0]);
                DateTime dt2 = Convert.ToDateTime(temp[1]);
                if (DateTime.Now > dt1 && DateTime.Now < dt2)
                {
                    IsOpen = true;
                }
                else
                {
                    IsOpen = false;
                }
            }
        }
        return IsOpen;
    }
    #endregion

    #region 游戏顶部
    private void GameTop()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href =\"" + Utils.getUrl("ssc.aspx") + "\">" + ub.GetSub("SSCName", xmlPath) + "</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    #endregion

    #region 游戏底部
    private void GameFoot()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏中心</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("klsf.aspx") + "\">" + ub.GetSub("SSCName", xmlPath) + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 快捷下注1
    private void kuai(int uid, int type, int ptype)//用户，游戏编号，下注类型
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string ac = Utils.GetRequest("ac", "all", 1, "", "");

        if (new BCW.QuickBet.BLL.QuickBet().ExistsUsID(meid))
        {

        }
        else//给会员自动添加默认的快捷下注
        {
            BCW.QuickBet.Model.QuickBet model = new BCW.QuickBet.Model.QuickBet();
            model.UsID = meid;
            model.Game = new BCW.QuickBet.BLL.QuickBet().GetGame();//十个编号的游戏|1:时时彩|2快乐十分|3:快乐扑克3|4:6场半|5:胜负彩
            model.Bet = new BCW.QuickBet.BLL.QuickBet().GetBety();
            new BCW.QuickBet.BLL.QuickBet().Add(model);
        }

        #region 快捷下注
        try
        {
            string game = new BCW.QuickBet.BLL.QuickBet().GetGame(meid);
            string bet = new BCW.QuickBet.BLL.QuickBet().GetBet(meid);
            string[] game1 = game.Split('#');
            string[] bet1 = bet.Split('#');
            for (int i = 0; i < game1.Length; i++)
            {
            }

            int j = 0;
            for (int i = 0; i < game1.Length; i++)
            {
                if (Convert.ToInt32(game1[i]) == type)//取出对应的游戏
                {
                    j = i;
                }
            }
            string str = string.Empty;
            string[] kuai = bet1[j].Split('|');//取出对应的快捷下注
            for (int i = 0; i < kuai.Length; i++)
            {
                if (kuai[i] != "0")
                {
                    if (Convert.ToInt64(kuai[i]) >= 10000)
                    {
                        if (Convert.ToInt64(kuai[i]) % 10000 == 0)
                        {
                            builder.Append("<input class=\"btn\" type=\"submit\" name=\"ac" + i + "\"  value=\"" + ChangeToWanSSC(kuai[i]) + "\" style =\"height:25px;\"  />" + "|");//
                        }
                        else
                        {
                            //string st = string.Empty; st = (Convert.ToInt64(kuai[i]) / 10000) + ".X万";
                            //builder.Append("<input class=\"btn\" type=\"hidden\" name=\"ace\"  value=\"" + i + "\" style =\"height:25px;\" />" + "");
                            builder.Append("<input class=\"btn\" type=\"submit\" name=\"ac" + i + "\"  value=\"" + Convert.ToInt64(kuai[i]) + "\" style =\"height:25px;\"  />" + "|");//Utils.ConvertGold(Convert.ToInt64(kuai[i]))
                            //Function(ac); 
                        }
                    }
                    else
                    {
                        builder.Append("<input class=\"btn\" type=\"submit\" name=\"ac" + i + "\"  value=\"" + Convert.ToInt64(kuai[i]) + "\" style =\"height:25px;\"  />" + "|");//
                    }
                }
            }

            builder.Append("<a href=\"" + Utils.getUrl("QuickBet.aspx?act=edit&amp;type=" + type + "&amp;ptype=" + ptype + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">添删</a>");
        }
        catch { }
        #endregion
    }
    #endregion

    #region 快捷下注2
    private void kuait(int uid, int type, int ptype, int t)//用户，游戏编号，下注类型，特殊下注编号
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string ac = Utils.GetRequest("ac", "all", 1, "", "");

        if (new BCW.QuickBet.BLL.QuickBet().ExistsUsID(meid))
        {

        }
        else//给会员自动添加默认的快捷下注
        {
            BCW.QuickBet.Model.QuickBet model = new BCW.QuickBet.Model.QuickBet();
            model.UsID = meid;
            model.Game = new BCW.QuickBet.BLL.QuickBet().GetGame();//十个编号的游戏|1:时时彩|2快乐十分|3:快乐扑克3|4:6场半|5:胜负彩
            model.Bet = new BCW.QuickBet.BLL.QuickBet().GetBety();
            new BCW.QuickBet.BLL.QuickBet().Add(model);
        }

        #region 快捷下注
        try
        {
            string game = new BCW.QuickBet.BLL.QuickBet().GetGame(meid);
            string bet = new BCW.QuickBet.BLL.QuickBet().GetBet(meid);
            string[] game1 = game.Split('#');
            string[] bet1 = bet.Split('#');
            for (int i = 0; i < game1.Length; i++)
            {
            }

            int j = 0;
            for (int i = 0; i < game1.Length; i++)
            {
                if (Convert.ToInt32(game1[i]) == type)//取出对应的游戏
                {
                    j = i;
                }
            }
            string str = string.Empty;
            string gold = string.Empty;
            string[] kuai = bet1[j].Split('|');//取出对应的快捷下注
            for (int i = 0; i < kuai.Length; i++)
            {
                //if (Convert.ToInt64(kuai[i]) >= 10000)
                //{
                //    if (Convert.ToInt64(kuai[i]) % 10000 == 0)
                //    {
                //        gold= Utils.ConvertGold(Convert.ToInt64(kuai[i])) ;//
                //    }
                //    else
                //    {
                //        gold = kuai[i];
                //    }
                //}
                //else
                //{
                //    gold = kuai[i];
                //}

                gold = ChangeToWanSSC(kuai[i]);

                if (kuai[i] != "0")
                {
                    if (ptype == 17)//龙虎和
                    {
                        if (t == 1)
                        {
                            if (i == 1)
                            {
                                builder.Append("<input class=\"btn\" type=\"submit\" name=\"yl11\"  value=\"" + gold + "\" style =\"height:25px;\"  />" + "|");
                            }
                            else
                            {
                                builder.Append("<input class=\"btn\" type=\"submit\" name=\"yl" + i + "\"  value=\"" + gold + "\" style =\"height:25px;\"  />" + "|");
                            }
                        }
                        if (t == 2)
                        {
                            if (i == 1)
                            {
                                builder.Append("<input class=\"btn\" type=\"submit\" name=\"yh11\"  value=\"" + gold + "\" style =\"height:25px;\"  />" + "|");
                            }
                            else
                            {
                                builder.Append("<input class=\"btn\" type=\"submit\" name=\"yh" + i + "\"  value=\"" + gold + "\" style =\"height:25px;\"  />" + "|");
                            }
                        }
                        if (t == 3)
                        {
                            if (i == 1)
                            {
                                builder.Append("<input class=\"btn\" type=\"submit\" name=\"yhe11\"  value=\"" + gold + "\" style =\"height:25px;\"  />" + "|");
                            }
                            else
                            {
                                builder.Append("<input class=\"btn\" type=\"submit\" name=\"yhe" + i + "\"  value=\"" + gold + "\" style =\"height:25px;\"  />" + "|");
                            }
                        }
                    }
                    else if (ptype == 28)//总和大小单双
                    {
                        if (t == 1)
                        {
                            if (i == 1)
                            {
                                builder.Append("<input class=\"btn\" type=\"submit\" name=\"dd11\"  value=\"" + gold + "\" style =\"height:25px;\"  />" + "|");
                            }
                            else
                            {
                                builder.Append("<input class=\"btn\" type=\"submit\" name=\"dd" + i + "\"  value=\"" + gold + "\" style =\"height:25px;\"  />" + "|");
                            }
                        }
                        if (t == 2)
                        {
                            if (i == 1)
                            {
                                builder.Append("<input class=\"btn\" type=\"submit\" name=\"ds11\"  value=\"" + gold + "\" style =\"height:25px;\"  />" + "|");
                            }
                            else
                            {
                                builder.Append("<input class=\"btn\" type=\"submit\" name=\"ds" + i + "\"  value=\"" + gold + "\" style =\"height:25px;\"  />" + "|");
                            }
                        }
                        if (t == 3)
                        {
                            if (i == 1)
                            {
                                builder.Append("<input class=\"btn\" type=\"submit\" name=\"xd11\"  value=\"" + gold + "\" style =\"height:25px;\"  />" + "|");
                            }
                            else
                            {
                                builder.Append("<input class=\"btn\" type=\"submit\" name=\"xd" + i + "\"  value=\"" + gold + "\" style =\"height:25px;\"  />" + "|");
                            }
                        }
                        if (t == 4)
                        {
                            if (i == 1)
                            {
                                builder.Append("<input class=\"btn\" type=\"submit\" name=\"xs11\"  value=\"" + gold + "\" style =\"height:25px;\"  />" + "|");
                            }
                            else
                            {
                                builder.Append("<input class=\"btn\" type=\"submit\" name=\"xs" + i + "\"  value=\"" + gold + "\" style =\"height:25px;\"  />" + "|");
                            }
                        }
                    }
                    else if (ptype == 2 || ptype == 5 || ptype == 8 || ptype == 11 || ptype == 14 || ptype == 25 || ptype == 36 || ptype == 43 || ptype == 50)
                    {
                        if (t == 1)
                        {
                            if (i == 1)
                            {
                                builder.Append("<input class=\"btn\" type=\"submit\" name=\"ddn11\"  value=\"" + gold + "\" style =\"height:25px;\"  />" + "|");
                            }
                            else
                            {
                                builder.Append("<input class=\"btn\" type=\"submit\" name=\"ddn" + i + "\"  value=\"" + gold + "\" style =\"height:25px;\"  />" + "|");
                            }
                        }
                        if (t == 2)
                        {
                            if (i == 1)
                            {
                                builder.Append("<input class=\"btn\" type=\"submit\" name=\"xsn11\"  value=\"" + gold + "\" style =\"height:25px;\"  />" + "|");
                            }
                            else
                            {
                                builder.Append("<input class=\"btn\" type=\"submit\" name=\"xsn" + i + "\"  value=\"" + gold + "\" style =\"height:25px;\"  />" + "|");
                            }
                        }
                    }


                    else if (ptype == 3 || ptype == 6 || ptype == 9 || ptype == 12 || ptype == 15 || ptype == 26 || ptype == 37 || ptype == 44 || ptype == 51)
                    {
                        if (t == 1)
                        {
                            if (i == 1)
                            {
                                builder.Append("<input class=\"btn\" type=\"submit\" name=\"ddnds11\"  value=\"" + gold + "\" style =\"height:25px;\"  />" + "|");
                            }
                            else
                            {
                                builder.Append("<input class=\"btn\" type=\"submit\" name=\"ddnds" + i + "\"  value=\"" + gold + "\" style =\"height:25px;\"  />" + "|");
                            }
                        }
                        if (t == 2)
                        {
                            if (i == 1)
                            {
                                builder.Append("<input class=\"btn\" type=\"submit\" name=\"xsnds11\"  value=\"" + gold + "\" style =\"height:25px;\"  />" + "|");
                            }
                            else
                            {
                                builder.Append("<input class=\"btn\" type=\"submit\" name=\"xsnds" + i + "\"  value=\"" + gold + "\" style =\"height:25px;\"  />" + "|");
                            }
                        }
                    }
                    else if (ptype == 23)
                    {
                        if (t == 1)
                        {
                            if (i == 1)
                            {
                                builder.Append("<input class=\"btn\" type=\"submit\" name=\"ddnyn11\"  value=\"" + gold + "\" style =\"height:25px;\"  />" + "|");
                            }
                            else
                            {
                                builder.Append("<input class=\"btn\" type=\"submit\" name=\"ddnyn" + i + "\"  value=\"" + gold + "\" style =\"height:25px;\"  />" + "|");
                            }
                        }
                        if (t == 2)
                        {
                            if (i == 1)
                            {
                                builder.Append("<input class=\"btn\" type=\"submit\" name=\"xsnyn11\"  value=\"" + gold + "\" style =\"height:25px;\"  />" + "|");
                            }
                            else
                            {
                                builder.Append("<input class=\"btn\" type=\"submit\" name=\"xsnyn" + i + "\"  value=\"" + gold + "\" style =\"height:25px;\"  />" + "|");
                            }
                        }
                    }

                }
            }

            builder.Append("<a href=\"" + Utils.getUrl("QuickBet.aspx?act=edit&amp;type=" + type + "&amp;ptype=" + ptype + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">添删</a>");
        }
        catch { }
        #endregion
    }
    #endregion

    #region 时时彩快捷下注转换成万
    /// <summary>
    /// 时时彩快捷下注转换成整万
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private string ChangeToWanSSC(string str)
    {
        string CW = string.Empty;
        try
        {
            long first = 0;
            first = Convert.ToInt64(str.Trim());
            if (first >= 10000)
            {
                if (first % 10000 == 0)
                {
                    CW = (first / 10000) + "万";
                }
                else
                {
                    CW = first.ToString();
                }
            }
            else
            {
                CW = first.ToString();
            }
        }
        catch { }
        return CW;
    }
    #endregion

    //private void Function(string name )
    //{
    //    builder.Append("<script language=\"JavaScript\"> ");
    //    builder.Append("   function checkUser(){");
    //    builder.Append("   var id = document.getElementById('" + name + "').value;");
    //    builder.Append(" var str = \" \";");
    //    builder.Append("    var value = new Array();");
    //    //builder.Append("   for(var i = 0; i < id.length; i++){");
    //    //builder.Append("     if(id[i].checked){");
    //    //builder.Append("   str =5;}");

    //    //builder.Append("    }  ");
    //    builder.Append("alert(id);");
    //   builder.Append(" document.getElementById(form1).submit();");
    //    builder.Append(" }");
    //    builder.Append("</script>");
    //    // return se;
    //}
}
