using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using BCW.Update.Model;

namespace BCW.Update
{
    /// <summary>
    /// 抓取FTP数据
    /// </summary>
    public class UpdataFTP
    {
        private string _ResponseValue = string.Empty;
        private string _CacheFolder = "~/Files/Cache/";

        #region 属性

        private bool _CacheUsed = false; //是否记录缓存/存TXT
        private int _CacheTime = 5;//缓存时间(分钟)

        /// <summary>
        /// 是否使用文件型缓存
        /// </summary>
        public bool CacheUsed
        {
            set { _CacheUsed = value; }
        }

        /// <summary>
        /// 文件型缓存过期时间
        /// </summary>
        public int CacheTime
        {
            set { _CacheTime = value; }
        }

        #endregion 属性

        /// <summary>
        /// 构造方法
        /// </summary>
        public UpdataFTP()
        {

        }

        /// <summary>
        /// 取得FTP
        /// </summary>
        public BCW.Update.Model.UpdateInfo GetFtpXML(string GetUrl)
        {
            BCW.Update.Model.UpdateInfo obj = null;
            string url = GetUrl;
            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = "lightcms-ftp";

            httpRequest.WebAsync.RevCharset = "utf-8";
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                obj = FtpHtml(this._ResponseValue);

            return obj;
        }


        /// <summary>
        /// 处理详细版本号
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private BCW.Update.Model.UpdateInfo FtpHtml(string p_html)
        {
            if (string.IsNullOrEmpty(p_html))
                return null;

            BCW.Update.Model.UpdateInfo obj = new BCW.Update.Model.UpdateInfo();


            using (XmlReaderExtend reader = new XmlReaderExtend(p_html))
            {
                while (reader.ReadToFollowing("data"))
                {
                    obj.RemoteHost = reader.GetElementValue("RemoteHost");
                    obj.RemotePort = Convert.ToInt32(reader.GetElementValue("RemotePort"));
                    obj.RemoteUser = reader.GetElementValue("RemoteUser");
                    obj.RemotePass = reader.GetElementValue("RemotePass");
                    obj.RemotePath = reader.GetElementValue("RemotePath");
                }
            }
            return obj;
        }

    }
}
