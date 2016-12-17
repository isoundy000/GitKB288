using System;
using System.Data;
using BCW.Common;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public partial class bbs_game_hc1rebot : System.Web.UI.Page
{
    protected string xmlPath = "/Controls/hc1.xml";
    protected string GameName = ub.GetSub("Hc1Name", "/Controls/hc1.xml");//游戏名字

    protected void Page_Load(object sender, EventArgs e)
    {
        //防止缓存
        Response.Buffer = true;
        Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
        Response.Expires = 0;
        Response.CacheControl = "no-cache";

        if (ub.GetSub("hc1IsBot", xmlPath) != "1")
        {
            Response.Write("<b>" + GameName + "_机器人处于关闭状态close1</b>");
        }
        else
        {
            ChangePalyHc1_Robot();//打开机器人
            //Response.Write("<br/>================="+a66);
        }
    }

    //自动游戏程序
    private void ChangePalyHc1_Robot()
    {
        //进行自动下注
        int hour = DateTime.Now.Hour;
        if (hour > 23 || hour < 9)
        {
            Response.Write("" + GameName + "_机器人已休息close1!");
        }
        else
        {
            try
            {
                BCW.Model.Game.HcList modelbuy1 = new BCW.BLL.Game.HcList().GetHcListNew(0);        //最后一期                                                                                                                                                                                 
                if (modelbuy1.EndTime > DateTime.Now)
                {
                    PlayHc1_Robot();
                    Response.Write("" + GameName + "_机器人正在工作中ok1!");
                }
                else
                {
                    Response.Write("" + GameName + "_机器人购买失败!投注时间截止close1");
                }
            }
            catch
            {
                Response.Write("" + GameName + "_机器人购买失败！系统未开期数error1");
            }
        }
    }
    //随机得到下注的类型
    private int GetPtype()
    {
        Random rac = new Random();
        int ptype = rac.Next(1, 10);//1选号玩法、2生肖玩法、3方位玩法、4四季玩法、5大小单双玩法、6六肖中奖玩法、7尾数大小玩法、8尾数单双玩法、9家禽野兽玩法
        System.Threading.Thread.Sleep(50);//延时
        return ptype;
    }

    //得到出动的ID
    private int GetUsID()
    {
        int UsID = 0;
        string PlayUsID = ub.GetSub("hc1ROBOTID", "/Controls/hc1.xml");
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

        int paycent = rac.Next(10, 99);
        long price = 0;
        try
        {
            price = Convert.ToInt64(paycent * Convert.ToInt32(ub.GetSub("hc1ROBOTCOST", xmlPath)));
        }
        catch
        {
            price = 0;
        }

        return price;
    }

    // 更新会员在线时长
    private void ChanageOnline(int UsID)
    {
        int OnTime = 5;
        new BCW.BLL.User().UpdateTime(UsID, OnTime);
    }

    //显示标题的投注方式
    private string OutType(int Types)
    {
        string TyName = string.Empty;
        if (Types == 1)
            TyName = "选号玩法";
        else if (Types == 2)
            TyName = "生肖玩法";
        else if (Types == 3)
            TyName = "方位玩法";
        else if (Types == 4)
            TyName = "四季玩法";
        else if (Types == 5)
            TyName = "大小单双";
        else if (Types == 6)
            TyName = "六肖中奖";
        else if (Types == 7)
            TyName = "尾数大小";
        else if (Types == 8)
            TyName = "尾数单双";
        else if (Types == 9)
            TyName = "家禽野兽";
        else if (Types == 10)
            TyName = "自选不中";
        return TyName;
    }

    //机器人购买
    private void PlayHc1_Robot()
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
        int buycou = 0;
        try
        {
            buycou = Convert.ToInt32(ub.GetSub("hc1ROBOTBUY", xmlPath));
        }
        catch
        {
            buycou = 0;
        }

        long Gold = new BCW.BLL.User().GetGold(meid);

        #region 判断机器人币够不够投注

        if (Gold < Price || Gold < 0)
        {
            Response.Write("<b>机器人" + meid + "币不够！请换一个机器人或者给该机器人充值 </b><br />");
        }
        else
        {
            #region 机器人投注

            int dnu = 0;
            string dsb = DateTime.Now.ToString("yyMMdd");
            dnu = int.Parse(dsb + "001");
            string where1 = string.Empty;
            where1 = "ORDER BY ID DESC";

            //最后一期
            DataSet ds = new BCW.BLL.Game.HcList().GetList("top 1 id", "State=0 order by EndTime desc");
            int maxid = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
            BCW.Model.Game.HcList modelbuy1 = new BCW.BLL.Game.HcList().GetHcList(maxid);
            BCW.Model.Game.HcPay modelbuy = new BCW.Model.Game.HcPay();
            int count = new BCW.BLL.Game.HcList().GetcountRebot(meid);
            if ((count < buycou) || (buycou == 0))
            {
                #region 大小单双投注

                if (num1 == 5)//1大2小/3单/4双
                {
                    int a11 = int.Parse(Get_DXSD());
                    modelbuy.Types = 5;
                    if (a11 == 1)
                    {
                        modelbuy.Vote = "大";
                    }
                    else if (a11 == 2)
                    {
                        modelbuy.Vote = "小";
                    }
                    else if (a11 == 3)
                    {
                        modelbuy.Vote = "单";
                    }
                    else
                    {
                        modelbuy.Vote = "双";
                    }
                }
                #endregion
                //选号玩法
                else if (num1 == 1)
                {
                    modelbuy.Types = 1;
                    Random a3 = new Random();
                    int a33 = a3.Next(1, 37);
                    modelbuy.Vote = Convert.ToString(a33);

                }
                //生肖玩法
                else if (num1 == 2)
                {
                    modelbuy.Types = 2;
                    Random a4 = new Random();
                    int a44 = a4.Next(1, 13);
                    switch (a44)
                    {
                        case 1:
                            modelbuy.Vote = "鼠";
                            break;
                        case 2:
                            modelbuy.Vote = "牛";
                            break;
                        case 3:
                            modelbuy.Vote = "虎";
                            break;
                        case 4:
                            modelbuy.Vote = "兔";
                            break;
                        case 5:
                            modelbuy.Vote = "龙";
                            break;
                        case 6:
                            modelbuy.Vote = "蛇";
                            break;
                        case 7:
                            modelbuy.Vote = "马";
                            break;
                        case 8:
                            modelbuy.Vote = "羊";
                            break;
                        case 9:
                            modelbuy.Vote = "猴";
                            break;
                        case 10:
                            modelbuy.Vote = "鸡";
                            break;
                        case 11:
                            modelbuy.Vote = "狗";
                            break;
                        case 12:
                            modelbuy.Vote = "猪";
                            break;
                    }
                }
                //方位玩法
                else if (num1 == 3)
                {
                    modelbuy.Types = 3;
                    Random a5 = new Random();
                    int a55 = a5.Next(1, 5);
                    switch (a55)
                    {
                        case 1:
                            modelbuy.Vote = "东";
                            break;
                        case 2:
                            modelbuy.Vote = "南";
                            break;
                        case 3:
                            modelbuy.Vote = "西";
                            break;
                        case 4:
                            modelbuy.Vote = "北";
                            break;
                    }

                }
                else if (num1 == 4)//四季玩法
                {
                    modelbuy.Types = 4;
                    Random a6 = new Random();
                    int a66 = a6.Next(1, 5);
                    switch (a66)
                    {
                        case 1:
                            modelbuy.Vote = "春";
                            break;
                        case 2:
                            modelbuy.Vote = "夏";
                            break;
                        case 3:
                            modelbuy.Vote = "秋";
                            break;
                        case 4:
                            modelbuy.Vote = "东";
                            break;
                    }
                }
                //六肖中奖
                else if (num1 == 6)
                {
                    modelbuy.Types = 6;
                    int a66 = 0;
                    //随机生成6个生肖
                    string rand = "";
                    while (a66 < 6)
                    {
                        rand += GetRandomNum1()[a66];
                        rand += ",";
                        a66++;
                    }
                    modelbuy.Vote = rand;
                }
                //尾数大小
                else if (num1 == 7)
                {
                    modelbuy.Types = 7;
                    Random a7 = new Random();
                    int a77 = a7.Next(0, 2);
                    switch (a77)
                    {
                        case 0:
                            modelbuy.Vote = "大";
                            break;
                        case 1:
                            modelbuy.Vote = "小";
                            break;
                    }
                }
                //尾数单双
                else if (num1 == 8)
                {
                    modelbuy.Types = 8;
                    Random a8 = new Random();
                    int a88 = a8.Next(0, 2);
                    switch (a88)
                    {
                        case 0:
                            modelbuy.Vote = "单";
                            break;
                        case 1:
                            modelbuy.Vote = "双";
                            break;
                    }
                }
                //家禽0野兽1
                else if (num1 == 9)
                {
                    modelbuy.Types = 9;
                    Random a9 = new Random();
                    int a99 = a9.Next(0, 2);
                    switch (a99)
                    {
                        case 0:
                            modelbuy.Vote = "家禽";
                            break;
                        case 1:
                            modelbuy.Vote = "野兽";
                            break;
                    }
                }

                modelbuy.UsID = meid;//用户id
                modelbuy.AddTime = DateTime.Now;//投注时间      
                modelbuy.State = 0;//未开奖
                modelbuy.WinCent = 0;//获得多少酷币
                modelbuy.IsSpier = 1;
                modelbuy.BzType = 0;
                modelbuy.Result = "";//开奖结果
                modelbuy.PayCent = Price;//每注投多少钱
                modelbuy.PayCents = Price;//总投了多少钱
                modelbuy.CID = modelbuy1.CID;//最新投注期数

                long gold = new BCW.BLL.User().GetGold(meid);
                long prices = Convert.ToInt64(Price);
                string mename = new BCW.BLL.User().GetUsName(meid);
                modelbuy.UsName = mename;//用户名字
                if (gold < prices)
                {
                    //更新消费记录
                    BCW.Model.Goldlog modelx = new BCW.Model.Goldlog();
                    modelx.BbTag = 3;
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
                new BCW.BLL.User().UpdateiGold(meid, mename, -prices, 1001);//新快3----更新排行榜与扣钱  
                new BCW.BLL.Game.HcPay().Add(modelbuy);
                new BCW.BLL.User().UpdateiGold(meid, mename, -prices, "好彩1" + modelbuy1.CID + "期投注消费");
                new BCW.BLL.Game.HcList().Update1(modelbuy1.CID, prices, 1);   //更新下注总额和下注额   
                ChanageOnline(meid);//邵广林 增加机器人在线时长
                //动态
                string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/hc1.aspx]" + GameName + "[/url]下注**" + ub.Get("SiteBz") + "";//" + prices + "
                new BCW.BLL.Action().Add(1001, 1, meid, "", wText);
            }
            #endregion
        }
        #endregion
    }
    //大小双单_自动获取
    private string Get_DXSD()
    {
        Random rac = new Random();
        string paycent = string.Empty;
        string[] sNum = { "1", "2", "3", "4" };
        paycent = (sNum[rac.Next(sNum.Length)]);
        return paycent;
    }
    //生成随机不重复的数
    private string[] GetRandomNum1()
    {
        Dictionary<string, int> dict = new Dictionary<string, int>();
        Random r = new Random();
        string[] result1 = new string[12] { "鼠", "牛", "虎", "兔", "龙", "蛇", "马", "羊", "猴", "鸡", "猪", "狗" };
        string[] result = new string[6];
        int num = 0;
        string num1 = "";
        for (int i = 0; i < 6; i++)
        {
            num = r.Next(0, 12);
            num1 = Convert.ToString(result1[num]);
            while (dict.ContainsKey(num1))
            {
                num = r.Next(0, 12);
                num1 = Convert.ToString(result1[num]);
            }
            dict.Add(num1, 1);
        }
        dict.Keys.CopyTo(result, 0);
        return result;
    }
    //机器人兑奖
    public void RobotCase()
    {
        string RoBot = ub.GetSub("hc1ROBOTID", xmlPath);
        string RoBotCost = ub.GetSub("hc1ROBOTCOST", xmlPath);
        string[] RoBotID = RoBot.Split('#');
        MatchCollection leyr = Regex.Matches(RoBot, "#");
        int ele = leyr.Count + 1;
        for (int i = 0; i < ele; i++)
        {
            int meid = Convert.ToInt32(RoBotID[i]);
            DataSet model1 = new BCW.BLL.Game.HcPay().GetList("ID", "UsID=" + meid + "and IsSpier=1");
            for (int i2 = 0; i2 < model1.Tables[0].Rows.Count; i2++)
            {
                try
                {
                    int pid = Convert.ToInt32(model1.Tables[0].Rows[0][0]);
                    if (new BCW.BLL.Game.HcPay().ExistsState(pid, meid))
                    {
                        new BCW.BLL.Game.HcPay().UpdateState(pid);
                        //操作币
                        long winMoney = Convert.ToInt64(new BCW.BLL.Game.HcPay().GetWinCent(pid));
                        //税率
                        long SysTax = 0;
                        winMoney = winMoney - SysTax;
                        new BCW.BLL.User().UpdateiGold(meid, winMoney, "好彩一机器人兑奖-标识ID" + pid + "");
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
