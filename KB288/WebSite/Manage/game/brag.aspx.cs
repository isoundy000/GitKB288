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

public partial class Manage_game_brag : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/brag.xml";
    protected long F1 = Convert.ToInt64(ub.GetSub("BragF1", "/Controls/brag.xml"));
    protected long F2 = Convert.ToInt64(ub.GetSub("BragF2", "/Controls/brag.xml"));
    protected long F3 = Convert.ToInt64(ub.GetSub("BragF3", "/Controls/brag.xml"));
    protected long F4 = Convert.ToInt64(ub.GetSub("BragF4", "/Controls/brag.xml"));
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
        Master.Title = "吹牛管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("吹牛");
        builder.Append(Out.Tab("</div>", "<br />"));

        string ac = Utils.ToSChinese(Utils.GetRequest("ac", "all", 1, "", ""));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        if (uid > 0)
        {
            if (ac == "搜吹牛")
                strWhere += "usid=" + uid + "";
            else if (ac == "搜参与")
                strWhere += "reid=" + uid + "";
        }
        string[] pageValUrl = { "ac", "act", "uid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Game.Brag> listBrag = new BCW.BLL.Game.Brag().GetBrags(pageIndex, pageSize, strWhere, out recordCount);
        if (listBrag.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Brag n in listBrag)
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
                        TrueText = "/答案" + n.TrueType + "";
                }
                else
                {
                    TrueText = "/答案" + n.TrueType + "";
                }
                builder.Append("<a href=\"" + Utils.getUrl("brag.aspx?act=del&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[删]&gt;</a>");
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("brag.aspx?act=view&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "|" + n.Title + "</a>(" + n.Price + "" + bzText + "" + TrueText + ")");
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
        string strOthe = "搜吹牛|搜参与,brag.aspx,post,1,red|blue";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("brag.aspx?act=stat") + "\">赢利分析</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("brag.aspx?act=reset") + "\">重置游戏</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("../xml/bragset.aspx?backurl=" + Utils.PostPage(1) + "") + "\">游戏配置</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("../toplist.aspx?act=top&amp;ptype=7&amp;backurl=" + Utils.PostPage(1) + "") + "\">排行榜单</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

    }

    private void ViewPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-2]\d*$", "1"));
        BCW.Model.Game.Brag model = new BCW.BLL.Game.Brag().GetBrag(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "吹牛《" + model.Title + "》";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("查看吹牛");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("发布时间:" + DT.FormatDate(model.AddTime, 0) + "<br />");
        builder.Append("应战时间:" + DT.FormatDate(model.ReTime, 0) + "<br />");
        builder.Append("吹牛人:<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.UsName + "</a><br />");
        builder.Append("内容:" + model.Title + "<br />");

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
        builder.Append("答案1:" + model.BragA + "");
        if (IsSeen)
        {
            if (model.TrueType == 1)
                builder.Append("(正确)");
        }
        builder.Append("<br />答案2:" + model.BragB + "");
        if (IsSeen)
        {
            if (model.TrueType == 2)
                builder.Append("(正确)");
        }
        string bzText = string.Empty;
        if (model.BzType == 0)
            bzText = ub.Get("SiteBz");
        else
            bzText = ub.Get("SiteBz2");

        builder.Append("<br />" + CaseBrag(model.Price, model.BzType) + ":" + model.Price + "" + bzText + "<br />");

        if (model.ReID > 0 && model.State != 4)
        {
            if (model.State == 0)
                builder.Append("等待");

            builder.Append("应答人:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.ReID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.ReName + "(" + model.ReID + ")</a><br />");
        }
        if (model.State == 0)
            builder.Append("开奖结果:未开奖");
        else if (model.State == 1 || model.State == 2)
        {
            if (model.TrueType != model.ChooseType)
                builder.Append("获胜者:<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.UsName + "(" + model.UsID + ")</a>");
            else
                builder.Append("获胜者:<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + model.ReID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.ReName + "(" + model.ReID + ")</a>");
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
        builder.Append("<a href=\"" + Utils.getUrl("brag.aspx?act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">删除吹牛</a><br />");
        builder.Append("<a href=\"" + Utils.getPage("brag.aspx") + "\">返回上一级</a>");
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
            Master.Title = "删除吹牛";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除该记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("brag.aspx?info=ok&amp;act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("brag.aspx?act=edit&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.BLL.Game.Brag().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            new BCW.BLL.Game.Brag().Delete(id);
            Utils.Success("删除吹牛", "删除成功..", Utils.getPage("brag.aspx"), "1");
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
            builder.Append("确定重置吹牛游戏吗，重置后将删除所有吹牛记录");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("brag.aspx?info=ok&amp;act=reset") + "\">确定重置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("brag.aspx") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            new BCW.Data.SqlUp().ClearTable("tb_Brag");
            Utils.Success("重置游戏", "重置游戏成功..", Utils.getUrl("brag.aspx"), "1");
        }
    }

    private void StatPage()
    {
        Master.Title = "赢利分析";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("赢利分析");
        builder.Append(Out.Tab("</div>", "<br />"));

        double Tar1 = Convert.ToDouble(ub.GetSub("BragTar1", xmlPath));
        double Tar2 = Convert.ToDouble(ub.GetSub("BragTar2", xmlPath));
        double Tar3 = Convert.ToDouble(ub.GetSub("BragTar3", xmlPath));
        //今天赢利
        long TodayWinCent1 = new BCW.BLL.Game.Brag().GetPrice("BzType=0 and State>0 and Price<" + F1 + " and Year(AddTime) = " + DateTime.Now.Year + " AND Month(AddTime) = " + DateTime.Now.Month + " and Day(AddTime) = " + DateTime.Now.Day + "");
        long TodayWinCent2 = new BCW.BLL.Game.Brag().GetPrice("BzType=0 and State>0 and Price>=" + F1 + " AND Price<" + F2 + " and Year(AddTime) = " + DateTime.Now.Year + " AND Month(AddTime) = " + DateTime.Now.Month + " and Day(AddTime) = " + DateTime.Now.Day + "");
        long TodayWinCent3 = new BCW.BLL.Game.Brag().GetPrice("BzType=0 and State>0 and Price>=" + F2 + " and Year(AddTime) = " + DateTime.Now.Year + " AND Month(AddTime) = " + DateTime.Now.Month + " and Day(AddTime) = " + DateTime.Now.Day + "");

        long TodayWin1 = Convert.ToInt64(TodayWinCent1 * Tar1 * 0.01);
        long TodayWin2 = Convert.ToInt64(TodayWinCent2 * Tar2 * 0.01);
        long TodayWin3 = Convert.ToInt64(TodayWinCent3 * Tar3 * 0.01);

        //昨天赢利
        long YesWinCent1 = new BCW.BLL.Game.Brag().GetPrice("BzType=0 and State>0 and Price<" + F1 + " and Year(AddTime) = " + DateTime.Now.AddDays(-1).Year + " AND Month(AddTime) = " + DateTime.Now.AddDays(-1).Month + " and Day(AddTime) = " + DateTime.Now.AddDays(-1).Day + " ");
        long YesWinCent2 = new BCW.BLL.Game.Brag().GetPrice("BzType=0 and State>0 and Price>=" + F1 + " AND Price<" + F2 + " and Year(AddTime) = " + DateTime.Now.AddDays(-1).Year + " AND Month(AddTime) = " + DateTime.Now.AddDays(-1).Month + " and Day(AddTime) = " + DateTime.Now.AddDays(-1).Day + " ");
        long YesWinCent3 = new BCW.BLL.Game.Brag().GetPrice("BzType=0 and State>0 and Price>=" + F2 + " and Year(AddTime) = " + DateTime.Now.AddDays(-1).Year + " AND Month(AddTime) = " + DateTime.Now.AddDays(-1).Month + " and Day(AddTime) = " + DateTime.Now.AddDays(-1).Day + " ");
       
        long YesWin1 = Convert.ToInt64(YesWinCent1 * Tar1 * 0.01);
        long YesWin2 = Convert.ToInt64(YesWinCent2 * Tar2 * 0.01);
        long YesWin3 = Convert.ToInt64(YesWinCent3 * Tar3 * 0.01);

        //本月赢利
        long MonthWinCent1 = new BCW.BLL.Game.Brag().GetPrice("BzType=0 and State>0 and Price<" + F1 + " and Year(AddTime) = " + (DateTime.Now.Year) + " AND Month(AddTime) = " + (DateTime.Now.Month) + " ");
        long MonthWinCent2 = new BCW.BLL.Game.Brag().GetPrice("BzType=0 and State>0 and Price>=" + F1 + " AND Price<" + F2 + " and Year(AddTime) = " + (DateTime.Now.Year) + " AND Month(AddTime) = " + (DateTime.Now.Month) + " ");
        long MonthWinCent3 = new BCW.BLL.Game.Brag().GetPrice("BzType=0 and State>0 and Price>=" + F2 + " and Year(AddTime) = " + (DateTime.Now.Year) + " AND Month(AddTime) = " + (DateTime.Now.Month) + " ");
       
        long MonthWin1 = Convert.ToInt64(MonthWinCent1 * Tar1 * 0.01);
        long MonthWin2 = Convert.ToInt64(MonthWinCent2 * Tar2 * 0.01);
        long MonthWin3 = Convert.ToInt64(MonthWinCent3 * Tar3 * 0.01);

        //上月赢利
        long Month2WinCent1 = new BCW.BLL.Game.Brag().GetPrice("BzType=0 and State>0 and Price<" + F1 + " and Year(AddTime) = " + (DateTime.Now.Year - DateTime.Now.Day) + " AND Month(AddTime) = " + (DateTime.Now.Month - DateTime.Now.Day) + " ");
        long Month2WinCent2 = new BCW.BLL.Game.Brag().GetPrice("BzType=0 and State>0 and Price>=" + F1 + " AND Price<" + F2 + " and Year(AddTime) = " + (DateTime.Now.Year - DateTime.Now.Day) + " AND Month(AddTime) = " + (DateTime.Now.Month - DateTime.Now.Day) + " ");
        long Month2WinCent3 = new BCW.BLL.Game.Brag().GetPrice("BzType=0 and State>0 and Price>=" + F2 + " and Year(AddTime) = " + (DateTime.Now.Year - DateTime.Now.Day) + " AND Month(AddTime) = " + (DateTime.Now.Month - DateTime.Now.Day) + " ");
        
        
        long Month2Win1 = Convert.ToInt64(Month2WinCent1 * Tar1 * 0.01);
        long Month2Win2 = Convert.ToInt64(Month2WinCent2 * Tar2 * 0.01);
        long Month2Win3 = Convert.ToInt64(Month2WinCent3 * Tar3 * 0.01);

        long WinCent1 = new BCW.BLL.Game.Brag().GetPrice("BzType=0 and State>0 and Price<" + F1 + "");
        long WinCent2 = new BCW.BLL.Game.Brag().GetPrice("BzType=0 and State>0 and Price>=" + F1 + " AND Price<" + F2 + " ");
        long WinCent3 = new BCW.BLL.Game.Brag().GetPrice("BzType=0 and State>0 and Price>=" + F2 + "");

        long Win1 = Convert.ToInt64(WinCent1 * Tar1 * 0.01);
        long Win2 = Convert.ToInt64(WinCent2 * Tar2 * 0.01);
        long Win3 = Convert.ToInt64(WinCent3 * Tar3 * 0.01);

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("今天赢利:" + (TodayWin1 + TodayWin2 + TodayWin3) + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("昨天赢利:" + (YesWin1 + YesWin2 + YesWin3) + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("本月赢利:" + (MonthWin1 + MonthWin2 + MonthWin3) + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("上月赢利:" + (Month2Win1 + Month2Win2 + Month2Win3) + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("赢利总计:" + (Win1 + Win2 + Win3) + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("brag.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private string CaseBrag(long Price, int BzType)
    {
        if (BzType == 0)
        {
            if (Price >= F2)
                return "犀牛";

            if (Price > F1)
                return "水牛";
        }
        else
        {
            if (Price >= F4)
                return "犀牛";

            if (Price > F3)
                return "水牛";
        }
        return "蜗牛";
    }

    private bool Isbz()
    {
        return true;

        //if (Utils.GetDomain().Contains("kubao") || Utils.GetDomain().Contains("tuhao") || Utils.GetDomain().Contains("th") || Utils.GetDomain().Contains("kb288"))
        //    return true;
        //else
        //    return false;
    }
}