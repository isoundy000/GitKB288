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
/// 【篮球】抓取页面 完成查阅 刷新机用
/// 
/// http://3g.8bo.com/3g/basketball/score/today.aspx?by=event&st=allEvents&page=" + Page + "
/// 
/// 黄国军 20151226
/// ==================================
/// </summary>
public partial class bbs_guess2_boCollec2 : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/guess2.xml";
    protected void Page_Load(object sender, EventArgs e)
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-9]$", "0"));
        int k = int.Parse(Utils.GetRequest("k", "get", 1, @"^[0-9]\d*$", "1"));

        //抓取篮球数据http://3g.8bo.com的数据
        string bo = new TPR2.Collec.Basketbo().GetBasketbolist(ptype, k);
        //获取页面HTML代码
        string[] boTemp = Regex.Split(bo, @"<td class=.W1\s[\w\d]+.>");

        for (int i = 1; i < boTemp.Length; i++)
        {
            #region 取比赛状态 获得 strState值 未、点(这个是代表点球)、完、待定、腰斩、推迟（无这些选项则取的比赛进行分钟数）
            //取比赛状态
            string strState = "";
            string strpattern = @"<td align=""center"">([\s\S]{1,10})</td>";//第N节、未、点(这个是代表点球)、完、待定、腰斩、推迟(无这些选项则取的比赛进行分钟数)
            Match mtitle = Regex.Match(boTemp[i], strpattern, RegexOptions.IgnoreCase);
            if (mtitle.Success)
            {
                strState = mtitle.Groups[1].Value.Trim();
            }
            #endregion

            #region 未完,大小，滚球 的数据等
            if ((strState == "未" && boTemp[i].Contains("<td>↑大小</td>")) || boTemp[i].Contains("<td>↑滾球</td>"))
            {
                #region 取联赛名称 获取title值
                //Response.Write(strState + "<br />");
                //取联赛名称
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
                    p_one = Regex.Replace(p_one, @"<small>\[[\w\d]+\]</small>", "");
                    p_one = Regex.Replace(p_one, @"<span class=""rc"">[\w\d]+</span>", "");
                    p_one = Regex.Replace(p_one, @"<.+?>", "");
                    //Response.Write(p_one + "<br />");
                }
                #endregion

                #region 取客队名称 p_two
                //取客队名称
                string p_two = "";
                strpattern = @"<td>(\d){2}:(\d){2}</td><td class=""teamname"">([\s\S]+)<small>\[[\w\d]+\]</small>";
                mtitle = Regex.Match(boTemp[i], strpattern, RegexOptions.IgnoreCase);
                if (mtitle.Success)
                {
                    p_two = mtitle.Groups[0].Value.Trim();
                    string[] p_twoTemp = Regex.Split(p_two, @"<tr class=""alternation"">");

                    p_two = Regex.Replace(p_twoTemp[0], @"<small>\[[\w\d]+\]</small>", "");
                    p_two = Regex.Replace(p_two, @"<span class=""rc"">[\w\d]+</span>", "");
                    p_two = Regex.Replace(p_two, @"<td>(\d){2}:(\d){2}</td>", "");
                    p_two = Regex.Replace(p_two, @"<.+?>", "");
                    //Response.Write(p_two + "<br />");
                }
                else
                {
                    strpattern = @"<td>(\d){2}:(\d){2}</td><td class=""teamname"">([\s\S]+)</td></tr>";
                    mtitle = Regex.Match(boTemp[i], strpattern, RegexOptions.IgnoreCase);
                    if (mtitle.Success)
                    {
                        p_two = mtitle.Groups[0].Value.Trim();
                        string[] p_twoTemp = Regex.Split(p_two, @"<tr class=""alternation"">");

                        p_two = Regex.Replace(p_twoTemp[0], @"<small>\[[\w\d]+\]</small>", "");
                        if (p_two.Contains("↑"))
                        {
                            p_two = Regex.Split(p_two, "↑")[0];
                        }
                        p_two = Regex.Replace(p_two, @"<td>(\d){2}:(\d){2}</td>", "");
                        p_two = Regex.Replace(p_two, @"<td colspan=""2"">\[<b class=""score"">((\d){1,}-(\d){1,})</b>\][\s\S]+", "");
                        p_two = Regex.Replace(p_two, @"<.+?>", "");
                        //Response.Write(p_two + "<br />");

                    }
                }
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
                #endregion

                #region 更新进数据库 TPR2.Model.guess.BaList 初始值
                //==================================更新进数据库====================================
                if (Convert.ToDateTime(Time) <= DateTime.Now.AddHours(Convert.ToDouble(ub.GetSub("SiteJcTime", xmlPath))))
                {
                    if (ub.GetSub("Sitelqstat", xmlPath).IndexOf(title) != -1)
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
                        model.p_big_lu = p_big_lu;
                        model.p_small_lu = p_small_lu;
                        model.p_bzs_lu = 0;
                        model.p_bzp_lu = 0;
                        model.p_bzx_lu = 0;
                        model.p_addtime = DateTime.Now;
                        model.p_TPRtime = p_TPRtime;
                        model.p_ison = 0;
                        TPR2.BLL.guess.BaList bll = new TPR2.BLL.guess.BaList();
                        if (!bll.ExistsByp_id(Convert.ToInt32(p_id)))
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
                            bll.Add(model);
                            //Response.Write("<br />====================<br />");
                        }
                    }
                }
                //Response.Write("<br />--------------------<br />");
                //==================================更新进数据库====================================
                #endregion

                #region 更新为滚球模式
                if (boTemp[i].Contains("<td>↑滾球</td>"))
                {
                    if (ub.GetSub("Sitegqstat2", "/Controls/guess2.xml").IndexOf(title) != -1)
                    {
                        //更新为滚球模式
                        new TPR2.BLL.guess.BaList().FootOnceType2(p_id, p_TPRtime.AddMinutes(150));
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
                //这里即时完场比分
                string Result = "";
                strpattern = @"<b class=""score"">((\d){1,3}-(\d){1,3})</b>";
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
        }

        #region 采集篮球赛事 提示
        Master.Title = "采集篮球赛事";
        if (bo.Contains("#NEXT#"))
        {
            Master.Refresh = 10;
            Master.Gourl = Utils.getUrl("boCollec2.aspx?k=" + (k + 1) + "");
            builder.Append("第" + k + "页采集结束，正在采集第" + (k + 1) + "页");
        }
        else
        {
            Master.Refresh = 10;
            Master.Gourl = Utils.getUrl("boCollec2.aspx");
            builder.Append("第" + k + "页采集结束，正在循环到第1页");
        }
        #endregion       
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