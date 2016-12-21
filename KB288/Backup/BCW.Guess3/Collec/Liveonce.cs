using System;
using System.Collections.Generic;
using System.Text;
using BCW.Common;
using System.Text.RegularExpressions;

namespace TPR3.Collec
{
    /// <summary>
    /// 即时数据抓取类（红黄牌\比分变化类）
    /// </summary>
    public class Once
    {
        private string _ResponseValue = string.Empty;
        private string _CacheFolder = "~/Files/Cache/live/";

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
        public Once()
        {
        }

        /// <summary>
        /// 取得即时足球change
        /// </summary>
        public IList<TPR3.Model.guess.BaList> GetOnce(out int p_recordCount)
        {

            IList<TPR3.Model.guess.BaList> obj = new List<TPR3.Model.guess.BaList>();
            p_recordCount = 0;
            string url = "http://bf.titan007.com/vbsxml/change.xml";//20秒内变化数据 (实时更新) 
            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = "足球即时change";

            httpRequest.WebAsync.RevCharset = "GB2312";
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                obj = OnceHtml(this._ResponseValue, out p_recordCount);

            return obj;
        }

        /// <summary>
        /// 取得即时足球change2
        /// </summary>
        public IList<TPR3.Model.guess.BaList> GetOnce2(out int p_recordCount)
        {

            IList<TPR3.Model.guess.BaList> obj = new List<TPR3.Model.guess.BaList>();
            p_recordCount = 0;
            string url = "http://bf.titan007.com/vbsxml/change2.xml";//150秒内变化数据，两个格式完全一样
            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = "足球即时change2";

            httpRequest.WebAsync.RevCharset = "GB2312";
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                obj = OnceHtml(this._ResponseValue, out p_recordCount);

            return obj;
        }


        /// <summary>
        /// 处理即时足球change2
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private IList<TPR3.Model.guess.BaList> OnceHtml(string p_html, out int p_recordCount)
        {
            IList<TPR3.Model.guess.BaList> objonce = new List<TPR3.Model.guess.BaList>();
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

                        obj.p_id = Convert.ToInt32(sTemp[0].Replace("<h><![CDATA[", ""));
                        //即时红黄牌
                        try
                        {
                            obj.p_zred = Convert.ToInt32(sTemp[6]);
                            obj.p_kred = Convert.ToInt32(sTemp[7]);
                            obj.p_zyellow = Convert.ToInt32(sTemp[12]);
                            obj.p_kyellow = Convert.ToInt32(sTemp[13]);
                        }
                        catch
                        {
                        }
                        //即时全场比分
                        try
                        {
                            obj.p_result_temp1 = Convert.ToInt32(sTemp[2]);
                            obj.p_result_temp2 = Convert.ToInt32(sTemp[3]);
                        }
                        catch
                        {
                        }
                        //即时上半场比分
                        try
                        {
                            obj.p_halfresult_temp1 = Convert.ToInt32(sTemp[4]);
                            obj.p_halfresult_temp2 = Convert.ToInt32(sTemp[5]);
                        }
                        catch
                        {
                            obj.p_halfresult_temp1 = 0;
                            obj.p_halfresult_temp2 = 0;
                        }
                        objonce.Add(obj);
                        p_recordCount++;
                    }
                }
            }
            return objonce;
        }

    }
}