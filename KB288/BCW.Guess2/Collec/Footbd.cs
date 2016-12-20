using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using BCW.Common;
using TPR2.Common;
using TPR2.Model;

///增加其他胆下注
///黄国军 20160706
///修复波胆指数
///黄国军 20160530
namespace TPR2.Collec
{
    /// <summary>
    /// 足球波胆抓取类
    /// </summary>
    public class Footbd
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
        public Footbd()
        {
        }

        /// <summary>
        /// 取得足球波胆指数
        /// </summary>
        public string GetFootbd(int p_id)
        {
            string obj = string.Empty;
            string url = "http://pl2.8bobf.com:23003/bd.aspx?id=" + p_id + "&companyid=1&ot=H&y=2014&typeid=1";

            //代理地址
            string ProxyHost = ub.GetSub("SiteViewStatus", "/Controls/guess2.xml");

            if (ProxyHost != "" && ProxyHost.StartsWith("http://"))
            {
                url = url.Replace("&", "**");
                url = ProxyHost + "8boGet.aspx?url=" + url;
            }

            string html = new TPR2.Collec.Footbo().GetHtml(url, "UTF-8");
            if (html != "")
                obj = html;

            //HttpRequestCache httpRequest = new HttpRequestCache(url);
            //httpRequest.Fc.CacheUsed = this._CacheUsed;
            //httpRequest.Fc.CacheTime = this._CacheTime;
            //httpRequest.Fc.CacheFolder = this._CacheFolder;
            //httpRequest.Fc.CacheFile = "足球波胆数据XML_" + p_id + "";

            //httpRequest.WebAsync.RevCharset = "utf-8";
            //if (httpRequest.MethodGetUrl(out this._ResponseValue))
            //    obj = FootbdHtml(this._ResponseValue);

            return obj;
        }

        /// <summary>
        /// 处理足球波胆指数
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        private string FootbdHtml(string p_html)
        {

            if (string.IsNullOrEmpty(p_html))
                return "";

            string p_str = "";
            string strpattern = @"<title>([\s\S]+?)</head>";
            Match mtitle = Regex.Match(p_html, strpattern, RegexOptions.Compiled);
            if (mtitle.Success)
            {
                //p_str = mtitle.Groups[1].Value;
            }

            return p_str;
        }



        /// <summary>
        /// 载入页面足球波胆指数
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        public string FootbdPageHtml_kb_old(int p_id)
        {
            string txt = "";
            txt = new TPR2.Collec.Footbd().GetFootbd(p_id);

            #region 分析波胆数据
            string z = "";
            string strpattern1 = @"odsdn\[397\] \= \[([\s\S]+?)\]";

            Match mtitle1 = Regex.Match(txt, strpattern1, RegexOptions.Compiled);
            if (mtitle1.Success)
            {
                z = SetFoodbd(p_id, mtitle1);
            }
            else
            {
                strpattern1 = @"odsdb\[397\] \= \[([\s\S]+?)\]";
                mtitle1 = Regex.Match(txt, strpattern1, RegexOptions.Compiled);
                if (mtitle1.Success)
                {
                    z = SetFoodbd(p_id, mtitle1);
                }
            }
            #endregion
            return txt;
        }

        /// <summary>
        /// 载入页面足球波胆指数
        /// </summary>
        /// <param name="p_html">HTML文档</param>
        public void FootbdPageHtml(int p_id)
        {
            string txt = "";
            txt = new TPR2.Collec.Footbd().GetFootbd(p_id);

            #region 分析波胆数据
            string z = "";
            string strpattern1 = @"odsdn\[397\] \= \[([\s\S]+?)\]";

            Match mtitle1 = Regex.Match(txt, strpattern1, RegexOptions.Compiled);
            if (mtitle1.Success)
            {
                z = SetFoodbd(p_id, mtitle1);
            }
            else
            {
                strpattern1 = @"odsdb\[397\] \= \[([\s\S]+?)\]";
                mtitle1 = Regex.Match(txt, strpattern1, RegexOptions.Compiled);
                if (mtitle1.Success)
                {
                    z = SetFoodbd(p_id, mtitle1);
                }
            }
            #endregion
        }

        private static string SetFoodbd(int p_id, Match mtitle1)
        {
            string z;
            z = mtitle1.Groups[1].Value;//1:0	2:0	2:1	3:0	3:1	3:2	4:0	4:1	4:2	4:3

            string[] score = z.Split(",".ToCharArray());
            string score10 = score[0];
            string score20 = score[1];
            string score21 = score[2];
            string score30 = score[3];
            string score31 = score[4];
            string score32 = score[5];
            string score40 = score[6];
            string score41 = score[7];
            string score42 = score[8];
            string score43 = score[9];
            string score01 = score[10];
            string score02 = score[11];
            string score12 = score[12];
            string score03 = score[13];
            string score13 = score[14];
            string score23 = score[15];
            string score04 = score[16];
            string score14 = score[17];
            string score24 = score[18];
            string score34 = score[19];
            string score00 = score[20];
            string score11 = score[21];
            string score22 = score[22];
            string score33 = score[23];
            string score44 = score[24];
            //五倍波胆 默认是6倍,可控开关
            string score5 = "-1";
            //其他波胆,即除了以上的情况,都为中奖状态
            string other = "15";

            TPR2.Model.guess.BaList model = new TPR2.BLL.guess.BaList().GetModelByp_id(p_id);
            if (model.p_score == "")
            {
                score5 = "-1";
                other = "15";
            }
            else
            {
                score5 = model.p_score.Split(',')[26].Split('|')[1];
                if (model.p_score.Split(',').Length > 27)
                {
                    other = model.p_score.Split(',')[27].Split('|')[1];
                }
            }

            if (z != "")
            {
                string scoreStr = "";
                scoreStr += "1:0|" + score10 + ",0:1|" + score01 + ",0:0|" + score00 + ",2:0|" + score20 + ",0:2|" + score02 + ",1:1|" + score11 + ",3:0|" + score30 + ",0:3|" + score03 + ",2:2|" + score22 + "";
                scoreStr += ",4:0|" + score40 + ",0:4|" + score04 + ",3:3|" + score33 + ",2:1|" + score21 + ",1:2|" + score12 + ",4:4|" + score44 + "";
                scoreStr += ",3:1|" + score31 + ",1:3|" + score13 + ",4:1|" + score41 + ",1:4|" + score14 + ",3:2|" + score32 + ",2:3|" + score23;
                scoreStr += ",4:2|" + score42 + ",2:4|" + score24 + ",4:3|" + score43 + ",3:4|" + score34 + ",5z|" + score5 + ",5k|" + score5;
                scoreStr += ",ot|" + other + "";

                new TPR2.BLL.guess.BaList().Updatep_score2(p_id, scoreStr);
            }

            return z;
        }
    }
}
