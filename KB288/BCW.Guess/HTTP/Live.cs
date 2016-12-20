using System;
using System.Text.RegularExpressions;
using BCW.Common;
using TPR.Model.Http;
namespace TPR.Http
{
    /// <summary>
    /// 比分直播处理类
    /// </summary>
    public class GetLive
    {
        private string _ResponseValue = string.Empty;
        private string _CacheFolder = "~/Files/Cache/live/jszq/";

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
        public GetLive()
        {
        }

        /// <summary>
        /// 取得足球即时比分列表
        /// </summary>
        public TPR.Model.Http.Live Getzlist(string p_title, int kind)
        {
            TPR.Model.Http.Live objzlist = null;

            string url = string.Format("http://wap.titan007.com/cnlist.aspx?nametype=1&kind={0}", kind);

            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = p_title + "" + kind;

            httpRequest.WebAsync.RevCharset = "UTF-8";
            //httpRequest.WebAsync.PostData = "Input=" + p_mobileNumber;
            //httpRequest.WebAsync.PostCharset = "UTF-8";

            //if (httpRequest.MethodPostUrl(out this._ResponseValue))
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                objzlist = LivezlistHtml(this._ResponseValue);

            return objzlist;
        }

        /// <summary>
        /// 处理足球即时比分列表
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private TPR.Model.Http.Live LivezlistHtml(string p_html)
        {
            string strVal = p_html;
            TPR.Model.Http.Live objzlist = new TPR.Model.Http.Live();

            if (!string.IsNullOrEmpty(strVal))
            {
                string pattern = "title=\"未开场\">未开场</a><br/>([\\s\\S]+?)</small><br/>";
                Match m1 = Regex.Match(strVal, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                if (m1.Success)
                {
                    string str = "";
                    str = m1.Groups[1].Value.Replace("<small>", "").Replace("</small>", "").Replace("/Schedule.aspx?", Utils.getUrl("live.aspx?act=5&amp;") + "&amp;");

                    objzlist.txtLivezlist = str;
                }
            }

            return objzlist;
        }


        /// <summary>
        /// 取得未开/进行/完场列表
        /// </summary>
        public TPR.Model.Http.Live Getmlist(string p_title, int State, int kind)
        {
            TPR.Model.Http.Live objmlist = null;

            string url = string.Format("http://wap.titan007.com/ScheduleAll.aspx?State={0}&kind={1}&nametype=1", State, kind);

            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = p_title + "" + State;

            httpRequest.WebAsync.RevCharset = "UTF-8";
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                objmlist = LivemlistHtml(this._ResponseValue);

            return objmlist;
        }
        /// <summary>
        /// 处理未开/进行/完场列表
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private TPR.Model.Http.Live LivemlistHtml(string p_html)
        {
            string strVal = p_html;
            TPR.Model.Http.Live objmlist = new TPR.Model.Http.Live();

            if (!string.IsNullOrEmpty(strVal))
            {
                string pattern = "&gt;&gt;</b><br/>([\\s\\S]+?)</p><p align=\"center\">";
                Match m1 = Regex.Match(strVal, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                if (m1.Success)
                {
                    string str = "";
                    str = m1.Groups[1].Value.Replace("<small>", "").Replace("</small>", "");
                    str = Regex.Replace(str, @"<a.href=./analysis/football.aspx[\s\S]+?>析</a>", "");
                    objmlist.txtLiveState = str;
                }
            }

            return objmlist;
        }


        /// <summary>
        /// 取得详细记录
        /// </summary>
        public TPR.Model.Http.Live Getvlist(int kind, int SClassID, string SClass)
        {
            TPR.Model.Http.Live objvlist = null;

            string url = string.Format("http://wap.titan007.com/Schedule.aspx?nametype=1&kind={0}&SClassID={1}&SClass={2}", kind, SClassID, SClass);

            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = SClass + "" + SClassID + "" + kind;

            httpRequest.WebAsync.RevCharset = "UTF-8";
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                objvlist = LivevlistHtml(this._ResponseValue);

            return objvlist;
        }

        /// <summary>
        /// 处理详细记录
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private TPR.Model.Http.Live LivevlistHtml(string p_html)
        {
            string strVal = p_html;
            TPR.Model.Http.Live objvlist = new TPR.Model.Http.Live();

            if (!string.IsNullOrEmpty(strVal))
            {
                string pattern = "</p><p>([\\s\\S]+?)</p><p align=\"center\">";
                Match m1 = Regex.Match(strVal, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                if (m1.Success)
                {
                    string str = "";
                    str = m1.Groups[1].Value.Replace("<small>", "").Replace("</small>", "").Replace("</big>", "").Replace("<big>", "");
                    str = Regex.Replace(str, @"<a.href=./analysis/football.aspx[\s\S]+?>析</a>", "");
                    str = Regex.Replace(str, @"广告[\s\S]+?[\d]{11}<br/>", "");
                    objvlist.txtLiveView = str;
                }
            }

            return objvlist;
        }
    }
}
