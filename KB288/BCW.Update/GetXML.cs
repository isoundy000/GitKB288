using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using BCW.Update.Model;

namespace BCW.Update
{
    /// <summary>
    /// 抓取升级简要数据
    /// </summary>
    public class UpdateXML
    {
        private string _ResponseValue = string.Empty;
        private string _CacheFolder = "~/Files/Cache/update";

        #region 属性

        private bool _CacheUsed = false; //是否记录缓存/存TXT
        private int _CacheTime = 60;//缓存时间(分钟)

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
        public UpdateXML()
        {

        }

        /// <summary>
        /// 取得版本号
        /// </summary>
        public BCW.Update.Model.UpdateInfo GetVersionXML(string GetUrl)
        {
            BCW.Update.Model.UpdateInfo obj = null;
            string url = GetUrl;
            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = "lightcms-Version";

            httpRequest.WebAsync.RevCharset = "utf-8";
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                obj = VersionHtml(this._ResponseValue);

            return obj;
        }


        /// <summary>
        /// 取得更新简要
        /// </summary>
        public BCW.Update.Model.UpdateInfo GetUpdateXML(string GetUrl)
        {
            BCW.Update.Model.UpdateInfo obj = null;
            string url = GetUrl;
            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = "lightcms_" + GetUrl + "";

            httpRequest.WebAsync.RevCharset = "utf-8";
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                obj = UpdateHtml(this._ResponseValue);

            return obj;
        }


        /// <summary>
        /// 处理详细版本号
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private BCW.Update.Model.UpdateInfo VersionHtml(string p_html)
        {
            if (string.IsNullOrEmpty(p_html))
                return null;

            BCW.Update.Model.UpdateInfo obj = new BCW.Update.Model.UpdateInfo();


            using (XmlReaderExtend reader = new XmlReaderExtend(p_html))
            {
                while (reader.ReadToFollowing("data"))
                {
                    obj.Version = reader.GetElementValue("Version");
                    obj.SpDomain = reader.GetElementValue("SpDomain");
                    obj.FtpData = reader.GetElementValue("FtpData");
                }
            }
            return obj;
        }

        /// <summary>
        /// 处理详细更新简要
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private BCW.Update.Model.UpdateInfo UpdateHtml(string p_html)
        {
            if (string.IsNullOrEmpty(p_html))
                return null;

            BCW.Update.Model.UpdateInfo obj = new BCW.Update.Model.UpdateInfo();


            using (XmlReaderExtend reader = new XmlReaderExtend(p_html))
            {
                while (reader.ReadToFollowing("data"))
                {
                    obj.Version = reader.GetElementValue("Version");
                    obj.ToPath = reader.GetElementValue("ToPath");
                    obj.RePath = reader.GetElementValue("RePath");
                    obj.Paths = reader.GetElementValue("Paths");
                    obj.Notes = reader.GetElementValue("Notes");
                    obj.WithTime = reader.GetElementValue("WithTime");
                    obj.AddTime = reader.GetElementValue("AddTime");
                }
            }
            return obj;
        }

    }
}
