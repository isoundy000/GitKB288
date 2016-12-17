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

public partial class votes : System.Web.UI.Page
{
    protected string xmlPath = "/Controls/votes.xml";
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {        
        //维护提示
        if (ub.GetSub("VotesStatus", xmlPath) == "1")
        {
            Utils.Safe("投票系统");
        }
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "ok":
                OkPage();
                break;
            case "list":
                ListPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        Master.Title = ub.GetSub("VotesName", xmlPath);
                int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        if (ub.GetSub("VotesLogo", xmlPath) != "")
        {
            builder.Append("<img src=\"" + ub.GetSub("VotesLogo", xmlPath) + "\" alt=\"load\"/><br />");
        }
        else {
            builder.Append(Out.Tab("<div class=\"title\">" + ub.GetSub("VotesName", xmlPath) + "</div>", ""));
        }
        int id = int.Parse(Utils.GetRequest("id", "get", 1, @"^[0-9]\d*$", "0"));
        //得到最后一条投票记录
        if (id == 0)
            id = new BCW.BLL.Votes().GetLastId();

        BCW.Model.Votes model = new BCW.BLL.Votes().GetVotes(id);
        if (model != null)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("主题:" + model.Title + "<br />");
            builder.Append("内容:");
            int pageIndex;
            int recordCount;
            int pageSize = 500;
            string[] pageValUrl = { };
            pageIndex = Utils.ParseInt(Request.QueryString["vp"]);
            if (pageIndex == 0)
                pageIndex = 1;

            int pover = int.Parse(Utils.GetRequest("pover", "get", 1, @"^[0-9]\d*$", "0"));
            string content = BasePage.MultiContent(model.Content, pageIndex, pageSize, pover, out recordCount);
            builder.Append(Out.SysUBB(content));

            builder.Append(BasePage.MultiContentPage(model.Content, pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "vp", pover));
            builder.Append(Out.Tab("</div>", "<br />"));

            if (!string.IsNullOrEmpty(model.Vote))
            {
                string Votetxt = string.Empty;
                if (model.VoteTiple == 0)
                    Votetxt = "单选";
                else
                    Votetxt = "多选";

                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("=" + Votetxt + "投票=");
                builder.Append(Out.Tab("</div>", ""));
                string[] vote = model.Vote.Split("#".ToCharArray());
                string[] addvote = model.AddVote.Split("#".ToCharArray());
                //取总投票数
                long voteNum = 0;
                bool isvote = false;
                if (model.VoteType == 0)
                {
                    isvote = true;
                }
                else
                {
                    if (model.VoteTiple == 0)
                    {
                        if (("#" + model.VoteID + "#").Contains("#" + meid + "#"))
                            isvote = true;
                    }
                    else
                    {
                        string inum = "_" + meid;
                        if (("#" + model.VoteID + "#").Contains("" + inum + "#"))
                            isvote = true;
                    }
                }

                for (int i = 0; i < addvote.Length; i++)
                {
                    voteNum += Convert.ToInt64(addvote[i]);
                }
                for (int i = 0; i < vote.Length; i++)
                {
                    if (vote[i] != null)
                    {
                        builder.Append(Out.Tab("<div>", "<br />"));
                        builder.Append((i + 1) + "." + vote[i] + "");
                        if (isvote)
                            builder.Append("(" + addvote[i] + ")");

                        builder.Append("<a href=\"" + Utils.getUrl("votes.aspx?act=ok&amp;id=" + id + "&amp;voteid=" + i + "&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;投票</a>");
                        builder.Append(Out.Tab("</div>", ""));
                    }
                }
            }

            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("发布时间:" + model.AddTime);
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("votes.aspx?act=list") + "\">&gt;&gt;历史投票</a>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void OkPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int voteid = int.Parse(Utils.GetRequest("voteid", "all", 2, @"^[0-9]\d*$", "投票选择错误"));
        if (!new BCW.BLL.Votes().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int IsVerify = new BCW.BLL.User().GetIsVerify(meid);
        if (IsVerify == 0)
        {
            Utils.Error("您属于手工注册会员，还未通过短信验证<br /><a href=\"/reg.aspx\">免费验证会员</a>", "");
        }

        BCW.Model.Votes model = new BCW.BLL.Votes().GetVotes(id);

        //等级限制
        if (model.VoteLeven > 0)
        {
            int Leven = new BCW.BLL.User().GetLeven(meid);
            if (model.VoteLeven > Leven)
            {
                Utils.Error("本投票需" + model.VoteLeven + "级才能投票", "");
            }
        }
        if (model.VoteExTime < DateTime.Now)
        {
            Utils.Error("投票已截止了", "");
        }
        //生成多选投票识别
        string inum = voteid + "_" + meid;
        //投票逻辑
        if (!string.IsNullOrEmpty(model.Vote))
        {
            bool vote = false;
            string AddVoteString = string.Empty;
            string[] addvote = model.AddVote.Split("#".ToCharArray());
            for (int i = 0; i < addvote.Length; i++)
            {
                int ivote = int.Parse(addvote[i]);
                if (voteid == i)
                {
                    ivote = ivote + 1;
                    AddVoteString += "#" + ivote.ToString();
                }
                else
                {
                    AddVoteString += "#" + addvote[i];
                }
            }
            AddVoteString = Utils.Mid(AddVoteString, 1, AddVoteString.Length);

            if (!string.IsNullOrEmpty(model.VoteID))
            {
                string[] svoteid = model.VoteID.Split("#".ToCharArray());
                for (int i = 0; i < svoteid.Length; i++)
                {
                    if (model.VoteTiple == 0)//如果为单选
                    {
                        if (Convert.ToInt32(svoteid[i]) == meid)
                        {
                            vote = true;
                            break;
                        }
                    }
                    else
                    {
                        if (svoteid[i] == inum)
                        {
                            vote = true;
                            break;
                        }
                    }

                }
            }

            if (vote == true)
            {
                Utils.Error("请不要重复投票", "");
            }
            //写入数据库
            BCW.Model.Votes addmodel = new BCW.Model.Votes();
            string VoteIDString = string.Empty;
            if (model.VoteTiple == 0)
            {
                if (!string.IsNullOrEmpty(model.VoteID))
                {
                    VoteIDString = model.VoteID + "#" + meid;
                }
                else
                {
                    VoteIDString = meid.ToString();
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(model.VoteID))
                {
                    VoteIDString = model.VoteID + "#" + inum;
                }
                else
                {
                    VoteIDString = inum;
                }
            }
            addmodel.ID = id;
            addmodel.AddVote = AddVoteString;
            addmodel.VoteID = VoteIDString;
            new BCW.BLL.Votes().UpdateVote(addmodel);
            Utils.Success("投票成功", "恭喜，投票成功，正在返回..", Utils.getPage("votes.aspx"), "2");
        }
    }

    private void ListPage()
    {
        Master.Title = "历史投票";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("历史投票");

        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "Types=0";
        string[] pageValUrl = { "act" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

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
                builder.AppendFormat("{0}.<a href=\"" + Utils.getUrl("votes.aspx?id={1}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}</a>{3}", (pageIndex - 1) * pageSize + k, n.ID, n.Title, sFace);


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
        builder.Append("<a href=\"" + Utils.getUrl("votes.aspx") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
}