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
/// <summary>
/// 增加小号管理内线功能 20160813 姚志光
/// 修改小号管理内线功能 20160815 陈志基
/// </summary>
public partial class bbs_guest : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/guest.xml";
    protected string strText = string.Empty;
    protected string strName = string.Empty;
    protected string strType = string.Empty;
    protected string strValu = string.Empty;
    protected string strEmpt = string.Empty;
    protected string strIdea = string.Empty;
    protected string strOthe = string.Empty;
    protected int et = Convert.ToInt32(ub.Get("SiteExTime"));
    protected string gestName = (ub.GetSub("gestName", "/Controls/guestlist.xml"));
    //protected string gestName = Convert.ToString(ub.GetSub("gestName", "/Controls/guestlist.xml"));//小号内线
    protected string guestsee = (ub.GetSub("guestsee", "/Controls/guestlist.xml"));//小号使用者
    protected string MidUidLists = (ub.GetSub("MidUidLists", "/Controls/guestlist.xml"));//小号
    protected string guestnew = (ub.GetSub("guestnew", "/Controls/guestlist.xml"));//接口ID 
    protected string guesttime = (ub.GetSub("guesttime", "/Controls/guestlist.xml"));//接口ID 
    protected string guestopen = (ub.GetSub("guestopen", "/Controls/guestlist.xml"));//接口开关
    protected void Page_Load(object sender, EventArgs e)
    {
        //维护提示
        if (ub.GetSub("GuestStatus", xmlPath) == "1")
        {
            Utils.Safe("内线系统");
        }
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "newlist":
                NewListPage(meid);
                break;
            case "add":
                AddPage(meid);
                break;
            case "save":
                SavePage(meid);
                break;
            case "addall":
                AddallPage(meid);
                break;
            case "saveall":
                SaveallPage(meid);
                break;
            case "recommend":
                RecomPage(meid);
                break;
            case "recomsave":
                RecomSavePage(meid);
                break;
            case "reportsave":
                ReportSavePage(meid);
                break;
            case "recommend2":
                Recom2Page(meid);
                break;
            case "reportsave2":
                ReportSave2Page(meid);
                break;
            case "view":
                ViewPage(meid);
                break;
            case "keep":
                KeepPage(meid);
                break;
            case "del":
                DelPage(meid);
                break;
            case "delpage":
                DelPagePage(meid);
                break;
            case "delptype":
                DelPtypePage(meid);
                break;
            case "read":
                ReadPage(meid);
                break;
            case "chat":
                ChatPage(meid);
                break;
            case "delhid":
                DelHidPage(meid);
                break;
            case "dels":
                DelsPage(meid);
                break;
            case "delx":
                DelxPage(meid);
                break;
            case "trans":
                TransPage(meid);
                break;
            case "smsmail":
                SmsMailPage(meid);
                break;
            case "smsmailhelp":
                SmsMailHelpPage(meid);
                break;
            case "numsPwd":
                numsPwd();
                break;
            case "createPwd":
                createPwd();
                break;
            case "fgPwd":
                fgPwd();
                break;
            case "newguest1":
                NewGuest1Manage(meid);
                break;
            case "newguest":
                NewGuestManage(meid);
                break;
            case "newview":
                ViewPageNew(meid);
                break;
            case "newsave":
                NewSavePage(meid);
                break;
            case "newchat":
                NewChatPage(meid);
                break;
            case "inuser":
                InUserPage();
                break;
            case "login":
                InUserPageSecond();
                break;
            case "sendok":
                SendOkPage(meid);
                break;
            default:
                ReloadPage(meid);
                break;
        }
    }
    //新加的页面 确定消息已阅
    private void SendOkPage(int meid)
    {
        Master.Title = "我的内线";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("已阅系统消息红包奖励");
        builder.Append(Out.Tab("</div>", "<br/>"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-3]$", "0"));
        int ID = int.Parse(Utils.GetRequest("ID", "get", 1, @"", "0"));
        if (!new BCW.BLL.Guest().Exists(ID))
        {
            Utils.Error("不存在的内线消息！", "");
        }

        try
        {
            BCW.Model.Guest model = new BCW.BLL.Guest().GetGuest(ID);
            if (meid != model.ToId)
            {
                Utils.Error("不存在的内线消息！", "");
            }
            if (model.Types != 5)
            {
                BCW.Model.tb_GuestSendList list = new BCW.BLL.tb_GuestSendList().Gettb_GuestSendListForUsidGuestID(meid, ID);
                if (!new BCW.BLL.tb_GuestSend().Exists(Convert.ToInt32(list.guestsendID)))
                {
                    Utils.Error("不存在的内线记录！", "");
                }
                if (list.type == 4)
                {
                    Utils.Error("本次阅读没有获得红包中奖！", "");
                }
                else
                {
                    if (list.getGold <= 0)
                    { Utils.Error("该系统消息已阅！", ""); }
                    builder.Append(Out.Tab("<div class=\"text\">", ""));
                    builder.Append("<font color=\"green\">恭喜,阅读获得系统奖励的红包</font>");
                    builder.Append("<b style=\" background:#FF8C00;color:#FFF\">拼</b> ");
                    if (list.getGold > 1000)
                    { builder.Append("<font color=\"red\"><b>" + list.getGold + "</b></font><font color=\"green\">" + ub.Get("SiteBz") + "</font><br/>"); }
                    else
                    {
                        builder.Append("<font color=\"red\">" + list.getGold + "</font><font color=\"green\">" + ub.Get("SiteBz") + "</font><br/>");
                    }
                    builder.Append("<img src=\"/bbs/game/img/hb.gif\"  alt=\"load\" />");
                    builder.Append("红包拼手气,本次红包已存入账户!<br/>");
                    builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">返回上级</a>");
                    builder.Append(Out.Tab("</div>", ""));
                }
            }
            else
            {
                string guestsendMustHitID = "#" + (ub.GetSub("guestsendMustHitID", "/Controls/guestsend.xml")) + "#";
                string guestsendMustNotHit = "#" + (ub.GetSub("guestsendMustNotHit", "/Controls/guestsend.xml")) + "#";
                new BCW.BLL.Guest().UpdateTypes(ID, 6);//确定已阅     
                BCW.Model.tb_GuestSendList list = new BCW.BLL.tb_GuestSendList().Gettb_GuestSendListForUsidGuestID(meid, ID);
                if (list == null)
                { Utils.Error("不存在的内线消息！", ""); }
                int guestsendID = Convert.ToInt32(list.guestsendID);
                if (new BCW.BLL.tb_GuestSendList().ExistsUidType(meid, guestsendID, 1))
                {
                    Utils.Error("本消息已阅读!", "");
                }
                int guestsendList = 0;
                if (list.type == 0)
                { Utils.Error("该系统消息已阅！", ""); }
                if (list.type == 4)
                {
                    Utils.Error("本次阅读没有获得红包中奖！", "");
                }
                //派红包
                if (!new BCW.BLL.tb_GuestSend().Exists(guestsendID))
                {
                    //   Utils.Error("不存在的内线记录！", "");
                    Utils.Success("不存在的内线记录！", "不存在的内线记录,2s后返回上一级..", Utils.getUrl("guest.aspx?ptype=1&amp;"), "2");
                }
                BCW.Model.tb_GuestSend guestsend = new BCW.BLL.tb_GuestSend().Gettb_GuestSend(guestsendID);
                int allCount = Convert.ToInt32(guestsend.maxCount);
                int getCount = Convert.ToInt32(guestsend.getCount);
                int usergetGold = 0;
                Random ran = new Random(unchecked((int)DateTime.Now.Ticks));
                int temp = ran.Next(0, allCount);
                list.type = 0;
                bool check = false;
                //检查ID
                if (temp < Convert.ToInt32(guestsend.getCount) || guestsendMustHitID.Contains("#" + meid + "#"))
                {
                    check = true;
                }
                if (guestsendMustNotHit.Contains("#" + meid + "#"))
                {
                    check = false;
                }
                if (check)
                {
                    string[] gold = guestsend.hbList.Split('#');
                    try
                    {
                        usergetGold = Convert.ToInt32(gold[0]);
                    }
                    catch { usergetGold = ran.Next(1, 10); }
                    list.type = 0;
                    int longth = gold[0].ToString().Length;
                    guestsend.hbList = guestsend.hbList.Remove(0, longth + 1);
                    guestsend.nowgold -= usergetGold;
                    guestsend.seeUidList += meid.ToString() + "#";
                    guestsend.seeCount += 1;
                    guestsend.notSeenIDList = ("#" + guestsend.notSeenIDList).Replace("#" + meid.ToString() + "#", "");
                    new BCW.BLL.User().UpdateiGold(meid, usergetGold, "内线阅读与熟知奖励红包ID" + guestsend.ID + "#编号" + list.ID);
                    builder.Append(Out.Tab("<div class=\"text\">", ""));
                    builder.Append("<font color=\"green\">恭喜,阅读获得系统奖励的红包</font>");
                    builder.Append("<b style=\" background:#FF8C00;color:#FFF\">拼</b> ");
                    if (usergetGold > 1000)
                    { builder.Append("<font color=\"red\"><b>" + usergetGold + "</b></font><font color=\"green\">" + ub.Get("SiteBz") + "</font><br/>"); }
                    else
                    {
                        builder.Append("<font color=\"red\">" + usergetGold + "</font><font color=\"green\">" + ub.Get("SiteBz") + "</font><br/>");
                    }
                    builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">返回上级</a>");
                    builder.Append(Out.Tab("</div>", ""));
                    list.getGold = usergetGold;
                    list.overtime = DateTime.Now;
                    new BCW.BLL.tb_GuestSend().Update(guestsend);
                    new BCW.BLL.tb_GuestSendList().Update(list);
                }
                else
                {
                    guestsend.seeCount += 1;
                    guestsend.seeUidList += meid.ToString() + "#";
                    list.type = 4;//不中奖
                    list.getGold = usergetGold;
                    list.overtime = DateTime.Now;
                    new BCW.BLL.tb_GuestSend().Update(guestsend);
                    new BCW.BLL.tb_GuestSendList().Update(list);
                    //builder.Append("差一点点，本次阅读后与系统奖励的红包擦肩而过...");
                    Utils.Success("差一点点！", "本次阅读与系统奖励的红包擦肩而过,2s后返回上一级..", Utils.getUrl("guest.aspx?ptype=1&amp;"), "2");
                }
            }
        }
        catch { Utils.Error("阅读成功!", ""); }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        //builder.Append(ShowBackPage(meid));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx") + "\">空间</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    //打开总管理新窗口
    private void InUserPage()
    {
        //  int hid = int.Parse(Utils.GetRequest("hid", "get", 2, @"^[0-9]\d*$", "ID错误"));
        int returnid = Convert.ToInt32(ub.GetSub("guestnew", "/Controls/guestlist.xml"));
        int hid = returnid;
        BCW.Model.User model = new BCW.BLL.User().GetKey(hid);
        if (model == null)
        {
            Utils.Error("不存在的ID", "");
        }
        //管理员可以进入小号
        string XIAOHAO = "#" + ub.GetSub("XiaoHao", "/Controls/xiaohao.xml") + "#";
        string Mobile = new BCW.BLL.User().GetMobile(hid);
        //if ((!("#" + XIAOHAO + "#").Contains("#" + hid + "#")) && Mobile != "15107582999")
        //{
        //    int ManageId = new BCW.User.Manage().IsManageLogin();
        //    if (ManageId != 1 && ManageId != 11 && ManageId != 13)
        //    {
        //        Utils.Error("此功能暂停开放", "");
        //    }
        //}
        //if (hid == 19611)
        //{
        //    Utils.Error("你已越权操作", "");
        //}
        //设置keys
        string keys = "";
        keys = BCW.User.Users.SetUserKeys(hid, model.UsPwd, model.UsKey);
        string VE = ConfigHelper.GetConfigString("VE");
        string SID = ConfigHelper.GetConfigString("SID");
        string bUrl = string.Empty;
        //if (meid==int.Parse(guestnew))
        //{
        //    bUrl = "guest.aspx?act=newguest&amp;" + VE + "=" + Utils.getstrVe() + "&amp;" + SID + "=" + keys + "";
        //}
        //else
        //{
        //    bUrl = "guest.aspx?act=newguest1&amp;" + VE + "=" + Utils.getstrVe() + "&amp;" + SID + "=" + keys + "";
        //}
        bUrl = "guest.aspx?act=newguest&amp;" + VE + "=" + Utils.getstrVe() + "&amp;" + SID + "=" + keys + "";
        //清Cookie
        HttpCookie Cookie = new HttpCookie("LoginComment");
        Cookie.Expires = DateTime.Now.AddDays(-1);
        HttpContext.Current.Response.Cookies.Add(Cookie);
        Utils.Success("进入主管理号", "正在进入..", bUrl, "1");
    }

    //新登录窗口
    private void InUserPageSecond()
    {
        // int hid = int.Parse(Utils.GetRequest("hid", "get", 2, @"^[0-9]\d*$", "1ID错误"));//用户ID
        string hidstr = Utils.GetRequest("hid", "get", 1, "", "");
        //Utils.Error(""+ hidstr, "");
        try
        {
            hidstr = new BCW.BLL.Security().DecryptQueryString(hidstr);
        }
        catch
        {
            Utils.Error("不存在的号码ID11", "/login.aspx");
        }
        string MidUidLists = (ub.GetSub("MidUidLists", "/Controls/guestlist.xml"));//小号
        int flag = 0;
        string[] strnums = MidUidLists.Split('#');
        for(int i=0;i< strnums.Length; i++) //只能用for判断，string1.contain(string)可能出错（string1太长带有换行符）
        {
            builder.Append(strnums[i]+",");
            string temp = strnums[i];
            if (hidstr.ToString() == temp|| (hidstr.ToString()+"\r\n")== temp)//如果找到相等的
            {
                flag = 1;
            }
        }
        if (flag == 0)
        {
            Utils.Error("不存在的号码ID222", "/login.aspx");
        }
        int hid = int.Parse(hidstr);//用户ID
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 2, @"^[0-9]\d*$", "3ID错误"));

        new BCW.BLL.User().UpdateTime(hid);
        BCW.Model.User model = new BCW.BLL.User().GetKey(hid);
        if (model == null)
        {
            Utils.Error("不存在的用户", "");
        }
        //管理员可以进入小号
        string XIAOHAO = "#" + ub.GetSub("XiaoHao", "/Controls/xiaohao.xml") + "#";
        string Mobile = new BCW.BLL.User().GetMobile(hid);
        string ForumSet = new BCW.BLL.User().GetForumSet(hid);
        string info = Utils.GetRequest("info", "post", 1, "", "");
        int Times = 0;//关闭IP异常
        if (Times < 0 || Times > 1440)
            Utils.Error("选择超时时间错误", "");

        DateTime dt = DateTime.Now;
        if (Times > 0)
            dt = dt.AddMinutes(Times);

        string[] fs = ForumSet.Split(",".ToCharArray());
        string sforumsets = string.Empty;
        for (int i = 0; i < fs.Length; i++)
        {
            string[] sfs = fs[i].ToString().Split("|".ToCharArray());
            if (i == 23)//支付超时
            {
                sforumsets += "," + sfs[0] + "|" + 0;//设置为关闭
            }
            else if (i == 25)//Ip变动时间
            {
                sforumsets += "," + sfs[0] + "|" + dt;
            }
            else if (i == 26)//IP超时时间 0关闭
            {
                sforumsets += "," + sfs[0] + "|" + Times;
            }
            else
            {
                sforumsets += "," + sfs[0] + "|" + sfs[1];
            }
        }
        sforumsets = Utils.Mid(sforumsets, 1, sforumsets.Length);
        new BCW.BLL.User().UpdateTime(hid);
        new BCW.BLL.User().UpdateIpTime(hid);
        new BCW.BLL.User().UpdateForumSet(hid, sforumsets);
        //设置keys
        string keys = "";
        keys = BCW.User.Users.SetUserKeys(hid, model.UsPwd, model.UsKey);
        string VE = ConfigHelper.GetConfigString("VE");
        string SID = ConfigHelper.GetConfigString("SID");
        string bUrl = string.Empty;
        if (ptype == 0)
        {
            bUrl = "/bbs/forum.aspx?" + VE + "=" + Utils.getstrVe() + "&amp;" + SID + "=" + keys + "";//论坛
        }
        else if (ptype == 1)
        {
            bUrl = "/default.aspx?id=13667&amp;ve=2a&amp;" + VE + "=" + Utils.getstrVe() + "&amp;" + SID + "=" + keys + "";//游戏
        }
        else if (ptype == 2)
        {
            bUrl = "game/speak.aspx?" + VE + "=" + Utils.getstrVe() + "&amp;" + SID + "=" + keys + "";//闲聊
        }
        else if (ptype == 3)
        {
            bUrl = "./chatroom.aspx?&amp;id=29&amp;" + VE + "=" + Utils.getstrVe() + "&amp;" + SID + "=" + keys + "";//群聊
        }
        else if (ptype == 4)
        {
            if (Utils.GetRequest("fromId", "get", 1, "", "") == "")
            {
                bUrl = "./uinfo.aspx?&amp;" + VE + "=" + Utils.getstrVe() + "&amp;" + SID + "=" + keys + "";//空间
            }
            else
            {
                int fromId = int.Parse(Utils.GetRequest("fromId", "get", 1, "", ""));//发用户ID
                bUrl = "./uinfo.aspx?uid=" + fromId + "&amp;" + VE + "=" + Utils.getstrVe() + "&amp;" + SID + "=" + keys + "";//空间
            }
        }
        else if (ptype == 5)
        {
            bUrl = "/default.aspx?&amp;ve=2a&amp;" + VE + "=" + Utils.getstrVe() + "&amp;" + SID + "=" + keys + "";//主页
        }
        else if (ptype == 6)//内线
        {
            int Id = int.Parse(Utils.GetRequest("Id", "get", 2, @"^[0-9]\d*$", "内线ID错误"));//内线ID
            int fromId = int.Parse(Utils.GetRequest("fromId", "get", 2, @"^[0-9]\d*$", "发内线用户ID错误"));//发用户ID
            bUrl = "/bbs/guest.aspx?act=view&amp;hid=" + fromId + "&amp;id=" + Id + "&amp;" + VE + "=" + Utils.getstrVe() + "&amp;" + SID + "=" + keys + "";//进入内线
        }
        //  bUrl = "guest.aspx?act=newguest&amp;" + VE + "=" + Utils.getstrVe() + "&amp;" + SID + "=" + keys + "";
        //Utils.Error("" + bUrl + "", "");
        //清Cookie
        HttpCookie Cookie = new HttpCookie("LoginComment");
        Cookie.Expires = DateTime.Now.AddDays(-1);
        HttpContext.Current.Response.Cookies.Add(Cookie);
        Utils.Success("进入主管理号", "正在进入..", bUrl, "1");


    }

    //newchat 小号管理对聊
    private void NewChatPage(int uid)
    {
        Master.Title = "聊天模式对话";
        int hid = int.Parse(Utils.GetRequest("hid", "get", 1, @"^[0-9]\d*$", "-1"));
        uid = int.Parse(Utils.GetRequest("uid", "get", 1, @"^[0-9]\d*$", "-1"));//本地
        int ordertype = int.Parse(Utils.GetRequest("ordertype", "get", 1, @"^[0-1]$", "0"));

        if (hid == -1)
        {
            Utils.Error("不存在的会员ID", "");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        if (hid != 0 && !new BCW.BLL.User().Exists(hid))
        {
            Utils.Error("不存在的会员ID", "");
        }

        if (hid != 0)
        {
            string UsName = new BCW.BLL.User().GetUsName(hid);
            builder.Append(new BCW.BLL.User().GetUsName(uid) + "正在和<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + hid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + UsName + "(" + hid + ")</a>对聊");
        }
        else
        {
            builder.Append("正在和系统对聊");
            builder.Append("|<a href=\"" + Utils.getUrl("guest.aspx?act=newguest&amp;backurl=" + Utils.getPage(0) + "") + "\">返回新内线</a>");
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        if (hid > 0)
        {
            builder.Append(Out.Tab("<div>", ""));
            //  builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=add&amp;hid=" + hid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">回复</a>/");
            //  builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=delhid&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">清空记录</a>/");
            if (ordertype == 1)
                builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=newchat&amp;hid=" + hid + "&amp;uid=" + uid + "&amp;ordertype=0&amp;backurl=" + Utils.getPage(0) + "") + "\">倒序</a>/");
            else
                builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=newchat&amp;hid=" + hid + "&amp;uid=" + uid + "&amp;ordertype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">正序</a>/");

            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=newchat&amp;hid=" + hid + "&amp;uid=" + uid + "&amp;ordertype=" + ordertype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">刷新</a>/");
            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=newguest&amp;backurl=" + Utils.getPage(0) + "") + "\">返回新内线</a>");
            builder.Append(Out.Tab("</div>", "<br />"));

            strText = ",,,,,";
            strName = "fromid,toid,Content,act,backurl";
            strType = "hidden,hidden,text,hidden,hidden";
            strValu = "" + uid + "'" + hid + "''newsave'" + Utils.PostPage(1) + "";
            strEmpt = "flase,false,true,false,false";
            strIdea = "";
            strOthe = "快速回复,guest.aspx,post,3,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("", "<br />"));
        }
        int pageIndex;
        int recordCount;
        //int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        int pageSize = 6;
        string strWhere = "";
        string strOrder = "";
        string[] pageValUrl = { "act", "hid", "ptype", "backurl", "uid" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "(FromId = " + uid + " and ToId = " + hid + " and FDel = 0) OR (FromId = " + hid + " and ToId = " + uid + " and TDel = 0)";
        if (ordertype == 1)
            strOrder = "ID Asc";
        else
            strOrder = "ID Desc";

        // 开始读取列表
        IList<BCW.Model.Guest> listGuest = new BCW.BLL.Guest().GetGuests(pageIndex, pageSize, strWhere, strOrder, out recordCount);
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
                string strTrans = string.Empty;
                string Content = Out.SysUBB(n.Content.Replace("showGuess.aspx?gid=", "/bbs/guess/showGuess.aspx?gid="));
                if (n.TransId > 0)
                {
                    strTrans = "[转自<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.TransId + "&amp;uid=" + uid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.TransId + "</a>]";
                }

                if (n.FromId == uid)
                {
                    builder.Append(new BCW.BLL.User().GetUsName(n.FromId) + "说:" + Content + "" + strTrans + "<small>(" + DT.FormatDate(n.AddTime, 1) + ")</small>");//小号说
                }
                else
                {
                    if (hid > 0)
                    {
                        builder.Append("<a href =\"" + Utils.getUrl("uinfo.aspx?uid=" + n.FromId + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(n.FromId) + "</a>" + "说:" + Content + "" + strTrans + "<small>(" + DT.FormatDate(n.AddTime, 1) + ")</small>");//他说
                        //  builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=view&amp;id=" + n.ID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">回复</a>");
                    }
                    else
                    {
                        builder.Append("系统:" + Content + "" + strTrans + "<small>(" + DT.FormatDate(n.AddTime, 1) + ")</small>");
                    }
                    //更新为已读
                    if (n.IsSeen == 0)
                    {
                        new BCW.BLL.Guest().UpdateIsSeen(n.ID);
                    }
                }
                builder.Append("<br />---------");
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
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append(ShowBackPage(uid));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("guest.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?backurl=" + Utils.getPage(0) + "") + "\">内线</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    //newsave 小号回复保存
    private void NewSavePage(int uid)
    {
        uid = int.Parse(Utils.GetRequest("fromid", "post", 1, @"^[1-9]\d*$", "0"));//本ID 52775
        BCW.User.Users.ShowVerifyRole("o", uid);//非验证会员提示
        new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Guest, uid);//会员本身权限
        int toid = int.Parse(Utils.GetRequest("toid", "post", 1, @"^[1-9]\d*$", "0"));
        int itoid = int.Parse(Utils.GetRequest("itoid", "post", 1, @"^[1-9]\d*$", "0"));
        int transid = int.Parse(Utils.GetRequest("transid", "post", 1, @"^(-)*[0-9]\d*$", "0"));
        string Content = Utils.GetRequest("Content", "post", 2, @"^[^\^]{1,300}$", "内线内容限1-300字");
        int Face = int.Parse(Utils.GetRequest("Face", "post", 1, @"^[0-9]\d*$", "0"));

        //   Utils.Error("uid=" + uid + "--toid=" + toid + "", "");
        if (Face > 0 & Face < 27)
            Content = "[F]" + Face + "[/F]" + Content;

        //是否刷屏
        string appName = "LIGHT_GUEST";
        int Expir = Convert.ToInt32(ub.GetSub("GuestExpir", xmlPath));
        BCW.User.Users.IsFresh(appName, Expir);

        int ToId = toid;
        if (toid == 0)
        {
            ToId = itoid;
        }
        if (ToId == 0)
        {
            Utils.Error("收信ID错误", "");
        }
        if (!new BCW.BLL.User().Exists(ToId))
        {
            Utils.Error("不存在的收信ID", "");
        }
        if (ToId.Equals(uid))
        {
            Utils.Error("不能给自己发送内线", "");
        }

        //你是否是对方的黑名单
        if (new BCW.BLL.Friend().Exists(ToId, uid, 1))
        {
            Utils.Error("对方已把您加入黑名单", "");
        }
        //对方是否拒绝接收非好友内线
        string ForumSet = new BCW.BLL.User().GetForumSet(ToId);
        int Nofri = BCW.User.Users.GetForumSet(ForumSet, 17);
        if (Nofri == 1)
        {
            if (!new BCW.BLL.Role().IsAdmin(uid))
            {
                if (!new BCW.BLL.Friend().Exists(ToId, uid, 0))
                {
                    Utils.Error("您不在对方的好友里，对方已设置拒绝接收非好友内线", "");
                }
            }
        }
        //对方是否接收非验证用户内线
        int NoUser = BCW.User.Users.GetForumSet(ForumSet, 18);
        if (NoUser == 1 && !new BCW.BLL.Friend().Exists(ToId, uid, 0))
        {
            int IsVerify = new BCW.BLL.User().GetIsVerify(uid);
            if (IsVerify == 0)
            {
                Utils.Error("您是非验证用户，对方已设置拒绝接收非验证用户内线", "");
            }
        }

        if (transid == uid)
            transid = 0;

        string mename = new BCW.BLL.User().GetUsName(uid);
        BCW.Model.Guest model = new BCW.Model.Guest();
        model.FromId = uid;//发送方
        model.FromName = mename;
        model.ToId = ToId;//接收方
        model.ToName = new BCW.BLL.User().GetUsName(ToId);
        model.Content = Content;
        model.TransId = transid;

        new BCW.BLL.Guest().Add(model);
        //更新联系时间
        new BCW.BLL.Friend().UpdateTime(uid, ToId);

        int smsCount = new BCW.BLL.Guest().GetCount(uid);

        if (smsCount > 0)
            Utils.Success("发送内线", "发送成功，正在返回..", Utils.getUrl("guest.aspx?act=newguest&amp;backurl=" + Utils.getPage(0) + ""), "1");
        else
            // Utils.Success("发送内线", "发送成功，正在返回..", Utils.getPage("guest.aspx").Replace("/utility/book.aspxurl=", "/utility/book.aspx?url="), "1");
            Utils.Success("发送内线", "发送成功，正在返回新内线..", Utils.getUrl("guest.aspx?act=newguest&amp;backurl=" + Utils.getPage(0) + ""), "2");
    }

    //newview 小号3条内线显示
    private void ViewPageNew(int uid)
    {
        Master.Title = "查看内线";
        uid = int.Parse(Utils.GetRequest("user", "get", 1, @"^[1-9]\d*$", "0"));//52775  本地ID 将发送方
        //  Utils.Error(""+ uid + "", "");
        int hid = 0;
        int v_id = 0;
        int id = int.Parse(Utils.GetRequest("id", "get", 1, @"^[1-9]\d*$", "0"));
        if (id != 0)
        {
            if (!new BCW.BLL.Guest().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
        }
        else
        {
            v_id = 1;
            hid = int.Parse(Utils.GetRequest("hid", "get", 1, @"^[0-9]\d*$", "-1"));
            if (hid == -1)
            {
                Utils.Error("不存在的会员ID", "");
            }
            if (hid != 0 && !new BCW.BLL.User().Exists(hid))
            {
                Utils.Error("不存在的会员ID", "");
            }
            if (hid == 0)
            {
                //记录上一个足迹
                string GetPageUrl = Utils.getPage(0);
                if (!GetPageUrl.Contains("%2fguest.aspx"))
                {
                    new BCW.BLL.User().UpdateVisitHy(uid, Server.UrlDecode(GetPageUrl));
                }
            }
            DataSet ds = new BCW.BLL.Guest().GetList("TOP 1 ID", "FromId=" + hid + " and ToId=" + uid + " and TDel=0 and IsSeen=0 Order by AddTime ASC");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                id = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
            }
            else
            {
                Utils.Success("查看内线", "暂无新内线.<br /><a href=\"" + Utils.getUrl("guest.aspx?backurl=" + Utils.getPage(0) + "") + "\">返回我的内线&gt;&gt;</a>", Utils.getPage("guest.aspx"), "3");
            }
        }
        BCW.Model.Guest model = new BCW.BLL.Guest().GetGuest(id);
        bool IsMeSend = false;
        int ptype = 0;
        if (model.FromId == uid)
        {
            //if (!new BCW.BLL.Guest().ExistsFrom(id, uid))
            //{
            //    Utils.Error("不存在的记录", "");
            //}
            IsMeSend = true;
            ptype = 2;
            hid = model.ToId;
        }
        else
        {
            //if (!new BCW.BLL.Guest().ExistsTo(id, uid))
            //{
            //    Utils.Error("不存在的记录", "");
            //}
            if (model.FromId == 0)
                ptype = 1;

            hid = model.FromId;//

            //更新为已读
            if (model.IsSeen == 0)
            {
                new BCW.BLL.Guest().UpdateIsSeen(id);
            }
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?backurl=" + Utils.getPage(0) + "") + "\">内线</a>&gt;查看内线");
        if (v_id == 1)
        {
            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=viewnew&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[刷新]</a>");
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        if (model.FromId == 0)
        {
            builder.Append("来自系统内线:" + DT.FormatDate(model.AddTime, 5) + "");
        }
        else
        {
            if (!IsMeSend)
                builder.Append("发送人:<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + model.FromId + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.FromName + "(" + model.FromId + ")</a>" + DT.FormatDate(model.AddTime, 5) + "");
            else
                builder.Append("收件人:<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + model.ToId + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.ToName + "(" + model.ToId + ")</a>" + DT.FormatDate(model.AddTime, 5) + "");
        }
        builder.Append(Out.Tab("</div>", Out.Hr()));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("" + Out.SysUBB(model.Content.Replace("showGuess.aspx?gid=", "/bbs/guess2/showGuess.aspx?gid=")) + "");
        if (model.TransId > 0)
        {
            builder.Append("<br />转自ID:<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + model.TransId + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.TransId + "</a>");
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        if (!IsMeSend && model.FromId != 0)
        {
            strText = ",,,,,,,";
            strName = "fromid,toid,transid,Face,Content,act,backurl";
            strType = "hidden,hidden,hidden,select,text,hidden,hidden";
            strValu = "" + model.ToId + "'" + model.FromId + "'" + (-id) + "'0''newsave'" + Utils.getPage(0) + "";
            strEmpt = "false,false,false,0|表情|1|微笑|2|哭泣|3|调皮|4|脸红|5|害羞|6|生气|7|偷笑|8|流汗|9|我倒|10|得意|11|眦牙|12|疑问|13|睡觉|14|好色|15|口水|16|敲头|17|可爱|18|猪头|19|胜利|20|玫瑰|21|吃药|22|白酒|23|可乐|24|囧囧|25|亲亲|26|抱抱,true,false,false";
            strIdea = "";
            strOthe = "回复,guest.aspx,post,3,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("", "<br />"));

            //取TA回复你哪一条信息
            if (model.TransId < 0)
            {
                BCW.Model.Guest model2 = new BCW.BLL.Guest().GetGuest(Math.Abs(model.TransId));
                if (model2 != null)
                {
                    builder.Append(Out.Tab("<div class=\"text\">", ""));
                    builder.Append("上次对话：" + Out.SysUBB(model2.Content.Replace("showGuess.aspx?gid=", "/bbs/guess2/showGuess.aspx?gid=")) + "");
                    builder.Append(Out.Tab("</div>", "<br />"));
                }
            }
        }

        //if (model.FromId == 0)
        //{
        //    builder.Append(Out.Tab("<div>", ""));
        //    builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=keep&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">收藏</a>.");
        //    builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=trans&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">转发</a>.");
        //    builder.Append("<a href=\"" + Utils.getUrl("function.aspx?act=copyg&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">复制</a>.");
        //    builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=del&amp;id=" + id + "&amp;ptype=" + ptype + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">删除</a>");
        //    builder.Append(Out.Tab("</div>", Out.Hr()));
        //}
        //else
        //{
        //    builder.Append(Out.Tab("", Out.RHr()));
        //}

        //读取3条这对话记录
        hid = model.FromId;
        DataSet ds2 = new BCW.BLL.Guest().GetList("TOP 5 * ", "(FromId = " + uid + " and ToId = " + hid + " and FDel = 0) OR (FromId = " + hid + " and ToId = " + uid + " and TDel = 0) Order by AddTime DESC");
        if (ds2 != null && ds2.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
            {
                int nid = int.Parse(ds2.Tables[0].Rows[i]["ID"].ToString());
                int FromId = int.Parse(ds2.Tables[0].Rows[i]["FromId"].ToString());
                int TransId = int.Parse(ds2.Tables[0].Rows[i]["TransId"].ToString());
                int IsSeen = int.Parse(ds2.Tables[0].Rows[i]["IsSeen"].ToString());
                DateTime AddTime = DateTime.Parse(ds2.Tables[0].Rows[i]["AddTime"].ToString());

                string strTrans = string.Empty;
                string Content = Out.SysUBB(ds2.Tables[0].Rows[i]["Content"].ToString().Replace("showGuess.aspx?gid=", "/bbs/guess/showGuess.aspx?gid="));
                if (TransId > 0)
                {
                    strTrans = "[转自<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + TransId + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + TransId + "</a>]";
                }
                builder.Append(Out.Tab("<div>", ""));
                if (FromId == uid)
                {
                    builder.Append("" + ds2.Tables[0].Rows[i]["FromName"].ToString() + "说:" + Content + "" + strTrans + "<small>(" + DT.FormatDate(AddTime, 1) + ")</small>");
                }
                else
                {
                    if (IsSeen == 0)
                        builder.Append("[新]");
                    if (model.FromId != 0)
                    {
                        builder.Append("<a href =\"" + Utils.getUrl("uinfo.aspx?uid=" + TransId + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + ds2.Tables[0].Rows[i]["FromName"].ToString() + "</a>" + "说:" + Content + "" + strTrans + "<small>(" + DT.FormatDate(AddTime, 1) + ")</small>");
                        //  builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=viewnew&amp;user="+ FromId + "&amp;id=" + nid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">回复</a>");
                    }
                    else
                    {
                        builder.Append("系统:" + Content + "" + strTrans + "<small>(" + DT.FormatDate(AddTime, 1) + ")</small>");
                    }
                }
                builder.Append("<br />---------");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
        }
        builder.Append(Out.Tab("<div>", ""));
        if (!IsMeSend)
        {
            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=newchat&amp;hid=" + hid + "&amp;uid=" + uid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[进入聊天模式]</a>");
        }
        if (hid == 0)
        {
            builder.Append("<br /><a href=\"" + Utils.getUrl("guest.aspx?act=read&amp;ptype=1&amp;backurl=" + Utils.PostPage(1) + "") + "\">[所有标为已读]</a>");
        }

        //if (model.FromId != 0)
        //{
        //    if (!IsMeSend)
        //    {
        //        builder.Append("<br /><a href=\"" + Utils.getUrl("guest.aspx?act=add&amp;hid=" + model.FromId + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">回复</a>.");
        //        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=keep&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">收藏</a>.");
        //    }
        //    builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=trans&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">转发</a>.");
        //    builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=del&amp;id=" + id + "&amp;ptype=" + ptype + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">删除</a><br />");
        //    builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=addblack&amp;hid=" + hid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">加黑</a>.");
        //    builder.Append("<a href=\"" + Utils.getUrl("function.aspx?act=copyg&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">复制</a>.");
        //    builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=recommend2&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">举报</a>");

        //}
        builder.Append(Out.Tab("</div>", ""));


        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append(ShowBackPage(uid));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("guest.aspx").Replace("/utility/book.aspxurl=", "/utility/book.aspx?url=") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx") + "\">内线</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    private void fgPwd()
    {
       
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("guest.aspx") + "\">内线</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx") + "\">空间</a>");
        builder.Append(Out.Tab("</div>", ""));
        string info = Utils.GetRequest("info", "all", 1, "", "");
        string info1 = Utils.GetRequest("info1", "all", 1, "", "");
        string info2= Utils.GetRequest("info2", "all", 1, "", "");
        BCW.Model.numsManage manage = new BCW.BLL.numsManage().GetByUsID(meid);
        if (guestsee.Contains(meid.ToString()))//大号  如果是管理号
        {
            if (info == "")
            {
                builder.Append(Out.Tab("<div class=\"\">", "<br/>"));
                builder.Append("你的管理问题是:"+manage.Question+"");
                builder.Append(Out.Tab("</div>", ""));
                strText = "请输入"+ gestName + "管理问题答案:/,,,";
                strName = "inanswer,act,info,backurl";
                strType = "text,hidden,hidden,hidden";
                strValu = "'fgPwd'ok'" + Utils.getPage(0) + "";
                strEmpt = "false,false,false,false";
                strIdea = "/";
                strOthe = "确定,guest.aspx,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
            else
            {
                string inanswer = Utils.GetRequest("inanswer", "post", 2, @"^[\s\S]+$", "请输入答案！");
                if (inanswer == manage.answer)
                {
                    if (info1 == "")
                    {
                        strText = "请输入你的" + gestName + "管理密码:/,,,,,";
                        strName = "pwd,inanswer,act,info,info1,backurl";
                        strType = "text,hidden,hidden,hidden,hidden,hidden";
                        strValu = "'"+ inanswer + "'fgPwd'ok'ok'" + Utils.getPage(0) + "";
                        strEmpt = "false,false,false,false,false,false";
                        strIdea = "/";
                        strOthe = "确定,guest.aspx,post,1,red";
                        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                    }
                    else
                    {
                        string pwd = Utils.GetRequest("pwd", "post", 2, @"^[A-Za-z0-9]{6,20}$", "密码限6-20位,必须由字母或数字组成");
                        if (info2 == "")
                        {
                            builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
                            builder.Append("请再次确认你的信息<br/>");
                            builder.Append("" + gestName + "管理密码:" + pwd + "");
                            builder.Append(Out.Tab("</div>", ""));
                            strText = ",,,,,,";
                            strName = "pwd,inanswer,act,info,info1,info2,backurl";
                            strType = "hidden,hidden,hidden,hidden,hidden,hidden,hidden";
                            strValu = pwd + "'" + inanswer + "'fgPwd'ok'ok'ok'" + Utils.getPage(0) + "";
                            strEmpt = "false,false,false,false,false,false,false";
                            strIdea = "/";
                            strOthe = "确定,guest.aspx,post,1,red";
                            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                        }
                        else
                        {
                            new BCW.BLL.numsManage().UpdatePwd(meid,Utils.MD5Str(pwd));
                            Utils.Success("设置成功", "设置成功!....", Utils.getUrl("guest.aspx"), "2");
                        }

                        
                    }
                }
                else
                {
                    Utils.Error("问题答案错误.", "");
                }
            }
        }
        else
        {
            Utils.Error("权限不足.", "");
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("guest.aspx") + "\">内线</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx") + "\">空间</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    private void numsPwd()
    {
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("guest.aspx") + "\">内线</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx") + "\">空间</a>");
        builder.Append(Out.Tab("</div>", ""));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (guestsee.Contains(meid.ToString()))//大号  如果是管理号
        {
            if (info == "")
            {
                //输入密码
                strText = "请输入" + gestName + "管理密码:/,,,";
                strName = "pwd,act,info,backurl";
                strType = "password,hidden,hidden,hidden";
                strValu = "'numsPwd'ok'" + Utils.getPage(0) + "";
                strEmpt = "false,false,false,false";
                strIdea = "/";
                strOthe = "确定,guest.aspx,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div class=\"\">", "<br/>"));
                builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=fgPwd&amp;backurl=" + Utils.getPage(0) + "") + "\">忘记密码</a>");
                builder.Append(Out.Tab("</div>", ""));
               
            }
            else
            {
                string pwd = Utils.GetRequest("pwd", "post", 1, "", "");
                BCW.Model.numsManage manage = new BCW.BLL.numsManage().GetByUsID(meid);
                if (manage.Pwd == Utils.MD5Str(pwd))
                {
                    //更新数据库时间
                    new BCW.BLL.numsManage().UpdateTime(meid);
                    new BCW.BLL.numsManage().UpdateTime(int.Parse(guestnew));
                    //跳转
                    Utils.Success("登录成功", "登录成功!....", Utils.getUrl("guest.aspx?act=newguest"), "2");
                }
                else
                {
                    Utils.Error("密码错误，请重新输入.", "");
                }
            }
        }
        else
        {
            Utils.Error("权限不足.", "");
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("guest.aspx") + "\">内线</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx") + "\">空间</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    //小号管理登录界面
    private void NewGuest1Manage(int uid)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        if (guestopen == "0")
        {
            Utils.Error("内线管理功能关闭！","");
        }
        if (guestsee.Contains(meid.ToString()))//大号  如果是管理号
        {
            if (meid.ToString()!= guestnew)//如果是主号，不需要登录密码
            {
                if (new BCW.BLL.numsManage().ExistsByUsID(meid)) //存在数据
                {
                    // Utils.Error("new BCW.BLL.numsManage().ExistsByUsID(meid)." + new BCW.BLL.numsManage().ExistsByUsID(meid), "");
                    BCW.Model.numsManage manage = new BCW.BLL.numsManage().GetByUsID(meid);
                    DateTime now = DateTime.Now;
                    TimeSpan span = now.Subtract(manage.loginTime);
                    //Utils.Error("span.Minutes." + span.TotalMinutes, "");
                    if (span.TotalMinutes > int.Parse(guesttime))
                    {
                        //输入密码
                        numsPwd();
                    }
                    else
                    {
                        //跳转小号界面
                        new BCW.BLL.numsManage().UpdateTime(int.Parse(guestnew));
                        Utils.Success("欢迎", "欢迎来到" + gestName + "管理功能!....", Utils.getUrl("guest.aspx?act=newguest"), "2");
                    }
                }
                else  //创建密码
                {
                    createPwd();
                }
            }
            else
            {
                Utils.Success("欢迎", "欢迎来到" + gestName + "管理功能!....", Utils.getUrl("guest.aspx?act=newguest"), "2");
            }
        }
        else
        {
            Utils.Error("权限不足.", "");
        }
    }
    private void createPwd()
    {
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("guest.aspx") + "\">内线</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx") + "\">空间</a>");
        builder.Append(Out.Tab("</div>", ""));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string info = Utils.GetRequest("info", "all", 1, "", "");
        string info1 = Utils.GetRequest("info1", "all", 1, "", "");
        if (guestsee.Contains(meid.ToString()))//大号  如果是管理号
        {
            if (info == "")
            {
                strText = "请输入你的" + gestName + "管理问题:/,请输入你的" + gestName + "管理答案(用于找回" + gestName + "管理密码):/,请输入" + gestName + "管理密码:/,请再次确认管理密码:/,,,";
                strName = "question,answer,pwd,pwd1,act,info,backurl";
                strType = "text,text,text,text,hidden,hidden,hidden";
                strValu = "''''createPwd'ok'" + Utils.getPage(0) + "";
                strEmpt = "false,false,false,false,false,false,false";
                strIdea = "/";
                strOthe = "确定,guest.aspx,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
            else
            {
                string pwd = Utils.GetRequest("pwd", "post", 2, @"^[A-Za-z0-9]{6,20}$", "密码限6-20位,必须由字母或数字组成");
                string pwd1 = Utils.GetRequest("pwd1", "post", 2, @"^[A-Za-z0-9]{6,20}$", "密码限6-20位,必须由字母或数字组成");
                // string pwd1 = Utils.GetRequest("newpwd", "post", 2, @"^[^\^]{6,20}$", "请输入六到二十个字符的密码");
                string question = Utils.GetRequest("question", "post", 2, @"^[\s\S]{3,50}$", "请输入问题（3-50字以内）");
                string answer = Utils.GetRequest("answer", "post", 2, @"^[\s\S]{3,50}$", "请输入问题答案（3-50字以内）");

                if (pwd != pwd1)
                {
                    Utils.Error("输入两次的密码不相等,请再次输入.", "");
                }
                else
                {
                    if (info1 == "")
                    {

                        builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
                        builder.Append("请再次确认你的信息<br/>");
                        builder.Append("你的" + gestName + "管理问题:" + question+"<br/>");
                        builder.Append("你的" + gestName + "管理答案:" + answer + "<br/>");
                        builder.Append("" + gestName + "管理密码:" + pwd + "");
                        builder.Append(Out.Tab("</div>", ""));

                        strText = "你的" + gestName + "管理问题:/,你的" + gestName + "管理答案:/," + gestName + "管理密码:/,,,,,";
                        strName = "question,answer,pwd,pwd1,act,info,info1,backurl";
                        strType = "hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden";
                        strValu = question+"'"+ answer + "'"+ pwd + "'"+ pwd1 + "'createPwd'ok'ok'" + Utils.getPage(0) + "";
                        strEmpt = "false,false,false,false,false,false,false,false";
                        strIdea = "/";
                        strOthe = "确定,guest.aspx,post,1,red";
                        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                    }
                    else
                    {
                        BCW.Model.numsManage numsManage = new BCW.Model.numsManage();
                        numsManage.UsID = meid;
                        numsManage.Pwd = Utils.MD5Str(pwd);
                        numsManage.loginTime = DateTime.Now;
                        numsManage.Question = question;
                        numsManage.answer = answer;
                        new BCW.BLL.numsManage().Add(numsManage);
                        Utils.Success("设置密码成功", "设置密码成功!，请牢记!,<br/>你的管理问题是:"+ question + "<br/>你的答案是:"+ answer + "<br/>你的密码是" + pwd + "....3秒后自动跳转到小号管理", Utils.getUrl("guest.aspx"), "3");
                    }
                }

            }
        }
        else
        {
            Utils.Error("权限不足.", "");
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("guest.aspx") + "\">内线</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx") + "\">空间</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    //小号管理主界面
    private void NewGuestManage(int uid)
    {
        //记录上一个足迹
        //string GetPageUrl = Utils.getPage(0);
        //if (!GetPageUrl.Contains("%2fguest.aspx"))
        //{
        //    new BCW.BLL.User().UpdateVisitHy(uid, "");
        //}
        string guestsee = (ub.GetSub("guestsee", "/Controls/guestlist.xml"));
        string GameName = Convert.ToString(ub.GetSub("gestName", "/Controls/guestlist.xml"));
        int guestgroup = int.Parse(ub.GetSub("guestgroup", "/Controls/guestlist.xml"));//得到分组数
        if (!guestsee.Contains(uid.ToString()))
        {
            Utils.Error("权限不足.", "");
        }    
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int myPage = int.Parse(Utils.GetRequest("myPage", "all", 1, @"^[0-9]+$", "10"));//1层
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-3]$", "0"));//1层
        int showtype = int.Parse(Utils.GetRequest("showtype", "all", 1, @"^[0-9]$", "0"));//3层
        int count = int.Parse(Utils.GetRequest("count", "get", 1, @"^[0-9]$", "1"));//批量数
        int userID = int.Parse(Utils.GetRequest("userID", "all", 1, @"", "0"));//1层
        int gid = int.Parse(Utils.GetRequest("gid", "all", 1, @"^[0-9]+$", "1"));//分组ID默认
        string numsTeam = Utils.GetRequest("numsTeam", "all", 1, "", "");
        string MidUidLists = (ub.GetSub("MidUidLists", "/Controls/guestlist.xml"));//得到小号号码
        MidUidLists = MidUidLists.Replace("##","#");
        string[] miduid = MidUidLists.Split('#');
        string[] temp = new string [miduid.Length];

        if (gid != 0)
        {
            int length = 0;
            if (gid * guestgroup > miduid.Length)
            {
                length = miduid.Length;
            }
            else
            {
                length = gid * guestgroup;
            }
            if ((gid - 1) * guestgroup > length)
            {
                Utils.Error("不存在组数", "");
            }
            for (int i=(gid-1)* guestgroup;i< length; i++)
            {
                temp[i-((gid - 1) * guestgroup)] = miduid[i];
            }
            miduid = temp;
        }
        Master.Title = gestName;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        if (ptype == 0)
            builder.Append("内线回复|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=newguest&amp;ptype=0&amp;ve=2a&amp;backurl=" + Utils.getPage(0) + "") + "\">内线回复</a>|");

        if (ptype == 1)
            builder.Append("进入站内|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=newguest&amp;ptype=1&amp;showtype=5&amp;ve=2a&amp;backurl=" + Utils.getPage(0) + "") + "\">进入站内</a>|");

        //if (ptype == 2)
        //    builder.Append("功能设置|");
        //else
        //    builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=newguest&amp;ptype=2&amp;backurl=" + Utils.getPage(0) + "") + "\">功能设置</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?&amp;ptype=0&amp;backurl=" + Utils.getPage(0) + "") + "\">返回内线</a>");
        builder.Append(Out.Tab("</div>", ""));
        string strWhere = "select * from tb_guest where id in (Select (select top 1 b.id from tb_Guest b where b.ToId=a.ToId order by b.AddTime desc) as id2 from tb_Guest a ) and ToId in ";
        string sstrWhere = "  ";
        DataSet dsall = null;
        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        int Totalnums = 0; //得到一共多少组
        if (miduid.Length % guestgroup == 0)
        {
            Totalnums = miduid.Length / guestgroup;
        }
        else
        {
            Totalnums = miduid.Length / guestgroup+1;
        }
        string VE1 = ConfigHelper.GetConfigString("VE");
        for (int i = 1; i <= 10; i++)
        {
            if (i == gid)
            {
                builder.Append("<b>" + i + "组|</b>");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=newguest&amp;gid=" + i + "&amp;showtype=" + showtype + "&amp;ptype=" + ptype + "&amp;ve=2a&amp;backurl=" + Utils.getPage(0) + "") + "\"><b>" + i + "组</b></a>|");
            }
        }
        builder.Append("一共"+ Totalnums + "组,更多请搜索！现在是第"+gid+"组");
        //  builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=newguest&amp;gid=" + 0 + "&amp;showtype=" + showtype + "&amp;ptype=" + ptype + "&amp;ve=2a&amp;backurl=" + Utils.getPage(0) + "") + "\"><b>全部号码</b></a>");
        builder.Append(Out.Tab("</div>", ""));
        #region 内线回复
        //内线回复
        if (ptype == 0)
        {
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            if (showtype == 0)
            {
                builder.Append("<b>新内线</b>|");
                builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=newguest&amp;ptype=0&amp;gid=" + gid + "&amp;showtype=2&amp;backurl=" + Utils.getPage(0) + "") + "\">已查看|</a>");
                builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=newguest&amp;ptype=0&amp;gid=" + gid + "&amp;showtype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">已回复</a>");
            }
            else if (showtype == 2)
            {
                builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=newguest&amp;ptype=0&amp;gid=" + gid + "&amp;showtype=0&amp;backurl=" + Utils.getPage(0) + "") + "\">新内线</a>|");
                builder.Append("<b>已查看</b>");
                builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=newguest&amp;ptype=0&amp;gid=" + gid + "&amp;showtype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">|已回复</a>");

            }
            else if (showtype == 1)
            {
                builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=newguest&amp;ptype=0&amp;gid=" + gid + "&amp;showtype=0&amp;backurl=" + Utils.getPage(0) + "") + "\">新内线</a>|");
                builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=newguest&amp;ptype=0&amp;gid=" + gid + "&amp;showtype=2&amp;backurl=" + Utils.getPage(0) + "") + "\">已查看|</a>"); ;
                builder.Append("<b>已回复</b>");
            }

            builder.Append(Out.Tab("</div>", "<br />"));
            #region 新内线
            //新内线
            if (showtype == 0 || showtype == 2)//收到或者已经查看
            {
                // builder.Append(Out.Tab("<div>", ""));
                DataSet ds = null;
                if (miduid.Length < 0)
                {
                    builder.Append("未添加小号.");
                }
                else
                {
                    int hid = 0;
                    int lists = 1;
                    strWhere = "";
                    strWhere = " ( ";
                    //   Utils.Error("" + userID + "", "");
                    if (userID > 0)
                    {
                        string xiaohao = (ub.GetSub("MidUidLists", "/Controls/guestlist.xml"));//小号
                        int flag = 0;
                        string[] strnums = xiaohao.Split('#');
                        for (int i = 0; i < strnums.Length; i++) //只能用for判断，string1.contain(string)可能出错（string1太长带有换行符）
                        {
                          //  builder.Append(strnums[i] + ",");
                            string temp1 = strnums[i];
                            if (userID.ToString() == temp1 || (userID.ToString() + "\r\n") == temp1)//如果找到相等的
                            {
                                flag = 1;
                            }
                        }
                        if (flag == 0)
                        {
                            Utils.Error("该号码不是号码管理中的号码", "");
                        }
                        strWhere += "" + userID + "";

                    }
                    else
                    {
                        for (int j = 0; j < miduid.Length; j++)
                        {
                            if (miduid[j] != null)
                            {
                                strWhere += miduid[j] + ",";
                            }
                        }
                        strWhere = strWhere.Substring(0, strWhere.Length - 1);
                    }
                    #region 数据开始
                    int pageIndex;
                    int recordCount;
                    int pageSize = myPage;
                    //int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
                    //myPage = pageSize;
                    string[] pageValUrl = { "act", "backurl", "showtype", "ptype", "myPage" , "numsTeam" , "userID", "gid" };
                    pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                    if (pageIndex == 0)
                        pageIndex = 1;
                    //   Utils.Error("" + strWhere + ""+ strWhere.Length+"|"+ (strWhere.Length - 1)+"", "");
                    IList<BCW.Model.Guest> list = null;
                    if (showtype == 0)//收到或者已经查看
                    {
                        sstrWhere = "   ToId in " + strWhere + " ) and IsSeen=0 and FromId>0 ";
                        list = new BCW.BLL.Guest().GetGuestsAsc(pageIndex, pageSize, sstrWhere, out recordCount);
                    }
                    else
                    {
                        sstrWhere = "   ToId in " + strWhere + " ) and IsSeen=1 and FromId>0 ";
                        list = new BCW.BLL.Guest().GetGuests(pageIndex, pageSize, sstrWhere, out recordCount);
                    }
                    if (list.Count > 0)
                    {
                        int k = 1;
                        foreach (BCW.Model.Guest n in list)
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
                            string keys = "";
                            hid = Convert.ToInt32(n.ToId);
                            BCW.Model.User model = new BCW.BLL.User().GetKey(hid);
                            keys = BCW.User.Users.SetUserKeys(hid, model.UsPwd, model.UsKey);
                            string VE = ConfigHelper.GetConfigString("VE");
                            string SID = ConfigHelper.GetConfigString("SID");
                            string bUrl = string.Empty;
                            //builder.Append("加密后" + new BCW.BLL.Security().EncryptQueryString(n.ToId.ToString()) + "<br/>");
                            //builder.Append("解密后" + new BCW.BLL.Security().DecryptQueryString(new BCW.BLL.Security().EncryptQueryString(n.ToId.ToString())) + "<br/>");
                            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=login&amp;ptype=5&amp;showtype=0&amp;hid=" + new BCW.BLL.Security().EncryptQueryString(n.ToId.ToString()) + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + (k) + ".ID" + n.ToId + "</a>|");
                            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=login&amp;ptype=0&amp;showtype=0&amp;hid=" + new BCW.BLL.Security().EncryptQueryString(n.ToId.ToString()) + "&amp;backurl=" + Utils.getPage(0) + "") + "\">论坛</a>|");
                            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=login&amp;ptype=1&amp;showtype=0&amp;hid=" + new BCW.BLL.Security().EncryptQueryString(n.ToId.ToString()) + "&amp;backurl=" + Utils.getPage(0) + "&amp;id=13667") + "\" > 游戏</a>|");
                            if (list.Count > 0)
                            {
                                builder.Append(" 收到 " + "<a href=\"" + Utils.getUrl("guest.aspx?act=login&amp;ptype=4&amp;showtype=0&amp;hid=" + new BCW.BLL.Security().EncryptQueryString(n.ToId.ToString()) + "&amp;fromId=" + n.FromId + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + n.FromName + "(" + n.FromId + ")" + "</a>" + " 的内线:");
                                builder.Append(" <a href =\"" + Utils.getUrl("/bbs/guest.aspx?act=login&amp;Id=" + n.ID + "&amp;hid=" + new BCW.BLL.Security().EncryptQueryString(n.ToId.ToString()) + "&amp;ptype=" + 6 + "&amp;fromId=" + n.FromId) + "\">" + "点击查看" + "</a>" + " ");
                            }
                            builder.Append(" [未读" + "<font color=\"red\">" + new BCW.BLL.Guest().GetCount(Convert.ToInt32(n.ToId)) + "</font>" + "]");
                            builder.Append(DT.DateDiff2(DateTime.Now, Convert.ToDateTime(n.AddTime)) + "前");
                            k++;
                            builder.Append(Out.Tab("</div>", ""));
                        }
                        builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                        #endregion
                    }
                    else
                    {
                        builder.Append(Out.Div("div", "没有相关记录.."));
                    }
                }
                //string strText = "按ID搜索:/查询会员:,";
                //string strName = "userID,backurl";
                //string strType = "num,hidden,hidden";
                //string strValu = userID + "'" + Utils.getPage(0) + "" ;
                //string strEmpt = "true,false";
                //string strIdea = "";
                //string strOthe = "确定搜索|reset,guest.aspx?act=newguest&amp;showtype="+ showtype + "&amp;type=0&amp;,post,1,red|blue";
                //builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

                // builder.Append(Out.Tab("</div>", ""));
            }
            #endregion
            #region 已回复
            //已回复
            else
            {
                int hid = 0;
                int lists = 1;
                strWhere = "";
                strWhere = " ( ";
                if (userID > 0)
                {
                    string xiaohao = (ub.GetSub("MidUidLists", "/Controls/guestlist.xml"));//小号
                    int flag = 0;
                    string[] strnums = xiaohao.Split('#');
                    for (int i = 0; i < strnums.Length; i++) //只能用for判断，string1.contain(string)可能出错（string1太长带有换行符）
                    {
                        //  builder.Append(strnums[i] + ",");
                        string temp1 = strnums[i];
                        if (userID.ToString() == temp1 || (userID.ToString() + "\r\n") == temp1)//如果找到相等的
                        {
                            flag = 1;
                        }
                    }
                    if (flag == 0)
                    {
                        Utils.Error("该号码不是号码管理中的号码", "");
                    }
                    strWhere += "" + userID + "";
                }
                else
                {
                    for (int j = 0; j < miduid.Length; j++)
                    {
                        if (miduid[j] != null)
                        {
                            strWhere += miduid[j] + ",";
                        }
                    //    strWhere += miduid[j] + ",";
                    }
                    strWhere = strWhere.Substring(0, strWhere.Length - 1);
                }

                //     strWhere = strWhere.Substring(0, strWhere.Length - 1);
                sstrWhere = "   FromId in " + strWhere + " ) and FromId>0 ";
                //    sstrWhere = "  id in (Select (select top 1 b.id from tb_Guest b where b.FromId=a.FromId order by b.AddTime desc) as id2 from tb_Guest a ) and FromId in " + strWhere + " )  order by AddTime desc";
                // Utils.Error("" + sstrWhere + "", "");
                //dsall = new BCW.BLL.Guest().GetList(" * ", sstrWhere);
                //  Utils.Error("" + dsall.Tables[0].Rows.Count + "", "");
                //  #region 数据开始
                //分页开始
                #region 数据开始
                int pageIndex;
                int recordCount;
                int pageSize = myPage;
                //int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
                string[] pageValUrl = { "act", "backurl", "showtype", "ptype", "myPage", "numsTeam", "userID", "gid" };
                pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                if (pageIndex == 0)
                    pageIndex = 1;

                IList<BCW.Model.Guest> list = new BCW.BLL.Guest().GetGuests(pageIndex, pageSize, sstrWhere, out recordCount);
                if (list.Count > 0)
                {
                    int k = 1;
                    foreach (BCW.Model.Guest n in list)
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
                        string keys = "";
                        hid = Convert.ToInt32(n.ToId);
                        BCW.Model.User model = new BCW.BLL.User().GetKey(hid);
                        keys = BCW.User.Users.SetUserKeys(hid, model.UsPwd, model.UsKey);
                        string VE = ConfigHelper.GetConfigString("VE");
                        string SID = ConfigHelper.GetConfigString("SID");
                        string bUrl = string.Empty;
                        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=login&amp;ptype=5&amp;showtype=0&amp;hid=" + new BCW.BLL.Security().EncryptQueryString(n.FromId.ToString()) + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + (k) + ".ID" + n.FromId + "</a>|");
                        builder.Append(" 回复 " + "<a href=\"" + Utils.getUrl("guest.aspx?act=login&amp;ptype=4&amp;showtype=0&amp;hid=" + new BCW.BLL.Security().EncryptQueryString(n.FromId.ToString()) + "&amp;fromId=" + n.ToId + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + n.ToName + "(" + n.ToId + ")" + "</a>" + "");
                        builder.Append(" <a href =\"" + Utils.getUrl("/bbs/guest.aspx?act=login&amp;Id=" + n.ID + "&amp;hid=" + new BCW.BLL.Security().EncryptQueryString(n.FromId.ToString()) + "&amp;ptype=" + 6 + "&amp;fromId=" + n.FromId) + "\">" + "点击查看" + "</a>" + " ");
                        // builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=login&amp;ptype=1&amp;showtype=0&amp;hid=" + Convert.ToInt32(n.ToId) + "&amp;backurl=" + Utils.getPage(0) + "&amp;id=13667") + "\" > 游戏</a>|");
                        //if (list.Count > 0)
                        //{
                        //    builder.Append(" 收到 " + " <a href =\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.FromId + "") + "\">" + n.FromName + "(" + n.FromId + ")" + "</a>" + " 的内线:");
                        //    builder.Append(" <a href =\"" + Utils.getUrl("/bbs/guest.aspx?act=login&amp;Id=" + n.ID + "&amp;hid=" + n.ToId + "&amp;ptype=" + 6 + "&amp;fromId=" + n.FromId) + "\">" + "点击查看" + "</a>" + " ");
                        //}
                        //  builder.Append(" [未读" + "<font color=\"red\">" + new BCW.BLL.Guest().GetCount(Convert.ToInt32(n.ToId)) + "</font>" + "]");
                        builder.Append(DT.DateDiff2(DateTime.Now, Convert.ToDateTime(n.AddTime)) + "前");
                        k++;
                        builder.Append(Out.Tab("</div>", ""));
                    }
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                    #endregion
                }
                else
                {
                    builder.Append(Out.Div("div", "没有相关记录.."));
                }
            }
            #endregion
            string strText = "按ID搜索:/查询会员:&nbsp;&nbsp;&nbsp;&nbsp;,每页显示多少:,搜索第几组&nbsp;&nbsp;&nbsp;,,,,";
            string strName = "userID,myPage,gid,backurl,act,showtype,type";
            string strType = "num,num,num,hidden,hidden,hidden,hidden";
            string strValu = userID + "'" + myPage + "'"+gid+"'" + Utils.getPage(0) + "'newguest'"+ showtype + "'0";
            string strEmpt = "true,false,false,false,false,false,false";
            string strIdea = "";
            string strOthe = "确定搜索,guest.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        }
        #endregion 新内线 已回复结束

        #region 进入站内 开始
        //进入站内
        else if (ptype == 1)
        {
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            if (showtype == 5)
                builder.Append("今日登录|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=newguest&amp;ptype=1&amp;gid=" + gid + "&amp;showtype=5&amp;backurl=" + Utils.getPage(0) + "") + "\">今日登录</a>|");

            if (showtype == 6)
                builder.Append("一周登录|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=newguest&amp;ptype=1&amp;gid=" + gid + "&amp;showtype=6&amp;backurl=" + Utils.getPage(0) + "") + "\">一周登录</a>|");
            if (showtype == 7)
                builder.Append("全部号码");
            else
                builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=newguest&amp;ptype=1&amp;gid=" + gid + "&amp;showtype=7&amp;backurl=" + Utils.getPage(0) + "") + "\">全部号码</a>");
            builder.Append(Out.Tab("</div>", ""));
            if (userID == 0)
            {
                int lists = 1;
                int hid = 0;
                DataSet ds = null;
                BCW.Model.User m;// = new BCW.BLL.User().GetBasic(uid);
                sstrWhere = "";
                 #region 今天
                //今天
                if (showtype == 5)
                {
                    // sstrWhere = " Year(EndTime) = " + DateTime.Now.Year + "" + " and Month(EndTime) = " + DateTime.Now.Month + "and Day(EndTime) = " + (DateTime.Now.Day) + " and  (";
                    sstrWhere = "EndTime >= '" + DateTime.Now.ToShortDateString() + " 00:00:00 '" + " and  ( ";
                    //sstrWhere = " datediff(day,[EndTime],getdate())=0 and (";
                    //   builder.Append("sstrWhere:"+ sstrWhere+"<br/>");
                    if (userID > 0)
                    {
                        string xiaohao = (ub.GetSub("MidUidLists", "/Controls/guestlist.xml"));//小号
                        int flag = 0;
                        string[] strnums = xiaohao.Split('#');
                        for (int i = 0; i < strnums.Length; i++) //只能用for判断，string1.contain(string)可能出错（string1太长带有换行符）
                        {
                            //  builder.Append(strnums[i] + ",");
                            string temp1 = strnums[i];
                            if (userID.ToString() == temp1 || (userID.ToString() + "\r\n") == temp1)//如果找到相等的
                            {
                                flag = 1;
                            }
                        }
                        if (flag == 0)
                        {
                            Utils.Error("该号码不是号码管理中的号码", "");
                        }
                        sstrWhere += "ID = " + userID + "";
                    }
                    else
                    {
                        for (int j = 0; j < miduid.Length; j++)
                        {
                            if (miduid[j] != null)
                            {
                                sstrWhere += " ID = " + miduid[j] + " or";
                            }
                        }
                        sstrWhere = sstrWhere.Substring(0, sstrWhere.Length - 2);
                    }
                    //   Utils.Error("=" +sstrWhere + "", "");
                    sstrWhere += ")  order by EndTime desc ";

                    dsall = new BCW.BLL.User().GetList("*", sstrWhere);

                }
                #endregion
                #region 一周
                //一周
                else if (showtype == 6)
                {

                    string M_Str_mindate = string.Empty;
                    switch (DateTime.Now.DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            M_Str_mindate = DateTime.Now.AddDays(0).ToShortDateString() + "";
                            break;
                        case DayOfWeek.Tuesday:
                            M_Str_mindate = DateTime.Now.AddDays(-1).ToShortDateString() + "";
                            break;
                        case DayOfWeek.Wednesday:
                            M_Str_mindate = DateTime.Now.AddDays(-2).ToShortDateString() + "";
                            break;
                        case DayOfWeek.Thursday:
                            M_Str_mindate = DateTime.Now.AddDays(-3).ToShortDateString() + "";
                            break;
                        case DayOfWeek.Friday:
                            M_Str_mindate = DateTime.Now.AddDays(-4).ToShortDateString() + "";
                            break;
                        case DayOfWeek.Saturday:
                            M_Str_mindate = DateTime.Now.AddDays(-5).ToShortDateString() + "";
                            break;
                        case DayOfWeek.Sunday:
                            M_Str_mindate = DateTime.Now.AddDays(-6).ToShortDateString() + "";
                            break;
                    }
                    sstrWhere = " EndTime>='" + M_Str_mindate + "' and (";
                    //sstrWhere = "";
                    //sstrWhere = " datediff(week,[EndTime],getdate()-1)=0 and (";
                    if (userID > 0)
                    {
                        string xiaohao = (ub.GetSub("MidUidLists", "/Controls/guestlist.xml"));//小号
                        int flag = 0;
                        string[] strnums = xiaohao.Split('#');
                        for (int i = 0; i < strnums.Length; i++) //只能用for判断，string1.contain(string)可能出错（string1太长带有换行符）
                        {
                            //  builder.Append(strnums[i] + ",");
                            string temp1 = strnums[i];
                            if (userID.ToString() == temp1 || (userID.ToString() + "\r\n") == temp1)//如果找到相等的
                            {
                                flag = 1;
                            }
                        }
                        if (flag == 0)
                        {
                            Utils.Error("该号码不是号码管理中的号码", "");
                        }
                        sstrWhere += "ID = " + userID + "";
                    }
                    else
                    {
                        for (int j = 0; j < miduid.Length; j++)
                        {
                            if (miduid[j] != null )
                            {
                                sstrWhere += " ID = " + miduid[j] + " or";
                            }
                        }
                        sstrWhere = sstrWhere.Substring(0, sstrWhere.Length - 2);
                    }
                    //  Utils.Error("" + sstrWhere + "", "");
                    sstrWhere += ") order by EndTime desc ";
                    dsall = new BCW.BLL.User().GetList("*", sstrWhere);
                }
                #endregion

                #region 全部数据
                //全部
                else if (showtype == 7)
                {

                    sstrWhere = "  ";
                    if (userID > 0)
                    {
                        string xiaohao = (ub.GetSub("MidUidLists", "/Controls/guestlist.xml"));//小号
                        int flag = 0;
                        string[] strnums = xiaohao.Split('#');
                        for (int i = 0; i < strnums.Length; i++) //只能用for判断，string1.contain(string)可能出错（string1太长带有换行符）
                        {
                            //  builder.Append(strnums[i] + ",");
                            string temp1 = strnums[i];
                            if (userID.ToString() == temp1 || (userID.ToString() + "\r\n") == temp1)//如果找到相等的
                            {
                                flag = 1;
                            }
                        }
                        if (flag == 0)
                        {
                            Utils.Error("该号码不是号码管理中的号码", "");
                        }
                        sstrWhere += "ID = " + userID + "";
                    }
                    else
                    {
                        for (int j = 0; j < miduid.Length; j++)
                        {
                            if (miduid[j] != null)
                            {
                                sstrWhere += " ID = " + miduid[j] + " or";
                            }
                        }
                        sstrWhere = sstrWhere.Substring(0, sstrWhere.Length - 2);
                    }
                    //   Utils.Error("=" +sstrWhere + "", "");
                    sstrWhere += " order by EndTime desc ";
                    dsall = new BCW.BLL.User().GetList("*", sstrWhere);
                }
                #endregion
                //builder.Append(" miduid.Length:" + miduid.Length + "<br/>");
                //builder.Append("sstrWhere:" + sstrWhere + "<br/>");
                // Utils.Error("" + sstrWhere + "", "");
                #region 数据开始
                //分页开始
                int pageIndex;
                int recordCount = 1;
                int pageSize = myPage;
                string[] pageValUrl = { "act", "backurl", "showtype", "ptype", "myPage", "pageSize", "sstrWhere", "numsTeam", "userID","gid" };
                pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                if (pageIndex == 0)
                    pageIndex = 1;
                if (dsall != null && dsall.Tables[0].Rows.Count > 0)
                {
                    recordCount = dsall.Tables[0].Rows.Count;
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
                        if (i % 2 == 0)
                        {
                            builder.Append(Out.Tab("<div >", "<br/>"));
                        }
                        else
                        {
                            if (i == 1)
                                builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
                            else
                                builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
                        }
                        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=login&amp;ptype=5&amp;showtype=0&amp;hid=" + new BCW.BLL.Security().EncryptQueryString(dsall.Tables[0].Rows[i + koo]["ID"].ToString()) + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + (lists) + ".ID" + dsall.Tables[0].Rows[i + koo]["ID"] + " |</a>");
                        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=login&amp;ptype=0&amp;showtype=0&amp;hid=" + new BCW.BLL.Security().EncryptQueryString(dsall.Tables[0].Rows[i + koo]["ID"].ToString()) + "&amp;backurl=" + Utils.getPage(0) + "") + "\">论坛</a>|");
                        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=login&amp;ptype=1&amp;showtype=0&amp;hid=" + new BCW.BLL.Security().EncryptQueryString(dsall.Tables[0].Rows[i + koo]["ID"].ToString()) + "&amp;backurl=" + Utils.getPage(0) + "") + "\" > 游戏</a>|");
                        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=login&amp;ptype=2&amp;showtype=0&amp;hid=" + new BCW.BLL.Security().EncryptQueryString(dsall.Tables[0].Rows[i + koo]["ID"].ToString()) + "&amp;backurl=" + Utils.getPage(0) + "") + "\">闲聊</a>|");
                        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=login&amp;ptype=3&amp;showtype=0&amp;hid=" + new BCW.BLL.Security().EncryptQueryString(dsall.Tables[0].Rows[i + koo]["ID"].ToString()) + "&amp;backurl=" + Utils.getPage(0) + "") + "\">群聊</a>|");
                        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=login&amp;ptype=4&amp;showtype=0&amp;hid=" + new BCW.BLL.Security().EncryptQueryString(dsall.Tables[0].Rows[i + koo]["ID"].ToString()) + "&amp;backurl=" + Utils.getPage(0) + "") + "\">空间</a>|");
                        //  builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + uid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">预览空间</a> ");
                        builder.Append("  " + Convert.ToDateTime(dsall.Tables[0].Rows[i + koo]["EndTime"]).ToString("yyyy-MM-dd HH:mm"));//("MM-dd HH:mm"));                                                                                                     //    builder.Append("<br/>");
                        lists++;
                        if (lists == 1)
                        { builder.Append("暂无新内线."); }
                        builder.Append(Out.Tab("</div>", ""));
                    }
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                }
                #endregion
            }
            else
            {
                string xiaohao = (ub.GetSub("MidUidLists", "/Controls/guestlist.xml"));//小号
                int flag = 0;
                string[] strnums = xiaohao.Split('#');
                for (int i = 0; i < strnums.Length; i++) //只能用for判断，string1.contain(string)可能出错（string1太长带有换行符）
                {
                    //  builder.Append(strnums[i] + ",");
                    string temp1 = strnums[i];
                    if (userID.ToString() == temp1 || (userID.ToString() + "\r\n") == temp1)//如果找到相等的
                    {
                        flag = 1;
                    }
                }
                if (flag == 0)
                {
                    Utils.Error("该号码不是号码管理中的号码", "");
                }
                #region 搜索ID
                if (new BCW.BLL.User().Exists(userID))
                {
                    //for (int i = 1; i <= Totalnums; i++)
                    //{
                    //    if (i == 1)
                    //    {
                    //        MidUidLists = (ub.GetSub("MidUidLists", "/Controls/guestlist.xml"));
                    //    }
                    //    else
                    //    {
                    //        MidUidLists += "#"+(ub.GetSub("MidUidLists" + i, "/Controls/guestlist.xml"));
                    //    }
                    //}
                    if (("#" + MidUidLists + "#").Contains("#" + userID.ToString() + "#"))//搜索在小号内的ID
                    {

                        int choose = int.Parse(Utils.GetRequest("choose", "get", 1, @"^[0-1]$", "0"));//回复/查看
                        builder.Append(Out.Tab("<div class=\"\">", "<br />"));
                        BCW.Model.User user = new BCW.BLL.User().GetBasic(userID);
                        builder.Append("<b>ID:" + userID + "最近登录时间是：" + user.EndTime + "</b><br>");

                        sstrWhere = "  ";
                        if (choose == 0)
                        {
                            builder.Append("<b>已回复内线</b>|");
                            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=newguest&amp;gid=" + gid + "&amp;ptype=1&amp;choose=1&amp;info=ok&amp;userID=" + userID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">已查看内线|</a>");
                            sstrWhere = "   FromId = " + userID + " ";
                        }
                        else
                        {
                            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=newguest&amp;gid=" + gid + "&amp;ptype=1&amp;choose=0&amp;info=ok&amp;userID=" + userID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">已回复内线|</a>");
                            builder.Append("<b>已查看内线</b>|");
                            sstrWhere = "   ToId = " + userID + " and IsSeen = 1 and FromId>0 ";
                        }

                        dsall = new BCW.BLL.Guest().GetList("*", sstrWhere);
                        // Utils.Error("" + sstrWhere + "", "");
                        //   builder.Append("<br/><b>sstrWhere</b>:"+ sstrWhere);
                        int pageIndex;
                        int recordCount;
                        int pageSize = myPage;
                        //int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
                        string[] pageValUrl = { "act", "backurl", "showtype", "ptype", "myPage", "numsTeam" , "pageSize", "dsall", "userID","gid" };
                        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                        if (pageIndex == 0)
                            pageIndex = 1;
                        int lists = 1;
                        if (dsall != null && dsall.Tables[0].Rows.Count > 0)
                        {
                            recordCount = dsall.Tables[0].Rows.Count;
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
                                if (i % 2 == 0)
                                {
                                    builder.Append(Out.Tab("<div >", "<br/>"));
                                }
                                else
                                {
                                    if (i == 1)
                                        builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
                                    else
                                        builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
                                }
                                if (choose == 0)
                                {
                                    builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=login&amp;ptype=5&amp;showtype="+ showtype + "&amp;hid=" + new BCW.BLL.Security().EncryptQueryString(dsall.Tables[0].Rows[i + koo]["FromId"].ToString()) + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + (lists) + ".ID" + dsall.Tables[0].Rows[i + koo]["FromId"] + "</a>|");
                                    builder.Append(" 回复 " + "<a href=\"" + Utils.getUrl("guest.aspx?act=login&amp;ptype=4&amp;showtype=" + showtype + "&amp;hid=" + new BCW.BLL.Security().EncryptQueryString(dsall.Tables[0].Rows[i + koo]["FromId"].ToString()) + "&amp;fromId=" + dsall.Tables[0].Rows[i + koo]["ToId"] + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + dsall.Tables[0].Rows[i + koo]["ToName"] + "(" + dsall.Tables[0].Rows[i + koo]["ToId"] + ")" + "</a>" + "");
                                    builder.Append(" <a href =\"" + Utils.getUrl("/bbs/guest.aspx?act=login&amp;Id=" + dsall.Tables[0].Rows[i + koo]["ID"] + "&amp;hid=" + new BCW.BLL.Security().EncryptQueryString(dsall.Tables[0].Rows[i + koo]["FromId"].ToString()) + "&amp;ptype=" + 6 + "&amp;fromId=" + dsall.Tables[0].Rows[i + koo]["FromId"]) + "\">" + "点击查看" + "</a>" + " ");
                                    builder.Append(DT.DateDiff2(DateTime.Now, Convert.ToDateTime(dsall.Tables[0].Rows[i + koo]["AddTime"])) + "前");
                                }
                                else
                                {
                                    builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=login&amp;ptype=5&amp;showtype=0&amp;hid=" + new BCW.BLL.Security().EncryptQueryString(dsall.Tables[0].Rows[i + koo]["ToId"].ToString()) + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + (lists) + ".ID" + dsall.Tables[0].Rows[i + koo]["ToId"] + "</a>|");
                                    builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=login&amp;ptype=0&amp;showtype=0&amp;hid=" + new BCW.BLL.Security().EncryptQueryString(dsall.Tables[0].Rows[i + koo]["ToId"].ToString()) + "&amp;backurl=" + Utils.getPage(0) + "") + "\">论坛</a>|");
                                    builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=login&amp;ptype=1&amp;showtype=0&amp;hid=" + new BCW.BLL.Security().EncryptQueryString(dsall.Tables[0].Rows[i + koo]["ToId"].ToString()) + "&amp;backurl=" + Utils.getPage(0) + "&amp;id=13667") + "\" > 游戏</a>|");
                                    //  if (list.Count > 0)
                                    {
                                        builder.Append(" 收到 " + "<a href=\"" + Utils.getUrl("guest.aspx?act=login&amp;ptype=4&amp;showtype=0&amp;hid=" + new BCW.BLL.Security().EncryptQueryString(dsall.Tables[0].Rows[i + koo]["ToId"].ToString()) + "&amp;fromId=" + dsall.Tables[0].Rows[i + koo]["FromId"] + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + dsall.Tables[0].Rows[i + koo]["FromName"] + "(" + dsall.Tables[0].Rows[i + koo]["FromId"] + ")" + "</a>" + " 的内线:");
                                        //builder.Append(" 收到 " + " <a href =\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + dsall.Tables[0].Rows[i + koo]["FromId"] + "") + "\">" + dsall.Tables[0].Rows[i + koo]["FromName"] + "(" + dsall.Tables[0].Rows[i + koo]["FromId"] + ")" + "</a>" + " 的内线:");
                                        builder.Append(" <a href =\"" + Utils.getUrl("/bbs/guest.aspx?act=login&amp;Id=" + dsall.Tables[0].Rows[i + koo]["ID"] + "&amp;hid=" + new BCW.BLL.Security().EncryptQueryString(dsall.Tables[0].Rows[i + koo]["ToId"].ToString()) + "&amp;ptype=" + 6 + "&amp;fromId=" + dsall.Tables[0].Rows[i + koo]["FromId"]) + "\">" + "点击查看" + "</a>" + " ");
                                    }
                                    builder.Append(" [未读" + "<font color=\"red\">" + new BCW.BLL.Guest().GetCount(Convert.ToInt32(dsall.Tables[0].Rows[i + koo]["ToId"])) + "</font>" + "]");
                                    builder.Append(DT.DateDiff2(DateTime.Now, Convert.ToDateTime(dsall.Tables[0].Rows[i + koo]["AddTime"])) + "前");
                                }                                                                                                    //    builder.Append("<br/>");
                                lists++;
                                if (lists == 1)
                                { builder.Append("暂无新内线."); }
                                builder.Append(Out.Tab("</div>", ""));
                            }
                            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                        }
                        builder.Append(Out.Tab("</div>", ""));
                    }
                }
                else
                {
                    builder.Append(Out.Div("div", "没有相关记录.."));
                }
                #endregion

            }
            string strText = "按ID搜索:/查询会员:&nbsp;&nbsp;&nbsp;&nbsp;,每页显示多少:,,,,,搜索第几组&nbsp;&nbsp;&nbsp;";
            string strName = "userID,myPage,backurl,info,choose,showtype,gid";
            string strType = "num,num,hidden,hidden,hidden,hidden,num";
            string strValu = "";
            strValu = userID + "'" + myPage + "'" + Utils.getPage(0) + "'ok'1'"+ showtype+"'"+gid;
            string strEmpt = "true,false,false,false,false,false,false";
            string strIdea = "";
            string strOthe = "确定搜索,guest.aspx?act=newguest&amp;ptype=1&amp;type=0,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            #endregion
        }

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append(ShowBackPage(uid));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx") + "\">空间</a>");
        builder.Append(Out.Tab("</div>", ""));

    }


    private void ReloadPage(int uid)
    {
        //记录上一个足迹
        //string GetPageUrl = Utils.getPage(0);
        //if (!GetPageUrl.Contains("%2fguest.aspx"))
        //{
        //    new BCW.BLL.User().UpdateVisitHy(uid, "");
        //}

        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-3]$", "0"));
        int showtype = int.Parse(Utils.GetRequest("showtype", "get", 1, @"^[0-1]$", "0"));
        Master.Title = "我的内线";
        builder.Append(Out.Tab("<div class=\"title\">", ""));

        if (ptype == 0)
            builder.Append("我的内线|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?ptype=0&amp;backurl=" + Utils.getPage(0) + "") + "\">我的内线</a>|");

        if (ptype == 1)
            builder.Append("系统内线|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">系统内线</a>|");

        builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?backurl=" + Utils.PostPage(1) + "") + "\">好友</a>");


        if (guestsee.Contains(uid.ToString()))//大号
        {
            if (ptype == 4)
                builder.Append(gestName + "|");
            else
                builder.Append("|<a href=\"" + Utils.getUrl("guest.aspx?act=newguest1&amp;ptype=4&amp;backurl=" + Utils.getPage(0) + "") + "\">" + gestName + "</a>");
        }
        

        if (("#" + MidUidLists + "#").Contains("#" + uid.ToString() + "#")|| ("#" + MidUidLists + "#").Contains("#" + uid.ToString() + "\r\n#"))//小号
        {
            if (ptype == 4)
                builder.Append(gestName + "|");
            else
                builder.Append("|<a href=\"" + Utils.getUrl("guest.aspx?act=inuser&amp;ptype=4&amp;backurl=" + Utils.getPage(0) + "") + "\">" + "返回号码" + "</a>");
        }
        if (showtype == 0)
            builder.Append("<br/>收信|");
        else
            builder.Append("<br/><a href=\"" + Utils.getUrl("guest.aspx?ptype=" + ((ptype == 2) ? 0 : ptype) + "&amp;backurl=" + Utils.getPage(0) + "") + "\">收信</a>|");

        if (ptype != 1)
        {
            if (showtype == 1)
                builder.Append("已发|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?ptype=2&amp;showtype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">已发</a>|");
        }
        if (ptype == 3)
            builder.Append("收藏|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?ptype=3&amp;backurl=" + Utils.getPage(0) + "") + "\">收藏</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=add&amp;backurl=" + Utils.getPage(0) + "") + "\">发新内线</a>");
        //if (guestsee.Contains(uid.ToString()))//大号
        //{
        //    if (ptype == 4)
        //        builder.Append(gestName + "|");
        //    else
        //        builder.Append("|<a href=\"" + Utils.getUrl("guest.aspx?act=newguest&amp;ptype=4&amp;backurl=" + Utils.getPage(0) + "") + "\">" + gestName + "</a>");
        //}
        //if (("#" + MidUidLists + "#").Contains("#" + uid.ToString() + "#"))//小号
        //{
        //    if (ptype == 4)
        //        builder.Append(gestName + "|");
        //    else
        //        builder.Append("|<a href=\"" + Utils.getUrl("guest.aspx?act=inuser&amp;ptype=4&amp;backurl=" + Utils.getPage(0) + "") + "\">" + "返回号码" + "</a>");
        //}
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = 6;
        string strWhere = "";
        string[] pageValUrl = { "act", "ptype", "showtype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        if (ptype == 0)//我的内线
            strWhere = "ToId=" + uid + " and FromId>0 and TDel=0 and IsKeep=0";
        else if (ptype == 1)//
            strWhere = "ToId=" + uid + " and FromId=0";
        else if (ptype == 2)//
            strWhere = "FromId=" + uid + " and FDel=0";
        else
            strWhere = "ToId=" + uid + " and TDel=0 and IsKeep=1";

        // 开始读取列表
        IList<BCW.Model.Guest> listGuest = new BCW.BLL.Guest().GetGuests(pageIndex, pageSize, strWhere, out recordCount);
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
                if (ptype != 1)
                {
                    if (n.Content.Length > 20)
                        sText = Utils.Left(n.Content, 20) + "..";
                }
                //sText = Regex.Replace(sText, @"^[\s\S]*?\[URL=[\s\S]+?$", "查看详细..", RegexOptions.IgnoreCase);
                //sText = Regex.Replace(sText, @"^[\s\S]*?\[IMG][\s\S]+?$", "查看详细..", RegexOptions.IgnoreCase);
                builder.AppendFormat("<a href=\"" + Utils.getUrl("guest.aspx?act=view&amp;id={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}.{3}</a>", n.ID, pageIndex, (pageIndex - 1) * pageSize + k, Out.SysUBB(sText));

                if (ptype == 0 || ptype == 3)
                {
                    if (n.IsSeen == 0)
                        builder.Append("[新]");

                    builder.AppendFormat("<br />发送人:{0}", n.FromName);
                }
                else if (ptype == 1)
                {
                    builder.Append("<br />发送人:系统");
                }
                else
                {
                    if (n.IsSeen == 1)
                        builder.Append("[已读]");

                    builder.AppendFormat("<br />收件人:{0}", n.ToName);
                }
                try
                {
                    if (ptype == 1 && new BCW.BLL.Guest().GetTypes(n.ID) == 5)
                    {
                        if (uid == 727)//测试用
                        {
                            builder.Append("<br/><font color=\"#BDBDBD\">如果你已阅读和熟知内容,<br/>请点击</font><a style=\"text-decoration:none\" href=\"" + Utils.getUrl("guest.aspx?act=sendok&amp;ID=" + n.ID + "&amp;ptype=" + ptype + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"><font color=\"green\">我已阅读本消息</font></a>");
                        }
                    }
                }
                catch { }
                builder.AppendFormat("<br />时间:{0}", DT.FormatDate(n.AddTime, 1));
                builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=del&amp;id=" + n.ID + "&amp;ptype=" + ptype + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">删除</a>");
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

        builder.Append(Out.Tab("<div>", "<br />"));
        //builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=delpage&amp;ptype=" + ptype + "&amp;page=" + pageIndex + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[删除本页内线]</a><br />");

        if (ptype == 0 || ptype == 1)
        {
            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=delptype&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[删除已读内线]</a><br />");
            if (ptype == 0)
            {
                builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=read&amp;ptype=0&amp;backurl=" + Utils.PostPage(1) + "") + "\">[所有标为已读]</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=dels&amp;backurl=" + Utils.getPage(0) + "") + "\">[清空我的内线]</a><br />");
            }
            if (ptype == 1)
            {
                builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=read&amp;ptype=1&amp;backurl=" + Utils.PostPage(1) + "") + "\">[所有标为已读]</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=delx&amp;backurl=" + Utils.getPage(0) + "") + "\">[清空系统内线]</a><br />");
            }
        }
        else if (ptype == 2)
        {
            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=delptype&amp;ptype=2&amp;backurl=" + Utils.getPage(0) + "") + "\">[删除已发内线]</a><br />");
        }
        else if (ptype == 3)
        {
            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=delptype&amp;ptype=3&amp;backurl=" + Utils.getPage(0) + "") + "\">[删除收藏内线]</a><br />");
        }
        builder.Append(Out.Tab("</div>", Out.RHr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=once&amp;backurl=" + Utils.PostPage(1) + "") + "\">最近联系人</a>.");

        builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?act=guest&amp;backurl=" + Utils.PostPage(1) + "") + "\">设置</a><br />");

        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=addall&amp;backurl=" + Utils.getPage(0) + "") + "\">群发内线</a>.");

        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=smsmail&amp;backurl=" + Utils.getPage(0) + "") + "\">短信提醒</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append(ShowBackPage(uid));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx") + "\">空间</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void NewListPage(int uid)
    {
        //记录上一个足迹
        string GetPageUrl = Utils.getPage(0);
        if (!GetPageUrl.Contains("%2fguest.aspx"))
        {
            new BCW.BLL.User().UpdateVisitHy(uid, Server.UrlDecode(GetPageUrl));
        }
        Master.Title = "我的新内线";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?ptype=0&amp;backurl=" + Utils.getPage(0) + "") + "\">我的内线</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">系统内线</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?backurl=" + Utils.PostPage(1) + "") + "\">好友</a>");
        if (guestsee.Contains(uid.ToString()))//大号
        {
            builder.Append("|<a href=\"" + Utils.getUrl("guest.aspx?act=newguest1&amp;ptype=4&amp;backurl=" + Utils.getPage(0) + "") + "\">" + gestName + "</a>");
        }
 
        if (("#" + MidUidLists + "#").Contains("#" + uid.ToString() + "#") || ("#" + MidUidLists + "#").Contains("#" + uid.ToString() + "\r\n#"))//小号
        {
            builder.Append("|<a href=\"" + Utils.getUrl("guest.aspx?act=inuser&amp;ptype=4&amp;backurl=" + Utils.getPage(0) + "") + "\">" + "返回号码" + "</a>");
        }
        builder.Append("<br/><a href=\"" + Utils.getUrl("guest.aspx?ptype=0&amp;backurl=" + Utils.getPage(0) + "") + "\">收信</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?ptype=2&amp;showtype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">已发</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?ptype=3&amp;backurl=" + Utils.getPage(0) + "") + "\">收藏</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=add&amp;backurl=" + Utils.getPage(0) + "") + "\">发新内线</a>");
        builder.Append(Out.Tab("</div>", Out.LHr()));
        DataSet ds = new BCW.BLL.Guest().GetList("FromId, COUNT(FromId) as iCount", "ToID=" + uid + " and FromId>0 and IsSeen=0 and Tdel=0 GROUP BY FromId ORDER BY COUNT(FromId) DESC");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            builder.Append(Out.Tab("<div>", ""));
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                int FromId = int.Parse(ds.Tables[0].Rows[i]["FromId"].ToString());
                int iCount = int.Parse(ds.Tables[0].Rows[i]["iCount"].ToString());
                builder.Append("<br /><a href=\"" + Utils.getUrl("guest.aspx?act=view&amp;hid=" + FromId + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + new BCW.BLL.User().GetUsName(FromId) + "(" + FromId + ")(" + iCount + ")</a>");
            }
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=read&amp;ptype=0&amp;backurl=" + Utils.PostPage(1) + "") + "\">[所有标为已读]</a>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append(ShowBackPage(uid));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx") + "\">内线</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private string ShowBackPage(int uid)
    {
        string BackUrl = new BCW.BLL.User().GetVisitHy(uid);
        if (BackUrl != "")
        {
            return "<a href=\"" + Utils.getUrl(Out.UBB(BackUrl)) + "\">[返回来源页面]</a><br />";
        }
        else
        {
            return "";
        }
    }
    private void AddPage(int uid)
    {
        Master.Title = "发送内线";
        int hid = int.Parse(Utils.GetRequest("hid", "all", 1, @"^[1-9]\d*$", "0"));
        int copy = int.Parse(Utils.GetRequest("copy", "all", 1, @"^[0-1]$", "0"));
        int ff = int.Parse(Utils.GetRequest("ff", "all", 1, @"^[0-9]\d*$", "-1"));
        int dd = int.Parse(Utils.GetRequest("dd", "all", 1, @"^[0-9]\d*$", "0"));
        //复制内容
        string Copytemp = string.Empty;
        if (ff >= 0)
            Copytemp += "[F]" + ff + "[/F]";

        if (dd > 0)
            Copytemp += new BCW.BLL.Submit().GetContent(dd, uid);

        if (copy == 1)
            Copytemp += new BCW.BLL.User().GetCopytemp(uid);

        builder.Append(Out.Tab("<div class=\"title\">发送内线</div>", ""));
        if (hid == 0)
        {
            //得到最近联系人
            string strFriend = string.Empty;
            DataSet ds = new BCW.BLL.Friend().GetList("Top 10 FriendID,FriendName", "UsID=" + uid + " and Types=0 and AddTime> '" + DateTime.Now.AddDays(-2) + "'");//限两天内为最近联系人
            if (ds != null && ds.Tables[0].Rows.Count != 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    strFriend += "|" + ds.Tables[0].Rows[i]["FriendID"] + "|" + Out.UBB(ds.Tables[0].Rows[i]["FriendName"].ToString().Replace("|", "")) + "";
                }
            }
            strFriend = "0|最近联系人" + strFriend;

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("请输入收信ID：");
            builder.Append(Out.Tab("</div>", ""));

            strText = ",或选择联系人：/,内容：(300字内)/,,,";
            strName = "toid,itoid,Content,act,backurl";
            strType = "snum,select,textarea,hidden,hidden";
            strValu = "'0'" + Copytemp + "'save'" + Utils.getPage(0) + "";
            strEmpt = "false," + strFriend + ",false,false,false";
            strIdea = "<a href=\"" + Utils.getUrl("friend.aspx?act=online&amp;backurl=" + Utils.PostPage(1) + "") + "\">从好友选择<／a>'''''|/";
        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("发给:<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + hid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(hid) + "(" + hid + ")</a>");
            builder.Append(Out.Tab("</div>", ""));

            strText = "内容：/,,,,";
            strName = "Content,toid,act,backurl";
            strType = "textarea,hidden,hidden,hidden";
            strValu = "" + Copytemp + "'" + hid + "'save'" + Utils.getPage(0) + "";
            strEmpt = "false,false,false,false";
            strIdea = "/";
        }
        strText += "表情";
        strName += ",Face";
        strType += ",select";
        strValu += "'0";
        strEmpt += ",0|表情|1|微笑|2|哭泣|3|调皮|4|脸红|5|害羞|6|生气|7|偷笑|8|流汗|9|我倒|10|得意|11|眦牙|12|疑问|13|睡觉|14|好色|15|口水|16|敲头|17|可爱|18|猪头|19|胜利|20|玫瑰|21|吃药|22|白酒|23|可乐|24|囧囧|25|亲亲|26|抱抱";

        strOthe = "&gt;确定发送,guest.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("插入:<a href=\"" + Utils.getUrl("function.aspx?act=admsub&amp;backurl=" + Utils.PostPage(1) + "") + "\">常用短语</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("function.aspx?act=face&amp;backurl=" + Utils.PostPage(1) + "") + "\">表情</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=add&amp;hid=" + hid + "&amp;ff=" + ff + "&amp;dd=" + dd + "&amp;copy=1&amp;backurl=" + Utils.getPage(0) + "") + "\">[粘贴]</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append(ShowBackPage(uid));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?backurl=" + Utils.getPage(0) + "") + "\">内线</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void SavePage(int uid)
    {
        BCW.User.Users.ShowVerifyRole("o", uid);//非验证会员提示
        new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Guest, uid);//会员本身权限
        int toid = int.Parse(Utils.GetRequest("toid", "post", 1, @"^[1-9]\d*$", "0"));
        int itoid = int.Parse(Utils.GetRequest("itoid", "post", 1, @"^[1-9]\d*$", "0"));
        int transid = int.Parse(Utils.GetRequest("transid", "post", 1, @"^(-)*[0-9]\d*$", "0"));
        string Content = Utils.GetRequest("Content", "post", 2, @"^[^\^]{1,300}$", "内线内容限1-300字");
        int Face = int.Parse(Utils.GetRequest("Face", "post", 1, @"^[0-9]\d*$", "0"));
        if (Face > 0 & Face < 27)
            Content = "[F]" + Face + "[/F]" + Content;

        //是否刷屏
        string appName = "LIGHT_GUEST";
        int Expir = Convert.ToInt32(ub.GetSub("GuestExpir", xmlPath));
        BCW.User.Users.IsFresh(appName, Expir);

        int ToId = toid;
        if (toid == 0)
        {
            ToId = itoid;
        }
        if (ToId == 0)
        {
            Utils.Error("收信ID错误", "");
        }
        if (!new BCW.BLL.User().Exists(ToId))
        {
            Utils.Error("不存在的收信ID", "");
        }
        if (ToId.Equals(uid))
        {
            Utils.Error("不能给自己发送内线", "");
        }

        //你是否是对方的黑名单
        if (new BCW.BLL.Friend().Exists(ToId, uid, 1))
        {
            Utils.Error("对方已把您加入黑名单", "");
        }
        //对方是否拒绝接收非好友内线
        string ForumSet = new BCW.BLL.User().GetForumSet(ToId);
        int Nofri = BCW.User.Users.GetForumSet(ForumSet, 17);
        if (Nofri == 1)
        {
            if (!new BCW.BLL.Role().IsAdmin(uid))
            {
                if (!new BCW.BLL.Friend().Exists(ToId, uid, 0))
                {
                    Utils.Error("您不在对方的好友里，对方已设置拒绝接收非好友内线", "");
                }
            }
        }
        //对方是否接收非验证用户内线
        int NoUser = BCW.User.Users.GetForumSet(ForumSet, 18);
        if (NoUser == 1 && !new BCW.BLL.Friend().Exists(ToId, uid, 0))
        {
            int IsVerify = new BCW.BLL.User().GetIsVerify(uid);
            if (IsVerify == 0)
            {
                Utils.Error("您是非验证用户，对方已设置拒绝接收非验证用户内线", "");
            }
        }

        if (transid == uid)
            transid = 0;

        string mename = new BCW.BLL.User().GetUsName(uid);
        BCW.Model.Guest model = new BCW.Model.Guest();
        model.FromId = uid;
        model.FromName = mename;
        model.ToId = ToId;
        model.ToName = new BCW.BLL.User().GetUsName(ToId);
        model.Content = Content;
        model.TransId = transid;
        new BCW.BLL.Guest().Add(model);
        //更新联系时间
        new BCW.BLL.Friend().UpdateTime(uid, ToId);

        int smsCount = new BCW.BLL.Guest().GetCount(uid);

        if (smsCount > 0)
            Utils.Success("发送内线", "发送成功，正在返回..", Utils.getUrl("guest.aspx?act=newlist&amp;backurl=" + Utils.getPage(0) + ""), "1");
        else
            Utils.Success("发送内线", "发送成功，正在返回..", Utils.getPage("guest.aspx").Replace("/utility/book.aspxurl=", "/utility/book.aspx?url="), "1");
    }

    private void AddallPage(int uid)
    {
        Master.Title = "群发内线";
        if (ub.GetSub("GuestAdd", xmlPath) == "1")
        {
            Utils.Error("群发功能已暂停", "");
        }
        int copy = int.Parse(Utils.GetRequest("copy", "get", 1, @"^[0-1]$", "0"));
        int ff = int.Parse(Utils.GetRequest("ff", "get", 1, @"^[0-9]\d*$", "-1"));
        int dd = int.Parse(Utils.GetRequest("dd", "get", 1, @"^[0-9]\d*$", "0"));
        //复制内容
        string Copytemp = string.Empty;
        if (ff >= 0)
            Copytemp += "[F]" + ff + "[/F]";

        if (dd > 0)
            Copytemp += new BCW.BLL.Submit().GetContent(dd, uid);

        if (copy == 1)
            Copytemp += new BCW.BLL.User().GetCopytemp(uid);

        builder.Append(Out.Tab("<div class=\"title\">群发内线</div>", ""));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("请输入群发内容:");
        builder.Append(Out.Tab("</div>", ""));

        strText = "内容：/,,,";
        strName = "Content,act,backurl";
        strType = "textarea,hidden,hidden";
        strValu = "" + Copytemp + "'saveall'" + Utils.getPage(0) + "";
        strEmpt = "false,false,false";
        strText += "表情：/";
        strName += ",Face";
        strType += ",select";
        strValu += "'0";
        strEmpt += ",0|表情|1|微笑|2|哭泣|3|调皮|4|脸红|5|害羞|6|生气|7|偷笑|8|流汗|9|我倒|10|得意|11|眦牙|12|疑问|13|睡觉|14|好色|15|口水|16|敲头|17|可爱|18|猪头|19|胜利|20|玫瑰|21|吃药|22|白酒|23|可乐|24|囧囧|25|亲亲|26|抱抱";
        strIdea = "/";
        strOthe = "[群发给好友],guest.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("温馨提示:群发收取" + ub.GetSub("GuestAll", xmlPath) + "" + ub.Get("SiteBz") + "/人<br />");
        builder.Append("插入:<a href=\"" + Utils.getUrl("function.aspx?act=admsub&amp;backurl=" + Utils.PostPage(1) + "") + "\">常用短语</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("function.aspx?act=face&amp;backurl=" + Utils.PostPage(1) + "") + "\">表情</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=addall&amp;ff=" + ff + "&amp;dd=" + dd + "&amp;copy=1&amp;backurl=" + Utils.getPage(0) + "") + "\">[粘贴]</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append(ShowBackPage(uid));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?backurl=" + Utils.getPage(0) + "") + "\">内线</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void SaveallPage(int uid)
    {
        Master.Title = "群发此内线";
        if (ub.GetSub("GuestAdd", xmlPath) == "1")
        {
            Utils.Error("群发功能已暂停", "");
        }
        BCW.User.Users.ShowVerifyRole("o", uid);//非验证会员提示
        new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Guest, uid);//会员本身权限
        string info = Utils.GetRequest("info", "post", 1, "", "");
        string Content = Utils.GetRequest("Content", "post", 2, @"^[^\^]{1,300}$", "内线内容限1-300字");
        int Face = int.Parse(Utils.GetRequest("Face", "post", 1, @"^[0-9]\d*$", "0"));
        if (Face > 0 & Face < 27)
            Content = "[F]" + Face + "[/F]" + Content;

        if (info == "")
        {
            builder.Append(Out.Tab("<div class=\"title\">群发此内线</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("请选择群发方式：");
            builder.Append(Out.Tab("</div>", ""));

            strText = ",,,,";
            strName = "Content,act,info,backurl";
            strType = "hidden,hidden,hidden,hidden";
            strValu = "" + Content + "'saveall'ok1'" + Utils.getPage(0) + "";
            strEmpt = "false,false,false,false";
            strIdea = "/";
            strOthe = "群发给在线好友,guest.aspx,post,1,blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("或选择好友组：");
            builder.Append(Out.Tab("</div>", ""));

            string strFrigroup = string.Empty;
            DataSet ds = new BCW.BLL.Frigroup().GetList("ID,Title", "Types=0 and UsID=" + uid + "");
            if (ds != null && ds.Tables[0].Rows.Count != 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    strFrigroup += "|" + ds.Tables[0].Rows[i]["ID"] + "|" + ds.Tables[0].Rows[i]["Title"] + "";
                }
            }
            strFrigroup = "0|默认分组" + strFrigroup;
            strText = ",,,,,";
            strName = "NodeId,Content,act,info,backurl";
            strType = "select,hidden,hidden,hidden,hidden";
            strValu = "0'" + Content + "'saveall'ok2'" + Utils.getPage(0) + "";
            strEmpt = "" + strFrigroup + ",false,false,false,false";
            strIdea = "/";
            strOthe = "[群发给此组好友],guest.aspx,post,1,blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示:群发收取" + ub.GetSub("GuestAll", xmlPath) + "" + ub.Get("SiteBz") + "/人");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append(ShowBackPage(uid));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?backurl=" + Utils.getPage(0) + "") + "\">内线</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            DataSet ds = null;
            if (info == "ok1")
            {
                ds = new BCW.BLL.Friend().GetList("FriendID,FriendName,UsName", "UsID=" + uid + " and Types=0 and (Select State from tb_User where id = FriendId)<>1 and (Select EndTime from tb_User where id = FriendId)>'" + DateTime.Now.AddMinutes(-et) + "'");
            }
            else
            {
                int NodeId = int.Parse(Utils.GetRequest("NodeId", "post", 2, @"^[0-9]\d*$", "选择好友组错误"));
                if (NodeId != 0)
                {
                    if (!new BCW.BLL.Frigroup().Exists(NodeId, uid, 0))
                    {
                        Utils.Error("不存在的分组", "");
                    }
                }
                ds = new BCW.BLL.Friend().GetList("FriendID,FriendName,UsName", "UsID=" + uid + " and Types=0 and NodeId=" + NodeId + "");
            }
            string UsName = new BCW.BLL.User().GetUsName(uid);

            int k = 0;
            if (ds != null && ds.Tables[0].Rows.Count != 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    int FriendID = int.Parse(ds.Tables[0].Rows[i]["FriendID"].ToString());
                    //你是否是对方的黑名单
                    bool Isblack = false;
                    if (new BCW.BLL.Friend().Exists(FriendID, uid, 1))
                    {
                        Isblack = true;
                    }
                    //对方是否拒绝接收好友群发内线
                    string ForumSet = new BCW.BLL.User().GetForumSet(FriendID);
                    int Nofri = BCW.User.Users.GetForumSet(ForumSet, 12);
                    if (Nofri == 0 && Isblack == false)
                    {
                        string FriendName = ds.Tables[0].Rows[i]["FriendName"].ToString();
                        BCW.Model.Guest model = new BCW.Model.Guest();
                        model.FromId = uid;
                        model.FromName = UsName;
                        model.ToId = FriendID;
                        model.ToName = FriendName;
                        model.Content = Content;
                        model.TransId = 0;
                        new BCW.BLL.Guest().Add(model);
                        //更新联系时间
                        new BCW.BLL.Friend().UpdateTime(uid, FriendID);
                    }
                    k++;
                }
                //收费
                long payCent = Convert.ToInt64(int.Parse(ub.GetSub("GuestAll", xmlPath)) * k);
                if (payCent > 0)
                {
                    new BCW.BLL.User().UpdateiGold(uid, -payCent, "群发内线");
                }
            }
            Utils.Success("群发内线", "群发" + k + "位好友成功，正在返回..", Utils.getUrl("guest.aspx?backurl=" + Utils.getPage(0) + ""), "1");
        }

    }

    private void RecomPage(int uid)
    {
        string ac = Utils.ToSChinese(Utils.GetRequest("ac", "all", 1, "", ""));
        string hold = Utils.ToSChinese(Utils.GetRequest("hold", "all", 1, "", "1"));
        string delete = Utils.ToSChinese(Utils.GetRequest("delete", "all", 1, "", "1"));
        String toidnum = Utils.GetRequest("toidnum", "post", 1, @"^[1-9]\d*$", "0");
        int deleteid = int.Parse(Utils.GetRequest("deleteid", "all", 1, "", "1"));
        String VE;
        String SID;
        if (ac.Contains("分享"))
            ac = "分享";
        else
            ac = "举报";
        if (delete == "删" || delete == "取消")
        {

            BCW.HB.Model.Shared shaxp3 = new BCW.HB.Model.Shared();
            BCW.HB.Model.Shared shadu = new BCW.HB.BLL.Shared().GetModel(uid);
            shaxp3.SharedIDList = "";
            string[] delelist = shadu.SharedIDList.Split(',');
            for (int j = 0; j < delelist.Length; j++)
            {
                if (delelist[j] != deleteid.ToString())
                {
                    if (j != 0)
                    {
                        shaxp3.SharedIDList = shaxp3.SharedIDList + "," + delelist[j];
                    }
                    else
                    {
                        shaxp3.SharedIDList = delelist[j];
                    }
                }
            }
            shaxp3.UserID = uid;
            shaxp3.ShareUrl = shadu.ShareUrl;
            shaxp3.ShareContent = shadu.ShareContent;
            new BCW.HB.BLL.Shared().Update(shaxp3);
        }
        string getPage = Utils.getPage(1);
        if (getPage == "")
        {
            BCW.HB.Model.Shared getpa = new BCW.HB.BLL.Shared().GetModel(uid);
            getPage = getpa.ShareContent;
        }
        else
        {
            bool exitse5 = new BCW.HB.BLL.Shared().Exists(uid);
            BCW.HB.Model.Shared shaxp = new BCW.HB.Model.Shared();
            if (exitse5)
            {
                shaxp.UserID = uid;
                shaxp.SharedIDList = "";
                shaxp.ShareUrl = "";
                shaxp.ShareContent = getPage;
                new BCW.HB.BLL.Shared().Update(shaxp);
            }
            else
            {
                shaxp.UserID = uid;
                shaxp.SharedIDList = "";
                shaxp.ShareUrl = "";
                shaxp.ShareContent = getPage;
                new BCW.HB.BLL.Shared().Add(shaxp);
            }
        }
        string purl = Utils.GetRequest("purl", "post", 1, "", "");
        if (purl == "")
        {
            string Purl = Out.UBB(Utils.removeUVe(getPage));
            string Purls = "http://" + Utils.GetDomain() + "" + Purl + "";
            string Title = Utils.GetSourceTextByUrl(Utils.getUrl(Purls).Replace("&amp;", "&"));
            Title = Utils.GetTitle(Title);
            purl = "[url=" + Purl + "]" + Title + "[/url]";
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        Master.Title = ac;
        builder.Append("" + ac + ":" + Out.SysUBB(purl));
        builder.Append(Out.Tab("</div>", "<br />"));
        if (ac == "分享")
        {
            #region 分享
            int ptt = int.Parse(Utils.GetRequest("ptt", "all", 1, @"^[0-9]\d*$", "0"));
            builder.Append(Out.Tab("<div class=\"class\">", "<br/>"));
            if (ptt == 0)
            {
                builder.Append("在线好友|");
                builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=recommend&amp;ptt=1&amp;ac=分享") + "\">全部好友</a>");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=recommend&amp;ptt=0&amp;ac=分享") + "\">在线好友</a>");
                builder.Append("|全部好友");
            }
            builder.Append(Out.Tab("</div>", "<br />"));
            int pageIn = int.Parse(Utils.GetRequest("pageIn", "all", 1, @"^[0-9]\d*$", "0"));
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string strWhere = "";
            string[] pageValUrl = { "ptype", "act", "ab", "ac", "ptt", "getPage", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            if (delete == "删" || delete == "取消")
            {
                pageIndex = pageIn;
            }
            if (hold == "选择")
            {
                int exnum = int.Parse(toidnum);
                bool exo = new BCW.BLL.User().Exists(exnum);
                if (exo == false)
                {
                    Utils.Error("不存在的ID或输入无效！", "");
                }
                pageIndex = pageIn;
                bool exitse = new BCW.HB.BLL.Shared().Exists(uid);
                BCW.HB.Model.Shared sha = new BCW.HB.Model.Shared();
                if (exitse)
                {
                    BCW.HB.Model.Shared shaex = new BCW.HB.BLL.Shared().GetModel(uid);
                    sha.UserID = uid;
                    if (shaex.SharedIDList.Trim() != "")
                    {
                        #region 去重
                        sha.SharedIDList = shaex.SharedIDList + "," + toidnum;
                        string[] stringArray = sha.SharedIDList.Split(',');
                        List<string> listString = new List<string>();
                        foreach (string eachString in stringArray)
                        {
                            if (!listString.Contains(eachString))
                                listString.Add(eachString);
                        }
                        string SharedIDList2 = "";
                        foreach (string eachString in listString)
                        {
                            SharedIDList2 = SharedIDList2 + "," + eachString;
                        }
                        string[] SharedIDList3 = SharedIDList2.Split(',');
                        string SharedIDList4 = "";
                        for (int i = 0; i < SharedIDList3.Length; i++)
                        {
                            if (SharedIDList3[i].Trim() != "")
                            {
                                if (i != 1)
                                {
                                    SharedIDList4 = SharedIDList4 + "," + SharedIDList3[i];
                                }
                                else
                                {
                                    SharedIDList4 = SharedIDList3[i];
                                }
                            }
                        }
                        #endregion
                        sha.SharedIDList = SharedIDList4;
                    }
                    else
                    {
                        sha.SharedIDList = toidnum;
                    }
                    sha.ShareUrl = purl;
                    sha.ShareContent = getPage;
                    if (toidnum.Trim() != "")
                    {
                        new BCW.HB.BLL.Shared().Update(sha);
                    }
                }
                else
                {
                    sha.UserID = uid;
                    sha.SharedIDList = toidnum;
                    sha.ShareUrl = purl;
                    sha.ShareContent = getPage;

                    new BCW.HB.BLL.Shared().Add(sha);
                }
            }
            //查询条件
            if (ptt == 0)
                strWhere = "UsID=" + uid + " and Types=0 and (Select State from tb_User where id = FriendId)<>1 and (Select EndTime from tb_User where id = FriendId)>'" + DateTime.Now.AddMinutes(-et) + "'";
            else
                strWhere = "UsID=" + uid + " and Types=0";
            // 开始读取列表
            IList<BCW.Model.Friend> listFriend = new BCW.BLL.Friend().GetFriends(pageIndex, pageSize, strWhere, out recordCount);
            if (listFriend.Count > 0)
            {
                int k = 1;
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<form id=\"forms\" method=\"post\" action=\"guest.aspx\">");
                builder.Append("<input type=\"num\" name=\"toidnum\" value=\"\" />");
                builder.Append("<input type=\"hidden\" name=\"act\" Value=\"recommend\"/>");
                builder.Append("<input type=\"hidden\" name=\"ac\" value=\"分享\"/>");
                builder.Append("<input type=\"hidden\" name=\"ptt\" value=\"" + ptt + "\"/>");
                builder.Append("<input type=\"hidden\" name=\"pageIn\" value=\"" + pageIndex + "\"/>");
                builder.Append("<input type=\"hidden\" name=\"purl\" Value=\"" + purl + "\"/>");
                builder.Append("<input type=\"hidden\" name=\"getPage\" Value=\"" + getPage + "\"/>");
                VE = ConfigHelper.GetConfigString("VE");
                SID = ConfigHelper.GetConfigString("SID");
                builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
                builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
                builder.Append("<input  name=\"hold\" class=\"btn-red\" type=\"submit\" value=\"选择\"/>");
                builder.Append("</form>");
                builder.Append(Out.Tab("</div>", ""));
                foreach (BCW.Model.Friend n in listFriend)
                {
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", ""));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", ""));
                    }

                    string sFriendName = n.FriendName;
                    string nFriendName = new BCW.BLL.User().GetUsName(n.FriendID);
                    if (sFriendName != nFriendName)
                        sFriendName = n.FriendName + "(" + nFriendName + ")";
                    builder.Append("<form id=\"form" + n.FriendID + "\" method=\"post\" action=\"guest.aspx\">");
                    builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.FriendID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + ((pageIndex - 1) * pageSize + k) + "." + sFriendName + "</a>(" + n.FriendID + ")");
                    builder.Append("<input type=\"hidden\" name=\"toidnum\" value=\"" + n.FriendID + "\" />");
                    builder.Append("<input type=\"hidden\" name=\"act\" Value=\"recommend\"/>");
                    builder.Append("<input type=\"hidden\" name=\"ac\" value=\"分享\"/>");
                    builder.Append("<input type=\"hidden\" name=\"ptt\" value=\"" + ptt + "\"/>");
                    builder.Append("<input type=\"hidden\" name=\"pageIn\" value=\"" + pageIndex + "\"/>");
                    builder.Append("<input type=\"hidden\" name=\"purl\" Value=\"" + purl + "\"/>");
                    builder.Append("<input type=\"hidden\" name=\"getPage\" Value=\"" + getPage + "\"/>");
                    VE = ConfigHelper.GetConfigString("VE");
                    SID = ConfigHelper.GetConfigString("SID");
                    builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
                    builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
                    #region 判断是否存在
                    bool exitse10 = new BCW.HB.BLL.Shared().Exists(uid);
                    if (exitse10)
                    {
                        BCW.HB.Model.Shared shaexo = new BCW.HB.BLL.Shared().GetModel(uid);
                        string[] pdid = shaexo.SharedIDList.Split(',');
                        bool cz = false;
                        for (int ii = 0; ii < pdid.Length; ii++)
                        {
                            if (pdid[ii].Trim() == n.FriendID.ToString())
                            {
                                cz = true;
                            }
                        }
                        if (cz == false)
                        {
                            builder.Append("<input  name=\"hold\" type=\"submit\" value=\"选择\"/>");
                        }
                        else
                        {
                            builder.Append("<input type=\"hidden\" name=\"deleteid\" Value=\"" + n.FriendID + "\"/>");
                            builder.Append("<input  name=\"delete\" type=\"submit\" value=\"取消\"/>");
                        }
                    }
                    else
                    {
                        builder.Append("<input  name=\"hold\" type=\"submit\" value=\"选择\"/>");
                    }
                    #endregion
                    builder.Append(Out.Tab("</div>", ""));
                    builder.Append("</form>");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }

                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 1));
                builder.Append(Out.Tab("<div>", "<br/>"));
                builder.Append("<b>已选择的好友:</b>");
                builder.Append(Out.Tab("</div>", "<br />"));
                bool exitse2 = new BCW.HB.BLL.Shared().Exists(uid);
                if (exitse2)
                {
                    BCW.HB.Model.Shared yxshare = new BCW.HB.BLL.Shared().GetModel(uid);
                    string[] xzs = yxshare.SharedIDList.Split(',');
                    string fxname = "";
                    for (int i = 0; i < xzs.Length; i++)
                    {
                        if (xzs[i].Trim() != "")
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
                            int xzs2 = Convert.ToInt32(xzs[i]);
                            fxname = new BCW.BLL.User().GetUsName(xzs2);
                            builder.Append("<form id=\"form10" + i + "\" method=\"post\" action=\"guest.aspx\">");
                            builder.Append("<input type=\"hidden\" name=\"deleteid\" Value=\"" + xzs2 + "\"/>");
                            builder.Append("<input type=\"hidden\" name=\"act\" Value=\"recommend\"/>");
                            builder.Append("<input type=\"hidden\" name=\"ac\" value=\"分享\"/>");
                            builder.Append("<input type=\"hidden\" name=\"ptt\" value=\"" + ptt + "\"/>");
                            builder.Append("<input type=\"hidden\" name=\"pageIn\" value=\"" + pageIndex + "\"/>");
                            builder.Append("<input type=\"hidden\" name=\"purl\" Value=\"" + purl + "\"/>");
                            builder.Append("<input type=\"hidden\" name=\"getPage\" Value=\"" + getPage + "\"/>");
                            VE = ConfigHelper.GetConfigString("VE");
                            SID = ConfigHelper.GetConfigString("SID");
                            builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
                            builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
                            builder.Append(fxname + "(" + xzs2 + ")");
                            builder.Append("<input name=\"delete\" type=\"submit\" value=\"删\"/><br />");
                            builder.Append("</form>");
                            builder.Append(Out.Tab("</div>", ""));
                        }

                    }
                }

                // 分页
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录.."));

            }
            BCW.HB.Model.Shared tjshare = new BCW.HB.BLL.Shared().GetModel(uid);
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<form id=\"form10\" method=\"post\" action=\"guest.aspx\">");
            builder.Append("<input type=\"hidden\" name=\"idlist\" Value=\"" + tjshare.SharedIDList + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"act\" Value=\"recomsave\"/>");
            builder.Append("<input type=\"hidden\" name=\"purl\" Value=\"" + purl + "\"/>");
            builder.Append("<b>附言:</b><br />");
            builder.Append("<input type=\"text\" name=\"Content\" Value=\"\"/><br />");
            VE = ConfigHelper.GetConfigString("VE");
            SID = ConfigHelper.GetConfigString("SID");
            builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
            builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
            builder.Append("<input class=\"btn-blue\"  name=\"share\" type=\"submit\" value=\"确认分享\"/>");
            builder.Append("</form>");
            builder.Append("注：不选则分享给全部好友。");
            builder.Append(Out.Tab("</div>", ""));
            #endregion
        }
        else
        {
            #region 举报
            string strReport = "0|广告宣传|1|非法转币|2|非法赌博|3|色情内线|4|其他举报";
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("欢迎举报非法帖子");
            builder.Append(Out.Tab("</div>", ""));
            strText = "举报类别:,举报描述:,,,,";
            strName = "ptype,Content,purl,act,backurl";
            strType = "select,text,hidden,hidden,hidden";
            strValu = "0''" + purl + "'reportsave'" + Utils.getPage(0) + "";
            strEmpt = "" + strReport + ",true,false,false,false";
            strIdea = "/";
            strOthe = "确定举报,guest.aspx,post,1,blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("注意:非法举报将受到严惩");
            builder.Append(Out.Tab("</div>", ""));
            #endregion
        }

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?backurl=" + Utils.getPage(0) + "") + "\">内线</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void RecomSavePage(int uid)
    {
        BCW.User.Users.ShowVerifyRole("o", uid);//非验证会员提示
        new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Guest, uid);//会员本身权限
        int toid = 0;
        int NodeId = 0;
        BCW.HB.Model.Shared share = new BCW.HB.BLL.Shared().GetModel(uid);
        //20160111
        String toidnum = Utils.GetRequest("idlist", "post", 1, "", "");
        if (toidnum == "")
        {
            toid = int.Parse(Utils.GetRequest("toid", "post", 1, @"^[1-9]\d*$", "0"));
        }
        if (toid == 0)
        {
            NodeId = int.Parse(Utils.GetRequest("NodeId", "post", 1, @"^[1-9]\d*$", "0"));

        }
        string Content = Utils.GetRequest("Content", "post", 3, @"^[^\^]{1,50}$", "附言最多50字，可留空");
        string purl = Utils.GetRequest("purl", "post", 2, @"^(\[url=(.[^\]]*)\])([\s\S]+?)(\[\/url\])$", "分享地址无效");
        if (Content != "")
            Content = "，附言:" + Content + "";

        int ToId = toid;
        string UsName = new BCW.BLL.User().GetUsName(uid);
        if (toidnum != "")
        {
            string[] toidlist = toidnum.Split(',');
            int[] Toids = new int[toidlist.Length];
            int sum = 0;
            for (int i = 0; i < toidlist.Length; i++)
            {
                try
                {
                    Toids[i] = Convert.ToInt32(toidlist[i]);
                    if (Toids[i] == 0)
                    {
                        Utils.Error("收信ID错误", "");
                    }
                    if (!new BCW.BLL.User().Exists(Toids[i]))
                    {
                        Utils.Error("不存在的收信ID", "");
                    }
                    if (ToId.Equals(uid))
                    {
                        Utils.Error("不能给自己分享", "");
                    }

                    //你是否是对方的黑名单
                    if (new BCW.BLL.Friend().Exists(Toids[i], uid, 1))
                    {
                        Utils.Error("对方已把您加入黑名单", "");
                    }
                    //对方是否拒绝接收分享内线
                    string ForumSet = new BCW.BLL.User().GetForumSet(Toids[i]);
                    int Nore = BCW.User.Users.GetForumSet(ForumSet, 14);
                    if (Nore == 1)
                    {
                        Utils.Error("对方已设置拒绝接收分享内线", "");
                    }

                    BCW.Model.Guest model = new BCW.Model.Guest();
                    model.FromId = 0;
                    model.FromName = UsName;
                    model.ToId = Toids[i];
                    model.ToName = new BCW.BLL.User().GetUsName(Toids[i]);
                    model.Content = "[url=/bbs/uinfo.aspx?uid=" + uid + "]" + UsName + "[/url]向您分享：" + purl + "" + Content + "";
                    model.TransId = 0;
                    new BCW.BLL.Guest().Add(model);
                    //更新联系时间
                    new BCW.BLL.Friend().UpdateTime(uid, Toids[i]);
                    long Share = Convert.ToInt64(ub.GetSub("GuestShare", xmlPath));
                    if (Share > 0)
                    {
                        new BCW.BLL.User().UpdateiGold(uid, -Share, "分享好友");
                    }
                    sum++;
                }
                catch
                {

                }
            }
            Utils.Success("分享好友", "分享" + sum + "位好友成功，正在返回..", Utils.getPage(share.ShareContent), "1");
        }
        if (toid != 0)
        {

            if (ToId == 0)
            {
                Utils.Error("收信ID错误", "");
            }
            if (!new BCW.BLL.User().Exists(ToId))
            {
                Utils.Error("不存在的收信ID", "");
            }
            if (ToId.Equals(uid))
            {
                Utils.Error("不能给自己分享", "");
            }

            //你是否是对方的黑名单
            if (new BCW.BLL.Friend().Exists(ToId, uid, 1))
            {
                Utils.Error("对方已把您加入黑名单", "");
            }
            //对方是否拒绝接收分享内线
            string ForumSet = new BCW.BLL.User().GetForumSet(ToId);
            int Nore = BCW.User.Users.GetForumSet(ForumSet, 14);
            if (Nore == 1)
            {
                Utils.Error("对方已设置拒绝接收分享内线", "");
            }

            BCW.Model.Guest model = new BCW.Model.Guest();
            model.FromId = 0;
            model.FromName = UsName;
            model.ToId = ToId;
            model.ToName = new BCW.BLL.User().GetUsName(ToId);
            model.Content = "[url=/bbs/uinfo.aspx?uid=" + uid + "]" + UsName + "[/url]向您分享：" + purl + "" + Content + "";
            model.TransId = 0;
            new BCW.BLL.Guest().Add(model);
            //更新联系时间
            new BCW.BLL.Friend().UpdateTime(uid, ToId);
            long Share = Convert.ToInt64(ub.GetSub("GuestShare", xmlPath));
            if (Share > 0)
            {
                new BCW.BLL.User().UpdateiGold(uid, -Share, "分享好友");
            }
            Utils.Success("分享", "分享成功，正在返回..", Utils.getPage(share.ShareContent), "1");
        }
        else
        {

            if (NodeId != 0)
            {
                if (!new BCW.BLL.Frigroup().Exists(NodeId, uid, 0))
                {
                    Utils.Error("不存在的好友分组", "");
                }
            }


            DataSet ds = new BCW.BLL.Friend().GetList("FriendID,FriendName,UsName", "UsID=" + uid + " and Types=0 and NodeId=" + NodeId + "");
            int k = 0;
            if (ds != null && ds.Tables[0].Rows.Count != 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    int FriendID = int.Parse(ds.Tables[0].Rows[i]["FriendID"].ToString());
                    //你是否是对方的黑名单
                    bool Isblack = false;
                    if (new BCW.BLL.Friend().Exists(FriendID, uid, 1))
                    {
                        Isblack = true;
                    }
                    //对方是否拒绝接收分享内线
                    string ForumSet = new BCW.BLL.User().GetForumSet(FriendID);
                    int Nore = BCW.User.Users.GetForumSet(ForumSet, 14);
                    if (Nore == 0 && Isblack == false)
                    {
                        string FriendName = ds.Tables[0].Rows[i]["FriendName"].ToString();
                        BCW.Model.Guest model = new BCW.Model.Guest();
                        model.FromId = 0;
                        model.FromName = UsName;
                        model.ToId = FriendID;
                        model.ToName = FriendName;
                        model.Content = "[url=/bbs/uinfo.aspx?uid=" + uid + "]" + UsName + "[/url]向您分享：" + purl + "" + Content + "";
                        model.TransId = 0;
                        new BCW.BLL.Guest().Add(model);
                        //更新联系时间
                        new BCW.BLL.Friend().UpdateTime(uid, FriendID);

                    }
                    k++;
                }
                long Share = Convert.ToInt64(int.Parse(ub.GetSub("GuestShare", xmlPath)) * k);
                if (Share > 0)
                {
                    new BCW.BLL.User().UpdateiGold(uid, -Share, "分享" + k + "位好友");
                }
                Utils.Success("分享好友", "分享" + k + "位好友成功，正在返回..", Utils.getPage(share.ShareContent), "1");
            }
        }


    }

    private void ReportSavePage(int uid)
    {
        new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Guest, uid);//会员本身权限
        int ptype = int.Parse(Utils.GetRequest("ptype", "post", 1, @"^[0-4]\d*$", "0"));
        string Content = Utils.GetRequest("Content", "post", 3, @"^[^\^]{1,50}$", "描述最多50字，可留空");
        string purl = Utils.GetRequest("purl", "post", 2, @"^(\[url=(.[^\]]*)\])(.[^\[]*)(\[\/url\])$", "举报地址无效");
        if (Content == "")
            Content = "无";

        int ToId = Convert.ToInt32(ub.GetSub("GuestReportID", xmlPath));//接收举报的管理员ID
        if (ToId.Equals(uid))
        {
            Utils.Error("不能给自己举报内线", "");
        }
        BCW.Model.Guest model = new BCW.Model.Guest();
        model.FromId = uid;
        model.FromName = new BCW.BLL.User().GetUsName(uid);
        model.ToId = ToId;
        model.ToName = new BCW.BLL.User().GetUsName(ToId);
        model.Content = "" + BCW.User.AppCase.CaseReport(ptype) + "举报：" + purl + "描述：" + Content + "";
        model.TransId = 0;
        new BCW.BLL.Guest().Add(model);
        //更新联系时间
        new BCW.BLL.Friend().UpdateTime(uid, ToId);

        Utils.Success("举报", "举报成功，正在返回..", Utils.getPage("guest.aspx"), "1");
    }


    private void Recom2Page(int uid)
    {
        Master.Title = "举报内线";
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.Guest model = new BCW.BLL.Guest().GetGuest(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.ToId != uid)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.FromId == 0)
        {
            Utils.Error("系统内线不能举报", "");
        }

        builder.Append(Out.Tab("<div class=\"title\">举报内线</div>", ""));

        string strReport = "0|广告宣传|1|非法转币|2|非法赌博|3|色情内线|4|其他举报";
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("欢迎举报非法内线");
        builder.Append(Out.Tab("</div>", ""));
        strText = "举报类别:,举报描述:,,,,";
        strName = "ptype,Content,id,act,backurl";
        strType = "select,text,hidden,hidden,hidden";
        strValu = "0''" + id + "'reportsave2'" + Utils.getPage(0) + "";
        strEmpt = "" + strReport + ",true,false,false,false";
        strIdea = "/";
        strOthe = "确定举报,guest.aspx,post,1,blue";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("注意:非法举报将受到严惩");
        builder.Append(Out.Tab("</div>", ""));


        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append(ShowBackPage(uid));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?backurl=" + Utils.getPage(0) + "") + "\">内线</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ReportSave2Page(int uid)
    {
        new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Guest, uid);//会员本身权限
        int ptype = int.Parse(Utils.GetRequest("ptype", "post", 1, @"^[0-4]\d*$", "0"));
        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.Guest model = new BCW.BLL.Guest().GetGuest(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.ToId != uid)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.FromId == 0)
        {
            Utils.Error("系统内线不能举报", "");
        }
        string Content = Utils.GetRequest("Content", "post", 3, @"^[^\^]{1,50}$", "描述最多50字，可留空");
        if (Content == "")
            Content = "无";

        int ToId = Convert.ToInt32(ub.GetSub("GuestReportID", xmlPath));//接收举报的管理员ID
        if (ToId.Equals(uid))
        {
            Utils.Error("不能给自己举报内线", "");
        }
        BCW.Model.Guest model2 = new BCW.Model.Guest();
        model2.FromId = uid;
        model2.FromName = new BCW.BLL.User().GetUsName(uid);
        model2.ToId = ToId;
        model2.ToName = new BCW.BLL.User().GetUsName(ToId);
        model2.Content = "" + BCW.User.AppCase.CaseReport(ptype) + "举报：" + model.FromName + "(" + model.FromId + ")内线(" + model.Content + ")描述：" + Content + "";
        model2.TransId = 0;
        new BCW.BLL.Guest().Add(model2);
        //更新联系时间
        new BCW.BLL.Friend().UpdateTime(uid, ToId);

        Utils.Success("举报", "举报成功，正在返回..", Utils.getPage("guest.aspx"), "1");
    }

    private void ViewPage(int uid)
    {
        Master.Title = "查看内线";
        int hid = 0;
        int v_id = 0;
        int id = int.Parse(Utils.GetRequest("id", "get", 1, @"^[1-9]\d*$", "0"));
        if (id != 0)
        {
            if (!new BCW.BLL.Guest().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
        }
        else
        {
            v_id = 1;
            hid = int.Parse(Utils.GetRequest("hid", "get", 1, @"^[0-9]\d*$", "-1"));
            if (hid == -1)
            {
                Utils.Error("不存在的会员ID", "");
            }
            if (hid != 0 && !new BCW.BLL.User().Exists(hid))
            {
                Utils.Error("不存在的会员ID", "");
            }
            if (hid == 0)
            {
                //记录上一个足迹
                string GetPageUrl = Utils.getPage(0);
                if (!GetPageUrl.Contains("%2fguest.aspx"))
                {
                    new BCW.BLL.User().UpdateVisitHy(uid, Server.UrlDecode(GetPageUrl));
                }
            }
            DataSet ds = new BCW.BLL.Guest().GetList("TOP 1 ID", "FromId=" + hid + " and ToId=" + uid + " and TDel=0 and IsSeen=0 Order by AddTime ASC");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                id = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
            }
            else
            {
                Utils.Success("查看内线", "暂无新内线.<br /><a href=\"" + Utils.getUrl("guest.aspx?backurl=" + Utils.getPage(0) + "") + "\">返回我的内线&gt;&gt;</a>", Utils.getPage("guest.aspx"), "3");
            }
        }
        BCW.Model.Guest model = new BCW.BLL.Guest().GetGuest(id);
        bool IsMeSend = false;
        int ptype = 0;
        if (model.FromId == uid)
        {
            if (!new BCW.BLL.Guest().ExistsFrom(id, uid))
            {
                Utils.Error("不存在的记录", "");
            }
            IsMeSend = true;
            ptype = 2;
            hid = model.ToId;
        }
        else
        {
            if (!new BCW.BLL.Guest().ExistsTo(id, uid))
            {
                Utils.Error("不存在的记录", "");
            }
            if (model.FromId == 0)
                ptype = 1;

            hid = model.FromId;

            //更新为已读
            if (model.IsSeen == 0)
            {
                new BCW.BLL.Guest().UpdateIsSeen(id);
            }
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?backurl=" + Utils.getPage(0) + "") + "\">内线</a>&gt;查看内线");
        if (v_id == 1)
        {
            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=view&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[刷新]</a>");
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        if (model.FromId == 0)
        {
            builder.Append("来自系统内线:" + DT.FormatDate(model.AddTime, 5) + "");
        }
        else
        {
            if (!IsMeSend)
                builder.Append("发送人:<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + model.FromId + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.FromName + "(" + model.FromId + ")</a>" + DT.FormatDate(model.AddTime, 5) + "");
            else
                builder.Append("收件人:<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + model.ToId + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.ToName + "(" + model.ToId + ")</a>" + DT.FormatDate(model.AddTime, 5) + "");
        }
        builder.Append(Out.Tab("</div>", Out.Hr()));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("" + Out.SysUBB(model.Content.Replace("showGuess.aspx?gid=", "/bbs/guess2/showGuess.aspx?gid=")) + "");
        if (model.Types == 5)
        {
            builder.Append("<font color=\"#BDBDBD\">如果你已阅读和熟知内容,<br/>请点击</font><a style=\"text-decoration:none\" href=\"" + Utils.getUrl("guest.aspx?act=sendok&amp;id=" + model.ID + "&amp;ptype=" + ptype + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"><font color=\"green\">我已阅读本消息</font></a><br/>");
        }
        if (model.TransId > 0)
        {
            builder.Append("<br />转自ID:<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + model.TransId + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.TransId + "</a>");
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        if (!IsMeSend && model.FromId != 0)
        {
            strText = ",,,,,,";
            strName = "toid,transid,Face,Content,act,backurl";
            strType = "hidden,hidden,select,text,hidden,hidden";
            strValu = "" + model.FromId + "'" + (-id) + "'0''save'" + Utils.getPage(0) + "";
            strEmpt = "false,false,0|表情|1|微笑|2|哭泣|3|调皮|4|脸红|5|害羞|6|生气|7|偷笑|8|流汗|9|我倒|10|得意|11|眦牙|12|疑问|13|睡觉|14|好色|15|口水|16|敲头|17|可爱|18|猪头|19|胜利|20|玫瑰|21|吃药|22|白酒|23|可乐|24|囧囧|25|亲亲|26|抱抱,true,false,false";
            strIdea = "";
            strOthe = "回复,guest.aspx,post,3,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("", "<br />"));


            //取TA回复你哪一条信息
            if (model.TransId < 0)
            {
                BCW.Model.Guest model2 = new BCW.BLL.Guest().GetGuest(Math.Abs(model.TransId));
                if (model2 != null)
                {
                    builder.Append(Out.Tab("<div class=\"text\">", ""));
                    builder.Append("上次对话：" + Out.SysUBB(model2.Content.Replace("showGuess.aspx?gid=", "/bbs/guess2/showGuess.aspx?gid=")) + "");
                    builder.Append(Out.Tab("</div>", "<br />"));
                }
            }
        }

        if (model.FromId == 0)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=keep&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">收藏</a>.");
            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=trans&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">转发</a>.");
            builder.Append("<a href=\"" + Utils.getUrl("function.aspx?act=copyg&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">复制</a>.");
            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=del&amp;id=" + id + "&amp;ptype=" + ptype + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">删除</a>");
            builder.Append(Out.Tab("</div>", Out.Hr()));
        }
        else
        {
            builder.Append(Out.Tab("", Out.RHr()));
        }

        //读取3条这对话记录
        hid = model.FromId;
        DataSet ds2 = new BCW.BLL.Guest().GetList("TOP 3 ID,FromId,Content,TransId,IsSeen,AddTime", "(FromId = " + uid + " and ToId = " + hid + " and FDel = 0) OR (FromId = " + hid + " and ToId = " + uid + " and TDel = 0) Order by AddTime DESC");
        if (ds2 != null && ds2.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
            {
                int nid = int.Parse(ds2.Tables[0].Rows[i]["ID"].ToString());
                int FromId = int.Parse(ds2.Tables[0].Rows[i]["FromId"].ToString());
                int TransId = int.Parse(ds2.Tables[0].Rows[i]["TransId"].ToString());
                int IsSeen = int.Parse(ds2.Tables[0].Rows[i]["IsSeen"].ToString());
                DateTime AddTime = DateTime.Parse(ds2.Tables[0].Rows[i]["AddTime"].ToString());

                string strTrans = string.Empty;
                string Content = Out.SysUBB(ds2.Tables[0].Rows[i]["Content"].ToString().Replace("showGuess.aspx?gid=", "/bbs/guess/showGuess.aspx?gid="));
                if (TransId > 0)
                {
                    strTrans = "[转自<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + TransId + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + TransId + "</a>]";
                }
                builder.Append(Out.Tab("<div>", ""));
                if (FromId == uid)
                {
                    builder.Append("WO说:" + Content + "" + strTrans + "<small>(" + DT.FormatDate(AddTime, 1) + ")</small>");
                }
                else
                {
                    if (IsSeen == 0)
                        builder.Append("[新]");


                    if (model.FromId != 0)
                    {
                        builder.Append("TA说:" + Content + "" + strTrans + "<small>(" + DT.FormatDate(AddTime, 1) + ")</small>");
                        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=view&amp;id=" + nid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">回复</a>");
                    }
                    else
                    {
                        builder.Append("系统:" + Content + "" + strTrans + "<small>(" + DT.FormatDate(AddTime, 1) + ")</small>");
                    }
                }
                builder.Append("<br />---------");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
        }
        builder.Append(Out.Tab("<div>", ""));
        if (!IsMeSend)
        {
            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=chat&amp;hid=" + hid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[进入聊天模式]</a>");
        }
        if (hid == 0)
        {
            builder.Append("<br /><a href=\"" + Utils.getUrl("guest.aspx?act=read&amp;ptype=1&amp;backurl=" + Utils.PostPage(1) + "") + "\">[所有标为已读]</a>");
        }

        if (model.FromId != 0)
        {
            if (!IsMeSend)
            {
                builder.Append("<br /><a href=\"" + Utils.getUrl("guest.aspx?act=add&amp;hid=" + model.FromId + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">回复</a>.");
                builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=keep&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">收藏</a>.");
            }
            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=trans&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">转发</a>.");
            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=del&amp;id=" + id + "&amp;ptype=" + ptype + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("friend.aspx?act=addblack&amp;hid=" + hid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">加黑</a>.");
            builder.Append("<a href=\"" + Utils.getUrl("function.aspx?act=copyg&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">复制</a>.");
            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=recommend2&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">举报</a>");

        }
        builder.Append(Out.Tab("</div>", ""));


        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append(ShowBackPage(uid));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("guest.aspx").Replace("/utility/book.aspxurl=", "/utility/book.aspx?url=") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx") + "\">内线</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void KeepPage(int uid)
    {
        Master.Title = "收藏内线";
        int id = int.Parse(Utils.GetRequest("id", "get", 1, @"^[1-9]\d*$", "ID错误"));
        string info = Utils.GetRequest("info", "get", 1, "", "");
        if (!new BCW.BLL.Guest().ExistsTo(id, uid))
        {
            Utils.Error("不存在的记录", "");
        }
        if (info != "ok")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定收藏此内线吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=keep&amp;info=ok&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定收藏</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=view&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            new BCW.BLL.Guest().UpdateIsKeep(id, uid);
            Utils.Success("收藏内线", "收藏成功，正在返回..", Utils.getPage("guest.aspx?backurl=" + Utils.getPage(0) + ""), "1");
        }
    }

    private void DelPage(int uid)
    {
        Master.Title = "删除内线";
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 2, @"^[0-3]\d*$", "类型错误"));
        int id = int.Parse(Utils.GetRequest("id", "get", 1, @"^[1-9]\d*$", "ID错误"));
        string info = Utils.GetRequest("info", "get", 1, "", "");
        if (ptype == 2)
        {
            if (!new BCW.BLL.Guest().ExistsFrom(id, uid))
            {
                Utils.Error("不存在的记录", "");
            }
        }
        else
        {
            if (!new BCW.BLL.Guest().ExistsTo(id, uid))
            {
                Utils.Error("不存在的记录", "");
            }

        }
        if (info != "ok")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此内线吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=del&amp;info=ok&amp;id=" + id + "&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=view&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            //更新为已读
            if (new BCW.BLL.Guest().GetIsSeen(id) == 0)
            {
                new BCW.BLL.Guest().UpdateIsSeen(id);
            }
            if (ptype == 1)
                new BCW.BLL.Guest().Delete(id);
            else if (ptype == 2)
                new BCW.BLL.Guest().UpdateFDel(id);
            else
                new BCW.BLL.Guest().UpdateTDel(id);

            Utils.Success("删除内线", "删除成功，正在返回..", Utils.getPage("guest.aspx?ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "").Replace("/utility/book.aspxurl=", "/utility/book.aspx?url="), "1");
        }
    }

    private void DelPagePage(int uid)
    {
        Master.Title = "删除本页内线";
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 2, @"^[0-3]\d*$", "类型错误"));
        int page = int.Parse(Utils.GetRequest("page", "get", 1, @"^[1-9]\d*$", "页面ID错误"));
        string info = Utils.GetRequest("info", "get", 1, "", "");
        if (info != "ok")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除本页已读内线吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=delpage&amp;info=ok&amp;ptype=" + ptype + "&amp;page=" + page + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            int pageIndex;
            int pageSize = 6;
            string strWhere = "";
            pageIndex = page;
            if (pageIndex == 0)
                pageIndex = 1;
            //查询条件
            if (ptype == 0)
                strWhere = "ToId=" + uid + " and FromId>0 and TDel=0 and IsSeen=1 and IsKeep=0";
            else if (ptype == 1)
                strWhere = "ToId=" + uid + " and FromId=0 and IsSeen=1 ";
            else if (ptype == 2)
                strWhere = "FromId=" + uid + " and FDel=0";
            else
                strWhere = "ToId=" + uid + " and TDel=0 and IsKeep=1";

            // 开始读取列表
            IList<BCW.Model.Guest> listGuest = new BCW.BLL.Guest().GetGuestsID(pageIndex, pageSize, strWhere);
            if (listGuest.Count > 0)
            {
                foreach (BCW.Model.Guest n in listGuest)
                {
                    if (ptype == 1)
                        new BCW.BLL.Guest().Delete(n.ID);
                    else if (ptype == 2)
                        new BCW.BLL.Guest().UpdateFDel(n.ID);
                    else
                        new BCW.BLL.Guest().UpdateTDel(n.ID);
                }
            }

            Utils.Success("删除内线", "删除成功，正在返回..", Utils.getUrl("guest.aspx?ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
    }

    private void DelPtypePage(int uid)
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 2, @"^[0-3]\d*$", "类型错误"));
        string info = Utils.GetRequest("info", "get", 1, "", "");
        string sText = string.Empty;
        if (ptype == 0)
        {
            sText = "删除已读内线";
        }
        else if (ptype == 1)
        {
            sText = "删除已读系统内线";
        }
        else if (ptype == 2)
        {
            sText = "删除已发内线";
        }
        else
        {
            sText = "删除收藏内线";
        }
        Master.Title = sText;

        if (info != "ok")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定" + sText + "吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=delptype&amp;info=ok&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            if (ptype == 0)
                new BCW.BLL.Guest().UpdateTDel2(uid);
            else if (ptype == 1)
                new BCW.BLL.Guest().UpdateXDel2(uid);
            else if (ptype == 2)
                new BCW.BLL.Guest().UpdateFDel2(uid);
            else
                new BCW.BLL.Guest().UpdateKDel2(uid);

            Utils.Success("删除内线", "删除成功，正在返回..", Utils.getUrl("guest.aspx?ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
    }

    private void ReadPage(int uid)
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 2, @"^[0-1]\d*$", "类型错误"));
        string info = Utils.GetRequest("info", "get", 1, "", "");
        string Text = string.Empty;
        if (ptype == 0)
            Text = "我的内线";
        else
            Text = "系统内线";

        Master.Title = "" + Text + "标为已读";

        if (info != "ok")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定标记" + Text + "为已读吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=read&amp;info=ok&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定标记</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            new BCW.BLL.Guest().UpdateIsSeenAll(uid, ptype);
            Utils.Success("标记已读", "标记已读成功，正在返回..", Utils.getPage("guest.aspx?ptype=" + ptype + ""), "1");
        }
    }

    private void ChatPage(int uid)
    {
        Master.Title = "聊天模式对话";
        int hid = int.Parse(Utils.GetRequest("hid", "get", 1, @"^[0-9]\d*$", "-1"));
        int ordertype = int.Parse(Utils.GetRequest("ordertype", "get", 1, @"^[0-1]$", "0"));
        if (hid == -1)
        {
            Utils.Error("不存在的会员ID", "");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        if (hid != 0 && !new BCW.BLL.User().Exists(hid))
        {
            Utils.Error("不存在的会员ID", "");
        }

        if (hid != 0)
        {
            string UsName = new BCW.BLL.User().GetUsName(hid);
            builder.Append("正在和<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + hid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + UsName + "(" + hid + ")</a>对聊");
        }
        else
        {
            builder.Append("正在和系统对聊");
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        if (hid > 0)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=add&amp;hid=" + hid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">回复</a>/");
            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=delhid&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">清空记录</a>/");
            if (ordertype == 1)
                builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=chat&amp;hid=" + hid + "&amp;ordertype=0&amp;backurl=" + Utils.getPage(0) + "") + "\">倒序</a>/");
            else
                builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=chat&amp;hid=" + hid + "&amp;ordertype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">正序</a>/");

            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=chat&amp;hid=" + hid + "&amp;ordertype=" + ordertype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">刷新</a>");
            builder.Append(Out.Tab("</div>", "<br />"));

            strText = ",,,,";
            strName = "toid,Content,act,backurl";
            strType = "hidden,text,hidden,hidden";
            strValu = "" + hid + "''save'" + Utils.PostPage(1) + "";
            strEmpt = "false,true,false,false";
            strIdea = "";
            strOthe = "快速回复,guest.aspx,post,3,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("", "<br />"));
        }
        int pageIndex;
        int recordCount;
        //int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        int pageSize = 6;
        string strWhere = "";
        string strOrder = "";
        string[] pageValUrl = { "act", "hid", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "(FromId = " + uid + " and ToId = " + hid + " and FDel = 0) OR (FromId = " + hid + " and ToId = " + uid + " and TDel = 0)";
        if (ordertype == 1)
            strOrder = "ID Asc";
        else
            strOrder = "ID Desc";

        // 开始读取列表
        IList<BCW.Model.Guest> listGuest = new BCW.BLL.Guest().GetGuests(pageIndex, pageSize, strWhere, strOrder, out recordCount);
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
                string strTrans = string.Empty;
                string Content = Out.SysUBB(n.Content.Replace("showGuess.aspx?gid=", "/bbs/guess/showGuess.aspx?gid="));
                if (n.TransId > 0)
                {
                    strTrans = "[转自<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.TransId + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.TransId + "</a>]";
                }

                if (n.FromId == uid)
                {
                    builder.Append("WO说:" + Content + "" + strTrans + "<small>(" + DT.FormatDate(n.AddTime, 1) + ")</small>");
                }
                else
                {
                    if (hid > 0)
                    {
                        builder.Append("TA说:" + Content + "" + strTrans + "<small>(" + DT.FormatDate(n.AddTime, 1) + ")</small>");
                        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=view&amp;id=" + n.ID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">回复</a>");
                    }
                    else
                    {
                        builder.Append("系统:" + Content + "" + strTrans + "<small>(" + DT.FormatDate(n.AddTime, 1) + ")</small>");
                    }
                    //更新为已读
                    if (n.IsSeen == 0)
                    {
                        new BCW.BLL.Guest().UpdateIsSeen(n.ID);
                    }
                }
                builder.Append("<br />---------");
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
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append(ShowBackPage(uid));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("guest.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?backurl=" + Utils.getPage(0) + "") + "\">内线</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void DelHidPage(int uid)
    {
        int hid = int.Parse(Utils.GetRequest("hid", "get", 2, @"^[1-9]\d*$", "ID错误"));
        if (!new BCW.BLL.User().Exists(hid))
        {
            Utils.Error("不存在的ID", "");
        }
        string info = Utils.GetRequest("info", "get", 1, "", "");

        Master.Title = "删除与TA的对话";

        if (info != "ok")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除对TA的对话吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=delhid&amp;info=ok&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=chat&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            new BCW.BLL.Guest().UpdateChatFDel(uid, hid);
            new BCW.BLL.Guest().UpdateChatTDel(uid, hid);

            Utils.Success("删除对话", "删除对话成功，正在返回..", Utils.getUrl("guest.aspx?act=chat&amp;hid=" + hid + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
    }

    private void DelsPage(int uid)
    {
        string info = Utils.GetRequest("info", "get", 1, "", "");

        Master.Title = "清空我的内线";

        if (info != "ok")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定清空我的内线吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=dels&amp;info=ok&amp;backurl=" + Utils.getPage(0) + "") + "\">确定清空</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            new BCW.BLL.Guest().UpdateSClear(uid);

            Utils.Success("清空我的内线", "清空我的内线成功，正在返回..", Utils.getUrl("guest.aspx?backurl=" + Utils.getPage(0) + ""), "1");
        }
    }

    private void DelxPage(int uid)
    {
        Master.Title = "清空系统内线";
        string info = Utils.GetRequest("info", "get", 1, "", "");
        if (info != "ok")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定清空所有系统内线吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=delx&amp;info=ok&amp;backurl=" + Utils.getPage(0) + "") + "\">确定清空</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            int GetNum = new BCW.BLL.Guest().UpdateXClear(uid);
            Utils.Success("清空系统内线", "删除" + GetNum + "条系统内线成功，正在返回..", Utils.getUrl("guest.aspx?backurl=" + Utils.getPage(0) + ""), "1");
        }
    }

    private void TransPage(int uid)
    {
        Master.Title = "转发此内线";
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Guest().ExistsTo(id, uid) && !new BCW.BLL.Guest().ExistsFrom(id, uid))
        {
            Utils.Error("不存在的记录", "");
        }
        builder.Append(Out.Tab("<div class=\"title\">转发此内线</div>", ""));

        string strFriend = string.Empty;
        DataSet ds = new BCW.BLL.Friend().GetList("Top 10 FriendID,FriendName", "UsID=" + uid + " and Types=0 and AddTime> '" + DateTime.Now.AddDays(-2) + "'");//限两天内为最近联系人
        if (ds != null && ds.Tables[0].Rows.Count != 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                strFriend += "|" + ds.Tables[0].Rows[i]["FriendID"] + "|" + ds.Tables[0].Rows[i]["FriendName"] + "";
            }
        }
        strFriend = "0|最近联系人" + strFriend;

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("请输入收信ID：");
        builder.Append(Out.Tab("</div>", ""));
        BCW.Model.Guest model = new BCW.BLL.Guest().GetGuest(id);
        string hid = Utils.GetRequest("hid", "get", 1, @"^[1-9]\d*$", "");

        strText = ",或选择联系人:/,,,,";
        strName = "toid,itoid,transid,Content,act,backurl";
        strType = "snum,select,hidden,hidden,hidden,hidden";
        strValu = "" + hid + "'0'" + model.FromId + "'" + Out.UBB(model.Content) + "'save'" + Utils.getPage(0) + "";
        strEmpt = "false," + strFriend + ",false,false,false,false";
        strIdea = "<a href=\"" + Utils.getUrl("friend.aspx?act=online&amp;backurl=" + Utils.PostPage(1) + "") + "\">从好友选择<／a>'''''|/";
        strOthe = "&gt;发送,guest.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append(ShowBackPage(uid));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("guest.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?backurl=" + Utils.getPage(0) + "") + "\">内线</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void SmsMailPage(int uid)
    {
        Master.Title = "短信/邮箱内线提醒";
        string ForumSet = new BCW.BLL.User().GetForumSet(uid);
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {
            //string Email = Utils.GetRequest("Email", "post", 2, @"^(?:13|14|15|18)\d{9}$", "请输入正确的139邮箱,邮箱必须是手机号，无须填写“@139.com”");
            string Email = Utils.GetRequest("Email", "post", 3, @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", "请输入正确的邮箱地址");
            int IsSms1 = int.Parse(Utils.GetRequest("IsSms1", "post", 2, @"^[0-1]$", "提醒私信选择错误"));
            int IsSms2 = int.Parse(Utils.GetRequest("IsSms2", "post", 2, @"^[0-1]$", "提醒系统内线选择错误"));
            //new BCW.BLL.User().UpdateSmsEmail(uid, Email + "@139.com");

            new BCW.BLL.User().UpdateSmsEmail(uid, Email);
            string[] fs = ForumSet.Split(",".ToCharArray());
            string sforumsets = string.Empty;
            for (int i = 0; i < fs.Length; i++)
            {
                string[] sfs = fs[i].ToString().Split("|".ToCharArray());

                if (i == 27)
                {
                    sforumsets += "," + sfs[0] + "|" + IsSms1;
                }
                else if (i == 28)
                {
                    sforumsets += "," + sfs[0] + "|" + IsSms2;
                }
                else
                {
                    sforumsets += "," + sfs[0] + "|" + sfs[1];
                }
            }
            sforumsets = Utils.Mid(sforumsets, 1, sforumsets.Length);
            new BCW.BLL.User().UpdateForumSet(uid, sforumsets);
            Utils.Success("设置短信/邮箱内线提醒", "设置成功，正在返回..", Utils.getUrl("guest.aspx?act=smsmail&amp;backurl=" + Utils.getPage(0) + ""), "1");

        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">设置短信内线提醒</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("填写您的邮箱:");
            builder.Append(Out.Tab("</div>", ""));

            string SmsEmail = new BCW.BLL.User().GetSmsEmail(uid);
            int IsSms1 = BCW.User.Users.GetForumSet(ForumSet, 27);
            int IsSms2 = BCW.User.Users.GetForumSet(ForumSet, 28);

            strText = ",提醒私信:/,提醒系统内线:/,,,";
            strName = "Email,IsSms1,IsSms2,act,info,backurl";
            strType = "stext,select,select,hidden,hidden,hidden";
            strValu = "" + SmsEmail + "'" + IsSms1 + "'" + IsSms2 + "'smsmail'ok'" + Utils.getPage(0) + "";
            strEmpt = "false,0|不提醒|1|提醒,0|不提醒|1|提醒,false,false,false";
            strIdea = "'''''|/";
            strOthe = "&gt;确定设置,guest.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示：<br />1.支持设置多种邮箱进行内线提醒，如使用139手机邮、163、qq、126、gmail等等.<br />2.使用139手机邮箱可以让您的手机实时收到内线提醒.<br />3.智能手机可以使用手机自带的邮箱功能，设置好邮箱(如QQ邮箱)即可收到提醒。<br />4.由于存在邮箱通讯时间,打开提醒时,其它会员给您发送内线时,点击“发送”时需要等待几秒才可以发送成功.<br />5.当收到的邮件存放在邮箱的“垃圾箱”时，您可以将此类信息设置为“这不是垃圾邮件”<br /><a href=\"" + Utils.getUrl("guest.aspx?act=smsmailhelp") + "\">免费开通139邮箱&gt;&gt;</a>");

            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getPage("guest.aspx") + "\">上级</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?backurl=" + Utils.getPage(0) + "") + "\">内线</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private void SmsMailHelpPage(int uid)
    {
        Master.Title = "免费开通139邮箱";
        builder.Append(Out.Tab("<div class=\"title\">免费开通139邮箱</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("注册免费版139邮箱免费邮箱：<br />编辑短信KTYX到10086<br />");
        builder.Append("然后收到确认定制的免费邮箱的短信，按提示回复“是”进行确定，成功后将会收到邮箱和邮箱密码的短信。<br />");
        builder.Append("然后使用IE浏览器登录<a href=\"http://wapmail.10086.cn/\">http://wapmail.10086.cn</a>进行如下设置：<br />");
        builder.Append("设置-邮件通知-邮件到达通知方式： 普通短信（支持70字，免费）<br />");
        builder.Append("设置-邮件通知-手机接收时间： 0点-24点 每天");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=smsmail") + "\">&lt;&lt;设置短信提醒</a>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("guest.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?backurl=" + Utils.getPage(0) + "") + "\">内线</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
}