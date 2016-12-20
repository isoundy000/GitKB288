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
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using BCW.Common;
using BCW.HP3;
using System.Timers;
using System.Xml;

/// <summary>
/// 蒙宗将 20161004 抓取优化
/// 蒙宗将 20161006 重写大小单双赔率
/// 蒙宗将 20161018 开奖任选3-6修改
/// 蒙宗将 20161028 修复顺子开奖
/// 蒙宗将 20161029 开奖获取机器人修改
///        20161122 直六返奖修复
/// </summary>

public partial class bbs_game_HP3GetNum : System.Web.UI.Page
{
    protected string xmlPath = "/Controls/HappyPoker3.xml";
    protected string GameName = ub.GetSub("HP3Name", "/Controls/HappyPoker3.xml");
    //读取xml中数据

    //显示
    protected void Page_Load(object sender, EventArgs e)
    {
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();
        Response.Write(GameName + "ok1");
        try
        {
            Response.Write("<br />最新期号:" + UpdateState() + "期");
        }
        catch
        {
            Response.Write("<br /><span style='color:red;font-size:50px'>更新最新期号出现异常！</span><br/>");
        }
        try
        {
            Response.Write("<br />第" + GetStage() + "期");
        }
        catch
        {
            Response.Write("<br /><span style='color:red;font-size:50px'>期号获取出现异常！</span><br/>");
        }

        try
        {
            Response.Write("<br />开奖号码:" + GetNum());
        }
        catch
        {
            Response.Write("<br /><span style='color:red;font-size:50px'>开奖号码获取出现异常！</span><br/>");
        }

        try
        {
            Response.Write("<br />开奖时间:" + GetTime());
        }
        catch
        {
            Response.Write("<br /><span style='color:red;font-size:50px'>开奖时间获取出现异常！</span><br/>");
        }



        if (new BCW.HP3.BLL.HP3_kjnum().ExistsUpdateResult())
        {
            try
            {
                hp3opengame();
            }
            catch (Exception er)
            {
                Response.Write("<br/><span style='color:red;font-size:50px'>存入开奖号码出现异常！" + er + "</span><br/>");
            }
        }
        try
        {
            Winner();
            Response.Write("<br />ok1");
        }
        catch
        {
            // Response.Write("<br/><span style='color:red;font-size:50px'>自动获奖失败" + er + "</span><br/>error1");(Exception er)
            // new BCW.BLL.Guest().Add(1, 726, "将似沙", "" + GameName + "刷新机抓取出错了：" + er.ToString() + "");//内线提示出错测试用
        }
        try
        {
            Winner2();
        }
        catch
        {
            //  Response.Write("<span style='color:red;font-size:50px'>试用版自动获奖失败，请手动获奖</span>");
        }


        #region 大小单双赔率浮动
        try
        {
            ub xml = new ub();
            Application.Remove(xmlPath);//清缓存
            xml.ReloadSub(xmlPath); //加载配置
            double Odds = Utils.ParseDouble(ub.GetSub("HP3FUDONG", xmlPath));
            double oddschushi = Utils.ParseDouble(ub.GetSub("HP3GU", xmlPath));
            double oddsmax = Utils.ParseDouble(ub.GetSub("Oddsmax", xmlPath));
            double oddsmin = Utils.ParseDouble(ub.GetSub("Oddsmin", xmlPath));
            double oddschushi2 = oddschushi * 2;

            string strWheres = string.Empty;
            strWheres += " Fnum !='null' order by datenum desc";
            DataSet ds = new BCW.HP3.BLL.HP3_kjnum().GetList("Winum", strWheres);

            string[] result1 = ds.Tables[0].Rows[0]["Winum"].ToString().Split(',');
            int temp1 = 0;
            int sum1 = 0;
            string temps1 = string.Empty;
            for (int j = 0; j < result1.Length; j++)
            {
                temp1 = Convert.ToInt32(result1[4]);
                sum1 += temp1;
            }
            temps1 = Convert.ToString(sum1);
            temps1 = temps1.Substring(temps1.Length - 1, 1);
            temp1 = Convert.ToInt32(temps1);
            int count1 = 1;
            int count2 = 1;

            bool s1 = true;
            bool s2 = true;

            #region 大
            if (temp1 > 4)
            {
                while (s1)
                {
                    string[] Result = ds.Tables[0].Rows[count1]["Winum"].ToString().Split(',');
                    int temp = 0;
                    temp = Convert.ToInt32(Result[4]);
                    if (temp > 4)
                        count1++;
                    else
                        s1 = false;
                }
                if (count1 == 1)
                {

                    xml.dss["HP3DA"] = oddschushi;
                    xml.dss["HP3XIAO"] = oddschushi;
                }
                else
                {
                    if (oddschushi + Odds * (count1 - 1) > oddsmax || oddschushi + Odds * (count1 - 1) < oddsmin)
                    {
                        if (oddschushi + Odds * (count1 - 1) > oddsmax) { xml.dss["HP3DA"] = oddsmax; xml.dss["HP3XIAO"] = oddschushi2 - oddsmax; }
                        if (oddschushi + Odds * (count1 - 1) < oddsmin) { xml.dss["HP3DA"] = oddsmin; xml.dss["HP3XIAO"] = oddschushi2 - oddsmin; }
                    }
                    else
                    {
                        xml.dss["HP3DA"] = oddschushi + Odds * (count1 - 1);
                        xml.dss["HP3XIAO"] = oddschushi2 - (oddschushi + Odds * (count1 - 1));
                    }
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #region 小
            else
            {
                while (s1)
                {
                    string[] Result = ds.Tables[0].Rows[count1]["Winum"].ToString().Split(',');
                    int temp = 0;
                    temp = Convert.ToInt32(Result[4]);
                    if (temp > 4)
                        s1 = false;
                    else
                        count1++;
                }
                if (count1 == 1)
                {
                    xml.dss["HP3DA"] = oddschushi;
                    xml.dss["HP3XIAO"] = oddschushi;
                }
                else
                {
                    if (oddschushi + Odds * (count1 - 1) > oddsmax || oddschushi + Odds * (count1 - 1) < oddsmin)
                    {
                        if (oddschushi + Odds * (count1 - 1) > oddsmax) { xml.dss["HP3XIAO"] = oddsmax; xml.dss["HP3DA"] = oddschushi2 - oddsmax; }
                        if (oddschushi + Odds * (count1 - 1) < oddsmin) { xml.dss["HP3XIAO"] = oddsmin; xml.dss["HP3DA"] = oddschushi2 - oddsmin; }
                    }
                    else
                    {
                        xml.dss["HP3DA"] = oddschushi2 - (oddschushi + Odds * (count1 - 1));
                        xml.dss["HP3XIAO"] = oddschushi + Odds * (count1 - 1);
                    }
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #region 单
            if (sum1 % 2 != 0)
            {
                while (s2)
                {
                    string[] Result = ds.Tables[0].Rows[count2]["Winum"].ToString().Split(',');
                    int temp = 0;
                    temp = Convert.ToInt32(Result[4]);
                    if (temp % 2 != 0)
                        count2++;
                    else
                        s2 = false;
                }
                if (count2 == 1)
                {
                    xml.dss["HP3DAN"] = oddschushi;
                    xml.dss["HP3SHUANG"] = oddschushi;
                }
                else
                {
                    if (oddschushi + Odds * (count2 - 1) > oddsmax || oddschushi + Odds * (count2 - 1) < oddsmin)
                    {
                        if (oddschushi + Odds * (count2 - 1) > oddsmax) { xml.dss["HP3DAN"] = oddsmax; xml.dss["HP3SHUANG"] = oddschushi2 - oddsmax; }
                        if (oddschushi + Odds * (count2 - 1) < oddsmin) { xml.dss["HP3DAN"] = oddsmin; xml.dss["HP3SHUANG"] = oddschushi2 - oddsmin; }
                    }
                    else
                    {
                        xml.dss["HP3DAN"] = oddschushi + Odds * (count2 - 1);
                        xml.dss["HP3SHUANG"] = oddschushi2 - (oddschushi + Odds * (count2 - 1));
                    }
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #region 双
            else
            {
                while (s2)
                {
                    string[] Result = ds.Tables[0].Rows[count2]["Winum"].ToString().Split(',');
                    int temp = 0;
                    temp = Convert.ToInt32(Result[4]);
                    if (temp % 2 != 0)
                        s2 = false;
                    else
                        count2++;
                }
                if (count2 == 1)
                {
                    xml.dss["HP3DAN"] = oddschushi;
                    xml.dss["HP3SHUANG"] = oddschushi;
                }
                else
                {
                    if (oddschushi + Odds * (count2 - 1) > oddsmax || oddschushi + Odds * (count2 - 1) < oddsmin)
                    {
                        if (oddschushi + Odds * (count2 - 1) > oddsmax) { xml.dss["HP3SHUANG"] = oddsmax; xml.dss["HP3DAN"] = oddschushi2 - oddsmax; }
                        if (oddschushi + Odds * (count2 - 1) < oddsmin) { xml.dss["HP3SHUANG"] = oddsmin; xml.dss["HP3DAN"] = oddschushi2 - oddsmin; }
                    }
                    else
                    {
                        xml.dss["HP3DAN"] = oddschushi2 - (oddschushi + Odds * (count2 - 1));
                        xml.dss["HP3SHUANG"] = oddschushi + Odds * (count2 - 1);
                    }
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
        }
        catch { }
        #endregion

        Response.Write("<b>上次获取时间：</b>" + DateTime.Now + "<br />");
        stopwatch.Stop();
        Response.Write("<font color=\"black\">" + "总耗时:" + stopwatch.Elapsed.TotalSeconds + "秒");
    }

    //抓取网页数据
    public static string GetNewsUrl()
    {
        string strUrl = "http://baidu.lecai.com/lottery/draw/view/567/";
        string str = "";
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strUrl);
        request.Timeout = 30000;
        request.AllowAutoRedirect = false;
        request.KeepAlive = false;
        request.ProtocolVersion = HttpVersion.Version11;
        request.Headers["Accept-Language"] = "zh-cn";
        request.UserAgent = "mozilla/4.0 (compatible; msie 6.0; windows nt 5.1; sv1; .net clr 1.0.3705; .net clr 2.0.50727; .net clr 1.1.4322)";
        try
        {
            WebResponse response = request.GetResponse();
            System.IO.Stream stream = response.GetResponseStream();
            System.IO.StreamReader sr = new System.IO.StreamReader(stream, Encoding.GetEncoding("UTF-8"));
            str = sr.ReadToEnd();
            stream.Close();
            sr.Close();
        }
        catch (Exception e)
        {
            str = e.ToString().Replace("\n", "<BR>");
        }
        return str;

    }
    //得到最新开奖期数
    public static string GetStage()
    {
        String s = GetNewsUrl();
        String stage = @"latest_draw_phase = '[\d]{10}';";
        Match stages = Regex.Match(s, stage);
        String stageto = @"[\d]{10}";
        Match stageok = Regex.Match(stages.Value, stageto);
        Match stageok2 = Regex.Match(BatGet(), stageto);
        long state1 = Convert.ToInt64(stageok.Value);
        long state2;
        try
        {
            state2 = Convert.ToInt64(stageok2.Value);
        }
        catch
        {
            state2 = 2015122702;
        }
        if (state2 > state1)
        {
            state1 = state2;
        }
        if (stageok.Value != null)
        {
            return state1.ToString();
        }
        else
        {
            return "0";
        }
    }
    //得到最新开奖号码
    public static string GetNum()
    {
        String s = GetNewsUrl();
        String Num = @"latest_draw_result = [\D]{2}red[\D]{4}[\d]{3}[\D]{3}[\d]{3}[\D]{3}[\d]{3}";
        String stage = @"latest_draw_phase = '[\d]{10}';";
        Match stages = Regex.Match(s, stage);
        String stageto = @"[\d]{10}";
        Match stageok = Regex.Match(stages.Value, stageto);
        Match stageok2 = Regex.Match(BatGet(), stageto);
        long state1 = Convert.ToInt64(stageok.Value);
        long state2;
        try
        {
            state2 = Convert.ToInt64(stageok2.Value);
        }
        catch
        {
            state2 = 2015022939;
        }
        if (state2 > state1)
        {
            s = BatGet();
            if (BatGet().Trim() == "")
            {
                s = "106,213,409";
            }
            Num = @"[\d]{3}[\D]{1}[\d]{3}[\D]{1}[\d]{3}";
        }
        Match Nums = Regex.Match(s, Num);
        string Numto = @"[\d]{3}";
        MatchCollection NumOK = Regex.Matches(Nums.Value, Numto);
        try
        {
            string fnums = NumOK[0].Value;
            string snums = NumOK[1].Value;
            string tnums = NumOK[2].Value;
            if (fnums != null && snums != null && tnums != null)
            {
                MatchCollection fnum = Regex.Matches(fnums, @"[\d]{1}");
                MatchCollection snum = Regex.Matches(snums, @"[\d]{1}");
                MatchCollection tnum = Regex.Matches(tnums, @"[\d]{1}");
                string f1 = fnum[0].Value;
                switch (fnum[0].Value)
                {
                    case "1":
                        f1 = "黑桃";
                        break;
                    case "2":
                        f1 = "红桃";
                        break;
                    case "3":
                        f1 = "梅花";
                        break;
                    case "4":
                        f1 = "方块";
                        break;
                }
                string s1 = snum[0].Value;
                switch (snum[0].Value)
                {
                    case "1":
                        s1 = "黑桃";
                        break;
                    case "2":
                        s1 = "红桃";
                        break;
                    case "3":
                        s1 = "梅花";
                        break;
                    case "4":
                        s1 = "方块";
                        break;
                }
                string t1 = tnum[0].Value;
                switch (tnum[0].Value)
                {
                    case "1":
                        t1 = "黑桃";
                        break;
                    case "2":
                        t1 = "红桃";
                        break;
                    case "3":
                        t1 = "梅花";
                        break;
                    case "4":
                        t1 = "方块";
                        break;
                }
                string f = fnum[2].Value;
                if (fnum[1].Value == "0" && fnum[2].Value == "1")
                {
                    f = "A";
                }
                else if (fnum[1].Value == "1" && fnum[2].Value == "0")
                {
                    f = "10";
                }
                else if (fnum[1].Value == "1" && fnum[2].Value == "1")
                {
                    f = "J";
                }
                else if (fnum[1].Value == "1" && fnum[2].Value == "2")
                {
                    f = "Q";
                }
                else if (fnum[1].Value == "1" && fnum[2].Value == "3")
                {
                    f = "K";
                }
                else
                    f = fnum[2].Value;
                {
                }
                string sn = snum[2].Value;
                if (snum[1].Value == "0" && snum[2].Value == "1")
                {
                    sn = "A";
                }
                else if (snum[1].Value == "1" && snum[2].Value == "0")
                {
                    sn = "10";
                }
                else if (snum[1].Value == "1" && snum[2].Value == "1")
                {
                    sn = "J";
                }
                else if (snum[1].Value == "1" && snum[2].Value == "2")
                {
                    sn = "Q";
                }
                else if (snum[1].Value == "1" && snum[2].Value == "3")
                {
                    sn = "K";
                }
                else
                {
                    sn = snum[2].Value;
                }
                string t = tnum[2].Value;
                if (tnum[1].Value == "0" && tnum[2].Value == "1")
                {
                    t = "A";
                }
                else if (tnum[1].Value == "1" && tnum[2].Value == "0")
                {
                    t = "10";
                }
                else if (tnum[1].Value == "1" && tnum[2].Value == "1")
                {
                    t = "J";
                }
                else if (tnum[1].Value == "1" && tnum[2].Value == "2")
                {
                    t = "Q";
                }
                else if (tnum[1].Value == "1" && tnum[2].Value == "3")
                {
                    t = "K";
                }
                else
                {
                    t = tnum[2].Value;
                }
                string numstring = f1 + f + "," + s1 + sn + "," + t1 + t;
                return numstring;
            }
            else
            {
                return "0";
            }
        }
        catch
        {
            return "0";
        }

    }
    //得到最新开奖时间
    public static string GetTime()
    {

        String s = GetNewsUrl();
        String timess = @"latest_draw_time = '[\d]{4}-[\d]{2}-[\d]{1,2} [\d]{2}:[\d]{2}:[\d]{2}';";
        Match times = Regex.Match(s, timess);
        String timeto = @"[\d]{4}-[\d]{2}-[\d]{1,2} [\d]{2}:[\d]{2}:[\d]{2}";
        Match timeok = Regex.Match(times.Value, timeto);
        Match timeok2 = Regex.Match(BatGet(), timeto);
        DateTime time1 = Convert.ToDateTime(timeok.Value);
        DateTime time2;
        try
        {
            time2 = Convert.ToDateTime(timeok2.Value);
        }
        catch
        {
            time2 = Convert.ToDateTime("2015-12-30 15:21:40");
        }

        if (time2 > time1)
        {
            return timeok2.Value;
        }
        else
        {
            return timeok.Value;
        }
    }
    //备用开奖期号号码时间
    public static string BatGet()
    {
        string strUrl = "http://c.apiplus.net/newly.do?token=5a8ebe52461354bc&code=sdklpk3&rows=1";
        string str = "";
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strUrl);
        request.Timeout = 30000;
        request.AllowAutoRedirect = false;
        request.KeepAlive = false;
        request.ProtocolVersion = HttpVersion.Version11;
        request.Headers["Accept-Language"] = "zh-cn";
        request.UserAgent = "mozilla/4.0 (compatible; msie 6.0; windows nt 5.1; sv1; .net clr 1.0.3705; .net clr 2.0.50727; .net clr 1.1.4322)";
        try
        {
            WebResponse response = request.GetResponse();
            System.IO.Stream stream = response.GetResponseStream();
            System.IO.StreamReader sr = new System.IO.StreamReader(stream, Encoding.GetEncoding("UTF-8"));
            str = sr.ReadToEnd();
            stream.Close();
            sr.Close();
        }
        catch (Exception e)
        {
            str = e.ToString().Replace("\n", "<BR>");
        }
        string str2 = @"row expect=[\D]{1}[\d]{10}[\D]{1} opencode=[\D]{1}[\d]{3}[\D]{1}[\d]{3}[\D]{1}[\d]{3}[\D]{1} opentime=[\D]{1}[\d]{4}-[\d]{2}-[\d]{1,2} [\d]{2}:[\d]{2}:[\d]{2}[\D]{1}";
        Match str3 = Regex.Match(str, str2);
        string str4 = str3.Value;
        if (str4.Trim() == "")
        {
            str4 = "row expect=2015122702 opencode=106,213,409 opentime=2015-12-27 09:11:40";
        }
        return str4;
    }
    // 更新期数
    public string UpdateState()
    {
        string OnTime = "08:50-22:00";
        if (OnTime != "")
        {
            if (Utils.IsRegex(OnTime, @"^[0-9]{2}:[0-9]{2}-[0-9]{2}:[0-9]{2}$"))
            {
                string[] temp = OnTime.Split("-".ToCharArray());
                DateTime dt1 = Convert.ToDateTime(temp[0]);
                DateTime dt2 = Convert.ToDateTime(temp[1]);
                if (DateTime.Now <= dt2 && DateTime.Now >= dt1)
                {
                    string dt3 = DateTime.Now.AddMinutes(5).Subtract(dt1).Duration().TotalMinutes.ToString();
                    decimal dt4 = Convert.ToDecimal(dt3);
                    int dt5 = Convert.ToInt32(dt4 / 10);
                    string dt6 = dt5.ToString();
                    if (dt6.Length == 1)
                    {
                        dt6 = "0" + dt6;
                    }
                    string state = DateTime.Now.ToString("yyyyMMdd") + dt6;
                    string datee = string.Empty;
                    datee = DateTime.ParseExact((("" + state.Substring(0, 8)) + " 08:50:00"), "yyyyMMdd HH:mm:ss", null).ToString("yyyy-MM-dd HH:mm:ss");
                    BCW.HP3.Model.HP3_kjnum model = new BCW.HP3.Model.HP3_kjnum();
                    model.datenum = state;
                    if (Convert.ToInt32(dt6) < 10 && Convert.ToInt32(dt6) >= 01)
                    {
                        model.datetime = Convert.ToDateTime(datee).AddMinutes(10 * (int.Parse(GetLastStr(state, 1)))).AddSeconds(0);
                    }
                    else
                    {
                        model.datetime = Convert.ToDateTime(datee).AddMinutes(10 * (int.Parse(GetLastStr(state, 2)))).AddSeconds(0);
                    }
                    model.Fnum = "null";
                    model.Snum = "null";
                    model.Tnum = "null";
                    model.Winum = "null";
                    bool s = new BCW.HP3.BLL.HP3_kjnum().Exists(model.datenum);
                    switch (s)
                    {
                        case false:
                            new BCW.HP3.BLL.HP3_kjnum().Add(model);
                            break;
                        case true:
                            break;
                    }

                    return state;
                }
                else if (DateTime.Now > dt2 && DateTime.Now < Convert.ToDateTime("" + DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59" + ""))
                {
                    BCW.HP3.Model.HP3_kjnum model = new BCW.HP3.Model.HP3_kjnum();
                    string state = DateTime.Now.AddDays(1).ToString("yyyyMMdd") + "01";
                    model.datenum = state;
                    model.datetime = Convert.ToDateTime("" + DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") + " 09:00:00" + "");
                    model.Fnum = "null";
                    model.Snum = "null";
                    model.Tnum = "null";
                    model.Winum = "null";
                    bool s = new BCW.HP3.BLL.HP3_kjnum().Exists(model.datenum);
                    switch (s)
                    {
                        case false:
                            new BCW.HP3.BLL.HP3_kjnum().Add(model);
                            break;
                        case true:
                            break;
                    }

                    return state;
                }
                else if (DateTime.Now > Convert.ToDateTime("" + DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00" + "") && DateTime.Now < dt1)
                {
                    BCW.HP3.Model.HP3_kjnum model = new BCW.HP3.Model.HP3_kjnum();
                    string state = DateTime.Now.ToString("yyyyMMdd") + "01";
                    model.datenum = state;
                    model.datetime = Convert.ToDateTime("" + DateTime.Now.ToString("yyyy-MM-dd") + " 09:00:00" + "");
                    model.Fnum = "null";
                    model.Snum = "null";
                    model.Tnum = "null";
                    model.Winum = "null";
                    bool s = new BCW.HP3.BLL.HP3_kjnum().Exists(model.datenum);
                    switch (s)
                    {
                        case false:
                            new BCW.HP3.BLL.HP3_kjnum().Add(model);
                            break;
                        case true:
                            break;
                    }

                    return state;
                }
            }
        }
        return "0";
    }

    //中奖代码
    public string zjdm(string getnum)
    {
        System.Text.StringBuilder dms = new System.Text.StringBuilder("");
        string dm = "0";
        string[] numoo = getnum.Split(',');
        try
        {
            string numo1 = numoo[0];
            string numo2 = numoo[1];
            string numo3 = numoo[2];
            Match sa1 = Regex.Match(numo1, @"[\u4e00-\u9fa5]{2}");
            Match sa2 = Regex.Match(numo2, @"[\u4e00-\u9fa5]{2}");
            Match sa3 = Regex.Match(numo3, @"[\u4e00-\u9fa5]{2}");
            Match san1 = Regex.Match(numo1, @"[A-Z\d]{1,2}");
            Match san2 = Regex.Match(numo2, @"[A-Z\d]{1,2}");
            Match san3 = Regex.Match(numo3, @"[A-Z\d]{1,2}");
            string s1 = sa1.Value;
            string s2 = sa2.Value;
            string s3 = sa3.Value;
            string sn1 = san1.Value;
            string sn2 = san2.Value;
            string sn3 = san3.Value;

            switch (sn1)
            {
                case "A":
                    sn1 = "1";
                    break;
                case "J":
                    sn1 = "11";
                    break;
                case "Q":
                    sn1 = "12";
                    break;
                case "K":
                    sn1 = "13";
                    break;
            }
            switch (sn2)
            {
                case "A":
                    sn2 = "1";
                    break;
                case "J":
                    sn2 = "11";
                    break;
                case "Q":
                    sn2 = "12";
                    break;
                case "K":
                    sn2 = "13";
                    break;
            }
            switch (sn3)
            {
                case "A":
                    sn3 = "1";
                    break;
                case "J":
                    sn3 = "11";
                    break;
                case "Q":
                    sn3 = "12";
                    break;
                case "K":
                    sn3 = "13";
                    break;
            }
            int a = int.Parse(sn1);
            int b = int.Parse(sn2);
            int c = int.Parse(sn3);
            List<int> list = new List<int>();
            list.Add(a);
            list.Add(b);
            list.Add(c);
            list.Sort();
            int n1 = list[0];
            int n2 = list[1];
            int n3 = list[2];
            #region 特殊代码
            if (s1 == s2 && s2 == s3)
            {
                if (n2 == n1 + 1 && n3 == n2 + 1)
                {
                    switch (s1)
                    {
                        case "黑桃":
                            dm = "19";
                            break;
                        case "红桃":
                            dm = "20";
                            break;
                        case "梅花":
                            dm = "21";
                            break;
                        case "方块":
                            dm = "22";
                            break;
                    }
                    switch (n1)
                    {
                        case 1:
                            dm += "06";
                            break;
                        case 2:
                            dm += "07";
                            break;
                        case 3:
                            dm += "08";
                            break;
                        case 4:
                            dm += "09";
                            break;
                        case 5:
                            dm += "10";
                            break;
                        case 6:
                            dm += "11";
                            break;
                        case 7:
                            dm += "12";
                            break;
                        case 8:
                            dm += "13";
                            break;
                        case 9:
                            dm += "14";
                            break;
                        case 10:
                            dm += "15";
                            break;
                        case 11:
                            dm += "16";
                            break;
                    }
                }
                else if (n2 == 12 && n1 == 1 && n3 == 13)
                {
                    if (s1 == s2 && s2 == s3 && s1 == s3)
                    {
                        switch (s1)
                        {
                            case "黑桃":
                                dm = "1917";
                                break;
                            case "红桃":
                                dm = "2017";
                                break;
                            case "梅花":
                                dm = "2117";
                                break;
                            case "方块":
                                dm = "2217";
                                break;
                        }
                    }
                    else
                    {
                        dm = "17";
                    }
                }
                else
                {

                    switch (s1.Trim())
                    {
                        case "黑桃":
                            dm = "1";
                            break;
                        case "红桃":
                            dm = "2";
                            break;
                        case "梅花":
                            dm = "3";
                            break;
                        case "方块":
                            dm = "4";
                            break;
                    }
                }

            }
            else if (n2 == n1 + 1 && n3 == n2 + 1)
            {
                if (s1 == s2 && s2 == s3 && s1 == s3)
                {
                    switch (s1)
                    {
                        case "黑桃":
                            dm = "19";
                            break;
                        case "红桃":
                            dm = "20";
                            break;
                        case "梅花":
                            dm = "21";
                            break;
                        case "方块":
                            dm = "22";
                            break;
                    }
                    switch (n1)
                    {
                        case 1:
                            dm += "06";
                            break;
                        case 2:
                            dm += "07";
                            break;
                        case 3:
                            dm += "08";
                            break;
                        case 4:
                            dm += "09";
                            break;
                        case 5:
                            dm += "10";
                            break;
                        case 6:
                            dm += "11";
                            break;
                        case 7:
                            dm += "12";
                            break;
                        case 8:
                            dm += "13";
                            break;
                        case 9:
                            dm += "14";
                            break;
                        case 10:
                            dm += "15";
                            break;
                        case 11:
                            dm += "16";
                            break;
                    }
                }
                else
                {
                    switch (n1)
                    {
                        case 1:
                            dm = "6";
                            break;
                        case 2:
                            dm = "7";
                            break;
                        case 3:
                            dm = "8";
                            break;
                        case 4:
                            dm = "9";
                            break;
                        case 5:
                            dm = "10";
                            break;
                        case 6:
                            dm = "11";
                            break;
                        case 7:
                            dm = "12";
                            break;
                        case 8:
                            dm = "13";
                            break;
                        case 9:
                            dm = "14";
                            break;
                        case 10:
                            dm = "15";
                            break;
                        case 11:
                            dm = "16";
                            break;
                        case 12:
                            dm = "17";
                            break;
                    }
                }

            }
            else if (n2 == 12 && n1 == 1 && n3 == 13)
            {
                if (s1 == s2 && s2 == s3 && s1 == s3)
                {
                    switch (s1)
                    {
                        case "黑桃":
                            dm = "1917";
                            break;
                        case "红桃":
                            dm = "2017";
                            break;
                        case "梅花":
                            dm = "2117";
                            break;
                        case "方块":
                            dm = "2217";
                            break;
                    }
                }
                else
                {
                    dm = "17";
                }

            }
            else if (n1 == n2 && n2 == n3)
            {
                switch (n1)
                {
                    case 1:
                        dm = "24";
                        break;
                    case 2:
                        dm = "25";
                        break;
                    case 3:
                        dm = "26";
                        break;
                    case 4:
                        dm = "27";
                        break;
                    case 5:
                        dm = "28";
                        break;
                    case 6:
                        dm = "29";
                        break;
                    case 7:
                        dm = "30";
                        break;
                    case 8:
                        dm = "31";
                        break;
                    case 9:
                        dm = "32";
                        break;
                    case 10:
                        dm = "33";
                        break;
                    case 11:
                        dm = "34";
                        break;
                    case 12:
                        dm = "35";
                        break;
                    case 13:
                        dm = "36";
                        break;
                }
            }
            else if (n1 == n2 || n2 == n3 || n1 == n3)
            {
                int nnn = 0;
                if (n1 == n2 || n1 == n3)
                {
                    nnn = n1;
                }
                else
                {
                    nnn = n2;
                }
                switch (nnn)
                {
                    case 1:
                        dm = "38";
                        break;
                    case 2:
                        dm = "39";
                        break;
                    case 3:
                        dm = "40";
                        break;
                    case 4:
                        dm = "41";
                        break;
                    case 5:
                        dm = "42";
                        break;
                    case 6:
                        dm = "43";
                        break;
                    case 7:
                        dm = "44";
                        break;
                    case 8:
                        dm = "45";
                        break;
                    case 9:
                        dm = "46";
                        break;
                    case 10:
                        dm = "47";
                        break;
                    case 11:
                        dm = "48";
                        break;
                    case 12:
                        dm = "49";
                        break;
                    case 13:
                        dm = "50";
                        break;
                }

            }
            #endregion
            int a1 = Convert.ToInt32(sn1);
            int a2 = Convert.ToInt32(sn2);
            int a3 = Convert.ToInt32(sn3);
            if (a1 == 10 || a1 == 11 || a1 == 12 || a1 == 13)
            {
                a1 = 0;
            }
            if (a2 == 10 || a2 == 11 || a2 == 12 || a2 == 13)
            {
                a2 = 0;
            }
            if (a3 == 10 || a3 == 11 || a3 == 12 || a3 == 13)
            {
                a3 = 0;
            }
            int sumsum = a1 + a2 + a3;
            sumsum = sumsum % 10;
            dms.Append(dm + "," + san1 + "," + san2 + "," + san3 + "," + sumsum);
            return dms.ToString();
        }
        catch
        {
            return "null";
        }

    }
    //更新数据库中开奖结果
    public void HP3kj_num()
    {

        BCW.HP3.Model.HP3_kjnum model = new BCW.HP3.Model.HP3_kjnum();
        string getnum = GetNum();
        string getdate = GetStage();
        try
        {
            string[] numoo = getnum.Split(',');
            model.datenum = getdate;
            model.Fnum = numoo[0];
            model.Snum = numoo[1];
            model.Tnum = numoo[2];
        }
        catch
        {
            model.datenum = GetStage();
            model.Fnum = "null";
            model.Snum = "null";
            model.Tnum = "null";
        }
        if (new BCW.HP3.BLL.HP3_kjnum().Exists(getdate.Trim()))
        {
            BCW.HP3.Model.HP3_kjnum mpp = new BCW.HP3.BLL.HP3_kjnum().GetDataByState(getdate.Trim());
            try
            {
                DateTime dt1 = Convert.ToDateTime(GetTime());
                model.datetime = dt1;
                if (mpp.datetime > dt1)
                {
                    model.datetime = mpp.datetime;
                }
            }
            catch
            {
                model.datetime = mpp.datetime;
            }
        }
        else
        {
            DateTime dt1 = Convert.ToDateTime(GetTime());
            model.datetime = dt1;
        }

        string opo = zjdm(getnum);
        model.Winum = opo;
        if (new BCW.HP3.DAL.HP3_kjnum().Exists(model.datenum))
        {
            new BCW.HP3.BLL.HP3_kjnum().Update(model);
        }
        else
        {
            new BCW.HP3.BLL.HP3_kjnum().Add(model);
        }
    }
    //获奖判断
    public void Winner()
    {
        double DwMon = 0;
        int WinZhu = 0;
        //取开奖号码表中数据
        BCW.HP3.Model.HP3_kjnum kj = new BCW.HP3.Model.HP3_kjnum();
        kj = new BCW.HP3.BLL.HP3_kjnum().GetListLastNull();
        string state = kj.datenum;
        string winum = kj.Winum.Trim();
        string[] wnum = winum.Split(',');
        int EndSum = Convert.ToInt32(wnum[4]);

        //取用户订单数据
        DataSet ds = new BCW.HP3.BLL.HP3Buy().GetList("*", "BuyDate=" + state);
        BCW.HP3.Model.HP3Buy model = new BCW.HP3.Model.HP3Buy();
        int n = ds.Tables[0].Rows.Count - 1;
        Response.Write("<br />本期共" + ds.Tables[0].Rows.Count + "条购彩记录<br />");
        for (; n >= 0; n--)
        {
            //Response.Write(n+1+".");
            model.ID = Convert.ToInt32(ds.Tables[0].Rows[n][0]);
            model.BuyID = Convert.ToInt32(ds.Tables[0].Rows[n][1]);
            model.BuyDate = Convert.ToString(ds.Tables[0].Rows[n][2]);
            model.BuyType = Convert.ToInt32(ds.Tables[0].Rows[n][3]);
            model.BuyNum = Convert.ToString(ds.Tables[0].Rows[n][4]);
            model.BuyMoney = Convert.ToInt64(ds.Tables[0].Rows[n][5]);
            model.BuyZhu = Convert.ToInt32(ds.Tables[0].Rows[n][6]);
            model.BuyTime = Convert.ToDateTime(ds.Tables[0].Rows[n][7]);
            model.Odds = Convert.ToDecimal(ds.Tables[0].Rows[n][8]);
            string IsRot = ds.Tables[0].Rows[n][10].ToString();
            //取获奖订单数据
            BCW.HP3.Model.HP3Winner modelWin = new BCW.HP3.Model.HP3Winner();
            modelWin.ID = model.ID;
            modelWin.WinDate = model.BuyDate;
            modelWin.WinUserID = model.BuyID;
            modelWin.WinBool = 1;
            bool isis = new BCW.HP3.BLL.HP3Winner().Exists(modelWin.ID);
            if (isis)
            {
                //Response.Write("获奖数据已存在!!!<br />");
            }
            else
            {
                //Response.Write("一条购奖数据成功判断!!!<br />");
                string mename = new BCW.BLL.User().GetUsName(model.BuyID);
                if (model.BuyType == 1)
                {

                    #region 特殊方法
                    DwMon = Convert.ToDouble(model.Odds);
                    if (model.BuyNum.Trim() == "1" || model.BuyNum.Trim() == "2" || model.BuyNum.Trim() == "3" || model.BuyNum.Trim() == "4")
                    {
                        if (wnum[0].Trim() == model.BuyNum)
                        {
                            WinZhu = 1;
                            modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                        }
                        if (wnum[0].Length > 2)
                        {

                            if (wnum[0].Substring(0, 2).Trim() == "19" && model.BuyNum.Trim() == "1")
                            {
                                WinZhu = 1;
                                modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            }
                            if (wnum[0].Substring(0, 2).Trim() == "20" && model.BuyNum.Trim() == "2")
                            {
                                WinZhu = 1;
                                modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            }
                            if (wnum[0].Substring(0, 2).Trim() == "21" && model.BuyNum.Trim() == "3")
                            {
                                WinZhu = 1;
                                modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            }
                            if (wnum[0].Substring(0, 2).Trim() == "22" && model.BuyNum.Trim() == "4")
                            {
                                WinZhu = 1;
                                modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            }
                        }

                    }
                    else if (model.BuyNum.Trim() == "5")
                    {
                        if (wnum[0].Trim() == "1" || wnum[0].Trim() == "2" || wnum[0].Trim() == "3" || wnum[0].Trim() == "4")
                        {
                            WinZhu = 1;
                            modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                        }
                        if (wnum[0].Length > 2)
                        {

                            if (wnum[0].Substring(0, 2).Trim() == "19" || wnum[0].Substring(0, 2).Trim() == "20" || wnum[0].Substring(0, 2).Trim() == "21" || wnum[0].Substring(0, 2).Trim() == "22")
                            {
                                WinZhu = 1;
                                modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            }

                        }
                    }
                    else if (model.BuyNum == "6" || model.BuyNum == "7" || model.BuyNum == "8" || model.BuyNum == "9" || model.BuyNum == "10" || model.BuyNum == "11" || model.BuyNum == "12" || model.BuyNum == "13" || model.BuyNum == "14" || model.BuyNum == "15" || model.BuyNum == "16" || model.BuyNum == "17")
                    {
                        if (wnum[0].Trim() == model.BuyNum)
                        {
                            WinZhu = 1;
                            modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                        }
                        if (wnum[0].Length > 2)
                        {

                            if (wnum[0].Substring(2, 2) == model.BuyNum)
                            {
                                WinZhu = 1;
                                modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            }
                            if (wnum[0].Substring(2, 2) == "06" && model.BuyNum == "6")
                            {
                                WinZhu = 1;
                                modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            }
                            if (wnum[0].Substring(2, 2) == "07" && model.BuyNum == "7")
                            {
                                WinZhu = 1;
                                modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            }
                            if (wnum[0].Substring(2, 2) == "08" && model.BuyNum == "8")
                            {
                                WinZhu = 1;
                                modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            }
                            if (wnum[0].Substring(2, 2) == "09" && model.BuyNum == "9")
                            {
                                WinZhu = 1;
                                modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            }
                        }
                    }
                    else if (model.BuyNum == "18")
                    {
                        if (wnum[0].Trim() == "6" || wnum[0].Trim() == "7" || wnum[0].Trim() == "8" || wnum[0].Trim() == "9" || wnum[0].Trim() == "10" || wnum[0].Trim() == "11" || wnum[0].Trim() == "12" || wnum[0].Trim() == "13" || wnum[0].Trim() == "14" || wnum[0].Trim() == "15" || wnum[0].Trim() == "16" || wnum[0].Trim() == "17")
                        {
                            WinZhu = 1;
                            modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                        }
                        if (wnum[0].Length > 2)
                        {
                            if (wnum[0].Substring(2, 2).Trim() == "06" || wnum[0].Substring(2, 2).Trim() == "07" || wnum[0].Substring(2, 2).Trim() == "08" || wnum[0].Substring(2, 2).Trim() == "09" || wnum[0].Substring(2, 2).Trim() == "10" || wnum[0].Substring(2, 2).Trim() == "11" || wnum[0].Substring(2, 2).Trim() == "12" || wnum[0].Substring(2, 2).Trim() == "13" || wnum[0].Substring(2, 2).Trim() == "14" || wnum[0].Substring(2, 2).Trim() == "15" || wnum[0].Substring(2, 2).Trim() == "16" || wnum[0].Substring(2, 2).Trim() == "17")
                            {
                                WinZhu = 1;
                                modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            }
                        }
                    }
                    else if (model.BuyNum == "19" || model.BuyNum == "20" || model.BuyNum == "21" || model.BuyNum == "22")
                    {
                        if (wnum[0].Length > 2)
                        {
                            if (wnum[0].Substring(0, 2).Trim() == model.BuyNum)
                            {
                                WinZhu = 1;
                                modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            }
                        }
                    }
                    else if (model.BuyNum == "23")
                    {
                        if (wnum[0].Length > 2)
                        {
                            if (wnum[0].Substring(0, 2).Trim() == "19" || wnum[0].Substring(0, 2).Trim() == "20" || wnum[0].Substring(0, 2).Trim() == "21" || wnum[0].Substring(0, 2).Trim() == "22")
                            {
                                WinZhu = 1;
                                modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            }
                        }
                    }
                    else if (model.BuyNum == "24" || model.BuyNum == "25" || model.BuyNum == "26" || model.BuyNum == "27" || model.BuyNum == "28" || model.BuyNum == "29" || model.BuyNum == "30" || model.BuyNum == "31" || model.BuyNum == "32" || model.BuyNum == "33" || model.BuyNum == "34" || model.BuyNum == "35" || model.BuyNum == "36")
                    {
                        if (wnum[0].Trim() == model.BuyNum)
                        {
                            WinZhu = 1;
                            modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                        }
                    }
                    else if (model.BuyNum == "37")
                    {
                        if (wnum[0].Trim() == "24" || wnum[0].Trim() == "25" || wnum[0].Trim() == "26" || wnum[0].Trim() == "27" || wnum[0].Trim() == "28" || wnum[0].Trim() == "29" || wnum[0].Trim() == "30" || wnum[0].Trim() == "31" || wnum[0].Trim() == "32" || wnum[0].Trim() == "33" || wnum[0].Trim() == "34" || wnum[0].Trim() == "35" || wnum[0].Trim() == "36")
                        {
                            WinZhu = 1;
                            modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                        }
                    }
                    else if (model.BuyNum == "38" || model.BuyNum == "39" || model.BuyNum == "40" || model.BuyNum == "41" || model.BuyNum == "42" || model.BuyNum == "43" || model.BuyNum == "44" || model.BuyNum == "45" || model.BuyNum == "46" || model.BuyNum == "47" || model.BuyNum == "48" || model.BuyNum == "49" || model.BuyNum == "50")
                    {
                        if (wnum[0].Trim() == model.BuyNum)
                        {
                            WinZhu = 1;
                            modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                        }
                    }
                    else if (model.BuyNum == "51")
                    {
                        if (wnum[0].Trim() == "38" || wnum[0].Trim() == "39" || wnum[0].Trim() == "40" || wnum[0].Trim() == "41" || wnum[0].Trim() == "42" || wnum[0].Trim() == "43" || wnum[0].Trim() == "44" || wnum[0].Trim() == "45" || wnum[0].Trim() == "46" || wnum[0].Trim() == "47" || wnum[0].Trim() == "48" || wnum[0].Trim() == "49" || wnum[0].Trim() == "50")
                        {
                            WinZhu = 1;
                            modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                        }
                    }
                    #endregion
                    if (WinZhu != 0)
                    {
                        string gameplay = "";
                        switch (model.BuyType)
                        {
                            case 1:
                                gameplay = "花色连号同号投注";
                                break;
                            case 17:
                                gameplay = "大小单双投注";
                                break;
                            default:
                                gameplay = "任选投注";
                                break;
                        }
                        int id = new BCW.HP3.BLL.HP3Winner().Add(modelWin);
                        if (IsRot.Trim() != "1")
                            new BCW.BLL.Guest().Add(1, modelWin.WinUserID, mename, "您的快乐扑克3:" + modelWin.WinDate + "期已经开奖，获得了" + modelWin.WinMoney + "酷币。[url=/bbs/game/HP3.aspx?act=case]马上兑奖[/url]");
                        //动态
                        string wText = "在[url=/bbs/game/HP3.aspx]快乐扑克3[/url]《" + gameplay + "》狂赚" + modelWin.WinMoney + "" + ub.Get("SiteBz") + "";
                        new BCW.BLL.Action().Add(25, id, modelWin.WinUserID, mename, wText);
                        //将该订单ID插入HP3Winner
                        new BCW.HP3.BLL.HP3Winner().UpdateWinZhu(model.ID, WinZhu);
                    }
                    WinZhu = 0;
                }
                else if (model.BuyType == 17)//大小单双
                {
                    #region 大小单双
                    if (wnum[1] == wnum[2] && wnum[1] == wnum[3])
                    {
                    }
                    else
                    {
                        string big_small = "";
                        string single_double = "";
                        if (EndSum >= 5)
                        {
                            big_small = "1";
                        }
                        else
                        {
                            big_small = "2";
                        }
                        if (EndSum % 2 != 0)
                        {
                            single_double = "3";
                        }
                        else
                        {
                            single_double = "4";
                        }
                        if (model.BuyNum == big_small || model.BuyNum == single_double)
                        {
                            int xmlnum = 20 + Convert.ToInt32(model.BuyNum);
                            DwMon = Convert.ToDouble(model.Odds);
                            WinZhu = 1;
                            modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            int id = new BCW.HP3.BLL.HP3Winner().Add(modelWin);
                            if (IsRot.Trim() != "1")
                                new BCW.BLL.Guest().Add(1, modelWin.WinUserID, mename, "您的快乐扑克3:" + modelWin.WinDate + "期已经开奖，获得了" + modelWin.WinMoney + "酷币。[url=/bbs/game/HP3.aspx?act=case]马上兑奖[/url]");
                            string gameplay = "";
                            switch (model.BuyType)
                            {
                                case 1:
                                    gameplay = "花色连号同号投注";
                                    break;
                                case 17:
                                    gameplay = "大小单双投注";
                                    break;
                                default:
                                    gameplay = "任选投注";
                                    break;
                            }
                            //动态
                            string wText = "在[url=/bbs/game/HP3.aspx]快乐扑克3[/url]《" + gameplay + "》狂赚" + modelWin.WinMoney + "" + ub.Get("SiteBz") + "";
                            new BCW.BLL.Action().Add(25, id, modelWin.WinUserID, mename, wText);

                            //将该订单ID插入HP3Winner
                            new BCW.HP3.BLL.HP3Winner().UpdateWinZhu(model.ID, WinZhu);
                            WinZhu = 0;
                        }
                    }
                    #endregion
                }
                else if (model.BuyType == 6)//直一
                {

                    #region 直一
                    if (model.BuyNum.Contains(wnum[1]) || model.BuyNum.Contains(wnum[2]) || model.BuyNum.Contains(wnum[3]))
                    {
                        #region MyRegion
                        WinZhu = 0;
                        DwMon = Convert.ToDouble(model.Odds);
                        if (wnum[0] == "24" || wnum[0] == "25" || wnum[0] == "26" || wnum[0] == "27" || wnum[0] == "28" || wnum[0] == "29" || wnum[0] == "30" || wnum[0] == "31" || wnum[0] == "32" || wnum[0] == "33" || wnum[0] == "34" || wnum[0] == "35" || wnum[0] == "36")
                        {
                            if (model.BuyNum.Contains(wnum[1]))
                            {
                                WinZhu = 1;
                            }
                        }
                        else if (wnum[0] == "38" || wnum[0] == "39" || wnum[0] == "40" || wnum[0] == "41" || wnum[0] == "42" || wnum[0] == "43" || wnum[0] == "44" || wnum[0] == "45" || wnum[0] == "46" || wnum[0] == "47" || wnum[0] == "48" || wnum[0] == "49" || wnum[0] == "50")
                        {
                            if (wnum[1] == wnum[2])
                            {
                                if (model.BuyNum.Contains(wnum[1]))
                                {
                                    WinZhu = WinZhu + 1;
                                }
                                if (model.BuyNum.Contains(wnum[3]))
                                {
                                    WinZhu = WinZhu + 1;
                                }
                            }
                            if (wnum[1] == wnum[3])
                            {
                                if (model.BuyNum.Contains(wnum[1]))
                                {
                                    WinZhu = WinZhu + 1;
                                }
                                if (model.BuyNum.Contains(wnum[2]))
                                {
                                    WinZhu = WinZhu + 1;
                                }
                            }
                            if (wnum[2] == wnum[3])
                            {
                                if (model.BuyNum.Contains(wnum[2]))
                                {
                                    WinZhu = WinZhu + 1;
                                }
                                if (model.BuyNum.Contains(wnum[1]))
                                {
                                    WinZhu = WinZhu + 1;
                                }
                            }
                        }
                        else
                        {
                            if (model.BuyNum.Contains(wnum[1]))
                            {
                                WinZhu = WinZhu + 1;
                            }
                            if (model.BuyNum.Contains(wnum[2]))
                            {
                                WinZhu = WinZhu + 1;
                            }
                            if (model.BuyNum.Contains(wnum[3]))
                            {
                                WinZhu = WinZhu + 1;
                            }
                        }
                        #endregion
                    }
                    #endregion
                    if (WinZhu != 0)
                    {
                        modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                        int id = new BCW.HP3.BLL.HP3Winner().Add(modelWin);
                        if (IsRot.Trim() != "1")
                            new BCW.BLL.Guest().Add(1, modelWin.WinUserID, mename, "您的快乐扑克3:" + modelWin.WinDate + "期已经开奖，获得了" + modelWin.WinMoney + "酷币。[url=/bbs/game/HP3.aspx?act=case]马上兑奖[/url]");
                        string gameplay = "";
                        switch (model.BuyType)
                        {
                            case 1:
                                gameplay = "花色连号同号投注";
                                break;
                            case 17:
                                gameplay = "大小单双投注";
                                break;
                            default:
                                gameplay = "任选投注";
                                break;
                        }
                        //动态
                        string wText = "在[url=/bbs/game/HP3.aspx]快乐扑克3[/url]《" + gameplay + "》狂赚" + modelWin.WinMoney + "" + ub.Get("SiteBz") + "";
                        new BCW.BLL.Action().Add(25, id, modelWin.WinUserID, mename, wText);
                        //将该订单ID插入HP3Winner
                        new BCW.HP3.BLL.HP3Winner().UpdateWinZhu(model.ID, WinZhu);
                        WinZhu = 0;
                    }

                }
                else if (model.BuyType == 7)//直二
                {
                    #region 直二
                    WinZhu = 0;
                    DwMon = Convert.ToDouble(model.Odds);
                    if (wnum[0] == "24" || wnum[0] == "25" || wnum[0] == "26" || wnum[0] == "27" || wnum[0] == "28" || wnum[0] == "29" || wnum[0] == "30" || wnum[0] == "31" || wnum[0] == "32" || wnum[0] == "33" || wnum[0] == "34" || wnum[0] == "35" || wnum[0] == "36")
                    {
                        WinZhu = 0;
                    }
                    else if (wnum[0] == "38" || wnum[0] == "39" || wnum[0] == "40" || wnum[0] == "41" || wnum[0] == "42" || wnum[0] == "43" || wnum[0] == "44" || wnum[0] == "45" || wnum[0] == "46" || wnum[0] == "47" || wnum[0] == "48" || wnum[0] == "49" || wnum[0] == "50")
                    {
                        if (model.BuyNum.Contains(wnum[1]) && model.BuyNum.Contains(wnum[2]) && model.BuyNum.Contains(wnum[3]))
                        {
                            WinZhu = 1;
                        }
                    }
                    else
                    {
                        if (model.BuyNum.Contains(wnum[1]) && model.BuyNum.Contains(wnum[2]))
                        {
                            WinZhu = WinZhu + 1;
                        }
                        if (model.BuyNum.Contains(wnum[2]) && model.BuyNum.Contains(wnum[3]))
                        {
                            WinZhu = WinZhu + 1;
                        }
                        if (model.BuyNum.Contains(wnum[1]) && model.BuyNum.Contains(wnum[3]))
                        {
                            WinZhu = WinZhu + 1;
                        }
                    }
                    #endregion
                    if (WinZhu != 0)
                    {
                        //将该订单ID插入HP3Winner
                        modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                        int id = new BCW.HP3.BLL.HP3Winner().Add(modelWin);
                        if (IsRot.Trim() != "1")
                            new BCW.BLL.Guest().Add(1, modelWin.WinUserID, mename, "您的快乐扑克3:" + modelWin.WinDate + "期已经开奖，获得了" + modelWin.WinMoney + "酷币。[url=/bbs/game/HP3.aspx?act=case]马上兑奖[/url]");
                        string gameplay = "";
                        switch (model.BuyType)
                        {
                            case 1:
                                gameplay = "花色连号同号投注";
                                break;
                            case 17:
                                gameplay = "大小单双投注";
                                break;
                            default:
                                gameplay = "任选投注";
                                break;
                        }
                        //动态
                        string wText = "在[url=/bbs/game/HP3.aspx]快乐扑克3[/url]《" + gameplay + "》狂赚" + modelWin.WinMoney + "" + ub.Get("SiteBz") + "";
                        new BCW.BLL.Action().Add(25, id, modelWin.WinUserID, mename, wText);
                        //将该订单ID插入HP3Winner
                        new BCW.HP3.BLL.HP3Winner().UpdateWinZhu(model.ID, WinZhu);
                        WinZhu = 0;
                    }

                }

                else if (model.BuyType == 9 || model.BuyType == 11 || model.BuyType == 13 || model.BuyType == 15)//直三、四、五、六
                {
                    #region 直三、四、五、六
                    WinZhu = 0;
                    DwMon = Convert.ToDouble(model.Odds);

                    string[] buynum = model.BuyNum.Split(',');
                    int zj_zs = 0; //统计中奖注数

                    string wnumk = wnum[1] + "," + wnum[2] + "," + wnum[3];
                    string[] winumk = wnumk.Split(',');
                    if (winumk[0] != winumk[1] && winumk[1] != winumk[2] && winumk[0] != winumk[2])
                    {
                        for (int fs = 0; fs < buynum.Length; fs++)
                            for (int p = 0; p < winumk.Length; p++)
                                if (string.Compare(buynum[fs], winumk[p]) == 0)
                                    zj_zs += 1;
                    }

                    if (model.BuyType == 9)
                    {
                        if (zj_zs == 3)
                        {
                            WinZhu = 1;
                        }
                        else WinZhu = 0;
                    }
                    else if (model.BuyType == 11)
                    {
                        if (zj_zs == 3)
                        {
                            WinZhu = C((buynum.Length - 3), 1);
                        }
                        else WinZhu = 0;
                    }
                    else if (model.BuyType == 13)
                    {
                        if (zj_zs == 3)
                        {
                            WinZhu = C((buynum.Length - 3), 2);
                        }
                        else WinZhu = 0;
                    }
                    else if (model.BuyType == 15)
                    {
                        if (zj_zs == 3)
                        {
                            WinZhu = C((buynum.Length - 3), 3);
                        }
                        else WinZhu = 0;
                    }

                    #endregion
                    if (WinZhu != 0)
                    {
                        modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                        int id = new BCW.HP3.BLL.HP3Winner().Add(modelWin);         //将该订单ID插入HP3Winner
                        if (IsRot.Trim() != "1")
                            new BCW.BLL.Guest().Add(1, modelWin.WinUserID, mename, "您的快乐扑克3:" + modelWin.WinDate + "期已经开奖，获得了" + modelWin.WinMoney + "酷币。[url=/bbs/game/HP3.aspx?act=case]马上兑奖[/url]");
                        string gameplay = "";
                        switch (model.BuyType)
                        {
                            case 1:
                                gameplay = "花色连号同号投注";
                                break;
                            case 17:
                                gameplay = "大小单双投注";
                                break;
                            default:
                                gameplay = "任选投注";
                                break;
                        }
                        //动态
                        string wText = "在[url=/bbs/game/HP3.aspx]快乐扑克3[/url]《" + gameplay + "》狂赚" + modelWin.WinMoney + "" + ub.Get("SiteBz") + "";
                        new BCW.BLL.Action().Add(25, id, modelWin.WinUserID, mename, wText);
                        //将该订单ID插入HP3Winner
                        new BCW.HP3.BLL.HP3Winner().UpdateWinZhu(model.ID, WinZhu);
                        WinZhu = 0;
                    }
                }
                else if (model.BuyType == 8)//胆二
                {
                    #region 胆二
                    WinZhu = 0;
                    DwMon = Convert.ToDouble(model.Odds);
                    string[] buynum = model.BuyNum.Split('#');
                    if (buynum[0].Contains(wnum[1]) || buynum[0].Contains(wnum[2]) || buynum[0].Contains(wnum[3]))
                    {
                        if (wnum[0] == "24" || wnum[0] == "25" || wnum[0] == "26" || wnum[0] == "27" || wnum[0] == "28" || wnum[0] == "29" || wnum[0] == "30" || wnum[0] == "31" || wnum[0] == "32" || wnum[0] == "33" || wnum[0] == "34" || wnum[0] == "35" || wnum[0] == "36")
                        {
                            WinZhu = 0;
                        }
                        else if (wnum[0] == "38" || wnum[0] == "39" || wnum[0] == "40" || wnum[0] == "41" || wnum[0] == "42" || wnum[0] == "43" || wnum[0] == "44" || wnum[0] == "45" || wnum[0] == "46" || wnum[0] == "47" || wnum[0] == "48" || wnum[0] == "49" || wnum[0] == "50")
                        {
                            if (buynum[1].Contains(wnum[1]) || buynum[1].Contains(wnum[2]) || buynum[1].Contains(wnum[3]))
                            {
                                WinZhu = 1;
                            }
                        }
                        else
                        {
                            if (buynum[1].Contains(wnum[1]))
                            {
                                WinZhu = WinZhu + 1;
                            }
                            if (buynum[1].Contains(wnum[2]))
                            {
                                WinZhu = WinZhu + 1;
                            }
                            if (buynum[1].Contains(wnum[3]))
                            {
                                WinZhu = WinZhu + 1;
                            }
                        }
                    }
                    #endregion
                    if (WinZhu != 0)
                    {
                        modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                        int id = new BCW.HP3.BLL.HP3Winner().Add(modelWin);
                        if (IsRot.Trim() != "1")
                            new BCW.BLL.Guest().Add(1, modelWin.WinUserID, mename, "您的快乐扑克3:" + modelWin.WinDate + "期已经开奖，获得了" + modelWin.WinMoney + "酷币。[url=/bbs/game/HP3.aspx?act=case]马上兑奖[/url]");
                        string gameplay = "";
                        switch (model.BuyType)
                        {
                            case 1:
                                gameplay = "花色连号同号投注";
                                break;
                            case 17:
                                gameplay = "大小单双投注";
                                break;
                            default:
                                gameplay = "任选投注";
                                break;
                        }
                        //动态
                        string wText = "在[url=/bbs/game/HP3.aspx]快乐扑克3[/url]《" + gameplay + "》狂赚" + modelWin.WinMoney + "" + ub.Get("SiteBz") + "";
                        new BCW.BLL.Action().Add(25, id, modelWin.WinUserID, mename, wText);
                        //将该订单ID插入HP3Winner
                        new BCW.HP3.BLL.HP3Winner().UpdateWinZhu(model.ID, WinZhu);
                        WinZhu = 0;
                    }
                }
                else if (model.BuyType == 10)//胆三
                {
                    #region 胆三
                    WinZhu = 0;
                    DwMon = Convert.ToDouble(model.Odds);
                    string[] buynum = model.BuyNum.Split('#');
                    MatchCollection dmnum = Regex.Matches(buynum[0], ",");
                    MatchCollection leyr = Regex.Matches(buynum[1], ",");
                    int dmn = dmnum.Count + 1;
                    int ele = leyr.Count + 1;
                    if (wnum[0] == "24" || wnum[0] == "25" || wnum[0] == "26" || wnum[0] == "27" || wnum[0] == "28" || wnum[0] == "29" || wnum[0] == "30" || wnum[0] == "31" || wnum[0] == "32" || wnum[0] == "33" || wnum[0] == "34" || wnum[0] == "35" || wnum[0] == "36")
                    {
                        WinZhu = 0;
                    }
                    else if (wnum[0] == "38" || wnum[0] == "39" || wnum[0] == "40" || wnum[0] == "41" || wnum[0] == "42" || wnum[0] == "43" || wnum[0] == "44" || wnum[0] == "45" || wnum[0] == "46" || wnum[0] == "47" || wnum[0] == "48" || wnum[0] == "49" || wnum[0] == "50")
                    {
                        WinZhu = 0;
                    }
                    else
                    {
                        if (model.BuyNum.Contains(wnum[1]) && model.BuyNum.Contains(wnum[2]) && model.BuyNum.Contains(wnum[3]))
                        {
                            if (dmn == 1)
                            {
                                if (buynum[0].Contains(wnum[1]) || buynum[0].Contains(wnum[2]) || buynum[0].Contains(wnum[3]))
                                {
                                    WinZhu = 1;
                                }
                            }
                            else if (dmn == 2)
                            {
                                if (buynum[0].Contains(wnum[1]) && buynum[0].Contains(wnum[2]))
                                {
                                    WinZhu = 1;
                                }
                                if (buynum[0].Contains(wnum[2]) && buynum[0].Contains(wnum[3]))
                                {
                                    WinZhu = 1;
                                }
                                if (buynum[0].Contains(wnum[1]) && buynum[0].Contains(wnum[3]))
                                {
                                    WinZhu = 1;
                                }
                            }
                        }
                    }
                    #endregion
                    if (WinZhu != 0)
                    {
                        modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                        int id = new BCW.HP3.BLL.HP3Winner().Add(modelWin);
                        if (IsRot.Trim() != "1")
                            new BCW.BLL.Guest().Add(1, modelWin.WinUserID, mename, "您的快乐扑克3:" + modelWin.WinDate + "期已经开奖，获得了" + modelWin.WinMoney + "酷币。[url=/bbs/game/HP3.aspx?act=case]马上兑奖[/url]");
                        string gameplay = "";
                        switch (model.BuyType)
                        {
                            case 1:
                                gameplay = "花色连号同号投注";
                                break;
                            case 17:
                                gameplay = "大小单双投注";
                                break;
                            default:
                                gameplay = "任选投注";
                                break;
                        }
                        //动态
                        string wText = "在[url=/bbs/game/HP3.aspx]快乐扑克3[/url]《" + gameplay + "》狂赚" + modelWin.WinMoney + "" + ub.Get("SiteBz") + "";
                        new BCW.BLL.Action().Add(25, id, modelWin.WinUserID, mename, wText);
                        //将该订单ID插入HP3Winner
                        new BCW.HP3.BLL.HP3Winner().UpdateWinZhu(model.ID, WinZhu);
                    }
                    WinZhu = 0;
                }
                else if (model.BuyType == 12 || model.BuyType == 14 || model.BuyType == 16)//胆四、五、六
                {
                    #region 胆四、五、六
                    WinZhu = 0;
                    if (wnum[0] == "24" || wnum[0] == "25" || wnum[0] == "26" || wnum[0] == "27" || wnum[0] == "28" || wnum[0] == "29" || wnum[0] == "30" || wnum[0] == "31" || wnum[0] == "32" || wnum[0] == "33" || wnum[0] == "34" || wnum[0] == "35" || wnum[0] == "36")
                    {
                        WinZhu = 0;
                    }
                    else if (wnum[0] == "38" || wnum[0] == "39" || wnum[0] == "40" || wnum[0] == "41" || wnum[0] == "42" || wnum[0] == "43" || wnum[0] == "44" || wnum[0] == "45" || wnum[0] == "46" || wnum[0] == "47" || wnum[0] == "48" || wnum[0] == "49" || wnum[0] == "50")
                    {
                        WinZhu = 0;
                    }
                    else
                    {
                        DwMon = Convert.ToDouble(model.Odds);
                        string[] buynum = model.BuyNum.Split('#');
                        MatchCollection dmnum = Regex.Matches(buynum[0], ",");
                        MatchCollection leyr = Regex.Matches(buynum[1], ",");
                        int dmn = dmnum.Count + 1;
                        int ele = leyr.Count + 1;
                        if (model.BuyNum.Contains(wnum[1]) && model.BuyNum.Contains(wnum[2]) && model.BuyNum.Contains(wnum[3]))
                        {
                            if (model.BuyType == 12)
                            {
                                #region 胆四
                                if (buynum[0].Contains(wnum[1]) && buynum[0].Contains(wnum[2]) && buynum[0].Contains(wnum[3]))
                                {
                                    WinZhu = ele;
                                }
                                else if (buynum[0].Contains(wnum[1]) && buynum[0].Contains(wnum[2]))
                                {
                                    if (dmn == 3)
                                    {
                                        WinZhu = 1;
                                    }
                                    else if (dmn == 2)
                                    {
                                        WinZhu = ele - 1;
                                    }

                                }
                                else if (buynum[0].Contains(wnum[2]) && buynum[0].Contains(wnum[3]))
                                {
                                    if (dmn == 3)
                                    {
                                        WinZhu = 1;
                                    }
                                    else if (dmn == 2)
                                    {
                                        WinZhu = ele - 1;
                                    }
                                }
                                else if (buynum[0].Contains(wnum[1]) && buynum[0].Contains(wnum[3]))
                                {
                                    if (dmn == 3)
                                    {
                                        WinZhu = 1;
                                    }
                                    else if (dmn == 2)
                                    {
                                        WinZhu = ele - 1;
                                    }
                                }
                                else if (buynum[0].Contains(wnum[1]) || buynum[0].Contains(wnum[2]) || buynum[0].Contains(wnum[3]))
                                {
                                    if (dmn == 2)
                                    {
                                        WinZhu = 1;
                                    }
                                    else if (dmn == 1)
                                    {
                                        WinZhu = ele - 2;
                                    }
                                }

                                #endregion
                            }
                            else if (model.BuyType == 14)
                            {
                                #region 胆五
                                if (buynum[0].Contains(wnum[1]) && buynum[0].Contains(wnum[2]) && buynum[0].Contains(wnum[3]))
                                {
                                    if (dmn == 4)
                                    {
                                        WinZhu = ele;
                                    }
                                    else if (dmn == 3)
                                    {
                                        WinZhu = ele * (ele - 1) / 2;
                                    }

                                }
                                else if (buynum[0].Contains(wnum[1]) && buynum[0].Contains(wnum[2]))
                                {
                                    if (dmn == 4)
                                    {
                                        WinZhu = 1;
                                    }
                                    else if (dmn == 3)
                                    {
                                        WinZhu = ele - 1;
                                    }
                                    else if (dmn == 2)
                                    {
                                        WinZhu = (ele - 1) * (ele - 2) / 2;
                                    }
                                }
                                else if (buynum[0].Contains(wnum[2]) && buynum[0].Contains(wnum[3]))
                                {
                                    if (dmn == 4)
                                    {
                                        WinZhu = 1;
                                    }
                                    else if (dmn == 3)
                                    {
                                        WinZhu = ele - 1;
                                    }
                                    else if (dmn == 2)
                                    {
                                        WinZhu = (ele - 1) * (ele - 2) / 2;
                                    }
                                }
                                else if (buynum[0].Contains(wnum[1]) && buynum[0].Contains(wnum[3]))
                                {
                                    if (dmn == 4)
                                    {
                                        WinZhu = 1;
                                    }
                                    else if (dmn == 3)
                                    {
                                        WinZhu = ele - 1;
                                    }
                                    else if (dmn == 2)
                                    {
                                        WinZhu = (ele - 1) * (ele - 2) / 2;
                                    }
                                }
                                else if (buynum[0].Contains(wnum[1]) || buynum[0].Contains(wnum[2]) || buynum[0].Contains(wnum[3]))
                                {
                                    if (dmn == 4)
                                    {
                                        WinZhu = 0;
                                    }
                                    else if (dmn == 3)
                                    {
                                        WinZhu = 1;
                                    }
                                    else if (dmn == 2)
                                    {
                                        WinZhu = ele - 2;
                                    }
                                    else if (dmn == 1)
                                    {
                                        WinZhu = (ele - 2) * (ele - 3) / 2;
                                    }
                                }
                                #endregion

                            }
                            else if (model.BuyType == 16)
                            {
                                #region 胆六
                                int s = 0;
                                if (buynum[0].Contains(wnum[1]) && buynum[0].Contains(wnum[2]) && buynum[0].Contains(wnum[3]))
                                {
                                    if (dmn == 5)
                                    {
                                        WinZhu = ele;
                                    }
                                    else if (dmn == 4)
                                    {
                                        WinZhu = ele * (ele - 1) / 2;
                                    }
                                    else if (dmn == 3)
                                    {
                                        WinZhu = ele * (ele - 1) * (ele - 2) / 6;
                                    }
                                }
                                else if (buynum[0].Contains(wnum[1]) && buynum[0].Contains(wnum[2]))
                                {
                                    if (dmn == 5)
                                    {
                                        WinZhu = 1;
                                    }
                                    else if (dmn == 4)
                                    {
                                        WinZhu = ele - 1;
                                    }
                                    else if (dmn == 3)
                                    {
                                        WinZhu = (ele - 1) * (ele - 2) / 2;
                                    }
                                    else if (dmn == 2)
                                    {
                                        WinZhu = (ele - 1) * (ele - 2) * (ele - 3) / 6;
                                    }

                                }
                                else if (buynum[0].Contains(wnum[2]) && buynum[0].Contains(wnum[3]))
                                {
                                    if (dmn == 5)
                                    {
                                        WinZhu = 1;
                                    }
                                    else if (dmn == 4)
                                    {
                                        WinZhu = ele - 1;
                                    }
                                    else if (dmn == 3)
                                    {
                                        WinZhu = (ele - 1) * (ele - 2) / 2;
                                    }
                                    else if (dmn == 2)
                                    {
                                        WinZhu = (ele - 1) * (ele - 2) * (ele - 3) / 6;
                                    }
                                }
                                else if (buynum[0].Contains(wnum[1]) && buynum[0].Contains(wnum[3]))
                                {
                                    if (dmn == 5)
                                    {
                                        WinZhu = 1;
                                    }
                                    else if (dmn == 4)
                                    {
                                        WinZhu = ele - 1;
                                    }
                                    else if (dmn == 3)
                                    {
                                        WinZhu = (ele - 1) * (ele - 2) / 2;
                                    }
                                    else if (dmn == 2)
                                    {
                                        WinZhu = (ele - 1) * (ele - 2) * (ele - 3) / 6;
                                    }
                                }
                                else if (buynum[0].Contains(wnum[1]) || buynum[0].Contains(wnum[2]) || buynum[0].Contains(wnum[3]))
                                {
                                    if (dmn == 4)
                                    {
                                        WinZhu = 1;
                                    }
                                    else if (dmn == 3)
                                    {
                                        WinZhu = ele - 2;
                                    }
                                    else if (dmn == 2)
                                    {
                                        WinZhu = (ele - 2) * (ele - 3) / 2;
                                    }
                                    else if (dmn == 1)
                                    {
                                        WinZhu = (ele - 2) * (ele - 3) * (ele - 4) / 6;
                                    }

                                }
                                #endregion
                            }
                        }
                    }
                    #endregion
                    if (WinZhu != 0)
                    {
                        modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                        int id = new BCW.HP3.BLL.HP3Winner().Add(modelWin);
                        if (IsRot.Trim() != "1")
                            new BCW.BLL.Guest().Add(1, modelWin.WinUserID, mename, "您的快乐扑克3:" + modelWin.WinDate + "期已经开奖，获得了" + modelWin.WinMoney + "酷币。[url=/bbs/game/HP3.aspx?act=case]马上兑奖[/url]");
                        string gameplay = "";
                        switch (model.BuyType)
                        {
                            case 1:
                                gameplay = "花色连号同号投注";
                                break;
                            case 17:
                                gameplay = "大小单双投注";
                                break;
                            default:
                                gameplay = "任选投注";
                                break;
                        }
                        //动态
                        string wText = "在[url=/bbs/game/HP3.aspx]快乐扑克3[/url]《" + gameplay + "》狂赚" + modelWin.WinMoney + "" + ub.Get("SiteBz") + "";
                        new BCW.BLL.Action().Add(25, id, modelWin.WinUserID, mename, wText);
                        //将该订单ID插入HP3Winner
                        new BCW.HP3.BLL.HP3Winner().UpdateWinZhu(model.ID, WinZhu);
                    }
                }
            }
            try
            {
                DataSet WillGets = new BCW.HP3.BLL.HP3Winner().GetLists(" ID=" + model.ID);
                long WillGetss = Convert.ToInt64(WillGets.Tables[0].Rows[0][3]);
                new BCW.HP3.BLL.HP3Buy().UpdateWillGet(model.ID, WillGetss);
            }
            catch
            {
            }

        }
    }

    //试用版获奖判断
    public void Winner2()
    {
        double DwMon = 0;
        int WinZhu = 0;
        //取开奖号码表中数据
        BCW.HP3.Model.HP3_kjnum kj = new BCW.HP3.Model.HP3_kjnum();
        kj = new BCW.HP3.BLL.HP3_kjnum().GetListLastNull();
        string state = kj.datenum;
        string winum = kj.Winum.Trim();
        string[] wnum = winum.Split(',');
        int EndSum = Convert.ToInt32(wnum[4]);

        //取用户订单数据
        DataSet ds = new BCW.HP3.BLL.HP3BuySY().GetList("*", "BuyDate=" + state);
        BCW.HP3.Model.HP3BuySY model = new BCW.HP3.Model.HP3BuySY();
        int n = ds.Tables[0].Rows.Count - 1;
        Response.Write("本期试用版共" + ds.Tables[0].Rows.Count + "条购彩记录<br />");
        for (; n >= 0; n--)
        {
            //Response.Write(n+1+".");
            model.ID = Convert.ToInt32(ds.Tables[0].Rows[n][0]);
            model.BuyID = Convert.ToInt32(ds.Tables[0].Rows[n][1]);
            model.BuyDate = Convert.ToString(ds.Tables[0].Rows[n][2]);
            model.BuyType = Convert.ToInt32(ds.Tables[0].Rows[n][3]);
            model.BuyNum = Convert.ToString(ds.Tables[0].Rows[n][4]);
            model.BuyMoney = Convert.ToInt64(ds.Tables[0].Rows[n][5]);
            model.BuyZhu = Convert.ToInt32(ds.Tables[0].Rows[n][6]);
            model.BuyTime = Convert.ToDateTime(ds.Tables[0].Rows[n][7]);
            model.Odds = Convert.ToDecimal(ds.Tables[0].Rows[n][8]);
            //取获奖订单数据
            BCW.HP3.Model.HP3WinnerSY modelWin = new BCW.HP3.Model.HP3WinnerSY();
            modelWin.ID = model.ID;
            modelWin.WinDate = model.BuyDate;
            modelWin.WinUserID = model.BuyID;
            modelWin.WinBool = 1;
            bool isis = new BCW.HP3.BLL.HP3WinnerSY().Exists(modelWin.ID);
            if (isis)
            {
                //Response.Write("获奖数据已存在!!!<br />");
            }
            else
            {
                //Response.Write("一条购奖数据成功判断!!!<br />");
                string mename = new BCW.BLL.User().GetUsName(model.BuyID);
                if (model.BuyType == 1)
                {
                    #region 特殊方法
                    DwMon = Convert.ToDouble(model.Odds);
                    if (model.BuyNum.Trim() == "1" || model.BuyNum.Trim() == "2" || model.BuyNum.Trim() == "3" || model.BuyNum.Trim() == "4")
                    {
                        if (wnum[0].Trim() == model.BuyNum)
                        {
                            WinZhu = 1;
                            modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                        }
                        if (wnum[0].Length > 2)
                        {

                            if (wnum[0].Substring(0, 2).Trim() == "19" && model.BuyNum.Trim() == "1")
                            {
                                WinZhu = 1;
                                modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            }
                            if (wnum[0].Substring(0, 2).Trim() == "20" && model.BuyNum.Trim() == "2")
                            {
                                WinZhu = 1;
                                modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            }
                            if (wnum[0].Substring(0, 2).Trim() == "21" && model.BuyNum.Trim() == "3")
                            {
                                WinZhu = 1;
                                modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            }
                            if (wnum[0].Substring(0, 2).Trim() == "22" && model.BuyNum.Trim() == "4")
                            {
                                WinZhu = 1;
                                modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            }
                        }

                    }
                    else if (model.BuyNum.Trim() == "5")
                    {
                        if (wnum[0].Trim() == "1" || wnum[0].Trim() == "2" || wnum[0].Trim() == "3" || wnum[0].Trim() == "4")
                        {
                            WinZhu = 1;
                            modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                        }
                        if (wnum[0].Length > 2)
                        {

                            if (wnum[0].Substring(0, 2).Trim() == "19" || wnum[0].Substring(0, 2).Trim() == "20" || wnum[0].Substring(0, 2).Trim() == "21" || wnum[0].Substring(0, 2).Trim() == "22")
                            {
                                WinZhu = 1;
                                modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            }

                        }
                    }
                    else if (model.BuyNum == "6" || model.BuyNum == "7" || model.BuyNum == "8" || model.BuyNum == "9" || model.BuyNum == "10" || model.BuyNum == "11" || model.BuyNum == "12" || model.BuyNum == "13" || model.BuyNum == "14" || model.BuyNum == "15" || model.BuyNum == "16" || model.BuyNum == "17")
                    {
                        if (wnum[0].Trim() == model.BuyNum)
                        {
                            WinZhu = 1;
                            modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                        }
                        if (wnum[0].Length > 2)
                        {

                            if (wnum[0].Substring(2, 2) == model.BuyNum)
                            {
                                WinZhu = 1;
                                modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            }
                            if (wnum[0].Substring(2, 2) == "06" && model.BuyNum == "06")
                            {
                                WinZhu = 1;
                                modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            }
                            if (wnum[0].Substring(2, 2) == "07" && model.BuyNum == "07")
                            {
                                WinZhu = 1;
                                modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            }
                            if (wnum[0].Substring(2, 2) == "08" && model.BuyNum == "08")
                            {
                                WinZhu = 1;
                                modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            }
                            if (wnum[0].Substring(2, 2) == "09" && model.BuyNum == "09")
                            {
                                WinZhu = 1;
                                modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            }
                        }
                    }
                    else if (model.BuyNum == "18")
                    {
                        if (wnum[0].Trim() == "6" || wnum[0].Trim() == "7" || wnum[0].Trim() == "8" || wnum[0].Trim() == "9" || wnum[0].Trim() == "10" || wnum[0].Trim() == "11" || wnum[0].Trim() == "12" || wnum[0].Trim() == "13" || wnum[0].Trim() == "14" || wnum[0].Trim() == "15" || wnum[0].Trim() == "16" || wnum[0].Trim() == "17")
                        {
                            WinZhu = 1;
                            modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                        }
                        if (wnum[0].Length > 2)
                        {
                            if (wnum[0].Substring(2, 2).Trim() == "06" || wnum[0].Substring(2, 2).Trim() == "07" || wnum[0].Substring(2, 2).Trim() == "08" || wnum[0].Substring(2, 2).Trim() == "09" || wnum[0].Substring(2, 2).Trim() == "10" || wnum[0].Substring(2, 2).Trim() == "11" || wnum[0].Substring(2, 2).Trim() == "12" || wnum[0].Substring(2, 2).Trim() == "13" || wnum[0].Substring(2, 2).Trim() == "14" || wnum[0].Substring(2, 2).Trim() == "15" || wnum[0].Substring(2, 2).Trim() == "16" || wnum[0].Substring(2, 2).Trim() == "17")
                            {
                                WinZhu = 1;
                                modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            }
                        }
                    }
                    else if (model.BuyNum == "19" || model.BuyNum == "20" || model.BuyNum == "21" || model.BuyNum == "22")
                    {
                        if (wnum[0].Length > 2)
                        {
                            if (wnum[0].Substring(0, 2).Trim() == model.BuyNum)
                            {
                                WinZhu = 1;
                                modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            }
                        }
                    }
                    else if (model.BuyNum == "23")
                    {
                        if (wnum[0].Length > 2)
                        {
                            if (wnum[0].Substring(0, 2).Trim() == "19" || wnum[0].Substring(0, 2).Trim() == "20" || wnum[0].Substring(0, 2).Trim() == "21" || wnum[0].Substring(0, 2).Trim() == "22")
                            {
                                WinZhu = 1;
                                modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            }
                        }
                    }
                    else if (model.BuyNum == "24" || model.BuyNum == "25" || model.BuyNum == "26" || model.BuyNum == "27" || model.BuyNum == "28" || model.BuyNum == "29" || model.BuyNum == "30" || model.BuyNum == "31" || model.BuyNum == "32" || model.BuyNum == "33" || model.BuyNum == "34" || model.BuyNum == "35" || model.BuyNum == "36")
                    {
                        if (wnum[0].Trim() == model.BuyNum)
                        {
                            WinZhu = 1;
                            modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                        }
                    }
                    else if (model.BuyNum == "37")
                    {
                        if (wnum[0].Trim() == "24" || wnum[0].Trim() == "25" || wnum[0].Trim() == "26" || wnum[0].Trim() == "27" || wnum[0].Trim() == "28" || wnum[0].Trim() == "29" || wnum[0].Trim() == "30" || wnum[0].Trim() == "31" || wnum[0].Trim() == "32" || wnum[0].Trim() == "33" || wnum[0].Trim() == "34" || wnum[0].Trim() == "35" || wnum[0].Trim() == "36")
                        {
                            WinZhu = 1;
                            modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                        }
                    }
                    else if (model.BuyNum == "38" || model.BuyNum == "39" || model.BuyNum == "40" || model.BuyNum == "41" || model.BuyNum == "42" || model.BuyNum == "43" || model.BuyNum == "44" || model.BuyNum == "45" || model.BuyNum == "46" || model.BuyNum == "47" || model.BuyNum == "48" || model.BuyNum == "49" || model.BuyNum == "50")
                    {
                        if (wnum[0].Trim() == model.BuyNum)
                        {
                            WinZhu = 1;
                            modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                        }
                    }
                    else if (model.BuyNum == "51")
                    {
                        if (wnum[0].Trim() == "38" || wnum[0].Trim() == "39" || wnum[0].Trim() == "40" || wnum[0].Trim() == "41" || wnum[0].Trim() == "42" || wnum[0].Trim() == "43" || wnum[0].Trim() == "44" || wnum[0].Trim() == "45" || wnum[0].Trim() == "46" || wnum[0].Trim() == "47" || wnum[0].Trim() == "48" || wnum[0].Trim() == "49" || wnum[0].Trim() == "50")
                        {
                            WinZhu = 1;
                            modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                        }
                    }
                    #endregion
                    int id = new BCW.HP3.BLL.HP3WinnerSY().Add(modelWin);
                    new BCW.BLL.Guest().Add(1, modelWin.WinUserID, mename, "您的快乐扑克3:" + modelWin.WinDate + "期已经开奖，获得了" + modelWin.WinMoney + "快乐币。[url=/bbs/game/HP3SW.aspx?act=case]马上兑奖[/url]");
                    string gameplay = "";
                    switch (model.BuyType)
                    {
                        case 1:
                            gameplay = "花色连号同号投注";
                            break;
                        case 17:
                            gameplay = "大小单双投注";
                            break;
                        default:
                            gameplay = "任选投注";
                            break;
                    }
                    if (WinZhu != 0)
                    {
                        //动态
                        string wText = "在[url=/bbs/game/HP3SW.aspx]快乐扑克3试玩版[/url]《" + gameplay + "》狂赚" + modelWin.WinMoney + "" + "快乐币" + "";
                        new BCW.BLL.Action().Add(25, id, modelWin.WinUserID, mename, wText);
                        //将该订单ID插入HP3WinnerSY
                        new BCW.HP3.BLL.HP3WinnerSY().UpdateWinZhu(model.ID, WinZhu);
                    }
                    WinZhu = 0;

                }
                else if (model.BuyType == 17)//大小单双
                {
                    #region 大小单双
                    if (wnum[1] == wnum[2] && wnum[1] == wnum[3])
                    {
                    }
                    else
                    {
                        string big_small = "";
                        string single_double = "";
                        if (EndSum >= 5)
                        {
                            big_small = "1";
                        }
                        else
                        {
                            big_small = "2";
                        }
                        if (EndSum % 2 != 0)
                        {
                            single_double = "3";
                        }
                        else
                        {
                            single_double = "4";
                        }
                        if (model.BuyNum == big_small || model.BuyNum == single_double)
                        {
                            int xmlnum = 20 + Convert.ToInt32(model.BuyNum);
                            DwMon = Convert.ToDouble(model.Odds);
                            WinZhu = 1;
                            modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            int id = new BCW.HP3.BLL.HP3WinnerSY().Add(modelWin);
                            new BCW.BLL.Guest().Add(1, modelWin.WinUserID, mename, "您的快乐扑克3:" + modelWin.WinDate + "期已经开奖，获得了" + modelWin.WinMoney + "快乐币。[url=/bbs/game/HP3SW.aspx?act=case]马上兑奖[/url]");
                            string gameplay = "";
                            switch (model.BuyType)
                            {
                                case 1:
                                    gameplay = "花色连号同号投注";
                                    break;
                                case 17:
                                    gameplay = "大小单双投注";
                                    break;
                                default:
                                    gameplay = "任选投注";
                                    break;
                            }
                            //动态
                            string wText = "在[url=/bbs/game/HP3SW.aspx]快乐扑克3试玩版[/url]《" + gameplay + "》狂赚" + modelWin.WinMoney + "" + "快乐币" + "";
                            new BCW.BLL.Action().Add(25, id, modelWin.WinUserID, mename, wText);

                            //将该订单ID插入HP3WinnerSY
                            new BCW.HP3.BLL.HP3WinnerSY().UpdateWinZhu(model.ID, WinZhu);
                            WinZhu = 0;
                        }
                    }
                    #endregion
                }
                else if (model.BuyType == 6)//直一
                {

                    #region 直一
                    if (model.BuyNum.Contains(wnum[1]) || model.BuyNum.Contains(wnum[2]) || model.BuyNum.Contains(wnum[3]))
                    {
                        #region MyRegion
                        WinZhu = 0;
                        DwMon = Convert.ToDouble(model.Odds);
                        if (wnum[0] == "24" || wnum[0] == "25" || wnum[0] == "26" || wnum[0] == "27" || wnum[0] == "28" || wnum[0] == "29" || wnum[0] == "30" || wnum[0] == "31" || wnum[0] == "32" || wnum[0] == "33" || wnum[0] == "34" || wnum[0] == "35" || wnum[0] == "36")
                        {
                            if (model.BuyNum.Contains(wnum[1]))
                            {
                                WinZhu = 1;
                            }
                        }
                        else if (wnum[0] == "38" || wnum[0] == "39" || wnum[0] == "40" || wnum[0] == "41" || wnum[0] == "42" || wnum[0] == "43" || wnum[0] == "44" || wnum[0] == "45" || wnum[0] == "46" || wnum[0] == "47" || wnum[0] == "48" || wnum[0] == "49" || wnum[0] == "50")
                        {
                            if (wnum[1] == wnum[2])
                            {
                                if (model.BuyNum.Contains(wnum[1]))
                                {
                                    WinZhu = WinZhu + 1;
                                }
                                if (model.BuyNum.Contains(wnum[3]))
                                {
                                    WinZhu = WinZhu + 1;
                                }
                            }
                            if (wnum[1] == wnum[3])
                            {
                                if (model.BuyNum.Contains(wnum[1]))
                                {
                                    WinZhu = WinZhu + 1;
                                }
                                if (model.BuyNum.Contains(wnum[2]))
                                {
                                    WinZhu = WinZhu + 1;
                                }
                            }
                            if (wnum[2] == wnum[3])
                            {
                                if (model.BuyNum.Contains(wnum[2]))
                                {
                                    WinZhu = WinZhu + 1;
                                }
                                if (model.BuyNum.Contains(wnum[1]))
                                {
                                    WinZhu = WinZhu + 1;
                                }
                            }
                        }
                        else
                        {
                            if (model.BuyNum.Contains(wnum[1]))
                            {
                                WinZhu = WinZhu + 1;
                            }
                            if (model.BuyNum.Contains(wnum[2]))
                            {
                                WinZhu = WinZhu + 1;
                            }
                            if (model.BuyNum.Contains(wnum[3]))
                            {
                                WinZhu = WinZhu + 1;
                            }
                        }
                        #endregion
                    }
                    #endregion
                    if (WinZhu != 0)
                    {
                        modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                        int id = new BCW.HP3.BLL.HP3WinnerSY().Add(modelWin);
                        new BCW.BLL.Guest().Add(1, modelWin.WinUserID, mename, "您的快乐扑克3:" + modelWin.WinDate + "期已经开奖，获得了" + modelWin.WinMoney + "快乐币。[url=/bbs/game/HP3SW.aspx?act=case]马上兑奖[/url]");
                        string gameplay = "";
                        switch (model.BuyType)
                        {
                            case 1:
                                gameplay = "花色连号同号投注";
                                break;
                            case 17:
                                gameplay = "大小单双投注";
                                break;
                            default:
                                gameplay = "任选投注";
                                break;
                        }
                        //动态
                        string wText = "在[url=/bbs/game/HP3SW.aspx]快乐扑克3试玩版[/url]《" + gameplay + "》狂赚" + modelWin.WinMoney + "" + "快乐币" + "";
                        new BCW.BLL.Action().Add(25, id, modelWin.WinUserID, mename, wText);
                        //将该订单ID插入HP3WinnerSY
                        new BCW.HP3.BLL.HP3WinnerSY().UpdateWinZhu(model.ID, WinZhu);
                        WinZhu = 0;
                    }
                }
                else if (model.BuyType == 7)//直二
                {
                    #region 直二
                    WinZhu = 0;
                    DwMon = Convert.ToDouble(model.Odds);
                    if (wnum[0] == "24" || wnum[0] == "25" || wnum[0] == "26" || wnum[0] == "27" || wnum[0] == "28" || wnum[0] == "29" || wnum[0] == "30" || wnum[0] == "31" || wnum[0] == "32" || wnum[0] == "33" || wnum[0] == "34" || wnum[0] == "35" || wnum[0] == "36")
                    {
                        WinZhu = 0;
                    }
                    else if (wnum[0] == "38" || wnum[0] == "39" || wnum[0] == "40" || wnum[0] == "41" || wnum[0] == "42" || wnum[0] == "43" || wnum[0] == "44" || wnum[0] == "45" || wnum[0] == "46" || wnum[0] == "47" || wnum[0] == "48" || wnum[0] == "49" || wnum[0] == "50")
                    {
                        if (model.BuyNum.Contains(wnum[1]) && model.BuyNum.Contains(wnum[2]) && model.BuyNum.Contains(wnum[3]))
                        {
                            WinZhu = 1;
                        }
                    }
                    else
                    {
                        if (model.BuyNum.Contains(wnum[1]) && model.BuyNum.Contains(wnum[2]))
                        {
                            WinZhu = WinZhu + 1;
                        }
                        if (model.BuyNum.Contains(wnum[2]) && model.BuyNum.Contains(wnum[3]))
                        {
                            WinZhu = WinZhu + 1;
                        }
                        if (model.BuyNum.Contains(wnum[1]) && model.BuyNum.Contains(wnum[3]))
                        {
                            WinZhu = WinZhu + 1;
                        }
                    }
                    #endregion
                    if (WinZhu != 0)
                    {
                        //将该订单ID插入HP3WinnerSY
                        modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                        int id = new BCW.HP3.BLL.HP3WinnerSY().Add(modelWin);
                        new BCW.BLL.Guest().Add(1, modelWin.WinUserID, mename, "您的快乐扑克3:" + modelWin.WinDate + "期已经开奖，获得了" + modelWin.WinMoney + "快乐币。[url=/bbs/game/HP3SW.aspx?act=case]马上兑奖[/url]");
                        string gameplay = "";
                        switch (model.BuyType)
                        {
                            case 1:
                                gameplay = "花色连号同号投注";
                                break;
                            case 17:
                                gameplay = "大小单双投注";
                                break;
                            default:
                                gameplay = "任选投注";
                                break;
                        }
                        //动态
                        string wText = "在[url=/bbs/game/HP3SW.aspx]快乐扑克3试玩版[/url]《" + gameplay + "》狂赚" + modelWin.WinMoney + "" + "快乐币" + "";
                        new BCW.BLL.Action().Add(25, id, modelWin.WinUserID, mename, wText);
                        new BCW.HP3.BLL.HP3WinnerSY().UpdateWinZhu(model.ID, WinZhu);
                        //将该订单ID插入HP3WinnerSY
                        WinZhu = 0;
                    }
                }

                else if (model.BuyType == 9 || model.BuyType == 11 || model.BuyType == 13 || model.BuyType == 15)//直三、四、五、六
                {
                    #region 直三、四、五、六
                    WinZhu = 0;
                    DwMon = Convert.ToDouble(model.Odds);
                    string[] buynum = model.BuyNum.Split(',');
                    int zj_zs = 0; //统计中奖注数

                    string wnumk = wnum[1] + "," + wnum[2] + "," + wnum[3];
                    string[] winumk = wnumk.Split(',');
                    if (winumk[0] != winumk[1] && winumk[1] != winumk[2] && winumk[0] != winumk[2])
                    {
                        for (int fs = 0; fs < buynum.Length; fs++)
                            for (int p = 0; p < winumk.Length; p++)
                                if (string.Compare(buynum[fs], winumk[p]) == 0)
                                    zj_zs += 1;
                    }

                    if (model.BuyType == 9)
                    {
                        if (zj_zs == 3)
                        {
                            WinZhu = 1;
                        }
                        else WinZhu = 0;
                    }
                    else if (model.BuyType == 11)
                    {
                        if (zj_zs == 3)
                        {
                            WinZhu = C((buynum.Length - 3), 1);
                        }
                        else WinZhu = 0;
                    }
                    else if (model.BuyType == 13)
                    {
                        if (zj_zs == 3)
                        {
                            zj_zs = C((buynum.Length - 3), 2);
                        }
                        else WinZhu = 0;
                    }
                    else if (model.BuyType == 15)
                    {
                        if (zj_zs == 3)
                        {
                            zj_zs = C((buynum.Length - 3), 3);
                        }
                        else WinZhu = 0;
                    }

                    #endregion
                    if (WinZhu != 0)
                    {
                        modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                        int id = new BCW.HP3.BLL.HP3WinnerSY().Add(modelWin);         //将该订单ID插入HP3WinnerSY
                        new BCW.BLL.Guest().Add(1, modelWin.WinUserID, mename, "您的快乐扑克3:" + modelWin.WinDate + "期已经开奖，获得了" + modelWin.WinMoney + "快乐币。[url=/bbs/game/HP3SW.aspx?act=case]马上兑奖[/url]");
                        string gameplay = "";
                        switch (model.BuyType)
                        {
                            case 1:
                                gameplay = "花色连号同号投注";
                                break;
                            case 17:
                                gameplay = "大小单双投注";
                                break;
                            default:
                                gameplay = "任选投注";
                                break;
                        }
                        //动态
                        string wText = "在[url=/bbs/game/HP3SW.aspx]快乐扑克3试玩版[/url]《" + gameplay + "》狂赚" + modelWin.WinMoney + "" + "快乐币" + "";
                        new BCW.BLL.Action().Add(25, id, modelWin.WinUserID, mename, wText);
                        new BCW.HP3.BLL.HP3WinnerSY().UpdateWinZhu(model.ID, WinZhu);
                        //将该订单ID插入HP3WinnerSY
                        WinZhu = 0;
                    }
                }
                else if (model.BuyType == 8)//胆二
                {
                    #region 胆二
                    WinZhu = 0;
                    DwMon = Convert.ToDouble(model.Odds);
                    string[] buynum = model.BuyNum.Split('#');
                    if (buynum[0].Contains(wnum[1]) || buynum[0].Contains(wnum[2]) || buynum[0].Contains(wnum[3]))
                    {
                        if (wnum[0] == "24" || wnum[0] == "25" || wnum[0] == "26" || wnum[0] == "27" || wnum[0] == "28" || wnum[0] == "29" || wnum[0] == "30" || wnum[0] == "31" || wnum[0] == "32" || wnum[0] == "33" || wnum[0] == "34" || wnum[0] == "35" || wnum[0] == "36")
                        {
                            WinZhu = 0;
                        }
                        else if (wnum[0] == "38" || wnum[0] == "39" || wnum[0] == "40" || wnum[0] == "41" || wnum[0] == "42" || wnum[0] == "43" || wnum[0] == "44" || wnum[0] == "45" || wnum[0] == "46" || wnum[0] == "47" || wnum[0] == "48" || wnum[0] == "49" || wnum[0] == "50")
                        {
                            if (buynum[1].Contains(wnum[1]) || buynum[1].Contains(wnum[2]) || buynum[1].Contains(wnum[3]))
                            {
                                WinZhu = 1;
                            }
                        }
                        else
                        {
                            if (buynum[1].Contains(wnum[1]))
                            {
                                WinZhu = WinZhu + 1;
                            }
                            if (buynum[1].Contains(wnum[2]))
                            {
                                WinZhu = WinZhu + 1;
                            }
                            if (buynum[1].Contains(wnum[3]))
                            {
                                WinZhu = WinZhu + 1;
                            }
                        }
                    }
                    #endregion
                    if (WinZhu != 0)
                    {
                        modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                        int id = new BCW.HP3.BLL.HP3WinnerSY().Add(modelWin);
                        new BCW.BLL.Guest().Add(1, modelWin.WinUserID, mename, "您的快乐扑克3:" + modelWin.WinDate + "期已经开奖，获得了" + modelWin.WinMoney + "快乐币。[url=/bbs/game/HP3SW.aspx?act=case]马上兑奖[/url]");
                        string gameplay = "";
                        switch (model.BuyType)
                        {
                            case 1:
                                gameplay = "花色连号同号投注";
                                break;
                            case 17:
                                gameplay = "大小单双投注";
                                break;
                            default:
                                gameplay = "任选投注";
                                break;
                        }
                        //动态
                        string wText = "在[url=/bbs/game/HP3SW.aspx]快乐扑克3试玩版[/url]《" + gameplay + "》狂赚" + modelWin.WinMoney + "" + "快乐币" + "";
                        new BCW.BLL.Action().Add(25, id, modelWin.WinUserID, mename, wText);
                        new BCW.HP3.BLL.HP3WinnerSY().UpdateWinZhu(model.ID, WinZhu);
                        //将该订单ID插入HP3WinnerSY
                        WinZhu = 0;
                    }

                }
                else if (model.BuyType == 10)//胆三
                {
                    #region 胆三
                    WinZhu = 0;
                    DwMon = Convert.ToDouble(model.Odds);
                    string[] buynum = model.BuyNum.Split('#');
                    MatchCollection dmnum = Regex.Matches(buynum[0], ",");
                    MatchCollection leyr = Regex.Matches(buynum[1], ",");
                    int dmn = dmnum.Count + 1;
                    int ele = leyr.Count + 1;
                    if (wnum[0] == "24" || wnum[0] == "25" || wnum[0] == "26" || wnum[0] == "27" || wnum[0] == "28" || wnum[0] == "29" || wnum[0] == "30" || wnum[0] == "31" || wnum[0] == "32" || wnum[0] == "33" || wnum[0] == "34" || wnum[0] == "35" || wnum[0] == "36")
                    {
                        WinZhu = 0;
                    }
                    else if (wnum[0] == "38" || wnum[0] == "39" || wnum[0] == "40" || wnum[0] == "41" || wnum[0] == "42" || wnum[0] == "43" || wnum[0] == "44" || wnum[0] == "45" || wnum[0] == "46" || wnum[0] == "47" || wnum[0] == "48" || wnum[0] == "49" || wnum[0] == "50")
                    {
                        WinZhu = 0;
                    }
                    else
                    {
                        if (model.BuyNum.Contains(wnum[1]) && model.BuyNum.Contains(wnum[2]) && model.BuyNum.Contains(wnum[3]))
                        {
                            if (dmn == 1)
                            {
                                if (buynum[0].Contains(wnum[1]) || buynum[0].Contains(wnum[2]) || buynum[0].Contains(wnum[3]))
                                {
                                    WinZhu = 1;
                                }
                            }
                            else if (dmn == 2)
                            {
                                if (buynum[0].Contains(wnum[1]) && buynum[0].Contains(wnum[2]))
                                {
                                    WinZhu = 1;
                                }
                                if (buynum[0].Contains(wnum[2]) && buynum[0].Contains(wnum[3]))
                                {
                                    WinZhu = 1;
                                }
                                if (buynum[0].Contains(wnum[1]) && buynum[0].Contains(wnum[3]))
                                {
                                    WinZhu = 1;
                                }
                            }
                        }
                    }
                    #endregion
                    if (WinZhu != 0)
                    {
                        modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                        int id = new BCW.HP3.BLL.HP3WinnerSY().Add(modelWin);
                        new BCW.BLL.Guest().Add(1, modelWin.WinUserID, mename, "您的快乐扑克3:" + modelWin.WinDate + "期已经开奖，获得了" + modelWin.WinMoney + "快乐币。[url=/bbs/game/HP3SW.aspx?act=case]马上兑奖[/url]");
                        string gameplay = "";
                        switch (model.BuyType)
                        {
                            case 1:
                                gameplay = "花色连号同号投注";
                                break;
                            case 17:
                                gameplay = "大小单双投注";
                                break;
                            default:
                                gameplay = "任选投注";
                                break;
                        }
                        //动态
                        string wText = "在[url=/bbs/game/HP3SW.aspx]快乐扑克3试玩版[/url]《" + gameplay + "》狂赚" + modelWin.WinMoney + "" + "快乐币" + "";
                        new BCW.BLL.Action().Add(25, id, modelWin.WinUserID, mename, wText);
                        new BCW.HP3.BLL.HP3WinnerSY().UpdateWinZhu(model.ID, WinZhu);
                        //将该订单ID插入HP3WinnerSY
                        WinZhu = 0;
                    }
                }
                else if (model.BuyType == 12 || model.BuyType == 14 || model.BuyType == 16)//胆四、五、六
                {
                    #region 胆四、五、六
                    WinZhu = 0;
                    if (wnum[0] == "24" || wnum[0] == "25" || wnum[0] == "26" || wnum[0] == "27" || wnum[0] == "28" || wnum[0] == "29" || wnum[0] == "30" || wnum[0] == "31" || wnum[0] == "32" || wnum[0] == "33" || wnum[0] == "34" || wnum[0] == "35" || wnum[0] == "36")
                    {
                        WinZhu = 0;
                    }
                    else if (wnum[0] == "38" || wnum[0] == "39" || wnum[0] == "40" || wnum[0] == "41" || wnum[0] == "42" || wnum[0] == "43" || wnum[0] == "44" || wnum[0] == "45" || wnum[0] == "46" || wnum[0] == "47" || wnum[0] == "48" || wnum[0] == "49" || wnum[0] == "50")
                    {
                        WinZhu = 0;
                    }
                    else
                    {
                        DwMon = Convert.ToDouble(model.Odds);
                        string[] buynum = model.BuyNum.Split('#');
                        MatchCollection dmnum = Regex.Matches(buynum[0], ",");
                        MatchCollection leyr = Regex.Matches(buynum[1], ",");
                        int dmn = dmnum.Count + 1;
                        int ele = leyr.Count + 1;
                        if (model.BuyNum.Contains(wnum[1]) && model.BuyNum.Contains(wnum[2]) && model.BuyNum.Contains(wnum[3]))
                        {
                            if (model.BuyType == 12)
                            {
                                #region 胆四
                                if (buynum[0].Contains(wnum[1]) && buynum[0].Contains(wnum[2]) && buynum[0].Contains(wnum[3]))
                                {
                                    WinZhu = ele;
                                }
                                else if (buynum[0].Contains(wnum[1]) && buynum[0].Contains(wnum[2]))
                                {
                                    if (dmn == 3)
                                    {
                                        WinZhu = 1;
                                    }
                                    else if (dmn == 2)
                                    {
                                        WinZhu = ele - 1;
                                    }

                                }
                                else if (buynum[0].Contains(wnum[2]) && buynum[0].Contains(wnum[3]))
                                {
                                    if (dmn == 3)
                                    {
                                        WinZhu = 1;
                                    }
                                    else if (dmn == 2)
                                    {
                                        WinZhu = ele - 1;
                                    }
                                }
                                else if (buynum[0].Contains(wnum[1]) && buynum[0].Contains(wnum[3]))
                                {
                                    if (dmn == 3)
                                    {
                                        WinZhu = 1;
                                    }
                                    else if (dmn == 2)
                                    {
                                        WinZhu = ele - 1;
                                    }
                                }
                                else if (buynum[0].Contains(wnum[1]) || buynum[0].Contains(wnum[2]) || buynum[0].Contains(wnum[3]))
                                {
                                    if (dmn == 2)
                                    {
                                        WinZhu = 1;
                                    }
                                    else if (dmn == 1)
                                    {
                                        WinZhu = ele - 2;
                                    }
                                }

                                #endregion
                            }
                            else if (model.BuyType == 14)
                            {
                                #region 胆五
                                if (buynum[0].Contains(wnum[1]) && buynum[0].Contains(wnum[2]) && buynum[0].Contains(wnum[3]))
                                {
                                    if (dmn == 4)
                                    {
                                        WinZhu = ele;
                                    }
                                    else if (dmn == 3)
                                    {
                                        WinZhu = ele * (ele - 1) / 2;
                                    }

                                }
                                else if (buynum[0].Contains(wnum[1]) && buynum[0].Contains(wnum[2]))
                                {
                                    if (dmn == 4)
                                    {
                                        WinZhu = 1;
                                    }
                                    else if (dmn == 3)
                                    {
                                        WinZhu = ele - 1;
                                    }
                                    else if (dmn == 2)
                                    {
                                        WinZhu = (ele - 1) * (ele - 2) / 2;
                                    }
                                }
                                else if (buynum[0].Contains(wnum[2]) && buynum[0].Contains(wnum[3]))
                                {
                                    if (dmn == 4)
                                    {
                                        WinZhu = 1;
                                    }
                                    else if (dmn == 3)
                                    {
                                        WinZhu = ele - 1;
                                    }
                                    else if (dmn == 2)
                                    {
                                        WinZhu = (ele - 1) * (ele - 2) / 2;
                                    }
                                }
                                else if (buynum[0].Contains(wnum[1]) && buynum[0].Contains(wnum[3]))
                                {
                                    if (dmn == 4)
                                    {
                                        WinZhu = 1;
                                    }
                                    else if (dmn == 3)
                                    {
                                        WinZhu = ele - 1;
                                    }
                                    else if (dmn == 2)
                                    {
                                        WinZhu = (ele - 1) * (ele - 2) / 2;
                                    }
                                }
                                else if (buynum[0].Contains(wnum[1]) || buynum[0].Contains(wnum[2]) || buynum[0].Contains(wnum[3]))
                                {
                                    if (dmn == 4)
                                    {
                                        WinZhu = 0;
                                    }
                                    else if (dmn == 3)
                                    {
                                        WinZhu = 1;
                                    }
                                    else if (dmn == 2)
                                    {
                                        WinZhu = ele - 2;
                                    }
                                    else if (dmn == 1)
                                    {
                                        WinZhu = (ele - 2) * (ele - 3) / 2;
                                    }
                                }
                                #endregion

                            }
                            else if (model.BuyType == 16)
                            {
                                #region 胆六
                                int s = 0;
                                if (buynum[0].Contains(wnum[1]) && buynum[0].Contains(wnum[2]) && buynum[0].Contains(wnum[3]))
                                {
                                    if (dmn == 5)
                                    {
                                        WinZhu = ele;
                                    }
                                    else if (dmn == 4)
                                    {
                                        WinZhu = ele * (ele - 1) / 2;
                                    }
                                    else if (dmn == 3)
                                    {
                                        WinZhu = ele * (ele - 1) * (ele - 2) / 6;
                                    }
                                }
                                else if (buynum[0].Contains(wnum[1]) && buynum[0].Contains(wnum[2]))
                                {
                                    if (dmn == 5)
                                    {
                                        WinZhu = 1;
                                    }
                                    else if (dmn == 4)
                                    {
                                        WinZhu = ele - 1;
                                    }
                                    else if (dmn == 3)
                                    {
                                        WinZhu = (ele - 1) * (ele - 2) / 2;
                                    }
                                    else if (dmn == 2)
                                    {
                                        WinZhu = (ele - 1) * (ele - 2) * (ele - 3) / 6;
                                    }

                                }
                                else if (buynum[0].Contains(wnum[2]) && buynum[0].Contains(wnum[3]))
                                {
                                    if (dmn == 5)
                                    {
                                        WinZhu = 1;
                                    }
                                    else if (dmn == 4)
                                    {
                                        WinZhu = ele - 1;
                                    }
                                    else if (dmn == 3)
                                    {
                                        WinZhu = (ele - 1) * (ele - 2) / 2;
                                    }
                                    else if (dmn == 2)
                                    {
                                        WinZhu = (ele - 1) * (ele - 2) * (ele - 3) / 6;
                                    }
                                }
                                else if (buynum[0].Contains(wnum[1]) && buynum[0].Contains(wnum[3]))
                                {
                                    if (dmn == 5)
                                    {
                                        WinZhu = 1;
                                    }
                                    else if (dmn == 4)
                                    {
                                        WinZhu = ele - 1;
                                    }
                                    else if (dmn == 3)
                                    {
                                        WinZhu = (ele - 1) * (ele - 2) / 2;
                                    }
                                    else if (dmn == 2)
                                    {
                                        WinZhu = (ele - 1) * (ele - 2) * (ele - 3) / 6;
                                    }
                                }
                                else if (buynum[0].Contains(wnum[1]) || buynum[0].Contains(wnum[2]) || buynum[0].Contains(wnum[3]))
                                {
                                    if (dmn == 4)
                                    {
                                        WinZhu = 1;
                                    }
                                    else if (dmn == 3)
                                    {
                                        WinZhu = ele - 2;
                                    }
                                    else if (dmn == 2)
                                    {
                                        WinZhu = (ele - 2) * (ele - 3) / 2;
                                    }
                                    else if (dmn == 1)
                                    {
                                        WinZhu = (ele - 2) * (ele - 3) * (ele - 4) / 6;
                                    }

                                }
                                #endregion
                            }
                        }
                    }
                    #endregion
                    if (WinZhu != 0)
                    {
                        modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                        int id = new BCW.HP3.BLL.HP3WinnerSY().Add(modelWin);
                        new BCW.HP3.BLL.HP3WinnerSY().UpdateWinZhu(model.ID, WinZhu);
                        new BCW.BLL.Guest().Add(1, modelWin.WinUserID, mename, "您的快乐扑克3:" + modelWin.WinDate + "期已经开奖，获得了" + modelWin.WinMoney + "快乐币。[url=/bbs/game/HP3SW.aspx?act=case]马上兑奖[/url]");
                        string gameplay = "";
                        switch (model.BuyType)
                        {
                            case 1:
                                gameplay = "花色连号同号投注";
                                break;
                            case 17:
                                gameplay = "大小单双投注";
                                break;
                            default:
                                gameplay = "任选投注";
                                break;
                        }
                        //动态
                        string wText = "在[url=/bbs/game/HP3SW.aspx]快乐扑克3试玩版[/url]《" + gameplay + "》狂赚" + modelWin.WinMoney + "" + "快乐币" + "";
                        new BCW.BLL.Action().Add(25, id, modelWin.WinUserID, mename, wText);

                        //将该订单ID插入HP3WinnerSY
                    }
                }
            }
            try
            {
                DataSet WillGets = new BCW.HP3.BLL.HP3WinnerSY().GetLists(" ID=" + model.ID);
                long WillGetss = Convert.ToInt64(WillGets.Tables[0].Rows[0][3]);
                new BCW.HP3.BLL.HP3BuySY().UpdateWillGet(model.ID, WillGetss);
            }
            catch
            {
            }
        }
    }
    //获取后几位数
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
    //超过七天兑奖失效
    public void CaseCancel()
    {
        string date = DateTime.Now.AddDays(-7).ToString("yyyyMMdd") + "01";
        new BCW.HP3.BLL.HP3Winner().UpdateByWinDate(date);
        Response.Write("第" + date + "期前<br />过期获奖取消成功！");
    }
    //试用版超过七天兑奖失效
    public void CaseCancel2()
    {
        string date = DateTime.Now.AddDays(-7).ToString("yyyyMMdd") + "01";
        new BCW.HP3.BLL.HP3WinnerSY().UpdateByWinDate(date);
        Response.Write("<br />第" + date + "期前<br />试用版过期获奖取消成功！");
    }


    /// <summary>
    /// 更新开奖结果
    /// </summary>
    public void hp3opengame()
    {
        string hp3Id = string.Empty; ;
        string hp3Result = "";
        List<Matchs> list = TranList();
        foreach (Matchs a in list)
        {
            hp3Id = a.expect;
            hp3Result = a.opencode;
            BCW.HP3.Model.HP3_kjnum model = new BCW.HP3.Model.HP3_kjnum();

            string[] numoo = hp3Result.Split(',');
            model.datenum = hp3Id;
            model.Fnum = getfsm(numoo[0], 1);
            model.Snum = getfsm(numoo[1], 2);
            model.Tnum = getfsm(numoo[2], 3);
            model.datetime = a.datetime;
            string num = getfsm(numoo[0], 1) + "," + getfsm(numoo[1], 2) + "," + getfsm(numoo[2], 3);
            string opo = zjdm(num);
            model.Winum = opo;

            try
            {
                if (true)
                {
                    bool s = new BCW.HP3.BLL.HP3_kjnum().Exists3(hp3Id);
                    switch (s)
                    {
                        case true:
                            new BCW.HP3.BLL.HP3_kjnum().Update(model);
                            break;
                        case false:
                            break;
                    }
                }
            }
            catch
            {
            }
        }
    }


    /// <summary>
    /// 将xml解析成list集合
    /// </summary>
    /// <returns>返回list集合</returns>
    public List<Matchs> TranList()
    {
        List<Matchs> list = new List<Matchs>();
        XmlDocument dom = GetXmlData();
        XmlNodeList nodelist = dom.SelectSingleNode("/xml").ChildNodes;

        foreach (XmlNode node in nodelist)//遍历xml节点
        {
            Matchs matchs = new Matchs();
            XmlElement xe = (XmlElement)node;//将子节点类型转换为xmlelement类型

            matchs.expect = xe.GetAttribute("expect"); ;
            matchs.opencode = xe.GetAttribute("opencode");
            matchs.datetime = Convert.ToDateTime(xe.GetAttribute("opentime"));

            list.Add(matchs);
        }
        return list;
    }

    /// <summary>
    /// 获得xml接口数据
    /// </summary>
    /// <returns>返回xmldocument格式数据</returns>
    public XmlDocument GetXmlData()
    {
        string url = "http://c.apiplus.net/newly.do?token=5a8ebe52461354bc&code=sdklpk3&rows=20";//xml接口
        XmlDocument xml = new XmlDocument();
        try
        {
            xml.Load(url);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }

        return xml;
    }
    /// <summary>
    /// 每期比赛
    /// </summary>
    public class Matchs
    {
        public string expect;
        public string opencode;
        public DateTime datetime;
    }

    public string getfsm(string NumOK, int i)
    {
        string name = string.Empty;
        try
        {
            string fnums = NumOK;
            string snums = NumOK;
            string tnums = NumOK;
            if (fnums != null && snums != null && tnums != null)
            {
                MatchCollection fnum = Regex.Matches(fnums, @"[\d]{1}");
                MatchCollection snum = Regex.Matches(snums, @"[\d]{1}");
                MatchCollection tnum = Regex.Matches(tnums, @"[\d]{1}");
                string f1 = fnum[0].Value;
                switch (fnum[0].Value)
                {
                    case "1":
                        f1 = "黑桃";
                        break;
                    case "2":
                        f1 = "红桃";
                        break;
                    case "3":
                        f1 = "梅花";
                        break;
                    case "4":
                        f1 = "方块";
                        break;
                }
                string s1 = snum[0].Value;
                switch (snum[0].Value)
                {
                    case "1":
                        s1 = "黑桃";
                        break;
                    case "2":
                        s1 = "红桃";
                        break;
                    case "3":
                        s1 = "梅花";
                        break;
                    case "4":
                        s1 = "方块";
                        break;
                }
                string t1 = tnum[0].Value;
                switch (tnum[0].Value)
                {
                    case "1":
                        t1 = "黑桃";
                        break;
                    case "2":
                        t1 = "红桃";
                        break;
                    case "3":
                        t1 = "梅花";
                        break;
                    case "4":
                        t1 = "方块";
                        break;
                }
                string f = fnum[2].Value;
                if (fnum[1].Value == "0" && fnum[2].Value == "1")
                {
                    f = "A";
                }
                else if (fnum[1].Value == "1" && fnum[2].Value == "0")
                {
                    f = "10";
                }
                else if (fnum[1].Value == "1" && fnum[2].Value == "1")
                {
                    f = "J";
                }
                else if (fnum[1].Value == "1" && fnum[2].Value == "2")
                {
                    f = "Q";
                }
                else if (fnum[1].Value == "1" && fnum[2].Value == "3")
                {
                    f = "K";
                }
                else
                    f = fnum[2].Value;
                {
                }
                string sn = snum[2].Value;
                if (snum[1].Value == "0" && snum[2].Value == "1")
                {
                    sn = "A";
                }
                else if (snum[1].Value == "1" && snum[2].Value == "0")
                {
                    sn = "10";
                }
                else if (snum[1].Value == "1" && snum[2].Value == "1")
                {
                    sn = "J";
                }
                else if (snum[1].Value == "1" && snum[2].Value == "2")
                {
                    sn = "Q";
                }
                else if (snum[1].Value == "1" && snum[2].Value == "3")
                {
                    sn = "K";
                }
                else
                {
                    sn = snum[2].Value;
                }
                string t = tnum[2].Value;
                if (tnum[1].Value == "0" && tnum[2].Value == "1")
                {
                    t = "A";
                }
                else if (tnum[1].Value == "1" && tnum[2].Value == "0")
                {
                    t = "10";
                }
                else if (tnum[1].Value == "1" && tnum[2].Value == "1")
                {
                    t = "J";
                }
                else if (tnum[1].Value == "1" && tnum[2].Value == "2")
                {
                    t = "Q";
                }
                else if (tnum[1].Value == "1" && tnum[2].Value == "3")
                {
                    t = "K";
                }
                else
                {
                    t = tnum[2].Value;
                }
                if (i == 1)
                {
                    name = f1 + f;
                }
                if (i == 2)
                {
                    name = s1 + sn;
                }
                if (i == 3)
                {
                    name = t1 + t;
                }
                return name;

            }
            else
            {
                name = null; return name;
            }
        }
        catch
        {
            name = null; return name;
        }
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
