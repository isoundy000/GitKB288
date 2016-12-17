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

public partial class bbs_mebook : System.Web.UI.Page
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

        int hid = int.Parse(Utils.GetRequest("hid", "all", 1, @"^[0-9]\d*$", "0"));
        if (hid == 0)
            hid = meid;

        if (new BCW.User.Limits().IsUserLimit(BCW.User.Limits.enumRole.Role_UsBook, hid))
        {
            Utils.Error("此留言本已被管理员禁止使用", "");
        }


        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "admin":
                AdminPage(meid, hid);
                break;
            case "add":
                AddPage(meid, hid);
                break;
            case "save":
                AddSavePage(meid, hid);
                break;
            case "del":
            case "del2":
                DelPage(act, meid, hid);
                break;
            case "reok":
                ReOkPage(meid, hid);
                break;
            case "top":
                TopPage(act, meid, hid);
                break;
            default:
                ReloadPage(meid, hid);
                break;
        }
    }

    private void ReloadPage(int meid, int hid)
    {
        string UsName = new BCW.BLL.User().GetUsName(hid);
        if (UsName == "")
            Utils.Error("不存在的会员ID", "");
        Master.Title = "" + UsName + "的留言本";
        builder.Append(Out.Tab("<div class=\"title\">" + UsName + "的留言本</div>", ""));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("留言|");
        builder.Append("<a href=\"" + Utils.getUrl("visit.aspx?act=list&amp;ptype=1&amp;hid=" + hid + "") + "\">拜访</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("visit.aspx?act=list&amp;ptype=2&amp;hid=" + hid + "") + "\">访客</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("action.aspx?act=friend&amp;hid=" + hid + "") + "\">好友</a>|");

        if (meid == hid)
            builder.Append("<a href=\"" + Utils.getUrl("action.aspx?act=fans&amp;hid=" + hid + "") + "\">关注</a>");
        else
            builder.Append("<a href=\"" + Utils.getUrl("action.aspx?act=me&amp;hid=" + hid + "") + "\">动态</a>");

        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("mebook.aspx?act=add&amp;hid=" + hid + "") + "\">我要留言</a>");
        if (meid == hid)
        {
            builder.Append("|<a href=\"" + Utils.getUrl("myedit.aspx?act=mebook&amp;backurl=" + Utils.PostPage(1) + "") + "\">设置</a>");
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "hid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        strWhere = "UsID=" + hid + " and type=0";//邵广林 20160524 前台留言为0，农场留言为1001

        // 开始读取列表
        IList<BCW.Model.Mebook> listMebook = new BCW.BLL.Mebook().GetMebooks(pageIndex, pageSize, strWhere, out recordCount);
        if (listMebook.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Mebook n in listMebook)
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
                if (n.IsTop == 1)
                    builder.Append("[顶]");

                builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.MID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.MName + "(" + n.MID + ")</a>");
                builder.Append(":" + Out.SysUBB(n.MContent) + "(" + DT.FormatDate(n.AddTime, 1) + ")");
                if (!string.IsNullOrEmpty(n.ReText))
                {
                    builder.Append(Out.Tab("<font color=\"red\">", ""));
                    builder.Append("<br />★主人回复:" + n.ReText + "");
                    builder.Append(Out.Tab("</font>", ""));
                }

                if (meid == hid || new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_UsBook, meid))
                {
                    builder.Append("<a href=\"" + Utils.getUrl("mebook.aspx?act=admin&amp;hid=" + hid + "&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[管理]</a>");
                }
                builder.Append(Out.Tab("", Out.LHr()));
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
        builder.Append(Out.Tab("", "<br />"));
        strText = ",,,";
        strName = "Content,hid,act";
        strType = "stext,hidden,hidden";
        strValu = "'" + hid + "'save";
        strEmpt = "true,false,false";
        strIdea = "";
        strOthe = "提交留言,mebook.aspx,post,3,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx?uid=" + meid + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("mebook.aspx?act=add&amp;hid=" + hid + "") + "\">我要留言</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void AdminPage(int meid, int hid)
    {
        if (meid != hid && !new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_UsBook, meid))
        {
            Utils.Error("你的权限不足", "");
        }
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.Mebook model = new BCW.BLL.Mebook().GetMebook(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "管理留言";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("管理留言");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        if (new BCW.BLL.Mebook().GetIsTop(id) == 0)
            builder.Append("<a href=\"" + Utils.getUrl("mebook.aspx?act=top&amp;hid=" + hid + "&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">置顶本条留言</a><br />");
        else
            builder.Append("<a href=\"" + Utils.getUrl("mebook.aspx?act=top&amp;hid=" + hid + "&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">去顶本条留言</a><br />");

        builder.Append("<a href=\"" + Utils.getUrl("mebook.aspx?act=del&amp;hid=" + hid + "&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">删除本条留言</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("mebook.aspx?act=del2&amp;hid=" + hid + "&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">删除TA的所有留言</a>");
        if (meid == hid)
        {
            builder.Append("<br /><a href=\"" + Utils.getUrl("mebook.aspx?act=reok&amp;hid=" + hid + "&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">回复本条留言</a>");
            builder.Append("<br /><a href=\"" + Utils.getUrl("friend.aspx?act=addblack&amp;hid=" + model.MID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">禁止TA留言</a>");
        }
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("mebook.aspx?hid=" + hid + "") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void TopPage(string act, int meid, int hid)
    {
        Master.Title = "置顶留言";
        if (meid != hid && !new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_UsBook, meid))
        {
            Utils.Error("你的权限不足", "");
        }
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));

        string info = Utils.GetRequest("info", "get", 1, "", "");

        if (!new BCW.BLL.Mebook().Exists(id, hid))
        {
            Utils.Error("不存在的记录", "");
        }
        int IsTop = new BCW.BLL.Mebook().GetIsTop(id);
        string topText = string.Empty;
        if (IsTop == 0)
            topText = "置顶";
        else
            topText = "去顶";

        if (info != "ok")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定" + topText + "此留言吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("mebook.aspx?act=" + act + "&amp;info=ok&amp;hid=" + hid + "&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定" + topText + "</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("mebook.aspx?act=admin&amp;hid=" + hid + "&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("mebook.aspx?hid=" + hid + "") + "\">&gt;返回上一级</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            if (IsTop == 0)
                new BCW.BLL.Mebook().UpdateIsTop(id, 1);
            else
                new BCW.BLL.Mebook().UpdateIsTop(id, 0);

            Utils.Success("" + topText + "留言", "" + topText + "成功，正在返回..", Utils.getPage("mebook.aspx?hid=" + hid + ""), "1");
        }
    }

    private void AddPage(int meid, int hid)
    {
        string UsName = new BCW.BLL.User().GetUsName(hid);
        if (UsName == "")
            Utils.Error("不存在的会员ID", "");
        Master.Title = "" + UsName + "的留言本";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("我要留言");
        builder.Append(Out.Tab("</div>", ""));

        int copy = int.Parse(Utils.GetRequest("copy", "get", 1, @"^[0-1]$", "0"));
        int ff = int.Parse(Utils.GetRequest("ff", "get", 1, @"^[0-9]\d*$", "-1"));
        int dd = int.Parse(Utils.GetRequest("dd", "get", 1, @"^[0-9]\d*$", "0"));
        //复制内容
        string Copytemp = string.Empty;
        if (ff >= 0)
            Copytemp += "[F]" + ff + "[/F]";

        if (dd > 0)
            Copytemp += new BCW.BLL.Submit().GetContent(dd, meid);

        if (copy == 1)
            Copytemp += new BCW.BLL.User().GetCopytemp(meid);

        strText = ",,,";
        strName = "Content,hid,act";
        strType = "text,hidden,hidden";
        strValu = "" + Copytemp + "'" + hid + "'save";
        strEmpt = "true,false,false";
        strIdea = "/";
        strOthe = "提交留言,mebook.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("插入:<a href=\"" + Utils.getUrl("function.aspx?act=admsub&amp;backurl=" + Utils.PostPage(1) + "") + "\">常用短语</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("function.aspx?act=face&amp;backurl=" + Utils.PostPage(1) + "") + "\">表情</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("mebook.aspx?act=add&amp;hid=" + hid + "&amp;ff=" + ff + "&amp;dd=" + dd + "&amp;copy=1") + "\">[粘贴]</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("mebook.aspx?hid=" + hid + "") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));

    }

    private void AddSavePage(int meid, int hid)
    {
        BCW.User.Users.ShowVerifyRole("k", meid);//非验证会员提示
        new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_UsBook, meid);//会员本身权限
        string Content = Utils.GetRequest("Content", "post", 2, @"^[\s\S]{1,500}$", "留言内容限1-500字");
        //是否刷屏
        string appName = "LIGHT_MEBOOK";
        int Expir = Convert.ToInt32(ub.GetSub("BbsMebookExpir", "/Controls/bbs.xml"));
        BCW.User.Users.IsFresh(appName, Expir);

        //你是否是对方的黑名单
        if (new BCW.BLL.Friend().Exists(hid, meid, 1))
        {
            Utils.Error("对方已把您加入黑名单", "");
        }
        //限制性
        string ForumSet = new BCW.BLL.User().GetForumSet(hid);
        int addBookSet = BCW.User.Users.GetForumSet(ForumSet, 19);
        if (addBookSet == 1 && meid != hid)
        {
            if (!new BCW.BLL.Friend().Exists(hid, meid, 0))
            {
                Utils.Error("您不在对方的好友里，对方已设置拒绝非好友留言", "");
            }
        }
        else if (addBookSet == 2)
        {
            Utils.Error("此留言本已禁止任何人留言", "");
        }

        BCW.Model.Mebook model = new BCW.Model.Mebook();
        model.UsID = hid;
        model.MID = meid;
        model.MName = new BCW.BLL.User().GetUsName(meid);
        model.MContent = Content;
        model.IsTop = 0;
        model.AddTime = DateTime.Now;
        new BCW.BLL.Mebook().Add(model);
        //内线通知主人
        if (meid != hid)
        {
            new BCW.BLL.Guest().Add(3, hid, new BCW.BLL.User().GetUsName(hid), "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + new BCW.BLL.User().GetUsName(meid) + "[/url]在您的空间[url=/bbs/mebook.aspx?hid=" + hid + "]留言啦[/url]!");
        }
        Utils.Success("发表留言", "发表留言成功，正在返回..", Utils.getUrl("mebook.aspx?hid=" + hid + ""), "1");
    }


    private void DelPage(string act, int meid, int hid)
    {
        Master.Title = "删除留言";
        if (meid != hid && !new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_UsBook, meid))
        {
            Utils.Error("你的权限不足", "");
        }
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));

        string info = Utils.GetRequest("info", "get", 1, "", "");

        if (!new BCW.BLL.Mebook().Exists(id, hid))
        {
            Utils.Error("不存在的记录", "");
        }
        if (info != "ok")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            if (act == "del")
                builder.Append("确定删除此留言吗");
            else
                builder.Append("确定删除此会员的所有留言吗");

            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("mebook.aspx?act=" + act + "&amp;info=ok&amp;hid=" + hid + "&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("mebook.aspx?act=admin&amp;hid=" + hid + "&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("mebook.aspx?hid=" + hid + "") + "\">&gt;返回上一级</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            if (act == "del")
                new BCW.BLL.Mebook().Delete(id);
            else
            {
                //取归属会员ID
                int MID = new BCW.BLL.Mebook().GetMID(id);
                new BCW.BLL.Mebook().Delete(meid, MID);
            }
            Utils.Success("删除留言", "删除成功，正在返回..", Utils.getPage("mebook.aspx?hid=" + hid + ""), "1");
        }
    }

    private void ReOkPage(int meid, int hid)
    {
        BCW.User.Users.ShowVerifyRole("k", meid);//非验证会员提示
        new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_UsBook, meid);//会员本身权限
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));

        string info = Utils.GetRequest("info", "post", 1, "", "");

        BCW.Model.Mebook model = new BCW.BLL.Mebook().GetMebook(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.UsID != meid)
        {
            Utils.Error("不存在的记录", "");
        }
        if (info == "ok")
        {
            string Content = Utils.GetRequest("Content", "post", 3, @"^[\s\S]{1,500}$", "回复留言内容限1-500字内");
            new BCW.BLL.Mebook().UpdateReText(id, Content);
            //内线通知留言者
            if (meid != model.MID)
            {
                new BCW.BLL.Guest().Add(3, model.MID, model.MName, "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + new BCW.BLL.User().GetUsName(meid) + "[/url]回复了你您在TA空间的留言啦，快去[url=/bbs/mebook.aspx?hid=" + meid + "]看看吧[/url]!");
            }
            Utils.Success("回复留言", "回复留言成功，正在返回..", Utils.getPage("mebook.aspx?hid=" + hid + ""), "1");
        }
        else
        {
            Master.Title = "回复留言";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("回复留言");
            builder.Append(Out.Tab("</div>", ""));

            string strText, strName, strType, strValu, strEmpt, strIdea, strOthe;
            strText = "回复内容:/,,,,,";
            strType = "textarea,hidden,hidden,hidden,hidden,hidden";
            strName = "Content,id,hid,act,info,backurl";
            strValu = "" + model.ReText + "'" + id + "'" + hid + "'reok'ok'" + Utils.getPage(0) + "";
            strEmpt = "true,false,false,false,false,false";
            strIdea = "/";
            strOthe = "确定回复|reset,mebook.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", " "));
            builder.Append("<a href=\"" + Utils.getUrl("mebook.aspx?act=admin&amp;hid=" + hid + "&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">取消</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("mebook.aspx?hid=" + hid + "") + "\">&gt;返回上一级</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }
}