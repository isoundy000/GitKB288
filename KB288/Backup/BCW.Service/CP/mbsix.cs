using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using BCW.Common;

namespace BCW.Service
{
    public class Getmbsix
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
        public Getmbsix()
        {

        }
        //酷网NTUyNTU3fDE4MjViYWRmYjA2ZDI4NDVhNzU5MGNjNGQwYTkxMzQ3
        //土豪NTUyNTU3fDE4MjViYWRmYjA2ZDI4NDVhNzU5MGNjNGQwYTkxMzQ3
        /// <summary>
        /// 取得列表
        /// </summary>
        public string GetmbsixXML()
        {
            string obj = "";
            string url = "http://game.mingban.hk/game49_index.action?z=NTUyNTU3fDE4MjViYWRmYjA2ZDI4NDVhNzU5MGNjNGQwYTkxMzQ3";
            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = "mbsix";
            httpRequest.WebAsync.RevCharset = "UTF-8";
            //httpRequest.WebAsync.PostData = "theCityName=" + theCityName;
            //httpRequest.WebAsync.PostCharset = "UTF-8";

            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                obj = mbsixHtml(this._ResponseValue);

            return obj;
        }
        /// <summary>
        /// 处理详细资料
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private string mbsixHtml(string p_html)
        {
            if (string.IsNullOrEmpty(p_html))
                return "";

            System.Text.StringBuilder builder = new System.Text.StringBuilder("");
            string pattern = @"数据分析-提高中奖几率<br/>([\s\S]+?)<a href=""/game_49_index.action";
            Match m = Regex.Match(p_html, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (m.Success)
            {
                string str1 = m.Groups[1].Value;
                str1 = str1.Replace("?z=NTUyNTU3fDE4MjViYWRmYjA2ZDI4NDVhNzU5MGNjNGQwYTkxMzQ3&amp;", "&amp;");
                str1 = str1.Replace("?z=NTUyNTU3fDE4MjViYWRmYjA2ZDI4NDVhNzU5MGNjNGQwYTkxMzQ3", "");
                str1 = str1.Replace("game_49_drawhistory.action", Utils.getUrl("cplist2.aspx?act=kzlist"));

                str1 = str1.Replace("game49_unshow.action", Utils.getUrl("cplist2.aspx?act=nolist"));
                str1 = str1.Replace("game49_cycle.action", Utils.getUrl("cplist2.aspx?act=fxlist"));

                str1 = str1.Replace("/game49_haoma.action", Utils.getUrl("/bbs/game/six49.aspx?act=cardso"));
                str1 = str1.Replace("gameUnshowType", "ptype");
                str1 = str1.Replace("orderType=desc", "showtype=1");
                str1 = str1.Replace("gameType", "ptype");
                str1 = str1.Replace("http://j.mingban.net/image/huo.png", "/files/huo.png");



                builder.Append(str1);
            }

            return builder.ToString();
        }

        /// <summary>
        /// 取得列表
        /// </summary>
        public string GetmbsixXML2(int page)
        {
            string obj = "";
            string url = "http://game.mingban.hk/game_49_drawhistory.action?z=NTUyNTU3fDE4MjViYWRmYjA2ZDI4NDVhNzU5MGNjNGQwYTkxMzQ3&page=" + page + "";
            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = "mbsix2_" + page + "";
            httpRequest.WebAsync.RevCharset = "UTF-8";
            //httpRequest.WebAsync.PostData = "theCityName=" + theCityName;
            //httpRequest.WebAsync.PostCharset = "UTF-8";

            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                obj = mbsixHtml2(this._ResponseValue);

            return obj;
        }
        /// <summary>
        /// 处理详细资料
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private string mbsixHtml2(string p_html)
        {
            if (string.IsNullOrEmpty(p_html))
                return "";

            System.Text.StringBuilder builder = new System.Text.StringBuilder("");
            string pattern = @"〓虚拟竞猜开奖历史〓<br/>([\s\S]+?)<br/>[\d]{2,5}条";
            Match m = Regex.Match(p_html, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (m.Success)
            {
                string str1 = m.Groups[1].Value;
                str1 = str1.Replace("?z=NTUyNTU3fDE4MjViYWRmYjA2ZDI4NDVhNzU5MGNjNGQwYTkxMzQ3&amp;", "&amp;");
                str1 = str1.Replace("?z=NTUyNTU3fDE4MjViYWRmYjA2ZDI4NDVhNzU5MGNjNGQwYTkxMzQ3", "");
                str1 = str1.Replace("game_49_drawhistory.action", Utils.getUrl("cplist2.aspx?act=kzlist"));

                str1 = str1.Replace("game49_unshow.action", Utils.getUrl("cplist2.aspx?act=nolist"));
                str1 = str1.Replace("game49_cycle.action", Utils.getUrl("cplist2.aspx?act=fxlist"));
                builder.Append(str1);
            }

            return builder.ToString();
        }

        /// <summary>
        /// 取得列表
        /// </summary>
        public string GetmbsixXML3(int ptype, int showtype)
        {
            string obj = "";
            string url = "";
            if (showtype == 0)
            {
                url = "http://game.mingban.hk/game49_unshow.action?z=NTUyNTU3fDE4MjViYWRmYjA2ZDI4NDVhNzU5MGNjNGQwYTkxMzQ3&gameUnshowType=" + ptype + "";
            }
            else
            {
                url = "http://game.mingban.hk/game49_unshow.action?z=NTUyNTU3fDE4MjViYWRmYjA2ZDI4NDVhNzU5MGNjNGQwYTkxMzQ3&gameUnshowType=" + ptype + "&orderType=desc";

            }
            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = "mbsix3_" + ptype + "_" + showtype + "";
            httpRequest.WebAsync.RevCharset = "UTF-8";
            //httpRequest.WebAsync.PostData = "theCityName=" + theCityName;
            //httpRequest.WebAsync.PostCharset = "UTF-8";

            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                obj = mbsixHtml3(this._ResponseValue);

            return obj;
        }

        /// <summary>
        /// 处理详细资料
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private string mbsixHtml3(string p_html)
        {
            if (string.IsNullOrEmpty(p_html))
                return "";

            System.Text.StringBuilder builder = new System.Text.StringBuilder("");
            string pattern = @"<br/>=======<br/>([\s\S]+?)<a href=""/game_49_index.action";
            Match m = Regex.Match(p_html, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (m.Success)
            {
                string str1 = m.Groups[1].Value;
                str1 = str1.Replace("?z=NTUyNTU3fDE4MjViYWRmYjA2ZDI4NDVhNzU5MGNjNGQwYTkxMzQ3&amp;", "&amp;");
                str1 = str1.Replace("?z=NTUyNTU3fDE4MjViYWRmYjA2ZDI4NDVhNzU5MGNjNGQwYTkxMzQ3", "");
                str1 = str1.Replace("game_49_drawhistory.action", Utils.getUrl("cplist2.aspx?act=kzlist"));

                str1 = str1.Replace("game49_unshow.action", Utils.getUrl("cplist2.aspx?act=nolist"));
                str1 = str1.Replace("game49_cycle.action", Utils.getUrl("cplist2.aspx?act=fxlist"));

                str1 = str1.Replace("gameUnshowType", "ptype");
                str1 = str1.Replace("orderType=desc", "showtype=1");
                str1 = str1.Replace("gameType", "ptype");
                str1 = str1.Replace("perPageNumb", "pn");
                builder.Append(str1);
            }

            return builder.ToString();
        }
        /// <summary>
        /// 取得列表
        /// </summary>
        public string GetmbsixXML4(int ptype, int pn)
        {
            string obj = "";
            string url = "";

            url = "http://game.mingban.hk/game49_cycle.action?z=NTUyNTU3fDE4MjViYWRmYjA2ZDI4NDVhNzU5MGNjNGQwYTkxMzQ3&gameType=" + ptype + "&perPageNumb=" + pn + "";

            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = "mbsix4_" + ptype + "_"+pn+"";
            httpRequest.WebAsync.RevCharset = "UTF-8";
            //httpRequest.WebAsync.PostData = "theCityName=" + theCityName;
            //httpRequest.WebAsync.PostCharset = "UTF-8";

            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                obj = mbsixHtml4(this._ResponseValue);

            return obj;
        }

        /// <summary>
        /// 处理详细资料
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private string mbsixHtml4(string p_html)
        {
            if (string.IsNullOrEmpty(p_html))
                return "";

            System.Text.StringBuilder builder = new System.Text.StringBuilder("");
            string pattern = @"<br/>=======<br/>([\s\S]+?)<a href=""/game_49_index.action";
            Match m = Regex.Match(p_html, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (m.Success)
            {
                string str1 = m.Groups[1].Value;


                string pattern1 = @"查询多期:\(最大200期\)<br/>[\s\S]+?查询</anchor><br/>";
                str1 = Regex.Replace(str1, pattern1, "[@搜索调用]");

                str1 = str1.Replace("?z=NTUyNTU3fDE4MjViYWRmYjA2ZDI4NDVhNzU5MGNjNGQwYTkxMzQ3&amp;", "&amp;");
                str1 = str1.Replace("?z=NTUyNTU3fDE4MjViYWRmYjA2ZDI4NDVhNzU5MGNjNGQwYTkxMzQ3", "");
                str1 = str1.Replace("game_49_drawhistory.action", Utils.getUrl("cplist2.aspx?act=kzlist"));

                str1 = str1.Replace("game49_unshow.action", Utils.getUrl("cplist2.aspx?act=nolist"));
                str1 = str1.Replace("game49_cycle.action", Utils.getUrl("cplist2.aspx?act=fxlist"));

                str1 = str1.Replace("gameUnshowType", "ptype");
                str1 = str1.Replace("orderType=desc", "showtype=1");
                str1 = str1.Replace("gameType", "ptype");
                str1 = str1.Replace("perPageNumb", "pn");

                
                builder.Append(str1);
            }

            return builder.ToString();
        }
    }
}
