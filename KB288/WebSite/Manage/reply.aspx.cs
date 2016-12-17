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
/// 陈志基 2016/08/11 添加恢复回帖判断
/// </summary>
public partial class Manage_reply : System.Web.UI.Page
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
            case "nodel":
                NoDelPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        Master.Title = "回帖管理";
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[0-9]\d*$", "0"));
        int bid = int.Parse(Utils.GetRequest("bid", "all", 1, @"^[0-9]\d*$", "0"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-3]$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("回帖管理");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 0)
            builder.Append("全部回帖|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?ptype=0&amp;bid=" + bid + "") + "\">全部</a>|");
        if (ptype == 1)
            builder.Append("精华回帖|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?uid=" + uid + "&amp;bid=" + bid + "&amp;ptype=1") + "\">精华</a>|");
        if (ptype == 2)
            builder.Append("置顶回帖|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?uid=" + uid + "&amp;bid=" + bid + "&amp;ptype=2") + "\">置顶</a>|");

        if (ptype == 3)
            builder.Append("已删回帖");
        else
            builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?uid=" + uid + "&amp;bid=" + bid + "&amp;ptype=3") + "\">已删</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string strOrder = "";
        string[] pageValUrl = { "uid", "bid", "ptype" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        if (uid > 0)
            strWhere += "UsID=" + uid + "";

        if (bid > 0)
        {
            if (strWhere != "")
                strWhere += " and ";

            strWhere += "bid=" + bid + "";
        }

        if (ptype == 1)
        {
            if (strWhere != "")
                strWhere += " and ";

            strWhere += " IsGood=1";
        }
        else if (ptype == 2)
        {
            if (strWhere != "")
                strWhere += " and ";

            strWhere += " IsTop=1";
        }
        else if (ptype == 3)
        {
            if (strWhere != "")
                strWhere += " and ";

            strWhere += " IsDel=1";
           
        }
        strOrder = "ID Desc";

        // 开始读取列表
        IList<BCW.Model.Reply> listReply = new BCW.BLL.Reply().GetReplys(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listReply.Count > 0)
        {

            int k = 1;
            foreach (BCW.Model.Reply n in listReply)
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
                string sText = n.Content;
                if (sText.Length > 30)
                {
                    sText = Utils.Left(sText, 30) + "..";
                }
                builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?act=edit&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[管理]&gt;</a>");
                builder.AppendFormat("<a href=\"" + Utils.getUrl("/bbs/reply.aspx?act=view&amp;forumid=" + n.ForumId + "&amp;bid={0}&amp;reid=" + n.Floor + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">{1}</a>{2}", n.Bid, sText, "(" + DT.FormatDate(n.AddTime, 2) + ")");
                builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?act=del&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">删</a>|");
                builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?act=nodel&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">恢复</a>");
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
        string strText = "用户ID:,帖子ID:,";
        string strName = "uid,bid,ptype";
        string strType = "snum,snum,hidden";
        string strValu = "''" + ptype + "";
        string strEmpt = "true,true,false";
        string strIdea = "/";
        string strOthe = "搜回帖,reply.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));

    }

    private void EditPage()
    {
        Master.Title = "编辑回帖";
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "回帖ID错误"));
        BCW.Model.Reply model = new BCW.BLL.Reply().GetReply(id, 0);
        if (model == null)
        {
            Utils.Error("不存在的回帖记录", "");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("编辑回帖");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "内容:/,用户ID:/,用户昵称:/,精华:,置顶:,回帖时间:/,,";
        string strName = "Content,UsID,UsName,IsGood,IsTop,AddTime,id,act";
        string strType = "textarea,num,text,select,select,date,hidden,hidden";
        string strValu = "" + model.Content + "'" + model.UsID + "'" + model.UsName + "'" + model.IsGood + "'" + model.IsTop + "'" + DT.FormatDate(model.AddTime, 0) + "'" + id + "'save";
        string strEmpt = "false,false,false,0|普通|1|精华,0|普通|1|置顶,false,false,false";
        string strIdea = "/";
        string strOthe = "确定编辑,reply.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(" <a href=\"" + Utils.getPage("reply.aspx") + "\">取消</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("reply.aspx") + "\">回帖管理</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void EditSavePage()
    {
        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[1-9]\d*$", "回帖ID错误"));
        string Content = Utils.GetRequest("Content", "post", 2, @"^[^\^]{1,50000}$", "请输入不超300的内容");
        int UsID = int.Parse(Utils.GetRequest("UsID", "post", 2, @"^[1-9]\d*$", "用户ID错误"));
        string UsName = Utils.GetRequest("UsName", "post", 2, @"^[^\^]{1,50}$", "昵称不超50字");
        int IsGood = int.Parse(Utils.GetRequest("IsGood", "post", 2, @"^[0-1]$", "精华选择错误"));
        int IsTop = int.Parse(Utils.GetRequest("IsTop", "post", 2, @"^[0-1]$", "置顶选择错误"));
        DateTime AddTime = Utils.ParseTime(Utils.GetRequest("AddTime", "post", 2, DT.RegexTime, "回帖时间填写出错"));
        if (!new BCW.BLL.Reply().Exists(id))
        {
            Utils.Error("不存在的回帖记录", "");
        }
        if (!new BCW.BLL.User().Exists(UsID))
        {
            Utils.Error("不存在的用户ID", "");
        }
        BCW.Model.Reply modelre = new BCW.BLL.Reply().GetReply(id, 0);

        BCW.Model.Reply model = new BCW.Model.Reply();
        model.ID = id;
        model.Content = Content;
        model.UsID = UsID;
        model.UsName = UsName;
        model.IsGood = IsGood;
        model.IsTop = IsTop;
        model.AddTime = AddTime;

        new BCW.BLL.Reply().Update(model);

        ///-------------------------------------
        if (modelre.Bid == 16729)
        {
            int ManageId = new BCW.User.Manage().IsManageLogin();
            String sLogFilePath = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "log.txt";
            LogHelper.Write(sLogFilePath, "" + ManageId + "编辑回帖" + id + "|旧内容:" + modelre.Content + "|新内容:" + Content + "");
        }
        //---------------------------------------


        Utils.Success("编辑回帖", "编辑回帖成功..", Utils.getUrl("reply.aspx?act=edit&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
    }

    private void DelPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        if (info != "ok")
        {
            Master.Title = "删除回帖";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("不能恢复！确定删除此回帖吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?info=ok&amp;act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("reply.aspx") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.BLL.Reply().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            //删除
            new BCW.BLL.Reply().Delete(id);
            Utils.Success("删除回帖", "删除回帖成功..", Utils.getPage("reply.aspx"), "1");
        }
    }
    /// <summary>
    /// 陈志基 2016/08/11 添加恢复回帖判断
    /// 陈志基         16 恢复帖子影响统计数
    /// </summary>
    private void NoDelPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.Reply reply = new BCW.BLL.Reply().GetByID(id);   
        BCW.Model.Text text = new BCW.BLL.Text().GetText(reply.Bid);
        if (text.IsDel == 1)
        {
               Utils.Error("请先恢复帖子再恢复回帖", "");
        }
        if (info != "ok")
        {
            Master.Title = "恢复回帖";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定恢复此回帖吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?info=ok&amp;act=nodel&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定恢复</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("reply.aspx") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.BLL.Reply().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            new BCW.BLL.Forumstat().Update3(2, reply.UsID, reply.ForumId, reply.AddTime);//更新统计表发帖
             //  恢复
            new BCW.BLL.Reply().UpdateIsDel1(id, 0);//
            Utils.Success("恢复回帖", "恢复回帖成功..", Utils.getPage("reply.aspx"), "1");
        }
    }
}
