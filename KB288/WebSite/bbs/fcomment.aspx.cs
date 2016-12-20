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

public partial class bbs_fcomment : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string strText = string.Empty;
    protected string strName = string.Empty;
    protected string strType = string.Empty;
    protected string strValu = string.Empty;
    protected string strEmpt = string.Empty;
    protected string strIdea = string.Empty;
    protected string strOthe = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        int leibie = int.Parse(Utils.GetRequest("leibie", "all", 1, @"^[0-4]\d*$", "0"));
        switch (act)
        {
            case "reply":
                ReplyPage(leibie);
                break;
            case "replysave":
                ReplySavePage(leibie);
                break;
            case "replylist":
                ReplyListPage(leibie);
                break;
            case "delreply":
                DelReplyPage(leibie);
                break;
            case "reok":
                ReOkPage(leibie);
                break;
            default:
                ReplyListPage(leibie);
                break;
        }
    }

    private void ReplyListPage(int leibie)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        int uid = 0;
        if (leibie == 0)
            uid = new BCW.BLL.Diary().GetUsID(id);
        else
            uid = new BCW.BLL.Upfile().GetUsID(id);

        if (uid == 0)
        {
            Utils.Error("不存在的记录", "");
        }
        string strLeibie = BCW.User.AppCase.CaseAlbums(leibie);
        Master.Title = "" + strLeibie + "评论";
        builder.Append(Out.Tab("<div class=\"title\">查看" + strLeibie + "评论</div>", ""));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "leibie", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "Types=" + leibie + " and DetailId=" + id + "";

        // 开始读取列表
        IList<BCW.Model.FComment> listFComment = new BCW.BLL.FComment().GetFComments(pageIndex, pageSize, strWhere, out recordCount);
        if (listFComment.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.FComment n in listFComment)
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

                builder.AppendFormat("{0}楼.{1}(<a href=\"" + Utils.getUrl("uinfo.aspx?uid={2}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{3}</a>/{4})", recordCount - (((pageIndex - 1) * pageSize) + (k - 1)), Out.SysUBB(n.Content), n.UsID, n.UsName, DT.FormatDate(n.AddTime, 1));
                if (!string.IsNullOrEmpty(n.ReText))
                {
                    builder.Append(Out.Tab("<font color=\"red\">", ""));
                    builder.Append("<br />★主人回复:" + n.ReText + "");
                    builder.Append(Out.Tab("</font>", ""));
                }
                bool flag = false;
                if (leibie == 0)
                {
                    if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_Diary, meid))
                        flag = true;
                }
                else
                {
                    if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_Albums, meid))
                        flag = true;
                }
                if (uid.Equals(meid))
                {
                    builder.Append("<a href=\"" + Utils.getUrl("fcomment.aspx?act=reok&amp;leibie=" + leibie + "&amp;detailid=" + n.DetailId + "&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[回复]</a>");
                }
                if (uid.Equals(meid) || flag == true)
                {
                    builder.Append("<a href=\"" + Utils.getUrl("fcomment.aspx?act=delreply&amp;leibie=" + leibie + "&amp;detailid=" + n.DetailId + "&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[删除]</a>");
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
        //得到随机短语
        string ReplyValu = string.Empty;
        DataSet ds = new BCW.BLL.Submit().GetList("TOP 1 Content", "UsID=" + meid + " and Types=0 ORDER BY NEWID()");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            ReplyValu = ds.Tables[0].Rows[0]["Content"].ToString();
        }
        builder.Append(Out.Tab("", "<br />"));
        strText = ",,,,,";
        strName = "Content,id,leibie,backurl,act";
        strType = "stext,hidden,hidden,hidden,hidden";
        strValu = "" + ReplyValu + "'" + id + "'" + leibie + "'" + Utils.PostPage(1) + "'replysave";
        strEmpt = "true,false,false,false,false";
        strIdea = "";
        strOthe = "快速评论,fcomment.aspx,post,3,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("fcomment.aspx?act=reply&amp;leibie=" + leibie + "&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">&gt;&gt;我要评论</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        if (meid == uid)
            builder.Append("我的:");
        else
            builder.Append("主人:");

        builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?uid=" + uid + "") + "\">日记</a>");
        if (leibie != 1)
            builder.Append("-<a href=\"" + Utils.getUrl("albums.aspx?leibie=1&amp;uid=" + uid + "") + "\">相册</a>");
        if (leibie != 2)
            builder.Append("-<a href=\"" + Utils.getUrl("albums.aspx?leibie=2&amp;uid=" + uid + "") + "\">音乐</a>");
        if (leibie != 3)
            builder.Append("-<a href=\"" + Utils.getUrl("albums.aspx?leibie=3&amp;uid=" + uid + "") + "\">视频</a>");
        if (leibie != 4)
            builder.Append("-<a href=\"" + Utils.getUrl("albums.aspx?leibie=4&amp;uid=" + uid + "") + "\">资源</a>");

        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        if (leibie == 0)
            builder.Append("<a href=\"" + Utils.getPage("diary.aspx?act=view&amp;id=" + id + "") + "\">上级</a>");
        else
            builder.Append("<a href=\"" + Utils.getPage("albums.aspx?act=view&amp;leibie=" + leibie + "&amp;id=" + id + "") + "\">上级</a>");

        builder.Append(Out.Tab("</div>", ""));
    }

    private void ReplyPage(int leibie)
    {
        Master.Title = "我要评论";
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        int copy = int.Parse(Utils.GetRequest("copy", "get", 1, @"^[0-1]$", "0"));
        int ff = int.Parse(Utils.GetRequest("ff", "get", 1, @"^[0-9]\d*$", "-1"));
        int dd = int.Parse(Utils.GetRequest("dd", "get", 1, @"^[0-9]\d*$", "0"));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string Title = string.Empty;
        if (leibie == 0)
            Title = new BCW.BLL.Diary().GetTitle(id);
        else
            Title = new BCW.BLL.Upfile().GetTitle(id);

        if (Title == "")
        {
            Utils.Error("不存在的记录", "");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        if (leibie == 0)
            builder.Append("评论:<a href=\"" + Utils.getUrl("diary.aspx?act=view&amp;id=" + id + "") + "\">" + Title + "</a>");
        else
            builder.Append("评论:<a href=\"" + Utils.getUrl("albums.aspx?act=view&amp;leibie=" + leibie + "&amp;id=" + id + "") + "\">" + Title + "</a>");

        builder.Append(Out.Tab("</div>", ""));

        string reText = string.Empty;
        if (ff >= 0)
            reText += "[F]" + ff + "[/F]";

        if (dd > 0)
            reText += new BCW.BLL.Submit().GetContent(dd, meid);

        if (copy == 1)
            reText += new BCW.BLL.User().GetCopytemp(meid);

        strText = "内容.300字内:/,,,,";
        strName = "Content,id,leibie,backurl,act";
        strType = "textarea,hidden,hidden,hidden,hidden";
        strValu = "" + reText + "'" + id + "'" + leibie + "'" + Utils.getPage(0) + "'replysave";
        strEmpt = "true,false,false,false,false";
        strIdea = "/";
        strOthe = "发表评论,fcomment.aspx,post,1,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("插入:<a href=\"" + Utils.getUrl("function.aspx?act=admsub&amp;backurl=" + Utils.PostPage(1) + "") + "\">常用短语</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("function.aspx?act=face&amp;backurl=" + Utils.PostPage(1) + "") + "\">表情</a>");
        builder.Append("<br /><a href=\"" + Utils.getUrl("fcomment.aspx?act=reply&amp;leibie=" + leibie + "&amp;id=" + id + "&amp;ff=" + ff + "&amp;dd=" + dd + "&amp;copy=1") + "\">[粘贴]</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        if (leibie == 0)
            builder.Append("<a href=\"" + Utils.getPage("diary.aspx?act=view&amp;id=" + id + "") + "\">取消发表</a>");
        else
            builder.Append("<a href=\"" + Utils.getPage("albums.aspx?act=view&amp;leibie=" + leibie + "&amp;id=" + id + "") + "\">取消发表</a>");

        builder.Append(Out.Tab("</div>", ""));
    }

    private void ReplySavePage(int leibie)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        BCW.User.Users.ShowVerifyRole("m", meid);//非验证会员提示
        new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Comment, meid);//会员本身权限
        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[1-9]\d*$", "ID错误"));
        string Content = Utils.GetRequest("Content", "post", 2, @"^[^\^]{1,200}$", "评论限1-200字");
        int NodeId = 0;
        string xmlPath = "/Controls/diary.xml";
        string xmlPath2 = "/Controls/albums.xml";
        int uid = 0;
        if (leibie == 0)
        {        
            //是否刷屏
            string appName = "LIGHT_DIARY";
            int Expir = Convert.ToInt32(ub.GetSub("DiaryExpir", xmlPath));
            BCW.User.Users.IsFresh(appName, Expir);
            NodeId = new BCW.BLL.Diary().GetNodeId(id);
            uid = new BCW.BLL.Diary().GetUsID(id);
        }
        else
        {           
            //是否刷屏
            string appName = "LIGHT_DIARY";
            int Expir = Convert.ToInt32(ub.GetSub("AlbumsExpir", xmlPath2));
            BCW.User.Users.IsFresh(appName, Expir);
            NodeId = new BCW.BLL.Upfile().GetNodeId(id);
            uid = new BCW.BLL.Upfile().GetUsID(id);
        }
        if (NodeId == -1)
        {
            Utils.Error("不存在的记录", "");
        }
        //你是否是对方的黑名单
        if (new BCW.BLL.Friend().Exists(uid, meid, 1))
        {
            Utils.Error("对方已把您加入黑名单", "");
        }
        int IsReview = new BCW.BLL.Upgroup().GetIsReview(NodeId);
        if (IsReview == 1)
        {
            Utils.Error("作者已设置不允许评论此分类", "");
        }

        string strLeibie = BCW.User.AppCase.CaseAlbums(leibie);
        BCW.Model.FComment model = new BCW.Model.FComment();
        model.Types = leibie;
        model.UsID = meid;
        model.UsName = new BCW.BLL.User().GetUsName(meid);
        model.DetailId = id;
        model.Content = Content;
        model.AddUsIP = Utils.GetUsIP();
        model.AddTime = DateTime.Now;
        new BCW.BLL.FComment().Add(model);
        //更新回复数
        if (leibie == 0)
            new BCW.BLL.Diary().UpdateReplyNum(id, 1);

        //内线通知主人
        if (meid != uid)
        {
            new BCW.BLL.Guest().Add(0, uid, new BCW.BLL.User().GetUsName(uid), "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + new BCW.BLL.User().GetUsName(meid) + "[/url]在您的空间" + BCW.User.AppCase.CaseAlbums(leibie) + "[url=/bbs/fcomment.aspx?leibie=" + leibie + "&amp;id=" + id + "]留言啦[/url]!");
        }
        Utils.Success("评论" + strLeibie + "", "发表评论成功，正在返回..", Utils.getPage("fcomment.aspx?act=replylist&amp;leibie=" + leibie + "&amp;id=" + id + ""), "1");

    }

    private void DelReplyPage(int leibie)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int detailid = int.Parse(Utils.GetRequest("detailid", "get", 2, @"^[1-9]\d*$", "ID错误"));
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        if (!new BCW.BLL.FComment().Exists(id, detailid))
        {
            Utils.Error("不存在的记录", "");
        }
        int uid = 0;
        if (leibie == 0)
            uid = new BCW.BLL.Diary().GetUsID(detailid);
        else
            uid = new BCW.BLL.Upfile().GetUsID(detailid);

        bool flag = false;
        if (leibie == 0)
        {
            if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_Diary, meid))
                flag = true;
        }
        else
        {
            if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_Albums, meid))
                flag = true;
        }

        if (!uid.Equals(meid) && !flag)
        {
            Utils.Error("你的权限不足", "");
        }
        string info = Utils.GetRequest("info", "get", 1, "", "");
        Master.Title = "删除评论";
        if (info == "")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此评论吗.");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("fcomment.aspx?act=delreply&amp;info=ok&amp;leibie=" + leibie + "&amp;detailid=" + detailid + "&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("fcomment.aspx?act=replylist&amp;leibie=" + leibie + "&amp;id=" + detailid + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            new BCW.BLL.FComment().Delete(id);
            Utils.Success("删除评论", "删除评论成功，正在返回..", Utils.getPage("fcomment.aspx?act=replylist&amp;leibie=" + leibie + "&amp;id=" + detailid + ""), "1");
        }
    }

    private void ReOkPage(int leibie)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int detailid = int.Parse(Utils.GetRequest("detailid", "all", 2, @"^[1-9]\d*$", "ID错误"));
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
        if (!new BCW.BLL.FComment().Exists(id, detailid))
        {
            Utils.Error("不存在的记录", "");
        }
        int uid = 0;
        if (leibie == 0)
            uid = new BCW.BLL.Diary().GetUsID(detailid);
        else
            uid = new BCW.BLL.Upfile().GetUsID(detailid);

        bool flag = false;
        if (leibie == 0)
        {
            if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_Diary, meid))
                flag = true;
        }
        else
        {
            if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_Albums, meid))
                flag = true;
        }

        if (!uid.Equals(meid) && !flag)
        {
            Utils.Error("你的权限不足", "");
        }
        string info = Utils.GetRequest("info", "post", 1, "", "");

        BCW.Model.FComment model = new BCW.BLL.FComment().GetFComment(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }

        if (info == "ok")
        {
            string Content = Utils.GetRequest("Content", "post", 3, @"^[\s\S]{1,500}$", "回复内容限1-500字内");
            new BCW.BLL.FComment().UpdateReText(id, Content);
            //内线通知留言者
            if (meid != model.UsID)
            {
                new BCW.BLL.Guest().Add(0, model.UsID, model.UsName, "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + new BCW.BLL.User().GetUsName(meid) + "[/url]回复了你您在TA空间" + BCW.User.AppCase.CaseAlbums(leibie) + "的评论啦，快去[url=/bbs/fcomment.aspx?leibie=" + leibie + "&amp;id=" + detailid + "]看看吧[/url]!");
            }
            Utils.Success("回复留言", "回复留言成功，正在返回..", Utils.getPage("fcomment.aspx?leibie=" + leibie + "&amp;id=" + detailid + ""), "1");
        }
        else
        {
            Master.Title = "回复留言";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("回复留言");
            builder.Append(Out.Tab("</div>", ""));

            string strText, strName, strType, strValu, strEmpt, strIdea, strOthe;
            strText = "回复内容:/,,,,,,";
            strType = "textarea,hidden,hidden,hidden,hidden,hidden,hidden";
            strName = "Content,id,detailid,leibie,act,info,backurl";
            strValu = "" + model.ReText + "'" + id + "'" + detailid + "'" + leibie + "'reok'ok'" + Utils.getPage(0) + "";
            strEmpt = "true,false,false,false,false,false,false";
            strIdea = "/";
            strOthe = "确定回复|reset,fcomment.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<a href=\"" + Utils.getPage("fcomment.aspx?leibie=" + leibie + "&amp;id=" + detailid + "") + "\">&gt;返回上一级</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }
}
