using System;
using System.IO;

namespace BCW.Update
{
    /// <summary>
    /// ���� WebRequest ȡ�õ����Ͻ����ļ�����
    /// </summary>
    public class FileCache
    {
        #region ����

        private bool _CacheUsed = true;
        /// <summary>
        /// �Ƿ�ʹ���ļ��ͻ���
        /// </summary>
        public bool CacheUsed
        {
            get { return _CacheUsed; }
            set { _CacheUsed = value; }
        }

        private int _CacheTime = 30;
        /// <summary>
        /// �ļ��ͻ������ʱ��
        /// </summary>
        public int CacheTime
        {
            get { return _CacheTime; }
            set { _CacheTime = value; }
        }

        private string _CacheFolder;
        /// <summary>
        /// �ļ��ͻ��汣���Ŀ¼·��
        /// </summary>
        public string CacheFolder
        {
            get { return _CacheFolder; }
            set { _CacheFolder = value; }
        }

        private string _CacheFile;
        /// <summary>
        /// �ļ��ͻ��汣����ļ���
        /// </summary>
        public string CacheFile
        {
            get { return _CacheFile; }
            set { _CacheFile = value; }
        }

        private string _FileText = string.Empty;
        /// <summary>
        /// �ļ���������
        /// </summary>
        public string FileText
        {
            get { return _FileText; }
        }

        /// <summary>
        /// �ļ�����·��
        /// </summary>
        private string _FullName
        {
            get { return Path.Combine(System.Web.HttpContext.Current.Server.MapPath(this.CacheFolder), Path.GetFileNameWithoutExtension(this.CacheFile) + ".txt"); }
        }

        #endregion ����

        /// <summary>
        /// ���췽��
        /// </summary>
        public FileCache()
        {
        }

        /// <summary>
        /// ��ȡ�ļ�������ı�����
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
        /// ���ı�����д���ļ�����
        /// </summary>
        /// <param name="p_strVal">�ı�����</param>
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
        /// �������������ڵ��ļ�����
        /// </summary>
        /// Modified Date: 2007-09-30 22:06
        private void ClearFileCache()
        {
            // �����ļ�����Ŀ¼
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
