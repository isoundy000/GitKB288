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
using System.Text.RegularExpressions;
using BCW.Common;
using BCW.dzpk;
using System.Threading;
using BCW.Data;

/// <summary>
/// 2016德州扑克 黄国军20160330
/// 2016德州扑克 修改操作记录及大小盲 黄国军20160430.16.18
/// </summary>
public partial class bbs_game_dzpk : System.Web.UI.Page
{
    protected string xmlPath = "/Controls/dzpk.xml";
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    BCW.dzpk.dzpk dz = new BCW.dzpk.dzpk();
    int meid = 0;

    #region 页面加载 Page_Load()
    protected void Page_Load(object sender, EventArgs e)
    {
        meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        //维护提示

        if (ub.GetSub("GameStatus", xmlPath) == "1")
        {
            Utils.Safe(ub.GetSub("DzpkName", xmlPath) + "正在维护");
        }

        Master.Title = ub.GetSub("DzpkName", xmlPath);
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "rule":        // 德州扑克【规则】页
                RulePage();
                break;
            case "PGame":       //【游戏页】
                PGamePage();
                break;
            case "inroom":      //进入房间
                Inroom();
                break;
            case "outroom":     //退出房间
                Outroom();
                break;
            case "HisGame":     //历史记录
                HisGame();
                break;
            case "notes":       //我的历史
                notes();
                break;
            case "LookNotes":   //查看历史
                LookNotes();
                break;
            case "Get":         //获得游戏币
                GetGame();
                break;
            default:            //德州首页
                ReloadPage();
                break;
        }
    }
    #endregion

    #region 德州首页 ReloadPage()
    private void ReloadPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;" + ub.GetSub("DzpkName", xmlPath));
        //游戏状态显示
        //游戏状态
        int GameState = 0;
        string Cb = "";
        if (ub.GetSub("GameStatus", xmlPath) == "2")
        {
            GameState = 1; builder.Append("(测试)"); Cb = ub.GetSub("DzCoin", xmlPath);
        }
        else
        {
            Cb = ub.Get("SiteBz");
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        #region 接收到的页面参数处理
        int DRType = int.Parse(Utils.GetRequest("DRType", "all", 1, @"^[0-9]\d*$", "-1"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        if (DRType >= 0)
            strWhere += "DRType=" + DRType + "";

        string[] pageValUrl = { "act", "uid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        #endregion

        #region 顶部统计信息
        // 开始读取列表
        IList<BCW.dzpk.Model.DzpkRooms> listRooms = new BCW.dzpk.BLL.DzpkRooms().GetDzpkRoomss(pageIndex, pageSize, strWhere, out recordCount);
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("房间数量:" + listRooms.Count.ToString() + " 在线游戏人数：" + dz.ShowPlayerCount() + " |<a href=\"" + Utils.getUrl("dzpk.aspx") + "\">刷新</a><br />");
        builder.Append("奖池总额:" + dz.ShowRoomPotALL() + " 今日总赢币:" + dz.ShowWinPotAll_Today(GameState) + "<br />");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx?act=rule&amp;backurl=" + Utils.PostPage(1)) + "\">游戏规则</a>|<a href=\"" + Utils.getUrl("dzpk.aspx?act=notes&amp;backurl=" + Utils.PostPage(1)) + "\">我的记录</a>");
        #endregion

        #region 判断自己是否在游戏中,并显示到界面上
        //判断自己是否在游戏中,并显示到界面上
        BCW.dzpk.Model.DzpkPlayRanks CurPlayerRank = dz.chkPlayerRanks(meid);
        if (CurPlayerRank != null)
        {
            //获得房间信息
            BCW.dzpk.Model.DzpkRooms DzpkRoom = new BCW.dzpk.BLL.DzpkRooms().GetDzpkRooms(CurPlayerRank.RmID);
            if (DzpkRoom != null)
            {
                builder.Append(Out.Hr());
                builder.Append("你正在" + DzpkRoom.DRName + "游戏中 " + "<a href =\"" + Utils.getUrl("dzpk.aspx?act=PGame&amp;GKeyStr=" + dz.SetRoomID(CurPlayerRank.RmID.ToString())) + "\">进入</a><br />");
            }
        }
        else
        {
            builder.Append("<br />");
        }
        #endregion

        #region 显示自己的持币数,领取测试币
        //显示自己的持币数
        if (ub.GetSub("GameStatus", xmlPath) == "2")
        {
            builder.Append("您目前自带" + new BCW.SWB.BLL().GeUserGold(meid, 31) + ub.GetSub("DzCoin", xmlPath));
            if (new BCW.SWB.BLL().ExistsUserID(meid, 31))
            {
                BCW.SWB.Model swb = new BCW.SWB.BLL().GetModelForUserId(meid, 31);
                if (swb.UpdateTime.AddSeconds(double.Parse(ub.GetSub("GetGoldTime", xmlPath))) < DateTime.Now)
                {
                    builder.Append(" <a href =\"" + Utils.getUrl("dzpk.aspx?act=Get&amp;backurl=" + Utils.PostPage(1)) + "\">领" + ub.GetSub("DzCoin", xmlPath) + "</a>");
                }
            }
            else
            {
                builder.Append(" <a href =\"" + Utils.getUrl("dzpk.aspx?act=Get&amp;backurl=" + Utils.PostPage(1)) + "\">领" + ub.GetSub("DzCoin", xmlPath) + "</a>");
            }
        }
        else
        {
            builder.Append("您目前自带" + new BCW.BLL.User().GetGold(meid) + ub.Get("SiteBz"));
        }
        builder.Append(Out.Tab("</div>", ""));
        #endregion

        #region 分类处理
        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx?backurl=" + Utils.PostPage(1) + "") + "\">全部</a>");
        for (int j = 0; j < dz.getDRTypeName(-1).ToString().Split('|').Length; j++)
        {
            if (DRType == j)
            {
                builder.Append("|<a href=\"" + Utils.getUrl("dzpk.aspx?DRType=" + j + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">【" + dz.getDRTypeName(-1).ToString().Split('|')[j].ToString() + "】</a>");
            }
            else
            {
                builder.Append("|<a href=\"" + Utils.getUrl("dzpk.aspx?DRType=" + j + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + dz.getDRTypeName(-1).ToString().Split('|')[j].ToString() + "</a>");
            }
        }
        builder.Append(Out.Tab("</div>", ""));
        #endregion

        #region 房间列表
        if (listRooms.Count > 0)
        {
            int k = 1;
            foreach (BCW.dzpk.Model.DzpkRooms room in listRooms)
            {
                #region 检查房间状态
                builder.Append(Out.Tab("<div>", "<br />"));

                //实际在线人数:扣除离线的，超时的
                int OnlineNum = dz.GetOnlineRoom(room);
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".");
                if (room.PassWD != "")
                {
                    builder.Append("(密)");
                }
                if (OnlineNum <= 0)
                {
                    builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx?act=PGame&amp;GKeyStr=" + dz.SetRoomID(room.ID.ToString()) + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + room.DRName + "|" + dz.getDRTypeName(room.DRType) + "|空闲</a>(人数:" + OnlineNum.ToString() + "/" + room.GMaxUser + ")");
                }
                else if (OnlineNum == 1)
                {
                    builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx?act=PGame&amp;GKeyStr=" + dz.SetRoomID(room.ID.ToString()) + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + room.DRName + "|" + dz.getDRTypeName(room.DRType) + "|等待</a>(人数:" + OnlineNum.ToString() + "/" + room.GMaxUser + ")");
                }
                else
                {
                    builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx?act=PGame&amp;GKeyStr=" + dz.SetRoomID(room.ID.ToString()) + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + room.DRName + "|" + dz.getDRTypeName(room.DRType) + "|游戏中</a>(人数:" + OnlineNum.ToString() + "/" + room.GMaxUser + ")");
                }


                builder.Append(" 限:" + Utils.ConvertGold(room.GMinb) + "/" + Utils.ConvertGold(room.GMaxb) + Cb /*+ " 扣:" + room.GSerCharge + "‰"*/);
                builder.Append(Out.Tab("</div>", ""));

                k++;
                #endregion
            }

            //分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "还没有创建房间.."));
        }
        #endregion

        #region 排行榜
        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("【达人排行榜】");
        builder.Append(Out.Tab("</div>", "<br />"));
        List<Winner> DzAllList = dz.ShowDzList_ALL(GameState);
        List<Winner> DzMasList = dz.ShowDzMaster_Today(GameState);
        builder.Append(Out.Tab("<div>", ""));
        if (DzMasList.Count > 0)
        {
            builder.Append("(" + new BCW.BLL.User().GetUsName(DzMasList[0].wUsID) + ") 今天赢了:" + DzMasList[0].Pot + Cb + "<br />");
        }

        if (DzAllList.Count > 0)
        {
            for (int i = 0; i < DzAllList.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        {
                            builder.Append("榜首达人: [" + new BCW.BLL.User().GetUsName(DzAllList[i].wUsID) + "] 共赢:" + DzAllList[i].Pot + Cb);
                        }; break;
                    case 1:
                        {
                            builder.Append("次次达人: [" + new BCW.BLL.User().GetUsName(DzAllList[i].wUsID) + "] 共赢:" + DzAllList[i].Pot + Cb);
                        }; break;
                    case 2:
                        {
                            builder.Append("险胜达人: [" + new BCW.BLL.User().GetUsName(DzAllList[i].wUsID) + "] 共赢:" + DzAllList[i].Pot + Cb);
                        }; break;
                    default:
                        {
                            break;
                        };
                }
                if (i < DzAllList.Count - 1) { builder.Append("<br />"); }
            }
        }
        builder.Append(Out.Tab("</div>", ""));
        #endregion

        //闲聊显示
        builder.Append(BCW.User.Users.ShowSpeak(31, "dzpk.aspx", 5, 0));

        //游戏底部Ubb
        string Foot = ub.GetSub("dzpkFoot", xmlPath);
        if (Foot != "")
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(Foot)));
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 退出房间 Outroom()
    /// <summary>
    /// 退出房间
    /// </summary>
    private void Outroom()
    {
        string f = dz.OutRoom(meid);
        if (f == UsAction.AC_SUCCESS)
        {
            UpdateiGoldByDzpk(meid);
        }
        Utils.Success("温馨提示", "退出成功", Utils.getUrl("dzpk.aspx"), "1");
    }
    #endregion

    #region 进入房间 Inroom()
    /// <summary>
    /// 进入房间
    /// </summary>
    private void Inroom()
    {
        #region 判断ID
        //判断ID
        string GKeyStr = Utils.GetRequest("GKeyStr", "all", 1, @"^[^\^]{1,20}$", "");
        int GKeyID = dz.GetRoomID(GKeyStr);
        if (GKeyID < 0) { Utils.Error("房间ID有误，请重新登入", Utils.getUrl("default.aspx")); }
        string GPWStr = Utils.GetRequest("GPWStr", "all", 1, @"^[^\^]{1,20}$", "");
        #endregion

        #region 初始化账号房间等信息
        //获得当前房间model
        BCW.dzpk.Model.DzpkRooms DzpkRoom = new BCW.dzpk.BLL.DzpkRooms().GetDzpkRooms(GKeyID);
        //获得当前玩家队列model
        BCW.dzpk.Model.DzpkPlayRanks CurPlayerRank = dz.chkPlayerRanks(meid);
        //获得全部队列信息
        DataTable dt_Ranks = new BCW.dzpk.BLL.DzpkPlayRanks().GetList("*", "Rmid=" + DzpkRoom.ID.ToString()).Tables[0];
        #endregion

        #region 房间基本信息
        //房间基本信息
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("dzpk.aspx") + "\">" + ub.GetSub("DzpkName", xmlPath) + "</a>&gt;" + DzpkRoom.DRName);
        builder.Append(Out.Tab("</div>", "<br />"));
        #endregion

        #region 判断是否测试号
        if (ub.GetSub("DemoIDS", xmlPath) != "")
        {
            bool noplay = false;
            string[] ids = ub.GetSub("DemoIDS", xmlPath).Split('#');
            for (int i = 0; i < ids.Length; i++)
            {
                if (ids[i].ToString() == meid.ToString())
                {
                    noplay = true;
                    break;
                }
            }
            if (!noplay)
            {
                Utils.Error("游戏正在紧张的内部测试,你不属于内测人员,请内心等待游戏开放,谢谢支持", Utils.getPage("dzpk.aspx"));
            }
        }
        #endregion

        #region 判定玩家是否在其他房间
        //判定玩家是否在其他房间
        if (CurPlayerRank != null)
        {
            if (CurPlayerRank.RmID != DzpkRoom.ID)
            {
                Utils.Success("温馨提示", "你已在【" + DzpkRoom.DRName + "】坐下，现在将为你跳转", Utils.getUrl("dzpk.aspx?act=PGame&amp;GKeyStr=" + dz.SetRoomID(CurPlayerRank.RmID.ToString())), "3");
            }
            else
            {
                Utils.Success("温馨提示", "你已经在房间内，请勿重复进入，现在将为你跳转", Utils.getUrl("dzpk.aspx?act=PGame&amp;GKeyStr=" + dz.SetRoomID(CurPlayerRank.RmID.ToString())), "3");
            }
        }
        #endregion

        #region 入房判断
        else
        {
            #region 判断房间人数最大值
            //判断房间人数最大值
            if (DzpkRoom.GMaxUser - dt_Ranks.Rows.Count <= 0) { Utils.Error("房间已满员，请尝试进入其他房间", Utils.getPage("dzpk.aspx")); }
            #endregion

            #region 正常范围
            else
            {
                #region 输入房间密码 跳转到 act=inroom
                if (DzpkRoom.PassWD != "" && GPWStr != DzpkRoom.PassWD)
                {
                    string strText = "请输入房间密码(可选):/,";
                    string strName = "GPWStr";
                    string strType = "text,hidden";
                    string strValu = "";
                    string strOthe = "";
                    strOthe = "进入|reset,dzpk.aspx?act=inroom&amp;GKeyStr=" + dz.SetRoomID(DzpkRoom.ID.ToString()) + "&amp;backurl=" + Utils.PostPage(1) + ",post,1,red|blue";
                    string strEmpt = "";
                    string strIdea = "/";
                    builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

                    builder.Append(Out.Tab("<div>", Out.Hr()));
                    if (GPWStr != "")
                    {
                        builder.Append("你输入的密码不正确，请重新输入！<br />");
                    }

                    if (GKeyID < 0)
                    {
                        builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx") + "\">返回上一级</a>");
                    }
                    else
                    {
                        builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx?act=PGame&amp;GKeyStr=" + GKeyStr) + "\">返回上一级</a>");
                    }
                    builder.Append(Out.Tab("</div>", ""));

                    builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
                    builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
                    builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
                    builder.Append(Out.Tab("</div>", ""));
                }
                #endregion
                else
                {
                    #region 判断游戏币是否足够 if (ub.GetSub("GameStatus", xmlPath) == "2")
                    //如果游戏处于测试中,则扣取测试币而不是充值币
                    if (ub.GetSub("GameStatus", xmlPath) == "2")
                    {
                        //判断测试币
                        if (DzpkRoom.GMinb > new BCW.SWB.BLL().GeUserGold(meid, 31))
                        {
                            Utils.Error("你持有的" + ub.GetSub("DzCoin", xmlPath) + "不够，请在游戏首页免费领取后再试", Utils.getPage("dzpk.aspx"));
                        }
                    }
                    else
                    {
                        if (DzpkRoom.GMinb > new BCW.BLL.User().GetGold(meid))
                        {
                            Utils.Error("你持有的" + ub.Get("SiteBz") + "不够，请充值后再试", Utils.getPage("dzpk.aspx"));
                        }
                    }
                    #endregion

                    #region 支付安全提示
                    //支付安全提示
                    string[] p_pageArr = { "GkeyStr", "backurl", "act" };
                    BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr);
                    #endregion

                    #region 玩家列表初始化
                    CurPlayerRank = new BCW.dzpk.Model.DzpkPlayRanks();
                    CurPlayerRank.RmID = DzpkRoom.ID;
                    CurPlayerRank.UsID = meid;
                    CurPlayerRank.RankChk = dz.getMaxRankChk();
                    if (1 > dt_Ranks.Rows.Count) { CurPlayerRank.RankMake = "A"; } else { CurPlayerRank.RankMake = ""; }
                    CurPlayerRank.PokerCards = "P";
                    CurPlayerRank.PokerChips = "P";
                    CurPlayerRank.TimeOut = DateTime.Now;
                    if (ub.GetSub("GameStatus", xmlPath) == "2")
                    {
                        CurPlayerRank.RmMake = "(测)";
                    }
                    else
                    {
                        CurPlayerRank.RmMake = "";
                    }
                    CurPlayerRank.RankBanker = "";
                    CurPlayerRank.RankChips = "";
                    #endregion

                    #region 加入奖金池
                    if (ub.GetSub("GameStatus", xmlPath) == "2")
                    {
                        #region 测试
                        //测试模式
                        //如果玩家持币不超过最大上限
                        if (DzpkRoom.GMaxb > new BCW.SWB.BLL().GeUserGold(meid, 31))
                        {
                            //更新酷币 从玩家身上扣除酷币
                            CurPlayerRank.UsPot = new BCW.SWB.BLL().GeUserGold(meid, 31);
                            new BCW.SWB.BLL().UpdateMoney(meid, -new BCW.SWB.BLL().GeUserGold(meid, 31), 31);
                        }
                        else
                        {
                            //玩家游戏币最大值为房间最大值
                            CurPlayerRank.UsPot = DzpkRoom.GMaxb;
                            new BCW.SWB.BLL().UpdateMoney(meid, -DzpkRoom.GMaxb, 31);
                        }
                        #endregion
                    }
                    else
                    {
                        #region 正常
                        //正常模式
                        //如果玩家持币不超过最大上限
                        if (DzpkRoom.GMaxb > new BCW.BLL.User().GetGold(meid))
                        {
                            //更新酷币 从玩家身上扣除酷币
                            CurPlayerRank.UsPot = new BCW.BLL.User().GetGold(meid);
                            new BCW.BLL.User().UpdateiGold(meid, new BCW.BLL.User().GetUsName(meid), -new BCW.BLL.User().GetGold(meid), ub.GetSub("DzpkName", xmlPath) + "进入房间:" + DzpkRoom.DRName);
                        }
                        else
                        {
                            //玩家游戏币最大值为房间最大值
                            CurPlayerRank.UsPot = DzpkRoom.GMaxb;
                            new BCW.BLL.User().UpdateiGold(meid, new BCW.BLL.User().GetUsName(meid), -DzpkRoom.GMaxb, ub.GetSub("DzpkName", xmlPath) + "进入房间:" + DzpkRoom.DRName);
                        }
                        #endregion
                    }
                    #endregion

                    #region 添加到游戏记录
                    ///添加到玩家列表
                    new BCW.dzpk.BLL.DzpkPlayRanks().Add(CurPlayerRank);
                    ///增加游戏记录
                    string AC = "";
                    if (ub.GetSub("GameStatus", xmlPath) == "2")
                    {
                        AC = "(测)" + UsAction.AC_INROOM;
                    }
                    else { AC = UsAction.AC_INROOM; }
                    dzpk.UpdateDzpkAct(CurPlayerRank, AC, CurPlayerRank.UsPot);
                    #endregion

                    Utils.Success("温馨提示", "你已进入【" + DzpkRoom.DRName + "】，现在将为你跳转，请耐心等待游戏开始", Utils.getUrl("dzpk.aspx?act=PGame&amp;GKeyStr=" + dz.SetRoomID(CurPlayerRank.RmID.ToString())), "1");
                }
            }
            #endregion
        }
        #endregion
    }
    #endregion

    #region 【游戏页】 PGamePage()
    private void PGamePage()
    {
        #region 判断ID
        //判断ID
        string GKeyStr = Utils.GetRequest("GKeyStr", "all", 1, @"^[^\^]{1,20}$", "");
        int GKeyID = dz.GetRoomID(GKeyStr);
        if (GKeyID < 0) { Utils.Error("房间ID有误，请重新登入", Utils.getUrl("default.aspx")); }
        #endregion

        #region 初始化房间和玩家队列
        //获得当前房间model
        BCW.dzpk.Model.DzpkRooms DzpkRoom = new BCW.dzpk.BLL.DzpkRooms().GetDzpkRooms(GKeyID);
        //获得当前玩家队列model
        BCW.dzpk.Model.DzpkPlayRanks CurPlayerRank = dz.chkPlayerRanks(meid);
        //是否可操作标记
        int P_Ctrl = 0;
        #endregion

        #region 房间基本信息
        //房间基本信息
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        if (ub.GetSub("GameStatus", xmlPath) == "2")
        {
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("dzpk.aspx") + "\">" + ub.GetSub("DzpkName", xmlPath) + "(测试)</a>&gt;" + DzpkRoom.DRName);
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("dzpk.aspx") + "\">" + ub.GetSub("DzpkName", xmlPath) + "</a>&gt;" + DzpkRoom.DRName);
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        string Process = "";
        #endregion

        #region 判定及下注
        //判定初期
        if (CurPlayerRank != null)
        {
            #region 重复登入其他房间 处理
            //重复登入其他房间
            if (CurPlayerRank.RmID != DzpkRoom.ID)
            {
                Utils.Success("温馨提示", "你在【" + DzpkRoom.DRName + "】，请勿重复入房，现在将为你跳转", Utils.getUrl("dzpk.aspx?act=PGame&amp;GKeyStr=" + dz.SetRoomID(CurPlayerRank.RmID.ToString())), "3");
            }
            #endregion

            #region 下注处理
            else
            {
                long ACount = 0;
                string AC = Utils.GetRequest("ac", "all", 1, "", "");

                //下注处理
                if ("addChips" == Utils.GetRequest("addact", "all", 1, "", ""))
                {
                    ACount = Int64.Parse(Utils.GetRequest("PokerChipsNum", "post", 4, @"^[0-9]\d*$", "押注输入错误"));
                }
                bool isNext = false;
                //保存下注前的金额
                long T_UsPot = CurPlayerRank.UsPot;
                Process = dz.RankMakeProcess(AC, ACount, CurPlayerRank, ref isNext);
                if (isNext)
                {
                    string Cb = "";
                    if (ub.GetSub("GameStatus", xmlPath) == "2")
                    {
                        Cb = ub.GetSub("DzCoin", xmlPath);
                    }
                    else
                    {
                        Cb = ub.Get("SiteBz");
                    }
                    if (AC != "")
                    {
                        int dd = AC.Length;
                        if (ACount <= 0)
                        {
                            Utils.Success("温馨提示", AC + " 操作成功", Utils.getUrl("dzpk.aspx?act=PGame&amp;GKeyStr=" + dz.SetRoomID(CurPlayerRank.RmID.ToString())), "1");
                        }
                        else
                        {
                            if (AC == UsAction.AC_FOLD)
                            {
                                Utils.Success("温馨提示", AC + " 操作成功", Utils.getUrl("dzpk.aspx?act=PGame&amp;GKeyStr=" + dz.SetRoomID(CurPlayerRank.RmID.ToString())), "1");
                            }
                            else if (AC != UsAction.AC_ALLIN)
                            {
                                Utils.Success("温馨提示", AC + ACount + Cb + " 操作成功", Utils.getUrl("dzpk.aspx?act=PGame&amp;GKeyStr=" + dz.SetRoomID(CurPlayerRank.RmID.ToString())), "1");
                            }
                            else
                            {
                                Utils.Success("温馨提示", AC + T_UsPot + Cb + " 操作成功", Utils.getUrl("dzpk.aspx?act=PGame&amp;GKeyStr=" + dz.SetRoomID(CurPlayerRank.RmID.ToString())), "1");
                            }
                        }
                    }
                    else
                    {
                        P_Ctrl = 1;
                    }
                }
            }
            #endregion
        }
        #endregion

        #region 基础信息
        //获得全部队列信息
        DataTable dt_Ranks = new BCW.dzpk.BLL.DzpkPlayRanks().GetList("*", "Rmid=" + DzpkRoom.ID.ToString() + " ORDER BY RankChk").Tables[0];
        //更新房间信息 在下注处理中可能会影响到房间信息,所以这里重新获取一次房间信息
        DzpkRoom = new BCW.dzpk.BLL.DzpkRooms().GetDzpkRooms(GKeyID);
        //显示房间信息
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("【" + dz.getDRTypeName(DzpkRoom.DRType) + "房间:" + DzpkRoom.DRName.ToString() + "】 <a href=\"" + Utils.getUrl("dzpk.aspx?act=rule&amp;GKeyStr=" + GKeyStr + "&amp;backurl=" + Utils.getPage(0) + "") + "\">游戏规则</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        if (ub.GetSub("GameStatus", xmlPath) == "2")
        {
            builder.Append(Out.Div("div", "在线玩家:" + dt_Ranks.Rows.Count + "/" + DzpkRoom.GMaxUser + "<br />奖池金额:" + Utils.ConvertGold(DzpkRoom.GSidePot) + ub.GetSub("DzCoin", xmlPath) + " <br />大小盲注:" + Utils.ConvertGold(DzpkRoom.GSmallb) + ub.GetSub("DzCoin", xmlPath) + "/" + Utils.ConvertGold(DzpkRoom.GBigb) + ub.GetSub("DzCoin", xmlPath) + "<br />下注限额:" + (DzpkRoom.GMinb) + ub.GetSub("DzCoin", xmlPath) + "/" + Utils.ConvertGold(DzpkRoom.GMaxb) + ub.GetSub("DzCoin", xmlPath) + "<br />当前流程:" + DzpkRoom.GActID.ToString()));
        }
        else
        {
            builder.Append(Out.Div("div", "在线玩家:" + dt_Ranks.Rows.Count + "/" + DzpkRoom.GMaxUser + "<br />奖池金额:" + Utils.ConvertGold(DzpkRoom.GSidePot) + ub.Get("SiteBz") + " <br />大小盲注:" + Utils.ConvertGold(DzpkRoom.GSmallb) + ub.Get("SiteBz") + "/" + Utils.ConvertGold(DzpkRoom.GBigb) + ub.Get("SiteBz") + "<br />下注限额:" + (DzpkRoom.GMinb) + ub.Get("SiteBz") + "/" + Utils.ConvertGold(DzpkRoom.GMaxb) + ub.Get("SiteBz") + "<br />当前流程:" + DzpkRoom.GActID.ToString()));
        }
        builder.Append(Out.Div("div", ""));
        #endregion

        #region 玩家列表 tmHtml 生成
        //游戏信息
        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("【玩家队列】 <a href=\"" + Utils.getUrl("dzpk.aspx?act=PGame&amp;GKeyStr=" + GKeyStr) + "\">刷新</a>");
        builder.Append(Out.Tab("</div>", ""));

        int Meinroom = 0;   //判断自己是否在列表中
        int MeiRanks = 0;   //判断是否到自己出牌
        string RankMake = ""; //当前出牌者
        string RankMeLeft = "";    //我自己
        string RankMeRight = "";

        string tmHtml = "";   //扑克牌储存代码

        builder.Append(Out.Tab("<div>", "<br />"));
        List<Winner> WinnerList = new List<Winner>();

        if (dt_Ranks.Rows.Count > 0)
        {
            for (int i = 0; i < dt_Ranks.Rows.Count; i++)
            {
                BCW.dzpk.Model.DzpkPlayRanks Ranks = new BCW.dzpk.BLL.DzpkPlayRanks().GetDzpkPlayRanks(int.Parse(dt_Ranks.Rows[i]["ID"].ToString()));

                #region 判断出牌者
                //自己在列表中，则不输出我要坐下
                if (Ranks.UsID == meid)
                {
                    RankMeLeft = "<font style='BACKGROUND-COLOR: #cccccc;font-weight:bold;'>&spades;";
                    RankMeRight = "</font>";
                    Meinroom++;
                }
                else
                {
                    RankMeLeft = "";
                    RankMeRight = "";
                }

                //是否为出牌者
                if (Ranks.RankMake == UsAction.US_RANKMAKE)
                {
                    RankMake = "☛&ensp;";
                    if (Ranks.UsID == meid)
                    {
                        MeiRanks++;
                        if (dt_Ranks.Rows.Count >= 2)
                        {
                            Master.Refresh = DzpkRoom.GSetTime;
                        }
                    }
                    RankMake = "→";

                }
                else
                {
                    RankMake = "&emsp;&ensp;";
                }
                #endregion

                #region 输出到界面，显示牌型
                switch (i)
                {
                    //取消庄家标记
                    case 0:
                        {
                            //小盲 标记为SB
                            tmHtml += (RankMake + +(i + 1) + "." + RankMeLeft + "(小)" + new BCW.BLL.User().GetUsName(Ranks.UsID) + "(" + Ranks.UsID + ")" + RankMeRight);
                        }; break;
                    case 1:
                        {
                            //大盲 标记为DB
                            tmHtml += ("<br />" + RankMake + (i + 1) + "." + RankMeLeft + "(大)" + new BCW.BLL.User().GetUsName(Ranks.UsID) + "(" + Ranks.UsID + ")" + RankMeRight);
                        }; break;
                    //case 2:
                    //    {
                    //        //大盲 标记为DB
                    //        tmHtml += ("<br />" + RankMake + (i + 1) + "." + RankMeLeft + "(大)" + new BCW.BLL.User().GetUsName(Ranks.UsID) + "(" + Ranks.UsID + ")" + RankMeRight);
                    //    }; break;
                    default:
                        {
                            tmHtml += ("<br />" + RankMake + (i + 1) + "." + RankMeLeft + "" + new BCW.BLL.User().GetUsName(Ranks.UsID) + "(" + Ranks.UsID + ")" + RankMeRight);
                        }; break;
                }
                if (ub.GetSub("GameStatus", xmlPath) == "2")
                {
                    tmHtml += ("  [" + Utils.ConvertGold(Ranks.UsPot) + ub.GetSub("DzCoin", xmlPath) + "]");
                }
                else
                {
                    tmHtml += ("  [" + Utils.ConvertGold(Ranks.UsPot) + ub.Get("SiteBz") + "]");
                }
                //显示玩家的扑克牌
                tmHtml += ("<br />&emsp;&emsp;&ensp;" + ShowPokerByUs(Ranks, DzpkRoom));
                //返回玩家下注信息 
                tmHtml += ("<br />&emsp;&emsp;&ensp;" + ShowChipByUs(Ranks));
                #endregion
            }
        }
        else
        {
            tmHtml += ("暂无玩家");
        }
        #endregion

        #region 旁观
        if (Meinroom == 0)
        {
            builder.Append("抱歉，房间目前暂不提供旁观功能!");
        }
        else
        {
            builder.Append(tmHtml);
        }
        #endregion

        builder.Append(Out.Hr());

        #region 提示及操作
        if (Meinroom == 0)
        {
            #region 我要坐下按钮
            builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx?act=inroom&amp;GkeyStr=" + dz.SetRoomID(DzpkRoom.ID.ToString()) + "&amp;backurl=" + Utils.PostPage(1)) + "\">我要坐下&gt;&gt;</a><br />注:系统将预先扣除" + Utils.ConvertGold(DzpkRoom.GMaxb));
            if (ub.GetSub("GameStatus", xmlPath) == "2")
            {
                builder.Append(ub.GetSub("DzCoin", xmlPath));
            }
            else
            {
                builder.Append(ub.Get("SiteBz"));
            }
            builder.Append(",退房后反还");
            #endregion
        }
        else
        {
            #region 玩家多于2时的处理
            if (dt_Ranks.Rows.Count >= 2)
            {
                if (DzpkRoom.GActID > 0)
                {
                    #region 大于0时的显示
                    if (MeiRanks > 0)
                    {
                        if (P_Ctrl == 0)
                        {
                            string strText = "";
                            if (ub.GetSub("GameStatus", xmlPath) == "2")
                            {
                                strText = "押" + ub.GetSub("DzCoin", xmlPath) + " 限时:" + DzpkRoom.GSetTime + "秒/,,";
                            }
                            else
                            {
                                strText = "押" + ub.Get("SiteBz") + " 限时:" + DzpkRoom.GSetTime + "秒/,,";
                            }
                            string strName = "PokerChipsNum,addact";
                            string strType = "num,hidden";
                            long ChipsVal = DzpkRoom.LastRank - dzpk.GetChipTotle(CurPlayerRank.PokerChips);
                            if (ChipsVal > CurPlayerRank.UsPot) { ChipsVal = CurPlayerRank.UsPot; }
                            string strValu = "" + ChipsVal.ToString() + "'addChips";
                            string strEmpt = "true,false";
                            string strIdea = "/";
                            string strOthe = UsAction.toACstring(ChipsVal, CurPlayerRank.UsPot) + "," + Utils.getUrl("dzpk.aspx?act=PGame&amp;GKeyStr=" + dz.SetRoomID(CurPlayerRank.RmID.ToString())) + ",post,1," + UsAction.toACcolor(ChipsVal, CurPlayerRank.UsPot);
                            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                            if (CurPlayerRank != null)
                            {
                                int timeout = int.Parse((DzpkRoom.GSetTime - DateTime.Now.Subtract(CurPlayerRank.TimeOut).TotalSeconds).ToString("0"));
                                if (timeout > 0)
                                {
                                    string daojishi = "";
                                    if (Utils.Isie() || Utils.GetUA().ToLower().Contains("opera/8"))
                                    {
                                        daojishi = new BCW.JS.somejs().daojishi("divDzpk", CurPlayerRank.TimeOut.AddSeconds(DzpkRoom.GSetTime + 1));
                                    }
                                    else
                                    {
                                        daojishi = new BCW.JS.somejs().daojishi2("divDzpk", CurPlayerRank.TimeOut.AddSeconds(DzpkRoom.GSetTime + 1));
                                    }
                                    builder.Append(Process + "<br />" + daojishi + "后系统自动弃牌,请及时操作");
                                }
                                else
                                {
                                    builder.Append(Process);
                                }
                            }
                            else
                            {
                                builder.Append(Process);
                            }
                        }
                        else
                        {
                            if (DzpkRoom.GActID != 0)
                            {
                                Master.Refresh = 3;
                            }
                            builder.Append("等待玩家操作");
                        }
                    }
                    else
                    {
                        if (DzpkRoom.GActID != 0)
                        {
                            Master.Refresh = 3;
                        }
                        builder.Append("等待玩家操作");
                    }
                    #endregion
                }
                else
                {
                    #region 倒计时
                    DateTime dtBeginTime = DateTime.Parse(DzpkRoom.LastTime.ToString()).AddSeconds(DzpkRoom.GSetTime + 2);
                    string daojishi = "";
                    if (Utils.Isie() || Utils.GetUA().ToLower().Contains("opera/8"))
                    {
                        daojishi = new BCW.JS.somejs().daojishi("divhp3sy", dtBeginTime);
                    }
                    else
                    {
                        daojishi = new BCW.JS.somejs().daojishi2("divhp3sy", dtBeginTime);
                    }
                    builder.Append("请稍后，" + daojishi + "后游戏即将开始");
                    #endregion
                }
            }
            else
            {
                if (DzpkRoom.GActID != 0)
                {
                    Master.Refresh = 3;
                }
                builder.Append("等待玩家");
            }
            #endregion
        }
        #endregion

        builder.Append(Out.Tab("</div>", ""));

        if (Meinroom > 0)
        {
            #region 操作记录
            builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
            //string RmMakeLast = "";            
            builder.Append("【" + CurPlayerRank.RmMake + "局】");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append(ShowGameAct(DzpkRoom, 10));
            builder.Append(Out.Tab("</div>", ""));
            #endregion

            #region 游戏历史
            builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
            builder.Append("【游戏历史】");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div>", "<br />"));
            string shg = ShowHisGame(DzpkRoom, 0, 3);
            builder.Append(shg);
            if (shg != "")
            {
                builder.Append("<br />");
            }
            builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx?act=HisGame&amp;GKeyStr=" + dz.SetRoomID(DzpkRoom.ID.ToString())) + "\">更多...</a>");
            builder.Append(Out.Tab("</div>", ""));
            #endregion
        }

        #region 底部
        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("【你的信息】");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        if (ub.GetSub("GameStatus", xmlPath) == "2")
        {
            builder.Append("您目前自带" + new BCW.SWB.BLL().GeUserGold(meid, 31) + ub.GetSub("DzCoin", xmlPath) + "");
        }
        else
        {
            builder.Append("您目前自带" + Utils.ConvertGold(new BCW.BLL.User().GetGold(meid)) + "" + ub.Get("SiteBz") + "");
        }
        builder.Append("<br /><a href=\"" + Utils.getUrl("dzpk.aspx?act=outroom") + "\">退出房间&gt;&gt;</a>");
        builder.Append(Out.Tab("</div>", ""));
        #endregion

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 显示【玩家输赢】信息 ShowHisCard
    /// <summary>
    /// 显示玩家输赢信息
    /// </summary>
    /// <param name="Ranks"></param>
    /// <param name="DzpkRoom"></param>
    /// <returns></returns>
    private string ShowHisCard(BCW.dzpk.Model.DzpkPlayRanks Ranks, BCW.dzpk.Model.DzpkRooms DzpkRoom)
    {
        string r = "";

        DataTable HisData = new BCW.dzpk.BLL.DzpkHistory().GetList("*", "RmID=" + DzpkRoom.ID.ToString() + " AND UsID=" + Ranks.UsID + " AND RmMake='" + Ranks.RmMake + "'").Tables[0];

        for (int i = 0; i < HisData.Rows.Count; i++)
        {
            BCW.dzpk.Model.DzpkHistory HisModel = new BCW.dzpk.BLL.DzpkHistory().GetDzpkHistory(int.Parse(HisData.Rows[i]["ID"].ToString()));
            {
                string[] HandNum = HisModel.PokerCards.Split('|');
                //在房间游戏的用户未派彩，退出则直接派彩
                if (HisModel.IsPayOut == 1)
                {
                    if (!string.IsNullOrEmpty(HisModel.RmMake))
                    {
                        switch (HisModel.Winner)
                        {
                            case UsAction.US_WIN:
                                {
                                    //赢
                                    if (HandNum.Length == 2)
                                    {
                                        r += " &gt;【胜" + Card.ShowPokerByStr(HandNum[1].Split(',')) + "】 获:" + HisModel.GetMoney.ToString();
                                    }
                                }; break;
                            case UsAction.US_LOST:
                                {
                                    //输
                                    r += " &gt;【输】 获:" + HisModel.GetMoney.ToString();
                                }; break;
                            case UsAction.US_TIMEOUT:
                                {
                                    //超时
                                    r += " &gt;【超时】 获:" + HisModel.GetMoney.ToString();
                                }; break;
                            case UsAction.US_DISCARD:
                                {
                                    //弃牌
                                    r += " &gt;【弃牌】 获:" + HisModel.GetMoney.ToString();
                                }; break;
                        }
                    }
                }
            }
        }
        return r;
    }
    #endregion

    #region 更新房间的币值到酷币账户 UpdateiGoldByDzpk
    /// <summary>
    /// 更新房间的币值到酷币账户
    /// </summary>
    /// <param name="meid"></param>
    private void UpdateiGoldByDzpk(int UsID)
    {
        try
        {
            DataTable dt = new BCW.dzpk.BLL.DzpkHistory().GetList("ID", "UsID=" + UsID.ToString()).Tables[0];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    BCW.dzpk.Model.DzpkHistory dHistory_Model = new BCW.dzpk.BLL.DzpkHistory().GetDzpkHistory(int.Parse(dt.Rows[i]["ID"].ToString()));
                    BCW.dzpk.Model.DzpkRooms Room_Model = new BCW.dzpk.BLL.DzpkRooms().GetDzpkRooms(dHistory_Model.RmID);
                    //反还标记 =0的时候退出房间需要退回金币到玩家账号
                    if (dHistory_Model.IsPayOut == 0)
                    {
                        if (dHistory_Model.GetMoney > 0)
                        {
                            //大于0的时候反还金币到用户账户中
                            #region 测试
                            if (dHistory_Model.RmMake.Contains("(测)"))
                            {
                                //增加币操作
                                new BCW.SWB.BLL().UpdateMoney(UsID, dHistory_Model.GetMoney, 31);
                            }
                            #endregion

                            #region 正常
                            else
                            {
                                //增加币操作,正常模式写入消费日志
                                new BCW.BLL.User().UpdateiGold(UsID, new BCW.BLL.User().GetUsName(UsID), dHistory_Model.GetMoney, ub.GetSub("DzpkName", xmlPath) + "离开房间:" + Room_Model.DRName);
                            }
                            #endregion

                            dHistory_Model.IsPayOut = 1;
                            new BCW.dzpk.BLL.DzpkHistory().Update(dHistory_Model);
                        }
                    }
                }
            }
        }
        catch { }
    }
    #endregion

    #region 德州扑克【规则】页 RulePage()
    private void RulePage()
    {
        string GKeyStr = Utils.GetRequest("GKeyStr", "all", 1, @"^[^\^]{1,20}$", "");
        int GKeyID = -1;
        if (GKeyStr != "")
        {
            GKeyID = dz.GetRoomID(GKeyStr);
        }

        #region 房间基本信息
        //房间基本信息
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("dzpk.aspx") + "\">" + ub.GetSub("DzpkName", xmlPath) + "</a>&gt;游戏规则");
        builder.Append(Out.Tab("</div>", "<br />"));
        #endregion

        #region 牌型
        //牌型
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("【牌型】");
        builder.Append(Out.Tab("</div>", "<br/>"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("1.皇家同花顺：" + Card.toHtml("414") + Card.toHtml("413") + Card.toHtml("412") + Card.toHtml("411") + Card.toHtml("410") + "<br />");
        builder.Append("2.同花顺，最大：" + Card.toHtml("413") + Card.toHtml("412") + Card.toHtml("411") + Card.toHtml("410") + Card.toHtml("409") + " 最小：" + Card.toHtml("114") + Card.toHtml("102") + Card.toHtml("103") + Card.toHtml("104") + Card.toHtml("105") + "<br />");
        builder.Append("3.四条(铁支)，最大：" + Card.toHtml("414") + Card.toHtml("314") + Card.toHtml("214") + Card.toHtml("114") + Card.toHtml("413") + " 最小：" + Card.toHtml("402") + Card.toHtml("302") + Card.toHtml("202") + Card.toHtml("102") + Card.toHtml("103") + "<br />");
        builder.Append("4.葫芦(豪斯)，最大：" + Card.toHtml("414") + Card.toHtml("314") + Card.toHtml("214") + Card.toHtml("413") + Card.toHtml("313") + " 最小：" + Card.toHtml("402") + Card.toHtml("302") + Card.toHtml("202") + Card.toHtml("203") + Card.toHtml("103") + "<br />");
        builder.Append("5.同花，最大：" + Card.toHtml("414") + Card.toHtml("413") + Card.toHtml("412") + Card.toHtml("411") + Card.toHtml("409") + " 最小：" + Card.toHtml("102") + Card.toHtml("103") + Card.toHtml("104") + Card.toHtml("105") + Card.toHtml("107") + "<br />");
        builder.Append("6.顺子，最大：" + Card.toHtml("414") + Card.toHtml("413") + Card.toHtml("412") + Card.toHtml("411") + Card.toHtml("310") + " 最小：" + Card.toHtml("114") + Card.toHtml("102") + Card.toHtml("103") + Card.toHtml("104") + Card.toHtml("205") + "<br />");
        builder.Append("7.三条(加两单牌)，最大：" + Card.toHtml("414") + Card.toHtml("314") + Card.toHtml("214") + Card.toHtml("413") + Card.toHtml("412") + " 最小：" + Card.toHtml("402") + Card.toHtml("302") + Card.toHtml("202") + Card.toHtml("103") + Card.toHtml("104") + "<br />");
        builder.Append("8.两对，最大：" + Card.toHtml("414") + Card.toHtml("314") + Card.toHtml("413") + Card.toHtml("313") + Card.toHtml("412") + " 最小：" + Card.toHtml("202") + Card.toHtml("102") + Card.toHtml("203") + Card.toHtml("103") + Card.toHtml("104") + "<br />");
        builder.Append("9.一对，最大：" + Card.toHtml("414") + Card.toHtml("314") + Card.toHtml("413") + Card.toHtml("412") + Card.toHtml("411") + " 最小：" + Card.toHtml("202") + Card.toHtml("102") + Card.toHtml("103") + Card.toHtml("104") + Card.toHtml("105") + "<br />");
        builder.Append("10.单牌，最大：" + Card.toHtml("414") + Card.toHtml("413") + Card.toHtml("412") + Card.toHtml("411") + Card.toHtml("309") + " 最小：" + Card.toHtml("202") + Card.toHtml("103") + Card.toHtml("104") + Card.toHtml("105") + Card.toHtml("107"));
        builder.Append(Out.Tab("</div>", ""));
        #endregion

        #region 游戏规则
        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("【游戏规则】");
        builder.Append(Out.Tab("</div>", "<br/>"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("庄家代号：DD，小盲代号：SB，大盲代号：DB<br />");
        builder.Append("按顺序押注，庄家和盲注首轮由系统【强制】押注，后面玩家顺序操作<br /><br />");
        builder.Append("1.第一轮下注完毕系统发配2只底牌，仅有自己能看<br />");
        builder.Append("2.第二轮下注完毕系统发配3只公共牌<br />");
        builder.Append("3.第三轮下注完毕系统发配1只公共牌<br />");
        builder.Append("4.第四轮下注完毕系统发配1只公共牌<br /><br />");
        builder.Append("四轮下注完毕中途没有人退出或弃牌人数剩余超过2人，系统自动摊牌，显示胜负");
        builder.Append(Out.Tab("</div>", ""));
        #endregion

        #region 牌型大小
        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("【牌型大小】");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("1.皇家同花顺>同花顺>四条>葫芦>同花>顺子>三条>两队>一对>单牌<br />");
        builder.Append("2.牌点从大到小为：A、K、Q、J、10、9、8、7、6、5、4、3、2，各花色不分大小<br />");
        builder.Append("3.同种牌型，对子时比对子的大小，其它牌型比最大的牌张，如最大牌张相同则比第二大的<br />");
        builder.Append("4.牌张，以此类推，都相同时为相同<br />");
        builder.Append(Out.Tab("</div>", "<br />"));
        #endregion

        #region 底部
        builder.Append(Out.Tab("<div>", ""));
        if (GKeyID < 0)
        {
            builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx") + "\">返回上一级</a>");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx?act=PGame&amp;GKeyStr=" + GKeyStr) + "\">返回上一级</a>");
        }
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
        #endregion
    }
    #endregion

    #region 显示玩家的【扑克牌】 ShowPokerByUs()
    /// <summary>
    /// 显示玩家的扑克牌
    /// </summary>
    /// <param name="Ranks"></param>
    /// <param name="DzpkRoom"></param>
    /// <returns></returns>
    private string ShowPokerByUs(BCW.dzpk.Model.DzpkPlayRanks Ranks, BCW.dzpk.Model.DzpkRooms DzpkRoom)
    {
        string strHtml = "";
        strHtml += "";
        if (Ranks.PokerCards == "")
        {
            strHtml += "等待发牌";
        }
        else
        {
            string[] Cards = Ranks.PokerCards.Split(',');
            if (Cards.Length > 0)
            {
                for (int i = 0; i < Cards.Length; i++)
                {
                    if (Cards[i] != UsAction.US_FINISH)
                    {
                        if (DzpkRoom.GActID == 0)
                        {
                            //为0则开底牌
                            //BCW.dzpk.Card.toHtml("aa");
                            strHtml += Card.toHtml(Cards[i]);
                            //strHtml += Ranks.PokerCards.ToString();
                        }
                        else
                        {
                            if (i == 2) { strHtml += ","; }
                            if (Ranks.UsID == meid)
                            {
                                strHtml += Card.toHtml(Cards[i]);
                            }
                            else
                            {
                                if (i < 2)
                                {
                                    strHtml += BCW.dzpk.Card.toHtml("aa");
                                    //strHtml += BCW.dzpk.Card.toHtml(Cards[i]);
                                }
                                else
                                {
                                    strHtml += Card.toHtml(Cards[i]);
                                }
                            }
                        }
                    }
                }
            }
            ///等于0时,显示上盘的记录
            if (DzpkRoom.GActID == 0)
            {
                strHtml += ShowHisCard(Ranks, DzpkRoom);
            };
        }

        return strHtml;
    }
    #endregion

    #region 返回玩家【下注信息】 ShowChipByUs()
    /// <summary>
    /// 返回玩家下注信息 
    /// </summary>
    /// <param name="Ranks"></param>
    /// <returns></returns>
    private string ShowChipByUs(BCW.dzpk.Model.DzpkPlayRanks Ranks)
    {
        string strHtml = "";// += "<br />&emsp;&emsp;&ensp;";

        if (UsAction.US_DISCARD == dzpk.GetUSHandle(Ranks) || UsAction.US_TIMEOUT == dzpk.GetUSHandle(Ranks))
        {
            strHtml += Card.toHtml(dzpk.GetUSHandle(Ranks));
        }
        else
        {
            strHtml += Card.toHtml(UsAction.US_GOLD) + " " + dzpk.GetUSHandle(Ranks);
            if (!string.IsNullOrEmpty(Ranks.PokerChips))
            {
                strHtml += ("/" + dzpk.GetChipTotle(Ranks.PokerChips));
            }
        }
        return strHtml;
    }
    #endregion

    #region 显示【游戏历史】信息 ShowHisGame
    /// <summary>
    /// 显示玩家输赢信息
    /// </summary>
    /// <param name="DzpkRoom"></param>
    /// <param name="iDisDay">显示天数，默认0，当天，1显示到今天至昨天</param>
    /// <param name="iDisCount">显示条数，默认0条</param>
    /// <returns></returns>
    private string ShowHisGame(BCW.dzpk.Model.DzpkRooms DzpkRoom, int iDisDay, int iDisCount)
    {
        string r = "";
        int HisGameCount = 0;
        string tmMake = string.Empty;
        int num = 1;
        int DisDay = iDisDay; //显示记录天数
        int DisCount = iDisCount;//显示记录条数

        DataTable HisData = new BCW.dzpk.BLL.DzpkHistory().GetList("*", "RmID=" + DzpkRoom.ID.ToString() + " AND (RmMake <> '') ORDER BY TimeOut DESC").Tables[0];

        if (HisData.Rows.Count <= 0)
        {
            r += "暂无对局信息";
        }
        else
        {
            #region 显示历史
            for (int i = 0; i < HisData.Rows.Count; i++)
            {
                BCW.dzpk.Model.DzpkHistory HisModel = new BCW.dzpk.BLL.DzpkHistory().GetDzpkHistory(int.Parse(HisData.Rows[i]["ID"].ToString()));
                {
                    string[] HandNum = HisModel.PokerCards.Split('|');

                    if (!string.IsNullOrEmpty(HisModel.RmMake))
                    {
                        if (tmMake != HisModel.RmMake)
                        {
                            if (tmMake != string.Empty)
                            {
                                r += "<br />";
                            }
                            tmMake = HisModel.RmMake;
                            num = 1;
                            HisGameCount++;
                        }

                        if (HisGameCount > iDisCount)
                        {
                            break;
                        }
                        else
                        {
                            if (HisGameCount <= iDisCount)
                            {
                                if (num != 1)
                                {
                                    r += "<br />";
                                }
                                else if (HisGameCount > 1) { r += "<br />"; }
                            }

                            r += num + " ID:" + HisModel.UsID + "&emsp;" + Card.ShowPokerByStr(HandNum[0].Split(','));
                            string cb = "";
                            if (HisModel.RmMake.Contains("(测)"))
                            {
                                cb = ub.GetSub("DzCoin", xmlPath);
                            }
                            else
                            {
                                cb = ub.Get("SiteBz");
                            }
                            switch (HisModel.Winner)
                            {
                                case UsAction.US_WIN:
                                    {
                                        //赢
                                        if (HandNum.Length == 2)
                                        {
                                            r += " &gt;【胜" + Card.ShowPokerByStr(HandNum[1].Split(',')) + "】";
                                        }
                                        else
                                        {
                                            r += " &gt;【胜】";
                                        }
                                    }; break;
                                case UsAction.US_LOST:
                                    {
                                        //输
                                        r += " &gt;【输】";
                                    }; break;
                                case UsAction.US_TIMEOUT:
                                    {
                                        //超时
                                        r += " &gt;【超时】";
                                    }; break;
                                case UsAction.US_DISCARD:
                                    {
                                        //弃牌
                                        r += " &gt;【弃牌】";
                                    }; break;
                                case UsAction.US_OUT:
                                    {
                                        //离开房间
                                        r += " &gt;【退出房间】";
                                    }; break;
                            }

                            if (!string.IsNullOrEmpty(HisModel.PokerChips))
                            {
                                r += " 投:" + dzpk.GetChipTotle(HisModel.PokerChips) + cb;
                            }
                            r += " 获:" + HisModel.GetMoney + cb + " 时间：" + HisModel.TimeOut.ToString();
                            num++;
                        }
                    }
                }
            }
            #endregion
        }
        return r;
    }
    #endregion

    #region 显示历史页 HisGame
    /// <summary>
    /// 显示历史页
    /// </summary>
    private void HisGame()
    {
        #region 进入房间开始
        //判断ID
        string GKeyStr = Utils.GetRequest("GKeyStr", "all", 1, @"^[^\^]{1,20}$", "");
        int GKeyID = dz.GetRoomID(GKeyStr);
        if (GKeyID < 0) { Utils.Error("房间ID有误，请重新登入", Utils.getUrl("default.aspx")); }

        //获得当前房间model
        BCW.dzpk.Model.DzpkRooms DzpkRoom = new BCW.dzpk.BLL.DzpkRooms().GetDzpkRooms(GKeyID);
        //获得当前玩家队列model
        BCW.dzpk.Model.DzpkPlayRanks CurPlayerRank = dz.chkPlayerRanks(meid);

        //房间基本信息 PGame
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("dzpk.aspx") + "\">" + ub.GetSub("DzpkName", xmlPath) + "</a>&gt;<a href=\"" + Utils.getUrl("dzpk.aspx?act=PGame&amp;GKeyStr=" + dz.SetRoomID(DzpkRoom.ID.ToString())) + "\">" + DzpkRoom.DRName + "</a>&gt;游戏历史");
        builder.Append(Out.Tab("</div>", "<br />"));
        #endregion

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("【" + dz.getDRTypeName(DzpkRoom.DRType) + "房间:" + DzpkRoom.DRName.ToString() + "】 游戏历史");
        builder.Append(Out.Tab("</div>", "<br />"));

        #region 游戏历史
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append(ShowHisGame(DzpkRoom, 0, 10));
        builder.Append("<br /><a href=\"" + Utils.getUrl("dzpk.aspx?act=PGame&amp;GKeyStr=" + dz.SetRoomID(DzpkRoom.ID.ToString())) + "\">返回 " + DzpkRoom.DRName + "</a>");
        builder.Append(Out.Tab("</div>", ""));
        #endregion

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 我的记录 notes
    /// <summary>
    /// 我的记录
    /// </summary>
    private void notes()
    {
        #region 基本信息
        //获得当前玩家队列model
        BCW.dzpk.Model.DzpkPlayRanks CurPlayerRank = dz.chkPlayerRanks(meid);
        string gs = "";     //SQL搜索判断
        string Cb = "";     //币名
        string gstate = ""; //房间状态
        if (ub.GetSub("GameStatus", xmlPath) == "2")
        {
            Cb = ub.GetSub("DzCoin", xmlPath);
            gs = " AND RmMake like '%(测)%'";
            gstate = "(测试)";
        }
        else
        {
            Cb = ub.Get("SiteBz");
            gs = " AND RmMake not like '%(测)%'";
        }
        //房间基本信息 PGame
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("dzpk.aspx") + "\">" + ub.GetSub("DzpkName", xmlPath) + gstate + "</a>&gt;我的记录");
        builder.Append(Out.Tab("</div>", "<br />"));
        #endregion

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("【我的记录】");
        builder.Append(Out.Tab("</div>", "<br />"));

        #region 接收到的页面参数处理
        string strWhere = string.Empty;
        if (meid != 0) { strWhere = "UsID in (" + meid + ") "; }
        strWhere += gs;
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string[] pageValUrl = { "act", "iWhere", "backurl" };

        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        IList<BCW.dzpk.Model.DzpkRankList> DzAllList = new BCW.dzpk.BLL.DzpkRankList().GetDzpkRankLists(pageIndex, pageSize, strWhere, out recordCount);
        #endregion

        #region 记录列表
        if (DzAllList.Count > 0)
        {
            int k = 1;
            for (int i = 0; i < DzAllList.Count; i++)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }

                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".");
                builder.Append("标识:" + DzAllList[i].RmMake + " ");
                BCW.dzpk.Model.DzpkRooms Room = new BCW.dzpk.BLL.DzpkRooms().GetRoom(DzAllList[i].RmMake);
                if (Room != null)
                    builder.Append(Room.DRName);
                if (DzAllList[i].GetPot < 0)
                {
                    builder.Append(" 输:" + Utils.ConvertGold(DzAllList[i].GetPot) + Cb);
                }
                else if (DzAllList[i].GetPot == 0)
                {
                    builder.Append(" 平:" + Utils.ConvertGold(DzAllList[i].GetPot) + Cb);
                }
                else
                {
                    builder.Append(" 赢:" + Utils.ConvertGold(DzAllList[i].GetPot) + Cb);
                }
                builder.Append(" " + Convert.ToDateTime(DzAllList[i].Gtime.ToString()).ToString("yyyy-MM-dd hh:mm:ss"));
                builder.Append(" <a href=\"" + Utils.getUrl("dzpk.aspx?act=LookNotes&amp;r=" + DzAllList[i].RmMake) + "\">查看</a>");
                k++;
                if (i < DzAllList.Count - 1)
                {
                    builder.Append("<br />");
                }
                builder.Append(Out.Tab("</div>", ""));
            }

            //分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "还没有游戏记录.."));
        }
        #endregion

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("-----------<br />");
        builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx") + "\">返回房间列表</a>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    #endregion

    #region 查看历史 LookNotes
    /// <summary>
    /// 查看历史
    /// </summary>
    private void LookNotes()
    {
        #region 进入房间开始
        string r = Utils.GetRequest("r", "all", 1, @"^[^\^]{1,20}$", "");
        string gs = "";     //SQL搜索判断
        string gstate = ""; //房间状态
        if (ub.GetSub("GameStatus", xmlPath) == "2")
        {
            gstate = "(测试)";
        }

        //房间基本信息 
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("dzpk.aspx") + "\">" + ub.GetSub("DzpkName", xmlPath) + gstate + "</a>&gt;" + r + "详细记录");
        builder.Append(Out.Tab("</div>", "<br />"));
        #endregion

        builder.Append(Out.Tab("<div>", ""));
        BCW.dzpk.Model.DzpkRooms Room = new BCW.dzpk.BLL.DzpkRooms().GetRoom(r);
        builder.Append("【");
        if (Room != null)
            builder.Append(Room.DRName + ",");
        builder.Append(r + "局】详细记录");
        builder.Append(Out.Tab("</div>", "<br />"));

        #region 详细操作
        string strWhere = string.Empty;
        strWhere = "RmMake  = '" + r + "'";
        string sOrder = "ActTime";
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string[] pageValUrl = { "act", "iWhere", "r", "backurl" };

        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        IList<BCW.dzpk.Model.DzpkAct> ActList = new BCW.dzpk.BLL.DzpkAct().GetDzpkActs(pageIndex, pageSize, strWhere, sOrder, out recordCount);
        if (ActList.Count > 0)
        {
            int k = 1;
            for (int i = 0; i < ActList.Count; i++)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }
                //显示US
                builder.Append(k + "." + new BCW.BLL.User().GetUsName(ActList[i].UsID) + "(" + ActList[i].UsID + ")");
                //显示扑克牌
                if (ActList[i].MaxCard != "")
                {
                    if (ActList[i].MaxCard.Split('|').Length == 2)
                    {
                        builder.Append(" " + Card.ShowPokerByStr(ActList[i].MaxCard.Split('|')[0].Split(',')) + ",");
                        builder.Append(" " + Card.ShowPokerByStr(ActList[i].MaxCard.Split('|')[1].Split(',')) + " ");
                    }
                    else
                    {
                        if (ActList[i].UsID == meid)
                        {
                            builder.Append(" " + Card.ShowPokerByStr(ActList[i].MaxCard.Split(',')) + " ");
                        }
                        else
                        {
                            string[] Cards = ActList[i].MaxCard.Split(',');
                            for (int j = 0; j < Cards.Length; j++)
                            {
                                if (j < 2)
                                    Cards[j] = "aa";
                            }
                            builder.Append(" " + Card.ShowPokerByStr(Cards) + " ");
                        }
                    }
                }
                //加注
                if (ActList[i].ActMake == UsAction.AC_RAISE)
                {
                    builder.Append("<font color='red'>");
                }
                //赢牌或跟注
                if (ActList[i].ActMake == UsAction.AC_GETGOLD || ActList[i].ActMake == UsAction.AC_CALL)
                {
                    builder.Append("<font color='green'>");
                }

                string Cb = "";
                if (ActList[i].RmMake.Contains("(测)"))
                {
                    Cb = ub.GetSub("DzCoin", xmlPath);
                }
                else
                {
                    Cb = ub.Get("SiteBz");
                }
                //过牌 弃牌不显示金额
                if (ActList[i].ActMake == UsAction.AC_FOLD || ActList[i].ActMake == UsAction.AC_CHECK || ActList[i].ActMake == UsAction.AC_TIMEOUT_STR)
                {
                    builder.Append(" [" + ActList[i].ActMake + "] ");
                }
                else
                {
                    builder.Append(" [" + ActList[i].ActMake + "=>" + ActList[i].Money + "" + Cb + "] ");
                }
                //标签结尾
                if (ActList[i].ActMake == UsAction.AC_RAISE || ActList[i].ActMake == UsAction.AC_GETGOLD || ActList[i].ActMake == UsAction.AC_CALL)
                {
                    builder.Append("</font>");
                }
                builder.Append(ActList[i].ActTime.ToString() + "<br />");
                builder.Append(Out.Tab("</div>", ""));
                k++;
            }
            //分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有记录.."));
        }
        #endregion

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("-----------<br />");
        builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx?act=notes") + "\">返回我的记录</a>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    #endregion

    #region 玩家操作记录 ShowGameAct()
    /// <summary>
    /// 玩家操作记录
    /// </summary>
    /// <param name="DzpkRoom"></param>
    /// <returns></returns>
    private string ShowGameAct(BCW.dzpk.Model.DzpkRooms DzpkRoom, int iNum)
    {
        int num = 1;
        string r = string.Empty;
        DataTable ActData = new BCW.dzpk.BLL.DzpkAct().GetList("*", "RmID=" + DzpkRoom.ID.ToString() + " ORDER BY ActTime DESC,ID desc").Tables[0];

        if (ActData.Rows.Count <= 0)
        {
            r += "暂无对局信息";
        }
        else
        {
            string tmpMark = "";
            for (int i = 0; i < ActData.Rows.Count; i++)
            {
                if (num > iNum) break;
                BCW.dzpk.Model.DzpkAct ActModel = new BCW.dzpk.BLL.DzpkAct().GetDzpkAct(int.Parse(ActData.Rows[i]["ID"].ToString()));
                if (tmpMark != ActModel.RmMake && tmpMark != "")
                {
                    r += "------------<br />";
                    tmpMark = ActModel.RmMake;
                }
                else
                {
                    tmpMark = ActModel.RmMake;
                }
                r += num + ":" + ActModel.RmMake + "局 " + new BCW.BLL.User().GetUsName(ActModel.UsID);
                if (ActModel.ActMake == UsAction.AC_GETGOLD)
                {
                    if (ActModel.MaxCard.Split('|').Length == 2)
                    {
                        r += " " + Card.ShowPokerByStr(ActModel.MaxCard.Split('|')[1].Split(',')) + " ";
                    }
                }
                if (ActModel.ActMake == UsAction.AC_RAISE)
                {
                    r += "<font color='red'>";
                }
                if (ActModel.ActMake == UsAction.AC_GETGOLD || ActModel.ActMake == UsAction.AC_CALL)
                {
                    r += "<font color='green'>";
                }

                string Cb = "";
                if (ActModel.RmMake.Contains("(测)"))
                {
                    Cb = ub.GetSub("DzCoin", xmlPath);
                }
                else
                {
                    Cb = ub.Get("SiteBz");
                }

                //过牌 弃牌不显示金额
                if (ActModel.ActMake == UsAction.AC_FOLD || ActModel.ActMake == UsAction.AC_CHECK || ActModel.ActMake == UsAction.AC_MASTER || ActModel.ActMake == UsAction.AC_TIMEOUT_STR)
                {
                    r += " [" + ActModel.ActMake + "] ";
                }
                else
                {
                    r += " [" + ActModel.ActMake + "=>" + ActModel.Money + "" + Cb + "] ";
                }

                if (ActModel.ActMake == UsAction.AC_RAISE || ActModel.ActMake == UsAction.AC_GETGOLD || ActModel.ActMake == UsAction.AC_CALL)
                {
                    r += "</font>";
                }
                if (i < iNum - 1)
                {
                    r += ActModel.ActTime.ToString("yyyy-MM-dd hh:mm:ss") + "<br />";
                }
                else
                {
                    r += ActModel.ActTime.ToString("yyyy-MM-dd hh:mm:ss");
                }
                num++;
            }
        }
        return r;
    }
    #endregion

    #region 领取测试币 GetGame
    private void GetGame()
    {
        #region 显示自己的持币数,领取测试币
        //显示自己的持币数
        if (ub.GetSub("GameStatus", xmlPath) == "2")
        {
            builder.Append("您目前自带" + new BCW.SWB.BLL().GeUserGold(meid, 31) + ub.GetSub("DzCoin", xmlPath));
            if (new BCW.SWB.BLL().ExistsUserID(meid, 31))
            {
                BCW.SWB.Model swb = new BCW.SWB.BLL().GetModelForUserId(meid, 31);
                new BCW.SWB.BLL().UpdateTime(meid, DateTime.Now, 31);
                if (swb.UpdateTime.AddSeconds(double.Parse(ub.GetSub("GetGoldTime", xmlPath))) < DateTime.Now)
                {
                    new BCW.SWB.BLL().UpdateMoney(meid, long.Parse(ub.GetSub("GetGold", xmlPath)), 31);
                    Utils.Success("温馨提示", "恭喜你,领取了" + long.Parse(ub.GetSub("GetGold", xmlPath)) + ub.GetSub("DzCoin", xmlPath) + "，现在将为你跳转", Utils.getUrl("dzpk.aspx"), "3");
                }
                else
                {
                    Utils.Error("你已经领取过,请在" + swb.UpdateTime.AddSeconds(double.Parse(ub.GetSub("GetGoldTime", xmlPath))).ToString("hh:mm:ss") + "后再来领取", Utils.getUrl("dzpk.aspx"));
                }
            }
            else
            {
                BCW.SWB.Model swbu = new BCW.SWB.Model();
                swbu.GameID = 31;
                swbu.UserID = meid;
                swbu.Money = long.Parse(ub.GetSub("GetGold", xmlPath));
                swbu.UpdateTime = DateTime.Now;
                new BCW.SWB.BLL().Add(swbu);
                Utils.Success("温馨提示", "恭喜你,领取了" + long.Parse(ub.GetSub("GetGold", xmlPath)) + ub.GetSub("DzCoin", xmlPath) + "，现在将为你跳转", Utils.getUrl("dzpk.aspx"), "3");
            }
        }
        else
        {
            Utils.Error("目前游戏属于正常模式,暂不开放免费领币功能", Utils.getUrl("dzpk.aspx"));
        }
        #endregion
    }
    #endregion
}