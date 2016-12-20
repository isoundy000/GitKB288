using System;
using System.Text.RegularExpressions;
using BCW.Common;
using TPR.Model.Http;
namespace TPR.Http
{
    /// <summary>
    /// WNBA文字直播处理类
    /// </summary>
    public class GetWNbaword
    {
        private string _ResponseValue = string.Empty;
        private string _CacheFolder = "~/Files/Cache/live/";

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
        public GetWNbaword()
        {
        }

        /// <summary>
        /// 取得WNba文字直播列表
        /// </summary>
        public string Getwnbalist()
        {
            string strVal = string.Empty;

            string url = "http://live.500wan.com/lq.php";

            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = "WNba文字直播";

            httpRequest.WebAsync.RevCharset = "GB2312";
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                strVal = LivewnbalistHtml(this._ResponseValue);

            return strVal;
        }

        /// <summary>
        /// 处理WNba文字直播列表
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private string LivewnbalistHtml(string p_html)
        {
            string strVal = p_html;
            if (!string.IsNullOrEmpty(strVal))
            {
                //string pattern = "var liveCategory = \"jc\",([\\s\\S]+?)底部滚动友情连接";

                string pattern = "var matchList = ([\\s\\S]+?)var oddsList = ";


                Match m1 = Regex.Match(strVal, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                if (m1.Success)
                {
                    string str = "";
                    str = m1.Groups[1].Value;

                    strVal = str;
                }
            }

            return strVal;
        }

      


    }
}
