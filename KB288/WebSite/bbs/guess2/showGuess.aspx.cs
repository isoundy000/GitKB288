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
using TPR2.Common;
//using LitJson;
using System.IO;
using System.Net;
using System.Collections.Generic;

/// <summary>
/// ==================================
/// 增加盘口变动历史功能 黄国军 20161028
/// 显示下注其他胆 黄国军 20160706
/// 增加代理抓取模式 黄国军 20160509
/// ==================================
/// </summary>
public partial class bbs_guess2_showGuess : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        int gid = Utils.ParseInt(Utils.GetRequest("gid", "get", 2, @"^[0-9]*$", "竞猜ID无效"));
        TPR2.BLL.guess.BaList bll = new TPR2.BLL.guess.BaList();
        TPR2.Model.guess.BaList st = bll.GetModel(gid);
        if (st == null)
        {
            Utils.Error("不存在的记录", "");
        }

        #region 立即更新水位 访问8bo
        int second = 0;
        string bo = "";
        //-----------------------------立即更新水位---------------------------------
        if (st.p_active == 0)
        {
            //读取SiteViewStatus 等于0时采用即时刷新，其他值时，通过刷新机刷新
            //黄国军 20160223
            if (ub.GetSub("SiteUpdateOpen", "/Controls/guess2.xml") == "" || ub.GetSub("SiteUpdateOpen", "/Controls/guess2.xml") == "0")
            {
                #region 进入旧版更新
                if (st.p_basketve == 0)
                {
                    if (st.p_type == 1)
                    {
                        if (st.p_ison == 1)
                        {
                            bo = new TPR2.Collec.Footbo().GetBoView_kb_old(Convert.ToInt32(st.p_id), true);
                        }
                        else
                        {
                            bo = new TPR2.Collec.Footbo().GetBoView_kb_old(Convert.ToInt32(st.p_id), false);
                            //进行波胆更新
                            if (st.p_score != "")
                            {
                                bo = new TPR2.Collec.Footbd().FootbdPageHtml_kb_old(Convert.ToInt32(st.p_id));
                            }
                        }
                    }
                    else
                    {
                        if (st.p_ison == 1)
                            bo = new TPR2.Collec.Basketbo().GetBoView_kb_old(Convert.ToInt32(st.p_id), true);
                        else
                            bo = new TPR2.Collec.Basketbo().GetBoView_kb_old(Convert.ToInt32(st.p_id), false);
                    }
                }
                else if (st.p_basketve == 9)
                {
                    //载入页面更新足球上半场
                    if (st.p_type == 1)
                    {
                        string s = "";
                        if (st.p_ison == 1)
                        {
                            bo = new TPR2.Collec.Footbo().GetBoView_kb_old(Convert.ToInt32(st.p_id), true);
                            bo = new TPR2.Collec.FootFalf().FootFalfPageHtml_kb_old(Convert.ToInt32(st.p_id), true, ref s);
                        }
                        else
                        {
                            bo = new TPR2.Collec.FootFalf().FootFalfPageHtml_kb_old(Convert.ToInt32(st.p_id), false, ref s);
                        }
                    }
                }
                #endregion
            }
            else
            {
                #region 进入新版更新
                if (st.p_basketve == 0)
                {
                    #region 全场赛事更新
                    if (st.p_type == 1)
                    {
                        if (st.p_ison == 1)
                        {
                            bo = new TPR2.Collec.Footbo().GetBoView1(Convert.ToInt32(st.p_id), true);
                        }
                        else
                        {
                            bo = new TPR2.Collec.Footbo().GetBoView1(Convert.ToInt32(st.p_id), false);
                            //进行波胆更新
                            if (st.p_score != "")
                            {
                                new TPR2.Collec.Footbd().FootbdPageHtml(Convert.ToInt32(st.p_id));
                            }
                        }
                        bool b = new TPR2.Collec.Footbo().ChkFootFlag(1);
                        if (!b) { bo = ""; }
                    }
                    else
                    {
                        if (st.p_ison == 1)
                            bo = new TPR2.Collec.Basketbo().GetBoView1(Convert.ToInt32(st.p_id), true);
                        else
                            bo = new TPR2.Collec.Basketbo().GetBoView1(Convert.ToInt32(st.p_id), false);
                        bool b = new TPR2.Collec.Footbo().ChkFootFlag(2);
                        if (!b) { bo = ""; }
                    }
                    #endregion
                }
                else if (st.p_basketve == 9)
                {
                    #region 半场赛事更新
                    //载入页面更新足球上半场
                    if (st.p_type == 1)
                    {
                        string s = "";
                        if (st.p_ison == 1)
                        {
                            bo = new TPR2.Collec.FootFalf().FootFalfPageHtml1(Convert.ToInt32(st.p_id), true, ref s);
                        }
                        else
                        {
                            bo = new TPR2.Collec.FootFalf().FootFalfPageHtml1(Convert.ToInt32(st.p_id), false, ref s);
                        }
                        bool b = new TPR2.Collec.Footbo().ChkFootFlag(1);
                        if (!b) { bo = ""; }
                    }
                    #endregion
                }
                #endregion
            }
            //篮球半场和单节
            if (st.p_basketve == 1 || st.p_basketve == 3) { bo = "1"; }
        }
        #endregion

        #region 获得实体ID，并显示到界面
        //获得实体
        TPR2.Model.guess.BaList model = bll.GetModel(gid);
        //显示到标题
        Master.Title = model.p_one + "VS" + model.p_two;
        if (Request["act"] == "once")
        {
            OncePage(gid);
        }
        else if (Request["act"] == "analysis")
        {
            AnalysisPage(gid);
        }
        else if (Request["act"] == "Oddhistory")
        {
            OddhistoryPage(gid);
        }
        else if (Request["act"] == "score")
        {
            ScorePage(gid);
        }
        else
        {
            LoadPage(gid, bo, model);
        }

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append(Out.waplink(Utils.getUrl("myGuess.aspx?ptype=1"), "未开投注") + " ");
        builder.Append(Out.waplink(Utils.getUrl("myGuess.aspx?ptype=2"), "历史投注") + "<br />");
        builder.Append(Out.waplink(Utils.getUrl("default.aspx"), "返回球彩首页") + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(Out.waplink(Utils.getUrl("/default.aspx"), "首页") + "-");
        builder.Append(Out.waplink(Utils.getPage("default.aspx"), "上级") + "-");
        builder.Append(Out.waplink(Utils.getUrl("/bbs/game/default.aspx"), "游戏") + "");
        builder.Append(Out.Tab("</div>", ""));
        #endregion
    }

    private void LoadPage(int gid, string bo, TPR2.Model.guess.BaList model)
    {
        #region 默认显示球赛内容

        //根据会员本身的ID取会员实体
        int meid = new BCW.User.Users().GetUsId();
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        long gold = new BCW.BLL.User().GetGold(meid);
        builder.Append("您现在有" + Utils.ConvertGold(gold) + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        string OnceText = "@";
        if (model.p_ison == 1)
            OnceText = "@" + ub.Get("SiteGqText") + "";

        builder.Append("<a href=\"" + Utils.getUrl("default.aspx?showtype=1&amp;ptype=4&amp;fly=" + model.p_title + "") + "\">" + model.p_title + "</a> " + OnceText + " " + DT.FormatDate(Convert.ToDateTime(model.p_TPRtime), 1));
        //是否有波胆,显示波胆连接
        if (!string.IsNullOrEmpty(model.p_score))
        {
            builder.Append("" + Out.waplink(Utils.getUrl("showGuess.aspx?act=score&amp;gid=" + model.ID + ""), "[波胆]"));
        }

        //是否已完场
        if (model.p_result_one != null && model.p_result_two != null)
        {
            builder.Append("<br />完场比分:" + model.p_result_one + ":" + model.p_result_two + "");
        }
        else
        {
            if (model.p_TPRtime > DateTime.Now)
                builder.Append("<br />比赛状态:未");
            else
                builder.Append("<br />比赛状态:" + Convertp_once(model.p_once) + "");

            builder.Append(Out.waplink(Utils.getUrl("showGuess.aspx?gid=" + gid + ""), "[刷新]") + "");
            builder.Append("<br />即时比分:" + model.p_result_temp1 + ":" + model.p_result_temp2 + "");
        }
        //分析按钮
        builder.Append("" + Out.waplink(Utils.getUrl("showGuess.aspx?act=analysis&amp;gid=" + model.ID + ""), "[析]"));

        builder.Append(Out.Tab("</div>", "<br />"));

        #region 处理封盘状态
        int Min = 0;
        try
        {
            Min = Convert.ToInt32(model.p_once.ToString().Replace("'", "").Replace("+", ""));
        }
        catch
        {
        }
        if (model.p_type == 1)
        {
            if (Min > 41 && Min < 46 || Min > 87 || (model.p_once == "中" && model.p_basketve == 9))
            {
                model.p_isluck = 1;
            }
        }
        #endregion

        #region 红牌显示
        string hp_one = "";
        string hp_two = "";
        if (model.p_type == 1)
        {
            if (model.p_hp_one > 0)
                hp_one = "<img src=\"/Files/sys/guess/redcard" + model.p_hp_one + ".gif\" alt=\"红" + model.p_hp_one + "\"/>";

            if (model.p_hp_two > 0)
                hp_two = "<img src=\"/Files/sys/guess/redcard" + model.p_hp_two + ".gif\" alt=\"红" + model.p_hp_two + "\"/>";

        }
        #endregion

        #region 让球盘
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("〓让球盘〓");
        if (model.p_basketve == 0)
        {
            builder.Append("" + Out.waplink(Utils.getUrl("showGuess.aspx?act=Oddhistory&amp;t=1&amp;gid=" + model.ID + ""), "[析]"));
        }
        else if (model.p_basketve == 9)
        {
            builder.Append("" + Out.waplink(Utils.getUrl("showGuess.aspx?act=Oddhistory&amp;t=4&amp;gid=" + model.ID + ""), "[析]"));
        }
        bool rqp = false;
        //主队
        if (((model.p_ison == 1 && model.p_isluckone == 1) || model.p_isluck == 1) && model.p_active == 0)
        {
            builder.Append("<b>(封)</b>");
            rqp = true;
        }
        else if (model.p_oncetime < DateTime.Now)
        {
            builder.Append("<b>(封)</b>");
            rqp = true;
        }
        else if (bo == "")
        {
            builder.Append("<b>(封)</b>");
            rqp = true;
        }
        if (rqp)
        {
            builder.Append("<br />" + model.p_one + "" + hp_one + "(" + Convert.ToDouble(model.p_one_lu) + ")");
        }
        else
        {
            builder.Append("<br />" + Out.waplink(Utils.getUrl("payGuess.aspx?gid=" + model.ID + "&amp;p=1"), model.p_one + "" + hp_one + "(" + Convert.ToDouble(model.p_one_lu) + ")"));
        }
        //=====================

        //让球
        if (model.p_type == 1)
            builder.Append("<br />" + GCK.getZqPn(Convert.ToInt32(model.p_pn)) + "" + GCK.getPkName(Convert.ToInt32(model.p_pk)) + "");
        else
            builder.Append("<br />" + Convert.ToDouble(model.p_pk) + "");
        //=====================

        //客队
        if (rqp)
        {
            builder.Append("<br />" + model.p_two + "" + hp_two + "(" + Convert.ToDouble(model.p_two_lu) + ")");
        }
        else
        {
            builder.Append("<br />" + Out.waplink(Utils.getUrl("payGuess.aspx?gid=" + model.ID + "&amp;p=2"), model.p_two + "" + hp_two + "(" + Convert.ToDouble(model.p_two_lu) + ")"));
        }
        //=====================
        #endregion

        #region 大小盘
        if (model.p_big_lu != 0 && model.p_big_lu != -1 && model.p_dx_pk != 0)
        {
            bool dxp = false;
            builder.Append("<br />〓大小盘〓");
            if (model.p_basketve == 0)
            {
                builder.Append("" + Out.waplink(Utils.getUrl("showGuess.aspx?act=Oddhistory&amp;t=2&amp;gid=" + model.ID + ""), "[析]"));
            }
            else if (model.p_basketve == 9)
            {
                builder.Append("" + Out.waplink(Utils.getUrl("showGuess.aspx?act=Oddhistory&amp;t=5&amp;gid=" + model.ID + ""), "[析]"));
            }
            if (((model.p_ison == 1 && model.p_islucktwo == 1) || model.p_isluck == 1) && model.p_active == 0)
            {
                builder.Append("<b>(封)</b>");
                dxp = true;
            }
            else if (model.p_oncetime < DateTime.Now)
            {
                builder.Append("<b>(封)</b>");
                dxp = true;
            }
            else if (bo == "")
            {
                builder.Append("<b>(封)</b>");
                dxp = true;
            }
            if (dxp)
            {
                builder.Append("<br />" + "大(" + Convert.ToDouble(model.p_big_lu) + ")");
            }
            else
            {
                builder.Append("<br />" + Out.waplink(Utils.getUrl("payGuess.aspx?gid=" + model.ID + "&amp;p=3"), "大(" + Convert.ToDouble(model.p_big_lu) + ")"));
            }

            if (model.p_type == 1)
                builder.Append(GCK.getDxPkName(Convert.ToInt32(model.p_dx_pk)));
            else
                builder.Append(Convert.ToDouble(model.p_dx_pk));
            if (dxp)
            {
                builder.Append("小(" + Convert.ToDouble(model.p_small_lu) + ")");
            }
            else
            {
                builder.Append(Out.waplink(Utils.getUrl("payGuess.aspx?gid=" + model.ID + "&amp;p=4"), "小(" + Convert.ToDouble(model.p_small_lu) + ")"));
            }
        }
        #endregion

        #region 标准盘
        if (ub.GetSub("SiteIsbz", "/Controls/guess2.xml") != "1")
        {
            if (model.p_bzs_lu != 0 && model.p_bzs_lu != -1)
            {
                bool bzp = false;

                builder.Append("<br />〓标准盘〓");
                if (model.p_basketve == 0)
                {
                    builder.Append("" + Out.waplink(Utils.getUrl("showGuess.aspx?act=Oddhistory&amp;t=3&amp;gid=" + model.ID + ""), "[析]"));
                }
                else if (model.p_basketve == 9)
                {
                    builder.Append("" + Out.waplink(Utils.getUrl("showGuess.aspx?act=Oddhistory&amp;t=6&amp;gid=" + model.ID + ""), "[析]"));
                }
                if (((model.p_ison == 1 && model.p_isluckthr == 1) || model.p_isluck == 1) && model.p_active == 0)
                {
                    builder.Append("<b>(封)</b>");
                    bzp = true;
                }
                else if (model.p_oncetime < DateTime.Now)
                {
                    builder.Append("<b>(封)</b>");
                    bzp = true;
                }
                else if (bo == "")
                {
                    builder.Append("<b>(封)</b>");
                    bzp = true;
                }
                if (bzp)
                {
                    builder.Append("<br />" + "主胜(" + Convert.ToDouble(model.p_bzs_lu) + ")");
                    builder.Append("<br />" + "平手(" + Convert.ToDouble(model.p_bzp_lu) + ")");
                    builder.Append("<br />" + "客胜(" + Convert.ToDouble(model.p_bzx_lu) + ")");
                }
                else
                {
                    builder.Append("<br />" + Out.waplink(Utils.getUrl("payGuess.aspx?gid=" + model.ID + "&amp;p=5"), "主胜(" + Convert.ToDouble(model.p_bzs_lu) + ")"));
                    builder.Append("<br />" + Out.waplink(Utils.getUrl("payGuess.aspx?gid=" + model.ID + "&amp;p=6"), "平手(" + Convert.ToDouble(model.p_bzp_lu) + ")"));
                    builder.Append("<br />" + Out.waplink(Utils.getUrl("payGuess.aspx?gid=" + model.ID + "&amp;p=7"), "客胜(" + Convert.ToDouble(model.p_bzx_lu) + ")"));
                }
            }
        }
        #endregion

        builder.Append(Out.Tab("</div>", Out.Hr()));

        string purl = "[url=/bbs/guess2/showguess.aspx?gid=" + gid + "]" + model.p_title + ":" + model.p_one + "VS" + model.p_two + "[/url]";
        string strName = "purl,act,backurl";
        string strValu = "" + purl + "'recommend'" + Utils.PostPage(1) + "";
        string strOthe = "分享给好友,/bbs/guest.aspx,post,1,red";
        builder.Append(Out.wapform(strName, strValu, strOthe));

        //builder.Append("系统在" + second + "秒前更新了数据");

        if (model.p_ison == 1)
        {
            builder.Append(Out.Tab("<div>", Out.Hr()));
            //修改显示内容

            builder.Append("提示：比赛开始后仍可下注的称为" + ub.Get("SiteGqText") + " " + Out.waplink(Utils.getUrl("help.aspx"), "球彩规则") + "");
            if (model.p_type == 1)
            {
                builder.Append("<br />比赛状态时间仅供参考，与分析里的时间有可能不一致");
            }
            else
            {
                builder.Append("<br />比分仅供参考，即时比分与直播比分（或8波、比分网）有可能不一致");
            }
            builder.Append(Out.Tab("</div>", ""));
        }
        #endregion
    }

    private void OncePage(int gid)
    {
        #region 比分参考
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("" + ub.Get("SiteGqText") + "比分参考");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        //得到比分更新时间集合
        string stronce = new TPR2.BLL.guess.BaList().Getp_temptimes(gid);
        if (stronce != "")
        {
            builder.Append("" + stronce.Replace("|", "<br />") + "");
        }
        else
        {
            builder.Append("暂无比分记录..");
        }
        builder.Append(Out.Tab("</div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.waplink(Utils.getUrl("showGuess.aspx?gid=" + gid + ""), "返回上级") + "");
        builder.Append(Out.Tab("</div>", ""));
        #endregion
    }

    private void ScorePage(int gid)
    {
        #region 波胆分析
        TPR2.BLL.guess.BaList bll = new TPR2.BLL.guess.BaList();
        TPR2.Model.guess.BaList model = bll.GetModel(gid);

        if (string.IsNullOrEmpty(model.p_score))
        {
            Utils.Error("不存在的波胆盘或已关闭波胆投注", "");
        }
        //根据会员本身的ID取会员实体
        int meid = new BCW.User.Users().GetUsId();
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        long gold = new BCW.BLL.User().GetGold(meid);
        builder.Append("您现在有" + Utils.ConvertGold(gold) + "" + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        string OnceText = "@";
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx?showtype=1&amp;ptype=4&amp;fly=" + model.p_title + "") + "\">" + model.p_title + "</a>:" + model.p_one + "VS" + model.p_two + " " + OnceText + " " + DT.FormatDate(Convert.ToDateTime(model.p_TPRtime), 1));
        builder.Append(Out.waplink(Utils.getUrl("showGuess.aspx?gid=" + gid + "&amp;backurl=" + Utils.getPage(0) + ""), "[普通盘]") + "");

        if (model.p_result_one != null && model.p_result_two != null)
        {
            builder.Append("<br />完场比分:" + model.p_result_one + ":" + model.p_result_two + "");
        }
        else
        {
            builder.Append("<br />比赛状态:" + Convertp_once(model.p_once) + "");
            builder.Append(Out.waplink(Utils.getUrl("showGuess.aspx?act=score&amp;gid=" + gid + ""), "[刷新]") + "");
            builder.Append("<br />即时比分:" + model.p_result_temp1 + ":" + model.p_result_temp2 + "");
        }

        builder.Append("" + Out.waplink(Utils.getUrl("showGuess.aspx?act=analysis&amp;gid=" + model.ID + "&amp;backurl=" + Utils.PostPage(1) + ""), "[析]"));
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("〓波胆盘〓<br />");
        builder.Append("主胜↔客胜↔打和<br />");
        string[] score = model.p_score.Split(',');

        for (int i = 0; i < score.Length; i++)
        {
            string[] Temp = score[i].Split('|');
            string stype = Temp[0].Replace(":", "");
            string stext = Temp[0].Replace("5z", "主净胜5球或以上").Replace("5k", "客净胜5球或以上").Replace("ot", "其他胆");
            if (i >= 25 && i <= 26)
            {
                if (Temp[1] != "-1")
                {
                    builder.Append("" + stext + "");
                    builder.Append("<a href=\"" + Utils.getUrl("payGuess2.aspx?gid=" + model.ID + "&amp;p=" + Temp[0].Replace(":", "") + "") + "\">[" + Temp[1] + "倍]</a> ");
                }
            }
            else
            {
                builder.Append("" + stext + "");
                if (Temp[1] != "-1")
                    builder.Append("<a href=\"" + Utils.getUrl("payGuess2.aspx?gid=" + model.ID + "&amp;p=" + Temp[0].Replace(":", "") + "") + "\">[" + Temp[1] + "倍]</a> ");
                else
                    builder.Append("[缺胆] ");
            }

            if (i < 15 && (i + 1) % 3 == 0)
                builder.Append("<br />");
            else if (i >= 15 && i <= 25 && (i) % 2 == 0)
                builder.Append("<br />");
            else if (i >= 25 && i < 27)
                if (i >= 25 && i <= 26)
                {
                    if (Temp[1] != "-1")
                    {
                        builder.Append("<br />");
                    }
                }
                else { builder.Append("<br />"); }
        }

        builder.Append(Out.Tab("</div>", Out.Hr()));
        string purl = "[url=/bbs/guess2/showguess.aspx?gid=" + gid + "]" + model.p_title + ":" + model.p_one + "VS" + model.p_two + "[/url]";
        string strName = "purl,act,backurl";
        string strValu = "" + purl + "'recommend'" + Utils.PostPage(1) + "";
        string strOthe = "分享给好友,/bbs/guest.aspx,post,1,red";
        builder.Append(Out.wapform(strName, strValu, strOthe));
        #endregion
    }

    #region 3g8bo球赛分析数据
    private void AnalysisPage(int gid)
    {
        #region 赛事分析 点击 析 字的操作 有抓取
        //获得实体
        TPR2.BLL.guess.BaList bll = new TPR2.BLL.guess.BaList();
        TPR2.Model.guess.BaList model = bll.GetModel(gid);
        Master.Title = "赛事分析";
        builder.Append(Out.Tab("<div class=\"title\">即时赛事分析</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        string strAnal = string.Empty;
        //类型为1 是足球
        if (model.p_type == 1)
        {
            //获取分析数据 进入8bo抓取【足球】数据
            strAnal = new TPR2.Collec.Analysis().GetAnalysisFoot(0, Convert.ToInt32(model.p_id), gid);
            if (strAnal == "暂无数据。")
            {
                strAnal = new TPR2.Collec.Analysis().GetAnalysisFoot(1, Convert.ToInt32(model.p_id), gid);
            }
        }
        else
        {
            //获取分析数据 进入8bo抓取【篮球】数据
            strAnal = new TPR2.Collec.Analysis().GetAnalysisBasket(0, Convert.ToInt32(model.p_id), gid);

            if (strAnal == "暂无数据。")
            {
                strAnal = new TPR2.Collec.Analysis().GetAnalysisBasket(1, Convert.ToInt32(model.p_id), gid);
            }

        }
        if (strAnal != "")
        {
            builder.Append("" + strAnal + "");
        }
        else
        {
            builder.Append("暂无记录..");
        }
        builder.Append(Out.Tab("</div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.waplink(Utils.getPage("showGuess.aspx?gid=" + gid + ""), "返回上级") + "");
        builder.Append(Out.Tab("</div>", ""));
        #endregion
    }
    #endregion

    #region 球赛盘口历史数据 OddhistoryPage
    /// <summary>
    /// 球赛盘口历史数据
    /// </summary>
    /// <param name="gid"></param>
    private void OddhistoryPage(int gid)
    {
        #region 赛事分析 点击 析 字的操作 有抓取
        //获得实体
        TPR2.BLL.guess.BaList bll = new TPR2.BLL.guess.BaList();
        TPR2.Model.guess.BaList model = bll.GetModel(gid);
        //类型（1全场让球盘，2全场大小盘，3全场标准盘；4半场让球盘，5半场大小盘，6半场标准盘；7第一节让球盘，8第一节大小盘；9第二节让球盘，10第二节大小盘；11第三节让球盘，12第三节大小盘）
        int t = Utils.ParseInt(Utils.GetRequest("t", "get", 2, @"^[0-12]*$", "类型无效"));

        if (model != null)
        {
            IList<TPR2.Model.guess.TBaListNew_History> models = bll.GetHistory(int.Parse(model.p_id.ToString()), t);
            Master.Title = model.p_one + "vs" + model.p_two + " " + TPR2.BLL.guess.BaList.TYPE_NAMES[t + 1];
            builder.Append(Out.Tab("<div class=\"title\">" + Out.waplink(Utils.getUrl("default.aspx"), "球彩首页") + ">" + Out.waplink(Utils.getUrl("showGuess.aspx?gid=" + gid + ""), "返回赛事") + "</div>", Out.waplink(Utils.getUrl("default.aspx"), "球彩首页") + ">" + Out.waplink(Utils.getUrl("showGuess.aspx?gid=" + gid + ""), "返回赛事")));
            builder.Append(Out.Tab("<div>", "<br />"));
            string strAnal = string.Empty;
            builder.Append(Out.SysUBB(model.p_one + " VS " + model.p_two + " " + TPR2.BLL.guess.BaList.TYPE_NAMES[t + 1] + "<br />"));
            builder.Append("开赛日期:" + model.p_TPRtime + "<br />");
            builder.Append(Out.SysUBB("注：数据仅作参考，不完全准确，如有错误请与[url=/bbs/uinfo.aspx?uid=10086]客服ID10086[/url]反映"));

            builder.Append("<style>table{border-collapse:collapse;align-text:center;border:solid 1px #d7d7d7;}table tr td{padding:0px 3px;border:solid 1px #d7d7d7;text-align:center;}</style>");
            builder.Append("<table>");
            if (t == 1 || t == 4)
            {
                builder.Append("<tr style=\"text-align:center;font-weight:bold;background-color:#1c5e84;color:#ffffff\"><td>序号</td><td>比分</td><td>时间</td><td>主队</td><td>让球</td><td>客队</td><td>变化时间</td><td>状态</td></tr>");
            }
            else if (t == 2 || t == 5)
            {
                builder.Append("<tr style=\"text-align:center;font-weight:bold;background-color:#1c5e84;color:#ffffff\"><td>序号</td><td>比分</td><td>时间</td><td>大球</td><td>大小球</td><td>小球</td><td>变化时间</td><td>状态</td></tr>");
            }
            else if (t == 3 || t == 6)
            {
                builder.Append("<tr style=\"text-align:center;font-weight:bold;background-color:#1c5e84;color:#ffffff\"><td>序号</td><td>比分</td><td>时间</td><td>胜</td><td>平</td><td>负</td><td>变化时间</td><td>状态</td></tr>");
            }
            if (models != null)
            {
                if (models.Count > 0)
                {
                    for (int i = 0; i < models.Count; i++)
                    {
                        builder.Append("<tr>");

                        TPR2.Model.guess.TBaListNew_History m = models[i];

                        builder.Append("<td>" + (models.Count - i) + "</td>");
                        builder.Append("<td>" + Out.SysUBB("[B]" + m.result + "[/B]") + "</td>");
                        builder.Append("<td>" + m.remark + "</td>");
                        if (m.lockflag == 1)
                        {
                            builder.Append("<td colspan=\"3\">" + Out.SysUBB("[绿]封[/绿]") + "</td>");
                        }
                        else
                        {
                            builder.Append("<td>" + Math.Round(double.Parse(m.v1.ToString()), 2).ToString() + "</td>");
                            string sr = "";
                            if (m.vs < 0) { sr = "[红]受[/红]"; }
                            if (t == 1 || t == 4)
                            {
                                builder.Append("<td>" + Out.SysUBB("[蓝]" + sr + GCK.getPkName(Math.Abs(Convert.ToInt32(m.vs))) + "[/蓝]") + "</td>");
                            }
                            else if (t == 2 || t == 5)
                            {
                                builder.Append("<td>" + Out.SysUBB("[蓝]" + sr + GCK.getDxPkName(Math.Abs(Convert.ToInt32(m.vs))) + "[/蓝]") + "</td>");
                            }
                            else if (t == 3 || t == 6)
                            {
                                builder.Append("<td>" + Out.SysUBB("[蓝]" + Convert.ToInt32(m.vs) + "[/蓝]") + "</td>");
                            }
                            builder.Append("<td>" + Math.Round(double.Parse(m.v2.ToString()), 2).ToString() + "</td>");
                        }

                        builder.Append("<td>" + Out.SysUBB("" + m.downloadtime.ToString("HH:mm:ss") + "") + "</td>");

                        if (m.zdflag == 0)
                        {
                            if (DateTime.Parse(model.p_TPRtime.ToString()).AddMonths(-1) > m.downloadtime)
                            {
                                builder.Append("<td>" + Out.SysUBB("[绿]临[/绿]") + "</td>");
                            }
                            else
                            {
                                builder.Append("<td>" + Out.SysUBB("[蓝]即[/蓝]") + "</td>");
                            }
                        }
                        else
                        {
                            builder.Append("<td>" + Out.SysUBB("[红]滚[/红]") + "</td>");
                        }
                        builder.Append("</tr>");
                    }
                }
                else
                {
                    builder.Append("暂无记录..");
                }
            }
            else
            {
                builder.Append("暂无记录..");
            }
            builder.Append("</table>");
        }
        else
        {
            builder.Append("不存在的赛事..");
        }

        builder.Append(Out.Tab("</div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.waplink(Utils.getPage("showGuess.aspx?gid=" + gid + ""), "返回上级") + "");
        builder.Append(Out.Tab("</div>", ""));
        #endregion
    }
    #endregion

    private string Convertp_once(string p_once)
    {
        string once = "";
        if (!string.IsNullOrEmpty(p_once))
        {
            if (p_once.Contains("'") && !p_once.Contains("+"))
            {
                try
                {
                    int min = Convert.ToInt32(p_once.Replace("'", ""));
                    if (min > 5)
                        //once = (min - 3) + "'";
                        once = (min - 0) + "'";
                    else
                        once = p_once;
                }
                catch
                {
                    once = p_once;
                }
            }
            else
            {
                once = p_once;
            }
        }
        return once;

        //return p_once;
    }

}
