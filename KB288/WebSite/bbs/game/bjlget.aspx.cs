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
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using BCW.Common;
using System.Timers;
using System.Xml;

public partial class bjlget : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string GameName = ub.GetSub("GameName", "/Controls/bjl.xml");//游戏名字
    protected string xmlPath = "/Controls/bjl.xml";
    protected int shouxufei = Convert.ToInt32(ub.GetSub("shouxufei", "/Controls/bjl.xml")); //手续费
    protected long RoomTime2 = Convert.ToInt64(ub.GetSub("baccaratRoomTime2", "/Controls/bjl.xml")); //自动封庄的局数
    protected int Times = int.Parse(ub.GetSub("PokerTimes", "/Controls/bjl.xml"));//开牌时间

    protected void Page_Load(object sender, EventArgs e)
    {
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();

        Open();

        stopwatch.Stop();
        Response.Write("<font color=\"red\">" + "<br/>刷新总耗时:" + stopwatch.Elapsed.TotalSeconds + "秒</font><br/>");
    }


    public void Open()
    {
        if (new BCW.Baccarat.BLL.BJL_Play().Exists())
        {
            //派奖
            DataSet ds = new BCW.Baccarat.BLL.BJL_Play().GetList("*", "HunterPoint='' AND type=0 ORDER BY ID ASC");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                #region
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    int RoomID = int.Parse(ds.Tables[0].Rows[i]["RoomID"].ToString());
                    int Play_Table = int.Parse(ds.Tables[0].Rows[i]["Play_Table"].ToString());
                    int ID = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());

                    //查询是否已开奖
                    BCW.Baccarat.Model.BJL_Play aa = new BCW.Baccarat.BLL.BJL_Play().GetBJL_Play2(ID);
                    if (aa.type == 0)
                    {
                        //根据房间的桌面查第一个下注的时间
                        DateTime Oldbettime = new BCW.Baccarat.BLL.BJL_Play().GetMinBetTime(RoomID, (Play_Table));
                        if (Oldbettime.AddSeconds((Times + 8)) < DateTime.Now)
                        {
                            BCW.Baccarat.Model.BJL_Card card = new BCW.Baccarat.BLL.BJL_Card().GetCardMessage(RoomID, Play_Table);
                            new BCW.Baccarat.BLL.BJL_Play().update_zd("BankerPoker='" + card.BankerPoker + "',HunterPoker='" + card.HunterPoker + "',BankerPoint=" + card.BankerPoint + ",HunterPoint=" + card.HunterPoint + "", "RoomID=" + RoomID + " and Play_Table=" + (Play_Table) + "");
                            //派奖
                            _price(RoomID, Play_Table);
                            Response.Write("已开奖第" + RoomID + "桌第" + Play_Table + "个房间.ok1<br/>");
                        }
                        else
                        {
                            Response.Write("请等待开奖：第" + RoomID + "桌第" + Play_Table + "个房间准备开奖.还有" + DT.DateDiff(Oldbettime.AddSeconds(Times + 8), DateTime.Now, 4) + "秒.ok1<br/>");
                        }
                    }
                }
                #endregion
            }

            //判断最低下注是否低于彩池，如果是，则封庄
            DataSet ds1 = new BCW.Baccarat.BLL.BJL_Room().GetList("*", "Total_Now<LowTotal and state=0");
            if (ds1 != null && ds1.Tables[0].Rows.Count > 0)
            {
                #region
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    int ID = int.Parse(ds1.Tables[0].Rows[i]["ID"].ToString());
                    int UsID = int.Parse(ds1.Tables[0].Rows[i]["UsID"].ToString());
                    long LowTotal = Convert.ToInt64(ds1.Tables[0].Rows[i]["LowTotal"].ToString());
                    long Total_Now = Convert.ToInt64(ds1.Tables[0].Rows[i]["Total_Now"].ToString());
                    if (Total_Now < LowTotal)
                    {
                        new BCW.Baccarat.BLL.BJL_Room().update_zd("state=1", "ID=" + ID + "");
                        if (Total_Now > 0)//退回给庄家
                        {
                            new BCW.BLL.User().UpdateiGold(UsID, new BCW.BLL.User().GetUsName(UsID), Total_Now, "你在" + GameName + "第" + ID + "桌系统自动封庄,系统退还剩余彩池" + Total_Now + ub.Get("SiteBz") + "-标识房间ID" + ID + "");
                            new BCW.BLL.Guest().Add(1, UsID, new BCW.BLL.User().GetUsName(UsID), "你在" + GameName + "第" + ID + "桌系统自动封庄,系统退还剩余彩池" + Total_Now + ub.Get("SiteBz") + ".[url=/bbs/game/bjl.aspx]进入" + GameName + "[/url]");
                        }
                        else
                        {
                            if ((new BCW.BLL.User().GetGold(UsID) + Total_Now) > 0)//够钱扣
                            {
                                new BCW.BLL.User().UpdateiGold(UsID, new BCW.BLL.User().GetUsName(UsID), -Total_Now, "你在" + GameName + "第" + ID + "桌的彩池已低于0,系统自动补扣" + Total_Now + "-标识房间ID" + ID + "");
                                new BCW.BLL.Guest().Add(1, UsID, new BCW.BLL.User().GetUsName(UsID), "你在" + GameName + "第" + ID + "桌的彩池已低于0,系统自动从你账户补扣" + Total_Now + ub.Get("SiteBz") + ".[url=/bbs/game/bjl.aspx]进入" + GameName + "[/url]");
                            }
                            else
                            {
                                BCW.Model.Gameowe owe = new BCW.Model.Gameowe();
                                owe.Types = 1;
                                owe.UsID = UsID;
                                owe.UsName = new BCW.BLL.User().GetUsName(UsID);
                                owe.Content = "你在" + GameName + "第" + ID + "桌的彩池已低于0,你欠下系统的" + (Total_Now + new BCW.BLL.User().GetGold(UsID)) + "" + ub.Get("SiteBz") + ".";
                                owe.OweCent = Total_Now + new BCW.BLL.User().GetGold(UsID);
                                owe.BzType = 12;//百家乐封庄记录type的id
                                owe.EnId = ID;
                                owe.AddTime = DateTime.Now;
                                new BCW.BLL.Gameowe().Add(owe);
                                new BCW.BLL.User().UpdateIsFreeze(UsID, 1);

                                //发送内线
                                string strGuess = "你在" + GameName + "第" + ID + "桌的彩池已低于0,你欠下系统的" + (Total_Now + new BCW.BLL.User().GetGold(UsID)) + "" + ub.Get("SiteBz") + ".[br]根据您的帐户数额,实扣" + new BCW.BLL.User().GetGold(UsID) + "" + ub.Get("SiteBz") + ".[br]您的" + ub.Get("SiteBz") + "不足,系统将您帐户冻结。";
                                new BCW.BLL.Guest().Add(1, UsID, new BCW.BLL.User().GetUsName(UsID), strGuess);
                                string bb = "" + new BCW.BLL.User().GetUsName(UsID) + "(" + UsID + ")在" + GameName + "第" + ID + "桌的彩池已低于0,欠下系统" + Total_Now + new BCW.BLL.User().GetGold(UsID) + "" + ub.Get("SiteBz") + ",系统已自动冻结TA的帐户.";
                                new BCW.BLL.Guest().Add(1, 10086, new BCW.BLL.User().GetUsName(10086), bb);
                            }
                        }
                    }
                }
                #endregion
            }

            //超过玩的局数
            DataSet ds2 = new BCW.Baccarat.BLL.BJL_Room().GetList2("*", "a LEFT JOIN tb_BJL_Play b ON a.UsID=b.UsID AND b.Play_Table>=" + RoomTime2 + " AND a.state=0");
            if (ds2 != null && ds2.Tables[0].Rows.Count > 0)
            {
                #region
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    int ID = int.Parse(ds2.Tables[0].Rows[i]["ID"].ToString());
                    int UsID = int.Parse(ds2.Tables[0].Rows[i]["UsID"].ToString());
                    long LowTotal = Convert.ToInt64(ds2.Tables[0].Rows[i]["LowTotal"].ToString());
                    long Total_Now = Convert.ToInt64(ds2.Tables[0].Rows[i]["Total_Now"].ToString());

                    new BCW.Baccarat.BLL.BJL_Room().update_zd("state=1", "ID=" + ID + "");
                    if (Total_Now > 0)//退回给庄家
                    {
                        new BCW.BLL.User().UpdateiGold(UsID, new BCW.BLL.User().GetUsName(UsID), Total_Now, "你在" + GameName + "第" + ID + "桌的已达到最高局数,系统自动封庄,退还" + Total_Now + "-标识房间ID" + ID + "");
                        new BCW.BLL.Guest().Add(1, UsID, new BCW.BLL.User().GetUsName(UsID), "你在" + GameName + "第" + ID + "桌的已达到最高局数,系统自动封庄,退还" + Total_Now + ub.Get("SiteBz") + ".[url=/bbs/game/bjl.aspx]进入" + GameName + "[/url]");
                    }
                }
                #endregion
            }
        }
        else
        {
            Response.Write("暂无房间需要开奖.ok1");
        }


    }

    private void _price(int RoomID, int Play_Table)
    {
        DataSet ds = new BCW.Baccarat.BLL.BJL_Play().GetList("*", "RoomID=" + RoomID + " and Play_Table=" + Play_Table + " and type=0");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            #region 中奖判断及返奖
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                long get_money = 0;//庄家赔
                long zj = 0;//庄家
                long ss = 0;//手续费
                long tui = 0;//和局退回
                string name = string.Empty;//中奖类型
                string longname = string.Empty;//中奖说明

                int ID = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                //int UsID = int.Parse(ds.Tables[0].Rows[i]["UsID"].ToString());
                //int RoomID = int.Parse(ds.Tables[0].Rows[i]["RoomID"].ToString());
                //int Play_Table = int.Parse(ds.Tables[0].Rows[i]["Play_Table"].ToString());
                int BankerPoint = int.Parse(ds.Tables[0].Rows[i]["BankerPoint"].ToString());
                int HunterPoint = int.Parse(ds.Tables[0].Rows[i]["HunterPoint"].ToString());
                int isRobot = int.Parse(ds.Tables[0].Rows[i]["isRobot"].ToString());
                //int type = int.Parse(ds.Tables[0].Rows[i]["type"].ToString());
                int buy_usid = int.Parse(ds.Tables[0].Rows[i]["buy_usid"].ToString());
                string PutTypes = ds.Tables[0].Rows[i]["PutTypes"].ToString();
                string BankerPoker = ds.Tables[0].Rows[i]["BankerPoker"].ToString();
                string HunterPoker = ds.Tables[0].Rows[i]["HunterPoker"].ToString();
                //DateTime updatetime = Convert.ToDateTime(ds.Tables[0].Rows[i]["updatetime"]);
                //long Total = Convert.ToInt64(ds.Tables[0].Rows[i]["Total"].ToString());
                long zhu_money = Convert.ToInt64(ds.Tables[0].Rows[i]["zhu_money"].ToString());
                //long PutMoney = Convert.ToInt64(ds.Tables[0].Rows[i]["PutMoney"].ToString());
                //long GetMoney = Convert.ToInt64(ds.Tables[0].Rows[i]["GetMoney"].ToString());
                if (BankerPoker != "")
                {
                    string[] hunter = HunterPoker.Split(',');
                    string[] banker = BankerPoker.Split(',');
                    string[] put = PutTypes.Split(',');
                    //庄闲家扑克牌总数
                    int pokerpoint = (banker.Length / 2) + (hunter.Length / 2);
                    string name1 = string.Empty;
                    string[] xiazhu = { "", "庄赢", "闲赢", "和局", "庄单", "庄双", "闲单", "闲双", "投大", "投小", "庄对", "闲对", "任意对", "完美对", };
                    for (int ab1 = 0; ab1 < put.Length; ab1++)
                    {
                        name1 = name1 + (xiazhu[int.Parse(put[ab1])]) + ",";
                    }

                    for (int j = 0; j < put.Length; j++)
                    {
                        #region 中奖判断
                        if (put[j] == "1")//庄赢
                        {
                            #region
                            //类型赔率
                            double percent = betpercent(1);
                            if (BankerPoint > HunterPoint)//赢
                            {
                                //庄家赔
                                long zhuangjia = Convert.ToInt64(zhu_money * percent) - zhu_money;
                                //收税
                                long shoushui = zhuangjia * shouxufei / 100;
                                //玩家所得
                                long wanjia = zhuangjia - shoushui;

                                get_money = get_money + wanjia + zhu_money;
                                zj = zj - zhuangjia;
                                ss = ss + shoushui;
                            }
                            else if (BankerPoint == HunterPoint)//和局退回下注金额，不收税
                            {
                                tui = tui + zhu_money;
                                zj = zj - zhu_money;
                            }
                            else//输
                            {
                                //收税
                                long shoushui = zhu_money * shouxufei / 100;
                                //庄家得
                                long zhuangjia = zhu_money - shoushui;
                                ss = ss + shoushui;
                                zj = zj + zhuangjia;
                            }
                            name = "1";
                            #endregion
                        }
                        else if (put[j] == "2")//闲赢
                        {
                            #region
                            //类型赔率
                            double percent = betpercent(2);
                            if (BankerPoint < HunterPoint)//赢
                            {
                                //庄家赔
                                long zhuangjia = Convert.ToInt64(zhu_money * percent) - zhu_money;
                                //收税
                                long shoushui = zhuangjia * shouxufei / 100;
                                //玩家所得
                                long wanjia = zhuangjia - shoushui;

                                get_money = get_money + wanjia + zhu_money;
                                zj = zj - zhuangjia;
                                ss = ss + shoushui;
                            }
                            else if (BankerPoint == HunterPoint)//和局退回下注金额，不收税
                            {
                                tui = tui + zhu_money;
                                zj = zj - zhu_money;
                            }
                            else//输
                            {
                                //收税
                                long shoushui = zhu_money * shouxufei / 100;
                                //庄家得
                                long zhuangjia = zhu_money - shoushui;
                                ss = ss + shoushui;
                                zj = zj + zhuangjia;
                            }
                            name = name + ",2";
                            #endregion
                        }
                        else if (put[j] == "3")//和局
                        {
                            #region
                            double percent = betpercent(3);
                            if (BankerPoint == HunterPoint)
                            {
                                //庄家赔
                                long zhuangjia = Convert.ToInt64(zhu_money * percent) - zhu_money;
                                //收税
                                long shoushui = zhuangjia * shouxufei / 100;
                                //玩家所得
                                long wanjia = zhuangjia - shoushui;

                                get_money = get_money + wanjia + zhu_money;
                                zj = zj - zhuangjia;
                                ss = ss + shoushui;
                            }
                            else
                            {
                                //收税
                                long shoushui = zhu_money * shouxufei / 100;
                                //庄家得
                                long zhuangjia = zhu_money - shoushui;
                                ss = ss + shoushui;
                                zj = zj + zhuangjia;
                            }
                            name = name + ",3";
                            #endregion
                        }
                        else if (put[j] == "4")//庄单
                        {
                            #region
                            double percent = betpercent(4);
                            if (BankerPoint % 2 != 0)
                            {
                                //庄家赔
                                long zhuangjia = Convert.ToInt64(zhu_money * percent) - zhu_money;
                                //收税
                                long shoushui = zhuangjia * shouxufei / 100;
                                //玩家所得
                                long wanjia = zhuangjia - shoushui;

                                get_money = get_money + wanjia + zhu_money;
                                zj = zj - zhuangjia;
                                ss = ss + shoushui;
                            }
                            else
                            {
                                //收税
                                long shoushui = zhu_money * shouxufei / 100;
                                //庄家得
                                long zhuangjia = zhu_money - shoushui;
                                ss = ss + shoushui;
                                zj = zj + zhuangjia;
                            }
                            name = name + ",4";
                            #endregion
                        }
                        else if (put[j] == "5")//庄双
                        {
                            #region
                            double percent = betpercent(5);
                            if (BankerPoint % 2 == 0)
                            {
                                //庄家赔
                                long zhuangjia = Convert.ToInt64(zhu_money * percent) - zhu_money;
                                //收税
                                long shoushui = zhuangjia * shouxufei / 100;
                                //玩家所得
                                long wanjia = zhuangjia - shoushui;

                                get_money = get_money + wanjia + zhu_money;
                                zj = zj - zhuangjia;
                                ss = ss + shoushui;
                            }
                            else
                            {
                                //收税
                                long shoushui = zhu_money * shouxufei / 100;
                                //庄家得
                                long zhuangjia = zhu_money - shoushui;
                                ss = ss + shoushui;
                                zj = zj + zhuangjia;
                            }
                            name = name + ",5";
                            #endregion
                        }
                        else if (put[j] == "6")//闲单
                        {
                            #region
                            double percent = betpercent(6);
                            if (HunterPoint % 2 != 0)
                            {
                                //庄家赔
                                long zhuangjia = Convert.ToInt64(zhu_money * percent) - zhu_money;
                                //收税
                                long shoushui = zhuangjia * shouxufei / 100;
                                //玩家所得
                                long wanjia = zhuangjia - shoushui;

                                get_money = get_money + wanjia + zhu_money;
                                zj = zj - zhuangjia;
                                ss = ss + shoushui;
                            }
                            else
                            {
                                //收税
                                long shoushui = zhu_money * shouxufei / 100;
                                //庄家得
                                long zhuangjia = zhu_money - shoushui;
                                ss = ss + shoushui;
                                zj = zj + zhuangjia;
                            }
                            name = name + ",6";
                            #endregion
                        }
                        else if (put[j] == "7")//闲双
                        {
                            #region
                            double percent = betpercent(7);
                            if (HunterPoint % 2 == 0)
                            {
                                //庄家赔
                                long zhuangjia = Convert.ToInt64(zhu_money * percent) - zhu_money;
                                //收税
                                long shoushui = zhuangjia * shouxufei / 100;
                                //玩家所得
                                long wanjia = zhuangjia - shoushui;

                                get_money = get_money + wanjia + zhu_money;
                                zj = zj - zhuangjia;
                                ss = ss + shoushui;
                            }
                            else
                            {
                                //收税
                                long shoushui = zhu_money * shouxufei / 100;
                                //庄家得
                                long zhuangjia = zhu_money - shoushui;
                                ss = ss + shoushui;
                                zj = zj + zhuangjia;
                            }
                            name = name + ",7";
                            #endregion
                        }
                        else if (put[j] == "8")//投大
                        {
                            #region
                            double percent = betpercent(8);
                            if (pokerpoint >= 5)
                            {
                                //庄家赔
                                long zhuangjia = Convert.ToInt64(zhu_money * percent) - zhu_money;
                                //收税
                                long shoushui = zhuangjia * shouxufei / 100;
                                //玩家所得
                                long wanjia = zhuangjia - shoushui;

                                get_money = get_money + wanjia + zhu_money;
                                zj = zj - zhuangjia;
                                ss = ss + shoushui;
                            }
                            else
                            {
                                //收税
                                long shoushui = zhu_money * shouxufei / 100;
                                //庄家得
                                long zhuangjia = zhu_money - shoushui;
                                ss = ss + shoushui;
                                zj = zj + zhuangjia;
                            }
                            name = name + ",8";
                            #endregion
                        }
                        else if (put[j] == "9")// 投小
                        {
                            #region
                            double percent = betpercent(9);
                            if (pokerpoint == 4)
                            {
                                //庄家赔
                                long zhuangjia = Convert.ToInt64(zhu_money * percent) - zhu_money;
                                //收税
                                long shoushui = zhuangjia * shouxufei / 100;
                                //玩家所得
                                long wanjia = zhuangjia - shoushui;

                                get_money = get_money + wanjia + zhu_money;
                                zj = zj - zhuangjia;
                                ss = ss + shoushui;
                            }
                            else
                            {
                                //收税
                                long shoushui = zhu_money * shouxufei / 100;
                                //庄家得
                                long zhuangjia = zhu_money - shoushui;
                                ss = ss + shoushui;
                                zj = zj + zhuangjia;
                            }
                            name = name + ",9";
                            #endregion
                        }
                        else if (put[j] == "10")//庄对
                        {
                            #region
                            double percent = betpercent(10);
                            if (PairofPoker(BankerPoker) == 1)
                            {
                                //庄家赔
                                long zhuangjia = Convert.ToInt64(zhu_money * percent) - zhu_money;
                                //收税
                                long shoushui = zhuangjia * shouxufei / 100;
                                //玩家所得
                                long wanjia = zhuangjia - shoushui;

                                get_money = get_money + wanjia + zhu_money;
                                zj = zj - zhuangjia;
                                ss = ss + shoushui;
                            }
                            else
                            {
                                //收税
                                long shoushui = zhu_money * shouxufei / 100;
                                //庄家得
                                long zhuangjia = zhu_money - shoushui;
                                ss = ss + shoushui;
                                zj = zj + zhuangjia;
                            }
                            name = name + ",10";
                            #endregion
                        }
                        else if (put[j] == "11")//闲对
                        {
                            #region
                            double percent = betpercent(11);
                            if (PairofPoker(HunterPoker) == 1)
                            {
                                //庄家赔
                                long zhuangjia = Convert.ToInt64(zhu_money * percent) - zhu_money;
                                //收税
                                long shoushui = zhuangjia * shouxufei / 100;
                                //玩家所得
                                long wanjia = zhuangjia - shoushui;

                                get_money = get_money + wanjia + zhu_money;
                                zj = zj - zhuangjia;
                                ss = ss + shoushui;
                            }
                            else
                            {
                                //收税
                                long shoushui = zhu_money * shouxufei / 100;
                                //庄家得
                                long zhuangjia = zhu_money - shoushui;
                                ss = ss + shoushui;
                                zj = zj + zhuangjia;
                            }
                            name = name + ",11";
                            #endregion
                        }
                        else if (put[j] == "12")//任意对
                        {
                            #region
                            double percent = betpercent(12);
                            if (anypoker(BankerPoker, HunterPoker) == 1)
                            {
                                //庄家赔
                                long zhuangjia = Convert.ToInt64(zhu_money * percent) - zhu_money;
                                //收税
                                long shoushui = zhuangjia * shouxufei / 100;
                                //玩家所得
                                long wanjia = zhuangjia - shoushui;

                                get_money = get_money + wanjia + zhu_money;
                                zj = zj - zhuangjia;
                                ss = ss + shoushui;
                            }
                            else
                            {
                                //收税
                                long shoushui = zhu_money * shouxufei / 100;
                                //庄家得
                                long zhuangjia = zhu_money - shoushui;
                                ss = ss + shoushui;
                                zj = zj + zhuangjia;
                            }
                            name = name + ",12";
                            #endregion
                        }
                        else if (put[j] == "13")//完美对
                        {
                            #region
                            double percent = betpercent(12);
                            if (perfectpoker(BankerPoker, HunterPoker) == 1)
                            {
                                //庄家赔
                                long zhuangjia = Convert.ToInt64(zhu_money * percent) - zhu_money;
                                //收税
                                long shoushui = zhuangjia * shouxufei / 100;
                                //玩家所得
                                long wanjia = zhuangjia - shoushui;

                                get_money = get_money + wanjia + zhu_money;
                                zj = zj - zhuangjia;
                                ss = ss + shoushui;
                            }
                            else
                            {
                                //收税
                                long shoushui = zhu_money * shouxufei / 100;
                                //庄家得
                                long zhuangjia = zhu_money - shoushui;
                                ss = ss + shoushui;
                                zj = zj + zhuangjia;
                            }
                            name = name + ",13";
                            #endregion
                        }
                        #endregion
                    }
                    long hj = zhu_money * put.Length;
                    //Utils.Error("玩家得到：" + get_money + "===手续费：" + ss + "===奖池：" + zj + "===系统退回：" + tui + "", "");
                    new BCW.Baccarat.BLL.BJL_Room().update_zd("Total_Now=Total_Now+" + zj + "-" + hj + "", "ID=" + RoomID + "");//彩池扣除
                    new BCW.Baccarat.BLL.BJL_Room().update_zd("shouxufei=shouxufei+'" + ss + "'", "ID=" + RoomID + "");//手续费
                    new BCW.Baccarat.BLL.BJL_Play().update_zd("shouxufei='" + ss + "'", "ID=" + ID + "");
                    if (tui > 0)//和局退回
                    {
                        //彩池增加
                        new BCW.Baccarat.BLL.BJL_Room().update_zd("Total_Now=Total_Now+" + tui + "", "ID=" + RoomID + "");
                        new BCW.BLL.User().UpdateiGold(buy_usid, new BCW.BLL.User().GetUsName(buy_usid), tui, "在第" + RoomID + "桌的第" + Play_Table + "局出现和局" + BankerPoint + ":" + HunterPoint + ",退还该下注" + tui + "-标识ID" + ID + "");
                        if (isRobot == 0)
                            new BCW.BLL.Guest().Add(1, buy_usid, new BCW.BLL.User().GetUsName(buy_usid), "在" + GameName + "" + RoomID + "桌第" + Play_Table + "局开奖:庄" + BankerPoint + "点|闲" + HunterPoint + "点为和局,退还该下注" + tui + "" + ub.Get("SiteBz") + ".");
                    }
                    if (get_money > 0)//用户中奖
                    {
                        new BCW.Baccarat.BLL.BJL_Play().update_zd("type=2", "ID=" + ID + "");//2为中奖
                        new BCW.Baccarat.BLL.BJL_Play().update_zd("GetMoney=" + get_money + "", "ID=" + ID + "");//加钱
                        if (isRobot == 0)
                            new BCW.BLL.Guest().Add(1, buy_usid, new BCW.BLL.User().GetUsName(buy_usid), "恭喜您中奖！在" + GameName + "第" + RoomID + "桌第" + Play_Table + "局购买:" + name1 + "开奖:庄" + BankerPoint + "点(" + allpoker(BankerPoker) + ")|闲" + HunterPoint + "点(" + allpoker(HunterPoker) + "),返彩" + get_money + "" + ub.Get("SiteBz") + "[url=/bbs/game/bjl.aspx?act=case]马上兑奖[/url]");
                    }
                    else
                        new BCW.Baccarat.BLL.BJL_Play().update_zd("type=1", "ID=" + ID + "");//1为不中奖
                }
            }
            #endregion
        }
    }


    #region 某些判断
    //输出点数和牌
    private string allpoker(string Poker)
    {
        string[] pokersuit = { "", "方块", "梅花", "红桃", "黑桃" };
        string[] pokerrank = { "", "", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };

        string[] poker = Poker.Split(',');
        string cardpoker = "";
        for (int i = 0; i < poker.Length; i++)
        {
            int point = 0;
            point = int.Parse(poker[i]);
            if (i % 2 == 0)
            {
                if (cardpoker == "")
                    cardpoker = pokersuit[point];
                else
                    cardpoker = cardpoker + pokersuit[point];
            }
            else
            {
                if (i == poker.Length - 1)
                    cardpoker = cardpoker + pokerrank[point];
                else
                    cardpoker = cardpoker + pokerrank[point] + ",";
            }
        }
        return cardpoker;
    }

    //判断是否任意对
    private int anypoker(string banker, string hunter)
    {
        string[] pokerrank = { "", "", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
        string[] BankerPoker;
        string[] HunterPoker;
        BankerPoker = banker.Split(',');
        HunterPoker = hunter.Split(',');
        int BankerRank1;
        int BankerRank2;
        int HunterRank1;
        int HunterRank2;
        BankerRank1 = int.Parse(BankerPoker[1]);
        BankerRank2 = int.Parse(BankerPoker[3]);
        HunterRank1 = int.Parse(HunterPoker[1]);
        HunterRank2 = int.Parse(HunterPoker[3]);
        if ((pokerrank[BankerRank1] == pokerrank[BankerRank2]) && (pokerrank[HunterRank1] == pokerrank[HunterRank2]) && (pokerrank[BankerRank1] == pokerrank[HunterRank1]))
            return 1;
        else
            return 0;
    }

    //选择各类型赔率
    private double betpercent(int p)
    {
        double percent = Convert.ToDouble(ub.GetSub("baccaratpercent" + p + "", xmlPath));
        return percent;
    }

    //判断是否为一对
    private int PairofPoker(string poker)
    {
        string[] pokerrank = { "", "", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
        string[] Poker;
        Poker = poker.Split(',');
        int rank1;
        int rank2;
        rank1 = int.Parse(Poker[1]);
        rank2 = int.Parse(Poker[3]);
        if (pokerrank[rank1] == pokerrank[rank2])
            return 1;
        else
            return 0;
    }

    //判断是否完美对
    private int perfectpoker(string banker, string hunter)
    {
        string[] pokerrank = { "", "", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
        string[] pokersuit = { "", "方块", "梅花", "红桃", "黑桃" };
        string[] BankerPoker;
        string[] HunterPoker;
        BankerPoker = banker.Split(',');
        HunterPoker = hunter.Split(',');
        int BankerRank1;
        int BankerRank2;
        int HunterRank1;
        int HunterRank2;
        int BankerSuit1;
        int BankerSuit2;
        int HunterSuit1;
        int HunterSuit2;
        BankerRank1 = int.Parse(BankerPoker[1]);
        BankerRank2 = int.Parse(BankerPoker[3]);
        HunterRank1 = int.Parse(HunterPoker[1]);
        HunterRank2 = int.Parse(HunterPoker[3]);

        BankerSuit1 = int.Parse(BankerPoker[0]);
        BankerSuit2 = int.Parse(BankerPoker[2]);
        HunterSuit1 = int.Parse(HunterPoker[0]);
        HunterSuit2 = int.Parse(HunterPoker[2]);

        string BankerPoker11;
        string BankerPoker12;
        string HunterPoker21;
        string HunterPoker22;
        BankerPoker11 = pokersuit[BankerSuit1] + pokerrank[BankerRank1];
        BankerPoker12 = pokersuit[BankerSuit2] + pokerrank[BankerRank2];
        HunterPoker21 = pokersuit[HunterSuit1] + pokerrank[HunterRank1];
        HunterPoker22 = pokersuit[HunterSuit2] + pokerrank[HunterRank2];

        if ((BankerPoker11 == BankerPoker12) && (HunterPoker21 == HunterPoker22) && (BankerPoker11 == HunterPoker21))
            return 1;
        else
            return 0;
    }
    #endregion


}


