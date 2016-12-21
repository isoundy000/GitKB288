using System;
using System.Web;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using BCW.Common;
using TPR2.Model;

namespace TPR2.Collec
{
    /// <summary>
    /// 足球抓取类
    /// </summary>
    public class Foot
    {
        private string _ResponseValue = string.Empty;
        private string _CacheFolder = "~/Files/Cache/live/getzq/";

        #region 属性

        private bool _CacheUsed = false; //是否记录缓存/存TXT
        private int _CacheTime = 5;//缓存时间(分钟)

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
        public Foot()
        {
        }

        /// <summary>
        /// 取得足球让球盘XML
        /// </summary>
        public IList<TPR2.Model.guess.BaList> GetFoot(out int p_recordCount)
        {

            IList<TPR2.Model.guess.BaList> obj = new List<TPR2.Model.guess.BaList>();
            p_recordCount = 0;

            string url = "http://61.143.225.173:88/xml/odds99.xml";

            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = "足球让球盘XML";

            httpRequest.WebAsync.RevCharset = "GB2312";
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                obj = FootHtml(this._ResponseValue, out p_recordCount);

            return obj;
        }

        /// <summary>
        /// 处理足球让球盘XML
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private IList<TPR2.Model.guess.BaList> FootHtml(string p_xml, out int p_recordCount)
        {
            IList<TPR2.Model.guess.BaList> listFoots = new List<TPR2.Model.guess.BaList>();
            p_recordCount = 0;
            try
            {
                using (XmlReaderExtend reader = new XmlReaderExtend(p_xml))
                {

                    while (reader.ReadToFollowing("m"))
                    {
                        TPR2.Model.guess.BaList obj = new TPR2.Model.guess.BaList();
                        obj.p_id = Convert.ToInt32(reader.GetElementValue("i"));
                        obj.p_title = reader.GetElementValue("le").Split(",".ToCharArray())[0];
                        obj.p_TPRtime = Convert.ToDateTime(reader.GetElementValue("t").Split(",".ToCharArray())[0]);
                        obj.p_one = reader.GetElementValue("ta").Split(",".ToCharArray())[0];
                        obj.p_two = reader.GetElementValue("tb").Split(",".ToCharArray())[0];
                        try
                        {
                            obj.p_pn = Convert.ToInt32(reader.GetElementValue("p").Split(",".ToCharArray())[0]);
                        }
                        catch
                        {
                            obj.p_pn = 1;
                        }
                        string[] saTemp = { };
                        saTemp = reader.GetElementValue("pl").Split(";".ToCharArray());
                        try
                        {
                            //SB：1/明升：9
                            //if (Utils.GetTopDomain() == "tl88.cc" || Utils.GetTopDomain() == "168yy.cc")
                            //{
                                //obj.p_pk = Convert.ToDecimal(saTemp[9].Split(",".ToCharArray())[1]);
                                //obj.p_one_lu = Convert.ToDecimal(saTemp[9].Split(",".ToCharArray())[2]);
                                //obj.p_two_lu = Convert.ToDecimal(saTemp[9].Split(",".ToCharArray())[3]);
                            //}
                            //else
                            //{
                                obj.p_pk = Convert.ToDecimal(saTemp[1].Split(",".ToCharArray())[1]);
                                obj.p_one_lu = Convert.ToDecimal(saTemp[1].Split(",".ToCharArray())[2]);
                                obj.p_two_lu = Convert.ToDecimal(saTemp[1].Split(",".ToCharArray())[3]);
                            //}
                        }
                        catch
                        {
                            obj.p_pk = 0;
                            obj.p_one_lu = 0;
                            obj.p_two_lu = 0;
                        }

                        listFoots.Add(obj);
                        p_recordCount++;
                    }

                    return listFoots;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 取得足球大小盘XML
        /// </summary>
        public IList<TPR2.Model.guess.BaList> GetFootdx(out int p_recordCount)
        {
            IList<TPR2.Model.guess.BaList> obj = new List<TPR2.Model.guess.BaList>();
            p_recordCount = 0;
            string url = "http://61.143.225.173:88/xml/odds97.xml";
            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = "足球大小盘XML";

            httpRequest.WebAsync.RevCharset = "GB2312";
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                obj = FootdxHtml(this._ResponseValue, out p_recordCount);

            return obj;
        }


        /// <summary>
        /// 处理足球大小盘XML
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private IList<TPR2.Model.guess.BaList> FootdxHtml(string p_xml, out int p_recordCount)
        {
            IList<TPR2.Model.guess.BaList> listFootdxs = new List<TPR2.Model.guess.BaList>();
            p_recordCount = 0;
            try
            {
                using (XmlReaderExtend reader = new XmlReaderExtend(p_xml))
                {
                    while (reader.ReadToFollowing("m"))
                    {
                        TPR2.Model.guess.BaList obj = new TPR2.Model.guess.BaList();
                        obj.p_id = Convert.ToInt32(reader.GetElementValue("i"));
                        obj.p_TPRtime = Convert.ToDateTime(reader.GetElementValue("t").Split(",".ToCharArray())[0]);

                        string[] saTemp = { };
                        saTemp = reader.GetElementValue("pl").Split(";".ToCharArray());
                        try
                        {     
                            //SB：1/明升：9
                            //if (Utils.GetTopDomain() == "tl88.cc" || Utils.GetTopDomain() == "168yy.cc")
                            //{
                                //obj.p_dx_pk = Convert.ToDecimal(saTemp[9].Split(",".ToCharArray())[1]);
                                //obj.p_big_lu = Convert.ToDecimal(saTemp[9].Split(",".ToCharArray())[2]);
                                //obj.p_small_lu = Convert.ToDecimal(saTemp[9].Split(",".ToCharArray())[3]);
                            //}
                            //else
                            //{
                                obj.p_dx_pk = Convert.ToDecimal(saTemp[1].Split(",".ToCharArray())[1]);
                                obj.p_big_lu = Convert.ToDecimal(saTemp[1].Split(",".ToCharArray())[2]);
                                obj.p_small_lu = Convert.ToDecimal(saTemp[1].Split(",".ToCharArray())[3]);
                            //}
                        }
                        catch
                        {
                            obj.p_dx_pk = 0;
                            obj.p_big_lu = 0;
                            obj.p_small_lu = 0;
                        }
                        listFootdxs.Add(obj);
                        p_recordCount++;
                    }

                    return listFootdxs;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 取得足球标准盘XML
        /// </summary>
        public IList<TPR2.Model.guess.BaList> GetFootbz(out int p_recordCount)
        {
            IList<TPR2.Model.guess.BaList> obj = new List<TPR2.Model.guess.BaList>();
            p_recordCount = 0;
            string url = "http://61.143.225.173:88/xml/odds98.xml";
            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = "足球大小盘XML";
            httpRequest.WebAsync.RevCharset = "GB2312";
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                obj = FootbzHtml(this._ResponseValue, out p_recordCount);

            return obj;
        }

        /// <summary>
        /// 处理足球标准盘XML
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private IList<TPR2.Model.guess.BaList> FootbzHtml(string p_xml, out int p_recordCount)
        {
            IList<TPR2.Model.guess.BaList> listFootbzs = new List<TPR2.Model.guess.BaList>();
            p_recordCount = 0;
            try
            {
                using (XmlReaderExtend reader = new XmlReaderExtend(p_xml))
                {
                    while (reader.ReadToFollowing("m"))
                    {
                        TPR2.Model.guess.BaList obj = new TPR2.Model.guess.BaList();
                        obj.p_id = Convert.ToInt32(reader.GetElementValue("i"));
                        obj.p_TPRtime = Convert.ToDateTime(reader.GetElementValue("t").Split(",".ToCharArray())[0]);

                        string[] saTemp = { };
                        saTemp = reader.GetElementValue("pl").Split(";".ToCharArray());
                        try
                        {     
                            //SB：1/明升：9
                            //if (Utils.GetTopDomain() == "tl88.cc" || Utils.GetTopDomain() == "168yy.cc")
                            //{
                                //obj.p_bzs_lu = Convert.ToDecimal(saTemp[9].Split(",".ToCharArray())[4]);
                                //obj.p_bzp_lu = Convert.ToDecimal(saTemp[9].Split(",".ToCharArray())[5]);
                                //obj.p_bzx_lu = Convert.ToDecimal(saTemp[9].Split(",".ToCharArray())[6]);
                            //}
                            //else
                            //{
                                obj.p_bzs_lu = Convert.ToDecimal(saTemp[1].Split(",".ToCharArray())[4]);
                                obj.p_bzp_lu = Convert.ToDecimal(saTemp[1].Split(",".ToCharArray())[5]);
                                obj.p_bzx_lu = Convert.ToDecimal(saTemp[1].Split(",".ToCharArray())[6]);
                            //}
                        }
                        catch
                        {
                            obj.p_bzs_lu = 0;
                            obj.p_bzp_lu = 0;
                            obj.p_bzx_lu = 0;
                        }

                        listFootbzs.Add(obj);
                        p_recordCount++;
                    }
                    return listFootbzs;
                }
            }
            catch
            {
                return null;
            }
        }

    }
}
