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

/// <summary>
/// 最后修改时间 20160202
/// 黄国军
/// </summary>
public partial class Manage_action : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    private string M_Str_mindate = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "动态管理";
        string act = Utils.GetRequest("act", "all", 1, "", "");

        switch (act)
        {
            case "clear":
                ClearPage();
                break;
            case "clearok":
                ClearOkPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    #region 动态管理 ReloadPage
    /// <summary>
    /// 动态管理
    /// 新增2016年游戏ID返回的连接
    /// 黄国军 20160202
    /// </summary>
    private void ReloadPage()
    {
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-1]\d*$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("动态管理");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 0)
            builder.Append("会员动态|<a href=\"" + Utils.getUrl("action.aspx?ptype=1") + "\">游戏动态</a>");
        else
            builder.Append("<a href=\"" + Utils.getUrl("action.aspx") + "\">会员动态</a>|游戏动态");

        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "uid", "ptype" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        if (ptype == 0)
            strWhere = "Types=0";
        else
            strWhere = "Types>0";

        if (uid > 0)
            strWhere += "usid=" + uid + "";

        // 开始读取列表
        IList<BCW.Model.Action> listAction = new BCW.BLL.Action().GetActions(pageIndex, pageSize, strWhere, out recordCount);
        if (listAction.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Action n in listAction)
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

                ///新增2016年游戏ID返回的连接 黄国军 20160202 new BCW.BLL.Action().CaseAction(n.Types) 
                if (ptype > 0)
                    builder.AppendFormat("{0}:{1}前，{2}", new BCW.BLL.Action().CaseAction(n.Types), DT.DateDiff2(DateTime.Now, n.AddTime), Out.SysUBB(n.Notes));
                else
                    builder.AppendFormat("{0}前<a href=\"" + Utils.getUrl("uinfo.aspx?uid={1}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}</a>{3}", DT.DateDiff2(DateTime.Now, n.AddTime), n.UsId, n.UsName, Out.SysUBB(n.Notes));
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
        string strName = "uid,ptype";
        string strType = "num,hidden";
        string strValu = "'" + ptype + "";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "搜动态,action.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("action.aspx?act=clear") + "\">清空记录</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    #endregion

    /// <summary>
    /// 清空动态记录
    /// </summary>
    private void ClearPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("请选择清空选项");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("action.aspx?act=clearok&amp;ptype=1") + "\">清空本日前</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("action.aspx?act=clearok&amp;ptype=2") + "\">清空本周前</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("action.aspx?act=clearok&amp;ptype=3") + "\">清空本月前</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("action.aspx?act=clearok&amp;ptype=4") + "\">清空全部</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("action.aspx") + "\">动态管理</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    /// <summary>
    /// 清空动态记录
    /// </summary>
    private void ClearOkPage()
    {
        string act = Utils.GetRequest("act", "get", 1, "", "");
        string info = Utils.GetRequest("info", "get", 1, "", "");
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-4]$", "0"));
        if (info != "ok" && info != "acok")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定要清空吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("action.aspx?info=ok&amp;act=" + act + "&amp;ptype=" + ptype + "") + "\">确定清空</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("action.aspx?act=clear") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("action.aspx") + "\">动态管理</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (info == "ok")
            {
                Utils.Success("正在清空", "正在清空,请稍后..", Utils.getUrl("action.aspx?info=acok&amp;act=" + act + "&amp;ptype=" + ptype + ""), "2");
            }
            else
            {
                if (ptype == 1)
                {
                    //保留本日计算
                    M_Str_mindate = DateTime.Now.ToShortDateString() + " 0:00:00";
                    new BCW.BLL.Action().Delete("AddTime<'" + M_Str_mindate + "'");
                }
                else if (ptype == 2)
                {
                    //保留本周计算
                    switch (DateTime.Now.DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            M_Str_mindate = DateTime.Now.AddDays(0).ToShortDateString() + " 0:00:00";
                            break;
                        case DayOfWeek.Tuesday:
                            M_Str_mindate = DateTime.Now.AddDays(-1).ToShortDateString() + " 0:00:00";
                            break;
                        case DayOfWeek.Wednesday:
                            M_Str_mindate = DateTime.Now.AddDays(-2).ToShortDateString() + " 0:00:00";
                            break;
                        case DayOfWeek.Thursday:
                            M_Str_mindate = DateTime.Now.AddDays(-3).ToShortDateString() + " 0:00:00";
                            break;
                        case DayOfWeek.Friday:
                            M_Str_mindate = DateTime.Now.AddDays(-4).ToShortDateString() + " 0:00:00";
                            break;
                        case DayOfWeek.Saturday:
                            M_Str_mindate = DateTime.Now.AddDays(-5).ToShortDateString() + " 0:00:00";
                            break;
                        case DayOfWeek.Sunday:
                            M_Str_mindate = DateTime.Now.AddDays(-6).ToShortDateString() + " 0:00:00";
                            break;
                    }
                    new BCW.BLL.Action().Delete("AddTime<'" + M_Str_mindate + "'");
                }
                else if (ptype == 3)
                {
                    //保留本月计算
                    string MonthText = DateTime.Now.Year + "-" + DateTime.Now.Month;
                    M_Str_mindate = MonthText + "-1 0:00:00";
                    new BCW.BLL.Action().Delete("AddTime<'" + M_Str_mindate + "'");
                }
                else if (ptype == 4)
                {
                    new BCW.BLL.Action().Delete("");
                }
                Utils.Success("清空成功", "清空操作成功..", Utils.getPage("action.aspx?act=clear"), "2");
            }
        }
    }
}
