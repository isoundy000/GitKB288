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
using TPR2.Common;

public partial class bbs_guess2_myGuess : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[1-2]$", "1"));

        int p_type = Utils.ParseInt(Utils.GetRequest("p_type", "get", 1, @"^[0-2]$", "0"));

        int paytype = Utils.ParseInt(Utils.GetRequest("paytype", "get", 1, @"^[0-4]$", "0"));

        string strTitle = "";
        if (ptype == 1)
            strTitle = "未开投注";
        else
            strTitle = "历史投注";

        Master.Title = strTitle;

        //会员身份页面取会员实体
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        builder.Append(Out.Tab("<div class=\"title\">", ""));

        builder.Append("球:");
        if (p_type == 0)
            builder.Append("全部 ");
        else
            builder.Append(Out.waplink(Utils.getUrl("myGuess.aspx?ptype=" + ptype + "&amp;p_type=0&amp;paytype=" + paytype + ""), "全部") + " ");

        if (p_type == 1)
            builder.Append("足球 ");
        else
            builder.Append(Out.waplink(Utils.getUrl("myGuess.aspx?ptype=" + ptype + "&amp;p_type=1&amp;paytype=" + paytype + ""), "足球") + " ");

        if (p_type == 2)
            builder.Append("篮球 ");
        else
            builder.Append(Out.waplink(Utils.getUrl("myGuess.aspx?ptype=" + ptype + "&amp;p_type=2&amp;paytype=" + paytype + ""), "篮球") + " ");

        builder.Append("<br />盘:");

        if (paytype == 0)
            builder.Append("全部 ");
        else
            builder.Append(Out.waplink(Utils.getUrl("myGuess.aspx?ptype=" + ptype + "&amp;p_type=" + p_type + "&amp;paytype=0"), "全部") + " ");

        if (paytype == 1)
            builder.Append("让球 ");
        else
            builder.Append(Out.waplink(Utils.getUrl("myGuess.aspx?ptype=" + ptype + "&amp;p_type=" + p_type + "&amp;paytype=1"), "让球") + " ");

        if (paytype == 2)
            builder.Append("大小 ");
        else
            builder.Append(Out.waplink(Utils.getUrl("myGuess.aspx?ptype=" + ptype + "&amp;p_type=" + p_type + "&amp;paytype=2"), "大小") + " ");

        if (p_type != 2)
        {
            if (paytype == 3)
                builder.Append("标准 ");
            else
                builder.Append(Out.waplink(Utils.getUrl("myGuess.aspx?ptype=" + ptype + "&amp;p_type=" + p_type + "&amp;paytype=3"), "标准") + " ");

            if (paytype == 4)
                builder.Append("波胆 ");
            else
                builder.Append(Out.waplink(Utils.getUrl("myGuess.aspx?ptype=" + ptype + "&amp;p_type=" + p_type + "&amp;paytype=4"), "波胆") + " ");
        }
        builder.Append(Out.Tab("</div >", "<br />"));

        int pageSize = 10;
        int pageIndex;
        int recordCount;
        string[] pageValUrl = { "ptype", "p_type", "paytype" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //组合条件
        string strWhere = "";
        if (ptype == 1)
            strWhere += "p_active=0 and payusid=" + meid + " and itypes=0 ";
        else
            strWhere += "p_active>0 and payusid=" + meid + " and itypes=0 ";

        if (p_type != 0)
            strWhere += "and ptype=" + p_type + "";

        if (paytype != 0)
        {
            if (paytype == 1)
                strWhere += "and (paytype=1 or paytype=2)";
            if (paytype == 2)
                strWhere += "and (paytype=3 or paytype=4)";
            if (paytype == 3)
                strWhere += "and (paytype=5 or paytype=6 or paytype=7)";
            if (paytype == 4)
                strWhere += "and (paytype>100)";
        }
        // 开始读取竞猜
        IList<TPR2.Model.guess.BaPay> listBaPay = new TPR2.BLL.guess.BaPay().GetBaPays(pageIndex, pageSize, strWhere, out recordCount);
        if (listBaPay.Count > 0)
        {
            int k = 1;
            foreach (TPR2.Model.guess.BaPay n in listBaPay)
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

                if (n.p_active == 0)
                {
                    builder.AppendFormat("ID:" + n.ID + ".{0}<br />时间:{1}", Out.SysUBB(n.payview), DT.FormatDate(Convert.ToDateTime(n.paytimes), 0));
                    if (n.state >= 1)
                        builder.Append("*待确认");
                }
                else if (n.p_active == 2)
                {
                    builder.AppendFormat("ID:" + n.ID + ".{0},平盘<br />时间:{1}", Out.SysUBB(n.payview), DT.FormatDate(Convert.ToDateTime(n.paytimes), 0));
                    builder.AppendFormat(" 返{0}币", Convert.ToDouble(n.p_getMoney));
                }
                else
                {
                    builder.AppendFormat("ID:" + n.ID + ".{0},结果{1}:{2}<br />时间:{3}", Out.SysUBB(n.payview), n.p_result_one, n.p_result_two, DT.FormatDate(Convert.ToDateTime(n.paytimes), 0));

                    if (Convert.ToInt32(n.p_getMoney) > 0)
                    {
                        builder.AppendFormat(" 返{0}币", Convert.ToDouble(n.p_getMoney));
                    }
                    else
                    {
                        builder.AppendFormat(" 输{0}币", Convert.ToDouble(n.payCent));
                    }
                }

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
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append(Out.waplink(Utils.getUrl("myGuess.aspx?ptype=1"), "未开投注") + " ");
        builder.Append(Out.waplink(Utils.getUrl("myGuess.aspx?ptype=2"), "历史投注") + "<br />");
        builder.Append(Out.waplink(Utils.getUrl("default.aspx"), "返回球彩首页") + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(Out.waplink(Utils.getUrl("/default.aspx"), "首页") + "-");
        builder.Append(Out.waplink(Utils.getPage("default.aspx"), "上级") + "-");
        builder.Append(Out.waplink(Utils.getUrl("default.aspx"), "球彩") + "");
        builder.Append(Out.Tab("</div>", ""));
 
    }
}
