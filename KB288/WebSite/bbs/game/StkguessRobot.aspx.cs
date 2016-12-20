using System;
using BCW.Common;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Data;

/// <summary>
///蒙宗将 20161125 增加单期单投注方式每id限额
/// </summary>

public partial class bbs_game_StkguessRobot : System.Web.UI.Page
{
    protected string xmlPath = "/Controls/stkguess.xml";
    protected string GameName = ub.GetSub("StkName", "/Controls/stkguess.xml");//游戏名字
    protected int gid = 32;

    protected void Page_Load(object sender, EventArgs e)
    {
        //防止缓存
        Response.Buffer = true;
        Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
        Response.Expires = 0;
        Response.CacheControl = "no-cache";

        if (ub.GetSub("StkIsBot", xmlPath) != "1")
        {
            Response.Write("<b>" + GameName + "_机器人处于关闭状态close1</b>");
        }
        else
        {
            try
            {
                StkRobot_Cheak();//打开机器人
            }
            catch { }
        }
    }

    /// <summary>
    /// 机器人状态
    /// </summary>
    private void StkRobot_Cheak()
    {
        //进行自动下注
        int hour = DateTime.Now.Hour;
        if (hour > 2 && hour < 8)
        {
            Response.Write("" + GameName + "_机器人已休息!close1");
        }
        else
        {
            BCW.Model.Game.Stklist stk = new BCW.BLL.Game.Stklist().GetStklist();
            if (stk.ID == 0)
            {
                Response.Write("" + GameName + "_请等待开通下期!close1");
            }
            else
            {
                if (DateTime.Now < stk.EndTime && DateTime.Now > stk.BeginTime)
                {
                    System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                    stopwatch.Start();

                    stkRobotPlay();
                    Response.Write("" + GameName + "_机器人正在工作中!ok1<br />");
                    int countall = new BCW.BLL.Game.Stkpay().GetRecordCount(" UsID>0 and StkId=" + stk.ID + "");
                    Response.Write("" + stk.ID + "(" + stk.EndTime.ToString("MM-dd") + ")期机器人已下注：" + countall + "注<br />");
                    Response.Write("<b>上次刷新时间：</b>" + DateTime.Now + "<br />");
                    stopwatch.Stop();
                    Response.Write("总耗时:" + stopwatch.Elapsed.TotalSeconds + "秒");
                }
                else
                {
                    Response.Write("当前期数下注已经截止<br />");
                    System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                    stopwatch.Start();

                    stkRobotPlay();
                    Response.Write("" + GameName + "_机器人正在工作中!ok1<br />");
                    int countall = new BCW.BLL.Game.Stkpay().GetRecordCount(" UsID>0 and StkId=" + stk.ID + "");
                    Response.Write("" + stk.ID + "(" + stk.EndTime.ToString("MM-dd") + ")期机器人已下注：" + countall + "注<br />");
                    Response.Write("<b>上次刷新时间：</b>" + DateTime.Now + "<br />");
                    stopwatch.Stop();
                    Response.Write("总耗时:" + stopwatch.Elapsed.TotalSeconds + "秒");
                }
            }
        }
    }

    /// <summary>
    /// 随机得到下注的类型
    /// </summary>
    /// <returns>下注的类型</returns>
    private int GetPtype()
    {
        Random r = new Random();
        Random rd = new Random(r.Next(30, 50));
        int ptype = rd.Next(0, 18);//
        System.Threading.Thread.Sleep(50);//延时
        return ptype;
    }

    /// <summary>
    /// 得到出动的ID
    /// </summary>
    /// <returns></returns>
    private int GetUsID()
    {
        int UsID = 0;
        string PlayUsID = ub.GetSub("StkRoBotID", "/Controls/stkguess.xml");
        if (PlayUsID == "")
            return 0;
        string[] sNum = Regex.Split(PlayUsID, "#");
        Random r = new Random();
        Random rd = new Random(r.Next(50));
        string usid = sNum[rd.Next(sNum.Length)].Replace(" ", "");
        if (usid != "")
        {
            UsID = Convert.ToInt32(usid);
        }
        System.Threading.Thread.Sleep(50);//延时
        return UsID;
    }

    /// <summary>
    ///  更新会员在线时长
    /// </summary>
    /// <param name="UsID"></param>
    private void ChanageOnline(int UsID)
    {
        int OnTime = 5;
        new BCW.BLL.User().UpdateTime(UsID, OnTime);
    }

    #region 获得类型对应名字 OutType
    private string OutType(int Types)
    {
        string Retn = string.Empty;
        if (Types >= 10)
        {
            if (Types == 10)
                Retn = "单";
            else if (Types == 11)
                Retn = "双";
            else if (Types == 12)
                Retn = "大";
            else if (Types == 13)
                Retn = "小";
            else if (Types == 14)
                Retn = "合单";
            else if (Types == 15)
                Retn = "合双";
            else if (Types == 16)
                Retn = "合大";
            else if (Types == 17)
                Retn = "合小";
        }
        else
        {
            Retn = Types.ToString();
        }
        return Retn;
    }
    #endregion

    #region 投注上限 OutOddsc
    /// <summary>
    /// 投注上限
    /// </summary>
    /// <param name="Types"></param>
    /// <returns></returns>
    private long OutOddsc(int Types)
    {
        string toppay = ub.GetSub("StkToppay", xmlPath);
        long pText = 0;
        string[] toppay1 = toppay.Split('|');
        pText = Convert.ToInt64(toppay1[Types]);

        return pText;
    }
    #endregion

    /// <summary>
    /// 机器人购买自动兑奖
    /// </summary>
    private void stkRobotPlay()
    {
        #region 机器人下注
        //得到随机的UsID
        int meid = GetUsID();
        if (meid == 0)
            return;
        if (!new BCW.BLL.User().Exists(meid))
        {
            Response.Write("机器人不存在");
            Response.End();
        }
        ChanageOnline(meid);
        //得到随机的类型
        int pType = GetPtype();
        //得到投注的酷币
        int Price = 0;
        string[] ppp = ub.GetSub("StkRoBotCent", "/Controls/stkguess.xml").Split('#');
        Random ran = new Random();
        int k = ran.Next(0, (ppp.Length));
        Price = Convert.ToInt32(ppp[k]);
        //每个机器人每一期的最高购买次数
        int buyCount = Convert.ToInt32(ub.GetSub("StkRoBotbuyCount", xmlPath));

        double Odds = 0;
        double stkOdds1 = Utils.ParseDouble(ub.GetSub("StkOdds0", xmlPath));//0        
        double stkOdds2 = Utils.ParseDouble(ub.GetSub("StkOdds1", xmlPath));//1
        double stkOdds3 = Utils.ParseDouble(ub.GetSub("StkOdds2", xmlPath));//2
        double stkOdds4 = Utils.ParseDouble(ub.GetSub("StkOdds3", xmlPath));//3
        double stkOdds5 = Utils.ParseDouble(ub.GetSub("StkOdds4", xmlPath));//4
        double stkOdds6 = Utils.ParseDouble(ub.GetSub("StkOdds5", xmlPath));//5
        double stkOdds7 = Utils.ParseDouble(ub.GetSub("StkOdds6", xmlPath));//6
        double stkOdds8 = Utils.ParseDouble(ub.GetSub("StkOdds7", xmlPath));//7
        double stkOdds9 = Utils.ParseDouble(ub.GetSub("StkOdds8", xmlPath));//8
        double stkOdds10 = Utils.ParseDouble(ub.GetSub("StkOdds9", xmlPath));//9
        double stkOdds11 = Utils.ParseDouble(ub.GetSub("StkOdds10", xmlPath));//单
        double stkOdds12 = Utils.ParseDouble(ub.GetSub("StkOdds11", xmlPath));//双
        double stkOdds13 = Utils.ParseDouble(ub.GetSub("StkOdds12", xmlPath));//大        
        double stkOdds14 = Utils.ParseDouble(ub.GetSub("StkOdds13", xmlPath));//小
        double stkOdds15 = Utils.ParseDouble(ub.GetSub("StkOdds14", xmlPath));//合单
        double stkOdds16 = Utils.ParseDouble(ub.GetSub("StkOdds15", xmlPath));//合双
        double stkOdds17 = Utils.ParseDouble(ub.GetSub("StkOdds16", xmlPath));//合大
        double stkOdds18 = Utils.ParseDouble(ub.GetSub("StkOdds17", xmlPath));//合小


        string mename = new BCW.BLL.User().GetUsName(meid);
        BCW.Model.Game.Stklist model = new BCW.BLL.Game.Stklist().GetStklist();

        if (model.EndTime > DateTime.Now && DateTime.Now > model.BeginTime)
        {
            BCW.Model.Game.Stkpay modelPay = new BCW.Model.Game.Stkpay();

            //计数出该机器人投注的次数是否大于xml限定次数
            int count = new BCW.BLL.Game.Stkpay().GetRecordCount(" UsID=" + meid + " and StkId=" + model.ID + "");
            Random r = new Random();
            if (count < buyCount)
            {
                if (pType == 0)//0
                {
                    Odds = stkOdds1;
                }
                else if (pType == 1)//1
                {
                    Odds = stkOdds2;
                }
                else if (pType == 2)//2
                {
                    Odds = stkOdds3;
                }
                else if (pType == 3)//3
                {
                    Odds = stkOdds4;
                }
                else if (pType == 4)//4
                {
                    Odds = stkOdds5;
                }
                else if (pType == 5)//5
                {
                    Odds = stkOdds6;
                }
                else if (pType == 6)//6
                {
                    Odds = stkOdds7;
                }
                else if (pType == 7)//7
                {
                    Odds = stkOdds8;
                }
                else if (pType == 8)//8
                {
                    Odds = stkOdds9;
                }
                else if (pType == 9)//9
                {
                    Odds = stkOdds10;
                }
                else if (pType == 10)//单
                {
                    Odds = stkOdds11;
                }
                else if (pType == 11)//双
                {
                    Odds = stkOdds12;
                }
                else if (pType == 12)//大
                {
                    Odds = stkOdds13;
                }
                else if (pType == 13)//小
                {
                    Odds = stkOdds14;
                }
                else if (pType == 14)//合单
                {
                    Odds = stkOdds15;
                }
                else if (pType == 15)//双
                {
                    Odds = stkOdds16;
                }
                else if (pType == 16)//大
                {
                    Odds = stkOdds17;
                }
                else if (pType == 17)//小
                {
                    Odds = stkOdds18;
                }

                long gold = 0;
                gold = new BCW.BLL.User().GetGold(meid);

                long prices = Convert.ToInt64(Price);

                long SmallPay = Utils.ParseInt64(ub.GetSub("StkSmallPay", xmlPath));
                long BigPay = Utils.ParseInt64(ub.GetSub("StkBigPay", xmlPath));
                if (prices >= SmallPay && prices <= BigPay)
                {

                    long xPrices = Utils.ParseInt64(ub.GetSub("StkMaxpay", xmlPath));
                    if (xPrices > 0 && gold >= 0)
                    {
                        long oPrices = new BCW.BLL.Game.Stkpay().GetSumPrices(meid, model.ID);
                        if (oPrices + prices <= xPrices)
                        {

                            #region 每期每玩法每ID投注上限
                            long ptyPrices = 0;
                            try
                            {
                                ptyPrices = Convert.ToInt64(OutOddscid(pType));
                            }
                            catch { ptyPrices = 0; }
                            if (ptyPrices > 0)
                            {
                                long oPrices2 = new BCW.BLL.Game.Stkpay().GetSumPrices(meid, model.ID, pType);
                                if (oPrices + prices > ptyPrices)
                                {
                                    Response.Write("" + meid + "本期竞猜" + OutType(pType) + "下注已达上限");
                                    Response.End();
                                }
                            }
                            #endregion


                            #region //浮动限制
                            long xPricesc = 0;
                            if (pType < 10)
                            {
                                xPricesc = OutOddsc(pType);
                                if (xPricesc > 0)
                                {
                                    long oPricesc = new BCW.BLL.Game.Stkpay().GetSumPricesbytype(pType, model.ID);
                                    if (oPricesc + prices > xPricesc)
                                    {
                                        Response.Write("投注达到限额ok1");
                                        Response.End();
                                    }
                                }
                            }
                            else if (pType == 10 || pType == 11)//单双
                            {
                                xPricesc = OutOddsc(10);
                                long Cent = new BCW.BLL.Game.Stkpay().GetSumPricesbytype(10, model.ID);
                                long Cent2 = new BCW.BLL.Game.Stkpay().GetSumPricesbytype(11, model.ID);

                                if (pType == 10)
                                {
                                    if (Math.Abs(Cent + prices - Cent2) > xPricesc)
                                    {
                                        Response.Write("投注达到限额ok1");
                                        Response.End();
                                    }
                                }
                                if (pType == 11)
                                {
                                    if (Math.Abs(Cent2 + prices - Cent) > xPricesc)
                                    {
                                        Response.Write("投注达到限额ok1");
                                        Response.End();
                                    }
                                }
                            }
                            else if (pType == 12 || pType == 13)//大小
                            {
                                xPricesc = OutOddsc(12);
                                long Cent = new BCW.BLL.Game.Stkpay().GetSumPricesbytype(12, model.ID);
                                long Cent2 = new BCW.BLL.Game.Stkpay().GetSumPricesbytype(13, model.ID);

                                if (pType == 12)
                                {
                                    if (Math.Abs(Cent + prices - Cent2) > xPricesc)
                                    {
                                        Response.Write("投注达到限额ok1");
                                        Response.End();
                                    }
                                }
                                if (pType == 13)
                                {
                                    if (Math.Abs(Cent2 + prices - Cent) > xPricesc)
                                    {
                                        Response.Write("投注达到限额ok1");
                                        Response.End();
                                    }
                                }
                            }
                            else if (pType == 14 || pType == 15)//合单双
                            {
                                xPricesc = OutOddsc(14);
                                long Cent = new BCW.BLL.Game.Stkpay().GetSumPricesbytype(14, model.ID);
                                long Cent2 = new BCW.BLL.Game.Stkpay().GetSumPricesbytype(15, model.ID);

                                if (pType == 14)
                                {
                                    if (Math.Abs(Cent + prices - Cent2) > xPricesc)
                                    {
                                        Response.Write("投注达到限额ok1");
                                        Response.End();
                                    }
                                }
                                if (pType == 15)
                                {
                                    if (Math.Abs(Cent2 + prices - Cent) > xPricesc)
                                    {
                                        Response.Write("投注达到限额ok1");
                                        Response.End();
                                    }
                                }
                            }
                            else if (pType == 16 || pType == 17)//合大小
                            {
                                xPricesc = OutOddsc(16);
                                long Cent = new BCW.BLL.Game.Stkpay().GetSumPricesbytype(16, model.ID);
                                long Cent2 = new BCW.BLL.Game.Stkpay().GetSumPricesbytype(17, model.ID);

                                if (pType == 16)
                                {
                                    if (Math.Abs(Cent + prices - Cent2) > xPricesc)
                                    {
                                        Response.Write("投注达到限额ok1");
                                        Response.End();
                                    }
                                }
                                if (pType == 17)
                                {
                                    if (Math.Abs(Cent2 + prices - Cent) > xPricesc)
                                    {
                                        Response.Write("投注达到限额ok1");
                                        Response.End();
                                    }
                                }
                            }
                            #endregion

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
                                modelx.AfterGold = (gold + prices);//更新后的币数

                                modelx.AcText = "系统机器人自动操作";
                                modelx.AddTime = DateTime.Now;
                                new BCW.BLL.Goldlog().Add(modelx);

                                BCW.Data.SqlHelper.ExecuteSql("Update tb_User set iGold=iGold+(" + prices + ") where ID=" + meid + "");

                                {
                                    modelPay.StkId = model.ID;
                                    modelPay.UsID = meid;
                                    modelPay.UsName = mename;
                                    modelPay.Types = pType;
                                    modelPay.WinNum = 0;
                                    modelPay.Odds = Convert.ToDecimal(Odds);
                                    modelPay.BuyCent = prices;
                                    modelPay.WinCent = 0;
                                    modelPay.AddTime = DateTime.Now;
                                    modelPay.State = 0;
                                    modelPay.bzType = 0;//酷币
                                    modelPay.isSpier = 1;//机器人
                                    int id = new BCW.BLL.Game.Stkpay().Add(modelPay);

                                    //动态投注后记录投注信息

                                    string wText = string.Empty;
                                    new BCW.BLL.User().UpdateiGold(meid, mename, -prices, "在-[url=/bbs/game/stkguess.aspx]" + GameName + "[/url]" + model.ID + "(" + model.EndTime.ToString("MM-dd") + ")-|押" + OutType(pType) + "|赔率:" + Odds + "|标识ID:" + id + "");
                                    wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/stkguess.aspx]" + GameName + "[/url]下注**" + "" + ub.Get("SiteBz") + "";//+ prices " + OutType(pType) + "
                                    new BCW.BLL.Action().Add(11, id, meid, "", wText);
                                }
                            }
                            else
                            {
                                modelPay.StkId = model.ID;
                                modelPay.UsID = meid;
                                modelPay.UsName = mename;
                                modelPay.Types = pType;
                                modelPay.WinNum = 0;
                                modelPay.Odds = Convert.ToDecimal(Odds);
                                modelPay.BuyCent = prices;
                                modelPay.WinCent = 0;
                                modelPay.AddTime = DateTime.Now;
                                modelPay.State = 0;
                                modelPay.bzType = 0;//酷币
                                modelPay.isSpier = 1;//机器人
                                int id = new BCW.BLL.Game.Stkpay().Add(modelPay);

                                //动态投注后记录投注信息

                                string wText = string.Empty;
                                new BCW.BLL.User().UpdateiGold(meid, mename, -prices, "在-[url=/bbs/game/stkguess.aspx]" + GameName + "[/url]" + model.ID + "(" + model.EndTime.ToString("MM-dd") + ")-|押" + OutType(pType) + "|赔率:" + Odds + "|标识ID:" + id + "");
                                wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/stkguess.aspx]" + GameName + "[/url]下注**" + "" + ub.Get("SiteBz") + "";//+ prices 竞猜" + OutType(pType) + "
                                new BCW.BLL.Action().Add(11, id, meid, "", wText);

                            }

                        }
                    }
                }
            }
        }
        #endregion

        #region 机器人自动兑奖
        string RoBot = ub.GetSub("StkRoBotID", xmlPath);
        string[] RoBotID = RoBot.Split('#');
        for (int i = 0; i < RoBotID.Length; i++)
        {
            if (RoBotID[i] != "")
            {
                meid = Convert.ToInt32(RoBotID[i].Replace(" ", ""));
                string mename1 = new BCW.BLL.User().GetUsName(meid);
                DataSet model2 = new BCW.BLL.Game.Stkpay().GetList("ID", "UsID=" + meid + " and isSpier=1 and WinCent > 0 and State!=2 ");
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
                        if (new BCW.BLL.Game.Stkpay().ExistsReBot(pid, meid))
                        {
                            if (new BCW.BLL.Game.Stkpay().getState(pid) != 2)
                            {
                                //操作币
                                BCW.Model.Game.Stkpay model1 = new BCW.BLL.Game.Stkpay().GetStkpay(pid);
                                int number = model1.StkId;
                                BCW.Model.Game.Stklist idd = new BCW.BLL.Game.Stklist().GetStklist(number);
                                long winMoney = Convert.ToInt64(model1.WinCent);
                                int bzType = new BCW.BLL.Game.Stkpay().GetbzType(pid);
                                //税率
                                long SysTax = 0;
                                int Tax = Utils.ParseInt(ub.GetSub("StkTax", xmlPath));
                                if (Tax > 0)
                                {
                                    SysTax = Convert.ToInt64(winMoney * Tax * 0.01);
                                }
                                winMoney = winMoney - SysTax;
                                new BCW.BLL.User().UpdateiGold(meid, mename1, winMoney, "" + GameName + "兑奖-" + "[url=./game/stkguess.aspx?act=view&amp;id=" + idd.ID + "&amp;ptype=2]" + number + "(" + idd.EndTime.ToString("MM-dd") + ")" + "[/url]" + "-标识ID：" + pid + "");
                                new BCW.BLL.Game.Stkpay().UpdateState(pid);
                                Response.Write("" + GameName + "机器人" + meid + "自动兑奖!ok1<br />");
                            }
                        }
                    }
                }
            }
        }
        #endregion

    }

    #region 投注上限 OutOddscid
    /// <summary>
    /// 投注上限
    /// </summary>
    /// <param name="Types"></param>
    /// <returns></returns>
    private long OutOddscid(int Types)
    {
        string toppay = ub.GetSub("StkPayID", xmlPath);
        long pText = 0;
        string[] toppay1 = toppay.Split('|');
        pText = Convert.ToInt64(toppay1[Types]);

        return pText;
    }
    #endregion
}
