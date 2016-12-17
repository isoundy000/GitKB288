using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using BCW.Common;
using BCW.Files;
using BCW.Files.Model;
public partial class Manage_app_filemanager : System.Web.UI.Page
{
    protected string folderPath = HttpContext.Current.Request["path"];
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");

    protected int folderNum = 0;
    protected int fileNum = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "在线FTP";
        string info = Utils.GetRequest("info", "all", 1, "", "");
        switch (info)
        {
            case "file":
                UploadFile();
                break;
            case "collec":
                CollecFile();
                break;
            case "text":
                CreateText();
                break;
            case "directory":
                CreateDirectory();
                break;
            case "rename":
                RenameDirFile();
                break;
            case "delete":
                DeleteDirFile();
                break;
            default:
                ReloadPage();
                break;
        }
    }
    private void ReloadPage()
    {
        builder.Append(Out.Div("title", "在线FTP系统"));
        string act = Utils.GetRequest("act", "all", 1, "", "");
        //建立文件
        if (act == "createtext")
        {
            bool isfile = bool.Parse(Utils.GetRequest("isfile", "all", 1, @"^True|False#", "False"));
            string txtFilePath = Utils.GetRequest("txtFilePath", "all", 2, @"^[(A-Za-z0-9_)]{1,50}$", "文件名必须为1-50位字母或数字");
            string txtFileExt = Utils.GetRequest("txtFileExt", "all", 2, @"^.txt|.wml|.html|.htm$", "扩展名选择出错");
            string ddlEncode = Utils.GetRequest("ddlEncode", "all", 1, "", "");
            string txtFileContent = Request["txtFileContent"];
            if (txtFileContent.Length > 10000)
            {
                Utils.Error("文件内容不能超10000字", "");
            }
            //拼接路径
            string filePath = Server.UrlDecode(folderPath + "\\" + txtFilePath) + txtFileExt;
            if (isfile == false)
            {
                if (File.Exists(filePath))
                {
                    Utils.Success("此文件名在文件夹中已存在", Utils.getUrl("filemanager.aspx?path=" + folderPath));
                }
            }
            string str = new FileManagerProcessor().SaveTextFile(filePath, txtFileContent, ddlEncode);
            Utils.Success(str, Utils.getUrl("filemanager.aspx?path=" + folderPath));
        }

        // 其它操作处理
        FileManagerProcessor fileManage = new FileManagerProcessor(act);
        if (!string.IsNullOrEmpty(fileManage.Value))
        {
            //得到上一级的文件夹
            string ordpath = "";
            string Temps = "";
            ordpath = Request["objfolder"];
            if (string.IsNullOrEmpty(ordpath))
                ordpath = Request["objfile"];
            if (!string.IsNullOrEmpty(ordpath))
            {
                string[] sTemp = ordpath.Split("\\".ToCharArray());
                for (int i = 0; i < sTemp.Length - 1; i++)
                {
                    Temps += "\\" + sTemp[i];
                }
                Temps = Utils.Mid(Temps, 1, Temps.Length);
                Utils.Success(fileManage.Value, Utils.getUrl("filemanager.aspx?path=" + Server.UrlEncode(Temps)));
            }
            else
            {
                Utils.Success(fileManage.Value, Utils.getUrl("filemanager.aspx?path=" + folderPath));
            }
        }

        if (string.IsNullOrEmpty(folderPath) || folderPath.ToUpper().IndexOf("\\FILES\\") == -1)
        {
            folderPath = Server.MapPath("\\FILES");
        }

        // 组合路径, 快速导航
        string comePath = "";
        foreach (string q in folderPath.Split("\\".ToCharArray()))
        {
            comePath += q;
            //builder.AppendFormat("<br />当前路径:<a href=\"" + Utils.getUrl("filemanager.aspx?path={1}") + "\">{0}</a>", q + "\\", comePath);
            comePath += "\\";
        }

        // 返回上一级
        if (new DirectoryInfo(folderPath).Root.ToString().Replace("\\", "") != folderPath.ToUpper())
        {
            //string previousFolder = folderPath.Substring(0, folderPath.LastIndexOf("\\"));
            //builder.Append("<br /><a href=\""+Utils.getUrl("filemanager.aspx?path=" + Server.UrlEncode(previousFolder) + "")+"\">返回上一级</a>");
        }
        else
        {
            folderPath += "\\";
        }

        int pageIndex;
        int recordCount;
        string path = comePath;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string[] pageValUrl = { "path" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;


        // 列表数据
        fileManage = new FileManagerProcessor();
        List<FileFolderInfo> files = fileManage.GetDirectories(folderPath);

        if (fileManage.Access)
        {
            folderNum = fileManage.FolderNum;
            fileNum = fileManage.FileNum;
            recordCount = folderNum + fileNum;
            if (files.Count > 0)
            {
                builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                builder.Append("本文件夹共" + folderNum + "文件夹," + fileNum + "文件");
                builder.Append(Out.Tab("</div>", "<br />~~~~~~"));
                int stratIndex = (pageIndex - 1) * pageSize;
                int endIndex = pageIndex * pageSize;
                int k = 0;
                foreach (FileFolderInfo n in files)
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        if ((k+1) % 2 == 0)
                            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                        if (!string.IsNullOrEmpty(n.Ext))
                            builder.AppendFormat("{0}大小:{1}", n.FormatName, n.Size);
                        else
                            builder.AppendFormat("{0}", n.FormatName);
                        if (Utils.Isie())
                        {
                            builder.Append("<a href=\"" + Utils.getUrl("filemanager.aspx?info=rename&amp;path=" + Server.UrlEncode(folderPath) + "&amp;file=" + n.FullName + "&amp;filename=" + n.Name + "") + "\"><img src=\"/Files/sys/IcoAdd.gif\"/></a>");
                            builder.Append("<a href=\"" + Utils.getUrl("filemanager.aspx?info=delete&amp;path=" + Server.UrlEncode(folderPath) + "&amp;file=" + n.FullName + "&amp;filename=" + n.Name + "&amp;type=" + n.Type + "") + "\"><img src=\"/Files/sys/IcoDelete.gif\"/></a>");
                        }
                        else
                        {
                            builder.Append("<a href=\"" + Utils.getUrl("filemanager.aspx?info=rename&amp;path=" + Server.UrlEncode(folderPath) + "&amp;file=" + n.FullName + "&amp;filename=" + n.Name + "") + "\">[改名]</a>");
                            builder.Append("<a href=\"" + Utils.getUrl("filemanager.aspx?info=delete&amp;path=" + Server.UrlEncode(folderPath) + "&amp;file=" + n.FullName + "&amp;filename=" + n.Name + "&amp;type=" + n.Type + "") + "\">[删除]</a>");
                        }
                        builder.AppendFormat("修改时间:{0}", n.ModifyDate);
                        builder.Append(Out.Tab("</div>", ""));
                    }
                    if (k == endIndex)
                        break;
                    k++;
                }

            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Tab("", "<br />"));
            builder.Append(Out.Div("text", "无权限访问该文件夹"));
        }

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        if (folderPath.ToUpper().IndexOf("\\FILES\\") != -1)
        {
            builder.Append("新建:<a href=\"" + Utils.getUrl("filemanager.aspx?info=directory&amp;path=" + Server.UrlEncode(folderPath) + "") + "\">文件夹</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("filemanager.aspx?info=text&amp;path=" + Server.UrlEncode(folderPath) + "") + "\">文件</a> ");
            if (Utils.getstrVe().Contains("1"))
            {
                string VE = ConfigHelper.GetConfigString("VE");
                string SID = ConfigHelper.GetConfigString("SID");
                builder.Append("<a href=\"filemanager.aspx?info=file&amp;path=" + Server.UrlEncode(folderPath) + "&amp;" + VE + "=2a&amp;" + SID + "=" + Utils.getstrU() + "\">上传</a> ");
            }
            else
                builder.Append("<a href=\"" + Utils.getUrl("filemanager.aspx?info=file&amp;path=" + Server.UrlEncode(folderPath) + "") + "\">上传</a> ");

            builder.Append("<a href=\"" + Utils.getUrl("filemanager.aspx?info=collec&amp;path=" + Server.UrlEncode(folderPath) + "") + "\">采集</a><br />");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("filemanager.aspx?info=directory&amp;path=" + Server.UrlEncode(folderPath) + "") + "\">新建文件夹</a><br />");
        }
            
        // 返回上一级
        if (folderPath.ToUpper().IndexOf("\\FILES\\") != -1)
        {
            string previousFolder = folderPath.Substring(0, folderPath.LastIndexOf("\\"));
            builder.Append("<a href=\"" + Utils.getUrl("filemanager.aspx?path=" + Server.UrlEncode(previousFolder) + "") + "\">返回上一级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("filemanager.aspx") + "\">返回主文件夹</a><br />");
        }
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">系统服务中心</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void UploadFile()
    {
        builder.Append(Out.Div("title", "上传文件"));
        string strText = "选择文件:/,,";
        string strName = "fileUpload,path,act";
        string strType = "file,hidden,hidden";
        string strValu = "'" + Server.UrlEncode(folderPath) + "'upload";
        string strEmpt = "false.false,false";
        string strIdea = "/";
        string strOthe = "上传文件|reset,filemanager.aspx,post,2,red|blue";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("filemanager.aspx?path=" + Server.UrlEncode(folderPath) + "") + "\">返回上一级</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">系统服务中心</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void CollecFile()
    {
        builder.Append(Out.Div("title", "采集文件"));
        string strText = "文件地址:/,,";
        string strName = "fileUpload,path,act";
        string strType = "text,hidden,hidden";
        string strValu = "'" + Server.UrlEncode(folderPath) + "'ccfile";
        string strEmpt = "false,false,false";
        string strIdea = "/";
        string strOthe = "采集文件|reset,filemanager.aspx,post,2,red|blue";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("filemanager.aspx?path=" + Server.UrlEncode(folderPath) + "") + "\">返回上一级</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">系统服务中心</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void CreateText()
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-9]\d*$", "0"));
        string objfile = Utils.GetRequest("objfile", "all", 1, "", "");
        string txtFilePaths = "";
        string txtFilePath = "";
        string txtFileExt = "";
        string aName = "新建";
        bool isfile = false;
        if (!string.IsNullOrEmpty(objfile))
        {
            if (File.Exists(Server.UrlDecode(objfile)))
            {
                txtFilePaths = Server.UrlDecode(objfile).Replace(Server.UrlDecode(folderPath), "").Replace("\\", "");
                //取扩展名
                txtFileExt = txtFilePaths.Substring(txtFilePaths.LastIndexOf("."), txtFilePaths.Length - txtFilePaths.LastIndexOf("."));
                //得到去掉扩展名的文件名
                txtFilePath = txtFilePaths.Substring(0, txtFilePaths.Length - txtFileExt.Length);

                isfile = true;
                aName = "编辑" + txtFilePaths + "";
            }
        }

        builder.Append(Out.Div("title", "" + aName + "文件"));

        if (isfile == false)
        {
            builder.Append(Out.Tab("<div class=\"m1\">", "<br />"));
            builder.Append("模板:<a href=\"" + Utils.getUrl("filemanager.aspx?info=text&amp;path=" + Server.UrlEncode(folderPath) + "") + "\">txt</a>  ");
            builder.Append(" <a href=\"" + Utils.getUrl("filemanager.aspx?info=text&amp;path=" + Server.UrlEncode(folderPath) + "&amp;ptype=1") + "\">wml</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("filemanager.aspx?info=text&amp;path=" + Server.UrlEncode(folderPath) + "&amp;ptype=2") + "\">html</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        string strText = "文件名:/,文件内容:/,文件扩展名:/,文件编码:/,,,";
        string strName = "txtFilePath,txtFileContent,txtFileExt,ddlEncode,path,act,isfile";
        string strType = "";
        if (isfile == false)
        {
            strType = "text,big,select,select,hidden,hidden,hidden";
        }
        else
        {
            strType = "hidden,big,hidden,select,hidden,hidden,hidden";
        }
        string strValu = "";
        if (isfile == false)
        {
            string str = "";
            string ext = "";
            if (ptype == 1)
            {
                str = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<!DOCTYPE wml PUBLIC \"-//WAPFORUM//DTD WML 1.1//EN\" \"http://www.wapforum.org/DTD/wml_1.1.xml\">\r\n<wml>\r\n<head>\r\n<meta http-equiv=\"Cache-Control\" content=\"max-age=0\" />\r\n</head>\r\n<card id=\"main\" title=\"页面标题\">\r\n<p>\r\n这里编写内容\r\n</p>\r\n</card>\r\n</wml>";
                str = Out.WmlEncode(str);
                ext = ".wml";
            }
            else if (ptype == 2)
            {
                str = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<!DOCTYPE html PUBLIC \"-//WAPFORUM//DTD XHTML Mobile 1.0//EN\" \"http://www.wapforum.org/DTD/xhtml-mobile10.dtd\">\r\n<html xmlns=\"http://www.w3.org/1999/xhtml\" >\r\n<head>\r\n<meta http-equiv=\"Content-Type\" content=\"application/xhtml+xml;charset=UTF-8\"/>\r\n<title>页面标题</title>\r\n</head>\r\n<body>\r\n这里编写内容\r\n</body>\r\n</html>";
                str = Out.WmlEncode(str);
                ext = ".html";
            }
            else
            {
                str = "请输入内容";
                ext = ".txt";
            }
            strValu = "'" + str + "'" + ext + "'UTF-8'" + Server.UrlEncode(folderPath) + "'createtext'" + isfile + "";
        }
        else
        {
            string fileContent;
            string getEncode;
            new FileManagerProcessor().ReadTextFile(objfile, out fileContent, out getEncode);
            strValu = "" + txtFilePath + "'" + Out.WmlEncode(fileContent) + "'" + txtFileExt + "'" + getEncode + "'" + Server.UrlEncode(folderPath) + "'createtext'" + isfile + "";
        }

        string strEmpt = ",,.txt|.txt|.wml|.wml|.html|.html,UTF-8|UTF-8|ANSI|ANSI|Unicode|Unicode|Unicode big endian|Unicode-be,,,";
        string strIdea = "/";
        string strOthe = "" + Utils.Left(aName, 2) + "文件|reset,filemanager.aspx,post,1,red|blue";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("filemanager.aspx?path=" + Server.UrlEncode(folderPath) + "") + "\">返回上一级</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">系统服务中心</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }


    private void CreateDirectory()
    {
        builder.Append(Out.Div("title", "新建文件夹"));
        string strText = "新文件夹:/,,";
        string strName = "txtFolderName,path,act";
        string strType = "text,hidden,hidden";
        string strValu = "NewFolder" + new Random().Next(1, 50).ToString() + "'" + Server.UrlEncode(folderPath) + "'create";
        string strEmpt = "false,false,false";
        string strIdea = "/";
        string strOthe = "新建文件夹|reset,filemanager.aspx,post,1,red|blue";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("filemanager.aspx?path=" + Server.UrlEncode(folderPath) + "") + "\">返回上一级</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">系统服务中心</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void RenameDirFile()
    {
        string file = Utils.GetRequest("file", "get", 2, @"[\s\S]{1,}", "路径错误");
        string filename = Utils.GetRequest("filename", "get", 2, @"[\s\S]{1,}", "路径错误");

        
        builder.Append(Out.Div("title", "重命名"));
        string strText = "名称:/,,,";
        string strName = "txtFolderName,path,act,txtOldName";
        string strType = "text,hidden,hidden,hidden";
        string strValu = "" + filename + "'" + Server.UrlEncode(folderPath) + "'rename'" + Server.UrlEncode(file) + "";
        string strEmpt = "false,false,false,false";
        string strIdea = "/对文件, 文件夹重命名时, 可在前面加入相对路径(如: \"..\\\", \"a\\\"), 即可实现移动文件, 文件夹功能./";
        string strOthe = "重命名|reset,filemanager.aspx,post,1,red|blue";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("filemanager.aspx?path=" + Server.UrlEncode(folderPath) + "") + "\">返回上一级</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">系统服务中心</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void DeleteDirFile()
    {
        string file = Utils.GetRequest("file", "get", 2, @"[\s\S]{1,}", "路径错误");
        string filename = Utils.GetRequest("filename", "get", 2, @"[\s\S]{1,}", "路径错误");
        string type = Utils.GetRequest("type", "get", 2, @"[\s\S]{1,}", "路径错误");

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("确定要删除" + filename + "吗<br />");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("filemanager.aspx?act=delete&amp;path=" + Server.UrlEncode(folderPath) + "&amp;file=" + Server.UrlEncode(file) + "&amp;type=" + type + "") + "\">确定删除</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("filemanager.aspx?path=" + Server.UrlEncode(folderPath)) + "\">先留着吧..</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
}