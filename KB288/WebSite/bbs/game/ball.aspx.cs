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

/// <summary>
/// 蒙宗将 20160513 抽奖值生成
/// 
///  蒙宗将 20160822 撤掉抽奖值生成
/// </summary>

public partial class bbs_game_ball : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/ball.xml";

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
        if (ub.GetSub("BallStatus", xmlPath) == "1")
        {
            Utils.Safe("此游戏");
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
            case "payok":
                PayOkPage();
                break;
            case "mylist":
                MyListPage();
                break;
            case "top":
                TopPage();
                break;
            case "list":
                ListPage();
                break;
            case "listview":
                ListViewPage();
                break;
            case "help":
                HelpPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        Master.Title = ub.GetSub("BallName", xmlPath);
        int meid = new BCW.User.Users().GetUsId();
        string Logo = ub.GetSub("BallLogo", xmlPath);
        if (Logo != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<img src=\"" + Logo + "\" alt=\"load\"/>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/game/default.aspx") + "\">游戏大厅</a>&gt;彩球");
        builder.Append(Out.Tab("</div>", "<br />"));

        string Notes = ub.GetSub("BallNotes", xmlPath);
        if (Notes != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.SysUBB(Notes) + "");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        if (IsOpen() == true)
        {
            new BCW.User.Ball().BallPage(0, 0);
            BCW.Model.Game.Balllist ball = null;
            ball = new BCW.BLL.Game.Balllist().GetBalllist();
            if (ball.ID == 0)
            {
                //第一期开始
                ball.WinNum = 0;
                ball.OutNum = Utils.ParseInt(ub.GetSub("BallOutNum", xmlPath));
                ball.iCent = Utils.ParseInt(ub.GetSub("BalliCent", xmlPath));
                ball.AddNum = 0;
                ball.Odds = Utils.ParseInt(ub.GetSub("BallOdds", xmlPath));
                //系统投入币
                int SysCent = Utils.ParseInt(ub.GetSub("BallSysPay", xmlPath));
                //开奖周期分钟
                int CycleMin = Utils.ParseInt(ub.GetSub("BallCycleMin", xmlPath));
                ball.Pool = Convert.ToInt64(SysCent);
                ball.BeforePool = Convert.ToInt64(SysCent);
                ball.BeginTime = DateTime.Now;
                ball.EndTime = DateTime.Now.AddMinutes(Convert.ToDouble(CycleMin));
                ball.ID = new BCW.BLL.Game.Balllist().Add(ball);
            }
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("欢迎进入疯狂彩球第" + ball.ID + "期");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            if (ball.EndTime < DateTime.Now)
                builder.Append("系统正在开奖。。。");
            else
                builder.Append("距离开奖还有" + DT.DateDiff(DateTime.Now, ball.EndTime) + "");

            builder.Append("<br />每份下注:" + Utils.ConvertGold(Convert.ToInt64(ball.iCent)) + "" + ub.Get("SiteBz") + "/赔率1:" + ball.Odds + "");
            builder.Append("<br />奖池:" + Utils.ConvertGold(ball.Pool) + "" + ub.Get("SiteBz") + "<br />");
            if (ball.BeforePool > 0)
            {
                builder.Append("上期落下奖池:" + Utils.ConvertGold(ball.BeforePool) + "" + ub.Get("SiteBz") + "<br />");
            }
            builder.Append("<a href=\"" + Utils.getUrl("ball.aspx?act=pay") + "\">立即投注</a> ");
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("欢迎进入疯狂彩球游戏");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("游戏开放时间:" + ub.GetSub("BallOnTime", xmlPath) + "");
            builder.Append("<br />目前还没到开放时间哦!");
            builder.Append("<br /><a href=\"" + Utils.getUrl("ball.aspx?act=list&amp;backurl=" + Utils.PostPage(1) + "") + "\">历史开奖</a> ");
        }
        builder.Append("<a href=\"" + Utils.getUrl("ball.aspx?act=case") + "\">兑奖</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("ball.aspx") + "\">刷新</a><br />");
        builder.Append("你目前自带" + Utils.ConvertGold(new BCW.BLL.User().GetGold(meid)) + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("ball.aspx?act=mylist&amp;ptype=1") + "\">未开投注</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("ball.aspx?act=mylist&amp;ptype=2") + "\">历史投注</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("ball.aspx?act=top") + "\">排行榜单</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("ball.aspx?act=help") + "\">游戏帮助</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("【往期开奖记录】");
        builder.Append(Out.Tab("</div>", "<br />"));
        IList<BCW.Model.Game.Balllist> listBalllist = new BCW.BLL.Game.Balllist().GetBalllists(3, "State=1");
        if (listBalllist.Count > 0)
        {
            builder.Append(Out.Tab("<div>", ""));
            int k = 1;
            foreach (BCW.Model.Game.Balllist n in listBalllist)
            {

                builder.Append("第" + n.ID + "期开出号码:<a href=\"" + Utils.getUrl("ball.aspx?act=listview&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"><b>" + n.WinNum + "</b></a><br />");

                k++;
            }
            builder.Append("<a href=\"" + Utils.getUrl("ball.aspx?act=list&amp;backurl=" + Utils.PostPage(1) + "") + "\">&gt;&gt;查看历史开奖</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }

        //闲聊显示
        builder.Append(Out.SysUBB(BCW.User.Users.ShowSpeak(8, "ball.aspx", 0, 0)));
        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        builder.Append("【游戏动态记录】");
        builder.Append(Out.Tab("</div>", ""));
        // 开始读取动态列表
        int SizeNum = 3;
        string strWhere = "Types=8";
        IList<BCW.Model.Action> listAction = new BCW.BLL.Action().GetActions(SizeNum, strWhere);
        if (listAction.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Action n in listAction)
            {
                builder.Append(Out.Tab("<div>", "<br />"));
                string ForNotes = Regex.Replace(n.Notes, @"\[url=\/bbs\/game\/([\s\S]+?)\]([\s\S]+?)\[\/url\]", "$2", RegexOptions.IgnoreCase);
                ForNotes = ForNotes.Replace("疯狂彩球", "");
                builder.AppendFormat("{0}前{1}", DT.DateDiff2(DateTime.Now, n.AddTime), Out.SysUBB(ForNotes));
                builder.Append(Out.Tab("</div>", ""));
                k++;
            }
            if (k > SizeNum)
            {
                builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("/bbs/action.aspx?ptype=8&amp;backurl=" + Utils.PostPage(1) + "") + "\">更多游戏动态</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        //游戏底部Ubb
        string Foot = ub.GetSub("BallFoot", xmlPath);
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
        if (IsOpen() == false)
        {
            Utils.Error("游戏开放时间:" + ub.GetSub("BallOnTime", xmlPath) + "", "");
        }

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Master.Title = "我要下注";
        BCW.Model.Game.Balllist ball = new BCW.BLL.Game.Balllist().GetBalllist();
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("疯狂彩球 第" + ball.ID + "期");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        if (ball.EndTime < DateTime.Now)
            builder.Append("系统正在开奖。。。");
        else
            builder.Append("距离开奖还有" + DT.DateDiff(DateTime.Now, ball.EndTime) + "");

        long gold = new BCW.BLL.User().GetGold(meid);
        builder.Append("<br />您目前自带" + Utils.ConvertGold(gold) + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", ""));
        strText = "下注数字(1-" + ub.GetSub("BallSysNum", xmlPath) + "):/,押多少份:/,";
        strName = "BuyNum,BuyCount,act";
        strType = "num,num,hidden";
        strValu = "''payok";
        strEmpt = "false,false,false";
        strIdea = "/每份" + ball.iCent + "" + ub.Get("SiteBz") + ",每人限购" + ub.GetSub("BallOutIDNum", xmlPath) + "份/";
        strOthe = "确定下注,ball.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("ball.aspx") + "\">返回疯狂彩球</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("ball.aspx") + "\">彩球</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void PayOkPage()
    {
        if (IsOpen() == false)
        {
            Utils.Error("游戏开放时间:" + ub.GetSub("BallOnTime", xmlPath) + "", "");
        }

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int BuyNum = int.Parse(Utils.GetRequest("BuyNum", "post", 2, @"^[1-9]$|^[1-4]([0-9])?$", "下注数字限1-" + ub.GetSub("BallSysNum", xmlPath) + ""));
        int BuyCount = int.Parse(Utils.GetRequest("BuyCount", "post", 2, @"^[1-9]\d*$", "下注份数填写错误"));
        if (BuyNum < 1 || BuyNum > Utils.ParseInt(ub.GetSub("BallSysNum", xmlPath)))
        {
            Utils.Error("下注数字限1-" + ub.GetSub("BallSysNum", xmlPath) + "", "");
        }
        //是否刷屏
        string appName = "LIGHT_BALL";
        int Expir = Utils.ParseInt(ub.GetSub("BallExpir", xmlPath));
        BCW.User.Users.IsFresh(appName, Expir);

        BCW.Model.Game.Balllist ball = new BCW.BLL.Game.Balllist().GetBalllist();
        //每期每ID限下注份数
        int BuyCounts = new BCW.BLL.Game.Ballpay().GetBuyCount(ball.ID, meid);
        if (BuyCount + BuyCounts > Utils.ParseInt(ub.GetSub("BallOutIDNum", xmlPath)))
        {
            if (BuyCounts >= Utils.ParseInt(ub.GetSub("BallOutIDNum", xmlPath)))
            {
                Utils.Error("系统限制每期每人下注" + Utils.ParseInt(ub.GetSub("BallOutIDNum", xmlPath)) + "份，欢迎在下期下注", "");
            }
            else
            {
                Utils.Error("系统限制每期每人下注" + Utils.ParseInt(ub.GetSub("BallOutIDNum", xmlPath)) + "份，你现在最多可以下注" + (Utils.ParseInt(ub.GetSub("BallOutIDNum", xmlPath)) - BuyCounts) + "份", "");
            }
        }
        Application.Lock();
        //每期下注限份数
        if ((ball.OutNum - ball.AddNum) < BuyCount)
        {
            if (ball.OutNum == 0)
            {
                Utils.Error("系统每期限购" + ball.OutNum + "份，欢迎在下期下注", "");
            }
            else
            {
                Utils.Error("系统每期限购" + ball.OutNum + "份，您还可以购买" + ((ball.OutNum - ball.AddNum) - BuyCount) + "份", "");
            }
        }
        if (Convert.ToInt64(BuyCount * ball.iCent) > new BCW.BLL.User().GetGold(meid))
        {
            Utils.Error("需花费" + Convert.ToInt64(BuyCount * ball.iCent) + "" + ub.Get("SiteBz") + "，你身上" + ub.Get("SiteBz") + "不足", "");
        }

        //支付安全提示
        string[] p_pageArr = { "act", "BuyNum", "BuyCount" };
        BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr);

        BCW.Model.Game.Ballpay model = new BCW.Model.Game.Ballpay();
        int id = 0;
        string mename = new BCW.BLL.User().GetUsName(meid);
        model.BallId = ball.ID;
        model.UsID = meid;
        model.UsName = mename;
        model.BuyCent = Convert.ToInt64(BuyCount * ball.iCent);
        model.BuyNum = BuyNum;
        model.BuyCount = BuyCount;
        model.State = 0;
        model.WinCent = 0;
        model.AddTime = DateTime.Now;
        if (!new BCW.BLL.Game.Ballpay().ExistsBuyNum(ball.ID, BuyNum, meid))
        {
            id = new BCW.BLL.Game.Ballpay().Add(model);
        }
        else
        {
            new BCW.BLL.Game.Ballpay().Update(model);
        }
        //加奖池基金并减购买份数
        new BCW.BLL.Game.Balllist().UpdatePool(ball.ID, Convert.ToInt64(BuyCount * ball.iCent), BuyCount);

       
        
        //扣币
        new BCW.BLL.User().UpdateiGold(meid, mename, -Convert.ToInt64(BuyCount * ball.iCent), 6);
        string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/ball.aspx]疯狂彩球第" + ball.ID + "期[/url]下注" + Convert.ToInt64(BuyCount * ball.iCent) + "" + ub.Get("SiteBz") + "";
        new BCW.BLL.Action().Add(8, id, 0, "", wText);
        Utils.Success("下注", "下注成功，花费了" + Convert.ToInt64(BuyCount * ball.iCent) + "" + ub.Get("SiteBz") + "<br /><a href=\"" + Utils.getUrl("ball.aspx?act=pay") + "\">&gt;继续下注</a>", Utils.getUrl("ball.aspx"), "2");
        Application.UnLock();
    }

    private void MyListPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[1-2]$", "1"));
        string strTitle = "";
        if (ptype == 1)
            strTitle = "我的未开投注";
        else
            strTitle = "我的历史投注";

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

        string Ballqi = "";
        // 开始读取列表
        IList<BCW.Model.Game.Ballpay> listBallpay = new BCW.BLL.Game.Ballpay().GetBallpays(pageIndex, pageSize, strWhere, out recordCount);
        if (listBallpay.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Ballpay n in listBallpay)
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

                if (n.BallId.ToString() != Ballqi)
                    builder.Append("=第" + n.BallId + "期=<br />");

                builder.Append("[" + ((pageIndex - 1) * pageSize + k) + "].");
                if (n.State == 0)
                    builder.Append("买" + n.BuyNum + "，共押"+n.BuyCount+"份/花费" + n.BuyCent + "" + ub.Get("SiteBz") + "[" + DT.FormatDate(n.AddTime, 13) + "]");
                else if (n.State == 1)
                {
                    builder.Append("买" + n.BuyNum + "，共押" + n.BuyCount + "份/花费" + n.BuyCent + "" + ub.Get("SiteBz") + "[" + DT.FormatDate(n.AddTime, 1) + "]");
                    if (n.WinCent > 0)
                    {
                        builder.Append("赢" + n.WinCent + "" + ub.Get("SiteBz") + "");
                    }
                }
                else
                    builder.Append("买" + n.BuyNum + "，共押" + n.BuyCount + "份/花费" + n.BuyCent + "" + ub.Get("SiteBz") + "，赢" + n.WinCent + "" + ub.Get("SiteBz") + "[" + DT.FormatDate(n.AddTime, 1) + "]");

                Ballqi = n.BallId.ToString();
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
        builder.Append("<a href=\"" + Utils.getUrl("ball.aspx?act=mylist&amp;ptype=1") + "\">未开投注</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("ball.aspx?act=mylist&amp;ptype=2") + "\">历史投注</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("ball.aspx") + "\">彩球</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    /// <summary>
    /// 排行榜
    /// </summary>
    private void TopPage()
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));
        Master.Title = "疯狂彩球排行榜";
        builder.Append(Out.Tab("<div class=\"title\">", ""));

        if (ptype == 1)
            builder.Append("赚币排行|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("ball.aspx?act=top&amp;ptype=1") + "\">赚币排行</a>|");

        if (ptype == 2)
            builder.Append("胜率排行");
        else
            builder.Append("<a href=\"" + Utils.getUrl("ball.aspx?act=top&amp;ptype=2") + "\">胜率排行</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string strOrder = "";
        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "Types=6";
        if (ptype == 1)
            strOrder = "(WinGold+PutGold) Desc";
        else
            strOrder = "(WinNum-PutNum) Desc";

        // 开始读取列表
        IList<BCW.Model.Toplist> listToplist = new BCW.BLL.Toplist().GetToplists(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listToplist.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Toplist n in listToplist)
            {
                    n.UsName = BCW.User.Users.SetVipName(n.UsId, n.UsName, false);
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }
                string sText = string.Empty;
                if (ptype == 1)
                    sText = "净赢" + (n.WinGold + n.PutGold) + "" + ub.Get("SiteBz") + "";
                else
                    sText = "胜" + (n.WinNum - n.PutNum) + "次";

                builder.AppendFormat("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid={0}&amp;backurl=" + Utils.getPage(1) + "") + "\">{1}</a>{2}", n.UsId, n.UsName, sText);
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
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("ball.aspx") + "\">彩球</a>");
        builder.Append(Out.Tab("</div>", ""));
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
        IList<BCW.Model.Game.Balllist> listBalllist = new BCW.BLL.Game.Balllist().GetBalllists(pageIndex, pageSize, strWhere, out recordCount);
        if (listBalllist.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Balllist n in listBalllist)
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

                builder.Append("第" + n.ID + "期开出号码:<a href=\"" + Utils.getUrl("ball.aspx?act=listview&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"><b>" + n.WinNum + "</b></a>");
      
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
        builder.Append("<a href=\"" + Utils.getUrl("ball.aspx?act=mylist&amp;ptype=1") + "\">未开投注</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("ball.aspx?act=mylist&amp;ptype=2") + "\">历史投注</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("ball.aspx") + "\">彩球</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ListViewPage()
    {
        int id = Utils.ParseInt(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.Game.Balllist model = new BCW.BLL.Game.Balllist().GetBalllist(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "第" + id + "期疯狂彩球";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getPage("ball.aspx?act=list") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        strWhere += "BallId=" + id + " and WinCent>0";

        string[] pageValUrl = { "act", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Game.Ballpay> listBallpay = new BCW.BLL.Game.Ballpay().GetBallpays(pageIndex, pageSize, strWhere, out recordCount);
        if (listBallpay.Count > 0)
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("第" + id + "期开出号码:<b>" + model.WinNum + "</b>");
            builder.Append("<br />共" + recordCount + "注中奖");
            builder.Append(Out.Tab("</div>", "<br />"));

            int k = 1;
            foreach (BCW.Model.Game.Ballpay n in listBallpay)
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
                builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "</a>获得" + n.WinCent + "" + ub.Get("SiteBz") + "");

                k++;
                builder.Append(Out.Tab("</div>", ""));
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

        }
        else
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("第" + id + "期开出号码:<b>" + model.WinNum + "</b>");
            builder.Append("<br />共0注中奖");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("ball.aspx?act=mylist&amp;ptype=1") + "\">未开投注</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("ball.aspx?act=mylist&amp;ptype=2") + "\">历史投注</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("ball.aspx") + "\">彩球</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void CaseOkPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int pid = Utils.ParseInt(Utils.GetRequest("pid", "get", 2, @"^[0-9]\d*$", "选择兑奖无效"));

        if (new BCW.BLL.Game.Ballpay().ExistsState(pid, meid))
        {
            new BCW.BLL.Game.Ballpay().UpdateState(pid);
            //操作币
            long winMoney = Convert.ToInt64(new BCW.BLL.Game.Ballpay().GetWinCent(pid));
            //税率
            long SysTax = 0;
            int Tax = Utils.ParseInt(ub.GetSub("BallTax", xmlPath));
            if (Tax > 0)
            {
                SysTax = Convert.ToInt64(winMoney * Tax * 0.01);
            }
            winMoney = winMoney - SysTax;
            new BCW.BLL.User().UpdateiGold(meid, winMoney, "彩球兑奖-标识ID" + pid + "");
            Utils.Success("兑奖", "恭喜，成功兑奖" + winMoney + "" + ub.Get("SiteBz") + "", Utils.getUrl("ball.aspx?act=case"), "1");
        }
        else
        {
            Utils.Success("兑奖", "恭喜，重复兑奖或没有可以兑奖的记录", Utils.getUrl("ball.aspx?act=case"), "1");
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
        long getwinMoney = 0;
        long winMoney = 0;
        for (int i = 0; i < strArrId.Length; i++)
        {
            int pid = Convert.ToInt32(strArrId[i]);
            if (new BCW.BLL.Game.Ballpay().ExistsState(pid, meid))
            {
                new BCW.BLL.Game.Ballpay().UpdateState(pid);
                //操作币
                winMoney = Convert.ToInt64(new BCW.BLL.Game.Ballpay().GetWinCent(pid));
                //税率
                long SysTax = 0;
                int Tax = Utils.ParseInt(ub.GetSub("BallTax", xmlPath));
                if (Tax > 0)
                {
                    SysTax = Convert.ToInt64(winMoney * Tax * 0.01);
                }
                winMoney = winMoney - SysTax;

                new BCW.BLL.User().UpdateiGold(meid, winMoney, "彩球兑奖-标识ID" + pid + "");
            }
            getwinMoney = getwinMoney + winMoney;
        }
        Utils.Success("兑奖", "恭喜，成功兑奖" + getwinMoney + "" + ub.Get("SiteBz") + "", Utils.getUrl("ball.aspx?act=case"), "1");
    }


    private void CasePage()
    {
        Master.Title = "兑奖中心";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        builder.Append(Out.Tab("<div class=\"title\">", ""));
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
        // 开始读取列表
        IList<BCW.Model.Game.Ballpay> listBallpay = new BCW.BLL.Game.Ballpay().GetBallpays(pageIndex, pageSize, strWhere, out recordCount);
        if (listBallpay.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Ballpay n in listBallpay)
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
                builder.Append("[第" + n.BallId + "期].");
                builder.Append("买" + n.BuyNum + "，押" + n.BuyCount + "份/花费" + n.BuyCent + "" + ub.Get("SiteBz") + "赢" + n.WinCent + "" + ub.Get("SiteBz") + "[" + DT.FormatDate(n.AddTime, 1) + "]<a href=\"" + Utils.getUrl("ball.aspx?act=caseok&amp;pid=" + n.ID + "") + "\">兑奖</a>");

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
            string strOthe = "本页兑奖,ball.aspx,post,0,red";
            builder.Append(Out.wapform(strName, strValu, strOthe));
        }
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("ball.aspx?act=mylist&amp;ptype=1") + "\">未开投注</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("ball.aspx?act=mylist&amp;ptype=2") + "\">历史投注</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("ball.aspx") + "\">彩球</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    /// <summary>
    /// 游戏玩法规则
    /// </summary>
    private void HelpPage()
    {
        Master.Title = "疯狂彩球玩法规则";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("=玩法规则=");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append(Out.SysUBB(ub.GetSub("BallRule", xmlPath)));
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("ball.aspx") + "\">彩球</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    //开放时间计算
    private bool IsOpen()
    {
        bool IsOpen = true;
        string OnTime = ub.GetSub("BallOnTime", xmlPath);
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
}
