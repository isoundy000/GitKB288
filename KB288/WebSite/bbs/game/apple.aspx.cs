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

public partial class bbs_game_apple : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/apple.xml";
    protected void Page_Load(object sender, EventArgs e)
    {
        //维护提示
        if (ub.GetSub("AppleStatus", xmlPath) == "1")
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
            case "mylist":
                MyListPage();
                break;
            case "list":
                ListPage();
                break;
            case "help":
                HelpPage();
                break;
            case "again":
                AgainPayPage();
                break;
            case "win":
                WinListPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        Master.Title = ub.GetSub("AppleName", xmlPath);
        int meid = new BCW.User.Users().GetUsId();
        string Logo = ub.GetSub("AppleLogo", xmlPath);
        if (Logo != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<img src=\"" + Logo + "\" alt=\"load\"/>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        string Notes = ub.GetSub("AppleNotes", xmlPath);
        if (Notes != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.SysUBB(Notes) + "");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");    
        builder.Append("苹果机");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("你目前自带" + Utils.ConvertGold(new BCW.BLL.User().GetGold(meid)) + "" + ub.Get("SiteBz") + "<br />");
        if (IsOpen() == true)
        {
            new BCW.User.Apple().ApplePage();
            BCW.Model.Game.Applelist apple = new BCW.BLL.Game.Applelist().GetApplelistBQ(0);
            if (apple.ID == 0)
            {
                //第一期开始
                int Minutes = Utils.ParseInt(ub.GetSub("AppleMinutes", xmlPath));
                apple.OpenText = "";
                apple.EndTime = DateTime.Now.AddMinutes(Minutes);
                apple.State = 0;
                apple.ID = new BCW.BLL.Game.Applelist().Add(apple);
            }

            if (apple.EndTime.AddSeconds(-10) > DateTime.Now)
                builder.Append("还有" + DT.DateDiff(apple.EndTime, DateTime.Now) + "开动");
            else
                builder.Append("苹果机正在转动。。。");

            builder.Append("<a href=\"" + Utils.getUrl("apple.aspx") + "\">刷新</a>");
            
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("【我要下注】<a href=\"" + Utils.getUrl("apple.aspx?act=case") + "\">兑奖</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("apple.aspx?act=pay&amp;ptype=1&amp;id=" + apple.ID + "") + "\">下注</a>苹果×" + ub.GetSub("AppleOdds1",xmlPath) + " (" + apple.PingGuo + "注)<br />");
            builder.Append("<a href=\"" + Utils.getUrl("apple.aspx?act=pay&amp;ptype=2&amp;id=" + apple.ID + "") + "\">下注</a>木瓜×" + ub.GetSub("AppleOdds2", xmlPath) + "|小木×" + ub.GetSub("AppleOddsSmall", xmlPath) + " (" + apple.MuGua + "注)<br />");
            builder.Append("<a href=\"" + Utils.getUrl("apple.aspx?act=pay&amp;ptype=3&amp;id=" + apple.ID + "") + "\">下注</a>西瓜×" + ub.GetSub("AppleOdds3", xmlPath) + "|小西×" + ub.GetSub("AppleOddsSmall", xmlPath) + " (" + apple.XiGua + "注)<br />");
            builder.Append("<a href=\"" + Utils.getUrl("apple.aspx?act=pay&amp;ptype=4&amp;id=" + apple.ID + "") + "\">下注</a>芒果×" + ub.GetSub("AppleOdds4", xmlPath) + "|小芒×" + ub.GetSub("AppleOddsSmall", xmlPath) + " (" + apple.MangGuo + "注)<br />");
            builder.Append("<a href=\"" + Utils.getUrl("apple.aspx?act=pay&amp;ptype=5&amp;id=" + apple.ID + "") + "\">下注</a>双星×" + ub.GetSub("AppleOdds5", xmlPath) + "|小星×" + ub.GetSub("AppleOddsSmall", xmlPath) + " (" + apple.ShuangXing + "注)<br />");
            builder.Append("<a href=\"" + Utils.getUrl("apple.aspx?act=pay&amp;ptype=6&amp;id=" + apple.ID + "") + "\">下注</a>金钟×" + ub.GetSub("AppleOdds6", xmlPath) + "|小钟×" + ub.GetSub("AppleOddsSmall", xmlPath) + " (" + apple.JinZhong + "注)<br />");
            builder.Append("<a href=\"" + Utils.getUrl("apple.aspx?act=pay&amp;ptype=7&amp;id=" + apple.ID + "") + "\">下注</a>双七×" + ub.GetSub("AppleOdds7", xmlPath) + "|小七×" + ub.GetSub("AppleOddsSmall", xmlPath) + " (" + apple.ShuangQi + "注)<br />");
            builder.Append("<a href=\"" + Utils.getUrl("apple.aspx?act=pay&amp;ptype=8&amp;id=" + apple.ID + "") + "\">下注</a>元宝×" + ub.GetSub("AppleOdds8", xmlPath) + " (" + apple.YuanBao + "注)");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            //本期下注
            int PayCount = 0;
            int BfPayCount = 0;
            if (meid > 0)
            {
                PayCount = new BCW.BLL.Game.Applepay().GetCount(meid, apple.ID);
            }
            if (PayCount > 0)
                builder.Append("本期您已下注<a href=\"" + Utils.getUrl("apple.aspx?act=mylist&amp;ptype=1") + "\">" + PayCount + "</a>注");
            else
                builder.Append("本期您还没有下注哦.");

            //上期下注
            BfPayCount = new BCW.BLL.Game.Applepay().GetCount(meid, (apple.ID - 1));
            if (BfPayCount > 0)
            {
                builder.Append("<br /><a href=\"" + Utils.getUrl("apple.aspx?act=again&amp;id=" + apple.ID + "") + "\">重压上期投注(" + BfPayCount + "注)</a>");
            }
        }
        BCW.Model.Game.Applelist bfapple = new BCW.BLL.Game.Applelist().GetApplelistBQ(1);
        if (bfapple.OpenText != "")
        {
            builder.Append("<br />上期第" + bfapple.ID + "期开" + bfapple.OpenText + ",<a href=\"" + Utils.getUrl("apple.aspx?act=win&amp;id=" + bfapple.ID + "") + "\">赢家" + bfapple.WinCount + "人</a>获" + bfapple.WinCent + "" + ub.Get("SiteBz") + "");
        }
        builder.Append(Out.Tab("</div>", ""));
        //闲聊显示
        builder.Append(Out.SysUBB(BCW.User.Users.ShowSpeak(17, "apple.aspx", 5, 0)));
        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        builder.Append("【下注记录】<br />");
        builder.Append("<a href=\"" + Utils.getUrl("apple.aspx?act=mylist&amp;ptype=1") + "\">未开下注</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("apple.aspx?act=mylist&amp;ptype=2") + "\">历史下注</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("apple.aspx?act=list") + "\">历史查询</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("apple.aspx?act=help") + "\">游戏规则</a>");
        builder.Append(Out.Tab("</div>", ""));
        //游戏底部Ubb
        string Foot = ub.GetSub("AppleFoot", xmlPath);
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
            Utils.Error("游戏开放时间:" + ub.GetSub("AppleOnTime", xmlPath) + "", "");
        }
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Master.Title = "我要下注";
        Master.Refresh = 3;
        Master.Gourl = Utils.getUrl("apple.aspx");

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("apple.aspx") + "\">苹果机</a>&gt;");
        builder.Append("下注");
        builder.Append(Out.Tab("</div>", ""));
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 2, @"^[1-8]$", "下注类型错误"));
        int num = int.Parse(Utils.GetRequest("num", "get", 1, @"^[1-9]\d*$", "1"));
        if (num > 500)
            num = 500;

        BCW.Model.Game.Applelist apple = new BCW.BLL.Game.Applelist().GetApplelist(id);
        if (apple == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (apple.EndTime.AddSeconds(-10) > DateTime.Now) { }
        else
            Utils.Error("苹果机正在转动。。。", "");

        long Price = Utils.ParseInt64(ub.GetSub("ApplePrice", xmlPath));
        if (num > 1)
            Price = Convert.ToInt64(Price * num);

        long gold = new BCW.BLL.User().GetGold(meid);
        if (gold < Price)
            Utils.Error("您的" + ub.Get("SiteBz") + "不足" + Price + "，无法下注!", "");

        int AppleOutIDNum = Utils.ParseInt(ub.GetSub("AppleOutIDNum", xmlPath));
        int myCount = new BCW.BLL.Game.Applepay().GetCount(meid, id);
        if (AppleOutIDNum < (myCount + num))
        {
            if (AppleOutIDNum < myCount)
                Utils.Error("很遗憾，每ID每期限下注" + AppleOutIDNum + "注", "");
            else
                Utils.Error("很遗憾，每ID每期限下注" + AppleOutIDNum + "注，你还可以下注" + (AppleOutIDNum - myCount - num) + "注", "");
        }
        //支付安全提示
        string[] p_pageArr = { "act", "id", "ptype", "num" };
        BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr, "get");
        //是否刷屏
        //string appName = "LIGHT_APPLE";
        //int Expir = Utils.ParseInt(ub.GetSub("AppleExpir", xmlPath));
        //BCW.User.Users.IsFresh(appName, Expir);

        //扣币
        string mename = new BCW.BLL.User().GetUsName(meid);
        new BCW.BLL.User().UpdateiGold(meid, mename, -Price, "苹果机消费");
        //更新总下注额与某项下注数
        new BCW.BLL.Game.Applelist().Update(id, Price, ptype, num);

        BCW.Model.Game.Applepay model = new BCW.Model.Game.Applepay();
        model.AppleId = id;
        model.PayCount = num;
        model.Types = ptype;
        model.UsID = meid;
        model.UsName = mename;
        model.AddTime = DateTime.Now;
        if (new BCW.BLL.Game.Applepay().Exists(ptype, meid, id))
            new BCW.BLL.Game.Applepay().Update(model);
        else
            new BCW.BLL.Game.Applepay().Add(model);
   
        string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/apple.aspx]苹果机第" + id + "期[/url]下注" + Price + "" + ub.Get("SiteBz") + "";
        new BCW.BLL.Action().Add(17, id, 0, "", wText);

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("下注" + AppleType(ptype) + "成功！花费" + Price + "" + ub.Get("SiteBz") + "<br />");
        builder.Append("您可以再压:<a href=\"" + Utils.getUrl("apple.aspx?act=pay&amp;ptype=" + ptype + "&amp;id=" + id + "&amp;num=1") + "\">1注</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("apple.aspx?act=pay&amp;ptype=" + ptype + "&amp;id=" + id + "&amp;num=5") + "\">5注</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("apple.aspx?act=pay&amp;ptype=" + ptype + "&amp;id=" + id + "&amp;num=10") + "\">10注</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("apple.aspx?act=pay&amp;ptype=" + ptype + "&amp;id=" + id + "&amp;num=50") + "\">50注</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("apple.aspx?act=pay&amp;ptype=" + ptype + "&amp;id=" + id + "&amp;num=100") + "\">100注</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("apple.aspx?act=pay&amp;ptype=" + ptype + "&amp;id=" + id + "&amp;num=200") + "\">200注</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("apple.aspx?act=pay&amp;ptype=" + ptype + "&amp;id=" + id + "&amp;num=500") + "\">500注</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("apple.aspx") + "\">苹果机</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void AgainPayPage()
    {
        if (IsOpen() == false)
        {
            Utils.Error("游戏开放时间:" + ub.GetSub("AppleOnTime", xmlPath) + "", "");
        }
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));

        BCW.Model.Game.Applelist apple = new BCW.BLL.Game.Applelist().GetApplelist(id);
        if (apple == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (apple.EndTime.AddSeconds(-10) > DateTime.Now) { }
        else
            Utils.Error("苹果机正在转动。。。", "");

        int myPayCount = new BCW.BLL.Game.Applepay().GetCount(meid, (id - 1));
        long Price = Utils.ParseInt64(ub.GetSub("ApplePrice", xmlPath));
        long Prices = Convert.ToInt64(Price * myPayCount);
        long gold = new BCW.BLL.User().GetGold(meid);
        if (gold < Prices)
            Utils.Error("您的" + ub.Get("SiteBz") + "不足" + Price + "，无法重压上期下注!", "");

        //支付安全提示
        string[] p_pageArr = { "act", "id" };
        BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr, "get");
        //扣币
        string mename = new BCW.BLL.User().GetUsName(meid);
        new BCW.BLL.User().UpdateiGold(meid, mename, -Prices, "苹果机消费");

        DataSet ds = new BCW.BLL.Game.Applepay().GetList("Types,PayCount", "UsID=" + meid + " and AppleId=" + (id - 1) + " and State=1");
        if (ds == null || ds.Tables[0].Rows.Count == 0)
        {
            Utils.Error("不存在的上期下注记录", "");
        }
        for (int i = 0; i <ds.Tables[0].Rows.Count; i++)
        {
            int Types = int.Parse(ds.Tables[0].Rows[i]["Types"].ToString());
            int PayCount = int.Parse(ds.Tables[0].Rows[i]["PayCount"].ToString());
            //更新总下注额与某项下注数
            new BCW.BLL.Game.Applelist().Update(id, Convert.ToInt64(PayCount*Price), Types, PayCount);
            BCW.Model.Game.Applepay model = new BCW.Model.Game.Applepay();
            model.AppleId = id;
            model.PayCount = PayCount;
            model.Types = Types;
            model.UsID = meid;
            model.UsName = mename;
            model.AddTime = DateTime.Now;
            if (new BCW.BLL.Game.Applepay().Exists(Types, meid, id))
                new BCW.BLL.Game.Applepay().Update(model);
            else
                new BCW.BLL.Game.Applepay().Add(model);
        }

        string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/apple.aspx]苹果机第" + id + "期[/url]下注" + Prices + "" + ub.Get("SiteBz") + "";
        new BCW.BLL.Action().Add(17, id, 0, "", wText);
        Utils.Success("重压上期", "重压上期下注成功，花费" + Prices + "" + ub.Get("SiteBz") + "", Utils.getUrl("apple.aspx"), "2");

    }

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

        string Appleqi = "";
        // 开始读取列表
        IList<BCW.Model.Game.Applepay> listApplepay = new BCW.BLL.Game.Applepay().GetApplepays(pageIndex, pageSize, strWhere, out recordCount);
        if (listApplepay.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Applepay n in listApplepay)
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
                if (n.AppleId.ToString() != Appleqi)
                    builder.Append("=第" + n.AppleId + "期=<br />");

                builder.Append("[" + ((pageIndex - 1) * pageSize + k) + "].");
                if (n.State == 0)
                    builder.Append("买" + AppleType(n.Types) + "，共押" + n.PayCount + "份[" + DT.FormatDate(n.AddTime, 1) + "]");
                else if (n.State == 1)
                {
                    builder.Append("买" + AppleType(n.Types) + "，共押" + n.PayCount + "份[" + DT.FormatDate(n.AddTime, 1) + "]");
                    if (n.WinCent > 0)
                    {
                        builder.Append("赢" + n.WinCent + "" + ub.Get("SiteBz") + "");
                    }
                }
                else
                    builder.Append("买" + AppleType(n.Types) + "，共押" + n.PayCount + "份，赢" + n.WinCent + "" + ub.Get("SiteBz") + "[" + DT.FormatDate(n.AddTime, 1) + "]");

                Appleqi = n.AppleId.ToString();
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
        builder.Append("<a href=\"" + Utils.getUrl("apple.aspx?act=mylist&amp;ptype=1") + "\">未开下注</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("apple.aspx?act=mylist&amp;ptype=2") + "\">历史下注</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("apple.aspx") + "\">苹果机</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void WinListPage()
    {
        int id = Utils.ParseInt(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        string strTitle = "第" + id + "期获奖名单";
        Master.Title = strTitle;
        BCW.Model.Game.Applelist model = new BCW.BLL.Game.Applelist().GetApplelist(id);
        if (model == null || model.State == 0)
        {
            Utils.Error("不存在的记录", "");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(strTitle);
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        strWhere += "AppleId=" + id + " and State>0 and WinCent>0";

        string[] pageValUrl = { "act", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Game.Applepay> listApplepay = new BCW.BLL.Game.Applepay().GetApplepays(pageIndex, pageSize, strWhere, out recordCount);
        if (listApplepay.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Applepay n in listApplepay)
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

                builder.Append("[" + ((pageIndex - 1) * pageSize + k) + "].");

                builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid="+n.UsID+"&amp;backurl="+Utils.PostPage(1)+"") + "\">"+n.UsName+"</a>赢" + n.WinCent + "" + ub.Get("SiteBz") + "[" + DT.FormatDate(n.AddTime, 1) + "]");

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
        builder.Append("<a href=\"" + Utils.getUrl("apple.aspx") + "\">苹果机</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ListPage()
    {
        string strTitle = "历史查询";
        Master.Title = strTitle;

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("近十五期开奖记录");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = 15;
        string strWhere = string.Empty;
        strWhere = "State>0";
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Game.Applelist> listApplelist = new BCW.BLL.Game.Applelist().GetApplelists(pageIndex, pageSize, strWhere, 15, out recordCount);
        if (listApplelist.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Applelist n in listApplelist)
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

                builder.Append("第" + n.ID + "期开奖结果为:");
                builder.Append(n.OpenText);

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
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("apple.aspx") + "\">苹果机</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void CaseOkPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int pid = Utils.ParseInt(Utils.GetRequest("pid", "get", 2, @"^[0-9]\d*$", "选择兑奖无效"));

        if (new BCW.BLL.Game.Applepay().ExistsState(pid, meid))
        {
            new BCW.BLL.Game.Applepay().UpdateState(pid);
            //操作币
            long winMoney = Convert.ToInt64(new BCW.BLL.Game.Applepay().GetWinCent(pid));
            //税率
            long SysTax = 0;
            int Tax = Utils.ParseInt(ub.GetSub("AppleTax", xmlPath));
            if (Tax > 0)
            {
                SysTax = Convert.ToInt64(winMoney * Tax * 0.01);
            }
            winMoney = winMoney - SysTax;
            new BCW.BLL.User().UpdateiGold(meid, winMoney, "苹果机兑奖-标识ID" + pid + "");
            Utils.Success("兑奖", "恭喜，成功兑奖" + winMoney + "" + ub.Get("SiteBz") + "", Utils.getUrl("apple.aspx?act=case"), "1");
        }
        else
        {
            Utils.Success("兑奖", "恭喜，重复兑奖或没有可以兑奖的记录", Utils.getUrl("apple.aspx?act=case"), "1");
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
            if (new BCW.BLL.Game.Applepay().ExistsState(pid, meid))
            {
                new BCW.BLL.Game.Applepay().UpdateState(pid);
                //操作币
                winMoney = Convert.ToInt64(new BCW.BLL.Game.Applepay().GetWinCent(pid));
                //税率
                long SysTax = 0;
                int Tax = Utils.ParseInt(ub.GetSub("AppleTax", xmlPath));
                if (Tax > 0)
                {
                    SysTax = Convert.ToInt64(winMoney * Tax * 0.01);
                }
                winMoney = winMoney - SysTax;

                new BCW.BLL.User().UpdateiGold(meid, winMoney, "苹果机兑奖-标识ID" + pid + "");
            }
            getwinMoney = getwinMoney + winMoney;
        }
        Utils.Success("兑奖", "恭喜，成功兑奖" + getwinMoney + "" + ub.Get("SiteBz") + "", Utils.getUrl("apple.aspx?act=case"), "1");
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
        IList<BCW.Model.Game.Applepay> listApplepay = new BCW.BLL.Game.Applepay().GetApplepays(pageIndex, pageSize, strWhere, out recordCount);
        if (listApplepay.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Applepay n in listApplepay)
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
                builder.Append("[第" + n.AppleId + "期].");
                builder.Append("买" + AppleType(n.Types) + "，押" + n.PayCount + "份，赢" + n.WinCent + "" + ub.Get("SiteBz") + "[" + DT.FormatDate(n.AddTime, 1) + "]<a href=\"" + Utils.getUrl("apple.aspx?act=caseok&amp;pid=" + n.ID + "") + "\">兑奖</a>");

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
            string strOthe = "本页兑奖,apple.aspx,post,0,red";
            builder.Append(Out.wapform(strName, strValu, strOthe));
        }
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("apple.aspx?act=mylist&amp;ptype=1") + "\">未开投注</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("apple.aspx?act=mylist&amp;ptype=2") + "\">历史投注</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("apple.aspx") + "\">苹果机</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    /// <summary>
    /// 游戏玩法规则
    /// </summary>
    private void HelpPage()
    {
        Master.Title = "苹果机游戏规则";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("=苹果机游戏规则=");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("1.苹果机游戏为"+ub.GetSub("AppleMinutes",xmlPath)+"分钟开一期，最后10秒为开机旋转阶段，无法再进行下注<br />");
        builder.Append("2.游戏中共有8个目标可下注，中奖以后各有不同倍数奖励，每注为" + ub.GetSub("ApplePrice", xmlPath) + "金币<br />");
        builder.Append("3.其中木瓜、西瓜、芒果、双星、双七、金钟分大小，开奖为普通的时候为正常倍数，开奖为小则为" + ub.GetSub("AppleOddsSmall", xmlPath) + "倍<br />");
        builder.Append("4.选择重压上期所选可以直接重复跟上期一样的下注，避免更多的操作节约流量");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("apple.aspx") + "\">苹果机</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    //开放时间计算
    private bool IsOpen()
    {
        bool IsOpen = true;
        string OnTime = ub.GetSub("AppleOnTime", xmlPath);
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

    private string AppleType(int Types)
    {
        string AppleName = string.Empty;
        if (Types == 1)
            AppleName = "苹果";
        else if (Types == 2)
            AppleName = "木瓜";
        else if (Types == 3)
            AppleName = "西瓜";
        else if (Types == 4)
            AppleName = "芒果";
        else if (Types == 5)
            AppleName = "双星";
        else if (Types == 6)
            AppleName = "金钟";
        else if (Types == 7)
            AppleName = "双七";
        else if (Types == 8)
            AppleName = "元宝";
        
        return AppleName;
    }
}
