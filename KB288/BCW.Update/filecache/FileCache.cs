using System;
using System.IO;

namespace BCW.Update
{
    /// <summary>
    /// 用于 WebRequest 取得的资料进行文件缓存
    /// </summary>
    public class FileCache
    {
        #region 属性

        private bool _CacheUsed = true;
        /// <summary>
        /// 是否使用文件型缓存
        /// </summary>
        public bool CacheUsed
        {
            get { return _CacheUsed; }
            set { _CacheUsed = value; }
        }

        private int _CacheTime = 30;
        /// <summary>
        /// 文件型缓存过期时间
        /// </summary>
        public int CacheTime
        {
            get { return _CacheTime; }
            set { _CacheTime = value; }
        }

        private string _CacheFolder;
        /// <summary>
        /// 文件型缓存保存的目录路径
        /// </summary>
        public string CacheFolder
        {
            get { return _CacheFolder; }
            set { _CacheFolder = value; }
        }

        private string _CacheFile;
        /// <summary>
        /// 文件型缓存保存的文件名
        /// </summary>
        public string CacheFile
        {
            get { return _CacheFile; }
            set { _CacheFile = value; }
        }

        private string _FileText = string.Empty;
        /// <summary>
        /// 文件缓存内容
        /// </summary>
        public string FileText
        {
            get { return _FileText; }
        }

        /// <summary>
        /// 文件完整路径
        /// </summary>
        private string _FullName
        {
            get { return Path.Combine(System.Web.HttpContext.Current.Server.MapPath(this.CacheFolder), Path.GetFileNameWithoutExtension(this.CacheFile) + ".txt"); }
        }

        #endregion 属性

        /// <summary>
        /// 构造方法
        /// </summary>
        public FileCache()
        {
        }

        /// <summary>
        /// 读取文件缓存的文本内容
        /// </summary>
        /// Modified Date: 2007-09-30 22:03
        public bool ReadFileCache()
        {
            if (!this._CacheUsed)
                return false;

            ClearFileCache();

            bool bCache = false;
            try
            {
                FileInfo fi = new FileInfo(this._FullName);
                if (fi.Exists)
                {
                    TimeSpan tsFile = new TimeSpan(DateTime.Now.Ticks - fi.LastWriteTime.Ticks);
                    if (tsFile.TotalMinutes < this._CacheTime)
                    {
                        using (FileStream fs = new FileStream(this._FullName, FileMode.Open, FileAccess.Read))
                        {
                            using (StreamReader sr = new StreamReader(fs, System.Text.Encoding.UTF8))
                            {
                                this._FileText = sr.ReadToEnd();
                                bCache = true;
                            }
                        }
                    }
                }
            }
            catch
            {
                bCache = false;
            }

            return bCache;
        }

        /// <summary>
        /// 把文本内容写入文件缓存
        /// </summary>
        /// <param name="p_strVal">文本内容</param>
        public void WriteFileCache(string p_strVal)
        {
            if (!this._CacheUsed)
                return;

            if(string.IsNullOrEmpty(p_strVal))
                return;

            ClearFileCache();

            using (FileStream fs = new FileStream(this._FullName, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8))
                {
                    sw.Write(p_strVal);
                    sw.Flush();
                }
            }
        }

        /// <summary>
        /// 随机触发清理过期的文件缓存
        /// </summary>
        /// Modified Date: 2007-09-30 22:06
        private void ClearFileCache()
        {
            // 创建文件缓存目录
            if (!Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(this._CacheFolder)))
                Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(this._CacheFolder));

            if (new Random().Next(1, 13) % 9 == 0)
            {
                DirectoryInfo dir = new DirectoryInfo(System.Web.HttpContext.Current.Server.MapPath(this._CacheFolder));

                foreach (FileSystemInfo fsi in dir.GetFileSystemInfos())
                {
                    if (fsi is FileInfo)
                    {
                        TimeSpan tsFile = new TimeSpan(DateTime.Now.Ticks - fsi.LastWriteTime.Ticks);
                        if (tsFile.TotalMinutes > this._CacheTime)
                        {
                            try { fsi.Delete(); }
                            catch { }
                        }
                    }
                }
            }
        }
    }
}
