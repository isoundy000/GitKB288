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

/// <summary>
/// 增加其他胆下注修改
/// 黄国军 20160706
/// 增加通过代理模式抓取/// 
/// 黄国军20160509
/// </summary>
public partial class Manage_guess2_ShowGuess : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        int gid = Utils.ParseInt(Utils.GetRequest("gid", "all", 2, @"^[0-9]*$", "竞猜ID无效"));
        string act = Utils.GetRequest("act", "all", 1, "", "");
        ///抓取更新标记
        string jc = Utils.GetRequest("jc", "get", 1, "", "");
        ///开奖状态更新标记
        string dr = Utils.GetRequest("dr", "get", 1, "", "");

        TPR2.BLL.guess.BaList bll = new TPR2.BLL.guess.BaList();
        TPR2.Model.guess.BaList st = bll.GetModel(gid);
        if (st == null)
        {
            Utils.Error("不存在的记录", "");
        }

        #region 立即更新水位 访问8bo

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
                    }
                    else
                    {
                        if (st.p_ison == 1)
                            bo = new TPR2.Collec.Basketbo().GetBoView1(Convert.ToInt32(st.p_id), true);
                        else
                            bo = new TPR2.Collec.Basketbo().GetBoView1(Convert.ToInt32(st.p_id), false);
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
                            bo = new TPR2.Collec.FootFalf().FootFalfPageHtml1(Convert.ToInt32(st.p_id), true, ref s);
                        }
                        else
                        {
                            bo = new TPR2.Collec.FootFalf().FootFalfPageHtml1(Convert.ToInt32(st.p_id), false, ref s);
                        }
                    }
                }

                #endregion
            }
            //篮球半场和单节
            if (st.p_basketve == 1 || st.p_basketve == 3) { bo = "1"; }
        }
        #endregion

        //更新封盘不封盘

        if (act == "luck1")
            new TPR2.BLL.guess.BaList().Updatep_isluck2(gid, 1, 1);
        else if (act == "luck2")
            new TPR2.BLL.guess.BaList().Updatep_isluck2(gid, 1, 2);
        else if (act == "luck3")
            new TPR2.BLL.guess.BaList().Updatep_isluck2(gid, 1, 3);
        else if (act == "noluck1")
            new TPR2.BLL.guess.BaList().Updatep_isluck2(gid, 0, 1);
        else if (act == "noluck2")
            new TPR2.BLL.guess.BaList().Updatep_isluck2(gid, 0, 2);
        else if (act == "noluck3")
            new TPR2.BLL.guess.BaList().Updatep_isluck2(gid, 0, 3);


        TPR2.Model.guess.BaList model = bll.GetModel(gid);

        Master.Title = model.p_one + "VS" + model.p_two;

        #region 更新隐藏与显示
        //更新隐藏与显示
        if (act == "yes")
        {
            //游戏日志记录
            string[] p_pageArr = { "act", "gid" };
            BCW.User.GameLog.GameLogGetPage(1, Utils.getPageUrl(), p_pageArr, "后台管理员" + ManageId + "号将赛事" + model.p_one + "VS" + model.p_two + "(" + gid + ")开放显示", gid);
            model.p_del = 0;
            new TPR2.BLL.guess.BaList().Updatep_del(model);
        }
        else if (act == "no")
        {

            //游戏日志记录
            string[] p_pageArr = { "act", "gid" };
            BCW.User.GameLog.GameLogGetPage(1, Utils.getPageUrl(), p_pageArr, "后台管理员" + ManageId + "号将赛事" + model.p_one + "VS" + model.p_two + "(" + gid + ")隐藏显示", gid);
            model.p_del = 1;
            new TPR2.BLL.guess.BaList().Updatep_del(model);
        }
        #endregion

        #region 更新抓取与不抓取
        //更新抓取与不抓取
        if (jc == "yes")
        {
            //游戏日志记录
            string[] p_pageArr = { "jc", "gid" };
            BCW.User.GameLog.GameLogGetPage(1, Utils.getPageUrl(), p_pageArr, "后台管理员" + ManageId + "号将赛事" + model.p_one + "VS" + model.p_two + "(" + gid + ")开启抓取", gid);
            model.p_jc = 0;
            new TPR2.BLL.guess.BaList().Updatep_jc(model);
        }
        else if (jc == "no")
        {
            //游戏日志记录
            string[] p_pageArr = { "jc", "gid" };
            BCW.User.GameLog.GameLogGetPage(1, Utils.getPageUrl(), p_pageArr, "后台管理员" + ManageId + "号将赛事" + model.p_one + "VS" + model.p_two + "(" + gid + ")关闭抓取", gid);
            model.p_jc = 1;
            new TPR2.BLL.guess.BaList().Updatep_jc(model);
        }
        #endregion

        #region 更新开奖方式:自动或人工
        //更新抓取与不抓取
        if (dr == "0")
        {
            //游戏日志记录
            string[] p_pageArr = { "dr", "gid" };
            BCW.User.GameLog.GameLogGetPage(1, Utils.getPageUrl(), p_pageArr, "后台管理员" + ManageId + "号将赛事" + model.p_one + "VS" + model.p_two + "(" + gid + ")设置为自动开奖", gid);
            model.p_dr = 0;
            new TPR2.BLL.guess.BaList().Updatep_dr(model);
        }
        else if (dr == "1")
        {
            //游戏日志记录
            string[] p_pageArr = { "dr", "gid" };
            BCW.User.GameLog.GameLogGetPage(1, Utils.getPageUrl(), p_pageArr, "后台管理员" + ManageId + "号将赛事" + model.p_one + "VS" + model.p_two + "(" + gid + ")设置为人工开奖", gid);
            model.p_dr = 1;
            new TPR2.BLL.guess.BaList().Updatep_dr(model);
        }
        #endregion

        #region 删除赛事
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
                BCW.User.GameLog.GameLogGetPage(1, Utils.getPageUrl(), p_pageArr, "后台管理员" + ManageId + "号删除赛事" + model.p_one + "VS" + model.p_two + "(" + gid + ")", gid);

                new TPR2.BLL.guess.BaList().Delete(gid);
                if (act == "delok2")
                {
                    TPR2.BLL.guess.BaPay bll2 = new TPR2.BLL.guess.BaPay();
                    new TPR2.BLL.guess.BaPay().Deletebcid(gid);
                }
                Utils.Success("删除赛事", "删除赛事成功..", Utils.getUrl("default.aspx"), "1");
            }
        }
        #endregion

        #region 转换成滚球模式
        //转换成" + ub.Get("SiteGqText") + "模式
        else if (act == "once" || act == "onceok")
        {
            if (act == "once")
            {
                string p_oncetime = string.Empty;
                if (string.IsNullOrEmpty(model.p_oncetime.ToString()))
                    p_oncetime = DT.FormatDate(Convert.ToDateTime(model.p_TPRtime).AddMinutes(130), 0);
                else
                    p_oncetime = DT.FormatDate(Convert.ToDateTime(model.p_oncetime), 0);

                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("确定要转换成" + ub.Get("SiteGqText") + "下注模式吗");
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
                DateTime oncetime = Utils.ParseTime(Utils.GetRequest("oncetime", "all", 2, DT.RegexTime, "请正确填写封盘时间"));

                if (Convert.ToDateTime(model.p_TPRtime) > oncetime)
                {
                    Utils.Error("封盘时间应大于开赛时间", "");
                }
                //游戏日志记录
                string[] p_pageArr = { "oncetime", "act", "gid" };
                BCW.User.GameLog.GameLogGetPage(1, Utils.getPageUrl(), p_pageArr, "后台管理员" + ManageId + "号编辑赛事" + model.p_one + "VS" + model.p_two + "(" + gid + ")成为" + ub.Get("SiteGqText") + "", gid);

                new TPR2.BLL.guess.BaList().FootOnceType(gid, oncetime);
                Utils.Success("转换" + ub.Get("SiteGqText") + "", "转换成功..", Utils.getUrl("showguess.aspx?gid=" + gid + ""), "1");
            }

        }
        #endregion

        #region 赛事分析
        else if (act == "analysis")
        {
            Master.Title = "赛事分析";
            builder.Append(Out.Tab("<div class=\"title\">即时赛事分析</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            string strAnal = string.Empty;
            if (model.p_type == 1)
            {
                strAnal = new TPR2.Collec.Analysis().GetAnalysisFoot(0, Convert.ToInt32(model.p_id), gid);

                if (strAnal == "暂无数据。")
                {
                    strAnal = new TPR2.Collec.Analysis().GetAnalysisFoot(1, Convert.ToInt32(model.p_id), gid);
                }
            }
            else
            {
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
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        #endregion

        #region 波胆设置
        else if (act == "score")
        {
            Master.Title = "波胆盘设置";

            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append(model.p_title + ":" + model.p_one + "VS" + model.p_two);
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("〓波胆盘〓<br />");
            builder.Append("主胜↔客胜↔打和");
            builder.Append(Out.Tab("</div>", "<br />"));


            string strText = "1:0,0:1,0:0,/,2:0,0:2,1:1,/,3:0,0:3,2:2,/,4:0,0:4,3:3,/,2:1,1:2,4:4,/,3:1,1:3,/,4:1,1:4,/,3:2,2:3,/,4:2,2:4,/,4:3,3:4,/,主净胜5球或以上,/,客净胜5球或以上,/,其他胆,,,,,,";
            string strName = "score10,score01,score00,,score20,score02,score11,,score30,score03,score22,,score40,score04,score33,,score21,score12,score44,,score31,score13,,score41,score14,,score32,score23,,score42,score24,,score43,score34,,score5z,,score5k,,scoreot,gid,act";
            string strType = "small,small,small,hr,small,small,small,hr,small,small,small,hr,small,small,small,hr,small,small,small,hr,small,small,hr,small,small,hr,small,small,hr,small,small,hr,small,small,hr,small,hr,small,hr,small,hidden,hidden";

            string strValu = "";
            if (!string.IsNullOrEmpty(model.p_score))
            {
                string[] score = model.p_score.Split(',');
                string other = "15";
                if (score.Length > 27)
                {
                    other = score[27].Split('|')[1];
                }
                strValu = "" + score[0].Split('|')[1] + "'" + score[1].Split('|')[1] + "'" + score[2].Split('|')[1] + "''" + score[3].Split('|')[1] + "'" + score[4].Split('|')[1] + "'" + score[5].Split('|')[1] + "''" + score[6].Split('|')[1] + "'" + score[7].Split('|')[1] + "'" + score[8].Split('|')[1] + "''" + score[9].Split('|')[1] + "'" + score[10].Split('|')[1] + "'" + score[11].Split('|')[1] + "''" + score[12].Split('|')[1] + "'" + score[13].Split('|')[1] + "'" + score[14].Split('|')[1] + "''" + score[15].Split('|')[1] + "'" + score[16].Split('|')[1] + "''" + score[17].Split('|')[1] + "'" + score[18].Split('|')[1] + "''" + score[19].Split('|')[1] + "'" + score[20].Split('|')[1] + "''" + score[21].Split('|')[1] + "'" + score[22].Split('|')[1] + "''" + score[23].Split('|')[1] + "'" + score[24].Split('|')[1] + "''" + score[25].Split('|')[1] + "''" + score[26].Split('|')[1] + "''" + other + "'" + gid + "'scoreok";
            }
            else
            {
                strValu = "''''''''''''''''''''''''''''''''''''''''" + gid + "'scoreok";
            }
            string strEmpt = "true,true,true,,true,true,true,,true,true,true,,true,true,true,,true,true,true,,true,true,,true,true,,true,true,,true,true,,true,true,,true,,true,true,true,true,true,true,false,false";
            string strIdea = "倍 '倍 '倍 ''倍 '倍 '倍 ''倍 '倍 '倍 ''倍 '倍 '倍 ''倍 '倍 '倍 ''倍 '倍 ''倍 '倍 ''倍 '倍 ''倍 '倍 ''倍 '倍 ''倍 ''倍 '''倍 ''倍 '倍 ''倍 ''倍 '''倍 ''倍 '倍 ''倍 ''倍 '''倍 ''倍 '倍 ''倍 ''倍 ''|/";
            string strOthe = "确定编辑,showGuess.aspx,post,3,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("温馨提示：当某项不开放投注或无赔率时，请填写-1<br />");
            builder.Append(Out.waplink(Utils.getUrl("showGuess.aspx?act=scoreok2&amp;gid=" + gid + ""), "[取消本场波胆]") + "<br />");
            builder.Append(Out.waplink(Utils.getUrl("showGuess.aspx?gid=" + gid + ""), "返回上级") + "");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        #endregion

        #region 游戏日志记录
        else if (act == "scoreok")
        {
            //游戏日志记录
            string[] p_pageArr = { "act", "gid", "score10", "score01", "score00", "score20", "score02", "score11", "score30", "score03", "score22", "score40", "score04", "score33", "score21", "score12", "score44", "score31", "score13", "score41", "score14", "score32", "score23", "score42", "score24", "score43", "score34", "score5z", "score5k", "scoreot" };
            BCW.User.GameLog.GameLogPage(1, Utils.getPageUrl(), p_pageArr, "后台管理员" + ManageId + "号将赛事编辑波胆" + model.p_one + "VS" + model.p_two + "(" + gid + ")", gid);

            decimal score10 = Convert.ToDecimal(Utils.GetRequest("score10", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写1赔率,小数点后保留1-2位"));
            decimal score01 = Convert.ToDecimal(Utils.GetRequest("score01", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写2赔率,小数点后保留1-2位"));
            decimal score00 = Convert.ToDecimal(Utils.GetRequest("score00", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写3赔率,小数点后保留1-2位"));
            decimal score20 = Convert.ToDecimal(Utils.GetRequest("score20", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写4赔率,小数点后保留1-2位"));
            decimal score02 = Convert.ToDecimal(Utils.GetRequest("score02", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写5赔率,小数点后保留1-2位"));
            decimal score11 = Convert.ToDecimal(Utils.GetRequest("score11", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写6赔率,小数点后保留1-2位"));
            decimal score30 = Convert.ToDecimal(Utils.GetRequest("score30", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写7赔率,小数点后保留1-2位"));
            decimal score03 = Convert.ToDecimal(Utils.GetRequest("score03", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写8赔率,小数点后保留1-2位"));
            decimal score22 = Convert.ToDecimal(Utils.GetRequest("score22", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写9赔率,小数点后保留1-2位"));
            decimal score40 = Convert.ToDecimal(Utils.GetRequest("score40", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写10赔率,小数点后保留1-2位"));
            decimal score04 = Convert.ToDecimal(Utils.GetRequest("score04", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写11赔率,小数点后保留1-2位"));
            decimal score33 = Convert.ToDecimal(Utils.GetRequest("score33", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写12赔率,小数点后保留1-2位"));
            decimal score21 = Convert.ToDecimal(Utils.GetRequest("score21", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写13赔率,小数点后保留1-2位"));
            decimal score12 = Convert.ToDecimal(Utils.GetRequest("score12", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写14赔率,小数点后保留1-2位"));
            decimal score44 = Convert.ToDecimal(Utils.GetRequest("score44", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写15赔率,小数点后保留1-2位"));
            decimal score31 = Convert.ToDecimal(Utils.GetRequest("score31", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写16赔率,小数点后保留1-2位"));
            decimal score13 = Convert.ToDecimal(Utils.GetRequest("score13", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写17赔率,小数点后保留1-2位"));
            decimal score41 = Convert.ToDecimal(Utils.GetRequest("score41", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写18赔率,小数点后保留1-2位"));
            decimal score14 = Convert.ToDecimal(Utils.GetRequest("score14", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写19赔率,小数点后保留1-2位"));
            decimal score32 = Convert.ToDecimal(Utils.GetRequest("score32", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写20赔率,小数点后保留1-2位"));
            decimal score23 = Convert.ToDecimal(Utils.GetRequest("score23", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写21赔率,小数点后保留1-2位"));
            decimal score42 = Convert.ToDecimal(Utils.GetRequest("score42", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写22赔率,小数点后保留1-2位"));
            decimal score24 = Convert.ToDecimal(Utils.GetRequest("score24", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写23赔率,小数点后保留1-2位"));
            decimal score43 = Convert.ToDecimal(Utils.GetRequest("score43", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写24赔率,小数点后保留1-2位"));
            decimal score34 = Convert.ToDecimal(Utils.GetRequest("score34", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写25赔率,小数点后保留1-2位"));
            decimal score5z = Convert.ToDecimal(Utils.GetRequest("score5z", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写26赔率,小数点后保留1-2位"));
            decimal score5k = Convert.ToDecimal(Utils.GetRequest("score5k", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写27赔率,小数点后保留1-2位"));
            decimal scoreot = Convert.ToDecimal(Utils.GetRequest("scoreot", "post", 2, @"^-1$|^(\d)*(\.(\d){1,2})?$", "请正确填写27赔率,小数点后保留1-2位"));

            string score = "";
            score += "1:0|" + score10 + ",0:1|" + score01 + ",0:0|" + score00 + ",2:0|" + score20 + ",0:2|" + score02 + ",1:1|" + score11 + ",3:0|" + score30 + ",0:3|" + score03 + ",2:2|" + score22 + "";
            score += ",4:0|" + score40 + ",0:4|" + score04 + ",3:3|" + score33 + ",2:1|" + score21 + ",1:2|" + score12 + ",4:4|" + score44 + "";
            score += ",3:1|" + score31 + ",1:3|" + score13 + ",4:1|" + score41 + ",1:4|" + score14 + ",3:2|" + score32 + ",2:3|" + score23 + ",4:2|" + score42 + ",2:4|" + score24 + ",4:3|" + score43 + ",3:4|" + score34 + ",5z|" + score5z + ",5k|" + score5k + ",ot|" + scoreot;

            new TPR2.BLL.guess.BaList().Updatep_score(gid, score);
            Utils.Success("波胆设置", "波胆设置成功..", Utils.getUrl("showguess.aspx?act=score&amp;gid=" + gid + ""), "1");
        }
        #endregion

        #region 清空波胆
        else if (act == "scoreok2")
        {
            //游戏日志记录
            string[] p_pageArr = { "act", "gid" };
            BCW.User.GameLog.GameLogGetPage(1, Utils.getPageUrl(), p_pageArr, "后台管理员" + ManageId + "号将赛事清空波胆" + model.p_one + "VS" + model.p_two + "(" + gid + ")", gid);

            new TPR2.BLL.guess.BaList().Updatep_score(gid, "");
            Utils.Success("清空波胆", "清空波胆成功..", Utils.getUrl("showguess.aspx?act=score&amp;gid=" + gid + ""), "1");
        }
        #endregion

        #region 下注受限设置
        else if (act == "xz")
        {
            Master.Title = "下注受限设置";

            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("下注受限设置");
            builder.Append(Out.Tab("</div>", ""));
            string strText = "全场受限ID(全局设置|用#分隔):/,上盘受限ID(用#分隔):/,下盘受限ID(用#分隔):/,大盘受限(用#分隔):/,小盘受限(用#分隔):/,,";
            string strName = "ID0,ID1,ID2,ID3,ID4,gid,act";
            string strType = "text,text,text,text,text,hidden,hidden";
            string strValu = "" + model.xID0 + "'" + model.xID1 + "'" + model.xID2 + "'" + model.xID3 + "'" + model.xID4 + "'" + gid + "'xzok";
            string strEmpt = "true,true,true,true,true,false,false";
            string strIdea = "/";
            string strOthe = "确定编辑,showGuess.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.waplink(Utils.getUrl("showGuess.aspx?gid=" + gid + ""), "返回上级") + "");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        #endregion

        #region 设置受限ID
        else if (act == "xzok")
        {
            //游戏日志记录
            string[] p_pageArr = { "act", "gid", "ID0", "ID1", "ID2", "ID3", "ID4" };
            BCW.User.GameLog.GameLogPage(1, Utils.getPageUrl(), p_pageArr, "后台管理员" + ManageId + "号将赛事设置受限ID" + model.p_one + "VS" + model.p_two + "(" + gid + ")", gid);

            string ID0 = Utils.GetRequest("ID0", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "填写错误");
            string ID1 = Utils.GetRequest("ID1", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "填写错误");
            string ID2 = Utils.GetRequest("ID2", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "填写错误");
            string ID3 = Utils.GetRequest("ID3", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "填写错误");
            string ID4 = Utils.GetRequest("ID4", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "填写错误");

            new TPR2.BLL.guess.BaList().UpdatexID(gid, ID0, 0);
            new TPR2.BLL.guess.BaList().UpdatexID(gid, ID1, 1);
            new TPR2.BLL.guess.BaList().UpdatexID(gid, ID2, 2);
            new TPR2.BLL.guess.BaList().UpdatexID(gid, ID3, 3);
            new TPR2.BLL.guess.BaList().UpdatexID(gid, ID4, 4);

            Utils.Success("下注受限", "下注受限设置成功..", Utils.getUrl("showguess.aspx?act=xz&amp;gid=" + gid + ""), "1");
        }
        #endregion

        #region 全局封盘
        if (act == "luck")
        {
            //游戏日志记录
            string[] p_pageArr = { "act", "gid" };
            BCW.User.GameLog.GameLogGetPage(1, Utils.getPageUrl(), p_pageArr, "后台管理员" + ManageId + "号将赛事" + model.p_one + "VS" + model.p_two + "(" + gid + ")全局封盘", gid);

            new TPR2.BLL.guess.BaList().Updatep_isluck(gid, 1);
            Utils.Success("全局封盘", "全局封盘成功，所有会员将不能下注本场，需要手工解除封盘才可以..", Utils.getUrl("showguess.aspx?gid=" + gid + ""), "2");
        }
        #endregion

        #region 全局解封
        if (act == "noluck")
        {
            //游戏日志记录
            string[] p_pageArr = { "act", "gid" };
            BCW.User.GameLog.GameLogGetPage(1, Utils.getPageUrl(), p_pageArr, "后台管理员" + ManageId + "号将赛事" + model.p_one + "VS" + model.p_two + "(" + gid + ")全局解封", gid);

            new TPR2.BLL.guess.BaList().Updatep_isluck(gid, 0);
            Utils.Success("全局解封", "全局解封成功，所有会员可以下注了..", Utils.getUrl("showguess.aspx?gid=" + gid + ""), "2");
        }
        #endregion

        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append(model.p_one + "VS" + model.p_two);
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));

            builder.Append(Out.waplink(Utils.getUrl("default.aspx?ptype=4&amp;fly=" + model.p_title + ""), model.p_title) + ":" + model.p_one + "VS" + model.p_two);

            #region 受限提示
            if (model.xID0 != "" || model.xID1 != "" || model.xID2 != "" || model.xID3 != "" || model.xID4 != "")
            {
                builder.Append("(有受限)");
            }
            #endregion

            #region 波胆
            builder.Append("<br />开赛:" + DT.FormatDate(Convert.ToDateTime(model.p_TPRtime), 0));
            if (model.p_type == 1)
            {
                builder.Append("" + Out.waplink(Utils.getUrl("showGuess.aspx?act=score&amp;gid=" + gid + ""), "[波胆]"));
                builder.AppendFormat("|" + Out.waplink(Utils.getUrl("plGuess.aspx?gid=" + model.ID + "&amp;p=100"), "{0}注") + "", new TPR2.BLL.guess.BaPay().GetCount(model.ID, Convert.ToInt32(model.p_type), 100));
            }

            #endregion

            #region 完场比分
            if (model.p_result_one != null && model.p_result_two != null)
                builder.Append("<br />完场比分：" + model.p_result_one + ":" + model.p_result_two + "");
            else
            {
                if (model.p_TPRtime > DateTime.Now)
                    builder.Append("<br />比赛状态:未");
                else
                    builder.Append("<br />比赛状态:" + Convertp_once(model.p_once) + "");

                builder.Append("<br />即时比分:" + model.p_result_temp1 + ":" + model.p_result_temp2 + "");
            }
            builder.Append("" + Out.waplink(Utils.getUrl("showGuess.aspx?act=analysis&amp;gid=" + gid + ""), "[析]"));

            builder.Append(Out.Tab("</div>", "<br />"));

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

            #region 让球盘
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("〓让球盘〓");

            if (model.p_ison == 1)
            {
                if (model.p_isluckone == 0)
                    builder.Append("" + Out.waplink(Utils.getUrl("showGuess.aspx?gid=" + gid + "&amp;act=luck1"), "封"));
                else
                    builder.Append("" + Out.waplink(Utils.getUrl("showGuess.aspx?gid=" + gid + "&amp;act=noluck1"), "解封"));
            }
            if (((model.p_ison == 1 && model.p_isluckone == 1) || model.p_isluck == 1) && model.p_active == 0)
                builder.Append("<b>(封)</b>");
            else if (bo == "")
            {
                builder.Append("<b>(停)</b>");
            }

            builder.AppendFormat("<br />{0}" + Out.waplink(Utils.getUrl("plGuess.aspx?gid=" + model.ID + "&amp;p=1"), "{1}注"), model.p_one + "" + hp_one + "(" + Convert.ToDouble(model.p_one_lu) + ")", new TPR2.BLL.guess.BaPay().GetCount(model.ID, Convert.ToInt32(model.p_type), 1));

            if (model.p_type == 1)
                builder.Append("<br />" + GCK.getZqPn(Convert.ToInt32(model.p_pn)) + "" + GCK.getPkName(Convert.ToInt32(model.p_pk)) + "");
            else
                builder.Append("<br />" + Convert.ToDouble(model.p_pk) + "");

            builder.AppendFormat("<br />{0}" + Out.waplink(Utils.getUrl("plGuess.aspx?gid=" + model.ID + "&amp;p=2"), "{1}注"), model.p_two + "" + hp_two + "(" + Convert.ToDouble(model.p_two_lu) + ")", new TPR2.BLL.guess.BaPay().GetCount(model.ID, Convert.ToInt32(model.p_type), 2));
            #endregion

            #region 大小盘
            if (model.p_big_lu != 0)
            {
                builder.Append("<br />〓大小盘〓");
                if (model.p_ison == 1)
                {
                    if (model.p_islucktwo == 0)
                        builder.Append("" + Out.waplink(Utils.getUrl("showGuess.aspx?gid=" + gid + "&amp;act=luck2"), "封"));
                    else
                        builder.Append("" + Out.waplink(Utils.getUrl("showGuess.aspx?gid=" + gid + "&amp;act=noluck2"), "解封"));
                }

                if (((model.p_ison == 1 && model.p_islucktwo == 1) || model.p_isluck == 1) && model.p_active == 0)
                    builder.Append("<b>(封)</b>");
                else if (bo == "")
                {
                    builder.Append("<b>(停)</b>");
                }

                builder.AppendFormat("<br />" + Out.waplink(Utils.getUrl("plGuess.aspx?gid=" + model.ID + "&amp;p=3"), "{0}注") + "{1}", new TPR2.BLL.guess.BaPay().GetCount(model.ID, Convert.ToInt32(model.p_type), 3), "大(" + Convert.ToDouble(model.p_big_lu) + ")");

                if (model.p_type == 1)
                    builder.Append(GCK.getDxPkName(Convert.ToInt32(model.p_dx_pk)));
                else
                    builder.Append(Convert.ToDouble(model.p_dx_pk));

                builder.AppendFormat("{0}" + Out.waplink(Utils.getUrl("plGuess.aspx?gid=" + model.ID + "&amp;p=4"), "{1}注"), "小(" + Convert.ToDouble(model.p_small_lu) + ")", new TPR2.BLL.guess.BaPay().GetCount(model.ID, Convert.ToInt32(model.p_type), 4));
            }
            #endregion

            #region 标准盘
            if (model.p_bzs_lu != 0)
            {
                builder.Append("<br />〓标准盘〓");
                if (model.p_ison == 1)
                {
                    if (model.p_isluckthr == 0)
                        builder.Append("" + Out.waplink(Utils.getUrl("showGuess.aspx?gid=" + gid + "&amp;act=luck3"), "封"));
                    else
                        builder.Append("" + Out.waplink(Utils.getUrl("showGuess.aspx?gid=" + gid + "&amp;act=noluck3"), "解封"));
                }
                if (((model.p_ison == 1 && model.p_isluckthr == 1) || model.p_isluck == 1) && model.p_active == 0)
                    builder.Append("<b>(封)</b>");
                else if (bo == "")
                {
                    builder.Append("<b>(停)</b>");
                }

                builder.AppendFormat("<br />{0}" + Out.waplink(Utils.getUrl("plGuess.aspx?gid=" + model.ID + "&amp;p=5"), "{1}注"), "主胜(" + Convert.ToDouble(model.p_bzs_lu) + ")", new TPR2.BLL.guess.BaPay().GetCount(model.ID, Convert.ToInt32(model.p_type), 5));
                builder.AppendFormat("<br />{0}" + Out.waplink(Utils.getUrl("plGuess.aspx?gid=" + model.ID + "&amp;p=6"), "{1}注"), "平手(" + Convert.ToDouble(model.p_bzp_lu) + ")", new TPR2.BLL.guess.BaPay().GetCount(model.ID, Convert.ToInt32(model.p_type), 6));
                builder.AppendFormat("<br />{0}" + Out.waplink(Utils.getUrl("plGuess.aspx?gid=" + model.ID + "&amp;p=7"), "{1}注"), "客胜(" + Convert.ToDouble(model.p_bzx_lu) + ")", new TPR2.BLL.guess.BaPay().GetCount(model.ID, Convert.ToInt32(model.p_type), 7));
            }
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            #endregion

            #region 统计 邵广林 20160817 增加机器人盈利分析
            long PayCents = new TPR2.BLL.guess.BaPay().GetBaPayCent(model.ID, Convert.ToInt32(model.p_type));
            long PayCents_robot = new TPR2.BLL.guess.BaPay().GetBaPayCent_robot(model.ID, Convert.ToInt32(model.p_type));
            builder.Append("" + ub.Get("SiteBz") + "统计:<br />会员总注数:" + new TPR2.BLL.guess.BaPay().GetBaPayNum(model.ID, Convert.ToInt32(model.p_type)) + "/会员下注额:" + PayCents + "");
            builder.Append("<br />机器总注数:" + new TPR2.BLL.guess.BaPay().GetBaPayNum_robot(model.ID, Convert.ToInt32(model.p_type)) + "/机器下注额:" + PayCents_robot + "");
            if (model.p_result_one != null && model.p_result_two != null)
            {
                long WinMoney = new TPR2.BLL.guess.BaPay().GetBaPaygetMoney("bcid=" + gid + " and Types=0 and isrobot=0");
                long WinMoney_robot = new TPR2.BLL.guess.BaPay().GetBaPaygetMoney("bcid=" + gid + " and Types=0 and isrobot=1");
                builder.Append("<br />会员总返彩:" + WinMoney + "/会员盈利额:" + (PayCents - WinMoney) + "");
                builder.Append("<br />机器总返彩:" + WinMoney_robot + "/机器盈利额:" + (PayCents_robot - WinMoney_robot) + "");
            }
            
            #endregion

            #region 〓管理〓
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

            if (model.p_dr == 0)
                builder.Append("<br />开奖方式：自动开奖<br />" + Out.waplink(Utils.getUrl("showGuess.aspx?gid=" + gid + "&amp;dr=1"), "人工开奖"));
            else
                builder.Append("<br />开奖方式：人工开奖<br />" + Out.waplink(Utils.getUrl("showGuess.aspx?gid=" + gid + "&amp;dr=0"), "自动开奖"));

            if (model.p_ison == 1)
                builder.Append("<br />" + ub.Get("SiteGqText") + "状态：" + ub.Get("SiteGqText") + "");
            else
                builder.Append("<br />" + ub.Get("SiteGqText") + "状态：非" + ub.Get("SiteGqText") + "");

            builder.Append("" + Out.waplink(Utils.getUrl("showGuess.aspx?gid=" + gid + "&amp;act=once"), "编辑"));

            builder.Append("<br />8bo:" + model.p_id + "<br />");
            if (model.p_isluck == 0)
                builder.Append("" + Out.waplink(Utils.getUrl("showGuess.aspx?gid=" + gid + "&amp;act=luck"), "全局封盘"));
            else
                builder.Append("" + Out.waplink(Utils.getUrl("showGuess.aspx?gid=" + gid + "&amp;act=noluck"), "全局解封"));

            builder.Append("<br />" + Out.waplink(Utils.getUrl("showGuess.aspx?gid=" + gid + "&amp;act=xz"), "本场受限ID"));
            builder.Append("<br />" + Out.waplink(Utils.getUrl("../forumlog.aspx?act=gamelog&amp;ptype=2&amp;gid=" + gid + "&amp;backurl=" + Utils.PostPage(1) + ""), "" + ub.Get("SiteGqText") + "失败日志"));
            builder.Append("<br />" + Out.waplink(Utils.getUrl("../forumlog.aspx?act=gamelog&amp;ptype=1&amp;gid=" + gid + "&amp;backurl=" + Utils.PostPage(1) + ""), "操作日志"));
            builder.Append("<br />" + Out.waplink(Utils.getUrl("../forumlog.aspx?act=gameowe&amp;ptype=1&amp;gid=" + gid + "&amp;backurl=" + Utils.PostPage(1) + ""), "欠币日志"));
            #endregion

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
                        once = (min - 3) + "'";
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
