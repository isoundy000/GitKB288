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

public partial class search : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        //==============仿止刷新开始===========
        string u = Utils.GetRequest("u", "all", 1, "", "");
        if (u != "")
        {
            object CacheU = DataCache.GetCache("CacheU");
            if (CacheU != null)
            {
                if (u.Equals(CacheU.ToString()))
                {
                    //Response.Redirect(Utils.getUrl("/search.aspx").Replace("&amp;", "&"));
                    //Response.End();
                }
            }
            DataCache.SetCache("CacheU", u, DateTime.Now.AddSeconds(10), TimeSpan.Zero);
        }
        //==============仿止刷新结束===========

        string act = Utils.GetRequest("act", "all", 1, "", "");
        string pt = Utils.GetRequest("pt", "all", 1, @"^[0-9]\d*_(.+?)$", "");
        int ptype = -1;
        string NodeId = string.Empty;
        if (pt != "")
        {
            string[] pTemp = pt.Split("_".ToCharArray());
            ptype = Convert.ToInt32(pTemp[0]);
            NodeId = pTemp[1].ToString();
        }
        if (act == "forum")
            ptype = 10;

        switch (ptype)
        {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
                DetailPage(ptype, NodeId);
                break;
            case 5:
            case 6:
                UserPage(ptype);
                break;
            case 7:
            case 8:
            case 9:
                MorePage(ptype);
                break;
            case 10:
                ForumPage();
                break;
            default:
                ReloadPage();
                break;
        }

    }

    private void ReloadPage()
    {
        Master.Title = "搜索中心";
        builder.Append(Out.Tab("<div class=\"title\">搜索中心</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("搜索关键字：<br />");

        builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB("[form=/search.aspx_post][input=keyword_8]关键字[/input][select=pt][option=0_0]资讯[/option][option=1_0]文件[/option][option=2_0]图片[/option][option=3_0]商品[/option][option=4_0]帖子[/option][option=5_0]会员[/option][option=6_0]城市[/option][option=7_0]圈子[/option][option=8_0]论坛[/option][option=9_0]聊室[/option][/select]#[postfield=keyword]$(keyword)[/postfield][postfield=pt]$(pt)[/postfield]#搜索#red[/form]")));
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ForumPage()
    {
        int forumid = int.Parse(Utils.GetRequest("forumid", "get", 1, @"^[0-9]\d*$", "0"));
        Master.Title = "论坛搜索中心";
        builder.Append(Out.Tab("<div class=\"title\">论坛搜索中心</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        if (forumid > 0)
        {
            if (!new BCW.BLL.Forum().Exists2(forumid))
            {
                Utils.Success("访问论坛", "该论坛不存在或已暂停使用", Utils.getUrl("/bbs/forum.aspx"), "1");
            }
        }
        builder.Append("搜索关键字：<br />");
        builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB("[form=/search.aspx_post][input=keyword_8]关键字[/input][select=pt][option=4_0]帖子[/option][option=5_0]会员[/option][option=6_0]城市[/option][option=8_0]论坛[/option][/select]#[postfield=keyword]$(keyword)[/postfield][postfield=pt]$(pt)[/postfield]#搜索#red[/form]")));
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        if (forumid == 0)
            builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">返回上级</a>");
        else
            builder.Append("<a href=\"" + Utils.getPage("/bbs/forum.aspx?forumid=" + forumid + "") + "\">返回" + new BCW.BLL.Forum().GetTitle(forumid) + "</a>");

        builder.Append(Out.Tab("</div>", ""));
    }

    private void DetailPage(int ptype, string NodeId)
    {
        Master.Title = "资源搜索";
        string keyword = Utils.GetRequest("keyword", "all", 2, @"^[\s\S]{1,102}$", "请输入1-10字的搜索关键字");
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("搜索:'" + keyword + "'");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "pt", "keyword", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        if (ptype == 0)
            strWhere = "Types=11 and (Title like '%" + keyword + "%' OR '#'+KeyWord+'#' like '%#" + keyword + "#%')";
        else if (ptype == 1)
            strWhere = "Types=13 and (Title like '%" + keyword + "%' OR '#'+KeyWord+'#' like '%#" + keyword + "#%')";
        else if (ptype == 2)
            strWhere = "Types=12 and (Title like '%" + keyword + "%' OR '#'+KeyWord+'#' like '%#" + keyword + "#%')";
        else if (ptype == 3)
            strWhere = "(Title like '%" + keyword + "%' OR '#'+KeyWord+'#' like '%#" + keyword + "#%')";

        if (NodeId != "0" && NodeId.Contains(";"))
        {
            NodeId = NodeId.Replace(";", ",");
            strWhere += " and NodeId in(" + NodeId + ")";
        }

        // 开始读取列表
        if (ptype < 3)
        {
            IList<BCW.Model.Detail> listDetail = new BCW.BLL.Detail().GetDetails(pageIndex, pageSize, strWhere, out recordCount);
            if (listDetail.Count > 0)
            {
                int k = 1;
                foreach (BCW.Model.Detail n in listDetail)
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
                    builder.AppendFormat("<a href=\"" + Utils.getUrl("detail.aspx?id={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{1}.{2}</a>", n.ID, (pageIndex - 1) * pageSize + k, n.Title);

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
        }
        else if (ptype == 3)
        {
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
        }
        else if (ptype == 4)
        {
            strWhere += "Title like '%" + keyword + "%'";
            string strOrder = "ID Desc";
            // 开始读取列表
            IList<BCW.Model.Text> listText = new BCW.BLL.Text().GetTexts(pageIndex, pageSize, strWhere, strOrder, out recordCount);
            if (listText.Count > 0)
            {
                int k = 1;
                foreach (BCW.Model.Text n in listText)
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
                    builder.AppendFormat("<a href=\"" + Utils.getUrl("/bbs/topic.aspx?forumid=" + n.ForumId + "&amp;bid={0}&amp;backurl=" + Utils.getPage(0) + "") + "\">{1}.{2}</a>", n.ID, (pageIndex - 1) * pageSize + k, n.Title);

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
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("search.aspx").Replace("&amp;?","?") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void UserPage(int ptype)
    {
        Master.Title = "会员搜索";
        string keyword = Utils.GetRequest("keyword", "all", 2, @"^[\s\S]{1,10}$", "请输入1-10字的搜索关键字");
        if (Utils.IsRegex(keyword, @"^[1-9]\d*$"))
        {
            Utils.Success("会员搜索", "正在查找会员..", Utils.getUrl("/bbs/uinfo.aspx?uid=" + keyword + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("搜索:'" + keyword + "'结果：");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "pt", "keyword", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        if (ptype == 6)
            strWhere = "City like '%" + keyword + "%'";
        else
            strWhere = "UsName like '%" + keyword + "%'";

        // 开始读取列表
        IList<BCW.Model.User> listUser = new BCW.BLL.User().GetUsers(pageIndex, pageSize, strWhere, out recordCount);
        if (listUser.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.User n in listUser)
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
                builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.ID + "") + "\">" + n.UsName + "(" + n.ID + ")</a>");

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
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("search.aspx") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void MorePage(int ptype)
    {
        string sText = string.Empty;
        if (ptype == 7)
            sText = "圈子";
        else if (ptype == 8)
            sText = "论坛";
        else
            sText = "聊室";

        Master.Title = "搜索" + sText + "";

        string keyword = Utils.GetRequest("keyword", "all", 2, @"^[\s\S]{1,10}$", "请输入1-10字的搜索关键字");
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("搜索:'" + keyword + "'结果：");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        if (ptype == 7)
        {
            DataSet ds = new BCW.BLL.Group().GetList("ID,Title", "Status=0 and Title like '%" + keyword + "%'");
            if (ds != null && ds.Tables[0].Rows.Count != 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    int id = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
                    string Title = ds.Tables[0].Rows[0]["Title"].ToString();
                    builder.Append("<a href=\"" + Utils.getUrl("/bbs/group.aspx?act=view&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + Title + "</a><br />");
                }
            }
            else
            {
                builder.Append("没有相关记录..<br />");
            }
        }
        else if (ptype == 8)
        {
            DataSet ds = new BCW.BLL.Forum().GetList("ID,Title", "IsActive=0 and Title like '%" + keyword + "%'");
            if (ds != null && ds.Tables[0].Rows.Count != 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    int id = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
                    string Title = ds.Tables[0].Rows[0]["Title"].ToString();
                    builder.Append("<a href=\"" + Utils.getUrl("/bbs/forum.aspx?forumid=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + Title + "</a><br />");
                }
            }
            else
            {
                builder.Append("没有相关记录..<br />");
            }
        }
        else if (ptype == 9)
        {
            DataSet ds = new BCW.BLL.Chat().GetList("ID,ChatName", "(ExTime='1990-1-1' OR ExTime>'" + DateTime.Now + "') and ChatName like '%" + keyword + "%'");
            if (ds != null && ds.Tables[0].Rows.Count != 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    int id = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
                    string Title = ds.Tables[0].Rows[0]["ChatName"].ToString();
                    builder.Append("<a href=\"" + Utils.getUrl("/bbs/chatroom.aspx?id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + Title + "</a><br />");
                }
            }
            else
            {
                builder.Append("没有相关记录..<br />");
            }
        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.RHr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("search.aspx") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
}