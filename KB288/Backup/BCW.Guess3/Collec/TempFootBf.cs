using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using BCW.Common;
using TPR3.Model;

namespace TPR3.Collec
{
    /// <summary>
    /// 赛程赛果 （建议更新频率：半天，可能不需要用到）
    /// </summary>
    public class TempFootBf
    {
        private string _ResponseValue = string.Empty;
        private string _CacheFolder = "~/odds/Cache/foot/";

        #region 属性

        private bool _CacheUsed = false; //是否记录缓存/存TXT
        private int _CacheTime = -1;//缓存时间(分钟,缓存时间小于0时，则定义单位为秒(绝对值))

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
        public TempFootBf()
        {
        }

        /// <summary>
        /// 取得足球赛程
        /// </summary>
        public string GetFootBfOnce()
        {
            string obj = string.Empty;

            string url = string.Empty;
            url = "http://bf.titan007.com/vbsxml/bfdata.js";
 
            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = "足球即时比分";

            httpRequest.WebAsync.RevCharset = "GB2312";
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                obj = FootBfOnceHtml(this._ResponseValue);

            return obj;
        }

        /// <summary>
        /// 得到足球赛程
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private string FootBfOnceHtml(string p_html)
        {
            if (string.IsNullOrEmpty(p_html))
                return "";

            return p_html;
        }

      
    }
}
