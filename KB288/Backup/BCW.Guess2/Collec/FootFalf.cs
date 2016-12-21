using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using BCW.Common;
using TPR2.Common;
using TPR2.Model;
using LitJson;

namespace TPR2.Collec
{
    /// <summary>
    /// 足球半场抓取类
    /// </summary>
    public class FootFalf
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
        public FootFalf()
        {
        }

        ///// <summary>
        ///// 取得足球半场指数
        ///// </summary>
        //public string GetFootFalf(int p_id)
        //{
        //    string obj = string.Empty;
        //    string url = "http://121.14.145.206:8031/oddslist.aspx?id=" + p_id + "&companyid=1&ot=H&y=2014&typeid=2";

        //    HttpRequestCache httpRequest = new HttpRequestCache(url);
        //    httpRequest.Fc.CacheUsed = this._CacheUsed;
        //    httpRequest.Fc.CacheTime = this._CacheTime;
        //    httpRequest.Fc.CacheFolder = this._CacheFolder;
        //    httpRequest.Fc.CacheFile = "足球数据XML_" + p_id + "";

        //    httpRequest.WebAsync.RevCharset = "utf-8";
        //    if (httpRequest.MethodGetUrl(out this._ResponseValue))
        //        obj = FootFalfHtml(this._ResponseValue);

        //    return obj;
        //}

        /// <summary>
        /// 取得足球半场指数
        /// </summary>
        public string GetFootFalf(int p_id, bool Iszd)
        {
            string obj = string.Empty;
            string url = "http://pl.8bobf.com:23002/oddslist.aspx?id=" + p_id + "&companyid=1&ot=H&y=2014&typeid=2";

            //半场走地
            if (Iszd)
            {
                url = "http://pl.8bobf.com:23002/oddszdlist.aspx?id=" + p_id + "&companyid=1&ot=H&y=2014&typeid=4";
            }

            //代理地址
            string ProxyHost = ub.GetSub("SiteViewStatus", "/Controls/guess2.xml");

            if (ProxyHost != "" && ProxyHost.StartsWith("http://"))
            {
                url = url.Replace("&", "**");
                url = ProxyHost + "8boGet.aspx?url=" + url;
            }

            string html = new TPR2.Collec.Footbo().GetHtml(url, "UTF-8");
            if (html != "")
                obj = FootFalfHtml(html);

            //HttpRequestCache httpRequest = new HttpRequestCache(url);
            //httpRequest.Fc.CacheUsed = this._CacheUsed;
            //httpRequest.Fc.CacheTime = this._CacheTime;
            //httpRequest.WebAsync.HttpRequest.Timeout = 5;
            //httpRequest.Fc.CacheFolder = this._CacheFolder;
            //httpRequest.Fc.CacheFile = "足球数据XML_" + p_id + "";
            //httpRequest.WebAsync.RevCharset = "utf-8";
            //if (httpRequest.MethodGetUrl(out this._ResponseValue))
            //    obj = FootFalfHtml(this._ResponseValue);
            if (obj == "")
            {
                obj = url;
            }
            return obj;
        }

        /// <summary>
        /// 处理足球半场指数
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private string FootFalfHtml(string p_html)
        {

            if (string.IsNullOrEmpty(p_html))
                return "";

            string p_str = "";
            string strpattern = @"<title>([\s\S]+?)</head>";
            Match mtitle = Regex.Match(p_html, strpattern, RegexOptions.Compiled);
            if (mtitle.Success)
            {
                p_str = mtitle.Groups[1].Value;
            }

            return p_str;
        }


        /// <summary>
        /// 取得足球XML
        /// </summary>
        public string GetFootbolist(int Page)
        {
            string obj = string.Empty;

            string url = string.Empty;

            url = "http://3g.8bo.com/3g/football/score/today.aspx?by=event&st=allEvents&pagesize=15&page=" + Page + "";//所有比赛

            //代理地址
            string ProxyHost = ub.GetSub("SiteViewStatus", "/Controls/guess2.xml");

            if (ProxyHost != "" && ProxyHost.StartsWith("http://"))
            {
                url = url.Replace("&", "**");
                url = ProxyHost + "8boGet.aspx?url=" + url;
            }

            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = "FOOT8波XML" + Page + "";
            httpRequest.WebAsync.RevCharset = "UTF-8";
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
            {
                if (this._ResponseValue.Contains(">尾页</a>]"))
                    obj = FootbolistHtml(this._ResponseValue) + "#NEXT#";
                else
                    obj = FootbolistHtml(this._ResponseValue);
            }

            return obj;
        }

        /// <summary>
        /// 处理足球XML
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private string FootbolistHtml(string p_html)
        {
            if (string.IsNullOrEmpty(p_html))
                return "";

            string str = string.Empty;
            string pattern = @"<table cellspacing=""0"" cellpadding=""0"" border=""0"" class=""events odds"">([\s\S]+?)</table>";
            Match m = Regex.Match(p_html, pattern, RegexOptions.IgnoreCase);
            if (m.Success)
            {
                str = m.Groups[1].Value;
            }

            return str;
        }


        ///// <summary>
        ///// 载入页面足球半场指数
        ///// </summary>
        ///// <param name="p_html">HTML文档</param>
        //public void FootFalfPageHtml(int p_id)
        //{

        //    decimal p_one_lu = 0;
        //    decimal p_two_lu = 0;
        //    decimal p_pk = 0;
        //    int p_pn = 1;
        //    decimal p_big_lu = 0;
        //    decimal p_small_lu = 0;
        //    decimal p_dx_pk = 0;
        //    decimal bzs = 0;
        //    decimal bzp = 0;
        //    decimal bzx = 0;
        //    string txt = "";
        //    txt = new TPR2.Collec.FootFalf().GetFootFalf(p_id);
        //    //取亚盘
        //    string yp = "";
        //    string strpattern1 = @"d2.push\(""([^\^]{5}\,[^\^]{1,3}\,[^\^]{5}\,[\d]{4}\-[\d]{2}\-[\d]{2}\s[\d]{2}\:[\d]{2}\:[\d]{2})\,\d""\);\rd3\.push";
        //    Match mtitle1 = Regex.Match(txt, strpattern1, RegexOptions.Compiled);
        //    if (mtitle1.Success)
        //    {
        //        yp = mtitle1.Groups[1].Value;
        //        string[] Temp = yp.Split(",".ToCharArray());
        //        p_one_lu = Convert.ToDecimal(Temp[0]) + 1;
        //        p_two_lu = Convert.ToDecimal(Temp[2]) + 1;
        //        p_pk = Convert.ToDecimal(Temp[1].Replace("-", ""));
        //        if (Temp[1].Contains("-"))
        //        {
        //            p_pn = 2;
        //        }
        //    }
        //    //取大小盘
        //    string dx = "";
        //    string strpattern2 = @"d3.push\(""([^\^]{5}\,[^\^]{1,3}\,[^\^]{5}\,[\d]{4}\-[\d]{2}\-[\d]{2}\s[\d]{2}\:[\d]{2}\:[\d]{2})\,\d""\);\rd1\.push";
        //    Match mtitle2 = Regex.Match(txt, strpattern2, RegexOptions.Compiled);
        //    if (mtitle2.Success)
        //    {
        //        dx = mtitle2.Groups[1].Value;
        //        string[] Temp = dx.Split(",".ToCharArray());
        //        p_big_lu = Convert.ToDecimal(Temp[0]) + 1;
        //        p_small_lu = Convert.ToDecimal(Temp[2]) + 1;
        //        p_dx_pk = GCK.getDxPkNum(GCK.getDxPkNameZH(Convert.ToInt32(Temp[1])));
        //    }
        //    //取标准盘
        //    string bz = "";
        //    string strpattern3 = @"d1.push\(""([^\^]{5,6}\,[^\^]{5,6}\,[^\^]{5,6}\,[\d]{4}\-[\d]{2}\-[\d]{2}\s[\d]{2}\:[\d]{2}\:[\d]{2})\,\d""\);\r</script>";
        //    Match mtitle3 = Regex.Match(txt, strpattern3, RegexOptions.Compiled);
        //    if (mtitle3.Success)
        //    {
        //        bz = mtitle3.Groups[1].Value;
        //        string[] Temp = bz.Split(",".ToCharArray());
        //        bzs = Convert.ToDecimal(Temp[0]);
        //        bzp = Convert.ToDecimal(Temp[1]);
        //        bzx = Convert.ToDecimal(Temp[2]);
        //    }
        //    //进行全部Odds更新
        //    if (p_one_lu != 0 && p_two_lu != 0 && p_big_lu != 0 && p_small_lu != 0)
        //    {
        //        TPR2.Model.guess.BaList model = new TPR2.Model.guess.BaList();
        //        model.p_id = p_id;
        //        model.p_one_lu = p_one_lu;
        //        model.p_two_lu = p_two_lu;
        //        model.p_pk = p_pk;
        //        model.p_pn = p_pn;
        //        model.p_big_lu = p_big_lu;
        //        model.p_small_lu = p_small_lu;
        //        model.p_dx_pk = p_dx_pk;
        //        model.p_bzs_lu = bzs;
        //        model.p_bzp_lu = bzp;
        //        model.p_bzx_lu = bzx;
        //        new TPR2.BLL.guess.BaList().UpdateFalf(model);
        //    }

        //}


        /// <summary>
        /// 载入页面足球半场指数
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        public string FootFalfPageHtml_kb_old(int p_id, bool Iszd, ref string htmlstr)
        {
            decimal p_one_lu = 0;
            decimal p_two_lu = 0;
            decimal p_pk = 0;
            int p_pn = 1;
            decimal p_big_lu = 0;
            decimal p_small_lu = 0;
            decimal p_dx_pk = 0;
            decimal bzs = 0;
            decimal bzp = 0;
            decimal bzx = 0;
            DateTime p_temptime1 = DateTime.Parse("1990-1-1");
            DateTime p_temptime2 = DateTime.Parse("1990-1-1");
            DateTime p_temptime3 = DateTime.Parse("1990-1-1");

            string txt = "";
            txt = new TPR2.Collec.FootFalf().GetFootFalf(p_id, Iszd);
            #region 处理
            htmlstr = txt;
            //取亚盘
            string yp = "";
            string strpattern1 = @"d2.push\(""([\d.]{1,5}\,[^\^]{1,2}\,[\d.]{1,5}\,[\d]{4}\-[\d]{2}\-[\d]{2}\s[\d]{2}\:[\d]{2}\:[\d]{2})\,\d""\);\rd3\.push";
            if (Iszd)
            {
                strpattern1 = @"d2.push\(""([\d.]{1,5}\,[\d.]{1,2}\,[\d.]{1,5}\,[\d]{4}\-[\d]{2}\-[\d]{2}\s[\d]{2}\:[\d]{2}\:[\d]{2})\,\d\,\d\,\d\,[\d]{2}'""\);\rd3\.push";
            }
            Match mtitle1 = Regex.Match(txt, strpattern1, RegexOptions.Compiled);
            if (mtitle1.Success)
            {
                yp = mtitle1.Groups[1].Value;
                string[] Temp = yp.Split(",".ToCharArray());
                p_one_lu = Convert.ToDecimal(Temp[0]);
                p_two_lu = Convert.ToDecimal(Temp[2]);
                p_pk = Convert.ToDecimal(Temp[1].Replace("-", ""));
                if (Temp[1].Contains("-"))
                {
                    p_pn = 2;
                }
                p_temptime1 = DateTime.Parse(Temp[3]);
            }
            //取大小盘
            string dx = "";
            string strpattern2 = @"d3.push\(""([\d.]{1,5}\,[\d.]{1,3}\,[\d.]{1,5}\,[\d]{4}\-[\d]{2}\-[\d]{2}\s[\d]{2}\:[\d]{2}\:[\d]{2})\,\d""\);\rd1\.push";
            if (Iszd)
            {
                strpattern2 = @"d3.push\(""([\d.]{1,5}\,[\d.]{1,3}\,[\d.]{1,5}\,[\d]{4}\-[\d]{2}\-[\d]{2}\s[\d]{2}\:[\d]{2}\:[\d]{2})\,\d\,\d\,\d\,[\d]{2}'""\);\rd1\.push";
            }
            Match mtitle2 = Regex.Match(txt, strpattern2, RegexOptions.Compiled);
            if (mtitle2.Success)
            {
                dx = mtitle2.Groups[1].Value;
                string[] Temp = dx.Split(",".ToCharArray());
                p_big_lu = Convert.ToDecimal(Temp[0]);
                p_small_lu = Convert.ToDecimal(Temp[2]);
                p_dx_pk = GCK.getDxPkNum2(GCK.getDxPkNameZH(Convert.ToInt32(Temp[1])));
                p_temptime2 = DateTime.Parse(Temp[3]);
            }
            //取标准盘
            string bz = "";
            string strpattern3 = @"d1.push\(""([\d.]{5,6}\,[\d.]{5,6}\,[\d.]{5,6}\,[\d]{4}\-[\d]{2}\-[\d]{2}\s[\d]{2}\:[\d]{2}\:[\d]{2})\,\d""\);[\r\s]{1,}</script>";
            if (Iszd)
            {
                strpattern3 = @"d1.push\(""([\d.]{5,6}\,[\d.]{5,6}\,[\d.]{5,6}\,[\d]{4}\-[\d]{2}\-[\d]{2}\s[\d]{2}\:[\d]{2}\:[\d]{2})\,\d\,\d\,\d\,[\d]{2}'""\);[\r\s]{1,}</script>";
            }
            Match mtitle3 = Regex.Match(txt, strpattern3, RegexOptions.Compiled);
            if (mtitle3.Success)
            {
                bz = mtitle3.Groups[1].Value;
                string[] Temp = bz.Split(",".ToCharArray());
                bzs = Convert.ToDecimal(Temp[0]);
                bzp = Convert.ToDecimal(Temp[1]);
                bzx = Convert.ToDecimal(Temp[2]);
                p_temptime3 = DateTime.Parse(Temp[3]);
            }

            decimal cc = Convert.ToDecimal("0.000");
            bool fp1 = false;
            bool fp2 = false;
            bool fp3 = false;
            if (Iszd)
            {
                if (cc == p_one_lu && cc == p_two_lu)
                {
                    new TPR2.BLL.guess.BaList().Updatep_isluck(p_id, 1, 1, 9, p_temptime1);
                    fp1 = true;
                }
                else
                {
                    new TPR2.BLL.guess.BaList().Updatep_isluck(p_id, 0, 1, 9, p_temptime1);
                }
                if (cc == p_big_lu && cc == p_small_lu)
                {
                    new TPR2.BLL.guess.BaList().Updatep_isluck(p_id, 1, 2, 9, p_temptime2);
                    fp2 = true;
                }
                else
                {
                    new TPR2.BLL.guess.BaList().Updatep_isluck(p_id, 0, 2, 9, p_temptime2);

                }
                if (cc == bzs && cc == bzp && cc == bzx)
                {
                    new TPR2.BLL.guess.BaList().Updatep_isluck(p_id, 1, 3, 9, p_temptime3);
                    fp3 = true;
                }
                else
                {
                    new TPR2.BLL.guess.BaList().Updatep_isluck(p_id, 0, 3, 9, p_temptime3);

                }
            }

            //进行全部Odds更新

            TPR2.Model.guess.BaList model = new TPR2.Model.guess.BaList();
            model.p_id = p_id;
            if (fp1 == false)
            {
                model.p_one_lu = p_one_lu + 1;
                model.p_two_lu = p_two_lu + 1;
                model.p_pk = p_pk;
                model.p_pn = p_pn;
                new TPR2.BLL.guess.BaList().UpdateFalf1(model);
            }
            if (fp2 == false)
            {
                model.p_big_lu = p_big_lu + 1;
                model.p_small_lu = p_small_lu + 1;
                model.p_dx_pk = p_dx_pk;
                new TPR2.BLL.guess.BaList().UpdateFalf2(model);
            }
            if (fp3 == false)
            {
                model.p_bzs_lu = bzs;
                model.p_bzp_lu = bzp;
                model.p_bzx_lu = bzx;
                new TPR2.BLL.guess.BaList().UpdateFalf3(model);
            }
            #endregion
            return txt;
        }

        public void FootFalfPageHtml(int p_id, bool Iszd)
        {
            decimal p_one_lu = 0;
            decimal p_two_lu = 0;
            decimal p_pk = 0;
            int p_pn = 1;
            decimal p_big_lu = 0;
            decimal p_small_lu = 0;
            decimal p_dx_pk = 0;
            decimal bzs = 0;
            decimal bzp = 0;
            decimal bzx = 0;
            DateTime p_temptime1 = DateTime.Parse("1990-1-1");
            DateTime p_temptime2 = DateTime.Parse("1990-1-1");
            DateTime p_temptime3 = DateTime.Parse("1990-1-1");

            string txt = "";
            txt = new TPR2.Collec.FootFalf().GetFootFalf(p_id, Iszd);
            #region 处理
            //取亚盘
            string yp = "";
            string strpattern1 = @"d2.push\(""([\d.]{1,5}\,[^\^]{1,2}\,[\d.]{1,5}\,[\d]{4}\-[\d]{2}\-[\d]{2}\s[\d]{2}\:[\d]{2}\:[\d]{2})\,\d""\);\rd3\.push";
            if (Iszd)
            {
                strpattern1 = @"d2.push\(""([\d.]{1,5}\,[\d.]{1,2}\,[\d.]{1,5}\,[\d]{4}\-[\d]{2}\-[\d]{2}\s[\d]{2}\:[\d]{2}\:[\d]{2})\,\d\,\d\,\d\,[\d]{2}'""\);\rd3\.push";
            }
            Match mtitle1 = Regex.Match(txt, strpattern1, RegexOptions.Compiled);
            if (mtitle1.Success)
            {
                yp = mtitle1.Groups[1].Value;
                string[] Temp = yp.Split(",".ToCharArray());
                p_one_lu = Convert.ToDecimal(Temp[0]);
                p_two_lu = Convert.ToDecimal(Temp[2]);
                p_pk = Convert.ToDecimal(Temp[1].Replace("-", ""));
                if (Temp[1].Contains("-"))
                {
                    p_pn = 2;
                }
                p_temptime1 = DateTime.Parse(Temp[3]);
            }
            //取大小盘
            string dx = "";
            string strpattern2 = @"d3.push\(""([\d.]{1,5}\,[\d.]{1,3}\,[\d.]{1,5}\,[\d]{4}\-[\d]{2}\-[\d]{2}\s[\d]{2}\:[\d]{2}\:[\d]{2})\,\d""\);\rd1\.push";
            if (Iszd)
            {
                strpattern2 = @"d3.push\(""([\d.]{1,5}\,[\d.]{1,3}\,[\d.]{1,5}\,[\d]{4}\-[\d]{2}\-[\d]{2}\s[\d]{2}\:[\d]{2}\:[\d]{2})\,\d\,\d\,\d\,[\d]{2}'""\);\rd1\.push";
            }
            Match mtitle2 = Regex.Match(txt, strpattern2, RegexOptions.Compiled);
            if (mtitle2.Success)
            {
                dx = mtitle2.Groups[1].Value;
                string[] Temp = dx.Split(",".ToCharArray());
                p_big_lu = Convert.ToDecimal(Temp[0]);
                p_small_lu = Convert.ToDecimal(Temp[2]);
                p_dx_pk = GCK.getDxPkNum2(GCK.getDxPkNameZH(Convert.ToInt32(Temp[1])));
                p_temptime2 = DateTime.Parse(Temp[3]);
            }
            //取标准盘
            string bz = "";
            string strpattern3 = @"d1.push\(""([\d.]{5,6}\,[\d.]{5,6}\,[\d.]{5,6}\,[\d]{4}\-[\d]{2}\-[\d]{2}\s[\d]{2}\:[\d]{2}\:[\d]{2})\,\d""\);[\r\s]{1,}</script>";
            if (Iszd)
            {
                strpattern3 = @"d1.push\(""([\d.]{5,6}\,[\d.]{5,6}\,[\d.]{5,6}\,[\d]{4}\-[\d]{2}\-[\d]{2}\s[\d]{2}\:[\d]{2}\:[\d]{2})\,\d\,\d\,\d\,[\d]{2}'""\);[\r\s]{1,}</script>";
            }
            Match mtitle3 = Regex.Match(txt, strpattern3, RegexOptions.Compiled);
            if (mtitle3.Success)
            {
                bz = mtitle3.Groups[1].Value;
                string[] Temp = bz.Split(",".ToCharArray());
                bzs = Convert.ToDecimal(Temp[0]);
                bzp = Convert.ToDecimal(Temp[1]);
                bzx = Convert.ToDecimal(Temp[2]);
                p_temptime3 = DateTime.Parse(Temp[3]);
            }

            decimal cc = Convert.ToDecimal("0.000");
            bool fp1 = false;
            bool fp2 = false;
            bool fp3 = false;
            if (Iszd)
            {
                if (cc == p_one_lu && cc == p_two_lu)
                {
                    new TPR2.BLL.guess.BaList().Updatep_isluck(p_id, 1, 1, 9, p_temptime1);
                    fp1 = true;
                }
                else
                {
                    new TPR2.BLL.guess.BaList().Updatep_isluck(p_id, 0, 1, 9, p_temptime1);
                }
                if (cc == p_big_lu && cc == p_small_lu)
                {
                    new TPR2.BLL.guess.BaList().Updatep_isluck(p_id, 1, 2, 9, p_temptime2);
                    fp2 = true;
                }
                else
                {
                    new TPR2.BLL.guess.BaList().Updatep_isluck(p_id, 0, 2, 9, p_temptime2);

                }
                if (cc == bzs && cc == bzp && cc == bzx)
                {
                    new TPR2.BLL.guess.BaList().Updatep_isluck(p_id, 1, 3, 9, p_temptime3);
                    fp3 = true;
                }
                else
                {
                    new TPR2.BLL.guess.BaList().Updatep_isluck(p_id, 0, 3, 9, p_temptime3);

                }
            }

            //进行全部Odds更新

            TPR2.Model.guess.BaList model = new TPR2.Model.guess.BaList();
            model.p_id = p_id;
            if (fp1 == false)
            {
                model.p_one_lu = p_one_lu + 1;
                model.p_two_lu = p_two_lu + 1;
                model.p_pk = p_pk;
                model.p_pn = p_pn;
                new TPR2.BLL.guess.BaList().UpdateFalf1(model);
            }
            if (fp2 == false)
            {
                model.p_big_lu = p_big_lu + 1;
                model.p_small_lu = p_small_lu + 1;
                model.p_dx_pk = p_dx_pk;
                new TPR2.BLL.guess.BaList().UpdateFalf2(model);
            }
            if (fp3 == false)
            {
                model.p_bzs_lu = bzs;
                model.p_bzp_lu = bzp;
                model.p_bzx_lu = bzx;
                new TPR2.BLL.guess.BaList().UpdateFalf3(model);
            }
            #endregion
        }
        /// <summary>
        /// 载入页面足球半场指数
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        public string FootFalfPageHtml1(int p_id, bool Iszd, ref string htmlstr)
        {
            decimal p_one_lu = 0;
            decimal p_two_lu = 0;
            decimal p_pk = 0;
            int p_pn = 1;
            decimal p_big_lu = 0;
            decimal p_small_lu = 0;
            decimal p_dx_pk = 0;
            decimal bzs = 0;
            decimal bzp = 0;
            decimal bzx = 0;
            DateTime p_temptime1 = DateTime.Parse("1990-1-1");
            DateTime p_temptime2 = DateTime.Parse("1990-1-1");
            DateTime p_temptime3 = DateTime.Parse("1990-1-1");
            string txt = "";
            txt = new TPR2.Collec.Footbo().GetFootBoView1(p_id);
            htmlstr = txt;
            if (txt != "")
            {
                #region 处理
                //取亚盘
                JsonData data = JsonMapper.ToObject(txt);
                if (Iszd)
                {
                    p_one_lu = Convert.ToDecimal(data["zdHRq1"].ToString());
                    p_two_lu = Convert.ToDecimal(data["zdHRq2"].ToString());
                    p_pk = Convert.ToDecimal(data["zdHRqpk"].ToString().Replace("-", ""));
                    if (data["zdHRqpk"].ToString().Contains("-"))
                    {
                        p_pn = 2;
                    }
                }
                else
                {
                    //获取半场走地盘口
                    p_one_lu = Convert.ToDecimal(data["fHRq1"].ToString());
                    p_two_lu = Convert.ToDecimal(data["fHRq2"].ToString());
                    p_pk = Convert.ToDecimal(data["fHRqpk"].ToString().Replace("-", ""));
                    if (data["fHRqpk"].ToString().Contains("-"))
                    {
                        p_pn = 2;
                    }
                }

                //取大小盘
                if (Iszd)
                {
                    p_big_lu = Convert.ToDecimal(data["zdHDx1"].ToString());
                    p_small_lu = Convert.ToDecimal(data["zdHDx2"].ToString());
                    decimal hdxpk = Convert.ToDecimal(data["zdHDxpk"].ToString());
                    int p_hdxpk = int.Parse(Math.Floor(hdxpk).ToString());
                    p_dx_pk = Convert.ToInt32(p_hdxpk);
                }
                else
                {
                    p_big_lu = Convert.ToDecimal(data["fHDx1"].ToString());
                    p_small_lu = Convert.ToDecimal(data["fHDx2"].ToString());
                    decimal fdxpk = Convert.ToDecimal(data["fHDxpk"].ToString());
                    int p_hdxpk = int.Parse(Math.Floor(fdxpk).ToString());
                    p_dx_pk =Convert.ToInt32(p_hdxpk);
                }

                //取标准盘            
                if (Iszd)
                {
                    bzs = Convert.ToDecimal(data["zdHBzz"].ToString());
                    bzp = Convert.ToDecimal(data["zdHBzh"].ToString());
                    bzx = Convert.ToDecimal(data["zdHBzk"].ToString());
                }
                else
                {
                    bzs = Convert.ToDecimal(data["fHBzz"].ToString());
                    bzp = Convert.ToDecimal(data["fHBzh"].ToString());
                    bzx = Convert.ToDecimal(data["fHBzk"].ToString());
                }

                bool fp1 = false;
                bool fp2 = false;
                bool fp3 = false;
                if (Iszd)
                {
                    #region 封盘操作
                    TPR2.BLL.guess.BaList bll = new TPR2.BLL.guess.BaList();
                    TPR2.Model.guess.BaList lk = bll.Getluck(p_id, 9);
                    if (lk.p_isluckone != 2)
                    {
                        if (data["p_zdHluck1"].ToString() == "1")
                        {
                            p_temptime1 = DateTime.Parse(data["p_zdHtemptime1"].ToString());
                            new TPR2.BLL.guess.BaList().Updatep_isluck(p_id, 1, 1, 9, p_temptime1);
                            fp1 = true;
                        }
                        else
                        {
                            new TPR2.BLL.guess.BaList().Updatep_isluck(p_id, 0, 1, 9, p_temptime1);
                        }
                    }

                    if (lk.p_islucktwo != 2)
                    {
                        if (data["p_zdHluck2"].ToString() == "1")
                        {
                            p_temptime2 = DateTime.Parse(data["p_zdHtemptime2"].ToString());
                            new TPR2.BLL.guess.BaList().Updatep_isluck(p_id, 1, 2, 9, p_temptime2);
                            fp2 = true;
                        }
                        else
                        {
                            new TPR2.BLL.guess.BaList().Updatep_isluck(p_id, 0, 2, 9, p_temptime2);

                        }
                    }

                    if (lk.p_isluckthr != 2)
                    {
                        if (data["p_zdHluck3"].ToString() == "1")
                        {
                            p_temptime2 = DateTime.Parse(data["p_zdHtemptime3"].ToString());
                            new TPR2.BLL.guess.BaList().Updatep_isluck(p_id, 1, 3, 9, p_temptime3);
                            fp3 = true;
                        }
                        else
                        {
                            new TPR2.BLL.guess.BaList().Updatep_isluck(p_id, 0, 3, 9, p_temptime3);
                        }
                    }
                    #endregion
                }

                //进行全部Odds更新

                TPR2.Model.guess.BaList model = new TPR2.Model.guess.BaList();
                model.p_id = p_id;
                //if (fp1 == false)
                //{
                model.p_one_lu = p_one_lu;
                model.p_two_lu = p_two_lu;
                model.p_pk = p_pk;
                model.p_pn = p_pn;
                new TPR2.BLL.guess.BaList().UpdateFalf1(model);
                //}
                //if (fp2 == false)
                //{
                model.p_big_lu = p_big_lu;
                model.p_small_lu = p_small_lu;
                model.p_dx_pk = p_dx_pk;
                new TPR2.BLL.guess.BaList().UpdateFalf2(model);
                //}
                //if (fp3 == false)
                //{
                model.p_bzs_lu = bzs;
                model.p_bzp_lu = bzp;
                model.p_bzx_lu = bzx;
                new TPR2.BLL.guess.BaList().UpdateFalf3(model);
                //}
                #endregion
            }
            return txt;
        }
    }
}
