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

public partial class Manage_albums : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "del":
                DelPage();
                break;
            case "edit":
                EditPage();
                break;
            case "editsave":
                EditSavePage();
                break;
            case "verifypage":
                VerifypagePage();
                break;
            case "delpage":
                DelpagePage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        Master.Title = "相册管理";
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("相册管理");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (uid > 0)
            builder.Append("ID:" + uid + "");

        if (ptype == 1)
            builder.Append("图片|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?ptype=1&amp;uid=" + uid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">图片</a>|");

        if (ptype == 2)
            builder.Append("其它文件|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?ptype=2&amp;uid=" + uid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">其它文件</a>|");

        if (ptype == 3)
            builder.Append("未审核");
        else
            builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?ptype=3&amp;uid=" + uid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">未审核</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = 5;
        if (ptype > 1)
            pageSize = Convert.ToInt32(ub.Get("SiteListNo"));

        string strWhere = "";
        string[] pageValUrl = { "act", "uid", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;


        if (ptype == 1)
            strWhere = "Types=1";
        else if (ptype == 2)
            strWhere = "Types>1";
        else if (ptype == 3)
            strWhere = "IsVerify=1";

        if (uid > 0)
        {
            if (strWhere != "")
                strWhere += " and ";

            strWhere += "UsID=" + uid + "";
        }

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

                if (n.Types == 1)
                {
                    builder.Append("<img src=\"" + n.PrevFiles + "\" alt=\"load\"/><br />");
                }
                string Content = n.Content;
                if (string.IsNullOrEmpty(Content))
                    Content = "无标题";

                builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=edit&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[管理]&gt;</a>" + ((pageIndex - 1) * pageSize + k) + ".");
                builder.Append("<a href=\"" + Utils.getUrl(n.Files) + "\">" + Content + "(" + n.FileExt + "/" + BCW.Files.FileTool.GetContentLength(n.FileSize) + ")</a>");

                builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=del&amp;leibie=" + n.Types + "&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[删除]</a>");
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
        string strText = "输入用户ID:/,,";
        string strName = "uid,ptype,backurl";
        string strType = "num,hidden,hidden";
        string strValu = "'" + ptype + "'" + Utils.getPage(0) + "";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "搜文件,albums.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        if (ptype == 3)
            builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=verifypage&amp;ptype=" + ptype + "&amp;page=" + pageIndex + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">审核本页文件</a><br />");

        builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=delpage&amp;ptype=" + ptype + "&amp;page=" + pageIndex + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">删除本页文件</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));

    }


    private void DelPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        if (info != "ok")
        {
            Master.Title = "删除文件";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("不能恢复！确定删除此文件吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?info=ok&amp;act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("albums.aspx") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("albums.aspx") + "\">返回上一级</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            BCW.Model.Upfile model = new BCW.BLL.Upfile().GetUpfile(id);
            if (model == null)
            {
                Utils.Error("不存在的记录", "");
            }
            //删除
            new BCW.BLL.Upfile().Delete(id);
            new BCW.BLL.FComment().Delete(model.Types, id);
            //删除文件
            BCW.Files.FileTool.DeleteFile(model.Files);
            if (!string.IsNullOrEmpty(model.PrevFiles))
                BCW.Files.FileTool.DeleteFile(model.PrevFiles);

            //关联帖子回帖减去文件数
            if (model.ReID > 0)
            {
                new BCW.BLL.Reply().UpdateFileNum(model.ReID, -1);
            }
            else if (model.BID > 0)
            {
                new BCW.BLL.Text().UpdateFileNum(model.BID, -1);
                int FileNum = new BCW.BLL.Text().GetFileNum(model.BID);
                if (FileNum == 0)
                {
                    //去掉附件帖标识
                    new BCW.BLL.Text().UpdateTypes(model.BID, 0);
                }
            }

            Utils.Success("删除文件", "删除文件成功..", Utils.getPage("albums.aspx"), "1");
        }
    }

    private void EditPage()
    {
        Master.Title = "编辑文件";
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "文件ID错误"));
        BCW.Model.Upfile model = new BCW.BLL.Upfile().GetUpfile(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("编辑文件");
        builder.Append(Out.Tab("</div>", ""));
        string strText = "文件地址:/,缩略图地址:/,文件描述:/,用户ID:/,审核状态:/,添加时间:/,,";
        string strName = "Files,PrevFiles,Content,UsID,IsVerify,AddTime,id,act";
        string strType = "text,text,text,num,select,date,hidden,hidden";
        string strValu = "" + model.Files + "'" + model.PrevFiles + "'" + model.Content + "'" + model.UsID + "'" + model.IsVerify + "'" + DT.FormatDate(model.AddTime, 0) + "'" + id + "'editsave";
        string strEmpt = "false,true,true,false,0|已审核|1|未审核,false,false,false";
        string strIdea = "/";
        string strOthe = "确定编辑,albums.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(" <a href=\"" + Utils.getPage("albums.aspx") + "\">取消</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("albums.aspx") + "\">相册管理</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void EditSavePage()
    {
        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[1-9]\d*$", "相册ID错误"));
        string Files = Utils.GetRequest("Files", "post", 2, @"^[^\^]{1,100}$", "缩略图地址限100字内,可留空");
        string PrevFiles = Utils.GetRequest("PrevFiles", "post", 3, @"^[^\^]{1,100}$", "文件地址限100字内");
        string Content = Utils.GetRequest("Content", "post", 3, @"^[^\^]{1,30}$", "请输入不超30字的描述");
        int UsID = int.Parse(Utils.GetRequest("UsID", "post", 2, @"^[1-9]\d*$", "用户ID错误"));
        int IsVerify = int.Parse(Utils.GetRequest("IsVerify", "post", 2, @"^[0-1]$", "审核状态选择错误"));
        DateTime AddTime = Utils.ParseTime(Utils.GetRequest("AddTime", "post", 2, DT.RegexTime, "添加时间填写出错"));
        BCW.Model.Upfile model = new BCW.Model.Upfile();
        model.ID = id;
        model.Files = Files;
        model.PrevFiles = PrevFiles;
        model.Content = Content;
        model.UsID = UsID;
        model.IsVerify = IsVerify;
        model.AddTime = AddTime;
        new BCW.BLL.Upfile().Update(model);

        Utils.Success("编辑文件", "编辑文件成功..", Utils.getUrl("albums.aspx?act=edit&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
    }

    private void VerifypagePage()
    {
 
        Master.Title = "审核本页文件";
        int page = int.Parse(Utils.GetRequest("page", "get", 1, @"^[1-9]\d*$", "页面ID错误"));
        string info = Utils.GetRequest("info", "get", 1, "", "");
        if (info != "ok")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定审核本页文件吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=verifypage&amp;info=ok&amp;page=" + page + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定审核</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("albums.aspx?ptype=3") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("albums.aspx?act=verify") + "\">返回上一级</a>");
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

            strWhere = "IsVerify=1";

            // 开始读取列表
            IList<BCW.Model.Upfile> listUpfile = new BCW.BLL.Upfile().GetUpfiles(pageIndex, pageSize, strWhere, out recordCount);
            if (listUpfile.Count > 0)
            {
                foreach (BCW.Model.Upfile n in listUpfile)
                {
                    new BCW.BLL.Upfile().UpdateIsVerify(n.ID, 0);
                }
            }

            Utils.Success("审核本页文件", "审核本页文件成功，正在返回..", Utils.getUrl("albums.aspx?ptype=3"), "1");
        }
    }

    private void DelpagePage()
    {

        Master.Title = "删除本页文件";
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 2, @"^[1-3]\d*$", "类型错误"));
        int page = int.Parse(Utils.GetRequest("page", "get", 1, @"^[1-9]\d*$", "页面ID错误"));
        string info = Utils.GetRequest("info", "get", 1, "", "");
        if (info != "ok")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除本页文件吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?act=delpage&amp;info=ok&amp;ptype=" + ptype + "&amp;page=" + page + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("albums.aspx?ptype=" + ptype + "") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("albums.aspx?ptype=" + ptype + "") + "\">返回上一级</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            int pageIndex;
            int recordCount;
            int pageSize = 5;

            string strWhere = "";
            pageIndex = page;
            if (pageIndex == 0)
                pageIndex = 1;

            if (ptype == 1)
                strWhere = "Types=1";
            else
                strWhere = "Types>1";

            // 开始读取列表
            IList<BCW.Model.Upfile> listUpfile = new BCW.BLL.Upfile().GetUpfiles(pageIndex, pageSize, strWhere, out recordCount);
            if (listUpfile.Count > 0)
            {
                foreach (BCW.Model.Upfile n in listUpfile)
                {
                    int id = n.ID;
                    BCW.Model.Upfile model = new BCW.BLL.Upfile().GetUpfile(id);
                    if (model != null)
                    {
                        //删除
                        new BCW.BLL.Upfile().Delete(id);
                        new BCW.BLL.FComment().Delete(model.Types, id);
                        //删除文件
                        BCW.Files.FileTool.DeleteFile(model.Files);
                        if (!string.IsNullOrEmpty(model.PrevFiles))
                            BCW.Files.FileTool.DeleteFile(model.PrevFiles);

                        //关联帖子回帖减去文件数
                        if (model.ReID > 0)
                        {
                            new BCW.BLL.Reply().UpdateFileNum(model.ReID, -1);
                        }
                        else if (model.BID > 0)
                        {
                            new BCW.BLL.Text().UpdateFileNum(model.BID, -1);
                            int FileNum = new BCW.BLL.Text().GetFileNum(model.BID);
                            if (FileNum == 0)
                            {
                                //去掉附件帖标识
                                new BCW.BLL.Text().UpdateTypes(model.BID, 0);
                            }
                        }
                    }
                }
            }

            Utils.Success("删除本页文件", "删除本页文件成功，正在返回..", Utils.getUrl("albums.aspx"), "1");
        }
    }

}
