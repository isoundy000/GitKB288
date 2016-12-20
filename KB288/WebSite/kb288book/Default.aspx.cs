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

public partial class book_Default : System.Web.UI.Page
{
    protected string xmlPath = "/Controls/book.xml";
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        //维护提示
        if (ub.GetSub("BookStatus", xmlPath) == "1")
        {
            Utils.Safe("书城系统");
        }
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "bbs":
                BbsPage();
                break;
            case "bbssave":
                BbsSavePage();
                break;
            case "fl":
                FLPage();
                break;
            case "top":
                TopPage();
                break;
            case "list":
                ListPage();
                break;
            case "listb":
                ListbPage();
                break;
            case "view":
                ViewPage();
                break;
            case "content":
                ContentPage();
                break;
            case "setpw":
                SetPwPage();
                break;
            case "setpwsave":
                SetPwSavePage();
                break;
            case "favadd":
                FavAddPage();
                break;
            case "favadd2":
                FavAdd2Page();
                break;
            case "fav":
                FavPage();
                break;
            case "favdel":
                FavDelPage();
                break;
            case "favdel2":
                FavDel2Page();
                break;
            case "remind":
                RemindPage();
                break;
            case "good":
                GoodPage();
                break;
            case "search":
                SearchPage();
                break;
            case "more":
                MorePage();
                break;
            case "home":
                HomePage();
                break;
            case "shufav":
                ShuFavPage();
                break;
            case "shufavadd":
                ShuFavAddPage();
                break;
            case "shufavaddsave":
                ShuFavAddSavePage();
                break;
            case "shufavedit":
                ShuFavEditPage();
                break;
            case "shufaveditsave":
                ShuFavEditSavePage();
                break;
            case "shufavdel":
                ShuFavDelPage();
                break;
            case "editbasic":
                EditBasicPage();
                break;
            case "editbasicsave":
                EditBasicSavePage();
                break;
            case "setpagenum":
                SetPageNumPage();
                break;
            case "setpagenumsave":
                SetPageNumSavePage();
                break;
            case "getremind":
                GetRemindPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        DataSet ds = null;
        int meid = new BCW.User.Users().GetUsId();
        Master.Title = ub.GetSub("BookName", xmlPath);
        string Logo = ub.GetSub("BookLogo", xmlPath);
        if (Logo != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<img src=\"" + Logo + "\" alt=\"load\"/>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        string Notes = ub.GetSub("BookNotes", xmlPath);
        if (Notes != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.SysUBB(Notes) + "");
            builder.Append(Out.Tab("</div>", "<br />"));
        }

        string SystemUbb = ub.GetSub("BookSystemUbb", xmlPath);
        string System = string.Empty;
        if (SystemUbb.Contains("[@搜索凋用]"))
        {
            string strText = ",,,,";
            string strName = "keyword,ptype,act,info,backurl";
            string strType = "stext,select,hidden,hidden,hidden";
            string strValu = "'0'search'ok'" + Utils.getPage(0) + "";
            string strEmpt = "true,0|书名|1|作者|2|关键词,false,false,false";
            string strIdea = "";
            string strOthe = "搜书,default.aspx,post,3,red";
            System = Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe);

            SystemUbb = SystemUbb.Replace("[@搜索凋用]", System);
        }
        if (SystemUbb.Contains("[@强推凋用]"))
        {
            ds = new Book.BLL.ShuMu().GetList("Top 1 id,nid,title,summary,img", "state>0 and isgood=1 and isdel=0 Order by NEWID()");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    int id = int.Parse(ds.Tables[0].Rows[i]["id"].ToString());
                    int nid = int.Parse(ds.Tables[0].Rows[i]["nid"].ToString());
                    string title = ds.Tables[0].Rows[i]["title"].ToString();
                    string summary = ds.Tables[0].Rows[i]["summary"].ToString();
                    string img = ds.Tables[0].Rows[i]["img"].ToString();
                    System = "";
                    if (img != "")
                    {
                        System += "<a href=\"" + Utils.getUrl("default.aspx") + "\"><img src=\"" + img + "\" alt=\"load\"/></a><br />";

                    }
                    System += "<a href=\"" + Utils.getUrl("default.aspx?act=view&amp;id=" + nid + "&amp;nid=" + id + "") + "\">《" + title + "》</a><br />";
                    System += "简介:" + Utils.Left(summary, 30) + "<a href=\"" + Utils.getUrl("default.aspx?act=view&amp;id=" + nid + "&amp;nid=" + id + "") + "\">阅读</a>";
                }
            }

            SystemUbb = SystemUbb.Replace("[@强推凋用]", System);
        }
           //输出界面
        if (!Utils.Isie())
        {
            SystemUbb = BCW.User.AdminCall.AdminUBB(Out.SysUBB(SystemUbb.Replace((Convert.ToChar(10).ToString()), "<br />")));
            builder.Append(SystemUbb);
        }
        else
        {
            string[] txtIndex = SystemUbb.Split((Convert.ToChar(10).ToString()).ToCharArray());
            for (int i = 0; i < txtIndex.Length; i++)
            {
                // 输出列表的格式
                if (BCW.User.AdminCall.AdminUBB(txtIndex[i]).IndexOf("</div>") == -1 && BCW.User.AdminCall.AdminUBB(txtIndex[i]).IndexOf("</select>") == -1)
                {
                    if ((i + 1) % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", ""));
                    else
                        builder.Append(Out.Tab("<div>", ""));
                }
                SystemUbb = BCW.User.AdminCall.AdminUBB(Out.SysUBB(txtIndex[i].ToString()));
                builder.Append(SystemUbb);

                if (BCW.User.AdminCall.AdminUBB(txtIndex[i]).IndexOf("</div>") == -1 && BCW.User.AdminCall.AdminUBB(txtIndex[i]).IndexOf("</select>") == -1)
                {
                    builder.Append(Out.Tab("</div>", ""));
                }

            }
        }

        builder.Append(Out.Tab("<div class=\"title2\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx") + "\">我的空间</a>");
        string AdminIDS = ub.GetSub("BookAdminIDS", xmlPath);
        string AdminIDS2 = ub.GetSub("BookAdminIDS2", xmlPath);
        if (("#" + AdminIDS + "#").Contains("#" + meid + "#"))
        {
            builder.Append("-<a href=\"" + Utils.getUrl("bookps.aspx") + "\">审核</a>");
        }
        if (("#" + AdminIDS2 + "#").Contains("#" + meid + "#"))
        {
            builder.Append("-<a href=\"" + Utils.getUrl("bookadmin.aspx") + "\">管理</a>");
        }
        builder.Append(Out.Tab("</div>", ""));
    }

    private void FLPage()
    {
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 1, @"^[0-3]\d*$", "0"));

        string sTitle = string.Empty;
        if (ptype == 0)
            sTitle = "分类大全";
        else if (ptype == 1)
            sTitle = "男频专区";
        else if (ptype == 2)
            sTitle = "女频专区";
        else if (ptype == 3)
            sTitle = "其他分类";

        Master.Title = sTitle;
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>&gt;");
        builder.Append(sTitle);
        builder.Append(Out.Tab("</div>", "<br />"));
        string SystemUbb2 = ub.GetSub("BookSystemUbb2", xmlPath);
        if (SystemUbb2 != "")
        {
            builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(SystemUbb2)));
        }
        else
        {
            if (ptype == 0 || ptype == 1)
            {
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("【男频专区】");
                builder.Append(Out.Tab("</div>", "<br />"));

                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=list&amp;id=4") + "\">军事历史</a>|");
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=list&amp;id=13") + "\">仙侠修真</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=list&amp;id=1") + "\">奇幻玄幻</a>|");
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=list&amp;id=8") + "\">穿越架空</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=list&amp;id=7") + "\">转世重生</a>|");
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=list&amp;id=3") + "\">都市异能</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=list&amp;id=16") + "\">官场小说</a>|");
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=list&amp;id=5") + "\">网游竞技</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=list&amp;id=17") + "\">黑道小说</a>|");
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=list&amp;id=14") + "\">古今名著</a>");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            if (ptype == 0 || ptype == 2)
            {
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("【女频专区】");
                builder.Append(Out.Tab("</div>", "<br />"));

                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=list&amp;id=11") + "\">古代言情</a>|");
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=list&amp;id=10") + "\">现代言情</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=list&amp;id=6") + "\">女生玄幻</a>|");
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=list&amp;id=12") + "\">耽美小说</a>");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            if (ptype == 0 || ptype == 3)
            {
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("【其他分类】");
                builder.Append(Out.Tab("</div>", "<br />"));

                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=list&amp;id=19") + "\">侦探推理</a>|");
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=list&amp;id=9") + "\">灵异恐怖</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=list&amp;id=2") + "\">武侠小说</a>|");
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=list&amp;id=20") + "\">畅销小说</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=list&amp;id=18") + "\">青春校园</a>|");
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=list&amp;id=21") + "\">同人小说</a>");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            builder.Append(Out.Tab("<div class=\"title2\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">上级</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">书城</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }


    private void TopPage()
    {
        Master.Title = "书本排行榜";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>&gt;");
        builder.Append("排行榜");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        strWhere = "state>0 and isdel=0";
        // 开始读取列表
        IList<Book.Model.ShuMu> listShuMu = new Book.BLL.ShuMu().GetShuMusTop(pageIndex, pageSize, strWhere, out recordCount);
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

                builder.AppendFormat("{0}.<a href=\"" + Utils.getUrl("default.aspx?act=view&amp;id={1}&amp;nid={2}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{3}</a>("+n.click+")", ((pageIndex - 1) * pageSize) + k, n.nid, n.id, n.title);

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
        builder.Append(Out.Tab("<div class=\"title2\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">书城</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ListbPage()
    {
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 1, @"^[1-6]$", "1"));

        string sTitle = string.Empty;
        if (ptype == 1)
            sTitle = "全本小说";
        else if (ptype == 2)
            sTitle = "最新小说";
        else if (ptype == 3)
            sTitle = "精品小说";
        else if (ptype == 4)
            sTitle = "连载小说";
        else if (ptype == 5)
            sTitle = "原创小说";
        else if (ptype == 6)
            sTitle = "更新小说";

        Master.Title = sTitle;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>&gt;");
        builder.Append(sTitle);
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string strOrder = "";
        if (ptype == 1)
            strWhere = "state=1 and isover=1 and isdel=0";
        else if (ptype == 2)
            strWhere = "state=1"; 
        else if (ptype == 3)
            strWhere = "state=1 and isgood=1 and isdel=0";
        else if (ptype == 4)
            strWhere = "state=1 and isover=0 and isdel=0";
        else if (ptype == 5)
            strWhere = "state=1 and types=0 and isdel=0";
        else if (ptype == 6)
            strWhere = "state=1 and isdel=0";

        if (ptype == 6)
            strOrder = "gxtime Desc";
        else
            strOrder = "ID Desc";

        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<Book.Model.ShuMu> listShuMu = new Book.BLL.ShuMu().GetShuMus(pageIndex, pageSize, strWhere, strOrder, out recordCount);
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

                builder.AppendFormat("{0}.<a href=\"" + Utils.getUrl("default.aspx?act=view&amp;id={1}&amp;nid={2}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{3}</a>", ((pageIndex - 1) * pageSize) + k, n.nid, n.id, n.title);

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
        builder.Append(Out.Tab("<div class=\"title2\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">书城</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ListPage()
    {
        int id = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        Book.Model.Navigation model = new Book.BLL.Navigation().GetNavigation(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "" + model.Name + "";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>&gt;");
        builder.Append("" + model.Name + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        strWhere = "nid=" + id + " and state>0 and isdel=0";
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

                builder.AppendFormat("{0}.<a href=\"" + Utils.getUrl("default.aspx?act=view&amp;id={1}&amp;nid={2}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{3}</a>", ((pageIndex - 1) * pageSize) + k, n.nid, n.id, n.title);

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
        builder.Append(Out.Tab("<div class=\"title2\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">书城</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ViewPage()
    {
        string strText = string.Empty;
        string strName = string.Empty;
        string strType = string.Empty;
        string strValu = string.Empty;
        string strEmpt = string.Empty;
        string strIdea = string.Empty;
        string strOthe = string.Empty;

        int meid = new BCW.User.Users().GetUsId();
        int id = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int nid = Utils.ParseInt(Utils.GetRequest("nid", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int jid = Utils.ParseInt(Utils.GetRequest("jid", "all", 1, @"^[0-9]\d*$", "0"));
        int showtype = Utils.ParseInt(Utils.GetRequest("showtype", "all", 1, @"^[0-1]$", "0"));

        Book.Model.ShuMu m = new Book.BLL.ShuMu().GetShuMu(nid);
        if (m == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (m.isdel == 1)
        {
            Utils.Error("不存在的记录", "");
        }
        
        Book.Model.Navigation model = new Book.BLL.Navigation().GetNavigation(m.nid);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        id = m.nid;

        Master.Title = "" + m.title + "";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=list&amp;id=" + id + "") + "\">" + model.Name + "</a>&gt;");
        builder.Append(m.title);
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append(m.title);
        builder.Append(Out.Tab("</div>", "<br />"));

        if (m.img != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(" <img src=\"" + m.img + "\" alt=\"load\"/>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        if (m.aid == 0)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("作者：" + m.author + "");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            strText = "作者：,,,,,";
            strName = "uid,author,act,backurl";
            strType = "hidden2,hidden,hidden,hidden";
            strValu = "" + m.aid + "'" + m.author + "'more'" + Utils.PostPage(1) + "";
            strEmpt = "false,false,false,false,false";
            strIdea = "";
            strOthe = "" + m.author + ",default.aspx,post,3,other";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }


        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("性质：");
        if (m.types == 0)
            builder.Append("原创");
        else
            builder.Append("转载");

        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("状态：");
        if (m.isover == 0)
        {
            builder.Append("连载中");
            if (!string.IsNullOrEmpty(m.gxids))
            {
                if (!("#" + m.gxids + "#").Contains("#" + meid + "#"))
                {
                    builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=remind&amp;id=" + id + "&amp;nid=" + nid + "") + "\">[更新提醒]</a>");
                }
                else
                {
                    builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=remind&amp;ptype=1&amp;id=" + id + "&amp;nid=" + nid + "") + "\">[取消提醒]</a>");
                }
            }
        }
        else
        {
            builder.Append("已完结");
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("关键词：" + m.tags + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        if (m.length > 0)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("书本字数：" + m.length + "");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("书本简介：" + m.summary + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("最后更新：" + DT.FormatDate(m.gxtime, 11) + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("人气:" + m.click + "");
        builder.Append("/推荐:" + m.good + "");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=favadd&amp;id=" + id + "&amp;nid=" + nid + "") + "\">收藏到书架</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/function.aspx?act=recombook&amp;id=" + id + "&amp;nid=" + nid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">推荐</a>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        builder.Append("<b><a href=\"" + Utils.getUrl("default.aspx?act=bbs&amp;id=" + id + "&amp;nid=" + nid + "") + "\">共("+m.pl+"条)书评</a></b>");
        builder.Append(Out.Tab("</div>", "<br />"));
        strText = ",,,,,";
        strName = "content,id,nid,act,backurl";
        strType = "textarea,hidden,hidden,hidden,hidden";
        strValu = "'" + id + "'" + nid + "'bbssave'" + Utils.PostPage(1) + "";
        strEmpt = "true,false,false,false,false";
        strIdea = "";
        strOthe = "发表书评,default.aspx,post,3,other";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        builder.Append("=书本目录=");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("排序:");
        if (showtype == 0)
            builder.Append("正序<a href=\"" + Utils.getUrl("default.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + jid + "&amp;showtype=1") + "\">|倒序</a>");
        else
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + jid + "&amp;showtype=0") + "\">正序</a>|倒序");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = 10;
        string strWhere = "";
        string strOrder = "";
        string[] pageValUrl = { "act", "id", "nid", "jid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        strWhere = "shi=" + nid + "";
        if (jid == 0)
            strWhere += " and jid<=0 and state=1 and isdel=0";
        else
            strWhere += " and jid=" + jid + " and state=1 and isdel=0";

        if (showtype == 0)
            strOrder = "pid asc";
        else
            strOrder = "pid desc";

        // 开始读取列表
        IList<Book.Model.Contents> listContents = new Book.BLL.Contents().GetContentss(pageIndex, pageSize, strWhere, strOrder, out recordCount);
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
                    builder.AppendFormat("{0}<a href=\"" + Utils.getUrl("default.aspx?act=view&amp;id={1}&amp;nid={2}&amp;jid={3}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{4}</a>", "", id, nid, n.id, n.title);
                else
                    builder.AppendFormat("{0}<a href=\"" + Utils.getUrl("default.aspx?act=content&amp;id={1}&amp;nid={2}&amp;jid={3}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{4}</a>", "", id, nid, n.id, n.title);

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
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + "") + "\">书卷目录</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"title2\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx?act=list&amp;id=" + id + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">书城</a>");
        builder.Append(Out.Tab("</div>", ""));
        //更新人气
        new Book.BLL.ShuMu().UpdateClick(nid);

    }

    private void BbsPage()
    {
        string strText = string.Empty;
        string strName = string.Empty;
        string strType = string.Empty;
        string strValu = string.Empty;
        string strEmpt = string.Empty;
        string strIdea = string.Empty;
        string strOthe = string.Empty;

        int id = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int nid = Utils.ParseInt(Utils.GetRequest("nid", "all", 2, @"^[0-9]\d*$", "ID错误"));
        Book.Model.Navigation model = new Book.BLL.Navigation().GetNavigation(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        Book.Model.ShuMu m = new Book.BLL.ShuMu().GetShuMu(nid);
        if (m == null)
        {
            Utils.Error("不存在的记录", "");
        }

        Master.Title = "查看书评";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + "") + "\">" + m.title + "</a>");
        builder.Append("&gt;书评");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
       
        string[] pageValUrl = { "act", "id", "nid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
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
                builder.AppendFormat("{0}.<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid={1}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}</a>", ((pageIndex - 1) * pageSize) + k, n.usid, n.usname);
                builder.Append(":" + Out.SysUBB(n.content));
                builder.Append("(" + DT.FormatDate(n.addtime, 1) + ")");
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
        strText = ",,,,,";
        strName = "content,id,nid,act,backurl";
        strType = "textarea,hidden,hidden,hidden,hidden";
        strValu = "'" + id + "'" + nid + "'bbssave'" + Utils.PostPage(1) + "";
        strEmpt = "true,false,false,false,false";
        strIdea = "/";
        strOthe = "发表书评,default.aspx,post,3,other";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"title2\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + "") + "\">&gt;&gt;返回上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void BbsSavePage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int nid = Utils.ParseInt(Utils.GetRequest("nid", "all", 2, @"^[0-9]\d*$", "ID错误"));
        Book.Model.Navigation model = new Book.BLL.Navigation().GetNavigation(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        Book.Model.ShuMu m = new Book.BLL.ShuMu().GetShuMu(nid);
        if (m == null)
        {
            Utils.Error("不存在的记录", "");
        }
        string content = Utils.GetRequest("content", "post", 2, @"[\s\S]{1,300}$", "书评字数最多300字");

        //是否刷屏
        string appName = "LIGHT_BOOKBBS";
        int Expir = Utils.ParseInt(ub.GetSub("BookExpir", xmlPath));
        BCW.User.Users.IsFresh(appName, Expir);

        Book.Model.ShuBbs n = new Book.Model.ShuBbs();
        n.nid = nid;
        n.usid = meid;
        n.usname = new BCW.BLL.User().GetUsName(meid);
        n.content = content;
        n.addusip = Utils.GetUsIP();
        n.addtime = DateTime.Now;
        n.retext = "";
        new Book.BLL.ShuBbs().Add(n);
        //更新书评数目
        new Book.BLL.ShuMu().Updatepl(nid, 1);
        Utils.Success("发表书评", "发表书评成功，正在返回..", Utils.getUrl("default.aspx?act=bbs&amp;id=" + id + "&amp;nid=" + nid + ""), "1");

    }

    private void FavAddPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int nid = Utils.ParseInt(Utils.GetRequest("nid", "all", 2, @"^[0-9]\d*$", "ID错误"));
        Book.Model.Navigation model = new Book.BLL.Navigation().GetNavigation(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        Book.Model.ShuMu m = new Book.BLL.ShuMu().GetShuMu(nid);
        if (m == null)
        {
            Utils.Error("不存在的记录", "");
        }
        string info = Utils.GetRequest("info", "post", 1, "", "");
        if (info == "ok")
        {
            int favid = Utils.ParseInt(Utils.GetRequest("favid", "post", 1, @"^[0-9]\d*$", "0"));

            //判断是否重复收藏
            if (new Book.BLL.Favorites().Exists2(meid, id, nid))
            {
                Utils.Error("重复收藏<br /><a href=\"" + Utils.getUrl("default.aspx?act=shufav") + "\">进入我的书架&gt;&gt;</a>", "");
            }
            Book.Model.Favorites fav = new Book.Model.Favorites();
            fav.favid = favid;
            fav.types = 0;
            fav.title = m.title;
            fav.nid = id;
            fav.sid = nid;
            fav.purl = "";
            fav.usid = meid;
            fav.addtime = DateTime.Now;
            new Book.BLL.Favorites().Add(fav);

            Utils.Success("收藏到书架", "收藏到书架成功，正在返回..<br /><a href=\"" + Utils.getUrl("default.aspx?act=shufav") + "\">进入我的书架&gt;&gt;</a>", Utils.getUrl("default.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + ""), "3");
        }
        else
        {
            Master.Title = "收藏书本";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("收藏到书架");
            builder.Append(Out.Tab("</div>", ""));

            string sEmpt = "0|默认书架";
            DataSet ds = new Book.BLL.ShuFav().GetList("id,name", "usid=" + meid + "");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sEmpt += "|" + ds.Tables[0].Rows[i]["id"] + "|" + ds.Tables[0].Rows[i]["name"] + "";
                }
            }
            string strText = "请选择书架:/,,,,,";
            string strName = "favid,id,nid,act,info,backurl";
            string strType = "select,hidden,hidden,hidden,hidden,hidden";
            string strValu = "'" + id + "'" + nid + "'favadd'ok'" + Utils.getPage(0) + "";
            string strEmpt = "" + sEmpt + ",false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定收藏,default.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"title2\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + "") + "\">&gt;&gt;返回上级</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private void FavAdd2Page()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int nid = Utils.ParseInt(Utils.GetRequest("nid", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int jid = Utils.ParseInt(Utils.GetRequest("jid", "all", 1, @"^[0-9]\d*$", "0"));
        int vp = Utils.ParseInt(Utils.GetRequest("vp", "all", 1, @"^[0-9]\d*$", "0"));

        Book.Model.Navigation model = new Book.BLL.Navigation().GetNavigation(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        Book.Model.ShuMu m = new Book.BLL.ShuMu().GetShuMu(nid);
        if (m == null)
        {
            Utils.Error("不存在的记录", "");
        }
        Book.Model.Contents n = new Book.BLL.Contents().GetContents(jid);
        if (n == null)
        {
            Utils.Error("不存在的记录", "");
        }
        string purl = "/book/default.aspx?act=content&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + jid + "&amp;vp=" + vp + "";

        //判断是否重复收藏
        if (new Book.BLL.Favorites().Exists3(meid, id, nid, purl))
        {
            Utils.Error("重复收藏<br /><a href=\"" + Utils.getUrl("default.aspx?act=fav&amp;ptype=1") + "\">进入我的书签&gt;&gt;</a>", "");
        }
        Book.Model.Favorites fav = new Book.Model.Favorites();
        fav.favid = 0;
        fav.types = 1;
        fav.title = m.title;
        fav.nid = id;
        fav.sid = nid;
        fav.purl = purl;
        fav.usid = meid;
        fav.addtime = DateTime.Now;
        new Book.BLL.Favorites().Add(fav);

        Utils.Success("收藏书签", "收藏书签成功，正在返回..<br /><a href=\"" + Utils.getUrl("default.aspx?act=fav&amp;ptype=1") + "\">进入我的书签&gt;&gt;</a>", Utils.getUrl("default.aspx?act=content&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + jid + "&amp;vp=" + vp + ""), "3");

    }

    private void RemindPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Book.Model.ShuSelf model2 = new Book.BLL.ShuSelf().GetShuSelf(meid);
        if (model2.id == 0)
        {
            Utils.Success("正在跳转", "请先修改个人资料再继续", Utils.getUrl("default.aspx?act=editbasic"), "1");
        }
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 1, @"^[0-1]\d*$", "0"));
        int id = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int nid = Utils.ParseInt(Utils.GetRequest("nid", "all", 2, @"^[0-9]\d*$", "ID错误"));
        Book.Model.Navigation model = new Book.BLL.Navigation().GetNavigation(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        Book.Model.ShuMu m = new Book.BLL.ShuMu().GetShuMu(nid);
        if (m == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (ptype == 0)
        {
            string gxids = "";
            if (!string.IsNullOrEmpty(m.gxids))
            {
                if (("#" + m.gxids + "#").Contains("#" + meid + "#"))
                {
                    Utils.Error("已设置更新提醒，无需重复设置", "");
                }
                gxids = m.gxids + "#" + meid;
            }
            else
                gxids = meid.ToString();

            new Book.BLL.ShuMu().Updategxids(nid, gxids);

            Utils.Success("设置更新提醒", "设置更新提醒成功，正在返回..", Utils.getUrl("default.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + ""), "2");
        }
        else
        {
            string gxids = "";
            if (!string.IsNullOrEmpty(m.gxids))
            {
                if (!("#" + m.gxids + "#").Contains("#" + meid + "#"))
                {
                    Utils.Error("未设置更新提醒，无需设置取消提醒", "");
                }
                gxids = ("#" + m.gxids + "#").Replace("#" + meid + "#", "#");
                gxids = Utils.Mid(gxids, 1, gxids.Length);
                gxids = Utils.DelLastChar(gxids, "#");
                new Book.BLL.ShuMu().Updategxids(nid, gxids);
            }
            else
            {
                Utils.Error("未设置更新提醒，无需设置取消提醒", "");
            }
            Utils.Success("取消更新提醒", "取消更新提醒成功，正在返回..", Utils.getUrl("default.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + ""), "2");
        }
    }

    private void GoodPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int nid = Utils.ParseInt(Utils.GetRequest("nid", "all", 2, @"^[0-9]\d*$", "ID错误"));
        Book.Model.Navigation model = new Book.BLL.Navigation().GetNavigation(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        Book.Model.ShuMu m = new Book.BLL.ShuMu().GetShuMu(nid);
        if (m == null)
        {
            Utils.Error("不存在的记录", "");
        }
        new Book.BLL.ShuMu().UpdateGood(nid);

        Utils.Success("推荐书本", "推荐成功，正在返回..", Utils.getUrl("default.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + ""), "1");

    }

    private void ContentPage()
    {
        int meid = new BCW.User.Users().GetUsId();

        int id = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int nid = Utils.ParseInt(Utils.GetRequest("nid", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int jid = Utils.ParseInt(Utils.GetRequest("jid", "all", 1, @"^[0-9]\d*$", "0"));
        Book.Model.ShuMu m = new Book.BLL.ShuMu().GetShuMu(nid);
        if (m == null)
        {
            Utils.Error("不存在的记录", "");
        }
        Book.Model.Navigation model = new Book.BLL.Navigation().GetNavigation(m.nid);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        Book.Model.Contents n = new Book.BLL.Contents().GetContents(jid);
        if (n == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (n.isdel == 1)
        {
            Utils.Error("不存在的记录", "");
        }
        if (n.state == 0)
        {
            Utils.Error("章节尚未审核", "");
        }
        id = m.nid;
        Master.Title = "" + m.title + "";

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=list&amp;id=" + id + "") + "\">" + model.Name + "</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + "") + "\">" + m.title + "</a>");
        builder.Append(Out.Tab("</div>", "<br />"));


        int pageIndex;
        int recordCount;
        int pover = int.Parse(Utils.GetRequest("pover", "get", 1, @"^[0-9]\d*$", "0"));
        string[] pageValUrl = { "act", "id", "nid", "jid", "pw", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["vp"]);
        if (pageIndex == 0)
            pageIndex = 1;

        int pn = Utils.ParseInt(Request.QueryString["pn"]);
        if (pn == 0)
            pn = 1;

        int pw = Utils.ParseInt(Request.QueryString["pw"]);
        if (pw == 0)
        {
            if (meid > 0)
            {
                pw = new Book.BLL.ShuSelf().GetPageNum(meid);
            }
            else
            {
                if (HttpContext.Current.Request.Cookies["BookComment"] != null && HttpContext.Current.Request.Cookies["BookComment"]["pw"] != null)
                {
                    pw = Utils.ParseInt(HttpContext.Current.Request.Cookies["BookComment"]["pw"].ToString());
                }
            }
        }
        if (pw == 0 || pw < 100 || pw > 10000)
        {

            pw = Utils.ParseInt(ub.GetSub("BookPwNum", xmlPath));
        }
        int pageSize = pw;

        //记录本次阅读缓存
        HttpCookie cookie = new HttpCookie("BookContent");
        cookie.Expires = DateTime.Now.AddDays(365);
        cookie.Values.Add("zurl", Server.UrlEncode("default.aspx?act=content&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + jid + "&amp;vp=" + pageIndex + ""));
        Response.Cookies.Add(cookie);


        //下章
        Book.Model.Contents xx = new Book.BLL.Contents().GetPreviousNextContents(jid, nid, n.jid, true);
        // 上章
        Book.Model.Contents ss = new Book.BLL.Contents().GetPreviousNextContents(jid, nid, n.jid, false);

        builder.Append(Out.Tab("<div>", ""));
        if (!string.IsNullOrEmpty(xx.title))
        {
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=content&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + xx.id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">下章</a>");
        }
        else
            builder.Append("下章");

        builder.Append("|<a href=\"" + Utils.getUrl("default.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + "") + "\">目录</a>|");

        if (!string.IsNullOrEmpty(ss.title))
        {
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=content&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + ss.id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">上章</a>");
        }
        else
            builder.Append("上章");

        builder.Append("|<a href=\"" + Utils.getUrl("default.aspx?act=favadd2&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + jid + "&amp;vp=" + pageIndex + "") + "\">加书签</a>");

        builder.Append(Out.Tab("</div>", Out.Hr()));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append(n.title);
        builder.Append(Out.Tab("</div>", "<br />"));

        string content = BasePage.MultiContent(n.contents, pageIndex, pageSize, pover, out recordCount);
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.SysUBB(content).Replace(" ", "&#160;"));
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(BasePage.MultiContentPage(n.contents, pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "vp", pover));

        builder.Append(Out.Tab("<div>", "<br />"));
        if (!string.IsNullOrEmpty(xx.title))
        {
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=content&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + xx.id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">下章</a>");
        }
        else
            builder.Append("下章");

        builder.Append("|<a href=\"" + Utils.getUrl("default.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + "") + "\">目录</a>|");

        if (!string.IsNullOrEmpty(ss.title))
        {
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=content&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + ss.id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">上章</a>");
        }
        else
            builder.Append("上章");

        builder.Append("|<a href=\"" + Utils.getUrl("default.aspx?act=favadd2&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + jid + "&amp;vp=" + pageIndex + "") + "\">加书签</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=setpw&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + jid + "&amp;pw=" + pw + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">设置</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=content&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + jid + "&amp;pw=3000") + "\">大</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=content&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + jid + "&amp;pw=1500") + "\">中</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=content&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + jid + "&amp;pw=800") + "\">小</a>|");
        builder.Append("" + pw + "字/页");
        builder.Append(Out.Tab("</div>", ""));

        //builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        //builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=list&amp;id=" + id + "") + "\">分类</a>&gt;");
        //builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + "") + "\">书本</a>");
        //if (n.jid > 0)
        //{
        //    builder.Append("&gt;<a href=\"" + Utils.getUrl("default.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + n.jid + "") + "\">" + new Book.BLL.Contents().GetTitle(Convert.ToInt32(n.jid)) + "</a>");
        //}
        //builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=list&amp;id=" + id + "") + "\">" + model.Name + "</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + "") + "\">" + m.title + "</a>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"title2\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx?act=list&amp;id=" + id + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">书城</a>");
        builder.Append(Out.Tab("</div>", ""));
        //更新人气
        new Book.BLL.ShuMu().UpdateClick(nid);

    }


    private void SetPwPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int nid = int.Parse(Utils.GetRequest("nid", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int jid = int.Parse(Utils.GetRequest("jid", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int pw = int.Parse(Utils.GetRequest("pw", "all", 2, @"^[0-9]\d*$", "字数错误"));
        Master.Title = "字数设置";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("翻页字数设置");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("当前设置:" + pw + "字/页<br />=自定义设置=");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "每页(100-10000字):/,,,,,";
        string strName = "pw,id,nid,jid,act";
        string strType = "num,hidden,hidden,hidden,hidden";
        string strValu = "" + pw + "'" + id + "'" + nid + "'" + jid + "'setpwsave'";
        string strEmpt = "false,false,false,false,false";
        string strIdea = "字''''|/";
        string strOthe = "马上设置,default.aspx,get,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        builder.Append("温馨提示：浏览器开启缓存（cookies）可以保存每页字数<br />");
        builder.Append("还可以永久<a href=\"" + Utils.getUrl("default.aspx?act=setpagenum") + "\">设置默认字数</a>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"title2\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("default.aspx?act=content&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + jid + "&amp;pw=" + pw + "") + "\">&gt;返回阅读</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void SetPwSavePage()
    {
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int nid = int.Parse(Utils.GetRequest("nid", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int jid = int.Parse(Utils.GetRequest("jid", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int pw = int.Parse(Utils.GetRequest("pw", "all", 2, @"^[0-9]\d*$", "字数错误"));
        if (pw == 0 || pw < 100 || pw > 10000)
        {
            Utils.Error("数字限100-10000字", "");
        }
        //记录缓存
        HttpCookie cookie = new HttpCookie("BookComment");
        cookie.Expires = DateTime.Now.AddDays(365);
        cookie.Values.Add("pw", pw.ToString());
        Response.Cookies.Add(cookie);

        Utils.Success("设置字数", "设置字数成功，正在返回阅读..", Utils.getUrl("default.aspx?act=content&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + jid + "&amp;pw=" + pw + ""), "1");
    }


    private void FavPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = Utils.ParseInt(Utils.GetRequest("id", "all", 1, @"^[0-9]\d*$", "0"));
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 1, @"^[0-1]$", "0"));
        int showtype = Utils.ParseInt(Utils.GetRequest("showtype", "all", 1, @"^[0-1]$", "0"));

        string FavName = "";
        if (ptype == 0)
        {
            if (id > 0)
            {
                Book.Model.ShuFav model = new Book.BLL.ShuFav().GetShuFav(id);
                if (model == null)
                {
                    Utils.Error("不存在的记录", "");
                }
                if (model.usid != meid)
                {
                    Utils.Error("不存在的记录", "");
                }
                FavName = model.name;
            }
            else
                FavName = "默认书架";
        }
        else
        {
            FavName = "我的书签";
        }
        Master.Title = FavName;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=home") + "\">书房</a>&gt;<a href=\"" + Utils.getUrl("default.aspx?act=shufav") + "\">书架</a>&gt;" + FavName + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "ptype", "showtype", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        strWhere = "usid=" + meid + " and types=" + ptype + "";
        if (ptype == 0 && id > 0)
            strWhere += " and favid=" + id + "";

        // 开始读取列表
        IList<Book.Model.Favorites> listFavorites = new Book.BLL.Favorites().GetFavoritess(pageIndex, pageSize, strWhere, out recordCount);
        if (listFavorites.Count > 0)
        {
            int k = 1;
            foreach (Book.Model.Favorites n in listFavorites)
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

                if (n.types == 0)
                    builder.AppendFormat("{0}.<a href=\"" + Utils.getUrl("default.aspx?act=view&amp;id={1}&amp;nid={2}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{3}</a>", ((pageIndex - 1) * pageSize) + k, n.nid, n.sid, n.title);
                else
                    builder.AppendFormat("{0}.<a href=\"" + Utils.getUrl("{1}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}</a>{3}", ((pageIndex - 1) * pageSize) + k, n.purl, n.title, DT.FormatDate(n.addtime, 1));

                if (showtype == 1)
                {
                    builder.AppendFormat("[<a href=\"" + Utils.getUrl("default.aspx?act=favdel&amp;id={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">删</a>]", n.id);
                }
                else
                {
                    if (ptype == 1)
                    {
                        builder.AppendFormat("<a href=\"" + Utils.getUrl("{0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">继续&gt;&gt;</a>", n.purl);
                    }
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
        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        if (showtype == 0)
        {
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=fav&amp;ptype=" + ptype + "&amp;id=" + id + "&amp;showtype=1") + "\">切换管理&gt;&gt;</a>");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=favdel2&amp;ptype=" + ptype + "&amp;id=" + id + "&amp;page=" + pageIndex + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[审核本页收藏]</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=fav&amp;ptype=" + ptype + "&amp;id=" + id + "&amp;showtype=0") + "\">切换普通&gt;&gt;</a>");
        }
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"title2\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx?act=shufav") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">书城</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void FavDelPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        Book.Model.Favorites m = new Book.BLL.Favorites().GetFavorites(id);
        if (m == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (m.usid != meid)
        {
            Utils.Error("不存在的记录", "");
        }
        if (info != "ok")
        {
            Master.Title = "删除收藏";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此收藏记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx?info=ok&amp;act=favdel&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("default.aspx?act=fav") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            //删除
            new Book.BLL.Favorites().Delete(id);
            Utils.Success("删除收藏", "删除收藏成功..", Utils.getPage("default.aspx?act=fav"), "1");
        }
    }

    private void FavDel2Page()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Master.Title = "删除本页收藏";
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 1, @"^[0-1]$", "0"));
        int page = int.Parse(Utils.GetRequest("page", "get", 1, @"^[1-9]\d*$", "页面ID错误"));
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));

        string info = Utils.GetRequest("info", "get", 1, "", "");
        if (info != "ok")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除本页收藏吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=favdel2&amp;info=ok&amp;ptype=" + ptype + "&amp;id=" + id + "&amp;page=" + page + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("default.aspx?act=favdel2&amp;ptype=" + ptype + "&amp;id=" + id + "&amp;showtype=1") + "\">再看看吧..</a>");
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

            //查询条件
            strWhere = "usid=" + meid + " and types=" + ptype + "";
            if (ptype == 0 && id > 0)
                strWhere += " and favid=" + id + "";

            // 开始读取列表
            IList<Book.Model.Favorites> listFavorites = new Book.BLL.Favorites().GetFavoritess(pageIndex, pageSize, strWhere, out recordCount);
            if (listFavorites.Count > 0)
            {
                foreach (Book.Model.Favorites n in listFavorites)
                {
                    new Book.BLL.Favorites().Delete(n.id);
                }
            }

            Utils.Success("删除本页收藏", "删除本页收藏成功，正在返回..", Utils.getPage("default.aspx?act=favdel2&amp;ptype=" + ptype + "&amp;id=" + id + "&amp;showtype=1"), "1");
        }
    }


    private void SearchPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 1, @"^[0-2]$", "0"));

        Master.Title = "搜书";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("搜书");
        builder.Append(Out.Tab("</div>", ""));
        if (info == "ok")
        {
            string keyword = Utils.GetRequest("keyword", "all", 1, @"^[\s\S]{1,30}$", "关键字填写错误");

            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string strWhere = "";
            string[] pageValUrl = { "act", "info", "ptype", "keyword", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            //查询条件
            if (ptype == 0)
                strWhere += "title LIKE '%" + keyword + "%' and isdel=0";
            else if (ptype == 1)
                strWhere += "author LIKE '%" + keyword + "%' and isdel=0";
            else if (ptype == 2)
                strWhere += "tags LIKE '%" + keyword + "%' and isdel=0";

            strWhere += " and state>0";
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
                        builder.Append(Out.Tab("<div>", "<br />"));

                    builder.AppendFormat("{0}.<a href=\"" + Utils.getUrl("default.aspx?act=view&amp;id={1}&amp;nid={2}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{3}</a>", ((pageIndex - 1) * pageSize) + k, n.nid, n.id, n.title);

                    k++;
                    builder.Append(Out.Tab("</div>", ""));

                }
                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Tab("", "<br />"));
                builder.Append(Out.Div("div", "没有相关记录.."));
            }
            builder.Append(Out.Tab("<div class=\"title2\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=search") + "\">继续搜索&gt;&gt;</a>");
            builder.Append(Out.Tab("</div>", ""));

        }
        else
        {
            string strText = "关键词:/,搜索类型:/,,,";
            string strName = "keyword,ptype,act,info,backurl";
            string strType = "text,select,hidden,hidden,hidden";
            string strValu = "'0'search'ok'" + Utils.getPage(0) + "";
            string strEmpt = "true,0|按书名|1|按作者|2|书本关键词,false,false,false";
            string strIdea = "/";
            string strOthe = "搜书,default.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        }
        builder.Append(Out.Tab("<div class=\"title2\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">书城</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void MorePage()
    {
        int uid = Utils.ParseInt(Utils.GetRequest("uid", "all", 1, @"^[0-9]\d*$", "0"));
        string author = Utils.GetRequest("author", "all", 2, @"[\s\S]{1,}", "提交错误");
        Master.Title = "作者书本";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("作者：<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + uid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + author + "</a>");
        builder.Append(Out.Tab("</div>", ""));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "uid", "author", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        strWhere = "aid=" + uid + "";
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
                    builder.Append(Out.Tab("<div>", "<br />"));

                builder.AppendFormat("{0}.<a href=\"" + Utils.getUrl("default.aspx?act=view&amp;id={1}&amp;nid={2}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{3}</a>", ((pageIndex - 1) * pageSize) + k, n.nid, n.id, n.title);

                k++;
                builder.Append(Out.Tab("</div>", ""));

            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Tab("", "<br />"));
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div class=\"title2\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">书城</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void HomePage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int aid = Utils.ParseInt(Utils.GetRequest("aid", "all", 1, @"^[0-9]\d*$", "0"));
        if (aid == 0)
            aid = meid;

        Book.Model.ShuSelf model = new Book.BLL.ShuSelf().GetShuSelf(aid);

        Master.Title = "我的书房";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("【个人资料】");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(""+model.name+"("+aid+")<br />");
        builder.Append("" + model.sex + "/" + model.city + "");
        if (aid == meid)
        {
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=editbasic") + "\">[修改]</a>");
        }
        int iday = 0;
        if (model.addtime != DateTime.Parse("1990-1-1"))
        {
            TimeSpan sp = DateTime.Now - model.addtime;
            iday = sp.Days;
        }
        builder.Append("<br />书龄:" + iday + "天");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("【我的应用】");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        if (HttpContext.Current.Request.Cookies["BookContent"] != null && HttpContext.Current.Request.Cookies["BookContent"]["zurl"] != null)
        {
            string zurl = Server.UrlDecode(HttpContext.Current.Request.Cookies["BookContent"]["zurl"].ToString());
            builder.Append("<a href=\"" + Utils.getUrl(zurl) + "\">继续上次阅读&gt;&gt;</a><br />");
        }

        builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=setpagenum") + "\">字数设置</a><br />");

        int cNum = 0;
        if (model.gxids != "")
        {
            cNum = Utils.GetStringNum(model.gxids, "#");
            if (cNum == 0)
                cNum = 1;
            else
                cNum = cNum + 1;
        }

        builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=getremind") + "\">更新提醒(" + cNum + ")</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("【书籍管理】");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=shufav&amp;showtype=1") + "\">书架藏书</a>(<a href=\"" + Utils.getUrl("default.aspx?act=shufav") + "\">" + new Book.BLL.Favorites().GetCount(meid, 0) + "本</a>) <br />");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=fav&amp;ptype=1&amp;showtype=1") + "\">我的书签</a>(<a href=\"" + Utils.getUrl("default.aspx?act=fav&amp;ptype=1") + "\">" + new Book.BLL.Favorites().GetCount(meid, 1) + "条</a>)");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("【<a href=\"" + Utils.getUrl("bookman.aspx") + "\">作者专区</a>】");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"title2\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">书城</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void EditBasicPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Book.Model.ShuSelf model = new Book.BLL.ShuSelf().GetShuSelf(meid);

        Master.Title = "编辑个人资料";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("编辑个人资料");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "昵称:,性别:,城市:,";
        string strName = "name,sex,city,act";
        string strType = "text,select,text,hidden";
        string strValu = "";
        if (model.id == 0)
            strValu = "'男''editbasicsave'";
        else
            strValu = "" + model.name + "'" + model.sex + "'" + model.city + "'editbasicsave'";

        string strEmpt = "false,男|男|女|女,false,false";
        string strIdea = "/";
        string strOthe = "确定修改,default.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"title2\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=home") + "\">&gt;&gt;返回书房</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void EditBasicSavePage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Book.Model.ShuSelf model = new Book.BLL.ShuSelf().GetShuSelf(meid);

        string name = Utils.GetRequest("name", "post", 2, @"^[\s\S]{1,10}$", "昵称限1-10字");
        string sex = Utils.GetRequest("sex", "post", 2, @"^男|女$", "性别选择错误");
        string city = Utils.GetRequest("city", "post", 2, @"^[\s\S]{2,10}$", "城市限2-10字");

        model.aid = meid;
        model.name = name;
        model.sex = sex;
        model.city = city;
        if (model.id == 0)
            new Book.BLL.ShuSelf().Add(model);
        else
            new Book.BLL.ShuSelf().Update(model);

        Utils.Success("修改资料", "修改资料成功..", Utils.getUrl("default.aspx?act=home"), "1");

    }

    private void SetPageNumPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Book.Model.ShuSelf model = new Book.BLL.ShuSelf().GetShuSelf(meid);
        if (model.id == 0)
        {
            Utils.Success("正在跳转", "请先修改个人资料再进行设置", Utils.getUrl("default.aspx?act=editbasic"), "1");
        }

        Master.Title = "设置默认字数";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("设置默认字数");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "字数:,";
        string strName = "pagenum,act";
        string strType = "num,hidden";
        string strValu = "";
        strValu = "" + model.pagenum + "'setpagenumsave'";
        string strEmpt = "false,false";
        string strIdea = "/";
        string strOthe = "确定修改,default.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"title2\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=home") + "\">&gt;&gt;返回书房</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void SetPageNumSavePage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int pagenum = Utils.ParseInt(Utils.GetRequest("pagenum", "post", 2, @"^[1-9]\d*$", "字数填写错误"));
        new Book.BLL.ShuSelf().UpdatePageNum(meid, pagenum);

        Utils.Success("设置默认字数", "设置默认字数成功..", Utils.getUrl("default.aspx?act=home"), "1");

    }

    private void GetRemindPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Book.Model.ShuSelf model = new Book.BLL.ShuSelf().GetShuSelf(meid);
        if (model.id == 0)
        {
            Utils.Success("正在跳转", "请先修改个人资料", Utils.getUrl("default.aspx?act=editbasic"), "1");
        }
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[0-2]$", "0"));
        if (ptype == 1)
        {
            new Book.BLL.ShuSelf().Updategxids(meid, "");
            Utils.Success("清空提醒", "清空全部提醒成功..", Utils.getUrl("default.aspx?act=getremind"), "1");        
        }
        else if (ptype == 2)
        {
            int id = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
            int nid = Utils.ParseInt(Utils.GetRequest("nid", "all", 2, @"^[0-9]\d*$", "ID错误"));
            if (model.gxids != "")
            {
                string gxids = "";
                if (("#" + model.gxids + "#").Contains("#" + nid + "#"))
                {
                    gxids = ("#" + model.gxids + "#").Replace("#" + nid + "#", "#");
                    gxids = Utils.Mid(gxids, 1, gxids.Length);
                    gxids = Utils.DelLastChar(gxids, "#");
                    new Book.BLL.ShuSelf().Updategxids(meid, gxids);
                }
            }
            Response.Redirect(Utils.getUrl("default.aspx?act=view&id=" + id + "&nid=" + nid + "&backurl=" + Utils.PostPage(1) + ""));

        }
        Master.Title = "我的更新提醒";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=home") + "\">书房</a>&gt;提醒");
        builder.Append(Out.Tab("</div>", ""));
        string getids = model.gxids;
        if (getids != "")
        {
            string[] sName = getids.Split("#".ToCharArray());

            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string[] pageValUrl = { "act" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            //总记录数
            recordCount = sName.Length;

            int stratIndex = (pageIndex - 1) * pageSize;
            int endIndex = pageIndex * pageSize;
            int k = 0;
            for (int i = 0; i < sName.Length; i++)
            {
                if (k >= stratIndex && k < endIndex)
                {
                    if ((k + 1) % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));

                    int nid = Utils.ParseInt(sName[i]);
                    Book.Model.ShuMu m = new Book.BLL.ShuMu().GetShuMu(nid);
                    if (m != null)
                    {
                        builder.Append("" + (i + 1) + ".");
                        builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=getremind&amp;ptype=2&amp;id=" + m.id + "&amp;nid=" + m.nid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + m.title + "|更新:" + DT.FormatDate(m.gxtime, 1) + "</a>");
                        builder.Append(Out.Tab("</div>", ""));
                    }
                }
                if (k == endIndex)
                    break;
                k++;
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Tab("", "<br />"));
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div class=\"title2\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=getremind&amp;ptype=1") + "\">[清空全部提醒]</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title2\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">书城</a>");
        builder.Append(Out.Tab("</div>", ""));
    }


    private void ShuFavPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int showtype = Utils.ParseInt(Utils.GetRequest("showtype", "all", 1, @"^[0-1]$", "0"));

        Master.Title = "我的书架";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=home") + "\">书房</a>&gt;书架");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=fav&amp;backurl=" + Utils.PostPage(1) + "") + "\">默认分类(" + new Book.BLL.Favorites().GetCount(0) + ")</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "showtype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        strWhere = "usid=" + meid + "";
        // 开始读取列表
        IList<Book.Model.ShuFav> listShuFav = new Book.BLL.ShuFav().GetShuFavs(pageIndex, pageSize, strWhere, out recordCount);
        if (listShuFav.Count > 0)
        {
            int k = 1;
            foreach (Book.Model.ShuFav n in listShuFav)
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
                builder.AppendFormat("<a href=\"" + Utils.getUrl("default.aspx?act=fav&amp;id={0}&amp;showtype=" + showtype + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">{1}(" + new Book.BLL.Favorites().GetCount(n.id) + ")</a>", n.id, n.name);
                if (showtype == 1)
                {
                    builder.AppendFormat("[<a href=\"" + Utils.getUrl("default.aspx?act=shufavedit&amp;id={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">改</a>]", n.id);
                    builder.AppendFormat("[<a href=\"" + Utils.getUrl("default.aspx?act=shufavdel&amp;id={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">删</a>]", n.id);
                }
                k++;
                builder.Append(Out.Tab("</div>", ""));

            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 1));
            builder.Append(Out.Tab("", "<br />"));
        }
        builder.Append(Out.Tab("<div class=\"text\">", Out.RHr()));
        if (showtype == 0)
        {
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=shufav&amp;showtype=1") + "\">切换管理&gt;&gt;</a>");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=shufavadd") + "\">添加书架分类</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=shufav&amp;showtype=0") + "\">切换普通&gt;&gt;</a>");
        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title2\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">书城</a>");
        builder.Append(Out.Tab("</div>", ""));
    }


    private void ShuFavAddPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Master.Title = "添加书架分类";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("添加书架分类");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "书架名称:/,";
        string strName = "name,act";
        string strType = "text,hidden";
        string strValu = "'shufavaddsave'";
        string strEmpt = "false,false";
        string strIdea = "/";
        string strOthe = "确定添加,default.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"title2\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=shufav&amp;showtype=1") + "\">&gt;&gt;返回书架</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ShuFavAddSavePage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string name = Utils.GetRequest("name", "post", 2, @"^[\s\S]{1,10}$", "分类名称1-10字");

        //最多20个书架
        int ShuFavNum = new Book.BLL.ShuFav().GetCount(meid);
        if (ShuFavNum > 20)
        {
            Utils.Error("最多可以添加20个书架", "");
        }
        Book.Model.ShuFav model = new Book.Model.ShuFav();
        model.name = name;
        model.usid = meid;
        new Book.BLL.ShuFav().Add(model);
        Utils.Success("添加书架", "添加书架成功..", Utils.getUrl("default.aspx?act=shufav&amp;showtype=1"), "1");
    }

    private void ShuFavEditPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = Utils.ParseInt(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        Book.Model.ShuFav model = new Book.BLL.ShuFav().GetShuFav(id);
        if (model == null)
        {
            Utils.Error("不存在的书架分类", "");
        }
        if (model.usid != meid)
        {
            Utils.Error("不存在的书架分类", "");
        }
        Master.Title = "编辑书架分类";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("编辑书架分类");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "书架名称:/,,";
        string strName = "name,id,act";
        string strType = "text,hidden,hidden";
        string strValu = ""+model.name+"'"+id+"'shufaveditsave'";
        string strEmpt = "false,false,false";
        string strIdea = "/";
        string strOthe = "确定修改,default.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"title2\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=shufav&amp;showtype=1") + "\">&gt;&gt;返回书架</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ShuFavEditSavePage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = Utils.ParseInt(Utils.GetRequest("id", "post", 2, @"^[0-9]\d*$", "ID错误"));
        Book.Model.ShuFav model = new Book.BLL.ShuFav().GetShuFav(id);
        if (model == null)
        {
            Utils.Error("不存在的书架分类", "");
        }
        if (model.usid != meid)
        {
            Utils.Error("不存在的书架分类", "");
        }
        string name = Utils.GetRequest("name", "post", 2, @"^[\s\S]{1,10}$", "分类名称1-10字");
    
        model.id = id;
        model.name = name;
        model.usid = meid;
        new Book.BLL.ShuFav().Update(model);
        Utils.Success("修改书架", "修改书架成功..", Utils.getUrl("default.aspx?act=shufav&amp;showtype=1"), "1");
    }

    private void ShuFavDelPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        Book.Model.ShuFav model = new Book.BLL.ShuFav().GetShuFav(id);
        if (model == null)
        {
            Utils.Error("不存在的书架分类", "");
        }
        if (model.usid != meid)
        {               
            Utils.Error("不存在的书架分类", "");
        }
        if (info != "ok")
        {
            Master.Title = "删除书架";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("删除书架将同时删除书架的收藏记录，确定删除此书架吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx?info=ok&amp;act=shufavdel&amp;id=" + id + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=shufav&amp;showtype=1") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            //删除
            new Book.BLL.ShuFav().Delete(id);
            new Book.BLL.Favorites().Delete("favid=" + id + "");
            Utils.Success("删除书架", "删除书架成功..", Utils.getUrl("default.aspx?act=shufav&amp;showtype=1"), "1");
        }
    }
}
