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
/// =============================
/// 修复排行榜单
/// 黄国军 20160612
/// 修改投注消费记录
/// 黄国军 20160314
/// =============================
public partial class Manage_game_six49 : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/six49.xml";

    #region 加载页面参数
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "add":
                AddPage();
                break;
            case "addsave":
                AddSavePage();
                break;
            case "edit":
                EditPage();
                break;
            case "editsave":
                EditSavePage();
                break;
            case "open":
                OpenPage();
                break;
            case "opensave":
                OpenSavePage();
                break;
            case "view":
                ViewPage();
                break;
            case "del":
                DelPage();
                break;
            case "top":
                TopPage();
                break;
            case "ChkTop":
                ChkTopPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }
    #endregion

    #region 虚拟投注管理 ReloadPage
    private void ReloadPage()
    {
        Master.Title = "虚拟投注管理";
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-3]$", "3"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
        builder.Append("虚拟投注");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        strWhere = "";

        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<LHC.Model.VoteNo49> listVoteNo49 = new LHC.BLL.VoteNo49().GetVoteNo49s(pageIndex, pageSize, strWhere, out recordCount);
        if (listVoteNo49.Count > 0)
        {
            int k = 1;
            foreach (LHC.Model.VoteNo49 n in listVoteNo49)
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
                builder.Append("<a href=\"" + Utils.getUrl("six49.aspx?act=edit&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[管理]</a>&gt;第" + n.qiNo + "期:");
                if (n.State == 0)
                    builder.Append("<a href=\"" + Utils.getUrl("six49.aspx?act=view&amp;id=" + n.ID + "") + "\">未开奖</a>[<a href=\"" + Utils.getUrl("six49.aspx?act=open&amp;id=" + n.ID + "") + "\">开奖</a>]");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("six49.aspx?act=view&amp;id=" + n.ID + "") + "\">" + n.pNum1 + "-" + n.pNum2 + "-" + n.pNum3 + "-" + n.pNum4 + "-" + n.pNum5 + "-" + n.pNum6 + "[特" + n.sNum + "]</a>");
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
        builder.Append("<a href=\"" + Utils.getUrl("six49.aspx?act=add") + "\">新开一期</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("../xml/six49set.aspx?backurl=" + Utils.PostPage(1) + "") + "\">游戏配置</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("six49.aspx?act=top&amp;ptype=5&amp;backurl=" + Utils.PostPage(1) + "") + "\">排行榜单</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    #endregion

    #region 查看投注记录 ViewPage
    private void ViewPage()
    {
        Master.Title = "查看投注记录";
        int id = Utils.ParseInt(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        LHC.Model.VoteNo49 model = new LHC.BLL.VoteNo49().GetVoteNo49(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }

        Master.Title = "第" + model.qiNo + "期投注";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("six49.aspx?act=list") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        strWhere += "qiNo=" + model.qiNo + "";

        string[] pageValUrl = { "act", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<LHC.Model.VotePay49> listVotePay49 = new LHC.BLL.VotePay49().GetVotePay49s(pageIndex, pageSize, strWhere, out recordCount);
        if (listVotePay49.Count > 0)
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("第" + model.qiNo + "期共投注:" + model.payCent + "" + ub.Get("SiteBz") + "");
            builder.Append("<br />下注总数为" + model.payCount + "注");
            long lCent = new LHC.BLL.VotePay49().GetwinCent(model.qiNo, 0);
            if (model.State == 1)
            {
                builder.Append("<br />返彩" + new LHC.BLL.VotePay49().GetwinCent(model.qiNo, 0) + "" + ub.Get("SiteBz") + "");
                builder.Append("<br />赢利" + (model.payCent - lCent) + "" + ub.Get("SiteBz") + "");
            }
            builder.Append(Out.Tab("</div>", "<br />"));

            int k = 1;
            foreach (LHC.Model.VotePay49 n in listVotePay49)
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
                string TyName = ForType(n.Types);
                string bzText = string.Empty;
                if (n.BzType == 0)
                    bzText = ub.Get("SiteBz");
                else
                    bzText = ub.Get("SiteBz2");

                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".");

                builder.Append("<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "</a>");
                if (n.State == 0)
                    builder.Append("押" + TyName + ":" + n.Vote + "(" + n.payCent + "" + bzText + ")[" + DT.FormatDate(n.AddTime, 0) + "]");
                else if (n.State == 1)
                {
                    builder.Append("押" + TyName + ":" + n.Vote + "(" + n.payCent + "" + bzText + ")[" + DT.FormatDate(n.AddTime, 0) + "]");
                    if (n.winCent > 0)
                    {
                        builder.Append("赢" + n.winCent + "" + bzText + "");
                    }
                }
                else
                    builder.Append("押" + TyName + ":" + n.Vote + "(" + n.payCent + "" + bzText + ")，赢" + n.winCent + "" + bzText + "[" + DT.FormatDate(n.AddTime, 0) + "]");


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
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    #endregion

    #region 开通新一期投注 AddPage
    private void AddPage()
    {
        LHC.Model.VoteNo49 m = new LHC.BLL.VoteNo49().GetVoteNo49New(0);
        if (m != null)
        {
            Utils.Error("上一期未结束，还不能开通下期", "");
        }
        Master.Title = "开通投注";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("开通新一期投注");
        builder.Append(Out.Tab("</div>", ""));
        string strText = "开通期数:,截止时间:,,";
        string strName = "qiNo,ExTime,act,backurl";
        string strType = "num,date,hidden,hidden";
        string strValu = "'" + DT.FormatDate(DateTime.Now.AddDays(1), 0) + "'addsave'" + Utils.getPage(0) + "";
        string strEmpt = "false,false,false,false";
        string strIdea = "/";
        string strOthe = "确定开通|reset,six49.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("six49.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    #endregion

    #region 确定开通 AddSavePage
    private void AddSavePage()
    {
        LHC.Model.VoteNo49 m = new LHC.BLL.VoteNo49().GetVoteNo49New(0);
        if (m != null)
        {
            Utils.Error("上一期未结束，还不能开通下期", "");
        }
        int qiNo = int.Parse(Utils.GetRequest("qiNo", "post", 2, @"^[1-9]\d*$", "期数填写错误"));
        DateTime ExTime = Utils.ParseTime(Utils.GetRequest("ExTime", "post", 2, DT.RegexTime, "截止时间格式填写出错,正确格式如" + DT.FormatDate(DateTime.Now, 0) + ""));

        LHC.Model.VoteNo49 model = new LHC.Model.VoteNo49();
        model.qiNo = qiNo;
        model.sNum = 0;
        model.pNum1 = 0;
        model.pNum2 = 0;
        model.pNum3 = 0;
        model.pNum4 = 0;
        model.pNum5 = 0;
        model.pNum6 = 0;
        model.payCent = 0;
        model.payCount = 0;
        model.ExTime = ExTime;
        model.State = 0;
        model.payCent2 = 0;
        model.payCount2 = 0;
        model.AddTime = DateTime.Now;
        new LHC.BLL.VoteNo49().Add(model);
        Utils.Success("开通投注", "开通第" + qiNo + "期投注成功..", Utils.getUrl("six49.aspx"), "1");
    }
    #endregion

    #region 编辑投注 EditPage
    private void EditPage()
    {
        //int ManageId = new BCW.User.Manage().IsManageLogin();
        //if (ManageId != 1 && ManageId != 9)
        //{
        //  Utils.Error("权限不足", "");
        //}
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        LHC.Model.VoteNo49 model = new LHC.BLL.VoteNo49().GetVoteNo49(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "编辑投注";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("编辑第" + model.qiNo + "期投注");
        builder.Append(Out.Tab("</div>", ""));
        string strText = "平.码1:,平.码2:,平.码3:,平.码4:,平.码5:,平.码6:,特别号码:,总下注" + ub.Get("SiteBz") + ":/,总下注" + ub.Get("SiteBz") + "注数:/,总下注" + ub.Get("SiteBz2") + ":/,总下注" + ub.Get("SiteBz2") + "注数:/,截止时间:/,状态:/,,,";
        string strName = "pNum1,pNum2,pNum3,pNum4,pNum5,pNum6,sNum,payCent,payCount,payCent2,payCount2,ExTime,State,id,act,backurl";
        string strType = "num,num,num,num,num,num,num,num,num,num,num,date,select,hidden,hidden,hidden";
        string strValu = "" + model.pNum1 + "'" + model.pNum2 + "'" + model.pNum3 + "'" + model.pNum4 + "'" + model.pNum5 + "'" + model.pNum6 + "'" + model.sNum + "'" + model.payCent + "'" + model.payCount + "'" + model.payCent2 + "'" + model.payCount2 + "'" + DT.FormatDate(model.ExTime, 0) + "'" + model.State + "'" + id + "'editsave'" + Utils.getPage(0) + "";
        string strEmpt = "false,false,false,false,false,false,false,false,false,false,false,false,0|未开奖|1|已开奖,false,false,false";
        string strIdea = "/";
        string strOthe = "确定编辑|reset,six49.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("six49.aspx") + "\">返回上一级</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("six49.aspx?act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">删除本期</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    #endregion

    #region 编辑投注成功 EditSavePage
    private void EditSavePage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        //if (ManageId != 1 && ManageId != 9)
        //{
        //Utils.Error("权限不足", "");
        //}
        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[1-9]\d*$", "ID错误"));
        int sNum = int.Parse(Utils.GetRequest("sNum", "post", 2, @"^[0-9]\d*$", "特别号码填写错误"));
        int pNum1 = int.Parse(Utils.GetRequest("pNum1", "post", 2, @"^[0-9]\d*$", "平.码1填写错误"));
        int pNum2 = int.Parse(Utils.GetRequest("pNum2", "post", 2, @"^[0-9]\d*$", "平.码2填写错误"));
        int pNum3 = int.Parse(Utils.GetRequest("pNum3", "post", 2, @"^[0-9]\d*$", "平.码3填写错误"));
        int pNum4 = int.Parse(Utils.GetRequest("pNum4", "post", 2, @"^[0-9]\d*$", "平.码4填写错误"));
        int pNum5 = int.Parse(Utils.GetRequest("pNum5", "post", 2, @"^[0-9]\d*$", "平.码5填写错误"));
        int pNum6 = int.Parse(Utils.GetRequest("pNum6", "post", 2, @"^[0-9]\d*$", "平.码6填写错误"));
        int payCent = int.Parse(Utils.GetRequest("payCent", "post", 2, @"^[0-9]\d*$", "总下注币填写错误"));
        int payCount = int.Parse(Utils.GetRequest("payCount", "post", 2, @"^[0-9]\d*$", "总下注数填写错误"));
        int payCent2 = int.Parse(Utils.GetRequest("payCent2", "post", 2, @"^[0-9]\d*$", "总下注币填写错误"));
        int payCount2 = int.Parse(Utils.GetRequest("payCount2", "post", 2, @"^[0-9]\d*$", "总下注数填写错误"));
        DateTime ExTime = Utils.ParseTime(Utils.GetRequest("ExTime", "post", 2, DT.RegexTime, "截止时间格式填写出错,正确格式如" + DT.FormatDate(DateTime.Now, 0) + ""));
        int State = int.Parse(Utils.GetRequest("State", "post", 2, @"^[0-1]$", "状态选择填写错误"));
        if (!new LHC.BLL.VoteNo49().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        LHC.Model.VoteNo49 model = new LHC.Model.VoteNo49();
        model.ID = id;
        model.sNum = sNum;
        model.pNum1 = pNum1;
        model.pNum2 = pNum2;
        model.pNum3 = pNum3;
        model.pNum4 = pNum4;
        model.pNum5 = pNum5;
        model.pNum6 = pNum6;
        model.payCent = payCent;
        model.payCount = payCount;
        model.payCent2 = payCent2;
        model.payCount2 = payCount2;
        model.ExTime = ExTime;
        model.State = State;

        new LHC.BLL.VoteNo49().Update(model);
        Utils.Success("编辑投注", "编辑投注成功..", Utils.getUrl("six49.aspx?act=edit&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
    }
    #endregion

    #region 删除投注 DelPage
    private void DelPage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId != 1 && ManageId != 9)
        {
            Utils.Error("权限不足", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        if (info == "")
        {
            Master.Title = "删除投注";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此记录吗.删除同时将会删除该期的下注记录.");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("six49.aspx?info=ok&amp;act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("six49.aspx?act=edit&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new LHC.BLL.VoteNo49().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            int qiNo = new LHC.BLL.VoteNo49().GetqiNo(id);
            new LHC.BLL.VoteNo49().Delete(id);
            new LHC.BLL.VotePay49().Delete("qiNo=" + qiNo + "");
            Utils.Success("删除投注", "删除投注记录成功..", Utils.getPage("six49.aspx"), "1");
        }
    }
    #endregion

    #region 投注开奖 OpenPage
    private void OpenPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        LHC.Model.VoteNo49 model = new LHC.BLL.VoteNo49().GetVoteNo49(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "投注开奖";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("第" + model.qiNo + "期投注开奖");
        builder.Append(Out.Tab("</div>", "<br />"));
        string strText = "平,-,-,-,-,-,特,,,";
        string strName = "pNum1,pNum2,pNum3,pNum4,pNum5,pNum6,sNum,id,act,backurl";
        string strType = "snum,snum,snum,snum,snum,snum,snum,hidden,hidden,hidden";
        string strValu = "'''''''" + id + "'opensave'" + Utils.getPage(0) + "";
        string strEmpt = "false,false,false,false,false,false,false,false,false,false,false";
        string strIdea = "/";
        string strOthe = "确定开奖|reset,six49.aspx,post,3,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("six49.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    #endregion

    #region 投注开奖确认 OpenSavePage
    private void OpenSavePage()
    {
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
        int Types = int.Parse(Utils.GetRequest("Types", "get", 1, @"^[1-9]\d*$", "0"));//类型
        int sNum = 0;
        int pNum1 = 0;
        int pNum2 = 0;
        int pNum3 = 0;
        int pNum4 = 0;
        int pNum5 = 0;
        int pNum6 = 0;
        if (Types == 0)
        {
            sNum = int.Parse(Utils.GetRequest("sNum", "post", 2, @"^[0-9]\d*$", "特别号码填写错误"));
            pNum1 = int.Parse(Utils.GetRequest("pNum1", "post", 2, @"^[0-9]\d*$", "平.码1填写错误"));
            pNum2 = int.Parse(Utils.GetRequest("pNum2", "post", 2, @"^[0-9]\d*$", "平.码2填写错误"));
            pNum3 = int.Parse(Utils.GetRequest("pNum3", "post", 2, @"^[0-9]\d*$", "平.码3填写错误"));
            pNum4 = int.Parse(Utils.GetRequest("pNum4", "post", 2, @"^[0-9]\d*$", "平.码4填写错误"));
            pNum5 = int.Parse(Utils.GetRequest("pNum5", "post", 2, @"^[0-9]\d*$", "平.码5填写错误"));
            pNum6 = int.Parse(Utils.GetRequest("pNum6", "post", 2, @"^[0-9]\d*$", "平.码6填写错误"));

            bool flag = true;
            string Nums = "";
            Nums = "" + sNum + "#" + pNum1 + "#" + pNum2 + "#" + pNum3 + "#" + pNum4 + "#" + pNum5 + "#" + pNum6 + "";
            string[] n = Regex.Split(Nums, "#");
            for (int i = 0; i < n.Length - 1; i++)
            {
                for (int j = i + 1; j < n.Length; j++)
                {
                    if (n[i] == n[j])
                    {
                        flag = false;
                        break;
                    }
                }
                if (!flag)
                {
                    Utils.Error("号码重复输入，请谨慎对照", "");
                }
            }


        }
        if (!new LHC.BLL.VoteNo49().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        int qiNo = new LHC.BLL.VoteNo49().GetqiNo(id);//得到期数
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "")
        {
            Master.Title = "投注开奖";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("请确定第" + qiNo + "期投注结果:<br />" + pNum1 + "-" + pNum2 + "-" + pNum3 + "-" + pNum4 + "-" + pNum5 + "-" + pNum6 + "特" + sNum + "");
            builder.Append(Out.Tab("</div>", "<br />"));
            string strName = "sNum,pNum1,pNum2,pNum3,pNum4,pNum5,pNum6,id,act,info,backurl";
            string strValu = "" + sNum + "'" + pNum1 + "'" + pNum2 + "'" + pNum3 + "'" + pNum4 + "'" + pNum5 + "'" + pNum6 + "'" + id + "'opensave'ok'" + Utils.getPage(0) + "";
            string strOthe = "确认开奖,six49.aspx,post,0,red";
            builder.Append(Out.wapform(strName, strValu, strOthe));

            builder.Append(Out.Tab("<div>", " "));
            builder.Append(" <a href=\"" + Utils.getUrl("six49.aspx?act=open&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">取消</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            #region 保存开奖数据
            LHC.Model.VoteNo49 model = null;
            if (Types == 0)
            {
                model = new LHC.Model.VoteNo49();
                model.ID = id;
                model.sNum = sNum;
                model.pNum1 = pNum1;
                model.pNum2 = pNum2;
                model.pNum3 = pNum3;
                model.pNum4 = pNum4;
                model.pNum5 = pNum5;
                model.pNum6 = pNum6;
                new LHC.BLL.VoteNo49().UpdateOpen(model);
                if (new LHC.BLL.VoteNo49().GetpayCount(id) == 0)
                {
                    new LHC.BLL.VoteNo49().UpdateState(qiNo);
                    Utils.Success("开奖成功", "恭喜，第" + qiNo + "期开奖成功", Utils.getPage("six49.aspx"), "1");
                }
            }
            else
            {
                model = new LHC.BLL.VoteNo49().GetVoteNo49(id);

                sNum = model.sNum;
                pNum1 = model.pNum1;
                pNum2 = model.pNum2;
                pNum3 = model.pNum3;
                pNum4 = model.pNum4;
                pNum5 = model.pNum5;
                pNum6 = model.pNum6;
            }
            #endregion

            #region 生成SQL
            ArrayList strSql = new ArrayList();
            DataSet ds = null;
            bool Ispp = false;
            //特别码码
            if (Types == 1)
            {
                ds = new LHC.BLL.VotePay49().GetList("ID,UsID,UsName,payCent,BzType,Vote", "Types=1 and qiNo=" + qiNo + " and State=0 and ','+Vote+',' Like '%," + sNum + ",%'");
            }
            //单双
            else if (Types == 2)
            {
                if (GetDS(sNum) == "打和")
                {
                    ds = new LHC.BLL.VotePay49().GetList("ID,UsID,UsName,payCent,BzType,Vote", "Types=2 and qiNo=" + qiNo + " and State=0");
                    Ispp = true;
                }
                else
                {
                    ds = new LHC.BLL.VotePay49().GetList("ID,UsID,UsName,payCent,BzType,Vote", "Types=2 and qiNo=" + qiNo + " and State=0 and Vote='" + GetDS(sNum) + "'");
                }
            }
            //大小
            else if (Types == 3)
            {
                if (GetDX(sNum) == "打和")
                {
                    ds = new LHC.BLL.VotePay49().GetList("ID,UsID,UsName,payCent,BzType,Vote", "Types=3 and qiNo=" + qiNo + " and State=0");
                    Ispp = true;
                }
                else
                {
                    ds = new LHC.BLL.VotePay49().GetList("ID,UsID,UsName,payCent,BzType,Vote", "Types=3 and qiNo=" + qiNo + " and State=0 and Vote='" + GetDX(sNum) + "'");
                }
            }
            //波色
            else if (Types == 4)
            {
                ds = new LHC.BLL.VotePay49().GetList("ID,UsID,UsName,payCent,BzType,Vote", "Types=4 and qiNo=" + qiNo + " and State=0 and Vote='" + GetBS(sNum) + "'");
            }
            //生肖
            else if (Types == 5)
            {
                ds = new LHC.BLL.VotePay49().GetList("ID,UsID,UsName,payCent,BzType,Vote", "Types=5 and qiNo=" + qiNo + " and State=0 and Vote='" + GetSX(sNum) + "'");
            }
            //家野
            else if (Types == 6)
            {
                ds = new LHC.BLL.VotePay49().GetList("ID,UsID,UsName,payCent,BzType,Vote", "Types=6 and qiNo=" + qiNo + " and State=0 and Vote='" + GetQS(sNum) + "'");
            }
            //特尾
            else if (Types == 7)
            {
                ds = new LHC.BLL.VotePay49().GetList("ID,UsID,UsName,payCent,BzType,Vote", "Types=7 and qiNo=" + qiNo + " and State=0 and Vote='" + Utils.Right(sNum.ToString(), 1) + "'");
            }
            //特头
            else if (Types == 8)
            {
                string tVote = string.Empty;
                if (sNum < 10)
                    tVote = "0";
                else
                    tVote = Utils.Left(sNum.ToString(), 1);

                ds = new LHC.BLL.VotePay49().GetList("ID,UsID,UsName,payCent,BzType,Vote", "Types=8 and qiNo=" + qiNo + " and State=0 and Vote='" + tVote + "'");
            }
            //合数大小
            else if (Types == 9)
            {
                if (GetHDX(sNum) == "打和")
                {
                    ds = new LHC.BLL.VotePay49().GetList("ID,UsID,UsName,payCent,BzType,Vote", "Types=9 and qiNo=" + qiNo + " and State=0");
                    Ispp = true;
                }
                else
                {
                    ds = new LHC.BLL.VotePay49().GetList("ID,UsID,UsName,payCent,BzType,Vote", "Types=9 and qiNo=" + qiNo + " and State=0 and Vote='" + GetHDX(sNum) + "'");
                }
            }
            //合数单双
            else if (Types == 10)
            {
                if (GetHDS(sNum) == "打和")
                {
                    ds = new LHC.BLL.VotePay49().GetList("ID,UsID,UsName,payCent,BzType,Vote", "Types=10 and qiNo=" + qiNo + " and State=0");
                    Ispp = true;
                }
                else
                {
                    ds = new LHC.BLL.VotePay49().GetList("ID,UsID,UsName,payCent,BzType,Vote", "Types=10 and qiNo=" + qiNo + " and State=0 and Vote='" + GetHDS(sNum) + "'");
                }
            }
            //平特肖
            else if (Types == 11)
            {
                ds = new LHC.BLL.VotePay49().GetList("ID,UsID,UsName,payCent,BzType,Vote", "Types=11 and qiNo=" + qiNo + " and State=0 and (Vote='" + GetSX(sNum) + "' OR Vote='" + GetSX(pNum1) + "' OR Vote='" + GetSX(pNum2) + "' OR Vote='" + GetSX(pNum3) + "' OR Vote='" + GetSX(pNum4) + "' OR Vote='" + GetSX(pNum5) + "' OR Vote='" + GetSX(pNum6) + "')");
            }
            //独平
            else if (Types == 12)
            {
                //ds = new LHC.BLL.VotePay49().GetList("ID,UsID,UsName,payCent,BzType,Vote", "Types=12 and qiNo=" + qiNo + " and State=0 and (Vote='" + pNum1 + "' OR Vote='" + pNum2 + "' OR Vote='" + pNum3 + "' OR Vote='" + pNum4 + "' OR Vote='" + pNum5 + "' OR Vote='" + pNum6 + "')");
                ds = new LHC.BLL.VotePay49().GetList("ID,UsID,UsName,payCent,BzType,Vote", "Types=12 and qiNo=" + qiNo + " and State=0");
            }
            //平特一尾
            else if (Types == 13)
            {
                ds = new LHC.BLL.VotePay49().GetList("ID,UsID,UsName,payCent,BzType,Vote", "Types=13 and qiNo=" + qiNo + " and State=0 and (Vote='" + Utils.Right(sNum.ToString(), 1) + "' OR Vote='" + Utils.Right(pNum1.ToString(), 1) + "' OR Vote='" + Utils.Right(pNum2.ToString(), 1) + "' OR Vote='" + Utils.Right(pNum3.ToString(), 1) + "' OR Vote='" + Utils.Right(pNum4.ToString(), 1) + "' OR Vote='" + Utils.Right(pNum5.ToString(), 1) + "' OR Vote='" + Utils.Right(pNum6.ToString(), 1) + "')");
            }
            //五行
            else if (Types == 14)
            {
                ds = new LHC.BLL.VotePay49().GetList("ID,UsID,UsName,payCent,BzType,Vote", "Types=14 and qiNo=" + qiNo + " and State=0 and Vote='" + GetWX(sNum) + "'");
            }
            //六肖
            else if (Types == 15)
            {
                ds = new LHC.BLL.VotePay49().GetList("ID,UsID,UsName,payCent,BzType,Vote", "Types=15 and qiNo=" + qiNo + " and State=0 and ','+Vote+',' Like '%," + GetSX(sNum) + ",%'");
            }
            //半波
            else if (Types == 16)
            {
                if (GetBBS(sNum) == "打和")
                {
                    ds = new LHC.BLL.VotePay49().GetList("ID,UsID,UsName,payCent,BzType,Vote", "Types=16 and qiNo=" + qiNo + " and State=0");
                    Ispp = true;
                }
                else
                {
                    ds = new LHC.BLL.VotePay49().GetList("ID,UsID,UsName,payCent,BzType,Vote", "Types=16 and qiNo=" + qiNo + " and State=0 and Vote='" + GetBBS(sNum) + "'");
                }
            }
            //五门
            else if (Types == 23)
            {
                ds = new LHC.BLL.VotePay49().GetList("ID,UsID,UsName,payCent,BzType,Vote", "Types=" + Types + " and qiNo=" + qiNo + " and State=0 and Vote='" + GetWM(sNum) + "'");
            }
            //尾数大小
            else if (Types == 24)
            {
                if (GetWSDX(sNum) == "打和")
                {
                    ds = new LHC.BLL.VotePay49().GetList("ID,UsID,UsName,payCent,BzType,Vote", "Types=24 and qiNo=" + qiNo + " and State=0");
                    Ispp = true;
                }
                else
                {
                    ds = new LHC.BLL.VotePay49().GetList("ID,UsID,UsName,payCent,BzType,Vote", "Types=24 and qiNo=" + qiNo + " and State=0 and Vote='" + GetWSDX(sNum) + "'");
                }
            }
            //尾数单双
            else if (Types == 25)
            {
                if (GetWSDS(sNum) == "打和")
                {
                    ds = new LHC.BLL.VotePay49().GetList("ID,UsID,UsName,payCent,BzType,Vote", "Types=25 and qiNo=" + qiNo + " and State=0");
                    Ispp = true;
                }
                else
                {
                    ds = new LHC.BLL.VotePay49().GetList("ID,UsID,UsName,payCent,BzType,Vote", "Types=25 and qiNo=" + qiNo + " and State=0 and Vote='" + GetWSDS(sNum) + "'");
                }
            }
            //总分大小
            else if (Types == 26)
            {
                if (GetZFDX(sNum, pNum1, pNum2, pNum3, pNum4, pNum5, pNum6) == "打和")
                {
                    ds = new LHC.BLL.VotePay49().GetList("ID,UsID,UsName,payCent,BzType,Vote", "Types=26 and qiNo=" + qiNo + " and State=0");
                    Ispp = true;
                }
                else
                {
                    ds = new LHC.BLL.VotePay49().GetList("ID,UsID,UsName,payCent,BzType,Vote", "Types=26 and qiNo=" + qiNo + " and State=0 and Vote='" + GetZFDX(sNum, pNum1, pNum2, pNum3, pNum4, pNum5, pNum6) + "'");
                }
            }
            //总分单双
            else if (Types == 27)
            {
                if (GetZFDS(sNum, pNum1, pNum2, pNum3, pNum4, pNum5, pNum6) == "打和")
                {
                    ds = new LHC.BLL.VotePay49().GetList("ID,UsID,UsName,payCent,BzType,Vote", "Types=27 and qiNo=" + qiNo + " and State=0");
                    Ispp = true;
                }
                else
                {
                    ds = new LHC.BLL.VotePay49().GetList("ID,UsID,UsName,payCent,BzType,Vote", "Types=27 and qiNo=" + qiNo + " and State=0 and Vote='" + GetZFDS(sNum, pNum1, pNum2, pNum3, pNum4, pNum5, pNum6) + "'");
                }
            }
            //半半波
            else if (Types == 28)
            {
                if (GetBBBS(sNum) == "打和")
                {
                    ds = new LHC.BLL.VotePay49().GetList("ID,UsID,UsName,payCent,BzType,Vote", "Types=28 and qiNo=" + qiNo + " and State=0");
                    Ispp = true;
                }
                else
                {
                    ds = new LHC.BLL.VotePay49().GetList("ID,UsID,UsName,payCent,BzType,Vote", "Types=28 and qiNo=" + qiNo + " and State=0 and Vote='" + GetBBBS(sNum) + "'");
                }
            }
            else
            {
                if (Types != 82)//82是空值，没这个选项
                {
                    if ((Types >= 1 && Types <= 40) || (Types >= 80 && Types <= 88))
                        ds = new LHC.BLL.VotePay49().GetList("ID,UsID,UsName,payCent,BzType,Vote", "Types=" + Types + " and qiNo=" + qiNo + " and State=0");
                }
            }
            #endregion

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                #region 返奖
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    int iwinCount = 0;//每投注里中多少注（用于复式）
                    int pid = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                    string UsName = ds.Tables[0].Rows[i]["UsName"].ToString();
                    int UsID = int.Parse(ds.Tables[0].Rows[i]["UsID"].ToString());
                    int payCent = int.Parse(ds.Tables[0].Rows[i]["payCent"].ToString());
                    int bzType = int.Parse(ds.Tables[0].Rows[i]["BzType"].ToString());
                    string Vote = ds.Tables[0].Rows[i]["Vote"].ToString();
                    bool isWin = true;
                    double Odds = 0;//赔率
                    /*-------------------------------------------------------------------------------------*/
                    if (Types == 1)
                    {
                        string TModds = ub.GetSub("SixTM" + sNum + "", xmlPath);
                        if (TModds == "")
                            Odds = Convert.ToDouble(ub.GetSub("SixTM", xmlPath));
                        else
                            Odds = Convert.ToDouble(TModds);
                    }
                    else if (Types == 2)
                    {
                        if (Vote == "单")
                            Odds = Convert.ToDouble(ub.GetSub("SixDS", xmlPath));
                        else
                            Odds = Convert.ToDouble(ub.GetSub("SixDS2", xmlPath));
                    }
                    else if (Types == 3)
                    {
                        if (Vote == "大")
                            Odds = Convert.ToDouble(ub.GetSub("SixDX", xmlPath));
                        else
                            Odds = Convert.ToDouble(ub.GetSub("SixDX2", xmlPath));
                    }
                    else if (Types == 4)
                    {
                        Odds = Convert.ToDouble(ub.GetSub("SixBS", xmlPath));
                    }
                    else if (Types == 5)
                    {
                        string SXodds = ub.GetSub("SixXX" + ForSXNum(GetSX(sNum)) + "", xmlPath);
                        if (SXodds == "")
                            Odds = Convert.ToDouble(ub.GetSub("SixSX", xmlPath));
                        else
                            Odds = Convert.ToDouble(SXodds);
                    }
                    else if (Types == 6)
                    {
                        if (Vote == "家禽")
                            Odds = Convert.ToDouble(ub.GetSub("SixQS", xmlPath));
                        else
                            Odds = Convert.ToDouble(ub.GetSub("SixQS2", xmlPath));
                    }
                    else if (Types == 7)
                    {
                        Odds = Convert.ToDouble(ub.GetSub("SixW" + Vote + "", xmlPath));
                    }
                    else if (Types == 8)
                    {
                        Odds = Convert.ToDouble(ub.GetSub("SixT" + Vote + "", xmlPath));
                    }
                    else if (Types == 9)
                    {
                        if (Vote == "合大")
                            Odds = Convert.ToDouble(ub.GetSub("SixHDX", xmlPath));
                        else
                            Odds = Convert.ToDouble(ub.GetSub("SixHDX2", xmlPath));
                    }
                    else if (Types == 10)
                    {
                        if (Vote == "合单")
                            Odds = Convert.ToDouble(ub.GetSub("SixHDS", xmlPath));
                        else
                            Odds = Convert.ToDouble(ub.GetSub("SixHDS2", xmlPath));
                    }
                    else if (Types == 11)
                    {

                        string PTXodds = ub.GetSub("SixPTX" + ForSXNum(Vote) + "", xmlPath);
                        if (PTXodds == "")
                            Odds = Convert.ToDouble(ub.GetSub("SixYX", xmlPath));
                        else
                            Odds = Convert.ToDouble(PTXodds);
                    }
                    else if (Types == 12)
                    {
                        //Odds = Convert.ToDouble(ub.GetSub("SixDP", xmlPath));

                        string[] strTemp = Vote.Split(',');
                        string AVote = "#" + pNum1 + "#" + pNum2 + "#" + pNum3 + "#" + pNum4 + "#" + pNum5 + "#" + pNum6 + "#";
                        for (int k = 0; k < strTemp.Length; k++)
                        {
                            if (AVote.Contains("#" + strTemp[k] + "#"))
                            {
                                Odds = Convert.ToDouble(ub.GetSub("SixDP", xmlPath));
                                iwinCount++;
                            }
                        }
                    }
                    else if (Types == 13)
                    {
                        Odds = Convert.ToDouble(ub.GetSub("SixPW" + Vote + "", xmlPath));
                    }
                    else if (Types == 14)
                    {
                        Odds = Convert.ToDouble(ub.GetSub("SixWX", xmlPath));
                    }
                    else if (Types == 15)
                    {
                        Odds = Convert.ToDouble(ub.GetSub("SixSIX", xmlPath));
                    }
                    else if (Types == 16)
                    {
                        if (Vote == "红单")
                            Odds = Convert.ToDouble(ub.GetSub("SixRed0", xmlPath));
                        else if (Vote == "红双")
                            Odds = Convert.ToDouble(ub.GetSub("SixRed1", xmlPath));
                        else if (Vote == "蓝单")
                            Odds = Convert.ToDouble(ub.GetSub("SixBlue0", xmlPath));
                        else if (Vote == "蓝双")
                            Odds = Convert.ToDouble(ub.GetSub("SixBlue1", xmlPath));
                        else if (Vote == "绿单")
                            Odds = Convert.ToDouble(ub.GetSub("SixGreen0", xmlPath));
                        else if (Vote == "绿双")
                            Odds = Convert.ToDouble(ub.GetSub("SixGreen1", xmlPath));

                    }
                    /*-------------------------------------------------------------------------------------*/

                    else if (Types == 17)//特连
                    {
                        string[] strTemp = Vote.Split(',');
                        string p1 = strTemp[0];
                        string p2 = strTemp[1];
                        if (GetPM(pNum1, pNum2, pNum3, pNum4, pNum5, pNum6, p1) == 1 && p2 == sNum.ToString())
                        {
                            Odds = Convert.ToDouble(ub.GetSub("SixTL", xmlPath));
                        }
                        else if (GetPM(pNum1, pNum2, pNum3, pNum4, pNum5, pNum6, p2) == 1 && p1 == sNum.ToString())
                        {
                            Odds = Convert.ToDouble(ub.GetSub("SixTL", xmlPath));
                        }
                        else
                        {
                            isWin = false;
                        }
                    }
                    else if (Types == 18)
                    {
                        if (GetPM(pNum1, pNum2, pNum3, pNum4, pNum5, pNum6, ds.Tables[0].Rows[i]["Vote"].ToString()) == 3)//三中三
                        {
                            Odds = Convert.ToDouble(ub.GetSub("SixSZS", xmlPath));
                        }
                        else
                        {
                            isWin = false;
                        }
                    }
                    else if (Types == 19)
                    {
                        if (GetPM(pNum1, pNum2, pNum3, pNum4, pNum5, pNum6, ds.Tables[0].Rows[i]["Vote"].ToString()) == 3)//三中三
                        {
                            Odds = Convert.ToDouble(ub.GetSub("SixSZE2", xmlPath));
                        }
                        else if (GetPM(pNum1, pNum2, pNum3, pNum4, pNum5, pNum6, ds.Tables[0].Rows[i]["Vote"].ToString()) == 2)//三中二
                        {
                            Odds = Convert.ToDouble(ub.GetSub("SixSZE", xmlPath));
                        }
                        else
                        {
                            isWin = false;
                        }
                    }
                    else if (Types == 20)
                    {
                        if (GetPM(pNum1, pNum2, pNum3, pNum4, pNum5, pNum6, ds.Tables[0].Rows[i]["Vote"].ToString()) == 2)//二中二
                        {
                            Odds = Convert.ToDouble(ub.GetSub("SixEZE", xmlPath));
                        }
                        else
                        {
                            isWin = false;
                        }
                    }
                    else if (Types == 80)//二中二复式
                    {

                        string[] vTemp = Vote.Split('-');
                        List<string> listNum = new Combination().GetCombination2(vTemp.Length, 2, vTemp);
                        if (listNum.Count > 0)
                        {
                            foreach (string n in listNum)
                            {
                                if (GetPM(pNum1, pNum2, pNum3, pNum4, pNum5, pNum6, n) == 2)
                                {
                                    Odds = Convert.ToDouble(ub.GetSub("SixEZE", xmlPath));
                                    iwinCount++;
                                }
                            }

                        }
                    }
                    else if (Types == 81)//三中三复式
                    {

                        string[] vTemp = Vote.Split('-');
                        List<string> listNum = new Combination().GetCombination2(vTemp.Length, 3, vTemp);
                        if (listNum.Count > 0)
                        {
                            foreach (string n in listNum)
                            {
                                if (GetPM(pNum1, pNum2, pNum3, pNum4, pNum5, pNum6, n) == 3)
                                {
                                    Odds = Convert.ToDouble(ub.GetSub("SixSZS", xmlPath));
                                    iwinCount++;
                                }
                            }

                        }
                    }
                    else if (Types == 83)//平特二肖复式
                    {

                        string[] vTemp = Vote.Split('-');
                        List<string> listNum = new Combination().GetCombination2(vTemp.Length, 2, vTemp);
                        if (listNum.Count > 0)
                        {
                            foreach (string n in listNum)
                            {
                                string[] temp = n.Split(',');
                                string AVote = GetSX(sNum) + GetSX(pNum1) + GetSX(pNum2) + GetSX(pNum3) + GetSX(pNum4) + GetSX(pNum5) + GetSX(pNum6);
                                if (AVote.Contains(temp[0]) && AVote.Contains(temp[1]))
                                {
                                    Odds = Convert.ToDouble(ub.GetSub("SixPTLX", xmlPath));
                                    iwinCount++;
                                }
                            }

                        }
                    }
                    else if (Types == 84)//平特三肖复式
                    {

                        string[] vTemp = Vote.Split('-');
                        List<string> listNum = new Combination().GetCombination2(vTemp.Length, 3, vTemp);
                        if (listNum.Count > 0)
                        {
                            foreach (string n in listNum)
                            {
                                string[] temp = n.Split(',');
                                string AVote = GetSX(sNum) + GetSX(pNum1) + GetSX(pNum2) + GetSX(pNum3) + GetSX(pNum4) + GetSX(pNum5) + GetSX(pNum6);
                                if (AVote.Contains(temp[0]) && AVote.Contains(temp[1]) && AVote.Contains(temp[2]))
                                {
                                    Odds = Convert.ToDouble(ub.GetSub("SixvPTLX2", xmlPath));
                                    iwinCount++;
                                }
                            }

                        }
                    }
                    else if (Types == 85)//平特四肖复式
                    {

                        string[] vTemp = Vote.Split('-');
                        List<string> listNum = new Combination().GetCombination2(vTemp.Length, 4, vTemp);
                        if (listNum.Count > 0)
                        {
                            foreach (string n in listNum)
                            {
                                string[] temp = n.Split(',');
                                string AVote = GetSX(sNum) + GetSX(pNum1) + GetSX(pNum2) + GetSX(pNum3) + GetSX(pNum4) + GetSX(pNum5) + GetSX(pNum6);
                                if (AVote.Contains(temp[0]) && AVote.Contains(temp[1]) && AVote.Contains(temp[2]) && AVote.Contains(temp[3]))
                                {
                                    Odds = Convert.ToDouble(ub.GetSub("SixvPTLX3", xmlPath));
                                    iwinCount++;
                                }
                            }

                        }
                    }
                    else if (Types == 86)//平特二尾复式
                    {

                        string[] vTemp = Vote.Split('-');
                        List<string> listNum = new Combination().GetCombination2(vTemp.Length, 2, vTemp);
                        if (listNum.Count > 0)
                        {
                            foreach (string n in listNum)
                            {
                                string[] temp = n.Split(',');
                                string AVote = "," + Utils.Right(sNum.ToString(), 1) + "," + Utils.Right(pNum1.ToString(), 1) + "," + Utils.Right(pNum2.ToString(), 1) + "," + Utils.Right(pNum3.ToString(), 1) + "," + Utils.Right(pNum4.ToString(), 1) + "," + Utils.Right(pNum5.ToString(), 1) + "," + Utils.Right(pNum6.ToString(), 1) + ",";

                                if (AVote.Contains("," + temp[0] + ",") && AVote.Contains("," + temp[1] + ","))
                                {
                                    Odds = Convert.ToDouble(ub.GetSub("SixvPTW", xmlPath));
                                    iwinCount++;
                                }
                            }

                        }
                    }
                    else if (Types == 87)//平特三尾复式
                    {

                        string[] vTemp = Vote.Split('-');
                        List<string> listNum = new Combination().GetCombination2(vTemp.Length, 3, vTemp);
                        if (listNum.Count > 0)
                        {
                            foreach (string n in listNum)
                            {
                                string[] temp = n.Split(',');
                                string AVote = "," + Utils.Right(sNum.ToString(), 1) + "," + Utils.Right(pNum1.ToString(), 1) + "," + Utils.Right(pNum2.ToString(), 1) + "," + Utils.Right(pNum3.ToString(), 1) + "," + Utils.Right(pNum4.ToString(), 1) + "," + Utils.Right(pNum5.ToString(), 1) + "," + Utils.Right(pNum6.ToString(), 1) + ",";

                                if (AVote.Contains("," + temp[0] + ",") && AVote.Contains("," + temp[1] + ",") && AVote.Contains("," + temp[2] + ","))
                                {
                                    Odds = Convert.ToDouble(ub.GetSub("SixvPTW2", xmlPath));
                                    iwinCount++;
                                }
                            }

                        }
                    }
                    else if (Types == 88)//平特四尾复式
                    {

                        string[] vTemp = Vote.Split('-');
                        List<string> listNum = new Combination().GetCombination2(vTemp.Length, 4, vTemp);
                        if (listNum.Count > 0)
                        {
                            foreach (string n in listNum)
                            {
                                string[] temp = n.Split(',');
                                string AVote = "," + Utils.Right(sNum.ToString(), 1) + "," + Utils.Right(pNum1.ToString(), 1) + "," + Utils.Right(pNum2.ToString(), 1) + "," + Utils.Right(pNum3.ToString(), 1) + "," + Utils.Right(pNum4.ToString(), 1) + "," + Utils.Right(pNum5.ToString(), 1) + "," + Utils.Right(pNum6.ToString(), 1) + ",";

                                if (AVote.Contains("," + temp[0] + ",") && AVote.Contains("," + temp[1] + ",") && AVote.Contains("," + temp[2] + ",") && AVote.Contains("," + temp[3] + ","))
                                {
                                    Odds = Convert.ToDouble(ub.GetSub("SixvPTW3", xmlPath));
                                    iwinCount++;
                                }
                            }

                        }
                    }
                    //-----------------平特二肖、三肖、四肖单式投注
                    else if (Types == 21)
                    {
                        string[] temp = ds.Tables[0].Rows[i]["Vote"].ToString().Split(',');
                        string AVote = GetSX(sNum) + GetSX(pNum1) + GetSX(pNum2) + GetSX(pNum3) + GetSX(pNum4) + GetSX(pNum5) + GetSX(pNum6);
                        if (AVote.Contains(temp[0]) && AVote.Contains(temp[1]))
                        {
                            Odds = Convert.ToDouble(ub.GetSub("SixPTLX", xmlPath));
                        }
                        else
                        {
                            isWin = false;
                        }
                    }
                    else if (Types == 29)
                    {
                        string[] temp = ds.Tables[0].Rows[i]["Vote"].ToString().Split(',');
                        string AVote = GetSX(sNum) + GetSX(pNum1) + GetSX(pNum2) + GetSX(pNum3) + GetSX(pNum4) + GetSX(pNum5) + GetSX(pNum6);
                        if (AVote.Contains(temp[0]) && AVote.Contains(temp[1]) && AVote.Contains(temp[2]))
                        {
                            Odds = Convert.ToDouble(ub.GetSub("SixvPTLX2", xmlPath));
                        }
                        else
                        {
                            isWin = false;
                        }
                    }
                    else if (Types == 30)
                    {
                        string[] temp = ds.Tables[0].Rows[i]["Vote"].ToString().Split(',');
                        string AVote = GetSX(sNum) + GetSX(pNum1) + GetSX(pNum2) + GetSX(pNum3) + GetSX(pNum4) + GetSX(pNum5) + GetSX(pNum6);
                        if (AVote.Contains(temp[0]) && AVote.Contains(temp[1]) && AVote.Contains(temp[2]) && AVote.Contains(temp[3]))
                        {
                            Odds = Convert.ToDouble(ub.GetSub("SixvPTLX3", xmlPath));
                        }
                        else
                        {
                            isWin = false;
                        }
                    }
                    //-----------------平特二肖、三肖、四肖单式投注

                     //-----------------平特二尾、三尾、四尾单式投注
                    else if (Types == 31)
                    {
                        string[] temp = ds.Tables[0].Rows[i]["Vote"].ToString().Split(',');
                        string AVote = "," + Utils.Right(sNum.ToString(), 1) + "," + Utils.Right(pNum1.ToString(), 1) + "," + Utils.Right(pNum2.ToString(), 1) + "," + Utils.Right(pNum3.ToString(), 1) + "," + Utils.Right(pNum4.ToString(), 1) + "," + Utils.Right(pNum5.ToString(), 1) + "," + Utils.Right(pNum6.ToString(), 1) + ",";

                        if (AVote.Contains("," + temp[0] + ",") && AVote.Contains("," + temp[1] + ","))
                        {
                            Odds = Convert.ToDouble(ub.GetSub("SixvPTW", xmlPath));
                        }
                        else
                        {
                            isWin = false;
                        }
                    }
                    else if (Types == 32)
                    {
                        string[] temp = ds.Tables[0].Rows[i]["Vote"].ToString().Split(',');
                        string AVote = "," + Utils.Right(sNum.ToString(), 1) + "," + Utils.Right(pNum1.ToString(), 1) + "," + Utils.Right(pNum2.ToString(), 1) + "," + Utils.Right(pNum3.ToString(), 1) + "," + Utils.Right(pNum4.ToString(), 1) + "," + Utils.Right(pNum5.ToString(), 1) + "," + Utils.Right(pNum6.ToString(), 1) + ",";

                        if (AVote.Contains("," + temp[0] + ",") && AVote.Contains("," + temp[1] + ",") && AVote.Contains("," + temp[2] + ","))
                        {
                            Odds = Convert.ToDouble(ub.GetSub("SixvPTW2", xmlPath));
                        }
                        else
                        {
                            isWin = false;
                        }
                    }
                    else if (Types == 33)
                    {
                        string[] temp = ds.Tables[0].Rows[i]["Vote"].ToString().Split(',');
                        string AVote = "," + Utils.Right(sNum.ToString(), 1) + "," + Utils.Right(pNum1.ToString(), 1) + "," + Utils.Right(pNum2.ToString(), 1) + "," + Utils.Right(pNum3.ToString(), 1) + "," + Utils.Right(pNum4.ToString(), 1) + "," + Utils.Right(pNum5.ToString(), 1) + "," + Utils.Right(pNum6.ToString(), 1) + ",";

                        if (AVote.Contains("," + temp[0] + ",") && AVote.Contains("," + temp[1] + ",") && AVote.Contains("," + temp[2] + ",") && AVote.Contains("," + temp[3] + ","))
                        {
                            Odds = Convert.ToDouble(ub.GetSub("SixvPTW3", xmlPath));
                        }
                        else
                        {
                            isWin = false;
                        }
                    }
                    //-----------------平特二尾、三尾、四尾单式投注
                    else if (Types == 22 || (Types >= 34 && Types <= 40))//自选不中
                    {
                        string BVote = "," + sNum + "," + pNum1 + "," + pNum2 + "," + pNum3 + "," + pNum4 + "," + pNum5 + "," + pNum6 + ",";
                        string[] temp = ds.Tables[0].Rows[i]["Vote"].ToString().Split(',');
                        for (int k = 0; k < temp.Length; k++)
                        {
                            if (BVote.Contains("," + Utils.ParseInt(temp[k]) + ","))
                            {
                                isWin = false;
                                break;
                            }
                        }
                        if (isWin == true)
                        {
                            Odds = Convert.ToDouble(ub.GetSub("SixSBZ" + Types + "", xmlPath));
                        }
                    }
                    else if (Types == 23)
                    {
                        Odds = Convert.ToDouble(ub.GetSub("SixvWM", xmlPath));
                    }
                    else if (Types == 24)
                    {
                        Odds = Convert.ToDouble(ub.GetSub("SixvWSDX", xmlPath));
                    }
                    else if (Types == 25)
                    {
                        Odds = Convert.ToDouble(ub.GetSub("SixvWSDS", xmlPath));
                    }
                    else if (Types == 26)
                    {
                        Odds = Convert.ToDouble(ub.GetSub("SixvZFDX", xmlPath));
                    }
                    else if (Types == 27)
                    {
                        Odds = Convert.ToDouble(ub.GetSub("SixvZFDS", xmlPath));
                    }
                    else if (Types == 28)
                    {
                        Odds = Convert.ToDouble(ub.GetSub("SixBBB" + ForBBB(Vote) + "", xmlPath));
                    }

                    string bzText = string.Empty;
                    if (bzType == 0)
                        bzText = ub.Get("SiteBz");
                    else
                        bzText = ub.Get("SiteBz2");

                    if (Ispp == true)
                    {
                        strSql.Add("update tb_VotePay49 set winCent=" + payCent + " Where id=" + pid + "");
                        //发送内线
                        new BCW.BLL.Guest().Add(1, UsID, UsName, "第" + qiNo + "期《" + ForType(Types) + "》投注，开奖为" + pNum1 + "-" + pNum2 + "-" + pNum3 + "-" + pNum4 + "-" + pNum5 + "-" + pNum6 + "特" + sNum + "，打和返还您" + payCent + "" + bzText + "，[URL=/bbs/game/six49.aspx?act=case]马上兑奖[/URL]");
                    }
                    else
                    {
                        if (isWin == true && iwinCount == 0)
                        {
                            strSql.Add("update tb_VotePay49 set winCent=" + Convert.ToInt64(payCent * Odds) + " Where id=" + pid + "");
                            //发送内线
                            new BCW.BLL.Guest().Add(1, UsID, UsName, "恭喜您在第" + qiNo + "期《" + ForType(Types) + "》投注，中奖" + Convert.ToInt64(payCent * Odds) + "" + bzText + "，开奖为" + pNum1 + "-" + pNum2 + "-" + pNum3 + "-" + pNum4 + "-" + pNum5 + "-" + pNum6 + "特" + sNum + "[URL=/bbs/game/six49.aspx?act=case]马上兑奖[/URL]");
                        }
                        if (iwinCount > 0)
                        {
                            strSql.Add("update tb_VotePay49 set winCent=" + Convert.ToInt64(payCent * Odds * iwinCount) + " Where id=" + pid + "");
                            //发送内线
                            new BCW.BLL.Guest().Add(1, UsID, UsName, "恭喜您在第" + qiNo + "期《" + ForType(Types) + ":" + Vote + "(" + iwinCount + "注中)》，中奖" + Convert.ToInt64(payCent * Odds * iwinCount) + "" + bzText + "，开奖为" + pNum1 + "-" + pNum2 + "-" + pNum3 + "-" + pNum4 + "-" + pNum5 + "-" + pNum6 + "特" + sNum + "[URL=/bbs/game/six49.aspx?act=case]马上兑奖[/URL]");
                        }
                    }
                }
                #endregion
            }
            //事务开始
            if (strSql != null)
            {
                BCW.Data.SqlHelper.ExecuteSqlTran(strSql);
            }
            if (Types < 88)
            {
                if (Types == 40)
                    Types = 79;

                Utils.Success("正在开奖", "正在更新开奖，请等待开奖完成，不要刷新..", Utils.getPage("six49.aspx?act=opensave&amp;info=ok&amp;id=" + id + "&amp;Types=" + (Types + 1) + ""), "1");
            }
            else
            {
                //完成返彩后正式更新该期为结束
                new LHC.BLL.VoteNo49().UpdateState(qiNo);
                new LHC.BLL.VotePay49().UpdateOver(qiNo);
                Utils.Success("开奖成功", "恭喜，第" + qiNo + "期开奖成功", Utils.getPage("six49.aspx"), "1");
            }
        }
    }
    #endregion

    #region 获得生肖 ForSX
    private string ForSX(string Vote)
    {
        string ForVote = string.Empty;

        if (Vote == "1")
            ForVote = "鼠";
        else if (Vote == "2")
            ForVote = "牛";
        else if (Vote == "3")
            ForVote = "虎";
        else if (Vote == "4")
            ForVote = "兔";
        else if (Vote == "5")
            ForVote = "龙";
        else if (Vote == "6")
            ForVote = "蛇";
        else if (Vote == "7")
            ForVote = "马";
        else if (Vote == "8")
            ForVote = "羊";
        else if (Vote == "9")
            ForVote = "猴";
        else if (Vote == "10")
            ForVote = "鸡";
        else if (Vote == "11")
            ForVote = "狗";
        else if (Vote == "12")
            ForVote = "猪";

        return ForVote;
    }
    #endregion

    #region 获得数字 ForSXNum
    private int ForSXNum(string Vote)
    {
        string ForVote = string.Empty;

        if (Vote == "鼠")
            ForVote = "1";
        else if (Vote == "牛")
            ForVote = "2";
        else if (Vote == "虎")
            ForVote = "3";
        else if (Vote == "兔")
            ForVote = "4";
        else if (Vote == "龙")
            ForVote = "5";
        else if (Vote == "蛇")
            ForVote = "6";
        else if (Vote == "马")
            ForVote = "7";
        else if (Vote == "羊")
            ForVote = "8";
        else if (Vote == "猴")
            ForVote = "9";
        else if (Vote == "鸡")
            ForVote = "10";
        else if (Vote == "狗")
            ForVote = "11";
        else if (Vote == "猪")
            ForVote = "12";

        return Convert.ToInt32(ForVote);
    }
    #endregion

    #region 获得单双 ForBB
    private string ForBB(string Vote)
    {
        string ForVote = string.Empty;

        if (Vote == "1")
            ForVote = "红单";
        else if (Vote == "2")
            ForVote = "红双";
        else if (Vote == "3")
            ForVote = "蓝单";
        else if (Vote == "4")
            ForVote = "蓝双";
        else if (Vote == "5")
            ForVote = "绿单";
        else if (Vote == "6")
            ForVote = "绿双";

        return ForVote;
    }
    #endregion

    #region 大小单双 ForBBB
    private string ForBBB(string Vote)
    {
        string ForVote = string.Empty;

        if (Vote == "红大单")
            ForVote = "1";
        else if (Vote == "红大双")
            ForVote = "2";
        else if (Vote == "红小单")
            ForVote = "3";
        else if (Vote == "红小双")
            ForVote = "4";
        else if (Vote == "蓝大单")
            ForVote = "5";
        else if (Vote == "蓝大双")
            ForVote = "6";
        else if (Vote == "蓝小单")
            ForVote = "7";
        else if (Vote == "蓝小双")
            ForVote = "8";
        else if (Vote == "绿大单")
            ForVote = "9";
        else if (Vote == "绿大双")
            ForVote = "10";
        else if (Vote == "绿小单")
            ForVote = "11";
        else if (Vote == "绿小双")
            ForVote = "12";

        return ForVote;
    }
    #endregion

    #region 特.码 ForType
    private string ForType(int Types)
    {
        string TyName = string.Empty;
        if (Types == 1)
            TyName = "特.码";
        else if (Types == 2)
            TyName = "单双";
        else if (Types == 3)
            TyName = "大小";
        else if (Types == 4)
            TyName = "波色";
        else if (Types == 5)
            TyName = "特肖";
        else if (Types == 6)
            TyName = "家野";
        else if (Types == 7)
            TyName = "特尾";
        else if (Types == 8)
            TyName = "特头";
        else if (Types == 9)
            TyName = "合数大小";
        else if (Types == 10)
            TyName = "合数单双";
        else if (Types == 11)
            TyName = "平特一肖";
        else if (Types == 12)
            TyName = "独平";
        else if (Types == 13)
            TyName = "平特一尾";
        else if (Types == 14)
            TyName = "五行";
        else if (Types == 15)
            TyName = "六肖";
        else if (Types == 16)
            TyName = "半波";
        else if (Types == 17)
            TyName = "特连";
        else if (Types == 18)
            TyName = "三中三";
        else if (Types == 19)
            TyName = "三中二";
        else if (Types == 20)
            TyName = "二中二";
        else if (Types == 21)
            TyName = "平特二肖";
        else if (Types == 22)
            TyName = "选五不中";
        else if (Types == 23)
            TyName = "五门";
        else if (Types == 24)
            TyName = "尾数大小";
        else if (Types == 25)
            TyName = "尾数单双";
        else if (Types == 26)
            TyName = "总分大小";
        else if (Types == 27)
            TyName = "总分单双";
        else if (Types == 28)
            TyName = "半半波";
        else if (Types == 29)
            TyName = "平特三肖";
        else if (Types == 30)
            TyName = "平特四肖";
        else if (Types == 31)
            TyName = "平特二尾";
        else if (Types == 32)
            TyName = "平特三尾";
        else if (Types == 33)
            TyName = "平特四尾";
        else if (Types == 34)
            TyName = "选六不中";
        else if (Types == 35)
            TyName = "选七不中";
        else if (Types == 36)
            TyName = "选八不中";
        else if (Types == 37)
            TyName = "选九不中";
        else if (Types == 38)
            TyName = "选十不中";
        else if (Types == 39)
            TyName = "十一不中";
        else if (Types == 40)
            TyName = "十五不中";
        else if (Types == 80)
            TyName = "复式平二中二";
        else if (Types == 81)
            TyName = "复式平三中三";
        else if (Types == 83)
            TyName = "复式平特二肖";
        else if (Types == 84)
            TyName = "复式平特三肖";
        else if (Types == 85)
            TyName = "复式平特四肖";
        else if (Types == 86)
            TyName = "复式平特二尾";
        else if (Types == 87)
            TyName = "复式平特三尾";
        else if (Types == 88)
            TyName = "复式平特四尾";

        return TyName;
    }
    #endregion

    #region 得到单双 GetDS
    /// <summary>
    /// 得到单双
    /// </summary>
    private string GetDS(int sNum)
    {
        string ForDS = string.Empty;
        if (sNum == 49)
            return "打和";

        if (sNum % 2 == 0)
            ForDS = "双";
        else
            ForDS = "单";

        return ForDS;
    }
    #endregion

    #region 得到大小 GetDX
    /// <summary>
    /// 得到大小
    /// </summary>
    private string GetDX(int sNum)
    {
        string ForDX = string.Empty;
        if (sNum <= 24)
            ForDX = "小";
        else if (sNum >= 25 && sNum != 49)
            ForDX = "大";
        else
            ForDX = "打和";

        return ForDX;
    }
    #endregion

    #region 得到尾数大小 GetWSDX
    /// <summary>
    /// 得到尾数大小
    /// </summary>
    private string GetWSDX(int sNum)
    {
        string ForDX = string.Empty;
        if (sNum == 49)
        {
            ForDX = "打和";
        }
        else
        {
            sNum = Convert.ToInt32(Utils.Right(sNum.ToString(), 1));

            if (sNum <= 4)
                ForDX = "小";
            else
                ForDX = "大";
        }
        return ForDX;
    }
    #endregion

    #region 得到尾数单双 GetWSDS
    /// <summary>
    /// 得到尾数单双
    /// </summary>
    private string GetWSDS(int sNum)
    {
        string ForDS = string.Empty;
        if (sNum == 49)
        {
            ForDS = "打和";
        }
        else
        {
            sNum = Convert.ToInt32(Utils.Right(sNum.ToString(), 1));

            if (sNum % 2 == 0)
                ForDS = "双";
            else
                ForDS = "单";
        }
        return ForDS;
    }
    #endregion

    #region 得到总分大小 GetZFDX
    /// <summary>
    /// 得到总分大小
    /// </summary>
    private string GetZFDX(int sNum, int pNum1, int pNum2, int pNum3, int pNum4, int pNum5, int pNum6)
    {
        int addNum = sNum + pNum1 + pNum2 + pNum3 + pNum4 + pNum5 + pNum6;

        string ForDX = string.Empty;
        if (sNum == 49)
        {
            ForDX = "打和";
        }
        else
        {
            if (addNum < 175)
                ForDX = "小";
            else
                ForDX = "大";
        }
        return ForDX;
    }
    #endregion

    #region 得到总分单双 GetZFDS
    /// <summary>
    /// 得到总分单双
    /// </summary>
    private string GetZFDS(int sNum, int pNum1, int pNum2, int pNum3, int pNum4, int pNum5, int pNum6)
    {
        int addNum = sNum + pNum1 + pNum2 + pNum3 + pNum4 + pNum5 + pNum6;

        string ForDS = string.Empty;
        if (sNum == 49)
        {
            ForDS = "打和";
        }
        else
        {
            if (addNum % 2 == 0)
                ForDS = "双";
            else
                ForDS = "单";
        }
        return ForDS;
    }
    #endregion

    #region 获得波色 GetBS
    private string GetBS(int sNum)
    {
        string ForBS = string.Empty;
        if (("#" + ub.GetSub("Sixred", xmlPath) + "#").Contains("#" + sNum + "#"))
        {
            ForBS = "红波";
        }
        else if (("#" + ub.GetSub("Sixblue", xmlPath) + "#").Contains("#" + sNum + "#"))
        {
            ForBS = "蓝波";
        }
        else if (("#" + ub.GetSub("Sixgreen", xmlPath) + "#").Contains("#" + sNum + "#"))
        {
            ForBS = "绿波";
        }

        return ForBS;
    }
    #endregion

    #region 获得波色 GetBBS
    private string GetBBS(int sNum)
    {
        string ForBS = string.Empty;
        string ForDS = string.Empty;
        if (sNum == 49)
        {
            return "打和";
        }
        else
        {
            if (("#" + ub.GetSub("Sixred", xmlPath) + "#").Contains("#" + sNum + "#"))
            {
                ForBS = "红";
            }
            else if (("#" + ub.GetSub("Sixblue", xmlPath) + "#").Contains("#" + sNum + "#"))
            {
                ForBS = "蓝";
            }
            else if (("#" + ub.GetSub("Sixgreen", xmlPath) + "#").Contains("#" + sNum + "#"))
            {
                ForBS = "绿";
            }

            if (sNum % 2 == 0)
                ForDS = "双";
            else
                ForDS = "单";

        }
        return ForBS + ForDS;
    }
    #endregion

    #region 得到半半波 GetBBBS
    /// <summary>
    /// 得到半半波
    /// </summary>
    /// <param name="sNum"></param>
    /// <returns></returns>
    private string GetBBBS(int sNum)
    {

        string ForBBBS = string.Empty;
        if (sNum == 49)
        {
            ForBBBS = "打和";
        }
        else
        {
            ForBBBS = GetBS(sNum).Replace("波", "") + GetDX(sNum) + GetDS(sNum);
        }
        return ForBBBS;
    }
    #endregion

    #region 获得生肖 GetSX
    /// <summary>
    /// 得到生肖
    /// </summary>
    private string GetSX(int sNum)
    {
        //生肖
        string ForSX = string.Empty;
        if (("#" + ub.GetSub("Sixsx1", xmlPath) + "#").Contains("#" + sNum + "#"))
        {
            ForSX = "鼠";
        }
        else if (("#" + ub.GetSub("Sixsx2", xmlPath) + "#").Contains("#" + sNum + "#"))
        {
            ForSX = "牛";
        }
        else if (("#" + ub.GetSub("Sixsx3", xmlPath) + "#").Contains("#" + sNum + "#"))
        {
            ForSX = "虎";
        }
        else if (("#" + ub.GetSub("Sixsx4", xmlPath) + "#").Contains("#" + sNum + "#"))
        {
            ForSX = "兔";
        }
        else if (("#" + ub.GetSub("Sixsx5", xmlPath) + "#").Contains("#" + sNum + "#"))
        {
            ForSX = "龙";
        }
        else if (("#" + ub.GetSub("Sixsx6", xmlPath) + "#").Contains("#" + sNum + "#"))
        {
            ForSX = "蛇";
        }
        else if (("#" + ub.GetSub("Sixsx7", xmlPath) + "#").Contains("#" + sNum + "#"))
        {
            ForSX = "马";
        }
        else if (("#" + ub.GetSub("Sixsx8", xmlPath) + "#").Contains("#" + sNum + "#"))
        {
            ForSX = "羊";
        }
        else if (("#" + ub.GetSub("Sixsx9", xmlPath) + "#").Contains("#" + sNum + "#"))
        {
            ForSX = "猴";
        }
        else if (("#" + ub.GetSub("Sixsx10", xmlPath) + "#").Contains("#" + sNum + "#"))
        {
            ForSX = "鸡";
        }
        else if (("#" + ub.GetSub("Sixsx11", xmlPath) + "#").Contains("#" + sNum + "#"))
        {
            ForSX = "狗";
        }
        else if (("#" + ub.GetSub("Sixsx12", xmlPath) + "#").Contains("#" + sNum + "#"))
        {
            ForSX = "猪";
        }
        return ForSX;
    }
    #endregion

    #region 获得五行 GetQS
    private string GetWX(int sNum)
    {
        string ForWX = string.Empty;
        if (("#" + ub.GetSub("Sixgold", xmlPath) + "#").Contains("#" + sNum + "#"))
        {
            ForWX = "金";
        }
        else if (("#" + ub.GetSub("Sixwood", xmlPath) + "#").Contains("#" + sNum + "#"))
        {
            ForWX = "木";
        }
        else if (("#" + ub.GetSub("Sixwater", xmlPath) + "#").Contains("#" + sNum + "#"))
        {
            ForWX = "水";
        }
        else if (("#" + ub.GetSub("Sixfire", xmlPath) + "#").Contains("#" + sNum + "#"))
        {
            ForWX = "火";
        }
        else if (("#" + ub.GetSub("Sixsoil", xmlPath) + "#").Contains("#" + sNum + "#"))
        {
            ForWX = "土";
        }
        return ForWX;
    }
    #endregion

    #region 获得家禽 GetQS
    private string GetQS(int num)
    {
        string JQ = "牛马羊鸡狗猪";
        string YS = "鼠猴兔虎龙蛇";
        string SX = GetSX(num);
        if (JQ.Contains(SX))
        {
            return "家禽";
        }
        else if (YS.Contains(SX))
        {
            return "野兽";
        }
        return "囧";
    }
    #endregion

    #region 合单双 GetHDS
    private string GetHDS(int num)
    {
        string HD = "1,3,5,7,9,10,12,14,16,18,21,23,25,27,29,30,32,34,36,38,41,43,45,47";
        string HS = "2,4,6,8,11,13,15,17,19,20,22,24,26,28,31,33,35,37,39,40,42,44,46,48";

        if (("," + HD + ",").Contains("," + num + ","))
        {
            return "合单";
        }
        else if (("," + HS + ",").Contains("," + num + ","))
        {
            return "合双";
        }
        return "打和";
    }
    #endregion

    #region 合大小 GetHDX
    private string GetHDX(int num)
    {
        string HX = "1,2,3,4,10,11,12,13,19,20,21,22,28,29,30,31,37,38,39,40,46,47,48";
        string HD = "5,6,7,8,9,14,15,16,17,18,23,24,25,26,27,32,33,34,35,36,41,42,43,44,45";

        if (("," + HD + ",").Contains("," + num + ","))
        {
            return "合大";
        }
        else if (("," + HX + ",").Contains("," + num + ","))
        {
            return "合小";
        }
        return "打和";
    }
    #endregion

    #region 五门 GetWM
    private string GetWM(int num)
    {
        string m1 = "1,2,3,4,5,6,7,8,9";
        string m2 = "10,11,12,13,14,15,16,17,18,19";
        string m3 = "20,21,22,23,24,25,26,27,28,29";
        string m4 = "30,31,32,33,34,35,36,37,38,39";
        string m5 = "40,41,42,43,44,45,46,47,48,49";
        if (("," + m1 + ",").Contains("," + num + ","))
        {
            return "一门";
        }
        else if (("," + m2 + ",").Contains("," + num + ","))
        {
            return "二门";
        }
        else if (("," + m3 + ",").Contains("," + num + ","))
        {
            return "三门";
        }
        else if (("," + m4 + ",").Contains("," + num + ","))
        {
            return "四门";
        }
        else if (("," + m5 + ",").Contains("," + num + ","))
        {
            return "五门";
        }
        return "囧";
    }
    #endregion

    #region 平码多选返回中的个数 GetPM
    /// <summary>
    /// 平码多选返回中的个数
    /// </summary>
    private int GetPM(int pNum1, int pNum2, int pNum3, int pNum4, int pNum5, int pNum6, string Vote)
    {
        int iNum = 0;
        if (("," + Vote + ",").Contains("," + pNum1 + ","))
            iNum++;
        if (("," + Vote + ",").Contains("," + pNum2 + ","))
            iNum++;
        if (("," + Vote + ",").Contains("," + pNum3 + ","))
            iNum++;
        if (("," + Vote + ",").Contains("," + pNum4 + ","))
            iNum++;
        if (("," + Vote + ",").Contains("," + pNum5 + ","))
            iNum++;
        if (("," + Vote + ",").Contains("," + pNum6 + ","))
            iNum++;

        return iNum;
    }
    #endregion

    #region 排行榜 TopPage 新20160611
    /// <summary>
    /// 排行榜
    /// </summary>
    private void TopPage()
    {
        Master.Title = "排行榜管理";
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-9]\d*$", "1"));
        int showtype = int.Parse(Utils.GetRequest("showtype", "all", 1, @"^[1-2]$", "1"));
        string uQiNo = Utils.GetRequest("uQiNo", "all", 1, "", "");

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("虚拟投注排行榜");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (showtype == 1)
            builder.Append("游戏赌神");
        else
            builder.Append("<a href=\"" + Utils.getUrl("toplist.aspx?act=top&amp;ptype=" + ptype + "&amp;showtype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">游戏赌神</a>|");

        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = " WHERE(State <> 0) ";
        string[] pageValUrl = { "uQiNo", "act", "ptype", "id", "showtype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        if (uQiNo != "")
        {
            string sql = "";
            #region 隔期或单期查询
            if (!uQiNo.Contains("-"))
            {
                string[] uqinos = uQiNo.Split('#');
                for (int i = 0; i < uqinos.Length; i++)
                {
                    if (i == 0)
                    {
                        sql = "(" + uqinos[i];
                    }
                    else
                    {
                        sql += "," + uqinos[i];
                    }
                }
                sql += ")";
            }
            else
            {
                string[] uqinos = uQiNo.Split('-');
                for (int i = 0; i < (int.Parse(uqinos[1]) + 1 - int.Parse(uqinos[0])); i++)
                {
                    if (i == 0)
                    {
                        sql = "(" + (int.Parse(uqinos[0]) + i);
                    }
                    else
                    {
                        sql += "," + (int.Parse(uqinos[0]) + i);
                    }
                }
                sql += ")";
            }
            if (sql != "") { }
            strWhere += " AND (qiNo IN " + sql + ")";
            #endregion
        }

        // 开始读取列表
        IList<LHC.Model.VotePay49> listToplist = new LHC.BLL.VotePay49().GetVotePay49s_px(pageIndex, pageSize, strWhere, out recordCount);
        if (listToplist.Count > 0)
        {
            int k = 1;
            foreach (LHC.Model.VotePay49 n in listToplist)
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
                string sText = string.Empty;
                sText = "净赢" + (n.winCent) + "" + ub.Get("SiteBz") + "";
                builder.AppendFormat("{0}.<a href=\"" + Utils.getUrl("../uinfo.aspx?uid={1}&amp;backurl=" + Utils.getPage(1) + "") + "\">{2}</a>{3}", (pageIndex - 1) * pageSize + k, n.UsID, new BCW.BLL.User().GetUsName(n.UsID), sText);
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

        string strText = "输入期号(多期用-分隔，单独几期用#号分隔)/如输入1-10或1#2#3或单独一期期号/,,";
        string strName = "uQiNo,act";
        string strType = "text,hidden";
        string strValu = uQiNo + "'top";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "搜期号," + Utils.getUrl("six49.aspx") + ",post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        //builder.Append("<a href=\"" + Utils.getUrl("toplist.aspx?act=del&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">清空记录</a><br />");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    #endregion

    #region 计算投注注数,并非常用功能
    /// <summary>
    /// 计算投注注数,并非常用功能
    /// </summary>
    private void ChkTopPage()
    {
        long fsCent = 0;//算出复式总下注
        #region 生成SQL
        ArrayList strSql = new ArrayList();
        DataSet ds = null;
        ds = new LHC.BLL.VotePay49().GetList("*", "  (Types = 1 or Types=12) AND payNum=1  AND (Vote LIKE '%,%') ");
        string Vote = "";
        #endregion

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            #region 返奖
            for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
            {
                LHC.Model.VotePay49 model = new LHC.BLL.VotePay49().GetVotePay49(int.Parse(ds.Tables[0].Rows[k]["ID"].ToString()));

                if (model.Types >= 80)//复式处理
                {
                    #region 投注类型判断
                    int iHm = 0;
                    if (model.Types == 80)
                        iHm = 2;
                    else if (model.Types == 81)
                        iHm = 3;
                    else if (model.Types == 83)
                        iHm = 2;
                    else if (model.Types == 84)
                        iHm = 3;
                    else if (model.Types == 85)
                        iHm = 4;
                    else if (model.Types == 86)
                        iHm = 2;
                    else if (model.Types == 87)
                        iHm = 3;
                    else if (model.Types == 88)
                        iHm = 4;
                    #endregion

                    #region 计算复式投注
                    string[] vTemp = { };

                    Vote = model.Vote;
                    Vote = Vote.Replace("-", ",").Replace("，", ",");
                    vTemp = Vote.Split(',');

                    string getNum = "";

                    List<string> listNum = new Combination().GetCombination2(vTemp.Length, iHm, vTemp);
                    if (listNum == null) { continue; }

                    if (listNum.Count > 0)
                    {
                        foreach (string n in listNum)
                        {
                            getNum += "，" + n;
                        }
                        getNum = Utils.Mid(getNum, 1, getNum.Length);
                    }

                    int payNum = 1;
                    if (getNum.Contains(","))
                    {
                        payNum = Utils.GetStringNum(getNum, "，") + 1;
                        fsCent = Convert.ToInt64(model.payCent * payNum);
                    }
                    else
                    {
                        fsCent = model.payCent;
                    }
                    #endregion

                    #region 写入数据库
                    model.PayNum = payNum;
                    new LHC.BLL.VotePay49().Update(model);
                    #endregion
                }

                if (model.Types == 1 || model.Types == 12)
                {
                    //计算多个号投注
                    string[] vTemp = { };
                    Vote = model.Vote;
                    Vote = Vote.Replace("-", ",").Replace("，", ",");
                    vTemp = Vote.Split(',');
                    if (vTemp.Length > 1)
                    {
                        model.PayNum = vTemp.Length;
                        new LHC.BLL.VotePay49().Update(model);
                    }
                }
            }
            #endregion

            Utils.Success("计算完成", "");
        }
        else
        {
            Utils.Success("暂无数据", "");
        }
    }
    #endregion
}