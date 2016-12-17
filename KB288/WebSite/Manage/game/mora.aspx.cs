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

public partial class Manage_game_mora : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/mora.xml";
    protected long F1 = Convert.ToInt64(ub.GetSub("MoraF1", "/Controls/mora.xml"));
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
        Master.Title = "猜拳管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("猜拳");
        builder.Append(Out.Tab("</div>", "<br />"));

        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        if (uid > 0)
            strWhere += "usid=" + uid + "";

        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Game.Mora> listMora = new BCW.BLL.Game.Mora().GetMoras(pageIndex, pageSize, strWhere, out recordCount);
        if (listMora.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Mora n in listMora)
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
                        TrueText = "/出拳" + MoraType(n.TrueType) + "";
                }
                else
                {
                    TrueText = "/出拳" + MoraType(n.TrueType) + "";
                }
                builder.Append("<a href=\"" + Utils.getUrl("mora.aspx?act=del&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[删]&gt;</a>");
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("mora.aspx?act=view&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "|" + n.Title + "</a>(" + n.Price + "" + bzText + "" + TrueText + ")");
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
        string strOthe = "搜猜拳,mora.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("mora.aspx?act=stat") + "\">赢利分析</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("mora.aspx?act=reset") + "\">重置游戏</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("../xml/moraset.aspx?backurl=" + Utils.PostPage(1) + "") + "\">游戏配置</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("../toplist.aspx?act=top&amp;ptype=8&amp;backurl=" + Utils.PostPage(1) + "") + "\">排行榜单</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

    }

    private void ViewPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-2]\d*$", "1"));
        BCW.Model.Game.Mora model = new BCW.BLL.Game.Mora().GetMora(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "查看猜拳";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("查看猜拳");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("" + DT.FormatDate(model.AddTime, 0) + "<br />");
        builder.Append("开局人:<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.UsName + "</a><br />");
        builder.Append("陷阱:" + model.Title + "<br />");

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
        if (IsSeen)
        {
            builder.Append("出拳:" + MoraType(model.TrueType) + "");
        }
        string bzText = string.Empty;
        if (model.BzType == 0)
            bzText = ub.Get("SiteBz");
        else
            bzText = ub.Get("SiteBz2");

        builder.Append("<br />猜拳:" + model.Price + "" + bzText + "<br />");

        if (model.ReID > 0 && model.State != 4)
        {
            if (model.State == 0)
                builder.Append("等待");

            builder.Append("应战人:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.ReID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.ReName + "(" + model.ReID + ")</a><br />");
            builder.Append("应战:" + MoraType(model.ChooseType) + "<br />");
        }

        if (model.State == 0)
            builder.Append("开奖结果:未开奖");
        else if (model.State == 1 || model.State == 2)
        {
            if (IsWinType(model.ChooseType, model.TrueType) == 2)
                builder.Append("结果:打个平手!");
            else if (IsWinType(model.ChooseType, model.TrueType) == 3)
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
        builder.Append("<a href=\"" + Utils.getUrl("mora.aspx?act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">删除猜拳</a><br />");
        builder.Append("<a href=\"" + Utils.getPage("mora.aspx") + "\">返回上一级</a>");
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
            Master.Title = "删除猜拳";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除该记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("mora.aspx?info=ok&amp;act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("mora.aspx?act=edit&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.BLL.Game.Mora().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            new BCW.BLL.Game.Mora().Delete(id);
            Utils.Success("删除猜拳", "删除成功..", Utils.getPage("mora.aspx"), "1");
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
            builder.Append("确定重置猜拳游戏吗，重置后将删除所有猜拳记录");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("mora.aspx?info=ok&amp;act=reset") + "\">确定重置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("mora.aspx") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            new BCW.Data.SqlUp().ClearTable("tb_Mora");
            Utils.Success("重置游戏", "重置游戏成功..", Utils.getUrl("mora.aspx"), "1");
        }
    }

    private void StatPage()
    {
        Master.Title = "赢利分析";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("赢利分析");
        builder.Append(Out.Tab("</div>", "<br />"));

        double Tar1 = Convert.ToDouble(ub.GetSub("MoraTar1", xmlPath));
        double Tar2 = Convert.ToDouble(ub.GetSub("MoraTar2", xmlPath));
        //今天赢利
        long TodayWinCent1 = new BCW.BLL.Game.Mora().GetPrice("BzType=0 and State>0 and Price<" + F1 + " and Year(AddTime) = " + DateTime.Now.Year + " AND Month(AddTime) = " + DateTime.Now.Month + " and Day(AddTime) = " + DateTime.Now.Day + "");
        long TodayWinCent2 = new BCW.BLL.Game.Mora().GetPrice("BzType=0 and State>0 and Price>=" + F1 + " and Year(AddTime) = " + DateTime.Now.Year + " AND Month(AddTime) = " + DateTime.Now.Month + " and Day(AddTime) = " + DateTime.Now.Day + "");

        long TodayWin1 = Convert.ToInt64(TodayWinCent1 * Tar1 * 0.01);
        long TodayWin2 = Convert.ToInt64(TodayWinCent2 * Tar2 * 0.01);

        //昨天赢利
        long YesWinCent1 = new BCW.BLL.Game.Mora().GetPrice("BzType=0 and State>0 and Price<" + F1 + " and Year(AddTime) = " + DateTime.Now.AddDays(-1).Year + " AND Month(AddTime) = " + DateTime.Now.AddDays(-1).Month + " and Day(AddTime) = " + DateTime.Now.AddDays(-1).Day + " ");
        long YesWinCent2 = new BCW.BLL.Game.Mora().GetPrice("BzType=0 and State>0 and Price>=" + F1 + " and Year(AddTime) = " + DateTime.Now.AddDays(-1).Year + " AND Month(AddTime) = " + DateTime.Now.AddDays(-1).Month + " and Day(AddTime) = " + DateTime.Now.AddDays(-1).Day + " ");

        long YesWin1 = Convert.ToInt64(YesWinCent1 * Tar1 * 0.01);
        long YesWin2 = Convert.ToInt64(YesWinCent2 * Tar2 * 0.01);

        //本月赢利
        long MonthWinCent1 = new BCW.BLL.Game.Mora().GetPrice("BzType=0 and State>0 and Price<" + F1 + " and Year(AddTime) = " + (DateTime.Now.Year) + " AND Month(AddTime) = " + (DateTime.Now.Month) + " ");
        long MonthWinCent2 = new BCW.BLL.Game.Mora().GetPrice("BzType=0 and State>0 and Price>=" + F1 + " and Year(AddTime) = " + (DateTime.Now.Year) + " AND Month(AddTime) = " + (DateTime.Now.Month) + " ");

        long MonthWin1 = Convert.ToInt64(MonthWinCent1 * Tar1 * 0.01);
        long MonthWin2 = Convert.ToInt64(MonthWinCent2 * Tar2 * 0.01);

        //上月赢利
        long Month2WinCent1 = new BCW.BLL.Game.Mora().GetPrice("BzType=0 and State>0 and Price<" + F1 + " and Year(AddTime) = " + (DateTime.Now.Year - DateTime.Now.Day) + " AND Month(AddTime) = " + (DateTime.Now.Month - DateTime.Now.Day) + " ");
        long Month2WinCent2 = new BCW.BLL.Game.Mora().GetPrice("BzType=0 and State>0 and Price>=" + F1 + " and Year(AddTime) = " + (DateTime.Now.Year - DateTime.Now.Day) + " AND Month(AddTime) = " + (DateTime.Now.Month - DateTime.Now.Day) + " ");

        long Month2Win1 = Convert.ToInt64(Month2WinCent1 * Tar1 * 0.01);
        long Month2Win2 = Convert.ToInt64(Month2WinCent2 * Tar2 * 0.01);

        long WinCent1 = new BCW.BLL.Game.Mora().GetPrice("BzType=0 and State>0 and Price<" + F1 + "");
        long WinCent2 = new BCW.BLL.Game.Mora().GetPrice("BzType=0 and State>0 and Price>=" + F1 + "");

        long Win1 = Convert.ToInt64(WinCent1 * Tar1 * 0.01);
        long Win2 = Convert.ToInt64(WinCent2 * Tar2 * 0.01);

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("今天赢利:" + (TodayWin1 + TodayWin2) + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("昨天赢利:" + (YesWin1 + YesWin2) + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("本月赢利:" + (MonthWin1 + MonthWin2) + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("上月赢利:" + (Month2Win1 + Month2Win2) + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("赢利总计:" + (Win1 + Win2) + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("mora.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private string MoraType(int Types)
    {
        string p_Val = string.Empty;
        if (Types == 1)
            p_Val = "剪刀";
        else if (Types == 2)
            p_Val = "石头";
        else
            p_Val = "布";

        return p_Val;
    }

    //闲家输与赢
    private int IsWinType(int ChooseType, int TrueType)
    {
        //1剪刀/2石头/3布
        if (TrueType == ChooseType)
            return 2;//平
        else if (ChooseType == 1 && TrueType == 2)
            return 3;//输
        else if (ChooseType == 1 && TrueType == 3)
            return 1;//赢
        else if (ChooseType == 2 && TrueType == 1)
            return 1;//赢
        else if (ChooseType == 2 && TrueType == 3)
            return 3;//输
        else if (ChooseType == 3 && TrueType == 1)
            return 3;//输
        else if (ChooseType == 3 && TrueType == 2)
            return 1;//赢

        return 0;
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