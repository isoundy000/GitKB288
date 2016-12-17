using System;
using System.Collections.Generic;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.IO;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text.RegularExpressions;
using BCW.Common;
using TPR2.Common;

/// <summary>
/// ==================================
/// 单节指的是篮球的第一节，第二节，第三节和上半场数据
/// 半场指的足球上半场数据。
///  http://vip.titan007.com/xmlvbs/fl_nbaGoal3.xml
/// 
/// 黄国军 20151227
/// ==================================
/// </summary>
public partial class bbs_guess3_boCollec3 : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/guess2.xml";
    protected void Page_Load(object sender, EventArgs e)
    {
        //防止缓存
        Response.Buffer = true;
        Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
        Response.Expires = 0;
        Response.CacheControl = "no-cache";

        string act = Utils.GetRequest("act", "get", 1, "", "1");
        switch (act)
        {
            case "demo":
                DemoPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    #region DemoPage()
    private void DemoPage()
    {

        string txt = new TPR2.Collec.BasketWap().GetBasketWap(216682);

        //builder.Append(txt);

        string p_title = "";
        string p_one = "";
        string p_two = "";
        DateTime p_TPRtime = DateTime.Now;

        string strpattern = @"[\s\S]+<strong\sstyle=[\s\S]+>([\s\S]+)</strong>[\s\S]+<strong\sstyle=[\s\S]+>([\s\S]+)</strong>[\s\S]+<b>开赛时间：</b>([\s\S]+)</font>&nbsp;&nbsp;场地：[\s\S]+target=_blank><u>([\s\S]+)";

        Match mtitle = Regex.Match(txt, strpattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        if (mtitle.Success)
        {
            p_title = mtitle.Groups[4].Value;
            p_title = Regex.Replace(p_title, @"[\s\r\n]+", "");
            //builder.Append(p_title + "");

            p_one = mtitle.Groups[1].Value;
            p_one = Regex.Replace(p_one, @"[\s\r\n]+", "");
            //builder.Append(p_one + "");

            p_two = mtitle.Groups[2].Value;
            p_two = Regex.Replace(p_two, @"[\s\r\n]+", "");
            //builder.Append(p_two + "");


            string p_time = mtitle.Groups[3].Value;
            p_time = Regex.Replace(p_time, @"[\s\r\n]+", "");
            p_time = Regex.Replace(p_time, @"&nbsp;星期[^\^]&nbsp;", " ");
            p_TPRtime = Convert.ToDateTime(p_time);
            //builder.Append(p_TPRtime + "");




            builder.Append(p_title + "<br />");
            builder.Append(p_one + "<br />");
            builder.Append(p_two + "<br />");
            builder.Append(p_TPRtime + "<br />");
        }
    }
    #endregion

    #region ReloadPage()
    public void ReloadPage()
    {
        try
        {
            #region 获得半场和单节
            //获得半场和单节 http://vip.titan007.com/xmlvbs/fl_nbaGoal3.xml
            string str = new TPR2.Collec.BasketDJ().GetBasketDJ();
            #endregion

            if (str != "")
            {
                using (XmlReaderExtend reader = new XmlReaderExtend(str))
                {
                    while (reader.ReadToFollowing("m"))
                    {
                        string p_str = reader.GetElementValue();
                        string[] Temp = p_str.Split(',');
                        int p_id = Convert.ToInt32(Temp[0]);

                        #region 第一节盘口
                        //第一节盘口
                        try
                        {
                            decimal d1_pk = Convert.ToDecimal(Temp[1]);
                            decimal d1_one_lu = Convert.ToDecimal(Temp[2]);
                            decimal d1_two_lu = Convert.ToDecimal(Temp[3]);
                            decimal d1_dx_pk = Convert.ToDecimal(Temp[16]);
                            decimal d1_big_lu = Convert.ToDecimal(Temp[17]);
                            decimal d1_small_lu = Convert.ToDecimal(Temp[18]);
                            AccessData(1, p_id, d1_pk, d1_dx_pk, d1_one_lu, d1_two_lu, d1_big_lu, d1_small_lu);
                        }
                        catch { }
                        #endregion

                        #region 第二节盘口
                        //第二节盘口
                        try
                        {
                            decimal d2_pk = Convert.ToDecimal(Temp[4]);
                            decimal d2_one_lu = Convert.ToDecimal(Temp[5]);
                            decimal d2_two_lu = Convert.ToDecimal(Temp[6]);
                            decimal d2_dx_pk = Convert.ToDecimal(Temp[19]);
                            decimal d2_big_lu = Convert.ToDecimal(Temp[20]);
                            decimal d2_small_lu = Convert.ToDecimal(Temp[21]);
                            AccessData(2, p_id, d2_pk, d2_dx_pk, d2_one_lu, d2_two_lu, d2_big_lu, d2_small_lu);
                        }
                        catch { }
                        #endregion

                        #region 半场盘口
                        //半场盘口
                        try
                        {
                            decimal d3_pk = Convert.ToDecimal(Temp[7]);
                            decimal d3_one_lu = Convert.ToDecimal(Temp[8]);
                            decimal d3_two_lu = Convert.ToDecimal(Temp[9]);
                            decimal d3_dx_pk = Convert.ToDecimal(Temp[22]);
                            decimal d3_big_lu = Convert.ToDecimal(Temp[23]);
                            decimal d3_small_lu = Convert.ToDecimal(Temp[24]);
                            AccessData(3, p_id, d3_pk, d3_dx_pk, d3_one_lu, d3_two_lu, d3_big_lu, d3_small_lu);
                        }
                        catch { }
                        #endregion

                        #region 第三节盘口
                        //第三节盘口
                        try
                        {
                            decimal d4_pk = Convert.ToDecimal(Temp[10]);
                            decimal d4_one_lu = Convert.ToDecimal(Temp[11]);
                            decimal d4_two_lu = Convert.ToDecimal(Temp[12]);
                            decimal d4_dx_pk = Convert.ToDecimal(Temp[25]);
                            decimal d4_big_lu = Convert.ToDecimal(Temp[26]);
                            decimal d4_small_lu = Convert.ToDecimal(Temp[27]);
                            AccessData(4, p_id, d4_pk, d4_dx_pk, d4_one_lu, d4_two_lu, d4_big_lu, d4_small_lu);
                        }
                        catch { }
                        #endregion
                    }
                }

                //<m>156660,
                //-2.5,0.90,0.90,   第一节让分1,2,3
                //,,,               第二节让分4,5,6
                //-4.5,0.90,0.90,   半场让分7,8,9
                //,,,               第三节让分10,11,12
                //-9.5,0.90,0.90,   初盘让分13,14,15
                //44,0.88,0.88,     第一节总分16,17,18
                //,,,               第二节总分19,20,21
                //88.5,0.88,0.88,   半场总分22,23,24
                //,,,               第三节总分25,26,27
                //178.5,0.88,0.88   初盘总分 28,29,30
                //</m>
            }
        }
        catch { }
        Master.Title = "采集单节赛事";
        Master.Refresh = 20;
        Master.Gourl = Utils.getUrl("boCollec3.aspx");
        builder.Append("[" + DateTime.Now + "]更新成功");
    }
    #endregion

    #region 写入数据库 AccessData
    /// <summary>
    /// 写入数据库
    /// </summary>
    /// <param name="p_id"></param>
    /// <param name="p_pk"></param>
    /// <param name="p_dx_pk"></param>
    /// <param name="p_one_lu"></param>
    /// <param name="p_two_lu"></param>
    /// <param name="p_big_lu"></param>
    /// <param name="p_small_lu"></param>
    public void AccessData(int Types, int p_id, decimal p_pk, decimal p_dx_pk, decimal p_one_lu, decimal p_two_lu, decimal p_big_lu, decimal p_small_lu)
    {
        #region 定义Model
        TPR2.Model.guess.BaList model = new TPR2.Model.guess.BaList();
        model.p_id = p_id;
        model.p_type = 2;

        if (p_pk < 0)
            model.p_pn = 2;
        else
            model.p_pn = 1;

        model.p_pk = p_pk;
        model.p_dx_pk = p_dx_pk;
        model.p_one_lu = p_one_lu + 1;
        model.p_two_lu = p_two_lu + 1;
        model.p_big_lu = p_big_lu + 1;
        model.p_small_lu = p_small_lu + 1;
        model.p_bzs_lu = 0;
        model.p_bzp_lu = 0;
        model.p_bzx_lu = 0;
        model.p_basketve = Types;
        #endregion

        #region 操作类 BLL
        TPR2.BLL.guess.BaList bll = new TPR2.BLL.guess.BaList();
        //检查是否存在记录
        if (!bll.ExistsByp_id(p_id, Types))
        {
            #region 是否先隐藏
            //是否先隐藏
            if (ub.GetSub("SiteIsyc", xmlPath) == "1")
            {
                model.p_del = 1;
            }
            else
            {
                model.p_del = 0;
            }
            #endregion

            #region 根据ID抓取数据 txt  http://nba.win007.com/analysis/" + p_id + ".htm
            //根据ID抓取数据 
            string txt = new TPR2.Collec.BasketWap().GetBasketWap(p_id);
            string p_title = "";
            string p_one = "";
            string p_two = "";
            DateTime p_TPRtime = DateTime.Now;
            #endregion

            #region 获取队名，主队副队，时间
            string strpattern = @"[\s\S]+<strong\sstyle=[\s\S]+>([\s\S]+)</strong>[\s\S]+<strong\sstyle=[\s\S]+>([\s\S]+)</strong>[\s\S]+<b>开赛时间：</b>([\s\S]+)</font>&nbsp;&nbsp;场地：[\s\S]+target=_blank><u>([\s\S]+)";
            Match mtitle = Regex.Match(txt, strpattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (mtitle.Success)
            {

                p_title = mtitle.Groups[4].Value;
                p_title = Regex.Replace(p_title, @"[\s\r\n]+", "");
                //builder.Append(p_title + "");

                p_one = mtitle.Groups[1].Value;
                p_one = Regex.Replace(p_one, @"[\s\r\n]+", "");
                //builder.Append(p_one + "");

                p_two = mtitle.Groups[2].Value;
                p_two = Regex.Replace(p_two, @"[\s\r\n]+", "");
                //builder.Append(p_two + "");


                string p_time = mtitle.Groups[3].Value;
                p_time = Regex.Replace(p_time, @"[\s\r\n]+", "");
                p_time = Regex.Replace(p_time, @"&nbsp;星期[^\^]&nbsp;", " ");
                p_TPRtime = Convert.ToDateTime(p_time);
                //builder.Append(p_TPRtime + "");

            }
            #endregion

            #region 一节 二节 上半场 三节更新
            if (Convert.ToDateTime(p_TPRtime) <= DateTime.Now.AddHours(30))
            {
                //if (ub.GetSub("Sitelqhalf", xmlPath).IndexOf(p_title) != -1 || ub.GetSub("Sitelqhalf", xmlPath) == "")
                //{
                model.p_title = p_title;
                model.p_two = p_two;
                if (Types == 1)
                    model.p_one = "(一节)" + p_one;
                else if (Types == 2)
                    model.p_one = "(二节)" + p_one;
                else if (Types == 3)
                    model.p_one = "(上半场)" + p_one;
                else if (Types == 4)
                    model.p_one = "(三节)" + p_one;


                model.p_addtime = DateTime.Now;
                model.p_TPRtime = p_TPRtime;
                model.p_ison = 0;
                bll.Add(model);
                //}
            }
            #endregion
        }
        else
        {
            bll.BasketUpdateOdds(model);
        }
        #endregion
    }
    #endregion

}