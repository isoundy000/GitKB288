using System;
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
using BCW.Files;
/// <summary>
/// 陈志基 16/5/18
/// 修改SaveFiles（）文件上传顺序
/// </summary>
public partial class bbs_Addfile : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/upfile.xml";
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

        switch (act)
        {
            case "upload":
                UploadPage();
                break;
            case "collec":
                CollecPage();
                break;
            case "collecload":
                CollecloadPage();
                break;
            case "photo":
                PhotoPage();
                break;
            case "phupload":
                PhUploadPage();
                break;
            default:
                UpfilePage();
                break;
        }
    }

    private void UpfilePage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Master.Title = "WAP2.0上传";
        int leibie = int.Parse(Utils.GetRequest("leibie", "all", 1, @"^[1-4]\d*$", "1"));
        if (!Utils.Isie())
        {
            string VE = ConfigHelper.GetConfigString("VE");
            string SID = ConfigHelper.GetConfigString("SID");
            builder.Append("<a href=\"addfile.aspx?leibie=" + leibie + "&amp;backurl=" + Utils.getPage(0) + "&amp;" + VE + "=20a&amp;" + SID + "=" + Utils.getstrU() + "\">[切换2.0上传]</a>");
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("WAP2.0上传" + BCW.User.AppCase.CaseAlbums(leibie).Replace("册", "片") + "");
            builder.Append(Out.Tab("</div>", ""));
            int max = Convert.ToInt32(ub.GetSub("UpaMaxFileNum", xmlPath));
            string maxfile = ub.GetSub("UpaMaxFileSize", xmlPath);
            string fileExt = ub.GetSub("UpaFileExt", xmlPath);
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("上传允许格式:" + fileExt + "<br />");
            builder.Append("每个文件限" + maxfile + "K");
            builder.Append(Out.Tab("</div>", "<br />"));

            int num = int.Parse(Utils.GetRequest("num", "get", 1, @"^[1-9]\d*$", "1"));
            if (num > max)
                num = max;

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("上传:");
            for (int i = 1; i <= max; i++)
            {
                builder.Append("<a href=\"" + Utils.getUrl("addfile.aspx?leibie=" + leibie + "&amp;num=" + i + "&amp;backurl=" + Utils.getPage(0) + "") + "\"><b>" + i + "</b></a> ");
            }
            builder.Append("个");

            builder.Append(Out.Tab("</div>", ""));
            string sUpType = string.Empty;
            string sText = string.Empty;
            string sName = string.Empty;
            string sType = string.Empty;
            string sValu = string.Empty;
            string sEmpt = string.Empty;
            for (int i = 0; i < num; i++)
            {
                string y = ",";
                if (num == 1)
                {
                    strText = strText + y + "选择" + sUpType + "附件:/," + sUpType + "附件描述(30字内):/";
                }
                else
                {
                    strText = strText + y + "" + sUpType + "第" + (i + 1) + "个附件:/," + sUpType + "附件描述" + (i + 1) + ":/";
                }
                strName = strName + y + "file" + (i + 1) + y + "stext" + (i + 1);
                strType = strType + y + "file" + y + "text";
                strValu = strValu + "''";
                strEmpt = strEmpt + y + y;
            }

            string strUpgroup = string.Empty;
            DataSet ds = new BCW.BLL.Upgroup().GetList("ID,Title", "Leibie=" + leibie + " and UsID=" + meid + " Order BY Paixu ASC");
            if (ds != null && ds.Tables[0].Rows.Count != 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    strUpgroup += "|" + ds.Tables[0].Rows[i]["ID"] + "|" + ds.Tables[0].Rows[i]["Title"] + "";
                }
            }
            strUpgroup = "0|默认分组" + strUpgroup;

            strText = sText + Utils.Mid(strText, 1, strText.Length) + "," + BCW.User.AppCase.CaseAlbums(leibie).Replace("册", "片") + "分类:,,";
            strName = sName + Utils.Mid(strName, 1, strName.Length) + ",NodeId,leibie,act";
            strType = sType + Utils.Mid(strType, 1, strType.Length) + ",select,hidden,hidden";
            strValu = sValu + Utils.Mid(strValu, 1, strValu.Length) + "'0'" + leibie + "'upload";
            strEmpt = sEmpt + Utils.Mid(strEmpt, 1, strEmpt.Length) + "," + strUpgroup + ",,,"; ;
            strIdea = "/";
            strOthe = "我要上传|reset,addfile.aspx,post,2,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + ReplaceWap(Utils.getUrl("/default.aspx")) + "\">首页</a>-");
        builder.Append("<a href=\"" + ReplaceWap(Utils.getPage("albums.aspx?uid=" + meid + "leibie=" + leibie + "")) + "\">上级</a>-");
        builder.Append("<a href=\"" + ReplaceWap(Utils.getUrl("albums.aspx")) + "\">相册</a>");
        builder.Append(Out.Tab("</div>", ""));

    }

    private void UploadPage()
    {
        int leibie = int.Parse(Utils.GetRequest("leibie", "all", 1, @"^[1-4]\d*$", "1"));
        int NodeId = int.Parse(Utils.GetRequest("NodeId", "all", 1, @"^[0-9]\d*$", "0"));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        BCW.User.Users.ShowVerifyRole("f", meid);//非验证会员提示
        new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Upfile, meid);//会员上传权限
        //上传文件
        int kk = 0;
        SaveFiles(meid, leibie, NodeId, out kk);
        string strOut = string.Empty;
        if (kk < 0)
        {
            if (kk == -999)
                kk = 0;

            strOut = "部分文件超出今天上传数量导致上传失败.";
            kk = Math.Abs(kk);
        }
        //动态记录
        new BCW.BLL.Action().Add(meid, "在相册上传了[URL=/bbs/albums.aspx?uid=" + meid + "]新的文件[/URL]");
        Utils.Success("上传文件", "上传" + kk + "个文件成功！" + strOut + "<br /><a href=\"" + ReplaceWap(Utils.getUrl("addfile.aspx?act=upfile&amp;leibie=" + leibie + "")) + "\">&gt;继续上传</a>", ReplaceWap(Utils.getUrl("albums.aspx?uid=" + meid + "&amp;leibie=" + leibie + "")), "2");

    }

    private void CollecPage()
    {
        Master.Title = "输入地址上传";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        int leibie = int.Parse(Utils.GetRequest("leibie", "all", 1, @"^[1-4]\d*$", "1"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("输入地址上传" + BCW.User.AppCase.CaseAlbums(leibie).Replace("册", "片") + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        int max = Convert.ToInt32(ub.GetSub("UpaMaxFileNum", xmlPath));
        string maxfile = ub.GetSub("UpaMaxFileSize", xmlPath);
        string fileExt = ub.GetSub("UpaFileExt", xmlPath);
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("只允许格式:.gif,.jpg,.jpeg,.png,.bmp<br />");
        builder.Append("每个文件限" + maxfile + "K");
        builder.Append(Out.Tab("</div>", ""));
        string strUpgroup = string.Empty;
        DataSet ds = new BCW.BLL.Upgroup().GetList("ID,Title", "Leibie=" + leibie + " and UsID=" + meid + " Order BY Paixu ASC");
        if (ds != null && ds.Tables[0].Rows.Count != 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                strUpgroup += "|" + ds.Tables[0].Rows[i]["ID"] + "|" + ds.Tables[0].Rows[i]["Title"] + "";
            }
        }
        strUpgroup = "0|默认分组" + strUpgroup;

        strText = "文件地址:/,附件描述(30字内):/," + BCW.User.AppCase.CaseAlbums(leibie).Replace("册", "片") + "分类:/,,";
        strName = "FileName,Content,NodeId,leibie,act";
        strType = "text,text,select,hidden,hidden";
        strValu = "http://''0'"+leibie+"'collecload";
        strEmpt = "false,true," + strUpgroup + ",false,false";
        strIdea = "/";
        strOthe = "我要上传|reset,addfile.aspx,post,2,red|blue|blue";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + ReplaceWap(Utils.getUrl("/default.aspx")) + "\">首页</a>-");
        builder.Append("<a href=\"" + ReplaceWap(Utils.getPage("albums.aspx?uid=" + meid + "leibie=" + leibie + "")) + "\">上级</a>-");
        builder.Append("<a href=\"" + ReplaceWap(Utils.getUrl("albums.aspx")) + "\">相册</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void CollecloadPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int NodeId = int.Parse(Utils.GetRequest("NodeId", "all", 1, @"^[0-9]\d*$", "0"));
        BCW.User.Users.ShowVerifyRole("f", meid);//非验证会员提示
        new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Upfile, meid);//会员上传权限
        int leibie = int.Parse(Utils.GetRequest("leibie", "post", 1, @"^[1-4]\d*$", "1"));
        string FileName = Utils.GetRequest("FileName", "post", 2, @"^.+?.(gif|jpg|bmp|jpeg|png)$", "请正确输入图片地址");
        //允许上传数量
        int maxAddNum = Convert.ToInt32(ub.GetSub("UpAddNum", xmlPath));
        if (maxAddNum > 0)
        {
            //计算今天上传数量
            int AddNum = new BCW.BLL.Upfile().GetTodayCount(meid);
            if (maxAddNum <= AddNum)
            {
                Utils.Error("今天上传数量已达" + maxAddNum + "个，不能再上传了", "");
            }
        }
        string DirPath = string.Empty;
        string prevDirPath = string.Empty;
        string Path = "/Files/bbs/" + meid + "/act/";
        string prevPath = "/Files/bbs/" + meid + "/prev/";
        if (FileTool.CreateDirectory(Path, out DirPath))
        {                        
            string sPath = BCW.Files.FileTool.DownloadFile(Path, 0, FileName);
            if (sPath != FileName)
            {
                //缩略图生成
                string fileExtension = BCW.Files.FileTool.GetFileExt(sPath).ToLower();
                string SavePath = System.Web.HttpContext.Current.Request.MapPath(sPath);
                //=============================图片木马检测,包括TXT===========================
                string vSavePath = SavePath;

                bool IsPass = true;
                System.IO.StreamReader sr = new System.IO.StreamReader(vSavePath, System.Text.Encoding.Default);
                string strContent = sr.ReadToEnd().ToLower();
                sr.Close();
                string str = "system.|request|javascript|script |script>|.getfolder|.createfolder|.deletefolder|.createdirectory|.deletedirectory|.saveas|wscript.shell|script.encode|server.|.createobject|execute|activexobject|language=";
                foreach (string s in str.Split('|'))
                {
                    if (strContent.IndexOf(s) != -1)
                    {
                        System.IO.File.Delete(vSavePath);
                        IsPass = false;
                        break;
                    }
                }
                if (IsPass == false)
                    Utils.Error("非法图片..", "");

                //=============================图片木马检测,包括TXT===========================
                string prevSavePath = string.Empty;
                int ThumbType = Convert.ToInt32(ub.GetSub("UpaThumbType", xmlPath));
                int width = Convert.ToInt32(ub.GetSub("UpaWidth", xmlPath));
                int height = Convert.ToInt32(ub.GetSub("UpaHeight", xmlPath));
                if (ThumbType > 0)
                {
                    try
                    {
                        bool pbool = false;
                        if (ThumbType == 1)
                            pbool = true;
                        if (FileTool.CreateDirectory(prevPath, out prevDirPath))
                        {
                            prevSavePath = SavePath.Replace("act", "prev");
                            int IsThumb = 0;
                            if (fileExtension == ".gif")
                            {
                                if (ThumbType > 0)
                                    new BCW.Graph.GifHelper().GetThumbnail(SavePath, prevSavePath, width, height, pbool);

                                IsThumb = Convert.ToInt32(ub.GetSub("UpaIsThumb", xmlPath));
                                if (IsThumb > 0)
                                {
                                    if (IsThumb == 1)
                                        new BCW.Graph.GifHelper().SmartWaterMark(prevSavePath, "", ub.GetSub("UpaWord", xmlPath), ub.GetSub("UpaWordColor", xmlPath), "Arial", 12, Convert.ToInt32(ub.GetSub("UpaPosition", xmlPath)));//文字水印
                                    else
                                        new BCW.Graph.GifHelper().WaterMark(prevSavePath, "", ub.GetSub("UpaWord", xmlPath), Convert.ToInt32(ub.GetSub("UpaPosition", xmlPath)), Convert.ToInt32(ub.GetSub("UpaPosition", xmlPath)));//图片水印
                                }
                            }
                            else
                            {
                                if (ThumbType > 0)
                                    new BCW.Graph.ImageHelper().ResizeImage(SavePath, prevSavePath, width, height, pbool);
                                IsThumb = Convert.ToInt32(ub.GetSub("UpaIsThumb", xmlPath));
                                if (IsThumb > 0)
                                {
                                    if (IsThumb == 1)
                                        new BCW.Graph.ImageHelper().WaterMark(prevSavePath, "", ub.GetSub("UpaWord", xmlPath), ub.GetSub("UpaWordColor", xmlPath), "Arial", 12, Convert.ToInt32(ub.GetSub("UpaPosition", xmlPath)));//文字水印
                                    else
                                        new BCW.Graph.ImageHelper().WaterMark(prevSavePath, "", ub.GetSub("UpaWord", xmlPath), Convert.ToInt32(ub.GetSub("UpaPosition", xmlPath)), Convert.ToInt32(ub.GetSub("UpaTran", xmlPath)));//图片水印
                                }
                            }
                        }
                    }
                    catch { }
                }
                string Content = Utils.GetRequest("Content", "post", 1, "", "");
                if (!string.IsNullOrEmpty(Content))
                    Content = Utils.Left(Content, 30);
                else
                    Content = "";
                
                BCW.Model.Upfile model = new BCW.Model.Upfile();
                model.Types = 1; // FileTool.GetExtType(fileExtension);
                model.NodeId = NodeId;
                model.UsID = meid;
                model.ForumID = 0;
                model.BID = 0;
                model.ReID = 0;
                model.Files = sPath;
                if (string.IsNullOrEmpty(prevDirPath))
                    model.PrevFiles = "";
                else
                    model.PrevFiles = sPath.Replace("act", "prev");

                model.Content = Content;
                model.FileSize = BCW.Files.FileTool.GetFileLength(sPath);
                model.FileExt = fileExtension;
                model.DownNum = 0;
                model.Cent = 0;
                //审核要求指示
                int Verify = Utils.ParseInt(ub.GetSub("UpIsVerify", xmlPath));
                if (Verify > 0)
                    model.IsVerify = 1;

                model.AddTime = DateTime.Now;
                new BCW.BLL.Upfile().Add(model);
                //动态记录
                new BCW.BLL.Action().Add(meid, "在相册上传了[URL=/bbs/albums.aspx?uid=" + meid + "]新的文件[/URL]");
                Utils.Success("地址上传文件", "上传1个文件成功！<br /><a href=\"" + ReplaceWap(Utils.getUrl("addfile.aspx?act=collec&amp;leibie=" + leibie + "")) + "\">&gt;继续上传</a>", ReplaceWap(Utils.getUrl("albums.aspx?uid=" + meid + "&amp;leibie=" + leibie + "")), "2");

            }
            else
            {
                Utils.Error("上传失败，请检查文件是否存在", "");
            }
        }
    }

    private void PhotoPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Master.Title = "WAP2.0上传";
        if (!Utils.Isie())
        {
            string VE = ConfigHelper.GetConfigString("VE");
            string SID = ConfigHelper.GetConfigString("SID");
            builder.Append("<a href=\"addfile.aspx?act=photo&amp;backurl=" + Utils.getPage(0) + "&amp;" + VE + "=20a&amp;" + SID + "=" + Utils.getstrU() + "\">[切换2.0上传]</a>");
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("WAP2.0上传头像");
            builder.Append(Out.Tab("</div>", ""));
            string maxfile = ub.GetSub("UpaPhFileSize", xmlPath);
            string fileExt = ".gif,.jpg,.jpeg";
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("上传允许格式:" + fileExt + "<br />");
            builder.Append("头像限" + maxfile + "K内.超过尺寸宽240*高320像素将自动缩小");
            builder.Append(Out.Tab("</div>", "<br />"));
            strText = "选择图片,";
            strName = "file1,act";
            strType = "file,hidden";
            strValu = "'phupload";
            strEmpt = "true,false"; ;
            strIdea = "/";
            strOthe = "我要上传|reset,addfile.aspx,post,2,red|blue|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + ReplaceWap(Utils.getUrl("/default.aspx")) + "\">首页</a>-");
        builder.Append("<a href=\"" + ReplaceWap(Utils.getPage("albums.aspx?uid=" + meid + "")) + "\">上级</a>-");
        builder.Append("<a href=\"" + ReplaceWap(Utils.getUrl("albums.aspx")) + "\">相册</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void PhUploadPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        BCW.User.Users.ShowVerifyRole("f", meid);//非验证会员提示
        new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Upfile, meid);//会员上传权限
        //遍历File表单元素
        System.Web.HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
        for (int iFile = 0; iFile < files.Count; iFile++)
        {
            //检查文件扩展名字
            System.Web.HttpPostedFile postedFile = files[iFile];
            string fileName, fileExtension;
            fileName = System.IO.Path.GetFileName(postedFile.FileName);
            string UpExt = ".gif,.jpg,.jpeg";
            int UpLength = Convert.ToInt32(ub.GetSub("UpaPhFileSize", xmlPath));
            if (fileName != "")
            {
                fileExtension = System.IO.Path.GetExtension(fileName).ToLower();
                //检查是否允许上传格式
                if (UpExt.IndexOf(fileExtension) == -1)
                {
                    Utils.Error("头像图片格式只允许" + UpExt + "..", "");
                }
                if (postedFile.ContentLength > Convert.ToInt32(UpLength * 1024))
                {
                    Utils.Error("头像大小限" + UpLength + "K内", "");
                }
                string DirPath = string.Empty;
                string prevDirPath = string.Empty;
                string Path = "/Files/bbs/" + meid + "/tx/";
                string prevPath = Path;
                if (FileTool.CreateDirectory(Path, out DirPath))
                {
                    //生成随机文件名
                    fileName = DT.getDateTimeNum() + iFile + fileExtension;
                    string SavePath = System.Web.HttpContext.Current.Request.MapPath(DirPath) + fileName;
                    postedFile.SaveAs(SavePath);
                    //=============================图片木马检测,包括TXT===========================
                    string vSavePath = SavePath;

                    bool IsPass = true;
                    System.IO.StreamReader sr = new System.IO.StreamReader(vSavePath, System.Text.Encoding.Default);
                    string strContent = sr.ReadToEnd().ToLower();
                    sr.Close();
                    string str = "system.|request|javascript|script |script>|.getfolder|.createfolder|.deletefolder|.createdirectory|.deletedirectory|.saveas|wscript.shell|script.encode|server.|.createobject|execute|activexobject|language=";
                    foreach (string s in str.Split('|'))
                    {
                        if (strContent.IndexOf(s) != -1)
                        {
                            System.IO.File.Delete(vSavePath);
                            IsPass = false;
                            break;
                        }
                    }
                    if (IsPass == false)
                        Utils.Error("非法图片..", "");

                    //=============================图片木马检测,包括TXT===========================
                    string wh = new BCW.Graph.ImageHelper().GetPicxywh(SavePath, 0);
                    wh = wh.Replace("像素", "");
                    string[] whTemp = wh.Split('*');
                    int w = Utils.ParseInt(whTemp[0]);
                    int h = Utils.ParseInt(whTemp[1]);
                    if (w < 15 || h < 15)
                    {
                        System.IO.File.Delete(SavePath);
                        Utils.Error("头像宽高尺寸都不能小于15像素", "");
                    }
                    if (w > 240 || h > 320)
                    {
                        //System.IO.File.Delete(SavePath);
                        //Utils.Error("头像尺寸限240*320像素内", "");

                        //------缩放图片(比例缩放)-----
                        int width = 240;
                        int height = 320;
                        try
                        {
                            bool pbool = true;
                            if (FileTool.CreateDirectory(prevPath, out prevDirPath))
                            {
                                string prevSavePath = System.Web.HttpContext.Current.Request.MapPath(prevDirPath) + fileName;

                                if (fileExtension == ".gif")
                                {
                                    new BCW.Graph.GifHelper().GetThumbnail(SavePath, prevSavePath, width, height, pbool);
                                }
                                else
                                {
                                    if (fileExtension == ".png" || fileExtension == ".jpg" || fileExtension == ".jpeg")
                                    {
                                        new BCW.Graph.ImageHelper().ResizeImage(SavePath, prevSavePath, width, height, pbool);
                                    }
                                }
                            }
                            DirPath = prevDirPath;
                        }
                        catch
                        {
                            //删除原图
                            BCW.Files.FileTool.DeleteFile(DirPath + fileName);
                            Utils.Error("头像上传失败，头像图片格式也许已损坏，可以重试一下", "");
                        }
                    }

                    //删除之前的自定义头像文件
                    string PhotoFile = new BCW.BLL.User().GetPhoto(meid);
                    if (PhotoFile != "" && PhotoFile.Contains("/tx/"))
                    {
                        BCW.Files.FileTool.DeleteFile(PhotoFile);
                    }
                    new BCW.BLL.User().UpdatePhoto(meid, DirPath + fileName);
                }
                else
                {
                    Utils.Error("上传出现错误..", "");
                }
            }
        }
        //动态记录
        new BCW.BLL.Action().Add(meid, "在空间设置了[URL=/bbs/uinfo.aspx?uid=" + meid + "]新的头像[/URL]");
        Utils.Success("设置头像", "设置头像成功！<br /><a href=\"" + ReplaceWap(Utils.getUrl("uinfo.aspx")) + "\">&gt;查看效果</a>", ReplaceWap(Utils.getPage("uinfo.aspx")), "2");
    }

    /// <summary>
    /// 上传文件
    /// </summary>
    private void SaveFiles(int meid, int leibie, int NodeId, out int kk)
    {        
        //允许上传数量
        int maxAddNum = Convert.ToInt32(ub.GetSub("UpAddNum", xmlPath));
        int AddNum = 0;
        if (maxAddNum > 0)
        {
            //计算今天上传数量
            AddNum = new BCW.BLL.Upfile().GetTodayCount(meid);
        }
        //遍历File表单元素
        System.Web.HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
        // int j = 1;
        int j = files.Count;
        int k = 0;
        try
        {
            string GetFiles = string.Empty;
            //for (int iFile = 0; iFile < files.Count; iFile++)
            for (int iFile = files.Count - 1; iFile > -1; iFile--)
            {
                //检查文件扩展名字
                System.Web.HttpPostedFile postedFile = files[iFile];
                string fileName, fileExtension;
                fileName = System.IO.Path.GetFileName(postedFile.FileName);
                string UpExt = ub.GetSub("UpaFileExt", xmlPath);
                int UpLength = Convert.ToInt32(ub.GetSub("UpaMaxFileSize", xmlPath));
                if (fileName != "")
                {
                    fileExtension = System.IO.Path.GetExtension(fileName).ToLower();
                    //检查是否允许上传格式
                    if (UpExt.IndexOf(fileExtension) == -1)
                    {
                        continue;
                    }
                    //非法上传
                    if (fileExtension == ".asp" || fileExtension == ".aspx" || fileExtension == ".jsp" || fileExtension == ".php" || fileExtension == ".asa" || fileExtension == ".cer" || fileExtension == ".cdx" || fileExtension == ".htr" || fileExtension == ".exe")
                    {
                        continue;
                    }
                    if (postedFile.ContentLength > Convert.ToInt32(UpLength * 1024))
                    {
                        continue;
                    }
                    string DirPath = string.Empty;
                    string prevDirPath = string.Empty;
                    string Path = "/Files/bbs/" + meid + "/act/";
                    string prevPath = "/Files/bbs/" + meid + "/prev/";
                    int IsVerify = 0;
                    if (FileTool.CreateDirectory(Path, out DirPath))
                    {                        
                        //上传数量限制
                        if (maxAddNum > 0)
                        {
                            if (maxAddNum <= (AddNum + k))
                            {
                                k = -k;
                                if (k == 0)
                                    k = -999;
                                break;
                            }
                        }
                        //生成随机文件名
                        fileName = DT.getDateTimeNum() + iFile + fileExtension;
                        string SavePath = System.Web.HttpContext.Current.Request.MapPath(DirPath) + fileName;
                        postedFile.SaveAs(SavePath);

                        //=============================图片木马检测,包括TXT===========================
                        string vSavePath = SavePath;
                        if (fileExtension == ".txt" || fileExtension == ".gif" || fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".png" || fileExtension == ".bmp")
                        {
                            bool IsPass = true;
                            System.IO.StreamReader sr = new System.IO.StreamReader(vSavePath, System.Text.Encoding.Default);
                            string strContent = sr.ReadToEnd().ToLower();
                            sr.Close();
                            string str = "system.|request|javascript|script |script>|.getfolder|.createfolder|.deletefolder|.createdirectory|.deletedirectory|.saveas|wscript.shell|script.encode|server.|.createobject|execute|activexobject|language=";
                            foreach (string s in str.Split('|'))
                            {
                                if (strContent.IndexOf(s) != -1)
                                {
                                    System.IO.File.Delete(vSavePath);
                                    IsPass = false;
                                    break;
                                }
                            }
                            if (IsPass == false)
                                continue;
                        }
                        //=============================图片木马检测,包括TXT===========================

                        //审核要求指示
                        int Verify = Utils.ParseInt(ub.GetSub("UpIsVerify", xmlPath));
                        //缩略图生成
                        if (fileExtension == ".gif" || fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".png" || fileExtension == ".bmp")
                        {
                            int ThumbType = Convert.ToInt32(ub.GetSub("UpaThumbType", xmlPath));
                            int width = Convert.ToInt32(ub.GetSub("UpaWidth", xmlPath));
                            int height = Convert.ToInt32(ub.GetSub("UpaHeight", xmlPath));
                            if (ThumbType > 0)
                            {
                                try
                                {
                                    bool pbool = false;
                                    if (ThumbType == 1)
                                        pbool = true;
                                    if (FileTool.CreateDirectory(prevPath, out prevDirPath))
                                    {
                                        string prevSavePath = System.Web.HttpContext.Current.Request.MapPath(prevDirPath) + fileName;
                                        int IsThumb = 0;
                                        if (fileExtension == ".gif")
                                        {
                                            if (ThumbType > 0)
                                                new BCW.Graph.GifHelper().GetThumbnail(SavePath, prevSavePath, width, height, pbool);

                                            IsThumb = Convert.ToInt32(ub.GetSub("UpaIsThumb", xmlPath));
                                            if (IsThumb > 0)
                                            {
                                                if (IsThumb == 1)
                                                    new BCW.Graph.GifHelper().SmartWaterMark(SavePath, "", ub.GetSub("UpaWord", xmlPath), ub.GetSub("UpaWordColor", xmlPath), "Arial", 12, Convert.ToInt32(ub.GetSub("UpaPosition", xmlPath)));//文字水印
                                                else
                                                    new BCW.Graph.GifHelper().WaterMark(SavePath, "", Server.MapPath(ub.GetSub("UpaWord", xmlPath)), Convert.ToInt32(ub.GetSub("UpaPosition", xmlPath)), Convert.ToInt32(ub.GetSub("UpaTran", xmlPath)));//图片水印
                                            }
                                        }
                                        else
                                        {
                                            if (fileExtension == ".png" || fileExtension == ".jpg" || fileExtension == ".jpeg")
                                            {
                                                if (ThumbType > 0)
                                                    new BCW.Graph.ImageHelper().ResizeImage(SavePath, prevSavePath, width, height, pbool);
                                                IsThumb = Convert.ToInt32(ub.GetSub("UpaIsThumb", xmlPath));
                                                if (IsThumb > 0)
                                                {
                                                    if (IsThumb == 1)
                                                        new BCW.Graph.ImageHelper().WaterMark(SavePath, "", ub.GetSub("UpaWord", xmlPath), ub.GetSub("UpaWordColor", xmlPath), "Arial", 12, Convert.ToInt32(ub.GetSub("UpaPosition", xmlPath)));//文字水印
                                                    else
                                                        new BCW.Graph.ImageHelper().WaterMark(SavePath, "", Server.MapPath(ub.GetSub("UpaWord", xmlPath)), Convert.ToInt32(ub.GetSub("UpaPosition", xmlPath)), Convert.ToInt32(ub.GetSub("UpaTran", xmlPath)));//图片水印
                                                }
                                            }
                                        }

                                    }
                                }
                                catch { }
                            }
                            //图片审核
                            if (Verify > 0)
                                IsVerify = 1;

                        }
                        else
                        {
                            //文件审核
                            if (Verify > 1)
                                IsVerify = 1;

                            //自动识别出非图片
                            if (leibie == 1)
                                leibie = FileTool.GetExtType(fileExtension);
                        }
                        string Content = Utils.GetRequest("stext" + j + "", "post", 1, "", "");
                        if (!string.IsNullOrEmpty(Content))
                            Content = Utils.Left(Content, 30);
                        else
                            Content = "";

                        BCW.Model.Upfile model = new BCW.Model.Upfile();
                        model.Types = leibie; // FileTool.GetExtType(fileExtension);
                        model.NodeId = NodeId;
                        model.UsID = meid;
                        model.ForumID = 0;
                        model.BID = 0;
                        model.ReID = 0;
                        model.Files = DirPath + fileName;
                        if (string.IsNullOrEmpty(prevDirPath))
                            model.PrevFiles = model.Files;
                        else
                            model.PrevFiles = prevDirPath + fileName;

                        model.Content = Content;
                        model.FileSize = Convert.ToInt64(postedFile.ContentLength);
                        model.FileExt = fileExtension;
                        model.DownNum = 0;
                        model.Cent = 0;
                        model.IsVerify = IsVerify;
                        model.AddTime = DateTime.Now;
                        new BCW.BLL.Upfile().Add(model);
                        k++;
                    }
                    // j++;
                    j--;
                }
            }

        }
        catch { }
        kk = k;
    }

    private static string ReplaceWap(string p_strUrl)
    {
        p_strUrl = p_strUrl.Replace("20a", "1a");

        return p_strUrl;
    }
}
