using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using BCW.Common;
using TPR.Model;

namespace TPR.Collec
{
    public class Basketbz
    {
        private string _ResponseValue = string.Empty;
        private string _CacheFolder = "~/Files/Cache/live/getlq/";

        #region 属性

        private bool _CacheUsed = false; //是否记录缓存/存TXT
        private int _CacheTime = 20;//缓存时间(分钟)

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
        public Basketbz()
        {
        }

        /// <summary>
        /// 取得篮球列表XML
        /// </summary>
        public string GetBasketbzlist()
        {
            string obj = string.Empty;

            string url = string.Empty;
            url = "http://wap.titan007.com/cnnbaoddslist.aspx?cid=3&nametype=1";
            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = "篮球2XML";

            httpRequest.WebAsync.RevCharset = "UTF-8";
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                obj = BasketbzlistHtml(this._ResponseValue);

            return obj;
        }


        /// <summary>
        /// 得到列表的A标签中的地址
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private string BasketbzlistHtml(string p_html)
        {
            if (string.IsNullOrEmpty(p_html))
                return "";

            System.Text.StringBuilder builder = new System.Text.StringBuilder("");
            MatchCollection mc = Regex.Matches(p_html, @"<a.href=./nbaoddsshow.aspx([\s\S]+?).\stitle=", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (mc.Count > 0)
            {
                for (int i = 0; i < mc.Count; i++)
                {
                    builder.Append("#" + mc[i].Groups[1].Value.ToString());
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// 取得篮球内容
        /// </summary>
        public string GetBasketbzview(string url)
        {
            string obj = string.Empty;

            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = "篮球内容2XML";

            httpRequest.WebAsync.RevCharset = "UTF-8";
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                obj = BasketbzviewHtml(this._ResponseValue);

            return obj;
        }

        /// <summary>
        /// 处理内容
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private string BasketbzviewHtml(string p_html)
        {
            if (string.IsNullOrEmpty(p_html))
                return "";

            System.Text.StringBuilder builder = new System.Text.StringBuilder("");
            MatchCollection mc = Regex.Matches(p_html, @"<small>([\s\S]+?)</small>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (mc.Count > 0)
            {
                for (int i = 0; i < mc.Count; i++)
                {
                    builder.Append("#" + mc[i].Groups[1].Value.ToString());
                }
            }

            return builder.ToString();
        }

    }
}
