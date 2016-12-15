using System;
using System.Collections.Generic;
using System.Text;
using BCW.Common;
using System.Data;

/// <summary>
/// 20160421德州扑克
/// 2016德州扑克 修改操作记录及大小盲 黄国军20160430.16.18
/// </summary>
namespace BCW.dzpk
{
    public class dzpk
    {
        //加密KEY
        string DzpkKey = "dzpk_1.0";
        protected static string xmlPath = "/Controls/dzpk.xml";

        #region 获得队列最大值编号 getMaxRankChk()
        /// <summary>
        /// 获得队列最大值编号
        /// </summary>
        /// <returns></returns>
        public int getMaxRankChk()
        {
            return new BCW.dzpk.BLL.DzpkPlayRanks().GetMaxId() - 1;
        }
        #endregion

        #region 房间分类 getDRTypeName()
        /// <summary>
        /// 房间分类
        /// </summary>
        /// <param name="drtype">分类编号</param>
        /// <returns></returns>
        public string getDRTypeName(int drtype)
        {
            string xmlPath = "/Controls/dzpk.xml";
            string rstr = "分类出错";

            //设置XML地址
            ub xml = new ub();
            xml.ReloadSub(xmlPath); //加载配置
            if (drtype >= 0)
            {
                string[] RTStr = xml.dss["RmType"].ToString().Split('|');
                for (int i = 0; i < RTStr.Length; i++)
                {
                    if (i == drtype)
                    {
                        rstr = RTStr[i].ToString();
                        break;
                    }
                }
            }
            else
            {
                rstr = xml.dss["RmType"].ToString();
            }
            return rstr;
        }
        #endregion

        #region 加密房间信息 SetRoomID()
        /// <summary>
        /// 加密房间信息
        /// </summary>
        /// <param name="GKeyStr"></param>
        /// <returns></returns>
        public string SetRoomID(string GKeyStr)
        {
            return BCW.Common.DESEncrypt.Encrypt(GKeyStr, DzpkKey);
        }
        #endregion

        #region 解密房间信息 GetRoomID()
        /// <summary>
        /// 解密房间信息
        /// </summary>
        /// <param name="GKeyStr"></param>
        /// <returns></returns>
        public int GetRoomID(string GKeyStr)
        {
            int Rid = -1;
            try
            {
                Rid = int.Parse(BCW.Common.DESEncrypt.Decrypt(GKeyStr, DzpkKey));
            }
            catch { };
            return Rid;
        }
        #endregion

        #region 刷新房间状态 GetOnlineRoom()
        /// <summary>
        /// 刷新房间状态
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public int GetOnlineRoom(BCW.dzpk.Model.DzpkRooms room)
        {

            DataTable DTab_PlayRanks = new BCW.dzpk.BLL.DzpkPlayRanks().GetList("*", "UsID>0 AND Rmid=" + room.ID.ToString() + " ORDER BY TimeOut DESC").Tables[0];

            //离线人数
            int OutGameNum = 0;

            if (DTab_PlayRanks.Rows.Count > 0)
            {

                BCW.dzpk.Model.DzpkPlayRanks MaxTime_Model = new BCW.dzpk.BLL.DzpkPlayRanks().GetDzpkPlayRanks(int.Parse(DTab_PlayRanks.Rows[0]["ID"].ToString()));

                //通过出牌人最大时间*房间最大人数，获得超时时间 CtrlTimeMissMax
                DateTime CtrlTimeMissMax = MaxTime_Model.TimeOut.AddSeconds(room.GSetTime * DTab_PlayRanks.Rows.Count);
                //builder.Append("时间最大值：" + CtrlTimeMissMax.ToString() + " 当前时间：" + DateTime.Now.ToString());
                if (DateTime.Compare(CtrlTimeMissMax, DateTime.Now) < 0)
                {
                    //超时操作，房间将变成空闲
                    OutGameNum = DTab_PlayRanks.Rows.Count;
                }
                //else
                //{
                //    for (int i = 0; i < DTab_PlayRanks.Rows.Count; i++)
                //    {
                //        BCW.dzpk.Model.DzpkPlayRanks dpr_model = new BCW.dzpk.BLL.DzpkPlayRanks().GetDzpkPlayRanks(int.Parse(DTab_PlayRanks.Rows[i]["ID"].ToString()));
                //        //为了计算更精准，循环时，获取每个玩家的全局数据，如已经离线，则减去该玩家的在线统计，后台直接赋值出牌为D，即在下一盘剔除玩家
                //        if (BCW.User.Users.UserOnline(dpr_model.UsID) != "在线") { OutGameNum++; }
                //    }
                //}
            }

            OutGameNum = DTab_PlayRanks.Rows.Count - OutGameNum;

            return OutGameNum;

        }
        #endregion

        #region 返回玩家Model chkPlayerRanks()
        /// <summary>
        /// 查询玩家Model
        /// </summary>
        /// <param name="UsID">玩家ID</param>
        /// <returns>返回玩家Model</returns>
        public BCW.dzpk.Model.DzpkPlayRanks chkPlayerRanks(int UsID)
        {
            BCW.dzpk.Model.DzpkPlayRanks dpr_Model = null;
            try
            {
                DataTable dt = new BCW.dzpk.BLL.DzpkPlayRanks().GetList("ID", "UsID=" + UsID.ToString()).Tables[0];
                if (dt.Rows.Count > 0) { dpr_Model = new BCW.dzpk.BLL.DzpkPlayRanks().GetDzpkPlayRanks(int.Parse(dt.Rows[0]["ID"].ToString())); }
            }
            catch { }
            return dpr_Model;
        }
        #endregion

        #region 离开房间 OutRoom()
        /// <summary>
        /// 剔除玩家
        /// </summary>
        /// <param name="UsID">玩家ID</param>
        public string OutRoom(int UsID)
        {
            string f = UsAction.AC_SUCCESS;
            try
            {
                #region 初始化信息
                //玩家列表
                DataTable dt = new BCW.dzpk.BLL.DzpkPlayRanks().GetList("ID", "UsID=" + UsID.ToString()).Tables[0];
                //当前玩家model
                BCW.dzpk.Model.DzpkPlayRanks cur_Model = new BCW.dzpk.BLL.DzpkPlayRanks().GetDzpkPlayRanks(int.Parse(dt.Rows[0]["ID"].ToString()));
                //历史记录表
                BCW.dzpk.Model.DzpkHistory dhy_model = new BCW.dzpk.Model.DzpkHistory();
                //排序列表
                DataTable dt_Ranks = new BCW.dzpk.BLL.DzpkPlayRanks().GetList("ID", "RmID=" + cur_Model.RmID.ToString() + " Order By RankChk ASC").Tables[0];
                #endregion

                #region 赋值一条新的历史记录
                ///写入历史记录表
                dhy_model.RmID = cur_Model.RmID;
                dhy_model.UsID = cur_Model.UsID;
                dhy_model.RankChk = cur_Model.RankChk;
                dhy_model.PokerCards = cur_Model.PokerCards;
                dhy_model.PokerChips = cur_Model.PokerChips;
                dhy_model.TimeOut = cur_Model.TimeOut;
                dhy_model.RmMake = cur_Model.RmMake;

                //将玩家在房间的币返还到玩家身上
                dhy_model.GetMoney = cur_Model.UsPot;
                dhy_model.IsPayOut = 0;
                dhy_model.Winner = UsAction.US_OUT;
                #endregion

                #region 判断是否为测试模式 写入记录表
                string AC = "";
                if (ub.GetSub("GameStatus", xmlPath) == "2")
                {
                    AC = "(测)" + UsAction.AC_OUTROOM;
                }
                else { AC = UsAction.AC_OUTROOM; }
                dzpk.UpdateDzpkAct(cur_Model, AC, cur_Model.UsPot);
                #endregion

                #region 处理当前出牌者多于2个的情况
                if (dt_Ranks.Rows.Count >= 2)
                {
                    //计数下一个出牌玩家，并更新他的出牌标示                            
                    for (int i = 0; i < dt_Ranks.Rows.Count; i++)
                    {
                        int nextMake = 0;
                        BCW.dzpk.Model.DzpkPlayRanks tmp_Model = new BCW.dzpk.BLL.DzpkPlayRanks().GetDzpkPlayRanks(int.Parse(dt_Ranks.Rows[i]["ID"].ToString()));

                        #region 更新下一轮出牌
                        if (((i + 1) - dt_Ranks.Rows.Count) < 0)
                        {
                            nextMake = i + 1;
                        }
                        //当前玩家位置
                        if (cur_Model.UsID == tmp_Model.UsID)
                        {
                            //更新下一轮出牌者
                            for (int j = nextMake; j < dt_Ranks.Rows.Count; j++)
                            {
                                BCW.dzpk.Model.DzpkPlayRanks update_Model = new BCW.dzpk.BLL.DzpkPlayRanks().GetDzpkPlayRanks(int.Parse(dt_Ranks.Rows[nextMake]["ID"].ToString()));

                                if (UsAction.US_DISCARD != GetUSHandle(update_Model) && UsAction.US_TIMEOUT != GetUSHandle(update_Model))
                                {
                                    //此处判断此轮玩家的下注数是否足够，再判断是否下注完成，完成则跳到下一步
                                    if (cur_Model.RankBanker == UsAction.US_RANKMAKE)
                                    {
                                        update_Model.RankBanker = UsAction.US_RANKMAKE;
                                    }
                                    update_Model.RankMake = UsAction.US_RANKMAKE;
                                    update_Model.TimeOut = DateTime.Now;
                                    new BCW.dzpk.BLL.DzpkPlayRanks().Update(update_Model);
                                    break;
                                }
                            }
                        }
                        #endregion
                    }
                }
                #endregion

                //加入到历史数据
                new BCW.dzpk.BLL.DzpkHistory().Add(dhy_model);
                //处理完成后，剔除玩家
                new BCW.dzpk.BLL.DzpkPlayRanks().Delete(cur_Model.ID);
            }
            catch { f = UsAction.AC_ERROR; }
            return f;
        }
        #endregion

        #region 【游戏过程】 RankMakeProcess
        /// <summary>
        /// 游戏过程
        /// </summary>
        /// <param name="AC"></param>
        /// <param name="Cps"></param>
        /// <param name="CurPlayerRank"></param>
        /// <returns></returns>
        public string RankMakeProcess(string AC, long Cps, BCW.dzpk.Model.DzpkPlayRanks CurPlayerRank, ref bool isNext)
        {
            #region 初始化信息
            string Process = "";
            //获得玩家所在的房间信息            
            BCW.dzpk.Model.DzpkRooms Room_Model = new BCW.dzpk.BLL.DzpkRooms().GetDzpkRooms(CurPlayerRank.RmID);
            //获得全部队列信息
            DataTable dt_Ranks = new BCW.dzpk.BLL.DzpkPlayRanks().GetList("*", "Rmid=" + CurPlayerRank.RmID.ToString() + " ORDER BY RankChk").Tables[0];
            #endregion

            //房间状态为FREE的时候才能进入操作,防止其他玩家同时进行操作
            if (Room_Model.GActBetID == UsAction.US_ROOM_FREE)
            {
                try
                {
                    #region 设置房间为忙碌状态
                    Room_Model.GActBetID = UsAction.US_ROOM_BUSY;
                    new BCW.dzpk.BLL.DzpkRooms().Update(Room_Model);
                    #endregion

                    #region 没有弃牌的人数大于2才继续游戏，否则结算
                    //没有弃牌的人数大于2才继续游戏，否则结算
                    int ChkUsNum = int.Parse(ChkAllMakePlayer(CurPlayerRank).Split('|')[1].ToString());
                    if (Room_Model.GActID < 2) { ChkUsNum = dt_Ranks.Rows.Count; }
                    #endregion

                    //在线人数必须大于2才能开始游戏
                    if (ChkUsNum >= 2)
                    {
                        //判断房间当前顺序
                        switch (Room_Model.GActID)
                        {
                            #region 游戏等待开始阶段 case 0
                            case 0:
                                {
                                    //游戏等待开始阶段
                                    DateTime dtBeginTime = DateTime.Parse(Room_Model.LastTime.ToString()).AddSeconds(Room_Model.GSetTime);
                                    if (DateTime.Compare(dtBeginTime, DateTime.Now) < 0)
                                    {
                                        //洗牌，游戏等待开始
                                        Process = "[ " + ShufflingProcess(Room_Model, dt_Ranks) + " ]";
                                    }
                                }; break;
                            #endregion

                            #region 首轮发牌，大小盲注设定 case 1
                            case 1:
                                {
                                    //首轮发牌，大小盲注设定
                                    //Process = "GActID:" + Room_Model.GActID + "【" + Poker_RoundOne(CurPlayerRank, dt_Ranks, Room_Model) + "】";
                                    Process = "[ " + Poker_RoundOne(CurPlayerRank, dt_Ranks, Room_Model) + " ]";
                                }; break;
                            #endregion

                            #region 对局开始 default
                            default:
                                {
                                    //对局开始
                                    //Process = "GActID:" + Room_Model.GActID + "【" + Poker_ChipRound(AC, Cps, CurPlayerRank, dt_Ranks, Room_Model) + "】";
                                    Process = "[ " + Poker_ChipRound(AC, Cps, CurPlayerRank, dt_Ranks, Room_Model, ref isNext) + " ]";
                                }; break;
                            #endregion
                        }
                    }
                    else
                    {
                        #region 少于2人直接结算
                        if (Room_Model.GActID > 0)
                        {
                            GameCheck(dt_Ranks, CurPlayerRank, Room_Model);
                        }
                        #endregion
                    }
                }
                finally
                {
                    #region 任何时候都需要将房间状态设置为空闲
                    Room_Model.GActBetID = UsAction.US_ROOM_FREE;
                    new BCW.dzpk.BLL.DzpkRooms().Update(Room_Model);
                    #endregion
                }
            }
            return Process;
        }
        #endregion

        #region 第一轮押注大小盲 Poker_RoundOne()
        /// <summary>
        /// 第一轮押注大小盲
        /// </summary>
        /// <param name="Cps"></param>
        /// <param name="CurPlayerRank"></param>
        /// <param name="dt_Ranks"></param>
        /// <param name="Room_Model"></param>
        /// <param name="UsGold"></param>
        /// <returns></returns>
        private string Poker_RoundOne(BCW.dzpk.Model.DzpkPlayRanks CurPlayerRank, DataTable dt_Ranks, BCW.dzpk.Model.DzpkRooms Room_Model)
        {
            string r = "";
            try
            {
                //第一轮自动押注：规则为，庄家不用下注，其次为小盲大盲，系统发牌2张底牌；
                int nextMake = 0;
                long Totle = 0;

                //【房间标记】，R+房间ID_最大ID+（时间）加密
                string TmpMake = "";
                if (ub.GetSub("GameStatus", xmlPath) == "2")
                {
                    TmpMake = "(测)R" + Room_Model.ID.ToString() + "_" + new BCW.dzpk.BLL.DzpkHistory().GetMaxId();
                }
                else
                {
                    TmpMake = "R" + Room_Model.ID.ToString() + "_" + new BCW.dzpk.BLL.DzpkHistory().GetMaxId();
                }

                #region 设置大小盲
                for (int i = 0; i < dt_Ranks.Rows.Count; i++)
                {
                    BCW.dzpk.Model.DzpkPlayRanks dpr_Model = new BCW.dzpk.BLL.DzpkPlayRanks().GetDzpkPlayRanks(int.Parse(dt_Ranks.Rows[i]["ID"].ToString()));

                    #region 复位玩家信息
                    dpr_Model.PokerCards = "";
                    dpr_Model.PokerChips = "";
                    //出牌标记
                    dpr_Model.RankBanker = "";
                    dpr_Model.RankMake = "";
                    dpr_Model.RankChips = "";
                    //房间标记 时间
                    dpr_Model.RmMake = TmpMake;
                    dpr_Model.TimeOut = DateTime.Now;
                    #endregion

                    //如玩家金币不够,则做过牌操作,并剔除出房间
                    if (dpr_Model.UsPot < Room_Model.GMinb)
                    {
                        OutRoom(dpr_Model.UsID);
                        continue;
                    }
                    else
                    {
                        #region 下注大小盲
                        switch (i)
                        {
                            //case 0:
                            //    {
                            //        //第一个为庄家                            
                            //        dpr_Model.PokerChips = "0";//庄家首轮不用下注
                            //        UpdateDzpkAct(dpr_Model, UsAction.AC_MASTER, 0);    //更新用户操作消息
                            //    }; break;
                            //取消庄家的意义,德州扑克庄家没什么真实的意义
                            case 0:
                                {
                                    //第一个为小盲 强行下载小盲
                                    dpr_Model.PokerChips = Room_Model.GSmallb.ToString();
                                    Totle += Room_Model.GSmallb;
                                    dpr_Model.UsPot = dpr_Model.UsPot - Room_Model.GSmallb;
                                    if (((i + 1) - dt_Ranks.Rows.Count) < 0) { nextMake = i + 1; };
                                    Room_Model.LastRank = Room_Model.GSmallb;
                                    new BCW.dzpk.BLL.DzpkRooms().Update(Room_Model);
                                    UpdateDzpkAct(dpr_Model, UsAction.AC_SB, Room_Model.GSmallb);   //更新用户操作消息
                                }; break;
                            case 1:
                                {
                                    //第二个为大盲
                                    dpr_Model.PokerChips = Room_Model.GBigb.ToString();
                                    Totle += Room_Model.GBigb;
                                    dpr_Model.UsPot = dpr_Model.UsPot - Room_Model.GBigb;
                                    if (((i + 1) - dt_Ranks.Rows.Count) < 0) { nextMake = i + 1; };
                                    Room_Model.LastRank = Room_Model.GBigb;
                                    //设置结束点
                                    dpr_Model.RankBanker = UsAction.US_RANKMAKE;
                                    new BCW.dzpk.BLL.DzpkRooms().Update(Room_Model);
                                    UpdateDzpkAct(dpr_Model, UsAction.AC_DB, Room_Model.GBigb);   //更新用户操作消息
                                }; break;
                        }

                        new BCW.dzpk.BLL.DzpkPlayRanks().Update(dpr_Model);
                        #endregion
                    }
                }
                #endregion

                //特殊人数处理,2人一下,首轮发牌,要庄家下注后再发牌,
                if (dt_Ranks.Rows.Count > 1 && dt_Ranks.Rows.Count < 3) { nextMake = 0; }

                //更新下一轮出牌者
                for (int i = nextMake; i < dt_Ranks.Rows.Count; i++)
                {
                    BCW.dzpk.Model.DzpkPlayRanks update_Model = new BCW.dzpk.BLL.DzpkPlayRanks().GetDzpkPlayRanks(int.Parse(dt_Ranks.Rows[nextMake]["ID"].ToString()));
                    if (UsAction.US_DISCARD != GetUSHandle(update_Model) && UsAction.US_TIMEOUT != GetUSHandle(update_Model))
                    {
                        Room_Model.GActID++;
                        update_Model.RankMake = UsAction.US_RANKMAKE;
                        update_Model.TimeOut = DateTime.Now;
                        new BCW.dzpk.BLL.DzpkRooms().Update(Room_Model);
                        new BCW.dzpk.BLL.DzpkPlayRanks().Update(update_Model);
                        break;
                    }
                }

                //第一次发出2张底牌
                PokerOne(dt_Ranks, Room_Model, Totle);
            }
            catch (Exception ex)
            {
                r = ex.ToString();
            }
            return r;
        }
        #endregion

        #region 【玩家更新及下注】Poker_ChipRound()
        /// <summary>
        /// 玩家更新及下注
        /// </summary>
        /// <param name="AC"></param>
        /// <param name="Cps"></param>
        /// <param name="CurPlayerRank"></param>
        /// <param name="dt_Ranks"></param>
        /// <param name="Room_Model"></param>
        /// <returns></returns>
        private string Poker_ChipRound(string AC, long Cps, BCW.dzpk.Model.DzpkPlayRanks CurPlayerRank, DataTable dt_Ranks, BCW.dzpk.Model.DzpkRooms Room_Model, ref bool isNext)
        {
            string r = "";
            //bool isNext = false;        //是否更新到下一个出牌者
            bool isBankerNext = false;  //更新Bank位置,当前循环的标记
            try
            {
                #region 下注 超时 处理 AddCps
                r += AddCps(AC, Cps, CurPlayerRank, dt_Ranks, Room_Model, ref isNext, ref isBankerNext);
                #endregion

                #region 更新下一轮出牌者 UpdateNextPlayer
                if (isNext)
                {
                    r += UpdateNextPlayer(CurPlayerRank, dt_Ranks, Room_Model, isBankerNext, AC, ref isNext);
                }
                #endregion

                #region 发牌 处理
                if (Room_Model.GActID == 4)
                {
                    //第二轮派牌 3只公共牌
                    PokerTwo(dt_Ranks, Room_Model);
                }
                if (Room_Model.GActID == 6)
                {
                    PokerThree(dt_Ranks, Room_Model);
                }

                if (Room_Model.GActID == 8)
                {
                    PokerFour(dt_Ranks, Room_Model);
                }

                if (Room_Model.GActID == 10)
                {
                    PokerFive(dt_Ranks, Room_Model);
                }

                if (Room_Model.GActID == 11)
                {
                    r += GameCheck(dt_Ranks, CurPlayerRank, Room_Model);
                }
                #endregion

            }
            catch (Exception ex)
            {
                r = ex.ToString();
            }
            return r;

        }
        #endregion

        #region 【玩家下注】 AddCps()
        /// <summary>
        /// 玩家下注
        /// </summary>
        /// <param name="AC"></param>
        /// <param name="Cps"></param>
        /// <param name="CurPlayerRank"></param>
        /// <param name="dt_Ranks"></param>
        /// <param name="Room_Model"></param>
        /// <param name="isNext"></param>
        /// <returns></returns>
        private static string AddCps(string AC, long Cps, BCW.dzpk.Model.DzpkPlayRanks CurPlayerRank, DataTable dt_Ranks, BCW.dzpk.Model.DzpkRooms Room_Model, ref bool isNext, ref bool isBankerNext)
        {
            int timeout = 0;
            DateTime UsTimeOut = DateTime.Now;
            //金币名称
            string Cb = "";
            if (ub.GetSub("GameStatus", xmlPath) == "2")
            { Cb = ub.GetSub("DzCoin", xmlPath); }
            else { Cb = ub.Get("SiteBz"); }

            #region 处理当前出牌者
            if (CurPlayerRank.RankMake == UsAction.US_RANKMAKE)
            {
                isNext = isNextUSPot(CurPlayerRank, Room_Model);
                if (!isNext)
                {
                    long RankPot_AC = Room_Model.LastRank - GetChipTotle(CurPlayerRank.PokerChips);
                    #region 超时处理 US_TIMEOUT
                    timeout = int.Parse((Room_Model.GSetTime - DateTime.Now.Subtract(CurPlayerRank.TimeOut).TotalSeconds).ToString("0"));
                    timeout = 0;
                    if (timeout < 0)
                    {
                        if (string.IsNullOrEmpty(CurPlayerRank.PokerChips))
                        {
                            CurPlayerRank.PokerChips += UsAction.US_TIMEOUT;
                        }
                        else
                        {
                            CurPlayerRank.PokerChips += "," + UsAction.US_TIMEOUT;
                        }
                        isNext = true;
                        CurPlayerRank.TimeOutCount++;
                        UpdateDzpkAct(CurPlayerRank, UsAction.AC_TIMEOUT_STR, 0);
                    }
                    #endregion

                    #region 过牌 AC_CHECK
                    //首轮下注，所有未下注的玩家不能过牌，只能跟注或加注，或弃牌
                    if (AC == UsAction.AC_CHECK)
                    {
                        if (!string.IsNullOrEmpty(CurPlayerRank.PokerChips))
                        {
                            if (RankPot_AC >= 0)
                            {
                                if (GetChipTotle(CurPlayerRank.PokerChips) <= 0)
                                {
                                    return "下注必须大于" + RankPot_AC + Cb;
                                }
                                else
                                {
                                    CurPlayerRank.PokerChips += "," + 0;
                                    isNext = true;
                                    UpdateDzpkAct(CurPlayerRank, UsAction.AC_CHECK, 0);
                                }
                            }
                            else
                            {
                                return "下注必须大于" + RankPot_AC + Cb;
                            }
                        }
                        else
                        {
                            return "此轮不能过牌哦";
                        }
                    }
                    #endregion

                    #region 跟注 AC_CALL
                    //跟注，即跟上一个玩家下注的币值
                    if (AC == UsAction.AC_CALL)
                    {
                        if (RankPot_AC == 0)
                        {
                            return "上轮没有玩家加注，你不能跟注哦！";
                        }

                        #region 判断玩家是否足够币 并下注
                        //判断玩家是否足够币
                        if (CurPlayerRank.UsPot <= RankPot_AC)
                        {
                            //币值不够，ALL IN
                            if (string.IsNullOrEmpty(CurPlayerRank.PokerChips))
                            {
                                CurPlayerRank.PokerChips += CurPlayerRank.UsPot;
                            }
                            else
                            {
                                CurPlayerRank.PokerChips += "," + CurPlayerRank.UsPot;
                            }
                            #region 更新操作列表
                            //加入房间奖池
                            Room_Model.GSidePot += CurPlayerRank.UsPot;
                            UpdateDzpkAct(CurPlayerRank, UsAction.AC_CALL, CurPlayerRank.UsPot);
                            CurPlayerRank.UsPot = 0;
                            #endregion
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(CurPlayerRank.PokerChips))
                            {
                                CurPlayerRank.PokerChips += RankPot_AC;
                            }
                            else
                            {
                                CurPlayerRank.PokerChips += "," + RankPot_AC;
                            }
                            //加入房间奖池
                            Room_Model.GSidePot += RankPot_AC;
                            CurPlayerRank.UsPot = CurPlayerRank.UsPot - RankPot_AC;

                            #region 更新操作列表
                            UpdateDzpkAct(CurPlayerRank, UsAction.AC_CALL, RankPot_AC);
                            #endregion
                        }
                        #endregion

                        ///GActID=3时,出牌者和最后加注者都为自己,则表明这是刚发牌时,并不够4个人庄家跟注的情况,复位最后下注数,下轮出牌者可过牌
                        //if (CurPlayerRank.RankBanker == UsAction.US_RANKMAKE && Room_Model.GActID == 3)
                        //{
                        //    Room_Model.LastRank = 0;
                        //}

                        isNext = true;
                    }
                    #endregion

                    #region 加注 AC_RAISE
                    //加注
                    if (AC == UsAction.AC_RAISE)
                    {
                        //加注数大于最少下注数 和少于用户持币数
                        if (RankPot_AC < Cps && Cps <= CurPlayerRank.UsPot)
                        {
                            if (string.IsNullOrEmpty(CurPlayerRank.PokerChips))
                            {
                                CurPlayerRank.PokerChips += Cps;
                            }
                            else
                            {
                                CurPlayerRank.PokerChips += "," + Cps;
                            }
                            Room_Model.GSidePot += Cps;
                            Room_Model.LastRank += Cps - RankPot_AC;
                            CurPlayerRank.UsPot = CurPlayerRank.UsPot - Cps;
                            //更新标识
                            CancelAllMake(dt_Ranks);
                            CurPlayerRank.RankBanker = UsAction.US_RANKMAKE;
                            CurPlayerRank.RankChips = UsAction.US_RANKMAKE;
                            UpdateDzpkAct(CurPlayerRank, UsAction.AC_RAISE, Cps);
                            isNext = true;
                        }
                        else
                        {
                            return "加注数必须大于[" + RankPot_AC.ToString() + "] 少于 [" + CurPlayerRank.UsPot.ToString() + "]哦";
                        }
                    }
                    #endregion

                    #region 弃牌 AC_FOLD
                    if (AC == UsAction.AC_FOLD)
                    {
                        if (string.IsNullOrEmpty(CurPlayerRank.PokerChips))
                        {
                            CurPlayerRank.PokerChips += UsAction.US_DISCARD;
                        }
                        else
                        {
                            CurPlayerRank.PokerChips += "," + UsAction.US_DISCARD;
                        }
                        UpdateDzpkAct(CurPlayerRank, UsAction.AC_FOLD, 0);
                        if (CurPlayerRank.RankBanker == UsAction.US_RANKMAKE)
                        {
                            CurPlayerRank.RankBanker = "";
                            isBankerNext = true;
                        }
                        isNext = true;
                    }
                    #endregion

                    #region 全押 AC_ALLIN
                    if (AC == UsAction.AC_ALLIN)
                    {
                        if (string.IsNullOrEmpty(CurPlayerRank.PokerChips))
                        {
                            CurPlayerRank.PokerChips += CurPlayerRank.UsPot;
                        }
                        else
                        {
                            CurPlayerRank.PokerChips += "," + CurPlayerRank.UsPot;
                        }
                        Room_Model.GSidePot += CurPlayerRank.UsPot;

                        //持有币比最后一家多，则属于加注，否则是跟注，并产生边池
                        if (CurPlayerRank.UsPot > Room_Model.LastRank)
                        {
                            Room_Model.LastRank += CurPlayerRank.UsPot;
                            //更新标识
                            CancelAllMake(dt_Ranks);
                            CurPlayerRank.RankBanker = UsAction.US_RANKMAKE;
                            CurPlayerRank.RankChips = UsAction.US_RANKMAKE;
                            CurPlayerRank.RankMake = UsAction.US_RANKMAKE;
                        }
                        //更新游戏操作记录
                        UpdateDzpkAct(CurPlayerRank, UsAction.AC_ALLIN, CurPlayerRank.UsPot);
                        CurPlayerRank.UsPot = 0;
                        isNext = true;
                    }
                    #endregion

                    new BCW.dzpk.BLL.DzpkRooms().Update(Room_Model);
                    new BCW.dzpk.BLL.DzpkPlayRanks().Update(CurPlayerRank);
                }
            }
            #endregion

            #region 处理飞当前出牌者 全押的情况自动处理下一轮
            else
            {
                isNext = isNextUSAllN(Room_Model);
            }
            #endregion
            return "剩余操作时间:" + timeout + "秒";
        }
        #endregion

        #region 更新下一轮玩家 UpdateNextPlayer()
        /// <summary>
        /// 更新下一轮玩家
        /// </summary>
        /// <param name="CurPlayerRank"></param>
        /// <param name="dt_Ranks"></param>
        /// <param name="Room_Model"></param>
        /// <returns></returns>
        private string UpdateNextPlayer(BCW.dzpk.Model.DzpkPlayRanks CurPlayerRank, DataTable dt_Ranks, BCW.dzpk.Model.DzpkRooms Room_Model, bool isBankerNext, string AC, ref bool isNext)
        {
            #region 玩家少于2个人时，直接进入结算
            if (int.Parse(ChkAllMakePlayer(CurPlayerRank).Split('|')[1].ToString()) <= 1)
            {
                GameCheck(dt_Ranks, CurPlayerRank, Room_Model);
                return "";
            }
            #endregion

            #region 获得下一轮出牌者位置
            int nextMake = 0;   //下一出牌者位置
            int curMake = 0;    //现在出牌者位置

            for (int i = 0; i < dt_Ranks.Rows.Count; i++)
            {
                BCW.dzpk.Model.DzpkPlayRanks Rank_Model = new BCW.dzpk.BLL.DzpkPlayRanks().GetDzpkPlayRanks(int.Parse(dt_Ranks.Rows[i]["ID"].ToString()));
                if (Rank_Model.RankMake == UsAction.US_RANKMAKE)
                {
                    #region 计算下一玩家的标志位置
                    //计算下一玩家的标志位置
                    curMake = i;
                    if ((i + 1) < dt_Ranks.Rows.Count)
                    {
                        nextMake = i + 1;
                    }
                    //清空出牌者标识
                    Rank_Model.RankMake = "";
                    #endregion

                    #region 通过标志位置计算下一轮未有弃牌的玩家标志
                    //通过标志位置计算下一轮未有弃牌的玩家标志
                    for (int j = nextMake; j < dt_Ranks.Rows.Count; j++)
                    {
                        BCW.dzpk.Model.DzpkPlayRanks NextRank_Model = new BCW.dzpk.BLL.DzpkPlayRanks().GetDzpkPlayRanks(int.Parse(dt_Ranks.Rows[j]["ID"].ToString()));

                        if (UsAction.US_DISCARD == GetUSHandle(NextRank_Model) || UsAction.US_TIMEOUT == GetUSHandle(NextRank_Model))
                        {
                            nextMake++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    #endregion

                    #region 获得下一轮玩家,计算是否第一位并没弃牌的玩家标志
                    //超出人数则默认为第一位
                    if (nextMake == dt_Ranks.Rows.Count) { nextMake = 0; }
                    //获得下一轮玩家
                    for (int j = nextMake; j < dt_Ranks.Rows.Count; j++)
                    {
                        BCW.dzpk.Model.DzpkPlayRanks NextRank_Model = new BCW.dzpk.BLL.DzpkPlayRanks().GetDzpkPlayRanks(int.Parse(dt_Ranks.Rows[j]["ID"].ToString()));

                        if (UsAction.US_DISCARD != GetUSHandle(NextRank_Model) && UsAction.US_TIMEOUT != GetUSHandle(NextRank_Model))
                        {
                            nextMake = j;
                            break;
                        }
                    }
                    #endregion

                    #region 如果下一轮是Banker那么则重置玩家加注标识
                    BCW.dzpk.Model.DzpkPlayRanks NRank_Model = new BCW.dzpk.BLL.DzpkPlayRanks().GetDzpkPlayRanks(int.Parse(dt_Ranks.Rows[nextMake]["ID"].ToString()));
                    NRank_Model.RankMake = UsAction.US_RANKMAKE;
                    NRank_Model.TimeOut = DateTime.Now;
                    //保存上一个出牌者
                    new BCW.dzpk.BLL.DzpkPlayRanks().Update(Rank_Model);
                    //如果下一轮是Banker那么则重置玩家加注标识
                    if (NRank_Model.RankBanker == UsAction.US_RANKMAKE)
                    {
                        NRank_Model.RankChips = "";
                        long UChips = 0;
                        //3为第一轮庄家下注，庄家只能跟注或加注
                        if (Room_Model.GActID != 2)
                        {
                            string ChipStr = GetUSHandle(NRank_Model);
                            try
                            {
                                UChips = long.Parse(ChipStr);
                            }
                            catch { }
                            //if (UChips > 0)
                            //    Room_Model.LastRank = Room_Model.LastRank - UChips;
                        }
                        Room_Model.GActID++;
                    }

                    //if (Room_Model.GActID == 3)
                    //{
                    //    if (CurPlayerRank.RankBanker == UsAction.US_RANKMAKE)
                    //    {
                    //        Room_Model.GActID++;
                    //    }
                    //}

                    ///玩家弃牌后,跳到下一家
                    if (isBankerNext)
                    {
                        NRank_Model.RankBanker = UsAction.US_RANKMAKE;
                    }
                    //if ()
                    #endregion

                    //更新下一个出牌者
                    new BCW.dzpk.BLL.DzpkPlayRanks().Update(NRank_Model);
                    break;
                }
            }
            #endregion

            return "【 next:" + nextMake.ToString() + " cur:" + curMake.ToString() + "count:" + dt_Ranks.Rows.Count.ToString() + " 】";
        }
        #endregion

        #region 更新操作数据 UpdateDzpkAct()
        /// <summary>
        /// 更新操作数据
        /// </summary>
        /// <param name="CurPlayerRank"></param>
        /// <param name="Room_Model"></param>
        /// <param name="AC"></param>
        public static void UpdateDzpkAct(BCW.dzpk.Model.DzpkPlayRanks iRank, string AC, long iGOLD)
        {
            BCW.dzpk.Model.DzpkAct DAC = new BCW.dzpk.Model.DzpkAct();
            DAC.RmID = iRank.RmID;
            DAC.UsID = iRank.UsID;
            DAC.ActMake = AC;
            DAC.ActTime = DateTime.Now;
            DAC.RmMake = iRank.RmMake;
            if (AC == UsAction.AC_GETGOLD)
            {
                string[] UsSevenPoker = iRank.PokerCards.Replace("," + UsAction.US_FINISH, "").Split(',');  //用户带公共牌7只
                string[] UsMaxFivePoker = new Card().FiveFromSeven(UsSevenPoker).Split(',');            //用户最大牌型5只
                DAC.MaxCard = iRank.PokerCards + "|" + UsMaxFivePoker[0] + "," + UsMaxFivePoker[1] + "," + UsMaxFivePoker[2] + "," + UsMaxFivePoker[3] + "," + UsMaxFivePoker[4];
            }
            else
            {
                DAC.MaxCard = iRank.PokerCards;
            }
            DAC.Money = iGOLD;
            new BCW.dzpk.BLL.DzpkAct().Add(DAC);
        }
        #endregion

        #region 取消所有玩家出牌标记 CancelAllMake
        /// <summary>
        /// 取消所有玩家出牌标记
        /// </summary>
        /// <param name="dt_Ranks"></param>
        private static void CancelAllMake(DataTable dt_Ranks)
        {
            for (int i = 0; i < dt_Ranks.Rows.Count; i++)
            {
                BCW.dzpk.Model.DzpkPlayRanks dpr_Model = new BCW.dzpk.BLL.DzpkPlayRanks().GetDzpkPlayRanks(int.Parse(dt_Ranks.Rows[i]["ID"].ToString()));
                dpr_Model.RankBanker = "";
                dpr_Model.RankMake = "";
                dpr_Model.RankChips = "";
                new BCW.dzpk.BLL.DzpkPlayRanks().Update(dpr_Model);
            }
        }
        #endregion

        #region 【第一次发牌过程】每个玩家2只底牌，更新房间总奖池，更新游戏轮回数 PokerOne()
        /// <summary>
        /// 【第一次发牌过程】每个玩家2只底牌，更新房间总奖池，更新游戏轮回数
        /// </summary>
        /// <param name="dt_Ranks"></param>
        /// <param name="Room_Model"></param>
        /// <param name="Totle"></param>
        private void PokerOne(DataTable dt_Ranks, BCW.dzpk.Model.DzpkRooms Room_Model, long Totle)
        {
            //发牌到每个玩家
            for (int j = 0; j < dt_Ranks.Rows.Count; j++)
            {
                BCW.dzpk.Model.DzpkPlayRanks Poker_Model = new BCW.dzpk.BLL.DzpkPlayRanks().GetDzpkPlayRanks(int.Parse(dt_Ranks.Rows[j]["ID"].ToString()));
                if (UsAction.US_DISCARD != GetUSHandle(Poker_Model))
                {
                    //发牌返回,每个玩家发2只
                    Poker_Model.PokerCards = PokerDeal(Room_Model, 2);
                    new BCW.dzpk.BLL.DzpkPlayRanks().Update(Poker_Model);
                }
            }
            //完成第二轮，进入第三轮
            Room_Model.GActID++;
            Room_Model.GSidePot = Totle;
            new BCW.dzpk.BLL.DzpkRooms().Update(Room_Model);
        }
        #endregion

        #region【第二次发牌过程】每个玩家3只公共牌 更新游戏轮回数 PokerTwo()
        /// <summary>
        /// 【第二次发牌过程】每个玩家3只公共牌 更新游戏轮回数
        /// </summary>
        /// <param name="dt_Ranks"></param>
        /// <param name="Room_Model"></param>
        private void PokerTwo(DataTable dt_Ranks, BCW.dzpk.Model.DzpkRooms Room_Model)
        {
            //公共牌每人发3只
            string DealCards = PokerDeal(Room_Model, 3);

            //发牌到每个玩家,弃牌的除外
            for (int j = 0; j < dt_Ranks.Rows.Count; j++)
            {
                BCW.dzpk.Model.DzpkPlayRanks Poker_Model = new BCW.dzpk.BLL.DzpkPlayRanks().GetDzpkPlayRanks(int.Parse(dt_Ranks.Rows[j]["ID"].ToString()));
                if (UsAction.US_DISCARD != GetUSHandle(Poker_Model) && UsAction.US_TIMEOUT != GetUSHandle(Poker_Model))
                {
                    //发牌返回，暂未完成
                    Poker_Model.PokerCards += "," + DealCards;
                    new BCW.dzpk.BLL.DzpkPlayRanks().Update(Poker_Model);
                }
            }
            //完成第三轮，进入第四轮
            Room_Model.GActID++;
            new BCW.dzpk.BLL.DzpkRooms().Update(Room_Model);
        }
        #endregion

        #region【第三次发牌过程】每个玩家1只公共牌，更新游戏轮回数 PokerThree()
        /// <summary>
        /// 【第三次发牌过程】每个玩家1只公共牌，更新游戏轮回数
        /// </summary>
        /// <param name="dt_Ranks"></param>
        /// <param name="Room_Model"></param>
        private void PokerThree(DataTable dt_Ranks, BCW.dzpk.Model.DzpkRooms Room_Model)
        {
            //公共牌每人发1只
            string DealCards = PokerDeal(Room_Model, 1);

            //发牌到每个玩家,弃牌的除外
            for (int j = 0; j < dt_Ranks.Rows.Count; j++)
            {
                BCW.dzpk.Model.DzpkPlayRanks Poker_Model = new BCW.dzpk.BLL.DzpkPlayRanks().GetDzpkPlayRanks(int.Parse(dt_Ranks.Rows[j]["ID"].ToString()));
                if (UsAction.US_DISCARD != GetUSHandle(Poker_Model) && UsAction.US_TIMEOUT != GetUSHandle(Poker_Model))
                {
                    //发牌返回，暂未完成
                    Poker_Model.PokerCards += "," + DealCards;
                    new BCW.dzpk.BLL.DzpkPlayRanks().Update(Poker_Model);
                }
            }
            //完成第三轮，进入第四轮
            Room_Model.GActID++;
            new BCW.dzpk.BLL.DzpkRooms().Update(Room_Model);
        }
        #endregion

        #region【第四次发牌过程】每个玩家1只公共牌，更新游戏轮回数 PokerFour()
        /// <summary>
        /// 【第四次发牌过程】每个玩家1只公共牌，更新游戏轮回数
        /// </summary>
        /// <param name="dt_Ranks"></param>
        /// <param name="Room_Model"></param>
        private void PokerFour(DataTable dt_Ranks, BCW.dzpk.Model.DzpkRooms Room_Model)
        {
            //公共牌每人发1只
            string DealCards = PokerDeal(Room_Model, 1);

            //发牌到每个玩家,弃牌的除外
            for (int j = 0; j < dt_Ranks.Rows.Count; j++)
            {
                BCW.dzpk.Model.DzpkPlayRanks Poker_Model = new BCW.dzpk.BLL.DzpkPlayRanks().GetDzpkPlayRanks(int.Parse(dt_Ranks.Rows[j]["ID"].ToString()));
                if (UsAction.US_DISCARD != GetUSHandle(Poker_Model) && UsAction.US_TIMEOUT != GetUSHandle(Poker_Model))
                {
                    //发牌返回，暂未完成
                    Poker_Model.PokerCards += "," + DealCards;
                    new BCW.dzpk.BLL.DzpkPlayRanks().Update(Poker_Model);
                }
            }
            //完成第四轮，进入第五轮
            Room_Model.GActID++;
            new BCW.dzpk.BLL.DzpkRooms().Update(Room_Model);
        }
        #endregion

        #region【全部下注完成】每个玩家发送完成标记 PokerFive()
        /// <summary>
        /// 每个玩家发送完成标记
        /// </summary>
        /// <param name="dt_Ranks"></param>
        /// <param name="Room_Model"></param>
        private static void PokerFive(DataTable dt_Ranks, BCW.dzpk.Model.DzpkRooms Room_Model)
        {
            //发牌到每个玩家,弃牌的除外
            for (int j = 0; j < dt_Ranks.Rows.Count; j++)
            {
                BCW.dzpk.Model.DzpkPlayRanks Poker_Model = new BCW.dzpk.BLL.DzpkPlayRanks().GetDzpkPlayRanks(int.Parse(dt_Ranks.Rows[j]["ID"].ToString()));
                if (UsAction.US_DISCARD != GetUSHandle(Poker_Model) && UsAction.US_TIMEOUT != GetUSHandle(Poker_Model))
                {
                    if (UsAction.US_FINISH != GetPokerHandle(Poker_Model))
                    {
                        //发牌Z，代表结算的玩家,有Z的才核算
                        Poker_Model.PokerCards += "," + UsAction.US_FINISH;
                        new BCW.dzpk.BLL.DzpkPlayRanks().Update(Poker_Model);
                    }
                }
            }
            //最后一轮完成
            Room_Model.GActID++;
            new BCW.dzpk.BLL.DzpkRooms().Update(Room_Model);
        }
        #endregion

        #region 检查剩余未弃牌的玩家数量 ChkAllMakePlayer()
        /// <summary>
        /// 检查剩余未弃牌的玩家数量
        /// </summary>
        /// <param name="CurPlayerRank"></param>
        /// <returns></returns>
        private string ChkAllMakePlayer(BCW.dzpk.Model.DzpkPlayRanks CurPlayerRank)
        {
            string s = "0";
            int r = 0;
            //获得全部队列信息
            DataTable dt_Ranks = new BCW.dzpk.BLL.DzpkPlayRanks().GetList("*", "Rmid=" + CurPlayerRank.RmID.ToString() + " ORDER BY RankChk").Tables[0];
            for (int i = 0; i < dt_Ranks.Rows.Count; i++)
            {
                BCW.dzpk.Model.DzpkPlayRanks Poker_Model = new BCW.dzpk.BLL.DzpkPlayRanks().GetDzpkPlayRanks(int.Parse(dt_Ranks.Rows[i]["ID"].ToString()));
                if (UsAction.US_DISCARD != GetUSHandle(Poker_Model) && UsAction.US_TIMEOUT != GetUSHandle(Poker_Model))
                {
                    if (r == 0) { s = Poker_Model.ID.ToString(); };
                    r++;
                }
            }
            return s + "|" + r.ToString();
        }
        #endregion

        #region 【游戏结算】 GameCheck()
        /// <summary>
        /// 游戏结算
        /// </summary>
        /// <param name="dt_Ranks"></param>
        /// <param name="CurPlayerRank"></param>
        /// <param name="Room_Model"></param>
        /// <param name="Totle"></param>
        /// <returns></returns>
        private string GameCheck(DataTable dt_Ranks, BCW.dzpk.Model.DzpkPlayRanks CurPlayerRank, BCW.dzpk.Model.DzpkRooms Room_Model)
        {
            string r = "";
            try
            {
                int ChkUsNum = int.Parse(ChkAllMakePlayer(CurPlayerRank).Split('|')[1].ToString());

                //赢家列表
                List<Winner> WnList = new List<Winner>();

                #region 计算玩家持牌大小及列表赋值 =>WnList
                for (int i = 0; i < dt_Ranks.Rows.Count; i++)
                {
                    BCW.dzpk.Model.DzpkPlayRanks Rank_Model = new BCW.dzpk.BLL.DzpkPlayRanks().GetDzpkPlayRanks(int.Parse(dt_Ranks.Rows[i]["ID"].ToString()));
                    if (ChkUsNum <= 1)
                    {
                        if (UsAction.US_DISCARD != GetUSHandle(Rank_Model) && UsAction.US_TIMEOUT != GetUSHandle(Rank_Model))
                        {
                            //小于=1时，即证明玩家游戏人数不足，直接分配
                            Rank_Model.UsPot += Convert.ToInt64(Room_Model.GSidePot * Math.Round((decimal)(1000 - Room_Model.GSerCharge) / 1000, 2));    //增加奖池到玩家台面币中
                            if (!Rank_Model.RmMake.Contains("测"))
                            {
                                //测试版记录扣除的手续费
                                Room_Model.GSerChargeALL += Convert.ToInt64(Room_Model.GSidePot * Math.Round((decimal)1 / 1000, 2));                        //统计费率到房间记录中
                            }
                            UpdateDzpkHistory(Rank_Model, 1, Room_Model.GSidePot, UsAction.US_WIN);                                                         //更新到用户表
                            Room_Model.GSidePot = 0;                                                                                                        //清空奖池
                            new BCW.dzpk.BLL.DzpkPlayRanks().Update(Rank_Model);                                                                            //更新用户数据
                            new BLL.DzpkRooms().Update(Room_Model);                                                                                         //更新房间数据
                        }
                        else
                        {
                            if (UsAction.US_DISCARD == GetUSHandle(Rank_Model))
                            {
                                //弃牌的玩家
                                UpdateDzpkHistory(Rank_Model, 1, 0, UsAction.US_DISCARD);
                            }
                            if (UsAction.US_TIMEOUT == GetUSHandle(Rank_Model))
                            {
                                //超时的玩家
                                UpdateDzpkHistory(Rank_Model, 1, 0, UsAction.US_TIMEOUT);
                            }
                        }
                    }
                    else
                    {
                        if (UsAction.US_DISCARD != GetUSHandle(Rank_Model) && UsAction.US_TIMEOUT != GetUSHandle(Rank_Model))
                        {
                            //大于1，开始对牌大小
                            if (UsAction.US_FINISH == GetPokerHandle(Rank_Model))
                            {
                                r += SetWinner(WnList, Rank_Model);
                            }
                        }
                    }
                }
                #endregion

                if (ChkUsNum > 1)
                {

                    #region 生成对牌玩家及奖池 =>Rank_List =>iGpot_List
                    //对牌的玩家列表 由投注大小排序
                    List<BCW.dzpk.Model.DzpkPlayRanks> Rank_List = new List<BCW.dzpk.Model.DzpkPlayRanks>();
                    //奖池列表
                    List<GPot> iGpot_List = new List<GPot>();

                    //获得所有未弃牌的玩家
                    for (int i = 0; i < dt_Ranks.Rows.Count; i++)
                    {
                        BCW.dzpk.Model.DzpkPlayRanks Player_Model = new BCW.dzpk.BLL.DzpkPlayRanks().GetDzpkPlayRanks(int.Parse(dt_Ranks.Rows[i]["ID"].ToString()));
                        if (UsAction.US_DISCARD != GetUSHandle(Player_Model) && UsAction.US_TIMEOUT != GetUSHandle(Player_Model))
                        {
                            if (GetChipTotle(Player_Model.PokerChips) > 0)
                            {
                                //加入列表
                                Rank_List.Add(Player_Model);
                            }
                        }
                    }

                    //玩家下注总额排序，获得玩家所在底池位置
                    Rank_List = RankChipsListSort(Rank_List);
                    #endregion

                    #region 统计前的准备数据
                    //r += "<br />";
                    bool NoWinner = true;
                    int Beginid = 0; //开始的标记

                    long maxgp = 0;     //所有对牌的金额
                    long foulgp = 0;    //弃牌的金额
                    //计算对牌的玩家奖池
                    for (int i = 0; i < WnList.Count; i++)
                    {
                        maxgp += WnList[i].Pot;
                    }
                    //获得弃牌金额，给第一个赢家
                    foulgp = Room_Model.GSidePot - maxgp;

                    //赢家的列表，用于后面更新非赢家判断
                    List<Winner> UpdateWinnerList = new List<Winner>();
                    #endregion

                    #region 扣除手续费
                    double SerCharge = (Room_Model.GSerCharge * 0.001);
                    Room_Model.GSerChargeALL += (long)((foulgp + maxgp) * SerCharge);
                    foulgp = (long)(foulgp - (foulgp * SerCharge));
                    maxgp = (long)(maxgp - (maxgp * SerCharge));
                    //return "al:" + (foulgp + maxgp).ToString() + " gc:" + SerCharge.ToString() + " ds:" + Room_Model.GSerChargeALL.ToString() + " fl:" + foulgp + " mp:" + maxgp;
                    #endregion

                    #region 循环判断处理赢家酷币返还

                    do
                    {
                        NoWinner = false;
                        //赢家数
                        int WinNum = 0;
                        List<Winner> tmMaxWinerList = new List<Winner>();

                        #region 获得最大牌的玩家 tmMaxWinerList
                        //循环赢家列表 获得最大牌的玩家
                        for (int i = Beginid; i < WnList.Count; i++)
                        {
                            if (WnList[i].Pot > 0)
                            {
                                if (WinNum == 0)
                                {
                                    Winner wn = new Winner();
                                    wn.wUsID = WnList[i].wUsID;
                                    wn.wRanking = WnList[i].wRanking;
                                    wn.Pot = WnList[i].Pot;
                                    wn.PokerCards = WnList[i].PokerCards;
                                    tmMaxWinerList.Add(wn);
                                    WinNum++;
                                }
                                else
                                {
                                    if (tmMaxWinerList[0].wRanking == WnList[i].wRanking)
                                    {
                                        Winner wn = new Winner();
                                        wn.wUsID = WnList[i].wUsID;
                                        wn.wRanking = WnList[i].wRanking;
                                        wn.Pot = WnList[i].Pot;
                                        wn.PokerCards = WnList[i].PokerCards;
                                        tmMaxWinerList.Add(wn);
                                        WinNum++;
                                    }
                                }
                            }
                        }
                        #endregion

                        #region 生成奖池 iGPot_List
                        //生成新奖池
                        for (int i = 0; i < WnList.Count; i++)
                        {
                            long Rank_ChipTotle = 0;
                            Rank_ChipTotle = WnList[i].Pot;

                            //检查和奖池里面是否有相同的下注数，没有则加入奖池
                            if (Rank_ChipTotle > 0)
                            {
                                int isNullPot = 0;
                                for (int j = 0; j < iGpot_List.Count; j++)
                                {
                                    if (Rank_ChipTotle == iGpot_List[j].Pot)
                                    {
                                        isNullPot++;
                                    }
                                }
                                if (isNullPot == 0)
                                {
                                    GPot iPot = new GPot();
                                    iPot.Pot = Rank_ChipTotle;
                                    iGpot_List.Add(iPot);
                                }
                            }
                        }

                        //奖池排列
                        iGpot_List = GpotListSort(iGpot_List);

                        #endregion

                        long MainPot = iGpot_List[iGpot_List.Count - 1].Pot;    //最小下注
                        int PlayNum = WnList.Count;           //玩家总数                    

                        #region 计算金币反还
                        if (maxgp > 1)
                        {
                            long UsGetPot = 0;      //玩家获得金币数

                            //计算最大牌的玩家
                            for (int i = 0; i < tmMaxWinerList.Count; i++)
                            {

                                int iPro = GetPotNumber(iGpot_List, tmMaxWinerList[i]);             //处于边池的位置 0为主池
                                int iProRank = GetRankNumber(WnList, tmMaxWinerList[i]);            //处于赢家的位置 1为第一个赢家

                                for (int j = 0; j < WnList.Count; j++)
                                {
                                    if (WnList[j].Pot <= iGpot_List[iPro].Pot)
                                    {
                                        UsGetPot += WnList[j].Pot;
                                        WnList[j].Pot = 0;
                                    }
                                    else
                                    {
                                        UsGetPot += iGpot_List[iPro].Pot;
                                        WnList[j].Pot = WnList[j].Pot - iGpot_List[iPro].Pot;
                                    }
                                }

                                long Getfgp = 0;

                                if (Beginid == 0)
                                {
                                    Getfgp = foulgp / tmMaxWinerList.Count;
                                }
                                long Getgp = ((long)(UsGetPot - (UsGetPot * SerCharge))) / tmMaxWinerList.Count;
                                //写入历史记录
                                maxgp = maxgp - Getgp;
                                UpdateDzpkAct(chkPlayerRanks(tmMaxWinerList[i].wUsID), UsAction.AC_GETGOLD, Getgp + Getfgp);
                                UpdateDzpkHistory(chkPlayerRanks(tmMaxWinerList[i].wUsID), 1, Getgp + Getfgp, UsAction.US_WIN);
                                UpdateWinnerList.Add(tmMaxWinerList[i]);
                            }
                            foulgp = 0;
                        }
                        else
                        {
                            //退出循环标志
                            NoWinner = true;
                        }
                        #endregion

                        Beginid++;
                    } while (NoWinner == false);

                    #endregion

                    #region 更新输家到历史
                    //更新输家到历史
                    for (int i = 0; i < dt_Ranks.Rows.Count; i++)
                    {
                        BCW.dzpk.Model.DzpkPlayRanks Rank_Model = new BCW.dzpk.BLL.DzpkPlayRanks().GetDzpkPlayRanks(int.Parse(dt_Ranks.Rows[i]["ID"].ToString()));
                        if (UsAction.US_DISCARD != GetUSHandle(Rank_Model) && UsAction.US_TIMEOUT != GetUSHandle(Rank_Model))
                        {
                            bool isLost = true;
                            for (int j = 0; j < UpdateWinnerList.Count; j++)
                            {
                                if (UpdateWinnerList[j].wUsID == Rank_Model.UsID)
                                {
                                    isLost = false;
                                }
                            }
                            if (isLost)
                            {
                                UpdateDzpkHistory(Rank_Model, 1, 0, UsAction.US_LOST);
                            }
                        }
                        else
                        {
                            if (UsAction.US_DISCARD == GetUSHandle(Rank_Model))
                            {
                                //弃牌的玩家
                                UpdateDzpkHistory(Rank_Model, 1, 0, UsAction.US_DISCARD);
                            }
                            if (UsAction.US_TIMEOUT == GetUSHandle(Rank_Model))
                            {
                                //超时的玩家
                                UpdateDzpkHistory(Rank_Model, 1, 0, UsAction.US_TIMEOUT);
                            }
                        }
                    }
                    #endregion
                }
            }
            finally
            {
                ResetRoom(Room_Model);
            }
            return r;
        }
        #endregion

        #region 获得玩家下注总数 GetChipTotle
        /// <summary>
        /// 获得玩家下注数
        /// </summary>
        /// <param name="Rank"></param>
        /// <returns></returns>
        public static long GetChipTotle(string PokerChips)
        {
            long ChipTotle = 0;
            if (!string.IsNullOrEmpty(PokerChips))
            {
                for (int j = 0; j < PokerChips.Split(',').Length; j++)
                {
                    if (PokerChips.Split(',')[j] != UsAction.US_DISCARD && PokerChips.Split(',')[j] != UsAction.US_TIMEOUT)
                    {
                        ChipTotle += long.Parse(PokerChips.Split(',')[j]);
                    }
                }
            }
            return ChipTotle;
        }
        #endregion

        #region 【更新历史】记录 UpdateDzpkHistory
        /// <summary>
        /// 更新历史记录
        /// </summary>
        /// <param name="iRank"></param>
        /// <param name="IsPayOut"></param>
        /// <param name="iGetMoney"></param>
        /// <param name="iWinner"></param>
        /// <param name="WinPoker">=>赢家的扑克牌</param>
        private void UpdateDzpkHistory(BCW.dzpk.Model.DzpkPlayRanks iRank, int IsPayOut, long iGetMoney, string iWinner)
        {
            BCW.dzpk.Model.DzpkHistory his_Model = new BCW.dzpk.Model.DzpkHistory();
            BCW.dzpk.Model.DzpkRankList dzRank_Model = new BCW.dzpk.Model.DzpkRankList(); //玩家统计
            his_Model.RmID = iRank.RmID;
            his_Model.UsID = iRank.UsID;
            his_Model.RankChk = iRank.RankChk;
            if (UsAction.US_FINISH == GetPokerHandle(iRank))
            {
                string[] UsSevenPoker = iRank.PokerCards.Replace("," + UsAction.US_FINISH, "").Split(',');  //用户带公共牌7只
                string[] UsMaxFivePoker = new Card().FiveFromSeven(UsSevenPoker).Split(',');            //用户最大牌型5只
                his_Model.PokerCards = iRank.PokerCards + "|" + UsMaxFivePoker[0] + "," + UsMaxFivePoker[1] + "," + UsMaxFivePoker[2] + "," + UsMaxFivePoker[3] + "," + UsMaxFivePoker[4];
            }
            else
            {
                his_Model.PokerCards = iRank.PokerCards;
            }
            his_Model.PokerChips = iRank.PokerChips;
            his_Model.TimeOut = iRank.TimeOut;
            his_Model.RmMake = iRank.RmMake;
            his_Model.Winner = iWinner;
            if (IsPayOut == 1)
            {
                iRank.UsPot += iGetMoney;
            }
            his_Model.IsPayOut = IsPayOut;
            his_Model.GetMoney = iGetMoney;
            dzRank_Model.Gtime = iRank.TimeOut;
            dzRank_Model.GetPot = (iGetMoney - GetChipTotle(iRank.PokerChips)); //获得币值，负数为输币
            dzRank_Model.UsID = iRank.UsID;
            dzRank_Model.RmMake = iRank.RmMake;
            new BCW.dzpk.BLL.DzpkRankList().Add(dzRank_Model);
            new BCW.dzpk.BLL.DzpkPlayRanks().Update(iRank);
            new BCW.dzpk.BLL.DzpkHistory().Add(his_Model);
        }
        #endregion

        #region 重置房间 ResetRoom
        /// <summary>
        /// 重置房间
        /// </summary>
        /// <param name="Room_Model"></param>
        public static void ResetRoom(BCW.dzpk.Model.DzpkRooms Room_Model)
        {
            Room_Model.GActID = 0;
            Room_Model.GSidePot = 0;
            Room_Model.GActBetID = UsAction.US_ROOM_FREE;
            Room_Model.LastTime = DateTime.Now;
            new BCW.dzpk.BLL.DzpkRooms().Update(Room_Model);
        }
        #endregion

        #region 获得玩家操作的标识 GetUSHandle
        /// <summary>
        /// 获得玩家操作的标识
        /// </summary>
        /// <param name="Poker_Model"></param>
        /// <returns></returns>
        public static string GetUSHandle(BCW.dzpk.Model.DzpkPlayRanks Rank_Model)
        {
            return Rank_Model.PokerChips.Split(',')[Rank_Model.PokerChips.Split(',').Length - 1].ToString();
        }
        #endregion

        #region 获得未弃牌人数大于2，并且身上还有筹码的人数 isNextUSPot
        /// <summary>
        /// 获得未弃牌人数大于2，并且身上还有筹码的人数
        /// </summary>
        /// <param name="Rank_Model"></param>
        /// <returns>true 更新下一轮</returns>
        public static bool isNextUSPot(BCW.dzpk.Model.DzpkPlayRanks Rank_Model, BCW.dzpk.Model.DzpkRooms Room_Model)
        {
            bool s = false;
            int r = 0;
            if (Rank_Model.RankMake == UsAction.US_RANKMAKE)
            {
                if (Rank_Model.UsPot <= 0)
                {
                    s = true;
                }
            }

            if (!s)
            {
                //获得全部队列信息
                DataTable dt_Ranks = new BCW.dzpk.BLL.DzpkPlayRanks().GetList("*", "Rmid=" + Rank_Model.RmID.ToString() + " ORDER BY RankChk").Tables[0];
                for (int i = 0; i < dt_Ranks.Rows.Count; i++)
                {
                    BCW.dzpk.Model.DzpkPlayRanks Poker_Model = new BCW.dzpk.BLL.DzpkPlayRanks().GetDzpkPlayRanks(int.Parse(dt_Ranks.Rows[i]["ID"].ToString()));
                    if (UsAction.US_DISCARD != GetUSHandle(Poker_Model) && UsAction.US_TIMEOUT != GetUSHandle(Poker_Model))
                    {
                        if (Rank_Model.UsID != Poker_Model.UsID)
                        {
                            if (UsAction.US_RANKMAKE != Poker_Model.RankMake)
                            {
                                if (Poker_Model.UsPot > 0)
                                {
                                    r++;
                                }
                            }
                        }
                    }
                }

                if (r < 1)
                {
                    if (Rank_Model.UsPot > 0 && Room_Model.LastRank > 0)
                    {
                        s = false;
                    }
                    else
                    {
                        s = true;
                    }
                }
            }
            return s;
        }
        #endregion

        #region 检查玩家是否还有超过2个以上有筹码的人 isNextUSAllN
        /// <summary>
        /// 检查玩家是否还有超过2个以上有筹码的人
        /// </summary>
        /// <param name="Rank_Model"></param>
        /// <returns>true 更新下一轮</returns>
        public static bool isNextUSAllN(BCW.dzpk.Model.DzpkRooms Room_Model)
        {
            bool s = true;
            int r = 0;

            //获得全部队列信息
            DataTable dt_Ranks = new BCW.dzpk.BLL.DzpkPlayRanks().GetList("*", "Rmid=" + Room_Model.ID.ToString() + " ORDER BY RankChk").Tables[0];
            for (int i = 0; i < dt_Ranks.Rows.Count; i++)
            {
                BCW.dzpk.Model.DzpkPlayRanks Poker_Model = new BCW.dzpk.BLL.DzpkPlayRanks().GetDzpkPlayRanks(int.Parse(dt_Ranks.Rows[i]["ID"].ToString()));
                if (UsAction.US_DISCARD != GetUSHandle(Poker_Model) && UsAction.US_TIMEOUT != GetUSHandle(Poker_Model))
                {

                    if (Poker_Model.UsPot > 0)
                    {
                        r++;
                    }
                    else
                    {
                        if (Room_Model.LastRank > 0)
                        {
                            r++;
                        }
                    }
                    if (r >= 2)
                    {
                        break;
                    }
                }
            }
            //玩家还有资金并且加注标记金额大于0的个数大于2时,不跳到下一个玩家
            if (r >= 2)
            {
                s = false;
            }
            //当前玩家离线不在时,其他玩家刷新页面也能控制超时
            if (!s)
            {
                for (int i = 0; i < dt_Ranks.Rows.Count; i++)
                {
                    BCW.dzpk.Model.DzpkPlayRanks Poker_Model = new BCW.dzpk.BLL.DzpkPlayRanks().GetDzpkPlayRanks(int.Parse(dt_Ranks.Rows[i]["ID"].ToString()));
                    //处理当前出牌者
                    if (UsAction.US_RANKMAKE == Poker_Model.RankMake)
                    {
                        #region 超时处理 US_TIMEOUT
                        int timeout = 0;
                        timeout = int.Parse((Room_Model.GSetTime - DateTime.Now.Subtract(Poker_Model.TimeOut).TotalSeconds).ToString("0"));
                        timeout = 0;
                        if (timeout < 0)
                        {
                            if (string.IsNullOrEmpty(Poker_Model.PokerChips))
                            {
                                Poker_Model.PokerChips += UsAction.US_TIMEOUT;
                            }
                            else
                            {
                                Poker_Model.PokerChips += "," + UsAction.US_TIMEOUT;
                            }
                            s = true;
                            Poker_Model.TimeOutCount++;
                            UpdateDzpkAct(Poker_Model, UsAction.AC_TIMEOUT_STR, 0);
                            new BCW.dzpk.BLL.DzpkPlayRanks().Update(Poker_Model);
                            break;
                        }
                        #endregion
                    }
                }
            }

            return s;
        }
        #endregion

        #region 获得玩家扑克牌的标识 GetPokerHandle
        /// <summary>
        /// 获得玩家扑克牌的标识
        /// </summary>
        /// <param name="Rank_Model"></param>
        /// <returns></returns>
        private static string GetPokerHandle(BCW.dzpk.Model.DzpkPlayRanks Rank_Model)
        {
            return Rank_Model.PokerCards.Split(',')[Rank_Model.PokerCards.Split(',').Length - 1].ToString();
        }
        #endregion

        #region 【赢家列表】 SetWinner()
        /// <summary>
        /// 获取赢家列表
        /// </summary>
        /// <param name="WinnerList">外部列表</param>
        /// <param name="i">序列</param>
        /// <param name="Ranks">玩家Model</param>
        private string SetWinner(List<Winner> WinnerList, BCW.dzpk.Model.DzpkPlayRanks Ranks)
        {
            string r = "";
            if (GetPokerHandle(Ranks) == UsAction.US_FINISH)
            {
                //获得玩家7只手牌中，最大的5只牌型
                string[] UsSevenPoker = Ranks.PokerCards.Replace("," + UsAction.US_FINISH, "").Split(',');  //用户带公共牌7只
                string[] UsMaxFivePoker = new Card().FiveFromSeven(UsSevenPoker).Split(',');            //用户最大牌型5只

                #region 插入赢家列表，最大为0，其次1,2,3
                int big_sort = 0; int Lost_sort = 0; int Last_sort = 0; string ComCard = "NO";
                Winner wn = new Winner();
                wn.wUsID = Ranks.UsID;
                wn.PokerCards = UsMaxFivePoker;
                wn.Pot = GetChipTotle(Ranks.PokerChips);

                if (wn.Pot > 0)
                {
                    if (WinnerList.Count == 0)
                    {
                        wn.wRanking = 0;
                        WinnerList.Add(wn);
                    }
                    else
                    {
                        for (int i = 0; i < WinnerList.Count; i++)
                        {

                            if (big_sort == 0)
                            {
                                Winner tm = WinnerList[i];
                                //对比牌型大小
                                ComCard = new Card().CompareCard(UsMaxFivePoker, tm.PokerCards);

                                //新牌，比旧牌大
                                if (ComCard == Card.CC_BIG_CARD.ToString())
                                {
                                    wn.wRanking = tm.wRanking;
                                    WinnerList[i].wRanking++;
                                    big_sort++;
                                }
                                if (ComCard == Card.CC_DRAW_CARD.ToString())
                                {
                                    wn.wRanking = tm.wRanking;
                                    Lost_sort++;
                                    break;
                                }
                            }
                            else
                            {
                                WinnerList[i].wRanking++;
                            }
                            Last_sort = WinnerList[i].wRanking;
                        }

                        if (big_sort == 0 && Lost_sort == 0)
                        {
                            Last_sort++;
                            wn.wRanking = Last_sort;
                        }

                        WinnerList.Add(wn);
                    }
                    //重新排序
                    WinnerList = WinRankListSort(WinnerList);
                }
                #endregion
            }
            return r;
        }
        #endregion

        #region 列表排序 WinRankListSort()
        /// <summary>
        /// 列表排序
        /// </summary>
        /// <param name="R"></param>
        public List<Winner> WinRankListSort(List<Winner> winList)
        {
            List<Winner> tmList = winList;
            bool tmSorted = true;
            Winner tmWinner = null;
            int tmLastCount = tmList.Count - 1;
            do
            {
                tmSorted = true;
                for (int i = 0; i < tmLastCount; i++)
                {
                    if ((tmList[i].wRanking > tmList[i + 1].wRanking) || ((tmList[i].wRanking == tmList[i + 1].wRanking) && (tmList[i].wRanking > tmList[i + 1].wRanking)))
                    {
                        tmWinner = tmList[i];
                        tmList[i] = tmList[i + 1];
                        tmList[i + 1] = tmWinner;
                        tmSorted = false;
                    }
                }

            } while (tmSorted == false);

            return tmList;
        }
        #endregion

        #region 用户币值排序 WinPotListSort()
        /// <summary>
        /// 列表排序
        /// </summary>
        /// <param name="R"></param>
        public List<Winner> WinPotListSort(List<Winner> winList)
        {
            List<Winner> tmList = winList;
            bool tmSorted = true;
            Winner tmWinner = null;
            int tmLastCount = tmList.Count - 1;
            do
            {
                tmSorted = true;
                for (int i = 0; i < tmLastCount; i++)
                {
                    if ((tmList[i].Pot < tmList[i + 1].Pot) || ((tmList[i].Pot == tmList[i + 1].Pot) && (tmList[i].Pot < tmList[i + 1].Pot)))
                    {
                        tmWinner = tmList[i];
                        tmList[i] = tmList[i + 1];
                        tmList[i + 1] = tmWinner;
                        tmSorted = false;
                    }
                }

            } while (tmSorted == false);

            return tmList;
        }
        #endregion

        #region 奖池排序 GpotListSort()
        /// <summary>
        /// 奖池排序
        /// </summary>
        /// <param name="iGPot"></param>
        /// <returns></returns>
        public List<GPot> GpotListSort(List<GPot> iGPot)
        {
            List<GPot> tmList = iGPot;
            bool tmSorted = true;
            GPot tmGPot = null;
            int tmLastCount = tmList.Count - 1;
            do
            {
                tmSorted = true;
                for (int i = 0; i < tmLastCount; i++)
                {
                    if ((tmList[i].Pot < tmList[i + 1].Pot) || ((tmList[i].Pot == tmList[i + 1].Pot) && (tmList[i].Pot < tmList[i + 1].Pot)))
                    {
                        tmGPot = tmList[i];
                        tmList[i] = tmList[i + 1];
                        tmList[i + 1] = tmGPot;
                        tmSorted = false;
                    }
                }

            } while (tmSorted == false);

            for (int i = 0; i < tmList.Count; i++)
            {
                tmList[i].ID = i;
            }
            return tmList;
        }
        #endregion

        #region 返回底池位置 GetPotNumber()
        /// <summary>
        /// 返回底池位置
        /// </summary>
        /// <param name="iGPot"></param>
        /// <param name="iWinner"></param>
        /// <returns></returns>
        private int GetPotNumber(List<GPot> iGPot, Winner iWinner)
        {
            int r = -1;
            for (int i = 0; i < iGPot.Count; i++)
            {
                if (iGPot[i].Pot == iWinner.Pot)
                {
                    r = i;
                    break;
                }
            }
            return r;
        }
        #endregion

        #region 返回底池位置 GetPotNumber()
        /// <summary>
        /// 返回底池位置
        /// </summary>
        /// <param name="iGPot"></param>
        /// <param name="iWinner"></param>
        /// <returns></returns>
        private int GetRankNumber(List<Winner> iWinnerList, Winner iWinner)
        {
            int r = -1;
            for (int i = 0; i < iWinnerList.Count; i++)
            {
                if (iWinnerList[i].wRanking == iWinner.wRanking)
                {
                    r = i + 1;
                    break;
                }
            }
            return r;
        }
        #endregion

        #region 玩家总下注排序 ChipsListSort()
        /// <summary>
        /// 奖池排序
        /// </summary>
        /// <param name="iGPot"></param>
        /// <returns></returns>
        public List<BCW.dzpk.Model.DzpkPlayRanks> RankChipsListSort(List<BCW.dzpk.Model.DzpkPlayRanks> iRank)
        {
            List<BCW.dzpk.Model.DzpkPlayRanks> tmList = iRank;
            bool tmSorted = true;
            BCW.dzpk.Model.DzpkPlayRanks tmRank = null;
            int tmLastCount = tmList.Count - 1;
            do
            {
                tmSorted = true;
                for (int i = 0; i < tmLastCount; i++)
                {
                    if ((GetChipTotle(tmList[i].PokerChips) < GetChipTotle(tmList[i + 1].PokerChips)) || ((GetChipTotle(tmList[i].PokerChips) == GetChipTotle(tmList[i + 1].PokerChips)) && (GetChipTotle(tmList[i].PokerChips) < GetChipTotle(tmList[i + 1].PokerChips))))
                    {
                        tmRank = tmList[i];
                        tmList[i] = tmList[i + 1];
                        tmList[i + 1] = tmRank;
                        tmSorted = false;
                    }
                }
            } while (tmSorted == false);
            return tmList;
        }
        #endregion

        #region 【洗牌】 ShufflingProcess()
        /// <summary>
        /// 洗牌
        /// </summary>
        /// <param name="Room_Model">当前房间Model</param>
        /// <returns>返回OK为正常</returns>
        private string ShufflingProcess(BCW.dzpk.Model.DzpkRooms Room_Model, DataTable dt_Ranks)
        {
            string r = "";
            try
            {
                //删除房间扑克
                new BCW.dzpk.BLL.DzpkCard().DeleteByRmID(Room_Model.ID);

                List<Card> cards = new List<Card>();
                //生成一副牌  最细2 最大A
                for (int i = Card.DEUCE; i <= Card.ACE; i++)
                {
                    //方块
                    cards.Add(new Card(Card.DIAMOND, i));
                    //梅花
                    cards.Add(new Card(Card.CLUB, i));
                    //桃心
                    cards.Add(new Card(Card.HEART, i));
                    //葵花
                    cards.Add(new Card(Card.SPADE, i));
                }

                //【打乱】扑克，获得新牌
                List<Card> NewCardslist = GetRandomList(cards);
                int k = 0;

                //加入数据
                foreach (Card d in NewCardslist)
                {
                    BCW.dzpk.Model.DzpkCard dzCard_Model = new BCW.dzpk.Model.DzpkCard();
                    dzCard_Model.ID = k;
                    dzCard_Model.PokerRank = d.getRank();
                    dzCard_Model.PokerSuit = d.getSuit();
                    dzCard_Model.RmID = Room_Model.ID;
                    new BCW.dzpk.BLL.DzpkCard().Add(dzCard_Model);
                    k++;
                }

                //清空币值不够和超时次数大于3次的用户
                for (int i = 0; i < dt_Ranks.Rows.Count; i++)
                {
                    BCW.dzpk.Model.DzpkPlayRanks dpr_Model = new BCW.dzpk.BLL.DzpkPlayRanks().GetDzpkPlayRanks(int.Parse(dt_Ranks.Rows[i]["ID"].ToString()));
                    if (dpr_Model.UsPot < Room_Model.GMinb)
                    {
                        OutRoom(dpr_Model.UsID);
                        //new BCW.dzpk.BLL.DzpkPlayRanks().Delete(dpr_Model.ID);
                    }
                    else if (dpr_Model.TimeOutCount >= 3)
                    {
                        OutRoom(dpr_Model.UsID);
                    }
                }

                //更新新的庄家
                for (int i = 0; i < dt_Ranks.Rows.Count; i++)
                {
                    BCW.dzpk.Model.DzpkPlayRanks dpr_Model = new BCW.dzpk.BLL.DzpkPlayRanks().GetDzpkPlayRanks(int.Parse(dt_Ranks.Rows[i]["ID"].ToString()));
                    if ((i + 1) < dt_Ranks.Rows.Count)
                    {
                        dpr_Model.RankChk = i + 1;
                    }
                    else
                    {
                        dpr_Model.RankChk = 0;
                    }
                    new BCW.dzpk.BLL.DzpkPlayRanks().Update(dpr_Model);
                }

                //更新房间数据
                Room_Model.GActID++;
                new BCW.dzpk.BLL.DzpkRooms().Update(Room_Model);
            }
            catch (Exception ex)
            {
                r = "处理错误" + ex.ToString();
            }

            return r;
        }
        #endregion

        #region 【打乱】 GetRandomList()
        /// <summary>
        /// 打乱
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inputList"></param>
        /// <returns></returns>
        public static List<T> GetRandomList<T>(List<T> inputList)
        {
            //Copy to a array
            T[] copyArray = new T[inputList.Count];
            inputList.CopyTo(copyArray);

            //Add range
            List<T> copyList = new List<T>();
            copyList.AddRange(copyArray);

            //Set outputList and random
            List<T> outputList = new List<T>();
            Random rd = new Random(DateTime.Now.Millisecond);

            while (copyList.Count > 0)
            {
                //Select an index and item
                int rdIndex = rd.Next(0, copyList.Count);
                T remove = copyList[rdIndex];

                //remove it from copyList and add it to output
                copyList.Remove(remove);
                outputList.Add(remove);
            }
            return outputList;
        }
        #endregion

        #region 发牌过程 PokerDeal()
        /// <summary>
        /// 发牌过程
        /// </summary>
        /// <param name="Room_Model">房间Model</param>
        /// <param name="num">获得牌数量</param>
        /// <returns>牌型代号</returns>
        private string PokerDeal(BCW.dzpk.Model.DzpkRooms Room_Model, int num)
        {
            string r = "";
            //获得房间扑克牌列表
            //.GetList("*", "Rmid=" + CurPlayerRank.RmID.ToString() + " ORDER BY RankChk").Tables[0];
            DataTable dt_RoomCardlist = new BCW.dzpk.BLL.DzpkCard().GetList("*", "Rmid=" + Room_Model.ID.ToString() + " ORDER BY ID ASC").Tables[0];
            for (int i = 0; i < dt_RoomCardlist.Rows.Count; i++)
            {
                BCW.dzpk.Model.DzpkCard dc = new BCW.dzpk.BLL.DzpkCard().GetDzpkCard(int.Parse(dt_RoomCardlist.Rows[i]["ID"].ToString()), int.Parse(dt_RoomCardlist.Rows[i]["RmID"].ToString()));
                if (i < num)
                {
                    if (i == 0)
                    {
                        if (dc.PokerRank < 10)
                        {
                            r = dc.PokerSuit.ToString() + "0" + dc.PokerRank.ToString();
                        }
                        else
                        {
                            r = dc.PokerSuit.ToString() + dc.PokerRank.ToString();
                        }
                    }
                    else
                    {
                        if (dc.PokerRank < 10)
                        {
                            r += "," + dc.PokerSuit.ToString() + "0" + dc.PokerRank.ToString();
                        }
                        else
                        {
                            r += "," + dc.PokerSuit.ToString() + dc.PokerRank.ToString();
                        }
                    }
                    new BCW.dzpk.BLL.DzpkCard().Delete(dc.ID, dc.RmID);
                }
            }
            return r;
        }
        #endregion

        #region 在线游戏人数 ShowPlayerCount
        /// <summary>
        /// 在线游戏人数
        /// </summary>
        /// <returns></returns>
        public string ShowPlayerCount()
        {
            //return new BCW.dzpk.BLL.DzpkPlayRanks().GetList("*", "").Tables[0].Rows.Count.ToString();
            int OnlineNum = 0;
            DataSet ds = new BCW.dzpk.BLL.DzpkRooms().GetList("*", "");
            if (ds.Tables.Count > 0)
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        BCW.dzpk.Model.DzpkRooms Room = new BCW.dzpk.BLL.DzpkRooms().GetDzpkRooms(int.Parse(ds.Tables[0].Rows[i]["ID"].ToString()));
                        OnlineNum += GetOnlineRoom(Room);
                    }
                }
            return OnlineNum.ToString();
        }
        #endregion

        #region 总奖池数 ShowRoomPotALL
        /// <summary>
        /// 总奖池数
        /// </summary>
        /// <returns></returns>
        public string ShowRoomPotALL()
        {
            return new BCW.dzpk.BLL.DzpkRooms().GetList("SUM(GSidePot) AS PotALL", "").Tables[0].Rows[0]["PotALL"].ToString();
        }
        #endregion

        #region 今日总盈币 ShowWinPotAll_Today
        /// <summary>
        /// 今日总盈币
        /// </summary>
        /// <returns></returns>
        public string ShowWinPotAll_Today(int GameState)
        {
            List<Winner> DzMasList = ShowDzMaster_Today(GameState);
            long totlepot = 0;
            for (int i = 0; i < DzMasList.Count; i++)
            {
                if (DzMasList[i].Pot > 0)
                {
                    totlepot += DzMasList[i].Pot;
                }
            }
            return totlepot.ToString();
        }
        #endregion

        #region 今日德州达人 ShowDzMaster_Today
        /// <summary>
        /// 今日德州达人
        /// </summary>
        /// <returns></returns>
        public List<Winner> ShowDzMaster_Today(int GameState)
        {
            DateTime B_DATE = DateTime.Parse(DateTime.Today.Date.ToShortDateString() + " 00:00:00");
            DateTime E_DATE = DateTime.Parse(DateTime.Today.Date.ToShortDateString() + " 23:59:59");
            List<Winner> DzMasList = GetDzMasterList(UsAction.US_WIN, B_DATE, E_DATE, string.Empty, GameState);
            return DzMasList;
        }
        #endregion

        #region 达人排行榜 ShowDzList_ALL
        public List<Winner> ShowDzList_ALL(int GameState)
        {
            DateTime B_DATE = DateTime.Parse(DateTime.Today.AddYears(-100).Date.ToShortDateString() + " 00:00:00");
            DateTime E_DATE = DateTime.Parse(DateTime.Today.Date.ToShortDateString() + " 23:59:59");
            List<Winner> DzMasList = GetDzMasterList(UsAction.US_WIN, B_DATE, E_DATE, string.Empty, GameState);
            return DzMasList;
        }
        #endregion

        #region 获得达人列表 GetDzMasterList
        /// <summary>
        /// 获得达人列表
        /// </summary>
        /// <param name="US">指令标志</param>
        /// <param name="B_DATE">开始日期</param>
        /// <param name="E_DATE">结束日期</param>
        /// <param name="UsID">指定用户，输入空值为所有玩家</param>
        /// <returns></returns>
        private List<Winner> GetDzMasterList(string US, DateTime B_DATE, DateTime E_DATE, string UsID, int GameState)
        {
            string whereUsstr = string.Empty;
            if (!string.IsNullOrEmpty(UsID)) { whereUsstr = "UsID=" + UsID + " AND "; }
            if (GameState == 0)
            {
                whereUsstr += " RmMake not like '%(测)%' AND ";
            }
            else
            {
                whereUsstr += " RmMake like '%(测)%' AND ";
            }
            whereUsstr += " (Gtime BETWEEN '" + B_DATE.ToString() + "' AND '" + E_DATE.ToString() + "') GROUP BY UsID ";
            DataTable dt_RankListData = new BCW.dzpk.BLL.DzpkRankList().GetList(" UsID, SUM(GetPot) AS GetPot", whereUsstr).Tables[0];
            List<Winner> DzMasList = new List<Winner>();

            for (int i = 0; i < dt_RankListData.Rows.Count; i++)
            {
                if (long.Parse(dt_RankListData.Rows[i]["GetPot"].ToString()) > 0)
                {
                    Winner tmWn = new Winner();
                    tmWn.Pot = long.Parse(dt_RankListData.Rows[i]["GetPot"].ToString());
                    tmWn.wUsID = int.Parse(dt_RankListData.Rows[i]["UsID"].ToString());
                    DzMasList.Add(tmWn);
                }
            }
            DzMasList = WinPotListSort(DzMasList);
            return DzMasList;
        }
        #endregion

    }

    #region 赢家类 Winner
    /// <summary>
    /// 赢家类
    /// </summary>
    public class Winner
    {
        private int _wUsID;
        private string[] _PokerCards;
        private int _wRanking;
        private long _Pot;

        /// <summary>
        /// WinnerID
        /// </summary>
        public int wUsID
        {
            set { _wUsID = value; }
            get { return _wUsID; }
        }

        /// <summary>
        /// 扑克牌
        /// </summary>
        public string[] PokerCards
        {
            set { _PokerCards = value; }
            get { return _PokerCards; }
        }

        /// <summary>
        /// 赢家排序数字越大，排序越后
        /// </summary>
        public int wRanking
        {
            set { _wRanking = value; }
            get { return _wRanking; }
        }

        /// <summary>
        /// 玩家下注数
        /// </summary>
        public long Pot
        {
            set { _Pot = value; }
            get { return _Pot; }
        }

        public Winner()
        {
        }
    }
    #endregion

    #region 奖池类 GPot
    /// <summary>
    /// 奖池类
    /// </summary>
    public class GPot
    {
        private int _ID;
        private long _Pot;

        /// <summary>
        /// 奖池ID
        /// </summary>
        public int ID
        {
            set { _ID = value; }
            get { return _ID; }
        }

        /// <summary>
        /// 奖池金额
        /// </summary>
        public long Pot
        {
            set { _Pot = value; }
            get { return _Pot; }
        }

        public GPot()
        {
        }
    }
    #endregion

    #region 操作类 UsAction
    /// <summary>
    /// 操作类
    /// </summary>
    public class UsAction
    {
        #region 用户操作标识定义
        /// <summary>
        /// 用户定位标识
        /// </summary>
        public const string US_RANKMAKE = "A";
        /// <summary>
        /// 用户超时下注标识
        /// </summary>
        public const string US_TIMEOUT = "T";
        /// <summary>
        /// 用户弃牌标识
        /// </summary>
        public const string US_DISCARD = "P";
        /// <summary>
        /// 用户完成下注标识
        /// </summary>
        public const string US_FINISH = "Z";
        /// <summary>
        /// 房间忙碌标识
        /// </summary>
        public const string US_ROOM_BUSY = "Z";
        /// <summary>
        /// 房间空闲标识
        /// </summary>
        public const string US_ROOM_FREE = "F";
        /// <summary>
        /// 金币标识
        /// </summary>
        public const string US_GOLD = "G";
        /// <summary>
        /// 胜
        /// </summary>
        public const string US_WIN = "W";
        /// <summary>
        /// 输
        /// </summary>
        public const string US_LOST = "L";
        /// <summary>
        /// 离开房间
        /// </summary>
        public const string US_OUT = "O";
        #endregion

        #region 押注操作定义
        /// <summary>
        /// 庄家
        /// </summary>
        public const string AC_MASTER = "庄家";

        /// <summary>
        /// 小盲
        /// </summary>
        public const string AC_SB = "小盲";

        /// <summary>
        /// 大盲
        /// </summary>
        public const string AC_DB = "大盲";

        /// <summary>
        /// 弃牌
        /// </summary>
        public const string AC_FOLD = "弃牌";
        /// <summary>
        /// 过牌
        /// </summary>
        public const string AC_CHECK = "过牌";
        /// <summary>
        /// 加注
        /// </summary>
        public const string AC_RAISE = "加注";
        /// <summary>
        /// 跟注
        /// </summary>
        public const string AC_CALL = "跟注";
        /// <summary>
        /// 全押
        /// </summary>
        public const string AC_ALLIN = "全押";
        /// <summary>
        /// 赢牌提示
        /// </summary>
        public const string AC_GETGOLD = "获得";
        /// <summary>
        /// 输牌提示
        /// </summary>
        public const string AC_LOSTGOLD = "丢失";
        /// <summary>
        /// 超时操作
        /// </summary>
        public const string AC_TIMEOUT_STR = "超时";
        /// <summary>
        /// 入房提示
        /// </summary>
        public const string AC_INROOM = "进入房间";
        /// <summary>
        /// 成功标识
        /// </summary>
        public const string AC_SUCCESS = "OK";
        /// <summary>
        /// 失败标识
        /// </summary>
        public const string AC_ERROR = "ERR";
        /// <summary>
        /// 离开房间提示
        /// </summary>
        public const string AC_OUTROOM = "离开房间";
        /// <summary>
        /// 输出所有操作 1.过牌 2.跟注 3.加注 4.弃牌 5.全押
        /// </summary>
        /// <returns></returns>
        public static string toACstring(long lastRank, long UsPot)
        {
            ///等于0时才可以过牌,大于零只能跟注或加注弃牌等
            if (lastRank <= 0)
            {
                return AC_CHECK + "|" + AC_CALL + "|" + AC_RAISE + "|" + AC_FOLD + "|" + AC_ALLIN;
            }
            else
            {
                string str = "";
                if (lastRank - UsPot < 0)
                {
                    str = AC_CALL + "|" + AC_RAISE + "|";
                }
                return str + AC_FOLD + "|" + AC_ALLIN;
            }
        }
        /// <summary>
        /// 输出所有操作的按钮颜色 1.过牌 2.跟注 3.加注 4.弃牌 5.全押
        /// </summary>
        /// <returns></returns>
        public static string toACcolor(long lastRank, long UsPot)
        {
            ///等于0时才可以过牌,大于零只能跟注或加注弃牌等
            if (lastRank <= 0)
            {
                return "blue|blue|blue|red|red";
            }
            else
            {
                string str = "";
                if (lastRank - UsPot < 0)
                {
                    str = "blue|blue|";
                }
                return str + "red|red";
            }
        }
        #endregion
    }
    #endregion

    #region 扑克牌类 Card
    /// <summary>
    /// 扑克牌类 ACE A,最小DEUCE 2
    /// </summary>
    public class Card
    {
        #region 扑克牌定义变量
        /// <summary>
        /// 方块(钻石)
        /// </summary>
        public const int DIAMOND = 1;  // 方块(钻石)
        /// <summary>
        /// 梅花
        /// </summary>
        public const int CLUB = 2;     // 梅花
        /// <summary>
        /// 红桃(红心)
        /// </summary>
        public const int HEART = 3;    // 红桃(红心)
        /// <summary>
        /// 黑桃(花锄)
        /// </summary>
        public const int SPADE = 4;    // 黑桃(花锄)
        //public static  int JOKER = 4; // 王

        public const int DEUCE = 2;    // 2
        public const int THREE = 3;    // 3
        public const int FOUR = 4;     // 4
        public const int FIVE = 5;     // 5
        public const int SIX = 6;      // 6
        public const int SEVEN = 7;    // 7
        public const int EIGHT = 8;    // 8
        public const int NINE = 9;     // 9
        public const int TEN = 10;      // 10
        public const int JACK = 11;     // J
        public const int QUEEN = 12;   // Q
        public const int KING = 13;    // K
        public const int ACE = 14;     // A
        #endregion

        #region 扑克牌类型定义
        //public  static int BLACK = 13;     // 小王
        //public  static int COLOR = 14;     // 大王
        /// <summary>
        /// 错误类型
        /// </summary>
        public const int CT_ERROR = 0;
        /// <summary>
        /// 单牌类型*
        /// </summary>
        public const int CT_SINGLE = 1;
        /// <summary>
        /// 对子类型*
        /// </summary>
        public const int CT_ONE_LONG = 2;
        /// <summary>
        /// 两对类型*
        /// </summary>
        public const int CT_TWO_LONG = 3;
        /// <summary>
        /// 三条类型*
        /// </summary>
        public const int CT_THREE_TIAO = 4;
        /// <summary>
        /// 顺子类型*
        /// </summary>
        public const int CT_SHUN_ZI = 5;
        /// <summary>
        /// 同花类型*
        /// </summary>
        public const int CT_TONG_HUA = 6;
        /// <summary>
        /// 葫芦类型
        /// </summary>
        public const int CT_HU_LU = 7;
        /// <summary>
        /// 铁支类型*
        /// </summary>
        public const int CT_TIE_ZHI = 8;
        /// <summary>
        /// 同花顺型*
        /// </summary>
        public const int CT_TONG_HUA_SHUN = 9;
        /// <summary>
        /// 皇家同花顺*
        /// </summary>
        public const int CT_KING_TONG_HUA_SHUN = 10;

        /// <summary>
        /// 大
        /// </summary>
        public const int CC_BIG_CARD = 2;

        /// <summary>
        /// 小
        /// </summary>
        public const int CC_SMALL_CARD = 1;

        /// <summary>
        /// 平
        /// </summary>
        public const int CC_DRAW_CARD = 0;
        #endregion

        #region 一些数据定义
        /** 花色 1代表方块, 2代表梅花, 3代表红桃, 4代表黑桃,4:王 */
        private int suit;
        /** 点数 规定: 2代表2, 3代表3, 4代表4,... */
        private int rank;

        public Card()
        {
        }

        public Card(int suit, int rank)
        {
            setRank(rank);
            setSuit(suit);
        }

        public int getSuit()
        {
            return suit;
        }

        public void setSuit(int suit)
        {
            if (suit < DIAMOND || suit > SPADE)
                throw new Exception("花色超过范围!");
            this.suit = suit;
        }

        public int getRank()
        {
            return rank;
        }

        public void setRank(int rank)
        {
            if (rank < DEUCE || rank > ACE)
            {
                throw new Exception("点数超过范围!");
            }
            this.rank = rank;
        }

        private static String[] RANK_NAMES = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
        private static String[] SUIT_NAMES = { "方块", "梅花", "红桃", "黑桃", "" };
        public static String[] TYPE_NAMES = { "错误类型", "高牌", "一对", "两对", "三条", "顺子", "同花", "葫芦", "四条", "同花顺", "皇家同花顺" };

        // 覆盖Object 类的toStirng() 方法. 实现对象的文本描述
        public String toString()
        {
            return SUIT_NAMES[suit] + RANK_NAMES[rank];
        }

        public static String toTypeName(int n)
        {
            return TYPE_NAMES[n];
        }

        #endregion

        #region 输出图片Html toHtml()
        /// <summary>
        /// 输出图片Html
        /// </summary>
        /// <param name="sr"></param>
        /// <returns></returns>
        public static String toHtml(string sr)
        {
            //设置XML地址
            ub xml = new ub();
            string xmlPath = "/Controls/dzpk.xml";
            xml.ReloadSub(xmlPath); //加载配置
            string img = @"<img src='../../" + xml.dss["Dzpk_img"] + sr + ".jpg' alt='load' />";
            return img;
        }
        #endregion

        #region 获取点数 GetRank
        /// <summary>
        /// 获取点数 
        /// </summary>
        /// <param name="rank"></param>
        /// <returns></returns>
        public static String GetRank(string rank)
        {
            string r = "";
            if ((rank.Length - 1) < 2)
            {
                r = "0";
            }
            r += rank.Substring(1, rank.Length - 1);
            return r;
        }
        #endregion

        #region  获取花色 GetSuit() 1方块，2梅花，3红桃，4葵花
        /// <summary>
        /// 获取花色 1方块，2梅花，3红桃，4葵花
        /// </summary>
        /// <param name="Suit"></param>
        /// <returns></returns>
        public static String GetSuit(string Suit)
        {
            return Suit.Substring(0, 1);
        }
        #endregion

        #region 获取牌型 GetCardType()
        /// <summary>
        /// 获取牌型
        /// </summary>
        /// <param name="crCards">传入参数要求已经做大小排序</param>
        /// <returns></returns>
        public static int GetCardType(string[] crCards)
        {

            bool tmSameColor = true;    //定义颜色是否一样
            bool tmLineCard = true;     //是否连续的值，判断同花顺
            string tmFirstSuit = GetSuit(crCards[0]);     //第一只牌的花色
            string tmFirstRank = GetRank(crCards[0]);     //第一只牌的点数

            int i = 1; 
            if( crCards.Length == 5 )
            {
                #region 由第二只牌开始分析               
                //由第二只牌开始分析
                for( i = 1; i < crCards.Length; i++ )
                {
                    //判断花色是否相同 如果牌面里面只要有一个不同，则定义为不相同的花色，排除同花类型
                    if( GetSuit( crCards[ i ] ) != tmFirstSuit )
                        tmSameColor = false;
                    //判断是否有连续的值
                    if( ( int.Parse( GetRank( crCards[ i ] ) ) + i ) != int.Parse( tmFirstRank ) )
                        tmLineCard = false;
                    //都为False则退出循环    
                    if( !tmSameColor && !tmLineCard )
                        break;
                }
                 #endregion

                #region 判断是同花顺或顺子
                i = 1;
                //判断是否最小同花顺        
                if( !tmLineCard && int.Parse( tmFirstRank ) == ACE )
                {
                    for( i = 1; i < crCards.Length; i++ )
                    {
                        if( ( int.Parse( GetRank( crCards[ i ] ) ) + i + 8 ) != int.Parse( tmFirstRank ) )
                            break;
                    }
                    if( i == crCards.Length )
                    {
                        tmLineCard = true;
                    }
                }

                //皇家同花顺 A K Q J 10
                if( tmSameColor && tmLineCard && ( int.Parse( GetRank( crCards[ 1 ] ) ) == KING ) )
                    return CT_KING_TONG_HUA_SHUN;
                //顺子类型
                if( !tmSameColor && tmLineCard )
                    return CT_SHUN_ZI;
                //同花类型
                if( tmSameColor && !tmLineCard )
                    return CT_TONG_HUA;
                //同花顺类型
                if( tmSameColor && tmLineCard )
                    return CT_TONG_HUA_SHUN;
                #endregion
            }
           



            #region 判断其他牌型
            //相同点数的牌数
            int tmSameCount = 0;
            //单只的数量,两只的数量,三只的数量
            int tmSignedCount = 0, tmTwoCount = 0, tmThreeCount = 0, tmFourCount = 0 ;

            for (i = 0; i < crCards.Length; i++)
            {
                tmSameCount = 0;

                for (int j = i + 1; j < crCards.Length; j++)
                {
                    
                    if (GetRank(crCards[i]) != GetRank(crCards[j]))
                    {
                        if( j + 1 < crCards.Length )
                        {
                            if( GetRank( crCards[ j ] ) == GetRank( crCards[ j + 1 ] ) )
                            {
                                if( tmSameCount > 1 )
                                {
                                    tmTwoCount++;
                                }
                            }
                        }
                        break;
                    }
                    tmSameCount++;
         
                }
                switch (tmSameCount)
                {
                    case 0: { tmSignedCount++; }; break;    //单只
                    case 1: { tmTwoCount++; }; break;       //两张
                    case 2: { tmThreeCount++; }; break;     //三张
                    case 3: { tmFourCount++; }; break;      //四张
                }

                if (tmFourCount == 1) return CT_TIE_ZHI;                            //铁枝牌 四条
                if (tmTwoCount == 2) return CT_TWO_LONG;                            //两对牌
                if (tmTwoCount == 1 && tmThreeCount == 1) return CT_HU_LU;          //葫芦牌
                if (tmTwoCount == 0 && tmThreeCount == 1) return CT_THREE_TIAO;     //三条牌
                if (tmTwoCount == 1 && tmSignedCount == 3) return CT_ONE_LONG;      //一对牌            
            }

            return CT_SINGLE;                                                       //高牌
            #endregion
        }
        #endregion

        #region 扑克排序 BubbleSort()
        /// <summary>
        /// 扑克排序
        /// </summary>
        /// <param name="R"></param>
        public static string[] BubbleSort(string[] crCards)
        {
            string[] tmCards = new string[crCards.Length];
            //赋值一个临时变量
            for (int i = 0; i < crCards.Length; i++)
            {
                tmCards[i] = crCards[i];
            }

            //排序操作
            bool tmSorted = true;
            string tmCardData = "";
            int tmLastCount = crCards.Length - 1;
            do
            {
                tmSorted = true;
                for (int i = 0; i < tmLastCount; i++)
                {
                    if ((int.Parse(GetRank(tmCards[i])) < int.Parse(GetRank(tmCards[i + 1]))) || ((int.Parse(GetRank(tmCards[i])) == int.Parse(GetRank(tmCards[i + 1]))) && (int.Parse(GetSuit(tmCards[i])) < int.Parse(GetSuit(tmCards[i + 1])))))
                    {
                        tmCardData = tmCards[i];
                        tmCards[i] = tmCards[i + 1];
                        tmCards[i + 1] = tmCardData;
                        tmSorted = false;
                    }
                }
                tmLastCount--;
            } while (tmSorted == false);
            return tmCards;
        }
        #endregion


        public string FiveFromSeven( string[] iCards, )
        {
            //return 0;
        }


        #region 用户最大牌型 FiveFromSeven()
        /// <summary>
        /// 用户最大牌型
        /// </summary>
        /// <param name="Cards">用户扑克列表，7个</param>
        /// <returns></returns>
        public string FiveFromSeven(string[] iCards)
        {
            string r = "";
            int num = 0;


            //最后一只是Z说明下注完成
            if (iCards.Length == 7)
            {
                #region 由大到小排列扑克牌
                //由大到小排列扑克牌
                string[] newCards = BubbleSort(iCards);

                for (int i = 0; i < newCards.Length; i++)
                {
                    r += toHtml(newCards[i]);
                }
                #endregion

                #region 查找最大牌型
                //当前最大牌型
                string[] big_Cards = new string[5];

                for (int a = 0; a < 3; a++)
                {
                    for (int b = a + 1; b < 4; b++)
                    {
                        for (int c = b + 1; c < 5; c++)
                        {
                            for (int d = c + 1; d < 6; d++)
                            {
                                for (int e = d + 1; e < 7; e++)
                                {
                                    string[] tm_Cards = new string[5];
                                    tm_Cards.SetValue(newCards[a], 0);
                                    tm_Cards.SetValue(newCards[b], 1);
                                    tm_Cards.SetValue(newCards[c], 2);
                                    tm_Cards.SetValue(newCards[d], 3);
                                    tm_Cards.SetValue(newCards[e], 4);
                                    //类型判断
                                    if (num == 0)
                                    {
                                        big_Cards = tm_Cards;
                                    }
                                    else
                                    {
                                        string CC = CompareCard(tm_Cards, big_Cards);
                                        if (CC == CC_BIG_CARD.ToString())
                                        {
                                            big_Cards = tm_Cards;
                                        }
                                    }
                                    r = big_Cards[0] + "," + big_Cards[1] + "," + big_Cards[2] + "," + big_Cards[3] + "," + big_Cards[4];
                                    num++;
                                }
                            }
                        }
                    }
                }
                #endregion
            }

            return r;
        }
        #endregion

        #region 计算牌型 CompareCard()
        /// <summary>
        /// 计算牌型 传入5只手牌
        /// </summary>
        /// <param name="NextCards">下一副牌 5只</param>
        /// <param name="BigCards">目前最大的牌 5只</param>
        /// <returns></returns>
        public string CompareCard(string[] NextCards, string[] BigCards)
        {

            //获取新旧牌类型
            int NextCardType = GetCardType(NextCards);
            int BigCardType = GetCardType(BigCards);

            //牌型不同，直接可对比大小的情况
            if (NextCardType > BigCardType) return CC_BIG_CARD.ToString(); //大
            if (NextCardType < BigCardType) return CC_SMALL_CARD.ToString(); //小

            //牌型相同，按照每个类型对比
            switch (BigCardType)
            {
                #region 高牌判断
                case CT_SINGLE://高牌情况，对比最大的点数
                    {
                        int i = 0;

                        for (i = 0; i < BigCards.Length; i++)
                        {
                            //高牌点数
                            int tmNextRank = int.Parse(GetRank(NextCards[i]));
                            int tmBigRank = int.Parse(GetRank(BigCards[i]));

                            if (tmNextRank > tmBigRank) return CC_BIG_CARD.ToString();         //大
                            else if (tmNextRank < tmBigRank) return CC_SMALL_CARD.ToString();  //小
                            else continue;                                          //平
                        }

                        if (i == BigCards.Length) return CC_DRAW_CARD.ToString();
                    }; break;
                #endregion

                #region 对子判断
                case CT_ONE_LONG:
                case CT_TWO_LONG:
                case CT_THREE_TIAO:
                case CT_TIE_ZHI:
                    {
                        //对子点数
                        string[] tmNextTWOLongRank = GetLongRank(NextCards).Split(',');
                        string[] tmBigTWOLongRank = GetLongRank(BigCards).Split(',');
                        for (int i = 0; i < tmNextTWOLongRank.Length; i++)
                        {
                            if (int.Parse(tmNextTWOLongRank[i]) > int.Parse(tmBigTWOLongRank[i])) return CC_BIG_CARD.ToString();         //大
                            else if (int.Parse(tmNextTWOLongRank[i]) < int.Parse(tmBigTWOLongRank[i])) return CC_SMALL_CARD.ToString();  //小                            
                        }
                        //去除同牌
                        string[] tmNextCards = RemoveRank(NextCards, tmNextTWOLongRank);
                        string[] tmBigCards = RemoveRank(BigCards, tmBigTWOLongRank);

                        int k = 0;
                        //对子点数相同，分析单只
                        for (k = 0; k < tmNextCards.Length; k++)
                        {
                            int tmNextRank = int.Parse(GetRank(tmNextCards[k]));
                            int tmBigRank = int.Parse(GetRank(tmBigCards[k]));

                            if (tmNextRank > tmBigRank) return CC_BIG_CARD.ToString();         //大
                            else if (tmNextRank < tmBigRank) return CC_SMALL_CARD.ToString();  //小
                            else continue;                                          //平
                        }
                        if (k == tmNextCards.Length) return CC_DRAW_CARD.ToString();
                    }; break;
                #endregion

                #region 顺子判断
                case CT_SHUN_ZI:
                case CT_TONG_HUA_SHUN:
                    {
                        int tmFirstNextRank = int.Parse(GetRank(NextCards[0]));
                        int tmFirstBigRank = int.Parse(GetRank(BigCards[0]));
                        bool bFirstNextMin = (tmFirstNextRank == int.Parse(GetRank(NextCards[1]) + 9));
                        bool bFirstBigMin = (tmFirstBigRank == int.Parse(GetRank(BigCards[1]) + 9));

                        if ((!bFirstNextMin) && (bFirstBigMin))
                        {
                            return CC_SMALL_CARD.ToString();
                        }
                        else if ((bFirstNextMin) && (!bFirstBigMin))
                        {
                            return CC_BIG_CARD.ToString();
                        }
                        else
                        {
                            if (tmFirstNextRank == tmFirstBigRank) return CC_DRAW_CARD.ToString();
                            if (tmFirstNextRank > tmFirstBigRank) return CC_BIG_CARD.ToString();
                            if (tmFirstNextRank < tmFirstBigRank) return CC_SMALL_CARD.ToString();
                        }
                    }; break;
                #endregion

                #region 同花判断
                case CT_TONG_HUA:
                    {
                        int i = 0;
                        for (i = 0; i < NextCards.Length; i++)
                        {
                            int tmNextRank = int.Parse(GetRank(NextCards[i]));
                            int tmBigRank = int.Parse(GetRank(BigCards[i]));
                            if (tmNextRank > tmBigRank) return CC_BIG_CARD.ToString();
                            if (tmNextRank < tmBigRank) return CC_SMALL_CARD.ToString();
                        }
                        if (i == NextCards.Length) return CC_DRAW_CARD.ToString();
                    }; break;
                #endregion

                #region 葫芦判断
                case CT_HU_LU:
                    {
                        int tmNextThreeRank = int.Parse(GetSameRank(NextCards, 3));
                        int tmBigThreeRank = int.Parse(GetSameRank(BigCards, 3));

                        if (tmNextThreeRank > tmBigThreeRank) return CC_BIG_CARD.ToString();
                        else if (tmNextThreeRank < tmBigThreeRank) return CC_SMALL_CARD.ToString();

                        int tmNextTwoRank = int.Parse(GetSameRank(NextCards, 2));
                        int tmBigTwoRank = int.Parse(GetSameRank(BigCards, 2));

                        if (tmNextTwoRank > tmBigTwoRank) return CC_BIG_CARD.ToString();
                        else if (tmNextTwoRank < tmBigTwoRank) return CC_SMALL_CARD.ToString();
                        else return CC_DRAW_CARD.ToString();
                    };
                #endregion

                #region 皇家同花顺判断
                case CT_KING_TONG_HUA_SHUN: { return CC_DRAW_CARD.ToString(); }
                #endregion
            }

            return "";
        }
        #endregion

        #region 获得对子点数 GetLongRank(string[] iCards)
        /// <summary>
        /// 获得对子点数
        /// </summary>
        /// <param name="Cards">扑克牌</param>
        /// <returns>"01","02","03"...</returns>
        private string GetLongRank(string[] iCards)
        {
            string r = "";
            int num = 0;
            for (int i = 0; i < iCards.Length; i++)
            {
                for (int j = i + 1; j < iCards.Length; j++)
                {
                    if (GetRank(iCards[i]) == GetRank(iCards[j]))
                    {
                        if (num == 0)
                            r += GetRank(iCards[i]);
                        else
                            r += "," + GetRank(iCards[i]);
                        num++;
                    }
                }
            }
            return r;
        }
        #endregion

        #region 获得指定同牌点数 GetSameRank(string[] iCards,int RankNum)
        /// <summary>
        /// 获得指定同牌点数
        /// </summary>
        /// <param name="iCards">牌</param>
        /// <param name="RankNum">相同点数的个数</param>
        /// <returns>点数</returns>
        private string GetSameRank(string[] iCards, int RankNum)
        {
            int num = 0;
            string cds = "";
            RankNum--;
            for (int i = 0; i < iCards.Length; i++)
            {
                for (int j = i + 1; j < iCards.Length; j++)
                {
                    if (GetRank(iCards[i]) == GetRank(iCards[j]))
                    {
                        num++;
                    }
                }

                if (num == RankNum)
                {
                    cds = GetRank(iCards[i]);
                    break;
                }
                else
                {
                    num = 0;
                }
            }
            return cds;
        }
        #endregion

        #region 去除对子 RemoveRank(string[] Cards, string[] CardsLong)
        /// <summary>
        /// 去除对子
        /// </summary>
        /// <param name="Cards">原牌</param>
        /// <param name="CardsLong">对子列表</param>
        /// <returns></returns>
        public string[] RemoveRank(string[] Cards, string[] CardsLong)
        {
            string nCards = "";
            int num = 0;
            for (int i = 0; i < Cards.Length; i++)
            {
                for (int j = 0; j < CardsLong.Length; j++)
                {
                    if (int.Parse(GetRank(Cards[i])) != int.Parse(CardsLong[j]))
                    {
                        if (num == 0) nCards = Cards[i]; else nCards += "," + Cards[i];
                        num++;
                        break;
                    }
                }
            }
            return nCards.Split(',');
        }
        #endregion

        #region 返回扑克牌 ShowPokerByStr
        /// <summary>
        /// 返回扑克牌
        /// </summary>
        /// <param name="iCards"></param>
        /// <returns></returns>
        public static string ShowPokerByStr(string[] iCards)
        {
            string r = "";
            for (int i = 0; i < iCards.Length; i++)
            {
                if (i == 0)
                {
                    r = toHtml(iCards[i]);
                }
                else
                {
                    if (iCards[i] != UsAction.US_FINISH)
                    {
                        if (iCards[i] != UsAction.US_DISCARD)
                        {
                            r += toHtml(iCards[i]);
                        }
                    }
                }
            }
            return r;
        }
        #endregion

    }
    #endregion
}
