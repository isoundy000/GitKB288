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

public partial class Manage_book : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "书城管理";
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "bbs":
                BbsPage();
                break;
            case "delbbs":
                DelBbsPage();
                break;
            case "add":
                AddPage();
                break;
            case "addsave":
                AddSavePage();
                break;
            case "edit":
                EditPage();
                break;
            case "editsave":
                EditSavePage();
                break;
            case "del":
                DelPage();
                break;
            case "list":
                ListPage();
                break;
            case "addshumu":
                AddShuMuPage();
                break;
            case "addshumusave":
                AddShuMuSavePage();
                break;
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
            case "addjuan":
                AddJuanPage();
                break;
            case "addjuansave":
                AddJuanSavePage();
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
            case "addcontent":
                AddContentPage();
                break;
            case "addcontentsave":
                AddContentSavePage();
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
            case "move":
                MovePage();
                break;
            case "box":
                BoxPage();
                break;
            case "boxback":
                BoxBackPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        Master.Title = "书城管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("书城管理");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("未审核:书本(<a href=\"" + Utils.getUrl("book.aspx?act=ps") + "\">" + new Book.BLL.ShuMu().GetCount(0) + "</a>)");
        builder.Append("章节(<a href=\"" + Utils.getUrl("book.aspx?act=ps&amp;ptype=1") + "\">" + new Book.BLL.Contents().GetCount(0) + "</a>)");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<Book.Model.Navigation> listNavigation = new Book.BLL.Navigation().GetNavigations(pageIndex, pageSize, strWhere, out recordCount);
        if (listNavigation.Count > 0)
        {
            int k = 1;
            foreach (Book.Model.Navigation n in listNavigation)
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

                builder.Append("<a href=\"" + Utils.getUrl("book.aspx?act=edit&amp;id=" + n.id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[管理]&gt;</a>");
                builder.AppendFormat("{0}.<a href=\"" + Utils.getUrl("book.aspx?act=list&amp;id={1}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}</a>", n.pid, n.id, n.Name);
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
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("book.aspx?act=add") + "\">添加分类</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("book.aspx?act=bbs") + "\">书评管理</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("xml/bookset.aspx") + "\">书城配置</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("book.aspx?act=box") + "\">回收中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void BoxPage()
    {
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 1, @"^[0-2]$", "0"));

        Master.Title = "书城回收中心";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("书城回收中心");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 0)
            builder.Append("书本|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("book.aspx?act=box&amp;ptype=0") + "\">书本</a>|");

        if (ptype == 1)
            builder.Append("分卷|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("book.aspx?act=box&amp;ptype=1") + "\">分卷</a>|");

        if (ptype == 2)
            builder.Append("章节");
        else
            builder.Append("<a href=\"" + Utils.getUrl("book.aspx?act=box&amp;ptype=2") + "\">章节</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        if (ptype == 0)
        {
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string strWhere = "";
            string[] pageValUrl = { "act", "ptype", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            strWhere = "isdel=1";
            // 开始读取列表
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
                    builder.Append("<a href=\"" + Utils.getUrl("book.aspx?act=editshumu&amp;id=" + n.nid + "&amp;nid=" + n.id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[管理]&gt;</a>");
                    builder.AppendFormat("{0}.<a href=\"" + Utils.getUrl("book.aspx?act=view&amp;id={1}&amp;nid={2}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{3}</a>", (pageIndex - 1) * pageSize + k, n.nid, n.id, n.title);
                    if (n.state == 0)
                        builder.Append("[未审核]");

                    if (n.isover == 1)
                        builder.Append("[已完结]");

                    builder.Append("[<a href=\"" + Utils.getUrl("book.aspx?act=boxback&amp;nid=" + n.id + "&amp;ptype=" + ptype + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">恢复</a>]");
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
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string strWhere = "";
            string[] pageValUrl = { "act", "ptype", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            //查询条件
            if (ptype == 1)
                strWhere += " jid<0 and isdel=1";
            else
                strWhere += " jid>=0 and isdel=1";

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
                        builder.AppendFormat("[{0}][分卷]<a href=\"" + Utils.getUrl("book.aspx?act=view&amp;id={1}&amp;nid={2}&amp;jid={3}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{4}</a>", (pageIndex - 1) * pageSize + k, "0", n.shi, n.id, n.title);
                        builder.Append("[<a href=\"" + Utils.getUrl("book.aspx?act=deljuan&amp;id=0&amp;nid=" + n.shi + "&amp;jid=" + n.id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">删</a>]");
                    }
                    else
                    {
                        builder.AppendFormat("[{0}][章节]<a href=\"" + Utils.getUrl("book.aspx?act=editcontent&amp;id={1}&amp;nid={2}&amp;jid={3}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{4}</a>", (pageIndex - 1) * pageSize + k, "0", n.shi, n.id, n.title);
                        builder.Append("(" + n.contents.Length + "字)");
                        if (n.state == 0)
                        {
                            builder.Append("[未审核]");
                        }
                        builder.Append("[<a href=\"" + Utils.getUrl("book.aspx?act=delcontent&amp;id=0&amp;nid=" + n.shi + "&amp;jid=" + n.id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">删</a>]");


                    }
                    builder.Append("[<a href=\"" + Utils.getUrl("book.aspx?act=boxback&amp;nid=" + n.id + "&amp;ptype=" + ptype + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">恢复</a>]");

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
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("book.aspx") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void BoxBackPage()
    {
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 1, @"^[0-2]$", "0"));
        int nid = Utils.ParseInt(Utils.GetRequest("nid", "all", 1, @"^[0-9]\d*$", "ID错误"));

        if (ptype == 0)
        {
            if (!new Book.BLL.ShuMu().Exists(nid))
            {
                Utils.Error("不存在的记录", "");
            }
            new Book.BLL.ShuMu().Updateisdel(nid, 0);
        }
        else
        {
            if (!new Book.BLL.Contents().Exists(nid))
            {
                Utils.Error("不存在的记录", "");
            }
            new Book.BLL.Contents().Updateisdel(nid, 0);
        
        }
        Utils.Success("恢复成功", "恢复成功，正在返回..", Utils.getUrl("book.aspx?act=box&amp;ptype=" + ptype + ""), "1");
    }



    private void AddPage()
    {
        Master.Title = "添加书本分类";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("添加分类");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "分类名称:/,排序(越小越靠前):/,";
        string strName = "name,pid,act";
        string strType = "text,num,hidden";
        string strValu = "'0'addsave";
        string strEmpt = "true,false,false";
        string strIdea = "/";
        string strOthe = "添加分类,book.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("book.aspx") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void AddSavePage()
    {
        int pid = Utils.ParseInt(Utils.GetRequest("pid", "all", 2, @"^[0-9]\d*$", "排序错误"));
        string name = Utils.GetRequest("name", "all", 2, @"^[\s\S]{1,10}$", "分类名称限1-10字");
        Book.Model.Navigation model = new Book.Model.Navigation();
        model.pid = pid;
        model.Name = name;
        new Book.BLL.Navigation().Add(model);
        Utils.Success("添加分类", "添加分类成功，正在返回..", Utils.getUrl("book.aspx"), "1");

    }

    private void EditPage()
    {
        int id = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        Book.Model.Navigation model = new Book.BLL.Navigation().GetNavigation(id);
        if (model == null)
        {
            Utils.Error("不存在的分类", "");
        }
        Master.Title = "编辑书本分类";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("编辑分类");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "分类名称:/,排序(越小越靠前):/,,";
        string strName = "name,pid,id,act";
        string strType = "text,num,hidden,hidden";
        string strValu = "" + model.Name + "'" + model.pid + "'" + id + "'editsave";
        string strEmpt = "true,false,false,false";
        string strIdea = "/";
        string strOthe = "确定修改,book.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("book.aspx?act=del&amp;id="+id+"") + "\">删除分类</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("book.aspx") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void EditSavePage()
    {
        int id = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int pid = Utils.ParseInt(Utils.GetRequest("pid", "all", 2, @"^[0-9]\d*$", "排序错误"));
        string name = Utils.GetRequest("name", "all", 2, @"^[\s\S]{1,10}$", "分类名称限1-10字");
        Book.Model.Navigation model = new Book.Model.Navigation();
        model.id = id;
        model.pid = pid;
        model.Name = name;
        new Book.BLL.Navigation().Update(model);
        Utils.Success("编辑分类", "修改分类成功，正在返回..", Utils.getUrl("book.aspx"), "1");

    }

    private void DelPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        if (info != "ok")
        {
            Master.Title = "删除分类";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此分类记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("book.aspx?info=ok&amp;act=del&amp;id=" + id + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("book.aspx?act=edit&amp;id=" + id + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new Book.BLL.Navigation().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            //有下级不能删除
            if (new Book.BLL.ShuMu().ExistsNode(id))
            {
                Utils.Error("存在下级书本，请先删除书本记录", "");
            }
            //删除
            new Book.BLL.Navigation().Delete(id);
            Utils.Success("删除分类", "删除分类成功..", Utils.getUrl("book.aspx"), "1");
        }
    }

    private void ListPage()
    {
        int id = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        Book.Model.Navigation model = new Book.BLL.Navigation().GetNavigation(id);
        if (model == null)
        {
            Utils.Error("不存在的分类", "");
        }
        Master.Title = "管理-" + model.Name + "";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("book.aspx") + "\">分类</a>&gt;");
        builder.Append("" + model.Name + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("书本列表");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title2\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("book.aspx?act=addshumu&amp;id=" + id + "") + "\">添加书本&gt;&gt;</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        strWhere = "nid=" + id + "";
        // 开始读取列表
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
                builder.Append("<a href=\"" + Utils.getUrl("book.aspx?act=editshumu&amp;id=" + n.nid + "&amp;nid=" + n.id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[管理]&gt;</a>");
                builder.AppendFormat("{0}.<a href=\"" + Utils.getUrl("book.aspx?act=view&amp;id={1}&amp;nid={2}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{3}</a>", (pageIndex - 1) * pageSize + k, n.nid,n.id, n.title);
                if (n.state == 0)
                    builder.Append("[未审核]");

                if (n.isover == 1)
                    builder.Append("[已完结]");

                builder.Append("[<a href=\"" + Utils.getUrl("book.aspx?act=move&amp;id=" + n.nid + "&amp;nid=" + n.id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">移</a>]");
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
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("book.aspx") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void MovePage()
    {
        int id = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int nid = Utils.ParseInt(Utils.GetRequest("nid", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int newid = Utils.ParseInt(Utils.GetRequest("newid", "all", 1, @"^[0-9]\d*$", "0"));
        string info = Utils.GetRequest("info", "get", 1, "", "");

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
        Master.Title = "移动书本";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("移动书本");
        builder.Append(Out.Tab("</div>", "<br />"));
        if (newid == 0)
        {
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string strWhere = "id<>" + id + "";
            string[] pageValUrl = { "act", "id", "nid", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            // 开始读取列表
            IList<Book.Model.Navigation> listNavigation = new Book.BLL.Navigation().GetNavigations(pageIndex, pageSize, strWhere, out recordCount);
            if (listNavigation.Count > 0)
            {
                int k = 1;
                foreach (Book.Model.Navigation n in listNavigation)
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

                    builder.AppendFormat("{0}.<a href=\"" + Utils.getUrl("book.aspx?act=list&amp;id={1}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}</a>", n.pid, n.id, n.Name);
                    builder.Append("[<a href=\"" + Utils.getUrl("book.aspx?act=move&amp;id=" + id + "&amp;nid=" + nid + "&amp;newid=" + n.id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">移入</a>]");

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
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("book.aspx?act=list&amp;id=" + id + "&amp;nid=" + nid + "") + "\">返回上级</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (info != "ok")
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("确定移动到此分类吗");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getUrl("book.aspx?info=ok&amp;act=move&amp;id=" + id + "&amp;nid=" + nid + "&amp;newid=" + newid + "") + "\">确定移动</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("book.aspx?act=move&amp;id=" + id + "&amp;nid=" + nid + "") + "\">先不移吧..</a>");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            else
            {
                new Book.BLL.ShuMu().Updatenid(nid, newid);
                Utils.Success("移动书本", "移动书本成功，正在进入新分类..<br /><a href=\"" + Utils.getUrl("book.aspx?act=list&amp;id=" + id + "") + "\">&gt;&gt;返回旧分类</a>", Utils.getUrl("book.aspx?act=list&amp;id=" + newid + ""), "2");
            }
        
        }
       
    }


    private void AddShuMuPage()
    {
        int id = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        Book.Model.Navigation model = new Book.BLL.Navigation().GetNavigation(id);
        if (model == null)
        {
            Utils.Error("不存在的分类", "");
        }
        Master.Title = "添加书本";
        if (!Utils.Isie())
        {
            builder.Append(Out.Tab("<div>", ""));
            string VE = ConfigHelper.GetConfigString("VE");
            string SID = ConfigHelper.GetConfigString("SID");
            builder.Append("<a href=\"book.aspx?act=addshumu&amp;id=" + id + "&amp;" + VE + "=20a&amp;" + SID + "=" + Utils.getstrU() + "\">[切换2.0彩版]</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("book.aspx?act=list&amp;id=" + id + "") + "\">" + model.Name + "</a>&gt;");
            builder.Append("添加书本");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "书本名称:/,书本简介:/,书本封面:/,书本作者:/,书本字数(未知请填0):/,搜索关键词(多个用#分开):/,书本状态:/,书本性质:/,上传会员ID(没有请填0):/,是否精品:/,,";
            string strName = "title,summary,img,author,length,tags,state,types,aid,isgood,id,act";
            string strType = "text,textarea,file,text,num,text,select,select,num,select,hidden,hidden";
            string strValu = "''''0''1'0'0'0'" + id + "'addshumusave";
            string strEmpt = "true,true,true,true,true,true,0|未审核|1|已审核,0|原创|1|转载,false,0|普通|1|精品,false,false";
            string strIdea = "/";
            string strOthe = "添加书本,book.aspx,post,2,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + ReplaceWap(Utils.getUrl("book.aspx?act=list&amp;id=" + id + "")) + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + ReplaceWap(Utils.getUrl("default.aspx")) + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

    }

    private void AddShuMuSavePage()
    {
        int id = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        string title = Utils.GetRequest("title", "all", 2, @"^[\s\S]{1,200}$", "书本名称限1-200字");
        string summary = Utils.GetRequest("summary", "all", 2, @"^[\s\S]{1,500}$", "书本简介限1-500字");
        string author = Utils.GetRequest("author", "all", 2, @"^[\s\S]{1,50}$", "书本作者限1-50字");
        int length = Utils.ParseInt(Utils.GetRequest("length", "all", 2, @"^[0-9]\d*$", "字数填写错误"));
        string tags = Utils.GetRequest("tags", "all", 2, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "多个搜索关键词请用#分开");
        int state = Utils.ParseInt(Utils.GetRequest("state", "all", 2, @"^[0-2]$", "书本状态选择错误"));
        int types = Utils.ParseInt(Utils.GetRequest("types", "all", 2, @"^[0-1]$", "书本性质选择错误"));
        int aid = Utils.ParseInt(Utils.GetRequest("aid", "all", 2, @"^[0-9]\d*$", "上传会员ID错误"));
        int isgood = Utils.ParseInt(Utils.GetRequest("isgood", "all", 2, @"^[0-1]$", "是否精品选择错误"));

        //------------------------------------附件接收开始------------------------------------
        int uid = 0;
        string img = "";
        //遍历File表单元素
        System.Web.HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
        for (int iFile = 0; iFile < files.Count; iFile++)
        {
            //检查文件扩展名字
            System.Web.HttpPostedFile postedFile = files[iFile];
            string fileName, fileExtension;
            fileName = System.IO.Path.GetFileName(postedFile.FileName);
            string UpExt = ".jpg,.jpeg,.gif,.png,.bmp";
            int UpLength = 500;
            if (fileName != "")
            {
                fileExtension = System.IO.Path.GetExtension(fileName).ToLower();
                //检查是否允许上传格式
                if (UpExt.IndexOf(fileExtension) == -1)
                {
                    Utils.Error("附件格式只允许" + UpExt + "", "");
                }
                if (postedFile.ContentLength > Convert.ToInt32(UpLength * 1024))
                {
                    Utils.Error("附件大小限500K内", "");
                }
                string DirPath = string.Empty;
                string prevDirPath = string.Empty;
                string Path = "/Files/book/" + uid + "/";
                if (BCW.Files.FileTool.CreateDirectory(Path, out DirPath))
                {
                    //生成随机文件名
                    fileName = DT.getDateTimeNum() + iFile + fileExtension;
                    string SavePath = System.Web.HttpContext.Current.Request.MapPath(DirPath) + fileName;
                    postedFile.SaveAs(SavePath);

                    img = DirPath + fileName;
                }
                else
                {
                    Utils.Error("上传出现错误", "");
                }
            }
        }
        //------------------------------------附件接收结束------------------------------------

        Book.Model.ShuMu model = new Book.Model.ShuMu();
        model.nid = id;
        model.title = title;
        model.summary = summary;
        model.img = img;
        model.author = author;
        model.length = length;
        model.tags = tags;
        model.state = state;
        model.addtime = DateTime.Now;
        model.aid = aid;
        model.types = types;
        model.isgood = isgood;
        new Book.BLL.ShuMu().Add(model);
        Utils.Success("添加书本", "添加书本成功，正在返回..", ReplaceWap(Utils.getUrl("book.aspx?act=list&amp;id=" + id + "")), "1");

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
        Master.Title = "编辑书本";

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("book.aspx?act=list&amp;id=" + id + "") + "\">" + model.Name + "</a>&gt;");
        builder.Append("编辑书本");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "书本名称:/,书本简介:/,书本封面:/,书本作者:/,书本字数(未知请填0):/,搜索关键词(多个用#分开):/,书本状态:/,书本性质:/,上传会员ID(没有请填0):/,添加时间:/,书本人气:/,书本推荐数:/,最后更新时间:/,是否完结:/,是否精品:/,,,,";
        string strName = "title,summary,img,author,length,tags,state,types,aid,addtime,click,good,gxtime,isover,isgood,id,nid,act,backurl";
        string strType = "text,textarea,text,text,num,text,select,select,num,date,num,num,date,select,select,hidden,hidden,hidden,hidden";
        string strValu = "" + m.title + "'" + m.summary + "'" + m.img + "'" + m.author + "'" + m.length + "'" + m.tags + "'" + m.state + "'" + m.types + "'" + m.aid + "'" + DT.FormatDate(m.addtime, 0) + "'" + m.click + "'" + m.good + "'" + DT.FormatDate(m.gxtime, 0) + "'" + m.isover + "'" + m.isgood + "'" + id + "'" + nid + "'editshumusave'" + Utils.getPage(0) + "";
        string strEmpt = "true,true,true,true,true,true,0|未审核|1|已审核,0|原创|1|转载,false,false,false,false,false,0|连载中|1|已完结,0|普通|1|精品,false,false,false,false";
        string strIdea = "/";
        string strOthe = "修改书本,book.aspx,post,2,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("book.aspx?act=delshumu&amp;id=" + id + "&amp;nid=" + nid + "") + "\">删除书本</a><br />");
        builder.Append("<a href=\"" + Utils.getPage("book.aspx?act=list&amp;id=" + id + "") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
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
        int state = Utils.ParseInt(Utils.GetRequest("state", "all", 2, @"^[0-1]$", "书本状态选择错误"));
        int types = Utils.ParseInt(Utils.GetRequest("types", "all", 2, @"^[0-1]$", "书本性质选择错误"));        
        int aid = Utils.ParseInt(Utils.GetRequest("aid", "all", 2, @"^[0-9]\d*$", "上传会员ID错误"));
        DateTime addtime = Utils.ParseTime(Utils.GetRequest("addtime", "post", 2, DT.RegexTime, "添加时间填写错误"));
        int click = Utils.ParseInt(Utils.GetRequest("click", "all", 2, @"^[0-9]\d*$", "书本人气填写错误"));
        int good = Utils.ParseInt(Utils.GetRequest("good", "all", 2, @"^[0-9]\d*$", "书本推荐数填写错误"));        
        DateTime gxtime = Utils.ParseTime(Utils.GetRequest("gxtime", "post", 2, DT.RegexTime, "最后更新时间填写错误"));
        int isover = Utils.ParseInt(Utils.GetRequest("isover", "all", 2, @"^[0-1]$", "是否完结选择错误"));
        int isgood = Utils.ParseInt(Utils.GetRequest("isgood", "all", 2, @"^[0-1]$", "是否精品选择错误"));

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
        model.click = click;
        model.good = good;
        model.gxtime = gxtime;
        model.isover = isover;
        model.isgood = isgood;
        new Book.BLL.ShuMu().Update2(model);
        Utils.Success("编辑书本", "修改书本成功，正在返回..", Utils.getPage("book.aspx?act=list&amp;id=" + id + ""), "1");
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
        if (info != "ok")
        {
            Master.Title = "删除书本";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("删除书本将会删除本书分卷和章节，确定删除此书本记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("book.aspx?info=ok&amp;act=delshumu&amp;id=" + id + "&amp;nid=" + nid + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("book.aspx?act=editshumu&amp;id=" + id + "&amp;nid=" + nid + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            //删除
            new Book.BLL.ShuMu().Delete(nid);
            new Book.BLL.Contents().Delete("shi=" + nid + "");
            Utils.Success("删除书本", "删除书本成功..", Utils.getUrl("book.aspx?act=list&amp;id=" + id + ""), "1");
        }
    }

    private void ViewPage()
    {
        int id = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int nid = Utils.ParseInt(Utils.GetRequest("nid", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int jid = Utils.ParseInt(Utils.GetRequest("jid", "all", 1, @"^[0-9]\d*$", "0"));

        //取分类ID（新）
        if (id == 0)
        {
            id = new Book.BLL.ShuMu().Getnid(nid);
        }


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

        Master.Title = "查看书本";

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("book.aspx?act=list&amp;id=" + id + "") + "\">" + model.Name + "</a>&gt;");
        builder.Append("查看书本");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append(m.title);
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title2\">", Out.Hr()));
        builder.Append("添加");
        if (jid == 0)
        {
            builder.Append("<a href=\"" + Utils.getUrl("book.aspx?act=addjuan&amp;id=" + id + "&amp;nid=" + nid + "") + "\">分卷</a>|");
        }
        builder.Append("<a href=\"" + Utils.getUrl("book.aspx?act=addcontent&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + jid + "") + "\">章节</a>");
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
        strWhere = "shi=" + nid + "";
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
                    builder.AppendFormat("[{0}][分卷]<a href=\"" + Utils.getUrl("book.aspx?act=view&amp;id={1}&amp;nid={2}&amp;jid={3}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{4}</a>", n.pid, id, nid, n.id, n.title);
                    builder.Append("[<a href=\"" + Utils.getUrl("book.aspx?act=editjuan&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + n.id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">改</a>]");
                    builder.Append("[<a href=\"" + Utils.getUrl("book.aspx?act=deljuan&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + n.id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">删</a>]");
                }
                else
                {
                    builder.AppendFormat("[{0}][章节]<a href=\"" + Utils.getUrl("book.aspx?act=editcontent&amp;id={1}&amp;nid={2}&amp;jid={3}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{4}</a>", n.pid, id, nid, n.id, n.title);
                    builder.Append("(" + n.contents.Length + "字)");
                    if (n.state == 0)
                    {
                        builder.Append("[未审核]");
                    }
                    builder.Append("[<a href=\"" + Utils.getUrl("book.aspx?act=editcontent&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + n.id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">改</a>]");
                    builder.Append("[<a href=\"" + Utils.getUrl("book.aspx?act=delcontent&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + n.id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">删</a>]");

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
            builder.Append("<a href=\"" + Utils.getUrl("book.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + "") + "\">书卷目录</a>");
            builder.Append(Out.Tab("</div>", ""));
        }

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("book.aspx?act=bbs&amp;nid=" + nid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">查看书评(" + m.pl + ")条</a><br />");
        builder.Append("<a href=\"" + Utils.getPage("book.aspx?act=list&amp;id=" + id + "") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }


    private void AddJuanPage()
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

        Master.Title = "添加书本分卷";

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("book.aspx?act=list&amp;id=" + id + "") + "\">" + model.Name + "</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("book.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + "") + "\">书本</a>&gt;");
        builder.Append("添加分卷");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append(m.title);
        builder.Append(Out.Tab("</div>", ""));
        //得到排序
        int pid = new Book.BLL.Contents().GetCount(nid, 0, 1);
        pid = (pid + 1) * 10;

        string strText = "分卷名称:/,排序(越大越向上|默认自动):/,,,";
        string strName = "title,pid,id,nid,act";
        string strType = "text,num,hidden,hidden,hidden";
        string strValu = "'"+pid+"'" + id + "'" + nid + "'addjuansave";
        string strEmpt = "true,false,false,false,false";
        string strIdea = "/";
        string strOthe = "添加分卷,book.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("book.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + "") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void AddJuanSavePage()
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
        string title = Utils.GetRequest("title", "all", 2, @"^[\s\S]{1,50}$", "分卷名称限1-50字");
        int pid = Utils.ParseInt(Utils.GetRequest("pid", "all", 2, @"^[0-9]\d*$", "排序错误"));

        Book.Model.Contents n = new Book.Model.Contents();
        n.jid = -1;
        n.shi = nid;
        n.state = 1;
        n.summary = "";
        n.tags = "";
        n.title = title;
        n.addtime = DateTime.Now;
        n.contents = "";
        n.aid = m.aid;
        n.pid = pid;

        new Book.BLL.Contents().Add(n);
        Utils.Success("添加分卷", "添加分卷成功，正在返回..", Utils.getUrl("book.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + ""), "1");
    
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
        Book.Model.Contents n = new Book.BLL.Contents().GetContents(jid);
        if (n == null)
        {
            Utils.Error("不存在的分卷", "");
        }
        Master.Title = "修改书本分卷";

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("book.aspx?act=list&amp;id=" + id + "") + "\">" + model.Name + "</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("book.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + "") + "\">书本</a>&gt;");
        builder.Append("修改分卷");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append(m.title);
        builder.Append(Out.Tab("</div>", ""));

        string strText = "分卷名称:/,排序(越大越向上|默认自动):/,,,,,";
        string strName = "title,pid,id,nid,jid,act,backurl";
        string strType = "text,num,hidden,hidden,hidden,hidden,hidden";
        string strValu = ""+n.title+"'"+n.pid+"'" + id + "'" + nid + "'" + jid + "'editjuansave'" + Utils.getPage(0) + "";
        string strEmpt = "true,false,false,false,false,false,false";
        string strIdea = "/";
        string strOthe = "修改分卷,book.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("book.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + "") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
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
        Book.Model.Contents n = new Book.BLL.Contents().GetContents(jid);
        if (n == null)
        {
            Utils.Error("不存在的分卷", "");
        }
        string title = Utils.GetRequest("title", "all", 2, @"^[\s\S]{1,50}$", "分卷名称限1-50字");
        int pid = Utils.ParseInt(Utils.GetRequest("pid", "all", 2, @"^[0-9]\d*$", "排序错误"));

        n.id = jid;
        n.jid = -1;
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
        Utils.Success("编辑分卷", "修改分卷成功，正在返回..", Utils.getUrl("book.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + ""), "1");

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
            builder.Append("<a href=\"" + Utils.getUrl("book.aspx?info=ok&amp;act=deljuan&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + jid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("book.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            //删除
            new Book.BLL.Contents().Delete(jid);
            new Book.BLL.Contents().Delete("jid=" + jid + "");
            Utils.Success("删除分卷", "删除分卷成功..", Utils.getPage("book.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + ""), "1");
        }
    }


    private void AddContentPage()
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
        Master.Title = "添加书本章节";

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("book.aspx?act=list&amp;id=" + id + "") + "\">" + model.Name + "</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("book.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + "") + "\">书本</a>&gt;");
        builder.Append("添加章节");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append(m.title);
        builder.Append(Out.Tab("</div>", ""));
        //得到排序
        int pid = new Book.BLL.Contents().GetCount(nid, jid, 0);
        pid = (pid + 1) * 10;

        string strText = "章节名称:/,章节内容:/,搜索关键词(多个用#分开):/,状态:/,排序(越大越向上|默认自动):/,,,,";
        string strName = "title,contents,tags,state,pid,id,nid,jid,act";
        string strType = "text,big,hidden,select,num,hidden,hidden,hidden,hidden";
        string strValu = "'''1'" + pid + "'" + id + "'" + nid + "'" + jid + "'addcontentsave";
        string strEmpt = "true,true,true,0|未审核|1|已审核,false,false,false,false,false";
        string strIdea = "/";
        string strOthe = "添加章节,book.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("book.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + jid + "") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void AddContentSavePage()
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
        string title = Utils.GetRequest("title", "all", 2, @"^[\s\S]{1,50}$", "章节名称限1-50字");
        string contents = Utils.GetRequest("contents", "all", 2, @"^[\s\S]{1,}$", "章节内容不能为空");
        string tags = Utils.GetRequest("tags", "all", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "多个搜索关键词请用#分开，不能为空");
        int state = Utils.ParseInt(Utils.GetRequest("state", "all", 2, @"^[0-1]$", "状态选择错误"));
        int pid = Utils.ParseInt(Utils.GetRequest("pid", "all", 2, @"^[0-9]\d*$", "排序错误"));

        Book.Model.Contents n = new Book.Model.Contents();
        n.jid = jid;
        n.shi = nid;
        n.state = state;
        n.summary = "";
        n.tags = tags;
        n.title = title;
        n.addtime = DateTime.Now;
        n.contents = contents;
        n.aid = m.aid;
        n.pid = pid;
        new Book.BLL.Contents().Add(n);
        //最后更新
        new Book.BLL.ShuMu().Updategxtime(nid);
        //更新提醒会员
        if (!string.IsNullOrEmpty(m.gxids))
        {
            string[] idsTemp = m.gxids.Split("#".ToCharArray());
            for (int i = 0; i < idsTemp.Length; i++)
            {
                int aid = Utils.ParseInt(idsTemp[i]);
                string mygxids = new Book.BLL.ShuSelf().Getgxids(aid);
                string gxids = "";
                if (mygxids != "")
                {
                    if (!("#" + mygxids + "#").Contains("#" + nid + "#"))
                    {
                        gxids = nid + "#" + mygxids;
                    }
                }
                else
                    gxids = nid.ToString();

                if (gxids != "")
                {
                    new Book.BLL.ShuSelf().Updategxids(aid, gxids);
                }

            }
        }


        Utils.Success("添加章节", "添加章节成功，正在返回..", Utils.getUrl("book.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + jid + ""), "1");

    }

    private void EditContentPage()
    {
        int id = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int nid = Utils.ParseInt(Utils.GetRequest("nid", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int jid = Utils.ParseInt(Utils.GetRequest("jid", "all", 2, @"^[0-9]\d*$", "ID错误"));

        //取分类ID（新）
        if (id == 0)
        {
            id = new Book.BLL.ShuMu().Getnid(nid);
        }

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
        Book.Model.Contents n = new Book.BLL.Contents().GetContents(jid);
        if (n == null)
        {
            Utils.Error("不存在的章节", "");
        }
        Master.Title = "修改书本章节";

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("book.aspx?act=list&amp;id=" + id + "") + "\">" + model.Name + "</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("book.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + "") + "\">书本</a>&gt;");
        builder.Append("编辑章节");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append(m.title);
        builder.Append(Out.Tab("</div>", ""));

        string strText = "章节名称:/,章节内容:/,搜索关键词(多个用#分开):/,添加时间:/,状态:/,排序(越大越向上|默认自动):/,,,,,";
        string strName = "title,contents,tags,addtime,state,pid,id,nid,jid,act,backurl";
        string strType = "text,big,hidden,date,select,num,hidden,hidden,hidden,hidden,hidden";
        string strValu = "" + n.title + "'" + n.contents + "'" + n.tags + "'" + DT.FormatDate(n.addtime,0) + "'"+n.state+"'"+n.pid+"'" + id + "'" + nid + "'" + jid + "'editcontentsave'" + Utils.getPage(0) + "";
        string strEmpt = "true,true,true,false,0|未审核|1|已审核,false,false,false,false,false,false";
        string strIdea = "/";
        string strOthe = "修改章节,book.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("book.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + jid + "") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
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
        Book.Model.Contents n = new Book.BLL.Contents().GetContents(jid);
        if (n == null)
        {
            Utils.Error("不存在的章节", "");
        }
        string title = Utils.GetRequest("title", "all", 2, @"^[\s\S]{1,50}$", "章节名称限1-50字");
        string contents = Utils.GetRequest("contents", "all", 2, @"^[\s\S]{1,}$", "章节内容不能为空");
        string tags = Utils.GetRequest("tags", "all", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "多个搜索关键词请用#分开，不能为空");
        DateTime addtime = Utils.ParseTime(Utils.GetRequest("addtime", "post", 2, DT.RegexTime, "时间填写错误"));
        int state = Utils.ParseInt(Utils.GetRequest("state", "all", 2, @"^[0-1]$", "状态选择错误"));
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
        Utils.Success("编辑章节", "修改章节成功，正在返回..", Utils.getPage("book.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + n.jid + ""), "1");

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
        if (info != "ok")
        {
            Master.Title = "删除章节";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此章节记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("book.aspx?info=ok&amp;act=delcontent&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + jid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("book.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + model.jid + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            //删除
            new Book.BLL.Contents().Delete(jid);
            Utils.Success("删除章节", "删除章节成功..", Utils.getPage("book.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + model.jid + ""), "1");
        }
    }


    private void PsPage()
    {
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 1, @"^[0-1]$", "0"));
 
        Master.Title = "管理未审核";

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("管理未审核");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 0)
            builder.Append("未审书本|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("book.aspx?act=ps&amp;ptype=0") + "\">未审书本</a>|");

        if (ptype == 1)
            builder.Append("未审章节");
        else
            builder.Append("<a href=\"" + Utils.getUrl("book.aspx?act=ps&amp;ptype=1") + "\">未审章节</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        strWhere = "state=0";
        // 开始读取列表
        if (ptype == 0)
        {
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
                    builder.Append("<a href=\"" + Utils.getUrl("book.aspx?act=editshumu&amp;id=" + n.nid + "&amp;nid=" + n.id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[管理]&gt;</a>");
                    builder.AppendFormat("{0}.<a href=\"" + Utils.getUrl("book.aspx?act=view&amp;id={1}&amp;nid={2}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{3}</a>", (pageIndex - 1) * pageSize + k, n.nid, n.id, n.title);
                    builder.Append("[<a href=\"" + Utils.getUrl("book.aspx?act=psok&amp;id=" + n.id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">审核</a>]");

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
                    builder.Append("<a href=\"" + Utils.getUrl("book.aspx?act=editcontent&amp;id=" + nid + "&amp;nid=" + n.shi + "&amp;jid=" + n.id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[管理]&gt;</a>");
                    builder.AppendFormat("{0}.<a href=\"" + Utils.getUrl("book.aspx?act=view&amp;id={1}&amp;nid={2}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{3}</a>", (pageIndex - 1) * pageSize + k, nid, n.shi, n.title);
                    builder.Append("[<a href=\"" + Utils.getUrl("book.aspx?act=psok&amp;ptype=1&amp;id=" + n.id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">审核</a>]");

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
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("book.aspx?act=psok2&amp;ptype=" + ptype + "&amp;page=" + pageIndex + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[审核本页记录]</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("book.aspx") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void PsOkPage()
    {
        int id = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 1, @"^[0-1]$", "0"));

        if (ptype == 0)
        {
            Book.Model.ShuMu m = new Book.BLL.ShuMu().GetShuMu(id);
            if (m == null)
            {
                Utils.Error("不存在的书本", "");
            }
            new Book.BLL.ShuMu().Updatestate(id,1);
        }
        else
        {
            Book.Model.Contents n = new Book.BLL.Contents().GetContents(id);
            if (n == null)
            {
                Utils.Error("不存在的分卷", "");
            }
            new Book.BLL.Contents().Updatestate(id,1);
        }
        Utils.Success("审核", "审核成功，正在返回..", Utils.getPage("book.aspx?act=ps&amp;ptype=" + ptype + ""), "1");

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
            builder.Append("<a href=\"" + Utils.getUrl("book.aspx?act=psok2&amp;info=ok&amp;ptype=" + ptype + "&amp;page=" + page + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定审核</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("book.aspx?act=ps&amp;ptype=" + ptype + "") + "\">再看看吧..</a>");
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

            strWhere = "state=0";

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

            Utils.Success("审核本页记录", "审核本页记录成功，正在返回..", Utils.getPage("book.aspx?act=ps&amp;ptype=" + ptype + ""), "1");
        }
    }

    private void BbsPage()
    {
        int nid = int.Parse(Utils.GetRequest("nid", "get", 1, @"^[0-9]\d*$", "0"));

        Master.Title = "书评管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("书评管理");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
       
        string[] pageValUrl = { "act", "nid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        if (nid > 0)
            strWhere = "nid=" + nid + "";

        // 开始读取列表
        IList<Book.Model.ShuBbs> listShuBbs = new Book.BLL.ShuBbs().GetShuBbss(pageIndex, pageSize, strWhere, out recordCount);
        if (listShuBbs.Count > 0)
        {
            int k = 1;
            foreach (Book.Model.ShuBbs n in listShuBbs)
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
                builder.AppendFormat("{0}.<a href=\"" + Utils.getUrl("uinfo.aspx?uid={1}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}</a>", ((pageIndex - 1) * pageSize) + k, n.usid, n.usname);
                builder.Append(":" + Out.SysUBB(n.content));
                builder.Append("(" + DT.FormatDate(n.addtime, 1) + ")");
                builder.Append("[<a href=\"" + Utils.getUrl("book.aspx?act=delbbs&amp;id=" + n.id + "&amp;nid=" + nid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">删</a>]");
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
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("book.aspx") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void DelBbsPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        int nid = int.Parse(Utils.GetRequest("nid", "get", 2, @"^[0-9]\d*$", "ID错误"));
        Book.Model.ShuBbs model = new Book.BLL.ShuBbs().GetShuBbs(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (info != "ok")
        {
            Master.Title = "删除书评";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此书评记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("book.aspx?info=ok&amp;act=delbbs&amp;id=" + id + "&amp;nid=" + nid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("book.aspx?act=bbs&amp;nid=" + nid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            //删除
            new Book.BLL.ShuBbs().Delete(id);
            //更新书评数目
            new Book.BLL.ShuMu().Updatepl(model.nid, -1);
            Utils.Success("删除书评", "删除书评成功..", Utils.getUrl("book.aspx?act=bbs&amp;nid=" + nid + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
    }

    private static string ReplaceWap(string p_strUrl)
    {
        p_strUrl = p_strUrl.Replace("20a", "1a");

        return p_strUrl;
    }
}
