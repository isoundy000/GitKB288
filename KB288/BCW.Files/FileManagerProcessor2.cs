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

    public class FileManagerProcessor2
    {
        private bool _Access;
        private int _FileNum;
        private int _FolderNum;
        private string _FolderPath;

        public FileManagerProcessor2()
        {
            HttpRequest request = HttpContext.Current.Request;
            this._FolderPath = request["path"];
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

                        str2 = "<a href=\"" + Utils.getUrl("" + Utils.getPageUrl() + "?path=" + server.UrlEncode(info.FullName) + "") + "\"> " + info.Name + "</a>";

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

                        str2 = "" + info2.Name + "";
                        
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
    }
}
