using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using System.Net;
using BCW.Common;

namespace BCW.Service
{
    /// <summary>
    /// 抓取好彩一数据
    /// </summary>
    public class GetHc1
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
        public GetHc1()
        {

        }

        /// <summary>
        /// 取得好彩一 抓取网页数据百度彩票抓取
        /// </summary>
        public string GetNewsUrl()
        {
            string strUrl = "http://baidu.lecai.com/lottery/draw/view/555";
            string str = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strUrl);
            request.Timeout = 30000;
            request.AllowAutoRedirect = false;
            request.KeepAlive = false;
            request.ProtocolVersion = HttpVersion.Version11;
            request.Headers["Accept-Language"] = "zh-cn";
            request.UserAgent = "mozilla/4.0 (compatible; msie 6.0; windows nt 5.1; sv1; .net clr 1.0.3705; .net clr 2.0.50727; .net clr 1.1.4322)";
            try
            {
                WebResponse response = request.GetResponse();
                System.IO.Stream stream = response.GetResponseStream();
                System.IO.StreamReader sr = new System.IO.StreamReader(stream, Encoding.GetEncoding("UTF-8"));
                str = sr.ReadToEnd();
                stream.Close();
                sr.Close();
            }
            catch (Exception e)
            {
                str = e.ToString().Replace("\n", "<BR>");
            }
            return str;

        }
        /// <summary>
        /// 得到上期开奖期数
        /// </summary>

        public string GetStageS()
        {
            String s = GetNewsUrl();
            String stage = @"乐彩彩票网广东好彩一开奖结果查询。广东好彩一[\d]{7}";
            Match stages = Regex.Match(s, stage);
            String stageto = @"[\d]{7}";
            Match stageok = Regex.Match(stages.Value, stageto);
            if (stageok.Value != null)
            {
                return stageok.Value;
            }
            else
            {
                return "0";
            }
        }
        /// <summary>
        /// 得到最新期开奖期数
        /// </summary>
        public string GetStage()
        {
            String s = GetNewsUrl();
            String stage = @"\""phase""\:\""[\d]{7}";
            Match stages = Regex.Match(s, stage);
            String stageto = @"[\d]{7}";
            Match stageok = Regex.Match(stages.Value, stageto);
            if (stageok.Value != null)
            {
                return stageok.Value;
            }
            else
            {
                return "0";
            }
        }
        /// <summary>
        /// 得到开奖时间
        /// </summary>
        public string GetTime()
        {
            String s = GetNewsUrl();
            String stage = @"\""time_endsale""\:\""[\d]{4}-[\d]{2}-[\d]{1,2} [\d]{2}:[\d]{2}:[\d]{2}";
            Match stages = Regex.Match(s, stage);
            String stageto = @"[\d]{4}-[\d]{2}-[\d]{1,2} [\d]{2}:[\d]{2}:[\d]{2}";
            Match stageok = Regex.Match(stages.Value, stageto);
            if (stageok.Value != null)
            {
                return stageok.Value;
            }
            else
            {
                return "0";
            }
        }
        /// <summary>
        /// 得到上期开奖号码
        /// </summary>
        public string Getnumber()
        {
            String s = GetNewsUrl();
            String stage = @"开奖结果：[\d]{2}";
            Match stages = Regex.Match(s, stage);
            String stageto = @"[\d]{2}";
            Match stageok = Regex.Match(stages.Value, stageto);
            if (stageok.Value != null)
            {
                return stageok.Value;
            }
            else
            {
                return "0";
            }
        }
        /// <summary>
        /// 处理详细好彩一
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private string Hc1Html1(string p_html)
        {
            if (string.IsNullOrEmpty(p_html))
                return "";
            string pattern = "开奖号码：<label>([\\s\\S]+?)</label>";
            //string pattern = "<label><span>([\\s\\S]*?)<span></label>";
            // string pattern = "开奖号码：<label>([\\s\\S]+?)</label>";
            Match m1 = Regex.Match(p_html, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            if (m1.Success)
            {
                string str = m1.Groups[1].Value.Replace("<span>", "").Replace("</span>", "");
                return str;
            }
            else
                return "";

        }
   


    }
}
