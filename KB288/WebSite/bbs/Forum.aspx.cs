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
/// 
/// 修改人陈志基 2016 0421
/// 修改红包为GIF
/// </summary>
public partial class bbs_Forum : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected System.Text.StringBuilder builderIndex = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        //维护提示
        if (ub.Get("SiteforumStatus") == "1")
        {
            Utils.Safe("论坛系统");
        }
        else if (ub.Get("SiteforumStatus") == "2")
        {
            int meid = new BCW.User.Users().GetUsId();
            if (meid == 0)
                Utils.Login();//显示登录
        }
        int forumid = int.Parse(Utils.GetRequest("forumid", "all", 1, @"^[0-9]\d*$", "0"));
        string act = Utils.GetRequest("act", "all", 1, "", "");
        if (forumid != 0)
            act = "list";

        switch (act)
        {
            case "list":
                ListPage(forumid);
                break;
            default:
                ReloadPage();
                break;
        }
    }

    /// <summary>
    /// 论坛首页
    /// </summary>
    private void ReloadPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[0-9]\d*$", "0"));
        string strWhere = string.Empty;
        int meid = new BCW.User.Users().GetUsId();
        if (id != 0)
        {
            if (!new BCW.BLL.Topics().ExistsIdLeibie(id, 2))
            {
                Utils.Error("不存在的记录", "");
            }
            if (meid == 0)
                strWhere = "NodeId=" + id + " AND Hidden=0 ORDER BY Paixu ASC";
            else
                strWhere = "NodeId=" + id + " AND Hidden<=1 ORDER BY Paixu ASC";

            BCW.Model.Topics model = new BCW.BLL.Topics().GetTopics(id);
            Master.Title = model.Title;

            //----------------业务处理开始
            bool IsTs = false;
            if (model.Cent != 0)
            {

                if (model.SellTypes == 0)//按次收费
                {
                    string payIDs = "|" + model.PayId + "|";
                    if (payIDs.IndexOf("|" + meid + "|") == -1)
                    {
                        IsTs = true;
                    }
                }

                else if (model.SellTypes == 1 || model.SellTypes == 2)//包周包月
                {
                    if (!new BCW.BLL.Order().Exists(id, meid, DateTime.Now))
                    {
                        IsTs = true;
                    }
                }
            }
            if (model.VipLeven != 0)
            {
                if (meid == 0)
                    Utils.Login();//显示登录

                int VipLeven = BCW.User.Users.VipLeven(meid);
                if (VipLeven < model.VipLeven)
                {
                    Utils.Error("本页面限VIP等级" + model.VipLeven + "级进入<br />您的VIP等级为<a href=\"" + Utils.getUrl("/bbs/finance.aspx?act=addvip") + "\">" + VipLeven + "级</a>", "");
                }
            }
            if (IsTs == true)
            {
                if (meid == 0)
                    Utils.Login();//显示登录

                long Cent = Convert.ToInt64(model.Cent);
                //取用户信息
                string Bz = string.Empty;
                long megold = 0;
                if (model.BzType == 0)
                {
                    megold = new BCW.BLL.User().GetGold(meid);
                    Bz = ub.Get("SiteBz");
                }
                else
                {
                    megold = new BCW.BLL.User().GetMoney(meid);
                    Bz = ub.Get("SiteBz2");
                }
                string act = Utils.GetRequest("act", "get", 1, "", "");
                if (act != "ok")
                {
                    new Out().head(Utils.ForWordType("温馨提示"));
                    Response.Write(Out.Tab("<div class=\"text\">", ""));
                    if (model.SellTypes == 0)
                    {
                        Response.Write("本页内容收费" + Cent + "" + Bz + "，扣费一次，永久浏览");

                    }
                    else if (model.SellTypes == 1)
                    {
                        Response.Write("本页内容为包周业务，,如果您是此内容的包周会员，可以免费进入，否则将扣除您的" + Cent + "" + Bz + "，您将免费浏览本页面为一周时间");
                    }
                    else if (model.SellTypes == 2)
                    {
                        Response.Write("本页内容为包月业务，,如果您是此内容的包月会员，可以免费进入，否则将扣除您的" + Cent + "" + Bz + "，您将免费浏览本页面为一个月时间");
                    }
                    Response.Write(Out.Tab("</div>", "<br />"));
                    Response.Write(Out.Tab("<div>", ""));
                    Response.Write("您自带" + megold + "" + Bz + "<a href=\"" + Utils.getUrl("/bbs/finance.aspx?act=vippay") + "\">[充值]</a><br />");
                    Response.Write("<a href=\"" + Utils.getUrl("forum.aspx?act=ok&amp;id=" + id + "") + "\">马上进入浏览</a><br />");
                    Response.Write("<a href=\"" + Utils.getUrl("forum.aspx") + "\">返回上级</a>");
                    Response.Write(Out.Tab("</div>", ""));
                    Response.Write(new Out().foot());
                    Response.End();
                }

                //支付安全提示
                string[] p_pageArr = { "act", "id" };
                BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr, "get");

                if (model.SellTypes == 0)//按次收费
                {
                    string payIDs = "|" + model.PayId + "|";
                    if (payIDs.IndexOf("|" + meid + "|") == -1)
                    {
                        if (megold < Cent)
                        {
                            Utils.Error("您的" + Bz + "不足", Utils.getUrl("default.aspx?id=" + id + ""));
                        }
                        //扣币
                        if (model.BzType == 0)
                            new BCW.BLL.User().UpdateiGold(meid, -Cent, "浏览收费页面");
                        else
                            new BCW.BLL.User().UpdateiMoney(meid, -Cent, "浏览收费页面");

                        //更新
                        payIDs = model.PayId + "|" + meid;
                        new BCW.BLL.Topics().UpdatePayId(id, payIDs);
                    }
                }

                else if (model.SellTypes == 1 || model.SellTypes == 2)//包周包月
                {
                    int iDays = 0;
                    if (model.SellTypes == 1)
                        iDays = 7;
                    else
                        iDays = 30;

                    if (!new BCW.BLL.Order().Exists(id, meid, DateTime.Now))
                    {
                        if (megold < Cent)
                        {
                            Utils.Error("您的" + Bz + "不足", "");
                        }
                        //扣币
                        if (model.BzType == 0)
                            new BCW.BLL.User().UpdateiGold(meid, -Cent, "浏览收费页面");
                        else
                            new BCW.BLL.User().UpdateiMoney(meid, -Cent, "浏览收费页面");

                        //增加/更新
                        BCW.Model.Order modelorder = new BCW.Model.Order();
                        modelorder.UsId = meid;
                        modelorder.UsName = new BCW.BLL.User().GetUsName(meid);
                        modelorder.TopicId = id;
                        modelorder.SellTypes = model.SellTypes;
                        modelorder.Title = model.Title;
                        modelorder.AddTime = DateTime.Now;
                        modelorder.ExTime = DateTime.Now.AddDays(iDays);
                        if (!new BCW.BLL.Order().Exists(id, meid))
                        {
                            new BCW.BLL.Order().Add(modelorder);
                        }
                        else
                        {
                            new BCW.BLL.Order().Update(modelorder);
                        }
                    }
                }
            }
            //----------------业务处理结束

            //----------------密码访问开始
            string pwd = Utils.GetRequest("pwd", "post", 1, "", "");
            if (!string.IsNullOrEmpty(model.InPwd) && pwd != model.InPwd)
            {
                new Out().head(Utils.ForWordType("温馨提示"));
                Response.Write(Out.Tab("<div class=\"title\">", ""));
                Response.Write("本页面内容已加密");
                Response.Write(Out.Tab("</div>", ""));
                string strText = "输入密码:/,,";
                string strName = "pwd,id,backurl";
                string strType = "password,hidden,hidden,hidden";
                string strValu = "'" + id + "'" + Utils.getPage(0) + "";
                string strEmpt = "false,false,false";
                string strIdea = "/";
                string strOthe = "确认访问,forum.aspx,post,1,red";

                Response.Write(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                Response.Write(Out.Tab("<div>", ""));
                Response.Write(" <a href=\"" + Utils.getUrl("default.aspx") + "\">取消</a>");
                Response.Write(Out.Tab("</div>", ""));
                Response.Write(new Out().foot());
                Response.End();
            }

            //----------------密码访问结束

            //----------------限制手机访问开始
            if (model.IsPc == 1)
            {
                if (!Utils.IsMobileUa())
                {
                    Utils.Error("请使用手机访问本页", "");
                }
            }
            //----------------限制手机访问结束
            //builder.Append(Out.Tab("<div class=\"title\">" + model.Title + "</div>", ""));
            //全区滚动
            BCW.Model.Text flow = new BCW.BLL.Text().GetTextFlow();
            if (flow != null)
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getUrl("manager.aspx?act=flow&amp;forumid=" + flow.ForumId + "&amp;bid=" + flow.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + Out.USB(TrueStrLength.cutTrueLength(flow.Title, 20, "…")) + "</a> ");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
        }
        else
        {
            if (meid == 0)
                strWhere = "NodeId=0 AND Leibie=2 AND Hidden=0 ORDER BY Paixu ASC";
            else
                strWhere = "NodeId=0 AND Leibie=2 AND Hidden<=1 ORDER BY Paixu ASC";

            Master.Title = ub.Get("SiteforumName");
            string Logo = ub.Get("SiteforumLogo");
            if (!string.IsNullOrEmpty(Logo))
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<img src=\"" + Logo + "\" alt=\"load\"/>");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            //拾物随机
            builder.Append(BCW.User.Game.GiftFlows.ShowGiftFlows(1));
            //顶部滚动
            builder.Append(BCW.User.Master.OutTopRand(2));

            //全区滚动
            BCW.Model.Text flow = new BCW.BLL.Text().GetTextFlow();
            if (flow != null)
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getUrl("manager.aspx?act=flow&amp;forumid=" + flow.ForumId + "&amp;bid=" + flow.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + Out.USB(TrueStrLength.cutTrueLength(flow.Title, 20, "…")) + "</a> ");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
        }

        DataSet ds = new BCW.BLL.Topics().GetList(strWhere);
        if (ds == null || ds.Tables[0].Rows.Count == 0)
        {
            if (id != 0)
            {
                builder.Append(Out.Div("div", "没有相关记录.."));
            }
            else
            {
                builder.Append(Out.Div("div", "论坛正在建设中.."));
            }
        }
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            string ID = ds.Tables[0].Rows[i]["ID"].ToString();
            string Types = ds.Tables[0].Rows[i]["Types"].ToString();
            string Title = ds.Tables[0].Rows[i]["Title"].ToString();
            string Content = ds.Tables[0].Rows[i]["Content"].ToString();
            string IsBr = ds.Tables[0].Rows[i]["IsBr"].ToString();
            int VipLeven = Utils.ParseInt(ds.Tables[0].Rows[i]["VipLeven"].ToString());
            string Br = string.Empty;
            if (IsBr == "0" && IsVipSeen(meid, VipLeven) == true)
                Br = Convert.ToChar(10).ToString();

            switch (Types)
            {
                case "1":
                    builderIndex.Append("<a href=\"" + Utils.getUrl("forum.aspx?id=" + ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + Title + "</a>");
                    break;
                case "2":
                    if (IsVipSeen(meid, VipLeven))
                        builderIndex.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(Content)));
                    break;
                case "3":
                    if (IsVipSeen(meid, VipLeven))
                        builderIndex.Append("<img src=\"" + Content + "\" alt=\"load\"/>");
                    break;
                case "4":
                    if (IsVipSeen(meid, VipLeven))
                        builderIndex.Append("<a href=\"" + BCW.User.AdminCall.AdminUBB(Utils.SetUrl(Content)) + "\">" + Title + "</a>");
                    break;
                case "5":
                    if (IsVipSeen(meid, VipLeven))
                        builderIndex.Append(Out.WmlDecode(Content));
                    break;
                case "6":
                    builderIndex.Append("<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + Content + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + Title + "</a>");
                    break;
                case "10":
                    builderIndex.Append(BCW.User.AdminCall.ShowAdvert());
                    break;
                default:
                    builderIndex.Append("");
                    break;
            }

            builderIndex.Append(Br);
        }
        if (!Utils.Isie())
        {
            builder.Append(builderIndex.ToString().Replace((Convert.ToChar(10).ToString()), "<br />"));
        }
        else
        {
            string[] txtIndex = builderIndex.ToString().Split((Convert.ToChar(10).ToString()).ToCharArray());
            for (int i = 0; i < txtIndex.Length; i++)
            {
                // 输出列表的格式
                if (txtIndex[i].IndexOf("</div>") == -1)
                {
                    if ((i + 1) % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", ""));
                    else
                        builder.Append(Out.Tab("<div>", ""));
                }
                builder.Append(txtIndex[i].ToString());
                if (txtIndex[i].IndexOf("</div>") == -1)
                {
                    builder.Append(Out.Tab("</div>", ""));
                }
            }
        }

        if (id != 0)
        {
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            int NodeId = new BCW.BLL.Topics().GetNodeId(id);
            if (NodeId != 0)
            {
                builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?id=" + NodeId + "") + "\">返回栏目</a><br />");
            }
            if (Utils.getPage(1) != "")
            {
                builder.Append("<a href=\"" + Utils.getPage(1) + "\">&gt;返回上一级</a>");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("forum.aspx") + "\">&gt;返回上一级</a>");
            }
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    /// <summary>
    /// 论坛版面
    /// </summary>
    private void ListPage(int forumid)
    {
        if (!new BCW.BLL.Forum().Exists2(forumid))
        {
            Utils.Success("访问论坛", "该论坛不存在或已暂停使用", Utils.getUrl("forum.aspx"), "1");
        }
        int meid = new BCW.User.Users().GetUsId();
        BCW.Model.Forum model = new BCW.BLL.Forum().GetForum(forumid);
        BCW.Model.Group modelgr = null;
        if (model.GroupId > 0)
        {
            if (meid == 0)
                Utils.Login();

            modelgr = new BCW.BLL.Group().GetGroupMe(model.GroupId);
            if (modelgr == null)
            {
                Utils.Error("不存在的" + ub.GetSub("GroupName", "/Controls/group.xml") + "", "");
            }
            else if (DT.FormatDate(modelgr.ExTime, 0) != "1990-01-01 00:00:00" && modelgr.ExTime < DateTime.Now)
            {
                Utils.Error("" + ub.GetSub("GroupName", "/Controls/group.xml") + "已过期", "");
            }
            if (modelgr.ForumStatus == 2)
            {
                Utils.Error("" + ub.GetSub("GroupName", "/Controls/group.xml") + "论坛已关闭", "");
            }
            string GroupId = new BCW.BLL.User().GetGroupId(meid);
            if (modelgr.ForumStatus == 1)
            {
                bool Isvi = false;
                //能够穿透的ID
                string CTID = "#" + ub.GetSub("GroupCTID", "/Controls/group.xml") + "#";
                if (CTID.IndexOf("#" + meid + "#") != -1)
                {
                    Isvi = true;
                }
                if (GroupId.IndexOf("#" + model.GroupId + "#") == -1 && Isvi == false)
                {

                    Utils.Error("非成员不能访问" + ub.GetSub("GroupName", "/Controls/group.xml") + "论坛！<br /><a href=\"" + Utils.getUrl("/bbs/group.aspx?act=addin&amp;id=" + model.GroupId + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">加入本" + ub.GetSub("GroupName", "/Controls/group.xml") + "</a>", "");
                }
            }
            int VipLeven = BCW.User.Users.VipLeven(meid);
            if (VipLeven == 0)
            {
                int DqNum = (Utils.GetStringNum(GroupId.Replace("##", "#"), "#") - 1);
                if (DqNum > 5)
                {
                    Utils.Error("您的VIP会员已过期，请进行续费才能同时加入5个以上的" + ub.GetSub("GroupName", "/Controls/group.xml") + "，否则只能同时加入5个才可以使用" + ub.GetSub("GroupName", "/Controls/group.xml") + "<br /><a href=\"" + Utils.getUrl("finance.aspx?act=addvip&amp;backurl=" + Utils.PostPage(1) + "") + "\">&gt;&gt;我现在要续费VIP</a><br /><a href=\"" + Utils.getUrl("group.aspx?act=me&amp;backurl=" + Utils.PostPage(1) + "") + "\">&lt;&lt;我要退出一些圈子</a>", "");
                }
            }

        }
        BCW.User.Users.ShowForumLimit(meid, model.Gradelt, model.Visitlt, model.VisitId, model.IsPc);
        Master.Title = model.Title;
        //个性设置
        int FsPageSize = 10;
        if (meid > 0)
        {
            string ForumSet = new BCW.BLL.User().GetForumSet(meid);
            FsPageSize = BCW.User.Users.GetForumSet(ForumSet, 0);
        }
        int pageIndex;
        int recordCount;
        int pageSize = FsPageSize;
        string strWhere = string.Empty;
        string strOrder = string.Empty;
        string[] pageValUrl = { "forumid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        if (pageIndex == 1)
        {
            if (!string.IsNullOrEmpty(model.Logo))
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<img src=\"" + model.Logo + "\" alt=\"load\"/>");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            //拾物随机
            builder.Append(BCW.User.Game.GiftFlows.ShowGiftFlows(1));
            //顶部滚动
            builder.Append(BCW.User.Master.OutTopRand(3));

            if (!string.IsNullOrEmpty(model.TopUbb))
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(model.TopUbb)));
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            //全区滚动
            BCW.Model.Text flow = new BCW.BLL.Text().GetTextFlow();
            if (flow != null)
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getUrl("manager.aspx?act=flow&amp;forumid=" + flow.ForumId + "&amp;bid=" + flow.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + Out.USB(TrueStrLength.cutTrueLength(flow.Title, 20, "…")) + "</a> ");
                builder.Append(Out.Tab("</div>", "<br />"));
            }

            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("/bbs/addThread.aspx?forumid=" + forumid + "") + "\">发帖</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("/bbs/manwork.aspx?forumid=" + forumid + "") + "\">版务</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("/bbs/sktype.aspx?forumid=" + forumid + "") + "\">精华</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("/bbs/forumts.aspx?forumid=" + forumid + "") + "\">专题</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("/search.aspx?act=forum&amp;forumid=" + forumid + "") + "\">搜索</a> ");
            if (new BCW.User.ForumInc().IsForumGSIDS(forumid) == true)
            {
                builder.Append("<a href=\"" + Utils.getUrl("/bbs/Gstoplist.aspx?forumid=" + forumid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">高手</a>");
            }
            builder.Append(Out.Tab("</div>", ""));
            if (model.GroupId > 0)
            {
                builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("/bbs/group.aspx?act=info&amp;id=" + model.GroupId + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">主页</a> ");
                builder.Append("<a href=\"" + Utils.getUrl("/bbs/group.aspx?act=fund&amp;id=" + model.GroupId + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">基金</a> ");
                builder.Append("<a href=\"" + Utils.getUrl("/bbs/group.aspx?act=view&amp;id=" + model.GroupId + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">资料</a> ");
                builder.Append("<a href=\"" + Utils.getUrl("/bbs/group.aspx?act=groupid&amp;id=" + model.GroupId + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">成员列表</a> ");
                builder.Append(Out.Tab("</div>", ""));
            }
            //本版滚动
            flow = new BCW.BLL.Text().GetTextFlow(forumid);
            if (flow != null)
            {
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("manwork.aspx?act=flow&amp;forumid=" + forumid + "&amp;bid=" + flow.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + Out.USB(TrueStrLength.cutTrueLength(flow.Title, 20, "…")) + "</a> ");
                builder.Append(Out.Tab("</div>", ""));
            }
        }

        //查询条件
        //string strWhe = "=" + forumid + "";
        //是否显示下级版块帖子
        //if (model.ShowType == 1)
        //{
        //    if (!string.IsNullOrEmpty(model.DoNode))
        //    {
        //        strWhe = "in (" + forumid + "," + model.DoNode + ")";
        //    }
        //}
        //strWhere = "IsDel=0 and (ForumId " + strWhe + " OR IsTop=2)";

        //查询条件
        strWhere = "ForumId =" + forumid + " and IsDel=0 and IsTop=0";
        //排序条件
        strOrder = "ReTime Desc";//Istop Desc,
        // 开始读取列表
        IList<BCW.Model.Text> listText = new BCW.BLL.Text().GetTexts(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listText.Count > 0)
        {
            if (pageIndex != 1)
            {
                //算出总页数
                int pageTotal = BasePage.CalcPageCount(recordCount, pageSize, ref pageIndex);
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                if (pageTotal > pageIndex)
                {
                    builder.Append("<a href=\"" + Utils.getUrl("/bbs/forum.aspx?forumid=" + forumid + "&amp;page=" + (pageIndex + 1) + "&amp;backurl=" + Utils.getPage(0) + "") + "\">下一页</a>|");
                }

                builder.Append("<a href=\"" + Utils.getUrl("/bbs/forum.aspx?forumid=" + forumid + "&amp;page=1&amp;backurl=" + Utils.getPage(0) + "") + "\">返回首页</a> ");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                //全区置顶帖子
                int j = 1;
                DataSet dsTop = new BCW.BLL.Text().GetList("Top 3 ID,ForumId,Title,UsID,UsName,ReplyNum,ReadNum", "IsTop=2 and IsDel=0 ORDER BY NEWID()");
                if (dsTop != null && dsTop.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsTop.Tables[0].Rows.Count; i++)
                    {
                        if (j % 2 == 0)
                            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));

                        builder.AppendFormat("<i><b>[顶]</b></i><a href=\"" + Utils.getUrl("/bbs/topic.aspx?forumid={0}&amp;bid={1}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}", dsTop.Tables[0].Rows[i]["ForumId"].ToString(), dsTop.Tables[0].Rows[i]["ID"].ToString(), Out.TitleUBB(dsTop.Tables[0].Rows[i]["Title"].ToString()));
                        builder.Append("</a><br />" + dsTop.Tables[0].Rows[i]["UsName"].ToString());
                        builder.AppendFormat("/阅{0}/回<a href=\"" + Utils.getUrl("/bbs/reply.aspx?forumid={1}&amp;bid={2}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{3}</a>", dsTop.Tables[0].Rows[i]["ReadNum"].ToString(), dsTop.Tables[0].Rows[i]["ForumId"].ToString(), dsTop.Tables[0].Rows[i]["ID"].ToString(), dsTop.Tables[0].Rows[i]["ReplyNum"].ToString());
                        j++;
                        builder.Append(Out.Tab("</div>", ""));
                    }
                }

                //本坛置顶帖子
                j = 1;
                dsTop = new BCW.BLL.Text().GetList("Top 5 ID,ForumId,Title,UsID,UsName,ReplyNum,ReadNum", "ForumId=" + forumid + " and IsTop=1 and IsDel=0 ORDER BY ReTime Desc");
                if (dsTop != null && dsTop.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsTop.Tables[0].Rows.Count; i++)
                    {
                        if (j % 2 == 0)
                            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));

                        builder.AppendFormat("<b>[顶]</b><a href=\"" + Utils.getUrl("/bbs/topic.aspx?forumid={0}&amp;bid={1}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}", dsTop.Tables[0].Rows[i]["ForumId"].ToString(), dsTop.Tables[0].Rows[i]["ID"].ToString(), Out.TitleUBB(dsTop.Tables[0].Rows[i]["Title"].ToString()));
                        builder.Append("</a><br />" + dsTop.Tables[0].Rows[i]["UsName"].ToString());
                        builder.AppendFormat("/阅{0}/回<a href=\"" + Utils.getUrl("/bbs/reply.aspx?forumid={1}&amp;bid={2}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{3}</a>", dsTop.Tables[0].Rows[i]["ReadNum"].ToString(), dsTop.Tables[0].Rows[i]["ForumId"].ToString(), dsTop.Tables[0].Rows[i]["ID"].ToString(), dsTop.Tables[0].Rows[i]["ReplyNum"].ToString());
                        j++;
                        builder.Append(Out.Tab("</div>", ""));
                    }
                }
            }


            int k = 1;
            foreach (BCW.Model.Text n in listText)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                    builder.Append(Out.Tab("<div>", "<br />"));

                builder.Append(BCW.User.AppCase.CaseIsTop(n.IsTop));
                builder.Append(BCW.User.AppCase.CaseIsGood(n.IsGood));
                builder.Append(BCW.User.AppCase.CaseIsRecom(n.IsRecom));
                builder.Append(BCW.User.AppCase.CaseIsLock(n.IsLock));
                builder.Append(BCW.User.AppCase.CaseIsOver(n.IsOver));
                // builder.Append(BCW.User.AppCase.CaseText(n.Types)); //派币
                if (n.Types == 3)
                {
                    builder.Append("<img src=\"/files/face/money.gif\" width=\"20\" height=\"25\"  alt=\"load\"/>"); //派币
                }
                else
                {
                    builder.Append(BCW.User.AppCase.CaseText(n.Types)); //派币/附件
                }
                string TextTab = string.Empty;
                if (n.IsTop != 2)
                    TextTab = BCW.User.AppCase.CaseLabel(n.LabelId, model.Label);

                builder.AppendFormat("<a href=\"" + Utils.getUrl("/bbs/topic.aspx?forumid={0}&amp;bid={1}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}.{3}{4}", n.ForumId, n.ID, (pageIndex - 1) * pageSize + k, TextTab, n.Title);
                if (n.Types == 8)
                {
                    builder.Append("[" + n.Gaddnum + "中" + n.Gwinnum + "]");
                }
                builder.Append("</a><br />" + n.UsName);
                builder.AppendFormat("/阅{0}/回<a href=\"" + Utils.getUrl("/bbs/reply.aspx?forumid={1}&amp;bid={2}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{3}</a>", n.ReadNum, n.ForumId, n.ID, n.ReplyNum);

                k++;
                builder.Append(Out.Tab("</div>", ""));

            }

            // 分页
            builder.Append(BasePage.ForumMultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

        }
        else
        {
            builder.Append(Out.Tab("", "<br />"));
            builder.Append(Out.Div("div", "没有相关记录.."));
        }

        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/addThread.aspx?forumid=" + forumid + "") + "\">发帖</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/sktype.aspx?act=text&amp;forumid=" + forumid + "") + "\">我帖</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/sktype.aspx?forumid=" + forumid + "") + "\">模式</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/forumstat.aspx?act=top&amp;forumid=" + forumid + "") + "\">排行</a>");
        builder.Append(Out.Tab("</div>", ""));

        if (!string.IsNullOrEmpty(model.Notes))
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append(model.Notes);
            builder.Append(Out.Tab("</div>", ""));
        }
        //更新版块在线人数
        if (pageIndex == 1)
        {
            new BCW.BLL.User().UpdateEndForumID(meid, forumid);
            int ForumLine = new BCW.BLL.User().GetForumNum(forumid);
            if (ForumLine != model.Line)
                new BCW.BLL.Forum().UpdateLine(forumid, ForumLine);
        }
        builder.Append(Out.Tab("<div>", ""));
        if (ub.GetSub("BbsIsOnline", "/Controls/bbs.xml") == "0")
        {
            builder.Append(Out.Tab("", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("/bbs/online.aspx?forumid=" + forumid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">在线" + model.Line + "人.最高" + model.TopLine + "人</a>");
        }
        //下级版块
        builder.Append(BCW.User.Users.ShowForumNode(model.IsNode, forumid));
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        if (model.GroupId > 0)
            builder.Append("<a href=\"" + Utils.getUrl("/bbs/group.aspx?act=info&amp;id=" + model.GroupId + "") + "\">" + ub.GetSub("GroupName", "/Controls/group.xml") + "主页</a>");
        else
            builder.Append("<a href=\"" + Utils.getUrl("/bbs/forum.aspx?forumid=" + model.NodeId + "") + "\">上级论坛</a>");

        builder.Append(Out.Tab("</div>", ""));

        if (!string.IsNullOrEmpty(model.FootUbb))
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(model.FootUbb)));
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    /// <summary>
    /// VIP等级可见
    /// </summary>
    private bool IsVipSeen(int meid, int VipLeven)
    {
        bool bl = true;
        if (VipLeven != 0)
        {
            if (meid == 0)
            {
                bl = false;
            }
            else
            {
                int myVipLeven = BCW.User.Users.VipLeven(meid);
                if (myVipLeven < VipLeven)
                {
                    bl = false;
                }
            }
        }
        return bl;
    }
}