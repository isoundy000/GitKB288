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

public partial class Manage_toplist : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string[] sName = { };
    protected string[] sUrl = { };
    protected void Page_Load(object sender, EventArgs e)
    {
        string endName = string.Empty;
        string endUrl = string.Empty;
        endName = "剪刀石头布,789游戏,猜猜乐,幸运28,虚拟投注,疯狂彩球,疯狂吹牛,猜拳游戏,大小庄,大小掷骰,时时彩,好彩一";
        endUrl = "1,2,3,4,5,6,7,8,9,10,11,24";

        sName = endName.Split(",".ToCharArray());
        sUrl = endUrl.Split(",".ToCharArray());

        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "top":
                TopPage();
                break;
            case "del":
                DelPage();
                break;
            default:
                TopPage();
                break;
        }
    }

    private void TopPage()
    {
        Master.Title = "排行榜管理";
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-9]\d*$", "1"));
        int showtype = int.Parse(Utils.GetRequest("showtype", "all", 1, @"^[1-2]$", "1"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        if (sName.Length < ptype)
        {
            Utils.Error("不存在的类型", "");
        }

        builder.Append("" + sName[ptype - 1] + "排行榜");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (showtype == 1)
            builder.Append("游戏赌神|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("toplist.aspx?act=top&amp;ptype=" + ptype + "&amp;showtype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">游戏赌神</a>|");

        if (showtype == 2)
            builder.Append("游戏狂人");
        else
            builder.Append("<a href=\"" + Utils.getUrl("toplist.aspx?act=top&amp;ptype=" + ptype + "&amp;showtype=2&amp;backurl=" + Utils.getPage(0) + "") + "\">游戏狂人</a>");

        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string strOrder = "";
        string[] pageValUrl = { "act", "ptype", "id", "showtype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        strWhere = "Types=" + ptype + "";

        //排序条件
        if (ptype == 1 || ptype == 2)
        {
            if (showtype == 1)
                strOrder = "(WinGold+PutGold) Desc";
            else
                strOrder = "PutNum Desc";
        }
        else
        {
            if (showtype == 1)
                strOrder = "(WinGold+PutGold) Desc";
            else
                strOrder = "(WinNum-PutNum) Desc";
        }
        // 开始读取列表
        IList<BCW.Model.Toplist> listToplist = new BCW.BLL.Toplist().GetToplists(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listToplist.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Toplist n in listToplist)
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
                string sText = string.Empty;
                if (ptype == 1 || ptype == 2)
                {
                    if (showtype == 1)
                        sText = "净赢" + (n.WinGold + n.PutGold) + "" + ub.Get("SiteBz") + "";
                    else
                        sText = "共出手" + n.PutNum + "次";
                }
                else
                {
                    if (showtype == 1)
                        sText = "净赢" + (n.WinGold + n.PutGold) + "" + ub.Get("SiteBz") + "";
                    else
                        sText = "胜" + (n.WinNum - n.PutNum) + "次";
                }
                builder.AppendFormat("{0}.<a href=\"" + Utils.getUrl("uinfo.aspx?uid={1}&amp;backurl=" + Utils.getPage(1) + "") + "\">{2}</a>{3}", (pageIndex - 1) * pageSize + k, n.UsId, n.UsName, sText);
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
        builder.Append("<a href=\"" + Utils.getUrl("toplist.aspx?act=del&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">清空记录</a><br />");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));

    }

    private void DelPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-9]\d*$", "1"));
        if (info != "ok")
        {
            Master.Title = "清空排行榜";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            if (ptype == 0)
                builder.Append("确定清空所有排行榜吗");
            else
                builder.Append("确定清空" + sName[ptype - 1] + "排行榜吗");

            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("toplist.aspx?info=ok&amp;act=del&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定清空</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {

            if (ptype == 12)
                ptype = 24;


            //删除
            if (ptype == 0)
                new BCW.BLL.Toplist().Clear();
            else
                new BCW.BLL.Toplist().Clear(ptype);

            Utils.Success("清空排行榜", "清空排行榜成功..", Utils.getPage("default.aspx"), "1");
        }
    }
}
