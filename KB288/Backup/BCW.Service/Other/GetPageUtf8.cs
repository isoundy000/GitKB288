using System;
using System.Web;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;
using System.Net;
using BCW.Common;

namespace BCW.Service
{
    /// <summary>
    /// 抓取数据
    /// </summary>
    public class GetPageUtf8
    {

        /// <summary>
        /// 构造方法
        /// </summary>
        public GetPageUtf8()
        {

        }

        /// <summary>
        /// 取得数据
        /// </summary>
        public string GetPageUtf8XML(string url, string Start, string Over)
        {

            CookieContainer container = new CookieContainer();
            HttpWebResponse response = null;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            //编码方式
            string ForCharset = string.Empty;
            ForCharset = "utf-8";
            //代理IP
            //if (!string.IsNullOrEmpty(ProxyIP))
            //    request.Proxy = new WebProxy(ProxyIP);
            //else
            request.Proxy = null;
            request.Accept = "application/vnd.wap.xhtml+xml, application/vnd.wap.wmlc, application/xhtml+xml, image/gif, */*";
            //服务器网关信息
            request.Headers.Add("VIA:HTTP/1.1 HIHK-PS-WAP2-SV04-plat2 (infoX-WISG, Huawei Technologies)");
            //浏览器支持信息
            request.Headers.Add("ACCEPT_LANGUAGE:zs");
            request.Headers.Add("ACCEPT_CHARSET:utf-8,utf-16,iso-8859-1");
            //浏览器型号
            request.Headers.Add("USER_AGENT:MAUI WAP Browser");
            //连接方式
            request.Headers.Add("X_SOURCE_ID:cmwap");
            //连接类型
            request.Headers.Add("X_UP_BEARER_TYPE:GPRS");
            //这个?
            request.Headers.Add("BEARER_INDICATION:0");
            request.AllowAutoRedirect = true;

            //POST传递数据
            if (HttpContext.Current.Request.Form.ToString() != "")
            {
                request.Method = "POST";
                string para = HttpContext.Current.Request.Form.ToString();
                Encoding encoding = Encoding.GetEncoding(ForCharset);
                byte[] byte1 = encoding.GetBytes(para);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byte1.Length;
                Stream newStream = null;
                newStream = request.GetRequestStream();
                newStream.Write(byte1, 0, byte1.Length);
                newStream.Close();
            }

            request.CookieContainer = container;
            //超时时间
            request.Timeout = 100000;

            response = (HttpWebResponse)request.GetResponse();

            if (response.Cookies.Count > 0)
            {
                try
                {
                    container.Add(response.Cookies);
                }
                catch
                {
                    CookieCollection cookies = container.GetCookies(request.RequestUri);
                    foreach (Cookie cookie in cookies)
                    {
                        cookie.Expired = true;
                    }
                }
            }
            string str = string.Empty;
            Stream responseStream = ((HttpWebResponse)request.GetResponse()).GetResponseStream();
            str = new StreamReader(responseStream, Encoding.GetEncoding(ForCharset)).ReadToEnd();
            responseStream.Close();

            HttpContext.Current.Response.Charset = "utf-8";

            str = GetPageUtf8Html(str, Start, Over);

            return str;
        }

        /// <summary>
        /// 处理数据
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private string GetPageUtf8Html(string p_html, string Start, string Over)
        {
            if (string.IsNullOrEmpty(p_html))
                return "";

            string pattern = "" + Start + "([\\s\\S]+?)" + Over + "";
            Match m1 = Regex.Match(p_html, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            if (m1.Success)
            {
                string str = m1.Groups[1].Value;
   
                return str;
            }
            else
                return "";

        }
    }
}
