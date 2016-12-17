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

public partial class Robot_DiceRobot : System.Web.UI.Page
{
    protected string xmlPath = "/Controls/dice.xml";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (ub.GetSub("DiceIsBot", xmlPath) != "1")
        {
            Response.Write("close");
        }
        else
        {
            ChangePalyDice();
            Response.Write("ok");
        }
    }

    /// <summary>
    /// 挖宝自动游戏程序
    /// </summary>
    /// <param name="DiceId">局数ID</param>
    /// <param name="dt">截止时间</param>
    private void ChangePalyDice()
    {
        BCW.Model.Game.Dicelist dice = null;
        new BCW.User.Game.Dice().DicePage();
        dice = new BCW.BLL.Game.Dicelist().GetDicelist();
        int DiceId = dice.ID;
        long Sec = DT.DateDiff(dice.EndTime, DateTime.Now, 4);
        if (DiceId > 0)
        {
            int UsIDNum = new BCW.BLL.Game.Dicepay().GetCount(DiceId);//当期下注ID数

            //如果小于15个则自在不同秒数中自动出动1个ID来下注
            if (UsIDNum < 5)
            {
                bool IsPlay = false;
                Sec = 360 - Sec;

                int ZD = 0;
                if (Sec > 5 && Sec <= 30 && ub.GetSub("DiceZD1", xmlPath) == "0")
                {
                    IsPlay = true;
                    ZD = 1;
                }
                if (Sec > 30 && Sec <= 60 && ub.GetSub("DiceZD2", xmlPath) == "0")
                {
                    IsPlay = true;
                    ZD = 2;
                }
                else if (Sec > 60 && Sec <= 90 && ub.GetSub("DiceZD3", xmlPath) == "0")
                {
                    IsPlay = true;
                    ZD = 3;
                }
                else if (Sec > 90 && Sec <= 120 && ub.GetSub("DiceZD4", xmlPath) == "0")
                {
                    IsPlay = true;
                    ZD = 4;
                }
                else if (Sec > 120 && Sec <= 150 && ub.GetSub("DiceZD5", xmlPath) == "0")
                {
                    IsPlay = true;
                    ZD = 5;
                }
                else if (Sec > 150 && Sec <= 180 && ub.GetSub("DiceZD6", xmlPath) == "0")
                {
                    IsPlay = true;
                    ZD = 6;
                }
                else if (Sec > 180 && Sec <= 210 && ub.GetSub("DiceZD7", xmlPath) == "0")
                {
                    IsPlay = true;
                    ZD = 7;
                }
                else if (Sec > 210 && Sec <= 240 && ub.GetSub("DiceZD8", xmlPath) == "0")
                {
                    IsPlay = true;
                    ZD = 8;
                }
                else if (Sec > 240 && Sec <= 270 && ub.GetSub("DiceZD9", xmlPath) == "0")
                {
                    IsPlay = true;
                    ZD = 9;
                }
                else if (Sec > 270 && Sec <= 300 && ub.GetSub("DiceZD10", xmlPath) == "0")
                {
                    IsPlay = true;
                    ZD = 10;
                }
                else if (Sec > 300 && Sec <= 330 && ub.GetSub("DiceZD11", xmlPath) == "0")
                {
                    IsPlay = true;
                    ZD = 11;
                }
                else if (Sec > 330 && Sec <= 355 && ub.GetSub("DiceZD12", xmlPath) == "0")
                {
                    IsPlay = true;
                    ZD = 12;
                }
                if (IsPlay)
                {
                    //更新某分钟已出动过
                    ub xml = new ub();
                    xml.ReloadSub(xmlPath); //加载配置
                    xml.dss["DiceZD" + ZD + ""] = 1;

                    System.IO.File.WriteAllText(HttpContext.Current.Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                    //进行自动下注
                    PlayDice(DiceId);
                }
            }
        }
    }

    //下注操作
    private void PlayDice(int DiceId)
    {
        int meid = GetUsID();

        ChanageOnline(meid);
        int ptype = GetPtype();
        int bzType = new Random().Next(0, 2);
        long paycent = GetPayCent(ptype);
        //if (bzType == 1)
            //paycent = paycent * 10;

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
            if (bzType == 0)
            {
                modelx.AcGold = 500000;
                modelx.AfterGold = gold + 500000;//更新后的币数
            }
            else
            {
                modelx.AcGold = 5000000;
                modelx.AfterGold = gold + 5000000;//更新后的币数
            }
            modelx.AcText = "系统机器人自动操作";
            modelx.AddTime = DateTime.Now;
            new BCW.BLL.Goldlog().Add(modelx);
            if (bzType == 0)
                BCW.Data.SqlHelper.ExecuteSql("Update tb_User set iGold=iGold+500000 where id=" + meid + "");
            else
                BCW.Data.SqlHelper.ExecuteSql("Update tb_User set iMoney=iMoney+5000000 where id=" + meid + "");

        }


        int BuyNum = GetBuyNum(ptype);

        //加总押注额
        if (bzType == 0)
        {
            new BCW.BLL.Game.Dicelist().UpdatePool(DiceId, paycent);
            new BCW.BLL.User().UpdateiGold(meid, mename, -paycent, "挖宝押注");
        }
        else
        {
            new BCW.BLL.Game.Dicelist().UpdateWinPool(DiceId, paycent);
            new BCW.BLL.User().UpdateiMoney(meid, mename, -paycent, "挖宝押注");
        }

        BCW.Model.Game.Dicepay model = new BCW.Model.Game.Dicepay();
        model.DiceId = DiceId;
        model.UsID = meid;
        model.UsName = mename;
        model.Types = ptype;
        model.BuyNum = BuyNum;
        model.BuyCent = paycent;
        model.BuyCount = 1;
        model.WinCent = 0;
        model.AddTime = DateTime.Now;
        model.State = 0;
        model.bzType = bzType;
        if (!new BCW.BLL.Game.Dicepay().Exists(DiceId, meid, bzType, ptype, BuyNum))
        {
            int pid=new BCW.BLL.Game.Dicepay().Add(model);
            BCW.Data.SqlHelper.ExecuteSql("Update tb_Dicepay set IsSpier=1 where id=" + pid + "");//机器人标识
        }
        else
            new BCW.BLL.Game.Dicepay().Update(model);

        string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/dice.aspx]挖宝第" + DiceId + "局[/url]押注" + paycent + "" + bzText + "";
        new BCW.BLL.Action().Add(9, 0, 0, "", wText);
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

    //随机得到下注的币数(取整十整百整千整万等)
    private long GetPayCent(int ptype)
    {
        Random rac = new Random(unchecked((int)DateTime.Now.Ticks));
        long paycent = 0;
        if (ptype == 1 || ptype == 2)
        {
            string[] sNum = { "1000", "2000", "5000", "10000", "20000", "50000", "100000", "200000", "300000", "500000" };
            paycent = Convert.ToInt64(sNum[rac.Next(sNum.Length)]);
        }
        else
        {
            string[] sNum = { "500", "1000", "2000", "5000", "10000", "20000" };
            paycent = Convert.ToInt64(sNum[rac.Next(sNum.Length)]);
        }


        return paycent;
    }


    //随机得到下注的类型
    private int GetPtype()
    {
        Random rac = new Random(unchecked((int)DateTime.Now.Ticks));
        int ptype = rac.Next(1, 6);
        if (ptype == 5)
        {
            int rd = new Random().Next(1, 3);
            if (rd == 1)
                ptype = rac.Next(1, 6);
            else
                ptype = rac.Next(1, 5);
        }

        return ptype;
    }

    //根据下注类型得到押注项
    private int GetBuyNum(int Ptype)
    {
        int BuyNum = 0;
        if (Ptype == 1 || Ptype == 2)
        {
            BuyNum = new Random().Next(1, 3);

        }
        else if (Ptype == 3)
        {
            BuyNum = new Random().Next(4, 18);
        }
        else if (Ptype == 4)
        {
            BuyNum = new Random().Next(1, 7);
        }
        else if (Ptype == 4)
        {
            BuyNum = new Random().Next(1, 7);
        }
        else
        {
            BuyNum = new Random().Next(0, 7);
        }
        return BuyNum;
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
