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
using System.Text.RegularExpressions;
using BCW.Common;

public partial class Robot_Luck28Robot : System.Web.UI.Page
{
    protected string xmlPath = "/Controls/luck28.xml";
    protected string GameName = ub.GetSub("Luck28Name", "/Controls/luck28.xml");//游戏名字

    protected void Page_Load(object sender, EventArgs e)
    {
        //防止缓存
        Response.Buffer = true;
        Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
        Response.Expires = 0;
        Response.CacheControl = "no-cache";

        int RobotSet = Utils.ParseInt(ub.GetSub("Luck28RobotSet", xmlPath));//0开，关1
        if (RobotSet == 1)
        {
            Response.Write("<b>" + GameName + "_机器人处于关闭状态close1</b>");
            // Response.End();
        }
        else
        {
            ChangePalyLuck();
            // Response.Write("ok");
        }
    }

    /// <summary>
    /// 幸运28自动游戏程序
    /// </summary>
    /// <param name="LuckId">局数ID</param>
    /// <param name="dt">截止时间</param>
    private void ChangePalyLuck()
    {
        BCW.Model.Game.Lucklist luck = null;
        //new BCW.User.Luck28().Luck28Page();
        luck = new BCW.BLL.Game.Lucklist().GetNextLucklist();
        int LuckId = luck.ID;
        #region
        //long Sec = DT.DateDiff(luck.EndTime, DateTime.Now, 4);
        //if (LuckId > 0)
        //{
        //    //int UsIDNum = new BCW.BLL.Game.Luckpay().GetCount(LuckId);//当期下注ID数
        //    int UsIDNum = Utils.ParseInt(ub.GetSub("Luck28UsIDNum", xmlPath));

        //    //如果小于N个则自在不同秒数中自动出动1个ID来下注
        //    int BotNum = Utils.ParseInt(ub.GetSub("Luck28BotNum", xmlPath));
        //    if (BotNum == 0)
        //        BotNum = new Random().Next(3, 6);

        //    int BotLuckId = Utils.ParseInt(ub.GetSub("Luck28BotLuckId", xmlPath));
        //    if (BotLuckId != LuckId)
        //    {
        //        BotNum = new Random().Next(3, 6);
        //        UsIDNum = 0;
        //    }
        //    if (UsIDNum < BotNum || BotLuckId != LuckId)
        //    {
        //        int BotSec = new Random().Next(30, 50);
        //        DateTime BotTime = Convert.ToDateTime(ub.GetSub("Luck28BotTime", xmlPath));
        //        if (BotTime < DateTime.Now.AddSeconds(-BotSec))
        //        {
        //            //更新某分钟已出动过
        //            ub xml = new ub();
        //            xml.ReloadSub(xmlPath); //加载配置
        //            xml.dss["Luck28BotTime"] = DT.FormatDate(DateTime.Now, 0);
        //            xml.dss["Luck28BotLuckId"] = LuckId.ToString();
        //            xml.dss["Luck28BotNum"] = BotNum.ToString();
        //            xml.dss["Luck28UsIDNum"] = (UsIDNum + 1).ToString();

        //            System.IO.File.WriteAllText(HttpContext.Current.Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
        //            //进行自动下注
        //            PlayLuck(LuckId);
        //        }
        //    }
        //}
        #endregion

        //进行自动下注
        int hour = DateTime.Now.Hour;
        if (hour > 23 || hour < 9)
        {
            Response.Write("" + GameName + "_机器人已休息!close1");
        }
        else
        {
            //try
            //{
            if (luck.Bjkl8Qihao > 2)
            {
                PlayLuck(LuckId); ;
                Response.Write("" + GameName + "_机器人正在工作中ok1!");
            }
            else
            {
                Response.Write("" + GameName + "_机器人购买失败!error1");
            }
            //}
            //catch
            //{
            //    Response.Write("" + GameName + "_机器人购买失败！系统未开期数error1");
            //}

        }
    }

    //机器人购买下注操作
    private void PlayLuck(int LuckId)
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
        #region  隐藏财产
        string ForumSet = new BCW.BLL.User().GetForumSet(meid);
        string[] fs = ForumSet.Split(",".ToCharArray());
        string sforumsets = string.Empty;
        for (int i = 0; i < fs.Length; i++)
        {
            string[] sfs = fs[i].ToString().Split("|".ToCharArray());
            if (i == 24)
            {
                if (string.IsNullOrEmpty(sforumsets))
                {
                    sforumsets += sfs[0] + "|" + 1;
                }
                else
                {
                    sforumsets += "," + sfs[0] + "|" + 1;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(sforumsets))
                {
                    sforumsets += sfs[0] + "|" + sfs[1];
                }
                else
                {
                    sforumsets += "," + sfs[0] + "|" + sfs[1];
                }
            }
        }
        new BCW.BLL.User().UpdateForumSet(meid, sforumsets);
        #endregion
        if (IsOpen() == false)
        {
            //Response.Write("" + GameName + "_机器人已休息close1!");
            //Response.End();
        }
        ChanageOnline(meid);

        //得到下注的类型  待写
        int num1 = Get28Type();
        //int num1 = 13;

        int bzType = 0;// new Random().Next(0, 2);
        long paycent = GetPayCent();//得到下注金额
                                    // Utils.Error("paycent:"+ paycent, "");
        string mename = new BCW.BLL.User().GetUsName(meid);

        long gold = 0;
        string bzText = string.Empty;
        if (bzType == 0)
        {
            gold = new BCW.BLL.User().GetGold(meid);
            bzText = ub.Get("SiteBz");
        }
        else
        {
            gold = new BCW.BLL.User().GetMoney(meid);
            bzText = ub.Get("SiteBz2");
        }
        #region  读XML
        long Luck28BigCents = Utils.ParseInt(ub.GetSub("Luck28BigCents", xmlPath));//半数浮动设置下注额
        long Luck28SmallCents = Utils.ParseInt(ub.GetSub("Luck28SmallCents", xmlPath));

        long Luck28SingleCents = Utils.ParseInt(ub.GetSub("Luck28SingleCents", xmlPath));
        long Luck28DoubleCents = Utils.ParseInt(ub.GetSub("Luck28DoubleCents", xmlPath));

        long Luck28BigSingleCents = Utils.ParseInt(ub.GetSub("Luck28BigSingleCents", xmlPath));
        long Luck28SmallSingleCents = Utils.ParseInt(ub.GetSub("Luck28SmallSingleCents", xmlPath));

        long Luck28BigDoubleCents = Utils.ParseInt(ub.GetSub("Luck28BigDoubleCents", xmlPath));
        long Luck28SmallDoubleCents = Utils.ParseInt(ub.GetSub("Luck28SmallDoubleCents", xmlPath));

        long Luck28FirstCents = Utils.ParseInt(ub.GetSub("Luck28FirstCents", xmlPath));//分段每个段设置下注额
        long Luck28SecendCents = Utils.ParseInt(ub.GetSub("Luck28SecendCents", xmlPath));

        long Luck28ThreeCents = Utils.ParseInt(ub.GetSub("Luck28ThreeCents", xmlPath));
        long Luck28AllCents = Utils.ParseInt(ub.GetSub("Luck28AllCents", xmlPath));

        string FloatBig = (ub.GetSub("FloatBig", xmlPath));//得到浮动赔率
        string FloatSmall = (ub.GetSub("FloatSmall", xmlPath));
        string FloatSingle = (ub.GetSub("FloatSingle", xmlPath));
        string FloatDouble = (ub.GetSub("FloatDouble", xmlPath));

        string bigsingleodds = ub.GetSub("Luck28BigSingle", xmlPath);
        string smallsingleodds = ub.GetSub("Luck28SmallSingle", xmlPath);
        string bigdoubleoddds = ub.GetSub("Luck28BigDouble", xmlPath);
        string smalldoubleodds = ub.GetSub("Luck28SmallDouble", xmlPath);
        string firstodds = ub.GetSub("Luck28First", xmlPath);
        string secendodds = ub.GetSub("Luck28Secend", xmlPath);
        string threeodds = ub.GetSub("Luck28Three", xmlPath);

        long Big = new BCW.BLL.Game.Luckpay().GetSumBuyTypeCent(LuckId, "Luck28Big");
        long small = new BCW.BLL.Game.Luckpay().GetSumBuyTypeCent(LuckId, "Luck28Small");

        long single = new BCW.BLL.Game.Luckpay().GetSumBuyTypeCent(LuckId, "Luck28Single");
        long doubl = new BCW.BLL.Game.Luckpay().GetSumBuyTypeCent(LuckId, "Luck28Double");

        long bigsingle = new BCW.BLL.Game.Luckpay().GetSumBuyTypeCent(LuckId, "Luck28BigSingle");
        long smallsingle = new BCW.BLL.Game.Luckpay().GetSumBuyTypeCent(LuckId, "Luck28SmallSingle");

        long bigdouble = new BCW.BLL.Game.Luckpay().GetSumBuyTypeCent(LuckId, "Luck28BigDouble");
        long smalldouble = new BCW.BLL.Game.Luckpay().GetSumBuyTypeCent(LuckId, "Luck28SmallDouble");

        long first = new BCW.BLL.Game.Luckpay().GetSumBuyTypeCent(LuckId, "Luck28First");
        long secend = new BCW.BLL.Game.Luckpay().GetSumBuyTypeCent(LuckId, "Luck28Secend");

        long three = new BCW.BLL.Game.Luckpay().GetSumBuyTypeCent(LuckId, "Luck28Three");
        long all = new BCW.BLL.Game.Luckpay().GetSumBuyTypeCent(LuckId, "Luck28All");
        long MaxPool = Utils.ParseInt(ub.GetSub("Luck28MaxPool", xmlPath));//每期下注总额
        long hadbuy = new BCW.BLL.Game.Luckpay().GetPrice("BuyCents", "LuckId=" + LuckId);//得到某一期所有玩家的下注金额
        long paysmall = Convert.ToInt64(ub.GetSub("Luck28SmallPay", xmlPath));
        long paybig = Convert.ToInt64(ub.GetSub("Luck28BigPay", xmlPath));
        #endregion
        //机器人每一期的最高购买次数
        int buyCount = Convert.ToInt32(ub.GetSub("Luck28RobotBuy", xmlPath));
        string BuyNum = "";
        string EngText = "";
        string Text = "";
        string myend = "";
        string odds = "";
        string weishu = string.Empty;
        int flag = 1; //是否可以下注
        long allBuyCent = 0;//总的下注额                  

        #region 机械人下注
        //  else
        //{
        BCW.Model.Game.Lucklist luck = new BCW.BLL.Game.Lucklist().GetNextLucklist();//得到最新一期
        long wanjia = 0;
        int count = new BCW.BLL.Game.Luckpay().GetRobotbuy(meid, luck.ID);//得到机械人的下注次数

        if ((count < buyCount) && (buyCount != 0))//可以下注
        {
            #region 机械人下注


            #region 大小单双
            if (num1 == 1)
            {
                BuyNum = "14,15,16,17,18,19,20,21,22,23,24,25,26,27";
                EngText = "Luck28Big";
                Text = "押大";
                wanjia = paycent;
                allBuyCent = paycent;
                if ((wanjia) > Luck28BigCents - Big + small)
                {
                    flag = 0;
                }
                if ((allBuyCent + hadbuy) > MaxPool)//当前玩家下注+之前所有玩家下注 >  当期最大下注金额
                {
                    flag = 0;
                }
                if (allBuyCent > paybig || allBuyCent < paysmall)//超过单注下注范围
                {
                    flag = 0;
                }
                odds = FloatBig;
            }
            else if (num1 == 2)
            {
                BuyNum = "0,1,2,3,4,5,6,7,8,9,10,11,12,13";
                Text = "小";
                EngText = "Luck28Small";
                wanjia = paycent;
                allBuyCent = paycent;
                if ((wanjia) > (Luck28SmallCents - small + Big))
                {
                    flag = 0;
                }
                if ((allBuyCent + hadbuy) > MaxPool)//当前玩家下注+之前所有玩家下注 >  当期最大下注金额
                {
                    flag = 0;
                }
                if (allBuyCent > paybig || allBuyCent < paysmall)//超过单注下注范围
                {
                    flag = 0;
                }
                odds = FloatSmall;
            }
            else if (num1 == 3)
            {
                BuyNum = "1,3,5,7,9,11,13,15,17,19,21,23,25,27";
                Text = "单";
                EngText = "Luck28Single";
                wanjia = paycent;
                allBuyCent = paycent;
                if ((wanjia) > (Luck28SingleCents - single + doubl))
                {
                    flag = 0;
                }
                if ((allBuyCent + hadbuy) > MaxPool)//当前玩家下注+之前所有玩家下注 >  当期最大下注金额
                {
                    flag = 0;
                }
                if (allBuyCent > paybig || allBuyCent < paysmall)//超过单注下注范围
                {
                    flag = 0;
                }
                odds = FloatSingle;
            }
            else if (num1 == 4)
            {

                BuyNum = "0,2,4,6,8,10,12,14,16,18,20,22,24,26";
                Text = "双";
                EngText = "Luck28Double";
                wanjia = paycent;
                allBuyCent = paycent;
                if ((wanjia) > (Luck28DoubleCents - doubl + single))
                {
                    flag = 0;
                }
                if ((allBuyCent + hadbuy) > MaxPool)//当前玩家下注+之前所有玩家下注 >  当期最大下注金额
                {
                    flag = 0;
                }
                if (allBuyCent > paybig || allBuyCent < paysmall)//超过单注下注范围
                {
                    flag = 0;
                }
                odds = FloatDouble;
            }
            else if (num1 == 5)
            {

                BuyNum = "15,17,19,21,23,25,27";
                Text = "大单";
                EngText = "Luck28BigSingle";
                wanjia = paycent;
                allBuyCent = paycent;
                if ((wanjia) > (Luck28BigSingleCents - bigsingle + smallsingle))
                {
                    flag = 0;
                }
                if ((allBuyCent + hadbuy) > MaxPool)//当前玩家下注+之前所有玩家下注 >  当期最大下注金额
                {
                    flag = 0;
                }
                if (allBuyCent > paybig || allBuyCent < paysmall)//超过单注下注范围
                {
                    flag = 0;
                }
                odds = bigsingleodds.ToString();
            }
            else if (num1 == 6)
            {

                BuyNum = "1,3,5,7,9,11,13";
                Text = "小单";
                EngText = "Luck28SmallSingle";
                wanjia = paycent;
                allBuyCent = paycent;
                if ((wanjia) > (Luck28SmallSingleCents - smallsingle + bigsingle))
                {
                    flag = 0;
                }
                if ((allBuyCent + hadbuy) > MaxPool)//当前玩家下注+之前所有玩家下注 >  当期最大下注金额
                {
                    flag = 0;
                }
                if (allBuyCent > paybig || allBuyCent < paysmall)//超过单注下注范围
                {
                    flag = 0;
                }
                odds = smallsingleodds.ToString();
            }
            else if (num1 == 7)
            {

                BuyNum = "14,16,18,20,22,24,26";
                Text = "大双";
                EngText = "Luck28BigDouble";
                wanjia = paycent;
                allBuyCent = paycent;
                if ((wanjia) > (Luck28BigDoubleCents - bigdouble + smalldouble))
                {
                    flag = 0;
                }
                if ((allBuyCent + hadbuy) > MaxPool)//当前玩家下注+之前所有玩家下注 >  当期最大下注金额
                {
                    flag = 0;
                }
                if (allBuyCent > paybig || allBuyCent < paysmall)//超过单注下注范围
                {
                    flag = 0;
                }
                odds = bigdoubleoddds.ToString();
            }
            else if (num1 == 8)
            {
                BuyNum = "0,2,4,6,8,10,12";
                Text = "小双";
                EngText = "Luck28SmallDouble";
                wanjia = paycent;
                allBuyCent = paycent;
                if ((wanjia) > (Luck28SmallDoubleCents - smalldouble + bigdouble))
                {
                    flag = 0;
                }
                if ((allBuyCent + hadbuy) > MaxPool)//当前玩家下注+之前所有玩家下注 >  当期最大下注金额
                {
                    flag = 0;
                }
                if (allBuyCent > paybig || allBuyCent < paysmall)//超过单注下注范围
                {
                    flag = 0;
                }
                odds = smalldoubleodds.ToString();
            }
            else if (num1 == 9)
            {
                BuyNum = "0,1,2,3,4,5,6,7,8,9";
                Text = "一段";
                EngText = "Luck28First";
                wanjia = paycent;
                allBuyCent = paycent;
                if ((wanjia) > (Luck28FirstCents - first))
                {
                    flag = 0;
                }
                if ((allBuyCent + hadbuy) > MaxPool)//当前玩家下注+之前所有玩家下注 >  当期最大下注金额
                {
                    flag = 0;
                }
                if (allBuyCent > paybig || allBuyCent < paysmall)//超过单注下注范围
                {
                    flag = 0;
                }
                odds = firstodds.ToString();
            }
            else if (num1 == 10)
            {
                BuyNum = "10,11,12,13,14,15,16,17,18";
                Text = "二段";
                EngText = "Luck28Secend";
                wanjia = paycent;
                allBuyCent = paycent;
                if ((wanjia) > (Luck28SecendCents - secend))
                {
                    flag = 0;
                }
                if ((allBuyCent + hadbuy) > MaxPool)//当前玩家下注+之前所有玩家下注 >  当期最大下注金额
                {
                    flag = 0;
                }
                if (allBuyCent > paybig || allBuyCent < paysmall)//超过单注下注范围
                {
                    flag = 0;
                }
                odds = secendodds.ToString();
            }
            else if (num1 == 11)
            {
                BuyNum = "19,20,21,22,23,24,25,26,27";
                Text = "三段";
                EngText = "Luck28Three";
                wanjia = paycent;
                allBuyCent = paycent;
                if ((wanjia) > (Luck28ThreeCents - three))
                {
                    flag = 0;
                }
                if ((allBuyCent + hadbuy) > MaxPool)//当前玩家下注+之前所有玩家下注 >  当期最大下注金额
                {
                    flag = 0;
                }
                if (allBuyCent > paybig || allBuyCent < paysmall)//超过单注下注范围
                {
                    flag = 0;
                }
                odds = threeodds.ToString();
            }
            #endregion
            else if (num1 == 12)
            {
                BuyNum = Choose(27);
                Text = "自选";
                EngText = "Luck28Choose";
                wanjia = paycent;
                //  Response.Write("产生的自选号码是:" + BuyNum + "<>br/");
                string[] a = BuyNum.Split(',');
                allBuyCent = paycent * a.Length;
                for (int i = 0; i < a.Length; i++)
                {
                    string haoma = a[i].ToString();//得到选的第几个号码
                    long canbuy = Utils.ParseInt(ub.GetSub("Luck28Buy" + haoma, xmlPath));//得到每个号码最大下注金额
                    long hasbuy = new BCW.BLL.Game.Luckpay().GetSumBuyCentbychoose(luck.ID, haoma);  //得到某数字的已下的总下注金额
                    if (hasbuy + paycent > canbuy)  //某数字的总下注金额 + 玩家单个数下注金额   >  该号码最大下注金额
                    {
                        flag = 0;
                    }
                    if ((allBuyCent + hadbuy) > MaxPool)//当前玩家下注+之前所有玩家下注 >  当期最大下注金额
                    {
                        flag = 0;
                    }
                }
                if (allBuyCent > paybig || allBuyCent < paysmall)//超过单注下注范围
                {
                    flag = 0;
                }
            }
            else if (num1 == 13)
            {
                BuyNum = Choose(10);
                myend = BuyNum;
                Text = "尾号";
                EngText = "Luck28End";
                wanjia = paycent;
                //  Response.Write("产生的尾号是:" + BuyNum + "<br/>");
                string[] a = BuyNum.Split(',');
                string getWS = "";
                for (int i = 0; i < a.Length; i++)
                {
                    #region 尾数判断
                    if (a[i] == "0")
                    {
                        getWS = "0,10,20";
                        long canbuy = Utils.ParseInt(ub.GetSub("Luck28Last0Cents", xmlPath));//得到号码最大下注金额
                        long hasbuy = new BCW.BLL.Game.Luckpay().GetSumBuyCent(luck.ID, getWS);  //得到某数字的总下注金额   
                                                                                                 // Response.Write("尾数:" + getWS+"<br/>");
                        if (((hasbuy + (paycent))) > canbuy)  //某数字的总下注金额 + 玩家单个数下注金额   >  号码最大下注金额
                        {
                            flag = 0;
                        }
                        if ((allBuyCent + hadbuy) > MaxPool)//当前玩家下注+之前所有玩家下注 >  当期最大下注金额
                        {
                            flag = 0;
                        }
                    }
                    else if (a[i] == "1")
                    {
                        getWS = "1,11,21";
                        long canbuy = Utils.ParseInt(ub.GetSub("Luck28Last1Cents", xmlPath));//得到号码最大下注金额
                        long hasbuy = new BCW.BLL.Game.Luckpay().GetSumBuyCent(luck.ID, getWS);  //得到某数字的总下注金额      
                                                                                                 // Response.Write("尾数:" + getWS + "<br/>");
                        if (((hasbuy + (paycent))) > canbuy)  //某数字的总下注金额 + 玩家单个数下注金额   >  号码最大下注金额
                        {

                            flag = 0;
                        }
                        if ((allBuyCent + hadbuy) > MaxPool)//当前玩家下注+之前所有玩家下注 >  当期最大下注金额
                        {
                            flag = 0;
                        }
                    }
                    else if (a[i] == "2")
                    {
                        getWS = "2,12,22";
                        long canbuy = Utils.ParseInt(ub.GetSub("Luck28Last2Cents", xmlPath));//得到号码最大下注金额
                        long hasbuy = new BCW.BLL.Game.Luckpay().GetSumBuyCent(luck.ID, getWS);  //得到某数字的总下注金额  
                                                                                                 // Response.Write("尾数:" + getWS + "<br/>");
                        if (((hasbuy + (paycent))) > canbuy)  //某数字的总下注金额 + 玩家单个数下注金额   >  号码最大下注金额
                        {
                            flag = 0;
                        }
                        if ((allBuyCent + hadbuy) > MaxPool)//当前玩家下注+之前所有玩家下注 >  当期最大下注金额
                        {
                            flag = 0;
                        }
                    }
                    else if (a[i] == "3")
                    {
                        getWS = "3,13,23";
                        long canbuy = Utils.ParseInt(ub.GetSub("Luck28Last3Cents", xmlPath));//得到号码最大下注金额
                        long hasbuy = new BCW.BLL.Game.Luckpay().GetSumBuyCent(luck.ID, getWS);  //得到某数字的总下注金额     
                                                                                                 // Response.Write("尾数:" + getWS + "<br/>");
                        if (((hasbuy + (paycent))) > canbuy)  //某数字的总下注金额 + 玩家单个数下注金额   >  号码最大下注金额
                        {
                            flag = 0;
                        }
                        if ((allBuyCent + hadbuy) > MaxPool)//当前玩家下注+之前所有玩家下注 >  当期最大下注金额
                        {
                            flag = 0;
                        }
                    }
                    else if (a[i] == "4")
                    {
                        getWS = "4,14,24";
                        long canbuy = Utils.ParseInt(ub.GetSub("Luck28Last4Cents", xmlPath));//得到号码最大下注金额
                        long hasbuy = new BCW.BLL.Game.Luckpay().GetSumBuyCent(luck.ID, getWS);  //得到某数字的总下注金额  
                                                                                                 // Response.Write("尾数:" + getWS + "<br/>");
                        if (((hasbuy + (paycent))) > canbuy)  //某数字的总下注金额 + 玩家单个数下注金额   >  号码最大下注金额
                        {
                            flag = 0;
                        }
                        if ((allBuyCent + hadbuy) > MaxPool)//当前玩家下注+之前所有玩家下注 >  当期最大下注金额
                        {
                            flag = 0;
                        }
                    }
                    else if (a[i] == "5")
                    {
                        getWS = "5,15,25";
                        long canbuy = Utils.ParseInt(ub.GetSub("Luck28Last5Cents", xmlPath));//得到号码最大下注金额
                        long hasbuy = new BCW.BLL.Game.Luckpay().GetSumBuyCent(luck.ID, getWS);  //得到某数字的总下注金额  
                                                                                                 //  Response.Write("尾数:" + getWS + "<br/>");
                        if (((hasbuy + (paycent))) > canbuy)  //某数字的总下注金额 + 玩家单个数下注金额   >  号码最大下注金额
                        {
                            flag = 0;
                        }
                        if ((allBuyCent + hadbuy) > MaxPool)//当前玩家下注+之前所有玩家下注 >  当期最大下注金额
                        {
                            flag = 0;
                        }
                    }
                    else if (a[i] == "6")
                    {
                        getWS = "6,16,26";
                        long canbuy = Utils.ParseInt(ub.GetSub("Luck28Last6Cents", xmlPath));//得到号码最大下注金额
                        long hasbuy = new BCW.BLL.Game.Luckpay().GetSumBuyCent(luck.ID, getWS);  //得到某数字的总下注金额   
                                                                                                 // Response.Write("尾数:" + getWS + "<br/>");
                        if (((hasbuy + (paycent))) > canbuy)  //某数字的总下注金额 + 玩家单个数下注金额   >  号码最大下注金额
                        {
                            flag = 0;
                        }
                        if ((allBuyCent + hadbuy) > MaxPool)//当前玩家下注+之前所有玩家下注 >  当期最大下注金额
                        {
                            flag = 0;
                        }
                    }
                    else if (a[i] == "7")
                    {
                        getWS = "7,17,27";
                        long canbuy = Utils.ParseInt(ub.GetSub("Luck28Last7Cents", xmlPath));//得到号码最大下注金额
                        long hasbuy = new BCW.BLL.Game.Luckpay().GetSumBuyCent(luck.ID, getWS);  //得到某数字的总下注金额    
                                                                                                 //  Response.Write("尾数:" + getWS + "<br/>");
                        if (((hasbuy + (paycent))) > canbuy)  //某数字的总下注金额 + 玩家单个数下注金额   >  号码最大下注金额
                        {
                            flag = 0;
                        }
                        if ((allBuyCent + hadbuy) > MaxPool)//当前玩家下注+之前所有玩家下注 >  当期最大下注金额
                        {
                            flag = 0;
                        }
                    }
                    else if (a[i] == "8")
                    {
                        getWS = "8,18";
                        long canbuy = Utils.ParseInt(ub.GetSub("Luck28Last8Cents", xmlPath));//得到号码最大下注金额
                        long hasbuy = new BCW.BLL.Game.Luckpay().GetSumBuyCent(luck.ID, getWS);  //得到某数字的总下注金额  
                                                                                                 // Response.Write("尾数:" + getWS + "<br/>");
                        if (((hasbuy + (paycent))) > canbuy)  //某数字的总下注金额 + 玩家单个数下注金额   >  号码最大下注金额
                        {
                            flag = 0;
                        }
                        if ((allBuyCent + hadbuy) > MaxPool)//当前玩家下注+之前所有玩家下注 >  当期最大下注金额
                        {
                            flag = 0;
                        }
                    }
                    else if (a[i] == "9")
                    {
                        getWS = "9,19";
                        long canbuy = Utils.ParseInt(ub.GetSub("Luck28Last9Cents", xmlPath));//得到号码最大下注金额
                        long hasbuy = new BCW.BLL.Game.Luckpay().GetSumBuyCent(luck.ID, getWS);  //得到某数字的总下注金额   
                                                                                                 //Response.Write("尾数:" + getWS + "<br/>");
                        if (((hasbuy + (paycent))) > canbuy)  //某数字的总下注金额 + 玩家单个数下注金额   >  号码最大下注金额
                        {
                            flag = 0;
                        }
                        if ((allBuyCent + hadbuy) > MaxPool)//当前玩家下注+之前所有玩家下注 >  当期最大下注金额
                        {
                            flag = 0;
                        }
                    }

                    if (string.IsNullOrEmpty(weishu))
                    {
                        weishu = weishu + getWS;
                    }
                    else
                    {
                        weishu = weishu + "," + getWS;
                    }
                    #endregion
                }
                BuyNum = weishu;
                //Response.Write("尾数是:" + weishu + "<br/>");
                //Response.Write("BuyNum:" + BuyNum + "<br/>");
                string[] Temp1 = myend.Split(',');
                //  Response.Write("尾数总数:" + Temp1.Length + "<br/>");
                allBuyCent = paycent * Temp1.Length;
                if ((allBuyCent + hadbuy) > MaxPool)//当前玩家下注+之前所有玩家下注 >  当期最大下注金额
                {
                    flag = 0;
                }
                //Response.Write("wanjia:" + wanjia + "<br/>");
                //Response.Write("Temp1.Length:" + Temp1.Length + "<br/>");
                //Response.Write("allBuyCent:" + allBuyCent + "<br/>");
                if (allBuyCent > paybig || allBuyCent < paysmall)//超过单注下注范围
                {
                    flag = 0;
                }


            }
            long myIDPay = new BCW.BLL.Game.Luckpay().GetPrice("BuyCents", "LuckId=" + LuckId + " and UsID=" + meid);//得到这一期该玩家的总下注额
            long XMLEveryPay = Convert.ToInt64(ub.GetSub("Luck28EveryPay", xmlPath));//限制每个ID每期下注的金额

            if (myIDPay + allBuyCent > XMLEveryPay)
            {
                flag = 0;
                Response.Write("每期每ID最多可下" + XMLEveryPay + ub.Get("SiteBz") + ",你已下" + myIDPay + ub.Get("SiteBz"));
                // Utils.Error("每期每ID最多可下" + XMLEveryPay + ub.Get("SiteBz") + ",你已下" + myIDPay + ub.Get("SiteBz"), "");
            }
            #endregion

            if (flag == 1)//可以下注并且不超过最大下注额
            {
                if (gold < allBuyCent || gold < 0)
                {
                    // Response.Write("<b>机器人" + meid + "币不够！请换一个机器人或者给该机器人充值 </b><br />");
                    //更新消费记录
                    BCW.Model.Goldlog modelx = new BCW.Model.Goldlog();
                    modelx.BbTag = 3;
                    modelx.Types = 0;
                    modelx.PUrl = Utils.getPageUrl();//操作的文件名
                    modelx.UsId = meid;
                    modelx.UsName = mename;

                    modelx.AcGold = paycent;
                    modelx.AfterGold = gold + allBuyCent;//更新后的币数

                    modelx.AcText = "系统机器人自动操作";
                    modelx.AddTime = DateTime.Now;
                    new BCW.BLL.Goldlog().Add(modelx);
                    BCW.Data.SqlHelper.ExecuteSql("Update tb_User set iGold=iGold+(" + allBuyCent + ") where id=" + meid + "");
                }
                //   Utils.Error("allBuyCent:"+ allBuyCent+ ",EngText:"+ EngText, "");
                Response.Write("paycent:" + paycent + "<br/>");
                Response.Write("BuyNum:" + BuyNum + "<br/>");
                Response.Write("allBuyCent:" + allBuyCent + "<br/>");
                Response.Write("EngText:" + EngText + "<br/>");
                Response.Write("flag:" + flag + "<br/>");
                //return;
                //  Response.Write("尾数:" + myend + "<br/>");
                count++;//机械人下注次数加一
                Response.Write(meid + "机械人可以加入数据库:" + count + "<br/>");
                BCW.Model.Game.Luckpay model = new BCW.Model.Game.Luckpay();
                model.LuckId = LuckId;
                model.UsID = meid;
                model.UsName = mename;
                model.BuyCent = paycent;
                model.BuyCents = allBuyCent;
                model.BuyNum = BuyNum;
                model.BuyType = EngText;
                model.State = 0;
                model.IsRobot = 1;//是机械人下注
                model.WinCent = 0;
                model.AddTime = DateTime.Now;
                if (odds != "")
                {
                    model.odds = odds;
                }
                else
                {
                    model.odds = "1";
                }
                // Response.Write("allBuyCent:" + allBuyCent + "<br/>");
                int id = new BCW.BLL.Game.Luckpay().Add(model);
                if (EngText == "Luck28Choose")
                {
                    //向102加钱
                    //  new BCW.BLL.User().UpdateiGold(102, new BCW.BLL.User().GetUsName(102), allBuyCent, "ID:" + meid + "二八第" + luck.Bjkl8Qihao + "期买" + Text + BuyNum + "共" + allBuyCent + "-标识" + id + "");
                    //扣币
                    new BCW.BLL.User().UpdateiGold(meid, new BCW.BLL.User().GetUsName(meid), -allBuyCent, "二八第" + luck.Bjkl8Qihao + "期押" + Text + BuyNum + "-标识" + id + "");
                }
                else if (Text == "Luck28End")
                {

                    new BCW.BLL.User().UpdateiGold(meid, new BCW.BLL.User().GetUsName(meid), -allBuyCent, "二八第" + luck.Bjkl8Qihao + "期押" + Text + "" + myend + "-标识" + id + "");
                }
                else
                {
                    new BCW.BLL.User().UpdateiGold(meid, new BCW.BLL.User().GetUsName(meid), -allBuyCent, "二八第" + luck.Bjkl8Qihao + "期押" + Text + ",赔率:" + model.odds + "-标识" + id + "");
                }
                string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/luck28.aspx]幸运28第" + luck.Bjkl8Qihao + "期[/url]下注**" + ub.Get("SiteBz") + "";//" + Convert.ToInt64(allBuyCent) + "
                new BCW.BLL.Action().Add(1020, id, 0, "", wText);
            }
            else
            {
                // Response.Write(meid + "机械人下注过大："+allBuyCent+"<br/>" );
            }

        }
        else
        {
            Response.Write(meid + "机械人不能再下注啦：" + count);
        }
    }
    #endregion

    /// <summary>
    /// 随机的投注方式
    /// </summary>
    /// <returns></returns>
    private int Get28Type()
    {
        Random rac = new Random();
        int ptype = rac.Next(1, 14);//1大，2小，3单，4双，5大单，6小单，7大双，8小双，9一段，10二段，11三段，12自选，13尾号
        System.Threading.Thread.Sleep(50);//延时
        return ptype;
    }
    /// <summary>
    /// 随机产生号码
    /// </summary>
    private string Choose(int num)
    {
        Random rac = new Random();
        string choose = string.Empty;
        int a = num;
        int length = rac.Next(1, a);//随机产生需要购买多少个号码
        while (choose.Length < length)
        {
            int temp = rac.Next(0, a);
            if (!choose.Contains(temp + ""))
            {
                if (string.IsNullOrEmpty(choose))
                {
                    choose = choose + temp;
                }
                else
                {
                    choose = choose + "," + temp;
                }
            }
        }
        choose = sort(choose);
        return choose;
    }
    /// <summary>
    /// 号码小到大排序
    /// </summary>
    /// <returns></returns>
    private string sort(string str)
    {
        string[] num = str.Split(',');
        int[] arry = new int[num.Length];
        int temp = 0;
        for (int i = 0; i < num.Length; i++)
        {
            arry[i] = Utils.ParseInt(num[i]);
        }
        for (int k = 0; k < arry.Length; k++)
        {
            for (int j = 0; j < arry.Length - k - 1; j++)
            {
                if (arry[j] > arry[j + 1])
                {
                    temp = arry[j];
                    arry[j] = arry[j + 1];
                    arry[j + 1] = temp;
                }
            }
        }
        string sortstr = string.Empty;
        for (int i = 0; i < arry.Length; i++)  //排序后的数组
        {
            if (string.IsNullOrEmpty(sortstr))
            {
                sortstr = sortstr + arry[i].ToString();
            }
            else
            {
                sortstr = sortstr + "," + arry[i].ToString();
            }

        }
        return sortstr;
    }

    //得到出动的ID
    private int GetUsID()
    {
        int UsID = 0;
        string PlayUsID = ub.GetSub("Luck28RobotIDS", "/Controls/luck28.xml");
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

    //随机得到下注的币数
    private long GetPayCent()
    {
        Random rac = new Random(unchecked((int)DateTime.Now.Ticks));
        long paycent = 0;

        string cent = ub.GetSub("Luck28RobotCent", xmlPath);
        string[] temp = cent.Split("#".ToCharArray());
        System.Threading.Thread.Sleep(200);//延时
        int index = rac.Next(temp.Length);

        paycent = Convert.ToInt64(temp[index]);

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

    //开放时间计算
    private bool IsOpen()
    {
        bool IsOpen = true;
        string OnTime = ub.GetSub("Luck28OnTime", xmlPath);
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
        }
        return IsOpen;
    }
}
