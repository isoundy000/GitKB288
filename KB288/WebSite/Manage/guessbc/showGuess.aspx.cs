using System;
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
using TPR3.Common;

public partial class Manage_guess3_ShowGuess : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        int gid = Utils.ParseInt(Utils.GetRequest("gid", "all", 2, @"^[0-9]*$", "竞猜ID无效"));
        string act = Utils.GetRequest("act", "all", 1, "", "");
        string jc = Utils.GetRequest("jc", "get", 1, "", "");
        int ManageId = new BCW.User.Manage().IsManageLogin();
        TPR3.BLL.guess.BaList bll = new TPR3.BLL.guess.BaList();

        TPR3.Model.guess.BaList st = bll.GetModel(gid);
        if (st == null)
        {
            Utils.Error("不存在的记录", "");
        }

        //载入页面更新足球上半场
        if (st.p_type == 1 && st.p_active == 0)
        {
            if (st.p_ison == 1)
            {
                new TPR2.Collec.Footbo().GetBoView(Convert.ToInt32(st.p_id), true);
                new TPR3.Collec.FootFalf().FootFalfPageHtml(Convert.ToInt32(st.p_id), true);
            }
            else
                new TPR3.Collec.FootFalf().FootFalfPageHtml(Convert.ToInt32(st.p_id), false);
        }

        TPR3.Model.guess.BaList model = bll.GetModel(gid);


        Master.Title = model.p_one + "VS" + model.p_two;

        //更新隐藏与显示
        if (act == "yes")
        {
            //游戏日志记录
            string[] p_pageArr = { "act", "gid" };
            BCW.User.GameLog.GameLogGetPage(8, Utils.getPageUrl(), p_pageArr, "后台管理员" + ManageId + "号将赛事" + model.p_one + "VS" + model.p_two + "(" + gid + ")开放显示", gid);
            model.p_del = 0;
            new TPR3.BLL.guess.BaList().Updatep_del(model);
        }
        else if (act == "no")
        {

            //游戏日志记录
            string[] p_pageArr = { "act", "gid" };
            BCW.User.GameLog.GameLogGetPage(8, Utils.getPageUrl(), p_pageArr, "后台管理员" + ManageId + "号将赛事" + model.p_one + "VS" + model.p_two + "(" + gid + ")隐藏显示", gid);
            model.p_del = 1;
            new TPR3.BLL.guess.BaList().Updatep_del(model);
        }

        //更新抓取与不抓取
        if (jc == "yes")
        {
            //游戏日志记录
            string[] p_pageArr = { "jc", "gid" };
            BCW.User.GameLog.GameLogGetPage(8, Utils.getPageUrl(), p_pageArr, "后台管理员" + ManageId + "号将赛事" + model.p_one + "VS" + model.p_two + "(" + gid + ")开启抓取", gid);
            model.p_jc = 0;
            new TPR3.BLL.guess.BaList().Updatep_jc(model);
        }
        else if (jc == "no")
        {
            //游戏日志记录
            string[] p_pageArr = { "jc", "gid" };
            BCW.User.GameLog.GameLogGetPage(8, Utils.getPageUrl(), p_pageArr, "后台管理员" + ManageId + "号将赛事" + model.p_one + "VS" + model.p_two + "(" + gid + ")关闭抓取", gid);
            model.p_jc = 1;
            new TPR3.BLL.guess.BaList().Updatep_jc(model);
        }

        //删除赛事
        if (act == "del" || act == "delok1" || act == "delok2")
        {
            if (act == "del")
            {
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("确定删除此赛事吗");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append(Out.waplink(Utils.getUrl("showGuess.aspx?gid=" + gid + "&amp;act=delok1"), "删除,不包含下注记录") + "<br />");
                builder.Append(Out.waplink(Utils.getUrl("showGuess.aspx?gid=" + gid + "&amp;act=delok2"), "删除,包含下注记录") + "<br />");
                builder.Append(Out.waplink(Utils.getUrl("showGuess.aspx?gid=" + gid + ""), "先留着吧.."));
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            else
            {
                //游戏日志记录
                string[] p_pageArr = { "act", "gid" };
                BCW.User.GameLog.GameLogGetPage(8, Utils.getPageUrl(), p_pageArr, "后台管理员" + ManageId + "号删除赛事" + model.p_one + "VS" + model.p_two + "(" + gid + ")", gid);

                new TPR3.BLL.guess.BaList().Delete(gid);
                if (act == "delok2")
                {
                    TPR3.BLL.guess.BaPay bll2 = new TPR3.BLL.guess.BaPay();
                    new TPR3.BLL.guess.BaPay().Deletebcid(gid);
                }
                Utils.Success("删除赛事", "删除赛事成功..", Utils.getUrl("default.aspx"), "1");
            }
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append(model.p_one + "VS" + model.p_two);
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.waplink(Utils.getUrl("default.aspx?ptype=4&amp;fly=" + model.p_title + ""), model.p_title) + ":" + model.p_one + "VS" + model.p_two);
            builder.Append("<br />开赛:" + DT.FormatDate(Convert.ToDateTime(model.p_TPRtime), 0));

            if (model.p_result_one != null && model.p_result_two != null)
                builder.Append("<br />上半场比分：" + model.p_result_one + ":" + model.p_result_two + "");
            else
            {

                if (model.p_TPRtime > DateTime.Now)
                {
                    builder.Append("<br />参考比分：未");
                }
                else
                {
                    if (model.p_type == 1)
                    {
                        builder.Append("<br />比赛状态:" + model.p_once + "");
                        builder.Append("<br />即时比分:" + model.p_result_temp1 + ":" + model.p_result_temp2 + "");
                    }
                    else
                    {
                        builder.Append("<br />比赛状态:进行中");
                    }
                }
            }
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("〓上下盘〓");
            if (model.p_ison == 1 && model.p_isluckone == 1 && model.p_active == 0)
                builder.Append("<b>(封)</b>");

            builder.AppendFormat("<br />{0}" + Out.waplink(Utils.getUrl("plGuess.aspx?gid=" + model.ID + "&amp;p=1"), "{1}注"), model.p_one + "(" + Convert.ToDouble(model.p_one_lu) + ")", new TPR3.BLL.guess.BaPay().GetCount(model.ID, Convert.ToInt32(model.p_type), 1));

            if (model.p_type == 1)
                builder.Append("<br />" + GCK.getZqPn(Convert.ToInt32(model.p_pn)) + "" + GCK.getPkName(Convert.ToInt32(model.p_pk)) + "");
            else
                builder.Append("<br />" + Convert.ToDouble(model.p_pk) + "");

            builder.AppendFormat("<br />{0}" + Out.waplink(Utils.getUrl("plGuess.aspx?gid=" + model.ID + "&amp;p=2"), "{1}注"), model.p_two + "(" + Convert.ToDouble(model.p_two_lu) + ")", new TPR3.BLL.guess.BaPay().GetCount(model.ID, Convert.ToInt32(model.p_type), 2));
            if (model.p_big_lu != 0)
            {
                builder.Append("<br />〓大小盘〓");
                if (model.p_ison == 1 && model.p_islucktwo == 1 && model.p_active == 0)
                    builder.Append("<b>(封)</b>");

                builder.AppendFormat("<br />" + Out.waplink(Utils.getUrl("plGuess.aspx?gid=" + model.ID + "&amp;p=3"), "{0}注") + "{1}", new TPR3.BLL.guess.BaPay().GetCount(model.ID, Convert.ToInt32(model.p_type), 3), "大(" + Convert.ToDouble(model.p_big_lu) + ")");

                if (model.p_type == 1)
                    builder.Append(GCK.getDxPkName(Convert.ToInt32(model.p_dx_pk)));
                else
                    builder.Append(Convert.ToDouble(model.p_dx_pk));

                builder.AppendFormat("{0}" + Out.waplink(Utils.getUrl("plGuess.aspx?gid=" + model.ID + "&amp;p=4"), "{1}注"), "小(" + Convert.ToDouble(model.p_small_lu) + ")", new TPR3.BLL.guess.BaPay().GetCount(model.ID, Convert.ToInt32(model.p_type), 4));
            }
            if (model.p_bzs_lu != 0)
            {
                builder.Append("<br />〓标准盘〓");
                if (model.p_ison == 1 && model.p_isluckthr == 1 && model.p_active == 0)
                    builder.Append("<b>(封)</b>");

                builder.AppendFormat("<br />{0}" + Out.waplink(Utils.getUrl("plGuess.aspx?gid=" + model.ID + "&amp;p=5"), "{1}注"), "主胜(" + Convert.ToDouble(model.p_bzs_lu) + ")", new TPR3.BLL.guess.BaPay().GetCount(model.ID, Convert.ToInt32(model.p_type), 5));
                builder.AppendFormat("<br />{0}" + Out.waplink(Utils.getUrl("plGuess.aspx?gid=" + model.ID + "&amp;p=6"), "{1}注"), "平手(" + Convert.ToDouble(model.p_bzp_lu) + ")", new TPR3.BLL.guess.BaPay().GetCount(model.ID, Convert.ToInt32(model.p_type), 6));
                builder.AppendFormat("<br />{0}" + Out.waplink(Utils.getUrl("plGuess.aspx?gid=" + model.ID + "&amp;p=7"), "{1}注"), "客胜(" + Convert.ToDouble(model.p_bzx_lu) + ")", new TPR3.BLL.guess.BaPay().GetCount(model.ID, Convert.ToInt32(model.p_type), 7));
            }
            if (model.p_d_lu != 0 && model.p_d_lu != -1)
            {
                builder.Append("<br />〓单双盘〓");
                builder.Append("<br />" + Out.waplink(Utils.getUrl("plGuess.aspx?gid=" + model.ID + "&amp;p=8"), "" + new TPR3.BLL.guess.BaPay().GetCount(model.ID, Convert.ToInt32(model.p_type), 8) + "注") + "单(" + Convert.ToDouble(model.p_d_lu) + ")");
                builder.Append("/双(" + Convert.ToDouble(model.p_s_lu) + ")" + Out.waplink(Utils.getUrl("plGuess.aspx?gid=" + model.ID + "&amp;p=9"), "" + new TPR3.BLL.guess.BaPay().GetCount(model.ID, Convert.ToInt32(model.p_type), 9) + "注") + "");
            }
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("当前总下注" + new TPR3.BLL.guess.BaPay().GetBaPayNum(model.ID, Convert.ToInt32(model.p_type)) + "注,下注额" + new TPR3.BLL.guess.BaPay().GetBaPayCent(model.ID, Convert.ToInt32(model.p_type)) + "币");

            builder.Append("<br />〓管理〓");
            if (model.p_active > 0 && model.p_result_one != null && model.p_result_two != null)
                builder.Append("<br />" + Out.waplink(Utils.getUrl("openGuess.aspx?gid=" + gid + ""), "重开奖"));
            else
                builder.Append("<br />" + Out.waplink(Utils.getUrl("openGuess.aspx?gid=" + gid + ""), "开奖"));

            builder.Append(" " + Out.waplink(Utils.getUrl("editGuess.aspx?gid=" + gid + ""), "修改"));
            builder.Append(" " + Out.waplink(Utils.getUrl("showGuess.aspx?gid=" + gid + "&amp;act=del"), "删除"));

            builder.Append(" " + Out.waplink(Utils.getUrl("payView.aspx?gid=" + gid + ""), "记录"));

            if (model.p_del == 0)
                builder.Append(" " + Out.waplink(Utils.getUrl("showGuess.aspx?gid=" + gid + "&amp;act=no"), "隐藏"));
            else
                builder.Append(" " + Out.waplink(Utils.getUrl("showGuess.aspx?gid=" + gid + "&amp;act=yes"), "显示"));

            if (model.p_jc == 0)
                builder.Append("<br />抓取状态：正常抓取<br />" + Out.waplink(Utils.getUrl("showGuess.aspx?gid=" + gid + "&amp;jc=no"), "关闭抓取"));
            else
                builder.Append("<br />抓取状态：停止抓取<br />" + Out.waplink(Utils.getUrl("showGuess.aspx?gid=" + gid + "&amp;jc=yes"), "开启抓取"));

            if (model.p_ison == 1)
                builder.Append("<br />" + ub.Get("SiteGqText") + "状态：" + ub.Get("SiteGqText") + "");
            else
                builder.Append("<br />" + ub.Get("SiteGqText") + "状态：非" + ub.Get("SiteGqText") + "");

            if (model.p_type == 1)
                builder.Append("<br />8波ID:" + model.p_id + "");
            else
                builder.Append("<br />球探ID:" + model.p_id + "");

            builder.Append("<br />" + Out.waplink(Utils.getUrl("../forumlog.aspx?act=gamelog&amp;ptype=8&amp;gid=" + gid + "&amp;backurl=" + Utils.PostPage(1) + ""), "操作日志"));
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.waplink(Utils.getPage("default.aspx"), "返回上一级"));
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }


    }
}
