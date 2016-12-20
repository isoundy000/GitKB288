using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Security.Cryptography;

namespace BCW.Update
{
    public class ub
    {
        public Hashtable ds;
        public Hashtable dss;

        //加载网站配置
        public void Reload()
        {
            if (HttpContext.Current.Application["xml_wap"] == null)
                HttpContext.Current.Application["xml_wap"] = Read(File.ReadAllText(HttpContext.Current.Server.MapPath("~/Controls/wap.xml"), Encoding.UTF8));
            ds = (Hashtable)HttpContext.Current.Application["xml_wap"];
        }

        //加载子配置
        public void ReloadSub(string xmlPath)
        {
            if (HttpContext.Current.Application["" + xmlPath + ""] == null)
                HttpContext.Current.Application["" + xmlPath + ""] = Read(File.ReadAllText(HttpContext.Current.Server.MapPath(xmlPath), Encoding.UTF8));
            dss = (Hashtable)HttpContext.Current.Application["" + xmlPath + ""];
        }


        //网站配置取指定值
        public static string Get(string p_strVal)
        {
            ub XmlStr = new ub();
            XmlStr.Reload(); //加载网站配置
            try
            {
                return XmlStr.ds[p_strVal].ToString();
            }
            catch
            {
                return "未配置";
            }
        }

        //子配置取指定值
        public static string GetSub(string p_strVal, string xmlPath)
        {
            ub XmlStr = new ub();
            XmlStr.ReloadSub(xmlPath); //加载子配置
            try
            {
                return XmlStr.dss[p_strVal].ToString();
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 将一段XML数据转换为集合
        /// </summary>
        /// <param name="XmlString">XML内容</param>
        /// <returns></returns>
        public Hashtable Read(String XmlString)
        {
            Hashtable Return = new Hashtable();
            XmlReader re = XmlReader.Create(new StringReader(XmlString));

            while (re.Read())
                if (re.NodeType == XmlNodeType.Element)
                    if (re.Name == "set")
                        Return.Add(re.GetAttribute("name"), re.GetAttribute("value"));
            re.Close();
            return Return;
        }

        /// <summary>
        /// 将设置集合生成相应的XML
        /// </summary>
        /// <param name="ds">XML集合</param>
        /// <returns></returns>
        public String Post(Hashtable ds)
        {
            StringBuilder Return = new StringBuilder();
            XmlWriterSettings xml = new XmlWriterSettings();
            xml.Indent = true;
            xml.ConformanceLevel = ConformanceLevel.Auto;
            xml.IndentChars = "\t";
            XmlWriter wr = XmlWriter.Create(Return, xml);

            wr.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"utf-8\"");
            wr.WriteStartElement("data");
            foreach (object Jian in ds.Keys)
            {
                wr.WriteStartElement("set");
                wr.WriteAttributeString("name", Convert.ToString(Jian));
                wr.WriteAttributeString("value", Convert.ToString(ds[Jian]));
                wr.WriteEndElement();
            }

            wr.WriteEndElement();
            wr.Close();
            return Return.ToString();
        }
    }
}