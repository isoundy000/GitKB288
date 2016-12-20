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
/// 修复红牌无法显示功能
/// 
/// 【足球】抓取页面 完成查阅 刷新机用
/// 
/// http://3g.8bo.com/3g/football/score/today.aspx?by=event&st=hasStart&pagesize=1000
/// 
/// 黄国军 20151226
/// ==================================
/// </summary>
public partial class bbs_guess2_boCollec : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/guess2.xml";
    protected void Page_Load(object sender, EventArgs e)
    {

        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-9]$", "0"));
        int k = int.Parse(Utils.GetRequest("k", "get", 1, @"^[0-9]\d*$", "1"));
        //抓取http://3g.8bo.com的数据
        string bo = GetFootbolist1x(ptype, k);
        //builder.Append(bo);
        //获取页面HTML代码
        string[] boTemp = Regex.Split(bo, @"<td class=.W1\s[\w\d]+.>");
        for (int i = 1; i < boTemp.Length; i++)
        {
            builder.Append(DateTime.Now.ToString("yyyyMMddHHmmss-fff") + " ");
            builder.Append(" (" + i + ") ");

            #region 取比赛状态 获得 strState值 未、点(这个是代表点球)、完、待定、腰斩、推迟（无这些选项则取的比赛进行分钟数）
            //取比赛状态
            string strState = "";
            string strpattern = @"<td align=""center"">([\s\S]{1,10})</td>";//未、点(这个是代表点球)、完、待定、腰斩、推迟（无这些选项则取的比赛进行分钟数）
            Match mtitle = Regex.Match(boTemp[i], strpattern, RegexOptions.IgnoreCase);
            if (mtitle.Success)
            {
                strState = mtitle.Groups[1].Value;
            }
            #endregion

            #region 未完,大小，滚球 的数据等
            if ((strState == "未" && boTemp[i].Contains("<td>↑大小</td>")) || boTemp[i].Contains("<td>↑滾球</td>"))
            {
                #region 取联赛名称 获取title值

                //取联赛名称
                //Response.Write(strState + "<br />");
                string title = "";
                strpattern = @"([\s\S]+)</td><td class=""W2"">";
                mtitle = Regex.Match(boTemp[i], strpattern, RegexOptions.IgnoreCase);
                if (mtitle.Success)
                {
                    title = mtitle.Groups[1].Value;
                    //Response.Write(title + "<br />");
                }
                #endregion

                #region 取p_id
                //取p_id
                int p_id = 0;
                strpattern = @"by=detail&amp;eid=(\d+).>析";
                mtitle = Regex.Match(boTemp[i], strpattern, RegexOptions.IgnoreCase);
                if (mtitle.Success)
                {
                    p_id = Utils.ParseInt(mtitle.Groups[1].Value);
                    //Response.Write(p_id + "<br />");
                }
                #endregion

                #region  取比赛日期 获取Date
                //取比赛日期
                string Date = "";
                strpattern = @"<td class=""W2"">((\d){2}-(\d){2})</td><td class=""teamname"">";
                mtitle = Regex.Match(boTemp[i], strpattern, RegexOptions.IgnoreCase);
                if (mtitle.Success)
                {
                    Date = mtitle.Groups[1].Value;
                    //Response.Write(Date + "<br />");
                }
                #endregion

                #region 取比赛时间 获取Time
                //取比赛时间
                string Time = "";
                strpattern = @"<td>((\d){2}:(\d){2})</td><td class=""teamname"">";
                mtitle = Regex.Match(boTemp[i], strpattern, RegexOptions.IgnoreCase);
                if (mtitle.Success)
                {
                    Time = mtitle.Groups[1].Value;
                    //Response.Write(Time + "<br />");
                }
                DateTime p_TPRtime = DateTime.Parse(DateTime.Now.Year + "-" + Date + " " + Time);
                //Response.Write(p_TPRtime + "<br />");
                #endregion

                #region 取主队名称 获取p_one
                //取主队名称
                string p_one = "";
                strpattern = @"<td class=""teamname"">([\s\S]+)<a href=""today.aspx";
                mtitle = Regex.Match(boTemp[i], strpattern, RegexOptions.IgnoreCase);
                if (mtitle.Success)
                {
                    p_one = mtitle.Groups[1].Value.Trim();

                    //这里取主队红牌数量
                    if (boTemp[i].Contains("<td>↑滾球</td>"))
                    {
                        string strpatternHp = @"<span class=""rc"">(\d)</span>";
                        Match mtitleHp = Regex.Match(p_one, strpatternHp, RegexOptions.IgnoreCase);
                        if (mtitleHp.Success)
                        {
                            int hp_one = Utils.ParseInt(mtitleHp.Groups[1].Value);
                            if (hp_one > 0)
                            {
                                new TPR2.BLL.guess.BaList().Updatep_hp_one(p_id, hp_one);
                            }
                        }
                    }

                    p_one = Regex.Replace(p_one, @"<small>\[[\w\d]+\]</small>", "");
                    p_one = Regex.Replace(p_one, @"<span class=""rc"">[\w\d]+</span>", "");
                    p_one = Regex.Replace(p_one, @"<.+?>", "");
                    //Response.Write(p_one + "<br />");
                }
                #endregion

                #region 取客队名称 p_two
                //取客队名称
                string p_two = "";
                strpattern = @"<td>(\d){2}:(\d){2}</td><td class=""teamname"">([\s\S]+)</td></tr>";
                mtitle = Regex.Match(boTemp[i], strpattern, RegexOptions.IgnoreCase);
                if (mtitle.Success)
                {
                    p_two = mtitle.Groups[0].Value.Trim();

                    //这里取客队红牌数量
                    if (boTemp[i].Contains("<td>↑滾球</td>"))
                    {
                        string strpatternHp2 = @"<span class=""rc"">(\d)</span>";
                        Match mtitleHp2 = Regex.Match(p_two, strpatternHp2, RegexOptions.IgnoreCase);
                        if (mtitleHp2.Success)
                        {
                            int hp_two = Utils.ParseInt(mtitleHp2.Groups[1].Value);
                            if (hp_two > 0)
                            {
                                new TPR2.BLL.guess.BaList().Updatep_hp_two(p_id, hp_two);
                            }
                        }
                    }

                    string[] p_twoTemp = Regex.Split(p_two, @"<tr class=""alternation"">");

                    p_two = Regex.Replace(p_twoTemp[0], @"<small>\[[\w\d]+\]</small>", "");
                    p_two = Regex.Replace(p_two, @"<span class=""rc"">[\w\d]+</span>", "");

                }
                else
                {
                    strpattern = @"<td>(\d){2}:(\d){2}</td><td class=""teamname"">([\s\S]+)</td></tr>";
                    mtitle = Regex.Match(boTemp[i], strpattern, RegexOptions.IgnoreCase);
                    if (mtitle.Success)
                    {
                        p_two = mtitle.Groups[0].Value.Trim();

                        //这里取客队红牌数量
                        if (boTemp[i].Contains("<td>↑滾球</td>"))
                        {
                            string strpatternHp2 = @"<span class=""rc"">(\d)</span>";
                            Match mtitleHp2 = Regex.Match(p_two, strpatternHp2, RegexOptions.IgnoreCase);
                            if (mtitleHp2.Success)
                            {
                                int hp_two = Utils.ParseInt(mtitleHp2.Groups[1].Value);
                                if (hp_two > 0)
                                {
                                    new TPR2.BLL.guess.BaList().Updatep_hp_two(p_id, hp_two);
                                }
                            }
                        }

                        string[] p_twoTemp = Regex.Split(p_two, @"<tr class=""alternation"">");

                        p_two = Regex.Replace(p_twoTemp[0], @"<small>\[[\w\d]+\]</small>", "");
                        p_two = Regex.Replace(p_two, @"<span class=""rc"">[\w\d]+</span>", "");
                        if (p_two.Contains("↑"))
                        {
                            p_two = Regex.Split(p_two, "↑")[0];
                        }
                    }
                }
                p_two = Regex.Replace(p_two, @"<td>(\d){2}:(\d){2}</td>", "");
                p_two = Regex.Replace(p_two, strpattern, "");
                p_two = Regex.Replace(p_two, @"<td>\(<em>((\d){1,2}-(\d){1,2})</em>\)</td>", "");
                p_two = Regex.Replace(p_two, @"<.+?>", "");
                if (p_two.Contains("↑"))
                {
                    p_two = Regex.Split(p_two, "↑")[0];
                }
                //Response.Write(p_two + "<br />");
                #endregion

                #region 取盘口 变量定义
                //取盘口 
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
                #endregion

                #region 更新进数据库 TPR2.Model.guess.BaList 初始值
                //==================================更新进数据库====================================

                TPR2.Model.guess.BaList model = new TPR2.Model.guess.BaList();
                model.p_one_lu = p_one_lu;
                model.p_two_lu = p_two_lu;
                model.p_pk = p_pk;
                model.p_pn = p_pn;
                model.p_addtime = DateTime.Now;
                model.p_type = 1;
                model.p_title = title;

                model.p_big_lu = p_big_lu;
                model.p_small_lu = p_small_lu;
                model.p_dx_pk = p_dx_pk;

                model.p_TPRtime = p_TPRtime;
                model.p_one = p_one;
                model.p_two = p_two;
                model.p_bzs_lu = bzs;
                model.p_bzp_lu = bzp;
                model.p_bzx_lu = bzx;
                model.p_id = p_id;
                #endregion

                #region 进行波胆更新 波胆地址有问题
                if (Convert.ToDateTime(model.p_TPRtime) <= DateTime.Now.AddHours(20))
                {
                    if (ub.GetSub("Sitezqstat", xmlPath).IndexOf(title) != -1)
                    {
                        TPR2.BLL.guess.BaList bll = new TPR2.BLL.guess.BaList();
                        if (!bll.ExistsByp_id(p_id))
                        {
                            //是否先隐藏
                            if (ub.GetSub("SiteIsyc", xmlPath) == "1")
                            {
                                model.p_del = 1;
                            }
                            else
                            {
                                model.p_del = 0;
                            }
                            bll.FootAdd(model);



                            //Response.Write("<br />====================<br />");
                        }
                        //进行波胆更新
                        if (ub.GetSub("Sitebdstat", xmlPath).IndexOf(title) != -1)
                        {
                            new TPR2.Collec.Footbd().FootbdPageHtml(p_id);
                        }
                    }
                }
                //Response.Write("<br />-----------------------<br />");
                #endregion

                #region 更新为滚球模式
                if (boTemp[i].Contains("滾球"))
                {
                    if (ub.GetSub("Sitegqstat", "/Controls/guess2.xml").IndexOf(title) != -1)
                    {
                        //更新为滚球模式 p_ison更新为1
                        new TPR2.BLL.guess.BaList().FootOnceType2(p_id, p_TPRtime.AddMinutes(130));
                    }
                }
                #endregion

                #region 更新比赛状态
                //更新比赛状态
                if (strState != "")
                {
                    new TPR2.BLL.guess.BaList().UpdateOnce(p_id, strState);
                }
                #endregion

                #region 更新半场
                //================================这里半场更新==================================
                if (strState == "未" || boTemp[i].Contains("<td>↑滾球</td>"))
                {
                    if (ub.GetSub("Sitezqhalf", xmlPath).IndexOf(title) != -1 || ub.GetSub("Sitezqhalf", xmlPath) == "")
                    {
                        #region 定义盘口变量
                        //取盘口
                        p_one_lu = 0;
                        p_two_lu = 0;
                        p_pk = 0;
                        p_pn = 1;
                        p_big_lu = 0;
                        p_small_lu = 0;
                        p_dx_pk = 0;
                        bzs = 0;
                        bzp = 0;
                        bzx = 0;
                        DateTime p_temptime1 = DateTime.Parse("1990-1-1");
                        DateTime p_temptime2 = DateTime.Parse("1990-1-1");
                        DateTime p_temptime3 = DateTime.Parse("1990-1-1");
                        #endregion

                        #region 是不是走地
                        bool Iszd = false;

                        Iszd = new TPR2.BLL.guess.BaList().Existsp_ison(p_id, 9);
                        string txt = "";
                        if (Iszd == false)
                        {
                            if (boTemp[i].Contains("滾球") && ub.GetSub("Sitegqstat3", "/Controls/guess2.xml").IndexOf(title) != -1)
                            {
                                //更新为滚球模式
                                new TPR2.BLL.guess.BaList().FootOnceType4(p_id, p_TPRtime.AddMinutes(65));

                                Iszd = true;
                            }
                        }

                        if (Iszd)
                        {
                            txt = new TPR2.Collec.FootFalf().GetFootFalf(p_id, true);
                        }
                        else
                        {
                            txt = new TPR2.Collec.FootFalf().GetFootFalf(p_id, false);
                        }
                        //builder.Append(txt);
                        #endregion

                        #region 取亚盘
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
                        #endregion

                        #region 取大小盘
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
                        #endregion

                        #region 取标准盘
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
                        #endregion

                        #region 走地更新
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
                        #endregion

                        #region 半场更新进数据库
                        //==================================更新进数据库====================================
                        if (p_one_lu != 0 && p_two_lu != 0 && p_big_lu != 0 && p_small_lu != 0)
                        {
                            model.p_one_lu = p_one_lu + 1;
                            model.p_two_lu = p_two_lu + 1;
                            model.p_pk = p_pk;
                            model.p_pn = p_pn;
                            model.p_addtime = DateTime.Now;
                            model.p_type = 1;
                            model.p_title = title;

                            model.p_big_lu = p_big_lu + 1;
                            model.p_small_lu = p_small_lu + 1;
                            model.p_dx_pk = p_dx_pk;

                            model.p_TPRtime = p_TPRtime;
                            model.p_one = "(上半场)" + p_one;
                            model.p_two = p_two;
                            model.p_bzs_lu = bzs;
                            model.p_bzp_lu = bzp;
                            model.p_bzx_lu = bzx;
                            model.p_id = p_id;
                            model.p_ison = 0;
                            model.p_basketve = 9;//半场标识

                            if (Convert.ToDateTime(model.p_TPRtime) <= DateTime.Now.AddHours(20))
                            {

                                TPR2.BLL.guess.BaList bll2 = new TPR2.BLL.guess.BaList();
                                if (!bll2.ExistsByp_id(p_id, 9))
                                {
                                    //是否先隐藏
                                    if (ub.GetSub("SiteIsyc", xmlPath) == "1")
                                    {
                                        model.p_del = 1;
                                    }
                                    else
                                    {
                                        model.p_del = 0;
                                    }
                                    bll2.Add(model);
                                }
                                else
                                {

                                    //bll2.UpdateFalf(model);
                                    if (fp1 == false)
                                    {
                                        new TPR2.BLL.guess.BaList().UpdateFalf1(model);
                                    }
                                    if (fp2 == false)
                                    {
                                        new TPR2.BLL.guess.BaList().UpdateFalf2(model);
                                    }
                                    if (fp3 == false)
                                    {
                                        new TPR2.BLL.guess.BaList().UpdateFalf3(model);
                                    }
                                }


                            }
                        }
                        #endregion
                    }
                }
                //================================这里半场更新==================================
                #endregion
            }
            #endregion

            #region 完场的数据
            if (strState == "完")
            {
                //取p_id
                int p_id = 0;
                strpattern = @"by=detail&amp;eid=(\d+).>析";
                mtitle = Regex.Match(boTemp[i], strpattern, RegexOptions.IgnoreCase);
                if (mtitle.Success)
                {
                    p_id = Utils.ParseInt(mtitle.Groups[1].Value);
                    //Response.Write(p_id + "<br />");
                }

                #region 取主队名称 获取p_one
                //取主队名称
                string p_one = "";
                strpattern = @"<td class=""teamname"">([\s\S]+)<a href=""today.aspx";
                mtitle = Regex.Match(boTemp[i], strpattern, RegexOptions.IgnoreCase);
                if (mtitle.Success)
                {
                    p_one = mtitle.Groups[1].Value.Trim();

                    //这里取主队红牌数量
                    if (boTemp[i].Contains("<td>↑滾球</td>"))
                    {
                        string strpatternHp = @"<span class=""rc"">(\d)</span>";
                        Match mtitleHp = Regex.Match(p_one, strpatternHp, RegexOptions.IgnoreCase);
                        if (mtitleHp.Success)
                        {
                            int hp_one = Utils.ParseInt(mtitleHp.Groups[1].Value);
                            if (hp_one > 0)
                            {
                                new TPR2.BLL.guess.BaList().Updatep_hp_one(p_id, hp_one);
                            }
                        }
                    }

                    p_one = Regex.Replace(p_one, @"<small>\[[\w\d]+\]</small>", "");
                    p_one = Regex.Replace(p_one, @"<span class=""rc"">[\w\d]+</span>", "");
                    p_one = Regex.Replace(p_one, @"<.+?>", "");
                    //Response.Write(p_one + "<br />");
                }
                #endregion

                #region 取客队名称 p_two
                //取客队名称
                string p_two = "";
                strpattern = @"<td>(\d){2}:(\d){2}</td><td class=""teamname"">([\s\S]+)</td></tr>";
                mtitle = Regex.Match(boTemp[i], strpattern, RegexOptions.IgnoreCase);
                if (mtitle.Success)
                {
                    p_two = mtitle.Groups[0].Value.Trim();

                    //这里取客队红牌数量                    
                    string strpatternHp2 = @"<span class=""rc"">(\d)</span>";
                    Match mtitleHp2 = Regex.Match(p_two, strpatternHp2, RegexOptions.IgnoreCase);
                    if (mtitleHp2.Success)
                    {
                        int hp_two = Utils.ParseInt(mtitleHp2.Groups[1].Value);
                        if (hp_two > 0)
                        {
                            new TPR2.BLL.guess.BaList().Updatep_hp_two(p_id, hp_two);
                        }
                    }

                    string[] p_twoTemp = Regex.Split(p_two, @"<tr class=""alternation"">");

                    p_two = Regex.Replace(p_twoTemp[0], @"<small>\[[\w\d]+\]</small>", "");
                    p_two = Regex.Replace(p_two, @"<span class=""rc"">[\w\d]+</span>", "");

                }
                else
                {
                    strpattern = @"<td>(\d){2}:(\d){2}</td><td class=""teamname"">([\s\S]+)</td></tr>";
                    mtitle = Regex.Match(boTemp[i], strpattern, RegexOptions.IgnoreCase);
                    if (mtitle.Success)
                    {
                        p_two = mtitle.Groups[0].Value.Trim();

                        //这里取客队红牌数量
                        string strpatternHp2 = @"<span class=""rc"">(\d)</span>";
                        Match mtitleHp2 = Regex.Match(p_two, strpatternHp2, RegexOptions.IgnoreCase);
                        if (mtitleHp2.Success)
                        {
                            int hp_two = Utils.ParseInt(mtitleHp2.Groups[1].Value);
                            if (hp_two > 0)
                            {
                                new TPR2.BLL.guess.BaList().Updatep_hp_two(p_id, hp_two);
                            }
                        }


                        string[] p_twoTemp = Regex.Split(p_two, @"<tr class=""alternation"">");

                        p_two = Regex.Replace(p_twoTemp[0], @"<small>\[[\w\d]+\]</small>", "");
                        p_two = Regex.Replace(p_two, @"<span class=""rc"">[\w\d]+</span>", "");
                        if (p_two.Contains("↑"))
                        {
                            p_two = Regex.Split(p_two, "↑")[0];
                        }
                    }
                }
                p_two = Regex.Replace(p_two, @"<td>(\d){2}:(\d){2}</td>", "");
                p_two = Regex.Replace(p_two, strpattern, "");
                p_two = Regex.Replace(p_two, @"<td>\(<em>((\d){1,2}-(\d){1,2})</em>\)</td>", "");
                p_two = Regex.Replace(p_two, @"<.+?>", "");
                if (p_two.Contains("↑"))
                {
                    p_two = Regex.Split(p_two, "↑")[0];
                }
                //Response.Write(p_two + "<br />");
                #endregion

                //这里即时完场比分
                string Result = "";
                strpattern = @"<b class=""score"">((\d){1,2}-(\d){1,2})</b>";
                mtitle = Regex.Match(boTemp[i], strpattern, RegexOptions.IgnoreCase);
                if (mtitle.Success)
                {
                    Result = mtitle.Groups[1].Value;
                    if (Result.Contains("-"))
                    {
                        string[] p_result = Result.Split('-');
                        int p_result_one = Convert.ToInt32(p_result[0]);
                        int p_result_two = Convert.ToInt32(p_result[1]);
                        new TPR2.BLL.guess.BaList().UpdateBoResult(p_id, p_result_one, p_result_two);
                    }
                }
                //更新比赛状态
                if (strState != "")
                {
                    new TPR2.BLL.guess.BaList().UpdateOnce(p_id, strState);
                }
            }
            #endregion

            #region 未完，更新分析链接 析
            if (strState != "未")
            {
                int Min = Utils.ParseInt(strState.Replace("+", "").Replace("'", ""));
                if (Min > 48 || strState == "完" || strState == "中")
                {
                    //这里即时半场比分
                    string Result = "";
                    strpattern = @"\(<em>((\d){1,2}-(\d){1,2})</em>\)";
                    mtitle = Regex.Match(boTemp[i], strpattern, RegexOptions.IgnoreCase);
                    if (mtitle.Success)
                    {
                        Result = mtitle.Groups[1].Value;
                        if (Result.Contains("-"))
                        {
                            //取p_id
                            int p_id = 0;
                            strpattern = @"by=detail&amp;eid=(\d+).>析";
                            mtitle = Regex.Match(boTemp[i], strpattern, RegexOptions.IgnoreCase);
                            if (mtitle.Success)
                            {
                                p_id = Utils.ParseInt(mtitle.Groups[1].Value);
                                //Response.Write(p_id + "<br />");
                            }

                            string[] p_result = Result.Split('-');
                            int p_result_one = Convert.ToInt32(p_result[0]);
                            int p_result_two = Convert.ToInt32(p_result[1]);
                            new TPR2.BLL.guess.BaList().UpdateBoResult(9, p_id, p_result_one, p_result_two);
                        }
                    }
                }
            }
            #endregion
            builder.Append(DateTime.Now.ToString("yyyyMMddHHmmss-fff" + "<br />"));
        }

        #region 采集足球赛事 提示
        Master.Title = "采集足球赛事";
        if (bo.Contains("#NEXT#"))
        {
            Master.Refresh = 10;
            Response.Redirect(Utils.getUrl("boCollec.aspx?k=" + (k + 1) + ""));
            Master.Gourl = Utils.getUrl("boCollec.aspx?k=" + (k + 1) + "");
            builder.Append("第" + k + "页采集结束，正在采集第" + (k + 1) + "页");
        }
        else
        {
            Master.Refresh = 10;
            Master.Gourl = Utils.getUrl("boCollec.aspx");
            builder.Append("第" + k + "页采集结束，正在循环到第1页");
        }
        #endregion
    }

    public string GetFootbolist1x(int Types, int Page)
    {

        //足球的：赛事更新、水位变动、全场滚球（包括封盘）、即时比分、完场比分（加返彩）、比赛进行时间

        //篮球的：赛事、水位、全场滚球（包括封盘）、即时比分、完场比分返彩、比赛进行时间

        string obj = string.Empty;

        string url = string.Empty;
        if (Types == 1)
            url = "http://3g.8bo.com/3g/football/score/today.aspx?by=event&st=hasStart&pagesize=1000";//已开赛
        else if (Types == 2)
            url = "http://3g.8bo.com/3g/football/score/today.aspx?by=event&st=notStart&pagesize=1000";//未开赛
        else if (Types == 3)
            url = "http://3g.8bo.com/3g/football/score/today.aspx?by=event&st=hasCompletedField&pagesize=1000";//已完场

        url = "http://3g.8bo.com/3g/football/score/today.aspx?by=event&st=allEvents&page=" + Page + "";//所有比赛
        if (Page == -1)
        {
            url = "http://3g.8bo.com/3g/football/score/today.aspx?by=event&st=hasStart&pagesize=1000";
        }
        string html = GetSourceTextByUrl(url, "UTF-8");

        if (html != "")
        {
            if (html.Contains(">尾页</a>]"))
                obj = FootbolistHtml(html) + "#NEXT#";
            else
                obj = FootbolistHtml(html);
        }

        return obj;
    }

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

    private string GetSourceTextByUrl(string url, string Encoding)
    {
        try
        {
            System.Net.WebRequest request = System.Net.WebRequest.Create(url);
            request.Timeout = 20000;
            System.Net.WebResponse response = request.GetResponse();

            System.IO.Stream resStream = response.GetResponseStream();
            System.IO.StreamReader sr = new System.IO.StreamReader(resStream, System.Text.Encoding.GetEncoding(Encoding));
            return sr.ReadToEnd();
        }
        catch
        {
            return "";
        }
    }
    private void binddata()
    {
        FileStream fs = new FileStream(Server.MapPath("/Files/sys/loading.gif"), FileMode.Open, FileAccess.Read);
        byte[] mydata = new byte[fs.Length];
        int Length = Convert.ToInt32(fs.Length);
        fs.Read(mydata, 0, Length);
        fs.Close();
        Response.Clear();
        Response.ContentType = "image/gif";
        Response.OutputStream.Write(mydata, 0, Length);
        Response.End();
    }
}