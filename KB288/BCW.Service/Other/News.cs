using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using BCW.Common;

namespace BCW.Service
{
    /// <summary>
    /// 抓取News数据
    /// </summary>
    public class GetNews
    {
        private string _ResponseValue = string.Empty;
        private string _CacheFolder = "~/Files/Cache/";

        #region 属性

        private bool _CacheUsed = true; //是否记录缓存/存TXT
        private int _CacheTime = 14400;//缓存时间(分钟)

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
        public GetNews()
        {

        }

        /// <summary>
        /// 取得News
        /// </summary>
        public string GetNewsXML(string Date)
        {
            string obj = "";
            string url = "http://news.sohu.com/_scroll_newslist/" + Date + "/news.inc";
            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = false;
            httpRequest.Fc.CacheTime = 0;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = "" + Date + "xml";
            httpRequest.WebAsync.RevCharset = "UTF-8";

            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                obj = NewsHtml(this._ResponseValue);

            return obj;
        }

        /// <summary>
        /// 处理News
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private string NewsHtml(string p_html)
        {
            if (string.IsNullOrEmpty(p_html))
                return "";

            string pattern = @"var\snewsJason\s=[\s\S]+?\]\,item\:\[([\s\S]+?)\]\}";
            Match m1 = Regex.Match(p_html, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            if (m1.Success)
            {
                string str = m1.Groups[1].ToString();
                return str;
            }
            else
                return "";

        }

        /// <summary>
        /// 取得News2
        /// </summary>
        public string GetNewsXML2(string url)
        {
            string obj = "";
            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = "" + url + "xml";
            httpRequest.WebAsync.RevCharset = "GB2312";

            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                obj = NewsHtml2(this._ResponseValue);

            return obj;
        }


        /// <summary>
        /// 处理News2
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private string NewsHtml2(string p_html)
        {
            if (string.IsNullOrEmpty(p_html))
                return "";

            string pattern = @"<!-- 正文 -->([\s\S]+?)<!-- seo标签描述 -->@<!-- 正文 st -->([\s\S]+?)<!-- 分享 st -->@<!-- 正文 st -->([\s\S]+?)<!-- 正文 end -->@<div class=""item-content"" id=""main-content"">([\s\S]+?)分享到：</span>";

            string[] pn = Regex.Split(pattern, "@");
            for (int i = 0; i < pn.Length; i++)
            {
                Match m1 = Regex.Match(p_html, pn[i], RegexOptions.Compiled | RegexOptions.IgnoreCase);

                if (m1.Success)
                {
                    string str = m1.Groups[1].ToString();
                    return str;
                }
            }
            return "";
        }


    }
}
