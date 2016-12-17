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

public partial class Manage_game_apple : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/apple.xml";
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
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
        Master.Title = "苹果机管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("苹果机");
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
        IList<BCW.Model.Game.Applelist> listApplelist = new BCW.BLL.Game.Applelist().GetApplelists(pageIndex, pageSize, strWhere, 0, out recordCount);
        if (listApplelist.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Game.Applelist n in listApplelist)
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
                builder.Append("<a href=\"" + Utils.getUrl("apple.aspx?act=edit&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[管理]&gt;</a>");

                if (n.State == 0)
                    builder.Append("第" + n.ID + "期开出:<a href=\"" + Utils.getUrl("apple.aspx?act=view&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">未开</a>");
                else
                    builder.Append("第" + n.ID + "期开出:<a href=\"" + Utils.getUrl("apple.aspx?act=view&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.OpenText + "</a>");
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
        builder.Append("<a href=\"" + Utils.getUrl("apple.aspx?act=reset") + "\">重置游戏</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("../xml/appleset.aspx?backurl=" + Utils.PostPage(1) + "") + "\">游戏配置</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

    }

    private void ViewPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-2]\d*$", "1"));
        BCW.Model.Game.Applelist model = new BCW.BLL.Game.Applelist().GetApplelist(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "第" + id + "期苹果机";
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("查看下注/开奖记录");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("查看:");
        if (ptype == 1)
            builder.Append("下注|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("apple.aspx?act=view&amp;ptype=1&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">下注</a>|");

        if (ptype == 2)
            builder.Append("中奖");
        else
            builder.Append("<a href=\"" + Utils.getUrl("apple.aspx?act=view&amp;ptype=2&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">中奖</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        if (ptype == 1)
            strWhere += "AppleId=" + id + "";
        else
            strWhere += "AppleId=" + id + " and state>0 and winCent>0";

        string[] pageValUrl = { "act", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Game.Applepay> listApplepay = new BCW.BLL.Game.Applepay().GetApplepays(pageIndex, pageSize, strWhere, out recordCount);
        if (listApplepay.Count > 0)
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("第" + id + "期开出:<b>" + model.OpenText + "</b>");
            if (ptype == 1)
                builder.Append("<br />共" + recordCount + "注下注");
            else
                builder.Append("<br />共" + recordCount + "注中奖");

            builder.Append(Out.Tab("</div>", "<br />"));

            int k = 1;
            foreach (BCW.Model.Game.Applepay n in listApplepay)
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
                    builder.Append("" + AppleType(n.Types) + "，押" + n.PayCount + "份[" + DT.FormatDate(n.AddTime, 13) + "]");
                else if (n.State == 1)
                {
                    builder.Append("" + AppleType(n.Types) + "，押" + n.PayCount + "份[" + DT.FormatDate(n.AddTime, 1) + "]");
                    if (n.WinCent > 0)
                    {
                        builder.Append("赢" + n.WinCent + "" + ub.Get("SiteBz") + "");
                    }
                }
                else
                    builder.Append("" + AppleType(n.Types) + "，押" + n.PayCount + "份，赢" + n.WinCent + "" + ub.Get("SiteBz") + "[" + DT.FormatDate(n.AddTime, 1) + "]");


                k++;
                builder.Append(Out.Tab("</div>", ""));
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

        }
        else
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("第" + id + "期开出:<b>" + model.OpenText + "</b>");
            builder.Append("<br />共0注中奖");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("apple.aspx") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void EditPage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId != 1 && ManageId != 9)
        {
            Utils.Error("权限不足", "");
        }
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "苹果机ID错误"));
        Master.Title = "编辑苹果机";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("编辑第" + id + "期苹果机");
        builder.Append(Out.Tab("</div>", ""));
        BCW.Model.Game.Applelist model = new BCW.BLL.Game.Applelist().GetApplelist(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.State == 0)
        {
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("当开出结果非空时，开奖时则使用该结果作为开奖");
            builder.Append(Out.Tab("</div>", ""));
        }
        string strText = "开出结果:/,开奖时间:/,,,";
        string strName = "OpenText,EndTime,id,act,backurl";
        string strType = "text,date,hidden,hidden,hidden";
        string strValu = "" + model.OpenText + "'" + model.EndTime + "'" + id + "'editsave'" + Utils.getPage(0) + "";
        string strEmpt = "false,false,false,false,false";
        string strIdea = "/";
        string strOthe = "确定编辑|reset,apple.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        builder.Append("开出结果格式:“苹果”或“苹果[小]”");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("apple.aspx") + "\">返回上一级</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("apple.aspx?act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">删除本期</a><br />");
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
        string OpenText = Utils.GetRequest("OpenText", "post", 2, @"^[^\^]{2,5}$", "开出结果填写错误");
        DateTime EndTime = Utils.ParseTime(Utils.GetRequest("EndTime", "post", 2, DT.RegexTime, "开奖时间格式填写出错,正确格式如" + DT.FormatDate(DateTime.Now, 0) + ""));

        if (!new BCW.BLL.Game.Applelist().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }

        //记录日志
        if (Utils.GetDomain().Contains("kubao") || Utils.GetDomain().Contains("tuhao") || Utils.GetDomain().Contains("th") || Utils.GetDomain().Contains("kb288"))
        {
            String sLogFilePath = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "/logstr/" + DateTime.Now.ToString("MM-dd") + ".txt";
            LogHelper.Write(sLogFilePath, "操作管理员:" + ManageId + "号/编辑苹果机期数:" + id + "|是否预设:" +OpenText + "");
        }

        BCW.Model.Game.Applelist model = new BCW.Model.Game.Applelist();
        model.ID = id;
        model.EndTime = EndTime;
        model.OpenText = OpenText;
        new BCW.BLL.Game.Applelist().Update(model);
        Utils.Success("编辑第" + id + "期", "编辑第" + id + "期成功..", Utils.getUrl("apple.aspx?act=edit&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
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
            builder.Append("<a href=\"" + Utils.getUrl("apple.aspx?info=ok&amp;act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("apple.aspx?act=edit&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.BLL.Game.Applelist().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }

            //记录日志
            if (Utils.GetDomain().Contains("kubao") || Utils.GetDomain().Contains("tuhao") || Utils.GetDomain().Contains("th") || Utils.GetDomain().Contains("kb288"))
            {
                String sLogFilePath = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "/logstr/" + DateTime.Now.ToString("MM-dd") + ".txt";
                LogHelper.Write(sLogFilePath, "操作管理员:" + ManageId + "号/删除苹果机期数:" + id + "");
            }

            new BCW.BLL.Game.Applelist().Delete(id);
            new BCW.BLL.Game.Applepay().Delete("AppleId=" + id + "");
            Utils.Success("删除第" + id + "期", "删除第" + id + "期成功..", Utils.getPage("apple.aspx"), "1");
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
            builder.Append("确定重置苹果机游戏吗，重置后，将重新从第一期开始，所有记录将会期数和下注记录全被删除");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("apple.aspx?info=ok&amp;act=reset") + "\">确定重置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("apple.aspx") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            new BCW.Data.SqlUp().ClearTable("tb_Applelist");
            new BCW.Data.SqlUp().ClearTable("tb_Applepay");
            Utils.Success("重置游戏", "重置游戏成功..", Utils.getUrl("apple.aspx"), "1");
        }
    }

    private string AppleType(int Types)
    {
        string AppleName = string.Empty;
        if (Types == 1)
            AppleName = "苹果";
        else if (Types == 2)
            AppleName = "木瓜";
        else if (Types == 3)
            AppleName = "西瓜";
        else if (Types == 4)
            AppleName = "芒果";
        else if (Types == 5)
            AppleName = "双星";
        else if (Types == 6)
            AppleName = "金钟";
        else if (Types == 7)
            AppleName = "双七";
        else if (Types == 8)
            AppleName = "元宝";

        return AppleName;
    }
}
