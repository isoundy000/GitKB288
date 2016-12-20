using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using BCW.Common;
using System.Timers;

public partial class bbs_game_xinkuai3_getnum : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/xinkuai3.xml";
    protected string GameName = ub.GetSub("XinKuai3Name", "/Controls/xinkuai3.xml");//游戏名字
    string date_1 = ub.GetSub("Xdate", "/Controls/xinkuai3.xml");//系统回收时间
    protected string klb = ub.GetSub("klb", "/Controls/xinkuai3_TRIAL_GAME.xml");//快乐币
    protected string xmlPath_SWB = "/Controls/xinkuai3_TRIAL_GAME.xml";

    protected void Page_Load(object sender, EventArgs e)
    {
        string st1 = "09:20";
        string st2 = "22:45";
        DateTime dt1 = Convert.ToDateTime(st1);
        DateTime dt2 = Convert.ToDateTime(st2);
        DateTime dt3 = DateTime.Now;

        Response.Write("1、《新快3》系统会在 00:00 点自动回收超过《" + date_1 + "天》还没领奖的数据。今天是：" + DateTime.Now + "<br/><br/>");
        if (Utils.GetDomain().Contains("kb288"))
            Response.Write("2、《新快3_试玩版》系统会在 00:00 点自动回收超过《" + date_1 + "天》还没领奖的数据。今天是：" + DateTime.Now + "<br/><br/>");

        string _time = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
        int _time_num = Convert.ToInt32(_time);

        UpdateState();

        if ((DateTime.Compare(dt1, dt3) > 0) || ((DateTime.Compare(dt2, dt3) < 0)))
        {
            Response.Write("<br />&nbsp;&nbsp;&nbsp;&nbsp;<b>程序新快3_没到开奖时间，请等待（close1）。。。</b><br /><br />");
            Response.Write("<br />&nbsp;&nbsp;&nbsp;&nbsp;<b>开奖时间：09:26--22:26</b><br /><br />");
        }
        else
        {
            XinKuai3_num();//获取开奖数据
            Robot_case();//机器人兑奖
        }
        if (_time_num == 0000)
        {
            update_state();//若超过7天未兑奖，系统自动回收该数据所得的酷币。
            if (Utils.GetDomain().Contains("kb288"))
                update_state_SWB();
            Response.Write("<br />&nbsp;&nbsp;&nbsp;&nbsp;<b style=\"color:red\">《新快3》系统已自动回收《超7天未兑奖》的" + ub.Get("SiteBz") + "。。。</b><br /><br />");
            if (Utils.GetDomain().Contains("kb288"))
                Response.Write("<br />&nbsp;&nbsp;&nbsp;&nbsp;<b style=\"color:red\">《新快3_试玩版》系统已自动回收《超7天未兑奖》的" + ub.Get("SiteBz") + "。。。</b><br /><br />");
        }
    }
    //算法
    private int suan(int n, string[] a, int s, int count, int sp)
    {
        int cfnum = int.Parse(ub.GetSub("cfnum", xmlPath));
        if (cfnum == 2)//连开2期
        {
            #region
            if (count >= 5)
            {
                if (a[n] != "0")
                {
                    if (n + 1 < sp - 1)
                    {
                        if (a[n] == a[n + 1])
                        {
                            s++;
                            return suan(n + 1, a, s, count, sp);
                        }
                        else
                            return s;
                    }
                    else
                        return s;
                }
                else
                {
                    if (n + 1 < sp - 1)
                    {
                        if (a[n - 1] == a[n + 1])
                        {
                            s++;
                            return suan(n + 1, a, s, count, sp);
                        }
                        else
                        {
                            return suan(n + 1, a, s, count, sp);
                        }
                    }
                    else
                        return s;
                }
            }
            else
                return s;
            #endregion
        }
        else if (cfnum == 3)
        {
            #region
            if (count >= 7)
            {
                if (a[n] != "0")
                {
                    if (n + 2 < sp - 1)
                    {
                        if (a[n] == a[n + 1] && a[n] == a[n + 2])
                        {
                            s++;
                            return suan(n + 1, a, s, count, sp);
                        }
                        else
                            return s;
                    }
                    else
                        return s;
                }
                else
                {
                    if (n + 2 < sp - 1)
                    {
                        if (a[n - 1] == a[n + 1] && a[n - 1] == a[n + 2])
                        {
                            s++;
                            return suan(n + 1, a, s, count, sp);
                        }
                        else
                        {
                            return suan(n + 1, a, s, count, sp);
                        }
                    }
                    else
                        return s;
                }
            }
            else
                return s;
            #endregion
        }
        else if (cfnum == 4)
        {
            #region
            if (count >= 9)
            {
                if (a[n] != "0")
                {
                    if (n + 3 < sp - 1)
                    {
                        if (a[n] == a[n + 1] && a[n] == a[n + 2] && a[n] == a[n + 3])
                        {
                            s++;
                            return suan(n + 1, a, s, count, sp);
                        }
                        else
                            return s;
                    }
                    else
                        return s;
                }
                else
                {
                    if (n + 3 < sp - 1)
                    {
                        if (a[n - 1] == a[n + 1] && a[n - 1] == a[n + 2] && a[n - 1] == a[n + 3])
                        {
                            s++;
                            return suan(n + 1, a, s, count, sp);
                        }
                        else
                        {
                            return suan(n + 1, a, s, count, sp);
                        }
                    }
                    else
                        return s;
                }
            }
            else
                return s;
            #endregion
        }
        else if (cfnum == 5)
        {
            #region
            if (count >= 11)
            {
                if (a[n] != "0")
                {
                    if (n + 4 < sp - 1)
                    {
                        if (a[n] == a[n + 1] && a[n] == a[n + 2] && a[n] == a[n + 3] && a[n] == a[n + 4])
                        {
                            s++;
                            return suan(n + 1, a, s, count, sp);
                        }
                        else
                            return s;
                    }
                    else
                        return s;
                }
                else
                {
                    if (n + 4 < sp - 1)
                    {
                        if (a[n - 1] == a[n + 1] && a[n - 1] == a[n + 2] && a[n - 1] == a[n + 3] && a[n - 1] == a[n + 4])
                        {
                            s++;
                            return suan(n + 1, a, s, count, sp);
                        }
                        else
                        {
                            return suan(n + 1, a, s, count, sp);
                        }
                    }
                    else
                        return s;
                }
            }
            else
                return s;
            #endregion
        }
        else if (cfnum == 6)
        {
            #region
            if (count >= 13)
            {
                if (a[n] != "0")
                {
                    if (n + 5 < sp - 1)
                    {
                        if (a[n] == a[n + 1] && a[n] == a[n + 2] && a[n] == a[n + 3] && a[n] == a[n + 4] && a[n] == a[n + 5])
                        {
                            s++;
                            return suan(n + 1, a, s, count, sp);
                        }
                        else
                            return s;
                    }
                    else
                        return s;
                }
                else
                {
                    if (n + 5 < sp - 1)
                    {
                        if (a[n - 1] == a[n + 1] && a[n - 1] == a[n + 2] && a[n - 1] == a[n + 3] && a[n - 1] == a[n + 4] && a[n - 1] == a[n + 5])
                        {
                            s++;
                            return suan(n + 1, a, s, count, sp);
                        }
                        else
                        {
                            return suan(n + 1, a, s, count, sp);
                        }
                    }
                    else
                        return s;
                }
            }
            else
                return s;
            #endregion
        }
        else
            return s;

        #region 旧方法
        //if (n < count)
        //{
        //    if (a[n] != "0")
        //    {
        //        if (a[n] == a[n + 1])
        //        {
        //            s++;
        //            return suan(n + 1, a, s, count);
        //        }
        //        else
        //        {
        //            return s;
        //        }
        //    }
        //    else
        //    {
        //        if (a[n - 1] == a[n + 1])
        //        {
        //            s++;
        //            return suan(n + 1, a, s, count);
        //        }
        //        else
        //        {
        //            return suan(n + 1, a, s, count);
        //        }
        //    }
        //}
        //else
        //    return s;
        #endregion

    }

    //获得网页数据——3G门户
    public string XK3IdPage()
    {
        //得到内容
        string str = GetPageUtf8Html("http://gc.3g.cn/Game/GuangxiQuick/GuangxiIndex.aspx", @"<p>", @"</p>", "UTF-8");
        return str;
    }

    //获得网页数据——开彩网
    public string XK3IdPage2()
    {
        //得到内容
        string str = GetPageUtf8Html("http://c.apiplus.net/newly.do?token=5a8ebe52461354bc&code=gxk3&rows=1", @"<row", @"opentime", "UTF-8");
        return str;
    }

    //正则取局部数据
    private string GetPageUtf8Html(string p_url, string Start, string Over, string Encoding)
    {
        string p_html = GetSourceTextByUrl(p_url, Encoding);

        if (string.IsNullOrEmpty(p_html))
            return "";

        string pattern = "" + Start + "([\\s\\S]+?)" + Over + "";
        Match m1 = Regex.Match(p_html, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        if (m1.Success)
        {
            string str = m1.Groups[1].Value;

            return str;
        }
        else
            return "";
    }

    //抓取一个网页源码
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

    // 更新期数
    public string UpdateState()
    {
        string OnTime = ub.GetSub("xOnTime", xmlPath);//09:26-22:26
        if (OnTime != "")
        {
            if (Utils.IsRegex(OnTime, @"^[0-9]{2}:[0-9]{2}-[0-9]{2}:[0-9]{2}$"))
            {
                string[] temp = OnTime.Split("-".ToCharArray());
                DateTime dt1 = Convert.ToDateTime(temp[0]);
                DateTime dt2 = Convert.ToDateTime(temp[1]);
                string state = string.Empty;
                if (DateTime.Now <= dt2 && DateTime.Now >= dt1)
                {
                    string dt3 = DateTime.Now.AddMinutes(5).Subtract(dt1).Duration().TotalMinutes.ToString();

                    decimal dt4 = Convert.ToDecimal(dt3);
                    int dt5 = Convert.ToInt32(dt4 / 10);
                    string dt6 = dt5.ToString();
                    if (dt6.Length == 1)
                    {
                        dt6 = "00" + dt6;
                    }
                    if (dt6.Length == 2)
                    {
                        dt6 = "0" + dt6;
                    }
                    state = DateTime.Now.ToString("yyyyMMdd").Substring(2, 6) + dt6;
                    BCW.XinKuai3.Model.XK3_Internet_Data model = new BCW.XinKuai3.Model.XK3_Internet_Data();
                    model.Lottery_issue = state;
                    model.Lottery_num = "";
                    string datee = DateTime.ParseExact((("20" + state.Substring(0, 6)) + " 09:26:00"), "yyyyMMdd HH:mm:ss", null).ToString("yyyy-MM-dd HH:mm:ss");
                    //根据期号算时间,
                    if (int.Parse(GetLastStr(state, 2)) < 10)//如果少于10
                    {
                        model.Lottery_time = Convert.ToDateTime(datee).AddMinutes(10 * (int.Parse(GetLastStr(state, 1))));
                    }
                    else
                    {
                        model.Lottery_time = Convert.ToDateTime(datee).AddMinutes(10 * (int.Parse(GetLastStr(state, 2))));
                    }
                    model.UpdateTime = DateTime.Now;

                    bool s = new BCW.XinKuai3.BLL.XK3_Internet_Data().Exists_num(state);
                    switch (s)
                    {
                        case false:
                            new BCW.XinKuai3.BLL.XK3_Internet_Data().Add_num(model);
                            Response.Write("3、新快3第" + state + "期期号已存入.<br /><br />");
                            break;
                        case true:
                            Response.Write("4、新快3第" + state + "期期号已存在.<br /><br />");
                            break;
                    }
                    return state;
                }
            }
        }
        return "0";
    }

    //更新数据库中开奖结果
    public void XinKuai3_num()
    {
        Match qihao;
        Match nums;

        string something = XK3IdPage2();
        //开奖期号
        Match qihao1 = Regex.Match(something, @"expect=[\D][\d]{11}[\D]");
        Match qihao2 = Regex.Match(qihao1.Value, @"[\d]{11}");
        qihao = Regex.Match(qihao2.Value, @"\d.{8}$");
        //开奖号码
        Match num1 = Regex.Match(something, @"opencode=[\D][1-6],[1-6],[1-6][\D]");
        nums = Regex.Match(num1.Value, @"[1-6],[1-6],[1-6]");

        if (qihao.Value == "")
        {
            string something2 = XK3IdPage();
            Match num = Regex.Match(something2, @"广西快3[\d]{9}期开奖号码:<br/>[1-6],[1-6],[1-6]<br/>");
            Match stage = Regex.Match(num.Value, @"[\d]{9}期");
            qihao = Regex.Match(stage.Value, @"[\d]{9}");
            nums = Regex.Match(num.Value, @"[1-6],[1-6],[1-6]");
        }

        if (qihao.Value != "")
        {
            #region  如果获取的期号不为空
            BCW.XinKuai3.Model.XK3_Internet_Data model = new BCW.XinKuai3.Model.XK3_Internet_Data();
            model.Lottery_issue = qihao.Value;//抓取的开奖期号
            model.Lottery_num = nums.Value;
            model.Lottery_time = DateTime.Now;
            model.UpdateTime = DateTime.Now;

            //判断该开奖号码是否存在
            bool s = new BCW.XinKuai3.BLL.XK3_Internet_Data().Exists_num(qihao.Value);
            switch (s)
            {
                case false:
                    #region 如果该开奖期号不存在
                    new BCW.XinKuai3.BLL.XK3_Internet_Data().Add_num(model);//存期号、开奖号码、时间
                    Response.Write("<br />&nbsp;&nbsp;&nbsp;<b>《新快3》_第" + qihao.Value + "期的《期号》已单独存入ok1</b><br/><br/>");
                    //若只有期号，无开奖号码，则不返回各中奖情况。
                    if (nums.Value != "")
                    {
                        //返回各自中奖情况
                        string a = nums.Value;
                        if (a != "")
                        {
                            string[] str1 = a.Split(',');
                            string t1 = str1[0];
                            string t2 = str1[1];
                            string t3 = str1[2];

                            Response.Write("<br />&nbsp;&nbsp;&nbsp;<b>《新快3》_第" + t1 + t2 + t3 + "期的《开奖数据》已存入</b><br/><br/>");

                            //大小单双
                            //1大2小、1单2双
                            if (((t1 == t2) && (t1 == t3)))//大小双单通食
                            {
                                model.DaXiao = "0";
                                model.DanShuang = "0";
                            }
                            else
                            {
                                if ((Int32.Parse(str1[0])) + (Int32.Parse(str1[1])) + (Int32.Parse(str1[2])) <= 10)
                                {
                                    model.DaXiao = "2";//和值开出是4-10,即为小

                                }
                                else
                                {
                                    model.DaXiao = "1";//和值开出是11-17,即为大

                                }
                                if (((Int32.Parse(str1[0])) + (Int32.Parse(str1[1])) + (Int32.Parse(str1[2]))) % 2 == 1)
                                {
                                    model.DanShuang = "1";//单数
                                }
                                else if (((Int32.Parse(str1[0])) + (Int32.Parse(str1[1])) + (Int32.Parse(str1[2]))) % 2 == 0)
                                {
                                    model.DanShuang = "2";//双数
                                }
                            }

                            //和值
                            string sum1 = (Int32.Parse(str1[0]) + Int32.Parse(str1[1]) + Int32.Parse(str1[2])).ToString();
                            model.Sum = sum1;
                            //三同号通选+三同号单选
                            if (((t1 == t2) && (t1 == t3)))
                            {
                                model.Three_Same_All = "1";
                                model.Three_Same_Single = t1 + t2 + t3;
                                //model.Three_Continue_All = "1";
                            }
                            else
                            {
                                model.Three_Same_All = "0";
                                model.Three_Same_Single = "0";
                                //model.Three_Continue_All = "0";
                            }
                            //三不同号
                            if ((t1 != t2) && (t1 != t3) && (t2 != t3))
                            {
                                model.Three_Same_Not = t1 + t2 + t3;
                            }
                            else
                            {
                                model.Three_Same_Not = "0";
                            }
                            //三连号通选
                            //if ((Int32.Parse(str1[1]) == (Int32.Parse(str1[0]) + 1)) && (Int32.Parse(str1[2]) == (Int32.Parse(str1[1]) + 1)))
                            if ((Int32.Parse(str1[1]) == (Int32.Parse(str1[0]) + 1)) && (Int32.Parse(str1[1]) == (Int32.Parse(str1[2]) - 1)))
                            {
                                //model.Three_Continue_All = t1 + t2 + t3;
                                model.Three_Continue_All = "1";
                            }
                            else
                            {
                                model.Three_Continue_All = "0";
                            }
                            //二同号复选
                            if ((t1 == t2) || (t2 == t3))
                            {
                                model.Two_Same_All = t2 + t2;

                            }
                            else
                            {
                                model.Two_Same_All = "0";

                            }
                            //二同号单选
                            if ((t1 == t2) || (t2 == t3))
                            {
                                model.Two_Same_Single = t1 + t2 + t3;
                            }
                            else if ((t1 == t2) && (t1 == t3))
                            {
                                model.Two_Same_Single = "0";
                            }
                            else
                            {
                                model.Two_Same_Single = "0";
                            }
                            //二不同号
                            if ((t1 != t2) && (t2 != t3))
                            {
                                model.Two_dissame = (t1 + t2) + "," + (t1 + t3) + "," + (t2 + t3);
                            }
                            else if ((t1 == t2) && (t2 != t3))
                            {
                                model.Two_dissame = t2 + t3;
                            }
                            else if ((t1 != t2) && (t2 == t3))
                            {
                                model.Two_dissame = t1 + t2;
                            }
                            else
                            {
                                model.Two_dissame = "0";
                            }
                            model.UpdateTime = DateTime.Now;
                            new BCW.XinKuai3.BLL.XK3_Internet_Data().Update_num(model);
                            //change_peilv();//根据最近几期的大小单双开奖情况，实时变动赔率。
                            change_peilv_new();
                            //if (Utils.GetDomain().Contains("kb288"))
                            //    change_peilv_SWB();
                        }
                    }
                    #endregion
                    break;
                case true:
                    //查询获取的期号是否存在开奖号码
                    string uu = "where Lottery_issue=" + qihao.Value + "";
                    BCW.XinKuai3.Model.XK3_Internet_Data model_select1 = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3listLast(uu);
                    string kainum_1 = model_select1.Lottery_num;//开奖号码

                    if (kainum_1 != "")
                    {
                        Open_price();
                        if (Utils.GetDomain().Contains("kb288"))
                            Open_price_SWB();
                        Response.Write("<br />&nbsp;&nbsp;&nbsp;<b>《新快3》_第***&nbsp;" + qihao.Value + "&nbsp;***期存在开奖期号《" + kainum_1 + "》，正在返彩，等待下一期开奖ok1......</b><br/><br/>");
                    }
                    else
                    {
                        if (nums.Value != "")//获取的开奖号码
                        {
                            Response.Write("<br />&nbsp;&nbsp;&nbsp;<b>《新快3》_第***&nbsp;" + qihao.Value + "&nbsp;***期对开奖号码已补录成功ok1。开奖：" + nums.Value + "</b><br/><br/>");
                            new BCW.XinKuai3.BLL.XK3_Internet_Data().update_num2(model);//根据开奖期号写入开奖号码

                            //返回各自中奖情况
                            string a = nums.Value;
                            if (a != "")
                            {
                                string[] str1 = a.Split(',');
                                string t1 = str1[0];
                                string t2 = str1[1];
                                string t3 = str1[2];
                                //大小单双
                                //1大2小、1单2双
                                if (((t1 == t2) && (t1 == t3)))//大小双单通食
                                {
                                    model.DaXiao = "0";
                                    model.DanShuang = "0";
                                }
                                else
                                {
                                    if ((Int32.Parse(str1[0])) + (Int32.Parse(str1[1])) + (Int32.Parse(str1[2])) <= 10)
                                    {
                                        model.DaXiao = "2";//和值开出是4-10,即为小

                                    }
                                    else
                                    {
                                        model.DaXiao = "1";//和值开出是11-17,即为大

                                    }
                                    if (((Int32.Parse(str1[0])) + (Int32.Parse(str1[1])) + (Int32.Parse(str1[2]))) % 2 == 1)
                                    {
                                        model.DanShuang = "1";//单数
                                    }
                                    else if (((Int32.Parse(str1[0])) + (Int32.Parse(str1[1])) + (Int32.Parse(str1[2]))) % 2 == 0)
                                    {
                                        model.DanShuang = "2";//双数
                                    }
                                }

                                //和值
                                string sum1 = (Int32.Parse(str1[0]) + Int32.Parse(str1[1]) + Int32.Parse(str1[2])).ToString();
                                model.Sum = sum1;
                                //三同号通选+三同号单选
                                if (((t1 == t2) && (t1 == t3)))
                                {
                                    model.Three_Same_All = "1";
                                    model.Three_Same_Single = t1 + t2 + t3;
                                    //model.Three_Continue_All = "1";
                                }
                                else
                                {
                                    model.Three_Same_All = "0";
                                    model.Three_Same_Single = "0";
                                    //model.Three_Continue_All = "0";
                                }
                                //三不同号
                                if ((t1 != t2) && (t1 != t3) && (t2 != t3))
                                {
                                    model.Three_Same_Not = t1 + t2 + t3;
                                }
                                else
                                {
                                    model.Three_Same_Not = "0";
                                }
                                //三连号通选
                                if ((Int32.Parse(str1[1]) == (Int32.Parse(str1[0]) + 1)) && (Int32.Parse(str1[1]) == (Int32.Parse(str1[2]) - 1)))
                                {
                                    //model.Three_Continue_All = t1 + t2 + t3;
                                    model.Three_Continue_All = "1";
                                }
                                else
                                {
                                    model.Three_Continue_All = "0";
                                }
                                //二同号复选
                                if ((t1 == t2) || (t2 == t3))
                                {
                                    model.Two_Same_All = t2 + t2;

                                }
                                else
                                {
                                    model.Two_Same_All = "0";

                                }
                                //二同号单选
                                if ((t1 == t2) || (t2 == t3))
                                {
                                    model.Two_Same_Single = t1 + t2 + t3;
                                }
                                else if ((t1 == t2) && (t1 == t3))
                                {
                                    model.Two_Same_Single = "0";
                                }
                                else
                                {
                                    model.Two_Same_Single = "0";
                                }
                                //二不同号
                                if ((t1 != t2) && (t2 != t3))
                                {
                                    model.Two_dissame = (t1 + t2) + "," + (t1 + t3) + "," + (t2 + t3);
                                }
                                else if ((t1 == t2) && (t2 != t3))
                                {
                                    model.Two_dissame = t2 + t3;
                                }
                                else if ((t1 != t2) && (t2 == t3))
                                {
                                    model.Two_dissame = t1 + t2;
                                }
                                else
                                {
                                    model.Two_dissame = "0";
                                }
                                model.UpdateTime = DateTime.Now;
                                new BCW.XinKuai3.BLL.XK3_Internet_Data().Update_num(model);
                                //change_peilv();//根据最近几期的大小单双开奖情况，实时变动赔率。
                                change_peilv_new();
                                //if (Utils.GetDomain().Contains("kb288"))
                                //    change_peilv_SWB();
                            }
                        }
                    }
                    break;
            }
            #endregion
        }
        else
        {
            #region 如果获取的期号为空，就自动加下一期的期号
            string _where = string.Empty;
            _where = "ORDER BY ID DESC";
            BCW.XinKuai3.Model.XK3_Internet_Data model_top = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3listLast(_where);
            string qihao_top = model_top.Lottery_issue;//最近一期开奖期号
            DateTime time_top = model_top.Lottery_time;//最近一期开奖时间
            string issue2 = (Int64.Parse(qihao_top) + 1).ToString();//下一个开奖期号
            string issue3 = Utils.Right(model_top.Lottery_issue.ToString(), 3);//最近一期开奖期号的后3位
            BCW.XinKuai3.Model.XK3_Internet_Data model_add = new BCW.XinKuai3.Model.XK3_Internet_Data();
            Response.Write("<br />&nbsp;&nbsp;&nbsp;<b>《新快3》_获取网络开奖期号信息失败..error1.第" + qihao_top + "期...请管理员检查问题。错误004</b><br/><br/>");

            if ((time_top.AddSeconds(605) > DateTime.Now) && (DateTime.Now > time_top.AddMinutes(10)))
            {
                if (issue3 != "078")
                {
                    model_add.Lottery_issue = issue2;
                    model_add.Lottery_time = time_top.AddMinutes(10);
                    model_add.Lottery_num = "";
                    model_add.UpdateTime = time_top.AddMinutes(10);
                    new BCW.XinKuai3.BLL.XK3_Internet_Data().Add_num(model_add);
                }
                Response.Write("<br />&nbsp;&nbsp;&nbsp;<b>《新快3》_获取开奖信息失败....error1..请管理员检查问题。错误005</b><br/><br/>");
            }
            if ((DateTime.Now > time_top.AddSeconds(40208)))
            {
                int dnu = 0;
                string dsb = DateTime.Now.ToString("yyMMdd");
                dnu = int.Parse(dsb + "001");
                model_add.Lottery_issue = dnu.ToString();
                model_add.Lottery_time = time_top.AddMinutes(670);
                model_add.Lottery_num = "";
                model_add.UpdateTime = time_top.AddMinutes(670);
                new BCW.XinKuai3.BLL.XK3_Internet_Data().Add_num(model_add);
            }
            #endregion
        }
    }

    //开始返彩====================正式
    public void Open_price()
    {
        //检查数据库最后一期开奖号码不为空的
        BCW.XinKuai3.Model.XK3_Internet_Data model_1 = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3listLast2();//网络开奖数据
        string qihao = model_1.Lottery_issue;
        string kai = model_1.Lottery_num;
        Response.Write("===============《新快3》_数据库最后一期的开奖数据===============" + qihao + "：" + kai + "<br/>");
        if (kai != "")//如果开奖号码为空，则不返奖
        {
            int sum = Int32.Parse(model_1.Sum);
            int Odds = 0;//和值的倍数

            //检查投注表是否存在没开奖数据
            if (new BCW.XinKuai3.BLL.XK3_Bet().Exists_num(qihao))
            {
                try
                {
                    DataSet ds = new BCW.XinKuai3.BLL.XK3_Bet().GetList("*", "State=0 and Lottery_issue=" + qihao + "");
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            //本地投注数据
                            int ID = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                            int UsID = int.Parse(ds.Tables[0].Rows[i]["UsID"].ToString());
                            int Play_Way = int.Parse(ds.Tables[0].Rows[i]["Play_Way"].ToString());
                            string Sum = ds.Tables[0].Rows[i]["Sum"].ToString();
                            string Three_Same_All = ds.Tables[0].Rows[i]["Three_Same_All"].ToString();
                            string Three_Same_Single = ds.Tables[0].Rows[i]["Three_Same_Single"].ToString();
                            string Three_Same_Not = ds.Tables[0].Rows[i]["Three_Same_Not"].ToString();
                            string Three_Continue_All = ds.Tables[0].Rows[i]["Three_Continue_All"].ToString();
                            string Two_Same_All = ds.Tables[0].Rows[i]["Two_Same_All"].ToString();
                            string Two_Same_Single = ds.Tables[0].Rows[i]["Two_Same_Single"].ToString();
                            string Two_dissame = ds.Tables[0].Rows[i]["Two_dissame"].ToString();
                            string Lottery_issue = ds.Tables[0].Rows[i]["Lottery_issue"].ToString();
                            string DanTuo = ds.Tables[0].Rows[i]["DanTuo"].ToString();
                            int Zhu = int.Parse(ds.Tables[0].Rows[i]["Zhu"].ToString());
                            int State = int.Parse(ds.Tables[0].Rows[i]["State"].ToString());
                            long Zhu_money = Int64.Parse(ds.Tables[0].Rows[i]["Zhu_money"].ToString());
                            long PutGold = Int64.Parse(ds.Tables[0].Rows[i]["PutGold"].ToString());
                            long GetMoney = Int64.Parse(ds.Tables[0].Rows[i]["GetMoney"].ToString());
                            string DaXiao = (ds.Tables[0].Rows[i]["DaXiao"].ToString());
                            string DanShuang = (ds.Tables[0].Rows[i]["DanShuang"].ToString());
                            float _odds = float.Parse(ds.Tables[0].Rows[i]["Odds"].ToString());
                            string mename = new BCW.BLL.User().GetUsName(UsID);//获得id对应的用户名
                            int isRobot = int.Parse(ds.Tables[0].Rows[i]["isRobot"].ToString());//机器人

                            new BCW.XinKuai3.BLL.XK3_Bet().UpdateState(ID, 1);

                            if (Play_Way == 1)//和值
                            {
                                #region 和值
                                if ((sum == 4) || (sum == 17))
                                {
                                    Odds = Utils.ParseInt(ub.GetSub("XinKuai3Sum2", xmlPath));
                                }
                                else if ((sum == 5) || (sum == 16))
                                {
                                    Odds = Utils.ParseInt(ub.GetSub("XSum1", xmlPath));
                                }
                                else if ((sum == 6) || (sum == 15))
                                {
                                    Odds = Utils.ParseInt(ub.GetSub("XSum2", xmlPath));
                                }
                                else if ((sum == 7) || (sum == 14))
                                {
                                    Odds = Utils.ParseInt(ub.GetSub("XSum3", xmlPath));
                                }
                                else if ((sum == 8) || (sum == 13))
                                {
                                    Odds = Utils.ParseInt(ub.GetSub("XSum4", xmlPath));
                                }
                                else if ((sum == 9) || (sum == 12))
                                {
                                    Odds = Utils.ParseInt(ub.GetSub("XSum5", xmlPath));
                                }
                                else if ((sum == 10) || (sum == 11))
                                {
                                    Odds = Utils.ParseInt(ub.GetSub("XinKuai3Sum1", xmlPath));
                                }
                                int a = 0;
                                string[] bd_sum = Sum.Split(',');
                                for (int bd = 0; bd < bd_sum.Length; bd++)
                                {
                                    if (bd_sum[bd] == model_1.Sum)
                                    {
                                        a++;
                                    }
                                }
                                if (a == 1)
                                //if (Sum.Contains(model_1.Sum))
                                {
                                    long WinCent = Convert.ToInt64(Zhu_money * Odds);
                                    if (WinCent > 0)
                                    {
                                        new BCW.XinKuai3.BLL.XK3_Bet().Update_win(ID, WinCent);
                                        if (isRobot == 0)
                                        {
                                            new BCW.BLL.Guest().Add(1, UsID, mename, "恭喜您在" + GameName + "第" + Lottery_issue + "期获得了" + WinCent + "" + ub.Get("SiteBz") + "[url=/bbs/game/xk3.aspx?act=case]马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                                        }
                                        string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + mename + "[/url]在[url=/bbs/game/xk3.aspx]" + GameName + "[/url]《" + Lottery_issue + "期》获得了**" + ub.Get("SiteBz") + "";//" + WinCent + "
                                        new BCW.BLL.Action().Add(1001, ID, UsID, "", wText);

                                        ////开奖时，把投注的钱添加到排行榜
                                        //if (!(new BCW.XinKuai3.BLL.XK3_Toplist().Exists_usid(UsID)))
                                        //{
                                        //    BCW.XinKuai3.Model.XK3_Toplist model_22 = new BCW.XinKuai3.Model.XK3_Toplist();
                                        //    model_22.UsId = UsID;
                                        //    model_22.UsName = mename;
                                        //    model_22.WinGold = 0;
                                        //    model_22.PutGold = PutGold;
                                        //    new BCW.XinKuai3.BLL.XK3_Toplist().Add(model_22);
                                        //}
                                        //else
                                        //{
                                        //    BCW.XinKuai3.Model.XK3_Toplist model_11 = new BCW.XinKuai3.BLL.XK3_Toplist().GetXK3_meid(UsID);
                                        //    long all_prices1 = model_11.PutGold + PutGold;
                                        //    new BCW.XinKuai3.BLL.XK3_Toplist().Update_gold(UsID, all_prices1);
                                        //}
                                        ////开奖时，把赢的钱存到排行榜
                                        //BCW.XinKuai3.Model.XK3_Toplist model_2 = new BCW.XinKuai3.BLL.XK3_Toplist().GetXK3_meid(UsID);
                                        //long all_prices = model_2.WinGold + WinCent;
                                        //new BCW.XinKuai3.BLL.XK3_Toplist().Update_getgold(UsID, all_prices);//兑奖的时候，把赢的钱，存进该meid对应的WinGold字段。
                                    }
                                }
                                #endregion
                            }
                            if (Play_Way == 2)//三同号通选
                            {
                                #region
                                if (model_1.Three_Same_All == "1")
                                {
                                    Odds = Utils.ParseInt(ub.GetSub("XinKuai3Three_Same_All", xmlPath));
                                    long WinCent = Convert.ToInt64(Zhu_money * Odds);
                                    if (WinCent > 0)
                                    {
                                        new BCW.XinKuai3.BLL.XK3_Bet().Update_win(ID, WinCent);
                                        if (isRobot == 0)
                                            new BCW.BLL.Guest().Add(1, UsID, mename, "恭喜您在" + GameName + "第" + Lottery_issue + "期获得了" + WinCent + "" + ub.Get("SiteBz") + "[url=/bbs/game/xk3.aspx?act=case]马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                                        string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + mename + "[/url]在[url=/bbs/game/xk3.aspx]" + GameName + "[/url]《" + Lottery_issue + "期》获得了**" + ub.Get("SiteBz") + "";
                                        new BCW.BLL.Action().Add(1001, ID, UsID, "", wText);
                                    }
                                }
                                #endregion
                            }
                            if (Play_Way == 3)//三同号单选
                            {
                                #region
                                if (Three_Same_Single.Contains(model_1.Three_Same_Single))
                                {
                                    Odds = Utils.ParseInt(ub.GetSub("XinKuai3Three_Same_Single", xmlPath));
                                    long WinCent = Convert.ToInt64(Zhu_money * Odds);
                                    if (WinCent > 0)
                                    {
                                        new BCW.XinKuai3.BLL.XK3_Bet().Update_win(ID, WinCent);
                                        if (isRobot == 0)
                                            new BCW.BLL.Guest().Add(1, UsID, mename, "恭喜您在" + GameName + "第" + Lottery_issue + "期获得了" + WinCent + "" + ub.Get("SiteBz") + "[url=/bbs/game/xk3.aspx?act=case]马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                                        string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + mename + "[/url]在[url=/bbs/game/xk3.aspx]" + GameName + "[/url]《" + Lottery_issue + "期》获得了**" + ub.Get("SiteBz") + "";
                                        new BCW.BLL.Action().Add(1001, ID, UsID, "", wText);
                                    }
                                }
                                #endregion
                            }
                            if (Play_Way == 4)//三不同号
                            {
                                #region
                                if (Three_Same_Not.Contains(model_1.Three_Same_Not))
                                {
                                    Odds = Utils.ParseInt(ub.GetSub("XinKuai3Three_Same_Not", xmlPath));
                                    long WinCent = Convert.ToInt64(Zhu_money * Odds);
                                    if (WinCent > 0)
                                    {
                                        new BCW.XinKuai3.BLL.XK3_Bet().Update_win(ID, WinCent);
                                        if (isRobot == 0)
                                            new BCW.BLL.Guest().Add(1, UsID, mename, "恭喜您在" + GameName + "第" + Lottery_issue + "期获得了" + WinCent + "" + ub.Get("SiteBz") + "[url=/bbs/game/xk3.aspx?act=case]马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                                        string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + mename + "[/url]在[url=/bbs/game/xk3.aspx]" + GameName + "[/url]《" + Lottery_issue + "期》获得了**" + ub.Get("SiteBz") + "";
                                        new BCW.BLL.Action().Add(1001, ID, UsID, "", wText);
                                    }
                                }
                                #endregion
                            }
                            if (Play_Way == 5)//三连号通选
                            {
                                #region
                                if (model_1.Three_Continue_All == "1")
                                {
                                    Odds = Utils.ParseInt(ub.GetSub("XinKuai3Three_Continue_All", xmlPath));
                                    long WinCent = Convert.ToInt64(Zhu_money * Odds);
                                    if (WinCent > 0)
                                    {
                                        new BCW.XinKuai3.BLL.XK3_Bet().Update_win(ID, WinCent);
                                        if (isRobot == 0)
                                            new BCW.BLL.Guest().Add(1, UsID, mename, "恭喜您在" + GameName + "第" + Lottery_issue + "期获得了" + WinCent + "" + ub.Get("SiteBz") + "[url=/bbs/game/xk3.aspx?act=case]马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                                        string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + mename + "[/url]在[url=/bbs/game/xk3.aspx]" + GameName + "[/url]《" + Lottery_issue + "期》获得了**" + ub.Get("SiteBz") + "";
                                        new BCW.BLL.Action().Add(1001, ID, UsID, "", wText);
                                    }
                                }
                                #endregion
                            }
                            if (Play_Way == 6)//二同号复选
                            {
                                #region
                                if (Two_Same_All.Contains(model_1.Two_Same_All))
                                {
                                    Odds = Utils.ParseInt(ub.GetSub("XinKuai3Two_Same_All", xmlPath));
                                    long WinCent = Convert.ToInt64(Zhu_money * Odds);
                                    if (WinCent > 0)
                                    {
                                        new BCW.XinKuai3.BLL.XK3_Bet().Update_win(ID, WinCent);
                                        if (isRobot == 0)
                                            new BCW.BLL.Guest().Add(1, UsID, mename, "恭喜您在" + GameName + "第" + Lottery_issue + "期获得了" + WinCent + "" + ub.Get("SiteBz") + "[url=/bbs/game/xk3.aspx?act=case]马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                                        string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + mename + "[/url]在[url=/bbs/game/xk3.aspx]" + GameName + "[/url]《" + Lottery_issue + "期》获得了**" + ub.Get("SiteBz") + "";
                                        new BCW.BLL.Action().Add(1001, ID, UsID, "", wText);
                                    }
                                }
                                #endregion
                            }
                            if (Play_Way == 7)//二同号单选
                            {
                                #region
                                string[] a1 = Two_Same_Single.Split(',');//分割购买的数据 {221,223,224,551,553,554}
                                int e = 0;
                                for (int p = 0; p < a1.Length; p++)
                                {
                                    //各自赋值给y
                                    int y = Convert.ToInt32(a1[p]);
                                    int y1 = y / 100;
                                    int y2 = (y - y1 * 100) / 10;
                                    int y3 = (y - y1 * 100 - y2 * 10);
                                    int[] num3 = { y1, y2, y3 };

                                    //冒泡排序 从小到大
                                    for (int t = 0; t < 3; t++)
                                    {
                                        for (int j = t + 1; j < 3; j++)
                                        {
                                            if (num3[j] < num3[t])
                                            {
                                                int temp = num3[t];
                                                num3[t] = num3[j];
                                                num3[j] = temp;
                                            }
                                        }
                                    }
                                    string num22 = string.Empty;
                                    for (int w = 0; w < 3; w++)//遍历数组显示结果
                                    {
                                        num22 = num22 + num3[w];
                                    }
                                    if (num22.Contains(model_1.Two_Same_Single))
                                    {
                                        e++;
                                    }
                                    if (e > 0)
                                    {
                                        break;
                                    }
                                }
                                //if (Two_Same_Single.Contains(model_1.Two_Same_Single))
                                if (e > 0)
                                {
                                    Odds = Utils.ParseInt(ub.GetSub("XinKuai3Two_Same_Single", xmlPath));
                                    long WinCent = Convert.ToInt64(Zhu_money * Odds);
                                    if (WinCent > 0)
                                    {
                                        new BCW.XinKuai3.BLL.XK3_Bet().Update_win(ID, WinCent);
                                        if (isRobot == 0)
                                            new BCW.BLL.Guest().Add(1, UsID, mename, "恭喜您在" + GameName + "第" + Lottery_issue + "期获得了" + WinCent + "" + ub.Get("SiteBz") + "[url=/bbs/game/xk3.aspx?act=case]马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                                        string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + mename + "[/url]在[url=/bbs/game/xk3.aspx]" + GameName + "[/url]《" + Lottery_issue + "期》获得了**" + ub.Get("SiteBz") + "";
                                        new BCW.BLL.Action().Add(1001, ID, UsID, "", wText);
                                    }
                                }
                                #endregion
                            }
                            if (Play_Way == 8)//二不同号
                            {
                                #region
                                string[] tt = (model_1.Two_dissame).Split(',');//开奖的数据
                                string[] bb = Two_dissame.Split(',');//投注的数据
                                int a = tt.Length;
                                int b = bb.Length;
                                int j = 0;

                                for (int y = 0; y <= tt.Length - 1; y++)
                                {
                                    for (int ii = 0; ii <= bb.Length - 1; ii++)
                                    {
                                        if (bb[ii].Contains(tt[y]))
                                        {
                                            j++;
                                        }
                                    }
                                }
                                if (j > 0)
                                {
                                    Odds = Utils.ParseInt(ub.GetSub("XinKuai3Two_dissame", xmlPath));
                                    long WinCent = Convert.ToInt64(Zhu_money * Odds * j);
                                    if (WinCent > 0)
                                    {
                                        new BCW.XinKuai3.BLL.XK3_Bet().Update_win(ID, WinCent);
                                        if (isRobot == 0)
                                            new BCW.BLL.Guest().Add(1, UsID, mename, "恭喜您在" + GameName + "第" + Lottery_issue + "期获得了" + WinCent + "" + ub.Get("SiteBz") + "[url=/bbs/game/xk3.aspx?act=case]马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                                        string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + mename + "[/url]在[url=/bbs/game/xk3.aspx]" + GameName + "[/url]《" + Lottery_issue + "期》获得了**" + ub.Get("SiteBz") + "";
                                        new BCW.BLL.Action().Add(1001, ID, UsID, "", wText);
                                    }
                                }
                                #endregion
                            }
                            //1大2小、1单2双
                            if (Play_Way == 9)//大小
                            {
                                #region
                                if (DaXiao == (model_1.DaXiao))
                                {
                                    long WinCent = Convert.ToInt64(Zhu_money * _odds);
                                    if (WinCent > 0)
                                    {
                                        new BCW.XinKuai3.BLL.XK3_Bet().Update_win(ID, WinCent);
                                        if (isRobot == 0)
                                            new BCW.BLL.Guest().Add(1, UsID, mename, "恭喜您在" + GameName + "第" + Lottery_issue + "期获得了" + WinCent + "" + ub.Get("SiteBz") + "[url=/bbs/game/xk3.aspx?act=case]马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                                        string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + mename + "[/url]在[url=/bbs/game/xk3.aspx]" + GameName + "[/url]《" + Lottery_issue + "期》获得了**" + ub.Get("SiteBz") + "";
                                        new BCW.BLL.Action().Add(1001, ID, UsID, "", wText);
                                    }
                                }
                                #endregion
                            }
                            if (Play_Way == 10)//单双
                            {
                                #region
                                if (DanShuang == (model_1.DanShuang))
                                {
                                    long WinCent = Convert.ToInt64(Zhu_money * _odds);
                                    if (WinCent > 0)
                                    {
                                        new BCW.XinKuai3.BLL.XK3_Bet().Update_win(ID, WinCent);
                                        if (isRobot == 0)
                                            new BCW.BLL.Guest().Add(1, UsID, mename, "恭喜您在" + GameName + "第" + Lottery_issue + "期获得了" + WinCent + "" + ub.Get("SiteBz") + "[url=/bbs/game/xk3.aspx?act=case]马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                                        string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + mename + "[/url]在[url=/bbs/game/xk3.aspx]" + GameName + "[/url]《" + Lottery_issue + "期》获得了**" + ub.Get("SiteBz") + "";
                                        new BCW.BLL.Action().Add(1001, ID, UsID, "", wText);
                                    }
                                }
                                #endregion
                            }

                        }
                    }
                }
                catch { }
            }
        }
    }

    //开始返彩====================试玩
    public void Open_price_SWB()
    {
        //检查数据库最后一期开奖号码
        BCW.XinKuai3.Model.XK3_Internet_Data model_1 = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3listLast2();//网络开奖数据
        string qihao = model_1.Lottery_issue;
        string kai = model_1.Lottery_num;
        //Response.Write("===============《新快3》_试玩版_数据库最后一期的开奖数据===============" + qihao + "：" + kai + "<br/>");
        if (kai != "")//如果开奖号码为空，则不返奖
        {
            int sum = Int32.Parse(model_1.Sum);
            int Odds = 0;//和值的倍数

            //检查投注表是否存在没开奖数据
            if (new BCW.XinKuai3.BLL.XK3_Bet_SWB().Exists_num(qihao))
            {
                Response.Write("<br />&nbsp;&nbsp;&nbsp;&nbsp;==============oh shit  《新快3》_试玩版_有未兑奖的==========<b></b><br /><br />");
                DataSet ds = new BCW.XinKuai3.BLL.XK3_Bet_SWB().GetList("*", "State=0 and Lottery_issue=" + qihao + "");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        //本地投注数据
                        int ID = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                        int UsID = int.Parse(ds.Tables[0].Rows[i]["UsID"].ToString());
                        int Play_Way = int.Parse(ds.Tables[0].Rows[i]["Play_Way"].ToString());
                        string Sum = ds.Tables[0].Rows[i]["Sum"].ToString();
                        string Three_Same_All = ds.Tables[0].Rows[i]["Three_Same_All"].ToString();
                        string Three_Same_Single = ds.Tables[0].Rows[i]["Three_Same_Single"].ToString();
                        string Three_Same_Not = ds.Tables[0].Rows[i]["Three_Same_Not"].ToString();
                        string Three_Continue_All = ds.Tables[0].Rows[i]["Three_Continue_All"].ToString();
                        string Two_Same_All = ds.Tables[0].Rows[i]["Two_Same_All"].ToString();
                        string Two_Same_Single = ds.Tables[0].Rows[i]["Two_Same_Single"].ToString();
                        string Two_dissame = ds.Tables[0].Rows[i]["Two_dissame"].ToString();
                        string Lottery_issue = ds.Tables[0].Rows[i]["Lottery_issue"].ToString();
                        string DanTuo = ds.Tables[0].Rows[i]["DanTuo"].ToString();
                        int Zhu = int.Parse(ds.Tables[0].Rows[i]["Zhu"].ToString());
                        int State = int.Parse(ds.Tables[0].Rows[i]["State"].ToString());
                        long Zhu_money = Int64.Parse(ds.Tables[0].Rows[i]["Zhu_money"].ToString());
                        long PutGold = Int64.Parse(ds.Tables[0].Rows[i]["PutGold"].ToString());
                        long GetMoney = Int64.Parse(ds.Tables[0].Rows[i]["GetMoney"].ToString());
                        string DaXiao = (ds.Tables[0].Rows[i]["DaXiao"].ToString());
                        string DanShuang = (ds.Tables[0].Rows[i]["DanShuang"].ToString());
                        float _odds = float.Parse(ds.Tables[0].Rows[i]["Odds"].ToString());
                        string mename = new BCW.BLL.User().GetUsName(UsID);//获得id对应的用户名

                        if (Play_Way == 1)//和值
                        {
                            if ((sum == 4) || (sum == 17))
                            {
                                Odds = Utils.ParseInt(ub.GetSub("XinKuai3Sum2", xmlPath_SWB));
                            }
                            else if ((sum == 5) || (sum == 16))
                            {
                                Odds = Utils.ParseInt(ub.GetSub("XSum1", xmlPath_SWB));
                            }
                            else if ((sum == 6) || (sum == 15))
                            {
                                Odds = Utils.ParseInt(ub.GetSub("XSum2", xmlPath_SWB));
                            }
                            else if ((sum == 7) || (sum == 14))
                            {
                                Odds = Utils.ParseInt(ub.GetSub("XSum3", xmlPath_SWB));
                            }
                            else if ((sum == 8) || (sum == 13))
                            {
                                Odds = Utils.ParseInt(ub.GetSub("XSum4", xmlPath_SWB));
                            }
                            else if ((sum == 9) || (sum == 12))
                            {
                                Odds = Utils.ParseInt(ub.GetSub("XSum5", xmlPath_SWB));
                            }
                            else if ((sum == 10) || (sum == 11))
                            {
                                Odds = Utils.ParseInt(ub.GetSub("XinKuai3Sum1", xmlPath_SWB));
                            }
                            int a = 0;
                            string[] bd_sum = Sum.Split(',');
                            for (int bd = 0; bd < bd_sum.Length; bd++)
                            {
                                if (bd_sum[bd] == model_1.Sum)
                                {
                                    a++;
                                }
                            }
                            if (a == 1)
                            {
                                long WinCent = Convert.ToInt64(Zhu_money * Odds);
                                new BCW.XinKuai3.BLL.XK3_Bet_SWB().Update_win(ID, WinCent);
                                new BCW.BLL.Guest().Add(1, UsID, mename, "恭喜您在" + GameName + "_试玩版第" + Lottery_issue + "期《和值投注》获得了" + WinCent + "" + klb + "[url=/bbs/game/xk3sw.aspx?act=case]马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                                string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + mename + "[/url]在[url=/bbs/game/xk3sw.aspx]" + GameName + "_试玩版[/url]《" + Lottery_issue + "期》获得了**" + klb + "";
                                new BCW.BLL.Action().Add(1002, ID, UsID, "", wText);

                                //BCW.XinKuai3.Model.XK3_Toplist_SWB model_2 = new BCW.XinKuai3.BLL.XK3_Toplist_SWB().GetXK3_meid(UsID);
                                //long all_prices = model_2.WinGold + WinCent;
                                //new BCW.XinKuai3.BLL.XK3_Toplist_SWB().Update_getgold(UsID, all_prices);//兑奖的时候，把赢的钱，存进该meid对应的WinGold字段。
                            }
                        }
                        if (Play_Way == 2)//三同号通选
                        {
                            if (model_1.Three_Same_All == "1")
                            {
                                Odds = Utils.ParseInt(ub.GetSub("XinKuai3Three_Same_All", xmlPath_SWB));
                                long WinCent = Convert.ToInt64(Zhu_money * Odds);
                                new BCW.XinKuai3.BLL.XK3_Bet_SWB().Update_win(ID, WinCent);
                                new BCW.BLL.Guest().Add(1, UsID, mename, "恭喜您在" + GameName + "_试玩版第" + Lottery_issue + "期《三同号通选投注》获得了" + WinCent + "" + klb + "[url=/bbs/game/xk3sw.aspx?act=case]马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                                string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + mename + "[/url]在[url=/bbs/game/xk3sw.aspx]" + GameName + "_试玩版[/url]《" + Lottery_issue + "期》获得了**" + klb + "";
                                new BCW.BLL.Action().Add(1002, ID, UsID, "", wText);

                            }
                        }
                        if (Play_Way == 3)//三同号单选
                        {
                            if (Three_Same_Single.Contains(model_1.Three_Same_Single))
                            {
                                Odds = Utils.ParseInt(ub.GetSub("XinKuai3Three_Same_Single", xmlPath_SWB));
                                long WinCent = Convert.ToInt64(Zhu_money * Odds);
                                new BCW.XinKuai3.BLL.XK3_Bet_SWB().Update_win(ID, WinCent);
                                new BCW.BLL.Guest().Add(1, UsID, mename, "恭喜您在" + GameName + "_试玩版第" + Lottery_issue + "期《三同号单选投注》获得了" + WinCent + "" + klb + "[url=/bbs/game/xk3sw.aspx?act=case]马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                                string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + mename + "[/url]在[url=/bbs/game/xk3sw.aspx]" + GameName + "_试玩版[/url]《" + Lottery_issue + "期》获得了**" + klb + "";
                                new BCW.BLL.Action().Add(1002, ID, UsID, "", wText);

                            }
                        }
                        if (Play_Way == 4)//三不同号
                        {
                            if (Three_Same_Not.Contains(model_1.Three_Same_Not))
                            {
                                Odds = Utils.ParseInt(ub.GetSub("XinKuai3Three_Same_Not", xmlPath_SWB));
                                long WinCent = Convert.ToInt64(Zhu_money * Odds);
                                new BCW.XinKuai3.BLL.XK3_Bet_SWB().Update_win(ID, WinCent);
                                new BCW.BLL.Guest().Add(1, UsID, mename, "恭喜您在" + GameName + "_试玩版第" + Lottery_issue + "期《三不同号投注》获得了" + WinCent + "" + klb + "[url=/bbs/game/xk3sw.aspx?act=case]马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                                string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + mename + "[/url]在[url=/bbs/game/xk3sw.aspx]" + GameName + "_试玩版[/url]《" + Lottery_issue + "期》获得了**" + klb + "";
                                new BCW.BLL.Action().Add(1002, ID, UsID, "", wText);

                            }
                        }
                        if (Play_Way == 5)//三连号通选
                        {
                            if (model_1.Three_Continue_All == "1")
                            {
                                Odds = Utils.ParseInt(ub.GetSub("XinKuai3Three_Continue_All", xmlPath_SWB));
                                long WinCent = Convert.ToInt64(Zhu_money * Odds);
                                new BCW.XinKuai3.BLL.XK3_Bet_SWB().Update_win(ID, WinCent);
                                new BCW.BLL.Guest().Add(1, UsID, mename, "恭喜您在" + GameName + "_试玩版第" + Lottery_issue + "期《三连号通选投注》获得了" + WinCent + "" + klb + "[url=/bbs/game/xk3sw.aspx?act=case]马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                                string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + mename + "[/url]在[url=/bbs/game/xk3sw.aspx]" + GameName + "_试玩版[/url]《" + Lottery_issue + "期》获得了**" + klb + "";
                                new BCW.BLL.Action().Add(1002, ID, UsID, "", wText);

                            }
                        }
                        if (Play_Way == 6)//二同号复选
                        {
                            if (Two_Same_All.Contains(model_1.Two_Same_All))
                            {
                                Odds = Utils.ParseInt(ub.GetSub("XinKuai3Two_Same_All", xmlPath_SWB));
                                long WinCent = Convert.ToInt64(Zhu_money * Odds);
                                new BCW.XinKuai3.BLL.XK3_Bet_SWB().Update_win(ID, WinCent);
                                new BCW.BLL.Guest().Add(1, UsID, mename, "恭喜您在" + GameName + "_试玩版第" + Lottery_issue + "期《二同号复选投注》获得了" + WinCent + "" + klb + "[url=/bbs/game/xk3sw.aspx?act=case]马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                                string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + mename + "[/url]在[url=/bbs/game/xk3sw.aspx]" + GameName + "_试玩版[/url]《" + Lottery_issue + "期》获得了**" + klb + "";
                                new BCW.BLL.Action().Add(1002, ID, UsID, "", wText);

                            }
                        }
                        if (Play_Way == 7)//二同号单选
                        {
                            string[] a1 = Two_Same_Single.Split(',');//分割购买的数据 {221,223,224,551,553,554}
                            int e = 0;
                            for (int p = 0; p < a1.Length; p++)
                            {
                                //各自赋值给y
                                int y = Convert.ToInt32(a1[p]);
                                int y1 = y / 100;
                                int y2 = (y - y1 * 100) / 10;
                                int y3 = (y - y1 * 100 - y2 * 10);
                                int[] num3 = { y1, y2, y3 };

                                //冒泡排序 从小到大
                                for (int t = 0; t < 3; t++)
                                {
                                    for (int j = t + 1; j < 3; j++)
                                    {
                                        if (num3[j] < num3[t])
                                        {
                                            int temp = num3[t];
                                            num3[t] = num3[j];
                                            num3[j] = temp;
                                        }
                                    }
                                }
                                string num22 = string.Empty;
                                for (int w = 0; w < 3; w++)//遍历数组显示结果
                                {
                                    num22 = num22 + num3[w];
                                }
                                if (num22.Contains(model_1.Two_Same_Single))
                                {
                                    e++;
                                }
                                if (e > 0)
                                {
                                    break;
                                }
                            }
                            //if (Two_Same_Single.Contains(model_1.Two_Same_Single))
                            if (e > 0)
                            {
                                Odds = Utils.ParseInt(ub.GetSub("XinKuai3Two_Same_Single", xmlPath_SWB));
                                long WinCent = Convert.ToInt64(Zhu_money * Odds);
                                new BCW.XinKuai3.BLL.XK3_Bet_SWB().Update_win(ID, WinCent);
                                new BCW.BLL.Guest().Add(1, UsID, mename, "恭喜您在" + GameName + "_试玩版第" + Lottery_issue + "期《二同号单选投注》获得了" + WinCent + "" + klb + "[url=/bbs/game/xk3sw.aspx?act=case]马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                                string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + mename + "[/url]在[url=/bbs/game/xk3sw.aspx]" + GameName + "_试玩版[/url]《" + Lottery_issue + "期》获得了**" + klb + "";
                                new BCW.BLL.Action().Add(1002, ID, UsID, "", wText);
                            }
                        }
                        if (Play_Way == 8)//二不同号
                        {
                            string[] tt = (model_1.Two_dissame).Split(',');//开奖的数据
                            string[] bb = Two_dissame.Split(',');
                            int a = tt.Length;
                            int b = bb.Length;
                            int j = 0;

                            for (int y = 0; y <= tt.Length - 1; y++)
                            {
                                for (int ii = 0; ii <= bb.Length - 1; ii++)
                                {
                                    if (bb[ii].Contains(tt[y]))
                                    {
                                        j++;
                                    }
                                }
                            }
                            if (j > 0)
                            {
                                Odds = Utils.ParseInt(ub.GetSub("XinKuai3Two_dissame", xmlPath_SWB));
                                long WinCent = Convert.ToInt64(Zhu_money * Odds * j);
                                new BCW.XinKuai3.BLL.XK3_Bet_SWB().Update_win(ID, WinCent);
                                new BCW.BLL.Guest().Add(1, UsID, mename, "恭喜您在" + GameName + "_试玩版第" + Lottery_issue + "期《二不同号投注》获得了" + WinCent + "" + klb + "[url=/bbs/game/xk3sw.aspx?act=case]马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                                string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + mename + "[/url]在[url=/bbs/game/xk3sw.aspx]" + GameName + "_试玩版[/url]《" + Lottery_issue + "期》获得了**" + klb + "";
                                new BCW.BLL.Action().Add(1002, ID, UsID, "", wText);
                            }
                        }
                        if (Play_Way == 9)//大小
                        {
                            if (DaXiao == (model_1.DaXiao))
                            {
                                long WinCent = Convert.ToInt64(Zhu_money * _odds);
                                new BCW.XinKuai3.BLL.XK3_Bet_SWB().Update_win(ID, WinCent);
                                new BCW.BLL.Guest().Add(1, UsID, mename, "恭喜您在" + GameName + "_试玩版第" + Lottery_issue + "期《大小投注》获得了" + WinCent + "" + klb + "[url=/bbs/game/xk3sw.aspx?act=case]马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                                string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + mename + "[/url]在[url=/bbs/game/xk3sw.aspx]" + GameName + "_试玩版[/url]《" + Lottery_issue + "期》获得了**" + klb + "";
                                new BCW.BLL.Action().Add(1002, ID, UsID, "", wText);
                            }
                        }
                        if (Play_Way == 10)//单双
                        {
                            if (DanShuang == (model_1.DanShuang))
                            {
                                long WinCent = Convert.ToInt64(Zhu_money * _odds);
                                new BCW.XinKuai3.BLL.XK3_Bet_SWB().Update_win(ID, WinCent);
                                new BCW.BLL.Guest().Add(1, UsID, mename, "恭喜您在" + GameName + "_试玩版第" + Lottery_issue + "期《单双投注》获得了" + WinCent + "" + klb + "[url=/bbs/game/xk3sw.aspx?act=case]马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                                string wText = "[url=/bbs/uinfo.aspx?uid=" + UsID + "]" + mename + "[/url]在[url=/bbs/game/xk3sw.aspx]" + GameName + "_试玩版[/url]《" + Lottery_issue + "期》获得了**" + klb + "";
                                new BCW.BLL.Action().Add(1002, ID, UsID, "", wText);
                            }
                        }
                        new BCW.XinKuai3.BLL.XK3_Bet_SWB().UpdateState(ID, 1);
                    }
                }

            }
        }
        else
        {
            //此次应该通知管理员：第几期有期号，而没开奖号码。
            //Response.Write("<br/><br/><b>《《《请注意：第" + qihao + "期因没有开奖号码，返奖失败。。。》》》</b><br/><br/>");
        }
    }

    //系统收回过期未兑奖的====================正式
    public void update_state()
    {
        string _where = string.Empty;
        _where = "WHERE GetMoney>0 AND State=1 AND Input_Time<DateAdd(dd,-" + date_1 + ",getdate())";
        new BCW.XinKuai3.BLL.XK3_Bet().UpdateExceed_num(_where);
    }

    //系统收回过期未兑奖的====================试玩
    public void update_state_SWB()
    {
        string _where = string.Empty;
        _where = "WHERE GetMoney>0 AND State=1 AND Input_Time<DateAdd(dd,-" + date_1 + ",getdate())";
        new BCW.XinKuai3.BLL.XK3_Bet_SWB().UpdateExceed_num(_where);
    }

    //根据最近几期的大小单双开奖情况，实时变动赔率。========正式
    public void change_peilv()
    {
        BCW.XinKuai3.Model.XK3_Internet_Data model_1 = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3listLast3();//网络开奖数据==Getxk3listLast2
        string qihao = model_1.Lottery_issue;
        string kai = model_1.Lottery_num;

        ub xml = new ub();
        string xmlPath_update = "/Controls/xinkuai3.xml";
        Application.Remove(xmlPath_update);//清缓存
        xml.ReloadSub(xmlPath_update); //加载配置

        if (kai != "" && qihao != "")
        {
            string issue3 = Utils.Right(model_1.Lottery_issue.ToString(), 3);//本期开奖期号的后3位
            if (issue3 != "001")
            {
                DataSet d1, d2;
                d1 = new BCW.XinKuai3.BLL.XK3_Internet_Data().GetList("TOP 1 *", "DaXiao!='' ORDER BY Lottery_time DESC");//DaXiao!='' ORDER BY ID DESC
                d2 = new BCW.XinKuai3.BLL.XK3_Internet_Data().GetList2("TOP 1 *", "id!='' ORDER BY Lottery_time ASC ");

                string Cents1 = "";
                string Cents2 = "";
                string Cents3 = "";
                string Cents4 = "";
                for (int i = 0; i < d1.Tables[0].Rows.Count; i++)
                {
                    try
                    {
                        Cents1 = Convert.ToString(d1.Tables[0].Rows[i]["DaXiao"]);//最后一期的大小
                        Cents2 = Convert.ToString(d1.Tables[0].Rows[i]["DanShuang"]);//最后一期的单双
                    }
                    catch
                    {
                        Cents1 = "";
                        Cents2 = "";
                    }
                }
                for (int i = 0; i < d2.Tables[0].Rows.Count; i++)
                {
                    try
                    {
                        Cents3 = Convert.ToString(d2.Tables[0].Rows[i]["DaXiao"]);//倒数第2期的大小
                        Cents4 = Convert.ToString(d2.Tables[0].Rows[i]["DanShuang"]);//倒数第2期的单双
                    }
                    catch
                    {
                        Cents3 = "";
                        Cents4 = "";
                    }
                }
                if (Cents1 != Cents3)//如果连续2期不相等，还原赔率----大小
                {
                    xml.dss["Xda1"] = xml.dss["Xda"];
                    xml.dss["Xxiao1"] = xml.dss["Xxiao"];
                    System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                }
                else
                {
                    //1大2小、1单2双
                    if (Cents1 == "2" && Cents3 == "2")//如果连续2期开小
                    {
                        if (Convert.ToDouble(xml.dss["Xxiao1"]) >= Convert.ToDouble(xml.dss["Xoverpeilv"]))//如果增加到一定的赔率，则不再增加。
                        {
                            xml.dss["Xda1"] = Convert.ToDouble(xml.dss["Xda1"]) - Convert.ToDouble(xml.dss["Xfudong"]);//大的赔率减少
                        }
                        else
                        {
                            xml.dss["Xxiao1"] = Convert.ToDouble(xml.dss["Xxiao1"]) + Convert.ToDouble(xml.dss["Xfudong"]);//小的赔率增加
                            xml.dss["Xda1"] = Convert.ToDouble(xml.dss["Xda1"]) - Convert.ToDouble(xml.dss["Xfudong"]);//大的赔率减少
                        }
                        System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                    }
                    else if (Cents1 == "1" && Cents3 == "1")//如果连续2期开大
                    {
                        if (Convert.ToDouble(xml.dss["Xda1"]) >= Convert.ToDouble(xml.dss["Xoverpeilv"]))
                        {
                            xml.dss["Xxiao1"] = Convert.ToDouble(xml.dss["Xxiao1"]) - Convert.ToDouble(xml.dss["Xfudong"]);//小的赔率减少
                        }
                        else
                        {
                            xml.dss["Xxiao1"] = Convert.ToDouble(xml.dss["Xxiao1"]) - Convert.ToDouble(xml.dss["Xfudong"]);//小的赔率减少
                            xml.dss["Xda1"] = Convert.ToDouble(xml.dss["Xda1"]) + Convert.ToDouble(xml.dss["Xfudong"]);//大的赔率增加
                        }
                        System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                    }
                }
                if (Cents2 != Cents4)//如果连续2期不相等，还原赔率----单双
                {
                    xml.dss["Xdan1"] = xml.dss["Xdan"];
                    xml.dss["Xshuang1"] = xml.dss["Xshuang"];
                    System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                }
                else
                {
                    //1大2小、1单2双
                    if (Cents2 == "1" && Cents4 == "1")//如果连续2期开单
                    {
                        if (Convert.ToDouble(xml.dss["Xdan1"]) >= Convert.ToDouble(xml.dss["Xoverpeilv"]))
                        {
                            xml.dss["Xshuang1"] = Convert.ToDouble(xml.dss["Xshuang1"]) - Convert.ToDouble(xml.dss["Xfudong"]);//双的赔率减少
                        }
                        else
                        {
                            xml.dss["Xdan1"] = Convert.ToDouble(xml.dss["Xdan1"]) + Convert.ToDouble(xml.dss["Xfudong"]);//单的赔率增加
                            xml.dss["Xshuang1"] = Convert.ToDouble(xml.dss["Xshuang1"]) - Convert.ToDouble(xml.dss["Xfudong"]);//双的赔率减少
                        }
                        System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                    }
                    else if (Cents2 == "2" && Cents4 == "2")//双
                    {
                        if (Convert.ToDouble(xml.dss["Xshuang1"]) >= Convert.ToDouble(xml.dss["Xoverpeilv"]))
                        {
                            xml.dss["Xdan1"] = Convert.ToDouble(xml.dss["Xdan1"]) - Convert.ToDouble(xml.dss["Xfudong"]);//单的赔率减少
                        }
                        else
                        {
                            xml.dss["Xdan1"] = Convert.ToDouble(xml.dss["Xdan1"]) - Convert.ToDouble(xml.dss["Xfudong"]);//单的赔率减少
                            xml.dss["Xshuang1"] = Convert.ToDouble(xml.dss["Xshuang1"]) + Convert.ToDouble(xml.dss["Xfudong"]);//双的赔率增加
                        }
                        System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                    }
                }
            }
        }
        //如果最后一期没有开奖期号和开奖号码，则大小单双的赔率重置
        else
        {
            xml.dss["Xda1"] = xml.dss["Xda"];
            xml.dss["Xxiao1"] = xml.dss["Xxiao"];
            xml.dss["Xdan1"] = xml.dss["Xdan"];
            xml.dss["Xshuang1"] = xml.dss["Xshuang"];
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
        }
    }

    //赔率浮动==正式20160803 邵广林 修改浮动赔率
    public void change_peilv_new()
    {
        //正式
        ub xml = new ub();
        string xmlPath_update = "/Controls/xinkuai3.xml";
        Application.Remove(xmlPath_update);
        xml.ReloadSub(xmlPath_update);

        //试玩
        ub xml2 = new ub();
        string xmlPath_update2 = "/Controls/xinkuai3_TRIAL_GAME.xml";
        Application.Remove(xmlPath_update2);
        xml2.ReloadSub(xmlPath_update2);

        BCW.XinKuai3.Model.XK3_Internet_Data model_1 = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3listLast3();
        string issue3 = Utils.Right(model_1.Lottery_issue.ToString(), 3);//本期开奖期号的后3位
        if (issue3 != "001")
        {
            string a = string.Empty;
            string b = string.Empty;
            DataSet d1 = new BCW.XinKuai3.BLL.XK3_Internet_Data().GetList("TOP(20)*", "Lottery_num!='' AND DateDiff(dd,Lottery_time,getdate())=0 Order by Lottery_time desc");
            for (int i = 0; i < d1.Tables[0].Rows.Count; i++)
            {
                if (i == d1.Tables[0].Rows.Count - 1)
                {
                    a = a + Convert.ToInt32(d1.Tables[0].Rows[i]["DaXiao"]);
                    b = b + Convert.ToInt32(d1.Tables[0].Rows[i]["DanShuang"]);
                }
                else
                {
                    a = a + Convert.ToInt32(d1.Tables[0].Rows[i]["DaXiao"]) + ",";
                    b = b + Convert.ToInt32(d1.Tables[0].Rows[i]["DanShuang"]) + ",";
                }
            }
            try
            {
                if (a.Length > 0)
                {
                    string[] sa = a.Split(',');
                    string[] sb = b.Split(',');
                    //int ab1 = suan(0, sa, 0, d1.Tables[0].Rows.Count - 1);
                    //int ab2 = suan(0, sb, 0, d1.Tables[0].Rows.Count - 1);
                    int ab1 = suan(0, sa, 0, a.Length, a.Split(',').Length);
                    int ab2 = suan(0, sb, 0, b.Length, b.Split(',').Length);

                    if (sa[0] == "1")//1大2小
                    {
                        if (Convert.ToDouble(xml.dss["Xda1"]) >= Convert.ToDouble(xml.dss["Xoverpeilv"]))//额度
                        {
                            xml.dss["Xxiao1"] = Convert.ToDouble(xml.dss["Xxiao"]) - Convert.ToDouble(xml.dss["Xfudong"]) * ab1;//小的赔率减少
                        }
                        else
                        {
                            xml.dss["Xxiao1"] = Convert.ToDouble(xml.dss["Xxiao"]) - Convert.ToDouble(xml.dss["Xfudong"]) * ab1;//小的赔率减少
                            xml.dss["Xda1"] = Convert.ToDouble(xml.dss["Xda"]) + Convert.ToDouble(xml.dss["Xfudong"]) * ab1;//大的赔率增加
                        }
                        System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                        if (Utils.GetDomain().Contains("kb288"))
                            System.IO.File.WriteAllText(Server.MapPath(xmlPath_SWB), xml2.Post(xml2.dss), System.Text.Encoding.UTF8);
                    }
                    else
                    {
                        if (Convert.ToDouble(xml.dss["Xxiao1"]) >= Convert.ToDouble(xml.dss["Xoverpeilv"]))//如果增加到一定的赔率，则不再增加。
                        {
                            xml.dss["Xda1"] = Convert.ToDouble(xml.dss["Xda"]) - Convert.ToDouble(xml.dss["Xfudong"]) * ab1;//大的赔率减少
                        }
                        else
                        {
                            xml.dss["Xda1"] = Convert.ToDouble(xml.dss["Xda"]) - Convert.ToDouble(xml.dss["Xfudong"]) * ab1;//大的赔率减少
                            xml.dss["Xxiao1"] = Convert.ToDouble(xml.dss["Xxiao"]) + Convert.ToDouble(xml.dss["Xfudong"]) * ab1;//小的赔率增加
                        }
                        System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                        if (Utils.GetDomain().Contains("kb288"))
                            System.IO.File.WriteAllText(Server.MapPath(xmlPath_SWB), xml2.Post(xml2.dss), System.Text.Encoding.UTF8);
                    }
                    if (sb[0] == "1")//1单2双
                    {
                        if (Convert.ToDouble(xml.dss["Xdan1"]) >= Convert.ToDouble(xml.dss["Xoverpeilv"]))
                        {
                            xml.dss["Xshuang1"] = Convert.ToDouble(xml.dss["Xshuang"]) - Convert.ToDouble(xml.dss["Xfudong"]) * ab2;//双的赔率减少
                        }
                        else
                        {
                            xml.dss["Xshuang1"] = Convert.ToDouble(xml.dss["Xshuang"]) - Convert.ToDouble(xml.dss["Xfudong"]) * ab2;//双的赔率减少
                            xml.dss["Xdan1"] = Convert.ToDouble(xml.dss["Xdan"]) + Convert.ToDouble(xml.dss["Xfudong"]) * ab2;//单的赔率增加
                        }
                        System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                        if (Utils.GetDomain().Contains("kb288"))
                            System.IO.File.WriteAllText(Server.MapPath(xmlPath_SWB), xml2.Post(xml2.dss), System.Text.Encoding.UTF8);
                    }
                    else
                    {
                        if (Convert.ToDouble(xml.dss["Xshuang1"]) >= Convert.ToDouble(xml.dss["Xoverpeilv"]))
                        {
                            xml.dss["Xdan1"] = Convert.ToDouble(xml.dss["Xdan"]) - Convert.ToDouble(xml.dss["Xfudong"]) * ab2;//单的赔率减少
                        }
                        else
                        {
                            xml.dss["Xdan1"] = Convert.ToDouble(xml.dss["Xdan"]) - Convert.ToDouble(xml.dss["Xfudong"]) * ab2;//单的赔率减少
                            xml.dss["Xshuang1"] = Convert.ToDouble(xml.dss["Xshuang"]) + Convert.ToDouble(xml.dss["Xfudong"]) * ab2;//双的赔率增加
                        }
                        System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                        if (Utils.GetDomain().Contains("kb288"))
                            System.IO.File.WriteAllText(Server.MapPath(xmlPath_SWB), xml2.Post(xml2.dss), System.Text.Encoding.UTF8);
                    }
                }
            }
            catch
            {
                xml.dss["Xda1"] = xml.dss["Xda"];
                xml.dss["Xxiao1"] = xml.dss["Xxiao"];
                xml.dss["Xdan1"] = xml.dss["Xdan"];
                xml.dss["Xshuang1"] = xml.dss["Xshuang"];
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                if (Utils.GetDomain().Contains("kb288"))
                    System.IO.File.WriteAllText(Server.MapPath(xmlPath_SWB), xml2.Post(xml2.dss), System.Text.Encoding.UTF8);
            }
        }
    }


    //根据最近几期的大小单双开奖情况，实时变动赔率。========试玩
    public void change_peilv_SWB()
    {
        BCW.XinKuai3.Model.XK3_Internet_Data model_1 = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3listLast3();//网络开奖数据
        string qihao = model_1.Lottery_issue;
        string kai = model_1.Lottery_num;

        ub xml = new ub();
        string xmlPath_update2 = "/Controls/xinkuai3_TRIAL_GAME.xml";
        Application.Remove(xmlPath_update2);//清缓存
        xml.ReloadSub(xmlPath_update2); //加载配置

        if (kai != "" && qihao != "")
        {
            string issue3 = Utils.Right(model_1.Lottery_issue.ToString(), 3);//本期开奖期号的后3位
            if (issue3 != "001")
            {
                DataSet d1, d2;
                d1 = new BCW.XinKuai3.BLL.XK3_Internet_Data().GetList("TOP 1 *", "DaXiao!='' ORDER BY Lottery_time DESC");
                d2 = new BCW.XinKuai3.BLL.XK3_Internet_Data().GetList2("TOP 1 *", "id!='' ORDER BY Lottery_time ASC ");

                string Cents1 = "";
                string Cents2 = "";
                string Cents3 = "";
                string Cents4 = "";
                for (int i = 0; i < d1.Tables[0].Rows.Count; i++)
                {
                    try
                    {
                        Cents1 = Convert.ToString(d1.Tables[0].Rows[i]["DaXiao"]);//最后一期的大小
                        Cents2 = Convert.ToString(d1.Tables[0].Rows[i]["DanShuang"]);//最后一期的单双
                    }
                    catch
                    {
                        Cents1 = "";
                        Cents2 = "";
                    }
                }
                for (int i = 0; i < d2.Tables[0].Rows.Count; i++)
                {
                    try
                    {
                        Cents3 = Convert.ToString(d2.Tables[0].Rows[i]["DaXiao"]);//倒数第2期的大小
                        Cents4 = Convert.ToString(d2.Tables[0].Rows[i]["DanShuang"]);//倒数第2期的单双
                    }
                    catch
                    {
                        Cents3 = "";
                        Cents4 = "";
                    }
                }
                if (Cents1 != Cents3)//如果连续2期不相等，还原赔率----大小
                {
                    xml.dss["Xda1"] = xml.dss["Xda"];
                    xml.dss["Xxiao1"] = xml.dss["Xxiao"];
                    System.IO.File.WriteAllText(Server.MapPath(xmlPath_SWB), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                }
                else
                {
                    //1大2小、1单2双
                    if (Cents1 == "2" && Cents3 == "2")//如果连续2期开小
                    {
                        if (Convert.ToDouble(xml.dss["Xxiao1"]) >= Convert.ToDouble(xml.dss["Xoverpeilv"]))//如果增加到一定的赔率，则不再增加。
                        {
                            xml.dss["Xda1"] = Convert.ToDouble(xml.dss["Xda1"]) - Convert.ToDouble(xml.dss["Xfudong"]);//大的赔率减少
                        }
                        else
                        {
                            xml.dss["Xxiao1"] = Convert.ToDouble(xml.dss["Xxiao1"]) + Convert.ToDouble(xml.dss["Xfudong"]);//小的赔率增加
                            xml.dss["Xda1"] = Convert.ToDouble(xml.dss["Xda1"]) - Convert.ToDouble(xml.dss["Xfudong"]);//大的赔率减少
                        }
                        System.IO.File.WriteAllText(Server.MapPath(xmlPath_SWB), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                    }
                    else if (Cents1 == "1" && Cents3 == "1")//大
                    {
                        if (Convert.ToDouble(xml.dss["Xda1"]) >= Convert.ToDouble(xml.dss["Xoverpeilv"]))
                        {
                            xml.dss["Xxiao1"] = Convert.ToDouble(xml.dss["Xxiao1"]) - Convert.ToDouble(xml.dss["Xfudong"]);//小的赔率减少
                        }
                        else
                        {
                            xml.dss["Xxiao1"] = Convert.ToDouble(xml.dss["Xxiao1"]) - Convert.ToDouble(xml.dss["Xfudong"]);//小的赔率减少
                            xml.dss["Xda1"] = Convert.ToDouble(xml.dss["Xda1"]) + Convert.ToDouble(xml.dss["Xfudong"]);//大的赔率增加
                        }
                        System.IO.File.WriteAllText(Server.MapPath(xmlPath_SWB), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                    }
                }
                if (Cents2 != Cents4)//如果连续2期不相等，还原赔率----单双
                {
                    xml.dss["Xdan1"] = xml.dss["Xdan"];
                    xml.dss["Xshuang1"] = xml.dss["Xshuang"];
                    System.IO.File.WriteAllText(Server.MapPath(xmlPath_SWB), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                }
                else
                {
                    if (Cents2 == "1" && Cents4 == "1")//如果连续2期开单
                    {
                        if (Convert.ToDouble(xml.dss["Xdan1"]) >= Convert.ToDouble(xml.dss["Xoverpeilv"]))
                        {
                            xml.dss["Xshuang1"] = Convert.ToDouble(xml.dss["Xshuang1"]) - Convert.ToDouble(xml.dss["Xfudong"]);//双的赔率减少
                        }
                        else
                        {
                            xml.dss["Xdan1"] = Convert.ToDouble(xml.dss["Xdan1"]) + Convert.ToDouble(xml.dss["Xfudong"]);//单的赔率增加
                            xml.dss["Xshuang1"] = Convert.ToDouble(xml.dss["Xshuang1"]) - Convert.ToDouble(xml.dss["Xfudong"]);//双的赔率减少
                        }
                        System.IO.File.WriteAllText(Server.MapPath(xmlPath_SWB), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                    }
                    else if (Cents2 == "2" && Cents4 == "2")//双
                    {
                        if (Convert.ToDouble(xml.dss["Xshuang1"]) >= Convert.ToDouble(xml.dss["Xoverpeilv"]))
                        {
                            xml.dss["Xdan1"] = Convert.ToDouble(xml.dss["Xdan1"]) - Convert.ToDouble(xml.dss["Xfudong"]);//单的赔率减少
                        }
                        else
                        {
                            xml.dss["Xdan1"] = Convert.ToDouble(xml.dss["Xdan1"]) - Convert.ToDouble(xml.dss["Xfudong"]);//单的赔率减少
                            xml.dss["Xshuang1"] = Convert.ToDouble(xml.dss["Xshuang1"]) + Convert.ToDouble(xml.dss["Xfudong"]);//双的赔率增加
                        }
                        System.IO.File.WriteAllText(Server.MapPath(xmlPath_SWB), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                    }
                }
            }
        }
        //如果最后一期没有开奖期号和开奖号码，则大小单双的赔率重置
        else
        {
            xml.dss["Xda1"] = xml.dss["Xda"];
            xml.dss["Xxiao1"] = xml.dss["Xxiao"];
            xml.dss["Xdan1"] = xml.dss["Xdan"];
            xml.dss["Xshuang1"] = xml.dss["Xshuang"];
            System.IO.File.WriteAllText(Server.MapPath(xmlPath_SWB), xml.Post(xml.dss), System.Text.Encoding.UTF8);
        }
    }

    //机器人兑奖
    public void Robot_case()
    {
        DataSet ds = new BCW.XinKuai3.BLL.XK3_Bet().GetList("*", "GetMoney>0 AND State=1 AND isRobot=1");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                //本地投注数据
                int ID = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                int UsID = int.Parse(ds.Tables[0].Rows[i]["UsID"].ToString());
                long GetMoney = Int64.Parse(ds.Tables[0].Rows[i]["GetMoney"].ToString());

                string mename = new BCW.BLL.User().GetUsName(UsID);//获得id对应的用户名

                //操作币

                new BCW.XinKuai3.BLL.XK3_Bet().UpdateState(ID, 2);//当state为1的时候，即为系统已开奖

                new BCW.BLL.User().UpdateiGold(UsID, GetMoney, "" + GameName + "兑奖-标识ID" + ID + "");

                //BCW.XinKuai3.Model.XK3_Toplist model_2 = new BCW.XinKuai3.BLL.XK3_Toplist().GetXK3_meid(UsID);
                //long all_prices = model_2.WinGold + GetMoney;
                //邵广林  20160701 去掉机器人重复领奖
                //new BCW.XinKuai3.BLL.XK3_Toplist().Update_getgold(UsID, all_prices);//兑奖的时候，把赢的钱，存进该meid对应的WinGold字段。
            }
        }

    }

    /// 获取后几位数
    /// <param name="str">要截取的字符串</param>
    /// <param name="num">返回的具体位数</param>
    /// <returns>返回结果的字符串</returns>
    public string GetLastStr(string str, int num)
    {
        int count = 0;
        if (str.Length > num)
        {
            count = str.Length - num;
            str = str.Substring(count, num);
        }
        return str;
    }

}


