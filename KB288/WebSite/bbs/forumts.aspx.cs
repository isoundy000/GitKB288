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

public partial class bbs_forumts : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string strText = string.Empty;
    protected string strName = string.Empty;
    protected string strType = string.Empty;
    protected string strValu = string.Empty;
    protected string strEmpt = string.Empty;
    protected string strIdea = string.Empty;
    protected string strOthe = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int forumid = int.Parse(Utils.GetRequest("forumid", "all", 1, @"^[1-9]\d*$", "0"));
        string ForumName = new BCW.BLL.Forum().GetTitle(forumid);
        if (ForumName == "")
            Utils.Error("不存在的论坛或此论坛已暂停使用", "");

        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "view":
                ViewPage(forumid, ForumName, meid);
                break;
            case "add":
                AddPage(forumid, ForumName, meid);
                break;
            case "edit":
                EditPage(forumid, ForumName, meid);
                break;
            case "save":
                SavePage(forumid, ForumName, meid);
                break;
            case "move":
                MovePage(forumid, ForumName, meid);
                break;
            case "del":
                DelPage(forumid, ForumName, meid);
                break;
            default:
                ReloadPage(forumid, ForumName, meid);
                break;
        }
    }

    private void ReloadPage(int forumid, string ForumName, int uid)
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-1]$", "0"));
        bool flag = new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_TopicsSet, uid, forumid);
        if (flag == false && ptype == 1)
        {
            Utils.Error("你的权限不足", "");
        }
        Master.Title = "[" + ForumName + "]专题";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "") + "\">" + ForumName + "</a>&gt;专题列表");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "forumid", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "ForumID=" + forumid + "";
        // 开始读取列表
        IList<BCW.Model.Forumts> listForumts = new BCW.BLL.Forumts().GetForumtss(pageIndex, pageSize, strWhere, out recordCount);
        if (listForumts.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Forumts n in listForumts)
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
                builder.Append("<a href=\"" + Utils.getUrl("forumts.aspx?act=view&amp;forumid=" + forumid + "&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + ((pageIndex - 1) * pageSize + k) + "." + n.Title + "</a>");
                if (ptype == 1)
                {
                    builder.Append("(专题ID:" + n.ID + ")<br /><a href=\"" + Utils.getUrl("forumts.aspx?act=edit&amp;forumid=" + forumid + "&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">编辑</a>.");
                    builder.Append("<a href=\"" + Utils.getUrl("forumts.aspx?act=move&amp;forumid=" + forumid + "&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">转移内容</a>.");
                    builder.Append("<a href=\"" + Utils.getUrl("forumts.aspx?act=del&amp;forumid=" + forumid + "&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">删除</a>");
                }
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 1));
            builder.Append(Out.Tab("", "<br />"));
            strText = ",,,,,";
            strName = "keyword,forumid,act,backurl";
            strType = "stext,hidden,hidden,hidden";
            strValu = "'" + forumid + "'forumsave'" + Utils.PostPage(1) + "";
            strEmpt = "false,false,false,false";
            strIdea = "";
            strOthe = "搜专题帖,/search.aspx,post,3,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        if (flag == true)
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            if (ptype == 0)
                builder.Append("<a href=\"" + Utils.getUrl("forumts.aspx?ptype=1&amp;forumid=" + forumid + "") + "\">&gt;切换管理模式</a>");
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("forumts.aspx?act=add&amp;forumid=" + forumid + "") + "\">添加新专题</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("forumts.aspx?forumid=" + forumid + "") + "\">&gt;切换普通模式</a>");
            }
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "") + "\">" + ForumName + "</a>");
        builder.Append(Out.Tab("</div>", ""));

    }

    private void ViewPage(int forumid, string ForumName, int uid)
    {
        Master.Title = "查看专题";
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        string ForumtsName = new BCW.BLL.Forumts().GetTitle(id, forumid);
        if (ForumtsName == "")
            Utils.Error("不存在的专题记录", "");

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("forumts.aspx?forumid=" + forumid + "") + "\">专题</a>&gt;" + ForumtsName + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        string strOrder = string.Empty;
        string[] pageValUrl = { "act", "forumid", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        strWhere += "ForumId=" + forumid + " and TsID=" + id + " and IsDel=0";

        //排序条件
        strOrder = "ID Desc";
        // 开始读取列表
        IList<BCW.Model.Text> listText = new BCW.BLL.Text().GetTextsMe(pageIndex, pageSize, strWhere, strOrder, out recordCount);
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

                builder.AppendFormat("<a href=\"" + Utils.getUrl("topic.aspx?forumid=" + forumid + "&amp;bid={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{1}.{2}</a>", n.ID, (pageIndex - 1) * pageSize + k, n.Title);

                k++;
                builder.Append(Out.Tab("</div>", ""));
            }

            // 分页
            builder.Append(BasePage.ForumMultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            builder.Append(Out.Tab("", "<br />"));
            strText = ",,,,,";
            strName = "keyword,forumid,act,backurl";
            strType = "stext,hidden,hidden,hidden";
            strValu = "'" + forumid + "'forumsave'" + Utils.PostPage(1) + "";
            strEmpt = "false,false,false,false";
            strIdea = "";
            strOthe = "搜专题帖,/search.aspx,post,3,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("forumts.aspx?forumid=" + forumid + "") + "\">专题</a>");
        builder.Append(Out.Tab("</div>", ""));

    }


    private void AddPage(int forumid, string ForumName, int uid)
    {
        new BCW.User.Role().CheckUserRole(BCW.User.Role.enumRole.Role_TopicsSet, uid, forumid);
        Master.Title = "添加专题";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("添加新的专题");
        builder.Append(Out.Tab("</div>", ""));

        strText = "专题名称:/,排序:(数字超大越靠前)/,,";
        strName = "Title,Paixu,forumid,act";
        strType = "text,snum,hidden,hidden";
        strValu = "'0'" + forumid + "'save";
        strEmpt = "false,false,false,false";
        strIdea = "/";
        strOthe = "确定添加,forumts.aspx,post,1,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("forumts.aspx?ptype=1&amp;forumid=" + forumid + "") + "\">专题</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void EditPage(int forumid, string ForumName, int uid)
    {
        new BCW.User.Role().CheckUserRole(BCW.User.Role.enumRole.Role_TopicsSet, uid, forumid);
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Forumts().Exists2(id, forumid))
        {
            Utils.Error("不存在的专题记录", "");
        }
        BCW.Model.Forumts model = new BCW.BLL.Forumts().GetForumts(id);
        Master.Title = "编辑专题";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("编辑专题");
        builder.Append(Out.Tab("</div>", ""));

        strText = "专题名称:/,排序:(数字超大越靠前)/,,,,";
        strName = "Title,Paixu,forumid,id,act,backurl";
        strType = "text,snum,hidden,hidden,hidden,hidden";
        strValu = "" + model.Title + "'" + model.Paixu + "'" + forumid + "'" + id + "'save'" + Utils.getPage(0) + "";
        strEmpt = "false,false,false,false,false";
        strIdea = "/";
        strOthe = "确定编辑,forumts.aspx,post,1,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("forumts.aspx?ptype=1&amp;forumid=" + forumid + "") + "\">专题</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void MovePage(int forumid, string ForumName, int uid)
    {
        new BCW.User.Role().CheckUserRole(BCW.User.Role.enumRole.Role_TopicsSet, uid, forumid);
        Master.Title = "转移专题内容";
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Forumts().Exists2(id, forumid))
        {
            Utils.Error("不存在的专题记录", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {
            int nid = int.Parse(Utils.GetRequest("nid", "post", 2, @"^[0-9]\d*$", "ID错误"));
            if (!new BCW.BLL.Forumts().Exists2(nid, forumid))
            {
                Utils.Error("不存在的专题记录", "");
            }
            new BCW.BLL.Text().UpdateTsID2(id, nid);
            Utils.Success("转移专题内容", "恭喜，转移专题内容成功", Utils.getPage("forumts.aspx?ptype=1&amp;forumid=" + forumid + ""), "1");
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("转移专题内容");
            builder.Append(Out.Tab("</div>", ""));

            strText = "请输入目标专题ID:/,,,,,";
            strName = "nid,id,forumid,act,info,backurl";
            strType = "num,hidden,hidden,hidden,hidden,hidden";
            strValu = "'" + id + "'" + forumid + "'move'ok'" + Utils.getPage(0) + "";
            strEmpt = "false,false,false,false,false,false";
            strIdea = "/注意:确认转移后,该专题下所有内容都将转移到目标专题里./";
            strOthe = "确定转移,forumts.aspx,post,1,red";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "") + "\">上级</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("forumts.aspx?ptype=1&amp;forumid=" + forumid + "") + "\">专题</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private void SavePage(int forumid, string ForumName, int uid)
    {
        new BCW.User.Role().CheckUserRole(BCW.User.Role.enumRole.Role_TopicsSet, uid, forumid);
        string Title = Utils.GetRequest("Title", "post", 2, @"^[^\^]{1,10}$", "专题名称限1-10字");
        int Paixu = int.Parse(Utils.GetRequest("Paixu", "post", 2, @"^[0-9]\d*$", "排序填写错误"));
        int id = int.Parse(Utils.GetRequest("id", "post", 1, @"^[0-9]\d*$", "0"));
        if (id == 0)
        {
            if (new BCW.BLL.Forumts().Exists(forumid, Title))
            {
                Utils.Error("同名专题名称已存在，请填写其它专题名称", "");
            }
        }
        else
        {
            if (!new BCW.BLL.Forumts().Exists2(id, forumid))
            {
                Utils.Error("不存在的专题记录", "");
            }
        }
        BCW.Model.Forumts model = new BCW.Model.Forumts();
        model.ID = id;
        model.ForumID = forumid;
        model.Title = Title;
        model.Paixu = Paixu;
        if (id == 0)
        {
            new BCW.BLL.Forumts().Add(model);
            Utils.Success("添加专题", "恭喜，添加专题成功", Utils.getUrl("forumts.aspx?ptype=1&amp;forumid=" + forumid + ""), "1");
        }
        else
        {
            new BCW.BLL.Forumts().Update(model);
            Utils.Success("编辑专题", "恭喜，编辑专题成功", Utils.getPage("forumts.aspx?ptype=1&amp;forumid=" + forumid + ""), "1");
        }
    }

    private void DelPage(int forumid, string ForumName, int uid)
    {
        new BCW.User.Role().CheckUserRole(BCW.User.Role.enumRole.Role_TopicsSet, uid, forumid);
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        if (info != "ok")
        {
            Master.Title = "删除专题";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此专题记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("forumts.aspx?info=ok&amp;act=del&amp;forumid=" + forumid + "&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("forumts.aspx?ptype=1&amp;forumid=" + forumid + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            if (!new BCW.BLL.Forumts().Exists2(id, forumid))
            {
                Utils.Error("不存在的记录", "");
            }
            //删除
            new BCW.BLL.Forumts().Delete(id);
            Utils.Success("删除专题", "删除专题成功..", Utils.getPage("forumts.aspx?ptype=1&amp;forumid=" + forumid + ""), "1");
        }
    }
}
