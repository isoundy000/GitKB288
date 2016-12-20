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

/// <summary>
/// 跑马修改消费记录
/// 
/// 黄国军 20160312
/// 
/// 邵广林 20160617 动态添加usid
/// 
/// 姚志光 20160621 活跃抽奖控制抽奖入口
/// </summary>
public partial class bbs_game_horse : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/horse.xml";
    protected string strText = string.Empty;
    protected string strName = string.Empty;
    protected string strType = string.Empty;
    protected string strValu = string.Empty;
    protected string strEmpt = string.Empty;
    protected string strIdea = string.Empty;
    protected string strOthe = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = ub.GetSub("HorseName", xmlPath);
        //维护提示
        if (ub.GetSub("HorseStatus", xmlPath) == "1")
        {
            Utils.Safe("此游戏");
        }
        bool IsWin = false;
        if (IsOpen() == true)
        {
            BCW.Model.Game.Horselist Horse = new BCW.BLL.Game.Horselist().GetHorselist();
            if (Horse.ID == 0)
            {
                //第一局开始
                Horse.WinNum = 0;
                Horse.WinCount = 0;
                Horse.WinPool = 0;
                Horse.State = 0;
                Horse.Odds = new BCW.User.Game.Horse().GetOdds();
                //开奖周局分钟
                int CycleMin = Utils.ParseInt(ub.GetSub("HorseCycleMin", xmlPath));
                Horse.Pool = 0;
                Horse.BeginTime = DateTime.Now;
                Horse.EndTime = DateTime.Now.AddMinutes(Convert.ToDouble(CycleMin));
                Horse.ID = new BCW.BLL.Game.Horselist().Add(Horse);
            }
            DateTime StartTime = Horse.EndTime.AddSeconds(30);//赛马用时时间
            long Diff = DT.DateDiff(Horse.EndTime, DateTime.Now, 4);
            if (Diff <= 15)
            {
                IsWin = true;
            }
            if (StartTime > DateTime.Now)
            {
                builder.Append(new BCW.User.Game.Horse().OutOpen(Horse.ID, Horse.EndTime, Horse.Odds));
            }
            else
            {

                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;跑马");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("正在转入下一局...");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div class=\"title\">", Out.RHr()));
                builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
                builder.Append(Out.Tab("</div>", ""));

                new BCW.User.Game.Horse().HorsePage();
                new BCW.User.Game.Horse().OpenNext();
                IsWin = true;
            }
        }
        if (!IsWin)
        {
            string act = Utils.GetRequest("act", "all", 1, "", "");
            switch (act)
            {
                case "pay":
                    PayPage();
                    break;
                case "payok":
                    PayOkPage();
                    break;
                case "paysave":
                    PaySavePage();
                    break;
                case "list":
                    ListPage();
                    break;
                case "listview":
                    ListViewPage();
                    break;
                case "raceview":
                    RaceViewPage();
                    break;
                case "mylist":
                    MyListPage();
                    break;
                case "case":
                    CasePage();
                    break;
                case "caseok":
                    CaseOkPage();
                    break;
                case "casepost":
                    CasePostPage();
                    break;
                case "help":
                    HelpPage();
                    break;
                case "top":
                    TopPage();
                    break;
                default:
                    ReloadPage();
                    break;
            }
        }
    }
    private void ReloadPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        string Logo = ub.GetSub("HorseLogo", xmlPath);
        if (Logo != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<img src=\"" + Logo + "\" alt=\"load\"/>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        //拾物随机
        builder.Append(BCW.User.Game.GiftFlows.ShowGiftFlows(5));

        string Notes = ub.GetSub("HorseNotes", xmlPath);
        if (Notes != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.SysUBB(Notes) + "");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;跑马");
        builder.Append(Out.Tab("</div>", "<br />"));
        BCW.Model.Game.Horselist horse = null;
        if (IsOpen() == true)
        {
            horse = new BCW.BLL.Game.Horselist().GetHorselist();
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("距本局开跑还有" + DT.DateDiff(horse.EndTime, DateTime.Now, 4) + "秒|<a href=\"" + Utils.getUrl("horse.aspx") + "\">刷新</a>");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("本局押注总量:" + Utils.ConvertGold(horse.Pool) + "" + ub.Get("SiteBz") + "");
            if (Isbz())
                builder.Append("|" + Utils.ConvertGold(horse.WinPool) + "" + ub.Get("SiteBz2") + "");

            BCW.Model.Game.Horselist bf = new BCW.BLL.Game.Horselist().GetHorselistBf();
            if (bf != null)
            {
                builder.Append("<br />上局跑出[" + Utils.ConvertSeparated(bf.WinNum.ToString(), 1, "-") + "]赔" + ForOdds(bf.Odds, Utils.ConvertSeparated(bf.WinNum.ToString(), 1, "-")) + "倍 ");
                if (bf.WinCount == 0)
                    builder.Append("通吃!!!");
                else
                    builder.Append("共<a href=\"" + Utils.getUrl("horse.aspx?act=listview&amp;id=" + bf.ID + "") + "\">" + bf.WinCount + "</a>人次中");

            }
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("游戏开放时间:" + ub.GetSub("HorseOnTime", xmlPath) + "");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("horse.aspx?act=help") + "\">游戏规则</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("horse.aspx?act=case") + "\">兑奖</a>|<a href=\"" + Utils.getUrl("horse.aspx?act=top") + "\">排行</a><br />");
        builder.Append("我的<a href=\"" + Utils.getUrl("horse.aspx?act=mylist&amp;ptype=1") + "\">未开</a>|<a href=\"" + Utils.getUrl("horse.aspx?act=mylist&amp;ptype=2") + "\">历史记录</a><br />");
        builder.Append("您目前自带" + Utils.ConvertGold(new BCW.BLL.User().GetGold(meid)) + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        if (IsOpen() == true)
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("==<a href=\"" + Utils.getUrl("horse.aspx?act=pay") + "\">下单买马</a>==");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<table columns=\"6\">");
            for (int i = 0; i <= 5; i++)
            {
                builder.Append("<tr>");
                if (Utils.Isie())
                    builder.Append("<td style=\"background-color:#EAEAEA;\">");
                else
                    builder.Append("<td>");

                if (i == 0)
                    builder.Append("押");
                else
                    builder.Append(i);

                builder.Append("</td>");

                int k = 6;
                while (k >= 2)
                {
                    if (i == 0)
                    {
                        if (Utils.Isie())
                            builder.Append("<td style=\"background-color:#EAEAEA;\">");
                        else
                            builder.Append("<td>");

                        builder.Append(k);
                    }
                    else
                    {
                        builder.Append("<td>");
                        if (i < k)
                            builder.Append("<a href=\"" + Utils.getUrl("horse.aspx?act=payok&amp;ptype=" + i + "" + k + "") + "\">" + ForOdds(horse.Odds, "" + i + "-" + k + "") + "</a>");
                        else
                            builder.Append("");
                    }
                    builder.Append("</td>");
                    k = k - 1;
                }
                builder.Append("</tr>");
            }
            builder.Append("</table>");
        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("==往期分析==");
        builder.Append(Out.Tab("</div>", "<br />"));

        IList<BCW.Model.Game.Horselist> listHorselist = new BCW.BLL.Game.Horselist().GetHorselists(3, "State=1");
        if (listHorselist.Count > 0)
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            int k = 1;
            foreach (BCW.Model.Game.Horselist n in listHorselist)
            {

                builder.Append("<a href=\"" + Utils.getUrl("horse.aspx?act=listview&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">第" + n.ID + "局:[" + Utils.ConvertSeparated(n.WinNum.ToString(), 1, "-") + "]赔" + ForOdds(n.Odds, Utils.ConvertSeparated(n.WinNum.ToString(), 1, "-")) + "倍</a>");
                builder.Append("|<a href=\"" + Utils.getUrl("horse.aspx?act=raceview&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">现场</a><br />");
                k++;
            }
            builder.Append("<a href=\"" + Utils.getUrl("horse.aspx?act=list") + "\">&gt;&gt;查看历史开奖</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        //闲聊显示
        builder.Append(Out.SysUBB(BCW.User.Users.ShowSpeak(10, "horse.aspx", 5, 0)));

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

        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-3]$", "0"));
        BCW.Model.Game.Horselist horse = new BCW.BLL.Game.Horselist().GetHorselist();

        //拾物随机
        builder.Append(BCW.User.Game.GiftFlows.ShowGiftFlows(5));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("horse.aspx") + "\">跑马</a>&gt;买马");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        if (horse.EndTime > DateTime.Now)
            builder.Append("本局开奖还有" + DT.DateDiff(horse.EndTime, DateTime.Now, 4) + "秒|");
        else
            builder.Append("系统正在开奖,请稍后|");

        builder.Append("<a href=\"" + Utils.getUrl("horse.aspx?act=pay&amp;ptype=" + ptype + "") + "\">刷新</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 0)
            builder.Append("全部|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("horse.aspx?act=pay&amp;ptype=0") + "\">全部</a>|");

        if (ptype == 1)
            builder.Append("热门|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("horse.aspx?act=pay&amp;ptype=1") + "\">热门</a>|");

        if (ptype == 2)
            builder.Append("低赔|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("horse.aspx?act=pay&amp;ptype=2") + "\">低赔</a>|");

        if (ptype == 3)
            builder.Append("高赔");
        else
            builder.Append("<a href=\"" + Utils.getUrl("horse.aspx?act=pay&amp;ptype=3") + "\">高赔</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        string[] sText = { "1-2", "1-3", "1-4", "1-5", "1-6", "2-3", "2-4", "2-5", "2-6", "3-4", "3-5", "3-6", "4-5", "4-6", "5-6" };
        int[] iText = { ForOdds(horse.Odds, "1-2"), ForOdds(horse.Odds, "1-3"), ForOdds(horse.Odds, "1-4"), ForOdds(horse.Odds, "1-5"), ForOdds(horse.Odds, "1-6"), ForOdds(horse.Odds, "2-3"), ForOdds(horse.Odds, "2-4"), ForOdds(horse.Odds, "2-5"), ForOdds(horse.Odds, "2-6"), ForOdds(horse.Odds, "3-4"), ForOdds(horse.Odds, "3-5"), ForOdds(horse.Odds, "3-6"), ForOdds(horse.Odds, "4-5"), ForOdds(horse.Odds, "4-6"), ForOdds(horse.Odds, "5-6") };

        if (ptype == 0)
        {
            for (int i = 0; i < sText.Length; i++)
            {
                int iType = Convert.ToInt32(sText[i].Replace("-", ""));
                strText = ",,,,";
                strName = "paycent" + iType + ",ptype,act,backurl";
                strType = "snum,hidden,hidden,hidden";
                strValu = "" + ub.GetSub("HorseDtPay", xmlPath) + "'" + iType + "'payok'" + Utils.PostPage(1) + "";
                strEmpt = "false,false,false,false";
                strIdea = "";
                strOthe = "押,horse.aspx,post,3,other,[" + sText[i] + "]x" + ForOdds(horse.Odds, sText[i].ToString()) + "";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("", "<br />"));
            }
        }
        else if (ptype == 1)
        {
            DataSet ds = new BCW.BLL.Game.Horsepay().GetList("Top 5 Types,Count(Types) AS BCount,Sum(BuyCent) AS BCent", "HorseId=" + horse.ID + " GROUP BY Types ORDER BY Sum(BuyCent) DESC");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    int iType = int.Parse(ds.Tables[0].Rows[i]["Types"].ToString());
                    string sType = Utils.ConvertSeparated(iType.ToString(), 1, "-");
                    long BCent = Int64.Parse(ds.Tables[0].Rows[i]["BCent"].ToString());
                    int BCount = int.Parse(ds.Tables[0].Rows[i]["BCount"].ToString());
                    strText = ",,,,";
                    strName = "paycent" + iType + ",ptype,act,backurl";
                    strType = "snum,hidden,hidden,hidden";
                    strValu = "" + ub.GetSub("HorseDtPay", xmlPath) + "'" + iType + "'payok'" + Utils.PostPage(1) + "";
                    strEmpt = "false,false,false,false";
                    long DCent = new BCW.BLL.Game.Horsepay().GetSumBuyCent(horse.ID, 0, iType);
                    strOthe = "押,horse.aspx,post,3,other,[" + sType + "]x" + ForOdds(horse.Odds, sType) + "<br />|共" + BCount + "人押 " + DCent + "" + ub.Get("SiteBz") + "";
                    if (Isbz())
                        strOthe += "/" + (BCent - DCent) + "" + ub.Get("SiteBz2") + "";

                    builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                    builder.Append(Out.Tab("", "<br />"));
                }
            }
        }
        else if (ptype >= 2)
        {
            int i, j;
            int key;
            int n = 15;
            int[] A = iText;
            for (i = 1; i < n; i++)
            {
                j = i;
                key = A[i];
                while (j > 0 && A[j - 1] > key)
                {
                    A[j] = A[j - 1];
                    j--;
                }
                A[j] = key;
            }
            int k = 0;
            int kLength = 7;
            if (ptype == 1)
            {
                kLength = 5;
            }
            if (ptype == 3)
            {
                k = 8;
                kLength = 15;
            }
            for (i = k; i < kLength; i++)
            {
                int iType = Convert.ToInt32(ForType(horse.Odds, A[i].ToString()).Replace("-", ""));
                strText = ",,,,";
                strName = "paycent" + iType + ",ptype,act,backurl";
                strType = "snum,hidden,hidden,hidden";
                strValu = "" + ub.GetSub("HorseDtPay", xmlPath) + "'" + iType + "'payok'" + Utils.PostPage(1) + "";
                strEmpt = "false,false,false,false";
                strIdea = "";
                strOthe = "押,horse.aspx,post,3,other,[" + ForType(horse.Odds, A[i].ToString()) + "]x" + ForOdds(horse.Odds, ForType(horse.Odds, A[i].ToString())) + "<br />|共" + new BCW.BLL.Game.Horsepay().GetCount(horse.ID, iType) + "人押 " + new BCW.BLL.Game.Horsepay().GetSumBuyCent(horse.ID, 0, iType) + "" + ub.Get("SiteBz") + "";
                if (Isbz())
                    strOthe += "/" + new BCW.BLL.Game.Horsepay().GetSumBuyCent(horse.ID, 1, iType) + "" + ub.Get("SiteBz2") + "";

                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("", "<br />"));
            }

        }
        builder.Append(Out.Tab("<div>", Out.RHr()));
        builder.Append("<a href=\"" + Utils.getUrl("horse.aspx") + "\">返回跑马游戏</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("horse.aspx") + "\">跑马</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void PayOkPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 2, @"[1-5][2-6]$", "押注类型错误"));
        long paycent = Int64.Parse(Utils.GetRequest("paycent" + ptype + "", "post", 1, @"[0-9]\d*$", "0"));

        BCW.Model.Game.Horselist horse = new BCW.BLL.Game.Horselist().GetHorselist();

        //拾物随机
        builder.Append(BCW.User.Game.GiftFlows.ShowGiftFlows(5));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("horse.aspx") + "\">跑马</a>&gt;买马");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        if (horse.EndTime > DateTime.Now)
            builder.Append("本局开奖还有" + DT.DateDiff(horse.EndTime, DateTime.Now, 4) + "秒|");
        else
            builder.Append("系统正在开奖,请稍后|");

        builder.Append("<a href=\"" + Utils.getUrl("horse.aspx?act=payok&amp;ptype=" + ptype + "&amp;paycent=" + paycent + "&amp;backurl=" + Utils.getPage(0) + "") + "\">刷新</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        string sType = Utils.ConvertSeparated(ptype.ToString(), 1, "-");
        int odds = ForOdds(horse.Odds, "" + sType + "");
        builder.Append("第" + horse.ID + "局您选择[" + sType + "]x" + odds + "<br />");
        builder.Append("即第" + Utils.Left(ptype.ToString(), 1) + "和第" + Utils.Right(ptype.ToString(), 1) + "匹马跑前两位,赔率为" + odds + "<br />");
        if (paycent > 0)
        {
            builder.Append("押注为" + paycent + "币<br />");
        }

        if (Isbz())
            builder.Append("您目前自带:" + Utils.ConvertGold(new BCW.BLL.User().GetGold(meid)) + "" + ub.Get("SiteBz") + "/" + Utils.ConvertGold(new BCW.BLL.User().GetMoney(meid)) + "" + ub.Get("SiteBz2") + "");
        else
            builder.Append("您目前自带:" + Utils.ConvertGold(new BCW.BLL.User().GetGold(meid)) + "" + ub.Get("SiteBz") + "");

        if (paycent > 0)
        {
            builder.Append("<br />" + ub.Get("SiteBz") + "限" + ub.GetSub("HorseSmallPay", xmlPath) + "-" + Convert.ToInt64(ub.GetSub("HorseBigPay", xmlPath)) + "");///" + ub.Get("SiteBz2") + "限" + ub.GetSub("HorseSmallPay", xmlPath) + "-" + (Convert.ToInt64(ub.GetSub("HorseBigPay", xmlPath)) * 10) + "
        }

        builder.Append(Out.Tab("</div>", "<br />"));
        if (paycent > 0)
        {
            strName = "paycent,id,ptype,act,backurl";
            strValu = "" + paycent + "'" + horse.ID + "'" + ptype + "'paysave'" + Utils.getPage(0) + "";
            if (Isbz())
                strOthe = "押" + ub.Get("SiteBz") + "|押" + ub.Get("SiteBz2") + ",horse.aspx,post,0,red|blue";
            else
                strOthe = "" + ub.Get("SiteBz") + "押注,horse.aspx,post,0,red";

            builder.Append(Out.wapform(strName, strValu, strOthe));

            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<a href=\"" + Utils.getPage("horse.aspx?act=pay&amp;ptype=" + ptype + "") + "\">再看看吧</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
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
                        strName = "paycent,id,ptype,act,backurl";
                        strValu = "" + sTemp[i] + "'" + horse.ID + "'" + ptype + "'paysave'" + Utils.PostPage(1) + "";
                        strOthe = "" + sTemp[i + 1] + ",horse.aspx,post,0,other";
                        builder.Append(Out.wapform(strName, strValu, strOthe));
                    }
                    //if ((i + 1) % 8 == 0 && (i + 1 != sTemp.Length))
                    if ((i + 1) % 8 == 0)
                        builder.Append(Out.Tab("", "<br />"));
                    else if (i % 2 != 0 && (i + 1 != sTemp.Length))
                        builder.Append(Out.Tab("", ". "));
                }
                Ps = "或";
            }
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("" + Ps + "输入金额:<br />" + ub.Get("SiteBz") + "限" + ub.GetSub("HorseSmallPay", xmlPath) + "-" + ub.GetSub("HorseBigPay", xmlPath) + "");///" + ub.Get("SiteBz2") + "限" + ub.GetSub("HorseSmallPay", xmlPath) + "-" + (Convert.ToInt64(ub.GetSub("HorseBigPay", xmlPath)) * 10) + "
            builder.Append(Out.Tab("</div>", ""));
            strText = ",,,,";
            strName = "paycent,id,ptype,act,backurl";
            strType = "snum,hidden,hidden,hidden,hidden";
            strValu = "" + ub.GetSub("HorseDtPay", xmlPath) + "'" + horse.ID + "'" + ptype + "'paysave'" + Utils.PostPage(1) + "";
            strEmpt = "false,false,false,false,false";
            strIdea = "";
            if (Isbz())
                strOthe = "押" + ub.Get("SiteBz") + "|押" + ub.Get("SiteBz2") + ",horse.aspx,post,0,red|blue";
            else
                strOthe = "押" + ub.Get("SiteBz") + ",horse.aspx,post,0,red";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("horse.aspx") + "\">返回跑马游戏</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("horse.aspx") + "\">跑马</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void PaySavePage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string ac = Utils.GetRequest("ac", "post", 1, "", "");
        int ptype = int.Parse(Utils.GetRequest("ptype", "post", 2, @"[1-5][2-6]$", "押注类型错误"));
        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"[1-9]\d*$", "ID错误"));

        long paycent = Int64.Parse(Utils.GetRequest("paycent", "post", 4, @"[0-9]\d*$", "押注金额错误"));

        //是否刷屏
        string appName = "LIGHT_HORSE";
        int Expir = Utils.ParseInt(ub.GetSub("HorseExpir", xmlPath));

        int bzType = 0;
        string bzText = string.Empty;
        long gold = 0;
        //if (Utils.ToSChinese(ac).Contains(Utils.ToSChinese(ub.Get("SiteBz2"))))
        //{
        //bzType = 1;
        //bzText = ub.Get("SiteBz2");
        //gold = new BCW.BLL.User().GetMoney(meid);
        //BCW.User.Users.IsFresh(appName, Expir);
        //if (paycent < Convert.ToInt64(ub.GetSub("HorseSmallPay", xmlPath)) || paycent > (Convert.ToInt64(ub.GetSub("HorseBigPay", xmlPath)) * 10))
        //{
        //Utils.Error("押注金额限" + ub.GetSub("HorseSmallPay", xmlPath) + "-" + (Convert.ToInt64(ub.GetSub("HorseBigPay", xmlPath)) * 10) + "" + bzText + "", "");
        //}
        //}
        //else
        //{
        //支付安全提示
        string[] p_pageArr = { "act", "id", "ptype", "paycent", "ac" };
        BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr);

        bzType = 0;
        bzText = ub.Get("SiteBz");
        gold = new BCW.BLL.User().GetGold(meid);
        long small = Convert.ToInt64(ub.GetSub("HorseSmallPay", xmlPath));
        long big = Convert.ToInt64(ub.GetSub("HorseBigPay", xmlPath));
        BCW.User.Users.IsFresh(appName, Expir, paycent, small, big);
        //}
        if (paycent > gold)
        {
            Utils.Error("你的" + bzText + "不足", Utils.getPage("horse.aspx"));
        }

        BCW.Model.Game.Horselist horse = new BCW.BLL.Game.Horselist().GetHorselist(id);

        if (horse.EndTime < DateTime.Now)
        {
            Utils.Error("本局已截止下注", Utils.getUrl("horse.aspx"));
        }

        string mename = new BCW.BLL.User().GetUsName(meid);
        string sType = "[" + Utils.ConvertSeparated(ptype.ToString(), 1, "-") + "]";
        //加总押注额
        if (bzType == 0)
        {
            new BCW.BLL.Game.Horselist().UpdatePool(horse.ID, paycent);
            new BCW.BLL.User().UpdateiGold(meid, mename, -paycent, "跑马" + horse.ID + "局押" + sType);
        }
        else
        {
            new BCW.BLL.Game.Horselist().UpdateWinPool(horse.ID, paycent);
            new BCW.BLL.User().UpdateiMoney(meid, mename, -paycent, "跑马" + horse.ID + "局押" + sType);
        }
        BCW.Model.Game.Horsepay model = new BCW.Model.Game.Horsepay();
        model.HorseId = horse.ID;
        model.UsID = meid;
        model.UsName = mename;
        model.Types = ptype;
        model.BuyCent = paycent;
        model.WinCent = 0;
        model.AddTime = DateTime.Now;
        model.State = 0;
        model.bzType = bzType;
        if (!new BCW.BLL.Game.Horsepay().Exists(horse.ID, meid, bzType, ptype))
            new BCW.BLL.Game.Horsepay().Add(model);
        else
            new BCW.BLL.Game.Horsepay().Update(model);

        string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/horse.aspx]跑马第" + horse.ID + "局[/url]押注" + paycent + "" + bzText + "";
        new BCW.BLL.Action().Add(10, 0, meid, "", wText);
        //活跃抽奖入口_20160621姚志光
        try
        {
            //表中存在记录
            if (new BCW.BLL.tb_WinnersGame().ExistsGameName(ub.GetSub("HorseName", xmlPath)))
            {
                //投注是否大于设定的限额，是则有抽奖机会
                if (paycent > new BCW.BLL.tb_WinnersGame().GetPrice(ub.GetSub("HorseName", xmlPath)))
                {
                    string TextForUbb = (ub.GetSub("TextForUbb", "/Controls/winners.xml"));//设置内线提示的文字
                    string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", "/Controls/winners.xml"));//1发内线2不发内线 
                    int hit = new BCW.winners.winners().CheckActionForAll(1, 1, meid, mename, ub.GetSub("HorseName", xmlPath)+"跑马", 3);
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
        Utils.Success("押注", "押注成功，花费了" + paycent + "" + bzText + "<br /><a href=\"" + Utils.getPage("horse.aspx?act=pay&amp;ptype=" + ptype + "") + "\">&gt;继续下注</a>", Utils.getUrl("horse.aspx"), "5");
    }

    private void ListPage()
    {
        Master.Title = "历史开奖";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("=历史开奖=");
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
        IList<BCW.Model.Game.Horselist> listHorselist = new BCW.BLL.Game.Horselist().GetHorselists(pageIndex, pageSize, strWhere, out recordCount);
        if (listHorselist.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Horselist n in listHorselist)
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
                builder.Append("第" + n.ID + "期跑出:<a href=\"" + Utils.getUrl("horse.aspx?act=listview&amp;id=" + n.ID + "") + "\">[" + Utils.ConvertSeparated(n.WinNum.ToString(), 1, "-") + "]赔" + ForOdds(n.Odds, Utils.ConvertSeparated(n.WinNum.ToString(), 1, "-")) + "倍</a>|<a href=\"" + Utils.getUrl("horse.aspx?act=raceview&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">现场</a>");
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
        builder.Append("<a href=\"" + Utils.getUrl("horse.aspx?act=mylist&amp;ptype=1") + "\">未开投注</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("horse.aspx?act=mylist&amp;ptype=2") + "\">历史投注</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append(" <a href=\"" + Utils.getUrl("horse.aspx") + "\">跑马</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ListViewPage()
    {
        int id = Utils.ParseInt(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.Game.Horselist model = new BCW.BLL.Game.Horselist().GetHorselist(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "第" + id + "局跑马";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getPage("horse.aspx?act=list") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        strWhere += "HorseId=" + id + " and WinCent>0";

        string[] pageValUrl = { "act", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Game.Horsepay> listHorsepay = new BCW.BLL.Game.Horsepay().GetHorsepays(pageIndex, pageSize, strWhere, out recordCount);
        if (listHorsepay.Count > 0)
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("第" + id + "局奖池押注:" + model.Pool + "");
            builder.Append("<br />共" + recordCount + "注中奖");
            builder.Append(Out.Tab("</div>", "<br />"));

            int k = 1;
            foreach (BCW.Model.Game.Horsepay n in listHorsepay)
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
                string bzTypes = string.Empty;
                if (n.bzType == 0)
                    bzTypes = ub.Get("SiteBz");
                else
                    bzTypes = ub.Get("SiteBz2");

                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".");
                builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "</a>获得" + n.WinCent + "" + bzTypes + "");

                k++;
                builder.Append(Out.Tab("</div>", ""));
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

        }
        else
        {
            builder.Append(Out.Div("div", "没有中奖记录.."));
        }
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("horse.aspx?act=mylist&amp;ptype=1") + "\">未开投注</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("horse.aspx?act=mylist&amp;ptype=2") + "\">历史投注</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append(" <a href=\"" + Utils.getUrl("horse.aspx") + "\">跑马</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void RaceViewPage()
    {
        int id = Utils.ParseInt(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.Game.Horselist model = new BCW.BLL.Game.Horselist().GetHorselist(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.State == 0)
        {
            Utils.Error("本期还没有开奖", "");
        }
        Master.Title = "第" + id + "局跑马现场重温";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getPage("horse.aspx?act=list") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));

        builder.Append("本局开[" + Utils.ConvertSeparated(model.WinNum.ToString(), 1, "-") + "]x" + ForOdds(model.Odds, Utils.ConvertSeparated(model.WinNum.ToString(), 1, "-")) + "共<a href=\"" + Utils.getUrl("horse.aspx?act=listview&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.WinCount + "</a>人中奖");

        builder.Append("<br />=========终<br />");
        builder.Append(new BCW.User.Game.Horse().OutHr(ForOdds(model.WinData, "1|"), 16) + "1<br />");
        builder.Append(new BCW.User.Game.Horse().OutHr(ForOdds(model.WinData, "2|"), 16) + "2<br />");
        builder.Append(new BCW.User.Game.Horse().OutHr(ForOdds(model.WinData, "3|"), 16) + "3<br />");
        builder.Append(new BCW.User.Game.Horse().OutHr(ForOdds(model.WinData, "4|"), 16) + "4<br />");
        builder.Append(new BCW.User.Game.Horse().OutHr(ForOdds(model.WinData, "5|"), 16) + "5<br />");
        builder.Append(new BCW.User.Game.Horse().OutHr(ForOdds(model.WinData, "6|"), 16) + "6<br />");
        builder.Append("=========点");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("horse.aspx?act=mylist&amp;ptype=1") + "\">未开押注</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("horse.aspx?act=mylist&amp;ptype=2") + "\">历史押注</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append(" <a href=\"" + Utils.getUrl("horse.aspx") + "\">跑马</a>");
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
            strTitle = "我的未开押注";
        else
            strTitle = "我的历史押注";

        Master.Title = strTitle;

        builder.Append(Out.Tab("<div class=\"title\">", ""));
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

        string Horseqi = "";
        // 开始读取列表
        IList<BCW.Model.Game.Horsepay> listHorsepay = new BCW.BLL.Game.Horsepay().GetHorsepays(pageIndex, pageSize, strWhere, out recordCount);
        if (listHorsepay.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Horsepay n in listHorsepay)
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

                if (n.HorseId.ToString() != Horseqi)
                {
                    builder.Append("第" + n.HorseId + "局");
                    if (ptype == 2)
                    {
                        DataSet ds = new BCW.BLL.Game.Horselist().GetList("WinNum,Odds", "ID=" + n.HorseId + "");
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            builder.Append("跑出:" + Utils.ConvertSeparated(ds.Tables[0].Rows[0]["WinNum"].ToString(), 1, "-") + "");
                            builder.Append("赔" + ForOdds(ds.Tables[0].Rows[0]["Odds"].ToString(), Utils.ConvertSeparated(ds.Tables[0].Rows[0]["WinNum"].ToString(), 1, "-")) + "倍");
                        }

                    }
                    builder.Append("<br />");
                }

                string sText = "押[" + Utils.ConvertSeparated(n.Types.ToString(), 1, "-") + "]";
                string bzTypes = string.Empty;
                if (n.bzType == 0)
                    bzTypes = ub.Get("SiteBz");
                else
                    bzTypes = ub.Get("SiteBz2");

                builder.Append("[" + ((pageIndex - 1) * pageSize + k) + "].");
                if (n.State == 0)
                    builder.Append("" + sText + "，共" + n.BuyCent + "" + bzTypes + "[" + DT.FormatDate(n.AddTime, 13) + "]");
                else if (n.State == 1)
                {
                    builder.Append("" + sText + "，共" + n.BuyCent + "" + bzTypes + "[" + DT.FormatDate(n.AddTime, 1) + "]");
                    if (n.WinCent > 0)
                    {
                        builder.Append("赢" + n.WinCent + "" + bzTypes + "");
                    }
                }
                else
                    builder.Append("" + sText + "，共" + n.BuyCent + "" + bzTypes + "，赢" + n.WinCent + "" + bzTypes + "[" + DT.FormatDate(n.AddTime, 1) + "]");

                Horseqi = n.HorseId.ToString();
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
        builder.Append("<a href=\"" + Utils.getUrl("horse.aspx?act=mylist&amp;ptype=1") + "\">未开押注</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("horse.aspx?act=mylist&amp;ptype=2") + "\">历史押注</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append(" <a href=\"" + Utils.getUrl("horse.aspx") + "\">跑马</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void CaseOkPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int pid = Utils.ParseInt(Utils.GetRequest("pid", "get", 2, @"^[0-9]\d*$", "选择兑奖无效"));

        if (new BCW.BLL.Game.Horsepay().ExistsState(pid, meid))
        {
            new BCW.BLL.Game.Horsepay().UpdateState(pid);
            //操作币
            long winMoney = Convert.ToInt64(new BCW.BLL.Game.Horsepay().GetWinCent(pid));
            int bzType = new BCW.BLL.Game.Horsepay().GetbzType(pid);
            //税率
            long SysTax = 0;
            int Tax = Utils.ParseInt(ub.GetSub("HorseTax", xmlPath));
            if (Tax > 0)
            {
                SysTax = Convert.ToInt64(winMoney * Tax * 0.01);
            }
            winMoney = winMoney - SysTax;
            if (bzType == 0)
            {
                new BCW.BLL.User().UpdateiGold(meid, winMoney, "跑马兑奖-标识ID" + pid + "");
                Utils.Success("兑奖", "恭喜，成功兑奖" + winMoney + "" + ub.Get("SiteBz") + "", Utils.getUrl("horse.aspx?act=case"), "1");
            }
            else
            {
                new BCW.BLL.User().UpdateiMoney(meid, winMoney, "跑马兑奖-标识ID" + pid + "");
                Utils.Success("兑奖", "恭喜，成功兑奖" + winMoney + "" + ub.Get("SiteBz2") + "", Utils.getUrl("horse.aspx?act=case"), "1");
            }

        }
        else
        {
            Utils.Success("兑奖", "恭喜，重复兑奖或没有可以兑奖的记录", Utils.getUrl("horse.aspx?act=case"), "1");
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
        string[] strArrId = arrId.Split(",".ToCharArray());
        long winMoney = 0;
        long winMoney2 = 0;
        for (int i = 0; i < strArrId.Length; i++)
        {
            int pid = Convert.ToInt32(strArrId[i]);
            if (new BCW.BLL.Game.Horsepay().ExistsState(pid, meid))
            {
                new BCW.BLL.Game.Horsepay().UpdateState(pid);
                //操作币
                long win = Convert.ToInt64(new BCW.BLL.Game.Horsepay().GetWinCent(pid));
                int bzType = new BCW.BLL.Game.Horsepay().GetbzType(pid);
                //税率
                long SysTax = 0;
                int Tax = Utils.ParseInt(ub.GetSub("HorseTax", xmlPath));
                if (Tax > 0)
                {
                    SysTax = Convert.ToInt64(winMoney * Tax * 0.01);
                }
                if (bzType == 0)
                {
                    winMoney += win - SysTax;
                    new BCW.BLL.User().UpdateiGold(meid, win, "跑马兑奖-标识ID" + pid + "");
                }
                else
                {
                    winMoney2 += win - SysTax;
                    new BCW.BLL.User().UpdateiMoney(meid, win, "跑马兑奖-标识ID" + pid + "");
                }
            }
        }
        Utils.Success("兑奖", "恭喜，成功兑奖" + winMoney + "" + ub.Get("SiteBz") + "/" + winMoney2 + "" + ub.Get("SiteBz2") + "", Utils.getUrl("horse.aspx?act=case"), "1");
    }


    private void CasePage()
    {
        Master.Title = "兑奖中心";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        builder.Append(Out.Tab("<div class=\"title\">", ""));
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
        string Horseqi = "";
        // 开始读取列表
        IList<BCW.Model.Game.Horsepay> listHorsepay = new BCW.BLL.Game.Horsepay().GetHorsepays(pageIndex, pageSize, strWhere, out recordCount);
        if (listHorsepay.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Horsepay n in listHorsepay)
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

                if (n.HorseId.ToString() != Horseqi)
                {
                    builder.Append("第" + n.HorseId + "局");
                    DataSet ds = new BCW.BLL.Game.Horselist().GetList("WinNum,Odds", "ID=" + n.HorseId + "");
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        builder.Append("跑出:[" + Utils.ConvertSeparated(ds.Tables[0].Rows[0]["WinNum"].ToString(), 1, "-") + "]");
                        builder.Append("赔" + ForOdds(ds.Tables[0].Rows[0]["Odds"].ToString(), Utils.ConvertSeparated(ds.Tables[0].Rows[0]["WinNum"].ToString(), 1, "-")) + "倍<br />");
                    }
                }

                string bzTypes = string.Empty;
                if (n.bzType == 0)
                    bzTypes = ub.Get("SiteBz");
                else
                    bzTypes = ub.Get("SiteBz2");

                builder.Append("[" + ((pageIndex - 1) * pageSize + k) + "].");
                string sText = "押[" + Utils.ConvertSeparated(n.Types.ToString(), 1, "-") + "]";
                builder.Append(sText + "，共" + n.BuyCent + "" + ub.Get("SiteBz") + "赢" + n.WinCent + "" + bzTypes + "[" + DT.FormatDate(n.AddTime, 1) + "]<a href=\"" + Utils.getUrl("horse.aspx?act=caseok&amp;pid=" + n.ID + "") + "\">兑奖</a>");
                Horseqi = n.HorseId.ToString();
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
            string strOthe = "本页兑奖,horse.aspx,post,0,red";
            builder.Append(Out.wapform(strName, strValu, strOthe));
        }
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("horse.aspx?act=mylist&amp;ptype=1") + "\">未开投注</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("horse.aspx?act=mylist&amp;ptype=2") + "\">历史投注</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append(" <a href=\"" + Utils.getUrl("horse.aspx") + "\">跑马</a>");
        builder.Append(Out.Tab("</div>", ""));
    }


    private void HelpPage()
    {
        Master.Title = "跑马游戏规则";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("跑马游戏规则");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("1.每期共有6匹马进行比赛；");
        builder.Append("<br />2.竞猜比赛胜出的前2匹马，如你买2号和5号马，那么就买2-5；");
        builder.Append("<br />3.每期" + ub.GetSub("HorseCycleMin", xmlPath) + "分钟一个周期；");
        builder.Append("<br />4.跑马等待时间为15秒，比赛时间15秒，结果显示15秒；");
        builder.Append("<br />5.中奖后点击[兑奖]领奖。");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("horse.aspx") + "\">跑马</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void TopPage()
    {
        string strTitle = "跑马排行榜";
        Master.Title = strTitle;

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(strTitle);
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        strWhere += "bzType=0 and State>0";

        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Game.Horsepay> listHorsepay = new BCW.BLL.Game.Horsepay().GetHorsepaysTop(pageIndex, pageSize, strWhere, out recordCount);
        if (listHorsepay.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Horsepay n in listHorsepay)
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
                builder.Append("[第" + ((pageIndex - 1) * pageSize + k) + "名]<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(n.UsID) + "</a>赢" + n.WinCent + "" + ub.Get("SiteBz") + "");
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
        builder.Append("<a href=\"" + Utils.getUrl("horse.aspx?act=mylist&amp;ptype=1") + "\">未开投注</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("horse.aspx?act=mylist&amp;ptype=2") + "\">历史投注</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append(" <a href=\"" + Utils.getUrl("horse.aspx") + "\">跑马</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    /// <summary>
    /// 得到赔率
    /// </summary>
    private int ForOdds(string Odds, string sType)
    {
        if (Odds == "" || sType == "")
            return 0;

        string[] temp = Odds.Split(",".ToCharArray());
        for (int i = 0; i < temp.Length; i++)
        {
            if (temp[i].ToString().Contains(sType))
            {
                string[] temp1 = temp[i].Split("|".ToCharArray());
                return Convert.ToInt32(temp1[1]);
            }
        }
        return 0;
    }

    /// <summary>
    /// 得到组号
    /// </summary>
    private string ForType(string Odds, string iOdds)
    {
        if (Odds == "" || iOdds == "")
            return "1-2";

        string[] temp = Odds.Split(",".ToCharArray());
        for (int i = 0; i < temp.Length; i++)
        {
            if ((temp[i].ToString() + ",").Contains("|" + iOdds + ","))
            {
                string[] temp1 = temp[i].Split("|".ToCharArray());
                return temp1[0];
            }
        }
        return "1-2";
    }

    //开放时间计算
    private bool IsOpen()
    {
        bool IsOpen = true;
        string OnTime = ub.GetSub("HorseOnTime", xmlPath);
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

    private bool Isbz()
    {

        return false;
    }
}
