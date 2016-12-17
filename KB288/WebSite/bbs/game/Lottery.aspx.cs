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
using BCW.Common;

public partial class bbs_game_Lottery : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        ub xml = new ub();
        string xmlPath = "/Controls/lottery.xml";
        HttpContext.Current.Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置

        //维护提示
        if (xml.dss["LotteryStatus"].ToString() == "1")
        {
            Utils.Safe("此游戏");
        }

        Master.Title = "第" + xml.dss["LotteryBenqi"].ToString() + "期抽奖";

        int FreshSec = Utils.ParseInt(xml.dss["LotteryFreshSec"].ToString());
        int FreshMin = Utils.ParseInt(xml.dss["LotteryFreshMin"].ToString());
        //long PayCent = Utils.ParseInt64(xml.dss["LotteryPayCent"].ToString());
        //long OutCent = Utils.ParseInt64(xml.dss["LotteryOutCent"].ToString());

        DateTime FreshSecTime = DateTime.Now.AddDays(-365);
        string strFreshSecTime = xml.dss["LotteryFreshSecTime"].ToString();
        string strFreshMinTime = xml.dss["LotteryFreshMinTime"].ToString();
        if (!string.IsNullOrEmpty(strFreshSecTime))
        {
            FreshSecTime = Convert.ToDateTime(strFreshSecTime);
        }
       
        DateTime FreshMinTime = DateTime.Now;
        if (!string.IsNullOrEmpty(strFreshMinTime))
        {
            FreshMinTime = Convert.ToDateTime(strFreshMinTime);
        }

        if (FreshMinTime >= DateTime.Now)
        {
            Utils.Success("温馨提示", "本期抽奖已经结束，请等待下一轮抽奖...", Utils.getPage("default.aspx"), "1");
        }
        //本局抽奖开始
        if (FreshSecTime > DateTime.Now)
        {
            //验证防刷
            string verify = Utils.GetRequest("verify", "get", 2, @"^[0-9]\d*$", "验证码错误");
            string meverify = new BCW.BLL.User().GetVerifys(meid);
            if (!string.IsNullOrEmpty(meverify))
            {
                if (verify.Equals(meverify))
                {
                    Utils.Error("验证码有误,请返回之前页再进行抽奖", Utils.getPage("default.aspx"));
                }
            }
            //更新验证码
            new BCW.BLL.User().UpdateVerifys(meid, verify);

            Random ra = new Random(unchecked((int)DateTime.Now.Ticks));
            int rdNext = ra.Next(1, 11);
            int winTar = Utils.ParseInt(xml.dss["LotteryWinTar"].ToString());
            if (rdNext > winTar)
            {
                builder.Append("很遗憾，本次你没有抽到奖。。。<br /><a href=\"" + Utils.getPage("default.aspx") + "\">&lt;&lt;返回继续抽奖</a>");
            }
            else
            {

                long PayCent = 0;

                string mename = new BCW.BLL.User().GetUsName(meid);
                string[] sNum = { "1", "2", "3" };
                Random rd = new Random();
                int icb = Convert.ToInt32(sNum[rd.Next(sNum.Length)]);
                if (icb == 1)
                {
                    //object obj = BCW.Data.SqlHelper.GetSingle("SELECT Sum(NodeId) from tb_Action where Types=997");
                    //if (obj != null)
                    //{
                       // PayCent = Convert.ToInt64(obj);
                    //}

                   // if (PayCent >= 60000)
                    //{
                     //   Utils.Error("本次积分抽奖已全部结束！", "");
                    //}
                    //int Bb = new Random().Next(1, 6);
                    //string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在第" + xml.dss["LotteryBenqi"] + "期抽奖活动中抽中" + Bb + "积分！";
                    //new BCW.BLL.Action().Add(997, Bb, 0, "", wText);
                    //new BCW.BLL.User().UpdateiScore(meid, Convert.ToInt64(Bb));
                    //builder.Append("恭喜您抽到" + Bb + "积分！");

builder.Append("很遗憾，本次你没有抽到奖。。。<br /><a href=\"" + Utils.getPage("default.aspx") + "\">&lt;&lt;返回继续抽奖</a>");

                }
                else if (icb == 2)
                {
                    object obj = BCW.Data.SqlHelper.GetSingle("SELECT Sum(NodeId) from tb_Action where Types=999");
                    if (obj != null)
                    {
                        PayCent = Convert.ToInt64(obj);
                    }

                    if (PayCent >= 10000000)
                    {
                        Utils.Error("本次" + ub.Get("SiteBz") + "抽奖已全部结束！", "");
                    }
                    int winMoney = BCW.User.Lottery.GetOddsByArr(BCW.User.Lottery.CreateIntArr());
                    string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在第" + xml.dss["LotteryBenqi"] + "期抽奖活动中抽中" + winMoney + "" + ub.Get("SiteBz") + "！";
                    new BCW.BLL.Action().Add(999, winMoney, 0, "", wText);
                    new BCW.BLL.User().UpdateiGold(meid, Convert.ToInt64(winMoney), "抽奖得到");
                    builder.Append("恭喜您抽到" + winMoney + "" + ub.Get("SiteBz") + "！");
                }
                else
                {
                    object obj = BCW.Data.SqlHelper.GetSingle("SELECT Sum(NodeId) from tb_Action where Types=998");
                    if (obj != null)
                    {
                        PayCent = Convert.ToInt64(obj);
                    }

                    if (PayCent >= 50000000)
                    {
                        Utils.Error("本次" + ub.Get("SiteBz2") + "抽奖已全部结束！", "");
                    }
                    int Bb = new Random().Next(10, 201);
                    string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在第" + xml.dss["LotteryBenqi"] + "期抽奖活动中抽中" + Bb + "" + ub.Get("SiteBz2") + "！";
                    new BCW.BLL.Action().Add(998, Bb, 0, "", wText);
                    new BCW.BLL.User().UpdateiMoney(meid, Convert.ToInt64(Bb), "抽奖得到");
                    builder.Append("恭喜您抽到" + Bb + "" + ub.Get("SiteBz2") + "！");
                }
            }
            if (xml.dss["LotteryContent"] != null)
            {
                builder.Append(Out.Tab("<div>", Out.Hr()));
                builder.Append(Out.SysUBB(xml.dss["LotteryContent"].ToString()));
                builder.Append(Out.Tab("</div>", ""));
            }

            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/bbs/action.aspx?ptype=999&amp;backurl=" + Utils.getPage(0) + "") + "\">查看中奖记录&gt;&gt;</a>");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("/bbs/default.aspx") + "\">我的空间</a>-");
            builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">上级</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            //更新下一期开局时间
            xml.dss["LotteryFreshMinTime"] = DateTime.Now.AddMinutes(FreshMin).ToString();
            //更新下一期抽奖时间(秒)
            xml.dss["LotteryFreshSecTime"] = DateTime.Now.AddMinutes(FreshMin).AddSeconds(FreshSec).ToString();
            xml.dss["LotteryBenqi"] = Utils.ParseInt(xml.dss["LotteryBenqi"].ToString()) + 1;
            System.IO.File.WriteAllText(HttpContext.Current.Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            HttpContext.Current.Application.Remove(xmlPath);//清缓存
            Utils.Success("温馨提示", "本期抽奖已经结束，请等待下一轮抽奖...", Utils.getPage("default.aspx"), "1");
        }

    }
}
