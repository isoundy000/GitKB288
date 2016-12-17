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

public partial class Robot_BigSmallRobot : System.Web.UI.Page
{
    protected string xmlPath = "/Controls/bjl.xml";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (ub.GetSub("BsIsBot", xmlPath) != "1")
        {
            Response.Write("close1");
        }
        else
        {
            //ChangePalyBigSmall();
            Response.Write("ok1!");
        }
    }
    /// <summary>
    /// 大小庄自动游戏程序
    /// </summary>
    /// <param name="BigSmallId">局数ID</param>
    /// <param name="dt">截止时间</param>
    private void ChangePalyBigSmall()
    {

        int BotSec = new Random().Next(420, 600);
        DateTime BotTime = Convert.ToDateTime(ub.GetSub("BsBotTime", xmlPath));
        if (BotTime < DateTime.Now.AddSeconds(-BotSec))
        {
            //更新某分钟已出动过
            ub xml = new ub();
            xml.ReloadSub(xmlPath); //加载配置
            xml.dss["BsBotTime"] = DT.FormatDate(DateTime.Now, 0);

            System.IO.File.WriteAllText(HttpContext.Current.Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            //进行自动下注
            PlayBigSmall();
        }
    }


    private void PlayBigSmall()
    {
        DataSet ds = new BCW.Baccarat.BLL.BJL_Room().GetList("TOP 5 *", "state=0 AND Total_Now>LowTotal Order by NEWID()");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                int id = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                long SmallPay = Int64.Parse(ds.Tables[0].Rows[i]["LowTotal"].ToString());//最小下注
                long BigPay = Int64.Parse(ds.Tables[0].Rows[i]["BigPay"].ToString());//最高下注

                long PayCent = GetPayCent();

                int k = 0;
                while (PayCent < SmallPay || PayCent > BigPay)
                {
                    long vPayCent = GetPayCent();
                    PayCent = vPayCent;

                    k++;

                    if (k > 200)
                    {
                        PayCent = SmallPay;
                        break;
                    }
                }
                int meid = GetUsID();
                PlayBigSmallInfo(meid, id, PayCent);
            }
        }
    }


    private void PlayBigSmallInfo(int meid, int id, long PayCent)
    {
        ChanageOnline(meid);
        string mename = new BCW.BLL.User().GetUsName(meid);

        BCW.Model.Game.Bslist model = new BCW.BLL.Game.Bslist().GetBslist(id);
        if (model.UsID == meid)
        {
            Response.End();
        }

        int bet = new Random().Next(0, 2);
        if (PayCent < model.SmallPay || PayCent > model.BigPay)
        {
            Response.End();
        }
        if (PayCent > model.Money)
        {
            Response.End();
        }

        long gold = 0;
        string bzText = string.Empty;
        if (model.BzType == 0)
        {
            bzText = ub.Get("SiteBz");
            gold = new BCW.BLL.User().GetGold(meid);
        }
        else
        {
            bzText = ub.Get("SiteBz2");
            gold = new BCW.BLL.User().GetMoney(meid);
        }
        if (gold < PayCent)
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

        bool IsWin = false;
        Random ra = new Random(unchecked((int)DateTime.Now.Ticks));
        int rdNext = ra.Next(0, 2);

        if (rdNext == bet)
            IsWin = true;

        //客家十赌六赢
        string IsWinBot = ub.GetSub("BsIsWinBot", xmlPath);
        if (IsWinBot == "0")
        {
            rdNext = ra.Next(1, 1000);
            if (rdNext <= 400)
            {
                rdNext = ((bet == 0) ? 1 : 0);
                IsWin = false;
            }
            else
            {
                rdNext = bet;
                IsWin = true;
            }
        }

        long WinCent = 0;
        if (IsWin)//闲胜
        {
            double XTar = Convert.ToDouble(ub.GetSub("BsXTar", xmlPath));
            long xMoney = PayCent - Convert.ToInt64(XTar * 0.01 * PayCent);
            if (model.BzType == 0)
            {
                //更新排行榜
                BCW.Model.Toplist modeltop = new BCW.Model.Toplist();
                modeltop.Types = 9;
                modeltop.UsId = model.UsID;
                modeltop.UsName = model.UsName;

                modeltop.PutNum = 1;
                modeltop.PutGold = -PayCent;

                if (!new BCW.BLL.Toplist().Exists(model.UsID, 9))
                    new BCW.BLL.Toplist().Add(modeltop);
                else
                    new BCW.BLL.Toplist().Update(modeltop);

                new BCW.BLL.User().UpdateiGold(meid, mename, xMoney, 9);
            }
            else
                new BCW.BLL.User().UpdateiMoney(meid, mename, xMoney, "大小庄赢得");

            new BCW.BLL.Game.Bslist().UpdateMoney(id, -PayCent);
            WinCent = xMoney;

        }
        else//庄胜
        {
            double ZTar = Convert.ToDouble(ub.GetSub("BsZTar", xmlPath));
            long zMoney = PayCent - Convert.ToInt64(ZTar * 0.01 * PayCent);
            new BCW.BLL.Game.Bslist().UpdateMoney(id, zMoney);
            if (model.BzType == 0)
            {
                //更新排行榜
                BCW.Model.Toplist modeltop = new BCW.Model.Toplist();
                modeltop.Types = 9;
                modeltop.UsId = model.UsID;
                modeltop.UsName = model.UsName;
                modeltop.WinNum = 1;
                modeltop.WinGold = zMoney;
                if (!new BCW.BLL.Toplist().Exists(model.UsID, 9))
                    new BCW.BLL.Toplist().Add(modeltop);
                else
                    new BCW.BLL.Toplist().Update(modeltop);

                new BCW.BLL.User().UpdateiGold(meid, mename, -PayCent, 9);
            }
            else
            {
                new BCW.BLL.User().UpdateiMoney(meid, mename, -PayCent, "大小庄失去");
            }
            WinCent = -PayCent;
        }

        //写进下注记录
        BCW.Model.Game.Bspay addmodel = new BCW.Model.Game.Bspay();
        addmodel.BsId = id;
        addmodel.BsTitle = model.Title;
        addmodel.BzType = model.BzType;
        addmodel.BetType = bet;
        addmodel.PayCent = PayCent;
        addmodel.UsID = meid;
        addmodel.UsName = mename;
        addmodel.WinCent = WinCent;
        addmodel.AddTime = DateTime.Now;
        new BCW.BLL.Game.Bspay().Add(addmodel);
        //写进人气
        new BCW.BLL.Game.Bslist().UpdateClick(id);
        //动态
        string wText = string.Empty;
        if (IsWin == true)
            wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/bigsmall.aspx]大小庄[/url]《" + model.Title + "》赢得了" + PayCent + "" + bzText + "";
        else
            wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/bigsmall.aspx]大小庄[/url]《" + model.Title + "》失去了" + PayCent + "" + bzText + "";

        new BCW.BLL.Action().Add(13, 0, 0, "", wText);

    }


    //得到出动的ID
    private int GetUsID()
    {
        int UsID = 0;
        string PlayUsID = ub.GetSub("BsBotID", "/Controls/bjl.xml");
        if (PlayUsID != "")
        {
            string[] sNum = Regex.Split(PlayUsID, "#");
            Random rd = new Random();
            UsID = Convert.ToInt32(sNum[rd.Next(sNum.Length)]);
        }
        return UsID;
        //N秒内，此ID是否消费过
        //if (new BCW.BLL.Goldlog().ExistsUsID(UsID, 300))
        //{
        //}
    }

    //随机得到下注的币数(取整十整百整千整万等)
    private long GetPayCent()
    {
        Random rac = new Random(unchecked((int)DateTime.Now.Ticks));
        long paycent = 100;
        string Pay = ub.GetSub("Bspay", "/Controls/bjl.xml");
        if (Pay != "")
        {
            string[] sNum = Regex.Split(Pay, "#");
            paycent = Convert.ToInt64(sNum[rac.Next(sNum.Length)]);
        }
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
