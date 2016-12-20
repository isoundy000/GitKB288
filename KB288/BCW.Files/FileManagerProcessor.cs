namespace BCW.Files
{
    using BCW.Files.Model;
    using ICSharpCode.SharpZipLib;
    using ICSharpCode.SharpZipLib.Zip;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Web;
    using BCW.Common;

    public class FileManagerProcessor
    {
        private bool _Access;
        private int _FileNum;
        private int _FolderNum;
        private string _FolderPath;
        private string _Value;

        public FileManagerProcessor()
        {
            HttpRequest request = HttpContext.Current.Request;
            this._FolderPath = request["path"];
        }

        public FileManagerProcessor(string p_act)
        {
            HttpRequest request = HttpContext.Current.Request;
            this._FolderPath = request["path"];
            string str = p_act;
            if (str != null)
            {
                if (!(str == "create"))
                {
                    if (!(str == "rename"))
                    {
                        if (!(str == "delete"))
                        {
                            if (!(str == "upload"))
                            {
                                if (!(str == "ccfile"))
                                {
                                    if (!(str == "compress"))
                                    {
                                        if (str == "unpack")
                                        {
                                            this.UnpackFile();
                                        }
                                        return;
                                    }
                                    this.CompressFolder();
                                    return;
                                }
                                this.CcFile();
                                return;
                            }
                            this.UploadFile();
                            return;
                        }
                        this.DeleteFileFolder();
                        return;
                    }
                }
                else
                {
                    this.CreateFolder();
                    return;
                }
                this.RenameFileFolder();
            }
        }

        private bool CheckExtEdit(string p_ext)
        {
            string str = ".ini|.txt|.log|.inc|.config|.xml|.htm|.html|.wml|";
            return (str.IndexOf(p_ext + "|") != -1);
        }

        private bool CheckExtHighlighter(string p_ext)
        {
            string str = ".c|.cpp|.cs|.css|.java|.jsp|.js|.php|.sql|.vb|.xml|.htm|.html|.aspx|.wml|.txt|";
            return (str.IndexOf(p_ext + "|") != -1);
        }

        private void CompressFolder()
        {
            string path = HttpContext.Current.Request.QueryString["objfolder"];
            FastZip zip = new FastZip();
            zip.CreateEmptyDirectories = true;
            try
            {
                if (File.Exists(this._FolderPath + @"\" + Path.GetFileName(path) + ".zip"))
                {
                    Random random = new Random();
                    string str2 = string.Concat(new object[] { Path.GetFileNameWithoutExtension(path), "_", random.Next(1, 0x3e8), ".zip" });
                    zip.CreateZip(this._FolderPath + @"\" + str2, path, true, "");
                    this._Value = "已成功压缩文件夹, 文件名已存在, 自动重命名为: " + str2;
                }
                else
                {
                    zip.CreateZip(this._FolderPath + @"\" + Path.GetFileName(path) + ".zip", path, true, "");
                    this._Value = "已成功压缩文件夹, 文件名为: " + Path.GetFileName(path) + ".zip";
                }
            }
            catch (Exception exception)
            {
                this._Value = exception.Message;
            }
        }

        private void CreateFolder()
        {
            string str = HttpContext.Current.Request.Form["txtFolderName"];
            DirectoryInfo info = new DirectoryInfo(HttpContext.Current.Server.UrlDecode(this._FolderPath) + @"\" + str);
            if (info.Exists)
            {
                this._Value = "文件夹名已存在";
            }
            else
            {
                try
                {
                    info.Create();
                    this._Value = "创建文件夹成功, 文件夹名称为: " + str;
                }
                catch
                {
                    this._Value = "创建文件夹失败, 权限不足";
                }
            }
        }

        private void DeleteFileFolder()
        {
            HttpRequest request = HttpContext.Current.Request;
            string path = request.QueryString["file"];
            switch (request.QueryString["type"])
            {
                case "file":
                    if (!File.Exists(path))
                    {
                        this._Value = @"要删除的文件不存在";
                        return;
                    }
                    try
                    {
                        File.Delete(path);
                        this._Value = @"删除文件成功,被删除的文件为:" + Path.GetFileName(path);
                    }
                    catch
                    {
                        this._Value = @"删除文件失败, 权限不足";
                    }
                    break;

                case "folder":
                    if (Directory.Exists(path))
                    {
                        try
                        {
                            Directory.Delete(path, true);
                            this._Value = @"删除文件夹成功, 被删除的文件夹为: " + Path.GetFileName(path);
                        }
                        catch
                        {
                            this._Value = "删除文件夹失败, 权限不足";
                        }
                    }
                    else
                    {
                        this._Value = @"要删除的文件夹不存在";
                    }
                    break;
            }
        }

        public List<FileFolderInfo> GetDirectories(string p_folderPath)
        {
            HttpServerUtility server = HttpContext.Current.Server;
            string sortExpression = HttpContext.Current.Request.QueryString["order"];
            List<FileFolderInfo> list = new List<FileFolderInfo>();
            DirectoryInfo info3 = new DirectoryInfo(p_folderPath);
            try
            {
                foreach (FileSystemInfo info4 in info3.GetFileSystemInfos())
                {
                    string str2;
                    if (string.IsNullOrEmpty(this._FolderPath))
                        this._FolderPath = HttpContext.Current.Request.PhysicalApplicationPath + "Files";

                    if (info4 is DirectoryInfo)
                    {
                        DirectoryInfo info = info4 as DirectoryInfo;
                        if (Utils.Isie())
                            str2 = "<a href=\"" + Utils.getUrl("filemanager.aspx?path=" + server.UrlEncode(info.FullName) + "") + "\"><img src=\"/Files/sys/IcoFolder.gif\" alt=\"文件夹\" /> " + info.Name + "</a>";
                        else
                            str2 = "<a href=\"" + Utils.getUrl("filemanager.aspx?path=" + server.UrlEncode(info.FullName) + "") + "\"> " + info.Name + "</a>";

                        string compic = "";
                        if (Utils.Isie())
                            compic = "<img src=\"/Files/sys/IcoPackage.gif\" alt=\"压缩\" />";
                        else
                            compic = "[压缩]";

                        str2 += " <a href=\"" + Utils.getUrl("filemanager.aspx?act=compress&amp;path=" + server.UrlEncode(this._FolderPath) + "&amp;objfolder=" + server.UrlEncode(info.FullName) + "") + "\">" + compic + "</a>";

                        FileFolderInfo info5 = new FileFolderInfo();
                        info5.Name = info.Name;
                        info5.FullName = server.UrlEncode(info.FullName);
                        info5.FormatName = str2;
                        info5.Ext = "";
                        info5.Size = "0";
                        info5.Type = "folder";
                        info5.ModifyDate = info.LastWriteTime;
                        list.Add(info5);
                        this._FolderNum++;
                    }
                    else
                    {
                        FileInfo info2 = info4 as FileInfo;
                        // str2 = "<a href=\"file.axd?file=" + server.UrlEncode(info2.FullName) + "\" target=\"_new\"><img src=\"/Files/sys/IcoOtherFile.gif\" alt=\"文件\" /> " + info2.Name + "</a>";
                        if (Utils.Isie())
                            str2 = "<img src=\"/Files/sys/IcoOtherFile.gif\" alt=\"文件\"/>" + info2.Name + "";
                        else
                            str2 = "" + info2.Name + "";
                        
                        if (info2.Extension.ToLower() == ".zip")
                        {
                            string str3 = str2;
                            if (Utils.Isie())
                                str2 = str3 + "<a href=\"" + Utils.getUrl("filemanager.aspx?act=unpack&amp;path=" + server.UrlEncode(this._FolderPath) + "&amp;objfile=" + server.UrlEncode(info2.FullName) + "") + "\"><img src=\"/Files/sys/IcoZip.gif\" alt=\"解压\" /></a>";
                            else
                                str2 = str3 + "<a href=\"" + Utils.getUrl("filemanager.aspx?act=unpack&amp;path=" + server.UrlEncode(this._FolderPath) + "&amp;objfile=" + server.UrlEncode(info2.FullName) + "") + "\">[解压]</a>";

                        }
                        else if ((info2.Length < 0xc350L)&& this.CheckExtHighlighter(info2.Extension.ToLower()))
                        {
                            if (Utils.Isie())
                                str2 = str2 + "<a href=\"" + Utils.getUrl("filemanager.aspx?info=text&amp;path=" + server.UrlEncode(this._FolderPath) + "&amp;objfile=" + server.UrlEncode(info2.FullName) + "") + "\"><img src=\"/Files/sys/IcoNotepad.gif\" alt=\"编辑\" /></a>";
                            else
                                str2 = str2 + "<a href=\"" + Utils.getUrl("filemanager.aspx?info=text&amp;path=" + server.UrlEncode(this._FolderPath) + "&amp;objfile=" + server.UrlEncode(info2.FullName) + "") + "\">[编辑]</a>";

                           // if (this.CheckExtHighlighter(info2.Extension.ToLower()))
                            //{
                                ////取虚拟路径
                                //string rootPath = server.MapPath("/");
                                //string FullName = info2.FullName.Remove(0, rootPath.Length);
                                //if (Utils.Isie())
                                //    str2 = str2 + "<a href=\"/" + FullName + "\"><img src=\"/Files/sys/IcoHighlighter.gif\" alt=\"访问\" /></a>";
                                //else
                                //    str2 = str2 + "<a href=\"/" + FullName + "\">[访问]</a>";

                            //}
                        }

                        //取虚拟路径
                        string rootPath = server.MapPath("/");
                        string FullName = info2.FullName.Remove(0, rootPath.Length);
                        if (Utils.Isie())
                            str2 = str2 + "<a href=\"/" + FullName + "\"><img src=\"/Files/sys/IcoHighlighter.gif\" alt=\"访问\" /></a>";
                        else
                            str2 = str2 + "<a href=\"/" + FullName + "\">[访问]</a>";

                        FileFolderInfo info6 = new FileFolderInfo();
                        info6.Name = info2.Name;
                        info6.FullName = server.UrlEncode(info2.FullName);
                        info6.FormatName = str2;
                        info6.Ext = info2.Extension;
                        info6.Size = info2.Length.ToString();
                        info6.Type = "file";
                        info6.ModifyDate = info2.LastWriteTime;
                        list.Add(info6);
                        this._FileNum++;
                    }
                }
                this._Access = true;
            }
            catch
            {
                this._Access = false;
                return list;
            }
            if (!string.IsNullOrEmpty(sortExpression))
            {
                list.Sort(new FilesComparer(sortExpression));
            }
            return list;
        }

        private Encoding GetEncoding(FileStream p_fs, out string p_fileEncode)
        {
            Encoding unicode = Encoding.Default;
            p_fileEncode = "ANSI";
            if ((p_fs != null) && (p_fs.Length >= 1L))
            {
                string str = "";
                string str2 = "";
                string str3 = "";
                p_fs.Seek(0L, SeekOrigin.Begin);
                str = Convert.ToByte(p_fs.ReadByte()).ToString("X");
                if (p_fs.Length >= 2L)
                {
                    str2 = Convert.ToByte(p_fs.ReadByte()).ToString("X");
                }
                if (p_fs.Length >= 3L)
                {
                    str3 = Convert.ToByte(p_fs.ReadByte()).ToString("X");
                }
                if (((str == "EF") && (str2 == "BB")) && (str3 == "BF"))
                {
                    unicode = Encoding.UTF8;
                    p_fileEncode = "UTF-8";
                }
                else if ((str == "FF") && (str2 == "FE"))
                {
                    unicode = Encoding.Unicode;
                    p_fileEncode = "Unicode";
                }
                else if ((str == "FE") && (str2 == "FF"))
                {
                    unicode = Encoding.BigEndianUnicode;
                    p_fileEncode = "Unicode big endian";
                }
                p_fs.Seek(0L, SeekOrigin.Begin);
            }
            return unicode;
        }

        public void ReadTextFile(string p_filePath, out string p_fileContent, out string p_fileEncode)
        {
            FileStream stream = new FileStream(p_filePath, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(stream, this.GetEncoding(stream, out p_fileEncode));
            p_fileContent = reader.ReadToEnd();
            reader.Close();
            stream.Close();
        }

        private void RenameFileFolder()
        {
            HttpRequest request = HttpContext.Current.Request;
            HttpServerUtility server = HttpContext.Current.Server;
            string str = server.UrlDecode(request.Form["txtFolderName"]);
            string path = server.UrlDecode(request.Form["txtOldName"]);

            if (File.Exists(path))
            {
                try
                {
                    if (File.Exists(Path.GetDirectoryName(path) + @"\" + str))
                    {
                        this._Value = @"文件已存在时, 无法重命名或移动该文件 ";
                    }
                    else
                    {
                        File.Move(path, Path.GetDirectoryName(path) + @"\" + str);
                        this._Value = "重命名或移动文件成功, 新的文件名为: " + str;
                    }
                }
                catch
                {
                    this._Value = @"重命名或移动文件失败, 权限不足";
                }
            }
            else
            {
                try
                {
                    if (Directory.Exists(Path.GetDirectoryName(path) + @"\" + str))
                    {
                        this._Value = @"文件夹已存在时, 无法重命名或移动该文件夹 ";
                    }
                    else
                    {
                        Directory.Move(path, Path.GetDirectoryName(path) + @"\" + str);
                        this._Value = @"重命名或移动文件夹成功, 新的文件夹名为: " + str;
                    }
                }
                catch (Exception exception)
                {
                    this._Value = exception.Message;
                }
            }
        }

        public string SaveTextFile(string p_filePath, string p_fileContent, string p_fileEncode)
        {
            string str;
            Encoding unicode = Encoding.Default;
            if (p_fileEncode == "UTF-8")
            {
                unicode = Encoding.UTF8;
            }
            else if (p_fileEncode == "Unicode")
            {
                unicode = Encoding.Unicode;
            }
            else if (p_fileEncode == "Unicode big endian")
            {
                unicode = Encoding.BigEndianUnicode;
            }
            FileStream stream = new FileStream(p_filePath, FileMode.Create, FileAccess.Write);
            StreamWriter writer = new StreamWriter(stream, unicode);
            try
            {
                writer.Write(p_fileContent);
                writer.Flush();
                str = "保存文件成功";
            }
            catch (Exception exception)
            {
                str = exception.Message;
            }
            finally
            {
                writer.Close();
                stream.Close();
            }
            return str;
        }

        private void UnpackFile()
        {
            string path = HttpContext.Current.Request.QueryString["objfile"];
            FastZip zip = new FastZip();
            try
            {
                if (Directory.Exists(this._FolderPath + @"\" + Path.GetFileNameWithoutExtension(path)))
                {
                    Random random = new Random();
                    string targetDirectory = string.Concat(new object[] { this._FolderPath, @"\", Path.GetFileNameWithoutExtension(path), "_", random.Next(1, 0x3e8) });
                    zip.ExtractZip(path, targetDirectory, "");
                    this._Value = @"解压完毕, 文件夹名称已存在, 自动重命名为: " + Path.GetFileNameWithoutExtension(targetDirectory);
                }
                else
                {
                    zip.ExtractZip(path, this._FolderPath + @"\" + Path.GetFileNameWithoutExtension(path), "");
                    this._Value = @"解压完毕, 存放在: " + Path.GetFileNameWithoutExtension(path);
                }
            }
            catch (Exception exception)
            {
                this._Value = exception.Message;
            }
        }

        private void UploadFile()
        {
            HttpPostedFile file = HttpContext.Current.Request.Files["fileUpload"];
            if (file.ContentLength == 0)
            {
                this._Value = @"请先选择文件";
            }
            else
            {
                string fileName = Path.GetFileName(file.FileName);
                //格式限制
                string fileExtension = Path.GetExtension(fileName).ToLower();
                string UpExt = ub.GetSub("UpbFileExt", "/Controls/upfile.xml");
                if (UpExt.IndexOf(fileExtension) == -1)
                {
                    this._Value = @"不允许上传此" + fileExtension + "格式文件";
                }
               //非法上传
                else if (fileExtension == ".asp" || fileExtension == ".aspx" || fileExtension == ".jsp" || fileExtension == ".php" || fileExtension == ".asa" || fileExtension == ".cer" || fileExtension == ".cdx" || fileExtension == ".htr" || fileExtension == ".exe")
                {
                    this._Value = @"不允许上传此" + fileExtension + "格式文件";
                }
                else
                {
                    if (File.Exists(this._FolderPath + @"\" + fileName))
                    {
                        Random random = new Random();
                        string str2 = string.Concat(new object[] { Path.GetFileNameWithoutExtension(fileName), "_", random.Next(1, 0x3e8), Path.GetExtension(fileName) });
                        try
                        {
                            file.SaveAs(HttpContext.Current.Server.UrlDecode(this._FolderPath) + @"\" + str2);
                            this._Value = @"上传的文件名已存在, 自动重命名为: " + str2;
                        }
                        catch
                        {
                            this._Value = @"写入文件失败, 权限不足";
                        }
                    }
                    else
                    {
                        try
                        {
                            file.SaveAs(HttpContext.Current.Server.UrlDecode(this._FolderPath) + @"\" + fileName);
                            this._Value = @"上传文件完毕, 文件名为: " + fileName;
                        }
                        catch
                        {
                            this._Value = @"写入文件失败, 权限不足";
                        }
                    }
                }
            }
        }

        private void CcFile()
        {
            string fileUpload = Utils.GetRequest("fileUpload", "post", 2, @"^[\s\S]{10,200}$", "请正确输入文件地址");
            string fileName = FileTool.GetFileExt(fileUpload);
            if (fileName == "")
            {
                this._Value = @"请正确输入文件地址";
            }
            else
            {
                //格式限制
                string fileExtension = fileName.ToLower();
                fileName = DT.getDateTimeNum() + fileExtension + "";
                string UpExt = ub.GetSub("UpbFileExt", "/Controls/upfile.xml");
                if (UpExt.IndexOf(fileExtension) == -1)
                {
                    this._Value = @"不允许采集此" + fileExtension + "格式文件";
                }
                //非法上传
                else if (fileExtension == ".asp" || fileExtension == ".aspx" || fileExtension == ".jsp" || fileExtension == ".php" || fileExtension == ".asa" || fileExtension == ".cer" || fileExtension == ".cdx" || fileExtension == ".htr" || fileExtension == ".exe")
                {
                    this._Value = @"不允许采集此" + fileExtension + "格式文件";
                }
                else
                {
                    try
                    {
                        System.Net.WebClient down = new System.Net.WebClient();
                        down.DownloadFile(fileUpload, HttpContext.Current.Server.UrlDecode(this._FolderPath) + @"\" + fileName);
                        down.Dispose();
                        this._Value = @"采集文件完毕, 文件名为: " + fileName;
                    }
                    catch
                    {
                        this._Value = @"采集文件失败, 远程文件不存在或权限不足";

                    }
                }
            }
        }

        public bool Access
        {
            get
            {
                return this._Access;
            }
        }

        public int FileNum
        {
            get
            {
                return this._FileNum;
            }
        }

        public int FolderNum
        {
            get
            {
                return this._FolderNum;
            }
        }

        public string Value
        {
            get
            {
                return this._Value;
            }
        }
    }
}
