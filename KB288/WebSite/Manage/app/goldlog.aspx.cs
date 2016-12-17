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
using BCW.Files.Model;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;

public partial class Manage_app_goldlog : System.Web.UI.Page
{
    protected string folderPath = HttpContext.Current.Request["path"];
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");

    protected int folderNum = 0;
    protected int fileNum = 0;
    static Stopwatch watch = new Stopwatch();

    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "read":
                ReadTxtPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        Master.Title = "历史消费日志";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("历史消费日志");
        builder.Append(Out.Tab("</div>", ""));

        string act = Utils.GetRequest("act", "all", 1, "", "");
        //得到上一级的文件夹
        string ordpath = "";

        ordpath = Request["objfolder"];
        if (string.IsNullOrEmpty(ordpath))
            ordpath = Request["objfile"];

        if (string.IsNullOrEmpty(folderPath) || folderPath.ToUpper().IndexOf("\\LOG\\") == -1)
        {
            folderPath = Server.MapPath("\\LOG\\GOLD");
        }

        // 组合路径, 快速导航
        string comePath = "";
        foreach (string q in folderPath.Split("\\".ToCharArray()))
        {
            comePath += q;
            //builder.AppendFormat("<br />当前路径:<a href=\"" + Utils.getUrl("" + Utils.getPageUrl() + "?path={1}") + "\">{0}</a>", q + "\\", comePath);
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


        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[0-9]\d*$", "0"));
        if (uid > 0)
        {
            folderPath += "\\" + uid + "\\" + DESEncrypt.Encrypt(uid.ToString(), "kubaoLogenpt") + "\\";

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
        FileManagerProcessor2 fileManage = new FileManagerProcessor2();
        List<FileFolderInfo> files = fileManage.GetDirectories(folderPath);

        if (fileManage.Access)
        {
            folderNum = fileManage.FolderNum;
            fileNum = fileManage.FileNum;
            recordCount = folderNum + fileNum;
            if (files.Count > 0)
            {
                //builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                //builder.Append("本文件夹共" + folderNum + "文件夹," + fileNum + "文件");
                //builder.Append(Out.Tab("</div>", "<br />~~~~~~"));
                int stratIndex = (pageIndex - 1) * pageSize;
                int endIndex = pageIndex * pageSize;
                int k = 0;
                foreach (FileFolderInfo n in files)
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        if ((k + 1) % 2 == 0)
                            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                        if (!string.IsNullOrEmpty(n.Ext))
                        {

                            builder.Append("<a href=\"" + Utils.getUrl("" + Utils.getPageUrl() + "?act=read&amp;path=" + Server.UrlEncode(folderPath) + "&amp;file=" + n.FullName + "&amp;filename=" + n.Name + "") + "\">" + n.FormatName.Replace("_0.log", "月" + ub.Get("SiteBz") + "日志").Replace("_1.log", "月" + ub.Get("SiteBz2") + "日志") + "</a>");
                            builder.AppendFormat("大小:{0}", n.Size);
                        }
                        else
                        {
                            builder.AppendFormat("{0}", n.FormatName);
                        }

                        //builder.AppendFormat("修改时间:{0}", n.ModifyDate);
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

        string strText = "输入用户ID:/,";
        string strName = "uid";
        string strType = "num";
        string strValu = "'";
        string strEmpt = "true";
        string strIdea = "/";
        string strOthe = "搜ID日志,goldlog.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));

        // 返回上一级
        if (folderPath.ToUpper().IndexOf("\\LOG\\GOLD\\") != -1)
        {
            string previousFolder = folderPath.Substring(0, folderPath.LastIndexOf("\\"));
            builder.Append("<a href=\"" + Utils.getUrl("" + Utils.getPageUrl() + "?path=" + Server.UrlEncode(previousFolder) + "") + "\">返回上一级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("" + Utils.getPageUrl() + "") + "\">返回主文件夹</a><br />");
        }
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">系统服务中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void ReadTxtPage()
    {
        string folderPath = Utils.GetRequest("Path", "all", 1, "", "");
        string filePath = Utils.GetRequest("file", "all", 1, "", "");
        string filename = Utils.GetRequest("filename", "all", 1, "", "");
        watch.Start();        

        string sTitle = string.Empty;
        string BzText = string.Empty;
        if (filename != "")
        {
            sTitle = filename.Replace("_0.log", "月份").Replace("_1.log", "月份");
            if (filename.Contains("_0.log"))
                BzText = ub.Get("SiteBz");
            else
                BzText = ub.Get("SiteBz2");
        }
        Master.Title = "查看" + sTitle + "" + BzText + "日志";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("查看" + sTitle + "" + BzText + "日志");
        builder.Append(Out.Tab("</div>", ""));

        int pageIndex;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string[] pageValUrl = { "act", "file", "path", "filename", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //逐行读取
        //string fileEncode;
        //FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        //StreamReader reader = new StreamReader(stream, Encoding.UTF8);
        //string line = string.Empty;
        //int recordCount = 0;// Convert.ToInt32(File.ReadAllLines(filePath).Length);
        //int stratIndex = (pageIndex - 1) * pageSize;
        //int endIndex = pageIndex * pageSize;
        //int k = 0;
        //while ((line = reader.ReadLine()) != null)
        //{
        //    if (k >= stratIndex && k < endIndex)
        //    {
        //        if ((k + 1) % 2 == 0)
        //            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        //        else
        //            builder.Append(Out.Tab("<div>", "<br />"));

        //        string[] getLine = line.Split("#".ToCharArray());
        //        builder.AppendFormat("{0}.{1}|操作{2}|结{3}({4})", (k + 1), getLine[1], getLine[2], getLine[3], DT.FormatDate(Convert.ToDateTime(getLine[0]), 1));

        //        builder.Append(Out.Tab("</div>", ""));
        //    }

        //    if (k == endIndex)
        //        break;

        //    k++;
        //}


        //// 分页
        ////builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));


        //builder.Append(Out.Tab("<div>", ""));

        //if (reader.ReadLine() != null)
        //    builder.Append("<a href=\"" + Utils.getUrl("txtpagedemo.aspx?page=" + (pageIndex + 1)) + "\">&gt;&gt;下页" + ((pageIndex != 1) ? "|" : "") + "</a>");

        //if (pageIndex != 1)
        //    builder.Append("<a href=\"" + Utils.getUrl("txtpagedemo.aspx?page=" + (pageIndex - 1)) + "\">&lt;&lt;上页</a>");

        //builder.Append(Out.Tab("</div>", ""));

        //reader.Close();
        //stream.Close();
        //watch.Stop();
        //builder.Append("执行用了" + watch.Elapsed + "<br />");

        //整体读取
  
        FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        StreamReader reader = new StreamReader(stream, Encoding.UTF8);
        string log = reader.ReadToEnd();
        if (!string.IsNullOrEmpty(log))
        {
            string[] sName = Regex.Split(log, "\r\n");
            //总记录数
            int recordCount = sName.Length-1;

            int stratIndex = (pageIndex - 1) * pageSize;
            int endIndex = pageIndex * pageSize;
            int k = 0;
            for (int i = recordCount; i > 0; i--)
            {
                if (k >= stratIndex && k < endIndex)
                {
                    if ((k + 1) % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));

                    string[] getLine = sName[i - 1].ToString().Split("#".ToCharArray());
                    builder.AppendFormat("{0}.{1}|操作{2}|结{3}({4})", ((pageIndex - 1) * pageSize + (k + 1)), getLine[1], getLine[2], getLine[3], DT.FormatDate(Convert.ToDateTime(getLine[0]), 0));


                    builder.Append(Out.Tab("</div>", ""));
                }
                if (k == endIndex)
                    break;
                k++;
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        reader.Close();
        stream.Close();
        watch.Stop();
        //builder.Append("执行用了" + watch.Elapsed + "<br />");


        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        if (folderPath.ToUpper().IndexOf("\\LOG\\GOLD\\") != -1)
        {
            builder.Append(Out.Tab("<div>", ""));
            string previousFolder = folderPath.Substring(0, folderPath.LastIndexOf("\\"));
            builder.Append("<a href=\"" + Utils.getUrl("" + Utils.getPageUrl() + "?path=" + Server.UrlEncode(previousFolder) + "") + "\">返回上一级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("" + Utils.getPageUrl() + "") + "\">返回主文件夹</a><br />");
            builder.Append(Out.Tab("</div>", ""));
        }

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
}