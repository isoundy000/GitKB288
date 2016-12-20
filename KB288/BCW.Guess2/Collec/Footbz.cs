using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using BCW.Common;
using TPR2.Model;

namespace TPR2.Collec
{
    /// <summary>
    /// 足球标准抓取类
    /// </summary>
    public class Footbz
    {
        private string _ResponseValue = string.Empty;
        private string _CacheFolder = "~/Files/Cache/live/getzq/";

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
        public Footbz()
        {
        }

        /// <summary>
        /// 取得足球标准盘列表XML
        /// </summary>
        public string GetFootbzlist()
        {
            string obj = string.Empty;

            string url = string.Empty;
            //if (Utils.GetTopDomain() == "tl88.cc" || Utils.GetTopDomain() == "168yy.cc")
            //{
                //url = "http://wap.titan007.com/oddsSclass.aspx?cid=24&nametype=1";
            //}
            //else
            //{
                url = "http://wap.titan007.com/oddsSclass.aspx?cid=3&nametype=1";
            //}
            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = "足球标准盘2XML";

            httpRequest.WebAsync.RevCharset = "UTF-8";
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                obj = FootbzlistHtml(this._ResponseValue);

            return obj;
        }

        /// <summary>
        /// 得到列表的A标签中的地址
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private string FootbzlistHtml(string p_html)
        {
            if (string.IsNullOrEmpty(p_html))
                return "";

            System.Text.StringBuilder builder = new System.Text.StringBuilder("");
            MatchCollection mc = Regex.Matches(p_html, @"<a.href=./OddsShow.aspx([\s\S]+?)\s+?.\stitle=", RegexOptions.Compiled | RegexOptions.IgnoreCase);
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
        /// 取得足球标准盘内容
        /// </summary>
        public string GetFootbzview(string url)
        {
            string obj = string.Empty;

            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = "足球标准盘2XML";

            httpRequest.WebAsync.RevCharset = "UTF-8";
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                obj = FootbzviewHtml(this._ResponseValue);

            return obj;
        }


        /// <summary>
        /// 处理内容
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private string FootbzviewHtml(string p_html)
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
