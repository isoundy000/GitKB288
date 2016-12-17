using System;
using BCW.Common;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Data;

/// <summary>
/// 蒙宗将 20160908 增加龙虎下注
/// 蒙宗将 20160929 消费兑奖增加期号链接
/// 蒙宗将 20160930 赔率调整
/// 蒙宗将 20161019 机器人下注修复
/// 蒙宗将 20161029 修复机器号问题
/// 蒙宗将 20161103 动态修复
/// </summary>

public partial class Robot_klsfRobot : System.Web.UI.Page
{
    protected string xmlPath = "/Controls/klsf.xml";
    protected string GameName = ub.GetSub("klsfName", "/Controls/klsf.xml");//游戏名字
    protected int gid = 32;

    protected void Page_Load(object sender, EventArgs e)
    {
        //防止缓存
        Response.Buffer = true;
        Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
        Response.Expires = 0;
        Response.CacheControl = "no-cache";

        if (ub.GetSub("klsfIsBot", xmlPath) != "1")
        {
            Response.Write("<b>" + GameName + "_机器人处于关闭状态close1</b>");
        }
        else
        {
            try
            {
                klsfRobot_Cheak();//打开机器人
            }
            catch { }
        }
    }

    /// <summary>
    /// 机器人状态
    /// </summary>
    private void klsfRobot_Cheak()
    {
        //进行自动下注
        int hour = DateTime.Now.Hour;
        if (hour > 2 && hour < 8)
        {
            Response.Write("" + GameName + "_机器人已休息!close1");
        }
        else
        {
            BCW.Model.klsflist model = new BCW.BLL.klsflist().GetklsflistLast();
            if (model.EndTime > DateTime.Now)
            {
                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();

                klsfRobotPlay();
                Response.Write("" + GameName + "_机器人正在工作中!ok1<br />");

                Response.Write("<b>上次刷新时间：</b>" + DateTime.Now + "<br />");
                stopwatch.Stop();
                Response.Write("总耗时:" + stopwatch.Elapsed.TotalSeconds + "秒");
            }
            else
            {
                Response.Write("" + GameName + "_第《" + model.klsfId + "》期未开奖，机器人购买失败!error1");
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
        int ptype = rd.Next(3, 16);//1五胆拖、2五任选、3四胆拖、4四任选、5三胆拖、6三任选、7二胆拖、8二任选、9连二组选、10连二直选、11前一红、12前一数、13大小、14单双、15龙虎
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
        string PlayUsID = ub.GetSub("klsfRobotId", "/Controls/klsf.xml");
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

    /// <summary>
    /// 显示标题的投注方式
    /// </summary>
    /// <param name="Types">下注类型</param>
    /// <returns></returns>
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
    /// 机器人购买自动兑奖
    /// </summary>
    private void klsfRobotPlay()
    {
        #region 机器人下注
        //得到随机的UsID
        int meid = GetUsID();
        if (meid == 0)
            return;
        ChanageOnline(meid);
        //得到随机的类型
        int pType = GetPtype();
        //得到投注的酷币
        int Price = 0;
        string[] ppp = ub.GetSub("klsfRobotBuyCent", "/Controls/klsf.xml").Split('#');
        Random ran = new Random();
        int k = ran.Next(0, (ppp.Length));
        Price = Convert.ToInt32(ppp[k]);

        //每个机器人每一期的最高购买次数
        int buyCount = Convert.ToInt32(ub.GetSub("klsfRobotBuy", xmlPath));

        double Odds = 0;
        double klsfOdds1 = Utils.ParseDouble(ub.GetSub("klsfOdds1", xmlPath));//任选五胆拖        
        double klsfOdds2 = Utils.ParseDouble(ub.GetSub("klsfOdds2", xmlPath));//任选五普通
        double klsfOdds3 = Utils.ParseDouble(ub.GetSub("klsfOdds3", xmlPath));//任选四胆拖
        double klsfOdds4 = Utils.ParseDouble(ub.GetSub("klsfOdds4", xmlPath));//任选四普通
        double klsfOdds5 = Utils.ParseDouble(ub.GetSub("klsfOdds5", xmlPath));//任选三胆拖
        double klsfOdds6 = Utils.ParseDouble(ub.GetSub("klsfOdds6", xmlPath));//任选三普通
        double klsfOdds7 = Utils.ParseDouble(ub.GetSub("klsfOdds7", xmlPath));//任选二胆拖
        double klsfOdds8 = Utils.ParseDouble(ub.GetSub("klsfOdds8", xmlPath));//任选二普通
        double klsfOdds9 = Utils.ParseDouble(ub.GetSub("klsfOdds9", xmlPath));//连二直选
        double klsfOdds10 = Utils.ParseDouble(ub.GetSub("klsfOdds10", xmlPath));//连二组选
        double klsfOdds11 = Utils.ParseDouble(ub.GetSub("klsfOdds11", xmlPath));//前一红投
        double klsfOdds12 = Utils.ParseDouble(ub.GetSub("klsfOdds12", xmlPath));//前一数投
        double klsfOdds13 = Utils.ParseDouble(ub.GetSub("klsfOdds13", xmlPath));//大        
        double klsfOdds14 = Utils.ParseDouble(ub.GetSub("klsfOdds14", xmlPath));//小
        double klsfOdds15 = Utils.ParseDouble(ub.GetSub("klsfOdds15", xmlPath));//单
        double klsfOdds16 = Utils.ParseDouble(ub.GetSub("klsfOdds16", xmlPath));//双
        double klsfOdds17 = Utils.ParseDouble(ub.GetSub("klsfOdds17", xmlPath));//浮动
        double klsfOdds18 = Utils.ParseDouble(ub.GetSub("klsfOdds18", xmlPath));//龙18
        double klsfOdds19 = Utils.ParseDouble(ub.GetSub("klsfOdds19", xmlPath));//虎81
        double klsfOdds20 = Utils.ParseDouble(ub.GetSub("klsfOdds20", xmlPath));//龙27
        double klsfOdds21 = Utils.ParseDouble(ub.GetSub("klsfOdds21", xmlPath));//虎72
        double klsfOdds22 = Utils.ParseDouble(ub.GetSub("klsfOdds22", xmlPath));//龙36
        double klsfOdds23 = Utils.ParseDouble(ub.GetSub("klsfOdds23", xmlPath));//虎63
        double klsfOdds24 = Utils.ParseDouble(ub.GetSub("klsfOdds24", xmlPath));//龙45
        double klsfOdds25 = Utils.ParseDouble(ub.GetSub("klsfOdds25", xmlPath));//虎54

        int stage = 0;
        string mename = new BCW.BLL.User().GetUsName(meid);
        string date = DateTime.Now.ToString("yyMMdd");
        stage = int.Parse(date + "01");
        BCW.Model.klsflist model = new BCW.BLL.klsflist().GetklsflistLast();

        if (model.EndTime > DateTime.Now)
        {
            BCW.Model.klsfpay modelPay = new BCW.Model.klsfpay();

            //计数出该机器人投注的次数是否大于xml限定次数
            int count = new BCW.BLL.klsfpay().GetRecordCount(" UsID=" + meid + " and klsfId=" + model.klsfId + "");
            Random r = new Random();

            if (count < buyCount)
            {
                string Notes = string.Empty;
                if (pType == 1)//五胆拖
                {
                    int n = r.Next(6, 10);
                    int m = r.Next(1, 3);
                    Notes = GetDTNotes(n, m);
                    modelPay.Notes = Notes;
                    modelPay.iCount = GetZS(Notes, pType);
                    Odds = klsfOdds1;
                }
                else if (pType == 2)//五任选
                {
                    int n = r.Next(6, 10);
                    Notes = GetRPNotes(n);
                    modelPay.Notes = Notes;
                    modelPay.iCount = GetZS(Notes, pType);
                    Odds = klsfOdds2;
                }
                else if (pType == 3)//四胆拖
                {
                    int n = r.Next(5, 9);
                    int m = r.Next(1, 3);
                    Notes = GetDTNotes(n, m);
                    modelPay.Notes = Notes;
                    modelPay.iCount = GetZS(Notes, pType);
                    Odds = klsfOdds3;
                }
                else if (pType == 4)//四任选
                {
                    int n = r.Next(5, 9);
                    Notes = GetRPNotes(n);
                    modelPay.Notes = Notes;
                    modelPay.iCount = GetZS(Notes, pType);
                    Odds = klsfOdds4;
                }
                else if (pType == 5)//三胆拖
                {
                    int n = r.Next(4, 8);
                    int m = r.Next(1, 2);
                    Notes = GetDTNotes(n, m);
                    modelPay.Notes = Notes;
                    modelPay.iCount = GetZS(Notes, pType);
                    Odds = klsfOdds5;
                }
                else if (pType == 6)//三任选
                {
                    int n = r.Next(4, 8);
                    Notes = GetRPNotes(n);
                    modelPay.Notes = Notes;
                    modelPay.iCount = GetZS(Notes, pType);
                    Odds = klsfOdds6;
                }
                else if (pType == 7)//两胆拖
                {
                    int n = r.Next(3, 7);
                    Notes = GetDTNotes(n, 1);
                    modelPay.Notes = Notes;
                    modelPay.iCount = GetZS(Notes, pType);
                    Odds = klsfOdds7;
                }
                else if (pType == 8 || pType == 10)//二任选、连二组选
                {
                    int n = r.Next(3, 7);
                    Notes = GetRPNotes(n);
                    modelPay.Notes = Notes;
                    modelPay.iCount = GetZS(Notes, pType);
                    if (pType == 8)
                        Odds = klsfOdds8;
                    if (pType == 10)
                        Odds = klsfOdds10;
                }
                else if (pType == 9)//连二直选
                {
                    int n = r.Next(4, 8);
                    int m = r.Next(1, n - 1);
                    Notes = GetDTNotes(n, m);
                    modelPay.Notes = Notes;
                    modelPay.iCount = GetZS(Notes, pType);
                    Odds = klsfOdds9;
                }
                else if (pType == 11)//前一红投
                {
                    int n = r.Next(1, 2);
                    Notes = GetFSNotes(n, 19, 21);
                    modelPay.Notes = Notes;
                    modelPay.iCount = GetZS(Notes, pType);
                    Odds = klsfOdds11;
                }
                else if (pType == 12)//前一数投
                {
                    int n = r.Next(1, 5);
                    Notes = GetFSNotes(n, 1, 19);
                    modelPay.Notes = Notes;
                    modelPay.iCount = GetZS(Notes, pType);
                    Odds = klsfOdds12;
                }
                else if (pType == 13)//买大小
                {
                    int n = r.Next(1, 3);
                    if (n == 1)
                    {
                        Notes = "大";
                        Odds = klsfOdds13;
                    }
                    else
                    {
                        Notes = "小";
                        Odds = klsfOdds14;
                    }
                    modelPay.Notes = Notes;
                    modelPay.iCount = 1;
                }
                else if (pType == 14)//押单双
                {
                    int n = r.Next(1, 3);
                    if (n == 1)
                    {
                        Notes = "单";
                        Odds = klsfOdds15;
                    }
                    else
                    {
                        Notes = "双";
                        Odds = klsfOdds16;
                    }
                    modelPay.Notes = Notes;
                    modelPay.iCount = 1;
                }
                else if (pType == 15)//押龙虎
                {
                    int n = r.Next(1, 9);
                    if (n == 1)
                    {
                        Notes = "龙（1与8位）";
                        Odds = klsfOdds18;
                    }
                    else if (n == 2)
                    {
                        Notes = "虎（1与8位）";
                        Odds = klsfOdds19;
                    }
                    else if (n == 3)
                    {
                        Notes = "龙（2与7位）";
                        Odds = klsfOdds20;
                    }
                    else if (n == 4)
                    {
                        Notes = "虎（2与7位）";
                        Odds = klsfOdds21;
                    }
                    else if (n == 5)
                    {
                        Notes = "龙（3与6位）";
                        Odds = klsfOdds22;
                    }
                    else if (n == 6)
                    {
                        Notes = "虎（3与6位）";
                        Odds = klsfOdds23;
                    }
                    else if (n == 7)
                    {
                        Notes = "龙（4与5位）";
                        Odds = klsfOdds24;
                    }
                    else if (n == 8)
                    {
                        Notes = "虎（4与5位）";
                        Odds = klsfOdds25;
                    }
                    modelPay.Notes = Notes;
                    modelPay.iCount = 1;
                }

                int IsSWB = Utils.ParseInt(ub.GetSub("klsfSWB", xmlPath));
                long gold = 0;

                gold = new BCW.BLL.User().GetGold(meid);

                long prices = Convert.ToInt64(Price * modelPay.iCount);

                long SmallPay = Utils.ParseInt64(ub.GetSub("klsfSmallPay", xmlPath));
                long BigPay = Utils.ParseInt64(ub.GetSub("klsfBigPay", xmlPath));

                if (prices >= SmallPay && prices <= BigPay)
                {

                    long xPrices = Utils.ParseInt64(ub.GetSub("klsfPrice", xmlPath));
                    if (xPrices > 0 && gold >= 0)
                    {
                        long oPrices = new BCW.BLL.klsfpay().GetSumPrices(meid, model.klsfId);
                        if (oPrices + prices <= xPrices)
                        {

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
                                    modelPay.klsfId = model.klsfId;//购买期数
                                    modelPay.Types = pType;//投注类型
                                    modelPay.UsID = meid;//用户id
                                    modelPay.UsName = mename;//用户昵称
                                    modelPay.AddTime = DateTime.Now;//投注时间
                                    modelPay.Result = "";//开奖结果
                                    modelPay.State = 0;//未开奖
                                    modelPay.WinCent = 0;//获得多少酷币
                                    modelPay.iWin = 0;//中了多少注
                                    modelPay.Price = Price;//每注投多少钱
                                    modelPay.Prices = prices;//总投了多少钱
                                    modelPay.isRoBot = 1;
                                    modelPay.Odds = Convert.ToDecimal(Odds);//赔率
                                    int id = new BCW.BLL.klsfpay().Add(modelPay);

                                    //动态投注后记录投注信息
                                    #region 根据版本选择快乐币和酷币的获取和显示
                                    string wText = string.Empty;
                                    if (IsSWB == 0)
                                    {
                                        new BCW.SWB.BLL().UpdateMoney(meid, -prices, gid);
                                        wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/klsf.aspx]" + GameName + "[/url]《" + OutType(pType) + "》下注" + prices + "快乐币";
                                    }
                                    else
                                    {
                                        new BCW.BLL.User().UpdateiGold(meid, mename, -prices, "" + GameName + "第[url=./game/klsf.aspx?act=view&amp;id=" + model.ID + "]" + model.klsfId + "[/url]期" + OutType(pType) + ":" + Notes + "|赔率" + modelPay.Odds + "|标识Id" + id + ""); //酷币
                                        //new BCW.BLL.User().UpdateiGold(105, new BCW.BLL.User().GetUsName(105), prices, "ID:[url=forumlog.aspx?act=xview&amp;uid=" + meid + "]" + meid + "[/url]" + GameName + "第[url=./game/klsf.aspx?act=view&amp;id=" +model.ID + "]" + model.klsfId + "[/url]期买" + OutType(pType) + ":" + Notes + "共" + prices+ub.Get("SiteBz") + "-标识Id" +id+ "");
                                        wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/klsf.aspx]" + GameName + "[/url]《" + OutType(pType) + "》下注**" + "" + ub.Get("SiteBz") + "";//+ prices 
                                    }
                                    #endregion

                                    new BCW.BLL.Action().Add(1006, id, meid, mename, wText);
                                }
                            }
                            else
                            {
                                // if (gold >= prices)
                                {
                                    modelPay.klsfId = model.klsfId;//购买期数
                                    modelPay.Types = pType;//投注类型
                                    modelPay.UsID = meid;//用户id
                                    modelPay.UsName = mename;//用户昵称
                                    modelPay.AddTime = DateTime.Now;//投注时间
                                    modelPay.Result = "";//开奖结果
                                    modelPay.State = 0;//未开奖
                                    modelPay.WinCent = 0;//获得多少酷币
                                    modelPay.iWin = 0;//中了多少注
                                    modelPay.Price = Price;//每注投多少钱
                                    modelPay.Prices = prices;//总投了多少钱
                                    modelPay.isRoBot = 1;
                                    modelPay.Odds = Convert.ToDecimal(Odds);//赔率
                                    int id = new BCW.BLL.klsfpay().Add(modelPay);

                                    //动态投注后记录投注信息
                                    #region 根据版本选择快乐币和酷币的获取和显示
                                    string wText = string.Empty;
                                    if (IsSWB == 0)
                                    {
                                        new BCW.SWB.BLL().UpdateMoney(meid, -prices, gid);
                                        wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/klsf.aspx]" + GameName + "[/url]《" + OutType(pType) + "》下注" + prices + "快乐币";
                                    }
                                    else
                                    {
                                        new BCW.BLL.User().UpdateiGold(meid, mename, -prices, "" + GameName + "第[url=./game/klsf.aspx?act=view&amp;id=" + model.ID + "]" + model.klsfId + "[/url]期" + OutType(pType) + ":" + Notes + "|赔率" + modelPay.Odds + "|标识Id" + id + ""); //酷币
                                        //new BCW.BLL.User().UpdateiGold(105, new BCW.BLL.User().GetUsName(105), prices, "ID:[url=forumlog.aspx?act=xview&amp;uid=" + meid + "]" + meid + "[/url]" + GameName + "第[url=./game/klsf.aspx?act=view&amp;id=" +model.ID + "]" + model.klsfId + "[/url]期买" + OutType(pType) + ":" + Notes + "共" + prices+ub.Get("SiteBz") + "-标识Id" +id+ "");
                                        wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/klsf.aspx]" + GameName + "[/url]《" + OutType(pType) + "》下注**" + "" + ub.Get("SiteBz") + "";//+ prices
                                    }
                                    #endregion

                                    new BCW.BLL.Action().Add(1006, id, meid, mename, wText);
                                }

                            }

                        }
                    }
                }
            }
        }
        #endregion

        #region 机器人自动兑奖
        string RoBot = ub.GetSub("klsfRobotId", xmlPath);
        //  string RoBotCost = ub.GetSub("ReBotCost", xmlPath);
        string[] RoBotID = RoBot.Split('#');
        for (int i = 0; i < RoBotID.Length; i++)
        {
            if (RoBotID[i] != "")
            {
                meid = Convert.ToInt32(RoBotID[i].Replace(" ", ""));
                string mename1 = new BCW.BLL.User().GetUsName(meid);
                DataSet model2 = new BCW.BLL.klsfpay().GetList("id", "usID=" + meid + " and isRoBot=1 and iWin > 0 and State!=2 ");
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
                        if (new BCW.BLL.klsfpay().ExistsReBot(pid, meid))
                        {
                            if (new BCW.DAL.klsfpay().getState(pid) != 2)
                            {
                                //操作币
                                BCW.Model.klsfpay model1 = new BCW.BLL.klsfpay().Getklsfpay(pid);
                                int number = new BCW.BLL.klsfpay().GetklsfId(pid);
                                BCW.Model.klsflist idd = new BCW.BLL.klsflist().Getklsflistbyklsfid(number);
                                long winMoney = Convert.ToInt64(model1.WinCent);
                                new BCW.BLL.User().UpdateiGold(meid, mename1, winMoney, "" + GameName + "兑奖-" + "[url=./game/klsf.aspx?act=view&amp;id=" + idd.ID + "&amp;ptype=2]" + number + "[/url]" + "-标识ID：" + pid + "");
                                //new BCW.BLL.User().UpdateiGold(105, new BCW.BLL.User().GetUsName(105), -winMoney, "ID:[url=forumlog.aspx?act=xview&amp;uid=" + meid + "]" + meid + "[/url]" + GameName + "第[url=./game/klsf.aspx?act=view&amp;id=" + model.ID + "]" + number + "[/url]期兑奖" + winMoney + "(标识ID" + pid + ")");
                                new BCW.BLL.klsfpay().UpdateState(pid, 2);
                                Response.Write("" + GameName + "机器人" + meid + "自动兑奖!ok1<br />");
                            }
                        }
                    }
                }
            }
        }
        #endregion

    }

    /// <summary>
    /// 随机生成一定数量的一定范围的不重复的随机数
    /// </summary>
    /// <param name="n">生成的数量</param>
    /// <param name="min">随机数的最小值</param>
    /// <param name="max">随机数的最大值</param>
    /// <returns>随机数</returns>
    private int[] GetRandomNum(int n, int min, int max)
    {
        List<int> list = new List<int>();
        Random r = new Random();
        int[] result = new int[n];
        int num = 0;
        for (int i = 0; i < n; i++)
        {
            num = r.Next(min, max + 1);
            while (list.Contains(num))
            {
                num = r.Next(min, max + 1);
            }
            list.Add(num);
        }
        list.CopyTo(result);
        return result;
    }

    /// <summary>
    /// 把随机数转成胆拖的Notes
    /// </summary>
    /// <param name="n">生成的随机数的数量</param>
    /// <param name="m">胆码的数量</param>
    /// <returns>Notes</returns>
    private string GetDTNotes(int n, int m)
    {
        int[] num = GetRandomNum(n, 1, 20);
        int temp = 0;
        for (int i = 0; i < m; i++)
            for (int j = 0; j < m - i; j++)
                if (num[j] > num[j + 1])
                {
                    temp = num[j];
                    num[j] = num[j + 1];
                    num[j + 1] = temp;
                }
        for (int i = m; i < n - 1; i++)
            for (int j = m; j < n - 1 - i; j++)
                if (num[j] > num[j + 1])
                {
                    temp = num[j];
                    num[j] = num[j + 1];
                    num[j + 1] = temp;
                }
        string[] result = new string[n];
        string dm = "";
        for (int i = 0; i < m; i++)
        {
            result[i] = string.Format("{0:D2}", num[i]);
            if (i == m - 1)
                dm += result[i];
            else
                dm += result[i] + ",";
        }
        string tm = "";
        for (int i = m; i < n; i++)
        {
            result[i] = string.Format("{0:D2}", num[i]);
            if (i == n - 1)
                tm += result[i];
            else
                tm += result[i] + ",";
        }
        string Notes = dm + "|" + tm;
        return Notes;
    }

    /// <summary>
    /// 把随机数生成任选类型的Notes
    /// </summary>
    /// <param name="n">生成随机数的个数</param>
    /// <returns>Notes</returns>
    private string GetRPNotes(int n)
    {
        int[] num = GetRandomNum(n, 1, 20);
        int temp = 0;
        for (int i = 0; i < n - 1; i++)
            for (int j = 0; j < n - 1 - i; j++)
                if (num[j] > num[j + 1])
                {
                    temp = num[j];
                    num[j] = num[j + 1];
                    num[j + 1] = temp;
                }
        string[] result = new string[n];
        for (int i = 0; i < n; i++)
        {
            result[i] = string.Format("{0:D2}", num[i]);
        }
        string Notes = string.Join(",", result);
        return Notes;
    }

    /// <summary>
    /// 获取一定数量的随机数，生成Notes
    /// </summary>
    /// <param name="n">随机数的数量</param>
    /// <param name="min">随机数的最小值</param>
    /// <param name="max">随机数的最大值</param>
    /// <returns>Notes</returns>
    private string GetFSNotes(int n, int min, int max)
    {
        int[] num = GetRandomNum(n, min, max);
        int temp = 0;
        for (int i = 0; i < n - 1; i++)
            for (int j = 0; j < n - 1 - i; j++)
                if (num[j] > num[j + 1])
                {
                    temp = num[j];
                    num[j] = num[j + 1];
                    num[j + 1] = temp;
                }
        string[] result = new string[n];
        for (int i = 0; i < n; i++)
        {
            result[i] = string.Format("{0:D2}", num[i]);
        }
        string Notes = string.Join(",", result);
        return Notes;
    }

    /// <summary>
    /// 计算注数
    /// </summary>
    /// <param name="Notes">投注记录</param>
    /// <param name="ptype">投注类型</param>
    /// <returns>注数</returns>
    private int GetZS(string Notes, int ptype)
    {
        string accNum = string.Empty;
        string[] strTemp = { };
        int iZhu = 0;

        if (ptype == 12) //前一数投
        {
            accNum = Notes;
            strTemp = accNum.Split(',');

            iZhu = strTemp.Length;  //注数等于选择数字的个数
        }
        else if (ptype == 11) //前一红投
        {
            accNum = Notes;
            strTemp = accNum.Split(',');

            iZhu = strTemp.Length; //注数等于选择数字的个数
        }
        else if (ptype == 10) //连二组选
        {
            accNum = Notes;
            strTemp = accNum.Split(',');

            iZhu = C(strTemp.Length, 2); //注数等于strTemp和2的组合数
        }
        else if (ptype == 9) //连二直选
        {
            int j = 0;
            string[] strt = Notes.Split('|');
            string[] strt1 = strt[0].Split(',');
            string[] strt2 = strt[1].Split(','); ;

            for (int i = 0; i < strt1.Length; i++)
            {
                for (int p = 0; p < strt2.Length; p++)
                {
                    if (strt1[i] != strt2[p])
                    {
                        j++;
                    }
                }
            }

            iZhu = j;
        }
        else if (ptype == 2 || ptype == 4 || ptype == 6 || ptype == 8) //任选
        {
            accNum = Notes;
            strTemp = accNum.Split(',');

            if (ptype == 8)
            {
                iZhu = C(strTemp.Length, 2);
            }
            else if (ptype == 6)
            {
                iZhu = C(strTemp.Length, 3);
            }
            else if (ptype == 4)
            {
                iZhu = C(strTemp.Length, 4);
            }
            else if (ptype == 2)
            {
                iZhu = C(strTemp.Length, 5);
            }
        }
        else if (ptype == 1 || ptype == 3 || ptype == 5 || ptype == 7) //胆拖
        {
            string[] strt = Notes.Split('|');
            string[] strt1 = strt[0].Split(',');
            string[] strt2 = strt[1].Split(',');

            if (ptype == 7)
            {
                iZhu = strt2.Length;
            }
            else if (ptype == 5)
            {
                iZhu = C(strt2.Length, 3 - strt1.Length);
            }
            else if (ptype == 3)
            {
                iZhu = C(strt2.Length, 4 - strt1.Length);
            }
            else if (ptype == 1)
            {
                iZhu = C(strt2.Length, 5 - strt1.Length);
            }
        }
        return iZhu;
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
