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
using TPR2.Common;

public partial class Manage_guess2_kzlist : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/guess.xml";
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");

        switch (act)
        {
            case "back":
                BackPage();
                break;
            case "backsave":
                BackSavePage();
                break;
            case "view":
                ViewPage();
                break;
            case "plist":
                PListPage();
                break;
            case "wlist":
                WListPage();
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
        Master.Title = "个人庄列表";
        int showtype = int.Parse(Utils.GetRequest("showtype", "all", 1, @"^[0-1]$", "0"));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[0-9]\d*$", "0"));
        builder.Append(Out.Tab("<div>", ""));
        if (uid == 0)
        {
            builder.Append("【全部个人庄】");
            builder.Append(Out.waplink(Utils.getUrl("kzbother.aspx"), "纠纷处理") + "");
        }
        else
        {
            builder.Append("【" + new BCW.BLL.User().GetUsName(uid) + "个人庄】<br />");

            if (showtype == 0)
                builder.Append("未过期|");
            else
                builder.Append(Out.waplink(Utils.getUrl("kzlist.aspx?uid=" + uid + "&amp;showtype=0"), "未过期") + "|");

            if (showtype == 1)
                builder.Append("已过期");
            else
                builder.Append(Out.waplink(Utils.getUrl("kzlist.aspx?uid=" + uid + "&amp;showtype=1"), "已过期") + "");
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));

        int pageIndex;
        int recordCount;
        string strDay = "";
        string strWhere = "";
        string[] pageValUrl = { "act", "uid", "showtype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        strWhere = "usid>0";

        if (uid > 0)
            strWhere += " and usid = " + uid + "";

        if (uid > 0)
        {
            if (showtype == 0)
                strWhere += " and p_TPRtime >= '" + System.DateTime.Now + "'";
            else
                strWhere += " and p_TPRtime < '" + System.DateTime.Now + "'";
        }
        string strOrder = "ID DESC";
        // 开始读取竞猜
        IList<TPR2.Model.guess.BaListMe> listBaListMe = new TPR2.BLL.guess.BaListMe().GetBaListMes(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listBaListMe.Count > 0)
        {
            int k = 1;
            foreach (TPR2.Model.guess.BaListMe n in listBaListMe)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div>", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }

                string IsNEW = "";
                if (n.p_ison == 1)
                    IsNEW = "走地";
                else if (n.p_ison == 2)
                    IsNEW = "固定水位";
                else
                    IsNEW = "自动水位";

                if (DT.GetTime(n.p_TPRtime.ToString(), "MM月dd日").ToString() != strDay.ToString())
                    builder.Append(DT.GetTime(n.p_TPRtime.ToString(), "MM月dd日") + "<br />");

                builder.AppendFormat("<a href=\"" + Utils.getUrl("kzlist.aspx?act=view&amp;gid={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{1}[{2}]{3}VS{4}[{5}]</a>", n.ID, DT.FormatDate(Convert.ToDateTime(n.p_TPRtime), 13), n.p_title, n.p_one, n.p_two, IsNEW);

                if (n.p_active > 0)
                {
                    if (n.p_active == 2)
                        builder.Append("(已平盘)");
                    else
                    {
                        if (n.p_result_one != null && n.p_result_two != null)
                            builder.Append("(比分:" + n.p_result_one + ":" + n.p_result_two + ")");
                    }
                }
                if (n.p_del == 1)
                    builder.Append("(已隐藏)");

                builder.Append(Out.Tab("</div>", ""));

                strDay = DT.GetTime(n.p_TPRtime.ToString(), "MM月dd日").ToString();
                k++;
            }

            // 分页

            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        string strText = "输入用户ID:/,,";
        string strName = "uid,showtype,backurl";
        string strType = "num,hidden,hidden";
        string strValu = "'" + showtype + "'" + Utils.getPage(0) + "";
        string strEmpt = "true,false,false";
        string strIdea = "/";
        string strOthe = "搜个人庄,kzlist.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        if (uid != 0)
        {
            builder.Append(Out.Tab("<div>", " "));
            builder.Append("<a href=\"" + Utils.getUrl("kzlist.aspx") + "\">返回</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

    }

    private void ViewPage()
    {
        int gid = Utils.ParseInt(Utils.GetRequest("gid", "get", 2, @"^[0-9]*$", "竞猜ID无效"));

        TPR2.BLL.guess.BaListMe bll = new TPR2.BLL.guess.BaListMe();


        TPR2.Model.guess.BaListMe model = bll.GetModel(gid);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }

        Master.Title = model.p_one + "VS" + model.p_two;

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("庄家:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(model.usid) + "(" + model.usid + ")</a>");

        builder.Append("<br /><a href=\"" + Utils.getUrl("default.aspx?showtype=1&amp;ptype=4&amp;fly=" + model.p_title + "") + "\">" + model.p_title + "</a>:" + model.p_one + "VS" + model.p_two + "");

        builder.Append("<br />开赛:" + DT.FormatDate(Convert.ToDateTime(model.p_TPRtime), 0));
        if (model.p_result_one != null && model.p_result_two != null)
        {
            builder.Append("<br />完场比分：" + model.p_result_one + ":" + model.p_result_two + "");
        }

        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("〓让球盘〓");
        builder.Append("<br />" + Out.waplink(Utils.getUrl("kzlist.aspx?act=pay&amp;gid=" + model.ID + "&amp;p=1"), model.p_one + "(" + Convert.ToDouble(model.p_one_lu) + ")"));


        builder.AppendFormat("{0}" + Out.waplink(Utils.getUrl("kzlist.aspx?act=plist&amp;gid=" + model.ID + "&amp;p=1"), "{1}注"), "|", new TPR2.BLL.guess.BaPayMe().GetCount(model.ID, Convert.ToInt32(model.p_type), 1));

        if (model.p_type == 1)
            builder.Append("<br />" + GCK.getZqPn(Convert.ToInt32(model.p_pn)) + "" + GCK.getPkName(Convert.ToInt32(model.p_pk)) + "");
        else
            builder.Append("<br />" + Convert.ToDouble(model.p_pk) + "");

        builder.Append("<br />" + Out.waplink(Utils.getUrl("kzlist.aspx?act=pay&amp;gid=" + model.ID + "&amp;p=2"), model.p_two + "(" + Convert.ToDouble(model.p_two_lu) + ")"));

        builder.AppendFormat("{0}" + Out.waplink(Utils.getUrl("kzlist.aspx?act=plist&amp;gid=" + model.ID + "&amp;p=2"), "{1}注"), "|", new TPR2.BLL.guess.BaPayMe().GetCount(model.ID, Convert.ToInt32(model.p_type), 2));

        if (model.p_big_lu != 0 && model.p_big_lu != -1 && model.p_dx_pk != 0)
        {
            builder.Append("<br />〓大小盘〓<br />");

            builder.AppendFormat("" + Out.waplink(Utils.getUrl("kzlist.aspx?act=plist&amp;gid=" + model.ID + "&amp;p=3"), "{0}注") + "{1}", new TPR2.BLL.guess.BaPayMe().GetCount(model.ID, Convert.ToInt32(model.p_type), 3), "|");

            builder.Append("" + Out.waplink(Utils.getUrl("kzlist.aspx?act=pay&amp;gid=" + model.ID + "&amp;p=3"), "大(" + Convert.ToDouble(model.p_big_lu) + ")"));

            if (model.p_type == 1)
                builder.Append(GCK.getDxPkName(Convert.ToInt32(model.p_dx_pk)));
            else
                builder.Append(Convert.ToDouble(model.p_dx_pk));

            builder.Append(Out.waplink(Utils.getUrl("kzlist.aspx?act=pay&amp;gid=" + model.ID + "&amp;p=4"), "小(" + Convert.ToDouble(model.p_small_lu) + ")"));

            builder.AppendFormat("{0}" + Out.waplink(Utils.getUrl("kzlist.aspx?act=plist&amp;gid=" + model.ID + "&amp;p=4"), "{1}注"), "|", new TPR2.BLL.guess.BaPayMe().GetCount(model.ID, Convert.ToInt32(model.p_type), 4));

        }
        if (ub.GetSub("SiteIsbz", "/Controls/guess.xml") != "1")
        {
            if (model.p_bzs_lu != 0 && model.p_bzs_lu != -1)
            {
                builder.Append("<br />〓标准盘〓");
                builder.Append("<br />" + Out.waplink(Utils.getUrl("kzlist.aspx?act=pay&amp;gid=" + model.ID + "&amp;p=5"), "主胜(" + Convert.ToDouble(model.p_bzs_lu) + ")"));

                builder.AppendFormat("{0}" + Out.waplink(Utils.getUrl("kzlist.aspx?act=plist&amp;gid=" + model.ID + "&amp;p=5"), "{1}注"), "|", new TPR2.BLL.guess.BaPayMe().GetCount(model.ID, Convert.ToInt32(model.p_type), 5));

                builder.Append("<br />" + Out.waplink(Utils.getUrl("kzlist.aspx?act=pay&amp;gid=" + model.ID + "&amp;p=6"), "平手(" + Convert.ToDouble(model.p_bzp_lu) + ")"));

                builder.AppendFormat("{0}" + Out.waplink(Utils.getUrl("kzlist.aspx?act=plist&amp;gid=" + model.ID + "&amp;p=6"), "{1}注"), "|", new TPR2.BLL.guess.BaPayMe().GetCount(model.ID, Convert.ToInt32(model.p_type), 6));

                builder.Append("<br />" + Out.waplink(Utils.getUrl("kzlist.aspx?act=pay&amp;gid=" + model.ID + "&amp;p=7"), "客胜(" + Convert.ToDouble(model.p_bzx_lu) + ")"));

                builder.AppendFormat("{0}" + Out.waplink(Utils.getUrl("kzlist.aspx?act=plist&amp;gid=" + model.ID + "&amp;p=7"), "{1}注"), "|", new TPR2.BLL.guess.BaPayMe().GetCount(model.ID, Convert.ToInt32(model.p_type), 7));

            }
        }
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("【" + Out.waplink(Utils.getUrl("kzguess2.aspx?gid=" + gid + ""), "编辑") + " ");
        builder.Append("" + Out.waplink(Utils.getUrl("kzlist.aspx?act=del&amp;gid=" + gid + ""), "删除") + " ");
        builder.Append("" + Out.waplink(Utils.getUrl("kzlist.aspx?act=wlist&amp;gid=" + gid + ""), "记录") + "】");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append(Out.waplink(Utils.getPage("kzlist.aspx"), "返回上一级") + "<br />");
        builder.Append(Out.waplink(Utils.getUrl("default.aspx"), "返回球彩管理") + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }


    private void PListPage()
    {

        int gid = Utils.ParseInt(Utils.GetRequest("gid", "all", 2, @"^[0-9]*$", "竞猜ID无效"));
        int p = Utils.ParseInt(Utils.GetRequest("p", "all", 2, @"^[1-7]*$", "选择无效"));

        TPR2.Model.guess.BaListMe model = new TPR2.BLL.guess.BaListMe().GetModel(gid);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }

        Master.Title = model.p_one + "VS" + model.p_two;

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(model.p_one + "VS" + model.p_two);
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (p == 1)
            builder.Append("让球盘:<b>主队</b>下注列表");
        else if (p == 2)
            builder.Append("让球盘:<b>客队</b>下注列表");
        else if (p == 3)
            builder.Append("大小盘:<b>大</b>下注列表");
        else if (p == 4)
            builder.Append("大小盘:<b>小</b>下注列表");
        else if (p == 5)
            builder.Append("标准盘:<b>主胜</b>下注列表");
        else if (p == 6)
            builder.Append("标准盘:<b>平手</b>下注列表");
        else if (p == 7)
            builder.Append("标准盘:<b>客胜</b>下注列表");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageSize = 10;
        int pageIndex;
        int recordCount;
        string[] pageValUrl = {"act", "gid", "p" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //组合条件
        string strWhere = "";
        strWhere += "bcid=" + gid + " and PayType=" + p + " ";

        // 开始读取竞猜
        IList<TPR2.Model.guess.BaPayMe> listBaPayMe = new TPR2.BLL.guess.BaPayMe().GetBaPayMes(pageIndex, pageSize, strWhere, out recordCount);
        if (listBaPayMe.Count > 0)
        {
            int k = 1;
            foreach (TPR2.Model.guess.BaPayMe n in listBaPayMe)
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

                builder.AppendFormat(Out.waplink(Utils.getUrl("../uinfo.aspx?uid={1}&amp;backurl=" + Utils.PostPage(1) + ""), "{0}({1})") + ":{2}[{3}]", n.payusname, n.payusid, Out.SysUBB(n.payview), n.paytimes);
                builder.Append(Out.waplink(Utils.getUrl("kzlist.aspx?act=back&amp;id=" + n.ID + "&amp;gid=" + gid + "&amp;backurl=" + Utils.PostPage(1) + ""), "[退]"));

                builder.Append(Out.Tab("</div>", ""));
                k++;
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append(Out.waplink(Utils.getPage("kzlist.aspx?act=view&amp;gid=" + gid + ""), "返回上一级") + "<br />");
        builder.Append(Out.waplink(Utils.getUrl("default.aspx"), "返回球彩管理") + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

    }

    private void WListPage()
    {

        int gid = Utils.ParseInt(Utils.GetRequest("gid", "all", 2, @"^[0-9]*$", "竞猜ID无效"));
        TPR2.Model.guess.BaListMe model = new TPR2.BLL.guess.BaListMe().GetModel(gid);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }

        Master.Title = model.p_one + "VS" + model.p_two + "-记录";

        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[0-9]\d*$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        if (ptype == 0)
        {

            builder.Append("下注记录|" + Out.waplink(Utils.getUrl("kzlist.aspx?act=wlist&amp;gid=" + gid + "&amp;ptype=1"), "赢输记录"));
        }
        else
        {
            builder.Append(Out.waplink(Utils.getUrl("kzlist.aspx?act=wlist&amp;gid=" + gid + "&amp;ptype=0"), "下注记录") + "|赢输记录");
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageSize = 10;
        int pageIndex;
        int recordCount;
        string[] pageValUrl = { "act", "gid", "ptype" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //组合条件
        string strWhere = "";
        strWhere += "bcid=" + gid + "";
        if (ptype == 1)
            strWhere += "and p_active<>0";
        // 开始读取竞猜
        IList<TPR2.Model.guess.BaPayMe> listBaPayMe = new TPR2.BLL.guess.BaPayMe().GetBaPayMeViews(pageIndex, pageSize, strWhere, ptype, out recordCount);
        if (listBaPayMe.Count > 0)
        {
            int k = 1;
            foreach (TPR2.Model.guess.BaPayMe n in listBaPayMe)
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
                if (ptype == 0)
                {
                    builder.AppendFormat("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.payusid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">{0}({1})</a>下注{2}币,共{3}注", n.payusname, n.payusid, Convert.ToDouble(n.payCents), n.payCount);
                }
                else
                {
                    builder.AppendFormat("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.payusid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">{0}({1})</a>下注{2}币,返{3},盈利{4}", n.payusname, n.payusid, Convert.ToDouble(n.payCents), Convert.ToDouble(n.payCount), Convert.ToDouble(n.payCount - n.payCents));

                }
                builder.Append(Out.Tab("</div>", ""));
                k++;
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            if (ptype == 0)
                builder.Append(Out.Div("div", "没有相关记录.."));
            else
                builder.Append(Out.Div("div", "没有记录或赛事并没有结束.."));

        }
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append(Out.waplink(Utils.getPage("kzlist.aspx?act=view&amp;gid=" + gid + ""), "返回上一级") + "<br />");
        builder.Append(Out.waplink(Utils.getUrl("default.aspx"), "返回球彩管理") + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

    }

    private void DelPage()
    {

        int gid = Utils.ParseInt(Utils.GetRequest("gid", "all", 2, @"^[0-9]*$", "竞猜ID无效"));
        TPR2.Model.guess.BaListMe model = new TPR2.BLL.guess.BaListMe().GetModel(gid);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "删除记录";
        string info = Utils.GetRequest("info", "get", 1, "", "");
        if (info != "ok")
        {

            long getMaxNum = new TPR2.BLL.guess.BaPayMe().GetBaPayMeCent(gid, Convert.ToInt32(model.p_type));

            builder.Append(Out.Tab("<div class=\"title\">", ""));
            if (getMaxNum > 0)
            {
                builder.Append("存在下注记录,");
            }
            builder.Append("确定要删除吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.waplink(Utils.getUrl("kzlist.aspx?info=ok&amp;act=del&amp;gid=" + gid + ""), "确定删除") + "<br />");
            builder.Append(Out.waplink(Utils.getUrl("kzlist.aspx?act=view&amp;gid=" + gid + ""), "先留着吧.."));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            //游戏日志记录
            int ManageId = new BCW.User.Manage().IsManageLogin();
            string[] p_pageArr = { "act", "gid", "info" };
            BCW.User.GameLog.GameLogGetPage(1, Utils.getPageUrl(), p_pageArr, "后台管理员" + ManageId + "号删除个人庄赛事" + model.p_one + "VS" + model.p_two + "(" + gid + ")", gid);


            new TPR2.BLL.guess.BaListMe().Delete(gid);
            new TPR2.BLL.guess.BaPayMe().DeleteStr("bcid=" + gid + "");
            Utils.Success("删除", "删除赛事成功..", Utils.getUrl("kzlist.aspx"), "1");
        }
    }

    private void BackPage()
    {
        int gid = Utils.ParseInt(Utils.GetRequest("gid", "get", 2, @"^[0-9]\d*$", "竞猜ID无效"));
        int id = Utils.ParseInt(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID无效"));

        if (!new TPR2.BLL.guess.BaListMe().Exists(gid))
        {
            Utils.Error("不存在的记录", "");
        }

        TPR2.Model.guess.BaPayMe model = new TPR2.BLL.guess.BaPayMe().GetModelIsCase(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "撤销押注并退回本金";

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("撤销押注并退回本金");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "您的个人庄" + Out.SysUBB(model.payview).Replace("/", "／").Replace(",", "，") + "竞猜失败已作退回本金处理，/,,,,";
        string strName = "Content,gid,id,act,backurl";
        string strType = "text,hidden,hidden,hidden,hidden";
        string strValu = "'" + gid + "'" + id + "'backsave'" + Utils.getPage(0) + "";
        string strEmpt = "true,false,false,false,false";
        string strIdea = "/";
        string strOthe = "本条撤销并退本金,kzlist.aspx,post,1,red";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.waplink(Utils.getPage("kzlist.aspx?act=plist&amp;gid=" + gid + ""), "返回上一级"));
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回球彩竞猜</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }


    private void BackSavePage()
    {
        int gid = Utils.ParseInt(Utils.GetRequest("gid", "post", 2, @"^[0-9]\d*$", "竞猜ID无效"));
        int id = Utils.ParseInt(Utils.GetRequest("id", "post", 2, @"^[0-9]\d*$", "ID无效"));

        if (!new TPR2.BLL.guess.BaListMe().Exists(gid))
        {
            Utils.Error("不存在的记录", "");
        }

        TPR2.Model.guess.BaPayMe model = new TPR2.BLL.guess.BaPayMe().GetModelIsCase(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        string Content = Utils.GetRequest("Content", "all", 3, @"^[\s\S]{1,200}$", "原因限200字内，可以留空");

        //游戏日志记录
        int ManageId = new BCW.User.Users().GetUsId();
        string[] p_pageArr = { "act", "gid", "id", "Content" };
        BCW.User.GameLog.GameLogPage(1, Utils.getPageUrl(), p_pageArr, "前台管理员ID" + ManageId + "号操作:会员ID" + model.payusid + ":个人庄" + model.payview + "竞猜失败已作退回本金处理", gid);


        string Msgtxt = string.Empty;
        if (Content == "")
        {
            Msgtxt = model.payview + "个人庄竞猜失败已作退回本金处理";

        }
        else
        {
            Msgtxt = model.payview + "个人庄竞猜失败已作退回本金处理，" + Content + "";
        }

        new BCW.BLL.Guest().Add(Convert.ToInt32(model.payusid), model.payusname, Msgtxt);
        //退本金
        if (model.Types == 0)
            new BCW.BLL.User().UpdateiGold(Convert.ToInt32(model.payusid), model.payusname, Convert.ToInt64(model.payCent), "系统撤销押注并退回本金");
        else
            new BCW.BLL.User().UpdateiMoney(Convert.ToInt32(model.payusid), model.payusname, Convert.ToInt64(model.payCent), "系统撤销押注并退回本金");

        //删除处理
        new TPR2.BLL.guess.BaPayMe().Delete(id);
        Utils.Success("撤销押注", "撤销押注并退回本金成功...", Utils.getPage("kzlist.aspx?act=plist&amp;gid=" + gid + ""), "1");
    }
}