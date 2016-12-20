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

public partial class Manage_guess_payView : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "下注详细记录";
        int gid = Utils.ParseInt(Utils.GetRequest("gid", "get", 2, @"^[0-9]\d*$", "竞猜ID无效"));
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[0-9]\d*$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        if (ptype == 0)
        {

            builder.Append("下注记录|" + Out.waplink(Utils.getUrl("payView.aspx?gid=" + gid + "&amp;ptype=1"), "赢输记录"));
        }
        else
        {
            builder.Append(Out.waplink(Utils.getUrl("payView.aspx?gid=" + gid + "&amp;ptype=0"), "下注记录") + "|赢输记录");
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageSize = 10;
        int pageIndex;
        int recordCount;
        string[] pageValUrl = { "gid", "ptype" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //组合条件
        string strWhere = "";
        strWhere += "bcid=" + gid + " and types=0";
        if (ptype==1)
            strWhere += "and p_active<>0";
        // 开始读取竞猜
        IList<TPR.Model.guess.BaPay> listBaPay = new TPR.BLL.guess.BaPay().GetBaPayViews(pageIndex, pageSize, strWhere, ptype, out recordCount);
        if (listBaPay.Count > 0)
        {
            int k = 1;
            foreach (TPR.Model.guess.BaPay n in listBaPay)
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
                if (ptype == 0)
                {
                    builder.AppendFormat("<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.payusid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">{0}({1})</a>下注{2}"+ub.Get("SiteBz")+",共{3}注", n.payusname, n.payusid, Convert.ToDouble(n.payCents), n.payCount);
                }
                else
                {
                    builder.AppendFormat("<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.payusid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">{0}({1})</a>下注{2}"+ub.Get("SiteBz")+",返{3},盈利{4}", n.payusname, n.payusid, Convert.ToDouble(n.payCents), Convert.ToDouble(n.payCount), Convert.ToDouble(n.payCount - n.payCents));

                }
                builder.Append(Out.Tab("</div>", ""));
                k++;
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            if (ptype == 0)
                builder.Append(Out.Div("div", "没有相关记录.."));
            else
                builder.Append(Out.Div("div", "没有记录或赛事并没有结束.."));

        }

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.waplink(Utils.getUrl("showGuess.aspx?gid=" + gid + ""), "返回上一级"));
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
}
