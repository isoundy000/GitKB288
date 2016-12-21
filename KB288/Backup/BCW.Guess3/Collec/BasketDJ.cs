using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using BCW.Common;
using TPR3.Model;

namespace TPR3.Collec
{
    /// <summary>
    /// 指数
    /// </summary>
    public class BasketDJ
    {
        private string _ResponseValue = string.Empty;
        private string _CacheFolder = "~/odds/Cache/basket/";

        #region 属性

        private bool _CacheUsed = false; //是否记录缓存/存TXT
        private int _CacheTime = 1;//缓存时间(分钟,缓存时间小于0时，则定义单位为秒(绝对值))

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
        public BasketDJ()
        {
        }

        /// <summary>
        /// 取得指数
        /// </summary>
        public string GetBasketDJ()
        {
            string obj = string.Empty;

            string url = "http://vip.titan007.com/xmlvbs/fl_nbaGoal3.xml";

            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = "皇冠单节";

            httpRequest.WebAsync.RevCharset = "UTF-8";
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                obj = FootHtml(this._ResponseValue);

            return obj;
        }

        /// <summary>
        /// 得到指数2
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private string FootHtml(string p_html)
        {
            if (string.IsNullOrEmpty(p_html))
                return "";

            return p_html;
        }

      
    }
}
