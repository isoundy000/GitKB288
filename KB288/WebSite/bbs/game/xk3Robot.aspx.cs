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
using BCW.Common;
using System.Text.RegularExpressions;

/// <summary>
/// 邵广林 20160506 改为机器人购买最新的那一期，而不是下下期
/// 邵广林 20160506 开奖数据不为空时，机器人购买
/// </summary>

public partial class bbs_game_xinkuai3_Robot : System.Web.UI.Page
{
    protected string xmlPath = "/Controls/xinkuai3.xml";
    protected string GameName = ub.GetSub("XinKuai3Name", "/Controls/xinkuai3.xml");//游戏名字
    protected float Da = float.Parse(ub.GetSub("Xda1", "/Controls/xinkuai3.xml"));//大
    protected float Xiao = float.Parse(ub.GetSub("Xxiao1", "/Controls/xinkuai3.xml"));//小
    protected float Dan = float.Parse(ub.GetSub("Xdan1", "/Controls/xinkuai3.xml"));//单
    protected float Shuang = float.Parse(ub.GetSub("Xshuang1", "/Controls/xinkuai3.xml"));//双

    protected void Page_Load(object sender, EventArgs e)
    {
        //防止缓存
        Response.Buffer = true;
        Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
        Response.Expires = 0;
        Response.CacheControl = "no-cache";

        if (ub.GetSub("XIsBot", xmlPath) != "1")
        {
            //Response.Write("<b>" + GameName + "_机器人处于关闭状态</b>");
            Response.Write("close1");
        }
        else
        {
            ChangePalyXK3_Robot();//打开机器人
        }
    }

    //新快3自动游戏程序
    private void ChangePalyXK3_Robot()
    {
        //进行自动下注
        int hour = DateTime.Now.Hour;
        if (hour > 23 || hour < 9)
        {
            //Response.Write("" + GameName + "_机器人已休息!");
            Response.Write("close1");
        }
        else
        {
            string where1 = string.Empty;
            where1 = "where Lottery_num!='' ORDER BY ID DESC";
            BCW.XinKuai3.Model.XK3_Internet_Data model = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3listLast(where1);//最后一期
            if (model.Lottery_num != "")
            {
                PlayXK3_Robot();
                //Response.Write("" + GameName + "_机器人正在工作中!");
                Response.Write("ok1");
            }
            else
            {
                //Response.Write("" + GameName + "_第《" + model.Lottery_issue + "》期未开奖，机器人购买失败!");
                Response.Write("error1");
            }
        }
    }

    //随机得到下注的类型
    private int GetPtype()
    {
        Random rac = new Random();
        //int ptype = rac.Next(1, 11);//邵广林 20160928 删除机器人购买豹子3
        string[] sNum = { "1", "2", "4", "5", "6", "7", "8", "9", "10" };
        int ptype = int.Parse((sNum[rac.Next(sNum.Length)]));
        System.Threading.Thread.Sleep(50);//延时
        return ptype;
    }

    //得到出动的ID
    private int GetUsID()
    {
        int UsID = 0;
        string PlayUsID = ub.GetSub("XK3ROBOTID", "/Controls/xinkuai3.xml");
        string[] sNum = Regex.Split(PlayUsID, "#");
        Random rd = new Random();
        try
        {
            UsID = Convert.ToInt32(sNum[rd.Next(sNum.Length)]);
        }
        catch
        {
            UsID = Convert.ToInt32(sNum[rd.Next(sNum.Length)]);
        }
        return UsID;
    }

    //随机得到下注的币数(取整十整百整千整万等)
    private long GetPayCent()
    {
        Random rac = new Random();
        long price = 100;
        string aa = ub.GetSub("XK3ROBOTCOST", xmlPath);
        if (aa != "")
        {
            string[] sNum = Regex.Split(aa, "#");
            int bb = sNum.Length;
            int cc = rac.Next(1, bb + 1);
            try
            {
                price = Convert.ToInt64(sNum[(cc - 1)]);
            }
            catch
            {
                price = Convert.ToInt64(sNum[(cc - 1)]);
            }
        }
        else
        {
            price = 100;
        }
        return price;
    }


    //显示标题的投注方式
    private string OutType(int Types)
    {
        string pText = string.Empty;
        if (Types == 1)
            pText = "和值投注";
        else if (Types == 2)
            pText = "三同号通选投注";
        else if (Types == 3)
            pText = "三同号单选投注";
        else if (Types == 4)
            pText = "三不同号投注";
        else if (Types == 5)
            pText = "三连号通选投注";
        else if (Types == 6)
            pText = "二同号复选投注";
        else if (Types == 7)
            pText = "二同号单选投注";
        else if (Types == 8)
            pText = "二不同号投注";
        else if (Types == 9)
            pText = "大小投注";
        else if (Types == 10)
            pText = "单双投注";
        return pText;
    }

    //机器人购买
    private void PlayXK3_Robot()
    {
        //得到随机的UsID
        int meid = GetUsID();
        if (meid == 0)
        {
            Response.Write("随机机器人ID出错.error1<br/>");
            Response.End();
        }
        if (!new BCW.BLL.User().ExistsID(meid))
        {
            Response.Write("随机机器人ID不存在.error1<br/>");
            Response.End();
        }
        //得到随机的类型
        int num1 = GetPtype();
        //得到随机投注的酷币
        long Price = GetPayCent();
        //xml限定每个机器人购买次数
        int buycou = Convert.ToInt32(ub.GetSub("XK3ROBOTBUY", xmlPath));

        int dnu = 0;
        string dsb = DateTime.Now.ToString("yyMMdd");
        dnu = int.Parse(dsb + "001");
        string where1 = string.Empty;
        where1 = "ORDER BY Lottery_issue DESC";

        BCW.XinKuai3.Model.XK3_Internet_Data model = new BCW.XinKuai3.BLL.XK3_Internet_Data().Getxk3listLast(where1);//最后一期

        //string issue2 = (Int64.Parse(model.Lottery_issue) + 1).ToString();//下一个开奖期号
        string issue3 = Utils.Right(model.Lottery_issue.ToString(), 3);//本期开奖期号的后3位
        string issue33 = model.Lottery_issue;
        BCW.XinKuai3.Model.XK3_Bet modelBuy = new BCW.XinKuai3.Model.XK3_Bet();
        if (IsOpen() == true)
        {
            modelBuy.Lottery_issue = issue33;
        }
        else
        {
            if (issue3 == "078")
            {
                modelBuy.Lottery_issue = dnu.ToString();
            }
            else
            {
                modelBuy.Lottery_issue = (issue33);//投注期号
            }
        }

        //计数出该机器人投注的次数是否大于xml限定次数
        int count = new BCW.XinKuai3.BLL.XK3_Bet().GetXK3_Bet_GetRecordCount(" UsID=" + meid + " and Lottery_issue='" + modelBuy.Lottery_issue + "'");
        if ((count < buycou) || (buycou == 0))
        {
            if (num1 == 9)//1大2小
            {
                int a11 = int.Parse(Get_DXSD());
                if (a11 == 1)
                {
                    modelBuy.DaXiao = "1";
                    modelBuy.Odds = (decimal)Da;
                }
                else
                {
                    modelBuy.DaXiao = "2";
                    modelBuy.Odds = (decimal)Xiao;
                }

                modelBuy.Play_Way = 9;
                modelBuy.Sum = "";
                modelBuy.Three_Same_All = "";
                modelBuy.Three_Same_Single = "";
                modelBuy.Three_Same_Not = "";
                modelBuy.Three_Continue_All = "";
                modelBuy.Two_Same_All = "";
                modelBuy.Two_Same_Single = "";
                modelBuy.Two_dissame = "";
                modelBuy.DanShuang = "";
            }
            else if (num1 == 10)//1单2双
            {
                int a22 = int.Parse(Get_DXSD());
                if (a22 == 1)
                {
                    modelBuy.DanShuang = "2";
                    modelBuy.Odds = (decimal)Shuang;
                }
                else
                {
                    modelBuy.DanShuang = "1";
                    modelBuy.Odds = (decimal)Dan;
                }
                modelBuy.Play_Way = 10;
                modelBuy.Sum = "";
                modelBuy.Three_Same_All = "";
                modelBuy.Three_Same_Single = "";
                modelBuy.Three_Same_Not = "";
                modelBuy.Three_Continue_All = "";
                modelBuy.Two_Same_All = "";
                modelBuy.Two_Same_Single = "";
                modelBuy.Two_dissame = "";
                modelBuy.DaXiao = "";
            }
            //和值4-17
            else if (num1 == 1)
            {
                Random a3 = new Random();
                int a33 = a3.Next(1, 15);

                switch (a33)
                {
                    case 1:
                        modelBuy.Sum = "4";
                        break;
                    case 2:
                        modelBuy.Sum = "5";
                        break;
                    case 3:
                        modelBuy.Sum = "6";
                        break;
                    case 4:
                        modelBuy.Sum = "7";
                        break;
                    case 5:
                        modelBuy.Sum = "8";
                        break;
                    case 6:
                        modelBuy.Sum = "9";
                        break;
                    case 7:
                        modelBuy.Sum = "10";
                        break;
                    case 8:
                        modelBuy.Sum = "11";
                        break;
                    case 9:
                        modelBuy.Sum = "12";
                        break;
                    case 10:
                        modelBuy.Sum = "13";
                        break;
                    case 11:
                        modelBuy.Sum = "14";
                        break;
                    case 12:
                        modelBuy.Sum = "15";
                        break;
                    case 13:
                        modelBuy.Sum = "16";
                        break;
                    case 14:
                        modelBuy.Sum = "17";
                        break;
                }
                modelBuy.Play_Way = 1;
                modelBuy.Three_Same_All = "";
                modelBuy.Three_Same_Single = "";
                modelBuy.Three_Same_Not = "";
                modelBuy.Three_Continue_All = "";
                modelBuy.Two_Same_All = "";
                modelBuy.Two_Same_Single = "";
                modelBuy.Two_dissame = "";
                modelBuy.DaXiao = "";
                modelBuy.DanShuang = "";
                modelBuy.Odds = 1;
            }
            //三同号通选
            else if (num1 == 2)
            {
                modelBuy.Play_Way = 2;
                modelBuy.Sum = "";
                modelBuy.Three_Same_All = "1";
                modelBuy.Three_Same_Single = "";
                modelBuy.Three_Same_Not = "";
                modelBuy.Three_Continue_All = "";
                modelBuy.Two_Same_All = "";
                modelBuy.Two_Same_Single = "";
                modelBuy.Two_dissame = "";
                modelBuy.DaXiao = "";
                modelBuy.DanShuang = "";
                modelBuy.Odds = 1;
            }
            //三同号单选
            else if (num1 == 3)
            {
                int a55 = int.Parse(Get_danxuan());
                switch (a55)
                {
                    case 1:
                        modelBuy.Three_Same_Single = "111";
                        break;
                    case 2:
                        modelBuy.Three_Same_Single = "222";
                        break;
                    case 3:
                        modelBuy.Three_Same_Single = "333";
                        break;
                    case 4:
                        modelBuy.Three_Same_Single = "444";
                        break;
                    case 5:
                        modelBuy.Three_Same_Single = "555";
                        break;
                    case 6:
                        modelBuy.Three_Same_Single = "666";
                        break;
                }
                modelBuy.Play_Way = 3;
                modelBuy.Sum = "";
                modelBuy.Three_Same_All = "";
                modelBuy.Three_Same_Not = "";
                modelBuy.Three_Continue_All = "";
                modelBuy.Two_Same_All = "";
                modelBuy.Two_Same_Single = "";
                modelBuy.Two_dissame = "";
                modelBuy.DaXiao = "";
                modelBuy.DanShuang = "";
                modelBuy.Odds = 1;
            }
            else if (num1 == 4)//三不同号-------------------再随机3个不同的数字
            {
                //随机产生3条不重复的1-6的数
                int[] result = new int[6];
                for (int i = 0; i < 6; i++) result[i] = i + 1;
                for (int j = 5; j > 0; j--)
                {
                    Random r = new Random();
                    int index = r.Next(0, j);
                    int temp = result[index];
                    result[index] = result[j];
                    result[j] = temp;
                }

                //冒泡排序 从大到小
                for (int i = 0; i < 3; i++)
                {
                    for (int j = i + 1; j < 3; j++)
                    {
                        if (result[j] < result[i])
                        {
                            int temp = result[i];
                            result[i] = result[j];
                            result[j] = temp;
                        }
                    }
                }

                modelBuy.Three_Same_Not = (result[0]).ToString() + (result[1]).ToString() + (result[2]).ToString();
                modelBuy.Play_Way = 4;
                modelBuy.Sum = "";
                modelBuy.Three_Same_All = "";
                modelBuy.Three_Same_Single = "";

                modelBuy.Three_Continue_All = "";
                modelBuy.Two_Same_All = "";
                modelBuy.Two_Same_Single = "";
                modelBuy.Two_dissame = "";
                modelBuy.DaXiao = "";
                modelBuy.DanShuang = "";
                modelBuy.Odds = 1;
            }
            else if (num1 == 5)//三连号
            {
                modelBuy.Play_Way = 5;
                modelBuy.Sum = "";
                modelBuy.Three_Same_All = "";
                modelBuy.Three_Same_Single = "";
                modelBuy.Three_Same_Not = "";
                modelBuy.Three_Continue_All = "1";
                modelBuy.Two_Same_All = "";
                modelBuy.Two_Same_Single = "";
                modelBuy.Two_dissame = "";
                modelBuy.DaXiao = "";
                modelBuy.DanShuang = "";
                modelBuy.Odds = 1;
            }
            //二同号复选
            else if (num1 == 6)
            {
                int a66 = int.Parse(Get_danxuan());
                switch (a66)
                {
                    case 1:
                        modelBuy.Two_Same_All = "11";
                        break;
                    case 2:
                        modelBuy.Two_Same_All = "22";
                        break;
                    case 3:
                        modelBuy.Two_Same_All = "33";
                        break;
                    case 4:
                        modelBuy.Two_Same_All = "44";
                        break;
                    case 5:
                        modelBuy.Two_Same_All = "55";
                        break;
                    case 6:
                        modelBuy.Two_Same_All = "66";
                        break;
                }
                modelBuy.Play_Way = 6;
                modelBuy.Sum = "";
                modelBuy.Three_Same_All = "";
                modelBuy.Three_Same_Single = "";
                modelBuy.Three_Same_Not = "";
                modelBuy.Three_Continue_All = "";
                modelBuy.Two_Same_Single = "";
                modelBuy.Two_dissame = "";
                modelBuy.DaXiao = "";
                modelBuy.DanShuang = "";
                modelBuy.Odds = 1;
            }
            else if (num1 == 7)//二同号单选
            {
                modelBuy.Play_Way = 7;
                modelBuy.Sum = "";
                modelBuy.Three_Same_All = "";
                modelBuy.Three_Same_Single = "";
                modelBuy.Three_Same_Not = "";
                modelBuy.Three_Continue_All = "";
                modelBuy.Two_Same_All = "";
                modelBuy.Two_Same_Single = Get_Two_Same_Single();
                modelBuy.Two_dissame = "";
                modelBuy.DaXiao = "";
                modelBuy.DanShuang = "";
                modelBuy.Odds = 1;
            }
            else if (num1 == 8)//二不同号
            {
                modelBuy.Play_Way = 8;
                modelBuy.Sum = "";
                modelBuy.Three_Same_All = "";
                modelBuy.Three_Same_Single = "";
                modelBuy.Three_Same_Not = "";
                modelBuy.Three_Continue_All = "";
                modelBuy.Two_Same_All = "";
                modelBuy.Two_Same_Single = "";
                modelBuy.Two_dissame = Get_Two_dissame2();
                modelBuy.DaXiao = "";
                modelBuy.DanShuang = "";
                modelBuy.Odds = 1;
            }

            modelBuy.UsID = meid;//用户id
            modelBuy.Input_Time = DateTime.Now;//投注时间
            modelBuy.Zhu = 1;//注数----默认全部投一注。

            modelBuy.DanTuo = "0";//胆拖
            modelBuy.State = 0;//未开奖
            modelBuy.GetMoney = 0;//获得多少酷币
            modelBuy.isRobot = 1;


            modelBuy.Zhu_money = Price;//每注投多少钱
            modelBuy.PutGold = Price * modelBuy.Zhu;//总投了多少钱


            long gold = new BCW.BLL.User().GetGold(meid);
            long prices = Convert.ToInt64(Price * modelBuy.Zhu);
            string mename = new BCW.BLL.User().GetUsName(meid);

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

            }

            long xPrices = Utils.ParseInt64(ub.GetSub("XK3Price", xmlPath));
            if (xPrices > 0)
            {
                long oPrices = 0;
                DataSet ds;
                if (IsOpen() == true)
                {
                    ds = new BCW.XinKuai3.BLL.XK3_Bet().GetList("PutGold", "UsID=" + meid + " and Lottery_issue='" + issue33 + "'");
                }
                else
                {
                    if (issue3 == "078")
                    {
                        ds = new BCW.XinKuai3.BLL.XK3_Bet().GetList("PutGold", "UsID=" + meid + " and Lottery_issue='" + dnu + "'");
                    }
                    else
                    {
                        ds = new BCW.XinKuai3.BLL.XK3_Bet().GetList("PutGold", "UsID=" + meid + " and Lottery_issue='" + issue33 + "'");
                    }
                }

                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    int drs = int.Parse(dr[0].ToString());
                    oPrices = oPrices + drs;
                }

                if (oPrices + prices > xPrices)
                {
                    Response.End();
                }
            }

            //new BCW.BLL.User().UpdateiGold(meid, mename, -prices, 12);//新快3----更新排行榜与扣钱  
            if (IsOpen() == true)
            {
                new BCW.BLL.User().UpdateiGold(meid, mename, -prices, "" + GameName + "第" + issue33 + "期投注消费");
            }
            else
            {
                if (issue3 == "078")
                {
                    new BCW.BLL.User().UpdateiGold(meid, mename, -prices, "" + GameName + "第" + dnu.ToString() + "期投注消费");
                }
                else
                {
                    new BCW.BLL.User().UpdateiGold(meid, mename, -prices, "" + GameName + "第" + issue33 + "期投注消费");
                }
            }
            int id = new BCW.XinKuai3.BLL.XK3_Bet().Add_Robot(modelBuy);

            new BCW.BLL.User().UpdateTime(meid, 5);// 更新会员在线时长

            ////更新排行榜
            //if (!(new BCW.XinKuai3.BLL.XK3_Toplist().Exists_usid(meid)))
            //{
            //    BCW.XinKuai3.Model.XK3_Toplist model_2 = new BCW.XinKuai3.Model.XK3_Toplist();
            //    model_2.UsId = meid;
            //    model_2.UsName = mename;
            //    model_2.WinGold = 0;
            //    model_2.PutGold = prices;
            //    new BCW.XinKuai3.BLL.XK3_Toplist().Add(model_2);
            //}
            //else
            //{
            //    BCW.XinKuai3.Model.XK3_Toplist model_1 = new BCW.XinKuai3.BLL.XK3_Toplist().GetXK3_meid(meid);

            //    long all_prices = model_1.PutGold + prices;

            //    new BCW.XinKuai3.BLL.XK3_Toplist().Update_gold(meid, all_prices);
            //}

            //动态
            string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/xk3.aspx]" + GameName + "[/url]下注**" + ub.Get("SiteBz") + "";//" + modelBuy.Zhu * modelBuy.Zhu_money + "
            new BCW.BLL.Action().Add(1001, id, meid, "", wText);
        }

    }

    //二同号单选_自动获取
    private string Get_Two_Same_Single()
    {
        Random rac = new Random();
        string paycent = string.Empty;

        string[] sNum = { "112", "113", "114", "115", "116", "221", "223", "224", "225", "226", "331", "332", "334", "335", "336", "441", "442", "443", "445", "446", "551", "552", "553", "554", "556", "661", "662", "663", "664", "665" };
        paycent = (sNum[rac.Next(sNum.Length)]);

        return paycent;
    }

    //二不同号_自动获取
    private string Get_Two_dissame2()
    {
        Random rac = new Random();
        string paycent = string.Empty;
        string[] sNum = { "12", "13", "14", "15", "16", "23", "24", "25", "26", "34", "35", "36", "45", "46", "56" };
        paycent = (sNum[rac.Next(sNum.Length)]);
        return paycent;
    }

    //大小双单_自动获取
    private string Get_DXSD()
    {
        Random rac = new Random();
        string paycent = string.Empty;
        string[] sNum = { "1", "2" };
        paycent = (sNum[rac.Next(sNum.Length)]);
        return paycent;
    }

    //三同号单选、二同号单选
    private string Get_danxuan()
    {
        Random rac = new Random();
        string paycent = string.Empty;
        string[] sNum = { "1", "2", "3", "4", "5", "6" };
        paycent = (sNum[rac.Next(sNum.Length)]);
        return paycent;
    }

    private bool IsOpen()
    {
        bool IsOpen = true;
        string OnTime = ub.GetSub("xOnTime", xmlPath);//20160423
        //string OnTime = "09:26-22:26";
        if (OnTime != "")
        {
            if (Utils.IsRegex(OnTime, @"^[0-9]{2}:[0-9]{2}-[0-9]{2}:[0-9]{2}$"))
            {
                string[] temp = OnTime.Split("-".ToCharArray());
                DateTime dt1 = Convert.ToDateTime(temp[0]);
                DateTime dt2 = Convert.ToDateTime(temp[1]);
                if (DateTime.Now > dt1 && DateTime.Now < dt2)
                {
                    IsOpen = true;
                }
                else
                {
                    IsOpen = false;
                }
            }
            else
                IsOpen = false;
        }
        return IsOpen;
    }
}
