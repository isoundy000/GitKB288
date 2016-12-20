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
/// 大小庄消费记录修改
/// 
/// 黄国军20160315
/// </summary>
/// <summary>
/// 蒙宗将 20160513 抽奖值生成
/// 
/// 邵广林 20160617 动态添加usid
/// 
/// 姚志光 20160621 抽奖写到页面
/// 
/// /// 蒙宗将 20160822 撤掉抽奖值生成
/// </summary>

public partial class bbs_game_bigsmall : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/bigsmall.xml";

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
        if (ub.GetSub("BsStatus", xmlPath) == "1")
        {
            Utils.Safe("此游戏");
        }
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "add":
                AddPage();
                break;
            case "addsave":
                AddSavePage();
                break;
            case "edit":
                EditPage();
                break;
            case "editsave":
                EditSavePage();
                break;
            case "view":
                ViewPage();
                break;
            case "pay":
            case "payon":
                PayPage();
                break;
            case "paylist":
                PayListPage();
                break;
            case "mylist":
                MyListPage();
                break;
            case "top":
                TopPage();
                break;
            case "getpay":
                GetPayPage();
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
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-2]$", "1"));
        int showtype = int.Parse(Utils.GetRequest("showtype", "get", 1, @"^[0-2]$", "0"));
        Master.Title = ub.GetSub("BsName", xmlPath);
        string Logo = ub.GetSub("BsLogo", xmlPath);
        if (Logo != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<img src=\"" + Logo + "\" alt=\"load\"/>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        //拾物随机
        builder.Append(BCW.User.Game.GiftFlows.ShowGiftFlows(8));

        string Notes = ub.GetSub("BsNotes", xmlPath);
        if (Notes != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.SysUBB(Notes) + "");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;大小庄");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("您目前自带" + Utils.ConvertGold(new BCW.BLL.User().GetGold(meid)) + "" + ub.Get("SiteBz") + "");
        if (Isbz())
            builder.Append("/" + Utils.ConvertGold(new BCW.BLL.User().GetMoney(meid)) + "" + ub.Get("SiteBz2") + "");

        builder.Append(Out.Tab("</div>", "<br />"));
        if (Isbz())
        {
            builder.Append(Out.Tab("<div>", ""));

            if (ptype == 1)
                builder.Append(ub.Get("SiteBz") + "区|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("bigsmall.aspx?ptype=1&amp;showtype=" + showtype + "") + "\">" + ub.Get("SiteBz") + "区</a>|");

            if (ptype == 2)
                builder.Append(ub.Get("SiteBz2") + "区|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("bigsmall.aspx?ptype=2&amp;showtype=" + showtype + "") + "\">" + ub.Get("SiteBz2") + "区</a>|");

            builder.Append("<a href=\"" + Utils.getUrl("bigsmall.aspx?ptype=" + ptype + "&amp;showtype=" + showtype + "") + "\">刷新</a>");

            builder.Append(Out.Tab("</div>", "<br />"));
        }

        int pageIndex;
        int recordCount;
        int pageSize = 10;
        string strWhere = string.Empty;
        string strOrder = string.Empty;

        if (ptype == 1)
            strWhere += "Money>=" + Convert.ToInt64(ub.GetSub("BsvMoney", xmlPath)) + " and BzType=0 and Money > SmallPay";
        else if (ptype == 2)
            strWhere += "Money>=" + Convert.ToInt64(ub.GetSub("BsvMoney2", xmlPath)) + " and BzType=1 and Money > SmallPay";

        if (showtype == 0)
            strOrder = "Money DESC,Click DESC";
        else if (showtype == 1)
            strOrder = "Click DESC,Money DESC";
        else
            strOrder = "BigPay DESC,SmallPay DESC";

        string[] pageValUrl = { "act", "ptype", "showtype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Game.Bslist> listBslist = new BCW.BLL.Game.Bslist().GetBslists(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listBslist.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Bslist n in listBslist)
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

                if (showtype == 2)
                    builder.Append("<a href=\"" + Utils.getUrl("bigsmall.aspx?act=view&amp;id=" + n.ID + "") + "\">" + ((pageIndex - 1) * pageSize + k) + "." + n.Title + "</a>(最小" + n.SmallPay + "/最大" + n.BigPay + ")");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("bigsmall.aspx?act=view&amp;id=" + n.ID + "") + "\">" + ((pageIndex - 1) * pageSize + k) + "." + n.Title + "</a>(" + bzText + "" + n.Money + "/人气" + n.Click + ")");
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
        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        builder.Append("排序:");
        builder.Append("<a href=\"" + Utils.getUrl("bigsmall.aspx?ptype=" + ptype + "&amp;showtype=0") + "\">本金</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("bigsmall.aspx?ptype=" + ptype + "&amp;showtype=1") + "\">人气</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("bigsmall.aspx?ptype=" + ptype + "&amp;showtype=2") + "\">下注额</a> ");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("bigsmall.aspx?act=add") + "\">我要开庄</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("bigsmall.aspx?act=view") + "\">我是庄家</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("bigsmall.aspx?act=mylist") + "\">投注记录</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("bigsmall.aspx?act=rule") + "\">游戏介绍</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("bigsmall.aspx?act=top") + "\">&gt;&gt;查看游戏排行</a> ");
        builder.Append(Out.Tab("</div>", ""));

        //闲聊显示
        builder.Append(Out.SysUBB(BCW.User.Users.ShowSpeak(13, "bigsmall.aspx", 5, 0)));

        //游戏底部Ubb
        string Foot = ub.GetSub("BsFoot", xmlPath);
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
        Master.Title = "我要开庄";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">大小庄</a>&gt;开庄");
        builder.Append(Out.Tab("</div>", ""));
        strText = "庄名(15字内):/,资金:/,最小下注:/,最大下注:/,,";
        strName = "Title,Money,SmallPay,BigPay,act,backurl";
        strType = "text,num,num,num,hidden,hidden";
        strValu = "''''addsave'" + Utils.getPage(0) + "";
        strEmpt = "false,false,false,false,false,false";
        strIdea = "/";
        if (Isbz())
            strOthe = "押" + ub.Get("SiteBz") + "|押" + ub.Get("SiteBz2") + ",bigsmall.aspx,post,0,red|blue";
        else
            strOthe = "押" + ub.Get("SiteBz") + ",bigsmall.aspx,post,0,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        builder.Append("*您目前自带" + Utils.ConvertGold(new BCW.BLL.User().GetGold(meid)) + "" + ub.Get("SiteBz") + "");
        if (Isbz())
            builder.Append("/" + Utils.ConvertGold(new BCW.BLL.User().GetMoney(meid)) + "" + ub.Get("SiteBz2") + "");

        builder.Append("<br />*最小启动资金是" + Utils.ConvertGold(Convert.ToInt64(ub.GetSub("BsSmallPay", xmlPath))) + "" + ub.Get("SiteBz") + "");
        if (Isbz())
            builder.Append("/" + Utils.ConvertGold(Convert.ToInt64(ub.GetSub("BsSmallPay2", xmlPath))) + "" + ub.Get("SiteBz2") + "");

        builder.Append("<br />*最大启动资金是" + Utils.ConvertGold(Convert.ToInt64(ub.GetSub("BsBigPay", xmlPath))) + "" + ub.Get("SiteBz") + "");
        if (Isbz())
            builder.Append("/" + Utils.ConvertGold(Convert.ToInt64(ub.GetSub("BsBigPay2", xmlPath))) + "" + ub.Get("SiteBz2") + "");

        builder.Append("<br />*最小下注和最大下注是控制用户的投注额");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("bigsmall.aspx") + "\">大小庄</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void AddSavePage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string ac = Utils.GetRequest("ac", "post", 1, "", "");
        int bzType = 0;
        if (Utils.ToSChinese(ac).Contains(Utils.ToSChinese(ub.Get("SiteBz2"))))
            bzType = 1;
        else
            bzType = 0;

        int id = new BCW.BLL.Game.Bslist().GetID(meid, bzType);
        if (id > 0)
        {
            Utils.Success("我是庄家", "您已开庄，正在进入..", Utils.getUrl("bigsmall.aspx?act=view&amp;id=" + id + ""), "1");
        }

        string Title = Utils.GetRequest("Title", "post", 2, @"^[\s\S]{1,15}$", "庄名限1-15字内");
        long Money = Int64.Parse(Utils.GetRequest("Money", "post", 2, @"^[0-9]\d*$", "资金填写错误"));
        long SmallPay = Int64.Parse(Utils.GetRequest("SmallPay", "post", 2, @"^[0-9]\d*$", "最小下注填写错误"));
        long BigPay = Int64.Parse(Utils.GetRequest("BigPay", "post", 2, @"^[0-9]\d*$", "最大下注填写错误"));
        if (SmallPay > Money || BigPay > Money)
        {
            Utils.Error("资金必须大于最小下注与最大下注", Utils.getPage("bigsmall.aspx?act=add"));
        }
        if (BigPay < SmallPay)
        {
            Utils.Error("最小下注不能大于最大下注", "");
        }

        if (bzType == 0)
        {
            long sCent = Utils.ParseInt64(ub.GetSub("BssCent", xmlPath));
            long bCent = Utils.ParseInt64(ub.GetSub("BsbCent", xmlPath));

            if (SmallPay < sCent || BigPay > bCent)
            {
                Utils.Error("最小下注限制" + sCent + "" + ub.Get("SiteBz") + "以上，最大下注限制" + bCent + "" + ub.Get("SiteBz") + "内", "");
            }
        }
        else
        {
            long sCent2 = Utils.ParseInt64(ub.GetSub("BssCent2", xmlPath));
            long bCent2 = Utils.ParseInt64(ub.GetSub("BsbCent2", xmlPath));
            if (SmallPay < sCent2 || BigPay > bCent2)
            {
                Utils.Error("最小下注限制" + sCent2 + "" + ub.Get("SiteBz2") + "以上，最大下注限制" + bCent2 + "" + ub.Get("SiteBz2") + "内", "");
            }

        }
        string bzText = string.Empty;
        long gold = 0;
        if (bzType == 1)
        {
            bzType = 1;
            bzText = ub.Get("SiteBz2");
            gold = new BCW.BLL.User().GetMoney(meid);
        }
        else
        {
            bzType = 0;
            bzText = ub.Get("SiteBz");
            gold = new BCW.BLL.User().GetGold(meid);
        }
        if (Money < Convert.ToInt64(ub.GetSub("BsSmallPay", xmlPath)) || Money > Convert.ToInt64(ub.GetSub("BsBigPay", xmlPath)))
        {
            Utils.Error("资金金额限" + ub.GetSub("BsSmallPay", xmlPath) + "-" + ub.GetSub("BsBigPay", xmlPath) + "" + bzText + "", "");
        }

        if (Money > gold)
        {
            Utils.Error("你的" + bzText + "不足", Utils.getPage("bigsmall.aspx?act=add"));
        }
        string mename = new BCW.BLL.User().GetUsName(meid);
        if (bzType == 0)
        {
            //支付安全提示
            string[] p_pageArr = { "act", "Title", "Money", "SmallPay", "BigPay", "backurl", "ac" };
            BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr);
            new BCW.BLL.User().UpdateiGold(meid, mename, -Money, "大小庄开庄");
        }
        else
        {
            new BCW.BLL.User().UpdateiMoney(meid, mename, -Money, "大小庄开庄");
        }

        BCW.Model.Game.Bslist model = new BCW.Model.Game.Bslist();
        model.Title = Title;
        model.UsID = meid;
        model.UsName = mename;
        model.Money = Money;
        model.SmallPay = SmallPay;
        model.BigPay = BigPay;
        model.AddTime = DateTime.Now;
        model.BzType = bzType;
        model.Click = 0;
        new BCW.BLL.Game.Bslist().Add(model);


        //动态
        string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/bigsmall.aspx]大小庄[/url]开庄《" + model.Title + "》";
        new BCW.BLL.Action().Add(13, 0, meid, "", wText);

        Utils.Success("我要开庄", "恭喜，开庄成功，祝君好运！", Utils.getUrl("bigsmall.aspx"), "3");
    }

    private void ViewPage()
    {
        Master.Title = "个人庄";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "get", 1, @"^[1-9]\d*$", "0"));
        if (id == 0)
        {
            int id1 = new BCW.BLL.Game.Bslist().GetID(meid, 0);
            int id2 = new BCW.BLL.Game.Bslist().GetID(meid, 1);
            if (id1 > 0 || id2 > 0)
            {
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append(Out.Tab("选择我的私庄", "<b>=选择我的私庄=</b>"));
                builder.Append(Out.Tab("</div>", ""));
                if (id1 > 0)
                    builder.Append("<br /><a href=\"" + Utils.getUrl("bigsmall.aspx?act=view&amp;id=" + id1 + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.Game.Bslist().GetTitle(id1) + "</a>(" + ub.Get("SiteBz") + ")");

                if (id2 > 0)
                    builder.Append("<br /><a href=\"" + Utils.getUrl("bigsmall.aspx?act=view&amp;id=" + id2 + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.Game.Bslist().GetTitle(id2) + "</a>(" + ub.Get("SiteBz2") + ")");

            }
        }
        else
        {

            BCW.Model.Game.Bslist model = new BCW.BLL.Game.Bslist().GetBslist(id);
            if (model == null)
            {
                Utils.Error("不存在的私庄", "");
            }
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append(Out.Tab(model.Title, "<b>=" + model.Title + "=</b>"));
            builder.Append(Out.Tab("</div>", "<br />"));

            string bzText = string.Empty;
            if (model.BzType == 0)
                bzText = ub.Get("SiteBz");
            else
                bzText = ub.Get("SiteBz2");

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("资金：" + model.Money + "<br />");
            builder.Append("币种：" + bzText + "<br />");
            builder.Append("人气：" + model.Click + "<br />");
            builder.Append("最小投注：" + model.SmallPay + "<br />");
            builder.Append("最大投注：" + model.BigPay + "<br />");
            builder.Append("<small><a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.UsName + "</a>创办于" + DT.FormatDate(model.AddTime, 2) + "</small>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("bigsmall.aspx?act=pay&amp;id=" + id + "") + "\">&gt;马上下注</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("bigsmall.aspx?act=paylist&amp;id=" + id + "") + "\">&gt;参与记录</a>");
            builder.Append(Out.Tab("</div>", ""));

            if (model.UsID == meid)
            {
                strText = "输入" + bzText + ":/,,,";
                strName = "iMoney,act,id,backurl";
                strType = "num,hidden,hidden,hidden";
                strValu = "'getpay'" + id + "'" + Utils.getPage(0) + "";
                strEmpt = "false,false,false,false";
                strIdea = "/";
                strOthe = "确定注入|提取,bigsmall.aspx,post,0,red|blue";

                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("bigsmall.aspx?act=edit&amp;id=" + id + "") + "\">修改个人庄&gt;&gt;</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append("*大小庄1赔" + ub.GetSub("BsOdds", xmlPath) + "，含本金<br />");
            builder.Append("*系统将扣除胜方手续费（庄家" + ub.GetSub("BsZTar", xmlPath) + "%，闲家" + ub.GetSub("BsXTar", xmlPath) + "%）");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("bigsmall.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("bigsmall.aspx") + "\">大小庄</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void GetPayPage()
    {
        Master.Title = "个人庄";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string ac = Utils.GetRequest("ac", "post", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[1-9]\d*$", "ID错误"));
        long iMoney = Int64.Parse(Utils.GetRequest("iMoney", "post", 4, @"^[1-9]\d*$", "输入币额错误"));
        BCW.Model.Game.Bslist model = new BCW.BLL.Game.Bslist().GetBslist(id);
        if (model == null || model.UsID != meid)
        {
            Utils.Error("不存在的记录", "");
        }

        //检测上一个取存款是否一样
        int uid = meid;
        long payCent = iMoney;
        DataSet ds = new BCW.BLL.Goldlog().GetList("TOP 1 PUrl,AcGold,AddTime", "UsId=" + uid + " ORDER BY ID DESC");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            string PUrl = ds.Tables[0].Rows[0]["PUrl"].ToString();
            long AcGold = Int64.Parse(ds.Tables[0].Rows[0]["AcGold"].ToString());
            DateTime AddTime = DateTime.Parse(ds.Tables[0].Rows[0]["AddTime"].ToString());

            if (PUrl == Utils.getPageUrl() && Math.Abs(AcGold) == payCent)
            {
                if (AddTime > DateTime.Now.AddSeconds(-5))
                {
                    new BCW.BLL.Guest().Add(10086, "管理员", "ID:" + uid + "在大小庄存取币有刷币嫌疑，请进后台查询消费日志并处理。");
                }
            }
        }

        string mename = model.UsName;
        if (Utils.ToSChinese(ac).Contains(Utils.ToSChinese("注入")))
        {
            long meMoney = new BCW.BLL.User().GetGold(meid);
            if (model.BzType == 0)
            {
                if (meMoney < iMoney)
                    Utils.Error("你的" + ub.Get("SiteBz") + "不足", "");

                //支付安全提示
                string[] p_pageArr = { "act", "ac", "iMoney", "id" };
                BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr);

                new BCW.BLL.User().UpdateiGold(meid, mename, -iMoney, "大小庄注入币,当前余额" + (model.Money + iMoney));
            }
            else
            {
                if (meMoney < iMoney)
                    Utils.Error("你的" + ub.Get("SiteBz2") + "不足", "");

                new BCW.BLL.User().UpdateiMoney(meid, mename, -iMoney, "大小庄注入币,当前余额" + model.Money);
            }
            //加资金
            new BCW.BLL.Game.Bslist().UpdateMoney(id, iMoney);
            Utils.Success("注入资金", "恭喜，注入" + iMoney + "资金成功，祝君好运！", Utils.getUrl("bigsmall.aspx?act=view&amp;id=" + id + ""), "1");
        }
        else if (Utils.ToSChinese(ac).Contains(Utils.ToSChinese("提取")))
        {
            if (model.Money < iMoney)
                Utils.Error("资金不足", "");

            if (model.BzType == 0)
            {
                new BCW.BLL.User().UpdateiGold(meid, mename, iMoney, "大小庄提取币,当前余额" + (model.Money - iMoney));
            }
            else
            {
                new BCW.BLL.User().UpdateiMoney(meid, mename, iMoney, "大小庄提取币,当前余额" + (model.Money - iMoney));
            }
            //扣资金
            new BCW.BLL.Game.Bslist().UpdateMoney(id, -iMoney);
            Utils.Success("注入资金", "恭喜，提取" + iMoney + "资金成功，再接再厉！", Utils.getUrl("bigsmall.aspx?act=view&amp;id=" + id + ""), "1");
        }
    }

    private void EditPage()
    {
        Master.Title = "个人庄";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.Game.Bslist model = new BCW.BLL.Game.Bslist().GetBslist(id);
        if (model == null || model.UsID != meid)
        {
            Utils.Error("不存在的记录", "");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(Out.Tab("修改个人庄", "<b>=修改个人庄=</b>"));
        builder.Append(Out.Tab("</div>", ""));

        strText = "庄名(15字内):/,最小下注:/,最大下注:/,,,";
        strName = "Title,SmallPay,BigPay,act,id,backurl";
        strType = "text,num,num,hidden,hidden,hidden";
        strValu = "" + model.Title + "'" + model.SmallPay + "'" + model.BigPay + "'editsave'" + id + "'" + Utils.getPage(0) + "";
        strEmpt = "false,false,false,false,false,false";
        strIdea = "/";
        strOthe = "确定修改,bigsmall.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("bigsmall.aspx?act=view&amp;id=" + id + "") + "\">&lt;返回继续</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("bigsmall.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("bigsmall.aspx") + "\">大小庄</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void EditSavePage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[1-9]\d*$", "ID错误"));
        string Title = Utils.GetRequest("Title", "post", 2, @"^[\s\S]{1,15}$", "庄名限1-15字内");
        long SmallPay = Int64.Parse(Utils.GetRequest("SmallPay", "post", 2, @"^[0-9]\d*$", "最小下注填写错误"));
        long BigPay = Int64.Parse(Utils.GetRequest("BigPay", "post", 2, @"^[0-9]\d*$", "最大下注填写错误"));
        if (BigPay < SmallPay)
        {
            Utils.Error("最小下注不能大于最大下注", "");
        }



        BCW.Model.Game.Bslist editmodel = new BCW.BLL.Game.Bslist().GetBslist(id);
        if (editmodel == null || editmodel.UsID != meid)
        {
            Utils.Error("不存在的记录", "");
        }

        if (editmodel.BzType == 0)
        {
            long sCent = Utils.ParseInt64(ub.GetSub("BssCent", xmlPath));
            long bCent = Utils.ParseInt64(ub.GetSub("BsbCent", xmlPath));

            if (SmallPay < sCent || BigPay > bCent)
            {
                Utils.Error("最小下注限制" + sCent + "" + ub.Get("SiteBz") + "以上，最大下注限制" + bCent + "" + ub.Get("SiteBz") + "内", "");
            }
        }
        else
        {
            long sCent2 = Utils.ParseInt64(ub.GetSub("BssCent2", xmlPath));
            long bCent2 = Utils.ParseInt64(ub.GetSub("BsbCent2", xmlPath));
            if (SmallPay < sCent2 || BigPay > bCent2)
            {
                Utils.Error("最小下注限制" + sCent2 + "" + ub.Get("SiteBz2") + "以上，最大下注限制" + bCent2 + "" + ub.Get("SiteBz2") + "内", "");
            }

        }
        string mename = new BCW.BLL.User().GetUsName(meid);
        BCW.Model.Game.Bslist model = new BCW.Model.Game.Bslist();
        model.ID = id;
        model.Title = Title;
        model.UsID = meid;
        model.UsName = mename;
        model.SmallPay = SmallPay;
        model.BigPay = BigPay;
        new BCW.BLL.Game.Bslist().UpdateBasic(model);
        Utils.Success("修改个人庄", "恭喜，修改个人庄成功", Utils.getUrl("bigsmall.aspx?act=view&amp;id=" + id + ""), "1");
    }

    private void PayPage()
    {
        Master.Title = "参与投注";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.Game.Bslist model = new BCW.BLL.Game.Bslist().GetBslist(id);
        if (model == null)
        {
            Utils.Error("不存在的私庄", "");
        }
        if (model.UsID == meid)
        {
            Utils.Error("不能在自己的私庄下注", "");
        }
        if (info == "ok")
        {
            int bet = 0;
            long PayCent = 0;
            if (Request["act"] == "payon")
            {
                BCW.Model.Game.Bspay modelBf = new BCW.BLL.Game.Bspay().GetBspayBf(meid, id);
                if (modelBf == null)
                {
                    Utils.Error("操作错误", "");
                }
                bet = Convert.ToInt32(modelBf.BetType);
                PayCent = modelBf.PayCent;
            }
            else
            {
                bet = int.Parse(Utils.GetRequest("bet", "post", 2, @"^[0-1]$", "选择大小错误"));
                PayCent = Int64.Parse(Utils.GetRequest("PayCent", "post", 2, @"^[0-9]\d*$", "赌注填写错误"));
            }
            if (PayCent < model.SmallPay || PayCent > model.BigPay)
            {
                Utils.Error("赌注限" + model.SmallPay + "-" + model.BigPay + "", "");
            }
            if (PayCent > model.Money)
            {
                Utils.Error("庄家资金不足，等待庄家续费再玩吧", "");
            }

            long gold = 0;
            string bzText = string.Empty;
            if (model.BzType == 0)
            {
                //支付安全提示
                string[] p_pageArr = { "act", "info", "id", "bet", "PayCent", "backurl", "verify" };
                BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr);

                bzText = ub.Get("SiteBz");
                gold = new BCW.BLL.User().GetGold(meid);
            }
            else
            {
                bzText = ub.Get("SiteBz2");
                gold = new BCW.BLL.User().GetMoney(meid);
            }
            if (gold < PayCent)
                Utils.Error("你的自带" + bzText + "不足", "");

            //是否刷屏
            string appName = "LIGHT_BIGSMALL";
            int Expir = Utils.ParseInt(ub.GetSub("BsExpir", xmlPath));
            BCW.User.Users.IsFresh(appName, Expir);

            //------------------------------------------------
            //验证防刷
            string verify = Utils.GetRequest("verify", "all", 2, @"^[\w\d]+?$", "验证码错误");
            verify = DESEncrypt.Decrypt(verify, "bskey8585");
            if (string.IsNullOrEmpty(verify))
            {
                Utils.Error("验证码错误", "");
            }
            string meverify = new BCW.BLL.User().GetVerifys(meid);
            if (!string.IsNullOrEmpty(meverify))
            {
                if (verify.Equals(meverify))
                {
                    Utils.Error("验证码有误,请点击返回上级再下注", Utils.getPage("bigsmall.aspx?act=pay&amp;id=" + id + ""));
                }
            }
            //更新验证码
            new BCW.BLL.User().UpdateVerifys(meid, verify);
            //------------------------------------------------

            bool IsWin = false;
            Random ra = new Random(unchecked((int)DateTime.Now.Ticks));
            int rdNext = ra.Next(0, 2);
            //if (rdNext == 1 || rdNext == 3 || rdNext == 5 || rdNext == 7 || rdNext == 9)
            //    rdNext = 1;
            //else
            //    rdNext = 0;

            if (rdNext == bet)
                IsWin = true;


            //庄家十赌六赢
            string ZWinID = ub.GetSub("BsZWinID", xmlPath);
            if (ZWinID != "")
            {
                if (("#" + ZWinID + "#").Contains("#" + model.UsID + "#"))
                {
                    rdNext = ra.Next(1, 1000);
                    if (rdNext <= 400)
                    {
                        rdNext = bet;
                        IsWin = true;
                    }
                    else
                    {
                        rdNext = ((bet == 0) ? 1 : 0);
                        IsWin = false;
                    }
                }
            }

            //庄家十赌六输
            string ZLostID = ub.GetSub("BsZLostID", xmlPath);
            if (ZLostID != "")
            {
                if (("#" + ZLostID + "#").Contains("#" + model.UsID + "#"))
                {
                    rdNext = ra.Next(1, 1000);
                    if (rdNext <= 400)
                    {
                        rdNext = ((bet == 0) ? 1 : 0);
                        IsWin = false;
                    }
                    else
                    {
                        rdNext = bet;
                        IsWin = true;
                    }
                }
            }

            //客家十赌六赢
            string WinID = ub.GetSub("BsWinID", xmlPath);
            if (WinID != "")
            {
                if (("#" + WinID + "#").Contains("#" + meid + "#"))
                {
                    rdNext = ra.Next(1, 1000);
                    if (rdNext <= 400)
                    {
                        rdNext = ((bet == 0) ? 1 : 0);
                        IsWin = false;
                    }
                    else
                    {
                        rdNext = bet;
                        IsWin = true;
                    }
                }
            }

            //客家十赌六输
            string LostID = ub.GetSub("BsLostID", xmlPath);
            if (LostID != "")
            {
                if (("#" + LostID + "#").Contains("#" + meid + "#"))
                {
                    rdNext = ra.Next(1, 1000);
                    if (rdNext <= 400)
                    {
                        rdNext = bet;
                        IsWin = true;
                    }
                    else
                    {
                        rdNext = ((bet == 0) ? 1 : 0);
                        IsWin = false;
                    }
                }
            }


            long WinCent = 0;
            string mename = new BCW.BLL.User().GetUsName(meid);
            if (IsWin)//闲胜
            {
                double XTar = Convert.ToDouble(ub.GetSub("BsXTar", xmlPath));
                long xMoney = PayCent - Convert.ToInt64(XTar * 0.01 * PayCent);
                if (model.BzType == 0)
                {
                    //更新排行榜
                    BCW.Model.Toplist modeltop = new BCW.Model.Toplist();
                    modeltop.Types = 9;
                    modeltop.UsId = model.UsID;
                    modeltop.UsName = model.UsName;

                    modeltop.PutNum = 1;
                    modeltop.PutGold = -PayCent;

                    if (!new BCW.BLL.Toplist().Exists(model.UsID, 9))
                        new BCW.BLL.Toplist().Add(modeltop);
                    else
                        new BCW.BLL.Toplist().Update(modeltop);

                    new BCW.BLL.User().UpdateiGold(meid, mename, xMoney, 9, "大小庄" + model.UsID.ToString() + "消费");
                }
                else
                    new BCW.BLL.User().UpdateiMoney(meid, mename, xMoney, "大小庄赢得");

                new BCW.BLL.Game.Bslist().UpdateMoney(id, -PayCent);
                WinCent = xMoney;

            }
            else//庄胜
            {
                double ZTar = Convert.ToDouble(ub.GetSub("BsZTar", xmlPath));
                long zMoney = PayCent - Convert.ToInt64(ZTar * 0.01 * PayCent);
                new BCW.BLL.Game.Bslist().UpdateMoney(id, zMoney);
                if (model.BzType == 0)
                {
                    //更新排行榜
                    BCW.Model.Toplist modeltop = new BCW.Model.Toplist();
                    modeltop.Types = 9;
                    modeltop.UsId = model.UsID;
                    modeltop.UsName = model.UsName;
                    modeltop.WinNum = 1;
                    modeltop.WinGold = zMoney;
                    if (!new BCW.BLL.Toplist().Exists(model.UsID, 9))
                        new BCW.BLL.Toplist().Add(modeltop);
                    else
                        new BCW.BLL.Toplist().Update(modeltop);

                    new BCW.BLL.User().UpdateiGold(meid, mename, -PayCent, 9, "大小庄" + model.UsID.ToString() + "消费");
                }
                else
                {
                    new BCW.BLL.User().UpdateiMoney(meid, mename, -PayCent, "大小庄失去");
                }
                WinCent = -PayCent;
            }

            //写进下注记录
            BCW.Model.Game.Bspay addmodel = new BCW.Model.Game.Bspay();
            addmodel.BsId = id;
            addmodel.BsTitle = model.Title;
            addmodel.BzType = model.BzType;
            addmodel.BetType = bet;
            addmodel.PayCent = PayCent;
            addmodel.UsID = meid;
            addmodel.UsName = mename;
            addmodel.WinCent = WinCent;
            addmodel.AddTime = DateTime.Now;
             int ID=new BCW.BLL.Game.Bspay().Add(addmodel);
            //写进人气
            new BCW.BLL.Game.Bslist().UpdateClick(id);
            //动态
            string wText = string.Empty;
            if (IsWin == true)
                wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/bigsmall.aspx]大小庄[/url]《" + model.Title + "》赢得了" + PayCent + "" + bzText + "";
            else
                wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/bigsmall.aspx]大小庄[/url]《" + model.Title + "》失去了" + PayCent + "" + bzText + "";

            new BCW.BLL.Action().Add(13, 0, meid, "", wText);
            ////活跃抽奖入口_20160621姚志光
            try
            {
                //表中存在虚拟球类记录
                if (new BCW.BLL.tb_WinnersGame().ExistsGameName("大小庄家"))
                {
                    //投注是否大于设定的限额，是则有抽奖机会
                    if (PayCent > new BCW.BLL.tb_WinnersGame().GetPrice("大小庄家"))
                    {
                        string TextForUbb = (ub.GetSub("TextForUbb", "/Controls/winners.xml"));//设置内线提示的文字
                        string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", "/Controls/winners.xml"));//1发内线2不发内线 
                        int hit = new BCW.winners.winners().CheckActionForAll(1, ID, meid, mename, "大小庄", 3);
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
            builder.Append(Out.Tab("<div>", ""));
            if (rdNext == 0)
                builder.Append("<img src=\"/Files/sys/game/big.gif\" alt=\"load\" />");
            else
                builder.Append("<img src=\"/Files/sys/game/small.gif\" alt=\"load\" />");

            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            if (IsWin == true && bet == 0)
                builder.Append("恭喜你！你买的是大，结果也是大，因而本局你赢得了此次下注");
            else if (IsWin == true && bet == 1)
                builder.Append("恭喜你！你买的是小，结果也是小，因而本局你赢得了此次下注");
            else if (IsWin == false && bet == 0)
                builder.Append("很可惜！你买的是大，结果却是小，因而本局你输掉了下注");
            else if (IsWin == false && bet == 1)
                builder.Append("很可惜！你买的是小，结果却是大，因而本局你输掉了下注");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("您目前自带" + Utils.ConvertGold(new BCW.BLL.User().GetGold(meid)) + "" + ub.Get("SiteBz") + "");
            if (Isbz())
                builder.Append("/" + Utils.ConvertGold(new BCW.BLL.User().GetMoney(meid)) + "" + ub.Get("SiteBz2") + "");

            builder.Append("<br /><a href=\"" + Utils.getUrl("bigsmall.aspx?act=payon&amp;info=ok&amp;id=" + id + "&amp;verify=" + DESEncrypt.Encrypt(new Rand().RandNumer(4), "bskey8585") + "") + "\">刷新下注</a>");
            builder.Append("<br /><a href=\"" + Utils.getUrl("bigsmall.aspx?act=pay&amp;id=" + id + "") + "\">再赌一把！</a>");
            builder.Append("<br /><a href=\"" + Utils.getUrl("bigsmall.aspx?act=view&amp;id=" + id + "") + "\">&lt;返回继续</a>");
            builder.Append(Out.Tab("</div>", ""));

        }
        else
        {
            //拾物随机
            builder.Append(BCW.User.Game.GiftFlows.ShowGiftFlows(8));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<img src=\"/Files/sys/game/bsrun.gif\" alt=\"load\" /><img src=\"/Files/sys/game/bsrun.gif\" alt=\"load\" />");
            builder.Append(Out.Tab("</div>", ""));

            string bzText = string.Empty;
            if (model.BzType == 0)
                bzText = ub.Get("SiteBz");
            else
                bzText = ub.Get("SiteBz2");

            strText = "你的选择:/,赌注:,,,,,";
            strName = "bet,PayCent,id,verify,act,info,backurl";
            strType = "select,snum,hidden,hidden,hidden,hidden,hidden";
            strValu = "''" + id + "'" + DESEncrypt.Encrypt(new Rand().RandNumer(4), "bskey8585") + "'pay'ok'" + Utils.getPage(0) + "";
            strEmpt = "0|『大』|1|『小』,false,false,false,false,false,false";
            strIdea = "'" + bzText + "'''''|/";
            strOthe = "买定离手,bigsmall.aspx,post,0,red";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("*赔率：1赔" + ub.GetSub("BsOdds", xmlPath) + "<br />*您目前自带" + Utils.ConvertGold(new BCW.BLL.User().GetGold(meid)) + "" + ub.Get("SiteBz") + "");
            if (Isbz())
                builder.Append("/" + Utils.ConvertGold(new BCW.BLL.User().GetMoney(meid)) + "" + ub.Get("SiteBz2") + "");

            builder.Append("<br /><a href=\"" + Utils.getUrl("bigsmall.aspx?act=view&amp;id=" + id + "") + "\">&lt;返回继续</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("bigsmall.aspx") + "\">大小庄</a>");
        builder.Append(Out.Tab("</div>", ""));

    }

    private void PayListPage()
    {
        Master.Title = "历史记录";
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Game.Bslist().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("bigsmall.aspx?act=view&amp;id=" + id + "") + "\">个人庄</a>&gt;参与记录");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        strWhere = "BsId=" + id + "";

        string[] pageValUrl = { "act", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Game.Bspay> listBspay = new BCW.BLL.Game.Bspay().GetBspays(pageIndex, pageSize, strWhere, out recordCount);
        if (listBspay.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Bspay n in listBspay)
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

                string winText = string.Empty;
                if (n.WinCent > 0)
                    winText = "，并赢得了此次下注";

                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "</a>参与" + n.PayCent + "" + bzText + "" + winText + "(" + DT.FormatDate(n.AddTime, 1) + ")");
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
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("bigsmall.aspx?act=view&amp;id=" + id + "") + "\">&lt;返回上一级</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("bigsmall.aspx") + "\">大小庄</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void MyListPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Master.Title = "我的投注记录";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("我的投注记录");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        strWhere = "UsID=" + meid + "";

        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Game.Bspay> listBspay = new BCW.BLL.Game.Bspay().GetBspays(pageIndex, pageSize, strWhere, out recordCount);
        if (listBspay.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Bspay n in listBspay)
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

                string winText = string.Empty;
                if (n.WinCent > 0)
                    winText = "，并赢得了此次下注";

                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".参加<a href=\"" + Utils.getUrl("bigsmall.aspx?act=view&amp;id=" + n.BsId + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.BsTitle + "</a>下注" + n.PayCent + "" + bzText + "" + winText + "(" + DT.FormatDate(n.AddTime, 1) + ")");
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
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("bigsmall.aspx") + "\">大小庄</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    /// <summary>
    /// 排行榜
    /// </summary>
    private void TopPage()
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));
        Master.Title = "大小庄排行榜";
        builder.Append(Out.Tab("<div class=\"title\">", ""));

        if (ptype == 1)
            builder.Append("赚币排行|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("bigsmall.aspx?act=top&amp;ptype=1") + "\">赚币排行</a>|");

        if (ptype == 2)
            builder.Append("胜率排行");
        else
            builder.Append("<a href=\"" + Utils.getUrl("bigsmall.aspx?act=top&amp;ptype=2") + "\">胜率排行</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = 50;
        string strWhere = "";
        string strOrder = "";
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        pageIndex = 1;

        //查询条件
        strWhere = "Types=9";
        if (ptype == 1)
        {
            strOrder = "(WinGold+PutGold) Desc";
        }
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

                builder.AppendFormat("[第" + k + "名]<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid={0}&amp;backurl=" + Utils.getPage(1) + "") + "\">{1}</a>{2}", n.UsId, n.UsName, sText);
                k++;
                builder.Append(Out.Tab("</div>", ""));

            }

            // 分页
            //builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("bigsmall.aspx") + "\">大小庄</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void RulePage()
    {
        Master.Title = "大小庄游戏介绍";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("大小庄游戏介绍");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("1.大小庄游戏是会员与会员之间的较量,系统不做庄,由会员自行开庄和会员参与<br />2.大小庄中，一个ID只能开一个" + ub.Get("SiteBz") + "庄和" + ub.Get("SiteBz2") + "庄,在开庄里面选择币种即可.<br />3.庄家可以注入金额或提取,如资金小于" + ub.GetSub("BsvMoney", xmlPath) + "时则不会显示在开庄列表.<br />4.已开庄的庄家可以在“我是庄家”进入管理或者注入/提取资金金额.<br />5.玩家进入其中一个庄,点击“马上下注”即可参与下注,输赢结果即时显示<br />6.大小庄赔率统一是" + ub.GetSub("BsOdds", xmlPath) + "倍(含本金),每局赢家将扣除一定的手续费,<br />游戏刺激好玩,快来<a href=\"" + Utils.getUrl("bigsmall.aspx") + "\">试试你的运气吧~</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("bigsmall.aspx") + "\">大小庄</a>");
        builder.Append(Out.Tab("</div>", ""));
    }


    private bool Isbz()
    {
        return true;
        //if (Utils.GetDomain().Contains("kubao") || Utils.GetDomain().Contains("tuhao") || Utils.GetDomain().Contains("th") || Utils.GetDomain().Contains("kb288"))
        //    return true;
        //else
        //    return false;
    }
}
