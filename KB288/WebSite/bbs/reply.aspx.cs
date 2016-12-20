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
///  修改人陈志基 2016 0428
///  修改派币问题
/// </summary>
/// <summary>
/// 蒙宗将 20160822 撤掉抽奖值生成
/// </summary>

public partial class bbs_reply : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/bbs.xml";
    protected string strText = string.Empty;
    protected string strName = string.Empty;
    protected string strType = string.Empty;
    protected string strValu = string.Empty;
    protected string strEmpt = string.Empty;
    protected string strIdea = string.Empty;
    protected string strOthe = string.Empty;

    // protected int paytype = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
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
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "reply":
            case "reply2":
                ReplyPage(act, forumid, bid);
                break;
            case "view":
                ViewPage(forumid, bid);
                break;
            case "save":
                SavePage(forumid, bid);
                break;
            case "edit":
                EditPage(forumid, bid);
                break;
            case "del":
                DelPage(forumid, bid);
                break;
            case "good":
            case "delgood":
                GoodPage(act, forumid, bid);
                break;
            case "forpraise":
                ForPraise(forumid, bid);
                break;
            case "praiseranking":
                PraiseRanking(forumid, bid);
                break;
            case "top":
            case "deltop":
                TopPage(act, forumid, bid);
                break;
            case "reward":
                RewardPage(forumid, bid);
                break;

            case "rewardlist":
                RewardListPage(forumid, bid);
                break;
            case "forgood":
                ForGoodPage(forumid, bid);
                break;
            case "forgoodlist":
                ForGoodListPage(forumid, bid);
                break;
            case "forgoodtop":
                ForGoodTopPage(forumid, bid);
                break;
            default:
                ReloadPage(forumid, bid);
                break;
        }
    }

    /// <summary>
    /// 回帖列表
    /// </summary>
    private void ReloadPage(int forumid, int bid)
    {
        int FsPageSize = 10;
        int FsIsImg = 0;
        int FsReLength = 30;
        int meid = new BCW.User.Users().GetUsId();
        //论坛限制性
        BCW.Model.Forum Forummodel = new BCW.BLL.Forum().GetForum(forumid);
        BCW.User.Users.ShowForumLimit(meid, Forummodel.Gradelt, Forummodel.Visitlt, Forummodel.VisitId, Forummodel.IsPc);
        if (meid > 0)
        {
            string ForumSet = new BCW.BLL.User().GetForumSet(meid);
            FsPageSize = BCW.User.Users.GetForumSet(ForumSet, 2);
            FsReLength = BCW.User.Users.GetForumSet(ForumSet, 3);
            FsIsImg = BCW.User.Users.GetForumSet(ForumSet, 7);
        }
        int showtype = int.Parse(Utils.GetRequest("showtype", "get", 1, @"^[0-9]\d*$", "0"));
        int ordertype = int.Parse(Utils.GetRequest("ordertype", "get", 1, @"^[0-9]\d*$", "0"));
        if (showtype == 0)
            Master.Title = "最新回帖";
        else if (showtype == 1)
            Master.Title = "精华回帖";
        else
            Master.Title = "楼主回帖";

        //得到帖子类型
        int Types = new BCW.BLL.Text().GetTypes(bid);
        int TextUsId = 0;
        if (showtype == 2 || Types == 7)
        {
            TextUsId = new BCW.BLL.Text().GetUsID(bid);
        }

        int pageIndex;
        int recordCount;
        int pageSize = FsPageSize;
        string strWhere = string.Empty;
        string strOrder = string.Empty;
        string[] pageValUrl = { "forumid", "bid", "showtype", "ordertype" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //拾物随机
        builder.Append(BCW.User.Game.GiftFlows.ShowGiftFlows(1));
        //顶部滚动
        builder.Append(BCW.User.Master.OutTopRand(3));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "") + "\">" + Forummodel.Title + "</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "") + "\">主题帖</a>&gt;");
        builder.Append("回帖");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?act=reply&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">回复</a>|");

        if (showtype == 0)
            builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;showtype=1&amp;ordertype=" + ordertype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">精华</a>|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;showtype=0&amp;ordertype=" + ordertype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">最新</a>|");

        builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;showtype=" + showtype + "&amp;ordertype=" + ordertype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">刷新</a>|");
        if (ordertype == 0)
            builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;showtype=" + showtype + "&amp;ordertype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">正序</a>");
        else
            builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;showtype=" + showtype + "&amp;ordertype=0&amp;backurl=" + Utils.getPage(0) + "") + "\">倒序</a>");
        builder.Append(Out.Tab("</div>", ""));

        if (Types == 7)
        {
            BCW.Model.Text m = new BCW.BLL.Text().GetPriceModel(bid);
            if (m != null)
            {
                string bzText = string.Empty;
                if (m.BzType == 0)
                    bzText = ub.Get("SiteBz");
                else
                    bzText = ub.Get("SiteBz2");

                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("悬赏:" + m.Prices + "/");
                builder.Append("剩余:" + (m.Prices - m.Pricel) + "" + bzText + "<br />");
                builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?act=rewardlist&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">悬赏名单&gt;&gt;</a>");
                builder.Append(Out.Tab("</div>", Out.LHr()));
            }
        }

        //查询条件
        strWhere = "Bid=" + bid + " and IsDel=0 ";
        if (showtype == 1)
            strWhere += " and IsGood=1";
        else if (showtype == 2)
            strWhere += " and UsID=" + TextUsId + "";
        if (ordertype == 1)
            strOrder = "id asc";
        else
            strOrder = "Istop Desc,AddTime Desc";

        // 开始读取列表
        IList<BCW.Model.Reply> listReply = new BCW.BLL.Reply().GetReplys(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listReply.Count > 0)
        {
            //算出总页数
            int pageTotal = BasePage.CalcPageCount(recordCount, pageSize, ref pageIndex);
            if (pageTotal > pageIndex || pageIndex != 1)
                builder.Append(Out.Tab("<div>", "<br />"));

            if (pageTotal > pageIndex)
            {
                builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;showtype=" + showtype + "&amp;ordertype=" + ordertype + "&amp;page=" + (pageIndex + 1) + "&amp;backurl=" + Utils.getPage(0) + "") + "\">下一页</a> ");
            }
            if (pageIndex != 1)
            {
                builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;showtype=" + showtype + "&amp;ordertype=" + ordertype + "&amp;page=" + (pageIndex - 1) + "&amp;backurl=" + Utils.getPage(0) + "") + "\">上一页</a> ");
            }
            if (pageTotal > pageIndex || pageIndex != 1)
                builder.Append(Out.Tab("</div>", ""));

            int k = 1;
            foreach (BCW.Model.Reply n in listReply)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                    builder.Append(Out.Tab("<div>", "<br />"));

                if (n.IsTop == 1)
                    builder.Append("[顶]");

                if (n.IsGood == 1)
                    builder.Append("[精]");

                if (n.FileNum > 0)
                    builder.Append("[附]");
                //这里开始修改

                builder.AppendFormat("<b>{0}楼</b>:", n.Floor);

                string reContent = string.Empty;
                if (FsIsImg == 1)
                    reContent = Utils.ClearImg(Out.SysUBB(n.Content));
                else
                    reContent = Out.SysUBB(n.Content);

                if (!string.IsNullOrEmpty(reContent))
                {
                    builder.Append(reContent);
                }

                if (!string.IsNullOrEmpty(n.CentText))
                    builder.Append("[获" + n.CentText + "]");
                else
                {
                    if (Types == 7 && TextUsId == meid)
                    {
                        builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?act=reward&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + n.Floor + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">赏</a> ");
                    }
                }
                builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?act=view&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + n.Floor + "&amp;page=" + pageIndex + "&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;&gt;</a> ");
                builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?act=reply&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + n.Floor + "&amp;page=" + pageIndex + "&amp;backurl=" + Utils.getPage(0) + "") + "\">评</a> ");

                if (n.ReplyId > 0)//回复多少楼的，如果false就是直接回复帖
                {
                    builder.Append("<br/>回复<a href=\"" + Utils.getUrl("reply.aspx?act=view&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + n.ReplyId + "&amp;page=" + pageIndex + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + n.ReplyId + "楼</a>内容:");
                    string neirong = new BCW.BLL.Reply().GetContent(bid, n.ReplyId);
                    if (neirong.Length > 15)
                    {
                        neirong = neirong.Substring(0, 15);
                        neirong += "...";
                        builder.Append(Out.SysUBB(neirong));
                    }
                    else
                    {
                        builder.Append(Out.SysUBB(neirong));

                    }
                }

                //图片直接显示
                if (n.FileNum > 0)
                {
                    builder.Append("<br />");
                    int pSize = 3;
                    string sWhere = "Types=1 and BID=" + bid + " and ReID=" + n.ID + "";
                    IList<BCW.Model.Upfile> listUpfile = new BCW.BLL.Upfile().GetUpfiles(pSize, sWhere);
                    if (listUpfile.Count > 0)
                    {
                        foreach (BCW.Model.Upfile nn in listUpfile)
                        {
                            string upContent = nn.Content;
                            if (string.IsNullOrEmpty(upContent))
                                upContent = "无标题";

                            builder.Append("<a href=\"" + Utils.getUrl("/showpic.aspx?pic=" + nn.PrevFiles + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"><img src=\"" + nn.PrevFiles + "\" alt=\"load\"/></a><br />" + upContent + "<br />");
                        }
                    }
                    builder.Append("<a href=\"" + Utils.getUrl("filelist.aspx?act=file&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">查看附件(共" + n.FileNum + "个)</a>");
                }


                n.UsName = BCW.User.Users.SetVipName(n.UsID, n.UsName, false);

                builder.Append("<br /><a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;" + n.UsName + "</a><font size=\" 1px\" >:" + DT.FormatDate(n.AddTime, 6) + "</font>");

                k++;
                //   需要修改textcent表增加回帖打赏字段
                BCW.Model.Textcent tt = new BCW.BLL.Textcent().GetTextcentReply(n.UsID, n.Floor, bid);
                if (tt != null)   //如果有该回帖有给打赏就显示打赏的
                {
                    // builder.Append(Out.Tab("<div class=\"text\">", ""));
                    builder.Append("<br/><b>友友打赏</b>|<a href=\"" + Utils.getUrl("reply.aspx?act=forgoodtop&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">排行</a>");
                    string bzText = string.Empty;
                    if (tt.BzType == 0)
                        bzText = ub.Get("SiteBz");
                    else
                        bzText = ub.Get("SiteBz2");

                    builder.Append("<br />" + tt.UsName + ":打赏" + tt.Cents + "" + bzText + ".");
                    builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?act=forgoodlist&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">更多</a>");
                  //  builder.Append(Out.Tab("</div>", ""));
                }
                builder.Append("<br/>----------------------------");
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.ForumMultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Tab("", "<br />"));
            builder.Append(Out.Div("div", "没有相关记录.."));
        }

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">返回主题帖</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        //得到随机短语
        string ReplyValu = string.Empty;

        strText = ",,,,,,";
        strName = "Face,Content,forumid,bid,backurl,act";
        strType = "select,text,hidden,hidden,hidden,hidden";
        strValu = "0'" + ReplyValu + "'" + forumid + "'" + bid + "'" + Utils.getPage(0) + "'save";
        strEmpt = "0|表情|1|微笑|2|哭泣|3|调皮|4|脸红|5|害羞|6|生气|7|偷笑|8|流汗|9|我倒|10|得意|11|眦牙|12|疑问|13|睡觉|14|好色|15|口水|16|敲头|17|可爱|18|猪头|19|胜利|20|玫瑰|21|吃药|22|白酒|23|可乐|24|囧囧|25|亲亲|26|抱抱,true,false,false,false,false";
        strIdea = "";
        strOthe = "回复,reply.aspx,post,3,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        if (Utils.getPage(1) != "")
        {
            builder.Append("<a href=\"" + Utils.getPage(1) + "\">上级</a>|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?act=reply&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">回复</a>|");
        }
        builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "") + "\">其它帖子</a>");
        builder.Append(Out.Tab("</div>", ""));

    }
    /// <summary>
    /// 
    /// 点赞加一 陈志基
    /// </summary>
    private void ForPraise(int forumid, int bid)
    {
        Master.Title = "点赞";
        BCW.Model.Text p = new BCW.BLL.Text().GetText(bid);
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        //更新回复ID
        int existreply = new BCW.BLL.Reply().GetCount2(meid, bid);
        if (existreply > 0) //判断有误回复
        {
            // builder.Append("用户：" + meid + "回复了" + existreply + "次");
            string sReplyID = p.PraiseID;
            if (!("#" + sReplyID + "#").Contains("#" + meid + "#"))
            //if (("#" + sReplyID + "#").IndexOf("#" + meid + "#") == -1)
            {
                if (string.IsNullOrEmpty(sReplyID))
                    sReplyID = meid.ToString();
                else
                    sReplyID = meid + "#" + sReplyID;

                new BCW.BLL.Text().UpdatePraiseID(bid, sReplyID);
                int praise = new BCW.BLL.Text().GetPraise(bid);
                praise = praise + 1;
                new BCW.BLL.Text().UpdatePraise(bid, praise);
                // new BCW.BLL.Text().UpdatePraiseID(bid, null);
                new BCW.BLL.Text().UpdatePraiseTime(bid, DateTime.Now);
                Utils.Success("点赞成功", "点赞成功！", Utils.getUrl("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + ""), "1");

            }
            else
            {
                Utils.Error("你已经点过赞啦", "");
            }


        }
        else  //没有回复不能点赞
        {
            //Utils.Error("你还没有回复！请回复后再点赞！", "");
            string ForumTitle = new BCW.BLL.Forum().GetTitle(forumid);
            builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
            builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "") + "\">" + ForumTitle + "</a>&gt;");
            builder.Append("<a href=\"" + Utils.getUrl("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "") + "\">主题帖</a>&gt;");
            builder.Append("点赞");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
            builder.Append("你还没有回复！请回复后再点赞！");
            builder.Append("<br/><a href=\"" + Utils.getUrl("reply.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">看回帖</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
            builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "") + "\">" + ForumTitle + "</a>&gt;");
            builder.Append("<a href=\"" + Utils.getUrl("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "") + "\">主题帖</a>");
            builder.Append(Out.Tab("</div>", ""));
        }

    }

    /// <summary>
    /// 点赞排行榜 陈志基
    /// </summary>
    private void PraiseRanking(int forumid, int bid)
    {
        Master.Title = "点赞排行";

        string ForumTitle = new BCW.BLL.Forum().GetTitle(forumid);
        builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "") + "\">" + ForumTitle + "</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "") + "\">主题帖</a>&gt;");
        builder.Append("点赞");
        builder.Append(Out.Tab("</div>", ""));


        int GroupId = new BCW.BLL.Forum().GetGroupId(forumid);//判断是否主题，圈子论坛
        string ForumName = new BCW.BLL.Forum().GetTitle(forumid);  //得到论坛名字


        builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/forumstat.aspx?act=top&amp;forumid=" + forumid + "") + "\">" + ForumName + "点赞排行</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/forumstat.aspx?act=top") + "\">总论坛排行</a>");
        string praiseid = new BCW.BLL.Text().GetPraiseID(bid);

        if (praiseid == String.Empty)  //判断是否有人点赞{
        {

            builder.Append("<br/>还没有人点赞，赶快去评论然后点赞吧！");
            builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">看回复</a>");

        }
        else
        {
            builder.Append("<br/>本帖子的全部点赞人<br/>");
        }


        if (praiseid != String.Empty)  //判断是否有人点赞
        {
            string[] pid = praiseid.Split('#');
            for (int i = 0; i < pid.Length; i++)
            {
                int ID = int.Parse(pid[i]);
                builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + ID + "&amp;backurl=" + Utils.getPage(0) + "") + "\"><img src=\"" + new BCW.BLL.User().GetPhoto(ID) + "\" width=\"25\" height=\"33\" alt=\"load\"/>" + new BCW.BLL.User().GetUsName(ID) + "</a><br/>");
            }

        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    /// <summary>
    /// 回帖页面
    /// </summary>
    private void ReplyPage(string act, int forumid, int bid)
    {
        int copy = int.Parse(Utils.GetRequest("copy", "get", 1, @"^[0-1]$", "0"));
        int ff = int.Parse(Utils.GetRequest("ff", "get", 1, @"^[0-9]\d*$", "-1"));
        int dd = int.Parse(Utils.GetRequest("dd", "get", 1, @"^[0-9]\d*$", "0"));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string ForumSet = new BCW.BLL.User().GetForumSet(meid);
        int FsIsImg = BCW.User.Users.GetForumSet(ForumSet, 7);
        int reid = int.Parse(Utils.GetRequest("reid", "get", 1, @"^[0-9]\d*$", "0"));
        int page = int.Parse(Utils.GetRequest("page", "get", 1, @"^[0-9]\d*$", "1"));
        Master.Title = "回复话题";

        string ForumTitle = new BCW.BLL.Forum().GetTitle(forumid);
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "") + "\">" + ForumTitle + "</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "") + "\">主题帖</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;page=" + page + "&amp;backurl=" + Utils.getPage(0) + "") + "\">看回帖</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        if (reid > 0)
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));

            if (act == "reply2")
                builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?act=reply&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + reid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">点评</a>|引用回复:");
            else
                builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?act=reply2&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + reid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">引用</a>|点评回复:");

            builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?act=view&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + reid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + reid + "楼</a>");
            string neirong = new BCW.BLL.Reply().GetContent(bid, reid);  //增加评论的内容
            if (neirong.Length > 15)
            {
                neirong = neirong.Substring(0, 15);
                neirong += "...";
                builder.Append(":" + Out.SysUBB(neirong));
            }
            else
            {
                builder.Append(":" + Out.SysUBB(neirong));
            }
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("回复:<a href=\"" + Utils.getUrl("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + new BCW.BLL.Text().GetTitle(bid) + "</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        int Isre = 0;
        string reEmpt = "0|不提醒|1|帖子作者";
        if (reid > 0)
        {
            reEmpt = "0|不提醒|1|帖子作者|2|回帖作者|3|全部提醒";
            Isre = 2;
        }
        string reText = string.Empty;
        if (ff >= 0)
            reText += "[F]" + ff + "[/F]";

        if (dd > 0)
            reText += new BCW.BLL.Submit().GetContent(dd, meid);

        if (reid > 0 && act == "reply2")
        {
            BCW.Model.Reply m = new BCW.BLL.Reply().GetReplyMe(bid, reid);
            string rContent = Regex.Replace(m.Content, @"(\[IMG\])(.[^\[]*)(\[\/IMG\])", @"[图片]", RegexOptions.IgnoreCase);
            reText += "[" + m.UsName + "]说：“" + rContent + "”";
        }
        if (copy == 1)
            reText += new BCW.BLL.User().GetCopytemp(meid);

        //提醒费用
        long Tips = Convert.ToInt64(ub.GetSub("BbsReplyTips", xmlPath));
        if (Tips > 0)
            strText = "内容.300字内:/,回帖提醒(每位" + Tips + "" + ub.Get("SiteBz") + "):/,,,,,";
        else
            strText = "内容.300字内:/,回帖提醒:/,,,,,";

        strName = "Content,Remind,reid,forumid,bid,backurl,act";
        strType = "textarea,select,hidden,hidden,hidden,hidden,hidden";
        strValu = "" + reText + "'" + Isre + "'" + reid + "'" + forumid + "'" + bid + "'" + Utils.getPage(0) + "'save";
        strEmpt = "true," + reEmpt + ",false,false,false,false,false";

        strText += ",选择表情:/";
        strName += ",Face";
        strType += ",select";
        strValu += "'0";
        strEmpt += ",0|表情|1|微笑|2|哭泣|3|调皮|4|脸红|5|害羞|6|生气|7|偷笑|8|流汗|9|我倒|10|得意|11|眦牙|12|疑问|13|睡觉|14|好色|15|口水|16|敲头|17|可爱|18|猪头|19|胜利|20|玫瑰|21|吃药|22|白酒|23|可乐|24|囧囧|25|亲亲|26|抱抱";

        strIdea = "/";
        strOthe = "发表回复,reply.aspx,post,1,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", "<br />"));
        // builder.Append("插入:<a href=\"" + Utils.getUrl("function.aspx?act=admsub&amp;backurl=" + Utils.PostPage(1) + "") + "\">常用短语</a>.");
        builder.Append("插入:<a href=\"" + Utils.getUrl("function.aspx?act=face&amp;backurl=" + Utils.PostPage(1) + "") + "\">表情</a>");
        string VE = ConfigHelper.GetConfigString("VE");
        string SID = ConfigHelper.GetConfigString("SID");
        builder.Append("<br /><a href=\"" + Utils.getUrl("addThread20.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">文件回复</a> ");
        builder.Append("<br /><a href=\"" + Utils.getUrl("addfilegc.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">文件回复(低端机)</a> ");
        builder.Append("<br /><a href=\"" + Utils.getUrl("reply.aspx?act=" + act + "&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + reid + "&amp;page=" + page + "&amp;ff=" + ff + "&amp;dd=" + dd + "&amp;copy=1&amp;backurl=" + Utils.getPage(0) + "") + "\">[粘贴]</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;page=" + page + "&amp;backurl=" + Utils.getPage(0) + "") + "\">取消发表</a>");
        builder.Append(Out.Tab("</div>", ""));
    }


    /// <summary>
    /// 查看回复
    /// </summary>
    private void ViewPage(int forumid, int bid)
    {
        int meid = new BCW.User.Users().GetUsId();

        //论坛限制性
        BCW.Model.Forum Forummodel = new BCW.BLL.Forum().GetForum(forumid);
        BCW.User.Users.ShowForumLimit(meid, Forummodel.Gradelt, Forummodel.Visitlt, Forummodel.VisitId, Forummodel.IsPc);
        int FsIsImg = 0;
        if (meid > 0)
        {
            string ForumSet = new BCW.BLL.User().GetForumSet(meid);
            FsIsImg = BCW.User.Users.GetForumSet(ForumSet, 7);
        }
        int reid = int.Parse(Utils.GetRequest("reid", "get", 2, @"^[0-9]\d*$", "ID错误"));
        int page = int.Parse(Utils.GetRequest("page", "get", 1, @"^[0-9]\d*$", "1"));
        if (!new BCW.BLL.Reply().Exists(bid, reid))
        {
            Utils.Error("不存在的记录", "");
        }
        BCW.Model.Reply model = new BCW.BLL.Reply().GetReply(bid, reid);
        if (model.IsDel == 1)
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "查看回帖";
        builder.Append(Out.Tab("<div class=\"title\">", ""));

        if (model.IsTop == 1)
            builder.Append("[顶]");

        if (model.IsGood == 1)
            builder.Append("[精]");

        if (model.FileNum > 0)
            builder.Append("[附]");

        builder.Append("" + model.Floor + "楼:<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.UsName + "</a>/" + DT.FormatDate(model.AddTime, 0) + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        string rText = model.Content;
        if (FsIsImg == 1)
            rText = Utils.ClearImg(Out.SysUBB(rText));
        else
            rText = Out.SysUBB(rText);

        builder.Append("内容:" + rText);

        builder.Append(Out.Tab("</div>", ""));

        //图片直接显示
        if (model.FileNum > 0)
        {
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            int pSize = 3;
            string sWhere = "Types=1 and BID=" + bid + " and ReID=" + model.ID + "";
            IList<BCW.Model.Upfile> listUpfile = new BCW.BLL.Upfile().GetUpfiles(pSize, sWhere);
            if (listUpfile.Count > 0)
            {
                foreach (BCW.Model.Upfile nn in listUpfile)
                {
                    string upContent = nn.Content;
                    if (string.IsNullOrEmpty(upContent))
                        upContent = "无标题";

                    builder.Append("<a href=\"" + Utils.getUrl("/showpic.aspx?pic=" + nn.PrevFiles + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"><img src=\"" + nn.PrevFiles + "\" alt=\"load\"/></a><br />" + upContent + "<br />");
                }
            }
            builder.Append("<a href=\"" + Utils.getUrl("filelist.aspx?act=file&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + model.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">查看附件(共" + model.FileNum + "个)</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div>", "<br />"));
        bool IsEdit = false;
        bool IsDel = false;
        if (ub.GetSub("BbsReplyEdit", xmlPath) == "0")
        {
            if (model.UsID == meid)
                IsEdit = true;
        }
        if (ub.GetSub("BbsReplyDel", xmlPath) == "0")
        {
            if (model.UsID == meid)
                IsDel = true;
        }
        //builder.Append(model.UsName);
        // builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?act=forgood&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + model.ID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[赏]</a>");
        builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?act=forgood&amp;forumid=" + forumid + "&amp;usid=" + model.UsID + "&amp;usname=" + model.UsName + "&amp;ReplyFloor=" + model.Floor + "&amp;bid=" + bid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[赏]</a>|");
        // builder.Append(model.UsID+".."+model.UsName);
        if (IsEdit == true || new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_EditReply, meid, forumid))
            builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?act=edit&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + reid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[编]</a>");

        if (IsDel == true || new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_DelReply, meid, forumid))
            builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?act=del&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + reid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[删]</a>");

        if (model.IsGood == 0)
        {
            if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_GoodReply, meid, forumid))
                builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?act=good&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + reid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[精]</a>");
        }
        else
        {
            if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_DelGoodReply, meid, forumid))
                builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?act=delgood&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + reid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[解精]</a>");
        }

        if (model.IsTop == 0)
        {
            if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_TopReply, meid, forumid))
                builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?act=top&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + reid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[顶]</a><br />");
        }
        else
        {
            if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_DelTopReply, meid, forumid))
                builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?act=deltop&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + reid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[去顶]</a><br />");
        }
        builder.Append("<br />");
        builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?act=reply&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + reid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">点评回复</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?act=reply2&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + reid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">引用此回复</a>");
        builder.Append("<br /><a href=\"" + Utils.getUrl("reply.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;page=" + page + "&amp;backurl=" + Utils.getPage(0) + "") + "\">最新回复</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">返回主题贴</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;page=" + page + "&amp;backurl=" + Utils.getPage(0) + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("function.aspx?act=copyr&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + reid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">复制</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    /// <summary>
    /// 编辑回帖
    /// </summary>
    private void EditPage(int forumid, int bid)
    {
        if (new BCW.User.ForumInc().IsForum68(forumid) == true)
        {
            Utils.Error("本论坛不允许编辑回帖", "");
        }
        Master.Title = "编辑回帖";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int reid = int.Parse(Utils.GetRequest("reid", "all", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.Reply model = new BCW.BLL.Reply().GetReplyMe(bid, reid);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (ub.GetSub("BbsReplyEdit", xmlPath) == "0")
        {
            if (model.UsID != meid && !new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_EditReply, meid, forumid))
            {
                Utils.Error("你的权限不足", "");
            }
        }
        else
        {
            if (!new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_EditReply, meid, forumid))
            {
                Utils.Error("你的权限不足", "");
            }
        }
        string info = Utils.GetRequest("info", "post", 1, "", "");
        if (info == "ok")
        {
            //管理安全提示
            string[] p_pageArr = { "act", "reid", "forumid", "info", "bid", "Content", "backurl" };
            BCW.User.Role.AdminSafePage(meid, Utils.getPageUrl(), p_pageArr, "post");

            string Content = Utils.GetRequest("Content", "post", 2, @"[^\^]{1,300}", "请输入1-300字的回帖内容");
            new BCW.BLL.Reply().Update(bid, reid, Content);

            //记录日志
            string strLog = string.Empty;
            if (model.UsID != meid)
            {
                strLog = "[url=/bbs/uinfo.aspx?uid=" + model.UsID + "]" + model.UsName + "[/url]的[url=/bbs/reply.aspx?act=view&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + reid + "]" + reid + "楼回帖[/url]被[url=/bbs/uinfo.aspx?uid=" + meid + "]" + new BCW.BLL.User().GetUsName(meid) + "[/url]编辑!";
                new BCW.BLL.Guest().Add(0, model.UsID, model.UsName, "您的[url=/bbs/reply.aspx?act=view&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + reid + "]" + reid + "楼回帖[/url]被[url=/bbs/uinfo.aspx?uid=" + meid + "]" + new BCW.BLL.User().GetUsName(meid) + "[/url]编辑!");
            }
            else
            {
                strLog = "[url=/bbs/uinfo.aspx?uid=" + model.UsID + "]" + model.UsName + "[/url]编辑自己的[url=/bbs/reply.aspx?act=view&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + reid + "]" + reid + "楼回帖[/url]";
            }
            new BCW.BLL.Forumlog().Add(7, forumid, bid, reid, strLog);

            Utils.Success("编辑回帖", "编辑回帖成功，正在返回..", Utils.getUrl("reply.aspx?act=view&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + reid + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("编辑<a href=\"" + Utils.getUrl("reply.aspx?act=view&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + reid + "") + "\">" + reid + "楼</a>回帖：");
            builder.Append(Out.Tab("</div>", ""));
            strText = "内容.300字内:/,,,,,,";
            strName = "Content,reid,forumid,bid,backurl,act,info";
            strType = "textarea,hidden,hidden,hidden,hidden,hidden,hidden";
            strValu = "" + model.Content + "'" + reid + "'" + forumid + "'" + bid + "'" + Utils.getPage(0) + "'edit'ok";
            strEmpt = "true,false,false,false,false,false,false";
            strIdea = "/";
            strOthe = "确定编辑,reply.aspx,post,1,red";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?act=view&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + reid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">回帖</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">主题贴</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    /// <summary>
    /// 删除回帖
    /// 修改更新删除字段统计数
    /// 陈志基 20160816
    /// </summary>
    private void DelPage(int forumid, int bid)
    {
        if (new BCW.User.ForumInc().IsForum68(forumid) == true)
        {
            Utils.Error("本论坛不允许删除回帖", "");
        }
        Master.Title = "删除回帖";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int reid = int.Parse(Utils.GetRequest("reid", "all", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.Reply model = new BCW.BLL.Reply().GetReplyMe(bid, reid);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (ub.GetSub("BbsReplyDel", xmlPath) == "0")
        {
            if (model.UsID != meid && !new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_DelReply, meid, forumid))
            {
                Utils.Error("你的权限不足", "");
            }
        }
        else
        {
            if (!new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_DelReply, meid, forumid))
            {
                Utils.Error("你的权限不足", "");
            }
        }
        List<BCW.Model.Reply> list = new BCW.BLL.Reply().GetReplysWhere("Bid="+ bid + " AND UsID="+model.UsID+ " AND IsDel=0");
     //   builder.Append(" list.Count;" + list.Count + "<br/>");
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info != "")
        {
            //管理安全提示
            if (model.UsID != meid)
            {
                string[] p_pageArr = { "act", "reid", "forumid", "info", "bid", "Why", "dType", "backurl" };
                BCW.User.Role.AdminSafePage(meid, Utils.getPageUrl(), p_pageArr, "post");
            }
            int dType = int.Parse(Utils.GetRequest("dType", "post", 1, @"^[0-1]$", "0"));
            if (dType == 1 || info == "ok2")//删除多个
            {
                //触发器代替删除回复数
                //计算该会员在此帖的回复数
                //int Total = new BCW.BLL.Reply().GetCount2(model.UsID, bid);
                //new BCW.BLL.Reply().Delete2(bid, model.UsID);
                DataSet ds = new BCW.BLL.Reply().GetList("ID,AddTime,UsID,IsDel", "forumid=" + forumid + " and bid=" + bid + ""+ " and UsID="+ model.UsID+" ");  //更新统计表回帖
                {
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            if (int.Parse(ds.Tables[0].Rows[i]["IsDel"].ToString()) == 0)//如果回帖没有删除
                            {
                                new BCW.BLL.Forumstat().Update2(2, int.Parse(ds.Tables[0].Rows[i]["UsID"].ToString()), forumid, DateTime.Parse(ds.Tables[0].Rows[i]["AddTime"].ToString()));
                            }

                        }
                    }
                }
                new BCW.BLL.Reply().UpdateIsDel3(bid, model.UsID, 1);
                //更新回复数
                //new BCW.BLL.Text().UpdateReplyNum(bid, -Total);
            }
            else   //删除单个
            {
                new BCW.BLL.Reply().UpdateIsDel2(bid, reid, 1);
                new BCW.BLL.Forumstat().Update2(2, model.UsID, forumid, model.AddTime);//更新统计表发帖
                //new BCW.BLL.Reply().Delete(bid, reid);
                //更新回复数
                //new BCW.BLL.Text().UpdateReplyNum(bid, -1);
            }
            //记录日志
            string Why = Utils.GetRequest("Why", "post", 3, @"^[^\^]{1,20}$", "理由限20字内，可留空");
            string strLog = string.Empty;
            if (model.UsID != meid)
            {
                //积分操作
                int GroupId = new BCW.BLL.Forum().GetGroupId(forumid);
                if (GroupId == 0)
                {
                    new BCW.User.Cent().UpdateCent(BCW.User.Cent.enumRole.Cent_DelReply, model.UsID);
                }
                strLog = "[url=/bbs/uinfo.aspx?uid=" + model.UsID + "]" + model.UsName + "[/url]的" + reid + "楼回帖被[url=/bbs/uinfo.aspx?uid=" + meid + "]" + new BCW.BLL.User().GetUsName(meid) + "[/url]删除!";
                new BCW.BLL.Guest().Add(0, model.UsID, model.UsName, "您的" + reid + "楼回帖被[url=/bbs/uinfo.aspx?uid=" + meid + "]" + new BCW.BLL.User().GetUsName(meid) + "[/url]删除!");
            }
            else
            {
                //积分操作
                int GroupId = new BCW.BLL.Forum().GetGroupId(forumid);
                if (GroupId == 0)
                {
                    new BCW.User.Cent().UpdateCent(BCW.User.Cent.enumRole.Cent_MeDelReply, model.UsID);
                }
                strLog = "[url=/bbs/uinfo.aspx?uid=" + model.UsID + "]" + model.UsName + "[/url]删除自己的跟帖";
            }
            if (!string.IsNullOrEmpty(Why))
                strLog += "理由:" + Why + "";

            new BCW.BLL.Forumlog().Add(6, forumid, bid, reid, strLog);

            Utils.Success("删除回帖", "删除回帖成功，正在返回..", Utils.getUrl("reply.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + ""), "1");

        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除<a href=\"" + Utils.getUrl("reply.aspx?act=view&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + reid + "") + "\">" + reid + "楼</a>回帖吗");
            builder.Append(Out.Tab("</div>", ""));
            if (model.UsID != meid)
            {
                strText = "原因:,选项:,,,,,,";
                strName = "Why,dType,reid,forumid,bid,backurl,act,info";
                strType = "text,select,hidden,hidden,hidden,hidden,hidden,hidden";
                strValu = "'0'" + reid + "'" + forumid + "'" + bid + "'" + Utils.getPage(0) + "'del'ok";
                strEmpt = "true,0|仅此回复|1|TA本帖回复,false,false,false,false,false,false";
                strIdea = "/";
                strOthe = "确定删除,reply.aspx,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

                builder.Append(Out.Tab("<div>", ""));
                builder.Append(" <a href=\"" + Utils.getUrl("reply.aspx?act=view&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + reid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">取消</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?info=ok&amp;act=del&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + reid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">删除此回复</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?info=ok2&amp;act=del&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + reid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">删除我本贴回复</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?act=view&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + reid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
                builder.Append(Out.Tab("</div>", ""));
            }

        }
    }

    /// <summary>
    /// 加精回帖
    /// </summary>
    private void GoodPage(string act, int forumid, int bid)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int reid = int.Parse(Utils.GetRequest("reid", "all", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.Reply model = new BCW.BLL.Reply().GetReplyMe(bid, reid);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (meid == model.UsID)
        {
            Utils.Error("不能加精自己的回帖", "");
        }

        string sName = string.Empty;
        int IsGood = 0;
        if (act == "good")
        {
            sName = "加精";
            IsGood = 1;
            if (model.IsGood == 1)
            {
                Utils.Error("此回帖已加精", "");
            }
            if (!new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_GoodReply, meid, forumid))
            {
                Utils.Error("你的权限不足", "");
            }
        }
        else
        {
            sName = "解精";
            IsGood = 0;
            if (model.IsGood == 0)
            {
                Utils.Error("此回帖已解精", "");
            }
            if (!new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_DelGoodReply, meid, forumid))
            {
                Utils.Error("你的权限不足", "");
            }
        }
        Master.Title = "" + sName + "回帖";
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {
            //管理安全提示
            string[] p_pageArr = { "act", "reid", "forumid", "info", "bid", "backurl" };
            BCW.User.Role.AdminSafePage(meid, Utils.getPageUrl(), p_pageArr, "get");

            new BCW.BLL.Reply().UpdateIsGood(bid, reid, IsGood);
            //记录日志
            string Why = Utils.GetRequest("Why", "post", 3, @"^[^\^]{1,20}$", "理由限20字内，可留空");
            string strLog = "[url=/bbs/reply.aspx?act=view&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + reid + "]" + reid + "楼回帖[/url]被[url=/bbs/uinfo.aspx?uid=" + meid + "]" + new BCW.BLL.User().GetUsName(meid) + "[/url]" + sName + "!";
            new BCW.BLL.Forumlog().Add(1, forumid, bid, reid, "[url=/bbs/uinfo.aspx?uid=" + model.UsID + "]" + model.UsName + "[/url]的" + strLog);
            new BCW.BLL.Guest().Add(0, model.UsID, model.UsName, "您的" + strLog);
            //积分操作
            //if (IsGood == 1)
            //    new BCW.User.Cent().UpdateCent(BCW.User.Cent.enumRole.Cent_GoodReply, model.UsID);
            //else
            //    new BCW.User.Cent().UpdateCent(BCW.User.Cent.enumRole.Cent_DelGoodReply, model.UsID);

            Utils.Success("" + sName + "回帖", "" + sName + "回帖成功，正在返回..", Utils.getUrl("reply.aspx?act=view&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + reid + "&amp;backurl=" + Utils.getPage(0) + ""), "2");

        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定" + sName + "<a href=\"" + Utils.getUrl("reply.aspx?act=view&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + reid + "") + "\">" + reid + "楼</a>回帖吗");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?info=ok&amp;act=" + act + "&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + reid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定" + sName + "</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?act=view&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + reid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    /// <summary>
    /// 置顶回帖
    /// </summary>
    private void TopPage(string act, int forumid, int bid)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int reid = int.Parse(Utils.GetRequest("reid", "all", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.Reply model = new BCW.BLL.Reply().GetReplyMe(bid, reid);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }

        string sName = string.Empty;
        int IsTop = 0;
        if (act == "top")
        {
            sName = "置顶";
            IsTop = 1;
            if (model.IsTop == 1)
            {
                Utils.Error("此回帖已置顶", "");
            }
            if (!new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_TopReply, meid, forumid))
            {
                Utils.Error("你的权限不足", "");
            }
        }
        else
        {
            sName = "去顶";
            IsTop = 0;
            if (model.IsTop == 0)
            {
                Utils.Error("此回帖已去顶", "");
            }
            if (!new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_DelTopReply, meid, forumid))
            {
                Utils.Error("你的权限不足", "");
            }
        }
        Master.Title = "" + sName + "回帖";
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {
            //管理安全提示
            string[] p_pageArr = { "act", "reid", "forumid", "info", "bid", "backurl" };
            BCW.User.Role.AdminSafePage(meid, Utils.getPageUrl(), p_pageArr, "get");

            new BCW.BLL.Reply().UpdateIsTop(bid, reid, IsTop);
            //记录日志
            string Why = Utils.GetRequest("Why", "post", 3, @"^[\s\S]{1,20}$", "理由限20字内，可留空");
            string strLog = "[url=/bbs/reply.aspx?act=view&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + reid + "]" + reid + "楼回帖[/url]被[url=/bbs/uinfo.aspx?uid=" + meid + "]" + new BCW.BLL.User().GetUsName(meid) + "[/url]" + sName + "!";
            new BCW.BLL.Forumlog().Add(3, forumid, bid, reid, "[url=/bbs/uinfo.aspx?uid=" + model.UsID + "]" + model.UsName + "[/url]的" + strLog);
            new BCW.BLL.Guest().Add(0, model.UsID, model.UsName, "您的" + strLog);
            Utils.Success("" + sName + "回帖", "" + sName + "回帖成功，正在返回..", Utils.getUrl("reply.aspx?act=view&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + reid + "&amp;backurl=" + Utils.getPage(0) + ""), "1");

        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定" + sName + "<a href=\"" + Utils.getUrl("reply.aspx?act=view&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + reid + "") + "\">" + reid + "楼</a>回帖吗");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?info=ok&amp;act=" + act + "&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + reid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定" + sName + "</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?act=view&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + reid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">返回主题贴</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void RewardPage(int forumid, int bid)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int reid = int.Parse(Utils.GetRequest("reid", "all", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.Reply model = new BCW.BLL.Reply().GetReplyMe(bid, reid);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }

        BCW.Model.Text m = new BCW.BLL.Text().GetPriceModel(bid);
        if (m == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (m.UsID != meid)
        {
            Utils.Error("你的权限不足", "");
        }
        string bzText = string.Empty;
        if (m.BzType == 0)
            bzText = ub.Get("SiteBz");
        else
            bzText = ub.Get("SiteBz2");

        string info = Utils.GetRequest("info", "post", 1, "", "");
        if (info == "ok")
        {
            long cent = Int64.Parse(Utils.GetRequest("cent", "post", 4, @"^[1-9]\d*$", "悬赏数额填写错误"));
            if (cent > (m.Prices - m.Pricel))
            {
                Utils.Error("" + bzText + "剩余不足", "");

            }
            //每层楼不重复悬赏
            if (model.CentText != "")
            {
                Utils.Error("此楼层已悬赏", "");
            }
            if (model.UsID == meid)
            {
                Utils.Error("不能悬赏给自己", "");
            }

            if (m.BzType == 0)
            {
                new BCW.BLL.User().UpdateiGold(model.UsID, model.UsName, cent, "来自ID:" + meid + "悬赏");
            }
            else
            {
                new BCW.BLL.User().UpdateiMoney(model.UsID, model.UsName, cent, "来自ID:" + meid + "悬赏");
            }
            //更新已赏
            new BCW.BLL.Text().UpdatePricel(bid, cent);
            new BCW.BLL.Reply().UpdateCentText(bid, reid, "" + cent + "" + bzText + "");
            //发内线
            string mename = new BCW.BLL.User().GetUsName(meid);
            new BCW.BLL.Guest().Add(0, model.UsID, model.UsName, "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]悬赏" + cent + "" + bzText + "给您，[url=/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "]进入主题[/url]");


            //检测15天前的悬赏帖，如果没有悬赏完则自动清0并自动结帖
            BCW.Data.SqlHelper.ExecuteSql("update tb_Text set Pricel=Prices where Types=7 and AddTime<'" + DateTime.Now.AddDays(-15) + "'");
            //if ((m.Prices - m.Pricel - cent) == 0)
            //{
            //    //悬赏完币即结帖
            //    new BCW.BLL.Text().UpdateIsOver(bid, 1);
            //}
            Utils.Success("悬赏", "成功悬赏:" + cent + "" + bzText + "<br />剩余:" + (m.Prices - m.Pricel - cent) + "" + bzText + "", Utils.getPage("reply.aspx?forumid=" + forumid + "&amp;bid=" + bid + ""), "3");
        }
        else
        {
            Master.Title = "悬赏" + bzText;

            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("悬赏<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.UsName + "</a>的");
            builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?act=view&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + reid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + reid + "楼回帖</a>");

            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("悬赏:" + m.Prices + "/剩余:" + (m.Prices - m.Pricel) + "" + bzText + "");
            builder.Append(Out.Tab("</div>", ""));
            strText = "悬赏数额:/,,,,,,";
            strName = "cent,reid,forumid,bid,backurl,act,info";
            strType = "num,hidden,hidden,hidden,hidden,hidden,hidden";
            strValu = "'" + reid + "'" + forumid + "'" + bid + "'" + Utils.getPage(0) + "'reward'ok";
            strEmpt = "true,false,false,false,false,false,false";
            strIdea = "" + bzText + "''''''|/";
            strOthe = "确定悬赏,reply.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">回帖</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">主题贴</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    /// <summary>
    /// 悬赏列表
    /// </summary>
    private void RewardListPage(int forumid, int bid)
    {
        Master.Title = "悬赏名单";
        BCW.Model.Text m = new BCW.BLL.Text().GetPriceModel(bid);
        if (m != null)
        {
            string bzText = string.Empty;
            if (m.BzType == 0)
                bzText = ub.Get("SiteBz");
            else
                bzText = ub.Get("SiteBz2");

            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("悬赏:" + m.Prices + "/");
            builder.Append("剩余:" + (m.Prices - m.Pricel) + "" + bzText + "");
            builder.Append(Out.Tab("</div>", Out.LHr()));
        }

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        string strOrder = string.Empty;
        string[] pageValUrl = { "act", "forumid", "bid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        strWhere = "Bid=" + bid + " and CentText<>''";

        strOrder = "Istop Desc,AddTime Desc";

        // 开始读取列表
        IList<BCW.Model.Reply> listReply = new BCW.BLL.Reply().GetReplys(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listReply.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Reply n in listReply)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                    builder.Append(Out.Tab("<div>", "<br />"));

                builder.AppendFormat("{0}楼:", n.Floor);

                builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;" + n.UsName + "</a>获" + n.CentText + "");

                k++;
                builder.Append(Out.Tab("</div>", ""));

            }
            // 分页
            builder.Append(BasePage.ForumMultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Tab("", "<br />"));
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">回帖</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">主题贴</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    /// <summary>
    /// 得到某论坛版主所有ID
    /// </summary>
    private string GetAdminSubIDS(int forumid)
    {
        string SubAdmin = string.Empty;
        DataSet ds = new BCW.BLL.Role().GetList("UsID", "ForumID=" + forumid + " and (OverTime>='" + DateTime.Now + "' OR OverTime='1990-1-1 00:00:00') and Status=0");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                SubAdmin += "#" + ds.Tables[0].Rows[i]["UsID"].ToString();
            }
            SubAdmin += "#";
        }
        return SubAdmin;
    }
    private void ForGoodPage(int forumid, int bid)
    {
        //int ReplyFloor = 0;//判断是否打赏给回帖的
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        BCW.Model.Text model = new BCW.BLL.Text().GetText(bid);
        string usid = Utils.GetRequest("usid", "all", 0, "", "");
        string ReplyFloor = Utils.GetRequest("ReplyFloor", "all", 0, "", "");
        string usname = Utils.GetRequest("usname", "all", 0, "", "");

        BCW.Model.User model1 = new BCW.BLL.User().GetTimeLimit(meid);//读取第一次打赏的时间
        // DateTime now = model1.timelimit.AddSeconds(120);
        TimeSpan ts = DateTime.Now - model1.TimeLimit;//判断 系统时间和第一次选择打赏方式的时间
        //int paytype = new BCW.BLL.User().GetPayType(meid);//获取打赏方式
        if (ts.TotalSeconds > 300)  //超过五分钟，重置打赏方式
        {
            new BCW.BLL.User().UpdatePayType(meid, 0);//重置为可以选择基金\自身财富
        }
        if (model.IsOver == 1)
        {
            Utils.Error("此帖子已经结束", "");
        }
        if (model.IsLock == 1)
        {
            Utils.Error("此帖子已经被锁定", "");
        }
        if (model.IsTop == -1)
        {
            Utils.Error("此帖子已经固底", "");

        }
        if (model.UsID == meid && ReplyFloor == "")
        {
            Utils.Error("不能打赏给自己", "");
        }
        string info = Utils.GetRequest("info", "post", 1, "", "");
        if (info == "ok")
        {

            //支付安全提示
            string[] p_pageArr = { "act", "forumid", "bid", "cent", "Types", "Notes", "info", "backurl", "usid", "ReplyFloor", "usname" };
            BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr, "post", true);

            string SysID = ub.GetSub("FinanceSysID", "/Controls/finance.xml");//  值=  11    判断权限
            if (("#" + SysID + "#").Contains("#" + meid + "#"))
            {
                Utils.Error("你的权限不足", "");
            }
            //内部ID过户软禁
            string SysID2 = ub.GetSub("FinanceSysID2", "/Controls/finance.xml");//  值=  11
            SysID2 += "#" + ub.GetSub("XiaoHao", "/Controls/xiaohao.xml");  //  值=  11#+一大串数字和#  （32012#13251#）

            if (("#" + SysID2 + "#").Contains("#" + meid + "#"))    //判断过户权限 SYSID2里面如果没有用户ID 就没有权限
            {
                Utils.Error("过户权限不足，请联系客服！", "");
            }
            int paybyfund = 0;
            long cent = Int64.Parse(Utils.GetRequest("cent", "post", 4, @"^[1-9]\d*$", "打赏数额填写错误"));
            int Types = int.Parse(Utils.GetRequest("Types", "post", 2, @"^[0-4]\d*$", "打赏币种选择错误"));
            string Notes = Utils.GetRequest("Notes", "post", 2, @"^[\s\S]{1,50}$", "附言不能为空，限1-50字");
            string GetiCentID = ub.GetSub("BbsGetiCentID", xmlPath);
            string iGetiCentID = ub.GetSub("BbsGetiCentID" + forumid + "", xmlPath);
            string mename = new BCW.BLL.User().GetUsName(meid);
            string bzText = string.Empty;
            if (Types == 0 && new BCW.BLL.User().GetPayType(meid) == 0)//首次选择打赏方式
            {
                Utils.Error("请选择币源", "");
            }

            else if (Types == 1)  //选择支出为自身财富
            {
                paybyfund = 0;  //设置为自身财富支出              
                //计算今天总打赏币
                long TodayCents = new BCW.BLL.Textcent().GetCents(0, 0, meid);
                if (TodayCents + cent > 5000000)
                {
                    Utils.Error("每人每天只能打赏5000000" + ub.Get("SiteBz") + "，请明天再来吧", "");
                }
                long gold = new BCW.BLL.User().GetGold(meid);
                if (gold < cent)
                {
                    Utils.Error("你的" + ub.Get("SiteBz") + "不足", "");
                }
                if (new BCW.BLL.User().GetPayType(meid) == 2)
                {
                    new BCW.BLL.User().UpdateTimeLimit(meid, DateTime.Now);
                }
                new BCW.BLL.User().UpdatePayType(meid, 1);
                if (ts.TotalSeconds > 300)
                {
                    new BCW.BLL.User().UpdateTimeLimit(meid, DateTime.Now);
                }
                if (ReplyFloor == "")//打赏贴吧的帖主
                {
                    new BCW.BLL.User().UpdateiGold(meid, mename, -cent, "打赏给ID:" + model.UsID + "");
                    new BCW.BLL.User().UpdateiGold(model.UsID, model.UsName, cent, "来自ID:" + meid + "打赏");
                }
                else   //打赏贴吧的回帖贴主
                {
                    new BCW.BLL.User().UpdateiGold(meid, mename, -cent, "打赏给ID:" + Convert.ToInt32(usid) + "");
                    new BCW.BLL.User().UpdateiGold(Convert.ToInt32(usid), usname, cent, "来自ID:" + meid + "打赏");
                }
                bzText = ub.Get("SiteBz");

            }
            else if (Types == 2) //选择支出为本版基金，在本版的基金支出明细中同时显示这个附言
            {

                paybyfund = 1;
                //
                BCW.Model.Forum ForumModel = new BCW.BLL.Forum().GetForumBasic(forumid);//论坛基金数
                if (new BCW.BLL.Forum().GetGroupId(forumid) == 0) //判断主题圈子
                {
                    if (!("#" + GetiCentID + "#").Contains("#" + meid + "#") && !("#" + iGetiCentID + "#").Contains("#" + meid + "#"))
                    {
                        Utils.Error("你没有取款的权限!", "");
                    }

                    //进一步检测版主权限
                    if (!("#" + GetiCentID + "#").Contains("#" + meid + "#"))
                    {
                        if (!("#" + GetAdminSubIDS(forumid) + "#").Contains("#" + meid + "#"))
                        {
                            Utils.Error("你没有取款的权限!", "");
                        }
                    }
                    if (ForumModel.iCent < cent)
                    {

                        Utils.Error("论坛的" + ub.Get("SiteBz") + "不足", "");
                    }
                }
                else//圈子
                {
                    string XmlsetID = ub.GetSub(ForumModel.GroupId + "setID", "/Controls/group.xml");//获取圈主设置的取款ID
                    //  builder.Append(XmlsetID);
                    if (!new BCW.BLL.Group().ExistsUsID(meid))//不是圈主
                    {
                        int temp = 0;
                        if (XmlsetID.Contains("#"))
                        {
                            string[] sid = XmlsetID.Split('#');
                            for (int i = 0; i < sid.Length; i++)
                            {
                                if (sid[i] == meid.ToString())
                                {
                                    temp = 1;
                                }
                            }
                        }
                        if (XmlsetID == meid.ToString())
                        {
                            temp = 1;
                        }
                        if (temp == 0)
                        {
                            Utils.Error("你没有取款的权限!", "");
                        }
                    }
                    BCW.Model.Group model4 = new BCW.BLL.Group().GetGroup(ForumModel.GroupId);
                    if (model4.iCent < cent)
                    {
                        Utils.Error("" + ub.GetSub("GroupName", xmlPath) + "基金不足" + cent + "" + ub.Get("SiteBz") + "", "");
                    }
                }
                long TodayCents;
                if (ReplyFloor == "")//打赏贴吧的帖主
                {
                    TodayCents = new BCW.BLL.Textcent().GetForrmCents(0, 0, model.UsID);//得到帖主这天的打赏币数
                }
                else //打赏给帖子里面的回帖的人
                {
                    TodayCents = new BCW.BLL.Textcent().GetForrmCents(0, 0, Convert.ToInt32(usid));//得到回帖的人这天的打赏数
                    // Utils.Error("每人每天只能打赏50000" + ub.Get("SiteBz") + "，请明天再来吧", "");
                }
                // TodayCents = new BCW.BLL.Textcent().GetForrmCents(0, 0, Convert.ToInt32(usid));//得到回帖的人这天的打赏数
                if (TodayCents + cent > 150000)
                {
                    Utils.Error("每人每天只能打赏15万" + ub.Get("SiteBz") + "，请明天再来吧", "");
                }
                if (new BCW.BLL.User().GetPayType(meid) == 1)
                {
                    new BCW.BLL.User().UpdateTimeLimit(meid, DateTime.Now);
                }
                new BCW.BLL.User().UpdatePayType(meid, 2);
                if (ts.TotalSeconds > 300)
                {
                    new BCW.BLL.User().UpdateTimeLimit(meid, DateTime.Now);
                }
                if (ReplyFloor == "")//打赏贴吧的帖主
                {
                    // new BCW.BLL.Forum().UpdateiCent(forumid, meid, mename, -cent, model.UsID, Notes);//减少论坛基金
                    if (new BCW.BLL.Forum().GetGroupId(forumid) == 0) //判断主题圈子
                    {
                        new BCW.BLL.Forum().UpdateiCent(forumid, meid, mename, -cent, model.UsID, Notes + "打赏给:[url=/bbs/uinfo.aspx?uid=" + model.UsID + "]" + model.UsName + "(" + model.UsID + ")[/url]" + "的贴子（帖子:" + model.Title + "）");//减少论坛基金
                    }
                    else
                    {
                        BCW.Model.Grouplog addmodel = new BCW.Model.Grouplog();
                        addmodel.GroupId = ForumModel.GroupId;
                        addmodel.Types = 2;
                        addmodel.UsID = meid;
                        addmodel.UsName = new BCW.BLL.User().GetUsName(meid);
                        addmodel.Content = "打赏给:[url=/bbs/uinfo.aspx?uid=" + model.UsID + "]" + model.UsName + "(" + model.UsID + ")[/url]" + "的贴子（帖子:" + model.Title + "）";
                        addmodel.PayCent = cent;
                        addmodel.AddTime = DateTime.Now;
                        new BCW.BLL.Grouplog().Add(addmodel);
                        new BCW.BLL.Group().UpdateiCent(ForumModel.GroupId, -cent);
                    }
                    new BCW.BLL.User().UpdateiGold(model.UsID, model.UsName, cent, "来自ID:" + meid + "打赏");
                }
                else   //打赏贴吧的回帖贴主
                {
                    // new BCW.BLL.Forum().UpdateiCent(forumid, meid, mename, -cent, Convert.ToInt32(usid), Notes);//减少论坛基金
                    if (new BCW.BLL.Forum().GetGroupId(forumid) == 0) //判断主题圈子
                    {
                        new BCW.BLL.Forum().UpdateiCent(forumid, meid, mename, -cent, Convert.ToInt32(usid), Notes + "打赏给:[url=/bbs/uinfo.aspx?uid=" + Convert.ToInt32(usid) + "]" + usname + "(" + Convert.ToInt32(usid) + ")[/url]" + ReplyFloor + "楼回贴（帖子:" + model.Title + "）");//减少论坛基金
                    }
                    else
                    {
                        BCW.Model.Grouplog addmodel = new BCW.Model.Grouplog();
                        addmodel.GroupId = ForumModel.GroupId;
                        addmodel.Types = 2;
                        addmodel.UsID = meid;
                        addmodel.UsName = new BCW.BLL.User().GetUsName(meid);
                        addmodel.Content = "打赏给:[url=/bbs/uinfo.aspx?uid=" + Convert.ToInt32(usid) + "]" + usname + "(" + Convert.ToInt32(usid) + ")[/url]" + ReplyFloor + "楼回贴（帖子:" + model.Title + "）";
                        addmodel.PayCent = cent;
                        addmodel.AddTime = DateTime.Now;
                        new BCW.BLL.Grouplog().Add(addmodel);
                        new BCW.BLL.Group().UpdateiCent(ForumModel.GroupId, -cent);
                    }
                    new BCW.BLL.User().UpdateiGold(Convert.ToInt32(usid), usname, cent, "来自ID:" + meid + "打赏");
                }

                bzText = ub.Get("SiteBz");
                //
            }
            if (Types == 0) //默认选择打赏方式之后的选择
            {
                if (new BCW.BLL.User().GetPayType(meid) == 1)
                {
                    paybyfund = 0;  //设置为自身财富支出              
                    //计算今天总打赏币
                    long TodayCents = new BCW.BLL.Textcent().GetCents(0, 0, meid);
                    if (TodayCents + cent > 5000000)
                    {
                        Utils.Error("每人每天只能打赏5000000" + ub.Get("SiteBz") + "，请明天再来吧", "");
                    }
                    long gold = new BCW.BLL.User().GetGold(meid);
                    if (gold < cent)
                    {
                        Utils.Error("你的" + ub.Get("SiteBz") + "不足", "");
                    }
                    if (new BCW.BLL.User().GetPayType(meid) == 2)
                    {
                        new BCW.BLL.User().UpdateTimeLimit(meid, DateTime.Now);
                    }
                    new BCW.BLL.User().UpdatePayType(meid, 1);
                    if (ts.TotalSeconds > 300)
                    {
                        new BCW.BLL.User().UpdateTimeLimit(meid, DateTime.Now);
                    }
                    if (ReplyFloor == "")//打赏贴吧的帖主
                    {
                        new BCW.BLL.User().UpdateiGold(meid, mename, -cent, "打赏给ID:" + model.UsID + "");
                        new BCW.BLL.User().UpdateiGold(model.UsID, model.UsName, cent, "来自ID:" + meid + "打赏");
                    }
                    else   //打赏贴吧的回帖贴主
                    {
                        new BCW.BLL.User().UpdateiGold(meid, mename, -cent, "打赏给ID:" + Convert.ToInt32(usid) + "");
                        new BCW.BLL.User().UpdateiGold(Convert.ToInt32(usid), usname, cent, "来自ID:" + meid + "打赏");
                    }
                    bzText = ub.Get("SiteBz");
                }
                else if (new BCW.BLL.User().GetPayType(meid) == 2)
                {

                    paybyfund = 1;
                    //
                    BCW.Model.Forum ForumModel = new BCW.BLL.Forum().GetForumBasic(forumid);//论坛基金数
                    if (new BCW.BLL.Forum().GetGroupId(forumid) == 0) //判断主题圈子
                    {
                        if (!("#" + GetiCentID + "#").Contains("#" + meid + "#") && !("#" + iGetiCentID + "#").Contains("#" + meid + "#"))
                        {
                            Utils.Error("你没有取款的权限!", "");
                        }

                        //进一步检测版主权限
                        if (!("#" + GetiCentID + "#").Contains("#" + meid + "#"))
                        {
                            if (!("#" + GetAdminSubIDS(forumid) + "#").Contains("#" + meid + "#"))
                            {
                                Utils.Error("你没有取款的权限!", "");
                            }
                        }
                        if (ForumModel.iCent < cent)
                        {

                            Utils.Error("论坛的" + ub.Get("SiteBz") + "不足", "");
                        }
                    }
                    else//圈子
                    {
                        string XmlsetID = ub.GetSub(ForumModel.GroupId + "setID", "/Controls/group.xml");//获取圈主设置的取款ID
                        //  builder.Append(XmlsetID);
                        if (!new BCW.BLL.Group().ExistsUsID(meid))//不是圈主
                        {
                            int temp = 0;
                            if (XmlsetID.Contains("#"))
                            {
                                string[] sid = XmlsetID.Split('#');
                                for (int i = 0; i < sid.Length; i++)
                                {
                                    if (sid[i] == meid.ToString())
                                    {
                                        temp = 1;
                                    }
                                }
                            }
                            if (XmlsetID == meid.ToString())
                            {
                                temp = 1;
                            }
                            if (temp == 0)
                            {
                                Utils.Error("你没有取款的权限!", "");
                            }
                        }
                        BCW.Model.Group model4 = new BCW.BLL.Group().GetGroup(ForumModel.GroupId);
                        if (model4.iCent < cent)
                        {
                            Utils.Error("" + ub.GetSub("GroupName", xmlPath) + "基金不足" + cent + "" + ub.Get("SiteBz") + "", "");
                        }
                    }
                    long TodayCents;
                    if (ReplyFloor == "")//打赏贴吧的帖主
                    {
                        TodayCents = new BCW.BLL.Textcent().GetForrmCents(0, 0, model.UsID);//得到帖主这天的打赏币数
                    }
                    else //打赏给帖子里面的回帖的人
                    {
                        TodayCents = new BCW.BLL.Textcent().GetForrmCents(0, 0, Convert.ToInt32(usid));//得到回帖的人这天的打赏数
                        // Utils.Error("每人每天只能打赏50000" + ub.Get("SiteBz") + "，请明天再来吧", "");
                    }
                    // TodayCents = new BCW.BLL.Textcent().GetForrmCents(0, 0, Convert.ToInt32(usid));//得到回帖的人这天的打赏数
                    if (TodayCents + cent > 150000)
                    {
                        Utils.Error("每人每天只能打赏15万" + ub.Get("SiteBz") + "，请明天再来吧", "");
                    }

                    if (new BCW.BLL.User().GetPayType(meid) == 1)
                    {
                        new BCW.BLL.User().UpdateTimeLimit(meid, DateTime.Now);
                    }
                    new BCW.BLL.User().UpdatePayType(meid, 2);
                    if (ts.TotalSeconds > 300)
                    {
                        new BCW.BLL.User().UpdateTimeLimit(meid, DateTime.Now);
                    }
                    if (ReplyFloor == "")//打赏贴吧的帖主
                    {
                        // new BCW.BLL.Forum().UpdateiCent(forumid, meid, mename, -cent, model.UsID, Notes);//减少论坛基金
                        if (new BCW.BLL.Forum().GetGroupId(forumid) == 0) //判断主题圈子
                        {
                            new BCW.BLL.Forum().UpdateiCent(forumid, meid, mename, -cent, model.UsID, Notes + "打赏给:[url=/bbs/uinfo.aspx?uid=" + model.UsID + "]" + model.UsName + "(" + model.UsID + ")[/url]" + "的贴子（帖子:" + model.Title + "）");//减少论坛基金
                        }
                        else
                        {
                            BCW.Model.Grouplog addmodel = new BCW.Model.Grouplog();
                            addmodel.GroupId = ForumModel.GroupId;
                            addmodel.Types = 2;
                            addmodel.UsID = meid;
                            addmodel.UsName = new BCW.BLL.User().GetUsName(meid);
                            addmodel.Content = "打赏给:[url=/bbs/uinfo.aspx?uid=" + model.UsID + "]" + model.UsName + "(" + model.UsID + ")[/url]" + "的贴子（帖子:" + model.Title + "）";
                            addmodel.PayCent = cent;
                            addmodel.AddTime = DateTime.Now;
                            new BCW.BLL.Grouplog().Add(addmodel);
                            new BCW.BLL.Group().UpdateiCent(ForumModel.GroupId, -cent);
                        }
                        new BCW.BLL.User().UpdateiGold(model.UsID, model.UsName, cent, "来自ID:" + meid + "打赏");
                    }
                    else   //打赏贴吧的回帖贴主
                    {
                        // new BCW.BLL.Forum().UpdateiCent(forumid, meid, mename, -cent, Convert.ToInt32(usid), Notes);//减少论坛基金
                        if (new BCW.BLL.Forum().GetGroupId(forumid) == 0) //判断主题圈子
                        {
                            new BCW.BLL.Forum().UpdateiCent(forumid, meid, mename, -cent, Convert.ToInt32(usid), Notes + "打赏给:[url=/bbs/uinfo.aspx?uid=" + Convert.ToInt32(usid) + "]" + usname + "(" + Convert.ToInt32(usid) + ")[/url]" + ReplyFloor + "楼回贴（帖子:" + model.Title + "）");//减少论坛基金
                        }
                        else
                        {
                            BCW.Model.Grouplog addmodel = new BCW.Model.Grouplog();
                            addmodel.GroupId = ForumModel.GroupId;
                            addmodel.Types = 2;
                            addmodel.UsID = meid;
                            addmodel.UsName = new BCW.BLL.User().GetUsName(meid);
                            addmodel.Content = "打赏给:[url=/bbs/uinfo.aspx?uid=" + Convert.ToInt32(usid) + "]" + usname + "(" + Convert.ToInt32(usid) + ")[/url]" + ReplyFloor + "楼回贴（帖子:" + model.Title + "）";
                            addmodel.PayCent = cent;
                            addmodel.AddTime = DateTime.Now;
                            new BCW.BLL.Grouplog().Add(addmodel);
                            new BCW.BLL.Group().UpdateiCent(ForumModel.GroupId, -cent);
                        }
                        new BCW.BLL.User().UpdateiGold(Convert.ToInt32(usid), usname, cent, "来自ID:" + meid + "打赏");
                    }

                    bzText = ub.Get("SiteBz");
                }
            }
            //写入打赏表
            //增加回帖打赏字段ReplyFloor，需要判断是打赏给贴吧发帖的人，还是在帖子里回帖的人
            BCW.Model.Textcent n = new BCW.Model.Textcent();
            n.BID = bid;
            n.PayByFund = paybyfund;
            n.UsID = meid;
            n.UsName = mename;
            if (ReplyFloor == "")//打赏贴吧的帖主
            {
                n.ToID = model.UsID;
                n.ReplyFloor = 0;
                // Notes = Notes + "(打赏给发帖帖主ID:" + model.UsID + "用户名:" + model.UsName + ")";

            }
            else  //打赏给帖子里面的回帖的人
            {
                n.ToID = Convert.ToInt32(usid);
                n.ReplyFloor = Convert.ToInt32(ReplyFloor);
                //   Notes = Notes + "(打赏给" + ReplyFloor + "楼回帖用户ID:" + Convert.ToInt32(usid) + "用户名:" + usname + ")";
            }
            n.Cents = cent;
            n.BzType = 0;
            n.Notes = Notes;
            n.AddTime = DateTime.Now;
            new BCW.BLL.Textcent().Add(n);
            //发内线
            if (ReplyFloor == "")//打赏贴吧的帖主
            {
                new BCW.BLL.Guest().Add(0, model.UsID, model.UsName, "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]打赏" + cent + "" + bzText + "给您，[url=/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "]进入主题[/url]");
            }
            else//打赏给帖子里面的回帖的人
            {
                new BCW.BLL.Guest().Add(0, Convert.ToInt32(usid), usname, "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]打赏" + cent + "" + bzText + "给您，[url=/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "]进入主题[/url]");
            }
            Utils.Success("打赏", "打赏" + cent + "" + bzText + "成功！", Utils.getUrl("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
            //   builder.Append(Types);
        }
        else
        {
            int paytype = new BCW.BLL.User().GetPayType(meid);
            // builder.Append(paytype);
            Master.Title = "打赏币币";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            if (usid == "" || usid.Equals("null"))//打赏给发帖子的人
            {
                builder.Append("打赏给帖主:<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.UsName + "(" + model.UsID + ")</a>");
            }
            else  //打赏给回帖的人
            {
                if (Convert.ToInt32(usid) == meid)//判断是否打赏给自己
                {
                    Utils.Error("不能打赏给自己", "");
                }
                else
                {
                    builder.Append("打赏给:<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + ReplyFloor + "楼—" + usid + "(" + usname + ")</a>");
                }
            }
            builder.Append(Out.Tab("</div>", ""));
            //
            //builder.Append("<br/>" + model1.TimeLimit.AddSeconds(300) + "<br/>");
            //builder.Append(ts.TotalSeconds + "<br/>");
            //builder.Append(DateTime.Now);
            //
            strText = ub.Get("SiteBz") + "数额:/,,附言(50字内):/,,,,,,,,";
            strName = "cent,Types,Notes,forumid,bid,backurl,act,info,ReplyFloor,usid,usname";
            strType = "num,select,text,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden";
            strValu = "'0'好帖！'" + forumid + "'" + bid + "'" + Utils.getPage(0) + "'forgood'ok'" + ReplyFloor + "'" + usid + "'" + usname;

            if (paytype == 0)
            {
                strEmpt = "true,0|选择币源|1|自身财富|2|本版基金,true,false,false,false,false,false,false,false,false";

            }
            if (paytype == 1)
            {
                strEmpt = "true,0|自身财富|2|本版基金,true,false,false,false,false,false,false,false,false";

            }
            if (paytype == 2)
            {
                strEmpt = "true,0|本版基金|1|自身财富,true,false,false,false,false,false,false,false,false";

            }
            strIdea = "/";
            strOthe = "确定打赏,reply.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getPage("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "") + "\">上级</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">主题贴</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }
    /// <summary>
    /// 打赏列表
    /// </summary>
    private void ForGoodListPage(int forumid, int bid)
    {
        Master.Title = "打赏名单";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("查看打赏名单");
        builder.Append(Out.Tab("</div>", Out.LHr()));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        string strOrder = string.Empty;
        string[] pageValUrl = { "act", "forumid", "bid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        strWhere = "Bid=" + bid + "";

        // 开始读取列表
        IList<BCW.Model.Textcent> listTextcent = new BCW.BLL.Textcent().GetTextcents(pageIndex, pageSize, strWhere, out recordCount);
        if (listTextcent.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Textcent n in listTextcent)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                    builder.Append(Out.Tab("<div>", "<br />"));

                string bzText = string.Empty;
                if (n.BzType == 0)
                    bzText = ub.Get("SiteBz");
                else
                    bzText = ub.Get("SiteBz2");

                builder.AppendFormat("{0}.", (pageIndex - 1) * pageSize + k);
                builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + n.UsName + "</a>打赏了" + n.Cents + "" + bzText + "");
                builder.Append("附言:" + n.Notes);
                BCW.Model.Forum ForumModel = new BCW.BLL.Forum().GetForumBasic(forumid);//论坛基金数
                //new BCW .BLL.Group().GetID
                if (n.ReplyFloor != 0)//判断是发帖还是回帖
                {
                    if (n.PayByFund == 1)
                    {
                        if (ForumModel.GroupId == 0)
                        {
                            builder.Append("(<a href=\"" + Utils.getUrl("manwork.aspx?act=fund&amp;forumid=" + forumid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + ForumModel.Title + "基金</a>");
                        }
                        else
                        {
                            builder.Append("(<a href=\"" + Utils.getUrl("group.aspx?act=fundlist&amp;id=" + ForumModel.GroupId + "&amp;ptype=2&amp;backurl=" + Utils.getPage(0) + "") + "\">" + ForumModel.Title + "基金</a>");
                        }
                    }
                    else
                    {
                        builder.Append("(<a href=\"" + Utils.getUrl("finance.aspx?act=conlist&amp;backurl=" + Utils.getPage(0) + "") + "\">财产</a>");
                    }
                    string name = new BCW.BLL.User().GetUsName(n.ToID);
                    //  builder.Append("<a href=\"" + Utils.getUrl("finance.aspx?act=conlist&amp;backurl=" + Utils.getPage(0) + "") + "\">消费记录</a>");
                    builder.Append("打赏给" + n.ReplyFloor + "楼回帖用户<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.ToID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + name + "(" + n.ToID + ")" + "</a>)");
                }
                else
                {
                    if (n.PayByFund == 1)
                    {
                        if (ForumModel.GroupId == 0)
                        {
                            builder.Append("(<a href=\"" + Utils.getUrl("manwork.aspx?act=fund&amp;forumid=" + forumid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + ForumModel.Title + "基金</a>");
                        }
                        else
                        {
                            builder.Append("(<a href=\"" + Utils.getUrl("group.aspx?act=fundlist&amp;id=" + ForumModel.GroupId + "&amp;ptype=2&amp;backurl=" + Utils.getPage(0) + "") + "\">" + ForumModel.Title + "基金</a>");
                        }
                    }
                    else
                    {
                        builder.Append("(<a href=\"" + Utils.getUrl("finance.aspx?act=conlist&amp;backurl=" + Utils.getPage(0) + "") + "\">财产</a>");
                    }
                    string name = new BCW.BLL.User().GetUsName(n.ToID);
                    //  builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?act=fund&amp;forumid=" + forumid + "") + "\">基金</a>");
                    builder.Append("打赏给发帖用户<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.ToID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + name + "(" + n.ToID + ")" + "</a>)");
                }
                builder.Append("[" + DT.FormatDate(n.AddTime, 1) + "]");
                // builder.Append("附言:" + n.Notes + "[" + DT.FormatDate(n.AddTime, 1) + "]");

                k++;
                builder.Append(Out.Tab("</div>", ""));

            }
            // 分页
            builder.Append(BasePage.ForumMultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Tab("", "<br />"));
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?act=forgoodtop&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">打赏排行榜</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">主题贴</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    /// <summary>
    /// 打赏排行
    /// </summary>
    private void ForGoodTopPage(int forumid, int bid)
    {
        Master.Title = "打赏排行榜";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("打赏排行榜");
        builder.Append(Out.Tab("</div>", "<br />"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-3]$", "0"));
        int showtype = int.Parse(Utils.GetRequest("showtype", "get", 1, @"^[0-1]$", "0"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));

        if (showtype == 0)
            builder.Append("" + ub.Get("SiteBz") + "榜|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?act=forgoodtop&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;ptype=" + ptype + "&amp;showtype=0&amp;backurl=" + Utils.getPage(0) + "") + "\">" + ub.Get("SiteBz") + "榜</a>|");

        if (showtype == 1)
            builder.Append("" + ub.Get("SiteBz2") + "榜<br />");
        else
            builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?act=forgoodtop&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;ptype=" + ptype + "&amp;showtype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">" + ub.Get("SiteBz2") + "榜</a><br />");

        if (ptype == 0)
            builder.Append("打赏榜|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?act=forgoodtop&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;ptype=0&amp;showtype=" + showtype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">打赏榜</a>|");

        if (ptype == 1)
            builder.Append("打赏次数榜|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?act=forgoodtop&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;ptype=1&amp;showtype=" + showtype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">打赏次数榜</a>|");

        if (ptype == 2)
            builder.Append("受赏榜|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?act=forgoodtop&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;ptype=2&amp;showtype=" + showtype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">受赏榜</a>|");

        if (ptype == 3)
            builder.Append("受赏次数榜");
        else
            builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?act=forgoodtop&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;ptype=3&amp;showtype=" + showtype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">受赏次数榜</a>");

        builder.Append(Out.Tab("</div>", Out.LHr()));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        string strOrder = string.Empty;
        string[] pageValUrl = { "act", "ptype", "showtype", "forumid", "bid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        strWhere = "BzType=" + showtype + "";

        string bzText = string.Empty;
        if (showtype == 0)
            bzText = ub.Get("SiteBz");
        else
            bzText = ub.Get("SiteBz2");

        if (ptype == 1 || ptype == 3)
            bzText = "次";

        // 开始读取列表
        IList<BCW.Model.Textcent> listTextcent = new BCW.BLL.Textcent().GetTextcentsTop(ptype, pageIndex, pageSize, strWhere, out recordCount);
        if (listTextcent.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Textcent n in listTextcent)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                    builder.Append(Out.Tab("<div>", "<br />"));

                builder.AppendFormat("{0}.", (pageIndex - 1) * pageSize + k);
                builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + new BCW.BLL.User().GetUsName(n.UsID) + "</a>共打赏" + n.Cents + "" + bzText + "");

                k++;
                builder.Append(Out.Tab("</div>", ""));

            }
            // 分页
            builder.Append(BasePage.ForumMultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Tab("", "<br />"));
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }


    /// <summary>
    /// 保存回复
    /// </summary>
    private void SavePage(int forumid, int bid)
    {
        //是否刷屏
        string appName = "LIGHT_REPLY_" + Utils.GetTopDomain() + "";
        int Expir = Convert.ToInt32(ub.GetSub("BbsReplyExpir", xmlPath));
        BCW.User.Users.IsFresh(appName, Expir);

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        BCW.User.Users.ShowVerifyRole("b", meid);//非验证会员提示
        new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Reply, meid);//会员本身权限
        new BCW.User.FLimits().CheckUserFLimit(BCW.User.FLimits.enumRole.Role_Reply, meid, forumid);//版块内权限
        //论坛限制性
        BCW.Model.Forum Forummodel = new BCW.BLL.Forum().GetForum(forumid);
        //圈子限制性
        BCW.Model.Group modelgr = null;
        if (Forummodel.GroupId > 0)
        {
            modelgr = new BCW.BLL.Group().GetGroupMe(Forummodel.GroupId);
            if (modelgr == null)
            {
                Utils.Error("不存在的" + ub.GetSub("GroupName", "/Controls/group.xml") + "", "");
            }
            else if (DT.FormatDate(modelgr.ExTime, 0) != "1990-01-01 00:00:00" && modelgr.ExTime < DateTime.Now)
            {
                Utils.Error("" + ub.GetSub("GroupName", "/Controls/group.xml") + "已过期", "");
            }
            if (modelgr.ForumStatus == 2)
            {
                Utils.Error("" + ub.GetSub("GroupName", "/Controls/group.xml") + "论坛已关闭", "");
            }
            if (modelgr.ForumStatus == 1)
            {
                if (meid == 0)
                    Utils.Login();

                string GroupId = new BCW.BLL.User().GetGroupId(meid);
                if (GroupId.IndexOf("#" + Forummodel.GroupId + "#") == -1 && IsCTID(meid) == false)
                {
                    Utils.Error("非成员不能访问" + ub.GetSub("GroupName", "/Controls/group.xml") + "论坛！<br /><a href=\"" + Utils.getUrl("/bbs/group.aspx?act=addin&amp;id=" + Forummodel.GroupId + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">加入本" + ub.GetSub("GroupName", "/Controls/group.xml") + "</a>", "");
                }
            }
        }
        BCW.User.Users.ShowForumLimit(meid, Forummodel.Gradelt, Forummodel.Visitlt, Forummodel.VisitId, Forummodel.IsPc);
        BCW.User.Users.ShowAddReply(meid, Forummodel.Replylt);
        BCW.Model.Text p = new BCW.BLL.Text().GetText(bid);

        if (p.IsOver == 1)
        {
            Utils.Error("此帖子已经结束，不允许任何人回复", "");
        }
        if (p.IsLock == 1)
        {
            Utils.Error("此帖子已经被锁定，不允许任何人回复", "");
        }
        if (p.IsTop == -1)
        {
            Utils.Error("此帖子已经固底，不允许任何人回复", "");
        }
        string Content = Utils.GetRequest("Content", "post", 2, @"^[\s\S]{1," + ub.GetSub("BbsReplyMax", xmlPath) + "}$", "请输入不超" + ub.GetSub("BbsReplyMax", xmlPath) + "字的回帖内容");
        int Remind = int.Parse(Utils.GetRequest("Remind", "post", 1, @"^[0-3]$", "0"));
        int reid = int.Parse(Utils.GetRequest("reid", "post", 1, @"^[0-9]\d*$", "0"));
        int Face = int.Parse(Utils.GetRequest("Face", "post", 1, @"^[0-9]\d*$", "0"));
        if (Face > 0 && Face < 27)
            Content = "[F]" + Face + "[/F]" + Content;

        int ReplyNum = Utils.ParseInt(ub.GetSub("BbsReplyNum", xmlPath));
        if (ReplyNum > 0)
        {
            int ToDayCount = new BCW.BLL.Forumstat().GetCount(meid, 2);//今天发布回帖数
            if (ToDayCount >= ReplyNum)
            {
                Utils.Error("系统限每天每ID限回帖" + ReplyNum + "次", "");
            }
        }

        string mename = new BCW.BLL.User().GetUsName(meid);
        int Floor = new BCW.BLL.Reply().GetFloor(bid);

        //派币帖
        string CentText = string.Empty;
        string PbCent = string.Empty;
        int iTypes = p.Types;
        if (iTypes == 3)
        {
            BCW.Model.Text model1 = new BCW.BLL.Text().GetText(bid);
            if (p.Prices - p.Pricel > 0)
            {
                string bzText = string.Empty;
                if (p.BzType == 0)
                    bzText = ub.Get("SiteBz");
                else
                    bzText = ub.Get("SiteBz2");

                long zPrice = 0;
                if (p.Price2 > 0)
                    zPrice = Convert.ToInt64(new Random().Next(p.Price, (p.Price2 + 1)));//随机得到奖币值
                else
                    zPrice = Convert.ToInt64(p.Price);

                long GetPrice = 0;
                if (p.Prices - p.Pricel < zPrice)
                    GetPrice = p.Prices - p.Pricel;
                else
                    GetPrice = zPrice;

                bool a = ("#" + p.IsPriceID + "#").IndexOf("#" + meid + "#") == -1;

                if (p.PayCi == "0")  //判断派币楼层
                {
                    if (!string.IsNullOrEmpty(model1.PricesLimit))//如果要求回复特殊内容
                    {

                        // builder.Append("判断的TF"+a);
                        //  if (model1.PricesLimit.Equals(Content))  //如果回帖正确
                        if (model1.PricesLimit.Replace(" ", "").Equals(Content.Replace(" ", "")))  //如果回复附言正确
                        {
                            if (("#" + p.IsPriceID + "#").IndexOf("#" + meid + "#") == -1) //判断是否存在已派币ID
                            {
                                if (p.BzType == 0)
                                    new BCW.BLL.User().UpdateiGold(meid, mename, GetPrice, "派币帖回帖获得");
                                else
                                    new BCW.BLL.User().UpdateiMoney(meid, mename, GetPrice, "派币帖回帖获得");

                                //更新已派
                                new BCW.BLL.Text().UpdatePricel(bid, GetPrice);
                                CentText = "" + GetPrice + "" + bzText + "";
                                PbCent = "楼主派" + GetPrice + "" + bzText + "";
                                //更新派币ID
                                string IsPriceID = p.IsPriceID;
                                if (("#" + IsPriceID + "#").IndexOf("#" + meid + "#") == -1)
                                {
                                    string sPriceID = string.Empty;
                                    if (string.IsNullOrEmpty(IsPriceID))
                                        sPriceID = meid.ToString();
                                    else
                                        sPriceID = IsPriceID + "#" + meid;
                                    new BCW.BLL.Text().UpdateIsPriceID(bid, sPriceID);
                                }
                            }
                        }

                    }
                    else //不需要回复内容
                    {
                        //builder.Append("判断的TF" + a);
                        if (("#" + p.IsPriceID + "#").IndexOf("#" + meid + "#") == -1)  //判断是否存在已派币ID
                        {
                            if (p.BzType == 0)
                                new BCW.BLL.User().UpdateiGold(meid, mename, GetPrice, "派币帖回帖获得");
                            else
                                new BCW.BLL.User().UpdateiMoney(meid, mename, GetPrice, "派币帖回帖获得");

                            //更新已派
                            new BCW.BLL.Text().UpdatePricel(bid, GetPrice);
                            CentText = "" + GetPrice + "" + bzText + "";
                            PbCent = "楼主派" + GetPrice + "" + bzText + "";
                            //更新派币ID
                            string IsPriceID = p.IsPriceID;
                            if (("#" + IsPriceID + "#").IndexOf("#" + meid + "#") == -1)
                            {
                                string sPriceID = string.Empty;
                                if (string.IsNullOrEmpty(IsPriceID))
                                    sPriceID = meid.ToString();
                                else
                                    sPriceID = IsPriceID + "#" + meid;
                                new BCW.BLL.Text().UpdateIsPriceID(bid, sPriceID);
                            }
                        }
                    }

                }
                else
                {
                    if (!string.IsNullOrEmpty(model1.PricesLimit))//如果要求回复特殊内容
                    {
                        if (("#" + p.PayCi + "#").IndexOf("#" + Utils.Right(Floor.ToString(), 1) + "#") != -1) //判断要求派币的楼层
                        {
                            if (model1.PricesLimit.Replace(" ", "").Equals(Content.Replace(" ", "")))  //如果回复附言正确
                            // if (model1.PricesLimit.Equals(Content))  //如果回帖正确
                            {
                                // builder.Append("判断的TF" + a);
                                //if (("#" + p.IsPriceID + "#").IndexOf("#" + meid + "#") == -1) //判断是否存在已派币ID
                                //{
                                if (p.BzType == 0)
                                    new BCW.BLL.User().UpdateiGold(meid, mename, GetPrice, "派币帖回帖获得");
                                else
                                    new BCW.BLL.User().UpdateiMoney(meid, mename, GetPrice, "派币帖回帖获得");

                                //更新已派
                                new BCW.BLL.Text().UpdatePricel(bid, GetPrice);
                                CentText = "" + GetPrice + "" + bzText + "";
                                PbCent = "踩中楼层" + Utils.Right(Floor.ToString(), 1) + "尾，楼主派" + GetPrice + "" + bzText + "";
                                //更新派币ID
                                string IsPriceID = p.IsPriceID;
                                if (("#" + IsPriceID + "#").IndexOf("#" + meid + "#") == -1)
                                {
                                    string sPriceID = string.Empty;
                                    if (string.IsNullOrEmpty(IsPriceID))
                                        sPriceID = meid.ToString();
                                    else
                                        sPriceID = IsPriceID + "#" + meid;
                                    new BCW.BLL.Text().UpdateIsPriceID(bid, sPriceID);
                                }
                                //}
                            }
                        }
                    }
                    else //不需要回复内容
                    {
                        if (("#" + p.PayCi + "#").IndexOf("#" + Utils.Right(Floor.ToString(), 1) + "#") != -1)
                        {
                            // builder.Append("判断的TF" + a);
                            //if (("#" + p.IsPriceID + "#").IndexOf("#" + meid + "#") == -1) //判断是否存在已派币ID
                            //{
                            if (p.BzType == 0)
                                new BCW.BLL.User().UpdateiGold(meid, mename, GetPrice, "派币帖回帖获得");
                            else
                                new BCW.BLL.User().UpdateiMoney(meid, mename, GetPrice, "派币帖回帖获得");
                            //更新已派
                            new BCW.BLL.Text().UpdatePricel(bid, GetPrice);
                            CentText = "" + GetPrice + "" + bzText + "";
                            PbCent = "踩中楼层" + Utils.Right(Floor.ToString(), 1) + "尾，楼主派" + GetPrice + "" + bzText + "";
                            //更新派币ID
                            string IsPriceID = p.IsPriceID;
                            if (("#" + IsPriceID + "#").IndexOf("#" + meid + "#") == -1)
                            {
                                string sPriceID = string.Empty;
                                if (string.IsNullOrEmpty(IsPriceID))
                                    sPriceID = meid.ToString();
                                else
                                    sPriceID = IsPriceID + "#" + meid;
                                new BCW.BLL.Text().UpdateIsPriceID(bid, sPriceID);
                            }
                            //}
                        }
                    }

                }
                //检测15天前的派币帖，如果没有派完则自动清0并自动结帖
                if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th"))
                {
                    BCW.Data.SqlHelper.ExecuteSql("update tb_Text set Pricel=Prices,IsOver=1 where Types=3 and AddTime<'" + DateTime.Now.AddDays(-15) + "'");
                }
                else
                {
                    BCW.Data.SqlHelper.ExecuteSql("update tb_Text set Pricel=Prices,IsOver=1 where Types=3 and AddTime<'" + DateTime.Now.AddDays(-7) + "'");

                }
            }
            else
            {
                //派完币即结帖
                new BCW.BLL.Text().UpdateIsOver(bid, 1);
            }
        }


        BCW.Model.Reply model = new BCW.Model.Reply();
        model.Floor = Floor;
        model.ForumId = forumid;
        model.Bid = bid;
        model.UsID = meid;
        model.UsName = mename;
        model.Content = Content;
        model.FileNum = 0;
        model.ReplyId = reid;
        model.AddTime = DateTime.Now;
        model.CentText = CentText;
        new BCW.BLL.Reply().Add(model);

        //builder.Append("p.IsPriceID=" + p.IsPriceID);

        //更新回复ID
        string sReplyID = p.ReplyID;
        if (("#" + sReplyID + "#").IndexOf("#" + meid + "#") == -1)
        {
            string ReplyID = string.Empty;
            if (string.IsNullOrEmpty(sReplyID))
                ReplyID = meid.ToString();
            else
                ReplyID = sReplyID + "#" + meid;
            new BCW.BLL.Text().UpdateReplyID(bid, ReplyID);
        }

        //更新回复数
        new BCW.BLL.Text().UpdateReplyNum(bid, 1);

        //回复提醒:0|不提醒|1|帖子作者|2|回帖作者|3|全部提醒
        string strRemind = string.Empty;
        //提醒费用
        long Tips = Convert.ToInt64(ub.GetSub("BbsReplyTips", xmlPath));
        if (Remind == 1 || Remind == 3)
        {
            if (!p.UsID.Equals(meid))
            {
                string pForumSet = new BCW.BLL.User().GetForumSet(p.UsID);
                if (BCW.User.Users.GetForumSet(pForumSet, 14) == 0)
                {
                    if (new BCW.BLL.User().GetGold(meid) >= Tips)
                    {

                        new BCW.BLL.Guest().Add(p.UsID, p.UsName, "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]回复了您的帖子[url=/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "]" + p.Title + "[/url]");
                        if (Tips > 0)
                        {
                            new BCW.BLL.User().UpdateiGold(meid, mename, -Tips, "回帖提醒帖子作者");
                        }
                    }
                }
                else
                {
                    strRemind = "帖子作者拒绝接收提醒消息.<br />";
                }
            }
        }


        if (Remind == 2 || Remind == 3)
        {
            //回帖用户实体
            BCW.Model.Reply m = new BCW.BLL.Reply().GetReplyMe(bid, reid);
            if (!m.UsID.Equals(meid))
            {
                string mForumSet = new BCW.BLL.User().GetForumSet(m.UsID);
                if (BCW.User.Users.GetForumSet(mForumSet, 14) == 0)
                {
                    if (new BCW.BLL.User().GetGold(meid) >= Tips)
                    {
                        string neirong = new BCW.BLL.Reply().GetContent(bid, reid);
                        if (neirong.Length > 30)
                        {
                            neirong = neirong.Substring(0, 30);
                            neirong += "...";
                            //builder.Append(":" + neirong);
                        }
                        else
                        {
                            // builder.Append(":" + neirong);
                        }
                        if (Content.Length > 30)
                        {
                            Content = Content.Substring(0, 30);
                            Content += "...";
                            //builder.Append(":" + neirong);
                        }
                        //  修改这里
                        // builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?act=reply&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + reid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">点评回复</a>|");
                        new BCW.BLL.Guest().Add(m.UsID, m.UsName, "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]点评了您的回帖[url=/bbs/reply.aspx?act=view&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + reid + "]" + reid + "楼[/url]:" + neirong + "<br/>回复内容为:" + Content + "[url=/bbs/reply.aspx?act=view&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + Floor + "]更多[/url]<br/>[url=/bbs/reply.aspx?act=reply&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + Floor + "]点评回复[/url]");
                        if (Tips > 0)
                        {
                            new BCW.BLL.User().UpdateiGold(meid, mename, -Tips, "回帖提醒回帖作者");
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(strRemind))
                        strRemind = "帖子作者与回帖作者拒绝接收提醒消息.<br />";
                    else
                        strRemind = "回帖作者拒绝接收提醒消息.<br />";
                }
            }
        }
        //论坛统计
        BCW.User.Users.UpdateForumStat(2, meid, mename, forumid);
        //动态记录
        if (Forummodel.GroupId == 0)
        {
            new BCW.BLL.Action().Add(-1, 0, meid, mename, "在" + Forummodel.Title + "回复帖子[URL=/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "]" + new BCW.BLL.Text().GetTitle(bid) + "[/URL]");
        }
        else
        {
            new BCW.BLL.Action().Add(-2, 0, meid, mename, "在圈坛-" + Forummodel.Title + "回复帖子[URL=/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "]" + new BCW.BLL.Text().GetTitle(bid) + "[/URL]");
        }
        //积分操作/论坛统计/圈子论坛不进行任何奖励
        int IsAcc = -1;
        if (Forummodel.GroupId == 0)
        {
            IsAcc = new BCW.User.Cent().UpdateCent2(BCW.User.Cent.enumRole.Cent_Reply, meid, true);
        }
        else
        {
            if (!Utils.GetDomain().Contains("th"))
                IsAcc = new BCW.User.Cent().UpdateCent2(BCW.User.Cent.enumRole.Cent_Reply, meid, false);
        }

        int ReplyGo = Utils.ParseInt(ub.GetSub("BbsReplyGo", xmlPath));
        string GoUrl = string.Empty;
        if (ReplyGo == 0)
            GoUrl = Utils.getUrl("reply.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "");
        else if (ReplyGo == 1)
            GoUrl = Utils.getUrl("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "");
        else
            GoUrl = Utils.getPage("forum.aspx?forumid=" + forumid + "");

        if (IsAcc >= 0)
            Utils.Success("回帖", "" + strRemind + "回复帖子成功！" + PbCent + "恭喜您获得" + BCW.User.Users.GetWinCent(1, meid) + "<br /><a href=\"" + Utils.getUrl("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">返回主题</a><br /><a href=\"" + Utils.getUrl("reply.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">回复列表</a>", GoUrl, "2");
        else
            Utils.Success("回帖", "" + strRemind + "回复帖子成功！" + PbCent + "<br /><a href=\"" + Utils.getUrl("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">返回主题</a><br /><a href=\"" + Utils.getUrl("reply.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">回复列表</a>", GoUrl, "2");

    }

    /// <summary>
    /// 穿透圈子限制ID
    /// </summary>
    private bool IsCTID(int meid)
    {
        bool Isvi = false;
        //能够穿透的ID
        string CTID = "#" + ub.GetSub("GroupCTID", "/Controls/group.xml") + "#";
        if (CTID.IndexOf("#" + meid + "#") != -1)
        {
            Isvi = true;
        }

        return Isvi;
    }
}