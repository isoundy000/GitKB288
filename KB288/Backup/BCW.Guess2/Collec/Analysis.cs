using System;
using System.Web;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using BCW.Common;
using TPR2.Common;
using TPR2.Model;

namespace TPR2.Collec
{
    public class Analysis
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
        public Analysis()
        {
        }
        /// <summary>
        /// 取得分析页面
        /// </summary>
        public string GetAnalysisFoot(int Types, int eid, int gid)
        {

            string obj = string.Empty;
            string url = "";
            string by = System.Web.HttpContext.Current.Request["by"];
            string show = System.Web.HttpContext.Current.Request["show"];
            if (string.IsNullOrEmpty(by))
                by = "detail";
            if (!string.IsNullOrEmpty(show))
                show = "&show=" + show + "";

            if (Types == 0)
                url = "http://3g.8bo.com/wap/football/score/today.aspx?by=" + by + "&eid=" + eid + "" + show + "";
            else
                url = "http://3g.8bo.com/wap/football/score/history.aspx?by=" + by + "&eid=" + eid + "" + show + "";

            //代理地址
            string ProxyHost = ub.GetSub("SiteViewStatus", "/Controls/guess2.xml");

            if (ProxyHost != "" && ProxyHost.StartsWith("http://"))
            {
                url = url.Replace("&", "**");
                url = ProxyHost + "8boGet.aspx?url=" + url;
            }

            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = "FOOT8波XML_Analysis" + eid + "";

            httpRequest.WebAsync.RevCharset = "UTF-8";
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
            {
                if (Types == 0)
                    obj = AnalysisFootHtml(this._ResponseValue, gid);
                else
                    obj = AnalysisFootHtml2(this._ResponseValue, gid);
            }

            return obj;
        }

        /// <summary>
        /// 处理分析页面
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private string AnalysisFootHtml(string p_html, int gid)
        {
            if (string.IsNullOrEmpty(p_html))
                return "";

            string str = string.Empty;
            string pattern = @"刷新</a>\]</p><p>([\s\S]+?)<p>\[<a href=""today.aspx";
            Match m = Regex.Match(p_html, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (m.Success)
            {
                str = m.Groups[1].Value;
                str = str.Replace("<b class=\"score\">", "<b>");
                str = str.Replace("</p><p>", "<br />");
                str = str.Replace("<div style=\"margin:1em 0;\">", "<br />");
                str = str.Replace("</div>", "");
                str = str.Replace("<p>", "");
                str = str.Replace("</p>", "");
                str = str.Replace("，<anchor>返回<prev /></anchor>", "");
                str = Regex.Replace(str, @"<span class=""(\w+)"">", "");
                str = Regex.Replace(str, @"<div class=""(\w+)"">", "");
                str = Regex.Replace(str, @"<span title=""(.+?)"">", "");
                str = str.Replace("</div>", "");
                str = str.Replace("</span>", "");
                str = str.Replace("today.aspx?", "showGuess.aspx?act=analysis&amp;gid=" + gid + "&amp;");
                str = AddUrl(str);
            }

            return str;
        }


        /// <summary>
        /// 处理分析页面
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private string AnalysisFootHtml2(string p_html, int gid)
        {
            if (string.IsNullOrEmpty(p_html))
                return "";

            string str = string.Empty;
            string pattern = @"</b>\]\s</p><p>([\s\S]+?)<p>\[<a href=""history.aspx";
            Match m = Regex.Match(p_html, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (m.Success)
            {
      
                str = m.Groups[1].Value;
                str = str.Replace("<b class=\"score\">", "<b>");
                str = str.Replace("</p><p>", "<br />");
                str = str.Replace("<div style=\"margin:1em 0;\">", "<br />");
                str = str.Replace("</div>", "");
                str = str.Replace("<p>", "");
                str = str.Replace("</p>", "");
                str = str.Replace("，<anchor>返回<prev /></anchor>", "");
                str = Regex.Replace(str, @"<span class=""(\w+)"">", "");
                str = Regex.Replace(str, @"<div class=""(\w+)"">", "");
                str = Regex.Replace(str, @"<span title=""(.+?)"">", "");
                str = str.Replace("</div>", "");
                str = str.Replace("</span>", "");
                str = str.Replace("history.aspx?", "showGuess.aspx?act=analysis&amp;gid=" + gid + "&amp;");
                str = AddUrl(str);

            }

            return str;
        }

        /// <summary>
        /// 取得分析页面
        /// </summary>
        public string GetAnalysisBasket(int Types, int eid, int gid)
        {

            string obj = string.Empty;
            string url = "";
            if (Types == 0)
                url = "http://3g.8bo.com/wap/basketball/score/today.aspx?by=detail&eid=" + eid + "";
            else
                url = "http://3g.8bo.com/wap/basketball/score/history.aspx?by=detail&eid=" + eid + "";

            //代理地址
            string ProxyHost = ub.GetSub("SiteViewStatus", "/Controls/guess2.xml");

            if (ProxyHost != "" && ProxyHost.StartsWith("http://"))
            {
                url = url.Replace("&", "**");
                url = ProxyHost + "8boGet.aspx?url=" + url;
            }

            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = "BASKET8波XML_Analysis" + eid + "";

            httpRequest.WebAsync.RevCharset = "UTF-8";
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
            {
                if (Types == 0)
                    obj = AnalysisBasketHtml(this._ResponseValue, gid);
                else
                    obj = AnalysisBasketHtml2(this._ResponseValue, gid);

            }

            return obj;
        }

        /// <summary>
        /// 处理分析页面
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private string AnalysisBasketHtml(string p_html, int gid)
        {
            if (string.IsNullOrEmpty(p_html))
                return "";


            string str = string.Empty;
            string pattern = @"刷新</a>\]</p><p>([\s\S]+?)<p>\[<a href=""today.aspx";
            Match m = Regex.Match(p_html, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (m.Success)
            {
                str = m.Groups[1].Value;
                str = str.Replace("<b class=\"score\">", "<b>");
                str = str.Replace("</p><p>", "<br />");
                str = str.Replace("<p>", "");
                str = str.Replace("</p>", "");
                str = str.Replace("，<anchor>返回<prev /></anchor>", "");
                str = str.Replace("</div>", "");
                str = str.Replace("</span>", "");
            
            }

            return str;
        }

        /// <summary>
        /// 处理分析页面
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private string AnalysisBasketHtml2(string p_html, int gid)
        {
            if (string.IsNullOrEmpty(p_html))
                return "";


            string str = string.Empty;
            string pattern = @"</b>\]\s</p><p>([\s\S]+?)<p>\[<a href=""history.aspx";
            Match m = Regex.Match(p_html, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (m.Success)
            {
                str = m.Groups[1].Value;
                str = str.Replace("<b class=\"score\">", "<b>");
                str = str.Replace("</p><p>", "<br />");
                str = str.Replace("<p>", "");
                str = str.Replace("</p>", "");
                str = str.Replace("，<anchor>返回<prev /></anchor>", "");
                str = str.Replace("</div>", "");
                str = str.Replace("</span>", "");

            }

            return str;
        }



        /// <summary>
        /// Url加U值
        /// </summary>
        public static string AddUrl(string p_strVal)
        {
            if (string.IsNullOrEmpty(p_strVal))
                return p_strVal;

            string VE = ConfigHelper.GetConfigString("VE");
            string SID = ConfigHelper.GetConfigString("SID");

            p_strVal = p_strVal.Replace("by=detail", "by=detail&amp;backurl=" + Utils.getPage(0) + "&amp;" + VE + "=" + Utils.getstrVe() + "&amp;" + SID + "=" + Utils.getstrU() + "");
            p_strVal = p_strVal.Replace("by=analyse", "by=analyse&amp;backurl=" + Utils.getPage(0) + "&amp;" + VE + "=" + Utils.getstrVe() + "&amp;" + SID + "=" + Utils.getstrU() + "");
            p_strVal = p_strVal.Replace("by=integral", "by=integral&amp;backurl=" + Utils.getPage(0) + "&amp;" + VE + "=" + Utils.getstrVe() + "&amp;" + SID + "=" + Utils.getstrU() + "");
            p_strVal = p_strVal.Replace("show=custom", "show=custom&amp;backurl=" + Utils.getPage(0) + "&amp;" + VE + "=" + Utils.getstrVe() + "&amp;" + SID + "=" + Utils.getstrU() + "");
            p_strVal = p_strVal.Replace("show=all", "show=all&amp;backurl=" + Utils.getPage(0) + "&amp;" + VE + "=" + Utils.getstrVe() + "&amp;" + SID + "=" + Utils.getstrU() + "");
            p_strVal = p_strVal.Replace("by=asia", "by=asia&amp;backurl=" + Utils.getPage(0) + "&amp;" + VE + "=" + Utils.getstrVe() + "&amp;" + SID + "=" + Utils.getstrU() + "");
            p_strVal = p_strVal.Replace("by=size", "by=size&amp;backurl=" + Utils.getPage(0) + "&amp;" + VE + "=" + Utils.getstrVe() + "&amp;" + SID + "=" + Utils.getstrU() + "");

            return p_strVal;
        }
    }
}