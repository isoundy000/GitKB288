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

/// <summary>
/// 蒙宗将 20161004 机器人重新管理
/// 蒙宗将 20161006 重写大小单双赔率
/// 蒙宗将 20161018 机器人优化
///  蒙宗将 20161019 机器人优化
///  蒙宗将 20161029 优化机器人
/// </summary>

public partial class bbs_game_HP3RoBot : System.Web.UI.Page
{
    protected string xmlPath = "/Controls/HappyPoker3.xml";
    protected string GameName = ub.GetSub("HP3Name", "/Controls/HappyPoker3.xml");

    protected void Page_Load(object sender, EventArgs e)
    {
        if (ub.GetSub("HP3IsBot", xmlPath) == "0")
        {
            Response.Write("" + GameName + "_机器人处于关闭状态close1");
        }
        else
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            //防止缓存
            Response.Buffer = true;
            Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
            Response.Expires = 0;
            Response.CacheControl = "no-cache";
            string OnTime = "00:01-23:59";

            RobotCase();

            if (OnTime != "")
            {
                if (Utils.IsRegex(OnTime, @"^[0-9]{2}:[0-9]{2}-[0-9]{2}:[0-9]{2}$"))
                {
                    string[] temp = OnTime.Split("-".ToCharArray());
                    DateTime dt1 = Convert.ToDateTime(temp[0]);
                    DateTime dt2 = Convert.ToDateTime(temp[1]);
                    if (DateTime.Now > dt2 || DateTime.Now < dt1)
                    {
                        Response.Write("机器人下班了!close1");
                    }
                    else
                    {
                        try
                        {
                            Robot();
                            Response.Write("" + GameName + "机器人玩的很开心!ok1<br />");
                        }
                        catch (Exception er)
                        {
                            //  //  Response.Write("机器人出现了问题!error1" + er);
                            //    // new BCW.BLL.Guest().Add(1, 726, "将似沙", "" + GameName + "机器人出错了：" + er.ToString() + "");//内线提示出错测试用
                        }
                    }
                }
            }

            Response.Write("<b>上次获取时间：</b>" + DateTime.Now + "<br />");
            stopwatch.Stop();
            Response.Write("<font color=\"black\">" + "总耗时:" + stopwatch.Elapsed.TotalSeconds + "秒");
        }
    }
    //随机从小到大的数组
    private void RandomKDiffer(int k, int[] arrayK)
    {
        System.Threading.Thread.Sleep(50);//当前休眠50毫秒
        int i = 0;
        int a, j;
        Random random = new Random();
        while (i < k)
        {
            a = random.Next(0, 13);
            for (j = 0; j < i; j++)
            {
                if (a == arrayK[j])
                {
                    break;
                }
            }
            if (j == i)
            {
                arrayK[i] = a;
                i++;
            }
        }
        for (int ii = 0; ii < k; ii++)
        {
            for (int jj = ii + 1; jj < k; jj++)
            {
                if (arrayK[jj] < arrayK[ii])
                {
                    int temp = arrayK[ii];
                    arrayK[ii] = arrayK[jj];
                    arrayK[jj] = temp;
                }
            }
        }
    }
    //读取xml中数据
    private string XML(int Types)
    {
        string xml = null;
        switch (Types)
        {
            case 1:
                xml = ub.GetSub("HP3TH1", xmlPath);
                break;
            case 2:
                xml = ub.GetSub("HP3TH2", xmlPath);
                break;
            case 3:
                xml = ub.GetSub("HP3THS1", xmlPath);
                break;
            case 4:
                xml = ub.GetSub("HP3THS2", xmlPath);
                break;
            case 5:
                xml = ub.GetSub("HP3SZ1", xmlPath);
                break;
            case 6:
                xml = ub.GetSub("HP3SZ2", xmlPath);
                break;
            case 7:
                xml = ub.GetSub("HP3BZ1", xmlPath);
                break;
            case 8:
                xml = ub.GetSub("HP3BZ2", xmlPath);
                break;
            case 9:
                xml = ub.GetSub("HP3DZ1", xmlPath);
                break;
            case 10:
                xml = ub.GetSub("HP3DZ2", xmlPath);
                break;
            case 11:
                xml = ub.GetSub("HP3RX1", xmlPath);
                break;
            case 12:
                xml = ub.GetSub("HP3RX2", xmlPath);
                break;
            case 13:
                xml = ub.GetSub("HP3RX3", xmlPath);
                break;
            case 14:
                xml = ub.GetSub("HP3RX4", xmlPath);
                break;
            case 15:
                xml = ub.GetSub("HP3RX5", xmlPath);
                break;
            case 16:
                xml = ub.GetSub("HP3RX6", xmlPath);
                break;
            case 21:
                xml = ub.GetSub("HP3DA", xmlPath);
                break;
            case 22:
                xml = ub.GetSub("HP3XIAO", xmlPath);
                break;
            case 23:
                xml = ub.GetSub("HP3DAN", xmlPath);
                break;
            case 24:
                xml = ub.GetSub("HP3SHUANG", xmlPath);
                break;
            case 25:
                xml = ub.GetSub("HP3ZUIDAMAX", xmlPath);
                break;
            case 26:
                xml = ub.GetSub("HP3FUDONG", xmlPath);
                break;
            case 27:
                xml = ub.GetSub("HP3DayChaMax", xmlPath);
                break;

        }
        return xml;
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

    //得到出动的ID
    private int GetUsID()
    {
        int UsID = 0;
        string PlayUsID = ub.GetSub("HP3ROBOTID", xmlPath);
        string[] sNum = Regex.Split(PlayUsID, "#");
        Random rd = new Random();
        try
        {
            UsID = Convert.ToInt32((sNum[rd.Next(sNum.Length)]).Replace(" ", ""));
        }
        catch { }
        return UsID;
    }
    //机器人ROBOT
    public void Robot()
    {
        string buycount = ub.GetSub("HP3ROBOTBUY", xmlPath);
        int buycou = Convert.ToInt32(buycount);
        string RoBotCost1 = ub.GetSub("HP3ROBOTCOST", xmlPath);

        //得到投注的酷币
        string[] ppp = RoBotCost1.Split('#');
        Random ran = new Random();
        int k = ran.Next(0, (ppp.Length));
        long RoBotCost = Convert.ToInt32(ppp[k]);
        int meid = GetUsID();//下注ID

        ChanageOnline(meid);
        BCW.HP3.Model.HP3_kjnum model2 = new BCW.HP3.Model.HP3_kjnum();
        model2 = new BCW.HP3.BLL.HP3_kjnum().GetListLast();
        int dnu = int.Parse(model2.datenum);
        DateTime kjtime = model2.datetime;

        BCW.HP3.Model.HP3Buy modelBuy = new BCW.HP3.Model.HP3Buy();
        int[] xx = new int[] { 1, 6, 7, 9, 11, 13, 15, 17 };
        string[] xx2 = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
        int x;
        Random r = new Random();
        x = r.Next(0, 8);
        modelBuy.BuyType = xx[x];
        modelBuy.BuyID = meid;
        modelBuy.BuyTime = DateTime.Now;
        modelBuy.BuyDate = dnu.ToString();
        Random Od = new Random();
        int Odd = Od.Next(1, 99);
        int zhu = 1;
        System.Threading.Thread.Sleep(100);//当前休眠100毫秒
        if (modelBuy.BuyType == 1)
        {
            int x2;
            Random r2 = new Random();
            x2 = r2.Next(1, 52);
            modelBuy.BuyNum = x2.ToString();
            modelBuy.BuyMoney = Convert.ToInt64(RoBotCost) * Odd;
            modelBuy.BuyZhu = 1;
        }
        else if (modelBuy.BuyType == 17)
        {
            string x3 = string.Empty;
            Random r3 = new Random();
            x3 = r3.Next(1, 5).ToString();
            modelBuy.BuyNum = x3;
            modelBuy.BuyMoney = Convert.ToInt64(RoBotCost) * Odd;
            modelBuy.BuyZhu = 1;
        }
        else if (modelBuy.BuyType == 6)
        {
            int x6;
            Random r6 = new Random();
            x6 = r6.Next(1, 14);
            int[] arrayK = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            RandomKDiffer(x6, arrayK);
            string sbb = "";
            for (int s = 0; s < x6; s++)
            {
                if (s != x6 - 1)
                {
                    sbb = sbb + xx2[arrayK[s]] + ",";
                }
                else
                {
                    sbb = sbb + xx2[arrayK[s]];
                }
            }
            modelBuy.BuyNum = sbb.ToString();
            modelBuy.BuyMoney = Convert.ToInt64(RoBotCost) * Odd;
        }
        else if (modelBuy.BuyType == 7)
        {
            int x6;
            Random r6 = new Random();
            x6 = r6.Next(2, 14);
            int[] arrayK = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            RandomKDiffer(x6, arrayK);
            string sbb = "";
            for (int s = 0; s < x6; s++)
            {
                if (s != x6 - 1)
                {
                    sbb = sbb + xx2[arrayK[s]] + ",";
                }
                else
                {
                    sbb = sbb + xx2[arrayK[s]];
                }
            }
            modelBuy.BuyNum = sbb.ToString();
            modelBuy.BuyMoney = Convert.ToInt64(RoBotCost) * Odd;
        }
        else if (modelBuy.BuyType == 9)
        {
            int x6;
            Random r6 = new Random();
            x6 = r6.Next(3, 14);
            int[] arrayK = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            RandomKDiffer(x6, arrayK);
            string sbb = "";
            for (int s = 0; s < x6; s++)
            {
                if (s != x6 - 1)
                {
                    sbb = sbb + xx2[arrayK[s]] + ",";
                }
                else
                {
                    sbb = sbb + xx2[arrayK[s]];
                }
            }
            modelBuy.BuyNum = sbb.ToString();
            modelBuy.BuyMoney = Convert.ToInt64(RoBotCost) * Odd;
        }
        else if (modelBuy.BuyType == 11)
        {
            int x6;
            Random r6 = new Random();
            x6 = r6.Next(4, 14);
            int[] arrayK = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            RandomKDiffer(x6, arrayK);
            string sbb = "";
            for (int s = 0; s < x6; s++)
            {
                if (s != x6 - 1)
                {
                    sbb = sbb + xx2[arrayK[s]] + ",";
                }
                else
                {
                    sbb = sbb + xx2[arrayK[s]];
                }
            }
            modelBuy.BuyNum = sbb.ToString();
            modelBuy.BuyMoney = Convert.ToInt64(RoBotCost) * Odd;
        }
        else if (modelBuy.BuyType == 13)
        {
            int x6;
            Random r6 = new Random();
            x6 = r6.Next(5, 14);
            int[] arrayK = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            RandomKDiffer(x6, arrayK);
            string sbb = "";
            for (int s = 0; s < x6; s++)
            {
                if (s != x6 - 1)
                {
                    sbb = sbb + xx2[arrayK[s]] + ",";
                }
                else
                {
                    sbb = sbb + xx2[arrayK[s]];
                }
            }
            modelBuy.BuyNum = sbb.ToString();
            modelBuy.BuyMoney = Convert.ToInt64(RoBotCost) * Odd;
        }
        else if (modelBuy.BuyType == 15)
        {
            int x6;
            Random r6 = new Random();
            x6 = r6.Next(6, 14);
            int[] arrayK = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            RandomKDiffer(x6, arrayK);
            string sbb = "";
            for (int s = 0; s < x6; s++)
            {
                if (s != x6 - 1)
                {
                    sbb = sbb + xx2[arrayK[s]] + ",";
                }
                else
                {
                    sbb = sbb + xx2[arrayK[s]];
                }
            }
            modelBuy.BuyNum = sbb.ToString();
            modelBuy.BuyMoney = Convert.ToInt64(RoBotCost) * Odd;
        }
        int ptype = modelBuy.BuyType;
        string Num2 = modelBuy.BuyNum;
        if (ptype == 6)//直1
        {
            int len = Num2.Length;
            zhu = (len + 1) / 2;
        }
        else if (ptype == 7)//直2
        {
            int len = Num2.Length;
            int Anum = (len + 1) / 2;
            switch (Anum)
            {
                case 2:
                    zhu = 1;
                    break;
                case 3:
                    zhu = 3;
                    break;
                case 4:
                    zhu = 6;
                    break;
                case 5:
                    zhu = 10;
                    break;
                case 6:
                    zhu = 15;
                    break;
                case 7:
                    zhu = 21;
                    break;
                case 8:
                    zhu = 28;
                    break;
                case 9:
                    zhu = 36;
                    break;
                case 10:
                    zhu = 45;
                    break;
                case 11:
                    zhu = 55;
                    break;
                case 12:
                    zhu = 66;
                    break;
                case 13:
                    zhu = 78;
                    break;
            }
        }

        else if (ptype == 9)//直3
        {
            int len = Num2.Length;
            int Anum = (len + 1) / 2;
            switch (Anum)
            {
                case 3:
                    zhu = 1;
                    break;
                case 4:
                    zhu = 4;
                    break;
                case 5:
                    zhu = 10;
                    break;
                case 6:
                    zhu = 20;
                    break;
                case 7:
                    zhu = 35;
                    break;
                case 8:
                    zhu = 56;
                    break;
                case 9:
                    zhu = 84;
                    break;
                case 10:
                    zhu = 120;
                    break;
                case 11:
                    zhu = 165;
                    break;
                case 12:
                    zhu = 220;
                    break;
                case 13:
                    zhu = 288;
                    break;
            }
        }
        else if (ptype == 11)//直4
        {
            int len = Num2.Length;
            int Anum = (len + 1) / 2;
            switch (Anum)
            {
                case 4:
                    zhu = 1;
                    break;
                case 5:
                    zhu = 5;
                    break;
                case 6:
                    zhu = 15;
                    break;
                case 7:
                    zhu = 35;
                    break;
                case 8:
                    zhu = 70;
                    break;
                case 9:
                    zhu = 126;
                    break;
                case 10:
                    zhu = 210;
                    break;
                case 11:
                    zhu = 330;
                    break;
                case 12:
                    zhu = 495;
                    break;
                case 13:
                    zhu = 715;
                    break;
            }
        }
        else if (ptype == 13)//直5
        {
            int len = Num2.Length;
            int Anum = (len + 1) / 2;
            switch (Anum)
            {
                case 5:
                    zhu = 1;
                    break;
                case 6:
                    zhu = 6;
                    break;
                case 7:
                    zhu = 21;
                    break;
                case 8:
                    zhu = 56;
                    break;
                case 9:
                    zhu = 126;
                    break;
                case 10:
                    zhu = 252;
                    break;
                case 11:
                    zhu = 462;
                    break;
                case 12:
                    zhu = 792;
                    break;
                case 13:
                    zhu = 1287;
                    break;
            }
        }
        else if (ptype == 15)//直6
        {
            int len = Num2.Length;
            int Anum = (len + 1) / 2;
            switch (Anum)
            {

                case 6:
                    zhu = 1;
                    break;
                case 7:
                    zhu = 7;
                    break;
                case 8:
                    zhu = 28;
                    break;
                case 9:
                    zhu = 84;
                    break;
                case 10:
                    zhu = 210;
                    break;
                case 11:
                    zhu = 462;
                    break;
                case 12:
                    zhu = 924;
                    break;
                case 13:
                    zhu = 1716;
                    break;
            }
        }
        modelBuy.BuyZhu = zhu;
        string buynum = modelBuy.BuyNum;
        if (modelBuy.BuyType == 1)
        {
            switch (buynum)
            {
                case "1":
                case "2":
                case "3":
                case "4":
                    modelBuy.Odds = Convert.ToDecimal(XML(2));
                    break;
                case "5":
                    modelBuy.Odds = Convert.ToDecimal(XML(1));
                    break;
                case "6":
                case "7":
                case "8":
                case "9":
                case "10":
                case "11":
                case "12":
                case "13":
                case "14":
                case "15":
                case "16":
                case "17":
                    modelBuy.Odds = Convert.ToDecimal(XML(6));
                    break;
                case "18":
                    modelBuy.Odds = Convert.ToDecimal(XML(5));
                    break;
                case "19":
                case "20":
                case "21":
                case "22":
                    modelBuy.Odds = Convert.ToDecimal(XML(4));
                    break;
                case "23":
                    modelBuy.Odds = Convert.ToDecimal(XML(3));
                    break;
                case "24":
                case "25":
                case "26":
                case "27":
                case "28":
                case "29":
                case "30":
                case "31":
                case "32":
                case "33":
                case "34":
                case "35":
                case "36":
                    modelBuy.Odds = Convert.ToDecimal(XML(8));
                    break;
                case "37":
                    modelBuy.Odds = Convert.ToDecimal(XML(7));
                    break;
                case "38":
                case "39":
                case "40":
                case "41":
                case "42":
                case "43":
                case "44":
                case "45":
                case "46":
                case "47":
                case "48":
                case "49":
                case "50":
                    modelBuy.Odds = Convert.ToDecimal(XML(10));
                    break;
                case "51":
                    modelBuy.Odds = Convert.ToDecimal(XML(9));
                    break;
            }

        }
        else if (modelBuy.BuyType == 6)
        {
            modelBuy.Odds = Convert.ToDecimal(XML(11));
        }
        else if (modelBuy.BuyType == 7)
        {
            modelBuy.Odds = Convert.ToDecimal(XML(12));
        }
        else if (modelBuy.BuyType == 9)
        {
            modelBuy.Odds = Convert.ToDecimal(XML(13));
        }
        else if (modelBuy.BuyType == 11)
        {
            modelBuy.Odds = Convert.ToDecimal(XML(14));
        }
        else if (modelBuy.BuyType == 13)
        {
            modelBuy.Odds = Convert.ToDecimal(XML(15));
        }
        else if (modelBuy.BuyType == 15)
        {
            modelBuy.Odds = Convert.ToDecimal(XML(16));
        }
        else if (modelBuy.BuyType == 17)
        {
            switch (modelBuy.BuyNum)
            {
                case "1":
                    modelBuy.Odds = Convert.ToDecimal(XML(21));
                    break;
                case "2":
                    modelBuy.Odds = Convert.ToDecimal(XML(22));
                    break;
                case "3":
                    modelBuy.Odds = Convert.ToDecimal(XML(23));
                    break;
                case "4":
                    modelBuy.Odds = Convert.ToDecimal(XML(24));
                    break;
            }

        }


        int count = new BCW.HP3.BLL.HP3Buy().GetRecordCount(" BuyID=" + meid + " and BuyDate='" + modelBuy.BuyDate + "'");

        long big = Convert.ToInt64(ub.GetSub("HP3BigPay", xmlPath));
        while (modelBuy.BuyMoney > big)
        {
            Random Od2 = new Random();
            int Odd2 = Od2.Next(1, 99);
            modelBuy.BuyMoney = Convert.ToInt64(RoBotCost) * Odd2;
        }
        long xPrices = Utils.ParseInt64(ub.GetSub("HP3Price", xmlPath));
        long prices = Convert.ToInt64(modelBuy.BuyMoney * modelBuy.BuyZhu);

        long SmallPay = Utils.ParseInt64(ub.GetSub("HP3SmallPay", xmlPath));
        long BigPay = Utils.ParseInt64(ub.GetSub("HP3BigPay", xmlPath));

        if (prices >= SmallPay && prices <= BigPay)
        {

            if (xPrices > 0)
            {
                DataSet ds = new BCW.HP3.BLL.HP3Buy().GetListByID("BuyMoney", meid, modelBuy.BuyDate);
                int oPrices = 0;
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    int drs = int.Parse(dr[0].ToString());
                    oPrices = oPrices + drs;
                }
                int end2 = int.Parse(GetLastStr(model2.datenum, 2));
                int LiangTing = int.Parse(ub.GetSub("HP3LianTing", xmlPath));

                long gold = new BCW.BLL.User().GetGold(meid);
                string mename = new BCW.BLL.User().GetUsName(meid);


                if (count < buycou && oPrices + prices < xPrices && gold >= 0)
                {
                    if (gold >= prices)
                    {
                        int Sec = Utils.ParseInt(ub.GetSub("HP3Sec", xmlPath));
                        if (kjtime < DateTime.Now.AddSeconds(Sec))
                        {
                            Response.Write("未到下注时间ok1");
                        }
                        else
                        {
                            string xfjl = "";
                            if (modelBuy.BuyType == 1)
                            {
                                xfjl = speChoose(Convert.ToInt32(modelBuy.BuyNum));
                            }
                            else if (modelBuy.BuyType == 17)
                            {
                                switch (modelBuy.BuyNum)
                                {
                                    case "1":
                                        xfjl = "大";
                                        break;
                                    case "2":
                                        xfjl = "小";
                                        break;
                                    case "3":
                                        xfjl = "单";
                                        break;
                                    case "4":
                                        xfjl = "双";
                                        break;
                                }
                            }
                            else
                            {
                                switch (modelBuy.BuyType)
                                {
                                    case 6:
                                        xfjl = "任选一：" + modelBuy.BuyNum;
                                        break;
                                    case 7:
                                        xfjl = "任选二：" + modelBuy.BuyNum;
                                        break;
                                    case 8:
                                        xfjl = "任选二胆拖：" + modelBuy.BuyNum;
                                        break;
                                    case 9:
                                        xfjl = "任选三：" + modelBuy.BuyNum;
                                        break;
                                    case 10:
                                        xfjl = "任选三胆拖：" + modelBuy.BuyNum;
                                        break;
                                    case 11:
                                        xfjl = "任选四：" + modelBuy.BuyNum;
                                        break;
                                    case 12:
                                        xfjl = "任选四胆拖：" + modelBuy.BuyNum;
                                        break;
                                    case 13:
                                        xfjl = "任选五：" + modelBuy.BuyNum;
                                        break;
                                    case 14:
                                        xfjl = "任选五胆拖：" + modelBuy.BuyNum;
                                        break;
                                    case 15:
                                        xfjl = "任选六：" + modelBuy.BuyNum;
                                        break;
                                    case 16:
                                        xfjl = "任选六胆拖：" + modelBuy.BuyNum;
                                        break;
                                }

                            }


                            int id = new BCW.HP3.BLL.HP3Buy().Add(modelBuy);
                            new BCW.BLL.User().UpdateiGold(meid, mename, -prices, "" + GameName + "第" + "[url=./game/HP3.aspx?act=BuyWin&qihaos=" + modelBuy.BuyDate + "&amp;ptype=2]" + modelBuy.BuyDate + "[/url]期买" + xfjl + "投注ID" + id);
                            new BCW.HP3.BLL.HP3Buy().UpdateIsRot(id, 1);
                            new BCW.HP3.BLL.HP3Buy().UpdateWillGet(id, 0);
                            string gameplay = "";
                            switch (modelBuy.BuyType)
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
                            string wText = "在[url=/bbs/game/HP3.aspx]" + GameName + "[/url]《" + gameplay + "》下注**" + "" + ub.Get("SiteBz") + "";// + prices 
                            new BCW.BLL.Action().Add(1003, id, meid, mename, wText);

                        }
                    }
                    else
                    {
                        // if (gold < prices)
                        //更新消费记录
                        BCW.Model.Goldlog modelx = new BCW.Model.Goldlog();
                        modelx.BbTag = 3;
                        modelx.Types = 0;
                        modelx.PUrl = Utils.getPageUrl();//操作的文件名
                        modelx.UsId = meid;
                        modelx.UsName = mename;
                        modelx.AcGold = prices;
                        modelx.AfterGold = gold + prices;//更新后的币数
                        modelx.AcText = "HP3系统机器人自动操作";
                        modelx.AddTime = DateTime.Now;
                        new BCW.BLL.Goldlog().Add(modelx);

                        //给机器人发钱
                        BCW.Data.SqlHelper.ExecuteSql("Update tb_User set iGold=iGold+(" + prices + ") where id=" + meid + "");


                        int Sec = Utils.ParseInt(ub.GetSub("HP3Sec", xmlPath));
                        if (kjtime < DateTime.Now.AddSeconds(Sec))
                        {
                            Response.Write("未到下注时间ok1");
                        }
                        else
                        {
                            string xfjl = "";
                            if (modelBuy.BuyType == 1)
                            {
                                xfjl = speChoose(Convert.ToInt32(modelBuy.BuyNum));
                            }
                            else if (modelBuy.BuyType == 17)
                            {
                                switch (modelBuy.BuyNum)
                                {
                                    case "1":
                                        xfjl = "大";
                                        break;
                                    case "2":
                                        xfjl = "小";
                                        break;
                                    case "3":
                                        xfjl = "单";
                                        break;
                                    case "4":
                                        xfjl = "双";
                                        break;
                                }
                            }
                            else
                            {
                                switch (modelBuy.BuyType)
                                {
                                    case 6:
                                        xfjl = "任选一：" + modelBuy.BuyNum;
                                        break;
                                    case 7:
                                        xfjl = "任选二：" + modelBuy.BuyNum;
                                        break;
                                    case 8:
                                        xfjl = "任选二胆拖：" + modelBuy.BuyNum;
                                        break;
                                    case 9:
                                        xfjl = "任选三：" + modelBuy.BuyNum;
                                        break;
                                    case 10:
                                        xfjl = "任选三胆拖：" + modelBuy.BuyNum;
                                        break;
                                    case 11:
                                        xfjl = "任选四：" + modelBuy.BuyNum;
                                        break;
                                    case 12:
                                        xfjl = "任选四胆拖：" + modelBuy.BuyNum;
                                        break;
                                    case 13:
                                        xfjl = "任选五：" + modelBuy.BuyNum;
                                        break;
                                    case 14:
                                        xfjl = "任选五胆拖：" + modelBuy.BuyNum;
                                        break;
                                    case 15:
                                        xfjl = "任选六：" + modelBuy.BuyNum;
                                        break;
                                    case 16:
                                        xfjl = "任选六胆拖：" + modelBuy.BuyNum;
                                        break;
                                }

                            }


                            int id = 0;
                            try
                            {
                                id = new BCW.HP3.BLL.HP3Buy().Add(modelBuy);
                                new BCW.BLL.User().UpdateiGold(meid, mename, -prices, "" + GameName + "第" + "[url=./game/HP3.aspx?act=BuyWin&qihaos=" + modelBuy.BuyDate + "&amp;ptype=2]" + modelBuy.BuyDate + "[/url]期买" + xfjl + "投注ID" + id);
                                new BCW.HP3.BLL.HP3Buy().UpdateIsRot(id, 1);
                                new BCW.HP3.BLL.HP3Buy().UpdateWillGet(id, 0);
                                string gameplay = "";
                                switch (modelBuy.BuyType)
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
                                string wText = "在[url=/bbs/game/HP3.aspx]" + GameName + "[/url]《" + gameplay + "》下注**" + "" + ub.Get("SiteBz") + "";//+ prices
                                new BCW.BLL.Action().Add(1003, id, meid, mename, wText);
                            }
                            catch { }
                        }

                    }

                    int countall = new BCW.HP3.BLL.HP3Buy().GetRecordCount("  BuyDate='" + modelBuy.BuyDate + "'");
                    Response.Write("第" + modelBuy.BuyDate + "期<br />");
                    Response.Write("机器人总计买了" + countall + "张<br />");

                }
            }
        }
    }
    //机器人兑奖
    public void RobotCase()
    {
        string RoBot = ub.GetSub("HP3ROBOTID", xmlPath);
        string RoBotCost = ub.GetSub("HP3ROBOTCOST", xmlPath);
        string[] RoBotID = RoBot.Split('#');
        MatchCollection leyr = Regex.Matches(RoBot, "#");
        int ele = leyr.Count + 1;
        for (int i = 0; i < ele; i++)
        {
            if (RoBotID[i] != "")
            {
                int meid = Convert.ToInt32(RoBotID[i].Replace(" ", ""));
                DataSet model2 = new BCW.HP3.BLL.HP3Winner().GetList("WinUserID=" + meid + " and WinBool=1");
                for (int i2 = 0; i2 < model2.Tables[0].Rows.Count; i2++)
                {
                    try
                    {
                        int pid = Convert.ToInt32(model2.Tables[0].Rows[0][i2]);
                        if (new BCW.HP3.BLL.HP3Winner().Exists3(pid, meid))
                        {
                            new BCW.HP3.BLL.HP3Winner().RoBotByID(pid);
                            //操作币
                            BCW.HP3.Model.HP3Winner model = new BCW.HP3.BLL.HP3Winner().GetModel(pid);
                            long winMoney = Convert.ToInt64(model.WinMoney);
                            new BCW.BLL.User().UpdateiGold(meid, winMoney, "" + GameName + "-" + "[url=./game/HP3.aspx?act=BuyWin&qihaos=" + model.WinDate + "&amp;ptype=1]" + model.WinDate + "[/url]" + "-兑奖-标识ID" + pid + "");
                            Response.Write("机器人自动兑奖!<br />");
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }
    }
    // 更新会员在线@
    private void ChanageOnline(int UsID)
    {
        int OnTime = 5;
        new BCW.BLL.User().UpdateTime(UsID, OnTime);
    }
    //特殊选择返回
    private string speChoose(int Types)
    {
        string s1 = string.Empty;
        if (Types == 1)
            s1 = "<b>黑桃同花</b>";
        else if (Types == 2)
            s1 = "<b style=\"color:red\">红桃同花</b>";
        else if (Types == 3)
            s1 = "<b>梅花同花</b>";
        else if (Types == 4)
            s1 = "<b style=\"color:red\">方块同花</b>";
        else if (Types == 5)
            s1 = "<b style=\"color:#8A7B66\">同花全包</b>";
        else if (Types == 6)
            s1 = "<b>A23</b>";
        else if (Types == 7)
            s1 = "<b>234</b>";
        else if (Types == 8)
            s1 = "<b>345</b>";
        else if (Types == 9)
            s1 = "<b>456</b>";
        else if (Types == 10)
            s1 = "<b>567</b>";
        else if (Types == 11)
            s1 = "<b>678</b>";
        else if (Types == 12)
            s1 = "<b>789</b>";
        else if (Types == 13)
            s1 = "<b>8910</b>";
        else if (Types == 14)
            s1 = "<b>910J</b>";
        else if (Types == 15)
            s1 = "<b>10JQ</b>";
        else if (Types == 16)
            s1 = "<b>JQK</b>";
        else if (Types == 17)
            s1 = "<b>QKA</b>";
        else if (Types == 18)
            s1 = "<b style=\"color:#8A7B66\">顺子全包</b>";
        else if (Types == 19)
            s1 = "<b>黑桃同花顺</b>";
        else if (Types == 20)
            s1 = "<b style=\"color:red\">红桃同花顺</b>";
        else if (Types == 21)
            s1 = "<b>梅花同花顺</b>";
        else if (Types == 22)
            s1 = "<b style=\"color:red\">方块同花顺</b>";
        else if (Types == 23)
            s1 = " <b style=\"color:#8A7B66\">同花顺全包</b>";
        else if (Types == 24)
            s1 = "<b>AAA</b>";
        else if (Types == 25)
            s1 = "<b>222</b>";
        else if (Types == 26)
            s1 = "<b>333</b>";
        else if (Types == 27)
            s1 = "<b>444</b>";
        else if (Types == 28)
            s1 = "<b>555</b>";
        else if (Types == 29)
            s1 = "<b>666</b>";
        else if (Types == 30)
            s1 = "<b>777</b>";
        else if (Types == 31)
            s1 = "<b>888</b>";
        else if (Types == 32)
            s1 = "<b>999</b>";
        else if (Types == 33)
            s1 = "<b>101010</b>";
        else if (Types == 34)
            s1 = "<b>JJJ</b>";
        else if (Types == 35)
            s1 = "<b>QQQ</b>";
        else if (Types == 36)
            s1 = "<b>KKK</b>";
        else if (Types == 37)
            s1 = "<b style=\"color:#8A7B66\">豹子全包</b>";
        else if (Types == 38)
            s1 = "<b>AA</b>";
        else if (Types == 39)
            s1 = "<b>22</b>";
        else if (Types == 40)
            s1 = "<b>33</b>";
        else if (Types == 41)
            s1 = "<b>44</b>";
        else if (Types == 42)
            s1 = "<b>55</b>";
        else if (Types == 43)
            s1 = "<b>66</b>";
        else if (Types == 44)
            s1 = "<b>77</b>";
        else if (Types == 45)
            s1 = "<b>88</b>";
        else if (Types == 46)
            s1 = "<b>99</b>";
        else if (Types == 47)
            s1 = "<b>1010</b>";
        else if (Types == 48)
            s1 = "<b>JJ</b>";
        else if (Types == 49)
            s1 = "<b>QQ</b>";
        else if (Types == 50)
            s1 = "<b>KK</b>";
        else if (Types == 51)
            s1 = "<b style=\"color:#8A7B66\">对子全包</b>";
        return s1;
    }

}
