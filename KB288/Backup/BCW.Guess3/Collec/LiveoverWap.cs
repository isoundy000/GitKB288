using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using BCW.Common;
using TPR3.Model;

namespace TPR3.Collec
{
    /// <summary>
    /// 完场比分抓取类
    /// </summary>
    public class LiveoverWap
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
        public LiveoverWap()
        {
        }

        /// <summary>
        /// 取得足球完场比分
        /// </summary>
        public string GetFootover(int State)
        {
            string obj = string.Empty;

            string url = "http://wap.titan007.com/Scheduleall.aspx?nametype=1&State=" + State + "";

            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = "足球完场比分XML";

            httpRequest.WebAsync.RevCharset = "UTF-8";
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                obj = FootoverHtml(this._ResponseValue);

            return obj;
        }

        /// <summary>
        /// 处理完场比分内容
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private string FootoverHtml(string p_html)
        {
            if (string.IsNullOrEmpty(p_html))
                return "";

            string pattern = @"场&gt;&gt;</b><br/>([\s\S]+?)</p><p align=.center.>";
            Match m = Regex.Match(p_html, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (m.Success)
            {
                p_html = m.Groups[1].Value;
            }
            
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

        /// <summary>
        /// 取得篮球完场比分
        /// </summary>
        public string GetBasketover()
        {
            string obj = string.Empty;

            string url = "http://wap.titan007.com/nbaScheduleall.aspx?nametype=1";

            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = "篮球完场比分XML";

            httpRequest.WebAsync.RevCharset = "UTF-8";
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                obj = BasketoverHtml(this._ResponseValue);

            return obj;
        }

        /// <summary>
        /// 处理完场比分内容
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private string BasketoverHtml(string p_html)
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
