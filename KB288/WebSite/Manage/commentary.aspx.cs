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
public partial class Manage_commentary : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "评论管理";

        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "comment":
                CommentPage(act);
                break;
            case "fcomment":
                FCommentPage(act);
                break;
            case "del":
                DelPage();
                break;
            case "del2":
                Del2Page();
                break;
            case "delpage":
                DelpagePage();
                break;
            case "delpage2":
                Delpage2Page();
                break;
            case "reok":
                ReOkPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("日志管理");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("主站评论");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("commentary.aspx?act=comment&amp;ptype=1") + "\">文章</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("commentary.aspx?act=comment&amp;ptype=2") + "\">图片</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("commentary.aspx?act=comment&amp;ptype=3") + "\">文件</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("commentary.aspx?act=comment&amp;ptype=4") + "\">商品</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("社区评论");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("commentary.aspx?act=fcomment&amp;ptype=1") + "\">日记评论</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("commentary.aspx?act=fcomment&amp;ptype=2") + "\">相册评论</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void CommentPage(string act)
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-9]\d*$", "0"));
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[0-9]\d*$", "0"));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[0-9]\d*$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("主站评论管理");
        builder.Append(Out.Tab("</div>", "<br />"));
        if (id != 0)
        {
            if (!new BCW.BLL.Detail().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            string Title = new BCW.BLL.Detail().GetTitle(id);
            builder.Append(Out.Tab("<div class=\"text\">", ""));

            builder.Append("<a href=\"" + Utils.getUrl("../detail.aspx?id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">.." + Title + "</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));

            if (ptype == 0)
                builder.Append("全部|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("commentary.aspx?act=" + act + "&amp;id=" + id + "&amp;uid=" + uid + "&amp;ptype=0&amp;backurl=" + Utils.getPage(0) + "") + "\">全部</a>|");

            if (ptype == 1)
                builder.Append("文章|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("commentary.aspx?act=" + act + "&amp;id=" + id + "&amp;uid=" + uid + "&amp;ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">文章</a>|");

            if (ptype == 2)
                builder.Append("图片|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("commentary.aspx?act=" + act + "&amp;id=" + id + "&amp;uid=" + uid + "&amp;ptype=2&amp;backurl=" + Utils.getPage(0) + "") + "\">图片</a>|");

            if (ptype == 3)
                builder.Append("文件|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("commentary.aspx?act=" + act + "&amp;id=" + id + "&amp;uid=" + uid + "&amp;ptype=3&amp;backurl=" + Utils.getPage(0) + "") + "\">文件</a>|");
            
            if (ptype == 4)
                builder.Append("商品");
            else
                builder.Append("<a href=\"" + Utils.getUrl("commentary.aspx?act=" + act + "&amp;id=" + id + "&amp;uid=" + uid + "&amp;ptype=4&amp;backurl=" + Utils.getPage(0) + "") + "\">商品</a>");

            builder.Append(Out.Tab("</div>", "<br />"));
        }
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        string arrId = string.Empty;
        string[] pageValUrl = { "act", "ptype", "id", "uid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        if (id == 0)
        {
            if (ptype != 0)
            {
                strWhere += "Types=" + (ptype + 10) + "";
            }
            if (uid != 0)
            {
                if (strWhere == "")
                    strWhere += "userid=" + uid + "";
                else
                    strWhere += "and userid=" + uid + "";
            }
        }
        else
        {
            strWhere = "DetailId=" + id + "";
        }
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
                string sFace = string.Empty;
                //if (n.Face != 0)
                //    sFace = "<img src=\"/Files/face/" + n.Face + ".gif\" alt=\"load\"/>";
                builder.AppendFormat("<a href=\"" + Utils.getUrl("commentary.aspx?act=del&amp;ptype=" + ptype + "&amp;id={0}&amp;backurl=" + Utils.PostPage(true) + "") + "\">[删]</a>{1}.{2}:{3}{4}({5})", n.ID, (pageIndex - 1) * pageSize + k, n.UserName, sFace, n.Content, DT.FormatDate(n.AddTime, 1));
                if (!string.IsNullOrEmpty(n.ReText))
                {
                    builder.Append(Out.Tab("<font color=\"red\">", ""));
                    builder.Append("<br />★管理员回复:" + n.ReText + "");
                    builder.Append(Out.Tab("</font>", ""));
                }
                builder.Append("<a href=\"" + Utils.getUrl("commentary.aspx?act=reok&amp;ptype=" + ptype + "&amp;pid=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[回]</a>");

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
        if (id == 0)
        {
            builder.Append(Out.Tab("</div>", ""));
            string strText = "输入用户ID:/,,";
            string strName = "uid,ptype,act";
            string strType = "num,hidden,hidden";
            string strValu = "'" + ptype + "'comment";
            string strEmpt = "true,false,false";
            string strIdea = "/";
            string strOthe = "搜评论,commentary.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        if (Utils.getPage(1) != "")
        {
            builder.Append(" <a href=\"" + Utils.getPage(1) + "\">返回上一级</a><br />");
        }
        builder.Append("<a href=\"" + Utils.getUrl("commentary.aspx?act=delpage&amp;ptype=" + ptype + "&amp;uid=" + uid + "&amp;id=" + id + "&amp;page=" + pageIndex + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">删除本页评论</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("commentary.aspx") + "\">返回评论管理</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void ReOkPage()
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-9]\d*$", "0"));
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
            Utils.Success("回复评论", "回复评论成功，正在返回..", Utils.getUrl("commentary.aspx?act=comment&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            Master.Title = "回复评论";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("回复评论");
            builder.Append(Out.Tab("</div>", ""));

            string strText, strName, strType, strValu, strEmpt, strIdea, strOthe;
            strText = "回复内容:/,,,,,";
            strType = "textarea,hidden,hidden,hidden,hidden,hidden";
            strName = "Content,pid,ptype,act,info,backurl";
            strValu = "'" + pid + "'" + ptype + "'reok'ok'" + Utils.getPage(0) + "";
            strEmpt = "true,false,false,false,false,false";
            strIdea = "/";
            strOthe = "确定回复|reset,commentary.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", " "));
            builder.Append("<a href=\"" + Utils.getUrl("commentary.aspx?act=comment&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">取消</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }


    private void DelPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-9]\d*$", "0"));
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        if (info != "ok")
        {
            Master.Title = "删除评论";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此评论记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("commentary.aspx?info=ok&amp;act=del&amp;ptype=" + ptype + "&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("commentary.aspx?act=comment") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            BCW.Model.Comment model = new BCW.BLL.Comment().GetCommentMe(id);
            if (model == null)
            {
                Utils.Error("不存在的记录", "");
            }
            //删除
            new BCW.BLL.Comment().Delete(id);
            //减主题评论数
            if (model.Types != 14)
                new BCW.BLL.Detail().UpdateRecount(model.DetailId, -1);
            else
                new BCW.BLL.Goods().UpdateRecount(model.DetailId, -1);

            Utils.Success("删除成功", "删除评论成功..", Utils.getUrl("commentary.aspx?act=comment&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
    }

    private void DelpagePage()
    {

        Master.Title = "删除本页评论";
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-9]\d*$", "0"));
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[0-9]\d*$", "0"));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[0-9]\d*$", "0"));
        int page = int.Parse(Utils.GetRequest("page", "get", 1, @"^[1-9]\d*$", "页面ID错误"));
        string info = Utils.GetRequest("info", "get", 1, "", "");
        if (info != "ok")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除本页评论吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("commentary.aspx?act=delpage&amp;info=ok&amp;ptype=" + ptype + "&amp;id=" + id + "&amp;uid=" + uid + "&amp;page=" + page + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("commentary.aspx?act=comment&amp;ptype=" + ptype + "") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));

            string strWhere = "";
            pageIndex = page;
            if (pageIndex == 0)
                pageIndex = 1;

            //查询条件
            if (id == 0)
            {
                if (ptype != 0)
                {
                    strWhere += "Types=" + (ptype + 10) + "";
                }
                if (uid != 0)
                {
                    if (strWhere == "")
                        strWhere += "userid=" + uid + "";
                    else
                        strWhere += "and userid=" + uid + "";
                }
            }
            else
            {
                strWhere = "DetailId=" + id + "";
            }

            // 开始读取列表
            IList<BCW.Model.Comment> listComment = new BCW.BLL.Comment().GetComments(pageIndex, pageSize, strWhere, out recordCount);
            if (listComment.Count > 0)
            {
                foreach (BCW.Model.Comment n in listComment)
                {
                    //删除
                    new BCW.BLL.Comment().Delete(n.ID);
                    //减主题评论数
                    if (n.Types != 14)
                        new BCW.BLL.Detail().UpdateRecount(n.DetailId, -1);
                    else
                        new BCW.BLL.Goods().UpdateRecount(n.DetailId, -1);
                }
            }

            Utils.Success("删除本页评论", "删除本页评论成功，正在返回..", Utils.getUrl("commentary.aspx?act=comment&amp;ptype=" + ptype + ""), "1");
        }
    }

    private void FCommentPage(string act)
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]\d*$", "1"));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[0-9]\d*$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("社区评论管理");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));

        if (ptype == 1)
            builder.Append("日记评论|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("commentary.aspx?act=" + act + "&amp;uid=" + uid + "&amp;ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">日记</a>|");

        if (ptype == 2)
            builder.Append("相册评论");
        else
            builder.Append("<a href=\"" + Utils.getUrl("commentary.aspx?act=" + act + "&amp;uid=" + uid + "&amp;ptype=2&amp;backurl=" + Utils.getPage(0) + "") + "\">相册</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        string arrId = string.Empty;
        string[] pageValUrl = { "act", "ptype", "uid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件

        if (ptype == 1)
        {
            strWhere += "Types=0";
        }
        else
        {
            strWhere += "Types>0";
        }
        if (uid != 0)
        {
            if (strWhere == "")
                strWhere += "usid=" + uid + "";
            else
                strWhere += "and usid=" + uid + "";
        }

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
                builder.AppendFormat("<a href=\"" + Utils.getUrl("commentary.aspx?act=del2&amp;id={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">[删]</a>{1}.{2}:{3}({4})", n.ID, (pageIndex - 1) * pageSize + k, n.UsName, n.Content, DT.FormatDate(n.AddTime, 1));
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

        builder.Append(Out.Tab("</div>", ""));
        string strText = "输入用户ID:/,,";
        string strName = "uid,ptype,act";
        string strType = "num,hidden,hidden";
        string strValu = "'" + ptype + "'fcomment";
        string strEmpt = "true,false,false";
        string strIdea = "/";
        string strOthe = "搜评论,commentary.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("commentary.aspx?act=delpage2&amp;ptype=" + ptype + "&amp;uid=" + uid + "&amp;page=" + pageIndex + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">删除本页评论</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("commentary.aspx") + "\">返回评论管理</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void Del2Page()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        if (info != "ok")
        {
            Master.Title = "删除评论";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此评论记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("commentary.aspx?info=ok&amp;act=del2&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("commentary.aspx?act=fcomment") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.BLL.FComment().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            //删除
            new BCW.BLL.FComment().Delete(id);
            Utils.Success("删除成功", "删除评论成功..", Utils.getPage("commentary.aspx?act=fcomment"), "1");
        }
    }

    private void Delpage2Page()
    {

        Master.Title = "删除本页评论";
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]\d*$", "1"));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[0-9]\d*$", "0"));
        int page = int.Parse(Utils.GetRequest("page", "get", 1, @"^[1-9]\d*$", "页面ID错误"));
        string info = Utils.GetRequest("info", "get", 1, "", "");
        if (info != "ok")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除本页评论吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("commentary.aspx?act=delpage2&amp;info=ok&amp;ptype=" + ptype + "&amp;uid=" + uid + "&amp;page=" + page + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("commentary.aspx?act=fcomment&amp;ptype=" + ptype + "") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));

            string strWhere = "";
            pageIndex = page;
            if (pageIndex == 0)
                pageIndex = 1;

            //查询条件
            if (ptype == 1)
            {
                strWhere += "Types=0";
            }
            else
            {
                strWhere += "Types>0";
            }
            if (uid != 0)
            {
                if (strWhere == "")
                    strWhere += "usid=" + uid + "";
                else
                    strWhere += "and usid=" + uid + "";
            }

            // 开始读取列表
            IList<BCW.Model.FComment> listFComment = new BCW.BLL.FComment().GetFComments(pageIndex, pageSize, strWhere, out recordCount);
            if (listFComment.Count > 0)
            {
                foreach (BCW.Model.FComment n in listFComment)
                {
                    //删除
                    new BCW.BLL.FComment().Delete(n.ID);
                }
            }

            Utils.Success("删除本页评论", "删除本页评论成功，正在返回..", Utils.getUrl("commentary.aspx?act=fcomment&amp;ptype=" + ptype + ""), "1");
        }
    }

}