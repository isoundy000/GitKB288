using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using BCW.Common;

/// <summary>
/// 修改上证抓取地址
/// 黄国军 20160524
/// </summary>
namespace BCW.Service
{
    /// <summary>
    /// 抓取上证指数数据
    /// </summary>
    public class GetStk
    {
        private string _ResponseValue = string.Empty;
        private string _CacheFolder = "~/Files/Cache/";

        #region 属性

        private bool _CacheUsed = false; //是否记录缓存/存TXT
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
        public GetStk()
        {

        }

        ///// <summary>
        ///// 取得上证指数
        ///// </summary>
        //public string GetStkXML()
        //{
        //    string obj = "";
        //    string url = "http://wap.tzt.cn";
        //    HttpRequestCache httpRequest = new HttpRequestCache(url);
        //    httpRequest.Fc.CacheUsed = this._CacheUsed;
        //    httpRequest.Fc.CacheTime = this._CacheTime;
        //    httpRequest.Fc.CacheFolder = this._CacheFolder;
        //    httpRequest.Fc.CacheFile = "上证指数";
        //    httpRequest.WebAsync.RevCharset = "UTF-8";

        //    if (httpRequest.MethodGetUrl(out this._ResponseValue))
        //        obj = StkHtml(this._ResponseValue);

        //    return obj;
        //}


        ///// <summary>
        ///// 处理详细上证指数
        ///// </summary>
        ///// <param name="p_html">HTML文档</param>
        //private string StkHtml(string p_html)
        //{

        //    return p_html;

        //    //if (string.IsNullOrEmpty(p_html))
        //    //    return "";

        //    //string pattern = @"<strong>沪\:([\s\S]+?)<strong>深\:";
        //    //Match m1 = Regex.Match(p_html, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        //    //if (m1.Success)
        //    //{
        //    //    string str = m1.Groups[1].Value.Replace("</strong><br/>", "");
        //    //    return str;
        //    //}
        //    //else
        //    //    return "";

        //}

        /// <summary>
        /// 抓取一个网页源码
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string GetSourceTextByUrl(string url, string Encoding)
        {
            try
            {
                System.Net.WebRequest request = System.Net.WebRequest.Create(url);
                request.Timeout = 20000;
                System.Net.WebResponse response = request.GetResponse();

                System.IO.Stream resStream = response.GetResponseStream();
                System.IO.StreamReader sr = new System.IO.StreamReader(resStream, System.Text.Encoding.GetEncoding(Encoding));
                return sr.ReadToEnd();
            }
            catch
            {
                return "";
            }
        }


        /// <summary>
        /// 正则取局部数据
        /// </summary>
        /// <param name="p_url"></param>
        /// <param name="Start"></param>
        /// <param name="Over"></param>
        /// <returns></returns>
        public string GetStkXML()
        {
            //string p_html = GetSourceTextByUrl("http://wap.tzt.cn", "UTF-8");
            //指数查询规则：s_sh000001,s_sz399001,s_sz399106,s_sh000300：上证指数，深证成指，深证综指，沪深300
            //股票查询规则：sh601857,sz002230：中石油，科大讯飞（以sh开头代表沪市A股，以sz开头代表深市股票，后面是对应的股票代码）
            string p_html = GetSourceTextByUrl("http://hq.sinajs.cn/list=s_sh000001", "gb2312");
            if (string.IsNullOrEmpty(p_html))
                return "";
            string[] pattern = p_html.Split(',');
            decimal s = decimal.Parse(pattern[1]);
            return s.ToString().Split('.')[0];
        }

        public string GetStkXMLFormat()
        {
            string p_html = GetSourceTextByUrl( "http://hq.sinajs.cn/list=s_sh000001", "gb2312" );
            if( string.IsNullOrEmpty( p_html ) )
                return "";
            string[] pattern = p_html.Split( ',' );
            string s =  pattern[ 1 ] ;
            s =  decimal.Parse( s ) .ToString( "#0.000" );
            //return s.Substring(0, s.Length - 1);
            return s;
        }
    }
}
