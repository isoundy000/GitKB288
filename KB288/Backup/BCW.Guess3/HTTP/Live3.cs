using System;
using System.Text.RegularExpressions;
using BCW.Common;
using TPR3.Model.Http;
namespace TPR3.Http
{
    /// <summary>
    /// 比分直播处理类
    /// </summary>
    public class GetLive3
    {
        private string _ResponseValue = string.Empty;
        private string _CacheFolder = "~/Files/Cache/Live/jslq/";

        #region 属性

        private bool _CacheUsed = true; //是否记录缓存/存TXT
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
        public GetLive3()
        {
        }

        /// <summary>
        /// 取得篮球即时比分列表
        /// </summary>
        public TPR3.Model.Http.Live3 Getllist(string p_title)
        {
            TPR3.Model.Http.Live3 objllist = null;

            string url = "http://wap.titan007.com/nbabflist.aspx?nametype=1";

            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = p_title;

            httpRequest.WebAsync.RevCharset = "UTF-8";
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                objllist = Live3llistHtml(this._ResponseValue);

            return objllist;
        }

        /// <summary>
        /// 处理篮球即时比分列表
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private TPR3.Model.Http.Live3 Live3llistHtml(string p_html)
        {
            string strVal = p_html;
            TPR3.Model.Http.Live3 objllist = new TPR3.Model.Http.Live3();

            if (!string.IsNullOrEmpty(strVal))
            {
                string pattern = "&gt;&gt;篮球即时比分<br/>([\\s\\S]+?)</p><p align=\"center\">";
                Match m1 = Regex.Match(strVal, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                if (m1.Success)
                {
                    string str = "";
                    str = m1.Groups[1].Value.Replace("<small>", "").Replace("</small>", "").Replace("/nbabfshow.aspx?", Utils.getUrl("live.aspx?act=11") + "&amp;").Replace("/nbaScheduleall.aspx?", Utils.getUrl("live.aspx?act=12") + "&amp;");
                    str = Regex.Replace(str, @"<a.href=.nbaScheduleall.aspx[\s\S]+?>今天全部赛事</a><br/>", "");
                    objllist.txtLivellist = str;
                }
            }

            return objllist;
        }


        /// <summary>
        /// 取得今天篮球全部赛事
        /// </summary>
        public TPR3.Model.Http.Live3 Gettlist(string p_title)
        {
            TPR3.Model.Http.Live3 objtlist = null;

            string url = "http://wap.titan007.com/nbaScheduleall.aspx?nametype=1";

            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = p_title;

            httpRequest.WebAsync.RevCharset = "UTF-8";
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                objtlist = Live3tlistHtml(this._ResponseValue);

            return objtlist;
        }

        /// <summary>
        /// 处理今天篮球全部赛事
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private TPR3.Model.Http.Live3 Live3tlistHtml(string p_html)
        {
            string strVal = p_html;
            TPR3.Model.Http.Live3 objtlist = new TPR3.Model.Http.Live3();

            if (!string.IsNullOrEmpty(strVal))
            {
                string pattern = "</p><p>([\\s\\S]+?)</p><p align=\"center\">";
                Match m1 = Regex.Match(strVal, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                if (m1.Success)
                {
                    string str = "";
                    str = m1.Groups[1].Value.Replace("<small>", "").Replace("</small>", "");
                    str = Regex.Replace(str, @"<a.href=./analysis/basketball.aspx[\s\S]+?>析</a>", "");
                    objtlist.txtLivetlist = str;
                }
            }

            return objtlist;
        }

        /// <summary>
        /// 取得详细记录
        /// </summary>
        public TPR3.Model.Http.Live3 Getvlist(int SClassID, string SClass)
        {
            TPR3.Model.Http.Live3 objvlist = null;

            string url = string.Format("http://wap.titan007.com/nbabfshow.aspx?nametype=1&SClassID={0}&SClass={1}", SClassID, SClass);

            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = SClass + "" + SClassID;

            httpRequest.WebAsync.RevCharset = "UTF-8";
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                objvlist = Live3vlistHtml(this._ResponseValue);

            return objvlist;
        }

        /// <summary>
        /// 处理详细记录
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private TPR3.Model.Http.Live3 Live3vlistHtml(string p_html)
        {
            string strVal = p_html;
            TPR3.Model.Http.Live3 objvlist = new TPR3.Model.Http.Live3();

            if (!string.IsNullOrEmpty(strVal))
            {
                string pattern = "</p><p>([\\s\\S]+?)</p><p align=\"center\">";
                Match m1 = Regex.Match(strVal, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                if (m1.Success)
                {
                    string str = "";
                    str = m1.Groups[1].Value.Replace("<small>", "").Replace("</small>", "");
                    str = Regex.Replace(str, @"<a.href=./analysis/basketball.aspx[\s\S]+?>析</a>", "");
                    objvlist.txtLiveView = str;
                }
            }

            return objvlist;
        }
    }
}
