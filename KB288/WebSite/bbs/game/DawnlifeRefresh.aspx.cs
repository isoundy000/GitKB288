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

public partial class bbs_game_DawnlifeRefresh : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/Dawnlife.xml";
    protected string GameName = ub.GetSub("DawnlifeName", "/Controls/Dawnlife.xml");

    protected double jcd = Convert.ToInt32(ub.GetSub("jcd", "/Controls/Dawnlife.xml"));
    protected double jcx = Convert.ToInt32(ub.GetSub("jcx", "/Controls/Dawnlife.xml"));
    protected double drd = Convert.ToInt32(ub.GetSub("drd", "/Controls/Dawnlife.xml"));
    protected double drq = Convert.ToInt32(ub.GetSub("drq", "/Controls/Dawnlife.xml"));
    protected double q1 = Convert.ToInt32(ub.GetSub("q1", "/Controls/Dawnlife.xml"));
    protected double q2 = Convert.ToInt32(ub.GetSub("q2", "/Controls/Dawnlife.xml"));
    protected double q3 = Convert.ToInt32(ub.GetSub("q3", "/Controls/Dawnlife.xml"));
    protected double q4 = Convert.ToInt32(ub.GetSub("q4", "/Controls/Dawnlife.xml"));
    protected double q5 = Convert.ToInt32(ub.GetSub("q5", "/Controls/Dawnlife.xml"));
    protected string dq = Convert.ToString(ub.GetSub("dq", "/Controls/Dawnlife.xml"));
    protected double juankuan = Convert.ToInt32(ub.GetSub("juankuan", "/Controls/Dawnlife.xml"));

    protected int r;
    protected string strText = string.Empty;
    protected string strName = string.Empty;
    protected string strType = string.Empty;
    protected string strValu = string.Empty;
    protected string strEmpt = string.Empty;
    protected string strIdea = string.Empty;
    protected string strOthe = string.Empty;
    ub xml = new ub();
    protected void Page_Load(object sender, EventArgs e)
    {
        string Logo = ub.GetSub("DawnlifeLogo", xmlPath);
        Response.Write("<br/>");
        Response.Write("======================================<img src=\"" + Logo + "\"  alt=\"load\"/>"+"" + GameName + "自动派奖页面============================================<br/><br/>");
        Response.Write("====================================上次刷新时间：" + DateTime.Now + "=========================================<br/><br/>");
        Response.Write("=================================每日凌晨00.00.00到00.01.00这一时间段为开奖时间" + "==============================<br/><br/>");
        AwardrPage();
    }

    //自动派奖与奖池更新面页
    private void AwardrPage()
    {
        //Master.Title = "管理中心";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("Dawnlife.aspx") + "\">" + GameName + "管理</a>&gt;自动派奖与奖池更新管理");
        builder.Append(Out.Tab("</div>", "<br />"));

        //第一个玩家更新奖池派奖,不存在第一条数据，可以自动派奖
        DataSet today = new BCW.BLL.dawnlifegift().GetList("Top 1 *", " DateDiff(day,date,getdate())=0 ");
        if (today.Tables[0].Rows.Count <= 0)
        {
            //兑奖页面
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("说明：本面页用于奖池自动更新与自动派奖，请管理员请勿随意操作");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<br />", Out.Hr()));
            builder.Append(Out.Tab("</div>", ""));
            string day = DateTime.Now.AddDays(-1).ToString("yyyy年MM月dd日");
            int pageIndex = 0;//当前页
            int recordCount;//记录总条数
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));//分页大小
            string strWhere = "";
            string strOrder = "";
            strWhere = " DateDiff(day,date,getdate())=1";
            strOrder = "date Desc";
            /////////////////////////////////////////////////////////
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("全国前五：");
            builder.Append(Out.Tab("</div>", "<br />"));
            IList<BCW.Model.dawnlifegift> listdawnlifeTop4 = new BCW.BLL.dawnlifegift().Getdawnlifegifts(pageIndex, pageSize, strWhere, strOrder, out recordCount);
            if (listdawnlifeTop4.Count > 0)
            {
                int l = 1;
                foreach (BCW.Model.dawnlifegift n4 in listdawnlifeTop4)
                {
                    if (l <= 1)
                    {
                        int gift = Convert.ToInt32(n4.gift);
                        //查询条件
                        strWhere = "DateDiff(day,date,getdate())=1";
                        strOrder = "money Desc";
                        string[] pageValUrl = { "act", "ptypet", "ptyped", "backurl" };
                        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                        if (pageIndex == 0)
                            pageIndex = 1;
                        string _time = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                        int _time_num = Convert.ToInt32(_time);
                        if (_time_num == 0)
                        {
                            // 开始读取列表
                            IList<BCW.Model.dawnlifeTop> listdawnlifeTop = new BCW.BLL.dawnlifeTop().GetdawnlifeTops(pageIndex, pageSize, strWhere, strOrder, out recordCount);
                            if (listdawnlifeTop.Count > 0)
                            {
                                int k = 1;
                                int j = 1;
                                int h = 1;
                                foreach (BCW.Model.dawnlifeTop n in listdawnlifeTop)
                                {
                                    if (j <= 5)
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
                                        int d = (pageIndex - 1) * 10 + k;
                                        int coin = Convert.ToInt16(n.coin);
                                        int money = Convert.ToInt32(n.money);
                                        string sText = string.Empty;
                                        int award = 0;
                                        if (h == 1) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q1 / 100); }
                                        if (h == 2) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q2 / 100); }
                                        if (h == 3) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q3 / 100); }
                                        if (h == 4) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q4 / 100); }
                                        if (h == 5) { award = Convert.ToInt32(gift * jcd / 100 * drq / 100 * q5 / 100); }
                                        if (n.sum == 0 && n.money > 0)
                                        {
                                            int usid = Convert.ToInt32(n.UsID);
                                            BCW.Model.dawnlifeTop update = new BCW.Model.dawnlifeTop();
                                            r = new BCW.BLL.dawnlifeTop().GetRowByUsID(usid, coin, money);
                                            new BCW.BLL.dawnlifeTop().Updatesum(r, 1);
                                            new BCW.BLL.User().UpdateiGold(usid, award, "闯荡" + DateTime.Now.AddDays(-1).ToString("MM月dd日") + "全国榜单兑奖|标识ID" + n.coin + "");
                                            //发内线
                                            string strLog = "根据你上期" + GameName + "全国排行榜上的赢利情况，你的闯荡获得第" + h + "名，闯荡君奖励你" + award + "酷币" + "[url=/bbs/game/Dawnlife.aspx]进入" + GameName + "[/url]";
                                            new BCW.BLL.Guest().Add(0, usid, new BCW.BLL.User().GetUsName(usid), strLog);

                                            sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "获得第" + h + "名，" + "奖励" + award + "酷币:" + "派奖";
                                        }
                                        else
                                        {
                                            sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "获得第" + h + "名，" + "奖励" + award + "酷币:" + "已派奖";
                                        }

                                        builder.AppendFormat("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=awardr&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>" + d + sText);
                                        k++;
                                        h++;
                                        builder.Append(Out.Tab("</div>", ""));
                                    }
                                    else
                                    { ;  }
                                    j++;
                                }
                            }
                            else
                            {
                                builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
                            }
                        }
                        else
                        {
                            string day1 = DateTime.Now.AddDays(0).ToString("yyyy年MM月dd日");
                            builder.Append("" + day + "获奖名单已经全部奖励，请前去游戏领奖查看" + "<br />");
                            builder.Append("等待" + day1 + "奖励时间开启中...");
                        }
                    }
                    else
                    {
                        ;
                    }
                    l++;
                }
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
            }
            builder.Append(Out.Tab("<br />", Out.Hr()));
            ///////////////////////////////////////////////////////////////////
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("广州地区前五：");
            builder.Append(Out.Tab("</div>", "<br />"));
            strWhere = " DateDiff(day,date,getdate())=1";
            strOrder = "date Desc";
            IList<BCW.Model.dawnlifegift> listdawnlifeTop1 = new BCW.BLL.dawnlifegift().Getdawnlifegifts(pageIndex, pageSize, strWhere, strOrder, out recordCount);
            if (listdawnlifeTop1.Count > 0)
            {
                int l = 1;
                foreach (BCW.Model.dawnlifegift n1 in listdawnlifeTop1)
                {
                    if (l <= 1)
                    {
                        int gift = Convert.ToInt32(n1.gift);
                        //查询条件
                        strWhere = "DateDiff(day,date,getdate())=1 and city ='1' ";
                        strOrder = "money Desc";

                        string[] pageValUrl = { "act", "ptypet", "ptyped", "backurl" };
                        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                        if (pageIndex == 0)
                            pageIndex = 1;
                        string _time = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                        int _time_num = Convert.ToInt32(_time);
                        if (_time_num == 0)
                        {
                            // 开始读取列表
                            IList<BCW.Model.dawnlifeTop> listdawnlifeTop11 = new BCW.BLL.dawnlifeTop().GetdawnlifeTops(pageIndex, pageSize, strWhere, strOrder, out recordCount);
                            if (listdawnlifeTop11.Count > 0)
                            {
                                int k = 1;
                                int j = 1;
                                int h = 1;
                                foreach (BCW.Model.dawnlifeTop n in listdawnlifeTop11)
                                {
                                    if (j <= 5)
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
                                        int d = (pageIndex - 1) * 10 + k;
                                        int coin = Convert.ToInt16(n.coin);
                                        int money = Convert.ToInt32(n.money);
                                        string sText = string.Empty;
                                        int award = 0;
                                        if (h == 1) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                        if (h == 2) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                        if (h == 3) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                        if (h == 4) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                        if (h == 5) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                        if (n.cum == 0 && n.money > 0)
                                        {
                                            int usid = Convert.ToInt32(n.UsID);
                                            BCW.Model.dawnlifeTop update = new BCW.Model.dawnlifeTop();
                                            r = new BCW.BLL.dawnlifeTop().GetRowByUsID(usid, coin, money);
                                            new BCW.BLL.dawnlifeTop().Updatecum(r, 1);
                                            new BCW.BLL.User().UpdateiGold(usid, award, "闯荡" + DateTime.Now.AddDays(-1).ToString("MM月dd日") + "广州榜单兑奖|标识ID" + n.coin + "");
                                            //发内线
                                            string strLog = "根据你上期" + GameName + "广州排行榜上的赢利情况，你的闯荡获得第" + h + "名，闯荡君奖励你" + award + "酷币" + "[url=/bbs/game/Dawnlife.aspx]进入" + GameName + "[/url]";
                                            new BCW.BLL.Guest().Add(0, usid, new BCW.BLL.User().GetUsName(usid), strLog);

                                            sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "获得第" + h + "名，" + "奖励" + award + "酷币:" + "派奖";
                                        }
                                        else
                                        {
                                            sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "获得第" + h + "名，" + "奖励" + award + "酷币:" + "已派奖";
                                        }

                                        builder.AppendFormat("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=awardr&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>" + d + sText);
                                        k++;
                                        h++;
                                        builder.Append(Out.Tab("</div>", ""));
                                    }
                                    else
                                    { ;  }
                                    j++;
                                }
                            }
                            else
                            {
                                builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
                            }
                        }
                        else
                        {
                            string day1 = DateTime.Now.AddDays(0).ToString("yyyy年MM月dd日");
                            builder.Append("" + day + "获奖名单已经全部奖励，请前去游戏领奖查看" + "<br />");
                            builder.Append("等待" + day1 + "奖励时间开启中...");
                        }
                    }
                    else
                    {
                        ;
                    }
                    l++;
                }
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
            }
            builder.Append(Out.Tab("<br />", Out.Hr()));

            ///////////////////////////////////////////////////////////////////
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("北京地区前五：");
            builder.Append(Out.Tab("</div>", "<br />"));
            strWhere = " DateDiff(day,date,getdate())=1";
            strOrder = "date Desc";
            IList<BCW.Model.dawnlifegift> listdawnlifeTop2 = new BCW.BLL.dawnlifegift().Getdawnlifegifts(pageIndex, pageSize, strWhere, strOrder, out recordCount);
            if (listdawnlifeTop2.Count > 0)
            {
                int l = 1;
                foreach (BCW.Model.dawnlifegift n1 in listdawnlifeTop2)
                {
                    if (l <= 1)
                    {
                        int gift = Convert.ToInt32(n1.gift);
                        //查询条件
                        strWhere = "DateDiff(day,date,getdate())=1 and city ='2' ";
                        strOrder = "money Desc";

                        string[] pageValUrl = { "act", "ptypet", "ptyped", "backurl" };
                        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                        if (pageIndex == 0)
                            pageIndex = 1;
                        string _time = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                        int _time_num = Convert.ToInt32(_time);
                        if (_time_num == 0)
                        {
                            // 开始读取列表
                            IList<BCW.Model.dawnlifeTop> listdawnlifeTop11 = new BCW.BLL.dawnlifeTop().GetdawnlifeTops(pageIndex, pageSize, strWhere, strOrder, out recordCount);
                            if (listdawnlifeTop11.Count > 0)
                            {
                                int k = 1;
                                int j = 1;
                                int h = 1;
                                foreach (BCW.Model.dawnlifeTop n in listdawnlifeTop11)
                                {
                                    if (j <= 5)
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
                                        int d = (pageIndex - 1) * 10 + k;
                                        int coin = Convert.ToInt16(n.coin);
                                        int money = Convert.ToInt32(n.money);
                                        string sText = string.Empty;
                                        int award = 0;
                                        if (h == 1) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                        if (h == 2) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                        if (h == 3) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                        if (h == 4) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                        if (h == 5) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                        if (n.cum == 0 && n.money > 0)
                                        {
                                            int usid = Convert.ToInt32(n.UsID);
                                            BCW.Model.dawnlifeTop update = new BCW.Model.dawnlifeTop();
                                            r = new BCW.BLL.dawnlifeTop().GetRowByUsID(usid, coin, money);
                                            new BCW.BLL.dawnlifeTop().Updatecum(r, 1);
                                            new BCW.BLL.User().UpdateiGold(usid, award, "闯荡" + DateTime.Now.AddDays(-1).ToString("MM月dd日") + "北京榜单兑奖|标识ID" + n.coin + "");
                                            //发内线
                                            string strLog = "根据你上期" + GameName + "北京排行榜上的赢利情况，你的闯荡获得第" + h + "名，闯荡君奖励你" + award + "酷币" + "[url=/bbs/game/Dawnlife.aspx]进入" + GameName + "[/url]";
                                            new BCW.BLL.Guest().Add(0, usid, new BCW.BLL.User().GetUsName(usid), strLog);

                                            sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "获得第" + h + "名，" + "奖励" + award + "酷币:" + "派奖";
                                        }
                                        else
                                        {
                                            sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "获得第" + h + "名，" + "奖励" + award + "酷币:" + "已派奖";
                                        }

                                        builder.AppendFormat("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=awardr&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>" + d + sText);
                                        k++;
                                        h++;
                                        builder.Append(Out.Tab("</div>", ""));
                                    }
                                    else
                                    { ;  }
                                    j++;
                                }
                            }
                            else
                            {
                                builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
                            }
                        }
                        else
                        {
                            string day1 = DateTime.Now.AddDays(0).ToString("yyyy年MM月dd日");
                            builder.Append("" + day + "获奖名单已经全部奖励，请前去游戏领奖查看" + "<br />");
                            builder.Append("等待" + day1 + "奖励时间开启中...");
                        }
                    }
                    else
                    {
                        ;
                    }
                    l++;
                }
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
            }
            builder.Append(Out.Tab("<br />", Out.Hr()));

            ///////////////////////////////////////////////////////////////////
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("上海地区前五：");
            builder.Append(Out.Tab("</div>", "<br />"));
            strWhere = " DateDiff(day,date,getdate())=1";
            strOrder = "date Desc";
            IList<BCW.Model.dawnlifegift> listdawnlifeTop3 = new BCW.BLL.dawnlifegift().Getdawnlifegifts(pageIndex, pageSize, strWhere, strOrder, out recordCount);
            if (listdawnlifeTop3.Count > 0)
            {
                int l = 1;
                foreach (BCW.Model.dawnlifegift n1 in listdawnlifeTop3)
                {
                    if (l <= 1)
                    {
                        int gift = Convert.ToInt32(n1.gift);
                        //查询条件
                        strWhere = "DateDiff(day,date,getdate())=1 and city ='3' ";
                        strOrder = "money Desc";

                        string[] pageValUrl = { "act", "ptypet", "ptyped", "backurl" };
                        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                        if (pageIndex == 0)
                            pageIndex = 1;
                        string _time = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                        int _time_num = Convert.ToInt32(_time);
                        if (_time_num == 0)
                        {
                            // 开始读取列表
                            IList<BCW.Model.dawnlifeTop> listdawnlifeTop11 = new BCW.BLL.dawnlifeTop().GetdawnlifeTops(pageIndex, pageSize, strWhere, strOrder, out recordCount);
                            if (listdawnlifeTop11.Count > 0)
                            {
                                int k = 1;
                                int j = 1;
                                int h = 1;
                                foreach (BCW.Model.dawnlifeTop n in listdawnlifeTop11)
                                {
                                    if (j <= 5)
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
                                        int d = (pageIndex - 1) * 10 + k;
                                        int coin = Convert.ToInt16(n.coin);
                                        int money = Convert.ToInt32(n.money);
                                        string sText = string.Empty;
                                        int award = 0;
                                        if (h == 1) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                        if (h == 2) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                        if (h == 3) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                        if (h == 4) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                        if (h == 5) { award = Convert.ToInt32(gift * jcd / 100 * drd / 100 * 0.06666667); }
                                        if (n.cum == 0 && n.money > 0)
                                        {
                                            int usid = Convert.ToInt32(n.UsID);
                                            BCW.Model.dawnlifeTop update = new BCW.Model.dawnlifeTop();
                                            r = new BCW.BLL.dawnlifeTop().GetRowByUsID(usid, coin, money);
                                            new BCW.BLL.dawnlifeTop().Updatecum(r, 1);
                                            new BCW.BLL.User().UpdateiGold(usid, award, "闯荡" + DateTime.Now.AddDays(-1).ToString("MM月dd日") + "上海榜单兑奖|标识ID" + n.coin + "");
                                            //发内线
                                            string strLog = "根据你上期" + GameName + "上海排行榜上的赢利情况，你的闯荡获得第" + h + "名，闯荡君奖励你" + award + "酷币" + "[url=/bbs/game/Dawnlife.aspx]进入" + GameName + "[/url]";
                                            new BCW.BLL.Guest().Add(0, usid, new BCW.BLL.User().GetUsName(usid), strLog);

                                            sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "获得第" + h + "名，" + "奖励" + award + "酷币:" + "派奖";
                                        }
                                        else
                                        {
                                            sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "获得第" + h + "名，" + "奖励" + award + "酷币:" + "已派奖";
                                        }

                                        builder.AppendFormat("<a href=\"" + Utils.getUrl("Dawnlife.aspx?act=awardr&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>" + d + sText);
                                        k++;
                                        h++;
                                        builder.Append(Out.Tab("</div>", ""));
                                    }
                                    else
                                    { ;  }
                                    j++;
                                }
                            }
                            else
                            {
                                builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
                            }
                        }
                        else
                        {
                            string day1 = DateTime.Now.AddDays(0).ToString("yyyy年MM月dd日");
                            builder.Append("" + day + "获奖名单已经全部奖励，请前去游戏领奖查看" + "<br />");
                            builder.Append("等待" + day1 + "奖励时间开启中...");
                        }
                    }
                    else
                    {
                        ;
                    }
                    l++;
                }
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
            }
            builder.Append(Out.Tab("<br />", Out.Hr()));


            ////////////////////////////////


            BCW.Model.dawnlifegift give = new BCW.Model.dawnlifegift();
            r = new BCW.BLL.dawnlifegift().GetMaxId();
            BCW.Model.dawnlifegift re = new BCW.BLL.dawnlifegift().Getdawnlifegift(r - 1);
            int gift2 = Convert.ToInt32(re.gift * jcx / 100) + Convert.ToInt32(re.giftj * juankuan / 100);
            string _time3 = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
            int _time_num3 = Convert.ToInt32(_time3);
            if (_time_num3 == 0)
            {
                give.date = DateTime.Now;
                give.gift = gift2;
                give.giftj = Convert.ToInt32(re.giftj * (1 - (juankuan / 100)));
                give.UsID = 1;
                give.UsName = Convert.ToInt32(re.giftj * juankuan / 100).ToString();
                give.coin = Convert.ToInt32(re.gift * jcd / 100);
                give.state = 3;
                new BCW.BLL.dawnlifegift().Add(give);
            }
        }
    }
}

