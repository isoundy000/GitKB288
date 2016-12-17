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

public partial class Manage_guess_Default : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "竞猜管理";

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(Out.waplink(Utils.getUrl("../game/default.aspx"), "游戏") + "&gt;球彩竞猜");
        builder.Append(Out.Tab("</div>", "<br />"));

        if (Utils.GetDomain().Contains("tl88.cc") || Utils.GetDomain().Contains("168yy.cc"))
        {
            Master.Title = "明升竞猜管理";
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("../guess2/default.aspx") + "\">&gt;&gt;切换皇冠竞猜</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        builder.Append(Out.Tab("<div>", ""));
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[0-4]$", "0"));
        int showtype = Utils.ParseInt(Utils.GetRequest("showtype", "get", 1, @"^[0-2]$", "0"));
        string keyword = Out.UBB(Utils.GetRequest("keyword", "all", 1, "", ""));
        string fly = "";
        if (ptype == 4)
            fly = Out.UBB(Utils.GetRequest("fly", "get", 2, @"^.+?$", "请选择联赛"));

        builder.Append(Out.waplink(Utils.getUrl("addGuess.aspx"), "增加赛事") + " ");
        builder.Append(Out.waplink(Utils.getUrl("topGuess.aspx"), "排行榜") + " ");
        builder.Append(Out.waplink(Utils.getUrl("search.aspx"), "搜索") + "<br />");
        builder.Append("赛事分析<a href=\"" + Utils.getUrl("stats.aspx?showtype=2") + "\">未开</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("stats.aspx") + "\">完场</a><br />");
        if (ptype == 0)
            builder.Append("全部 ");
        else
            builder.Append(Out.waplink(Utils.getUrl("default.aspx"), "全部") + " ");
        if (ptype == 1)
            builder.Append("足球 ");
        else
            builder.Append(Out.waplink(Utils.getUrl("default.aspx?ptype=1"), "足球") + " ");

        if (ptype == 2)
            builder.Append("篮球 ");
        else
            builder.Append(Out.waplink(Utils.getUrl("default.aspx?ptype=2"), "篮球") + " ");

        if (ptype == 3)
            builder.Append("联赛 ");
        else
            builder.Append(Out.waplink(Utils.getUrl("default.aspx?ptype=3"), "联赛") + "");

        //builder.Append(Out.waplink(Utils.getUrl("super.aspx"), "串串") + "");

        //builder.Append(Out.waplink(Utils.getUrl("clear.aspx?act=actspaceguess"), "一键清空无投注赛事") + "");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageSize = 10;
        int pageIndex;
        int recordCount;
        string strDay = "";
        string strWhere = "";
        string[] pageValUrl = { "ptype", "fly", "keyword" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        if (ptype != 3)
        {
            if (keyword == "")
            {
                if (ptype > 0 && ptype < 4)
                    strWhere = "p_type=" + ptype + " ";
                else if (ptype == 4)
                    strWhere = "p_title like '%" + fly + "%'";
                else
                    strWhere = "";
            }
            else
            {
                if (Utils.IsRegex(keyword, @"^\d+$"))
                    strWhere = "id=" + keyword + "";
                else
                    strWhere = "(p_one like '%" + keyword + "%' or p_two like '%" + keyword + "%')";
            }
            string strOrder = "p_TPRtime DESC,ID DESC";
            // 开始读取竞猜
            IList<TPR.Model.guess.BaList> listBaList = new TPR.BLL.guess.BaList().GetBaLists(pageIndex, pageSize, strWhere, strOrder, out recordCount);
            if (listBaList.Count > 0)
            {
                int k = 1;
                foreach (TPR.Model.guess.BaList n in listBaList)
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

                    if (DT.GetTime(n.p_TPRtime.ToString(), "MM月dd日").ToString() != strDay.ToString())
                        builder.Append(DT.GetTime(n.p_TPRtime.ToString(), "MM月dd日") + "<br />");

                    string Sonce = string.Empty;
                    if (n.p_ison == 1)
                        Sonce = "(走)";
                    builder.Append(Out.waplink(Utils.getUrl("editGuess.aspx?gid=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + ""), "[管理]"));
                    builder.AppendFormat(Out.waplink(Utils.getUrl("showGuess.aspx?gid={0}&amp;backurl=" + Utils.PostPage(1) + ""), "{1}{2}VS{3}{4}") + "", n.ID, DT.FormatDate(Convert.ToDateTime(n.p_TPRtime), 13), n.p_one, n.p_two, Sonce);
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
        }

        else
        {
            string strLXWhere = "p_active=0";

            // 开始读取竞猜
            IList<TPR.Model.guess.BaList> listBaListLX = new TPR.BLL.guess.BaList().GetBaListLX(pageIndex, pageSize, strLXWhere, out recordCount);
            if (listBaListLX.Count > 0)
            {
                int k = 1;
                foreach (TPR.Model.guess.BaList n in listBaListLX)
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

                    builder.AppendFormat(Out.waplink(Utils.getUrl("default.aspx?ptype=4&amp;fly={0}"), "-{1}(共{2}场)") + "", n.p_title, n.p_title, new TPR.BLL.guess.BaList().GetCountByp_title(n.p_title));
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

        }
        string strText = "输入球队或赛事ID:/,";
        string strName = "keyword,backurl";
        string strType = "text,hidden";
        string strValu = "'" + Utils.getPage(0) + "";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "搜赛事,default.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        if (keyword != "")
        {
            builder.Append(Out.Tab("<div>", " "));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("clear.aspx") + "\">清空记录</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("../xml/guessset.aspx?backurl=" + Utils.PostPage(1) + "") + "\">游戏配置</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
}