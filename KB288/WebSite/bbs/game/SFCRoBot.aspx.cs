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
/// 20161007 蒙宗将 优化获取上期
/// 蒙宗将 20161112 优化开奖
/// </summary>

public partial class bbs_game_SFCRoBot : System.Web.UI.Page
{
    protected string xmlPath = "/Controls/SFC.xml";
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    ub xml = new ub();
    //首页
    protected void Page_Load(object sender, EventArgs e)
    {
        //防止缓存
        Response.Buffer = true;
        Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
        Response.Expires = 0;
        Response.CacheControl = "no-cache";
        string IsBot = ub.GetSub("IsBot", xmlPath);
        if (IsBot != "")
        {
            if (IsBot == "0")
            {
                Response.Write("<span style='color:red;font-size:50px'>机器人未开通!close1</span>");
            }
            else if (IsBot == "1")
            {
                try
                {
                    builder.Append("ok1");
                    Robot();

                }
                catch
                {
                    builder.Append("error1");
                    builder.Append("器人出错了!");
                }

                RobotCase();
            }
            else
            {
                builder.Append("close1");
                Response.Write("<span style='color:red;font-size:50px'>机器人未开通!</span>");
            }
        }
    }

    //机器人ROBOT
    public void Robot()
    {
        string xml = ub.GetSub("IsBot", xmlPath);
        int buycount = int.Parse(ub.GetSub("RoBotCount", xmlPath));//次数
        string buycost = ub.GetSub("RoBotCost", xmlPath);//倍数

        if (xml == "1")
        {
            string RoBot = ub.GetSub("RoBotID", xmlPath);
            string[] RoBotID = RoBot.Split('#');
            int n = RoBot.Split('#').Length;

            Random rr = new Random();
            int aa = 0;
            aa = rr.Next(0, n);
            string ss = RoBotID[aa];
            int usID = Convert.ToInt32(ss);
            ChanageOnline(Convert.ToInt32(RoBotID[aa]));

            string mename = new BCW.BLL.User().GetUsName(usID);//用户姓名

            //未开奖当前投注期号
            DataSet ds1 = new BCW.SFC.BLL.SfList().GetList("TOP 1 CID", "State=0 order by CID ASC");
            int CID = 0;
            DateTime AddTime = DateTime.Now;

            try
            {
                CID = int.Parse(ds1.Tables[0].Rows[0][0].ToString());
            }
            catch
            {

            }

            BCW.SFC.Model.SfList mod = new BCW.SFC.BLL.SfList().GetSfList1(CID);
            if (mod.Sale_StartTime < DateTime.Now && mod.EndTime > DateTime.Now)
            {
                //计数出该机器人投注的次数是否大于xml限定次数
                int countrebot = new BCW.SFC.BLL.SfPay().GetSFCRobotCount("usID=" + usID + " and IsSpier=1 and CID=" + CID + "");
                if (countrebot <= buycount)
                {
                    long bei = 0;
                    string[] ppp = buycost.Split('#');
                    Random ranm = new Random();
                    int k = ranm.Next(0, (ppp.Length));
                    bei = Convert.ToInt32(ppp[k]);

                    long Price1 = Utils.ParseInt64(ub.GetSub("SFmallPay", xmlPath)) * bei;
                    string SFprice = ub.GetSub("SFprice", xmlPath);

                    //是否刷屏
                    long small = Convert.ToInt64(ub.GetSub("SFmallPay", xmlPath));
                    long big = Convert.ToInt64(ub.GetSub("SFBigPay", xmlPath));
                    string appName = "SFC";
                    int Expir = Utils.ParseInt(ub.GetSub("SFExpir", xmlPath));//5
                    //BCW.User.Users.IsFresh(appName, Expir);


                    string GameName = ub.GetSub("SFName", xmlPath);
                    //个人每期限投
                    long xPrices = 1;
                    xPrices = Utils.ParseInt64(ub.GetSub("SFCprice", xmlPath));

                    Random ran = new Random();
                    int[] a = new int[] { 0, 1, 3 };
                    string votenum1 = string.Empty;
                    string votenum = string.Empty;
                    string s = string.Empty;
                    for (int j = 0; j < 14; j++)
                    {
                        s = a[ran.Next(0, 3)].ToString();
                        votenum1 += s + ",";
                    }
                    votenum = votenum1;//得到随机投注

                    //支付安全提示
                    string[] p_pageArr = { "CID", "Price", "bei", "Price1", "votenum", "ptype", "act", "info" };
                    long gold = new BCW.BLL.User().GetGold(usID);

                    if (gold < Price1)
                    {
                        builder.Append("您的" + ub.Get("SiteBz") + "不足购买此下注");
                    }
                    if (gold < 0)
                    {
                        builder.Append("您的" + ub.Get("SiteBz") + "不足");
                    }
                    if (xPrices > 0)
                    {
                        int oPrices = 0;
                        DataSet ds = null;
                        try
                        {
                            ds = new BCW.SFC.BLL.SfPay().GetList("PayCents", "UsID=" + usID + " and CID=" + CID + "");
                        }
                        catch
                        {
                            Utils.Error("！", "");
                        }
                        DataTable dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)
                        {
                            int drs = int.Parse(dr[0].ToString());
                            oPrices = oPrices + drs;
                        }
                        if (oPrices + Price1 > xPrices)
                        {
                            if (oPrices >= xPrices)
                                Utils.Error("您（" + usID + "）本期下注已达上限，请等待下期...", "");
                            else
                                Utils.Error("您（" + usID + "）本期最多还可以下注" + (xPrices - oPrices) + "" + ub.Get("SiteBz") + "", "");
                        }
                    }
                    if (gold > Price1 && gold > 0 && CID != 0)
                    {

                        BCW.SFC.Model.SfPay modelpay = new BCW.SFC.Model.SfPay();
                        modelpay.usID = usID;
                        modelpay.Vote = votenum;//投注情况
                        modelpay.VoteNum = 1;//投注总数
                        modelpay.IsPrize = 0;//中奖情况
                        modelpay.IsSpier = 1;//机器人为1
                        modelpay.AddTime = DateTime.Now;
                        modelpay.CID = CID;//期号
                        modelpay.OverRide = Convert.ToInt32(bei);//投注倍率
                        modelpay.PayCents = Price1;//投注总额
                        modelpay.State = 0;//兑奖
                        modelpay.PayCent = Convert.ToInt32(ub.GetSub("SFmallPay", xmlPath));
                        modelpay.change = " ";
                        new BCW.SFC.BLL.SfPay().Add(modelpay);
                        //添加奖池数据
                        BCW.SFC.Model.SfJackpot mo = new BCW.SFC.Model.SfJackpot();
                        mo.usID = usID;
                        mo.WinPrize = 0;
                        mo.Prize = Price1;
                        mo.other = "下注" + Convert.ToString(Price1);
                        mo.allmoney = AllPrize(CID);
                        mo.AddTime = DateTime.Now;
                        mo.CID = CID;
                        new BCW.SFC.BLL.SfJackpot().Add(mo);


                        int maxid = new BCW.SFC.BLL.SfPay().GetMaxId(usID);
                        new BCW.BLL.User().UpdateiGold(usID, mename, -Price1, "胜负彩第" + CID + "期投注" + votenum + "标识id" + (maxid - 1));//胜负彩----更新排行榜与扣钱
                        //更新每期下注额度
                        new BCW.SFC.BLL.SfList().UpdatePayCent(new BCW.SFC.BLL.SfPay().PayCents(CID), CID);
                        //更新每期下注数
                        new BCW.SFC.BLL.SfList().UpdatePayCount(new BCW.SFC.BLL.SfPay().VoteNum(CID), CID);
                        string wText = "[url=/bbs/uinfo.aspx?uid=" + usID + "]" + mename + "[/url]在第" + CID + "期[url=/bbs/game/SFC.aspx]" + GameName + "[/url]下注**" + "" + ub.Get("SiteBz") + "";//+ Price1 
                        new BCW.BLL.Action().Add(1016, 1, usID, "", wText);
                        Response.Write("<br /><span style='color:red;font-size:20px'>胜负彩机器人正在执行下注任务!</span>");


                    }
                }
                else
                {
                    Response.Write("<br /><span style='color:red;font-size:20px'>胜负彩机器人(" + usID + ")已经完成下注任务!当期下注次数已达上限！</span>");
                }
            }
            else
            {
                Response.Write("<br /><span style='color:red;font-size:20px'>胜负彩机器人未进行下注，等待当期结束，下期开始投注！</span>");
            }
        }
    }
    //机器人兑奖
    public void RobotCase()
    {
        string sfc = ub.GetSub("SFName", xmlPath);
        string RoBot = ub.GetSub("RoBotID", xmlPath);
        string RoBotCost = ub.GetSub("ReBotCost", xmlPath);
        string[] RoBotID = RoBot.Split('#');
        int meid = 0;
        for (int i = 0; i < RoBotID.Length; i++)
        {
            meid = Convert.ToInt32(RoBotID[i]);
            DataSet model2 = new BCW.SFC.BLL.SfPay().GetList("id", "usID=" + meid + " and IsSpier=1 and WinCent > 0 and State!=2 ");
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
                    if (new BCW.SFC.BLL.SfPay().ExistsReBot(pid, meid))
                    {
                        if (new BCW.SFC.DAL.SfPay().getState(pid) != 2)
                        {
                            //new BCW.SFC.BLL.SfPay().RoBotByID(pid);
                            //操作币
                            BCW.SFC.Model.SfPay model = new BCW.SFC.BLL.SfPay().GetSfPay(pid);
                            long winMoney = Convert.ToInt64(model.WinCent);
                            int number = new BCW.SFC.BLL.SfPay().GetCID(pid);
                            if (model.IsPrize == 10)
                            {
                                new BCW.BLL.User().UpdateiGold(meid, winMoney, "" + sfc + "兑奖-期号-" + number + "-标识ID" + pid + "(" + model.change + "奖)");
                            }
                            else
                            {
                                new BCW.BLL.User().UpdateiGold(meid, winMoney, "" + sfc + "兑奖-期号-" + number + "-标识ID" + pid + "(" + model.IsPrize + "等奖)");
                            }
                            new BCW.SFC.BLL.SfPay().UpdateState(pid);
                            builder.Append("机器人" + meid + "自动兑奖!ok1<br />");
                        }
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
    // 获得奖池币
    private long AllPrize(int resultCID)
    {
        //获取当前期数投注总额
        long AllPrice = new BCW.SFC.BLL.SfPay().AllPrice(resultCID);
        //获取已经派出奖金
        long _Price = new BCW.SFC.BLL.SfPay().AllWinCentbyCID(resultCID);
        //获取当前期数系统投注总额
        //long SysPrice = new BCW.SFC.BLL.SfJackpot().SysPrice();
        long Sysprize = new BCW.SFC.DAL.SfList().getsysprize(resultCID);
        //获取当期系统投注状态
        int Sysprizestatue = new BCW.SFC.DAL.SfList().getsysprizestatue(resultCID);
        //获取上一期滚存下来的奖池
        int lastcid = 0;
        if (new BCW.SFC.BLL.SfList().ExistsCID(resultCID - 1))
        {
            lastcid = (resultCID - 1);
        }
        else
        {
            lastcid = LastOpenCID();
        }
        long Nextprize = new BCW.SFC.DAL.SfList().getnextprize(lastcid);

        //获取当前期数系统回收总额
        long SysWin = new BCW.SFC.BLL.SfJackpot().SysWin(resultCID);
        //奖池总额
        long Prices = 0;
        if (Sysprizestatue == 3 || Sysprizestatue == 1)
        {
            Prices = (AllPrice + Nextprize + Sysprize);
        }
        else
        {
            Prices = (AllPrice + Nextprize);
        }
        return Prices;
    }
    // 获得当期奖池结余
    private long NextPrize(int resultCID)
    {
        long nowprize = new BCW.SFC.BLL.SfList().nowprize(resultCID);
        //获取已经派出奖金
        long _Price = new BCW.SFC.BLL.SfPay().AllWinCentbyCID(resultCID);
        long sysprizeshouxu = Convert.ToInt64(nowprize * Convert.ToInt64(ub.GetSub("SFsys", "/Controls/SFC.xml")) * 0.01);
        long Prices = nowprize - _Price - sysprizeshouxu;//当期结余=当期奖池-当期系统收取-当期派奖
        return Prices;
    }
    // 获得当期剩余奖池（为每一次减去中奖额减去系统回收）
    private long NowPrize(int resultCID)
    {
        long nowprize1 = new BCW.SFC.BLL.SfList().nowprize(resultCID);
        long sysprizeshouxu = Convert.ToInt64(nowprize1 * Convert.ToInt64(ub.GetSub("SFsys", "/Controls/SFC.xml")) * 0.01);
        long Prices = nowprize1 - sysprizeshouxu;//当期结余=当期奖池-当期系统收取-当期派奖
        return Prices;
    }
    //获取数据库最新已经开奖期号
    private int LastOpenCID()
    {
        try
        {
            int CID = 0;
            DataSet ds = new BCW.SFC.BLL.SfList().GetList("CID", " State=1 Order by CID Desc ");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                CID = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
            }
            return CID;
        }
        catch { return 0; }
    }
    //获取数据库最新未开奖期号
    private int FirstNewCID()
    {
        try
        {
            int CID = 0;
            DataSet ds = new BCW.SFC.BLL.SfList().GetList("CID", " State=0 Order by CID Asc ");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                CID = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
            }
            return CID;
        }
        catch { return 0; }
    }
}
