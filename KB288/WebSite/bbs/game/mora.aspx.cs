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

public partial class bbs_game_mora : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/mora.xml";
    protected long F1 = Convert.ToInt64(ub.GetSub("MoraF1", "/Controls/mora.xml"));
    protected long F2 = Convert.ToInt64(ub.GetSub("MoraF2", "/Controls/mora.xml"));
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
        if (ub.GetSub("MoraStatus", xmlPath) == "1")
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
            case "list":
                ListPage();
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
            case "add":
                AddPage();
                break;
            case "addsave":
                AddSavePage();
                break;
            case "view":
                ViewPage();
                break;
            case "del":
                DelPage();
                break;
            case "no":
                NoPage();
                break;
            case "top":
                TopPage();
                break;
            case "rule":
                RulePage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        Master.Title = ub.GetSub("MoraName", xmlPath);
        string Logo = ub.GetSub("MoraLogo", xmlPath);
        if (Logo != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<img src=\"" + Logo + "\" alt=\"load\"/>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        string Notes = ub.GetSub("MoraNotes", xmlPath);
        if (Notes != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.SysUBB(Notes) + "");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;猜拳");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("【一拳定输赢】<a href=\"" + Utils.getUrl("mora.aspx?act=rule") + "\">规则</a>.<a href=\"" + Utils.getUrl("mora.aspx?act=case") + "\">兑奖</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("mora.aspx?act=add") + "\">我要猜拳</a>.<a href=\"" + Utils.getUrl("mora.aspx?act=mylist") + "\">历史</a>|<a href=\"" + Utils.getUrl("mora.aspx?act=cylist") + "\">猜拳记录</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/friend.aspx?act=online&amp;backurl=" + Server.UrlEncode("/bbs/game/mora.aspx?act=add") + "") + "\">找人对战</a>.<a href=\"" + Utils.getUrl("mora.aspx?act=dzlist") + "\">对战史</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/action.aspx?ptype=16&amp;backurl=" + Utils.PostPage(1) + "") + "\">动态</a><br />");
        builder.Append("今日猜拳总数:" + new BCW.BLL.Game.Mora().GetCount() + "个<br />");
        builder.Append("今日猜拳总量:" + new BCW.BLL.Game.Mora().GetPrice(0) + "" + ub.Get("SiteBz") + "");
        if (Isbz())
            builder.Append("/" + new BCW.BLL.Game.Mora().GetPrice(1) + "" + ub.Get("SiteBz2") + "");
        
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("【最新对局】<a href=\"" + Utils.getUrl("mora.aspx") + "\">刷新</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        // 开始读取猜拳列表
        int SizeNum = 3;
        string strWhere = "Types=0 and state=0 and StopTime>='" + DateTime.Now + "'";
        IList<BCW.Model.Game.Mora> listMora = new BCW.BLL.Game.Mora().GetMoras(SizeNum, strWhere);
        if (listMora.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Mora n in listMora)
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

                builder.AppendFormat("<a href=\"" + Utils.getUrl("mora.aspx?act=view&amp;id={0}") + "\">{1}({2}{3})</a>", n.ID, n.Title, n.Price, bzText);
                builder.Append(Out.Tab("</div>", ""));
                k++;
            }

        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("mora.aspx?act=list") + "\">&gt;&gt;更多</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("mora.aspx?act=list&amp;ptype=2") + "\">+已应战猜拳&gt;&gt;</a>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("【大局推荐】");
        builder.Append("<a href=\"" + Utils.getUrl("mora.aspx?act=list&amp;ptype=1") + "\">更多&gt;&gt;</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        SizeNum = 3;
        if (!Isbz())
            strWhere = "Types=0 and state=0 and StopTime>='" + DateTime.Now + "' and (Price>=" + F1 + " and BzType=0)";
        else
            strWhere = "Types=0 and state=0 and StopTime>='" + DateTime.Now + "' and ((Price>=" + F1 + " and BzType=0) or (Price>=" + F2 + " and BzType=1))";

        listMora = new BCW.BLL.Game.Mora().GetMoras(SizeNum, strWhere);
        if (listMora.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Mora n in listMora)
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

                builder.AppendFormat("<a href=\"" + Utils.getUrl("mora.aspx?act=view&amp;id={0}") + "\">{1}({2}{3})</a>", n.ID, n.Title, n.Price, bzText);
                builder.Append(Out.Tab("</div>", ""));
                k++;
            }
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }

        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("★猜拳排行★");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));

        DataSet ds = new BCW.BLL.Toplist().GetList("Top 1 UsID,WinGold,PutGold", "Types=8 Order by ((-PutGold)+WinGold) Desc");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            builder.Append("№拳王：<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + ds.Tables[0].Rows[0]["UsID"] + "") + "\">" + BCW.User.Users.SetUser(int.Parse(ds.Tables[0].Rows[0]["UsID"].ToString())) + "</a>参与" + (Convert.ToInt64(ds.Tables[0].Rows[0]["WinGold"]) + (-Convert.ToInt64(ds.Tables[0].Rows[0]["PutGold"]))) + "" + ub.Get("SiteBz") + "<br />");
        }
        else
            builder.Append("№拳王：暂无记录<br />");

        ds = new BCW.BLL.Toplist().GetList("Top 1 UsID,WinGold,PutGold", "Types=8 Order by (WinGold+PutGold) Desc");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            builder.Append("№拳总：<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + ds.Tables[0].Rows[0]["UsID"] + "") + "\">" + BCW.User.Users.SetUser(int.Parse(ds.Tables[0].Rows[0]["UsID"].ToString())) + "</a>净赚" + (Convert.ToInt64(ds.Tables[0].Rows[0]["WinGold"]) + Convert.ToInt64(ds.Tables[0].Rows[0]["PutGold"])) + "" + ub.Get("SiteBz") + "<br />");
        }
        else
            builder.Append("№拳总：暂无记录<br />");


        ds = new BCW.BLL.Toplist().GetList("Top 1 UsID,WinNum,PutNum", "Types=8 Order by (WinNum-PutNum) Desc");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            builder.Append("№拳胜：<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + ds.Tables[0].Rows[0]["UsID"] + "") + "\">" + BCW.User.Users.SetUser(int.Parse(ds.Tables[0].Rows[0]["UsID"].ToString())) + "</a>净胜" + (Convert.ToInt64(ds.Tables[0].Rows[0]["WinNum"]) - Convert.ToInt64(ds.Tables[0].Rows[0]["PutNum"])) + "场");
        }
        else
            builder.Append("№拳胜：暂无记录");


        builder.Append("<br /><a href=\"" + Utils.getUrl("mora.aspx?act=top&amp;ptype=1") + "\">+猜拳排行榜&gt;&gt;</a>");

        builder.Append(Out.Tab("</div>", ""));


        //游戏底部Ubb
        string Foot = ub.GetSub("MoraFoot", xmlPath);
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

    private void AddPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int hid = int.Parse(Utils.GetRequest("hid", "all", 1, @"^[0-9]\d*$", "0"));
        Master.Title = "我要猜拳";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>&gt;<a href=\"" + Utils.getUrl("mora.aspx") + "\">猜拳</a>&gt;我要猜拳");
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
        //strText = "猜拳，一拳定输赢!/我出:,陷阱:,有效时限:,押注金额:/,,";
        //strName = "TrueType,Title,iTime,Price,hid,act";
        //strType = "select,text,select,num,hidden,hidden";
        //strValu = "1''1'" + ub.GetSub("MoraSmallPay", xmlPath) + "'" + hid + "'addsave";
        //strEmpt = "1|剪刀|2|石头|3|布,true,1|1小时|2|2小时|4|4小时|6|6小时|12|12小时|24|1天|48|2天,false,false,false";
        //strIdea = "/";
        //if (Isbz())
        //    strOthe = "押" + ub.Get("SiteBz") + "|押" + ub.Get("SiteBz2") + ",mora.aspx,post,0,red|blue";
        //else
        //    strOthe = "押" + ub.Get("SiteBz") + ",mora.aspx,post,0,red";

        strText = "猜拳，一拳定输赢!/陷阱:,有效时限:,押注金额:/,,";
        strName = "itle,iTime,Price,hid,act";
        strType = "text,select,num,hidden,hidden";
        strValu = "'1'" + ub.GetSub("MoraSmallPay", xmlPath) + "'" + hid + "'addsave";
        strEmpt = "true,1|1小时|2|2小时|4|4小时|6|6小时|12|12小时|24|1天|48|2天,false,false,false";
        strIdea = "/";
        strOthe = "押剪刀|石头|布,mora.aspx,post,0,red|blue|red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("温馨提示:<br />陷阱是用来迷惑对手.可留空.例:我出的是拳头,你信吗?<br />");
        builder.Append("下注限" + ub.GetSub("MoraSmallPay", xmlPath) + "" + ub.Get("SiteBz") + "-" + ub.GetSub("MoraBigPay", xmlPath) + "<br />");
        builder.Append("您目前自带" + Utils.ConvertGold(new BCW.BLL.User().GetGold(meid)) + "" + ub.Get("SiteBz") + "");
        if (Isbz())
            builder.Append("/" + Utils.ConvertGold(new BCW.BLL.User().GetMoney(meid)) + "" + ub.Get("SiteBz2") + "");

        builder.Append("<br /><a href=\"" + Utils.getPage("mora.aspx") + "\">再考虑一下&gt;&gt;</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("mora.aspx") + "\">猜拳</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void AddSavePage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string ac = Utils.GetRequest("ac", "post", 1, "", "");
        string Title = Utils.GetRequest("Title", "post", 3, @"^[\s\S]{1,16}$", "猜拳陷阱请保持在16字内");
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

        int TrueType = 1;
        if (Utils.ToSChinese(ac).Contains(Utils.ToSChinese("押剪刀")))
        {
            TrueType = 1;
        }
        else if (Utils.ToSChinese(ac).Contains(Utils.ToSChinese("石头")))
        {
            TrueType = 2;
        }
        else
        {
            TrueType = 3;
        }
        //int TrueType = int.Parse(Utils.GetRequest("TrueType", "post", 2, @"^[1-3]$", "出拳类型选择错误"));
        long Price = Int64.Parse(Utils.GetRequest("Price", "post", 4, @"^[0-9]\d*$", "押注金额错误"));

        int bzType = 0;
        string bzText = string.Empty;
        long gold = 0;
        //if (Utils.ToSChinese(ac).Contains(Utils.ToSChinese(ub.Get("SiteBz2"))))
        //{
        //    bzType = 1;
        //    bzText = ub.Get("SiteBz2");
        //    gold = new BCW.BLL.User().GetMoney(meid);
        //    if (Price < Convert.ToInt64(ub.GetSub("MoraSmallPay2", xmlPath)) || Price > Convert.ToInt64(ub.GetSub("MoraBigPay2", xmlPath)))
        //    {
        //        Utils.Error("押注金额限" + ub.GetSub("MoraSmallPay2", xmlPath) + "-" + ub.GetSub("MoraBigPay2", xmlPath) + "" + bzText + "", "");
        //    }
        //}
        //else
        //{
            //支付安全提示
            string[] p_pageArr = { "act", "hid", "Title", "iTime", "TrueType", "Price", "ac" };
            BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr);

            bzType = 0;
            bzText = ub.Get("SiteBz");
            gold = new BCW.BLL.User().GetGold(meid);
            if (Price < Convert.ToInt64(ub.GetSub("MoraSmallPay", xmlPath)) || Price > Convert.ToInt64(ub.GetSub("MoraBigPay", xmlPath)))
            {
                Utils.Error("押注金额限" + ub.GetSub("MoraSmallPay", xmlPath) + "-" + ub.GetSub("MoraBigPay", xmlPath) + "" + bzText + "", "");
            }
        //}
        if (Price > gold)
        {
            Utils.Error("你的" + bzText + "不足", "");
        }

        //是否刷屏
        string appName = "LIGHT_MORA";
        int Expir = Utils.ParseInt(ub.GetSub("MoraExpir", xmlPath));
        BCW.User.Users.IsFresh(appName, Expir);

        string mename = new BCW.BLL.User().GetUsName(meid);
        if (bzType == 0)
        {
            //私人对战不计排行榜
            //if (hid == 0)
            //    new BCW.BLL.User().UpdateiGold(meid, mename, -Price, 8);
            //else
                new BCW.BLL.User().UpdateiGold(meid, mename, -Price, "猜拳消费");
        }
        else
        {
            new BCW.BLL.User().UpdateiMoney(meid, mename, -Price, "猜拳消费");
        }

        BCW.Model.Game.Mora model = new BCW.Model.Game.Mora();
        if (Title == "")
            Title = mename;

        model.Title = Title;
        if (hid == 0)
            model.Types = 0;
        else
            model.Types = 1;

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
        int id = new BCW.BLL.Game.Mora().Add(model);
        if (hid == 0)
        {
            string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/mora.aspx]猜拳[/url]开盘成功(" + Price + "" + bzText + ")";
            new BCW.BLL.Action().Add(16, id, 0, "", wText);

            Utils.Success("我要猜拳", "恭喜您开盘成功!<br /><a href=\"" + Utils.getUrl("mora.aspx?act=add") + "\">我要继续猜拳&gt;&gt;</a>", Utils.getUrl("mora.aspx"), "3");
        }
        else
        {
            new BCW.BLL.Guest().Add(2, hid, UsName, "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]邀请您参与TA的猜拳:" + model.Title + "，[url=/bbs/game/mora.aspx?act=view&amp;id=" + id + "]立即参与[/url]，[url=/bbs/game/mora.aspx?act=no&amp;id=" + id + "]谢绝[/url]");
            string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/mora.aspx]疯狂猜拳[/url]向" + UsName + "发起挑战(" + Price + "" + bzText + ")";
            new BCW.BLL.Action().Add(16, id, 0, "", wText);
            Utils.Success("我要猜拳", "恭喜您开盘成功!<br />如果对方谢绝猜拳，这个猜拳将更改为公共猜拳<br /><a href=\"" + Utils.getUrl("/bbs/friend.aspx?act=online&amp;backurl=" + Server.UrlEncode("/bbs/game/mora.aspx?act=add") + "") + "\">我要继续对战&gt;&gt;</a>", Utils.getUrl("mora.aspx"), "3");
        }
    }

    /// <summary>
    /// 排行榜
    /// </summary>
    private void TopPage()
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));
        Master.Title = "猜拳排行榜";
        builder.Append(Out.Tab("<div class=\"title\">", ""));

        if (ptype == 1)
            builder.Append("赚币排行|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("mora.aspx?act=top&amp;ptype=1") + "\">赚币排行</a>|");

        if (ptype == 2)
            builder.Append("胜率排行");
        else
            builder.Append("<a href=\"" + Utils.getUrl("mora.aspx?act=top&amp;ptype=2") + "\">胜率排行</a>");
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
        strWhere = "Types=8";
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
        builder.Append("<a href=\"" + Utils.getUrl("mora.aspx") + "\">猜拳</a>");
        builder.Append(Out.Tab("</div>", ""));
    }


    private void RulePage()
    {
        Master.Title = "猜拳游戏规则";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("猜拳游戏规则");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("猜拳是由用户自己开局猜拳与另一个玩家进行心理大战的趣味游戏,游戏过程只允许2个用户参与,开局人自行设定出手类型,时限以及虚拟币数量,开局成功后其他应战用户均可以参与,但只有一名用户可以最终参与并应战,游戏结束后将收取胜利方一定的手续费.<br />");
        builder.Append("【注意事项】<br />");
        builder.Append("-自己开的局不能自己来应答!<br />");
        builder.Append("-两个人猜拳,一拳定输赢<br />");
        builder.Append("-石头胜剪刀,剪刀胜布,布胜石头<br />");
        builder.Append("-游戏结束后收取获胜者" + ub.GetSub("MoraTar1", xmlPath) + "%手续费，如是大局猜拳则收取" + ub.GetSub("MoraTar2", xmlPath) + "%<br />");
        builder.Append("-若双方出拳一样,则打平本金返还,不扣手续费<br />");

        if (ub.GetSub("MoraTNum", xmlPath) != "0")
            builder.Append("-每人每天可以先应战" + ub.GetSub("MoraTNum", xmlPath) + "次,之后必须自己开一局才能再应战" + ub.GetSub("MoraTNum", xmlPath) + "个<br />");

        builder.Append("-猜拳有时间性,过了有效时间,则本金返还,不会扣除手续费<br />");
        builder.Append("-应答赢的猜拳系统已自动兑奖，开局人获胜需点“兑奖”才能领奖<br />");
        builder.Append("-请猜拳者自重!禁止一切侮辱他人或者捏造事实诽谤他人!<br />");
        builder.Append("-请猜拳者自重!任何违反国家相关法律的言论,一旦违反将严肃处理!<br />");
        builder.Append("<a href=\"" + Utils.getUrl("mora.aspx") + "\">马上挑战&gt;&gt;</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("mora.aspx") + "\">猜拳</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void CaseOkPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int pid = Utils.ParseInt(Utils.GetRequest("pid", "get", 2, @"^[0-9]\d*$", "选择兑奖无效"));

        if (new BCW.BLL.Game.Mora().ExistsState(pid, meid))
        {
            BCW.Model.Game.Mora model = new BCW.BLL.Game.Mora().GetMora(pid);
            //操作币
            long winMoney = model.Price;
            //税率
            if (model.State == 6)
            {
                long SysTax = 0;
                int Tax = Utils.ParseInt(ub.GetSub("MoraTar1", xmlPath));
                if (model.Price >= F1)
                    Tax = Utils.ParseInt(ub.GetSub("MoraTar2", xmlPath));

                if (Tax > 0)
                {
                    SysTax = Convert.ToInt64(winMoney * Tax * 0.01);
                }
                winMoney = winMoney + winMoney - SysTax;
            }

            new BCW.BLL.Game.Mora().UpdateState(pid, 2);

            if (model.BzType == 0)
            {
                //if (model.ReID == 0)
                //    new BCW.BLL.User().UpdateiGold(meid, winMoney, 8);
                //else
                new BCW.BLL.User().UpdateiGold(meid, winMoney, "猜拳兑奖-标识ID" + pid + "");

                Utils.Success("兑奖", "恭喜，成功兑奖" + winMoney + "" + ub.Get("SiteBz") + "", Utils.getUrl("mora.aspx?act=case"), "1");
            }
            else
            {
                new BCW.BLL.User().UpdateiMoney(meid, winMoney, "猜拳兑奖-标识ID" + pid + "");
                Utils.Success("兑奖", "恭喜，成功兑奖" + winMoney + "" + ub.Get("SiteBz2") + "", Utils.getUrl("mora.aspx?act=case"), "1");
            }

        }
        else
        {
            Utils.Success("兑奖", "恭喜，重复兑奖或没有可以兑奖的记录", Utils.getUrl("mora.aspx?act=case"), "1");
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
            if (new BCW.BLL.Game.Mora().ExistsState(pid, meid))
            {
                BCW.Model.Game.Mora model = new BCW.BLL.Game.Mora().GetMora(pid);
                //操作币
                long win = model.Price;
                //税率
                if (model.State == 6)
                {
                    long SysTax = 0;
                    int Tax = Utils.ParseInt(ub.GetSub("MoraTar1", xmlPath));
                    if (model.Price >= F1)
                        Tax = Utils.ParseInt(ub.GetSub("MoraTar2", xmlPath));

                    if (Tax > 0)
                    {
                        SysTax = Convert.ToInt64(win * Tax * 0.01);
                    }
                    win = win + win - SysTax;
                }

                new BCW.BLL.Game.Mora().UpdateState(pid, 2);

                if (model.BzType == 0)
                {
                    winMoney += win;
                    //if (model.ReID == 0)
                    //    new BCW.BLL.User().UpdateiGold(meid, win, 8);
                    //else
                    new BCW.BLL.User().UpdateiGold(meid, win, "猜拳兑奖-标识ID" + pid + "");

                }
                else
                {
                    winMoney2 += win;
                    new BCW.BLL.User().UpdateiMoney(meid, win, "猜拳兑奖-标识ID" + pid + "");
                }
            }
        }
        Utils.Success("兑奖", "恭喜，成功兑奖" + winMoney + "" + ub.Get("SiteBz") + "/" + winMoney2 + "" + ub.Get("SiteBz2") + "", Utils.getUrl("mora.aspx?act=case"), "1");
    }

    private void CasePage()
    {
        Master.Title = "兑奖中心";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>&gt;<a href=\"" + Utils.getUrl("mora.aspx") + "\">猜拳</a>&gt;兑奖");
        builder.Append(Out.Tab("</div>", Out.Hr()));

        int pageIndex;
        int recordCount;
        int pageSize = 5;
        string strWhere = string.Empty;
        strWhere = "UsID=" + meid + " and (State=3 or State=5 or State=6)";
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        string arrId = "";
        string strDay = "";
        // 开始读取列表
        IList<BCW.Model.Game.Mora> listMora = new BCW.BLL.Game.Mora().GetMoras(pageIndex, pageSize, strWhere, out recordCount);
        if (listMora.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Mora n in listMora)
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

                if (DT.FormatDate(n.AddTime, 4) != strDay.ToString())
                    builder.Append(DT.FormatDate(n.AddTime, 4) + " 猜拳兑奖<br />");

                builder.Append("出拳:" + MoraType(n.TrueType) + "");
                builder.Append("陷阱:" + n.Title + "<br />");
                builder.Append("猜拳:" + n.Price + "" + bzText + "<br />");
                builder.Append("应战:" + MoraType(n.TrueType) + "<br />");
                builder.Append("应战人:" + n.ReName + "<br />");
                //操作币
                long winMoney = n.Price;
                //税率
                long SysTax = 0;
                int Tax = Utils.ParseInt(ub.GetSub("MoraTar1", xmlPath));
                if (n.Price >= F1)
                    Tax = Utils.ParseInt(ub.GetSub("MoraTar2", xmlPath));

                if (Tax > 0)
                {
                    SysTax = Convert.ToInt64(winMoney * Tax * 0.01);
                }
                winMoney = winMoney + winMoney - SysTax;

                if (n.TrueType == n.ChooseType)
                {
                    builder.Append("打平返还本金" + n.Price + "" + bzText + "");
                }
                else
                {
                    if (n.State == 3)
                        builder.Append("开奖结果:过期返还" + n.Price + "" + bzText + "");
                    else if (n.State == 5)
                        builder.Append("开奖结果:平手返还" + n.Price + "" + bzText + "");
                    else
                        builder.Append("开奖结果:赢" + winMoney + "" + bzText + "");
                }

                builder.Append("<a href=\"" + Utils.getUrl("mora.aspx?act=caseok&amp;pid=" + n.ID + "") + "\">兑奖</a>");
                arrId = arrId + " " + n.ID;
                strDay = DT.FormatDate(n.AddTime, 4);
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
            string strOthe = "本页兑奖,mora.aspx,post,0,red";
            builder.Append(Out.wapform(strName, strValu, strOthe));
        }
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("mora.aspx?act=mylist") + "\">我的猜拳</a>|<a href=\"" + Utils.getUrl("mora.aspx?act=dzlist") + "\">对战史</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("mora.aspx?act=cylist") + "\">猜拳记录&gt;&gt;</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append(" <a href=\"" + Utils.getUrl("mora.aspx") + "\">猜拳</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ViewPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        ExpirDelPage();
        Master.Title = "猜拳";
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
        string info = Utils.GetRequest("info", "post", 1, "", "");
        BCW.Model.Game.Mora model = new BCW.BLL.Game.Mora().GetMora(id);
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
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>&gt;<a href=\"" + Utils.getUrl("mora.aspx") + "\">猜拳</a>&gt;已答");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(DT.FormatDate(model.AddTime, 0) + "<br />");
            builder.Append("开局人:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.UsName + "(" + model.UsID + ")</a><br />");
            builder.Append("陷阱:" + model.Title + "<br />");
            builder.Append("出拳:" + MoraType(model.TrueType) + "<br />");
            builder.Append("猜拳:" + model.Price + "" + bzText + "<br />");

            if (model.ReID > 0 && model.State != 4)
            {
                builder.Append("应战人:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.ReID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.ReName + "(" + model.ReID + ")</a><br />");
                builder.Append("应战:" + MoraType(model.ChooseType) + "<br />");
            
            }
            if (model.State == 1 || model.State == 2)
            {
                if (IsWinType(model.ChooseType, model.TrueType) == 2)
                    builder.Append("结果:打个平手!");
                else if (IsWinType(model.ChooseType, model.TrueType) == 3)
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
            builder.Append("<a href=\"" + Utils.getPage("mora.aspx") + "\">返回上一级</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            if (model.Types == 1)
            {
                if (model.UsID != meid && model.ReID != meid)
                {
                    Utils.Error("不存在的记录", "");
                }
            }
            //操作币
            long winMoney = model.Price;
            //税率
            long SysTax = 0;
            int Tax = Utils.ParseInt(ub.GetSub("MoraTar1", xmlPath));
            if (model.Price >= F1)
                Tax = Utils.ParseInt(ub.GetSub("MoraTar2", xmlPath));

            if (Tax > 0)
            {
                SysTax = Convert.ToInt64(winMoney * Tax * 0.01);
            }
            winMoney = winMoney - SysTax;

            if (info != "ok")
            {
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>&gt;<a href=\"" + Utils.getUrl("mora.aspx") + "\">猜拳</a>&gt;确认");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append(DT.FormatDate(model.AddTime, 0) + "<br />");
                builder.Append("开局人:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.UsName + "</a><br />");
                builder.Append("陷阱:" + model.Title + "<br />");
                builder.Append("猜拳:" + model.Price + "" + bzText + "<br />");
                builder.Append("如果您获胜将得到对方的" + winMoney + "" + bzText + "");
                builder.Append("<br />请您出招:");
                builder.Append(Out.Tab("</div>", "<br />"));
                //strText = "我出:,,,";
                //strName = "ChooseType,id,act,info";
                //strType = "select,hidden,hidden,hidden";
                //strValu = "1'" + id + "'view'ok";
                //strEmpt = "1|剪刀|2|石头|3|布,false,false,false";
                //strIdea = "";
                //strOthe = "确定,mora.aspx,post,0,red";

                //builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

                string strName = "id,act,info";
                string strValu = "" + id + "'view'ok";
                string strOthe = "剪刀|石头|布,mora.aspx,post,0,red|blue|red";

                builder.Append(Out.wapform(strName, strValu, strOthe));
            }
            else
            {
                string ac = Utils.GetRequest("ac", "post", 1, "", "");
                int ChooseType = 1;
                if (Utils.ToSChinese(ac).Contains(Utils.ToSChinese("剪刀")))
                {
                    ChooseType = 1;
                }
                else if (Utils.ToSChinese(ac).Contains(Utils.ToSChinese("石头")))
                {
                    ChooseType = 2;
                }
                else
                {
                    ChooseType = 3;
                }

                //int ChooseType = int.Parse(Utils.GetRequest("ChooseType", "post", 2, @"^[1-3]$", "出手错误"));
                if (model.UsID == meid)
                {
                    Utils.Error("不能应战自己的猜拳", "");
                }
                if (model.State > 0)
                {
                    Utils.Error("该猜拳已经结束", "");
                }
                long gold = 0;
                if (model.BzType == 0)
                {
                    gold = new BCW.BLL.User().GetGold(meid);
                }
                else
                {
                    gold = new BCW.BLL.User().GetMoney(meid);
                }
                if (model.Price > gold)
                {
                    Utils.Error("你的" + bzText + "不足", "");
                }
                if (model.Types == 0)
                {
                    int TNum = Utils.ParseInt(ub.GetSub("MoraTNum", xmlPath));
                    if (TNum > 0)
                    {
                        int TCount2 = new BCW.BLL.Game.Mora().GetCount2(meid);
                        if (TCount2 > TNum)
                        {
                            int TCount = new BCW.BLL.Game.Mora().GetCount(meid);
                            if ((TCount2 - TNum) > Convert.ToInt32(TCount * TNum))
                            {
                                Utils.Error("每人每天可以先应战" + TNum + "次,之后必须自己开盘一次才能再应战" + TNum + "次..<br /><a href=\"" + Utils.getUrl("mora.aspx?act=add") + "\">好的,我要猜拳&gt;&gt;</a>", "");
                            }
                        }
                    }
                }
                //支付安全提示
                string[] p_pageArr = { "act", "info", "ChooseType", "id" };
                BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr);
                //-------------------------输赢分析---------------------------
                if (Utils.GetDomain().Contains("168yy") || Utils.GetDomain().Contains("tl88") || Utils.GetDomain().Contains("127.0.0.6"))
                {
                    int WinTar = Utils.ParseInt(ub.GetSub("MoraWinTar", xmlPath));
                    Random ra = new Random(unchecked((int)DateTime.Now.Ticks));
                    //庄家十赌六赢
                    string ZWinID = ub.GetSub("MoraZWinID", xmlPath);
                    if (ZWinID != "")
                    {
                        if (("#" + ZWinID + "#").Contains("#" + model.UsID + "#"))
                        {
                            int rdNext = ra.Next(1, 1000);
                            if (rdNext <= (1000 - WinTar))
                            {
                                if (IsWinType(ChooseType, model.TrueType) == 3)
                                {
                                    if (ChooseType == 1)
                                        model.TrueType = 3;
                                    else if (ChooseType == 2)
                                        model.TrueType = 1;
                                    else if (ChooseType == 3)
                                        model.TrueType = 2;

                                    //更新庄家出拳
                                    new BCW.BLL.Game.Mora().UpdateTrueType(id, model.TrueType);
                                }
                            }
                            else
                            {
                            
                                if (IsWinType(ChooseType, model.TrueType) != 3)
                                {
                                    if (ChooseType == 1)
                                        model.TrueType = 2;
                                    else if (ChooseType == 2)
                                        model.TrueType = 3;
                                    else if (ChooseType == 3)
                                        model.TrueType = 1;

                                    //更新庄家出拳

                                    new BCW.BLL.Game.Mora().UpdateTrueType(id, model.TrueType);
                                }
                            }

                        }
                    }

                    //客家十赌六赢
                    string WinID = ub.GetSub("MoraWinID", xmlPath);
                    if (WinID != "")
                    {
                        if (("#" + WinID + "#").Contains("#" + meid + "#"))
                        {
                            int rdNext = ra.Next(1, 1000);
                            if (rdNext <= (1000 - WinTar))
                            {
                                if (IsWinType(ChooseType, model.TrueType) == 1)
                                {
                                    if (model.TrueType == 1)
                                        ChooseType = 3;
                                    else if (model.TrueType == 2)
                                        ChooseType = 1;
                                    else if (model.TrueType == 3)
                                        ChooseType = 2;
                                }
                            }
                            else
                            {
                                if (IsWinType(ChooseType, model.TrueType) != 1)
                                {
                                    if (model.TrueType == 1)
                                        ChooseType = 2;
                                    else if (model.TrueType == 2)
                                        ChooseType = 3;
                                    else if (model.TrueType == 3)
                                        ChooseType = 1;
                                }
                            }
                        }
                    }
                }
                //-------------------------输赢分析---------------------------

                //更新猜拳记录
                string mename = new BCW.BLL.User().GetUsName(meid);
                BCW.Model.Game.Mora m = new BCW.Model.Game.Mora();
                m.ID = id;
                m.ReID = meid;
                m.ReName = mename;
                m.ReTime = DateTime.Now;
                m.ChooseType = ChooseType;
                m.State = 1;
                new BCW.BLL.Game.Mora().UpdateState(m);
                if (IsWinType(ChooseType, model.TrueType) == 1)//应战家赢
                {
                    if (model.BzType == 0)
                    {
                        if (model.ReID == 0)
                        {
                            new BCW.BLL.User().UpdateiGold(meid, mename, winMoney, 8);
                            new BCW.BLL.User().UpdateiGoldTop(model.UsID, model.UsName, -model.Price, 8);
                        }
                        else
                            new BCW.BLL.User().UpdateiGold(meid, mename, winMoney, "猜拳消费");
                    }
                    else
                        new BCW.BLL.User().UpdateiMoney(meid, mename, winMoney, "猜拳消费");

                    new BCW.BLL.Guest().Add(1, model.UsID, model.UsName, "您的猜拳:" + model.Title + "已经结束，应战人[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]以" + MoraType(ChooseType) + "战胜你的" + MoraType(model.TrueType) + "，结果你输了！[url=/bbs/game/mora.aspx?act=add]我要继续猜拳[/url]");

                    string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/mora.aspx]猜拳[/url]以" + MoraType(ChooseType) + "战胜了" + model.UsName + "的" + MoraType(model.TrueType) + "(赢" + model.Price + "" + bzText + ")";
                    new BCW.BLL.Action().Add(16, id, 0, "", wText);

                }
                else if (IsWinType(ChooseType, model.TrueType) == 2)
                {

                    new BCW.BLL.Guest().Add(1, model.UsID, model.UsName, "您的猜拳:" + model.Title + "已经结束，应战人[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]以" + MoraType(ChooseType) + "对你的" + MoraType(model.TrueType) + "，打平手返还本金！[url=/bbs/game/mora.aspx?act=case]兑奖[/url]");

                    string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/mora.aspx]猜拳[/url]以" + MoraType(ChooseType) + "对" + model.UsName + "的" + MoraType(model.TrueType) + "，打平手返还本金";
                    new BCW.BLL.Action().Add(16, id, 0, "", wText);
                    new BCW.BLL.Game.Mora().UpdateState(id, 5);

                }
                else if (IsWinType(ChooseType, model.TrueType) == 3)
                {
                    if (model.BzType == 0)
                    {
                        if (model.ReID == 0)
                        {
                            new BCW.BLL.User().UpdateiGold(meid, mename, -model.Price, 8);
                            new BCW.BLL.User().UpdateiGoldTop(model.UsID, model.UsName, winMoney, 8);
                        }
                        else
                            new BCW.BLL.User().UpdateiGold(meid, mename, -model.Price, "猜拳消费");
                    }
                    else
                        new BCW.BLL.User().UpdateiMoney(meid, mename, -model.Price, "猜拳消费");

                    new BCW.BLL.Guest().Add(1, model.UsID, model.UsName, "您的猜拳:" + model.Title + "已经结束，应战人[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]以" + MoraType(ChooseType) + "对你的" + MoraType(model.TrueType) + "，结束你赢了！[url=/bbs/game/mora.aspx?act=case]兑奖[/url]");

                    string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/mora.aspx]猜拳[/url]以" + MoraType(ChooseType) + "负了" + model.UsName + "的" + MoraType(model.TrueType) + "(输" + model.Price + "" + bzText + ")";
                    new BCW.BLL.Action().Add(16, id, 0, "", wText);
                    new BCW.BLL.Game.Mora().UpdateState(id, 6);

                }
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>&gt;<a href=\"" + Utils.getUrl("mora.aspx") + "\">猜拳</a>&gt;结果");
                builder.Append(Out.Tab("</div>", "<br />"));
                if (IsWinType(ChooseType, model.TrueType) == 1)
                    builder.Append("恭喜,你赢了!!<br />");
                else if (IsWinType(ChooseType, model.TrueType) == 2)
                    builder.Append("打个平手!<br />你出的是<br />");
                else if (IsWinType(ChooseType, model.TrueType) == 3)
                    builder.Append("真可惜,你输了!<br />你出的是<br />");

                builder.Append("<img src=\"/Files/sys/game/mora_" + ChooseType + ".gif\" alt=\"load\"/><br />");
                builder.Append("庄家" + model.UsName + "出的是<br />");
                builder.Append("<img src=\"/Files/sys/game/mora_" + model.TrueType + ".gif\" alt=\"load\"/><br />");

                if (IsWinType(ChooseType, model.TrueType) == 1)
                {
                    builder.Append("你赢了" + winMoney + "" + bzText + "!<br />你水平那么高,不如自己也来开个盘?<br /><a href=\"" + Utils.getUrl("mora.aspx?act=add") + "\">好的,我要猜拳&gt;&gt;</a>");
                }
                else if (IsWinType(ChooseType, model.TrueType) == 2)
                    builder.Append("本局你不输不赢!");
                else if (IsWinType(ChooseType, model.TrueType) == 3)
                    builder.Append("你输了" + model.Price + "" + bzText + "!");

            }

            builder.Append(Out.Tab("</div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("您目前自带" + Utils.ConvertGold(new BCW.BLL.User().GetGold(meid)) + "" + ub.Get("SiteBz") + "");
            if (Isbz())
                builder.Append("/" + Utils.ConvertGold(new BCW.BLL.User().GetMoney(meid)) + "" + ub.Get("SiteBz2") + "");

            builder.Append("<br /><a href=\"" + Utils.getPage("mora.aspx") + "\">返回再猜&gt;&gt;</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("mora.aspx") + "\">猜拳</a>");
        builder.Append(Out.Tab("</div>", ""));
    }


    private string MoraType(int Types)
    {
        string p_Val = string.Empty;
        if (Types == 1)
            p_Val = "剪刀";
        else if (Types == 2)
            p_Val = "石头";
        else
            p_Val = "布";

        return p_Val;
    }

    //闲家输与赢
    private int IsWinType(int ChooseType, int TrueType)
    {
        //1剪刀/2石头/3布
        if (TrueType == ChooseType)
            return 2;//平
        else if (ChooseType == 1 && TrueType == 2)
            return 3;//输
        else if (ChooseType == 1 && TrueType == 3)
            return 1;//赢
        else if (ChooseType == 2 && TrueType == 1)
            return 1;//赢
        else if (ChooseType == 2 && TrueType == 3)
            return 3;//输
        else if (ChooseType == 3 && TrueType == 1)
            return 3;//输
        else if (ChooseType == 3 && TrueType == 2)
            return 1;//赢

        return 0;
    }

    private void ListPage()
    {
        Master.Title = "猜拳列表";
        int showtype = Utils.ParseInt(Utils.GetRequest("showtype", "get", 1, @"^[0-1]$", "0"));
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[0-2]$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>&gt;<a href=\"" + Utils.getUrl("mora.aspx") + "\">猜拳</a>&gt;列表");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (Isbz())
        {
            builder.Append("币种:");

            if (showtype == 0)
                builder.Append("" + ub.Get("SiteBz") + "|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("mora.aspx?act=list&amp;ptype=" + ptype + "") + "\">" + ub.Get("SiteBz") + "</a>|");

            if (showtype == 1)
                builder.Append("" + ub.Get("SiteBz2") + "<br />");
            else
                builder.Append("<a href=\"" + Utils.getUrl("mora.aspx?act=list&amp;ptype=" + ptype + "&amp;showtype=1") + "\">" + ub.Get("SiteBz2") + "</a><br />");
        }
        if (ptype == 0)
        {
            builder.Append("=最新猜拳=");
            builder.Append("<a href=\"" + Utils.getUrl("mora.aspx?act=list&amp;ptype=" + ptype + "&amp;showtype=" + showtype + "") + "\">刷新</a>");
        }
        else if (ptype == 1)
        {
            builder.Append("=大局推荐=");
            builder.Append("<a href=\"" + Utils.getUrl("mora.aspx?act=list&amp;ptype=" + ptype + "&amp;showtype=" + showtype + "") + "\">刷新</a>");
        }
        else
        {
            builder.Append("=已应答的猜拳=");
            builder.Append("<a href=\"" + Utils.getUrl("mora.aspx?act=list&amp;showtype=" + showtype + "") + "\">最新</a>");
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = 10;
        string strWhere = "";
        if (ptype == 1)
        {
            if (showtype == 0)
                strWhere += "Types=0 and State=0  and BzType=" + showtype + " and StopTime>='" + DateTime.Now + "' and Price>=" + F1 + "";
            else
                strWhere += "Types=0 State=0 and BzType=" + showtype + " and StopTime>='" + DateTime.Now + "' and Price>=" + F2 + "";
        }
        else if (ptype == 2)
            strWhere += "State>0 and BzType=" + showtype + "";
        else
            strWhere += "Types=0 and State=0 and BzType=" + showtype + "";

        string[] pageValUrl = { "act", "ptype", "showtype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Game.Mora> listMora = new BCW.BLL.Game.Mora().GetMoras(pageIndex, pageSize, strWhere, out recordCount);
        if (listMora.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Mora n in listMora)
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

                builder.AppendFormat("<a href=\"" + Utils.getUrl("mora.aspx?act=view&amp;id={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{1}({2}{3})</a>", n.ID, n.Title, n.Price, bzText);

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
        builder.Append(Out.SysUBB(BCW.User.Users.ShowSpeak(16, "mora.aspx?act=list&amp;ptype=" + ptype + "", 5, 0)));

        builder.Append(Out.Tab("<div>", Out.Hr()));
        if (ptype == 0)
            builder.Append("<a href=\"" + Utils.getUrl("mora.aspx?act=list&amp;ptype=2") + "\">已结束的猜拳&gt;&gt;</a>");
        else
            builder.Append("<a href=\"" + Utils.getUrl("mora.aspx?act=list") + "\">最新猜拳&gt;&gt;</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append(" <a href=\"" + Utils.getUrl("mora.aspx") + "\">猜拳</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void MyListPage()
    {
        Master.Title = "我的猜拳";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        ExpirDelPage();

        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[0-2]$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>&gt;<a href=\"" + Utils.getUrl("mora.aspx") + "\">猜拳</a>&gt;历史");
        builder.Append(Out.Tab("</div>", Out.Hr()));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 0)
            builder.Append("全部|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("mora.aspx?act=mylist") + "\">全部</a>|");

        if (ptype == 1)
            builder.Append("未结束|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("mora.aspx?act=mylist&amp;ptype=1") + "\">未结束</a>|");

        if (ptype == 2)
            builder.Append("已结束");
        else
            builder.Append("<a href=\"" + Utils.getUrl("mora.aspx?act=mylist&amp;ptype=2") + "\">已结束</a>");

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
        IList<BCW.Model.Game.Mora> listMora = new BCW.BLL.Game.Mora().GetMoras(pageIndex, pageSize, strWhere, out recordCount);
        if (listMora.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Mora n in listMora)
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
                builder.Append("出拳:" + MoraType(n.TrueType) + "<br />");
                builder.Append("陷阱:" + n.Title + "<br />");
                builder.Append("应战:" + MoraType(n.ChooseType) + "<br />");
                if (n.ReID > 0 && n.State != 4)
                {
                    if (n.State == 0)
                        builder.Append("等待");

                    builder.Append("对家:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.ReID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.ReName + "(" + n.ReID + ")</a><br />");
                }
                builder.Append("猜拳:" + n.Price + "" + bzText + "<br />");

                if (n.State == 0)
                    builder.Append("开奖结果:未开奖<a href=\"" + Utils.getUrl("mora.aspx?act=del&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[撤销猜拳]</a>");
                else if (n.State == 1 || n.State == 2)
                {
                    if (n.State == 2 && n.ReID == 0)
                    {
                        builder.Append("状态:已返还" + n.Price + "" + bzText + "");
                    }
                    else
                    {
                        if (IsWinType(n.ChooseType, n.TrueType) == 2)
                            builder.Append("开奖结果:平手");
                        else if (IsWinType(n.ChooseType, n.TrueType) == 3)
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
        builder.Append("<a href=\"" + Utils.getUrl("mora.aspx?act=mylist") + "\">我的猜拳</a>|<a href=\"" + Utils.getUrl("mora.aspx?act=dzlist") + "\">对战史</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("mora.aspx?act=cylist") + "\">猜拳记录&gt;&gt;</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append(" <a href=\"" + Utils.getUrl("mora.aspx") + "\">猜拳</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void CyListPage()
    {
        Master.Title = "猜拳记录";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>&gt;<a href=\"" + Utils.getUrl("mora.aspx") + "\">猜拳</a>&gt;记录");
        builder.Append(Out.Tab("</div>", Out.Hr()));
        int pageIndex;
        int recordCount;
        int pageSize = 5;
        string strWhere = string.Empty;
        strWhere = "ReID=" + meid + "";

        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Game.Mora> listMora = new BCW.BLL.Game.Mora().GetMoras(pageIndex, pageSize, strWhere, out recordCount);
        if (listMora.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Mora n in listMora)
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
                builder.Append("出拳:" + MoraType(n.TrueType) + "<br />");
                builder.Append("陷阱:" + n.Title + "<br />");
                builder.Append("开局人:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")</a><br />");
                builder.Append("应战:" + MoraType(n.ChooseType) + "<br />");
                builder.Append("猜拳:" + n.Price + "" + bzText + "<br />");

                if (IsWinType(n.ChooseType, n.TrueType) == 2)
                    builder.Append("开奖结果:平手");
                else if (IsWinType(n.ChooseType, n.TrueType) == 1)
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
        builder.Append("<a href=\"" + Utils.getUrl("mora.aspx?act=mylist") + "\">我的猜拳</a>|<a href=\"" + Utils.getUrl("mora.aspx?act=dzlist") + "\">对战史</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("mora.aspx?act=cylist") + "\">猜拳记录&gt;&gt;</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append(" <a href=\"" + Utils.getUrl("mora.aspx") + "\">猜拳</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void DzListPage()
    {
        Master.Title = "猜拳对战史";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[0-1]$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>&gt;<a href=\"" + Utils.getUrl("mora.aspx") + "\">猜拳</a>&gt;对战史");
        builder.Append(Out.Tab("</div>", Out.Hr()));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 0)
            builder.Append("挑战史|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("mora.aspx?act=dzlist") + "\">挑战史</a>|");

        if (ptype == 1)
            builder.Append("应战史");
        else
            builder.Append("<a href=\"" + Utils.getUrl("mora.aspx?act=dzlist&amp;ptype=1") + "\">应战史</a>");

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
        IList<BCW.Model.Game.Mora> listMora = new BCW.BLL.Game.Mora().GetMoras(pageIndex, pageSize, strWhere, out recordCount);
        if (listMora.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Mora n in listMora)
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
                    builder.Append("开局人:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")</a><br />");

                builder.Append("陷阱:" + n.Title + "<br />");

                if ((ptype == 1 && n.State > 0) || ptype == 0)
                    builder.Append("出拳:" + MoraType(n.TrueType) + "<br />");

                builder.Append("猜拳:" + n.Price + "" + bzText + "<br />");
                if (ptype == 0)
                {
                    if (n.State == 0)
                        builder.Append("等待");

                    builder.Append("应战人:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.ReID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.ReName + "(" + n.ReID + ")</a><br />");
                    if (n.ChooseType > 0)
                        builder.Append("他出拳:" + MoraType(n.ChooseType) + "<br />");
                }
                else
                {
                    if (n.ChooseType > 0)
                        builder.Append("我出拳:" + MoraType(n.ChooseType) + "<br />");
                }
                if (n.State > 0)
                {
                    if (ptype == 0)
                    {
                        if (IsWinType(n.ChooseType, n.TrueType) == 2)
                            builder.Append("开奖结果:平手");
                        else if (IsWinType(n.ChooseType, n.TrueType) == 3)
                            builder.Append("开奖结果:全赢");
                        else
                            builder.Append("开奖结果:全输");
                    }
                    else
                    {
                        if (IsWinType(n.ChooseType, n.TrueType) == 2)
                            builder.Append("开奖结果:平手");
                        else if (IsWinType(n.ChooseType, n.TrueType) == 1)
                            builder.Append("开奖结果:全赢");
                        else
                            builder.Append("开奖结果:全输");
                    }
                }
                else
                {
                    if (ptype == 0)
                        builder.Append("<a href=\"" + Utils.getUrl("mora.aspx?act=del&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[撤销猜拳]</a>");
                    else
                    {
                        builder.Append("<a href=\"" + Utils.getUrl("mora.aspx?act=view&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[应战]</a>");
                        builder.Append("<a href=\"" + Utils.getUrl("mora.aspx?act=no&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[谢绝]</a>");
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
        builder.Append("<a href=\"" + Utils.getUrl("mora.aspx?act=mylist") + "\">我的猜拳</a>|<a href=\"" + Utils.getUrl("mora.aspx?act=dzlist") + "\">对战史</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("mora.aspx?act=cylist") + "\">猜拳记录&gt;&gt;</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append(" <a href=\"" + Utils.getUrl("mora.aspx") + "\">猜拳</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void DelPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        string info = Utils.GetRequest("info", "get", 1, "", "");
        BCW.Model.Game.Mora model = new BCW.BLL.Game.Mora().GetMora(id);
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
            Utils.Error("该猜拳已经删除或者已经结束.", "");
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
                new BCW.BLL.User().UpdateiGold(meid, model.UsName, Price, "撤销猜拳得到");
            else
                new BCW.BLL.User().UpdateiMoney(meid, model.UsName, Price, "撤销猜拳得到");

            new BCW.BLL.Game.Mora().UpdateState(id, 4);

            Utils.Success("撤销猜拳", "撤销猜拳成功，返还" + Price + "" + bzText + "", Utils.getPage("mora.aspx?act=mylist"), "1");

        }
        else
        {
            Master.Title = "猜拳";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>&gt;<a href=\"" + Utils.getUrl("mora.aspx") + "\">猜拳</a>&gt;撤销");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("您的出拳:" + MoraType(model.TrueType) + "<br />");
            builder.Append("陷阱:" + model.Title + "");
            builder.Append("<br />猜拳:" + model.Price + "" + bzText + ",如果取消将自动返还您" + Price + "" + bzText + ".");
            builder.Append("<br /><a href=\"" + Utils.getUrl("mora.aspx?act=del&amp;info=ok&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">我确定撤销</a>");
            builder.Append("<br /><a href=\"" + Utils.getPage("mora.aspx") + "\">再考虑一下&gt;&gt;</a>");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("mora.aspx") + "\">猜拳</a>");
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
        BCW.Model.Game.Mora model = new BCW.BLL.Game.Mora().GetMora(id);
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
            Utils.Error("该猜拳已经删除或者已经结束.", "");
        }
        if (info == "ok")
        {
            new BCW.BLL.Game.Mora().UpdateState2(id);
            new BCW.BLL.Guest().Add(1, model.UsID, model.UsName, "您对[url=/bbs/uinfo.aspx?uid=" + meid + "]" + new BCW.BLL.User().GetUsName(meid) + "[/url]发起的猜拳:" + model.Title + "已经被谢绝，系统已将此猜拳自动更改为公共猜拳！[url=/bbs/game/mora.aspx?act=view&amp;id=" + id + "]查看详情[/url]");
            Utils.Success("谢绝猜拳", "谢绝猜拳成功", Utils.getPage("mora.aspx?act=dzlist&amp;ptype=1"), "1");
        }
        else
        {
            Master.Title = "谢绝猜拳";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>&gt;<a href=\"" + Utils.getUrl("mora.aspx") + "\">猜拳</a>&gt;谢绝");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("开局人:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.UsName + "(" + model.UsID + ")</a><br />");
            builder.Append("陷阱:" + model.Title + "");

            string bzText = string.Empty;
            if (model.BzType == 0)
                bzText = ub.Get("SiteBz");
            else
                bzText = ub.Get("SiteBz2");

            builder.Append("<br />猜拳:" + model.Price + "" + bzText + ",如果谢绝猜拳，这个猜拳将更改为公共猜拳.");
            builder.Append("<br /><a href=\"" + Utils.getUrl("mora.aspx?act=no&amp;info=ok&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">我确定不应战</a>");
            builder.Append("<br /><a href=\"" + Utils.getPage("mora.aspx") + "\">再考虑一下&gt;&gt;</a>");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("mora.aspx") + "\">猜拳</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private bool Isbz()
    {
        return false;

        //if (Utils.GetDomain().Contains("kubao") || Utils.GetDomain().Contains("tuhao") || Utils.GetDomain().Contains("th") || Utils.GetDomain().Contains("kb288"))
        //    return true;
        //else
        //    return false;
    }

    private void ExpirDelPage()
    {
        DataSet ds = new BCW.BLL.Game.Mora().GetList("ID", "StopTime<'" + DateTime.Now + "' and State=0");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                int id = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                new BCW.BLL.Game.Mora().UpdateState(id, 3);
            }
        }
    }
}
