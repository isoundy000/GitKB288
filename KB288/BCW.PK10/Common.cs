using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;
namespace BCW.PK10
{
    public static class Common
    {
        #region 从网页中截取字符串（正则）
        public static string GetPageUtf8Html(string html,string start, string over)
        {
            if (string.IsNullOrEmpty(html))
                return "";
            else
                return GetValueFromStr(html, start, over);
        }
        public static string GetPageUtf8Html(string p_url, string start, string over, string Encoding)
        {
            string p_html = GetSourceTextByUrl(p_url, Encoding);
            return GetPageUtf8Html(p_html, start, over);
        }
        #endregion
        #region 从网页中读取源
        public static string GetSourceTextByUrl(string url, string Encoding)
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
        #endregion
        #region 截取单个字符串（正则）
        public static string GetValueFromStr(string str, string start, string over)
        {
            string result = "";
            //
            string pattern = start + "([\\s\\S]+?)" + over;
            Match m1 = Regex.Match(str, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (m1.Success)
            {
                result = m1.Groups[1].Value;
            }
            //
            return result;
        }
        //
        #endregion
        #region 截取所有匹配的字符串（正则）
        public static string[] GetValuesFromStr(string str, string start, string over)
        {
            string[] result = null;
            //
            string pattern = start + "([\\s\\S]+?)" + over;
            MatchCollection ms=Regex.Matches(str,pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if(ms.Count>0)
            {
                
                for (int i = 0; i < ms.Count; i++)
                {
                    result.SetValue(ms[i].Value, i);
                }

            }
            //
            return result;
        }
        //
        #endregion
        #region 取得单个匹配的字符串（正则）
        public static string GetValueFromStr(string str,string match)
        {
            string result = "";
            //
            string pattern = "("+match+")";
            Match m1 = Regex.Match(str, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (m1.Success)
            {
                result = m1.Groups[1].Value;
            }
            //
            return result;
        }
        //
        #endregion
        #region 取得所有匹配的字符串（正则）
        public static string[] GetValuesFromStr(string str, string match)
        {
            string[] result = null;
            //
            string pattern =  "("+match+")" ;
            MatchCollection ms = Regex.Matches(str, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (ms.Count > 0)
            {
                result = new string[ms.Count];
                for (int i = 0; i < ms.Count; i++)
                {
                    result[i] = ms[i].Value;
                }

            }
            //
            return result;
        }
        //
        #endregion
        //
        #region TXT文件
        /// <summary>
        /// 读取TXT文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static List<string> GetFromTxt(string file)
        {
            List<string> list = new List<string>();
            #region 读取Txt的每一行
            try
            {
                StreamReader reader = new StreamReader(file,Encoding.Default);
                String line;
                while ((line = reader.ReadLine()) != null)
                {
                    list.Add(line);
                }
                reader.Close();
            }
            catch(Exception ex)
            {
                throw new Exception("读取Txt文件失败：" + ex.Message);
            }
            #endregion
            return list;
        }
        #endregion
        #region XML文件

        #endregion
    }
}

