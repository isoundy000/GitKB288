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
/// 邵广林 20160617 动态添加usid
/// 
/// 姚志光 20160621 修改活跃抽奖控制入口
/// </summary>

public partial class bbs_game_dxdice : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected long F1 = Convert.ToInt64(ub.GetSub("DxdiceF1", "/Controls/dxdice.xml"));
    protected long F2 = Convert.ToInt64(ub.GetSub("DxdiceF2", "/Controls/dxdice.xml"));
    protected string xmlPath = "/Controls/dxdice.xml";
    protected string xmlPath2 = "/Controls/gamezdid.xml";
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
        //int NEWmeid = new BCW.User.Users().GetUsId();
        //if (NEWmeid == 1112 || NEWmeid == 1113)
        //{ }
        //else
        //{
        if (ub.GetSub("DxdiceStatus", xmlPath) == "1")
        {
            Utils.Safe("此游戏");
        }
        //}
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
            case "add":
                AddPage();
                break;
            case "addsave":
                AddSavePage();
                break;
            case "view":
                ViewPage();
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
        Master.Title = ub.GetSub("DxdiceName", xmlPath);
        string Logo = ub.GetSub("DxdiceLogo", xmlPath);
        if (Logo != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<img src=\"" + Logo + "\" alt=\"load\"/>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        //拾物随机
        builder.Append(BCW.User.Game.GiftFlows.ShowGiftFlows(10));

        string Notes = ub.GetSub("DxdiceNotes", xmlPath);
        if (Notes != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.SysUBB(Notes) + "");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;掷骰");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("【大小掷骰】<a href=\"" + Utils.getUrl("dxdice.aspx?act=rule") + "\">规则</a>.<a href=\"" + Utils.getUrl("dxdice.aspx?act=case") + "\">兑奖</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("dxdice.aspx?act=add") + "\">我要掷骰</a>.<a href=\"" + Utils.getUrl("dxdice.aspx?act=mylist") + "\">历史</a>|<a href=\"" + Utils.getUrl("dxdice.aspx?act=cylist") + "\">掷骰记录</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/friend.aspx?act=online&amp;backurl=" + Server.UrlEncode("/bbs/game/dxdice.aspx?act=add") + "") + "\">找人对战</a>.<a href=\"" + Utils.getUrl("dxdice.aspx?act=dzlist") + "\">对战史</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/action.aspx?ptype=18&amp;backurl=" + Utils.PostPage(1) + "") + "\">动态</a><br />");
        builder.Append("今日掷骰总数:" + new BCW.BLL.Game.Dxdice().GetCount() + "个<br />");
        builder.Append("今日掷骰总量:" + new BCW.BLL.Game.Dxdice().GetPrice(0) + "" + ub.Get("SiteBz") + "");
        if (Isbz())
            builder.Append("/" + new BCW.BLL.Game.Dxdice().GetPrice(1) + "" + ub.Get("SiteBz2") + "");

        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("【最新掷骰】<a href=\"" + Utils.getUrl("dxdice.aspx") + "\">刷新</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        // 开始读取掷骰列表
        int SizeNum = 3;
        string strWhere = "Types=0 and state=0 and StopTime>='" + DateTime.Now + "'";
        IList<BCW.Model.Game.Dxdice> listDxdice = new BCW.BLL.Game.Dxdice().GetDxdices(SizeNum, strWhere);
        if (listDxdice.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Dxdice n in listDxdice)
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

                builder.AppendFormat("<a href=\"" + Utils.getUrl("dxdice.aspx?act=view&amp;id={0}") + "\">{1}({2}{3})</a>", n.ID, n.UsName, n.Price, bzText);
                builder.Append(Out.Tab("</div>", ""));
                k++;
            }

        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("dxdice.aspx?act=list") + "\">&gt;&gt;更多</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("dxdice.aspx?act=list&amp;ptype=2") + "\">+已应答的掷骰&gt;&gt;</a>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("【大骰推荐】");
        builder.Append(Out.Tab("</div>", "<br />"));
        SizeNum = 3;
        strWhere = "Types=0 and state=0 and StopTime>='" + DateTime.Now + "' and ((Price>=" + F1 + " and BzType=0) or (Price>=" + F2 + " and BzType=1))";
        listDxdice = new BCW.BLL.Game.Dxdice().GetDxdices(SizeNum, strWhere);
        if (listDxdice.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Dxdice n in listDxdice)
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

                builder.AppendFormat("<a href=\"" + Utils.getUrl("dxdice.aspx?act=view&amp;id={0}") + "\">{1}({2}{3})</a>", n.ID, n.UsName, n.Price, bzText);
                builder.Append(Out.Tab("</div>", ""));
                k++;
            }

            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("dxdice.aspx?act=list&amp;ptype=1") + "\">&gt;&gt;更多大骰</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("★排行榜:<a href=\"" + Utils.getUrl("dxdice.aspx?act=top&amp;ptype=1") + "\">赚币</a>|<a href=\"" + Utils.getUrl("dxdice.aspx?act=top&amp;ptype=2") + "\">胜率</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));

        DataSet ds = new BCW.BLL.Toplist().GetList("Top 1 UsID,WinGold,PutGold", "Types=10 Order by ((-PutGold)+WinGold) Desc");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            builder.Append("№骰王：<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + ds.Tables[0].Rows[0]["UsID"] + "") + "\">" + BCW.User.Users.SetUser(int.Parse(ds.Tables[0].Rows[0]["UsID"].ToString())) + "</a>参与" + (Convert.ToInt64(ds.Tables[0].Rows[0]["WinGold"]) + (-Convert.ToInt64(ds.Tables[0].Rows[0]["PutGold"]))) + "" + ub.Get("SiteBz") + "<br />");
        }
        else
            builder.Append("№骰王：暂无记录<br />");

        ds = new BCW.BLL.Toplist().GetList("Top 1 UsID,WinGold,PutGold", "Types=10 Order by (WinGold+PutGold) Desc");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            builder.Append("№骰总：<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + ds.Tables[0].Rows[0]["UsID"] + "") + "\">" + BCW.User.Users.SetUser(int.Parse(ds.Tables[0].Rows[0]["UsID"].ToString())) + "</a>净赚" + (Convert.ToInt64(ds.Tables[0].Rows[0]["WinGold"]) + Convert.ToInt64(ds.Tables[0].Rows[0]["PutGold"])) + "" + ub.Get("SiteBz") + "<br />");
        }
        else
            builder.Append("№骰总：暂无记录<br />");


        ds = new BCW.BLL.Toplist().GetList("Top 1 UsID,WinNum,PutNum", "Types=10 Order by (WinNum-PutNum) Desc");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            builder.Append("№骰胜：<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + ds.Tables[0].Rows[0]["UsID"] + "") + "\">" + BCW.User.Users.SetUser(int.Parse(ds.Tables[0].Rows[0]["UsID"].ToString())) + "</a>净胜" + (Convert.ToInt64(ds.Tables[0].Rows[0]["WinNum"]) - Convert.ToInt64(ds.Tables[0].Rows[0]["PutNum"])) + "场");
        }
        else
            builder.Append("№骰胜：暂无记录");

        builder.Append(Out.Tab("</div>", ""));


        //游戏底部Ubb
        string Foot = ub.GetSub("DxdiceFoot", xmlPath);
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
        Master.Title = "我要掷骰";
        //拾物随机
        builder.Append(BCW.User.Game.GiftFlows.ShowGiftFlows(10));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>&gt;<a href=\"" + Utils.getUrl("dxdice.aspx") + "\">掷骰</a>&gt;我要掷骰");
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
        strText = "有效时限:,押注金额:/,,";
        strName = "iTime,Price,hid,act";
        strType = "select,num,hidden,hidden";
        strValu = "1'" + ub.GetSub("DxdiceSmallPay", xmlPath) + "'" + hid + "'addsave";
        strEmpt = "1|1小时|2|2小时|4|4小时|6|6小时|12|12小时|24|1天|48|2天,false,false,false";
        strIdea = "/";
        if (Isbz())
            strOthe = "押" + ub.Get("SiteBz") + "|押" + ub.Get("SiteBz2") + ",dxdice.aspx,post,0,red|blue";
        else
            strOthe = "押" + ub.Get("SiteBz") + ",dxdice.aspx,post,0,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("您目前自带" + Utils.ConvertGold(new BCW.BLL.User().GetGold(meid)) + "" + ub.Get("SiteBz") + "");
        if (Isbz())
            builder.Append("/" + Utils.ConvertGold(new BCW.BLL.User().GetMoney(meid)) + "" + ub.Get("SiteBz2") + "");

        builder.Append("<br /><a href=\"" + Utils.getPage("dxdice.aspx") + "\">再考虑一下&gt;&gt;</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("dxdice.aspx") + "\">掷骰</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void AddSavePage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string ac = Utils.GetRequest("ac", "post", 1, "", "");
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

        long Price = Int64.Parse(Utils.GetRequest("Price", "post", 4, @"^[0-9]\d*$", "押注金额错误"));

        int bzType = 0;
        string bzText = string.Empty;
        long gold = 0;
        if (Utils.ToSChinese(ac).Contains(Utils.ToSChinese(ub.Get("SiteBz2"))))
        {
            bzType = 1;
            bzText = ub.Get("SiteBz2");
            gold = new BCW.BLL.User().GetMoney(meid);
            if (Price < Convert.ToInt64(ub.GetSub("DxdiceSmallPay2", xmlPath)) || Price > Convert.ToInt64(ub.GetSub("DxdiceBigPay2", xmlPath)))
            {
                Utils.Error("押注金额限" + ub.GetSub("DxdiceSmallPay2", xmlPath) + "-" + ub.GetSub("DxdiceBigPay2", xmlPath) + "" + bzText + "", "");
            }
        }
        else
        {
            //支付安全提示
            string[] p_pageArr = { "act", "hid", "iTime", "Price", "ac" };
            BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr);

            bzType = 0;
            bzText = ub.Get("SiteBz");
            gold = new BCW.BLL.User().GetGold(meid);
            if (Price < Convert.ToInt64(ub.GetSub("DxdiceSmallPay", xmlPath)) || Price > Convert.ToInt64(ub.GetSub("DxdiceBigPay", xmlPath)))
            {
                Utils.Error("押注金额限" + ub.GetSub("DxdiceSmallPay", xmlPath) + "-" + ub.GetSub("DxdiceBigPay", xmlPath) + "" + bzText + "", "");
            }
        }
        if (Price > gold)
        {
            Utils.Error("你的" + bzText + "不足", "");
        }

        //是否刷屏
        string appName = "LIGHT_DXDICE";
        int Expir = Utils.ParseInt(ub.GetSub("DxdiceExpir", xmlPath));
        BCW.User.Users.IsFresh(appName, Expir);

        string mename = new BCW.BLL.User().GetUsName(meid);
        if (bzType == 0)
        {
            new BCW.BLL.User().UpdateiGold(meid, mename, -Price, "掷骰消费");
        }
        else
        {
            new BCW.BLL.User().UpdateiMoney(meid, mename, -Price, "掷骰消费");
        }

        Random rd = new Random(unchecked((int)DateTime.Now.Ticks));
        int A = rd.Next(1, 7);
        int B = rd.Next(1, 7);
        //int C = rd.Next(1, 1000);
        string OutText = string.Empty;
        //if (A <= 2 || B <= 2)//1和2不显示出来
        //{
        //    C = 1;
        //}
        //if (hid > 0)
        //{
        //    OutText = "骰子已掷好，等TA来应战吧!";
        //}
        //else
        //{
        //    if (C <= 400)
        //    {
        //        OutText = "你偷偷的瞄了一眼,哎呀!怎么什么都没看到!";
        //    }
        //else if (C == 2)
        //{
        //    OutText = "你偷偷的瞄了一眼,看见一个骰子是<img src=\"/files/sys/game/dxdice_" + A + ".gif\" alt=\"load\"/>点.哎呀!还有一个骰子被挡住了.";
        //}
        //else if (C == 3)
        //{
        //    OutText = "哈哈!老天开眼,我终于看见点数了.<br /><img src=\"/files/sys/game/dxdice_" + A + ".gif\" alt=\"load\"/><img src=\"/files/sys/game/dxdice_" + B + ".gif\" alt=\"load\"/>";
        //}
        //    else
        //    {
        //        if ((A == 1 || A == 4) && (B == 1 || B == 4))
        //        {
        //            OutText = "你偷偷的瞄了一眼,看见骰子两个都是红色!";
        //        }
        //        else if ((A != 1 && A != 4) && (B != 1 && B != 4))
        //        {
        //            OutText = "你偷偷的瞄了一眼,看见骰子两个都是黑色!";
        //        }
        //        else if ((A == 1 || A == 4) && (B != 1 && B != 4))
        //        {
        //            OutText = "你偷偷的瞄了一眼,看见两个骰子一红一黑!";
        //        }
        //        else if ((A != 1 && A != 4) && (B == 1 || B == 4))
        //        {
        //            OutText = "你偷偷的瞄了一眼,看见两个骰子一黑一红!";
        //        }
        //        else
        //        {
        //            OutText = "你偷偷的瞄了一眼,哎呀!怎么什么都没看到!";
        //        }
        //    }
        //}


        OutText = "骰子已掷好，等别人来应战吧!";

        BCW.Model.Game.Dxdice model = new BCW.Model.Game.Dxdice();

        if (hid == 0)
            model.Types = 0;
        else
            model.Types = 1;

        model.DxdiceA = A + "#" + B;
        model.DxdiceB = "";
        model.StopTime = DateTime.Now.AddHours(iTime);
        model.UsID = meid;
        model.UsName = mename;
        model.AddTime = DateTime.Now;
        model.ReID = hid;
        model.ReName = UsName;
        model.Price = Price;
        model.IsWin = 0;
        model.State = 0;
        model.BzType = bzType;
        int id = new BCW.BLL.Game.Dxdice().Add(model);

        Master.Title = "我要掷骰";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("恭喜您开盘成功");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        if (hid == 0)
        {
            string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/dxdice.aspx]大小掷骰[/url]开盘成功(" + Price + "" + bzText + ")";
            new BCW.BLL.Action().Add(18, id, meid, "", wText);

            builder.Append(OutText + "<br /><a href=\"" + Utils.getUrl("dxdice.aspx?act=add") + "\">我要继续掷骰&gt;&gt;</a>");
        }
        else
        {
            new BCW.BLL.Guest().Add(2, hid, UsName, "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]邀请您参与TA的掷骰，[url=/bbs/game/dxdice.aspx?act=view&amp;id=" + id + "]立即参与[/url]，[url=/bbs/game/dxdice.aspx?act=no&amp;id=" + id + "]谢绝[/url]");
            string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/dxdice.aspx]大小掷骰[/url]向" + UsName + "发起挑战(" + Price + "" + bzText + ")";
            new BCW.BLL.Action().Add(18, id, meid, "", wText);
            builder.Append(OutText + "<br />如果对方谢绝掷骰，这个掷骰将更改为公共掷骰<br /><a href=\"" + Utils.getUrl("/bbs/friend.aspx?act=online&amp;backurl=" + Server.UrlEncode("/bbs/game/dxdice.aspx?act=add") + "") + "\">我要继续对战&gt;&gt;</a>");

        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("dxdice.aspx") + "\">掷骰</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ViewPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        ExpirDelPage();
        Master.Title = "掷骰";
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-2]$", "0"));
        BCW.Model.Game.Dxdice model = new BCW.BLL.Game.Dxdice().GetDxdice(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        int zDiceNum = (Utils.ParseInt(Utils.Left(model.DxdiceA, 1)) + Utils.ParseInt(Utils.Right(model.DxdiceA, 1)));
        int kDiceNum = 0;
        string bzText = string.Empty;
        if (model.BzType == 0)
            bzText = ub.Get("SiteBz");
        else
            bzText = ub.Get("SiteBz2");

        if (model.State > 0)
        {
            kDiceNum = (Utils.ParseInt(Utils.Left(model.DxdiceB, 1)) + Utils.ParseInt(Utils.Right(model.DxdiceB, 1)));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("=掷骰战况=<br />");
            builder.Append(DT.FormatDate(model.AddTime, 0) + "<br />");
            builder.Append("庄家:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.UsName + "(" + model.UsID + ")</a><br />");
            builder.Append("庄家掷骰:" + zDiceNum + "点(" + model.DxdiceA + ")<br />");
            builder.Append("掷骰:" + model.Price + "" + bzText + "<br />");

            if (model.ReID > 0 && model.State != 4)
            {
                builder.Append("闲家:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.ReID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.ReName + "(" + model.ReID + ")</a><br />");
                builder.Append("闲家掷骰:" + kDiceNum + "点(" + model.DxdiceB + ")<br />");
            }
            if (model.State == 1 || model.State == 2)
            {
                builder.Append("开奖结果:");
                int iWin = GetiWin(model.DxdiceA, model.DxdiceB);
                if (iWin == 1)
                {
                    builder.Append("庄家全赢");
                }
                else if (iWin == 2)
                {
                    builder.Append("双方打和");
                }
                else if (iWin == 3)
                {
                    builder.Append("闲家全赢");
                }
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
            builder.Append("<a href=\"" + Utils.getPage("dxdice.aspx") + "\">返回上一级</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            //拾物随机
            builder.Append(BCW.User.Game.GiftFlows.ShowGiftFlows(10));

            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>&gt;<a href=\"" + Utils.getUrl("dxdice.aspx") + "\">掷骰</a>&gt;应答");
            builder.Append(Out.Tab("</div>", "<br />"));

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
            int Tax = Utils.ParseInt(ub.GetSub("DxdiceTar", xmlPath));

            if (Tax > 0)
            {
                SysTax = Convert.ToInt64(winMoney * Tax * 0.01);
            }
            winMoney = winMoney - SysTax;

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("=掷骰挑战=<br />");
            builder.Append(DT.FormatDate(model.AddTime, 0) + "<br />");
            if (ptype == 0)
            {
                builder.Append("庄家:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.UsName + "</a><br />");
                builder.Append("掷骰:" + model.Price + "" + bzText + "<br />");
                builder.Append("确认要和他来玩一把?<br />");
                builder.Append("你掷骰:" + model.Price + "" + bzText + ",如果赢将返还您" + (model.Price + winMoney) + "" + bzText + ",打平全额返还,掷骰系统自动扣除赢家" + ub.GetSub("DxdiceTar", xmlPath) + "%的掷骰税.如果您明确掷骰规则,请确认掷骰.<br />");
                builder.Append("<a href=\"" + Utils.getUrl("dxdice.aspx?act=view&amp;id=" + id + "&amp;ptype=1") + "\">确定掷骰</a>");
            }
            else
            {
                if (model.UsID == meid)
                {
                    Utils.Error("不能挑战自己的掷骰", "");
                }
                long gold = 0;
                if (model.BzType == 0)
                    gold = new BCW.BLL.User().GetGold(meid);
                else
                    gold = new BCW.BLL.User().GetMoney(meid);

                if (model.Price > gold)
                {
                    Utils.Error("你的" + bzText + "不足", "");
                }

                if (model.Types == 0)
                {
                    int TNum = Utils.ParseInt(ub.GetSub("DxdiceTNum", xmlPath));
                    if (TNum > 0)
                    {
                        int TCount2 = new BCW.BLL.Game.Dxdice().GetCount2(meid);
                        if (TCount2 > TNum)
                        {
                            int TCount = new BCW.BLL.Game.Dxdice().GetCount(meid);
                            if ((TCount2 - TNum) > Convert.ToInt32(TCount * TNum))
                            {
                                Utils.Error("每人每天可以先应答" + TNum + "次,之后必须自己开盘一次才能再应答" + TNum + "次..<br /><a href=\"" + Utils.getUrl("dxdice.aspx?act=add") + "\">好的,我要掷骰&gt;&gt;</a>", "");
                            }
                        }
                    }
                }
                //支付安全提示
                string[] p_pageArr = { "act", "id", "ptype" };
                BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr, "get");

                Random rd = new Random(unchecked((int)DateTime.Now.Ticks));
                int C = rd.Next(1, 7);
                int D = rd.Next(1, 7);

                //string[] cdtemp = model.DxdiceA.Split("#".ToCharArray());
                //int C = Convert.ToInt32(cdtemp[0]);
                //int D = Convert.ToInt32(cdtemp[1]);

                int IsWin = 0;

                //赢输比例设置
                if (Utils.GetDomain().Contains("tkss") || Utils.GetDomain().Contains("127.0.0.6"))
                {
                    string KzID = "#" + ub.GetSub("DxdiceKzID", xmlPath) + "#";
                    if (KzID.Contains("#" + model.UsID + "#"))
                    {
                        int DxdiceA = Utils.ParseInt(Utils.Left(model.DxdiceA, 1)) + Utils.ParseInt(Utils.Right(model.DxdiceA, 1));
                        int DxdiceB = C + D;
                        int BL1 = Utils.ParseInt(ub.GetSub("DxdiceBL1", xmlPath));
                        int BL2 = Utils.ParseInt(ub.GetSub("DxdiceBL2", xmlPath));
                        if (BL1 != 0 && BL2 != 0)
                        {
                            int BLsum = BL1 + BL2;
                            int BB = rd.Next(1, BLsum);
                            int i = 0;
                            int mid = 0;
                            if (BB <= BL1)
                            {
                                //赢
                                if (DxdiceA <= DxdiceB)
                                {
                                    while (mid == 0)
                                    {
                                        if (i < 300)
                                        {
                                            C = rd.Next(1, 7);
                                            D = rd.Next(1, 7);
                                            if (DxdiceA <= (C + D))
                                            {
                                                mid = 0;
                                            }
                                            else
                                            {
                                                mid = 1;
                                            }
                                        }
                                        else
                                        {
                                            mid = 1;
                                        }
                                        i++;
                                    }
                                }
                            }
                            else
                            {
                                //输
                                if (DxdiceA > DxdiceB)
                                {
                                    while (mid == 0)
                                    {
                                        if (i < 300)
                                        {
                                            C = rd.Next(1, 7);
                                            D = rd.Next(1, 7);
                                            if (DxdiceA > (C + D))
                                            {
                                                mid = 0;
                                            }
                                            else
                                            {
                                                mid = 1;
                                            }
                                        }
                                        else
                                        {
                                            mid = 1;
                                        }
                                        i++;
                                    }
                                }

                            }
                        }
                    }
                    else if (KzID.Contains("#" + meid + "#"))
                    {
                        int DxdiceA = C + D;
                        int DxdiceB = Utils.ParseInt(Utils.Left(model.DxdiceA, 1)) + Utils.ParseInt(Utils.Right(model.DxdiceA, 1));
                        int BL1 = Utils.ParseInt(ub.GetSub("DxdiceBL1", xmlPath));
                        int BL2 = Utils.ParseInt(ub.GetSub("DxdiceBL2", xmlPath));

                        if (BL1 != 0 && BL2 != 0)
                        {
                            int BLsum = BL1 + BL2;
                            int BB = rd.Next(1, BLsum);
                            int i = 0;
                            int mid = 0;
                            if (BB <= BL1)
                            {
                                //赢
                                if (DxdiceA <= DxdiceB)
                                {
                                    while (mid == 0)
                                    {
                                        if (i < 1000)
                                        {
                                            C = rd.Next(1, 7);
                                            D = rd.Next(1, 7);
                                            if (DxdiceA <= (C + D))
                                            {
                                                mid = 0;
                                            }
                                            else
                                            {
                                                mid = 1;
                                            }
                                        }
                                        else
                                        {
                                            mid = 1;
                                        }
                                        i++;
                                    }
                                }
                            }
                            else
                            {
                                //输
                                if (DxdiceA > DxdiceB)
                                {
                                    while (mid == 0)
                                    {
                                        if (i < 1000)
                                        {
                                            C = rd.Next(1, 7);
                                            D = rd.Next(1, 7);
                                            if (DxdiceA > (C + D))
                                            {
                                                mid = 0;
                                            }
                                            else
                                            {
                                                mid = 1;
                                            }
                                        }
                                        else
                                        {
                                            mid = 1;
                                        }
                                        i++;
                                    }
                                }

                            }
                        }
                    }
                }


                builder.Append("你<img src=\"/files/sys/game/dxdice_" + C + ".gif\" alt=\"load\"/><img src=\"/files/sys/game/dxdice_" + D + ".gif\" alt=\"load\"/><br />");
                builder.Append("庄<img src=\"/files/sys/game/dxdice_" + Utils.Left(model.DxdiceA, 1) + ".gif\" alt=\"load\"/><img src=\"/files/sys/game/dxdice_" + Utils.Right(model.DxdiceA, 1) + ".gif\" alt=\"load\"/><br />");

                builder.Append("你掷了:" + (C + D) + "点<br />");
                builder.Append("庄家" + model.UsName + "掷了" + zDiceNum + "点<br />");
                //庄家是不是机器人
                bool IsRobot = false;
                if (("#" + ub.GetSub("GameZDID", xmlPath2) + "#").Contains("#" + model.UsID + "#"))
                {
                    IsRobot = true;
                }
                string mename = new BCW.BLL.User().GetUsName(meid);
                int iWin = GetiWin(model.DxdiceA, "" + C + "#" + D + "");
                if (iWin == 3)
                {
                    //消费
                    if (model.BzType == 0)
                    {
                        if (model.ReID == 0)
                        {
                            new BCW.BLL.User().UpdateiGold(meid, mename, winMoney, 10);
                            new BCW.BLL.User().UpdateiGoldTop(model.UsID, model.UsName, -model.Price, 10);
                        }
                        else
                            new BCW.BLL.User().UpdateiGold(meid, mename, winMoney, "掷骰消费");
                    }
                    else
                        new BCW.BLL.User().UpdateiMoney(meid, mename, winMoney, "掷骰消费");

                    //内线与动态
                    new BCW.BLL.Guest().Add(1, model.UsID, model.UsName, "您的掷骰已经结束，参与庄家[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]，结果对方掷了" + C + "#" + D + "点战胜了你掷的" + model.DxdiceA + "点！[url=/bbs/game/dxdice.aspx?act=add]我要继续掷骰[/url]");
                    string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/dxdice.aspx]大小掷骰[/url]以" + C + "#" + D + "点战胜了" + model.UsName + "掷的" + model.DxdiceA + "点(赢" + model.Price + "" + bzText + ")";
                    new BCW.BLL.Action().Add(18, id, meid, "", wText);

                    builder.Append("恭喜,您赢了" + model.Price + "" + bzText + "！<br />");
                    builder.Append("你的运气这么好,不如自己也来开个盘?<br />");
                    builder.Append("<a href=\"" + Utils.getUrl("dxdice.aspx?act=add") + "\">好的,我要掷骰&gt;&gt;</a>");

                }
                else if (iWin == 2)
                {
                    IsWin = 2;//打平
                    //内线与动态
                    if (!IsRobot)
                    {
                        new BCW.BLL.Guest().Add(1, model.UsID, model.UsName, "您的掷骰已经结束，参与庄家[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]，结果对方掷了" + C + "#" + D + "点打平了你掷的" + model.DxdiceA + "点！[url=/bbs/game/dxdice.aspx?act=case]马上兑奖[/url]");

                    }
                    else//自动兑换
                    {
                        new BCW.BLL.Game.Dxdice().UpdateState(id, 2);
                        if (model.BzType == 0)
                        {
                            new BCW.BLL.User().UpdateiGold(model.UsID, model.Price, "掷骰消费");
                        }
                        else
                        {
                            new BCW.BLL.User().UpdateiMoney(model.UsID, model.Price, "掷骰消费");
                        }

                    }
                    string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/dxdice.aspx]大小掷骰[/url]以" + C + "#" + D + "点打平" + model.UsName + "掷的" + model.DxdiceA + "点";
                    new BCW.BLL.Action().Add(18, id, meid, "", wText);

                    builder.Append("嘿嘿,本局打和！");
                }
                else
                {
                    IsWin = 1;
                    //消费
                    if (model.BzType == 0)
                    {
                        if (model.ReID == 0)
                        {
                            new BCW.BLL.User().UpdateiGold(meid, mename, -model.Price, 10);
                            new BCW.BLL.User().UpdateiGoldTop(model.UsID, model.UsName, winMoney, 10);
                        }
                        else
                            new BCW.BLL.User().UpdateiGold(meid, mename, -model.Price, "掷骰消费");
                    }
                    else
                        new BCW.BLL.User().UpdateiMoney(meid, mename, -model.Price, "掷骰消费");


                    //内线与动态
                    if (!IsRobot)
                    {
                        new BCW.BLL.Guest().Add(1, model.UsID, model.UsName, "您的掷骰已经结束，参与庄家[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]，结果对方掷了" + C + "#" + D + "点负了你掷的" + model.DxdiceA + "点！[url=/bbs/game/dxdice.aspx?act=case]马上兑奖[/url]");
                    }
                    else//自动兑换
                    {
                        new BCW.BLL.Game.Dxdice().UpdateState(id, 2);
                        if (model.BzType == 0)
                        {
                            new BCW.BLL.User().UpdateiGold(model.UsID, winMoney, "掷骰消费");
                        }
                        else
                        {
                            new BCW.BLL.User().UpdateiMoney(model.UsID, winMoney, "掷骰消费");
                        }

                    }

                    string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/dxdice.aspx]大小掷骰[/url]以" + C + "#" + D + "点负了" + model.UsName + "掷的" + model.DxdiceA + "点(输" + model.Price + "" + bzText + ")";
                    new BCW.BLL.Action().Add(18, id, meid, "", wText);
                    builder.Append("真可惜,你输了" + model.Price + "" + bzText + "！");
                }
                //活跃抽奖入口_20160621姚志光
                try
                {
                    //表中存在大小掷骰记录
                    if (new BCW.BLL.tb_WinnersGame().ExistsGameName("大小掷骰"))
                    {
                        //投注是否大于设定的限额，是则有抽奖机会
                        if (model.Price > new BCW.BLL.tb_WinnersGame().GetPrice("大小掷骰"))
                        {
                            string TextForUbb = (ub.GetSub("TextForUbb", "/Controls/winners.xml"));//设置内线提示的文字
                            string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", "/Controls/winners.xml"));//1发内线2不发内线 
                            int hit = new BCW.winners.winners().CheckActionForAll(1, 1, meid, mename, "大小掷骰", 3);
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

                //更新掷骰记录
                BCW.Model.Game.Dxdice m = new BCW.Model.Game.Dxdice();
                m.ID = id;
                m.ReID = meid;
                m.DxdiceB = C + "#" + D;
                m.ReName = mename;
                m.ReTime = DateTime.Now;
                m.IsWin = IsWin;
                m.State = 1;
                new BCW.BLL.Game.Dxdice().UpdateState(m);
            }

            builder.Append(Out.Tab("</div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("您目前自带" + Utils.ConvertGold(new BCW.BLL.User().GetGold(meid)) + "" + ub.Get("SiteBz") + "");
            if (Isbz())
                builder.Append("/" + Utils.ConvertGold(new BCW.BLL.User().GetMoney(meid)) + "" + ub.Get("SiteBz2") + "");

            builder.Append("<br /><a href=\"" + Utils.getPage("dxdice.aspx") + "\">返回再掷骰&gt;&gt;</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("dxdice.aspx") + "\">掷骰</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void RulePage()
    {
        Master.Title = "掷骰游戏规则";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("掷骰游戏规则");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("大小掷骰是由用户自己开骰比大小的趣味游戏,游戏过程只允许2个用户参与,开骰人自行设定时限以及" + ub.Get("SiteBz") + "数量,开骰成功后其他应答用户均可以参与互比大小,但只有一名用户可以最终参与,币数为开局人开局时所设定参与的币数,获胜的一方将支付一定的掷骰税.<br />");
        builder.Append("【注意事项】<br />");
        builder.Append("1点和4点为红色,2点3点5点6点为黑色<br />");
        builder.Append("-两个人掷出的骰子比大小<br />");
        builder.Append("-点数大的一方获胜<br />");
        builder.Append("-点数相同时,单个骰子大的获胜<br />");
        builder.Append("-如两者的骰子组合完全一致,则判不分胜负,本金返还<br />");
        builder.Append("-自己开的骰不能自己来比大小<br />");
        if (ub.GetSub("DxdiceTNum", xmlPath) != "0")
            builder.Append("-每人每天可以先应答" + ub.GetSub("DxdiceTNum", xmlPath) + "次,之后必须自己开盘一次才能再应答" + ub.GetSub("DxdiceTNum", xmlPath) + "次<br />");

        builder.Append("-开骰有时间性,过了有效时间,则本金返还,不会扣除掷骰税<br />");
        builder.Append("-掷骰结束后将收取获胜方" + ub.GetSub("DxdiceTar", xmlPath) + "%的手续费");

        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("dxdice.aspx") + "\">掷骰</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void CaseOkPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int pid = Utils.ParseInt(Utils.GetRequest("pid", "get", 2, @"^[0-9]\d*$", "选择兑奖无效"));

        if (new BCW.BLL.Game.Dxdice().ExistsState(pid, meid))
        {
            BCW.Model.Game.Dxdice model = new BCW.BLL.Game.Dxdice().GetDxdice(pid);
            //操作币
            long winMoney = model.Price;
            //税率
            if (model.State == 1 && model.IsWin == 1)
            {
                long SysTax = 0;
                int Tax = Utils.ParseInt(ub.GetSub("DxdiceTar", xmlPath));

                if (Tax > 0)
                {
                    SysTax = Convert.ToInt64(winMoney * Tax * 0.01);
                }
                winMoney = winMoney + winMoney - SysTax;
            }

            new BCW.BLL.Game.Dxdice().UpdateState(pid, 2);

            if (model.BzType == 0)
            {
                new BCW.BLL.User().UpdateiGold(meid, winMoney, "掷骰兑奖-标识ID" + pid + "");
                Utils.Success("兑奖", "恭喜，成功兑奖" + winMoney + "" + ub.Get("SiteBz") + "", Utils.getUrl("dxdice.aspx?act=case"), "1");
            }
            else
            {
                new BCW.BLL.User().UpdateiMoney(meid, winMoney, "掷骰兑奖-标识ID" + pid + "");
                Utils.Success("兑奖", "恭喜，成功兑奖" + winMoney + "" + ub.Get("SiteBz2") + "", Utils.getUrl("dxdice.aspx?act=case"), "1");
            }

        }
        else
        {
            Utils.Success("兑奖", "恭喜，重复兑奖或没有可以兑奖的记录", Utils.getUrl("dxdice.aspx?act=case"), "1");
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
            if (new BCW.BLL.Game.Dxdice().ExistsState(pid, meid))
            {
                BCW.Model.Game.Dxdice model = new BCW.BLL.Game.Dxdice().GetDxdice(pid);
                //操作币
                long win = model.Price;
                //税率
                if (model.State == 1 && model.IsWin == 1)
                {
                    long SysTax = 0;
                    int Tax = Utils.ParseInt(ub.GetSub("DxdiceTar", xmlPath));

                    if (Tax > 0)
                    {
                        SysTax = Convert.ToInt64(win * Tax * 0.01);
                    }
                    win = win + win - SysTax;
                }

                new BCW.BLL.Game.Dxdice().UpdateState(pid, 2);

                if (model.BzType == 0)
                {
                    winMoney += win;
                    new BCW.BLL.User().UpdateiGold(meid, win, "掷骰兑奖-标识ID" + pid + "");
                }
                else
                {
                    winMoney2 += win;
                    new BCW.BLL.User().UpdateiMoney(meid, win, "掷骰兑奖-标识ID" + pid + "");
                }
            }
        }
        Utils.Success("兑奖", "恭喜，成功兑奖" + winMoney + "" + ub.Get("SiteBz") + "/" + winMoney2 + "" + ub.Get("SiteBz2") + "", Utils.getUrl("dxdice.aspx?act=case"), "1");
    }

    private void CasePage()
    {
        Master.Title = "兑奖中心";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>&gt;<a href=\"" + Utils.getUrl("dxdice.aspx") + "\">掷骰</a>&gt;兑奖");
        builder.Append(Out.Tab("</div>", Out.Hr()));

        int pageIndex;
        int recordCount;
        int pageSize = 5;
        string strWhere = string.Empty;
        strWhere = "UsID=" + meid + " and ((IsWin>0 and State=1) or State=3)";
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        string arrId = "";
        // 开始读取列表
        IList<BCW.Model.Game.Dxdice> listDxdice = new BCW.BLL.Game.Dxdice().GetDxdices(pageIndex, pageSize, strWhere, out recordCount);
        if (listDxdice.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Dxdice n in listDxdice)
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
                if (n.State != 3)
                {
                    builder.Append("闲家:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.ReID + "") + "\">" + n.ReName + "(" + n.ReID + ")</a><br />");
                    builder.Append("闲家掷骰:" + (Utils.ParseInt(Utils.Left(n.DxdiceB, 1)) + Utils.ParseInt(Utils.Right(n.DxdiceB, 1))) + "点(" + n.DxdiceB + ")<br />");
                }
                builder.Append("掷骰:" + n.Price + "" + bzText + "<br />");
                builder.Append("我掷骰:" + (Utils.ParseInt(Utils.Left(n.DxdiceA, 1)) + Utils.ParseInt(Utils.Right(n.DxdiceA, 1))) + "点(" + n.DxdiceA + ")<br />");

                //操作币
                long winMoney = n.Price;
                //税率
                long SysTax = 0;
                int Tax = Utils.ParseInt(ub.GetSub("DxdiceTar", xmlPath));

                if (Tax > 0)
                {
                    SysTax = Convert.ToInt64(winMoney * Tax * 0.01);
                }
                winMoney = winMoney + winMoney - SysTax;

                if (n.State == 3)
                    builder.Append("开奖结果:过期返还" + n.Price + "" + bzText + "");
                else
                {
                    int iWin = GetiWin(n.DxdiceA, n.DxdiceB);
                    if (iWin == 2)
                        builder.Append("开奖结果:打和返还" + n.Price + "" + bzText + "");
                    else
                        builder.Append("开奖结果:全赢返还" + winMoney + "" + bzText + "");
                }
                builder.Append("<a href=\"" + Utils.getUrl("dxdice.aspx?act=caseok&amp;pid=" + n.ID + "") + "\">兑奖</a>");
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
            string strOthe = "本页兑奖,dxdice.aspx,post,0,red";
            builder.Append(Out.wapform(strName, strValu, strOthe));
        }
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("dxdice.aspx?act=mylist") + "\">我的掷骰</a>|<a href=\"" + Utils.getUrl("dxdice.aspx?act=dzlist") + "\">对战史</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("dxdice.aspx?act=cylist") + "\">掷骰记录&gt;&gt;</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("dxdice.aspx") + "\">掷骰</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void MyListPage()
    {
        Master.Title = "我的掷骰";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        ExpirDelPage();

        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[0-2]$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>&gt;<a href=\"" + Utils.getUrl("dxdice.aspx") + "\">掷骰</a>&gt;历史");
        builder.Append(Out.Tab("</div>", Out.Hr()));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 0)
            builder.Append("全部|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("dxdice.aspx?act=mylist") + "\">全部</a>|");

        if (ptype == 1)
            builder.Append("未结束|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("dxdice.aspx?act=mylist&amp;ptype=1") + "\">未结束</a>|");

        if (ptype == 2)
            builder.Append("已结束");
        else
            builder.Append("<a href=\"" + Utils.getUrl("dxdice.aspx?act=mylist&amp;ptype=2") + "\">已结束</a>");

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
        IList<BCW.Model.Game.Dxdice> listDxdice = new BCW.BLL.Game.Dxdice().GetDxdices(pageIndex, pageSize, strWhere, out recordCount);
        if (listDxdice.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Dxdice n in listDxdice)
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
                builder.Append("掷骰:" + n.Price + "" + bzText + "<br />");
                if (n.State > 0)
                    builder.Append("掷骰:" + (Utils.ParseInt(Utils.Left(n.DxdiceA, 1)) + Utils.ParseInt(Utils.Right(n.DxdiceA, 1))) + "点(" + n.DxdiceA + ")<br />");
                else
                    builder.Append("掷骰:xx点(xx,xx)<br />");

                if (n.ReID > 0 && n.State != 4)
                {
                    if (n.State == 0)
                        builder.Append("等待");

                    builder.Append("对家:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.ReID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.ReName + "(" + n.ReID + ")</a><br />");
                    if (n.State == 0)
                        builder.Append("对家掷骰:xx点(xx,xx)<br />");
                    else
                        builder.Append("对家掷骰:" + (Utils.ParseInt(Utils.Left(n.DxdiceB, 1)) + Utils.ParseInt(Utils.Right(n.DxdiceB, 1))) + "点(" + n.DxdiceB + ")<br />");
                }
                if (n.State == 0)
                    builder.Append("开奖结果:未开奖");
                else if (n.State == 1 || n.State == 2)
                {
                    if (n.State == 2 && n.ReID == 0)
                    {
                        builder.Append("状态:已返还" + n.Price + "" + bzText + "");
                    }
                    else
                    {
                        int iWin = GetiWin(n.DxdiceA, n.DxdiceB);
                        if (iWin == 1)
                            builder.Append("开奖结果:全赢");
                        else if (iWin == 2)
                            builder.Append("开奖结果:打和");
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
        builder.Append("<a href=\"" + Utils.getUrl("dxdice.aspx?act=mylist") + "\">我的掷骰</a>|<a href=\"" + Utils.getUrl("dxdice.aspx?act=dzlist") + "\">对战史</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("dxdice.aspx?act=cylist") + "\">掷骰记录&gt;&gt;</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append(" <a href=\"" + Utils.getUrl("dxdice.aspx") + "\">掷骰</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void CyListPage()
    {
        Master.Title = "掷骰记录";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>&gt;<a href=\"" + Utils.getUrl("dxdice.aspx") + "\">掷骰</a>&gt;记录");
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
        IList<BCW.Model.Game.Dxdice> listDxdice = new BCW.BLL.Game.Dxdice().GetDxdices(pageIndex, pageSize, strWhere, out recordCount);
        if (listDxdice.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Dxdice n in listDxdice)
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
                builder.Append("庄家:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.ReID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.ReName + "(" + n.ReID + ")</a><br />");
                builder.Append("庄家掷骰:" + (Utils.ParseInt(Utils.Left(n.DxdiceA, 1)) + Utils.ParseInt(Utils.Right(n.DxdiceA, 1))) + "点(" + n.DxdiceA + ")<br />");
                builder.Append("掷骰:" + n.Price + "" + bzText + "<br />");
                builder.Append("我掷骰:" + (Utils.ParseInt(Utils.Left(n.DxdiceB, 1)) + Utils.ParseInt(Utils.Right(n.DxdiceB, 1))) + "点(" + n.DxdiceB + ")<br />");

                int iWin = GetiWin(n.DxdiceA, n.DxdiceB);
                if (iWin == 3)
                    builder.Append("开奖结果:全赢");
                else if (iWin == 2)
                    builder.Append("开奖结果:打和");
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
        builder.Append("<a href=\"" + Utils.getUrl("dxdice.aspx?act=mylist") + "\">我的掷骰</a>|<a href=\"" + Utils.getUrl("dxdice.aspx?act=dzlist") + "\">对战史</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("dxdice.aspx?act=cylist") + "\">掷骰记录&gt;&gt;</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append(" <a href=\"" + Utils.getUrl("dxdice.aspx") + "\">掷骰</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void DzListPage()
    {
        Master.Title = "掷骰对战史";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[0-1]$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>&gt;<a href=\"" + Utils.getUrl("dxdice.aspx") + "\">掷骰</a>&gt;对战史");
        builder.Append(Out.Tab("</div>", Out.Hr()));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 0)
            builder.Append("挑战史|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("dxdice.aspx?act=dzlist") + "\">挑战史</a>|");

        if (ptype == 1)
            builder.Append("应战史");
        else
            builder.Append("<a href=\"" + Utils.getUrl("dxdice.aspx?act=dzlist&amp;ptype=1") + "\">应战史</a>");

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
        IList<BCW.Model.Game.Dxdice> listDxdice = new BCW.BLL.Game.Dxdice().GetDxdices(pageIndex, pageSize, strWhere, out recordCount);
        if (listDxdice.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Dxdice n in listDxdice)
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
                if (ptype == 0)
                {
                    builder.Append("掷骰:" + n.Price + "" + bzText + "<br />");
                    if (n.State > 0)
                    {
                        builder.Append("我掷骰:" + (Utils.ParseInt(Utils.Left(n.DxdiceA, 1)) + Utils.ParseInt(Utils.Right(n.DxdiceA, 1))) + "点(" + n.DxdiceA + ")<br />");
                    }
                    else
                        builder.Append("我掷骰:xx点(xx,xx)<br />");

                    builder.Append("应战人:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.ReID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.ReName + "(" + n.ReID + ")</a><br />");
                    if (n.State > 0)
                    {
                        builder.Append("应战掷骰:" + (Utils.ParseInt(Utils.Left(n.DxdiceB, 1)) + Utils.ParseInt(Utils.Right(n.DxdiceB, 1))) + "点(" + n.DxdiceB + ")<br />");
                        int iWin = GetiWin(n.DxdiceA, n.DxdiceB);
                        if (iWin == 1)
                            builder.Append("挑战结果:全赢");
                        else if (iWin == 2)
                            builder.Append("挑战结果:打和");
                        else
                            builder.Append("挑战结果:全输");
                    }
                    else
                        builder.Append("挑战结果:等待挑战");
                }
                else
                {
                    builder.Append("庄家:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")</a><br />");
                    if (n.State > 0)
                        builder.Append("庄家掷骰:" + (Utils.ParseInt(Utils.Left(n.DxdiceA, 1)) + Utils.ParseInt(Utils.Right(n.DxdiceA, 1))) + "点(" + n.DxdiceA + ")<br />");
                    else
                        builder.Append("庄家掷骰:xx点(xx,xx)<br />");

                    builder.Append("掷骰:" + n.Price + "" + bzText + "<br />");

                    if (n.State > 0)
                    {
                        builder.Append("我掷骰:" + (Utils.ParseInt(Utils.Left(n.DxdiceB, 1)) + Utils.ParseInt(Utils.Right(n.DxdiceB, 1))) + "点(" + n.DxdiceB + ")<br />");

                        int iWin = GetiWin(n.DxdiceA, n.DxdiceB);
                        if (iWin == 3)
                            builder.Append("挑战结果:全赢");
                        else if (iWin == 2)
                            builder.Append("挑战结果:打和");
                        else
                            builder.Append("挑战结果:全输");
                    }
                    else
                    {
                        builder.Append("我掷骰:等待应战<br />");
                        builder.Append("<a href=\"" + Utils.getUrl("dxdice.aspx?act=view&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[应战]</a>");
                        builder.Append("<a href=\"" + Utils.getUrl("dxdice.aspx?act=no&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[谢绝]</a>");
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
        builder.Append("<a href=\"" + Utils.getUrl("dxdice.aspx?act=mylist") + "\">我的掷骰</a>|<a href=\"" + Utils.getUrl("dxdice.aspx?act=dzlist") + "\">对战史</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("dxdice.aspx?act=cylist") + "\">掷骰记录&gt;&gt;</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append(" <a href=\"" + Utils.getUrl("dxdice.aspx") + "\">掷骰</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void NoPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        string info = Utils.GetRequest("info", "get", 1, "", "");
        BCW.Model.Game.Dxdice model = new BCW.BLL.Game.Dxdice().GetDxdice(id);
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
            Utils.Error("该掷骰已经删除或者已经结束.", "");
        }
        if (info == "ok")
        {
            new BCW.BLL.Game.Dxdice().UpdateState2(id);
            new BCW.BLL.Guest().Add(1, model.UsID, model.UsName, "您对[url=/bbs/uinfo.aspx?uid=" + meid + "]" + new BCW.BLL.User().GetUsName(meid) + "[/url]发起的掷骰已经被谢绝，系统已将此掷骰自动更改为公共掷骰！[url=/bbs/game/dxdice.aspx?act=view&amp;id=" + id + "]查看详情[/url]");
            Utils.Success("谢绝掷骰", "谢绝掷骰成功", Utils.getPage("dxdice.aspx?act=dzlist&amp;ptype=1"), "1");
        }
        else
        {
            Master.Title = "谢绝掷骰";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>&gt;<a href=\"" + Utils.getUrl("dxdice.aspx") + "\">掷骰</a>&gt;谢绝");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("挑战人:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.UsName + "(" + model.UsID + ")</a>");
            string bzText = string.Empty;
            if (model.BzType == 0)
                bzText = ub.Get("SiteBz");
            else
                bzText = ub.Get("SiteBz2");

            builder.Append("<br />掷骰:" + model.Price + "" + bzText + "<br />如果谢绝掷骰，这个掷骰将更改为公共掷骰.");
            builder.Append("<br /><a href=\"" + Utils.getUrl("dxdice.aspx?act=no&amp;info=ok&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定谢绝</a>");
            builder.Append("<br /><a href=\"" + Utils.getPage("dxdice.aspx") + "\">再考虑一下&gt;&gt;</a>");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("dxdice.aspx") + "\">掷骰</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    /// <summary>
    /// 排行榜
    /// </summary>
    private void TopPage()
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));
        Master.Title = "大小掷骰排行榜";
        builder.Append(Out.Tab("<div class=\"title\">", ""));

        if (ptype == 1)
            builder.Append("赚币排行|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("dxdice.aspx?act=top&amp;ptype=1") + "\">赚币排行</a>|");

        if (ptype == 2)
            builder.Append("胜率排行");
        else
            builder.Append("<a href=\"" + Utils.getUrl("dxdice.aspx?act=top&amp;ptype=2") + "\">胜率排行</a>");
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
        strWhere = "Types=10";
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
        builder.Append("<a href=\"" + Utils.getUrl("dxdice.aspx") + "\">掷骰</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ListPage()
    {
        Master.Title = "掷骰列表";
        int showtype = Utils.ParseInt(Utils.GetRequest("showtype", "get", 1, @"^[0-1]$", "0"));
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[0-3]$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大厅</a>&gt;<a href=\"" + Utils.getUrl("dxdice.aspx") + "\">掷骰</a>&gt;列表");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (Isbz())
        {
            builder.Append("币种:");

            if (showtype == 0)
                builder.Append("" + ub.Get("SiteBz") + "|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("dxdice.aspx?act=list&amp;ptype=" + ptype + "") + "\">" + ub.Get("SiteBz") + "</a>|");

            if (showtype == 1)
                builder.Append("" + ub.Get("SiteBz2") + "<br />");
            else
                builder.Append("<a href=\"" + Utils.getUrl("dxdice.aspx?act=list&amp;ptype=" + ptype + "&amp;showtype=1") + "\">" + ub.Get("SiteBz2") + "</a><br />");
        }
        if (ptype == 0)
        {
            builder.Append("=最新掷骰=");
            builder.Append("<a href=\"" + Utils.getUrl("dxdice.aspx?act=list&amp;ptype=" + ptype + "&amp;showtype=" + showtype + "") + "\">刷新</a>");
        }
        else if (ptype == 1)
        {
            builder.Append("=大骰推荐=");
            builder.Append("<a href=\"" + Utils.getUrl("dxdice.aspx?act=list&amp;ptype=" + ptype + "&amp;showtype=" + showtype + "") + "\">刷新</a>");
        }
        else
        {
            builder.Append("=已应答的掷骰=");
            builder.Append("<a href=\"" + Utils.getUrl("dxdice.aspx?act=list&amp;showtype=" + showtype + "") + "\">最新</a>");
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
        IList<BCW.Model.Game.Dxdice> listDxdice = new BCW.BLL.Game.Dxdice().GetDxdices(pageIndex, pageSize, strWhere, out recordCount);
        if (listDxdice.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Dxdice n in listDxdice)
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

                builder.AppendFormat("<a href=\"" + Utils.getUrl("dxdice.aspx?act=view&amp;id={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{1}({2}{3})</a>", n.ID, n.UsName, n.Price, bzText);

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
        builder.Append(Out.SysUBB(BCW.User.Users.ShowSpeak(18, "dxdice.aspx?act=list&amp;ptype=" + ptype + "", 5, 0)));

        builder.Append(Out.Tab("<div>", Out.Hr()));
        if (ptype == 0)
            builder.Append("<a href=\"" + Utils.getUrl("dxdice.aspx?act=list&amp;ptype=2") + "\">已结束的掷骰&gt;&gt;</a>");
        else
            builder.Append("<a href=\"" + Utils.getUrl("dxdice.aspx?act=list") + "\">最新掷骰&gt;&gt;</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append(" <a href=\"" + Utils.getUrl("dxdice.aspx") + "\">掷骰</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private bool Isbz()
    {

        if (Utils.GetDomain().Contains("kubao") || Utils.GetDomain().Contains("tuhao") || Utils.GetDomain().Contains("th") || Utils.GetDomain().Contains("kb288") || Utils.GetDomain().Contains("127.0.0.6"))
            return true;
        else
            return false;
    }

    private void ExpirDelPage()
    {
        DataSet ds = new BCW.BLL.Game.Dxdice().GetList("ID", "StopTime<'" + DateTime.Now + "' and State=0");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                int id = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                new BCW.BLL.Game.Dxdice().UpdateState(id, 3);
            }
        }
    }

    /// <summary>
    /// 1开盘人赢/2打和/3开盘人输
    /// </summary>
    /// <param name="DxdiceA">开盘骰</param>
    /// <param name="DxdiceB">激战骰</param>
    /// <returns></returns>
    private int GetiWin(string DxdiceA, string DxdiceB)
    {
        int diceA1 = Utils.ParseInt(Utils.Left(DxdiceA, 1));
        int diceA2 = Utils.ParseInt(Utils.Right(DxdiceA, 1));
        int diceB1 = Utils.ParseInt(Utils.Left(DxdiceB, 1));
        int diceB2 = Utils.ParseInt(Utils.Right(DxdiceB, 1));
        int diceA = diceA1 + diceA2;
        int diceB = diceB1 + diceB2;
        if (diceA == diceB)
        {
            if (diceA1 == diceB1 || diceA1 == diceB2)
            {
                return 2;
            }
            int[] iNum = { diceA1, diceA2, diceB1, diceB2 };
            int max = Utils.maxNum(iNum);
            if (max == diceA1 || max == diceA2)
            {
                return 1;
            }
            else
            {
                return 3;
            }
        }
        else if (diceA > diceB)
        {
            return 1;
        }
        else
        {
            return 3;
        }

    }
}
