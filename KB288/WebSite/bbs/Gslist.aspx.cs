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

public partial class bbs_Gslist : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "参赛记录";
        int forumid = int.Parse(Utils.GetRequest("forumid", "all", 2, @"^[0-9]\d*$", "论坛ID错误"));
        int bid = int.Parse(Utils.GetRequest("bid", "all", 2, @"^[0-9]\d*$", "帖子ID错误"));
        if (!new BCW.BLL.Forum().Exists2(forumid))
        {
            Utils.Success("访问论坛", "该论坛不存在或已暂停使用", Utils.getUrl("forum.aspx"), "1");
        }
        if (!new BCW.BLL.Text().Exists2(bid, forumid))
        {
            Utils.Error("帖子不存在或已被删除", "");
        }
        if (new BCW.User.ForumInc().IsForumGSIDS(forumid) == true)
        { }
        else
        {
            Utils.Error("不存在的记录", "");
        }
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "save":
                SavePage(forumid, bid);
                break;
            default:
                ReloadPage(forumid, bid);
                break;
        }
    }

    private void SavePage(int forumid, int bid)
    {

    }

    private void ReloadPage(int forumid, int bid)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int hid = int.Parse(Utils.GetRequest("hid", "all", 1, @"^[0-9]\d*$", "0"));
        if (hid == 0)
            hid = meid;

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("查看参赛记录");
        builder.Append(Out.Tab("</div>", "<br />"));
        BCW.Model.Text model = new BCW.BLL.Text().GetText(bid);

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("<b>记录:连中" + model.Glznum + "期,月中" + model.Gmnum + "期,历史" + model.Gaddnum + "中" + model.Gwinnum + "</b>");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = 10;
        string strWhere = string.Empty;
        string[] pageValUrl = { "forumid", "bid", "hid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        strWhere = "Forumid=" + forumid + " and BID=" + bid + " and UsID=" + hid + "";
        // 开始读取列表
        IList<BCW.Model.Forumvote> listForumvote = new BCW.BLL.Forumvote().GetForumvotes(pageIndex, pageSize, strWhere, out recordCount);
        if (listForumvote.Count > 0)
        {

            int k = 1;
            foreach (BCW.Model.Forumvote n in listForumvote)
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
                builder.Append("[" + n.qiNum + "]期:" + n.Notes + "");
                if (n.state == 0)
                {
                    builder.Append("[开?]");
                }
                else
                {
                    if (n.IsWin == 0)
                        builder.Append("[未中]");
                    else
                        builder.Append("[中]");

                }
                
                k++;
                builder.Append(Out.Tab("</div>", ""));

            }

            // 分页
            builder.Append(BasePage.ForumMultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "") + "\">&lt;&lt;返回主題</a>");
        builder.Append(Out.Tab("</div>", ""));

    }
}
