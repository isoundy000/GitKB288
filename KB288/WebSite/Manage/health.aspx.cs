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
using BCW.Common;
public partial class Manage_health : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "系统健康监测";
        string act = Utils.GetRequest("act", "all", 1, "", "");
        builder.Append(Out.Tab("", ""));
        switch (act)
        {
            case "clear":
                ClearHealth();
                break;
            default:
                ReloadPage();
                break;
        }
    }
    private void ReloadPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">异常监测结果</div>", ""));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string[] pageValUrl = { "" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取专题
        IList<BCW.Model.Health> listHealth = new BCW.BLL.Health().GetHealths(pageIndex, pageSize, out recordCount);
        if (listHealth.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Health n in listHealth)
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
                builder.AppendFormat("{0}.时间:{1}事件消息:{2}请求 Url:{3}异常类型:{4}异常消息:{5}", (pageIndex - 1) * pageSize + k, n.EventTime, n.Message, Out.UBB(n.RequestUrl), Out.UBB(n.ExceptionType), Out.UBB(n.ExceptionMessage));

                k++;
                builder.Append(Out.Tab("</div>", ""));

            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录"));
        }

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("health.aspx?act=clear") + "\">清空异常</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    private void ClearHealth()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info != "ok")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定要清空异常记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("health.aspx?info=ok&amp;act=clear") + "\">确定清空</a> ");
            builder.Append("<br /><a href=\"" + Utils.getUrl("health.aspx") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            new BCW.BLL.Health().Delete();
            Utils.Success("清空异常", "清空异常记录成功..", Utils.getUrl("health.aspx"), "1");
        }
    }
}