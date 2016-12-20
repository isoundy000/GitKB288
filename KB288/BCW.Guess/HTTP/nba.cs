using System;
using System.Text.RegularExpressions;
using BCW.Common;
using TPR.Model.Http;
namespace TPR.Http
{
    /// <summary>
    /// NBA文字直播处理类
    /// </summary>
    public class GetNbaword
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
        public GetNbaword()
        {
        }

        /// <summary>
        /// 取得Nba文字直播列表
        /// </summary>
        public TPR.Model.Http.Nba Getnbalist()
        {
            TPR.Model.Http.Nba objnbalist = null;

            string url = "http://tx.com.cn/txnews/sports/nba/nbaLive.do?z=0XdNIkqeE7579";

            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = "Nba文字直播";

            httpRequest.WebAsync.RevCharset = "UTF-8";
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                objnbalist = LivenbalistHtml(this._ResponseValue);

            return objnbalist;
        }

        /// <summary>
        /// 处理Nba文字直播列表
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private TPR.Model.Http.Nba LivenbalistHtml(string p_html)
        {
            string strVal = p_html;
            TPR.Model.Http.Nba objnbalist = new TPR.Model.Http.Nba();

            if (!string.IsNullOrEmpty(strVal))
            {
                string pattern = "<img src=.http://img.tx.com.cn/img/tx/NBA/1_bfzb.gif. alt=.zb. /><br/>([\\s\\S]+?)<a href=.nbaLiveMatchType.do[\\s\\S]+?>篮球比分首页&gt;&gt;</a>";
                Match m1 = Regex.Match(strVal, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                if (m1.Success)
                {
                    string str = "";
                    str = m1.Groups[1].Value;
                    str = removeZ(str);
                    str = str.Replace("nbaLive.do?", Utils.getUrl("live.aspx?act=16"));
                    str = str.Replace("nbaLiving.do?ar1", Utils.getUrl("live.aspx?act=17") + "&amp;nbaid");
                    str = str.Replace("<br/>----------<br/>", "");
                    objnbalist.txtLivenbalist = str;
                }
            }

            return objnbalist;
        }

        /// <summary>
        /// 取得Nba文字直播页面
        /// </summary>
        public TPR.Model.Http.Nba Getnbapage(int nbaid)
        {
            TPR.Model.Http.Nba objnbapage = null;

            string url = "http://tx.com.cn/txnews/sports/nba/nbaLiving.do?z=0XdNIkqeE7579&ar1=" + nbaid + "";

            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = "Nba文字直播页面_" + nbaid + "";

            httpRequest.WebAsync.RevCharset = "UTF-8";
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                objnbapage = LivenbapageHtml(this._ResponseValue);

            return objnbapage;
        }

        /// <summary>
        /// 处理Nba文字直播页面
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private TPR.Model.Http.Nba LivenbapageHtml(string p_html)
        {
            string strVal = p_html;
            TPR.Model.Http.Nba objnbapage = new TPR.Model.Http.Nba();

            if (!string.IsNullOrEmpty(strVal))
            {
                string pattern = "<img src=.http://img.tx.com.cn/img/tx/NBA/1_bfzb.gif. alt=.zb.  /><br/>([\\s\\S]+?)<a href=.index.do[\\s\\S]+?>NBA</a>";
                Match m1 = Regex.Match(strVal, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                if (m1.Success)
                {
                    string str = "";
                    str = m1.Groups[1].Value;
                    //str = removeZ(str);
                    str = Regex.Replace(str, @"<a href=[\s\S]+?>文字直播</a>", "<#a href=\"" + Utils.getUrl("live.aspx?act=17&amp;nbaid=" + System.Web.HttpContext.Current.Request["nbaid"] + "") + "\">[刷新]</a><br /><#a href=\"" + Utils.getUrl("live.aspx?act=18&amp;nbaid=" + System.Web.HttpContext.Current.Request["nbaid"] + "") + "\">文字直播</a>");
                    str = Regex.Replace(str, @"<a href=./room/rindex.do[\s\S]+?>NBA直播聊天室&gt;&gt;</a>", "<#a href=\"" + Utils.getUrl("user/userTalk.aspx") + "\">NBA直播聊天室&gt;&gt;</a>");

                    str = Regex.Replace(str, @"<a href=[\s\S]+?>[\s\S]+?</a>", "");
                    str = str.Replace("<#a", "<a");
                    str = str.Replace("&gt;<br/>----------<br/><br/>", "");
                    str = str.Replace("&gt;<br/>----------<br/>", "");
                 
                    str = str.Replace("----------<br/>----------", "");
                    str = str.Replace("<br/><br/>", "");
                    str = Regex.Replace(str, @"半场[\s\S]+?\(主\)[\s\S]+?\(客\)[\s\S]+?次", "");
                    str = Regex.Replace(str, @"\(主\)[\s\S]+?\(客\)[\s\S]+?次", "");

                    objnbapage.txtLivenbapage = str;
                }
            }

            return objnbapage;
        }

        /// <summary>
        /// 取得Nba文字直播页面
        /// </summary>
        public TPR.Model.Http.Nba Getnbapage2(int nbaid)
        {
            TPR.Model.Http.Nba objnbapage = null;

            string url = "http://tx.com.cn/txnews/sports/nba/nbaLiving.do?z=0XdNIkqeE7579&ar1=" + nbaid + "";

            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = "Nba文字直播页面2_" + nbaid + "";

            httpRequest.WebAsync.RevCharset = "UTF-8";
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                objnbapage = Livenbapage2Html(this._ResponseValue);

            return objnbapage;
        }

        /// <summary>
        /// 处理Nba文字直播页面
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private TPR.Model.Http.Nba Livenbapage2Html(string p_html)
        {
            string strVal = p_html;
            TPR.Model.Http.Nba objnbapage = new TPR.Model.Http.Nba();

            if (!string.IsNullOrEmpty(strVal))
            {
                string pattern = "<img src=.http://img.tx.com.cn/img/tx/NBA/1_bfzb.gif. alt=.zb.  /><br/>([\\s\\S]+?)<a href=.index.do[\\s\\S]+?>NBA</a>";
                Match m1 = Regex.Match(strVal, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                if (m1.Success)
                {
                    string str = "";
                    str = m1.Groups[1].Value;
                    //str = removeZ(str);
                    str = Regex.Replace(str, @"<a href=[\s\S]+?>文字直播</a>", "<#a href=\"" + Utils.getUrl("live.aspx?act=20&amp;nbaid=" + System.Web.HttpContext.Current.Request["nbaid"] + "") + "\">[刷新]</a><br /><#a href=\"" + Utils.getUrl("live.aspx?act=18&amp;nbaid=" + System.Web.HttpContext.Current.Request["nbaid"] + "") + "\">文字直播</a>");
                    str = Regex.Replace(str, @"<a href=./room/rindex.do[\s\S]+?>NBA直播聊天室&gt;&gt;</a>", "<#a href=\"" + Utils.getUrl("user/userTalk.aspx") + "\">NBA直播聊天室&gt;&gt;</a>");

                    str = Regex.Replace(str, @"<a href=[\s\S]+?>[\s\S]+?</a>", "");
                    str = str.Replace("<#a", "<a");
                    str = str.Replace("&gt;<br/>----------<br/><br/>", "");
                    str = str.Replace("&gt;<br/>----------<br/>", "");

                    str = str.Replace("----------<br/>----------", "");

                    //str = Regex.Replace(str, @"\(主\)[\s\S]+?\(客\)[\s\S]+?次", "");

                    objnbapage.txtLivenbapage = str;
                }
            }

            return objnbapage;
        }


        /// <summary>
        /// 取得Nba文字直播内容
        /// </summary>
        public TPR.Model.Http.Nba Getnbaview(int nbaid, int pn)
        {
            TPR.Model.Http.Nba objnbaview = null;

            string url = "http://tx.com.cn/txnews/sports/nba/cs/nbaLiving.do?z=0XdNIkqeE7579&op=1&ar1=" + nbaid + "&pn=" + pn + "";
            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = "Nba文字直播内容_" + nbaid + "_" + pn + "";

            httpRequest.WebAsync.RevCharset = "UTF-8";
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                objnbaview = LivenbaviewHtml(this._ResponseValue);

            return objnbaview;
        }

        /// <summary>
        /// 处理Nba文字直播内容
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private TPR.Model.Http.Nba LivenbaviewHtml(string p_html)
        {
            string strVal = p_html;
            TPR.Model.Http.Nba objnbaview = new TPR.Model.Http.Nba();

            if (!string.IsNullOrEmpty(strVal))
            {
                string pattern = "<img src=.http://img.tx.com.cn/img/tx/NBA/1_wzzb.gif. alt=.zb.  /><br/>([\\s\\S]+?)<a href=.index.do[\\s\\S]+?>NBA</a>&gt;";
                Match m1 = Regex.Match(strVal, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                if (m1.Success)
                {
                    string str = "";
                    str = m1.Groups[1].Value;
                    str = removeZ(str);
                    str = str.Replace("nbaLiving.do?op=1&amp;ar1", Utils.getUrl("live.aspx?act=18") + "&amp;nbaid");
                    str = str.Replace("pn=", "page=");
                    str = str.Replace("&amp;gt;&amp;gt;", "").Replace("&amp;lt;&amp;lt;", "");
                    str = Regex.Replace(str, @"<a href=[\s\S]+?>比分直播</a>", "<a href=\"" + Utils.getUrl("live.aspx?act=18&amp;nbaid=" + System.Web.HttpContext.Current.Request["nbaid"] + "") + "\">[刷新]</a><br /><a href=\"" + Utils.getUrl("live.aspx?act=17&amp;nbaid=" + System.Web.HttpContext.Current.Request["nbaid"] + "") + "\">比分直播</a>");

                    objnbaview.txtLivenbaview = str;
                }
            }

            return objnbaview;
        }

        /// <summary>
        /// Url去掉z值
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
            p_strVal = Regex.Replace(p_strVal, @"z=[\w\d]*&{0,}", string.Empty, RegexOptions.IgnoreCase);
            if (bl)
            {
                p_strVal = p_strVal.Replace("&", "&amp;");
            }
            return p_strVal;
        }
    }
}