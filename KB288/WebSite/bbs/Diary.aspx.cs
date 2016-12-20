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

public partial class bbs_Diary : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string strText = string.Empty;
    protected string strName = string.Empty;
    protected string strType = string.Empty;
    protected string strValu = string.Empty;
    protected string strEmpt = string.Empty;
    protected string strIdea = string.Empty;
    protected string strOthe = string.Empty;
    protected string xmlPath = "/Controls/diary.xml";
    protected void Page_Load(object sender, EventArgs e)
    {
        //维护提示
        if (ub.GetSub("DiaryStatus", xmlPath) == "1")
        {
            Utils.Safe("日记系统");
        }
        string act = Utils.GetRequest("act", "all", 1, "", "");
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[0-9]\d*$", "0"));
        if (uid > 0 && act == "")
        {
            act = "me";
        }
        switch (act)
        {
            case "me":
                MePage(uid);
                break;
            case "list":
                ListPage(uid);
                break;
            case "toplist":
            case "newlist":
                MoreListPage(act);
                break;
            case "view":
                ViewPage();
                break;
            case "diaryadm":
                DiaryAdminPage();
                break;
            case "add":
                AddPage();
                break;
            case "save":
                SavePage();
                break;
            case "fill":
                FillPage();
                break;
            case "edit":
                EditPage();
                break;
            case "del":
                DelPage();
                break;
            case "delcomment":
                DelCommentPage();
                break;
            case "top":
                TopPage();
                break;
            case "admin":
                AdminPage();
                break;
            case "group":
                GroupPage();
                break;
            case "addgroup":
                AddGroupPage();
                break;
            case "groupsave":
                GroupSavePage();
                break;
            case "editgroup":
                EditGroupPage();
                break;
            case "editgroupsave":
                EditGroupSavePage();
                break;
            case "delgroup":
                DelGroupPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        Master.Title = ub.GetSub("DiaryName", xmlPath);
        if (ub.GetSub("DiaryLogo", xmlPath) != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<img src=\"" + ub.GetSub("DiaryLogo", xmlPath) + "\" alt=\"load\"/>");
            builder.Append(Out.Tab("</div>", "<br />"));

        }
        //拾物随机
        builder.Append(BCW.User.Game.GiftFlows.ShowGiftFlows(2));
        //顶部滚动
        builder.Append(BCW.User.Master.OutTopRand(5));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?uid=" + meid + "") + "\">我的日记</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?act=add") + "\">写日记</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("【人气日记】");
        builder.Append(Out.Tab("</div>", "<br />"));
        DataSet ds = null;
        int TopNum = Convert.ToInt32(ub.GetSub("DiaryTopNum", xmlPath));
        int NewNum = Convert.ToInt32(ub.GetSub("DiaryNewNum", xmlPath));
        ds = new BCW.BLL.Diary().GetList("TOP " + TopNum + " ID,Title", "((Select b.Types from tb_Upgroup b where b.ID=NodeId)=0 OR NodeId=0) and AddTime>='" + DateTime.Now.AddDays(-30) + "' ORDER BY ReadNum DESC");
        if (ds != null && ds.Tables[0].Rows.Count != 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                int id = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                string Title = ds.Tables[0].Rows[i]["Title"].ToString();
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?act=view&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + Title + "</a>");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?act=toplist") + "\">&gt;&gt;更多人气日记</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("【最新发表日记】");
        builder.Append(Out.Tab("</div>", "<br />"));
        ds = new BCW.BLL.Diary().GetList("TOP " + NewNum + " ID,Title,AddTime", "(Select b.Types from tb_Upgroup b where b.ID=NodeId)=0 OR NodeId=0 ORDER BY ID DESC");
        if (ds != null && ds.Tables[0].Rows.Count != 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                int id = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                string Title = ds.Tables[0].Rows[i]["Title"].ToString();
                DateTime AddTime = DateTime.Parse(ds.Tables[0].Rows[i]["AddTime"].ToString());
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("" + (i + 1) + ".<a href=\"" + Utils.getUrl("diary.aspx?act=view&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + Title + "</a>(" + DT.FormatDate(AddTime, 9) + ")");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?act=newlist") + "\">&gt;&gt;更多日记</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.RHr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("albums.aspx") + "\">相册</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void MePage(int uid)
    {
        string UsName = new BCW.BLL.User().GetUsName(uid);
        if (UsName == "")
        {
            Utils.Error("不存在的会员ID", "");
        }
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        //拾物随机
        builder.Append(BCW.User.Game.GiftFlows.ShowGiftFlows(2));
        //顶部滚动
        builder.Append(BCW.User.Master.OutTopRand(5));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        if (uid == meid)
        {
            new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Diary, meid);//日记权限
            Master.Title = "我的日记";
            builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?act=add") + "\">写日记</a>.");
            builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?act=admin") + "\">管理日记</a>");
        }
        else
        {
            Master.Title = "" + UsName + "的日记本";
            builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?uid=" + meid + "") + "\">&gt;&gt;进入我的日记本</a>");
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("=<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + uid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + UsName + "</a>的日记=");
        builder.Append(Out.Tab("</div>", "<br />"));
        //最新日记
        int pSize = 5;
        string strWhere = "UsID=" + uid + "";
        IList<BCW.Model.Diary> listDiary = new BCW.BLL.Diary().GetDiarysTop(pSize, strWhere);
        if (listDiary.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Diary n in listDiary)
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
                if (n.IsTop == 1)
                    builder.Append("[顶]");

                builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?act=view&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.Title + "</a>(" + DT.FormatDate(n.AddTime, 4) + ")");
                builder.Append("<br /><a href=\"" + Utils.getUrl("fcomment.aspx?act=replylist&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">评论" + n.ReplyNum + "</a>/人气" + n.ReadNum + "");
                builder.Append(Out.Tab("</div>", ""));
                k++;
            }
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?act=list&amp;uid=" + uid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">更多日记(" + new BCW.BLL.Diary().GetCount(uid) + ")</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        //分类显示
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("=日记分类=");
        builder.Append(Out.Tab("</div>", "<br />"));
        DataSet ds = null;
        ds = new BCW.BLL.Upgroup().GetList("ID,Title", "Leibie=0 and UsID=" + uid + " and Types=0 ORDER BY Paixu ASC");
        if (ds != null && ds.Tables[0].Rows.Count != 0)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("[公]:");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                int id = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                string Title = Utils.Left(ds.Tables[0].Rows[i]["Title"].ToString(), 2);
                builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?act=list&amp;uid=" + uid + "&amp;id=" + id + "") + "\">" + Title + "(" + new BCW.BLL.Diary().GetCount(uid, id) + ")</a>.");
            }
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        ds = new BCW.BLL.Upgroup().GetList("ID,Title", "Leibie=0 and UsID=" + uid + " and Types=1 ORDER BY Paixu ASC");
        if (ds != null && ds.Tables[0].Rows.Count != 0)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("[私]:");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                int id = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                string Title = Utils.Left(ds.Tables[0].Rows[i]["Title"].ToString(), 2);
                builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?act=list&amp;uid=" + uid + "&amp;id=" + id + "") + "\">" + Title + "(" + new BCW.BLL.Diary().GetCount(uid, id) + ")</a>.");
            }
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        ds = new BCW.BLL.Upgroup().GetList("ID,Title", "Leibie=0 and UsID=" + uid + " and Types=2 ORDER BY Paixu ASC");
        if (ds != null && ds.Tables[0].Rows.Count != 0)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("[友]:");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                int id = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                string Title = Utils.Left(ds.Tables[0].Rows[i]["Title"].ToString(), 2);
                builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?act=list&amp;uid=" + uid + "&amp;id=" + id + "") + "\">" + Title + "(" + new BCW.BLL.Diary().GetCount(uid, id) + ")</a>.");
            }
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        //builder.Append(Out.Tab("<div>", ""));
        //builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?act=list&amp;uid=" + uid + "&amp;id=0&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;&gt;默认分类(" + new BCW.BLL.Diary().GetCount(uid, 0) + ")</a>");
        //builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (meid == uid)
            builder.Append("我的:");
        else
            builder.Append("主人:");

        builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?leibie=1&amp;uid=" + uid + "") + "\">相册</a>");
        builder.Append("-<a href=\"" + Utils.getUrl("albums.aspx?leibie=2&amp;uid=" + uid + "") + "\">音乐</a>");
        builder.Append("-<a href=\"" + Utils.getUrl("albums.aspx?leibie=3&amp;uid=" + uid + "") + "\">视频</a>");
        builder.Append("-<a href=\"" + Utils.getUrl("albums.aspx?leibie=4&amp;uid=" + uid + "") + "\">资源</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("diary.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ListPage(int uid)
    {
        string UsName = new BCW.BLL.User().GetUsName(uid);
        if (UsName == "")
        {
            Utils.Error("不存在的会员ID", "");
        }
        int id = int.Parse(Utils.GetRequest("id", "get", 1, @"^[0-9]\d*$", "-1"));
        string Title = string.Empty;
        if (id > 0)
        {
            Title = new BCW.BLL.Upgroup().GetTitle(id, uid);
            if (Title == "")
            {
                Utils.Error("不存在的日记分组", "");
            }
        }
        else if (id == 0)
        {
            Title = "默认分类";
        }
        else
        {
            Title = "所有日记";

        }
        Master.Title = "" + UsName + "的日记";
        //拾物随机
        builder.Append(BCW.User.Game.GiftFlows.ShowGiftFlows(2));
        //顶部滚动
        builder.Append(BCW.User.Master.OutTopRand(5));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?uid=" + uid + "") + "\">" + UsName + "的日记</a>&gt;" + Title + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = 8;
        string strWhere = "";
        string strOrder = "";
        string[] pageValUrl = { "act", "uid", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        if (id != -1)
            strWhere = "UsID=" + uid + " and NodeId=" + id + "";
        else
            strWhere = "UsID=" + uid + "";

        strOrder = "IsTop Desc,ID Desc";
        // 开始读取列表
        IList<BCW.Model.Diary> listDiary = new BCW.BLL.Diary().GetDiarys(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listDiary.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Diary n in listDiary)
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
                //builder.AppendFormat("{0}.", (pageIndex - 1) * pageSize + k);
                if (n.IsTop == 1)
                    builder.Append("[顶]");

                builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?act=view&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.Title + "</a>(" + DT.FormatDate(n.AddTime, 4) + ")");
                builder.Append("<br /><a href=\"" + Utils.getUrl("fcomment.aspx?act=replylist&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">评论" + n.ReplyNum + "</a>/人气" + n.ReadNum + "");
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
        builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?uid=" + uid + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("diary.aspx") + "\">日记首页</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void MoreListPage(string act)
    {
        string Title = string.Empty;
        if (act == "toplist")
            Title = "本月人气日记";
        else
            Title = "更多最新日记";

        Master.Title = Title;

        //拾物随机
        builder.Append(BCW.User.Game.GiftFlows.ShowGiftFlows(2));
        //顶部滚动
        builder.Append(BCW.User.Master.OutTopRand(5));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(Title);
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string strOrder = "";
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //排序条件
        if (act == "toplist")
        {
            //查询条件
            strWhere = "AddTime>='" + DateTime.Now.AddDays(-30) + "'";
            strOrder = "ReadNum DESC,ID DESC";
        }
        else
            strOrder = "ID Desc";

        // 开始读取列表
        IList<BCW.Model.Diary> listDiary = new BCW.BLL.Diary().GetDiarys(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listDiary.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Diary n in listDiary)
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
                builder.AppendFormat("{0}.", (pageIndex - 1) * pageSize + k);
                builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?act=view&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.Title + "</a>(" + DT.FormatDate(n.AddTime, 4) + ")");
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
        builder.Append("<a href=\"" + Utils.getUrl("diary.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }


    private void ViewPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.Diary model = new BCW.BLL.Diary().GetDiary(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        //查看权限(1好友可见/2私人)
        if (model.UsID != meid)
        {
            int Types = new BCW.BLL.Upgroup().GetTypes(model.NodeId);
            if (Types == 2)
            {
                Utils.Error("此日记属<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.UsName + "</a>私人日记..", "");
            }
            else if (Types == 1)
            {
                if (!new BCW.BLL.Friend().Exists(model.UsID, meid, 0))
                {
                    Utils.Error("此日记仅限<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.UsName + "</a>的好友才可以查看..", "");
                }
            }
        }

        Master.Title = model.Title;
        //拾物随机
        builder.Append(BCW.User.Game.GiftFlows.ShowGiftFlows(2));
        //顶部滚动
        builder.Append(BCW.User.Master.OutTopRand(5));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?uid=" + model.UsID + "") + "\">" + model.UsName + "的日记</a>&gt;日记");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("标题:" + model.Title + "<br />");
        builder.Append("作者:" + model.UsName + "<br />");
        builder.Append("时间:" + model.AddTime + "<br />");
        if (model.NodeId == 0)
            builder.Append("分类:未分类");
        else
            builder.Append("分类:" + new BCW.BLL.Upgroup().GetTitle(model.NodeId, model.UsID) + "");

        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));

        int pageIndex;
        int recordCount;
        int pageSize = 500;
        string[] pageValUrl = { "act", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["vp"]);
        if (pageIndex == 0)
            pageIndex = 1;

        int pover = int.Parse(Utils.GetRequest("pover", "get", 1, @"^[0-9]\d*$", "0"));
        string content = BasePage.MultiContent(model.Content, pageIndex, pageSize, pover, out recordCount);
        builder.Append(Out.SysUBB(content));

        builder.Append(BasePage.MultiContentPage(model.Content, pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "vp", pover));
        builder.Append(Out.Tab("</div>", ""));

        //更新人气数
        new BCW.BLL.Diary().UpdateReadNum(id, 1);
        if (model.UsID.Equals(meid))
        {
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?act=diaryadm&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;&gt;管理日记</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_Diary, meid))
            {
                builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?act=del&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">&gt;&gt;删除日记</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        builder.Append(Out.Tab("", "<br />"));

        string strName = "purl,act,backurl";
        string strValu = "'recommend'" + Utils.PostPage(1) + "";
        string strOthe = "&gt;分享给好友,/bbs/guest.aspx,post,1,other";
        builder.Append(Out.wapform(strName, strValu, strOthe));

        builder.Append(Out.Tab("", Out.Hr()));
        DataSet ds = new BCW.BLL.FComment().GetList("UsName,Content,AddTime,ReText", "Types=0 and DetailId=" + id + " ORDER BY ID DESC");
        if (ds != null && ds.Tables[0].Rows.Count != 0)
        {
            int k = ds.Tables[0].Rows.Count;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if ((i + 1) % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", ""));
                else
                    builder.Append(Out.Tab("<div>", ""));

                string UsName = ds.Tables[0].Rows[i]["UsName"].ToString();
                string Content = ds.Tables[0].Rows[i]["Content"].ToString();
                DateTime AddTime = DateTime.Parse(ds.Tables[0].Rows[i]["AddTime"].ToString());
                string ReText = ds.Tables[0].Rows[i]["ReText"].ToString();
                string strContent = Regex.Replace(Utils.Left(Content, 30), @"&(?![\#\w]{2,6};)", "&amp;");
                if (Content.Length > 30)
                    strContent = strContent + "..";

                builder.AppendFormat("{0}楼.{1}:{2}({3})", k, UsName, Out.SysUBB(strContent), DT.FormatDate(AddTime, 1));
                if (!string.IsNullOrEmpty(ReText))
                {
                    builder.Append(Out.Tab("<font color=\"red\">", ""));
                    builder.Append("<br />★主人回复:" + ReText + "");
                    builder.Append(Out.Tab("</font>", ""));
                }
                builder.Append(Out.Tab("</div>", "<br />"));
                if ((i + 1) >= 3)//显示条数
                    break;
                k = k - 1;
            }
        }
        else
        {
            builder.Append(Out.Div("div", "还没有评论，快抢沙发呀.."));
            builder.Append(Out.Tab("", "<br />"));
        }
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("fcomment.aspx?act=replylist&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">&gt;&gt;查看评论(" + model.ReplyNum + "条)</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        //得到随机短语
        string ReplyValu = string.Empty;
        ds = new BCW.BLL.Submit().GetList("TOP 1 Content", "UsID=" + meid + " and Types=0 ORDER BY NEWID()");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            ReplyValu = ds.Tables[0].Rows[0]["Content"].ToString();
        }
        strText = ",,,,";
        strName = "Content,id,backurl,act";
        strType = "stext,hidden,hidden,hidden";
        strValu = "" + ReplyValu + "'" + id + "'" + Utils.PostPage(1) + "'replysave";
        strEmpt = "true,false,false,false";
        strIdea = "";
        strOthe = "快速评论,fcomment.aspx,post,3,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("diary.aspx?uid=" + model.UsID + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("diary.aspx") + "\">日记首页</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void DiaryAdminPage()
    {
        Master.Title = "管理日记";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Diary, meid);//日记权限
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.Diary model = new BCW.BLL.Diary().GetDiary(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.UsID != meid)
        {
            Utils.Error("不存在的记录", "");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("管理日记");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?act=fill&amp;id=" + id + "") + "\">续写日记</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?act=edit&amp;id=" + id + "") + "\">编辑日记</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?act=del&amp;id=" + id + "") + "\">删除日记</a><br />");
        if (model.IsTop == 0)
            builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?act=top&amp;id=" + id + "") + "\">设置置顶</a>");
        else
            builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?act=top&amp;id=" + id + "") + "\">去掉置顶</a>");

        builder.Append("<br /><a href=\"" + Utils.getUrl("diary.aspx?act=delcomment&amp;id=" + id + "") + "\">清空评论</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?act=view&amp;id=" + id + "") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void FillPage()
    {
        Master.Title = "续写日记";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Diary, meid);//日记权限
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.Diary model = new BCW.BLL.Diary().GetDiary(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.UsID != meid)
        {
            Utils.Error("不存在的记录", "");
        }
        //计算可续写字数
        int maxLength = Utils.ParseInt(ub.GetSub("DiaryContentNum", xmlPath));
        int ContentLength = maxLength - model.Content.Length;
        if (ContentLength < 0)
            ContentLength = 0;

        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info != "ok")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("续写：<a href=\"" + Utils.getUrl("diary.aspx?act=view&amp;id=" + id + "") + "\">" + model.Title + "</a>");
            builder.Append(Out.Tab("</div>", ""));

            strText = "续写内容(可续" + ContentLength + "字):/,,,,,";
            strName = "Content,id,act,info,backurl";
            strType = "textarea,hidden,hidden,hidden,hidden";
            strValu = "[续]'" + id + "'fill'ok'" + Utils.getPage(0) + "";
            strEmpt = "false,false,false,false,false";
            strIdea = "/";
            strOthe = "确定续写|reset,diary.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(" <a href=\"" + Utils.getUrl("diary.aspx?act=diaryadm&amp;id=" + id + "") + "\">取消</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            string Content = Utils.GetRequest("Content", "post", 2, @"^[^\^]{1," + ContentLength + "}$", "请输入不超" + ContentLength + "的内容");
            new BCW.BLL.Diary().UpdateContent(id, model.Content + Content);

            Utils.Success("续写日记", "续写成功，正在返回..<br /><a href=\"" + Utils.getUrl("diary.aspx?act=diaryadm&amp;id=" + id + "") + "\">继续管理</a>", Utils.getUrl("diary.aspx?act=view&amp;id=" + id + ""), "3");
        }
    }

    private void EditPage()
    {
        Master.Title = "编辑日记";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Diary, meid);//日记权限
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.Diary model = new BCW.BLL.Diary().GetDiary(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.UsID != meid)
        {
            Utils.Error("不存在的记录", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info != "ok")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("编辑：<a href=\"" + Utils.getUrl("diary.aspx?act=view&amp;id=" + id + "") + "\">" + model.Title + "</a>");
            builder.Append(Out.Tab("</div>", ""));

            string strUpgroup = string.Empty;
            DataSet ds = new BCW.BLL.Upgroup().GetList("ID,Title", "Leibie=0 and UsID=" + meid + "");
            if (ds != null && ds.Tables[0].Rows.Count != 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    strUpgroup += "|" + ds.Tables[0].Rows[i]["ID"] + "|" + ds.Tables[0].Rows[i]["Title"] + "";
                }
            }
            strUpgroup = "0|默认分组" + strUpgroup;
            strText = "日记标题:(15字内)/,内容:/,天气:,日记分组:/,,,,";
            strName = "Title,Content,Weather,NodeId,id,act,info,backurl";
            strType = "text,textarea,stext,select,hidden,hidden,hidden,hidden";
            strValu = "" + model.Title + "'" + model.Content + "'" + model.Weather + "'" + model.NodeId + "'" + id + "'edit'ok'" + Utils.getPage(0) + "";
            strEmpt = "false,false,false," + strUpgroup + ",false,false,false,false";
            strIdea = "/";
            strOthe = "确定编辑,diary.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(" <a href=\"" + Utils.getUrl("diary.aspx?act=diaryadm&amp;id=" + id + "") + "\">取消</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            string Title = Utils.GetRequest("Title", "post", 2, @"^[^\^]{1,15}$", "标题限1-15字");
            string Content = Utils.GetRequest("Content", "post", 2, @"^[^\^]{1,5000}$", "内容最多5000字");
            string Weather = Utils.GetRequest("Weather", "post", 2, @"^[^\^]{1,5}$", "天气限1-5字");
            int NodeId = int.Parse(Utils.GetRequest("NodeId", "post", 2, @"^[0-9]\d*$", "选择分组错误"));
            if (NodeId > 0)
            {
                if (!new BCW.BLL.Upgroup().Exists(NodeId, meid, 0))
                {
                    Utils.Error("选择的分组不存在", "");
                }
            }
            BCW.Model.Diary model2 = new BCW.Model.Diary();
            model2.ID = id;
            model2.NodeId = NodeId;
            model2.Weather = Weather;
            model2.Title = Title;
            model2.Content = Content;
            model2.UsID = meid;
            model2.UsName = new BCW.BLL.User().GetUsName(meid);
            new BCW.BLL.Diary().Update(model2);
            Utils.Success("编辑日记", "编辑成功，正在返回..<br /><a href=\"" + Utils.getUrl("diary.aspx?act=diaryadm&amp;id=" + id + "") + "\">继续管理</a>", Utils.getUrl("diary.aspx?act=view&amp;id=" + id + ""), "3");

        }
    }

    private void DelPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Diary, meid);//日记权限
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_Diary, meid))
        {
            if (!new BCW.BLL.Diary().Exists(id))
            {
                Utils.Error("不存在的日记记录", "");
            }
        }
        else
        {
            if (!new BCW.BLL.Diary().Exists(id, meid))
            {
                Utils.Error("不存在的日记记录", "");
            }
        }
        string info = Utils.GetRequest("info", "get", 1, "", "");
        Master.Title = "删除日记";
        if (info == "")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此日记吗.");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?act=del&amp;info=ok&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("diary.aspx?act=diaryadm&amp;id=" + id + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            new BCW.BLL.Diary().Delete(id);
            new BCW.BLL.FComment().Delete(0, id);
            //扣除积分操作
            new BCW.User.Cent().UpdateCent(BCW.User.Cent.enumRole.Cent_Diary, meid, 1);

            Utils.Success("删除日记", "删除日记成功，正在返回..", Utils.getPage("diary.aspx?act=list&amp;uid=" + meid + ""), "1");
        }
    }

    private void DelCommentPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Diary, meid);//日记权限
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.Diary model = new BCW.BLL.Diary().GetDiary(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.UsID != meid)
        {
            Utils.Error("不存在的记录", "");
        }
        string info = Utils.GetRequest("info", "get", 1, "", "");
        Master.Title = "清空日记评论";
        if (info == "")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定清空此日记的评论吗.");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?act=delcomment&amp;info=ok&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定清空</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("diary.aspx?act=diaryadm&amp;id=" + id + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            new BCW.BLL.FComment().Delete(0, id);
            Utils.Success("清空日记评论", "清空日记评论成功，正在返回..<br /><a href=\"" + Utils.getUrl("diary.aspx?act=diaryadm&amp;id=" + id + "") + "\">继续管理</a>", Utils.getUrl("diary.aspx?act=view&amp;id=" + id + ""), "3");
        }
    }

    private void TopPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Diary, meid);//日记权限
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Diary().Exists(id, meid))
        {
            Utils.Error("不存在的日记记录", "");
        }
        int IsTop = new BCW.BLL.Diary().GetIsTop(id);
        int Types = 0;
        string Title = string.Empty;
        if (IsTop == 0)
        {
            Types = 1;
            Title = "置顶";
        }
        else
        {
            Title = "去顶";
        }
        Master.Title = Title + "日记";
        string info = Utils.GetRequest("info", "get", 1, "", "");
        if (info == "")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定" + Title + "此日记吗.");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?act=top&amp;info=ok&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定" + Title + "</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("diary.aspx?act=diaryadm&amp;id=" + id + "") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            new BCW.BLL.Diary().UpdateIsTop(id, Types);
            Utils.Success("" + Title + "日记", "" + Title + "操作成功，正在返回..<br /><a href=\"" + Utils.getUrl("diary.aspx?act=diaryadm&amp;id=" + id + "") + "\">继续管理</a>", Utils.getUrl("diary.aspx?act=view&amp;id=" + id + ""), "3");

        }
    }

    private void AddPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Diary, meid);//日记权限
        int copy = int.Parse(Utils.GetRequest("copy", "get", 1, @"^[0-1]$", "0"));
        int ff = int.Parse(Utils.GetRequest("ff", "get", 1, @"^[0-9]\d*$", "-1"));
        int dd = int.Parse(Utils.GetRequest("dd", "get", 1, @"^[0-9]\d*$", "0"));
        string reText = string.Empty;
        if (ff >= 0)
            reText += "[F]" + ff + "[/F]";

        if (dd > 0)
            reText += new BCW.BLL.Submit().GetContent(dd, meid);

        if (copy == 1)
            reText += new BCW.BLL.User().GetCopytemp(meid);

        Master.Title = "编写新日记";
        string strUpgroup = string.Empty;
        DataSet ds = new BCW.BLL.Upgroup().GetList("ID,Title", "Leibie=0 and UsID=" + meid + "");
        if (ds != null && ds.Tables[0].Rows.Count != 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                strUpgroup += "|" + ds.Tables[0].Rows[i]["ID"] + "|" + ds.Tables[0].Rows[i]["Title"] + "";
            }
        }
        strUpgroup = "0|默认分组" + strUpgroup;
        builder.Append(Out.Tab("<div class=\"title\">编写新日记</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("日记标题:(15字内)");
        builder.Append(Out.Tab("</div>", ""));
        strText = ",内容:/,天气:,日记分组:/,,";
        strName = "Title,Content,Weather,NodeId,act,backurl";
        strType = "text,textarea,stext,select,hidden,hidden";
        strValu = "'" + reText + "'晴天'0'save'" + Utils.getPage(0) + "";
        strEmpt = "false,false,false," + strUpgroup + ",false,false";
        strIdea = "/";
        strOthe = "确定提交,diary.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("插入:<a href=\"" + Utils.getUrl("function.aspx?act=admsub&amp;backurl=" + Utils.PostPage(1) + "") + "\">常用短语</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("function.aspx?act=face&amp;backurl=" + Utils.PostPage(1) + "") + "\">表情</a>");
        builder.Append("<br /><a href=\"" + Utils.getUrl("diary.aspx?act=add&amp;ff=" + ff + "&amp;dd=" + dd + "&amp;copy=1") + "\">[粘贴]</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>");
        builder.Append("<a href=\"" + Utils.getPage("diary.aspx?uid=" + meid + "") + "\">上级</a>");
        builder.Append("<a href=\"" + Utils.getUrl("diary.aspx") + "\">日记首页</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void SavePage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        BCW.User.Users.ShowVerifyRole("e", meid);//非验证会员提示
        new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Diary, meid);//日记权限
        string Title = Utils.GetRequest("Title", "post", 2, @"^[\s\S]{1," + ub.GetSub("DiaryTitleNum", xmlPath) + "}$", "标题限1-15字");
        string Content = Utils.GetRequest("Content", "post", 2, @"^[\s\S]{1," + ub.GetSub("DiaryContentNum", xmlPath) + "}$", "内容最多5000字");
        string Weather = Utils.GetRequest("Weather", "post", 2, @"^[\s\S]{1,5}$", "天气限1-5字");
        int NodeId = int.Parse(Utils.GetRequest("NodeId", "post", 2, @"^[0-9]\d*$", "选择分组错误"));
        //是否刷屏
        string appName = "LIGHT_ADDDIARY";
        int Expir = Convert.ToInt32(ub.GetSub("DiaryAddExpir", xmlPath));
        BCW.User.Users.IsFresh(appName, Expir);
        if (NodeId > 0)
        {
            if (!new BCW.BLL.Upgroup().Exists(NodeId, meid, 0))
            {
                Utils.Error("选择的分组不存在", "");
            }
        }
        //检查今天发表数量
        int maxAddNum = Convert.ToInt32(ub.GetSub("DiaryAddNum", xmlPath));
        if (maxAddNum > 0)
        {
            int AddNum = new BCW.BLL.Diary().GetTodayCount(meid);
            if (AddNum >= maxAddNum)
            {
                Utils.Error("每人每天只可以发表" + maxAddNum + "篇日记哦", "");
            }
        }
        BCW.Model.Diary model = new BCW.Model.Diary();
        model.NodeId = NodeId;
        model.Title = Title;
        model.Weather = Weather;
        model.Content = Content;
        model.UsID = meid;
        model.UsName = new BCW.BLL.User().GetUsName(meid);
        model.AddUsIP = Utils.GetUsIP();
        model.AddTime = DateTime.Now;
        int nid = new BCW.BLL.Diary().Add(model);
        //动态记录
        new BCW.BLL.Action().Add(meid, "发表了日记[URL=/bbs/diary.aspx?act=view&amp;id=" + nid + "]" + Title + "[/URL]");
        //积分操作
        new BCW.User.Cent().UpdateCent(BCW.User.Cent.enumRole.Cent_Diary, meid);
        Utils.Success("编写日记", "编写日记成功，正在返回..", Utils.getPage("diary.aspx?uid=" + meid + ""), "1");
    }

    private void AdminPage()
    {
        Master.Title = "管理日记";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Diary, meid);//日记权限
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?act=add") + "\">编写新日记</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?act=addgroup") + "\">新建分组</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?act=group") + "\">编辑分组</a><br />");
        builder.Append("设定皮肤");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?uid=" + meid + "") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void GroupPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Diary, meid);//日记权限
        Master.Title = "日记分组管理";
        builder.Append(Out.Tab("<div class=\"title\">日记分组管理</div>", ""));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?act=addgroup&amp;backurl=" + Utils.PostPage(1) + "") + "\">新建分组</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?act=list&amp;uid=" + meid + "&amp;id=0&amp;backurl=" + Utils.PostPage(1) + "") + "\">默认分组(" + new BCW.BLL.Diary().GetCount(meid, 0) + ")</a>");
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
        strWhere = "Leibie=0 and UsID=" + meid + "";

        // 开始读取列表
        IList<BCW.Model.Upgroup> listUpgroup = new BCW.BLL.Upgroup().GetUpgroups(pageIndex, pageSize, strWhere, out recordCount);
        if (listUpgroup.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Upgroup n in listUpgroup)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                    builder.Append(Out.Tab("<div>", "<br />"));

                string aText = "(公)";
                if (n.Types == 1)
                    aText = "(友)";
                else if (n.Types == 2)
                    aText = "(私)";

                builder.Append("" + aText + "<a href=\"" + Utils.getUrl("diary.aspx?act=list&amp;uid=" + n.UsID + "&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.Title + "(" + new BCW.BLL.Diary().GetCount(meid, n.ID) + ")</a>");
                builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?act=editgroup&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[编辑]</a>");
                builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?act=delgroup&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[删除]</a>");

                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 1));

        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?act=admin") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void AddGroupPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Diary, meid);//日记权限
        Master.Title = "添加日记分组";

        builder.Append(Out.Tab("<div class=\"title\">添加日记分组</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("分组名称(建议2字):");
        builder.Append(Out.Tab("</div>", ""));
        strText = ",性质:/,允许评论:/,排序:/,,,";
        strName = "Title,Types,IsReview,Paixu,act,backurl";
        strType = "text,select,select,snum,hidden,hidden";
        strValu = "'0'0'0'groupsave'" + Utils.getPage(0) + "";
        strEmpt = "false,0|对外公开|1|好友可见|2|个人隐私,0|允许|1|不允许,false,false,false";
        strIdea = "/";
        strOthe = "添加分组,diary.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?act=group&amp;uid=" + meid + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?act=admin") + "\">日记管理</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void GroupSavePage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Diary, meid);//日记权限
        string Title = Utils.GetRequest("Title", "post", 2, @"^[A-Za-zＡ-Ｚ\d０-９\u4E00-\u9FA5]{1,10}$", "分组名称限1-10字，不能使用特殊字符");
        int Types = int.Parse(Utils.GetRequest("Types", "post", 2, @"^[0-2]\d*$", "分组性质选择错误"));
        int IsReview = int.Parse(Utils.GetRequest("IsReview", "post", 2, @"^[0-1]$", "允许评论选择错误"));
        int Paixu = int.Parse(Utils.GetRequest("Paixu", "post", 2, @"^[0-9]\d*$", "排序必须是数字形式"));

        if (new BCW.BLL.Upgroup().ExistsTitle(meid, Title, 0))
        {
            Utils.Error("此分组名称已存在", "");
        }
        BCW.Model.Upgroup model = new BCW.Model.Upgroup();
        model.Leibie = 0;
        model.Types = Types;
        model.PostType = 0;
        model.Title = Title;
        model.UsID = meid;
        model.IsReview = IsReview;
        model.Paixu = Paixu;
        model.AddTime = DateTime.Now;
        new BCW.BLL.Upgroup().Add(model);

        Utils.Success("添加日记分组", "添加日记分组成功，正在返回..", Utils.getUrl("diary.aspx?act=group&amp;backurl=" + Utils.getPage(0) + ""), "1");
    }

    private void EditGroupPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Diary, meid);//日记权限
        Master.Title = "编辑日记分组";
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "分组ID错误"));
        if (!new BCW.BLL.Upgroup().Exists(id, meid, 0))
        {
            Utils.Error("不存在的记录", "");
        }
        BCW.Model.Upgroup model = new BCW.BLL.Upgroup().GetUpgroup(id);
        builder.Append(Out.Tab("<div class=\"title\">编辑日记分组</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("分组名称(建议2字):");
        builder.Append(Out.Tab("</div>", ""));
        strText = ",性质:/, 允许评论:/,排序:/,,,";
        strName = "Title,Types,IsReview,Paixu,id,act,backurl";
        strType = "text,select,select,snum,hidden,hidden,hidden";
        strValu = "" + model.Title + "'" + model.Types + "'" + model.IsReview + "'" + model.Paixu + "'" + id + "'editgroupsave'" + Utils.getPage(0) + "";
        strEmpt = "false,0|对外公开|1|好友可见|2|个人隐私,0|允许|1|不允许,false,false,false,false";
        strIdea = "/";
        strOthe = "编辑分组,diary.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?act=group&amp;uid=" + meid + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?act=admin") + "\">日记管理</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void EditGroupSavePage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Diary, meid);//日记权限
        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[0-9]\d*$", "ID错误"));
        string Title = Utils.GetRequest("Title", "post", 2, @"^[A-Za-zＡ-Ｚ\d０-９\u4E00-\u9FA5]{1,10}$", "分组名称限1-10字，不能使用特殊字符");
        int Types = int.Parse(Utils.GetRequest("Types", "post", 2, @"^[0-2]\d*$", "分组性质选择错误"));
        int IsReview = int.Parse(Utils.GetRequest("IsReview", "post", 2, @"^[0-1]$", "允许评论选择错误"));
        int Paixu = int.Parse(Utils.GetRequest("Paixu", "post", 2, @"^[0-9]\d*$", "排序必须是数字形式"));
        if (!new BCW.BLL.Upgroup().Exists(id, meid, 0))
        {
            Utils.Error("不存在的记录", "");
        }
        BCW.Model.Upgroup model = new BCW.Model.Upgroup();
        model.ID = id;
        model.Types = Types;
        model.PostType = 0;
        model.Title = Title;
        model.UsID = meid;
        model.IsReview = IsReview;
        model.Paixu = Paixu;
        model.AddTime = DateTime.Now;
        new BCW.BLL.Upgroup().Update(model);

        Utils.Success("编辑日记分组", "编辑日记分组成功，正在返回..", Utils.getUrl("diary.aspx?act=group&amp;backurl=" + Utils.getPage(0) + ""), "1");
    }

    private void DelGroupPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Diary, meid);//日记权限
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Upgroup().Exists(id, meid, 0))
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
            builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?act=delgroup&amp;info=ok1&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除一(保留分组里的日记)</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?act=delgroup&amp;info=ok2&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除二(分组里的日记删除)</a><br />");

            builder.Append("<a href=\"" + Utils.getPage("diary.aspx?act=group") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("温馨提示：选择删除一，本组日记将移到默认分组.");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            new BCW.BLL.Upgroup().Delete(id);
            if (info == "ok1")
                new BCW.BLL.Diary().UpdateNodeId(meid, id);
            else
                new BCW.BLL.Diary().Delete(meid, id);

            Utils.Success("删除分组", "删除分组成功，正在返回..", Utils.getUrl("diary.aspx?act=group&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
    }
}
