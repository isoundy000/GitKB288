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

public partial class bbs_favorites : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string strText = string.Empty;
    protected string strName = string.Empty;
    protected string strType = string.Empty;
    protected string strValu = string.Empty;
    protected string strEmpt = string.Empty;
    protected string strIdea = string.Empty;
    protected string strOthe = string.Empty;
    protected int et = Convert.ToInt32(ub.Get("SiteExTime"));
    protected void Page_Load(object sender, EventArgs e)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "add":
            case "addin":
                AddPage(act, meid);
                break;
            case "addok":
                AddokPage(meid);
                break;
            case "save":
                SavePage(meid);
                break;
            case "edit":
                EditPage(meid);
                break;
            case "editsave":
                EditSavePage(meid);
                break;
            case "del":
                DelPage(meid);
                break;
            case "group":
                GroupPage(meid);
                break;
            case "addgroup":
                AddGroupPage(meid);
                break;
            case "groupsave":
                GroupSavePage(meid);
                break;
            case "editgroup":
                EditGroupPage(meid);
                break;
            case "editgroupsave":
                EditGroupSavePage(meid);
                break;
            case "delgroup":
                DelGroupPage(meid);
                break;
            case "list":
                ListPage(meid);
                break;
            case "view":
                ViewPage(meid);
                break;
            default:
                ReloadPage(meid);
                break;
        }
    }

    private void ReloadPage(int uid)
    {
        Master.Title = "我的收藏夹";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("favorites.aspx?act=list&amp;backurl=" + Utils.getPage(0) + "") + "\">全部收藏</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("favorites.aspx?act=add&amp;backurl=" + Utils.getPage(0) + "") + "\">新建收藏</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "UsID=" + uid + " and Types=0";

        // 开始读取列表
        IList<BCW.Model.Favgroup> listFavgroup = new BCW.BLL.Favgroup().GetFavgroups(pageIndex, pageSize, strWhere, out recordCount);
        if (listFavgroup.Count > 0)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("-|自建收藏夹");
            builder.Append(Out.Tab("</div>", "<br />"));
            int k = 1;
            foreach (BCW.Model.Favgroup n in listFavgroup)
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

                builder.Append("<a href=\"" + Utils.getUrl("favorites.aspx?act=list&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.Title + "(" + new BCW.BLL.Favorites().GetCount(uid, n.ID) + ")</a>");
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 1));

        }
        else
        {
            builder.Append(Out.Div("div", "还没有自建收藏夹.."));
        }
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("favorites.aspx?act=group&amp;backurl=" + Utils.PostPage(1) + "") + "\">管理收藏夹</a><br />");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("-|系统整理夹");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("favorites.aspx?act=list&amp;showtype=1&amp;backurl=" + Utils.PostPage(1) + "") + "\">帖子收藏(" + new BCW.BLL.Favorites().GetTypesCount(uid, 1) + ")</a>");
        builder.Append("<br /><a href=\"" + Utils.getUrl("favorites.aspx?act=list&amp;showtype=2&amp;backurl=" + Utils.PostPage(1) + "") + "\">相片收藏(" + new BCW.BLL.Favorites().GetTypesCount(uid, 2) + ")</a>");
        builder.Append("<br /><a href=\"" + Utils.getUrl("favorites.aspx?act=list&amp;showtype=3&amp;backurl=" + Utils.PostPage(1) + "") + "\">日志收藏(" + new BCW.BLL.Favorites().GetTypesCount(uid, 3) + ")</a>");
        builder.Append("<br /><a href=\"" + Utils.getUrl("favorites.aspx?act=list&amp;showtype=4&amp;backurl=" + Utils.PostPage(1) + "") + "\">资讯文章(" + new BCW.BLL.Favorites().GetTypesCount(uid, 4) + ")</a>");
        builder.Append("<br /><a href=\"" + Utils.getUrl("favorites.aspx?act=list&amp;showtype=5&amp;backurl=" + Utils.PostPage(1) + "") + "\">本地收藏(" + new BCW.BLL.Favorites().GetTypesCount(uid, 5) + ")</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx?uid=" + uid + "") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));

    }

    private void GroupPage(int uid)
    {
        Master.Title = "自建收藏夹管理";
        builder.Append(Out.Tab("<div class=\"title\">自建收藏夹管理</div>", ""));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("favorites.aspx?act=addgroup&amp;backurl=" + Utils.PostPage(1) + "") + "\">新建收藏夹</a>");
        builder.Append(Out.Tab("</div>", ""));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "UsID=" + uid + " and Types=0";

        // 开始读取列表
        IList<BCW.Model.Favgroup> listFavgroup = new BCW.BLL.Favgroup().GetFavgroups(pageIndex, pageSize, strWhere, out recordCount);
        if (listFavgroup.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Favgroup n in listFavgroup)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                    builder.Append(Out.Tab("<div>", "<br />"));

                builder.Append("<a href=\"" + Utils.getUrl("favorites.aspx?act=list&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.Title + "(" + new BCW.BLL.Favorites().GetCount(uid, n.ID) + ")</a>");
                builder.Append("<a href=\"" + Utils.getUrl("favorites.aspx?act=editgroup&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[编辑]</a>");
                builder.Append("<a href=\"" + Utils.getUrl("favorites.aspx?act=delgroup&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[删除]</a>");

                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 1));

        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx?uid=" + uid + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("favorites.aspx") + "\">收藏夹</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void AddGroupPage(int uid)
    {
        Master.Title = "新建收藏夹";

        builder.Append(Out.Tab("<div class=\"title\">新建收藏夹</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("请输入收藏夹名称:");
        builder.Append(Out.Tab("</div>", ""));
        strText = ",排序:/,,,";
        strName = "Title,Paixu,act,backurl";
        strType = "text,snum,hidden,hidden";
        strValu = "'0'groupsave'" + Utils.getPage(0) + "";
        strEmpt = "false,false,false,false";
        strIdea = "/";
        strOthe = "添加收藏夹,favorites.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx?uid=" + uid + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("favorites.aspx") + "\">收藏夹</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void GroupSavePage(int uid)
    {
        string Title = Utils.GetRequest("Title", "post", 2, @"^[A-Za-zＡ-Ｚ\d０-９\u4E00-\u9FA5]{1,10}$", "分组名称限1-10字，不能使用特殊字符");
        int Paixu = int.Parse(Utils.GetRequest("Paixu", "post", 2, @"^[0-9]\d*$", "排序必须是数字形式"));

        if (new BCW.BLL.Favgroup().ExistsTitle(uid, Title, 0))
        {
            Utils.Error("此收藏夹名称已存在", "");
        }
        BCW.Model.Favgroup model = new BCW.Model.Favgroup();
        model.Types = 0;
        model.Title = Title;
        model.UsID = uid;
        model.Paixu = Paixu;
        model.AddTime = DateTime.Now;
        new BCW.BLL.Favgroup().Add(model);

        Utils.Success("添加收藏夹", "添加收藏夹成功，正在返回..", Utils.getUrl("favorites.aspx?act=group&amp;backurl=" + Utils.getPage(0) + ""), "1");
    }

    private void EditGroupPage(int uid)
    {
        Master.Title = "编辑收藏夹";
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "收藏夹ID错误"));
        if (!new BCW.BLL.Favgroup().Exists(id, uid, 0))
        {
            Utils.Error("不存在的记录", "");
        }
        BCW.Model.Favgroup model = new BCW.BLL.Favgroup().GetFavgroup(id);
        builder.Append(Out.Tab("<div class=\"title\">编辑好友分组</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("请输入分组名称:");
        builder.Append(Out.Tab("</div>", ""));
        strText = ",排序:/,,,";
        strName = "Title,Paixu,id,act,backurl";
        strType = "text,snum,hidden,hidden,hidden";
        strValu = "" + model.Title + "'" + model.Paixu + "'" + id + "'editgroupsave'" + Utils.getPage(0) + "";
        strEmpt = "false,false,false,false,false";
        strIdea = "/";
        strOthe = "编辑收藏夹,favorites.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx?uid=" + uid + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("favorites.aspx") + "\">收藏夹</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void EditGroupSavePage(int uid)
    {
        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[0-9]\d*$", "ID错误"));
        string Title = Utils.GetRequest("Title", "post", 2, @"^[A-Za-zＡ-Ｚ\d０-９\u4E00-\u9FA5]{1,10}$", "收藏夹名称限1-10字，不能使用特殊字符");
        int Paixu = int.Parse(Utils.GetRequest("Paixu", "post", 2, @"^[0-9]\d*$", "排序必须是数字形式"));
        if (!new BCW.BLL.Favgroup().Exists(id, uid, 0))
        {
            Utils.Error("不存在的记录", "");
        }
        BCW.Model.Favgroup model = new BCW.Model.Favgroup();
        model.ID = id;
        model.Types = 0;
        model.Title = Title;
        model.UsID = uid;
        model.Paixu = Paixu;
        model.AddTime = DateTime.Now;
        new BCW.BLL.Favgroup().Update(model);

        Utils.Success("编辑收藏夹分组", "编辑收藏夹成功，正在返回..", Utils.getUrl("favorites.aspx?act=group&amp;backurl=" + Utils.getPage(0) + ""), "1");
    }

    private void DelGroupPage(int uid)
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Favgroup().Exists(id, uid, 0))
        {
            Utils.Error("不存在的记录", "");
        }
        string info = Utils.GetRequest("info", "get", 1, "", "");

        Master.Title = "删除收藏夹";

        if (info == "")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此收藏夹吗.删除将收藏内容一起删除.");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("favorites.aspx?act=delgroup&amp;info=ok1&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先转移内容</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("favorites.aspx?act=delgroup&amp;info=ok2&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");

            builder.Append("<a href=\"" + Utils.getPage("favorites.aspx?act=group") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            new BCW.BLL.Favgroup().Delete(id);
            if (info == "ok2")
                new BCW.BLL.Favorites().Delete(uid, id);

            Utils.Success("删除收藏夹", "删除收藏夹成功，正在返回..", Utils.getUrl("favorites.aspx?act=group&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
    }
    private void AddPage(string act, int uid)
    {
        Master.Title = "新建收藏";
        string strFavgroup = string.Empty;
        string Purl = "http://";
        string Title = string.Empty;
        int ptype = 0;
        if (act == "addin")
        {
            Purl = Out.UBB(Utils.removeUVe(Utils.getPage(1)));

            //自动识别收藏类型
            if (Purl.ToLower().Contains("albums.aspx?act=view"))
                ptype = 2;
            else if (Purl.ToLower().Contains("diary.aspx?act=view"))
                ptype = 3;
            else if (Purl.ToLower().Contains("detail.aspx"))
                ptype = 4;
            else
                ptype = 5;

            string Purls = "http://" + Utils.GetDomain() + "" + Purl + "";
            Title = Utils.GetSourceTextByUrl(Utils.getUrl(Purls).Replace("&amp;", "&"));
            Title = Utils.GetTitle(Title);
        }
        DataSet ds = new BCW.BLL.Favgroup().GetList("ID,Title", "UsID=" + uid + " and Types=0");
        if (ds == null || ds.Tables[0].Rows.Count == 0)
        {
            Utils.Success("新建收藏", "请先建收藏夹再新建收藏..", Utils.getUrl("favorites.aspx?act=addgroup&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        int NodeId = 0;
        NodeId = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            strFavgroup += "|" + ds.Tables[0].Rows[i]["ID"] + "|" + ds.Tables[0].Rows[i]["Title"] + "";
        }
        strFavgroup = Utils.Mid(strFavgroup, 1, strFavgroup.Length);

        builder.Append(Out.Tab("<div class=\"title\">新建收藏</div>", ""));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("标题:");
        builder.Append(Out.Tab("</div>", ""));
        strText = ",地址:/,选择收藏夹:/,,,,";
        strName = "Title,PUrl,NodeId,ptype,act,backurl";
        strType = "text,text,select,hidden,hidden,hidden";
        strValu = "" + Title + "'" + Purl + "'" + NodeId + "'" + ptype + "'save'" + Utils.getPage(0) + "";
        strEmpt = "false,false," + strFavgroup + ",false,false,false";
        strIdea = "/";
        strOthe = "确定收藏,favorites.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx?uid=" + uid + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("favorites.aspx") + "\">收藏夹</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void AddokPage(int uid)
    {
        Master.Title = "加入收藏";
        string strFavgroup = string.Empty;
        DataSet ds = new BCW.BLL.Favgroup().GetList("ID,Title", "UsID=" + uid + " and Types=0");
        if (ds == null || ds.Tables[0].Rows.Count == 0)
        {
            Utils.Success("加入收藏", "请先建收藏夹再加入收藏..", Utils.getUrl("favorites.aspx?act=addgroup&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        int NodeId = 0;
        NodeId = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            strFavgroup += "|" + ds.Tables[0].Rows[i]["ID"] + "|" + ds.Tables[0].Rows[i]["Title"] + "";
        }
        strFavgroup = Utils.Mid(strFavgroup, 1, strFavgroup.Length);
        builder.Append(Out.Tab("<div class=\"title\">加入收藏</div>", ""));

        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 2, @"^[1-4]$", "收藏类型选择错误"));
        int forumid = int.Parse(Utils.GetRequest("forumid", "get", 1, @"^[1-9]\d*$", "0"));
        int bid = int.Parse(Utils.GetRequest("bid", "get", 1, @"^[1-9]\d*$", "0"));
        string Title = string.Empty;
        string PUrl = string.Empty;
        //加入帖子收藏
        builder.Append(Out.Tab("<div>", ""));
        if (ptype == 1)
        {
            Title = new BCW.BLL.Text().GetTitle(bid);
            if (Title == "")
                Utils.Error("不存在的记录", "");
            PUrl = "/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "";
            builder.Append("主题:<a href=\"" + Utils.getUrl(PUrl) + "\">" + Title + "</a>");
        }
        builder.Append(Out.Tab("</div>", ""));
        strText = ",,选择收藏夹:/,,,";
        strName = "Title,PUrl,NodeId,ptype,act,backurl";
        strType = "hidden,hidden,select,hidden,hidden,hidden";
        strValu = "" + Title + "'" + PUrl + "'" + NodeId + "'" + ptype + "'save'" + Utils.getPage(0) + "";
        strEmpt = "false,false," + strFavgroup + ",false,false,false";
        strIdea = "/";
        strOthe = "确定收藏,favorites.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx?uid=" + uid + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("favorites.aspx") + "\">收藏夹</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void SavePage(int uid)
    {
        string PUrl = string.Empty;
        int ptype = int.Parse(Utils.GetRequest("ptype", "post", 1, @"^[1-5]$", "0"));
        string Title = Utils.GetRequest("Title", "post", 2, @"^[^\^]{1,30}$", "标题限1-30字");
        if (ptype == 0)
            PUrl = Utils.GetRequest("PUrl", "post", 2, @"http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", "请输入正确的地址");
        else
            PUrl = Utils.GetRequest("PUrl", "post", 2, @"(/[\w- ./?%&=]*)?", "收藏地址错误");

        int NodeId = int.Parse(Utils.GetRequest("NodeId", "post", 2, @"^[0-9]\d*$", "收藏夹ID错误"));
        if (!new BCW.BLL.Favgroup().Exists(NodeId, uid, 0))
        {
            Utils.Error("不存在的收藏夹", "");
        }
        if (new BCW.BLL.Favorites().ExistsTitle(uid, Title, PUrl))
        {
            Utils.Error("重复收藏", "");
        }
        BCW.Model.Favorites model = new BCW.Model.Favorites();
        model.Types = ptype;
        model.NodeId = NodeId;
        model.UsID = uid;
        model.Title = Title;
        model.PUrl = PUrl;
        model.AddTime = DateTime.Now;
        int favid = new BCW.BLL.Favorites().Add(model);

        Utils.Success("新建收藏", "收藏成功,正在返回..<br /><a href=\"" + Utils.getUrl("favorites.aspx?act=view&amp;id=" + NodeId + "&amp;favid=" + favid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;查看收藏</a>", Utils.getPage("favorites.aspx"), "3");
    }

    private void ListPage(int uid)
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-9]\d*$", "0"));
        int showtype = int.Parse(Utils.GetRequest("showtype", "get", 1, @"^[1-5]\d*$", "0"));
        int id = int.Parse(Utils.GetRequest("id", "get", 1, @"^[0-9]\d*$", "0"));
        string sText = string.Empty;
        if (id != 0)
        {
            sText = new BCW.BLL.Favgroup().GetTitle(id, uid, 0);
            if (sText == "")
            {
                Utils.Error("不存在的记录", "");
            }
        }
        else
        {
            sText = "我的全部收藏";
        }
        sText = "我的" + BCW.User.AppCase.CaseFav(showtype) + "收藏";

        Master.Title = sText;
        builder.Append(Out.Tab("<div class=\"title\">" + sText + "</div>", ""));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "ptype", "showtype", "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        if (id != 0)
            strWhere = "UsID=" + uid + " and NodeId=" + id + "";
        else
        {
            if (showtype != 0)
                strWhere = "UsID=" + uid + " and Types=" + showtype + "";
            else
                strWhere = "UsID=" + uid + "";
        }
        // 开始读取列表
        IList<BCW.Model.Favorites> listFavorites = new BCW.BLL.Favorites().GetFavoritess(pageIndex, pageSize, strWhere, out recordCount);
        if (listFavorites.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Favorites n in listFavorites)
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

                builder.Append("<a href=\"" + Utils.getUrl("favorites.aspx?act=view&amp;id=" + n.NodeId + "&amp;favid=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + ((pageIndex - 1) * pageSize + k) + "." + n.Title + "</a>");
                if (ptype == 1)
                {
                    builder.Append("<a href=\"" + Utils.getUrl("favorites.aspx?act=del&amp;id=" + n.NodeId + "&amp;favid=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[删除]</a>");
                }

                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 1));

        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div>", "<br />"));
        if (ptype == 0)
            builder.Append("<a href=\"" + Utils.getUrl("favorites.aspx?act=list&amp;ptype=1&amp;showtype=" + showtype + "&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;切换管理</a>");
        else
            builder.Append("<a href=\"" + Utils.getUrl("favorites.aspx?act=list&amp;ptype=0&amp;showtype=" + showtype + "&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;切换普通</a>");

        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx?uid=" + uid + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("favorites.aspx") + "\">收藏夹</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ViewPage(int uid)
    {
        int favid = int.Parse(Utils.GetRequest("favid", "get", 2, @"^[1-9]\d*$", "ID错误"));
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Favorites().Exists(favid, id, uid))
        {
            Utils.Error("不存在的收藏记录", "");
        }
        BCW.Model.Favorites model = new BCW.BLL.Favorites().GetFavorites(favid);
        Master.Title = "查看收藏";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("" + BCW.User.AppCase.CaseFav(model.Types) + ":" + model.Title + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        if (model.Types == 0)
        {
            builder.Append("地址:" + model.PUrl + "<br />");
            builder.Append("<a href=\"gourl.aspx?tt=" + model.PUrl + "&amp;ve=" + Utils.getstrVe() + "\">&gt;马上访问</a><br />");
        }
        else
            builder.Append("<a href=\"" + Utils.getUrl(model.PUrl + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">&gt;进入查看</a><br />");

        builder.Append("<a href=\"" + Utils.getUrl("favorites.aspx?act=edit&amp;id=" + id + "&amp;favid=" + favid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">编辑</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("favorites.aspx?act=del&amp;id=" + id + "&amp;favid=" + favid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">删除</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("favorites.aspx?act=list&amp;id=" + id + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("favorites.aspx") + "\">收藏夹</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void EditPage(int uid)
    {
        Master.Title = "编辑收藏";
        int favid = int.Parse(Utils.GetRequest("favid", "get", 2, @"^[1-9]\d*$", "ID错误"));
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Favorites().Exists(favid, id, uid))
        {
            Utils.Error("不存在的收藏记录", "");
        }
        BCW.Model.Favorites model = new BCW.BLL.Favorites().GetFavorites(favid);
        string sType = "text";
        if (model.Types > 0)
        {
            sType = "hidden";
        }
        string strFavgroup = string.Empty;
        DataSet ds = new BCW.BLL.Favgroup().GetList("ID,Title", "UsID=" + uid + " and Types=0");
        if (ds != null && ds.Tables[0].Rows.Count != 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                strFavgroup += "|" + ds.Tables[0].Rows[i]["ID"] + "|" + ds.Tables[0].Rows[i]["Title"] + "";
            }
        }
        strFavgroup = Utils.Mid(strFavgroup, 1, strFavgroup.Length);
        builder.Append(Out.Tab("<div class=\"title\">编辑收藏</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("标题:");
        builder.Append(Out.Tab("</div>", ""));

        strText = ",地址:/,请选择收藏夹:/,,,,";
        strName = "Title,PUrl,NodeId,favid,ptype,act,backurl";
        strType = "text," + sType + ",select,hidden,hidden,hidden,hidden";
        strValu = "" + model.Title + "'" + model.PUrl + "'" + model.NodeId + "'" + favid + "'" + model.Types + "'editsave'" + Utils.getPage(0) + "";
        strEmpt = "false,false," + strFavgroup + ",false,false,false,false";
        strIdea = "/";
        strOthe = "编辑收藏,favorites.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append(" <a href=\"" + Utils.getUrl("favorites.aspx?act=view&amp;&amp;id=" + id + "&amp;favid=" + favid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">取消</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void EditSavePage(int uid)
    {
        string PUrl = string.Empty;
        int ptype = int.Parse(Utils.GetRequest("ptype", "post", 1, @"^[1-5]$", "0"));
        string Title = Utils.GetRequest("Title", "post", 2, @"^[^\^]{1,30}$", "标题限1-30字");
        if (ptype == 0)
            PUrl = Utils.GetRequest("Purl", "post", 2, @"http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", "请输入正确的地址");
        else
            PUrl = Utils.GetRequest("Purl", "post", 2, @"(/[\w- ./?%&=]*)?", "帖子地址错误");
        int favid = int.Parse(Utils.GetRequest("favid", "post", 2, @"^[1-9]\d*$", "收藏ID错误"));
        int NodeId = int.Parse(Utils.GetRequest("NodeId", "post", 2, @"^[0-9]\d*$", "收藏夹ID错误"));
        if (!new BCW.BLL.Favorites().Exists(favid, uid))
        {
            Utils.Error("不存在的收藏", "");
        }
        if (!new BCW.BLL.Favgroup().Exists(NodeId, uid, 0))
        {
            Utils.Error("不存在的收藏夹", "");
        }

        BCW.Model.Favorites model = new BCW.Model.Favorites();
        model.ID = favid;
        model.NodeId = NodeId;
        model.UsID = uid;
        model.Title = Title;
        model.PUrl = PUrl;
        model.AddTime = DateTime.Now;
        new BCW.BLL.Favorites().Update(model);

        Utils.Success("编辑收藏", "编辑收藏成功,正在返回..<br /><a href=\"" + Utils.getUrl("favorites.aspx?act=view&amp;id=" + NodeId + "&amp;favid=" + favid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;查看收藏</a>", Utils.getPage("favorites.aspx"), "3");
    }

    private void DelPage(int uid)
    {
        int favid = int.Parse(Utils.GetRequest("favid", "get", 2, @"^[1-9]\d*$", "ID错误"));
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Favorites().Exists(favid, id, uid))
        {
            Utils.Error("不存在的收藏记录", "");
        }
        string info = Utils.GetRequest("info", "get", 1, "", "");

        Master.Title = "删除收藏";

        if (info == "")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此收藏吗.");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("favorites.aspx?act=del&amp;info=ok&amp;id=" + id + "&amp;favid=" + favid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");

            builder.Append("<a href=\"" + Utils.getUrl("favorites.aspx?act=view&amp;id=" + id + "&amp;favid=" + favid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("favorites.aspx") + "\">&gt;返回上一级</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            new BCW.BLL.Favorites().Delete(favid);

            Utils.Success("删除收藏", "删除收藏成功，正在返回..", Utils.getPage("favorites.aspx"), "1");
        }
    }
}