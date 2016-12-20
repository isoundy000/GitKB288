using System;
using System.Collections;
using System.Collections.Generic;
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
using BCW.HP3;

/// <summary>
/// 蒙宗将 20161004 管理优化
/// 蒙宗将 20161006 重写大小单双赔率、增加投注限额
/// 蒙宗将 20161008 中奖纪录增加中奖注数，每注金额
/// 蒙宗将 20161018 规则XML增加，开奖任选3-6修改
/// 蒙宗将 20161019 增加查询兑奖
/// 蒙宗将 20161028 修复顺子开奖问题 ,修复重开奖
/// 蒙宗将 20161101 修复下注上限
/// 蒙宗将 20161114 开放重置游戏权限
///        20161118 返赢反负完善
///        20161122 直六返奖修复 23 盈利分析
/// </summary>

public partial class Manage_HP3 : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/HappyPoker3.xml";
    ub xml = new ub();
    protected int SWB = Convert.ToInt32(ub.GetSub("SWB", "/Controls/HappyPoker3.xml"));
    protected string GameName = ub.GetSub("HP3Name", "/Controls/HappyPoker3.xml");
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "del":
                DelPage();
                break;
            case "reset":
                ResetPage();
                break;
            case "stat":
                StatPage();
                break;
            case "weihu":
                Weihu();
                break;
            case "set":
                Set();
                break;
            case "BuyWin":
                BuyAndWin();
                break;
            case "OpenCase":
                OpenCase();
                break;
            case "OpenSave":
                OpenSave();
                break;
            case "bang":
                UserBang();
                break;
            case "Back":
                Back();
                break;
            case "backsave":
            case "backsave2":
                BackSavePage(act);
                break;
            case "ByDate":
                ByDate();
                break;
            case "ReBot":
                ReBot();
                break;
            case "ReWard":
                ReWard();
                break;
            case "ReWardCase":
                ReWardCase();
                break;
            case "MeAdd":
                MeAddDate();
                break;
            case "swsz":
                SWSZ();
                break;
            case "case":
                CasePage();//未/已兑奖
                break;
            case "caseok":
                CaseokPage();//帮他兑奖
                break;
            default:
                ReloadPage();
                break;
        }
    }
    //主设置页面
    private void ReloadPage()
    {
        Master.Title = "" + GameName + "";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("开奖号码：");
        builder.Append(Out.Tab("</div>", "<br />"));

        if (SWB == 0)
        {
            #region 正式版
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string strWhere = string.Empty;
            strWhere = "";
            string[] pageValUrl = { "act", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            // 开始读取列表
            IList<BCW.HP3.Model.HP3_kjnum> listHP3 = new BCW.HP3.BLL.HP3_kjnum().GetHP3ListByPage(pageIndex, pageSize, strWhere, out recordCount);
            if (listHP3.Count > 0)
            {
                int k = 1;
                foreach (BCW.HP3.Model.HP3_kjnum n in listHP3)
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
                    n.Fnum = n.Fnum.Trim();
                    n.Snum = n.Snum.Trim();
                    n.Tnum = n.Tnum.Trim();
                    n.Winum = n.Winum.Trim();
                    if (n.Fnum != "null" && n.Snum != "null" && n.Tnum != "null" && n.Winum != "null")
                    {
                        builder.Append("<b>" + n.datenum + "期:</b>" + n.Fnum + n.Snum + n.Tnum);
                    }
                    else
                    {
                        builder.Append("<b>" + n.datenum + "期:</b>");
                        builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=OpenCase&amp;id=" + n.datenum + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[开奖∪兑奖]</a>");
                    }
                    int countess = new BCW.HP3.BLL.HP3Buy().GetRecordCount(" BuyDate='" + n.datenum + "'");
                    if (countess != 0)
                    {
                        builder.Append("|共<a href=\"" + Utils.getUrl("HP3.aspx?act=BuyWin&amp;qihaos=" + n.datenum + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"><b style=\"color:red\">" + countess + "</b></a>张票单");
                    }
                    else
                    {
                        builder.Append("|共<b style=\"color:red\">" + countess + "</b>张票单");
                    }
                    if (n.Fnum != "null" && n.Snum != "null" && n.Tnum != "null" && n.Winum != "null")
                    {
                        builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=OpenCase&amp;ck=1&amp;id=" + n.datenum + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[重开]</a>");
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
            #endregion
        }
        else
        {
            #region
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string strWhere = string.Empty;
            strWhere = "";
            string[] pageValUrl = { "act", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            // 开始读取列表
            IList<BCW.HP3.Model.HP3_kjnum> listHP3 = new BCW.HP3.BLL.HP3_kjnum().GetHP3ListByPage(pageIndex, pageSize, strWhere, out recordCount);
            if (listHP3.Count > 0)
            {
                int k = 1;
                foreach (BCW.HP3.Model.HP3_kjnum n in listHP3)
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
                    n.Fnum = n.Fnum.Trim();
                    n.Snum = n.Snum.Trim();
                    n.Tnum = n.Tnum.Trim();
                    n.Winum = n.Winum.Trim();
                    if (n.Fnum != "null" && n.Snum != "null" && n.Tnum != "null" && n.Winum != "null")
                    {
                        builder.Append("<b>" + n.datenum + "期:</b>" + n.Fnum + n.Snum + n.Tnum);
                    }
                    else
                    {
                        builder.Append("<b>" + n.datenum + "期:</b>");
                        builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=OpenCase&amp;id=" + n.datenum + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[开奖∪兑奖]</a>");
                    }
                    int countess = new BCW.HP3.BLL.HP3BuySY().GetRecordCount(" BuyDate='" + n.datenum + "'");
                    if (countess != 0)
                    {
                        builder.Append("|共<a href=\"" + Utils.getUrl("HP3.aspx?act=BuyWin&amp;qihaos=" + n.datenum + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"><b style=\"color:red\">" + countess + "</b></a>张票单");
                    }
                    else
                    {
                        builder.Append("|共<b style=\"color:red\">" + countess + "</b>张票单");
                    }
                    if (n.Fnum != "null" && n.Snum != "null" && n.Tnum != "null" && n.Winum != "null")
                    {
                        builder.Append("<a  href=\"" + Utils.getUrl("HP3.aspx?act=OpenCase&amp;ck=1&amp;id=" + n.datenum + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[重开]</a>");
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
            #endregion
        }

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        if (SWB == 0)
        {
            #region 正式版
            builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=BuyWin") + "\">用户购买及获奖数据查询</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=stat") + "\">盈利分析</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
            builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=set") + "\">游戏配置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=bang") + "\">用户排行</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
            builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=Back") + "\">返赢返负</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=reset") + "\">重置游戏</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
            builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=weihu") + "\">游戏维护</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=case") + "\">查询兑奖</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=ReBot") + "\">机器人设置</a>");
            #endregion
        }
        else
        {
            #region
            builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=BuyWin") + "\">用户购买及获奖数据查询</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=stat") + "\">奖励记录</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
            builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=set") + "\">游戏配置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=bang") + "\">用户排行</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
            builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=weihu") + "\">游戏维护</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=swsz") + "\">试玩设置</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
            builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=reset") + "\">重置游戏</a><br />");
            #endregion
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    //删除页面
    private void DelPage()
    {
        if (SWB == 0)
        {
            #region 正式版
            string info = Utils.GetRequest("info", "all", 1, "", "");
            int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
            if (info == "")
            {
                Master.Title = "退回订单";
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("确定退回该记录吗");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?info=ok&amp;act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=edit&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            else
            {
                if (!new BCW.HP3.BLL.HP3Winner().Exists(id) && !new BCW.HP3.BLL.HP3Buy().Exists(id))
                {
                    Utils.Error("不存在的记录", "");
                }
                try
                {
                    DataSet WinId = new BCW.HP3.BLL.HP3Winner().GetList("ID=" + id);
                    if (Convert.ToInt32(WinId.Tables[0].Rows[0][4]) == 0)
                    {
                        int userid = Convert.ToInt32(WinId.Tables[0].Rows[0][1]);
                        long money = Convert.ToInt64(WinId.Tables[0].Rows[0][3]);
                        long Price = -money;
                        new BCW.BLL.User().UpdateiGold(userid, Price, "非法获取返回系统");
                    }
                    new BCW.HP3.BLL.HP3Winner().Delete(id);
                    new BCW.HP3.BLL.HP3Buy().Delete(id);
                }
                catch
                {
                    DataSet BuyId = new BCW.HP3.BLL.HP3Buy().GetList("*", "ID=" + id);
                    int BuyID = Convert.ToInt32(BuyId.Tables[0].Rows[0][1]);
                    long BuyMoney = Convert.ToInt64(BuyId.Tables[0].Rows[0][5]) * Convert.ToInt32(BuyId.Tables[0].Rows[0][6]);
                    string mename = new BCW.BLL.User().GetUsName(BuyID);
                    new BCW.BLL.User().UpdateiGold(BuyID, BuyMoney, "" + GameName + "第" + BuyId.Tables[0].Rows[0][2] + "期错误订单" + id + "返还");
                    new BCW.BLL.Guest().Add(1, BuyID, mename, "您的" + GameName + "无效订单已返还，获得了" + BuyMoney + "酷币。");
                    new BCW.HP3.BLL.HP3Buy().Delete(id);
                }

                Utils.Success("删除订单", "删除成功..", Utils.getPage("HP3.aspx"), "1");
            }
            #endregion
        }
        else
        {
            #region
            string info = Utils.GetRequest("info", "all", 1, "", "");
            int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
            if (info == "")
            {
                Master.Title = "删除订单";
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("确定删除该记录吗");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?info=ok&amp;act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=edit&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            else
            {
                if (!new BCW.HP3.BLL.HP3WinnerSY().Exists(id) && !new BCW.HP3.BLL.HP3BuySY().Exists(id))
                {
                    Utils.Error("不存在的记录", "");
                }
                try
                {
                    DataSet WinId = new BCW.HP3.BLL.HP3WinnerSY().GetList("ID=" + id);
                    if (Convert.ToInt32(WinId.Tables[0].Rows[0][4]) == 0)
                    {
                        int userid = Convert.ToInt32(WinId.Tables[0].Rows[0][1]);
                        long money = Convert.ToInt64(WinId.Tables[0].Rows[0][3]);
                        long Price = -money;
                        new BCW.HP3.BLL.SWB().UpdateHP3Money(userid, Price);
                    }
                    new BCW.HP3.BLL.HP3WinnerSY().Delete(id);
                    new BCW.HP3.BLL.HP3BuySY().Delete(id);
                }
                catch
                {
                    DataSet BuyId = new BCW.HP3.BLL.HP3BuySY().GetList("*", "ID=" + id);
                    int BuyID = Convert.ToInt32(BuyId.Tables[0].Rows[0][1]);
                    long BuyMoney = Convert.ToInt64(BuyId.Tables[0].Rows[0][5]) * Convert.ToInt32(BuyId.Tables[0].Rows[0][6]);
                    string mename = new BCW.BLL.User().GetUsName(BuyID);
                    new BCW.HP3.BLL.SWB().UpdateHP3Money(BuyID, BuyMoney);
                    new BCW.BLL.Guest().Add(1, BuyID, mename, "您的" + GameName + "无效订单已返还，获得了" + BuyMoney + "快乐币。");
                    new BCW.HP3.BLL.HP3BuySY().Delete(id);
                }

                Utils.Success("删除订单", "删除成功..", Utils.getPage("HP3.aspx"), "1");
            }
            #endregion
        }
    }
    //重置游戏
    private void ResetPage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId != 1 && ManageId != 9)
        {
            Utils.Error("权限不足", "");
        }

        if (SWB == 0)
        {
            #region 正式版
            string info = Utils.GetRequest("info", "all", 1, "", "");
            if (info == "")
            {
                Master.Title = "重置游戏";
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("确定重置" + GameName + "吗，重置后将删除所有数据库记录");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?info=ok&amp;act=reset") + "\">确定重置</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx") + "\">再看看吧..</a>");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            else
            {
                new BCW.HP3.BLL.HP3_kjnum().ClearTable("tb_HP3Winner");
                new BCW.HP3.BLL.HP3_kjnum().ClearTable("tb_HP3Buy");
                new BCW.HP3.BLL.HP3_kjnum().ClearTable("tb_HP3_kjnum");
                Utils.Success("重置游戏", "重置游戏成功..", Utils.getUrl("HP3.aspx"), "1");
            }
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
            #endregion
        }
        else
        {
            #region
            string info = Utils.GetRequest("info", "all", 1, "", "");
            if (info == "")
            {
                Master.Title = "重置游戏";
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("确定重置" + GameName + "吗，重置后将删除所有数据库记录");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?info=ok&amp;act=reset") + "\">确定重置</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx") + "\">再看看吧..</a>");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            else
            {
                new BCW.HP3.BLL.HP3_kjnum().ClearTable("tb_HP3WinnerSY");
                new BCW.HP3.BLL.HP3_kjnum().ClearTable("tb_HP3BuySY");
                new BCW.HP3.BLL.HP3_kjnum().ClearTable("tb_HP3_kjnum");
                Utils.Success("重置游戏", "重置游戏成功..", Utils.getUrl("HP3.aspx"), "1");
            }
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
            #endregion
        }
    }
    //营业分析
    private void StatPage()
    {
        if (SWB == 0)
        {
            #region 正式版
            Master.Title = "盈利分析";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
            builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>&gt;");
            builder.Append("盈利分析");
            builder.Append(Out.Tab("</div>", "<br />"));

            string searchday = Utils.GetRequest("inputdate", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02(0[1-9]|[1][0-9]|2[0-8])))$", DateTime.Now.ToString("yyyyMMdd"));
            if (searchday == "")
            {
                searchday = DateTime.Now.ToString("yyyyMMdd");
            }
            string strText = "按日期查询：格式（如" + DateTime.Now.ToString("yyyyMMdd") + "）/";
            string strName = "inputdate";
            string strType = "num";
            string strValu = searchday;
            string strEmpt = "";
            string strIdea = "";
            string strOthe = "查询,HP3.aspx?act=stat&amp; ,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("" + "-----------" + "<br />");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("统计数据：<b>" + DateTime.ParseExact(strValu, "yyyyMMdd", null).ToString("yyyy-MM-dd") + "</b><br />");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("" + "-----------" + "<br />");
            builder.Append(Out.Tab("</div>", ""));
            DataSet Tbuymoney = new BCW.HP3.BLL.HP3Buy().GetMoney(strValu);
            DataSet Twinmoney = new BCW.HP3.BLL.HP3Winner().GetMoney(strValu);
            double TodayBuy = 0;
            try
            {
                TodayBuy = Convert.ToDouble(Tbuymoney.Tables[0].Rows[0][0]);
            }
            catch
            {
                TodayBuy = 0;
            }
            double TodayWin = 0;
            try
            {
                TodayWin = Convert.ToDouble(Twinmoney.Tables[0].Rows[0][0]);
            }
            catch
            {
                TodayWin = 0;
            }
            double TodayGet = TodayBuy - TodayWin;
            string mont = strValu.Substring(0, 6);
            DataSet Mbuymoney = new BCW.HP3.BLL.HP3Buy().GetMoney(mont);
            DataSet Mwinmoney = new BCW.HP3.BLL.HP3Winner().GetMoney(mont);
            double MonthBuy = 0;
            try
            {
                MonthBuy = Convert.ToDouble(Mbuymoney.Tables[0].Rows[0][0]);
            }
            catch
            {
                MonthBuy = 0;
            }
            double MonthWin = 0;
            try
            {
                MonthWin = Convert.ToDouble(Mwinmoney.Tables[0].Rows[0][0]);
            }
            catch
            {
                MonthWin = 0;
            }
            double MonthGet = MonthBuy - MonthWin;
            DataSet Hbuymoney = new BCW.HP3.BLL.HP3Buy().GetMoney2("20001212", DateTime.Now.ToString("yyyyMMdd"));
            DataSet Hwinmoney = new BCW.HP3.BLL.HP3Winner().GetMoney("");
            double AllBuy = 0;
            try
            {
                AllBuy = Convert.ToDouble(Hbuymoney.Tables[0].Rows[0][0]);
            }
            catch
            {
                AllBuy = 0;
            }
            double AllWin = 0;
            try
            {
                AllWin = Convert.ToDouble(Hwinmoney.Tables[0].Rows[0][0]);
            }
            catch
            {
                AllWin = 0;
            }
            double AllGet = AllBuy - AllWin;
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("当日投注总金额:<b style=\"color:red\">" + TodayBuy + "</b>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("当日兑奖总金额:<b style=\"color:red\">" + TodayWin + "</b>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("当日总酷币赢利:<b style=\"color:red\">" + TodayGet + "</b>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("" + "-----------" + "<br />");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("当月投注总金额:<b style=\"color:red\">" + MonthBuy + "</b>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("当月兑奖总金额:<b style=\"color:red\">" + MonthWin + "</b>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("当月总酷币赢利:<b style=\"color:red\">" + MonthGet + "</b>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("" + "-----------" + "<br />");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("历史投注总金额:<b style=\"color:red\">" + AllBuy + "</b>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("历史兑奖总金额:<b style=\"color:red\">" + AllWin + "</b>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("游戏历史总收入:<b style=\"color:red\">" + AllGet + "</b>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("" + "-----------" + "<br />");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("HP3.aspx?act=ByDate") + "\">按时间段查询</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
            #endregion
        }
        else
        {
            #region 试玩版
            Master.Title = "奖励记录";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
            builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>&gt;");
            builder.Append("奖励记录");
            builder.Append(Out.Tab("</div>", "<br />"));

            string searchday = Utils.GetRequest("inputdate", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8])))$", DateTime.Now.ToString("yyyyMMdd"));
            if (searchday == "")
            {
                searchday = DateTime.Now.ToString("yyyyMMdd");
            }
            string strText = "按日期查询：格式（如" + DateTime.Now.ToString("yyyyMMdd") + "）/";
            string strName = "inputdate";
            string strType = "num";
            string strValu = searchday;
            string strEmpt = "";
            string strIdea = "";
            string strOthe = "查询,HP3.aspx?act=stat&amp; ,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append("<hr />");
            string swhere = DateTime.ParseExact(strValu, "yyyyMMdd", null).ToString("yyyy-MM-dd");
            builder.Append("统计数据：<b>" + swhere + "</b><br />");
            builder.Append("<hr />");
            long TodayPut = 0;
            long MonthPut = 0;
            long AllPut = 0;
            DataSet todayputs = new BCW.HP3.BLL.SWB().ReWardList(swhere, swhere, swhere);
            int todaycount = new BCW.HP3.BLL.SWB().ReWardCount(swhere, swhere, swhere);
            for (int zg = 0; zg < todaycount; zg++)
            {
                string todayput = todayputs.Tables[0].Rows[zg][0].ToString();
                Match tput = Regex.Match(todayput, @"[\d]{1,}酷币$");
                Match tputs = Regex.Match(tput.Value, @"[\d]{1,}");
                TodayPut = TodayPut + Convert.ToInt64(tputs.Value);
            }
            DataSet monthputs = new BCW.HP3.BLL.SWB().ReWardList(swhere, swhere, "");
            int monthcount = new BCW.HP3.BLL.SWB().ReWardCount(swhere, swhere, "");
            for (int zj = 0; zj < monthcount; zj++)
            {
                string monthput = monthputs.Tables[0].Rows[zj][0].ToString();
                Match tput = Regex.Match(monthput, @"[\d]{1,}酷币$");
                Match tputs = Regex.Match(tput.Value, @"[\d]{1,}");
                MonthPut = MonthPut + Convert.ToInt64(tputs.Value);
            }
            DataSet allputs = new BCW.HP3.BLL.SWB().ReWardList("", "", "");
            int allcount = new BCW.HP3.BLL.SWB().ReWardCount("", "", "");
            for (int dy = 0; dy < allcount; dy++)
            {
                string allput = allputs.Tables[0].Rows[dy][0].ToString();
                Match tput = Regex.Match(allput, @"[\d]{1,}酷币$");
                Match tputs = Regex.Match(tput.Value, @"[\d]{1,}");
                AllPut = AllPut + Convert.ToInt64(tputs.Value);
            }
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("当日派奖总金额:<b style=\"color:red\">" + TodayPut + "</b>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("当月派奖总金额:<b style=\"color:red\">" + MonthPut + "</b>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("历史派奖总金额:<b style=\"color:red\">" + AllPut + "</b>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("HP3.aspx") + "\">返回上一级</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
            #endregion
        }
    }
    //按期号间隔查询
    private void ByDate()
    {
        Master.Title = "按期号间隔查询";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=stat&amp; ") + "\">盈利分析</a>&gt;");
        builder.Append("按期号间隔查询");
        builder.Append(Out.Tab("</div>", "<br />"));

        string searchday = Utils.GetRequest("inputdate", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02(0[1-9]|[1][0-9]|2[0-8])))", DateTime.Now.ToString("yyyyMMdd") + "01");
        string searchday2 = Utils.GetRequest("inputdate2", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02(0[1-9]|[1][0-9]|2[0-8])))", DateTime.Now.ToString("yyyyMMdd") + "79");
        if (searchday == "")
        {
            searchday = DateTime.Now.ToString("yyyyMMdd") + "01";
        }
        if (searchday2 == "")
        {
            searchday = DateTime.Now.ToString("yyyyMMdd") + "79";
        }

        string strText = "开始期号：格式（如" + DateTime.Now.ToString("yyyyMMdd") + "01）/,截至期号：格式（如" + DateTime.Now.ToString("yyyyMMdd") + "79）/";
        string strName = "inputdate,inputdate2";
        string strType = "num,num";
        string strValu = searchday + "'" + searchday2;
        string strEmpt = "true,true";
        string strIdea = "";
        string strOthe = "查询,HP3.aspx?act=ByDate&amp; ,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("" + "-----------" + "<br />");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("统计数据：<b>" + searchday + "</b>期到<b>" + searchday2 + "</b><br />");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("" + "-----------" + "<br />");
        builder.Append(Out.Tab("</div>", ""));
        DataSet Hbuymoney = new BCW.HP3.BLL.HP3Buy().GetMoney2(searchday, searchday2);
        DataSet Hwinmoney = new BCW.HP3.BLL.HP3Winner().GetMoney2(searchday, searchday2);
        double AllBuy = 0;
        try
        {
            AllBuy = Convert.ToDouble(Hbuymoney.Tables[0].Rows[0][0]);
        }
        catch
        {
            AllBuy = 0;
        }
        double AllWin = 0;
        try
        {
            AllWin = Convert.ToDouble(Hwinmoney.Tables[0].Rows[0][0]);
        }
        catch
        {
            AllWin = 0;
        }
        double AllGet = AllBuy - AllWin;
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("酷币入账:<b style=\"color:red\">" + AllBuy + "</b>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("酷币兑出:<b style=\"color:red\">" + AllWin + "</b>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("酷币赢利:<b style=\"color:red\">" + AllGet + "</b>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("HP3.aspx?act=stat&amp;") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    //特殊选择返回
    private string speChoose(int Types)
    {
        string s1 = string.Empty;
        if (Types == 1)
            s1 = "同花投注:<b>黑桃同花</b>";
        else if (Types == 2)
            s1 = "同花投注:<b style=\"color:red\">红桃同花</b>";
        else if (Types == 3)
            s1 = "同花投注:<b>梅花同花</b>";
        else if (Types == 4)
            s1 = "同花投注:<b style=\"color:red\">方块同花</b>";
        else if (Types == 5)
            s1 = "同花投注:<b style=\"color:#8A7B66\">同花全包</b>";
        else if (Types == 6)
            s1 = "顺子投注:<b>A23</b>";
        else if (Types == 7)
            s1 = "顺子投注:<b>234</b>";
        else if (Types == 8)
            s1 = "顺子投注:<b>345</b>";
        else if (Types == 9)
            s1 = "顺子投注:<b>456</b>";
        else if (Types == 10)
            s1 = "顺子投注:<b>567</b>";
        else if (Types == 11)
            s1 = "顺子投注:<b>678</b>";
        else if (Types == 12)
            s1 = "顺子投注:<b>789</b>";
        else if (Types == 13)
            s1 = "顺子投注:<b>8910</b>";
        else if (Types == 14)
            s1 = "顺子投注:<b>910J</b>";
        else if (Types == 15)
            s1 = "顺子投注:<b>10JQ</b>";
        else if (Types == 16)
            s1 = "顺子投注:<b>JQK</b>";
        else if (Types == 17)
            s1 = "顺子投注:<b>QKA</b>";
        else if (Types == 18)
            s1 = "顺子投注:<b style=\"color:#8A7B66\">顺子全包</b>";
        else if (Types == 19)
            s1 = "同花顺投注:<b>黑桃同花顺</b>";
        else if (Types == 20)
            s1 = "同花顺投注:<b style=\"color:red\">红桃同花顺</b>";
        else if (Types == 21)
            s1 = "同花顺投注:<b>梅花同花顺</b>";
        else if (Types == 22)
            s1 = "同花顺投注:<b style=\"color:red\">方块同花顺</b>";
        else if (Types == 23)
            s1 = "同花顺投注:<b style=\"color:#8A7B66\">同花顺全包</b>";
        else if (Types == 24)
            s1 = "同花顺投注:<b>AAA</b>";
        else if (Types == 25)
            s1 = "豹子投注:<b>222</b>";
        else if (Types == 26)
            s1 = "豹子投注;<b>333</b>";
        else if (Types == 27)
            s1 = "豹子投注;<b>444</b>";
        else if (Types == 28)
            s1 = "豹子投注:<b>555</b>";
        else if (Types == 29)
            s1 = "豹子投注:<b>666</b>";
        else if (Types == 30)
            s1 = "豹子投注:<b>777</b>";
        else if (Types == 31)
            s1 = "豹子投注:<b>888</b>";
        else if (Types == 32)
            s1 = "豹子投注;<b>999</b>";
        else if (Types == 33)
            s1 = "豹子投注;<b>101010</b>";
        else if (Types == 34)
            s1 = "豹子投注:<b>JJJ</b>";
        else if (Types == 35)
            s1 = "豹子投注:<b>QQQ</b>";
        else if (Types == 36)
            s1 = "豹子投注:<b>KKK</b>";
        else if (Types == 37)
            s1 = "豹子投注:<b style=\"color:#8A7B66\">豹子全包</b>";
        else if (Types == 38)
            s1 = "对子投注:<b>AA</b>";
        else if (Types == 39)
            s1 = "对子投注:<b>22</b>";
        else if (Types == 40)
            s1 = "对子投注:<b>33</b>";
        else if (Types == 41)
            s1 = "对子投注:<b>44</b>";
        else if (Types == 42)
            s1 = "对子投注:<b>55</b>";
        else if (Types == 43)
            s1 = "对子投注:<b>66</b>";
        else if (Types == 44)
            s1 = "对子投注:<b>77</b>";
        else if (Types == 45)
            s1 = "对子投注:<b>88</b>";
        else if (Types == 46)
            s1 = "对子投注:<b>99</b>";
        else if (Types == 47)
            s1 = "对子投注:<b>1010</b>";
        else if (Types == 48)
            s1 = "对子投注:<b>JJ</b>";
        else if (Types == 49)
            s1 = "对子投注:<b>QQ</b>";
        else if (Types == 50)
            s1 = "对子投注:<b>KK</b>";
        else if (Types == 51)
            s1 = "对子投注:<b style=\"color:#8A7B66\">对子全包</b>";
        return s1;
    }
    //数字转类型
    private string NumToType(int num)
    {
        string type = "";
        switch (num)
        {
            case 1:
                type = "";
                break;
            case 6:
                type = "任一直选";
                break;
            case 7:
                type = "任二直选";
                break;
            case 8:
                type = "任二胆拖";
                break;
            case 9:
                type = "任三直选";
                break;
            case 10:
                type = "任三胆拖";
                break;
            case 11:
                type = "任四直选";
                break;
            case 12:
                type = "任四胆拖";
                break;
            case 13:
                type = "任五直选";
                break;
            case 14:
                type = "任五胆拖";
                break;
            case 15:
                type = "任六直选";
                break;
            case 16:
                type = "任六胆拖";
                break;
            case 17:
                type = "大小单双";
                break;
        }
        return type;
    }
    //游戏维护
    private void Weihu()
    {
        if (SWB == 0)
        {
            #region 正式版
            Master.Title = "游戏维护";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
            builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>&gt;");
            builder.Append("游戏维护");
            builder.Append(Out.Tab("</div>", "<br />"));

            Application.Remove(xmlPath);//清缓存
            xml.ReloadSub(xmlPath); //加载配置
            string Status = Utils.GetRequest("Status", "post", 1, @"^[0-2]$", "0");
            string HtestID = Utils.GetRequest("HtestID", "all", 1, "", "");
            string ac = Utils.GetRequest("ac", "all", 1, "", "");
            if (ac == "确定修改")
            {
                xml.dss["HP3Status"] = Status;
                xml.dss["HtestID"] = HtestID;
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            }
            string strText = "游戏状态:/,内测ID:/,";
            string strName = "Status,HtestID,backurl";
            string strType = "select,textarea,hidden";
            string strValu = xml.dss["HP3Status"].ToString() + "'" + xml.dss["HtestID"] + "'" + Utils.getPage(0) + "";
            string strEmpt = "0|正常|1|维护|2|内测,true,false";
            string strIdea = "/";
            string strOthe = "确定修改,HP3.aspx?act=weihu,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            if (xml.dss["HP3Status"].ToString() == "1")
            {
                builder.Append("<b style=\"color:red\">游戏已进入维护状态！</b>");
            }
            else if (xml.dss["HP3Status"].ToString() == "2")
            {
                builder.Append("<b style=\"color:red\">游戏内测中！</b>");
            }
            else
            {
                builder.Append("<b style=\"color:red\">游戏正常工作中！</b>");
            }
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("温馨提示：多个内测ID请用#分隔<br />");
            string aa = xml.dss["HtestID"].ToString();
            if (aa == "")
            {
                builder.Append("<b>(提示：共有0个试玩ID.请输入试玩ID.)</b><br />");
            }
            else
            {
                string[] sNum = Regex.Split(aa, "#");
                int s = 0;
                foreach (char item in aa)
                {
                    if (item != 35)
                    {
                    }
                    else
                    {
                        s++;
                    }
                }
                builder.Append("<b>(提示：共有《" + sNum.Length + "》个内测ID)</b><br />当前试玩ID为：<br />");
                string[] name = aa.Split('#');
                for (int n = 0; n < name.Length; n++)
                {
                    if ((n + 1) % 5 == 0)
                    {
                        builder.Append(name[n] + "," + "<br />");
                    }
                    else
                    {
                        builder.Append(name[n] + ",");
                    }
                }
            }
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("HP3.aspx") + "\">返回上一级</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
            #endregion
        }
        else
        {
            #region
            Master.Title = "游戏维护";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
            builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>&gt;");
            builder.Append("游戏维护");
            builder.Append(Out.Tab("</div>", "<br />"));

            Application.Remove(xmlPath);//清缓存
            xml.ReloadSub(xmlPath); //加载配置
            string Status = Utils.GetRequest("Status", "post", 1, @"^[0-1]$", "0");
            xml.dss["HP3Status"] = Status;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            string strText = "游戏状态:/";
            string strName = "Status";
            string strType = "select";
            string strValu = xml.dss["HP3Status"].ToString();
            string strEmpt = "0|正常|1|维护";
            string strIdea = "/";
            string strOthe = "确定修改,HP3.aspx?act=weihu&amp; ,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            if (strValu == "1")
            {
                builder.Append("<b style=\"color:red\">游戏已进入维护状态！</b>");
            }
            else
            {
                builder.Append("<b style=\"color:red\">游戏正常工作中！</b>");
            }


            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("HP3.aspx") + "\">返回上一级</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
            #endregion
        }
    }
    //用户购买及获奖数据查询
    private void BuyAndWin()
    {
        if (SWB == 0)
        {
            #region 正式版
            Master.Title = "用户购买及获奖记录查询";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
            builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>&gt;");
            builder.Append("用户购买及获奖记录查询");
            builder.Append(Out.Tab("</div>", "<br />"));
            int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "2"));
            int qihaos = int.Parse(Utils.GetRequest("qihaos", "all", 1, @"^[1-9]\d*$", "0"));
            builder.Append(Out.Tab("<div>", ""));
            if (ptype == 2)
                builder.Append("下注|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=BuyWin&amp;ptype=2&amp;qihaos=" + qihaos + "") + "\">下注</a>|");
            if (ptype == 1)
                builder.Append("中奖");
            else
                builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=BuyWin&amp;ptype=1&amp;qihaos=" + qihaos + "") + "\">中奖</a>");
            builder.Append(Out.Tab("</div>", "<br />"));

            if (ptype == 1)
            {
                int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));

                int pageIndex;
                int recordCount;
                string strWhere = string.Empty;
                int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
                if (uid > 0)
                {
                    strWhere += "WinUserID=" + uid + "";
                    if (qihaos > 0)
                    {
                        strWhere += " and WinDate=" + qihaos + "";
                    }
                }
                else if (qihaos > 0)
                {
                    strWhere += "WinDate=" + qihaos + "";
                }
                string[] pageValUrl = { "act", "uid", "qihaos", "ptype", "backurl" };
                pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                if (pageIndex == 0)
                    pageIndex = 1;
                // 开始读取列表
                string HP3qi = "";
                IList<BCW.HP3.Model.HP3Winner> listHP3Win = new BCW.HP3.BLL.HP3Winner().GetListNes(pageIndex, pageSize, strWhere, out recordCount);
                if (listHP3Win.Count > 0)
                {
                    int k = 1;

                    foreach (BCW.HP3.Model.HP3Winner n in listHP3Win)
                    {
                        if (n.WinDate.ToString() != HP3qi)
                        {
                            builder.Append(Out.Tab("<div>", "<br/>"));
                            BCW.HP3.Model.HP3_kjnum wins = new BCW.HP3.BLL.HP3_kjnum().GetDataByState(n.WinDate);
                            if (wins.Fnum.Trim() != "null")
                                builder.Append("=第" + n.WinDate + "期=" + wins.Fnum + wins.Snum + wins.Tnum + "");
                            else
                                builder.Append("=第" + n.WinDate + "期=");
                            builder.Append(Out.Tab("</div>", ""));
                        }
                        if (k % 2 == 0)
                            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                        else
                        {
                            if (k == 1)
                                builder.Append(Out.Tab("<div>", "<br />"));
                            else
                                builder.Append(Out.Tab("<div>", "<br />"));
                        }
                        builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=del&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[退]&gt;</a>");
                        string TF = "";
                        if (n.WinBool == 1)
                        {
                            TF = "未";
                        }
                        else
                        {
                            TF = "已";
                        }
                        BCW.HP3.Model.HP3_kjnum moop = new BCW.HP3.BLL.HP3_kjnum().GetDataByState(n.WinDate);
                        string name = new BCW.BLL.User().GetUsName(n.WinUserID);
                        BCW.HP3.Model.HP3Buy m = new BCW.HP3.BLL.HP3Buy().GetModel(n.ID);
                        builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".[编号" + n.ID + "]<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.WinUserID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + name + "</a>（" + n.WinUserID + "）：" + NumToType(m.BuyType));

                        if (m.BuyType == 1)
                        {
                            builder.Append(speChoose(Convert.ToInt32(m.BuyNum)));
                            builder.Append(",每注" + m.BuyMoney + ub.Get("SiteBz") + "");
                        }
                        else if (m.BuyType == 17)
                        {
                            switch (m.BuyNum.Trim())
                            {
                                case "1":
                                    builder.Append(":买大");
                                    break;
                                case "2":
                                    builder.Append(":买小");
                                    break;
                                case "3":
                                    builder.Append(":买单");
                                    break;
                                case "4":
                                    builder.Append(":买双");
                                    break;

                            }
                            builder.Append(",每注" + m.BuyMoney + ub.Get("SiteBz") + "");
                        }
                        else
                        {
                            builder.Append(":选号（" + m.BuyNum + "）,每注" + m.BuyMoney + ub.Get("SiteBz") + "");
                        }
                        builder.Append(",中<b>" + n.WinZhu + "</b>注,赔率" + m.Odds + ",共赢<b>" + n.WinMoney + "</b>酷币," + TF + "兑奖");
                        HP3qi = n.WinDate.ToString();
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
            else
            {
                int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
                int pageIndex;
                int recordCount;
                string strWhere = string.Empty;
                int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
                if (uid > 0)
                {
                    strWhere += "BuyID=" + uid + "";
                    if (qihaos > 0)
                    {
                        strWhere += " and BuyDate=" + qihaos + "";
                    }
                }
                else if (qihaos > 0)
                {
                    strWhere += "BuyDate=" + qihaos + "";
                }
                string[] pageValUrl = { "act", "uid", "qihaos", "ptype", "backurl" };
                pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                if (pageIndex == 0)
                    pageIndex = 1;
                // 开始读取列表
                string HP3qi = "";
                IList<BCW.HP3.Model.HP3Buy> listHP3Buy = new BCW.HP3.BLL.HP3Buy().GetHP3ListByPage(pageIndex, pageSize, strWhere, out recordCount);
                if (listHP3Buy.Count > 0)
                {
                    int k = 1;
                    foreach (BCW.HP3.Model.HP3Buy n in listHP3Buy)
                    {
                        if (n.BuyDate.ToString() != HP3qi)
                        {
                            builder.Append(Out.Tab("<div>", "<br/>"));
                            builder.Append("=第" + n.BuyDate + "期=");
                            try
                            {
                                BCW.HP3.Model.HP3_kjnum wins = new BCW.HP3.BLL.HP3_kjnum().GetDataByState(n.BuyDate);
                                if (wins.Fnum.Trim() != "null")
                                    builder.Append(wins.Fnum + wins.Snum + wins.Tnum + "");
                            }
                            catch
                            {

                            }
                            builder.Append(Out.Tab("</div>", ""));
                        }
                        if (k % 2 == 0)
                            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                        else
                        {
                            if (k == 1)
                                builder.Append(Out.Tab("<div>", "<br />"));
                            else
                                builder.Append(Out.Tab("<div>", "<br />"));
                        }
                        builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=del&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[退]&gt;</a>");
                        string name = new BCW.BLL.User().GetUsName(n.BuyID);
                        //
                        builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".编号[" + n.ID + "]<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.BuyID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + name + "</a>(" + n.BuyID + ")," + NumToType(n.BuyType));
                        if (n.BuyType == 1)
                        {
                            builder.Append(speChoose(Convert.ToInt32(n.BuyNum)));
                        }
                        else if (n.BuyType == 17)
                        {
                            switch (n.BuyNum.Trim())
                            {
                                case "1":
                                    builder.Append(":买大");
                                    break;
                                case "2":
                                    builder.Append(":买小");
                                    break;
                                case "3":
                                    builder.Append(":买单");
                                    break;
                                case "4":
                                    builder.Append(":买双");
                                    break;

                            }
                        }
                        else
                        {
                            builder.Append(":选号（" + n.BuyNum + "）");
                        }
                        builder.Append(",每注 <b>" + n.BuyMoney + " </b>酷币,共<b>" + n.BuyZhu + "</b>注,赔率" + n.Odds + "");
                        if (new BCW.HP3.BLL.HP3Winner().Exists(n.ID))
                        {
                            BCW.HP3.Model.HP3Winner swin = new BCW.HP3.BLL.HP3Winner().GetModel(n.ID);
                            builder.Append(",中奖 <b>" + swin.WinZhu + " </b>注,共赢<b>" + swin.WinMoney + "</b>酷币.");
                        }
                        HP3qi = n.BuyDate.ToString();
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

            string strText = "输入用户ID(可为空):/,输入彩票期号(可为空):/,";
            string strName = "uid,qihaos,backurl";
            string strType = "num,num,hidden";
            string strValu = "'" + "'" + Utils.getPage(0) + "";
            string strEmpt = "true,true,false";
            string strIdea = "/";
            string strOthe = "搜一搜,HP3.aspx?act=BuyWin&amp;ptype=" + ptype + ",post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            #endregion
        }
        else
        {
            #region
            Master.Title = "用户购买及获奖数据查询";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
            builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>&gt;");
            builder.Append("用户购买及获奖数据查询");
            builder.Append(Out.Tab("</div>", "<br />"));
            int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "2"));
            int qihaos = int.Parse(Utils.GetRequest("qihaos", "all", 1, @"^[1-9]\d*$", "0"));
            builder.Append(Out.Tab("<div>", ""));
            if (ptype == 1)
                builder.Append("中奖|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=BuyWin&amp;ptype=1&amp;qihaos=" + qihaos) + "\">中奖</a>|");

            if (ptype == 2)
                builder.Append("下注");
            else
                builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=BuyWin&amp;ptype=2&amp;qihaos=" + qihaos) + "\">下注</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            if (ptype == 1)
            {
                int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
                int pageIndex;
                int recordCount;
                string strWhere = string.Empty;
                int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
                if (uid > 0)
                {
                    strWhere += "WinUserID=" + uid + "";
                    if (qihaos > 0)
                    {
                        strWhere += " and WinDate=" + qihaos + "";
                    }
                }
                else if (qihaos > 0)
                {
                    strWhere += "WinDate=" + qihaos + "";
                }
                string[] pageValUrl = { "act", "uid", "qihaos", "ptype", "backurl" };
                pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                if (pageIndex == 0)
                    pageIndex = 1;
                // 开始读取列表
                string HP3qi = "";
                IList<BCW.HP3.Model.HP3WinnerSY> listHP3Win = new BCW.HP3.BLL.HP3WinnerSY().GetListNes(pageIndex, pageSize, strWhere, out recordCount);
                if (listHP3Win.Count > 0)
                {
                    int k = 1;
                    foreach (BCW.HP3.Model.HP3WinnerSY n in listHP3Win)
                    {
                        if (n.WinDate.ToString() != HP3qi)
                        {
                            builder.Append(Out.Tab("<div>", ""));
                            BCW.HP3.Model.HP3_kjnum wins = new BCW.HP3.BLL.HP3_kjnum().GetDataByState(n.WinDate);
                            if (wins.Fnum.Trim() != "null")
                                builder.Append("=第" + n.WinDate + "期=" + wins.Fnum + wins.Snum + wins.Tnum + "");
                            else
                                builder.Append("=第" + n.WinDate + "期=");
                            builder.Append(Out.Tab("</div>", "<br/>"));
                        }
                        if (k % 2 == 0)
                            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                        else
                        {
                            if (k == 1)
                                builder.Append(Out.Tab("<div>", ""));
                            else
                                builder.Append(Out.Tab("<div>", "<br />"));
                        }
                        builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=del&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[删]&gt;</a>");
                        string TF = "";
                        if (n.WinBool == 1)
                        {
                            TF = "未";
                        }
                        else
                        {
                            TF = "已";
                        }
                        BCW.HP3.Model.HP3_kjnum moop = new BCW.HP3.BLL.HP3_kjnum().GetDataByState(n.WinDate);
                        string name = new BCW.BLL.User().GetUsName(n.WinUserID);
                        builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".[编号" + n.ID + "]<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.WinUserID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + name + "</a>（" + n.WinUserID + ")：中<b>" + n.WinZhu + "</b>注，共赢<b>" + n.WinMoney + "</b>快乐币，" + TF + "兑奖");
                        HP3qi = n.WinDate.ToString();
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
            else
            {
                int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
                int pageIndex;
                int recordCount;
                string strWhere = string.Empty;
                int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
                if (uid > 0)
                {
                    strWhere += "BuyID=" + uid + "";
                    if (qihaos > 0)
                    {
                        strWhere += " and BuyDate=" + qihaos + "";
                    }
                }
                else if (qihaos > 0)
                {
                    strWhere += "BuyDate=" + qihaos + "";
                }
                string[] pageValUrl = { "act", "uid", "qihaos", "ptype", "backurl" };
                pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                if (pageIndex == 0)
                    pageIndex = 1;
                string HP3qi = "";
                // 开始读取列表
                IList<BCW.HP3.Model.HP3BuySY> listHP3Buy = new BCW.HP3.BLL.HP3BuySY().GetHP3ListByPage(pageIndex, pageSize, strWhere, out recordCount);
                if (listHP3Buy.Count > 0)
                {
                    int k = 1;
                    foreach (BCW.HP3.Model.HP3BuySY n in listHP3Buy)
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
                        builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=del&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[删]&gt;</a>");
                        string name = new BCW.BLL.User().GetUsName(n.BuyID);
                        builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".编号[" + n.ID + "]<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.BuyID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + name + "</a>(" + n.BuyID + ")," + NumToType(n.BuyType)); if (n.BuyType == 1)
                        {
                            builder.Append(speChoose(Convert.ToInt32(n.BuyNum)));
                        }
                        else if (n.BuyType == 17)
                        {
                            switch (n.BuyNum.Trim())
                            {
                                case "1":
                                    builder.Append(":买大");
                                    break;
                                case "2":
                                    builder.Append(":买小");
                                    break;
                                case "3":
                                    builder.Append(":买单");
                                    break;
                                case "4":
                                    builder.Append(":买双");
                                    break;

                            }
                        }
                        else
                        {
                            builder.Append(":选号（" + n.BuyNum + ")");
                        }
                        HP3qi = n.BuyDate.ToString();
                        builder.Append(",下注 <b>" + n.BuyMoney + " </b>酷币,下<b>" + n.BuyZhu + "</b>注.");
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

            string strText = "输入用户ID(可为空):/,输入彩票期号(可为空):/,";
            string strName = "uid,qihaos,backurl";
            string strType = "num,num,hidden";
            string strValu = "'" + "'" + Utils.getPage(0) + "";
            string strEmpt = "true,true,false";
            string strIdea = "/";
            string strOthe = "搜一搜,HP3.aspx?act=BuyWin&amp;ptype=" + ptype + ",post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            #endregion
        }

    }
    //手动开奖兑奖
    private void OpenCase()
    {
        if (SWB == 0)
        {
            #region 正式版
            Master.Title = "手动开奖兑奖";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
            builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>&gt;");
            builder.Append("手动开奖兑奖");
            builder.Append(Out.Tab("</div>", "<br />"));
            int state = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
            int ck = int.Parse(Utils.GetRequest("ck", "all", 1, @"^[0-2]", "0"));
            int ac = int.Parse(Utils.GetRequest("ac", "all", 1, @"^[0-1]", "0"));

            if (ck == 0)
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<b>第<b style=\"color:red\">" + state + "</b>期</b>");
                builder.Append(Out.Tab("</div>", "<br />"));

                if (ac == 1)
                {
                    string num1hs = Utils.GetRequest("num1hs", "all", 2, "", "");
                    string num1nu = Utils.GetRequest("num1nu", "all", 2, "", "");
                    string num2hs = Utils.GetRequest("num2hs", "all", 2, "", "");
                    string num2nu = Utils.GetRequest("num2nu", "all", 2, "", "");
                    string num3hs = Utils.GetRequest("num3hs", "all", 2, "", "");
                    string num3nu = Utils.GetRequest("num3nu", "all", 2, "", "");

                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("开奖为：" + num1hs + num1nu + "," + num2hs + num2nu + "," + num3hs + num3nu);
                    builder.Append("<br />是否确定手动开奖？");
                    builder.Append(Out.Tab("</div>", ""));

                    string strText = ",,,,,,,";
                    string strName = "num1hs,num1nu,num2hs,num2nu,num3hs,num3nu,state,backurl";
                    string strType = "hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden";
                    string strValu = "" + num1hs + "'" + num1nu + "'" + num2hs + "'" + num2nu + "'" + num3hs + "'" + num3nu + "'" + state.ToString() + "'" + Utils.getPage(0) + "";
                    string strEmpt = "黑桃|黑桃|红桃|红桃|梅花|梅花|方块|方块,A|A|2|2|3|3|4|4|5|5|6|6|7|7|8|8|9|9|10|10|J|J|Q|Q|K|K,黑桃|黑桃|红桃|红桃|梅花|梅花|方块|方块,A|A|2|2|3|3|4|4|5|5|6|6|7|7|8|8|9|9|10|10|J|J|Q|Q|K|K,黑桃|黑桃|红桃|红桃|梅花|梅花|方块|方块,A|A|2|2|3|3|4|4|5|5|6|6|7|7|8|8|9|9|10|10|J|J|Q|Q|K|K,true,false";
                    string strIdea = "/";
                    string strOthe = "确定开奖,HP3.aspx?act=OpenSave&amp;id=" + state.ToString() + ",post,1,red";
                    builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

                    builder.Append(Out.Tab("<div>", "<br />"));
                    builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=OpenCase&amp;id=" + state + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">返回重选</a>");
                    builder.Append(Out.Tab("</div>", ""));
                }
                else
                {
                    string strText = "第一个开奖号码:/,,第二个开奖号码:/,,第三个开奖号码:/,,期号/,";
                    string strName = "num1hs,num1nu,num2hs,num2nu,num3hs,num3nu,state,backurl";
                    string strType = "select,select,select,select,select,select,hidden,hidden";
                    string strValu = "黑桃'A'红桃'J'梅花'K'" + state.ToString() + "'" + Utils.getPage(0) + "";
                    string strEmpt = "黑桃|黑桃|红桃|红桃|梅花|梅花|方块|方块,A|A|2|2|3|3|4|4|5|5|6|6|7|7|8|8|9|9|10|10|J|J|Q|Q|K|K,黑桃|黑桃|红桃|红桃|梅花|梅花|方块|方块,A|A|2|2|3|3|4|4|5|5|6|6|7|7|8|8|9|9|10|10|J|J|Q|Q|K|K,黑桃|黑桃|红桃|红桃|梅花|梅花|方块|方块,A|A|2|2|3|3|4|4|5|5|6|6|7|7|8|8|9|9|10|10|J|J|Q|Q|K|K,true,false";
                    string strIdea = "/";
                    string strOthe = "确定开奖,HP3.aspx?act=OpenCase&amp;ac=1&amp;id=" + state.ToString() + ",post,1,red";
                    builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                }
            }
            if (ck == 1)
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<b>第<b style=\"color:red\">" + state + "</b>期</b>");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("是否确定重开奖？<br />");
                builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=OpenCase&amp;id=" + state + "&amp;ck=2&amp;backurl=" + Utils.PostPage(1) + "") + "\">确定重开奖</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx") + "\">返回管理</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            else if (ck == 2)
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<b>第<b style=\"color:red\">" + state + "</b>期</b>");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("重开奖：");
                builder.Append(Out.Tab("</div>", "<br />"));


                List<BCW.HP3.Model.HP3Winner> DelList = new BCW.HP3.BLL.HP3Winner().GetModelList("WinDate='" + state + "'");
                foreach (BCW.HP3.Model.HP3Winner n in DelList)
                {
                    if (n.WinBool == 0)
                    {
                        new BCW.BLL.User().UpdateiGold(n.WinUserID, -n.WinMoney, "" + GameName + "第" + n.WinDate + "期兑奖标识" + n.ID + "重开奖扣回");

                        try
                        {
                            if (new BCW.BLL.User().GetIsSpier(n.WinUserID) != 1)
                            {
                                DataSet ds = new BCW.HP3.BLL.HP3Buy().GetList("IsRot", " ID=" + n.ID + " ");
                                if (ds.Tables[0].Rows[0][0].ToString() != "1")
                                    new BCW.BLL.User().UpdateiGold(107, new BCW.BLL.User().GetUsName(107), n.WinMoney, "ID:[url=forumlog.aspx?act=xview&amp;uid=" + n.WinUserID + "]" + new BCW.BLL.User().GetUsName(n.WinUserID) + "[/url]" + GameName + "第[url=./game/HP3.aspx?act=BuyWin&amp;ptype=1&amp;qihaos=" + n.WinDate + "]" + n.WinDate + "[/url]期重开奖扣回" + n.WinMoney + "(标识ID" + n.ID + ")");
                            }
                        }
                        catch { }
                    }

                    string mename = new BCW.BLL.User().GetUsName(n.WinUserID);
                    new BCW.BLL.Guest().Add(1, n.WinUserID, mename, "" + GameName + "第" + n.WinDate + "期兑奖标识" + n.ID + "重开奖了！");
                }

                BCW.Data.SqlHelper.ExecuteSql("update tb_HP3Buy set WillGet=0 Where BuyDate=" + state + "");//原先的willget将全部置零
                new BCW.HP3.BLL.HP3Winner().Delete("WinDate='" + state + "'");


                BCW.HP3.Model.HP3_kjnum mool = new BCW.HP3.BLL.HP3_kjnum().GetDataByState(state.ToString());
                mool.Fnum = "null";
                mool.Snum = "null";
                mool.Tnum = "null";
                mool.Winum = "null";
                new BCW.HP3.BLL.HP3_kjnum().Update(mool);


                if (ac == 1)
                {
                    string num1hs = Utils.GetRequest("num1hs", "all", 2, "", "");
                    string num1nu = Utils.GetRequest("num1nu", "all", 2, "", "");
                    string num2hs = Utils.GetRequest("num2hs", "all", 2, "", "");
                    string num2nu = Utils.GetRequest("num2nu", "all", 2, "", "");
                    string num3hs = Utils.GetRequest("num3hs", "all", 2, "", "");
                    string num3nu = Utils.GetRequest("num3nu", "all", 2, "", "");

                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("开奖为：" + num1hs + num1nu + "," + num2hs + num2nu + "," + num3hs + num3nu);
                    builder.Append("<br />是否确定手动重开奖？");
                    builder.Append(Out.Tab("</div>", ""));

                    string strText = ",,,,,,,";
                    string strName = "num1hs,num1nu,num2hs,num2nu,num3hs,num3nu,state,backurl";
                    string strType = "hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden";
                    string strValu = "" + num1hs + "'" + num1nu + "'" + num2hs + "'" + num2nu + "'" + num3hs + "'" + num3nu + "'" + state.ToString() + "'" + Utils.getPage(0) + "";
                    string strEmpt = "黑桃|黑桃|红桃|红桃|梅花|梅花|方块|方块,A|A|2|2|3|3|4|4|5|5|6|6|7|7|8|8|9|9|10|10|J|J|Q|Q|K|K,黑桃|黑桃|红桃|红桃|梅花|梅花|方块|方块,A|A|2|2|3|3|4|4|5|5|6|6|7|7|8|8|9|9|10|10|J|J|Q|Q|K|K,黑桃|黑桃|红桃|红桃|梅花|梅花|方块|方块,A|A|2|2|3|3|4|4|5|5|6|6|7|7|8|8|9|9|10|10|J|J|Q|Q|K|K,true,false";
                    string strIdea = "/";
                    string strOthe = "确定开奖,HP3.aspx?act=OpenSave&amp;id=" + state.ToString() + ",post,1,red";
                    builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

                    builder.Append(Out.Tab("<div>", "<br />"));
                    builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=OpenCase&amp;id=" + state + "&amp;ac=0&amp;ck=2&amp;backurl=" + Utils.PostPage(1) + "") + "\">返回重选</a>");
                    builder.Append(Out.Tab("</div>", ""));
                }
                else
                {
                    string strText = "第一个开奖号码:/,,第二个开奖号码:/,,第三个开奖号码:/,,期号/,";
                    string strName = "num1hs,num1nu,num2hs,num2nu,num3hs,num3nu,state,backurl";
                    string strType = "select,select,select,select,select,select,hidden,hidden";
                    string strValu = "黑桃'A'红桃'J'梅花'K'" + state.ToString() + "'" + Utils.getPage(0) + "";
                    string strEmpt = "黑桃|黑桃|红桃|红桃|梅花|梅花|方块|方块,A|A|2|2|3|3|4|4|5|5|6|6|7|7|8|8|9|9|10|10|J|J|Q|Q|K|K,黑桃|黑桃|红桃|红桃|梅花|梅花|方块|方块,A|A|2|2|3|3|4|4|5|5|6|6|7|7|8|8|9|9|10|10|J|J|Q|Q|K|K,黑桃|黑桃|红桃|红桃|梅花|梅花|方块|方块,A|A|2|2|3|3|4|4|5|5|6|6|7|7|8|8|9|9|10|10|J|J|Q|Q|K|K,true,false";
                    string strIdea = "/";
                    string strOthe = "确定开奖,HP3.aspx?act=OpenCase&amp;ac=1&amp;ck=2&amp;id=" + state.ToString() + ",post,1,red";
                    builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                }
            }


            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("HP3.aspx") + "\">返回上一级</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
            #endregion
        }
        else
        {
            #region
            Master.Title = "手动开奖兑奖";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
            builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>&gt;");
            builder.Append("手动开奖兑奖");
            builder.Append(Out.Tab("</div>", "<br />"));
            int state = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
            int ck = int.Parse(Utils.GetRequest("ck", "get", 1, @"^[0-1]", "0"));
            builder.Append(Out.Tab("<div>", ""));
            if (ck == 1)
            {
                builder.Append("重开");
                new BCW.HP3.BLL.HP3WinnerSY().Delete("WinDate='" + state + "'");
                BCW.HP3.Model.HP3_kjnum mool = new BCW.HP3.BLL.HP3_kjnum().GetDataByState(state.ToString());
                mool.Fnum = "null";
                mool.Snum = "null";
                mool.Tnum = "null";
                mool.Winum = "null";
                new BCW.HP3.BLL.HP3_kjnum().Update(mool);
            }
            builder.Append("<b>第<b style=\"color:red\">" + state + "</b>期</b>");
            builder.Append(Out.Tab("</div>", "<br />"));
            string strText = "第一个开奖号码:/,,第二个开奖号码:/,,第三个开奖号码:/,,期号/,";
            string strName = "num1hs,num1nu,num2hs,num2nu,num3hs,num3nu,state,backurl";
            string strType = "select,select,select,select,select,select,hidden,hidden";
            string strValu = "黑桃'A'红桃'J'梅花'K'" + state.ToString() + "'" + Utils.getPage(0) + "";
            string strEmpt = "黑桃|黑桃|红桃|红桃|梅花|梅花|方块|方块,A|A|2|2|3|3|4|4|5|5|6|6|7|7|8|8|9|9|10|10|J|J|Q|Q|K|K,黑桃|黑桃|红桃|红桃|梅花|梅花|方块|方块,A|A|2|2|3|3|4|4|5|5|6|6|7|7|8|8|9|9|10|10|J|J|Q|Q|K|K,黑桃|黑桃|红桃|红桃|梅花|梅花|方块|方块,A|A|2|2|3|3|4|4|5|5|6|6|7|7|8|8|9|9|10|10|J|J|Q|Q|K|K,true,false";
            string strIdea = "/";
            string strOthe = "确定开奖,HP3.aspx?act=OpenSave&amp;id=" + state.ToString() + ",post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("HP3.aspx") + "\">返回上一级</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
            #endregion
        }
    }
    //开奖存入
    private void OpenSave()
    {
        if (SWB == 0)
        {
            #region 正式版
            Master.Title = "开奖存入";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
            builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>&gt;");
            builder.Append("开奖存入");
            builder.Append(Out.Tab("</div>", "<br />"));

            string num1hs = Utils.GetRequest("num1hs", "post", 2, "", "");
            string num1nu = Utils.GetRequest("num1nu", "post", 2, "", "");
            string num2hs = Utils.GetRequest("num2hs", "post", 2, "", "");
            string num2nu = Utils.GetRequest("num2nu", "post", 2, "", "");
            string num3hs = Utils.GetRequest("num3hs", "post", 2, "", "");
            string num3nu = Utils.GetRequest("num3nu", "post", 2, "", "");
            string state = Utils.GetRequest("state", "post", 1, "", "");
            string num1 = num1hs + num1nu;
            string num2 = num2hs + num2nu;
            string num3 = num3hs + num3nu;
            if (num1 == num2 || num2 == num3 || num1 == num3)
            {
                Utils.Error("请勿选择相同号码！", "");
            }
            string dm = zjdm(num1, num2, num3);
            BCW.HP3.Model.HP3_kjnum model = new BCW.HP3.Model.HP3_kjnum();
            model.datenum = state;
            model.Fnum = num1;
            model.Snum = num2;
            model.Tnum = num3;
            model.Winum = dm;
            bool isno = new BCW.HP3.BLL.HP3_kjnum().Exists(state);
            if (isno)
            {
                BCW.HP3.Model.HP3_kjnum Bystate = new BCW.HP3.BLL.HP3_kjnum().GetDataByState(state);
                model.datetime = Bystate.datetime.AddSeconds(50);
                new BCW.HP3.BLL.HP3_kjnum().Update(model);

            }
            else
            {

                model.datetime = DateTime.Now.AddSeconds(50);
                bool ss = new BCW.HP3.BLL.HP3_kjnum().Add(model);
                builder.Append(ss);
            }
            builder.Append("第" + state + "期");
            builder.Append("<br />第一个号码：" + num1);
            builder.Append("<br />第二个号码：" + num2);
            builder.Append("<br />第三个号码：" + num3);
            builder.Append("<br />中间代码为：" + dm);
            builder.Append("<br />已成功存入开奖号码！");
            try
            {
                Winner(state);
                builder.Append("<br />已成功获取中奖列表！");
            }
            catch
            {
                builder.Append("手动获奖失败，请人工返奖！！！");
            }
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("HP3.aspx") + "\">返回上一级</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
            #endregion
        }
        else
        {
            #region
            Master.Title = "开奖存入";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
            builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>&gt;");
            builder.Append("开奖存入");
            builder.Append(Out.Tab("</div>", "<br />"));

            string num1hs = Utils.GetRequest("num1hs", "post", 2, "", "");
            string num1nu = Utils.GetRequest("num1nu", "post", 2, "", "");
            string num2hs = Utils.GetRequest("num2hs", "post", 2, "", "");
            string num2nu = Utils.GetRequest("num2nu", "post", 2, "", "");
            string num3hs = Utils.GetRequest("num3hs", "post", 2, "", "");
            string num3nu = Utils.GetRequest("num3nu", "post", 2, "", "");
            string state = Utils.GetRequest("state", "post", 1, "", "");
            string num1 = num1hs + num1nu;
            string num2 = num2hs + num2nu;
            string num3 = num3hs + num3nu;
            if (num1 == num2 || num2 == num3 || num1 == num3)
            {
                Utils.Error("请勿选择相同号码！", "");
            }
            string dm = zjdm(num1, num2, num3);

            BCW.HP3.Model.HP3_kjnum model = new BCW.HP3.Model.HP3_kjnum();
            model.datenum = state;
            model.Fnum = num1;
            model.Snum = num2;
            model.Tnum = num3;

            model.Winum = dm;

            bool isno = new BCW.HP3.BLL.HP3_kjnum().Exists(state);
            if (isno)
            {
                BCW.HP3.Model.HP3_kjnum Bystate = new BCW.HP3.BLL.HP3_kjnum().GetDataByState(state);
                model.datetime = Bystate.datetime.AddSeconds(50);
                new BCW.HP3.BLL.HP3_kjnum().Update(model);

            }
            else
            {
                model.datetime = DateTime.Now.AddSeconds(50);
                bool ss = new BCW.HP3.BLL.HP3_kjnum().Add(model);
                builder.Append(ss);
            }

            builder.Append("第" + state + "期");
            builder.Append("<br />第一个号码：" + num1);
            builder.Append("<br />第二个号码：" + num2);
            builder.Append("<br />第三个号码：" + num3);
            builder.Append("<br />中间代码为：" + dm);
            builder.Append("<br />已成功存入开奖号码！");
            try
            {
                Winner(state);
                builder.Append("<br />已成功获取中奖列表！");
            }
            catch
            {
                builder.Append("手动获奖失败，请人工返奖！！！");
            }
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("HP3.aspx") + "\">返回上一级</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
            #endregion
        }
    }
    //中奖代码
    public string zjdm(string num1, string num2, string num3)
    {
        System.Text.StringBuilder dms = new System.Text.StringBuilder("");
        string dm = "0";
        Match sa1 = Regex.Match(num1, @"[\u4e00-\u9fa5]{2}");
        Match sa2 = Regex.Match(num2, @"[\u4e00-\u9fa5]{2}");
        Match sa3 = Regex.Match(num3, @"[\u4e00-\u9fa5]{2}");
        Match san1 = Regex.Match(num1, @"[A-Z\d]{1,2}");
        Match san2 = Regex.Match(num2, @"[A-Z\d]{1,2}");
        Match san3 = Regex.Match(num3, @"[A-Z\d]{1,2}");
        string s1 = sa1.Value;
        string s2 = sa2.Value;
        string s3 = sa3.Value;
        string sn1 = san1.Value;
        string sn2 = san2.Value;
        string sn3 = san3.Value;

        switch (sn1)
        {
            case "A":
                sn1 = "1";
                break;
            case "J":
                sn1 = "11";
                break;
            case "Q":
                sn1 = "12";
                break;
            case "K":
                sn1 = "13";
                break;
        }
        switch (sn2)
        {
            case "A":
                sn2 = "1";
                break;
            case "J":
                sn2 = "11";
                break;
            case "Q":
                sn2 = "12";
                break;
            case "K":
                sn2 = "13";
                break;
        }
        switch (sn3)
        {
            case "A":
                sn3 = "1";
                break;
            case "J":
                sn3 = "11";
                break;
            case "Q":
                sn3 = "12";
                break;
            case "K":
                sn3 = "13";
                break;
        }
        int a = int.Parse(sn1);
        int b = int.Parse(sn2);
        int c = int.Parse(sn3);
        List<int> list = new List<int>();
        list.Add(a);
        list.Add(b);
        list.Add(c);
        list.Sort();
        int n1 = list[0];
        int n2 = list[1];
        int n3 = list[2];
        #region 特殊代码
        if (s1 == s2 && s2 == s3)
        {
            if (n2 == n1 + 1 && n3 == n2 + 1)
            {
                switch (s1)
                {
                    case "黑桃":
                        dm = "19";
                        break;
                    case "红桃":
                        dm = "20";
                        break;
                    case "梅花":
                        dm = "21";
                        break;
                    case "方块":
                        dm = "22";
                        break;
                }
                switch (n1)
                {
                    case 1:
                        dm += "06";
                        break;
                    case 2:
                        dm += "07";
                        break;
                    case 3:
                        dm += "08";
                        break;
                    case 4:
                        dm += "09";
                        break;
                    case 5:
                        dm += "10";
                        break;
                    case 6:
                        dm += "11";
                        break;
                    case 7:
                        dm += "12";
                        break;
                    case 8:
                        dm += "13";
                        break;
                    case 9:
                        dm += "14";
                        break;
                    case 10:
                        dm += "15";
                        break;
                    case 11:
                        dm += "16";
                        break;
                }
            }
            else if (n2 == 12 && n1 == 1 && n3 == 13)
            {
                if (s1 == s2 && s2 == s3 && s1 == s3)
                {
                    switch (s1)
                    {
                        case "黑桃":
                            dm = "1917";
                            break;
                        case "红桃":
                            dm = "2017";
                            break;
                        case "梅花":
                            dm = "2117";
                            break;
                        case "方块":
                            dm = "2217";
                            break;
                    }
                }
                else
                {
                    dm = "17";
                }
            }
            else
            {

                switch (s1.Trim())
                {
                    case "黑桃":
                        dm = "1";
                        break;
                    case "红桃":
                        dm = "2";
                        break;
                    case "梅花":
                        dm = "3";
                        break;
                    case "方块":
                        dm = "4";
                        break;
                }
            }

        }
        else if (n2 == n1 + 1 && n3 == n2 + 1)
        {
            if (s1 == s2 && s2 == s3 && s1 == s3)
            {
                switch (s1)
                {
                    case "黑桃":
                        dm = "19";
                        break;
                    case "红桃":
                        dm = "20";
                        break;
                    case "梅花":
                        dm = "21";
                        break;
                    case "方块":
                        dm = "22";
                        break;
                }
                switch (n1)
                {
                    case 1:
                        dm += "06";
                        break;
                    case 2:
                        dm += "07";
                        break;
                    case 3:
                        dm += "08";
                        break;
                    case 4:
                        dm += "09";
                        break;
                    case 5:
                        dm += "10";
                        break;
                    case 6:
                        dm += "11";
                        break;
                    case 7:
                        dm += "12";
                        break;
                    case 8:
                        dm += "13";
                        break;
                    case 9:
                        dm += "14";
                        break;
                    case 10:
                        dm += "15";
                        break;
                    case 11:
                        dm += "16";
                        break;
                }
            }
            else
            {
                switch (n1)
                {
                    case 1:
                        dm = "6";
                        break;
                    case 2:
                        dm = "7";
                        break;
                    case 3:
                        dm = "8";
                        break;
                    case 4:
                        dm = "9";
                        break;
                    case 5:
                        dm = "10";
                        break;
                    case 6:
                        dm = "11";
                        break;
                    case 7:
                        dm = "12";
                        break;
                    case 8:
                        dm = "13";
                        break;
                    case 9:
                        dm = "14";
                        break;
                    case 10:
                        dm = "15";
                        break;
                    case 11:
                        dm = "16";
                        break;
                    case 12:
                        dm = "17";
                        break;
                }
            }

        }
        else if (n2 == 12 && n1 == 1 && n3 == 13)
        {
            if (s1 == s2 && s2 == s3 && s1 == s3)
            {
                switch (s1)
                {
                    case "黑桃":
                        dm = "1917";
                        break;
                    case "红桃":
                        dm = "2017";
                        break;
                    case "梅花":
                        dm = "2117";
                        break;
                    case "方块":
                        dm = "2217";
                        break;
                }
            }
            else
            {
                dm = "17";
            }

        }
        else if (n1 == n2 && n2 == n3)
        {
            switch (n1)
            {
                case 1:
                    dm = "24";
                    break;
                case 2:
                    dm = "25";
                    break;
                case 3:
                    dm = "26";
                    break;
                case 4:
                    dm = "27";
                    break;
                case 5:
                    dm = "28";
                    break;
                case 6:
                    dm = "29";
                    break;
                case 7:
                    dm = "30";
                    break;
                case 8:
                    dm = "31";
                    break;
                case 9:
                    dm = "32";
                    break;
                case 10:
                    dm = "33";
                    break;
                case 11:
                    dm = "34";
                    break;
                case 12:
                    dm = "35";
                    break;
                case 13:
                    dm = "36";
                    break;
            }
        }
        else if (n1 == n2 || n2 == n3 || n1 == n3)
        {
            int nnn = 0;
            if (n1 == n2 || n1 == n3)
            {
                nnn = n1;
            }
            else
            {
                nnn = n2;
            }
            switch (nnn)
            {
                case 1:
                    dm = "38";
                    break;
                case 2:
                    dm = "39";
                    break;
                case 3:
                    dm = "40";
                    break;
                case 4:
                    dm = "41";
                    break;
                case 5:
                    dm = "42";
                    break;
                case 6:
                    dm = "43";
                    break;
                case 7:
                    dm = "44";
                    break;
                case 8:
                    dm = "45";
                    break;
                case 9:
                    dm = "46";
                    break;
                case 10:
                    dm = "47";
                    break;
                case 11:
                    dm = "48";
                    break;
                case 12:
                    dm = "49";
                    break;
                case 13:
                    dm = "50";
                    break;
            }

        }
        #endregion
        if (n1 == 10 || n1 == 11 || n1 == 12 || n1 == 13)
        {
            n1 = 0;
        }
        if (n2 == 10 || n2 == 11 || n2 == 12 || n2 == 13)
        {
            n2 = 0;
        }
        if (n3 == 10 || n3 == 11 || n3 == 12 || n3 == 13)
        {
            n3 = 0;
        }
        int sum = Convert.ToInt32(n1) + Convert.ToInt32(n2) + Convert.ToInt32(n3);
        sum = sum % 10;
        dms.Append(dm + "," + san1 + "," + san2 + "," + san3 + "," + sum);
        return dms.ToString();
    }
    //获奖判断
    public void Winner(string statesss)
    {
        if (SWB == 0)
        {
            #region 正式版
            double DwMon = 0;
            int WinZhu = 0;
            //取开奖号码表中数据
            BCW.HP3.Model.HP3_kjnum kj = new BCW.HP3.Model.HP3_kjnum();
            kj = new BCW.HP3.BLL.HP3_kjnum().GetDataByState(statesss.Trim());
            string state = kj.datenum;
            string winum = kj.Winum.Trim();
            string[] wnum = winum.Split(',');
            int EndSum = Convert.ToInt32(wnum[4]);
            //取用户订单数据
            DataSet ds = new BCW.HP3.BLL.HP3Buy().GetList("*", "BuyDate='" + state + "'");
            BCW.HP3.Model.HP3Buy model = new BCW.HP3.Model.HP3Buy();
            int n = ds.Tables[0].Rows.Count - 1;
            builder.Append("<br />本期共" + ds.Tables[0].Rows.Count + "条购彩记录<br />");
            for (; n >= 0; n--)
            {
                builder.Append(n + 1 + ".");
                model.ID = Convert.ToInt32(ds.Tables[0].Rows[n][0]);
                model.BuyID = Convert.ToInt32(ds.Tables[0].Rows[n][1]);
                model.BuyDate = Convert.ToString(ds.Tables[0].Rows[n][2]);
                model.BuyType = Convert.ToInt32(ds.Tables[0].Rows[n][3]);
                model.BuyNum = Convert.ToString(ds.Tables[0].Rows[n][4]);
                model.BuyMoney = Convert.ToInt64(ds.Tables[0].Rows[n][5]);
                model.BuyZhu = Convert.ToInt32(ds.Tables[0].Rows[n][6]);
                model.BuyTime = Convert.ToDateTime(ds.Tables[0].Rows[n][7]);
                model.Odds = Convert.ToDecimal(ds.Tables[0].Rows[n][8]);
                string IsRot = ds.Tables[0].Rows[n][10].ToString();
                //取获奖订单数据
                BCW.HP3.Model.HP3Winner modelWin = new BCW.HP3.Model.HP3Winner();
                modelWin.ID = model.ID;
                modelWin.WinDate = model.BuyDate;
                modelWin.WinUserID = model.BuyID;
                modelWin.WinBool = 1;
                bool isis = new BCW.HP3.BLL.HP3Winner().Exists(modelWin.ID);
                if (isis)
                {
                    builder.Append("获奖数据已存在!!!<br />");
                }
                else
                {
                    builder.Append("一条购奖数据成功判断!!!<br />");
                    string mename = new BCW.BLL.User().GetUsName(model.BuyID);
                    if (model.BuyType == 1)
                    {
                        #region 特殊方法
                        DwMon = Convert.ToDouble(model.Odds);
                        if (model.BuyNum.Trim() == "1" || model.BuyNum.Trim() == "2" || model.BuyNum.Trim() == "3" || model.BuyNum.Trim() == "4")
                        {
                            if (wnum[0].Trim() == model.BuyNum)
                            {
                                WinZhu = 1;
                                modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            }
                            if (wnum[0].Length > 2)
                            {

                                if (wnum[0].Substring(0, 2).Trim() == "19" && model.BuyNum.Trim() == "1")
                                {
                                    WinZhu = 1;
                                    modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                                }
                                if (wnum[0].Substring(0, 2).Trim() == "20" && model.BuyNum.Trim() == "2")
                                {
                                    WinZhu = 1;
                                    modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                                }
                                if (wnum[0].Substring(0, 2).Trim() == "21" && model.BuyNum.Trim() == "3")
                                {
                                    WinZhu = 1;
                                    modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                                }
                                if (wnum[0].Substring(0, 2).Trim() == "22" && model.BuyNum.Trim() == "4")
                                {
                                    WinZhu = 1;
                                    modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                                }
                            }

                        }
                        else if (model.BuyNum.Trim() == "5")
                        {
                            if (wnum[0].Trim() == "1" || wnum[0].Trim() == "2" || wnum[0].Trim() == "3" || wnum[0].Trim() == "4")
                            {
                                WinZhu = 1;
                                modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            }
                            if (wnum[0].Length > 2)
                            {

                                if (wnum[0].Substring(0, 2).Trim() == "19" || wnum[0].Substring(0, 2).Trim() == "20" || wnum[0].Substring(0, 2).Trim() == "21" || wnum[0].Substring(0, 2).Trim() == "22")
                                {
                                    WinZhu = 1;
                                    modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                                }

                            }
                        }
                        else if (model.BuyNum == "6" || model.BuyNum == "7" || model.BuyNum == "8" || model.BuyNum == "9" || model.BuyNum == "10" || model.BuyNum == "11" || model.BuyNum == "12" || model.BuyNum == "13" || model.BuyNum == "14" || model.BuyNum == "15" || model.BuyNum == "16" || model.BuyNum == "17")
                        {
                            if (wnum[0].Trim() == model.BuyNum)
                            {
                                WinZhu = 1;
                                modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            }
                            if (wnum[0].Length > 2)
                            {

                                if (wnum[0].Substring(2, 2) == model.BuyNum)
                                {
                                    WinZhu = 1;
                                    modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                                }
                                if (wnum[0].Substring(2, 2) == "06" && model.BuyNum == "6")
                                {
                                    WinZhu = 1;
                                    modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                                }
                                if (wnum[0].Substring(2, 2) == "07" && model.BuyNum == "7")
                                {
                                    WinZhu = 1;
                                    modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                                }
                                if (wnum[0].Substring(2, 2) == "08" && model.BuyNum == "8")
                                {
                                    WinZhu = 1;
                                    modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                                }
                                if (wnum[0].Substring(2, 2) == "09" && model.BuyNum == "9")
                                {
                                    WinZhu = 1;
                                    modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                                }
                            }
                        }
                        else if (model.BuyNum == "18")
                        {
                            if (wnum[0].Trim() == "6" || wnum[0].Trim() == "7" || wnum[0].Trim() == "8" || wnum[0].Trim() == "9" || wnum[0].Trim() == "10" || wnum[0].Trim() == "11" || wnum[0].Trim() == "12" || wnum[0].Trim() == "13" || wnum[0].Trim() == "14" || wnum[0].Trim() == "15" || wnum[0].Trim() == "16" || wnum[0].Trim() == "17")
                            {
                                WinZhu = 1;
                                modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            }
                            if (wnum[0].Length > 2)
                            {
                                if (wnum[0].Substring(2, 2).Trim() == "06" || wnum[0].Substring(2, 2).Trim() == "07" || wnum[0].Substring(2, 2).Trim() == "08" || wnum[0].Substring(2, 2).Trim() == "09" || wnum[0].Substring(2, 2).Trim() == "10" || wnum[0].Substring(2, 2).Trim() == "11" || wnum[0].Substring(2, 2).Trim() == "12" || wnum[0].Substring(2, 2).Trim() == "13" || wnum[0].Substring(2, 2).Trim() == "14" || wnum[0].Substring(2, 2).Trim() == "15" || wnum[0].Substring(2, 2).Trim() == "16" || wnum[0].Substring(2, 2).Trim() == "17")
                                {
                                    WinZhu = 1;
                                    modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                                }
                            }
                        }
                        else if (model.BuyNum == "19" || model.BuyNum == "20" || model.BuyNum == "21" || model.BuyNum == "22")
                        {
                            if (wnum[0].Length > 2)
                            {
                                if (wnum[0].Substring(0, 2).Trim() == model.BuyNum)
                                {
                                    WinZhu = 1;
                                    modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                                }
                            }
                        }
                        else if (model.BuyNum == "23")
                        {
                            if (wnum[0].Length > 2)
                            {
                                if (wnum[0].Substring(0, 2).Trim() == "19" || wnum[0].Substring(0, 2).Trim() == "20" || wnum[0].Substring(0, 2).Trim() == "21" || wnum[0].Substring(0, 2).Trim() == "22")
                                {
                                    WinZhu = 1;
                                    modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                                }
                            }
                        }
                        else if (model.BuyNum == "24" || model.BuyNum == "25" || model.BuyNum == "26" || model.BuyNum == "27" || model.BuyNum == "28" || model.BuyNum == "29" || model.BuyNum == "30" || model.BuyNum == "31" || model.BuyNum == "32" || model.BuyNum == "33" || model.BuyNum == "34" || model.BuyNum == "35" || model.BuyNum == "36")
                        {
                            if (wnum[0].Trim() == model.BuyNum)
                            {
                                WinZhu = 1;
                                modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            }
                        }
                        else if (model.BuyNum == "37")
                        {
                            if (wnum[0].Trim() == "24" || wnum[0].Trim() == "25" || wnum[0].Trim() == "26" || wnum[0].Trim() == "27" || wnum[0].Trim() == "28" || wnum[0].Trim() == "29" || wnum[0].Trim() == "30" || wnum[0].Trim() == "31" || wnum[0].Trim() == "32" || wnum[0].Trim() == "33" || wnum[0].Trim() == "34" || wnum[0].Trim() == "35" || wnum[0].Trim() == "36")
                            {
                                WinZhu = 1;
                                modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            }
                        }
                        else if (model.BuyNum == "38" || model.BuyNum == "39" || model.BuyNum == "40" || model.BuyNum == "41" || model.BuyNum == "42" || model.BuyNum == "43" || model.BuyNum == "44" || model.BuyNum == "45" || model.BuyNum == "46" || model.BuyNum == "47" || model.BuyNum == "48" || model.BuyNum == "49" || model.BuyNum == "50")
                        {
                            if (wnum[0].Trim() == model.BuyNum)
                            {
                                WinZhu = 1;
                                modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            }
                        }
                        else if (model.BuyNum == "51")
                        {
                            if (wnum[0].Trim() == "38" || wnum[0].Trim() == "39" || wnum[0].Trim() == "40" || wnum[0].Trim() == "41" || wnum[0].Trim() == "42" || wnum[0].Trim() == "43" || wnum[0].Trim() == "44" || wnum[0].Trim() == "45" || wnum[0].Trim() == "46" || wnum[0].Trim() == "47" || wnum[0].Trim() == "48" || wnum[0].Trim() == "49" || wnum[0].Trim() == "50")
                            {
                                WinZhu = 1;
                                modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            }
                        }
                        #endregion
                        string gameplay = "";
                        switch (model.BuyType)
                        {
                            case 1:
                                gameplay = "花色连号同号投注";
                                break;
                            case 17:
                                gameplay = "大小单双投注";
                                break;
                            default:
                                gameplay = "任选投注";
                                break;
                        }
                        if (WinZhu != 0)
                        {
                            int id = new BCW.HP3.BLL.HP3Winner().Add(modelWin);
                            if (IsRot.Trim() != "1")
                                new BCW.BLL.Guest().Add(1, modelWin.WinUserID, mename, "您的" + GameName + ":" + modelWin.WinDate + "期已经开奖，获得了" + modelWin.WinMoney + "酷币。[url=/bbs/game/HP3.aspx?act=case]马上兑奖[/url]");

                            //动态
                            string wText = "在[url=/bbs/game/HP3.aspx]" + GameName + "[/url]《" + gameplay + "》狂赚" + modelWin.WinMoney + "" + ub.Get("SiteBz") + "";
                            new BCW.BLL.Action().Add(25, id, modelWin.WinUserID, mename, wText);
                            //将该订单ID插入HP3Winner
                            new BCW.HP3.BLL.HP3Winner().UpdateWinZhu(model.ID, WinZhu);
                        }
                        WinZhu = 0;

                    }
                    else if (model.BuyType == 17)//大小单双
                    {
                        #region 大小单双
                        if (wnum[1] == wnum[2] && wnum[1] == wnum[3])
                        {
                        }
                        else
                        {
                            string big_small = "";
                            string single_double = "";
                            if (EndSum >= 5)
                            {
                                big_small = "1";
                            }
                            else
                            {
                                big_small = "2";
                            }
                            if (EndSum % 2 != 0)
                            {
                                single_double = "3";
                            }
                            else
                            {
                                single_double = "4";
                            }
                            if (model.BuyNum == big_small || model.BuyNum == single_double)
                            {
                                int xmlnum = 20 + Convert.ToInt32(model.BuyNum);
                                DwMon = Convert.ToDouble(model.Odds);
                                WinZhu = 1;
                                modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                                int id = new BCW.HP3.BLL.HP3Winner().Add(modelWin);
                                string gameplay = "";
                                switch (model.BuyType)
                                {
                                    case 1:
                                        gameplay = "花色连号同号投注";
                                        break;
                                    case 17:
                                        gameplay = "大小单双投注";
                                        break;
                                    default:
                                        gameplay = "任选投注";
                                        break;
                                }
                                if (WinZhu != 0)
                                {
                                    if (IsRot.Trim() != "1")
                                        new BCW.BLL.Guest().Add(1, modelWin.WinUserID, mename, "您的" + GameName + ":" + modelWin.WinDate + "期已经开奖，获得了" + modelWin.WinMoney + "酷币。[url=/bbs/game/HP3.aspx?act=case]马上兑奖[/url]");

                                    //动态
                                    string wText = "在[url=/bbs/game/HP3.aspx]" + GameName + "[/url]《" + gameplay + "》狂赚" + modelWin.WinMoney + "" + ub.Get("SiteBz") + "";
                                    new BCW.BLL.Action().Add(25, id, modelWin.WinUserID, mename, wText);

                                    //将该订单ID插入HP3Winner
                                    new BCW.HP3.BLL.HP3Winner().UpdateWinZhu(model.ID, WinZhu);
                                }
                                WinZhu = 0;
                            }
                        }
                        #endregion
                    }
                    else if (model.BuyType == 6)//直一
                    {

                        #region 直一
                        if (model.BuyNum.Contains(wnum[1]) || model.BuyNum.Contains(wnum[2]) || model.BuyNum.Contains(wnum[3]))
                        {
                            #region MyRegion
                            WinZhu = 0;
                            DwMon = Convert.ToDouble(model.Odds);
                            if (wnum[0] == "24" || wnum[0] == "25" || wnum[0] == "26" || wnum[0] == "27" || wnum[0] == "28" || wnum[0] == "29" || wnum[0] == "30" || wnum[0] == "31" || wnum[0] == "32" || wnum[0] == "33" || wnum[0] == "34" || wnum[0] == "35" || wnum[0] == "36")
                            {
                                if (model.BuyNum.Contains(wnum[1]))
                                {
                                    WinZhu = 1;
                                }
                            }
                            else if (wnum[0] == "38" || wnum[0] == "39" || wnum[0] == "40" || wnum[0] == "41" || wnum[0] == "42" || wnum[0] == "43" || wnum[0] == "44" || wnum[0] == "45" || wnum[0] == "46" || wnum[0] == "47" || wnum[0] == "48" || wnum[0] == "49" || wnum[0] == "50")
                            {
                                if (wnum[1] == wnum[2])
                                {
                                    if (model.BuyNum.Contains(wnum[1]))
                                    {
                                        WinZhu = WinZhu + 1;
                                    }
                                    if (model.BuyNum.Contains(wnum[3]))
                                    {
                                        WinZhu = WinZhu + 1;
                                    }
                                }
                                if (wnum[1] == wnum[3])
                                {
                                    if (model.BuyNum.Contains(wnum[1]))
                                    {
                                        WinZhu = WinZhu + 1;
                                    }
                                    if (model.BuyNum.Contains(wnum[2]))
                                    {
                                        WinZhu = WinZhu + 1;
                                    }
                                }
                                if (wnum[2] == wnum[3])
                                {
                                    if (model.BuyNum.Contains(wnum[2]))
                                    {
                                        WinZhu = WinZhu + 1;
                                    }
                                    if (model.BuyNum.Contains(wnum[1]))
                                    {
                                        WinZhu = WinZhu + 1;
                                    }
                                }
                            }
                            else
                            {
                                if (model.BuyNum.Contains(wnum[1]))
                                {
                                    WinZhu = WinZhu + 1;
                                }
                                if (model.BuyNum.Contains(wnum[2]))
                                {
                                    WinZhu = WinZhu + 1;
                                }
                                if (model.BuyNum.Contains(wnum[3]))
                                {
                                    WinZhu = WinZhu + 1;
                                }
                            }
                            #endregion
                        }
                        #endregion
                        if (WinZhu != 0)
                        {
                            modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            int id = new BCW.HP3.BLL.HP3Winner().Add(modelWin);
                            if (IsRot.Trim() != "1")
                                new BCW.BLL.Guest().Add(1, modelWin.WinUserID, mename, "您的" + GameName + ":" + modelWin.WinDate + "期已经开奖，获得了" + modelWin.WinMoney + "酷币。[url=/bbs/game/HP3.aspx?act=case]马上兑奖[/url]");
                            string gameplay = "";
                            switch (model.BuyType)
                            {
                                case 1:
                                    gameplay = "花色连号同号投注";
                                    break;
                                case 17:
                                    gameplay = "大小单双投注";
                                    break;
                                default:
                                    gameplay = "任选投注";
                                    break;
                            }
                            //动态
                            string wText = "在[url=/bbs/game/HP3.aspx]" + GameName + "[/url]《" + gameplay + "》狂赚" + modelWin.WinMoney + "" + ub.Get("SiteBz") + "";
                            new BCW.BLL.Action().Add(25, id, modelWin.WinUserID, mename, wText);
                            //将该订单ID插入HP3Winner
                            new BCW.HP3.BLL.HP3Winner().UpdateWinZhu(model.ID, WinZhu);
                            WinZhu = 0;
                        }

                    }
                    else if (model.BuyType == 7)//直二
                    {
                        #region 直二
                        WinZhu = 0;
                        DwMon = Convert.ToDouble(model.Odds);
                        if (wnum[0] == "24" || wnum[0] == "25" || wnum[0] == "26" || wnum[0] == "27" || wnum[0] == "28" || wnum[0] == "29" || wnum[0] == "30" || wnum[0] == "31" || wnum[0] == "32" || wnum[0] == "33" || wnum[0] == "34" || wnum[0] == "35" || wnum[0] == "36")
                        {
                            WinZhu = 0;
                        }
                        else if (wnum[0] == "38" || wnum[0] == "39" || wnum[0] == "40" || wnum[0] == "41" || wnum[0] == "42" || wnum[0] == "43" || wnum[0] == "44" || wnum[0] == "45" || wnum[0] == "46" || wnum[0] == "47" || wnum[0] == "48" || wnum[0] == "49" || wnum[0] == "50")
                        {
                            if (model.BuyNum.Contains(wnum[1]) && model.BuyNum.Contains(wnum[2]) && model.BuyNum.Contains(wnum[3]))
                            {
                                WinZhu = 1;
                            }
                        }
                        else
                        {
                            if (model.BuyNum.Contains(wnum[1]) && model.BuyNum.Contains(wnum[2]))
                            {
                                WinZhu = WinZhu + 1;
                            }
                            if (model.BuyNum.Contains(wnum[2]) && model.BuyNum.Contains(wnum[3]))
                            {
                                WinZhu = WinZhu + 1;
                            }
                            if (model.BuyNum.Contains(wnum[1]) && model.BuyNum.Contains(wnum[3]))
                            {
                                WinZhu = WinZhu + 1;
                            }
                        }
                        #endregion
                        if (WinZhu != 0)
                        {
                            //将该订单ID插入HP3Winner
                            modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            int id = new BCW.HP3.BLL.HP3Winner().Add(modelWin);
                            if (IsRot.Trim() != "1")
                                new BCW.BLL.Guest().Add(1, modelWin.WinUserID, mename, "您的" + GameName + ":" + modelWin.WinDate + "期已经开奖，获得了" + modelWin.WinMoney + "酷币。[url=/bbs/game/HP3.aspx?act=case]马上兑奖[/url]");
                            string gameplay = "";
                            switch (model.BuyType)
                            {
                                case 1:
                                    gameplay = "花色连号同号投注";
                                    break;
                                case 17:
                                    gameplay = "大小单双投注";
                                    break;
                                default:
                                    gameplay = "任选投注";
                                    break;
                            }
                            //动态
                            string wText = "在[url=/bbs/game/HP3.aspx]" + GameName + "[/url]《" + gameplay + "》狂赚" + modelWin.WinMoney + "" + ub.Get("SiteBz") + "";
                            new BCW.BLL.Action().Add(25, id, modelWin.WinUserID, mename, wText);
                            //将该订单ID插入HP3Winner
                            new BCW.HP3.BLL.HP3Winner().UpdateWinZhu(model.ID, WinZhu);
                            WinZhu = 0;
                        }

                    }

                    else if (model.BuyType == 9 || model.BuyType == 11 || model.BuyType == 13 || model.BuyType == 15)//直三、四、五、六
                    {
                        #region 直三、四、五、六
                        WinZhu = 0;
                        DwMon = Convert.ToDouble(model.Odds);

                        string[] buynum = model.BuyNum.Split(',');
                        int zj_zs = 0; //统计中奖注数

                        string wnumk = wnum[1] + "," + wnum[2] + "," + wnum[3];
                        string[] winumk = wnumk.Split(',');
                        if (winumk[0] != winumk[1] && winumk[1] != winumk[2] && winumk[0] != winumk[2])
                        {
                            for (int fs = 0; fs < buynum.Length; fs++)
                                for (int p = 0; p < winumk.Length; p++)
                                    if (string.Compare(buynum[fs], winumk[p]) == 0)
                                        zj_zs += 1;
                        }

                        if (model.BuyType == 9)
                        {
                            if (zj_zs == 3)
                            {
                                WinZhu = 1;
                            }
                            else WinZhu = 0;
                        }
                        else if (model.BuyType == 11)
                        {
                            if (zj_zs == 3)
                            {
                                WinZhu = C((buynum.Length - 3), 1);
                            }
                            else WinZhu = 0;
                        }
                        else if (model.BuyType == 13)
                        {
                            if (zj_zs == 3)
                            {
                                WinZhu = C((buynum.Length - 3), 2);
                            }
                            else WinZhu = 0;
                        }
                        else if (model.BuyType == 15)
                        {
                            if (zj_zs == 3)
                            {
                                WinZhu = C((buynum.Length - 3), 3);
                            }
                            else WinZhu = 0;
                        }
                        #endregion
                        if (WinZhu != 0)
                        {
                            modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            int id = new BCW.HP3.BLL.HP3Winner().Add(modelWin);         //将该订单ID插入HP3Winner
                            new BCW.BLL.Guest().Add(1, modelWin.WinUserID, mename, "您的" + GameName + ":" + modelWin.WinDate + "期已经开奖，获得了" + modelWin.WinMoney + "酷币。[url=/bbs/game/HP3.aspx?act=case]马上兑奖[/url]");
                            string gameplay = "";
                            switch (model.BuyType)
                            {
                                case 1:
                                    gameplay = "花色连号同号投注";
                                    break;
                                case 17:
                                    gameplay = "大小单双投注";
                                    break;
                                default:
                                    gameplay = "任选投注";
                                    break;
                            }
                            //动态
                            string wText = "在[url=/bbs/game/HP3.aspx]" + GameName + "[/url]《" + gameplay + "》狂赚" + modelWin.WinMoney + "" + ub.Get("SiteBz") + "";
                            if (IsRot.Trim() != "1")
                                new BCW.BLL.Guest().Add(1, modelWin.WinUserID, mename, "您的" + GameName + ":" + modelWin.WinDate + "期已经开奖，获得了" + modelWin.WinMoney + "酷币。[url=/bbs/game/HP3.aspx?act=case]马上兑奖[/url]");
                            //将该订单ID插入HP3Winner
                            new BCW.HP3.BLL.HP3Winner().UpdateWinZhu(model.ID, WinZhu);
                            WinZhu = 0;
                        }
                    }
                    else if (model.BuyType == 8)//胆二
                    {
                        #region 胆二
                        WinZhu = 0;
                        DwMon = Convert.ToDouble(model.Odds);
                        string[] buynum = model.BuyNum.Split('#');
                        if (buynum[0].Contains(wnum[1]) || buynum[0].Contains(wnum[2]) || buynum[0].Contains(wnum[3]))
                        {
                            if (wnum[0] == "24" || wnum[0] == "25" || wnum[0] == "26" || wnum[0] == "27" || wnum[0] == "28" || wnum[0] == "29" || wnum[0] == "30" || wnum[0] == "31" || wnum[0] == "32" || wnum[0] == "33" || wnum[0] == "34" || wnum[0] == "35" || wnum[0] == "36")
                            {
                                WinZhu = 0;
                            }
                            else if (wnum[0] == "38" || wnum[0] == "39" || wnum[0] == "40" || wnum[0] == "41" || wnum[0] == "42" || wnum[0] == "43" || wnum[0] == "44" || wnum[0] == "45" || wnum[0] == "46" || wnum[0] == "47" || wnum[0] == "48" || wnum[0] == "49" || wnum[0] == "50")
                            {
                                if (buynum[1].Contains(wnum[1]) || buynum[1].Contains(wnum[2]) || buynum[1].Contains(wnum[3]))
                                {
                                    WinZhu = 1;
                                }
                            }
                            else
                            {
                                if (buynum[1].Contains(wnum[1]))
                                {
                                    WinZhu = WinZhu + 1;
                                }
                                if (buynum[1].Contains(wnum[2]))
                                {
                                    WinZhu = WinZhu + 1;
                                }
                                if (buynum[1].Contains(wnum[3]))
                                {
                                    WinZhu = WinZhu + 1;
                                }
                            }
                        }
                        #endregion
                        if (WinZhu != 0)
                        {
                            modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            int id = new BCW.HP3.BLL.HP3Winner().Add(modelWin);
                            if (IsRot.Trim() != "1")
                                new BCW.BLL.Guest().Add(1, modelWin.WinUserID, mename, "您的" + GameName + ":" + modelWin.WinDate + "期已经开奖，获得了" + modelWin.WinMoney + "酷币。[url=/bbs/game/HP3.aspx?act=case]马上兑奖[/url]");
                            string gameplay = "";
                            switch (model.BuyType)
                            {
                                case 1:
                                    gameplay = "花色连号同号投注";
                                    break;
                                case 17:
                                    gameplay = "大小单双投注";
                                    break;
                                default:
                                    gameplay = "任选投注";
                                    break;
                            }
                            //动态
                            string wText = "在[url=/bbs/game/HP3.aspx]" + GameName + "[/url]《" + gameplay + "》狂赚" + modelWin.WinMoney + "" + ub.Get("SiteBz") + "";
                            new BCW.BLL.Action().Add(25, id, modelWin.WinUserID, mename, wText);
                            //将该订单ID插入HP3Winner
                            new BCW.HP3.BLL.HP3Winner().UpdateWinZhu(model.ID, WinZhu);
                            WinZhu = 0;
                        }
                    }
                    else if (model.BuyType == 10)//胆三
                    {
                        #region 胆三
                        WinZhu = 0;
                        DwMon = Convert.ToDouble(model.Odds);
                        string[] buynum = model.BuyNum.Split('#');
                        MatchCollection dmnum = Regex.Matches(buynum[0], ",");
                        MatchCollection leyr = Regex.Matches(buynum[1], ",");
                        int dmn = dmnum.Count + 1;
                        int ele = leyr.Count + 1;
                        if (wnum[0] == "24" || wnum[0] == "25" || wnum[0] == "26" || wnum[0] == "27" || wnum[0] == "28" || wnum[0] == "29" || wnum[0] == "30" || wnum[0] == "31" || wnum[0] == "32" || wnum[0] == "33" || wnum[0] == "34" || wnum[0] == "35" || wnum[0] == "36")
                        {
                            WinZhu = 0;
                        }
                        else if (wnum[0] == "38" || wnum[0] == "39" || wnum[0] == "40" || wnum[0] == "41" || wnum[0] == "42" || wnum[0] == "43" || wnum[0] == "44" || wnum[0] == "45" || wnum[0] == "46" || wnum[0] == "47" || wnum[0] == "48" || wnum[0] == "49" || wnum[0] == "50")
                        {
                            WinZhu = 0;
                        }
                        else
                        {
                            if (model.BuyNum.Contains(wnum[1]) && model.BuyNum.Contains(wnum[2]) && model.BuyNum.Contains(wnum[3]))
                            {
                                if (dmn == 1)
                                {
                                    if (buynum[0].Contains(wnum[1]) || buynum[0].Contains(wnum[2]) || buynum[0].Contains(wnum[3]))
                                    {
                                        WinZhu = 1;
                                    }
                                }
                                else if (dmn == 2)
                                {
                                    if (buynum[0].Contains(wnum[1]) && buynum[0].Contains(wnum[2]))
                                    {
                                        WinZhu = 1;
                                    }
                                    if (buynum[0].Contains(wnum[2]) && buynum[0].Contains(wnum[3]))
                                    {
                                        WinZhu = 1;
                                    }
                                    if (buynum[0].Contains(wnum[1]) && buynum[0].Contains(wnum[3]))
                                    {
                                        WinZhu = 1;
                                    }
                                }
                            }
                        }
                        #endregion
                        if (WinZhu != 0)
                        {
                            modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            int id = new BCW.HP3.BLL.HP3Winner().Add(modelWin);
                            if (IsRot.Trim() != "1")
                                new BCW.BLL.Guest().Add(1, modelWin.WinUserID, mename, "您的" + GameName + ":" + modelWin.WinDate + "期已经开奖，获得了" + modelWin.WinMoney + "酷币。[url=/bbs/game/HP3.aspx?act=case]马上兑奖[/url]");
                            string gameplay = "";
                            switch (model.BuyType)
                            {
                                case 1:
                                    gameplay = "花色连号同号投注";
                                    break;
                                case 17:
                                    gameplay = "大小单双投注";
                                    break;
                                default:
                                    gameplay = "任选投注";
                                    break;
                            }
                            //动态
                            string wText = "在[url=/bbs/game/HP3.aspx]" + GameName + "[/url]《" + gameplay + "》狂赚" + modelWin.WinMoney + "" + ub.Get("SiteBz") + "";
                            new BCW.BLL.Action().Add(25, id, modelWin.WinUserID, mename, wText);
                            //将该订单ID插入HP3Winner
                            new BCW.HP3.BLL.HP3Winner().UpdateWinZhu(model.ID, WinZhu);
                        }
                        WinZhu = 0;
                    }
                    else if (model.BuyType == 12 || model.BuyType == 14 || model.BuyType == 16)//胆四、五、六
                    {
                        #region 胆四、五、六
                        WinZhu = 0;
                        if (wnum[0] == "24" || wnum[0] == "25" || wnum[0] == "26" || wnum[0] == "27" || wnum[0] == "28" || wnum[0] == "29" || wnum[0] == "30" || wnum[0] == "31" || wnum[0] == "32" || wnum[0] == "33" || wnum[0] == "34" || wnum[0] == "35" || wnum[0] == "36")
                        {
                            WinZhu = 0;
                        }
                        else if (wnum[0] == "38" || wnum[0] == "39" || wnum[0] == "40" || wnum[0] == "41" || wnum[0] == "42" || wnum[0] == "43" || wnum[0] == "44" || wnum[0] == "45" || wnum[0] == "46" || wnum[0] == "47" || wnum[0] == "48" || wnum[0] == "49" || wnum[0] == "50")
                        {
                            WinZhu = 0;
                        }
                        else
                        {
                            DwMon = Convert.ToDouble(model.Odds);
                            string[] buynum = model.BuyNum.Split('#');
                            MatchCollection dmnum = Regex.Matches(buynum[0], ",");
                            MatchCollection leyr = Regex.Matches(buynum[1], ",");
                            int dmn = dmnum.Count + 1;
                            int ele = leyr.Count + 1;
                            if (model.BuyNum.Contains(wnum[1]) && model.BuyNum.Contains(wnum[2]) && model.BuyNum.Contains(wnum[3]))
                            {
                                if (model.BuyType == 12)
                                {
                                    #region 胆四
                                    if (buynum[0].Contains(wnum[1]) && buynum[0].Contains(wnum[2]) && buynum[0].Contains(wnum[3]))
                                    {
                                        WinZhu = ele;
                                    }
                                    else if (buynum[0].Contains(wnum[1]) && buynum[0].Contains(wnum[2]))
                                    {
                                        if (dmn == 3)
                                        {
                                            WinZhu = 1;
                                        }
                                        else if (dmn == 2)
                                        {
                                            WinZhu = ele - 1;
                                        }

                                    }
                                    else if (buynum[0].Contains(wnum[2]) && buynum[0].Contains(wnum[3]))
                                    {
                                        if (dmn == 3)
                                        {
                                            WinZhu = 1;
                                        }
                                        else if (dmn == 2)
                                        {
                                            WinZhu = ele - 1;
                                        }
                                    }
                                    else if (buynum[0].Contains(wnum[1]) && buynum[0].Contains(wnum[3]))
                                    {
                                        if (dmn == 3)
                                        {
                                            WinZhu = 1;
                                        }
                                        else if (dmn == 2)
                                        {
                                            WinZhu = ele - 1;
                                        }
                                    }
                                    else if (buynum[0].Contains(wnum[1]) || buynum[0].Contains(wnum[2]) || buynum[0].Contains(wnum[3]))
                                    {
                                        if (dmn == 2)
                                        {
                                            WinZhu = 1;
                                        }
                                        else if (dmn == 1)
                                        {
                                            WinZhu = ele - 2;
                                        }
                                    }

                                    #endregion
                                }
                                else if (model.BuyType == 14)
                                {
                                    #region 胆五
                                    if (buynum[0].Contains(wnum[1]) && buynum[0].Contains(wnum[2]) && buynum[0].Contains(wnum[3]))
                                    {
                                        if (dmn == 4)
                                        {
                                            WinZhu = ele;
                                        }
                                        else if (dmn == 3)
                                        {
                                            WinZhu = ele * (ele - 1) / 2;
                                        }

                                    }
                                    else if (buynum[0].Contains(wnum[1]) && buynum[0].Contains(wnum[2]))
                                    {
                                        if (dmn == 4)
                                        {
                                            WinZhu = 1;
                                        }
                                        else if (dmn == 3)
                                        {
                                            WinZhu = ele - 1;
                                        }
                                        else if (dmn == 2)
                                        {
                                            WinZhu = (ele - 1) * (ele - 2) / 2;
                                        }
                                    }
                                    else if (buynum[0].Contains(wnum[2]) && buynum[0].Contains(wnum[3]))
                                    {
                                        if (dmn == 4)
                                        {
                                            WinZhu = 1;
                                        }
                                        else if (dmn == 3)
                                        {
                                            WinZhu = ele - 1;
                                        }
                                        else if (dmn == 2)
                                        {
                                            WinZhu = (ele - 1) * (ele - 2) / 2;
                                        }
                                    }
                                    else if (buynum[0].Contains(wnum[1]) && buynum[0].Contains(wnum[3]))
                                    {
                                        if (dmn == 4)
                                        {
                                            WinZhu = 1;
                                        }
                                        else if (dmn == 3)
                                        {
                                            WinZhu = ele - 1;
                                        }
                                        else if (dmn == 2)
                                        {
                                            WinZhu = (ele - 1) * (ele - 2) / 2;
                                        }
                                    }
                                    else if (buynum[0].Contains(wnum[1]) || buynum[0].Contains(wnum[2]) || buynum[0].Contains(wnum[3]))
                                    {
                                        if (dmn == 4)
                                        {
                                            WinZhu = 0;
                                        }
                                        else if (dmn == 3)
                                        {
                                            WinZhu = 1;
                                        }
                                        else if (dmn == 2)
                                        {
                                            WinZhu = ele - 2;
                                        }
                                        else if (dmn == 1)
                                        {
                                            WinZhu = (ele - 2) * (ele - 3) / 2;
                                        }
                                    }
                                    #endregion

                                }
                                else if (model.BuyType == 16)
                                {
                                    #region 胆六
                                    int s = 0;
                                    if (buynum[0].Contains(wnum[1]) && buynum[0].Contains(wnum[2]) && buynum[0].Contains(wnum[3]))
                                    {
                                        if (dmn == 5)
                                        {
                                            WinZhu = ele;
                                        }
                                        else if (dmn == 4)
                                        {
                                            WinZhu = ele * (ele - 1) / 2;
                                        }
                                        else if (dmn == 3)
                                        {
                                            WinZhu = ele * (ele - 1) * (ele - 2) / 6;
                                        }
                                    }
                                    else if (buynum[0].Contains(wnum[1]) && buynum[0].Contains(wnum[2]))
                                    {
                                        if (dmn == 5)
                                        {
                                            WinZhu = 1;
                                        }
                                        else if (dmn == 4)
                                        {
                                            WinZhu = ele - 1;
                                        }
                                        else if (dmn == 3)
                                        {
                                            WinZhu = (ele - 1) * (ele - 2) / 2;
                                        }
                                        else if (dmn == 2)
                                        {
                                            WinZhu = (ele - 1) * (ele - 2) * (ele - 3) / 6;
                                        }

                                    }
                                    else if (buynum[0].Contains(wnum[2]) && buynum[0].Contains(wnum[3]))
                                    {
                                        if (dmn == 5)
                                        {
                                            WinZhu = 1;
                                        }
                                        else if (dmn == 4)
                                        {
                                            WinZhu = ele - 1;
                                        }
                                        else if (dmn == 3)
                                        {
                                            WinZhu = (ele - 1) * (ele - 2) / 2;
                                        }
                                        else if (dmn == 2)
                                        {
                                            WinZhu = (ele - 1) * (ele - 2) * (ele - 3) / 6;
                                        }
                                    }
                                    else if (buynum[0].Contains(wnum[1]) && buynum[0].Contains(wnum[3]))
                                    {
                                        if (dmn == 5)
                                        {
                                            WinZhu = 1;
                                        }
                                        else if (dmn == 4)
                                        {
                                            WinZhu = ele - 1;
                                        }
                                        else if (dmn == 3)
                                        {
                                            WinZhu = (ele - 1) * (ele - 2) / 2;
                                        }
                                        else if (dmn == 2)
                                        {
                                            WinZhu = (ele - 1) * (ele - 2) * (ele - 3) / 6;
                                        }
                                    }
                                    else if (buynum[0].Contains(wnum[1]) || buynum[0].Contains(wnum[2]) || buynum[0].Contains(wnum[3]))
                                    {
                                        if (dmn == 4)
                                        {
                                            WinZhu = 1;
                                        }
                                        else if (dmn == 3)
                                        {
                                            WinZhu = ele - 2;
                                        }
                                        else if (dmn == 2)
                                        {
                                            WinZhu = (ele - 2) * (ele - 3) / 2;
                                        }
                                        else if (dmn == 1)
                                        {
                                            WinZhu = (ele - 2) * (ele - 3) * (ele - 4) / 6;
                                        }

                                    }
                                    #endregion
                                }
                            }
                        }
                        #endregion
                        if (WinZhu != 0)
                        {
                            modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            int id = new BCW.HP3.BLL.HP3Winner().Add(modelWin);
                            if (IsRot.Trim() != "1")
                                new BCW.BLL.Guest().Add(1, modelWin.WinUserID, mename, "您的" + GameName + ":" + modelWin.WinDate + "期已经开奖，获得了" + modelWin.WinMoney + "酷币。[url=/bbs/game/HP3.aspx?act=case]马上兑奖[/url]");
                            string gameplay = "";
                            switch (model.BuyType)
                            {
                                case 1:
                                    gameplay = "花色连号同号投注";
                                    break;
                                case 17:
                                    gameplay = "大小单双投注";
                                    break;
                                default:
                                    gameplay = "任选投注";
                                    break;
                            }
                            //动态
                            string wText = "在[url=/bbs/game/HP3.aspx]" + GameName + "[/url]《" + gameplay + "》狂赚" + modelWin.WinMoney + "" + ub.Get("SiteBz") + "";
                            new BCW.BLL.Action().Add(25, id, modelWin.WinUserID, mename, wText);
                            //将该订单ID插入HP3Winner
                            new BCW.HP3.BLL.HP3Winner().UpdateWinZhu(model.ID, WinZhu);
                        }
                    }
                }
                try
                {
                    DataSet WillGets = new BCW.HP3.BLL.HP3Winner().GetLists(" ID=" + model.ID);
                    long WillGetss = Convert.ToInt64(WillGets.Tables[0].Rows[0][3]);
                    new BCW.HP3.BLL.HP3Buy().UpdateWillGet(model.ID, WillGetss);
                }
                catch
                {
                }
            }
            #endregion
        }
        else
        {
            #region 试玩
            double DwMon = 0;
            int WinZhu = 0;
            //取开奖号码表中数据
            BCW.HP3.Model.HP3_kjnum kj = new BCW.HP3.Model.HP3_kjnum();
            kj = new BCW.HP3.BLL.HP3_kjnum().GetDataByState(statesss.Trim());

            string state = kj.datenum;
            string winum = kj.Winum.Trim();
            string[] wnum = winum.Split(',');
            int EndSum = Convert.ToInt32(wnum[4]);
            //取用户订单数据
            DataSet ds = new BCW.HP3.BLL.HP3BuySY().GetList("*", "BuyDate=" + state);
            BCW.HP3.Model.HP3BuySY model = new BCW.HP3.Model.HP3BuySY();
            int n = ds.Tables[0].Rows.Count - 1;
            builder.Append("<br />本期共" + ds.Tables[0].Rows.Count + "条购彩记录<br />");
            for (; n >= 0; n--)
            {
                builder.Append(n + 1 + ".");
                model.ID = Convert.ToInt32(ds.Tables[0].Rows[n][0]);
                model.BuyID = Convert.ToInt32(ds.Tables[0].Rows[n][1]);
                model.BuyDate = Convert.ToString(ds.Tables[0].Rows[n][2]);
                model.BuyType = Convert.ToInt32(ds.Tables[0].Rows[n][3]);
                model.BuyNum = Convert.ToString(ds.Tables[0].Rows[n][4]);
                model.BuyMoney = Convert.ToInt64(ds.Tables[0].Rows[n][5]);
                model.BuyZhu = Convert.ToInt32(ds.Tables[0].Rows[n][6]);
                model.BuyTime = Convert.ToDateTime(ds.Tables[0].Rows[n][7]);
                model.Odds = Convert.ToDecimal(ds.Tables[0].Rows[n][8]);
                //取获奖订单数据
                BCW.HP3.Model.HP3WinnerSY modelWin = new BCW.HP3.Model.HP3WinnerSY();
                modelWin.ID = model.ID;
                modelWin.WinDate = model.BuyDate;
                modelWin.WinUserID = model.BuyID;
                modelWin.WinBool = 1;
                bool isis = new BCW.HP3.BLL.HP3WinnerSY().Exists(modelWin.ID);
                if (isis)
                {
                    builder.Append("获奖数据已存在!!!<br />");
                }
                else
                {
                    builder.Append("一条购奖数据成功判断!!!<br />");
                    string mename = new BCW.BLL.User().GetUsName(model.BuyID);
                    if (model.BuyType == 1)
                    {

                        #region 特殊方法
                        DwMon = Convert.ToDouble(model.Odds);
                        if (model.BuyNum.Trim() == "1" || model.BuyNum.Trim() == "2" || model.BuyNum.Trim() == "3" || model.BuyNum.Trim() == "4")
                        {
                            if (wnum[0].Trim() == model.BuyNum)
                            {
                                WinZhu = 1;
                                modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            }
                            if (wnum[0].Length > 2)
                            {

                                if (wnum[0].Substring(0, 2).Trim() == "19" && model.BuyNum.Trim() == "1")
                                {
                                    WinZhu = 1;
                                    modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                                }
                                if (wnum[0].Substring(0, 2).Trim() == "20" && model.BuyNum.Trim() == "2")
                                {
                                    WinZhu = 1;
                                    modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                                }
                                if (wnum[0].Substring(0, 2).Trim() == "21" && model.BuyNum.Trim() == "3")
                                {
                                    WinZhu = 1;
                                    modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                                }
                                if (wnum[0].Substring(0, 2).Trim() == "22" && model.BuyNum.Trim() == "4")
                                {
                                    WinZhu = 1;
                                    modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                                }
                            }

                        }
                        else if (model.BuyNum.Trim() == "5")
                        {
                            if (wnum[0].Trim() == "1" || wnum[0].Trim() == "2" || wnum[0].Trim() == "3" || wnum[0].Trim() == "4")
                            {
                                WinZhu = 1;
                                modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            }
                            if (wnum[0].Length > 2)
                            {

                                if (wnum[0].Substring(0, 2).Trim() == "19" || wnum[0].Substring(0, 2).Trim() == "20" || wnum[0].Substring(0, 2).Trim() == "21" || wnum[0].Substring(0, 2).Trim() == "22")
                                {
                                    WinZhu = 1;
                                    modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                                }

                            }
                        }
                        else if (model.BuyNum == "6" || model.BuyNum == "7" || model.BuyNum == "8" || model.BuyNum == "9" || model.BuyNum == "10" || model.BuyNum == "11" || model.BuyNum == "12" || model.BuyNum == "13" || model.BuyNum == "14" || model.BuyNum == "15" || model.BuyNum == "16" || model.BuyNum == "17")
                        {
                            if (wnum[0].Trim() == model.BuyNum)
                            {
                                WinZhu = 1;
                                modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            }
                            if (wnum[0].Length > 2)
                            {

                                if (wnum[0].Substring(2, 2) == model.BuyNum)
                                {
                                    WinZhu = 1;
                                    modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                                }
                                if (wnum[0].Substring(2, 2) == "06" && model.BuyNum == "06")
                                {
                                    WinZhu = 1;
                                    modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                                }
                                if (wnum[0].Substring(2, 2) == "07" && model.BuyNum == "07")
                                {
                                    WinZhu = 1;
                                    modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                                }
                                if (wnum[0].Substring(2, 2) == "08" && model.BuyNum == "08")
                                {
                                    WinZhu = 1;
                                    modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                                }
                                if (wnum[0].Substring(2, 2) == "09" && model.BuyNum == "09")
                                {
                                    WinZhu = 1;
                                    modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                                }
                            }
                        }
                        else if (model.BuyNum == "18")
                        {
                            if (wnum[0].Trim() == "6" || wnum[0].Trim() == "7" || wnum[0].Trim() == "8" || wnum[0].Trim() == "9" || wnum[0].Trim() == "10" || wnum[0].Trim() == "11" || wnum[0].Trim() == "12" || wnum[0].Trim() == "13" || wnum[0].Trim() == "14" || wnum[0].Trim() == "15" || wnum[0].Trim() == "16" || wnum[0].Trim() == "17")
                            {
                                WinZhu = 1;
                                modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            }
                            if (wnum[0].Length > 2)
                            {
                                if (wnum[0].Substring(2, 2).Trim() == "06" || wnum[0].Substring(2, 2).Trim() == "07" || wnum[0].Substring(2, 2).Trim() == "08" || wnum[0].Substring(2, 2).Trim() == "09" || wnum[0].Substring(2, 2).Trim() == "10" || wnum[0].Substring(2, 2).Trim() == "11" || wnum[0].Substring(2, 2).Trim() == "12" || wnum[0].Substring(2, 2).Trim() == "13" || wnum[0].Substring(2, 2).Trim() == "14" || wnum[0].Substring(2, 2).Trim() == "15" || wnum[0].Substring(2, 2).Trim() == "16" || wnum[0].Substring(2, 2).Trim() == "17")
                                {
                                    WinZhu = 1;
                                    modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                                }
                            }
                        }
                        else if (model.BuyNum == "19" || model.BuyNum == "20" || model.BuyNum == "21" || model.BuyNum == "22")
                        {
                            if (wnum[0].Length > 2)
                            {
                                if (wnum[0].Substring(0, 2).Trim() == model.BuyNum)
                                {
                                    WinZhu = 1;
                                    modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                                }
                            }
                        }
                        else if (model.BuyNum == "23")
                        {
                            if (wnum[0].Length > 2)
                            {
                                if (wnum[0].Substring(0, 2).Trim() == "19" || wnum[0].Substring(0, 2).Trim() == "20" || wnum[0].Substring(0, 2).Trim() == "21" || wnum[0].Substring(0, 2).Trim() == "22")
                                {
                                    WinZhu = 1;
                                    modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                                }
                            }
                        }
                        else if (model.BuyNum == "24" || model.BuyNum == "25" || model.BuyNum == "26" || model.BuyNum == "27" || model.BuyNum == "28" || model.BuyNum == "29" || model.BuyNum == "30" || model.BuyNum == "31" || model.BuyNum == "32" || model.BuyNum == "33" || model.BuyNum == "34" || model.BuyNum == "35" || model.BuyNum == "36")
                        {
                            if (wnum[0].Trim() == model.BuyNum)
                            {
                                WinZhu = 1;
                                modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            }
                        }
                        else if (model.BuyNum == "37")
                        {
                            if (wnum[0].Trim() == "24" || wnum[0].Trim() == "25" || wnum[0].Trim() == "26" || wnum[0].Trim() == "27" || wnum[0].Trim() == "28" || wnum[0].Trim() == "29" || wnum[0].Trim() == "30" || wnum[0].Trim() == "31" || wnum[0].Trim() == "32" || wnum[0].Trim() == "33" || wnum[0].Trim() == "34" || wnum[0].Trim() == "35" || wnum[0].Trim() == "36")
                            {
                                WinZhu = 1;
                                modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            }
                        }
                        else if (model.BuyNum == "38" || model.BuyNum == "39" || model.BuyNum == "40" || model.BuyNum == "41" || model.BuyNum == "42" || model.BuyNum == "43" || model.BuyNum == "44" || model.BuyNum == "45" || model.BuyNum == "46" || model.BuyNum == "47" || model.BuyNum == "48" || model.BuyNum == "49" || model.BuyNum == "50")
                        {
                            if (wnum[0].Trim() == model.BuyNum)
                            {
                                WinZhu = 1;
                                modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            }
                        }
                        else if (model.BuyNum == "51")
                        {
                            if (wnum[0].Trim() == "38" || wnum[0].Trim() == "39" || wnum[0].Trim() == "40" || wnum[0].Trim() == "41" || wnum[0].Trim() == "42" || wnum[0].Trim() == "43" || wnum[0].Trim() == "44" || wnum[0].Trim() == "45" || wnum[0].Trim() == "46" || wnum[0].Trim() == "47" || wnum[0].Trim() == "48" || wnum[0].Trim() == "49" || wnum[0].Trim() == "50")
                            {
                                WinZhu = 1;
                                modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            }
                        }
                        #endregion
                        string gameplay = "";
                        switch (model.BuyType)
                        {
                            case 1:
                                gameplay = "花色连号同号投注";
                                break;
                            case 17:
                                gameplay = "大小单双投注";
                                break;
                            default:
                                gameplay = "任选投注";
                                break;
                        }
                        if (WinZhu != 0)
                        {
                            int id = new BCW.HP3.BLL.HP3WinnerSY().Add(modelWin);
                            new BCW.BLL.Guest().Add(1, modelWin.WinUserID, mename, "您的" + GameName + ":" + modelWin.WinDate + "期已经开奖，获得了" + modelWin.WinMoney + "快乐币。[url=/bbs/game/HP3SW.aspx?act=case]马上兑奖[/url]");
                            //动态
                            string wText = "在[url=/bbs/game/HP3.aspx]" + GameName + "[/url]《" + gameplay + "》狂赚" + modelWin.WinMoney + "" + "快乐币" + "";
                            new BCW.BLL.Action().Add(25, id, modelWin.WinUserID, mename, wText);
                            //将该订单ID插入HP3WinnerSY
                            new BCW.HP3.BLL.HP3WinnerSY().UpdateWinZhu(model.ID, WinZhu);
                        }
                        WinZhu = 0;
                    }
                    else if (model.BuyType == 17)//大小单双
                    {
                        #region 大小单双
                        if (wnum[1] == wnum[2] && wnum[1] == wnum[3])
                        {
                        }
                        else
                        {
                            string big_small = "";
                            string single_double = "";
                            if (EndSum >= 5)
                            {
                                big_small = "1";
                            }
                            else
                            {
                                big_small = "2";
                            }
                            if (EndSum % 2 != 0)
                            {
                                single_double = "3";
                            }
                            else
                            {
                                single_double = "4";
                            }
                            if (model.BuyNum == big_small || model.BuyNum == single_double)
                            {
                                int xmlnum = 20 + Convert.ToInt32(model.BuyNum);
                                DwMon = Convert.ToDouble(model.Odds);
                                WinZhu = 1;
                                modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                                int id = new BCW.HP3.BLL.HP3WinnerSY().Add(modelWin);
                                new BCW.BLL.Guest().Add(1, modelWin.WinUserID, mename, "您的" + GameName + ":" + modelWin.WinDate + "期已经开奖，获得了" + modelWin.WinMoney + "快乐币。[url=/bbs/game/HP3SW.aspx?act=case]马上兑奖[/url]");
                                string gameplay = "";
                                switch (model.BuyType)
                                {
                                    case 1:
                                        gameplay = "花色连号同号投注";
                                        break;
                                    case 17:
                                        gameplay = "大小单双投注";
                                        break;
                                    default:
                                        gameplay = "任选投注";
                                        break;
                                }
                                //动态
                                string wText = "在[url=/bbs/game/HP3.aspx]" + GameName + "[/url]《" + gameplay + "》狂赚" + modelWin.WinMoney + "" + "快乐币" + "";
                                new BCW.BLL.Action().Add(25, id, modelWin.WinUserID, mename, wText);

                                //将该订单ID插入HP3WinnerSY
                                new BCW.HP3.BLL.HP3WinnerSY().UpdateWinZhu(model.ID, WinZhu);
                                WinZhu = 0;
                            }
                        }
                        #endregion
                    }
                    else if (model.BuyType == 6)//直一
                    {

                        #region 直一
                        if (model.BuyNum.Contains(wnum[1]) || model.BuyNum.Contains(wnum[2]) || model.BuyNum.Contains(wnum[3]))
                        {
                            #region MyRegion
                            WinZhu = 0;
                            DwMon = Convert.ToDouble(model.Odds);
                            if (wnum[0] == "24" || wnum[0] == "25" || wnum[0] == "26" || wnum[0] == "27" || wnum[0] == "28" || wnum[0] == "29" || wnum[0] == "30" || wnum[0] == "31" || wnum[0] == "32" || wnum[0] == "33" || wnum[0] == "34" || wnum[0] == "35" || wnum[0] == "36")
                            {
                                if (model.BuyNum.Contains(wnum[1]))
                                {
                                    WinZhu = 1;
                                }
                            }
                            else if (wnum[0] == "38" || wnum[0] == "39" || wnum[0] == "40" || wnum[0] == "41" || wnum[0] == "42" || wnum[0] == "43" || wnum[0] == "44" || wnum[0] == "45" || wnum[0] == "46" || wnum[0] == "47" || wnum[0] == "48" || wnum[0] == "49" || wnum[0] == "50")
                            {
                                if (wnum[1] == wnum[2])
                                {
                                    if (model.BuyNum.Contains(wnum[1]))
                                    {
                                        WinZhu = WinZhu + 1;
                                    }
                                    if (model.BuyNum.Contains(wnum[3]))
                                    {
                                        WinZhu = WinZhu + 1;
                                    }
                                }
                                if (wnum[1] == wnum[3])
                                {
                                    if (model.BuyNum.Contains(wnum[1]))
                                    {
                                        WinZhu = WinZhu + 1;
                                    }
                                    if (model.BuyNum.Contains(wnum[2]))
                                    {
                                        WinZhu = WinZhu + 1;
                                    }
                                }
                                if (wnum[2] == wnum[3])
                                {
                                    if (model.BuyNum.Contains(wnum[2]))
                                    {
                                        WinZhu = WinZhu + 1;
                                    }
                                    if (model.BuyNum.Contains(wnum[1]))
                                    {
                                        WinZhu = WinZhu + 1;
                                    }
                                }
                            }
                            else
                            {
                                if (model.BuyNum.Contains(wnum[1]))
                                {
                                    WinZhu = WinZhu + 1;
                                }
                                if (model.BuyNum.Contains(wnum[2]))
                                {
                                    WinZhu = WinZhu + 1;
                                }
                                if (model.BuyNum.Contains(wnum[3]))
                                {
                                    WinZhu = WinZhu + 1;
                                }
                            }
                            #endregion
                        }
                        #endregion
                        if (WinZhu != 0)
                        {
                            modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            int id = new BCW.HP3.BLL.HP3WinnerSY().Add(modelWin);
                            new BCW.BLL.Guest().Add(1, modelWin.WinUserID, mename, "您的" + GameName + ":" + modelWin.WinDate + "期已经开奖，获得了" + modelWin.WinMoney + "快乐币。[url=/bbs/game/HP3SW.aspx?act=case]马上兑奖[/url]");
                            string gameplay = "";
                            switch (model.BuyType)
                            {
                                case 1:
                                    gameplay = "花色连号同号投注";
                                    break;
                                case 17:
                                    gameplay = "大小单双投注";
                                    break;
                                default:
                                    gameplay = "任选投注";
                                    break;
                            }
                            //动态
                            string wText = "在[url=/bbs/game/HP3.aspx]" + GameName + "[/url]《" + gameplay + "》狂赚" + modelWin.WinMoney + "" + "快乐币" + "";
                            new BCW.BLL.Action().Add(25, id, modelWin.WinUserID, mename, wText);
                            //将该订单ID插入HP3WinnerSY
                            new BCW.HP3.BLL.HP3WinnerSY().UpdateWinZhu(model.ID, WinZhu);
                            WinZhu = 0;
                        }
                    }
                    else if (model.BuyType == 7)//直二
                    {
                        #region 直二
                        WinZhu = 0;
                        DwMon = Convert.ToDouble(model.Odds);
                        if (wnum[0] == "24" || wnum[0] == "25" || wnum[0] == "26" || wnum[0] == "27" || wnum[0] == "28" || wnum[0] == "29" || wnum[0] == "30" || wnum[0] == "31" || wnum[0] == "32" || wnum[0] == "33" || wnum[0] == "34" || wnum[0] == "35" || wnum[0] == "36")
                        {
                            WinZhu = 0;
                        }
                        else if (wnum[0] == "38" || wnum[0] == "39" || wnum[0] == "40" || wnum[0] == "41" || wnum[0] == "42" || wnum[0] == "43" || wnum[0] == "44" || wnum[0] == "45" || wnum[0] == "46" || wnum[0] == "47" || wnum[0] == "48" || wnum[0] == "49" || wnum[0] == "50")
                        {
                            if (model.BuyNum.Contains(wnum[1]) && model.BuyNum.Contains(wnum[2]) && model.BuyNum.Contains(wnum[3]))
                            {
                                WinZhu = 1;
                            }
                        }
                        else
                        {
                            if (model.BuyNum.Contains(wnum[1]) && model.BuyNum.Contains(wnum[2]))
                            {
                                WinZhu = WinZhu + 1;
                            }
                            if (model.BuyNum.Contains(wnum[2]) && model.BuyNum.Contains(wnum[3]))
                            {
                                WinZhu = WinZhu + 1;
                            }
                            if (model.BuyNum.Contains(wnum[1]) && model.BuyNum.Contains(wnum[3]))
                            {
                                WinZhu = WinZhu + 1;
                            }
                        }
                        #endregion
                        if (WinZhu != 0)
                        {
                            //将该订单ID插入HP3WinnerSY
                            modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            int id = new BCW.HP3.BLL.HP3WinnerSY().Add(modelWin);
                            new BCW.BLL.Guest().Add(1, modelWin.WinUserID, mename, "您的" + GameName + ":" + modelWin.WinDate + "期已经开奖，获得了" + modelWin.WinMoney + "快乐币。[url=/bbs/game/HP3SW.aspx?act=case]马上兑奖[/url]");
                            string gameplay = "";
                            switch (model.BuyType)
                            {
                                case 1:
                                    gameplay = "花色连号同号投注";
                                    break;
                                case 17:
                                    gameplay = "大小单双投注";
                                    break;
                                default:
                                    gameplay = "任选投注";
                                    break;
                            }
                            //动态
                            string wText = "在[url=/bbs/game/HP3.aspx]" + GameName + "[/url]《" + gameplay + "》狂赚" + modelWin.WinMoney + "" + "快乐币" + "";
                            new BCW.BLL.Action().Add(25, id, modelWin.WinUserID, mename, wText);
                            new BCW.HP3.BLL.HP3WinnerSY().UpdateWinZhu(model.ID, WinZhu);
                            //将该订单ID插入HP3WinnerSY
                            WinZhu = 0;
                        }
                    }

                    else if (model.BuyType == 9 || model.BuyType == 11 || model.BuyType == 13 || model.BuyType == 15)//直三、四、五、六
                    {
                        #region 直三、四、五、六
                        WinZhu = 0;
                        DwMon = Convert.ToDouble(model.Odds);
                        if (model.BuyNum.Contains(wnum[1]) && model.BuyNum.Contains(wnum[2]) && model.BuyNum.Contains(wnum[3]))
                        {
                            MatchCollection leyr = Regex.Matches(model.BuyNum, ",");
                            int ele = leyr.Count + 1;
                            if (model.BuyType == 9)
                            {
                                WinZhu = 1;
                            }
                            else if (model.BuyType == 11)
                            {
                                WinZhu = ele - 3;
                            }
                            else if (model.BuyType == 13)
                            {
                                WinZhu = (ele - 3) * (ele - 4) / 2;
                            }
                            else if (model.BuyType == 15)
                            {
                                WinZhu = (ele - 3) * (ele - 4) * (ele - 5) / 6;
                            }
                        }
                        #endregion
                        if (WinZhu != 0)
                        {
                            modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            int id = new BCW.HP3.BLL.HP3WinnerSY().Add(modelWin);         //将该订单ID插入HP3WinnerSY
                            new BCW.BLL.Guest().Add(1, modelWin.WinUserID, mename, "您的" + GameName + ":" + modelWin.WinDate + "期已经开奖，获得了" + modelWin.WinMoney + "快乐币。[url=/bbs/game/HP3SW.aspx?act=case]马上兑奖[/url]");
                            string gameplay = "";
                            switch (model.BuyType)
                            {
                                case 1:
                                    gameplay = "花色连号同号投注";
                                    break;
                                case 17:
                                    gameplay = "大小单双投注";
                                    break;
                                default:
                                    gameplay = "任选投注";
                                    break;
                            }
                            //动态
                            string wText = "在[url=/bbs/game/HP3.aspx]" + GameName + "[/url]《" + gameplay + "》狂赚" + modelWin.WinMoney + "" + "快乐币" + "";
                            new BCW.BLL.Action().Add(25, id, modelWin.WinUserID, mename, wText);
                            new BCW.HP3.BLL.HP3WinnerSY().UpdateWinZhu(model.ID, WinZhu);
                            //将该订单ID插入HP3WinnerSY
                            WinZhu = 0;
                        }
                    }
                    else if (model.BuyType == 8)//胆二
                    {
                        #region 胆二
                        WinZhu = 0;
                        DwMon = Convert.ToDouble(model.Odds);
                        string[] buynum = model.BuyNum.Split('#');
                        if (buynum[0].Contains(wnum[1]) || buynum[0].Contains(wnum[2]) || buynum[0].Contains(wnum[3]))
                        {
                            if (wnum[0] == "24" || wnum[0] == "25" || wnum[0] == "26" || wnum[0] == "27" || wnum[0] == "28" || wnum[0] == "29" || wnum[0] == "30" || wnum[0] == "31" || wnum[0] == "32" || wnum[0] == "33" || wnum[0] == "34" || wnum[0] == "35" || wnum[0] == "36")
                            {
                                WinZhu = 0;
                            }
                            else if (wnum[0] == "38" || wnum[0] == "39" || wnum[0] == "40" || wnum[0] == "41" || wnum[0] == "42" || wnum[0] == "43" || wnum[0] == "44" || wnum[0] == "45" || wnum[0] == "46" || wnum[0] == "47" || wnum[0] == "48" || wnum[0] == "49" || wnum[0] == "50")
                            {
                                if (buynum[1].Contains(wnum[1]) || buynum[1].Contains(wnum[2]) || buynum[1].Contains(wnum[3]))
                                {
                                    WinZhu = 1;
                                }
                            }
                            else
                            {
                                if (buynum[1].Contains(wnum[1]))
                                {
                                    WinZhu = WinZhu + 1;
                                }
                                if (buynum[1].Contains(wnum[2]))
                                {
                                    WinZhu = WinZhu + 1;
                                }
                                if (buynum[1].Contains(wnum[3]))
                                {
                                    WinZhu = WinZhu + 1;
                                }
                            }
                        }
                        #endregion
                        if (WinZhu != 0)
                        {
                            modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            int id = new BCW.HP3.BLL.HP3WinnerSY().Add(modelWin);
                            new BCW.BLL.Guest().Add(1, modelWin.WinUserID, mename, "您的" + GameName + ":" + modelWin.WinDate + "期已经开奖，获得了" + modelWin.WinMoney + "快乐币。[url=/bbs/game/HP3SW.aspx?act=case]马上兑奖[/url]");
                            string gameplay = "";
                            switch (model.BuyType)
                            {
                                case 1:
                                    gameplay = "花色连号同号投注";
                                    break;
                                case 17:
                                    gameplay = "大小单双投注";
                                    break;
                                default:
                                    gameplay = "任选投注";
                                    break;
                            }
                            //动态
                            string wText = "在[url=/bbs/game/HP3.aspx]" + GameName + "[/url]《" + gameplay + "》狂赚" + modelWin.WinMoney + "" + "快乐币" + "";
                            new BCW.BLL.Action().Add(25, id, modelWin.WinUserID, mename, wText);
                            new BCW.HP3.BLL.HP3WinnerSY().UpdateWinZhu(model.ID, WinZhu);
                            //将该订单ID插入HP3WinnerSY
                            WinZhu = 0;
                        }

                    }
                    else if (model.BuyType == 10)//胆三
                    {
                        #region 胆三
                        WinZhu = 0;
                        DwMon = Convert.ToDouble(model.Odds);
                        string[] buynum = model.BuyNum.Split('#');
                        MatchCollection dmnum = Regex.Matches(buynum[0], ",");
                        MatchCollection leyr = Regex.Matches(buynum[1], ",");
                        int dmn = dmnum.Count + 1;
                        int ele = leyr.Count + 1;
                        if (wnum[0] == "24" || wnum[0] == "25" || wnum[0] == "26" || wnum[0] == "27" || wnum[0] == "28" || wnum[0] == "29" || wnum[0] == "30" || wnum[0] == "31" || wnum[0] == "32" || wnum[0] == "33" || wnum[0] == "34" || wnum[0] == "35" || wnum[0] == "36")
                        {
                            WinZhu = 0;
                        }
                        else if (wnum[0] == "38" || wnum[0] == "39" || wnum[0] == "40" || wnum[0] == "41" || wnum[0] == "42" || wnum[0] == "43" || wnum[0] == "44" || wnum[0] == "45" || wnum[0] == "46" || wnum[0] == "47" || wnum[0] == "48" || wnum[0] == "49" || wnum[0] == "50")
                        {
                            WinZhu = 0;
                        }
                        else
                        {
                            if (model.BuyNum.Contains(wnum[1]) && model.BuyNum.Contains(wnum[2]) && model.BuyNum.Contains(wnum[3]))
                            {
                                if (dmn == 1)
                                {
                                    if (buynum[0].Contains(wnum[1]) || buynum[0].Contains(wnum[2]) || buynum[0].Contains(wnum[3]))
                                    {
                                        WinZhu = 1;
                                    }
                                }
                                else if (dmn == 2)
                                {
                                    if (buynum[0].Contains(wnum[1]) && buynum[0].Contains(wnum[2]))
                                    {
                                        WinZhu = 1;
                                    }
                                    if (buynum[0].Contains(wnum[2]) && buynum[0].Contains(wnum[3]))
                                    {
                                        WinZhu = 1;
                                    }
                                    if (buynum[0].Contains(wnum[1]) && buynum[0].Contains(wnum[3]))
                                    {
                                        WinZhu = 1;
                                    }
                                }
                            }
                        }
                        #endregion
                        if (WinZhu != 0)
                        {
                            modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            int id = new BCW.HP3.BLL.HP3WinnerSY().Add(modelWin);
                            new BCW.BLL.Guest().Add(1, modelWin.WinUserID, mename, "您的" + GameName + ":" + modelWin.WinDate + "期已经开奖，获得了" + modelWin.WinMoney + "快乐币。[url=/bbs/game/HP3SW.aspx?act=case]马上兑奖[/url]");
                            string gameplay = "";
                            switch (model.BuyType)
                            {
                                case 1:
                                    gameplay = "花色连号同号投注";
                                    break;
                                case 17:
                                    gameplay = "大小单双投注";
                                    break;
                                default:
                                    gameplay = "任选投注";
                                    break;
                            }
                            //动态
                            string wText = "在[url=/bbs/game/HP3.aspx]" + GameName + "[/url]《" + gameplay + "》狂赚" + modelWin.WinMoney + "" + "快乐币" + "";
                            new BCW.BLL.Action().Add(25, id, modelWin.WinUserID, mename, wText);
                            new BCW.HP3.BLL.HP3WinnerSY().UpdateWinZhu(model.ID, WinZhu);
                            //将该订单ID插入HP3WinnerSY
                            WinZhu = 0;
                        }
                    }
                    else if (model.BuyType == 12 || model.BuyType == 14 || model.BuyType == 16)//胆四、五、六
                    {
                        #region 胆四、五、六
                        WinZhu = 0;
                        if (wnum[0] == "24" || wnum[0] == "25" || wnum[0] == "26" || wnum[0] == "27" || wnum[0] == "28" || wnum[0] == "29" || wnum[0] == "30" || wnum[0] == "31" || wnum[0] == "32" || wnum[0] == "33" || wnum[0] == "34" || wnum[0] == "35" || wnum[0] == "36")
                        {
                            WinZhu = 0;
                        }
                        else if (wnum[0] == "38" || wnum[0] == "39" || wnum[0] == "40" || wnum[0] == "41" || wnum[0] == "42" || wnum[0] == "43" || wnum[0] == "44" || wnum[0] == "45" || wnum[0] == "46" || wnum[0] == "47" || wnum[0] == "48" || wnum[0] == "49" || wnum[0] == "50")
                        {
                            WinZhu = 0;
                        }
                        else
                        {
                            DwMon = Convert.ToDouble(model.Odds);
                            string[] buynum = model.BuyNum.Split('#');
                            MatchCollection dmnum = Regex.Matches(buynum[0], ",");
                            MatchCollection leyr = Regex.Matches(buynum[1], ",");
                            int dmn = dmnum.Count + 1;
                            int ele = leyr.Count + 1;
                            if (model.BuyNum.Contains(wnum[1]) && model.BuyNum.Contains(wnum[2]) && model.BuyNum.Contains(wnum[3]))
                            {
                                if (model.BuyType == 12)
                                {
                                    #region 胆四
                                    if (buynum[0].Contains(wnum[1]) && buynum[0].Contains(wnum[2]) && buynum[0].Contains(wnum[3]))
                                    {
                                        WinZhu = ele;
                                    }
                                    else if (buynum[0].Contains(wnum[1]) && buynum[0].Contains(wnum[2]))
                                    {
                                        if (dmn == 3)
                                        {
                                            WinZhu = 1;
                                        }
                                        else if (dmn == 2)
                                        {
                                            WinZhu = ele - 1;
                                        }

                                    }
                                    else if (buynum[0].Contains(wnum[2]) && buynum[0].Contains(wnum[3]))
                                    {
                                        if (dmn == 3)
                                        {
                                            WinZhu = 1;
                                        }
                                        else if (dmn == 2)
                                        {
                                            WinZhu = ele - 1;
                                        }
                                    }
                                    else if (buynum[0].Contains(wnum[1]) && buynum[0].Contains(wnum[3]))
                                    {
                                        if (dmn == 3)
                                        {
                                            WinZhu = 1;
                                        }
                                        else if (dmn == 2)
                                        {
                                            WinZhu = ele - 1;
                                        }
                                    }
                                    else if (buynum[0].Contains(wnum[1]) || buynum[0].Contains(wnum[2]) || buynum[0].Contains(wnum[3]))
                                    {
                                        if (dmn == 2)
                                        {
                                            WinZhu = 1;
                                        }
                                        else if (dmn == 1)
                                        {
                                            WinZhu = ele - 2;
                                        }
                                    }

                                    #endregion
                                }
                                else if (model.BuyType == 14)
                                {
                                    #region 胆五
                                    if (buynum[0].Contains(wnum[1]) && buynum[0].Contains(wnum[2]) && buynum[0].Contains(wnum[3]))
                                    {
                                        if (dmn == 4)
                                        {
                                            WinZhu = ele;
                                        }
                                        else if (dmn == 3)
                                        {
                                            WinZhu = ele * (ele - 1) / 2;
                                        }

                                    }
                                    else if (buynum[0].Contains(wnum[1]) && buynum[0].Contains(wnum[2]))
                                    {
                                        if (dmn == 4)
                                        {
                                            WinZhu = 1;
                                        }
                                        else if (dmn == 3)
                                        {
                                            WinZhu = ele - 1;
                                        }
                                        else if (dmn == 2)
                                        {
                                            WinZhu = (ele - 1) * (ele - 2) / 2;
                                        }
                                    }
                                    else if (buynum[0].Contains(wnum[2]) && buynum[0].Contains(wnum[3]))
                                    {
                                        if (dmn == 4)
                                        {
                                            WinZhu = 1;
                                        }
                                        else if (dmn == 3)
                                        {
                                            WinZhu = ele - 1;
                                        }
                                        else if (dmn == 2)
                                        {
                                            WinZhu = (ele - 1) * (ele - 2) / 2;
                                        }
                                    }
                                    else if (buynum[0].Contains(wnum[1]) && buynum[0].Contains(wnum[3]))
                                    {
                                        if (dmn == 4)
                                        {
                                            WinZhu = 1;
                                        }
                                        else if (dmn == 3)
                                        {
                                            WinZhu = ele - 1;
                                        }
                                        else if (dmn == 2)
                                        {
                                            WinZhu = (ele - 1) * (ele - 2) / 2;
                                        }
                                    }
                                    else if (buynum[0].Contains(wnum[1]) || buynum[0].Contains(wnum[2]) || buynum[0].Contains(wnum[3]))
                                    {
                                        if (dmn == 4)
                                        {
                                            WinZhu = 0;
                                        }
                                        else if (dmn == 3)
                                        {
                                            WinZhu = 1;
                                        }
                                        else if (dmn == 2)
                                        {
                                            WinZhu = ele - 2;
                                        }
                                        else if (dmn == 1)
                                        {
                                            WinZhu = (ele - 2) * (ele - 3) / 2;
                                        }
                                    }
                                    #endregion

                                }
                                else if (model.BuyType == 16)
                                {
                                    #region 胆六
                                    int s = 0;
                                    if (buynum[0].Contains(wnum[1]) && buynum[0].Contains(wnum[2]) && buynum[0].Contains(wnum[3]))
                                    {
                                        if (dmn == 5)
                                        {
                                            WinZhu = ele;
                                        }
                                        else if (dmn == 4)
                                        {
                                            WinZhu = ele * (ele - 1) / 2;
                                        }
                                        else if (dmn == 3)
                                        {
                                            WinZhu = ele * (ele - 1) * (ele - 2) / 6;
                                        }
                                    }
                                    else if (buynum[0].Contains(wnum[1]) && buynum[0].Contains(wnum[2]))
                                    {
                                        if (dmn == 5)
                                        {
                                            WinZhu = 1;
                                        }
                                        else if (dmn == 4)
                                        {
                                            WinZhu = ele - 1;
                                        }
                                        else if (dmn == 3)
                                        {
                                            WinZhu = (ele - 1) * (ele - 2) / 2;
                                        }
                                        else if (dmn == 2)
                                        {
                                            WinZhu = (ele - 1) * (ele - 2) * (ele - 3) / 6;
                                        }

                                    }
                                    else if (buynum[0].Contains(wnum[2]) && buynum[0].Contains(wnum[3]))
                                    {
                                        if (dmn == 5)
                                        {
                                            WinZhu = 1;
                                        }
                                        else if (dmn == 4)
                                        {
                                            WinZhu = ele - 1;
                                        }
                                        else if (dmn == 3)
                                        {
                                            WinZhu = (ele - 1) * (ele - 2) / 2;
                                        }
                                        else if (dmn == 2)
                                        {
                                            WinZhu = (ele - 1) * (ele - 2) * (ele - 3) / 6;
                                        }
                                    }
                                    else if (buynum[0].Contains(wnum[1]) && buynum[0].Contains(wnum[3]))
                                    {
                                        if (dmn == 5)
                                        {
                                            WinZhu = 1;
                                        }
                                        else if (dmn == 4)
                                        {
                                            WinZhu = ele - 1;
                                        }
                                        else if (dmn == 3)
                                        {
                                            WinZhu = (ele - 1) * (ele - 2) / 2;
                                        }
                                        else if (dmn == 2)
                                        {
                                            WinZhu = (ele - 1) * (ele - 2) * (ele - 3) / 6;
                                        }
                                    }
                                    else if (buynum[0].Contains(wnum[1]) || buynum[0].Contains(wnum[2]) || buynum[0].Contains(wnum[3]))
                                    {
                                        if (dmn == 4)
                                        {
                                            WinZhu = 1;
                                        }
                                        else if (dmn == 3)
                                        {
                                            WinZhu = ele - 2;
                                        }
                                        else if (dmn == 2)
                                        {
                                            WinZhu = (ele - 2) * (ele - 3) / 2;
                                        }
                                        else if (dmn == 1)
                                        {
                                            WinZhu = (ele - 2) * (ele - 3) * (ele - 4) / 6;
                                        }

                                    }
                                    #endregion
                                }
                            }
                        }
                        #endregion
                        if (WinZhu != 0)
                        {
                            modelWin.WinMoney = Convert.ToInt64(model.BuyMoney * DwMon * WinZhu);
                            int id = new BCW.HP3.BLL.HP3WinnerSY().Add(modelWin);
                            new BCW.HP3.BLL.HP3WinnerSY().UpdateWinZhu(model.ID, WinZhu);
                            new BCW.BLL.Guest().Add(1, modelWin.WinUserID, mename, "您的" + GameName + ":" + modelWin.WinDate + "期已经开奖，获得了" + modelWin.WinMoney + "快乐币。[url=/bbs/game/HP3SW.aspx?act=case]马上兑奖[/url]");
                            string gameplay = "";
                            switch (model.BuyType)
                            {
                                case 1:
                                    gameplay = "花色连号同号投注";
                                    break;
                                case 17:
                                    gameplay = "大小单双投注";
                                    break;
                                default:
                                    gameplay = "任选投注";
                                    break;
                            }
                            //动态
                            string wText = "在[url=/bbs/game/HP3.aspx]" + GameName + "[/url]《" + gameplay + "》狂赚" + modelWin.WinMoney + "" + "快乐币" + "";
                            new BCW.BLL.Action().Add(25, id, modelWin.WinUserID, mename, wText);

                            //将该订单ID插入HP3WinnerSY
                        }
                    }
                }
                try
                {
                    DataSet WillGets = new BCW.HP3.BLL.HP3WinnerSY().GetLists(" ID=" + model.ID);
                    long WillGetss = Convert.ToInt64(WillGets.Tables[0].Rows[0][3]);
                    new BCW.HP3.BLL.HP3BuySY().UpdateWillGet(model.ID, WillGetss);
                }
                catch
                {
                }
            }
            #endregion
        }

    }
    //游戏配置
    private void Set()
    {
        Master.Title = "游戏配置";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>&gt;");
        builder.Append("游戏配置");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-2]$", "0"));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");

        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置


        if (Utils.ToSChinese(ac) == "确定修改")
        {
            if (ptype == 0)
            {
                string Name = Utils.GetRequest("Name", "post", 2, @"^[^\^]{1,20}$", "系统名称限1-20字内");
                string Logo = Utils.GetRequest("Logo", "post", 3, @"^[^\^]{1,200}$", "Logo地址限200字内");
                string Sec = Utils.GetRequest("Sec", "post", 2, @"^[0-9]\d*$", "秒数填写出错");
                string SmallPay = Utils.GetRequest("SmallPay", "post", 2, @"^[0-9]\d*$", "最小下注" + ub.Get("SiteBz") + "填写错误");
                string BigPay = Utils.GetRequest("BigPay", "post", 2, @"^[0-9]\d*$", "最大下注" + ub.Get("SiteBz") + "填写错误");
                string Price = Utils.GetRequest("Price", "post", 2, @"^[0-9]\d*$", "每期每ID限购多少" + ub.Get("SiteBz") + "填写错误");
                string Expir = Utils.GetRequest("Expir", "post", 2, @"^[0-9]\d*$", "防刷秒数填写出错");
                string OnTime = Utils.GetRequest("OnTime", "post", 3, @"^[^\^]{1,20000}$", "游戏规则填写错误");
                string Foot = Utils.GetRequest("Foot", "post", 3, @"^[\s\S]{1,2000}$", "底部Ubb限2000字内");
                string Top = Utils.GetRequest("Top", "post", 3, @"^[\s\S]{1,2000}$", "顶部Ubb限2000字内");
                string GuestSet = Utils.GetRequest("GuestSet", "post", 2, @"^[0-1]$", "兑奖内线选择出错");

                string LianTing = Utils.GetRequest("LianTing", "post", 2, @"^[0-9]\d*$", "填写出错");
                string SWB = Utils.GetRequest("SWB", "post", 2, @"^[0-1]$", "正式版与选择出错");
                ////继续验证时间
                //if (OnTime != "")
                //{
                //    string[] temp = OnTime.Split("-".ToCharArray());
                //    DateTime dt1 = Utils.ParseTime(temp[0]);
                //    DateTime dt2 = Utils.ParseTime(temp[1]);
                //}
                if (SWB == "0")
                {
                    xml.dss["HP3Name"] = Name.Replace("_试玩版", "");
                }
                else
                {
                    xml.dss["HP3Name"] = Name + "_试玩版";
                }
                xml.dss["HP3Logo"] = Logo;
                xml.dss["HP3Sec"] = Sec;
                xml.dss["HP3SmallPay"] = SmallPay;
                xml.dss["HP3BigPay"] = BigPay;
                xml.dss["HP3Price"] = Price;
                xml.dss["HP3Expir"] = Expir;
                xml.dss["HP3OnTime"] = OnTime;
                xml.dss["HP3Foot"] = Foot;
                xml.dss["HP3Top"] = Top;
                xml.dss["HP3GuestSet"] = GuestSet;

                xml.dss["HP3LianTing"] = LianTing;
                xml.dss["SWB"] = SWB;
            }
            else if (ptype == 1)
            {
                string[] Odds = new string[25];
                for (int i = 1; i <= 24; i++)
                {
                    Odds[i] = Utils.GetRequest("Odds" + i + "", "post", 2, @"^[1-9]\d*\.\d*|0\.\d*[1-9]\d*$|^[\d]{1,10}$", "赔率错误");
                }

                xml.dss["HP3TH2"] = Odds[1];
                xml.dss["HP3TH1"] = Odds[2];
                xml.dss["HP3THS2"] = Odds[3];
                xml.dss["HP3THS1"] = Odds[4];
                xml.dss["HP3SZ2"] = Odds[5];
                xml.dss["HP3SZ1"] = Odds[6];
                xml.dss["HP3BZ2"] = Odds[7];
                xml.dss["HP3BZ1"] = Odds[8];
                xml.dss["HP3DZ2"] = Odds[9];
                xml.dss["HP3DZ1"] = Odds[10];
                xml.dss["HP3RX1"] = Odds[11];
                xml.dss["HP3RX2"] = Odds[12];
                xml.dss["HP3RX3"] = Odds[13];
                xml.dss["HP3RX4"] = Odds[14];
                xml.dss["HP3RX5"] = Odds[15];
                xml.dss["HP3RX6"] = Odds[16];
                xml.dss["HP3DA"] = Odds[17];
                xml.dss["HP3XIAO"] = Odds[18];
                xml.dss["HP3DAN"] = Odds[19];
                xml.dss["HP3SHUANG"] = Odds[20];
                xml.dss["HP3FUDONG"] = Odds[21];
                xml.dss["HP3GU"] = Odds[22];
                xml.dss["Oddsmax"] = Odds[23];
                xml.dss["Oddsmin"] = Odds[24];
            }
            else
            {
                for (int i = 1; i <= 13; i++)
                {
                    string Oddsc = "";
                    Oddsc = Utils.GetRequest("Oddsc" + i + "", "all", 2, @"^[0-9]\d*$", "下注方式上限错误" + i + "");

                    xml.dss["Oddsc" + i + ""] = Oddsc;
                }
            }

            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("" + GameName + "设置", "设置成功，正在返回..", Utils.getUrl("HP3.aspx?act=set&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            if (ptype == 0)
            {
                builder.Append("" + GameName + "设置|");
                builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=set&amp;ptype=1") + "\">赔率</a>|");
                builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=set&amp;ptype=2") + "\">投注限额</a>");
            }
            else if (ptype == 1)
            {
                builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=set&amp;ptype=0") + "\">" + GameName + "设置</a>");
                builder.Append("|赔率|");
                builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=set&amp;ptype=2") + "\">投注限额</a>");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=set&amp;ptype=0") + "\">" + GameName + "设置</a>|");
                builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=set&amp;ptype=1") + "\">赔率</a>|");

                builder.Append("投注限额");
            }
            builder.Append(Out.Tab("</div>", ""));
            if (ptype == 0)
            {
                string strText = "游戏名称:/,游戏Logo(可留空):/,离截止时间N秒前不能下注/,最小下注" + ub.Get("SiteBz") + ":/,最大下注" + ub.Get("SiteBz") + ":/,每期每ID限购多少" + ub.Get("SiteBz") + "(填0则不限制):/,下注防刷(秒):/,规则:/,顶部Ubb:/,底部Ubb:/,兑奖内线:/,限制最大连停期数:/,切换正式版与试玩版:/,";
                string strName = "Name,Logo,Sec,SmallPay,BigPay,Price,Expir,OnTime,Top,Foot,GuestSet,LianTing,SWB,backurl";
                string strType = "text,text,num,num,num,num,num,textarea,textarea,textarea,select,num,select,hidden";
                string strValu = "" + xml.dss["HP3Name"] + "'" + xml.dss["HP3Logo"] + "'" + xml.dss["HP3Sec"] + "'" + xml.dss["HP3SmallPay"] + "'" + xml.dss["HP3BigPay"] + "'" + xml.dss["HP3Price"] + "'" + xml.dss["HP3Expir"] + "'" + xml.dss["HP3OnTime"] + "'" + xml.dss["HP3Top"] + "'" + xml.dss["HP3Foot"] + "'" + xml.dss["HP3GuestSet"] + "'" + xml.dss["HP3LianTing"] + "'" + xml.dss["SWB"] + "'" + Utils.getPage(0) + "";
                string strEmpt = "false,true,false,false,false,false,false,true,true,true,0|开启|1|关闭,flase,0|正式版|1|试玩版,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,HP3.aspx?act=set,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("温馨提示:<br /><b style=\"color:red\">注意切换版本后台管理也会立即切换.</b>每日大小单双出现次数差值上限将对赔率最大产生影响。");
                builder.Append(Out.Tab("</div>", ""));
            }
            else if (ptype == 1)
            {
                string strText = "同花单选:,同花包选:,同花顺单选:,同花顺包选:,顺子单选:,顺子包选:,豹子单选:,豹子包选:,对子单选:,对子包选:,任选一:,任选二:,任选三:,任选四:,任选五:,任选六:,大:,小:,单:,双:,大小单双固定赔率:/,大小单双浮动赔率:/,大小单双赔率浮动上限:/,大小单双赔率浮动下限:/,,";
                string strName = "Odds1,Odds2,Odds3,Odds4,Odds5,Odds6,Odds7,Odds8,Odds9,Odds10,Odds11,Odds12,Odds13,Odds14,Odds15,Odds16,Odds17,Odds18,Odds19,Odds20,Odds22,Odds21,Odds23,Odds24,ptype,backurl";
                string strType = "num,num,num,text,num,text,num,num,num,text,text,text,num,num,num,num,text,text,text,text,text,text,text,text,hidden,hidden";
                string strValu = "" + xml.dss["HP3TH2"] + "'" + xml.dss["HP3TH1"] + "'" + xml.dss["HP3THS2"] + "'" + xml.dss["HP3THS1"] + "'" + xml.dss["HP3SZ2"] + "'" + xml.dss["HP3SZ1"] + "'" + xml.dss["HP3BZ2"] + "'" + xml.dss["HP3BZ1"] + "'" + xml.dss["HP3DZ2"] + "'" + xml.dss["HP3DZ1"] + "'" + xml.dss["HP3RX1"] + "'" + xml.dss["HP3RX2"] + "'" + xml.dss["HP3RX3"] + "'" + xml.dss["HP3RX4"] + "'" + xml.dss["HP3RX5"] + "'" + xml.dss["HP3RX6"] + "'" + xml.dss["HP3DA"] + "'" + xml.dss["HP3XIAO"] + "'" + xml.dss["HP3DAN"] + "'" + xml.dss["HP3SHUANG"] + "'" + xml.dss["HP3GU"] + "'" + xml.dss["HP3FUDONG"] + "'" + xml.dss["Oddsmax"] + "'" + xml.dss["Oddsmin"] + "'1'" + Utils.getPage(0) + "";
                string strEmpt = "false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,HP3.aspx?act=set,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("温馨提示:<br />大小单双浮动赔率调为0，可使大小单双赔率为固定赔率，否则赔率将随当天开出大小单双的次数差而产生浮动。");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                string strText = "同花:,顺子:,同花顺:,豹子:,对子:,任选一:,任选二:,任选三:,任选四:,任选五:,任选六:,大小:,单双:,,";
                string strName = "Oddsc1,Oddsc2,Oddsc3,Oddsc4,Oddsc5,Oddsc6,Oddsc7,Oddsc8,Oddsc9,Oddsc10,Oddsc11,Oddsc12,Oddsc13,ptype,backurl";
                string strType = "num,num,num,num,num,num,num,num,num,num,num,num,num,hidden,hidden";
                string strValu = "" + xml.dss["Oddsc1"] + "'" + xml.dss["Oddsc2"] + "'" + xml.dss["Oddsc3"] + "'" + xml.dss["Oddsc4"] + "'" + xml.dss["Oddsc5"] + "'" + xml.dss["Oddsc6"] + "'" + xml.dss["Oddsc7"] + "'" + xml.dss["Oddsc8"] + "'" + xml.dss["Oddsc9"] + "'" + xml.dss["Oddsc10"] + "'" + xml.dss["Oddsc11"] + "'" + xml.dss["Oddsc12"] + "'" + xml.dss["Oddsc13"] + "'2'" + Utils.getPage(0) + "";
                string strEmpt = "false,false,false,false,false,false,false,false,false,false,false,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,HP3.aspx?act=set,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
        }

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("HP3.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    //用户排行
    private void UserBang()
    {
        if (SWB == 0)
        {
            #region 正式版
            Master.Title = "用户排行";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
            builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>&gt;");
            builder.Append("用户排行");
            builder.Append(Out.Tab("</div>", "<br />"));
            int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));
            builder.Append(Out.Tab("<div>", ""));
            if (ptype == 1)
                builder.Append("获奖排行|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=bang&amp;ptype=1") + "\">获奖排行</a>|");

            if (ptype == 2)
                builder.Append("购奖排行|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=bang&amp;ptype=2") + "\">购奖排行</a>|");
            if (ptype == 3)
                builder.Append("净赚排行");
            else
                builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=bang&amp;ptype=3") + "\">净赚排行</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            int CID1 = new BCW.HP3.BLL.HP3_kjnum().GetXXCID("datenum>0 order by datenum asc");
            int CID2 = new BCW.HP3.BLL.HP3_kjnum().GetXXCID("datenum>0 order by datenum desc");
            int startstate = int.Parse(Utils.GetRequest("startstate", "all", 1, @"^[1-9]\d*$", "" + CID1 + ""));
            int endstate = int.Parse(Utils.GetRequest("endstate", "all", 1, @"^[1-9]\d*$", "" + CID2 + ""));
            string rewardid = "";
            int pageIndex;
            if (ptype == 1)
            {
                int recordCount;
                string strWhere = string.Empty;
                int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
                string[] pageValUrl = { "act", "startstate", "endstate", "ptype", "backurl" };
                pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                if (pageIndex == 0)
                    pageIndex = 1;
                builder.Append(Out.Tab("<div>", ""));
                if (startstate == CID1 && endstate == CID2)
                {
                    builder.Append("总排名为：");
                }
                else
                {
                    builder.Append("第" + startstate + "期至第" + endstate + "期排名为：");
                }
                builder.Append(Out.Tab("</div>", "<br/>"));
                DataSet rowbang = new BCW.HP3.BLL.HP3Winner().GetListBang2(startstate.ToString(), endstate.ToString());
                recordCount = rowbang.Tables[0].Rows.Count;
                DataSet bang = new BCW.HP3.BLL.HP3Winner().GetListByPage2(0, recordCount, startstate.ToString(), endstate.ToString());
                if (recordCount >= 0)
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
                            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                        else
                        {
                            if (k == 1)
                                builder.Append(Out.Tab("<div>", ""));
                            else
                                builder.Append(Out.Tab("<div>", "<br />"));
                        }
                        string mename = new BCW.BLL.User().GetUsName(usid);
                        int wd = (pageIndex - 1) * 10 + k;
                        builder.Append("[<b style=\"color:red\">TOP" + wd + "</b>].<a  href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + mename + "(" + usid + ")</a>兑奖" + "<b style=\"color:red\">" + usmoney + "</b>" + ub.Get("SiteBz") + "");
                        k++;
                        builder.Append(Out.Tab("</div>", ""));
                        rewardid = rewardid + usid.ToString() + " ";
                    }
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                }
                else
                {
                    builder.Append(Out.Div("div", "没有相关记录.."));
                }
            }
            else if (ptype == 3)
            {
                int recordCount;
                string strWhere = string.Empty;
                int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
                string[] pageValUrl = { "act", "startstate", "endstate", "ptype", "backurl" };
                pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                if (pageIndex == 0)
                    pageIndex = 1;
                builder.Append(Out.Tab("<div>", ""));
                if (startstate == CID1 && endstate == CID2)
                {
                    builder.Append("总排名为：");
                }
                else
                {
                    builder.Append("第" + startstate + "期至第" + endstate + "期排名为：");
                }
                builder.Append(Out.Tab("</div>", "<br/>"));
                DataSet rowbang = new BCW.HP3.BLL.HP3Buy().GetBang(startstate.ToString(), endstate.ToString());
                recordCount = rowbang.Tables[0].Rows.Count;
                DataSet bang = new BCW.HP3.BLL.HP3Buy().GetBangByPage(0, recordCount, startstate.ToString(), endstate.ToString());
                if (recordCount >= 0)
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
                            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                        else
                        {
                            if (k == 1)
                                builder.Append(Out.Tab("<div>", ""));
                            else
                                builder.Append(Out.Tab("<div>", "<br />"));
                        }
                        string mename = new BCW.BLL.User().GetUsName(usid);
                        int wd = (pageIndex - 1) * 10 + k;
                        builder.Append("[<b style=\"color:red\">TOP" + wd + "</b>].<a   href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + mename + "(" + usid + ")</a>净赚" + "<b style=\"color:red\">" + usmoney + "</b>" + ub.Get("SiteBz") + "");
                        k++;
                        builder.Append(Out.Tab("</div>", ""));
                        rewardid = rewardid + usid.ToString() + " ";
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

                int recordCount;
                string strWhere = string.Empty;
                int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
                string[] pageValUrl = { "act", "startstate", "endstate", "ptype", "backurl" };
                pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                if (pageIndex == 0)
                    pageIndex = 1;
                builder.Append(Out.Tab("<div>", ""));
                if (startstate == CID1 && endstate == CID2)
                {
                    builder.Append("总排名为：");
                }
                else
                {
                    builder.Append("第" + startstate + "期至第" + endstate + "期排名为：");
                }
                builder.Append(Out.Tab("</div>", "<br/>"));
                DataSet rowbang = new BCW.HP3.BLL.HP3Buy().GetListBang2(startstate.ToString(), endstate.ToString());
                recordCount = rowbang.Tables[0].Rows.Count;
                DataSet bang = new BCW.HP3.BLL.HP3Buy().GetListByPage(0, recordCount, startstate.ToString(), endstate.ToString());
                if (recordCount >= 0)
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
                            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                        else
                        {
                            if (k == 1)
                                builder.Append(Out.Tab("<div>", ""));
                            else
                                builder.Append(Out.Tab("<div>", "<br />"));
                        }
                        string mename = new BCW.BLL.User().GetUsName(usid);
                        int wd = (pageIndex - 1) * 10 + k;
                        builder.Append("[<b style=\"color:red\">TOP" + wd + "</b>].<a   href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + mename + "(" + usid + ")</a>购奖" + "<b style=\"color:red\">" + usmoney + "</b>" + ub.Get("SiteBz") + "");
                        k++;
                        builder.Append(Out.Tab("</div>", ""));
                        rewardid = rewardid + usid.ToString() + " ";
                    }
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                }
                else
                {
                    builder.Append(Out.Div("div", "没有相关记录.."));
                }

            }

            string strText = "输入开始期号:/,输入截止期号:/,";
            string strName = "startstate,endstate,backurl";
            string strType = "num,num,hidden";
            string strValu = "'" + "'" + Utils.getPage(0) + "";
            string strEmpt = "true,true,false";
            string strIdea = "/";
            string strOthe = "搜一搜,HP3.aspx?act=bang&amp;ptype=" + ptype + ",post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            string wdy = "";
            if (pageIndex == 1)
                wdy = "TOP10";
            else
                wdy = "TOP" + (pageIndex - 1).ToString() + "1-" + pageIndex.ToString() + "0";
            builder.Append(wdy + " ID:" + rewardid);
            builder.Append(Out.Tab("</div>", ""));
            string strText2 = ",,,,";
            string strName2 = "startstate,endstate,pageIndex,rewardid,backurl";
            string strType2 = "hidden,hidden,hidden,hidden,hidden";
            string strValu2 = startstate + "'" + endstate + "'" + pageIndex + "'" + rewardid + "'" + Utils.getPage(0) + "";
            string strEmpt2 = "true,true,false";
            string strIdea2 = "/";
            string strOthe2 = wdy + "奖励发放,HP3.aspx?act=ReWard&amp;ptype=" + ptype + ",post,1,red";
            builder.Append(Out.wapform(strText2, strName2, strType2, strValu2, strEmpt2, strIdea2, strOthe2));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("HP3.aspx") + "\">返回上一级</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
            #endregion
        }
        else
        {
            #region
            Master.Title = "用户排行";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
            builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>&gt;");
            builder.Append("用户排行");
            builder.Append(Out.Tab("</div>", "<br />"));
            int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));
            builder.Append(Out.Tab("<div>", ""));
            if (ptype == 1)
                builder.Append("获奖排行|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=bang&amp;ptype=1") + "\">获奖排行</a>|");

            if (ptype == 2)
                builder.Append("购奖排行|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=bang&amp;ptype=2") + "\">购奖排行</a>|");
            if (ptype == 3)
                builder.Append("净赚排行");
            else
                builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=bang&amp;ptype=3") + "\">净赚排行</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            int CID1 = new BCW.HP3.BLL.HP3_kjnum().GetXXCID("datenum>0 order by datenum asc");
            int CID2 = new BCW.HP3.BLL.HP3_kjnum().GetXXCID("datenum>0 order by datenum desc");
            int startstate = int.Parse(Utils.GetRequest("startstate", "all", 1, @"^[1-9]\d*$", "" + CID1 + ""));
            int endstate = int.Parse(Utils.GetRequest("endstate", "all", 1, @"^[1-9]\d*$", "" + CID2 + ""));
            string rewardid = "";
            int pageIndex;
            if (ptype == 1)
            {
                int recordCount;
                string strWhere = string.Empty;
                int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
                string[] pageValUrl = { "act", "startstate", "endstate", "ptype", "backurl" };
                pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                if (pageIndex == 0)
                    pageIndex = 1;
                builder.Append(Out.Tab("<div>", ""));
                if (startstate == CID1 && endstate == CID2)
                {
                    builder.Append("总排名为：");
                }
                else
                {
                    builder.Append("第" + startstate + "期至第" + endstate + "期排名为：");
                }
                builder.Append(Out.Tab("</div>", "<br/>"));
                DataSet rowbang = new BCW.HP3.BLL.HP3WinnerSY().GetListBang2(startstate.ToString(), endstate.ToString());
                recordCount = rowbang.Tables[0].Rows.Count;
                DataSet bang = new BCW.HP3.BLL.HP3WinnerSY().GetListByPage2(0, recordCount, startstate.ToString(), endstate.ToString());
                if (recordCount >= 0)
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
                            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                        else
                        {
                            if (k == 1)
                                builder.Append(Out.Tab("<div>", ""));
                            else
                                builder.Append(Out.Tab("<div>", "<br />"));
                        }
                        string mename = new BCW.BLL.User().GetUsName(usid);
                        int wd = (pageIndex - 1) * 10 + k;
                        builder.Append("[<b style=\"color:red\">TOP" + wd + "</b>].<a  href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + mename + "(" + usid + ")</a>兑奖" + "<b style=\"color:red\">" + usmoney + "</b>" + "快乐币" + "");
                        k++;
                        builder.Append(Out.Tab("</div>", ""));
                        rewardid = rewardid + usid.ToString() + " ";
                    }
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                }
                else
                {
                    builder.Append(Out.Div("div", "没有相关记录.."));
                }
            }
            else if (ptype == 3)
            {
                int recordCount;
                string strWhere = string.Empty;
                int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
                string[] pageValUrl = { "act", "startstate", "endstate", "ptype", "backurl" };
                pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                if (pageIndex == 0)
                    pageIndex = 1;
                builder.Append(Out.Tab("<div>", ""));
                if (startstate == CID1 && endstate == CID2)
                {
                    builder.Append("总排名为：");
                }
                else
                {
                    builder.Append("第" + startstate + "期至第" + endstate + "期排名为：");
                }
                builder.Append(Out.Tab("</div>", "<br/>"));
                DataSet rowbang = new BCW.HP3.BLL.HP3BuySY().GetBang(startstate.ToString(), endstate.ToString());
                recordCount = rowbang.Tables[0].Rows.Count;
                DataSet bang = new BCW.HP3.BLL.HP3BuySY().GetBangByPage(0, recordCount, startstate.ToString(), endstate.ToString());
                if (recordCount >= 0)
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
                            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                        else
                        {
                            if (k == 1)
                                builder.Append(Out.Tab("<div>", ""));
                            else
                                builder.Append(Out.Tab("<div>", "<br />"));
                        }
                        string mename = new BCW.BLL.User().GetUsName(usid);
                        int wd = (pageIndex - 1) * 10 + k;
                        builder.Append("[<b style=\"color:red\">TOP" + wd + "</b>].<a  href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + mename + "(" + usid + ")</a>净赚" + "<b style=\"color:red\">" + usmoney + "</b>" + "快乐币" + "");
                        k++;
                        builder.Append(Out.Tab("</div>", ""));
                        rewardid = rewardid + usid.ToString() + " ";
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

                int recordCount;
                string strWhere = string.Empty;
                int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
                string[] pageValUrl = { "act", "startstate", "endstate", "ptype", "backurl" };
                pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                if (pageIndex == 0)
                    pageIndex = 1;
                builder.Append(Out.Tab("<div>", ""));
                if (startstate == CID1 && endstate == CID2)
                {
                    builder.Append("总排名为：");
                }
                else
                {
                    builder.Append("第" + startstate + "期至第" + endstate + "期排名为：");
                }
                builder.Append(Out.Tab("</div>", "<br />"));
                DataSet rowbang = new BCW.HP3.BLL.HP3BuySY().GetListBang2(startstate.ToString(), endstate.ToString());
                recordCount = rowbang.Tables[0].Rows.Count;
                DataSet bang = new BCW.HP3.BLL.HP3BuySY().GetListByPage(0, recordCount, startstate.ToString(), endstate.ToString());
                if (recordCount >= 0)
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
                            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                        else
                        {
                            if (k == 1)
                                builder.Append(Out.Tab("<div>", ""));
                            else
                                builder.Append(Out.Tab("<div>", "<br />"));
                        }
                        string mename = new BCW.BLL.User().GetUsName(usid);
                        int wd = (pageIndex - 1) * 10 + k;
                        builder.Append("[<b style=\"color:red\">TOP" + wd + "</b>].<a  href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + mename + "(" + usid + ")</a>购奖" + "<b style=\"color:red\">" + usmoney + "</b>" + "快乐币" + "");
                        k++;
                        builder.Append(Out.Tab("</div>", ""));
                        rewardid = rewardid + usid.ToString() + " ";
                    }
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                }
                else
                {
                    builder.Append(Out.Div("div", "没有相关记录.."));
                }

            }

            string strText = "输入开始期号:/,输入截止期号:/,";
            string strName = "startstate,endstate,backurl";
            string strType = "num,num,hidden";
            string strValu = "'" + "'" + Utils.getPage(0) + "";
            string strEmpt = "true,true,false";
            string strIdea = "/";
            string strOthe = "搜一搜,HP3.aspx?act=bang&amp;ptype=" + ptype + ",post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            string wdy = "";
            if (pageIndex == 1)
                wdy = "TOP10";
            else
                wdy = "TOP" + (pageIndex - 1).ToString() + "1-" + pageIndex.ToString() + "0";
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append(wdy + " ID:" + rewardid);
            builder.Append(Out.Tab("</div>", ""));
            string strText2 = ",,,,";
            string strName2 = "startstate,endstate,pageIndex,rewardid,backurl";
            string strType2 = "hidden,hidden,hidden,hidden,hidden";
            string strValu2 = startstate + "'" + endstate + "'" + pageIndex + "'" + rewardid + "'" + Utils.getPage(0) + "";
            string strEmpt2 = "true,true,false";
            string strIdea2 = "/";
            string strOthe2 = wdy + "奖励发放,HP3.aspx?act=ReWard&amp;ptype=" + ptype + ",post,1,red";
            builder.Append(Out.wapform(strText2, strName2, strType2, strValu2, strEmpt2, strIdea2, strOthe2));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("HP3.aspx") + "\">返回上一级</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
            #endregion
        }
    }
    //返赢返负
    private void Back()
    {
        Master.Title = "返赢返负";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>&gt;");
        builder.Append("返赢返负");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("返赢点操作");
        builder.Append(Out.Tab("</div>", ""));
        string strText = "开始时间:,结束时间:,返赢千分比:,至少赢多少才返:,消息通知,";
        string strName = "sTime,oTime,iTar,iPrice,tz,act";
        string strType = "date,date,num,num,text,hidden";
        string strValu = "" + DT.FormatDate(DateTime.Now.AddDays(-10), 0) + "'" + DT.FormatDate(DateTime.Now, 0) + "''''backsave";
        string strEmpt = "false,false,false,false,false,false";
        string strIdea = "/";
        string strOthe = "马上返赢,HP3.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("返负点操作");
        builder.Append(Out.Tab("</div>", ""));
        strText = "开始时间:,结束时间:,返负千分比:,至少负多少才返:,消息通知,";
        strName = "sTime,oTime,iTar,iPrice,tz,act";
        strType = "date,date,num,num,text,hidden";
        strValu = "" + DT.FormatDate(DateTime.Now.AddDays(-10), 0) + "'" + DT.FormatDate(DateTime.Now, 0) + "''''backsave2";
        strEmpt = "false,false,false,false,false,false";
        strIdea = "/";
        strOthe = "马上返负,HP3.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));


        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("HP3.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    private void BackSavePage(string act)
    {

        DateTime sTime = Utils.ParseTime(Utils.GetRequest("sTime", "all", 2, DT.RegexTime, "开始时间填写无效"));
        DateTime oTime = Utils.ParseTime(Utils.GetRequest("oTime", "all", 2, DT.RegexTime, "结束时间填写无效"));
        int iTar = Utils.ParseInt(Utils.GetRequest("iTar", "all", 2, @"^[0-9]\d*$", "千分比填写错误"));
        int iPrice = Utils.ParseInt(Utils.GetRequest("iPrice", "all", 2, @"^[0-9]\d*$", "至少多少币才返填写错误"));
        string texts = Utils.GetRequest("tz", "post", 1, "", "");
        string ac = Utils.GetRequest("ac", "post", 2, "", "");
        if (Utils.ToSChinese(ac) == "立即返还")
        {
            #region 立即返还
            if (act == "backsave")
            {
                DataSet ds = new BCW.HP3.BLL.HP3Buy().GetList("BuyID,sum(WillGet-BuyMoney*BuyZhu) as WinCents", "BuyTime>='" + sTime + "'and BuyTime<='" + oTime + "' group by BuyID");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    long Cents = Convert.ToInt64(ds.Tables[0].Rows[i]["WinCents"]);
                    if (Cents > 0 && Cents >= iPrice)
                    {
                        int usid = Convert.ToInt32(ds.Tables[0].Rows[i]["BuyID"]);
                        long cent = Convert.ToInt64(Cents * (iTar * 0.001));
                        new BCW.BLL.User().UpdateiGold(usid, cent, "" + GameName + "返赢");
                        //发内线
                        string strLog = texts + "，返还了" + cent + "" + ub.Get("SiteBz") + "[url=/bbs/game/HP3.aspx]进入" + GameName + "[/url]";
                        new BCW.BLL.Guest().Add(0, usid, new BCW.BLL.User().GetUsName(usid), strLog);
                    }
                }

                long sum = 0; int sumi = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    long Cents = Convert.ToInt64(ds.Tables[0].Rows[i]["WinCents"]);
                    long cent = 0;
                    if (Cents > 0 && Cents >= iPrice)
                    {
                        int usid = Convert.ToInt32(ds.Tables[0].Rows[i]["BuyID"]);
                        cent = Convert.ToInt64(Cents * (iTar * 0.001));
                        sumi++;
                    }
                    sum += cent;
                }
                new BCW.BLL.User().UpdateiGold(107, new BCW.BLL.User().GetUsName(107), 0, "" + GameName + sTime + "-" + oTime + "返赢|千分比" + iTar + "|至少赢" + iPrice + "|共计人次" + sumi + "|共返" + sum + "|此记录为返赢记录,不操作107的币数");

                Utils.Success("返赢操作", "返赢操作成功", Utils.getUrl("HP3.aspx"), "1");
            }
            else
            {
                DataSet ds = new BCW.HP3.BLL.HP3Buy().GetList("BuyID,sum(WillGet-BuyMoney*BuyZhu) as WinCents", "BuyTime>='" + sTime + "'and BuyTime<='" + oTime + "' group by BuyID");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    long Cents = Convert.ToInt64(ds.Tables[0].Rows[i]["WinCents"]);
                    if (Cents < 0 && Cents <= -iPrice)
                    {
                        int usid = Convert.ToInt32(ds.Tables[0].Rows[i]["BuyID"]);
                        long cent = Convert.ToInt64((-Cents) * (iTar * 0.001));
                        new BCW.BLL.User().UpdateiGold(usid, cent, "" + GameName + "返负");
                        //发内线
                        string strLog = texts + "，返还了" + cent + "" + ub.Get("SiteBz") + "[url=/bbs/game/HP3.aspx]进入" + GameName + "[/url]";
                        new BCW.BLL.Guest().Add(0, usid, new BCW.BLL.User().GetUsName(usid), strLog);
                    }
                }

                long sum = 0; int sumi = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    long Cents = Convert.ToInt64(ds.Tables[0].Rows[i]["WinCents"]);
                    long cent = 0;
                    if (Cents < 0 && Cents <= (-iPrice))
                    {
                        int usid = Convert.ToInt32(ds.Tables[0].Rows[i]["BuyID"]);
                        cent = Convert.ToInt64((-Cents) * (iTar * 0.001));
                        sumi++;
                    }
                    sum += cent;
                }
                new BCW.BLL.User().UpdateiGold(107, new BCW.BLL.User().GetUsName(107), 0, "" + GameName + sTime + "-" + oTime + "返负|千分比" + iTar + "|至少负" + iPrice + "|共计人次" + sumi + "|共返" + sum + "|此记录为返负记录,不操作107的币数");

                Utils.Success("返负操作", "返负操作成功", Utils.getUrl("HP3.aspx"), "1");
            }
            #endregion
        }
        else
        {
            string cz = "";
            if (act == "backsave")
            {
                cz = "返赢";
            }
            else
            {
                cz = "返负";
            }
            Master.Title = cz + "操作";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append(cz + "操作");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(cz + "时间段：" + sTime + "到" + oTime + "<br/>");
            builder.Append(cz + "千分比：" + iTar + "<br/>");
            builder.Append("至少多少币：" + iPrice + "<br/>");
            builder.Append("消息通知：" + texts + "<br />");
            if (act == "backsave")
            {
                DataSet ds = new BCW.HP3.BLL.HP3Buy().GetList("BuyID,sum(WillGet-BuyMoney*BuyZhu) as WinCents", "BuyTime>='" + sTime + "'and BuyTime<='" + oTime + "' group by BuyID");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    long sum = 0; int sumi = 0;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        long Cents = Convert.ToInt64(ds.Tables[0].Rows[i]["WinCents"]);
                        long cent = 0;
                        if (Cents > 0 && Cents >= iPrice)
                        {
                            int usid = Convert.ToInt32(ds.Tables[0].Rows[i]["BuyID"]);
                            cent = Convert.ToInt64(Cents * (iTar * 0.001));
                            sumi++;
                        }
                        sum += cent;
                    }
                    builder.Append("本次返赢人次：" + sumi + "<br />");
                    builder.Append("本次返赢金额：" + sum + "");
                }
            }
            else
            {
                DataSet ds = new BCW.HP3.BLL.HP3Buy().GetList("BuyID,sum(WillGet-BuyMoney*BuyZhu) as WinCents", "BuyTime>='" + sTime + "'and BuyTime<='" + oTime + "' group by BuyID");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    long sum = 0; int sumi = 0;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        long Cents = Convert.ToInt64(ds.Tables[0].Rows[i]["WinCents"]);
                        long cent = 0;
                        if (Cents < 0 && Cents <= (-iPrice))
                        {
                            int usid = Convert.ToInt32(ds.Tables[0].Rows[i]["BuyID"]);
                            cent = Convert.ToInt64((-Cents) * (iTar * 0.001));
                            sumi++;
                        }
                        sum += cent;
                    }
                    builder.Append("本次返负人次：" + sumi + "<br />");
                    builder.Append("本次返负金额：" + sum + "");
                }
            }
            builder.Append(Out.Tab("</div>", "<br />"));
            string strText = ",,,,,";
            string strName = "sTime,oTime,iTar,iPrice,tz,act";
            string strType = "hidden,hidden,hidden,hidden,hidden,hidden";
            string strValu = DT.FormatDate(sTime, 0) + "'" + DT.FormatDate(oTime, 0) + "'" + iTar + "'" + iPrice + "'" + texts + "'" + act;
            string strEmpt = "false,false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "立即返还,HP3.aspx,post,1,red";
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getPage("HP3.aspx?act=Back") + "\">返回上级</a><br />");
            builder.Append(Out.Tab("</div>", "<br />"));

        }


    }
    //机器人ID设置
    private void ReBot()
    {
        Master.Title = "机器人设置";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>&gt;");
        builder.Append("机器人设置");
        builder.Append(Out.Tab("</div>", "<br />"));

        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        string RoBotID = Utils.GetRequest("RoBotID", "post", 1, "", xml.dss["HP3ROBOTID"].ToString());
        string IsBot = Utils.GetRequest("IsBot", "post", 1, @"^[0-1]$", xml.dss["HP3IsBot"].ToString());
        string RoBotCost = Utils.GetRequest("RoBotCost", "post", 1, "", xml.dss["HP3ROBOTCOST"].ToString());
        string RoBotCount = Utils.GetRequest("RoBotCount", "post", 1, @"^[0-9]\d*$", xml.dss["HP3ROBOTBUY"].ToString());
        xml.dss["HP3ROBOTID"] = RoBotID;
        xml.dss["HP3IsBot"] = IsBot;
        xml.dss["HP3ROBOTCOST"] = RoBotCost;
        xml.dss["HP3ROBOTBUY"] = RoBotCount;
        System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
        string strText = "机器人ID:/,机器人状态:/,机器人单注投注倍数:/,机器人每期购买订单数:/";
        string strName = "RoBotID,IsBot,RoBotCost,RoBotCount";
        string strType = "text,select,text,text";
        string strValu = xml.dss["HP3ROBOTID"].ToString() + "'" + xml.dss["HP3IsBot"].ToString() + "'" + xml.dss["HP3ROBOTCOST"].ToString() + "'" + xml.dss["HP3ROBOTBUY"].ToString();
        string strEmpt = "true,0|关闭|1|开启,true,false";
        string strIdea = "/";
        string strOthe = "确定修改,HP3.aspx?act=ReBot&amp; ,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("温馨提示:多个机器人ID请用#分隔。<br />");

        string HP3ROBOTID = Convert.ToString(ub.GetSub("HP3ROBOTID", xmlPath));
        string[] name1 = HP3ROBOTID.Split('#');
        string name2 = string.Empty;
        for (int n = 0; n < name1.Length; n++)
        {
            if ((n + 1) % 5 == 0)
                name2 = name2 + name1[n] + "," + "<br />";
            else
                name2 = name2 + name1[n] + ",";
        }
        builder.Append("当前机器人ID为：<br /><b style=\"color:red\">" + name2 + "</b><br />");

        if (xml.dss["HP3IsBot"].ToString() == "0")
        {
            builder.Append("机器人状态：<b style=\"color:red\">关闭</b><br />");
        }
        else
        {
            builder.Append("当前机器人状态：<b style=\"color:red\">开启</b><br />");
        }
        builder.Append("当前机器人单注投注金额：<b style=\"color:red\">" + xml.dss["HP3ROBOTCOST"].ToString() + "</b><br />");
        builder.Append("当前机器人每期购买彩票数：<b style=\"color:red\">" + xml.dss["HP3ROBOTBUY"].ToString() + "</b>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        //builder.Append("<a target=\"_blank\" href=\"" + Utils.getPage("../../Robot/HP3RoBot.aspx") + "\">机器人刷新页面</a><br />");
        builder.Append("<a href=\"" + Utils.getPage("HP3.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    //排行榜奖励发放：
    private void ReWard()
    {
        if (SWB == 0)
        {
            #region 正式版
            Master.Title = "奖励发放";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
            builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>&gt;");
            builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=bang") + "\">用户排行</a>&gt;");
            builder.Append("奖励发放");
            builder.Append(Out.Tab("</div>", "<br />"));
            int CID1 = new BCW.HP3.BLL.HP3_kjnum().GetXXCID("datenum>0 order by datenum asc");
            int CID2 = new BCW.HP3.BLL.HP3_kjnum().GetXXCID("datenum>0 order by datenum desc");
            int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));
            int startstate = int.Parse(Utils.GetRequest("startstate", "all", 1, @"^[1-9]\d*$", "" + CID1 + ""));
            int endstate = int.Parse(Utils.GetRequest("endstate", "all", 1, @"^[1-9]\d*$", "" + CID2 + ""));
            int pageIndex = int.Parse(Utils.GetRequest("pageIndex", "all", 1, @"^[1-9]\d*$", "1"));
            string rewardid = Utils.GetRequest("rewardid", "post", 2, @"^[\s\S]{1,500}$", "当前页无可操作用户");
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("第" + startstate + "期至第" + endstate + "期排名为：");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            switch (ptype)
            {
                case 1:
                    builder.Append("获奖排行");
                    break;
                case 2:
                    builder.Append("<b>购奖排行</b><br />");
                    break;
                case 3:
                    builder.Append("<b>净赚排行</b><br />");
                    break;
            }
            string wdy = "";
            if (pageIndex == 1)
                wdy = "TOP10";
            else
                wdy = "TOP" + (pageIndex - 1).ToString() + "1-" + pageIndex.ToString() + "0";
            builder.Append("<b>" + wdy + "</b>奖励发放<br />");
            builder.Append(Out.Tab("</div>", ""));
            int mzj = (pageIndex - 1) * 10;
            string[] IdRe = rewardid.TrimEnd().Split(' ');
            try
            {
                string strText2 = ",,,,TOP" + (mzj + 1) + ":" + IdRe[0] + ",,TOP" + (mzj + 2) + ":" + IdRe[1] + ",,TOP" + (mzj + 3) + ":" + IdRe[2] + ",,TOP" + (mzj + 4) + ":" + IdRe[3] + ",,TOP" + (mzj + 5) + ":" + IdRe[4] + ",,TOP" + (mzj + 6) + ":" + IdRe[5] + ",,TOP" + (mzj + 7) + ":" + IdRe[6] + ",,TOP" + (mzj + 8) + ":" + IdRe[7] + ",,TOP" + (mzj + 9) + ":" + IdRe[8] + ",,TOP" + pageIndex * 10 + ":" + IdRe[9] + ",";
                string strName2 = "act,ptype,startstate,endstate,top1,IdRe1,top2,IdRe2,top3,IdRe3,top4,IdRe4,top5,IdRe5,top6,IdRe6,top7,IdRe7,top8,IdRe8,top9,IdRe9,top10,IdRe10";
                string strType2 = "hidden,hidden,hidden,hidden,num,hidden,num,hidden,num,hidden,num,hidden,num,hidden,num,hidden,num,hidden,num,hidden,num,hidden,num,hidden";
                string strValu2 = "ReWardCase'" + ptype + "'" + startstate + "'" + endstate + "'" + "0'" + IdRe[0] + "'0'" + IdRe[1] + "'0'" + IdRe[2] + "'0'" + IdRe[3] + "'0'" + IdRe[4] + "'0'" + IdRe[5] + "'0'" + IdRe[6] + "'0'" + IdRe[7] + "'0'" + IdRe[8] + "'0'" + IdRe[9] + "'0";
                string strEmpt2 = "false,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true";
                string strIdea2 = "/";
                string strOthe2 = "提交,HP3.aspx,post,1,red";
                builder.Append(Out.wapform(strText2, strName2, strType2, strValu2, strEmpt2, strIdea2, strOthe2));
            }
            catch
            {
                builder.Append("当页少于10人，无法发放！");
            }
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("HP3.aspx?act=bang") + "\">返回上一级</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
            #endregion
        }
        else
        {
            #region
            Master.Title = "奖励发放";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
            builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>&gt;");
            builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx?act=bang") + "\">用户排行</a>&gt;");
            builder.Append("奖励发放");
            builder.Append(Out.Tab("</div>", "<br />"));
            int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));
            int CID1 = new BCW.HP3.BLL.HP3_kjnum().GetXXCID("datenum>0 order by datenum asc");
            int CID2 = new BCW.HP3.BLL.HP3_kjnum().GetXXCID("datenum>0 order by datenum desc");
            int startstate = int.Parse(Utils.GetRequest("startstate", "all", 1, @"^[1-9]\d*$", "" + CID1 + ""));
            int endstate = int.Parse(Utils.GetRequest("endstate", "all", 1, @"^[1-9]\d*$", "" + CID2 + ""));
            int pageIndex = int.Parse(Utils.GetRequest("pageIndex", "all", 1, @"^[1-9]\d*$", "1"));
            string rewardid = Utils.GetRequest("rewardid", "post", 2, @"^[\s\S]{1,500}$", "当前页无可操作用户");
            builder.Append("第" + startstate + "期到第" + endstate + "期");
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            switch (ptype)
            {
                case 1:
                    builder.Append("获奖排行");
                    break;
                case 2:
                    builder.Append("<b>购奖排行</b><br />");
                    break;
                case 3:
                    builder.Append("<b>净赚排行</b><br />");
                    break;
            }
            string wdy = "";
            if (pageIndex == 1)
                wdy = "TOP10";
            else
                wdy = "TOP" + (pageIndex - 1).ToString() + "1-" + pageIndex.ToString() + "0";
            builder.Append("<b>" + wdy + "</b>奖励发放<br />");
            int mzj = (pageIndex - 1) * 10;
            string[] IdRe = rewardid.TrimEnd().Split(' ');
            try
            {
                string strText2 = ",,,,TOP" + (mzj + 1) + ":" + IdRe[0] + ",,TOP" + (mzj + 2) + ":" + IdRe[1] + ",,TOP" + (mzj + 3) + ":" + IdRe[2] + ",,TOP" + (mzj + 4) + ":" + IdRe[3] + ",,TOP" + (mzj + 5) + ":" + IdRe[4] + ",,TOP" + (mzj + 6) + ":" + IdRe[5] + ",,TOP" + (mzj + 7) + ":" + IdRe[6] + ",,TOP" + (mzj + 8) + ":" + IdRe[7] + ",,TOP" + (mzj + 9) + ":" + IdRe[8] + ",,TOP" + pageIndex * 10 + ":" + IdRe[9] + ",";
                string strName2 = "act,ptype,startstate,endstate,top1,IdRe1,top2,IdRe2,top3,IdRe3,top4,IdRe4,top5,IdRe5,top6,IdRe6,top7,IdRe7,top8,IdRe8,top9,IdRe9,top10,IdRe10";
                string strType2 = "hidden,hidden,hidden,hidden,num,hidden,num,hidden,num,hidden,num,hidden,num,hidden,num,hidden,num,hidden,num,hidden,num,hidden,num,hidden";
                string strValu2 = "ReWardCase'" + ptype + "'" + startstate + "'" + endstate + "'" + "0'" + IdRe[0] + "'0'" + IdRe[1] + "'0'" + IdRe[2] + "'0'" + IdRe[3] + "'0'" + IdRe[4] + "'0'" + IdRe[5] + "'0'" + IdRe[6] + "'0'" + IdRe[7] + "'0'" + IdRe[8] + "'0'" + IdRe[9] + "'0";
                string strEmpt2 = "false,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true";
                string strIdea2 = "/";
                string strOthe2 = "提交,HP3.aspx,post,1,red";
                builder.Append(Out.wapform(strText2, strName2, strType2, strValu2, strEmpt2, strIdea2, strOthe2));
            }
            catch
            {
                builder.Append("当页少于10人，无法发放！");
            }
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("HP3.aspx?act=bang") + "\">返回上一级</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
            #endregion
        }
    }
    private void ReWardCase()
    {
        if (SWB == 0)
        {
            #region 正式版
            int[] IdRe = new int[11];
            long[] Top = new long[11];
            IdRe[1] = int.Parse(Utils.GetRequest("IdRe1", "post", 1, "", ""));
            IdRe[2] = int.Parse(Utils.GetRequest("IdRe2", "post", 1, "", ""));
            IdRe[3] = int.Parse(Utils.GetRequest("IdRe3", "post", 1, "", ""));
            IdRe[4] = int.Parse(Utils.GetRequest("IdRe4", "post", 1, "", ""));
            IdRe[5] = int.Parse(Utils.GetRequest("IdRe5", "post", 1, "", ""));
            IdRe[6] = int.Parse(Utils.GetRequest("IdRe6", "post", 1, "", ""));
            IdRe[7] = int.Parse(Utils.GetRequest("IdRe7", "post", 1, "", ""));
            IdRe[8] = int.Parse(Utils.GetRequest("IdRe8", "post", 1, "", ""));
            IdRe[9] = int.Parse(Utils.GetRequest("IdRe9", "post", 1, "", ""));
            IdRe[10] = int.Parse(Utils.GetRequest("IdRe10", "post", 1, "", ""));
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
            int startstate = int.Parse(Utils.GetRequest("startstate", "all", 1, @"^[1-9]\d*$", ""));
            int endstate = int.Parse(Utils.GetRequest("endstate", "all", 1, @"^[1-9]\d*$", ""));
            string wdy = "";
            string ac = Utils.GetRequest("ac", "all", 1, "", "");
            switch (ptype)
            {
                case 1:
                    wdy = "幸运富豪榜";
                    break;
                case 2:
                    wdy = "快乐土豪榜";
                    break;
                case 3:
                    wdy = "净赚达人榜";
                    break;
            }
            if (Utils.ToSChinese(ac) == "确定发放")
            {
                for (int i = 1; i <= 10; i++)
                {
                    if (Top[i] != 0)
                    {
                        new BCW.BLL.User().UpdateiGold(IdRe[i], Top[i], "" + GameName + "排行榜奖励");
                        //发内线
                        string strLog = "你在第" + startstate + "期到第" + endstate + "期的" + GameName + "" + wdy + "上取得了第" + i + "名的好成绩，系统奖励了" + Top[i] + "" + ub.Get("SiteBz") + "[url=/bbs/game/HP3.aspx]进入" + GameName + "[/url]";
                        new BCW.BLL.Guest().Add(0, IdRe[i], new BCW.BLL.User().GetUsName(IdRe[i]), strLog);
                        //动态
                        string mename = new BCW.BLL.User().GetUsName(IdRe[i]);
                        string wText = "在[url=/bbs/game/HP3.aspx]" + GameName + "[/url]" + wdy + "上取得了第" + i + "名的好成绩，系统奖励了" + Top[i] + "" + ub.Get("SiteBz");
                        new BCW.BLL.Action().Add(2501, 0, IdRe[i], mename, wText);
                    }
                }
                Utils.Success("奖励操作", "奖励操作成功", Utils.getUrl("HP3.aspx"), "1");
            }
            else
            {
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("正在发放" + wdy + "奖励");
                builder.Append(Out.Tab("</div>", "<br />"));

                builder.Append(Out.Tab("<div>", ""));
                builder.Append("第" + startstate + "期到第" + endstate + "期<br/>");
                for (int j = 1; j <= 10; j++)
                {
                    builder.Append("TOP" + j + ":" + IdRe[j] + "(" + Top[j] + "酷币)<br/>");
                }

                string strText2 = ",,,,,,,,,,,,,,,,,,,,,,,";
                string strName2 = "act,ptype,startstate,endstate,top1,IdRe1,top2,IdRe2,top3,IdRe3,top4,IdRe4,top5,IdRe5,top6,IdRe6,top7,IdRe7,top8,IdRe8,top9,IdRe9,top10,IdRe10";
                string strType2 = "hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden";
                string strValu2 = "ReWardCase'" + ptype + "'" + startstate + "'" + endstate + "'" + Top[1] + "'" + IdRe[1] + "'" + Top[2] + "'" + IdRe[2] + "'" + Top[3] + "'" + IdRe[3] + "'" + Top[4] + "'" + IdRe[4] + "'" + Top[5] + "'" + IdRe[5] + "'" + Top[6] + "'" + IdRe[6] + "'" + Top[7] + "'" + IdRe[7] + "'" + Top[8] + "'" + IdRe[8] + "'" + Top[9] + "'" + IdRe[9] + "'" + Top[10] + "'" + IdRe[10];
                string strEmpt2 = "false,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true";
                string strIdea2 = "/";
                string strOthe2 = "确定发放,HP3.aspx,post,1,red";
                builder.Append(Out.wapform(strText2, strName2, strType2, strValu2, strEmpt2, strIdea2, strOthe2));
                builder.Append("<br /><a href=\"" + Utils.getPage("HP3.aspx?act=bang") + "\">再看看吧</a>");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            #endregion
        }
        else
        {
            #region
            int[] IdRe = new int[11];
            long[] Top = new long[11];
            IdRe[1] = int.Parse(Utils.GetRequest("IdRe1", "post", 1, "", ""));
            IdRe[2] = int.Parse(Utils.GetRequest("IdRe2", "post", 1, "", ""));
            IdRe[3] = int.Parse(Utils.GetRequest("IdRe3", "post", 1, "", ""));
            IdRe[4] = int.Parse(Utils.GetRequest("IdRe4", "post", 1, "", ""));
            IdRe[5] = int.Parse(Utils.GetRequest("IdRe5", "post", 1, "", ""));
            IdRe[6] = int.Parse(Utils.GetRequest("IdRe6", "post", 1, "", ""));
            IdRe[7] = int.Parse(Utils.GetRequest("IdRe7", "post", 1, "", ""));
            IdRe[8] = int.Parse(Utils.GetRequest("IdRe8", "post", 1, "", ""));
            IdRe[9] = int.Parse(Utils.GetRequest("IdRe9", "post", 1, "", ""));
            IdRe[10] = int.Parse(Utils.GetRequest("IdRe10", "post", 1, "", ""));
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
            int startstate = int.Parse(Utils.GetRequest("startstate", "all", 1, @"^[1-9]\d*$", ""));
            int endstate = int.Parse(Utils.GetRequest("endstate", "all", 1, @"^[1-9]\d*$", ""));
            string ac = Utils.GetRequest("ac", "all", 1, "", "");
            string wdy = "";
            switch (ptype)
            {
                case 1:
                    wdy = "幸运富豪榜";
                    break;
                case 2:
                    wdy = "快乐土豪榜";
                    break;
                case 3:
                    wdy = "净赚达人榜";
                    break;
            }
            if (Utils.ToSChinese(ac) == "确定发放")
            {
                for (int i = 1; i <= 10; i++)
                {
                    if (Top[i] != 0)
                    {
                        new BCW.BLL.User().UpdateiGold(IdRe[i], Top[i], "" + GameName + "排行榜奖励");
                        //发内线
                        string strLog = "你在第" + startstate + "期到第" + endstate + "期的" + GameName + "" + wdy + "上取得了第" + i + "名的好成绩，系统奖励了" + Top[i] + "" + "快乐币" + "[url=/bbs/game/HP3SW.aspx]进入" + GameName + "[/url]";
                        new BCW.BLL.Guest().Add(0, IdRe[i], new BCW.BLL.User().GetUsName(IdRe[i]), strLog);
                        //动态
                        string mename = new BCW.BLL.User().GetUsName(IdRe[i]);
                        string wText = "在[url=/bbs/game/HP3SW.aspx]" + GameName + "[/url]" + wdy + "上取得了第" + i + "名的好成绩，系统奖励了" + Top[i] + "" + "快乐币";
                        new BCW.BLL.Action().Add(2501, 0, IdRe[i], mename, wText);
                    }
                }
                Utils.Success("奖励操作", "奖励操作成功", Utils.getUrl("HP3.aspx"), "1");
            }
            else
            {
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("正在发放" + wdy + "奖励");
                builder.Append(Out.Tab("</div>", "<br />"));

                builder.Append(Out.Tab("<div>", ""));
                builder.Append("第" + startstate + "期到第" + endstate + "期<br/>");
                for (int j = 1; j <= 10; j++)
                {
                    builder.Append("TOP" + j + ":" + IdRe[j] + "(" + Top[j] + "酷币)<br/>");
                }

                string strText2 = ",,,,,,,,,,,,,,,,,,,,,,,";
                string strName2 = "act,ptype,startstate,endstate,top1,IdRe1,top2,IdRe2,top3,IdRe3,top4,IdRe4,top5,IdRe5,top6,IdRe6,top7,IdRe7,top8,IdRe8,top9,IdRe9,top10,IdRe10";
                string strType2 = "hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden";
                string strValu2 = "ReWardCase'" + ptype + "'" + startstate + "'" + endstate + "'" + Top[1] + "'" + IdRe[1] + "'" + Top[2] + "'" + IdRe[2] + "'" + Top[3] + "'" + IdRe[3] + "'" + Top[4] + "'" + IdRe[4] + "'" + Top[5] + "'" + IdRe[5] + "'" + Top[6] + "'" + IdRe[6] + "'" + Top[7] + "'" + IdRe[7] + "'" + Top[8] + "'" + IdRe[8] + "'" + Top[9] + "'" + IdRe[9] + "'" + Top[10] + "'" + IdRe[10];
                string strEmpt2 = "false,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true";
                string strIdea2 = "/";
                string strOthe2 = "确定发放,HP3.aspx,post,1,red";
                builder.Append(Out.wapform(strText2, strName2, strType2, strValu2, strEmpt2, strIdea2, strOthe2));
                builder.Append("<a href=\"" + Utils.getPage("HP3.aspx?act=bang") + "\">再看看吧</a>");
                builder.Append(Out.Tab("</div>", ""));

            }
            #endregion
        }
    }
    //手动刷期号
    private void MeAddDate()
    {
        BCW.HP3.Model.HP3_kjnum kjget = new BCW.HP3.BLL.HP3_kjnum().GetListLast();
        BCW.HP3.Model.HP3_kjnum kjput = new BCW.HP3.Model.HP3_kjnum();
        try
        {
            kjput.datenum = (Convert.ToInt64(kjget.datenum) + 1).ToString();
            kjput.datetime = DateTime.Now.AddMinutes(10);
            kjput.Fnum = "null";
            kjput.Snum = "null";
            kjput.Tnum = "null";
            kjput.Winum = "null";
            new BCW.HP3.BLL.HP3_kjnum().Add(kjput);
            Response.Write("添加" + kjput.datenum + "期成功！");
        }
        catch
        {
            Response.Write("添加失败！");
        }

    }
    //试玩配置
    private void SWSZ()
    {
        Master.Title = "试玩配置";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>&gt;");
        builder.Append("试玩配置");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-1]$", "0"));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");

        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string HP3SWKQ = Utils.GetRequest("IsOpen", "post", 2, @"^[0,1]$", "");
            string GETMONEY = Utils.GetRequest("GetMoney", "post", 2, @"^[0-9]\d*$", "");
            string GETMONEYMAX = Utils.GetRequest("MaxMoney", "post", 2, @"^[0-9]\d*$", "");
            string GETMONEYCI = Utils.GetRequest("Ci", "post", 2, @"^[0-9]\d*$", "");
            xml.dss["HP3SWKQ"] = HP3SWKQ;
            xml.dss["GETMONEY"] = GETMONEY;
            xml.dss["GETMONEYMAX"] = GETMONEYMAX;
            xml.dss["GETMONEYCI"] = GETMONEYCI;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("" + GameName + "设置", "设置成功，正在返回..", Utils.getUrl("HP3.aspx?act=swsz&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            string strText = "试玩注册开启:/,每次可领取金额:/,最大快乐币获取:/,每隔几分钟可领一次:/,";
            string strName = "IsOpen,GetMoney,MaxMoney,Ci,backurl";
            string strType = "select,num,num,num,hidden";
            string strValu = xml.dss["HP3SWKQ"] + "'" + xml.dss["GETMONEY"] + "'" + xml.dss["GETMONEYMAX"] + "'" + xml.dss["GETMONEYCI"] + "'" + Utils.getPage(0) + "";
            string strEmpt = "0|开启|1|关闭,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定修改|reset,HP3.aspx?act=swsz,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示:<br />试玩注册开启后允许所有用户试玩。");
            builder.Append(Out.Tab("</div>", ""));
        }

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("HP3.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    ///<summary>
    ///未/已兑奖
    /// </summary>
    private void CasePage()
    {
        Master.Title = "" + GameName + "兑奖查看";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>");
        builder.Append("&gt;兑奖查看");
        builder.Append(Out.Tab("</div>", "<br />"));

        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-1]$", "0"));
        //用户ID
        int usid = int.Parse(Utils.GetRequest("usid", "all", 1, @"^[1-9]\d*$", "0"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("〓查兑奖〓");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        if (ptype == 0)
        {
            builder.Append("未兑奖 | <a href=\"" + Utils.getUrl("HP3.aspx?act=case&amp;ptype=1&amp;usid=" + usid + "") + "\">已兑奖</a>");
        }
        else
        {
            builder.Append(" <a href=\"" + Utils.getUrl("HP3.aspx?act=case&amp;ptype=0&amp;usid=" + usid + "") + "\">未兑奖</a> | 已兑奖");
        }
        builder.Append(Out.Tab("</div>", "<br />"));


        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        if (ptype == 0)
        {
            if (usid == 0)
                strWhere += " WinBool=1 and WinMoney >0 ";
            else
                strWhere += " WinBool=1 and WinUserID=" + usid + " and WinMoney >0 ";
        }
        else
        {
            if (usid == 0)
                strWhere += " (WinBool=2 or  WinBool=0)  ";
            else
                strWhere += " (WinBool=2 or  WinBool=0)  and WinUserID=" + usid + " ";
        }
        string[] pageValUrl = { "act", "ptype", "usid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        string SSCqi = "";
        // 开始读取列表
        IList<BCW.HP3.Model.HP3Winner> listSSCpay = new BCW.HP3.BLL.HP3Winner().GetListNes(pageIndex, pageSize, strWhere, out recordCount);
        if (listSSCpay.Count > 0)
        {
            int k = 1;
            foreach (BCW.HP3.Model.HP3Winner n in listSSCpay)
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
                if (n.WinDate.ToString() != SSCqi)
                {
                    BCW.HP3.Model.HP3_kjnum wins = new BCW.HP3.BLL.HP3_kjnum().GetDataByState(n.WinDate);
                    if (wins.Fnum == "")
                        builder.Append("=第" + n.WinDate + "期=<br />");
                    else
                        builder.Append("=第" + n.WinDate + "期=开出:<f style=\"color:red\">" + wins.Fnum + wins.Snum + wins.Tnum + "</f><br />");
                }
                BCW.HP3.Model.HP3Buy m = new BCW.HP3.BLL.HP3Buy().GetModel(n.ID);
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<b>" + ((pageIndex - 1) * pageSize + k) + ".</b>");
                if (m.BuyType >= 6 && m.BuyType <= 16)
                {
                    builder.Append("<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.WinUserID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(n.WinUserID) + "(" + n.WinUserID + ")" + "</a> <b>[" + NumToType(m.BuyType) + "]</b>位号:" + m.BuyNum + "/每注" + m.BuyMoney + "" + ub.Get("SiteBz") + "/共" + m.BuyZhu + "注/赔率" + m.Odds + "/标识ID:" + m.ID + "[" + DT.FormatDate(m.BuyTime, 1) + "]");
                }
                else if (m.BuyType == 17)
                {
                    string st = "null";
                    switch (m.BuyNum.Trim())
                    {
                        case "1":
                            st = "买大";
                            break;
                        case "2":
                            st = "买小";
                            break;
                        case "3":
                            st = "买单";
                            break;
                        case "4":
                            st = "买双";
                            break;

                    }
                    builder.Append("<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.WinUserID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(n.WinUserID) + "(" + n.WinUserID + ")" + "</a> <b>[" + st + "]</b>位号:" + speChoose(Convert.ToInt32(m.BuyNum)) + "/每注" + m.BuyMoney + "" + ub.Get("SiteBz") + "/共" + m.BuyZhu + "注/赔率" + m.Odds + "/标识ID:" + m.ID + "[" + DT.FormatDate(m.BuyTime, 1) + "]");

                }
                else
                {
                    string st = "null";
                    switch (m.BuyNum)
                    {
                        case "1":
                        case "2":
                        case "3":
                        case "4":
                        case "5":
                            st = "同花投注";
                            break;
                        case "6":
                        case "7":
                        case "8":
                        case "9":
                        case "10":
                        case "11":
                        case "12":
                        case "13":
                        case "14":
                        case "15":
                        case "16":
                        case "17":
                        case "18":
                            st = "顺子投注";
                            break;
                        case "19":
                        case "20":
                        case "21":
                        case "22":
                        case "23":
                            st = "同花顺投注";
                            break;
                        case "24":
                        case "25":
                        case "26":
                        case "27":
                        case "28":
                        case "29":
                        case "30":
                        case "31":
                        case "32":
                        case "33":
                        case "34":
                        case "35":
                        case "36":
                        case "37":
                            st = "豹子投注";
                            break;
                        case "38":
                        case "39":
                        case "40":
                        case "41":
                        case "42":
                        case "43":
                        case "44":
                        case "45":
                        case "46":
                        case "47":
                        case "48":
                        case "49":
                        case "50":
                        case "51":
                            st = "对子投注";
                            break;

                    }
                    builder.Append("<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.WinUserID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(n.WinUserID) + "(" + n.WinUserID + ")" + "</a> <b>[" + st + "]</b>位号:" + speChoose(Convert.ToInt32(m.BuyNum)) + "/每注" + m.BuyMoney + "" + ub.Get("SiteBz") + "/共" + m.BuyZhu + "注/赔率" + m.Odds + "/标识ID:" + m.ID + "[" + DT.FormatDate(m.BuyTime, 1) + "]");
                }
                if (n.WinMoney > 0)
                {
                    builder.Append("中" + n.WinZhu + "注/赢" + n.WinMoney + "" + ub.Get("SiteBz") + "");
                    if (ptype == 0)
                    {
                        builder.Append("|<f style=\"color:blue\"><a href=\"" + Utils.getUrl("HP3.aspx?act=caseok&amp;id=" + n.ID + "") + "\">帮他兑奖</a></f>");
                    }
                    else
                    {
                        builder.Append("|<f style=\"color:black\">已兑奖</f>");
                    }
                }
                builder.Append(Out.Tab("</div>", ""));
                SSCqi = n.WinDate.ToString();
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

        builder.Append(Out.Tab("<div class=\"hr\"></div>", "<br />-----------"));

        string strText = "用户ID查询:/,";
        string strName = "usid,act";
        string strType = "num,hidden";
        string strValu = "" + usid + "'" + Utils.getPage(0) + "";
        string strEmpt = "true,true,false";
        string strIdea = "/";
        string strOthe = "确认搜索,HP3.aspx?act=case&amp;ptype=" + ptype + ",post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("温馨提示：查询用户ID为0时，默认查询所有用户记录<br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    ///<summary>
    ///帮他兑奖
    /// </summary>
    private void CaseokPage()
    {
        Master.Title = "" + GameName + "帮他兑奖";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("HP3.aspx") + "\">" + GameName + "</a>");
        builder.Append("&gt;帮他兑奖");
        builder.Append(Out.Tab("</div>", "<br />"));

        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[1-9]\d*$", "0"));
        string info = Utils.GetRequest("info", "all", 1, "", "");
        BCW.HP3.Model.HP3Winner n = new BCW.HP3.BLL.HP3Winner().GetModel(id);

        if (info == "ok")
        {
            if (new BCW.HP3.BLL.HP3Winner().Exists3(id, n.WinUserID))
            {
                new BCW.HP3.BLL.HP3Winner().UpdateByID(id);

                BCW.User.Users.IsFresh("hp3", 1);//防刷
                BCW.HP3.Model.HP3_kjnum idd = new BCW.HP3.BLL.HP3_kjnum().GetDataByState(n.WinDate);
                new BCW.BLL.User().UpdateiGold(n.WinUserID, new BCW.BLL.User().GetUsName(n.WinUserID), n.WinMoney, "" + GameName + "兑奖-" + "[url=./game/HP3.aspx?act=BuyWin&qihaos=" + n.WinDate + "&amp;ptype=1]" + n.WinDate + "[/url]" + "-标识ID" + n.ID + "");
                if (new BCW.BLL.User().GetIsSpier(n.WinUserID) != 1)
                    new BCW.BLL.User().UpdateiGold(107, new BCW.BLL.User().GetUsName(107), -n.WinMoney, "ID:[url=forumlog.aspx?act=xview&amp;uid=" + n.WinUserID + "]" + n.WinUserID + "[/url]" + GameName + "第[url=./game/HP3.aspx?act=BuyWin&qihaos=" + n.WinDate + "&amp;ptype=1]" + n.WinDate + "[/url]期兑奖" + n.WinMoney + "|(标识ID" + n.ID + ")");

                new BCW.BLL.Guest().Add(1, n.WinUserID, BCW.User.Users.SetUser(n.WinUserID), "您在[URL=/bbs/game/HP3.aspx]" + GameName + "[/URL]第" + n.WinDate + "期的投注" + "系统已经帮您兑奖，获得了" + n.WinMoney + ub.Get("SiteBz") + "。标识ID" + n.ID + "");//开奖提示信息,1表示开奖信息
                Utils.Success("兑奖", "恭喜，成功帮他兑奖" + n.WinMoney + "" + ub.Get("SiteBz") + "", Utils.getUrl("HP3.aspx?act=case&amp;ptype=0"), "1");

            }
            else
            {
                Utils.Success("兑奖", "重复兑奖或没有可以兑奖的记录", Utils.getUrl("HP3.aspx?act=case&amp;ptype=0"), "1");
            }
        }
        else
        {
            BCW.HP3.Model.HP3_kjnum wins = new BCW.HP3.BLL.HP3_kjnum().GetDataByState(n.WinDate);
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("=第" + n.WinDate + "期=开出:<f style=\"color:red\">" + wins.Fnum + wins.Snum + wins.Tnum + "</f><br />");
            BCW.HP3.Model.HP3Buy m = new BCW.HP3.BLL.HP3Buy().GetModel(n.ID);
            builder.Append("用户：<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.WinUserID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(n.WinUserID) + " (" + n.WinUserID + ")" + "</a><br /> ");
            if (m.BuyType >= 6 && m.BuyType <= 16)
            {
                builder.Append("兑奖ID：" + id + "<br />投注方式：<b>[" + NumToType(m.BuyType) + "]</b>位号:" + m.BuyNum + "<br />每注：" + m.BuyMoney + "" + ub.Get("SiteBz") + "<br />共投：" + m.BuyZhu + "注<br />赔率：" + m.Odds + "<br />投注时间：" + DT.FormatDate(m.BuyTime, 1) + "<br />");
            }
            else if (m.BuyType == 17)
            {
                string st = "null";
                switch (m.BuyNum.Trim())
                {
                    case "1":
                        st = "买大";
                        break;
                    case "2":
                        st = "买小";
                        break;
                    case "3":
                        st = "买单";
                        break;
                    case "4":
                        st = "买双";
                        break;
                }
                builder.Append("兑奖ID：" + id + "<br />投注方式：<b>[" + NumToType(m.BuyType) + "]</b>位号:" + st + "<br />每注：" + m.BuyMoney + "" + ub.Get("SiteBz") + "<br />共投：" + m.BuyZhu + "注<br />赔率：" + m.Odds + "<br />投注时间：" + DT.FormatDate(m.BuyTime, 1) + "<br />");

            }
            else
            {
                string st = "null";
                switch (m.BuyNum)
                {
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                        st = "同花投注";
                        break;
                    case "6":
                    case "7":
                    case "8":
                    case "9":
                    case "10":
                    case "11":
                    case "12":
                    case "13":
                    case "14":
                    case "15":
                    case "16":
                    case "17":
                    case "18":
                        st = "顺子投注";
                        break;
                    case "19":
                    case "20":
                    case "21":
                    case "22":
                    case "23":
                        st = "同花顺投注";
                        break;
                    case "24":
                    case "25":
                    case "26":
                    case "27":
                    case "28":
                    case "29":
                    case "30":
                    case "31":
                    case "32":
                    case "33":
                    case "34":
                    case "35":
                    case "36":
                    case "37":
                        st = "豹子投注";
                        break;
                    case "38":
                    case "39":
                    case "40":
                    case "41":
                    case "42":
                    case "43":
                    case "44":
                    case "45":
                    case "46":
                    case "47":
                    case "48":
                    case "49":
                    case "50":
                    case "51":
                        st = "对子投注";
                        break;

                }
                builder.Append("兑奖ID：" + id + "<br />投注方式：<b>[" + st + "]</b>位号:" + speChoose(Convert.ToInt32(m.BuyNum)) + "<br />每注：" + m.BuyMoney + "" + ub.Get("SiteBz") + "<br />共投：" + m.BuyZhu + "注<br />赔率：" + m.Odds + "<br />投注时间：" + DT.FormatDate(m.BuyTime, 1) + "<br />");

            }

            if (n.WinMoney > 0)
            {

                builder.Append("中奖：" + n.WinZhu + "注/赢" + n.WinMoney + "" + ub.Get("SiteBz") + "");

                builder.Append("<br /><a href=\"" + Utils.getUrl("HP3.aspx?act=caseok&amp;info=ok&amp;id=" + n.ID + "") + "\">确定帮他兑奖</a>");
            }
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    #region 计算组合的数量
    static long jc(int N)//阶乘
    {
        long t = 1;

        for (int i = 1; i <= N; i++)
        {
            t *= i;
        }
        return t;
    }
    static long P(int N, int R)//组合的计算公式
    {
        long t = jc(N) / jc(N - R);

        return t;
    }
    static int C(int N, int R)//组合
    {
        long i = P(N, R) / jc(R);
        int t = Convert.ToInt32(i);
        return t;
    }
    #endregion
}
