using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using BCW.Common;

namespace BCW.Service
{
    /// <summary>
    /// 抓取天气数据
    /// </summary>
    public class GetWeather
    {
        private string _ResponseValue = string.Empty;
        private string _CacheFolder = "~/Files/Cache/";

        #region 属性

        private bool _CacheUsed = true; //是否记录缓存/存TXT
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
        public GetWeather()
        {

        }

        /// <summary>
        /// 取得天气
        /// </summary>
        public string GetWeatherXML(string theCityName)
        {
            string obj = "";
            string url = "http://www.webxml.com.cn/WebServices/WeatherWebService.asmx/getWeatherbyCityName";
            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = "" + theCityName + "天气";
            httpRequest.WebAsync.RevCharset = "UTF-8";
            httpRequest.WebAsync.PostData = "theCityName=" + theCityName;
            httpRequest.WebAsync.PostCharset = "UTF-8";

            if (httpRequest.MethodPostUrl(out this._ResponseValue))
                obj = WeatherHtml(this._ResponseValue);

            return obj;
        }


        /// <summary>
        /// 处理详细天气
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private string WeatherHtml(string p_html)
        {
            if (string.IsNullOrEmpty(p_html))
                return "";

            System.Text.StringBuilder builder = new System.Text.StringBuilder("");
            MatchCollection mc = Regex.Matches(p_html, @"<string>([\s\S]+?)</string>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
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
