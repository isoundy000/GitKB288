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

public partial class bbs_guess2_listGuess : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "赛事记录";

        int p_type = Utils.ParseInt(Utils.GetRequest("p_type", "get", 1, @"^[0-4]$", "0"));
        string fly = "";
        if (p_type == 4)
            fly = Out.UBB(Utils.GetRequest("fly", "get", 2, @"^.+?$", "请选择联赛"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));

        if (p_type == 0)
            builder.Append("全部 ");
        else
            builder.Append(Out.waplink(Utils.getUrl("listGuess.aspx?p_type=0"), "全部") + " ");

        if (p_type == 1)
            builder.Append("足球 ");
        else
            builder.Append(Out.waplink(Utils.getUrl("listGuess.aspx?p_type=1"), "足球") + " ");

        if (p_type == 2)
            builder.Append("篮球 ");
        else
            builder.Append(Out.waplink(Utils.getUrl("listGuess.aspx?p_type=2"), "篮球") + " ");

        if (p_type == 3)
            builder.Append("联赛 ");
        else
            builder.Append(Out.waplink(Utils.getUrl("listGuess.aspx?p_type=3"), "联赛") + " ");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageSize = 10;
        int pageIndex;
        int recordCount;
        string strDay = "";
        string strWhere = "";
        string[] pageValUrl = {"p_type","fly" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        if (p_type != 3)
        {

            if (p_type > 0 && p_type < 4)
                strWhere = "p_type=" + p_type + "";
            else if (p_type == 4)
                strWhere = "p_title like '%" + fly + "%'";

            string strOrder = "p_TPRtime DESC";

            // 开始读取竞猜
            IList<TPR2.Model.guess.BaList> listBaList = new TPR2.BLL.guess.BaList().GetBaLists(pageIndex, pageSize, strWhere, strOrder, out recordCount);
            if (listBaList.Count > 0)
            {
                int k = 1;
                foreach (TPR2.Model.guess.BaList n in listBaList)
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

                    if (DT.GetTime(n.p_TPRtime.ToString(), "MM月dd日").ToString() != strDay.ToString())
                        builder.Append(DT.GetTime(n.p_TPRtime.ToString(), "MM月dd日") + "<br />");

                    string strResult = "";
                    if (n.p_active == 1)
                    {
                        strResult = "[" + n.p_result_one + ":" + n.p_result_two + "]";
                    }
                    else
                    {
                        if (n.p_result_temp1 != null)
                            strResult = "[未结束" + n.p_result_temp1 + ":" + n.p_result_temp2 + "]";
                        else
                            strResult = "[未结束]";
                    }

                    builder.AppendFormat(Out.waplink(Utils.getUrl("showGuess.aspx?gid={0}&amp;backurl=" + Utils.PostPage(1) + ""), "{1}{2}{3}{4}") + "", n.ID, n.p_title, n.p_one, strResult, n.p_two);

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
            if (p_type == 4)
            {
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append( Out.waplink(Utils.getUrl("listGuess.aspx?p_type=3"), "返回上一级"));
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        else
        {
            string strLXWhere = "p_active>=0";

            // 开始读取竞猜
            IList<TPR2.Model.guess.BaList> listBaListLX = new TPR2.BLL.guess.BaList().GetBaListLX(pageIndex, 10, strLXWhere, out recordCount);
            if (listBaListLX.Count > 0)
            {
                int k = 1;
                foreach (TPR2.Model.guess.BaList n in listBaListLX)
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

                    builder.AppendFormat(Out.waplink(Utils.getUrl("listGuess.aspx?p_type=4&amp;fly={0}"), "-{1}(共{2}场)") + "", n.p_title, n.p_title, new TPR2.BLL.guess.BaList().GetCount(n));
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
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append(Out.waplink(Utils.getUrl("myGuess.aspx?ptype=1"), "未开投注") + " ");
        builder.Append(Out.waplink(Utils.getUrl("myGuess.aspx?ptype=2"), "历史投注") + "<br />");
        builder.Append(Out.waplink(Utils.getUrl("default.aspx"), "返回球彩首页") + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(Out.waplink(Utils.getUrl("/default.aspx"), "首页") + "-");
        builder.Append(Out.waplink(Utils.getPage("default.aspx"), "上级") + "-");
        builder.Append(Out.waplink(Utils.getUrl("default.aspx"), "球彩") + "");
        builder.Append(Out.Tab("</div>", ""));
    }
}
