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

public partial class Manage_forum : System.Web.UI.Page
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
        Master.Title = "论坛版块管理";
        string act = Utils.GetRequest("act", "all", 1, "", "");
        builder.Append(Out.Tab("", ""));
        switch (act)
        {
            case "add":
            case "edit":
                AddPage();
                break;
            case "save":
            case "editsave":
                SavePage();
                break;
            case "view":
                ViewPage();
                break;
            case "del":
            case "del2":
                DelPage(act);
                break;
            case "move":
                MovePage();
                break;
            case "move2":
                Move2Page();
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
        builder.Append("论坛版块管理");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 0)
            builder.Append("主题论坛|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?ptype=0") + "\">主题论坛</a>|");

        if (ptype == 1)
            builder.Append("圈子论坛");
        else
            builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?ptype=1") + "\">圈子论坛</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        string strWhere = string.Empty;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string[] pageValUrl = { "ptype" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        if (ptype == 0)
            strWhere = "NodeId=0 and GroupId=0";
        else
            strWhere = "GroupId>0";

        // 开始读取专题
        IList<BCW.Model.Forum> listForum = new BCW.BLL.Forum().GetForums(pageIndex, pageSize, strWhere, out recordCount);
        if (listForum.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Forum n in listForum)
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
                builder.AppendFormat("<a href=\"" + Utils.getUrl("forum.aspx?act=edit&amp;id={0}") + "\">[管理]&gt;</a>{1}.<a href=\"" + Utils.getUrl("forum.aspx?act=view&amp;id={0}") + "\">{2}(ID:{0})</a>", n.ID, n.Paixu, n.Title);
                if (n.GroupId > 0)
                    builder.Append("(圈坛)");

                builder.Append("[帖子" + new BCW.BLL.Forumstat().GetCount(n.ID, 1, 0) + "/回帖" + new BCW.BLL.Forumstat().GetCount(n.ID, 2, 0) + "/当前在线" + n.Line + "/最高" + n.TopLine + "]");
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
        if (ptype == 0)
            builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?act=add") + "\">添加论坛</a><br />");
        else
            builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?act=add&amp;ptype=1") + "\">添加圈坛</a><br />");

        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    private void ViewPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Forum().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        BCW.Model.Forum model = new BCW.BLL.Forum().GetForum(id);
        Master.Title = "" + model.Title + "管理";
        builder.Append(Out.Tab("<div class=\"title\">版块:" + model.Title + "</div>", ""));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?act=add&amp;forumid=" + id + "") + "\">添加</a>.<a href=\"" + Utils.getUrl("../bbs/forum.aspx?forumid=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">预览</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        string strWhere = string.Empty;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string[] pageValUrl = { "" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        strWhere = "NodeId=" + id + "";
        // 开始读取专题
        IList<BCW.Model.Forum> listForum = new BCW.BLL.Forum().GetForums(pageIndex, pageSize, strWhere, out recordCount);
        if (listForum.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Forum n in listForum)
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
                builder.AppendFormat("<a href=\"" + Utils.getUrl("forum.aspx?act=edit&amp;id={0}") + "\">[管理]&gt;</a>{1}.<a href=\"" + Utils.getUrl("forum.aspx?act=view&amp;id={0}") + "\">{2}(ID:{0})</a>", n.ID, n.Paixu, n.Title);
                k++;
                builder.Append(Out.Tab("</div>", ""));

            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有下级版块"));
        }

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?act=edit&amp;id=" + id + "") + "\">编辑论坛</a><br />");
        if (model.GroupId == 0)
            builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?act=move&amp;id=" + id + "") + "\">转移论坛</a><br />");

        builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?act=move2&amp;id=" + id + "") + "\">转移帖子</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?act=del2&amp;id=" + id + "") + "\">清空帖子</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?act=del&amp;id=" + id + "") + "\">删除论坛</a><br />");

        if (model.NodeId != 0)
        {
            builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?act=view&amp;id=" + model.NodeId + "") + "\">返回上一级</a><br />");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("forum.aspx") + "\">返回上一级</a><br />");
        }

        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));

    }
    private void AddPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 1, @"^[0-9]\d*$", "0"));
        int forumid = int.Parse(Utils.GetRequest("forumid", "get", 1, @"^[0-9]\d*$", "0"));
        string PostName = string.Empty;
        if (id != 0)
            PostName = "编辑";
        else
            PostName = "添加";

        BCW.Model.Forum model = null;
        int NodeId = 0;
        //添加状态时的显示
        string sText = string.Empty;
        string sName = string.Empty;
        string sType = string.Empty;
        string sValu = string.Empty;
        string sEmpt = string.Empty;
        if (id == 0)
        {
            sText = "*分配论坛ID(填0则自动分配):/,";
            sName = "id,";
            sType = "num,";
            sValu = "0'";
            sEmpt = "false,";
        }
        else
        {
            model = new BCW.BLL.Forum().GetForum(id);
            if (model == null)
            {
                Utils.Error("不存在的论坛记录", "");
            }
            NodeId = model.NodeId;
            sText = ",";
            sName = "id,";
            sType = "hidden,";
            sValu = "" + model.ID + "'";
            sEmpt = "false,";
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("" + PostName + "论坛版块");
        builder.Append(Out.Tab("</div>", ""));
        strText = "" + sText + "*论坛名称:/,论坛口号:/,论坛LOGO:/,版规公告:/,帖子分类标签:(可空)/,版块顶部Ubb:/,版块底部Ubb:/,发帖限制:/,回帖限制:/,等级限制(级):/,访问限制:/,显示帖子方式:/,显示下级版块:/,允许访问的ID(用#分开):/,关联圈子ID(不关联请填0):/,电脑访问:/,运行状态:/,排序:/,,";
        strName = "" + sName + "Title,Notes,Logo,Content,Label,TopUbb,FootUbb,Postlt,Replylt,Gradelt,Visitlt,ShowType,IsNode,VisitId,GroupId,IsPc,IsActive,Paixu,forumid,act";
        strType = "" + sType + "text,text,text,textarea,text,textarea,textarea,select,select,num,select,select,select,text,num,select,select,num,hidden,hidden";
        if (id == 0)
        {
            strValu = "" + sValu + "'''''''0'0'0'0'0'0''0'0'0'0'" + forumid + "'save";
        }
        else
        {
            strValu = "" + sValu + "" + model.Title + "'" + model.Notes + "'" + model.Logo + "'" + model.Content + "'" + model.Label + "'" + model.TopUbb + "'" + model.FootUbb + "'" + model.Postlt + "'" + model.Replylt + "'" + model.Gradelt + "'" + model.Visitlt + "'" + model.ShowType + "'" + model.IsNode + "'" + model.VisitId + "'" + model.GroupId + "'" + model.IsPc + "'" + model.IsActive + "'" + model.Paixu + "'" + model.NodeId + "'editsave";
        }
        strEmpt = "" + sEmpt + "false,true,true,true,true,true,true,0|不限制|1|VIP会员|2|限版主|3|限管理员|4|禁止发帖,0|不限制|1|VIP会员|2|限版主|3|限管理员|4|禁止回帖,false,0|不限制|1|限会员|2|VIP会员|3|限版主|4|限管理员,0|只显本版贴|1|显示下级帖,0|不显示|1|显示,true,false,0|开启电脑访问|1|关闭电脑访问,0|正常运行|1|暂停运行,false,false";
        strIdea = "/";
        strOthe = "确定" + PostName + "|reset,forum.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("温馨提示:<br />1.帖子标签格式:1|美图|2|游戏|3|软件,其中1、2、3是编号，设置后请慎重修改.<br />2.允许访问的ID留空时则不限制.<br />3.不管限制如何，版主和管理员都可以穿透限制.");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        if (id != 0)
        {
            builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?act=view&amp;id=" + id + "") + "\">查看论坛</a><br />");
        }
        if (NodeId != 0)
        {
            builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?act=view&amp;id=" + NodeId + "") + "\">返回上一级</a><br />");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("forum.aspx") + "\">返回上一级</a><br />");
        }
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void SavePage()
    {
        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[0-9]\d*$", "ID错误"));
        int forumid = int.Parse(Utils.GetRequest("forumid", "post", 2, @"^[0-9]\d*$", "ID错误"));
        string Title = Utils.GetRequest("Title", "post", 2, @"^[^\^]{1,30}$", "论坛名称不能超30字符");
        string Notes = Utils.GetRequest("Notes", "post", 3, @"^[^\^]{1,30}$", "论坛口号不能超50字符");
        string Logo = Utils.GetRequest("Logo", "post", 3, @"^[^\^]{1,200}$", "论坛LOGO不能超200字符");
        string Content = Utils.GetRequest("Content", "post", 3, @"^[^\^]{1,3000}$", "版块公告不能超3000字符");

        string Label = Utils.GetRequest("Label", "post", 3, @"^[^\|]{1,5}(?:\|[^\|]{1,5}){1,500}$", "帖子分类标签填写错误,例子如:1|美图|2|游戏|3|软件");

        //----------计算分类合法性开始
        if (Label != "")
        {
            int GetNum = Utils.GetStringNum(Label, "|");
            if (GetNum % 2 == 0)
            {
                Utils.Error("帖子分类标签填写错误,例子如:1|美图|2|游戏|3|软件", "");
            }
            string[] sTemp = Label.Split("|".ToCharArray());
            for (int j = 0; j < sTemp.Length; j++)
            {
                if (j % 2 == 0)
                {
                    if (sTemp[j] == "0")
                    {
                        Utils.Error("帖子分类标签填写错误,例子如:1|美图|2|游戏|3|软件", "");
                        break;
                    }
                    try
                    {
                        int a = int.Parse(sTemp[j]);
                    }
                    catch
                    {
                        Utils.Error("帖子分类标签填写错误,例子如:1|美图|2|游戏|3|软件", "");
                        break;
                    }
                    int b = int.Parse(sTemp[j]);
                    if (j != 0)
                    {
                        if ((b - 1) != int.Parse(sTemp[j - 2]))
                        {
                            Utils.Error("帖子分类标签填写错误,例子如:1|美图|2|游戏|3|软件", "");
                            break;
                        }
                    }
                }
            }
        }
        //----------计算分类合法性结束

        string TopUbb = Utils.GetRequest("TopUbb", "post", 3, @"^[^\^]{1,800}$", "版块顶部Ubb限800字内");
        string FootUbb = Utils.GetRequest("FootUbb", "post", 3, @"^[^\^]{1,800}$", "版块底部Ubb限800字内");
        int Postlt = int.Parse(Utils.GetRequest("Postlt", "post", 2, @"^[0-4]$", "发帖限制选择错误"));
        int Replylt = int.Parse(Utils.GetRequest("Replylt", "post", 2, @"^[0-4]$", "回帖限制选择错误"));
        int Gradelt = int.Parse(Utils.GetRequest("Gradelt", "post", 2, @"^[0-4]$", "等级级数填写错误"));
        int Visitlt = int.Parse(Utils.GetRequest("Visitlt", "post", 2, @"^[0-4]$", "访问限制选择错误"));
        int ShowType = int.Parse(Utils.GetRequest("ShowType", "post", 2, @"^[0-1]$", "显示帖子方式选择错误"));
        int IsNode = int.Parse(Utils.GetRequest("IsNode", "post", 2, @"^[0-1]$", "显示下级版块选择错误"));
        string VisitId = Utils.GetRequest("VisitId", "post", 3, @"^[^\#]{1,10}(?:\#[^\#]{1,10}){0,800}$", "限制访问ID填写错误，格式如1234#1111#2222");
        int GroupId = int.Parse(Utils.GetRequest("GroupId", "post", 2, @"^[0-9]\d*$", "关联圈子ID填写错误，不关联请填写0"));
        int IsPc = int.Parse(Utils.GetRequest("IsPc", "post", 2, @"^[0-1]$", "电脑访问选择错误"));
        int IsActive = int.Parse(Utils.GetRequest("IsActive", "post", 2, @"^[0-1]$", "运行状态选择错误"));
        int Paixu = int.Parse(Utils.GetRequest("Paixu", "post", 2, @"^[0-9]\d*$", "排序填写错误"));
        //ID是否可用
        if (Request["act"] == "save")
        {
            if (id != 0)
            {
                if (new BCW.BLL.Forum().Exists(id))
                {
                    Utils.Error("此ID已被使用，请选择其它ID或者填0自动分配", "");
                }
            }
            else
            {
                id = new BCW.BLL.Forum().GetMaxId();
            }
        }
        BCW.Model.Forum model = new BCW.Model.Forum();
        model.ID = id;
        model.NodeId = forumid;
        model.Title = Title;
        model.Notes = Notes;
        model.Logo = Logo;
        model.Content = Content;
        model.Label = Label;
        model.Postlt = Postlt;
        model.Replylt = Replylt;
        model.Gradelt = Gradelt;
        model.Visitlt = Visitlt;
        model.ShowType = ShowType;
        model.IsNode = IsNode;
        model.VisitId = VisitId;
        model.GroupId = GroupId;
        model.IsPc = IsPc;
        model.IsActive = IsActive;
        model.TopUbb = TopUbb;
        model.FootUbb = FootUbb;
        model.Paixu = Paixu;
        if (Request["act"] == "save")
        {
            new BCW.BLL.Forum().Add(model);
            string ForUrl = string.Empty;
            if (forumid == 0)
                ForUrl = "forum.aspx";
            else
                ForUrl = "forum.aspx?act=view&amp;id=" + forumid + "";

            // 更新forumid下级ID集合
            if (forumid > 0)
                BCW.User.Users.UpdateForumNode(id);

            Utils.Success("添加论坛", "添加论坛成功..", Utils.getUrl(ForUrl), "1");

        }
        else
        {
            new BCW.BLL.Forum().Update(model);
            Utils.Success("编辑论坛", "编辑论坛成功..", Utils.getUrl("forum.aspx?act=edit&amp;id=" + id + ""), "1");
        }

    }
    private void DelPage(string act)
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (!new BCW.BLL.Forum().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        if (new BCW.BLL.Forum().ExistsNodeId(id))
        {
            Utils.Error("请先删除下级版块", "");
        }
        if (info != "ok")
        {

            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定要删除论坛吗？不可恢复，请慎重操作");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?act=move2&amp;id=" + id + "") + "\">先转移版块帖子</a>");
            if (act == "del")
                builder.Append("<br /><a href=\"" + Utils.getUrl("forum.aspx?info=ok&amp;act=del&amp;id=" + id + "") + "\">确定删除(含版块帖子/回帖)</a>");
            else
                builder.Append("<br /><a href=\"" + Utils.getUrl("forum.aspx?info=ok&amp;act=del2&amp;id=" + id + "") + "\">确定清空帖子/回帖</a>");

            builder.Append("<br /><a href=\"" + Utils.getUrl("forum.aspx?act=view&amp;id=" + id + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (act == "del")
            {
                int NodeId = new BCW.BLL.Forum().GetNodeId(id);

                string ForUrl = string.Empty;
                if (NodeId != 0)
                    ForUrl = "forum.aspx?act=view&amp;id=" + NodeId + "";
                else
                    ForUrl = "forum.aspx";

                //删除论坛
                new BCW.BLL.Forum().Delete(id);
                //更新下级版块ID集合
                BCW.User.Users.UpdateDoNode();
                //关联数据删除
                new BCW.BLL.Text().DeleteStr("ForumId=" + id + "");
                new BCW.BLL.Reply().DeleteStr("ForumId=" + id + "");
                new BCW.BLL.Forumstat().DeleteStr("ForumId=" + id + "");

                Utils.Success("删除论坛", "删除论坛成功..", Utils.getUrl(ForUrl), "1");
            }
            else
            {
                new BCW.BLL.Text().DeleteStr("ForumId=" + id + "");
                new BCW.BLL.Reply().DeleteStr("ForumId=" + id + "");
                new BCW.BLL.Forumstat().DeleteStr("ForumId=" + id + "");
                Utils.Success("清空帖子/回帖", "清空帖子/回帖成功..", Utils.getUrl("forum.aspx?act=view&amp;id=" + id + ""), "1");
            }

        }
    }

    private void MovePage()
    {
        Master.Title = "转移论坛";
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        int nid = int.Parse(Utils.GetRequest("nid", "get", 1, @"^[0-9]\d*$", "-1"));
        if (!new BCW.BLL.Forum().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        if (nid !=-1)
        {
            if (!new BCW.BLL.Forum().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            string info = Utils.GetRequest("info", "all", 1, "", "");
            if (info == "")
            {
                string goFourm = string.Empty;
                if (nid > 0)
                    goFourm = "《" + new BCW.BLL.Forum().GetTitle(nid) + "》的下级论坛";
                else
                    goFourm = "根目录";

                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("确定把《" + new BCW.BLL.Forum().GetTitle(id) + "》转成" + goFourm + "吗？");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?info=ok&amp;act=move&amp;id=" + id + "&amp;nid=" + nid + "") + "\">确定转移</a>");
                builder.Append("<br /><a href=\"" + Utils.getUrl("forum.aspx?act=view&amp;id=" + id + "") + "\">再看看吧..</a>");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            else
            {
                //不能转移到自己下级的版块
                if (nid > 0)
                {
                    int NodeId2 = new BCW.BLL.Forum().GetNodeId(nid);
                    string DoNode = new BCW.BLL.Forum().GetDoNode(id);
                    if (id == NodeId2)
                    {
                        Utils.Error("不能转移到此版下级版块", "");
                    }
                    if (("," + DoNode + ",").Contains("," + nid + ","))
                    {
                        Utils.Error("不能转移到此版下级版块", "");
                    }
                }
                //得到节点ID
                int NodeId = new BCW.BLL.Forum().GetNodeId(id);
                //转移版块
                new BCW.BLL.Forum().UpdateNodeId(id, nid);
                //更新下级版块ID集合
                BCW.User.Users.UpdateDoNode();
                Utils.Success("转移论坛", "转移论坛成功..", Utils.getUrl("forum.aspx?act=view&amp;id=" + id + ""), "1");
            }
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.AppendFormat("请选择转移到的论坛");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?act=move&amp;id=" + id + "&amp;nid=0") + "\">移到根目录←</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            int pageIndex;
            int recordCount;
            string strWhere = string.Empty;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string[] pageValUrl = { "act","id","nid" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            strWhere = "id<>" + id + " and GroupId=0";

            // 开始读取论坛
            IList<BCW.Model.Forum> listForum = new BCW.BLL.Forum().GetForums(pageIndex, pageSize, strWhere, out recordCount);
            if (listForum.Count > 0)
            {
                int k = 1;
                foreach (BCW.Model.Forum n in listForum)
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
                    builder.AppendFormat("{0}<a href=\"" + Utils.getUrl("forum.aspx?act=view&amp;id=" + id + "") + "\">{1} ID{2}</a>.<a href=\"" + Utils.getUrl("forum.aspx?act=move&amp;id=" + id + "&amp;nid={2}") + "\">[移]</a>", "", n.Title, n.ID);
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
            builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?act=view&amp;id=" + id + "") + "\">返回上一级</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }

    private void Move2Page()
    {
        Master.Title = "批量转移帖子";
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        int nid = int.Parse(Utils.GetRequest("nid", "get", 1, @"^[0-9]\d*$", "0"));
        if (!new BCW.BLL.Forum().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        if (nid > 0)
        {
            if (!new BCW.BLL.Forum().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            string info = Utils.GetRequest("info", "all", 1, "", "");
            if (info == "")
            {
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("确定把《" + new BCW.BLL.Forum().GetTitle(id) + "》的帖子转移到《" + new BCW.BLL.Forum().GetTitle(nid) + "》论坛吗？");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?info=ok&amp;act=move2&amp;id=" + id + "&amp;nid=" + nid + "") + "\">确定转移</a>");
                builder.Append("<br /><a href=\"" + Utils.getUrl("forum.aspx?act=view&amp;id=" + id + "") + "\">再看看吧..</a>");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            else
            {
                new BCW.BLL.Text().UpdateForumID2(id, nid);
                new BCW.BLL.Reply().UpdateForumID2(id, nid);
                //论坛统计更新
                new BCW.BLL.Forumstat().UpdateForumID(id, nid);
                Utils.Success("批量转移帖子", "批量转移帖子成功..", Utils.getUrl("forum.aspx?act=view&amp;id=" + id + ""), "1");
            }
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.AppendFormat("请选择转移到的论坛");
            builder.Append(Out.Tab("</div>", "<br />"));
            int pageIndex;
            int recordCount;
            string strWhere = string.Empty;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string[] pageValUrl = { "act", "id","nid" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            strWhere = "id<>" + id + "";

            // 开始读取论坛
            IList<BCW.Model.Forum> listForum = new BCW.BLL.Forum().GetForums(pageIndex, pageSize, strWhere, out recordCount);
            if (listForum.Count > 0)
            {
                int k = 1;
                foreach (BCW.Model.Forum n in listForum)
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
                    builder.AppendFormat("<a href=\"" + Utils.getUrl("forum.aspx?act=move2&amp;id=" + id + "&amp;nid={2}") + "\">{1} ID{2}</a>.<a href=\"" + Utils.getUrl("forum.aspx?act=move2&amp;id=" + id + "&amp;nid={2}") + "\">[移]</a>", "", n.Title, n.ID);
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
            builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?act=view&amp;id=" + id + "") + "\">返回上一级</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
}
