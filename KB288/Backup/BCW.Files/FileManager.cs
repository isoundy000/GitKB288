using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using System.Net;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using BCW.Common;
namespace BCW.Files
{
    public class FileTool
    {
        //全部常用格式".swf,.apk,.sis,.exe,.sisx,.jad,.jar,.mpkg,.pgk,.mis,.rarz,.txt,.mtf,.cab,.tsk,.hme,.zip,.rar,.aif,.app,.rsc,.pak,.dll,.sav,.db,.gz,.dskin,.thm,.utz,.sdt,.nth,.mbm,.mp3,.mp4,.mid,.mmf,.midi,.amr,.ogg,.rm,.au,.acc,.imy,.wav,.wmv,.wma,.seq,.m4a,.aac,.ape,.flac,.3gp,.3gpp,.avi,.mov,.rmvb,.3gp2,.mms,.jpg,.gif,.jpeg,.png,.bmp,.lrc,.umd,.pdf,.html,.wml,.chm,.htm,.doc,.wps,.ppt";

        //图片格式
        protected static string picExt = ".jpg,.gif,.jpeg,.png,.bmp";
        //音乐格式
        protected static string musicExt = ".mp3,.mid,.midi,.acc,.m4a,.wav,.amr,.seq,.ape,.imy,.mms";
        //视频格式
        protected static string videoExt = ".swf,.mp4,.rm,.wmv,.wma,.flac,.3gp,.3gpp,.avi,.mov,.rmvb,.3gp2";
        //其它格式略


        /// <summary>
        /// 得到文件组类型
        /// </summary>
        /// <param name="FileExt">文件格式</param>
        /// <returns></returns>
        public static int GetExtType(string FileExt)
        { 
            int Retn=0;
            if (picExt.Contains(FileExt))
                Retn = 1;
            else if (musicExt.Contains(FileExt))
                Retn = 2;
            else if (videoExt.Contains(FileExt))
                Retn = 3;
            else
                Retn = 4;

            return Retn;
        }

        /// <summary>
        /// 创建年-月目录
        /// </summary>
        /// <param name="DirPath">初始目录</param>
        /// <param name="sPath">输出路径</param>
        /// <returns>Out sPath</returns>
        public static Boolean CreateDirectory(string DirPath, out string sPath)
        {
            string Path = DirPath;
            int iMonth = DateTime.Now.Month;
            if (iMonth < 10)
                Path += (DateTime.Now.Year + "/0" + DateTime.Now.Month).ToString() + "/";
            else
                Path += (DateTime.Now.Year + "/" + DateTime.Now.Month).ToString() + "/";
            try
            {
                if (System.IO.Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(Path)) == false)//如果不存在就创建文件夹
                {
                    System.IO.Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(Path));
                }
                sPath = Path;
                return true;
            }
            catch
            {
                sPath = "";
                return false;
            }
        }

        /// <summary>
        /// 文件是否存在
        /// </summary>
        /// <param name="PathName"></param>
        /// <returns></returns>
        public static bool IsFileExists(string PathName)
        {
            if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(PathName)) == true)//如果文件存在
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 计算文件大小
        /// </summary>
        /// <param name="ContentLength"></param>
        /// <returns></returns>
        public static string GetContentLength(Int64 ContentLength)
        {
            if (ContentLength < 1024)
                return ContentLength + "B";
            else if (ContentLength < 1024 * 1024)
                return (Convert.ToInt64(ContentLength / 1024.00)).ToString() + "K";
            else
                return (Convert.ToInt64(ContentLength / (1024 * 1024.00))).ToString() + "M";
        }

        /// <summary>
        /// 比较两张图片是否相同
        /// </summary>
        public static bool ImageEquals(Bitmap bmpOne, Bitmap bmpTwo)
        {

            for (int i = 0; i < bmpOne.Width; i++)
            {
                for (int j = 0; j < bmpOne.Height; j++)
                {
                    if (bmpOne.GetPixel(i, j) != bmpTwo.GetPixel(i, j))
                        return false;
                }
            }
            return true;
        }


        /// <summary>
        /// 删除文件夹以及文件夹中的子目录，文件
        /// </summary>
        /// <param name="DirPath"></param>
        /// <returns></returns>
        public static Boolean DeleteDirectory(string DirPath)
        {
            try
            {
                System.IO.Directory.Delete(System.Web.HttpContext.Current.Server.MapPath(DirPath), true);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static Boolean DeleteFile(string FilePath)
        {
            try
            {
                System.IO.File.Delete(System.Web.HttpContext.Current.Server.MapPath(FilePath));
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 删除[IMG]图片地址[/IMG]之间的图片文件
        /// </summary>
        /// <param name="IMGFiles"></param>
        public static void DeleteIMGFiles(string IMGFiles)
        {
            if (!string.IsNullOrEmpty(IMGFiles))
            {
                MatchCollection mc = Regex.Matches(IMGFiles, @"(\[IMG\])(.[^\[]*)(\[\/IMG\])", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                if (mc.Count > 0)
                {
                    for (int i = 0; i < mc.Count; i++)
                    {
                        DeleteFile(mc[i].Groups[2].Value.ToString());
                        //删除图片系统的原图
                        if (mc[i].Groups[2].Value.ToString().Contains("prev"))
                        {
                            DeleteFile(mc[i].Groups[2].Value.ToString().Replace("prev", "act"));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 删除[URL=文件地址][/URL]之间的图片文件
        /// </summary>
        /// <param name="URLFiles"></param>
        public static void DeleteURLFiles(string URLFiles)
        {
            if (!string.IsNullOrEmpty(URLFiles))
            {
                MatchCollection mc = Regex.Matches(URLFiles, @"(\[URL=(.[^\]]*)\])(.[^\[]*)(\[\/URL\])", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                if (mc.Count > 0)
                {
                    for (int i = 0; i < mc.Count; i++)
                    {
                        DeleteFile(mc[i].Groups[2].Value.ToString());
                    }
                }

            }
        }

        /// <summary>
        /// 得到文件名
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static string GetFileName(string FilePath)
        {
            if (string.IsNullOrEmpty(FilePath))
                return string.Empty;
            try
            {
                return FilePath.Substring(FilePath.LastIndexOf("/"), FilePath.Length - FilePath.LastIndexOf("/")).Replace("/", "");
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 得到文件扩展名
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static string GetFileExt(string FilePath)
        {
            if (string.IsNullOrEmpty(FilePath))
                return string.Empty;
            try
            {
                return FilePath.Substring(FilePath.LastIndexOf("."), FilePath.Length - FilePath.LastIndexOf("."));
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 得到文件大小
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static string GetFileContentLength(string FilePath)
        {
            if (string.IsNullOrEmpty(FilePath))
                return string.Empty;
            string fileSize = string.Empty;
            string Path = string.Empty;
            if (FilePath.IndexOf("http://") == -1)
                Path = System.Web.HttpContext.Current.Server.MapPath(FilePath);
            else
                Path = FilePath;
            try
            {
                System.Net.WebRequest req = WebRequest.Create(Path);
                System.Net.WebResponse rep = req.GetResponse();
                fileSize = GetContentLength(Convert.ToInt64(rep.ContentLength));
                rep.Close();
            }
            catch { }

            return fileSize;
        }

        /// <summary>
        /// 得到文件大小
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static long GetFileLength(string FilePath)
        {
            if (string.IsNullOrEmpty(FilePath))
                return 0;

            long fileSize = 0;
            string Path = string.Empty;
            if (FilePath.IndexOf("http://") == -1)
                Path = System.Web.HttpContext.Current.Server.MapPath(FilePath);
            else
                Path = FilePath;
            try
            {
                FileInfo fl = new FileInfo(Path);
                fileSize = fl.Length;
            }
            catch { }
            return fileSize;
        }

        /// <summary>
        /// 得到文件夹大小
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static long GetPathLength(string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            long length = 0;
            foreach (FileSystemInfo fsi in directoryInfo.GetFileSystemInfos())
            {
                if (fsi is FileInfo)
                {
                    length += ((FileInfo)fsi).Length;
                }
                else
                {
                    length += GetPathLength(fsi.FullName);
                }
            }
            return length;
        }


        /// <summary>
        /// 保存远程文件到本地(如果是图片,动态不变)
        /// </summary>
        /// <param name="FilePath">远程文件URL</param>
        /// <param name="Path">保存的本地路径</param>
        /// <returns></returns>
        public static string DownloadFile(string Path, int iFiles, string FilePath)
        {
            if (string.IsNullOrEmpty(FilePath))
                return string.Empty;
            string Return = string.Empty;
            string fileName = string.Empty;
            string fileExt = string.Empty;
            string DirPath = string.Empty;
            fileExt = GetFileExt(FilePath);
            //生成随机文件名
            fileName = DT.getDateTimeNum()  + iFiles + fileExt.ToLower();
            if (CreateDirectory(Path, out DirPath))
            {
                try
                {
                    System.Net.WebClient down = new System.Net.WebClient();
                    down.DownloadFile(FilePath, System.Web.HttpContext.Current.Server.MapPath(DirPath) + fileName);
                    down.Dispose();
                    Return = DirPath + fileName;
                }
                catch
                {
                    Return = FilePath;
                }
            }
            return Return;
        }

        public static string DownloadFile(string SavePath, string FilePath)
        {
            if (string.IsNullOrEmpty(FilePath))
                return string.Empty;
            string Return = string.Empty;

            try
            {
                System.Net.WebClient down = new System.Net.WebClient();
                down.DownloadFile(FilePath, System.Web.HttpContext.Current.Server.MapPath(SavePath));
                down.Dispose();
                Return = SavePath;
            }
            catch{ }
            return Return;
        }

        /// <summary>
        /// 保存远程图片(图片保存成静态)
        /// </summary>
        /// <param name="ImgPath">远程图片地址</param>
        /// <param name="Path">保存的本地路径</param>
        /// <returns></returns>
        public static string SaveRemoteImg(string Path, int iFiles, string ImgPath)
        {
            if (string.IsNullOrEmpty(ImgPath))
                return string.Empty;
            string Return = string.Empty;
            string imgName = string.Empty;
            string imgExt = string.Empty;
            string DirPath = string.Empty;
            imgExt = GetFileExt(ImgPath);
            //生成随机文件名
            imgName = DT.getDateTimeNum()  + iFiles + imgExt.ToLower();
            if (CreateDirectory(Path, out DirPath))
            {
                try
                {
                    WebRequest wreq = WebRequest.Create(ImgPath);
                    wreq.Timeout = 10000;
                    HttpWebResponse wresp = (HttpWebResponse)wreq.GetResponse();
                    Stream s = wresp.GetResponseStream();
                    System.Drawing.Image img;
                    img = System.Drawing.Image.FromStream(s);
                    switch (imgExt.ToLower())
                    {
                        case ".gif":
                            img.Save(System.Web.HttpContext.Current.Server.MapPath(DirPath) + imgName, ImageFormat.Gif);
                            break;
                        case ".jpg":
                        case ".jpeg":
                            img.Save(System.Web.HttpContext.Current.Server.MapPath(DirPath) + imgName, ImageFormat.Jpeg);
                            break;
                        case ".png":
                            img.Save(System.Web.HttpContext.Current.Server.MapPath(DirPath) + imgName, ImageFormat.Png);
                            break;
                        case ".icon":
                            img.Save(System.Web.HttpContext.Current.Server.MapPath(DirPath) + imgName, ImageFormat.Icon);
                            break;
                        case ".bmp":
                            img.Save(System.Web.HttpContext.Current.Server.MapPath(DirPath) + imgName, ImageFormat.Bmp);
                            break;
                    }
                    img.Dispose();
                    s.Dispose();
                    Return = DirPath + imgName;
                }
                catch
                {
                    Return = ImgPath;
                }
            }
            return Return;
        }

        public static string SaveRemoteImg(string SavePath, string ImgPath)
        {
            if (string.IsNullOrEmpty(ImgPath))
                return string.Empty;
            string Return = string.Empty;
            string imgExt = string.Empty;
            imgExt = GetFileExt(ImgPath);
            try
            {
                WebRequest wreq = WebRequest.Create(ImgPath);
                wreq.Timeout = 10000;
                HttpWebResponse wresp = (HttpWebResponse)wreq.GetResponse();
                Stream s = wresp.GetResponseStream();
                System.Drawing.Image img;
                img = System.Drawing.Image.FromStream(s);
                switch (imgExt.ToLower())
                {
                    case ".gif":
                        img.Save(System.Web.HttpContext.Current.Server.MapPath(SavePath), ImageFormat.Gif);
                        break;
                    case ".jpg":
                    case ".jpeg":
                        img.Save(System.Web.HttpContext.Current.Server.MapPath(SavePath), ImageFormat.Jpeg);
                        break;
                    case ".png":
                        img.Save(System.Web.HttpContext.Current.Server.MapPath(SavePath), ImageFormat.Png);
                        break;
                    case ".icon":
                        img.Save(System.Web.HttpContext.Current.Server.MapPath(SavePath), ImageFormat.Icon);
                        break;
                    case ".bmp":
                        img.Save(System.Web.HttpContext.Current.Server.MapPath(SavePath), ImageFormat.Bmp);
                        break;
                }
                img.Dispose();
                s.Dispose();
                Return = SavePath;
            }
            catch{ }
            return Return;
        }

    }
}