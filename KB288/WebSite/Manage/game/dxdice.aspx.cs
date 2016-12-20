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

public partial class Manage_game_dxdice : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/dxdice.xml";
    protected long F1 = Convert.ToInt64(ub.GetSub("DxdiceF1", "/Controls/dxdice.xml"));
    protected long F2 = Convert.ToInt64(ub.GetSub("DxdiceF2", "/Controls/dxdice.xml"));
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "view":
                ViewPage();
                break;
            case "del":
                DelPage();
                break;
            case "reset":
                ResetPage();
                break;
            case "stat":
                StatPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        Master.Title = "掷骰管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("掷骰");
        builder.Append(Out.Tab("</div>", "<br />"));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        if (uid > 0)
        {
            if (Utils.ToSChinese(ac) == "应战")
                strWhere += "reid=" + uid + "";
            else
                strWhere += "usid=" + uid + "";
        }
        string[] pageValUrl = {"ac", "act", "uid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Game.Dxdice> listDxdice = new BCW.BLL.Game.Dxdice().GetDxdices(pageIndex, pageSize, strWhere, out recordCount);
        if (listDxdice.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Dxdice n in listDxdice)
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
                string bzText = string.Empty;
                if (n.BzType == 0)
                {
                    bzText = ub.Get("SiteBz");
                }
                else
                {
                    bzText = ub.Get("SiteBz2");
                }
                string TrueText = string.Empty;
                if (Utils.GetDomain().Contains("kubao") || Utils.GetDomain().Contains("tuhao") || Utils.GetDomain().Contains("th") || Utils.GetDomain().Contains("kb288"))
                {
                    int ManageId = new BCW.User.Manage().IsManageLogin();
                    if (ManageId == 1)
                        TrueText = "/庄" + n.DxdiceA + "/闲" + ((n.DxdiceB == "") ? "xx" : n.DxdiceB) + "";
                }
                else
                {
                    TrueText = "/庄" + n.DxdiceA + "/闲" + ((n.DxdiceB == "") ? "xx" : n.DxdiceB) + "";
                }
                builder.Append("<a href=\"" + Utils.getUrl("dxdice.aspx?act=del&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[删]&gt;</a>");
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("dxdice.aspx?act=view&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "</a>(" + n.Price + "" + bzText + "" + TrueText + ")");
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
        string strText = "输入用户ID:/,";
        string strName = "uid,backurl";
        string strType = "num,hidden";
        string strValu = "'" + Utils.getPage(0) + "";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "搜掷骰|应战,dxdice.aspx,post,1,red|blue";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("dxdice.aspx?act=stat") + "\">赢利分析</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("dxdice.aspx?act=reset") + "\">重置游戏</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("../xml/dxdiceset.aspx?backurl=" + Utils.PostPage(1) + "") + "\">游戏配置</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("../toplist.aspx?act=top&amp;ptype=10&amp;backurl=" + Utils.PostPage(1) + "") + "\">排行榜单</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

    }

    private void ViewPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-2]\d*$", "1"));
        BCW.Model.Game.Dxdice model = new BCW.BLL.Game.Dxdice().GetDxdice(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "查看掷骰";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("查看掷骰");
        builder.Append(Out.Tab("</div>", "<br />"));


        int zDiceNum = (Utils.ParseInt(Utils.Left(model.DxdiceA, 1)) + Utils.ParseInt(Utils.Right(model.DxdiceA, 1)));
        int kDiceNum = (Utils.ParseInt(Utils.Left(model.DxdiceB, 1)) + Utils.ParseInt(Utils.Right(model.DxdiceB, 1)));
        string bzText = string.Empty;
        if (model.BzType == 0)
            bzText = ub.Get("SiteBz");
        else
            bzText = ub.Get("SiteBz2");

        bool IsSeen = true;
        if (Utils.GetDomain().Contains("kubao") || Utils.GetDomain().Contains("tuhao") || Utils.GetDomain().Contains("th") || Utils.GetDomain().Contains("kb288"))
        {
            if (model.State == 0)
            {
                int ManageId = new BCW.User.Manage().IsManageLogin();
                if (ManageId != 1 && ManageId != 11 )
                    IsSeen = false;
            }
        }

        builder.Append(Out.Tab("<div>", ""));
        builder.Append(DT.FormatDate(model.AddTime, 0) + "<br />");
        builder.Append("庄家:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.UsName + "(" + model.UsID + ")</a><br />");
        if (IsSeen == true)
            builder.Append("庄家掷骰:" + zDiceNum + "点(" + model.DxdiceA + ")<br />");
        else
            builder.Append("庄家掷骰:xx点(xx,xx)<br />");

        builder.Append("掷骰:" + model.Price + "" + bzText + "<br />");

        if (model.ReID > 0 && model.State != 4)
        {
            builder.Append("闲家:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.ReID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.ReName + "(" + model.ReID + ")</a><br />");
            builder.Append("闲家掷骰:" + kDiceNum + "点(" + model.DxdiceB + ")");
        }
        else
        {
            builder.Append("闲家:等待挑战<br />");
            builder.Append("闲家掷骰:庄家掷骰:xx点(xx,xx)");
        }
        if (model.State == 1 || model.State == 2)
        {
            builder.Append("开奖结果:");
            int iWin = GetiWin(model.DxdiceA, model.DxdiceB);
            if (iWin == 1)
            {
                builder.Append("庄家全赢");
            }
            else if (iWin == 2)
            {
                builder.Append("双方打和");
            }
            else if (iWin == 3)
            {
                builder.Append("闲家全赢");
            }
        }
        else if (model.State == 3)
        {
            builder.Append("状态:过期返还" + model.Price + "" + bzText + "");
        }
        else if (model.State == 4)
        {
            builder.Append("状态:已经撤销");
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("dxdice.aspx?act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">删除掷骰</a><br />");
        builder.Append("<a href=\"" + Utils.getPage("dxdice.aspx") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void DelPage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId != 1 && ManageId != 9)
        {
            Utils.Error("权限不足", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        if (info == "")
        {
            Master.Title = "删除掷骰";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除该记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("dxdice.aspx?info=ok&amp;act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("dxdice.aspx?act=edit&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.BLL.Game.Dxdice().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            new BCW.BLL.Game.Dxdice().Delete(id);
            Utils.Success("删除掷骰", "删除成功..", Utils.getPage("dxdice.aspx"), "1");
        }
    }

    private void ResetPage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId != 1 && ManageId != 9)
        {
            Utils.Error("权限不足", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "")
        {
            Master.Title = "重置游戏";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定重置掷骰游戏吗，重置后将删除所有掷骰记录");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("dxdice.aspx?info=ok&amp;act=reset") + "\">确定重置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("dxdice.aspx") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            new BCW.Data.SqlUp().ClearTable("tb_Dxdice");
            Utils.Success("重置游戏", "重置游戏成功..", Utils.getUrl("dxdice.aspx"), "1");
        }
    }

    private void StatPage()
    {
        Master.Title = "赢利分析";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("赢利分析");
        builder.Append(Out.Tab("</div>", "<br />"));

        double Tar1 = Convert.ToDouble(ub.GetSub("DxdiceTar", xmlPath));
        //今天赢利
        long TodayWinCent1 = new BCW.BLL.Game.Dxdice().GetPrice("BzType=0 and State>0 and Year(AddTime) = " + DateTime.Now.Year + " AND Month(AddTime) = " + DateTime.Now.Month + " and Day(AddTime) = " + DateTime.Now.Day + "");
        long TodayWin1 = Convert.ToInt64(TodayWinCent1 * Tar1 * 0.01);


        //昨天赢利
        long YesWinCent1 = new BCW.BLL.Game.Dxdice().GetPrice("BzType=0 and State>0 and Year(AddTime) = " + DateTime.Now.AddDays(-1).Year + " AND Month(AddTime) = " + DateTime.Now.AddDays(-1).Month + " and Day(AddTime) = " + DateTime.Now.AddDays(-1).Day + " ");
        long YesWin1 = Convert.ToInt64(YesWinCent1 * Tar1 * 0.01);


        //本月赢利
        long MonthWinCent1 = new BCW.BLL.Game.Dxdice().GetPrice("BzType=0 and State>0 and Year(AddTime) = " + (DateTime.Now.Year) + " AND Month(AddTime) = " + (DateTime.Now.Month) + " ");
        long MonthWin1 = Convert.ToInt64(MonthWinCent1 * Tar1 * 0.01);

        //上月赢利
        long Month2WinCent1 = new BCW.BLL.Game.Dxdice().GetPrice("BzType=0 and State>0 and Year(AddTime) = " + (DateTime.Now.Year - DateTime.Now.Day) + " AND Month(AddTime) = " + (DateTime.Now.Month - DateTime.Now.Day) + " ");
        long Month2Win1 = Convert.ToInt64(Month2WinCent1 * Tar1 * 0.01);

        long WinCent1 = new BCW.BLL.Game.Dxdice().GetPrice("BzType=0 and State>0");
        long Win1 = Convert.ToInt64(WinCent1 * Tar1 * 0.01);

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("今天赢利:" + (TodayWin1) + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("昨天赢利:" + (YesWin1) + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("本月赢利:" + (MonthWin1) + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("上月赢利:" + (Month2Win1) + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("赢利总计:" + (Win1) + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("dxdice.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    /// <summary>
    /// 1开盘人赢/2打和/3开盘人输
    /// </summary>
    /// <param name="DxdiceA">开盘骰</param>
    /// <param name="DxdiceB">激战骰</param>
    /// <returns></returns>
    private int GetiWin(string DxdiceA, string DxdiceB)
    {
        int diceA1 = Utils.ParseInt(Utils.Left(DxdiceA, 1));
        int diceA2 = Utils.ParseInt(Utils.Right(DxdiceA, 1));
        int diceB1 = Utils.ParseInt(Utils.Left(DxdiceB, 1));
        int diceB2 = Utils.ParseInt(Utils.Right(DxdiceB, 1));
        int diceA = diceA1 + diceA2;
        int diceB = diceB1 + diceB2;
        if (diceA == diceB)
        {
            if (diceA1 == diceB1 || diceA1 == diceB2)
            {
                return 2;
            }
            int[] iNum = { diceA1, diceA2, diceB1, diceB2 };
            int max = Utils.maxNum(iNum);
            if (max == diceA1 || max == diceA2)
            {
                return 1;
            }
            else
            {
                return 3;
            }
        }
        else if (diceA > diceB)
        {
            return 1;
        }
        else
        {
            return 3;
        }

    }
}