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

public partial class Manage_guess2_payView : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Request["act"] == "case")
        {
            int pid = Utils.ParseInt(Utils.GetRequest("pid", "get", 2, @"^[0-9]\d*$", "选择赛事无效"));
            int hid = Utils.ParseInt(Utils.GetRequest("hid", "get", 1, @"^[0-9]\d*$", "0"));

            if (new TPR2.BLL.guess.BaPay().ExistsIsCase(pid, hid))
            {
                int bcid = new TPR2.BLL.guess.BaPay().Getbcid(pid);
                new TPR2.BLL.guess.BaPay().UpdateIsCase(pid);
                //操作币
                long win = Convert.ToInt64(new TPR2.BLL.guess.BaPay().Getp_getMoney(pid));
                int Types = new TPR2.BLL.guess.BaPay().GetTypes(pid);
                if (Types == 0)
                {
                    new BCW.BLL.User().UpdateiGold(hid, win, "竞猜赛事ID" + bcid + "#记录ID" + pid + "兑奖(管)");
                    Utils.Success("兑奖", "成功兑奖" + win + "" + ub.Get("SiteBz") + "", Utils.getPage("payView.aspx?ptype=2"), "1");
                }
                else
                {
                    new BCW.BLL.User().UpdateiMoney(hid, win, "竞猜赛事ID" + bcid + "#记录ID" + pid + "兑奖(管)");
                    Utils.Success("兑奖", "成功兑奖" + win + "" + ub.Get("SiteBz2") + "", Utils.getPage("payView.aspx?ptype=2"), "1");
                }

            }
            else
            {
                Utils.Success("兑奖", "重复兑奖或没有可以兑奖的记录", Utils.getPage("payView.aspx?ptype=2"), "1");
            }
        }
        else
        {

            Master.Title = "下注详细记录";
            int gid = Utils.ParseInt(Utils.GetRequest("gid", "get", 2, @"^[0-9]\d*$", "竞猜ID无效"));
            int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[0-2]$", "0"));
            int showtype = Utils.ParseInt(Utils.GetRequest("showtype", "get", 1, @"^[0-1]$", "0"));

            builder.Append(Out.Tab("<div class=\"title\">", ""));
            if (ptype == 0)
            {

                builder.Append("下注记录|" + Out.waplink(Utils.getUrl("payView.aspx?gid=" + gid + "&amp;ptype=1"), "赢输记录") + "|" + Out.waplink(Utils.getUrl("payView.aspx?gid=" + gid + "&amp;ptype=2"), "兑奖记录"));
            }
            else if (ptype == 1)
            {
                builder.Append(Out.waplink(Utils.getUrl("payView.aspx?gid=" + gid + "&amp;ptype=0"), "下注记录") + "|赢输记录|" + Out.waplink(Utils.getUrl("payView.aspx?gid=" + gid + "&amp;ptype=2"), "兑奖记录"));
            }
            else if (ptype == 2)
            {
                builder.Append(Out.waplink(Utils.getUrl("payView.aspx?gid=" + gid + "&amp;ptype=0"), "下注记录") + "|" + Out.waplink(Utils.getUrl("payView.aspx?gid=" + gid + "&amp;ptype=1"), "赢输记录") + "|兑奖记录");
            }
            builder.Append(Out.Tab("</div>", "<br />"));

            if (ptype == 2)
            {
                builder.Append(Out.Tab("<div>", ""));
                if (showtype == 0)
                    builder.Append("未兑奖|");
                else
                    builder.Append(Out.waplink(Utils.getUrl("payView.aspx?gid=" + gid + "&amp;ptype=2&amp;showtype=0"), "未兑奖") + "|");

                if (showtype == 1)
                    builder.Append("已兑奖");
                else
                    builder.Append(Out.waplink(Utils.getUrl("payView.aspx?gid=" + gid + "&amp;ptype=2&amp;showtype=1"), "已兑奖"));

                builder.Append(Out.Tab("</div>", "<br />"));
            }

            int pageSize = 10;
            int pageIndex;
            int recordCount;
            string[] pageValUrl = { "gid", "ptype", "showtype" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            if (ptype <= 1)
            {


                //组合条件
                string strWhere = "";
                strWhere += "bcid=" + gid + " and Types=0";
                if (ptype == 1)
                    strWhere += "and p_active<>0";
                // 开始读取竞猜
                IList<TPR2.Model.guess.BaPay> listBaPay = new TPR2.BLL.guess.BaPay().GetBaPayViews(pageIndex, pageSize, strWhere, ptype, out recordCount);
                if (listBaPay.Count > 0)
                {
                    int k = 1;
                    foreach (TPR2.Model.guess.BaPay n in listBaPay)
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
                            builder.AppendFormat("<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.payusid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">{0}({1})</a>下注{2}币,共{3}注", n.payusname, n.payusid, Convert.ToDouble(n.payCents), n.payCount);
                        }
                        else
                        {
                            builder.AppendFormat("<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.payusid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">{0}({1})</a>下注{2}币,返{3},盈利{4}", n.payusname, n.payusid, Convert.ToDouble(n.payCents), Convert.ToDouble(n.payCount), Convert.ToDouble(n.payCount - n.payCents));
                        }
                        if (n.isrobot == 1) { builder.AppendFormat("<h style=\"color:red\">.[机器人]</h>"); }
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
            }
            else
            {
                //组合条件
                string strWhere = "";

                strWhere += "p_active>0 and itypes=0 ";

                strWhere += " and bcid=" + gid + " and p_case=" + showtype + " and p_getMoney > 0";

                // 开始读取竞猜
                string arrId = "";
                IList<TPR2.Model.guess.BaPay> listBaPay = new TPR2.BLL.guess.BaPay().GetBaPays(pageIndex, pageSize, strWhere, out recordCount);
                if (listBaPay.Count > 0)
                {
                    int k = 1;
                    foreach (TPR2.Model.guess.BaPay n in listBaPay)
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
                        string bzType = string.Empty;
                        if (n.Types == 0)
                            bzType = ub.Get("SiteBz");
                        else
                            bzType = ub.Get("SiteBz2");

                        builder.Append("[" + n.ID + "]");
                        builder.Append(Out.waplink(Utils.getUrl("../uinfo.aspx?uid=" + n.payusid + "&amp;backurl=" + Utils.PostPage(1) + ""), "" + n.payusname + "") + ":");
                        string robot = "";
                        if (n.isrobot == 1) { robot = "<h style=\"color:red\">.[机器人]</h>"; }
                        if (n.p_active == 2)
                        {
                            builder.AppendFormat("{0},平盘" + robot + "<br />时间:{1}", Out.SysUBB(n.payview).Replace("/bbs/guess2/", ""), n.paytimes);
                            builder.AppendFormat(" 返{0}" + bzType + "", Convert.ToDouble(n.p_getMoney));
                            if (showtype == 0)
                                builder.Append("" + Out.waplink(Utils.getUrl("payView.aspx?act=case&amp;pid=" + n.ID + "&amp;hid=" + n.payusid + "&amp;backurl=" + Utils.PostPage(1) + ""), "帮TA兑奖") + "");

                        }
                        else
                        {
                            builder.AppendFormat("{0},结果{1}:{2}" + robot + "<br />时间:{3}", Out.SysUBB(n.payview).Replace("/bbs/guess2/", ""), n.p_result_one, n.p_result_two, n.paytimes);
                            builder.AppendFormat(" 赢{0}" + bzType + "", Convert.ToDouble(n.p_getMoney));
                            if (showtype == 0)
                                builder.Append("" + Out.waplink(Utils.getUrl("payView.aspx?act=case&amp;pid=" + n.ID + "&amp;hid=" + n.payusid + "&amp;backurl=" + Utils.PostPage(1) + ""), "帮TA兑奖") + "");

                        }
                        builder.Append(Out.Tab("</div>", ""));
                        arrId = arrId + " " + n.ID;
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

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.waplink(Utils.getUrl("showGuess.aspx?gid=" + gid + ""), "返回上一级"));
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
}
