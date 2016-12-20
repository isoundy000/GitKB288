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

public partial class Manage_guess_topGuess : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        int p_type = Utils.ParseInt(Utils.GetRequest("p_type", "get", 1, @"^[0-2]$", "0"));
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[1-3]$", "1"));
        Master.Title = "竞猜排行榜";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("榜:");
        if (p_type == 0)
            builder.Append("总榜 ");
        else
            builder.Append(Out.waplink(Utils.getUrl("topGuess.aspx?p_type=0&amp;ptype=" + ptype + ""), "总榜") + " ");
        if (p_type == 1)
            builder.Append("足球 ");
        else
            builder.Append(Out.waplink(Utils.getUrl("topGuess.aspx?p_type=1&amp;ptype=" + ptype + ""), "足球") + " ");

        if (p_type == 2)
            builder.Append("篮球 ");
        else
            builder.Append(Out.waplink(Utils.getUrl("topGuess.aspx?p_type=2&amp;ptype=" + ptype + ""), "篮球") + " ");

        builder.Append("<br />单:");

        if (ptype == 1)
            builder.Append("盈利 ");
        else
            builder.Append(Out.waplink(Utils.getUrl("topGuess.aspx?p_type=" + p_type + "&amp;ptype=1"), "盈利") + " ");
        if (ptype == 2)
            builder.Append("胜负率 ");
        else
            builder.Append(Out.waplink(Utils.getUrl("topGuess.aspx?p_type=" + p_type + "&amp;ptype=2"), "胜负率") + " ");

        if (ptype == 3)
            builder.Append("亏损 ");
        else
            builder.Append(Out.waplink(Utils.getUrl("topGuess.aspx?p_type=" + p_type + "&amp;ptype=3"), "亏损") + " ");

        builder.Append(Out.Tab("</div>", "<br />"));

        //组件查询条件
        string strWhere = "";
        string strOrder = "";

        if (p_type > 0)
            strWhere += "Orderstats=" + p_type + " and ";

        if (ptype == 1)
        {
            strWhere += "Orderjbnum>0";
            strOrder = "Orderjbnum DESC";
        }
        else if (ptype == 2)
        {
            strWhere += "Orderbanum>0";
            strOrder = "Orderbanum DESC";
        }
        else
        {
            strWhere += "Orderjbnum<0";
            strOrder = "Orderjbnum ASC";
        }

        int pageSize = 10;
        int pageIndex;
        int recordCount;
        string[] pageValUrl = { "p_type" ,"ptype"};
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取排行榜
        IList<TPR.Model.guess.BaOrder> listBaOrder = new TPR.BLL.guess.BaOrder().GetBaOrders(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listBaOrder.Count > 0)
        {
            int k = 1;
            foreach (TPR.Model.guess.BaOrder n in listBaOrder)
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

                if (ptype == 1)
                    builder.AppendFormat("[第{0}名]" + Out.waplink(Utils.getUrl("../uinfo.aspx?uid={1}&amp;backurl=" + Utils.PostPage(1)) + "", "{2}") + "盈利{3}币", (pageIndex - 1) * 10 + k, n.Orderusid, n.Orderusname, Convert.ToDouble(n.Orderjbnum));
                else if (ptype == 2)
                    builder.AppendFormat("[第{0}名]" + Out.waplink(Utils.getUrl("../uinfo.aspx?uid={1}&amp;backurl=" + Utils.PostPage(1)) + "", "{2}") + "胜负率{3}点,其中胜{4}场,负{5}场", (pageIndex - 1) * 10 + k, n.Orderusid, n.Orderusname, Convert.ToDouble(n.Orderbanum - n.Orderfanum), Convert.ToDouble(n.Orderbanum), Convert.ToDouble(n.Orderfanum));
                else
                    builder.AppendFormat("[第{0}名]" + Out.waplink(Utils.getUrl("../uinfo.aspx?uid={1}&amp;backurl=" + Utils.PostPage(1)) + "", "{2}") + "亏损{3}币", (pageIndex - 1) * 10 + k, n.Orderusid, n.Orderusname, Convert.ToDouble(n.Orderjbnum));

                builder.Append(Out.Tab("</div>", ""));

                k++;
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
        builder.Append(Out.waplink(Utils.getUrl("default.aspx"), "返回上一级"));
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
}
