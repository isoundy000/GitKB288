using System;
using System.Collections.Generic;
using System.Text;
using BCW.Common;
using TPR.Model;

namespace TPR.Collec
{
    /// <summary>
    /// 篮球抓取类
    /// </summary>
    public class Basket
    {
        private string _ResponseValue = string.Empty;
        private string _CacheFolder = "~/Files/Cache/live/getlq/";

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
        public Basket()
        {
        }

        /// <summary>
        /// 取得篮球XML
        /// </summary>
        public IList<TPR.Model.guess.BaList> GetBasket(out int p_recordCount)
        {
            IList<TPR.Model.guess.BaList> obj = new List<TPR.Model.guess.BaList>();
            p_recordCount = 0;
            string url = "http://nba.titan007.com/odds/odds.xml";
            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = "篮球XML";

            httpRequest.WebAsync.RevCharset = "GB2312";
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                obj = BasketHtml(this._ResponseValue, out p_recordCount);

            return obj;
        }

        /// <summary>
        /// 处理篮球XML
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private IList<TPR.Model.guess.BaList> BasketHtml(string p_xml, out int p_recordCount)
        {
            IList<TPR.Model.guess.BaList> listBaskets = new List<TPR.Model.guess.BaList>();
            p_recordCount = 0;
            try
            {
                using (XmlReaderExtend reader = new XmlReaderExtend(p_xml))
                {
                    while (reader.ReadToFollowing("m"))
                    {
                        TPR.Model.guess.BaList obj = new TPR.Model.guess.BaList();
                        obj.p_id = Convert.ToInt32(reader.GetElementValue("i"));
                        obj.p_title = reader.GetElementValue("le").Split(",".ToCharArray())[2];
                        obj.p_TPRtime = Convert.ToDateTime(reader.GetElementValue("t").Split(",".ToCharArray())[0]);
                        obj.p_one = reader.GetElementValue("ta").Split(",".ToCharArray())[0];
                        obj.p_two = reader.GetElementValue("tb").Split(",".ToCharArray())[0];

                        if (Utils.Left(reader.GetElementValue("p"), 1) == "2")
                            obj.p_pn = 2;
                        else
                            obj.p_pn = 1;
                    



                        string[] saTemp = { };
                        saTemp = reader.GetElementValue("pl").Split(";".ToCharArray());
                        try
                        {
                            obj.p_pk = Convert.ToDecimal(saTemp[2].Split(",".ToCharArray())[1]);
                            obj.p_one_lu = Convert.ToDecimal(saTemp[2].Split(",".ToCharArray())[2]);
                            obj.p_two_lu = Convert.ToDecimal(saTemp[2].Split(",".ToCharArray())[3]);
                        }
                        catch
                        {
                            obj.p_pk = 0;
                            obj.p_one_lu = 0;
                            obj.p_two_lu = 0;
                        }
                        try
                        {
                            obj.p_dx_pk = Convert.ToDecimal(saTemp[7].Split(",".ToCharArray())[1]);
                            obj.p_big_lu = Convert.ToDecimal(saTemp[7].Split(",".ToCharArray())[2]);
                            obj.p_small_lu = Convert.ToDecimal(saTemp[7].Split(",".ToCharArray())[3]);
                        }
                        catch
                        {
                            obj.p_dx_pk = 0;
                            obj.p_big_lu = 0;
                            obj.p_small_lu = 0;
                        }
                        listBaskets.Add(obj);
                        p_recordCount++;
                    }
                    return listBaskets;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}