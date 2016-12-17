using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Net;
using System.IO;
using System.Drawing.Imaging;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text.RegularExpressions;
using BCW.Common;
using BCW.Files;
public partial class addfileok : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/upfile.xml";
    protected void Page_Load(object sender, EventArgs e)
    {
        //防止缓存
        Response.Expires = -1;
        Response.Cache.SetNoStore();
        Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[0-9]\d*$", "0"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 2, @"^[0-9]\d*$", "类型选择错误"));
        int nid = int.Parse(Utils.GetRequest("nid", "all", 1, @"^[0-9]\d*$", "0"));
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "soft":
                UpdateOther(act, "文件", ptype, nid, id);
                break;
            case "pic2":
                UiPic(act, ptype, nid, id);
                break;
            case "pic2save":
                UpdatePic(act, ptype, nid, id);
                break;
            default:
                UpdateOther(act, "文件", ptype, nid, id);
                break;
        }
    }

    /// <summary>
    /// 添加/编辑文章/图片/文件
    /// </summary>
    private void UpdateOther(string act, string TypeName, int ptype, int nid, int id)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string ac = Utils.GetRequest("ac", "post", 1, "", "");
        string Title = Utils.GetRequest("Title", "post", 2, @"^[\s\S]{1,50}$", "" + TypeName + "标题限1-50字");
        string KeyWord = Utils.GetRequest("KeyWord", "post", 1, "", "");
        string Content = Utils.GetRequest("Content", "post", 2, @"^[\s\S]{1,}$", "" + TypeName + "内容不能为空");
        string TarText = string.Empty;
        string LanText = string.Empty;
        string SafeText = string.Empty;
        string LyText = string.Empty;
        string UpText = string.Empty;
        int IsVisa = 0;
        bool IsAd = true;
        bool blpic = bool.Parse(Utils.GetRequest("blpic", "post", 1, @"^False|True$", "False"));//文件是否上传截图
        int BzType = 0;
        int Cent = 0;
        string sFiles = string.Empty;
        string aName = string.Empty;
        string Model = Utils.GetRequest("Model", "post", 1, "", "");
        int newId = 0;

        //关键字的生成
        if (string.IsNullOrEmpty(KeyWord))
        {
            KeyWord = Out.CreateKeyWord(Title, 2);
        }
        else
        {
            if (KeyWord.Length > 500)
            {
                Utils.Error("关键字不能超500字", "");
            }
            KeyWord = Utils.GetRequest("KeyWord", "post", 2, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "关键字填写格式错误");
        }
        if (blpic != true)
        {
            if (id == 0)
            {
                aName = "添加" + TypeName + "";
            }
            else
            {
                aName = "编辑" + TypeName + "";
            }
        }
        else
        {
            aName = "上传截图";
        }
        Master.Title = aName;

        BCW.Model.Detail model = new BCW.Model.Detail();
        model.Title = Title;
        model.KeyWord = KeyWord;
        model.Model = Model.ToUpper().Replace("，", ",");
        model.IsAd = IsAd;
        model.Types = ptype;
        model.NodeId = nid;
        model.Content = Content;
        model.TarText = TarText;
        model.LanText = LanText;
        model.SafeText = SafeText;
        model.LyText = LyText;
        model.UpText = UpText;
        model.IsVisa = IsVisa;
        model.AddTime = DateTime.Now;
        model.Readcount = 0;
        model.Recount = 0;
        model.Cent = Cent;
        model.BzType = BzType;
        model.Hidden = 1;
        model.UsID = meid;
        if (id == 0)
        {
            newId = new BCW.BLL.Detail().Add(model);
        }
        else
        {
            newId = id;
            model.ID = id;
            new BCW.BLL.Detail().Update(model);
        }

        //-----------------文件附件提交开始
        if (SaveFiles(ptype, newId, out sFiles))
        {
            aName += "/上传文件";
        }
        //-----------------文件附件提交结束
        //得到截图文件
        string sPics = "";
        string Pics = "";
        if (sFiles != "")
            sPics = Utils.Mid(sFiles, 1, sFiles.Length);
        if (sPics == "#")
            sPics = "";

        if (sPics != "")
        {
            Pics = new BCW.BLL.Detail().GetPics(newId);
            if (Pics != "")
                sPics = Pics + "#" + sPics;

            new BCW.BLL.Detail().UpdatePics(newId, sPics);
        }

        //截图上传开始

        if (blpic == true)
        {
            UiPic(act, ptype, nid, newId);
        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("" + aName + "成功！请等待管理员进行审核，多谢您对本站的支持！");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">返回上级</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    /// <summary>
    /// 更新截图进数据库
    /// </summary>
    /// <param name="act"></param>
    /// <param name="ptype"></param>
    /// <param name="nid"></param>
    /// <param name="id"></param>
    private void UpdatePic(string act, int ptype, int nid, int id)
    {
        string ac = Utils.GetRequest("ac", "post", 1, "", "");
        string sFiles = string.Empty;
        string aName = string.Empty;
        if (Utils.ToSChinese(ac) == "上传截图")
        {
            if (SaveFiles(15, id, out sFiles))
            {
                aName = "上传截图成功";
                string sPics = "";
                if (sFiles != "")
                    sPics = Utils.Mid(sFiles, 1, sFiles.Length);
                if (sPics == "#")
                    sPics = "";

                if (sPics != "")
                {
                    string Pics = new BCW.BLL.Detail().GetPics(id);
                    if (Pics != "")
                        sPics = Pics + "#" + sPics;

                    new BCW.BLL.Detail().UpdatePics(id, sPics);
                }
            }
            else
            {
                aName = "上传截图失败";
            }
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("" + aName + "成功！请等待管理员进行审核，多谢您对本站的支持！");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">返回上级</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }


    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="sFiles"></param>
    /// <returns></returns>
    private Boolean SaveFiles(int ptype, int newId, out string sFiles)
    {
        //图片系统缩略图设置
        int ThumbType = int.Parse(Utils.GetRequest("ThumbType", "post", 1, @"^[0-2]$", "0"));
        int width = int.Parse(Utils.GetRequest("width", "post", 1, @"^[1-9]\d*$", "75"));
        int height = int.Parse(Utils.GetRequest("height", "post", 1, @"^[1-9]\d*$", "100"));

        //遍历File表单元素
        System.Web.HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
        try
        {
            int k = 0;
            int j = 1;
            string GetFiles = string.Empty;
            for (int iFile = 0; iFile < files.Count; iFile++)
            {
                //检查文件扩展名字
                System.Web.HttpPostedFile postedFile = files[iFile];
                string fileName, fileExtension;
                fileName = System.IO.Path.GetFileName(postedFile.FileName);
                string UpExt = ub.GetSub("UpbFileExt", xmlPath);
                if (fileName != "")
                {
                    fileExtension = System.IO.Path.GetExtension(fileName).ToLower();
                    if (UpExt.IndexOf(fileExtension) == -1)
                    {
                        continue;
                    }
                    //非法上传
                    if (fileExtension == ".asp" || fileExtension == ".aspx" || fileExtension == ".jsp" || fileExtension == ".php" || fileExtension == ".asa" || fileExtension == ".cer" || fileExtension == ".cdx" || fileExtension == ".htr" || fileExtension == ".exe")
                    {
                        continue;
                    }
                    string DirPath = string.Empty;
                    string prevDirPath = string.Empty;
                    string Path = string.Empty;
                    string prevPath = string.Empty;
     
                    Path = "/Files/soft/";
                    if (FileTool.CreateDirectory(Path, out DirPath))
                    {
                        //生成随机文件名
                        fileName = DT.getDateTimeNum() + iFile + fileExtension;
                        string SavePath = System.Web.HttpContext.Current.Request.MapPath(DirPath) + fileName;
                        postedFile.SaveAs(SavePath);
                        //图片系统缩略图
                        if (ptype == 12)
                        {
                            if (ThumbType > 0)
                            {
                                bool pbool = false;
                                if (ThumbType == 1)
                                    pbool = true;
                                if (FileTool.CreateDirectory(prevPath, out prevDirPath))
                                {
                                    string prevSavePath = System.Web.HttpContext.Current.Request.MapPath(prevDirPath) + fileName;
                                    if (fileExtension == ".gif")
                                    {
                                        if (ThumbType > 0)
                                            new BCW.Graph.GifHelper().GetThumbnail(SavePath, prevSavePath, width, height, pbool);

                                    }
                                    else
                                    {
                                        if (ThumbType > 0)
                                            new BCW.Graph.ImageHelper().ResizeImage(SavePath, prevSavePath, width, height, pbool);
                                    }
                                }
                            }
                        }
                        if (ptype == 11 || ptype == 14)
                        {

                            GetFiles += "#" + DirPath + fileName + "";
                        }
                        else if (ptype == 12)
                        {
                            //取显示的名称
                            string fname = string.Empty;
                            if (!string.IsNullOrEmpty(Request.Form["stext" + j + ""].ToString()))
                                fname = Out.UBB(Request.Form["stext" + j + ""].Trim());
                            else
                                fname = "";

                            BCW.Model.File model = new BCW.Model.File();
                            model.Types = 1;
                            model.NodeId = newId;
                            model.Files = DirPath + fileName;
                            if (!string.IsNullOrEmpty(prevDirPath))
                                model.PrevFiles = prevDirPath + fileName;
                            else
                                model.PrevFiles = "";

                            model.FileSize = Convert.ToInt64(postedFile.ContentLength);
                            model.FileExt = fileExtension;
                            model.Content = fname;
                            model.DownNum = 0;
                            model.AddTime = DateTime.Now;
                            new BCW.BLL.File().Add(model);

                        }
                        else if (ptype == 13)
                        {
                            //取显示的名称
                            string fname = string.Empty;
                            if (!string.IsNullOrEmpty(Request.Form["stext" + j + ""].ToString()))
                                fname = Out.UBB(Request.Form["stext" + j + ""].Trim());
                            else
                                fname = fileName;

                            BCW.Model.File model = new BCW.Model.File();
                            model.Types = 2;
                            model.NodeId = newId;
                            model.Files = DirPath + fileName;
                            model.PrevFiles = "";
                            model.FileSize = Convert.ToInt64(postedFile.ContentLength);
                            model.FileExt = fileExtension;
                            model.Content = fname;
                            model.DownNum = 0;
                            model.AddTime = DateTime.Now;
                            new BCW.BLL.File().Add(model);
                        }
                        else
                        {
                            GetFiles += "#" + DirPath + fileName + "";
                        }
                        //封面设置
                        InsertCover(ptype, Path, SavePath, fileName, fileExtension, iFile, files.Count, newId);
                        //添加水印
                        UpdateThumb(ptype, fileExtension, SavePath);

                        k++;
                    }
                    j++;
                }
            }
            sFiles = GetFiles;
            if (k > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch
        {
            sFiles = "";
            return false;
        }
    }

    /// <summary>
    /// 文件上传截图
    /// </summary>
    /// <param name="act"></param>
    /// <param name="ptype"></param>
    /// <param name="nid"></param>
    /// <param name="id"></param>
    private void UiPic(string act, int ptype, int nid, int id)
    {
        if (act == "pic2")
        {
            Master.Title = "添加文件";
            builder.Append(Out.Div("title", "上传截图"));
        }
        //上传个数
        int nn = int.Parse(Utils.GetRequest("nn", "all", 1, @"^[0-9]\d*$", "1"));
        if (nn == 0)
            nn = 1;
        if (nn > 10)
            nn = 10;
        string strText = string.Empty;
        string strName = string.Empty;
        string strType = string.Empty;
        string strValu = string.Empty;
        string strEmpt = string.Empty;
        string strIdea = string.Empty;
        string strOthe = string.Empty;
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("截图:");
        for (int i = 1; i < 10; i++)
        {
            builder.Append("<a href=\"" + Utils.getUrl("addfileok.aspx?act=pic2&amp;nid=" + nid + "&amp;id=" + id + "&amp;ptype=" + ptype + "&amp;nn=" + i + "") + "\"><b>" + i + "</b></a> ");
        }
        builder.Append("张");
        builder.Append(Out.Tab("</div>", ""));
        for (int i = 0; i < nn; i++)
        {
            string y = ",";
            if (nn == 1)
            {
                strText = strText + y + "选择截图:/";
            }
            else
            {
                strText = strText + y + "第" + (i + 1) + "张截图:/";
            }
            strName = strName + y + "pic" + (i + 1);
            strType = strType + y + "file";
            strValu = strValu + "'";
            strEmpt = strEmpt + y;
        }
        strText += ",,,,,";
        strName += ",ptype,nid,id,act,nn";
        strType += ",hidden,hidden,hidden,hidden,hidden";
        strValu += "'" + ptype + "'" + nid + "'" + id + "'pic2save'" + nn + "";
        strEmpt += ",,,,,";
        strIdea += "/";
        strOthe = "上传截图|reset,addfileok.aspx,post,2,red|blue|blue";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
    }

    /// <summary>
    /// 添加水印
    /// </summary>
    private void UpdateThumb(int ptype, string fileExtension, string SavePath)
    {
        if (fileExtension == ".gif" || fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".png" || fileExtension == ".bmp")
        {
            int IsThumb = 0;
            if (ptype == 11)
                IsThumb = Convert.ToInt32(ub.GetSub("UpIsTextThumb", xmlPath));
            else if (ptype == 12)
                IsThumb = Convert.ToInt32(ub.GetSub("UpIsPicThumb", xmlPath));
            else if (ptype == 13 || ptype == 15)
                IsThumb = Convert.ToInt32(ub.GetSub("UpIsFileThumb", xmlPath));
            else if (ptype == 14)
                IsThumb = Convert.ToInt32(ub.GetSub("UpIsShopThumb", xmlPath));

            if (IsThumb > 0)
            {
                int IsThumbType = 0;
                IsThumbType = Convert.ToInt32(ub.GetSub("UpbIsThumb", xmlPath));
                if (fileExtension == ".gif")
                {

                    if (IsThumbType == 1)
                        new BCW.Graph.GifHelper().SmartWaterMark(SavePath, "", ub.GetSub("UpbWord", xmlPath), ub.GetSub("UpbWordColor", xmlPath), "Arial", 12, Convert.ToInt32(ub.GetSub("UpbPosition", xmlPath)));//文字水印
                    else if (IsThumbType == 2)
                        new BCW.Graph.GifHelper().WaterMark(SavePath, "", ub.GetSub("UpbWord", xmlPath), Convert.ToInt32(ub.GetSub("UpbPosition", xmlPath)), Convert.ToInt32(ub.GetSub("UpbPosition", xmlPath)));//图片水印

                }
                else
                {
                    if (IsThumbType == 1)
                        new BCW.Graph.ImageHelper().WaterMark(SavePath, "", ub.GetSub("UpbWord", xmlPath), ub.GetSub("UpbWordColor", xmlPath), "Arial", 12, Convert.ToInt32(ub.GetSub("UpbPosition", xmlPath)));//文字水印
                    else if (IsThumbType == 2)
                        new BCW.Graph.ImageHelper().WaterMark(SavePath, "", ub.GetSub("UpbWord", xmlPath), Convert.ToInt32(ub.GetSub("UpbPosition", xmlPath)), Convert.ToInt32(ub.GetSub("UpbTran", xmlPath)));//图片水印

                }
            }
        }
    }

    /// <summary>
    /// 设置列表封面
    /// </summary>
    private void InsertCover(int ptype, string Path, string SavePath, string fileName, string fileExtension, int iFile, int iFileCount, int newId)
    {
        if (fileExtension == ".gif" || fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".png" || fileExtension == ".bmp")
        {
            if ((iFile + 1) == iFileCount)
            {
                string CoverPath = Path + "cover/";
                string CoverDirPath = "";
                if (FileTool.CreateDirectory(CoverPath, out CoverDirPath))
                {
                    string CoverSavePath = System.Web.HttpContext.Current.Request.MapPath(CoverDirPath) + fileName;
                    int width = Convert.ToInt32(ub.GetSub("UpCoverWidth", xmlPath));
                    int height = Convert.ToInt32(ub.GetSub("UpCoverHeight", xmlPath));
                    bool pbool = false;
                    if (fileExtension == ".gif")
                    {
                        new BCW.Graph.GifHelper().GetThumbnail(SavePath, CoverSavePath, width, height, pbool);

                    }
                    else
                    {
                        new BCW.Graph.ImageHelper().ResizeImage(SavePath, CoverSavePath, width, height, pbool);
                    }
                    //更新封面
                    string Cover = "";
                    if (ptype != 14)
                    {
                        Cover = new BCW.BLL.Detail().GetCover(newId);
                    }
                    else
                    {
                        Cover = new BCW.BLL.Goods().GetCover(newId);
                    }
                    if (Cover != "")
                    {
                        BCW.Files.FileTool.DeleteFile(Cover);
                    }
                    if (ptype != 14)
                        new BCW.BLL.Detail().UpdateCover(newId, CoverDirPath + fileName);
                    else
                        new BCW.BLL.Goods().UpdateCover(newId, CoverDirPath + fileName);

                }
            }
        }
    }
}