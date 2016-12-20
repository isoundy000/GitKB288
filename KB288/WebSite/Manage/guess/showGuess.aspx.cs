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
using BCW.Common;
using TPR.Common;

public partial class Manage_guess_ShowGuess : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        int gid = Utils.ParseInt(Utils.GetRequest("gid", "all", 2, @"^[0-9]*$", "竞猜ID无效"));
        string act = Utils.GetRequest("act", "all", 1, "", "");
        string jc = Utils.GetRequest("jc", "get", 1, "", "");

        TPR.BLL.guess.BaList bll = new TPR.BLL.guess.BaList();


        TPR.Model.guess.BaList model = bll.GetModel(gid);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }

        Master.Title = model.p_one + "VS" + model.p_two;

        //更新隐藏与显示
        if (act == "yes")
        {
            model.p_del = 0;
            new TPR.BLL.guess.BaList().Updatep_del(model);
            //游戏日志记录
            string[] p_pageArr = { "act", "gid" };
            BCW.User.GameLog.GameLogGetPage(Utils.getPageUrl(), p_pageArr, "后台管理员" + ManageId + "号将赛事" + model.p_one + "VS" + model.p_two + "(" + gid + ")开放显示", gid);
        }
        else if (act == "no")
        {
            model.p_del = 1;
            new TPR.BLL.guess.BaList().Updatep_del(model);
            //游戏日志记录
            string[] p_pageArr = { "act", "gid" };
            BCW.User.GameLog.GameLogGetPage(Utils.getPageUrl(), p_pageArr, "后台管理员" + ManageId + "号将赛事" + model.p_one + "VS" + model.p_two + "(" + gid + ")隐藏显示", gid);
        }

        //更新抓取与不抓取
        if (jc == "yes")
        {
            model.p_jc = 0;
            new TPR.BLL.guess.BaList().Updatep_jc(model);
            //游戏日志记录
            string[] p_pageArr = { "jc", "gid" };
            BCW.User.GameLog.GameLogGetPage(Utils.getPageUrl(), p_pageArr, "后台管理员" + ManageId + "号将赛事" + model.p_one + "VS" + model.p_two + "(" + gid + ")开启抓取", gid);
        }
        else if (jc == "no")
        {
            model.p_jc = 1;
            new TPR.BLL.guess.BaList().Updatep_jc(model);
            //游戏日志记录
            string[] p_pageArr = { "jc", "gid" };
            BCW.User.GameLog.GameLogGetPage(Utils.getPageUrl(), p_pageArr, "后台管理员" + ManageId + "号将赛事" + model.p_one + "VS" + model.p_two + "(" + gid + ")关闭抓取", gid);
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
                BCW.User.GameLog.GameLogGetPage(Utils.getPageUrl(), p_pageArr, "后台管理员" + ManageId + "号删除赛事" + model.p_one + "VS" + model.p_two + "(" + gid + ")", gid);

                new TPR.BLL.guess.BaList().Delete(gid);
                if (act == "delok2")
                {
                    TPR.BLL.guess.BaPay bll2 = new TPR.BLL.guess.BaPay();
                    new TPR.BLL.guess.BaPay().Deletebcid(gid);
                }
                Utils.Success("删除赛事", "删除赛事成功..", Utils.getUrl("default.aspx"), "1");
            }
        }

        //转换成走地模式
        else if (act == "once" || act == "onceok")
        {
            if (!Utils.GetDomain().Contains("tl88") && !Utils.GetDomain().Contains("168yy"))
            {
                Utils.Error("不存在的页面", "");
            }

            if (act == "once")
            {
                string p_oncetime = string.Empty;
                if (string.IsNullOrEmpty(model.p_oncetime.ToString()))
                    p_oncetime = DT.FormatDate(Convert.ToDateTime(model.p_TPRtime).AddMinutes(100), 0);
                else
                    p_oncetime = DT.FormatDate(Convert.ToDateTime(model.p_oncetime), 0);

                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("确定要转换成走地下注模式吗");
                builder.Append(Out.Tab("</div>", ""));
                string strText = "封盘时间,,";
                string strName = "oncetime,gid,act";
                string strType = "date,hidden,hidden";
                string strValu = "" + p_oncetime + "'" + gid + "'onceok";
                string strEmpt = "false,false,false";
                string strIdea = "/";
                string strOthe = "转换,showGuess.aspx,post,1,red";

                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", " "));
                builder.Append(Out.waplink(Utils.getUrl("showGuess.aspx?gid=" + gid + ""), "取消"));
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            else
            {
                DateTime oncetime = Utils.ParseTime(Utils.GetRequest("oncetime", "post", 2, DT.RegexTime, "请正确填写封盘时间"));

                if (Convert.ToDateTime(model.p_TPRtime) > oncetime)
                {
                    Utils.Error("封盘时间应大于开赛时间", "");
                }
                //游戏日志记录
                string[] p_pageArr = {"oncetime", "act", "gid" };
                BCW.User.GameLog.GameLogGetPage(Utils.getPageUrl(), p_pageArr, "后台管理员" + ManageId + "号编辑赛事" + model.p_one + "VS" + model.p_two + "(" + gid + ")成为走地", gid);

                new TPR.BLL.guess.BaList().FootOnceType(gid, oncetime);
                Utils.Success("转移走地", "转换成功..", Utils.getUrl("showguess.aspx?gid=" + gid + ""), "1");
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
                builder.Append("<br />完场比分：" + model.p_result_one + ":" + model.p_result_two + "");
            else
                builder.Append("<br />即时比分：" + model.p_result_temp1 + ":" + model.p_result_temp2 + "");

            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("〓让球盘〓");
            builder.AppendFormat("<br />{0}" + Out.waplink(Utils.getUrl("plGuess.aspx?gid=" + model.ID + "&amp;p=1"), "{1}注"), model.p_one + "(" + Convert.ToDouble(model.p_one_lu) + ")", new TPR.BLL.guess.BaPay().GetCount(model.ID, Convert.ToInt32(model.p_type), 1));

            if (model.p_type == 1)
                builder.Append("<br />" + GCK.getZqPn(Convert.ToInt32(model.p_pn)) + "" + GCK.getPkName(Convert.ToInt32(model.p_pk)) + "");
            else
                builder.Append("<br />" + Convert.ToDouble(model.p_pk) + "");

            builder.AppendFormat("<br />{0}" + Out.waplink(Utils.getUrl("plGuess.aspx?gid=" + model.ID + "&amp;p=2"), "{1}注"), model.p_two + "(" + Convert.ToDouble(model.p_two_lu) + ")", new TPR.BLL.guess.BaPay().GetCount(model.ID, Convert.ToInt32(model.p_type), 2));
            if (model.p_big_lu != 0)
            {
                builder.Append("<br />〓大小盘〓");
                builder.AppendFormat("<br />" + Out.waplink(Utils.getUrl("plGuess.aspx?gid=" + model.ID + "&amp;p=3"), "{0}注") + "{1}", new TPR.BLL.guess.BaPay().GetCount(model.ID, Convert.ToInt32(model.p_type), 3), "大(" + Convert.ToDouble(model.p_big_lu) + ")");

                if (model.p_type == 1)
                    builder.Append(GCK.getDxPkName(Convert.ToInt32(model.p_dx_pk)));
                else
                    builder.Append(Convert.ToDouble(model.p_dx_pk));

                builder.AppendFormat("{0}" + Out.waplink(Utils.getUrl("plGuess.aspx?gid=" + model.ID + "&amp;p=4"), "{1}注"), "小(" + Convert.ToDouble(model.p_small_lu) + ")", new TPR.BLL.guess.BaPay().GetCount(model.ID, Convert.ToInt32(model.p_type), 4));
            }
            if (model.p_bzs_lu != 0)
            {
                builder.Append("<br />〓标准盘〓");
                builder.AppendFormat("<br />{0}" + Out.waplink(Utils.getUrl("plGuess.aspx?gid=" + model.ID + "&amp;p=5"), "{1}注"), "主胜(" + Convert.ToDouble(model.p_bzs_lu) + ")", new TPR.BLL.guess.BaPay().GetCount(model.ID, Convert.ToInt32(model.p_type), 5));
                builder.AppendFormat("<br />{0}" + Out.waplink(Utils.getUrl("plGuess.aspx?gid=" + model.ID + "&amp;p=6"), "{1}注"), "平手(" + Convert.ToDouble(model.p_bzp_lu) + ")", new TPR.BLL.guess.BaPay().GetCount(model.ID, Convert.ToInt32(model.p_type), 6));
                builder.AppendFormat("<br />{0}" + Out.waplink(Utils.getUrl("plGuess.aspx?gid=" + model.ID + "&amp;p=7"), "{1}注"), "客胜(" + Convert.ToDouble(model.p_bzx_lu) + ")", new TPR.BLL.guess.BaPay().GetCount(model.ID, Convert.ToInt32(model.p_type), 7));
            }
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));

            long PayCents = new TPR.BLL.guess.BaPay().GetBaPayCent(model.ID, Convert.ToInt32(model.p_type));
            builder.Append("" + ub.Get("SiteBz") + "统计:<br />总注数:" + new TPR.BLL.guess.BaPay().GetBaPayNum(model.ID, Convert.ToInt32(model.p_type)) + "/下注额:" + PayCents + "");
            if (model.p_result_one != null && model.p_result_two != null)
            {
                long WinMoney = new TPR.BLL.guess.BaPay().GetBaPaygetMoney("bcid=" + gid + " and Types=0");

                builder.Append("<br />总返彩:" + WinMoney + "/盈利额:" + (PayCents - WinMoney) + "");
            }

            builder.Append("<br />〓管理〓");
            if (model.p_result_one != null && model.p_result_two != null)
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

             builder.Append("球探网ID:" + model.p_id + "<br />");


            if (model.p_type == 1)
            {
                if (Utils.GetDomain().Contains("tl88") || Utils.GetDomain().Contains("168yy"))
                {
                    if (model.p_ison == 1)
                        builder.Append("<br />走地状态：走地");
                    else
                        builder.Append("<br />走地状态：非走地");

                    builder.Append("" + Out.waplink(Utils.getUrl("showGuess.aspx?gid=" + gid + "&amp;act=once"), "编辑"));
                }
            }
            if (Utils.GetDomain().Contains("kubao") || Utils.GetDomain().Contains("tuhao") || Utils.GetDomain().Contains("th") || Utils.GetDomain().Contains("kb288") || Utils.GetDomain().Contains("127.0.0.6"))
            {
                builder.Append("<br />" + Out.waplink(Utils.getUrl("../forumlog.aspx?act=gamelog&amp;gid=" + gid + "&amp;backurl=" + Utils.PostPage(1) + ""), "操作日志"));
                builder.Append("<br />" + Out.waplink(Utils.getUrl("../forumlog.aspx?act=gameowe&amp;gid=" + gid + "&amp;backurl=" + Utils.PostPage(1) + ""), "欠币日志"));
            }
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
