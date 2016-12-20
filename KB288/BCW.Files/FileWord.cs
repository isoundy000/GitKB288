using System;
using System.Web;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using System.Net;
using System.IO;

namespace BCW.Files
{
    public class FileWord
    {
        /// <summary>
        /// 生成TXT文件
        /// </summary>
        /// <param name="PathName">生成全路径</param>
        /// <param name="Content">TXT内容</param>
        /// <param name="bl">为true时,如果文件存在则追加内容</param>
        /// <returns></returns>
        public static bool CreateTxt(string PathName, string Content, bool bl)
        {
            try
            {
                StreamWriter sw = new StreamWriter(HttpContext.Current.Server.MapPath(PathName), bl, Encoding.GetEncoding("utf-8"));
                sw.BaseStream.Seek(0, System.IO.SeekOrigin.End);
                sw.Write(Content);
                sw.Flush();
                sw.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// 读取TXT文件
        /// </summary>
        /// <param name="PathName">读取的文件路径</param>
        /// <returns>返回读取内容</returns>
        public static string ReadTxt(string PathName,string Encode)
        {
            string str = "";
            try
            {
                FileStream fs = new FileStream(HttpContext.Current.Server.MapPath(PathName), FileMode.Open);
                if (Encode == "")
                    Encode = "utf-8";
                StreamReader sr = new StreamReader(fs, Encoding.GetEncoding(Encode));
                str = sr.ReadToEnd();
                sr.Close();
                fs.Close();
            }
            catch
            { 
            
            }
            return str;
        }

    }
}