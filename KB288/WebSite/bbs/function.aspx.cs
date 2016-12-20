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
/// 修复下一页选择问题
/// 黄国军 20160729
/// 
/// 陈志基 2016-5-21
/// 修改 RecomSaveAllPage（）
/// 附言重复问题
/// </summary>
public partial class bbs_function : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/guest.xml";
    protected string strText = string.Empty;
    protected string strName = string.Empty;
    protected string strType = string.Empty;
    protected string strValu = string.Empty;
    protected string strEmpt = string.Empty;
    protected string strIdea = string.Empty;
    protected string strOthe = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string act = Utils.GetRequest("act", "all", 1, "", "");
        string actok = Utils.GetRequest("actok", "all", 1, "", "");
        string ToIds = Utils.GetRequest("ToIds", "all", 1, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "");

        if (actok == "recomsaveall" && ToIds != "")
            act = "recomsaveall";

        switch (act)
        {
            case "addsub":
                AddSubPage(meid);
                break;
            case "savesub":
            case "savetemp":
                SaveSubPage(act, meid);
                break;
            case "admsub":
            case "admtemp":
                AdmSubPage(act, meid);
                break;
            case "editsub":
            case "edittemp":
                EditSubPage(act, meid);
                break;
            case "delsub":
            case "deltemp":
                DelSubPage(act, meid);
                break;
            case "viewsub":
            case "viewtemp":
                ViewSubPage(act, meid);
                break;
            case "face":
            case "face2":
                FacePage(act);
                break;
            case "fzl":
                fzlPage(meid);
                break;
            case "fzr":
                fzrPage(meid);
                break;
            case "copyg":
                CopygPage(meid);
                break;
            case "recom":
                RecomPage(meid);
                break;
            case "recomsave":
                RecomSavePage(meid);
                break;
            case "recomsaveall":
                RecomSaveAllPage(meid);
                break;
            case "recomadmin":
                RecomAdminPage(meid);
                break;
            case "recomadminsave":
                RecomAdminSavePage(meid);
                break;
            default:
                ReloadPage(meid);
                break;
        }
    }

    private void ReloadPage(int uid)
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
        Master.Title = "论坛功能箱";
        builder.Append(Out.Tab("<div class=\"title\">论坛功能箱</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("+|基本功能:");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("topic.aspx?act=fzt&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">复制内容</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("function.aspx?act=fzl&amp;backurl=" + Utils.getPage(0) + "") + "\">复制链接</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("favorites.aspx?act=addok&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;ptype=1&amp;backurl=" + Utils.PostPage(1) + "") + "\">收藏</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("topic.aspx?act=downtxt&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">下载txt</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("topic.aspx?act=downjar&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">jar</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("topic.aspx?act=downumd&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">umd</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("topic.aspx?act=downpdf&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">pdf</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("topic.aspx?act=downchm&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">chm</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("topic.aspx?act=downword&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">word</a><br />");
        if (new BCW.BLL.Role().IsSumMode(uid))
        {
            builder.Append("<a href=\"" + Utils.getUrl("function.aspx?act=recomadmin&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;推给管理员/版主</a><br />");
        }
        builder.Append("<a href=\"" + Utils.getUrl("function.aspx?act=recom&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;推荐给好友</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        string purl = "[url=/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "]" + new BCW.BLL.Text().GetTitle(bid) + "[/url]";
        strText = ",,,";
        strName = "purl,act,backurl";
        strType = "hidden,hidden,hidden";
        strValu = "" + purl + "'recommend'" + Utils.PostPage(1) + "";
        strEmpt = "false,false,false";
        strIdea = "/";
        strOthe = "&gt;举报帖子,guest.aspx,post,1,other";
        builder.Append(Out.wapform(strName, strValu, strOthe));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?act=forum&amp;backurl=" + Utils.PostPage(1) + "") + "\">个性设置</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void RecomAdminPage(int uid)
    {
        if (!new BCW.BLL.Role().IsSumMode(uid))
        {
            Utils.Error("你的权限不足", "");
        }
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
        Master.Title = "推荐给管理员/版主";
        builder.Append(Out.Tab("<div class=\"title\">推荐给管理员/版主</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("输入附言(50字内)：");
        builder.Append(Out.Tab("</div>", ""));
        strText = ",,,,,";
        strName = "Content,forumid,bid,act,backurl";
        strType = "textarea,hidden,hidden,hidden,hidden";
        strValu = "'" + forumid + "'" + bid + "'recomadminsave'" + Utils.getPage(0) + "";
        strEmpt = "false,false,false,false,false";
        strIdea = "/";
        strOthe = "&gt;确定推荐,function.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getPage("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "") + "\">&lt;&lt;返回主题帖</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("function.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">功能箱</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void RecomAdminSavePage(int uid)
    {
        if (!new BCW.BLL.Role().IsSumMode(uid))
        {
            Utils.Error("你的权限不足", "");
        }
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
        string Content = Utils.GetRequest("Content", "post", 3, @"^[\s\S]{1,50}$", "附言限50字内，可以留空");

        string strWhere = "Status=0 and (OverTime>='" + DateTime.Now + "' OR OverTime='1990-1-1 00:00:00')";

        string usname = new BCW.BLL.User().GetUsName(uid);
        if (Content != "")
            Content = "，附言:" + Content + "";

        string strContent = "[url=/bbs/uinfo.aspx?uid=" + uid + "]" + usname + "[/url]向全体管理员/版主推荐帖子：[url=/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "]" + new BCW.BLL.Text().GetTitle(bid) + "[/url]" + Content + "";

        DataSet ds = new BCW.BLL.Role().GetList("UsID,UsName", strWhere);
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                int ihid = int.Parse(ds.Tables[0].Rows[i]["UsID"].ToString());
                string mename = ds.Tables[0].Rows[i]["UsName"].ToString();
                new BCW.BLL.Guest().Add(ihid, mename, strContent);
            }
        }
        Utils.Success("推荐给管理员/版主", "恭喜，推荐管理员/版主成功，正在返回..", Utils.getUrl("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + ""), "2");
    }

    private void RecomPage(int uid)
    {
        Master.Title = "推荐帖子";
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
        string VE = ConfigHelper.GetConfigString("VE");
        string SID = ConfigHelper.GetConfigString("SID");
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<b>推荐给好友</b>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<form name=\"rdshare\" action=\"function.aspx\" method=\"post\">", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("附言(50字内,可空):<br /><input maxlength=\"500\" type=\"text\" emptyok=\"true\" name=\"Content\" />");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("〓选择好友马上推荐〓");
        builder.Append(Out.Tab("</div>", "<br />"));
        string acText = string.Empty;
        if (Utils.Isie())
        {
            builder.Append("<input type=\"hidden\" name=\"ToId\"/>");
            builder.Append("<input type=\"hidden\" name=\"Content\"/>");
            builder.AppendFormat("<input type=\"hidden\" name=\"forumid\" value=\"{0}\"/>", forumid);
            builder.AppendFormat("<input type=\"hidden\" name=\"bid\" value=\"{0}\"/>", bid);
            builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
            builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
            builder.AppendFormat("<input type=\"hidden\" name=\"act\" value=\"{0}\"/>", "recomsave");
        }
        else
        {
            acText += "<postfield name=\"Content\" value=\"$(Content)\"/>";

        }
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "forumid", "bid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "UsID=" + uid + " and Types=0";
        // 开始读取列表
        IList<BCW.Model.Friend> listFriend = new BCW.BLL.Friend().GetFriends(pageIndex, pageSize, strWhere, out recordCount);
        if (listFriend.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Friend n in listFriend)
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

                string sFriendName = n.FriendName;
                string nFriendName = new BCW.BLL.User().GetUsName(n.FriendID);
                if (sFriendName != nFriendName)
                    sFriendName = n.FriendName + "(" + nFriendName + ")";

                if (Utils.Isie())
                {
                    builder.Append("<a href=\"" + Utils.getUrl("#") + "\" onclick=\"Submitshare('" + n.FriendID + "')\">" + BCW.User.Users.SetUser(n.FriendID) + "(" + n.FriendID + ")</a></div>");
                }
                else
                {
                    builder.Append("<anchor title=\"" + sFriendName + "\"><go href=\"" + Utils.getUrl("function.aspx?act=recomsave&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;ToId=" + n.FriendID + "") + "\" method=\"post\" accept-charset=\"utf-8\">" + acText + "</go>" + BCW.User.Users.SetUser(n.FriendID) + "(" + n.FriendID + ")</anchor>");
                }
                k++;
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 1));

        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("〓输入会员ID推荐〓<br />提示：多个会员ID用#分开,最多15人");
        builder.Append("<br /><input maxlength=\"500\" type=\"text\" emptyok=\"true\" name=\"ToIds\" />");
        builder.Append(Out.Tab("</div>", ""));
        if (Utils.Isie())
        {
            builder.AppendFormat("<input type=\"hidden\" name=\"actok\" value=\"{0}\"/>", "recomsaveall");
            builder.Append("<div><input class=\"btn-red\" type=\"submit\" value=\"确定推荐\"/>");
            builder.Append("</form>");
        }
        else
        {
            acText += "<postfield name=\"forumid\" value=\"" + forumid + "\"/>";
            acText += "<postfield name=\"bid\" value=\"" + bid + "\"/>";
            acText += "<postfield name=\"" + VE + "\" value=\"" + Utils.getstrVe() + "\"/>";
            acText += "<postfield name=\"" + SID + "\" value=\"" + Utils.getstrU() + "\"/>";
            acText += "<postfield name=\"act\" value=\"recomsaveall\"/>";
            acText += "<postfield name=\"ToIds\" value=\"$(ToIds)\"/>";
            builder.Append("<br /><anchor title=\"确定推荐\"><go href=\"function.aspx\" method=\"post\" accept-charset=\"utf-8\">" + acText + "</go>确定推荐</anchor>");
        }
        builder.Append(Out.Tab("", "<br />"));
        string strName = "purl,act,backurl";
        string strValu = "'recommend'" + Utils.getPage(0) + "";
        string strOthe = "&gt;按好友分组分享,/bbs/guest.aspx,post,1,other";
        builder.Append(Out.wapform(strName, strValu, strOthe));

        builder.Append(Out.Tab("<div>", "<br />"));
        long Share = Convert.ToInt64(ub.GetSub("GuestShare", xmlPath));
        if (Share > 0)
        {
            builder.Append("温馨提示:推荐每人收费" + Share + "" + ub.Get("SiteBz") + "<br />");
        }
        builder.Append("<a href=\"" + Utils.getPage("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "") + "\">&lt;&lt;返回主题帖</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("function.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">功能箱</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void RecomSavePage(int uid)
    {
        BCW.User.Users.ShowVerifyRole("o", uid);//非验证会员提示
        new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Guest, uid);//会员本身权限
        //int forumid = int.Parse(Utils.GetRequest("forumid", "all", 2, @"^[0-9]\d*$", "论坛ID错误"));
        //int bid = int.Parse(Utils.GetRequest("bid", "all", 2, @"^[0-9]\d*$", "帖子ID错误"));
        string Forforumid = Utils.GetRequest("forumid", "all", 2, @"^[^\^]+$", "论坛ID错误");
        string Forbid = Utils.GetRequest("bid", "all", 2, @"^[^\^]+$", "帖子ID错误");
        if (Forforumid.Contains(","))
        {
            Forforumid = Utils.DelLastChar(Forforumid, ",");
        }
        if (Forbid.Contains(","))
        {
            Forbid = Utils.DelLastChar(Forbid, ",");
        }
        if (Utils.ParseInt(Forforumid) == 0)
        {
            Utils.Error("论坛ID错误", "");
        }
        if (Utils.ParseInt(Forbid) == 0)
        {
            Utils.Error("帖子ID错误", "");
        }
        int forumid = Convert.ToInt32(Forforumid);
        int bid = Convert.ToInt32(Forbid);

        if (!new BCW.BLL.Forum().Exists2(forumid))
        {
            Utils.Success("访问论坛", "该论坛不存在或已暂停使用", Utils.getUrl("forum.aspx"), "1");
        }
        if (!new BCW.BLL.Text().Exists2(bid, forumid))
        {
            Utils.Error("帖子不存在或已被删除", "");
        }
        int ToId = int.Parse(Utils.GetRequest("ToId", "all", 2, @"^[1-9]\d*$", "推荐会员ID错误"));
        string Content = Utils.GetRequest("Content", "post", 3, @"^[^\^]{1,50}$", "附言最多50字，可留空");
        if (Content.Contains(","))
        {
            Content = Utils.DelLastChar(Content, ",");
        }
        if (!new BCW.BLL.User().Exists(ToId))
        {
            Utils.Error("不存在的推荐会员ID", "");
        }
        //你是否是对方的黑名单
        if (new BCW.BLL.Friend().Exists(ToId, uid, 1))
        {
            Utils.Error("对方已把您加入黑名单", "");
        }
        string ForumSet = new BCW.BLL.User().GetForumSet(ToId);
        int Nore = BCW.User.Users.GetForumSet(ForumSet, 14);
        if (Nore == 1)
        {
            Utils.Error("对方已设置拒绝接收推荐消息", "");
        }
        if (Content != "")
            Content = "，附言:" + Content + "";

        string UsName = new BCW.BLL.User().GetUsName(uid);
        BCW.Model.Guest model = new BCW.Model.Guest();
        model.FromId = 0;
        model.FromName = UsName;
        model.ToId = ToId;
        model.ToName = new BCW.BLL.User().GetUsName(ToId);
        model.Content = "[url=/bbs/uinfo.aspx?uid=" + uid + "]" + UsName + "[/url]向您推荐帖子：[url=/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "]" + new BCW.BLL.Text().GetTitle(bid) + "[/url]" + Content + "";
        model.TransId = 0;
        new BCW.BLL.Guest().Add(model);
        //更新联系时间
        new BCW.BLL.Friend().UpdateTime(uid, ToId);
        long Share = Convert.ToInt64(ub.GetSub("GuestShare", xmlPath));
        if (Share > 0)
        {
            new BCW.BLL.User().UpdateiGold(uid, -Share, "分享好友");
        }
        Utils.Success("推荐帖子", "推荐帖子成功<br /><a href=\"" + Utils.getUrl("function.aspx?act=recom&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">继续推荐&gt;&gt;</a>", Utils.getPage("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + ""), "3");
    }

    private void RecomSaveAllPage(int uid)
    {
        BCW.User.Users.ShowVerifyRole("o", uid);//非验证会员提示
        new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Guest, uid);//会员本身权限
        //int forumid = int.Parse(Utils.GetRequest("forumid", "all", 2, @"^[0-9]\d*$", "论坛ID错误"));
        //int bid = int.Parse(Utils.GetRequest("bid", "all", 2, @"^[0-9]\d*$", "帖子ID错误"));
        string Forforumid = Utils.GetRequest("forumid", "all", 2, @"^[^\^]+$", "论坛ID错误");
        string Forbid = Utils.GetRequest("bid", "all", 2, @"^[^\^]+$", "帖子ID错误");
        if (Forforumid.Contains(","))
        {
            Forforumid = Utils.DelLastChar(Forforumid, ",");
        }
        if (Forbid.Contains(","))
        {
            Forbid = Utils.DelLastChar(Forbid, ",");
        }
        if (Utils.ParseInt(Forforumid) == 0)
        {
            Utils.Error("论坛ID错误", "");
        }
        if (Utils.ParseInt(Forbid) == 0)
        {
            Utils.Error("帖子ID错误", "");
        }
        int forumid = Convert.ToInt32(Forforumid);
        int bid = Convert.ToInt32(Forbid);

        if (!new BCW.BLL.Forum().Exists2(forumid))
        {
            Utils.Success("访问论坛", "该论坛不存在或已暂停使用", Utils.getUrl("forum.aspx"), "1");
        }
        if (!new BCW.BLL.Text().Exists2(bid, forumid))
        {
            Utils.Error("帖子不存在或已被删除", "");
        }
        string ToIds = Utils.GetRequest("ToIds", "all", 2, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "多个会员ID请用#分隔");
        string Content = Utils.GetRequest("Content", "post", 3, @"^[^\^]{1,50}$", "附言最多50字，可留空");
        if (Content.Contains(","))
        {
            Content = Utils.DelLastChar(Content, ",");
        }
        if (!Utils.IsRegex(ToIds.Replace("#", ""), @"^[0-9]\d*$"))
        {
            Utils.Error("多个会员ID请用#分隔", "");
        }
        int iNum = Utils.GetStringNum(ToIds, "#");
        if (iNum > 14)
        {
            Utils.Error("最多可以推荐15人", "");
        }
        string[] Temp = ToIds.Split('#');
        int k = 0;
        if (Content != "")
            Content = "，附言:" + Content + "";

        for (int i = 0; i < Temp.Length; i++)
        {
            int ToId = Utils.ParseInt(Temp[i]);
            if (!new BCW.BLL.User().Exists(ToId))
            {
                continue;//不存在的推荐会员ID
            }
            //你是否是对方的黑名单
            if (new BCW.BLL.Friend().Exists(ToId, uid, 1))
            {
                continue;//对方已把您加入黑名单
            }
            string ForumSet = new BCW.BLL.User().GetForumSet(ToId);
            int Nore = BCW.User.Users.GetForumSet(ForumSet, 14);
            if (Nore == 1)
            {
                continue;//对方已设置拒绝接收推荐消息
            }

            string UsName = new BCW.BLL.User().GetUsName(uid);
            BCW.Model.Guest model = new BCW.Model.Guest();
            model.FromId = 0;
            model.FromName = UsName;
            model.ToId = ToId;
            model.ToName = new BCW.BLL.User().GetUsName(ToId);
            model.Content = "[url=/bbs/uinfo.aspx?uid=" + uid + "]" + UsName + "[/url]向您推荐帖子：[url=/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "]" + new BCW.BLL.Text().GetTitle(bid) + "[/url]" + Content + "";
            model.TransId = 0;
            new BCW.BLL.Guest().Add(model);
            //更新联系时间
            new BCW.BLL.Friend().UpdateTime(uid, ToId);
            long Share = Convert.ToInt64(ub.GetSub("GuestShare", xmlPath));
            if (Share > 0)
            {
                new BCW.BLL.User().UpdateiGold(uid, -Share, "分享好友");
            }
            k++;
        }
        Utils.Success("推荐帖子", "恭喜！成功推荐" + k + "位会员<br /><a href=\"" + Utils.getUrl("function.aspx?act=recom&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">继续推荐&gt;&gt;</a>", Utils.getPage("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + ""), "3");
    }


    /// <summary>
    /// 复制链接
    /// </summary>
    private void fzlPage(int uid)
    {
        Master.Title = "复制选项";
        Master.IsFoot = false;
        builder.Append(Out.Tab("<div class=\"title\">复制选项</div>", ""));
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info != "")
        {
            bool IsSource = false;
            string pContent = Out.UBB(Utils.removeUVe(Utils.getPage(1)));
            pContent = "http://" + Utils.GetDomain() + "" + pContent + "";
            if (info == "ok1")
            {
                IsSource = true;
            }
            else if (info == "ok2")
            {
                IsSource = true;
                pContent = "[url=" + pContent + "]点击这里[/url]";
            }
            else if (info == "ok3" || info == "ok4")
            {
                string Content = Utils.GetRequest("Content", "post", 1, "", "");
                if (Content == "")
                {
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("您可以编辑后再复制");
                    builder.Append(Out.Tab("</div>", ""));
                    string str = Utils.GetSourceTextByUrl(Utils.getUrl(pContent).Replace("&amp;", "&"));
                    if (info == "ok3")
                    {
                        str = Regex.Replace(str, @"<style[\s\S]+?</style>", "");
                        str = Out.WmlEncode(Out.RemoveHtml(str));
                    }
                    else
                        str = Out.WmlEncode(str);

                    strText = ",,,,";
                    strName = "Content,act,info,backurl";
                    strType = "textarea,hidden,hidden,hidden";
                    strValu = "" + str + "'fzl'" + info + "'" + Utils.getPage(0) + "";
                    strEmpt = "false,false,false,false";
                    strIdea = "/";
                    strOthe = "&gt;确定复制,function.aspx,post,1,red";
                    builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                    builder.Append(Out.Tab("<div>", " "));
                    builder.Append(Out.back("取消"));
                    builder.Append(Out.Tab("</div>", ""));
                }
                else
                {
                    pContent = Content;
                    IsSource = true;
                }
            }
            if (IsSource)
            {
                new BCW.BLL.User().UpdateCopytemp(uid, pContent);
                Utils.Success("复制", "恭喜，复制成功，正在返回..", Utils.getPage("default.aspx"), "1");
            }
        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("function.aspx?act=fzl&amp;info=ok1&amp;backurl=" + Utils.getPage(0) + "") + "\">复制文本地址</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("function.aspx?act=fzl&amp;info=ok2&amp;backurl=" + Utils.getPage(0) + "") + "\">复制UBB地址</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("function.aspx?act=fzl&amp;info=ok3&amp;backurl=" + Utils.getPage(0) + "") + "\">复制页面内容</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("function.aspx?act=fzl&amp;info=ok4&amp;backurl=" + Utils.getPage(0) + "") + "\">复制网页源码</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    /// <summary>
    /// 复制回帖
    /// </summary>
    private void fzrPage(int uid)
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
        int reid = int.Parse(Utils.GetRequest("reid", "get", 2, @"^[0-9]\d*$", "回帖ID错误"));
        if (!new BCW.BLL.Reply().Exists(bid, reid))
            Utils.Error("不存在的记录", "");

        string Content = new BCW.BLL.Reply().GetContent(bid, reid);
        new BCW.BLL.User().UpdateCopytemp(uid, Content);
        Utils.Success("复制回帖内容", "恭喜，复制回帖内容成功，正在返回..", Utils.getUrl("reply.aspx?act=view&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + reid + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
    }

    /// <summary>
    /// 复制消息
    /// </summary>
    private void CopygPage(int uid)
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Guest().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        BCW.Model.Guest model = new BCW.BLL.Guest().GetGuest(id);
        if (model.FromId == uid)
        {
            if (!new BCW.BLL.Guest().ExistsFrom(id, uid))
            {
                Utils.Error("不存在的记录", "");
            }
        }
        else
        {
            if (!new BCW.BLL.Guest().ExistsTo(id, uid))
            {
                Utils.Error("不存在的记录", "");
            }
        }

        string Content = model.Content;
        new BCW.BLL.User().UpdateCopytemp(uid, Content);
        Utils.Success("复制消息内容", "恭喜，复制消息内容成功，正在返回..", Utils.getUrl("guest.aspx?act=view&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
    }

    private void FacePage(string act)
    {
        Master.Title = "选择表情";
        Master.IsFoot = false;
        builder.Append(Out.Tab("<div class=\"title\">选择表情</div>", ""));

        int pageIndex;
        int recordCount;
        int kk = 0;
        int pageSize = 5;
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //总记录数
        if (act == "face")
        {
            kk = 1;
            recordCount = 24;
        }
        else
        {
            kk = 1001;
            recordCount = 13;
        }
        builder.Append(Out.Tab("<div>", ""));
        int stratIndex = (pageIndex - 1) * pageSize;
        int endIndex = pageIndex * pageSize;
        int k = 0;
        string gUrl = Server.UrlDecode(Utils.getPage(0));
        gUrl = gUrl.Replace("&amp;", "&");
        gUrl = Regex.Replace(gUrl, @"([\s\S]+?)[&|?]{0,1}ff=[^&]*&{0,}", @"$1&amp;", RegexOptions.IgnoreCase);
        gUrl = Out.UBB(gUrl);
        for (int i = 0; i < recordCount; i++)
        {
            if (k >= stratIndex && k < endIndex)
            {
                builder.Append("<a href=\"" + Utils.getUrl(gUrl + "&amp;ff=" + (kk + i) + "") + "\"><img src=\"/files/Face/" + (kk + i) + ".gif\" alt=\"load\"/></a>\r\n");

            }
            if (k == endIndex)
                break;
            k++;
        }
        builder.Append(Out.Tab("</div>", ""));

        // 分页
        builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        if (act == "face")
            builder.Append("<a href=\"" + Utils.getUrl("function.aspx?act=face2&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;切换表情二</a>");
        else
            builder.Append("<a href=\"" + Utils.getUrl("function.aspx?act=face&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;切换表情一</a>");

        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }


    private void AdmSubPage(string act, int uid)
    {
        string sName = "短语";
        string acturl = "sub";
        int itype = 0;
        if (act == "admtemp")
        {
            sName = "草稿";
            itype = 1;
            acturl = "temp";
        }

        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-1]\d*$", "0"));
        string sTitle = string.Empty;
        if (ptype == 1)
            sTitle = "管理" + sName + "";
        else
            sTitle = "选择" + sName + "";
        Master.Title = sTitle;
        Master.IsFoot = false;
        builder.Append(Out.Tab("<div class=\"title\">" + sTitle + "</div>", ""));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "ptype", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件

        strWhere = "UsID=" + uid + " and Types=" + itype + "";

        // 开始读取列表
        IList<BCW.Model.Submit> listSubmit = new BCW.BLL.Submit().GetSubmits(pageIndex, pageSize, strWhere, out recordCount);
        if (listSubmit.Count > 0)
        {
            int k = 1;
            string gUrl = Server.UrlDecode(Utils.getPage(0));
            gUrl = gUrl.Replace("&amp;", "&");
            gUrl = Regex.Replace(gUrl, @"([\s\S]+?)[&|?]{0,1}dd=[^&]*&{0,}", @"$1&amp;", RegexOptions.IgnoreCase);
            gUrl = Out.UBB(gUrl);
            foreach (BCW.Model.Submit n in listSubmit)
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
                string sText = n.Content;
                if (sText.Length > 12)
                    sText = Utils.Left(sText, 12) + "...";
                builder.AppendFormat("[{0}]<a href=\"" + Utils.getUrl("function.aspx?act=view" + acturl + "&amp;id={1}&amp;backurl=" + Utils.PostPage(true) + "") + "\">{2}</a>", (pageIndex - 1) * pageSize + k, n.ID, sText);

                if (gUrl.ToLower().Contains(".aspx"))
                {
                    builder.Append("<a href=\"" + Utils.getUrl(gUrl + "&amp;dd=" + n.ID + "") + "\">[选择]</a>");
                }
                else
                {
                    if (act == "admtemp")
                    {
                        builder.Append("<a href=\"" + Utils.getUrl("addThread.asp?dd=" + n.ID + "") + "\">[发表]</a>");
                    }
                }
                if (ptype == 1)
                {
                    builder.Append("<br /><a href=\"" + Utils.getUrl("function.aspx?act=edit" + acturl + "&amp;id=" + n.ID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[编辑]</a>.");
                    builder.Append("<a href=\"" + Utils.getUrl("function.aspx?act=del" + acturl + "&amp;id=" + n.ID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[删除]</a>");
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

        if (ptype == 0)
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("function.aspx?act=adm" + acturl + "&amp;ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;切换管理</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            if (act != "admtemp")
            {
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("function.aspx?act=addsub&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;新建短语</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
        }

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ViewSubPage(string act, int uid)
    {
        string sName = "短语";
        string acturl = "sub";
        int itype = 0;
        if (act == "viewtemp")
        {
            sName = "草稿";
            itype = 1;
            acturl = "temp";
        }
        Master.Title = "查看" + sName + "";
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Submit().Exists(id, uid, itype))
        {
            Utils.Error("不存在的记录", "");
        }
        BCW.Model.Submit model = new BCW.BLL.Submit().GetSubmit(id);

        builder.Append(Out.Tab("<div class=\"title\">查看" + sName + "</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(model.Content + "<br />");
        string gUrl = Server.UrlDecode(Utils.getPage(0));
        gUrl = gUrl.Replace("&amp;", "&");
        gUrl = Regex.Replace(gUrl, @"([\s\S]+?)[&|?]{0,1}dd=[^&]*&{0,}", @"$1&amp;", RegexOptions.IgnoreCase);
        gUrl = Out.UBB(gUrl);
        if (gUrl.ToLower().Contains("addthread.aspx"))
        {
            builder.Append("<a href=\"" + Utils.getUrl(gUrl + "&amp;dd=" + id + "") + "\">[发表]</a>.");
        }
        else
        {
            if (act == "viewtemp")
            {
                builder.Append("<a href=\"" + Utils.getUrl("addThread.asp?dd=" + id + "") + "\">[发表]</a>.");
            }
        }
        builder.Append("<a href=\"" + Utils.getUrl("function.aspx?act=edit" + acturl + "&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[编辑]</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("function.aspx?act=del" + acturl + "&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[删除]</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("function.aspx?act=adm" + acturl + "&amp;backurl=" + Utils.getPage(0) + "") + "\">管理" + sName + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void EditSubPage(string act, int uid)
    {
        string sName = "短语";
        int itype = 0;
        if (act == "edittemp")
        {
            sName = "草稿";
            itype = 1;
        }
        Master.Title = "编辑" + sName + "";
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Submit().Exists(id, uid, itype))
        {
            Utils.Error("不存在的记录", "");
        }
        BCW.Model.Submit model = new BCW.BLL.Submit().GetSubmit(id);
        builder.Append(Out.Tab("<div class=\"title\">编辑" + sName + "</div>", ""));
        if (act == "editsub")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("请输入不超300字的短语：");
            builder.Append(Out.Tab("</div>", ""));
            strText = ",,,,";
            strName = "Content,id,act,backurl";
            strType = "textarea,hidden,hidden,hidden";
            strValu = "" + model.Content + "'" + id + "'savesub'" + Utils.getPage(0) + "";
            strEmpt = "false,false,false,false";
            strIdea = "/";
            strOthe = "&gt;确定,function.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("function.aspx?act=admsub&amp;backurl=" + Utils.getPage(0) + "") + "\">管理短语</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("主题(30字内):");
            builder.Append(Out.Tab("</div>", ""));
            strText = ",内容(5000字内):/,,,";
            strName = "Title,Content,id,act,backurl";
            strType = "text,textarea,hidden,hidden,hidden";
            strValu = "" + model.Title + "'" + model.Content + "'" + id + "'savetemp'" + Utils.getPage(0) + "";
            strEmpt = "false,false,false,false,false";
            strIdea = "/";
            strOthe = "&gt;确定,function.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("function.aspx?act=admtemp&amp;backurl=" + Utils.getPage(0) + "") + "\">管理草稿</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private void AddSubPage(int uid)
    {
        Master.Title = "新建短语";
        builder.Append(Out.Tab("<div class=\"title\">新建短语</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("请输入不超300字的短语：");
        builder.Append(Out.Tab("</div>", ""));
        strText = ",,,";
        strName = "Content,act,backurl";
        strType = "textarea,hidden,hidden";
        strValu = "'savesub'" + Utils.getPage(0) + "";
        strEmpt = "false,false,false";
        strIdea = "/";
        strOthe = "&gt;确定,function.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("function.aspx?act=admsub&amp;backurl=" + Utils.getPage(0) + "") + "\">管理短语</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void SaveSubPage(string act, int uid)
    {
        string sName = "短语";
        string acturl = "sub";
        int itype = 0;
        string Title = string.Empty;
        string Content = string.Empty;
        int id = 0;
        if (act == "savetemp")
        {
            sName = "草稿";
            itype = 1;
            acturl = "temp";
            id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[1-9]\d*$", "ID错误"));
            Title = Utils.GetRequest("Title", "post", 2, @"^[^\^]{1,30}$", "请输入不超30字的标题");
            Content = Utils.GetRequest("Content", "post", 2, @"^[^\^]{1,5000}$", "请输入不超5000字的内容");
        }
        else
        {
            id = int.Parse(Utils.GetRequest("id", "post", 1, @"^[0-9]\d*$", "0"));
            Content = Utils.GetRequest("Content", "post", 2, @"^[^\^]{1,300}$", "请输入不超300字的短语");
        }
        BCW.Model.Submit model = new BCW.Model.Submit();
        model.ID = id;
        model.Types = itype;
        model.Title = Title;
        model.Content = Content;
        model.UsID = uid;
        model.AddTime = DateTime.Now;
        if (id != 0)
        {
            if (!new BCW.BLL.Submit().Exists(id, uid, itype))
            {
                Utils.Error("不存在的记录", "");
            }
            new BCW.BLL.Submit().Update(model);
            Utils.Success("编辑" + sName + "", "编辑" + sName + "成功，正在返回..", Utils.getUrl("function.aspx?act=view" + acturl + "&amp;ptype=1&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            new BCW.BLL.Submit().Add(model);
            Utils.Success("新建短语", "新建短语成功，正在返回..", Utils.getUrl("function.aspx?act=admsub&amp;ptype=1&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
    }

    private void DelSubPage(string act, int uid)
    {
        string sName = "短语";
        int itype = 0;
        string acturl = "sub";
        if (act == "deltemp")
        {
            sName = "草稿";
            itype = 1;
            acturl = "temp";
        }
        Master.Title = "查看短语";
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        string info = Utils.GetRequest("info", "get", 1, "", "");
        if (info != "ok")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此" + sName + "吗");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("function.aspx?act=del" + acturl + "&amp;info=ok&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("function.aspx?act=view" + acturl + "&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            if (!new BCW.BLL.Submit().Exists(id, uid, itype))
            {
                Utils.Error("不存在的记录", "");
            }
            new BCW.BLL.Submit().Delete(id);
            Utils.Success("删除" + sName + "", "删除" + sName + "成功，正在返回..", Utils.getPage("function.aspx?act=adm" + acturl + "&amp;backurl=" + Utils.getPage(0) + ""), "1");

        }
    }




}