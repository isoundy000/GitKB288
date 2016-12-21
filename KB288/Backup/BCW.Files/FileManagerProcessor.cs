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
                    this._Value = "�ѳɹ�ѹ���ļ���, �ļ����Ѵ���, �Զ�������Ϊ: " + str2;
                }
                else
                {
                    zip.CreateZip(this._FolderPath + @"\" + Path.GetFileName(path) + ".zip", path, true, "");
                    this._Value = "�ѳɹ�ѹ���ļ���, �ļ���Ϊ: " + Path.GetFileName(path) + ".zip";
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
                this._Value = "�ļ������Ѵ���";
            }
            else
            {
                try
                {
                    info.Create();
                    this._Value = "�����ļ��гɹ�, �ļ�������Ϊ: " + str;
                }
                catch
                {
                    this._Value = "�����ļ���ʧ��, Ȩ�޲���";
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
                        this._Value = @"Ҫɾ�����ļ�������";
                        return;
                    }
                    try
                    {
                        File.Delete(path);
                        this._Value = @"ɾ���ļ��ɹ�,��ɾ�����ļ�Ϊ:" + Path.GetFileName(path);
                    }
                    catch
                    {
                        this._Value = @"ɾ���ļ�ʧ��, Ȩ�޲���";
                    }
                    break;

                case "folder":
                    if (Directory.Exists(path))
                    {
                        try
                        {
                            Directory.Delete(path, true);
                            this._Value = @"ɾ���ļ��гɹ�, ��ɾ�����ļ���Ϊ: " + Path.GetFileName(path);
                        }
                        catch
                        {
                            this._Value = "ɾ���ļ���ʧ��, Ȩ�޲���";
                        }
                    }
                    else
                    {
                        this._Value = @"Ҫɾ�����ļ��в�����";
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
                            str2 = "<a href=\"" + Utils.getUrl("filemanager.aspx?path=" + server.UrlEncode(info.FullName) + "") + "\"><img src=\"/Files/sys/IcoFolder.gif\" alt=\"�ļ���\" /> " + info.Name + "</a>";
                        else
                            str2 = "<a href=\"" + Utils.getUrl("filemanager.aspx?path=" + server.UrlEncode(info.FullName) + "") + "\"> " + info.Name + "</a>";

                        string compic = "";
                        if (Utils.Isie())
                            compic = "<img src=\"/Files/sys/IcoPackage.gif\" alt=\"ѹ��\" />";
                        else
                            compic = "[ѹ��]";

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
                        // str2 = "<a href=\"file.axd?file=" + server.UrlEncode(info2.FullName) + "\" target=\"_new\"><img src=\"/Files/sys/IcoOtherFile.gif\" alt=\"�ļ�\" /> " + info2.Name + "</a>";
                        if (Utils.Isie())
                            str2 = "<img src=\"/Files/sys/IcoOtherFile.gif\" alt=\"�ļ�\"/>" + info2.Name + "";
                        else
                            str2 = "" + info2.Name + "";
                        
                        if (info2.Extension.ToLower() == ".zip")
                        {
                            string str3 = str2;
                            if (Utils.Isie())
                                str2 = str3 + "<a href=\"" + Utils.getUrl("filemanager.aspx?act=unpack&amp;path=" + server.UrlEncode(this._FolderPath) + "&amp;objfile=" + server.UrlEncode(info2.FullName) + "") + "\"><img src=\"/Files/sys/IcoZip.gif\" alt=\"��ѹ\" /></a>";
                            else
                                str2 = str3 + "<a href=\"" + Utils.getUrl("filemanager.aspx?act=unpack&amp;path=" + server.UrlEncode(this._FolderPath) + "&amp;objfile=" + server.UrlEncode(info2.FullName) + "") + "\">[��ѹ]</a>";

                        }
                        else if ((info2.Length < 0xc350L)&& this.CheckExtHighlighter(info2.Extension.ToLower()))
                        {
                            if (Utils.Isie())
                                str2 = str2 + "<a href=\"" + Utils.getUrl("filemanager.aspx?info=text&amp;path=" + server.UrlEncode(this._FolderPath) + "&amp;objfile=" + server.UrlEncode(info2.FullName) + "") + "\"><img src=\"/Files/sys/IcoNotepad.gif\" alt=\"�༭\" /></a>";
                            else
                                str2 = str2 + "<a href=\"" + Utils.getUrl("filemanager.aspx?info=text&amp;path=" + server.UrlEncode(this._FolderPath) + "&amp;objfile=" + server.UrlEncode(info2.FullName) + "") + "\">[�༭]</a>";

                           // if (this.CheckExtHighlighter(info2.Extension.ToLower()))
                            //{
                                ////ȡ����·��
                                //string rootPath = server.MapPath("/");
                                //string FullName = info2.FullName.Remove(0, rootPath.Length);
                                //if (Utils.Isie())
                                //    str2 = str2 + "<a href=\"/" + FullName + "\"><img src=\"/Files/sys/IcoHighlighter.gif\" alt=\"����\" /></a>";
                                //else
                                //    str2 = str2 + "<a href=\"/" + FullName + "\">[����]</a>";

                            //}
                        }

                        //ȡ����·��
                        string rootPath = server.MapPath("/");
                        string FullName = info2.FullName.Remove(0, rootPath.Length);
                        if (Utils.Isie())
                            str2 = str2 + "<a href=\"/" + FullName + "\"><img src=\"/Files/sys/IcoHighlighter.gif\" alt=\"����\" /></a>";
                        else
                            str2 = str2 + "<a href=\"/" + FullName + "\">[����]</a>";

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
                        this._Value = @"�ļ��Ѵ���ʱ, �޷����������ƶ����ļ� ";
                    }
                    else
                    {
                        File.Move(path, Path.GetDirectoryName(path) + @"\" + str);
                        this._Value = "���������ƶ��ļ��ɹ�, �µ��ļ���Ϊ: " + str;
                    }
                }
                catch
                {
                    this._Value = @"���������ƶ��ļ�ʧ��, Ȩ�޲���";
                }
            }
            else
            {
                try
                {
                    if (Directory.Exists(Path.GetDirectoryName(path) + @"\" + str))
                    {
                        this._Value = @"�ļ����Ѵ���ʱ, �޷����������ƶ����ļ��� ";
                    }
                    else
                    {
                        Directory.Move(path, Path.GetDirectoryName(path) + @"\" + str);
                        this._Value = @"���������ƶ��ļ��гɹ�, �µ��ļ�����Ϊ: " + str;
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
                str = "�����ļ��ɹ�";
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
                    this._Value = @"��ѹ���, �ļ��������Ѵ���, �Զ�������Ϊ: " + Path.GetFileNameWithoutExtension(targetDirectory);
                }
                else
                {
                    zip.ExtractZip(path, this._FolderPath + @"\" + Path.GetFileNameWithoutExtension(path), "");
                    this._Value = @"��ѹ���, �����: " + Path.GetFileNameWithoutExtension(path);
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
                this._Value = @"����ѡ���ļ�";
            }
            else
            {
                string fileName = Path.GetFileName(file.FileName);
                //��ʽ����
                string fileExtension = Path.GetExtension(fileName).ToLower();
                string UpExt = ub.GetSub("UpbFileExt", "/Controls/upfile.xml");
                if (UpExt.IndexOf(fileExtension) == -1)
                {
                    this._Value = @"�������ϴ���" + fileExtension + "��ʽ�ļ�";
                }
               //�Ƿ��ϴ�
                else if (fileExtension == ".asp" || fileExtension == ".aspx" || fileExtension == ".jsp" || fileExtension == ".php" || fileExtension == ".asa" || fileExtension == ".cer" || fileExtension == ".cdx" || fileExtension == ".htr" || fileExtension == ".exe")
                {
                    this._Value = @"�������ϴ���" + fileExtension + "��ʽ�ļ�";
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
                            this._Value = @"�ϴ����ļ����Ѵ���, �Զ�������Ϊ: " + str2;
                        }
                        catch
                        {
                            this._Value = @"д���ļ�ʧ��, Ȩ�޲���";
                        }
                    }
                    else
                    {
                        try
                        {
                            file.SaveAs(HttpContext.Current.Server.UrlDecode(this._FolderPath) + @"\" + fileName);
                            this._Value = @"�ϴ��ļ����, �ļ���Ϊ: " + fileName;
                        }
                        catch
                        {
                            this._Value = @"д���ļ�ʧ��, Ȩ�޲���";
                        }
                    }
                }
            }
        }

        private void CcFile()
        {
            string fileUpload = Utils.GetRequest("fileUpload", "post", 2, @"^[\s\S]{10,200}$", "����ȷ�����ļ���ַ");
            string fileName = FileTool.GetFileExt(fileUpload);
            if (fileName == "")
            {
                this._Value = @"����ȷ�����ļ���ַ";
            }
            else
            {
                //��ʽ����
                string fileExtension = fileName.ToLower();
                fileName = DT.getDateTimeNum() + fileExtension + "";
                string UpExt = ub.GetSub("UpbFileExt", "/Controls/upfile.xml");
                if (UpExt.IndexOf(fileExtension) == -1)
                {
                    this._Value = @"������ɼ���" + fileExtension + "��ʽ�ļ�";
                }
                //�Ƿ��ϴ�
                else if (fileExtension == ".asp" || fileExtension == ".aspx" || fileExtension == ".jsp" || fileExtension == ".php" || fileExtension == ".asa" || fileExtension == ".cer" || fileExtension == ".cdx" || fileExtension == ".htr" || fileExtension == ".exe")
                {
                    this._Value = @"������ɼ���" + fileExtension + "��ʽ�ļ�";
                }
                else
                {
                    try
                    {
                        System.Net.WebClient down = new System.Net.WebClient();
                        down.DownloadFile(fileUpload, HttpContext.Current.Server.UrlDecode(this._FolderPath) + @"\" + fileName);
                        down.Dispose();
                        this._Value = @"�ɼ��ļ����, �ļ���Ϊ: " + fileName;
                    }
                    catch
                    {
                        this._Value = @"�ɼ��ļ�ʧ��, Զ���ļ������ڻ�Ȩ�޲���";

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
