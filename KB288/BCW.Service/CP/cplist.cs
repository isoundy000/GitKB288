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
    public class GetCplist
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
        public GetCplist()
        {

        }

        /// <summary>
        /// 取得列表
        /// </summary>
        public string GetCplistXML(int ID, int pageIndex)
        {
            string obj = "";
            string url = "http://xxln.net/list.asp?class=" + ID + "&page=" + pageIndex + "";
            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = "cplist" + ID + "_" + pageIndex + "";
            httpRequest.WebAsync.RevCharset = "UTF-8";
            //httpRequest.WebAsync.PostData = "theCityName=" + theCityName;
            //httpRequest.WebAsync.PostCharset = "UTF-8";

            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                obj = CplistHtml(this._ResponseValue);

            return obj;
        }


        /// <summary>
        /// 处理详细资料
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private string CplistHtml(string p_html)
        {
            if (string.IsNullOrEmpty(p_html))
                return "";

            System.Text.StringBuilder builder = new System.Text.StringBuilder("");
            string pattern = @"<card.title=.([\s\S]+?).><p>([\s\S]+?)搜索本栏目内容</a><br/>([\s\S]+?)---------<br/>";
            Match m = Regex.Match(p_html, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (m.Success)
            {
                string str1 = m.Groups[1].Value;
                string str = m.Groups[3].Value;
                str = removeZ(str);
                str = str.Replace("list.asp?", "cplist.aspx?act=list&amp;");
                str = str.Replace("view.asp?", "cplist.aspx?act=view&amp;");
                str = str.Replace("id", "pid");
                str = str.Replace("class", "id");
                str = str.Replace("&amp;&amp;", "&amp;");
                str = str.Replace("&amp;nbsp;", "&nbsp;");
                str = str.Replace("page=", "&amp;page=");
                str = Regex.Replace(str, @"<a.href=.http://xxln.net[\s\S]+?>[\s\S]+?</a>", "");
                builder.Append(str1 + "!@#!@#" + str);
            }

            return builder.ToString();
        }

        /// <summary>
        /// 取得内容
        /// </summary>
        public string GetCplistXML2(int pid, int ID, int pageIndex, int o)
        {
            string obj = "";
            string url = "http://xxln.net/view.asp?id=" + pid + "&class=" + ID + "&i=" + pageIndex + "&o=" + o + "";
            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = "cpview" + ID + "_" + pid + "_" + pageIndex + "_" + o + "";
            httpRequest.WebAsync.RevCharset = "UTF-8";
            //httpRequest.WebAsync.PostData = "theCityName=" + theCityName;
            //httpRequest.WebAsync.PostCharset = "UTF-8";

            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                obj = CplistHtml2(this._ResponseValue);

            return obj;
        }

        /// <summary>
        /// 处理详细内容
        /// </summary>
        private string CplistHtml2(string p_html)
        {
            if (string.IsNullOrEmpty(p_html))
                return "";

            System.Text.StringBuilder builder = new System.Text.StringBuilder("");
            string pattern = @"<card.title=.([\s\S]+?).><p>([\s\S]+?)<a href='http://xxln.net'>返回网站首页</a>";
            Match m = Regex.Match(p_html, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (m.Success)
            {
                string str1 = m.Groups[1].Value;
                string str = m.Groups[2].Value;
                str = removeZ(str);
                str = str.Replace("list.asp?", "cplist.aspx?act=list&amp;");
                str = str.Replace("view.asp?", "cplist.aspx?act=view&amp;");
                str = str.Replace("id", "pid");
                str = str.Replace("class", "id");
                str = str.Replace("&amp;&amp;", "&amp;");
                str = str.Replace("&amp;nbsp;", "&nbsp;");
                
                str = str.Replace("page=", "&amp;page=");
                str = str.Replace("o=", "&amp;o=");
                str = str.Replace("hkwwk.net", "");
                str = str.Replace("6hvip.at", "");
                str = Regex.Replace(str, @"<a.href=.http://[\s\S]+?>[\s\S]+?</a>", "");
                builder.Append(str1 + "!@#!@#" + str);
            }
            return builder.ToString();
        }

        /// <summary>
        /// 取得报码
        /// </summary>
        public string GetCplistXML3()
        {
            string obj = "";
            string url = "http://6hw.me/baoma.asp";//http://xxln.net/bm.asp
            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = false;
            httpRequest.Fc.CacheTime = 1;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = "bmbm";
            httpRequest.WebAsync.RevCharset = "UTF-8";
            //httpRequest.WebAsync.PostData = "theCityName=" + theCityName;
            //httpRequest.WebAsync.PostCharset = "UTF-8";

            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                obj = CplistHtml3(this._ResponseValue);

            return obj;
        }

        /// <summary>
        /// 处理详细报码
        /// </summary>
        private string CplistHtml3(string p_html)
        {
            if (string.IsNullOrEmpty(p_html))
                return "";

            System.Text.StringBuilder builder = new System.Text.StringBuilder("");
            string pattern = @"<p>([\s\S]+?)<br/>------------";
            Match m = Regex.Match(p_html, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (m.Success)
            {
                string str = m.Groups[1].Value;
                str = Regex.Replace(str, @"<a href=([\s\S]+?)>清除缓存即刻刷新</a>", "");
                str = str.Replace("6hw.me", Utils.GetTopDomain());
                str = str.Replace("<b>请见证六合网报码速度</b>", "");
                builder.Append(str);
    
            }
            return builder.ToString();
        }
        /// <summary>
        /// Url去掉Curl值
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
            p_strVal = Regex.Replace(p_strVal, @"Curl=[\w\.\d]*&{0,}", "&backurl=" + Utils.getPage(0) + "&" + VE + "=" + Utils.getstrVe() + "&" + SID + "=" + Utils.getstrU() + "", RegexOptions.IgnoreCase);
            if (bl)
            {
                p_strVal = p_strVal.Replace("&", "&amp;");
            }
            return p_strVal;
        }
    }
}