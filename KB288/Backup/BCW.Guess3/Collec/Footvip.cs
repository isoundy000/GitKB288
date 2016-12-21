using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using BCW.Common;
using TPR3.Model;

namespace TPR3.Collec
{
    /// <summary>
    /// 足球即时赔率抓取类
    /// </summary>
    public class Footvip
    {
        private string _ResponseValue = string.Empty;
        private string _CacheFolder = "~/Files/Cache/";

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
        public Footvip()
        {
        }


        /// <summary>
        /// 更新一场足球(列表使用，用作转换走地和更新封盘)
        /// </summary>
        /// <param name="p_id">球探标识ID</param>
        public void Update2(int p_id)
        {
            string Footvip = new TPR3.Collec.Footvip().GetFootvip(p_id);
            if (!string.IsNullOrEmpty(Footvip))
            {
                Footvip = Utils.Mid(Footvip, 1, Footvip.Length);
                string[] Temp = Regex.Split(Footvip, "@");
                string Lock1 = Temp[0];
                string Lock2 = Temp[1];
                string Lock3 = Temp[2];

                //------------让球盘开始------------
                int p_isluckone = 0;
                if (Lock1.Contains("封") || Lock1.Contains("background-color:red;color:White;"))
                {
                    p_isluckone = 1;
                }
                //更新是否封盘
                if (Lock1.Contains("滚"))
                {
                    new TPR3.BLL.guess.BaList().Updatep_isluck(p_id, p_isluckone, 1);

                    //自动转换成走地
                    string xmlPath = "/Controls/guess.xml";
                    string title = new TPR3.BLL.guess.BaList().Getp_title(p_id);
                    if (ub.GetSub("Sitegqstat", xmlPath).IndexOf(title) != -1)
                    {
                    DateTime p_TPRtime = new TPR3.BLL.guess.BaList().Getp_TPRtime(p_id);
                    int FootMin = Utils.ParseInt(ub.GetSub("SiteFootMin", xmlPath));
                    new TPR3.BLL.guess.BaList().FootOnceType2(p_id, p_TPRtime.AddMinutes(FootMin));
                    }
                }

                //------------让球盘结束------------

                //------------大小盘开始------------
                int p_islucktwo = 0;
                if (Lock2.Contains("封") || Lock2.Contains("background-color:red;color:White;"))
                {
                    p_islucktwo = 1;
                }
                //更新是否封盘
                if (Lock2.Contains("滚"))
                {
                    new TPR3.BLL.guess.BaList().Updatep_isluck(p_id, p_islucktwo, 2);
                }
                //------------大小盘结束------------

                //------------标准盘开始------------
                int p_isluckthr = 0;
                if (Lock3.Contains("封") || Lock3.Contains("background-color:red;color:White;"))
                {
                    p_isluckthr = 1;
                }
                //更新是否封盘
                if (Lock3.Contains("滚"))
                {
                    new TPR3.BLL.guess.BaList().Updatep_isluck(p_id, p_isluckthr, 3);
                }
                //------------标准盘结束------------
            }
        }


        /// <summary>
        /// 更新一场足球(载入比赛页面使用)
        /// </summary>
        /// <param name="p_id">球探标识ID</param>
        public void Update(int p_id)
        {
            string Footvip = new TPR3.Collec.Footvip().GetFootvip(p_id);
            if (!string.IsNullOrEmpty(Footvip))
            {
                Footvip = Utils.Mid(Footvip, 1, Footvip.Length);
                string[] Temp = Regex.Split(Footvip, "@");
                string Lock1 = Temp[0];
                string Lock2 = Temp[1];
                string Lock3 = Temp[2];

                //------------让球盘开始------------
                int p_isluckone = 0;
                if (Lock1.Contains("封") || Lock1.Contains("background-color:red;color:White;"))
                {
                    p_isluckone = 1;
                }
                else
                {
                    string ypodds = "";
                    MatchCollection mc = Regex.Matches(Lock1, @"<td style=""color[\s\S]+?>([\s\S]+?)</td>", RegexOptions.IgnoreCase);
                    if (mc.Count > 0)
                    {
                        for (int i = 0; i < mc.Count; i++)
                        {
                            ypodds += "@" + mc[i].Groups[1].Value.ToString();
                        }
                        ypodds = Utils.Mid(ypodds, 1, ypodds.Length);
                    }
                    //builder.Append(ypodds);
                    string[] ypTemp = Regex.Split(ypodds, "@");
                    int k = 0;
                    if (ypTemp.Length == 4)
                        k = 1;

                    int p_pn = 1;
                    string sxpk = ypTemp[k + 1];
                    if (sxpk.Contains("受让"))
                        p_pn = 2;

                    decimal p_pk = TPR3.Common.GCK.getPkNum2(sxpk.Replace("受让", ""));
                    decimal p_one_lu = Convert.ToDecimal(ypTemp[k + 0]) + 1;
                    decimal p_two_lu = Convert.ToDecimal(ypTemp[k + 2]) + 1;
                    //builder.Append(p_pk + "|" + sxpk + "<br />");
                    //builder.Append(p_one_lu + "<br />");
                    //builder.Append(p_two_lu + "<br />");

                    TPR3.Model.guess.BaList model = new TPR3.Model.guess.BaList();
                    model.p_id = p_id;
                    model.p_se = 0;
                    model.p_pn = p_pn;
                    model.p_pk = p_pk;
                    model.p_one_lu = p_one_lu;
                    model.p_two_lu = p_two_lu;
                    new TPR3.BLL.guess.BaList().FootypUpdate(model);
                }

                //更新是否封盘
                if (Lock1.Contains("滚"))
                {
                    new TPR3.BLL.guess.BaList().Updatep_isluck(p_id, p_isluckone, 1);
                }

                //------------让球盘结束------------

                //------------大小盘开始------------
                int p_islucktwo = 0;
                if (Lock2.Contains("封") || Lock2.Contains("background-color:red;color:White;"))
                {
                    p_islucktwo = 1;
                }
                else
                {
                    string dxodds = "";
                    MatchCollection mc = Regex.Matches(Lock2, @"<td style=""color[\s\S]+?>([\s\S]+?)</td>", RegexOptions.IgnoreCase);
                    if (mc.Count > 0)
                    {
                        for (int i = 0; i < mc.Count; i++)
                        {
                            dxodds += "@" + mc[i].Groups[1].Value.ToString();
                        }
                        dxodds = Utils.Mid(dxodds, 1, dxodds.Length);
                    }
                    //builder.Append(dxodds);
                    string[] dxTemp = Regex.Split(dxodds, "@");
                    int k = 0;
                    if (dxTemp.Length == 4)
                        k = 1;

                    decimal p_dx_pk = TPR3.Common.GCK.getDxPkNum2(dxTemp[k + 1]);
                    decimal p_big_lu = Convert.ToDecimal(dxTemp[k + 0]) + 1;
                    decimal p_small_lu = Convert.ToDecimal(dxTemp[k + 2]) + 1;

                    //builder.Append(p_dx_pk + "<br />");
                    //builder.Append(p_big_lu + "<br />");
                    //builder.Append(p_small_lu + "<br />");
                    TPR3.Model.guess.BaList model = new TPR3.Model.guess.BaList();
                    model.p_id = p_id;
                    model.p_se = 0;
                    model.p_dx_pk = p_dx_pk;
                    model.p_big_lu = p_big_lu;
                    model.p_small_lu = p_small_lu;
                    new TPR3.BLL.guess.BaList().FootdxUpdate(model);
                }
                //更新是否封盘
                if (Lock2.Contains("滚"))
                {
                    new TPR3.BLL.guess.BaList().Updatep_isluck(p_id, p_islucktwo, 2);
                }
                //------------大小盘结束------------

                //------------标准盘开始------------
                int p_isluckthr = 0;
                if (Lock3.Contains("封") || Lock3.Contains("background-color:red;color:White;"))
                {
                    p_isluckthr = 1;
                }
                else
                {
                    string bzodds = "";
                    MatchCollection mc = Regex.Matches(Lock3, @"<td style=""color[\s\S]+?>([\s\S]+?)</td>", RegexOptions.IgnoreCase);
                    if (mc.Count > 0)
                    {
                        for (int i = 0; i < mc.Count; i++)
                        {
                            bzodds += "@" + mc[i].Groups[1].Value.ToString();
                        }
                        bzodds = Utils.Mid(bzodds, 1, bzodds.Length);
                    }
                    //builder.Append(bzodds);
                    string[] bzTemp = Regex.Split(bzodds, "@");
                    int k = 0;
                    if (bzTemp.Length == 4)
                        k = 1;

                    decimal bzs = Convert.ToDecimal(bzTemp[k + 0]);
                    decimal bzp = Convert.ToDecimal(bzTemp[k + 1]);
                    decimal bzx = Convert.ToDecimal(bzTemp[k + 2]);

                    //builder.Append(bzs + "|" + bzp + "|" + bzx + "<br />");

                    TPR3.Model.guess.BaList model = new TPR3.Model.guess.BaList();
                    model.p_id = p_id;
                    model.p_se = 0;
                    model.p_bzs_lu = bzs;
                    model.p_bzp_lu = bzp;
                    model.p_bzx_lu = bzx;
                    new TPR3.BLL.guess.BaList().FootbzUpdate(model);
                }
                //更新是否封盘
                if (Lock3.Contains("滚"))
                {
                    new TPR3.BLL.guess.BaList().Updatep_isluck(p_id, p_isluckthr, 3);
                }
                //------------标准盘结束------------
            }
        }

        /// <summary>
        /// 取得足球赔率页面
        /// </summary>
        public string GetFootvip(int p_id)
        {
            string obj = string.Empty;

            string url = "http://vip.titan007.com/changeDetail/3in1Odds.aspx?id=" + p_id + "&companyid=3";

            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = "足球赔率页面XML" + p_id + "";

            httpRequest.WebAsync.RevCharset = "GB2312";
            httpRequest.WebAsync.RevCharset = "GB2312";
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                obj = FootvipHtml(this._ResponseValue);

            return obj;
        }


        private string FootvipHtml(string p_html)
        {
            if (string.IsNullOrEmpty(p_html))
                return "";

            System.Text.StringBuilder builder = new System.Text.StringBuilder("");
            MatchCollection mc = Regex.Matches(p_html, @"<table width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""1"" bgcolor=""#DDDDDD""  class=""gts"">[\s\S]+?<tr class="" gt1"">([\s\S]+?)</tr>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (mc.Count > 0)
            {
                for (int i = 0; i < mc.Count; i++)
                {
                    builder.Append("@" + mc[i].Groups[1].Value.ToString());
                }
            }

            return builder.ToString();

        }

       
    }
}