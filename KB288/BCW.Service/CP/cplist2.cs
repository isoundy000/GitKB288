using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using BCW.Common;

namespace BCW.Service
{
    /// <summary>
    /// 抓取彩票数据
    /// </summary>
    public class GetCplist2
    {
        private string _ResponseValue = string.Empty;
        private string _CacheFolder = "~/Files/Cache/";

        #region 属性

        private bool _CacheUsed = true; //是否记录缓存/存TXT
        private int _CacheTime = 1;//缓存时间(分钟)

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
        public GetCplist2()
        {

        }

        /// <summary>
        /// 取得列表
        /// </summary>
        public string GetCplist2XML(int classid, int nclassid, int pageIndex)
        {
            string obj = "";
            string url = "http://wap.111hz.net/lang.asp?classid=" + classid + "&nclassid=" + nclassid + "&page=" + pageIndex + "";
            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = "Cplist2" + classid + "_" + nclassid + "_" + pageIndex + "";
            httpRequest.WebAsync.RevCharset = "UTF-8";
            //httpRequest.WebAsync.PostData = "theCityName=" + theCityName;
            //httpRequest.WebAsync.PostCharset = "UTF-8";

            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                obj = Cplist2Html(this._ResponseValue);

            return obj;
        }


        /// <summary>
        /// 处理详细资料
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private string Cplist2Html(string p_html)
        {
            if (string.IsNullOrEmpty(p_html))
                return "";

            System.Text.StringBuilder builder = new System.Text.StringBuilder("");
            string pattern = @"title=.([\s\S]+?).>([\s\S]+?)-------------<br/>";
            Match m = Regex.Match(p_html, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (m.Success)
            {
                string str1 = m.Groups[1].Value;
                string str = m.Groups[2].Value;
                str = removeZ(str);
                str = str.Replace("<p>", "");
                str = str.Replace("-49hz.cc", "");
                str = str.Replace("49hz.cc", "");
                str = str.Replace("lang.asp?", "cplist.aspx?act=listb&amp;");
                str = str.Replace("tlist.asp?", "cplist.aspx?act=viewb&amp;");
                str = str.Replace("&amp;&amp;", "&amp;");
                str = str.Replace("&amp;nbsp;", "&nbsp;");
                str = str.Replace("page=", "&amp;page=");
                str = Regex.Replace(str, @"<a.href=.http://[\s\S]+?.>(\*)*[\s\S]+?\*</a>(<br/>)*", "");
                builder.Append(str1 + "!@#!@#" + str);
            }

            return builder.ToString();
        }

        /// <summary>
        /// 取得内容
        /// </summary>
        public string GetCplist2XML2(int id, int nclassid, int p)
        {
            string obj = "";
            string url = "http://wap.111hz.net/tlist.asp?id=" + id + "&nclassid=" + nclassid + "&p=" + p + "";
            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = "cpview2" + id + "_" + nclassid + "_" + p + "";
            httpRequest.WebAsync.RevCharset = "UTF-8";
            //httpRequest.WebAsync.PostData = "theCityName=" + theCityName;
            //httpRequest.WebAsync.PostCharset = "UTF-8";

            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                obj = Cplist2Html2(this._ResponseValue);

            return obj;
        }

        /// <summary>
        /// 处理详细内容
        /// </summary>
        private string Cplist2Html2(string p_html)
        {
            if (string.IsNullOrEmpty(p_html))
                return "";

            System.Text.StringBuilder builder = new System.Text.StringBuilder("");
            string pattern = @"title=.([\s\S]+?).>([\s\S]+?)\[文章列表\]</a>";
            Match m = Regex.Match(p_html, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (m.Success)
            {
                string str1 = m.Groups[1].Value;
                string str = m.Groups[2].Value;
                str = removeZ(str);
                str = str.Replace("<p mode=\"wrap\">", "");
                str = str.Replace("<br/>－－－－－<br/>", "");
                str = str.Replace("－－－－－<br/>", "");
                str = str.Replace("-49hz.cc", "");
                str = str.Replace("49hz.cc", "");
                str = str.Replace("lang.asp?", "cplist.aspx?act=listb&amp;");
                str = str.Replace("tlist.asp?", "cplist.aspx?act=viewb&amp;");
                str = str.Replace("&amp;&amp;", "&amp;");
                str = str.Replace("&amp;nbsp;", "&nbsp;");
                str = str.Replace("page=", "&amp;page=");
                str = str.Replace("(wap.11hz.cc)", "图片");

                str = Regex.Replace(str, @"<a.href='http://[\s\S]+?'>(\*)*[\s\S]+?\*</a>(<br/>)*", "");



                builder.Append(str1 + "!@#!@#" + str + "[文章列表]</a>");
            }
            return builder.ToString();
        }

        /// <summary>
        /// 取得神童免费资料区
        /// </summary>
        public string GetCplist2XML3(string gourl)
        {
            string obj = "";
            string url = "http://st6h.cc" + gourl + "";
            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = false;
            httpRequest.Fc.CacheTime = 1;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = "" + gourl + "";
            httpRequest.WebAsync.RevCharset = "UTF-8";
            //httpRequest.WebAsync.PostData = "theCityName=" + theCityName;
            //httpRequest.WebAsync.PostCharset = "UTF-8";

            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                obj = Cplist2Html3(this._ResponseValue);

            return obj;
        }

        /// <summary>
        /// 处理神童免费资料区
        /// </summary>
        private string Cplist2Html3(string p_html)
        {
            if (string.IsNullOrEmpty(p_html))
                return "";

            System.Text.StringBuilder builder = new System.Text.StringBuilder("");
            string pattern = @"<card.title=.([\s\S]+?).><p>([\s\S]+?)</p>";
            Match m = Regex.Match(p_html, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (m.Success)
            {
                string str1 = m.Groups[1].Value;
                string str = m.Groups[2].Value;
                str = Regex.Replace(str, @"<a.href=.[\s\S]+?.>[\s\S]+?</a>(<br/>)*", "");
                builder.Append(str1 + "!@#!@#" + str + "");
    
            }
            return builder.ToString();
        }
        /// <summary>
        /// Url去掉mpd值
        /// </summary>
        public static string removeZ(string p_strVal)
        {
            if (string.IsNullOrEmpty(p_strVal))
                return p_strVal;

            bool bl = false;
            if (p_strVal.IndexOf("&amp;") != -1)
            {
                bl = true;
                p_strVal = p_strVal.Replace("&amp;", "&");
            }
            string VE = ConfigHelper.GetConfigString("VE");
            string SID = ConfigHelper.GetConfigString("SID");
            p_strVal = Regex.Replace(p_strVal, @"mpd=[\w\.\d]*&{0,}", "&backurl=" + Utils.getPage(0) + "&" + VE + "=" + Utils.getstrVe() + "&" + SID + "=" + Utils.getstrU() + "", RegexOptions.IgnoreCase);
            if (bl)
            {
                p_strVal = p_strVal.Replace("&", "&amp;");
            }
            return p_strVal;
        }
    }
}