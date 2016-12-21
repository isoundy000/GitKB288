using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using BCW.Common;
using TPR3.Model;

namespace TPR3.Collec
{
    /// <summary>
    /// 篮球即时赔率抓取类
    /// </summary>
    public class Basketvip
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
        public Basketvip()
        {
        }

        /// <summary>
        /// 更新一场篮球(列表使用，用作转换走地和更新封盘)
        /// </summary>
        /// <param name="p_id">球探标识ID</param>
        public void Update2(int p_id)
        {

            //得到整个页面
            string Basketvip = new TPR3.Collec.Basketvip().GetBasketvip(p_id);//166516、160572
            //得到让球盘赔率集合
            string Basketvip1 = new TPR3.Collec.Basketvip().BasketvipHtml(Basketvip);
            //得到大小盘赔率集合
            string Basketvip2 = new TPR3.Collec.Basketvip().BasketvipHtml2(Basketvip);
            //分析让球盘赔率
            if (!string.IsNullOrEmpty(Basketvip1))
            {
                Basketvip1 = Utils.Mid(Basketvip1, 1, Basketvip1.Length);

                string[] Temp = Regex.Split(Basketvip1, "@");
                string ypxml = Temp[0];
                //builder.Append(ypodds + "<br />");//此值当显示封盘时取不到
                int p_isluckone = 0;
                if (ypxml.Contains("封"))
                {
                    p_isluckone = 1;
                }
                //更新是否封盘
                if (ypxml.Contains("滚"))
                {
                    new TPR3.BLL.guess.BaList().Updatep_isluck(p_id, p_isluckone, 1);

                    //自动转换成走地
                    string xmlPath = "/Controls/guess.xml";
                    string title = new TPR3.BLL.guess.BaList().Getp_title(p_id);
                    if (ub.GetSub("Sitegqstat2", xmlPath).IndexOf(title) != -1)
                    {
                    DateTime p_TPRtime = new TPR3.BLL.guess.BaList().Getp_TPRtime(p_id);
                    int BasketMin = Utils.ParseInt(ub.GetSub("SiteBasketMin", xmlPath));
                    new TPR3.BLL.guess.BaList().FootOnceType3(p_id, p_TPRtime.AddMinutes(BasketMin));
                    }
                }
            }
            //分析大小盘赔率
            if (!string.IsNullOrEmpty(Basketvip2))
            {
                Basketvip2 = Utils.Mid(Basketvip2, 1, Basketvip2.Length);

                string[] Temp2 = Regex.Split(Basketvip2, "@");
                string dxxml = Temp2[0];
                //builder.Append(dxodds + "<br />");//此值当显示封盘时取不到
                int p_islucktwo = 0;
                if (dxxml.Contains("封"))
                {
                    p_islucktwo = 1;
                }
                //更新是否封盘
                if (dxxml.Contains("滚"))
                {
                    new TPR3.BLL.guess.BaList().Updatep_isluck(p_id, p_islucktwo, 2);
                }
            }
        }

        /// <summary>
        /// 更新一场篮球(载入比赛页面使用)
        /// </summary>
        /// <param name="p_id">球探标识ID</param>
        public void Update(int p_id)
        {

            //得到整个页面
            string Basketvip = new TPR3.Collec.Basketvip().GetBasketvip(p_id);//166516、160572
            //得到让球盘赔率集合
            string Basketvip1 = new TPR3.Collec.Basketvip().BasketvipHtml(Basketvip);
            //得到大小盘赔率集合
            string Basketvip2 = new TPR3.Collec.Basketvip().BasketvipHtml2(Basketvip);
            //分析让球盘赔率
            if (!string.IsNullOrEmpty(Basketvip1))
            {
                Basketvip1 = Utils.Mid(Basketvip1, 1, Basketvip1.Length);

                string[] Temp = Regex.Split(Basketvip1, "@");
                string ypxml = Temp[0];

                string ypodds = "";
                MatchCollection mc = Regex.Matches(ypxml, @"<td style=""color[\s\S]+?>([\s\S]+?)</td>", RegexOptions.IgnoreCase);
                if (mc.Count > 0)
                {
                    for (int i = 0; i < mc.Count; i++)
                    {
                        ypodds += "@" + mc[i].Groups[1].Value.ToString();
                    }
                    ypodds = Utils.Mid(ypodds, 1, ypodds.Length);
                }
                //builder.Append(ypodds + "<br />");//此值当显示封盘时取不到
                int p_isluckone = 0;
                if (ypodds.Contains("封"))
                {
                    p_isluckone = 1;
                }
                else
                {
                    string[] ypTemp = Regex.Split(ypodds, "@");
                    decimal p_one_lu = Convert.ToDecimal(ypTemp[1]) + 1;
                    decimal p_pk = Convert.ToDecimal(ypTemp[2]);
                    decimal p_two_lu = Convert.ToDecimal(ypTemp[3]) + 1;
                    TPR3.Model.guess.BaList model = new TPR3.Model.guess.BaList();
                    model.p_id = p_id;
                    model.p_se = 0;
                    //更新让球盘
                    model.p_pk = p_pk;
                    model.p_one_lu = p_one_lu;
                    model.p_two_lu = p_two_lu;
                    if (p_pk < 0)
                        model.p_pn = 2;
                    else
                        model.p_pn = 1;

                    new TPR3.BLL.guess.BaList().BasketUpdateYp(model);
                }
                //更新是否封盘
                if (ypxml.Contains("滚"))
                {
                    new TPR3.BLL.guess.BaList().Updatep_isluck(p_id, p_isluckone, 1);
                }
            }
            //分析大小盘赔率
            if (!string.IsNullOrEmpty(Basketvip2))
            {
                Basketvip2 = Utils.Mid(Basketvip2, 1, Basketvip2.Length);

                string[] Temp2 = Regex.Split(Basketvip2, "@");
                string dxxml = Temp2[0];

                string dxodds = "";
                MatchCollection mc2 = Regex.Matches(dxxml, @"<td style=""color[\s\S]+?>([\s\S]+?)</td>", RegexOptions.IgnoreCase);
                if (mc2.Count > 0)
                {
                    for (int i = 0; i < mc2.Count; i++)
                    {
                        dxodds += "@" + mc2[i].Groups[1].Value.ToString();
                    }
                    dxodds = Utils.Mid(dxodds, 1, dxodds.Length);
                }
                //builder.Append(dxodds + "<br />");//此值当显示封盘时取不到
                int p_islucktwo = 0;
                if (dxodds.Contains("封"))
                {
                    p_islucktwo = 1;
                }
                else
                {
                    string[] dxTemp = Regex.Split(dxodds, "@");
                    decimal p_big_lu = Convert.ToDecimal(dxTemp[1]) + 1;
                    decimal p_dx_pk = Convert.ToDecimal(dxTemp[2]);
                    decimal p_small_lu = Convert.ToDecimal(dxTemp[3]) + 1;

                    TPR3.Model.guess.BaList model = new TPR3.Model.guess.BaList();
                    model.p_id = p_id;
                    model.p_se = 0;
                    //更新大小盘
                    model.p_dx_pk = p_dx_pk;
                    model.p_big_lu = p_big_lu;
                    model.p_small_lu = p_small_lu;
                    new TPR3.BLL.guess.BaList().BasketUpdateDx(model);
                }
                //更新是否封盘
                if (dxxml.Contains("滚"))
                {
                    new TPR3.BLL.guess.BaList().Updatep_isluck(p_id, p_islucktwo, 2);
                }
            }
        }
        /// <summary>
        /// 取得篮球赔率页面
        /// </summary>
        public string GetBasketvip(int p_id)
        {
            string obj = string.Empty;

            string url = "http://nba.titan007.com/odds/2in1Odds.aspx?id=" + p_id + "&cid=3&t=6";

            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = "篮球赔率页面XML" + p_id + "";

            httpRequest.WebAsync.RevCharset = "GB2312";
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                obj = this._ResponseValue;

            return obj;
        }


        /// <summary>
        /// 让球盘即时赔率
        /// </summary>
        /// <param name="p_html"></param>
        /// <returns></returns>
        public string BasketvipHtml(string p_html)
        {
            if (string.IsNullOrEmpty(p_html))
                return "";

            System.Text.StringBuilder builder = new System.Text.StringBuilder("");
            if (p_html.Contains("<tr bgcolor=\"#FFFFFF\">"))
            {
                MatchCollection mc = Regex.Matches(p_html, @"<tr bgcolor=""#FFFFFF"">([\s\S]+?)</tr>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                if (mc.Count > 0)
                {
                    for (int i = 0; i < mc.Count; i++)
                    {
                        builder.Append("@" + mc[i].Groups[1].Value.ToString());
                    }
                }
            }

            return builder.ToString();
        
        }

        /// <summary>
        /// 大小盘即时赔率
        /// </summary>
        /// <param name="p_html"></param>
        /// <returns></returns>
        public string BasketvipHtml2(string p_html)
        {
            if (string.IsNullOrEmpty(p_html))
                return "";

                System.Text.StringBuilder builder = new System.Text.StringBuilder("");
                if (p_html.Contains("<tr style=\"background-color:White;\">"))
                {
                    MatchCollection mc = Regex.Matches(p_html, @"<tr style=""background-color:White;"">([\s\S]+?)</tr>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    if (mc.Count > 0)
                    {
                        for (int i = 0; i < mc.Count; i++)
                        {
                            builder.Append("@" + mc[i].Groups[1].Value.ToString());
                        }
                    }
                }

            return builder.ToString();

        }
    }
}