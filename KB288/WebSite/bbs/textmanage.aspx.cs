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

public partial class bbs_textmanage : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/bbs.xml";
    protected string strText = string.Empty;
    protected string strName = string.Empty;
    protected string strType = string.Empty;
    protected string strValu = string.Empty;
    protected string strEmpt = string.Empty;
    protected string strIdea = string.Empty;
    protected string strOthe = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "管理帖子";
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
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "fill":
                FillPage(meid, forumid, bid);
                break;
            case "over":
                OverPage(meid, forumid, bid);
                break;
            case "del":
                DelPage(meid, forumid, bid);
                break;
            case "edit":
                EditPage(meid, forumid, bid);
                break;
            case "opedit":
                OpEditPage(act, meid, forumid, bid);
                break;
            case "good":
            case "delgood":
                GoodPage(act, meid, forumid, bid);
                break;
            case "recom":
            case "delrecom":
                RecomPage(act, meid, forumid, bid);
                break;
            case "top":
            case "deltop":
                TopPage(act, meid, forumid, bid);
                break;
            case "top2":
            case "deltop2":
                Top2Page(act, meid, forumid, bid);
                break;
            case "lock":
            case "dellock":
                LockPage(act, meid, forumid, bid);
                break;
            case "move":
                MovePage(meid, forumid, bid);
                break;
            case "forumts":
            case "delforumts":
                ForumtsPage(act, meid, forumid, bid);
                break;
            case "flow":
            case "delflow":
                FlowPage(act, meid, forumid, bid);
                break;
            case "flowset":
                FlowSetPage(act, meid, forumid, bid);
                break;
            default:
                ReloadPage(meid, forumid, bid);
                break;
        }
    }

    private void ReloadPage(int uid, int forumid, int bid)
    {
        BCW.Model.Text model = new BCW.BLL.Text().GetText(bid);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("管理：<a href=\"" + Utils.getPage("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "") + "\">" + model.Title + "</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("楼主：<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.UsName + "</a>");
        builder.Append(Out.Tab("</div>", "<br />"));


        builder.Append(Out.Tab("<div>", ""));

        if (model.UsID == uid)
        {
            builder.Append("<a href=\"" + Utils.getUrl("textmanage.aspx?act=edit&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">编辑</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("textmanage.aspx?act=opedit&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">分段编辑</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("textmanage.aspx?act=fill&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">续写</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("textmanage.aspx?act=over&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">结帖</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("addThread20.aspx?act=fill&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">追加文件</a> ");
            if (ub.GetSub("BbsThreadDel", xmlPath) == "0")
                builder.Append("<a href=\"" + Utils.getUrl("textmanage.aspx?act=del&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">删除</a> ");

        }
        else
        {
            if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_EditText, uid, forumid))
            {
                builder.Append("<a href=\"" + Utils.getUrl("textmanage.aspx?act=edit&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">编辑</a> ");
            }
            //删除
            if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_DelText, uid, forumid))
            {
                builder.Append("<a href=\"" + Utils.getUrl("textmanage.aspx?act=del&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">删除</a> ");
            }
        }
        
        //精华
        if (model.IsGood == 0)
        {
            if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_GoodText, uid, forumid))
            {
                builder.Append("<a href=\"" + Utils.getUrl("textmanage.aspx?act=good&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">加精</a> ");
            }
        }
        else
        {
            if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_DelGoodText, uid, forumid))
            {
                builder.Append("<a href=\"" + Utils.getUrl("textmanage.aspx?act=delgood&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">解精</a> ");
            }

        }
        //推荐
        if (model.IsRecom == 0)
        {
            if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_RecomText, uid, forumid))
            {
                builder.Append("<a href=\"" + Utils.getUrl("textmanage.aspx?act=recom&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">推荐</a> ");
            }
        }
        else
        {
            if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_DelRecomText, uid, forumid))
            {
                builder.Append("<a href=\"" + Utils.getUrl("textmanage.aspx?act=delrecom&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">解推荐</a> ");
            }
        }
        //锁定
        if (model.IsLock == 0)
        {
            if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_LockText, uid, forumid))
            {
                builder.Append("<a href=\"" + Utils.getUrl("textmanage.aspx?act=lock&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">锁定</a> ");
            }
        }
        else
        {
            if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_DelLockText, uid, forumid))
            {
                builder.Append("<a href=\"" + Utils.getUrl("textmanage.aspx?act=dellock&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">解锁</a> ");
            }
        }
        //置顶
        if (model.IsTop != 1 && model.IsTop != 2)
        {
            if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_TopText, uid, forumid))
            {
                builder.Append("<a href=\"" + Utils.getUrl("textmanage.aspx?act=top&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">置顶</a> ");
            }
        }
        else
        {
            if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_DelTopText, uid, forumid))
            {
                builder.Append("<a href=\"" + Utils.getUrl("textmanage.aspx?act=deltop&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">去顶</a> ");
            }
        }
        //固底
        if (model.IsTop != -1)
        {
            if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_Top2Text, uid, forumid))
            {
                builder.Append("<a href=\"" + Utils.getUrl("textmanage.aspx?act=top2&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">固底</a> ");
            }
        }
        else
        {
            if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_DelTop2Text, uid, forumid))
            {
                builder.Append("<a href=\"" + Utils.getUrl("textmanage.aspx?act=deltop2&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">去底</a> ");
            }
        }
        //转移
        if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_MoveText, uid, forumid))
        {
            builder.Append("<a href=\"" + Utils.getUrl("textmanage.aspx?act=move&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">转移</a> ");
        }

        if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_TopicsSet, uid, forumid))
        {
            if (model.IsFlow == 0)
            {
                if (!IsGdID(uid))
                {
                    builder.Append("<a href=\"" + Utils.getUrl("textmanage.aspx?act=flow&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">设滚</a> ");
                }
                else
                {
                    builder.Append("<a href=\"" + Utils.getUrl("textmanage.aspx?act=flowset&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">设滚</a> ");                
                }
            
            }
            else
                builder.Append("<a href=\"" + Utils.getUrl("textmanage.aspx?act=delflow&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">撤滚</a> ");

            builder.Append("<a href=\"" + Utils.getUrl("textmanage.aspx?act=forumts&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">设专</a> ");
        }

        if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_AddForumBlack, uid, forumid))
        {
            builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?act=blackadd&amp;uid=" + model.UsID + "&amp;forumid=" + forumid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">加黑</a> ");
        }

        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "") + "\">主题帖子</a>");
        builder.Append(Out.Tab("</div>", ""));

    }

    private void DelPage(int uid, int forumid, int bid)
    {
        BCW.Model.Text model = new BCW.BLL.Text().GetText(bid);//GetTextMe
        if (ub.GetSub("BbsThreadDel", xmlPath) == "0")
        {
            if (model.UsID != uid && new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_DelText, uid, forumid) == false)
            {
                Utils.Error("你的权限不足", "");
            }
        }
        else
        {
            if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_DelText, uid, forumid) == false)
            {
                Utils.Error("你的权限不足", "");
            }
        }

        if (model.IsGood == 1)
        {
            Utils.Error("本帖子已加精，不能删除", "");
        }
        if (model.IsRecom == 1)
        {
            Utils.Error("本帖子已推荐，不能删除", "");
        }
        if (model.IsLock == 1)
        {
            Utils.Error("本帖子已锁定，不能删除", "");
        }
        if (model.IsTop == -1)
        {
            Utils.Error("本帖子已固底，不能删除", "");
        }
        if (model.ForumId == 1)
        {
            Utils.Error("本版帖子不允许删除", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此帖子吗");
            builder.Append(Out.Tab("</div>", ""));
            if (model.UsID != uid)
            {
                strText = "理由:,,,,,";
                strName = "Why,forumid,bid,act,info,backurl";
                strType = "text,hidden,hidden,hidden,hidden,hidden";
                strValu = "'" + forumid + "'" + bid + "'del'ok'" + Utils.getPage(0) + "";
                strEmpt = "true,false,false,false,false,false";
                strIdea = "/";
                strOthe = "确定删除,textmanage.aspx,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

                builder.Append(Out.Tab("<div>", ""));
                builder.Append(" <a href=\"" + Utils.getUrl("textmanage.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">取消</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("textmanage.aspx?info=ok&amp;act=del&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("textmanage.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getPage("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "") + "\">主题帖子</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            //管理安全提示
            if (model.UsID != uid)
            {
                string[] p_pageArr = { "act", "forumid", "bid", "Why", "info", "backurl" };
                BCW.User.Role.AdminSafePage(uid, Utils.getPageUrl(), p_pageArr, "post");
            }
            new BCW.BLL.Text().UpdateIsDel(bid, 1);
            new BCW.BLL.Forumstat().Update2(1, model.UsID, forumid,model.AddTime);//更新统计表发帖
            DataSet ds = new BCW.BLL.Reply().GetList("ID,AddTime,UsID,IsDel", "forumid=" + forumid + " and bid=" + bid + "");  //更新统计表回帖
            {
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (int.Parse(ds.Tables[0].Rows[i]["IsDel"].ToString()) == 0)//如果回帖没有删除
                        {
                             new BCW.BLL.Forumstat().Update2(2, int.Parse(ds.Tables[0].Rows[i]["UsID"].ToString()), forumid, DateTime.Parse(ds.Tables[0].Rows[i]["AddTime"].ToString()));
                        }

                    }
                }
            }
            new BCW.BLL.Reply().UpdateIsDel(bid, 1);
            //删除附件
            //if (info == "ok2")
            //{
            //    DataSet ds = new BCW.BLL.Upfile().GetList("ID,Files", "UsID=" + UsID + " and BID=" + bid + "");
            //    if (ds != null && ds.Tables[0].Rows.Count > 0)
            //    {
            //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //        {
            //            new BCW.BLL.Upfile().Delete(int.Parse(ds.Tables[0].Rows[i]["ID"].ToString()));
            //            BCW.Files.FileTool.DeleteFile(ds.Tables[0].Rows[i]["Files"].ToString());
            //        }
            //    }
            //}

            //记录日志
            string Why = Utils.GetRequest("Why", "post", 3, @"^[^\^]{1,20}$", "理由限20字内，可留空");
            string strLog = string.Empty;
            if (model.UsID != uid)
            {            
                //积分操作
                new BCW.User.Cent().UpdateCent(BCW.User.Cent.enumRole.Cent_DelText, model.UsID);
                strLog = "[url=/bbs/uinfo.aspx?uid=" + model.UsID + "]" + model.UsName + "[/url]的主题《" + model.Title + "》被[url=/bbs/uinfo.aspx?uid=" + uid + "]" + new BCW.BLL.User().GetUsName(uid) + "[/url]删除!";
                new BCW.BLL.Guest().Add(0, model.UsID, model.UsName, "您的主题《" + model.Title + "》被[url=/bbs/uinfo.aspx?uid=" + uid + "]" + new BCW.BLL.User().GetUsName(uid) + "[/url]删除!");
            }
            else
            {           
                //积分操作
                new BCW.User.Cent().UpdateCent(BCW.User.Cent.enumRole.Cent_MeDelText, model.UsID);
                strLog = "[url=/bbs/uinfo.aspx?uid=" + model.UsID + "]" + model.UsName + "[/url]删除自己的主题《" + model.Title + "》";
            }
            if (!string.IsNullOrEmpty(Why))
                strLog += "理由:" + Why + "";
            new BCW.BLL.Forumlog().Add(6, forumid, strLog);
            Utils.Success("删除帖子", "删除帖子成功，正在返回..", Utils.getUrl("forum.aspx?forumid=" + forumid + ""), "1");
        }
    }

    private void GoodPage(string act, int uid, int forumid, int bid)
    {
        string sText = string.Empty;
        int iText = 0;
        if (act == "good")
        {
            sText = "加为";
            iText = 1;
            new BCW.User.Role().CheckUserRole(BCW.User.Role.enumRole.Role_GoodText, uid, forumid);
        }
        else
        {
            sText = "解除";
            iText = 0;
            new BCW.User.Role().CheckUserRole(BCW.User.Role.enumRole.Role_DelGoodText, uid, forumid);
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info != "ok")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定" + sText + "精华吗");
            builder.Append(Out.Tab("</div>", ""));

            strText = "理由:,,,,,";
            strName = "Why,forumid,bid,act,info,backurl";
            strType = "text,hidden,hidden,hidden,hidden,hidden";
            strValu = "'" + forumid + "'" + bid + "'" + act + "'ok'" + Utils.getPage(0) + "";
            strEmpt = "true,false,false,false,false,false";
            strIdea = "/";
            strOthe = "确定" + sText + ",textmanage.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append(" <a href=\"" + Utils.getUrl("textmanage.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">取消</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getPage("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "") + "\">主题帖子</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            //管理安全提示
            string[] p_pageArr = { "act", "forumid", "bid", "Why", "info", "backurl" };
            BCW.User.Role.AdminSafePage(uid, Utils.getPageUrl(), p_pageArr, "post");

            int IsGood = new BCW.BLL.Text().GetIsGood(bid);
            if (iText == 1 && IsGood == 1)
            {
                Utils.Error("本帖已是精华", "");
            }
            else if (iText == 0 && IsGood == 0)
            {
                Utils.Error("本帖已解除精华", "");
            }
            BCW.Model.Text model = new BCW.BLL.Text().GetTextMe(bid);
            if (uid == model.UsID)
            {
                Utils.Error("不能加精/解精自己的帖子", "");
            }
            //记录日志
           

            //积分操作/论坛统计/圈子论坛不进行任何奖励
            int GroupId = new BCW.BLL.Forum().GetGroupId(forumid);
            if (GroupId == 0)
            {
                if (iText == 1)
                    new BCW.User.Cent().UpdateCent(BCW.User.Cent.enumRole.Cent_GoodText, model.UsID);
                else
                    new BCW.User.Cent().UpdateCent(BCW.User.Cent.enumRole.Cent_DelGoodText, model.UsID);
            }
            string Why = Utils.GetRequest("Why", "post", 3, @"^[^\^]{1,20}$", "理由限20字内，可留空");
            string strLog = "主题[url=/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "]《" + model.Title + "》[/url]被[url=/bbs/uinfo.aspx?uid=" + uid + "]" + new BCW.BLL.User().GetUsName(uid) + "[/url]" + sText + "精华!";
            if (!string.IsNullOrEmpty(Why))
                strLog += "理由:" + Why + "";
            new BCW.BLL.Text().UpdateIsGood(bid, iText);

            new BCW.BLL.Forumlog().Add(1, forumid, bid, "[url=/bbs/uinfo.aspx?uid=" + model.UsID + "]" + model.UsName + "[/url]的" + strLog);
            new BCW.BLL.Guest().Add(0, model.UsID, model.UsName, "您的" + strLog);
            Utils.Success("" + sText + "精华", "" + sText + "精华成功，正在返回..<br /><a href=\"" + Utils.getUrl("textmanage.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">继续管理</a>", Utils.getPage("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + ""), "3");
        }
    }

    private void RecomPage(string act, int uid, int forumid, int bid)
    {
        string sText = string.Empty;
        int iText = 0;
        BCW.Model.Text model = new BCW.BLL.Text().GetTextMe(bid);
        if (act == "recom")
        {
            sText = "加为";
            iText = 1;
            new BCW.User.Role().CheckUserRole(BCW.User.Role.enumRole.Role_RecomText, uid, forumid);

            if (model.IsRecom == 1)
                Utils.Error("此帖已加为推荐", "");
        }
        else
        {
            sText = "解除";
            iText = 0;
            new BCW.User.Role().CheckUserRole(BCW.User.Role.enumRole.Role_DelRecomText, uid, forumid);

            if (model.IsRecom == 0)
                Utils.Error("此帖已解除推荐", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info != "ok")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定" + sText + "推荐吗");
            builder.Append(Out.Tab("</div>", ""));

            strText = "理由:,,,,,";
            strName = "Why,forumid,bid,act,info,backurl";
            strType = "text,hidden,hidden,hidden,hidden,hidden";
            strValu = "'" + forumid + "'" + bid + "'" + act + "'ok'" + Utils.getPage(0) + "";
            strEmpt = "true,false,false,false,false,false";
            strIdea = "/";
            strOthe = "确定" + sText + ",textmanage.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append(" <a href=\"" + Utils.getUrl("textmanage.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">取消</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            //管理安全提示
            string[] p_pageArr = { "act", "forumid", "bid", "Why", "info", "backurl" };
            BCW.User.Role.AdminSafePage(uid, Utils.getPageUrl(), p_pageArr, "post");

            new BCW.BLL.Text().UpdateIsRecom(bid, iText);
            //记录日志
            //积分操作/论坛统计/圈子论坛不进行任何奖励
            int GroupId = new BCW.BLL.Forum().GetGroupId(forumid);
            if (GroupId == 0)
            {
                if (iText == 1)
                    new BCW.User.Cent().UpdateCent(BCW.User.Cent.enumRole.Cent_RecomText, model.UsID);
                else
                    new BCW.User.Cent().UpdateCent(BCW.User.Cent.enumRole.Cent_DelRecomText, model.UsID);
            }
            string Why = Utils.GetRequest("Why", "post", 3, @"^[^\^]{1,20}$", "理由限20字内，可留空");
            string strLog = "主题[url=/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "]《" + model.Title + "》[/url]被[url=/bbs/uinfo.aspx?uid=" + uid + "]" + new BCW.BLL.User().GetUsName(uid) + "[/url]" + sText + "推荐!";
            if (!string.IsNullOrEmpty(Why))
                strLog += "理由:" + Why + "";
            new BCW.BLL.Forumlog().Add(2, forumid, bid, "[url=/bbs/uinfo.aspx?uid=" + model.UsID + "]" + model.UsName + "[/url]的" + strLog);
            new BCW.BLL.Guest().Add(0, model.UsID, model.UsName, "您的" + strLog);
            Utils.Success("" + sText + "推荐", "" + sText + "推荐成功，正在返回..<br /><a href=\"" + Utils.getUrl("textmanage.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">继续管理</a>", Utils.getPage("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + ""), "3");
        }
    }

    private void TopPage(string act, int uid, int forumid, int bid)
    {
        bool IsSuper = false;//是否总版权限
        if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_TopText, uid))
        {
            IsSuper = true;
        }
        string sText = string.Empty;
        if (act == "top")
        {
            sText = "设置";
            new BCW.User.Role().CheckUserRole(BCW.User.Role.enumRole.Role_TopText, uid, forumid);
        }
        else
        {
            sText = "去掉";
            new BCW.User.Role().CheckUserRole(BCW.User.Role.enumRole.Role_DelTopText, uid, forumid);
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定" + sText + "置顶吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            if (IsSuper)
            {
                if (act == "top")
                {
                    builder.Append("<a href=\"" + Utils.getUrl("textmanage.aspx?act=" + act + "&amp;info=ok&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">本版置顶</a><br />");
                    builder.Append("<a href=\"" + Utils.getUrl("textmanage.aspx?act=" + act + "&amp;info=ok2&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">全区置顶</a><br />");
                }
                else
                {
                    builder.Append("<a href=\"" + Utils.getUrl("textmanage.aspx?act=" + act + "&amp;info=ok3&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定" + sText + "</a><br />");
                }
            }
            else
            {
                if (act == "top")
                {
                    builder.Append("<a href=\"" + Utils.getUrl("textmanage.aspx?act=" + act + "&amp;info=ok&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定" + sText + "</a><br />");
                }
                else
                {
                    builder.Append("<a href=\"" + Utils.getUrl("textmanage.aspx?act=" + act + "&amp;info=ok3&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定" + sText + "</a><br />");
                }
            }
            builder.Append("<a href=\"" + Utils.getUrl("textmanage.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            //管理安全提示
            string[] p_pageArr = { "act", "forumid", "bid", "Why", "info", "backurl" };
            BCW.User.Role.AdminSafePage(uid, Utils.getPageUrl(), p_pageArr, "get");

            //得到置顶类型
            int topType = new BCW.BLL.Text().GetIsTop(bid);
            if (info == "ok")
            {
                if (topType > 0)
                {
                    Utils.Error("此帖已置顶", "");
                }
                new BCW.BLL.Text().UpdateIsTop(bid, 1);
            }
            else if (info == "ok2")
            {
                if (topType == 2)
                {
                    Utils.Error("此帖已全版区置顶", "");
                }
                if (!IsSuper)
                {
                    Utils.Error("权限不足，不能操作全版区置顶", "");
                }
                new BCW.BLL.Text().UpdateIsTop(bid, 2);
                sText = "设置全版区";
            }
            else
            {
                if (topType == 2 && IsSuper == false)
                {
                    Utils.Error("权限不足，不能操作去掉全版区置顶帖", "");
                }
                if (topType == 2)
                {
                    sText = "去掉全版区";
                }
                new BCW.BLL.Text().UpdateIsTop(bid, 0);

            }

            //加币与扣币(无)

            //记录日志
            BCW.Model.Text model = new BCW.BLL.Text().GetTextMe(bid);
            string Why = Utils.GetRequest("Why", "post", 3, @"^[^\^]{1,20}$", "理由限20字内，可留空");
            string strLog = "主题[url=/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "]《" + model.Title + "》[/url]被[url=/bbs/uinfo.aspx?uid=" + uid + "]" + new BCW.BLL.User().GetUsName(uid) + "[/url]" + sText + "置顶!";
            if (!string.IsNullOrEmpty(Why))
                strLog += "理由:" + Why + "";
            new BCW.BLL.Forumlog().Add(3, forumid, bid, "[url=/bbs/uinfo.aspx?uid=" + model.UsID + "]" + model.UsName + "[/url]的" + strLog);
            new BCW.BLL.Guest().Add(0, model.UsID, model.UsName, "您的" + strLog);
            Utils.Success("" + sText + "置顶", "" + sText + "置顶成功，正在返回..<br /><a href=\"" + Utils.getUrl("textmanage.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">继续管理</a>", Utils.getPage("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + ""), "3");
        }
    }

    private void Top2Page(string act, int uid, int forumid, int bid)
    {
        string sText = string.Empty;
        int iText = 0;
        if (act == "top2")
        {
            sText = "设置";
            iText = -1;
            new BCW.User.Role().CheckUserRole(BCW.User.Role.enumRole.Role_Top2Text, uid, forumid);
        }
        else
        {
            sText = "去掉";
            iText = 0;
            new BCW.User.Role().CheckUserRole(BCW.User.Role.enumRole.Role_DelTop2Text, uid, forumid);
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info != "ok")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定" + sText + "固底吗");
            builder.Append(Out.Tab("</div>", ""));

            strText = "理由:,,,,,";
            strName = "Why,forumid,bid,act,info,backurl";
            strType = "text,hidden,hidden,hidden,hidden,hidden";
            strValu = "'" + forumid + "'" + bid + "'" + act + "'ok'" + Utils.getPage(0) + "";
            strEmpt = "true,false,false,false,false,false";
            strIdea = "/";
            strOthe = "确定" + sText + ",textmanage.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append(" <a href=\"" + Utils.getUrl("textmanage.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">取消</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            //管理安全提示
            string[] p_pageArr = { "act", "forumid", "bid", "Why", "info", "backurl" };
            BCW.User.Role.AdminSafePage(uid, Utils.getPageUrl(), p_pageArr, "post");

            //得到置顶类型
            int topType = new BCW.BLL.Text().GetIsTop(bid);
            if (topType > 0)
            {
                Utils.Error("此帖已置顶,请去顶后再进行固底", "");
            }
            new BCW.BLL.Text().UpdateIsTop(bid, iText);
            //记录日志
            BCW.Model.Text model = new BCW.BLL.Text().GetTextMe(bid);
            int GroupId = new BCW.BLL.Forum().GetGroupId(forumid);
            if (GroupId == 0)
            {
                //积分操作
                if (iText == -1)
                    new BCW.User.Cent().UpdateCent(BCW.User.Cent.enumRole.Cent_Top2Text, model.UsID);
            }
            string Why = Utils.GetRequest("Why", "post", 3, @"^[^\^]{1,20}$", "理由限20字内，可留空");
            string strLog = "主题[url=/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "]《" + model.Title + "》[/url]被[url=/bbs/uinfo.aspx?uid=" + uid + "]" + new BCW.BLL.User().GetUsName(uid) + "[/url]" + sText + "固底!";
            if (!string.IsNullOrEmpty(Why))
                strLog += "理由:" + Why + "";
            new BCW.BLL.Forumlog().Add(4, forumid, bid, "[url=/bbs/uinfo.aspx?uid=" + model.UsID + "]" + model.UsName + "[/url]的" + strLog);
            new BCW.BLL.Guest().Add(0, model.UsID, model.UsName, "您的" + strLog);
            Utils.Success("" + sText + "固底", "" + sText + "固底成功，正在返回..<br /><a href=\"" + Utils.getUrl("textmanage.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">继续管理</a>", Utils.getPage("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + ""), "3");
        }
    }

    private void LockPage(string act, int uid, int forumid, int bid)
    {
        string sText = string.Empty;
        int iText = 0;
        if (act == "lock")
        {
            sText = "设为";
            iText = 1;
            new BCW.User.Role().CheckUserRole(BCW.User.Role.enumRole.Role_LockText, uid, forumid);
        }
        else
        {
            sText = "解除";
            iText = 0;
            new BCW.User.Role().CheckUserRole(BCW.User.Role.enumRole.Role_DelLockText, uid, forumid);
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info != "ok")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定" + sText + "锁定吗");
            builder.Append(Out.Tab("</div>", ""));

            if (iText == 1)
            {
                strText = "选择理由:/,其它原因:/,,,,,";
                strName = "IsWhy,Why,forumid,bid,act,info,backurl";
                strType = "select,text,hidden,hidden,hidden,hidden,hidden";
                strValu = "0''" + forumid + "'" + bid + "'" + act + "'ok'" + Utils.getPage(0) + "";
                strEmpt = "0|其它原因|1|诽谤攻击他人|2|发布不良内容|3|不符合本版主题|4|问题已处理完毕|5|回帖已说明原因,true,false,false,false,false,false";
                strIdea = "/提示:选择其它原因时请填写内容/";
            }
            else
            {
                strText = "理由:,,,,,";
                strName = "Why,forumid,bid,act,info,backurl";
                strType = "text,hidden,hidden,hidden,hidden,hidden";
                strValu = "'" + forumid + "'" + bid + "'" + act + "'ok'" + Utils.getPage(0) + "";
                strEmpt = "true,false,false,false,false,false";
                strIdea = "/";
            }

            strOthe = "确定" + sText + ",textmanage.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(" <a href=\"" + Utils.getUrl("textmanage.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">取消</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            //管理安全提示
            string[] p_pageArr = { "act", "forumid", "bid", "Why", "info", "backurl", "IsWhy" };
            BCW.User.Role.AdminSafePage(uid, Utils.getPageUrl(), p_pageArr, "post");

            //得到锁定类型
            int lockType = new BCW.BLL.Text().GetIsLock(bid);
            if (act == "lock")
            {
                if (lockType == 1)
                {
                    Utils.Error("此帖已锁定", "");
                }
            }
            else
            {
                if (lockType == 0)
                {
                    Utils.Error("此帖已解锁", "");
                }
            }
            string Why = string.Empty;
            int IsWhy = int.Parse(Utils.GetRequest("IsWhy", "post", 1, @"^[0-5]$", "0"));
            if (IsWhy == 1)
                Why = "诽谤攻击他人";
            else if (IsWhy == 2)
                Why = "发布不良内容";
            else if (IsWhy == 3)
                Why = "不符合本版主题";
            else if (IsWhy == 4)
                Why = "问题已处理完毕";
            else if (IsWhy == 5)
                Why = "回帖已说明原因";
            else
                Why = Utils.GetRequest("Why", "post", 3, @"^[\s\S]{1,20}$", "理由限20字内，可留空");

            new BCW.BLL.Text().UpdateIsLock(bid, iText);

            //记录日志
            BCW.Model.Text model = new BCW.BLL.Text().GetTextMe(bid);
            int GroupId = new BCW.BLL.Forum().GetGroupId(forumid);
            if (GroupId == 0)
            {
                //积分操作
                if (iText == -1)
                    new BCW.User.Cent().UpdateCent(BCW.User.Cent.enumRole.Cent_LockText, model.UsID);
            }
            string strLog = "主题[url=/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "]《" + model.Title + "》[/url]被[url=/bbs/uinfo.aspx?uid=" + uid + "]" + new BCW.BLL.User().GetUsName(uid) + "[/url]" + sText + "锁定!";
            if (!string.IsNullOrEmpty(Why))
                strLog += "理由:" + Why + "";
            new BCW.BLL.Forumlog().Add(5, forumid, bid, "[url=/bbs/uinfo.aspx?uid=" + model.UsID + "]" + model.UsName + "[/url]的" + strLog);
            new BCW.BLL.Guest().Add(0, model.UsID, model.UsName, "您的" + strLog);
            Utils.Success("" + sText + "锁定", "" + sText + "锁定成功，正在返回..<br /><a href=\"" + Utils.getUrl("textmanage.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">继续管理</a>", Utils.getPage("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + ""), "3");
        }
    }

    private void FlowPage(string act, int uid, int forumid, int bid)
    {

        int IsFlow = new BCW.BLL.Text().GetIsFlow(bid);
        if (IsFlow == 2)
        {
            if (!IsGdID(uid))
            {
                Utils.Error("此滚动属滚动管理员设置，你无法撤除", "");
            }
        }
        else if (IsFlow == 0)
        {
            if (IsGdID(uid))
            {
                Utils.Error("你的权限不足", "");
            }
        }
        new BCW.User.Role().CheckUserRole(BCW.User.Role.enumRole.Role_TopicsSet, uid, forumid);
        string sText = string.Empty;
        int iText = 0;
        if (act == "flow")
        {
            sText = "设为";
            iText = 1;

        }
        else
        {
            sText = "撤除";
            iText = 0;
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info != "ok")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定" + sText + "滚动吗");
            builder.Append(Out.Tab("</div>", ""));

            strText = "理由:,,,,,";
            strName = "Why,forumid,bid,act,info,backurl";
            strType = "text,hidden,hidden,hidden,hidden,hidden";
            strValu = "'" + forumid + "'" + bid + "'" + act + "'ok'" + Utils.getPage(0) + "";
            strEmpt = "true,false,false,false,false,false";
            strIdea = "/";
            strOthe = "确定" + sText + ",textmanage.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append(" <a href=\"" + Utils.getPage("textmanage.aspx?forumid=" + forumid + "&amp;bid=" + bid + "") + "\">取消</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            //管理安全提示
            string[] p_pageArr = { "act", "forumid", "bid", "Why", "info", "backurl" };
            BCW.User.Role.AdminSafePage(uid, Utils.getPageUrl(), p_pageArr, "post");

            new BCW.BLL.Text().UpdateIsFlow(bid, iText);
            //记录日志
            BCW.Model.Text model = new BCW.BLL.Text().GetTextMe(bid);
            string Why = Utils.GetRequest("Why", "post", 3, @"^[^\^]{1,20}$", "理由限20字内，可留空");
            string strLog = "主题[url=/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "]《" + model.Title + "》[/url]被[url=/bbs/uinfo.aspx?uid=" + uid + "]" + new BCW.BLL.User().GetUsName(uid) + "[/url]" + sText + "滚动!";
            if (!string.IsNullOrEmpty(Why))
                strLog += "理由:" + Why + "";
            new BCW.BLL.Forumlog().Add(2, forumid, bid, "[url=/bbs/uinfo.aspx?uid=" + model.UsID + "]" + model.UsName + "[/url]的" + strLog);
            new BCW.BLL.Guest().Add(0, model.UsID, model.UsName, "您的" + strLog);
            Utils.Success("" + sText + "滚动", "" + sText + "滚动成功，正在返回..<br /><a href=\"" + Utils.getUrl("textmanage.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">继续管理</a>", Utils.getPage("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + ""), "3");
        }
    }


    private void FlowSetPage(string act, int uid, int forumid, int bid)
    {

        if (!IsGdID(uid))
        {
            Utils.Error("你的权限不足", "");
        }
        BCW.Model.Text model = new BCW.BLL.Text().GetText(bid);
        if (model == null || model.ForumId != forumid)
        {
            Utils.Error("不存在的主题", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {

            int day = int.Parse(Utils.GetRequest("day", "post", 2, @"^[1-9]\d*$", "天数填写错误"));
            string Why = Utils.GetRequest("Why", "post", 3, @"^[^\^]{1,20}$", "理由限20字内，可留空");
            int Include = int.Parse(Utils.GetRequest("Include", "post", 2, @"^[1-2]$", "性质填写错误"));
            string IncText = string.Empty;
            if (Include == 2)
                IncText = "全区";

            //管理安全提示
            string[] p_pageArr = { "act", "forumid", "bid", "Why", "info", "backurl", "Include", "day" };
            BCW.User.Role.AdminSafePage(uid, Utils.getPageUrl(), p_pageArr, "post");


            DateTime dt = DateTime.Now.AddDays(day);
            new BCW.BLL.Text().UpdateFlowTime(bid, dt, Include);
            string strLog = "主题[url=/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "]《" + model.Title + "》[/url]被[url=/bbs/uinfo.aspx?uid=" + uid + "]" + new BCW.BLL.User().GetUsName(uid) + "[/url]设为" + IncText + "滚动!";
            if (!string.IsNullOrEmpty(Why))
                strLog += "理由:" + Why + "";

            new BCW.BLL.Forumlog().Add(2, forumid, bid, "[url=/bbs/uinfo.aspx?uid=" + model.UsID + "]" + model.UsName + "[/url]的" + strLog);
            new BCW.BLL.Guest().Add(0, model.UsID, model.UsName, "您的" + strLog);

            Utils.Success("设置滚动", "设置" + IncText + "滚动成功！", Utils.getPage("textmanage.aspx?forumid=" + forumid + "&amp;bid=" + bid + ""), "1");

        }

        Master.Title = "设置社区滚动";
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-1]$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("主题:<a href=\"" + Utils.getUrl("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "") + "\">" + model.Title + "</a>");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "理由:/,性质:/,滚动天数(当全区滚动时才有效):/,,,,";
        string strName = "Why,Include,day,forumid,bid,act,info";
        string strType = "text,select,snum,hidden,hidden,hidden,hidden";
        string strValu = "'2'5'" + forumid + "'" + bid + "'flowset'ok";
        string strEmpt = "true,1|版块滚动|2|全区滚动,false,false,false,false,false";
        string strIdea = "''天''''|/";
        string strOthe = "确定设置,textmanage.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append(" <a href=\"" + Utils.getPage("textmanage.aspx?forumid=" + forumid + "&amp;bid=" + bid + "") + "\">取消</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    /// <summary>
    /// 修改转移帖子论坛统计问题
    /// 陈志基 20160816
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="forumid"></param>
    /// <param name="bid"></param>
    private void MovePage(int uid, int forumid, int bid)
    {
        new BCW.User.Role().CheckUserRole(BCW.User.Role.enumRole.Role_MoveText, uid, forumid);

        int newid = int.Parse(Utils.GetRequest("newid", "get", 1, @"^[1-9]\d*$", "0"));
        if (newid > 0)
        {
            if (!new BCW.BLL.Forum().Exists2(newid))
            {
                Utils.Success("选择版块", "该版块不存在或已暂停使用", Utils.getUrl("textmanage.aspx?act=move&amp;forumid=" + forumid + "&amp;bid=" + bid + ""), "1");
            }
            if (newid.Equals(forumid))
            {
                Utils.Error("不能转至同一个版块", "");
            }

            //管理安全提示
            string[] p_pageArr = { "act", "forumid", "bid", "newid", "backurl" };
            BCW.User.Role.AdminSafePage(uid, Utils.getPageUrl(), p_pageArr, "get");

            BCW.Model.Text model = new BCW.BLL.Text().GetText(bid);
            //去掉精华和推荐再转移
            if (model.IsGood == 1)
            {
                new BCW.BLL.Text().UpdateIsGood(bid, 0);
            }
            if (model.IsRecom == 1)
            {
                new BCW.BLL.Text().UpdateIsRecom(bid, 0);
            }
            //转移
            new BCW.BLL.Text().UpdateForumID(bid, newid);
            //重新进行原论坛回复统计
            DataSet ds = new BCW.BLL.Reply().GetList("ID,AddTime,UsID,IsDel,UsName", "forumid=" + forumid + " and bid=" + bid + "");
            {
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (int.Parse(ds.Tables[0].Rows[i]["IsDel"].ToString()) == 0)//如果回帖没有删除
                        {
                            //减少对旧论坛回帖用户的回帖统计
                            new BCW.BLL.Forumstat().Update2(2, int.Parse(ds.Tables[0].Rows[i]["UsID"].ToString()), forumid, DateTime.Parse(ds.Tables[0].Rows[i]["AddTime"].ToString()));
                            //增加对新论坛回帖用户的回帖统计
                            new BCW.BLL.Forumstat().Update3(2, int.Parse(ds.Tables[0].Rows[i]["UsID"].ToString()), newid, DateTime.Parse(ds.Tables[0].Rows[i]["AddTime"].ToString()));
                           // BCW.User.Users.UpdateForumStat(2, int.Parse(ds.Tables[0].Rows[i]["UsID"].ToString()), ds.Tables[0].Rows[i]["UsName"].ToString(), newid);
                        }
                        //更新回帖中的论坛ID
                          new BCW.BLL.Reply().UpdateForumID(int.Parse(ds.Tables[0].Rows[i]["ID"].ToString()), newid);
                       // new BCW.BLL.Forumstat().Update2(2, model.UsID, forumid, DateTime.Parse(ds.Tables[0].Rows[i]["AddTime"].ToString()));
                       // BCW.User.Users.UpdateForumStat(2, model.UsID, model.UsName, newid);
                       // new BCW.BLL.Reply().UpdateForumID(int.Parse(ds.Tables[0].Rows[i]["ID"].ToString()), newid);
                    }
                }
            }
            //转移的原论坛帖子统计
            new BCW.BLL.Forumstat().Update2(1, model.UsID, forumid, model.AddTime);
            //转移的新论坛帖子统计
           // BCW.User.Users.UpdateForumStat(1, model.UsID, model.UsName, newid);
            new BCW.BLL.Forumstat().Update3(1, model.UsID, newid, model.AddTime);
            //记录日志

            string Why = Utils.GetRequest("Why", "post", 3, @"^[^\^]{1,20}$", "理由限20字内，可留空");
            string strLog = "主题[url=/bbs/topic.aspx?forumid=" + newid + "&amp;bid=" + bid + "]《" + model.Title + "》[/url]被[url=/bbs/uinfo.aspx?uid=" + uid + "]" + new BCW.BLL.User().GetUsName(uid) + "[/url]转移到[url=/bbs/forum.aspx?forumid=" + newid + "]" + new BCW.BLL.Forum().GetTitle(newid) + "[/url]!";
            string strLog2 = "主题[url=/bbs/topic.aspx?forumid=" + newid + "&amp;bid=" + bid + "]《" + model.Title + "》[/url]被[url=/bbs/uinfo.aspx?uid=" + uid + "]" + new BCW.BLL.User().GetUsName(uid) + "[/url]从[url=/bbs/forum.aspx?forumid=" + forumid + "]" + new BCW.BLL.Forum().GetTitle(forumid) + "[/url]转移到本坛!";
            if (!string.IsNullOrEmpty(Why))
                strLog += "理由:" + Why + "";
            new BCW.BLL.Forumlog().Add(8, forumid, bid, "[url=/bbs/uinfo.aspx?uid=" + model.UsID + "]" + model.UsName + "[/url]的" + strLog);
            new BCW.BLL.Forumlog().Add(8, newid, bid, "[url=/bbs/uinfo.aspx?uid=" + model.UsID + "]" + model.UsName + "[/url]的" + strLog2);
            new BCW.BLL.Guest().Add(0, model.UsID, model.UsName, "您的" + strLog);
            Utils.Success("转移帖子", "转移成功，正在返回..<br /><a href=\"" + Utils.getUrl("textmanage.aspx?forumid=" + newid + "&amp;bid=" + bid + "") + "\">继续管理</a>", Utils.getUrl("topic.aspx?forumid=" + newid + "&amp;bid=" + bid + ""), "3");
        }

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("请选择转移到的版块:");
        builder.Append(Out.Tab("</div>", Out.Hr()));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("当前:<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "") + "\">" + new BCW.BLL.Forum().GetTitle(forumid) + "</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = 5;
        string strWhere = string.Empty;
        string strOrder = string.Empty;
        string[] pageValUrl = { "act", "forumid", "bid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        strWhere = "id<>" + forumid + " and IsActive=0";

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

                //黄国军修改 20160203
                builder.Append(k + ":<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + n.ID + "") + "\">" + n.Title + "</a>.");
                builder.Append("<a href=\"" + Utils.getUrl("textmanage.aspx?act=move&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;newid=" + n.ID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[移]</a>");
                k++;
                builder.Append(Out.Tab("</div>", ""));

            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("text", "没有相关记录"));
        }

        builder.Append(Out.Tab("<div>", ""));
        builder.Append(" <a href=\"" + Utils.getUrl("textmanage.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">取消</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "") + "\">主题帖子</a>");
        builder.Append(Out.Tab("</div>", ""));

    }

    private void ForumtsPage(string act, int uid, int forumid, int bid)
    {
        new BCW.User.Role().CheckUserRole(BCW.User.Role.enumRole.Role_TopicsSet, uid, forumid);
        Master.Title = "设置帖子到专题";
        BCW.Model.Text model = new BCW.BLL.Text().GetText(bid);
        int id = int.Parse(Utils.GetRequest("id", "get", 1, @"^[0-9]\d*$", "-1"));
        if (id > -1)
        {

            //管理安全提示
            string[] p_pageArr = { "act", "forumid", "bid", "id", "backurl" };
            BCW.User.Role.AdminSafePage(uid, Utils.getPageUrl(), p_pageArr, "get");

            string sName = "移出";
            string ForumtsName = string.Empty;
            if (id > 0)
            {
                ForumtsName = new BCW.BLL.Forumts().GetTitle(id, forumid);
                if (ForumtsName == "")
                {
                    Utils.Error("不存在的专题记录", "");
                }
                sName = "加入";
                //积分操作
                //new BCW.User.Cent().UpdateCent(BCW.User.Cent.enumRole.Cent_TopicsText, model.UsID);
            }
            else
            {
                ForumtsName = new BCW.BLL.Forumts().GetTitle(model.TsID, forumid);
                //积分操作
                //new BCW.User.Cent().UpdateCent(BCW.User.Cent.enumRole.Cent_TopicsText, model.UsID);
            }
            new BCW.BLL.Text().UpdateTsID(bid, id);


            //记录日志
            string strLog = "主题[url=/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "]《" + model.Title + "》[/url]被[url=/bbs/uinfo.aspx?uid=" + uid + "]" + new BCW.BLL.User().GetUsName(uid) + "[/url]" + sName + "[url=/bbs/forumts.aspx?act=view&amp;id=" + id + "&amp;forumid=" + forumid + "]" + ForumtsName + "[/url]专题!";
            new BCW.BLL.Forumlog().Add(9, forumid, bid, "[url=/bbs/uinfo.aspx?uid=" + model.UsID + "]" + model.UsName + "[/url]的" + strLog);
            new BCW.BLL.Guest().Add(0, model.UsID, model.UsName, "您的" + strLog);
            Utils.Success("" + sName + "专题", "" + sName + "专题成功，正在返回..<br /><a href=\"" + Utils.getUrl("textmanage.aspx?forumid=" + forumid + "&amp;bid=" + bid + "") + "\">继续管理</a>", Utils.getUrl("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + ""), "3");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("请选择设置到的专题");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "forumid", "bid", "backurl" };
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
                builder.Append("<a href=\"" + Utils.getUrl("forumts.aspx?act=view&amp;forumid=" + forumid + "&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + ((pageIndex - 1) * pageSize + k) + "." + n.Title + "</a>(专题ID:" + n.ID + ")");
                builder.Append("<a href=\"" + Utils.getUrl("textmanage.aspx?act=forumts&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[选择]</a>");
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
        if (model.TsID > 0)
            builder.Append("<a href=\"" + Utils.getUrl("textmanage.aspx?act=forumts&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;id=0&amp;backurl=" + Utils.getPage(0) + "") + "\">移出专题</a><br />");

        builder.Append(" <a href=\"" + Utils.getUrl("textmanage.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">取消</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "") + "\">主题帖子</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void EditPage(int uid, int forumid, int bid)
    {
        Master.Title = "编辑帖子";
        BCW.Model.Text model = new BCW.BLL.Text().GetText(bid);
        if (model.UsID != uid && new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_EditText, uid, forumid) == false)
        {
            Utils.Error("你的权限不足", "");
        }

        if (model.Types == 8)
        {
            DateTime GsStopTime = Utils.ParseTime(ub.GetSub("BbsGsStopTime", xmlPath));
            int GsqiNum = Utils.ParseInt(ub.GetSub("BbsGsqiNum", xmlPath));
            if (GsqiNum == 0)
            {
                Utils.Error("本期尚未设置", "");
            }
            if (GsStopTime < DateTime.Now)
            {
                Utils.Error("本期第" + GsqiNum + "期已截止发表，截止时间" + GsStopTime + "", "");
            }
        }
        //if (model.IsGood == 1)
        //{
        //    Utils.Error("本帖子已加精，不能编辑", "");
        //}
        //if (model.IsRecom == 1)
        //{
        //    Utils.Error("本帖子已推荐，不能编辑", "");
        //}
        if (model.IsLock == 1)
        {
            Utils.Error("本帖子已锁定，不能编辑", "");
        }
        if (model.IsTop == -1)
        {
            Utils.Error("本帖子已固底，不能编辑", "");
        }

        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info != "ok")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("编辑：<a href=\"" + Utils.getUrl("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "") + "\">" + model.Title + "</a>");
            builder.Append(Out.Tab("</div>", ""));

            strText = "主题(" + ub.GetSub("BbsThreadMax", xmlPath) + "字内):,内容(" + ub.GetSub("BbsContentMax", xmlPath) + "字内):/,,,,,";
            strName = "Title,Content,forumid,bid,act,info,backurl";
            strType = "text,big,hidden,hidden,hidden,hidden,hidden";
            strValu = "" + model.Title + "'" + model.Content + "'" + forumid + "'" + bid + "'edit'ok'" + Utils.getPage(0) + "";
            strEmpt = "false,false,false,false,false,false,false";
            strIdea = "/";
            strOthe = "确定编辑|reset,textmanage.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(" <a href=\"" + Utils.getUrl("textmanage.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">取消</a>");
            builder.Append(Out.Tab("</div>", ""));
            if (!Utils.Isie())
            {
                builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
                string VE = ConfigHelper.GetConfigString("VE");
                string SID = ConfigHelper.GetConfigString("SID");
                builder.Append("<a href=\"textmanage.aspx?act=edit&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "&amp;" + VE + "=20a&amp;" + SID + "=" + Utils.getstrU() + "\">切换彩版编辑&gt;&gt;</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getPage("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "") + "\">主题帖子</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            string Title = Utils.GetRequest("Title", "post", 2, @"^[\s\S]{1," + ub.GetSub("BbsThreadMax", xmlPath) + "}$", "标题限1-50字");
            string Content = Utils.GetRequest("Content", "post", 2, @"^[\s\S]{1," + ub.GetSub("BbsContentMax", xmlPath) + "}$", "请输入不超" + ub.GetSub("BbsContentMax", xmlPath) + "字的内容");


            //管理安全提示
            if (model.UsID != uid)
            {
                string[] p_pageArr = { "act", "forumid", "bid", "info", "Title", "Content", "backurl" };
                BCW.User.Role.AdminSafePage(uid, Utils.getPageUrl(), p_pageArr, "post");
            }

            BCW.Model.Text model2 = new BCW.Model.Text();
            model2.ID = bid;
            model2.Title = Title;
            model2.Content = Content;
            new BCW.BLL.Text().Update(model2);
            //记录日志
            string strLog = string.Empty;
            if (model.UsID != uid)
            {
                strLog = "[url=/bbs/uinfo.aspx?uid=" + model.UsID + "]" + model.UsName + "[/url]的主题[url=/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "]《" + model.Title + "》[/url]被[url=/bbs/uinfo.aspx?uid=" + uid + "]" + new BCW.BLL.User().GetUsName(uid) + "[/url]编辑!";
                new BCW.BLL.Guest().Add(0, model.UsID, model.UsName, "您的主题[url=/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "]《" + model.Title + "》[/url]被[url=/bbs/uinfo.aspx?uid=" + uid + "]" + new BCW.BLL.User().GetUsName(uid) + "[/url]编辑!");
            }
            else
            {
                strLog = "[url=/bbs/uinfo.aspx?uid=" + model.UsID + "]" + model.UsName + "[/url]编辑自己的主题[url=/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "]《" + model.Title + "》[/url]!";
            }
            new BCW.BLL.Forumlog().Add(7, forumid, bid, strLog);

            Utils.Success("编辑帖子", "编辑成功，正在返回..<br /><a href=\"" + ReplaceWap(Utils.getUrl("textmanage.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "")) + "\">继续管理</a>", ReplaceWap(Utils.getUrl("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "")), "3");

        }
    }

    private void OpEditPage(string act, int uid, int forumid, int bid)
    {
        Master.Title = "分段编辑内容";
        BCW.Model.Text model = new BCW.BLL.Text().GetText(bid);
        if (model.UsID != uid && new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_EditText, uid, forumid) == false)
        {
            Utils.Error("你的权限不足", "");
        }

        if (model.Types == 8)
        {
            DateTime GsStopTime = Utils.ParseTime(ub.GetSub("BbsGsStopTime", xmlPath));
            int GsqiNum = Utils.ParseInt(ub.GetSub("BbsGsqiNum", xmlPath));
            if (GsqiNum == 0)
            {
                Utils.Error("本期尚未设置", "");
            }
            if (GsStopTime < DateTime.Now)
            {
                Utils.Error("本期第" + GsqiNum + "期已截止发表，截止时间" + GsStopTime + "", "");
            }
        }
        //if (model.IsGood == 1)
        //{
        //    Utils.Error("本帖子已加精，不能编辑", "");
        //}
        //if (model.IsRecom == 1)
        //{
        //    Utils.Error("本帖子已推荐，不能编辑", "");
        //}
        if (model.IsLock == 1)
        {
            Utils.Error("本帖子已锁定，不能编辑", "");
        }
        if (model.IsTop == -1)
        {
            Utils.Error("本帖子已固底，不能编辑", "");
        }

        int pageIndex;
        int recordCount;
        int pageSize = 0;
        //取分页字数
        string ForumSet = new BCW.BLL.User().GetForumSet(uid);
        pageSize = BCW.User.Users.GetForumSet(ForumSet, 1);

        string[] pageValUrl = { "act", "forumid", "bid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["vp"]);
        if (pageIndex == 0)
            pageIndex = 1;

        string info = Utils.GetRequest("info", "all", 1, "", "");
        int pover = int.Parse(Utils.GetRequest("pover", "all", 1, @"^[0-9]\d*$", "0"));
        if (info != "ok")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("编辑：<a href=\"" + Utils.getUrl("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "") + "\">" + model.Title + "</a>");
            builder.Append(Out.Tab("</div>", ""));

            string content = BasePage.MultiContent(model.Content, pageIndex, pageSize, pover, out recordCount);
            strText = "内容:/,,,,,,,";
            strName = "Content,forumid,bid,vp,pover,act,info,backurl";
            strType = "big,hidden,hidden,hidden,hidden,hidden,hidden,hidden";
            strValu = "" + content + "'" + forumid + "'" + bid + "'" + pageIndex + "'" + pover + "'opedit'ok'" + Utils.getPage(0) + "";
            strEmpt = "false,false,false,false,false,false,false,false";
            strIdea = "/";
            strOthe = "确定编辑|reset,textmanage.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(BasePage.MultiContentPage(model.Content, pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "vp", pover));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append(" <a href=\"" + Utils.getUrl("textmanage.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">取消</a>");
            builder.Append(Out.Tab("</div>", ""));
            if (!Utils.Isie())
            {
                builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
                string VE = ConfigHelper.GetConfigString("VE");
                string SID = ConfigHelper.GetConfigString("SID");
                builder.Append("<a href=\"textmanage.aspx?act=edit&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "&amp;" + VE + "=20a&amp;" + SID + "=" + Utils.getstrU() + "\">切换彩版编辑&gt;&gt;</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getPage("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "") + "\">主题帖子</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            //管理安全提示
            if (model.UsID != uid)
            {
                string[] p_pageArr = { "act", "forumid", "bid", "info", "vp", "pover", "Content", "backurl" };
                BCW.User.Role.AdminSafePage(uid, Utils.getPageUrl(), p_pageArr, "post");
            }
            int pageTotal = BasePage.CalcPageCount(model.Content, model.Content.Length, pageSize, ref pageIndex);
            int vp = int.Parse(Utils.GetRequest("vp", "post", 2, @"^[1-9]\d*$", "页码ID出错"));
            if (vp > pageTotal)
                vp = pageTotal;

            string Content = Utils.GetRequest("Content", "post", 1, "", "");
            string strContent = string.Empty;
            if (pover == 0)
            {
                for (int i = 1; i <= pageTotal; i++)
                {
                    if (i == vp)
                        strContent += Content;
                    else
                        strContent += BasePage.MultiContent(model.Content, i, pageSize, pover, out recordCount);
                }
            }
            else
            {
                strContent = Utils.Mid(model.Content, 0, Convert.ToInt32((vp - 1) * pageSize)) + Content;
            
            }
            if (strContent.Length > Convert.ToInt32(ub.GetSub("BbsContentMax", xmlPath)))
            {
                Utils.Error("字数已超出" + ub.GetSub("BbsContentMax", xmlPath) + "字，无法编辑", "");
            }
            new BCW.BLL.Text().UpdateContent(bid, strContent);
            //记录日志
            string strLog = string.Empty;
            if (model.UsID != uid)
            {
                strLog = "[url=/bbs/uinfo.aspx?uid=" + model.UsID + "]" + model.UsName + "[/url]的主题[url=/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "]《" + model.Title + "》[/url]被[url=/bbs/uinfo.aspx?uid=" + uid + "]" + new BCW.BLL.User().GetUsName(uid) + "[/url]编辑!";
                new BCW.BLL.Guest().Add(0, model.UsID, model.UsName, "您的主题[url=/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "]《" + model.Title + "》[/url]被[url=/bbs/uinfo.aspx?uid=" + uid + "]" + new BCW.BLL.User().GetUsName(uid) + "[/url]编辑!");
            }
            else
            {
                strLog = "[url=/bbs/uinfo.aspx?uid=" + model.UsID + "]" + model.UsName + "[/url]编辑自己的主题[url=/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "]《" + model.Title + "》[/url]!";
            }
            new BCW.BLL.Forumlog().Add(7, forumid, bid, strLog);
            Utils.Success("编辑帖子", "编辑成功，正在返回..<br /><a href=\"" + ReplaceWap(Utils.getUrl("textmanage.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "")) + "\">继续管理</a>", ReplaceWap(Utils.getUrl("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "")), "3");

        }
    }

    private void FillPage(int uid, int forumid, int bid)
    {
        Master.Title = "续写帖子";
        BCW.Model.Text model = new BCW.BLL.Text().GetTextMe(bid);
        if (model.UsID != uid)
        {
            Utils.Error("不存在的记录", "");
        }
        //计算可续写字数
        int ContentLength = Convert.ToInt32(ub.GetSub("BbsContentMax", xmlPath)) - model.Content.Length;
        if (ContentLength < 0)
            ContentLength = 0;

        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info != "ok")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("续写：<a href=\"" + Utils.getUrl("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "") + "\">" + model.Title + "</a>");
            builder.Append(Out.Tab("</div>", ""));


            strText = "续写内容(可续" + ContentLength + "字):/,,,,,";
            strName = "Content,forumid,bid,act,info,backurl";
            strType = "big,hidden,hidden,hidden,hidden,hidden";
            strValu = "[续]'" + forumid + "'" + bid + "'fill'ok'" + Utils.getPage(0) + "";
            strEmpt = "false,false,false,false,false,false";
            strIdea = "/";
            strOthe = "确定续写|reset,textmanage.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(" <a href=\"" + Utils.getUrl("textmanage.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">取消</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getPage("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "") + "\">主题帖子</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {

            string Content = Utils.GetRequest("Content", "post", 2, @"^[\s\S]{1," + ContentLength + "}$", "请输入不超" + ContentLength + "的内容");

            new BCW.BLL.Text().UpdateContent(bid, model.Content + Content);
            //记录日志
            string strLog = string.Empty;
            strLog = "[url=/bbs/uinfo.aspx?uid=" + model.UsID + "]" + model.UsName + "[/url]续写自己的主题[url=/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "]《" + model.Title + "》[/url]!";
            new BCW.BLL.Forumlog().Add(7, forumid, bid, strLog);

            Utils.Success("续写帖子", "续写成功，正在返回..<br /><a href=\"" + Utils.getUrl("textmanage.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">继续管理</a>", Utils.getUrl("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + ""), "3");

        }
    }
    private void OverPage(int uid, int forumid, int bid)
    {
        Master.Title = "结束帖子";
        BCW.Model.Text model = new BCW.BLL.Text().GetTextMe(bid);
        if (model.UsID != uid)
        {
            Utils.Error("不存在的记录", "");
        }

        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info != "ok")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定结束此帖子吗");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("textmanage.aspx?info=ok&amp;act=over&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定结帖</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("textmanage.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            new BCW.BLL.Text().UpdateIsOver(bid, 1);
            //记录日志
            string strLog = string.Empty;
            strLog = "[url=/bbs/uinfo.aspx?uid=" + model.UsID + "]" + model.UsName + "[/url]结束自己的主题[url=/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "]《" + model.Title + "》[/url]!";
            new BCW.BLL.Forumlog().Add(7, forumid, bid, strLog);
            Utils.Success("结束帖子", "结帖成功，帖子不再允许任何人回复，正在返回..<br /><a href=\"" + Utils.getUrl("textmanage.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">继续管理</a>", Utils.getPage("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + ""), "3");
        }
    }

    private static string ReplaceWap(string p_strUrl)
    {
        p_strUrl = p_strUrl.Replace("20a", "1a");

        return p_strUrl;
    }

    /// <summary>
    /// 滚动管理员ID
    /// </summary>
    private bool IsGdID(int meid)
    {
        bool Isvi = false;
        //能够穿透的ID
        string GdID = "#" + ub.GetSub("FreshGdID", "/Controls/fresh.xml") + "#";
        if (GdID.IndexOf("#" + meid + "#") != -1)
        {
            Isvi = true;
        }

        return Isvi;
    }
}