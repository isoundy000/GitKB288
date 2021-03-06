﻿using System;
using System.Web;
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
    /// 篮球抓取类
    /// </summary>
    public class Basketbo
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
        public Basketbo()
        {
        }

        /// <summary>
        /// 取得篮球XML
        /// </summary>
        public string GetBasketbolist(int Types, int Page)
        {

            //篮球的：赛事更新、水位变动、全场滚球（包括封盘）、即时比分、完场比分（加返彩）、比赛进行时间

            //篮球的：赛事、水位、全场滚球（包括封盘）、即时比分、完场比分返彩、比赛进行时间

            string obj = string.Empty;

            string url = string.Empty;
            if (Types == 1)
                url = "http://3g.8bo.com/3g/basketball/score/today.aspx?by=event&st=hasStart&pagesize=1000&page=" + Page + "";//已开赛
            else if (Types == 2)
                url = "http://3g.8bo.com/3g/basketball/score/today.aspx?by=event&st=notStart&pagesize=1000&page=" + Page + "";//未开赛
            else if (Types == 3)
                url = "http://3g.8bo.com/3g/basketball/score/today.aspx?by=event&st=hasCompletedField&pagesize=1000&page=" + Page + "";//已完场
            else
                url = "http://3g.8bo.com/3g/basketball/score/today.aspx?by=event&st=allEvents&page=" + Page + "";//所有比赛

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
            httpRequest.Fc.CacheFile = "Basket8波XML_" + Page + "";

            httpRequest.WebAsync.RevCharset = "UTF-8";
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
            {
                if (this._ResponseValue.Contains(">下一页</a>]"))
                    obj = BasketbolistHtml(this._ResponseValue) + "#NEXT#";
                else
                    obj = BasketbolistHtml(this._ResponseValue);
            }
            return obj;
        }

        /// <summary>
        /// 处理篮球XML
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private string BasketbolistHtml(string p_html)
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

        /// <summary>
        /// 得到赔率分析页面
        /// </summary>
        /// <param name="boid"></param>
        /// <returns></returns>
        public string GetBasketBoView(int boid)
        {


            string obj = string.Empty;

            string url = string.Empty;
            url = "http://3g.8bo.com/3g/basketball/score/today.aspx?eid=" + boid + "&by=detail";

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
            httpRequest.Fc.CacheFile = "Basket8波ViewXML";

            httpRequest.WebAsync.RevCharset = "UTF-8";
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                obj = BasketboViewHtml(this._ResponseValue);
            //string html = TPR2.Collec.Footbo.GetHtml(url, "UTF-8");
            //if (html != "")
            //    obj = BasketboViewHtml(html);
            return obj;
        }

        /// <summary>
        /// 得到赔率分析页面【新】
        /// </summary>
        /// <param name="boid"></param>
        /// <returns></returns>
        public string GetBasketBoView1(int boid)
        {
            string obj = string.Empty;
            string url = string.Empty;
            //接口地址
            string Host = ub.GetSub("SiteViewStatus_1", "/Controls/guess2.xml");
            if (Host != "") { url = Host + "?pid=" + boid; }
            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = "Basket8波ViewXML";

            httpRequest.WebAsync.RevCharset = "UTF-8";
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                obj = this._ResponseValue;
            //string html = TPR2.Collec.Footbo.GetHtml(url, "UTF-8");
            //if (html != "")
            //    obj = BasketboViewHtml(html);
            return obj;
        }

        /// <summary>
        /// 处理赔率分析页面
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private string BasketboViewHtml(string p_html)
        {
            if (string.IsNullOrEmpty(p_html))
                return "";

            string str = string.Empty;
            string pattern = @"<table cellspacing=""0"" cellpadding=""0"" border=""0"" class=""odds"">([\s\S]+?)</table>";
            Match m = Regex.Match(p_html, pattern, RegexOptions.IgnoreCase);
            if (m.Success)
            {
                str = m.Groups[1].Value;
            }

            return str;
        }

        public string GetBoView_kb_old(int boid, bool Iszd)
        {
            string bo = new TPR2.Collec.Basketbo().GetBasketBoView(boid);
            #region 处理
            if (bo != "")
            {

                //取联赛名称
                string title = "";
                string strpattern = @"([\s\S]+)</td><td class=""W2"">";
                Match mtitle = Regex.Match(bo, strpattern, RegexOptions.IgnoreCase);
                if (mtitle.Success)
                {
                    title = mtitle.Groups[1].Value;
                    title = Regex.Replace(title, @"<.+?>", "");

                }
                //取比赛状态
                string strState = "";
                strpattern = @"<td align=""center"">([\s\S]{1,10})</td>";//未、点(这个是代表点球)、完、待定、腰斩、推迟（无这些选项则取的比赛进行分钟数）
                mtitle = Regex.Match(bo, strpattern, RegexOptions.IgnoreCase);
                if (mtitle.Success)
                {
                    strState = mtitle.Groups[1].Value.Trim();
                }
                //取比赛日期
                string Date = "";
                strpattern = @"<td class=""W2"">((\d){2}-(\d){2})</td><td class=""teamname"">";
                mtitle = Regex.Match(bo, strpattern, RegexOptions.IgnoreCase);
                if (mtitle.Success)
                {
                    Date = mtitle.Groups[1].Value;
                    //HttpContext.Current.Response.Write(Date + "<br />");
                }
                //取比赛时间
                string Time = "";
                strpattern = @"<td>((\d){2}:(\d){2})</td><td class=""teamname"">";
                mtitle = Regex.Match(bo, strpattern, RegexOptions.IgnoreCase);
                if (mtitle.Success)
                {
                    Time = mtitle.Groups[1].Value;
                    //HttpContext.Current.Response.Write(Time + "<br />");
                }
                DateTime p_TPRtime = DateTime.Parse(DateTime.Now.Year + "-" + Date + " " + Time);


                //取主队名称
                string p_one = "";
                strpattern = @"<td class=""teamname"">([\s\S]+)<td align=""center"">";
                mtitle = Regex.Match(bo, strpattern, RegexOptions.IgnoreCase);
                if (mtitle.Success)
                {
                    p_one = mtitle.Groups[1].Value.Trim();
                    p_one = Regex.Replace(p_one, @"<small>\[[\s\S]+\]</small>", "");
                    p_one = Regex.Replace(p_one, @"<span class=""rc"">[\w\d]+</span>", "");
                    p_one = Regex.Replace(p_one, @"<.+?>", "");

                }

                //取客队名称
                string p_two = "";
                if (strState == "未")
                    strpattern = @"<td>(\d){2}:(\d){2}</td><td class=""teamname"">([\s\S]+)</td></tr>";
                else
                    strpattern = @"<td>(\d){2}:(\d){2}</td><td class=""teamname"">([\s\S]+)<td colspan=""2"">";

                mtitle = Regex.Match(bo, strpattern, RegexOptions.IgnoreCase);
                if (mtitle.Success)
                {
                    p_two = mtitle.Groups[0].Value.Trim();
                    string[] p_twoTemp = Regex.Split(p_two, @"<tr class=""alternation"">");

                    p_two = Regex.Replace(p_twoTemp[0], @"<small>\[[\s\S]+\]</small>", "");
                    p_two = Regex.Replace(p_two, @"<span class=""rc"">[\w\d]+</span>", "");
                    if (p_two.Contains("↑"))
                    {
                        p_two = Regex.Split(p_two, "↑")[0];
                    }
                    p_two = Regex.Replace(p_two, @"<td>(\d){2}:(\d){2}</td>", "");


                    //这里即时完场比分
                    string Result = "";
                    strpattern = @"\[<b class=""score"">((\d){1,3}-(\d){1,3})</b>\]";
                    mtitle = Regex.Match(bo, strpattern, RegexOptions.IgnoreCase);
                    if (mtitle.Success)
                    {
                        //取即时比分
                        Result = mtitle.Groups[1].Value;
                        int p_id = boid;

                        if (Result.Contains("-"))
                        {
                            try
                            {
                                string[] p_result = Result.Split('-');
                                if (strState == "完")
                                {
                                    int p_result_one = Convert.ToInt32(p_result[0]);
                                    int p_result_two = Convert.ToInt32(p_result[1]);
                                    new TPR2.BLL.guess.BaList().UpdateBoResult(p_id, p_result_one, p_result_two);
                                    new TPR2.BLL.guess.BaList().UpdateOnce(boid, strState);

                                }
                                else
                                {
                                    int p_result_temp1 = Convert.ToInt32(p_result[0]);
                                    int p_result_temp2 = Convert.ToInt32(p_result[1]);
                                    TPR2.Model.guess.BaList bf = new TPR2.BLL.guess.BaList().GetTemp(p_id);
                                    if (bf != null)
                                    {
                                        if (bf.p_result_temp1 != p_result_temp1 || bf.p_result_temp2 != p_result_temp2)
                                            new TPR2.BLL.guess.BaList().UpdateBoResult3(p_id, p_result_temp1, p_result_temp2);

                                    }
                                }
                            }
                            catch { }
                        }
                    }
                    p_two = Regex.Replace(p_two, strpattern, "");

                    p_two = Regex.Replace(p_two, @"<.+?>", "");
                    p_two = p_two.Replace("单式", "").Replace("滾球", "");

                }

                //HttpContext.Current.Response.Write(boid + "<br />");
                //HttpContext.Current.Response.Write(title + "<br />");
                //HttpContext.Current.Response.Write(strState + "<br />");
                //HttpContext.Current.Response.Write(p_TPRtime + "<br />");
                //HttpContext.Current.Response.Write(p_one + "<br />");

                //取盘口
                decimal p_one_lu = 0;
                decimal p_two_lu = 0;
                decimal p_pk = 0;
                int p_pn = 1;
                decimal p_big_lu = 0;
                decimal p_small_lu = 0;
                decimal p_dx_pk = 0;

                //==========================================================================
                string odds = "";
                //取滚球水位
                if (Iszd)
                {
                    strpattern = @"<b>滾球</b>([\s\S]+)<b>即时</b>";
                    mtitle = Regex.Match(bo, strpattern, RegexOptions.IgnoreCase);
                    if (mtitle.Success)
                    {
                        odds = mtitle.Groups[1].Value;

                        //=======================分析滚球封盘=======================
                        int p_id = boid;
                        string Todds = Regex.Replace(odds, @"<[^del].+?>", "");
                        Todds = "@↑让球" + Todds + "@";
                        if (Todds.Contains("↑让球<del>"))//封让球盘
                        {
                            //HttpContext.Current.Response.Write("封让球盘<br />");
                            new TPR2.BLL.guess.BaList().Updatep_isluck(p_id, 1, 1);
                        }
                        else
                        {
                            new TPR2.BLL.guess.BaList().Updatep_isluck(p_id, 0, 1);
                        }
                        if (Todds.Contains("↑大小<del>"))//封大小盘
                        {
                            //HttpContext.Current.Response.Write("封大小盘<br />");
                            new TPR2.BLL.guess.BaList().Updatep_isluck(p_id, 1, 2);
                        }
                        else
                        {
                            new TPR2.BLL.guess.BaList().Updatep_isluck(p_id, 0, 2);
                        }
                        //============================分析滚球封盘================================
                        odds = Regex.Replace(odds, @"<.+?>", "");
                        odds = odds.Replace("↑大小", " ").Replace("↑让球", "");

                        //HttpContext.Current.Response.Write("-----------滚球水位-----------<br />");
                        string[] oddsTemp = Regex.Split(odds, " ");
                        p_one_lu = Convert.ToDecimal(oddsTemp[0]) + 1;
                        p_two_lu = Convert.ToDecimal(oddsTemp[2]) + 1;
                        p_pk = Convert.ToDecimal(oddsTemp[1]);
                        if (p_pk < 0)
                            p_pn = 2;

                        //大小盘 没有的情况下会报错,跳过处理 黄国军 20160215
                        if (oddsTemp.Length > 3)
                        {
                            p_big_lu = Convert.ToDecimal(oddsTemp[3]) + 1;
                            p_small_lu = Convert.ToDecimal(oddsTemp[5]) + 1;
                            p_dx_pk = Convert.ToDecimal(oddsTemp[4]);
                        }


                        //水位如果有变则更新变动的时间
                        TPR2.Model.guess.BaList m = new TPR2.BLL.guess.BaList().GetModelByp_id(boid);
                        if (m != null)
                        {
                            string newodds = p_pk + "_" + p_one_lu + "_" + p_two_lu + "_" + p_dx_pk + "_" + p_big_lu + "_" + p_small_lu;
                            string oldodds = m.p_pk + "_" + m.p_one_lu + "_" + m.p_two_lu + "_" + m.p_dx_pk + "_" + m.p_big_lu + "_" + m.p_small_lu;
                            if (oldodds != newodds)
                            {
                                BCW.Data.SqlHelper.ExecuteSql("update tb_SysTemp set GuessOddsTime='" + DateTime.Now + "' where id=1");//更新水位变化时间

                            }
                        }
                        //HttpContext.Current.Response.Write(p_two + "<br />");
                        //HttpContext.Current.Response.Write(Result + "<br />");
                        //HttpContext.Current.Response.Write("上盘水位" + p_one_lu + "<br />");
                        //HttpContext.Current.Response.Write("下盘水位" + p_two_lu + "<br />");
                        //HttpContext.Current.Response.Write("让球盘口" + p_pk + "<br />");
                        //HttpContext.Current.Response.Write("大盘" + p_big_lu + "<br />");
                        //HttpContext.Current.Response.Write("小盘" + p_small_lu + "<br />");
                        //HttpContext.Current.Response.Write("大小盘口" + p_dx_pk + "<br />");
                    }
                    //==========================================================================
                }
                else
                {
                    if (ub.GetSub("Sitegqstat2", "/Controls/guess2.xml").IndexOf(title) != -1)
                    {

                        if (bo.Contains("滾球") || bo.Contains("滚球"))
                        {
                            new TPR2.BLL.guess.BaList().FootOnceType2(boid, p_TPRtime.AddMinutes(150));
                        }

                    }
                    //取单式水位
                    strpattern = @"<b>即时</b>([\s\S]+)<b>初盘</b>";

                    mtitle = Regex.Match(bo, strpattern, RegexOptions.IgnoreCase);
                    if (!mtitle.Success)
                    {
                        strpattern = @"<b>即时</b>([\s\S]+)";
                        mtitle = Regex.Match(bo, strpattern, RegexOptions.IgnoreCase);
                    }
                    if (mtitle.Success)
                    {
                        odds = mtitle.Groups[1].Value;
                        odds = Regex.Replace(odds, @"<.+?>", "");
                        odds = odds.Replace("↑大小", " ").Replace("↑让球", "");
                        //HttpContext.Current.Response.Write("-----------单式水位-----------<br />");
                        string[] oddsTemp = Regex.Split(odds, " ");
                        p_one_lu = Convert.ToDecimal(oddsTemp[0]) + 1;
                        p_two_lu = Convert.ToDecimal(oddsTemp[2]) + 1;
                        p_pk = Convert.ToDecimal(oddsTemp[1]);
                        if (p_pk < 0)
                            p_pn = 2;

                        p_big_lu = Convert.ToDecimal(oddsTemp[3]) + 1;
                        p_small_lu = Convert.ToDecimal(oddsTemp[5]) + 1;
                        p_dx_pk = Convert.ToDecimal(oddsTemp[4]);

                        //HttpContext.Current.Response.Write("上盘水位" + p_one_lu + "<br />");
                        //HttpContext.Current.Response.Write("下盘水位" + p_two_lu + "<br />");
                        //HttpContext.Current.Response.Write("让球盘口" + p_pk + "<br />");
                        //HttpContext.Current.Response.Write("大盘" + p_big_lu + "<br />");
                        //HttpContext.Current.Response.Write("小盘" + p_small_lu + "<br />");
                        //HttpContext.Current.Response.Write("大小盘口" + p_dx_pk + "<br />");
                    }
                    //==========================================================================
                }


                //更新比赛状态
                if (strState != "完")
                {
                    new TPR2.BLL.guess.BaList().UpdateOnce(boid, strState);
                }
                //==================================更新进数据库====================================
                if (strState != "完")
                {
                    int p_id = boid;
                    if (p_one_lu != 0)
                    {
                        TPR2.Model.guess.BaList model = new TPR2.Model.guess.BaList();
                        model.p_id = p_id;
                        model.p_title = title;
                        model.p_type = 2;
                        model.p_one = p_one;
                        model.p_two = p_two;
                        model.p_pk = p_pk;
                        model.p_dx_pk = p_dx_pk;
                        model.p_pn = p_pn;

                        model.p_one_lu = p_one_lu;
                        model.p_two_lu = p_two_lu;
                        if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th"))
                        {
                            if (model.p_one_lu + model.p_two_lu < 4)
                            {
                                model.p_one_lu = p_one_lu + Convert.ToDecimal("0.02");
                                model.p_two_lu = p_two_lu + Convert.ToDecimal("0.02");
                            }
                        }

                        model.p_big_lu = p_big_lu;
                        model.p_small_lu = p_small_lu;
                        if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th"))
                        {
                            if (model.p_big_lu + model.p_small_lu < 4)
                            {
                                model.p_big_lu = p_big_lu + Convert.ToDecimal("0.02");
                                model.p_small_lu = p_small_lu + Convert.ToDecimal("0.02");
                            }
                        }
                        model.p_bzs_lu = 0;
                        model.p_bzp_lu = 0;
                        model.p_bzx_lu = 0;
                        model.p_addtime = DateTime.Now;
                        model.p_TPRtime = p_TPRtime;
                        model.p_ison = 0;
                        TPR2.BLL.guess.BaList bll = new TPR2.BLL.guess.BaList();

                        bll.BasketUpdate(model);

                    }
                }
                //==================================更新进数据库====================================

            }
            else
            {
                //HttpContext.Current.Response.Write("已没有数据");
                //HttpContext.Current.Response.End();
            }
            #endregion
            return bo;
        }

        public void GetBoView(int boid, bool Iszd)
        {
            string bo = new TPR2.Collec.Basketbo().GetBasketBoView(boid);
            #region 处理
            if (bo != "")
            {

                //取联赛名称
                string title = "";
                string strpattern = @"([\s\S]+)</td><td class=""W2"">";
                Match mtitle = Regex.Match(bo, strpattern, RegexOptions.IgnoreCase);
                if (mtitle.Success)
                {
                    title = mtitle.Groups[1].Value;
                    title = Regex.Replace(title, @"<.+?>", "");

                }
                //取比赛状态
                string strState = "";
                strpattern = @"<td align=""center"">([\s\S]{1,10})</td>";//未、点(这个是代表点球)、完、待定、腰斩、推迟（无这些选项则取的比赛进行分钟数）
                mtitle = Regex.Match(bo, strpattern, RegexOptions.IgnoreCase);
                if (mtitle.Success)
                {
                    strState = mtitle.Groups[1].Value.Trim();
                }
                //取比赛日期
                string Date = "";
                strpattern = @"<td class=""W2"">((\d){2}-(\d){2})</td><td class=""teamname"">";
                mtitle = Regex.Match(bo, strpattern, RegexOptions.IgnoreCase);
                if (mtitle.Success)
                {
                    Date = mtitle.Groups[1].Value;
                    //HttpContext.Current.Response.Write(Date + "<br />");
                }
                //取比赛时间
                string Time = "";
                strpattern = @"<td>((\d){2}:(\d){2})</td><td class=""teamname"">";
                mtitle = Regex.Match(bo, strpattern, RegexOptions.IgnoreCase);
                if (mtitle.Success)
                {
                    Time = mtitle.Groups[1].Value;
                    //HttpContext.Current.Response.Write(Time + "<br />");
                }
                DateTime p_TPRtime = DateTime.Parse(DateTime.Now.Year + "-" + Date + " " + Time);


                //取主队名称
                string p_one = "";
                strpattern = @"<td class=""teamname"">([\s\S]+)<td align=""center"">";
                mtitle = Regex.Match(bo, strpattern, RegexOptions.IgnoreCase);
                if (mtitle.Success)
                {
                    p_one = mtitle.Groups[1].Value.Trim();
                    p_one = Regex.Replace(p_one, @"<small>\[[\s\S]+\]</small>", "");
                    p_one = Regex.Replace(p_one, @"<span class=""rc"">[\w\d]+</span>", "");
                    p_one = Regex.Replace(p_one, @"<.+?>", "");

                }

                //取客队名称
                string p_two = "";
                if (strState == "未")
                    strpattern = @"<td>(\d){2}:(\d){2}</td><td class=""teamname"">([\s\S]+)</td></tr>";
                else
                    strpattern = @"<td>(\d){2}:(\d){2}</td><td class=""teamname"">([\s\S]+)<td colspan=""2"">";

                mtitle = Regex.Match(bo, strpattern, RegexOptions.IgnoreCase);
                if (mtitle.Success)
                {
                    p_two = mtitle.Groups[0].Value.Trim();
                    string[] p_twoTemp = Regex.Split(p_two, @"<tr class=""alternation"">");

                    p_two = Regex.Replace(p_twoTemp[0], @"<small>\[[\s\S]+\]</small>", "");
                    p_two = Regex.Replace(p_two, @"<span class=""rc"">[\w\d]+</span>", "");
                    if (p_two.Contains("↑"))
                    {
                        p_two = Regex.Split(p_two, "↑")[0];
                    }
                    p_two = Regex.Replace(p_two, @"<td>(\d){2}:(\d){2}</td>", "");


                    //这里即时完场比分
                    string Result = "";
                    strpattern = @"\[<b class=""score"">((\d){1,3}-(\d){1,3})</b>\]";
                    mtitle = Regex.Match(bo, strpattern, RegexOptions.IgnoreCase);
                    if (mtitle.Success)
                    {
                        //取即时比分
                        Result = mtitle.Groups[1].Value;
                        int p_id = boid;

                        if (Result.Contains("-"))
                        {
                            try
                            {
                                string[] p_result = Result.Split('-');
                                if (strState == "完")
                                {
                                    int p_result_one = Convert.ToInt32(p_result[0]);
                                    int p_result_two = Convert.ToInt32(p_result[1]);
                                    new TPR2.BLL.guess.BaList().UpdateBoResult(p_id, p_result_one, p_result_two);
                                    new TPR2.BLL.guess.BaList().UpdateOnce(boid, strState);

                                }
                                else
                                {
                                    int p_result_temp1 = Convert.ToInt32(p_result[0]);
                                    int p_result_temp2 = Convert.ToInt32(p_result[1]);
                                    TPR2.Model.guess.BaList bf = new TPR2.BLL.guess.BaList().GetTemp(p_id);
                                    if (bf != null)
                                    {
                                        if (bf.p_result_temp1 != p_result_temp1 || bf.p_result_temp2 != p_result_temp2)
                                            new TPR2.BLL.guess.BaList().UpdateBoResult3(p_id, p_result_temp1, p_result_temp2);

                                    }
                                }
                            }
                            catch { }
                        }
                    }
                    p_two = Regex.Replace(p_two, strpattern, "");

                    p_two = Regex.Replace(p_two, @"<.+?>", "");
                    p_two = p_two.Replace("单式", "").Replace("滾球", "");

                }

                //HttpContext.Current.Response.Write(boid + "<br />");
                //HttpContext.Current.Response.Write(title + "<br />");
                //HttpContext.Current.Response.Write(strState + "<br />");
                //HttpContext.Current.Response.Write(p_TPRtime + "<br />");
                //HttpContext.Current.Response.Write(p_one + "<br />");

                //取盘口
                decimal p_one_lu = 0;
                decimal p_two_lu = 0;
                decimal p_pk = 0;
                int p_pn = 1;
                decimal p_big_lu = 0;
                decimal p_small_lu = 0;
                decimal p_dx_pk = 0;

                //==========================================================================
                string odds = "";
                //取滚球水位
                if (Iszd)
                {
                    strpattern = @"<b>滾球</b>([\s\S]+)<b>即时</b>";
                    mtitle = Regex.Match(bo, strpattern, RegexOptions.IgnoreCase);
                    if (mtitle.Success)
                    {
                        odds = mtitle.Groups[1].Value;

                        //=======================分析滚球封盘=======================
                        int p_id = boid;
                        string Todds = Regex.Replace(odds, @"<[^del].+?>", "");
                        Todds = "@↑让球" + Todds + "@";
                        if (Todds.Contains("↑让球<del>"))//封让球盘
                        {
                            //HttpContext.Current.Response.Write("封让球盘<br />");
                            new TPR2.BLL.guess.BaList().Updatep_isluck(p_id, 1, 1);
                        }
                        else
                        {
                            new TPR2.BLL.guess.BaList().Updatep_isluck(p_id, 0, 1);
                        }
                        if (Todds.Contains("↑大小<del>"))//封大小盘
                        {
                            //HttpContext.Current.Response.Write("封大小盘<br />");
                            new TPR2.BLL.guess.BaList().Updatep_isluck(p_id, 1, 2);
                        }
                        else
                        {
                            new TPR2.BLL.guess.BaList().Updatep_isluck(p_id, 0, 2);
                        }
                        //============================分析滚球封盘================================
                        odds = Regex.Replace(odds, @"<.+?>", "");
                        odds = odds.Replace("↑大小", " ").Replace("↑让球", "");

                        //HttpContext.Current.Response.Write("-----------滚球水位-----------<br />");
                        string[] oddsTemp = Regex.Split(odds, " ");
                        p_one_lu = Convert.ToDecimal(oddsTemp[0]) + 1;
                        p_two_lu = Convert.ToDecimal(oddsTemp[2]) + 1;
                        p_pk = Convert.ToDecimal(oddsTemp[1]);
                        if (p_pk < 0)
                            p_pn = 2;

                        //大小盘 没有的情况下会报错,跳过处理 黄国军 20160215
                        if (oddsTemp.Length > 3)
                        {
                            p_big_lu = Convert.ToDecimal(oddsTemp[3]) + 1;
                            p_small_lu = Convert.ToDecimal(oddsTemp[5]) + 1;
                            p_dx_pk = Convert.ToDecimal(oddsTemp[4]);
                        }


                        //水位如果有变则更新变动的时间
                        TPR2.Model.guess.BaList m = new TPR2.BLL.guess.BaList().GetModelByp_id(boid);
                        if (m != null)
                        {
                            string newodds = p_pk + "_" + p_one_lu + "_" + p_two_lu + "_" + p_dx_pk + "_" + p_big_lu + "_" + p_small_lu;
                            string oldodds = m.p_pk + "_" + m.p_one_lu + "_" + m.p_two_lu + "_" + m.p_dx_pk + "_" + m.p_big_lu + "_" + m.p_small_lu;
                            if (oldodds != newodds)
                            {
                                BCW.Data.SqlHelper.ExecuteSql("update tb_SysTemp set GuessOddsTime='" + DateTime.Now + "' where id=1");//更新水位变化时间

                            }
                        }
                        //HttpContext.Current.Response.Write(p_two + "<br />");
                        //HttpContext.Current.Response.Write(Result + "<br />");
                        //HttpContext.Current.Response.Write("上盘水位" + p_one_lu + "<br />");
                        //HttpContext.Current.Response.Write("下盘水位" + p_two_lu + "<br />");
                        //HttpContext.Current.Response.Write("让球盘口" + p_pk + "<br />");
                        //HttpContext.Current.Response.Write("大盘" + p_big_lu + "<br />");
                        //HttpContext.Current.Response.Write("小盘" + p_small_lu + "<br />");
                        //HttpContext.Current.Response.Write("大小盘口" + p_dx_pk + "<br />");
                    }
                    //==========================================================================
                }
                else
                {
                    if (ub.GetSub("Sitegqstat2", "/Controls/guess2.xml").IndexOf(title) != -1)
                    {

                        if (bo.Contains("滾球") || bo.Contains("滚球"))
                        {
                            new TPR2.BLL.guess.BaList().FootOnceType2(boid, p_TPRtime.AddMinutes(150));
                        }

                    }
                    //取单式水位
                    strpattern = @"<b>即时</b>([\s\S]+)<b>初盘</b>";

                    mtitle = Regex.Match(bo, strpattern, RegexOptions.IgnoreCase);
                    if (!mtitle.Success)
                    {
                        strpattern = @"<b>即时</b>([\s\S]+)";
                        mtitle = Regex.Match(bo, strpattern, RegexOptions.IgnoreCase);
                    }
                    if (mtitle.Success)
                    {
                        odds = mtitle.Groups[1].Value;
                        odds = Regex.Replace(odds, @"<.+?>", "");
                        odds = odds.Replace("↑大小", " ").Replace("↑让球", "");
                        //HttpContext.Current.Response.Write("-----------单式水位-----------<br />");
                        string[] oddsTemp = Regex.Split(odds, " ");
                        p_one_lu = Convert.ToDecimal(oddsTemp[0]) + 1;
                        p_two_lu = Convert.ToDecimal(oddsTemp[2]) + 1;
                        p_pk = Convert.ToDecimal(oddsTemp[1]);
                        if (p_pk < 0)
                            p_pn = 2;

                        p_big_lu = Convert.ToDecimal(oddsTemp[3]) + 1;
                        p_small_lu = Convert.ToDecimal(oddsTemp[5]) + 1;
                        p_dx_pk = Convert.ToDecimal(oddsTemp[4]);

                        //HttpContext.Current.Response.Write("上盘水位" + p_one_lu + "<br />");
                        //HttpContext.Current.Response.Write("下盘水位" + p_two_lu + "<br />");
                        //HttpContext.Current.Response.Write("让球盘口" + p_pk + "<br />");
                        //HttpContext.Current.Response.Write("大盘" + p_big_lu + "<br />");
                        //HttpContext.Current.Response.Write("小盘" + p_small_lu + "<br />");
                        //HttpContext.Current.Response.Write("大小盘口" + p_dx_pk + "<br />");
                    }
                    //==========================================================================
                }


                //更新比赛状态
                if (strState != "完")
                {
                    new TPR2.BLL.guess.BaList().UpdateOnce(boid, strState);
                }
                //==================================更新进数据库====================================
                if (strState != "完")
                {
                    int p_id = boid;
                    if (p_one_lu != 0)
                    {
                        TPR2.Model.guess.BaList model = new TPR2.Model.guess.BaList();
                        model.p_id = p_id;
                        model.p_title = title;
                        model.p_type = 2;
                        model.p_one = p_one;
                        model.p_two = p_two;
                        model.p_pk = p_pk;
                        model.p_dx_pk = p_dx_pk;
                        model.p_pn = p_pn;

                        model.p_one_lu = p_one_lu;
                        model.p_two_lu = p_two_lu;
                        if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th"))
                        {
                            if (model.p_one_lu + model.p_two_lu < 4)
                            {
                                model.p_one_lu = p_one_lu + Convert.ToDecimal("0.02");
                                model.p_two_lu = p_two_lu + Convert.ToDecimal("0.02");
                            }
                        }

                        model.p_big_lu = p_big_lu;
                        model.p_small_lu = p_small_lu;
                        if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th"))
                        {
                            if (model.p_big_lu + model.p_small_lu < 4)
                            {
                                model.p_big_lu = p_big_lu + Convert.ToDecimal("0.02");
                                model.p_small_lu = p_small_lu + Convert.ToDecimal("0.02");
                            }
                        }
                        model.p_bzs_lu = 0;
                        model.p_bzp_lu = 0;
                        model.p_bzx_lu = 0;
                        model.p_addtime = DateTime.Now;
                        model.p_TPRtime = p_TPRtime;
                        model.p_ison = 0;
                        TPR2.BLL.guess.BaList bll = new TPR2.BLL.guess.BaList();

                        bll.BasketUpdate(model);

                    }
                }
                //==================================更新进数据库====================================

            }
            else
            {
                //HttpContext.Current.Response.Write("已没有数据");
                //HttpContext.Current.Response.End();
            }
            #endregion
        }

        /// <summary>
        /// 获取篮球水位
        /// </summary>
        /// <param name="boid"></param>
        /// <param name="Iszd"></param>
        /// <returns></returns>
        public string GetBoView1(int boid, bool Iszd)
        {
            string bo = new TPR2.Collec.Basketbo().GetBasketBoView1(boid);
            #region 处理
            if (bo != "")
            {
                //篮球只读取全场的水位
                JsonData data = JsonMapper.ToObject(bo);
                //取联赛名称
                string title = data["p_title"].ToString();
                //取比赛状态
                string strState = data["p_once"].ToString();
                //取主队名称
                string p_one = data["p_one"].ToString();
                //取客队名称
                string p_two = data["p_two"].ToString();
                //比赛时间
                DateTime p_TPRtime = DateTime.Parse(data["p_TPRtime"].ToString());

                //这里即时完场比分
                string r1 = "0", r2 = "0";
                if (data["result_one"].ToString() != "-1") { r1 = data["result_one"].ToString(); }
                if (data["result_two"].ToString() != "-1") { r2 = data["result_two"].ToString(); }
                string Result = r1 + "-" + r2;

                int p_id = int.Parse(data["p_id"].ToString());

                #region 全场比分更新
                if (Result.Contains("-"))
                {
                    try
                    {
                        string[] p_result = Result.Split('-');
                        if (strState == "完")
                        {
                            int p_result_one = Convert.ToInt32(p_result[0]);
                            int p_result_two = Convert.ToInt32(p_result[1]);
                            new TPR2.BLL.guess.BaList().UpdateBoResult(p_id, p_result_one, p_result_two);
                            new TPR2.BLL.guess.BaList().UpdateOnce(boid, strState);

                        }
                        else
                        {
                            int p_result_temp1 = Convert.ToInt32(p_result[0]);
                            int p_result_temp2 = Convert.ToInt32(p_result[1]);
                            TPR2.Model.guess.BaList bf = new TPR2.BLL.guess.BaList().GetTemp(p_id);
                            if (bf != null)
                            {
                                if (bf.p_result_temp1 != p_result_temp1 || bf.p_result_temp2 != p_result_temp2)
                                    new TPR2.BLL.guess.BaList().UpdateBoResult3(p_id, p_result_temp1, p_result_temp2);
                            }
                        }
                    }
                    catch { }
                }
                #endregion

                //取盘口
                decimal p_one_lu = 0;
                decimal p_two_lu = 0;
                decimal p_pk = 0;
                int p_pn = 1;
                decimal p_big_lu = 0;
                decimal p_small_lu = 0;
                decimal p_dx_pk = 0;

                //==========================================================================
                string odds = "";
                //取滚球水位
                if (Iszd)
                {
                    #region 封盘操作
                    //=======================分析滚球封盘=======================
                    TPR2.BLL.guess.BaList bll = new TPR2.BLL.guess.BaList();
                    TPR2.Model.guess.BaList model = bll.Getluck(p_id, 0);
                    if (model.p_isluckone != 2)
                    {

                        if (data["p_zdluck1"].ToString() == "1")//封让球盘
                        {

                            //HttpContext.Current.Response.Write("封让球盘<br />");
                            new TPR2.BLL.guess.BaList().Updatep_isluck(p_id, 1, 1);
                        }
                        else
                        {
                            new TPR2.BLL.guess.BaList().Updatep_isluck(p_id, 0, 1);
                        }
                    }

                    if (model.p_islucktwo != 2)
                    {
                        if (data["p_zdluck2"].ToString() == "1")//封大小盘
                        {
                            //HttpContext.Current.Response.Write("封大小盘<br />");
                            new TPR2.BLL.guess.BaList().Updatep_isluck(p_id, 1, 2);
                        }
                        else
                        {
                            new TPR2.BLL.guess.BaList().Updatep_isluck(p_id, 0, 2);
                        }
                    }
                    //============================分析滚球封盘================================
                    #endregion

                    #region 滚球水位
                    p_one_lu = Convert.ToDecimal(data["zqRq1"].ToString());
                    p_two_lu = Convert.ToDecimal(data["zqRq2"].ToString());
                    p_pk = Convert.ToDecimal(data["zdRqpk"].ToString());
                    if (p_pk < 0)
                        p_pn = 2;

                    p_big_lu = Convert.ToDecimal(data["zdDx1"].ToString());
                    p_small_lu = Convert.ToDecimal(data["zdDx2"].ToString());
                    p_dx_pk = Convert.ToDecimal(data["zdDxpk"].ToString());


                    //水位如果有变则更新变动的时间
                    TPR2.Model.guess.BaList m = new TPR2.BLL.guess.BaList().GetModelByp_id(boid);
                    if (m != null)
                    {
                        string newodds = p_pk + "_" + p_one_lu + "_" + p_two_lu + "_" + p_dx_pk + "_" + p_big_lu + "_" + p_small_lu;
                        string oldodds = m.p_pk + "_" + m.p_one_lu + "_" + m.p_two_lu + "_" + m.p_dx_pk + "_" + m.p_big_lu + "_" + m.p_small_lu;
                        if (oldodds != newodds)
                        {
                            BCW.Data.SqlHelper.ExecuteSql("update tb_SysTemp set GuessOddsTime='" + DateTime.Now + "' where id=1");//更新水位变化时间

                        }
                    }
                    //==========================================================================
                    #endregion 滚球水位
                }
                else
                {
                    if (ub.GetSub("Sitegqstat2", "/Controls/guess2.xml").IndexOf(title) != -1)
                    {
                        if (data["ison_f"].ToString() == "1")
                        {
                            new TPR2.BLL.guess.BaList().FootOnceType2(boid, p_TPRtime.AddMinutes(150));
                        }

                    }

                    if (data["fRqpk"].ToString().Contains("-"))
                    {
                        p_pn = 2;
                    }
                    //HttpContext.Current.Response.Write("-----------即时水位-----------<br />");
                    try
                    {
                        p_one_lu = Convert.ToDecimal(data["fRq1"].ToString());
                        p_two_lu = Convert.ToDecimal(data["fRq2"].ToString());
                        p_pk = Convert.ToDecimal(data["fRqpk"].ToString());
                        if (p_pk < 0)
                            p_pn = 2;
                        p_big_lu = Convert.ToDecimal(data["fDx1"].ToString());
                        p_small_lu = Convert.ToDecimal(data["fDx2"].ToString());
                        p_dx_pk = Convert.ToDecimal(data["fDxpk"].ToString());
                    }
                    catch { }
                    //==========================================================================
                }


                //更新比赛状态
                if (strState != "完")
                {
                    new TPR2.BLL.guess.BaList().UpdateOnce(boid, strState);
                }
                //==================================更新进数据库====================================
                if (strState != "完")
                {
                    if (p_one_lu != 0)
                    {
                        TPR2.Model.guess.BaList model = new TPR2.Model.guess.BaList();
                        model.p_id = p_id;
                        model.p_title = title;
                        model.p_type = 2;
                        model.p_one = p_one;
                        model.p_two = p_two;
                        model.p_pk = p_pk;
                        model.p_dx_pk = p_dx_pk;
                        model.p_pn = p_pn;

                        model.p_one_lu = p_one_lu;
                        model.p_two_lu = p_two_lu;
                        if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th"))
                        {
                            if (model.p_one_lu + model.p_two_lu < 4)
                            {
                                model.p_one_lu = p_one_lu + Convert.ToDecimal("0.02");
                                model.p_two_lu = p_two_lu + Convert.ToDecimal("0.02");
                            }
                        }

                        model.p_big_lu = p_big_lu;
                        model.p_small_lu = p_small_lu;
                        if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th"))
                        {
                            if (model.p_big_lu + model.p_small_lu < 4)
                            {
                                model.p_big_lu = p_big_lu + Convert.ToDecimal("0.02");
                                model.p_small_lu = p_small_lu + Convert.ToDecimal("0.02");
                            }
                        }
                        model.p_bzs_lu = 0;
                        model.p_bzp_lu = 0;
                        model.p_bzx_lu = 0;
                        model.p_addtime = DateTime.Now;
                        model.p_TPRtime = p_TPRtime;
                        model.p_ison = 0;
                        TPR2.BLL.guess.BaList bll = new TPR2.BLL.guess.BaList();
                        bll.BasketUpdate(model);
                    }
                }
                //==================================更新进数据库====================================

            }
            else
            {
                //HttpContext.Current.Response.Write("已没有数据");
                //HttpContext.Current.Response.End();
            }
            #endregion
            return bo;
        }

        public void GetBoViewOver(int boid)
        {

            string bo = new TPR2.Collec.Basketbo().GetBasketBoView(boid);
            if (bo != "")
            {
                //取比赛状态
                string strState = "";
                string strpattern = @"<td align=""center"">([\s\S]{1,10})</td>";//未、点(这个是代表点球)、完、待定、腰斩、推迟（无这些选项则取的比赛进行分钟数）
                Match mtitle = Regex.Match(bo, strpattern, RegexOptions.IgnoreCase);
                if (mtitle.Success)
                {
                    strState = mtitle.Groups[1].Value.Trim();
                }
                //这里即时完场比分
                string Result = "";
                strpattern = @"\[<b class=""score"">((\d){1,3}-(\d){1,3})</b>\]";
                mtitle = Regex.Match(bo, strpattern, RegexOptions.IgnoreCase);
                if (mtitle.Success)
                {
                    //取即时比分
                    Result = mtitle.Groups[1].Value;
                    int p_id = boid;

                    if (Result.Contains("-"))
                    {
                        try
                        {
                            string[] p_result = Result.Split('-');
                            if (strState == "完")
                            {
                                int p_result_one = Convert.ToInt32(p_result[0]);
                                int p_result_two = Convert.ToInt32(p_result[1]);
                                new TPR2.BLL.guess.BaList().UpdateBoResult(p_id, p_result_one, p_result_two);

                            }
                            else
                            {
                                int p_result_temp1 = Convert.ToInt32(p_result[0]);
                                int p_result_temp2 = Convert.ToInt32(p_result[1]);
                                TPR2.Model.guess.BaList bf = new TPR2.BLL.guess.BaList().GetTemp(p_id);
                                if (bf != null)
                                {
                                    if (bf.p_result_temp1 != p_result_temp1 || bf.p_result_temp2 != p_result_temp2)
                                        new TPR2.BLL.guess.BaList().UpdateBoResult3(p_id, p_result_temp1, p_result_temp2);

                                }
                            }
                        }
                        catch { }
                    }
                    new TPR2.BLL.guess.BaList().UpdateOnce(boid, strState);

                }
            }
            else
            {
                //HttpContext.Current.Response.Write("已没有数据");
                //HttpContext.Current.Response.End();
            }
        }
    }

}
