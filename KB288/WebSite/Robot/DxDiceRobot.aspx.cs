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

public partial class Robot_DxDiceRobot : System.Web.UI.Page
{
    protected string xmlPath = "/Controls/dxdice.xml";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (ub.GetSub("DxdiceIsBot", xmlPath) != "1")
        {
            Response.Write("close");
        }
        else
        {
            ChangePalyDxdice();
            Response.Write("ok");
        }
    }

    /// <summary>
    /// 掷骰自动游戏程序
    /// </summary>
    /// <param name="DxdiceId">局数ID</param>
    /// <param name="dt">截止时间</param>
    private void ChangePalyDxdice()
    {
        int UsIDNum = new BCW.BLL.Game.Dxdice().GetCountState(0);//多少未结束的掷骰

        //如果小于5个则自动出动1个ID来开掷骰
        if (UsIDNum < 5)
        {
            PlayDxdice(0);
        }
        //应战
        PlayDxdice2(0);
    }

    //下注操作
    private void PlayDxdice(int meid)
    {
        if (meid == 0)
            meid = GetUsID();

        ChanageOnline(meid);
        //币的类型
        int bzType = new Random().Next(0, 2);
        long paycent = GetPayCent();
        if (bzType == 1)
            paycent = paycent * 10;

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
        if (paycent > gold)
        {
            //更新消费记录
            BCW.Model.Goldlog modelx = new BCW.Model.Goldlog();
            modelx.BbTag = 3;
            modelx.Types = bzType;
            modelx.PUrl = Utils.getPageUrl();//操作的文件名
            modelx.UsId = meid;
            modelx.UsName = mename;
            modelx.AcGold = 500000;
            modelx.AfterGold = gold + 500000;//更新后的币数
            modelx.AcText = "系统机器人自动操作";
            modelx.AddTime = DateTime.Now;
            new BCW.BLL.Goldlog().Add(modelx);
            if (bzType == 0)
                BCW.Data.SqlHelper.ExecuteSql("Update tb_User set iGold=iGold+500000 where id=" + meid + "");
            else
                BCW.Data.SqlHelper.ExecuteSql("Update tb_User set iMoney=iMoney+500000 where id=" + meid + "");
        }
        if (bzType == 0)
            new BCW.BLL.User().UpdateiGold(meid, mename, -paycent, "掷骰消费");
        else
            new BCW.BLL.User().UpdateiMoney(meid, mename, -paycent, "掷骰消费");


        Random rd = new Random(unchecked((int)DateTime.Now.Ticks));
        int A = rd.Next(1, 7);
        int B = rd.Next(1, 7);

        if (A + B < 5)
        {
            if (A > B)
            {
                A = A + rd.Next(1, 3);
            }
            else
            {
                B = B + rd.Next(1, 3);
            }
        }


        BCW.Model.Game.Dxdice model = new BCW.Model.Game.Dxdice();
        model.Types = 0;
        model.DxdiceA = A + "#" + B;
        model.DxdiceB = "";
        model.StopTime = DateTime.Now.AddHours(1);
        model.UsID = meid;
        model.UsName = mename;
        model.AddTime = DateTime.Now;
        model.ReID = 0;
        model.ReName = "";
        model.Price = paycent;
        model.IsWin = 0;
        model.State = 0;
        model.BzType = bzType;
        int id = new BCW.BLL.Game.Dxdice().Add(model);
        string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/dxdice.aspx]大小掷骰[/url]开盘成功(" + paycent + "" + bzText + ")";
        new BCW.BLL.Action().Add(18, id, 0, "", wText);
    }


    //应战操作
    private void PlayDxdice2(int meid)
    {
        if (meid == 0)
            meid = GetUsID();

        ChanageOnline(meid);

        DataSet ds = new BCW.BLL.Game.Dxdice().GetList("ID", "Types=0 and State=0 and UsID<>" + meid + " and AddTime<'" + DateTime.Now.AddMinutes(-20) + "' ORDER BY NEWID()");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            int TNum = Utils.ParseInt(ub.GetSub("DxdiceTNum", xmlPath));
            if (TNum > 0)
            {
                int TCount2 = new BCW.BLL.Game.Dxdice().GetCount2(meid);
                if (TCount2 > TNum)
                {
                    int TCount = new BCW.BLL.Game.Dxdice().GetCount(meid);
                    if ((TCount2 - TNum) > Convert.ToInt32(TCount * TNum))
                    {
                        PlayDxdice(0);
                        meid = GetUsID();
                    }
                }
            }
            string mename = new BCW.BLL.User().GetUsName(meid);

            int id = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
            BCW.Model.Game.Dxdice model = new BCW.BLL.Game.Dxdice().GetDxdice(id);

            long gold = 0;
            string bzText = string.Empty;
            if (model.BzType == 0)
            {
                gold = new BCW.BLL.User().GetGold(meid);
                bzText = ub.Get("SiteBz");
            }
            else
            {
                gold = new BCW.BLL.User().GetMoney(meid);
                bzText = ub.Get("SiteBz2");
            }
            if (model.Price > gold)
            {
                //更新消费记录
                BCW.Model.Goldlog modelx = new BCW.Model.Goldlog();
                modelx.BbTag = 3;
                modelx.Types = model.BzType;
                modelx.PUrl = Utils.getPageUrl();//操作的文件名
                modelx.UsId = meid;
                modelx.UsName = mename;
                modelx.AcGold = 500000;
                modelx.AfterGold = gold + 500000;//更新后的币数
                modelx.AcText = "系统机器人自动操作";
                modelx.AddTime = DateTime.Now;
                new BCW.BLL.Goldlog().Add(modelx);
                if (model.BzType == 0)
                    BCW.Data.SqlHelper.ExecuteSql("Update tb_User set iGold=iGold+500000 where id=" + meid + "");
                else
                    BCW.Data.SqlHelper.ExecuteSql("Update tb_User set iMoney=iMoney+500000 where id=" + meid + "");
            }

            //操作币
            long winMoney = model.Price;
            //税率
            long SysTax = 0;
            int Tax = Utils.ParseInt(ub.GetSub("DxdiceTar", xmlPath));

            if (Tax > 0)
            {
                SysTax = Convert.ToInt64(winMoney * Tax * 0.01);
            }
            winMoney = winMoney - SysTax;
    
            //庄家是不是机器人
            bool IsRobot = false;
            if (new BCW.BLL.User().GetIsSpier(model.UsID) == 1)
            {
                IsRobot = true;
            }

            Random rd = new Random(unchecked((int)DateTime.Now.Ticks));
            int C = rd.Next(1, 7);
            int D = rd.Next(1, 7);
            int IsWin = 0;
            int iWin = GetiWin(model.DxdiceA, "" + C + "#" + D + "");
            if (iWin == 3)
            {
                //消费
                if (model.BzType == 0)
                {
                    new BCW.BLL.User().UpdateiGold(meid, mename, winMoney, 10);
                    new BCW.BLL.User().UpdateiGoldTop(model.UsID, model.UsName, -model.Price, 10);
                }
                else
                    new BCW.BLL.User().UpdateiMoney(meid, mename, winMoney, "掷骰消费");

                //内线与动态
                if (!IsRobot)
                {
                    new BCW.BLL.Guest().Add(1, model.UsID, model.UsName, "您的掷骰已经结束，参与庄家[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]，结果对方掷了" + C + "#" + D + "点战胜了你掷的" + model.DxdiceA + "点！[url=/bbs/game/dxdice.aspx?act=add]我要继续掷骰[/url]");
                }

                string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/dxdice.aspx]大小掷骰[/url]以" + C + "#" + D + "点战胜了" + model.UsName + "掷的" + model.DxdiceA + "点(赢" + model.Price + "" + bzText + ")";
                new BCW.BLL.Action().Add(18, id, 0, "", wText);

            }
            else if (iWin == 2)
            {
                IsWin = 2;//打平
                //内线与动态
                if (!IsRobot)
                {
                    new BCW.BLL.Guest().Add(1, model.UsID, model.UsName, "您的掷骰已经结束，参与庄家[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]，结果对方掷了" + C + "#" + D + "点打平了你掷的" + model.DxdiceA + "点！[url=/bbs/game/dxdice.aspx?act=case]马上兑奖[/url]");

                }
                else//自动兑换
                {
                    new BCW.BLL.Game.Dxdice().UpdateState(id, 2);
                    if (model.BzType == 0)
                    {
                        new BCW.BLL.User().UpdateiGold(model.UsID, model.Price, "掷骰消费");
                    }
                    else
                    {
                        new BCW.BLL.User().UpdateiMoney(model.UsID, model.Price, "掷骰消费");
                    }

                }
                string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/dxdice.aspx]大小掷骰[/url]以" + C + "#" + D + "点打平" + model.UsName + "掷的" + model.DxdiceA + "点";
                new BCW.BLL.Action().Add(18, id, 0, "", wText);

            }
            else
            {
                IsWin = 1;
                //消费
                if (model.BzType == 0)
                {
                    new BCW.BLL.User().UpdateiGold(meid, mename, -model.Price, 10);
                    new BCW.BLL.User().UpdateiGoldTop(model.UsID, model.UsName, winMoney, 10);
                }
                else
                    new BCW.BLL.User().UpdateiMoney(meid, mename, -model.Price, "掷骰消费");

                //内线与动态
                if (!IsRobot)
                {
                    new BCW.BLL.Guest().Add(1, model.UsID, model.UsName, "您的掷骰已经结束，参与庄家[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]，结果对方掷了" + C + "#" + D + "点负了你掷的" + model.DxdiceA + "点！[url=/bbs/game/dxdice.aspx?act=case]马上兑奖[/url]");
                }
                else//自动兑换
                {
                    new BCW.BLL.Game.Dxdice().UpdateState(id, 2);
                    if (model.BzType == 0)
                    {
                        new BCW.BLL.User().UpdateiGold(model.UsID, winMoney, "掷骰消费");
                    }
                    else
                    {
                        new BCW.BLL.User().UpdateiMoney(model.UsID, winMoney, "掷骰消费");
                    }
                
                }
                string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/dxdice.aspx]大小掷骰[/url]以" + C + "#" + D + "点负了" + model.UsName + "掷的" + model.DxdiceA + "点(输" + model.Price + "" + bzText + ")";
                new BCW.BLL.Action().Add(18, id, 0, "", wText);
            }
            //更新掷骰记录
            BCW.Model.Game.Dxdice m = new BCW.Model.Game.Dxdice();
            m.ID = id;
            m.ReID = meid;
            m.DxdiceB = C + "#" + D;
            m.ReName = mename;
            m.ReTime = DateTime.Now;
            m.IsWin = IsWin;
            m.State = 1;
            new BCW.BLL.Game.Dxdice().UpdateState(m);
        }
    }

    /// <summary>
    /// 1开盘人赢/2打和/3开盘人输
    /// </summary>
    /// <param name="DxdiceA">开盘骰</param>
    /// <param name="DxdiceB">激战骰</param>
    /// <returns></returns>
    private int GetiWin(string DxdiceA, string DxdiceB)
    {
        int diceA1 = Utils.ParseInt(Utils.Left(DxdiceA, 1));
        int diceA2 = Utils.ParseInt(Utils.Right(DxdiceA, 1));
        int diceB1 = Utils.ParseInt(Utils.Left(DxdiceB, 1));
        int diceB2 = Utils.ParseInt(Utils.Right(DxdiceB, 1));
        int diceA = diceA1 + diceA2;
        int diceB = diceB1 + diceB2;
        if (diceA == diceB)
        {
            if (diceA1 == diceB1 || diceA1 == diceB2)
            {
                return 2;
            }
            int[] iNum = { diceA1, diceA2, diceB1, diceB2 };
            int max = Utils.maxNum(iNum);
            if (max == diceA1 || max == diceA2)
            {
                return 1;
            }
            else
            {
                return 3;
            }
        }
        else if (diceA > diceB)
        {
            return 1;
        }
        else
        {
            return 3;
        }

    }

    //得到出动的ID
    private int GetUsID()
    {
        int UsID = 0;
        string PlayUsID = ub.GetSub("GameZDID", "/Controls/GameZDID.xml");
        string[] sNum = Regex.Split(PlayUsID, "#");
        Random rd = new Random();
        UsID = Convert.ToInt32(sNum[rd.Next(sNum.Length)]);
        return UsID;
    }

    //随机得到下注的币数
    private long GetPayCent()
    {
        Random rac = new Random(unchecked((int)DateTime.Now.Ticks));
        long paycent = 0;

        string[] sNum = { "100", "200", "1000", "5000", "10000", "50000" };
        paycent = Convert.ToInt64(sNum[rac.Next(sNum.Length)]);

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




}
