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

public partial class bbs_Gstoplist : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/bbs.xml";
    protected void Page_Load(object sender, EventArgs e)
    {

        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "top":
                TopPage();
                break;
            case "top2":
                Top2Page();
                break;
            case "cent":
                CentPage();
                break;
            case "centlist":
                CentListPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        int forumid = int.Parse(Utils.GetRequest("forumid", "all", 2, @"^[0-9]\d*$", "论坛ID错误"));
        if (!new BCW.BLL.Forum().Exists2(forumid))
        {
            Utils.Success("访问论坛", "该论坛不存在或已暂停使用", Utils.getUrl("forum.aspx"), "1");
        }
        if (new BCW.User.ForumInc().IsForumGSIDS(forumid) == true)
        { }
        else
        {
            Utils.Error("不存在的记录", "");
        }

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-2]$", "0"));
        Master.Title = "高手排行榜";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        string ForumTitle = new BCW.BLL.Forum().GetTitle(forumid);
        builder.Append("" + ForumTitle + "-排行榜");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));

        if (ptype == 0)
            builder.Append("连中榜|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Gstoplist.aspx?forumid=" + forumid + "&amp;ptype=0&amp;backurl=" + Utils.getPage(0) + "") + "\">连中榜</a>|");

        if (ptype == 1)
            builder.Append("月中榜|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Gstoplist.aspx?forumid=" + forumid + "&amp;ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">月中榜</a>|");

        if (ptype == 2)
            builder.Append("历史");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Gstoplist.aspx?forumid=" + forumid + "&amp;ptype=2&amp;backurl=" + Utils.getPage(0) + "") + "\">历史</a>");

        builder.Append("|<a href=\"" + Utils.getUrl("Gstoplist.aspx?act=centlist&amp;forumid=" + forumid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">获奖</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        string strOrder = string.Empty;
        string[] pageValUrl = { "act", "forumid", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        strWhere = "ForumId=" + forumid + " and Types=8";
        if (ptype == 0)
        {
            strWhere += " and Glznum>0";
            strOrder = "Glznum DESC";
        }
        else if (ptype == 1)
        {
            strWhere += " and Gmnum>0";
            strOrder = "Gmnum DESC";
        }
        else if (ptype == 2)
        {
            strWhere += " and Gwinnum>0";
            strOrder = "Gwinnum DESC";
        }

        string GsAdminID = ub.GetSub("BbsGsAdminID", xmlPath);

        // 开始读取列表
        IList<BCW.Model.Text> listText = new BCW.BLL.Text().GetTextsGs(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listText.Count > 0)
        {

            int k = 1;
            foreach (BCW.Model.Text n in listText)
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
                builder.Append(""+((pageIndex - 1) * pageSize + k)+".<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "</a>");
                if (ptype == 0)
                    builder.Append("连中" + n.Glznum + "");
                else if (ptype == 1)
                    builder.Append("月中" + n.Gmnum + "");
                else
                    builder.Append("" + n.Gaddnum + "中" + n.Gwinnum + "");

                builder.Append("<a href=\"" + Utils.getUrl("topic.aspx?forumid=" + n.ForumId + "&amp;bid=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.Title + "</a>");


                if (("#" + GsAdminID + "#").Contains("#" + meid + "#"))
                {
                    builder.Append("|<a href=\"" + Utils.getUrl("Gstoplist.aspx?act=cent&amp;forumid=" + n.ForumId + "&amp;bid=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">奖励</a>");
                }

                k++;
                builder.Append(Out.Tab("</div>", ""));

            }

            // 分页
            builder.Append(BasePage.ForumMultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("Gstoplist.aspx?act=top&amp;backurl=" + Utils.getPage(0) + "") + "\">【各坛排行记录】</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("Gstoplist.aspx?act=top2&amp;backurl=" + Utils.getPage(0) + "") + "\">【各坛奖励记录】</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx?backurl=" + Utils.getPage(0) + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "") + "\">论坛</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void CentPage()
    {
        int forumid = int.Parse(Utils.GetRequest("forumid", "all", 2, @"^[0-9]\d*$", "论坛ID错误"));
        if (!new BCW.BLL.Forum().Exists2(forumid))
        {
            Utils.Success("访问论坛", "该论坛不存在或已暂停使用", Utils.getUrl("forum.aspx"), "1");
        }
        if (new BCW.User.ForumInc().IsForumGSIDS(forumid) == true)
        { }
        else
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "奖励会员";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string GsAdminID = ub.GetSub("BbsGsAdminID", xmlPath);
        if (!("#" + GsAdminID + "#").Contains("#" + meid + "#"))
        {
            Utils.Error("权限不足", "");
        }

        int bid = int.Parse(Utils.GetRequest("bid", "all", 2, @"^[0-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Text().Exists2(bid, forumid))
        {
            Utils.Error("帖子不存在或已被删除", "");
        }
        BCW.Model.Text model = new BCW.BLL.Text().GetText(bid);

        string info = Utils.GetRequest("info", "post", 1, "", "");
        if (info != "")
        {
            string Content = Utils.GetRequest("Content", "post", 2, @"^[\s\S]{1,50}$", "奖励原因限1-50字内，不能留空");
            long Gold = Int64.Parse(Utils.GetRequest("iGold", "post", 1, @"^[0-9]\d*$", "0"));
            long Money = Int64.Parse(Utils.GetRequest("iMoney", "post", 1, @"^[0-9]\d*$", "0"));
            long Score = Int64.Parse(Utils.GetRequest("iScore", "post", 1, @"^[0-9]\d*$", "0"));
            int Vip = int.Parse(Utils.GetRequest("iVip", "post", 1, @"^[0-3]$", "0"));

            string LogText = "" + Content + "";
            if (Gold > 0)
                LogText += "/奖" + Gold + "" + ub.Get("SiteBz") + "";
            if (Money > 0)
                LogText += "/奖" + Money + "" + ub.Get("SiteBz2") + "";
            if (Score > 0)
                LogText += "/奖" + Score + "积分";
            if (Vip > 0)
                LogText += "/奖VIP" + Vip + "月";

            if (info == "ok")
            {
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("奖励对象：<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.UsName + "(" + model.UsID + ")</a>");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("奖励并生成日志：" + LogText + "");
                builder.Append(Out.Tab("</div>", "<br />"));

                string strName = "Content,iGold,iMoney,iScore,iVip,forumid,bid,act,info,backurl";
                string strValu = "" + Content + "'" + Gold + "'" + Money + "'" + Score + "'" + Vip + "'" + forumid + "'" + bid + "'cent'ok2'" + Utils.getPage(0) + "'";
                string strOthe = "确认奖励,Gstoplist.aspx,post,0,red";

                builder.Append(Out.wapform(strName, strValu, strOthe));
            }
            else
            {
                if (Gold > 0)
                {
                    new BCW.BLL.User().UpdateiGold(model.UsID, model.UsName, Gold, Content);
                }
                if (Money > 0)
                {
                    new BCW.BLL.User().UpdateiMoney(model.UsID, model.UsName, Money, Content);
                }
                if (Score > 0)
                {
                    new BCW.BLL.User().UpdateiScore(model.UsID, Score);
                }
                if (Vip > 0)
                {
                    string xmlPathvip = "/Controls/bbs.xml";
                    int VipGrow1 = Utils.ParseInt(ub.GetSub("VipGrow1", xmlPathvip));
                    int VipGrow2 = Utils.ParseInt(ub.GetSub("VipGrow2", xmlPathvip));
                    int VipGrow3 = Utils.ParseInt(ub.GetSub("VipGrow3", xmlPathvip));
                    int VipGrow4 = Utils.ParseInt(ub.GetSub("VipGrow4", xmlPathvip));

                    int Grow = 0;
                    int Day = 0;
                    if (Vip == 1)
                    {
                        Grow = VipGrow1;
                        Day = 30;
                    }
                    else if (Vip == 2)
                    {
                        Grow = VipGrow2;
                        Day = 90;
                    }
                    else if (Vip == 3)
                    {
                        Grow = VipGrow3;
                        Day = 180;
                    }

                    BCW.Model.User model2 = new BCW.BLL.User().GetVipData(model.UsID);
                    try
                    {
                        if (model2.VipDate != null && model2.VipDate > DateTime.Now)
                            new BCW.BLL.User().UpdateVipData(model.UsID, Grow, model2.VipDate.AddDays(Day));
                        else
                            new BCW.BLL.User().UpdateVipData(model.UsID, Grow, DateTime.Now.AddDays(Day));
                    }
                    catch
                    {
                        new BCW.BLL.User().UpdateVipData(model.UsID, Grow, DateTime.Now.AddDays(Day));
                    }
                    //清缓存
                    string CacheKey = CacheName.App_UserVip(model.UsID);
                    DataCache.RemoveByPattern(CacheKey);
                }
                //记录日志
                BCW.Model.Forumvotelog m = new BCW.Model.Forumvotelog();
                m.UsID = model.UsID;
                m.UsName = model.UsName;
                m.AdminId = meid;
                m.BID = bid;
                m.ForumId = forumid;
                m.Title = model.Title;
                m.Notes = LogText;
                m.AddTime = DateTime.Now;
                new BCW.BLL.Forumvotelog().Add(m);
                   
                Utils.Success("奖励会员", "恭喜，奖励成功，正在返回..", Utils.getPage("Gstoplist.aspx?forumid=" + forumid + ""), "2");

            }

        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("主题：<a href=\"" + Utils.getUrl("topic.aspx?forumid=" + model.ForumId + "&amp;bid=" + bid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.Title + "</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("奖励对象：<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.UsName + "(" + model.UsID + ")</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("Gstoplist.aspx?act=centlist&amp;forumid=" + model.ForumId + "&amp;hid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">查看奖励记录&gt;&gt;</a>");            
            builder.Append(Out.Tab("</div>", ""));
            string strText = "奖励原因(如填写:2013001期5连中):/,奖" + ub.Get("SiteBz") + ":/,奖" + ub.Get("SiteBz2") + ":/,奖积分:/,奖VIP:/,,,,,,";
            string strName = "Content,iGold,iMoney,iScore,iVip,forumid,bid,act,info,backurl";
            string strType = "textarea,num,num,num,select,hidden,hidden,hidden,hidden,hidden";
            string strValu = "''''0'" + forumid + "'" + bid + "'cent'ok'" + Utils.getPage(0) + "";
            string strEmpt = "true,true,true,true,0|不赠送|1|1个月|2|3个月|3|6个月,false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定奖励,Gstoplist.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
            builder.Append("<a href=\"" + Utils.getPage("Gstoplist.aspx?forumid=" + forumid + "") + "\">&lt;&lt;返回上级</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?backurl=" + Utils.getPage(0) + "") + "\">上级</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "") + "\">论坛</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private void CentListPage()
    {
        int forumid = int.Parse(Utils.GetRequest("forumid", "all", 1, @"^[0-9]\d*$", "0"));
        if (forumid > 0)
        {
            if (!new BCW.BLL.Forum().Exists2(forumid))
            {
                Utils.Success("访问论坛", "该论坛不存在或已暂停使用", Utils.getUrl("forum.aspx"), "1");
            }
            if (new BCW.User.ForumInc().IsForumGSIDS(forumid) == true)
            { }
            else
            {
                Utils.Error("不存在的记录", "");
            }
        }
        Master.Title = "高手奖励记录";

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string Title = string.Empty;
        int hid = int.Parse(Utils.GetRequest("hid", "all", 1, @"^[0-9]\d*$", "0"));
        if (hid > 0)
        {
            Title = new BCW.BLL.User().GetUsName(hid);
            if (Title == "")
                Utils.Error("不存在的会员记录", "");

            Title = "<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + hid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + Title + "(" + hid + ")</a>";

        }
        else
        {
            if (forumid > 0)
                Title = new BCW.BLL.Forum().GetTitle(forumid);
            else
                Title = "财经论坛";
        }

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("" + Title + "-奖励记录");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        string strOrder = string.Empty;
        string[] pageValUrl = { "act", "forumid", "hid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        if (hid > 0)
            strWhere = "UsID=" + hid + "";
        else
        {
            if (forumid > 0)
                strWhere = "ForumId=" + forumid + "";
        }
        // 开始读取列表
        IList<BCW.Model.Forumvotelog> listForumvotelog = new BCW.BLL.Forumvotelog().GetForumvotelogs(pageIndex, pageSize, strWhere, out recordCount);
        if (listForumvotelog.Count > 0)
        {

            int k = 1;
            foreach (BCW.Model.Forumvotelog n in listForumvotelog)
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
                if (hid == 0)
                {
                    builder.Append("奖励会员:<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "</a><br />");
                }
                builder.Append("主题:<a href=\"" + Utils.getUrl("topic.aspx?forumid=" + n.ForumId + "&amp;bid=" + n.BID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.Title + "</a>(" + DT.FormatDate(n.AddTime, 1) + ")");
                builder.Append("<br />" + n.Notes);

                k++;
                builder.Append(Out.Tab("</div>", ""));

            }

            // 分页
            builder.Append(BasePage.ForumMultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }

        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        if (hid > 0)
        {
            builder.Append("<a href=\"" + Utils.getUrl("Gstoplist.aspx?act=centlist&amp;forumid=" + forumid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">本坛奖励记录&gt;&gt;</a><br />");
        }
        else
        {
            if (forumid > 0)
                builder.Append("<a href=\"" + Utils.getUrl("Gstoplist.aspx?act=centlist&amp;backurl=" + Utils.getPage(0) + "") + "\">全部奖励记录&gt;&gt;</a><br />");
        }

        builder.Append("<a href=\"" + Utils.getPage("forum.aspx?forumid=" + forumid + "") + "\">&lt;&lt;返回上级</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?backurl=" + Utils.getPage(0) + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "") + "\">论坛</a>");
        builder.Append(Out.Tab("</div>", ""));
    }


    private void TopPage()
    {
        Master.Title = "分类排行榜";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("分类排行榜");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Gstoplist.aspx?act=toplist&amp;forumid=113&amp;backurl=" + Utils.getPage(0) + "") + "\">六肖区</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("Gstoplist.aspx?act=toplist&amp;forumid=114&amp;backurl=" + Utils.getPage(0) + "") + "\">波色区</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("Gstoplist.aspx?act=toplist&amp;forumid=116&amp;backurl=" + Utils.getPage(0) + "") + "\">平特一肖区</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("Gstoplist.aspx?act=toplist&amp;forumid=117&amp;backurl=" + Utils.getPage(0) + "") + "\">五不中区</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("Gstoplist.aspx?act=toplist&amp;forumid=115&amp;backurl=" + Utils.getPage(0) + "") + "\">大小区</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("Gstoplist.aspx?act=toplist&amp;forumid=121&amp;backurl=" + Utils.getPage(0) + "") + "\">单双区</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("Gstoplist.aspx?act=toplist&amp;forumid=122&amp;backurl=" + Utils.getPage(0) + "") + "\">家野区</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("Gstoplist.aspx?act=toplist&amp;forumid=119&amp;backurl=" + Utils.getPage(0) + "") + "\">五尾区</a>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        builder.Append(Out.back("返回上级"));
        builder.Append(Out.Tab("</div>", ""));

    }

    private void Top2Page()
    {
        Master.Title = "分类奖励记录";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("分类奖励记录");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Gstoplist.aspx?act=centlist&amp;forumid=113&amp;backurl=" + Utils.getPage(0) + "") + "\">六肖区</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("Gstoplist.aspx?act=centlist&amp;forumid=114&amp;backurl=" + Utils.getPage(0) + "") + "\">波色区</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("Gstoplist.aspx?act=centlist&amp;forumid=116&amp;backurl=" + Utils.getPage(0) + "") + "\">平特一肖区</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("Gstoplist.aspx?act=centlist&amp;forumid=117&amp;backurl=" + Utils.getPage(0) + "") + "\">五不中区</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("Gstoplist.aspx?act=centlist&amp;forumid=115&amp;backurl=" + Utils.getPage(0) + "") + "\">大小区</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("Gstoplist.aspx?act=centlist&amp;forumid=121&amp;backurl=" + Utils.getPage(0) + "") + "\">单双区</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("Gstoplist.aspx?act=centlist&amp;forumid=122&amp;backurl=" + Utils.getPage(0) + "") + "\">家野区</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("Gstoplist.aspx?act=centlist&amp;forumid=119&amp;backurl=" + Utils.getPage(0) + "") + "\">五尾区</a>");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "输入用户ID:/,,";
        string strName = "hid,act,backurl";
        string strType = "num,hidden,hidden";
        string strValu = "'centlist'" + Utils.getPage(0) + "";
        string strEmpt = "true,false,false";
        string strIdea = "/";
        string strOthe = "搜奖励记录,Gstoplist.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        builder.Append(Out.back("返回上级"));
        builder.Append(Out.Tab("</div>", ""));

    }
}
