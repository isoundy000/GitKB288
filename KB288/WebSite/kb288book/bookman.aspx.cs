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

public partial class book_bookman : System.Web.UI.Page
{
    protected string xmlPath = "/Controls/book.xml";
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Master.Title = "作者专区";
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "list":
                ListPage(meid);
                break;
            case "addshumu":
                AddShuMuPage(meid);
                break;
            case "addshumusave":
                AddShuMuSavePage(meid);
                break;
            case "editshumu":
                EditShuMuPage(meid);
                break;
            case "editshumusave":
                EditShuMuSavePage(meid);
                break;
            case "delshumu":
                DelShuMuPage(meid);
                break;
            case "view":
                ViewPage(meid);
                break;
            case "addjuan":
                AddJuanPage(meid);
                break;
            case "addjuansave":
                AddJuanSavePage(meid);
                break;
            case "editjuan":
                EditJuanPage(meid);
                break;
            case "editjuansave":
                EditJuanSavePage(meid);
                break;
            case "deljuan":
                DelJuanPage(meid);
                break;
            case "addcontent":
                AddContentPage(meid);
                break;
            case "addcontentsave":
                AddContentSavePage(meid);
                break;
            case "editcontent":
                EditContentPage(meid);
                break;
            case "editcontentsave":
                EditContentSavePage(meid);
                break;
            case "delcontent":
                DelContentPage(meid);
                break;
            default:
                ReloadPage(meid);
                break;
        }
    }

    private void ReloadPage(int meid)
    {
        Master.Title = "作者专区";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("请选择分类写书");
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

                builder.AppendFormat("<a href=\"" + Utils.getUrl("bookman.aspx?act=list&amp;id={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{1}</a>", n.id, n.Name);
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
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回书城中心</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ListPage(int meid)
    {
        int id = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        Book.Model.Navigation model = new Book.BLL.Navigation().GetNavigation(id);
        if (model == null)
        {
            Utils.Error("不存在的分类", "");
        }
        Master.Title = "写书-" + model.Name + "";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("bookman.aspx") + "\">分类</a>&gt;");
        builder.Append("" + model.Name + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("书本列表");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title2\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("bookman.aspx?act=addshumu&amp;id=" + id + "") + "\">添加书本&gt;&gt;</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = {"act", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        strWhere = "nid=" + id + " and aid=" + meid + " and isdel=0";
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
                builder.Append("<a href=\"" + Utils.getUrl("bookman.aspx?act=editshumu&amp;id=" + n.nid + "&amp;nid=" + n.id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[管理]&gt;</a>");
                builder.AppendFormat("{0}.<a href=\"" + Utils.getUrl("bookman.aspx?act=view&amp;id={1}&amp;nid={2}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{3}</a>", (pageIndex - 1) * pageSize + k, n.nid,n.id, n.title);
                if (n.state == 0)
                    builder.Append("[未审核]");

                if (n.isover == 1)
                    builder.Append("[已完结]");

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
        builder.Append("<a href=\"" + Utils.getUrl("bookman.aspx") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回书城中心</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void AddShuMuPage(int meid)
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
            builder.Append("<a href=\"bookman.aspx?act=addshumu&amp;id=" + id + "&amp;" + VE + "=20a&amp;" + SID + "=" + Utils.getstrU() + "\">[切换2.0彩版]</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("bookman.aspx?act=list&amp;id=" + id + "") + "\">" + model.Name + "</a>&gt;");
            builder.Append("添加书本");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "书本名称:/,书本简介:/,书本封面:/,书本作者:/,书本字数(未知请填0):/,搜索关键词(多个用#分开):/,书本状态:/,书本性质:/,上传会员ID(没有请填0):/,,";
            string strName = "title,summary,img,author,length,tags,state,types,aid,id,act";
            string strType = "text,textarea,file,text,num,text,hidden,select,hidden,hidden,hidden";
            string strValu = "''''0''0'0'0'" + id + "'addshumusave";
            string strEmpt = "true,true,true,true,true,true,0|未审核|1|已审核,0|原创|1|转载,false,false,false";
            string strIdea = "/";
            string strOthe = "添加书本,bookman.aspx,post,2,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + ReplaceWap(Utils.getUrl("bookman.aspx?act=list&amp;id=" + id + "")) + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + ReplaceWap(Utils.getUrl("default.aspx")) + "\">返回书城中心</a>");
        builder.Append(Out.Tab("</div>", ""));

    }

    private void AddShuMuSavePage(int meid)
    {
        int id = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        string title = Utils.GetRequest("title", "all", 2, @"^[\s\S]{1,200}$", "书本名称限1-200字");
        string summary = Utils.GetRequest("summary", "all", 2, @"^[\s\S]{1,500}$", "书本简介限1-500字");
        string author = Utils.GetRequest("author", "all", 2, @"^[\s\S]{1,50}$", "书本作者限1-50字");
        int length = Utils.ParseInt(Utils.GetRequest("length", "all", 2, @"^[0-9]\d*$", "字数填写错误"));
        string tags = Utils.GetRequest("tags", "all", 2, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "多个搜索关键词请用#分开");
        //int state = Utils.ParseInt(Utils.GetRequest("state", "all", 2, @"^[0-2]$", "书本状态选择错误"));
        int types = Utils.ParseInt(Utils.GetRequest("types", "all", 2, @"^[0-1]$", "书本性质选择错误"));
        int aid = meid;// Utils.ParseInt(Utils.GetRequest("aid", "all", 2, @"^[0-9]\d*$", "上传会员ID错误"));
        //------------------------------------附件接收开始------------------------------------
        int uid = meid;
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
        int state = Utils.ParseInt(ub.GetSub("BookIsShuMu", xmlPath));

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
        model.isgood = 0;
        new Book.BLL.ShuMu().Add(model);
        Utils.Success("添加书本", "添加书本成功，正在返回..", ReplaceWap(Utils.getUrl("bookman.aspx?act=list&amp;id=" + id + "")), "1");

    }

    private void EditShuMuPage(int meid)
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
        if (m.aid != meid)
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "编辑书本";

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("bookman.aspx?act=list&amp;id=" + id + "") + "\">" + model.Name + "</a>&gt;");
        builder.Append("编辑书本");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "书本名称:/,书本简介:/,书本封面:/,书本作者:/,书本字数(未知请填0):/,搜索关键词(多个用#分开):/,书本状态:/,书本性质:/,上传会员ID(没有请填0):/,添加时间:/,是否完结:/,,,,";
        string strName = "title,summary,img,author,length,tags,state,types,aid,addtime,isover,id,nid,act,backurl";
        string strType = "text,textarea,text,text,num,text,hidden,select,hidden,hidden,select,hidden,hidden,hidden,hidden";
        string strValu = "" + m.title + "'" + m.summary + "'" + m.img + "'" + m.author + "'" + m.length + "'" + m.tags + "'" + m.state + "'" + m.types + "'" + m.aid + "'" + DT.FormatDate(m.addtime, 0) + "'"+m.isover+"'" + id + "'" + nid + "'editshumusave'" + Utils.getPage(0) + "";
        string strEmpt = "true,true,true,true,true,true,0|未审核|1|已审核,0|原创|1|转载,false,false,0|连载中|1|已完结,false,false,false,false";
        string strIdea = "/";
        string strOthe = "修改书本,bookman.aspx,post,2,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("bookman.aspx?act=delshumu&amp;id=" + id + "&amp;nid=" + nid + "") + "\">删除书本</a><br />");
        builder.Append("<a href=\"" + Utils.getPage("bookman.aspx?act=list&amp;id=" + id + "") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回书城中心</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
   

    private void EditShuMuSavePage(int meid)
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
        int aid = meid;// Utils.ParseInt(Utils.GetRequest("aid", "all", 2, @"^[0-9]\d*$", "上传会员ID错误"));
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
        if (m.aid != meid)
        {
            Utils.Error("不存在的记录", "");
        }
        //int state = Utils.ParseInt(ub.GetSub("BookIsShuMu2", xmlPath));

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
        Utils.Success("编辑书本", "修改书本成功，正在返回..", Utils.getPage("bookman.aspx?act=list&amp;id=" + id + ""), "1");
    }

    private void DelShuMuPage(int meid)
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
        if (m.aid != meid)
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
            builder.Append("<a href=\"" + Utils.getUrl("bookman.aspx?info=ok&amp;act=delshumu&amp;id=" + id + "&amp;nid=" + nid + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("bookman.aspx?act=editshumu&amp;id=" + id + "&amp;nid=" + nid + "") + "\">先留着吧..</a>");
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
            Utils.Success("删除书本", "删除书本成功..", Utils.getUrl("bookman.aspx?act=list&amp;id=" + id + ""), "1");
        }
    }

    private void ViewPage(int meid)
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
        if (m.aid != meid)
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "查看书本";

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("bookman.aspx?act=list&amp;id=" + id + "") + "\">" + model.Name + "</a>&gt;");
        builder.Append("查看书本");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append(m.title);
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title2\">", Out.Hr()));
        builder.Append("添加");
        if (jid == 0)
        {
            builder.Append("<a href=\"" + Utils.getUrl("bookman.aspx?act=addjuan&amp;id=" + id + "&amp;nid=" + nid + "") + "\">分卷</a>|");
        }
        builder.Append("<a href=\"" + Utils.getUrl("bookman.aspx?act=addcontent&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + jid + "") + "\">章节</a>");
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
                    builder.AppendFormat("[{0}][分卷]<a href=\"" + Utils.getUrl("bookman.aspx?act=view&amp;id={1}&amp;nid={2}&amp;jid={3}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{4}</a>", n.pid, id, nid, n.id, n.title);
                    builder.Append("[<a href=\"" + Utils.getUrl("bookman.aspx?act=editjuan&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + n.id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">改</a>]");
                    builder.Append("[<a href=\"" + Utils.getUrl("bookman.aspx?act=deljuan&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + n.id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">删</a>]");
                }
                else
                {
                    builder.AppendFormat("[{0}][章节]<a href=\"" + Utils.getUrl("bookman.aspx?act=editcontent&amp;id={1}&amp;nid={2}&amp;jid={3}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{4}</a>", n.pid, id, nid, n.id, n.title);
                    builder.Append("(" + n.contents.Length + "字)");
                    if (n.state == 0)
                    {
                        builder.Append("[未审核]");
                    }
                    builder.Append("[<a href=\"" + Utils.getUrl("bookman.aspx?act=editcontent&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + n.id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">改</a>]");
                    builder.Append("[<a href=\"" + Utils.getUrl("bookman.aspx?act=delcontent&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + n.id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">删</a>]");
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
            builder.Append("<a href=\"" + Utils.getUrl("bookman.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + "") + "\">书卷目录</a>");
            builder.Append(Out.Tab("</div>", ""));
        }

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("bookman.aspx?act=list&amp;id=" + id + "") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回书城中心</a>");
        builder.Append(Out.Tab("</div>", ""));
    }


    private void AddJuanPage(int meid)
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
        if (m.aid != meid)
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "添加书本分卷";

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("bookman.aspx?act=list&amp;id=" + id + "") + "\">" + model.Name + "</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bookman.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + "") + "\">书本</a>&gt;");
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
        string strValu = "'" + pid + "'" + id + "'" + nid + "'addjuansave";
        string strEmpt = "true,false,false,false,false";
        string strIdea = "/";
        string strOthe = "添加分卷,bookman.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("bookman.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + "") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回书城中心</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void AddJuanSavePage(int meid)
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
        if (m.aid != meid)
        {
            Utils.Error("不存在的记录", "");
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
        Utils.Success("添加分卷", "添加分卷成功，正在返回..", Utils.getUrl("bookman.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + ""), "1");
    
    }

    private void EditJuanPage(int meid)
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
        if (m.aid != meid)
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
        builder.Append("<a href=\"" + Utils.getUrl("bookman.aspx?act=list&amp;id=" + id + "") + "\">" + model.Name + "</a>&gt;");
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
        string strOthe = "修改分卷,bookman.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("bookman.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + "") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回书城中心</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void EditJuanSavePage(int meid)
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
        if (m.aid != meid)
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
        if (n.aid != meid)
        {
            Utils.Error("不存在的记录", "");
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
        Utils.Success("编辑分卷", "修改分卷成功，正在返回..", Utils.getUrl("bookman.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + ""), "1");

    }

    private void DelJuanPage(int meid)
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
        if (model.aid != meid)
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


    private void AddContentPage(int meid)
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
        if (m.aid != meid)
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "添加书本章节";

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("bookman.aspx?act=list&amp;id=" + id + "") + "\">" + model.Name + "</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bookman.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + "") + "\">书本</a>&gt;");
        builder.Append("添加章节");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append(m.title);
        builder.Append(Out.Tab("</div>", ""));
        //得到排序
        int pid = new Book.BLL.Contents().GetCount(nid, jid, 0);
        pid = (pid + 1) * 10;

        string strText = "章节名称:/,章节内容:/,搜索关键词(多个用#分开):/,排序(越大越向上|默认自动):/,,,,";
        string strName = "title,contents,tags,pid,id,nid,jid,act";
        string strType = "text,big,hidden,num,hidden,hidden,hidden,hidden";
        string strValu = "'''"+pid+"'" + id + "'" + nid + "'" + jid + "'addcontentsave";
        string strEmpt = "true,true,true,false,false,false,false";
        string strIdea = "/";
        string strOthe = "添加章节,bookman.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("bookman.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + jid + "") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回书城中心</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void AddContentSavePage(int meid)
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
        if (m.aid != meid)
        {
            Utils.Error("不存在的记录", "");
        }
        string title = Utils.GetRequest("title", "all", 2, @"^[\s\S]{1,50}$", "章节名称限1-50字");
        string contents = Utils.GetRequest("contents", "all", 2, @"^[\s\S]{1,}$", "章节内容不能为空");
        string tags = Utils.GetRequest("tags", "all", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "多个搜索关键词请用#分开，不能为空");
        int pid = Utils.ParseInt(Utils.GetRequest("pid", "all", 2, @"^[0-9]\d*$", "排序错误"));

        int state = Utils.ParseInt(ub.GetSub("BookIsContents", xmlPath));
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
        Utils.Success("添加章节", "添加章节成功，正在返回..", Utils.getUrl("bookman.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + jid + ""), "1");

    }

    private void EditContentPage(int meid)
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
        if (m.aid != meid)
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
        if (n.aid != meid)
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "修改书本章节";

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("bookman.aspx?act=list&amp;id=" + id + "") + "\">" + model.Name + "</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bookman.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + "") + "\">书本</a>&gt;");
        builder.Append("编辑章节");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append(m.title);
        builder.Append(Out.Tab("</div>", ""));

        string strText = "章节名称:/,章节内容:/,搜索关键词(多个用#分开):/,添加时间:/,排序(越大越向上|默认自动):/,,,,,";
        string strName = "title,contents,tags,addtime,pid,id,nid,jid,act,backurl";
        string strType = "text,big,hidden,hidden,num,hidden,hidden,hidden,hidden,hidden";
        string strValu = "" + n.title + "'" + n.contents + "'" + n.tags + "'" + DT.FormatDate(n.addtime,0) + "'"+n.pid+"'" + id + "'" + nid + "'" + jid + "'editcontentsave'" + Utils.getPage(0) + "";
        string strEmpt = "true,true,true,false,false,false,false,false,false,false";
        string strIdea = "/";
        string strOthe = "修改章节,bookman.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("bookman.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + jid + "") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回书城中心</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void EditContentSavePage(int meid)
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
        if (m.aid != meid)
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
        if (n.aid != meid)
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
        Utils.Success("编辑章节", "修改章节成功，正在返回..", Utils.getPage("bookman.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + n.jid + ""), "1");

    }

    private void DelContentPage(int meid)
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
        if (model.aid != meid)
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
            builder.Append("<a href=\"" + Utils.getUrl("bookman.aspx?info=ok&amp;act=delcontent&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + jid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("bookman.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + model.jid + "") + "\">先留着吧..</a>");
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
            Utils.Success("删除章节", "删除章节成功..", Utils.getPage("bookman.aspx?act=view&amp;id=" + id + "&amp;nid=" + nid + "&amp;jid=" + model.jid + ""), "1");
        }
    }

    private static string ReplaceWap(string p_strUrl)
    {
        p_strUrl = p_strUrl.Replace("20a", "1a");

        return p_strUrl;
    }
}
