using System.Collections.Generic;
using System;
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

public partial class bbs_friend : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string strText = string.Empty;
    protected string strName = string.Empty;
    protected string strType = string.Empty;
    protected string strValu = string.Empty;
    protected string strEmpt = string.Empty;
    protected string strIdea = string.Empty;
    protected string strOthe = string.Empty;
    protected int et = Convert.ToInt32(ub.Get("SiteExTime"));
    protected void Page_Load(object sender, EventArgs e)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "add":
                AddPage(meid);
                break;
            case "addok":
                AddokPage(meid);
                break;
            case "save":
                SavePage(meid);
                break;
            case "edit":
                EditPage(meid);
                break;
            case "editsave":
                EditSavePage(meid);
                break;
            case "addgroup":
                AddGroupPage(meid);
                break;
            case "groupsave":
                GroupSavePage(meid);
                break;
            case "editgroup":
                EditGroupPage(meid);
                break;
            case "editgroupsave":
                EditGroupSavePage(meid);
                break;
            case "once":
                OncePage(meid);
                break;
            case "list":
                ListPage(meid);
                break;
            case "black":
                BlackPage(meid);
                break;
            case "addblack":
                AddBlackPage(meid);
                break;
            case "addfans":
                AddFansPage(meid);
                break;
            case "delfans":
                DelFansPage(meid);
                break;
            case "fans":
                FansPage(meid);
                break;
            case "online":
                OnlinePage(meid);
                break;
            case "group":
                GroupPage(meid);
                break;
            case "del":
                DelPage(meid);
                break;
            case "delblack":
                DelBlackPage(meid);
                break;
            case "delgroup":
                DelGroupPage(meid);
                break;
            case "contact":
                ContactPage(meid);
                break;
            case "listct":
                ListctPage(meid);
                break;
            case "groupct":
                GroupctPage(meid);
                break;
            case "addgroupct":
                AddGroupctPage(meid);
                break;
            case "groupctsave":
                GroupctSavePage(meid);
                break;
            case "addct":
                AddctPage(meid);
                break;
            case "savect":
                SavectPage(meid);
                break;
            case "editct":
                EditctPage(meid);
                break;
            case "delct":
                DelctPage(meid);
                break;
            case "viewct":
                ViewctPage(meid);
                break;
            case "editgroupct":
                EditGroupctPage(meid);
                break;
            case "editgroupctsave":
                EditGroupctSavePage(meid);
                break;
            case "delgroupct":
                DelGroupctPage(meid);
                break;
            case "searchct":
                SearchctPage();
                break;
            case "searchctsave":
                SearchctSavePage(meid);
                break;
            default:
                ReloadPage(meid);
                break;
        }
    }

    private void ReloadPage(int uid)
    {
        Master.Title = "好友";

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=once&amp;backurl=" + Utils.PostPage(1) + "") + "\">最近联系人</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=black&amp;backurl=" + Utils.PostPage(1) + "") + "\">黑名单</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?act=friend&amp;backurl=" + Utils.PostPage(1) + "") + "\">设置</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        //在线好友
        DataSet ds = new BCW.BLL.Friend().GetList("TOP 8 FriendID,FriendName", "UsID=" + uid + " and Types=0 and (Select State from tb_User where id = FriendId)<>1 and (Select EndTime from tb_User where id = FriendId)>'" + DateTime.Now.AddMinutes(-et) + "' Order by AddTime Desc");
        if (ds != null && ds.Tables[0].Rows.Count != 0)
        {
            int k = 1;
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("-|在线好友");
            builder.Append(Out.Tab("</div>", "<br />"));
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + ds.Tables[0].Rows[i]["FriendID"].ToString() + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + ds.Tables[0].Rows[i]["FriendName"].ToString() + "</a>");
                builder.Append(Out.Tab("</div>", "<br />"));
                k++;
            }
            if (k > 1)
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=online&amp;&amp;backurl=" + Utils.getPage(0) + "") + "\">..更多</a>");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
        }
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("-|好友分组");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=list&amp;backurl=" + Utils.PostPage(1) + "") + "\">默认分组(" + new BCW.BLL.Friend().GetCount(uid, 0) + ")</a>");
        builder.Append(Out.Tab("</div>", ""));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "UsID=" + uid + " and Types=0";

        // 开始读取列表
        IList<BCW.Model.Frigroup> listFrigroup = new BCW.BLL.Frigroup().GetFrigroups(pageIndex, pageSize, strWhere, out recordCount);
        if (listFrigroup.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Frigroup n in listFrigroup)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div>", "<br />"));
                else
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));

                builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=list&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.Title + "(" + new BCW.BLL.Friend().GetCount(uid, n.ID) + ")</a>");
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 1));

        }

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("=好友搜索=");
        builder.Append(Out.Tab("</div>", ""));

        strText = ",,,,";
        strName = "keyword,pt,backurl";
        strType = "text,hidden,hidden";
        strValu = "'5_0'" + Utils.PostPage(1) + "";
        strEmpt = "false,false,false";
        strIdea = "/";
        strOthe = "搜ID|昵称,/search.aspx,post,1,red|blue";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=add&amp;backurl=" + Utils.PostPage(1) + "") + "\">添加好友</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=list&amp;ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">好友管理</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=group&amp;backurl=" + Utils.PostPage(1) + "") + "\">分组管理</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=contact&amp;backurl=" + Utils.PostPage(1) + "") + "\">通讯名片</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?backurl=" + Utils.PostPage(1) + "") + "\">信箱</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ListPage(int uid)
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-9]\d*$", "0"));
        int id = int.Parse(Utils.GetRequest("id", "get", 1, @"^[0-9]\d*$", "0"));
        string sText = string.Empty;
        if (id != 0)
        {
            sText = new BCW.BLL.Frigroup().GetTitle(id, uid, 0);
            if (sText == "")
            {
                Utils.Error("不存在的记录", "");
            }
        }
        else
        {
            sText = "我的好友";
        }
        Master.Title = sText;

        builder.Append(Out.Tab("<div class=\"title\">" + sText + "</div>", ""));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "ptype", "act", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "UsID=" + uid + " and NodeId=" + id + " and Types=0";

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

                builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.FriendID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + ((pageIndex - 1) * pageSize + k) + "." + ForFriendName(n.FriendID, n.FriendName) + "</a>");
                if (ptype == 1)
                {
                    builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=edit&amp;hid=" + n.FriendID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[编辑]</a>");
                    builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=del&amp;hid=" + n.FriendID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[删除]</a>");
                }
                if (n.AddTime != null)
                    builder.Append("<br />最近联系时间:" + DT.FormatDate(Convert.ToDateTime(n.AddTime), 5) + "");
                else
                    builder.Append("<br />暂未有联系");

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
        if (ptype == 0)
            builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=list&amp;ptype=1&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;切换管理</a>");
        else
            builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=list&amp;ptype=0&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;切换普通</a>");

        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("friend.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void BlackPage(int uid)
    {
        Master.Title = "我的黑名单";

        builder.Append(Out.Tab("<div class=\"title\">我的黑名单</div>", ""));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "UsID=" + uid + " and Types=1";

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
                builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.FriendID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + ((pageIndex - 1) * pageSize + k) + "." + ForFriendName(n.FriendID, n.FriendName) + "</a>");
                builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=delblack&amp;hid=" + n.FriendID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[解除]</a>");

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
        builder.Append("输入ID：");
        builder.Append(Out.Tab("</div>", ""));

        strText = ",,,";
        strName = "hid,act,backurl";
        strType = "num,hidden,hidden";
        strValu = "'addblack'" + Utils.PostPage(1) + "";
        strEmpt = "false,false,false";
        strIdea = "/";
        strOthe = "立即加黑,friend.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("friend.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void FansPage(int uid)
    {
        Master.Title = "我关注的友友";

        builder.Append(Out.Tab("<div class=\"title\">我关注的友友</div>", ""));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "UsID=" + uid + " and Types=2";

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
                builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.FriendID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + ((pageIndex - 1) * pageSize + k) + "." + ForFriendName(n.FriendID, n.FriendName) + "</a>");
                builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=delfans&amp;hid=" + n.FriendID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[删除]</a>");

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
        builder.Append("<a href=\"" + Utils.getPage("visit.aspx?act=list&amp;ptype=4") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("friend.aspx") + "\">我的好友</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void OncePage(int uid)
    {
        Master.Title = "最近联系人";

        builder.Append(Out.Tab("<div class=\"title\">最近联系人</div>", ""));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "UsID=" + uid + " and Types=0 and AddTime> '" + DateTime.Now.AddDays(-15) + "'";//限两天内为最近联系人

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
                builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.FriendID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + ((pageIndex - 1) * pageSize + k) + "." + ForFriendName(n.FriendID, n.FriendName) + "</a>");
                builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=add&amp;hid=" + n.FriendID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[密TA]</a>");

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
        builder.Append("<a href=\"" + Utils.getPage("friend.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void OnlinePage(int uid)
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-9]\d*$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        if (ptype == 0)
        {
            Master.Title = "在线好友";
            builder.Append("在线好友|");
            builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=online&amp;ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">全部好友</a>");
        }
        else
        {
            Master.Title = "全部好友";
            builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=online&amp;ptype=0&amp;backurl=" + Utils.getPage(0) + "") + "\">在线好友</a>");
            builder.Append("|全部好友");
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "ptype", "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        if (ptype == 0)
            strWhere = "UsID=" + uid + " and Types=0 and (Select State from tb_User where id = FriendId)<>1 and (Select EndTime from tb_User where id = FriendId)>'" + DateTime.Now.AddMinutes(-et) + "'";
        else
            strWhere = "UsID=" + uid + " and Types=0";

        bool blUrl = false;
        string gUrl = Server.UrlDecode(Utils.getPage(0));
        if (gUrl.Contains("guest.aspx") || gUrl.Contains("bbsshop.aspx") || gUrl.Contains("brag.aspx") || gUrl.Contains("mora.aspx") || gUrl.Contains("dxdice.aspx"))
        {
            gUrl = gUrl.Replace("&amp;", "&");
            gUrl = Regex.Replace(gUrl, @"([\s\S]+?)[&|?]{0,1}hid=[^&]*&{0,}", @"$1&amp;", RegexOptions.IgnoreCase);
            gUrl = Out.UBB(gUrl);
            blUrl = true;
        }
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

                builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.FriendID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + ((pageIndex - 1) * pageSize + k) + "." + sFriendName + "</a>");
                if (blUrl)
                {
                    builder.Append("<a href=\"" + Utils.getUrl(gUrl + "&amp;hid=" + n.FriendID + "") + "\">[选择]</a>");
                }
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
        if (blUrl)
        {
            builder.Append(Out.Tab("", "<br />"));
            string strText = "或输入ID:(限一个):/";
            string strName = "hid";
            string strType = "snum";
            string strValu = "'";
            if (gUrl.Contains("bbsshop.aspx"))
            {
                if (HttpContext.Current.Request.Cookies["GiftComment"] != null)
                {
                    if (HttpContext.Current.Request.Cookies["GiftComment"]["GiftUsId"] != null)
                    {
                        strValu = HttpContext.Current.Request.Cookies["GiftComment"]["GiftUsId"].ToString() + "'";
                    }
                }
            }

            string strEmpt = "true";
            string strIdea = "/";
            string strOthe = "提交," + gUrl + ",post,3,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("friend.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void AddPage(int uid)
    {
        Master.Title = "添加好友";
        int frid = int.Parse(Utils.GetRequest("hid", "get", 1, @"^[1-9]\d*$", "0"));
        int NC_id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[0-9]\d*$", "0"));//邵广林 20160525 增加农场跳转id为1
        if (frid != 0)
        {
            if (!new BCW.BLL.User().Exists(frid))
            {
                Utils.Error("不存在的会员ID", "");
            }
        }
        string strFrigroup = string.Empty;
        DataSet ds = new BCW.BLL.Frigroup().GetList("ID,Title", "UsID=" + uid + " and Types=0");
        if (ds != null && ds.Tables[0].Rows.Count != 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                strFrigroup += "|" + ds.Tables[0].Rows[i]["ID"] + "|" + ds.Tables[0].Rows[i]["Title"] + "";
            }
        }
        strFrigroup = "0|默认分组" + strFrigroup;

        builder.Append(Out.Tab("<div class=\"title\">添加好友</div>", ""));

        builder.Append(Out.Tab("<div>", ""));
        string sType = string.Empty;
        if (frid == 0)
        {
            builder.Append("请输入好友ID:");
            sType = "num";
        }
        else
        {
            builder.Append("好友ID:" + frid + "");
            sType = "hidden";
        }
        builder.Append(Out.Tab("</div>", ""));
        strText = ",请选择好友分组:/,,,";
        strName = "frid,NodeId,act,backurl,id";
        strType = "" + sType + ",select,hidden,hidden,hidden";
        strValu = "" + frid + "'0'save'" + Utils.getPage(0) + "'" + NC_id + "";//农场ID为1
        strEmpt = "false," + strFrigroup + ",false,false,false";
        strIdea = "/";
        strOthe = "添加好友,friend.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("friend.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void AddokPage(int uid)
    {
        int frid = int.Parse(Utils.GetRequest("hid", "get", 2, @"^[1-9]\d*$", "好友ID错误"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 2, @"^[1-2]$", "选择类型错误"));
        if (!new BCW.BLL.User().Exists(frid))
        {
            Utils.Error("不存在的会员ID", "");
        }
        if (new BCW.BLL.Friend().Exists(frid, uid, 1))
        {
            Utils.Error("对方已把你加入黑名单", "");
        }
        if (!new BCW.BLL.Friend().Exists(frid, uid, 9))
        {
            Utils.Error("不存在的请求", "");
        }
        if (ptype == 1)
        {
            new BCW.BLL.Friend().UpdateTypes(frid, uid);//更新为正式好友
            new BCW.BLL.Guest().Add(frid, new BCW.BLL.User().GetUsName(frid), "[url=/bbs/uinfo.aspx?uid=" + uid + "]" + new BCW.BLL.User().GetUsName(uid) + "[/url]接受了您的加好友请求.");
            Utils.Success("接受好友请求", "接受好友请求成功，正在返回", Utils.getPage("friend.aspx?backurl=" + Utils.getPage(0) + ""), "3");
        }
        else
        {
            new BCW.BLL.Friend().Delete(frid, uid, 9);//拒绝则删除好友
            new BCW.BLL.Guest().Add(frid, new BCW.BLL.User().GetUsName(frid), "[url=/bbs/uinfo.aspx?uid=" + uid + "]" + new BCW.BLL.User().GetUsName(uid) + "[/url]拒绝了您的加好友请求.");
            Utils.Success("拒绝好友请求", "拒绝好友请求成功，正在返回", Utils.getPage("friend.aspx?backurl=" + Utils.getPage(0) + ""), "3");
        }

    }

    private void SavePage(int uid)
    {
        int frid = int.Parse(Utils.GetRequest("frid", "all", 2, @"^[1-9]\d*$", "好友ID错误"));
        int NodeId = int.Parse(Utils.GetRequest("NodeId", "post", 1, @"^[0-9]\d*$", "0"));
        int NC_id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[0-9]\d*$", "0"));//邵广林 20160525 增加农场跳转id为1
        if (frid.Equals(uid))
        {
            Utils.Error("不能加自己为好友", "");
        }
        if (!new BCW.BLL.User().Exists(frid))
        {
            Utils.Error("不存在的会员ID", "");
        }
        if (new BCW.BLL.Friend().Exists(frid, uid, 1))
        {
            Utils.Error("对方已把你加入黑名单", "");
        }
        if (new BCW.BLL.Friend().Exists(uid, frid, 0))
        {
            Utils.Error("此ID已是你的好友", "");
        }
        if (NodeId != 0)
        {
            if (!new BCW.BLL.Frigroup().Exists(NodeId, uid, 0))
            {
                Utils.Error("不存在的记录", "");
            }
        }
        int iTypes = 0;
        string Msg = "添加好友成功，正在返回..";
        //对方是否存在已加自己
        bool IsFriend = new BCW.BLL.Friend().Exists(frid, uid, 9);
        if (!IsFriend)
        {
            string ForumSet = new BCW.BLL.User().GetForumSet(frid);
            int friSet = BCW.User.Users.GetForumSet(ForumSet, 11);
            //0|允许任何人|1|不允许任何人|2|需要验证才允许|3|只有VIP才能
            if (friSet == 1)
            {
                Utils.Error("对方拒绝加TA为好友", "");
            }
            else if (friSet == 2)
            {
                Msg = "需要通过对方验证，请求已经发出，请耐心等待，正在返回..";
                iTypes = 9;
                if (!new BCW.BLL.Friend().Exists(uid, frid, 9))
                {
                    if (!new BCW.BLL.Friend().Exists(frid, uid, 0))
                        new BCW.BLL.Guest().Add(frid, new BCW.BLL.User().GetUsName(frid), "[url=/bbs/uinfo.aspx?uid=" + uid + "]" + new BCW.BLL.User().GetUsName(uid) + "[/url]申请加您为Ta的好友.[br]您可以接受并[url=/bbs/friend.aspx?act=save&amp;frid=" + uid + "]加Ta为好友[/url],[url=/bbs/friend.aspx?act=addok&amp;hid=" + uid + "&amp;ptype=1]只接受[/url]|[url=/bbs/friend.aspx?act=addok&amp;hid=" + uid + "&amp;ptype=2]拒绝Ta[/url]");
                    else
                        new BCW.BLL.Guest().Add(frid, new BCW.BLL.User().GetUsName(frid), "[url=/bbs/uinfo.aspx?uid=" + uid + "]" + new BCW.BLL.User().GetUsName(uid) + "[/url]申请加您为Ta的好友.[br][url=/bbs/friend.aspx?act=addok&amp;hid=" + uid + "&amp;ptype=1]接受申请[/url]|[url=/bbs/friend.aspx?act=addok&amp;hid=" + uid + "&amp;ptype=2]拒绝Ta[/url]");
                }
                else
                {
                    iTypes = -1;
                }
            }
            else if (friSet == 3)
            {
                int isvip = BCW.User.Users.VipLeven(uid);
                if (isvip == 0)
                {
                    Utils.Error("对方限VIP会员才可以添加TA为好友", "");
                }
            }
            if (friSet != 2)
            {
                if (!new BCW.BLL.Friend().Exists(frid, uid, 0))
                {
                    new BCW.BLL.Guest().Add(frid, new BCW.BLL.User().GetUsName(frid), "[url=/bbs/uinfo.aspx?uid=" + uid + "]" + new BCW.BLL.User().GetUsName(uid) + "[/url]加您为好友.您也可以[url=/bbs/friend.aspx?act=add&amp;hid=" + uid + "&amp;ptype=1]加Ta为好友[/url]");
                }
                else
                {
                    new BCW.BLL.Guest().Add(frid, new BCW.BLL.User().GetUsName(frid), "[url=/bbs/uinfo.aspx?uid=" + uid + "]" + new BCW.BLL.User().GetUsName(uid) + "[/url]加您为好友.");
                }
            }
        }
        else
        {
            new BCW.BLL.Friend().UpdateTypes(frid, uid);//更新为正式好友
            new BCW.BLL.Guest().Add(frid, new BCW.BLL.User().GetUsName(frid), "[url=/bbs/uinfo.aspx?uid=" + uid + "]" + new BCW.BLL.User().GetUsName(uid) + "[/url]接受了您的加好友请求并加您为好友.");
            Msg = "接受并添加好友成功，正在返回..";
        }
        if (iTypes != -1)
        {
            BCW.Model.Friend model = new BCW.Model.Friend();
            model.Types = iTypes;
            model.NodeId = NodeId;
            model.UsID = uid;
            model.FriendID = frid;
            model.UsName = new BCW.BLL.User().GetUsName(uid);
            model.FriendName = new BCW.BLL.User().GetUsName(frid);
            new BCW.BLL.Friend().Add(model);
        }
        if (NC_id == 1)//邵广林 农场跳转 20160525
        {
            Utils.Success("添加好友", "" + Msg + "<br /><a href=\"" + Utils.getUrl("friend.aspx") + "\">&gt;回我的好友</a>", Utils.getUrl("/bbs/game/farm.aspx?act=do&amp;uid=" + frid + ""), "2");
        }
        else
        {
            Utils.Success("添加好友", "" + Msg + "<br /><a href=\"" + Utils.getUrl("friend.aspx") + "\">&gt;回我的好友</a>", Utils.getPage("friend.aspx?backurl=" + Utils.getPage(0) + ""), "3");
        }
    }

    private void EditPage(int uid)
    {
        Master.Title = "编辑好友";
        int frid = int.Parse(Utils.GetRequest("hid", "get", 2, @"^[1-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Friend().Exists(uid, frid, 0))
        {
            Utils.Error("不存在的记录", "");
        }
        string strFrigroup = string.Empty;
        DataSet ds = new BCW.BLL.Frigroup().GetList("ID,Title", "UsID=" + uid + " and Types=0");
        if (ds != null && ds.Tables[0].Rows.Count != 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                strFrigroup += "|" + ds.Tables[0].Rows[i]["ID"] + "|" + ds.Tables[0].Rows[i]["Title"] + "";
            }
        }
        strFrigroup = "0|默认分组" + strFrigroup;

        BCW.Model.Friend model = new BCW.BLL.Friend().GetFriend(frid);
        builder.Append(Out.Tab("<div class=\"title\">编辑好友</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("好友备注名称:");
        builder.Append(Out.Tab("</div>", ""));

        strText = ",请选择好友分组:/,,,,";
        strName = "Name,NodeId,frid,act,backurl";
        strType = "text,select,hidden,hidden,hidden";
        strValu = "" + model.FriendName + "'" + model.NodeId + "'" + frid + "'editsave'" + Utils.getPage(0) + "";
        strEmpt = "false," + strFrigroup + ",false,false,false";
        strIdea = "/";
        strOthe = "编辑好友,friend.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("friend.aspx") + "\">取消</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void EditSavePage(int uid)
    {
        string Name = Utils.GetRequest("Name", "post", 2, @"^[A-Za-zＡ-Ｚ\d０-９\u4E00-\u9FA5]{1,10}$", "备注名称限1-10字，不能使用特殊字符");
        int frid = int.Parse(Utils.GetRequest("frid", "post", 2, @"^[1-9]\d*$", "ID错误"));
        int NodeId = int.Parse(Utils.GetRequest("NodeId", "post", 1, @"^[0-9]\d*$", "0"));

        if (!new BCW.BLL.Friend().Exists(uid, frid, 0))
        {
            Utils.Error("不存在的记录", "");
        }
        if (NodeId != 0)
        {
            if (!new BCW.BLL.Frigroup().Exists(NodeId, uid, 0))
            {
                Utils.Error("不存在的分组记录", "");
            }
        }

        BCW.Model.Friend model = new BCW.Model.Friend();
        model.Types = 0;
        model.NodeId = NodeId;
        model.UsID = uid;
        model.FriendID = frid;
        model.UsName = new BCW.BLL.User().GetUsName(uid);
        model.FriendName = Name;
        new BCW.BLL.Friend().Update(model);

        Utils.Success("编辑好友", "编辑好友成功，正在返回..", Utils.getUrl("friend.aspx?act=list&amp;ptype=1&amp;id=" + NodeId + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
    }

    private void GroupPage(int uid)
    {
        Master.Title = "好友分组管理";
        builder.Append(Out.Tab("<div class=\"title\">好友分组管理</div>", ""));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=addgroup&amp;backurl=" + Utils.PostPage(1) + "") + "\">新建分组</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=list&amp;backurl=" + Utils.PostPage(1) + "") + "\">默认分组(" + new BCW.BLL.Friend().GetCount(uid, 0) + ")</a>");
        builder.Append(Out.Tab("</div>", ""));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "UsID=" + uid + " and Types=0";

        // 开始读取列表
        IList<BCW.Model.Frigroup> listFrigroup = new BCW.BLL.Frigroup().GetFrigroups(pageIndex, pageSize, strWhere, out recordCount);
        if (listFrigroup.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Frigroup n in listFrigroup)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                    builder.Append(Out.Tab("<div>", "<br />"));

                builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=list&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.Title + "(" + new BCW.BLL.Friend().GetCount(uid, n.ID) + ")</a>");
                builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=editgroup&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[编辑]</a>");
                builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=delgroup&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[删除]</a>");

                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 1));

        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("friend.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void AddGroupPage(int uid)
    {
        Master.Title = "添加好友分组";

        builder.Append(Out.Tab("<div class=\"title\">添加好友分组</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("请输入分组名称:");
        builder.Append(Out.Tab("</div>", ""));
        strText = ",排序:/,,,";
        strName = "Title,Paixu,act,backurl";
        strType = "text,snum,hidden,hidden";
        strValu = "'0'groupsave'" + Utils.getPage(0) + "";
        strEmpt = "false,false,false,false";
        strIdea = "/";
        strOthe = "添加分组,friend.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("friend.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void GroupSavePage(int uid)
    {
        string Title = Utils.GetRequest("Title", "post", 2, @"^[A-Za-zＡ-Ｚ\d０-９\u4E00-\u9FA5]{1,10}$", "分组名称限1-10字，不能使用特殊字符");
        int Paixu = int.Parse(Utils.GetRequest("Paixu", "post", 2, @"^[0-9]\d*$", "排序必须是数字形式"));

        if (new BCW.BLL.Frigroup().ExistsTitle(uid, Title, 0))
        {
            Utils.Error("此分组名称已存在", "");
        }
        BCW.Model.Frigroup model = new BCW.Model.Frigroup();
        model.Types = 0;
        model.Title = Title;
        model.UsID = uid;
        model.Paixu = Paixu;
        model.AddTime = DateTime.Now;
        new BCW.BLL.Frigroup().Add(model);

        Utils.Success("添加好友分组", "添加好友分组成功，正在返回..", Utils.getUrl("friend.aspx?act=group&amp;backurl=" + Utils.getPage(0) + ""), "1");
    }

    private void EditGroupPage(int uid)
    {
        Master.Title = "编辑好友分组";
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "分组ID错误"));
        if (!new BCW.BLL.Frigroup().Exists(id, uid, 0))
        {
            Utils.Error("不存在的记录", "");
        }
        BCW.Model.Frigroup model = new BCW.BLL.Frigroup().GetFrigroup(id);
        builder.Append(Out.Tab("<div class=\"title\">编辑好友分组</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("请输入分组名称:");
        builder.Append(Out.Tab("</div>", ""));
        strText = ",排序:/,,,";
        strName = "Title,Paixu,id,act,backurl";
        strType = "text,snum,hidden,hidden,hidden";
        strValu = "" + model.Title + "'" + model.Paixu + "'" + id + "'editgroupsave'" + Utils.getPage(0) + "";
        strEmpt = "false,false,false,false,false";
        strIdea = "/";
        strOthe = "编辑分组,friend.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("friend.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void EditGroupSavePage(int uid)
    {
        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[0-9]\d*$", "ID错误"));
        string Title = Utils.GetRequest("Title", "post", 2, @"^[A-Za-zＡ-Ｚ\d０-９\u4E00-\u9FA5]{1,10}$", "分组名称限1-10字，不能使用特殊字符");
        int Paixu = int.Parse(Utils.GetRequest("Paixu", "post", 2, @"^[0-9]\d*$", "排序必须是数字形式"));
        if (!new BCW.BLL.Frigroup().Exists(id, uid, 0))
        {
            Utils.Error("不存在的记录", "");
        }
        BCW.Model.Frigroup model = new BCW.Model.Frigroup();
        model.ID = id;
        model.Types = 0;
        model.Title = Title;
        model.UsID = uid;
        model.Paixu = Paixu;
        model.AddTime = DateTime.Now;
        new BCW.BLL.Frigroup().Update(model);

        Utils.Success("编辑好友分组", "编辑好友分组成功，正在返回..", Utils.getUrl("friend.aspx?act=group&amp;backurl=" + Utils.getPage(0) + ""), "1");
    }

    private void DelGroupPage(int uid)
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Frigroup().Exists(id, uid, 0))
        {
            Utils.Error("不存在的记录", "");
        }
        string info = Utils.GetRequest("info", "get", 1, "", "");

        Master.Title = "删除分组";

        if (info == "")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此分组吗.");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=delgroup&amp;info=ok1&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除一(保留分组里的好友)</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=delgroup&amp;info=ok2&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除二(分组里的好友删除)</a><br />");

            builder.Append("<a href=\"" + Utils.getPage("friend.aspx?act=group") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("温馨提示：选择删除一，本组好友将移到[默认分组].<br />");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            new BCW.BLL.Frigroup().Delete(id);
            if (info == "ok1")
                new BCW.BLL.Friend().UpdateNodeId(uid, id);
            else
                new BCW.BLL.Friend().Delete(uid, id);

            Utils.Success("删除分组", "删除分组成功，正在返回..", Utils.getUrl("friend.aspx?act=group&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
    }

    private void DelPage(int uid)
    {
        int hid = int.Parse(Utils.GetRequest("hid", "get", 2, @"^[1-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Friend().Exists(uid, hid, 0))
        {
            Utils.Error("不存在的记录", "");
        }
        string info = Utils.GetRequest("info", "get", 1, "", "");

        Master.Title = "删除好友";

        if (info == "")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此好友吗.");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=del&amp;info=ok&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");

            builder.Append("<a href=\"" + Utils.getPage("friend.aspx") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            new BCW.BLL.Friend().Delete(uid, hid, 0);

            Utils.Success("删除好友", "删除好友成功，正在返回..", Utils.getPage("friend.aspx"), "1");
        }
    }

    private void DelBlackPage(int uid)
    {
        int hid = int.Parse(Utils.GetRequest("hid", "get", 2, @"^[1-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Friend().Exists(uid, hid, 1))
        {
            Utils.Error("不存在的记录", "");
        }
        string info = Utils.GetRequest("info", "get", 1, "", "");

        Master.Title = "解除黑名单";

        if (info == "")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定解除此黑名单吗.");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=delblack&amp;info=ok&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定解除</a><br />");

            builder.Append("<a href=\"" + Utils.getPage("friend.aspx?act=black") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            new BCW.BLL.Friend().Delete(uid, hid, 1);

            Utils.Success("解除黑名单", "解除黑名单成功，正在返回..", Utils.getUrl("friend.aspx?act=black"), "1");
        }
    }

    private void DelFansPage(int uid)
    {
        int hid = int.Parse(Utils.GetRequest("hid", "get", 2, @"^[1-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Friend().Exists(uid, hid, 2))
        {
            Utils.Error("不存在的记录", "");
        }
        string info = Utils.GetRequest("info", "get", 1, "", "");

        Master.Title = "删除关注";

        if (info == "")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此关注吗.");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=delfans&amp;info=ok&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");

            builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=fans&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            new BCW.BLL.Friend().Delete(uid, hid, 2);

            Utils.Success("删除关注", "删除关注成功，正在返回..", Utils.getUrl("friend.aspx?act=fans"), "1");
        }
    }

    //通讯录
    private void ContactPage(int uid)
    {
        Master.Title = "通讯录";

        builder.Append(Out.Tab("<div class=\"title\">我的通讯录</div>", ""));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "UsID=" + uid + " and Types=1";

        // 开始读取列表
        IList<BCW.Model.Frigroup> listFrigroup = new BCW.BLL.Frigroup().GetFrigroups(pageIndex, pageSize, strWhere, out recordCount);
        if (listFrigroup.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Frigroup n in listFrigroup)
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

                builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=listct&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.Title + "(" + new BCW.BLL.Contact().GetCount(uid, n.ID) + ")</a>");
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 1));

        }
        else
        {
            builder.Append(Out.Div("div", "还没有添加分组.."));
        }

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=searchct&amp;backurl=" + Utils.getPage(0) + "") + "\">查联系人</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=groupct&amp;backurl=" + Utils.getPage(0) + "") + "\">分组管理</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("friend.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("friend.aspx") + "\">我的好友</a>");
        builder.Append(Out.Tab("</div>", ""));

    }

    private void GroupctPage(int uid)
    {
        Master.Title = "通讯录分组管理";
        builder.Append(Out.Tab("<div class=\"title\">通讯录分组管理</div>", ""));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=addgroupct&amp;backurl=" + Utils.PostPage(1) + "") + "\">新建分组</a>");
        builder.Append(Out.Tab("</div>", ""));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "UsID=" + uid + " and Types=1";

        // 开始读取列表
        IList<BCW.Model.Frigroup> listFrigroup = new BCW.BLL.Frigroup().GetFrigroups(pageIndex, pageSize, strWhere, out recordCount);
        if (listFrigroup.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Frigroup n in listFrigroup)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                    builder.Append(Out.Tab("<div>", "<br />"));

                builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=listct&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.Title + "(" + new BCW.BLL.Contact().GetCount(uid, n.ID) + ")</a>");
                builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=editgroupct&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[编辑]</a>");
                builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=delgroupct&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[删除]</a>");

                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 1));
        }
        else
        {
            builder.Append(Out.Tab("", "<br />"));
            builder.Append(Out.Div("div", "还没有添加分组.."));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("friend.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=contact&amp;backurl=" + Utils.getPage(01) + "") + "\">通讯录</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void AddGroupctPage(int uid)
    {
        Master.Title = "添加通讯分组";

        builder.Append(Out.Tab("<div class=\"title\">添加通讯分组</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("请输入通讯名称:");
        builder.Append(Out.Tab("</div>", ""));
        strText = ",排序:/,,,";
        strName = "Title,Paixu,act,backurl";
        strType = "text,snum,hidden,hidden";
        strValu = "'0'groupctsave'" + Utils.getPage(0) + "";
        strEmpt = "false,false,false,false";
        strIdea = "/";
        strOthe = "添加分组,friend.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(" <a href=\"" + Utils.getUrl("friend.aspx?act=groupct&amp;backurl=" + Utils.getPage(0) + "") + "\">取消</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void GroupctSavePage(int uid)
    {
        string Title = Utils.GetRequest("Title", "post", 2, @"^[A-Za-zＡ-Ｚ\d０-９\u4E00-\u9FA5]{1,10}$", "分组名称限1-10字，不能使用特殊字符");
        int Paixu = int.Parse(Utils.GetRequest("Paixu", "post", 2, @"^[0-9]\d*$", "排序必须是数字形式"));

        if (new BCW.BLL.Frigroup().ExistsTitle(uid, Title, 1))
        {
            Utils.Error("此分组名称已存在", "");
        }
        BCW.Model.Frigroup model = new BCW.Model.Frigroup();
        model.Types = 1;
        model.Title = Title;
        model.UsID = uid;
        model.Paixu = Paixu;
        model.AddTime = DateTime.Now;
        new BCW.BLL.Frigroup().Add(model);

        Utils.Success("添加通讯分组", "添加通讯分组成功，正在返回..", Utils.getUrl("friend.aspx?act=groupct&amp;backurl=" + Utils.getPage(0) + ""), "1");
    }

    private void ListctPage(int uid)
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        string sText = new BCW.BLL.Frigroup().GetTitle(id, uid, 1);
        if (sText == "")
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = sText;

        builder.Append(Out.Tab("<div class=\"title\">" + sText + "</div>", ""));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "UsID=" + uid + " and NodeId=" + id + "";

        // 开始读取列表
        IList<BCW.Model.Contact> listContact = new BCW.BLL.Contact().GetContacts(pageIndex, pageSize, strWhere, out recordCount);
        if (listContact.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Contact n in listContact)
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
                builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=viewct&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + ((pageIndex - 1) * pageSize + k) + "." + n.Name + "</a>");
                builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=delct&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[删除]</a>");


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
        builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=addct&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">添加联系人</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("friend.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=contact&amp;backurl=" + Utils.getPage(0) + "") + "\">通讯录</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void AddctPage(int uid)
    {
        Master.Title = "添加联系人";
        int id = int.Parse(Utils.GetRequest("id", "get", 1, @"^[1-9]\d*$", "0"));
        if (!new BCW.BLL.Frigroup().Exists(id, uid, 1))
        {
            Utils.Error("不存在的记录", "");
        }
        builder.Append(Out.Tab("<div class=\"title\">添加联系人</div>", ""));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("姓名(*):");
        builder.Append(Out.Tab("</div>", ""));

        strText = ",手机:/,家庭电话:/,办公电话:/,传真:/,电子邮件:/,公司:/,职位:/,备注信息:/,,,";
        strName = "Name,Mobile,HomePhone,OfficePhone,Fax,Email,Company,Posit,Content,NodeId,act,backurl";
        strType = "text,text,text,text,text,text,text,text,textarea,hidden,hidden,hidden";
        strValu = "'''''''''" + id + "'savect'" + Utils.getPage(0) + "";
        strEmpt = "false,false,false,false,false,false,false,false,false,false,false,false,false";
        strIdea = "/";
        strOthe = "确定添加,friend.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append(" <a href=\"" + Utils.getPage("friend.aspx?act=contact") + "\">取消</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void SavectPage(int uid)
    {
        int id = int.Parse(Utils.GetRequest("id", "post", 1, @"^[0-9]\d*$", "0"));
        if (id != 0)
        {
            if (!new BCW.BLL.Contact().Exists(id, uid))
            {
                Utils.Error("不存在的记录", "");
            }
        }
        int NodeId = int.Parse(Utils.GetRequest("NodeId", "post", 1, @"^[0-9]\d*$", "ID错误"));
        string Name = Utils.GetRequest("Name", "post", 2, @"^[A-Za-zＡ-Ｚ\d０-９\u4E00-\u9FA5]{1,10}$", "姓名限10字，不能使用特殊字符");
        string Mobile = Utils.GetRequest("Mobile", "post", 3, @"^(?:13|15|18)\d{9}$", "请正确输入十一位数的手机号码");
        string HomePhone = Utils.GetRequest("HomePhone", "post", 3, @"^(\d{3}-|\d{4}-)?(\d{8}|\d{7})?(-\d+)?$", "请正确输入家庭电话");
        string OfficePhone = Utils.GetRequest("OfficePhone", "post", 3, @"^(\d{3}-|\d{4}-)?(\d{8}|\d{7})?(-\d+)?$", "请正确输入办公电话");
        string Fax = Utils.GetRequest("Fax", "post", 3, @"^[^\^]{1,50}$", "请正确输入传真");
        string Email = Utils.GetRequest("Email", "post", 3, @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", "请正确输入电子邮件");
        string Company = Utils.GetRequest("Company", "post", 3, @"^[^\^]{1,50}$", "请正确输入公司");
        string Posit = Utils.GetRequest("Posit", "post", 3, @"^[^\^]{1,50}$", "请正确输入职位");
        string Content = Utils.GetRequest("Content", "post", 3, @"^[^\^]{1,200}$", "备注限200字内");

        if (!new BCW.BLL.Frigroup().Exists(NodeId, uid, 1))
        {
            Utils.Error("不存在的记录", "");
        }

        BCW.Model.Contact model = new BCW.Model.Contact();
        model.ID = id;
        model.NodeId = NodeId;
        model.UsID = uid;
        model.Name = Name;
        model.Mobile = Mobile;
        model.HomePhone = HomePhone;
        model.OfficePhone = OfficePhone;
        model.Fax = Fax;
        model.Email = Email;
        model.Company = Company;
        model.Posit = Posit;
        model.Content = Content;
        if (id == 0)
        {
            new BCW.BLL.Contact().Add(model);
            Utils.Success("添加联系人", "添加联系人成功，正在返回..", Utils.getUrl("friend.aspx?act=listct&amp;id=" + NodeId + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            new BCW.BLL.Contact().Update(model);
            Utils.Success("修改联系人", "修改联系人成功，正在返回..", Utils.getUrl("friend.aspx?act=viewct&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
    }

    private void DelctPage(int uid)
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Contact().Exists(id, uid))
        {
            Utils.Error("不存在的记录", "");
        }
        string info = Utils.GetRequest("info", "get", 1, "", "");

        Master.Title = "删除联系人";

        if (info == "")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此联系人吗.");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=delct&amp;info=ok&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");

            builder.Append("<a href=\"" + Utils.getPage("friend.aspx?act=contact") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            int NodeId = new BCW.BLL.Contact().GetNodeId(id);
            new BCW.BLL.Contact().Delete(id);
            Utils.Success("删除联系人", "删除联系人成功，正在返回..", Utils.getUrl("friend.aspx?act=listct&amp;id=" + NodeId + ""), "1");
        }
    }

    private void ViewctPage(int uid)
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Contact().Exists(id, uid))
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "查看联系人";
        BCW.Model.Contact model = new BCW.BLL.Contact().GetContact(id);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("查看联系人");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("姓名:" + model.Name + "<br />");
        builder.Append("手机:" + model.Mobile + "<br />");
        builder.Append("家庭电话:" + model.HomePhone + "<br />");
        builder.Append("办公电话:" + model.OfficePhone + "<br />");
        builder.Append("传真:" + model.Fax + "<br />");
        builder.Append("电子邮件:" + model.Email + "<br />");
        builder.Append("公司:" + model.Company + "<br />");
        builder.Append("职位:" + model.Posit + "<br />");
        builder.Append("备注:" + model.Content + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=editct&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">编辑联系人</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=delct&amp;id=" + id + "") + "\">删除联系人</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("friend.aspx?act=listct&amp;id=" + model.NodeId + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=contact&amp;backurl=" + Utils.getPage(0) + "") + "\">通讯录</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void EditctPage(int uid)
    {
        Master.Title = "修改联系人";
        int id = int.Parse(Utils.GetRequest("id", "get", 1, @"^[1-9]\d*$", "0"));
        if (!new BCW.BLL.Contact().Exists(id, uid))
        {
            Utils.Error("不存在的记录", "");
        }
        string strFrigroup = string.Empty;
        DataSet ds = new BCW.BLL.Frigroup().GetList("ID,Title", "UsID=" + uid + " and Types=1");
        if (ds != null && ds.Tables[0].Rows.Count != 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                strFrigroup += "|" + ds.Tables[0].Rows[i]["ID"] + "|" + ds.Tables[0].Rows[i]["Title"] + "";
            }
        }
        strFrigroup = Utils.Mid(strFrigroup, 1, strFrigroup.Length);
        BCW.Model.Contact model = new BCW.BLL.Contact().GetContact(id);
        builder.Append(Out.Tab("<div class=\"title\">修改联系人</div>", ""));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("姓名(*):");
        builder.Append(Out.Tab("</div>", ""));

        strText = ",手机:/,家庭电话:/,办公电话:/,传真:/,电子邮件:/,公司:/,职位:/,备注信息:/,分组:/,,,,";
        strName = "Name,Mobile,HomePhone,OfficePhone,Fax,Email,Company,Posit,Content,NodeId,id,act,backurl";
        strType = "text,text,text,text,text,text,text,text,textarea,select,hidden,hidden,hidden";
        strValu = "" + model.Name + "'" + model.Mobile + "'" + model.HomePhone + "'" + model.OfficePhone + "'" + model.Fax + "'" + model.Email + "'" + model.Company + "'" + model.Posit + "'" + model.Content + "'" + model.NodeId + "'" + id + "'savect'" + Utils.getPage(0) + "";
        strEmpt = "false,false,false,false,false,false,false,false,false," + strFrigroup + ",false,false,false";
        strIdea = "/";
        strOthe = "确定修改,friend.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("friend.aspx?act=listct&amp;id=" + model.NodeId + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=contact&amp;backurl=" + Utils.getPage(0) + "") + "\">通讯录</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void EditGroupctPage(int uid)
    {
        Master.Title = "编辑通讯分组";
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "分组ID错误"));
        if (!new BCW.BLL.Frigroup().Exists(id, uid, 1))
        {
            Utils.Error("不存在的记录", "");
        }
        BCW.Model.Frigroup model = new BCW.BLL.Frigroup().GetFrigroup(id);
        builder.Append(Out.Tab("<div class=\"title\">编辑通讯分组</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("请输入分组名称:");
        builder.Append(Out.Tab("</div>", ""));
        strText = ",排序:/,,,";
        strName = "Title,Paixu,id,act,backurl";
        strType = "text,snum,hidden,hidden,hidden";
        strValu = "" + model.Title + "'" + model.Paixu + "'" + id + "'editgroupctsave'" + Utils.getPage(0) + "";
        strEmpt = "false,false,false,false,false";
        strIdea = "/";
        strOthe = "编辑分组,friend.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("friend.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=contact&amp;backurl=" + Utils.getPage(0) + "") + "\">通讯录</a>");
        builder.Append(Out.Tab("</div>", ""));

    }

    private void EditGroupctSavePage(int uid)
    {
        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[0-9]\d*$", "ID错误"));
        string Title = Utils.GetRequest("Title", "post", 2, @"^[A-Za-zＡ-Ｚ\d０-９\u4E00-\u9FA5]{1,10}$", "分组名称限1-10字，不能使用特殊字符");
        int Paixu = int.Parse(Utils.GetRequest("Paixu", "post", 2, @"^[0-9]\d*$", "排序必须是数字形式"));
        if (!new BCW.BLL.Frigroup().Exists(id, uid, 1))
        {
            Utils.Error("不存在的记录", "");
        }
        BCW.Model.Frigroup model = new BCW.Model.Frigroup();
        model.ID = id;
        model.Types = 1;
        model.Title = Title;
        model.UsID = uid;
        model.Paixu = Paixu;
        model.AddTime = DateTime.Now;
        new BCW.BLL.Frigroup().Update(model);

        Utils.Success("编辑通讯分组", "编辑通讯分组成功，正在返回..", Utils.getUrl("friend.aspx?act=groupct&amp;backurl=" + Utils.getPage(0) + ""), "1");
    }

    private void DelGroupctPage(int uid)
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Frigroup().Exists(id, uid, 1))
        {
            Utils.Error("不存在的记录", "");
        }
        string info = Utils.GetRequest("info", "get", 1, "", "");

        Master.Title = "删除分组";

        if (info == "")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此分组吗.");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=delgroupct&amp;info=ok&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("friend.aspx?act=groupct") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            if (new BCW.BLL.Contact().Exists2(id, uid))
            {
                Utils.Error("此分组存在联系人，不能删除", "");
            }
            new BCW.BLL.Frigroup().Delete(id);
            Utils.Success("删除分组", "删除分组成功，正在返回..", Utils.getUrl("friend.aspx?act=groupct&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
    }

    private void SearchctPage()
    {
        Master.Title = "查找联系人";

        builder.Append(Out.Tab("<div class=\"title\">查找联系人</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("联系人名称:");
        builder.Append(Out.Tab("</div>", ""));
        strText = ",,,";
        strName = "keyword,act,backurl";
        strType = "text,hidden,hidden";
        strValu = "'searchctsave'" + Utils.PostPage(1) + "";
        strEmpt = "false,false,false";
        strIdea = "/";
        strOthe = "查找,friend.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(" <a href=\"" + Utils.getUrl("friend.aspx?act=contact&amp;backurl=" + Utils.getPage(0) + "") + "\">取消</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void SearchctSavePage(int uid)
    {
        string keyword = Utils.GetRequest("keyword", "all", 2, @"^[\s\S]{1,10}$", "请输入1-10字的搜索关键字");
        Master.Title = "查找" + keyword + "";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("搜索:“" + keyword + "”结果：");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));

        DataSet ds = new BCW.BLL.Contact().GetList("ID,Name", "UsID=" + uid + " and Name like '%" + keyword + "%'");
        if (ds != null && ds.Tables[0].Rows.Count != 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                int id = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
                string Name = ds.Tables[0].Rows[0]["Name"].ToString();
                builder.Append("<a href=\"" + Utils.getUrl("/bbs/friend.aspx?act=viewct&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + Name + "</a><br />");
            }
        }
        else
        {
            builder.Append("没有相关记录..<br />");
        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.RHr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=contact") + "\">通讯录</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void AddBlackPage(int uid)
    {
        int hid = int.Parse(Utils.GetRequest("hid", "all", 2, @"^[1-9]\d*$", "ID错误"));
        if (!new BCW.BLL.User().Exists(hid))
        {
            Utils.Error("不存在的会员ID", "");
        }
        if (uid.Equals(hid))
        {
            Utils.Error("不能加黑自己", "");
        }
        string info = Utils.GetRequest("info", "get", 1, "", "");
        Master.Title = "加黑名单";

        if (info == "")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定要加黑ID" + hid + "吗.");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=addblack&amp;info=ok&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定加黑</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("friend.aspx?act=black") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            if (new BCW.BLL.Friend().Exists(uid, hid, 1))
            {
                Utils.Error("此ID已加为黑名单", "");
            }
            //同时从对方好友中删除自己、在自己好友中删除对方
            new BCW.BLL.Friend().Delete(uid, hid, 0);
            new BCW.BLL.Friend().Delete(hid, uid, 0);

            BCW.Model.Friend model = new BCW.Model.Friend();
            model.Types = 1;
            model.NodeId = 0;
            model.UsID = uid;
            model.FriendID = hid;
            model.UsName = new BCW.BLL.User().GetUsName(uid);
            model.FriendName = new BCW.BLL.User().GetUsName(hid);
            new BCW.BLL.Friend().Add(model);

            Utils.Success("加黑ID", "加黑ID" + hid + "成功，同时从对方好友中删除自己.正在返回..", Utils.getPage("friend.aspx?act=black&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
    }

    private void AddFansPage(int uid)
    {
        int hid = int.Parse(Utils.GetRequest("hid", "all", 2, @"^[1-9]\d*$", "ID错误"));
        if (!new BCW.BLL.User().Exists(hid))
        {
            Utils.Error("不存在的会员ID", "");
        }
        if (uid.Equals(hid))
        {
            Utils.Error("不能将自己设为关注对象", "");
        }
        string info = Utils.GetRequest("info", "get", 1, "", "");
        Master.Title = "加为关注";

        if (info == "")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("您确定要将" + new BCW.BLL.User().GetUsName(hid) + "(" + hid + ")的设为关注对象吗.");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=addfans&amp;info=ok&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定设置</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            if (new BCW.BLL.Friend().Exists(uid, hid, 2))
            {
                Utils.Error("该友友已是您的关注对象，不用再次设置", "");
            }

            BCW.Model.Friend model = new BCW.Model.Friend();
            model.Types = 2;
            model.NodeId = 0;
            model.UsID = uid;
            model.FriendID = hid;
            model.UsName = new BCW.BLL.User().GetUsName(uid);
            model.FriendName = new BCW.BLL.User().GetUsName(hid);
            new BCW.BLL.Friend().Add(model);

            Utils.Success("加为关注", "将" + new BCW.BLL.User().GetUsName(uid) + "(" + uid + ")加为关注成功，正在返回..", Utils.getPage("uinfo.aspx"), "1");
        }
    }


    //得到备注与当前昵称
    private string ForFriendName(int FriendID, string FriendName)
    {
        string sFriendName = FriendName;
        string nFriendName = new BCW.BLL.User().GetUsName(FriendID);
        if (sFriendName != nFriendName)
            sFriendName = sFriendName + "(" + nFriendName + ")";

        return sFriendName;
    }
}
