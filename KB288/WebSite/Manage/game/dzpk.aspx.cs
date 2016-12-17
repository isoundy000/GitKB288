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
using BCW.Common;
using BCW.dzpk;
using BCW.TexasPoker;

public partial class Manage_game_dzpk : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    BCW.dzpk.dzpk dz = new BCW.dzpk.dzpk();
    protected string xmlPath = "/Controls/dzpk.xml";

    #region 加载数据分配
    /// <summary>
    /// 加载数据分配
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = ub.GetSub("DzpkName", xmlPath) + "管理";
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "view":
                //ViewPage();
                break;
            case "del":
                DelPage();
                break;
            case "KickOnlineALL":
                KickOnlineALLPage();
                break;
            case "KickOnline":      //清除在线玩家(单独操作)
                KickOnlinePage();
                break;
            case "newroom":         //新建或编辑房间
                ViewRoomPage();
                break;
            case "ResetGame":       //复位房间(不踢)
                ResetGamePage();
                break;
            case "ResetGameALL":
                ResetGameALLPage();
                break;
            case "DzList":          //盈亏排行
                DzList();
                break;
            case "Reward":          //奖励发放
                Reward();
                break;
            case "BackCoin":        //反负设置
                BackCoin();
                break;
            case "backsave":        //反负确认
                BackSavePage();
                break;
            case "notes":         //历史记录
                HisGamePage();
                break;
            case "LookNotes":       //详细记录
                LookNotes();
                break;
            case "ClrRoom":         //清房间手续费
                ClrRoom();
                break;
            case "PGame":           //游戏页
                PGamePage();
                break;
            default:
                ReloadPage();
                break;
        }
    }
    #endregion

    #region 管理首页 ReloadPage()
    /// <summary>
    /// 管理首页
    /// </summary>
    private void ReloadPage() {
        int GameState = 0;
        string Cb = "";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("..\\default.aspx") + "\">管理中心</a>&gt;");
        builder.Append(ub.GetSub("DzpkName", xmlPath));
        if (ub.GetSub("GameStatus", xmlPath) == "2")
        {
            GameState = 1; 
            Cb = ub.GetSub("DzCoin", xmlPath);
            builder.Append("(测试)");
        }
        else
        {
            Cb = ub.Get("SiteBz");
        }
        builder.Append("管理");
        builder.Append(Out.Tab("</div>", "<br />"));
        

        #region 接收到的页面参数处理
        int DRType = int.Parse(Utils.GetRequest("DRType", "all", 1, @"^[0-9]\d*$", "-1"));
        string iWhere = Utils.GetRequest("iWhere", "all", 1, "", "");
        string AC = Utils.GetRequest("ac", "all", 1, "", "");
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        string isAnd=string.Empty;
        if (DRType >= 0)
            strWhere += "DRType=" + DRType + "";

        if (strWhere != string.Empty) {
            isAnd = " AND ";
        }

        if (!string.IsNullOrEmpty(iWhere)) {
            if (AC == "搜房间")
            {
                strWhere += isAnd + "DRName like '%" + iWhere + "%'";
            }
        }


        string[] pageValUrl = { "act", "iWhere", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        #endregion

        // 开始读取列表
        IList<BCW.dzpk.Model.DzpkRooms> listRooms = new BCW.dzpk.BLL.DzpkRooms().GetDzpkRoomss(pageIndex, pageSize, strWhere, out recordCount);

        #region 顶部统计信息
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("房间数量:" + listRooms.Count.ToString() + " 在线游戏人数：" + dz.ShowPlayerCount() + "<br />");
        builder.Append("奖池总额:" + dz.ShowRoomPotALL() + " 今日总赢币:" + dz.ShowWinPotAll_Today(GameState) + "<br />");
        List<Winner> DzMasList = dz.ShowDzMaster_Today(GameState);
        if (DzMasList.Count > 0)
        {
            builder.Append("今日达人: [" + new BCW.BLL.User().GetUsName(DzMasList[0].wUsID) + "] 今天赢了:" + DzMasList[0].Pot + ub.Get("SiteBz"));
        }
        else
        {
            builder.Append("今日还木有达人哦");
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        #endregion       

        #region 分类处理
        builder.Append(Out.Tab("<div class=\"text\">【房间数:" + listRooms.Count.ToString() + "】", ""));
        builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx?backurl=" + Utils.PostPage(1) + "") + "\">全部</a>");
        for (int j = 0; j < dz.getDRTypeName(-1).ToString().Split('|').Length; j++) {
            if (DRType == j)
            {
                builder.Append("|<a href=\"" + Utils.getUrl("dzpk.aspx?DRType=" + j + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">【" + dz.getDRTypeName(-1).ToString().Split('|')[j].ToString() + "】</a>");
            }
            else
            {
                builder.Append("|<a href=\"" + Utils.getUrl("dzpk.aspx?DRType=" + j + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + dz.getDRTypeName(-1).ToString().Split('|')[j].ToString() + "</a>");
            }
        }
        builder.Append("·<a href=\"" + Utils.getUrl("dzpk.aspx") + "\">刷新</a>");
        builder.Append( "<br />测试区============================<br />" );

        foreach( KeyValuePair<int, Room> _keyVal in TexasPokerGame.Instance().roomMgr.dctRooms )
        {
            builder.Append( "<br />房间ID：" + _keyVal.Value.id );
        }

        builder.Append( "<br />测试区============================" );

        builder.Append( "<br />测试区============================" );
        builder.Append(Out.Tab("</div>", "<br />"));
        #endregion

        #region 列表
        if (listRooms.Count > 0)
        {
            int k = 1;
            foreach (BCW.dzpk.Model.DzpkRooms room in listRooms)
            {
                #region 检查房间状态
                builder.Append(Out.Tab("<div>", ""));

                //实际在线人数：扣除离线的，超时的
                int OnlineNum = dz.GetOnlineRoom(room);
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".");
                //builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx?act=ClrRoom&amp;id=" +room.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[清币]</a>");
                //builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx?act=KickOnline&amp;id=" + room.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[清在线]</a>");
                //builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx?act=ResetGame&amp;id=" + room.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[复位]</a>");

                if (OnlineNum <= 0)
                {
                    //builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx?act=del&amp;id=" + room.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[删房]</a>&gt;&nbsp;");
                    builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx?act=PGame&amp;GKeyStr=" + dz.SetRoomID(room.ID.ToString()) + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + room.DRName + "|" + dz.getDRTypeName(room.DRType) + "|空闲</a>(人数:" + OnlineNum.ToString() + "/" + room.GMaxUser + ")");
                }
                else if (OnlineNum == 1)
                {
                    //builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx?act=del&amp;id=" + room.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[删房]</a>&gt;&nbsp;");
                    builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx?act=PGame&amp;GKeyStr=" + dz.SetRoomID(room.ID.ToString()) + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + room.DRName + "|" + dz.getDRTypeName(room.DRType) + "|等待</a>(人数:" + OnlineNum.ToString() + "/" + room.GMaxUser + ")");
                }
                else
                {
                    builder.Append("&nbsp;<a href=\"" + Utils.getUrl("dzpk.aspx?act=PGame&amp;GKeyStr=" + dz.SetRoomID(room.ID.ToString()) + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + room.DRName + "|" + dz.getDRTypeName(room.DRType) + "|游戏中</a>(人数:" + OnlineNum.ToString() + "/" + room.GMaxUser + ")");
                }
                builder.Append("&nbsp;[入房限制:" + Utils.ConvertGold(room.GMinb) + "/" + Utils.ConvertGold(room.GMaxb) + ub.Get("SiteBz") + " 费率:" + room.GSerCharge + "‰ 已扣:" + Utils.ConvertGold(room.GSerChargeALL) + ub.Get("SiteBz") + "]");
                builder.Append(Out.Tab("</div>", "<br />"));

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

        #region 搜索
        string strText = "输入房名:/,";
        string strName = "iWhere,backurl";
        string strType = "text,hidden";
        string strValu = "'" + Utils.getPage(0) + "";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "搜房间,dzpk.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        #endregion

        #region 管理链接
        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("【游戏数据】");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx?act=DzList&amp;backurl=" + Utils.PostPage(1)) + "\">盈亏排行</a><br />");        
        builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx?act=notes&&amp;backurl=" + Utils.PostPage(1)) + "\">游戏历史</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx?act=BackCoin&amp;backurl=" + Utils.PostPage(1)) + "\">反负操作</a><br />");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("【基础操作】");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx?act=newroom&amp;DRType=" + DRType + "&amp;backurl=" + Utils.PostPage(1)) + "\">创建房间</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx?act=KickOnlineALL&amp;backurl=" + Utils.PostPage(1) + "") + "\">剔除所有玩家</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx?act=ResetGameALL&amp;backurl=" + Utils.PostPage(1) + "") + "\">复位所有房间</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("../xml/dzpkset.aspx?backurl=" + Utils.PostPage(1) + "") + "\">基础配置</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        #endregion
    }
    #endregion

    #region 复位房间 ResetGamePage
    /// <summary>
    /// 复位房间
    /// </summary>
    private void ResetGamePage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int RoomID = int.Parse(Utils.GetRequest("id", "all", 1, "", ""));
        BCW.dzpk.Model.DzpkRooms Room_Model = new BCW.dzpk.BLL.DzpkRooms().GetDzpkRooms(RoomID);

        if (info == "")
        {
            Master.Title = "房间复位";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定复位德州房间【" + Room_Model.DRName + "】吗？");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("【" + Room_Model.DRName + "】房间复位，游戏将重新开始(玩家不会被剔除)。<br />&nbsp;注意：已押下的酷币将无法返还，请注意操作！<br /><br />");
            builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx?act=ResetGame&amp;info=ok&amp;id=" + RoomID) + "\">确定复位</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            dzpk.ResetRoom(Room_Model);
            Utils.Success("房间复位", "操作成功..", Utils.getUrl("dzpk.aspx?act=PGame&amp;GKeyStr=" + dz.SetRoomID(RoomID.ToString())), "1");
        }
    }
    #endregion

    #region 复位全部房间 ResetGameALLPage
    /// <summary>
    /// 复位全部房间
    /// </summary>
    private void ResetGameALLPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");

        if (info == "")
        {
            Master.Title = "房间复位";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定复位【所有】德州房间吗？");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("【所有】德州房间将复位，游戏将重新开始(玩家不会被剔除)。<br />&nbsp;注意：已押下的酷币将无法返还，请注意操作！<br /><br />");
            builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx?act=ResetGameALL&amp;info=ok") + "\">确定复位</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            DataTable dt_Room = new BCW.dzpk.BLL.DzpkRooms().GetList("ID", "").Tables[0];
            for (int i = 0; i < dt_Room.Rows.Count; i++)
            {
                BCW.dzpk.Model.DzpkRooms Room_Model = new BCW.dzpk.BLL.DzpkRooms().GetDzpkRooms(int.Parse(dt_Room.Rows[i]["ID"].ToString()));
                dzpk.ResetRoom(Room_Model);
            }
            Utils.Success("房间复位", "操作成功..", Utils.getUrl("dzpk.aspx"), "1");
        }
    }
    #endregion

    #region 盈亏排行 DzList
    /// <summary>
    /// 盈亏排行
    /// </summary>
    private void DzList()
    {

        #region 变量接收
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        string us = Utils.GetRequest("us", "post", 1, "", "");
        string uid = Utils.GetRequest("uid", "all", 1, "", "");
        string Sort = Utils.GetRequest("Sort", "all", 1, "", "");
        string IOrder = Utils.GetRequest("IOrder", "all", 1, "", "");
        string nn = Utils.GetRequest("nn", "all", 1, "", "1");
        string gs = ""; //SQL搜索判断
        string Cb = ""; //币名

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("..\\default.aspx") + "\">管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx") + "\">" + ub.GetSub("DzpkName", xmlPath) + "管理</a>&gt;");
        if (Utils.ToSChinese(ac) != "发放奖励" && Utils.ToSChinese(ac) != "确定发奖")
        {
            builder.Append("盈亏排行");
        }
        else {
            builder.Append(Utils.ToSChinese(ac));
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        if (nn == "2")
        {
            Cb = ub.GetSub("DzCoin", xmlPath);
            gs = " AND RmMake like '%(测)%'";
        }
        else {
            Cb = ub.Get("SiteBz");
            gs = " AND RmMake not like '%(测)%'";
        }
        #endregion

        #region 日期赋值
        DateTime B_DATE;
        DateTime E_DATE;
        try
        {
            B_DATE = DateTime.Parse(Utils.GetRequest("B_DATE", "all", 1, "", DateTime.Today.AddYears(-1).Date.ToShortDateString() + " 00:00:00"));// DateTime.Today.AddYears(-100).Date.ToShortDateString() + " 00:00:00"));
            E_DATE = DateTime.Parse(Utils.GetRequest("E_DATE", "all", 1, "", DateTime.Today.Date.ToShortDateString() + " 23:59:59"));// DateTime.Today.AddYears(-100).Date.ToShortDateString() + " 23:59:59"));        
        }
        catch
        {
            B_DATE = DateTime.Parse(DateTime.Today.AddYears(-1).Date.ToShortDateString() + " 00:00:00");
            E_DATE = DateTime.Parse(DateTime.Today.Date.ToShortDateString() + " 23:59:59");
        }
        #endregion

        #region 盈亏排行
        if (Utils.ToSChinese(ac) == "" || Utils.ToSChinese(ac) == "搜索")
        {
            #region 顶部按钮
            string strWhere = string.Empty;
            builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
            builder.Append("【盈亏统计排行榜】<br />");
            #region 切换按钮
            //切换按钮
            if (nn == "1")
            {
                builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx?act=DzList&amp;Sort=" + Sort + "&amp;IOrder=" + IOrder + "&amp;B_DATE=" + B_DATE + "&amp;E_DATE=" + E_DATE + "&amp;uid=" + uid + "&amp;nn=2") + "\">正常模式</a>");
            }
            else {
                builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx?act=DzList&amp;Sort=" + Sort + "&amp;IOrder=" + IOrder + "&amp;B_DATE=" + B_DATE + "&amp;E_DATE=" + E_DATE + "&amp;uid=" + uid + "&amp;nn=1") + "\">测试模式</a>");
            }

            if (IOrder == "A")
            {
                builder.Append("|<a href=\"" + Utils.getUrl("dzpk.aspx?act=DzList&amp;Sort=" + Sort + "&amp;IOrder=D&amp;B_DATE=" + B_DATE + "&amp;E_DATE=" + E_DATE + "&amp;uid=" + uid + "&amp;nn=" + nn) + "\">降序</a>");
            }
            else {
                builder.Append("|<a href=\"" + Utils.getUrl("dzpk.aspx?act=DzList&amp;Sort=" + Sort + "&amp;IOrder=A&amp;B_DATE=" + B_DATE + "&amp;E_DATE=" + E_DATE + "&amp;uid=" + uid + "&amp;nn=" + nn) + "\">升序</a>");
            }

            if (Sort == "w")
            {
                builder.Append("|<a href=\"" + Utils.getUrl("dzpk.aspx?act=DzList&amp;Sort=l&amp;IOrder=" + IOrder + "&amp;B_DATE=" + B_DATE + "&amp;E_DATE=" + E_DATE + "&amp;uid=" + uid + "&amp;nn=" + nn) + "\">赢</a>");
            }
            else {
                builder.Append("|<a href=\"" + Utils.getUrl("dzpk.aspx?act=DzList&amp;Sort=w&amp;IOrder=" + IOrder + "&amp;B_DATE=" + B_DATE + "&amp;E_DATE=" + E_DATE + "&amp;uid=" + uid + "&amp;nn=" + nn) + "\">输</a>");
            }

            builder.Append("·<a href=\"" + Utils.getUrl("dzpk.aspx?act=DzList&amp;Sort=" + Sort + "&amp;IOrder=" + IOrder + "&amp;B_DATE=" + B_DATE + "&amp;E_DATE=" + E_DATE + "&amp;uid=" + uid + "&amp;nn=" + nn) + "\">刷新</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            #endregion

            if (!string.IsNullOrEmpty(uid)) { strWhere = "UsID in (" + uid + ") AND "; }
            strWhere += " (Gtime BETWEEN '" + B_DATE.ToString() + "' AND '" + E_DATE.ToString() + "') ";
            strWhere += gs;
            #endregion

            #region 列表
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string[] pageValUrl = { "act", "Sort", "IOrder", "B_DATE", "E_DATE", "uid", "nn", "iWhere", "backurl" };

            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            string strsql;

            IList<BCW.dzpk.Model.DzpkRankList> DzAllList = new BCW.dzpk.BLL.DzpkRankList().GetDzpkRankLists_Total(pageIndex, pageSize, strWhere, Sort, IOrder, out recordCount, out strsql);
            IList<BCW.dzpk.Model.DzpkRankList> DzAllList_ALL = new BCW.dzpk.BLL.DzpkRankList().GetDzpkRankLists_Total_All(strWhere, Sort, IOrder);
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("统计日:" + B_DATE.ToString("yyyy-MM-dd HH:mm:ss") + " 至 " + E_DATE.ToString("yyyy-MM-dd HH:mm:ss") + "<br />");
            ///赢家排行
            string DzWinUslist = "";
            if (DzAllList.Count > 0)
            {
                int k = 1;
                for (int i = 0; i < DzAllList.Count; i++)
                {
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".");
                    if (DzAllList[i].GetPot < 0)
                    {
                        builder.Append(new BCW.BLL.User().GetUsName(DzAllList[i].UsID) + "(" + DzAllList[i].UsID + ") 输:" + Utils.ConvertGold(DzAllList[i].GetPot) + Cb);
                    }
                    else
                    {
                        builder.Append(new BCW.BLL.User().GetUsName(DzAllList[i].UsID) + "(" + DzAllList[i].UsID + ") 赢:" + Utils.ConvertGold(DzAllList[i].GetPot) + Cb);
                    }
                    k++;

                    if (i < DzAllList.Count - 1)
                    {
                        builder.Append("<br />");
                    }
                }

                for (int i = 0; i < DzAllList_ALL.Count; i++)
                {
                    ///重新搜索所有的列表
                    if (DzWinUslist != "")
                    {
                        DzWinUslist += "#" + DzAllList_ALL[i].UsID;
                    }
                    else { DzWinUslist += DzAllList_ALL[i].UsID; }
                }

                //分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            builder.Append(Out.Tab("</div>", "<br />"));
            #endregion

            #region 表单
            builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
            builder.Append("【搜索】");
            builder.Append(Out.Tab("</div>", "<br />"));

            string strText = "输入会员ID(可输入多个，用'&sbquo;'分隔):/,开始时间:/,结束时间:/,:/,";
            string strName = "uid,B_DATE,E_DATE,backurl,us";
            string strType = "text,date,date,hidden,hidden";
            string strValu = uid + "'" + B_DATE.ToString("yyyy-MM-dd HH:mm:ss") + "'" + E_DATE.ToString("yyyy-MM-dd HH:mm:ss") + "'" + Utils.getPage(0) + "'" + DzWinUslist;
            string strEmpt = "true,true,true,false,false";
            string strIdea = "/";
            string strBtn = "发放奖励";
            string strOthe = "搜索|" + strBtn + "," + Utils.getUrl("dzpk.aspx?act=DzList&amp;Sort=" + Sort + "&amp;IOrder=" + IOrder + "&amp;uid=" + uid + "&amp;nn=" + nn) + ",post,1,red|red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            #endregion
        }
        #endregion

        #region 确定发奖
        else if (Utils.ToSChinese(ac) == "确定发奖")
        {
            if (Utils.GetRequest("us", "all", 1, "", "") != "")
            {
                int su = 0;
                string[] usss = Utils.GetRequest("us", "all", 1, "", "").Split('#');
                for (int i = 0; i < usss.Length; i++)
                {
                    long gold = long.Parse(Utils.GetRequest("gold" + i, "post", 1, "", "0"));
                    if (gold > 0)
                    {
                        int guid = int.Parse(usss[i]);
                        if (nn == "2")
                        {
                            new BCW.SWB.BLL().UpdateMoney(guid, gold, 31);
                        }
                        else {
                            new BCW.BLL.User().UpdateiGold(guid, gold, ub.GetSub("DzpkName", xmlPath) + "排行榜奖励");
                        }
                        //发内线
                        string strLog = "您在" + B_DATE.ToString("yyyy-MM-dd HH:mm:ss") + "到" + E_DATE.ToString("yyyy-MM-dd HH:mm:ss") + "的" + ub.GetSub("DzpkName", xmlPath) + "排行榜" + "上取得了第" + (i + 1) + "名的好成绩，系统奖励了" + gold + "" + Cb + "[url=/bbs/game/dzpk.aspx]进入" + ub.GetSub("DzpkName", xmlPath) + "[/url]";
                        new BCW.BLL.Guest().Add(0, guid, new BCW.BLL.User().GetUsName(guid), strLog);
                        //动态
                        string mename = new BCW.BLL.User().GetUsName(guid);
                        string wText = "[url=/bbs/uinfo.aspx?uid=" + guid + "]" + mename + "[/url]在[url=/bbs/game/dzpk.aspx]" + ub.GetSub("DzpkName", xmlPath) + "[/url]" + "上取得了第" + (i + 1) + "名的好成绩，系统奖励了" + gold + "" + Cb;
                        new BCW.BLL.Action().Add(1012, 0, guid, mename, wText);
                        su++;
                    }
                }
                Utils.Success(ub.GetSub("DzpkName", xmlPath) + "奖励派发", su + "个奖励,派发成功，正在返回..", Utils.getUrl("dzpk.aspx?act=DzList"), "1");
            }
            else {
                builder.Append(Out.Tab("<div>", Out.Hr()));
                builder.Append("暂无奖励玩家数据");
                builder.Append("<br /><a href=\"" + Utils.getUrl("dzpk.aspx?act=DzList") + "\">返回盈亏列表</a><br />");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
        }
        #endregion        

        #region 发放奖励
        else {
            if (us != "")
            {
                string[] uss = us.Split('#');
                builder.Append(Out.Tab("<div>", Out.Hr()));
                builder.Append("日期范围" + B_DATE.ToString("yyyy-MM-dd HH:mm:ss") + "到" + E_DATE.ToString("yyyy-MM-dd HH:mm:ss") + "<br />");
                if (nn == "2")
                {
                    builder.Append("测试版");
                }
                else { builder.Append("正式版"); }
                builder.Append("将发放" + Cb);
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
                builder.Append("【奖励设置】");
                builder.Append(Out.Tab("</div>", "<br />"));
                if (uss.Length > 0)
                {
                    int numus = 0;
                    string strText = "";
                    string strName = "";
                    string strType = "";
                    string strValu = "";
                    string strEmpt = "";
                    string strIdea = "/";
                    for (int i = 0; i < uss.Length; i++)
                    {
                        if (i == 9) { break; }
                        numus++;
                        if (i == 0)
                        {
                            strText += "top" + (i + 1) + ":" + new BCW.BLL.User().GetUsName(int.Parse(uss[i])) + "(" + uss[i] + ")/,";
                            strName += "gold" + i;
                            strType += "text";
                            strValu += "0";
                            strEmpt += "true";
                        }
                        else {
                            strText += "top" + (i + 1) + ":" + new BCW.BLL.User().GetUsName(int.Parse(uss[i])) + "(" + uss[i] + ")/,";
                            strName += ",gold" + i;
                            strType += ",text";
                            strValu += "'0";
                            strEmpt += ",true";
                        }
                    }
                    if (numus > 0)
                    {
                        strText += "/,/,/,/,";
                        strName += ",numus,us,B_DATE,E_DATE";
                        strType += ",hidden,hidden,hidden,hidden";
                        strValu += "'" + numus + "'" + us + "'" + B_DATE + "'" + E_DATE;
                        strEmpt += ",true,true";
                    }
                    string strOthe = "确定发奖," + Utils.getUrl("dzpk.aspx?act=DzList&amp;Sort=" + Sort + "&amp;IOrder=" + IOrder + "&amp;uid=" + uid + "&amp;nn=" + nn) + ",post,1,red";
                    builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

                    builder.Append(Out.Tab("<div>", Out.Hr()));
                    builder.Append("小提示:默认0,不发放奖励");
                    builder.Append(Out.Tab("</div>", "<br />"));
                }
            }
            else {
                builder.Append(Out.Tab("<div>", Out.Hr()));
                builder.Append("暂无奖励玩家数据");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
        }
        #endregion

        builder.Append(Out.Tab("<div>", Out.Hr()));
        if (Utils.ToSChinese(ac) == "发放奖励")
        {
            builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx?act=DzList") + "\">返回盈亏列表</a><br />");
        }
        
        builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx") + "\">返回上一层</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    #endregion

    #region 奖励发放 Reward
    /// <summary>
    /// 奖励发放
    /// </summary>
    private void Reward()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("..\\default.aspx") + "\">管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx") + "\">" + ub.GetSub("DzpkName", xmlPath) + "管理</a>&gt;");
        builder.Append("奖励发放");
        builder.Append(Out.Tab("</div>", "<br />"));

        #region 顶部按钮
        string uid = Utils.GetRequest("uid", "all", 1, "", "");
        string Sort = Utils.GetRequest("Sort", "all", 1, "", "");
        string IOrder = Utils.GetRequest("IOrder", "all", 1, "", "");
        string strWhere = string.Empty;
        DateTime B_DATE;
        DateTime E_DATE;
        try
        {
            B_DATE = DateTime.Parse(Utils.GetRequest("B_DATE", "all", 1, "", DateTime.Today.AddYears(-1).Date.ToShortDateString() + " 00:00:00"));// DateTime.Today.AddYears(-100).Date.ToShortDateString() + " 00:00:00"));
            E_DATE = DateTime.Parse(Utils.GetRequest("E_DATE", "all", 1, "", DateTime.Today.Date.ToShortDateString() + " 23:59:59"));// DateTime.Today.AddYears(-100).Date.ToShortDateString() + " 23:59:59"));        
        }
        catch
        {
            B_DATE = DateTime.Parse(DateTime.Today.AddYears(-1).Date.ToShortDateString() + " 00:00:00");
            E_DATE = DateTime.Parse(DateTime.Today.Date.ToShortDateString() + " 23:59:59");
        }

        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("【盈亏统计排行榜】");
        //builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx?act=DzList&amp;B_DATE=" + B_DATE + "&amp;E_DATE=" + E_DATE + "&amp;uid=" + uid) + "\">盈亏</a>");
        //builder.Append("|<a href=\"" + Utils.getUrl("dzpk.aspx?act=DzList&amp;Sort=w&amp;IOrder=" + IOrder + "&amp;B_DATE=" + B_DATE + "&amp;E_DATE=" + E_DATE + "&amp;uid=" + uid) + "\">单盈</a>");
        //builder.Append("|<a href=\"" + Utils.getUrl("dzpk.aspx?act=DzList&amp;Sort=l&amp;IOrder=" + IOrder + "&amp;B_DATE=" + B_DATE + "&amp;E_DATE=" + E_DATE + "&amp;uid=" + uid) + "\">单亏</a>");
        builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx?act=DzList&amp;Sort=" + Sort + "&amp;IOrder=D&amp;B_DATE=" + B_DATE + "&amp;E_DATE=" + E_DATE + "&amp;uid=" + uid) + "\">升序</a>");
        builder.Append("|<a href=\"" + Utils.getUrl("dzpk.aspx?act=DzList&amp;Sort=" + Sort + "&amp;IOrder=A&amp;B_DATE=" + B_DATE + "&amp;E_DATE=" + E_DATE + "&amp;uid=" + uid) + "\">降序</a>");
        builder.Append("·<a href=\"" + Utils.getUrl("dzpk.aspx") + "\">刷新</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        if (!string.IsNullOrEmpty(uid)) { strWhere = "UsID in (" + uid + ") AND "; }
        strWhere += " (Gtime BETWEEN '" + B_DATE.ToString() + "' AND '" + E_DATE.ToString() + "') ";
        #endregion

        #region 列表
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string[] pageValUrl = { "act", "iWhere", "backurl" };

        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        string strsql;

        IList<BCW.dzpk.Model.DzpkRankList> DzAllList = new BCW.dzpk.BLL.DzpkRankList().GetDzpkRankLists_Total(pageIndex, pageSize, strWhere, Sort, IOrder, out recordCount, out strsql);
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("统计日:" + B_DATE.ToString("yyyy-MM-dd HH:mm:ss") + " 至 " + E_DATE.ToString("yyyy-MM-dd HH:mm:ss") + "<br />");
        if (DzAllList.Count > 0)
        {
            int k = 1;
            for (int i = 0; i < DzAllList.Count; i++)
            {
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".");
                if (DzAllList[i].GetPot < 0)
                {
                    builder.Append("[" + new BCW.BLL.User().GetUsName(DzAllList[i].UsID) + "] 输:" + Utils.ConvertGold(DzAllList[i].GetPot) + ub.Get("SiteBz"));
                }
                else
                {
                    builder.Append("[" + new BCW.BLL.User().GetUsName(DzAllList[i].UsID) + "] 赢:" + Utils.ConvertGold(DzAllList[i].GetPot) + ub.Get("SiteBz"));
                }
                k++;
                if (i < DzAllList.Count - 1)
                {
                    builder.Append("<br />");
                }
            }
            //分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        #endregion

        #region 搜索
        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("【搜索】");
        builder.Append(Out.Tab("</div>", "<br />"));

        string strText = "输入会员ID(可输入多个，用'&sbquo;'分隔):/,开始时间:/,结束时间:/,";
        string strName = "uid,B_DATE,E_DATE,backurl";
        string strType = "text,date,date,hidden";
        string strValu = uid + "'" + B_DATE.ToString("yyyy-MM-dd HH:mm:ss") + "'" + E_DATE.ToString("yyyy-MM-dd HH:mm:ss") + "'" + Utils.getPage(0);
        string strEmpt = "true,true,true,false";
        string strIdea = "/";
        string strOthe = "搜索|奖励发放," + Utils.getUrl("dzpk.aspx?act=DzList") + ",post,1,red|red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        #endregion

        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx") + "\">返回上一层</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    #endregion

    #region 反负设置 BackCoin
    /// <summary>
    /// 反负设置
    /// </summary>
    private void BackCoin()
    {
        string nn = Utils.GetRequest("nn", "all", 1, "", "1");
        string gs = ""; //SQL搜索判断
        string Cb = ""; //币名       

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("..\\default.aspx") + "\">管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx") + "\">" + ub.GetSub("DzpkName", xmlPath));
        if (ub.GetSub("GameStatus", xmlPath) == "2")
        {            
            Cb = ub.GetSub("DzCoin", xmlPath);
            builder.Append("(测试)");
        }
        else
        {
            Cb = ub.Get("SiteBz");
        }
        builder.Append("管理</a>&gt;");
        builder.Append("反负设置");
        builder.Append(Out.Tab("</div>", "<br />"));

        #region 顶部按钮
        string strWhere = string.Empty;
        DateTime B_DATE;
        DateTime E_DATE;
        try
        {
            B_DATE = DateTime.Parse(Utils.GetRequest("B_DATE", "all", 1, "", DateTime.Today.AddYears(-1).Date.ToShortDateString() + " 00:00:00"));// DateTime.Today.AddYears(-100).Date.ToShortDateString() + " 00:00:00"));
            E_DATE = DateTime.Parse(Utils.GetRequest("E_DATE", "all", 1, "", DateTime.Today.Date.ToShortDateString() + " 23:59:59"));// DateTime.Today.AddYears(-100).Date.ToShortDateString() + " 23:59:59"));        
        }
        catch
        {
            B_DATE = DateTime.Parse(DateTime.Today.AddYears(-1).Date.ToShortDateString() + " 00:00:00");
            E_DATE = DateTime.Parse(DateTime.Today.Date.ToShortDateString() + " 23:59:59");
        }
        #endregion

        #region 反负设置
        builder.Append(Out.Tab("<div>", Out.Hr()));
        string t = "";
        if (ub.GetSub("GameStatus", xmlPath) == "2")
        {
            builder.Append("测试版");
            t = "反负只有内线没有消费记录";
        }
        else { builder.Append("正式版"); }
        builder.Append("将反负" + Cb);
        if (t != "") { builder.Append("<br />注意:" + Cb + t); }
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("【反负设置】");
        builder.Append(Out.Tab("</div>", "<br />"));

        string strText = "开始时间:/,结束时间:/,返负千分比:/,至少负多少才返:/,";
        string strName = "sTime,oTime,iTar,iPrice,act";
        string strType = "date,date,num,num,hidden";
        string strValu = "" + DT.FormatDate(B_DATE, 0) + "'" + DT.FormatDate(E_DATE, 0) + "'''backsave";
        string strEmpt = "false,false,true,true,false";
        string strIdea = "/";
        string strOthe = "马上返负,dzpk.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        #endregion

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx") + "\">返回上一层</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    #endregion

    #region 反负确认 BackSavePage
    /// <summary>
    /// 反负确认
    /// </summary>
    private void BackSavePage()
    {
        DateTime sTime = Utils.ParseTime(Utils.GetRequest("sTime", "post", 2, DT.RegexTime, "开始时间填写无效"));
        DateTime oTime = Utils.ParseTime(Utils.GetRequest("oTime", "post", 2, DT.RegexTime, "结束时间填写无效"));
        int iTar = Utils.ParseInt(Utils.GetRequest("iTar", "post", 2, @"^[0-9]\d*$", "千分比填写错误"));
        int iPrice = Utils.ParseInt(Utils.GetRequest("iPrice", "post", 2, @"^[0-9]\d*$", "至少多少币才返填写错误"));
        string gs = "";
        string Cb = "";
        if (ub.GetSub("GameStatus", xmlPath) == "2")
        {

            Cb = ub.GetSub("DzCoin", xmlPath);
            builder.Append("(测试)");
        }
        else
        {
            gs = "not";
            Cb = ub.Get("SiteBz");
        }
        string sfield = "UsID, SUM(GetPot)AS GetPot";
        string strsql = "(Gtime BETWEEN '" + sTime + "' AND '" + oTime + "')  AND RmMake " + gs + " like '%(测)%' GROUP BY UsID having SUM(GetPot) < 0 ORDER BY GetPot DESC";
        DataSet ds = new BCW.dzpk.BLL.DzpkRankList().GetList(sfield, strsql);
        if (ds.Tables.Count > 0)
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    long Cents = Convert.ToInt64(ds.Tables[0].Rows[i]["GetPot"]);
                    if (Cents < 0 && Cents < (-iPrice))
                    {
                        int usid = Convert.ToInt32(ds.Tables[0].Rows[i]["UsID"]);
                        long cent = Convert.ToInt64((-Cents) * (iTar * 0.001));
                        if (ub.GetSub("GameStatus", xmlPath) == "2")
                        {
                            new BCW.SWB.BLL().UpdateMoney(usid, cent, 31);
                        }
                        else {
                            new BCW.BLL.User().UpdateiGold(usid, cent, ub.GetSub("DzpkName", xmlPath) + "返负");
                        }
                        //发内线
                        string strLog = "根据您上期" + ub.GetSub("DzpkName", xmlPath) + "排行榜上的亏损情况，系统自动返还了" + cent + "" + Cb + "[url=/bbs/game/dzpk.aspx]进入" + ub.GetSub("DzpkName", xmlPath) + "[/url]";
                        string mename = new BCW.BLL.User().GetUsName(usid);
                        new BCW.BLL.Guest().Add(0, usid, mename, strLog);
                    }
                }
            }
        Utils.Success("返负操作", "返负操作成功", Utils.getUrl("dzpk.aspx?act=BackCoin"), "1");
    }
    #endregion

    #region 剔除在线玩家 KickOnlinePage()
    /// <summary>
    /// 剔除在线玩家
    /// </summary>
    private void KickOnlinePage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int RoomID = int.Parse(Utils.GetRequest("id", "all", 1, "", ""));
        BCW.dzpk.Model.DzpkRooms Room_Model = new BCW.dzpk.BLL.DzpkRooms().GetDzpkRooms(RoomID);

        if (info == "")
        {
            Master.Title = "剔除玩家";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定剔除德州房间【" + Room_Model.DRName + "】的 [在线玩家] 数据吗？");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("【" + Room_Model.DRName + "】的 [在线玩家] 将被全部剔除 。<br />&nbsp;注意：已押下的酷币将无法返还，请注意操作！<br /><br />");
            builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx?act=KickOnline&amp;info=ok&amp;id=" + RoomID) + "\">确定剔除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            new BCW.dzpk.BLL.DzpkPlayRanks().DeleteByRmID(RoomID);
            Utils.Success("剔除玩家", "操作成功..", Utils.getUrl("dzpk.aspx?act=PGame&amp;GKeyStr=" + dz.SetRoomID(RoomID.ToString())), "1");
        }
    }
    #endregion

    #region 剔除所有在线玩家 KickOnlineALLPage()
    /// <summary>
    /// 剔除所有在线玩家
    /// </summary>
    private void KickOnlineALLPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "")
        {
            Master.Title = "剔除所有玩家";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定[剔除]德州房间 【所有在线】玩家吗？");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("重置后将剔除所有在线的玩家（房间不清空），已押下的酷币将无法返还，请注意操作！<br /><br />");
            builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx?act=KickOnlineALL&amp;info=ok") + "\">确定剔除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            new BCW.Data.SqlUp().ClearTable("tb_DzpkPlayRanks");
            Utils.Success("剔除所有玩家", "操作成功..", Utils.getUrl("dzpk.aspx"), "1");
        }
    }
    #endregion    

    #region 重置手续费 ClrRoom
    /// <summary>
    /// 重置手续费
    /// </summary>
    private void ClrRoom()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int RoomID = int.Parse(Utils.GetRequest("id", "all", 1, "", ""));
        BCW.dzpk.Model.DzpkRooms Room_Model = new BCW.dzpk.BLL.DzpkRooms().GetDzpkRooms(RoomID);

        if (info == "")
        {
            Master.Title = "重置游戏";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定重置德州房间 【" + Room_Model.DRName + "】 的 [手续费] 数据吗？");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("【"+Room_Model.DRName + "】的手续费现在总额是" + Utils.ConvertGold(Room_Model.GSerChargeALL) + ub.Get("SiteBz") + "<br />");
            builder.Append("&nbsp;&gt;&gt;重置后将扣币统计将恢复为 [0] 状态，请注意操作！<br /><br />");
            builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx?act=ClrRoom&amp;info=ok&amp;id=" + RoomID) + "\">确定重置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            Room_Model.GSerChargeALL = 0;
            new BCW.dzpk.BLL.DzpkRooms().Update(Room_Model);
            Utils.Success("重置游戏", "重置【手续费】成功..", Utils.getUrl("dzpk.aspx?act=PGame&amp;GKeyStr=" + dz.SetRoomID(RoomID.ToString())), "1");
        }
    }
    #endregion

    #region 删除房间页 DelPage()
    private void DelPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.dzpk.Model.DzpkRooms drms_model = new BCW.dzpk.BLL.DzpkRooms().GetDzpkRooms(id);

        if (info == "")
        {
            Master.Title = "删除德州扑克房间:"+drms_model.DRName;
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("如果房间有玩家，房间队列数据将被删除，已下注将无法返还，确定删除【" + drms_model.DRName + "】吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx?info=ok&amp;act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx?act=newroom&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.dzpk.BLL.DzpkRooms().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            else
            {
                new BCW.dzpk.BLL.DzpkPlayRanks().DeleteByRmID(id);
                new BCW.dzpk.BLL.DzpkRooms().Delete(id);
                Utils.Success("删除房间", "删除成功..", Utils.getPage("dzpk.aspx"), "1");
            }
        }
    }

    #endregion

    #region 【游戏页】 PGamePage()
    private void PGamePage()
    {
        #region 进入房间开始
        //判断ID
        string GKeyStr = Utils.GetRequest("GKeyStr", "all", 1, @"^[^\^]{1,20}$", "");
        int GKeyID = dz.GetRoomID(GKeyStr);
        if (GKeyID < 0) { Utils.Error("房间ID有误，请重新登入", Utils.getUrl("dzpk.aspx")); }

        //获得当前房间model
        BCW.dzpk.Model.DzpkRooms DzpkRoom = new BCW.dzpk.BLL.DzpkRooms().GetDzpkRooms(GKeyID);
        
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("..\\default.aspx") + "\">管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx") + "\">" + ub.GetSub("DzpkName", xmlPath) + "管理</a>&gt;");
        builder.Append(DzpkRoom.DRName + "[管理]");
        builder.Append(Out.Tab("</div>", "<br />"));
        #endregion
        
        #region 房间基础信息显示
        //获得全部队列信息
        DataTable dt_Ranks = new BCW.dzpk.BLL.DzpkPlayRanks().GetList("*", "Rmid=" + DzpkRoom.ID.ToString() + " ORDER BY RankChk").Tables[0];
        //更新房间信息
        DzpkRoom = new BCW.dzpk.BLL.DzpkRooms().GetDzpkRooms(GKeyID);

        //显示房间信息
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("【" + dz.getDRTypeName(DzpkRoom.DRType) + "房间:" + DzpkRoom.DRName.ToString() + "】");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Div("div", "在线玩家:" + dt_Ranks.Rows.Count + "/" + DzpkRoom.GMaxUser + "<br />奖池金额:" + Utils.ConvertGold(DzpkRoom.GSidePot) + ub.Get("SiteBz") + " 【手续费:" + DzpkRoom.GSerCharge + "‰】<br />大小盲注:" + Utils.ConvertGold(DzpkRoom.GSmallb) + ub.Get("SiteBz") + "/" + Utils.ConvertGold(DzpkRoom.GBigb) + ub.Get("SiteBz") + "<br />下注限额:" + (DzpkRoom.GMinb) + ub.Get("SiteBz") + "/" + Utils.ConvertGold(DzpkRoom.GMaxb) + ub.Get("SiteBz") + "<br />当前流程:" + DzpkRoom.GActID.ToString()));
        #endregion

        #region 玩家列表

        //游戏信息
        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("【玩家队列】 <a href=\"" + Utils.getUrl("dzpk.aspx?act=PGame&amp;GKeyStr=" + GKeyStr) + "\">刷新</a>");
        builder.Append(Out.Tab("</div>", ""));

        string RankMake = ""; //当前出牌者


        builder.Append(Out.Tab("<div>", "<br />"));
        List<Winner> WinnerList = new List<Winner>();

        if (dt_Ranks.Rows.Count > 0)
        {

            for (int i = 0; i < dt_Ranks.Rows.Count; i++)
            {
                BCW.dzpk.Model.DzpkPlayRanks Ranks = new BCW.dzpk.BLL.DzpkPlayRanks().GetDzpkPlayRanks(int.Parse(dt_Ranks.Rows[i]["ID"].ToString()));

                #region 判断出牌者
                //是否为出牌者
                if (Ranks.RankMake == UsAction.US_RANKMAKE)
                {
                    RankMake = "☛&ensp;";                   
                }
                else
                {
                    RankMake = "&emsp;&ensp;";
                }
                #endregion

                #region 输出到界面，显示牌型
                switch (i)
                {
                    case 0:
                        {
                            //庄家 标记为D
                            builder.Append(RankMake + +(i + 1) + "." + "(庄)" + new BCW.BLL.User().GetUsName(Ranks.UsID));
                        }; break;
                    case 1:
                        {
                            //小盲 标记为SB
                            builder.Append("<br />" + RankMake + (i + 1) + "." + "(小)" + new BCW.BLL.User().GetUsName(Ranks.UsID));
                        }; break;
                    case 2:
                        {
                            //大盲 标记为DB
                            builder.Append("<br />" + RankMake + (i + 1) + "." + "(大)" + new BCW.BLL.User().GetUsName(Ranks.UsID));
                        }; break;
                    default:
                        {
                            builder.Append("<br />" + RankMake + (i + 1) + "." + "" + new BCW.BLL.User().GetUsName(Ranks.UsID));
                        }; break;
                }
                string Cb = "";
                if (Ranks.RmMake.Contains("(测)"))
                {
                    Cb = ub.GetSub("DzCoin", xmlPath);
                }
                else {
                    Cb = ub.Get("SiteBz");
                }

                builder.Append("  [" + Utils.ConvertGold(Ranks.UsPot) + Cb + "]");
                builder.Append("<br />&emsp;&emsp;&ensp;" + ShowPokerByUs(Ranks, DzpkRoom));
                builder.Append("<br />&emsp;&emsp;&ensp;" + ShowChipByUs(Ranks));
                #endregion
            }
        }
        else
        {
            builder.Append("暂无玩家");
        }
        #endregion
        
        #region 游戏提示及操作部分
        builder.Append(Out.Hr());

        builder.Append(Out.Tab("</div>", "<br />"));
        #endregion
        
        #region 操作记录
        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("【操作记录】");
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
        builder.Append(ShowHisGame(DzpkRoom, 0, 1));
        builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx?act=HisGame&amp;GKeyStr=" + dz.SetRoomID(DzpkRoom.ID.ToString())) + "\">更多...</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        #endregion

        #region 底部编辑
        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("【房间操作】");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx?act=ClrRoom&amp;id=" + DzpkRoom.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">清房间手续费</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx?act=KickOnline&amp;id=" + DzpkRoom.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">清在线玩家(全踢)</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx?act=ResetGame&amp;id=" + DzpkRoom.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">复位房间(不踢)</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx?act=del&amp;id=" + DzpkRoom.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">删除房间</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx?act=newroom&amp;id=" + DzpkRoom.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">编辑房间</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx") + "\">返回上一层</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        #endregion
    }
    #endregion

    #region 创建或修改房间页 ViewRoomPage()
    private void ViewRoomPage() {
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 1, @"^[0-9]\d*$", "-1"));
        int DRType = int.Parse(Utils.GetRequest("DRType", "get", 1, @"^[0-9]\d*$", "0"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("..\\default.aspx") + "\">管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("..\\game\\dzpk.aspx") + "\">" + ub.GetSub("DzpkName", xmlPath) + "管理</a>&gt;");

        //设置XML地址
        ub xml = new ub();
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置

        //房间model
        BCW.dzpk.Model.DzpkRooms dpr_model = null;

        if (id <= 0)
        {
            builder.Append("创建房间");
            builder.Append(Out.Tab("</div>", "<br />"));
            dpr_model = new BCW.dzpk.Model.DzpkRooms();
        }
        else
        {
            dpr_model = new BCW.dzpk.BLL.DzpkRooms().GetDzpkRooms(id);
            builder.Append("编辑房间");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Div("div", "当前房间：" + dpr_model.DRName));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        }

        switch (Utils.ToSChinese(ac))
        {
            #region 确定添加 处理
            case "确定添加":
                {
                    string RoomName = Utils.GetRequest("RoomName", "post", 2, @"^[^\^]{1,20}$", "系统名称限1-20字内");
                    int RmType = int.Parse(Utils.GetRequest("RmType", "post", 2, @"^[0-9]\d*$", "房间类型 选择出错"));
                    string PassWD = Utils.GetRequest("PassWD", "post", 1, @"^[0-9]\d*$", "");
                    string GSerCharge= Utils.GetRequest("GSerCharge", "post", 1, @"^[0-9]\d*$", "费率 填写出错");

                    int GSmallb = int.Parse(xml.dss["GSmallb"].ToString().Split('|')[RmType]);
                    int GBigb = int.Parse(xml.dss["GBigb"].ToString().Split('|')[RmType]);
                    int GMinb = int.Parse(xml.dss["GMinb"].ToString().Split('|')[RmType]);
                    int GMaxb = int.Parse(xml.dss["GMaxb"].ToString().Split('|')[RmType]);
                    int GMaxUser = int.Parse(xml.dss["GMaxUser"].ToString().Split('|')[RmType]);
                    int GsetTime = int.Parse(xml.dss["GsetTime"].ToString().Split('|')[RmType]);

                    dpr_model.DRName = RoomName;
                    dpr_model.DRType = RmType;
                    dpr_model.PassWD = PassWD;
                    dpr_model.GSmallb = GSmallb;
                    dpr_model.GBigb = GBigb;
                    dpr_model.GMinb = GMinb;
                    dpr_model.GMaxb = GMaxb;
                    dpr_model.GMaxUser = GMaxUser;
                    dpr_model.GSetTime = GsetTime;
                    dpr_model.GSerCharge = long.Parse(GSerCharge);
                    dpr_model.GActID = 1;
                    dpr_model.GActBetID = BCW.dzpk.UsAction.US_ROOM_FREE;
                    dpr_model.LastTime = DateTime.Now;
                    new BCW.dzpk.BLL.DzpkRooms().Add(dpr_model);
                    Utils.Success(ub.GetSub("DzpkName", xmlPath) + "游戏设置", "房间添加成功，正在返回..", Utils.getUrl("dzpk.aspx?act=PGame&amp;GKeyStr=" + dz.SetRoomID(id.ToString())), "1");
                }; break;
            #endregion

            #region 确定修改 处理
            case "确定修改":
                {
                    string RoomName = Utils.GetRequest("RoomName", "post", 2, @"^[^\^]{1,20}$", "系统名称限1-20字内");
                    int RmType = int.Parse(Utils.GetRequest("RmType", "post", 2, @"^[0-9]\d*$", "房间类型 选择出错"));
                    string PassWD = Utils.GetRequest("PassWD", "post", 1, @"^[0-9]\d*$", "");
                    string GSerCharge = Utils.GetRequest("GSerCharge", "post", 1, @"^[0-9]\d*$", "费率 填写出错");

                    int GSmallb = int.Parse(xml.dss["GSmallb"].ToString().Split('|')[RmType]);
                    int GBigb = int.Parse(xml.dss["GBigb"].ToString().Split('|')[RmType]);
                    int GMinb = int.Parse(xml.dss["GMinb"].ToString().Split('|')[RmType]);
                    int GMaxb = int.Parse(xml.dss["GMaxb"].ToString().Split('|')[RmType]);
                    int GMaxUser = int.Parse(xml.dss["GMaxUser"].ToString().Split('|')[RmType]);
                    int GsetTime = int.Parse(xml.dss["GsetTime"].ToString().Split('|')[RmType]);

                    dpr_model.DRName = RoomName;
                    dpr_model.DRType = RmType;
                    dpr_model.PassWD = PassWD;
                    dpr_model.GSmallb = GSmallb;
                    dpr_model.GBigb = GBigb;
                    dpr_model.GMinb = GMinb;
                    dpr_model.GMaxb = GMaxb;
                    dpr_model.GMaxUser = GMaxUser;
                    dpr_model.GSetTime = GsetTime;
                    dpr_model.GSerCharge = long.Parse(GSerCharge);

                    new BCW.dzpk.BLL.DzpkRooms().Update(dpr_model);
                    Utils.Success(ub.GetSub("DzpkName", xmlPath) + "游戏设置", "房间修改成功，正在返回..", Utils.getUrl("dzpk.aspx?act=PGame&amp;GKeyStr=" + dz.SetRoomID(id.ToString())), "1");
                }; break;
            #endregion

            #region 默认提交表单
            default:
                {
                    string strText = "房间名称:/,房间类型:/,房间密码(可选 限四位数字):/,费率(‰):/,";
                    string strName = "RoomName,RmType,PassWD,GSerCharge,backurl";
                    string strType = "text,select,text,text,hidden";
                    string strValu = "";
                    string strOthe = "";

                    if (id <= 0)
                    {
                        strValu = "" + new BCW.dzpk.BLL.DzpkRooms().GetMaxId() + xml.dss["RUnits"] + "'" + DRType + "''" + xml.dss["GSerCharge"].ToString().Split('|')[DRType] + "'" + Utils.getPage(0) + "";
                        strOthe = "确定添加|reset,dzpk.aspx?act=newroom&amp;backurl=" + Utils.PostPage(1) + ",post,1,red|blue";
                    }
                    else
                    {
                        strValu = "" + dpr_model.DRName + "'" + dpr_model.DRType + "'" + dpr_model.PassWD + "'" + xml.dss["GSerCharge"].ToString().Split('|')[DRType] + "'" + Utils.getPage(0) + "";
                        strOthe = "确定修改|reset,dzpk.aspx?act=newroom&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + ",post,1,red|blue";
                    }

                    string strRoomType = "";

                    for (int i = 0; i < xml.dss["RmType"].ToString().Split('|').Length; i++)
                    {
                        if (i > 0) { strRoomType += "|"; }
                        strRoomType += i + "|" + xml.dss["RmType"].ToString().Split('|')[i].ToString();
                    }

                    string strEmpt = "false," + strRoomType + ",false,false";
                    string strIdea = "/";

                    builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

                    builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
                    builder.Append(Out.Tab("<div>", ""));

                    if (id > 0) { builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx?act=del&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">删除房间</a><br />"); }
                    builder.Append("<a href=\"" + Utils.getPage("") + "\">返回上级</a><br />");
                    builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
                    builder.Append(Out.Tab("</div>", "<br />"));
                }; break;
                #endregion
        }
    }
    #endregion

    #region 游戏历史 HisGamePage
    private void HisGamePage()
    {
        Master.Title = "德州扑克管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("..\\default.aspx") + "\">管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx") + "\">德州扑克管理</a>&gt;");
        builder.Append("游戏历史");
        builder.Append(Out.Tab("</div>", "<br />"));

        #region 游戏历史
        string gs = "";     //SQL搜索判断
        string Cb = "";     //币名
        string gstate = ""; //房间状态
        if (ub.GetSub("GameStatus", xmlPath) == "2")
        {
            Cb = ub.GetSub("DzCoin", xmlPath);
            gs = " RmMake like '%(测)%'";
            gstate = "(测试)";
        }
        else
        {
            Cb = ub.Get("SiteBz");
            gs = " RmMake not like '%(测)%'";
        }
        #endregion

        #region 接收到的页面参数处理
        string strWhere = string.Empty;
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

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
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
        Master.Title = "德州扑克管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("..\\default.aspx") + "\">管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx") + "\">德州扑克管理</a>&gt;");
        builder.Append(r + "详细记录");
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
                    else {
                        string[] Cards = ActList[i].MaxCard.Split(',');
                        builder.Append(" " + Card.ShowPokerByStr(Cards) + " ");
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
                else {
                    Cb = ub.Get("SiteBz");
                }
                //过牌 弃牌不显示金额
                if (ActList[i].ActMake == UsAction.AC_FOLD || ActList[i].ActMake == UsAction.AC_CHECK || ActList[i].ActMake == UsAction.AC_TIMEOUT_STR)
                {
                    builder.Append(" [" + ActList[i].ActMake + "] ");
                }
                else {
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
        builder.Append("<a href=\"" + Utils.getUrl("dzpk.aspx?act=notes") + "\">返回记录列表</a>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
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
        string Roomid = "";
        if (DzpkRoom != null) {
            Roomid = "RmID=" + DzpkRoom.ID.ToString() + " AND ";
        }

        DataTable HisData = new BCW.dzpk.BLL.DzpkHistory().GetList("*", Roomid + "(RmMake <> '') ORDER BY TimeOut DESC").Tables[0];

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
                            r += num+ HisModel.RmMake + " ID:" + HisModel.UsID + "&emsp;" + Card.ShowPokerByStr(HandNum[0].Split(','));

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
                            string Cb = "";
                            if (HisModel.RmMake.Contains("(测)"))
                            {
                                Cb = ub.GetSub("DzCoin", xmlPath);
                            }
                            else {
                                Cb = ub.Get("SiteBz");
                            }

                            if (!string.IsNullOrEmpty(HisModel.PokerChips))
                            {
                                r += " 投:" + dzpk.GetChipTotle(HisModel.PokerChips) + Cb;
                            }
                            r += " 获:" + HisModel.GetMoney + Cb + " 时间：" + HisModel.TimeOut.ToString();
                            if (i < HisData.Rows.Count - 1)
                            {
                                r += "<br />";
                            }
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
                            strHtml += Card.toHtml(Cards[i]);
                        }
                    }
                }
            }
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
        DataTable ActData = new BCW.dzpk.BLL.DzpkAct().GetList("*", "RmID=" + DzpkRoom.ID.ToString() + " ORDER BY ActTime DESC").Tables[0];

        if (ActData.Rows.Count <= 0)
        {
            r += "暂无对局信息";
        }
        else
        {
            for (int i = 0; i < ActData.Rows.Count; i++)
            {
                if (num > iNum) break;
                BCW.dzpk.Model.DzpkAct ActModel = new BCW.dzpk.BLL.DzpkAct().GetDzpkAct(int.Parse(ActData.Rows[i]["ID"].ToString()));
                r += num + ":" + ActModel.RmMake + " " + new BCW.BLL.User().GetUsName(ActModel.UsID);
                if (ActModel.ActMake == UsAction.AC_GETGOLD)
                {
                    if (ActModel.MaxCard.Split('|').Length == 2)
                    {
                        r += " " + Card.ShowPokerByStr(ActModel.MaxCard.Split('|')[1].Split(',')) + " ";
                    }
                }
                if (ActModel.ActMake == UsAction.AC_RAISE)
                {
                    r += "<font color=red>";
                }
                if (ActModel.ActMake == UsAction.AC_GETGOLD || ActModel.ActMake == UsAction.AC_CALL)
                {
                    r += "<font color=green>";
                }
                string Cb = "";
                if (ActModel.RmMake.Contains("(测)"))
                {
                    Cb = ub.GetSub("DzCoin", xmlPath);
                }
                else {
                    Cb = ub.Get("SiteBz");
                }
                r += " [" + ActModel.ActMake + "=>" + ActModel.Money + "" + Cb + "] ";
                if (ActModel.ActMake == UsAction.AC_RAISE || ActModel.ActMake == UsAction.AC_GETGOLD || ActModel.ActMake == UsAction.AC_CALL)
                {
                    r += "</font>";
                }
                r += ActModel.ActTime.ToString() + "<br />";
                num++;
            }
        }
        return r;
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
                        string Cb = "";
                        if (HisModel.RmMake.Contains("(测)"))
                        {
                            Cb = ub.GetSub("DzCoin", xmlPath);
                        }
                        else {
                            Cb = ub.Get("SiteBz");
                        }
                        r += Cb;
                    }
                }
            }
        }
        return r;
    }
    #endregion
}
