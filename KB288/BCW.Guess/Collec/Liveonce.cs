using System;
using System.Collections.Generic;
using System.Text;
using BCW.Common;
using System.Text.RegularExpressions;

namespace TPR.Collec
{
    /// <summary>
    /// 即时比分，足球走地抓取类
    /// </summary>
    public class Once
    {
        private string _ResponseValue = string.Empty;
        private string _CacheFolder = "~/Files/Cache/live/";

        #region 属性

        private bool _CacheUsed = false; //是否记录缓存/存TXT
        private int _CacheTime = 0;//缓存时间(分钟)

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
        public Once()
        {
        }

        //http://61.143.225.72/xml/change.xml
        //http://live2.titan007.com/vbsxml/change.xml
        //http://live2.titan007.com/vbsxml/change2.xml
        //http://basket.titan007.com/nba/change.xml
        //http://61.143.225.107/vbsxml/change2.xml足球走地，如限制了IP请用域名live6.titan007.com
        /// <summary>
        /// 取得即时足球比分XML
        /// </summary>
        public IList<TPR.Model.guess.BaList> GetOnce(out int p_recordCount)
        {

            IList<TPR.Model.guess.BaList> obj = new List<TPR.Model.guess.BaList>();
            p_recordCount = 0;
            string url = "http://bf.titan007.com/vbsxml/change2.xml?rd=" + new Random().Next(1, 9999) + "";
            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = "即时足球比分XML";

            httpRequest.WebAsync.RevCharset = "GB2312";
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                obj = OnceHtml(this._ResponseValue, out p_recordCount);

            return obj;
        }

        /// <summary>
        /// 取得即时篮球比分XML
        /// </summary>
        public IList<TPR.Model.guess.BaList> GetOnce1(out int p_recordCount)
        {
            IList<TPR.Model.guess.BaList> obj = new List<TPR.Model.guess.BaList>();
            p_recordCount = 0;
            string url = "http://basket.titan007.com/nba/change.xml?rd=" + new Random().Next(1, 9999) + "";
            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = "即时足球比分XML";

            httpRequest.WebAsync.RevCharset = "GB2312";
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                obj = OnceHtml1(this._ResponseValue, out p_recordCount);

            return obj;
        }

        /// <summary>
        /// 处理即时足球比分XML
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private IList<TPR.Model.guess.BaList> OnceHtml(string p_html, out int p_recordCount)
        {
            IList<TPR.Model.guess.BaList> objonce = new List<TPR.Model.guess.BaList>();
            p_recordCount = 0;
            if (!string.IsNullOrEmpty(p_html))
            {
                MatchCollection mc = Regex.Matches(p_html, @"<h>([\s\S].+?)</h>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                if (mc.Count > 0)
                {
                    for (int i = 0; i < mc.Count; i++)
                    {
                        TPR.Model.guess.BaList obj = new TPR.Model.guess.BaList();
                        string[] sTemp = { };
                        sTemp = mc[i].Groups[0].Value.Split("^".ToCharArray());

                        obj.p_id = Convert.ToInt32(sTemp[0].Replace("<h><![CDATA[", ""));
                        try
                        {
                            obj.p_result_temp1 = Convert.ToInt32(sTemp[2]);
                            obj.p_result_temp2 = Convert.ToInt32(sTemp[3]);
                        }
                        catch
                        {
                            obj.p_result_temp1 = 0;
                            obj.p_result_temp2 = 0;
                        }
                        objonce.Add(obj);
                        p_recordCount++;
                    }
                }
            }
            return objonce;
        }

        /// <summary>
        /// 处理即时篮球比分XML
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private IList<TPR.Model.guess.BaList> OnceHtml1(string p_html, out int p_recordCount)
        {
            IList<TPR.Model.guess.BaList> objonce = new List<TPR.Model.guess.BaList>();
            p_recordCount = 0;
            if (!string.IsNullOrEmpty(p_html))
            {
                MatchCollection mc = Regex.Matches(p_html, @"<h>([\s\S].+?)</h>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                if (mc.Count > 0)
                {
                    for (int i = 0; i < mc.Count; i++)
                    {
                        TPR.Model.guess.BaList obj = new TPR.Model.guess.BaList();
                        string[] sTemp = { };
                        sTemp = mc[i].Groups[0].Value.Split("^".ToCharArray());

                        obj.p_id = Convert.ToInt32(sTemp[0].Replace("<h><![CDATA[", ""));
                        try
                        {
                            obj.p_result_temp1 = Convert.ToInt32(sTemp[3]);
                            obj.p_result_temp2 = Convert.ToInt32(sTemp[4]);
                        }
                        catch
                        {
                            obj.p_result_temp1 = 0;
                            obj.p_result_temp2 = 0;
                        }
                        objonce.Add(obj);
                        p_recordCount++;
                    }
                }
            }
            return objonce;
        }


        /// <summary>
        /// 取得足球走地XML(取进球的分钟数集合)
        /// </summary>
        public IList<TPR.Model.guess.BaList> GetOnce2(int ID)
        {
            IList<TPR.Model.guess.BaList> obj = new List<TPR.Model.guess.BaList>();
            string url = "http://bf.titan007.com/detail/" + ID + ".htm";
            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = "走地XML" + ID + "";

            httpRequest.WebAsync.RevCharset = "GB2312";
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                obj = OnceHtml2(this._ResponseValue);

            return obj;
        }

        /// <summary>
        /// 处理足球走地XML(取进球的分钟数集合)
        /// </summary>
        /// <param name="p_xml">HTML文档</param>
        private IList<TPR.Model.guess.BaList> OnceHtml2(string p_xml)
        {
            IList<TPR.Model.guess.BaList> objonce = new List<TPR.Model.guess.BaList>();
            if (!string.IsNullOrEmpty(p_xml))
            {
                MatchCollection mc = Regex.Matches(p_xml, @"<tr class=font12 height=16 bgcolor=[\s\S]+?<img src=../bf_img/(1|7).gif>[\s\S]+?</td><td>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                if (mc.Count > 0)
                {
                    string html = string.Empty;
                    for (int i = 0; i < mc.Count; i++)
                    {
                        html += mc[i].Groups[0].Value;
                    }
                    //System.Web.HttpContext.Current.Response.Write(html);
                    //System.Web.HttpContext.Current.Response.End();
                    MatchCollection mc1 = Regex.Matches(html, @"<font color=#FFFFFF>([\s\S]+?)'</font>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    if (mc1.Count > 0)
                    {
                        for (int i = 0; i < mc1.Count; i++)
                        {
                            TPR.Model.guess.BaList obj = new TPR.Model.guess.BaList();
                            obj.p_once = mc1[i].Groups[1].Value;
                            objonce.Add(obj);
                        }
                    }
                }
            }
            return objonce;
        }

    }
}