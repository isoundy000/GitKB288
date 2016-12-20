using System;
using System.Text.RegularExpressions;
using BCW.Common;
using TPR2.Model.Http;
namespace TPR2.Http
{
    /// <summary>
    /// 比分直播处理类
    /// </summary>
    public class GetLive2
    {
        private string _ResponseValue = string.Empty;
        private string _CacheFolder = "~/Files/Cache/live/zq/";

        #region 属性

        private bool _CacheUsed = true; //是否记录缓存/存TXT
        private int _CacheTime = 60*10;//缓存时间(分钟)

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
        public GetLive2()
        {
        }

        /// <summary>
        /// 取得足球完场比分/一周赛事
        /// </summary>
        public TPR2.Model.Http.Live2 Getzlist2(string p_title, int itype)
        {
            TPR2.Model.Http.Live2 objzlist2 = null;

            string url = string.Format("http://wap.titan007.com/weekindex.aspx?type={0}&nametype=1", itype);

            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = p_title+""+itype;

            httpRequest.WebAsync.RevCharset = "UTF-8";
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                objzlist2 = Livezlist2Html(this._ResponseValue,itype);

            return objzlist2;
        }

        /// <summary>
        /// 处理足球完场比分/一周赛事
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private TPR2.Model.Http.Live2 Livezlist2Html(string p_html, int itype)
        {
            string strVal = p_html;
            TPR2.Model.Http.Live2 objzlist2 = new TPR2.Model.Http.Live2();

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
                    str = m1.Groups[1].Value.Replace("/Sclassseach.aspx?", Utils.getUrl("live.aspx?act=6") + "&amp;").Replace("/Sclassseach2.aspx?", Utils.getUrl("live.aspx?act=7") + "&amp;");
                    
                    objzlist2.txtLive2zlist = str;
                }
            }

            return objzlist2;
        }

        /// <summary>
        /// 取得足球完场比分/一周赛事子列表
        /// </summary>
        public TPR2.Model.Http.Live2 Getzlists2(string p_day, int itype)
        {
            TPR2.Model.Http.Live2 objzlists2 = null;
            string url = "";
            if (itype == 1)
            {
                url = string.Format("http://wap.titan007.com/Sclassseach.aspx?day={0}&type=1&nametype=1", p_day);
            }
            else
            {
                url = string.Format("http://wap.titan007.com/Sclassseach2.aspx?day={0}&type=2&nametype=1", p_day);
            }

            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = p_day + "" + itype;

            httpRequest.WebAsync.RevCharset = "UTF-8";
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                objzlists2 = Livezlists2Html(this._ResponseValue);

            return objzlists2;
        }

        /// <summary>
        /// 处理足球完场比分/一周赛事子列表
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private TPR2.Model.Http.Live2 Livezlists2Html(string p_html)
        {
            string strVal = p_html;
            TPR2.Model.Http.Live2 objzlists2 = new TPR2.Model.Http.Live2();

            if (!string.IsNullOrEmpty(strVal))
            {
                string pattern = "";
                pattern = "</p><p>([\\s\\S]+?)</p><p align=\"center\">";
                Match m1 = Regex.Match(strVal, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                if (m1.Success)
                {
                    string str = "";
                    str = m1.Groups[1].Value.Replace("Scheduleseach.aspx?", Utils.getUrl("live.aspx?act=8") + "&amp;").Replace("Scheduleseach2.aspx?", Utils.getUrl("live.aspx?act=9") + "&amp;").Replace("<small>", "").Replace("</small>", "");

                    objzlists2.txtLive2zlists = str;
                }
            }

            return objzlists2;
        }

        /// <summary>
        /// 取得详细记录
        /// </summary>
        public TPR2.Model.Http.Live2 GetView(string p_day, int act,int SClassID)
        {
            TPR2.Model.Http.Live2 objview = null;
            string url = "";
            if (act == 8)
            {
                url = string.Format("http://wap.titan007.com/Scheduleseach.aspx?day={0}&nametype=1&SClassID={1}", p_day, SClassID);
            }
            else
            {
                url = string.Format("http://wap.titan007.com/Scheduleseach2.aspx?day={0}&nametype=1&SClassID={1}", p_day, SClassID);
            }

            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = p_day + "" + SClassID + "" + act;

            httpRequest.WebAsync.RevCharset = "UTF-8";
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                objview = LiveViewHtml(this._ResponseValue);

            return objview;
        }

        /// <summary>
        /// 处理详细记录
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private TPR2.Model.Http.Live2 LiveViewHtml(string p_html)
        {
            string strVal = p_html;
            TPR2.Model.Http.Live2 objview = new TPR2.Model.Http.Live2();

            if (!string.IsNullOrEmpty(strVal))
            {
                string pattern = "";
                pattern = "</p><p>([\\s\\S]+?)</p><p align=\"center\">";
                Match m1 = Regex.Match(strVal, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                if (m1.Success)
                {
                    string str = "";
                    str = m1.Groups[1].Value.Replace("<small>", "").Replace("</small>", "");
                    str = Regex.Replace(str, @"<a.href=./analysis/football.aspx[\s\S]+?>析</a>", "");
                    objview.txtLive2View = str;
                }
            }

            return objview;
        }
    }
}
