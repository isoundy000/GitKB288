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
public partial class shop : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/front.xml";
    protected void Page_Load(object sender, EventArgs e)
    {
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[0-9]\d*$", "0"));
        if (!new BCW.BLL.Topics().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        BCW.Model.Topics model = new BCW.BLL.Topics().GetTopics(id);
        Master.Title = model.Title;

        //顶部调用
        string TopUbb = string.Empty;
        TopUbb = ub.GetSub("FtShopListTop", xmlPath);

        if (TopUbb != "")
        {
            TopUbb = BCW.User.AdminCall.AdminUBB(Out.SysUBB(TopUbb));
            if (TopUbb.IndexOf("</div>") == -1)
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append(TopUbb);
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                builder.Append(TopUbb);
            }
        }

        builder.Append(Out.Tab("<div class=\"title\">" + model.Title + "</div>", ""));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "id" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "NodeId=" + id + "";

        // 开始读取列表
        IList<BCW.Model.Goods> listGoods = new BCW.BLL.Goods().GetGoodss(pageIndex, pageSize, strWhere, out recordCount);
        if (listGoods.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Goods n in listGoods)
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
                builder.AppendFormat("<a href=\"" + Utils.getUrl("shopdetail.aspx?id={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}.{3}</a>", n.ID, pageIndex, (pageIndex - 1) * pageSize + k, n.Title);
                if (!string.IsNullOrEmpty(n.Cover) && IsCover() == true)
                    builder.Append("<br /><img src=\"" + n.Cover + "\" alt=\"load\"/>");

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

        //底部调用
        string FootUbb = string.Empty;
        FootUbb = ub.GetSub("FtShopListFoot", xmlPath);

        if (FootUbb != "")
        {
            FootUbb = BCW.User.AdminCall.AdminUBB(Out.SysUBB(FootUbb));
            if (FootUbb.IndexOf("</div>") == -1)
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append(FootUbb);
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                builder.Append(FootUbb);
            }
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">上级</a>");
        if (model.NodeId != 0)
        {
            builder.Append("-<a href=\"" + Utils.getUrl("default.aspx?id=" + model.NodeId + "") + "\">" + new BCW.BLL.Topics().GetTitle(model.NodeId) + "</a>");
        }
        builder.Append(Out.Tab("</div>", ""));
    }

    private bool IsCover()
    {
        bool bl = false;
        if (ub.GetSub("FtShopCover", xmlPath) == "0")
        {
            bl = true;
        }
        return bl;
    }
}
