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

public partial class book_bookps : System.Web.UI.Page
{
    protected string xmlPath = "/Controls/book.xml";
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string AdminIDS = ub.GetSub("BookAdminIDS", xmlPath);
        if (!("#" + AdminIDS + "#").Contains("#" + meid + "#"))
        {
            Utils.Error("权限不足", "");
        }

        Master.Title = "审核专区";
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "editshumu":
                EditShuMuPage();
                break;
            case "editshumusave":
                EditShuMuSavePage();
                break;
            case "delshumu":
                DelShuMuPage();
                break;
            case "view":
                ViewPage();
                break;
            case "editjuan":
                EditJuanPage();
                break;
            case "editjuansave":
                EditJuanSavePage();
                break;
            case "deljuan":
                DelJuanPage();
                break;
            case "editcontent":
                EditContentPage();
                break;
            case "editcontentsave":
                EditContentSavePage();
                break;
            case "delcontent":
                DelContentPage();
                break;
            case "ps":
                PsPage();
                break;
            case "psok":
                PsOkPage();
                break;
            case "psok2":
                PsOk2Page();
                break;
            default:
                PsPage();
                break;
        }
    }

    private void PsPage()
    {
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 1, @"^[0-3]$", "0"));

        Master.Title = "管理未审核";

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("管理未审核");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 0)
            builder.Append("未审书本|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("bookps.aspx?act=ps&amp;ptype=0") + "\">未审书本</a>|");

        if (ptype == 1)
            builder.Append("未审章节|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("bookps.aspx?act=ps&amp;ptype=1") + "\">未审章节</a>|");

        if (ptype == 2)
            builder.Append("已审书本|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("bookps.aspx?act=ps&amp;ptype=2") + "\">已审书本</a>|");

        if (ptype == 3)
            builder.Append("已审章节");
        else
            builder.Append("<a href=\"" + Utils.getUrl("bookps.aspx?act=ps&amp;ptype=3") + "\">已审章节</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        strWhere = "isdel=0";
        // 开始读取列表
        if (ptype == 0 ||ptype==2)
        {
            if (ptype == 2)
            {
                strWhere += " and state=1";
            }
            else
            {
                strWhere += " and state=0";
            }
            IList<Book.Model.ShuMu> listShuMu = new Book.BLL.ShuMu().GetShuMus(pageIndex, pageSize, strWhere, out recordCount);
            if (listShuMu.Count > 0)
            {
                int k = 1;
                foreach (Book.Model.ShuMu n in listShuMu)
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
                    builder.Append("<a href=\"" + Utils.getUrl("bookps.aspx?act=editshumu&amp;id=" + n.nid + "&amp;nid=" + n.id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[管理]&gt;</a>");
                    builder.AppendFormat("{0}.<a href=\"" + Utils.getUrl("bookps.aspx?act=view&amp;id={1}&amp;nid={2}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{3}</a>", (pageIndex - 1) * pageSize + k, n.nid, n.id, n.title);
                    if (ptype == 2)
                        builder.Append("[<a href=\"" + Utils.getUrl("bookps.aspx?act=psok&amp;p=0&amp;id=" + n.id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">取消审核</a>]");
                    else
                        builder.Append("[<a href=\"" + Utils.getUrl("bookps.aspx?act=psok&amp;p=1&amp;id=" + n.id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">审核</a>]");

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
        }
        else
        {
            if (ptype == 3)
            {
                strWhere += " and state=1";
            }
            else
            {
                strWhere += " and state=0";
            }
            IList<Book.Model.Contents> listContents = new Book.BLL.Contents().GetContentss(pageIndex, pageSize, strWhere, out recordCount);
            if (listContents.Count > 0)
            {
                int k = 1;
                foreach (Book.Model.Contents n in listContents)
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
                    int nid = new Book.BLL.ShuMu().Getnid(n.shi);
                    builder.Append("<a href=\"" + Utils.getUrl("bookps.aspx?act=editcontent&amp;id=" + nid + "&amp;nid=" + n.shi + "&amp;jid=" + n.id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[管理]&gt;</a>");
                    builder.AppendFormat("{0}.<a href=\"" + Utils.getUrl("bookps.aspx?act=view&amp;id={1}&amp;nid={2}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{3}</a>", (pageIndex - 1) * pageSize + k, nid, n.shi, n.title);
                    if (ptype == 3)
                        builder.Append("[<a href=\"" + Utils.getUrl("bookps.aspx?act=psok&amp;ptype=1&amp;p=0&amp;id=" + n.id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">取消审核</a>]");
                    else
                        builder.Append("[<a href=\"" + Utils.getUrl("bookps.aspx?act=psok&amp;ptype=1&amp;p=1&amp;&amp;id=" + n.id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">审核</a>]");

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
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        if (ptype < 2)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("bookps.aspx?act=psok2&amp;ptype=" + ptype + "&amp;page=" + pageIndex + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[审核本页记录]</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回书城中心</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void PsOkPage()
    {
        int p = Utils.ParseInt(Utils.GetRequest("p", "all", 2, @"^[0-1]$", "选择错误"));
        int id = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 1, @"^[0-1]$", "0"));

        if (ptype == 0)
        {
            Book.Model.ShuMu m = new Book.BLL.ShuMu().GetShuMu(id);
            if (m == null)
            {
                Utils.Error("不存在的书本", "");
            }
            if (m.isdel == 1)
            {
                Utils.Error("不存在的记录", "");
            }
            new Book.BLL.ShuMu().Updatestate(id, p);
        }
        else
        {
            Book.Model.Contents n = new Book.BLL.Contents().GetContents(id);
            if (n == null)
            {
                Utils.Error("不存在的分卷", "");
            }
            if (n.isdel == 1)
            {
                Utils.Error("不存在的记录", "");
            }
            new Book.BLL.Contents().Updatestate(id, p);
        }
        if (p == 1)
            Utils.Success("审核", "审核成功，正在返回..", Utils.getPage("bookps.aspx?act=ps&amp;ptype=" + ptype + ""), "1");
        else
            Utils.Success("取消审核", "取消审核成功，正在返回..", Utils.getPage("bookps.aspx?act=ps&amp;ptype=" + ptype + ""), "1");

    }

    private void PsOk2Page()
    {

        Master.Title = "审核本页记录";
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 1, @"^[0-1]$", "0"));
        int page = int.Parse(Utils.GetRequest("page", "get", 1, @"^[1-9]\d*$", "页面ID错误"));
        string info = Utils.GetRequest("info", "get", 1, "", "");
        if (info != "ok")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定审核本页记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("bookps.aspx?act=psok2&amp;info=ok&amp;ptype=" + ptype + "&amp;page=" + page + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定审核</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("bookps.aspx?act=ps&amp;ptype=" + ptype + "") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
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

            strWhere = "state=0 and isdel=0";

            // 开始读取列表
            if (ptype == 0)
            {
                IList<Book.Model.ShuMu> listShuMu = new Book.BLL.ShuMu().GetShuMus(pageIndex, pageSize, strWhere, out recordCount);
                if (listShuMu.Count > 0)
                {
                    foreach (Book.Model.ShuMu n in listShuMu)
                    {
                        new Book.BLL.ShuMu().Updatestate(n.id,1);
                    }
                }
            }
            else
            {
                IList<Book.Model.Contents> listContents = new Book.BLL.Contents().GetContentss(pageIndex, pageSize, strWhere, out recordCount);
                if (listContents.Count > 0)
                {
                    foreach (Book.Model.Contents n in listContents)
                    {
                        new Book.BLL.Contents().Updatestate(n.id,1);
                    }
                }
            }

            Utils.Success("审核本页记录", "审核本页记录成功，正在返回..", Utils.getPage("bookps.aspx?act=ps&amp;ptype=" + ptype + ""), "1");
        }
    }


    private void EditShuMuPage()
    {
        int id = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int nid = Utils.ParseInt(Utils.GetRequest("nid", "all", 2, @"^[0-9]\d*$", "ID错误"));
        Book.Model.Navigation model = new Book.BLL.Navigation().GetNavigation(id);
        if (model == null)
        {
            Utils.Error("不存在的分类", "");
        }
        Book.Model.ShuMu m = new Book.BLL.ShuMu().GetShuMu(nid);
        if (m == null)
        {
            Utils.Error("不存在的书本", "");
        }
        if (m.isdel == 1)
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "编辑书本";

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("bookps.aspx?act=list&amp;id=" + id + "") + "\">" + model.Name + "</a>&gt;");
        builder.Append("编辑书本");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "书本名称:/,书本简介:/,书本封面:/,书本作者:/,书本字数(未知请填0):/,搜索关键词(多个用#分开):/,书本状态:/,书本性质:/,上传会员ID(没有请填0):/,添加时间:/,是否完结:/,,,,";
        string strName = "title,summary,img,author,length,tags,state,types,aid,addtime,isover,id,nid,act,backurl";
        string strType = "text,textarea,text,text,num,text,hidden,select,hidden,hidden,select,hidden,hidden,hidden,hidden";
        string strValu = "" + m.title + "'" + m.summary + "'" + m.img + "'" + m.author + "'" + m.length + "'" + m.tags + "'" + m.state + "'" + m.types + "'" + m.aid + "'" + DT.FormatDate(m.addtime, 0) + "'" + m.isover + "'" + id + "'" + nid + "'editshumusave'" + Utils.getPage(0) + "";
        string strEmpt = "true,true,true,true,true,true,0|未审核|1|已审核,0|原创|1|转载,false,false,0|连载中|1|已完结,false,false,false,false";
        string strIdea = "/";
        string strOthe = "修改书本,bookps.aspx,post,2,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("bookps.aspx?act=delshumu&amp;id=" + id + "&amp;nid=" + nid + "") + "\">删除书本</a><br />");
        builder.Append("<a href=\"" + Utils.getPage("bookps.aspx?act=list&amp;id=" + id + "") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回书城中心</a>");
        builder.Append(Out.Tab("</div>", ""));
    }


    private void EditShuMuSavePage()
    {
        int id = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int nid = Utils.ParseInt(Utils.GetRequest("nid", "all", 2, @"^[0-9]\d*$", "ID错误"));
        string title = Utils.GetRequest("title", "all", 2, @"^[\s\S]{1,200}$", "书本名称限1-200字");
        string summary = Utils.GetRequest("summary", "all", 2, @"^[\s\S]{1,500}$", "书本简介限1-500字");
        string img = Utils.GetRequest("img", "all", 2, @"^[\s\S]{1,200}$", "书本封面地址填写出错");
        string author = Utils.GetRequest("author", "all", 2, @"^[\s\S]{1,50}$", "书本作者限1-50字");
        int length = Utils.ParseInt(Utils.GetRequest("length", "all", 2, @"^[0-9]\d*$", "字数填写错误"));
        string tags = Utils.GetRequest("tags", "all", 2, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "多个搜索关键词请用#分开");
        //int state = Utils.ParseInt(Utils.GetRequest("state", "all", 2, @"^[0-1]$", "书本状态选择错误"));
        int types = Utils.ParseInt(Utils.GetRequest("types", "all", 2, @"^[0-1]$", "书本性质选择错误"));
        int aid = Utils.ParseInt(Utils.GetRequest("aid", "all", 2, @"^[0-9]\d*$", "上传会员ID错误"));
        DateTime addtime = Utils.ParseTime(Utils.GetRequest("addtime", "post", 2, DT.RegexTime, "添加时间填写错误"));
        int isover = Utils.ParseInt(Utils.GetRequest("isover", "all", 2, @"^[0-1]$", "是否完结选择错误"));

        Book.Model.ShuMu m = new Book.BLL.ShuMu().GetShuMu(nid);
        if (m == null)
        {
            Utils.Error("不存在的书本", "");
        }
        if (m.isdel == 1)
        {
            Utils.Error("不存在的记录", "");
        }
        int state = Utils.ParseInt(ub.GetSub("BookIsShuMu2", xmlPath));

        Book.Model.ShuMu model = new Book.Model.ShuMu();
        model.id = nid;
        model.nid = id;
        model.title = title;
        model.summary = summary;
        model.img = img;
        model.author = author;
        model.length = length;
        model.tags = tags;
        model.state = state;
        model.addtime = addtime;
        model.aid = aid;
        model.types = types;
        model.isover = isover;
        new Book.BLL.ShuMu().Update(model);
        Utils.Success("编辑书本", "修改书本成功，正在返回..", Utils.getPage("bookps.aspx?act=list&amp;id=" + id + ""), "1");
    }

    private void DelShuMuPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        int nid = int.Parse(Utils.GetRequest("nid", "get", 2, @"^[0-9]\d*$", "ID错误"));
        Book.Model.ShuMu m = new Book.BLL.ShuMu().GetShuMu(nid);
        if (m == null)
        {
            Utils.Error("不存在的书本", "");
        }
        if (m.isdel == 1)
        {
            Utils.Error("不存在的记录", "");
        }
        if (info != "ok")
        {
            Master.Title = "删除书本";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("删除书本将会删除本书分卷和章节，确定删除此书本记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("bookps.aspx?info=ok&amp;act=delshumu&amp;id=" + id + "&amp;nid=" + nid + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("bookps.aspx?act=editshumu&amp;id=" + id + "&amp;nid=" + nid + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回书城中心</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            //删除
            new Book.BLL.ShuMu().Delete(nid);
            new Book.BLL.Contents().Delete("shi=" + nid + "");
            Utils.Success("删除书本", "删除书本成功..", Utils.getUrl("bookps.aspx?act=list&amp;id=" + id + ""), "1");
        }
    }

    private void ViewPage()
    {
        int id = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int nid = Utils.ParseInt(Utils.GetRequest("nid", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int jid = Utils.ParseInt(Utils.GetRequest("jid", "all", 1, @"^[0-9]\d*$", "0"));

        Book.Model.Navigation model = new Book.BLL.Navigation().GetNavigation(id);
        if (model == null)
        {
            Utils.Error("不存在的分类", "");
        }
        Book.Model.ShuMu m = new Book.BLL.ShuMu().GetShuMu(nid);
        if (m == null)
        {
            Utils.Error("不存在的书本", "");
        }
        if (m.isdel == 1)
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "查看书本";

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("查看书本");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append(m.title);
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        builder.Append("=本章目录=");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "id", "nid", "jid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        strWhere = "shi=" + nid + " and isdel=0";
        if (jid == 0)
            strWhere += " and jid<=0";
        else
            strWhere += " and jid=" + jid + "";

        // 开始读取列表
        IList<Book.Model.Contents> listContents = new Book.BLL.Contents().GetContentss(pageIndex, pageSize, strWhere, out recordCount);
        if (listContents.Count > 0)
        {
            int k = 1;
            foreach (Book.Model.Contents n in listContents)
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

                if (n.jid == -1)
                {
                    builder.AppendFormat("[{0}][分卷]<a href=\"" + Utils.getUrl("bookps.aspx?act=view&amp;id={1}&amp;nid={2}&amp;jid={3}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{4}</a>", n.pid, id, nid, n.id, n.title);
                    builder.Append("[<a href=\"" + Utils.getUrl("bookps.aspx?act=editjuan&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + n.id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">改</a>]");
                    builder.Append("[<a href=\"" + Utils.getUrl("bookps.aspx?act=deljuan&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + n.id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">删</a>]");
                }
                else
                {
                    builder.AppendFormat("[{0}][章节]<a href=\"" + Utils.getUrl("bookps.aspx?act=editcontent&amp;id={1}&amp;nid={2}&amp;jid={3}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{4}</a>", n.pid, id, nid, n.id, n.title);
                    builder.Append("(" + n.contents.Length + "字)");
                    if (n.state == 0)
                    {
                        builder.Append("[未审核]");
                    }
                    builder.Append("[<a href=\"" + Utils.getUrl("bookps.aspx?act=editcontent&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + n.id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">改</a>]");
                    builder.Append("[<a href=\"" + Utils.getUrl("bookps.aspx?act=delcontent&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + n.id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">删</a>]");
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
        if (jid > 0)
        {
            builder.Append(Out.Tab("<div class=\"title2\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("bookps.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + "") + "\">书卷目录</a>");
            builder.Append(Out.Tab("</div>", ""));
        }

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("bookps.aspx?act=list&amp;id=" + id + "") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回书城中心</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void EditJuanPage()
    {
        int id = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int nid = Utils.ParseInt(Utils.GetRequest("nid", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int jid = Utils.ParseInt(Utils.GetRequest("jid", "all", 2, @"^[0-9]\d*$", "ID错误"));

        Book.Model.Navigation model = new Book.BLL.Navigation().GetNavigation(id);
        if (model == null)
        {
            Utils.Error("不存在的分类", "");
        }
        Book.Model.ShuMu m = new Book.BLL.ShuMu().GetShuMu(nid);
        if (m == null)
        {
            Utils.Error("不存在的书本", "");
        }
        if (m.isdel == 1)
        {
            Utils.Error("不存在的记录", "");
        }
        Book.Model.Contents n = new Book.BLL.Contents().GetContents(jid);
        if (n == null)
        {
            Utils.Error("不存在的分卷", "");
        }
        if (n.isdel == 1)
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "修改书本分卷";

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("bookman.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + "") + "\">书本</a>&gt;");
        builder.Append("修改分卷");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append(m.title);
        builder.Append(Out.Tab("</div>", ""));

        string strText = "分卷名称:/,排序(越大越向上|默认自动):/,,,,,";
        string strName = "title,pid,id,nid,jid,act,backurl";
        string strType = "text,num,hidden,hidden,hidden,hidden,hidden";
        string strValu = "" + n.title + "'" + n.pid + "'" + id + "'" + nid + "'" + jid + "'editjuansave'" + Utils.getPage(0) + "";
        string strEmpt = "true,false,false,false,false,false,false";
        string strIdea = "/";
        string strOthe = "修改分卷,bookps.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("bookman.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + "") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回书城中心</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void EditJuanSavePage()
    {
        int id = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int nid = Utils.ParseInt(Utils.GetRequest("nid", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int jid = Utils.ParseInt(Utils.GetRequest("jid", "all", 2, @"^[0-9]\d*$", "ID错误"));

        Book.Model.Navigation model = new Book.BLL.Navigation().GetNavigation(id);
        if (model == null)
        {
            Utils.Error("不存在的分类", "");
        }
        Book.Model.ShuMu m = new Book.BLL.ShuMu().GetShuMu(nid);
        if (m == null)
        {
            Utils.Error("不存在的书本", "");
        }
        if (m.isdel == 1)
        {
            Utils.Error("不存在的记录", "");
        }
        Book.Model.Contents n = new Book.BLL.Contents().GetContents(jid);
        if (n == null)
        {
            Utils.Error("不存在的分卷", "");
        }
        if (n.isdel == 1)
        {
            Utils.Error("不存在的记录", "");
        }
        string title = Utils.GetRequest("title", "all", 2, @"^[\s\S]{1,50}$", "分卷名称限1-50字");
        int pid = Utils.ParseInt(Utils.GetRequest("pid", "all", 2, @"^[0-9]\d*$", "排序错误"));

        n.id = jid;
        n.jid = 0;
        n.shi = nid;
        n.state = 1;
        n.summary = "";
        n.tags = "";
        n.title = title;
        n.addtime = DateTime.Now;
        n.contents = "";
        n.aid = m.aid;
        n.pid = pid;
        new Book.BLL.Contents().Update(n);
        Utils.Success("编辑分卷", "修改分卷成功，正在返回..", Utils.getUrl("bookman.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + ""), "1");

    }

    private void DelJuanPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        int nid = int.Parse(Utils.GetRequest("nid", "get", 2, @"^[0-9]\d*$", "ID错误"));
        int jid = Utils.ParseInt(Utils.GetRequest("jid", "all", 2, @"^[0-9]\d*$", "ID错误"));
        Book.Model.Contents model = new Book.BLL.Contents().GetContents(jid);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (info != "ok")
        {
            Master.Title = "删除分卷";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此分卷记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("bookman.aspx?info=ok&amp;act=deljuan&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + jid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("bookman.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回书城中心</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            //删除
            new Book.BLL.Contents().Delete(jid);
            new Book.BLL.Contents().Delete("jid=" + jid + "");
            Utils.Success("删除分卷", "删除分卷成功..", Utils.getPage("bookman.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + ""), "1");
        }
    }


    private void EditContentPage()
    {
        int id = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int nid = Utils.ParseInt(Utils.GetRequest("nid", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int jid = Utils.ParseInt(Utils.GetRequest("jid", "all", 2, @"^[0-9]\d*$", "ID错误"));

        Book.Model.Navigation model = new Book.BLL.Navigation().GetNavigation(id);
        if (model == null)
        {
            Utils.Error("不存在的分类", "");
        }
        Book.Model.ShuMu m = new Book.BLL.ShuMu().GetShuMu(nid);
        if (m == null)
        {
            Utils.Error("不存在的书本", "");
        }
        if (m.isdel == 1)
        {
            Utils.Error("不存在的记录", "");
        }
        Book.Model.Contents n = new Book.BLL.Contents().GetContents(jid);
        if (n == null)
        {
            Utils.Error("不存在的章节", "");
        }
        if (n.isdel == 1)
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "修改书本章节";

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("bookps.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + "") + "\">书本</a>&gt;");
        builder.Append("编辑章节");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append(m.title);
        builder.Append(Out.Tab("</div>", ""));

        string strText = "章节名称:/,章节内容:/,搜索关键词(多个用#分开):/,添加时间:/,排序(越大越向上|默认自动):/,,,,,";
        string strName = "title,contents,tags,addtime,pid,id,nid,jid,act,backurl";
        string strType = "text,big,hidden,hidden,num,hidden,hidden,hidden,hidden,hidden";
        string strValu = "" + n.title + "'" + n.contents + "'" + n.tags + "'" + DT.FormatDate(n.addtime, 0) + "'" + n.pid + "'" + id + "'" + nid + "'" + jid + "'editcontentsave'" + Utils.getPage(0) + "";
        string strEmpt = "true,true,true,false,false,false,false,false,false,false";
        string strIdea = "/";
        string strOthe = "修改章节,bookps.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("bookps.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + jid + "") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回书城中心</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void EditContentSavePage()
    {
        int id = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int nid = Utils.ParseInt(Utils.GetRequest("nid", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int jid = Utils.ParseInt(Utils.GetRequest("jid", "all", 2, @"^[0-9]\d*$", "ID错误"));

        Book.Model.Navigation model = new Book.BLL.Navigation().GetNavigation(id);
        if (model == null)
        {
            Utils.Error("不存在的分类", "");
        }
        Book.Model.ShuMu m = new Book.BLL.ShuMu().GetShuMu(nid);
        if (m == null)
        {
            Utils.Error("不存在的书本", "");
        }
        if (m.isdel == 1)
        {
            Utils.Error("不存在的记录", "");
        }
        Book.Model.Contents n = new Book.BLL.Contents().GetContents(jid);
        if (n == null)
        {
            Utils.Error("不存在的章节", "");
        }
        if (n.isdel == 1)
        {
            Utils.Error("不存在的记录", "");
        }
        string title = Utils.GetRequest("title", "all", 2, @"^[\s\S]{1,50}$", "章节名称限1-50字");
        string contents = Utils.GetRequest("contents", "all", 2, @"^[\s\S]{1,}$", "章节内容不能为空");
        string tags = Utils.GetRequest("tags", "all", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "多个搜索关键词请用#分开，不能为空");
        DateTime addtime = Utils.ParseTime(Utils.GetRequest("addtime", "post", 2, DT.RegexTime, "时间填写错误"));

        int state = Utils.ParseInt(ub.GetSub("BookIsContents2", xmlPath));
        int pid = Utils.ParseInt(Utils.GetRequest("pid", "all", 2, @"^[0-9]\d*$", "排序填写错误"));

        n.id = jid;
        n.shi = nid;
        n.state = state;
        n.summary = "";
        n.tags = tags;
        n.title = title;
        n.addtime = addtime;
        n.contents = contents;
        n.aid = m.aid;
        n.pid = pid;
        new Book.BLL.Contents().Update(n);
        Utils.Success("编辑章节", "修改章节成功，正在返回..", Utils.getPage("bookps.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + n.jid + ""), "1");

    }

    private void DelContentPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        int nid = int.Parse(Utils.GetRequest("nid", "get", 2, @"^[0-9]\d*$", "ID错误"));
        int jid = Utils.ParseInt(Utils.GetRequest("jid", "all", 2, @"^[0-9]\d*$", "ID错误"));
        Book.Model.Contents model = new Book.BLL.Contents().GetContents(jid);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.isdel == 1)
        {
            Utils.Error("不存在的记录", "");
        }
        if (info != "ok")
        {
            Master.Title = "删除章节";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此章节记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("bookps.aspx?info=ok&amp;act=delcontent&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + jid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("bookps.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + model.jid + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回书城中心</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            //删除
            new Book.BLL.Contents().Delete(jid);
            Utils.Success("删除章节", "删除章节成功..", Utils.getPage("bookps.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + model.jid + ""), "1");
        }
    }

}
