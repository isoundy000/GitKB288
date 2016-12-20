using System;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using BCW.Common;
namespace TPR3.Collec
{
    public class GetLiveover
    {
        private string _ResponseValue = string.Empty;
        private string _CacheFolder = "~/Files/Cache/live/over/";
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
        public GetLiveover()
        {
        }
        /// <summary>

        ///// <summary>
        ///// 取得篮球完场比分
        ///// </summary>
        //public IList<TPR3.Model.guess.BaList> Getbasketover(string Times, out int p_recordCount)
        //{
        //    IList<TPR3.Model.guess.BaList> objbasket = new List<TPR3.Model.guess.BaList>();
        //    p_recordCount = 0;

        //    string url = "http://bf.titan007.com/nba_date.aspx?date=" + Times + "";

        //    HttpRequestCache httpRequest = new HttpRequestCache(url);
        //    httpRequest.Fc.CacheUsed = this._CacheUsed;
        //    httpRequest.Fc.CacheTime = this._CacheTime;
        //    httpRequest.Fc.CacheFolder = this._CacheFolder;
        //    httpRequest.Fc.CacheFile = "basket";

        //    httpRequest.WebAsync.RevCharset = "GB2312";
        //    if (httpRequest.MethodGetUrl(out this._ResponseValue))
        //        objbasket = BasketLiveoverHtml(this._ResponseValue, out p_recordCount);

        //    return objbasket;
        //}

        ///// <summary>
        ///// 处理篮球完场比分
        ///// </summary>
        ///// <param name="p_html">HTML文档</param>
        //private IList<TPR3.Model.guess.BaList> BasketLiveoverHtml(string p_html, out int p_recordCount)
        //{
        //    IList<TPR3.Model.guess.BaList> objbasket = new List<TPR3.Model.guess.BaList>();
        //    p_recordCount = 0;
        //    if (!string.IsNullOrEmpty(p_html))
        //    {
        //        MatchCollection mc = Regex.Matches(p_html, @"<h>([\s\S].+?)</h>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        //        if (mc.Count > 0)
        //        {
        //            for (int i = 0; i < mc.Count; i++)
        //            {
        //                TPR3.Model.guess.BaList obj = new TPR3.Model.guess.BaList();
        //                string[] sTemp = { };
        //                sTemp = mc[i].Groups[0].Value.Split("^".ToCharArray());
        //                int Iswc = Convert.ToInt32(sTemp[5]);
        //                if (Iswc == -1)
        //                {
        //                    obj.p_id = Convert.ToInt32(sTemp[0].Replace("<h><![CDATA[", ""));
        //                    obj.p_TPRtime = Convert.ToDateTime(sTemp[4].Replace("<br>", ""));
        //                    try
        //                    {
        //                        obj.p_result_one = Convert.ToInt32(sTemp[11]);
        //                        obj.p_result_two = Convert.ToInt32(sTemp[12]);
        //                    }
        //                    catch
        //                    {
        //                        obj.p_result_one = -1;
        //                        obj.p_result_two = -1;
        //                    }
        //                }
        //                else
        //                {
        //                    obj.p_result_one = -1;
        //                    obj.p_result_two = -1;
        //                }
        //                objbasket.Add(obj);
        //                p_recordCount++;
        //            }
        //        }
        //    }
        //    return objbasket;
        //}

        /// <summary>
        /// 取得篮球完场比分(搜索)
        /// </summary>
        public IList<TPR3.Model.guess.BaList> Getbasketover(string p_one, out int p_recordCount)
        {
            IList<TPR3.Model.guess.BaList> objbasket = new List<TPR3.Model.guess.BaList>();
            p_recordCount = 0;
            string url = "http://bf.titan007.com/nba_date.aspx";

            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = "basket1111";

            httpRequest.WebAsync.RevCharset = "GB2312";
            httpRequest.WebAsync.PostData = "team=" + p_one;
            httpRequest.WebAsync.PostCharset = "GB2312";
            if (httpRequest.MethodPostUrl(out this._ResponseValue))
                objbasket = BasketLiveoverHtml(this._ResponseValue, out p_recordCount);

            return objbasket;
        }

        /// <summary>
        /// 处理篮球完场比分(搜索)
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private IList<TPR3.Model.guess.BaList> BasketLiveoverHtml(string p_html, out int p_recordCount)
        {
            IList<TPR3.Model.guess.BaList> objbasket = new List<TPR3.Model.guess.BaList>();
            p_recordCount = 0;
            if (!string.IsNullOrEmpty(p_html))
            {
                MatchCollection mc = Regex.Matches(p_html, @"<h>([\s\S].+?)</h>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                if (mc.Count > 0)
                {
                    for (int i = 0; i < mc.Count; i++)
                    {
                        TPR3.Model.guess.BaList obj = new TPR3.Model.guess.BaList();
                        string[] sTemp = { };
                        sTemp = mc[i].Groups[0].Value.Split("^".ToCharArray());
                        int Iswc = Convert.ToInt32(sTemp[5]);
                        if (Iswc == -1)
                        {
                            obj.p_id = Convert.ToInt32(sTemp[0].Replace("<h><![CDATA[", ""));
                            //obj.p_TPRtime = Convert.ToDateTime(sTemp[4].Replace("<br>", ""));
                            try
                            {
                                obj.p_result_one = Convert.ToInt32(sTemp[11]);
                                obj.p_result_two = Convert.ToInt32(sTemp[12]);
                            }
                            catch
                            {
                                obj.p_result_one = -1;
                                obj.p_result_two = -1;
                            }
                        }
                        else
                        {
                            obj.p_result_one = -1;
                            obj.p_result_two = -1;
                        }
                        objbasket.Add(obj);
                        p_recordCount++;
                    }
                }
            }
            return objbasket;
        }

        /// <summary>
        /// 取得足球完场比分
        /// </summary>
        public IList<TPR3.Model.guess.BaList> Getfootover(string Times, out int p_recordCount)
        {
            IList<TPR3.Model.guess.BaList> objfoot = new List<TPR3.Model.guess.BaList>();
            p_recordCount = 0;

            string url = "http://bf.titan007.com/Over_matchdate.aspx?matchdate=" + Times + "";
            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = "foot";

            httpRequest.WebAsync.RevCharset = "GB2312";
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                objfoot = FootLiveoverHtml(this._ResponseValue, out p_recordCount);

            return objfoot;
        }

        /// <summary>
        /// 处理足球完场比分
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private IList<TPR3.Model.guess.BaList> FootLiveoverHtml(string p_html, out int p_recordCount)
        {
            IList<TPR3.Model.guess.BaList> objfoot = new List<TPR3.Model.guess.BaList>();
            p_recordCount = 0;
            if (!string.IsNullOrEmpty(p_html))
            {
                MatchCollection mc = Regex.Matches(p_html, @"<tr class=font12 align=center bgColor=([\s\S]+?)>欧</a>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                if (mc.Count > 0)
                {
                    for (int i = 0; i < mc.Count; i++)
                    {
                        TPR3.Model.guess.BaList obj = new TPR3.Model.guess.BaList();
                        string sID = "0";
                        string pattern = @"<a href=javascript: onclick='analysis([\s\S]+?)'>析</a>";
                        Match m1 = Regex.Match(mc[i].Groups[0].Value, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                        if (m1.Success)
                        {
                            sID = m1.Groups[0].Value.Replace("<a href=javascript: onclick='analysis(", "").Replace(")'>析</a>", "");
                        }
                        string sTime = "2010-1-1 11:11:11";
                        string pattern2 = @"</td><td>([\s\S]+?)</td><td class=style1>";
                        Match m2 = Regex.Match(mc[i].Groups[0].Value, pattern2, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                        if (m2.Success)
                        {
                            sTime = m2.Groups[1].Value;
                        }
                        int result1 = -1;
                        int result2 = -1;
                        string pattern3 = @"</td><td class=style1 style=([\s\S]+?)(\d*-\d*)</td><td align=left>";
                        Match m3 = Regex.Match(mc[i].Groups[0].Value, pattern3, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                        if (m2.Success)
                        {
                            string sTemp = Regex.Replace(m3.Groups[2].Value, @"<.+?>", "", RegexOptions.IgnoreCase);
                            if (!string.IsNullOrEmpty(sTemp))
                            {
                                result1 = Convert.ToInt32(sTemp.Split('-')[0]);
                                result2 = Convert.ToInt32(sTemp.Split('-')[1]);

                            }
                        }

                        obj.p_TPRtime = Convert.ToDateTime(sTime);
                        obj.p_id = Convert.ToInt32(sID);
                        obj.p_result_one = result1;
                        obj.p_result_two = result2;
                        objfoot.Add(obj);
                        p_recordCount++;
                    }
                }
            }
            return objfoot;
        }
    }
}