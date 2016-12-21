using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using BCW.Common;

namespace BCW.Service
{
    /// <summary>
    /// 抓取天气数据
    /// </summary>
    public class GetWeather3gcn
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
        public GetWeather3gcn()
        {

        }

        /// <summary>
        /// 取得天气
        /// </summary>
        public string GetWeather3gcnXML(string theCityName)
        {
            string obj = "";
            string url = "http://fuwu.3g.cn/foryou/weather/tianqi.aspx?city=" + theCityName + "";
            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = "" + theCityName + "天气";
            httpRequest.WebAsync.RevCharset = "UTF-8";
            //httpRequest.WebAsync.PostData = "theCityName=" + theCityName;
            //httpRequest.WebAsync.PostCharset = "UTF-8";

            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                obj = Weather3gcnHtml(this._ResponseValue);

            return obj;
        }


        /// <summary>
        /// 处理详细天气
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private string Weather3gcnHtml(string p_html)
        {
            if (string.IsNullOrEmpty(p_html))
                return "";

            System.Text.StringBuilder builder = new System.Text.StringBuilder("");
            string pattern = @"地区天气预报<br/>([\s\S]+?)(<br/>----------------<br/>|看你的城市呼啦圈)";
            Match m = Regex.Match(p_html, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (m.Success)
            {
                string str = m.Groups[1].Value;
                //string str = Regex.Replace(m.Groups[1].Value, @"<a href=[\s\S]+?</a>(<br/>)?", string.Empty, RegexOptions.IgnoreCase);
                str = str.Replace(@"					----------------<br/>
					
		<br/>", "----------------<br/>");


                string VE = ConfigHelper.GetConfigString("VE");
                string SID = ConfigHelper.GetConfigString("SID");

                str = str.Replace("./images/", "/Files/sys/weather/");
                str = str.Replace("images/", "/Files/sys/weather/");
                str = str.Replace("get2.aspx?", "weather.aspx?act=more&amp;");
                str = str.Replace("sid=&amp;id=&amp;", "");
                str = str.Replace("&amp;mob=0&amp;waped=2", "&amp;" + VE + "=" + Utils.getstrVe() + "&amp;" + SID + "=" + Utils.getstrU() + "");
                str = str.Replace("type", "ptype");
                str = str.Replace("架车指数", "驾车指数");

                builder.Append(str);
            }

            return builder.ToString();
        }


        /// <summary>
        /// 取得天气指数
        /// </summary>
        public string GetWeather3gcnXML2(int cityid, int ptype, string theCityName)
        {
            string obj = "";
            string url = "http://fuwu.3g.cn/foryou/weather/get2.aspx?cityid=" + cityid + "&type=" + ptype + "&city=" + theCityName + "";
            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = "" + theCityName + "天气" + cityid + "_" + ptype + "";
            httpRequest.WebAsync.RevCharset = "UTF-8";
            //httpRequest.WebAsync.PostData = "theCityName=" + theCityName;
            //httpRequest.WebAsync.PostCharset = "UTF-8";

            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                obj = Weather3gcnHtml2(this._ResponseValue);

            return obj;
        }

        /// <summary>
        /// 处理详细天气指数
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private string Weather3gcnHtml2(string p_html)
        {
            if (string.IsNullOrEmpty(p_html))
                return "";

            System.Text.StringBuilder builder = new System.Text.StringBuilder("");
            string pattern = @"<p>([\s\S]+?)<a href=.tianqi.aspx";
            Match m = Regex.Match(p_html, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (m.Success)
            {

                string VE = ConfigHelper.GetConfigString("VE");
                string SID = ConfigHelper.GetConfigString("SID");

                string str = m.Groups[1].Value;
                str = str.Replace("get2.aspx?", "weather.aspx?act=more&amp;");
                str = str.Replace("sid=&amp;id=&amp;", "");
                str = str.Replace("&amp;sr=&amp;mob=&amp;waped=", "&amp;" + VE + "=" + Utils.getstrVe() + "&amp;" + SID + "=" + Utils.getstrU() + "");
                str = str.Replace("type", "ptype");
                str = str.Replace("架车指数", "驾车指数");
                str = str.Replace("<br /><br />", "");

                builder.Append(str);
            }

            return builder.ToString();
        }

        /// <summary>
        /// 处理详细天气2(作为简单调用)
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        public string WeatherCall(string city)
        {
            string weather = GetWeather3gcnXML(city);
            if (string.IsNullOrEmpty(weather))
                return "";

            System.Text.StringBuilder builder = new System.Text.StringBuilder("");
            string pattern = @"\[天气\]([\s\S]+?)\[风向\]";
            Match m = Regex.Match(weather, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (m.Success)
            {
                string str = m.Groups[1].Value;
                str = str.Replace("<br/>", "");
                str = str.Replace("<br />", "");
                str = str.Replace("[天气]", "");
                str = str.Replace("[气温]", "");
                str = str.Replace("\n", "");
                str = str.Replace("	", "");
                str = str.Replace("\r  ", "");
                str = str.Replace("\r ", "");
                builder.Append(str);
            }
            else
            {
                string pattern2 = @"\[明天\]([\s\S]+?)\[";
                Match m2 = Regex.Match(weather, pattern2, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                if (m2.Success)
                {
                    string str = m2.Groups[1].Value + "";
                    str = str.Replace("" + Convert.ToInt32(DateTime.Now.Month) + "月" + Convert.ToInt32(DateTime.Now.Day) + "日 ", "");
                    builder.Append(str);
                }

            }

            return builder.ToString();
        }

        /// <summary>
        /// 处理详细天气2(作为简单调用2)
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        public string WeatherCall2(string city)
        {
            string weather = GetWeather3gcnXML(city);
            if (string.IsNullOrEmpty(weather))
                return "";

            System.Text.StringBuilder builder = new System.Text.StringBuilder("");
            string pattern = @"\[天气\]([\s\S]+?)\[风向\]";
            Match m = Regex.Match(weather, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (m.Success)
            {
                string str = m.Groups[1].Value;
                str = str.Replace("<br/>", "");
                str = str.Replace("<br />", "");
                str = str.Replace("[天气]", "");
                str = str.Replace("[气温]", "");
                str = str.Replace("\n", "");
                str = str.Replace("	", "");
                str = str.Replace("\r  ", "");
                str = str.Replace("\r ", "");
                str = Regex.Replace(str, @"<[^>]+>?[^<]*>", " ", RegexOptions.IgnoreCase);//<.+?>
                str = str.Replace("℃/", " ~ ");
                str = Regex.Replace(str, @"\([\s\S]+?\)", " ", RegexOptions.IgnoreCase);
                str = str.Replace("℃", " ℃");
                builder.Append(str);
            }
            else
            {
                string pattern2 = @"\[明天\]([\s\S]+?)℃ ";
                Match m2 = Regex.Match(weather, pattern2, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                if (m2.Success)
                {
                    string str = m2.Groups[1].Value + "℃";
                    str = str.Replace("" + Convert.ToInt32(DateTime.Now.Month) + "月" + Convert.ToInt32(DateTime.Now.Day) + "日 ", "");
                    str = Regex.Replace(str, @"<[^>]+>?[^<]*>", " ", RegexOptions.IgnoreCase);//<.+?>
                    builder.Append(str);
                }

            }
            return builder.ToString();
        }
    }
}
