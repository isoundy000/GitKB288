using System;
using System.Collections.Generic;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text.RegularExpressions;
using BCW.Common;

/// <summary>
/// 蒙宗将 20161027 新开发 10.28
/// 蒙宗将 20161029 优化机器人
/// 蒙宗将 20161101 优化
/// 蒙宗将 20161103 动态修复
///        20161122 增加浮动额度
///        20161124 动态去掉玩法 25 增加ID限额
///        20161126 连停优化
/// </summary>

public partial class Robot_SSCRobot : System.Web.UI.Page
{
    protected string xmlPath = "/Controls/ssc.xml";
    protected string GameName = ub.GetSub("SSCName", "/Controls/ssc.xml");
    protected void Page_Load(object sender, EventArgs e)
    {
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();

        //防止缓存
        Response.Buffer = true;
        Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
        Response.Expires = 0;
        Response.CacheControl = "no-cache";

        if (ub.GetSub("SSCIsBot", xmlPath) != "1")
        {
            Response.Write(GameName + "机器人已经关闭close1");
        }
        else
        {
            ChangePalySSC();
            Response.Write(GameName + "[1]机器人正在工作中ok1" + "<br />");
            //   Case();
        }

        Response.Write("<b>上次获取时间：</b>" + DateTime.Now + "<br />");
        stopwatch.Stop();
        Response.Write("总耗时:" + stopwatch.Elapsed.TotalSeconds + "秒");
    }
    /// <summary>
    /// 时时彩自动游戏程序
    /// </summary>
    /// <param name="SSCId">局数ID</param>
    /// <param name="dt">截止时间</param>
    private void ChangePalySSC()
    {
        //进行自动下注
        int hour = DateTime.Now.Hour;
        if (hour > 2 && hour < 9)
        {
            Response.Write(GameName + "机器人正在休息中close1");
        }
        else
        {
            int LianTing = Convert.ToInt32(ub.GetSub("SSCLianTing", xmlPath));//连停期数 只考虑ID，不考虑数据库里面的期数
            int IDlastopen = 0;
            int IDlastnew = 0;
            DataSet dslastopen = new BCW.ssc.BLL.SSClist().GetList("Top(1) ID,SSCId,Result,State", " State=1 Order by ID DESC");
            if (dslastopen != null && dslastopen.Tables[0].Rows.Count > 0)
            {
                IDlastopen = Convert.ToInt32(dslastopen.Tables[0].Rows[0]["ID"]);//最后一条开奖的ID
            }
            DataSet dslastnew = new BCW.ssc.BLL.SSClist().GetList("Top(1) ID,SSCId,Result,State", " State=0 Order by ID DESC");
            if (dslastnew != null && dslastnew.Tables[0].Rows.Count > 0)
            {
                IDlastnew = Convert.ToInt32(dslastnew.Tables[0].Rows[0]["ID"]);//最新期数
            }

            if (dslastnew.Tables[0].Rows.Count >= LianTing)
            {
                if (IDlastnew - IDlastopen >= LianTing)//判断连停
                {
                    Response.Write(GameName + "由于开奖出现未开的期数大于最大连停期数，机器人停止工作ok1");
                }
            }
            else
            {
                try
                {
                    PlaySSC();
                }
                catch { }
                Response.Write(GameName + "[2]机器人正在工作中ok1<br />");

            }

        }

    }
    private void PlaySSC()
    {
        int meid = GetUsID();//下注ID
        if (!new BCW.BLL.User().Exists(meid))
        {
            Response.Write("机器人不存在ok1");
            Response.End();//
        }
        ChanageOnline(meid);//
        int ptype = GetPtype();//下注方式

        string TypeTitle = OutType(ptype);//玩法
        decimal Odds = 0;//赔率

        BCW.ssc.Model.SSClist model = new BCW.ssc.BLL.SSClist().GetSSClistLast();
        if (model.ID == 0)
        {
            // Response.End();
            Response.Write("机器人被停止了！ok1");
        }

        int Sec = Utils.ParseInt(ub.GetSub("SSCSec", xmlPath));
        if (model.EndTime > DateTime.Now.AddSeconds(Sec))
        {
            #region 下注，玩法，注数，赔率
            Random ra = new Random(unchecked((int)DateTime.Now.Ticks));
            string accNum = string.Empty;
            int iZhu = 0;

            if (ptype == 1)
            {
                string num = string.Empty;
                num = PlayNum(1, 10);
                accNum = num;
                iZhu = num.Split(',').Length;
                Odds = Convert.ToDecimal(OutOdds(ptype, 1));
            }
            else if (ptype == 4)
            {
                string num = string.Empty;
                num = PlayNum(1, 10);
                accNum = num;
                iZhu = num.Split(',').Length;
                Odds = Convert.ToDecimal(OutOdds(ptype, 1));
            }
            else if (ptype == 7)
            {
                string num = string.Empty;
                num = PlayNum(1, 10);
                accNum = num;
                iZhu = num.Split(',').Length;
                Odds = Convert.ToDecimal(OutOdds(ptype, 1));
            }
            else if (ptype == 10)
            {
                string num = string.Empty;
                num = PlayNum(1, 10);
                accNum = num;
                iZhu = num.Split(',').Length;
                Odds = Convert.ToDecimal(OutOdds(ptype, 1));
            }
            else if (ptype == 13)
            {
                string num = string.Empty;
                num = PlayNum(1, 10);
                accNum = num;
                iZhu = num.Split(',').Length;
                Odds = Convert.ToDecimal(OutOdds(ptype, 1));
            }
            else if (ptype == 16)
            {
                string num = string.Empty;
                Random r = new Random(unchecked((int)DateTime.Now.Ticks));
                num = r.Next(0, 10).ToString();
                accNum = num;
                iZhu = 1;
                Odds = Convert.ToDecimal(OutOdds(ptype, 1));
            }
            else if (ptype == 18)
            {
                string num = string.Empty;
                num = PlayNum(1, 10);
                accNum = "" + num + "";
                iZhu = num.Split(',').Length;
                Odds = Convert.ToDecimal(OutOdds(ptype, 1));
            }
            else if (ptype == 19)
            {
                string num = string.Empty;
                string num1 = PlayNum(0, 1);
                string num2 = PlayNum(2, 10);
                string num3 = string.Empty;
                if (num1 != "")
                {
                    num3 = num2.Replace(num1, "");
                    if (num3.Substring(num3.Length - 1, 1) == ",")
                    {
                        num3 = num3.Remove(num3.Length - 1);
                    }
                    if (num3.Substring(0, 1) == ",")
                    {
                        num3 = num3.Substring(1, num3.Length - 1);
                    }
                    num3 = num3.Replace(",,", ",");
                }

                if (num1.Split(',').Length == 1)
                {
                    if (num3.Length > 1 && !num3.Contains(","))
                    {
                        num = "";
                    }
                    else
                    {
                        num = num1 + "#" + num3;
                    }
                    iZhu = num3.Split(',').Length;//有一个胆码，注数为拖码的个数

                }
                else
                {
                    if (num2.Length > 1 && !num2.Contains(","))
                    {
                        num = "";
                    }
                    else
                    {
                        num = num1 + "#" + num2;
                    }
                    iZhu = C(num2.Split(',').Length, 2);//无胆码，注数为拖码的组合选2
                }
                accNum = num;
                Odds = Convert.ToDecimal(OutOdds(ptype, 1));
            }
            else if (ptype == 20)
            {
                string num = string.Empty;
                string num1 = PlayNum(0, 2);
                string num2 = PlayNum(3, 10);
                string num3 = string.Empty;
                if (num1.Split(',').Length == 1)
                {
                    if (num1 != "")
                    {
                        num3 = num2.Replace(num1, "");
                    }
                    if (num3 != "")
                    {
                        if (num3.Substring(num3.Length - 1, 1) == ",")
                        {
                            num3 = num3.Remove(num3.Length - 1);
                        }
                        if (num3.Substring(0, 1) == ",")
                        {
                            num3 = num3.Substring(1, num3.Length - 1);
                        }
                    }
                    num3 = num3.Replace(",,", ",");
                }
                if (num1.Split(',').Length == 2)
                {
                    num3 = num2.Replace(num1.Substring(0, 1), "");
                    num3 = num3.Replace(num1.Replace(num1.Substring(0, 1), ""), "");
                    if (num3.Substring(num3.Length - 1, 1) == ",")
                    {
                        num3 = num3.Remove(num3.Length - 1);
                    }
                    if (num3.Substring(0, 1) == ",")
                    {
                        num3 = num3.Substring(1, num3.Length - 1);
                    }
                    num3 = num3.Replace(",,", ",");
                }

                if (num1.Split(',').Length == 2)
                {
                    if (num3.Length > 1 && !num3.Contains(","))
                    {
                        num = "";
                    }
                    else
                    {
                        num = num1 + "#" + num3;
                    }
                    iZhu = num3.Split(',').Length;//有2个胆码
                }
                else if (num1.Split(',').Length == 1)
                {
                    if (num3.Length > 1 && !num3.Contains(","))
                    {
                        num = "";
                    }
                    else
                    {
                        num = num1 + "#" + num3;
                    }
                    iZhu = C(num3.Split(',').Length, 2);
                }
                else
                {
                    if (num2.Length > 1 && !num2.Contains(","))
                    {
                        num = "";
                    }
                    else
                    {
                        num = num1 + "#" + num2;
                    }
                    iZhu = C(num2.Split(',').Length, 3);//无胆码，注数为拖码的组合选3
                }
                accNum = num;
                Odds = Convert.ToDecimal(OutOdds(ptype, 1));
            }
            else if (ptype == 21)
            {
                string num = string.Empty;
                string num1 = PlayNum(0, 3);
                string num2 = PlayNum(4, 10);
                string num3 = string.Empty;
                if (num1.Split(',').Length == 1)
                {
                    if (num1 != "")
                    {
                        num3 = num2.Replace(num1, "");
                    }
                    if (num3 != "")
                    {
                        if (num3.Substring(num3.Length - 1, 1) == ",")
                        {
                            num3 = num3.Remove(num3.Length - 1);
                        }
                        if (num3.Substring(0, 1) == ",")
                        {
                            num3 = num3.Substring(1, num3.Length - 1);
                        }
                    }
                    num3 = num3.Replace(",,", ",");
                }
                if (num1.Split(',').Length == 2)
                {
                    num3 = num2.Replace(num1.Substring(0, 1), "");
                    num3 = num3.Replace(num1.Replace(num1.Substring(0, 1), ""), "");
                    if (num3.Substring(num3.Length - 1, 1) == ",")
                    {
                        num3 = num3.Remove(num3.Length - 1);
                    }
                    if (num3.Substring(0, 1) == ",")
                    {
                        num3 = num3.Substring(1, num3.Length - 1);
                    }
                    num3 = num3.Replace(",,", ",");
                }
                if (num1.Split(',').Length == 3)
                {
                    string d = string.Empty;
                    string a = num1;
                    string c;
                    c = a.Substring(0, 1);//#1
                    d = a.Replace(c, "");
                    c = a.Replace(a.Substring(0, 1), "").Substring(0, 1);//#2
                    d = d.Replace(c, "");//#3

                    num3 = num2.Replace(a.Substring(0, 1), "");
                    num3 = num3.Replace(c, "");
                    num3 = num3.Replace(d, "");
                    if (num3.Substring(num3.Length - 1, 1) == ",")
                    {
                        num3 = num3.Remove(num3.Length - 1);
                    }
                    if (num3.Substring(0, 1) == ",")
                    {
                        num3 = num3.Substring(1, num3.Length - 1);
                    }
                    num3 = num3.Replace(",,", ",");
                }

                if (num1.Split(',').Length == 3)
                {
                    if (num3.Length > 1 && !num3.Contains(","))
                    {
                        num = "";
                    }
                    else
                    {
                        num = num1 + "#" + num3;
                    }
                    iZhu = num3.Split(',').Length;
                }
                else if (num1.Split(',').Length == 2)
                {
                    if (num3.Length > 1 && !num3.Contains(","))
                    {
                        num = "";
                    }
                    else
                    {
                        num = num1 + "#" + num3;
                    }
                    iZhu = C(num3.Split(',').Length, 2);
                }
                else if (num1.Split(',').Length == 1)
                {
                    if (num3.Length > 1 && !num3.Contains(","))
                    {
                        num = "";
                    }
                    else
                    {
                        num = num1 + "#" + num3;
                    }
                    iZhu = C(num3.Split(',').Length, 3);
                }
                else
                {
                    if (num2.Length > 1 && !num2.Contains(","))
                    {
                        num = "";
                    }
                    else
                    {
                        num = num1 + "#" + num2;
                    }
                    iZhu = C(num2.Split(',').Length, 4);//无胆码，注数为拖码的组合选4
                }

                accNum = num;
                Odds = Convert.ToDecimal(OutOdds(ptype, 1));
            }
            else if (ptype == 22)
            {
                string num = string.Empty;
                string num1 = PlayNum(0, 4);
                string num2 = PlayNum(5, 10);
                string num3 = string.Empty;
                if (num1.Split(',').Length == 1)
                {
                    if (num1 != "")
                    {
                        num3 = num2.Replace(num1, "");
                    }
                    if (num3 != "")
                    {
                        if (num3.Substring(num3.Length - 1, 1) == ",")
                        {
                            num3 = num3.Remove(num3.Length - 1);
                        }
                        if (num3.Substring(0, 1) == ",")
                        {
                            num3 = num3.Substring(1, num3.Length - 1);
                        }
                    }
                    num3 = num3.Replace(",,", ",");
                }
                if (num1.Split(',').Length == 2)
                {
                    num3 = num2.Replace(num1.Substring(0, 1), "");
                    num3 = num3.Replace(num1.Replace(num1.Substring(0, 1), ""), "");
                    if (num3.Substring(num3.Length - 1, 1) == ",")
                    {
                        num3 = num3.Remove(num3.Length - 1);
                    }
                    if (num3.Substring(0, 1) == ",")
                    {
                        num3 = num3.Substring(1, num3.Length - 1);
                    }
                    num3 = num3.Replace(",,", ",");
                }
                if (num1.Split(',').Length == 3)
                {
                    string d = string.Empty;
                    string a = num1;
                    string c;
                    c = a.Substring(0, 1);//#1
                    d = a.Replace(c, "");
                    c = a.Replace(a.Substring(0, 1), "").Substring(0, 1);//#2
                    d = d.Replace(c, "");//#3

                    num3 = num2.Replace(a.Substring(0, 1), "");
                    num3 = num3.Replace(c, "");
                    num3 = num3.Replace(d, "");
                    if (num3.Substring(num3.Length - 1, 1) == ",")
                    {
                        num3 = num3.Remove(num3.Length - 1);
                    }
                    if (num3.Substring(0, 1) == ",")
                    {
                        num3 = num3.Substring(1, num3.Length - 1);
                    }
                    num3 = num3.Replace(",,", ",");
                }
                if (num1.Split(',').Length == 4)
                {
                    string d = string.Empty;
                    string a = num1;
                    string f = string.Empty;
                    string c;
                    string aa;
                    c = a.Substring(0, 1);//#1
                    d = a.Replace(c, "");
                    c = a.Replace(a.Substring(0, 1), "").Substring(0, 1);//#2
                    d = d.Replace(c, "").Substring(0, 1);//#3
                    aa = a.Replace(a.Substring(0, 1), "");
                    aa = aa.Replace(c, "");
                    aa = aa.Replace(d, "");

                    num3 = num2.Replace(a.Substring(0, 1), "");
                    num3 = num3.Replace(c, "");
                    num3 = num3.Replace(d, "");
                    num3 = num3.Replace(aa, "");
                    if (num3.Substring(num3.Length - 1, 1) == ",")
                    {
                        num3 = num3.Remove(num3.Length - 1);
                    }
                    if (num3.Substring(0, 1) == ",")
                    {
                        num3 = num3.Substring(1, num3.Length - 1);
                    }
                    num3 = num3.Replace(",,", ",");
                }

                if (num1.Split(',').Length == 4)
                {
                    if (num3.Length > 1 && !num3.Contains(","))
                    {
                        num = "";
                    }
                    else
                    {
                        num = num1 + "#" + num3;
                    }
                    iZhu = num3.Split(',').Length;
                }
                else if (num1.Split(',').Length == 3)
                {
                    if (num3.Length > 1 && !num3.Contains(","))
                    {
                        num = "";
                    }
                    else
                    {
                        num = num1 + "#" + num3;
                    }
                    iZhu = C(num3.Split(',').Length, 2);
                }
                else if (num1.Split(',').Length == 2)
                {
                    if (num3.Length > 1 && !num3.Contains(","))
                    {
                        num = "";
                    }
                    else
                    {
                        num = num1 + "#" + num3;
                    }
                    iZhu = C(num3.Split(',').Length, 3);
                }
                else if (num1.Split(',').Length == 1)
                {
                    if (num3.Length > 1 && !num3.Contains(","))
                    {
                        num = "";
                    }
                    else
                    {
                        num = num1 + "#" + num3;
                    }
                    iZhu = C(num3.Split(',').Length, 4);
                }
                else
                {
                    if (num2.Length > 1 && !num2.Contains(","))
                    {
                        num = "";
                    }
                    else
                    {
                        num = num1 + "#" + num2;
                    }
                    iZhu = C(num2.Split(',').Length, 5);//无胆码，注数为拖码的组合选5
                }

                accNum = num;
                Odds = Convert.ToDecimal(OutOdds(ptype, 1));
            }
            else if (ptype == 24)
            {
                string num = string.Empty;
                num = PlayNum(1, 10);
                accNum = num;
                iZhu = num.Split(',').Length;
                Odds = Convert.ToDecimal(OutOdds(ptype, 1));
            }
            else if (ptype == 27)
            {
                string num = string.Empty;
                Random r = new Random(unchecked((int)DateTime.Now.Ticks));
                int a = r.Next(1, 6);
                if (a == 1)
                {
                    num = "一门";
                    Odds = Convert.ToDecimal(OutOdds(ptype, 1));
                }
                else if (a == 2)
                {
                    num = "二门";
                    Odds = Convert.ToDecimal(OutOdds(ptype, 2));
                }
                else if (a == 3)
                {
                    num = "三门";
                    Odds = Convert.ToDecimal(OutOdds(ptype, 3));
                }
                else if (a == 4)
                {
                    num = "四门";
                    Odds = Convert.ToDecimal(OutOdds(ptype, 4));
                }
                else
                {
                    num = "五门";
                    Odds = Convert.ToDecimal(OutOdds(ptype, 5));
                }
                accNum = num;
                iZhu = 1;
            }
            else if (ptype == 2 || ptype == 5 || ptype == 8 || ptype == 11 || ptype == 14 || ptype == 25 || ptype == 36 || ptype == 43 || ptype == 50)
            {
                string num = string.Empty;
                Random r = new Random(unchecked((int)DateTime.Now.Ticks));
                int a = r.Next(1, 11);
                if (a < 6)
                {
                    num = "大";
                    Odds = Convert.ToDecimal(OutOdds(ptype, 1));
                }
                else
                {
                    num = "小";
                    Odds = Convert.ToDecimal(OutOdds(ptype, 2));
                }
                accNum = num;
                iZhu = 1;
            }
            else if (ptype == 3 || ptype == 6 || ptype == 9 || ptype == 12 || ptype == 15 || ptype == 26 || ptype == 37 || ptype == 44 || ptype == 51)
            {
                string num = string.Empty;
                Random r = new Random(unchecked((int)DateTime.Now.Ticks));
                int a = r.Next(1, 11);
                if (a < 6)
                {
                    num = "单";
                    Odds = Convert.ToDecimal(OutOdds(ptype, 1));
                }
                else
                {
                    num = "双";
                    Odds = Convert.ToDecimal(OutOdds(ptype, 2));
                }
                accNum = num;
                iZhu = 1;
            }
            else if (ptype == 17)
            {
                string num = string.Empty;
                Random r = new Random(unchecked((int)DateTime.Now.Ticks));
                int a = r.Next(1, 4);
                if (a == 1)
                {
                    num = "龙";
                    Odds = Convert.ToDecimal(OutOdds(ptype, 1));
                }
                else if (a == 2)
                {
                    num = "虎";
                    Odds = Convert.ToDecimal(OutOdds(ptype, 2));
                }
                else
                {
                    num = "和";
                    Odds = Convert.ToDecimal(OutOdds(ptype, 3));
                }
                accNum = num;
                iZhu = 1;
            }
            else if (ptype == 23)
            {
                string num = string.Empty;
                Random r = new Random(unchecked((int)DateTime.Now.Ticks));
                int a = r.Next(1, 3);
                if (a == 1)
                {
                    num = "有牛";
                    Odds = Convert.ToDecimal(OutOdds(ptype, 1));
                }
                else
                {
                    num = "无牛";
                    Odds = Convert.ToDecimal(OutOdds(ptype, 2));
                }
                accNum = num;
                iZhu = 1;
            }
            else if (ptype == 28)
            {
                string num = string.Empty;
                Random r = new Random(unchecked((int)DateTime.Now.Ticks));
                int a = r.Next(1, 5);
                if (a == 1)
                {
                    num = "大单";
                    Odds = Convert.ToDecimal(OutOdds(ptype, 1));
                }
                else if (a == 2)
                {
                    num = "大双";
                    Odds = Convert.ToDecimal(OutOdds(ptype, 2));
                }
                else if (a == 3)
                {
                    num = "小单";
                    Odds = Convert.ToDecimal(OutOdds(ptype, 2));
                }
                else
                {
                    num = "小双";
                    Odds = Convert.ToDecimal(OutOdds(ptype, 1));
                }
                accNum = num;
                iZhu = 1;
            }
            else if (ptype == 29)
            {
                string num = string.Empty;
                num = "炸弹";
                accNum = num;
                iZhu = 1;
                Odds = Convert.ToDecimal(OutOdds(ptype, 1));
            }
            else if (ptype == 30)
            {
                string num = string.Empty;
                num = "葫芦";
                accNum = num;
                iZhu = 1;
                Odds = Convert.ToDecimal(OutOdds(ptype, 1));
            }
            else if (ptype == 31)
            {
                string num = string.Empty;
                num = "顺子";
                accNum = num;
                iZhu = 1;
                Odds = Convert.ToDecimal(OutOdds(ptype, 1));
            }
            else if (ptype == 32)
            {
                string num = string.Empty;
                num = "三条";
                accNum = num;
                iZhu = 1;
                Odds = Convert.ToDecimal(OutOdds(ptype, 1));
            }
            else if (ptype == 33)
            {
                string num = string.Empty;
                num = "两对";
                accNum = num;
                iZhu = 1;
                Odds = Convert.ToDecimal(OutOdds(ptype, 1));
            }
            else if (ptype == 34)
            {
                string num = string.Empty;
                num = "单对";
                accNum = num;
                iZhu = 1;
                Odds = Convert.ToDecimal(OutOdds(ptype, 1));
            }
            else if (ptype == 35)
            {
                string num = string.Empty;
                num = "散牌";
                accNum = num;
                iZhu = 1;
                Odds = Convert.ToDecimal(OutOdds(ptype, 1));
            }
            else if (ptype == 38 || ptype == 45 || ptype == 52)
            {
                string num = string.Empty;
                num = "豹子";
                accNum = num;
                iZhu = 1;
                Odds = Convert.ToDecimal(OutOdds(ptype, 1));
            }
            else if (ptype == 39 || ptype == 46 || ptype == 53)
            {
                string num = string.Empty;
                num = "顺子";
                accNum = num;
                iZhu = 1;
                Odds = Convert.ToDecimal(OutOdds(ptype, 1));
            }
            else if (ptype == 40 || ptype == 47 || ptype == 54)
            {
                string num = string.Empty;
                num = "对子";
                accNum = num;
                iZhu = 1;
                Odds = Convert.ToDecimal(OutOdds(ptype, 1));
            }
            else if (ptype == 41 || ptype == 48 || ptype == 55)
            {
                string num = string.Empty;
                num = "半顺";
                accNum = num;
                iZhu = 1;
                Odds = Convert.ToDecimal(OutOdds(ptype, 1));
            }
            else if (ptype == 42 || ptype == 49 || ptype == 56)
            {
                string num = string.Empty;
                num = "杂六";
                accNum = num;
                iZhu = 1;
                Odds = Convert.ToDecimal(OutOdds(ptype, 1));
            }
            #endregion

            long Price = GetPayCent();//下注金额
            long gold = new BCW.BLL.User().GetGold(meid);
            long prices = Convert.ToInt64(Price * iZhu);
            string mename = new BCW.BLL.User().GetUsName(meid);

            long SmallPay = Utils.ParseInt64(ub.GetSub("SSCSmallPay", xmlPath));
            long BigPay = Utils.ParseInt64(ub.GetSub("SSCBigPay", xmlPath));

            if (prices >= SmallPay && prices <= BigPay)
            {

                long xPrices = Utils.ParseInt64(ub.GetSub("SSCPrice", xmlPath));//每个ＩＤ每期限购金额
                if (xPrices > 0 && gold >= 0)
                {
                    long oPrices = new BCW.ssc.BLL.SSCpay().GetSumPrices(meid, model.SSCId);
                    if (oPrices + prices <= xPrices)
                    {

                        #region 每期每玩法每ID投注上限
                        if (ptype == 17 || ptype == 28 || ptype == 2 || ptype == 3 || ptype == 5 || ptype == 6 || ptype == 8 || ptype == 9 || ptype == 11 || ptype == 12 || ptype == 14 || ptype == 15 || ptype == 25 || ptype == 26 || ptype == 36 || ptype == 37 || ptype == 43 || ptype == 44 || ptype == 50 || ptype == 51)// || ptype == 23
                        {
                            long ptyPrices = 0;
                            try
                            {
                                ptyPrices = Convert.ToInt64(OutOddscTid(ptype));
                            }
                            catch { ptyPrices = 0; }
                            if (ptyPrices > 0)
                            {
                                long oPricesid = new BCW.ssc.BLL.SSCpay().GetSumPrices(meid, model.SSCId, ptype);
                                if (oPricesid + prices > ptyPrices)
                                {
                                    Response.Write("当期投注超额ok1");
                                    Response.End();
                                }
                            }
                        }
                        else
                        {
                            long ptyPrices = 0;
                            try
                            {
                                ptyPrices = Convert.ToInt64(OutOddscid(ptype));
                            }
                            catch { ptyPrices = 0; }
                            if (ptyPrices > 0)
                            {
                                long oPricesid = new BCW.ssc.BLL.SSCpay().GetSumPrices(meid, model.SSCId, ptype);
                                if (oPricesid + prices > ptyPrices)
                                {
                                    Response.Write("当期投注超额ok1");
                                    Response.End();
                                }
                            }
                        }
                        #endregion

                        #region 投注上限
                        long xPricesc = 0;
                        //投注上限
                        if (ptype == 1 || ptype == 4 || ptype == 7 || ptype == 10 || ptype == 13 || (ptype >= 16 && ptype <= 22) || ptype == 24 || (ptype >= 29 && ptype <= 35) || (ptype >= 38 && ptype <= 42) || (ptype >= 45 && ptype <= 49) || (ptype >= 52 && ptype <= 56))
                        {
                            xPricesc = Convert.ToInt64(OutOddsc(ptype));
                            if (xPricesc > 0)
                            {
                                long oPricesc = new BCW.ssc.BLL.SSCpay().GetSumPricebyTypes(ptype, model.SSCId);
                                if (oPricesc + prices > xPricesc)
                                {
                                    Response.Write("投注超额ok1");
                                    Response.End();
                                }
                            }
                        }
                        else if (ptype == 23)//有牛无牛上限
                        {
                            string yn = OutOddsc(ptype);
                            string[] yny = yn.Split('|');
                            if (accNum == "有牛")
                            {
                                xPricesc = Convert.ToInt64(yny[0]);
                                long oPricesc = new BCW.ssc.BLL.SSCpay().GetSumPriceby23(23, model.SSCId, 1);
                                if (oPricesc + prices > xPricesc)
                                {
                                    Response.Write("投注超额ok1");
                                    Response.End();
                                }
                            }
                            if (accNum == "无牛")
                            {
                                xPricesc = Convert.ToInt64(yny[1]);
                                long oPricesc = new BCW.ssc.BLL.SSCpay().GetSumPriceby23(23, model.SSCId, 2);
                                if (oPricesc + prices > xPricesc)
                                {
                                    Response.Write("投注超额ok1");
                                    Response.End();
                                }
                            }
                        }
                        else if (ptype == 27)//五门上限
                        {
                            string yn = OutOddsc(ptype);
                            string[] yny = yn.Split('|');
                            if (accNum == "一门")
                            {
                                xPricesc = Convert.ToInt64(yny[0]);
                                long oPricesc = new BCW.ssc.BLL.SSCpay().GetSumPriceby27(27, model.SSCId, 1);
                                if (oPricesc + prices > xPricesc)
                                {
                                    Response.Write("投注超额ok1");
                                    Response.End();
                                }
                            }
                            if (accNum == "二门")
                            {
                                xPricesc = Convert.ToInt64(yny[1]);
                                long oPricesc = new BCW.ssc.BLL.SSCpay().GetSumPriceby27(27, model.SSCId, 2);
                                if (oPricesc + prices > xPricesc)
                                {
                                    Response.Write("投注超额ok1");
                                    Response.End();
                                }
                            }
                            if (accNum == "三门")
                            {
                                xPricesc = Convert.ToInt64(yny[2]);
                                long oPricesc = new BCW.ssc.BLL.SSCpay().GetSumPriceby27(27, model.SSCId, 3);
                                if (oPricesc + prices > xPricesc)
                                {
                                    Response.Write("投注超额ok1");
                                    Response.End();
                                }
                            }
                            if (accNum == "四门")
                            {
                                xPricesc = Convert.ToInt64(yny[3]);
                                long oPricesc = new BCW.ssc.BLL.SSCpay().GetSumPriceby27(27, model.SSCId, 4);
                                if (oPricesc + prices > xPricesc)
                                {
                                    Response.Write("投注超额ok1");
                                    Response.End();
                                }
                            }
                            if (accNum == "五门")
                            {
                                xPricesc = Convert.ToInt64(yny[4]);
                                long oPricesc = new BCW.ssc.BLL.SSCpay().GetSumPriceby27(27, model.SSCId, 5);
                                if (oPricesc + prices > xPricesc)
                                {
                                    Response.Write("投注超额ok1");
                                    Response.End();
                                }
                            }
                        }
                        else//浮动额度
                        {
                            xPricesc = Convert.ToInt64(OutOddsc(ptype));
                            if (ptype == 2 || ptype == 5 || ptype == 8 || ptype == 11 || ptype == 14 || ptype == 25 || ptype == 36 || ptype == 43 || ptype == 50)//大小浮动
                            {
                                long Cent = new BCW.ssc.BLL.SSCpay().GetSumPricebyDX(ptype, model.SSCId, 1);
                                long Cent2 = new BCW.ssc.BLL.SSCpay().GetSumPricebyDX(ptype, model.SSCId, 2);

                                if (accNum == "大")
                                {
                                    if (Math.Abs(Cent + prices - Cent2) > xPricesc)
                                    {
                                        Response.Write("投注超额ok1");
                                        Response.End();
                                    }
                                }
                                if (accNum == "小")
                                {
                                    if (Math.Abs(Cent2 + prices - Cent) > xPricesc)
                                    {
                                        Response.Write("投注超额ok1");
                                        Response.End();
                                    }
                                }

                            }
                            else if (ptype == 3 || ptype == 6 || ptype == 9 || ptype == 12 || ptype == 15 || ptype == 26 || ptype == 37 || ptype == 44 || ptype == 51)//单双浮动
                            {
                                long Cent = new BCW.ssc.BLL.SSCpay().GetSumPricebyDS(ptype, model.SSCId, 1);
                                long Cent2 = new BCW.ssc.BLL.SSCpay().GetSumPricebyDS(ptype, model.SSCId, 2);

                                if (accNum == "单")
                                {
                                    if (Math.Abs(Cent + prices - Cent2) > xPricesc)
                                    {
                                        Response.Write("投注超额ok1");
                                        Response.End();
                                    }
                                }
                                if (accNum == "双")
                                {
                                    if (Math.Abs(Cent2 + prices - Cent) > xPricesc)
                                    {
                                        Response.Write("投注超额ok1");
                                        Response.End();
                                    }
                                }
                            }
                            else if (ptype == 28)
                            {
                                if (accNum == "大单" || accNum == "大双")
                                {
                                    long Cent = new BCW.ssc.BLL.SSCpay().GetSumPricebyHD(ptype, model.SSCId, 1);
                                    long Cent2 = new BCW.ssc.BLL.SSCpay().GetSumPricebyHD(ptype, model.SSCId, 2);
                                    if (accNum == "大单")
                                    {
                                        if (Math.Abs(Cent + prices - Cent2) > xPricesc)
                                        {
                                            Response.Write("投注超额ok1");
                                            Response.End();
                                        }
                                    }
                                    if (accNum == "大双")
                                    {
                                        if (Math.Abs(Cent2 + prices - Cent) > xPricesc)
                                        {
                                            Response.Write("投注超额ok1");
                                            Response.End();
                                        }
                                    }
                                }
                                if (accNum == "小单" || accNum == "小双")
                                {
                                    long Cent = new BCW.ssc.BLL.SSCpay().GetSumPricebyHDx(ptype, model.SSCId, 1);
                                    long Cent2 = new BCW.ssc.BLL.SSCpay().GetSumPricebyHDx(ptype, model.SSCId, 2);
                                    if (accNum == "小单")
                                    {
                                        if (Math.Abs(Cent + prices - Cent2) > xPricesc)
                                        {
                                            Response.Write("投注超额ok1");
                                            Response.End();
                                        }
                                    }
                                    if (accNum == "小双")
                                    {
                                        if (Math.Abs(Cent2 + prices - Cent) > xPricesc)
                                        {
                                            Response.Write("投注超额ok1");
                                            Response.End();
                                        }
                                    }
                                }
                            }
                        }
                        #endregion


                        int count1 = new BCW.ssc.BLL.SSCpay().GetRecordCount(" UsID=" + meid + " and SSCId='" + model.SSCId + "'");
                        //每个机器人每一期的最高购买次数
                        int buyCount = Convert.ToInt32(ub.GetSub("SSCRoBotbuyCount", xmlPath));
                        if (count1 < buyCount)
                        {
                            if (accNum != "" && accNum != "#" && accNum != ",")
                            {
                                if (gold < prices)
                                {
                                    //更新消费记录
                                    BCW.Model.Goldlog modelx = new BCW.Model.Goldlog();
                                    modelx.BbTag = 3;
                                    modelx.Types = 0;
                                    modelx.PUrl = Utils.getPageUrl();//操作的文件名
                                    modelx.UsId = meid;
                                    modelx.UsName = mename;

                                    modelx.AcGold = prices;
                                    modelx.AfterGold = gold + prices;//更新后的币数

                                    modelx.AcText = "系统机器人自动操作";
                                    modelx.AddTime = DateTime.Now;
                                    new BCW.BLL.Goldlog().Add(modelx);

                                    BCW.Data.SqlHelper.ExecuteSql("Update tb_User set iGold=iGold+(" + prices + ") where id=" + meid + "");

                                    BCW.ssc.Model.SSCpay modelpay = new BCW.ssc.Model.SSCpay();
                                    modelpay.SSCId = model.SSCId;
                                    modelpay.Types = ptype;
                                    modelpay.UsID = meid;
                                    modelpay.UsName = mename;
                                    modelpay.iCount = iZhu;
                                    modelpay.Price = Price;
                                    modelpay.State = 0;
                                    modelpay.Prices = prices;
                                    modelpay.WinCent = 0;
                                    modelpay.Result = "";
                                    modelpay.Notes = accNum;
                                    modelpay.Odds = Odds;
                                    modelpay.WinNotes = "";
                                    modelpay.AddTime = DateTime.Now;
                                    modelpay.IsSpier = 1;
                                    int id = new BCW.ssc.BLL.SSCpay().Add2(modelpay);

                                    new BCW.BLL.User().UpdateiGold(meid, mename, -prices, 11, "" + GameName + "第[url=./game/ssc.aspx?act=view&amp;id=" + model.ID + "]" + model.SSCId + "[/url]期[" + OutType(ptype) + "]位号:" + accNum + "|赔率" + modelpay.Odds + "|标识ID" + id + ""); //酷币

                                    //动态
                                    string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/ssc.aspx]" + GameName + "第" + model.SSCId + "期[/url]下注**" + "" + ub.Get("SiteBz") + "";//+ prices 

                                    new BCW.BLL.Action().Add(1019, id, meid, "", wText);

                                }
                                else
                                {
                                    BCW.ssc.Model.SSCpay modelpay = new BCW.ssc.Model.SSCpay();
                                    modelpay.SSCId = model.SSCId;
                                    modelpay.Types = ptype;
                                    modelpay.UsID = meid;
                                    modelpay.UsName = mename;
                                    modelpay.iCount = iZhu;
                                    modelpay.Price = Price;
                                    modelpay.State = 0;
                                    modelpay.Prices = prices;
                                    modelpay.WinCent = 0;
                                    modelpay.Result = "";
                                    modelpay.Notes = accNum;
                                    modelpay.Odds = Odds;
                                    modelpay.WinNotes = "";
                                    modelpay.AddTime = DateTime.Now;
                                    modelpay.IsSpier = 1;
                                    int id = new BCW.ssc.BLL.SSCpay().Add2(modelpay);

                                    new BCW.BLL.User().UpdateiGold(meid, mename, -prices, 11, "" + GameName + "第[url=./game/ssc.aspx?act=view&amp;id=" + model.ID + "]" + model.SSCId + "[/url]期[" + OutType(ptype) + "]位号:" + accNum + "|赔率" + modelpay.Odds + "|标识ID" + id + ""); //酷币

                                    //动态
                                    string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/ssc.aspx]" + GameName + "第" + model.SSCId + "期[/url]下注**" + "" + ub.Get("SiteBz") + "";//+ prices

                                    new BCW.BLL.Action().Add(1019, id, meid, "", wText);
                                }
                            }
                        }
                        //string RoBot = ub.GetSub("SSCRoBotID", xmlPath);
                        //string[] RoBotID = RoBot.Split('#');
                        //for (int i = 0; i < RoBotID.Length; i++)
                        //{
                        //    int count = new BCW.ssc.BLL.SSCpay().GetRecordCount(" UsID=" + RoBotID[i] + " and SSCId='" + model.SSCId + "'");
                        //    Response.Write("=第" + model.SSCId + "期=<br />");
                        //    Response.Write("机器人:" + RoBotID[i] + "买了" + count + "张<br />");
                        //}

                        int count = new BCW.ssc.BLL.SSCpay().GetRecordCount(" SSCId='" + model.SSCId + "'");
                        Response.Write("=第" + model.SSCId + "期=<br />");
                        Response.Write("机器人共" + "买了" + count + "张<br />");
                    }
                    else
                    {
                        Response.Write("当前时间不可下注ok1");
                    }
                }
            }
        }
    }

    //机器人自动兑奖
    private void Case()
    {
        #region 机器人自动兑奖
        //得到随机的UsID
        int meid = GetUsID();
        string RoBot = ub.GetSub("SSCRoBotID", xmlPath);
        string[] RoBotID = RoBot.Split('#');
        for (int i = 0; i < RoBotID.Length; i++)
        {
            meid = Convert.ToInt32(RoBotID[i]);
            string mename1 = new BCW.BLL.User().GetUsName(meid);
            DataSet model2 = new BCW.ssc.BLL.SSCpay().GetList("ID", "UsID=" + meid + " and IsSpier=1 and WinCent > 0 and State!=2 ");
            if (model2 != null && model2.Tables[0].Rows.Count > 0)
            {
                for (int i2 = 0; i2 < model2.Tables[0].Rows.Count; i2++)
                {
                    int pid = 0;
                    try
                    {
                        pid = Convert.ToInt32(model2.Tables[0].Rows[0][i2]);
                    }
                    catch { }
                    if (new BCW.ssc.BLL.SSCpay().ExistsReBot(pid, meid))
                    {
                        if (new BCW.ssc.BLL.SSCpay().GetState(pid) != 2)
                        {
                            //操作币
                            BCW.ssc.Model.SSCpay model1 = new BCW.ssc.BLL.SSCpay().GetSSCpay(pid);
                            int number = model1.SSCId;
                            BCW.ssc.Model.SSClist idd = new BCW.ssc.BLL.SSClist().GetSSClistbySSCId(number);
                            long winMoney = Convert.ToInt64(model1.WinCent);
                            new BCW.BLL.User().UpdateiGold(meid, mename1, winMoney, "" + GameName + "兑奖-" + "[url=./game/klsf.aspx?act=view&amp;id=" + idd.ID + "&amp;ptype=2]" + number + "[/url]" + "-标识ID：" + pid + "");
                            new BCW.ssc.BLL.SSCpay().UpdateState(pid, 2);
                            Response.Write("" + GameName + "机器人" + meid + "自动兑奖!ok1<br />");
                        }
                    }
                }
            }
        }
        #endregion
    }

    //随机得到下注的类型
    private int GetPtype()
    {
        Random rac = new Random(unchecked((int)DateTime.Now.Ticks));
        int ptype = rac.Next(1, 57);//18,23

        return ptype;
    }

    //得到出动的ID
    private int GetUsID()
    {
        int UsID = 0;
        string PlayUsID = ub.GetSub("SSCRoBotID", xmlPath);
        string[] sNum = Regex.Split(PlayUsID, "#");
        Random rd = new Random();
        try { UsID = Convert.ToInt32((sNum[rd.Next(sNum.Length)]).Replace(" ", "")); }
        catch { }

        return UsID;
    }

    //随机得到下注的币数(取整十整百整千整万等)
    private long GetPayCent()
    {
        Random rac = new Random(unchecked((int)DateTime.Now.Ticks));
        long paycent = 0;

        string Pay = ub.GetSub("SSCRoBotCent", xmlPath);
        string[] sNum = Regex.Split(Pay, "#");
        try
        {
            paycent = Convert.ToInt64(sNum[rac.Next(sNum.Length)]);
        }
        catch { }
        return paycent;
    }

    /// <summary>
    /// 更新会员在线
    /// </summary>
    private void ChanageOnline(int UsID)
    {
        int OnTime = 5;
        new BCW.BLL.User().UpdateTime(UsID, OnTime);
    }

    #region 下注类型 OutType
    /// <summary>
    /// 下注类型
    /// </summary>
    /// <param name="Types"></param>
    /// <returns></returns>
    private string OutType(int Types)
    {
        string ptypey = string.Empty;
        string payname1 = string.Empty;
        string odds1 = string.Empty;
        string oddsc1 = string.Empty;
        string rule1 = string.Empty;
        for (int i = 1; i < 57; i++)
        {
            ptypey = ub.GetSub("ptype" + i + "", xmlPath);
            string[] ptypef = ptypey.Split('#');
            payname1 += "#" + ptypef[0];
            odds1 += "#" + ptypef[1];
            oddsc1 += "#" + ptypef[2];
            rule1 += "#" + ptypef[3];
        }
        string[] payname2 = payname1.Split('#');
        string[] odds2 = odds1.Split('#');
        string[] oddsc2 = oddsc1.Split('#');
        string[] rule2 = rule1.Split('#');
        string pText = string.Empty;

        for (int i = 1; i < 57; i++)
        {
            if (Types == i)
                pText = payname2[i];
        }

        return pText;
    }
    #endregion

    #region 玩法提示 OutRule
    /// <summary>
    /// 玩法提示
    /// </summary>
    /// <param name="Types"></param>
    /// <returns></returns>
    private string OutRule(int Types)
    {
        string ptypey = string.Empty;
        string payname1 = string.Empty;
        string odds1 = string.Empty;
        string oddsc1 = string.Empty;
        string rule1 = string.Empty;
        for (int i = 1; i < 57; i++)
        {
            ptypey = ub.GetSub("ptype" + i + "", xmlPath);
            string[] ptypef = ptypey.Split('#');
            payname1 += "#" + ptypef[0];
            odds1 += "#" + ptypef[1];
            oddsc1 += "#" + ptypef[2];
            rule1 += "#" + ptypef[3];
        }
        string[] payname2 = payname1.Split('#');
        string[] odds2 = odds1.Split('#');
        string[] oddsc2 = oddsc1.Split('#');
        string[] rule2 = rule1.Split('#');
        string pText = string.Empty;

        for (int i = 1; i < 57; i++)
        {
            if (Types == i)
                pText = rule2[i];
        }

        return pText;
    }
    #endregion

    #region 赔率 OutOdds
    /// <summary>
    /// 赔率 如果赔率只有一个，n就是1位，赔率取第二位，n就是2，赔率n位，取N位
    /// </summary>
    /// <param name="Types"></param>
    /// <returns></returns>
    private string OutOdds(int Types, int n)
    {
        string ptypey = string.Empty;
        string payname1 = string.Empty;
        string odds1 = string.Empty;
        string oddsc1 = string.Empty;
        string rule1 = string.Empty;
        for (int i = 1; i < 57; i++)
        {
            ptypey = ub.GetSub("ptype" + i + "", xmlPath);
            string[] ptypef = ptypey.Split('#');
            payname1 += "#" + ptypef[0];
            odds1 += "#" + ptypef[1];
            oddsc1 += "#" + ptypef[2];
            rule1 += "#" + ptypef[3];
        }
        string[] payname2 = payname1.Split('#');
        string[] odds2 = odds1.Split('#');
        string[] oddsc2 = oddsc1.Split('#');
        string[] rule2 = rule1.Split('#');
        string pText = string.Empty;

        for (int i = 1; i < 57; i++)
        {
            string[] odds = odds2[i].Split('|');

            if (odds.Length == 1)
            {
                if (Types == i)
                    pText = odds2[i];
            }
            else
            {
                for (int m = 0; m < odds.Length; m++)
                {
                    if (Types == i && m == (n - 1))
                    {
                        pText = odds[m];
                    }
                }
            }
        }

        return pText;
    }
    #endregion

    #region 投注上限 OutOddsc
    /// <summary>
    /// 投注上限
    /// </summary>
    /// <param name="Types"></param>
    /// <returns></returns>
    private string OutOddsc(int Types)
    {
        string ptypey = string.Empty;
        string payname1 = string.Empty;
        string odds1 = string.Empty;
        string oddsc1 = string.Empty;
        string rule1 = string.Empty;
        for (int i = 1; i < 57; i++)
        {
            ptypey = ub.GetSub("ptype" + i + "", xmlPath);
            string[] ptypef = ptypey.Split('#');
            payname1 += "#" + ptypef[0];
            odds1 += "#" + ptypef[1];
            oddsc1 += "#" + ptypef[2];
            rule1 += "#" + ptypef[3];
        }
        string[] payname2 = payname1.Split('#');
        string[] odds2 = odds1.Split('#');
        string[] oddsc2 = oddsc1.Split('#');
        string[] rule2 = rule1.Split('#');
        string pText = string.Empty;

        for (int i = 1; i < 57; i++)
        {
            if (Types == i)
                pText = oddsc2[i];
        }

        return pText;
    }
    #endregion

    #region 投注上限 OutOddscTid
    /// <summary>
    /// 投注上限
    /// </summary>
    /// <param name="Types"></param>
    /// <returns></returns>
    private string OutOddscTid(int Types)
    {
        string ptypey = string.Empty;
        string oddsc1 = string.Empty;
        for (int i = 1; i < 57; i++)
        {
            ptypey = ub.GetSub("ptype" + i + "", xmlPath);
            string[] ptypef = ptypey.Split('#');
            string p8 = string.Empty;
            try { p8 = ptypef[8]; }
            catch { p8 = "0"; }
            oddsc1 += "#" + p8;

        }
        string[] oddsc2 = oddsc1.Split('#');
        string pText = string.Empty;

        for (int i = 1; i < 57; i++)
        {
            if (Types == i)
                pText = oddsc2[i];
        }

        return pText;
    }
    #endregion

    #region 投注上限 OutOddscid
    /// <summary>
    /// 投注上限
    /// </summary>
    /// <param name="Types"></param>
    /// <returns></returns>
    private string OutOddscid(int Types)
    {
        string ptypey = string.Empty;
        string oddsc1 = string.Empty;
        for (int i = 1; i < 57; i++)
        {
            ptypey = ub.GetSub("ptype" + i + "", xmlPath);
            string[] ptypef = ptypey.Split('#');
            oddsc1 += "#" + ptypef[4];

        }
        string[] oddsc2 = oddsc1.Split('#');
        string pText = string.Empty;

        for (int i = 1; i < 57; i++)
        {
            if (Types == i)
                pText = oddsc2[i];
        }

        return pText;
    }
    #endregion

    /// <summary>
    /// 将两个数字转化成三个数字的两组集合
    /// </summary>
    /// <param name="sNum"></param>
    /// <returns></returns>
    private string OutStrNum(string sNum)
    {
        string[] Temp = sNum.Split(',');
        string strNum1 = Temp[0] + "," + sNum;
        string strNum2 = sNum + "," + Temp[1];
        string strNum = strNum1 + "，" + strNum2;
        return strNum.Replace(",", "");
    }

    /// <summary>
    /// 随机得到位号
    /// </summary>
    /// <param name="max">最多组合多少个数字(随机)</param>
    /// <returns></returns>
    private string OutNum(int min, int max)
    {

        Random rd = new Random(unchecked((int)DateTime.Now.Ticks));
        int ird = rd.Next(min, max);
        string str = "";
        int[] a = new int[ird];
        int n = 0;
        int mid = 0;
        while (mid == 0)
        {
            int k = rd.Next(0, 10);
            if (!str.Contains(k.ToString()))
            {
                str += k;
                a[n] = k;
                n++;
            }
            else
                mid = 1;

            if (n >= ird)
                break;
            else
                mid = 0;

        }
        //从小到大排序(冒泡排序)
        string b = string.Empty;
        int temp;
        for (int i = 0; i < a.Length; i++)
        {
            for (int j = i; j < a.Length; j++)
            {
                if (a[i] > a[j])
                {
                    temp = a[j];
                    a[j] = a[i];
                    a[i] = temp;
                }
            }
        }
        string str2 = "";
        for (int i = 0; i < a.Length; i++)
        {
            str2 += "" + a[i].ToString();
        }
        str2 = Utils.Mid(str2, 1, str2.Length);

        //string[] aa = new string[a.Length];
        //for (int i = 0; i < a.Length; i++)
        //{
        //    aa[i] = a[i].ToString();
        //}
        return str2;
    }
    /// <summary>
    /// 随机产生n个结果（0-9）按小到大排
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    private string PlayNum(int min, int max)
    {
        Random rd = new Random(unchecked((int)DateTime.Now.Ticks));
        int n = rd.Next(min, (max + 1));
        string num = string.Empty;
        //随机产生n个不重复的0-9的数
        int[] goods = new int[10];
        for (int i = 0; i < 10; i++) goods[i] = i;
        for (int j = 9; j > 0; j--)
        {
            Random ra = new Random();
            int index = ra.Next(0, j);
            int temp = goods[index];
            goods[index] = goods[j];
            goods[j] = temp;
        }
        //冒泡排序 从大到小
        for (int i = 0; i < n; i++)
        {
            for (int j = i + 1; j < n; j++)
            {
                if (goods[j] < goods[i])
                {
                    int temp = goods[i];
                    goods[i] = goods[j];
                    goods[j] = temp;
                }
            }
        }
        string goodss = "";
        for (int i = 0; i < n; i++)//遍历数组显示结果
        {
            if (i != (n - 1))
                goodss += goods[i] + ",";
            else
                goodss += goods[i] + "";
        }
        num = goodss;
        return num;
    }

    #region 计算组合的数量
    static long jc(int N)//阶乘
    {
        long t = 1;

        for (int i = 1; i <= N; i++)
        {
            t *= i;
        }
        return t;
    }
    static long P(int N, int R)//组合的计算公式
    {
        long t = jc(N) / jc(N - R);

        return t;
    }
    static int C(int N, int R)//组合
    {
        long i = P(N, R) / jc(R);
        int t = Convert.ToInt32(i);
        return t;
    }
    #endregion
}
