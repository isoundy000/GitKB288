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
using System.Text.RegularExpressions;
using BCW.Common;

public partial class Manage_guess_super : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string Strbuilder = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "超级竞猜(串串)管理";
         string act = "";
        act = Utils.GetRequest("act", "all", 1, "", "");

        switch (act)
        {
            case "view":
                ViewPage();
                break;
            default:
                ReloadPage();
                break;
        }

    }
    
    private void ReloadPage()
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-1]$", "0"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("超级竞猜(串串)管理");
        builder.Append(Out.Tab("</div>", "<br />"));
        int uid = Utils.ParseInt(Utils.GetRequest("uid", "all", 1, "", "0"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));

        if (ptype == 0)
            builder.Append("全部|" + Out.waplink(Utils.getUrl("super.aspx?ptype=1&amp;uid=" + uid + ""), "未返") + "");
        else
            builder.Append("" + Out.waplink(Utils.getUrl("super.aspx?uid=" + uid + ""), "全部") + "|未返");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageSize = 10;
        int pageIndex;
        int recordCount;
        string strWhere = "";
        string[] pageValUrl = { "act", "ptype","uid" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        if (uid > 0)
            strWhere += "UsID=" + uid + " and ";

        strWhere += "IsOpen=1";

        if (ptype == 1)
            strWhere += " and Status=0";

        // 开始读取竞猜
        IList<TPR.Model.guess.Super> listSuper = new TPR.BLL.guess.Super().GetSupers(pageIndex, pageSize, strWhere, out recordCount);
        if (listSuper.Count > 0)
        {
            int k = 1;
            foreach (TPR.Model.guess.Super n in listSuper)
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

                string sWin = string.Empty;
                if (n.Status == 0)
                    sWin = "未返";
                else if (n.Status == 1)
                    sWin = "赢";
                else
                    sWin = "输";

                builder.Append(Out.waplink(Utils.getUrl("super.aspx?act=view&amp;uid=" + n.UsID + "&amp;id=" + n.ID + ""), "[" + sWin + "]串竞猜" + Convert.ToDouble(n.PayCent) + "" + ub.Get("SiteBz") + "") + "");
                string[] Title = Regex.Split(Utils.Mid(n.Title, 2, n.Title.Length), "##");
                for (int i = 0; i < Title.Length; i++)
                {
                    builder.Append("<br />场次" + (i + 1) + ":" + Title[i] + "");
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
        string strText = "输入用户ID:/,";
        string strName = "uid,backurl";
        string strType = "text,hidden";
        string strValu = "'" + Utils.getPage(0) + "";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "搜串串记录,super.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.waplink(Utils.getUrl("default.aspx"), "返回上一级"));
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void ViewPage()
    {
        int uid = Utils.ParseInt(Utils.GetRequest("uid", "get", 2, @"^[0-9]\d*$", "ID无效"));
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID无效"));
        if (!new TPR.BLL.guess.Super().Exists(id, uid))
        {
            Utils.Error("不存在的记录..", "");
        }

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("" + Out.waplink(Utils.getUrl("super.aspx?uid=" + uid + ""), "串串记录") + "|查看详情");
        builder.Append(Out.Tab("</div>", "<br />"));

        TPR.Model.guess.Super model = new TPR.BLL.guess.Super().GetSuper(id);

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("" + Out.waplink(Utils.getUrl("../uinfo.aspx?uid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + ""), "" + model.UsName + "(" + model.UsID + ")") + "|" + DT.FormatDate(Convert.ToDateTime(model.AddTime), 0) + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        string[] BID = Regex.Split(Utils.Mid(model.BID, 2, model.BID.Length), "##");
        string[] Title = Regex.Split(Utils.Mid(model.Title, 2, model.Title.Length), "##");
        string[] Times = Regex.Split(Utils.Mid(model.Times, 2, model.Times.Length), "##");
        string[] StTitle = Regex.Split(Utils.Mid(model.StTitle, 2, model.StTitle.Length), "##");
        string[] Odds = Regex.Split(Utils.Mid(model.Odds, 2, model.Odds.Length), "##");

        string[] getOdds = null;
        if (model.Status > 0)
        {
            getOdds = Regex.Split(Utils.Mid(model.getOdds, 2, model.getOdds.Length), "##");
        }
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(model.AddTime);

        double pl = 1;
        double pCent = Convert.ToDouble(model.PayCent);
        for (int i = 0; i < Title.Length; i++)
        {
            builder.Append("<br />比赛场次" + (i + 1) + "：" + Title[i] + "<br />");
            builder.Append("比赛时间：" + Times[i] + "<br />");
            builder.Append("" + StTitle[i] + ",");
            builder.Append("赔率:" + Odds[i]);

            if (model.Status > 0)
            {
                builder.Append("<br />结果:" + getOdds[i]);
                string str = getOdds[i].ToString();

                if (str == "全输")
                {
                    pl = pl * 0;
                    pCent = pCent * 0;
                }
                else if (str == "平盘")
                {
                    pl = pl * 1;
                    pCent = pCent * 1;
                }
                else if (str == "输半")
                {
                    pl = pl * 0.5;
                    pCent = pCent * 0.5;
                }
                else if (str == "全赢")
                {
                    pl = pl * 1.9;
                    pCent = pCent * 1.9;
                }
                else//赢半
                {
                    pl = pl * 1.45;
                    pCent = pCent * 1.45;
                }

            }
            else
            {
                builder.Append("<br />结果:未返");
            }
        }
        builder.Append(Out.Tab("</div>", Out.Hr()));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (model.Status == 1)
        {
            builder.Append("投注:" + Convert.ToDouble(model.PayCent) + "" + ub.Get("SiteBz") + "<br />");
            builder.Append("总计:赢了" + pCent + "" + ub.Get("SiteBz") + "");
            if (model.p_case == 0)
                builder.Append("(未兑奖)");
            else
                builder.Append("(已兑奖)");

            builder.Append("<br />总赔率:" + Convert.ToDouble(pl) + "");
        }
        else if (model.Status == 2)
        {
            builder.Append("投注:" + Convert.ToDouble(model.PayCent) + "" + ub.Get("SiteBz") + "<br />");
            builder.Append("总计:输了" + Convert.ToDouble(model.PayCent) + "" + ub.Get("SiteBz") + "");
        }
        else
        {
            builder.Append("投注:" + Convert.ToDouble(model.PayCent) + "" + ub.Get("SiteBz") + "<br />");
            builder.Append("总计:等待返彩");
        }
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.waplink(Utils.getUrl("super.aspx?uid=" + uid + ""), "返回上一级") + "");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
}
