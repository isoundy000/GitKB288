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
///  修改人陈志基 2016 0421
///  1·添加设置取款权限
///  2·修改取款方式
/// </summary>
public partial class bbs_group : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/group.xml";
    protected string strText = string.Empty;
    protected string strName = string.Empty;
    protected string strType = string.Empty;
    protected string strValu = string.Empty;
    protected string strEmpt = string.Empty;
    protected string strIdea = string.Empty;
    protected string strOthe = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        //维护提示
        if (ub.GetSub("GroupStatus", xmlPath) == "1")
        {
            Utils.Safe("" + ub.GetSub("GroupName", xmlPath) + "系统");
        }

        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "info":
            case "save":
                InfoPage(act);
                break;
            case "addchat":
                AddChatPage();
                break;
            case "addsavechat":
                AddSaveChatPage();
                break;
            case "deltext":
                DelTextPage();
                break;
            case "add":
                AddPage();
                break;
            case "addsave":
                AddSavePage();
                break;
            case "search":
                SearchPage();
                break;
            case "help":
                HelpPage();
                break;
            case "inforum":
            case "inchat":
                InPage(act);
                break;
            case "sign":
                SignPage();
                break;
            case "addin":
                AddInPage();
                break;
            case "addout":
                AddOutPage();
                break;
            case "addinok":
                AddInOkPage();
                break;
            case "groupid":
                GroupIdPage();
                break;
            case "me":
                MeGroupPage();
                break;
            case "exit":
                ExitPage();
                break;
            case "top":
            case "new":
                TopPage(act);
                break;
            case "leibie":
                LeibiePage();
                break;
            case "city":
                CityPage();
                break;
            case "list":
                ListPage();
                break;
            case "view":
                ViewPage();
                break;
            case "admin":
                AdminPage();
                break;
            case "adminset":
                AdminSetPage();
                break;
            case "adminsetsave":
                AdminSetSavePage();
                break;
            case "adminpay":
                AdminPayPage();
                break;
            case "adminpass":
                AdminPassPage();
                break;
            case "adminaddguest":
                AdminAddGuestPage();
                break;
            case "fund":
                FundPage();
                break;
            case"sdfundget":
                SDFundGet();
                break;
            case "fundlist":
                FundListPage();
                break;
            case "fundpay":
                FundPayPage();
                break;
            case "fundget":
                FundGetPage();
                break;
            case "fundpwd":
                FundPwdPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        Master.Title = ub.GetSub("GroupName", xmlPath);

        if (ub.GetSub("GroupLogo", xmlPath) != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<img src=\"" + ub.GetSub("GroupLogo", xmlPath) + "\" alt=\"load\"/>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        //拾物随机
        builder.Append(BCW.User.Game.GiftFlows.ShowGiftFlows(15));
        //顶部滚动
        builder.Append(BCW.User.Master.OutTopRand(7));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=me") + "\">我的" + ub.GetSub("GroupName", xmlPath) + "</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=top") + "\">" + ub.GetSub("GroupName", xmlPath) + "排行</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("【热门" + ub.GetSub("GroupName", xmlPath) + "】");
        builder.Append(Out.Tab("</div>", "<br />"));
        DataSet ds = null;
        //热门
        ds = new BCW.BLL.Group().GetList("TOP 5 ID,Title,iTotal", "Status=0 ORDER BY iTotal Desc");
        if (ds != null && ds.Tables[0].Rows.Count != 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                int id = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                string Title = ds.Tables[0].Rows[i]["Title"].ToString();
                int iTotal = int.Parse(ds.Tables[0].Rows[i]["iTotal"].ToString());
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=info&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + Title + "(" + iTotal + "人)</a>");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=top") + "\">..更多热门" + ub.GetSub("GroupName", xmlPath) + "</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("【新开" + ub.GetSub("GroupName", xmlPath) + "】");
        builder.Append(Out.Tab("</div>", "<br />"));
        //新开
        ds = new BCW.BLL.Group().GetList("TOP 5 ID,Title,iTotal", "Status=0 ORDER BY ID Desc");
        if (ds != null && ds.Tables[0].Rows.Count != 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                int id = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                string Title = ds.Tables[0].Rows[i]["Title"].ToString();
                int iTotal = int.Parse(ds.Tables[0].Rows[i]["iTotal"].ToString());
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=info&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + Title + "(" + iTotal + "人)</a>");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=new") + "\">..更多新增" + ub.GetSub("GroupName", xmlPath) + "</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        strText = ",,";
        strName = "keyword,act";
        strType = "stext,hidden";
        strValu = "'search";
        strEmpt = "true,false";
        strIdea = "";
        strOthe = "搜" + ub.GetSub("GroupName", xmlPath) + ",group.aspx,post,3,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("查看:<a href=\"" + Utils.getUrl("group.aspx?act=leibie") + "\">分类</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=city") + "\">地区</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=help") + "\">帮助</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void SearchPage()
    {
        Master.Title = "搜索" + ub.GetSub("GroupName", xmlPath) + "";
        string keyword = Utils.GetRequest("keyword", "all", 2, @"^[\s\S]{1,10}$", "请输入1-10字的搜索关键字");
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("搜索:“" + keyword + "”结果：");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        DataSet ds = new BCW.BLL.Group().GetList("ID,Title", "Status=0 and Title like '%" + keyword + "%'");
        if (ds != null && ds.Tables[0].Rows.Count != 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                int id = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
                string Title = ds.Tables[0].Rows[0]["Title"].ToString();
                builder.Append("<a href=\"" + Utils.getUrl("/bbs/group.aspx?act=info&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + Title + "</a><br />");
            }
        }
        else
        {
            builder.Append("没有相关记录..<br />");
        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.RHr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("group.aspx") + "\">返回上级</a>\n");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回首页</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void TopPage(string act)
    {
        Master.Title = "" + ub.GetSub("GroupName", xmlPath) + "排行榜";
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));
        //拾物随机
        builder.Append(BCW.User.Game.GiftFlows.ShowGiftFlows(15));
        //顶部滚动
        builder.Append(BCW.User.Master.OutTopRand(7));
        if (act == "top")
        {
            builder.Append(Out.Tab("<div class=\"title\">" + ub.GetSub("GroupName", xmlPath) + "排行</div>", ""));
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("排行:");
            if (ptype == 1)
                builder.Append("成员|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=top&amp;ptype=1") + "\">成员</a>|");

            if (ptype == 2)
                builder.Append("基金|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=top&amp;ptype=2") + "\">基金</a>|");

            if (ptype == 3)
                builder.Append("人气");
            else
                builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=top&amp;ptype=3") + "\">人气</a>");

            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            Master.Title = "最新开通" + ub.GetSub("GroupName", xmlPath) + "";
            builder.Append(Out.Tab("<div class=\"title\">最新" + ub.GetSub("GroupName", xmlPath) + "</div>", ""));
        }
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string strOrder = "";
        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        strWhere = "Status=0";
        //排序条件
        if (ptype == 1)
            strOrder = "iTotal Desc";
        else if (ptype == 2)
            strOrder = "iCent Desc";
        else if (ptype == 3)
            strOrder = "iClick Desc";
        else
            strOrder = "ID Desc";
        // 开始读取列表
        IList<BCW.Model.Group> listGroup = new BCW.BLL.Group().GetGroups(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listGroup.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Group n in listGroup)
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
                builder.AppendFormat("{0}.<a href=\"" + Utils.getUrl("group.aspx?act=info&amp;id={1}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}({3})</a>", ((pageIndex - 1) * pageSize + k), n.ID, n.Title, n.iTotal);
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
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("group.aspx") + "\">" + ub.GetSub("GroupName", xmlPath) + "首页</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ListPage()
    {
        Master.Title = "" + ub.GetSub("GroupName", xmlPath) + "列表";
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-9]\d*$", "0"));
        int pid = int.Parse(Utils.GetRequest("pid", "all", 1, @"^[0-9]\d*$", "-1"));
        int cid = int.Parse(Utils.GetRequest("cid", "all", 1, @"^[0-9]\d*$", "-1"));
        string sTitle = "全部";
        if (ptype > 0)
            sTitle = BCW.User.AppCase.CaseGroup(ptype);
        else
        {
            if (pid != -1 && cid != -1)
            {
                sTitle = BCW.User.City.city[pid][cid].ToString();

            }
        }
        //拾物随机
        builder.Append(BCW.User.Game.GiftFlows.ShowGiftFlows(15));
        //顶部滚动
        builder.Append(BCW.User.Master.OutTopRand(7));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(sTitle + "" + ub.GetSub("GroupName", xmlPath) + "列表");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string strOrder = "";
        string[] pageValUrl = { "act", "ptype", "pid", "cid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        strWhere = "Status=0";
        if (ptype > 0)
        {
            strWhere += " and Types=" + ptype + "";
        }
        else
        {
            if (pid != -1 && cid != -1)
            {
                strWhere += " and City LIKE '%" + sTitle + "%'";
            }
        }
        //排序条件
        strOrder = "iTotal Desc";
        // 开始读取列表
        IList<BCW.Model.Group> listGroup = new BCW.BLL.Group().GetGroups(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listGroup.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Group n in listGroup)
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
                builder.AppendFormat("{0}.<a href=\"" + Utils.getUrl("group.aspx?act=info&amp;id={1}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}({3})</a>", ((pageIndex - 1) * pageSize + k), n.ID, n.Title, n.iTotal);
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
        builder.Append("<a href=\"" + Utils.getPage("group.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("group.aspx") + "\">" + ub.GetSub("GroupName", xmlPath) + "首页</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void LeibiePage()
    {
        Master.Title = "" + ub.GetSub("GroupName", xmlPath) + "分类";
        //拾物随机
        builder.Append(BCW.User.Game.GiftFlows.ShowGiftFlows(15));
        //顶部滚动
        builder.Append(BCW.User.Master.OutTopRand(7));
        builder.Append(Out.Tab("<div class=\"title\">" + ub.GetSub("GroupName", xmlPath) + "分类</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        for (int i = 0; i < 20; i++)
        {
            builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=list&amp;ptype=" + (i + 1) + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.AppCase.CaseGroup(i + 1) + "</a>(" + new BCW.BLL.Group().GetGroupNum(i + 1) + ") ");
            if (i > 0 && (i + 1) % 4 == 0)
                builder.Append("<br />");
        }
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("group.aspx") + "\">" + ub.GetSub("GroupName", xmlPath) + "首页</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void CityPage()
    {
        int pid = int.Parse(Utils.GetRequest("pid", "get", 1, @"^[0-9]\d*$", "-1"));
        if (pid == -1)
        {
            Master.Title = "省份列表";
            builder.Append(Out.Tab("<div class=\"title\">省份列表</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            for (int i = 0; i <= 33; i++)
            {
                builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=city&amp;pid=" + (i) + "") + "\">" + BCW.User.AppCase.CaseSheng(i) + "</a> ");
                if (i > 0 && (i + 1) % 4 == 0)
                    builder.Append("<br />");
            }
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            Master.Title = "城市列表";
            builder.Append(Out.Tab("<div class=\"title\">城市列表</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            string[] city = BCW.User.City.city[pid];
            for (int i = 0; i < city.Length; i++)
            {
                builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=list&amp;pid=" + pid + "&amp;cid=" + i + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + city[i] + "</a>(" + new BCW.BLL.Group().GetGroupNum(city[i]) + ") ");
                if (i > 0 && (i + 1) % 4 == 0)
                    builder.Append("<br />");
            }
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        if (pid != -1)
            builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=city") + "\">上级</a>-");
        else
            builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");

        builder.Append("<a href=\"" + Utils.getUrl("group.aspx") + "\">" + ub.GetSub("GroupName", xmlPath) + "首页</a>");

        builder.Append(Out.Tab("</div>", ""));
    }


    private void InfoPage(string act)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int VipLeven = BCW.User.Users.VipLeven(meid);
        string GroupId = new BCW.BLL.User().GetGroupId(meid);
        if (VipLeven == 0)
        {
            int DqNum = (Utils.GetStringNum(GroupId.Replace("##", "#"), "#") - 1);
            if (DqNum > 5)
            {
                Utils.Error("您的VIP会员已过期，请进行续费才能同时加入5个以上的" + ub.GetSub("GroupName", xmlPath) + "，否则只能同时加入5个才可以使用" + ub.GetSub("GroupName", xmlPath) + "<br /><a href=\"" + Utils.getUrl("finance.aspx?act=addvip&amp;backurl=" + Utils.PostPage(1) + "") + "\">&gt;&gt;我现在要续费VIP</a><br /><a href=\"" + Utils.getUrl("group.aspx?act=me&amp;backurl=" + Utils.PostPage(1) + "") + "\">&lt;&lt;我要退出一些圈子</a>", "");
            }
        }

        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "" + ub.GetSub("GroupName", xmlPath) + "ID错误"));
        BCW.Model.Group model = new BCW.BLL.Group().GetGroup(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        else if (model.Status == 1)
        {
            Utils.Error("" + ub.GetSub("GroupName", xmlPath) + "未开通", "");
        }
        Master.Title = model.Title;
        //不同一天则清空今天来访ID
        string VisitId = model.VisitId;
        if (string.IsNullOrEmpty(model.VisitTime.ToString()) || model.VisitTime.ToShortDateString() != DateTime.Now.ToShortDateString())
        {
            new BCW.BLL.Group().UpdateVisitId(id);
            VisitId = string.Empty;
        }
        if (meid > 0)
        {
            if (string.IsNullOrEmpty(VisitId) || VisitId.IndexOf("#" + meid + "#") == -1)
            {
                new BCW.BLL.Group().UpdateVisitId(id, VisitId + "#" + meid + "#");
            }
        }
        //拾物随机
        builder.Append(BCW.User.Game.GiftFlows.ShowGiftFlows(15));
        //顶部滚动
        builder.Append(BCW.User.Master.OutTopRand(7));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("宣言:" + model.Notes + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        bool Isvi = true;
        if (GroupId.IndexOf("#" + id + "#") == -1)
        {
            Isvi = IsCTID(meid);
        }

        if (model.ChatStatus == 0 && Isvi == false)
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=addin&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">加入本" + ub.GetSub("GroupName", xmlPath) + "</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=groupid&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + ub.GetSub("GroupName", xmlPath) + "成员(" + new BCW.BLL.User().GetGroupNum(id) + "/" + model.iTotal + ")</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=addchat&amp;id=" + id + "") + "\">发言</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=info&amp;id=" + id + "") + "\">刷新</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=fund&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">基金</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=view&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">资料</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=sign&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">签到</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string strWhere = "";
            string[] pageValUrl = { "act", "id", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            //查询条件
            strWhere = "GroupId=" + id + "";

            // 开始读取列表
            IList<BCW.Model.GroupText> listGroupText = new BCW.BLL.GroupText().GetGroupTexts(pageIndex, pageSize, strWhere, out recordCount);
            if (listGroupText.Count > 0)
            {
                int k = 1;
                foreach (BCW.Model.GroupText n in listGroupText)
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
                    builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "</a> (" + DT.FormatDate(n.AddTime, 12) + ")");

                    builder.Append("<br />" + Out.SysUBB(n.Content));
                    if (model.UsID == meid || IsCTID(meid) == true)
                    {
                        builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=deltext&amp;id=" + n.GroupId + "&amp;bid=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[删]</a>");
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
        }
        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("myedit.aspx?act=groupchat&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + ub.GetSub("GroupName", xmlPath) + "聊天设置&gt;&gt;</a><br />");
        if (model.ForumId > 0)
            builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + model.ForumId + "") + "\">进入" + ub.GetSub("GroupName", xmlPath) + "论坛&gt;&gt;</a> ");
        else
            builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=inforum&amp;id=" + id + "") + "\">进入" + ub.GetSub("GroupName", xmlPath) + "论坛&gt;&gt;</a> ");

        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("group.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("group.aspx") + "\">" + ub.GetSub("GroupName", xmlPath) + "首页</a>");
        if (model.UsID == meid)
        {
            builder.Append("<br /><a href=\"" + Utils.getUrl("group.aspx?act=admin&amp;id=" + id + "") + "\">=" + ub.GetSub("GroupzName", xmlPath) + "管理=</a>");
        }
        builder.Append(Out.Tab("</div>", ""));
    }

    private void DelTextPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
        int bid = int.Parse(Utils.GetRequest("bid", "all", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.Group model = new BCW.BLL.Group().GetGroup(id);
        if (model == null)
        {
            Utils.Error("不存在的" + ub.GetSub("GroupName", xmlPath) + "", "");
        }
        if (model.UsID != meid && IsCTID(meid) == false)
        {
            Utils.Error("你的权限不足", "");
        }
        if (!new BCW.BLL.GroupText().Exists(bid, id))
        {
            Utils.Error("你的权限不足", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {
            new BCW.BLL.GroupText().Delete(bid);
            Utils.Success("删除发言", "删除发言成功..", Utils.getUrl("group.aspx?act=info&amp;id=" + id + ""), "1");
        }
        else
        {
            Master.Title = "删除发言";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此发言?");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("group.aspx?info=ok&amp;act=deltext&amp;id=" + id + "&amp;bid=" + bid + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=info&amp;id=" + id + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private void AddChatPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Master.Title = "发言";
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "" + ub.GetSub("GroupName", xmlPath) + "ID错误"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("" + ub.GetSub("GroupName", xmlPath) + "发言");
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
        strName = "Content,id,act";
        strType = "text,hidden,hidden";
        strValu = "" + Copytemp + "'" + id + "'addsavechat";
        strEmpt = "true,false,false";
        strIdea = "/";
        strOthe = "确定发言,group.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("插入:<a href=\"" + Utils.getUrl("function.aspx?act=admsub&amp;backurl=" + Utils.PostPage(1) + "") + "\">常用短语</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("function.aspx?act=face&amp;backurl=" + Utils.PostPage(1) + "") + "\">表情</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=addchat&amp;id=" + id + "&amp;ff=" + ff + "&amp;dd=" + dd + "&amp;copy=1") + "\">[粘贴]</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=info&amp;id=" + id + "") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));

    }

    private void AddSaveChatPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[1-9]\d*$", "" + ub.GetSub("GroupName", xmlPath) + "ID错误"));
        string Content = Utils.GetRequest("Content", "post", 2, @"^[\s\S]{1,500}$", "留言内容限1-500字");
        BCW.Model.Group group = new BCW.BLL.Group().GetGroupMe(id);
        bool Isvi = true;
        string GroupId = new BCW.BLL.User().GetGroupId(meid);
        if (GroupId.IndexOf("#" + id + "#") == -1)
        {
            Isvi = IsCTID(meid);
        }
        if (group.ChatStatus <= 1 && Isvi == false)
        {
            Utils.Error("非成员不能发言！<br /><a href=\"" + Utils.getUrl("group.aspx?act=addin&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">加入本" + ub.GetSub("GroupName", xmlPath) + "</a>", "");
        }
        //是否刷屏
        string appName = "LIGHT_GROUPCHAT";
        int Expir = Convert.ToInt32(ub.GetSub("GroupExpir", xmlPath));
        BCW.User.Users.IsFresh(appName, Expir);

        BCW.Model.GroupText model = new BCW.Model.GroupText();
        model.GroupId = id;
        model.UsID = meid;
        model.ToID = meid;
        model.UsName = new BCW.BLL.User().GetUsName(meid);
        model.ToName = "";
        model.Content = Content;
        model.IsKiss = 0;
        model.AddTime = DateTime.Now;
        new BCW.BLL.GroupText().Add(model);
        //通知圈内在线成员
        string OnIDs = "";
        DataSet ds = new BCW.BLL.User().GetList("ID", "EndTime>='" + DateTime.Now.AddMinutes(-Convert.ToInt32(ub.Get("SiteExTime"))) + "' and GroupId LIKE '%#" + id + "#%' and ForumSet LIKE '%圈聊提醒|0%'");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                OnIDs += "#" + ds.Tables[0].Rows[i]["ID"].ToString() + "#";
            }
        }
        if (OnIDs != "")
        {
            BCW.Model.Network work = new BCW.Model.Network();
            if (Content.Length > 30)
                work.Title = Utils.Left(Content, 30) + "..";
            else
                work.Title = Content;

            work.Types = 2;
            work.UsID = id;
            work.UsName = group.Title;
            work.OverTime = DateTime.Now.AddMinutes(10);
            work.AddTime = DateTime.Now;
            work.OnIDs = OnIDs;
            work.IsUbb = 0;

            if (new BCW.BLL.Network().ExistsGroupChat(2, id))
                new BCW.BLL.Network().UpdateGroupChat(work);
            else
                new BCW.BLL.Network().Add(work);
        }
        Utils.Success("发言", "发言成功，正在返回..", Utils.getUrl("group.aspx?act=info&amp;id=" + id + ""), "1");
    }

    private void ViewPage()
    {
        int meid = new BCW.User.Users().GetUsId();

        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "" + ub.GetSub("GroupName", xmlPath) + "ID错误"));
        BCW.Model.Group model = new BCW.BLL.Group().GetGroup(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        else if (model.Status == 1)
        {
            Utils.Error("" + ub.GetSub("GroupName", xmlPath) + "未开通", "");
        }
        Master.Title = model.Title;
        //不同一天则清空今天来访ID
        string VisitId = model.VisitId;
        if (string.IsNullOrEmpty(model.VisitTime.ToString()) || model.VisitTime.ToShortDateString() != DateTime.Now.ToShortDateString())
        {
            new BCW.BLL.Group().UpdateVisitId(id);
            VisitId = string.Empty;
        }
        if (meid > 0)
        {
            if (string.IsNullOrEmpty(VisitId) || VisitId.IndexOf("#" + meid + "#") == -1)
            {
                new BCW.BLL.Group().UpdateVisitId(id, VisitId + "#" + meid + "#");
            }
        }
        //拾物随机
        builder.Append(BCW.User.Game.GiftFlows.ShowGiftFlows(15));
        //顶部滚动
        builder.Append(BCW.User.Master.OutTopRand(7));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("" + ub.GetSub("GroupName", xmlPath) + "徽章");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        if (!string.IsNullOrEmpty(model.Logo))
            builder.Append("<img src=\"" + model.Logo + "\" alt=\"load\"/>");
        else
            builder.Append("<img src=\"/Files/sys/group.gif\" alt=\"load\"/>");

        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("" + ub.GetSub("GroupName", xmlPath) + "名称:" + model.Title + "<br />");
        builder.Append("" + ub.GetSub("GroupName", xmlPath) + "类型:" + BCW.User.AppCase.CaseGroup(model.Types) + "<br />");
        builder.Append("所属城市:" + model.City + "<br />");
        builder.Append("" + ub.GetSub("GroupzName", xmlPath) + ":<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(model.UsID, 2) + "</a><br />");
        builder.Append("创建时间:" + DT.FormatDate(model.AddTime, 5) + "<br />");
        builder.Append("成员人数:" + model.iTotal + "人<br />");
        builder.Append("在线人数:" + new BCW.BLL.User().GetGroupNum(id) + "<br />");

        int TodaySign = 0;
        if (!string.IsNullOrEmpty(model.SignID))
            TodaySign = (Utils.GetStringNum(model.SignID, "##") + 1);

        builder.Append("今天签到:" + TodaySign + "人<br />");

        int TodayClick = 0;
        if (!string.IsNullOrEmpty(model.VisitId))
            TodayClick = (Utils.GetStringNum(model.VisitId, "##") + 1);

        builder.Append("今天人气:" + TodayClick + "人<br />");
        builder.Append("累积人气:" + model.iClick + "人<br />");
        if (DT.FormatDate(model.ExTime, 0) != "1990-01-01 00:00:00")
        {
            if (model.ExTime >= DateTime.Now)
            {
                builder.Append("有效期限:还有" + DT.DateDiff2(model.ExTime, DateTime.Now) + "<br />");
            }
            else
            {
                builder.Append("有效期限:已过期,请" + ub.GetSub("GroupName", xmlPath) + "主续费.<br />");
            }
        }
        else
        {
            builder.Append("有效期限:永不过期<br />");
        }
        builder.Append("库存基金:" + model.iCent + "" + ub.Get("SiteBz") + "<br />");
        builder.Append("宣言:" + model.Notes + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (model.ForumId > 0)
            builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + model.ForumId + "") + "\">" + ub.GetSub("GroupName", xmlPath) + "论坛</a>");
        else
            builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=inforum&amp;id=" + id + "") + "\">" + ub.GetSub("GroupName", xmlPath) + "论坛</a>");

        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=groupid&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">成员列表</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=sign&amp;id=" + id + "") + "\">马上签到</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=addin&amp;id=" + id + "") + "\">加入" + ub.GetSub("GroupName", xmlPath) + "</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=fund&amp;id=" + id + "") + "\">圈子基金|捐款</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("group.aspx") + "\">" + ub.GetSub("GroupName", xmlPath) + "首页</a>");
        builder.Append(Out.Tab("</div>", ""));

        if (model.UsID == meid)
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=admin&amp;id=" + id + "") + "\">=管理" + ub.GetSub("GroupName", xmlPath) + "=</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private void AdminPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "" + ub.GetSub("GroupName", xmlPath) + "ID错误"));
        BCW.Model.Group model = new BCW.BLL.Group().GetGroup(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.UsID != meid)
        {
            Utils.Error("你的权限不足", "");
        }
        Master.Title = "管理" + model.Title;
        builder.Append(Out.Tab("<div class=\"title\">管理" + ub.GetSub("GroupName", xmlPath) + "</div>", ""));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=adminset&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + ub.GetSub("GroupName", xmlPath) + "设置</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=adminpass&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">审核成员</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=groupid&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">成员管理</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=adminaddguest&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">群发消息</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=fund&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + ub.GetSub("GroupName", xmlPath) + "基金</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=adminpay&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">续费" + ub.GetSub("GroupName", xmlPath) + "</a>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=info&amp;id=" + id + "") + "\">我的" + ub.GetSub("GroupName", xmlPath) + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void AdminSetPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "" + ub.GetSub("GroupName", xmlPath) + "ID错误"));
        BCW.Model.Group model = new BCW.BLL.Group().GetGroup(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.UsID != meid)
        {
            Utils.Error("你的权限不足", "");
        }
        Master.Title = "管理" + model.Title;
        builder.Append(Out.Tab("<div class=\"title\">管理" + ub.GetSub("GroupName", xmlPath) + "</div>", ""));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("" + ub.GetSub("GroupName", xmlPath) + "设置");
        builder.Append(Out.Tab("</div>", ""));

        strText = "" + ub.GetSub("GroupName", xmlPath) + "徽章(本站图片地址):/,宣言:/,加入" + ub.GetSub("GroupName", xmlPath) + "限制:/," + ub.GetSub("GroupName", xmlPath) + "论坛设置:/,非成员发言性质:/,签到得币:/,,";
        strName = "Logo,Notes,InType,ForumStatus,ChatStatus,SignCent,id,act";
        strType = "text,text,select,select,select,snum,hidden,hidden";
        strValu = "" + model.Logo + "'" + model.Notes + "'" + model.InType + "'" + model.ForumStatus + "'" + model.ChatStatus + "'" + model.SignCent + "'" + id + "'adminsetsave";
        strEmpt = "true,true,0|不限制|1|需要验证|2|禁止加入,0|开放非成员|1|禁止非成员|2|暂停论坛,0|禁非成员|1|可以浏览|2|允许发言,false,false,false";
        strIdea = "/";
        strOthe = "确定设置,group.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("group.aspx?act=admin&amp;id=" + id + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=info&amp;id=" + id + "") + "\">我的" + ub.GetSub("GroupName", xmlPath) + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void AdminSetSavePage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        //管理安全提示
        string[] p_pageArr = { "act", "id", "Logo", "Notes", "InType", "ForumStatus", "ChatStatus", "SignCent", "backurl" };
        BCW.User.Role.AdminSafePage(meid, Utils.getPageUrl(), p_pageArr, "post");

        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[1-9]\d*$", "ID错误"));
        string Logo = Utils.GetRequest("Logo", "post", 3, @"^.+?.(gif|jpg|bmp|jpeg|png)$", "请输入正确的徽章图片地址，可留空");
        string Notes = Utils.GetRequest("Notes", "post", 3, @"^[^\^]{1,50}$", "宣言限50字内，可留空");
        int InType = int.Parse(Utils.GetRequest("InType", "post", 2, @"^[0-2]$", "加入" + ub.GetSub("GroupName", xmlPath) + "限制选择错误"));
        int ForumStatus = int.Parse(Utils.GetRequest("ForumStatus", "post", 2, @"^[0-1]$", "论坛开关选择错误"));
        int ChatStatus = int.Parse(Utils.GetRequest("ChatStatus", "post", 2, @"^[0-2]$", "非成员发言性质选择错误"));
        int SignCent = int.Parse(Utils.GetRequest("SignCent", "post", 2, @"^[0-9]\d*$", "签到得币填写错误"));
        BCW.Model.Group m = new BCW.BLL.Group().GetGroup(id);
        if (m == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (m.UsID != meid)
        {
            Utils.Error("你的权限不足", "");
        }
        if (!Utils.IsinUrl(Logo))
        {
            Utils.Error("徽章地址必须是本站图片地址", "");
        }
        BCW.Model.Group model = new BCW.Model.Group();
        model.ID = id;
        model.Logo = Logo;
        model.Notes = Notes;
        model.InType = InType;
        model.ForumStatus = ForumStatus;
        model.ChatStatus = ChatStatus;
        model.SignCent = SignCent;
        new BCW.BLL.Group().Update2(model);
        Utils.Success("设置" + ub.GetSub("GroupName", xmlPath) + "", "设置" + ub.GetSub("GroupName", xmlPath) + "成功..", Utils.getUrl("group.aspx?act=admin&amp;id=" + id + ""), "1");
    }

    private void AdminAddGuestPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
        string info = Utils.GetRequest("info", "post", 1, "", "");
        BCW.Model.Group m = new BCW.BLL.Group().GetGroup(id);
        if (m == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (m.UsID != meid)
        {
            Utils.Error("你的权限不足", "");
        }
        if (info == "ok")
        {
            //管理安全提示
            string[] p_pageArr = { "act", "id", "addType", "Content", "info" };
            BCW.User.Role.AdminSafePage(meid, Utils.getPageUrl(), p_pageArr, "post");

            int addType = int.Parse(Utils.GetRequest("addType", "post", 1, @"^[0-1]$", "0"));
            string Content = Utils.GetRequest("Content", "post", 2, @"^[\s\S]{1,300}$", "内容限1-300字");
            string strWhere = "GroupId LIKE '%#" + id + "#%'";
            if (addType == 0)
            {
                strWhere += " and EndTime>'" + DateTime.Now.AddMinutes(-Convert.ToInt32(ub.Get("SiteExTime"))) + "'";
            }
            int k = 0;
            int iPrice = 0;
            DataSet ds = new BCW.BLL.User().GetList("ID,UsName", strWhere);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                //计算花费
                iPrice = Utils.ParseInt(ub.GetSub("GroupGuestPrice", xmlPath));
                if (m.iCent < iPrice * ds.Tables[0].Rows.Count)
                {
                    Utils.Error("" + ub.GetSub("GroupName", xmlPath) + "基金不足", "");
                }
                string mename = new BCW.BLL.User().GetUsName(meid);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    int hid = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                    string UsName = ds.Tables[0].Rows[i]["UsName"].ToString();
                    //发送内线
                    //new BCW.BLL.Guest().Add(hid, UsName, Content);

                    BCW.Model.Guest model = new BCW.Model.Guest();
                    model.FromId = meid;
                    model.FromName = mename;
                    model.ToId = hid;
                    model.ToName = UsName;
                    model.Content = Content;
                    model.TransId = 0;
                    new BCW.BLL.Guest().Add(model);
                    //更新联系时间
                    new BCW.BLL.Friend().UpdateTime(meid, hid);

                    k++;
                }
                BCW.Model.Grouplog addmodel = new BCW.Model.Grouplog();
                addmodel.GroupId = id;
                addmodel.Types = 2;
                addmodel.UsID = meid;
                addmodel.UsName = new BCW.BLL.User().GetUsName(meid);
                addmodel.Content = "" + ub.GetSub("GroupName", xmlPath) + "群发";
                addmodel.PayCent = Convert.ToInt64(iPrice * k);
                addmodel.AddTime = DateTime.Now;
                new BCW.BLL.Grouplog().Add(addmodel);
                new BCW.BLL.Group().UpdateiCent(id, -Convert.ToInt64(iPrice * k));
            }
            Utils.Success("群发消息", "群发" + k + "位成员成功，花费" + Convert.ToInt64(iPrice * k) + "" + ub.Get("SiteBz") + "", Utils.getPage("group.aspx?act=admin&amp;id=" + id + ""), "2");
        }
        else
        {
            Master.Title = "发送消息";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("发送系统消息");
            builder.Append(Out.Tab("</div>", ""));
            string strText = "群发选项:/,消息内容/,,,";
            string strName = "addType,Content,id,act,info";
            string strType = "select,textarea,hidden,hidden,hidden";
            string strValu = "''" + id + "'adminaddguest'ok";
            string strEmpt = "0|在线成员|1|全部成员,true,false,false,false";
            string strIdea = "/";
            string strOthe = "群发" + ub.GetSub("GroupName", xmlPath) + "内成员,group.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append("提示:群发每位成员收费" + ub.GetSub("GroupGuestPrice", xmlPath) + "" + ub.Get("SiteBz") + "，群发费用将在基金中扣取.");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getPage("group.aspx?act=admin&amp;id=" + id + "") + "\">上级</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=info&amp;id=" + id + "") + "\">我的" + ub.GetSub("GroupName", xmlPath) + "</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private void AdminPayPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "" + ub.GetSub("GroupName", xmlPath) + "ID错误"));
        BCW.Model.Group model = new BCW.BLL.Group().GetGroup(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.UsID != meid)
        {
            Utils.Error("你的权限不足", "");
        }
        if (model.Status == 1)
        {
            Utils.Error("" + ub.GetSub("GroupName", xmlPath) + "未开通", "");
        }
        if (DT.FormatDate(model.ExTime, 0) == "1990-01-01 00:00:00")
        {
            Utils.Error("" + ub.GetSub("GroupName", xmlPath) + "属永不过期类型，不用续费", "");
        }
        Master.Title = "续费" + model.Title;
        builder.Append(Out.Tab("<div class=\"title\">续费" + ub.GetSub("GroupName", xmlPath) + "</div>", ""));
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok" || info == "acok")
        {
            //管理安全提示
            string[] p_pageArr = { "act", "Day", "Types", "id", "info", "backurl" };
            BCW.User.Role.AdminSafePage(meid, Utils.getPageUrl(), p_pageArr, "post");

            int Day = int.Parse(Utils.GetRequest("Day", "post", 2, @"^[0-6]$", "选择时长错误"));
            int Types = int.Parse(Utils.GetRequest("Types", "post", 2, @"^[0-1]$", "选择续费类型错误"));
            DateTime ExTime = model.ExTime;
            int Cent = 0;
            int iCent = Convert.ToInt32(ub.GetSub("GroupiPrice", xmlPath));//每天多少币
            if (Day == 0)
            {
                Cent = iCent * 5;
                ExTime = ExTime.AddDays(5);
            }
            else if (Day == 1)
            {
                Cent = iCent * 10;
                ExTime = ExTime.AddDays(10);
            }
            else if (Day == 2)
            {
                Cent = iCent * 15;
                ExTime = ExTime.AddDays(15);
            }
            else if (Day == 3)
            {
                Cent = iCent * 30;
                ExTime = ExTime.AddDays(30);
            }
            else if (Day == 4)
            {
                Cent = iCent * 90;
                ExTime = ExTime.AddDays(90);
            }
            else if (Day == 5)
            {
                Cent = iCent * 180;
                ExTime = ExTime.AddDays(180);
            }
            else if (Day == 6)
            {
                Cent = iCent * 360;
                ExTime = ExTime.AddDays(360);
            }
            //消费币
            long payCent = Convert.ToInt64(Cent);
            if (info != "acok")
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("续费需" + payCent + "" + ub.Get("SiteBz") + ",确定要续费吗");
                builder.Append(Out.Tab("</div>", "<br />"));

                strName = "Day,Types,id,act,info,backurl";
                strValu = "" + Day + "'" + Types + "'" + id + "'adminpay'acok'" + Utils.getPage(0) + "";
                strOthe = "确认续费,group.aspx,post,0,red";
                builder.Append(Out.wapform(strName, strValu, strOthe));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append(" <a href=\"" + Utils.getUrl("group.aspx?act=adminpay&amp;id=" + id + "") + "\">取消</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                if (Types == 1)
                {
                    long Gold = new BCW.BLL.User().GetGold(meid);
                    if (Gold < payCent)
                    {
                        Utils.Error("您的" + ub.Get("SiteBz") + "不足" + payCent + "" + ub.Get("SiteBz") + "", "");
                    }
                    //操作币
                    new BCW.BLL.User().UpdateiGold(meid, -payCent, "续费" + ub.GetSub("GroupName", xmlPath) + "");
                }
                else
                {
                    if (model.iCent < payCent)
                    {
                        Utils.Error("库存基金不足" + payCent + "" + ub.Get("SiteBz") + "，请使用自带" + ub.Get("SiteBz") + "进行续费", "");
                    }
                    BCW.Model.Grouplog addmodel = new BCW.Model.Grouplog();
                    addmodel.GroupId = id;
                    addmodel.Types = 2;
                    addmodel.UsID = meid;
                    addmodel.UsName = new BCW.BLL.User().GetUsName(meid);
                    addmodel.Content = "" + ub.GetSub("GroupName", xmlPath) + "续费";
                    addmodel.PayCent = payCent;
                    addmodel.AddTime = DateTime.Now;
                    new BCW.BLL.Grouplog().Add(addmodel);
                    new BCW.BLL.Group().UpdateiCent(id, -payCent);

                }
                new BCW.BLL.Group().UpdateExitTime(id, ExTime);

                Utils.Success("续费" + ub.GetSub("GroupName", xmlPath) + "", "恭喜，续费" + ub.GetSub("GroupName", xmlPath) + "成功，正在返回..", Utils.getUrl("group.aspx?act=info&amp;id=" + id + ""), "1");

            }
        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("选择续费时长:");
            builder.Append(Out.Tab("</div>", ""));
            strText = ",在哪里续费:/,,,,";
            strName = "Day,Types,id,act,info,backurl";
            strType = "select,select,hidden,hidden,hidden,hidden";
            strValu = "0'0'" + id + "'adminpay'ok'" + Utils.getPage(0) + "";
            strEmpt = "0|5天|1|10天|2|半个月|3|一个月|4|三个月|5|半年|6|一年,0|" + ub.GetSub("GroupName", xmlPath) + "基金|1|自身帐户,false,false,false,false";
            strIdea = "/";
            strOthe = "确定续费,group.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(" <a href=\"" + Utils.getUrl("group.aspx?act=admin&amp;id=" + id + "") + "\">取消</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }
    private void SDFundGet()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.Group model = new BCW.BLL.Group().GetGroup(id);
        if (model == null)
        {
            Utils.Error("不存在的" + ub.GetSub("GroupName", xmlPath) + "", "");
        }
        else if (model.Status == 1)
        {
            Utils.Error("" + ub.GetSub("GroupName", xmlPath) + "未开通", "");
        }
        else if (DT.FormatDate(model.ExTime, 0) != "1990-01-01 00:00:00" && model.ExTime < DateTime.Now)
        {
            Utils.Error("" + ub.GetSub("GroupName", xmlPath) + "已过期", "");
        }
         string info = Utils.GetRequest("info", "all", 1, "", "");
         string XmlsetID = ub.GetSub(id+"setID", xmlPath);
         if (info == "ok")
         {
             string temp = Utils.GetRequest("setID", "all", 1, @"^[^\^]{0,1000}$", "");//派币附言
             //if(!string.IsNullOrEmpty(temp))
             //string setID = Utils.GetRequest("setID", "post", 2, @"^([0-9]+[#]?){0,500}$", "设置取款权限ID填写错误，格式如1234#1111#2222,不能有空格
             string setID = string.Empty ;
             string setID1 = Utils.GetRequest("setID", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "多个ID请用#分隔，可以留空");
             if (!string.IsNullOrEmpty(setID1))
             {
                 setID = Utils.GetRequest("setID", "post", 2, @"^([0-9]+[#]?){1,500}$", "设置取款权限ID填写错误，格式如1234#1111#2222,不能有空格");      
                 int pageIndex;
                 int recordCount;
                 int pageSize = 20;
                 string strWhere = "";
                 string[] pageValUrl = { "act", "backurl" };
                 pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                 if (pageIndex == 0)
                     pageIndex = 1;
                 //查询条件
                 strWhere = "ForumID=" + model.ForumId + " and (OverTime>='" + DateTime.Now + "' OR OverTime='1990-1-1 00:00:00') and Status=0";

                 // 开始读取列表
                 IList<BCW.Model.Role> listRole = new BCW.BLL.Role().GetRoles(pageIndex, pageSize, strWhere, out recordCount);
                 if (setID.Contains("#"))
                 {
                     string[] sid = setID.Split('#');
                     if (listRole.Count > 0)
                     {
                         for (int i = 0; i < sid.Length; i++)
                         {
                             int flag = 0;  //标识符
                             foreach (BCW.Model.Role n in listRole)
                             {
                                 if ((sid[i] == n.UsID.ToString()))
                                 {
                                     flag = 1;//是版主
                                 }
                             }
                             if (flag == 0)
                             {
                                 Utils.Error("ID:" + sid[i] + "还不是版主，不能设置取款权限", "");
                             }

                         }
                     }
                     else
                     {
                         Utils.Error("还没有版主，欢迎申请..", "");
                     }
                 }
                 else
                 {
                     if (listRole.Count > 0)
                     {
                         int flag = 0;
                         foreach (BCW.Model.Role n in listRole)
                         {
                             if ((setID == n.UsID.ToString()))
                             {
                                 flag = 1;//是版主                       
                             }
                         }
                         if (flag == 0)
                         {
                             Utils.Error("ID:" + setID + "还不是版主，不能设置取款权限", "");
                         }
                     }
                     else
                     {
                         Utils.Error("还没有版主，欢迎申请..", "");
                     }
                 }

             }
             ub xml = new ub();
             Application.Remove(xmlPath);//清缓存
             xml.ReloadSub(xmlPath); //加载配置
             if (!string.IsNullOrEmpty(setID1))


             {
                 xml.dss[id + "setID"] = setID;
             }
             else
             {
                 xml.dss[id + "setID"] = setID1;
             }
             System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
             Utils.Success("设置成功", "设置取款权限成功！，正在返回..", Utils.getUrl("group.aspx?act=fund&amp;id="+id), "3");

         }
         else
         {

             Master.Title = "【" + model.Title + "】取款权限";

             builder.Append(Out.Tab("<div class=\"title\">" + model.Title + "取款权限</div>", ""));
             builder.Append(Out.Tab("<div>", ""));
             builder.Append("圈主为最大权限，可不填写！");
             builder.Append(Out.Tab("</div>", ""));
             strText = "设置取款权限ID（多个用#隔开）:/,,,,";
             strName = "setID,id,info,act,backurl";
             strType = "textarea,hidden,hidden,hidden,hidden";
             if (string.IsNullOrEmpty(XmlsetID))
             {
                 strValu = "'" + id + "'ok'sdfundget'" + Utils.getPage(0) + "";
             }
             else
             {
                 strValu = XmlsetID + "'" + id + "'ok'sdfundget'" + Utils.getPage(0) + "";
             }
             strEmpt = "false,false,false,false,flase";
             strIdea = "/";
             strOthe = "确定设置,group.aspx,post,1,red";
             builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
             builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
             builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
             builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=fund&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">基金</a>-");
             builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=info&amp;id=" + id + "") + "\">" + ub.GetSub("GroupName", xmlPath) + "</a>");
             builder.Append(Out.Tab("</div>", ""));
         }

    }
    private void FundPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.Group model = new BCW.BLL.Group().GetGroup(id);
        if (model == null)
        {
            Utils.Error("不存在的" + ub.GetSub("GroupName", xmlPath) + "", "");
        }
        else if (model.Status == 1)
        {
            Utils.Error("" + ub.GetSub("GroupName", xmlPath) + "未开通", "");
        }
        else if (DT.FormatDate(model.ExTime, 0) != "1990-01-01 00:00:00" && model.ExTime < DateTime.Now)
        {
            Utils.Error("" + ub.GetSub("GroupName", xmlPath) + "已过期", "");
        }
        Master.Title = "【" + model.Title + "】基金";
        builder.Append(Out.Tab("<div class=\"title\">" + model.Title + "基金</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("金库:" + model.iCent + "" + ub.Get("SiteBz") + "");
        //权限
        if (model.UsID == meid)//圈主最大权限
        {
            builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=fundget&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[取款]</a>");
            //builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=fundpwd&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[改密]</a>");
            builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=sdfundget&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[设置取款权限ID]</a>");
        }
        else//其他圈主设置可以取款的ID
        {
            string XmlsetID = ub.GetSub(id + "setID", xmlPath);
            if (XmlsetID.Contains("#"))
            {
                string[] sid = XmlsetID.Split('#');
                for (int i = 0; i < sid.Length; i++)
                {
                    if (sid[i] == meid.ToString())
                    {
                        builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=fundget&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[取款]</a>");
                    }
                }
            }
            if (XmlsetID == meid.ToString())
            {
                builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=fundget&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[取款]</a>");
            }
        }

        builder.Append("<br /><a href=\"" + Utils.getUrl("group.aspx?act=fundpay&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">我要捐款</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=fundlist&amp;ptype=2&amp;&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">支出明细</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("【最新捐款】<a href=\"" + Utils.getUrl("group.aspx?act=fundlist&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">光荣榜</a>");
        builder.Append(Out.Tab("</div>", ""));

        DataSet ds = new BCW.BLL.Grouplog().GetList("TOP 10 UsID,UsName,PayCent,AddTime", "Types=1 and GroupId=" + id + " ORDER BY PayCent DESC");
        if (ds != null && ds.Tables[0].Rows.Count != 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + ds.Tables[0].Rows[i]["UsID"].ToString() + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + ds.Tables[0].Rows[i]["UsName"].ToString() + "</a>捐了" + ds.Tables[0].Rows[i]["PayCent"].ToString() + "" + ub.Get("SiteBz") + "(" + DT.FormatDate(DateTime.Parse(ds.Tables[0].Rows[i]["AddTime"].ToString()), 4) + ")");
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
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=info&amp;id=" + id + "") + "\">我的" + ub.GetSub("GroupName", xmlPath) + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void FundListPage()
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.Group model = new BCW.BLL.Group().GetGroup(id);
        if (model == null)
        {
            Utils.Error("不存在的" + ub.GetSub("GroupName", xmlPath) + "", "");
        }
        else if (model.Status == 1)
        {
            Utils.Error("" + ub.GetSub("GroupName", xmlPath) + "未开通", "");
        }
        else if (DT.FormatDate(model.ExTime, 0) != "1990-01-01 00:00:00" && model.ExTime < DateTime.Now)
        {
            Utils.Error("" + ub.GetSub("GroupName", xmlPath) + "已过期", "");
        }
        string Title = string.Empty;
        if (ptype == 1)
            Title = "光荣榜";
        else
            Title = "支出明细";

        Master.Title = "【" + model.Title + "】" + Title + "";
        builder.Append(Out.Tab("<div class=\"title\">" + model.Title + "" + Title + "</div>", ""));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "ptype", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        strWhere = "Types=" + ptype + " and GroupId=" + id + "";

        // 开始读取列表
        IList<BCW.Model.Grouplog> listGrouplog = new BCW.BLL.Grouplog().GetGrouplogs(pageIndex, pageSize, strWhere, out recordCount);
        if (listGrouplog.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Grouplog n in listGrouplog)
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
                    builder.Append(Out.SysUBB("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + n.UsName + "(" + n.UsID + ")</a>取出" + n.PayCent + ub.Get("SiteBz") + Why + "[" + DT.FormatDate(n.AddTime, 5) + "]"));
                   // builder.AppendFormat("<a href=\"" + Utils.getUrl("uinfo.aspx?uid={0}&amp;backurl=" + Utils.getPage(0) + "") + "\">{1}({0})</a>取出{2}" + ub.Get("SiteBz") + "{3}[{4}]", n.UsID, n.UsName, n.PayCent, Why, DT.FormatDate(n.AddTime, 5));
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
        builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=fund&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">基金</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=info&amp;id=" + id + "") + "\">" + ub.GetSub("GroupName", xmlPath) + "</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void FundPayPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.Group model = new BCW.BLL.Group().GetGroup(id);
        if (model == null)
        {
            Utils.Error("不存在的" + ub.GetSub("GroupName", xmlPath) + "", "");
        }
        else if (model.Status == 1)
        {
            Utils.Error("" + ub.GetSub("GroupName", xmlPath) + "未开通", "");
        }
        else if (DT.FormatDate(model.ExTime, 0) != "1990-01-01 00:00:00" && model.ExTime < DateTime.Now)
        {
            Utils.Error("" + ub.GetSub("GroupName", xmlPath) + "已过期", "");
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
            BCW.Model.Grouplog addmodel = new BCW.Model.Grouplog();
            addmodel.GroupId = id;
            addmodel.Types = 1;
            addmodel.UsID = meid;
            addmodel.UsName = new BCW.BLL.User().GetUsName(meid);
            addmodel.Content = "";
            addmodel.PayCent = payCent;
            addmodel.AddTime = DateTime.Now;
            new BCW.BLL.Grouplog().Add(addmodel);
            new BCW.BLL.Group().UpdateiCent(id, payCent);
            //操作币
            new BCW.BLL.User().UpdateiGold(meid, -payCent, "" + ub.GetSub("GroupName", xmlPath) + "捐款");
            Utils.Success("捐款", "捐款成功..", Utils.getUrl("group.aspx?act=fund&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
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
            strName = "payCent,Pwd,id,info,act,backurl";
            strType = "num,password,hidden,hidden,hidden,hidden";
            strValu = "''" + id + "'ok'fundpay'" + Utils.getPage(0) + "";
            strEmpt = "false,false,false,false,false,flase";
            strIdea = "/";
            strOthe = "确定捐款,group.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=fund&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">基金</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=info&amp;id=" + id + "") + "\">" + ub.GetSub("GroupName", xmlPath) + "</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private void FundGetPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.Group model = new BCW.BLL.Group().GetGroup(id);
        if (model == null)
        {
            Utils.Error("不存在的" + ub.GetSub("GroupName", xmlPath) + "", "");
        }
        else if (model.Status == 1)
        {
            Utils.Error("" + ub.GetSub("GroupName", xmlPath) + "未开通", "");
        }
        else if (DT.FormatDate(model.ExTime, 0) != "1990-01-01 00:00:00" && model.ExTime < DateTime.Now)
        {
            Utils.Error("" + ub.GetSub("GroupName", xmlPath) + "已过期", "");
        }
        string XmlsetID = ub.GetSub(id + "setID", xmlPath);
        if (model.UsID != meid)
        {
            int temp = 0;
            if (XmlsetID.Contains("#"))
            {
                string[] sid = XmlsetID.Split('#');
                for (int i = 0; i < sid.Length; i++)
                {
                    if (sid[i] == meid.ToString())
                    {
                        temp = 1;
                    }
                }
            }
            if (XmlsetID == meid.ToString())
            {
                temp = 1;
            }
            if (temp == 0)
            {
                Utils.Error("你的权限不足", "");
            }
        }
        string paypwd = new BCW.BLL.User().GetUsPled(meid);
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {


            //管理安全提示
            string[] p_pageArr = { "act", "payCent", "Pwd", "id", "info", "backurl", "Why"};
            BCW.User.Role.AdminSafePage(meid, Utils.getPageUrl(), p_pageArr, "post");

            long payCent = Convert.ToInt64(Utils.GetRequest("payCent", "post", 2, @"^[1-9]\d*$", "请正确填写取款数目"));       
            string Why = Utils.GetRequest("Why", "post", 2, @"^[\s\S]{1,50}$", "原因限1-50字内");
            string Pwd = Utils.GetRequest("Pwd", "post", 2, @"^[\s\S]{1,20}$", "请正确填写支付密码");
           
            if (!Utils.MD5Str(Pwd).Equals(paypwd))
            {
                Utils.Error("支付密码不正确", "");
            }
            //if (!Pwd.Equals(model.iCentPwd))
            //{
            //    Utils.Error("基金密码不正确", "");
            //}
            if (model.iCent < payCent)
            {
                Utils.Error("" + ub.GetSub("GroupName", xmlPath) + "基金不足" + payCent + "" + ub.Get("SiteBz") + "", "");
            }
            BCW.Model.Grouplog addmodel = new BCW.Model.Grouplog();
            addmodel.GroupId = id;
            addmodel.Types = 2;
            addmodel.UsID = meid;
            addmodel.UsName = new BCW.BLL.User().GetUsName(meid);
            addmodel.Content = Why;
            addmodel.PayCent = payCent;
            addmodel.AddTime = DateTime.Now;
            new BCW.BLL.Grouplog().Add(addmodel);
            new BCW.BLL.Group().UpdateiCent(id, -payCent);
            //操作币
            new BCW.BLL.User().UpdateiGold(meid, payCent, "" + ub.GetSub("GroupName", xmlPath) + "取款");
            Utils.Success("取款", "取款成功..", Utils.getUrl("group.aspx?act=fund&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            if (string.IsNullOrEmpty(model.iCentPwd))
            {
                Utils.Error("你还没有设置基金密码呢<br /><a href=\"" + Utils.getUrl("group.aspx?act=fundpwd&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;马上设置</a>", "");
            }
            Master.Title = "【" + model.Title + "】取款";
            builder.Append(Out.Tab("<div class=\"title\">" + model.Title + "取款</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("金库:" + model.iCent + "" + ub.Get("SiteBz") + "<br />");
            builder.Append("您自带:" + new BCW.BLL.User().GetGold(meid) + "" + ub.Get("SiteBz") + "");
            builder.Append(Out.Tab("</div>", ""));
            strText = "" + ub.Get("SiteBz") + "数目:/,取款原因（50字内）:/,支付密码:/,,,,";
            strName = "payCent,Why,Pwd,id,info,act,backurl";
            strType = "num,text,password,hidden,hidden,hidden,hidden";
            strValu = "'''" + id + "'ok'fundget'" + Utils.getPage(0) + "";
            strEmpt = "false,false,false,false,false,false,flase";
            strIdea = "/";
            strOthe = "确定取出,group.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=fund&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">基金</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=info&amp;id=" + id + "") + "\">" + ub.GetSub("GroupName", xmlPath) + "</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private void FundPwdPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.Group model = new BCW.BLL.Group().GetGroup(id);
        if (model == null)
        {
            Utils.Error("不存在的" + ub.GetSub("GroupName", xmlPath) + "", "");
        }
        else if (model.Status == 1)
        {
            Utils.Error("" + ub.GetSub("GroupName", xmlPath) + "未开通", "");
        }
        else if (DT.FormatDate(model.ExTime, 0) != "1990-01-01 00:00:00" && model.ExTime < DateTime.Now)
        {
            Utils.Error("" + ub.GetSub("GroupName", xmlPath) + "已过期", "");
        }
        if (model.UsID != meid)
        {
            Utils.Error("你的权限不足", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {
            //管理安全提示
            string[] p_pageArr = { "act", "oPwd", "nPwd", "rPwd", "id", "info", "backurl" };
            BCW.User.Role.AdminSafePage(meid, Utils.getPageUrl(), p_pageArr, "post");

            string oPwd = Utils.GetRequest("oPwd", "post", 3, @"^[A-Za-z0-9]{6,20}$", "原密码限6-20位,必须由字母或数字组成");
            string nPwd = Utils.GetRequest("nPwd", "post", 2, @"^[A-Za-z0-9]{6,20}$", "新密码限6-20位,必须由字母或数字组成");
            string rPwd = Utils.GetRequest("rPwd", "post", 2, @"^[A-Za-z0-9]{6,20}$", "确认密码限6-20位,必须由字母或数字组成");
            if (!nPwd.Equals(rPwd))
            {
                Utils.Error("新密码与确认密码不相符", "");
            }

            string ordPwd = model.iCentPwd;
            if (!string.IsNullOrEmpty(ordPwd))
            {
                if (!oPwd.Equals(ordPwd))
                {
                    Utils.Error("原基金密码不正确", "");
                }
            }
            new BCW.BLL.Group().UpdateiCentPwd(id, nPwd);
            Utils.Success("修改基金密码", "恭喜，修改基金密码成功，正在返回..", Utils.getUrl("group.aspx?act=fund&amp;id=" + id + ""), "2");
        }
        else
        {
            Master.Title = "修改基金密码";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("修改基金密码");
            builder.Append(Out.Tab("</div>", ""));
            if (string.IsNullOrEmpty(model.iCentPwd))
            {
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("您是首次设置基金密码:");
                builder.Append(Out.Tab("</div>", ""));
                strText = "基金密码:/,确认密码:/,,,,";
                strName = "nPwd,rPwd,id,act,info,backurl";
                strType = "password,password,hidden,hidden,hidden,hidden";
                strValu = "''" + id + "'fundpwd'ok'" + Utils.getPage(0) + "";
                strEmpt = "false,false,false,false,false,false";
            }
            else
            {
                strText = "原密码:/,新密码:/,确认密码:/,,,,";
                strName = "oPwd,nPwd,rPwd,id,act,info,backurl";
                strType = "password,password,password,hidden,hidden,hidden,hidden";
                strValu = "'''" + id + "'fundpwd'ok'" + Utils.getPage(0) + "";
                strEmpt = "false,false,false,false,false,false,false";
            }
            strIdea = "/";
            strOthe = "确定设置,group.aspx,post,1,red";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=fund&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">基金</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=info&amp;id=" + id + "") + "\">" + ub.GetSub("GroupName", xmlPath) + "</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private void AddInPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.Group model = new BCW.BLL.Group().GetGroup(id);
        if (model == null)
        {
            Utils.Error("不存在的" + ub.GetSub("GroupName", xmlPath) + "", "");
        }
        else if (model.Status == 1)
        {
            Utils.Error("" + ub.GetSub("GroupName", xmlPath) + "未开通", "");
        }
        else if (DT.FormatDate(model.ExTime, 0) != "1990-01-01 00:00:00" && model.ExTime < DateTime.Now)
        {
            Utils.Error("" + ub.GetSub("GroupName", xmlPath) + "已过期", "");
        }
        Master.Title = "加入" + ub.GetSub("GroupName", xmlPath) + "";
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {
            string GroupId = new BCW.BLL.User().GetGroupId(meid);
            if (GroupId.IndexOf("$" + id + "$") != -1)
            {
                Utils.Error("您已申请加入本" + ub.GetSub("GroupName", xmlPath) + "了，请等待" + ub.GetSub("GroupName", xmlPath) + "主确认或发消息给" + ub.GetSub("GroupName", xmlPath) + "主..", "");
            }
            else if (GroupId != "")
            {
                if (GroupId.IndexOf("#" + id + "#") != -1)
                {
                    Utils.Error("您已申请加入本" + ub.GetSub("GroupName", xmlPath) + "了，请不要重复加入", "");
                }
                int JiaNum = 5;
                int VipLeven = BCW.User.Users.VipLeven(meid);
                if (VipLeven > 0)
                    JiaNum = 20;

                int DqNum = (Utils.GetStringNum(GroupId.Replace("##", "#"), "#") - 1);
                if (DqNum >= JiaNum)
                {
                    Utils.Error("加入" + ub.GetSub("GroupName", xmlPath) + "数量达上限，普通会员5个/VIP会员20个<br /><a href=\"" + Utils.getUrl("finance.aspx?act=addvip&amp;backurl=" + Utils.PostPage(1) + "") + "\">&gt;&gt;马上开通VIP</a>", "");
                }
            }
            if (model.InType == 2)
            {
                Utils.Error("" + ub.GetSub("GroupName", xmlPath) + "主已设置本" + ub.GetSub("GroupName", xmlPath) + "禁止加入成员了", "");
            }
            else if (model.InType == 0)
            {
                new BCW.BLL.User().UpdateGroupId(meid, "" + GroupId + "#" + id + "#");
                new BCW.BLL.Guest().Add(model.UsID, new BCW.BLL.User().GetUsName(model.UsID), "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + new BCW.BLL.User().GetUsName(meid) + "[/url]加入了您的" + ub.GetSub("GroupName", xmlPath) + "[url=/bbs/group.aspx?act=info&amp;id=" + id + "]" + model.Title + "[/url]");
                //更新成员人数
                new BCW.BLL.Group().UpdateiTotal(id, 1);
                Utils.Success("加入" + ub.GetSub("GroupName", xmlPath) + "", "恭喜，加入" + ub.GetSub("GroupName", xmlPath) + "“" + model.Title + "”成功，正在返回..", Utils.getUrl("group.aspx?act=info&amp;id=" + id + ""), "2");
            }
            else if (model.InType == 1)
            {
                new BCW.BLL.User().UpdateGroupId(meid, "" + GroupId + "$" + id + "$");//作标识
                new BCW.BLL.Guest().Add(model.UsID, new BCW.BLL.User().GetUsName(model.UsID), "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + new BCW.BLL.User().GetUsName(meid) + "[/url]申请加入了您的" + ub.GetSub("GroupName", xmlPath) + "[url=/bbs/group.aspx?act=info&amp;id=" + id + "]" + model.Title + "[/url][br][url=/bbs/group.aspx?act=addinok&amp;id=" + id + "&amp;uid=" + meid + "&amp;ptype=1]同意[/url].[url=/bbs/group.aspx?act=addinok&amp;id=" + id + "&amp;uid=" + meid + "&amp;ptype=2]拒绝Ta[/url]");
                Utils.Success("加入" + ub.GetSub("GroupName", xmlPath) + "", "恭喜，申请加入" + ub.GetSub("GroupName", xmlPath) + "“" + model.Title + "”成功，请等待" + ub.GetSub("GroupName", xmlPath) + "主验证通过..", Utils.getUrl("group.aspx?act=info&amp;id=" + id + ""), "2");
            }
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定加入" + ub.GetSub("GroupName", xmlPath) + "“" + model.Title + "”吗");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("group.aspx?info=ok&amp;act=addin&amp;id=" + id + "") + "\">确定加入</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=info&amp;id=" + id + "") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private void AddOutPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        int uid = int.Parse(Utils.GetRequest("uid", "get", 2, @"^[1-9]\d*$", "会员ID错误"));
        BCW.Model.Group model = new BCW.BLL.Group().GetGroup(id);
        if (model == null)
        {
            Utils.Error("不存在的" + ub.GetSub("GroupName", xmlPath) + "", "");
        }
        else if (model.Status == 1)
        {
            Utils.Error("" + ub.GetSub("GroupName", xmlPath) + "未开通", "");
        }
        else if (DT.FormatDate(model.ExTime, 0) != "1990-01-01 00:00:00" && model.ExTime < DateTime.Now)
        {
            Utils.Error("" + ub.GetSub("GroupName", xmlPath) + "已过期", "");
        }
        if (model.UsID != meid)
        {
            Utils.Error("你的权限不足", "");
        }
        Master.Title = "踢出" + ub.GetSub("GroupName", xmlPath) + "";
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {
            //管理安全提示
            string[] p_pageArr = { "act", "uid", "id", "info", "backurl" };
            BCW.User.Role.AdminSafePage(meid, Utils.getPageUrl(), p_pageArr, "get");

            string GroupId = new BCW.BLL.User().GetGroupId(uid);
            if (GroupId.IndexOf("#" + id + "#") == -1)
            {
                Utils.Error("不存在的成员记录..", "");
            }
            new BCW.BLL.User().UpdateGroupId(uid, GroupId.Replace("#" + id + "#", ""));
            //更新成员人数
            new BCW.BLL.Group().UpdateiTotal(id, -1);
            new BCW.BLL.Guest().Add(uid, new BCW.BLL.User().GetUsName(uid), "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + new BCW.BLL.User().GetUsName(meid) + "[/url]将您踢出" + ub.GetSub("GroupName", xmlPath) + "[url=/bbs/group.aspx?act=info&amp;id=" + id + "]" + model.Title + "[/url]");
            Utils.Success("踢出成员", "恭喜，踢出成员成功，正在返回..", Utils.getPage("group.aspx?act=info&amp;id=" + id + ""), "2");
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定将ID:" + uid + "中踢出" + ub.GetSub("GroupName", xmlPath) + "“" + model.Title + "”吗");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("group.aspx?info=ok&amp;act=addout&amp;id=" + id + "&amp;uid=" + uid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定踢出</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("group.aspx?act=info&amp;id=" + id + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }


    private void AddInOkPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 2, @"^[1-3]\d*$", "类型错误"));
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        BCW.Model.Group model = new BCW.BLL.Group().GetGroup(id);
        if (model == null)
        {
            Utils.Error("不存在的" + ub.GetSub("GroupName", xmlPath) + "", "");
        }
        else if (model.Status == 1)
        {
            Utils.Error("" + ub.GetSub("GroupName", xmlPath) + "未开通", "");
        }
        else if (DT.FormatDate(model.ExTime, 0) != "1990-01-01 00:00:00" && model.ExTime < DateTime.Now)
        {
            Utils.Error("" + ub.GetSub("GroupName", xmlPath) + "已过期", "");
        }
        if (ptype == 3)
        {
            string GroupId = new BCW.BLL.User().GetGroupId(meid);
            if (GroupId.IndexOf("$" + id + "$") == -1)
            {
                Utils.Error("不存在的申请记录..", "");
            }
            new BCW.BLL.User().UpdateGroupId(meid, GroupId.Replace("$" + id + "$", ""));
            Utils.Success("撤销加入", "恭喜，撤销加入" + ub.GetSub("GroupName", xmlPath) + "成功，正在返回..", Utils.getPage("group.aspx"), "2");
        }
        else
        {
            //管理安全提示
            string[] p_pageArr = { "act", "uid", "id", "ptype", "backurl" };
            BCW.User.Role.AdminSafePage(meid, Utils.getPageUrl(), p_pageArr, "post");

            if (!new BCW.BLL.User().Exists(uid))
            {
                Utils.Error("不存在的会员ID", "");
            }
            if (model.UsID != meid)
            {
                Utils.Error("你的权限不足", "");
            }
            string GroupId = new BCW.BLL.User().GetGroupId(uid);
            if (GroupId.IndexOf("$" + id + "$") == -1)
            {
                Utils.Error("不存在的申请记录..", "");
            }
            if (ptype == 1)
            {
                new BCW.BLL.User().UpdateGroupId(uid, GroupId.Replace("$" + id + "$", "#" + id + "#"));
                //更新成员人数
                new BCW.BLL.Group().UpdateiTotal(id, 1);
                new BCW.BLL.Guest().Add(uid, new BCW.BLL.User().GetUsName(uid), "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + new BCW.BLL.User().GetUsName(meid) + "[/url]已同意您加入了" + ub.GetSub("GroupName", xmlPath) + "[url=/bbs/group.aspx?act=info&amp;id=" + id + "]" + model.Title + "[/url]");
                Utils.Success("同意加入", "恭喜，同意成员加入" + ub.GetSub("GroupName", xmlPath) + "成功，正在返回..", Utils.getPage("group.aspx?act=info&amp;id=" + id + ""), "2");
            }
            else if (ptype == 2)
            {
                new BCW.BLL.User().UpdateGroupId(uid, GroupId.Replace("$" + id + "$", ""));
                new BCW.BLL.Guest().Add(uid, new BCW.BLL.User().GetUsName(uid), "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + new BCW.BLL.User().GetUsName(meid) + "[/url]拒绝您加入" + ub.GetSub("GroupName", xmlPath) + "[url=/bbs/group.aspx?act=info&amp;id=" + id + "]" + model.Title + "[/url]");
                Utils.Success("拒绝加入", "恭喜，拒绝成员加入" + ub.GetSub("GroupName", xmlPath) + "成功，正在返回..", Utils.getPage("group.aspx?act=info&amp;id=" + id + ""), "2");
            }
        }

    }

    private void GroupIdPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.Group model = new BCW.BLL.Group().GetGroup(id);
        if (model == null)
        {
            Utils.Error("不存在的" + ub.GetSub("GroupName", xmlPath) + "", "");
        }
        else if (model.Status == 1)
        {
            Utils.Error("" + ub.GetSub("GroupName", xmlPath) + "未开通", "");
        }
        else if (DT.FormatDate(model.ExTime, 0) != "1990-01-01 00:00:00" && model.ExTime < DateTime.Now)
        {
            Utils.Error("" + ub.GetSub("GroupName", xmlPath) + "已过期", "");
        }
        Master.Title = "成员列表";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(model.Title + "成员列表");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        strWhere += "GroupId LIKE '%#" + id + "#%'";
        // 开始读取列表
        IList<BCW.Model.User> listUser = new BCW.BLL.User().GetUsers(pageIndex, pageSize, strWhere, out recordCount);
        if (listUser.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.User n in listUser)
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
                builder.AppendFormat("<a href=\"" + Utils.getUrl("uinfo.aspx?uid={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{1}.{2}({0})</a>", n.ID, ((pageIndex - 1) * pageSize + k), n.UsName);
                if (model.UsID == meid)
                {
                    builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=addout&amp;id=" + id + "&amp;uid=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[踢]</a>");
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
        builder.Append("<a href=\"" + Utils.getPage("group.aspx?act=info&amp;id=" + id + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("group.aspx") + "\">" + ub.GetSub("GroupName", xmlPath) + "首页</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void AdminPassPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.Group model = new BCW.BLL.Group().GetGroup(id);
        if (model == null)
        {
            Utils.Error("不存在的" + ub.GetSub("GroupName", xmlPath) + "", "");
        }
        else if (model.Status == 1)
        {
            Utils.Error("" + ub.GetSub("GroupName", xmlPath) + "未开通", "");
        }
        else if (DT.FormatDate(model.ExTime, 0) != "1990-01-01 00:00:00" && model.ExTime < DateTime.Now)
        {
            Utils.Error("" + ub.GetSub("GroupName", xmlPath) + "已过期", "");
        }
        if (model.UsID != meid)
        {
            Utils.Error("你的权限不足", "");
        }
        Master.Title = "审核成员";
        builder.Append(Out.Tab("<div class=\"title\">审核成员</div>", ""));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        strWhere += "GroupId LIKE '%$" + id + "$%'";
        // 开始读取列表
        IList<BCW.Model.User> listUser = new BCW.BLL.User().GetUsers(pageIndex, pageSize, strWhere, out recordCount);
        if (listUser.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.User n in listUser)
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
                builder.AppendFormat("<a href=\"" + Utils.getUrl("uinfo.aspx?uid={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{1}.{2}({0})</a>", n.ID, ((pageIndex - 1) * pageSize + k), n.UsName);
                builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=addinok&amp;id=" + id + "&amp;uid=" + n.ID + "&amp;ptype=1&amp;backurl=" + Utils.PostPage(1) + "") + "\">[同意]</a>");
                builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=addinok&amp;id=" + id + "&amp;uid=" + n.ID + "&amp;ptype=2&amp;backurl=" + Utils.PostPage(1) + "") + "\">[拒绝]</a>");
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
        builder.Append("<a href=\"" + Utils.getPage("group.aspx?act=info&amp;id=" + id + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("group.aspx") + "\">" + ub.GetSub("GroupName", xmlPath) + "首页</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void MeGroupPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Master.Title = "我的" + ub.GetSub("GroupName", xmlPath) + "";
        builder.Append(Out.Tab("<div class=\"title\">我的" + ub.GetSub("GroupName", xmlPath) + "</div>", ""));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("<b>创建的" + ub.GetSub("GroupName", xmlPath) + "</b>");
        builder.Append(Out.Tab("</div>", "<br />"));
        //我的" + ub.GetSub("GroupName", xmlPath) + "
        builder.Append(Out.Tab("<div>", ""));
        DataSet ds = new BCW.BLL.Group().GetList("ID,Title,iTotal", "UsID=" + meid + " and Status=0");
        if (ds != null && ds.Tables[0].Rows.Count != 0)
        {
            int id = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
            string Title = ds.Tables[0].Rows[0]["Title"].ToString();
            int iTotal = int.Parse(ds.Tables[0].Rows[0]["iTotal"].ToString());
            builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=info&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + Title + "(" + iTotal + "人)</a>");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=add") + "\">&gt;&gt;马上创建</a>");
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("<b>加入的" + ub.GetSub("GroupName", xmlPath) + "</b>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        string strGroupId = new BCW.BLL.User().GetGroupId(meid);
        string GroupId = strGroupId.Replace("$", "#");
        //GroupId = Regex.Replace(GroupId, @"$(\d*)$", "");
        if (GroupId == "" || !GroupId.Contains("#"))
        {
            builder.Append("没有相关记录..<br />");
        }
        else
        {
            GroupId = Utils.DelLastChar(GroupId, "#");
            GroupId = Utils.Mid(GroupId, 1, GroupId.Length);
            string[] sName = Regex.Split(GroupId, "##");
            int pageIndex;
            int recordCount;
            int pageSize = 15;
            string[] pageValUrl = { "act", "backurl" };
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
                    builder.Append("" + (i + 1) + ".<a href=\"" + Utils.getUrl("group.aspx?act=info&amp;id=" + sName[i] + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.Group().GetTitle(Convert.ToInt32(sName[i])) + "</a>");
                    if (strGroupId.IndexOf("$" + sName[i] + "$") != -1)
                    {
                        builder.Append("(未通过)<a href=\"" + Utils.getUrl("group.aspx?act=addinok&amp;id=" + sName[i] + "&amp;ptype=3&amp;backurl=" + Utils.PostPage(1) + "") + "\">[撤销]</a><br />");
                    }
                    else
                    {
                        builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=exit&amp;id=" + sName[i] + "") + "\">[退" + ub.GetSub("GroupName", xmlPath) + "]</a><br />");
                    }
                }
                if (k == endIndex)
                    break;
                k++;
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 1));
        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.RHr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("group.aspx") + "\">" + ub.GetSub("GroupName", xmlPath) + "首页</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ExitPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.Group model = new BCW.BLL.Group().GetGroup(id);
        if (model == null)
        {
            Utils.Error("不存在的" + ub.GetSub("GroupName", xmlPath) + "", "");
        }
        Master.Title = "退出" + ub.GetSub("GroupName", xmlPath) + "";
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {
            string GroupId = new BCW.BLL.User().GetGroupId(meid);
            if (GroupId.IndexOf("#" + id + "#") == -1)
            {
                Utils.Error("不存在的加入记录", "");
            }
            new BCW.BLL.User().UpdateGroupId(meid, GroupId.Replace("#" + id + "#", ""));
            //更新成员人数
            new BCW.BLL.Group().UpdateiTotal(id, -1);
            new BCW.BLL.Guest().Add(model.UsID, new BCW.BLL.User().GetUsName(model.UsID), "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + new BCW.BLL.User().GetUsName(meid) + "[/url]退出您的" + ub.GetSub("GroupName", xmlPath) + "[url=/bbs/group.aspx?act=info&amp;id=" + id + "]" + model.Title + "[/url]");
            Utils.Success("退出" + ub.GetSub("GroupName", xmlPath) + "", "恭喜，退出" + ub.GetSub("GroupName", xmlPath) + "成功..", Utils.getUrl("group.aspx?act=me"), "2");
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定退出" + ub.GetSub("GroupName", xmlPath) + "“" + model.Title + "”吗");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("group.aspx?info=ok&amp;act=exit&amp;id=" + id + "") + "\">确定退出</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=me") + "\">再看看吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private void SignPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.Group model = new BCW.BLL.Group().GetGroup(id);
        if (model == null)
        {
            Utils.Error("不存在的" + ub.GetSub("GroupName", xmlPath) + "", "");
        }
        else if (model.Status == 1)
        {
            Utils.Error("" + ub.GetSub("GroupName", xmlPath) + "未开通", "");
        }
        else if (DT.FormatDate(model.ExTime, 0) != "1990-01-01 00:00:00" && model.ExTime < DateTime.Now)
        {
            Utils.Error("" + ub.GetSub("GroupName", xmlPath) + "已过期", "");
        }
        string GroupId = new BCW.BLL.User().GetGroupId(meid);
        if (GroupId.IndexOf("#" + id + "#") == -1)
        {
            Utils.Error("你不是本" + ub.GetSub("GroupName", xmlPath) + "成员..", "");
        }
        //不同一天则清空签到ID
        string SignID = model.SignID;
        if (string.IsNullOrEmpty(model.SignTime.ToString()) || model.SignTime.ToShortDateString() != DateTime.Now.ToShortDateString())
        {
            new BCW.BLL.Group().UpdateSignID(id, "");
            SignID = string.Empty;
        }

        if (SignID.IndexOf("#" + meid + "#") != -1)
        {
            Utils.Error("你今天已签到过了", "");
        }
        long payCent = Convert.ToInt64(model.SignCent);
        if (payCent > model.iCent)
        {
            Utils.Error("基金库存不足，签到没币了..", "");
        }
        //操作币
        new BCW.BLL.User().UpdateiGold(meid, payCent, "" + ub.GetSub("GroupName", xmlPath) + "签到");
        new BCW.BLL.Group().UpdateiCent(id, -payCent);
        new BCW.BLL.Group().UpdateSignID(id, SignID + "#" + meid + "#");
        Utils.Success("签到成功", "恭喜，今天签到成功，奖" + payCent + "" + ub.Get("SiteBz") + "，正在返回..", Utils.getPage("group.aspx?act=info&amp;id=" + id + ""), "1");
    }

    private void AddPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        if (new BCW.BLL.Group().ExistsUsID(meid))
        {
            Utils.Error("你已经申请" + ub.GetSub("GroupName", xmlPath) + "..", "");
        }
        int leven = new BCW.BLL.User().GetLeven(meid);
        int AddLeven = Convert.ToInt32(ub.GetSub("GroupAddLeven", xmlPath));
        if (leven < AddLeven)
        {
            Utils.Error("等级不够" + AddLeven + "级，不能申请创建" + ub.GetSub("GroupName", xmlPath) + "", "");
        }

        Master.Title = "申请创建" + ub.GetSub("GroupName", xmlPath) + "";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("申请创建" + ub.GetSub("GroupName", xmlPath) + "");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "" + ub.GetSub("GroupName", xmlPath) + "名称:/," + ub.GetSub("GroupName", xmlPath) + "类型:/,所属城市(如广州):/,创建原因(800字内):/,,";
        string strName = "Title,Types,City,Content,act,backurl";
        string strType = "text,select,text,textarea,hidden,hidden";
        string strValu = "'1'''addsave'" + Utils.getPage(0) + "";
        string strEmpt = "false,1|地区|2|社会|3|打工|4|情感|5|生活|6|娱乐|7|音乐|8|彩票|9|军事|10|时尚|11|数码|12|体育|13|家族|14|文学|15|购物|16|汽车|17|财经|18|动漫|19|校园|20|其它,false,false,false,false";
        string strIdea = "/";
        string strOthe = "确定申请|reset,group.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("group.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=help&amp;backurl=" + Utils.PostPage(1) + "") + "\">查看帮助</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void AddSavePage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        BCW.User.Users.ShowVerifyRole("i", meid);//非验证会员提示
        if (new BCW.BLL.Group().ExistsUsID(meid))
        {
            Utils.Error("你已经申请" + ub.GetSub("GroupName", xmlPath) + "..", "");
        }
        int leven = new BCW.BLL.User().GetLeven(meid);
        int AddLeven = Convert.ToInt32(ub.GetSub("GroupAddLeven", xmlPath));
        if (leven < AddLeven)
        {
            Utils.Error("等级不够" + AddLeven + "级，不能申请创建" + ub.GetSub("GroupName", xmlPath) + "", "");
        }
        string Title = Utils.GetRequest("Title", "post", 2, @"^[^\^]{1,30}$", "" + ub.GetSub("GroupName", xmlPath) + "名称限30字内");
        int Types = int.Parse(Utils.GetRequest("Types", "post", 2, @"^[1-9]\d*$", "" + ub.GetSub("GroupName", xmlPath) + "类型选择错误"));
        string City = Utils.GetRequest("City", "post", 2, @"^[^\^]{2,8}$", "城市限2-8字");
        string Content = Utils.GetRequest("Content", "post", 2, @"^[^\^]{1,800}$", "创建原因限1-800字内");
        string allCity = "#" + BCW.User.City.GetCity() + "#";
        if (allCity.IndexOf("#" + City + "#") == -1)
        {
            Utils.Error("不存在的城市名称", "");
        }
        long payCent = Convert.ToInt64(ub.GetSub("GroupAddPrice", xmlPath));
        long gold = new BCW.BLL.User().GetGold(meid);
        if (gold < payCent)
        {
            Utils.Error("创建" + ub.GetSub("GroupName", xmlPath) + "需" + payCent + "" + ub.Get("SiteBz") + "，你的" + ub.Get("SiteBz") + "不足", "");
        }
        BCW.Model.Group model = new BCW.Model.Group();
        model.Title = Title;
        model.Types = Types;
        model.City = City;
        model.Logo = "";
        model.Notes = "";
        model.Content = Content;
        model.UsID = meid;
        model.InType = 0;
        model.Status = 1;
        model.ExTime = DateTime.Now;
        model.AddTime = DateTime.Now;
        new BCW.BLL.Group().Add(model);
        //操作币
        new BCW.BLL.User().UpdateiGold(meid, -payCent, "申请创建" + ub.GetSub("GroupName", xmlPath) + "");

        //通知管理员审核
        string GuestID = "" + ub.GetSub("GroupGuestID", xmlPath) + "";
        if (GuestID != "")
        {
            string[] gTemp = GuestID.Split("#".ToCharArray());
            for (int i = 0; i < gTemp.Length; i++)
            {
                int guestid = Convert.ToInt32(gTemp[i]);

                new BCW.BLL.Guest().Add(guestid, "管理员", "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + new BCW.BLL.User().GetUsName(meid) + "[/url]申请创建" + ub.GetSub("GroupName", xmlPath) + "，请到后台处理审核。");
            }
        }

        Utils.Success("申请创建" + ub.GetSub("GroupName", xmlPath) + "", "申请创建" + ub.GetSub("GroupName", xmlPath) + "成功，请等待管理员审核..", Utils.getUrl("group.aspx"), "2");
    }

    private void HelpPage()
    {
        Master.Title = "" + ub.GetSub("GroupName", xmlPath) + "帮助";
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-2]$", "0"));

        if (ptype == 0)
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("" + ub.GetSub("GroupName", xmlPath) + "帮助");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("想拥有一个属于自己的" + ub.GetSub("GroupName", xmlPath) + "吗？创建后可以申请自己的论坛和聊天室，还可以把您的亲人朋友邀请到您的" + ub.GetSub("GroupName", xmlPath) + "交流，并且自己会有最高管理权限哦！");
            builder.Append(Out.Tab("</div>", ""));
        }
        else if (ptype == 1)
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("创建" + ub.GetSub("GroupName", xmlPath) + "帮助");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("1.每人只能申请一个" + ub.GetSub("GroupName", xmlPath) + ".请珍惜创建" + ub.GetSub("GroupName", xmlPath) + "的机会.<br />");
            builder.Append("2.请认真填写，创建后，名称、类别、城市都不能修改!<br />");
            builder.Append("3.为方便大家更好的记得您的" + ub.GetSub("GroupName", xmlPath) + "," + ub.GetSub("GroupName", xmlPath) + "名称请勿使用特殊字符.<br />");
            builder.Append("4.填写精彩的" + ub.GetSub("GroupName", xmlPath) + "宣言,更能展示您" + ub.GetSub("GroupName", xmlPath) + "的个性和主题!<br />");
            builder.Append("5.创建理由:请详细描述您申请" + ub.GetSub("GroupName", xmlPath) + "的理由,可以包括对本" + ub.GetSub("GroupName", xmlPath) + "主题的解释和您建设该" + ub.GetSub("GroupName", xmlPath) + "的发展计划等等(这项很重要哦,以便管理员斟酌批准申请).");
            builder.Append(Out.Tab("</div>", ""));

        }
        else if (ptype == 2)
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("" + ub.GetSub("GroupName", xmlPath) + "费用");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("1.每位会员只能创建一个" + ub.GetSub("GroupName", xmlPath) + "，并且申请后系统会收取" + ub.GetSub("GroupAddPrice", xmlPath) + "" + ub.Get("SiteBz") + "开" + ub.GetSub("GroupName", xmlPath) + "费<br />");
            builder.Append("2.使用" + ub.GetSub("GroupName", xmlPath) + "过程中必须支付一定" + ub.Get("SiteBz") + "，系统每天收取" + ub.GetSub("GroupiPrice", xmlPath) + "" + ub.Get("SiteBz") + "，请" + ub.GetSub("GroupName", xmlPath) + "主注意续费，否则" + ub.GetSub("GroupName", xmlPath) + "将被删除");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=help&amp;ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;创建条件</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("group.aspx?act=help&amp;ptype=2&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;" + ub.GetSub("GroupName", xmlPath) + "费用</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("group.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("group.aspx") + "\">" + ub.GetSub("GroupName", xmlPath) + "首页</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void InPage(string act)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.Group model = new BCW.BLL.Group().GetGroup(id);
        if (model == null)
        {
            Utils.Error("不存在的" + ub.GetSub("GroupName", xmlPath) + "", "");
        }
        else if (model.Status == 1)
        {
            Utils.Error("" + ub.GetSub("GroupName", xmlPath) + "未开通", "");
        }
        else if (DT.FormatDate(model.ExTime, 0) != "1990-01-01 00:00:00" && model.ExTime < DateTime.Now)
        {
            Utils.Error("" + ub.GetSub("GroupName", xmlPath) + "已过期", "");
        }
        if (act == "inforum")
        {
            if (model.ForumId > 0)
                Utils.Success("正在进入" + ub.GetSub("GroupName", xmlPath) + "坛", "正在进入" + ub.GetSub("GroupName", xmlPath) + "坛..", Utils.getUrl("forum.aspx?forumid=" + model.ForumId + ""), "1");

            Master.Title = "" + ub.GetSub("GroupName", xmlPath) + "论坛";
            builder.Append(Out.Tab("<div class=\"title\">" + ub.GetSub("GroupName", xmlPath) + "论坛</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            int PelNum = Convert.ToInt32(ub.GetSub("GroupPelNum", xmlPath));
            if (model.iTotal >= PelNum)
            {
                builder.Append("本" + ub.GetSub("GroupName", xmlPath) + "成员已达" + PelNum + "人，如果您是" + ub.GetSub("GroupName", xmlPath) + "主，可以联系管理员开通论坛..");
            }
            else
            {
                builder.Append("本" + ub.GetSub("GroupName", xmlPath) + "成员未达到" + PelNum + "人，还不能开通论坛..");
            }
            builder.Append(Out.Tab("</div>", ""));
        }
        //else
        //{
        //    if (model.ChatId > 0)
        //        Utils.Success("正在进入" + ub.GetSub("GroupName", xmlPath) + "聊", "正在进入" + ub.GetSub("GroupName", xmlPath) + "聊..", Utils.getUrl("chatroom.aspx?id=" + model.ChatId + ""), "1");

        //    Master.Title = "" + ub.GetSub("GroupName", xmlPath) + "聊室";
        //    builder.Append(Out.Tab("<div class=\"title\">" + ub.GetSub("GroupName", xmlPath) + "聊室</div>", ""));
        //    builder.Append(Out.Tab("<div>", ""));
        //    int PelNum = Convert.ToInt32(ub.GetSub("GroupPel2Num", xmlPath));
        //    if (model.iTotal >= PelNum)
        //    {
        //        builder.Append("本" + ub.GetSub("GroupName", xmlPath) + "成员已达" + PelNum + "人，如果您是" + ub.GetSub("GroupName", xmlPath) + "主，可以联系管理员开通" + ub.GetSub("GroupName", xmlPath) + "聊..");
        //    }
        //    else
        //    {
        //        builder.Append("本" + ub.GetSub("GroupName", xmlPath) + "成员未达到" + PelNum + "人，还不能开通" + ub.GetSub("GroupName", xmlPath) + "聊..");
        //    }
        //    builder.Append(Out.Tab("</div>", ""));
        //}
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("group.aspx?act=info&amp;id=" + id + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("group.aspx") + "\">" + ub.GetSub("GroupName", xmlPath) + "首页</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    /// <summary>
    /// 穿透圈子限制ID
    /// </summary>
    private bool IsCTID(int meid)
    {
        bool Isvi = false;
        //能够穿透的ID
        string CTID = "#" + ub.GetSub("GroupCTID", xmlPath) + "#";
        if (CTID.IndexOf("#" + meid + "#") != -1)
        {
            Isvi = true;
        }

        return Isvi;
    }
}