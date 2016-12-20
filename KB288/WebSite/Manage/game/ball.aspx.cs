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

public partial class Manage_game_ball : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/ball.xml";
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "open":
                OpenPage();
                break;
            case "opensave":
                OpenSavePage();
                break;
            case "edit":
                EditPage();
                break;
            case "editsave":
                EditSavePage();
                break;
            case "del":
                DelPage();
                break;
            case "view":
                ViewPage();
                break;
            case "reset":
                ResetPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        Master.Title = "疯狂彩球管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("疯狂彩球");
        builder.Append(Out.Tab("</div>", "<br />"));

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
        IList<BCW.Model.Game.Balllist> listBalllist = new BCW.BLL.Game.Balllist().GetBalllists(pageIndex, pageSize, strWhere, out recordCount);
        if (listBalllist.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Balllist n in listBalllist)
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
                builder.Append("<a href=\"" + Utils.getUrl("ball.aspx?act=edit&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[管理]&gt;</a>");

                if (n.State == 0)
                    builder.Append("第" + n.ID + "期开出号码:<a href=\"" + Utils.getUrl("ball.aspx?act=view&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">未开</a> <a href=\"" + Utils.getUrl("ball.aspx?act=open&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">开奖</a>");
                else
                    builder.Append("第" + n.ID + "期开出号码:<a href=\"" + Utils.getUrl("ball.aspx?act=view&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.WinNum + "</a>");
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
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("ball.aspx?act=reset") + "\">重置游戏</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("../xml/ballset.aspx?backurl=" + Utils.PostPage(1) + "") + "\">游戏配置</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("../toplist.aspx?act=top&amp;ptype=6&amp;backurl=" + Utils.PostPage(1) + "") + "\">排行榜单</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

    }

    private void ViewPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-2]\d*$", "1"));
        BCW.Model.Game.Balllist model = new BCW.BLL.Game.Balllist().GetBalllist(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "第" + id + "期疯狂彩球";
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("查看下注/开奖记录");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("查看:");
        if (ptype == 1)
            builder.Append("下注|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("ball.aspx?act=view&amp;ptype=1&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">下注</a>|");

        if (ptype == 2)
            builder.Append("中奖");
        else
            builder.Append("<a href=\"" + Utils.getUrl("ball.aspx?act=view&amp;ptype=2&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">中奖</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        if (ptype == 1)
            strWhere += "BallId=" + id + "";
        else
            strWhere += "BallId=" + id + " and state>0 and winCent>0";

        string[] pageValUrl = { "act", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Game.Ballpay> listBallpay = new BCW.BLL.Game.Ballpay().GetBallpays(pageIndex, pageSize, strWhere, out recordCount);
        if (listBallpay.Count > 0)
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("第" + id + "期开出号码:<b>" + model.WinNum + "</b>");
            if (ptype == 1)
                builder.Append("<br />共" + recordCount + "注下注");
            else
                builder.Append("<br />共" + recordCount + "注中奖");

            builder.Append(Out.Tab("</div>", "<br />"));

            int k = 1;
            foreach (BCW.Model.Game.Ballpay n in listBallpay)
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

                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "</a>");
                if (n.State == 0)
                    builder.Append("买" + n.BuyNum + "，共押" + n.BuyCount + "份/花费" + n.BuyCent + "" + ub.Get("SiteBz") + "[" + DT.FormatDate(n.AddTime, 13) + "]");
                else if (n.State == 1)
                {
                    builder.Append("买" + n.BuyNum + "，共押" + n.BuyCount + "份/花费" + n.BuyCent + "" + ub.Get("SiteBz") + "[" + DT.FormatDate(n.AddTime, 1) + "]");
                    if (n.WinCent > 0)
                    {
                        builder.Append("赢" + n.WinCent + "" + ub.Get("SiteBz") + "");
                    }
                }
                else
                    builder.Append("买" + n.BuyNum + "，共押" + n.BuyCount + "份/花费" + n.BuyCent + "" + ub.Get("SiteBz") + "，赢" + n.WinCent + "" + ub.Get("SiteBz") + "[" + DT.FormatDate(n.AddTime, 1) + "]");


                k++;
                builder.Append(Out.Tab("</div>", ""));
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

        }
        else
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("第" + id + "期开出号码:<b>" + model.WinNum + "</b>");
            builder.Append("<br />共0注中奖");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("ball.aspx") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void OpenPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "疯狂彩球ID错误"));
        Master.Title = "疯狂彩球开奖";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("第" + id + "期疯狂彩球开奖");
        builder.Append(Out.Tab("</div>", ""));
        BCW.Model.Game.Balllist model = new BCW.BLL.Game.Balllist().GetBalllist(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.State == 1)
        {
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("本期已开奖，要重新开奖吗");
            builder.Append(Out.Tab("</div>", ""));
        }
        string strText = "开出数字:/,,,";
        string strName = "WinNum,id,act,backurl";
        string strType = "num,hidden,hidden,hidden";
        string strValu = "" + model.WinNum + "'" + id + "'opensave'" + Utils.getPage(0) + "";
        string strEmpt = "false,false,false,false";
        string strIdea = "/";
        string strOthe = "确定开奖|reset,ball.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("ball.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    private void OpenSavePage()
    {
        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[1-9]\d*$", "ID错误"));
        int WinNum = int.Parse(Utils.GetRequest("WinNum", "post", 2, @"^[1-9]$|^[1-4]([0-9])?$", "开出数字限1-" + ub.GetSub("BallSysNum", xmlPath) + ""));
        if (WinNum < 1 || WinNum > Utils.ParseInt(ub.GetSub("BallSysNum", xmlPath)))
        {
            Utils.Error("下注数字限1-" + ub.GetSub("BallSysNum", xmlPath) + "", "");
        }
        if (!new BCW.BLL.Game.Balllist().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }

        //记录日志
        if (Utils.GetDomain().Contains("kubao") || Utils.GetDomain().Contains("tuhao") || Utils.GetDomain().Contains("th") || Utils.GetDomain().Contains("kb288"))
        {
            int ManageId = new BCW.User.Manage().IsManageLogin();
            String sLogFilePath = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "/logstr/" + DateTime.Now.ToString("MM-dd") + ".txt";
            LogHelper.Write(sLogFilePath, "操作管理员:" + ManageId + "号/彩球开奖|期数:" + id + "/开奖数字:" + WinNum + "");
        }

        new BCW.User.Ball().BallPage(id, WinNum);
        Utils.Success("第" + id + "期开奖", "第" + id + "期开奖成功..", Utils.getUrl("ball.aspx"), "1");
    }

    private void EditPage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId != 1 && ManageId != 9)
        {
            Utils.Error("权限不足", "");
        }
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "疯狂彩球ID错误"));
        Master.Title = "编辑疯狂彩球";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("编辑第" + id + "期疯狂彩球");
        builder.Append(Out.Tab("</div>", ""));
        BCW.Model.Game.Balllist model = new BCW.BLL.Game.Balllist().GetBalllist(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.State == 0)
        {
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("当开出数字非0时，开奖时则使用该数字作为开奖结果");
            builder.Append(Out.Tab("</div>", ""));
        }
        string strText = "开出数字:/,开盘时间:/,开奖时间:/,本期可购买份数:/,已购买份数:/,每份价格:/,本期赔率:/,奖池额:/,上期奖池额:/,,,";
        string strName = "WinNum,BeginTime,EndTime,OutNum,AddNum,iCent,Odds,Pool,BeforePool,id,act,backurl";
        string strType = "num,date,date,num,num,num,num,num,num,hidden,hidden,hidden";
        string strValu = "" + model.WinNum + "'" + model.BeginTime + "'" + model.EndTime + "'" + model.OutNum + "'" + model.AddNum + "'" + model.iCent + "'" + model.Odds + "'" + model.Pool + "'" + model.BeforePool + "'" + id + "'editsave'" + Utils.getPage(0) + "";
        string strEmpt = "false,false,false,false,false,false,false,false,false,false,false,false";
        string strIdea = "/";
        string strOthe = "确定编辑|reset,ball.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("ball.aspx") + "\">返回上一级</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("ball.aspx?act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">删除本期</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void EditSavePage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId != 1 && ManageId != 9)
        {
            Utils.Error("权限不足", "");
        }
        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[1-9]\d*$", "ID错误"));
        int WinNum = int.Parse(Utils.GetRequest("WinNum", "post", 2, @"^[0-9]\d*$", "开出数字填写错误"));
        DateTime BeginTime = Utils.ParseTime(Utils.GetRequest("BeginTime", "post", 2, DT.RegexTime, "开盘时间格式填写出错,正确格式如" + DT.FormatDate(DateTime.Now, 0) + ""));
        DateTime EndTime = Utils.ParseTime(Utils.GetRequest("EndTime", "post", 2, DT.RegexTime, "开奖时间格式填写出错,正确格式如" + DT.FormatDate(DateTime.Now, 0) + ""));
        int OutNum = int.Parse(Utils.GetRequest("OutNum", "post", 2, @"^[0-9]\d*$", "可购买份数填写错误"));
        int AddNum = int.Parse(Utils.GetRequest("AddNum", "post", 2, @"^[0-9]\d*$", "已购买份数填写错误"));
        int iCent = int.Parse(Utils.GetRequest("iCent", "post", 2, @"^[0-9]\d*$", "每份价格填写错误"));
        int Odds = int.Parse(Utils.GetRequest("Odds", "post", 2, @"^[0-9]\d*$", "本期赔率填写错误"));
        long Pool = Int64.Parse(Utils.GetRequest("Pool", "post", 4, @"^[0-9]\d*$", "奖池额填写错误"));
        long BeforePool = Int64.Parse(Utils.GetRequest("BeforePool", "post", 4, @"^[0-9]\d*$", "上期奖池额填写错误"));

        if (!new BCW.BLL.Game.Balllist().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }

        //记录日志
        if (Utils.GetDomain().Contains("kubao") || Utils.GetDomain().Contains("tuhao") || Utils.GetDomain().Contains("th") || Utils.GetDomain().Contains("kb288"))
        {
            String sLogFilePath = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "/logstr/" + DateTime.Now.ToString("MM-dd") + ".txt";
            LogHelper.Write(sLogFilePath, "操作管理员:" + ManageId + "号/编辑彩球期数:" + id + "|是否预设:" + WinNum + "");
        }

        BCW.Model.Game.Balllist model = new BCW.Model.Game.Balllist();
        model.ID = id;
        model.WinNum = WinNum;
        model.BeginTime = BeginTime;
        model.EndTime = EndTime;
        model.OutNum = OutNum;
        model.AddNum = AddNum;
        model.iCent = iCent;
        model.Odds = Odds;
        model.Pool = Pool;
        model.BeforePool = BeforePool;
        new BCW.BLL.Game.Balllist().Update(model);
        Utils.Success("编辑第" + id + "期", "编辑第" + id + "期成功..", Utils.getUrl("ball.aspx?act=edit&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
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
            Master.Title = "删除第" + id + "期";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定第" + id + "期记录吗.删除同时将会删除该期的下注记录.");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("ball.aspx?info=ok&amp;act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("ball.aspx?act=edit&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.BLL.Game.Balllist().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }

            //记录日志
            if (Utils.GetDomain().Contains("kubao") || Utils.GetDomain().Contains("tuhao") || Utils.GetDomain().Contains("th") || Utils.GetDomain().Contains("kb288"))
            {
                String sLogFilePath = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "/logstr/" + DateTime.Now.ToString("MM-dd") + ".txt";
                LogHelper.Write(sLogFilePath, "操作管理员:" + ManageId + "号/删除彩球期数:" + id + "");
            }

            new BCW.BLL.Game.Balllist().Delete(id);
            new BCW.BLL.Game.Ballpay().Delete("BallId=" + id + "");
            Utils.Success("删除第" + id + "期", "删除第" + id + "期成功..", Utils.getPage("ball.aspx"), "1");
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
            builder.Append("确定重置疯狂彩球游戏吗，重置后，将重新从第一期开始，所有记录将会期数和下注记录全被删除");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("ball.aspx?info=ok&amp;act=reset") + "\">确定重置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("ball.aspx") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            new BCW.Data.SqlUp().ClearTable("tb_Balllist");
            new BCW.Data.SqlUp().ClearTable("tb_Ballpay");
            Utils.Success("重置游戏", "重置游戏成功..", Utils.getUrl("ball.aspx"), "1");
        }
    }
}