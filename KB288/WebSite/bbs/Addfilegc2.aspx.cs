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
using System.Text.RegularExpressions;
using BCW.Common;
using BCW.Files;
/// <summary>
/// 陈志基 16/5/18
/// 修改SaveFiles（）文件上传顺序
/// </summary>
public partial class bbs_Addfilegc2 : System.Web.UI.Page
{
    protected string xmlPath = "/Controls/upfile.xml";
    protected string xmlPath2 = "/Controls/bbs.xml";
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Write("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
        Response.Write("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
        Response.Write("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
        Response.Write("<head>");
        Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
        Response.Write("<meta name=\"viewport\" content=\"width=device-width; initial-scale=1.0; minimum-scale=1.0; maximum-scale=2.0; user-scalable=0;\" />");
        Response.Write("<meta http-equiv=\"Cache-Control\" content=\"max-age=0\"/>");
        Response.Write("<title>WAP2.0文件上传</title>");
        Response.Write("</head>");
        Response.Write("<body>");
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
        {
            Response.Write("请您先<a href=\"" + Utils.getUrl("/login.aspx") + "\">登录</a>!<br/>");
            Response.Write(Out.back("返回上一级"));
            Response.Write("</body></html>");
            Response.End();
        }

        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "upload":
                UploadPage(meid);
                break;
            case "photo":
                PhotoPage(meid);
                break;
            case "phupload":
                PhUploadPage(meid);
                break;
            default:
                UpfilePage(meid);
                break;
        }

    }

    private void UpfilePage(int meid)
    {
        int leibie = int.Parse(Utils.GetRequest("leibie", "all", 1, @"^[1-4]\d*$", "1"));
        Response.Write("WAP2.0上传" + BCW.User.AppCase.CaseAlbums(leibie).Replace("册", "片") + "");
        int max = Convert.ToInt32(ub.GetSub("UpaMaxFileNum", xmlPath));
        string maxfile = ub.GetSub("UpaMaxFileSize", xmlPath);
        string fileExt = ub.GetSub("UpaFileExt", xmlPath);

        Response.Write("上传允许格式:" + fileExt + "<br />");
        Response.Write("每个文件限" + maxfile + "K");


        Response.Write("<form name=\"form2\" method=\"post\" action=\"addfilegc2.aspx\" enctype=\"multipart/form-data\">");
        string VE = ConfigHelper.GetConfigString("VE");
        string SID = ConfigHelper.GetConfigString("SID");

        Response.Write("选择文件(第1个必须选择,否则无法上传)<br/>");
        Response.Write("第1个文件:<br/><input type=\"file\" name=\"filename1\"/><BR/>");
        Response.Write("描述1(30字内):<br/><input name=\"stext1\" maxlength=\"30\" value=\"\"/><br/><br/>");

        Response.Write("第2个文件:<br/><input type=\"file\" name=\"filename2\"/><BR/>");
        Response.Write("描述2(30字内):<br/><input name=\"stext2\" maxlength=\"30\" value=\"\"/><br/><br/>");

        Response.Write("第3个文件:<br/><input type=\"file\" name=\"filename3\"/><BR/>");
        Response.Write("描述3(30字内):<br/><input name=\"stext3\" maxlength=\"30\" value=\"\"/><br/><br/>");
        Response.Write("<input name=\"leibie\" type=\"hidden\" value=\"" + leibie + "\"/>");
        Response.Write("<input name=\"act\" type=\"hidden\" value=\"upload\"/>");
        Response.Write("<input type=\"hidden\" name=\"" + VE + "\" value=\"" + Utils.getstrVe() + "\"/>");
        Response.Write("<input type=\"hidden\" name=\"" + SID + "\" value=\"" + Utils.getstrU() + "\"/>");
        Response.Write("<input name=\"backurl\" type=\"hidden\" value=\"" + Utils.getPage(0) + "\"/>");
        Response.Write("<input type=\"submit\" value=\"我要上传\"/>");
        Response.Write("</form>");

        Response.Write("<a href=\"" + ReplaceWap(Utils.getUrl("/default.aspx")) + "\">首页</a>-");
        Response.Write("<a href=\"" + ReplaceWap(Utils.getPage("albums.aspx?uid=" + meid + "leibie=" + leibie + "")) + "\">上级</a>-");
        Response.Write("<a href=\"" + ReplaceWap(Utils.getUrl("albums.aspx")) + "\">相册</a>");
        Response.Write("<br/><a href=\"" + Utils.getUrl("default.aspx") + "\">返回社区首页</a>");
        Response.Write("<br/>=上传帮助说明= ");
        Response.Write("<br/>1.建议所上传文件名和目录组合是字母与数字. ");
        Response.Write("<br/>2.上传文件过大可能造成上传失败. ");
        Response.Write("<br/>3.只有部分手机支持上传,详情请联系您的手机供应商. ");
        Response.Write("<br/>4.文件上传需要时间较长,请耐心等待. ");
        Response.Write("<br/>5.如果上传失败,建议您尝试更换手机的上网端口80或改成9201.");
        Response.Write("</body></html>");
    }

    /// <summary>
    /// 上传文件页面
    /// </summary>
    private void UploadPage(int meid)
    {
        int leibie = int.Parse(Utils.GetRequest("leibie", "all", 1, @"^[1-4]\d*$", "1"));
        int NodeId = int.Parse(Utils.GetRequest("NodeId", "all", 1, @"^[0-9]\d*$", "0"));

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
        Response.Write("上传" + kk + "个文件成功！" + strOut + "");
        Response.Write("<br/><a href=\"" + ReplaceWap(Utils.getUrl("addfile.aspx?act=upfile&amp;leibie=" + leibie + "")) + "\">返回继续上传</a>");
        Response.Write("<br/><a href=\"" + ReplaceWap(Utils.getUrl("albums.aspx?uid=" + meid + "&amp;leibie=" + leibie + "")) + "\">返回之前页面</a>");
        Response.Write("<br/><a href=\"" + ReplaceWap(Utils.getUrl("default.aspx")) + "\">返回社区首页</a>");
        Response.Write("</body></html>");

    }

    private void PhotoPage(int meid)
    {
        Response.Write("WAP2.0上传头像");
        string maxfile = ub.GetSub("UpaPhFileSize", xmlPath);
        string fileExt = ".gif,.jpg,.jpeg";
        Response.Write("上传允许格式:" + fileExt + "<br />");
        Response.Write("头像限" + maxfile + "K内.超过尺寸宽240*高320像素将自动缩小");
        Response.Write("<form name=\"form2\" method=\"post\" action=\"addfilegc2.aspx\" enctype=\"multipart/form-data\">");
        string VE = ConfigHelper.GetConfigString("VE");
        string SID = ConfigHelper.GetConfigString("SID");

        Response.Write("选择图片<br/>");
        Response.Write("<input type=\"file\" name=\"filename1\"/><BR/>");
        Response.Write("<input name=\"act\" type=\"hidden\" value=\"phupload\"/>");
        Response.Write("<input type=\"hidden\" name=\"" + VE + "\" value=\"" + Utils.getstrVe() + "\"/>");
        Response.Write("<input type=\"hidden\" name=\"" + SID + "\" value=\"" + Utils.getstrU() + "\"/>");
        Response.Write("<input name=\"backurl\" type=\"hidden\" value=\"" + Utils.getPage(0) + "\"/>");
        Response.Write("<input type=\"submit\" value=\"我要上传\"/>");
        Response.Write("</form>");

        Response.Write("<a href=\"" + ReplaceWap(Utils.getPage("myedit.aspx")) + "\">返回之前页面</a>");
        Response.Write("<br/><a href=\"" + Utils.getUrl("default.aspx") + "\">返回社区首页</a>");
        Response.Write("<br/>=上传帮助说明= ");
        Response.Write("<br/>1.建议所上传文件名和目录组合是字母与数字. ");
        Response.Write("<br/>2.上传文件过大可能造成上传失败. ");
        Response.Write("<br/>3.只有部分手机支持上传,详情请联系您的手机供应商. ");
        Response.Write("<br/>4.文件上传需要时间较长,请耐心等待. ");
        Response.Write("<br/>5.如果上传失败,建议您尝试更换手机的上网端口80或改成9201.");
        Response.Write("</body></html>");
    }

    private void PhUploadPage(int meid)
    {
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
            string UpExt = ".gif,.jpg,.jpeg,.png,.bmp";
            int UpLength = Convert.ToInt32(ub.GetSub("UpaPhFileSize", xmlPath));
            if (fileName != "")
            {
                fileExtension = System.IO.Path.GetExtension(fileName).ToLower();
                //检查是否允许上传格式
                if (UpExt.IndexOf(fileExtension) == -1)
                {
                    Response.Write("头像图片格式只允许" + UpExt + "<br/>");
                    Response.Write(Out.back("返回上一级"));
                    Response.Write("</body></html>");
                    Response.End();
                }
                if (postedFile.ContentLength > Convert.ToInt32(UpLength * 1024))
                {
                    Response.Write("头像大小限" + UpLength + "K内<br/>");
                    Response.Write(Out.back("返回上一级"));
                    Response.Write("</body></html>");
                    Response.End();
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
                        Response.Write("头像宽高尺寸都不能小于15像素<br/>");
                        Response.Write(Out.back("返回上一级"));
                        Response.Write("</body></html>");
                        Response.End();
                    }
                    if (w > 240 || h > 320)
                    {
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
                            Response.Write("头像上传失败，头像图片格式也许已损坏，可以重试一下<br/>");
                            Response.Write(Out.back("返回上一级"));
                            Response.Write("</body></html>");
                            Response.End();
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
                    Response.Write("传出现错误<br/>");
                    Response.Write(Out.back("返回上一级"));
                    Response.Write("</body></html>");
                    Response.End();
                }
            }
        }
        //动态记录
        new BCW.BLL.Action().Add(meid, "在空间设置了[URL=/bbs/uinfo.aspx?uid=" + meid + "]新的头像[/URL]");
        Response.Write("上传/设置头像成功！");
        Response.Write("<br/><a href=\"" + ReplaceWap(Utils.getUrl("uinfo.aspx")) + "\">&gt;查看效果</a>");
        Response.Write("<br/><a href=\"" + ReplaceWap(Utils.getPage("uinfo.aspx")) + "\">返回之前页面</a>");
        Response.Write("<br/><a href=\"" + ReplaceWap(Utils.getUrl("default.aspx")) + "\">返回社区首页</a>");
        Response.Write("</body></html>");
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