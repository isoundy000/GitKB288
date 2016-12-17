using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
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
using System.Net;
using BCW.Common;
using System.Timers;

public partial class bbs_game_klsfget : System.Web.UI.Page
{
    protected string xmlPath = "/Controls/klsf.xml";
    protected int gid = 32;
    protected string GameName = ub.GetSub("klsfName", "/Controls/klsf.xml");
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");

    /// <summary>
    /// 蒙宗将 20160901 全新模式抓取
    /// 蒙宗将 20160902 增加马上兑奖链接
    /// 蒙宗将 20160906 修复开奖单双出错
    /// 蒙宗将 20160908 增加龙虎开奖与龙虎赔率
    /// 蒙宗将 20160910 修改期数错误
    ///  蒙宗将 20160913 修复连二组选算法(9.22 修)
    ///  蒙宗将 20160930 赔率调整，
    ///  蒙宗将 20161011 修改赔率返奖
    ///  蒙宗将 20161029 修复返奖问题 再
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();

        #region 抓取
        try
        {
            UpdateState();//算法更新期数
        }
        catch
        {
            //  new BCW.tbasic.klsf().klsfLISTPAGE();//更新期数抓取获取
        }

        if (new BCW.BLL.klsflist().ExistsUpdateResult())
        {
            try
            {
                klsfopengame();//接口更新结果
            }
            catch
            {
                //  //  new BCW.tbasic.klsf().klsfOPENPAGE();//更新结果抓取
            }
        }
        try
        {
            klsffc();
        }
        catch
        {
            // Response.Write("出错了：" + er.ToString()); (Exception er)
            // new BCW.BLL.Guest().Add(1,726,"将似沙","出错了："+er.ToString()+"");//内线提示出错测试用
        }


        #region 大小单双赔率浮动
        try
        {
            ub xml = new ub();
            Application.Remove(xmlPath);//清缓存
            xml.ReloadSub(xmlPath); //加载配置
            double Odds = Utils.ParseDouble(ub.GetSub("klsfOdds17", xmlPath));
            double oddschushi = Utils.ParseDouble(ub.GetSub("klsfOddschushi", xmlPath));
            double oddsmax = Utils.ParseDouble(ub.GetSub("klsfOddsmax", xmlPath));
            double oddsmin = Utils.ParseDouble(ub.GetSub("klsfOddsmin", xmlPath));
            double oddschushi2 = oddschushi * 2;

            string strWheres = string.Empty;
            strWheres += "State = 1 order by ID desc";
            DataSet ds = new BCW.BLL.klsflist().GetList("Result", strWheres);

            string[] result1 = ds.Tables[0].Rows[0]["Result"].ToString().Split(',');
            int temp1 = 0;
            int sum1 = 0;
            string temps1 = string.Empty;
            for (int j = 0; j < result1.Length; j++)
            {
                temp1 = Convert.ToInt32(result1[j]);
                sum1 += temp1;
            }
            temps1 = Convert.ToString(sum1);
            temps1 = temps1.Substring(temps1.Length - 1, 1);
            temp1 = Convert.ToInt32(temps1);
            int count1 = 1;
            int count2 = 1;
            int count3 = 1;
            int count4 = 1;
            int count5 = 1;
            int count6 = 1;
            bool s1 = true;
            bool s2 = true;
            bool s3 = true;
            bool s4 = true;
            bool s5 = true;
            bool s6 = true;
            #region 大
            if (temp1 > 4)
            {
                while (s1)
                {
                    string[] Result = ds.Tables[0].Rows[count1]["Result"].ToString().Split(',');
                    int temp = 0;
                    int sum = 0;
                    string temps = string.Empty;
                    for (int j = 0; j < Result.Length; j++)
                    {
                        temp = Convert.ToInt32(Result[j]);
                        sum += temp;
                    }
                    temps = Convert.ToString(sum);
                    temps = temps.Substring(temps.Length - 1, 1);
                    temp = Convert.ToInt32(temps);
                    if (temp > 4)
                        count1++;
                    else
                        s1 = false;
                }
                if (count1 == 1)
                {
                    xml.dss["klsfOdds13"] = oddschushi;
                    xml.dss["klsfOdds14"] = oddschushi;
                }
                else
                {
                    if (oddschushi + Odds * (count1 - 1) > oddsmax || oddschushi + Odds * (count1 - 1) < oddsmin)
                    {
                        if (oddschushi + Odds * (count1 - 1) > oddsmax) { xml.dss["klsfOdds13"] = oddsmax; xml.dss["klsfOdds14"] = oddschushi2 - oddsmax; }
                        if (oddschushi + Odds * (count1 - 1) < oddsmin) { xml.dss["klsfOdds13"] = oddsmin; xml.dss["klsfOdds14"] = oddschushi2 - oddsmin; }
                    }
                    else
                    {
                        xml.dss["klsfOdds13"] = oddschushi + Odds * (count1 - 1);
                        xml.dss["klsfOdds14"] = oddschushi2 - (oddschushi + Odds * (count1 - 1));
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
                    string[] Result = ds.Tables[0].Rows[count1]["Result"].ToString().Split(',');
                    int temp = 0;
                    int sum = 0;
                    string temps = string.Empty;
                    for (int j = 0; j < Result.Length; j++)
                    {
                        temp = Convert.ToInt32(Result[j]);
                        sum += temp;
                    }
                    temps = Convert.ToString(sum);
                    temps = temps.Substring(temps.Length - 1, 1);
                    temp = Convert.ToInt32(temps);
                    if (temp > 4)
                        s1 = false;
                    else
                        count1++;
                }
                if (count1 == 1)
                {
                    xml.dss["klsfOdds13"] = oddschushi;
                    xml.dss["klsfOdds14"] = oddschushi;
                }
                else
                {
                    if (oddschushi + Odds * (count1 - 1) > oddsmax || oddschushi + Odds * (count1 - 1) < oddsmin)
                    {
                        if (oddschushi + Odds * (count1 - 1) > oddsmax) { xml.dss["klsfOdds14"] = oddsmax; xml.dss["klsfOdds13"] = oddschushi2 - oddsmax; }
                        if (oddschushi + Odds * (count1 - 1) < oddsmin) { xml.dss["klsfOdds14"] = oddsmin; xml.dss["klsfOdds13"] = oddschushi2 - oddsmin; }
                    }
                    else
                    {
                        xml.dss["klsfOdds13"] = oddschushi2 - (oddschushi + Odds * (count1 - 1));
                        xml.dss["klsfOdds14"] = oddschushi + Odds * (count1 - 1);
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
                    string[] Result = ds.Tables[0].Rows[count2]["Result"].ToString().Split(',');
                    int temp = 0;
                    int sum = 0;
                    string temps = string.Empty;
                    for (int j = 0; j < Result.Length; j++)
                    {
                        temp = Convert.ToInt32(Result[j]);
                        sum += temp;
                    }
                    temps = Convert.ToString(sum);
                    temps = temps.Substring(temps.Length - 1, 1);
                    temp = Convert.ToInt32(temps);
                    if (temp % 2 != 0)
                        count2++;
                    else
                        s2 = false;
                }
                if (count2 == 1)
                {
                    xml.dss["klsfOdds15"] = oddschushi;
                    xml.dss["klsfOdds16"] = oddschushi;
                }
                else
                {
                    if (oddschushi + Odds * (count2 - 1) > oddsmax || oddschushi + Odds * (count2 - 1) < oddsmin)
                    {
                        if (oddschushi + Odds * (count2 - 1) > oddsmax) { xml.dss["klsfOdds15"] = oddsmax; xml.dss["klsfOdds16"] = oddschushi2 - oddsmax; }
                        if (oddschushi + Odds * (count2 - 1) < oddsmin) { xml.dss["klsfOdds15"] = oddsmin; xml.dss["klsfOdds16"] = oddschushi2 - oddsmin; }
                    }
                    else
                    {
                        xml.dss["klsfOdds15"] = oddschushi + Odds * (count2 - 1);
                        xml.dss["klsfOdds16"] = oddschushi2 - (oddschushi + Odds * (count2 - 1));
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
                    string[] Result = ds.Tables[0].Rows[count2]["Result"].ToString().Split(',');
                    int temp = 0;
                    int sum = 0;
                    string temps = string.Empty;
                    for (int j = 0; j < Result.Length; j++)
                    {
                        temp = Convert.ToInt32(Result[j]);
                        sum += temp;
                    }
                    temps = Convert.ToString(sum);
                    temps = temps.Substring(temps.Length - 1, 1);
                    temp = Convert.ToInt32(temps);
                    if (temp % 2 != 0)
                        s2 = false;
                    else
                        count2++;
                }
                if (count2 == 1)
                {
                    xml.dss["klsfOdds15"] = oddschushi;
                    xml.dss["klsfOdds16"] = oddschushi;
                }
                else
                {
                    if (oddschushi + Odds * (count2 - 1) > oddsmax || oddschushi + Odds * (count2 - 1) < oddsmin)
                    {
                        if (oddschushi + Odds * (count2 - 1) > oddsmax) { xml.dss["klsfOdds16"] = oddsmax; xml.dss["klsfOdds15"] = oddschushi2 - oddsmax; }
                        if (oddschushi + Odds * (count2 - 1) < oddsmin) { xml.dss["klsfOdds16"] = oddsmin; xml.dss["klsfOdds15"] = oddschushi2 - oddsmin; }
                    }
                    else
                    {
                        xml.dss["klsfOdds15"] = oddschushi2 - (oddschushi + Odds * (count2 - 1));
                        xml.dss["klsfOdds16"] = oddschushi + Odds * (count2 - 1);
                    }
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion

            #region 龙（1与8位）
            if (GetLHF(ds.Tables[0].Rows[0]["Result"].ToString(), 1) == 1)
            {
                while (s3)
                {
                    string Result = ds.Tables[0].Rows[count3]["Result"].ToString();

                    if (GetLHF(Result, 1) == 1)
                        count3++;
                    else
                        s3 = false;
                }
                if (count3 == 1)
                {
                    xml.dss["klsfOdds18"] = oddschushi;
                    xml.dss["klsfOdds19"] = oddschushi;
                }
                else
                {
                    if (oddschushi + Odds * (count3 - 1) > oddsmax || oddschushi + Odds * (count3 - 1) < oddsmin)
                    {
                        if (oddschushi + Odds * (count3 - 1) > oddsmax) { xml.dss["klsfOdds18"] = oddsmax; xml.dss["klsfOdds19"] = oddschushi2 - oddsmax; }
                        if (oddschushi + Odds * (count3 - 1) < oddsmin) { xml.dss["klsfOdds18"] = oddsmin; xml.dss["klsfOdds19"] = oddschushi2 - oddsmin; }
                    }
                    else
                    {
                        xml.dss["klsfOdds18"] = oddschushi + Odds * (count3 - 1);
                        xml.dss["klsfOdds19"] = oddschushi2 - (oddschushi + Odds * (count3 - 1));
                    }
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #region 虎（1与8位）
            else// if (GetLHF(ds.Tables[0].Rows[0]["Result"].ToString(), 1) == 2)
            {
                while (s3)
                {
                    string Result = ds.Tables[0].Rows[count3]["Result"].ToString();

                    if (GetLHF(Result, 1) == 2)
                        count3++;
                    else
                        s3 = false;
                }
                if (count3 == 1)
                {
                    xml.dss["klsfOdds18"] = oddschushi;
                    xml.dss["klsfOdds19"] = oddschushi;
                }
                else
                {
                    if (oddschushi + Odds * (count3 - 1) > oddsmax || oddschushi + Odds * (count3 - 1) < oddsmin)
                    {
                        if (oddschushi + Odds * (count3 - 1) > oddsmax) { xml.dss["klsfOdds19"] = oddsmax; xml.dss["klsfOdds18"] = oddschushi2 - oddsmax; }
                        if (oddschushi + Odds * (count3 - 1) < oddsmin) { xml.dss["klsfOdds19"] = oddsmin; xml.dss["klsfOdds18"] = oddschushi2 - oddsmin; }
                    }
                    else
                    {
                        xml.dss["klsfOdds19"] = oddschushi + Odds * (count3 - 1);
                        xml.dss["klsfOdds18"] = oddschushi2 - (oddschushi + Odds * (count3 - 1));
                    }
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #region 龙（2与7位）
            if (GetLHF(ds.Tables[0].Rows[0]["Result"].ToString(), 2) == 1)
            {
                while (s4)
                {
                    string Result = ds.Tables[0].Rows[count4]["Result"].ToString();

                    if (GetLHF(Result, 2) == 1)
                        count4++;
                    else
                        s4 = false;
                }
                if (count4 == 1)
                {
                    xml.dss["klsfOdds20"] = oddschushi;
                    xml.dss["klsfOdds21"] = oddschushi;
                }
                else
                {
                    if (oddschushi + Odds * (count4 - 1) > oddsmax || oddschushi + Odds * (count4 - 1) < oddsmin)
                    {
                        if (oddschushi + Odds * (count4 - 1) > oddsmax) { xml.dss["klsfOdds20"] = oddsmax; xml.dss["klsfOdds21"] = oddschushi2 - oddsmax; }
                        if (oddschushi + Odds * (count4 - 1) < oddsmin) { xml.dss["klsfOdds20"] = oddsmin; xml.dss["klsfOdds21"] = oddschushi2 - oddsmin; }
                    }
                    else
                    {
                        xml.dss["klsfOdds20"] = oddschushi + Odds * (count4 - 1);
                        xml.dss["klsfOdds21"] = oddschushi2 - (oddschushi + Odds * (count4 - 1));
                    }
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #region 虎（2与7位）
            else// if (GetLHF(ds.Tables[0].Rows[0]["Result"].ToString(), 2) == 2)
            {
                while (s4)
                {
                    string Result = ds.Tables[0].Rows[count4]["Result"].ToString();

                    if (GetLHF(Result, 2) == 2)
                        count4++;
                    else
                        s4 = false;
                }
                if (count4 == 1)
                {
                    xml.dss["klsfOdds20"] = oddschushi;
                    xml.dss["klsfOdds21"] = oddschushi;
                }
                else
                {
                    if (oddschushi + Odds * (count4 - 1) > oddsmax || oddschushi + Odds * (count4 - 1) < oddsmin)
                    {
                        if (oddschushi + Odds * (count4 - 1) > oddsmax) { xml.dss["klsfOdds21"] = oddsmax; xml.dss["klsfOdds20"] = oddschushi2 - oddsmax; }
                        if (oddschushi + Odds * (count4 - 1) < oddsmin) { xml.dss["klsfOdds21"] = oddsmin; xml.dss["klsfOdds20"] = oddschushi2 - oddsmin; }
                    }
                    else
                    {
                        xml.dss["klsfOdds21"] = oddschushi + Odds * (count4 - 1);
                        xml.dss["klsfOdds20"] = oddschushi2 - (oddschushi + Odds * (count4 - 1));
                    }
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #region 龙（3与6位）
            if (GetLHF(ds.Tables[0].Rows[0]["Result"].ToString(), 3) == 1)
            {
                while (s5)
                {
                    string Result = ds.Tables[0].Rows[count5]["Result"].ToString();

                    if (GetLHF(Result, 3) == 1)
                        count5++;
                    else
                        s5 = false;
                }
                if (count5 == 1)
                {
                    xml.dss["klsfOdds22"] = oddschushi;
                    xml.dss["klsfOdds23"] = oddschushi;
                }
                else
                {
                    if (oddschushi + Odds * (count5 - 1) > oddsmax || oddschushi + Odds * (count5 - 1) < oddsmin)
                    {
                        if (oddschushi + Odds * (count5 - 1) > oddsmax) { xml.dss["klsfOdds22"] = oddsmax; xml.dss["klsfOdds23"] = oddschushi2 - oddsmax; }
                        if (oddschushi + Odds * (count5 - 1) < oddsmin) { xml.dss["klsfOdds22"] = oddsmin; xml.dss["klsfOdds23"] = oddschushi2 - oddsmin; }
                    }
                    else
                    {
                        xml.dss["klsfOdds22"] = oddschushi + Odds * (count5 - 1);
                        xml.dss["klsfOdds23"] = oddschushi2 - (oddschushi + Odds * (count5 - 1));
                    }
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #region 虎（3与6位）
            else//  if (GetLHF(ds.Tables[0].Rows[0]["Result"].ToString(), 3) == 2)
            {
                while (s5)
                {
                    string Result = ds.Tables[0].Rows[count5]["Result"].ToString();

                    if (GetLHF(Result, 3) == 2)
                        count5++;
                    else
                        s5 = false;
                }
                if (count5 == 1)
                {
                    xml.dss["klsfOdds23"] = oddschushi;
                    xml.dss["klsfOdds22"] = oddschushi;
                }
                else
                {
                    if (oddschushi + Odds * (count5 - 1) > oddsmax || oddschushi + Odds * (count5 - 1) < oddsmin)
                    {
                        if (oddschushi + Odds * (count5 - 1) > oddsmax) { xml.dss["klsfOdds23"] = oddsmax; xml.dss["klsfOdds22"] = oddschushi2 - oddsmax; }
                        if (oddschushi + Odds * (count5 - 1) < oddsmin) { xml.dss["klsfOdds23"] = oddsmin; xml.dss["klsfOdds22"] = oddschushi2 - oddsmin; }
                    }
                    else
                    {
                        xml.dss["klsfOdds23"] = oddschushi + Odds * (count5 - 1);
                        xml.dss["klsfOdds22"] = oddschushi2 - (oddschushi + Odds * (count5 - 1));
                    }
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #region 龙（4与5位）
            if (GetLHF(ds.Tables[0].Rows[0]["Result"].ToString(), 4) == 1)
            {
                while (s6)
                {
                    string Result = ds.Tables[0].Rows[count6]["Result"].ToString();

                    if (GetLHF(Result, 4) == 1)
                        count6++;
                    else
                        s6 = false;
                }
                if (count6 == 1)
                {
                    xml.dss["klsfOdds24"] = oddschushi;
                    xml.dss["klsfOdds25"] = oddschushi;
                }
                else
                {
                    if (oddschushi + Odds * (count6 - 1) > oddsmax || oddschushi + Odds * (count6 - 1) < oddsmin)
                    {
                        if (oddschushi + Odds * (count6 - 1) > oddsmax) { xml.dss["klsfOdds24"] = oddsmax; xml.dss["klsfOdds25"] = oddschushi2 - oddsmax; }
                        if (oddschushi + Odds * (count6 - 1) < oddsmin) { xml.dss["klsfOdds24"] = oddsmin; xml.dss["klsfOdds25"] = oddschushi2 - oddsmin; }
                    }
                    else
                    {
                        xml.dss["klsfOdds24"] = oddschushi + Odds * (count6 - 1);
                        xml.dss["klsfOdds25"] = oddschushi2 - (oddschushi + Odds * (count6 - 1));
                    }
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion
            #region 虎（4与5位）
            else //  if (GetLHF(ds.Tables[0].Rows[0]["Result"].ToString(), 4) == 2)
            {
                while (s6)
                {
                    string Result = ds.Tables[0].Rows[count6]["Result"].ToString();

                    if (GetLHF(Result, 4) == 2)
                        count6++;
                    else
                        s6 = false;
                }
                if (count6 == 1)
                {
                    xml.dss["klsfOdds24"] = oddschushi;
                    xml.dss["klsfOdds25"] = oddschushi;
                }
                else
                {
                    if (oddschushi + Odds * (count6 - 1) > oddsmax || oddschushi + Odds * (count6 - 1) < oddsmin)
                    {
                        if (oddschushi + Odds * (count6 - 1) > oddsmax) { xml.dss["klsfOdds25"] = oddsmax; xml.dss["klsfOdds24"] = oddschushi2 - oddsmax; }
                        if (oddschushi + Odds * (count6 - 1) < oddsmin) { xml.dss["klsfOdds25"] = oddsmin; xml.dss["klsfOdds24"] = oddschushi2 - oddsmin; }
                    }
                    else
                    {
                        xml.dss["klsfOdds25"] = oddschushi + Odds * (count6 - 1);
                        xml.dss["klsfOdds24"] = oddschushi2 - (oddschushi + Odds * (count6 - 1));
                    }
                }
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            #endregion

        }
        catch
        {

        }
        #endregion

        try
        {
            BCW.Model.klsflist la = new BCW.BLL.klsflist().GetklsflistLast();
            BCW.Model.klsflist lar = new BCW.BLL.klsflist().GetklsflistLast2();
            Response.Write("" + GameName + "刷新ok1<br/>");
            Response.Write("现在是第" + la.klsfId + "期<br/>");
            Response.Write("这期的投注截至时间是" + la.EndTime + "<br/>");
            Response.Write("已开奖的最新一期是第" + lar.klsfId + "期<br/>");
            Response.Write("开奖结果是" + lar.Result + "<br />");
        }
        catch
        {

        }

        #endregion

        Response.Write("<b>上次获取时间：</b>" + DateTime.Now + "<br />");
        stopwatch.Stop();
        Response.Write("总耗时:" + stopwatch.Elapsed.TotalSeconds + "秒");
    }

    /// <summary>
    /// 返彩的方法
    /// </summary>
    private void klsffc()
    {
        //开始返彩
        DataSet ds = new BCW.BLL.klsfpay().GetList("ID,Types,klsfId,UsID,UsName,Price,Notes,Result,Prices,Odds,isRoBot", "State=0 and Result<>''");  //选取没有返彩的数据 state=0未返彩
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                int ID = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());   //该条数据的ID
                int Types = int.Parse(ds.Tables[0].Rows[i]["Types"].ToString()); //该条记录的投注类型
                int klsfId = int.Parse(ds.Tables[0].Rows[i]["klsfId"].ToString()); //该条记录的期数
                string Notes = ds.Tables[0].Rows[i]["Notes"].ToString();  //该用户在该期的投注
                string[] Temp = Notes.Split(',');
                int UsID = int.Parse(ds.Tables[0].Rows[i]["UsID"].ToString()); //该用户的ID
                string UsName = ds.Tables[0].Rows[i]["UsName"].ToString(); //该用户的用户名
                string Result = ds.Tables[0].Rows[i]["Result"].ToString().Replace(" ", ""); //该期的开奖结果
                long Price = long.Parse(ds.Tables[0].Rows[i]["Price"].ToString()); //每注的单价
                decimal Odds = decimal.Parse(ds.Tables[0].Rows[i]["Odds"].ToString()); //赔率
                int IsRoBot = int.Parse(ds.Tables[0].Rows[i]["isRoBot"].ToString());//机器人标识
                int IsSWB = Utils.ParseInt(ub.GetSub("klsfSWB", xmlPath));
                long WinCent = 0;
                //int Prices = int.Parse(ds.Tables[0].Rows[i]["Prices"].ToString()); //这一次投注的总价


                if (Types == 1 || Types == 3 || Types == 5 || Types == 7)  //胆拖类型的返彩
                {
                    string[] iNum_kj = Result.Split(',');  //存放结果
                    string[] iNum_kj_hm = Result.Split(',');  //存放结果
                    string[] iNum_zj = Notes.Split('|');  //把胆码和拖码分开存储
                    string[] iNum_zj_d = iNum_zj[0].Split(','); //胆码
                    string[] iNum_zj_t = iNum_zj[1].Split(',');  //拖码

                    int zj_ds = 0; //中奖的胆码数
                    int zj_ts = 0; //中奖的拖码数
                    int zj = 0; //中奖的注数

                    //统计胆码的中奖数
                    for (int i1 = 0; i1 < iNum_zj_d.Length; i1++)
                    {
                        for (int p = 0; p < iNum_kj.Length; p++)
                        {
                            if (iNum_zj_d[i1] == iNum_kj[p])
                            {
                                zj_ds += 1;
                            }
                        }
                    }

                    if (zj_ds == iNum_zj_d.Length) //判断胆码是否全中
                    {
                        //统计拖码的中奖数
                        for (int i1 = 0; i1 < iNum_zj_t.Length; i1++)
                        {
                            for (int p = 0; p < iNum_kj.Length; p++)
                            {
                                if (iNum_zj_t[i1] == iNum_kj[p])
                                {
                                    zj_ts += 1;
                                }
                            }
                        }

                        switch (Types)
                        {
                            case 1: //五胆拖类型
                                {
                                    if (zj_ds + zj_ts >= 5)
                                    {
                                        zj = C(zj_ts, 5 - iNum_zj_d.Length);
                                        WinCent = Convert.ToInt64(zj * (Odds * Price));
                                    }
                                }
                                break;
                            case 3: //四胆拖类型
                                {
                                    if (zj_ds + zj_ts >= 4)
                                    {
                                        zj = C(zj_ts, 4 - iNum_zj_d.Length);
                                        WinCent = Convert.ToInt64(zj * (Odds * Price));
                                    }
                                }
                                break;
                            case 5: //三胆拖类型
                                {
                                    if (zj_ds + zj_ts >= 3)
                                    {
                                        zj = C(zj_ts, 3 - iNum_zj_d.Length);
                                        WinCent = Convert.ToInt64(zj * (Odds * Price));
                                    }
                                }
                                break;
                            default: //二胆拖类型
                                {
                                    if (zj_ds + zj_ts >= 2)
                                    {
                                        zj = C(zj_ts, 2 - iNum_zj_d.Length);
                                        WinCent = Convert.ToInt64(zj * (Odds * Price));
                                    }
                                }
                                break;
                        }
                    }
                    else  //胆码没有全中则没有中奖
                    {
                        zj = 0;
                        WinCent = 0;
                    }
                    //把中奖结果更新到数据库
                    string str_SQL = string.Format(@"UPDATE [tb_klsfpay]  SET [WinCent] = {0},[iWin]={1}  WHERE klsfId={2} and ID = {3}", WinCent, zj, klsfId, ID);
                    //如果中奖了就发一条内线
                    if (zj > 0)
                    {
                        BCW.Data.SqlHelper.ExecuteSql(str_SQL);
                        #region 根据版本选择快乐币和酷币的获取和显示

                        //string PlayUsID = ub.GetSub("klsfRobotId", "/Controls/klsf.xml");
                        // bool IsRob = false;
                        // if (PlayUsID != "")
                        // {
                        //     string[] sNum = PlayUsID.Split('#');
                        //     int[] iNum = new int[sNum.Length];
                        //     for (int j = 0; j < sNum.Length; j++)
                        //     {
                        //         if (sNum[j].Replace(" ","") != "")
                        //         {
                        //             iNum[j] = int.Parse((sNum[j].Replace(" ", "")));
                        //             if (iNum[j] == UsID)
                        //             {
                        //                 IsRob = true;
                        //                 break;
                        //             }
                        //         }
                        //     }
                        // }
                        if (IsRoBot == 0)//!IsRob
                        {
                            if (IsSWB == 0)
                            {
                                new BCW.BLL.Guest().Add(1, UsID, UsName, "您的[url=/bbs/game/klsf.aspx]" + GameName + "[/url]:" + klsfId + "期" + OutType(Types) + "已经开奖，获得了" + WinCent + "快乐币。");//开奖提示信息,1表示开奖信息
                            }
                            else
                            {
                                new BCW.BLL.Guest().Add(1, UsID, UsName, "您的[url=/bbs/game/klsf.aspx]" + GameName + "[/url]:" + klsfId + "期" + OutType(Types) + "已经开奖，获得了" + WinCent + ub.Get("SiteBz") + "[url=/bbs/game/klsf.aspx?act=case]>>马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                            }
                        }
                        #endregion
                    }
                }
                else if (Types == 2 || Types == 4 || Types == 6 || Types == 8)  //任选普通类型的返彩
                {
                    string[] iNum_kj = Result.Split(',');  //该期开奖的结果
                    string[] iNum_zj = Notes.Split(',');  //该期该用户购买的彩票

                    int zj_zs = 0; //统计中奖注数

                    for (int fs = 0; fs < iNum_zj.Length; fs++)
                        for (int p = 0; p < iNum_kj.Length; p++)
                            if (string.Compare(iNum_zj[fs], iNum_kj[p]) == 0)
                                zj_zs += 1;


                    switch (Types)
                    {
                        case 2: //任选五普通
                            {
                                if (zj_zs >= 5)
                                {
                                    zj_zs = C(zj_zs, 5);
                                    WinCent = Convert.ToInt64(zj_zs * (Odds * Price));
                                }
                                else
                                    zj_zs = 0;
                            }
                            break;
                        case 4: //任选四普通
                            {
                                if (zj_zs >= 4)
                                {
                                    zj_zs = C(zj_zs, 4);
                                    WinCent = Convert.ToInt64(zj_zs * (Odds * Price));
                                }
                                else
                                    zj_zs = 0;
                            }
                            break;
                        case 6: //任选三普通
                            {
                                if (zj_zs >= 3)
                                {
                                    zj_zs = C(zj_zs, 3);
                                    WinCent = Convert.ToInt64(zj_zs * (Odds * Price));
                                }
                                else
                                    zj_zs = 0;
                            }
                            break;
                        default: //任选二普通
                            {

                                if (zj_zs >= 2)
                                {
                                    zj_zs = C(zj_zs, 2);
                                    WinCent = Convert.ToInt64(zj_zs * (Odds * Price));
                                }
                                else
                                    zj_zs = 0;
                            }
                            break;
                    }
                    //把中奖结果更新到数据库
                    string str_SQL = string.Format(@"UPDATE [tb_klsfpay]  SET [WinCent] = {0},[iWin]={1}  WHERE klsfId={2} and ID = {3}", WinCent, zj_zs, klsfId, ID);
                    //如果中奖了就发一条内线通知
                    if (zj_zs > 0)
                    {
                        BCW.Data.SqlHelper.ExecuteSql(str_SQL);
                        #region 根据版本选择快乐币和酷币的获取和显示
                        //          string PlayUsID = ub.GetSub("klsfRobotId", "/Controls/klsf.xml");
                        //bool IsRob = false;
                        //if (PlayUsID != "")
                        //{
                        //    string[] sNum = PlayUsID.Split('#');
                        //    int[] iNum = new int[sNum.Length];
                        //    for (int j = 0; j < sNum.Length; j++)
                        //    {
                        //        if (sNum[j].Replace(" ", "") != "")
                        //        {
                        //            iNum[j] = int.Parse((sNum[j].Replace(" ", "")));
                        //            if (iNum[j] == UsID)
                        //            {
                        //                IsRob = true;
                        //                break;
                        //            }
                        //        }
                        //    }
                        //}
                        if (IsRoBot == 0)//!IsRob
                        {
                            if (IsSWB == 0)
                            {
                                new BCW.BLL.Guest().Add(1, UsID, UsName, "您的[url=/bbs/game/klsf.aspx]" + GameName + "[/url]:" + klsfId + "期" + OutType(Types) + "已经开奖，获得了" + WinCent + "快乐币。");//开奖提示信息,1表示开奖信息
                            }
                            else
                            {
                                new BCW.BLL.Guest().Add(1, UsID, UsName, "您的[url=/bbs/game/klsf.aspx]" + GameName + "[/url]:" + klsfId + "期" + OutType(Types) + "已经开奖，获得了" + WinCent + ub.Get("SiteBz") + "[url=/bbs/game/klsf.aspx?act=case]>>马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                            }
                        }
                        #endregion
                    }
                }
                else if (Types == 9) //连二直选
                {
                    string[] iNum_kj = Result.Split(',');
                    string[] iNum_kj_q = Result.Split(',');
                    string[] iNum_kj_h = Result.Split(',');
                    string[] iNum_zj = Notes.Split('|');
                    string[] iNum_zj_q = iNum_zj[0].Split(',');
                    string[] iNum_zj_h = iNum_zj[1].Split(',');

                    for (int c = 0; c < iNum_kj.Length; c++) //把对比投注组设为0，作判断依据
                    {
                        iNum_kj_q[c] = "9";
                        iNum_kj_h[c] = "9";
                    }

                    int zj_zs = 0; //中奖的注数

                    for (int j = 0; j < iNum_zj_q.Length; j++) //寻找到匹配的数字，替换掉对比组内相对应位置的"0"
                        for (int p = 0; p < iNum_kj.Length; p++)
                            if (iNum_zj_q[j] == iNum_kj[p])
                                iNum_kj_q[p] = iNum_kj[p];

                    for (int j = 0; j < iNum_zj_h.Length; j++) //寻找到匹配的数字，替换掉对比组内相对应位置的"0"
                        for (int p = 0; p < iNum_kj.Length; p++)
                            if (iNum_zj_h[j] == iNum_kj[p])
                                iNum_kj_h[p] = iNum_kj[p];

                    for (int h = 0; h < iNum_kj.Length - 1; h++) //如果对应位置不是"0"则是中奖
                    {
                        if ((iNum_kj_q[h] != "9") && (iNum_kj_h[h + 1] != "9"))
                        {
                            zj_zs += 1;
                        }
                    }
                    WinCent = Convert.ToInt64(zj_zs * (Odds * Price));
                    //把中奖结果更新到数据库
                    string str_SQL = string.Format(@"UPDATE [tb_klsfpay]  SET [WinCent] = {0},[iWin]={1}  WHERE klsfId={2} and ID = {3}", WinCent, zj_zs, klsfId, ID);
                    //如果中奖了就发一条内线通知
                    if (zj_zs > 0)
                    {
                        BCW.Data.SqlHelper.ExecuteSql(str_SQL);
                        #region 根据版本选择快乐币和酷币的获取和显示
                        //          string PlayUsID = ub.GetSub("klsfRobotId", "/Controls/klsf.xml");
                        //bool IsRob = false;
                        //if (PlayUsID != "")
                        //{
                        //    string[] sNum = PlayUsID.Split('#');
                        //    int[] iNum = new int[sNum.Length];
                        //    for (int j = 0; j < sNum.Length; j++)
                        //    {
                        //        if (sNum[j].Replace(" ", "") != "")
                        //        {
                        //            iNum[j] = int.Parse((sNum[j].Replace(" ", "")));
                        //            if (iNum[j] == UsID)
                        //            {
                        //                IsRob = true;
                        //                break;
                        //            }
                        //        }
                        //    }
                        //}
                        if (IsRoBot == 0)//!IsRob
                        {
                            if (IsSWB == 0)
                            {
                                new BCW.BLL.Guest().Add(1, UsID, UsName, "您的[url=/bbs/game/klsf.aspx]" + GameName + "[/url]:" + klsfId + "期" + OutType(Types) + "已经开奖，获得了" + WinCent + "快乐币。");//开奖提示信息,1表示开奖信息
                            }
                            else
                            {
                                new BCW.BLL.Guest().Add(1, UsID, UsName, "您的[url=/bbs/game/klsf.aspx]" + GameName + "[/url]:" + klsfId + "期" + OutType(Types) + "已经开奖，获得了" + WinCent + ub.Get("SiteBz") + "[url=/bbs/game/klsf.aspx?act=case]>>马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                            }
                        }
                        #endregion
                    }
                }
                else if (Types == 10) //连二组选
                {
                    string[] iNum_kj = Result.Split(',');
                    string[] iNum_kj_hm = Result.Split(',');
                    string[] iNum_zj = Notes.Split(',');

                    for (int c = 0; c < iNum_kj_hm.Length; c++)
                    {
                        iNum_kj_hm[c] = "9";
                    }

                    int zj_zs = 0;

                    for (int j = 0; j < iNum_zj.Length; j++)
                        for (int p = 0; p < iNum_kj.Length; p++)
                            if (iNum_zj[j] == iNum_kj[p])
                                iNum_kj_hm[p] = p.ToString();

                    for (int h = 0; h < iNum_kj_hm.Length - 1; h++)
                        if ((iNum_kj_hm[h] != "9") && (iNum_kj_hm[h + 1] != "9"))
                            zj_zs += 1;


                    WinCent = Convert.ToInt64(zj_zs * (Odds * Price));
                    //把中奖结果更新到数据库
                    string str_SQL = string.Format(@"UPDATE [tb_klsfpay]  SET [WinCent] = {0},[iWin]={1}  WHERE klsfId={2} and ID = {3}", WinCent, zj_zs, klsfId, ID);
                    //如果中奖了就发一条内线通知
                    if (zj_zs > 0)
                    {
                        BCW.Data.SqlHelper.ExecuteSql(str_SQL);
                        #region 根据版本选择快乐币和酷币的获取和显示
                        //          string PlayUsID = ub.GetSub("klsfRobotId", "/Controls/klsf.xml");
                        //bool IsRob = false;
                        //if (PlayUsID != "")
                        //{
                        //    string[] sNum = PlayUsID.Split('#');
                        //    int[] iNum = new int[sNum.Length];
                        //    for (int j = 0; j < sNum.Length; j++)
                        //    {
                        //        if (sNum[j].Replace(" ", "") != "")
                        //        {
                        //            iNum[j] = int.Parse((sNum[j].Replace(" ", "")));
                        //            if (iNum[j] == UsID)
                        //            {
                        //                IsRob = true;
                        //                break;
                        //            }
                        //        }
                        //    }
                        //}
                        if (IsRoBot == 0)//!IsRob
                        {
                            if (IsSWB == 0)
                            {
                                new BCW.BLL.Guest().Add(1, UsID, UsName, "您的[url=/bbs/game/klsf.aspx]" + GameName + "[/url]:" + klsfId + "期" + OutType(Types) + "已经开奖，获得了" + WinCent + "快乐币。");//开奖提示信息,1表示开奖信息
                            }
                            else
                            {
                                new BCW.BLL.Guest().Add(1, UsID, UsName, "您的[url=/bbs/game/klsf.aspx]" + GameName + "[/url]:" + klsfId + "期" + OutType(Types) + "已经开奖，获得了" + WinCent + ub.Get("SiteBz") + "[url=/bbs/game/klsf.aspx?act=case]>>马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                            }
                        }
                        #endregion
                    }
                }
                else if (Types == 11)//前一红投
                {
                    string[] iNum_kj = Result.Split(',');
                    string[] iNum_zj = Notes.Split(',');
                    int zj_zs = 0;
                    for (int j = 0; j < iNum_zj.Length; j++)
                    {
                        if (iNum_zj[j] == iNum_kj[0])
                        {
                            zj_zs += 1;
                        }
                    }

                    WinCent = Convert.ToInt64(zj_zs * (Odds * Price));
                    //把中奖结果更新到数据库
                    string str_SQL = string.Format(@"UPDATE [tb_klsfpay]  SET [WinCent] = {0},[iWin]={1}  WHERE klsfId={2} and ID = {3}", WinCent, zj_zs, klsfId, ID);
                    //如果中奖了就发一条内线通知
                    if (zj_zs > 0)
                    {
                        BCW.Data.SqlHelper.ExecuteSql(str_SQL);
                        #region 根据版本选择快乐币和酷币的获取和显示
                        //          string PlayUsID = ub.GetSub("klsfRobotId", "/Controls/klsf.xml");
                        //bool IsRob = false;
                        //if (PlayUsID != "")
                        //{
                        //    string[] sNum = PlayUsID.Split('#');
                        //    int[] iNum = new int[sNum.Length];
                        //    for (int j = 0; j < sNum.Length; j++)
                        //    {
                        //        if (sNum[j].Replace(" ", "") != "")
                        //        {
                        //            iNum[j] = int.Parse((sNum[j].Replace(" ", "")));
                        //            if (iNum[j] == UsID)
                        //            {
                        //                IsRob = true;
                        //                break;
                        //            }
                        //        }
                        //    }
                        //}
                        if (IsRoBot == 0)//!IsRob
                        {
                            if (IsSWB == 0)
                            {
                                new BCW.BLL.Guest().Add(1, UsID, UsName, "您的[url=/bbs/game/klsf.aspx]" + GameName + "[/url]:" + klsfId + "期" + OutType(Types) + "已经开奖，获得了" + WinCent + "快乐币。");//开奖提示信息,1表示开奖信息
                            }
                            else
                            {
                                new BCW.BLL.Guest().Add(1, UsID, UsName, "您的[url=/bbs/game/klsf.aspx]" + GameName + "[/url]:" + klsfId + "期" + OutType(Types) + "已经开奖，获得了" + WinCent + ub.Get("SiteBz") + "[url=/bbs/game/klsf.aspx?act=case]>>马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                            }
                        }
                        #endregion
                    }

                }
                else if (Types == 12)//前一数投
                {
                    string[] iNum_kj = Result.Split(',');
                    string[] iNum_zj = Notes.Split(',');
                    int zj_zs = 0;
                    for (int j = 0; j < iNum_zj.Length; j++)
                        if (iNum_zj[j] == iNum_kj[0])
                            zj_zs += 1;

                    WinCent = Convert.ToInt64(zj_zs * (Odds * Price));
                    //把中奖结果更新到数据库
                    string str_SQL = string.Format(@"UPDATE [tb_klsfpay]  SET [WinCent] = {0},[iWin]={1}  WHERE klsfId={2} and ID = {3}", WinCent, zj_zs, klsfId, ID);
                    //如果中奖了就发一条内线通知
                    if (zj_zs > 0)
                    {
                        //Response.Write(Out.Tab("<div class=\"title\">", ""));
                        //Response.Write(Out.Tab("中奖金额:" + str_SQL + " ", ""));
                        //Response.Write(Out.Tab("</div>", "<br />"));//在前台显示变量得值 测试用
                        BCW.Data.SqlHelper.ExecuteSql(str_SQL);
                        #region 根据版本选择快乐币和酷币的获取和显示
                        //          string PlayUsID = ub.GetSub("klsfRobotId", "/Controls/klsf.xml");
                        //bool IsRob = false;
                        //if (PlayUsID != "")
                        //{
                        //    string[] sNum = PlayUsID.Split('#');
                        //    int[] iNum = new int[sNum.Length];
                        //    for (int j = 0; j < sNum.Length; j++)
                        //    {
                        //        if (sNum[j].Replace(" ", "") != "")
                        //        {
                        //            iNum[j] = int.Parse((sNum[j].Replace(" ", "")));
                        //            if (iNum[j] == UsID)
                        //            {
                        //                IsRob = true;
                        //                break;
                        //            }
                        //        }
                        //    }
                        //}
                        if (IsRoBot == 0)//!IsRob
                        {
                            if (IsSWB == 0)
                            {
                                new BCW.BLL.Guest().Add(1, UsID, UsName, "您的[url=/bbs/game/klsf.aspx]" + GameName + "[/url]:" + klsfId + "期" + OutType(Types) + "已经开奖，获得了" + WinCent + "快乐币。");//开奖提示信息,1表示开奖信息
                            }
                            else
                            {
                                new BCW.BLL.Guest().Add(1, UsID, UsName, "您的[url=/bbs/game/klsf.aspx]" + GameName + "[/url]:" + klsfId + "期" + OutType(Types) + "已经开奖，获得了" + WinCent + ub.Get("SiteBz") + "[url=/bbs/game/klsf.aspx?act=case]>>马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                            }
                        }
                        #endregion
                    }
                }
                else if (Types == 13)
                {
                    string[] iNum_kj = Result.Split(',');
                    string zj = Notes;
                    int sum = 0;
                    int zj_zs = 0;

                    try
                    {
                        for (int j = 0; j < iNum_kj.Length; j++)
                        {
                            sum += Convert.ToInt32(iNum_kj[j]);
                        }
                        string temp = Convert.ToString(sum);
                        temp = temp.Substring(temp.Length - 1, 1);
                        sum = Convert.ToInt32(temp);
                        if (sum >= 5 && zj == "大")
                        {
                            zj_zs = 1;
                            WinCent = Convert.ToInt64(zj_zs * (Odds * Price));
                        }
                        else if (sum <= 4 && zj == "小")
                        {
                            zj_zs = 1;
                            WinCent = Convert.ToInt64(zj_zs * (Odds * Price));
                        }
                    }
                    catch
                    { }
                    //把中奖结果更新到数据库
                    string str_SQL = string.Format(@"UPDATE [tb_klsfpay]  SET [WinCent] = {0},[iWin]={1}  WHERE klsfId={2} and ID = {3}", WinCent, zj_zs, klsfId, ID);
                    //如果中奖了就发一条内线通知
                    if (zj_zs > 0)
                    {
                        BCW.Data.SqlHelper.ExecuteSql(str_SQL);
                        #region 根据版本选择快乐币和酷币的获取和显示
                        //          string PlayUsID = ub.GetSub("klsfRobotId", "/Controls/klsf.xml");
                        //bool IsRob = false;
                        //if (PlayUsID != "")
                        //{
                        //    string[] sNum = PlayUsID.Split('#');
                        //    int[] iNum = new int[sNum.Length];
                        //    for (int j = 0; j < sNum.Length; j++)
                        //    {
                        //        if (sNum[j].Replace(" ", "") != "")
                        //        {
                        //            iNum[j] = int.Parse((sNum[j].Replace(" ", "")));
                        //            if (iNum[j] == UsID)
                        //            {
                        //                IsRob = true;
                        //                break;
                        //            }
                        //        }
                        //    }
                        //}
                        if (IsRoBot == 0)//!IsRob
                        {
                            if (IsSWB == 0)
                            {
                                new BCW.BLL.Guest().Add(1, UsID, UsName, "您的[url=/bbs/game/klsf.aspx]" + GameName + "[/url]:" + klsfId + "期" + OutType(Types) + "已经开奖，获得了" + WinCent + "快乐币。");//开奖提示信息,1表示开奖信息
                            }
                            else
                            {
                                new BCW.BLL.Guest().Add(1, UsID, UsName, "您的[url=/bbs/game/klsf.aspx]" + GameName + "[/url]:" + klsfId + "期" + OutType(Types) + "已经开奖，获得了" + WinCent + ub.Get("SiteBz") + "[url=/bbs/game/klsf.aspx?act=case]>>马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                            }
                        }
                        #endregion
                    }
                }
                else if (Types == 14)
                {
                    string[] iNum_kj = Result.Split(',');
                    string zj = Notes;
                    int sum = 0;
                    int zj_zs = 0;

                    try
                    {
                        for (int j = 0; j < iNum_kj.Length; j++)
                        {
                            sum += Convert.ToInt32(iNum_kj[j]);
                        }
                        if (sum % 2 != 0 && zj == "单")
                        {
                            zj_zs = 1;
                            WinCent = Convert.ToInt64(zj_zs * (Odds * Price));
                        }
                        else if (sum % 2 == 0 && zj == "双")
                        {
                            zj_zs = 1;
                            WinCent = Convert.ToInt64(zj_zs * (Odds * Price));
                        }
                    }
                    catch
                    { }
                    //把中奖结果更新到数据库
                    string str_SQL = string.Format(@"UPDATE [tb_klsfpay]  SET [WinCent] = {0},[iWin]={1}  WHERE klsfId={2} and ID = {3}", WinCent, zj_zs, klsfId, ID);
                    //如果中奖了就发一条内线通知
                    if (zj_zs > 0)
                    {
                        BCW.Data.SqlHelper.ExecuteSql(str_SQL);
                        #region 根据版本选择快乐币和酷币的获取和显示
                        //string PlayUsID = ub.GetSub("klsfRobotId", "/Controls/klsf.xml");
                        //bool IsRob = false;
                        //if (PlayUsID != "")
                        //{
                        //    string[] sNum = PlayUsID.Split('#');
                        //    int[] iNum = new int[sNum.Length];
                        //    for (int j = 0; j < sNum.Length; j++)
                        //    {
                        //        if (sNum[j].Replace(" ", "") != "")
                        //        {
                        //            iNum[j] = int.Parse((sNum[j].Replace(" ", "")));
                        //            if (iNum[j] == UsID)
                        //            {
                        //                IsRob = true;
                        //                break;
                        //            }
                        //        }
                        //    }
                        //}
                        if (IsRoBot == 0)//!IsRob
                        {
                            if (IsSWB == 0)
                            {
                                new BCW.BLL.Guest().Add(1, UsID, UsName, "您的[url=/bbs/game/klsf.aspx]" + GameName + "[/url]:" + klsfId + "期" + OutType(Types) + "已经开奖，获得了" + WinCent + "快乐币。");//开奖提示信息,1表示开奖信息
                            }
                            else
                            {
                                new BCW.BLL.Guest().Add(1, UsID, UsName, "您的[url=/bbs/game/klsf.aspx]" + GameName + "[/url]:" + klsfId + "期" + OutType(Types) + "已经开奖，获得了" + WinCent + ub.Get("SiteBz") + "[url=/bbs/game/klsf.aspx?act=case]>>马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                            }
                        }
                        #endregion

                    }
                }
                else if (Types == 15)//龙虎
                {
                    string[] iNum_kj = Result.Split(',');
                    int num1, num2, num3, num4, num5, num6, num7, num8;
                    num1 = Convert.ToInt32(iNum_kj[0]);
                    num2 = Convert.ToInt32(iNum_kj[1]);
                    num3 = Convert.ToInt32(iNum_kj[2]);
                    num4 = Convert.ToInt32(iNum_kj[3]);
                    num5 = Convert.ToInt32(iNum_kj[4]);
                    num6 = Convert.ToInt32(iNum_kj[5]);
                    num7 = Convert.ToInt32(iNum_kj[6]);
                    num8 = Convert.ToInt32(iNum_kj[7]);
                    string zj = Notes;
                    int zhug = 0;

                    try
                    {
                        if (num1 > num8 && zj == "龙（1与8位）")
                        {
                            zhug = 1;
                            WinCent = Convert.ToInt64(zhug * (Odds * Price));
                        }
                        if (num8 > num1 && zj == "虎（1与8位）")
                        {
                            zhug = 1;
                            WinCent = Convert.ToInt64(zhug * (Odds * Price));
                        }
                        if (num2 > num7 && zj == "龙（2与7位）")
                        {
                            zhug = 1;
                            WinCent = Convert.ToInt64(zhug * (Odds * Price));
                        }
                        if (num7 > num2 && zj == "虎（2与7位）")
                        {
                            zhug = 1;
                            WinCent = Convert.ToInt64(zhug * (Odds * Price));
                        }
                        if (num3 > num6 && zj == "龙（3与6位）")
                        {
                            zhug = 1;
                            WinCent = Convert.ToInt64(zhug * (Odds * Price));
                        }
                        if (num6 > num3 && zj == "虎（3与6位）")
                        {
                            zhug = 1;
                            WinCent = Convert.ToInt64(zhug * (Odds * Price));
                        }
                        if (num4 > num5 && zj == "龙（4与5位）")
                        {
                            zhug = 1;
                            WinCent = Convert.ToInt64(zhug * (Odds * Price));
                        }
                        if (num5 > num4 && zj == "虎（4与5位）")
                        {
                            zhug = 1;
                            WinCent = Convert.ToInt64(zhug * (Odds * Price));
                        }
                    }
                    catch
                    { }
                    //把中奖结果更新到数据库
                    string str_SQL = string.Format(@"UPDATE [tb_klsfpay]  SET [WinCent] = {0},[iWin]={1}  WHERE klsfId={2} and ID = {3}", WinCent, zhug, klsfId, ID);
                    //如果中奖了就发一条内线通知
                    if (zhug > 0)
                    {
                        BCW.Data.SqlHelper.ExecuteSql(str_SQL);
                        #region 根据版本选择快乐币和酷币的获取和显示
                        //string PlayUsID = ub.GetSub("klsfRobotId", "/Controls/klsf.xml");
                        //bool IsRob = false;
                        //if (PlayUsID != "")
                        //{
                        //    string[] sNum = PlayUsID.Split('#');
                        //    int[] iNum = new int[sNum.Length];
                        //    for (int j = 0; j < sNum.Length; j++)
                        //    {
                        //        if (sNum[j].Replace(" ", "") != "")
                        //        {
                        //            iNum[j] = int.Parse((sNum[j].Replace(" ", "")));
                        //            if (iNum[j] == UsID)
                        //            {
                        //                IsRob = true;
                        //                break;
                        //            }
                        //        }
                        //    }
                        //}
                        if (IsRoBot == 0)//!IsRob
                        {
                            if (IsSWB == 0)
                            {
                                new BCW.BLL.Guest().Add(1, UsID, UsName, "您的[url=/bbs/game/klsf.aspx]" + GameName + "[/url]:" + klsfId + "期已经开奖，获得了" + WinCent + "快乐币。");//开奖提示信息,1表示开奖信息
                            }
                            else
                            {
                                new BCW.BLL.Guest().Add(1, UsID, UsName, "您的[url=/bbs/game/klsf.aspx]" + GameName + "[/url]:" + klsfId + "期" + OutType(Types) + "已经开奖，获得了" + WinCent + ub.Get("SiteBz") + "[url=/bbs/game/klsf.aspx?act=case]>>马上兑奖[/url]");//开奖提示信息,1表示开奖信息
                            }
                        }
                        #endregion

                    }
                }
                //更新已开奖
                new BCW.BLL.klsfpay().UpdateState(ID, 1);
            }
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

    /// <summary>
    /// 投注方式
    /// </summary>
    /// <param name="Types">投注类型的编号</param>
    /// <returns>对应的类型</returns>
    private string OutType(int Types)
    {
        string pText = string.Empty;
        if (Types == 1)
            pText = "任选五胆拖投注";
        else if (Types == 2)
            pText = "任选五普通复式";
        else if (Types == 3)
            pText = "任选四胆拖投注";
        else if (Types == 4)
            pText = "任选四普通复式";
        else if (Types == 5)
            pText = "任选三胆拖投注";
        else if (Types == 6)
            pText = "任选三普通复式";
        else if (Types == 7)
            pText = "任选二胆拖投注";
        else if (Types == 8)
            pText = "任选二普通复式";
        else if (Types == 9)
            pText = "连二直选";
        else if (Types == 10)
            pText = "连二组选";
        else if (Types == 11)
            pText = "前一红投";
        else if (Types == 12)
            pText = "前一数投";
        else if (Types == 13)
            pText = "大小";
        else if (Types == 14)
            pText = "单双";
        else if (Types == 15)
            pText = "龙虎";
        return pText;
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

            matchs.expect = xe.GetAttribute("expect").Substring(2, 6) + xe.GetAttribute("expect").Substring(9, 2);
            matchs.opencode = xe.GetAttribute("opencode");

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
        string url = "http://c.apiplus.net/newly.do?token=5a8ebe52461354bc&code=cqklsf&rows=20";//xml接口
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
    }

    #region 更新期数
    public string UpdateState()
    {
        //计算出期号截止时间，如果抓取的开售时间有变请更改代码！！！
        DateTime dt1 = Convert.ToDateTime("10:00:00");//10点钟之后开始第15期
        DateTime dt2 = Convert.ToDateTime("02:00:00");//2.12到10点之间固定为14期的时间
        DateTime dt22 = Convert.ToDateTime("23:50:00");//10点到23.50为14到97期
        DateTime dat = Convert.ToDateTime("00:00:00");//00点判断新的开始，23.50到00点为停售时间，不做操作
        DateTime dt23 = Convert.ToDateTime("23:59:59");//10点到23.50为14到97期
        if (DateTime.Now >= dat && DateTime.Now < dt23)
        {
            BCW.Model.klsflist model = new BCW.Model.klsflist();
            string state = string.Empty;
            if (DateTime.Now > dt22 && DateTime.Now < dt23)
            {
                state = DateTime.Now.AddDays(1).ToString("yyyyMMdd").Substring(2, 6) + "01";
                model.EndTime = Convert.ToDateTime("" + DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") + " 00:00:00" + "");
                model.klsfId = Convert.ToInt32(state);
                model.Result = "";
            }
            else
            {

                string dt3 = string.Empty;
                if (DateTime.Now >= dat && DateTime.Now <= dt2)
                {
                    dt3 = DateTime.Now.AddMinutes(5).Subtract(dat).Duration().TotalMinutes.ToString();
                }
                if (DateTime.Now > dt2 && DateTime.Now < dt1)
                {

                    dt3 = "145.00";
                }
                if (DateTime.Now >= dt1 && DateTime.Now <= dt22)
                {
                    dt3 = DateTime.Now.AddMinutes(145).Subtract(dt1).Duration().TotalMinutes.ToString();
                }

                decimal dt4 = Convert.ToDecimal(dt3);
                int dt5 = Convert.ToInt32(dt4 / 10);
                string dt6 = dt5.ToString();
                if (dt6.Length == 1)
                {
                    dt6 = "0" + dt6;
                }
                if (dt6.Length == 2)
                {
                    dt6 = "" + dt6;
                }
                state = DateTime.Now.ToString("yyyyMMdd").Substring(2, 6) + dt6;


                if (DateTime.Now.Hour >= 0 && DateTime.Now.Hour < 2)
                {
                    model.klsfId = Convert.ToInt32(state) + 1;
                }
                else
                {
                    model.klsfId = Convert.ToInt32(state);
                }

                model.Result = "";
                string datee = string.Empty;
                if (DateTime.Now.Hour >= 0 && DateTime.Now.Hour <= 2)
                {
                    datee = DateTime.ParseExact((("20" + state.Substring(0, 6)) + " 00:00:00"), "yyyyMMdd HH:mm:ss", null).ToString("yyyy-MM-dd HH:mm:ss");
                }
                if (DateTime.Now.Hour >= 10)
                {
                    datee = DateTime.ParseExact((("20" + state.Substring(0, 6)) + " 00:00:00"), "yyyyMMdd HH:mm:ss", null).ToString("yyyy-MM-dd HH:mm:ss");
                }
                //根据期号算时间,

                if (Convert.ToInt32(dt6) < 10 && Convert.ToInt32(dt6) >= 01)
                {
                    for (int i = 0; i < Convert.ToInt32(dt6); i++)
                    {
                        model.EndTime = Convert.ToDateTime(datee).AddMinutes(10 * (int.Parse(GetLastStr(state, 1)))).AddSeconds(20);

                    }
                }
                else if (Convert.ToInt32(dt6) >= 10 && Convert.ToInt32(dt6) <= 13)
                {
                    model.EndTime = Convert.ToDateTime(datee).AddMinutes(10 * (int.Parse(GetLastStr(state, 2)))).AddSeconds(20);
                }
                else if (Convert.ToInt32(dt6) == 14)
                {
                    model.EndTime = Convert.ToDateTime(" " + DateTime.Now.ToString("yyyy-MM-dd") + " 10:00:20");
                }
                else if (Convert.ToInt32(dt6) > 14 && Convert.ToInt32(dt6) <= 97)
                {
                    for (int i = 0; i < Convert.ToInt32(dt6) - 14; i++)
                    {
                        model.EndTime = Convert.ToDateTime(datee).AddMinutes(10 * (int.Parse(GetLastStr(state, 2)))).AddHours(8).AddMinutes(-20).AddSeconds(20);

                    }
                }
            }

            model.State = 0;
            bool s;
            if (DateTime.Now.Hour >= 0 && DateTime.Now.Hour < 2)
            {
                s = new BCW.BLL.klsflist().ExistsklsfId(Convert.ToInt32(state) + 1);
            }
            else
            {
                s = new BCW.BLL.klsflist().ExistsklsfId(Convert.ToInt32(state));
            }

            switch (s)
            {
                case false:
                    new BCW.BLL.klsflist().Add(model);

                    break;
                case true:
                    break;
            }
            return state;

        }
        return "0";
    }
    #endregion

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


    /// <summary>
    /// 更新开奖结果
    /// </summary>
    public void klsfopengame()
    {
        int klsfId = 0;
        string klsfResult = "";
        List<Matchs> list = TranList();
        foreach (Matchs a in list)
        {
            klsfId = Convert.ToInt32(a.expect);
            klsfResult = a.opencode;

            try
            {
                if (true)
                {
                    bool s = new BCW.BLL.klsflist().ExistsklsfId(klsfId);
                    switch (s)
                    {
                        case true:
                            new BCW.BLL.klsflist().UpdateResult(klsfId.ToString(), klsfResult);
                            new BCW.BLL.klsfpay().UpdateResult(klsfId.ToString(), klsfResult);
                            break;
                        case false:
                            break;
                    }
                }
            }
            catch
            { }
        }
    }

    #region 计算开奖龙虎
    public string GetLH(string Result)
    {
        string LH = string.Empty;
        string kai1 = string.Empty;
        string kai2 = string.Empty;
        string kai3 = string.Empty;
        string kai4 = string.Empty;
        string[] num = Result.Split(',');
        try
        {
            if (Convert.ToInt32(num[0]) > Convert.ToInt32(num[7]))
            {
                kai1 = "龙（1与8位）";
            }
            else//   if (Convert.ToInt32(num[0]) < Convert.ToInt32(num[7]))
            {
                kai1 = "虎（1与8位）";
            }
            if (Convert.ToInt32(num[1]) > Convert.ToInt32(num[6]))
            {
                kai2 = "龙（2与7位）";
            }
            else// if (Convert.ToInt32(num[6]) < Convert.ToInt32(num[1]))
            {
                kai2 = "虎（2与7位）";
            }
            if (Convert.ToInt32(num[2]) > Convert.ToInt32(num[5]))
            {
                kai3 = "龙（3与6位）";
            }
            else//  if (Convert.ToInt32(num[5]) < Convert.ToInt32(num[2]))
            {
                kai3 = "虎（3与6位）";
            }
            if (Convert.ToInt32(num[3]) > Convert.ToInt32(num[4]))
            {
                kai4 = "龙（4与5位）";
            }
            else//  if (Convert.ToInt32(num[4]) < Convert.ToInt32(num[3]))
            {
                kai4 = "虎（4与5位）";
            }
            LH = kai1 + ", " + kai2 + ", " + kai3 + ", " + kai4;
        }
        catch
        {
            return "";
        }

        return LH;
    }
    #endregion

    #region 计算开奖龙虎分析
    public int GetLHF(string Result, int i)
    {
        int LH = 0;
        string kai1 = string.Empty;
        string kai2 = string.Empty;
        string kai3 = string.Empty;
        string kai4 = string.Empty;
        string[] num = Result.Split(',');
        try
        {
            if (i == 1)
            {
                if (Convert.ToInt32(num[0]) > Convert.ToInt32(num[7]))
                {
                    LH = 1;
                }
                else//   if (Convert.ToInt32(num[0]) < Convert.ToInt32(num[7]))
                {
                    LH = 2;
                }
            }
            if (i == 2)
            {
                if (Convert.ToInt32(num[1]) > Convert.ToInt32(num[6]))
                {
                    LH = 1;
                }
                else// if (Convert.ToInt32(num[6]) < Convert.ToInt32(num[1]))
                {
                    LH = 2;
                }
            }
            if (i == 3)
            {
                if (Convert.ToInt32(num[2]) > Convert.ToInt32(num[5]))
                {
                    LH = 1;
                }
                else//  if (Convert.ToInt32(num[5]) < Convert.ToInt32(num[2]))
                {
                    LH = 2;
                }
            }
            if (i == 4)
            {
                if (Convert.ToInt32(num[3]) > Convert.ToInt32(num[4]))
                {
                    LH = 1;
                }
                else//  if (Convert.ToInt32(num[4]) < Convert.ToInt32(num[3]))
                {
                    LH = 2;
                }
            }

        }
        catch
        {
            return 0;
        }

        return LH;
    }
    #endregion
}
