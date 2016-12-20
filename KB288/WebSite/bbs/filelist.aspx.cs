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

public partial class bbs_filelist : System.Web.UI.Page
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
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int forumid = int.Parse(Utils.GetRequest("forumid", "all", 2, @"^[0-9]\d*$", "论坛ID错误"));
        int bid = int.Parse(Utils.GetRequest("bid", "all", 2, @"^[0-9]\d*$", "帖子ID错误"));
        int reid = int.Parse(Utils.GetRequest("reid", "all", 1, @"^[0-9]\d*$", "0"));
        if (!new BCW.BLL.Forum().Exists2(forumid))
        {
            Utils.Success("访问论坛", "该论坛不存在或已暂停使用", Utils.getUrl("forum.aspx"), "1");
        }
        if (!new BCW.BLL.Text().Exists2(bid, forumid))
        {
            Utils.Error("帖子不存在或已被删除", "");
        }
        if (reid > 0)
        {
            if (!new BCW.BLL.Reply().Exists(reid))
            {
                Utils.Error("不存在的回帖记录", "");
            }
        }

        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "delfile":
                DelFilePage(meid, forumid, bid, reid);
                break;
            default:
                ReloadPage(meid, forumid, bid, reid);
                break;
        }
    }

    private void ReloadPage(int uid, int forumid, int bid, int reid)
    {
        Master.Title = "查看附件";
        builder.Append(Out.Tab("<div class=\"title\">查看附件</div>", ""));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        int showtype = int.Parse(Utils.GetRequest("showtype", "get", 1, @"^[0-1]$", "0"));
        builder.Append("图片:");
        if (showtype == 0)
            builder.Append("缩略图|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("filelist.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;uid=" + uid + "&amp;reid=" + reid + "&amp;showtype=0&amp;backurl=" + Utils.getPage(0) + "") + "\">缩略图</a>|");

        if (showtype == 1)
            builder.Append("原图");
        else
            builder.Append("<a href=\"" + Utils.getUrl("filelist.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;uid=" + uid + "&amp;reid=" + reid + "&amp;showtype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">原图</a>");

        builder.Append(Out.Tab("</div>", Out.Hr()));
        int pageIndex;
        int recordCount;
        int pageSize = 5;
        string strWhere = string.Empty;
        string[] pageValUrl = { "act", "showtype", "uid", "forumid", "bid", "reid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        strWhere = "BID=" + bid + " and ReID=" + reid + "";
        // 开始读取列表
        IList<BCW.Model.Upfile> listUpfile = new BCW.BLL.Upfile().GetUpfiles(pageIndex, pageSize, strWhere, out recordCount);
        if (listUpfile.Count > 0)
        {

            int k = 1;
            foreach (BCW.Model.Upfile n in listUpfile)
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
                string Content = n.Content;
                if (string.IsNullOrEmpty(Content))
                    Content = "无标题";

                if (n.Types == 1)
                {
                    if (showtype == 0)
                        builder.Append("<a href=\"" + Utils.getUrl("/showpic.aspx?pic=" + n.Files + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"><img src=\"" + n.PrevFiles + "\" alt=\"load\"/></a><br />" + ((pageIndex - 1) * pageSize + k) + "." + Content + "");
                    else
                        builder.Append("<a href=\"" + Utils.getUrl("/showpic.aspx?pic=" + n.Files + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"><img src=\"" + n.Files + "\" alt=\"load\"/></a><br />" + ((pageIndex - 1) * pageSize + k) + "." + Content + "");
                }
                else
                    builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=down&amp;leibie=" + n.Types + "&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + ((pageIndex - 1) * pageSize + k) + "" + Content + "(" + BCW.Files.FileTool.GetContentLength(n.FileSize) + ")</a>");

                if (n.UsID == uid)
                    builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=edit&amp;leibie=" + n.Types + "&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[改]</a>");

                if (n.UsID == uid || new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_DelFile, uid, forumid))
                    builder.Append("<a href=\"" + Utils.getUrl("filelist.aspx?act=delfile&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + reid + "&amp;id=" + n.ID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[删]</a>");
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
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("forum.aspx?forumid=" + forumid + "") + "\">上级</a>-");

        if (reid > 0)
            builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?act=view&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + new BCW.BLL.Reply().GetFloor2(reid) + "&amp;backurl=" + Utils.getPage(0) + "") + "\">回帖</a>-");

        builder.Append("<a href=\"" + Utils.getUrl("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">主题帖</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void DelFilePage(int uid, int forumid, int bid, int reid)
    {
        Master.Title = "删除附件";
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.Upfile model = new BCW.BLL.Upfile().GetUpfileMe(id);
        if (model == null || model.BID != bid)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.UsID != uid && !new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_DelFile, uid, forumid))
        {
            Utils.Error("你的权限不足", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info != "ok")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此附件吗");
            builder.Append(Out.Tab("</div>", ""));
            if (model.UsID != uid)
            {
                strText = "理由:,,,,,,";
                strName = "Why,forumid,bid,reid,id,act,info,backurl";
                strType = "text,hidden,hidden,hidden,hidden,hidden,hidden,hidden";
                strValu = "'" + forumid + "'" + bid + "'" + reid + "'" + id + "'delfile'ok'" + Utils.getPage(0) + "";
                strEmpt = "true,false,false,false,false,false,false,false";
                strIdea = "/";
                strOthe = "确定删除,filelist.aspx,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getUrl("filelist.aspx?act=file&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + reid + "&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">取消</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("filelist.aspx?info=ok&amp;act=delfile&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + reid + "&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("filelist.aspx?act=file&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + reid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        else
        {
            new BCW.BLL.Upfile().Delete(id);
            //删除文件
            BCW.Files.FileTool.DeleteFile(model.Files);
            if (!string.IsNullOrEmpty(model.PrevFiles))
                BCW.Files.FileTool.DeleteFile(model.PrevFiles);
            string strRe = string.Empty;
            //减去文件数
            if (reid > 0)
            {
                new BCW.BLL.Reply().UpdateFileNum(reid, -1);
                strRe = "(回复ID:" + reid + ")";
            }
            else
            {
                new BCW.BLL.Text().UpdateFileNum(bid, -1);
                int FileNum = new BCW.BLL.Text().GetFileNum(bid);
                if (FileNum == 0)
                {
                    //去掉附件帖标识
                    new BCW.BLL.Text().UpdateTypes(bid, 0);
                }
            }
            //记录日志
            string Why = Utils.GetRequest("Why", "post", 3, @"^[^\^]{1,20}$", "理由限20字内，可留空");
            string strLog = string.Empty;
            if (model.UsID != uid)
                strLog = "[url=/bbs/uinfo.aspx?uid=" + model.UsID + "]" + new BCW.BLL.User().GetUsName(model.UsID) + "[/url]的主题[url=/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "]《" + new BCW.BLL.Text().GetTitle(bid) + "》[/url]" + strRe + "附件被[url=/bbs/uinfo.aspx?uid=" + model.UsID + "]" + new BCW.BLL.User().GetUsName(uid) + "[/url]删除!";
            else
                strLog = "[url=/bbs/uinfo.aspx?uid=" + model.UsID + "]" + new BCW.BLL.User().GetUsName(model.UsID) + "[/url]自删主题[url=/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "]《" + new BCW.BLL.Text().GetTitle(bid) + "》[/url]的" + strRe + "附件!";
                
            new BCW.BLL.Forumlog().Add(7, forumid, strLog);
            Utils.Success("删除附件", "删除附件成功，正在返回..", Utils.getUrl("filelist.aspx?act=file&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + reid + "&amp;backurl=" + Utils.getPage(0) + ""), "1");

        }
    }

}