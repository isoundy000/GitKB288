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
///   姚志光 20160621 活跃抽奖入口
/// </summary>
public partial class bbs_game_race : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/race.xml";
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
        if (ub.GetSub("RaceStatus", xmlPath) == "1")
        {
            Utils.Safe("此游戏");
        }
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "rule":
                RulePage();
                break;
            case "del":
                DelPage();
                break;
            case "win":
                WinPage();
                break;
            case "more":
                MorePage();
                break;
            case "my":
                MyPage();
                break;
            case "view":
                ViewPage();
                break;
            case "list":
                ListPage();
                break;
            case "pay":
                PayPage();
                break;
            case "add":
                AddPage();
                break;
            case "save":
                SavePage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        Master.Title = ub.GetSub("RaceName", xmlPath);
        string Logo = ub.GetSub("RaceLogo", xmlPath);
        if (Logo != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<img src=\"" + Logo + "\" alt=\"load\"/>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/game/default.aspx") + "\">游戏大厅</a>&gt;竞拍");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        string Notes = ub.GetSub("RaceNotes", xmlPath);
        if (Notes != "")
            builder.Append(Out.SysUBB(Notes) + "<br />");

        builder.Append("<a href=\"" + Utils.getUrl("race.aspx?act=add") + "\">发布竞拍</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("race.aspx?act=my") + "\">我的竞拍</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("正在进行的竞拍");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        IList<BCW.Model.Game.Race> listRace = new BCW.BLL.Game.Race().GetRaces(5, "paytype=1");
        if (listRace.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Race n in listRace)
            {

                if (k == 1)
                    builder.Append(Out.Tab("<div>", ""));
                else
                    builder.Append(Out.Tab("<div>", Out.Hr()));

                builder.Append("<a href=\"" + Utils.getUrl("race.aspx?act=view&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + k + "." + n.title + "(" + n.payCount + "人竞拍中)</a>");
                if (n.totime > DateTime.Now)
                {
                    builder.Append("<br />剩余时间:" + DT.DateDiff(DateTime.Now, n.totime) + "");
                }
                else
                {
                    builder.Append("<br />剩余时间:已截止");
                }
                if (!string.IsNullOrEmpty(n.fileurl))
                    builder.Append("<br /><img src=\"" + n.fileurl + "\" alt=\"load\"/>");

                builder.Append(Out.Tab("</div>", ""));
                k++;
            }
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("race.aspx?act=more&amp;backurl=" + Utils.PostPage(1) + "") + "\">&gt;&gt;查看更多竞拍</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("</div>", ""));

        //闲聊显示
        builder.Append(Out.SysUBB(BCW.User.Users.ShowSpeak(4, "race.aspx", 5, 0)));

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("race.aspx?act=more&amp;ptype=2") + "\">历史竞拍</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("race.aspx?act=rule") + "\">竞拍规则</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("race.aspx?act=win") + "\">竞拍历史得主</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void AddPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Master.Title = "发布竞拍";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("发布竞拍");
        builder.Append(Out.Tab("</div>", ""));

        strText = "标题(20字内):/,物品描述(支持UBB):/,物品截图地址(可空):/,保密内容(管理员和竞拍得主可见):/,币种类型:/,起拍价:/,截止时间:/,";
        strName = "Title,Content,FileUrl,PContent,Types,Price,Totime,act";
        strType = "text,textarea,text,text,select,num,date,hidden";
        strValu = "''''0''" + DT.FormatDate(DateTime.Now.AddDays(1), 0) + "'save";
        strEmpt = "false,false,true,true,0|" + ub.Get("SiteBz") + "|1|" + ub.Get("SiteBz2") + ",false,false,false";
        strIdea = "/";
        strOthe = "确定发布,race.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("race.aspx?act=my") + "\">返回我的竞拍</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("race.aspx") + "\">竞拍</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void SavePage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string Title = Utils.GetRequest("Title", "post", 2, @"^[\s\S]{1,20}$", "标题限1-20字");
        string Content = Utils.GetRequest("Content", "post", 2, @"^[\s\S]{1,1000}$", "物品描述限1-1000字");
        string FileUrl = Utils.GetRequest("FileUrl", "post", 3, @"^[\s\S]{1,100}$", "截图地址长度限100字符，可留空");
        string PContent = Utils.GetRequest("PContent", "post", 2, @"^[\s\S]{1,300}$", "保密内容限1-300字");
        int Types = int.Parse(Utils.GetRequest("Types", "post", 2, @"^[0-1]$", "币种类型错误"));
        int Price = int.Parse(Utils.GetRequest("Price", "post", 2, @"^[0-9]\d*$", "起拍价填写错误"));
        DateTime Totime = Utils.ParseTime(Utils.GetRequest("Totime", "post", 2, DT.RegexTime, "截止时间格式填写出错,正确格式如" + DT.FormatDate(DateTime.Now, 0) + ""));
        if (Types == 0)
        {
            int Startlow = Utils.ParseInt(ub.GetSub("RaceStartlow", xmlPath));
            int Starthigh = Utils.ParseInt(ub.GetSub("RaceStarthigh", xmlPath));
            if (Price < Startlow || Price > Starthigh)
            {
                Utils.Error("起拍价限" + Startlow + "-" + Starthigh + "" + ub.Get("SiteBz") + "", "");
            }
        }
        else
        {
            int Startlow = Utils.ParseInt(ub.GetSub("RaceStartlow2", xmlPath));
            int Starthigh = Utils.ParseInt(ub.GetSub("RaceStarthigh2", xmlPath));
            if (Price < Startlow || Price > Starthigh)
            {
                Utils.Error("起拍价限" + Startlow + "-" + Starthigh + "" + ub.Get("SiteBz2") + "", "");
            }
        }
        if (Totime < DateTime.Now)
        {
            Utils.Error("截止时间必须大于当时时间", "");
        }
        int DayNum = Utils.ParseInt(ub.GetSub("RaceDayNum", xmlPath));
        int MyDayNum = new BCW.BLL.Game.Race().GetTodayCount(meid);
        if (MyDayNum >= DayNum)
        {
            Utils.Error("每天每ID只可以发布" + DayNum + "个竞拍", "");
        }
        BCW.Model.Game.Race model = new BCW.Model.Game.Race();
        string mename = new BCW.BLL.User().GetUsName(meid);
        model.title = Title;
        model.content = Content;
        model.fileurl = FileUrl;
        model.pcontent = PContent;
        model.price = Price;
        model.topPrice = Price;
        model.totime = Totime;
        model.userid = meid;
        model.username = mename;
        model.writetime = DateTime.Now;
        model.writedate = DateTime.Parse(DateTime.Now.ToLongDateString());
        model.Types = Types;
        model.paytype = 0;
        int id = new BCW.BLL.Game.Race().Add(model);

        string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]发布竞拍[url=/bbs/game/race.aspx?act=view&amp;id=" + id + "]" + Title + "[/url]";
        new BCW.BLL.Action().Add(4, id, 0, "", wText);
        Utils.Success("发布竞拍", "发布成功，请等待管理员审核才会显示出来..", Utils.getUrl("race.aspx"), "1");

    }

    private void MyPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-2]$", "0"));
        Master.Title = "我的竞拍";
        builder.Append(Out.Tab("<div class=\"title\">我的竞拍</div>", ""));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 0)
            builder.Append("未审核|");
        else
            builder.Append(" <a href=\"" + Utils.getUrl("race.aspx?act=my&amp;ptype=0") + "\">未审核</a>|");

        if (ptype == 1)
            builder.Append("进行中|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("race.aspx?act=my&amp;ptype=1") + "\">进行中</a>|");

        if (ptype == 2)
            builder.Append("已完成");
        else
            builder.Append("<a href=\"" + Utils.getUrl("race.aspx?act=my&amp;ptype=2") + "\">已完成</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        if (ptype == 0)
            strWhere = "userid=" + meid + " and paytype=0";
        else if (ptype == 1)
            strWhere = "userid=" + meid + " and paytype=1";
        else
            strWhere = "userid=" + meid + " and paytype=2";

        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Game.Race> listRace = new BCW.BLL.Game.Race().GetRaces(pageIndex, pageSize, strWhere, out recordCount);
        if (listRace.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Race n in listRace)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", Out.Hr()));
                }

                string bzText = string.Empty;
                if (n.Types == 0)
                    bzText = ub.Get("SiteBz");
                else
                    bzText = ub.Get("SiteBz2");

                builder.Append("<a href=\"" + Utils.getUrl("race.aspx?act=view&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.title + "(起拍价" + n.price + "" + bzText + ")</a>");
                if (ptype == 0)
                {
                    builder.Append("未审核");
                }
                else if (ptype == 1)
                {
                    if (n.totime < DateTime.Now)
                        builder.Append("<br />剩余时间:竞拍已截止");
                    else
                        builder.Append("<br />剩余时间:剩余" + DT.DateDiff(DateTime.Now, n.totime) + "");
                }
                else
                {
                    builder.Append("<br />竞拍得主:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.winID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.winName + "</a>");

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
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("race.aspx") + "\">竞拍</a>");
        builder.Append(Out.Tab("</div>", ""));

    }

    private void ViewPage()
    {
        int meid = new BCW.User.Users().GetUsId();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.Game.Race model = new BCW.BLL.Game.Race().GetRace(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        string bzText = string.Empty;
        if (model.Types == 0)
            bzText = ub.Get("SiteBz");
        else
            bzText = ub.Get("SiteBz2");

        Master.Title = "查看竞拍";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(model.title);
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("" + Out.SysUBB(model.content) + "");
        if (meid > 0)
        {
            if (model.paytype == 2 && model.winID == meid)
            {
                builder.Append("<br />=保密内容=<br />" + model.pcontent + "");
            }
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        if (!string.IsNullOrEmpty(model.fileurl))
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<img src=\"" + model.fileurl + "\" alt=\"load\"/>");
            builder.Append("<br /><a href=\"" + Utils.getUrl(model.fileurl) + "\">下载截图</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
     
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("发布时间:" + model.writetime + "");
        if (model.totime < DateTime.Now)
        {
            builder.Append("<br />剩余时间:已截止");
        }
        else
        {
            builder.Append("<br />剩余时间:" + DT.DateDiff(DateTime.Now, model.totime) + "");
        }
        builder.Append("<br />截止时间:" + model.totime + "");
        builder.Append("<br />起拍价:" + model.price + "" + bzText + "");
        builder.Append("<br />发布会员:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.userid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.username + "</a>");
        if (meid > 0)
        {
            if (model.payCount == 0 && model.userid == meid)
            {
                builder.Append("<br /><a href=\"" + Utils.getUrl("race.aspx?act=del&amp;id=" + id + "") + "\">删除竞拍</a>");
            }
        }
        if (model.paytype == 2 && model.winID != 0)
        {
            builder.Append("<br /><b>竞拍最后得主:</b><a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.winID + "") + "\">" + model.winName + "</a>");
        }
        else
        {
            if (model.winID > 0)
            {
                builder.Append("<br /><b>当前最高价:</b>" + model.topPrice + "" + bzText + ",<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.winID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.winName + "</a>");
            }
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        if (model.paytype > 0)
        {
            if (model.paytype == 1 && model.totime > DateTime.Now)
            {
                //增幅
                 long paycents = 0;
                 if (model.Types == 0)
                     paycents = model.topPrice + Convert.ToInt64(ub.GetSub("RaceZfPrice", xmlPath));
                 else
                     paycents = model.topPrice + Convert.ToInt64(ub.GetSub("RaceZfPrice2", xmlPath));

                builder.Append(Out.Tab("<div>", ""));
                long gold = new BCW.BLL.User().GetGold(meid);
                long money = new BCW.BLL.User().GetMoney(meid);
                builder.Append("您目前共有" + Utils.ConvertGold(gold) + "" + ub.Get("SiteBz") + "/" + Utils.ConvertGold(money) + "" + ub.Get("SiteBz2") + "");
                builder.Append(Out.Tab("</div>", "<br />"));
                strText = "出价(须高于最高价和起拍价):/,,";
                strName = "payCent,id,act";
                strType = "stext,hidden,hidden";
                strValu = "" + paycents + "'" + id + "'pay";
                strEmpt = "false,false,false";
                strIdea = "";
                strOthe = "我拍,race.aspx,post,3,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("增幅至少" + (paycents - model.topPrice) + "" + bzText + ",即" + paycents + "" + bzText + "");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("=最新出价=");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            IList<BCW.Model.Game.Racelist> listRacelist = new BCW.BLL.Game.Racelist().GetRacelists(5, "raceid=" + id + "");
            if (listRacelist.Count > 0)
            {
                foreach (BCW.Model.Game.Racelist n in listRacelist)
                {
                    builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.payusid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.payname + "出价" + n.payCent + "" + bzText + "</a>" + DT.DateDiff(DateTime.Now, n.paytime) + "前<br />");
                }
                builder.Append("<a href=\"" + Utils.getUrl("race.aspx?act=list&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">&gt;&gt;更多出价记录</a>");
            }
            else
            {
                builder.Append("没有出价记录..");
            }
            builder.Append(Out.Tab("</div>", ""));
            //显示闲聊
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("=最新留言=");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(BCW.User.Users.ShowSpeak(4, "race.aspx", 5, meid, "view", id));
            builder.Append(Out.Tab("", "<br />"));
        }
        builder.Append(Out.Tab("<div>", Out.RHr()));
        builder.Append("<a href=\"" + Utils.getPage("race.aspx?act=my") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("race.aspx") + "\">竞拍</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void PayPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[1-9]\d*$", "ID错误"));
        long payCent = Int64.Parse(Utils.GetRequest("payCent", "post", 4, @"^[1-9]\d*$", "出价错误"));

        BCW.Model.Game.Race model = new BCW.BLL.Game.Race().GetRace(id);
        if (model.userid == meid)
        {
            Utils.Error("不能竞拍自己的物品", "");
        }
        if (model.paytype != 1)
        {
            Utils.Error("此竞拍未通过审核或已结束", "");
        }
        if (model.totime < DateTime.Now)
        {
            Utils.Error("竞拍已经结束", "");
        }
        string bzText = string.Empty;
        if (model.Types == 0)
            bzText = ub.Get("SiteBz");
        else
            bzText = ub.Get("SiteBz2");

        long payCents = 0;
        if (model.Types == 0)
            payCents = model.topPrice + Utils.ParseInt64(ub.GetSub("RaceZfPrice", xmlPath));
        else
            payCents = model.topPrice + Utils.ParseInt64(ub.GetSub("RaceZfPrice2", xmlPath));

        if (payCent < payCents)
        {
            Utils.Error("出价至少" + payCents + "" + bzText + "", "");
        }
        long gold = 0;
        if (model.Types == 0)
            gold = new BCW.BLL.User().GetGold(meid);
        else
            gold = new BCW.BLL.User().GetMoney(meid);

        if (gold < Convert.ToInt64(payCent))
        {
            Utils.Error("你的" + bzText + "不足", "");
        }
        if (model.winID == meid)
        {
            Utils.Error("你已成功出价", "");
        }
        string mename = new BCW.BLL.User().GetUsName(meid);
        if (model.Types == 0)
        {
            //支付安全提示
            string[] p_pageArr = { "act", "id", "payCent" };
            BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr);
            //扣币
            new BCW.BLL.User().UpdateiGold(meid, mename, -payCent, "竞拍物品");

            //退回上个会员ID的币
            if (model.winID > 0 && model.topPrice > 0)
            {
                new BCW.BLL.User().UpdateiGold(model.winID, model.winName, model.topPrice, "竞拍退回");
                //发内线提示上一个会员ID
                new BCW.BLL.Guest().Add(1, model.winID, model.winName, "[URL=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/URL]以" + payCent + "" + bzText + "竞拍[URL=/bbs/game/race.aspx?act=view&amp;id=" + id + "]" + model.title + "[/URL]，系统将您之前竞拍的" + model.topPrice + "" + bzText + "退回到您的帐上");
            }
        }
        else
        {
            //扣币
            new BCW.BLL.User().UpdateiMoney(meid, mename, -payCent, "竞拍物品");

            //退回上个会员ID的币
            if (model.winID > 0 && model.topPrice > 0)
            {
                new BCW.BLL.User().UpdateiMoney(model.winID, model.winName, model.topPrice, "竞拍退回");
                //发内线提示上一个会员ID
                new BCW.BLL.Guest().Add(1, model.winID, model.winName, "[URL=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/URL]以" + payCent + "" + bzText + "竞拍[URL=/bbs/game/race.aspx?act=view&amp;id=" + id + "]" + model.title + "[/URL]，系统将您之前竞拍的" + model.topPrice + "" + bzText + "退回到您的帐上");
            }
        }
        //写入购买记录
        BCW.Model.Game.Racelist paymodel = new BCW.Model.Game.Racelist();
        paymodel.payname = mename;
        paymodel.payusid = meid;
        paymodel.payCent = payCent;
        paymodel.paytime = DateTime.Now;
        paymodel.raceid = id;
        paymodel.paytype = 0;
        new BCW.BLL.Game.Racelist().Add(paymodel);
        //更新最新报价和会员ID
        new BCW.BLL.Game.Race().UpdatetopPrice(id, payCent, meid, mename, 1);

        //检查结束时间是否少于5分钟
        string sText = string.Empty;
        DateTime stime = model.totime.AddMinutes(5);//加5分钟
        if (DateTime.Now > model.totime.AddMinutes(-5))
        {
            new BCW.BLL.Game.Race().Updatetotime(id, stime);
            sText = "为公平起见，竞拍结束时间延长5分钟";
        }
        string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]以" + payCent + "" + bzText + "竞拍[url=/bbs/game/race.aspx?act=view&amp;id=" + id + "]" + model.title + "[/url]";
        new BCW.BLL.Action().Add(4, id, 0, "", wText);
        //活跃抽奖入口_20160621姚志光
        try
        {
            //表中存在记录
            if (new BCW.BLL.tb_WinnersGame().ExistsGameName("欢乐竞拍"))
            {
                //投注是否大于设定的限额，是则有抽奖机会
                if (payCent > new BCW.BLL.tb_WinnersGame().GetPrice("欢乐竞拍"))
                {
                    string TextForUbb = (ub.GetSub("TextForUbb", "/Controls/winners.xml"));//设置内线提示的文字
                    string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", "/Controls/winners.xml"));//1发内线2不发内线 
                    int hit = new BCW.winners.winners().CheckActionForAll(1, 1, meid, mename, "竞拍", 3);
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
        Utils.Success("出价竞拍", "出价成功!" + sText + "", Utils.getUrl("race.aspx?act=view&amp;id=" + id + ""), "1");
    }

    private void DelPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info != "ok")
        {
            Master.Title = "删除竞拍";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此竞拍记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("race.aspx?info=ok&amp;act=del&amp;id=" + id + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("race.aspx?act=view&amp;id=" + id + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            if (!new BCW.BLL.Game.Race().Exists(id, meid))
            {
                Utils.Error("不存在的记录或竞拍已经有购买记录", "");
            }
            //删除
            new BCW.BLL.Game.Race().Delete(id);

            Utils.Success("删除竞拍", "删除竞拍成功..", Utils.getUrl("race.aspx"), "1");
        }
    }

    private void ListPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
          BCW.Model.Game.Race model = new BCW.BLL.Game.Race().GetRace(id);
        if (model==null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.paytype < 1)
        {
            Utils.Error("此竞拍还没有通过审核", "");
        }
        string bzText = string.Empty;
        if (model.Types == 0)
            bzText = ub.Get("SiteBz");
        else
            bzText = ub.Get("SiteBz2");

        Master.Title = "查看出价记录";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("race.aspx?act=view&amp;id=" + id + "") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        strWhere = "raceid=" + id + "";
        string[] pageValUrl = { "act", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Game.Racelist> listRacelist = new BCW.BLL.Game.Racelist().GetRacelists(pageIndex, pageSize, strWhere, out recordCount);
        if (listRacelist.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Racelist n in listRacelist)
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

                builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.payusid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[" + DT.FormatDate(n.paytime, 6) + "]" + n.payname + "</a>(出价" + n.payCent + "" + bzText + ")");

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
        builder.Append("<a href=\"" + Utils.getUrl("race.aspx?act=view&amp;id=" + id + "") + "\">返回查看竞拍</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("race.aspx") + "\">竞拍</a>");
        builder.Append(Out.Tab("</div>", ""));

    }

    private void MorePage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-2]$", "1"));
        Master.Title = "查看竞拍";
        builder.Append(Out.Tab("<div class=\"title\">查看竞拍</div>", ""));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 1)
            builder.Append("正在竞拍|<a href=\"" + Utils.getUrl("race.aspx?act=more&amp;ptype=2") + "\">历史竞拍</a>");
        else
            builder.Append("<a href=\"" + Utils.getUrl("race.aspx?act=more&amp;ptype=1") + "\">正在竞拍</a>|历史竞拍");

        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        strWhere = "paytype=" + ptype + "";
        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Game.Race> listRace = new BCW.BLL.Game.Race().GetRaces(pageIndex, pageSize, strWhere, out recordCount);
        if (listRace.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Race n in listRace)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", Out.Hr()));
                }
                string bzText = string.Empty;
                if (n.Types == 0)
                    bzText = ub.Get("SiteBz");
                else
                    bzText = ub.Get("SiteBz2");

                builder.Append("<a href=\"" + Utils.getUrl("race.aspx?act=view&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.title + "(起拍价" + n.price + "" + bzText + ")</a>");
                if (ptype == 0)
                {
                    builder.Append("未审核");
                }
                else if (ptype == 1)
                {
                    if (n.totime < DateTime.Now)
                        builder.Append("<br />剩余时间:竞拍已截止");
                    else
                        builder.Append("<br />剩余时间:" + DT.DateDiff(DateTime.Now, n.totime) + "");
                }
                else
                {
                    builder.Append("<br />竞拍得主:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.winID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.winName + "</a>");

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
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("race.aspx") + "\">竞拍</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void WinPage()
    {
        Master.Title = "竞拍得主名单";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("竞拍得主名单");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        strWhere = "paytype=2 and winID>0";
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Game.Race> listRace = new BCW.BLL.Game.Race().GetRaces(pageIndex, pageSize, strWhere, out recordCount);
        if (listRace.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Race n in listRace)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", Out.Hr()));
                }
                builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.winID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[" + DT.FormatDate(n.totime, 4) + "]" + n.winName + "拍得" + n.title + "</a>");

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
        builder.Append("<a href=\"" + Utils.getUrl("race.aspx") + "\">竞拍</a>");
        builder.Append(Out.Tab("</div>", ""));
    }


    private void RulePage()
    {
        Master.Title = "竞拍游戏规则";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("竞拍游戏规则");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.SysUBB(ub.GetSub("RaceRule", xmlPath)));
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("race.aspx") + "\">竞拍</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private string CaseRaceStatus(int Types)
    {
        string sType = string.Empty;
        if (Types == 0)
            sType = "审核中";
        else if (Types == 1)
            sType = "进行中";
        else
            sType = "已结束";

        return sType;

    }
}
