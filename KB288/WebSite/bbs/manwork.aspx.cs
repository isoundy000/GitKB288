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
using System.Text.RegularExpressions;
using BCW.Common;
/// <summary>
/// 陈志基 16/5/18
/// 添加发帖提醒ID功能RemindID（）
/// 包括管理员，总坛主
/// </summary>

public partial class bbs_manwork : System.Web.UI.Page
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
        Master.Title = "论坛版务";

        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "blackadd":
                BlackAddPage();
                break;
            case "blacksave":
                BlackSavePage();
                break;
            case "blackdel":
                BlackDelPage();
                break;
            case "history":
                HistoryPage();
                break;
            case "black":
                BlackPage();
                break;
            case "recycl":
                RecyclPage();
                break;
            case "log":
                LogPage();
                break;
            case "logview":
                LogViewPage();
                break;
            case "vice":
                VicePage();
                break;
            case "editvice":
                EditVicePage();
                break;
            case "editfootubb":
                EditFootUbbPage();
                break;
            case "fund":
                FundPage();
                break;
            case "fundpay":
                FundPayPage();
                break;
            case "fundlist":
                FundListPage();
                break;
            case "fundget":
                FundGetPage();
                break;
            case "fundgetre":
                FundGetRePage();
                break;
            case "fans":
                FansPage();
                break;
            case "fansadd":
                FansAddPage();
                break;
            case "fansinfo":
                FansInfoPage();
                break;
            case "fansdel":
                FansDelPage();
                break;
            case "fansok":
                FansOkPage();
                break;
            case "remind":
                RemindID();
                break;
            case "fansapply":
                FansApplyPage();
                break;
            case "flow":
                FlowPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        int forumid = int.Parse(Utils.GetRequest("forumid", "get", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.Forum model = new BCW.BLL.Forum().GetForumBasic(forumid);
        if (model == null)
            Utils.Error("不存在的论坛", "");

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "") + "\">返回论坛</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("版主列表");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = 20;
        string strWhere = "";
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "ForumID=" + forumid + " and (OverTime>='" + DateTime.Now + "' OR OverTime='1990-1-1 00:00:00') and Status=0";

        // 开始读取列表
        IList<BCW.Model.Role> listRole = new BCW.BLL.Role().GetRoles(pageIndex, pageSize, strWhere, out recordCount);
        if (listRole.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Role n in listRole)
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

                builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(n.UsID) + "(" + n.UsID + ")(" + BCW.User.Users.UserOnline(n.UsID) + ")</a>");
                builder.Append("<br />上任:" + DT.FormatDate(n.StartTime, 3) + "");
                builder.Append("<br />宣言:" + n.AddName + "");
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("注：排名不分先后");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            builder.Append(Out.Div("div", "还没有版主，欢迎申请.."));
        }
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("论坛ID:" + forumid + "<br />短地址:<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "") + "\">http://" + Utils.GetDomain() + "/forum-" + forumid + ".aspx</a>");
        builder.Append("<br />口号:" + ((model.Notes == "") ? "未设置口号" : model.Notes) + "");
        if (model.GroupId == 0)
            builder.Append("<br />基金:<a href=\"" + Utils.getUrl("manwork.aspx?act=fund&amp;forumid=" + forumid + "") + "\">" + model.iCent + "" + ub.Get("SiteBz") + "</a>");

        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));



        //计算粉丝人数
        string PayID = GetForumFans(forumid);
        int PayIDNum = 0;
        if (!string.IsNullOrEmpty(PayID))
        {
            PayIDNum = Utils.GetStringNum(PayID, "#");
            if (PayIDNum > 0)
                PayIDNum++;
            else
                PayIDNum = 1;
        }
        builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?act=fans&amp;forumid=" + forumid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">本版粉丝(" + PayIDNum + "人)</a>|<a href=\"" + Utils.getUrl("manwork.aspx?act=fansapply&amp;forumid=" + forumid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">加入</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?act=flow&amp;forumid=" + forumid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">本版滚动</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("sktype.aspx?forumid=" + forumid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">浏览模式</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?act=log&amp;forumid=" + forumid + "") + "\">版务日志</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?act=vice&amp;forumid=" + forumid + "") + "\">版规公告</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?act=black&amp;forumid=" + forumid + "") + "\">黑名单房</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?act=recycl&amp;forumid=" + forumid + "") + "\">帖子回收</a><br />");
        if (model.GroupId == 0)
            builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?act=fund&amp;forumid=" + forumid + "&amp;ptype=1") + "\">论坛基金</a>|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=fund&amp;id=" + model.GroupId + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">圈子基金</a>|");

        builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?act=logview&amp;ptype=14&amp;forumid=" + forumid + "") + "\">任职记录</a>");
        builder.Append("<br/><a href=\"" + Utils.getUrl("manwork.aspx?act=remind&amp;forumid=" + forumid + "") + "\">发帖提醒ID</a>");

        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("forumstat.aspx?act=top&amp;forumid=" + forumid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">本版排行</a>/");
        builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?act=forum&amp;backurl=" + Utils.PostPage(1) + "") + "\">个性</a>/");
        builder.Append("<a href=\"" + Utils.getUrl("forumstat.aspx?forumid=" + forumid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">统计</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("manager.aspx?backurl=" + Utils.PostPage(1) + "") + "\">呼叫社区管理员</a>");
        builder.Append(Out.Tab("</div>", Out.Hr()));
        if (new BCW.User.ForumInc().IsForumGSIDS(forumid) == true)
        {
            string GsString = "";
            string GsAdminID = ub.GetSub("BbsGsAdminID", xmlPath);
            string GsAdminID2 = ub.GetSub("BbsGsAdminID2", xmlPath);
            if (("#" + GsAdminID + "#").Contains("#" + meid + "#"))
            {
                GsString += "<a href=\"" + Utils.getUrl("Gsopen.aspx?forumid=" + forumid + "") + "\">【高手开奖管理】</a>";
            }
            if (("#" + GsAdminID2 + "#").Contains("#" + meid + "#"))
            {
                if (GsString != "")
                    GsString += "<br />";

                GsString += "<a href=\"" + Utils.getUrl("Gsqiset.aspx?backurl=" + Utils.PostPage(1) + "") + "\">【设置期数/时间】</a>";
            }
            if (GsString != "")
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append(GsString);
                builder.Append(Out.Tab("</div>", Out.Hr()));
            }
        }

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "") + "\">" + model.Title + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void FansApplyPage()
    {
        Master.Title = "申请加入本版粉丝团";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int forumid = int.Parse(Utils.GetRequest("forumid", "all", 2, @"^[1-9]\d*$", "论坛ID错误"));

        string ForumName = new BCW.BLL.Forum().GetTitle(forumid);
        if (ForumName == "")
            Utils.Error("不存在的论坛", "");

        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {
            DataSet ds = new BCW.BLL.Role().GetList("UsID,UsName,ForumName", "ForumID=" + forumid + " and (OverTime>='" + DateTime.Now + "' OR OverTime='1990-1-1 00:00:00') and Status=0");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {

                if (new BCW.BLL.Medal().ExistsTypes(2, meid))
                {
                    Utils.Error("你已经是某个论坛的粉丝了", "");
                }

                for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                {
                    int UsID = int.Parse(ds.Tables[0].Rows[k]["UsID"].ToString());
                    string UsName = ds.Tables[0].Rows[k]["UsName"].ToString();

                    new BCW.BLL.Guest().Add(UsID, UsName, "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + new BCW.BLL.User().GetUsName(meid) + "[/url]已加入“" + ForumName + "”论坛粉丝团，[url=/bbs/manwork.aspx?act=fansadd&amp;forumid=" + forumid + "]马上添加&gt;&gt;[/url]");

                }
            }
            else
            {
                Utils.Error("本版暂无版主管理...", "");
            }

            Utils.Success("申请加入粉丝团", "您的请求已通知该版版主，请等待版主审核并给您挑选论坛个性徽章...", Utils.getUrl("manwork.aspx?forumid=" + forumid + ""), "1");
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定要申请加入这个粉丝团吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?act=fansapply&amp;info=ok&amp;forumid=" + forumid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定申请</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?forumid=" + forumid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }

    }

    private void VicePage()
    {
        int forumid = int.Parse(Utils.GetRequest("forumid", "get", 2, @"^[1-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Forum().Exists2(forumid))
            Utils.Error("不存在的论坛", "");
        int meid = new BCW.User.Users().GetUsId();
        string Content = new BCW.BLL.Forum().GetContent(forumid);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("版规公告");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("内容:");

        int pageIndex;
        int recordCount;
        int pageSize = 500;
        string[] pageValUrl = { "act", "forumid" };
        pageIndex = Utils.ParseInt(Request.QueryString["vp"]);
        if (pageIndex == 0)
            pageIndex = 1;

        int pover = int.Parse(Utils.GetRequest("pover", "get", 1, @"^[0-9]\d*$", "0"));
        string content = BasePage.MultiContent(Content, pageIndex, pageSize, pover, out recordCount);
        builder.Append(Out.SysUBB(content));

        builder.Append(BasePage.MultiContentPage(Content, pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "vp", pover));
        builder.Append(Out.Tab("</div>", ""));

        if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_ForumPara, meid, forumid))
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?act=editvice&amp;forumid=" + forumid + "") + "\">修改版规公告</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?act=editfootubb&amp;forumid=" + forumid + "") + "\">修改本版底链</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?forumid=" + forumid + "") + "\">版务中心</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    private void EditVicePage()
    {
        int forumid = int.Parse(Utils.GetRequest("forumid", "all", 2, @"^[1-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Forum().Exists2(forumid))
            Utils.Error("不存在的论坛", "");

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        new BCW.User.Role().CheckUserRole(BCW.User.Role.enumRole.Role_ForumPara, meid, forumid);
        string info = Utils.GetRequest("info", "post", 1, "", "");
        if (info == "ok")
        {
            //管理安全提示
            string[] p_pageArr = { "act", "forumid", "Content", "info" };
            BCW.User.Role.AdminSafePage(meid, Utils.getPageUrl(), p_pageArr, "post");

            string Content = Utils.GetRequest("Content", "post", 2, @"^[^\^]{1,3000}$", "版规内容限1-3000字");
            new BCW.BLL.Forum().UpdateContent(forumid, Content);
            Utils.Success("修改版规", "修改版规成功，正在返回..", Utils.getUrl("manwork.aspx?act=editvice&amp;forumid=" + forumid + ""), "1");
        }
        else
        {
            string Content = new BCW.BLL.Forum().GetContent(forumid);
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("修改版规公告");
            builder.Append(Out.Tab("</div>", ""));
            strText = "内容:/,,,";
            strName = "Content,forumid,act,info";
            strType = "textarea,hidden,hidden,hidden";
            strValu = "" + Content + "'" + forumid + "'editvice'ok";
            strEmpt = "false,false,false,false";
            strIdea = "/";
            strOthe = "确定修改,manwork.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(" <a href=\"" + Utils.getUrl("manwork.aspx?act=vice&amp;forumid=" + forumid + "") + "\">取消</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private void EditFootUbbPage()
    {
        int forumid = int.Parse(Utils.GetRequest("forumid", "all", 2, @"^[1-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Forum().Exists2(forumid))
            Utils.Error("不存在的论坛", "");

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        new BCW.User.Role().CheckUserRole(BCW.User.Role.enumRole.Role_ForumPara, meid, forumid);
        string info = Utils.GetRequest("info", "post", 1, "", "");
        if (info == "ok")
        {
            //管理安全提示
            string[] p_pageArr = { "act", "forumid", "FootUbb", "info" };
            BCW.User.Role.AdminSafePage(meid, Utils.getPageUrl(), p_pageArr, "post");

            string FootUbb = Utils.GetRequest("FootUbb", "post", 3, @"^[\s\S]{1,800}$", "底链限800字内");
            if (FootUbb.Contains("http://"))
            {
                Utils.Error("Ubb内的链接地址不用填写http://和本站域名", "");

            }
            new BCW.BLL.Forum().UpdateFootUbb(forumid, FootUbb);
            Utils.Success("修改底链", "修改本版底链成功，正在返回..", Utils.getUrl("manwork.aspx?act=editfootubb&amp;forumid=" + forumid + ""), "1");
        }
        else
        {
            string FootUbb = new BCW.BLL.Forum().GetFootUbb(forumid);
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("修改本版底链");
            builder.Append(Out.Tab("</div>", ""));
            strText = "内容(使用UBB):/,,,";
            strName = "FootUbb,forumid,act,info";
            strType = "textarea,hidden,hidden,hidden";
            strValu = "" + FootUbb + "'" + forumid + "'editfootubb'ok";
            strEmpt = "true,false,false,false";
            strIdea = "/";
            strOthe = "确定修改,manwork.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(" <a href=\"" + Utils.getUrl("manwork.aspx?act=vice&amp;forumid=" + forumid + "") + "\">取消</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private void HistoryPage()
    {
        int forumid = int.Parse(Utils.GetRequest("forumid", "get", 2, @"^[1-9]\d*$", "ID错误"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-2]\d*$", "1"));
        if (!new BCW.BLL.Forum().Exists2(forumid))
            Utils.Error("不存在的论坛", "");

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        if (ptype == 1)
            builder.Append("往届版主|<a href=\"" + Utils.getUrl("manwork.aspx?act=history&amp;forumid=" + forumid + "&amp;ptype=2") + "\">荣誉版主</a>");
        else
            builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?act=history&amp;forumid=" + forumid + "&amp;ptype=1") + "\">往届版主</a>|荣誉版主");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "ptype" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        if (ptype == 1)
            strWhere = "ForumID=" + forumid + " and (OverTime<'" + DateTime.Now + "' and OverTime<>'1990-1-1 00:00:00') and Status<>1";
        else
            strWhere = "ForumID=" + forumid + " and Status=2";

        // 开始读取列表
        IList<BCW.Model.Role> listRole = new BCW.BLL.Role().GetRoles(pageIndex, pageSize, strWhere, out recordCount);
        if (listRole.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Role n in listRole)
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

                builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + ((pageIndex - 1) * pageSize + k) + "." + BCW.User.Users.SetUser(n.UsID) + "(" + n.UsID + ")</a>");
                builder.Append("<br />上任:" + n.StartTime + "");
                builder.Append("<br />离任:" + n.OverTime + "");
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
        builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?forumid=" + forumid + "") + "\">版务中心</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void BlackPage()
    {
        Master.Title = "黑名单";
        int meid = new BCW.User.Users().GetUsId();

        int forumid = int.Parse(Utils.GetRequest("forumid", "all", 1, @"^[1-9]\d*$", "0"));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        if (forumid > 0)
        {
            if (!new BCW.BLL.Forum().Exists2(forumid))
                Utils.Error("不存在的论坛", "");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        if (forumid > 0 && uid == 0)
            builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "") + "\">" + new BCW.BLL.Forum().GetTitle(forumid) + "</a>&gt;黑名单");
        else
            builder.Append("黑名单列表");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        if (uid != 0)
            strWhere += "UsID=" + uid + " and ";
        if (forumid != 0)
            strWhere += "ForumID=" + forumid + " and ";

        strWhere += "ExitTime>='" + DateTime.Now + "'";

        // 开始读取列表
        IList<BCW.Model.Blacklist> listBlacklist = new BCW.BLL.Blacklist().GetBlacklists(pageIndex, pageSize, strWhere, out recordCount);
        if (listBlacklist.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Blacklist n in listBlacklist)
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
                string sInclude = string.Empty;
                if (n.Include == 1)
                    sInclude = "(含下级版块)";

                string sRole = string.Empty;
                if (n.ForumID == 0)
                    sRole = BCW.User.Limits.OutLimitString(n.BlackRole);
                else
                    sRole = BCW.User.FLimits.OutFLimitString(n.BlackRole);

                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")</a>");
                builder.Append("" + sRole + ":" + n.BlackDay + "天/理由:" + n.BlackWhy + "[" + DT.FormatDate(n.AddTime, 5) + "]");
                builder.Append("<br />范围:<a href=\"" + Utils.getUrl("/bbs/forum.aspx?forumid=" + n.ForumID + "") + "\">" + n.ForumName + "" + sInclude + "</a>");
                builder.Append("操作ID:<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.AdminUsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.AdminUsID + "</a>");
                if (n.ForumID > 0)
                {
                    if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_DelForumBlack, meid, forumid))
                    {
                        builder.Append(".<a href=\"" + Utils.getUrl("manwork.aspx?act=blackdel&amp;uid=" + n.UsID + "&amp;forumid=" + n.ForumID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">(解黑)</a>");
                    }
                }
                else
                {
                    if (new BCW.User.Role().IsUserRole(meid))
                    {
                        builder.Append(".<a href=\"" + Utils.getUrl("usermanage.aspx?hid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">(管理)</a>");
                    }
                }
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

        strText = "会员ID:,,";
        strName = "uid,forumid,act";
        strType = "snum,hidden,hidden";
        strValu = "'" + forumid + "'black";
        strEmpt = "false,false,false";
        strIdea = "";
        strOthe = "搜黑名,manwork.aspx,post,3,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_AddForumBlack, meid, forumid))
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?act=blackadd&amp;forumid=" + forumid + "") + "\">加黑名单</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?forumid=" + forumid + "") + "\">版务中心</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void BlackAddPage()
    {
        Master.Title = "加黑名单";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int uid = int.Parse(Utils.GetRequest("uid", "get", 1, @"^[1-9]\d*$", "0"));
        int forumid = int.Parse(Utils.GetRequest("forumid", "get", 2, @"^[1-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Forum().Exists2(forumid))
        {
            Utils.Error("不存在的论坛", "");
        }
        if (uid > 0)
        {
            if (!new BCW.BLL.User().Exists(uid))
            {
                Utils.Error("不存在的会员ID", "");
            }
        }
        new BCW.User.Role().CheckUserRole(BCW.User.Role.enumRole.Role_AddForumBlack, meid, forumid);

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("加入<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "") + "\">" + new BCW.BLL.Forum().GetTitle(forumid) + "</a>黑名单");
        builder.Append(Out.Tab("</div>", ""));

        if (uid > 0)
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("加黑会员<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + uid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + new BCW.BLL.User().GetUsName(uid) + "(" + uid + ")</a>");
            builder.Append(Out.Tab("</div>", ""));
            strText = ",禁止权限，可多选:/,加黑理由(50字内):/,期限:,,,";
            strName = "uid,Role,Why,rDay,forumid,act,backurl";
            strType = "hidden,multiple,textarea,select,hidden,hidden,hidden";
            strValu = "" + uid + "'''10'" + forumid + "'blacksave'" + Utils.getPage(0) + "";
            strEmpt = "false," + BCW.User.FLimits.GetLimitString() + ",false,1|1天|2|2天|3|3天|5|5天|10|10天|15|15天|30|30天,false,false,false";
        }
        else
        {
            strText = "会员ID:/,禁止权限，可多选:/,加黑理由(50字内):/,期限:,,";
            strName = "uid,Role,Why,rDay,forumid,act";
            strType = "num,multiple,textarea,select,hidden,hidden";
            strValu = "'''10'" + forumid + "'blacksave";
            strEmpt = "false," + BCW.User.FLimits.GetLimitString() + ",false,1|1天|2|2天|3|3天|5|5天|10|10天|15|15天|30|30天,false,false";
        }
        strIdea = "/";
        strOthe = "加黑|reset,manwork.aspx,post,1,red|blue";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?act=black&amp;forumid=" + forumid + "") + "\">黑名单</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void BlackSavePage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();


        //管理安全提示
        string[] p_pageArr = { "act", "forumid", "Role", "uid", "Why", "rDay", "backurl" };
        BCW.User.Role.AdminSafePage(meid, Utils.getPageUrl(), p_pageArr, "post");


        int forumid = int.Parse(Utils.GetRequest("forumid", "post", 2, @"^[1-9]\d*$", "ID错误"));
        int uid = int.Parse(Utils.GetRequest("uid", "post", 2, @"^[1-9]\d*$", "会员ID错误"));
        string Role = Utils.GetRequest("Role", "post", 2, @"^[\w((;|,)\w)?]+$", "选择权限错误");
        Role = Role.Replace(",", ";");
        string Why = Utils.GetRequest("Why", "post", 2, @"^[^\^]{1,50}$", "加黑理由限1-50字");
        int rDay = int.Parse(Utils.GetRequest("rDay", "post", 2, @"^1|2|3|5|10|15|30$", "期限选择错误"));

        string UsName = new BCW.BLL.User().GetUsName(uid);
        if (UsName == "")
            Utils.Error("不存在的会员ID", "");

        string ForumName = new BCW.BLL.Forum().GetTitle(forumid);
        if (ForumName == "")
            Utils.Error("不存在的论坛", "");

        new BCW.User.Role().CheckUserRole(BCW.User.Role.enumRole.Role_AddForumBlack, meid, forumid);

        if (new BCW.BLL.Blacklist().Exists(uid, forumid))
        {
            Utils.Error("此ID已在本版黑名单里，如要再次加黑，请先解除再进行加黑", "");
        }
        BCW.Model.Blacklist model = new BCW.Model.Blacklist();
        model.UsID = uid;
        model.UsName = UsName;
        model.ForumID = forumid;
        model.ForumName = ForumName;
        model.BlackRole = Role;
        model.BlackWhy = Why;
        model.BlackDay = rDay;
        model.Include = 0;
        model.AdminUsID = meid;
        model.ExitTime = DateTime.Now.AddDays(rDay);
        model.AddTime = DateTime.Now;
        new BCW.BLL.Blacklist().Add(model);
        //记录日志
        string strLog = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + new BCW.BLL.User().GetUsName(meid) + "[/url]加黑[url=/bbs/uinfo.aspx?uid=" + uid + "]" + UsName + "[/url]" + rDay + "天," + BCW.User.FLimits.OutFLimitString(Role) + "";
        string strLog2 = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + new BCW.BLL.User().GetUsName(meid) + "[/url]加黑你" + rDay + "天," + BCW.User.FLimits.OutFLimitString(Role) + "";
        if (!string.IsNullOrEmpty(Why))
        {
            strLog += ",理由:" + Why + "";
            strLog2 += ",理由:" + Why + "";
        }
        new BCW.BLL.Forumlog().Add(10, forumid, strLog);
        new BCW.BLL.Guest().Add(0, uid, UsName, strLog2);
        Utils.Success("加黑名单", "加黑名单成功，正在返回..", Utils.getPage("manwork.aspx?act=black&amp;forumid=" + forumid + ""), "1");
    }

    private void BlackDelPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int forumid = int.Parse(Utils.GetRequest("forumid", "all", 2, @"^[1-9]\d*$", "ID错误"));

        int uid = int.Parse(Utils.GetRequest("uid", "all", 2, @"^[1-9]\d*$", "会员ID错误"));

        string UsName = new BCW.BLL.User().GetUsName(uid);
        if (UsName == "")
            Utils.Error("不存在的会员ID", "");

        string ForumName = new BCW.BLL.Forum().GetTitle(forumid);
        if (ForumName == "")
            Utils.Error("不存在的论坛", "");

        new BCW.User.Role().CheckUserRole(BCW.User.Role.enumRole.Role_DelForumBlack, meid, forumid);

        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info != "ok")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定将" + UsName + "(" + uid + ")解除吗");
            builder.Append(Out.Tab("</div>", ""));

            strText = ",理由:,,,";
            strName = "uid,Why,forumid,act,info";
            strType = "hidden,text,hidden,hidden,hidden";
            strValu = "" + uid + "''" + forumid + "'blackdel'ok";
            strEmpt = "false,true,false,false,false";
            strIdea = "/";
            strOthe = "确定解除,manwork.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append(" <a href=\"" + Utils.getPage("manwork.aspx?act=back&amp;forumid=" + forumid + "") + "\">取消</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            //管理安全提示
            string[] p_pageArr = { "act", "forumid", "uid", "Why", "info" };
            BCW.User.Role.AdminSafePage(meid, Utils.getPageUrl(), p_pageArr, "post");

            if (!new BCW.BLL.Blacklist().Exists(uid, forumid))
            {
                Utils.Error("不存在的黑名单记录", "");
            }
            new BCW.BLL.Blacklist().Delete(uid, forumid);
            //记录日志
            string Why = Utils.GetRequest("Why", "post", 3, @"^[^\^]{1,50}$", "理由限50字内，可留空");
            string strLog = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + new BCW.BLL.User().GetUsName(meid) + "[/url]解黑[url=/bbs/uinfo.aspx?uid=" + uid + "]" + UsName + "[/url]";
            string strLog2 = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + new BCW.BLL.User().GetUsName(meid) + "[/url]将你解黑";

            if (!string.IsNullOrEmpty(Why))
            {
                strLog += ",理由:" + Why + "";
                strLog2 += ",理由:" + Why + "";
            }

            new BCW.BLL.Forumlog().Add(10, forumid, strLog);
            new BCW.BLL.Guest().Add(0, uid, UsName, strLog2);
            Utils.Success("解除黑名单", "解除黑名单" + UsName + "(" + uid + ")成功，正在返回..", Utils.getUrl("manwork.aspx?act=black&amp;forumid=" + forumid + ""), "3");
        }
    }

    private void RecyclPage()
    {
        Master.Title = "帖子回收站";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int forumid = int.Parse(Utils.GetRequest("forumid", "all", 2, @"^[1-9]\d*$", "ID错误"));
        string ForumName = new BCW.BLL.Forum().GetTitle(forumid);
        if (ForumName == "")
            Utils.Error("不存在的论坛", "");
        bool flag = new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_BackText, meid, forumid);
        int bid = int.Parse(Utils.GetRequest("bid", "all", 1, @"^[1-9]\d*$", "0"));
        if (bid > 0)
        {
            if (flag == false)
            {
                Utils.Error("你的权限不足", "");
            }
            string info = Utils.GetRequest("info", "all", 1, "", "");
            if (info == "ok")
            {

                //管理安全提示
                string[] p_pageArr = { "act", "forumid", "bid", "Why", "info" };
                BCW.User.Role.AdminSafePage(meid, Utils.getPageUrl(), p_pageArr, "post");

                if (!new BCW.BLL.Text().Exists(bid, forumid))
                {
                    Utils.Error("不存在的回收记录", "");
                }
                //帖子
                new BCW.BLL.Text().UpdateIsDel(bid, 0);
                //回帖
                new BCW.BLL.Reply().UpdateIsDel(bid, 0);
                //记录日志
                BCW.Model.Text model = new BCW.BLL.Text().GetTextMe(bid);
                string Why = Utils.GetRequest("Why", "post", 3, @"^[^\^]{1,20}$", "理由限20字内，可留空");
                string strLog = "主题[url=/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "]《" + model.Title + "》[/url]被[url=/bbs/uinfo.aspx?uid=" + meid + "]" + new BCW.BLL.User().GetUsName(meid) + "[/url]恢复!";
                if (!string.IsNullOrEmpty(Why))
                    strLog += "理由:" + Why + "";
                new BCW.BLL.Forumlog().Add(6, forumid, "[url=/bbs/uinfo.aspx?uid=" + model.UsID + "]" + model.UsName + "[/url]的" + strLog);
                new BCW.BLL.Guest().Add(0, model.UsID, model.UsName, "您的" + strLog);
                Utils.Success("恢复帖子", "收复帖子(" + model.Title + ")成功，正在返回..", Utils.getUrl("manwork.aspx?act=recycl&amp;forumid=" + forumid + ""), "2");
            }
            else
            {
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("确定将此帖恢复吗");
                builder.Append(Out.Tab("</div>", ""));

                strText = ",理由:,,,";
                strName = "bid,Why,forumid,act,info";
                strType = "hidden,text,hidden,hidden,hidden";
                strValu = "" + bid + "''" + forumid + "'recycl'ok";
                strEmpt = "false,true,false,false,false";
                strIdea = "/";
                strOthe = "确定恢复,manwork.aspx,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

                builder.Append(Out.Tab("<div>", ""));
                builder.Append(" <a href=\"" + Utils.getPage("manwork.aspx?act=recycl&amp;forumid=" + forumid + "") + "\">取消</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "") + "\">" + ForumName + "</a>&gt;回收站");
            builder.Append(Out.Tab("</div>", "<br />"));
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string strWhere = string.Empty;
            string strOrder = string.Empty;
            string[] pageValUrl = { "act", "forumid", "tsid", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            //查询条件
            strWhere += "ForumId=" + forumid + " and IsDel=1";

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

                    builder.AppendFormat("{0}." + n.UsName + "(" + n.UsID + "){1}({2})", (pageIndex - 1) * pageSize + k, n.Title, n.ID);
                    if (flag)
                    {
                        builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?act=recycl&amp;forumid=" + forumid + "&amp;bid=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[恢复]</a>");
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
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示:系统将定期清空回收站..");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?forumid=" + forumid + "") + "\">版务中心</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void LogPage()
    {
        Master.Title = "版务日志";
        int forumid = int.Parse(Utils.GetRequest("forumid", "all", 2, @"^[1-9]\d*$", "ID错误"));
        string ForumName = new BCW.BLL.Forum().GetTitle(forumid);
        if (ForumName == "")
            Utils.Error("不存在的论坛", "");

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "") + "\">" + ForumName + "</a>&gt;版务日志");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?act=logview&amp;forumid=" + forumid + "") + "\">=全部日志=</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?act=logview&amp;ptype=1&amp;forumid=" + forumid + "") + "\">精华帖日志</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?act=logview&amp;ptype=2&amp;forumid=" + forumid + "") + "\">推荐帖日志</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?act=logview&amp;ptype=3&amp;forumid=" + forumid + "") + "\">置顶帖日志</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?act=logview&amp;ptype=4&amp;forumid=" + forumid + "") + "\">固底帖日志</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?act=logview&amp;ptype=5&amp;forumid=" + forumid + "") + "\">锁定帖日志</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?act=logview&amp;ptype=6&amp;forumid=" + forumid + "") + "\">删除帖日志</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?act=logview&amp;ptype=7&amp;forumid=" + forumid + "") + "\">编辑帖日志</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?act=logview&amp;ptype=8&amp;forumid=" + forumid + "") + "\">转移帖日志</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?act=logview&amp;ptype=9&amp;forumid=" + forumid + "") + "\">设专题日志</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?act=logview&amp;ptype=10&amp;forumid=" + forumid + "") + "\">黑名单日志</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?act=logview&amp;ptype=13&amp;forumid=" + forumid + "") + "\">设粉丝日志</a>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?forumid=" + forumid + "") + "\">版务中心</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    private void RemindID()
    {
        int forumid = int.Parse(Utils.GetRequest("forumid", "all", 2, @"^[1-9]\d*$", "I11D错误"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("设置发帖提醒ID");
        builder.Append(Out.Tab("</div>", "<br />"));
        int node = new BCW.BLL.Forum().GetNodeId(forumid);   //得到对应forumid 的上级总版主 ID
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string info = Utils.GetRequest("info", "all", 1, "", "");
        string remind = ub.GetSub("remindid" + forumid, xmlPath);//获取XML的值
        if (info == "")
        {


            int flag = 0;//判断标识

            int pageIndex;
            int recordCount;
            int pageSize = 20;
            string strWhere = "";
            string[] pageValUrl = { "act", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            //查询条件
            if (forumid > 0) //版主的
            {
                if (forumid == 28)
                {
                    //builder.Append("get in here<br/>");
                    int c = 12;
                    strWhere = "ForumID=" + c + " and (OverTime>='" + DateTime.Now + "' OR OverTime='1990-1-1 00:00:00') and Status=0";
                }
                else if (node != 0)  //如果有上级ID
                {
                    strWhere = "(ForumID=" + node + "or ForumID=" + forumid + " )and (OverTime>='" + DateTime.Now + "' OR OverTime='1990-1-1 00:00:00') and Status=0";
                    //builder.Append("上级ID ：" + node + "<br/>");
                }
                else
                {
                    //builder.Append("原本ID ：" + forumid + "<br/>");
                    strWhere = "ForumID=" + forumid + " and (OverTime>='" + DateTime.Now + "' OR OverTime='1990-1-1 00:00:00') and Status=0";
                }

                // 开始读取列表
                IList<BCW.Model.Role> listRole = new BCW.BLL.Role().GetRoles(pageIndex, pageSize, strWhere, out recordCount);//版主列表
                if (listRole.Count > 0)
                {
                    foreach (BCW.Model.Role n in listRole)
                    {
                        if (n.UsID == meid)//如果是版主
                        {
                            flag = 1;
                        }

                    }
                }
                //这里开始读取管理员(最大权限)
                int a = -1;
                int b = 0;
                strWhere = "(ForumID=" + a + " or ForumID=" + b + ")and (OverTime>='" + DateTime.Now + "' OR OverTime='1990-1-1 00:00:00') and Status=0";
                // 开始读取列表
                IList<BCW.Model.Role> listRole1 = new BCW.BLL.Role().GetRoles(pageIndex, pageSize, strWhere, out recordCount);//版主列表
                // builder.Append("listRole1个数 ：" + listRole1.Count + "<br/>");
                if (listRole1.Count > 0)
                {
                    foreach (BCW.Model.Role n in listRole1)
                    {
                        //builder.Append("listRole1管理员 ：" + n.UsName+"<br/>");
                        if (n.UsID == meid)//如果是管理员
                        {
                            flag = 1;
                        }

                    }
                }
            }

            if (flag == 1)
            {
                strText = "设置提醒ID(用#符号分割):/,,,";
                strName = "remindid,forumid,act,info";
                strType = "text,hidden,hidden,hidden";
                strValu = remind + "'" + forumid + "'remind'ok";
                strEmpt = "false,false,false,false";
                strIdea = "/";
                strOthe = "确定编辑,manwork.aspx,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
            else
            {
                Utils.Error("抱歉，你暂时不是版主还没有权限设置提醒ID！", "");
            }
        }
        else
        {
            string remindid = string.Empty;
            string remindid1 = Utils.GetRequest("remindid", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "多个ID请用#分隔，可以留空");
            if (!string.IsNullOrEmpty(remindid1))
            {
                //builder.Append("得到的forumid：" + forumid);
                //builder.Append("上级forumid：" + node);
                remindid = Utils.GetRequest("remindid", "post", 2, @"^([0-9]+[#]?){1,500}$", "设置发帖提醒ID填写错误，格式如1234#1111#2222,不能有空格");
                ub xml = new ub();
                Application.Remove(xmlPath);//清缓存
                xml.ReloadSub(xmlPath); //加载配置
                xml.dss["remindid" + forumid] = remindid;  //修改可下注金额
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                //if (node != 0)  //如果有上级ID
                //{
                //    Utils.Success("设置成功11", "11设置提醒ID成功！，正在返回..", Utils.getUrl("manwork.aspx?a&amp;forumid=" + node), "3");
                //}
                //else 
                {
                    Utils.Success("设置成功", "设置提醒ID成功！，正在返回..", Utils.getUrl("manwork.aspx?a&amp;forumid=" + forumid), "3");
                }
            }
            else
            {
                ub xml = new ub();
                Application.Remove(xmlPath);//清缓存
                xml.ReloadSub(xmlPath); //加载配置
                xml.dss["remindid" + forumid] = "";  //修改可下注金额
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                //if (node != 0)  //如果有上级ID
                //{
                //    Utils.Success("设置成功", "设置提醒ID成功！，正在返回..", Utils.getUrl("manwork.aspx?a&amp;forumid=" + node), "3");
                //}
                //else
                {
                    Utils.Success("设置成功", "设置提醒ID成功！，正在返回..", Utils.getUrl("manwork.aspx?a&amp;forumid=" + forumid), "3");
                }
            }

        }

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("forum.aspx?forumid=" + forumid + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?forumid=" + forumid + "") + "\">版务中心</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    private void LogViewPage()
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-9]\d*$", "0"));
        int forumid = int.Parse(Utils.GetRequest("forumid", "all", 2, @"^[1-9]\d*$", "ID错误"));
        int bid = int.Parse(Utils.GetRequest("bid", "all", 1, @"^[1-9]\d*$", "0"));
        string ForumName = new BCW.BLL.Forum().GetTitle(forumid);
        if (ForumName == "")
            Utils.Error("不存在的论坛", "");

        string sText = string.Empty;
        if (ptype == 1)
            sText = "精华帖日志";
        else if (ptype == 2)
            sText = "推荐帖日志";
        else if (ptype == 3)
            sText = "置顶帖日志";
        else if (ptype == 4)
            sText = "固底帖日志";
        else if (ptype == 5)
            sText = "锁定帖日志";
        else if (ptype == 6)
            sText = "删除帖日志";
        else if (ptype == 7)
            sText = "编辑帖日志";
        else if (ptype == 8)
            sText = "转移帖日志";
        else if (ptype == 9)
            sText = "设专题日志";
        else if (ptype == 10)
            sText = "黑名单日志";
        else if (ptype == 13)
            sText = "设粉丝日志";
        else if (ptype == 14)
            sText = "版主任职记录";
        else
            sText = "全部日志";

        if (bid > 0)
            sText = "帖子日志";

        Master.Title = sText;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        if (ptype == 14)
            builder.Append("" + sText + "");
        else
            builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?act=log&amp;forumid=" + forumid + "") + "\">日志</a>&gt;" + sText + "");

        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "ptype", "forumid", "bid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        if (ptype != 0)
            strWhere = "ForumID=" + forumid + " and Types=" + ptype + "";
        else
            strWhere = "ForumID=" + forumid + "";

        if (bid > 0)
            strWhere += " and BID=" + bid + " and ReID=0";

        // 开始读取列表
        IList<BCW.Model.Forumlog> listForumlog = new BCW.BLL.Forumlog().GetForumlogs(pageIndex, pageSize, strWhere, out recordCount);
        if (listForumlog.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Forumlog n in listForumlog)
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
                if (ptype == 14)
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + "." + Out.SysUBB(n.Content) + "");
                else
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + "." + Out.SysUBB(n.Content) + "(" + DT.FormatDate(n.AddTime, 6) + ")");
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
        builder.Append("<a href=\"" + Utils.getPage("forum.aspx?forumid=" + forumid + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?forumid=" + forumid + "") + "\">版务中心</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void FundPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int forumid = int.Parse(Utils.GetRequest("forumid", "all", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.Forum model = new BCW.BLL.Forum().GetForumBasic(forumid);
        if (model == null)
            Utils.Error("不存在的论坛", "");

        if (model.GroupId != 0)
        {
            Utils.Error("非主题论坛无此基金功能", "");
        }
        Master.Title = "【" + model.Title + "】基金";
        builder.Append(Out.Tab("<div class=\"title\">" + model.Title + "基金</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("金库:" + model.iCent + "" + ub.Get("SiteBz") + "");
        builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?act=fundget&amp;forumid=" + forumid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[取款]</a>");
        builder.Append("<br /><a href=\"" + Utils.getUrl("manwork.aspx?act=fundpay&amp;forumid=" + forumid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">我要捐款</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?act=fundlist&amp;ptype=2&amp;&amp;forumid=" + forumid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">支出明细</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("【最新捐款】<a href=\"" + Utils.getUrl("manwork.aspx?act=fundlist&amp;forumid=" + forumid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">光荣榜</a>");
        builder.Append(Out.Tab("</div>", ""));

        DataSet ds = new BCW.BLL.Forumfund().GetList("TOP 10 UsID,UsName,PayCent,AddTime", "Types=1 and ForumId=" + forumid + " ORDER BY ID DESC");
        if (ds != null && ds.Tables[0].Rows.Count != 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + ds.Tables[0].Rows[i]["UsID"].ToString() + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + ds.Tables[0].Rows[i]["UsName"].ToString() + "</a>捐了" + ds.Tables[0].Rows[i]["PayCent"].ToString() + "" + ub.Get("SiteBz") + "(" + DT.FormatDate(DateTime.Parse(ds.Tables[0].Rows[i]["AddTime"].ToString()), 5) + ")");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        else
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("没有相关记录..");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?forumid=" + forumid + "") + "\">版务中心</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void FundPayPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int forumid = int.Parse(Utils.GetRequest("forumid", "all", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.Forum model = new BCW.BLL.Forum().GetForumBasic(forumid);
        if (model == null)
            Utils.Error("不存在的论坛", "");

        if (model.GroupId != 0)
        {
            Utils.Error("非主题论坛无此基金功能", "");
        }

        string info = Utils.GetRequest("info", "all", 1, "", "");
        string paypwd = new BCW.BLL.User().GetUsPled(meid);
        if (info == "ok")
        {
            long payCent = Convert.ToInt64(Utils.GetRequest("payCent", "post", 2, @"^[1-9]\d*$", "请正确填写捐款数目"));
            string Pwd = Utils.GetRequest("Pwd", "post", 2, @"^[\s\S]{1,20}$", "请正确填写支付密码");
            if (!Utils.MD5Str(Pwd).Equals(paypwd))
            {
                Utils.Error("支付密码不正确", "");
            }
            if (new BCW.BLL.User().GetGold(meid) < payCent)
            {
                Utils.Error("你的" + ub.Get("SiteBz") + "不足", "");
            }
            BCW.Model.Forumfund addmodel = new BCW.Model.Forumfund();
            addmodel.ForumId = forumid;
            addmodel.Types = 1;
            addmodel.UsID = meid;
            addmodel.UsName = new BCW.BLL.User().GetUsName(meid);
            addmodel.Content = "";
            addmodel.PayCent = payCent;
            addmodel.AddTime = DateTime.Now;
            new BCW.BLL.Forumfund().Add(addmodel);
            new BCW.BLL.Forum().UpdateiCent(forumid, payCent);
            //操作币
            new BCW.BLL.User().UpdateiGold(meid, -payCent, "" + model.Title + "-捐款");
            Utils.Success("捐款", "捐款成功..", Utils.getUrl("manwork.aspx?act=fund&amp;forumid=" + forumid + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            if (paypwd == "")
            {
                Utils.Error("你还没有设置支付密码呢<br /><a href=\"" + Utils.getUrl("myedit.aspx?act=paypwd&amp;backurl=" + Utils.PostPage(1) + "") + "\">&gt;马上设置</a>", "");
            }
            Master.Title = "【" + model.Title + "】捐款";
            builder.Append(Out.Tab("<div class=\"title\">" + model.Title + "捐款</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("金库:" + model.iCent + "" + ub.Get("SiteBz") + "<br />");
            builder.Append("您自带:" + new BCW.BLL.User().GetGold(meid) + "" + ub.Get("SiteBz") + "");
            builder.Append(Out.Tab("</div>", ""));
            strText = "" + ub.Get("SiteBz") + "数目:/,支付密码:/,,,,";
            strName = "payCent,Pwd,forumid,info,act,backurl";
            strType = "num,password,hidden,hidden,hidden,hidden";
            strValu = "''" + forumid + "'ok'fundpay'" + Utils.getPage(0) + "";
            strEmpt = "false,false,false,false,false,flase";
            strIdea = "/";
            strOthe = "确定捐款,manwork.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?act=fund&amp;forumid=" + forumid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">基金</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?forumid=" + forumid + "") + "\">版务中心</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }


    private void FundListPage()
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));
        int forumid = int.Parse(Utils.GetRequest("forumid", "all", 2, @"^[1-9]\d*$", "ID错误"));
        string ForumName = new BCW.BLL.Forum().GetTitle(forumid);
        if (ForumName == "")
            Utils.Error("不存在的论坛", "");
        string Title = string.Empty;
        if (ptype == 1)
            Title = "光荣榜";
        else
            Title = "支出明细";

        Master.Title = "【" + ForumName + "】" + Title + "";
        builder.Append(Out.Tab("<div class=\"title\">" + ForumName + "" + Title + "</div>", ""));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "ptype", "forumid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        strWhere = "Types=" + ptype + " and ForumId=" + forumid + "";

        // 开始读取列表
        IList<BCW.Model.Forumfund> listForumfund = new BCW.BLL.Forumfund().GetForumfunds(pageIndex, pageSize, strWhere, out recordCount);
        if (listForumfund.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Forumfund n in listForumfund)
            {

                if (k == 1)
                    builder.Append(Out.Tab("<div>", ""));
                else
                    builder.Append(Out.Tab("<div>", "<br />"));

                string Why = string.Empty;
                if (n.Content != "")
                    Why = ",原因:" + n.Content + "";

                if (ptype == 1)
                    builder.AppendFormat("<a href=\"" + Utils.getUrl("uinfo.aspx?uid={0}&amp;backurl=" + Utils.getPage(0) + "") + "\">{1}({0})</a>捐了{2}" + ub.Get("SiteBz") + "[{3}]", n.UsID, n.UsName, n.PayCent, DT.FormatDate(n.AddTime, 5));
                else
                {
                    if (n.ToID != 0)//如果有打赏给人的
                    {
                        string name = new BCW.BLL.User().GetUsName(n.ToID);
                        builder.Append(Out.SysUBB("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + n.UsName + "(" + n.UsID + ")</a>取出" + n.PayCent + ub.Get("SiteBz") + Why + "[" + DT.FormatDate(n.AddTime, 5) + "]"));
                        // builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + n.UsName + "(" + n.UsID + ")</a>取出"+n.PayCent+ ub.Get("SiteBz")+Why+"["+ DT.FormatDate(n.AddTime, 5)+"]");
                        // builder.AppendFormat("<a href=\"" + Utils.getUrl("uinfo.aspx?uid={0}&amp;backurl=" + Utils.getPage(0) + "") + "\">{1}({0})</a>取出{2}" + ub.Get("SiteBz") + "{3}打赏给<a href=\"" + Utils.getUrl("uinfo.aspx?uid="+n.ToID+"&amp;backurl=" + Utils.getPage(0) + "") + "\">"+name+"</a>[{4}]", n.UsID, n.UsName, n.PayCent, Why, DT.FormatDate(n.AddTime, 5));
                    }
                    else//原本的
                    {
                        builder.AppendFormat("<a href=\"" + Utils.getUrl("uinfo.aspx?uid={0}&amp;backurl=" + Utils.getPage(0) + "") + "\">{1}({0})</a>取出{2}" + ub.Get("SiteBz") + "{3}[{4}]", n.UsID, n.UsName, n.PayCent, Why, DT.FormatDate(n.AddTime, 5));
                    }
                }
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
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?act=fund&amp;forumid=" + forumid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">基金</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?forumid=" + forumid + "") + "\">版务中心</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void FundGetPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int forumid = int.Parse(Utils.GetRequest("forumid", "all", 2, @"^[1-9]\d*$", "ID错误"));
        string ForumName = new BCW.BLL.Forum().GetTitle(forumid);
        if (ForumName == "")
            Utils.Error("不存在的论坛", "");

        //能取款的ID
        string GetiCentID = ub.GetSub("BbsGetiCentID", xmlPath);
        string iGetiCentID = ub.GetSub("BbsGetiCentID" + forumid + "", xmlPath);

        string info = Utils.GetRequest("info", "all", 1, "", "");
        string paypwd = new BCW.BLL.User().GetUsPled(meid);
        if (info == "ok")
        {
            //管理安全提示
            string[] p_pageArr = { "act", "forumid", "payCent", "Why", "Pwd", "info", "backurl" };
            BCW.User.Role.AdminSafePage(meid, Utils.getPageUrl(), p_pageArr, "post");

            if (!("#" + GetiCentID + "#").Contains("#" + meid + "#") && !("#" + iGetiCentID + "#").Contains("#" + meid + "#"))
            {
                Utils.Error("你没有取款的权限", "");
            }

            //进一步检测版主权限
            if (!("#" + GetiCentID + "#").Contains("#" + meid + "#"))
            {
                if (!("#" + GetAdminSubIDS(forumid) + "#").Contains("#" + meid + "#"))
                {
                    Utils.Error("你没有取款的权限", "");
                }
            }
            long payCent = Convert.ToInt64(Utils.GetRequest("payCent", "post", 2, @"^[1-9]\d*$", "请正确填写取款数目"));
            string Why = Utils.GetRequest("Why", "post", 2, @"^[\s\S]{1,50}$", "原因限1-50字内");
            string Pwd = Utils.GetRequest("Pwd", "post", 2, @"^[\s\S]{1,20}$", "请正确填写支付密码");
            if (!Utils.MD5Str(Pwd).Equals(paypwd))
            {
                Utils.Error("支付密码不正确", "");
            }
            if (new BCW.BLL.Forum().GetiCent(forumid) < payCent)
            {
                Utils.Error("基金不足" + payCent + "" + ub.Get("SiteBz") + "", "");
            }
            BCW.Model.Forumfund addmodel = new BCW.Model.Forumfund();
            addmodel.ForumId = forumid;
            addmodel.Types = 2;
            addmodel.UsID = meid;
            addmodel.UsName = new BCW.BLL.User().GetUsName(meid);
            addmodel.Content = Why;
            addmodel.PayCent = payCent;
            addmodel.AddTime = DateTime.Now;
            new BCW.BLL.Forumfund().Add(addmodel);
            new BCW.BLL.Forum().UpdateiCent(forumid, -payCent);
            //操作币
            new BCW.BLL.User().UpdateiGold(meid, payCent, "" + ForumName + "-取款");
            Utils.Success("取款", "取款成功..", Utils.getUrl("manwork.aspx?act=fund&amp;forumid=" + forumid + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            if (paypwd == "")
            {
                Utils.Error("你还没有设置支付密码呢<br /><a href=\"" + Utils.getUrl("myedit.aspx?act=paypwd&amp;backurl=" + Utils.PostPage(1) + "") + "\">&gt;马上设置</a>", "");
            }
            Master.Title = "【" + ForumName + "】取款";
            builder.Append(Out.Tab("<div class=\"title\">" + ForumName + "取款</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("金库:" + new BCW.BLL.Forum().GetiCent(forumid) + "" + ub.Get("SiteBz") + "<br />");
            builder.Append("您自带:" + new BCW.BLL.User().GetGold(meid) + "" + ub.Get("SiteBz") + "");
            builder.Append(Out.Tab("</div>", ""));
            if (("#" + GetiCentID + "#").Contains("#" + meid + "#") || ("#" + iGetiCentID + "#").Contains("#" + meid + "#"))
            {
                strText = "" + ub.Get("SiteBz") + "数目:/,取款原因(50字内):/,您的支付密码:/,,,,";
                strName = "payCent,Why,Pwd,forumid,info,act,backurl";
                strType = "num,text,password,hidden,hidden,hidden,hidden";
                strValu = "'''" + forumid + "'ok'fundget'" + Utils.getPage(0) + "";
                strEmpt = "false,false,false,false,false,false,flase";
                strIdea = "/";
                strOthe = "确定取出,manwork.aspx,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }

            builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
            builder.Append("取款请联系以下管理员↓");
            builder.Append(Out.Tab("</div>", ""));

            if (GetiCentID != "" || iGetiCentID != "")
            {
                if (iGetiCentID != "")
                    GetiCentID = GetiCentID + "#" + iGetiCentID;

                string[] sName = Regex.Split(GetiCentID, "#");
                int pageIndex;
                int recordCount;
                int pageSize = 15;
                string[] pageValUrl = { "act", "ptype", "backurl" };
                pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                if (pageIndex == 0)
                    pageIndex = 1;

                //总记录数
                recordCount = sName.Length;

                int stratIndex = (pageIndex - 1) * pageSize;
                int endIndex = pageIndex * pageSize;
                int k = 0;
                for (int i = 0; i < sName.Length; i++)
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        if ((k + 1) % 2 == 0)
                            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));

                        builder.Append("" + (i + 1) + ".<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + sName[i] + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(Convert.ToInt32(sName[i])) + "</a>");
                        builder.Append(Out.Tab("</div>", ""));
                    }
                    if (k == endIndex)
                        break;
                    k++;
                }
                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 1));
            }
            else
            {
                builder.Append("还没有取款管理员，请联系客服");
            }
            if (("#" + ub.GetSub("BbsGetiCentID", xmlPath) + "#").Contains("#" + meid + "#"))
            {
                builder.Append(Out.Tab("<div>", Out.Hr()));
                builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?act=fundgetre&amp;forumid=" + forumid + "") + "\">设置版主取款权限</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?act=fund&amp;forumid=" + forumid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">基金</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?forumid=" + forumid + "") + "\">版务中心</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private void FundGetRePage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int forumid = int.Parse(Utils.GetRequest("forumid", "all", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.Forum model = new BCW.BLL.Forum().GetForumBasic(forumid);
        if (model == null)
            Utils.Error("不存在的论坛", "");

        if (model.GroupId != 0)
        {
            Utils.Error("非主题论坛无此基金功能", "");
        }


        //能设置取款的ID
        string GetiCentID = ub.GetSub("BbsGetiCentID", xmlPath);

        if (!("#" + GetiCentID + "#").Contains("#" + meid + "#"))
        {
            Utils.Error("你没有设置取款ID的权限", "");
        }

        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {
            //管理安全提示
            string[] p_pageArr = { "act", "forumid", "GetiCentID" + forumid + "", "info", "backurl" };
            BCW.User.Role.AdminSafePage(meid, Utils.getPageUrl(), p_pageArr, "post");

            string iGetiCentID = Utils.GetRequest("GetiCentID" + forumid + "", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "多个ID请用#分隔，可以留空");
            if (iGetiCentID != "")
            {
                string[] IdTemp = iGetiCentID.Split('#');
                for (int i = 0; i < IdTemp.Length; i++)
                {
                    if (!GetAdminSubIDS(forumid).Contains("#" + IdTemp[i] + "#"))
                    {
                        Utils.Error("ID:" + IdTemp[i] + "不是本版版主，请不要跨权限设置", "");
                    }
                }
            }
            ub xml = new ub();
            Application.Remove(xmlPath);//清缓存
            xml.ReloadSub(xmlPath); //加载配置

            xml.dss["BbsGetiCentID" + forumid + ""] = iGetiCentID;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);

            Utils.Success("设置", "设置取款版主成功..", Utils.getUrl("manwork.aspx?act=fundgetre&amp;forumid=" + forumid + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            Master.Title = "【" + model.Title + "】设置取款版主";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("设置取款版主");
            builder.Append(Out.Tab("</div>", ""));
            strText = "本版取款版主ID(多个请用#分隔):/,,,,";
            strName = "GetiCentID" + forumid + ",forumid,info,act,backurl";
            strType = "textarea,hidden,hidden,hidden,hidden";
            strValu = "" + ub.GetSub("BbsGetiCentID" + forumid + "", xmlPath) + "'" + forumid + "'ok'fundgetre'" + Utils.getPage(0) + "";
            strEmpt = "true,false,false,false,flase";
            strIdea = "/";
            strOthe = "确定设置,manwork.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?act=fund&amp;forumid=" + forumid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">基金</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?forumid=" + forumid + "") + "\">版务中心</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private void FansAddPage()
    {
        Master.Title = "添加粉丝";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int forumid = int.Parse(Utils.GetRequest("forumid", "all", 1, @"^[1-9]\d*$", "0"));
        if (!new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_AddForumBlack, meid, forumid))
        {
            Utils.Error("你的权限不足", "");
        }
        string ForumName = new BCW.BLL.Forum().GetTitle(forumid);
        if (ForumName == "")
            Utils.Error("不存在的论坛", "");

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("添加本版粉丝");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "forumid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "Types=2 and ForumId=" + forumid + "";

        // 开始读取列表
        IList<BCW.Model.Medal> listMedal = new BCW.BLL.Medal().GetMedals(pageIndex, pageSize, strWhere, out recordCount);
        if (listMedal.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Medal n in listMedal)
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

                builder.AppendFormat("{0}<img src=\"{1}\" alt=\"load\"/>", n.Title, n.ImageUrl);
                builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?act=fansinfo&amp;id=" + n.ID + "&amp;forumid=" + forumid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[选择]</a>");
                if (n.Notes != "")
                    builder.AppendFormat("<br />{0}", n.Notes);

                k++;
                builder.Append(Out.Tab("</div>", ""));

            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

        }
        else
        {
            builder.Append(Out.Div("div", "本论坛还没有粉丝标识图，请联系管理员"));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("forum.aspx?forumid=" + forumid + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?forumid=" + forumid + "") + "\">版务中心</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void FansInfoPage()
    {
        Master.Title = "添加粉丝";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
        int forumid = int.Parse(Utils.GetRequest("forumid", "all", 2, @"^[1-9]\d*$", "论坛ID错误"));
        if (!new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_AddForumBlack, meid, forumid))
        {
            Utils.Error("你的权限不足", "");
        }
        string ForumName = new BCW.BLL.Forum().GetTitle(forumid);
        if (ForumName == "")
            Utils.Error("不存在的论坛", "");

        BCW.Model.Medal model = new BCW.BLL.Medal().GetMedal(id);

        if (!new BCW.BLL.Medal().ExistsForumId(id, forumid))
        {
            Utils.Error("不存在的记录", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");

        if (info == "ok")
        {
            //管理安全提示
            string[] p_pageArr = { "act", "forumid", "uid", "info", "id" };
            BCW.User.Role.AdminSafePage(meid, Utils.getPageUrl(), p_pageArr, "post");

            int uid = int.Parse(Utils.GetRequest("uid", "all", 2, @"^[1-9]\d*$", "会员ID错误"));

            if (new BCW.BLL.Medal().ExistsTypes(2, uid))
            {
                Utils.Error("该会员ID已存在粉丝标识，需要会员自己联系原论坛版主解除", "");
            }
            //临时加入
            if (string.IsNullOrEmpty(model.PayIDtemp) || !model.PayIDtemp.Contains("#" + uid + "#"))
            {
                string PayIDtemp = model.PayIDtemp + "#" + uid + "#";
                new BCW.BLL.Medal().UpdatePayIDtemp(id, PayIDtemp);
            }

            //记录日志
            string strLog = string.Empty;
            strLog = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + new BCW.BLL.User().GetUsName(meid) + "[/url]向[url=/bbs/uinfo.aspx?uid=" + uid + "]" + new BCW.BLL.User().GetUsName(uid) + "[/url]发出加粉丝请求";

            new BCW.BLL.Forumlog().Add(13, forumid, strLog);

            new BCW.BLL.Guest().Add(uid, new BCW.BLL.User().GetUsName(uid), "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + new BCW.BLL.User().GetUsName(meid) + "[/url]申请加您为论坛-[url=/bbs/forum.aspx?forumid=" + forumid + "]" + ForumName + "[/url]的粉丝.[br]您可以[url=/bbs/manwork.aspx?act=fansok&amp;forumid=" + forumid + "&amp;ptype=1]接受并加入[/url]或[url=/bbs/manwork.aspx?act=fansok&amp;forumid=" + forumid + "&amp;ptype=2]拒绝请求[/url]");
            Utils.Success("添加粉丝", "申请添加成功，请等待会员回应...", Utils.getUrl("manwork.aspx?act=fans&amp;forumid=" + forumid + ""), "1");
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.AppendFormat("授予:{0}<img src=\"{1}\" alt=\"load\"/>", model.Title, model.ImageUrl);
            builder.Append(Out.Tab("</div>", ""));
            strText = "会员ID:/,,,,";
            strName = "uid,id,forumid,act,info";
            strType = "num,hidden,hidden,hidden,hidden";
            strValu = "'" + id + "'" + forumid + "'fansinfo'ok";
            strEmpt = "false,false,false,false,false";
            strIdea = "/";
            strOthe = "确定设置,manwork.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?act=fansadd&amp;forumid=" + forumid + "") + "\">&lt;&lt;重新选择</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "") + "\">上级</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?forumid=" + forumid + "") + "\">版务中心</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private void FansOkPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int forumid = int.Parse(Utils.GetRequest("forumid", "all", 2, @"^[1-9]\d*$", "论坛ID错误"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 2, @"^[1-2]$", "类型错误"));
        string ForumName = new BCW.BLL.Forum().GetTitle(forumid);
        if (ForumName == "")
            Utils.Error("不存在的论坛", "");

        if (!new BCW.BLL.Medal().ExistsPayIDtemp(forumid, meid))
        {
            Utils.Error("不存在的记录", "");
        }

        BCW.Model.Medal model = null;

        if (ptype == 1)//接受
        {
            if (new BCW.BLL.Medal().ExistsTypes(2, meid))
            {
                Utils.Error("你已经是某个论坛的粉丝了", "");
            }
            model = new BCW.BLL.Medal().GetMedalMe(forumid, meid);

            string PayID = model.PayID + "#" + meid + "#";
            string ExTime = "1990-1-1";

            string PayExTime = model.PayExTime + "#" + ExTime + "#";
            new BCW.BLL.Medal().UpdatePayID(model.ID, PayID, PayExTime);
            //清缓存
            string CacheKey = CacheName.App_UserMedal(meid);
            DataCache.RemoveByPattern(CacheKey);
            //清临时记录
            string PayIDtemp = model.PayIDtemp.Replace("#" + meid + "#", "");
            new BCW.BLL.Medal().UpdatePayIDtemp(model.ID, PayIDtemp);
            //记录动态
            string strLog = "恭喜[url=/bbs/uinfo.aspx?uid=" + meid + "]" + new BCW.BLL.User().GetUsName(meid) + "[/url]成为[url=/bbs/forum.aspx?forumid=" + forumid + "]" + ForumName + "[/url]的粉丝啦！";
            new BCW.BLL.Action().Add(20, 0, 0, "", strLog);

            //记录日志
            new BCW.BLL.Forumlog().Add(13, forumid, strLog);

            Utils.Success("接受加入粉丝", "加入" + ForumName + "-粉丝成功...", Utils.getUrl("manwork.aspx?act=fans&amp;forumid=" + forumid + ""), "1");
        }
        else//拒绝
        {
            model = new BCW.BLL.Medal().GetMedalMe(forumid, meid);
            //清临时记录
            string PayIDtemp = model.PayIDtemp.Replace("#" + meid + "#", "");
            new BCW.BLL.Medal().UpdatePayIDtemp(model.ID, PayIDtemp);
            Utils.Success("拒绝加入粉丝", "拒绝加入" + ForumName + "-粉丝成功...", Utils.getPage("manwork.aspx?act=fans&amp;forumid=" + forumid + ""), "1");
        }

    }

    private void FansPage()
    {
        int meid = new BCW.User.Users().GetUsId();

        int forumid = int.Parse(Utils.GetRequest("forumid", "all", 1, @"^[1-9]\d*$", "0"));
        string ForumName = new BCW.BLL.Forum().GetTitle(forumid);
        if (ForumName == "")
            Utils.Error("不存在的论坛", "");

        Master.Title = "" + ForumName + "-粉丝团";

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("" + ForumName + "-粉丝团");
        builder.Append(Out.Tab("</div>", ""));
        string PayID = GetForumFans(forumid);
        if (!string.IsNullOrEmpty(PayID))
        {
            string[] sName = Regex.Split(PayID, "#");

            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string[] pageValUrl = { "act", "forumid", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            //总记录数
            recordCount = sName.Length;

            int stratIndex = (pageIndex - 1) * pageSize;
            int endIndex = pageIndex * pageSize;
            int k = 0;
            for (int i = 0; i < sName.Length; i++)
            {
                if (k >= stratIndex && k < endIndex)
                {
                    if ((k + 1) % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));

                    int uid = Convert.ToInt32(sName[i].ToString());
                    builder.Append("" + (i + 1) + "." + BCW.User.Users.SetForumPic(uid) + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + uid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(uid) + "</a>");
                    if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_AddForumBlack, meid, forumid) || uid == meid)
                    {
                        string sText = "[撤]";
                        if (uid == meid)
                            sText = "[退]";

                        builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?act=fansdel&amp;hid=" + uid + "&amp;forumid=" + forumid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + sText + "</a>");
                    }

                    builder.Append(Out.Tab("</div>", ""));
                }
                if (k == endIndex)
                    break;
                k++;
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        builder.Append(Out.Tab("<div>", Out.Hr()));
        if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_AddForumBlack, meid, forumid))
        {
            builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?act=fansadd&amp;forumid=" + forumid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">添加粉丝&gt;&gt;</a><br />");
        }
        builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?act=logview&amp;forumid=" + forumid + "&amp;ptype=13&amp;backurl=" + Utils.PostPage(1) + "") + "\">设粉丝日志</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?forumid=" + forumid + "") + "\">版务中心</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void FansDelPage()
    {
        Master.Title = "本版粉丝";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int hid = int.Parse(Utils.GetRequest("hid", "all", 2, @"^[1-9]\d*$", "会员ID错误"));
        int forumid = int.Parse(Utils.GetRequest("forumid", "all", 2, @"^[1-9]\d*$", "论坛ID错误"));
        if (!new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_AddForumBlack, meid, forumid))
        {
            if (hid != meid)
                Utils.Error("你的权限不足", "");
        }
        string ForumName = new BCW.BLL.Forum().GetTitle(forumid);
        if (ForumName == "")
            Utils.Error("不存在的论坛", "");

        if (!new BCW.BLL.User().Exists(hid))
        {
            Utils.Error("不存在的会员记录", "");
        }

        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {
            //管理安全提示
            if (hid != meid)
            {
                string[] p_pageArr = { "act", "forumid", "hid", "info", "backurl" };
                BCW.User.Role.AdminSafePage(meid, Utils.getPageUrl(), p_pageArr, "get");
            }
            DataSet ds = new BCW.BLL.Medal().GetList("ID", "Types=2 and ForumId=" + forumid + " and PayID LIKE '%#" + hid + "#%'");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                {
                    int id = int.Parse(ds.Tables[0].Rows[k]["ID"].ToString());
                    BCW.Model.Medal model = new BCW.BLL.Medal().GetMedalMe(id);
                    string sPayID = string.Empty;
                    sPayID = Utils.Mid(model.PayID, 1, model.PayID.Length);
                    sPayID = Utils.DelLastChar(sPayID, "#");
                    string[] temp = Regex.Split(sPayID, "##");
                    //得到位置
                    int strPn = 0;
                    for (int i = 0; i < temp.Length; i++)
                    {
                        if (temp[i].ToString() == hid.ToString())
                        {
                            strPn = i;
                            break;
                        }
                    }
                    string sPayExTime = string.Empty;
                    sPayExTime = Utils.Mid(model.PayExTime, 1, model.PayExTime.Length);
                    sPayExTime = Utils.DelLastChar(sPayExTime, "#");
                    string[] temp2 = Regex.Split(sPayExTime, "##");
                    string PayExTime = string.Empty;
                    for (int i = 0; i < temp2.Length; i++)
                    {
                        if (i != strPn)
                        {
                            PayExTime += "#" + temp2[i] + "#";
                        }

                    }
                    string PayID = model.PayID.Replace("#" + hid + "#", "");
                    //更新
                    new BCW.BLL.Medal().UpdatePayID(id, PayID, PayExTime);
                    //清缓存
                    string CacheKey = CacheName.App_UserMedal(hid);
                    DataCache.RemoveByPattern(CacheKey);

                }

                //记录日志
                string strLog = string.Empty;
                if (meid == hid)
                    strLog = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + new BCW.BLL.User().GetUsName(meid) + "[/url]退出了[url=/bbs/forum.aspx?forumid=" + forumid + "]" + ForumName + "[/url]粉丝团！";
                else
                    strLog = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + new BCW.BLL.User().GetUsName(meid) + "[/url]将[url=/bbs/uinfo.aspx?uid=" + hid + "]" + new BCW.BLL.User().GetUsName(hid) + "[/url]从[url=/bbs/forum.aspx?forumid=" + forumid + "]" + ForumName + "[/url]粉丝团撤除";

                new BCW.BLL.Forumlog().Add(13, forumid, strLog);
            }

            Utils.Success("撤销", "撤销粉丝成功！", Utils.getUrl("manwork.aspx?act=fans&amp;forumid=" + forumid + ""), "1");

        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定要撤出这个粉丝团吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?act=fansdel&amp;info=ok&amp;hid=" + hid + "&amp;forumid=" + forumid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定撤除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?act=fans&amp;forumid=" + forumid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }

    }


    private void FlowPage()
    {
        int meid = new BCW.User.Users().GetUsId();

        int forumid = int.Parse(Utils.GetRequest("forumid", "get", 2, @"^[1-9]\d*$", "ID错误"));
        int bid = int.Parse(Utils.GetRequest("bid", "get", 1, @"^[1-9]\d*$", "0"));
        string ForumName = new BCW.BLL.Forum().GetTitle(forumid);
        if (ForumName == "")
            Utils.Error("不存在的论坛", "");

        Master.Title = ForumName + "滚动列表";

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("" + ForumName + "滚动列表");
        builder.Append(Out.Tab("</div>", "<br />"));
        bool IsFlow = IsGdID(meid);
        if (IsFlow == false)
            IsFlow = new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_TopicsSet, meid, forumid);

        if (bid > 0)
        {
            BCW.Model.Text flow = new BCW.BLL.Text().GetText(bid);
            if (flow != null)
            {
                if (flow.IsFlow == 1 && flow.ForumId == forumid)
                {
                    builder.Append(Out.Tab("<div class=\"text\">", ""));
                    builder.Append("<a href=\"" + Utils.getUrl("topic.aspx?forumid=" + flow.ForumId + "&amp;bid=" + bid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + flow.Title + "</a>");
                    if (IsFlow)
                    {
                        builder.Append("[<a href=\"" + Utils.getUrl("textmanage.aspx?act=delflow&amp;forumid=" + flow.ForumId + "&amp;bid=" + bid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">撤</a>]");
                    }
                    builder.Append(Out.Tab("</div>", Out.Hr()));
                }
            }
        }
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string strOrder = "";
        string[] pageValUrl = { "act", "forumid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "ForumID=" + forumid + " and IsFlow=1 and ID<>" + bid + "";
        strOrder = "AddTime Desc";

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
                builder.Append("<a href=\"" + Utils.getUrl("topic.aspx?forumid=" + n.ForumId + "&amp;bid=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + ((pageIndex - 1) * pageSize + k) + "." + n.Title + "</a>");
                if (IsFlow)
                {
                    builder.Append("[<a href=\"" + Utils.getUrl("textmanage.aspx?act=delflow&amp;forumid=" + n.ForumId + "&amp;bid=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">撤</a>]");
                }
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有更多的记录.."));
        }
        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "") + "\">返回" + ForumName + "</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("forum.aspx?forumid=" + forumid + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?forumid=" + forumid + "") + "\">版务中心</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    /// <summary>
    /// 得到某论坛有个性勋章的ID集合
    /// </summary>
    /// <param name="forumid"></param>
    /// <returns></returns>
    private string GetForumFans(int forumid)
    {
        string PayID = string.Empty;
        DataSet ds = new BCW.BLL.Medal().GetList("PayID", "Types=2 and ForumId=" + forumid + "");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                PayID += "" + ds.Tables[0].Rows[i]["PayID"].ToString();
            }
        }
        PayID = PayID.Replace("##", "#");
        PayID = Utils.Mid(PayID, 1, PayID.Length - 2);

        return PayID;
    }

    /// <summary>
    /// 得到某论坛版主所有ID
    /// </summary>
    private string GetAdminSubIDS(int forumid)
    {
        string SubAdmin = string.Empty;
        DataSet ds = new BCW.BLL.Role().GetList("UsID", "ForumID=" + forumid + " and (OverTime>='" + DateTime.Now + "' OR OverTime='1990-1-1 00:00:00') and Status=0");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                SubAdmin += "#" + ds.Tables[0].Rows[i]["UsID"].ToString();
            }
            SubAdmin += "#";
        }
        return SubAdmin;
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