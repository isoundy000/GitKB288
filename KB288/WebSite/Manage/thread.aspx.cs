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
/// <summary>
///陈志基 20160816
/// 修改保存帖子
/// </summary>
public partial class Manage_thread : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "edit":
                EditPage();
                break;
            case "save":
                EditSavePage();
                break;
            case "del":
                DelPage();
                break;
            case "clearrecycl":
                ClearRecyclPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {

        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[0-9]\d*$", "0"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-6]$", "0"));
        string sName = "帖子管理";
        if (ptype == 6)
            sName = "帖子回收站";

        Master.Title = sName;
        if (uid > 0)
        {
            if (!new BCW.BLL.User().Exists(uid))
            {
                Utils.Error("会员ID不存在", "");
            }
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(sName);
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype != 6)
        {
            if (ptype == 0)
                builder.Append("全部|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("thread.aspx?ptype=0") + "\">全部</a>|");
            if (ptype == 1)
                builder.Append("精华|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("thread.aspx?uid=" + uid + "&amp;ptype=1") + "\">精华</a>|");
            if (ptype == 2)
                builder.Append("推荐|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("thread.aspx?uid=" + uid + "&amp;ptype=2") + "\">推荐</a>|");
            if (ptype == 3)
                builder.Append("置顶|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("thread.aspx?uid=" + uid + "&amp;ptype=3") + "\">置顶</a>|");
            if (ptype == 4)
                builder.Append("锁定|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("thread.aspx?uid=" + uid + "&amp;ptype=4") + "\">锁定</a>|");
            if (ptype == 5)
                builder.Append("固底");
            else
                builder.Append("<a href=\"" + Utils.getUrl("thread.aspx?uid=" + uid + "&amp;ptype=5") + "\">固底</a>");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("thread.aspx?ptype=0") + "\">全部</a>|帖子回收");
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        string strOrder = string.Empty;
        string[] pageValUrl = { "uid", "ptype" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        if (uid > 0)
            strWhere += "UsID=" + uid + " and ";

        if (ptype == 0)
            strWhere += " IsDel=0 ";
        else if (ptype == 1)
            strWhere += " IsGood=1 and IsDel=0";
        else if (ptype == 2)
            strWhere += " IsRecom=1 and IsDel=0";
        else if (ptype == 3)
            strWhere += " IsTop>=1 and IsDel=0";
        else if (ptype == 4)
            strWhere += " IsLock=1 and IsDel=0";
        else if (ptype == 5)
            strWhere += " IsTop=-1 and IsDel=0";
        else if (ptype == 6)
            strWhere += "IsDel=1";

        strOrder = "ID Desc";

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
                builder.Append("<a href=\"" + Utils.getUrl("thread.aspx?act=edit&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[管理]&gt;</a>");
                builder.AppendFormat("<a href=\"" + Utils.getUrl("/bbs/topic.aspx?forumid=" + n.ForumId + "&amp;bid={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{1}</a>{2}", n.ID, n.Title, "(" + DT.FormatDate(n.AddTime, 2) + ")");
                builder.Append("<a href=\"" + Utils.getUrl("forumlog.aspx?act=bview&amp;bid=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">日志</a>.");
                builder.Append("<a href=\"" + Utils.getUrl("thread.aspx?act=del&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">删</a>");
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
        string strText = "输入用户ID:/,";
        string strName = "uid,ptype";
        string strType = "num,hidden";
        string strValu = "'" + ptype + "";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "搜帖子,thread.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        if (ptype == 6)
            builder.Append("<a href=\"" + Utils.getUrl("thread.aspx?act=clearrecycl") + "\">清空回收站</a><br />");
        else
            builder.Append("<a href=\"" + Utils.getUrl("thread.aspx?ptype=6") + "\">帖子回收站</a><br />");

        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void EditPage()
    {
        Master.Title = "编辑帖子";
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "帖子ID错误"));
        BCW.Model.Text model = new BCW.BLL.Text().GetText(id);
        if (model == null)
        {
            Utils.Error("不存在的帖子记录", "");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("编辑帖子");
        builder.Append(Out.Tab("</div>", ""));
        //得到论坛列表
        string strForum = string.Empty;
        DataSet ds = new BCW.BLL.Forum().GetList("ID,Title", "");
        if (ds != null && ds.Tables[0].Rows.Count != 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                strForum += "|" + ds.Tables[0].Rows[i]["ID"] + "|" + ds.Tables[0].Rows[i]["Title"] + "";
            }
        }
        strForum = Utils.Mid(strForum, 1, strForum.Length);
        //得到帖子类型
        string Label = new BCW.BLL.Forum().GetLabel(model.ForumId);
        if (Label != "")
            Label = "0|综合|" + Label + "";
        else
            Label = "0|综合";

        string strText = "主题:/,内容:/,类型:,用户ID:/,用户昵称:/,阅读数:/,精华:,推荐:,置顶:,锁定:,结束:,删除:,回帖时间:/,所在版块:/,,";
        string strName = "Title,Content,LabelId,UsID,UsName,ReadNum,IsGood,IsRecom,IsTop,IsLock,IsOver,IsDel,ReTime,ForumID,id,act";
        string strType = "text,textarea,select,num,text,num,select,select,select,select,select,select,date,select,hidden,hidden";
        string strValu = "" + model.Title + "'" + model.Content + "'" + model.LabelId + "'" + model.UsID + "'" + model.UsName + "'" + model.ReadNum + "'" + model.IsGood + "'" + model.IsRecom + "'" + model.IsTop + "'" + model.IsLock + "'" + model.IsOver + "'" + model.IsDel + "'" + DT.FormatDate(model.ReTime, 0) + "'" + model.ForumId + "'" + id + "'save";
        string strEmpt = "false,false," + Label + ",false,false,false,0|普通|1|精华,0|普通|1|推荐,-1|固底|0|普通|1|普通置顶|2|全区置顶,0|普通|1|锁定,0|普通|1|结束,0|正常|1|已删,false," + strForum + ",false,false";
        string strIdea = "/";
        string strOthe = "确定编辑,thread.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(" <a href=\"" + Utils.getPage("thread.aspx") + "\">取消</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("thread.aspx") + "\">帖子管理</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    /// <summary>
    /// 陈志基 20160816
    /// 修改保存属性改变触发事件
    /// </summary>
    private void EditSavePage()
    {
        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[1-9]\d*$", "帖子ID错误"));
        string Title = Utils.GetRequest("Title", "post", 2, @"^[^\^]{1,50}$", "标题限1-50字");
        string Content = Utils.GetRequest("Content", "post", 2, @"^[^\^]{1,50000}$", "请输入不超50000的内容");
        int LabelId = int.Parse(Utils.GetRequest("LabelId", "post", 2, @"^[0-9]\d*$", "类型选择错误"));
        int UsID = int.Parse(Utils.GetRequest("UsID", "post", 2, @"^[1-9]\d*$", "用户ID错误"));
        string UsName = Utils.GetRequest("UsName", "post", 2, @"^[^\^]{1,50}$", "昵称不超50字");
        int ReadNum = int.Parse(Utils.GetRequest("ReadNum", "post", 2, @"^[0-9]\d*$", "阅读数填写错误"));
        int IsGood = int.Parse(Utils.GetRequest("IsGood", "post", 2, @"^[0-1]$", "精华选择错误"));
        int IsRecom = int.Parse(Utils.GetRequest("IsRecom", "post", 2, @"^[0-1]$", "推荐选择错误"));
        int IsTop = int.Parse(Utils.GetRequest("IsTop", "post", 2, @"^-1|0|1|2$", "置顶选择错误"));
        int IsLock = int.Parse(Utils.GetRequest("IsLock", "post", 2, @"^[0-1]$", "锁定选择错误"));
        int IsOver = int.Parse(Utils.GetRequest("IsOver", "post", 2, @"^[0-1]$", "结束选择错误"));
        int IsDel = int.Parse(Utils.GetRequest("IsDel", "post", 2, @"^[0-1]$", "删除选择错误"));
        DateTime ReTime = Utils.ParseTime(Utils.GetRequest("ReTime", "post", 2, DT.RegexTime, "回帖时间填写出错"));
        int ForumID = int.Parse(Utils.GetRequest("ForumID", "post", 2, @"^[1-9]\d*$", "推荐选择错误"));
        if (!new BCW.BLL.Text().Exists(id))
        {
            Utils.Error("不存在的帖子记录", "");
        }
        if (!new BCW.BLL.User().Exists(UsID))
        {
            Utils.Error("不存在的用户ID", "");
        }
     

        BCW.Model.Text model2 = new BCW.BLL.Text().GetText(id);

        if (model2.IsDel != IsDel)//触发事件
        {
            if (IsDel == 1)
            {
                new BCW.BLL.Forumstat().Update2(1, UsID, model2.ForumId, model2.AddTime);//更新统计表发帖
            }
            else
            {
                new BCW.BLL.Forumstat().Update3(1, UsID, model2.ForumId, model2.AddTime);//更新统计表发帖
            }
        }
       
        BCW.Model.Text model = new BCW.Model.Text();
        model.ID = id;
        model.Title = Title;
        model.Content = Content;
        model.LabelId = LabelId;
        model.UsID = UsID;
        model.UsName = UsName;
        model.ReadNum = ReadNum;
        model.IsGood = IsGood;
        model.IsRecom = IsRecom;
        model.IsTop = IsTop;
        model.IsLock = IsLock;
        model.IsOver = IsOver;
        model.IsDel = IsDel;
        model.ReTime = ReTime;
        model.ForumId = ForumID;
        new BCW.BLL.Text().Update2(model);
        if (!ForumID.Equals(model2.ForumId))//触发事件
        {
            //去掉精华和推荐再转移
            if (model2.IsGood == 1)
            {
                new BCW.BLL.Text().UpdateIsGood(id, 0);
            }
            if (model2.IsRecom == 1)
            {
                new BCW.BLL.Text().UpdateIsRecom(id, 0);
            }
            //重新进行原论坛回复统计
            DataSet ds = new BCW.BLL.Reply().GetList("ID,AddTime,UsID,IsDel,UsName", "forumid=" + model2.ForumId + " and bid=" + id + "");
            {
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (int.Parse(ds.Tables[0].Rows[i]["IsDel"].ToString()) == 0)//如果回帖没有删除
                        {
                            //减少对旧论坛回帖用户的回帖统计
                            new BCW.BLL.Forumstat().Update2(2, int.Parse(ds.Tables[0].Rows[i]["UsID"].ToString()), model2.ForumId, DateTime.Parse(ds.Tables[0].Rows[i]["AddTime"].ToString()));
                            //增加对新论坛回帖用户的回帖统计
                            new BCW.BLL.Forumstat().Update3(2, int.Parse(ds.Tables[0].Rows[i]["UsID"].ToString()), ForumID, DateTime.Parse(ds.Tables[0].Rows[i]["AddTime"].ToString()));
                            // BCW.User.Users.UpdateForumStat(2, int.Parse(ds.Tables[0].Rows[i]["UsID"].ToString()), ds.Tables[0].Rows[i]["UsName"].ToString(), ForumID);
                        }
                        //更新回帖中的论坛ID
                        new BCW.BLL.Reply().UpdateForumID(int.Parse(ds.Tables[0].Rows[i]["ID"].ToString()), ForumID);
                        // new BCW.BLL.Forumstat().Update2(2, model.UsID, forumid, DateTime.Parse(ds.Tables[0].Rows[i]["AddTime"].ToString()));
                        // BCW.User.Users.UpdateForumStat(2, model.UsID, model.UsName, newid);
                        // new BCW.BLL.Reply().UpdateForumID(int.Parse(ds.Tables[0].Rows[i]["ID"].ToString()), newid);
                    }
                }
            }
            //转移的原论坛帖子统计
            new BCW.BLL.Forumstat().Update2(1, model2.UsID, model2.ForumId, model2.AddTime);
            //转移的新论坛帖子统计
           // BCW.User.Users.UpdateForumStat(1, model2.UsID, model2.UsName, ForumID);
            new BCW.BLL.Forumstat().Update3(1, model.UsID, ForumID, model.AddTime);
        }
        Utils.Success("编辑帖子", "编辑帖子成功..", Utils.getUrl("thread.aspx?act=edit&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
    }
   /// <summary>
   /// 陈志基 20160816
   /// 修改删除情况
   /// </summary>
    private void DelPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        if (info != "ok")
        {
            Master.Title = "删除帖子";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("不能恢复！确定删除此帖子吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("thread.aspx?info=ok&amp;act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("thread.aspx") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.BLL.Text().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            #region 修改
            //BCW.Model.Text text = new BCW.BLL.Text().GetText(id);
            //if (text.IsDel == 0)//如果帖子没有删除，后台强行删除（不能恢复）
            //{
            //    new BCW.BLL.Text().UpdateIsDel(id, 1);
            //    new BCW.BLL.Forumstat().Update2(1, text.UsID, text.ForumId, text.AddTime);//更新统计表发帖
            //    DataSet ds = new BCW.BLL.Reply().GetList("ID,AddTime,UsID,IsDel", "forumid=" + text.ForumId + " and bid=" + id + "");  //更新统计表回帖
            //    {
            //        if (ds != null && ds.Tables[0].Rows.Count > 0)
            //        {
            //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //            {
            //                if (int.Parse(ds.Tables[0].Rows[i]["IsDel"].ToString()) == 0)//如果回帖没有删除
            //                {
            //                    new BCW.BLL.Forumstat().Update2(2, int.Parse(ds.Tables[0].Rows[i]["UsID"].ToString()), text.ForumId, DateTime.Parse(ds.Tables[0].Rows[i]["AddTime"].ToString()));
            //                }

            //            }
            //        }
            //    }
            //    new BCW.BLL.Reply().UpdateIsDel(id, 1);
            //}
            //else
            //{

            //}
            #endregion
            //删除
            new BCW.BLL.Text().Delete(id);
            Utils.Success("删除帖子", "删除帖子成功..", Utils.getPage("thread.aspx"), "1");
        }
    }

    private void ClearRecyclPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info != "ok")
        {
            Master.Title = "清空回收站";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("不能恢复！确定删除回收站的所有帖子吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("thread.aspx?info=ok&amp;act=clearrecycl") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("thread.aspx?ptype=6") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            //删除
            new BCW.BLL.Text().Delete();
            //删除排行残余数据（数据为0的记录）
            new BCW.BLL.Forumstat().Delete();
            Utils.Success("清空回收站", "清空回收站成功..", Utils.getUrl("thread.aspx?ptype=6"), "1");
        }
    }
}