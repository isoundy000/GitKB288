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
using BCW.Baccarat;

public partial class bbs_game_bjl : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/bjl.xml";
    protected long shaoPay = Convert.ToInt64(ub.GetSub("baccaratLowerPay", "/Controls/bjl.xml")); //开庄的最低要求
    protected long HigherPay = Convert.ToInt64(ub.GetSub("baccaratHigherPay", "/Controls/bjl.xml")); //开庄的最高投入
    protected int RoomTime1 = Convert.ToInt32(ub.GetSub("baccaratRoomTime1", "/Controls/bjl.xml")); //封庄的最低局数
    protected long RoomTime2 = Convert.ToInt64(ub.GetSub("baccaratRoomTime2", "/Controls/bjl.xml")); //自动封庄的局数
    protected string textID = (ub.GetSub("textID", "/Controls/bjl.xml"));//试玩ID
    protected string GameName = ub.GetSub("baccaratName", "/Controls/bjl.xml");//游戏名字
    protected int kaizhuang = Convert.ToInt32(ub.GetSub("kaizhuang", "/Controls/bjl.xml"));//开庄数量限制
    protected int shouxufei = Convert.ToInt32(ub.GetSub("shouxufei", "/Controls/bjl.xml")); //手续费
    protected string baccarat_img = ub.GetSub("baccarat_img", "/Controls/bjl.xml");//图片路径
    protected int Times = int.Parse(ub.GetSub("PokerTimes", "/Controls/bjl.xml"));//开牌时间

    protected void Page_Load(object sender, EventArgs e)
    {
        if (ub.GetSub("Status", xmlPath) == "1")//当status=1时，进行游戏维护
        {
            Utils.Safe("此游戏");
        }

        if (ub.GetSub("Status", xmlPath) == "2")//当status=2时，游戏内侧
        {
            int meid = new BCW.User.Users().GetUsId();
            if (meid == 0)
                Utils.Login();
            if (textID != "")
            {
                string[] sNum = textID.Split('#');
                int sbsy = 0;
                for (int a = 0; a < sNum.Length; a++)
                {
                    int tid = 0;
                    int.TryParse(sNum[a].Trim(), out tid);
                    if (meid == tid)
                    {
                        sbsy++;
                    }
                }
                if (sbsy == 0)
                {
                    Utils.Error("你没获取测试此游戏的资格，请与客服联系，谢谢。", "");
                }
            }
        }

        updatetable();//开奖更新

        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "case":
                CasePage();//---------------开始兑奖页面
                break;
            case "caseok":
                CaseOkPage();//-------------单个兑奖
                break;
            case "casepost":
                CasePostPage();//-----------多个兑奖
                break;
            case "addRoom":
                addRoomPage();//开庄
                break;
            case "taimian":
                taimianPage();//桌面
                break;
            case "history":
                historyPage();//所有桌面
                break;
            case "fangjian":
                fangjianPage();//房间
                break;
            case "setroom":
                setroomPage();//最高最低下注设置
                break;
            case "pay":
                payPage();//下注确认
                break;
            case "mybet":
                MyBetPage();//我的下注记录
                break;
            case "rule":
                RulePage(); //游戏规则
                break;
            case "tablelist":
                tablelistPage();//查看牌局详细
                break;
            case "gonggao":
                gonggaoPage();//我的公告查看与修改
                break;
            case "ggshow":
                ggshowPage();//公告查看
                break;
            case "zhuijia":
                zhuijiaPage();//追加彩池
                break;
            case "diary":
                diaryPage();//日志显示
                break;
            case "fzhuang":
                fzhuangPage();//封庄
                break;
            case "top":
                TopPage();//游戏排行界面
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        //if (meid == 0)
        //    Utils.Login();
        if (meid > 0)
        {
            if (!new BCW.Baccarat.BLL.BJL_user().Exists(meid))
            {
                BCW.Baccarat.Model.BJL_user user = new BCW.Baccarat.Model.BJL_user();
                user.usid = meid;
                user.setshow = 0;
                user.kainum = 0;
                new BCW.Baccarat.BLL.BJL_user().Add(user);//新增usid
                Utils.Success("欢迎新玩家", "欢迎来到" + GameName + ".<b>我决定要成为一代赌神!</b>", Utils.getUrl("bjl.aspx"), "2");
            }
        }

        Master.Title = GameName;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;" + GameName + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        string Note = ub.GetSub("baccarattop", xmlPath);//头部ubb
        if (Note != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(Note)) + "");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        string gif2 = ub.GetSub("baccaratLogo", xmlPath);//logo
        if (gif2 != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<img height=\"70px\" width=\"200px\" src=\"" + gif2 + "\" alt=\"load\"/><br/>");
            builder.Append(Out.Tab("</div>", ""));
        }

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=taimian") + "\">我的桌面</a>|<a href=\"" + Utils.getUrl("bjl.aspx?act=history") + "\">所有桌面</a>|<a href=\"" + Utils.getUrl("bjl.aspx?act=mybet") + "\">历史下注</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=rule") + "\">游戏规则</a>|<a href=\"" + Utils.getUrl("bjl.aspx?act=top") + "\">游戏排行</a>|<a href=\"" + Utils.getUrl("bjl.aspx?act=case") + "\">我要兑奖</a>");
        builder.Append(Out.Tab("</div>", ""));


        #region 最新桌面
        int showtype = int.Parse(Utils.GetRequest("showtype", "get", 1, @"^[0-2]$", "0"));
        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("【最新桌面】<a href=\"" + Utils.getUrl("bjl.aspx?act=addRoom") + "\"><b  style=\"color:red\">我要开庄>></b></a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        string strWhere = "state=0";
        string strOrder = "";
        if (showtype == 0)
            strOrder = "Total_Now DESC,Click DESC";//彩池：大>小
        else if (showtype == 1)
            strOrder = "Click DESC,Total_Now DESC";//人气
        else
            strOrder = "Total DESC,LowTotal DESC";//下注额:大>小

        string[] pageValUrl = { "act", "ptype", "showtype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
        IList<BCW.Baccarat.Model.BJL_Room> listBaccarat = new BCW.Baccarat.BLL.BJL_Room().GetBJL_Rooms(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listBaccarat.Count > 0)
        {
            int k = 1;
            foreach (BCW.Baccarat.Model.BJL_Room n in listBaccarat)
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
                if (showtype == 0)
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("bjl.aspx?act=fangjian&amp;roomid=" + n.ID + "") + "\">" + n.ID + "号桌</a>(彩池:" + n.Total_Now + "/人气:" + n.Click + ")");
                if (showtype == 1)
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("bjl.aspx?act=fangjian&amp;roomid=" + n.ID + "") + "\">" + n.ID + "号桌</a>(人气:" + n.Click + "/彩池:" + n.Total_Now + ")");
                if (showtype == 2)
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("bjl.aspx?act=fangjian&amp;roomid=" + n.ID + "") + "\">" + n.ID + "号桌</a>(最小:" + n.LowTotal + "/最大:" + n.BigPay + ")");
                if (new BCW.Baccarat.BLL.BJL_Play().Exists_wj(n.ID))
                {
                    builder.Append("[正在进行中..]");
                }

                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            if (recordCount > 10)
            {
                builder.Append(Out.Tab("<div >", "<br/>"));
                builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=history") + "\">更多桌面>></a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录..."));
        }
        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        builder.Append("排序:");
        builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?showtype=0") + "\">彩池</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?showtype=1") + "\">人气</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?showtype=2") + "\">下注额</a> ");
        builder.Append(Out.Tab("</div>", ""));
        #endregion

        #region 玩家动态
        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("【玩家动态】");
        builder.Append(Out.Tab("</div>", ""));

        // 开始读取动态列表
        int SizeNum_Action = 3;
        string strWhere_Action = "Types=1009";
        IList<BCW.Model.Action> listAction = new BCW.BLL.Action().GetActions(SizeNum_Action, strWhere_Action);
        if (listAction.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Action n in listAction)
            {
                builder.Append(Out.Tab("<div>", "<br />"));
                string ForNotes = Regex.Replace(n.Notes, @"\[url=\/bbs\/game\/([\s\S]+?)\]([\s\S]+?)\[\/url\]", "$2", RegexOptions.IgnoreCase);
                builder.AppendFormat("{0}前{1}", DT.DateDiff2(DateTime.Now, n.AddTime), Out.SysUBB(ForNotes));
                builder.Append(Out.Tab("</div>", ""));
                k++;
            }
            if (k > SizeNum_Action)
            {
                builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("/bbs/action.aspx?ptype=1009") + "\">更多动态&gt;&gt;</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("暂无动态记录.");
            builder.Append(Out.Tab("</div>", ""));
        }
        #endregion

        //闲聊显示
        builder.Append(Out.SysUBB(BCW.User.Users.ShowSpeak(22, "bjl.aspx", 5, 0)));
        foot();
    }

    //开庄
    private void addRoomPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);
        long price = new BCW.BLL.User().GetGold(meid);//自己的" + ub.Get("SiteBz") + "

        Master.Title = "" + GameName + "_我要开庄";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx") + "\">" + GameName + "</a>&gt;我要开庄");
        builder.Append(Out.Tab("</div>", ""));

        string info = Utils.GetRequest("info", "post", 1, "", "");
        if (info == "ok2")
        {
            int Total = int.Parse(Utils.GetRequest("Total", "all", 2, @"^[0-9]\d*$", "彩池填写不能为空"));
            int low = int.Parse(Utils.GetRequest("low", "all", 2, @"^[0-9]\d*$", "最低下注不能为空"));
            int BigPay = int.Parse(Utils.GetRequest("BigPay", "all", 2, @"^[0-9]\d*$", "最高下注不能为空"));

            //个人开庄数量>开庄限制
            if (new BCW.Baccarat.BLL.BJL_Room().Get_kz_num(meid, 0) >= kaizhuang)
                Utils.Error("你目前的开庄次数已满，请把前面的桌面进行封庄处理！", "");

            if (Total > price)
            {
                Utils.Error("您的" + ub.Get("SiteBz") + "不足", "");
            }
            else if (Total < shaoPay || Total > HigherPay)
            {
                Utils.Error("开庄奖池为： " + shaoPay + "-" + HigherPay + "", "");
            }
            else if (low > BigPay)
            {
                Utils.Error("最低下注不可以高于最高下注,请重新填写.", "");
            }
            else
            {
                //支付安全提示
                string[] p_pageArr = { "act", "low", "BigPay", "Total", "info", };
                BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr);
                BCW.User.Users.IsFresh("bjl", 2);//防刷

                //新增房间
                BCW.Baccarat.Model.BJL_Room room = new BCW.Baccarat.Model.BJL_Room();
                room.AddTime = DateTime.Now;
                room.UsID = meid;
                room.Total = Total;
                room.LowTotal = low;
                room.Title = "";
                room.contact = "";
                room.state = 0;
                room.zhui_Total = 0;
                room.Total_Now = Total;
                room.shouxufei = 0;
                room.BigPay = BigPay;
                int roomid = new BCW.Baccarat.BLL.BJL_Room().Add(room);

                BCW.Baccarat.Model.BJL_user user = new BCW.Baccarat.Model.BJL_user();
                if (new BCW.Baccarat.BLL.BJL_user().Exists_user(meid))
                {
                    new BCW.Baccarat.BLL.BJL_user().update_zd("kainum=kainum+1", "usid=" + meid + "");//更新开庄次数
                }
                else
                {
                    user.usid = meid;
                    user.setshow = 0;
                    user.kainum = 1;
                    new BCW.Baccarat.BLL.BJL_user().Add(user);//新增usid
                }
                new BCW.BLL.User().UpdateiGold(meid, mename, -Total, "在" + GameName + "开桌成功,花费了" + Total + "-标识ID" + roomid + "");
                //动态
                string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/bjl.aspx]" + GameName + "[/url]开桌成功.";
                new BCW.BLL.Action().Add(1009, roomid, meid, "", wText);
                Utils.Success("温馨提示", "恭喜，开庄成功，祝君好运！正在进入房间...", Utils.getUrl("bjl.aspx?act=fangjian&amp;roomid=" + roomid + ""), "2");
            }
        }
        else
        {
            string strText = "桌面彩池：/,最低下注：/,最高下注：/,,,";
            string strName = "Total,low,BigPay,act,info";
            string strType = "text,text,text,hidden,hidden";
            string strValue = "'''addRoom'ok2";
            string strEmpt = "true,true,true,false,false";
            string strIdea = "/";
            string strOthe = "立刻开台,bjl.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValue, strEmpt, strIdea, strOthe));
        }

        //开庄的注意事项设置
        DataSet num = new BCW.Baccarat.BLL.BJL_Room().GetList("COUNT(*) AS a", "state=0 AND UsID=" + meid + "");
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("开庄提示：<br />");
        builder.Append("1、您目前自带" + new BCW.BLL.User().GetGold(meid) + "酷币.你还可以开" + (kaizhuang - int.Parse(num.Tables[0].Rows[0]["a"].ToString())) + "个庄.<br />");
        builder.Append("2、当彩池低于最低投注额时将自动封庄.<br/>");
        builder.Append("3、彩池范围" + shaoPay + "" + ub.Get("SiteBz") + "——" + HigherPay + "" + ub.Get("SiteBz") + ".<br />");
        builder.Append("4、封庄最少需要达到" + RoomTime1 + "局,若达到" + RoomTime2 + "局系统将自动封庄.");
        builder.Append(Out.Tab("</div>", ""));

        foot();
    }

    //桌面
    private void taimianPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);

        int uid = Utils.ParseInt(Utils.GetRequest("uid", "get", 1, @"^[1-9]\d*$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        if (uid != 0)
        {
            meid = uid;
            Master.Title = "" + GameName + "_" + new BCW.BLL.User().GetUsName(meid) + "的桌面";
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx") + "\">" + GameName + "</a>&gt;" + new BCW.BLL.User().GetUsName(meid) + "的桌面");
        }
        else
        {
            Master.Title = "" + GameName + "_我的桌面";
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx") + "\">" + GameName + "</a>&gt;我的桌面");
        }
        builder.Append(Out.Tab("</div>", "<br/>"));

        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[1-2]$", "1"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 1)
            builder.Append("<h style=\"color:red\">进行中</h>|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=taimian&amp;ptype=1&amp;uid=" + uid + "") + "\">进行中</a>|");
        if (ptype == 2)
            builder.Append("<h style=\"color:red\">已结束</h>");
        else
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=taimian&amp;ptype=2&amp;uid=" + uid + "") + "\">已结束</a>");
        builder.Append(Out.Tab("</div>", "<br/>"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
        string[] pageValUrl = { "act", "ptype", "uid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        string strWhere = string.Empty;
        if (ptype == 1)
            strWhere += "UsID=" + meid + " and state=0";
        if (ptype == 2)
            strWhere += "UsID=" + meid + " and state=1";
        string strOrder = "ID Desc";


        //开始读取列表
        IList<BCW.Baccarat.Model.BJL_Room> listplay = new BCW.Baccarat.BLL.BJL_Room().GetBJL_Rooms(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listplay.Count > 0)
        {
            int k = 1;
            foreach (BCW.Baccarat.Model.BJL_Room n in listplay)
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
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("bjl.aspx?act=fangjian&amp;roomid=" + n.ID + "") + "\">" + n.ID + "号桌</a>");
                if (ptype == 1)
                {
                    builder.Append("(彩池:" + n.Total_Now + "/人气:" + n.Click + ")");
                }
                else
                {
                    builder.Append("(已结束)(开庄:" + n.Total + "/封庄:" + n.Total_Now + "/人气:" + n.Click + ")");//(n.Total_Now - n.Total)
                }
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=addRoom") + "\">我要开庄&gt;&gt;</a>");
        builder.Append(Out.Tab("</div>", ""));

        foot();
    }

    //所有桌面
    private void historyPage()
    {
        Master.Title = "" + GameName + "_所有桌面";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx") + "\">" + GameName + "</a>&gt;所有桌面");
        builder.Append(Out.Tab("</div>", "<br/>"));

        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[1-2]$", "1"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 1)
            builder.Append("<h style=\"color:red\">进行中</h>|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=history&amp;ptype=1") + "\">进行中</a>|");
        if (ptype == 2)
            builder.Append("<h style=\"color:red\">已结束</h>");
        else
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=history&amp;ptype=2") + "\">已结束</a>");
        builder.Append(Out.Tab("</div>", "<br/>"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
        string strWhere = string.Empty;
        strWhere = "";
        if (ptype == 1)
            strWhere += " state=0";
        if (ptype == 2)
            strWhere += " state=1";
        string strOrder = "ID Desc";
        string[] pageValurl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //开始读取列表
        IList<BCW.Baccarat.Model.BJL_Room> listplay = new BCW.Baccarat.BLL.BJL_Room().GetBJL_Rooms(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listplay.Count > 0)
        {
            int k = 1;
            foreach (BCW.Baccarat.Model.BJL_Room n in listplay)
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

                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("bjl.aspx?act=fangjian&amp;roomid=" + n.ID + "") + "\">" + n.ID + "号桌");
                if (ptype == 1)
                {
                    builder.Append("(彩池:" + n.Total + "/人气:" + n.Click + ")</a>");
                }
                else
                {
                    builder.Append("(已结束)</a>盈利:" + n.Total_Now + "");//(n.Total_Now - n.Total)
                }
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValurl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }

        foot();
    }

    //房间
    private void fangjianPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int roomid = Utils.ParseInt(Utils.GetRequest("roomid", "get", 1, @"^[1-9]\d*$", "0"));//房间ID
        int Play_Table = 0;
        BCW.Baccarat.Model.BJL_Room play = new BCW.Baccarat.BLL.BJL_Room().GetBJL_Room(roomid);
        if (play.ID == 0)
        {
            Utils.Error("该房间不存在.", "");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        if (meid == play.UsID)
        {
            Master.Title = new BCW.BLL.User().GetUsName(meid) + "的桌面(" + play.ID + ")号桌";
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("bjl.aspx?act=taimian") + "\">我的桌面</a>&gt;我的房间");
        }
        else
        {
            Master.Title = new BCW.BLL.User().GetUsName(play.UsID) + "的桌面(" + play.ID + ")号桌";
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("bjl.aspx?act=taimian&amp;uid=" + play.UsID + "") + "\">Ta的桌面</a>&gt;" + new BCW.BLL.User().GetUsName(play.UsID) + "的房间");
        }
        builder.Append(Out.Tab("</div>", "<br />"));


        builder.Append(Out.Tab("<div>", ""));
        builder.Append("庄家：<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + play.UsID + "") + "\">" + new BCW.BLL.User().GetUsName(play.UsID) + "(" + play.UsID + ")</a><br />");
        if (play.Title == "")
        {
            builder.Append("庄家公告：无");
        }
        else
            builder.Append("庄家公告：<a href=\"" + Utils.getUrl("bjl.aspx?act=ggshow&amp;roomid=" + play.ID + "") + "\">" + play.Title + "</a>");
        builder.Append(Out.Tab("</div>", "<br />"));


        BCW.Baccarat.Model.BJL_Play buy = new BCW.Baccarat.BLL.BJL_Play().GetBJL_Play(roomid);//得到最后一桌的信息
        if (play.state == 0)
        {
            #region 未封庄界面
            if (meid == play.UsID)
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("【庄家操作】<br />");
                builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=gonggao&amp;roomid=" + play.ID + "") + "\" >公告</a>.<a href=\"" + Utils.getUrl("bjl.aspx?act=zhuijia&amp;roomid=" + play.ID + "") + "\">追加</a>.");
                builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=diary&amp;roomid=" + play.ID + "") + "\">日志</a>.<a href=\"" + Utils.getUrl("bjl.aspx?act=fzhuang&amp;roomid=" + play.ID + "") + "\">封庄</a>.");
                builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=setroom&amp;roomid=" + play.ID + "") + "\">设置</a><br/>");
                builder.Append("开始彩池：" + (play.Total) + "" + ub.Get("SiteBz") + "");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            builder.Append(Out.Tab("<div>", ""));
            {
                builder.Append("当前彩池：<h style=\"color:red\">" + new BCW.Baccarat.BLL.BJL_Room().Getcaichi(roomid) + "</h>" + ub.Get("SiteBz") + "<br />");
            }
            builder.Append("桌面局数：第" + play.ID + "桌,");
            if (buy.Play_Table == 0)
            {
                Play_Table = 1;
            }
            else
            {
                Play_Table = buy.Play_Table + 1;
            }
            builder.Append("第" + Play_Table + "局");
            builder.Append("[<a href=\"" + Utils.getUrl("bjl.aspx?act=fangjian&amp;roomid=" + roomid + "") + "\">刷新</a>]<br/>");
            builder.Append("投注限额：最低" + play.LowTotal + "" + ub.Get("SiteBz") + ",最高" + play.BigPay + "" + ub.Get("SiteBz") + "");//new BCW.Baccarat.BLL.BJL_Room().Getcaichi(roomid) / Percent
            builder.Append(Out.Tab("</div>", "<br/>"));

            //获取roomID和table下的最旧下注时间
            //如果无人下注，不显示倒计时
            DateTime Oldbettime = new BCW.Baccarat.BLL.BJL_Play().GetMinBetTime(roomid, (Play_Table));
            if (new BCW.Baccarat.BLL.BJL_Play().Exists(roomid, (Play_Table)))
            {
                BCW.Baccarat.Model.BJL_Card card = new BCW.Baccarat.BLL.BJL_Card().GetCardMessage(roomid, Play_Table);
                BCW.Baccarat.Model.BJL_user userlimits = new BCW.Baccarat.BLL.BJL_user().GetBJL_user(meid);//获得用户设置
                long sum = 0;//下注金额计算
                sum = new BCW.Baccarat.BLL.BJL_Play().GetPrice(roomid, Play_Table);
                builder.Append(Out.Tab("<div>", ""));
                if (Oldbettime.AddSeconds(Times) > DateTime.Now)
                {
                    string daojishi = new BCW.JS.somejs().newDaojishi("divhp3sy6", Oldbettime.AddSeconds(Times));
                    builder.Append("当局已下注" + sum + "" + ub.Get("SiteBz") + "," + daojishi + "后发牌<br/>");
                }
                else if (Oldbettime.AddSeconds((Times + 1)) > DateTime.Now)
                {
                    builder.Append("正在发牌...<br/>");
                }
                else if (Oldbettime.AddSeconds((Times + 8)) > DateTime.Now)
                {
                    //显示发牌
                    string[] hunter = card.HunterPoker.Split(',');
                    string[] banker = card.BankerPoker.Split(',');
                    int bankersum = 0;
                    int huntersum = 0;

                    #region 文字发牌效果
                    if (userlimits.setshow == 0)
                    {
                        if (Oldbettime.AddSeconds((Times + 1)) <= DateTime.Now)
                        {
                            builder.Append("庄家:");
                            if (Oldbettime.AddSeconds((Times + 1)) <= DateTime.Now)
                            {
                                string[] zhuang1 = Poker(banker[0], banker[1], bankersum).Split('#');
                                builder.Append(zhuang1[0] + ",");
                                bankersum = int.Parse(zhuang1[1]);
                            }
                            if (Oldbettime.AddSeconds((Times + 3)) <= DateTime.Now)
                            {
                                string[] zhuang2 = Poker(banker[2], banker[3], bankersum).Split('#');
                                builder.Append(zhuang2[0]);
                                bankersum = int.Parse(zhuang2[1]);
                            }
                            if (Oldbettime.AddSeconds((Times + 5)) <= DateTime.Now)
                            {
                                try
                                {
                                    string[] zhuang3 = Poker(banker[4], banker[5], bankersum).Split('#');
                                    builder.Append("," + zhuang3[0]);
                                    bankersum = int.Parse(zhuang3[1]);
                                }
                                catch { }
                            }
                            builder.Append("<br />");
                            builder.Append("点数为:" + bankersum + "点<br />");
                            builder.Append("闲家:");
                            if (Oldbettime.AddSeconds((Times + 2)) <= DateTime.Now)
                            {
                                string[] xian1 = Poker(hunter[0], hunter[1], huntersum).Split('#');
                                builder.Append(xian1[0] + ",");
                                huntersum = int.Parse(xian1[1]);
                            }
                            if (Oldbettime.AddSeconds((Times + 4)) <= DateTime.Now)
                            {
                                string[] xian2 = Poker(hunter[2], hunter[3], huntersum).Split('#');
                                builder.Append(xian2[0]);
                                huntersum = int.Parse(xian2[1]);
                            }
                            if (Oldbettime.AddSeconds((Times + 6)) <= DateTime.Now)
                            {
                                try
                                {
                                    string[] xian3 = Poker(hunter[4], hunter[5], huntersum).Split('#');
                                    builder.Append("," + xian3[0]);
                                    huntersum = int.Parse(xian3[1]);
                                }
                                catch { }
                            }
                            builder.Append("<br />");
                            builder.Append("点数为:" + huntersum + "点<br />");
                        }
                        builder.Append(Out.Tab("</div>", "<br />"));
                    }
                    #endregion

                    #region 图片发牌显示效果
                    else
                    {
                        if (Oldbettime.AddSeconds((Times + 1)) <= DateTime.Now)
                        {
                            builder.Append("庄家:");
                            if (Oldbettime.AddSeconds((Times + 1)) <= DateTime.Now)
                            {
                                PokerPicture(OnePoker(banker[0], banker[1]));
                                bankersum = PokerPoint(banker[1], bankersum);
                            }
                            if (Oldbettime.AddSeconds((Times + 3)) <= DateTime.Now)
                            {
                                PokerPicture(OnePoker(banker[2], banker[3]));
                                bankersum = PokerPoint(banker[3], bankersum);
                            }
                            if (Oldbettime.AddSeconds((Times + 5)) <= DateTime.Now)
                            {
                                try
                                {
                                    PokerPicture(OnePoker(banker[4], banker[5]));
                                    bankersum = PokerPoint(banker[5], bankersum);
                                }
                                catch { }
                            }
                            builder.Append("(" + bankersum + "点)");
                            builder.Append("<br />");
                            builder.Append("闲家:");
                            if (Oldbettime.AddSeconds((Times + 2)) <= DateTime.Now)
                            {
                                PokerPicture(OnePoker(hunter[0], hunter[1]));
                                huntersum = PokerPoint(hunter[1], huntersum);
                            }
                            if (Oldbettime.AddSeconds((Times + 4)) <= DateTime.Now)
                            {
                                PokerPicture(OnePoker(hunter[2], hunter[3]));
                                huntersum = PokerPoint(hunter[3], huntersum);
                            }
                            if (Oldbettime.AddSeconds((Times + 6)) <= DateTime.Now)
                            {
                                try
                                {
                                    PokerPicture(OnePoker(hunter[4], hunter[5]));
                                    huntersum = PokerPoint(hunter[5], huntersum);
                                }
                                catch { }
                            }
                            builder.Append("(:" + huntersum + "点)");
                        }
                        builder.Append(Out.Tab("</div>", "<br />"));
                    }
                    #endregion

                }
                else
                {
                    builder.Append("第" + Play_Table + "局:庄" + card.BankerPoint + "点,闲" + card.HunterPoint + "点<br/>");
                    //加入发牌到投注表
                    new BCW.Baccarat.BLL.BJL_Play().update_zd("BankerPoker='" + card.BankerPoker + "',HunterPoker='" + card.HunterPoker + "',BankerPoint=" + card.BankerPoint + ",HunterPoint=" + card.HunterPoint + "", "RoomID=" + roomid + " and Play_Table=" + (Play_Table) + "");
                }
            }
            else
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("等待投注：有下注立即进入" + Times + "秒发牌倒计时");
                builder.Append(Out.Tab("</div>", "<br/>"));
            }
            builder.Append(Out.Tab("</div>", ""));
            if (meid != play.UsID)
                multichoice(play.ID, Play_Table);//下注
            history1(roomid);//历史数据
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx") + "\">" + GameName + "</a>");
            builder.Append(Out.Tab("</div>", ""));
            #endregion
        }
        else//已封庄的房间界面
        {
            if (meid == play.UsID)
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("【庄家操作】<br />");
                builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=diary&amp;roomid=" + play.ID + "") + "\">日志</a>.");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("【房间信息】<br />");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            if (meid == play.UsID)
                builder.Append("开始彩池:" + (play.Total) + "<br />");
            builder.Append("结束彩池：" + (play.Total_Now) + "<br />");
            builder.Append("桌面局数：第" + play.ID + "桌,");
            builder.Append("共" + (buy.Play_Table) + "局");
            builder.Append(Out.Tab("</div>", "<br/>"));
            history1(roomid);//历史数据
            foot();
        }

    }

    //设置
    private void setroomPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);

        int roomid = Utils.ParseInt(Utils.GetRequest("roomid", "all", 1, @"^[1-9]\d*$", ""));//房间ID

        BCW.Baccarat.Model.BJL_Room play = new BCW.Baccarat.BLL.BJL_Room().GetBJL_Room(roomid, meid);
        if (play.ID == 0)
            Utils.Error("该房间不存在.", "");


        Master.Title = "" + play.ID + "号桌的设置";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("bjl.aspx?act=taimian") + "\">我的桌面</a>&gt;<a href=\"" + Utils.getUrl("bjl.aspx?act=fangjian&amp;roomid=" + roomid + "") + "\">我的房间</a>&gt;设置");
        builder.Append(Out.Tab("</div>", ""));

        string info = Utils.GetRequest("info", "post", 1, "", "");
        if (info == "")
        {
            string strText = "每注最高下注：/,每注最低下注：/,每局每人最高下注:(0为不限制)/,,,";
            string strName = "BigPay,LowTotal,Bigmoney,roomid,act,info";
            string strType = "num,num,num,hidden,hidden,hidden";
            string strValu = "" + play.BigPay + "'" + play.LowTotal + "'" + play.Bigmoney + "'" + roomid + "'setroom'add";
            string strEmpt = "true,true,true,false,false,false";
            string strIdea = "/";
            string strOthe = "修改设置,bjl.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        else
        {
            if (play.state == 1)
                Utils.Error("该房间已封装,不可以修改.", "");
            BCW.User.Users.IsFresh("baccarat", 2);//是否刷屏
            long BigPay = Utils.ParseInt64(Utils.GetRequest("BigPay", "all", 2, @"^[1-9]\d*$", "最高下注额填写错误"));
            long LowTotal = Utils.ParseInt64(Utils.GetRequest("LowTotal", "all", 2, @"^[1-9]\d*$", "最低下注额填写错误"));
            long Bigmoney = Utils.ParseInt64(Utils.GetRequest("Bigmoney", "all", 2, @"^[0-9]\d*$", "每局每人最高下注额填写错误"));
            if (LowTotal > BigPay)
            {
                Utils.Error("抱歉,最低下注不可以高于最高下注,请重新输入.", "");
            }
            if (BigPay > play.Total_Now)
            {
                Utils.Error("抱歉,修改后的最高下注不可以超过实时奖池,请重新输入.", "");
            }
            if (Bigmoney > 0)
            {
                if (Bigmoney < LowTotal)
                {
                    Utils.Error("抱歉,每局下注额要高于最低下注,请重新输入.", "");
                }
            }

            int roomid1 = Utils.ParseInt(Utils.GetRequest("roomid", "all", 2, @"^[0-9]\d*$", "房间号码错误"));
            new BCW.Baccarat.BLL.BJL_Room().update_zd("BigPay=" + BigPay + ",LowTotal=" + LowTotal + "", "ID=" + roomid1 + "");
            Utils.Success("修改设置", "恭喜您，设置成功.", Utils.getUrl("bjl.aspx?act=fangjian&amp;roomid=" + roomid1 + ""), "1");
        }
        foot();
    }

    //下注选择
    private void multichoice(int roomid, int table)
    {
        string VE = ConfigHelper.GetConfigString("VE");
        string SID = ConfigHelper.GetConfigString("SID");
        double percent1 = Convert.ToDouble(ub.GetSub("baccaratpercent1", xmlPath));//1.98倍
        double percent2 = Convert.ToDouble(ub.GetSub("baccaratpercent2", xmlPath));//1.98倍
        double percent3 = Convert.ToDouble(ub.GetSub("baccaratpercent3", xmlPath));//8倍
        double percent4 = Convert.ToDouble(ub.GetSub("baccaratpercent4", xmlPath));//1.98倍
        double percent5 = Convert.ToDouble(ub.GetSub("baccaratpercent5", xmlPath));//1.98倍
        double percent6 = Convert.ToDouble(ub.GetSub("baccaratpercent6", xmlPath));//1.98倍
        double percent7 = Convert.ToDouble(ub.GetSub("baccaratpercent7", xmlPath));//1.98倍
        double percent8 = Convert.ToDouble(ub.GetSub("baccaratpercent8", xmlPath));//1.58倍
        double percent9 = Convert.ToDouble(ub.GetSub("baccaratpercent9", xmlPath));//2.45倍
        double percent10 = Convert.ToDouble(ub.GetSub("baccaratpercent10", xmlPath));//8倍
        double percent11 = Convert.ToDouble(ub.GetSub("baccaratpercent11", xmlPath));//8倍
        double percent12 = Convert.ToDouble(ub.GetSub("baccaratpercent12", xmlPath));//5倍
        double percent13 = Convert.ToDouble(ub.GetSub("baccaratpercent13", xmlPath));//15倍
        builder.Append("<style>table{border-collapse:collapse;align-text:center;border:solid 1px #d7d7d7;}table tr td{padding:0px 3px;border:solid 1px #d7d7d7;}</style>");
        builder.Append(("<form name=\"form1\" action=\"bjl.aspx\" method=\"post\">"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<table>");
        builder.Append("<tr><td align=\"center\">庄赢</td><td align=\"right\">x" + percent1 + "<input type=\"checkbox\" name=\"vote\" value=\"1\" /> </td>");
        builder.Append("<td align=\"center\">闲赢</td><td align=\"right\">x" + percent2 + "<input type=\"checkbox\" name=\"vote\" value=\"2\" /> </td>");
        builder.Append("<td align=\"center\">和局</td><td align=\"right\">x" + percent3 + "<input type=\"checkbox\" name=\"vote\" value=\"3\" /></td></tr>");

        builder.Append("<tr><td align=\"center\">庄单</td><td align=\"right\">x" + percent4 + "<input type=\"checkbox\" name=\"vote\" value=\"4\" /> </td>");
        builder.Append("<td align=\"center\">庄双</td><td align=\"right\">x" + percent5 + "<input type=\"checkbox\" name=\"vote\" value=\"5\" /></td></tr>");

        builder.Append("<tr><td align=\"center\">闲单</td><td align=\"right\">x" + percent6 + "<input type=\"checkbox\" name=\"vote\" value=\"6\" /> </td>");
        builder.Append("<td align=\"center\">闲双</td><td align=\"right\">x" + percent7 + "<input type=\"checkbox\" name=\"vote\" value=\"7\" /></td></tr>");

        builder.Append("<tr><td align=\"center\">投大</td><td align=\"right\">x" + percent8 + "<input type=\"checkbox\" name=\"vote\" value=\"8\" /> </td>");
        builder.Append("<td align=\"center\">投小</td><td align=\"right\">x" + percent9 + "<input type=\"checkbox\" name=\"vote\" value=\"9\" /></td></tr>");

        builder.Append("<tr><td align=\"center\">庄对</td><td align=\"right\">x" + percent10 + "<input type=\"checkbox\" name=\"vote\" value=\"10\" /> </td>");
        builder.Append("<td align=\"center\">闲对</td><td align=\"right\">x" + percent11 + "<input type=\"checkbox\" name=\"vote\" value=\"11\" /></td></tr>");

        builder.Append("<tr><td align=\"center\">任意对</td><td align=\"right\">x" + percent12 + "<input type=\"checkbox\" name=\"vote\" value=\"12\" /> </td>");
        builder.Append("<td align=\"center\">完美对</td><td align=\"right\">x" + percent13 + "<input type=\"checkbox\" name=\"vote\" value=\"13\" /></td></tr>");
        builder.Append("</table>");

        builder.Append("每项金额：<br />");
        builder.Append("<input maxlength=\"500\" type=\"text\" emptyok=\"true\" name=\"Cent\" /><br />");
        builder.Append("<input type=\"hidden\" name=\"table\" value=\"" + table + "\" />");
        builder.Append("<input type=\"hidden\" name=\"roomid\" value=\"" + roomid + "\" />");
        builder.Append("<input type=\"hidden\" name=\"act\" value=\"pay\" />");
        builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
        builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
        builder.Append("<input class=\"btn-red\" type=\"submit\" name=\"ac1\" value=\"确定下注\" />");//<br />
        //builder.Append("<input class=\"btn-red\" type=\"submit\" name=\"ac2\" value=\"一万\" />");
        //builder.Append(" <input class=\"btn-red\" type=\"submit\" name=\"ac3\" value=\"十万\" />");
        //builder.Append(" <input class=\"btn-red\" type=\"submit\" name=\"ac4\" value=\"二十万\" />");
        //builder.Append(" <input class=\"btn-red\" type=\"submit\" name=\"ac5\" value=\"一百万\" />");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append("</form>");
    }

    //下注判定界面
    private void payPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);
        long gold = new BCW.BLL.User().GetGold(meid);

        int money = 0;
        int money1 = 0;
        int Times = int.Parse(ub.GetSub("PokerTimes", xmlPath));//发牌时间
        string ac1 = Utils.GetRequest("ac1", "all", 1, "", "");
        string ac2 = Utils.GetRequest("ac2", "all", 1, "", "");
        string ac3 = Utils.GetRequest("ac3", "all", 1, "", "");
        string ac4 = Utils.GetRequest("ac4", "all", 1, "", "");
        string ac5 = Utils.GetRequest("ac5", "all", 1, "", "");
        int table = Utils.ParseInt(Utils.GetRequest("table", "post", 2, @"^[1-9]\d*$", "台数错误"));
        int roomid = Utils.ParseInt(Utils.GetRequest("roomid", "post", 2, @"^[1-9]\d*$", "房间号码错误"));
        string info = Utils.GetRequest("info", "post", 1, "", "");
        string vote = Utils.GetRequest("vote", "post", 2, "", "下注类型选择错误");
        if (vote == "")
        {
            Utils.Error("至少选择一项进行投注", "");
        }
        string[] str = vote.Split(',');//将选择的答案分割出来存进数组
        if (ac1 == "确定下注")
        {
            money1 = Utils.ParseInt(Utils.GetRequest("Cent", "post", 2, @"^[1-9]\d*$", "投注额填写错误"));
            money = money1 * str.Length;
        }
        if (ac2 == "一万")
        {
            money1 = 10000;
            money = money1 * str.Length;
        }
        if (ac3 == "十万")
        {
            money1 = 100000;
            money = money1 * str.Length;
        }
        if (ac4 == "二十万")
        {
            money1 = 200000;
            money = money1 * str.Length;
        }
        if (ac5 == "一百万")
        {
            money1 = 1000000;
            money = money1 * str.Length;
        }

        Master.Title = new BCW.BLL.User().GetUsName(meid) + "桌面上第(" + roomid + ")号桌第" + table + "局下注";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("bjl.aspx?act=taimian") + "\">我的桌面</a>&gt;<a href=\"" + Utils.getUrl("bjl.aspx?act=fangjian&amp;roomid=" + roomid + "") + "\">我的房间</a>&gt;我的下注");
        builder.Append(Out.Tab("</div>", "<br />"));

        if (info == "ok2")
        {
            long _money = Utils.ParseInt64(Utils.GetRequest("money", "post", 2, @"^[1-9]\d*$", "总投注额填写错误"));
            long _money1 = Utils.ParseInt64(Utils.GetRequest("money1", "post", 2, @"^[1-9]\d*$", "每注投注额填写错误"));
            int _table = Utils.ParseInt(Utils.GetRequest("table", "post", 2, @"^[1-9]\d*$", "台数错误"));
            int _roomid = Utils.ParseInt(Utils.GetRequest("roomid", "post", 2, @"^[1-9]\d*$", "房间号码错误"));
            string _vote = Utils.GetRequest("vote", "post", 2, "", "下注类型选择错误");

            DateTime Oldbettime = new BCW.Baccarat.BLL.BJL_Play().GetMinBetTime(_roomid, (_table));
            if (Oldbettime.AddSeconds(Times) < DateTime.Now)
            {
                Utils.Error("抱歉,第" + roomid + "桌第" + table + "局已截止下注,正在开奖...", "");
            }

            BCW.Baccarat.Model.BJL_Room play = new BCW.Baccarat.BLL.BJL_Room().GetBJL_Room(_roomid);
            if (play.state == 1)
            {
                Utils.Error("该桌已封庄,请选择其他下注.", "");
            }
            if (play.Bigmoney > 0)//个人每局限投
            {
                DataSet ds = new BCW.Baccarat.BLL.BJL_Play().GetList("sum(PutMoney) AS a", "buy_usid=" + meid + " and Play_Table='" + _table + "' and RoomID=" + _roomid + "");
                long oPrices = 0;
                oPrices = Utils.ParseInt64(ds.Tables[0].Rows[0]["a"].ToString());
                if (oPrices + _money > play.Bigmoney)
                {
                    if (oPrices >= play.Bigmoney)
                        Utils.Error("您本局投注已达上限，请等待下局...", "");
                    else
                        Utils.Error("您本局最多还可以投注" + (play.Bigmoney - oPrices) + "" + ub.Get("SiteBz") + "", "");
                }
            }
            if (gold < _money)
            {
                Utils.Error("您的" + ub.Get("SiteBz") + "不足", "");
            }
            else if (_money > play.BigPay)//new BCW.Baccarat.BLL.BJL_Room().Getcaichi(roomid) / Percent
            {
                Utils.Error("本桌面下注最高金额为" + play.BigPay + "" + ub.Get("SiteBz") + ",你的下注金额已经超过了最高金额，请重新下注", "");
            }
            else if (_money < play.LowTotal)
            {
                Utils.Error("本桌面下注最低金额为" + play.LowTotal + "" + ub.Get("SiteBz") + ",你的下注金额低于最低金额，请重新下注", "");
            }
            else
            {
                //判断是否存在最新的桌面
                DataSet bb = new BCW.Baccarat.BLL.BJL_Play().GetList("TOP(1)*", "RoomID=" + _roomid + "  ORDER BY Play_Table DESC");
                if (bb.Tables[0].Rows.Count == 0 || bb.Tables[0].Rows.Count > 0)
                {
                    int ww = 0;
                    string kai = string.Empty;
                    try
                    {
                        ww = int.Parse(bb.Tables[0].Rows[0]["Play_Table"].ToString());
                        kai = bb.Tables[0].Rows[0]["BankerPoker"].ToString().Trim();
                    }
                    catch { }
                    int r = 0;
                    if (ww == 0)
                        r = ww + 1;
                    else
                    {
                        if (kai == "")
                            r = ww;
                        else
                            r = ww + 1;
                    }
                    if (r != _table && kai == "")
                    {
                        new Out().head(Utils.ForWordType("温馨提示"));
                        Response.Write(Out.Tab("<div class=\"title\">", ""));
                        Response.Write("温馨提示");
                        Response.Write(Out.Tab("</div>", "<br />"));

                        Response.Write(Out.Tab("<div class=\"text\">", ""));
                        Response.Write("你下注的第" + _roomid + "桌的第" + _table + "局变为" + r + "局");
                        Response.Write(Out.Tab("</div>", "<br />"));

                        string strName = "money,money1,table,roomid,vote,act,info";
                        string strValu = "" + _money + "'" + _money1 + "'" + r + "'" + _roomid + "'" + _vote + "'pay'ok2";
                        string strOthe = "确定下注,bjl.aspx,post,0,red";
                        Response.Write(Out.wapform(strName, strValu, strOthe));

                        Response.Write(Out.Tab("<div>", "<br />"));
                        Response.Write("<a href=\"" + Utils.getUrl("bjl.aspx?act=fangjian&amp;roomid=" + _roomid + "") + "\">[取消返回]</a><br />");
                        Response.Write(Out.Tab("</div>", ""));
                        Response.Write(new Out().foot());
                        Response.End();
                    }
                }
                else
                    Utils.Error("下注失败,请重新下注.", "");

                //支付安全提示
                string[] p_pageArr = { "act", "roomid", "table", "vote", "ac", "money", "money1", "info", };
                BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr);
                BCW.User.Users.IsFresh("bjl", 5);//防刷


                BCW.Baccarat.Model.BJL_Play add = new BCW.Baccarat.Model.BJL_Play();
                add.BankerPoint = 0;
                add.BankerPoker = "";
                add.buy_usid = meid;
                add.GetMoney = 0;
                add.HunterPoint = 0;
                add.HunterPoker = "";
                add.isRobot = 0;

                add.PutMoney = _money;
                add.PutTypes = _vote;
                add.RoomID = _roomid;
                add.updatetime = DateTime.Now;
                add.UsID = play.UsID;
                add.zhu_money = _money1;
                add.Play_Table = _table;//局
                add.type = 0;
                add.shouxufei = 0;
                //如果是第一个,则加上原始
                if (new BCW.Baccarat.BLL.BJL_Play().Exists(_roomid) == true)//有数据就true
                {
                    BCW.Baccarat.Model.BJL_Play uu = new BCW.Baccarat.BLL.BJL_Play().GetBJL_Play3(_roomid);
                    add.Total = uu.Total + _money;//开始彩池
                    //Utils.Error("" + add.Total + "", "");
                }
                else
                {
                    BCW.Baccarat.Model.BJL_Room uu = new BCW.Baccarat.BLL.BJL_Room().GetBJL_Room(_roomid);
                    add.Total = _money + uu.Total_Now;//实时彩池
                }
                int id = new BCW.Baccarat.BLL.BJL_Play().Add(add);
                //判断牌是否存在
                bool cd = new BCW.Baccarat.BLL.BJL_Card().ExistsCard(_roomid, _table);
                if (cd == false)
                {
                    BCW.Baccarat.Model.BJL_Card card = new BCW.Baccarat.Model.BJL_Card();
                    BCW.Baccarat.baccarat bjl = new BCW.Baccarat.baccarat();//3层方法
                    string s = bjl.ShufflingProcess();//获取庄家和闲家的扑克牌的字符串
                    string[] poker = s.Split('#');//分割庄家和闲家的扑克牌并存进poker数组
                    int bankerpoint = Point(poker[1]);//庄家点数
                    int hunterpoint = Point(poker[0]);//闲家点数
                    card.RoomID = _roomid;
                    card.RoomTable = _table;
                    card.BankerPoker = poker[1];
                    card.BankerPoint = bankerpoint;
                    card.HunterPoker = poker[0];
                    card.HunterPoint = hunterpoint;
                    new BCW.Baccarat.BLL.BJL_Card().Add(card);
                }
                new BCW.Baccarat.BLL.BJL_Room().update_zd("Total_Now=Total_Now+" + _money + "", "ID=" + _roomid + "");//加钱到房间的实时彩池
                string name1 = string.Empty;
                string[] xiazhu = { "", "庄赢", "闲赢", "和局", "庄单", "庄双", "闲单", "闲双", "投大", "投小", "庄对", "闲对", "任意对", "完美对", };
                for (int ab1 = 0; ab1 < str.Length; ab1++)
                {
                    name1 = name1 + (xiazhu[int.Parse(str[ab1])]) + ",";
                }
                //更新人气
                new BCW.Baccarat.BLL.BJL_Room().update_zd("Click=Click+1", "ID=" + _roomid + "");
                //更新酷币
                new BCW.BLL.User().UpdateiGold(meid, mename, -_money, "在" + new BCW.BLL.User().GetUsName(play.UsID) + "(" + play.UsID + ")的桌面[url=./game/bjl.aspx?act=fangjian&amp;roomid=" + _roomid + "]第" + _roomid + "桌第" + table + "局[/url]投注" + name1 + "共" + _money + "-标识ID" + id + "");
                //动态
                string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/game/bjl.aspx]" + GameName + "[/url]投注**" + ub.Get("SiteBz") + ".";//" + _money + "
                new BCW.BLL.Action().Add(1009, id, meid, "", wText);
                Utils.Success("投注", "下注成功，花费了" + _money + "" + ub.Get("SiteBz") + "", Utils.getUrl("bjl.aspx?act=fangjian&amp;roomid=" + _roomid + ""), "1");
            }
        }
        else
        {
            BCW.Baccarat.Model.BJL_Room play = new BCW.Baccarat.BLL.BJL_Room().GetBJL_Room(roomid);
            if (play.state == 1)
            {
                Utils.Error("该桌已封庄,请选择其他下注.", "");
            }
            if (meid == play.UsID)
            {
                Utils.Error("不能在自己的桌面下注.", "");
            }
            //判断是否中止下注

            string name = string.Empty;
            string[] xiazhu = { "", "庄赢", "闲赢", "和局", "庄单", "庄双", "闲单", "闲双", "投大", "投小", "庄对", "闲对", "任意对", "完美对", };
            for (int ab = 0; ab < str.Length; ab++)
            {
                name = name + (xiazhu[int.Parse(str[ab])]) + ",";
            }
            DateTime Oldbettime = new BCW.Baccarat.BLL.BJL_Play().GetMinBetTime(roomid, (table));
            string hh = string.Empty;
            if (new BCW.Baccarat.BLL.BJL_Play().Exists(roomid, (table)))
            {
                string daojishi = new BCW.JS.somejs().newDaojishi("divhp3sy6", Oldbettime.AddSeconds(Times));
                hh = ",投注:" + daojishi + "";
            }
            if (Oldbettime.AddSeconds(Times) > DateTime.Now)
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("【" + new BCW.BLL.User().GetUsName(meid) + "】桌面上第[" + roomid + "]号桌第" + table + "局下注" + hh + "<br/>");
                builder.Append("你选择了：" + name + "<br/>");
                builder.Append("总下注数：" + str.Length + "<br/>");
                builder.Append("每注金额：" + money1 + "<br/>");
                builder.Append("总下金额：" + money + "<br/>");
                builder.Append("自带金额：" + gold + "");
                builder.Append(Out.Tab("</div>", "<br/>"));

                string strName = "money,money1,vote,act,info,table,roomid";
                string strValu = "" + money + "'" + money1 + "'" + vote + "'pay'ok2'" + table + "'" + roomid + "";
                string strOthe = "确定投注,bjl.aspx,post,0,red";
                builder.Append(Out.wapform(strName, strValu, strOthe));
            }
            else
                Utils.Error("抱歉,第" + roomid + "桌第" + table + "局已截止下注,正在开奖...", "");
        }

        foot();
    }

    //我的下注记录
    private void MyBetPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);

        Master.Title = "" + GameName + "_我的下注";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx") + "\">" + GameName + "</a>&gt;我的下注");
        builder.Append(Out.Tab("</div>", "<br/>"));

        int pageIndex;
        int recordCount;
        string strWhere = "buy_usid=" + meid + "";
        string strOrder = "RoomID desc,Play_Table desc";
        string[] pageValurl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));

        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[1-4]$", "1"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 1)
        {
            builder.Append("<h style=\"color:red\">全部</h>|");
        }
        else
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=mybet&amp;ptype=1") + "\">全部</a>|");
        if (ptype == 2)
        {
            strWhere += "and type=2";
            builder.Append("<h style=\"color:red\">中奖</h>|");
        }
        else
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=mybet&amp;ptype=2") + "\">中奖</a>|");
        if (ptype == 3)
        {
            strWhere += "and type=1";
            builder.Append("<h style=\"color:red\">不中</h>|");
        }
        else
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=mybet&amp;ptype=3") + "\">不中</a>|");
        if (ptype == 4)
        {
            strWhere += "and type=0";
            builder.Append("<h style=\"color:red\">未开</h>");
        }
        else
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=mybet&amp;ptype=4") + "\">未开</a>");
        builder.Append(Out.Tab("</div>", "<br/>"));


        IList<BCW.Baccarat.Model.BJL_Play> listplay = new BCW.Baccarat.BLL.BJL_Play().GetBJL_Plays(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listplay.Count > 0)
        {
            int k = 1;
            foreach (BCW.Baccarat.Model.BJL_Play n in listplay)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div>", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }
                string[] str = n.PutTypes.Split(',');
                string name1 = string.Empty;
                string[] xiazhu = { "", "庄赢", "闲赢", "和局", "庄单", "庄双", "闲单", "闲双", "投大", "投小", "庄对", "闲对", "任意对", "完美对", };
                for (int ab1 = 0; ab1 < str.Length; ab1++)
                {
                    name1 = name1 + (xiazhu[int.Parse(str[ab1])]) + ",";
                }
                if (ptype == 1)//全部
                {
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".在<a href=\"" + Utils.getUrl("bjl.aspx?act=fangjian&amp;roomid=" + n.RoomID + "") + "\">" + n.RoomID + "号桌</a>—");
                    builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=tablelist&amp;&amp;roomid=" + n.RoomID + "&amp;table=" + n.Play_Table + "&amp;type=1") + "\">第" + n.Play_Table + "局</a>下注:" + name1 + "");
                    try
                    {
                        builder.Append("(庄:" + allpoker(n.BankerPoker) + "(<h style=\"color:red\">" + n.BankerPoint + "点</h>)闲:" + allpoker(n.HunterPoker) + "(<h style=\"color:red\">" + n.HunterPoint + "点</h>))<br/>");
                    }
                    catch
                    {
                        builder.Append("(庄:?(<h style=\"color:red\">" + n.BankerPoint + "点</h>)闲:?(<h style=\"color:red\">" + n.HunterPoint + "点</h>))<br/>");
                        new BCW.BLL.Guest().Add(1, 52784, "森林仔666", "" + GameName + "第" + n.RoomID + "桌第" + n.Play_Table + "局" + meid + "的全部下注记录出现异常.");
                    }

                    builder.Append("(开奖:庄" + n.BankerPoint + "点闲" + n.HunterPoint + "点)");
                    builder.Append("每注" + n.zhu_money + "共" + n.PutMoney + ".[" + DT.FormatDate(n.updatetime, 1) + "]");
                    if (n.type == 1)
                    {
                        builder.Append("(不中奖)");
                    }
                    if (n.type == 2)
                    {
                        builder.Append("(中奖" + n.GetMoney + ")");
                    }
                    if (n.type == 2 && n.type != 3)
                    {
                        builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=case") + "\">去兑奖>></a>");
                    }
                }
                else if (ptype == 2)//中奖
                {
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".在<a href=\"" + Utils.getUrl("bjl.aspx?act=fangjian&amp;roomid=" + n.RoomID + "") + "\">" + n.RoomID + "号桌</a>—");
                    builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=tablelist&amp;&amp;roomid=" + n.RoomID + "&amp;table=" + n.Play_Table + "&amp;type=1") + "\">第" + n.Play_Table + "局</a>下注:" + name1 + "");
                    builder.Append("(庄:" + allpoker(n.BankerPoker) + "(<h style=\"color:red\">" + n.BankerPoint + "点</h>)闲:" + allpoker(n.HunterPoker) + "(<h style=\"color:red\">" + n.HunterPoint + "点</h>))<br/>");
                    builder.Append("(开奖:庄" + n.BankerPoint + "点闲" + n.HunterPoint + "点)每注" + n.zhu_money + "共" + n.PutMoney + ".[" + DT.FormatDate(n.updatetime, 1) + "](中奖" + n.GetMoney + ")");
                    if (n.type != 3)
                    {
                        builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=case") + "\">去兑奖>></a>");
                    }
                }
                else if (ptype == 3)//不中
                {
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".在<a href=\"" + Utils.getUrl("bjl.aspx?act=fangjian&amp;roomid=" + n.RoomID + "") + "\">" + n.RoomID + "号桌</a>—");
                    builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=tablelist&amp;&amp;roomid=" + n.RoomID + "&amp;table=" + n.Play_Table + "&amp;type=1") + "\">第" + n.Play_Table + "局</a>下注:" + name1 + "");
                    builder.Append("(庄:" + allpoker(n.BankerPoker) + "(<h style=\"color:red\">" + n.BankerPoint + "点</h>)闲:" + allpoker(n.HunterPoker) + "(<h style=\"color:red\">" + n.HunterPoint + "点</h>))<br/>");
                    builder.Append("每注" + n.zhu_money + "共" + n.PutMoney + ".[" + DT.FormatDate(n.updatetime, 1) + "](不中奖)");
                }
                else//未开
                {
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".在<a href=\"" + Utils.getUrl("bjl.aspx?act=fangjian&amp;roomid=" + n.RoomID + "") + "\">" + n.RoomID + "号桌</a>—");
                    builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=tablelist&amp;&amp;roomid=" + n.RoomID + "&amp;table=" + n.Play_Table + "&amp;type=1") + "\">第" + n.Play_Table + "局</a>下注:" + name1 + "");
                    try
                    {
                        builder.Append("(庄:" + allpoker(n.BankerPoker) + "(<h style=\"color:red\">" + n.BankerPoint + "点</h>)闲:" + allpoker(n.HunterPoker) + "(<h style=\"color:red\">" + n.HunterPoint + "点</h>))<br/>");
                    }
                    catch
                    {
                        builder.Append("(庄:?(<h style=\"color:red\">" + n.BankerPoint + "点</h>)闲:?(<h style=\"color:red\">" + n.HunterPoint + "点</h>))<br/>");
                        new BCW.BLL.Guest().Add(1, 52784, "森林仔666", "" + GameName + "第" + n.RoomID + "桌第" + n.Play_Table + "局" + meid + "的未开下注记录出现异常.");
                    }

                    builder.Append("每注" + n.zhu_money + "共" + n.PutMoney + ".[" + DT.FormatDate(n.updatetime, 1) + "]");
                }
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValurl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        foot();
    }

    //游戏规则
    private void RulePage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("bjl.aspx") + "\">" + GameName + "</a>&gt;游戏规则");
        builder.Append(Out.Tab("</div>", "<br/>"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        int vote = int.Parse(Utils.GetRequest("vote", "all", 1, "", "0"));

        if (ptype == 1)
        {
            Master.Title = ("" + GameName + "_游戏说明");
            builder.Append("<h style=\"color:red\">游戏说明" + "</h>" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=rule&amp;ptype=1") + "\">游戏说明</a>" + "|");
        }
        if (ptype == 2)
        {
            Master.Title = ("" + GameName + "_游戏派彩与赔率");
            builder.Append("<h style=\"color:red\">派彩与赔率" + "</h>" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=rule&amp;ptype=2") + "\">派彩与赔率</a>" + "|");
        }
        if (ptype == 3)
        {
            Master.Title = ("" + GameName + "_游戏显示方式");
            builder.Append("<h style=\"color:red\">显示方式" + "</h>" + "");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=rule&amp;ptype=3") + "\">显示方式</a>" + "");
        }
        builder.Append(Out.Tab("</div>", ""));

        if (ptype == 1)
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("该" + GameName + "使用8副扑克牌：系统会自动派出“庄家”和“闲家”两份牌。<br />");
            builder.Append("A是1点，2到9的牌面即为点数，K、Q、J、10是0点，加起来等于10也当作是0点；总数9点或最接近9点的一家胜出。<br />");
            builder.Append("当任何一家起手牌的点数总和为8或9，就称为“天生赢家”，牌局就算结束，双方不再补牌。<br />");
            builder.Append("你有13种下注选择：◎庄赢◎闲赢◎和局◎庄单◎庄双◎闲单◎闲双◎投小◎投大◎庄对◎闲对◎任意对◎完美对。<br />");
            builder.Append("派完起手牌，将依补牌规则补1张牌。");
            builder.Append(Out.Tab("</div>", "<br/>"));

            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("【闲家补牌规则】<br />");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(" 起手牌点数总和--补牌规则<br /> 0须补牌<br /> 1须补牌<br /> 2须补牌<br /> 3须补牌<br /> 4须补牌<br /> 5须补牌<br /> 6不须补牌<br /> 7不须补牌<br /> 8“天生赢家”<br /> 9“天生赢家”");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
            builder.Append("【庄家补牌规则】<br />");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(" 起手牌点数总和--补牌规则<br /> 0须补牌<br /> 1须补牌<br /> 2须补牌<br /> 3当闲家补得第三张牌是8，不须补牌；其余则须补牌<br /> 4当闲家补得第三张牌是0.1.8.9，不须补牌；其余则须补牌<br />");
            builder.Append(" 5当闲家补得第三张牌是0.1.2.3.8.9，不须补牌；其余则须补牌<br /> 6当闲家补得第三张牌是0.1.2.3.4.5.8.9，不须补牌；其余则须补牌<br /> 7不须补牌<br /> 8“天生赢家”<br /> 9“天生赢家”<br /> ");
            builder.Append(" 闲家起手牌点数为6点或7点，闲家不须补牌，此条件下庄家起手牌点数为5或5点以下，庄家必须补第三张牌。");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
            builder.Append("【下注玩法】<br />");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(" “庄赢”指庄的总点数大于闲的总点数<br /> “闲赢”指庄的总点数小于闲的总点数<br /> “和局”指庄与闲的总点数相同。<br /> “庄单”指庄总点数为1,3,5,7,9。<br /> “庄双”指庄总点数为0,2,4,6,8。<br />");
            builder.Append(" “闲单”指闲总点数为1,3,5,7,9。<br /> “闲双”指闲总点数为0,2,4,6,8。<br /> “投小”指当局开牌张数总和4张牌为小。<br /> “投大”指当局开牌张数总和5张牌或6张牌为大。<br /> “庄对”指庄的起手牌为同数字或英文字母。<br />");
            builder.Append(" “闲对”指闲的起手牌为同数字或英文字母。<br /> “任意对”指庄或闲的起手牌为同数字或英文字母。<br /> “完美对”指庄或闲的起手牌为同花色且同数字或同花色且同英文字母。");
            builder.Append(Out.Tab("</div>", ""));
        }
        else if (ptype == 2)
        {
            builder.Append(Out.Tab("<div >", "<br />"));
            builder.Append("【派彩(赔率包含本金)】<br />");
            builder.Append("“庄赢”(1赔" + betpercent(1) + ")指庄的总点数大于闲的总点数(开和局时退回下注金额)<br />“闲赢”(1赔" + betpercent(2) + ")指庄的总点数小于闲的总点数(开和局时退回下注金额)<br />“和局”(1赔" + betpercent(3) + ")指庄与闲的总点数相同。<br />");
            builder.Append("“庄单”(1赔" + betpercent(4) + ")指庄总点数为1,3,5,7,9。<br />“庄双”(1赔" + betpercent(5) + ")指庄总点数为0,2,4,6,8。<br />“闲单”(1赔" + betpercent(6) + ")指闲总点数为1,3,5,7,9。<br />“闲双”(1赔" + betpercent(7) + ")指闲总点数为0,2,4,6,8。<br />");
            builder.Append("“投大”(1赔" + betpercent(8) + ")指当局开牌张数总和5张牌或6张牌为大。<br />“投小”(1赔" + betpercent(9) + ")指当局开牌张数总和4张牌为小。<br />“庄对”(1赔" + betpercent(10) + ")指庄的起手牌为同数字或英文字母。<br />");
            builder.Append("“闲对”(1赔" + betpercent(11) + ")指闲的起手牌为同数字或英文字母。<br />“任意对”(1赔" + betpercent(12) + ")指庄或闲的起手牌为同数字或英文字母。<br />“完美对”(1赔" + betpercent(13) + ")指庄或闲的起手牌为同花色且同数字或同花色且同英文字母。<br />");
            if (shouxufei > 0)
            {
                builder.Append("温馨提示：<br/>1、系统每局收取盈利方" + shouxufei + "%手续费.<br/>");
                builder.Append("2、按庄赢为例,1.96水下注10000金币：若玩家赢了,庄家赔9600金币,系统收96金币,玩家赚9504(加上1万本金即19504)。若玩家输了,10000金币赔给庄家9900金币,系统收100金币.开和时退回下注额,不收税.");
            }
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            int meid = new BCW.User.Users().GetUsId();
            if (meid == 0)
                Utils.Login();

            if (Utils.ToSChinese(ac) == "确定修改")
            {
                new BCW.Baccarat.BLL.BJL_user().update_zd("setshow=" + vote + "", "usid=" + meid + "");
                Utils.Success("温馨提示", "设置发牌显示成功，正在返回...", Utils.getUrl("bjl.aspx?act=rule&amp;ptype=3" + ""), "1");
            }
            else
            {
                BCW.Baccarat.Model.BJL_user user = new BCW.Baccarat.BLL.BJL_user().GetBJL_setshow(meid);
                string strText = "显示效果：,";
                string strName = "vote";
                string strType = "select";
                string strValu = user.setshow + "'";
                string strEmpt = "0|文字|1|图片";
                string strIdea = "/";
                string strOthe = "确定修改,bjl.aspx?act=rule&amp;ptype=3,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
        }
        foot();
    }

    //查看牌局
    private void tablelistPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);

        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-2]$", "0"));
        int roomid = Utils.ParseInt(Utils.GetRequest("roomid", "get", 1, @"^[1-9]\d*$", "1"));
        int table = Utils.ParseInt(Utils.GetRequest("table", "get", 1, @"^[1-9]\d*$", "1"));
        int t_type = Utils.ParseInt(Utils.GetRequest("type", "get", 1, @"^[0-1]\d*$", "0"));
        BCW.Baccarat.Model.BJL_Play play = new BCW.Baccarat.BLL.BJL_Play().GetBJL_Play(roomid);
        if (play.ID == 0)
            Utils.Error("该房间不存在.", "");

        //判断是否开奖
        BCW.Baccarat.Model.BJL_Card card = new BCW.Baccarat.BLL.BJL_Card().GetCardMessage(roomid, table);
        if (card.ID == 0)
        {
            Utils.Error("该局还没开奖或不存在该局.", "");
        }

        Master.Title = "查看牌局";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("bjl.aspx?act=fangjian&amp;roomid=" + roomid + "") + "\">" + new BCW.BLL.User().GetUsName(play.UsID) + "的房间</a>&gt;查看牌局");
        builder.Append(Out.Tab("</div>", "<br />"));

        if (ptype == 1)
        {
            new BCW.Baccarat.BLL.BJL_user().update_zd("setshow=1", "usid=" + meid + "");
        }
        else if (ptype == 2)
        {
            new BCW.Baccarat.BLL.BJL_user().update_zd("setshow=0", "usid=" + meid + "");
        }


        BCW.Baccarat.Model.BJL_user user = new BCW.Baccarat.BLL.BJL_user().GetBJL_setshow(meid);

        builder.Append(Out.Tab("<div  class=\"text\">", ""));
        builder.Append("【" + roomid + "号桌,第" + table + "局】");
        if (user.setshow == 0)
        {
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=tablelist&amp;roomid=" + roomid + "&amp;ptype=1&amp;table=" + table + "&amp;type=" + t_type + "") + "\">[图片显示]</a><br />");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=tablelist&amp;roomid=" + roomid + "&amp;ptype=2&amp;table=" + table + "&amp;type=" + t_type + "") + "\">[文字显示]</a><br />");
        }

        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));


        if (user.setshow == 1)//图片显示
        {
            builder.Append("庄家:");
            string[] a = card.BankerPoker.Split(',');
            if (a.Length == 4)
            {
                PokerPicture(OnePoker(a[0], a[1]));
                PokerPicture(OnePoker(a[2], a[3]));
            }
            else if (a.Length == 6)
            {
                PokerPicture(OnePoker(a[0], a[1]));
                PokerPicture(OnePoker(a[2], a[3]));
                PokerPicture(OnePoker(a[4], a[5]));
            }
            else
            {
                builder.Append("" + allpoker(card.BankerPoker) + "");
            }
            builder.Append("(<h style=\"color:red\">" + card.BankerPoint + "点</h>)<br />");

            builder.Append("闲家:");
            string[] b = card.HunterPoker.Split(',');
            if (b.Length == 4)
            {
                PokerPicture(OnePoker(b[0], b[1]));
                PokerPicture(OnePoker(b[2], b[3]));
            }
            else if (b.Length == 6)
            {
                PokerPicture(OnePoker(b[0], b[1]));
                PokerPicture(OnePoker(b[2], b[3]));
                PokerPicture(OnePoker(b[4], b[5]));
            }
            else
            {
                builder.Append("" + allpoker(card.HunterPoker) + "");
            }
            builder.Append("(<h style=\"color:red\">" + card.HunterPoint + "点</h>)<br />");
        }
        else
        {
            builder.Append("庄家:" + allpoker(card.BankerPoker) + "(<h style=\"color:red\">" + card.BankerPoint + "点</h>)<br />");
            builder.Append("闲家:" + allpoker(card.HunterPoker) + "(<h style=\"color:red\">" + card.HunterPoint + "点</h>)<br />");
        }

        builder.Append("总下注:" + new BCW.Baccarat.BLL.BJL_Play().GetPrice(roomid, table) + ",");
        builder.Append("总中奖:" + new BCW.Baccarat.BLL.BJL_Play().Getmoney(roomid, table) + ",");
        builder.Append("总手续费:" + new BCW.Baccarat.BLL.BJL_Play().Getsxf(roomid, table) + "");
        builder.Append(Out.Tab("</div>", Out.Hr()));

        builder.Append(Out.Tab("<div  class=\"text\">", ""));
        builder.Append("【下注记录】");
        builder.Append(Out.Tab("</div>", "<br/>"));

        int pageIndex;
        int recordCount;
        string[] pageValUrl = { "act", "roomid", "ptype", "table", "type", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));

        DataSet ds = new BCW.Baccarat.BLL.BJL_Play().GetList("*", "RoomID=" + roomid + " and Play_Table=" + table + "");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            recordCount = ds.Tables[0].Rows.Count;
            int k = 1;
            int koo = (pageIndex - 1) * pageSize;
            int skt = pageSize;
            if (recordCount > koo + pageSize)
            {
                skt = pageSize;
            }
            else
            {
                skt = recordCount - koo;
            }
            for (int i = 0; i < skt; i++)
            {
                int ID = int.Parse(ds.Tables[0].Rows[koo + i]["ID"].ToString());
                int UsID = int.Parse(ds.Tables[0].Rows[koo + i]["UsID"].ToString());
                int RoomID = int.Parse(ds.Tables[0].Rows[koo + i]["RoomID"].ToString());
                int Play_Table = int.Parse(ds.Tables[0].Rows[koo + i]["Play_Table"].ToString());
                int BankerPoint = int.Parse(ds.Tables[0].Rows[koo + i]["BankerPoint"].ToString());
                int HunterPoint = int.Parse(ds.Tables[0].Rows[koo + i]["HunterPoint"].ToString());
                int isRobot = int.Parse(ds.Tables[0].Rows[koo + i]["isRobot"].ToString());
                int type = int.Parse(ds.Tables[0].Rows[koo + i]["type"].ToString());
                int buy_usid = int.Parse(ds.Tables[0].Rows[koo + i]["buy_usid"].ToString());
                string PutTypes = ds.Tables[0].Rows[koo + i]["PutTypes"].ToString();
                string BankerPoker = ds.Tables[0].Rows[koo + i]["BankerPoker"].ToString();
                string HunterPoker = ds.Tables[0].Rows[koo + i]["HunterPoker"].ToString();
                DateTime updatetime = Convert.ToDateTime(ds.Tables[0].Rows[koo + i]["updatetime"]);
                long Total = Convert.ToInt64(ds.Tables[0].Rows[koo + i]["buy_usid"].ToString());
                long zhu_money = Convert.ToInt64(ds.Tables[0].Rows[koo + i]["zhu_money"].ToString());
                long PutMoney = Convert.ToInt64(ds.Tables[0].Rows[koo + i]["PutMoney"].ToString());
                long GetMoney = Convert.ToInt64(ds.Tables[0].Rows[koo + i]["GetMoney"].ToString());

                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div>", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }

                string[] str = PutTypes.Split(',');
                string name1 = string.Empty;
                string[] xiazhu = { "", "庄赢", "闲赢", "和局", "庄单", "庄双", "闲单", "闲双", "投大", "投小", "庄对", "闲对", "任意对", "完美对", };
                for (int ab1 = 0; ab1 < str.Length; ab1++)
                {
                    name1 = name1 + (xiazhu[int.Parse(str[ab1])]) + ",";
                }
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + buy_usid + "") + "\">" + new BCW.BLL.User().GetUsName(buy_usid) + "</a>：[" + name1 + "]共" + PutMoney + ".");
                if (GetMoney > 0)
                {
                    if (BankerPoint == HunterPoint)
                    {
                        builder.Append("(和:退本金" + GetMoney + "" + ub.Get("SiteBz") + ")");
                    }
                    else
                    {
                        builder.Append("(赢:" + GetMoney + "" + ub.Get("SiteBz") + ")");
                    }

                }
                builder.Append("[" + DT.FormatDate(updatetime, 1) + "]");
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有下注记录.."));
        }

        builder.Append(Out.Tab("<div>", "<br/>"));
        if (t_type == 1)
        {
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=mybet") + "\">返回上级>></a>");
        }
        else
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=fangjian&amp;roomid=" + roomid + "") + "\">返回上级>></a>");
        builder.Append(Out.Tab("</div>", ""));

        foot();
    }

    //公告查看与修改
    private void gonggaoPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);

        int roomid = Utils.ParseInt(Utils.GetRequest("roomid", "all", 1, @"^[1-9]\d*$", ""));//房间ID

        BCW.Baccarat.Model.BJL_Room play = new BCW.Baccarat.BLL.BJL_Room().GetBJL_Room(roomid, meid);
        if (play.ID == 0)
            Utils.Error("该房间不存在.", "");


        Master.Title = "修改" + play.ID + "号桌的公告";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("bjl.aspx?act=taimian") + "\">我的桌面</a>&gt;<a href=\"" + Utils.getUrl("bjl.aspx?act=fangjian&amp;roomid=" + roomid + "") + "\">我的房间</a>&gt;公告修改");
        builder.Append(Out.Tab("</div>", ""));

        string info = Utils.GetRequest("info", "post", 1, "", "");
        if (info == "")
        {
            string strText = "标题：/,内容：/,,,";
            string strName = "title,contact,roomid,act,info";
            string strType = "text,textarea,hidden,hidden,hidden";
            string strValu = "" + play.Title + "'" + play.contact + "'" + roomid + "'gonggao'add";
            string strEmpt = "true,true,false,false,false";
            string strIdea = "/";
            string strOthe = "修改公告,bjl.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        else
        {
            if (play.state == 1)
                Utils.Error("该房间已封装,不可以修改.", "");
            BCW.User.Users.IsFresh("baccarat", 2);//是否刷屏
            string title = Utils.GetRequest("title", "all", 2, @"^[\s\S]{1,20}$", "标题限1-20字");
            string contact = Utils.GetRequest("contact", "all", 2, @"^[\s\S]{1,100}$", "内容限1-100字");
            int roomid1 = Utils.ParseInt(Utils.GetRequest("roomid", "all", 2, @"^[0-9]\d*$", "房间号码错误"));
            new BCW.Baccarat.BLL.BJL_Room().update_zd("title=" + title + ",contact=" + contact + "", "ID=" + roomid1 + "");
            Utils.Success("公告", "恭喜您，修改公告成功.", Utils.getUrl("bjl.aspx?act=fangjian&amp;roomid=" + roomid1 + ""), "1");
        }
        foot();
    }

    //公告显示
    private void ggshowPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        //房间ID
        int roomid = Utils.ParseInt(Utils.GetRequest("roomid", "get", 1, @"^[1-9]\d*$", "0"));
        BCW.Baccarat.Model.BJL_Room play = new BCW.Baccarat.BLL.BJL_Room().GetBJL_Room(roomid);
        if (play.ID == 0)
            Utils.Error("该房间不存在.", "");

        Master.Title = "" + GameName + "_公告显示";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("bjl.aspx?act=fangjian&amp;roomid=" + play.ID + "") + "\">" + new BCW.BLL.User().GetUsName(play.UsID) + "的房间</a>&gt;公告显示");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("公告标题：" + play.Title + "<br/>");
        builder.Append("公告内容：" + play.contact + "");
        builder.Append("<br/><a href=\"" + Utils.getUrl("bjl.aspx?act=fangjian&amp;roomid=" + play.ID + "") + "\">返回>></a>");
        builder.Append(Out.Tab("</div>", ""));
        foot();
    }

    //历史数据
    private void history1(int roomid)
    {
        DataSet a1 = new BCW.Baccarat.BLL.BJL_Play().GetList2(roomid, "BankerPoint>HunterPoint");
        DataSet b1 = new BCW.Baccarat.BLL.BJL_Play().GetList2(roomid, "BankerPoint<HunterPoint");
        DataSet c1 = new BCW.Baccarat.BLL.BJL_Play().GetList2(roomid, "BankerPoint=HunterPoint");
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("【庄:" + a1.Tables[0].Rows[0]["a"].ToString() + "次,闲:" + b1.Tables[0].Rows[0]["a"].ToString() + "次,和:" + c1.Tables[0].Rows[0]["a"].ToString() + "次】<br />");
        builder.Append(Out.Tab("</div>", ""));
        int pageIndex;
        int recordCount;
        string strWhere = "RoomID=" + roomid + "";
        string[] pageValUrl = { "act", "roomid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));

        IList<BCW.Baccarat.Model.BJL_Card> listplay = new BCW.Baccarat.BLL.BJL_Card().GetBJL_Cards(pageIndex, pageSize, strWhere, out recordCount);
        if (listplay.Count > 0)
        {
            int k = 1;
            foreach (BCW.Baccarat.Model.BJL_Card n in listplay)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div>", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }
                DateTime Oldbettime = new BCW.Baccarat.BLL.BJL_Play().GetMinBetTime(roomid, (n.RoomTable));
                if (Oldbettime.AddSeconds((Times + 9)) <= DateTime.Now)
                {
                    builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=tablelist&amp;roomid=" + n.RoomID + "&amp;table=" + n.RoomTable + "") + "\">第" + n.RoomTable + "局</a>:");
                    builder.Append("庄" + n.BankerPoint + "点,闲" + n.HunterPoint + "点");

                    if (n.BankerPoint > n.HunterPoint)
                        builder.Append("(庄)");
                    else if (n.BankerPoint == n.HunterPoint)
                        builder.Append("(和)");
                    else
                        builder.Append("(闲)");
                }
                else
                {
                    //如果没有投注记录
                    if (new BCW.Baccarat.BLL.BJL_Play().Exists_xz(n.RoomID, n.RoomTable))
                    {
                        builder.Append("第" + n.RoomTable + "局:等待开奖.");
                    }
                }
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
    }

    //追加操作
    private void zhuijiaPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);

        long gold = new BCW.BLL.User().GetGold(meid);

        int roomid = Utils.ParseInt(Utils.GetRequest("roomid", "all", 1, @"^[1-9]\d*$", "0"));
        BCW.Baccarat.Model.BJL_Room play = new BCW.Baccarat.BLL.BJL_Room().GetBJL_Room(roomid, meid);
        if (play.ID == 0)
            Utils.Error("该房间不存在.", "");

        Master.Title = "追加" + play.ID + "号桌的彩池";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("bjl.aspx?act=taimian") + "\">我的桌面</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=fangjian&amp;roomid=" + play.ID + "") + "\">我的房间</a>&gt;彩池追加");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("初始彩池:" + play.Total + ub.Get("SiteBz") + "<br />");
        builder.Append("当前彩池:" + play.Total_Now + ub.Get("SiteBz") + "");
        if (play.zhui_Total > 0)
        {
            builder.Append("<br />已追加彩池:" + play.zhui_Total + ub.Get("SiteBz") + "");
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        string info = Utils.GetRequest("aq", "post", 1, "", "");
        if (info == "")
        {
            string VE = ConfigHelper.GetConfigString("VE");
            string SID = ConfigHelper.GetConfigString("SID");
            builder.Append(("<form name=\"form\" action=\"bjl.aspx\" method=\"post\">"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("追加彩池:");
            builder.Append("<input maxlength=\"500\" type=\"text\" emptyok=\"true\" name=\"Cent\" />");
            builder.Append(Out.Tab("</div>", "<br/>"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<input type=\"hidden\" name=\"roomid\" value=\"" + play.ID + "\" />");
            builder.Append("<input type=\"hidden\" name=\"act\" value=\"zhuijia\" />");
            builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
            builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
            builder.Append("<input class=\"btn-red\" type=\"submit\" name=\"aq\" value=\"立刻追加\" />");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append("</form>");
        }
        else
        {
            if (play.state == 1)
                Utils.Error("该房间已封装,不可以追加.", "");
            int Cent = Utils.ParseInt(Utils.GetRequest("Cent", "post", 1, @"^[1-9]\d*$", "0"));
            if (Cent == 0)
                Utils.Error("请重新输入追加金额.", "");
            else if (Cent > gold)
                Utils.Error("你的金钱不足，追加失败", "");
            else
            {
                //支付安全提示
                string[] p_pageArr = { "act", "roomid", "Cent", "aq", };
                BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr);
                BCW.User.Users.IsFresh("bjl", 5);//防刷
                new BCW.BLL.User().UpdateiGold(meid, mename, -Cent, "在" + GameName + "第" + play.ID + "桌追加彩池" + Cent + ub.Get("SiteBz") + "-标识ID" + play.ID + "");
                new BCW.Baccarat.BLL.BJL_Room().update_zd("zhui_Total=zhui_Total+" + Cent + ",Total_Now=Total_Now+" + Cent + "", "ID=" + play.ID + "");

                //发送内线
                new BCW.BLL.Guest().Add(1, meid, mename, "系统恭喜！你为第" + roomid + "桌的彩池追加" + Cent + "" + ub.Get("SiteBz") + ",目前彩池有" + (play.Total_Now + Cent) + ub.Get("SiteBz") + "。[url=/bbs/game/bjl.aspx?act=fangjian&amp;roomid=" + roomid + "]进入桌面[/url]");
                Utils.Success("追加彩池", "第" + roomid + "桌的彩池追加成功,花费了" + Cent + ub.Get("SiteBz") + "", Utils.getUrl("bjl.aspx?act=fangjian&amp;roomid=" + roomid + ""), "2");
            }
        }

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    //日志显示
    private void diaryPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);

        int roomid = Utils.ParseInt(Utils.GetRequest("roomid", "get", 1, @"^[1-9]\d*$", ""));

        BCW.Baccarat.Model.BJL_Room play = new BCW.Baccarat.BLL.BJL_Room().GetBJL_Room(roomid, meid);
        if (play.ID == 0)
            Utils.Error("该房间不存在.", "");

        Master.Title = play.ID + "号桌的日志";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("bjl.aspx?act=taimian") + "\">我的桌面</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=fangjian&amp;roomid=" + play.ID + "") + "\">我的房间</a>&gt;日志查看");
        builder.Append(Out.Tab("</div>", "<br />"));


        builder.Append(Out.Tab("<div class=\"text\">", ""));
        int ptye = Utils.ParseInt(Utils.GetRequest("ptye", "get", 1, @"^[0-1]$", "1"));
        if (ptye == 0)
        {
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=diary&amp;ptye=1&amp;roomid=" + roomid + "") + "\">下注日志</a>" + "|");
            builder.Append("<h style=\"color:red\">追加日志" + "</h>" + "");
        }
        else
        {
            builder.Append("<h style=\"color:red\">下注日志" + "</h>" + "|");
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=diary&amp;ptye=0&amp;roomid=" + roomid + "") + "\">追加日志</a>" + "");
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        string strWhere = "RoomID=" + roomid + " ";
        string[] pageValUrl = { "act", "roomid", "ptye", "backurl" };
        string strOrder = " updatetime desc,Play_Table asc";
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));

        if (ptye == 0)
        {
            strWhere = "ToId=" + meid + " and Content LIKE '%第" + roomid + "桌的彩池追加%'";
            IList<BCW.Model.Guest> listGuest = new BCW.BLL.Guest().GetGuests(pageIndex, pageSize, strWhere, out recordCount);//strOrder,
            if (listGuest.Count > 0)
            {
                int k = 1;
                foreach (BCW.Model.Guest n in listGuest)
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
                    string sText = n.Content;
                    string ss = "";
                    for (int u = 0; u <= sText.Length; u++)
                    {
                        ss = ss + sText[u].ToString();
                        if (sText[u].ToString() == "。")
                        {
                            break;
                        }
                    }
                    try
                    {
                        builder.AppendFormat("{0}.{1}", (pageIndex - 1) * pageSize + k, Out.SysUBB(ss.ToString().Substring(7, ss.Length - 7)));
                    }
                    catch
                    {
                        builder.AppendFormat("{0}.{1}", (pageIndex - 1) * pageSize + k, Out.SysUBB(sText));
                    }
                    builder.AppendFormat("时间:{0}", DT.FormatDate(n.AddTime, 1));
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录.."));
            }
        }
        else
        {
            IList<BCW.Baccarat.Model.BJL_Play> listplay = new BCW.Baccarat.BLL.BJL_Play().GetBJL_Plays(pageIndex, pageSize, strWhere, strOrder, out recordCount);
            if (listplay.Count > 0)
            {
                int k = 1;
                foreach (BCW.Baccarat.Model.BJL_Play n in listplay)
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

                    string[] str = n.PutTypes.Split(',');
                    string name1 = string.Empty;
                    string[] xiazhu = { "", "庄赢", "闲赢", "和局", "庄单", "庄双", "闲单", "闲双", "投大", "投小", "庄对", "闲对", "任意对", "完美对", };
                    for (int ab1 = 0; ab1 < str.Length; ab1++)
                    {
                        name1 = name1 + (xiazhu[int.Parse(str[ab1])]) + ",";
                    }
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + "." + new BCW.BLL.User().GetUsName(n.buy_usid) + "在第" + n.Play_Table + "局：押" + name1 + "");
                    try
                    {
                        builder.Append("[庄:" + allpoker(n.BankerPoker) + "(" + n.BankerPoint + "点)闲:" + allpoker(n.HunterPoker) + "(" + n.HunterPoint + "点)]<br/>");
                    }
                    catch
                    {
                        builder.Append("[庄:?(" + n.BankerPoint + "点)闲:?(" + n.HunterPoint + "点)]<br/>");
                        new BCW.BLL.Guest().Add(1, 52784, "森林仔666", "" + GameName + "在" + n.UsID + "的第" + n.RoomID + "桌第" + n.Play_Table + "局ID:" + n.buy_usid + "开奖日志出现异常.");
                    }
                    builder.Append("共投" + n.PutMoney + ".");//每注" + n.zhu_money + "
                    if (n.GetMoney > 0)
                    {
                        builder.Append("中奖" + n.GetMoney + ",");
                    }
                    builder.Append("结" + n.Total + ".[" + DT.FormatDate(n.updatetime, 1) + "]");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录.."));
            }
        }
        foot();
    }

    //封庄
    private void fzhuangPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        //房间ID
        int roomid = Utils.ParseInt(Utils.GetRequest("roomid", "all", 1, @"^[1-9]\d*$", ""));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");

        BCW.Baccarat.Model.BJL_Room play = new BCW.Baccarat.BLL.BJL_Room().GetBJL_Room(roomid, meid);
        if (play.ID == 0)
            Utils.Error("该房间不存在.", "");
        if (play.state == 1)
            Utils.Error("该房间已封庄,不用重复封庄.", "");
        BCW.Baccarat.Model.BJL_Play ab = new BCW.Baccarat.BLL.BJL_Play().GetBJL_Play(roomid, meid);
        if (ab.Play_Table < RoomTime1)
        {
            Utils.Error("局数不足" + RoomTime1 + "局，封庄失败." + "", "");
        }
        if (ab.BankerPoker == "")
        {
            Utils.Error("这局正有玩家在玩，请稍后再封庄.", "");
        }


        Master.Title = "" + GameName + "_封庄";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("bjl.aspx?act=taimian") + "\">我的桌面</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=fangjian&amp;roomid=" + play.ID + "") + "\">我的房间</a>&gt;封庄");
        builder.Append(Out.Tab("</div>", ""));

        if (Utils.ToSChinese(ac) == "确定封庄")
        {
            new BCW.Baccarat.BLL.BJL_Room().update_zd("state=1", "ID=" + roomid + "");
            if (play.Total_Now > 0)//退回给庄家
            {
                new BCW.BLL.User().UpdateiGold(play.UsID, new BCW.BLL.User().GetUsName(play.UsID), play.Total_Now, "你在" + GameName + "[url=./game/bjl.aspx?act=fangjian&amp;roomid=" + roomid + "]第" + roomid + "桌[/url]手动封庄,系统退还剩余彩池" + play.Total_Now + ub.Get("SiteBz") + "-标识房间ID" + roomid + "");
                new BCW.BLL.Guest().Add(1, play.UsID, new BCW.BLL.User().GetUsName(play.UsID), "你在" + GameName + "[url=./game/bjl.aspx?act=fangjian&amp;roomid=" + roomid + "]第" + roomid + "桌[/url]手动封庄,系统退还剩余彩池" + play.Total_Now + ub.Get("SiteBz") + ".[url=/bbs/game/bjl.aspx]进入" + GameName + "[/url]");
            }
            else
            {
                if ((new BCW.BLL.User().GetGold(play.UsID) + play.Total_Now) > 0)//够钱扣
                {
                    new BCW.BLL.User().UpdateiGold(play.UsID, new BCW.BLL.User().GetUsName(play.UsID), -play.Total_Now, "你在" + GameName + "[url=./game/bjl.aspx?act=fangjian&amp;roomid=" + roomid + "]第" + roomid + "桌[/url]的彩池已低于0,系统自动补扣" + play.Total_Now + ub.Get("SiteBz") + "-标识房间ID" + roomid + "");
                    new BCW.BLL.Guest().Add(1, play.UsID, new BCW.BLL.User().GetUsName(play.UsID), "你在" + GameName + "[url=./game/bjl.aspx?act=fangjian&amp;roomid=" + roomid + "]第" + roomid + "桌[/url]的彩池已低于0,系统自动从你账户补扣" + play.Total_Now + ub.Get("SiteBz") + ".[url=/bbs/game/bjl.aspx]进入" + GameName + "[/url]");
                }
                else
                {
                    BCW.Model.Gameowe owe = new BCW.Model.Gameowe();
                    owe.Types = 1;
                    owe.UsID = play.UsID;
                    owe.UsName = new BCW.BLL.User().GetUsName(play.UsID);
                    owe.Content = "你在" + GameName + "[url=./game/bjl.aspx?act=fangjian&amp;roomid=" + roomid + "]第" + roomid + "桌[/url]的彩池已低于0,你欠下系统的" + play.Total_Now + new BCW.BLL.User().GetGold(play.UsID) + "" + ub.Get("SiteBz") + ".";
                    owe.OweCent = play.Total_Now + new BCW.BLL.User().GetGold(play.UsID);
                    owe.BzType = 12;//百家乐封庄记录type的id
                    owe.EnId = play.ID;
                    owe.AddTime = DateTime.Now;
                    new BCW.BLL.Gameowe().Add(owe);
                    new BCW.BLL.User().UpdateIsFreeze(play.UsID, 1);

                    //发送内线
                    string strGuess = "你在" + GameName + "[url=./game/bjl.aspx?act=fangjian&amp;roomid=" + roomid + "]第" + roomid + "桌[/url]的彩池已低于0,你欠下系统的" + play.Total_Now + new BCW.BLL.User().GetGold(play.UsID) + "" + ub.Get("SiteBz") + ".[br]根据您的帐户数额,实扣" + new BCW.BLL.User().GetGold(play.UsID) + "" + ub.Get("SiteBz") + ".[br]您的" + ub.Get("SiteBz") + "不足,系统将您帐户冻结。";
                    new BCW.BLL.Guest().Add(1, play.UsID, new BCW.BLL.User().GetUsName(play.UsID), strGuess);
                    string bb = "" + new BCW.BLL.User().GetUsName(play.UsID) + "(" + play.UsID + ")在" + GameName + "[url=./game/bjl.aspx?act=fangjian&amp;roomid=" + roomid + "]第" + roomid + "桌[/url]的彩池已低于0,欠下系统" + play.Total_Now + new BCW.BLL.User().GetGold(play.UsID) + "" + ub.Get("SiteBz") + ",系统已自动冻结TA的帐户.";
                    new BCW.BLL.Guest().Add(1, 10086, new BCW.BLL.User().GetUsName(10086), bb);
                }
            }
            Utils.Success("封庄成功", "封庄成功.正在返回.", Utils.getUrl("bjl.aspx?act=fangjian&amp;roomid=" + roomid + ""), "1");
        }
        else
        {
            builder.Append(Out.Tab("<div>", "<br/>"));
            builder.Append("桌面信息:第" + play.ID + "桌<br/>");
            builder.Append("当前彩池:" + play.Total_Now + "<br/>");
            builder.Append("开庄时间:" + play.AddTime + "<br/>");
            builder.Append("确定封庄吗？封庄后游戏即会结束！");
            builder.Append(Out.Tab("</div>", ""));
            string strText = "";
            string strName = "roomid";
            string strType = "hidden";
            string strValu = "" + play.ID + "";
            string strEmpt = "false";
            string strIdea = "/";
            string strOthe = "确定封庄,bjl.aspx?act=fzhuang,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br/>"));
            builder.Append("<b>注意:桌面正在等待下注的情况下才可以封庄,封庄后立即返还实时彩池到您的账户.</b>");
            builder.Append(Out.Tab("</div>", ""));
        }

        foot();
    }

    //游戏排行
    private void TopPage()
    {
        Master.Title = "" + GameName + "_排行榜";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx") + "\">" + GameName + "</a>&gt;排行榜");
        builder.Append(Out.Tab("</div>", "<br/>"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[1-2]$", "1"));
        if (ptype == 1)
        {
            builder.Append("<h style=\"color:red\">赚币排行" + "</h>" + "|");
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=top&amp;ptype=2") + "\">胜率排行</a>" + "");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=top&amp;ptype=1") + "\">赚币排行</a>" + "|");
            builder.Append("<h style=\"color:red\">胜率排行" + "</h>" + "");
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        string rewardid = "";
        int pageIndex = 1;
        int recordCount;
        string strWhere = string.Empty;
        string strWhere2 = string.Empty;
        string strWhere3 = string.Empty;
        string strWhere4 = string.Empty;
        int pageSize = 10;
        string[] pageValUrl = { "act", "startstate2", "endstate2", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        if (pageIndex > 10)
            pageIndex = 10;

        if (ptype == 1)
        {
            #region
            //if (Utils.GetDomain().Contains("kb288"))
            strWhere = "type>0 GROUP BY buy_usid ORDER BY Sum(GetMoney-PutMoney) DESC";
            strWhere2 = "buy_usid,Sum(GetMoney-PutMoney) as bb";

            strWhere3 = "buy_usid,sum(GetMoney-PutMoney) AS'bb' into #bang3";
            strWhere4 = "WHERE " + strWhere + "";

            DataSet rowbang = new BCW.Baccarat.BLL.BJL_Play().GetList(strWhere2, strWhere);
            recordCount = rowbang.Tables[0].Rows.Count;
            DataSet bang = new BCW.Baccarat.BLL.BJL_Play().GetListByPage2(0, recordCount, strWhere3, strWhere4);
            if (recordCount > 0)
            {
                int k = 1;
                int koo = (pageIndex - 1) * pageSize;
                int skt = pageSize;
                if (recordCount > koo + pageSize)
                {
                    skt = pageSize;
                }
                else
                {
                    skt = recordCount - koo;
                }

                for (int soms = 0; soms < skt; soms++)
                {
                    int usid;
                    long usmoney;
                    usid = Convert.ToInt32(bang.Tables[0].Rows[koo + soms][1]);
                    usmoney = Convert.ToInt64(bang.Tables[0].Rows[koo + soms][2]);
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div>", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br/>"));
                    }
                    string mename = new BCW.BLL.User().GetUsName(usid);
                    int wd = (pageIndex - 1) * 10 + k;
                    builder.Append("[<h style=\"color:red\">第" + wd + "名</h>]<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + usid + "") + "\">" + mename + "(" + usid + ")</a>净赢<h style=\"color:red\">" + usmoney + "</h>" + ub.Get("SiteBz") + "");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                    rewardid = rewardid + usid.ToString() + "#";
                }
                if (recordCount >= 100)
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, 100, Utils.getPageUrl(), pageValUrl, "page", 0));
                else
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录.."));
            }
            #endregion
        }
        else
        {
            #region
            DataSet shoudao = new BCW.Baccarat.BLL.BJL_Play().GetList("buy_usid,COUNT(GetMoney) AS a", "GetMoney>0 GROUP BY buy_usid ORDER BY a DESC");
            if (shoudao != null && shoudao.Tables[0].Rows.Count > 0)
            {
                recordCount = shoudao.Tables[0].Rows.Count;
                int k = 1;
                int koo = (pageIndex - 1) * pageSize;
                int skt = pageSize;
                if (recordCount > koo + pageSize)
                {
                    skt = pageSize;
                }
                else
                {
                    skt = recordCount - koo;
                }
                for (int i = 0; i < skt; i++)
                {
                    int usid = int.Parse(shoudao.Tables[0].Rows[koo + i]["buy_usid"].ToString());
                    int bb = int.Parse(shoudao.Tables[0].Rows[koo + i]["a"].ToString());


                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }
                    builder.Append("[<h style=\"color:red\">第" + ((pageIndex - 1) * pageSize + k) + "名</h>]<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + usid + "") + "\">" + new BCW.BLL.User().GetUsName(usid) + "(" + usid + ")</a>胜<h style=\"color:red\">" + bb + "</h>次");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                if (recordCount >= 100)
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, 100, Utils.getPageUrl(), pageValUrl, "page", 0));
                else
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录.."));
            }
            #endregion
        }
        foot();

    }


    //兑奖开始
    private void CasePage()
    {
        Master.Title = "" + GameName + "_兑奖中心";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("bjl.aspx") + "\">" + GameName + "</a>&gt;兑奖");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("您现在有" + new BCW.BLL.User().GetGold(meid) + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", "<br />"));


        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
        string strWhere = string.Empty;
        strWhere = "buy_usid=" + meid + "and GetMoney >0 and type=2";
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        string strOrder = string.Empty;

        string arrId = "";

        // 开始读取列表
        IList<BCW.Baccarat.Model.BJL_Play> listpay = new BCW.Baccarat.BLL.BJL_Play().GetBJL_Plays(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listpay.Count > 0)
        {
            int k = 1;
            foreach (BCW.Baccarat.Model.BJL_Play n in listpay)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div>", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }

                string[] str = n.PutTypes.Split(',');
                string name1 = string.Empty;
                string[] xiazhu = { "", "庄赢", "闲赢", "和局", "庄单", "庄双", "闲单", "闲双", "投大", "投小", "庄对", "闲对", "任意对", "完美对", };
                for (int ab1 = 0; ab1 < str.Length; ab1++)
                {
                    name1 = name1 + (xiazhu[int.Parse(str[ab1])]) + ",";
                }

                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".下注:" + name1 + "共投" + n.PutMoney + "" + ub.Get("SiteBz") + "");
                builder.Append("(庄" + n.BankerPoint + "点,闲" + n.HunterPoint + "点)[" + DT.FormatDate(n.updatetime, 1) + "]");
                builder.Append("<h style=\"color:red\">赢" + n.GetMoney + "" + ub.Get("SiteBz") + "</h>");
                builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=caseok&amp;pid=" + n.ID + "") + "\">兑奖</a>");

                arrId = arrId + " " + n.ID;
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        //多个兑奖
        if (!string.IsNullOrEmpty(arrId))
        {
            builder.Append(Out.Tab("", "<br />"));
            arrId = arrId.Trim();
            arrId = arrId.Replace(" ", ",");
            string strName = "arrId,act";
            string strValu = "" + arrId + "'casepost";
            string strOthe = "本页兑奖,bjl.aspx,post,0,red";
            builder.Append(Out.wapform(strName, strValu, strOthe));
        }

        foot();
    }

    //单个兑奖
    private void CaseOkPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int pid = Utils.ParseInt(Utils.GetRequest("pid", "get", 2, @"^[0-9]\d*$", "选择兑奖无效"));

        if (new BCW.Baccarat.BLL.BJL_Play().ExistsState(pid, meid))
        {
            BCW.Baccarat.Model.BJL_Play model = new BCW.Baccarat.BLL.BJL_Play().GetBJL_Play2(pid);

            long winMoney = model.GetMoney;//获得该中奖的酷币

            BCW.User.Users.IsFresh("bjl", 2);//防刷

            new BCW.Baccarat.BLL.BJL_Play().update_zd("type=3", "ID=" + pid + "");
            new BCW.BLL.User().UpdateiGold(meid, winMoney, "在" + GameName + "第" + model.RoomID + "桌第" + model.Play_Table + "局兑奖-标识ID" + pid + "");
            Utils.Success("兑奖", "恭喜，成功兑奖" + winMoney + "" + ub.Get("SiteBz") + "", Utils.getUrl("bjl.aspx?act=case"), "1");
        }
        else
        {
            Utils.Success("兑奖", "重复兑奖或没有可以兑奖的记录", Utils.getUrl("bjl.aspx?act=case"), "1");
        }
    }

    //多个兑奖
    private void CasePostPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string arrId = "";
        arrId = Utils.GetRequest("arrId", "post", 1, "", "");

        if (!Utils.IsRegex(arrId.Replace(",", ""), @"^[0-9]\d*$"))
        {
            Utils.Error("选择本页兑奖出错", "");
        }
        string[] strArrId = arrId.Split(",".ToCharArray());
        long getwinMoney = 0;
        long winMoney = 0;
        BCW.User.Users.IsFresh("bjl", 2);//防刷
        for (int i = 0; i < strArrId.Length; i++)
        {
            int pid = Convert.ToInt32(strArrId[i]);

            if (new BCW.Baccarat.BLL.BJL_Play().ExistsState(pid, meid))
            {
                BCW.Baccarat.Model.BJL_Play model = new BCW.Baccarat.BLL.BJL_Play().GetBJL_Play2(pid);

                winMoney = Convert.ToInt64(model.GetMoney);

                new BCW.Baccarat.BLL.BJL_Play().update_zd("type=3", "ID=" + pid + "");

                new BCW.BLL.User().UpdateiGold(meid, winMoney, "在" + GameName + "第" + model.RoomID + "桌第" + model.Play_Table + "局兑奖-标识ID" + pid + "");
            }
            getwinMoney = getwinMoney + winMoney;
        }
        Utils.Success("兑奖", "恭喜，成功兑奖" + getwinMoney + "" + ub.Get("SiteBz") + "", Utils.getUrl("bjl.aspx?act=case"), "1");
    }


    //中奖判断及返奖=====================
    private void updatetable()
    {
        DataSet ds = new BCW.Baccarat.BLL.BJL_Play().GetList("*", "type=0");
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
                int UsID = int.Parse(ds.Tables[0].Rows[i]["UsID"].ToString());
                int RoomID = int.Parse(ds.Tables[0].Rows[i]["RoomID"].ToString());
                int Play_Table = int.Parse(ds.Tables[0].Rows[i]["Play_Table"].ToString());
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
                        new BCW.BLL.User().UpdateiGold(buy_usid, new BCW.BLL.User().GetUsName(buy_usid), tui, "在" + new BCW.BLL.User().GetUsName(UsID) + "(" + UsID + ")的桌面[url=./game/bjl.aspx?act=fangjian&amp;roomid=" + RoomID + "]第" + RoomID + "桌第" + Play_Table + "局[/url]出现和局" + BankerPoint + ":" + HunterPoint + ",退还该下注" + tui + "-标识ID" + ID + "");
                        if (isRobot == 0)
                            new BCW.BLL.Guest().Add(1, buy_usid, new BCW.BLL.User().GetUsName(buy_usid), "在" + GameName + "第" + RoomID + "桌第" + Play_Table + "局开奖:庄" + BankerPoint + "点|闲" + HunterPoint + "点为和局,退还该下注" + tui + "" + ub.Get("SiteBz") + ".");
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

        DataSet ds1 = new BCW.Baccarat.BLL.BJL_Room().GetList("*", "Total_Now<LowTotal and state=0");
        if (ds1 != null && ds1.Tables[0].Rows.Count > 0)
        {
            #region 判断最高下注是否少于最低下注，如果是，则封庄
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

        DataSet ds2 = new BCW.Baccarat.BLL.BJL_Room().GetList2("*", "a LEFT JOIN tb_BJL_Play b ON a.UsID=b.UsID AND b.Play_Table>=" + RoomTime2 + " AND a.state=0");
        if (ds2 != null && ds2.Tables[0].Rows.Count > 0)
        {
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
        }

    }

    //判断点数
    private int Point(string Poker)
    {
        string[] pokerrank = { "", "", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
        System.Text.StringBuilder builder2 = new System.Text.StringBuilder("");
        string[] poker = Poker.Split(',');
        int point;
        int sum = 0;
        for (int i = 0; i < poker.Length; i++)
        {
            point = int.Parse(poker[i]);

            if (i % 2 != 0)
            {
                if (pokerrank[point] == "J")
                {
                    sum = (sum + 0) % 10;
                }
                else if (pokerrank[point] == "Q")
                {
                    sum = (sum + 0) % 10;
                }
                else if (pokerrank[point] == "K")
                {
                    sum = (sum + 0) % 10;
                }
                else if (pokerrank[point] == "A")
                {
                    sum = (sum + 1) % 10;
                }
                else
                {
                    int rank;
                    rank = int.Parse(pokerrank[point]);
                    sum = (sum + rank) % 10;
                }
            }
        }
        return sum;
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

    //判断类型
    private string Types(string type)
    {
        string bettypes = "";
        if (type == "1")
            bettypes = "庄赢";
        else if (type == "2")
            bettypes = "闲赢";
        else if (type == "3")
            bettypes = "和局";
        else if (type == "4")
            bettypes = "庄单";
        else if (type == "5")
            bettypes = "庄双";
        else if (type == "6")
            bettypes = "闲单";
        else if (type == "7")
            bettypes = "闲双";
        else if (type == "8")
            bettypes = "投小";
        else if (type == "9")
            bettypes = "投大";
        else if (type == "10")
            bettypes = "庄对";
        else if (type == "11")
            bettypes = "闲对";
        else if (type == "12")
            bettypes = "任意对";
        else if (type == "13")
            bettypes = "完美对";
        return bettypes;
    }

    //输出点数和牌==文字
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

    //选择各类型赔率
    private double betpercent(int p)
    {
        double percent = Convert.ToDouble(ub.GetSub("baccaratpercent" + p + "", xmlPath));
        return percent;
    }

    //牌型判断并输出==点数发牌
    private string Poker(string s1, string s2, int bankersum)
    {
        string[] pokersuit = { "", "方块", "梅花", "红桃", "黑桃" };
        string[] pokerrank = { "", "", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
        System.Text.StringBuilder builder2 = new System.Text.StringBuilder("");
        if (pokerrank[int.Parse(s2)] == "J")
        {
            bankersum = (bankersum + 0) % 10;
        }
        else if (pokerrank[int.Parse(s2)] == "Q")
        {
            bankersum = (bankersum + 0) % 10;
        }
        else if (pokerrank[int.Parse(s2)] == "K")
        {
            bankersum = (bankersum + 0) % 10;
        }
        else if (pokerrank[int.Parse(s2)] == "A")
        {
            bankersum = (bankersum + 1) % 10;
        }
        else
        {
            int bankerrank;
            bankerrank = int.Parse(pokerrank[int.Parse(s2)]);
            bankersum = (bankersum + bankerrank) % 10;
        }
        builder2.Append(pokersuit[int.Parse(s1)] + pokerrank[int.Parse(s2)] + "(" + bankersum + " 点)");
        builder2.Append("#" + bankersum);
        return builder2.ToString();
    }

    //牌型判断并输出==图片发牌
    private string OnePoker(string s1, string s2)
    {
        string[] pokersuit = { "", "方块", "梅花", "红桃", "黑桃" };
        string[] pokerrank = { "", "", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
        System.Text.StringBuilder builder2 = new System.Text.StringBuilder("");
        builder2.Append(pokersuit[int.Parse(s1)] + pokerrank[int.Parse(s2)]);
        return builder2.ToString();
    }

    //扑克牌图片输出
    private void PokerPicture(string poker)
    {
        #region 方块牌图片输出
        if (poker == "方块A")
        {
            builder.Append("<img src=\"" + baccarat_img + "40.jpg" + "\"  alt=\"load\"/>");
        }
        else if (poker == "方块2")
        {
            builder.Append("<img src=\"" + baccarat_img + "41.jpg" + "\"  alt=\"load\"/>");
        }
        else if (poker == "方块3")
        {
            builder.Append("<img src=\"" + baccarat_img + "42.jpg" + "\"  alt=\"load\"/>");
        }
        else if (poker == "方块4")
        {
            builder.Append("<img src=\"" + baccarat_img + "43.jpg" + "\"  alt=\"load\"/>");
        }
        else if (poker == "方块5")
        {
            builder.Append("<img src=\"" + baccarat_img + "44.jpg" + "\"  alt=\"load\"/>");
        }
        else if (poker == "方块6")
        {
            builder.Append("<img src=\"" + baccarat_img + "45.jpg" + "\"  alt=\"load\"/>");
        }
        else if (poker == "方块7")
        {
            builder.Append("<img src=\"" + baccarat_img + "46.jpg" + "\"  alt=\"load\"/>");
        }
        else if (poker == "方块8")
        {
            builder.Append("<img src=\"" + baccarat_img + "47.jpg" + "\"  alt=\"load\"/>");
        }
        else if (poker == "方块9")
        {
            builder.Append("<img src=\"" + baccarat_img + "48.jpg" + "\"  alt=\"load\"/>");
        }
        else if (poker == "方块10")
        {
            builder.Append("<img src=\"" + baccarat_img + "49.jpg" + "\"  alt=\"load\"/>");
        }
        else if (poker == "方块J")
        {
            builder.Append("<img src=\"" + baccarat_img + "50.jpg" + "\"  alt=\"load\"/>");
        }
        else if (poker == "方块Q")
        {
            builder.Append("<img src=\"" + baccarat_img + "51.jpg" + "\"  alt=\"load\"/>");
        }
        else if (poker == "方块K")
        {
            builder.Append("<img src=\"" + baccarat_img + "52.jpg" + "\"  alt=\"load\"/>");
        }
        #endregion

        #region 梅花牌图片输出
        else if (poker == "梅花A")
        {
            builder.Append("<img src=\"" + baccarat_img + "27.jpg" + "\"  alt=\"load\"/>");
        }
        else if (poker == "梅花2")
        {
            builder.Append("<img src=\"" + baccarat_img + "28.jpg" + "\"  alt=\"load\"/>");
        }
        else if (poker == "梅花3")
        {
            builder.Append("<img src=\"" + baccarat_img + "29.jpg" + "\"  alt=\"load\"/>");
        }
        else if (poker == "梅花4")
        {
            builder.Append("<img src=\"" + baccarat_img + "30.jpg" + "\"  alt=\"load\"/>");
        }
        else if (poker == "梅花5")
        {
            builder.Append("<img src=\"" + baccarat_img + "31.jpg" + "\"  alt=\"load\"/>");
        }
        else if (poker == "梅花6")
        {
            builder.Append("<img src=\"" + baccarat_img + "32.jpg" + "\"  alt=\"load\"/>");
        }
        else if (poker == "梅花7")
        {
            builder.Append("<img src=\"" + baccarat_img + "33.jpg" + "\"  alt=\"load\"/>");
        }
        else if (poker == "梅花8")
        {
            builder.Append("<img src=\"" + baccarat_img + "34.jpg" + "\"  alt=\"load\"/>");
        }
        else if (poker == "梅花9")
        {
            builder.Append("<img src=\"" + baccarat_img + "35.jpg" + "\"  alt=\"load\"/>");
        }
        else if (poker == "梅花10")
        {
            builder.Append("<img src=\"" + baccarat_img + "36.jpg" + "\"  alt=\"load\"/>");
        }
        else if (poker == "梅花J")
        {
            builder.Append("<img src=\"" + baccarat_img + "37.jpg" + "\"  alt=\"load\"/>");
        }
        else if (poker == "梅花Q")
        {
            builder.Append("<img src=\"" + baccarat_img + "38.jpg" + "\"  alt=\"load\"/>");
        }
        else if (poker == "梅花K")
        {
            builder.Append("<img src=\"" + baccarat_img + "39.jpg" + "\"  alt=\"load\"/>");
        }
        #endregion

        #region 红桃牌图片输出
        else if (poker == "红桃A")
        {
            builder.Append("<img src=\"" + baccarat_img + "14.jpg" + "\"  alt=\"load\"/>");
        }
        else if (poker == "红桃2")
        {
            builder.Append("<img src=\"" + baccarat_img + "15.jpg" + "\"  alt=\"load\"/>");
        }
        else if (poker == "红桃3")
        {
            builder.Append("<img src=\"" + baccarat_img + "16.jpg" + "\"  alt=\"load\"/>");
        }
        else if (poker == "红桃4")
        {
            builder.Append("<img src=\"" + baccarat_img + "17.jpg" + "\"  alt=\"load\"/>");
        }
        else if (poker == "红桃5")
        {
            builder.Append("<img src=\"" + baccarat_img + "18.jpg" + "\"  alt=\"load\"/>");
        }
        else if (poker == "红桃6")
        {
            builder.Append("<img src=\"" + baccarat_img + "19.jpg" + "\"  alt=\"load\"/>");
        }
        else if (poker == "红桃7")
        {
            builder.Append("<img src=\"" + baccarat_img + "20.jpg" + "\"  alt=\"load\"/>");
        }
        else if (poker == "红桃8")
        {
            builder.Append("<img src=\"" + baccarat_img + "21.jpg" + "\"  alt=\"load\"/>");
        }
        else if (poker == "红桃9")
        {
            builder.Append("<img src=\"" + baccarat_img + "22.jpg" + "\"  alt=\"load\"/>");
        }
        else if (poker == "红桃10")
        {
            builder.Append("<img src=\"" + baccarat_img + "23.jpg" + "\"  alt=\"load\"/>");
        }
        else if (poker == "红桃J")
        {
            builder.Append("<img src=\"" + baccarat_img + "24.jpg" + "\"  alt=\"load\"/>");
        }
        else if (poker == "红桃Q")
        {
            builder.Append("<img src=\"" + baccarat_img + "25.jpg" + "\"  alt=\"load\"/>");
        }
        else if (poker == "红桃K")
        {
            builder.Append("<img src=\"" + baccarat_img + "26.jpg" + "\"  alt=\"load\"/>");
        }
        #endregion

        #region 黑桃牌图片输出
        else if (poker == "黑桃A")
        {
            builder.Append("<img src=\"" + baccarat_img + "1.jpg" + "\"  alt=\"load\"/>");
        }
        else if (poker == "黑桃2")
        {
            builder.Append("<img src=\"" + baccarat_img + "2.jpg" + "\"  alt=\"load\"/>");
        }
        else if (poker == "黑桃3")
        {
            builder.Append("<img src=\"" + baccarat_img + "3.jpg" + "\"  alt=\"load\"/>");
        }
        else if (poker == "黑桃4")
        {
            builder.Append("<img src=\"" + baccarat_img + "4.jpg" + "\"  alt=\"load\"/>");
        }
        else if (poker == "黑桃5")
        {
            builder.Append("<img src=\"" + baccarat_img + "5.jpg" + "\"  alt=\"load\"/>");
        }
        else if (poker == "黑桃6")
        {
            builder.Append("<img src=\"" + baccarat_img + "6.jpg" + "\"  alt=\"load\"/>");
        }
        else if (poker == "黑桃7")
        {
            builder.Append("<img src=\"" + baccarat_img + "7.jpg" + "\"  alt=\"load\"/>");
        }
        else if (poker == "黑桃8")
        {
            builder.Append("<img src=\"" + baccarat_img + "8.jpg" + "\"  alt=\"load\"/>");
        }
        else if (poker == "黑桃9")
        {
            builder.Append("<img src=\"" + baccarat_img + "9.jpg" + "\"  alt=\"load\"/>");
        }
        else if (poker == "黑桃10")
        {
            builder.Append("<img src=\"" + baccarat_img + "10.jpg" + "\"  alt=\"load\"/>");
        }
        else if (poker == "黑桃J")
        {
            builder.Append("<img src=\"" + baccarat_img + "11.jpg" + "\"  alt=\"load\"/>");
        }
        else if (poker == "黑桃Q")
        {
            builder.Append("<img src=\"" + baccarat_img + "12.jpg" + "\"  alt=\"load\"/>");
        }
        else if (poker == "黑桃K")
        {
            builder.Append("<img src=\"" + baccarat_img + "13.jpg" + "\"  alt=\"load\"/>");
        }
        #endregion
    }

    //扑克牌点数输出
    private int PokerPoint(string point, int pokersum)
    {
        string[] pokerrank = { "", "", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
        if (pokerrank[int.Parse(point)] == "J" || pokerrank[int.Parse(point)] == "Q" || pokerrank[int.Parse(point)] == "K")
        {
            pokersum = (pokersum + 0) % 10;
        }
        else if (pokerrank[int.Parse(point)] == "A")
        {
            pokersum = (pokersum + 1) % 10;
        }
        else
        {
            int bankerrank;
            bankerrank = int.Parse(pokerrank[int.Parse(point)]);
            pokersum = (pokersum + bankerrank) % 10;
        }
        return pokersum;
    }

    //底部链接
    private void foot()
    {
        //游戏底部Ubb
        string Foot = ub.GetSub("BaccaratFoot", xmlPath);
        if (Foot != "")
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(Foot)));
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

}
