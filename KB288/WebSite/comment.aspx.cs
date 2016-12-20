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
public partial class comment : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/front.xml";
    protected void Page_Load(object sender, EventArgs e)
    {
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-9]\d*$", "0"));
        string Title = "";
        if (ptype == 0)
        {
            Title = new BCW.BLL.Detail().GetTitle(id);

        }
        else
        {
            Title = new BCW.BLL.Goods().GetTitle(id);
        }
        if (Title == "")
        {
            Utils.Error("不存在的记录", "");
        }
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "add":
                AddComment(id, ptype);
                break;
            case "admin":
                AdminComment(Title, id, ptype);
                break;
            case "del":
                DelComment(Title, id, ptype);
                break;
            case "reok":
                ReOkComment(Title, id, ptype);
                break;
            case "ok":
                SaveComment(id, ptype);
                break;
            default:
                ReloadPage(Title, id, ptype);
                break;
        }
    }
    private void ReloadPage(string Title, int id, int ptype)
    {
        int meid = new BCW.User.Users().GetUsId();
        Master.Title = "网友评论";
        builder.Append(Out.Tab("<div class=\"title\">网友评论</div>", ""));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("comment.aspx?act=add&amp;ptype=" + ptype + "&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;&gt;发表评论</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "ptype", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "DetailId=" + id + "";

        // 开始读取列表
        IList<BCW.Model.Comment> listComment = new BCW.BLL.Comment().GetComments(pageIndex, pageSize, strWhere, out recordCount);
        if (listComment.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Comment n in listComment)
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
                builder.AppendFormat("{0}.{1}:{2}({3})", recordCount - (((pageIndex - 1) * pageSize) + (k - 1)), n.UserName, n.Content, DT.FormatDate(n.AddTime, 1));
                if (!string.IsNullOrEmpty(n.ReText))
                {
                    builder.Append(Out.Tab("<font color=\"red\">", ""));
                    builder.Append("<br />★管理员回复:" + n.ReText + "");
                    builder.Append(Out.Tab("</font>", ""));
                }
                if (("#" + ub.GetSub("FtCommentAdmin", xmlPath) + "#").Contains("#" + meid + "#"))
                {
                    builder.Append("<a href=\"" + Utils.getUrl("comment.aspx?act=admin&amp;ptype=" + ptype + "&amp;id=" + id + "&amp;pid=" + n.ID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[管理]</a>");
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
        if (ptype == 0)
            builder.Append("<a href=\"" + Utils.getUrl("detail.aspx?id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;&gt;" + Title + "</a>");
        else
            builder.Append("<a href=\"" + Utils.getUrl("shopdetail.aspx?id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;&gt;" + Title + "</a>");

        builder.Append(Out.Tab("</div>", ""));

    }
    private void AddComment(int id, int ptype)
    {
        if (ub.GetSub("FtCommentIsUser", xmlPath) == "2")
        {
            Utils.Error("评论已关闭", "");
        }
        int meid = new BCW.User.Users().GetUsId();
        if (ub.GetSub("FtCommentIsUser", xmlPath) == "1")
        {
            if (meid == 0)
                Utils.Login();

            BCW.User.Users.ShowVerifyRole("l", meid);//非验证会员提示
        }

        string mename = "网友";
        Master.Title = "发表评论";
        builder.Append(Out.Tab("<div class=\"title\">发表评论</div>", "发表评论"));

        string strText, strName, strType, strValu, strEmpt, strIdea, strOthe;
        if (meid == 0)
        {
            strText = "您的昵称:/,评论内容.限" + ub.GetSub("FtCommentLength", xmlPath) + "字:/,,,,";
            strType = "text,textarea,hidden,hidden,hidden,hidden";
        }
        else
        {
            strText = ",评论内容.限" + ub.GetSub("FtCommentLength", xmlPath) + "字:/,,,,";
            strType = "hidden,textarea,hidden,hidden,hidden,hidden";
        }
        strName = "UsName,Content,id,ptype,act,backurl";
        strValu = "" + mename + "''" + id + "'" + ptype + "'ok'" + Utils.getPage(0) + "";
        strEmpt = "true,true,false,false,false,false";
        strIdea = "/";
        strOthe = "发表评论|reset,comment.aspx,post,1,red|blue";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("comment.aspx?ptype=" + ptype + "&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">取消评论</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void SaveComment(int id, int ptype)
    {
        if (ub.GetSub("FtCommentIsUser", xmlPath) == "2")
        {
            Utils.Error("评论已关闭", "");
        }
        int meid = new BCW.User.Users().GetUsId();
        if (ub.GetSub("FtCommentIsUser", xmlPath) == "1")
        {
            if (meid == 0)
                Utils.Login();
        }
        Master.Title = "发表评论";
        new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Comment, meid);//会员本身权限
        builder.Append(Out.Tab("<div class=\"title\">发表评论</div>", "发表评论"));
        string mename = string.Empty;
        if (meid == 0)
        {
            mename = Utils.GetRequest("UsName", "post", 3, @"^[A-Za-zＡ-Ｚ\d０-９\u4E00-\u9FA5]{1,7}$", "请输入不超过7个字的昵称，不能有特殊符号");
        }
        else
        {
            mename = new BCW.BLL.User().GetUsName(meid);
        }
        if (string.IsNullOrEmpty(mename))
            mename = "网友";

        string Content = Utils.GetRequest("Content", "post", 2, @"^[\s\S]{1," + ub.GetSub("FtCommentLength", xmlPath) + "}$", "请输入不超过" + ub.GetSub("FtCommentLength", xmlPath) + "字的评论内容");

        //是否刷屏
        string appName = "LIGHT_COMMENT";
        int Expir = Convert.ToInt32(ub.GetSub("FtCommentExpir", xmlPath));
        BCW.User.Users.IsFresh(appName, Expir);

        //写入评论
        BCW.Model.Comment model = new BCW.Model.Comment();
        model.NodeId = new BCW.BLL.Detail().GetNodeId(id);
        if (ptype == 0)
            model.Types = new BCW.BLL.Detail().GetTypes(id);
        else
            model.Types = 14;

        model.DetailId = id;
        model.UserId = meid;
        model.UserName = mename;
        model.Face = 0;
        model.Content = Content;
        model.AddUsIP = Utils.GetUsIP();
        model.AddTime = DateTime.Now;
        new BCW.BLL.Comment().Add(model);
        //更新评论条数
        if (ptype == 0)
            new BCW.BLL.Detail().UpdateRecount(id, 1);
        else
            new BCW.BLL.Goods().UpdateRecount(id, 1);

        Utils.Success("发表评论", "发表成功，正在返回..", Utils.getUrl("comment.aspx?ptype=" + ptype + "&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
    }

    private void AdminComment(string Title, int id, int ptype)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        if (!("#" + ub.GetSub("FtCommentAdmin", xmlPath) + "#").Contains("#" + meid + "#"))
        {
            Utils.Error("你的权限不足", "");
        }
        int pid = int.Parse(Utils.GetRequest("pid", "all", 2, @"^[0-9]\d*$", "评论ID错误"));
        if (!new BCW.BLL.Comment().Exists(pid))
        {
            Utils.Error("不存在的评论记录", "");
        }
        Master.Title = "管理评论";
        builder.Append(Out.Tab("<div class=\"title\">管理评论</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("comment.aspx?act=reok&amp;ptype=" + ptype + "&amp;id=" + id + "&amp;pid=" + pid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">回复评论</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("comment.aspx?act=del&amp;ptype=" + ptype + "&amp;id=" + id + "&amp;pid=" + pid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">删除评论</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("comment.aspx?ptype=" + ptype + "&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        if (ptype == 0)
            builder.Append("<a href=\"" + Utils.getUrl("detail.aspx?id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;&gt;" + Title + "</a>");
        else
            builder.Append("<a href=\"" + Utils.getUrl("shopdetail.aspx?id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;&gt;" + Title + "</a>");

        builder.Append(Out.Tab("</div>", ""));
    }

    private void ReOkComment(string Title, int id, int ptype)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        if (!("#" + ub.GetSub("FtCommentAdmin", xmlPath) + "#").Contains("#" + meid + "#"))
        {
            Utils.Error("你的权限不足", "");
        }
        int pid = int.Parse(Utils.GetRequest("pid", "all", 2, @"^[0-9]\d*$", "评论ID错误"));
        if (!new BCW.BLL.Comment().Exists(pid))
        {
            Utils.Error("不存在的评论记录", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {
            string Content = Utils.GetRequest("Content", "post", 3, @"^[\s\S]{1,500}$", "回复评论内容限1-500字内");
            new BCW.BLL.Comment().UpdateReText(pid, Content);
            Utils.Success("回复评论", "回复评论成功，正在返回..", Utils.getUrl("comment.aspx?ptype=" + ptype + "&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            Master.Title = "回复评论";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("回复评论");
            builder.Append(Out.Tab("</div>", ""));

            string strText, strName, strType, strValu, strEmpt, strIdea, strOthe;
            strText = "回复内容:/,,,,,,";
            strType = "textarea,hidden,hidden,hidden,hidden,hidden,hidden";
            strName = "Content,id,pid,ptype,act,info,backurl";
            strValu = "'" + id + "'" + pid + "'" + ptype + "'reok'ok'" + Utils.getPage(0) + "";
            strEmpt = "true,false,false,false,false,false,false";
            strIdea = "/";
            strOthe = "确定回复|reset,comment.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", " "));
            builder.Append("<a href=\"" + Utils.getUrl("comment.aspx?ptype=" + ptype + "&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">取消</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            if (ptype == 0)
                builder.Append("<a href=\"" + Utils.getUrl("detail.aspx?id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;&gt;" + Title + "</a>");
            else
                builder.Append("<a href=\"" + Utils.getUrl("shopdetail.aspx?id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;&gt;" + Title + "</a>");

            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private void DelComment(string Title, int id, int ptype)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        if (!("#" + ub.GetSub("FtCommentAdmin", xmlPath) + "#").Contains("#" + meid + "#"))
        {
            Utils.Error("你的权限不足", "");
        }

        string info = Utils.GetRequest("info", "all", 1, "", "");
        int pid = int.Parse(Utils.GetRequest("pid", "all", 2, @"^[0-9]\d*$", "评论ID错误"));
        if (info != "ok")
        {
            Master.Title = "删除评论";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此评论记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("comment.aspx?info=ok&amp;act=del&amp;ptype=" + ptype + "&amp;id=" + id + "&amp;pid=" + pid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("comment.aspx?ptype=" + ptype + "&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            if (!new BCW.BLL.Comment().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            //删除
            new BCW.BLL.Comment().Delete(pid);
            //减主题评论数
            if (ptype == 0)
                new BCW.BLL.Detail().UpdateRecount(id, -1);
            else
                new BCW.BLL.Goods().UpdateRecount(id, -1);

            Utils.Success("删除评论", "删除成功，正在返回..", Utils.getUrl("comment.aspx?ptype=" + ptype + "&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
    }
}