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

public partial class bbs_game_brag : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/brag.xml";
    protected string xmlPath2 = "/Controls/gamezdid.xml";
    protected long F1 = Convert.ToInt64(ub.GetSub("BragF1", "/Controls/brag.xml"));
    protected long F2 = Convert.ToInt64(ub.GetSub("BragF2", "/Controls/brag.xml"));
    protected long F3 = Convert.ToInt64(ub.GetSub("BragF3", "/Controls/brag.xml"));
    protected long F4 = Convert.ToInt64(ub.GetSub("BragF4", "/Controls/brag.xml"));
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
        if (ub.GetSub("BragStatus", xmlPath) == "1")
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
            case "mylist":
                MyListPage();
                break;
            case "cylist":
                CyListPage();
                break;
            case "dzlist":
                DzListPage();
                break;
            case "list":
                ListPage();
                break;
            case "del":
                DelPage();
                break;
            case "no":
                NoPage();
                break;
            case "add":
                AddPage();
                break;
            case "addsave":
                AddSavePage();
                break;
            case "view":
                ViewPage();
                break;
            case "info":
                InfoPage();
                break;
            case "rule":
                RulePage();
                break;
            case "top":
                TopPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        Master.Title = ub.GetSub("BragName", xmlPath);
        string Logo = ub.GetSub("BragLogo", xmlPath);
        if (Logo != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<img src=\"" + Logo + "\" alt=\"load\"/>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        //拾物随机
        builder.Append(BCW.User.Game.GiftFlows.ShowGiftFlows(9));

        string Notes = ub.GetSub("BragNotes", xmlPath);
        if (Notes != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.SysUBB(Notes) + "");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;吹牛");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("【疯狂吹牛】<a href=\"" + Utils.getUrl("brag.aspx?act=rule") + "\">规则</a>.<a href=\"" + Utils.getUrl("brag.aspx?act=case") + "\">兑奖</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("brag.aspx?act=add") + "\">我要吹牛</a>.<a href=\"" + Utils.getUrl("brag.aspx?act=mylist") + "\">历史</a>|<a href=\"" + Utils.getUrl("brag.aspx?act=cylist") + "\">吹牛记录</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/friend.aspx?act=online&amp;backurl=" + Server.UrlEncode("/bbs/game/brag.aspx?act=add") + "") + "\">找人对战</a>.<a href=\"" + Utils.getUrl("brag.aspx?act=dzlist") + "\">对战史</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/action.aspx?ptype=14&amp;backurl=" + Utils.PostPage(1) + "") + "\">动态</a><br />");
        builder.Append("今日吹牛总数:" + new BCW.BLL.Game.Brag().GetCount() + "个<br />");
        builder.Append("今日吹牛总量:" + new BCW.BLL.Game.Brag().GetPrice(0) + "" + ub.Get("SiteBz") + "");
        if (Isbz())
            builder.Append("/" + new BCW.BLL.Game.Brag().GetPrice(1) + "" + ub.Get("SiteBz2") + "");

        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("【最新吹牛】<a href=\"" + Utils.getUrl("brag.aspx") + "\">刷新</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        // 开始读取吹牛列表
        int SizeNum = 3;
        string strWhere = "Types=0 and state=0 and StopTime>='" + DateTime.Now + "'";
        IList<BCW.Model.Game.Brag> listBrag = new BCW.BLL.Game.Brag().GetBrags(SizeNum, strWhere);
        if (listBrag.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Brag n in listBrag)
            {
                if (k == 1)
                    builder.Append(Out.Tab("<div>", ""));
                else
                    builder.Append(Out.Tab("<div>", "<br />"));

                string bzText = string.Empty;
                if (n.BzType == 0)
                    bzText = ub.Get("SiteBz");
                else
                    bzText = ub.Get("SiteBz2");

                builder.AppendFormat("[" + CaseBrag(n.Price, n.BzType) + "]<a href=\"" + Utils.getUrl("brag.aspx?act=view&amp;id={0}") + "\">{1}({2}{3})</a>", n.ID, n.Title, n.Price, bzText);
                builder.Append(Out.Tab("</div>", ""));
                k++;
            }

        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("brag.aspx?act=list") + "\">&gt;&gt;更多</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("brag.aspx?act=list&amp;ptype=3") + "\">+已应答的吹牛&gt;&gt;</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("【水牛推荐】");
        builder.Append(Out.Tab("</div>", "<br />"));

        // 开始读取吹牛列表
        SizeNum = 3;
        strWhere = "Types=0 and state=0 and StopTime>='" + DateTime.Now + "' and ((Price>=" + F1 + " and Price<" + F2 + " and BzType=0) or (Price>=" + F3 + " and Price<" + F4 + " and BzType=1))";
        listBrag = new BCW.BLL.Game.Brag().GetBrags(SizeNum, strWhere);
        if (listBrag.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Brag n in listBrag)
            {
                if (k == 1)
                    builder.Append(Out.Tab("<div>", ""));
                else
                    builder.Append(Out.Tab("<div>", "<br />"));

                string bzText = string.Empty;
                if (n.BzType == 0)
                    bzText = ub.Get("SiteBz");
                else
                    bzText = ub.Get("SiteBz2");

                builder.AppendFormat("<a href=\"" + Utils.getUrl("brag.aspx?act=view&amp;id={0}") + "\">{1}({2}{3})</a>", n.ID, n.Title, n.Price, bzText);
                builder.Append(Out.Tab("</div>", ""));
                k++;
            }

            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("brag.aspx?act=list&amp;ptype=1") + "\">&gt;&gt;更多水牛</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }

        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("【犀牛待宰】");
        builder.Append(Out.Tab("</div>", "<br />"));
        SizeNum = 3;
        strWhere = "Types=0 and state=0 and StopTime>='" + DateTime.Now + "' and ((Price>=" + F2 + " and BzType=0) or (Price>=" + F4 + " and BzType=1))";
        listBrag = new BCW.BLL.Game.Brag().GetBrags(SizeNum, strWhere);
        if (listBrag.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Brag n in listBrag)
            {
                if (k == 1)
                    builder.Append(Out.Tab("<div>", ""));
                else
                    builder.Append(Out.Tab("<div>", "<br />"));

                string bzText = string.Empty;
                if (n.BzType == 0)
                    bzText = ub.Get("SiteBz");
                else
                    bzText = ub.Get("SiteBz2");

                builder.AppendFormat("<a href=\"" + Utils.getUrl("brag.aspx?act=view&amp;id={0}") + "\">{1}({2}{3})</a>", n.ID, n.Title, n.Price, bzText);
                builder.Append(Out.Tab("</div>", ""));
                k++;
            }

            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("brag.aspx?act=list&amp;ptype=2") + "\">&gt;&gt;更多犀牛</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }

        //builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        //builder.Append("〓最新动态〓");
        //builder.Append(Out.Tab("</div>", ""));
        //// 开始读取动态列表
        //SizeNum = 3;
        //strWhere = "Types=14";
        //IList<BCW.Model.Action> listAction = new BCW.BLL.Action().GetActions(SizeNum, strWhere);
        //if (listAction.Count > 0)
        //{
        //    int k = 1;
        //    foreach (BCW.Model.Action n in listAction)
        //    {
        //        builder.Append(Out.Tab("<div>", "<br />"));
        //        string ForNotes = Regex.Replace(n.Notes, @"\[url=\/bbs\/game\/([\s\S]+?)\]([\s\S]+?)\[\/url\]", "$2", RegexOptions.IgnoreCase);
        //        ForNotes = ForNotes.Replace("在疯狂吹牛", "");
        //        builder.AppendFormat("{0}前{1}", DT.DateDiff2(DateTime.Now, n.AddTime), Out.SysUBB(ForNotes));
        //        builder.Append(Out.Tab("</div>", ""));
        //        k++;
        //    }
        //    if (k > SizeNum)
        //    {
        //        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        //        builder.Append("<a href=\"" + Utils.getUrl("/bbs/action.aspx?ptype=14&amp;backurl=" + Utils.PostPage(1) + "") + "\">更多游戏动态&gt;&gt;</a>");
        //        builder.Append(Out.Tab("</div>", ""));
        //    }
        //}


        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("★排行榜:<a href=\"" + Utils.getUrl("brag.aspx?act=top&amp;ptype=1") + "\">赚币</a>|<a href=\"" + Utils.getUrl("brag.aspx?act=top&amp;ptype=2") + "\">胜率</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));

        DataSet ds = new BCW.BLL.Toplist().GetList("Top 1 UsID,WinGold,PutGold", "Types=7 Order by ((-PutGold)+WinGold) Desc");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            builder.Append("№牛王：<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + ds.Tables[0].Rows[0]["UsID"] + "") + "\">" + BCW.User.Users.SetUser(int.Parse(ds.Tables[0].Rows[0]["UsID"].ToString())) + "</a>参与" + (Convert.ToInt64(ds.Tables[0].Rows[0]["WinGold"]) + (-Convert.ToInt64(ds.Tables[0].Rows[0]["PutGold"]))) + "" + ub.Get("SiteBz") + "<br />");
        }
        else
            builder.Append("№牛王：暂无记录<br />");

        ds = new BCW.BLL.Toplist().GetList("Top 1 UsID,WinGold,PutGold", "Types=7 Order by (WinGold+PutGold) Desc");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            builder.Append("№牛总：<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + ds.Tables[0].Rows[0]["UsID"] + "") + "\">" + BCW.User.Users.SetUser(int.Parse(ds.Tables[0].Rows[0]["UsID"].ToString())) + "</a>净赚" + (Convert.ToInt64(ds.Tables[0].Rows[0]["WinGold"]) + Convert.ToInt64(ds.Tables[0].Rows[0]["PutGold"])) + "" + ub.Get("SiteBz") + "<br />");
        }
        else
            builder.Append("№牛总：暂无记录<br />");


        ds = new BCW.BLL.Toplist().GetList("Top 1 UsID,WinNum,PutNum", "Types=7 Order by (WinNum-PutNum) Desc");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            builder.Append("№牛胜：<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + ds.Tables[0].Rows[0]["UsID"] + "") + "\">" + BCW.User.Users.SetUser(int.Parse(ds.Tables[0].Rows[0]["UsID"].ToString())) + "</a>净胜" + (Convert.ToInt64(ds.Tables[0].Rows[0]["WinNum"]) - Convert.ToInt64(ds.Tables[0].Rows[0]["PutNum"])) + "场");
        }
        else
            builder.Append("№牛胜：暂无记录");

        builder.Append(Out.Tab("</div>", ""));


        //游戏底部Ubb
        string Foot = ub.GetSub("BragFoot", xmlPath);
        if (Foot != "")
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(Foot)));
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("", "<br />"));
        strName = "purl,act,backurl";
        strValu = "'recommend'" + Utils.PostPage(1) + "";
        strOthe = "&gt;分享给好友,/bbs/guest.aspx,post,1,other";
        builder.Append(Out.wapform(strName, strValu, strOthe));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void AddPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int hid = int.Parse(Utils.GetRequest("hid", "all", 1, @"^[0-9]\d*$", "0"));
        Master.Title = "我要吹牛";
        //拾物随机
        builder.Append(BCW.User.Game.GiftFlows.ShowGiftFlows(9));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>&gt;<a href=\"" + Utils.getUrl("brag.aspx") + "\">吹牛</a>&gt;我要吹牛");
        builder.Append(Out.Tab("</div>", ""));

        if (hid > 0)
        {
            string UsName = new BCW.BLL.User().GetUsName(hid);
            if (UsName == "")
            {
                Utils.Error("不存在的会员", "");
            }
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("我邀请<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + hid + "") + "\">" + UsName + "(" + hid + ")</a>参与对战");
            builder.Append(Out.Tab("</div>", ""));
        }
        strText = "输入吹牛主题(15字内):/,有效时限:,选择答案1:/,选择答案2:/,正确答案为:/,押注金额:/,,";
        strName = "Title,iTime,BragA,BragB,TrueType,Price,hid,act";
        strType = "text,select,text,text,select,num,hidden,hidden";
        strValu = "我比你帅多了'1'信'不信'1'" + ub.GetSub("BragSmallPay", xmlPath) + "'" + hid + "'addsave";
        strEmpt = "false,1|1小时|2|2小时|4|4小时|6|6小时|12|12小时|24|1天|48|2天,false,false,1|答案1|2|答案2,false,false,false";
        strIdea = "/";
        if (Isbz())
            strOthe = "押" + ub.Get("SiteBz") + "|押" + ub.Get("SiteBz2") + ",brag.aspx,post,0,red|blue";
        else
            strOthe = "押" + ub.Get("SiteBz") + ",brag.aspx,post,0,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("您目前自带" + Utils.ConvertGold(new BCW.BLL.User().GetGold(meid)) + "" + ub.Get("SiteBz") + "");
        if (Isbz())
            builder.Append("/" + Utils.ConvertGold(new BCW.BLL.User().GetMoney(meid)) + "" + ub.Get("SiteBz2") + "");

        builder.Append("<br /><a href=\"" + Utils.getPage("brag.aspx") + "\">再考虑一下&gt;&gt;</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("brag.aspx") + "\">吹牛</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void AddSavePage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string ac = Utils.GetRequest("ac", "post", 1, "", "");
        string Title = Utils.GetRequest("Title", "post", 2, @"^[\s\S]{1,15}$", "吹牛主题请保持在15字内");
        int iTime = int.Parse(Utils.GetRequest("iTime", "post", 2, @"^[1-9]\d*$", "有效时限选择错误"));
        int hid = int.Parse(Utils.GetRequest("hid", "post", 1, @"^[0-9]\d*$", "0"));
        string UsName = string.Empty;
        if (hid > 0)
        {
            UsName = new BCW.BLL.User().GetUsName(hid);
            if (UsName == "")
            {
                Utils.Error("邀请的会员不存在", "");
            }
        }
        if (iTime <= 0 && iTime > 48)
            Utils.Error("有效时限选择错误", "");

        string BragA = Utils.GetRequest("BragA", "post", 2, @"^[\s\S]{1,10}$", "选择答案1限1-10字");
        string BragB = Utils.GetRequest("BragB", "post", 2, @"^[\s\S]{1,10}$", "选择答案2限1-10字");
        int TrueType = int.Parse(Utils.GetRequest("TrueType", "post", 2, @"^[1-2]$", "正确答案选择错误"));
        long Price = Int64.Parse(Utils.GetRequest("Price", "post", 4, @"^[0-9]\d*$", "押注金额错误"));

        int bzType = 0;
        string bzText = string.Empty;
        long gold = 0;
        if (Utils.ToSChinese(ac).Contains(Utils.ToSChinese(ub.Get("SiteBz2"))))
        {
            bzType = 1;
            bzText = ub.Get("SiteBz2");
            gold = new BCW.BLL.User().GetMoney(meid);
            if (Price < Convert.ToInt64(ub.GetSub("BragSmallPay2", xmlPath)) || Price > Convert.ToInt64(ub.GetSub("BragBigPay2", xmlPath)))
            {
                Utils.Error("押注金额限" + ub.GetSub("BragSmallPay2", xmlPath) + "-" + ub.GetSub("BragBigPay2", xmlPath) + "" + bzText + "", "");
            }
        }
        else
        {
            //支付安全提示
            string[] p_pageArr = { "act", "hid", "Title", "iTime", "BragA", "BragB", "TrueType", "Price", "ac" };
            BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr);

            bzType = 0;
            bzText = ub.Get("SiteBz");
            gold = new BCW.BLL.User().GetGold(meid);
            if (Price < Convert.ToInt64(ub.GetSub("BragSmallPay", xmlPath)) || Price > Convert.ToInt64(ub.GetSub("BragBigPay", xmlPath)))
            {
                Utils.Error("押注金额限" + ub.GetSub("BragSmallPay", xmlPath) + "-" + ub.GetSub("BragBigPay", xmlPath) + "" + bzText + "", "");
            }
        }
        if (Price > gold)
        {
            Utils.Error("你的" + bzText + "不足", "");
        }

        //是否刷屏
        string appName = "LIGHT_BRAG";
        int Expir = Utils.ParseInt(ub.GetSub("BragExpir", xmlPath));
        BCW.User.Users.IsFresh(appName, Expir);

        string mename = new BCW.BLL.User().GetUsName(meid);
        if (bzType == 0)
        {
            new BCW.BLL.User().UpdateiGold(meid, mename, -Price, "吹牛消费");
        }
        else
        {
            new BCW.BLL.User().UpdateiMoney(meid, mename, -Price, "吹牛消费");
        }

        BCW.Model.Game.Brag model = new BCW.Model.Game.Brag();
        model.Title = Title;
        if (hid == 0)
            model.Types = 0;
        else
            model.Types = 1;

        model.BragA = BragA;
        model.BragB = BragB;
        model.ChooseType = 0;
        model.TrueType = TrueType;
        model.StopTime = DateTime.Now.AddHours(iTime);
        model.UsID = meid;
        model.UsName = mename;
        model.AddTime = DateTime.Now;
        model.ReID = hid;
        model.ReName = UsName;
        model.Price = Price;
        model.State = 0;
        model.BzType = bzType;
        int id = new BCW.BLL.Game.Brag().Add(model);
        //活跃抽奖入口_20160621姚志光
        try
        {
            //表中存在虚拟球类记录
            if (new BCW.BLL.tb_WinnersGame().ExistsGameName("吹牛游戏"))
            {
                //投注是否大于设定的限额，是则有抽奖机会
                if (500000 > new BCW.BLL.tb_WinnersGame().GetPrice("吹牛游戏"))
                {
                    string TextForUbb = (ub.GetSub("TextForUbb", "/Controls/winners.xml"));//设置内线提示的文字
                    string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", "/Controls/winners.xml"));//1发内线2不发内线 
                    int hit = new BCW.winners.winners().CheckActionForAll(1, 1, meid, mename, "吹牛", 3);
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
        if (hid == 0)
        {
            string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/brag.aspx]疯狂吹牛[/url]开盘成功(" + Price + "" + bzText + ")";
            new BCW.BLL.Action().Add(14, id, 0, "", wText);

            Utils.Success("我要吹牛", "恭喜您开盘成功!<br />牛已吹好,看谁来上当吧<br /><a href=\"" + Utils.getUrl("brag.aspx?act=add") + "\">我要继续吹牛&gt;&gt;</a>", Utils.getUrl("brag.aspx"), "3");
        }
        else
        {
            new BCW.BLL.Guest().Add(2, hid, UsName, "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]邀请您参与TA的吹牛:" + model.Title + "，[url=/bbs/game/brag.aspx?act=view&amp;id=" + id + "]立即参与[/url]，[url=/bbs/game/brag.aspx?act=no&amp;id=" + id + "]谢绝[/url]");
            string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/brag.aspx]疯狂吹牛[/url]向" + UsName + "发起挑战(" + Price + "" + bzText + ")";
            new BCW.BLL.Action().Add(14, id, 0, "", wText);
            Utils.Success("我要吹牛", "恭喜您开盘成功!<br />牛已吹好,等TA来上当吧<br />如果对方谢绝吹牛，这个吹牛将更改为公共吹牛<br /><a href=\"" + Utils.getUrl("/bbs/friend.aspx?act=online&amp;backurl=" + Server.UrlEncode("/bbs/game/brag.aspx?act=add") + "") + "\">我要继续对战&gt;&gt;</a>", Utils.getUrl("brag.aspx"), "3");
        }
    }

    private void ViewPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        ExpirDelPage();
        Master.Title = "吹牛";
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-2]$", "0"));
        BCW.Model.Game.Brag model = new BCW.BLL.Game.Brag().GetBrag(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        string bzText = string.Empty;
        if (model.BzType == 0)
            bzText = ub.Get("SiteBz");
        else
            bzText = ub.Get("SiteBz2");

        if (model.State > 0)
        {

            builder.Append("吹牛内容:" + model.Title + "<br />");
            builder.Append(DT.FormatDate(model.AddTime, 0) + "<br />");
            builder.Append("答案1:" + model.BragA + "");
            if (model.TrueType == 1)
                builder.Append("(正确)");

            builder.Append("<br />答案2:" + model.BragB + "");
            if (model.TrueType == 2)
                builder.Append("(正确)");

            builder.Append("<br />吹牛人:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.UsName + "(" + model.UsID + ")</a><br />");

            builder.Append("" + CaseBrag(model.Price, model.BzType) + ":" + model.Price + "" + bzText + "<br />");

            if (model.ReID > 0 && model.State != 4)
            {
                builder.Append("挑战者:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.ReID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.ReName + "(" + model.ReID + ")</a><br />");
            }
            if (model.State == 1 || model.State == 2)
            {
                if (model.TrueType != model.ChooseType)
                    builder.Append("获胜者:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.UsName + "(" + model.UsID + ")</a>");
                else
                    builder.Append("获胜者:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.ReID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.ReName + "(" + model.ReID + ")</a>");

            }
            else if (model.State == 3)
            {
                builder.Append("状态:过期返还" + model.Price + "" + bzText + "");
            }
            else if (model.State == 4)
            {
                builder.Append("状态:已经撤销");
            }
            builder.Append(Out.Tab("</div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("brag.aspx") + "\">返回上一级</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            //拾物随机
            builder.Append(BCW.User.Game.GiftFlows.ShowGiftFlows(9));

            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>&gt;<a href=\"" + Utils.getUrl("brag.aspx") + "\">吹牛</a>&gt;应答");
            builder.Append(Out.Tab("</div>", "<br />"));

            if (model.Types == 1)
            {
                if (model.UsID != meid && model.ReID != meid)
                {
                    Utils.Error("不存在的记录", "");
                }
            }
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("吹牛内容:" + model.Title + "<br />");
            builder.Append(DT.FormatDate(model.AddTime, 0) + "<br />");
            if (ptype == 0)
            {
                builder.Append("吹牛人:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.UsName + "</a><br />");
                builder.Append("" + CaseBrag(model.Price, model.BzType) + ":" + model.Price + "" + bzText + "");
                builder.Append("<br />请选择答案:<br />");
                builder.Append("<a href=\"" + Utils.getUrl("brag.aspx?act=view&amp;ptype=1&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + model.BragA + "</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("brag.aspx?act=view&amp;ptype=2&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + model.BragB + "</a>");
            }
            else
            {
                if (model.UsID == meid)
                {
                    Utils.Error("你害不害臊啊?不能够自吹自擂.", "");
                }
                if (model.Types == 0)
                {
                    int TNum = Utils.ParseInt(ub.GetSub("BragTNum", xmlPath));
                    if (TNum > 0)
                    {
                        int TCount2 = new BCW.BLL.Game.Brag().GetCount2(meid);
                        if (TCount2 > TNum)
                        {
                            int TCount = new BCW.BLL.Game.Brag().GetCount(meid);
                            if ((TCount2 - TNum) > Convert.ToInt32(TCount * TNum))
                            {
                                Utils.Error("每人每天可以先应答" + TNum + "次,之后必须自己吹一个牛才能再应答" + TNum + "次..<br /><a href=\"" + Utils.getUrl("brag.aspx?act=add") + "\">好的,我要吹牛&gt;&gt;</a>", "");
                            }
                        }
                    }
                }
                builder.Append("您猜的答案:");
                if (ptype == 1)
                    builder.Append(model.BragA);
                else
                    builder.Append(model.BragB);

                //操作币
                long winMoney = model.Price;
                //税率
                long SysTax = 0;
                int Tax = 0;
                if (CaseBrag(model.Price, model.BzType) == "蜗牛")
                    Tax = Utils.ParseInt(ub.GetSub("BragTar1", xmlPath));
                else if (CaseBrag(model.Price, model.BzType) == "水牛")
                    Tax = Utils.ParseInt(ub.GetSub("BragTar2", xmlPath));
                else
                    Tax = Utils.ParseInt(ub.GetSub("BragTar3", xmlPath));

                if (Tax > 0)
                {
                    SysTax = Convert.ToInt64(winMoney * Tax * 0.01);
                }
                winMoney = winMoney - SysTax;

                builder.Append(".吹牛:" + model.Price + "" + bzText + ",如果赢将返还您" + winMoney + "" + bzText + ".");
                builder.Append("<br /><a href=\"" + Utils.getUrl("brag.aspx?act=info&amp;ptype=" + ptype + "&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确认</a>");

            }

            builder.Append(Out.Tab("</div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("您目前自带" + Utils.ConvertGold(new BCW.BLL.User().GetGold(meid)) + "" + ub.Get("SiteBz") + "");
            if (Isbz())
                builder.Append("/" + Utils.ConvertGold(new BCW.BLL.User().GetMoney(meid)) + "" + ub.Get("SiteBz2") + "");

            builder.Append("<br /><a href=\"" + Utils.getPage("brag.aspx") + "\">再考虑一下&gt;&gt;</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("brag.aspx") + "\">吹牛</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void InfoPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Master.Title = "吹牛结果";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>&gt;<a href=\"" + Utils.getUrl("brag.aspx") + "\">吹牛</a>&gt;结果");
        builder.Append(Out.Tab("</div>", "<br />"));

        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 2, @"^[1-2]$", "选择答案错误"));
        BCW.Model.Game.Brag model = new BCW.BLL.Game.Brag().GetBrag(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.Types == 1)
        {
            if (model.UsID != meid && model.ReID != meid)
            {
                Utils.Error("不存在的记录", "");
            }
        }
        if (model.State > 0)
        {
            Utils.Error("该吹牛已经结束", "");
        }
        long gold = 0;
        string bzText = string.Empty;
        if (model.BzType == 0)
        {
            gold = new BCW.BLL.User().GetGold(meid);
            bzText = ub.Get("SiteBz");
        }
        else
        {
            gold = new BCW.BLL.User().GetMoney(meid);
            bzText = ub.Get("SiteBz2");
        }

        if (model.UsID == meid)
        {
            Utils.Error("你害不害臊啊?不能够自吹自擂.", "");
        }
        if (model.Price > gold)
        {
            Utils.Error("你的" + bzText + "不足", "");
        }
        if (model.Types == 0)
        {
            int TNum = Utils.ParseInt(ub.GetSub("BragTNum", xmlPath));
            if (TNum > 0)
            {
                int TCount2 = new BCW.BLL.Game.Brag().GetCount2(meid);
                if (TCount2 > TNum)
                {
                    int TCount = new BCW.BLL.Game.Brag().GetCount(meid);
                    if ((TCount2 - TNum) > Convert.ToInt32(TCount * TNum))
                    {
                        Utils.Error("每人每天可以先应答" + TNum + "次,之后必须自己吹一个牛才能再应答" + TNum + "次..<br /><a href=\"" + Utils.getUrl("brag.aspx?act=add") + "\">好的,我要吹牛&gt;&gt;</a>", "");
                    }
                }
            }
        }
        //支付安全提示
        string[] p_pageArr = { "act", "id", "ptype" };
        BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr, "get");

        //庄家是不是机器人
        bool IsRobot = false;
        if (("#" + ub.GetSub("GameZDID", xmlPath2) + "#").Contains("#" + model.UsID + "#"))
        {
            IsRobot = true;
        }
        //应战十赌六输
        if (IsRobot == true)
        {
            int bet = ptype;
            Random ra = new Random(unchecked((int)DateTime.Now.Ticks));
            int rdNext = ra.Next(1, 1000);
            if (rdNext <= 400)
            {
                rdNext = bet;
            }
            else
            {
                rdNext = ((bet == 1) ? 2 : 1);
            }
            BCW.Data.SqlHelper.ExecuteSql("update tb_Brag set TrueType=" + rdNext + " where id=" + id + "");
            model.TrueType = rdNext;
        }


        //更新吹牛记录
        string mename = new BCW.BLL.User().GetUsName(meid);
        BCW.Model.Game.Brag m = new BCW.Model.Game.Brag();
        m.ID = id;
        m.ReID = meid;
        m.ReName = mename;
        m.ReTime = DateTime.Now;
        m.ChooseType = ptype;
        m.State = 1;
        new BCW.BLL.Game.Brag().UpdateState(m);

        //操作币
        long winMoney = model.Price;
        //税率
        long SysTax = 0;
        int Tax = 0;
        if (CaseBrag(model.Price, model.BzType) == "蜗牛")
            Tax = Utils.ParseInt(ub.GetSub("BragTar1", xmlPath));
        else if (CaseBrag(model.Price, model.BzType) == "水牛")
            Tax = Utils.ParseInt(ub.GetSub("BragTar2", xmlPath));
        else
            Tax = Utils.ParseInt(ub.GetSub("BragTar3", xmlPath));

        if (Tax > 0)
        {
            SysTax = Convert.ToInt64(winMoney * Tax * 0.01);
        }
        winMoney = winMoney - SysTax;

        builder.Append(Out.Tab("<div>", ""));
        if (model.TrueType == ptype)
        {

            if (model.BzType == 0)
            {
                if (model.ReID == 0)
                {
                    new BCW.BLL.User().UpdateiGold(meid, mename, winMoney, 7);
                    new BCW.BLL.User().UpdateiGoldTop(model.UsID, model.UsName, -model.Price, 7);
                }
                else
                    new BCW.BLL.User().UpdateiGold(meid, mename, winMoney, "吹牛消费");
            }
            else
                new BCW.BLL.User().UpdateiMoney(meid, mename, winMoney, "吹牛消费");

            new BCW.BLL.Guest().Add(1, model.UsID, model.UsName, "您的吹牛:" + model.Title + "已经结束，参与吹牛人[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]，结果你忽悠不了对方，输了！[url=/bbs/game/brag.aspx?act=add]我要继续吹[/url]");

            string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/brag.aspx]疯狂吹牛[/url]识破了" + model.UsName + "的吹牛(赢" + model.Price + "" + bzText + ")";
            new BCW.BLL.Action().Add(14, id, 0, "", wText);

            builder.Append("恭喜!您识破了<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.UsName + "</a>那套吹牛的小伎俩,从TA的口袋里拿来了" + model.Price + "" + bzText + "!");
        }
        else
        {
            string TrueBrag = model.BragA;
            if (ptype == 1)
                TrueBrag = model.BragB;

            if (model.BzType == 0)
            {
                if (model.ReID == 0)
                {
                    new BCW.BLL.User().UpdateiGold(meid, mename, -model.Price, 7);
                    new BCW.BLL.User().UpdateiGoldTop(model.UsID, model.UsName, winMoney, 7);
                }
                else
                    new BCW.BLL.User().UpdateiGold(meid, mename, -model.Price, "吹牛消费");
            }
            else
                new BCW.BLL.User().UpdateiMoney(meid, mename, -model.Price, "吹牛消费");

            new BCW.BLL.Guest().Add(1, model.UsID, model.UsName, "您的吹牛:" + model.Title + "已经结束，参与吹牛人[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]，结果全赢！[url=/bbs/game/brag.aspx?act=case]马上兑奖[/url]");

            string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/brag.aspx]疯狂吹牛[/url]中的" + model.UsName + "的吹牛上当了(输" + model.Price + "" + bzText + ")";
            new BCW.BLL.Action().Add(14, id, 0, "", wText);

            builder.Append("你上当拉!正确答案应该是" + TrueBrag + ",您投的" + model.Price + "" + bzText + "都跑进入了<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.UsName + "</a>的口袋!如果不服气的话,那就赶快去进行下一场吹牛吧.");
        }
        builder.Append("<br />不如自己也来开个盘?<br />");
        builder.Append("<a href=\"" + Utils.getUrl("brag.aspx?act=add") + "\">好的,我要吹牛&gt;&gt;</a>");
        builder.Append(Out.Tab("</div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("您目前自带" + Utils.ConvertGold(new BCW.BLL.User().GetGold(meid)) + "" + ub.Get("SiteBz") + "");
        if (Isbz())
            builder.Append("/" + Utils.ConvertGold(new BCW.BLL.User().GetMoney(meid)) + "" + ub.Get("SiteBz2") + "");

        string BragText = CaseBrag(model.Price, model.BzType);
        int types = 0;
        if (BragText == "水牛")
            types = 1;
        else if (BragText == "犀牛")
            types = 2;

        builder.Append("<br /><a href=\"" + Utils.getUrl("brag.aspx?act=list&amp;ptype=" + types + "") + "\">返回" + CaseBrag(model.Price, model.BzType) + "列表&gt;&gt;</a>");
        builder.Append("<br /><a href=\"" + Utils.getUrl("brag.aspx?act=list") + "\">返回吹牛列表&gt;&gt;</a>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("brag.aspx") + "\">吹牛</a>");
        builder.Append(Out.Tab("</div>", ""));
    }


    private void CaseOkPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int pid = Utils.ParseInt(Utils.GetRequest("pid", "get", 2, @"^[0-9]\d*$", "选择兑奖无效"));

        if (new BCW.BLL.Game.Brag().ExistsState(pid, meid))
        {
            BCW.Model.Game.Brag model = new BCW.BLL.Game.Brag().GetBrag(pid);
            //操作币
            long winMoney = model.Price;
            //税率
            if (model.State == 1)
            {
                long SysTax = 0;
                int Tax = 0;
                if (CaseBrag(model.Price, model.BzType) == "蜗牛")
                    Tax = Utils.ParseInt(ub.GetSub("BragTar1", xmlPath));
                else if (CaseBrag(model.Price, model.BzType) == "水牛")
                    Tax = Utils.ParseInt(ub.GetSub("BragTar2", xmlPath));
                else
                    Tax = Utils.ParseInt(ub.GetSub("BragTar3", xmlPath));

                if (Tax > 0)
                {
                    SysTax = Convert.ToInt64(winMoney * Tax * 0.01);
                }
                winMoney = winMoney + winMoney - SysTax;
            }

            new BCW.BLL.Game.Brag().UpdateState(pid, 2);

            if (model.BzType == 0)
            {
                new BCW.BLL.User().UpdateiGold(meid, winMoney, "吹牛兑奖-标识ID" + pid + "");
                Utils.Success("兑奖", "恭喜，成功兑奖" + winMoney + "" + ub.Get("SiteBz") + "", Utils.getUrl("brag.aspx?act=case"), "1");
            }
            else
            {
                new BCW.BLL.User().UpdateiMoney(meid, winMoney, "吹牛兑奖-标识ID" + pid + "");
                Utils.Success("兑奖", "恭喜，成功兑奖" + winMoney + "" + ub.Get("SiteBz2") + "", Utils.getUrl("brag.aspx?act=case"), "1");
            }

        }
        else
        {
            Utils.Success("兑奖", "恭喜，重复兑奖或没有可以兑奖的记录", Utils.getUrl("brag.aspx?act=case"), "1");
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
            if (new BCW.BLL.Game.Brag().ExistsState(pid, meid))
            {
                BCW.Model.Game.Brag model = new BCW.BLL.Game.Brag().GetBrag(pid);
                //操作币
                long win = model.Price;
                //税率
                if (model.State == 1)
                {
                    long SysTax = 0;
                    int Tax = 0;
                    if (CaseBrag(model.Price, model.BzType) == "蜗牛")
                        Tax = Utils.ParseInt(ub.GetSub("BragTar1", xmlPath));
                    else if (CaseBrag(model.Price, model.BzType) == "水牛")
                        Tax = Utils.ParseInt(ub.GetSub("BragTar2", xmlPath));
                    else
                        Tax = Utils.ParseInt(ub.GetSub("BragTar3", xmlPath));

                    if (Tax > 0)
                    {
                        SysTax = Convert.ToInt64(win * Tax * 0.01);
                    }
                    win = win + win - SysTax;
                }

                new BCW.BLL.Game.Brag().UpdateState(pid, 2);

                if (model.BzType == 0)
                {
                    winMoney += win;
                    new BCW.BLL.User().UpdateiGold(meid, win, "吹牛兑奖-标识ID" + pid + "");
                }
                else
                {
                    winMoney2 += win;
                    new BCW.BLL.User().UpdateiMoney(meid, win, "吹牛兑奖-标识ID" + pid + "");
                }
            }
        }
        Utils.Success("兑奖", "恭喜，成功兑奖" + winMoney + "" + ub.Get("SiteBz") + "/" + winMoney2 + "" + ub.Get("SiteBz2") + "", Utils.getUrl("brag.aspx?act=case"), "1");
    }

    private void CasePage()
    {
        Master.Title = "兑奖中心";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>&gt;<a href=\"" + Utils.getUrl("brag.aspx") + "\">吹牛</a>&gt;兑奖");
        builder.Append(Out.Tab("</div>", Out.Hr()));

        int pageIndex;
        int recordCount;
        int pageSize = 5;
        string strWhere = string.Empty;
        strWhere = "UsID=" + meid + " and ((TrueType<>ChooseType and State=1) or State=3)";
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        string arrId = "";
        // 开始读取列表
        IList<BCW.Model.Game.Brag> listBrag = new BCW.BLL.Game.Brag().GetBrags(pageIndex, pageSize, strWhere, out recordCount);
        if (listBrag.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Brag n in listBrag)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }

                string bzText = string.Empty;
                if (n.BzType == 0)
                    bzText = ub.Get("SiteBz");
                else
                    bzText = ub.Get("SiteBz2");

                builder.Append(DT.FormatDate(n.AddTime, 1) + "<br />");
                builder.Append("内容:" + n.Title + "<br />");
                builder.Append("答案1:" + n.BragA + "");
                if (n.TrueType == 1)
                    builder.Append("(正确)");

                builder.Append("<br />答案2:" + n.BragB + "");
                if (n.TrueType == 2)
                    builder.Append("(正确)");

                //操作币
                long winMoney = n.Price;
                //税率
                long SysTax = 0;
                int Tax = 0;
                if (CaseBrag(n.Price, n.BzType) == "蜗牛")
                    Tax = Utils.ParseInt(ub.GetSub("BragTar1", xmlPath));
                else if (CaseBrag(n.Price, n.BzType) == "水牛")
                    Tax = Utils.ParseInt(ub.GetSub("BragTar2", xmlPath));
                else
                    Tax = Utils.ParseInt(ub.GetSub("BragTar3", xmlPath));

                if (Tax > 0)
                {
                    SysTax = Convert.ToInt64(winMoney * Tax * 0.01);
                }
                winMoney = winMoney + winMoney - SysTax;

                builder.Append("<br />" + CaseBrag(n.Price, n.BzType) + ":" + n.Price + "" + bzText + "<br />");
                if (n.State == 3)
                    builder.Append("开奖结果:过期返还" + n.Price + "" + bzText + "");
                else
                    builder.Append("开奖结果:赢" + winMoney + "" + bzText + "");

                builder.Append("<a href=\"" + Utils.getUrl("brag.aspx?act=caseok&amp;pid=" + n.ID + "") + "\">兑奖</a>");
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
            string strOthe = "本页兑奖,brag.aspx,post,0,red";
            builder.Append(Out.wapform(strName, strValu, strOthe));
        }
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("brag.aspx?act=mylist") + "\">我的吹牛</a>|<a href=\"" + Utils.getUrl("brag.aspx?act=dzlist") + "\">对战史</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("brag.aspx?act=cylist") + "\">吹牛记录&gt;&gt;</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append(" <a href=\"" + Utils.getUrl("brag.aspx") + "\">吹牛</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void MyListPage()
    {
        Master.Title = "我的吹牛";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        ExpirDelPage();

        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[0-2]$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>&gt;<a href=\"" + Utils.getUrl("brag.aspx") + "\">吹牛</a>&gt;历史");
        builder.Append(Out.Tab("</div>", Out.Hr()));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 0)
            builder.Append("全部|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("brag.aspx?act=mylist") + "\">全部</a>|");

        if (ptype == 1)
            builder.Append("未结束|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("brag.aspx?act=mylist&amp;ptype=1") + "\">未结束</a>|");

        if (ptype == 2)
            builder.Append("已结束");
        else
            builder.Append("<a href=\"" + Utils.getUrl("brag.aspx?act=mylist&amp;ptype=2") + "\">已结束</a>");

        builder.Append(Out.Tab("</div>", Out.Hr()));

        int pageIndex;
        int recordCount;
        int pageSize = 5;
        string strWhere = string.Empty;
        strWhere = "UsID=" + meid + "";
        if (ptype == 1)
            strWhere += " and State=0";
        else if (ptype == 2)
            strWhere += " and State>0";

        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Game.Brag> listBrag = new BCW.BLL.Game.Brag().GetBrags(pageIndex, pageSize, strWhere, out recordCount);
        if (listBrag.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Brag n in listBrag)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }

                string bzText = string.Empty;
                if (n.BzType == 0)
                    bzText = ub.Get("SiteBz");
                else
                    bzText = ub.Get("SiteBz2");

                builder.Append(DT.FormatDate(n.AddTime, 1) + "<br />");
                builder.Append("内容:" + n.Title + "<br />");
                builder.Append("答案1:" + n.BragA + "");
                if (n.TrueType == 1)
                    builder.Append("(正确)");

                builder.Append("<br />答案2:" + n.BragB + "");
                if (n.TrueType == 2)
                    builder.Append("(正确)");

                builder.Append("<br />" + CaseBrag(n.Price, n.BzType) + ":" + n.Price + "" + bzText + "<br />");

                if (n.ReID > 0 && n.State != 4)
                {
                    if (n.State == 0)
                        builder.Append("等待");

                    builder.Append("应答人:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.ReID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.ReName + "(" + n.ReID + ")</a><br />");
                }
                if (n.State == 0)
                    builder.Append("开奖结果:未开奖<a href=\"" + Utils.getUrl("brag.aspx?act=del&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[撤销吹牛]</a>");
                else if (n.State == 1 || n.State == 2)
                {
                    if (n.State == 2 && n.ReID == 0)
                    {
                        builder.Append("状态:已返还" + n.Price + "" + bzText + "");
                    }
                    else
                    {
                        if (n.TrueType != n.ChooseType)
                            builder.Append("开奖结果:全赢");
                        else
                            builder.Append("开奖结果:全输");
                    }
                }
                else if (n.State == 3)
                {
                    builder.Append("状态:过期返还" + n.Price + "" + bzText + "");
                }
                else if (n.State == 4)
                {
                    builder.Append("状态:已经撤销");
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
        builder.Append("<a href=\"" + Utils.getUrl("brag.aspx?act=mylist") + "\">我的吹牛</a>|<a href=\"" + Utils.getUrl("brag.aspx?act=dzlist") + "\">对战史</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("brag.aspx?act=cylist") + "\">吹牛记录&gt;&gt;</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append(" <a href=\"" + Utils.getUrl("brag.aspx") + "\">吹牛</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void CyListPage()
    {
        Master.Title = "吹牛记录";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>&gt;<a href=\"" + Utils.getUrl("brag.aspx") + "\">吹牛</a>&gt;记录");
        builder.Append(Out.Tab("</div>", Out.Hr()));
        int pageIndex;
        int recordCount;
        int pageSize = 5;
        string strWhere = string.Empty;
        strWhere = "ReID=" + meid + " and State>0";

        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Game.Brag> listBrag = new BCW.BLL.Game.Brag().GetBrags(pageIndex, pageSize, strWhere, out recordCount);
        if (listBrag.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Brag n in listBrag)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }

                string bzText = string.Empty;
                if (n.BzType == 0)
                    bzText = ub.Get("SiteBz");
                else
                    bzText = ub.Get("SiteBz2");

                builder.Append(DT.FormatDate(n.AddTime, 1) + "<br />");
                builder.Append("吹牛人:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")</a><br />");
                builder.Append("内容:" + n.Title + "<br />");
                builder.Append("答案1:" + n.BragA + "");
                if (n.TrueType == 1)
                    builder.Append("(正确)");

                builder.Append("<br />答案2:" + n.BragB + "");
                if (n.TrueType == 2)
                    builder.Append("(正确)");

                builder.Append("<br />" + CaseBrag(n.Price, n.BzType) + ":" + n.Price + "" + bzText + "<br />");

                if (n.TrueType == n.ChooseType)
                    builder.Append("开奖结果:全赢");
                else
                    builder.Append("开奖结果:全输");

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
        builder.Append("<a href=\"" + Utils.getUrl("brag.aspx?act=mylist") + "\">我的吹牛</a>|<a href=\"" + Utils.getUrl("brag.aspx?act=dzlist") + "\">对战史</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("brag.aspx?act=cylist") + "\">吹牛记录&gt;&gt;</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append(" <a href=\"" + Utils.getUrl("brag.aspx") + "\">吹牛</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void DzListPage()
    {
        Master.Title = "吹牛对战史";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[0-1]$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>&gt;<a href=\"" + Utils.getUrl("brag.aspx") + "\">吹牛</a>&gt;对战史");
        builder.Append(Out.Tab("</div>", Out.Hr()));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 0)
            builder.Append("挑战史|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("brag.aspx?act=dzlist") + "\">挑战史</a>|");

        if (ptype == 1)
            builder.Append("应战史");
        else
            builder.Append("<a href=\"" + Utils.getUrl("brag.aspx?act=dzlist&amp;ptype=1") + "\">应战史</a>");

        builder.Append(Out.Tab("</div>", Out.Hr()));

        int pageIndex;
        int recordCount;
        int pageSize = 5;
        string strWhere = "Types=1 and ";
        if (ptype == 0)
            strWhere += "UsID=" + meid + "";
        else
            strWhere += "ReID=" + meid + "";

        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Game.Brag> listBrag = new BCW.BLL.Game.Brag().GetBrags(pageIndex, pageSize, strWhere, out recordCount);
        if (listBrag.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Brag n in listBrag)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }

                string bzText = string.Empty;
                if (n.BzType == 0)
                    bzText = ub.Get("SiteBz");
                else
                    bzText = ub.Get("SiteBz2");

                builder.Append(DT.FormatDate(n.AddTime, 1) + "<br />");
                if (ptype == 1)
                    builder.Append("吹牛人:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")</a><br />");

                builder.Append("内容:" + n.Title + "<br />");
                builder.Append("答案1:" + n.BragA + "");
                if (n.TrueType == 1 && n.State > 0)
                    builder.Append("(正确)");

                builder.Append("<br />答案2:" + n.BragB + "");
                if (n.TrueType == 2 && n.State > 0)
                    builder.Append("(正确)");

                builder.Append("<br />" + CaseBrag(n.Price, n.BzType) + ":" + n.Price + "" + bzText + "<br />");
                if (ptype == 0)
                {
                    if (n.State == 0)
                        builder.Append("等待");

                    builder.Append("应答人:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.ReID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.ReName + "(" + n.ReID + ")</a><br />");
                }
                if (n.State > 0)
                {
                    if (ptype == 0)
                    {
                        if (n.TrueType != n.ChooseType)
                            builder.Append("开奖结果:全赢");
                        else
                            builder.Append("开奖结果:全输");
                    }
                    else
                    {
                        if (n.TrueType == n.ChooseType)
                            builder.Append("开奖结果:全赢");
                        else
                            builder.Append("开奖结果:全输");
                    }
                }
                else
                {
                    if (ptype == 0)
                        builder.Append("<a href=\"" + Utils.getUrl("brag.aspx?act=del&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[撤销吹牛]</a>");
                    else
                    {
                        builder.Append("<a href=\"" + Utils.getUrl("brag.aspx?act=view&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[应战]</a>");
                        builder.Append("<a href=\"" + Utils.getUrl("brag.aspx?act=no&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[谢绝]</a>");
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
        builder.Append("<a href=\"" + Utils.getUrl("brag.aspx?act=mylist") + "\">我的吹牛</a>|<a href=\"" + Utils.getUrl("brag.aspx?act=dzlist") + "\">对战史</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("brag.aspx?act=cylist") + "\">吹牛记录&gt;&gt;</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append(" <a href=\"" + Utils.getUrl("brag.aspx") + "\">吹牛</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void DelPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        string info = Utils.GetRequest("info", "get", 1, "", "");
        BCW.Model.Game.Brag model = new BCW.BLL.Game.Brag().GetBrag(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.UsID != meid)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.State > 0)
        {
            Utils.Error("该吹牛已经删除或者已经结束.", "");
        }
        string bzText = string.Empty;
        if (model.BzType == 0)
            bzText = ub.Get("SiteBz");
        else
            bzText = ub.Get("SiteBz2");

        long Price = 0;
        Price = model.Price;

        if (info == "ok")
        {
            if (model.BzType == 0)
                new BCW.BLL.User().UpdateiGold(meid, model.UsName, Price, "撤销吹牛得到");
            else
                new BCW.BLL.User().UpdateiMoney(meid, model.UsName, Price, "撤销吹牛得到");

            new BCW.BLL.Game.Brag().UpdateState(id, 4);

            Utils.Success("撤销吹牛", "撤销吹牛成功，返还" + Price + "" + bzText + "", Utils.getPage("brag.aspx?act=mylist"), "1");

        }
        else
        {
            Master.Title = "吹牛";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>&gt;<a href=\"" + Utils.getUrl("brag.aspx") + "\">吹牛</a>&gt;撤销");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("您的吹牛内容:" + model.Title + "<br />");
            if (model.TrueType == 1)
                builder.Append("答案:" + model.BragA + "");
            else
                builder.Append("答案:" + model.BragB + "");

            builder.Append("<br />吹牛:" + model.Price + "" + bzText + ",如果取消将自动返还您" + Price + "" + bzText + ".");
            builder.Append("<br /><a href=\"" + Utils.getUrl("brag.aspx?act=del&amp;info=ok&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">我确定这个牛不吹了</a>");
            builder.Append("<br /><a href=\"" + Utils.getPage("brag.aspx") + "\">再考虑一下&gt;&gt;</a>");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("brag.aspx") + "\">吹牛</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private void NoPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        string info = Utils.GetRequest("info", "get", 1, "", "");
        BCW.Model.Game.Brag model = new BCW.BLL.Game.Brag().GetBrag(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.ReID != meid)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.State > 0)
        {
            Utils.Error("该吹牛已经删除或者已经结束.", "");
        }
        if (info == "ok")
        {
            new BCW.BLL.Game.Brag().UpdateState2(id);
            new BCW.BLL.Guest().Add(1, model.UsID, model.UsName, "您对[url=/bbs/uinfo.aspx?uid=" + meid + "]" + new BCW.BLL.User().GetUsName(meid) + "[/url]发起的吹牛:" + model.Title + "已经被谢绝，系统已将此吹牛自动更改为公共吹牛！[url=/bbs/game/brag.aspx?act=view&amp;id=" + id + "]查看详情[/url]");
            Utils.Success("谢绝吹牛", "谢绝吹牛成功", Utils.getPage("brag.aspx?act=dzlist&amp;ptype=1"), "1");
        }
        else
        {
            Master.Title = "谢绝吹牛";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>&gt;<a href=\"" + Utils.getUrl("brag.aspx") + "\">吹牛</a>&gt;谢绝");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("吹牛人:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.UsName + "(" + model.UsID + ")</a><br />");
            builder.Append("吹牛内容:" + model.Title + "<br />");
            builder.Append("答案1:" + model.BragA + "<br />");
            builder.Append("答案2:" + model.BragB + "");

            string bzText = string.Empty;
            if (model.BzType == 0)
                bzText = ub.Get("SiteBz");
            else
                bzText = ub.Get("SiteBz2");

            builder.Append("<br />吹牛:" + model.Price + "" + bzText + ",如果谢绝吹牛，这个吹牛将更改为公共吹牛.");
            builder.Append("<br /><a href=\"" + Utils.getUrl("brag.aspx?act=no&amp;info=ok&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">我确定不猜这个牛</a>");
            builder.Append("<br /><a href=\"" + Utils.getPage("brag.aspx") + "\">再考虑一下&gt;&gt;</a>");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("brag.aspx") + "\">吹牛</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private void ListPage()
    {
        Master.Title = "吹牛列表";
        int showtype = Utils.ParseInt(Utils.GetRequest("showtype", "get", 1, @"^[0-1]$", "0"));
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[0-3]$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>&gt;<a href=\"" + Utils.getUrl("brag.aspx") + "\">吹牛</a>&gt;列表");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (Isbz())
        {
            builder.Append("币种:");

            if (showtype == 0)
                builder.Append("" + ub.Get("SiteBz") + "|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("brag.aspx?act=list&amp;ptype=" + ptype + "") + "\">" + ub.Get("SiteBz") + "</a>|");

            if (showtype == 1)
                builder.Append("" + ub.Get("SiteBz2") + "<br />");
            else
                builder.Append("<a href=\"" + Utils.getUrl("brag.aspx?act=list&amp;ptype=" + ptype + "&amp;showtype=1") + "\">" + ub.Get("SiteBz2") + "</a><br />");
        }
        if (ptype == 0)
        {
            builder.Append("=最新吹牛=");
            builder.Append("<a href=\"" + Utils.getUrl("brag.aspx?act=list&amp;ptype=" + ptype + "&amp;showtype=" + showtype + "") + "\">刷新</a>");
        }
        else if (ptype == 1)
        {
            builder.Append("=水牛推荐=");
            builder.Append("<a href=\"" + Utils.getUrl("brag.aspx?act=list&amp;ptype=" + ptype + "&amp;showtype=" + showtype + "") + "\">刷新</a>");
        }
        else if (ptype == 2)
        {
            builder.Append("=犀牛待宰=");
            builder.Append("<a href=\"" + Utils.getUrl("brag.aspx?act=list&amp;ptype=" + ptype + "&amp;showtype=" + showtype + "") + "\">刷新</a>");
        }
        else
        {
            builder.Append("=已应答的吹牛=");
            builder.Append("<a href=\"" + Utils.getUrl("brag.aspx?act=list&amp;showtype=" + showtype + "") + "\">最新</a>");
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = 10;
        string strWhere = "";
        if (ptype == 1)
        {
            if (showtype == 0)
                strWhere += "Types=0 and State=0  and BzType=" + showtype + " and StopTime>='" + DateTime.Now + "' and Price>=" + F1 + " and Price<" + F2 + "";
            else
                strWhere += "Types=0 State=0 and BzType=" + showtype + " and StopTime>='" + DateTime.Now + "' and Price>=" + F3 + " and Price<" + F4 + "";
        }
        else if (ptype == 2)
        {
            if (showtype == 0)
                strWhere += "Types=0 State=0 and BzType=" + showtype + " and StopTime>='" + DateTime.Now + "' and Price>=" + F2 + " ";
            else
                strWhere += "Types=0 State=0 and BzType=" + showtype + " and StopTime>='" + DateTime.Now + "' and Price>=" + F4 + " ";
        }
        else if (ptype == 3)
            strWhere += "State>0 and BzType=" + showtype + "";
        else
            strWhere += "Types=0 and State=0 and BzType=" + showtype + "";

        string[] pageValUrl = { "act", "ptype", "showtype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Game.Brag> listBrag = new BCW.BLL.Game.Brag().GetBrags(pageIndex, pageSize, strWhere, out recordCount);
        if (listBrag.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Brag n in listBrag)
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

                string bzText = string.Empty;
                if (n.BzType == 0)
                    bzText = ub.Get("SiteBz");
                else
                    bzText = ub.Get("SiteBz2");

                builder.AppendFormat("[" + CaseBrag(n.Price, n.BzType) + "]<a href=\"" + Utils.getUrl("brag.aspx?act=view&amp;id={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{1}({2}{3})</a>", n.ID, n.Title, n.Price, bzText);

                k++;
                builder.Append(Out.Tab("</div>", ""));
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 1));

        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }


        //闲聊显示
        builder.Append(Out.SysUBB(BCW.User.Users.ShowSpeak(14, "brag.aspx?act=list&amp;ptype=" + ptype + "", 5, 0)));

        builder.Append(Out.Tab("<div>", Out.Hr()));
        if (ptype == 0)
            builder.Append("<a href=\"" + Utils.getUrl("brag.aspx?act=list&amp;ptype=3") + "\">已结束的吹牛&gt;&gt;</a>");
        else
            builder.Append("<a href=\"" + Utils.getUrl("brag.aspx?act=list") + "\">最新吹牛&gt;&gt;</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append(" <a href=\"" + Utils.getUrl("brag.aspx") + "\">吹牛</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    /// <summary>
    /// 排行榜
    /// </summary>
    private void TopPage()
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));
        Master.Title = "疯狂吹牛排行榜";
        builder.Append(Out.Tab("<div class=\"title\">", ""));

        if (ptype == 1)
            builder.Append("赚币排行|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("brag.aspx?act=top&amp;ptype=1") + "\">赚币排行</a>|");

        if (ptype == 2)
            builder.Append("胜率排行");
        else
            builder.Append("<a href=\"" + Utils.getUrl("brag.aspx?act=top&amp;ptype=2") + "\">胜率排行</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string strOrder = "";
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "Types=7";
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
        builder.Append("<a href=\"" + Utils.getUrl("brag.aspx") + "\">吹牛</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void RulePage()
    {
        Master.Title = "吹牛游戏规则";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("吹牛游戏规则");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("疯狂吹牛是由用户自己开局吹牛与另一个玩家进行心理大战的趣味游戏,游戏过程只允许2个用户参与,吹牛人自行设定吹牛内容,答案,时限以及虚拟币数量,开局成功后其他应答用户均可以参与,但只有一名用户可以最终参与并应答,游戏结束后将收取胜利方一定的手续费.<br />");
        builder.Append("【吹牛分类】<br />");
        if (Isbz())
            builder.Append("=" + ub.Get("SiteBz") + "类=<br />");

        builder.Append("[蜗牛]" + ub.GetSub("BragSmallPay", xmlPath) + "-" + ub.GetSub("BragF1", xmlPath) + "" + ub.Get("SiteBz") + "之间|" + ub.GetSub("BragTar1", xmlPath) + "%手续费<br />");
        builder.Append("[水牛]" + ub.GetSub("BragF1", xmlPath) + "-" + ub.GetSub("BragF2", xmlPath) + "" + ub.Get("SiteBz") + "之间|" + ub.GetSub("BragTar2", xmlPath) + "%手续费<br />");
        builder.Append("[犀牛]" + ub.GetSub("BragF2", xmlPath) + "-" + ub.GetSub("BragBigPay", xmlPath) + "" + ub.Get("SiteBz") + "之间|" + ub.GetSub("BragTar3", xmlPath) + "%手续费<br />");
        if (Isbz())
        {
            builder.Append("=" + ub.Get("SiteBz2") + "类=<br />");
            builder.Append("[蜗牛]" + ub.GetSub("BragSmallPay2", xmlPath) + "-" + ub.GetSub("BragF3", xmlPath) + "" + ub.Get("SiteBz2") + "之间|" + ub.GetSub("BragTar1", xmlPath) + "%手续费<br />");
            builder.Append("[水牛]" + ub.GetSub("BragF3", xmlPath) + "-" + ub.GetSub("BragF4", xmlPath) + "" + ub.Get("SiteBz2") + "之间|" + ub.GetSub("BragTar2", xmlPath) + "%手续费<br />");
            builder.Append("[犀牛]" + ub.GetSub("BragF4", xmlPath) + "-" + ub.GetSub("BragBigPay2", xmlPath) + "" + ub.Get("SiteBz2") + "之间|" + ub.GetSub("BragTar3", xmlPath) + "%手续费<br />");
        }
        builder.Append("【注意事项】<br />");
        builder.Append("-自己吹的牛不能自己来应答!<br />");
        if (ub.GetSub("BragTNum", xmlPath) != "0")
            builder.Append("-每人每天可以先应答" + ub.GetSub("BragTNum", xmlPath) + "次,之后必须自己吹一个牛才能再应答" + ub.GetSub("BragTNum", xmlPath) + "次.<br />");

        builder.Append("-吹牛有时间性,过了有效时间,则本金返还,不会扣除手续费.<br />");
        builder.Append("-应答赢的吹牛系统已自动兑奖，吹牛人获胜需点“兑奖”才能领奖.<br />");
        builder.Append("-请吹牛者自重!禁止一切侮辱他人或者捏造事实诽谤他人!<br />");
        builder.Append("-请吹牛者自重!任何违反国家相关法律的言论,一旦违反将严肃处理!");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("brag.aspx") + "\">吹牛</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private string CaseBrag(long Price, int BzType)
    {
        if (BzType == 0)
        {
            if (Price >= F2)
                return "犀牛";

            if (Price > F1)
                return "水牛";
        }
        else
        {
            if (Price >= F4)
                return "犀牛";

            if (Price > F3)
                return "水牛";
        }
        return "蜗牛";
    }

    private bool Isbz()
    {
        return true;

        //if (Utils.GetDomain().Contains("kubao") || Utils.GetDomain().Contains("tuhao") || Utils.GetDomain().Contains("th") || Utils.GetDomain().Contains("kb288"))
        //    return true;
        //else
        //    return false;
    }

    private void ExpirDelPage()
    {
        DataSet ds = new BCW.BLL.Game.Brag().GetList("ID", "StopTime<'" + DateTime.Now + "' and State=0");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                int id = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                new BCW.BLL.Game.Brag().UpdateState(id, 3);
            }
        }
    }
}