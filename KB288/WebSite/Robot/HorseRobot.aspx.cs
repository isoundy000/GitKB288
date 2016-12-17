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

public partial class Robot_HorseRobot : System.Web.UI.Page
{
    protected string xmlPath = "/Controls/horse.xml";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (ub.GetSub("HorseIsBot", xmlPath) != "1")
        {
            Response.Write("close");
        }
        else
        {
            ChangePalyHorse();
            Response.Write("ok!");
        }
    }
    /// <summary>
    /// 跑马自动游戏程序
    /// </summary>
    /// <param name="HorseId">局数ID</param>
    /// <param name="dt">截止时间</param>
    private void ChangePalyHorse()
    {
        BCW.Model.Game.Horselist horse = null;
        new BCW.User.Game.Horse().HorsePage();
        horse = new BCW.BLL.Game.Horselist().GetHorselist();
        int HorseId = horse.ID;
        long Sec = DT.DateDiff(horse.EndTime, DateTime.Now, 4);
        if (HorseId > 0)
        {
            int UsIDNum = new BCW.BLL.Game.Horsepay().GetCount(HorseId);//当期下注ID数

            //如果小于15个则自在不同秒数中自动出动1个ID来下注
            if (UsIDNum < 5)
            {
                bool IsPlay = false;
                Sec = 480 - Sec;

                int ZD = 0;
                if (Sec > 5 && Sec <= 30 && Utils.ParseInt(ub.GetSub("HorseZD1", xmlPath)) == 0)
                {
                    IsPlay = true;
                    ZD = 1;
                }
                if (Sec > 30 && Sec <= 60 && Utils.ParseInt(ub.GetSub("HorseZD2", xmlPath)) == 0)
                {
                    IsPlay = true;
                    ZD = 2;
                }
                else if (Sec > 60 && Sec <= 90 && Utils.ParseInt(ub.GetSub("HorseZD3", xmlPath)) == 0)
                {
                    IsPlay = true;
                    ZD = 3;
                }
                else if (Sec > 90 && Sec <= 120 && Utils.ParseInt(ub.GetSub("HorseZD4", xmlPath)) == 0)
                {
                    IsPlay = true;
                    ZD = 4;
                }
                else if (Sec > 120 && Sec <= 150 && Utils.ParseInt(ub.GetSub("HorseZD5", xmlPath)) == 0)
                {
                    IsPlay = true;
                    ZD = 5;
                }
                else if (Sec > 150 && Sec <= 180 && Utils.ParseInt(ub.GetSub("HorseZD6", xmlPath)) == 0)
                {
                    IsPlay = true;
                    ZD = 6;
                }
                else if (Sec > 180 && Sec <= 210 && Utils.ParseInt(ub.GetSub("HorseZD7", xmlPath)) == 0)
                {
                    IsPlay = true;
                    ZD = 7;
                }
                else if (Sec > 210 && Sec <= 240 && Utils.ParseInt(ub.GetSub("HorseZD8", xmlPath)) == 0)
                {
                    IsPlay = true;
                    ZD = 8;
                }
                else if (Sec > 240 && Sec <= 270 && Utils.ParseInt(ub.GetSub("HorseZD9", xmlPath)) == 0)
                {
                    IsPlay = true;
                    ZD = 9;
                }
                else if (Sec > 270 && Sec <= 300 && Utils.ParseInt(ub.GetSub("HorseZD10", xmlPath)) == 0)
                {
                    IsPlay = true;
                    ZD = 10;
                }
                else if (Sec > 300 && Sec <= 330 && Utils.ParseInt(ub.GetSub("HorseZD11", xmlPath)) == 0)
                {
                    IsPlay = true;
                    ZD = 11;
                }
                else if (Sec > 330 && Sec <= 360 && Utils.ParseInt(ub.GetSub("HorseZD12", xmlPath)) == 0)
                {
                    IsPlay = true;
                    ZD = 12;
                }
                else if (Sec > 360 && Sec <= 390 && Utils.ParseInt(ub.GetSub("HorseZD13", xmlPath)) == 0)
                {
                    IsPlay = true;
                    ZD = 13;
                }
                else if (Sec > 390 && Sec <= 420 && Utils.ParseInt(ub.GetSub("HorseZD14", xmlPath)) == 0)
                {
                    IsPlay = true;
                    ZD = 14;
                }
                else if (Sec > 420 && Sec <= 450 && Utils.ParseInt(ub.GetSub("HorseZD15", xmlPath)) == 0)
                {
                    IsPlay = true;
                    ZD = 15;
                }

                if (IsPlay)
                {
                    //更新某分钟已出动过
                    ub xml = new ub();
                    xml.ReloadSub(xmlPath); //加载配置
                    xml.dss["HorseZD" + ZD + ""] = 1;

                    System.IO.File.WriteAllText(HttpContext.Current.Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                    //进行自动下注
                    PlayHorse(HorseId);

                   
                }
            }
        }
    }


    private void PlayHorse(int HorseId)
    {
        int meid = GetUsID();
        ChanageOnline(meid);

        int ptype = GetPtype();
        long paycent = GetPayCent();

        int bzType = 0;//new Random().Next(0, 2)
        if (bzType == 1)
            paycent = paycent * 5;

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

        BCW.Model.Game.Horselist horse = new BCW.BLL.Game.Horselist().GetHorselist();
        if (horse.ID == HorseId)
        {

            //加总押注额
            if (bzType == 0)
            {
                new BCW.BLL.Game.Horselist().UpdatePool(horse.ID, paycent);
                new BCW.BLL.User().UpdateiGold(meid, mename, -paycent, "跑马押注");
            }
            else
            {
                new BCW.BLL.Game.Horselist().UpdateWinPool(horse.ID, paycent);
                new BCW.BLL.User().UpdateiMoney(meid, mename, -paycent, "跑马押注");
            }
            BCW.Model.Game.Horsepay model = new BCW.Model.Game.Horsepay();
            model.HorseId = horse.ID;
            model.UsID = meid;
            model.UsName = mename;
            model.Types = ptype;
            model.BuyCent = paycent;
            model.WinCent = 0;
            model.AddTime = DateTime.Now;
            model.State = 0;
            model.bzType = bzType;
            if (!new BCW.BLL.Game.Horsepay().Exists(horse.ID, meid, bzType, ptype)){
                int pid=new BCW.BLL.Game.Horsepay().Add(model);
            BCW.Data.SqlHelper.ExecuteSql("Update tb_Horsepay set IsSpier=1 where id=" + pid + "");//机器人标识
}
            else
                new BCW.BLL.Game.Horsepay().Update(model);

            string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/horse.aspx]跑马第" + horse.ID + "局[/url]押注" + paycent + "" + bzText + "";
            new BCW.BLL.Action().Add(10, 0, 0, "", wText);
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

    //随机得到下注的币数(取整十整百整千整万等)
    private long GetPayCent()
    {
        Random rac = new Random(unchecked((int)DateTime.Now.Ticks));
        long paycent = 0;

        string[] sNum = { "100", "1000", "5000", "10000", "200", "500" };
        paycent = Convert.ToInt64(sNum[rac.Next(sNum.Length)]);

        return paycent;
    }


    //随机得到下注的类型
    private int GetPtype()
    {
        Random rac = new Random(unchecked((int)DateTime.Now.Ticks));
        string[] sText = { "1-2", "1-3", "1-4", "1-5", "1-6", "2-3", "2-4", "2-5", "2-6", "3-4", "3-5", "3-6", "4-5", "4-6", "5-6" };
        string iText = sText[rac.Next(sText.Length)];
        int ptype = Convert.ToInt32(iText.Replace("-", ""));

        return ptype;
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
