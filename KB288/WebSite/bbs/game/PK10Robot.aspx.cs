using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BCW.Common;
using BCW.PK10;
using BCW.PK10.Model;
using System.Collections;
public partial class bbs_game_PK10Robot : System.Web.UI.Page
{
    protected string xmlPath = "/Controls/PK10.xml";
    protected bool myIsRobot = false;
    protected string myRobots = "";
    protected int myIsTest = 0;
    protected int myMinPayPrice = 100;
    protected int myMaxPayPrice = 1000000;
    protected int defaultTestMoney = 100000; //默认测试用的PK币
    protected PK10 _logic;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            _logic = new PK10();
            string GameName = ub.GetSub("GameName", xmlPath);
            DateTime GameBeginTime, GameEndTime, CurrentTime;
            int GameOpenTimes, SaleTimes, OpenTimes, isRobot;
            string Robots;
            #region 读取配置项，并初始化变量
            try
            {
                #region 游戏状态判断
                string GameStatus = ub.GetSub("GameStatus", xmlPath);
                switch (GameStatus)
                {
                    case "1": //维护
                        Utils.Safe(GameName);
                        break;
                    case "2": //内测(PK币)
                    case "3"://公测(PK币)
                        myIsTest = 1;
                        break;
                    case "4"://内测(酷币)
                        myIsTest = 0;
                        break;
                    default:  //正常 
                        myIsTest = 0;
                        break;
                }
                #endregion
                string ct1 = ub.GetSub("GameBeginTime", xmlPath);
                string ct2 = ub.GetSub("GameEndTime", xmlPath);
                GameBeginTime = Convert.ToDateTime(ct1);
                GameEndTime = Convert.ToDateTime(ct2);
                CurrentTime = DateTime.Now;
                GameOpenTimes = int.Parse(ub.GetSub("GameOpenTimes", xmlPath)); //游戏每天开奖期数
                SaleTimes = int.Parse(ub.GetSub("SaleTimes", xmlPath));//每期游戏开售分钟数
                OpenTimes = int.Parse(ub.GetSub("OpenTimes", xmlPath));//每期游戏开奖分钟数（在上期停售后N分钟后开奖）
                isRobot = int.Parse(ub.GetSub("isRobot", xmlPath)); //是否启用机器人的标志
                Robots = ub.GetSub("Robots", xmlPath); //机器人列表
                myIsRobot = (isRobot == 1) ? true : false;
                myRobots = Robots;
                myMinPayPrice = int.Parse(ub.GetSub("MinPayPrice", xmlPath)); //游戏最小下注单价
                myMaxPayPrice = int.Parse(ub.GetSub("MaxPayPrice", xmlPath)); //游戏最大下注单价
            }
            catch (Exception ex)
            {
                Response.Write("eror1:“读取配置项并初始化变量”出错！！！" + "</br>");
                Response.Write(ex.Message.Replace("\n", "</br>"));
                return;
            }
            #endregion
            Response.Write("PK10每天的销售时间为" + GameBeginTime.ToShortTimeString() + "至" + GameEndTime.ToShortTimeString() + "，每" + SaleTimes.ToString() + "分钟一期，每天" + GameOpenTimes.ToString().Trim() + "期" + "<br/>");
            if (!myIsRobot)
            {
                Response.Write("close1:" + "没设置开启机器人！");
                return;
            }
            #region 排除非销售时间
            //if (CurrentTime < GameBeginTime || CurrentTime > GameEndTime.AddMinutes(SaleTimes))
            //{
            //    Response.Write("ok1:" + "非销售时间！");
            //    return;
            //}
            #endregion
            #region 机器人下注、兑奖
            try
            {
                Response.Write("机器人...下注！" + "</br>");
                OpenRobot();
                Response.Write("ok1");
            }
            catch (Exception ex)
            {
                Response.Write("error1:“机器人下注、兑奖”出错！！！" + "</br>");
                Response.Write(ex.Message.Replace("\n", "</br>"));
                return;
            }
            #endregion
        }
        catch
        {
            Response.Write("error1");
        }
    }
    private void OpenRobot()
    {
        #region 取到机器人
        int RobotID = GetRobot();
        if (RobotID == 0)
        {
            Response.Write("没有可用的机器人！！！" + "</br>");
            return;
        }
        string RobotName = new BCW.BLL.User().GetUsName(RobotID);
        #endregion

        #region 更新机器人在线时间
        try
        {
            new BCW.BLL.User().UpdateTime(RobotID, 5);
        }
        catch { }
        #endregion
        //
        #region 下注
        PK10_List list = new PK10().GetCurrentSaleData();//取到可下注的期号记录
        if (list == null)
        {
            Response.Write("没有开售记录！" + "</br>");
        }
        else
        {
            PK10_Stutas status = new PK10().GetListStatus(list);
            if (status != PK10_Stutas.在售)
            {
                Response.Write("没有开售记录！" + "</br>");
            }
            else
            {
                Response.Write("第" + list.No.Trim() + "期" + "</br>");
                #region 下注
                PK10_Buy buy = CreateBuy(RobotID, RobotName, list);//生成购买记录
                if (buy == null)
                {
                    Response.Write("不能生成购买记录！" + "</br>");
                }
                else
                {
                    Response.Write(buy.BuyDescript + "，每注下：" + buy.BuyPrice.ToString().Trim() + "</br>");
                    #region 付款
                    string cPay = new PK10().Pay(buy, Utils.getPageUrl());
                    if (string.IsNullOrEmpty(cPay))
                        Response.Write("成功付款！" + "</br>");
                    else
                    {
                        Response.Write("付款失败...." + "</br>");
                        Response.Write(cPay.Replace("\n", "</br>") + "</br>");
                    }
                    #endregion
                }
                #endregion
            }
        }
        #endregion
        //
        #region 兑奖
        string caseFlag = _logic.CaseRobot(Utils.getPageUrl());
        if (caseFlag == "")
        {
            Response.Write("成功兑奖！" + "</br>");
        }
        else
        {
            Response.Write("兑奖失败...." + "</br>");
            Response.Write(caseFlag.Replace("\n", "</br>") + "</br>");
        }
        #endregion
    }
 
    private int GetRobot() //随机取得机器人
    {
        int id = 0;
        try
        {
            string[] nums = myRobots.Split('#');
            if (nums.Length > 0)
            {
                Random rd = new Random();
                id = Convert.ToInt32(nums[rd.Next(nums.Length)]);
            }
        }
        catch { };
        if(id!=0 && myIsTest==1) //如果是测试，则生成/判断是否在测试用户列表中已经有此用户
        {
            if (!_logic.CreateTestUser(id, GetDefaultTestMoney()))
                id = 0;
        }
        return id;
    }
    private int GetDefaultTestMoney()
    {
        int money = 0;
        int.TryParse(ub.GetSub("TestUserDefaultMoney", xmlPath), out money);
        if (money <= 0)
            money = defaultTestMoney;
        return money;
    }
    private PK10_BuyType GetBuyType() //随机取到购买的类型
    {
        PK10_BuyType buytype = new PK10_BuyType();
        try
        {
            List<PK10_BuyType> lists = new PK10().GetBuyTypes3();
            if (lists != null)
            {
                Random rd = new Random();
                buytype = lists[rd.Next(lists.Count)];
            }
        }
        catch
        {
            buytype = null;
        }
        return buytype;
    }
    private int GetPrice()//随机下注单价
    {
        int price = myMinPayPrice;
        string cprics= ub.GetSub("RobotPrice", xmlPath);
        if(string.IsNullOrEmpty(cprics))
            price = new Random().Next(1, 5) * price;
        else
        {
            string[] aprices = cprics.Split('#');
            int i = new Random().Next(aprices.Length - 1);
            price = Convert.ToInt32(aprices[i]);
            if (price < myMinPayPrice)
                price = myMinPayPrice;
        }
        return price;
    }
    private PK10_Buy CreateBuy(int uid,string uname, PK10_List list) //随机生成购买记录
    {
        PK10_Buy buy = new PK10_Buy();
        try
        {
            #region 取到购买类型
            PK10_BuyType buytype = GetBuyType();
            if (buytype == null)
            {
                Response.Write("不能取到购买类型！" + "</br>");
                return null;
            }
            #endregion
            #region 生成购买记录
            buy.ListID = list.ID;
            buy.ListNo = list.No;
            buy.uID = uid;
            buy.uName = uname;
            buy.isRobot = 1;
            buy.isTest = myIsTest;
            buy.BuyType = buytype.ID;
            buy.BuyPrice = GetPrice();
            buy.BuyTime = DateTime.Now;
            buy.BuyMulti = 1;// new Random().Next(1, 5);//下注陪数(该游戏用直接填写下注单价的形式，不需要倍数）;
            int ntype = new Random().Next(1, 10); //用Random().Next(0，1)总返回0；
            buy.NumType = (ntype > 5) ? 1 : 0;//号码组合类型（随机0单式1复式）   
            //
            if (buytype.MultiSelect == 0 && buy.NumType == 1) //如果购买类型不允许复式的，重置当前购买组合类型为单式
                buy.NumType = 0;
            //        
            CreateBuyNums(buy,buytype);//随机生成号码组号，并计算注数                     
            string numtype = (buy.NumType == 0) ? "单式" : "复式";
            string descript = "[" + buytype.Name.Trim() + "-" + numtype + "][" + buy.BuyCount.ToString().Trim() + "注]#" + buy.Nums.Trim() + "#[" + buy.BuyMulti + "倍]"; //例：[猜冠军-复式][4注]#03,04|07,09#[4倍]
            buy.BuyDescript = descript;
            buy.PayMoney = buy.BuyPrice * buy.BuyCount * buy.BuyMulti;
            #endregion
            if (buy.BuyCount == 0)
                buy = null;
        }
        catch
        {
            buy = null;
        }
        return buy;
    }
    private void CreateBuyNums(PK10_Buy buy, PK10_BuyType buytype)//生成号码组号
    {
        string strAll = "01,02,03,04,05,06,07,08,09,10";
        string newNums = "";
        int nums = buytype.NumsCount;
        int parentid = buytype.ParentID;
        switch (parentid)
        {
            case 1:
                #region 号码买法
                if (buy.NumType == 0)//单式，例子（直选前三)：02|04|03
                {
                    #region 直接生成号码组号
                    newNums = CreateRandomString(null, strAll, nums, "");
                    newNums = newNums.Replace(',', '|');
                    buy.Nums = newNums;
                    buy.NumsDetail = newNums;
                    buy.BuyCount = 1;
                    #endregion
                }
                else //复式，例子(前三复式)：02,03|03,05,06|09
                {
                    #region 生成号码组号
                    Random rd0 = new Random();
                    Random rd1 = new Random();
                    for (int i = 0; i < nums; i++)
                    {
                        int randNums = rd0.Next(1, 3);
                        string result = CreateRandomString(rd1, strAll, randNums, ""); //随机选择几个数字（机器人复式的每一位最多为3个号码）
                        if (i == 0)
                            newNums = result;
                        else
                            newNums += "|" + result;
                    }
                    buy.Nums = newNums;
                    #endregion
                    #region 拆分复式成每一注有效的单式号码，并变换成字符串格式，例猜前二位：01|02#01|03#01#04
                    List<string> list = new PK10().GenBuyNums(newNums, buytype.NumsCount);
                    buy.BuyCount = list.Count;
                    if (list.Count > 0)
                    {
                        string details = "";
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (i == 0)
                                details = list[i];
                            else
                                details += "#" + list[i];
                            Response.Write(list[i] + " </br>");
                        }
                        buy.NumsDetail = details;
                    }
                    #endregion
                }
                #endregion
                break;
            case 2://大小玩法
            case 3://单双玩法
            case 4://龙虎玩法
                #region 大小/单双/龙虎玩法
                int ntype = new Random().Next(1, 10); //用Random().Next(0，1)总返回0；
                newNums = (ntype > 5) ? "1" : "0";
                buy.Nums = newNums;
                buy.NumsDetail = newNums;
                buy.BuyCount = 1;
                #endregion
                break;
            case 5://任选
                #region 直接生成号码组号
                newNums = CreateRandomString(null, strAll, nums, "");
                newNums = newNums.Replace(',', '|');
                buy.Nums = newNums;
                buy.NumsDetail = newNums;
                buy.BuyCount = 1;
                #endregion
                break;
            case 6:
                #region 冠亚军玩法
                switch(buytype.Type)
                {
                    case 2://和值大小
                    case 3://和值单双
                        #region
                        ntype = new Random().Next(1, 10); //用Random().Next(0，1)总返回0；
                        newNums = (ntype > 5) ? "1" : "0";
                        buy.Nums = newNums;
                        buy.NumsDetail = newNums;
                        buy.BuyCount = 1;
                        #endregion
                        break;
                    default: //和值
                        #region 直接生成号码组号
                        strAll = "3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19";
                        newNums = CreateRandomString(null, strAll, nums, "");
                        newNums = newNums.Replace(',', '|');
                        buy.Nums = newNums;
                        buy.NumsDetail = newNums;
                        buy.BuyCount = 1;
                        #endregion
                        break;
                }
                #endregion
                break;
        }
    }
    private string CreateRandomString(Random rm,string strSource, int resultLength,string strExclude) //从字符串strSource（每个独立字符用","分隔），取得随机组合而且无重复出现字符的字符串（长度为resultLength）
    {
        string result = "";
        List<string> list = new List<string>();
        //
        #region 建立查询列表
        if (strSource.Length > 0)
        {
            string[] astr = strSource.Split(',');
            if (astr.Length > 0)
            {
                for (int i = 0; i < astr.Length; i++)
                    list.Add(astr[i]);
            }
        }
        #endregion
        #region 排除字符
        if (strExclude.Length > 0)
        {
            string[] astr1 = strExclude.Split(',');
            if (astr1.Length > 0)
            {
                for (int i = 0; i < astr1.Length; i++)
                {
                    if (list.Contains(astr1[i]))
                        list.Remove(astr1[i]);
                }
            }
        }
        #endregion
        #region 生成随机不重复组合
        int length = list.Count;
        if (length > 0 && length >= resultLength)
        {
            List<string> list1 = new List<string>();
            #region 循环
            if (rm == null)
                rm = new Random();
            for (int i = 0; list1.Count < resultLength; i++)
            {
                int nIndex = rm.Next(list.Count);
                string cValue = list[nIndex];
                if (!list1.Contains(cValue))
                {
                    list1.Add(cValue);
                    list.Remove(cValue);
                }
            }
            #endregion
            #region 将结果List1转为“，”分割的字符串
            if (list1.Count > 0)
            {
                for (int i = 0; i < list1.Count; i++)
                {
                    if (i==0)
                        result = list1[i];
                    else
                        result += "," + list1[i];
                }
            }
            #endregion
        }
        #endregion
        //
        return result;
    }

}