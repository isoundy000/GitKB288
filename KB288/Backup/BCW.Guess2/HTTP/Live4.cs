using System;
using System.Text.RegularExpressions;
using BCW.Common;
using TPR2.Model.Http;
namespace TPR2.Http
{
    /// <summary>
    /// 比分直播处理类
    /// </summary>
    public class GetLive4
    {
        private string _ResponseValue = string.Empty;
        private string _CacheFolder = "~/Files/Cache/live/lq/";

        #region 属性

        private bool _CacheUsed = true; //是否记录缓存/存TXT
        private int _CacheTime = 60 * 10;//缓存时间(分钟)

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
        public GetLive4()
        {
        }

        /// <summary>
        /// 取得篮球完场比分/一周赛事
        /// </summary>
        public TPR2.Model.Http.Live4 Getllist2(string p_title, int itype)
        {
            TPR2.Model.Http.Live4 objllist2 = null;

            string url = string.Format("http://wap.titan007.com/nbaweekindex.aspx?type={0}&nametype=1", itype);

            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = p_title + "" + itype;

            httpRequest.WebAsync.RevCharset = "UTF-8";
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                objllist2 = Livellist2Html(this._ResponseValue, itype);

            return objllist2;
        }

        /// <summary>
        /// 处理篮球完场比分/一周赛事
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private TPR2.Model.Http.Live4 Livellist2Html(string p_html, int itype)
        {
            string strVal = p_html;
            TPR2.Model.Http.Live4 objllist2 = new TPR2.Model.Http.Live4();

            if (!string.IsNullOrEmpty(strVal))
            {
                string pattern = "";
                if (itype == 1)
                {
                    pattern = "</p><p>([\\s\\S]+?)<br/>查询完场比分：<br/>";
                }
                else
                {
                    pattern = "</p><p>([\\s\\S]+?)</p><p align=\"center\">";
                }
                Match m1 = Regex.Match(strVal, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                if (m1.Success)
                {
                    string str = "";

                    str = m1.Groups[1].Value.Replace("/nbaSclassseach.aspx?", Utils.getUrl("live.aspx?act=14") + "&amp;");


                    objllist2.txtLive2llist = str;
                }
            }

            return objllist2;
        }

        /// <summary>
        /// 取得篮球完场比分/一周赛事子列表
        /// </summary>
        public TPR2.Model.Http.Live4 Getllists2(string p_day, int itype)
        {
            TPR2.Model.Http.Live4 objllists2 = null;
            string url = "";
            url = string.Format("http://wap.titan007.com/nbaSclassseach.aspx?day={0}&type={1}&nametype=1", p_day, itype);
            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = p_day + "" + itype;

            httpRequest.WebAsync.RevCharset = "UTF-8";
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                objllists2 = Livellists2Html(this._ResponseValue, itype);

            return objllists2;
        }

        /// <summary>
        /// 处理篮球完场比分/一周赛事子列表
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private TPR2.Model.Http.Live4 Livellists2Html(string p_html, int itype)
        {
            string strVal = p_html;
            TPR2.Model.Http.Live4 objllists2 = new TPR2.Model.Http.Live4();

            if (!string.IsNullOrEmpty(strVal))
            {
                string pattern = "";
                pattern = "</p><p>([\\s\\S]+?)</p><p align=\"center\">";
                Match m1 = Regex.Match(strVal, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                if (m1.Success)
                {
                    string str = "";
                    str = m1.Groups[1].Value.Replace("nbaScheduleseach.aspx?", Utils.getUrl("live.aspx?act=15") + "&amp;").Replace("<small>", "").Replace("</small>", "");

                    objllists2.txtLive2llists = str;
                }
            }

            return objllists2;
        }

        /// <summary>
        /// 取得详细记录
        /// </summary>
        public TPR2.Model.Http.Live4 GetView(string p_day, int itype, int SClassID, string SClass)
        {
            TPR2.Model.Http.Live4 objview = null;
            string url = "";
            url = string.Format("http://wap.titan007.com/nbaScheduleseach.aspx?type={0}&day={1}&nametype=1&SClassID={2}&SClass={3}", itype, p_day, SClassID, SClass);

            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = p_day + "" + SClassID + "" + SClass + "" + itype;

            httpRequest.WebAsync.RevCharset = "UTF-8";
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                objview = LiveViewHtml(this._ResponseValue);

            return objview;
        }

        /// <summary>
        /// 处理详细记录
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private TPR2.Model.Http.Live4 LiveViewHtml(string p_html)
        {
            string strVal = p_html;
            TPR2.Model.Http.Live4 objview = new TPR2.Model.Http.Live4();

            if (!string.IsNullOrEmpty(strVal))
            {
                string pattern = "";
                pattern = "</p><p>([\\s\\S]+?)</p><p align=\"center\">";
                Match m1 = Regex.Match(strVal, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                if (m1.Success)
                {
                    string str = "";
                    str = m1.Groups[1].Value.Replace("<small>", "").Replace("</small>", "");
                    str = Regex.Replace(str, @"<a.href=./analysis/basketball.aspx[\s\S]+?>析</a>", "");
                    objview.txtLive2View = str;
                }
            }

            return objview;
        }
    }
}
