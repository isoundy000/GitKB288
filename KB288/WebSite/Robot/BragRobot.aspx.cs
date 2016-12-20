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

public partial class Robot_BragRobot : System.Web.UI.Page
{
    protected string xmlPath = "/Controls/brag.xml";
    protected long F1 = Convert.ToInt64(ub.GetSub("BragF1", "/Controls/brag.xml"));
    protected long F2 = Convert.ToInt64(ub.GetSub("BragF2", "/Controls/brag.xml"));
    protected long F3 = Convert.ToInt64(ub.GetSub("BragF3", "/Controls/brag.xml"));
    protected long F4 = Convert.ToInt64(ub.GetSub("BragF4", "/Controls/brag.xml"));
    protected void Page_Load(object sender, EventArgs e)
    {
        if (ub.GetSub("BragIsBot", xmlPath) != "1")
        {
            Response.Write("close");
        }
        else
        {
            ChangePalyBrag();
            Response.Write("ok");
        }
    }

    /// <summary>
    /// 吹牛自动游戏程序
    /// </summary>
    /// <param name="BragId">局数ID</param>
    /// <param name="dt">截止时间</param>
    private void ChangePalyBrag()
    {
        int UsIDNum = new BCW.BLL.Game.Brag().GetCountState(0);//多少未结束的吹牛

        //如果小于8个则自动出动1个ID来开吹牛
        if (UsIDNum < 5)
        {
            PlayBrag(0);
        }
        //应战
        PlayBrag2(0);
    }

    //下注操作
    private void PlayBrag(int meid)
    {
        if (meid == 0)
            meid = GetUsID();

        ChanageOnline(meid);

        Random ra = new Random(unchecked((int)DateTime.Now.Ticks));
        int bzType = ra.Next(0, 2);
        int ptype = ra.Next(1, 3);
        //币的类型
     
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
            new BCW.BLL.User().UpdateiGold(meid, mename, -paycent, "吹牛消费");
        else
            new BCW.BLL.User().UpdateiMoney(meid, mename, -paycent, "吹牛消费");

        BCW.Model.Game.Brag model = new BCW.Model.Game.Brag();
        model.Title = "我比你帅多了";
        model.Types = 0;
        model.BragA = "信";
        model.BragB = "不信";
        model.ChooseType = 0;
        model.TrueType = ptype;
        model.StopTime = DateTime.Now.AddHours(1);
        model.UsID = meid;
        model.UsName = mename;
        model.AddTime = DateTime.Now;
        model.ReID = 0;
        model.ReName = "";
        model.Price = paycent;
        model.State = 0;
        model.BzType = bzType;
        int id = new BCW.BLL.Game.Brag().Add(model);
        string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/brag.aspx]疯狂吹牛[/url]开盘成功(" + paycent + "" + bzText + ")";
        new BCW.BLL.Action().Add(14, id, 0, "", wText);
    }


    //应战操作
    private void PlayBrag2(int meid)
    {
        if (meid == 0)
            meid = GetUsID();

        ChanageOnline(meid);

        Random ra = new Random(unchecked((int)DateTime.Now.Ticks));
        int ptype = ra.Next(1, 3);
     
        DataSet ds = new BCW.BLL.Game.Brag().GetList("ID", "Types=0 and State=0 and UsID<>" + meid + " and AddTime<'" + DateTime.Now.AddMinutes(-20) + "' ORDER BY NEWID()");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            int TNum = Utils.ParseInt(ub.GetSub("BragTNum", xmlPath));
            if (TNum > 0)
            {
                int TCount2 = new BCW.BLL.Game.Brag().GetCount2(meid);
                if (TCount2 > TNum)
                {
                    int TCount = new BCW.BLL.Game.Brag().GetCount(meid);
                    if ((TCount2 - TNum) > Convert.ToInt32(TCount * TNum))
                    {
                        PlayBrag(0);
                        meid = GetUsID();
                    }
                }
            }
            string mename = new BCW.BLL.User().GetUsName(meid);

            int id = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
            BCW.Model.Game.Brag model = new BCW.BLL.Game.Brag().GetBrag(id);

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
            //应战十赌六赢
            //if (model.Price > 10000)
            //{
                int bet = model.TrueType;
                Random ra2 = new Random(unchecked((int)DateTime.Now.Ticks));
                int rdNext = ra2.Next(1, 1000);
                if (rdNext <= 400)
                {
                    rdNext = ((bet == 1) ? 2 : 1);
                }
                else
                {
                    rdNext = bet;
                }
                ptype = rdNext;
            //}

            //更新吹牛记录
            BCW.Model.Game.Brag m = new BCW.Model.Game.Brag();
            m.ID = id;
            m.ReID = meid;
            m.ReName = mename;
            m.ReTime = DateTime.Now;
            m.ChooseType = ptype;
            m.State = 1;
            new BCW.BLL.Game.Brag().UpdateState(m);

            //操作币
            long winMoney = model.Price;
            //税率
            long SysTax = 0;
            int Tax = 0;
            if (CaseBrag(model.Price, model.BzType) == "蜗牛")
                Tax = Utils.ParseInt(ub.GetSub("BragTar1", xmlPath));
            else if (CaseBrag(model.Price, model.BzType) == "水牛")
                Tax = Utils.ParseInt(ub.GetSub("BragTar2", xmlPath));
            else
                Tax = Utils.ParseInt(ub.GetSub("BragTar3", xmlPath));

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

            if (model.TrueType == ptype)
            {
                if (model.BzType == 0)
                {
                    new BCW.BLL.User().UpdateiGold(meid, mename, winMoney, 7);
                    new BCW.BLL.User().UpdateiGoldTop(model.UsID, model.UsName, -model.Price, 7);
                }
                else
                {
                    new BCW.BLL.User().UpdateiMoney(meid, mename, winMoney, "吹牛消费");
                }
                if (!IsRobot)
                {
                    new BCW.BLL.Guest().Add(1, model.UsID, model.UsName, "您的吹牛:" + model.Title + "已经结束，参与吹牛人[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]，结果你忽悠不了对方，输了！[url=/bbs/game/brag.aspx?act=add]我要继续吹[/url]");
                }
                string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/brag.aspx]疯狂吹牛[/url]识破了" + model.UsName + "的吹牛(赢" + model.Price + "" + bzText + ")";
                new BCW.BLL.Action().Add(14, id, 0, "", wText);

            }
            else
            {
                if (model.BzType == 0)
                {
                    new BCW.BLL.User().UpdateiGold(meid, mename, -model.Price, 7);
                    new BCW.BLL.User().UpdateiGoldTop(model.UsID, model.UsName, winMoney, 7);
                }
                else
                {
                    new BCW.BLL.User().UpdateiGold(meid, mename, -model.Price, "吹牛消费");
                }
                //如果是机器人就马上兑奖
                if (!IsRobot)
                {
                    new BCW.BLL.Guest().Add(1, model.UsID, model.UsName, "您的吹牛:" + model.Title + "已经结束，参与吹牛人[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]，结果全赢！[url=/bbs/game/brag.aspx?act=case]马上兑奖[/url]");
                }
                else//自动兑换
                {
                    new BCW.BLL.Game.Brag().UpdateState(id, 2);
                    if (model.BzType == 0)
                    {
                        new BCW.BLL.User().UpdateiGold(model.UsID, winMoney, "吹牛消费");
                    }
                    else
                    {
                        new BCW.BLL.User().UpdateiMoney(model.UsID, winMoney, "吹牛消费");
                    }
                }
                string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/brag.aspx]疯狂吹牛[/url]中的" + model.UsName + "的吹牛上当了(输" + model.Price + "" + bzText + ")";
                new BCW.BLL.Action().Add(14, id, 0, "", wText);
            }
        }
    }

    private string CaseBrag(long Price, int BzType)
    {
        if (BzType == 0)
        {
            if (Price >= F2)
                return "犀牛";

            if (Price > F1)
                return "水牛";
        }
        else
        {
            if (Price >= F4)
                return "犀牛";

            if (Price > F3)
                return "水牛";
        }
        return "蜗牛";
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
