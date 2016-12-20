using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using BCW.Common;
using TPR2.Model;

namespace TPR2.Collec
{
    /// <summary>
    /// 篮球盘口抓取类
    /// </summary>
    public class BasketWap
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
        public BasketWap()
        {
        }

        /// <summary>
        /// 取得篮球指数必要数据
        /// </summary>
        public string GetBasketWap(int p_id)
        {
            string obj = string.Empty;

            //string url = "http://wap.titan007.com/analysis/basketball.aspx?id=" + p_id + "";

            string url = "http://nba.win007.com/analysis/" + p_id + ".htm";


            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = "篮球指数必要数据XML" + p_id + "";

            httpRequest.WebAsync.RevCharset = "UTF-8";
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                obj = BasketWapHtml(this._ResponseValue);

            return obj;
        }

        /// <summary>
        /// 处理篮球指数必要数据
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private string BasketWapHtml(string p_html)
        {
            if (string.IsNullOrEmpty(p_html))
                return "";

            System.Text.StringBuilder builder = new System.Text.StringBuilder("");
            //MatchCollection mc = Regex.Matches(p_html, @"<small>([\s\S]+?)</small>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            MatchCollection mc = Regex.Matches(p_html, @"<table width=""100%"" border=""0"" bgcolor=""#FFFFFF"">([\s\S]+?)\s资料库</u>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
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
