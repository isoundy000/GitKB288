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

public partial class Manage_votes : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "投票管理";

        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "add":
                AddPage();
                break;
            case "save":
                SavePage();
                break;
            case "edit":
                EditPage();
                break;
            case "editsave":
                EditSavePage();
                break;
            case "del":
                DelPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("投票管理");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 1)
            builder.Append("正在投票|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("votes.aspx?ptype=1") + "\">正在</a>|");

        if (ptype == 2)
            builder.Append("已结束|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("votes.aspx?ptype=2") + "\">结束</a>|");

        builder.Append("<a href=\"" + Utils.getUrl("votes.aspx?act=add") + "\">发布</a>");

        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "uid", "ptype" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        if (ptype == 1)
            strWhere += " Types=0 and Status=0";
        else
            strWhere += " Types=0 and Status=1";

        // 开始读取列表
        IList<BCW.Model.Votes> listVotes = new BCW.BLL.Votes().GetVotess(pageIndex, pageSize, strWhere, out recordCount);
        if (listVotes.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Votes n in listVotes)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));

                string sFace = string.Empty;
                if (n.VoteTiple == 0)
                    sFace = "(单选)";
                else
                    sFace = "(多选)";
                builder.AppendFormat("<a href=\"" + Utils.getUrl("votes.aspx?act=edit&amp;id={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">[管理]</a>{1}.<a href=\"" + Utils.getUrl("/votes.aspx?id={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}</a>{3}", n.ID, (pageIndex - 1) * pageSize + k, n.Title, sFace);


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
        builder.Append("<a href=\"" + Utils.getUrl("votes.aspx?act=add") + "\">发布投票</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void AddPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("发布投票");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "投票标题:/,投票内容:/,投票选项(用#隔开):/,投票类型:/,等级要求(0级则不限制):/,投票性质:/,截止投票时间:/,";
        string strName = "Title,Content,Vote,VoteType,VoteLeven,VoteTiple,VoteExTime,act";
        string strType = "text,textarea,text,select,num,select,date,hidden";
        string strValu = "''''0''" + DT.FormatDate(DateTime.Now.AddDays(1), 0) + "'save";
        string strEmpt = "false,false,false,0|任何人可见|1|投票后可见,false,0|限制单选|1|允许多选,false,false";
        string strIdea = "/";
        string strOthe = "发布投票|reset,votes.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("votes.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void SavePage()
    {
        string Title = Utils.GetRequest("Title", "post", 2, @"^[^\^]{1,30}$", "标题限1-30字");
        string Content = Utils.GetRequest("Content", "post", 2, @"^[^\^]{1,5000}$", "内容限1-5000字");
        string Vote = Utils.GetRequest("Vote", "post", 2, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){1,500}$", "投票选项必须用#隔开、选项两个以上");
        int VoteType = int.Parse(Utils.GetRequest("VoteType", "post", 2, @"^[0-1]$", "投票类型选择错误"));
        int VoteLeven = int.Parse(Utils.GetRequest("VoteLeven", "post", 2, @"^[0-9]\d*$", "等级数填写错误"));
        int VoteTiple = int.Parse(Utils.GetRequest("VoteTiple", "post", 2, @"^[0-1]$", "投票性质选择错误"));
        DateTime VoteExTime = Utils.ParseTime(Utils.GetRequest("VoteExTime", "post", 2, DT.RegexTime, "截止投票时间格式填写出错,正确格式如" + DateTime.Now.ToString("yyy-MM-dd HH:mm:ss") + ""));
        
        //统计项
        string[] sVote = Vote.Split("#".ToCharArray());
        string sAddVote = string.Empty;
        for (int i = 0; i < sVote.Length; i++)
        {
            sAddVote += "#0";
        }
        sAddVote = Utils.Mid(sAddVote, 1, sAddVote.Length);

        BCW.Model.Votes model = new BCW.Model.Votes();
        model.Types = 0;
        model.UsID = 0;
        model.Title = Title;
        model.Content = Content;
        model.Vote = Vote;
        model.AddVote = sAddVote;
        model.VoteType = VoteType;
        model.VoteLeven = VoteLeven;
        model.VoteTiple = VoteTiple;
        model.Readcount = 0;
        model.VoteExTime = VoteExTime;
        model.AddTime = DateTime.Now;

        new BCW.BLL.Votes().Add(model);
        Utils.Success("发布投票", "发表投票成功..", Utils.getUrl("votes.aspx"), "1");
    }

    private void EditPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Votes().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        BCW.Model.Votes model = new BCW.BLL.Votes().GetVotes(id);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("修改投票");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "投票标题:/,投票内容:/,投票选项(用#隔开):/,投票类型:/,等级要求(0级则不限制):/,投票性质:/,人气点击:/,发布时间:/,截止投票时间:/,状态:/,,";
        string strName = "Title,Content,Vote,VoteType,VoteLeven,VoteTiple,Readcount,AddTime,VoteExTime,Status,id,act";
        string strType = "text,textarea,text,select,num,select,num,date,date,select,hidden,hidden";
        string strValu = "" + model.Title + "'" + model.Content + "'" + model.Vote + "'" + model.VoteType + "'" + model.VoteLeven + "'" + model.VoteTiple + "'" + model.Readcount + "'" + DT.FormatDate(model.AddTime, 0) + "'" + DT.FormatDate(model.VoteExTime, 0) + "'" + model.Status + "'" + id + "'editsave";
        string strEmpt = "false,false,false,0|任何人可见|1|投票后可见,false,0|限制单选|1|允许多选,false,false,false,0|正常|1|结束,false,false";
        string strIdea = "/";
        string strOthe = "修改投票|reset,votes.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("votes.aspx?act=del&amp;id=" + id + "") + "\">删除投票</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("votes.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void EditSavePage()
    {
        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[0-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Votes().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        string Title = Utils.GetRequest("Title", "post", 2, @"^[^\^]{1,30}$", "标题限1-30字");
        string Content = Utils.GetRequest("Content", "post", 2, @"^[^\^]{1,5000}$", "内容限1-5000字");
        string Vote = Utils.GetRequest("Vote", "post", 2, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){1,500}$", "投票选项必须用#隔开、选项两个以上");
        int VoteType = int.Parse(Utils.GetRequest("VoteType", "post", 2, @"^[0-1]$", "投票类型选择错误"));
        int VoteLeven = int.Parse(Utils.GetRequest("VoteLeven", "post", 2, @"^[0-9]\d*$", "等级数填写错误"));
        int VoteTiple = int.Parse(Utils.GetRequest("VoteTiple", "post", 2, @"^[0-1]$", "投票性质选择错误"));
        int Readcount = int.Parse(Utils.GetRequest("Readcount", "post", 2, @"^[0-9]\d*$", "人气点击填写错误"));
        int Status = int.Parse(Utils.GetRequest("Status", "post", 2, @"^[0-1]$", "状态选择错误"));
        DateTime VoteExTime = Utils.ParseTime(Utils.GetRequest("VoteExTime", "post", 2, DT.RegexTime, "截止投票时间格式填写出错,正确格式如" + DateTime.Now.ToString("yyy-MM-dd HH:mm:ss") + ""));
        DateTime AddTime = Utils.ParseTime(Utils.GetRequest("AddTime", "post", 2, DT.RegexTime, "发布时间格式填写出错,正确格式如" + DateTime.Now.ToString("yyy-MM-dd HH:mm:ss") + ""));

        BCW.Model.Votes model = new BCW.Model.Votes();
        model.ID = id;
        model.Types = 0;
        model.UsID = 0;
        model.Title = Title;
        model.Content = Content;
        model.Vote = Vote;
        model.VoteType = VoteType;
        model.VoteLeven = VoteLeven;
        model.VoteTiple = VoteTiple;
        model.Readcount = Readcount;
        model.Status = Status;
        model.VoteExTime = VoteExTime;
        model.AddTime = AddTime;

        new BCW.BLL.Votes().Update(model);
        Utils.Success("修改投票", "修改投票成功..", Utils.getUrl("votes.aspx?act=edit&amp;id=" + id + ""), "1");
    }

    private void DelPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        if (info != "ok")
        {
            Master.Title = "删除投票";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此投票记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("votes.aspx?info=ok&amp;act=del&amp;id=" + id + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("votes.aspx?act=edit&amp;id=" + id + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.BLL.Votes().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            //删除
            new BCW.BLL.Votes().Delete(id);
            Utils.Success("删除投票", "删除投票成功..", Utils.getUrl("votes.aspx"), "1");
        }
    }
}
