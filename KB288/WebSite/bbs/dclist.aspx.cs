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

public partial class bbs_dclist : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        int forumid = int.Parse(Utils.GetRequest("forumid", "all", 2, @"^[0-9]\d*$", "论坛ID错误"));
        int bid = int.Parse(Utils.GetRequest("bid", "all", 2, @"^[0-9]\d*$", "帖子ID错误"));
        if (!new BCW.BLL.Forum().Exists2(forumid))
        {
            Utils.Success("访问论坛", "该论坛不存在或已暂停使用", Utils.getUrl("forum.aspx"), "1");
        }
        if (!new BCW.BLL.Text().Exists2(bid, forumid))
        {
            Utils.Error("帖子不存在或已被删除", "");
        }
        switch (act)
        {
            case "info":
                InfoPage(forumid, bid);
                break;
            case "info2":
                Info2Page(forumid, bid);
                break;
            case "infosave":
                InfoSavePage(forumid, bid);
                break;
            case "list":
                ListPage(forumid, bid);
                break;
            case "win":
                WinPage(forumid, bid);
                break;
            case "winok":
                WinOkPage(forumid, bid);
                break;
            case "win2":
                Win2Page(forumid, bid);
                break;
            case "win2ok":
                Win2OkPage(forumid, bid);
                break;
            case "winno":
                WinNoPage(forumid, bid);
                break;
            case "win2no":
                Win2NoPage(forumid, bid);
                break;
            case "win3":
                Win3Page(forumid, bid);
                break;
            case "logview":
                LogViewPage(forumid, bid);
                break;
            case "winadmin":
                WinAdminPage(forumid, bid);
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {

    }

    private void InfoPage(int forumid, int bid)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Master.Title = "我要应战";
        BCW.Model.Textdc dc = new BCW.BLL.Textdc().GetTextdc(bid, 0);
        if (dc == null)
        {
            Utils.Error("不存在的竞猜记录", "");
        }
        string bzText = string.Empty;
        if (dc.BzType == 0)
            bzText = ub.Get("SiteBz");
        else
            bzText = ub.Get("SiteBz2");

        builder.Append(Out.Tab("<div class=\"title\">我要参与应战</div>", ""));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("庄家:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + dc.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(dc.UsID) + "(" + dc.UsID + ")</a><br />");
        builder.Append("庄保证金:" + dc.OutCent + "" + bzText + "<br />");
        BCW.Model.Textdc dc2 = new BCW.BLL.Textdc().GetTextdc(bid, 1, meid);
        if (dc2 != null)
        {
            builder.Append("---------<br />");
            builder.Append("您已押" + dc2.OutCent + "" + bzText + "");
            builder.Append("<br />=增加您的保证金=");
        }
        else
        {
            builder.Append("=输入您的保证金=");
        }
        builder.Append(Out.Tab("</div>", ""));

        string strText = ",,,,,";
        string strName = "cent,forumid,bid,act,backurl";
        string strType = "num,hidden,hidden,hidden,hidden";
        string strValu = "'" + forumid + "'" + bid + "'infosave'" + Utils.getPage(0) + "";
        string strEmpt = "false,false,false,false,false";
        string strIdea = "" + bzText + "''''|/";
        string strOthe = "决定了,dclist.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("dclist.aspx?act=list&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">查看应战记录&gt;&gt;</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("dclist.aspx?act=list&amp;forumid=" + forumid + "&amp;bid=" + bid + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "") + "\">主题</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void Info2Page(int forumid, int bid)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Master.Title = "我要续币";
        BCW.Model.Textdc dc = new BCW.BLL.Textdc().GetTextdc(bid, 0);
        if (dc == null)
        {
            Utils.Error("不存在的竞猜记录", "");
        }
        string bzText = string.Empty;
        if (dc.BzType == 0)
            bzText = ub.Get("SiteBz");
        else
            bzText = ub.Get("SiteBz2");

        builder.Append(Out.Tab("<div class=\"title\">我要续币</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("我是庄家:<br />");
        builder.Append("我的保证金:" + dc.OutCent + "" + bzText + "<br />");
        builder.Append("---------<br />");
        builder.Append("=增加您的保证金=");
        builder.Append(Out.Tab("</div>", ""));

        string strText = ",,,,,";
        string strName = "cent,forumid,bid,act,backurl";
        string strType = "num,hidden,hidden,hidden,hidden";
        string strValu = "'" + forumid + "'" + bid + "'infosave'" + Utils.getPage(0) + "";
        string strEmpt = "false,false,false,false,false";
        string strIdea = "" + bzText + "''''|/";
        string strOthe = "决定了,dclist.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("dclist.aspx?act=list&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">查看应战记录&gt;&gt;</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("dclist.aspx?act=list&amp;forumid=" + forumid + "&amp;bid=" + bid + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "") + "\">主题</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void InfoSavePage(int forumid, int bid)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        BCW.Model.Text p = new BCW.BLL.Text().GetText(bid);

        if (p.IsOver == 1)
        {
            Utils.Error("此帖子已经结束，不允许再进行竞技", "");
        }
        if (p.IsLock == 1)
        {
            Utils.Error("此帖子已经被锁定，不允许再进行竞技", "");
        }
        if (p.IsTop == -1)
        {
            Utils.Error("此帖子已经固底，不允许再进行竞技", "");
        }

        Master.Title = "增加保证金";

        BCW.Model.Textdc dc = new BCW.BLL.Textdc().GetTextdc(bid, 0);
        if (dc == null)
        {
            Utils.Error("不存在的竞猜记录", "");
        }
        string bzText = string.Empty;
        if (dc.BzType == 0)
            bzText = ub.Get("SiteBz");
        else
            bzText = ub.Get("SiteBz2");

        long cent = Int64.Parse(Utils.GetRequest("cent", "post", 4, @"^[1-9]\d*$", "保证金填写错误"));
        string mename = new BCW.BLL.User().GetUsName(meid);
        long xCent = 0;
        if (dc.UsID == meid)
        {
            if (dc.OutCent + cent > 50000000)
            {
                Utils.Error("庄保证金不能大于5000万" + bzText + "", "");
            }
        }
        else
        {
            xCent = new BCW.BLL.Textdc().GetCents(bid, 1);
            if (dc.OutCent < xCent + cent)
            {
                Utils.Error("闲家保证金已超出庄保证金，当前只可以增加" + (dc.OutCent - xCent) + "" + bzText + "", "");
            }
        
        }
        if (dc.BzType == 0)
        {
            string SysID = ub.GetSub("FinanceSysID", "/Controls/finance.xml");
            if (("#" + SysID + "#").Contains("#" + meid + "#"))
            {
                Utils.Error("你的权限不足", "");
            }
            //内部ID过户软禁
            string SysID2 = ub.GetSub("FinanceSysID2", "/Controls/finance.xml");
            SysID2 += "#" + ub.GetSub("XiaoHao", "/Controls/xiaohao.xml");

            if (("#" + SysID2 + "#").Contains("#" + meid + "#"))
            {
                Utils.Error("过户权限不足，请联系客服！", "");
            }
            long mecent = new BCW.BLL.User().GetGold(meid);
            if (mecent < cent)
            {
                Utils.Error("你的" + ub.Get("SiteBz") + "不足", "");
            }

            //支付安全提示
            string[] p_pageArr = { "act", "forumid", "bid", "cent" };
            BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr);

            //扣币
            new BCW.BLL.User().UpdateiGold(meid, mename, -cent, "帖子竞猜消费");

        }
        else
        {
            long mecent = new BCW.BLL.User().GetMoney(meid);
            if (mecent < cent)
            {
                Utils.Error("你的" + ub.Get("SiteBz2") + "不足", "");
            }
            //扣币
            new BCW.BLL.User().UpdateiMoney(meid, mename, -cent, "帖子竞猜消费");
        }
        if (dc.UsID == meid)
        {

            BCW.Model.Textdc m = new BCW.Model.Textdc();
            m.BID = bid;
            m.UsID = meid;
            m.OutCent = cent;
            m.IsZtid = 0;
            m.BzType = dc.BzType;
            m.AddTime = DateTime.Now;

            new BCW.BLL.Textdc().UpdateOutCent(m);
            //记录日志
            string LogText = "" + mename + "(" + meid + ")增加保证金" + cent + "" + bzText + "(" + DT.FormatDate(DateTime.Now, 1) + ")|" + dc.LogText + "";
            new BCW.BLL.Textdc().UpdateLogText(bid, dc.UsID, LogText);

            builder.Append(Out.Tab("<div class=\"title\">增加保证金</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("上次剩余保证金:" + dc.OutCent + "" + bzText + ",本次增加" + cent + "" + bzText + "<br />当前保证金:" + (dc.OutCent + cent) + "" + bzText + "");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("dclist.aspx?act=info2&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">&lt;&lt;继续增加保证金</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("dclist.aspx?act=list&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">查看应战记录&gt;&gt;</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {

            BCW.Model.Textdc m = new BCW.Model.Textdc();
            m.BID = bid;
            m.UsID = meid;
            m.OutCent = cent;
            m.IsZtid = 1;
            m.BzType = dc.BzType;
            m.AddTime = DateTime.Now;
            m.LogText = "";
            builder.Append(Out.Tab("<div class=\"title\">增加保证金</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            BCW.Model.Textdc dc2 = new BCW.BLL.Textdc().GetTextdc(bid, 1, meid);
            if (dc2 == null)
            {
                new BCW.BLL.Textdc().Add(m);
                //记录日志
                string LogText = "" + mename + "(" + meid + ")使用" + cent + "" + bzText + "应战(" + DT.FormatDate(DateTime.Now, 1) + ")|" + dc.LogText + "";
                new BCW.BLL.Textdc().UpdateLogText(bid, dc.UsID, LogText);

                builder.Append("成功增加保证金:" + cent + "" + bzText + "");
            }
            else
            {
                if (xCent + cent > dc.OutCent)
                {
                    Utils.Error("闲家保证金已超出庄保证金，当前只可以增加" + (dc.OutCent - xCent) + "" + bzText + "", "");
                }
                new BCW.BLL.Textdc().UpdateOutCent(m);
                //记录日志
                string LogText = "" + mename + "(" + meid + ")增加保证金" + cent + "" + bzText + "(" + DT.FormatDate(DateTime.Now, 1) + ")|" + dc.LogText + "";
                new BCW.BLL.Textdc().UpdateLogText(bid, dc.UsID, LogText);

                builder.Append("上次剩余保证金:" + dc2.OutCent + "" + bzText + ",本次增加" + cent + "" + bzText + "<br />当前保证金:" + (dc2.OutCent + cent) + "" + bzText + "");
            }
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("dclist.aspx?act=info&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">&lt;&lt;继续增加保证金</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("dclist.aspx?act=list&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">查看应战记录&gt;&gt;</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("dclist.aspx?act=list&amp;forumid=" + forumid + "&amp;bid=" + bid + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "") + "\">主题</a>");
        builder.Append(Out.Tab("</div>", ""));

    }

    private void ListPage(int forumid, int bid)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Master.Title = "应战记录";

        BCW.Model.Textdc dc = new BCW.BLL.Textdc().GetTextdc(bid, 0);
        if (dc == null)
        {
            Utils.Error("不存在的竞猜记录", "");
        }
        string bzText = string.Empty;
        if (dc.BzType == 0)
            bzText = ub.Get("SiteBz");
        else
            bzText = ub.Get("SiteBz2");

        builder.Append(Out.Tab("<div class=\"title\">应战记录</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">查看主题</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">看回复</a><br />");
        builder.Append("庄家:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + dc.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(dc.UsID) + "(" + dc.UsID + ")</a><br />");
        builder.Append("庄保证金:" + dc.OutCent + "" + bzText + "");
        if (dc.UsID == meid)
        {
            builder.Append("<a href=\"" + Utils.getUrl("dclist.aspx?act=win3&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[提取]</a>");
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("=闲家应战记录=");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "forumid", "bid", "backurl" }; ;
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        strWhere = "BID=" + bid + " and IsZtid=1";

        // 开始读取列表
        IList<BCW.Model.Textdc> listTextdc = new BCW.BLL.Textdc().GetTextdcs(pageIndex, pageSize, strWhere, out recordCount);
        if (listTextdc.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Textdc n in listTextdc)
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

                builder.Append("[" + ((pageIndex - 1) * pageSize + k) + "]");
                builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(n.UsID) + "(" + n.UsID + ")</a>参与" + n.OutCent + "" + bzText + "");
                if (n.State == 0)
                {
                    builder.Append("[进行中]");
                    if (dc.UsID == meid)
                    {
                        builder.Append("<a href=\"" + Utils.getUrl("dclist.aspx?act=win2&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;id=" + n.ID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[确定结果]</a>");
                        //builder.Append("<a href=\"" + Utils.getUrl("dclist.aspx?act=win2ok&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;id=" + n.ID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[闲赢]</a>");
                    }
                    else if (n.UsID == meid)
                    {
                        builder.Append("<a href=\"" + Utils.getUrl("dclist.aspx?act=win&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;id=" + n.ID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[确定结果]</a>");
                        //builder.Append("<a href=\"" + Utils.getUrl("dclist.aspx?act=winok&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;id=" + n.ID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[庄赢]</a>");
                    }
                }
                else if (n.State == 1 || n.State == 2)
                {
                    if (n.AcCent < 0)
                        builder.Append("[闲赢" + Math.Abs(n.AcCent) + "]");
                    else
                        builder.Append("[庄赢" + Math.Abs(n.AcCent) + "]");

                    if (dc.UsID == meid && n.State == 1)
                    {
                        builder.Append("<a href=\"" + Utils.getUrl("dclist.aspx?act=win2ok&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;id=" + n.ID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[同意]</a>");
                        builder.Append("<a href=\"" + Utils.getUrl("dclist.aspx?act=win2no&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;id=" + n.ID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[不同意]</a>");
                    }
                    else if (n.UsID == meid && n.State == 2)
                    {

                        builder.Append("<a href=\"" + Utils.getUrl("dclist.aspx?act=winok&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;id=" + n.ID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[同意]</a>");
                        builder.Append("<a href=\"" + Utils.getUrl("dclist.aspx?act=winno&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;id=" + n.ID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[不同意]</a>");
                    }
                    else
                    {
                        builder.Append("[确认中]");
                    }
                }
                else
                {
                    builder.Append("[已完成]");
                }

                if (n.State != 3)
                {
                    if (IsCTID(meid, forumid) == true)
                    {
                        builder.Append("[<a href=\"" + Utils.getUrl("dclist.aspx?act=winadmin&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;id=" + n.ID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">管理</a>]");
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
        builder.Append("<a href=\"" + Utils.getUrl("dclist.aspx?act=logview&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">交易日志&gt;&gt;</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("dclist.aspx?act=info&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">我要应战&gt;&gt;</a>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "") + "\">主题</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void WinPage(int forumid, int bid)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));

        Master.Title = "闲家申请";

        BCW.Model.Textdc dc2 = new BCW.BLL.Textdc().GetTextdc2(id, bid);
        if (dc2 == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (dc2.State == 1 || dc2.State == 2)
        {
            Utils.Error("已成功申请，请耐心等待对方进行回应", "");
        }
        if (dc2.State == 3)
        {
            Utils.Error("这个记录已结束", "");
        }
        BCW.Model.Textdc dc = new BCW.BLL.Textdc().GetTextdc(bid, 0);

        string bzText = string.Empty;
        if (dc.BzType == 0)
            bzText = ub.Get("SiteBz");
        else
            bzText = ub.Get("SiteBz2");

        string info = Utils.GetRequest("info", "post", 1, "", "");
        if (info == "ok")
        {
            int ptype = int.Parse(Utils.GetRequest("ptype", "post", 2, @"^[124]$", "结果选择错误"));
            long cent = Int64.Parse(Utils.GetRequest("cent", "post", 2, @"^[0-9]\d*$", "币额填写错误"));
            string acText = string.Empty;
            if (ptype == 1)
            {
                if (cent > dc.OutCent)
                {
                    Utils.Error("币数超出庄家赔付额度", "");
                }
                acText = "闲(" + dc2.UsID + ")赢";
                new BCW.BLL.Textdc().UpdateState(id, 1, -cent);
            }
            else if(ptype==2)
            {
                if (cent > dc2.OutCent)
                {
                    Utils.Error("币数超出闲家赔付额度", "");
                }
                acText = "庄(" + dc.UsID + ")赢";
                new BCW.BLL.Textdc().UpdateState(id, 1, cent);
            }
            string mename = new BCW.BLL.User().GetUsName(meid);
            string username = new BCW.BLL.User().GetUsName(dc.UsID);
            //记录日志
            string LogText = "" + mename + "(" + meid + ")申请" + acText + "" + cent + "" + bzText + "(" + DT.FormatDate(DateTime.Now, 1) + ")|" + dc.LogText + "";
            new BCW.BLL.Textdc().UpdateLogText(bid, dc.UsID, LogText);

            //给对方发送内线
            new BCW.BLL.Guest().Add(dc.UsID, username, "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]请求您确认帖子竞猜结果：申请" + acText + "" + cent + "" + bzText + "[url=/bbs/dclist.aspx?act=list&amp;forumid=" + forumid + "&amp;bid=" + bid + "]查看详情[/url]");

            Utils.Success("闲家申请", "闲家申请成功，已申请" + acText + "" + cent + "" + bzText + "，请耐心等待庄家确认..", Utils.getUrl("dclist.aspx?act=list&amp;forumid=" + forumid + "&amp;bid=" + bid + ""), "2");
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">闲家申请</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("庄家:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + dc.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(dc.UsID) + "(" + dc.UsID + ")</a><br />");
            builder.Append("庄保证金:" + dc.OutCent + "" + bzText + "<br />");
            builder.Append("闲保证金:" + dc2.OutCent + "" + bzText + "");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "结果:,币额(含本金):,,,,,,";
            string strName = "ptype,cent,forumid,bid,id,act,info,backurl";
            string strType = "select,stext,hidden,hidden,hidden,hidden,hidden,hidden";
            string strValu = "2''" + forumid + "'" + bid + "'" + id + "'win'ok'" + Utils.getPage(0) + "";
            string strEmpt = "2|庄赢|1|闲赢,false,false,false,false,false,false,false";
            string strIdea = "'" + bzText + "''''''|/";
            string strOthe = "确定结果,dclist.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
            builder.Append("温馨提示:<br />当币额填写0时，结果将相当于平手计算.<br />如对方超出12小时不确认您的申请，请联系版主进行确认.");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getPage("dclist.aspx?act=list&amp;forumid=" + forumid + "&amp;bid=" + bid + "") + "\">上级</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "") + "\">主题</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private void WinOkPage(int forumid, int bid)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));

        Master.Title = "闲家确认结果";

        BCW.Model.Textdc dc2 = new BCW.BLL.Textdc().GetTextdc2(id, bid);
        if (dc2 == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (dc2.State == 3)
        {
            Utils.Error("这个记录已结束", "");
        }
        if (dc2.State != 2)
        {
            Utils.Error("不存在的记录", "");
        }
        BCW.Model.Textdc dc = new BCW.BLL.Textdc().GetTextdc(bid, 0);

        if (dc2.UsID != meid)
        {
            Utils.Error("不存在的记录", "");
        }
        string acText = string.Empty;
        if (dc2.AcCent < 0)
        {
            if (Math.Abs(dc2.AcCent) > dc.OutCent)
            {
                Utils.Error("币数超出庄家赔付额度", "");
            }
            acText = "闲(" + dc2.UsID + ")赢";
        }
        else
        {
            if (Math.Abs(dc2.AcCent) > dc2.OutCent)
            {
                Utils.Error("币数超出闲家赔付额度", "");
            }
            acText = "庄(" + dc.UsID + ")赢";
        }
        string bzText = string.Empty;
        if (dc.BzType == 0)
            bzText = ub.Get("SiteBz");
        else
            bzText = ub.Get("SiteBz2");

        string info = Utils.GetRequest("info", "post", 1, "", "");
        if (info == "ok")
        {

            string mename = new BCW.BLL.User().GetUsName(dc2.UsID);
            string username = new BCW.BLL.User().GetUsName(dc.UsID);
            BCW.Model.Textdc m = new BCW.Model.Textdc();
            m.BID = bid;
            m.UsID = dc.UsID;
            if (dc2.AcCent < 0)
            {
                m.OutCent = (dc2.AcCent + dc2.OutCent);
            }
            else
            {
                m.OutCent = dc2.AcCent;
            }
            m.IsZtid = 0;
            m.BzType = dc.BzType;
            m.AddTime = DateTime.Now;
            new BCW.BLL.Textdc().UpdateOutCent(m);
            if (dc2.AcCent < 0)
            {
                long cent = Convert.ToInt64(Math.Abs(dc2.AcCent));
                if (dc.BzType == 0)
                    new BCW.BLL.User().UpdateiGold(dc2.UsID, mename, cent, "帖子竞猜获得");
                else
                    new BCW.BLL.User().UpdateiMoney(dc2.UsID, mename, cent, "帖子竞猜获得");

                new BCW.BLL.Guest().Add(dc.UsID, username, "[url=/bbs/uinfo.aspx?uid=" + dc2.UsID + "]" + mename + "[/url]已确认帖子竞猜结果：您输了" + Math.Abs(dc2.AcCent) + "" + bzText + "[url=/bbs/dclist.aspx?act=list&amp;forumid=" + forumid + "&amp;bid=" + bid + "]查看详情[/url]");

            }
            else
            {
                long cent = Convert.ToInt64(dc2.OutCent - dc2.AcCent);
                if (cent > 0)
                {
                    if (dc.BzType == 0)
                        new BCW.BLL.User().UpdateiGold(dc2.UsID, mename, cent, "帖子竞猜退回");
                    else
                        new BCW.BLL.User().UpdateiMoney(dc2.UsID, mename, cent, "帖子竞猜退回");

                }
                new BCW.BLL.Guest().Add(dc.UsID, username, "[url=/bbs/uinfo.aspx?uid=" + dc2.UsID + "]" + mename + "[/url]已确认帖子竞猜结果：您赢了" + dc2.AcCent + "" + bzText + "[url=/bbs/dclist.aspx?act=list&amp;forumid=" + forumid + "&amp;bid=" + bid + "]查看详情[/url]");
            }

            new BCW.BLL.Textdc().UpdateState(id, 3);

            string LogText = "" + mename + "(" + dc2.UsID + ")确认" + acText + "" + Math.Abs(dc2.AcCent) + "" + bzText + "(" + DT.FormatDate(DateTime.Now, 1) + ")|" + dc.LogText + "";
            new BCW.BLL.Textdc().UpdateLogText(bid, dc.UsID, LogText);

            Utils.Success("闲家确认", "确认" + acText + "" + Math.Abs(dc2.AcCent) + "" + bzText + "成功.", Utils.getUrl("dclist.aspx?act=list&amp;forumid=" + forumid + "&amp;bid=" + bid + ""), "2");
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">确认庄赢</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("庄家:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + dc.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(dc.UsID) + "(" + dc.UsID + ")</a><br />");
            builder.Append("闲保证金:" + dc2.OutCent + "" + bzText + "<br />");
            builder.Append("结果:" + acText + "" + Math.Abs(dc2.AcCent) + "" + bzText + "");
            builder.Append(Out.Tab("</div>", "<br />"));
            string strName = "forumid,bid,id,act,info,backurl";
            string strValu = "" + forumid + "'" + bid + "'" + id + "'winok'ok'" + Utils.getPage(0) + "";
            string strOthe = "同意结果,dclist.aspx,post,0,red";

            builder.Append(Out.wapform(strName, strValu, strOthe));
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getPage("dclist.aspx?act=list&amp;forumid=" + forumid + "&amp;bid=" + bid + "") + "\">上级</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "") + "\">主题</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }


    private void Win2Page(int forumid, int bid)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));

        Master.Title = "庄家申请";

        BCW.Model.Textdc dc = new BCW.BLL.Textdc().GetTextdc(bid, 0, meid);
        if (dc == null)
        {
            Utils.Error("不存在的记录", "");
        }
        string bzText = string.Empty;
        if (dc.BzType == 0)
            bzText = ub.Get("SiteBz");
        else
            bzText = ub.Get("SiteBz2");

        BCW.Model.Textdc dc2 = new BCW.BLL.Textdc().GetTextdc2(id, bid);
        if (dc2 == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (dc2.State == 1 || dc2.State == 2)
        {
            Utils.Error("已成功申请，请耐心等待对方进行回应", "");
        }
        if (dc2.State == 3)
        {
            Utils.Error("这个记录已结束", "");
        }
        string info = Utils.GetRequest("info", "post", 1, "", "");
        if (info == "ok")
        {
            int ptype = int.Parse(Utils.GetRequest("ptype", "post", 2, @"^[1-2]$", "结果选择错误"));
            long cent = Int64.Parse(Utils.GetRequest("cent", "post", 2, @"^[0-9]\d*$", "币额填写错误"));
            string acText = string.Empty;


            if (ptype == 1)
            {
                if (cent > dc.OutCent)
                {
                    Utils.Error("币数超出庄家赔付额度", "");
                }
                acText = "闲(" + dc2.UsID + ")赢";
                new BCW.BLL.Textdc().UpdateState(id, 2, -cent);

            }
            else
            {
                if (cent > dc2.OutCent)
                {
                    Utils.Error("币数超出闲家赔付额度", "");
                }
                acText = "庄(" + dc.UsID + ")赢";
                new BCW.BLL.Textdc().UpdateState(id, 2, cent);
            }

            string mename = new BCW.BLL.User().GetUsName(meid);
            string username = new BCW.BLL.User().GetUsName(dc2.UsID);
            //记录日志
            string LogText = "" + mename + "(" + meid + ")申请" + acText + "" + cent + "" + bzText + "(" + DT.FormatDate(DateTime.Now, 1) + ")|" + dc.LogText + "";
            new BCW.BLL.Textdc().UpdateLogText(bid, dc.UsID, LogText);
            //给对方发送内线
            new BCW.BLL.Guest().Add(dc2.UsID, username, "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]请求您确认帖子竞猜结果：申请" + acText + "" + cent + "" + bzText + "[url=/bbs/dclist.aspx?act=list&amp;forumid=" + forumid + "&amp;bid=" + bid + "]查看详情[/url]");

            Utils.Success("庄家申请", "庄家申请成功，已申请" + acText + "" + cent + "" + bzText + "，请耐心等待闲家确认..", Utils.getUrl("dclist.aspx?act=list&amp;forumid=" + forumid + "&amp;bid=" + bid + ""), "2");
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">庄家申请</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("闲家:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + dc2.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(dc2.UsID) + "(" + dc2.UsID + ")</a><br />");
            builder.Append("闲保证金:" + dc2.OutCent + "" + bzText + "<br />");
            builder.Append("庄保证金:" + dc.OutCent + "" + bzText + "");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "结果:,币额(含本金):,,,,,,";
            string strName = "ptype,cent,forumid,bid,id,act,info,backurl";
            string strType = "select,stext,hidden,hidden,hidden,hidden,hidden,hidden";
            string strValu = "2''" + forumid + "'" + bid + "'" + id + "'win2'ok'" + Utils.getPage(0) + "";
            string strEmpt = "2|庄赢|1|闲赢,false,false,false,false,false,false,false";
            string strIdea = "'" + bzText + "''''''|/";
            string strOthe = "确定结果,dclist.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
            builder.Append("温馨提示:<br />当币额填写0时，结果将相当于平手计算.<br />如对方超出12小时不确认您的申请，请联系版主进行确认.");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getPage("dclist.aspx?act=list&amp;forumid=" + forumid + "&amp;bid=" + bid + "") + "\">上级</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "") + "\">主题</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }


    private void Win2OkPage(int forumid, int bid)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));

        Master.Title = "庄家确认结果";

        BCW.Model.Textdc dc = new BCW.BLL.Textdc().GetTextdc(bid, 0);
        if (dc == null)
        {
            Utils.Error("不存在的记录", "");
        }
        string bzText = string.Empty;
        if (dc.BzType == 0)
            bzText = ub.Get("SiteBz");
        else
            bzText = ub.Get("SiteBz2");

        BCW.Model.Textdc dc2 = new BCW.BLL.Textdc().GetTextdc2(id, bid);
        if (dc2 == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (dc2.State == 3)
        {
            Utils.Error("这个记录已结束", "");
        }
        if (dc2.State != 1)
        {
            Utils.Error("不存在的记录", "");
        }
        if (dc.UsID != meid)
        {
            Utils.Error("不存在的记录", "");
        }
        string acText = string.Empty;
        if (dc2.AcCent < 0)
        {
            if (Math.Abs(dc2.AcCent) > dc.OutCent)
            {
                Utils.Error("币数超出庄家赔付额度", "");
            }
            acText = "闲(" + dc2.UsID + ")赢";
        }
        else
        {
            if (Math.Abs(dc2.AcCent) > dc2.OutCent)
            {
                Utils.Error("币数超出闲家赔付额度", "");
            }
            acText = "庄(" + dc.UsID + ")赢";
        }
        string info = Utils.GetRequest("info", "post", 1, "", "");
        if (info == "ok")
        {
            string mename = new BCW.BLL.User().GetUsName(meid);
            string username = new BCW.BLL.User().GetUsName(dc2.UsID);
            BCW.Model.Textdc m = new BCW.Model.Textdc();
            m.BID = bid;
            m.UsID = dc.UsID;
            if (dc2.AcCent < 0)
            {
                m.OutCent = (dc2.AcCent + dc2.OutCent);
            }
            else
            {
                m.OutCent = dc2.AcCent;
            }
            m.IsZtid = 0;
            m.BzType = dc.BzType;
            m.AddTime = DateTime.Now;
            new BCW.BLL.Textdc().UpdateOutCent(m);

            if (dc2.AcCent < 0)
            {
                long cent = Convert.ToInt64(Math.Abs(dc2.AcCent));
                if (dc.BzType == 0)
                    new BCW.BLL.User().UpdateiGold(dc2.UsID, username, cent, "帖子竞猜获得");
                else
                    new BCW.BLL.User().UpdateiMoney(dc2.UsID, username, cent, "帖子竞猜获得");

                new BCW.BLL.Guest().Add(dc2.UsID, username, "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]已确认帖子竞猜结果：您获得了" + (Math.Abs(dc2.AcCent)) + "" + bzText + "[url=/bbs/dclist.aspx?act=list&amp;forumid=" + forumid + "&amp;bid=" + bid + "]查看详情[/url]");

            }
            else
            {
                long cent = Convert.ToInt64(dc2.OutCent - dc2.AcCent);
                if (cent > 0)
                {
                    if (dc.BzType == 0)
                        new BCW.BLL.User().UpdateiGold(dc2.UsID, username, cent, "帖子竞猜退回");
                    else
                        new BCW.BLL.User().UpdateiMoney(dc2.UsID, username, cent, "帖子竞猜退回");

                    new BCW.BLL.Guest().Add(dc2.UsID, username, "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]已确认帖子竞猜结果：您输了" + dc2.AcCent + "" + bzText + "，系统已将" + cent + "" + bzText + "返还您.[url=/bbs/dclist.aspx?act=list&amp;forumid=" + forumid + "&amp;bid=" + bid + "]查看详情[/url]");

                }
                else
                {
                    new BCW.BLL.Guest().Add(dc2.UsID, username, "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]已确认帖子竞猜结果：您输了" + dc2.AcCent + "" + bzText + "[url=/bbs/dclist.aspx?act=list&amp;forumid=" + forumid + "&amp;bid=" + bid + "]查看详情[/url]");
                }
            }

            new BCW.BLL.Textdc().UpdateState(id, 3);

            //记录日志
            string LogText = "" + mename + "(" + meid + ")确认" + acText + "" + Math.Abs(dc2.AcCent) + "" + bzText + "(" + DT.FormatDate(DateTime.Now, 1) + ")|" + dc.LogText + "";
            new BCW.BLL.Textdc().UpdateLogText(bid, dc.UsID, LogText);
            //给对方发送内线

            Utils.Success("庄家确认", "确认" + acText + "" + Math.Abs(dc2.AcCent) + "" + bzText + "成功.", Utils.getUrl("dclist.aspx?act=list&amp;forumid=" + forumid + "&amp;bid=" + bid + ""), "2");
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">确认结果</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("闲家:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + dc2.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(dc2.UsID) + "(" + dc2.UsID + ")</a><br />");
            builder.Append("闲保证金:" + dc2.OutCent + "" + bzText + "<br />");
            builder.Append("结果:" + acText + "" + Math.Abs(dc2.AcCent) + "" + bzText + "");
            builder.Append(Out.Tab("</div>", "<br />"));
            string strName = "forumid,bid,id,act,info,backurl";
            string strValu = "" + forumid + "'" + bid + "'" + id + "'win2ok'ok'" + Utils.getPage(0) + "";
            string strOthe = "同意结果,dclist.aspx,post,0,red";

            builder.Append(Out.wapform(strName, strValu, strOthe));
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getPage("dclist.aspx?act=list&amp;forumid=" + forumid + "&amp;bid=" + bid + "") + "\">上级</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "") + "\">主题</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private void WinNoPage(int forumid, int bid)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));

        Master.Title = "不同意结果";

        BCW.Model.Textdc dc2 = new BCW.BLL.Textdc().GetTextdc2(id, bid);
        if (dc2 == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (dc2.State == 3)
        {
            Utils.Error("这个记录已结束", "");
        }
        if (dc2.State != 2)
        {
            Utils.Error("不存在的记录", "");
        }
        BCW.Model.Textdc dc = new BCW.BLL.Textdc().GetTextdc(bid, 0);
        if (dc2.UsID != meid)
        {
            Utils.Error("不存在的记录", "");
        }
        string acText = string.Empty;
        if (dc2.AcCent < 0)
            acText = "闲(" + dc2.UsID + ")赢";
        else
            acText = "庄(" + dc.UsID + ")赢";

        string bzText = string.Empty;
        if (dc.BzType == 0)
            bzText = ub.Get("SiteBz");
        else
            bzText = ub.Get("SiteBz2");

        string info = Utils.GetRequest("info", "post", 1, "", "");
        if (info == "ok")
        {

            new BCW.BLL.Textdc().UpdateState(id, 0);
            string mename = new BCW.BLL.User().GetUsName(meid);
            string username = new BCW.BLL.User().GetUsName(dc.UsID);
            //记录日志
            string LogText = "" + mename + "(" + meid + ")不同意" + acText + "" + dc2.AcCent + "" + bzText + "(" + DT.FormatDate(DateTime.Now, 1) + ")|" + dc.LogText + "";
            new BCW.BLL.Textdc().UpdateLogText(bid, dc.UsID, LogText);

            //给对方发送内线
            new BCW.BLL.Guest().Add(dc.UsID, username, "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]不同意帖子竞猜结果，系统已将记录改为进行中状态[url=/bbs/dclist.aspx?act=list&amp;forumid=" + forumid + "&amp;bid=" + bid + "]查看详情[/url]");

            Utils.Success("不同意结果", "确认不同意" + acText + "成功，系统已将记录改为进行中状态.", Utils.getUrl("dclist.aspx?act=list&amp;forumid=" + forumid + "&amp;bid=" + bid + ""), "2");
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">确认不同意庄赢</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("庄家:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + dc.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(dc.UsID) + "(" + dc.UsID + ")</a><br />");
            builder.Append("闲保证金:" + dc2.OutCent + "" + bzText + "<br />");
            builder.Append("结果:" + acText + "" + Math.Abs(dc2.AcCent) + "" + bzText + "");
            builder.Append(Out.Tab("</div>", "<br />"));
            string strName = "forumid,bid,id,act,info,backurl";
            string strValu = "" + forumid + "'" + bid + "'" + id + "'winno'ok'" + Utils.getPage(0) + "";
            string strOthe = "不同意庄赢,dclist.aspx,post,0,red";

            builder.Append(Out.wapform(strName, strValu, strOthe));
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getPage("dclist.aspx?act=list&amp;forumid=" + forumid + "&amp;bid=" + bid + "") + "\">上级</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "") + "\">主题</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private void Win2NoPage(int forumid, int bid)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));

        Master.Title = "不同意结果";

        BCW.Model.Textdc dc2 = new BCW.BLL.Textdc().GetTextdc2(id, bid);
        if (dc2 == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (dc2.State == 3)
        {
            Utils.Error("这个记录已结束", "");
        }
        if (dc2.State != 1)
        {
            Utils.Error("不存在的记录", "");
        }
        BCW.Model.Textdc dc = new BCW.BLL.Textdc().GetTextdc(bid, 0);
        if (dc.UsID != meid)
        {
            Utils.Error("不存在的记录", "");
        }
        string acText = string.Empty;
        if (dc2.AcCent < 0)
            acText = "闲(" + dc2.UsID + ")赢";
        else
            acText = "庄(" + dc.UsID + ")赢";

        string bzText = string.Empty;
        if (dc.BzType == 0)
            bzText = ub.Get("SiteBz");
        else
            bzText = ub.Get("SiteBz2");

        string info = Utils.GetRequest("info", "post", 1, "", "");
        if (info == "ok")
        {

            new BCW.BLL.Textdc().UpdateState(id, 0);
            string mename = new BCW.BLL.User().GetUsName(meid);
            string username = new BCW.BLL.User().GetUsName(dc2.UsID);
            //记录日志
            string LogText = "" + mename + "(" + meid + ")不同意" + acText + "" + dc2.AcCent + "" + bzText + "(" + DT.FormatDate(DateTime.Now, 1) + ")|" + dc.LogText + "";
            new BCW.BLL.Textdc().UpdateLogText(bid, dc.UsID, LogText);
            //给对方发送内线
            new BCW.BLL.Guest().Add(dc2.UsID, username, "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]不同意帖子竞猜结果，系统已将记录改为进行中状态[url=/bbs/dclist.aspx?act=list&amp;forumid=" + forumid + "&amp;bid=" + bid + "]查看详情[/url]");

            Utils.Success("不同意结果", "确认不同意" + acText + "成功，系统已将记录改为进行中状态.", Utils.getUrl("dclist.aspx?act=list&amp;forumid=" + forumid + "&amp;bid=" + bid + ""), "2");
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">确认不同意结果</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("闲家:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + dc2.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(dc2.UsID) + "(" + dc2.UsID + ")</a><br />");
            builder.Append("闲交易:" + dc2.OutCent + "" + bzText + "<br />");
            builder.Append("结果:" + acText + "" + Math.Abs(dc2.AcCent) + "" + bzText + "");
            builder.Append(Out.Tab("</div>", "<br />"));
            string strName = "forumid,bid,id,act,info,backurl";
            string strValu = "" + forumid + "'" + bid + "'" + id + "'win2no'ok'" + Utils.getPage(0) + "";
            string strOthe = "不同意结果,dclist.aspx,post,0,red";

            builder.Append(Out.wapform(strName, strValu, strOthe));
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getPage("dclist.aspx?act=list&amp;forumid=" + forumid + "&amp;bid=" + bid + "") + "\">上级</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "") + "\">主题</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private void Win3Page(int forumid, int bid)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Master.Title = "取出庄币";

        if (new BCW.BLL.Textdc().Exists2(bid))
        {
            Utils.Error("当前不能取出币，原因还存在未确认的记录", "");
        }
        BCW.Model.Textdc dc = new BCW.BLL.Textdc().GetTextdc(bid, 0, meid);
        if (dc == null)
        {
            Utils.Error("不存在的记录", "");
        }
        string bzText = string.Empty;
        if (dc.BzType == 0)
            bzText = ub.Get("SiteBz");
        else
            bzText = ub.Get("SiteBz2");

        string info = Utils.GetRequest("info", "post", 1, "", "");
        if (info == "ok")
        {
            long cent = Int64.Parse(Utils.GetRequest("cent", "post", 4, @"^[1-9]\d*$", "币额填写错误"));
            if (cent > dc.OutCent)
            {
                Utils.Error("您最多可以提取" + dc.OutCent + "" + bzText + "", "");
            }
            string mename = new BCW.BLL.User().GetUsName(meid);
            BCW.Model.Textdc m = new BCW.Model.Textdc();
            m.BID = bid;
            m.UsID = meid;
            m.OutCent = -cent;
            m.IsZtid = 0;
            m.BzType = dc.BzType;
            m.AddTime = DateTime.Now;
            new BCW.BLL.Textdc().UpdateOutCent(m);

            if (dc.BzType == 0)
                new BCW.BLL.User().UpdateiGold(meid, mename, cent, "帖子竞猜提取");
            else
                new BCW.BLL.User().UpdateiMoney(meid, mename, cent, "帖子竞猜提取");

            //记录日志
            string LogText = "" + mename + "(" + meid + ")提取保证金" + cent + "" + bzText + "(" + DT.FormatDate(DateTime.Now, 1) + ")|" + dc.LogText + "";
            new BCW.BLL.Textdc().UpdateLogText(bid, dc.UsID, LogText);

            Utils.Success("取出庄币", "提取成功，已提取" + cent + "" + bzText + "", Utils.getUrl("dclist.aspx?act=win3&amp;forumid=" + forumid + "&amp;bid=" + bid + ""), "2");
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">取出庄币</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("庄保证金:" + dc.OutCent + "" + bzText + "<br />");
            builder.Append("---------<br />");
            builder.Append("=取出我的庄币=");
            builder.Append(Out.Tab("</div>", ""));

            string strText = ",,,,,";
            string strName = "cent,forumid,bid,act,info,backurl";
            string strType = "num,hidden,hidden,hidden,hidden,hidden";
            string strValu = "'" + forumid + "'" + bid + "'win3'ok'" + Utils.getPage(0) + "";
            string strEmpt = "false,false,false,false,false,false";
            string strIdea = "" + bzText + "'''''|/";
            string strOthe = "确定取出,dclist.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getPage("dclist.aspx?act=list&amp;forumid=" + forumid + "&amp;bid=" + bid + "") + "\">上级</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "") + "\">主题</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private void LogViewPage(int forumid, int bid)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Master.Title = "查看日志";
        builder.Append(Out.Tab("<div class=\"title\">查看日志</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("dclist.aspx?act=list&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">返回应战记录</a>");
        builder.Append(Out.Tab("</div>", ""));
        string LogView = new BCW.BLL.Textdc().GetLogText(bid);

        if (LogView != null)
        {
            LogView = LogView.Replace("|", "##");
            string[] sName = Regex.Split(LogView, "##");

            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string[] pageValUrl = { "act", "forumid", "bid", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            //总记录数
            recordCount = sName.Length;

            int stratIndex = (pageIndex - 1) * pageSize;
            int endIndex = pageIndex * pageSize;
            int k = 0;
            for (int i = 0; i < sName.Length; i++)
            {
                if (k >= stratIndex && k < endIndex)
                {
                    if ((k + 1) % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));

                    builder.Append("" + (i + 1) + "." + sName[i].ToString() + "");
                    builder.Append(Out.Tab("</div>", ""));
                }
                if (k == endIndex)
                    break;
                k++;
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
        builder.Append("<a href=\"" + Utils.getPage("dclist.aspx?act=list&amp;forumid=" + forumid + "&amp;bid=" + bid + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "") + "\">主题</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void WinAdminPage(int forumid, int bid)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        if (IsCTID(meid, forumid) == false)
        {
            Utils.Error("你的权限不足", "");
        }
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));

        Master.Title = "管理员定输赢";

        BCW.Model.Textdc dc2 = new BCW.BLL.Textdc().GetTextdc2(id, bid);
        if (dc2 == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (dc2.State == 3)
        {
            Utils.Error("这个记录已结束", "");
        }
        BCW.Model.Textdc dc = new BCW.BLL.Textdc().GetTextdc(bid, 0);

        string bzText = string.Empty;
        if (dc.BzType == 0)
            bzText = ub.Get("SiteBz");
        else
            bzText = ub.Get("SiteBz2");

        string info = Utils.GetRequest("info", "post", 1, "", "");
        if (info == "ok")
        {
            int ptype = int.Parse(Utils.GetRequest("ptype", "post", 2, @"^[1-2]$", "结果选择错误"));
            long cent = Int64.Parse(Utils.GetRequest("cent", "post", 2, @"^[1-9]\d*$", "币额填写错误"));
            string acText = string.Empty;
            long AcCent = 0;
            if (ptype == 1)
            {
                acText = "闲(" + dc2.UsID + ")赢";
                if (cent > dc.OutCent)
                {
                    Utils.Error("庄币已不够赔付", "");
                }
                new BCW.BLL.Textdc().UpdateState(id, 3, -cent);
                AcCent = -cent;
            }
            else
            {
                acText = "庄(" + dc.UsID + ")赢";
                if (cent > dc2.OutCent)
                {
                    Utils.Error("闲币已不够赔付", "");
                }
                new BCW.BLL.Textdc().UpdateState(id, 3, cent);
                AcCent = cent;
            }
            string mename = new BCW.BLL.User().GetUsName(dc2.UsID);
            string username = new BCW.BLL.User().GetUsName(dc.UsID);
            BCW.Model.Textdc m = new BCW.Model.Textdc();
            m.BID = bid;
            m.UsID = dc.UsID;
            m.OutCent = AcCent;
            m.IsZtid = 0;
            m.BzType = dc.BzType;
            m.AddTime = DateTime.Now;
            new BCW.BLL.Textdc().UpdateOutCent(m);
            if (AcCent < 0)
            {
                long cent2 = Convert.ToInt64(dc2.OutCent + Math.Abs(AcCent));
                if (dc.BzType == 0)
                    new BCW.BLL.User().UpdateiGold(dc2.UsID, mename, cent2, "帖子竞猜获得");
                else
                    new BCW.BLL.User().UpdateiMoney(dc2.UsID, mename, cent2, "帖子竞猜获得");
            }
            else
            {
                long cent2 = Convert.ToInt64(dc2.OutCent - AcCent);
                if (cent2 > 0)
                {
                    if (dc.BzType == 0)
                        new BCW.BLL.User().UpdateiGold(dc2.UsID, mename, cent2, "帖子竞猜退回");
                    else
                        new BCW.BLL.User().UpdateiMoney(dc2.UsID, mename, cent2, "帖子竞猜退回");

                }
            }
            //记录日志
            string LogText = "竞技管理员(ID:" + meid + ")确认帖子竞猜结果:" + acText + "" + Math.Abs(AcCent) + "" + bzText + "(" + DT.FormatDate(DateTime.Now, 1) + ")|" + dc.LogText + "";
            new BCW.BLL.Textdc().UpdateLogText(bid, dc.UsID, LogText);

            new BCW.BLL.Guest().Add(dc.UsID, username, "[url=/bbs/uinfo.aspx?uid=" + meid + "]竞技管理员(ID:" + meid + ")[/url]已确认帖子竞猜结果：" + acText + "" + Math.Abs(AcCent) + "" + bzText + "[url=/bbs/dclist.aspx?act=list&amp;forumid=" + forumid + "&amp;bid=" + bid + "]查看详情[/url]");
            new BCW.BLL.Guest().Add(dc2.UsID, mename, "[url=/bbs/uinfo.aspx?uid=" + meid + "]竞技管理员(ID:" + meid + ")[/url]已确认帖子竞猜结果：" + acText + "" + Math.Abs(AcCent) + "" + bzText + "[url=/bbs/dclist.aspx?act=list&amp;forumid=" + forumid + "&amp;bid=" + bid + "]查看详情[/url]");
            Utils.Success("决定输赢", "确认结果：" + acText + "" + AcCent + "" + bzText + "成功.", Utils.getUrl("dclist.aspx?act=list&amp;forumid=" + forumid + "&amp;bid=" + bid + ""), "2");
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">决定输赢</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("庄家:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + dc.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(dc.UsID) + "(" + dc.UsID + ")</a><br />");
            builder.Append("闲家:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + dc2.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(dc2.UsID) + "(" + dc2.UsID + ")</a><br />");
            builder.Append("庄保证金:" + dc.OutCent + "" + bzText + "<br />");
            builder.Append("闲保证金:" + dc2.OutCent + "" + bzText + "");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "结果:,币额:,,,,,,";
            string strName = "ptype,cent,forumid,bid,id,act,info,backurl";
            string strType = "select,stext,hidden,hidden,hidden,hidden,hidden,hidden";
            string strValu = "2''" + forumid + "'" + bid + "'" + id + "'winadmin'ok'" + Utils.getPage(0) + "";
            string strEmpt = "2|庄赢|1|闲赢,false,false,false,false,false,false,false";
            string strIdea = "'" + bzText + "''''''|/";
            string strOthe = "确定结果,dclist.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getPage("dclist.aspx?act=list&amp;forumid=" + forumid + "&amp;bid=" + bid + "") + "\">上级</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "") + "\">主题</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    /// <summary>
    /// 能帮确认的版主ID
    /// </summary>
    private bool IsCTID(int meid, int forumid)
    {
        bool Isvi = false;
        string CTID = "#10086#7888#4082#26115#1860#1001#19611#1002#";
        if (CTID.IndexOf("#" + meid + "#") != -1)
        {
            Isvi = true;
        }
        //if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_AddForumBlack, meid, forumid))
        //{
        //    Isvi = true;
        //}
        return Isvi;
    }
}