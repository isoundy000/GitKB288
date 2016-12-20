using System;
using System.Web;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using BCW.Common;

namespace BCW.Service
{
    public class Browser
    {
        /// <summary>
        /// 生成内网IP
        /// </summary>
        /// <returns></returns>
        private static string GetForIP()
        {
            int[] numArray = GetRandomNum(3, 1, 0xff);
            return string.Concat(new object[] { "10.", numArray[0], ".", numArray[1], ".", numArray[2] });
        }
        /// <summary>
        /// 生成随机手机号
        /// </summary>
        /// <returns></returns>
        public static string GetRandoMobile()
        {
            return ("86137" + GetRandom(8));
        }

        public static string GetRandom(int intLength)
        {
            int num = 9;
            string str = "";
            int num2 = intLength / num;
            int num3 = intLength % num;
            for (int i = 0; i < num2; i++)
            {
                str = str + GetRandomShort(num);
            }
            str = str + GetRandomShort(num3);
            int length = str.Length;
            if (length > intLength)
            {
                return str.Substring(0, intLength);
            }
            if (length < intLength)
            {
                string str2 = new string('0', intLength - length);
                str = str + str2;
            }
            return str;
        }

        public static int[] GetRandomNum(int num, int minValue, int maxValue)
        {
            Random ra = new Random((int)DateTime.Now.Ticks);
            int[] arrNum = new int[num];
            int tmp = 0;
            for (int i = 0; i <= (num - 1); i++)
            {
                tmp = ra.Next(minValue, maxValue);
                arrNum[i] = GetNum(arrNum, tmp, minValue, maxValue, ra);
            }
            return arrNum;
        }

        public static int GetNum(int[] arrNum, int tmp, int minValue, int maxValue, Random ra)
        {
            for (int i = 0; i <= (arrNum.Length - 1); i++)
            {
                if (arrNum[i] == tmp)
                {
                    tmp = ra.Next(minValue, maxValue);
                    GetNum(arrNum, tmp, minValue, maxValue, ra);
                }
            }
            return tmp;
        }
        private static string GetRandomShort(int intLength)
        {
            int minValue = 1;
            int maxValue = 10;
            for (int i = 1; i < intLength; i++)
            {
                minValue *= 10;
                maxValue *= 10;
            }
            Random random = new Random(Guid.NewGuid().GetHashCode());
            return random.Next(minValue, maxValue).ToString().Replace("-", "");
        }

        public static string GetFileExt(string FilePath)
        {
            if (string.IsNullOrEmpty(FilePath))
                return string.Empty;
            try
            {
                return FilePath.Substring(FilePath.LastIndexOf("."), FilePath.Length - FilePath.LastIndexOf("."));
            }
            catch
            {
                return "";
            }
        }

        public static string getGurlPage(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return "";
            }
            string gurl = HttpContext.Current.Request["gurl"];
            if (!string.IsNullOrEmpty(gurl))
            {
                url = gurl;
            }
            url = HttpContext.Current.Server.UrlDecode(url);

            url = Regex.Replace(url, @"[\s\S]+?gurl=(http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?)", "$1", RegexOptions.IgnoreCase);


            url = HttpContext.Current.Server.UrlEncode(url);

            url = Regex.Replace(url, "%25", "%");
            return url;
        }

        public string CreateUrl(string GetUrl)
        {
            string url = string.Empty;
            if (GetUrl == "")
            {
                url = Utils.getPageAll().ToLower();
            }
            else
            {
                url = GetUrl;
            }

            string gurl = HttpContext.Current.Request["gurl"];
            if (!string.IsNullOrEmpty(gurl))
            {
                url = gurl;
            }
            url = HttpContext.Current.Server.UrlDecode(url);
            
            url = Regex.Replace(url, @"[\s\S]+?gurl=(http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?)", "$1", RegexOptions.IgnoreCase);
            //return url;

            //取连接域名
            string furl = url.Replace("http://", "");
            string burl = furl.Split("/".ToCharArray())[0];

            string urlex = GetFileExt(url).Replace(".", "");
            //如果是文件则转向下载
            string fileex = "|swf|apk|sis|exe|sisx|jad|jar|mpkg|pgk|mis|rarz|txt|mtf|cab|tsk|hme|zip|rar|aif|app|rsc|dat|pak|dll|sav|db|gz|dskin|thm|utz|sdt|nth|mbm|mp3|mp4|mid|mmf|midi|amr|ogg|rm|au|acc|imy|wav|wmv|wma|seq|m4a|aac|ape|flac|3gp|3gpp|avi|mov|rmvb|3gp2|mms|jpg|gif|jpeg|png|bmp|lrc|umd|pdf|html|wml|chm|htm|doc|wps|ppt|";
            if (fileex.IndexOf(urlex.ToLower()) != -1)
            {
                HttpContext.Current.Response.Redirect(url);
            }
            string Forip = GetForIP();
            CookieContainer container = new CookieContainer();
            HttpWebResponse response = null;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            //读取系统设置Cookies
            string HeadType = string.Empty;
            string IsPic = string.Empty;
            string TimeOut = string.Empty;
            string Charset = string.Empty;

            if (HttpContext.Current.Request.Cookies["HeadAgentComment"] != null)
            {
                HeadType = HttpContext.Current.Request.Cookies["HeadAgentComment"]["HeadType"];
                IsPic = HttpContext.Current.Request.Cookies["HeadAgentComment"]["IsPic"];
                TimeOut = HttpContext.Current.Request.Cookies["HeadAgentComment"]["TimeOut"];
                Charset = HttpContext.Current.Request.Cookies["HeadAgentComment"]["Charset"];
            }

            //读取传递设置Cookies
            string ProxyIP = string.Empty;
            string Agent = string.Empty;
            string Via = string.Empty;
            string Accept = string.Empty;
            string IsMobile = string.Empty;
            string IsForIP = string.Empty;

            if (HttpContext.Current.Request.Cookies["AgentComment"] != null)
            {
                ProxyIP = HttpContext.Current.Request.Cookies["AgentComment"]["ProxyIP"];
                Agent = HttpContext.Current.Request.Cookies["AgentComment"]["Agent"];
                Via = HttpContext.Current.Request.Cookies["AgentComment"]["Via"];
                Accept = HttpContext.Current.Request.Cookies["AgentComment"]["Accept"];
                IsMobile = HttpContext.Current.Request.Cookies["AgentComment"]["IsMobile"];
                IsForIP = HttpContext.Current.Request.Cookies["AgentComment"]["IsForIP"];
            }
            //编码方式
            string ForCharset = string.Empty;
            if (Charset == "0")
                ForCharset = "utf-8";
            else if (Charset == "1")
                ForCharset = "gb2312";
            else
                ForCharset = "utf-8";

            if (!string.IsNullOrEmpty(ProxyIP))
                request.Proxy = new WebProxy(ProxyIP);
            else
                request.Proxy = null;

            if (!string.IsNullOrEmpty(Accept))
                request.Accept = Accept;
            else
                request.Accept = "application/vnd.wap.xhtml+xml, application/vnd.wap.wmlc, application/xhtml+xml, image/gif, */*";

            //服务器网关信息
            if (!string.IsNullOrEmpty(Via))
                request.Headers.Add("VIA:" + Via + ")");
            else
                request.Headers.Add("VIA:HTTP/1.1 HIHK-PS-WAP2-SV04-plat2 (infoX-WISG, Huawei Technologies)");

            //浏览器支持信息
            request.Headers.Add("ACCEPT_LANGUAGE:zs");
            request.Headers.Add("ACCEPT_CHARSET:utf-8,utf-16,iso-8859-1");

            //浏览器型号
            if (!string.IsNullOrEmpty(Agent))
                request.Headers.Add("USER_AGENT:" + Agent + "");
            else
                request.Headers.Add("USER_AGENT:MAUI WAP Browser");

            //内网IP
            if (!string.IsNullOrEmpty(IsForIP) && IsForIP == "0")
            {
                request.Headers.Add("X_FORWARDED_FOR:" + Forip);
            }

            //手机号码
            if (!string.IsNullOrEmpty(IsMobile) && IsMobile == "0")
            {
                request.Headers.Add("X_UP_CALLING_LINE_ID:" + GetRandoMobile());
            }

            //连接方式
            request.Headers.Add("X_SOURCE_ID:cmwap");
            //连接类型
            request.Headers.Add("X_UP_BEARER_TYPE:GPRS");
            //手机综合信息
            string strInfo = "GPRS," + GetRandoMobile() + "," + Forip + ",cmwap,unsecured";
            request.Headers.Add("X_NETWORK_INFO:" + strInfo);
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
            if (!string.IsNullOrEmpty(TimeOut))
                request.Timeout = Convert.ToInt32(TimeOut);
            else
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
 
            str = Regex.Replace(str, @"<a[^>]*href=.([^>]*)(""|')\s*>(.*?)</a>", @"<a href=""browser.aspx?GUrl=http://[burl]/$1"">$3</a>", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"<go[^>]*href=.([^>]*)(""|')\s*>", @"<go href=""browser.aspx?GUrl=http://[burl]/$1"">", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"(<form[^>]*action=.)([^>]*)(""|')\s*>", @"$1browser.aspx?GUrl=http://[burl]/$2"">", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"(?:src\s*=)\s*.(.+?\.(gif|jpg|bmp|jpeg|png|tiff))(""|')", @"src=$3http://[burl]/$1$3", RegexOptions.IgnoreCase);
            str = str.Replace("[burl]", burl);
            str = str.Replace("http://" + burl + "/http://", "http://");
            str = str.Replace("http://" + burl + "//", "http://" + burl + "/");
            str = str.Replace("browser.aspx?", "browser.aspx?ve=" + Utils.getstrVe() + "&amp;u=" + Utils.getstrU() + "&amp;");
            //关闭图片显示
            if (IsPic == "1")
                str = Regex.Replace(str, @"<(img)[^>]*>|<\/(img)>", "", RegexOptions.IgnoreCase);
            return str;
        }
    }
}
