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
using System.Text;
using System.IO;
using System.Net;
using System.Xml;

/// <summary>
/// 陈志基 Luck28 增加抓取判断calnum
/// 16/6/24
/// 
/// /// 邵广林 20161111
/// 修改赔率浮动算法
/// </summary>
public partial class bbs_game_Luck28Get : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/luck28.xml";
    protected static int biaoshi = 0;//开奖标识
    public static int qihao = 0;
    public void Page_Load(object sender, EventArgs e)
    {
        string _time = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
        int _time_num = Convert.ToInt32(_time);
        string now = DateTime.Now.ToShortDateString();
        string over = DateTime.Now.AddDays(-30).ToShortDateString();
        string overDay = ub.GetSub("overDay", xmlPath);
        //Response.Write("overDay:" + overDay + "<br/>");
        //Response.Write("now:" + now + "<br/>");
        //Response.Write("over:" + over + "<br/>");
        //Response.Write("_time_num:" + _time_num + "<br/>");
        if (_time_num == 2300) //晚上11点收回过期酷币
        {
            new BCW.BLL.Game.Luckpay().UpdateOverDay(over);
        }
        if (IsOpen() == true)
        {
            Luck28List();
            // Luck28Get();
        }
        else
        {
            Response.Write(Out.Tab("<div class=\"text\">", ""));
            Response.Write("幸运28游戏");
            Response.Write(Out.Tab("</div>", "<br />"));
            Response.Write(Out.Tab("<div>", ""));
            Response.Write("游戏开放时间:" + ub.GetSub("Luck28OnTime", xmlPath) + "");
            Response.Write("<br />目前还没到开放时间哦!close1");
            Response.Write("<br /><a href=\"" + Utils.getUrl("luck28.aspx?act=list&amp;backurl=" + Utils.PostPage(1) + "") + "\">历史开奖</a> ");
            Response.Write(Out.Tab("</div>", ""));

        }
    }
    /// <summary>
    /// 获取开奖数据
    /// </summary>
    public void Luck28Get()
    {
        string pan = DateTime.Now.Hour.ToString() + DateTime.Now.Minute;
        qihao = Utils.ParseInt(GetStageqihao());//抓取的期号
        Response.Write("最新抓取的期号是:" + qihao + "<br/>");
        string getresult = GetStageS();//得到开奖期号，三个号码
                                       //   Response.Write(":" + getresult + "<br/>");
        string[] result = getresult.Split(",".ToCharArray());
        long allcents = 0;//所有玩家赢的总钱数
        if (getresult != "")
        {
            Response.Write("<b>数据获取中！！！！ ok1</b><br/>");
        }
        else
        {
            Response.Write("<b>数据获取失败！！！！ error1</b><br/>");
        }
        string Luck28OpenType = ub.GetSub("Luck28OpenType", xmlPath);
        if (Luck28OpenType == "0")   //选择抓取开奖
        {
            int num = 0;
            string postnum = string.Empty;
            int[] allnums = CalNum(getresult);   //获取号码
            if (allnums.Length == 4)   //计算正确
            {
                Response.Write("号码是:");
                #region  计算号码
                for (int ii = 0; ii < allnums.Length; ii++)
                {
                    if (ii == 3)
                    {
                        Response.Write("开奖结果是:" + allnums[ii] + "<br/>");
                    }
                    else
                    {
                        Response.Write("," + allnums[ii] + "");
                        if (string.IsNullOrEmpty(postnum))
                        {
                            postnum = postnum + allnums[ii];
                        }
                        else
                        {
                            postnum = postnum + "+" + allnums[ii];//得到三个号码
                        }
                    }
                }
                #endregion
                num = allnums[3];//得到总和 
                if (!string.IsNullOrEmpty(postnum))  //如果抓取到号码
                {
                    allcents = duijiang(num, new BCW.BLL.Game.Lucklist().GetID(qihao));//遍历数据兑奖
                    #region  更新开奖结果
                    //  ChangeOdds(num);  //修改浮动赔率
                    BCW.Model.Game.Lucklist luck1 = new BCW.Model.Game.Lucklist();
                    luck1.SumNum = num;
                    luck1.Bjkl8Qihao = qihao;
                    luck1.PostNum = postnum;
                    luck1.LuckCent = new BCW.BLL.Game.Luckpay().GetSumBuyCent(new BCW.BLL.Game.Lucklist().GetID(qihao), num.ToString());
                    luck1.State = 1;
                    luck1.Bjkl8Nums = getresult;
                    //  luck1.Bjkl8Qihao = Utils.ParseInt(GetStageqihao()) + 2;
                    new BCW.BLL.Game.Lucklist().Update3(luck1);//更新最新一期
                    #endregion
                }
            }
        }
        UpdateState();
    }
    /// <summary>
    ///   遍历数据兑奖
    /// </summary>
    /// <param name="num">开奖号码</param>
    /// <param name="ID">期数</param>
    /// <returns>返回所有玩家赢的钱</returns>
    private long duijiang(int num, int ID)
    {
        #region   遍历数据兑奖
        DataSet model = new BCW.BLL.Game.Luckpay().GetList("*", "LuckId=" + ID + " and State=0");//遍历这一期所有的下注
        long allcents = 0;//所有玩家赢的总钱数
        for (int i = 0; i < model.Tables[0].Rows.Count; i++)
        {
            // if (Utils.ParseInt(model.Tables[0].Rows[i][11].ToString())== 0)//不是机械人下注
            {
                //Response.Write("抓取网站最新已开奖号码是:<br/>" + getresult + "<br/>");
                long wincents = 0;
                string r = model.Tables[0].Rows[i]["BuyNum"].ToString();
                string types = model.Tables[0].Rows[i]["BuyType"].ToString();//得到玩家买的类型
                // Response.Write("<br/>第" + (i + 1) + "注:" + r + "<br/>");
                // string[] BuyNum = r.Split(",".ToCharArray());//得到每注的下注号码
                double odds = 1; ;
                if (("," + r + ",").Contains("," + num + ","))//中奖
                {
                    #region   switch(types)
                    switch (types)
                    {
                        case "Luck28Choose":
                            odds = float.Parse((ub.GetSub("Buy" + num + "odds", xmlPath)));//根据类型得到XML的赔率
                            wincents = Convert.ToInt64(Convert.ToInt64(model.Tables[0].Rows[i]["BuyCent"]) * odds);  //计算赢的钱
                            //new BCW.BLL.Guest().Add(0, 729, "", "Luck28Choose出错了");
                            break;
                        case "Luck28End":
                            #region  尾数

                            if (num % 10 == 0)//尾数0
                            {
                                odds = float.Parse(ub.GetSub("End0odds", xmlPath));//根据类型得到XML的赔率
                                wincents = Convert.ToInt64(Convert.ToInt64(model.Tables[0].Rows[i]["BuyCent"]) * odds);  //计算赢的钱
                            }
                            else if (num % 10 == 1)//尾数1
                            {
                                odds = float.Parse(ub.GetSub("End1odds", xmlPath));//根据类型得到XML的赔率
                                wincents = Convert.ToInt64(Convert.ToInt64(model.Tables[0].Rows[i]["BuyCent"]) * odds);  //计算赢的钱
                            }
                            else if (num % 10 == 2)//尾数2
                            {
                                odds = float.Parse(ub.GetSub("End2odds", xmlPath));//根据类型得到XML的赔率
                                wincents = Convert.ToInt64(Convert.ToInt64(model.Tables[0].Rows[i]["BuyCent"]) * odds);  //计算赢的钱
                            }
                            else if (num % 10 == 3)//尾数3
                            {
                                odds = float.Parse(ub.GetSub("End3odds", xmlPath));//根据类型得到XML的赔率
                                wincents = Convert.ToInt64(Convert.ToInt64(model.Tables[0].Rows[i]["BuyCent"]) * odds);  //计算赢的钱
                            }
                            else if (num % 10 == 4)//尾数4
                            {
                                odds = float.Parse(ub.GetSub("End4odds", xmlPath));//根据类型得到XML的赔率
                                wincents = Convert.ToInt64(Convert.ToInt64(model.Tables[0].Rows[i]["BuyCent"]) * odds);  //计算赢的钱
                            }
                            else if (num % 10 == 5)//尾数5
                            {
                                odds = float.Parse(ub.GetSub("End5odds", xmlPath));//根据类型得到XML的赔率
                                wincents = Convert.ToInt64(Convert.ToInt64(model.Tables[0].Rows[i]["BuyCent"]) * odds);  //计算赢的钱
                            }
                            else if (num % 10 == 6)//尾数6
                            {
                                odds = float.Parse(ub.GetSub("End6odds", xmlPath));//根据类型得到XML的赔率
                                wincents = Convert.ToInt64(Convert.ToInt64(model.Tables[0].Rows[i]["BuyCent"]) * odds);  //计算赢的钱
                            }
                            else if (num % 10 == 7)//尾数7
                            {
                                odds = float.Parse(ub.GetSub("End7odds", xmlPath));//根据类型得到XML的赔率
                                wincents = Convert.ToInt64(Convert.ToInt64(model.Tables[0].Rows[i]["BuyCent"]) * odds);  //计算赢的钱
                            }
                            else if (num % 10 == 8)//尾数8
                            {
                                odds = float.Parse(ub.GetSub("End8odds", xmlPath));//根据类型得到XML的赔率
                                wincents = Convert.ToInt64(Convert.ToInt64(model.Tables[0].Rows[i]["BuyCent"]) * odds);  //计算赢的钱
                            }
                            else if (num % 10 == 9)//尾数9
                            {
                                odds = float.Parse(ub.GetSub("End9odds", xmlPath));//根据类型得到XML的赔率
                                wincents = Convert.ToInt64(Convert.ToInt64(model.Tables[0].Rows[i]["BuyCent"]) * odds);  //计算赢的钱
                            }
                            else
                            {
                                //new BCW.BLL.Guest().Add(0, 729, "", "Luck28End出错了");
                            }
                            break;
                        #endregion
                        case "Luck28Big":
                            // odds = float.Parse(ub.GetSub("Luck28Big", xmlPath));//根据类型得到XML的赔率
                            odds = Convert.ToDouble(model.Tables[0].Rows[i]["odds"]);
                            wincents = Convert.ToInt64(Convert.ToInt64(model.Tables[0].Rows[i]["BuyCents"]) * odds);  //计算赢的钱
                                                                                                                      //  new BCW.BLL.Guest().Add(0, 50167, "", "Luck28Big---odds:" + odds + ",BuyCents:" + Convert.ToInt64(model.Tables[0].Rows[i]["BuyCents"]) + ",wincents:" + wincents);
                            break;
                        case "Luck28Small":
                            //  odds = float.Parse(ub.GetSub("Luck28Small", xmlPath));//根据类型得到XML的赔率
                            odds = Convert.ToDouble(model.Tables[0].Rows[i]["odds"]);
                            //  Convert.ToDecimal
                            wincents = Convert.ToInt64(Convert.ToInt64(model.Tables[0].Rows[i]["BuyCents"]) * odds);  //计算赢的钱
                                                                                                                      // new BCW.BLL.Guest().Add(0, 729, "", "Luck28Small出错了");
                                                                                                                      //  new BCW.BLL.Guest().Add(0, 50167, "", "Luck28Big---odds:" + odds + ",BuyCents:" + Convert.ToInt64(model.Tables[0].Rows[i]["BuyCents"]) + ",wincents:" + wincents);
                            break;
                        case "Luck28Single":
                            //Utils.Error("" + (ub.GetSub("Luck28Single", xmlPath)) + "", "");
                            //odds = float.Parse(ub.GetSub("Luck28Single", xmlPath));//根据类型得到XML的赔率
                            odds = Convert.ToDouble(model.Tables[0].Rows[i]["odds"]);
                            wincents = Convert.ToInt64(Convert.ToInt64(model.Tables[0].Rows[i]["BuyCents"]) * odds);  //计算赢的钱
                                                                                                                      //  new BCW.BLL.Guest().Add(0, 729, "", "Luck28Single出错了");

                            // new BCW.BLL.Guest().Add(0, 50167, "", "Luck28Big---odds:" + odds + ",BuyCents:" + Convert.ToInt64(model.Tables[0].Rows[i]["BuyCents"]) + ",wincents:" + wincents);
                            break;
                        case "Luck28Double":
                            //  odds = float.Parse(ub.GetSub("Luck28Double", xmlPath));//根据类型得到XML的赔率
                            odds = Convert.ToDouble(model.Tables[0].Rows[i]["odds"]);
                            wincents = Convert.ToInt64(Convert.ToInt64(model.Tables[0].Rows[i]["BuyCents"]) * odds);  //计算赢的钱
                                                                                                                      //new BCW.BLL.Guest().Add(0, 729, "", "Luck28Double出错了");
                                                                                                                      // new BCW.BLL.Guest().Add(0, 50167, "", "Luck28Big---odds:" + odds + ",BuyCents:" + Convert.ToInt64(model.Tables[0].Rows[i]["BuyCents"]) + ",wincents:" + wincents);
                            break;
                        case "Luck28BigSingle":
                            odds = double.Parse(ub.GetSub("Luck28BigSingle", xmlPath));//根据类型得到XML的赔率
                            wincents = Convert.ToInt64(Convert.ToInt64(model.Tables[0].Rows[i]["BuyCents"]) * odds);  //计算赢的钱
                            // new BCW.BLL.Guest().Add(0, 729, "", "Luck28BigSingle出错了");
                            break;
                        case "Luck28SmallSingle":
                            odds = double.Parse((ub.GetSub("Luck28SmallSingle", xmlPath)));//根据类型得到XML的赔率
                            wincents = Convert.ToInt64(Convert.ToInt64(model.Tables[0].Rows[i]["BuyCents"]) * odds);  //计算赢的钱
                            // new BCW.BLL.Guest().Add(0, 729, "", "Luck28SmallSingle出错了");
                            break;
                        case "Luck28BigDouble":
                            odds = double.Parse(ub.GetSub("Luck28BigDouble", xmlPath));//根据类型得到XML的赔率
                            wincents = Convert.ToInt64(Convert.ToInt64(model.Tables[0].Rows[i]["BuyCents"]) * odds);  //计算赢的钱
                            // new BCW.BLL.Guest().Add(0, 729, "", "Luck28BigDouble出错了");
                            break;
                        case "Luck28SmallDouble":
                            odds = double.Parse(ub.GetSub("Luck28SmallDouble", xmlPath));//根据类型得到XML的赔率
                            wincents = Convert.ToInt64(Convert.ToInt64(model.Tables[0].Rows[i]["BuyCents"]) * odds);  //计算赢的钱
                            // new BCW.BLL.Guest().Add(0, 729, "", "Luck28SmallDouble出错了");
                            break;
                        case "Luck28Secend":
                            odds = double.Parse(ub.GetSub("Luck28Secend", xmlPath));//根据类型得到XML的赔率
                            wincents = Convert.ToInt64(Convert.ToInt64(model.Tables[0].Rows[i]["BuyCents"]) * odds);  //计算赢的钱
                            // new BCW.BLL.Guest().Add(0, 729, "", "Luck28Secend出错了");
                            break;
                        case "Luck28First":
                            odds = double.Parse(ub.GetSub("Luck28First", xmlPath));//根据类型得到XML的赔率
                            wincents = Convert.ToInt64(Convert.ToInt64(model.Tables[0].Rows[i]["BuyCents"]) * odds);  //计算赢的钱
                            // new BCW.BLL.Guest().Add(0, 729, "", "Luck28First出错了");
                            break;
                        case "Luck28Three":
                            odds = double.Parse(ub.GetSub("Luck28Three", xmlPath));//根据类型得到XML的赔率
                            wincents = Convert.ToInt64(Convert.ToInt64(model.Tables[0].Rows[i]["BuyCents"]) * odds);  //计算赢的钱
                            // new BCW.BLL.Guest().Add(0, 729, "", "Luck28Three出错了");
                            break;
                        case "Luck28All":
                            odds = double.Parse(ub.GetSub("Luck28All", xmlPath));//根据类型得到XML的赔率
                            wincents = Convert.ToInt64(Convert.ToInt64(model.Tables[0].Rows[i]["BuyCents"]) * odds);  //计算赢的钱
                            //  new BCW.BLL.Guest().Add(0, 729, "", "Luck28All出错了");
                            break;

                    }
                    #endregion
                    //发系统内线
                    //Response.Write("is robot:" + model.Tables[0].Rows[i][11].ToString()+"<br>");
                    if ((model.Tables[0].Rows[i]["IsRobot"].ToString()) == "0")//不是机械人下注
                    {
                        string GuestSet = ub.GetSub("Luck28GuestSet", xmlPath);
                        if (GuestSet == "0")  //兑奖内线开
                        {

                            // if (model.Tables[0].Rows[i]["WinCent"].ToString() != "0")
                            if (wincents > 0)
                            {
                                int myid = Utils.ParseInt(model.Tables[0].Rows[i]["LuckId"].ToString());
                                new BCW.BLL.Guest().Add(0, Convert.ToInt32(model.Tables[0].Rows[i]["UsID"]), model.Tables[0].Rows[i]["UsName"].ToString(), "恭喜你!你在幸运28第" + new BCW.BLL.Game.Lucklist().GetLucklist(myid).Bjkl8Qihao + "期赢的了" + wincents + ub.Get("SiteBz") + "[url=/bbs/game/luck28.aspx?act=case]马上兑奖[/url]");
                            }
                        }

                        new BCW.BLL.Game.Luckpay().Update(Convert.ToInt32(model.Tables[0].Rows[i]["ID"]), wincents, 1);
                    }
                    else  //机械人兑奖 
                    {
                        int idd = Convert.ToInt32(model.Tables[0].Rows[i]["UsID"]);
                        string chinese = GetChinese(types);//得到中文类型   
                        BCW.Model.Game.Lucklist luckqihao = new BCW.BLL.Game.Lucklist().GetLucklist(Utils.ParseInt(model.Tables[0].Rows[i]["LuckId"].ToString()));
                        //机械人消费记录
                        new BCW.BLL.User().UpdateiGold(idd, new BCW.BLL.User().GetUsName(idd), wincents, "" + "" + "二八第" + luckqihao.Bjkl8Qihao + "期兑奖-标识ID-" + Convert.ToInt32(model.Tables[0].Rows[i]["ID"]) + "");

                        new BCW.BLL.Game.Luckpay().Update(Convert.ToInt32(model.Tables[0].Rows[i]["ID"]), wincents, 2);

                        //new BCW.BLL.User().UpdateiGold(102, new BCW.BLL.User().GetUsName(102), -wincents, "ID" + idd + "二八第" + luckqihao.Bjkl8Qihao + "期" + chinese + "兑奖" + wincents + "(标识ID" + model.Tables[0].Rows[i]["ID"].ToString() + ")");
                    }
                    // Response.Write("赢的钱：" + wincents + "<br/>");
                    allcents = allcents + wincents;//计算玩家一共赢了多少钱
                }
                else
                //if (flag == 0)//不中奖
                {
                    new BCW.BLL.Game.Luckpay().Update(Convert.ToInt32(model.Tables[0].Rows[i]["ID"]), 0, 1);
                }
            }
        } //遍历下注
        return allcents;

        #endregion

    }
    private string GetChinese(string Eng)
    {
        if (Eng == "Luck28Big")
            return "押大";

        if (Eng == "Luck28Small")
            return "押小";

        if (Eng == "Luck28Single")
            return "押单";

        if (Eng == "Luck28Double")
            return "押双";

        if (Eng == "Luck28BigSingle")
            return "押大单";

        if (Eng == "Luck28SmallSingle")
            return "押小单";

        if (Eng == "Luck28BigDouble")
            return "押大双";

        if (Eng == "Luck28SmallDouble")
            return "押小双";

        if (Eng == "Luck28First")
            return "押一段";

        if (Eng == "Luck28Secend")
            return "押二段";

        if (Eng == "Luck28Three")
            return "押三段";

        if (Eng == "Luck28End")
            return "押尾数";

        if (Eng == "Luck28Choose")
            return "押自选";
        else
            return "";
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
                return s;
            #endregion
        }
        else if (cfnum == 3)
        {
            #region
            if (count >= 7)
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
                return s;
            #endregion
        }
        else if (cfnum == 4)
        {
            #region
            if (count >= 9)
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
                return s;
            #endregion
        }
        else if (cfnum == 5)
        {
            #region
            if (count >= 11)
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
                return s;
            #endregion
        }
        else if (cfnum == 6)
        {
            #region
            if (count >= 13)
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
                return s;
            #endregion
        }
        else
            return s;
    }
    #region 旧算法
    //private int suan(int n, string[] a, int s, int count)
    //{
    //    if (n < count)
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
    //        return s;
    //}
    #endregion

    /// <summary>
    /// 修改浮动赔率
    /// </summary>
    /// <param name="num">开奖号码</param>
    public void ChangeOdds()
    {
        double oddssub = Convert.ToDouble(ub.GetSub("Luck28OddsSub", xmlPath));
        string where = "  (Bjkl8Qihao not in (select top 1 Bjkl8Qihao  FROM tb_Lucklist order by Bjkl8Qihao desc))AND DateDiff(dd,EndTime,getdate())=0 AND State=1 order by Bjkl8Qihao desc";
        DataSet model = new BCW.BLL.Game.Lucklist().GetList("TOP(30)*", where);//原20
        string bs = string.Empty;
        string sd = string.Empty;
        for (int i = 0; i < model.Tables[0].Rows.Count; i++)
        {

            int sumnum = Convert.ToInt16(model.Tables[0].Rows[i]["SumNum"]);
            //  Response.Write(model.Tables[0].Rows[i]["Bjkl8Qihao"] + ",&nbsp;开奖号码：" + sumnum + "<br/>");
            #region 判断大小单双
            if (sumnum > 13)//1大2小
            {
                if (string.IsNullOrEmpty(bs))
                {
                    bs = bs + "1";
                }
                else
                {
                    bs = bs + ",1";
                }
            }
            else
            {
                if (string.IsNullOrEmpty(bs))
                {
                    bs = bs + "2";
                }
                else
                {
                    bs = bs + ",2";
                }
            }
            if (sumnum % 2 != 0)//1单2双
            {
                if (string.IsNullOrEmpty(sd))
                {
                    sd = sd + "1";
                }
                else
                {
                    sd = sd + ",1";
                }
            }
            else
            {
                if (string.IsNullOrEmpty(sd))
                {
                    sd = sd + "2";
                }
                else
                {
                    sd = sd + ",2";
                }
            }
            #endregion
        }
        //Response.Write("大小：" + bs + "<br/>");
        //Response.Write("单双：" + sd + "<br/>");
        string[] sa = bs.Split(',');//大小
        string[] sb = sd.Split(',');//单双
        //int ab1 = suan(0, sa, 0, model.Tables[0].Rows.Count - 1);
        //int ab2 = suan(0, sb, 0, model.Tables[0].Rows.Count - 1);
        int ab1 = suan(0, sa, 0, bs.Length, bs.Split(',').Length);
        int ab2 = suan(0, sb, 0, sb.Length, sd.Split(',').Length);

        //Response.Write("大小浮动：" + ab1 * oddssub + "<br/>");
        //Response.Write("单双浮动：" + ab2 * oddssub + "<br/>");
        int num = 24;
        ub xml = new ub();
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置

        #region XML
        double FloatBig = Convert.ToDouble(ub.GetSub("FloatBig", xmlPath));//得到浮动赔率
        double FloatSmall = Convert.ToDouble(ub.GetSub("FloatSmall", xmlPath));
        double FloatSingle = Convert.ToDouble(ub.GetSub("FloatSingle", xmlPath));
        double FloatDouble = Convert.ToDouble(ub.GetSub("FloatDouble", xmlPath));

        double Luck28Big = Convert.ToDouble(ub.GetSub("Luck28Big", xmlPath));//  大赔率
        double Luck28Small = Convert.ToDouble(ub.GetSub("Luck28Small", xmlPath));//小赔率

        double Luck28Single = Convert.ToDouble(ub.GetSub("Luck28Single", xmlPath));
        double Luck28Double = Convert.ToDouble(ub.GetSub("Luck28Double", xmlPath));

        double Luck28BigSingle = Convert.ToDouble(ub.GetSub("Luck28BigSingle", xmlPath));
        double Luck28SmallSingle = Convert.ToDouble(ub.GetSub("Luck28SmallSingle", xmlPath));

        double Luck28BigDouble = Convert.ToDouble(ub.GetSub("Luck28BigDouble", xmlPath));
        double Luck28SmallDouble = Convert.ToDouble(ub.GetSub("Luck28SmallDouble", xmlPath));


        double BSMin = Convert.ToDouble(ub.GetSub("Luck28BSMin", xmlPath));//大小最低赔率
        double BSMax = Convert.ToDouble(ub.GetSub("Luck28BSMax", xmlPath));//大小最高赔率

        double SDMin = Convert.ToDouble(ub.GetSub("Luck28SDMin", xmlPath));//单双最低赔率
        double SDMax = Convert.ToDouble(ub.GetSub("Luck28SDMax", xmlPath));//单双最高赔率

        double Luck28BSSMin = Convert.ToDouble(ub.GetSub("Luck28BSSMin", xmlPath));//大小单最低赔率
        double Luck28BSDMin = Convert.ToDouble(ub.GetSub("Luck28BSDMin", xmlPath));//大小双最低赔率
        #endregion
        double Luck28OddsSub = Convert.ToDouble(ub.GetSub("Luck28OddsSub", xmlPath));//赔率加减位数
        //Luck28BSMin = Luck28BSMin - Luck28OddsSub;
        if (sa[0] == "1")//1大2小
        {
            if (FloatBig >= BSMax)//大浮动到达最大赔率,大浮动不变
            {
                xml.dss["FloatBig"] = BSMax; //最大赔率
                FloatSmall = Luck28Small - ab1 * oddssub;//小的赔率减少
                if (FloatSmall <= BSMin) //小浮动到达最低赔率,小浮动不变
                {
                    xml.dss["FloatSmall"] = BSMin;//最低赔率
                }
                else
                {
                    xml.dss["FloatSmall"] = FloatSmall;//浮动赔率
                }
            }
            else  //浮动赔率
            {
                xml.dss["FloatBig"] = Luck28Big + ab1 * oddssub;//大的赔率增加
                xml.dss["FloatSmall"] = Luck28Small - ab1 * oddssub;//小的赔率减少
            }

        }
        else
        {
            if (FloatSmall >= BSMax)//小浮动到达最大赔率,小浮动不变
            {
                xml.dss["FloatSmall"] = BSMax; //最大赔率
                FloatBig = Luck28Big - ab1 * oddssub;//大的赔率减少
                if (FloatBig <= BSMin)//大浮动到达最低赔率,大浮动不变
                {
                    xml.dss["FloatBig"] = BSMin;//最低赔率
                }
                else
                {
                    xml.dss["FloatBig"] = FloatBig;//浮动赔率
                }
            }
            else
            {
                xml.dss["FloatBig"] = Luck28Big - ab1 * oddssub;//大的赔率-
                xml.dss["FloatSmall"] = Luck28Small + ab1 * oddssub;//小的赔率+
            }
        }
        if (sb[0] == "1")//1单2双
        {
            if (FloatSingle >= SDMax)//单浮动到达最大赔率,单浮动不变
            {
                xml.dss["FloatSingle"] = SDMax; //最大赔率
                FloatDouble = Luck28Double - ab2 * oddssub;//双赔率减少
                if (FloatDouble <= SDMin)//双浮动到达最低赔率，双浮动不变
                {
                    xml.dss["FloatDouble"] = SDMin;//最低赔率
                }
                else
                {
                    xml.dss["FloatDouble"] = FloatDouble;//浮动赔率
                }
            }
            else
            {
                xml.dss["FloatSingle"] = Luck28Single + ab2 * oddssub;//单的赔率+
                xml.dss["FloatDouble"] = Luck28Double - ab2 * oddssub;//双的赔率-
            }
        }
        else
        {
            if (FloatDouble >= SDMax)//双浮动到达最大赔率,双浮动不变
            {
                xml.dss["FloatDouble"] = SDMax; //最大赔率
                FloatSingle = Luck28Single - ab2 * oddssub;//双赔率减少
                if (FloatSingle <= SDMin)//单浮动到达最低赔率，单浮动不变
                {
                    xml.dss["FloatSingle"] = SDMin;//最低赔率
                }
                else
                {
                    xml.dss["FloatSingle"] = FloatSingle;//浮动赔率
                }
            }
            else
            {
                xml.dss["FloatSingle"] = Luck28Single - ab2 * oddssub;//单的赔率-
                xml.dss["FloatDouble"] = Luck28Double + ab2 * oddssub;//双的赔率+
            }

        }
        System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);

        /*
        #region 开大小
        if (num > 13) //开大
        {
            //  Luck28Big = Luck28Big - Luck28OddsSub;
            if ((Luck28Big - Luck28OddsSub) < Luck28BSMin)  //如果开大赔率低于0.3
            {
                xml.dss["Luck28Big"] = Luck28Big;
                xml.dss["Luck28Small"] = Luck28Small;
                //System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            else
            {
                xml.dss["Luck28Big"] = Luck28Big;
                Luck28Small = Luck28Small + Luck28OddsSub;
                xml.dss["Luck28Small"] = Luck28Small;
              //  System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            if (num % 2 == 0) //开大双
            {
                if ((Luck28BigDouble - Luck28OddsSub) < Luck28BSDMin)  //如果开大双赔率低于最低赔率
                {
                    xml.dss["Luck28BigDouble"] = Luck28BigDouble;
                    xml.dss["Luck28SmallDouble"] = Luck28SmallDouble;
                //    System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                }
                else
                {
                    xml.dss["Luck28BigDouble"] = Luck28BigDouble;
                    Luck28SmallDouble = Luck28SmallDouble + Luck28OddsSub;
                    xml.dss["Luck28SmallDouble"] = Luck28SmallDouble;
               //     System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                }
            }
            else   //开大单
            {
                //Luck28BigSingle = Luck28BigSingle - Luck28OddsSub;
                if ((Luck28BigSingle - Luck28OddsSub) < Luck28BSSMin)  //如果开大单赔率低于最低赔率
                {
                    xml.dss["Luck28BigSingle"] = Luck28BigSingle;
                    xml.dss["Luck28SmallSingle"] = Luck28SmallSingle;
                 //   System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                }
                else
                {
                    xml.dss["Luck28BigSingle"] = Luck28BigSingle;
                    Luck28SmallSingle = Luck28SmallSingle + Luck28OddsSub;
                    xml.dss["Luck28SmallSingle"] = Luck28SmallSingle;
                 //   System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                }
            }
        }
        else  //开小
        {
            //Luck28Small = Luck28Small - Luck28OddsSub;
            if ((Luck28Small - Luck28OddsSub) < Luck28BSMin)  //如果开大赔率低于0.3
            {
                xml.dss["Luck28Small"] = Luck28Small;
                xml.dss["Luck28Big"] = Luck28Big;
                //System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            else
            {
                xml.dss["Luck28Small"] = Luck28Small;
                Luck28Big = Luck28Big + Luck28OddsSub;
                xml.dss["Luck28Big"] = Luck28Big;
            //    System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }

            if (num % 2 == 0)  //开小双
            {
                //Luck28SmallDouble = Luck28SmallDouble - Luck28OddsSub;
                if ((Luck28SmallDouble - Luck28OddsSub) < Luck28BSDMin)  //如果开小双赔率低于最低赔率
                {
                    xml.dss["Luck28SmallDouble"] = Luck28SmallDouble;
                    xml.dss["Luck28BigDouble"] = Luck28BigDouble;
                  //  System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                }
                else
                {
                    xml.dss["Luck28SmallDouble"] = Luck28SmallDouble;
                    Luck28BigDouble = Luck28BigDouble + Luck28OddsSub;
                    xml.dss["Luck28BigDouble"] = Luck28BigDouble;
                 //   System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                }
            }
            else        //开小单
            {
                //Luck28SmallSingle = Luck28SmallSingle - Luck28OddsSub;
                if ((Luck28SmallSingle - Luck28OddsSub) < Luck28BSSMin)  //如果开大单赔率低于最低赔率
                {
                    xml.dss["Luck28SmallSingle"] = Luck28SmallSingle;
                    xml.dss["Luck28BigSingle"] = Luck28BigSingle;
                //    System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                }
                else
                {
                    xml.dss["Luck28SmallSingle"] = Luck28SmallSingle;
                    Luck28BigSingle = Luck28BigSingle + Luck28OddsSub;
                    xml.dss["Luck28BigSingle"] = Luck28BigSingle;
                  //  System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                }
            }

        }
        #endregion
        #region  开单双
        if (num % 2 == 0)  //开双
        {
            //   Luck28Double = Luck28Double - Luck28OddsSub;
            if ((Luck28Double - Luck28OddsSub) < Luck28SDMin)  //如果开双赔率低于0.3
            {
                xml.dss["Luck28Double"] = Luck28Double;
                xml.dss["Luck28Single"] = Luck28Single;
                //System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);

            }
            else
            {
                xml.dss["Luck28Double"] = Luck28Double;
                Luck28Single = Luck28Single + Luck28OddsSub;
                xml.dss["Luck28Single"] = Luck28Single;
             //   System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
        }
        else  //开单
        {
            // Luck28Single = Luck28Single - Luck28OddsSub;
            if ((Luck28Single - Luck28OddsSub) < Luck28SDMin)  //如果开双赔率低于0.3
            {
                xml.dss["Luck28Single"] = Luck28Single;
                xml.dss["Luck28Double"] = Luck28Double;
             //   System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            else
            {
                xml.dss["Luck28Single"] = Luck28Single;
                Luck28Double = Luck28Double + Luck28OddsSub;
                xml.dss["Luck28Double"] = Luck28Double;
               // System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
        }
        #endregion
        */
    }
    /// <summary>
    /// 更新期数
    /// </summary>
    /// <returns></returns>
    public string UpdateState()
    {
        string OnTime = ub.GetSub("Luck28OnTime", xmlPath);//09:00-23:55
        if (OnTime != "")
        {
            if (Utils.IsRegex(OnTime, @"^[0-9]{2}:[0-9]{2}-[0-9]{2}:[0-9]{2}$"))
            {

                string[] temp = OnTime.Split("-".ToCharArray());
                DateTime dt1 = Convert.ToDateTime(temp[0]);//09:00
                                                           // dt1 = dt1.AddMinutes(-5);
                DateTime dt2 = Convert.ToDateTime(temp[1]);//23:56
                DateTime lasttime = Convert.ToDateTime("23:55");
                string state = string.Empty;
                // if (DateTime.Now <= dt2 && DateTime.Now >= dt1)
                {

                    string dt3 = DateTime.Now.Subtract(dt1).Duration().TotalMinutes.ToString();
                    string[] time = dt3.Split('.');
                    string timeone = time[0];
                    int result = Convert.ToInt32(timeone) % Utils.ParseInt(ub.GetSub("Luck28CycleMin", xmlPath));
                    // int getqh = Convert.ToInt32(new BCW.BLL.Game.Lucklist().GetPanduan());
                    int getqh = Utils.ParseInt(ub.GetSub("Luck28StartQH", xmlPath));
                    Response.Write("得到数据库当天开始期数:" + getqh + "<br/>");
                    int current = 0;
                    int change = Utils.ParseInt(ub.GetSub("Luck28qhchange", xmlPath));
                    if (change == 0)
                    {
                        current = (Convert.ToInt32(timeone) / 5) + getqh;//当前正在开奖的期号  
                    }
                    else
                    {
                        current = getqh;
                    }
                    Response.Write("当天第几期正在开奖:" + ((Convert.ToInt32(timeone) / 5)) + "<br/>");
                    Response.Write("现在开奖期数是:" + current + "<br/>");
                    Response.Write("change:" + change + "<br/>");
                    ub xml = new ub();
                    Application.Remove(xmlPath);//清缓存
                    xml.ReloadSub(xmlPath); //加载配置
                                            //   if (result == 0)  //余数为0 添加新一期 
                    {
                        BCW.Model.Game.Lucklist addluck = new BCW.Model.Game.Lucklist();
                        addluck.SumNum = 0;
                        addluck.PostNum = "";
                        if (DateTime.Now > lasttime)  //如果时间大于23:55分，在下一期开始时间在第二天9点，开奖时间为9:05
                        {
                            //   Response.Write("change:if" + change + "<br/>");
                            addluck.BeginTime = dt1.AddDays(1);
                            addluck.EndTime = dt1.AddDays(1).AddMinutes(Utils.ParseInt(ub.GetSub("Luck28CycleMin", xmlPath)));
                            addluck.panduan = current.ToString();//修改当开始的期数
                            if (change == 0)
                            {
                                //    Response.Write("change:if" + change + "<br/>");
                                xml.dss["Luck28StartQH"] = current;  //修改下一天开始期号
                                xml.dss["Luck28qhchange"] = 1;
                                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                            }
                        }
                        else
                        {
                            //  Response.Write("change:else" + change + "<br/>");
                            xml.dss["Luck28qhchange"] = 0;
                            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                            addluck.BeginTime = DateTime.Now;
                            addluck.EndTime = dt1.AddMinutes((Convert.ToInt32(timeone) + 5));
                            addluck.panduan = getqh.ToString();
                        }
                        addluck.Bjkl8Qihao = current;
                        addluck.LuckCent = 0;
                        addluck.Pool = 0;
                        addluck.BeforePool = 1;
                        addluck.State = 0;
                        BCW.Model.Game.Lucklist last = new BCW.BLL.Game.Lucklist().GetLucklistState();


                        string[] a = last.EndTime.ToString().Split(' ');
                        DateTime add1 = Convert.ToDateTime(a[0] + " 00:00:00").AddDays(1);
                        Response.Write("add1:" + add1 + "<br/>");
                        Response.Write("qihao:" + qihao + "<br/>");
                        Response.Write("last.Bjkl8Qihao" + last.Bjkl8Qihao + "<br/>");
                        if (!new BCW.BLL.Game.Lucklist().ExistsBJQH(current))  //如果不存在该期号，存入数据库
                        {
                            int j = 0;
                            while ((qihao - last.Bjkl8Qihao) > 19)  //刷新机出问题了
                            {

                                BCW.Model.Game.Lucklist add = new BCW.Model.Game.Lucklist();
                                add.SumNum = 0;
                                add.PostNum = "";
                                add.BeginTime = last.EndTime.AddMinutes(j * 5);
                                Response.Write("add.BeginTime:" + add.BeginTime + "---");
                                add.EndTime = last.EndTime.AddMinutes(j * 5 + 5);
                                Response.Write("add.EndTime:" + add.EndTime + "---");
                                add.panduan = getqh.ToString();
                                DateTime BeginTime = Convert.ToDateTime(a[0] + " 09:00:00");
                                if (add.EndTime == add1)//添加的期数是当天的最后一期
                                {

                                    add.panduan = (getqh + 179).ToString();

                                    add.BeginTime = BeginTime.AddDays(1);
                                    last.BeginTime = BeginTime.AddDays(1);
                                    Response.Write("add.BeginTime:" + add.BeginTime + "---");

                                    add.EndTime = BeginTime.AddDays(1).AddMinutes(5);
                                    last.EndTime = BeginTime.AddDays(1).AddMinutes(5);
                                    Response.Write("add.EndTime:" + add.EndTime + "---");
                                    a = last.EndTime.ToString().Split(' ');
                                    add1 = Convert.ToDateTime(a[0] + " 00:00:00").AddDays(1);
                                    //    Response.Write("BeginTime" + BeginTime + "---");
                                    //xml.dss["Luck28StartQH"] = (getqh + 179);  //修改下一天开始期号
                                    // System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                                }
                                add.Bjkl8Qihao = last.Bjkl8Qihao + 1;
                                add.LuckCent = 0;
                                add.Pool = 0;
                                add.BeforePool = 1;
                                add.State = 0;
                                last.Bjkl8Qihao += 1;
                                if (add.BeginTime == BeginTime.AddDays(1))//添加的期数是当天的最后一期
                                {
                                    j = 0;
                                }
                                else
                                {
                                    j++;
                                }
                                Response.Write("j:" + j + " ");
                                //   new BCW.BLL.Game.Lucklist().Add(add);//增加最新一期   
                                Response.Write("add.Bjkl8Qihao" + add.Bjkl8Qihao + "<br/>");
                            }
                            //   new BCW.BLL.Game.Lucklist().Add(addluck);//增加最新一期                      
                        }
                    }
                }
            }
        }
        return "0";
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
    /// <summary>
    /// 抓取的一行数据
    /// </summary>
    public class Row
    {
        public string expect;
        public string opencode;
        public string opentime;
    }
    /// <summary>
    /// 使用list抓取
    /// </summary>
    /// <returns></returns>
    public XmlDocument GetListUrl()
    {

        string url = "http://c.apiplus.net/newly.do?token=5a8ebe52461354bc&code=bjkl8&rows=20";
        XmlDocument xml = new XmlDocument();
        try
        {
            xml.Load(url);
            Response.Write("<b>数据获取中！！！！ ok1</b><br/>");
        }
        catch
        {
            xml.Load("http://f.apiplus.cn/bjkl8-20.xml");
            Response.Write("<b>数据获取失败！！！！使用备用接口</b><br/>");
        }
        return xml;

    }
    /// <summary>
    /// 将xml解析成list集合
    /// </summary>
    /// <returns></returns>
    public List<Row> TranList()
    {
        List<Row> list = new List<Row>();
        try
        {
            XmlDocument dom = GetListUrl();//返回一个XML文档
            XmlNodeList nodelist = dom.SelectSingleNode("/xml").ChildNodes; //返回20行数据
            foreach (XmlNode node in nodelist)
            {
                Row row = new Row();
                XmlElement xe = (XmlElement)node;//将子节点类型转换为xmlelement类型
                row.expect = xe.GetAttribute("expect").ToString();
                row.opencode = xe.GetAttribute("opencode").ToString();
                row.opentime = xe.GetAttribute("opentime").ToString();
                list.Add(row);
            }
        }
        catch (Exception e)
        {
            Response.Write("erro1");
        }
        return list;
    }
    /// <summary>
    /// 新的抓取，前提数据库最新一条数据正确
    /// </summary>
    /// 
    public void Luck28List()
    {
        List<Row> list = TranList();//得到抓取数据
        if (list.Count > 1)
        {
            DateTime dt1 = Convert.ToDateTime("09:00");//09:00
            DateTime dt2 = Convert.ToDateTime("23:55");//"23:55"
            string dt3 = DateTime.Now.Subtract(dt1).Duration().TotalMinutes.ToString();
            string[] time1 = dt3.Split('.');
            string timeone = time1[0];
            int opening = (Convert.ToInt32(timeone) / 5);//当天中第几期正在开奖
            Response.Write("当天中第几期正在开奖：" + opening + "-<br/>");
            Row xinqi = list[0];
            Response.Write("xinqi.expect:" + xinqi.expect + "-<br/>");

            int tianjia = int.Parse(xinqi.expect) + 1;//正在开奖期数

            Response.Write("最新一期" + tianjia + "-<br/>");
            Response.Write("当天开始的期数" + (tianjia - opening) + "-<br/>");
            BCW.Model.Game.Lucklist xin = new BCW.Model.Game.Lucklist();
            xin.SumNum = 0;
            xin.PostNum = "";
            //xin.BeginTime = dt1.AddMinutes(opening * 5);
            //xin.EndTime = dt1.AddMinutes(opening * 5 + 5);
            string[] day = xinqi.opentime.Split(' ');//天；  去秒数
            string[] hm = day[1].Split(':');
            string mt = day[0] + " " + hm[0] + ":" + hm[1] + ":00";
            //   DateTime mlast = Convert.ToDateTime(mt);
            xin.BeginTime = Convert.ToDateTime(mt);
            xin.EndTime = Convert.ToDateTime(mt).AddMinutes(5);

            xin.panduan = (tianjia - opening).ToString();
            //    Response.Write("dt2 == xin.BeginTime:" + (dt2 == xin.BeginTime) + "-<br/>");
            //ub xml = new ub();
            //Application.Remove(xmlPath);//清缓存
            //xml.ReloadSub(xmlPath); //加载配置
            if (dt2 <= xin.BeginTime)  //添加最新一期是下一天第一期
            {
                xin.BeginTime = dt1.AddDays(1);
                xin.EndTime = dt1.AddDays(1).AddMinutes(5);
                xin.panduan = tianjia.ToString();
                //   xml.dss["Luck28StartQH"] = tianjia;  //修改下一天开始期号
                //  System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            else
            {
                // xml.dss["Luck28StartQH"] = tianjia - opening;  //修改下一天开始期号
                //   System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            //  Response.Write("下一天开奖期数:" + ub.GetSub("Luck28StartQH", xmlPath) + "-<br/>");
            // Response.Write("BeginTime:" + xin.BeginTime + "-");
            //   Response.Write("EndTime:" + xin.EndTime + "-<br/>");
            xin.LuckCent = 0;
            xin.Pool = 0;
            xin.BeforePool = 1;
            xin.State = 0;

            //   Response.Write("panduan:" + xin.panduan + "-<br/>");
            xin.Bjkl8Qihao = tianjia;
            //   Response.Write("Bjkl8Qihao:" + xin.Bjkl8Qihao + "-<br/>");
            if (!new BCW.BLL.Game.Lucklist().ExistsBJQH(tianjia))  //如果不存在该期号，存入数据库
            {
                Response.Write("ok1<br/>");
                new BCW.BLL.Game.Lucklist().Add(xin);//增加最新一期   
            }
            //   Response.Write("<br/>");
            int temp = 0;
            int flag = 0;
            foreach (Row a in list)
            {
                if (!new BCW.BLL.Game.Lucklist().ExistsBJQH(int.Parse(a.expect)))  //如果不存在该期号，存入数据库
                {
                    BCW.Model.Game.Lucklist last = new BCW.BLL.Game.Lucklist().GetLucklistSecond();

                    string[] a1 = last.EndTime.ToString().Split(' ');
                    DateTime time = Convert.ToDateTime(a1[0] + " 23:55:00");
                    int j = 0;
                    //    Response.Write("time:" + time + "---");
                    //Response.Write("(int.Parse(a.expect):" + int.Parse(a.expect) + "---");
                    //Response.Write("temp:" + temp + "---");
                    //Response.Write("last.Bjkl8Qihao:" + last.Bjkl8Qihao + "---");
                    //Response.Write("while:" + (((int.Parse(a.expect) - last.Bjkl8Qihao) > 20) && temp == 0) + "---");
                    int maxid = new BCW.BLL.Game.Lucklist().GetCount();//统计数据
                    if (maxid > 1)
                    {
                        while (((int.Parse(a.expect) - last.Bjkl8Qihao) > 20) && temp == 0)  //刷新机出问题了超过20期
                        {
                            BCW.Model.Game.Lucklist add1 = new BCW.Model.Game.Lucklist();
                            add1.SumNum = 0;
                            add1.PostNum = "";
                            add1.BeginTime = last.EndTime.AddMinutes(j * 5);
                            //   Response.Write("add.BeginTime:" + add1.BeginTime + "---");
                            add1.EndTime = last.EndTime.AddMinutes(j * 5 + 5);
                            //   Response.Write("add.EndTime:" + add1.EndTime + "---");
                            add1.panduan = last.panduan;
                            DateTime BeginTime = Convert.ToDateTime(a1[0] + " 09:00:00");
                            //Response.Write("add.BeginTime:" + add1.BeginTime + "---");
                            //Response.Write("time.time:" + time + "---");
                            //Response.Write("add1.EndTime:" + add1.EndTime + "---<br/>");
                            if (add1.BeginTime >= time)//添加的期数是当天的最后一期
                            {
                                add1.panduan = last.Bjkl8Qihao.ToString();

                                add1.BeginTime = BeginTime.AddDays(1);
                                last.BeginTime = BeginTime.AddDays(1);
                                //   Response.Write("add.BeginTime:" + add1.BeginTime + "---");

                                add1.EndTime = BeginTime.AddDays(1).AddMinutes(5);
                                last.EndTime = BeginTime.AddDays(1).AddMinutes(5);
                                //     Response.Write("add.EndTime:" + add1.EndTime + "---");

                                last.panduan = (last.Bjkl8Qihao + 1).ToString();
                                a1 = last.EndTime.ToString().Split(' ');
                                time = Convert.ToDateTime(a1[0] + " 23:55:00");
                                //        Response.Write("time:" + time + "---");
                            }
                            last.Bjkl8Qihao += 1;
                            add1.Bjkl8Qihao = last.Bjkl8Qihao;
                            add1.LuckCent = 0;
                            add1.Pool = 0;
                            add1.BeforePool = 1;
                            add1.State = 0;
                            if (add1.BeginTime == BeginTime.AddDays(1))//添加的期数是当天的最后一期
                            {
                                j = 0;
                            }
                            else
                            {
                                j++;
                            }
                            //   Response.Write("j:" + j + " ");
                            if (!new BCW.BLL.Game.Lucklist().ExistsBJQH(last.Bjkl8Qihao))  //如果不存在该期号，存入数据库
                            {
                                new BCW.BLL.Game.Lucklist().Add(add1);//增加最新一期   
                            }

                            //    Response.Write("add.Bjkl8Qihao" + last.Bjkl8Qihao + "<br/>");
                        }
                    }
                    temp = 1;

                    BCW.Model.Game.Lucklist add = new BCW.Model.Game.Lucklist();
                    string[] opencode = a.opencode.Split('+');
                    int[] sums = CalNum(opencode[0]);
                    add.SumNum = sums[3];
                    //  Response.Write("SumNum:" + sums[3] + "-");
                    add.PostNum = sums[0] + "+" + sums[1] + "+" + sums[2];
                    //  Response.Write("PostNum:" + sums[0] + "+" + sums[1] + "+" + sums[2] + "-");
                    DateTime endtime = Convert.ToDateTime(a.opentime);
                    add.BeginTime = endtime.AddMinutes(-5);
                    //   Response.Write("BeginTime:" + add.BeginTime + "-");
                    add.EndTime = endtime;
                    //   Response.Write("EndTime:" + add.EndTime + "-<br/>");
                    add.panduan = a.expect;
                    add.Bjkl8Qihao = int.Parse(a.expect);
                    //    Response.Write("add.Bjkl8Qihao:" + a.expect + "-<br/>");
                    add.LuckCent = 0;
                    add.Pool = 0;
                    add.BeforePool = 1;
                    add.State = 1;
                    add.Bjkl8Nums = opencode[0];
                    new BCW.BLL.Game.Lucklist().Add2(add);//增加最新一期   
                    Response.Write("ok1<br/>");


                }
                else  //存在数据->开奖
                {
                    string[] opencode = a.opencode.Split('+');
                    int[] sums = CalNum(opencode[0]);
                    long allcents = duijiang(sums[3], new BCW.BLL.Game.Lucklist().GetID(int.Parse(a.expect)));//遍历数据兑奖
                    BCW.Model.Game.Lucklist luck1 = new BCW.Model.Game.Lucklist();
                    luck1.SumNum = sums[3];
                    luck1.Bjkl8Qihao = int.Parse(a.expect);
                    luck1.PostNum = sums[0] + "+" + sums[1] + "+" + sums[2];
                    luck1.LuckCent = new BCW.BLL.Game.Luckpay().GetSumBuyCent(new BCW.BLL.Game.Lucklist().GetID(int.Parse(a.expect)), sums[3]);
                    luck1.State = 1;
                    luck1.Bjkl8Nums = opencode[0];
                    //  luck1.Bjkl8Qihao = Utils.ParseInt(GetStageqihao()) + 2;
                    new BCW.BLL.Game.Lucklist().Update3(luck1);//更新最新一期

                    //  string where = "  (Bjkl8Qihao not in (select top 1 Bjkl8Qihao  FROM tb_Lucklist order by Bjkl8Qihao desc))order by Bjkl8Qihao desc";
                    //开始修改浮动赔率
                    if (flag == 0)
                    {
                        ChangeOdds();
                        //DataSet model = new BCW.BLL.Game.Lucklist().GetList("TOP(20)*", where);
                        //for (int i = 0; i < model.Tables[0].Rows.Count; i++)
                        //{
                        //    int num = Convert.ToInt16(model.Tables[0].Rows[i]["SumNum"]);
                        //    Response.Write(model.Tables[0].Rows[i]["Bjkl8Qihao"] +",&nbsp;开奖号码："+ num + "<br/>");
                        //}
                        flag = 1;
                    }


                }
            }
        }
    }
    /// <summary>
    /// 取得幸运28 抓取网页数据幸运28抓取
    /// </summary>
    public string GetNewsUrl()
    {
        string strUrl = "http://c.apiplus.net/newly.do?token=5a8ebe52461354bc&code=bjkl8&rows=1";
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
            System.IO.StreamReader sr = new System.IO.StreamReader(stream, Encoding.GetEncoding("gb2312"));
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


    /// <summary>
    /// 计算出最后的和
    /// </summary>
    /// <returns></returns>
    public int[] CalNum(string getresult)
    {
        string result = getresult;//获取的号码
                                  //  string result = GetStageS();//获取的号码
        if (result.Contains(","))
        {
            string[] nums = result.Split(",".ToCharArray());
            int one = 0;
            int two = 0;
            int three = 0;
            if (nums.Length > 1)
            {
                for (int i = 0; i < 6; i++)
                {
                    int aa = Utils.ParseInt(nums[i]);
                    one = one + aa;
                }
                one = one % 10;//算出第1个值
                for (int j = 6; j < 12; j++)
                {
                    int bb = Utils.ParseInt(nums[j]);
                    two = two + bb;
                }
                two = two % 10;//算出第2个值
                for (int k = 12; k < 18; k++)
                {
                    three = three + Utils.ParseInt(nums[k]);
                }
                three = three % 10;//算出第3个值
            }
            int all = one + two + three;//总的数值
            int[] getback = { one, two, three, all };
            return getback;
        }
        else
        {
            int[] error = { 11, 11 };
            return error;
        }

    }

    /// <summary>
    /// 得到最新一期开奖号码
    /// </summary>
    public string GetStageS()
    {
        String s = GetNewsUrl();
        String stage = @"opencode=([\s\S]+?)opentime=";

        Match stages = Regex.Match(s, stage);
        //Match haoma = Regex.Match(stages.Value, @"opencode=([\s\S]+?)");
        string result = string.Empty;
        MatchCollection num33 = Regex.Matches(stages.Value, @"[\d]{2}");
        int i = 0;
        if (num33.Count > 0)
        {

            foreach (Match match in num33)
            {
                if (i < 20)  //只要前20个数
                {
                    if (string.IsNullOrEmpty(result))
                    {
                        if (match.Value != null)
                        {
                            result = result + match.Value.Trim();
                        }
                    }
                    else
                    {
                        if (match.Value != null)
                        {
                            result = result + "," + match.Value.Trim();
                        }
                    }
                }
                i++;
            }
        }
        if (result != null)
        {
            return result;
        }
        else
        {
            return "0";
        }
    }

    /// <summary>
    /// 得到抓取网页最新一期开奖期号
    /// </summary>
    public string GetStageqihao()
    {
        String s = GetNewsUrl();
        Match qihao = Regex.Match(s, @"[\d]{6}");//期号

        if (qihao.Value != null)
        {
            return qihao.Value;
        }
        else
        {
            return "0";
        }
    }
    /// <summary>
    /// 得到抓取网页上一期开奖期号
    /// </summary>
    public string GetLastqihao()
    {
        String s = GetNewsUrl();
        Match qihao = Regex.Match(s, @"[\d]{6}");//期号
        MatchCollection haoma = Regex.Matches(s, @"[\d]{6}");
        string lasthaoma = string.Empty;
        foreach (Match match in haoma)
        {
            if (match.Value != null)
            {
                if (match.Value != GetStageqihao())  //如果不是最新一起号码
                {
                    lasthaoma = match.Value;

                }
            }

        }
        if (lasthaoma != null)
        {
            return lasthaoma;
        }
        else
        {
            return "0";
        }
    }
    /// <summary>
    /// 开奖时间
    /// </summary>
    /// <returns></returns>
    private bool IsOpen()
    {
        bool IsOpen = true;
        string OnTime = ub.GetSub("Luck28OnTime", xmlPath);
        if (OnTime != "")
        {
            if (Utils.IsRegex(OnTime, @"^[0-9]{2}:[0-9]{2}-[0-9]{2}:[0-9]{2}$"))
            {
                string[] temp = OnTime.Split("-".ToCharArray());
                DateTime dt2 = Convert.ToDateTime("23:57");
                DateTime dt1 = Convert.ToDateTime("08:56");
                //DateTime dt1 = Convert.ToDateTime(temp[0]);
                //DateTime dt2 = Convert.ToDateTime(temp[1]);
                if (DateTime.Now > dt1 && DateTime.Now < dt2)
                {
                    IsOpen = true;
                }
                else
                {
                    IsOpen = false;
                }
            }
        }
        return IsOpen;
    }

}
