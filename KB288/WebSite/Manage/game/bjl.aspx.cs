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
using System.Text.RegularExpressions;
using BCW.Data;

public partial class Manage_game_bjl : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/bjl.xml";
    protected string GameName = ub.GetSub("baccaratName", "/Controls/bjl.xml");
    protected string baccarat_img = ub.GetSub("baccarat_img", "/Controls/bjl.xml");//图片路径
    protected int Times = int.Parse(ub.GetSub("PokerTimes", "/Controls/bjl.xml"));//开牌时间
    protected int RoomTime1 = Convert.ToInt32(ub.GetSub("baccaratRoomTime1", "/Controls/bjl.xml")); //封庄的最低局数

    ub xml = new ub();
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {

            case "stat":
                StatPage();//盈利分析
                break;
            case "weihu":
                WeihuPage();//游戏维护
                break;
            case "reset":
                ResetPage();//游戏重置
                break;
            case "set":
                SetPage();//系统设置
                break;
            case "top":
                TopPage();//游戏排行
                break;
            case "back":
                BackPage();//返赢返负
                break;
            case "backsave":
            case "backsave2":
                BackSavePage(act);
                break;
            case "backsave3":
            case "backsave5":
                BackSavePage2(act);
                break;
            case "ReWard":
                ReWard();//排行榜奖励发放--界面
                break;
            case "ReWardCase":
                ReWardCase();//排行榜奖励发放--操作
                break;
            case "xiazhu":
                xiazhuPage();
                break;
            case "setroom":
                setroomPage();//最高最低下注设置
                break;
            case "del":
                delPage();//删除某一下注
                break;
            case "fangjian":
                fangjianPage();//房间
                break;
            case "gonggao":
                gonggaoPage();//公告查看与修改
                break;
            case "ggshow":
                ggshowPage();//公告查看
                break;
            case "fzhuang":
                fzhuangPage();//封庄
                break;
            case "diary":
                diaryPage();//日志显示
                break;
            case "tablelist":
                tablelistPage();//查看牌局详细
                break;
            case "edit":
                EditPage();//修改删除桌面
                break;
            default:
                ReloadPage();
                break;
        }
    }
    // 首页
    private void ReloadPage()
    {
        Master.Title = "" + GameName + "";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));

        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[1-2]$", "1"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 1)
            builder.Append("<h style=\"color:red\">进行中</h>|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?ptype=1&amp;uid=" + uid + "") + "\">进行中</a>|");
        if (ptype == 2)
            builder.Append("<h style=\"color:red\">已结束</h>");
        else
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?ptype=2&amp;uid=" + uid + "") + "\">已结束</a>");
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

        if (uid > 0)
            strWhere += "and usid=" + uid + "";
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
                builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=edit&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[管理]&gt;</a>");
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("bjl.aspx?act=fangjian&amp;roomid=" + n.ID + "") + "\">" + n.ID + "号桌</a>:");
                builder.Append("" + new BCW.BLL.User().GetUsName(n.UsID) + "(" + n.UsID + ")");

                if (ptype == 1)
                {
                    builder.Append("(彩池:" + n.Total_Now + "/人气:" + n.Click + ")");
                }
                else
                {
                    builder.Append("(已结束)盈利:" + n.Total_Now + "");//(n.Total_Now - n.Total)
                }
                if (new BCW.Baccarat.BLL.BJL_Play().Exists_wj(n.ID))
                {
                    builder.Append("[正在进行中..]");
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
        string strValu = "";
        string strText = "输入庄家ID:/";
        string strName = "uid";
        string strType = "num";
        if (uid > 0)
            strValu = "" + uid + "'";
        else
            strValu = "'";
        string strEmpt = "true";
        string strIdea = "/";
        string strOthe = "搜庄,bjl.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));


        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("【管理操作】");
        builder.Append(Out.Tab("</div>", "<br/>"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=xiazhu") + "\">用户下注</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=top") + "\">游戏排行</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=stat") + "\">赢利分析</a><br />");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("【系统操作】");
        builder.Append(Out.Tab("</div>", "<br/>"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=weihu") + "\">游戏维护</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=reset") + "\">游戏重置</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=set") + "\">游戏配置</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=back") + "\">返赢返负</a>");
        builder.Append(Out.Tab("</div>", ""));
        foot();
    }

    //盈利分析
    private void StatPage()
    {
        Master.Title = "" + GameName + "_赢利分析";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx") + "\">" + GameName + "</a>&gt;赢利分析");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("今天赢利:" + new BCW.Baccarat.BLL.BJL_Room().GetPrice("shouxufei", "DateDiff(dd,AddTime,getdate())=0") + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("昨日赢利:" + new BCW.Baccarat.BLL.BJL_Room().GetPrice("shouxufei", "DateDiff(dd,AddTime,getdate()-1)=0") + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("本月赢利:" + new BCW.Baccarat.BLL.BJL_Room().GetPrice("shouxufei", "DateDiff(month,AddTime,getdate())=0") + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("上月赢利:" + new BCW.Baccarat.BLL.BJL_Room().GetPrice("shouxufei", "DateDiff(month,AddTime,getdate())=1") + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", "<br />"));


        builder.Append(Out.Tab("<div>", ""));
        builder.Append("总计赢利:" + new BCW.Baccarat.BLL.BJL_Room().GetPrice("shouxufei", "") + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", ""));


        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        if (Utils.ToSChinese(ac) == "马上查询")
        {
            DateTime searchday1 = Utils.ParseTime(Utils.GetRequest("sTime", "all", 2, DT.RegexTime, "开始时间填写无效"));
            DateTime searchday2 = Utils.ParseTime(Utils.GetRequest("oTime", "all", 2, DT.RegexTime, "结束时间填写无效"));

            long dateTou = new BCW.Baccarat.BLL.BJL_Room().GetPrice("shouxufei", "AddTime>='" + searchday1 + "' and AddTime<='" + searchday2 + "'");

            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append("<b>盈利" + (dateTou) + "" + ub.Get("SiteBz") + ".</b>");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "开始日期：,结束日期：,";
            string strName = "sTime,oTime";
            string strType = "date,date";
            string strValu = "" + searchday1.ToString("yyyy-MM-dd HH:mm:ss") + "'" + searchday2.ToString("yyyy-MM-dd HH:mm:ss") + "''";
            string strEmpt = "false,false";
            string strIdea = "/";
            string strOthe = "马上查询,bjl.aspx?act=stat,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        else
        {
            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append("<b>盈利0" + ub.Get("SiteBz") + ".</b>");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "开始日期：,结束日期：,";
            string strName = "sTime,oTime";
            string strType = "date,date";
            string strValu = "" + DT.FormatDate(DateTime.Now.AddDays(-1), 0) + "'" + DT.FormatDate(DateTime.Now, 0) + "''";
            string strEmpt = "false,false";
            string strIdea = "/";
            string strOthe = "马上查询,bjl.aspx?act=stat,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("温馨提示:此盈利只算系统收取的手续费总和.");
        builder.Append(Out.Tab("</div>", ""));

        foot();
    }

    //游戏维护
    private void WeihuPage()
    {
        Master.Title = "" + GameName + "_游戏维护";
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/bjl.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置

        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string Status = Utils.GetRequest("Status", "post", 2, @"^[0-2]$", "0");
            string textID = Utils.GetRequest("textID", "all", 1, @"^[^\^]{1,2000}$", "请输入测试号.");//测试号
            xml.dss["Status"] = Status;
            xml.dss["textID"] = textID;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);

            Utils.Success("" + GameName + "_游戏维护", "设置成功，正在返回..", Utils.getUrl("bjl.aspx?act=weihu"), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;<a href=\"" + Utils.getUrl("bjl.aspx") + "\">" + GameName + "</a>&gt;游戏维护"));
            string strText = "游戏状态:/,测试ID：/,";
            string strName = "Status,textID,act";
            string strType = "select,big,hidden";
            string strValu = "" + xml.dss["Status"] + "'" + xml.dss["textID"] + "'weihu";
            string strEmpt = "0|正常|1|维护|2|内测,false,false";
            string strIdea = "/";
            string strOthe = "确定修改,bjl.aspx?act=weihu,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("温馨提示：多个测试ID请用#分隔.");
        builder.Append(Out.Tab("</div>", ""));

        foot();
    }

    //底部
    private void foot()
    {
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //重置游戏
    private void ResetPage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId != 1 && ManageId != 9)
        {
            //Utils.Error("权限不足", "");
        }

        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "11")
        {
            new BCW.Baccarat.BLL.BJL_Room().ClearTable("tb_BJL_Card");
            new BCW.Baccarat.BLL.BJL_Room().ClearTable("tb_BJL_Play");
            new BCW.Baccarat.BLL.BJL_Room().ClearTable("tb_BJL_Room");
            new BCW.Baccarat.BLL.BJL_Room().ClearTable("tb_BJL_user");
            SqlHelper.ExecuteSql("DELETE  FROM tb_Action WHERE Types=1036");//删除动态
            Utils.Success("重置游戏", "重置[所有数据]成功..", Utils.getUrl("bjl.aspx?act=reset"), "2");
        }
        else if (info == "1")
        {
            Master.Title = "重置所有表吗？";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定重置所有表吗？将会把所有数据清空哦.");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=reset&amp;info=11") + "\">确定重置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=reset") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        else if (info == "22")
        {
            new BCW.Baccarat.BLL.BJL_Room().ClearTable("tb_BJL_Card");
            Utils.Success("重置游戏", "重置[扑克表]成功..", Utils.getUrl("bjl.aspx?act=reset"), "2");
        }
        else if (info == "2")
        {
            Master.Title = "重置扑克表吗？";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定重置扑克表吗？将会把所有数据清空哦.");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=reset&amp;info=22") + "\">确定重置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=reset") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        else if (info == "33")
        {
            new BCW.Baccarat.BLL.BJL_Room().ClearTable("tb_BJL_Play");
            Utils.Success("重置游戏", "重置[下注表]成功..", Utils.getUrl("bjl.aspx?act=reset"), "2");
        }
        else if (info == "3")
        {
            Master.Title = "重置下注表吗？";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定重置下注表吗？将会把所有数据清空哦.");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=reset&amp;info=33") + "\">确定重置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=reset") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        else if (info == "44")
        {
            new BCW.Baccarat.BLL.BJL_Room().ClearTable("tb_BJL_Room");
            Utils.Success("重置游戏", "重置[房间表]成功..", Utils.getUrl("bjl.aspx?act=reset"), "2");
        }
        else if (info == "4")
        {
            Master.Title = "重置房间表吗？";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定重置房间表吗？将会把所有数据清空哦.");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=reset&amp;info=44") + "\">确定重置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=reset") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        else if (info == "55")
        {
            new BCW.Baccarat.BLL.BJL_Room().ClearTable("tb_BJL_user");
            Utils.Success("重置游戏", "重置[用户表]成功..", Utils.getUrl("bjl.aspx?act=reset"), "2");
        }
        else if (info == "5")
        {
            Master.Title = "重置用户表吗？";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定重置用户表吗？将会把所有数据清空哦.");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=reset&amp;info=55") + "\">确定重置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=reset") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        else if (info == "66")
        {
            SqlHelper.ExecuteSql("DELETE  FROM tb_Action WHERE Types=1036");//删除动态
            Utils.Success("清理玩家动态", "清理玩家动态成功..", Utils.getUrl("bjl.aspx?act=reset"), "2");
        }
        else if (info == "6")
        {
            Master.Title = "清理玩家动态吗？";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定清理玩家动态吗？将会把所有数据清空哦.");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=reset&amp;info=66") + "\">确定清理</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=reset") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        else
        {
            Master.Title = "" + GameName + "_重置游戏";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx") + "\">" + GameName + "</a>&gt;重置游戏");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?info=1&amp;act=reset") + "\">[<b>一键全部重置</b>]</a>");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?info=2&amp;act=reset") + "\">[单独重置-扑克表]</a>");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?info=3&amp;act=reset") + "\">[单独重置-下注表]</a>");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?info=4&amp;act=reset") + "\">[单独重置-房间表]</a>");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?info=5&amp;act=reset") + "\">[单独重置-用户表]</a>");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?info=6&amp;act=reset") + "\">[单独清理-玩家动态]</a>");
            builder.Append(Out.Tab("</div>", ""));
            foot();
        }
    }

    //系统配置
    private void SetPage()
    {
        Master.Title = "" + GameName + "_游戏配置";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx") + "\">" + GameName + "</a>&gt;游戏配置");
        builder.Append(Out.Tab("</div>", "<br/>"));

        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-1]$", "0"));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        string xmlPath = "/Controls/bjl.xml";
        Application.Remove(xmlPath);//清楚缓存
        xml.ReloadSub(xmlPath);//加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            if (ptype == 0)
            {
                string baccaratName = Utils.GetRequest("baccaratName", "post", 2, @"^[^\^]{1,20}$", "游戏名称限1-20字内");
                string baccarattop = Utils.GetRequest("baccarattop", "post", 3, @"^[\s\S]{1,2000}$", "头部Ubb限2000字内");
                string baccaratLogo = Utils.GetRequest("baccaratLogo", "post", 3, @"^[^\^]{1,200}$", "Logo地址限200字内");
                string baccaratLowerPay = Utils.GetRequest("baccaratLowerPay", "post", 2, @"^[0-9]\d*$", "最低彩池金额填写出错");
                string baccaratHigherPay = Utils.GetRequest("baccaratHigherPay", "post", 2, @"^[0-9]\d*$", "最高彩池金额填写出错");
                string baccaratRoomTime1 = Utils.GetRequest("baccaratRoomTime1", "post", 2, @"^[0-9]\d*$", "封庄最低局数填写错误");
                string baccaratRoomTime2 = Utils.GetRequest("baccaratRoomTime2", "post", 2, @"^[0-9]\d*$", "自动封庄局数填写错误");
                string BaccaratFoot = Utils.GetRequest("BaccaratFoot", "post", 3, @"^[\s\S]{1,2000}$", "底部Ubb限2000字内");
                string PokerTimes = Utils.GetRequest("PokerTimes", "post", 2, @"^(\d)*(\.(\d){1,2})?$", "发牌时间输入错误");
                string baccarat_img = Utils.GetRequest("baccarat_img", "post", 3, @"^[^\^]{1,200}$", "扑克图片地址限200字内");
                string shouxufei = Utils.GetRequest("shouxufei", "post", 2, @"^[0-9]\d*$", "手续费填写错误");
                string kaizhuang = Utils.GetRequest("kaizhuang", "post", 2, @"^[0-9]\d*$", "开庄限制填写错误");
                string SiteListNo = Utils.GetRequest("SiteListNo", "post", 2, @"^[0-9]\d*$", "分页条数填写错误");

                xml.dss["baccaratName"] = baccaratName;
                xml.dss["PokerTimes"] = PokerTimes;
                xml.dss["baccaratLogo"] = baccaratLogo;
                xml.dss["baccaratLowerPay"] = baccaratLowerPay;
                xml.dss["baccaratHigherPay"] = baccaratHigherPay;
                xml.dss["baccaratRoomTime1"] = baccaratRoomTime1;
                xml.dss["baccaratRoomTime2"] = baccaratRoomTime2;
                xml.dss["baccarattop"] = baccarattop;
                xml.dss["BaccaratFoot"] = BaccaratFoot;
                xml.dss["shouxufei"] = shouxufei;
                xml.dss["baccarat_img"] = baccarat_img;
                xml.dss["kaizhuang"] = kaizhuang;
                xml.dss["SiteListNo"] = SiteListNo;
            }
            else
            {
                string baccaratpercent1 = Utils.GetRequest("baccaratpercent1", "post", 2, @"^(\d)*(\.(\d){1,2})?$", "庄赢赔率输入错误");
                string baccaratpercent2 = Utils.GetRequest("baccaratpercent2", "post", 2, @"^(\d)*(\.(\d){1,2})?$", "闲赢赔率输入错误");
                string baccaratpercent3 = Utils.GetRequest("baccaratpercent3", "post", 2, @"^(\d)*(\.(\d){1,2})?$", "和局赔率输入错误");
                string baccaratpercent4 = Utils.GetRequest("baccaratpercent4", "post", 2, @"^(\d)*(\.(\d){1,2})?$", "庄单赔率输入错误");
                string baccaratpercent5 = Utils.GetRequest("baccaratpercent5", "post", 2, @"^(\d)*(\.(\d){1,2})?$", "庄双赔率输入错误");
                string baccaratpercent6 = Utils.GetRequest("baccaratpercent6", "post", 2, @"^(\d)*(\.(\d){1,2})?$", "闲单赔率输入错误");
                string baccaratpercent7 = Utils.GetRequest("baccaratpercent7", "post", 2, @"^(\d)*(\.(\d){1,2})?$", "闲双赔率输入错误");
                string baccaratpercent8 = Utils.GetRequest("baccaratpercent8", "post", 2, @"^(\d)*(\.(\d){1,2})?$", "投大赔率输入错误");
                string baccaratpercent9 = Utils.GetRequest("baccaratpercent9", "post", 2, @"^(\d)*(\.(\d){1,2})?$", "投小赔率输入错误");
                string baccaratpercent10 = Utils.GetRequest("baccaratpercent10", "post", 2, @"^(\d)*(\.(\d){1,2})?$", "庄对赔率输入错误");
                string baccaratpercent11 = Utils.GetRequest("baccaratpercent11", "post", 2, @"^(\d)*(\.(\d){1,2})?$", "闲对赔率输入错误");
                string baccaratpercent12 = Utils.GetRequest("baccaratpercent12", "post", 2, @"^(\d)*(\.(\d){1,2})?$", "任意对赔率输入错误");
                string baccaratpercent13 = Utils.GetRequest("baccaratpercent13", "post", 2, @"^(\d)*(\.(\d){1,2})?$", "完美对赔率输入错误");

                xml.dss["baccaratpercent1"] = baccaratpercent1;
                xml.dss["baccaratpercent2"] = baccaratpercent2;
                xml.dss["baccaratpercent3"] = baccaratpercent3;
                xml.dss["baccaratpercent4"] = baccaratpercent4;
                xml.dss["baccaratpercent5"] = baccaratpercent5;
                xml.dss["baccaratpercent6"] = baccaratpercent6;
                xml.dss["baccaratpercent7"] = baccaratpercent7;
                xml.dss["baccaratpercent8"] = baccaratpercent8;
                xml.dss["baccaratpercent9"] = baccaratpercent9;
                xml.dss["baccaratpercent10"] = baccaratpercent10;
                xml.dss["baccaratpercent11"] = baccaratpercent11;
                xml.dss["baccaratpercent12"] = baccaratpercent12;
                xml.dss["baccaratpercent13"] = baccaratpercent13;
            }
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("系统设置", "设置成功，正在返回..", Utils.getUrl("bjl.aspx?act=set&amp;ptype=" + ptype + ""), "1");
        }
        else
        {
            if (ptype == 0)
            {
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("系统设置|");
                builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=set&amp;ptype=1") + "\">赔率设置</a>");
                builder.Append(Out.Tab("</div>", ""));
                string strText = "游戏名称：/,每人最大开庄数：/,游戏Logo路径：/,每局发牌时间(秒)：/,系统收取手续费：(%)0不收取/,开庄金额最低要求：/,开庄金额最高上限：/,允许封庄局数最低为：/,达到多少自动封庄局数为：/,派牌图片的路径：/,分页数目：/,头部Ubb：/,底部Ubb：/,";
                string strName = "baccaratName,kaizhuang,baccaratLogo,PokerTimes,shouxufei,baccaratLowerPay,baccaratHigherPay,baccaratRoomTime1,baccaratRoomTime2,baccarat_img,SiteListNo,baccarattop,BaccaratFoot,backurl";
                string strType = "text,text,text,text,text,num,num,num,num,text,text,textarea,textarea,hidden";
                string strValu = "" + xml.dss["baccaratName"] + "'" + xml.dss["kaizhuang"] + "'" + xml.dss["baccaratLogo"] + "'" + xml.dss["PokerTimes"] + "'" + xml.dss["shouxufei"] + "'" + xml.dss["baccaratLowerPay"] + "'" + xml.dss["baccaratHigherPay"] + "'" + xml.dss["baccaratRoomTime1"] + "'" + xml.dss["baccaratRoomTime2"] + "'" + xml.dss["baccarat_img"] + "'" + xml.dss["SiteListNo"] + "'" + xml.dss["baccarattop"] + "'" + xml.dss["BaccaratFoot"] + "'" + Utils.getPage(0) + "";
                string strEmpt = "false,false,false,false,false,true,false,false,false,false,false,false,true,false";
                string strIdea = "/";
                string strOthe = "确定修改,bjl.aspx?act=set&amp;ptype=0,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
            else
            {
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=set&amp;ptype=0") + "\">系统设置</a>");
                builder.Append("|赔率设置");
                builder.Append(Out.Tab("</div>", ""));
                string strText = "庄赢：/,闲赢：/,和局：/,庄单：/,庄双：/,闲单：/,闲双：/,投大：/,投小：/,庄对：/,闲对：/,任意对：/,完美对：/";
                string strName = "baccaratpercent1,baccaratpercent2,baccaratpercent3,baccaratpercent4,baccaratpercent5,baccaratpercent6,baccaratpercent7,baccaratpercent8,baccaratpercent9,baccaratpercent10,baccaratpercent11,baccaratpercent12,baccaratpercent13";
                string strType = "text,text,text,text,text,text,text,text,text,text,text,text,text";
                string strValu = "" + xml.dss["baccaratpercent1"] + "'" + xml.dss["baccaratpercent2"] + "'" + xml.dss["baccaratpercent3"] + "'" + xml.dss["baccaratpercent4"] + "'" + xml.dss["baccaratpercent5"] + "'" + xml.dss["baccaratpercent6"] + "'" + xml.dss["baccaratpercent7"] + "'" + xml.dss["baccaratpercent8"] + "'" + xml.dss["baccaratpercent9"] + "'" + xml.dss["baccaratpercent10"] + "'" + xml.dss["baccaratpercent11"] + "'" + xml.dss["baccaratpercent12"] + "'" + xml.dss["baccaratpercent13"] + "";
                string strEmpt = "false,false,false,false,false,false,false,false,false,false,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确定修改,bjl.aspx?act=set&amp;ptype=1,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
            foot();
        }

    }

    //排行榜
    private void TopPage()
    {
        Master.Title = "" + GameName + "_用户排行";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx") + "\">" + GameName + "</a>&gt;用户排行");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[1-4]$", "1"));
        if (ptype == 1)
        {
            builder.Append("<h style=\"color:red\">赚币排行" + "</h>" + "|");
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=top&amp;ptype=2") + "\">胜率排行</a>" + "|");
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=top&amp;ptype=3") + "\">次数排行</a>" + "|");
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=top&amp;ptype=4") + "\">庄家盈利</a>" + "");
        }
        else if (ptype == 2)
        {
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=top&amp;ptype=1") + "\">赚币排行</a>" + "|");
            builder.Append("<h style=\"color:red\">胜率排行" + "</h>" + "|");
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=top&amp;ptype=3") + "\">次数排行</a>" + "|");
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=top&amp;ptype=4") + "\">庄家盈利</a>" + "");
        }
        else if (ptype == 3)
        {
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=top&amp;ptype=1") + "\">赚币排行</a>" + "|");
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=top&amp;ptype=2") + "\">胜率排行</a>" + "|");
            builder.Append("<h style=\"color:red\">次数排行" + "</h>" + "|");
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=top&amp;ptype=4") + "\">庄家盈利</a>" + "");
        }
        else if (ptype == 4)
        {
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=top&amp;ptype=1") + "\">赚币排行</a>" + "|");
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=top&amp;ptype=2") + "\">胜率排行</a>" + "|");
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=top&amp;ptype=3") + "\">次数排行</a>" + "|");
            builder.Append("<h style=\"color:red\">庄家盈利" + "</h>" + "");
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        DateTime startstate = Utils.ParseTime(Utils.GetRequest("startstate2", "all", 1, DT.RegexTime, DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd HH:mm:ss")));
        DateTime endstate = Utils.ParseTime(Utils.GetRequest("endstate2", "all", 1, DT.RegexTime, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");

        int recordCount;
        string strWhere = string.Empty;
        string strWhere2 = string.Empty;
        string strWhere3 = string.Empty;
        string strWhere4 = string.Empty;
        int pageSize = 10;
        string[] pageValUrl = { "act", "startstate2", "endstate2", "ptype", "backurl" };
        int pageIndex = 1;
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        string rewardid = "";
        if (ptype == 1)
        {
            #region
            if (Utils.ToSChinese(ac) == "马上查询")
                strWhere = "updatetime>='" + startstate + "' and updatetime<='" + endstate + "' and type>0 GROUP BY buy_usid ORDER BY Sum(GetMoney-PutMoney) DESC";
            else
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
                    builder.Append("[<h style=\"color:red\">第" + wd + "名</h>]<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + usid + "") + "\">" + mename + "</a>净赢<h style=\"color:red\">" + usmoney + "</h>" + ub.Get("SiteBz") + "");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                    rewardid = rewardid + usid.ToString() + "#";
                }
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录.."));
            }
            #endregion
        }
        else if (ptype == 2)
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
                    builder.Append("[<h style=\"color:red\">第" + ((pageIndex - 1) * pageSize + k) + "名</h>]<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + usid + "") + "\">" + new BCW.BLL.User().GetUsName(usid) + "</a>胜<h style=\"color:red\">" + bb + "</h>次");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录.."));
            }
            #endregion
        }
        else if (ptype == 3)
        {
            #region
            DataSet cishu = new BCW.Baccarat.BLL.BJL_Play().GetList("buy_usid,COUNT(buy_usid)as aa", "BankerPoint!='' GROUP BY buy_usid ORDER BY aa DESC");
            if (cishu != null && cishu.Tables[0].Rows.Count > 0)
            {
                recordCount = cishu.Tables[0].Rows.Count;
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
                    int usid = int.Parse(cishu.Tables[0].Rows[koo + i]["buy_usid"].ToString());
                    int bb = int.Parse(cishu.Tables[0].Rows[koo + i]["aa"].ToString());


                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }
                    builder.Append("[<h style=\"color:red\">第" + ((pageIndex - 1) * pageSize + k) + "名</h>]<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + usid + "") + "\">" + new BCW.BLL.User().GetUsName(usid) + "</a>总<h style=\"color:red\">" + bb + "</h>次");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
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
            DataSet cishu = new BCW.Baccarat.BLL.BJL_Room().GetList("UsID,SUM(Total_Now - Total)as aa", "Total!='' GROUP BY UsID ORDER BY aa DESC");
            if (cishu != null && cishu.Tables[0].Rows.Count > 0)
            {
                recordCount = cishu.Tables[0].Rows.Count;
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
                    int usid = int.Parse(cishu.Tables[0].Rows[koo + i]["UsID"].ToString());
                    int bb = int.Parse(cishu.Tables[0].Rows[koo + i]["aa"].ToString());

                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }
                    builder.Append("[<h style=\"color:red\">第" + ((pageIndex - 1) * pageSize + k) + "名</h>]<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + usid + "") + "\">" + new BCW.BLL.User().GetUsName(usid) + "</a>赚币:<h style=\"color:red\">" + bb + "</h>");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录.."));
            }
            #endregion
        }

        if (ptype == 1)
        {
            string strText = "开始日期：/,结束日期：/";
            string strName = "startstate2,endstate2";
            string strType = "date,date";
            string strValu = "" + DT.FormatDate(startstate, 0) + "'" + DT.FormatDate(endstate, 0) + "";
            string strEmpt = "false,false";
            string strIdea = "/";
            string strOthe = "马上查询,bjl.aspx?act=top&amp;ptype=" + ptype + ",post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            if (Utils.ToSChinese(ac) != "马上查询")
            {
                builder.Append(Out.Tab("<div>", Out.Hr()));
                builder.Append("排行榜奖励提示：<br/>");
                builder.Append("如需发放奖励，请按日期查询.");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                string wdy = "";
                if (pageIndex == 1)
                    wdy = "TOP10";
                else
                    wdy = "TOP" + (pageIndex - 1).ToString() + "1-" + pageIndex.ToString() + "0";

                builder.Append(Out.Tab("<div>", Out.Hr()));
                builder.Append(wdy + " 的用户ID分别是：" + rewardid);
                builder.Append(Out.Tab("</div>", ""));
                string strText2 = ",,,,";
                string strName2 = "startstate,endstate,pageIndex,rewardid,backurl";
                string strType2 = "hidden,hidden,hidden,hidden,hidden";
                string strValu2 = DT.FormatDate(startstate, 0) + "'" + DT.FormatDate(endstate, 0) + "'" + pageIndex + "'" + rewardid + "'" + Utils.getPage(0) + "";
                string strEmpt2 = "true,true,false";
                string strIdea2 = "/";
                string strOthe2 = wdy + "奖励发放,bjl.aspx?act=ReWard&amp;ptype=" + ptype + ",post,1,red";
                builder.Append(Out.wapform(strText2, strName2, strType2, strValu2, strEmpt2, strIdea2, strOthe2));
            }
        }



        foot();
    }


    //排行榜奖励发放
    private void ReWard()
    {
        Master.Title = "" + GameName + "_奖励发放";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("bjl.aspx?act=top") + "\">用户排行</a>&gt;奖励发放");
        builder.Append(Out.Tab("</div>", "<br />"));


        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));
        DateTime startstate = Utils.ParseTime(Utils.GetRequest("startstate", "all", 2, DT.RegexTime, "开始时间填写无效"));
        DateTime endstate = Utils.ParseTime(Utils.GetRequest("endstate", "all", 2, DT.RegexTime, "结束时间填写无效"));
        int pageIndex = int.Parse(Utils.GetRequest("pageIndex", "all", 1, @"^[1-9]\d*$", "1"));
        string rewardid = Utils.GetRequest("rewardid", "post", 2, @"^[\s\S]{1,500}$", "当前页无可操作用户");

        string wdy = "";
        if (pageIndex == 1)
            wdy = "TOP10";
        else
            wdy = "TOP" + (pageIndex - 1).ToString() + "1-" + pageIndex.ToString() + "0";
        builder.Append(Out.Tab("<div>", ""));
        switch (ptype)
        {
            case 1:
                builder.Append("《赚币排行》" + wdy + "奖励发放：");
                break;
        }
        builder.Append(Out.Tab("</div>", ""));

        int mzj = (pageIndex - 1) * 10;
        string[] IdRe = rewardid.Split('#');
        try
        {
            string strText2 = ",,,,TOP" + (mzj + 1) + "：" + IdRe[0] + "&nbsp;&nbsp;,,TOP" + (mzj + 2) + "：" + IdRe[1] + "&nbsp;&nbsp;,,TOP" + (mzj + 3) + "：" + IdRe[2] + "&nbsp;&nbsp;,,TOP" + (mzj + 4) + "：" + IdRe[3] + "&nbsp;&nbsp;,,TOP" + (mzj + 5) + "：" + IdRe[4] + "&nbsp;&nbsp;,,TOP" + (mzj + 6) + "：" + IdRe[5] + "&nbsp;&nbsp;,,TOP" + (mzj + 7) + "：" + IdRe[6] + "&nbsp;&nbsp;,,TOP" + (mzj + 8) + "：" + IdRe[7] + "&nbsp;&nbsp;,,TOP" + (mzj + 9) + "：" + IdRe[8] + "&nbsp;&nbsp;,,TOP" + pageIndex * 10 + "：" + IdRe[9] + "&nbsp;&nbsp;,";
            string strName2 = "act,ptype,startstate,endstate,top1,IdRe1,top2,IdRe2,top3,IdRe3,top4,IdRe4,top5,IdRe5,top6,IdRe6,top7,IdRe7,top8,IdRe8,top9,IdRe9,top10,IdRe10";
            string strType2 = "hidden,hidden,hidden,hidden,num,hidden,num,hidden,num,hidden,num,hidden,num,hidden,num,hidden,num,hidden,num,hidden,num,hidden,num,hidden";
            string strValu2 = "ReWardCase'" + ptype + "'" + DT.FormatDate(startstate, 0) + "'" + DT.FormatDate(endstate, 0) + "'" + "0'" + IdRe[0] + "'0'" + IdRe[1] + "'0'" + IdRe[2] + "'0'" + IdRe[3] + "'0'" + IdRe[4] + "'0'" + IdRe[5] + "'0'" + IdRe[6] + "'0'" + IdRe[7] + "'0'" + IdRe[8] + "'0'" + IdRe[9] + "'0";
            string strEmpt2 = "false,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true";
            string strIdea2 = "/";
            string strOthe2 = "提交,bjl.aspx,post,1,red";
            builder.Append(Out.wapform(strText2, strName2, strType2, strValu2, strEmpt2, strIdea2, strOthe2));
        }
        catch
        {
            builder.Append(Out.Tab("<div>", "<br/>"));
            builder.Append("当页少于10人，无法发放！");
            builder.Append(Out.Tab("</div>", ""));

        }
        builder.Append(Out.Tab("</div>", Out.Hr()));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("bjl.aspx?act=top") + "\">&lt;&lt;&lt;返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //排行榜奖励发放--界面
    private void ReWardCase()
    {
        int[] IdRe = new int[11];
        long[] Top = new long[11];
        IdRe[1] = int.Parse(Utils.GetRequest("IdRe1", "post", 1, "", "10086"));
        IdRe[2] = int.Parse(Utils.GetRequest("IdRe2", "post", 1, "", "10086"));
        IdRe[3] = int.Parse(Utils.GetRequest("IdRe3", "post", 1, "", "10086"));
        IdRe[4] = int.Parse(Utils.GetRequest("IdRe4", "post", 1, "", "10086"));
        IdRe[5] = int.Parse(Utils.GetRequest("IdRe5", "post", 1, "", "10086"));
        IdRe[6] = int.Parse(Utils.GetRequest("IdRe6", "post", 1, "", "10086"));
        IdRe[7] = int.Parse(Utils.GetRequest("IdRe7", "post", 1, "", "10086"));
        IdRe[8] = int.Parse(Utils.GetRequest("IdRe8", "post", 1, "", "10086"));
        IdRe[9] = int.Parse(Utils.GetRequest("IdRe9", "post", 1, "", "10086"));
        IdRe[10] = int.Parse(Utils.GetRequest("IdRe10", "post", 1, "", "10086"));
        Top[1] = Convert.ToInt64(Utils.GetRequest("top1", "post", 1, "", ""));
        Top[2] = Convert.ToInt64(Utils.GetRequest("top2", "post", 1, "", ""));
        Top[3] = Convert.ToInt64(Utils.GetRequest("top3", "post", 1, "", ""));
        Top[4] = Convert.ToInt64(Utils.GetRequest("top4", "post", 1, "", ""));
        Top[5] = Convert.ToInt64(Utils.GetRequest("top5", "post", 1, "", ""));
        Top[6] = Convert.ToInt64(Utils.GetRequest("top6", "post", 1, "", ""));
        Top[7] = Convert.ToInt64(Utils.GetRequest("top7", "post", 1, "", ""));
        Top[8] = Convert.ToInt64(Utils.GetRequest("top8", "post", 1, "", ""));
        Top[9] = Convert.ToInt64(Utils.GetRequest("top9", "post", 1, "", ""));
        Top[10] = Convert.ToInt64(Utils.GetRequest("top10", "post", 1, "", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));
        DateTime startstate = Utils.ParseTime(Utils.GetRequest("startstate", "all", 2, DT.RegexTime, "开始时间填写无效"));
        DateTime endstate = Utils.ParseTime(Utils.GetRequest("endstate", "all", 2, DT.RegexTime, "结束时间填写无效"));

        string wdy = "";
        switch (ptype)
        {
            case 1:
                wdy = "赚币排行榜";
                break;
        }
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        if (Utils.ToSChinese(ac) == "确定发放")
        {
            for (int i = 1; i <= 10; i++)
            {
                if (Top[i] != 0)
                {
                    new BCW.BLL.User().UpdateiGold(IdRe[i], Top[i], "" + GameName + "排行榜奖励");
                    //发内线
                    string strLog = "您在 " + startstate + " 至 " + endstate + " 里在游戏《" + GameName + "》" + wdy + "上取得了第" + i + "名的好成绩，系统奖励了" + Top[i] + "" + ub.Get("SiteBz") + "[url=/bbs/game/bjl.aspx]进入《" + GameName + "》[/url]";
                    new BCW.BLL.Guest().Add(0, IdRe[i], new BCW.BLL.User().GetUsName(IdRe[i]), strLog);
                    //动态
                    string mename = new BCW.BLL.User().GetUsName(IdRe[i]);
                    string wText = "[url=/bbs/uinfo.aspx?uid=" + IdRe[i] + "]" + mename + "[/url]在[url=/bbs/game/bjl.aspx]《" + GameName + "》[/url]" + wdy + "上取得了第" + i + "名的好成绩，系统奖励了" + Top[i] + "" + ub.Get("SiteBz");
                    new BCW.BLL.Action().Add(1001, 0, IdRe[i], "", wText);
                }
            }
            Utils.Success("奖励操作", "奖励操作成功", Utils.getUrl("bjl.aspx?act=top"), "1");
        }
        else
        {
            Master.Title = "" + GameName + "_奖励发放";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("bjl.aspx?act=top") + "\">用户排行</a>&gt;奖励发放");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("正在发放《" + wdy + "》奖励：");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("时间从：" + startstate + "到" + endstate + "<br/>");
            for (int j = 1; j <= 10; j++)
            {
                if (j == 10)
                {
                    builder.Append("TOP" + j + "：" + IdRe[j] + ".[" + Top[j] + "" + ub.Get("SiteBz") + "]");
                }
                else
                {
                    builder.Append("TOP" + j + "：" + IdRe[j] + ".[" + Top[j] + "" + ub.Get("SiteBz") + "]<br/>");
                }
            }

            string strText2 = ",,,,,,,,,,,,,,,,,,,,,,,";
            string strName2 = "act,ptype,startstate,endstate,top1,IdRe1,top2,IdRe2,top3,IdRe3,top4,IdRe4,top5,IdRe5,top6,IdRe6,top7,IdRe7,top8,IdRe8,top9,IdRe9,top10,IdRe10";
            string strType2 = "hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden";
            string strValu2 = "ReWardCase'" + ptype + "'" + DT.FormatDate(startstate, 0) + "'" + DT.FormatDate(endstate, 0) + "'" + Top[1] + "'" + IdRe[1] + "'" + Top[2] + "'" + IdRe[2] + "'" + Top[3] + "'" + IdRe[3] + "'" + Top[4] + "'" + IdRe[4] + "'" + Top[5] + "'" + IdRe[5] + "'" + Top[6] + "'" + IdRe[6] + "'" + Top[7] + "'" + IdRe[7] + "'" + Top[8] + "'" + IdRe[8] + "'" + Top[9] + "'" + IdRe[9] + "'" + Top[10] + "'" + IdRe[10];
            string strEmpt2 = "false,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true";
            string strIdea2 = "/";
            string strOthe2 = "确定发放,bjl.aspx,post,1,red";
            builder.Append(Out.wapform(strText2, strName2, strType2, strValu2, strEmpt2, strIdea2, strOthe2));

            builder.Append(Out.Tab("</div>", ""));


            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getPage("bjl.aspx?act=top") + "\">&lt;&lt;再看看吧</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }


    }

    //下注记录
    private void xiazhuPage()
    {
        Master.Title = "" + GameName + "_用户下注";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx") + "\">" + GameName + "</a>&gt;用户下注");
        builder.Append(Out.Tab("</div>", "<br />"));

        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        int roomid = int.Parse(Utils.GetRequest("roomid", "all", 1, @"^[1-9]\d*$", "0"));
        int isRobot = int.Parse(Utils.GetRequest("isRobot", "all", 1, @"^[0-1]$", "0"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
        string strWhere = "";
        string[] pageValUrl = { "act", "uid", "ptype", "roomid", "isRobot", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 1)
        {
            builder.Append("<h style=\"color:red\">下注" + "</h>" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=xiazhu&amp;ptype=1&amp;uid=" + uid + "&amp;roomid=" + roomid + "&amp;isRobot=" + isRobot + "") + "\">下注</a>" + "|");
        }
        if (ptype == 2)
        {
            builder.Append("<h style=\"color:red\">中奖" + "</h>" + "");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=xiazhu&amp;ptype=2&amp;uid=" + uid + "&amp;roomid=" + roomid + "&amp;isRobot=" + isRobot + "") + "\">中奖</a>" + "");
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        strWhere = "isRobot=" + isRobot + "";
        if (uid > 0)
        {
            if (roomid > 0)
                strWhere += "and RoomID=" + roomid + " and buy_usid=" + uid + "";
            else
                strWhere += "and buy_usid=" + uid + "";
        }
        else
        {
            if (roomid > 0)
                strWhere += "and RoomID=" + roomid + "";
        }
        string strOrder = "ID desc";

        if (ptype == 2)
        {
            strWhere += "and (type=2 or type=3)";
        }

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
                builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=del&amp;id=" + n.ID + "") + "\">[退]</a>" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.buy_usid + "") + "\">" + new BCW.BLL.User().GetUsName(n.buy_usid) + "</a>在第" + n.Play_Table + "局:押" + name1 + "");
                try
                {
                    builder.Append("(庄:" + allpoker(n.BankerPoker) + "(" + n.BankerPoint + "点)闲:" + allpoker(n.HunterPoker) + "(" + n.HunterPoint + "点))<br/>");
                }
                catch
                {
                    builder.Append("(庄:?(" + n.BankerPoint + "点)闲:?(" + n.HunterPoint + "点))<br/>");
                    //new BCW.BLL.Guest().Add(1, 52784, "森林仔666", "" + GameName + "在" + n.UsID + "的第" + n.RoomID + "桌第" + n.Play_Table + "局ID:" + n.buy_usid + "开奖日志出现异常.");
                }
                builder.Append("共投" + n.PutMoney + ".");//每注" + n.zhu_money + "
                if (n.GetMoney > 0)
                {
                    builder.Append("中奖" + n.GetMoney + ",");
                }
                builder.Append("结" + n.Total + ".[" + DT.FormatDate(n.updatetime, 1) + "]");
                if (n.type == 2)
                    builder.Append("<h style=\"color:red\">(未领)</h>");
                if (n.type == 3)
                    builder.Append("<h style=\"color:red\">(已领)</h>");
                builder.Append(Out.Tab("</div>", ""));
                k++;
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }

        string strText = "用户ID(可为空):/,桌面(可为空):/,机器人投注显示：/";
        string strName = "uid,roomid,isRobot";
        string strType = "num,num,select";
        string strValu = string.Empty;
        if (uid == 0)
        {
            if (roomid == 0)
                strValu = "''" + isRobot + "";
            else
                strValu = "'" + roomid + "'" + isRobot + "";
        }
        else
        {
            if (roomid == 0)
                strValu = "" + uid + "''" + isRobot + "";
            else
                strValu = "" + uid + "'" + roomid + "'" + isRobot + "";
        }
        string strEmpt = "true,true,0|关|1|开";
        string strIdea = "/";
        string strOthe = "搜一搜,bjl.aspx?act=xiazhu&amp;ptype=" + ptype + ",post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        foot();
    }

    //删除某一下注
    private void delPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "")
        {
            int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));

            Master.Title = "删除一条投注记录";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除该记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?info=ok&amp;act=del&amp;id=" + id + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=xiazhu") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            int id2 = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
            if (!new BCW.Baccarat.BLL.BJL_Play().Exists_id(id2))
            {
                Utils.Error("不存在的记录", "");
            }
            else
            {
                BCW.Baccarat.Model.BJL_Play model = new BCW.Baccarat.BLL.BJL_Play().GetBJL_Play2(id2);
                int meid = model.buy_usid;
                string mename = new BCW.BLL.User().GetUsName(meid);
                int state_get = model.type;//用户购买情况
                long Price = 0;
                //如果未开奖，退回本金
                if (state_get == 0 || state_get == 1 || state_get == 2)
                {
                    Price = model.PutMoney;
                    string yy = string.Empty;
                    if (state_get == 0)
                        yy = "未开奖";
                    if (state_get == 1)
                        yy = "不中奖";
                    new BCW.BLL.User().UpdateiGold(model.buy_usid, Price, "系统退回" + GameName + "第" + model.RoomID + "桌第" + model.Play_Table + "局" + yy + "的" + model.PutMoney + "" + ub.Get("SiteBz") + "！");
                    new BCW.BLL.Guest().Add(1, meid, mename, "系统退回您的" + GameName + "：第" + model.RoomID + "桌第" + model.Play_Table + "局" + yy + "的" + model.PutMoney + "" + ub.Get("SiteBz") + "！");
                    //减少彩池
                    new BCW.Baccarat.BLL.BJL_Room().update_zd("Total_Now=Total_Now-" + Price + "", "ID=" + model.RoomID + "");
                }
                else
                {
                    long gold = 0;//个人酷币
                    long cMoney = 0;//差多少
                    long sMoney = 0;//实扣
                    string ui = string.Empty;
                    gold = new BCW.BLL.User().GetGold(model.buy_usid);//个人酷币
                    if (model.GetMoney > gold)
                    {
                        cMoney = model.GetMoney - gold + model.PutMoney;
                        sMoney = model.GetMoney;
                    }
                    else
                    {
                        sMoney = model.GetMoney;
                    }

                    //如果币不够扣则记录日志并冻结IsFreeze
                    if (cMoney > 0)
                    {
                        BCW.Model.Gameowe owe = new BCW.Model.Gameowe();
                        owe.Types = 1;
                        owe.UsID = model.buy_usid;
                        owe.UsName = mename;
                        owe.Content = "" + GameName + "第" + model.RoomID + "桌第" + model.Play_Table + "局下注" + model.PutMoney + "" + ub.Get("SiteBz") + ".系统管理员" + new BCW.User.Manage().IsManageLogin() + "删除.";
                        owe.OweCent = cMoney;
                        owe.BzType = 11;
                        owe.EnId = model.ID;
                        owe.AddTime = DateTime.Now;
                        new BCW.BLL.Gameowe().Add(owe);
                        new BCW.BLL.User().UpdateIsFreeze(model.buy_usid, 1);
                        ui = "实扣" + sMoney + ",还差" + (cMoney) + ",系统已自动将您帐户冻结.";
                    }
                    string oop = string.Empty;
                    if (model.GetMoney > 0)
                    {
                        oop = "并扣除所得的" + model.GetMoney + "。";
                    }
                    if (state_get == 2)
                    {
                        new BCW.BLL.User().UpdateiGold(model.buy_usid, model.PutMoney, "无效购奖或非法操作，系统退回" + GameName + "第" + model.RoomID + "桌第" + model.Play_Table + "局下注的" + model.PutMoney + "" + ub.Get("SiteBz") + "." + ui);
                        new BCW.BLL.Guest().Add(1, meid, mename, "无效购奖或非法操作，系统退回" + GameName + "第" + model.RoomID + "桌第" + model.Play_Table + "局下注的" + model.PutMoney + "" + ub.Get("SiteBz") + "." + ui);
                    }

                    else
                    {
                        new BCW.BLL.User().UpdateiGold(model.buy_usid, model.PutMoney - model.GetMoney, "无效购奖或非法操作，系统退回" + GameName + "第" + model.RoomID + "桌第" + model.Play_Table + "局下注的" + model.PutMoney + "" + ub.Get("SiteBz") + "." + oop + "" + ui);
                        new BCW.BLL.Guest().Add(1, meid, mename, "无效购奖或非法操作，系统退回" + GameName + "第" + model.RoomID + "桌第" + model.Play_Table + "局下注的" + model.PutMoney + "" + ub.Get("SiteBz") + "." + oop + "" + ui);
                    }
                    //增加彩池
                    new BCW.Baccarat.BLL.BJL_Room().update_zd("Total_Now=Total_Now-" + (model.PutMoney) + "+" + (model.GetMoney) + "", "ID=" + model.RoomID + "");
                }
                new BCW.Baccarat.BLL.BJL_Play().Delete(id2);
                Utils.Success("删除投注记录", "删除记录成功..", Utils.getPage("bjl.aspx?act=xiazhu"), "2");
            }
        }
    }

    //房间
    private void fangjianPage()
    {
        int roomid = Utils.ParseInt(Utils.GetRequest("roomid", "get", 1, @"^[1-9]\d*$", "0"));//房间ID
        BCW.Baccarat.Model.BJL_Play buy = new BCW.Baccarat.BLL.BJL_Play().GetBJL_Play(roomid);//得到最后一桌的信息

        int Play_Table = 0;
        BCW.Baccarat.Model.BJL_Room play = new BCW.Baccarat.BLL.BJL_Room().GetBJL_Room(roomid);
        if (play.ID == 0)
        {
            Utils.Error("该房间不存在.", "");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        Master.Title = new BCW.BLL.User().GetUsName(play.UsID) + "的(" + play.ID + ")号桌";
        builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx") + "\">" + GameName + "</a>&gt;" + new BCW.BLL.User().GetUsName(play.UsID) + "的房间");
        builder.Append(Out.Tab("</div>", "<br />"));


        builder.Append(Out.Tab("<div>", ""));
        builder.Append("庄家：<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + play.UsID + "") + "\">" + new BCW.BLL.User().GetUsName(play.UsID) + "(" + play.UsID + ")</a><br />");
        if (play.Title == "")
        {
            builder.Append("庄家公告：无");
        }
        else
            builder.Append("庄家公告：<a href=\"" + Utils.getUrl("bjl.aspx?act=ggshow&amp;roomid=" + play.ID + "") + "\">" + play.Title + "</a>");
        builder.Append(Out.Tab("</div>", "<br/>"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=gonggao&amp;roomid=" + play.ID + "&amp;uid=" + play.UsID + "") + "\" >公告</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=diary&amp;roomid=" + play.ID + "&amp;uid=" + play.UsID + "") + "\">下注</a>.");
        if (play.state == 0)
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=fzhuang&amp;roomid=" + play.ID + "&amp;uid=" + play.UsID + "") + "\">封庄</a>.");
        if (play.state == 1)
            builder.Append("已封庄.");
        builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=setroom&amp;roomid=" + play.ID + "") + "\">设置</a><br/>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("开始彩池：" + (play.Total) + "" + ub.Get("SiteBz") + "<br/>");
        if (play.state == 0)
            builder.Append("实时彩池：<h style=\"color:red\">" + (play.Total_Now) + "</h>" + ub.Get("SiteBz") + "<br />");
        if (play.state == 1)
            builder.Append("封庄彩池：" + (play.Total_Now) + "" + ub.Get("SiteBz") + "<br />");
        builder.Append("追加彩池：" + play.zhui_Total + ub.Get("SiteBz") + "<br/>");
        builder.Append("桌面局数：第" + play.ID + "桌,");
        if (buy.Play_Table == 0)
        {
            Play_Table = 1;
        }
        else
        {
            Play_Table = buy.Play_Table + 1;
        }
        if (play.state == 0)
            builder.Append("第" + Play_Table + "局");
        if (play.state == 1)
            builder.Append("共" + (buy.Play_Table) + "局");
        builder.Append("[<a href=\"" + Utils.getUrl("bjl.aspx?act=fangjian&amp;roomid=" + roomid + "") + "\">刷新</a>]<br />");
        builder.Append("投注限额：最低" + play.LowTotal + "" + ub.Get("SiteBz") + ",最高" + play.BigPay + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", "<br/>"));

        DateTime Oldbettime = new BCW.Baccarat.BLL.BJL_Play().GetMinBetTime(roomid, (Play_Table));
        if (new BCW.Baccarat.BLL.BJL_Play().Exists(roomid, (Play_Table)))
        {
            BCW.Baccarat.Model.BJL_Card card = new BCW.Baccarat.BLL.BJL_Card().GetCardMessage(roomid, Play_Table);
            long sum = 0;//下注金额计算
            sum = new BCW.Baccarat.BLL.BJL_Play().GetPrice(roomid, Play_Table);
            builder.Append(Out.Tab("<div>", ""));
            if (Oldbettime.AddSeconds(Times) > DateTime.Now)
            {
                string daojishi = new BCW.JS.somejs().newDaojishi("divhp3sy6", Oldbettime.AddSeconds(Times));
                builder.Append("当局已下注<a href=\"" + Utils.getUrl("bjl.aspx?act=tablelist&amp;roomid=" + roomid + "&amp;table=" + Play_Table + "") + "\">" + sum + "" + ub.Get("SiteBz") + "</a>," + daojishi + "后发牌<br/>");
            }
        }

        history1(roomid);//历史数据
        danjufx(roomid);//单桌分析

        foot();
    }


    //设置
    private void setroomPage()
    {
        int roomid = Utils.ParseInt(Utils.GetRequest("roomid", "all", 1, @"^[1-9]\d*$", ""));//房间ID

        BCW.Baccarat.Model.BJL_Room play = new BCW.Baccarat.BLL.BJL_Room().GetBJL_Room(roomid);
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
            string strText = "最高下注：/,最低下注：/,,,";
            string strName = "BigPay,LowTotal,roomid,act,info";
            string strType = "text,text,hidden,hidden,hidden";
            string strValu = "" + play.BigPay + "'" + play.LowTotal + "'" + roomid + "'setroom'add";
            string strEmpt = "true,true,false,false,false";
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

            if (LowTotal > BigPay)
            {
                Utils.Error("抱歉,最低下注不可以高于最高下注,请重新输入.", "");
            }
            if (BigPay > play.Total_Now / 10)
            {
                Utils.Error("抱歉,修改后的最高下注不可以超过实时奖池的百分之十,请重新输入.", "");
            }
            int roomid1 = Utils.ParseInt(Utils.GetRequest("roomid", "all", 2, @"^[0-9]\d*$", "房间号码错误"));

            new BCW.Baccarat.BLL.BJL_Room().update_zd("BigPay=" + BigPay + ",LowTotal=" + LowTotal + "", "ID=" + roomid1 + "");
            Utils.Success("修改设置", "恭喜您，设置成功.", Utils.getUrl("bjl.aspx?act=fangjian&amp;roomid=" + roomid1 + ""), "1");
        }
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
                builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=tablelist&amp;roomid=" + n.RoomID + "&amp;table=" + n.RoomTable + "") + "\">第" + n.RoomTable + "局</a>:");
                builder.Append("庄" + n.BankerPoint + "点,闲" + n.HunterPoint + "点");

                if (n.BankerPoint > n.HunterPoint)
                    builder.Append("(庄)");
                else if (n.BankerPoint == n.HunterPoint)
                    builder.Append("(和)");
                else
                    builder.Append("(闲)");
                DateTime Oldbettime = new BCW.Baccarat.BLL.BJL_Play().GetMinBetTime(roomid, (n.RoomTable));
                if (Oldbettime.AddSeconds((Times + 9)) > DateTime.Now)
                {
                    builder.Append("[前台等待开奖]");
                    //如果没有投注记录
                    if (!new BCW.Baccarat.BLL.BJL_Play().Exists_xz(n.RoomID, n.RoomTable))
                    {
                        builder.Append("暂无投注.");
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


    //单桌分析
    private void danjufx(int roomid)
    {
        builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
        builder.Append("【数据统计】<br />");
        builder.Append(Out.Tab("</div>", ""));

        BCW.Baccarat.Model.BJL_Room aa = new BCW.Baccarat.BLL.BJL_Room().GetBJL_Room(roomid);
        long a = 0;
        long b = 0;
        if (aa.ID != 0)
        {
            a = aa.shouxufei;
            b = aa.Total_Now - aa.Total;
        }

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("本桌庄家赢利:" + b + "<br/>");
        builder.Append("本站在此桌赢利:" + a + "");
        builder.Append(Out.Tab("</div>", ""));
    }

    //公告查看与修改
    private void gonggaoPage()
    {
        int roomid = Utils.ParseInt(Utils.GetRequest("roomid", "all", 1, @"^[1-9]\d*$", ""));//房间ID
        int meid = Utils.ParseInt(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        BCW.Baccarat.Model.BJL_Room play = new BCW.Baccarat.BLL.BJL_Room().GetBJL_Room(roomid, meid);
        if (play.ID == 0)
            Utils.Error("该房间不存在.", "");

        Master.Title = "修改" + play.ID + "号桌的公告";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("bjl.aspx?act=fangjian&amp;roomid=" + roomid + "") + "\">" + new BCW.BLL.User().GetUsName(meid) + "的房间</a>&gt;公告修改");
        builder.Append(Out.Tab("</div>", ""));

        string info = Utils.GetRequest("info", "post", 1, "", "");
        if (info == "")
        {
            string strText = "标题：/,内容：/,,,,";
            string strName = "title,contact,roomid,act,info,uid";
            string strType = "text,textarea,hidden,hidden,hidden,hidden";
            string strValu = "" + play.Title + "'" + play.contact + "'" + roomid + "'gonggao'add'" + meid + "";
            string strEmpt = "true,true,false,false,false,false";
            string strIdea = "/";
            string strOthe = "修改公告,bjl.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        else
        {
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
        //房间ID
        int roomid = Utils.ParseInt(Utils.GetRequest("roomid", "get", 1, @"^[1-9]\d*$", "0"));
        BCW.Baccarat.Model.BJL_Room play = new BCW.Baccarat.BLL.BJL_Room().GetBJL_Room(roomid);
        if (play.ID == 0)
            Utils.Error("该房间不存在.", "");

        Master.Title = "" + GameName + "_公告显示";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("bjl.aspx?act=fangjian&amp;roomid=" + play.ID + "") + "\">" + new BCW.BLL.User().GetUsName(play.UsID) + "的房间</a>&gt;公告显示");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("公告标题：" + play.Title + "<br/>");
        builder.Append("公告内容：" + play.contact + "");
        builder.Append("<br/><a href=\"" + Utils.getUrl("bjl.aspx?act=fangjian&amp;roomid=" + play.ID + "") + "\">返回>></a>");
        builder.Append(Out.Tab("</div>", ""));
        foot();
    }

    //封庄
    private void fzhuangPage()
    {
        //房间ID
        int roomid = Utils.ParseInt(Utils.GetRequest("roomid", "all", 1, @"^[1-9]\d*$", ""));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        int meid = Utils.ParseInt(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));

        BCW.Baccarat.Model.BJL_Room play = new BCW.Baccarat.BLL.BJL_Room().GetBJL_Room(roomid, meid);
        if (play.ID == 0)
            Utils.Error("该房间不存在.", "");
        if (play.state == 1)
            Utils.Error("该房间已封庄,不用重复封庄.", "");
        BCW.Baccarat.Model.BJL_Play ab = new BCW.Baccarat.BLL.BJL_Play().GetBJL_Play(roomid, meid);
        if (ab.BankerPoker == "")
        {
            Utils.Error("这局正有玩家在玩，请稍后再封庄.", "");
        }


        Master.Title = "" + GameName + "_封庄";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx") + "\">" + GameName + "</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=fangjian&amp;roomid=" + play.ID + "") + "\">" + new BCW.BLL.User().GetUsName(play.UsID) + "的房间</a>&gt;封庄");
        builder.Append(Out.Tab("</div>", ""));

        if (Utils.ToSChinese(ac) == "确定封庄")
        {
            new BCW.Baccarat.BLL.BJL_Room().update_zd("state=1", "ID=" + roomid + "");
            if (play.Total_Now > 0)//退回给庄家
            {
                new BCW.BLL.User().UpdateiGold(play.UsID, new BCW.BLL.User().GetUsName(play.UsID), play.Total_Now, "系统操作:在" + GameName + "第" + roomid + "桌手动封庄,系统退还剩余彩池" + play.Total_Now + ub.Get("SiteBz") + "标识房间ID" + roomid + "");
                new BCW.BLL.Guest().Add(1, play.UsID, new BCW.BLL.User().GetUsName(play.UsID), "系统操作:在" + GameName + "第" + roomid + "桌手动封庄,系统退还剩余彩池" + play.Total_Now + ub.Get("SiteBz") + ".[url=/bbs/game/bjl.aspx]进入" + GameName + "[/url]");
            }
            else
            {
                if ((new BCW.BLL.User().GetGold(play.UsID) + play.Total_Now) > 0)//够钱扣
                {
                    new BCW.BLL.User().UpdateiGold(play.UsID, new BCW.BLL.User().GetUsName(play.UsID), -play.Total_Now, "系统操作:在" + GameName + "第" + roomid + "桌的彩池已低于0,系统自动补扣" + play.Total_Now + ub.Get("SiteBz") + "标识房间ID" + roomid + "");
                    new BCW.BLL.Guest().Add(1, play.UsID, new BCW.BLL.User().GetUsName(play.UsID), "系统操作:在" + GameName + "第" + roomid + "桌的彩池已低于0,系统自动从你账户补扣" + play.Total_Now + ub.Get("SiteBz") + ".[url=/bbs/game/bjl.aspx]进入" + GameName + "[/url]");
                }
                else
                {
                    BCW.Model.Gameowe owe = new BCW.Model.Gameowe();
                    owe.Types = 1;
                    owe.UsID = play.UsID;
                    owe.UsName = new BCW.BLL.User().GetUsName(play.UsID);
                    owe.Content = "系统操作:在" + GameName + "第" + roomid + "桌的彩池已低于0,你欠下系统的" + play.Total_Now + new BCW.BLL.User().GetGold(play.UsID) + "" + ub.Get("SiteBz") + ".";
                    owe.OweCent = play.Total_Now + new BCW.BLL.User().GetGold(play.UsID);
                    owe.BzType = 12;//百家乐封庄记录type的id
                    owe.EnId = play.ID;
                    owe.AddTime = DateTime.Now;
                    new BCW.BLL.Gameowe().Add(owe);
                    new BCW.BLL.User().UpdateIsFreeze(play.UsID, 1);

                    //发送内线
                    string strGuess = "系统操作:在" + GameName + "第" + roomid + "桌的彩池已低于0,你欠下系统的" + play.Total_Now + new BCW.BLL.User().GetGold(play.UsID) + "" + ub.Get("SiteBz") + ".[br]根据您的帐户数额,实扣" + new BCW.BLL.User().GetGold(play.UsID) + "" + ub.Get("SiteBz") + ".[br]您的" + ub.Get("SiteBz") + "不足,系统将您帐户冻结。";
                    new BCW.BLL.Guest().Add(1, play.UsID, new BCW.BLL.User().GetUsName(play.UsID), strGuess);
                    string bb = "系统操作:在" + GameName + "第" + roomid + "桌的彩池已低于0,欠下系统" + play.Total_Now + new BCW.BLL.User().GetGold(play.UsID) + "" + ub.Get("SiteBz") + ",系统已自动冻结TA的帐户.";
                    new BCW.BLL.Guest().Add(1, 10086, new BCW.BLL.User().GetUsName(10086), bb);
                }
            }
            Utils.Success("封庄成功", "封庄成功.正在返回.", Utils.getUrl("bjl.aspx?act=fangjian&amp;roomid=" + roomid + ""), "1");
        }
        else
        {
            string abc = "";
            if (ab.Play_Table < RoomTime1)
            {
                abc += "该庄家局数不足" + RoomTime1 + "局!!<br/>";
                //Utils.Error("局数不足" + RoomTime1 + "局，封庄失败." + "", "");
            }
            builder.Append(Out.Tab("<div>", "<br/>"));
            builder.Append("<b>" + abc + "</b>确定封庄吗？封庄后游戏即会结束！");
            builder.Append(Out.Tab("</div>", ""));
            string strText = "";
            string strName = "roomid,uid";
            string strType = "hidden,hidden";
            string strValu = "" + play.ID + "'" + meid + "";
            string strEmpt = "false,false";
            string strIdea = "/";
            string strOthe = "确定封庄,bjl.aspx?act=fzhuang,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }

        foot();
    }

    //日志显示
    private void diaryPage()
    {
        int meid = Utils.ParseInt(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        int roomid = Utils.ParseInt(Utils.GetRequest("roomid", "all", 1, @"^[1-9]\d*$", ""));

        BCW.Baccarat.Model.BJL_Room play = new BCW.Baccarat.BLL.BJL_Room().GetBJL_Room(roomid, meid);
        if (play.ID == 0)
            Utils.Error("该房间不存在.", "");

        Master.Title = play.ID + "号桌的日志";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx") + "\">" + GameName + "</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=fangjian&amp;roomid=" + play.ID + "") + "\">" + new BCW.BLL.User().GetUsName(play.UsID) + "的房间</a>&gt;日志查看");
        builder.Append(Out.Tab("</div>", "<br />"));


        builder.Append(Out.Tab("<div class=\"text\">", ""));
        int ptye = Utils.ParseInt(Utils.GetRequest("ptye", "get", 1, @"^[0-1]$", "1"));
        int ptye2 = Utils.ParseInt(Utils.GetRequest("ptye2", "get", 1, @"^[0-1]$", "0"));
        if (ptye == 0)
        {
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=diary&amp;ptye=1&amp;roomid=" + roomid + "&amp;uid=" + meid + "") + "\">下注日志</a>" + "|");
            builder.Append("<h style=\"color:red\">追加日志" + "</h>" + "");
        }
        else
        {
            builder.Append("<h style=\"color:red\">下注日志" + "</h>" + "|");
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=diary&amp;ptye=0&amp;roomid=" + roomid + "&amp;uid=" + meid + "") + "\">追加日志</a>" + "");
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        if (ptye == 1)
        {
            builder.Append(Out.Tab("<div>", ""));
            if (ptye2 == 0)
            {
                builder.Append("<h style=\"color:red\">下注" + "</h>" + "|");
                builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=diary&amp;ptye=" + ptye + "&amp;ptye2=1&amp;uid=" + meid + "&amp;roomid=" + roomid + "") + "\">中奖</a>" + "");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=diary&amp;ptye=" + ptye + "&amp;ptye2=0&amp;uid=" + meid + "&amp;roomid=" + roomid + "") + "\">下注</a>" + "|");
                builder.Append("<h style=\"color:red\">中奖" + "</h>" + "");
            }
            builder.Append(Out.Tab("</div>", "<br />"));
        }

        int pageIndex;
        int recordCount;
        string strWhere = "RoomID=" + roomid + " ";
        string[] pageValUrl = { "act", "roomid", "ptye", "uid", "backurl" };
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
            if (ptye2 == 1)
            {
                strWhere += "and (type=2 or type=3)";
            }
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
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.buy_usid + "") + "\">" + new BCW.BLL.User().GetUsName(n.buy_usid) + "</a>在第" + n.Play_Table + "局:押" + name1 + "");
                    try
                    {
                        builder.Append("[庄:" + allpoker(n.BankerPoker) + "(" + n.BankerPoint + "点)闲:" + allpoker(n.HunterPoker) + "(" + n.HunterPoint + "点)]<br/>");
                    }
                    catch
                    {
                        builder.Append("[庄:?(" + n.BankerPoint + "点)闲:?(" + n.HunterPoint + "点)]<br/>");
                        //new BCW.BLL.Guest().Add(1, 52784, "森林仔666", "" + GameName + "在" + n.UsID + "的第" + n.RoomID + "桌第" + n.Play_Table + "局ID:" + n.buy_usid + "开奖日志出现异常.");
                    }
                    builder.Append("共投" + n.PutMoney + ".");//每注" + n.zhu_money + "
                    if (n.GetMoney > 0)
                    {
                        builder.Append("中奖" + n.GetMoney + ",");
                    }
                    builder.Append("结" + n.Total + ".[" + DT.FormatDate(n.updatetime, 1) + "]");
                    if (n.type == 2)
                        builder.Append("<h style=\"color:red\">(未领)</h>");
                    if (n.type == 3)
                        builder.Append("<h style=\"color:red\">(已领)</h>");
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

    //查看牌局
    private void tablelistPage()
    {
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
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("bjl.aspx?act=fangjian&amp;roomid=" + roomid + "") + "\">" + new BCW.BLL.User().GetUsName(play.UsID) + "的房间</a>&gt;查看牌局");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div  class=\"text\">", ""));
        builder.Append("【" + roomid + "号桌,第" + table + "局】<br />");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("庄家:" + allpoker(card.BankerPoker) + "(<h style=\"color:red\">" + card.BankerPoint + "点</h>)<br />");
        builder.Append("闲家:" + allpoker(card.HunterPoker) + "(<h style=\"color:red\">" + card.HunterPoint + "点</h>)<br />");
        builder.Append("总下注:" + new BCW.Baccarat.BLL.BJL_Play().GetPrice(roomid, table) + ",");
        builder.Append("总中奖:" + new BCW.Baccarat.BLL.BJL_Play().Getmoney(roomid, table) + ",");
        builder.Append("总手续费:" + new BCW.Baccarat.BLL.BJL_Play().Getsxf(roomid, table) + "");
        builder.Append(Out.Tab("</div>", Out.Hr()));

        builder.Append(Out.Tab("<div  class=\"text\">", ""));
        builder.Append("【下注记录】");
        builder.Append(Out.Tab("</div>", "<br/>"));

        int pageIndex;
        int recordCount;
        string[] pageValUrl = { "act", "roomid", "table", "backurl" };
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
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + buy_usid + "") + "\">" + new BCW.BLL.User().GetUsName(buy_usid) + "</a>:[" + name1 + "]共" + PutMoney + ".");
                if (GetMoney > 0)
                {
                    builder.Append("(赢" + GetMoney + "" + ub.Get("SiteBz") + ")");
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

    //删除修改房间
    private void EditPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "" + GameName + "房间ID错误"));
        string info = Utils.GetRequest("info", "all", 1, "", "");

        if (info == "")
        {
            Master.Title = "" + GameName + "_编辑房间";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx") + "\">" + GameName + "</a>&gt;编辑房间");
            builder.Append(Out.Tab("</div>", ""));
            BCW.Baccarat.Model.BJL_Room model = new BCW.Baccarat.BLL.BJL_Room().GetBJL_Room(id);
            if (model.ID == 0)
            {
                Utils.Error("不存在的记录", "");
            }
            string strText = "用户ID:/,公告标题:/,公告内容:/,开始彩池:/,实时彩池:/,追加彩池:/,最高下注:/,最小下注:/,每局每人最高下注:(0为不限制)/,已收手续费:/,人气:/,是否封庄:/,开庄时间:/,,,";
            string strName = "UsID,Title,contact,Total,Total_Now,zhui_Total,BigPay,LowTotal,Bigmoney,shouxufei,Click,state,AddTime,info,id,act";
            string strType = "num,text,text,num,num,num,num,num,num,num,num,select,date,hidden,hidden,hidden";
            string strValu = "" + model.UsID + "'" + model.Title + "'" + model.contact + "'" + model.Total + "'" + model.Total_Now + "'" + model.zhui_Total + "'" + model.BigPay + "'" + model.LowTotal + "'" + model.Bigmoney + "'" + model.shouxufei + "'" + model.Click + "'" + model.state + "'" + model.AddTime + "'editok'" + id + "'edit";
            string strEmpt = "false,false,false,false,false,false,false,false,false,false,false,0|开庄|1|封庄,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定编辑,bjl.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=edit&amp;id=" + id + "&amp;info=del") + "\">删除房间</a>");
            builder.Append(Out.Tab("</div>", ""));
            foot();
        }
        else if (info == "del")
        {
            Master.Title = "删除桌面";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除该桌面吗？删除同时将会删除该桌面的所有下注记录.");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?info=delok&amp;act=edit&amp;id=" + id + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx?act=edit&amp;id=" + id + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else if (info == "editok")
        {
            int UsID = int.Parse(Utils.GetRequest("UsID", "all", 2, @"^[1-9]\d*$", "用户ID错误"));
            string Title = Utils.GetRequest("Title", "all", 1, @"^[\s\S]{1,15}$", "");//标题限1-15个字符
            string contact = Utils.GetRequest("contact", "all", 1, @"^[\s\S]{1,200}$", "");//内容限1-200个字符
            int Total = int.Parse(Utils.GetRequest("Total", "all", 2, @"^[1-9]\d*$", "彩池填写不能为空"));

            long Total_Now = Utils.ParseInt64(Utils.GetRequest("Total_Now", "all", 2, @"^[0-9]\d*$", "实时彩池输入错误"));
            long zhui_Total = Utils.ParseInt64(Utils.GetRequest("zhui_Total", "all", 2, @"^[0-9]\d*$", "追加彩池输入错误"));
            long LowTotal = Utils.ParseInt64(Utils.GetRequest("LowTotal", "all", 2, @"^[1-9]\d*$", "最低下注输入错误"));
            long BigPay = Utils.ParseInt64(Utils.GetRequest("BigPay", "all", 2, @"^[1-9]\d*$", "最高下注输入错误"));
            long Bigmoney = Utils.ParseInt64(Utils.GetRequest("Bigmoney", "all", 2, @"^[0-9]\d*$", "每局每人最高下注额填写错误"));
            int shouxufei = int.Parse(Utils.GetRequest("shouxufei", "all", 2, @"^[0-9]\d*$", "手续费输入错误"));
            int Click = int.Parse(Utils.GetRequest("Click", "all", 2, @"^[0-9]\d*$", "人气输入错误"));
            int state = int.Parse(Utils.GetRequest("state", "all", 2, @"^[0-9]\d*$", "封装状态出错"));
            DateTime AddTime = Utils.ParseTime(Utils.GetRequest("AddTime", "all", 1, "", "" + DateTime.Now + ""));

            new BCW.Baccarat.BLL.BJL_Room().update_zd("UsID=" + UsID + ",Title='" + Title + "',contact='" + contact + "',Total=" + Total + ",Total_Now=" + Total_Now + ",zhui_Total=" + zhui_Total + ",BigPay=" + BigPay + ",Bigmoney=" + Bigmoney + ",LowTotal=" + LowTotal + ",shouxufei=" + shouxufei + ",state=" + state + ",Click=" + Click + ",AddTime='" + AddTime + "'", "id=" + id + "");

            Utils.Success("修改成功", "修改成功.正在返回.", Utils.getUrl("bjl.aspx?act=edit&amp;id=" + id + ""), "1");
        }
        else if (info == "delok")
        {
            if (!new BCW.Baccarat.BLL.BJL_Room().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            new BCW.Baccarat.BLL.BJL_Room().Delete(id);
            new BCW.Baccarat.BLL.BJL_Play().Delete("RoomID=" + id + "");
            Utils.Success("删除桌面", "删除成功..", Utils.getPage("bjl.aspx"), "1");
        }

    }

    //返赢返负界面
    private void BackPage()
    {
        Master.Title = "" + GameName + "_返赢返负";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx") + "\">" + GameName + "</a>&gt;返赢返负");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("返赢点操作");
        builder.Append(Out.Tab("</div>", ""));
        string strText = "开始时间:,结束时间:,返赢千分比:,至少赢多少才返:,消息通知:,";
        string strName = "sTime,oTime,iTar,iPrice,text,act";
        string strType = "date,date,num,num,text,hidden";
        string strValu = "" + DT.FormatDate(DateTime.Now.AddDays(-10), 0) + "'" + DT.FormatDate(DateTime.Now, 0) + "''''backsave3";
        string strEmpt = "false,false,false,false,false,false";
        string strIdea = "/";
        string strOthe = "马上返赢,bjl.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("返负点操作");
        builder.Append(Out.Tab("</div>", ""));
        strText = "开始时间:,结束时间:,返负千分比:,至少负多少才返:,消息通知:,";
        strName = "sTime,oTime,iTar,iPrice,text,act";
        strType = "date,date,num,num,text,hidden";
        strValu = "" + DT.FormatDate(DateTime.Now.AddDays(-10), 0) + "'" + DT.FormatDate(DateTime.Now, 0) + "''''backsave5";
        strEmpt = "false,false,false,false,false,false";
        strIdea = "/";
        strOthe = "马上返负,bjl.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        foot();
    }

    //返赢返负判断
    private void BackSavePage2(string act)
    {
        DateTime sTime = Utils.ParseTime(Utils.GetRequest("sTime", "all", 2, DT.RegexTime, "开始时间填写无效"));
        DateTime oTime = Utils.ParseTime(Utils.GetRequest("oTime", "all", 2, DT.RegexTime, "结束时间填写无效"));
        int iTar = Utils.ParseInt(Utils.GetRequest("iTar", "all", 2, @"^[0-9]\d*$", "千分比填写错误"));
        int iPrice = Utils.ParseInt(Utils.GetRequest("iPrice", "all", 2, @"^[0-9]\d*$", "返币填写错误"));
        string text = Utils.GetRequest("text", "all", 2, @"^[^\^]{1,5000}$", "消息填写太多了");

        Master.Title = "" + GameName + "_返赢返负";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");


        if (act == "backsave3")
        {
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("bjl.aspx?act=back") + "\">返赢返负</a>&gt;返赢确认");
            builder.Append(Out.Tab("</div>", "<br />"));

            //邵广林 20161024
            DataSet ds = new BCW.Baccarat.BLL.BJL_Play().GetList("buy_usid,sum(GetMoney-PutMoney) as WinCents", "updatetime>='" + sTime + "'and updatetime<'" + oTime + "' group by buy_usid");
            long a = 0;
            int b = 0;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                long Cents = Convert.ToInt64(ds.Tables[0].Rows[i]["WinCents"]);
                if (Cents > 0 && Cents >= iPrice)
                {
                    int usid = Convert.ToInt32(ds.Tables[0].Rows[i]["buy_usid"]);
                    long cent = Convert.ToInt64(Cents * (iTar * 0.001));
                    b += 1;
                    a += cent;
                }
            }
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("本次返赢的开始时间： " + DT.FormatDate(sTime, 0) + "<br />");
            builder.Append("本次返赢的截止时间： " + DT.FormatDate(oTime, 0) + "<br />");
            builder.Append("本次返赢的千分比为： " + iTar.ToString().Trim() + "<br />");
            builder.Append("本次至少赢多少才返： " + iPrice.ToString().Trim() + "<br />");
            builder.Append("本次返赢的用户数为： " + b + "<br />");
            builder.Append("本次返赢的总金额为： " + a + "<br />");
            builder.Append("消息通知：" + text + ".");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "开始时间:,结束时间:,返赢千分比:,至少赢多少才返:,消息通知:,";
            string strName = "sTime,oTime,iTar,iPrice,text,act";
            string strType = "hidden,hidden,hidden,hidden,hidden,hidden";
            string strValu = "" + DT.FormatDate(sTime, 0) + "'" + DT.FormatDate(oTime, 0) + "'" + iTar + "'" + iPrice + "'" + text + "'backsave";
            string strEmpt = "false,false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定返赢,bjl.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        else if (act == "backsave5")
        {
            builder.Append("<a href=\"" + Utils.getUrl("bjl.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("bjl.aspx?act=back") + "\">返赢返负</a>&gt;返负确认");
            builder.Append(Out.Tab("</div>", "<br />"));

            //邵广林 20161024
            DataSet ds = new BCW.Baccarat.BLL.BJL_Play().GetList("buy_usid,sum(GetMoney-PutMoney) as WinCents", "updatetime>='" + sTime + "'and updatetime<'" + oTime + "' group by buy_usid");
            long a = 0;
            int b = 0;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                long Cents = Convert.ToInt64(ds.Tables[0].Rows[i]["WinCents"]);
                if (Cents < 0 && Cents < (-iPrice))
                {
                    int usid = Convert.ToInt32(ds.Tables[0].Rows[i]["buy_usid"]);
                    long cent = Convert.ToInt64(Cents * (iTar * 0.001));
                    b += 1;
                    a += cent;
                }
            }


            builder.Append(Out.Tab("<div>", ""));
            builder.Append("本次返负的开始时间： " + DT.FormatDate(sTime, 0) + "<br />");
            builder.Append("本次返负的截止时间： " + DT.FormatDate(oTime, 0) + "<br />");
            builder.Append("本次返负的千分比为： " + iTar.ToString().Trim() + "<br />");
            builder.Append("本次至少负多少才返： " + iPrice.ToString().Trim() + "<br />");
            builder.Append("本次返负的用户数为： " + b + "<br />");
            builder.Append("本次返负的总金额为： " + a + "<br />");
            builder.Append("消息通知：" + text + ".");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "开始时间:,结束时间:,返负千分比:,至少负多少才返:,消息通知:,";
            string strName = "sTime,oTime,iTar,iPrice,text,act";
            string strType = "hidden,hidden,hidden,hidden,hidden,hidden";
            string strValu = "" + DT.FormatDate(sTime, 0) + "'" + DT.FormatDate(oTime, 0) + "'" + iTar + "'" + iPrice + "'" + text + "'backsave2";
            string strEmpt = "false,false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定返负,bjl.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }

        foot();
    }

    //返赢返负执行
    private void BackSavePage(string act)
    {
        DateTime sTime = Utils.ParseTime(Utils.GetRequest("sTime", "all", 2, DT.RegexTime, "开始时间填写无效"));
        DateTime oTime = Utils.ParseTime(Utils.GetRequest("oTime", "all", 2, DT.RegexTime, "结束时间填写无效"));
        int iTar = Utils.ParseInt(Utils.GetRequest("iTar", "all", 2, @"^[0-9]\d*$", "千分比填写错误"));
        int iPrice = Utils.ParseInt(Utils.GetRequest("iPrice", "all", 2, @"^[0-9]\d*$", "返币填写错误"));
        string text = Utils.GetRequest("text", "all", 2, @"^[^\^]{1,5000}$", "消息填写太多了");

        if (act == "backsave")
        {
            DataSet ds = new BCW.Baccarat.BLL.BJL_Play().GetList("buy_usid,sum(GetMoney-PutMoney) as WinCents", "updatetime>='" + sTime + "'and updatetime<'" + oTime + "' group by buy_usid");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                long Cents = Convert.ToInt64(ds.Tables[0].Rows[i]["WinCents"]);
                if (Cents > 0 && Cents >= iPrice)
                {
                    int usid = Convert.ToInt32(ds.Tables[0].Rows[i]["buy_usid"]);
                    long cent = Convert.ToInt64(Cents * (iTar * 0.001));
                    new BCW.BLL.User().UpdateiGold(usid, cent, "" + GameName + "返赢");
                    //发内线
                    string strLog = text + "返还了：" + cent + "" + ub.Get("SiteBz") + "[url=/bbs/game/bjl.aspx]进入" + GameName + "[/url]";

                    new BCW.BLL.Guest().Add(0, usid, new BCW.BLL.User().GetUsName(usid), strLog);
                }
            }
            Utils.Success("返赢操作", "返赢操作成功", Utils.getUrl("bjl.aspx?act=back"), "1");
        }
        else if (act == "backsave2")
        {
            DataSet ds = new BCW.Baccarat.BLL.BJL_Play().GetList("buy_usid,sum(GetMoney-PutMoney) as WinCents", "updatetime>='" + sTime + "'and updatetime<'" + oTime + "' group by buy_usid");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                long Cents = Convert.ToInt64(ds.Tables[0].Rows[i]["WinCents"]);
                if (Cents < 0 && Cents < (-iPrice))
                {
                    int usid = Convert.ToInt32(ds.Tables[0].Rows[i]["buy_usid"]);
                    long cent = Convert.ToInt64((-Cents) * (iTar * 0.001));
                    new BCW.BLL.User().UpdateiGold(usid, cent, "" + GameName + "返负");
                    //发内线
                    string strLog = text + "返还了：" + cent + "" + ub.Get("SiteBz") + "[url=/bbs/game/bjl.aspx]进入" + GameName + "[/url]";
                    new BCW.BLL.Guest().Add(0, usid, new BCW.BLL.User().GetUsName(usid), strLog);
                }
            }
            Utils.Success("返负操作", "返负操作成功", Utils.getUrl("bjl.aspx?act=back"), "1");
        }
    }


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

}
