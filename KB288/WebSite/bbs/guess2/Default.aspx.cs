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
using System.Web.SessionState;
using BCW.Common;

/// <summary>
/// ==================================
/// 增加夏季联赛分类
/// 黄国军 20160716
/// 
/// 体育球彩全场竞猜 没有读取外部网站
/// 
/// 黄国军 20160105 增加注释
/// ==================================
/// </summary>
public partial class bbs_guess2_Default : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/guess2.xml";
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = ub.GetSub("SiteName", xmlPath);
        DataSet ds = null;
        int meid = new BCW.User.Users().GetUsId();
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[0-9]\d*$", "0"));
        int showtype = Utils.ParseInt(Utils.GetRequest("showtype", "get", 1, @"^[0-1]$", "0"));
        int sp = Utils.ParseInt(Utils.GetRequest("sp", "get", 1, @"^[0-1]$", "0"));
        string fly = "";
        if (ptype == 4)
            fly = Out.UBB(Utils.GetRequest("fly", "get", 2, @"^.+?$", "请选择联赛"));

        #region 页面logo
        if (ub.GetSub("SiteLogo", xmlPath) != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<img src=\"" + ub.GetSub("SiteLogo", xmlPath) + "\" alt=\"load\"/>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        if (ub.GetSub("Sitevice", xmlPath) != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.SysUBB(ub.GetSub("Sitevice", xmlPath)));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        #endregion

        #region 拾物随机
        //拾物随机
        builder.Append(BCW.User.Game.GiftFlows.ShowGiftFlows(4));
        #endregion

        #region 顶部导航
        if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th"))
        {
            #region 土豪网的表头导航
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("/bbs/game/default.aspx") + "\">游戏大厅</a>&gt;球彩");
            builder.Append("&gt;<a href=\"" + Utils.getUrl("default.aspx") + "\">半单</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            long gold = new BCW.BLL.User().GetGold(meid);
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("您现在有" + Utils.ConvertGold(gold) + "" + ub.Get("SiteBz") + "");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("【全场赛事】<a href=\"" + Utils.getUrl("default.aspx?ptype=" + ptype + "&amp;fly=" + fly + "") + "\">刷新</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("");
            builder.Append("<b><a href=\"" + Utils.getUrl("default.aspx?showtype=1") + "\">全场</a></b>:");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx?showtype=1&amp;ptype=1") + "\">足球</a>.");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx?showtype=1&amp;ptype=2") + "\">篮球</a>.");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx?showtype=1&amp;ptype=3") + "\">联赛</a>.");
            builder.Append("<a href=\"" + Utils.getUrl("kzlist.aspx") + "\">个庄</a>.");
            builder.Append("<a href=\"" + Utils.getUrl("caseGuess.aspx") + "\">兑奖</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx?showtype=1&amp;ptype=5") + "\">" + ub.Get("SiteGqText") + "</a>.");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx?showtype=1&amp;ptype=6") + "\">波胆</a>.");
            builder.Append("<a href=\"" + Utils.getUrl("betsuper.aspx") + "\">串关</a>.");
            builder.Append("<a href=\"" + Utils.getUrl("live.aspx") + "\">赛果</a>.");
            builder.Append("<a href=\"" + Utils.getUrl("live.aspx?act=16") + "\">NBA直播</a> ");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("<b><a href=\"" + Utils.getUrl("default.aspx?showtype=1&amp;ptype=7") + "\">半单</a></b>:");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx?showtype=1&amp;ptype=8") + "\">足半</a>.");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx?showtype=1&amp;ptype=9") + "\">篮半</a>.");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx?showtype=1&amp;ptype=10") + "\">篮单</a>.");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx?showtype=1&amp;ptype=3") + "\">联赛</a>.");
            builder.Append("<a href=\"" + Utils.getUrl("caseGuess.aspx") + "\">兑奖</a>");
            builder.Append(Out.Tab("</div>", ""));
            #endregion
        }
        else
        {
            #region 表头导航
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("/bbs/game/default.aspx") + "\">游戏大厅</a>&gt;");
            builder.Append("<a href=\"" + Utils.getUrl("/bbs/guess2/default.aspx") + "\">全场</a>&gt;");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx?showtype=1&amp;ptype=7") + "\">半单</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<b>体育球彩全场竞猜</b>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">全部</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx?showtype=1&amp;ptype=1") + "\">足球</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx?showtype=1&amp;ptype=2") + "\">篮球</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx?showtype=1&amp;ptype=5") + "\">" + ub.Get("SiteGqText") + "</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx?ptype=" + ptype + "&amp;fly=" + fly + "") + "\">刷新</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx?showtype=1&amp;ptype=6") + "\">波胆</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("betsuper.aspx") + "\">串关</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx?showtype=1&amp;ptype=3") + "\">联赛</a> ");
            //builder.Append("<a href=\"" + Utils.getUrl("/default.aspx?id=13088") + "\">资讯</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx?showtype=1&amp;ptype=7") + "\">半单</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("caseGuess.aspx") + "\">兑奖</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("kzlist.aspx") + "\">个人庄</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx?sp=1") + "\">NBA</a> ");
            //builder.Append("<a href=\"" + Utils.getUrl("live.aspx") + "\">即时比分</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("live.aspx?act=16") + "\">NBA直播</a> ");
            builder.Append(Out.Tab("</div>", ""));
            #endregion
        }
        #endregion

        #region 分页参数 暂未知 p_ison=1 and p_isondel=0的意思
        int pageSize = 0;
        if (showtype == 1)
        {
            pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        }
        else
        {
            if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th"))
                pageSize = 6;
            else
                pageSize = 5;
        }
        int pageIndex;
        int recordCount;
        string strDay = "";
        string strWhere = "";
        string strOrder = "";
        string[] pageValUrl = { "showtype", "ptype", "fly" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        #endregion

        #region 读取循环 暂未知 p_ison=1 and p_isondel=0的意思 默认的分类 ptype == 0 && showtype==0
        //默认的分类 ptype == 0 && showtype==0
        if (ptype == 0 && showtype == 0)
        {
            #region 土豪的竞猜连接顶部
            if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th"))
            {
                builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                builder.Append("" + ub.Get("SiteGqText") + "赛事--");
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx?showtype=1&amp;ptype=5") + "\">更多&gt;&gt;</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            #endregion

            #region SQL
            //读取的SQL
            strWhere = "p_active=0 and p_del=0 and p_ison=1 and p_isondel=0 and p_basketve=0";
            if (sp == 1)
            {
                strWhere += "  AND p_title LIKE '%NBA%'";
            }
            strOrder = "p_TPRtime ASC,ID ASC";
            #endregion

            #region 开始读取竞猜表
            // 开始读取竞猜表 tb_TBaList 按 p_TPRtime 时间排列
            IList<TPR2.Model.guess.BaList> listBaList = new TPR2.BLL.guess.BaList().GetBaLists(pageIndex, pageSize, strWhere, strOrder, out recordCount);
            if (listBaList.Count > 0)
            {
                #region 循环
                int k = 1;
                foreach (TPR2.Model.guess.BaList n in listBaList)
                {
                    if (k % 2 == 0)
                    {
                        builder.Append(Out.Tab("<div>", "<br />"));
                    }
                    else
                    {
                        builder.Append(Out.Tab("<div>", "<br />"));
                    }

                    if (DT.GetTime(n.p_TPRtime.ToString(), "MM月dd日").ToString() != strDay.ToString())
                        builder.Append(DT.GetTime(n.p_TPRtime.ToString(), "MM月dd日") + "<br />");

                    string Sonce = string.Empty;
                    string VS = "VS";
                    string hp_one = "";
                    string hp_two = "";
                    if (n.p_ison == 1)
                    {
                        Sonce = "(" + ub.Get("SiteGqText") + ")";
                        if (n.p_result_temp1 == null && n.p_result_temp2 == null)
                            VS = "(0-0)";
                        else
                            VS = "(" + n.p_result_temp1 + "-" + n.p_result_temp2 + ")";

                        if (n.p_type == 1)
                        {
                            if (n.p_hp_one > 0)
                                hp_one = "<img src=\"/Files/sys/guess/redcard" + n.p_hp_one + ".gif\" alt=\"红" + n.p_hp_one + "\"/>";

                            if (n.p_hp_two > 0)
                                hp_two = "<img src=\"/Files/sys/guess/redcard" + n.p_hp_two + ".gif\" alt=\"红" + n.p_hp_two + "\"/>";

                        }

                    }
                    string actUrl = string.Empty;
                    if (ptype == 6)
                        actUrl = "act=score&amp;";

                    builder.AppendFormat("<a href=\"" + Utils.getUrl("showGuess.aspx?" + actUrl + "gid={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{1}[{2}]{3}" + hp_one + "" + VS + "{4}" + hp_two + "{5}</a>", n.ID, DT.FormatDate(Convert.ToDateTime(n.p_TPRtime), 13), n.p_title, n.p_one, n.p_two, Sonce);
                    builder.Append("" + Convertp_once(n.p_once) + "" + Out.waplink(Utils.getUrl("showGuess.aspx?act=analysis&amp;gid=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + ""), "[析]"));
                    builder.Append(Out.Tab("</div>", ""));

                    strDay = DT.GetTime(n.p_TPRtime.ToString(), "MM月dd日").ToString();
                    k++;
                }
                #endregion

                #region 分页
                if (!Utils.GetTopDomain().Contains("tuhao") && !Utils.GetTopDomain().Contains("th"))
                {
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    builder.Append("<b><a href=\"" + Utils.getUrl("default.aspx?showtype=1&amp;ptype=5") + "\">更多滚球竞猜记录&gt;&gt;</a></b>");
                    builder.Append(Out.Tab("</div>", ""));
                }
                #endregion
            }
            #endregion
        }
        #endregion

        builder.Append(Out.Tab("", "<br />"));

        #region 土豪的导航
        if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th"))
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            if (ptype == 1)
                builder.Append("足球赛事--");
            else if (ptype == 2)
                builder.Append("篮球赛事--");
            else if (ptype == 3)
                builder.Append("联赛列表--");
            else if (ptype == 5)
                builder.Append("" + ub.Get("SiteGqText") + "赛事--");
            else if (ptype == 6)
                builder.Append("波胆赛事--");
            else
                builder.Append("未开赛事--");

            if (showtype == 0)
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx?showtype=1") + "\">更多&gt;&gt;</a>");
            else
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">&lt;&lt;返回</a>");

            builder.Append(Out.Tab("</div>", "<br />"));
        }
        #endregion

        #region 非土豪网,分页数量
        if (!Utils.GetTopDomain().Contains("tuhao") && !Utils.GetTopDomain().Contains("th"))
        {
            if (showtype == 0)
                pageSize = 6;
        }
        #endregion

        if (ptype != 3)
        {
            #region 类型不等于3的时候显示列表 ptype != 3
            strDay = "";
            if (ptype > 0 && ptype < 4)
                strWhere = "p_type=" + ptype + " and p_active=0 and p_del=0 and (p_TPRtime >= '" + System.DateTime.Now + "' OR (p_ison=1 and p_isondel=0))  and p_basketve=0";
            else if (ptype == 4)
                strWhere = "p_active=0 and p_del=0 and p_TPRtime >= '" + System.DateTime.Now + "' and p_title like '%" + fly + "'";
            else if (ptype == 5)
                strWhere = "p_active=0 and p_del=0 and p_ison=1 and p_isondel=0 and p_basketve=0";
            else if (ptype == 6)
                strWhere = "p_active=0 and p_del=0 and p_TPRtime >= '" + System.DateTime.Now + "' and p_score IS NOT NULL AND p_score<>''";
            else if (ptype == 7)
                strWhere = "p_active=0 and p_del=0 and (p_TPRtime >= '" + System.DateTime.Now + "' OR (p_ison=1 and p_isondel=0)) and p_basketve>0";
            else if (ptype == 8)
                strWhere = "p_active=0 and p_del=0 and (p_TPRtime >= '" + System.DateTime.Now + "' OR (p_ison=1 and p_isondel=0)) and p_basketve=9";
            else if (ptype == 9)
                strWhere = "p_active=0 and p_del=0 and (p_TPRtime >= '" + System.DateTime.Now + "') and p_basketve=3";
            else if (ptype == 10)
                strWhere = "p_active=0 and p_del=0 and (p_TPRtime >= '" + System.DateTime.Now + "') and p_basketve>0 and p_basketve<9 and p_basketve<>3 ";
            else
                strWhere = "p_active=0 and p_del=0 and p_TPRtime >= '" + System.DateTime.Now + "' and p_basketve=0";
            if (sp == 1)
            {
                strWhere += "  AND p_title LIKE '%NBA%'";
            }
            strOrder = "p_TPRtime ASC,ID ASC";
            #endregion

            #region 开始读取竞猜 列表
            // 开始读取竞猜
            IList<TPR2.Model.guess.BaList> listBaList = new TPR2.BLL.guess.BaList().GetBaLists(pageIndex, pageSize, strWhere, strOrder, out recordCount);
            if (listBaList.Count > 0)
            {
                int k = 1;
                foreach (TPR2.Model.guess.BaList n in listBaList)
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
                    string VS = "VS";
                    string hp_one = "";
                    string hp_two = "";
                    string p_once = "";
                    if (n.p_ison == 1 && (ptype == 5 || showtype == 1))
                    {
                        Sonce = "(" + ub.Get("SiteGqText") + ")";
                        if (n.p_result_temp1 == null && n.p_result_temp2 == null)
                            VS = "(0-0)";
                        else
                            VS = "(" + n.p_result_temp1 + "-" + n.p_result_temp2 + ")";

                        if (n.p_type == 1)
                        {
                            if (n.p_hp_one > 0)
                                hp_one = "<img src=\"/Files/sys/guess/redcard" + n.p_hp_one + ".gif\" alt=\"红" + n.p_hp_one + "\"/>";

                            if (n.p_hp_two > 0)
                                hp_two = "<img src=\"/Files/sys/guess/redcard" + n.p_hp_two + ".gif\" alt=\"红" + n.p_hp_two + "\"/>";

                        }
                        p_once = Convertp_once(n.p_once);
                    }

                    string actUrl = string.Empty;
                    if (ptype == 6)
                        actUrl = "act=score&amp;";

                    builder.AppendFormat("<a href=\"" + Utils.getUrl("showGuess.aspx?" + actUrl + "gid={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{1}[{2}]{3}" + VS + "{4}{5}</a>", n.ID, DT.FormatDate(Convert.ToDateTime(n.p_TPRtime), 13), n.p_title, n.p_one, n.p_two, Sonce);
                    builder.Append("" + p_once + "" + Out.waplink(Utils.getUrl("showGuess.aspx?act=analysis&amp;gid=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + ""), "[析]"));
                    builder.Append(Out.Tab("</div>", ""));

                    strDay = DT.GetTime(n.p_TPRtime.ToString(), "MM月dd日").ToString();
                    k++;
                }

                // 分页
                if (showtype == 1)
                {
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                }

            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录.."));
            }
            #endregion

            #region 土豪网更多精彩连接
            if (!Utils.GetTopDomain().Contains("tuhao") && !Utils.GetTopDomain().Contains("th"))
            {
                builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                if (ptype >= 7)
                {
                    builder.Append("<b><a href=\"" + Utils.getUrl("default.aspx?showtype=1&amp;ptype=7") + "\">更多半场单节记录&gt;&gt;</a></b>");
                }
                else
                {
                    builder.Append("<b><a href=\"" + Utils.getUrl("default.aspx?showtype=1") + "\">更多全场竞猜记录&gt;&gt;</a></b>");

                }
                builder.Append(Out.Tab("</div>", ""));
            }
            #endregion
        }
        else
        {
            #region 类型等于3时 SQL
            string strLXWhere = "p_active=0 and p_del=0 and p_TPRtime >= '" + System.DateTime.Now + "'";
            #endregion

            #region 开始读取竞猜
            // 开始读取竞猜
            IList<TPR2.Model.guess.BaList> listBaListLX = new TPR2.BLL.guess.BaList().GetBaListLX(pageIndex, pageSize, strLXWhere, out recordCount);
            if (listBaListLX.Count > 0)
            {
                int k = 1;
                foreach (TPR2.Model.guess.BaList n in listBaListLX)
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

                    builder.AppendFormat("<a href=\"" + Utils.getUrl("default.aspx?ptype=4&amp;showtype=1&amp;fly={0}") + "\">-{1}(共{2}场)</a>", n.p_title, n.p_title, new TPR2.BLL.guess.BaList().GetCount(n));
                    builder.Append(Out.Tab("</div>", ""));

                    k++;
                }

                // 分页
                if (showtype == 1)
                {
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                }
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录.."));
            }
            #endregion
        }

        #region 半场和单节 ptype == 0 && showtype == 0
        if (ptype == 0 && showtype == 0)
        {
            #region 球彩下半部分导航(半场和单节)
            if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th"))
            {
                builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                builder.Append("【半场单节】");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
                builder.Append("<b>体育球彩半单竞猜</b>");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx?showtype=1&amp;ptype=8") + "\">足半</a> ");
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx?showtype=1&amp;ptype=9") + "\">篮半</a> ");
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx?showtype=1&amp;ptype=10") + "\">篮单</a> ");
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx?showtype=1&amp;ptype=3") + "\">联赛</a> ");
                builder.Append("<a href=\"" + Utils.getUrl("caseGuess.aspx") + "\">兑奖</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            #endregion

            #region 半单SQL
            strWhere = "p_active=0 and p_del=0 and p_isondel=0 and (p_TPRtime >= '" + System.DateTime.Now + "' OR p_ison=1) and p_basketve>0";
            if (sp == 1)
            {
                strWhere += "  AND p_title LIKE '%NBA%'";
            }
            strOrder = "p_TPRtime ASC,ID ASC";
            #endregion

            #region 开始读取竞猜
            // 开始读取竞猜
            IList<TPR2.Model.guess.BaList> listBaList = new TPR2.BLL.guess.BaList().GetBaLists(pageIndex, pageSize, strWhere, strOrder, out recordCount);
            if (listBaList.Count > 0)
            {
                #region 循环
                int k = 1;
                foreach (TPR2.Model.guess.BaList n in listBaList)
                {
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div>", "<br />"));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));

                    string Sonce = string.Empty;
                    string VS = "VS";
                    //string hp_one = "";
                    //string hp_two = "";
                    if (n.p_ison == 1)
                    {
                        Sonce = "(" + ub.Get("SiteGqText") + ")";
                        if (n.p_result_temp1 == null && n.p_result_temp2 == null)
                            VS = "(0-0)";
                        else
                            VS = "(" + n.p_result_temp1 + "-" + n.p_result_temp2 + ")";

                        //if (n.p_type == 1)
                        //{
                        //    if (n.p_hp_one > 0)
                        //        hp_one = "<img src=\"/Files/sys/guess/redcard" + n.p_hp_one + ".gif\" alt=\"红" + n.p_hp_one + "\"/>";

                        //    if (n.p_hp_two > 0)
                        //        hp_two = "<img src=\"/Files/sys/guess/redcard" + n.p_hp_two + ".gif\" alt=\"红" + n.p_hp_two + "\"/>";

                        //}
                    }

                    if (DT.GetTime(n.p_TPRtime.ToString(), "MM月dd日").ToString() != strDay.ToString())
                        builder.Append(DT.GetTime(n.p_TPRtime.ToString(), "MM月dd日") + "<br />");

                    builder.AppendFormat("<a href=\"" + Utils.getUrl("showGuess.aspx?gid={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{1}[{2}]{3}" + VS + "{4}" + Sonce + "</a>", n.ID, DT.FormatDate(Convert.ToDateTime(n.p_TPRtime), 13), n.p_title, n.p_one, n.p_two);
                    if (n.p_ison == 1)
                    {
                        builder.Append("" + n.p_once + "");
                    }
                    builder.Append(Out.Tab("</div>", ""));

                    strDay = DT.GetTime(n.p_TPRtime.ToString(), "MM月dd日").ToString();
                    k++;
                }
                #endregion
                builder.Append(Out.Tab("<div>", "<br />"));

                #region 半单底部导航
                if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th"))
                    builder.Append("<a href=\"" + Utils.getUrl("default.aspx?showtype=1&amp;ptype=7") + "\">更多半场单节&gt;&gt;</a>");
                else
                    builder.Append("<b><a href=\"" + Utils.getUrl("default.aspx?showtype=1&amp;ptype=7") + "\">更多半场单节记录&gt;&gt;</a></b>");
                #endregion

                builder.Append(Out.Tab("</div>", ""));
            }
            #endregion
        }
        #endregion

        #region 显示类型 showtype == 0
        if (showtype == 0)
        {
            #region 闲聊显示
            //闲聊显示
            builder.Append(BCW.User.Users.ShowSpeak(5, "default2.aspx", 5, 0));
            if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th"))
            {
                builder.Append(Out.Tab("<div>", Out.Hr()));
                builder.Append("查看:<a href=\"" + Utils.getUrl("myGuess.aspx?ptype=1") + "\">未开</a>.");
                builder.Append("<a href=\"" + Utils.getUrl("myGuess.aspx?pzztype=2") + "\">历史</a>.");
                builder.Append("<a href=\"" + Utils.getUrl("listGuess.aspx") + "\">记录</a>.");
                builder.Append("<a href=\"" + Utils.getUrl("help.aspx") + "\">规则</a>");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("【竞猜排行榜】");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            else
            {
                builder.Append(Out.Tab("<div>", Out.Hr()));
                builder.Append("<a href=\"" + Utils.getUrl("myGuess.aspx?ptype=1") + "\">未开投注</a> ");
                builder.Append("<a href=\"" + Utils.getUrl("myGuess.aspx?pzztype=2") + "\">历史投注</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("listGuess.aspx") + "\">赛事记录</a> ");
                builder.Append("<a href=\"" + Utils.getUrl("help.aspx") + "\">游戏规则</a>");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            #endregion

            #region 半单底部导航
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("topGuess.aspx") + "\">排行榜单</a>:<a href=\"" + Utils.getUrl("topGuess.aspx?p_type=1") + "\">足球</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("topGuess.aspx?p_type=2") + "\">篮球</a> <a href=\"" + Utils.getUrl("betsuper.aspx?act=top") + "\">串榜</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            #endregion

            #region 昨日神猜
            DateTime dt = DateTime.Parse(DateTime.Now.ToLongDateString());
            string dt1 = dt.AddDays(-1).AddHours(12).ToString();
            string dt2 = dt.AddHours(12).ToString();

            string dt3 = Convert.ToDateTime(DT.GetWeekStart()).AddHours(12).ToString();
            string dt4 = Convert.ToDateTime(DT.GetWeekOver()).AddHours(12).ToString();
            ds = new TPR2.BLL.guess.BaPay().GetBaPayList("TOP 1 payusid,Count(DISTINCT bcid) as payCount", "p_TPRtime >= '" + dt1 + "' AND p_TPRtime <= '" + dt2 + "' and itypes=0 and types = 0 and p_active>0 and p_getMoney>paycent group by payusid order by Count(DISTINCT bcid) desc");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                builder.Append("<a href=\"" + Utils.getUrl("topGuess.aspx?act=todaytop") + "\">昨日神猜</a>：<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + ds.Tables[0].Rows[0]["payusid"].ToString() + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(int.Parse(ds.Tables[0].Rows[0]["payusid"].ToString())) + "</a>(获胜" + ds.Tables[0].Rows[0]["payCount"] + "场)<br />");
            }
            else
                builder.Append("昨日神猜：暂无数据..<br />");
            #endregion

            #region 昨日之星
            ds = new TPR2.BLL.guess.BaPay().GetBaPayList("TOP 1 payusid,sum(p_getMoney-payCent) as payCents", "p_TPRtime >= '" + dt1 + "' AND p_TPRtime <= '" + dt2 + "' and itypes=0 and types = 0 and p_active>0 group by payusid order by sum(p_getMoney-payCent) desc");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                builder.Append("<a href=\"" + Utils.getUrl("topGuess.aspx?act=todaystar") + "\">昨日之星</a>：<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + ds.Tables[0].Rows[0]["payusid"].ToString() + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(int.Parse(ds.Tables[0].Rows[0]["payusid"].ToString())) + "</a>(净赚" + Convert.ToDouble(ds.Tables[0].Rows[0]["payCents"]) + "" + ub.Get("SiteBz") + ")");
            }
            else
                builder.Append("昨日之星：暂无数据..");
            #endregion

            #region 一周神猜
            ds = new TPR2.BLL.guess.BaPay().GetBaPayList("TOP 1 payusid,Count(DISTINCT bcid) as payCount", "p_TPRtime >= '" + dt3 + "' AND p_TPRtime <= '" + dt4 + "' and itypes=0 and types = 0 and p_active>0 and p_getMoney>paycent group by payusid order by Count(DISTINCT bcid) desc");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                builder.Append("<br /><a href=\"" + Utils.getUrl("topGuess.aspx?act=todaytop2") + "\">一周神猜</a>：<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + ds.Tables[0].Rows[0]["payusid"].ToString() + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(int.Parse(ds.Tables[0].Rows[0]["payusid"].ToString())) + "</a>(获胜" + ds.Tables[0].Rows[0]["payCount"] + "场)");
            }
            else
                builder.Append("<br />一周神猜：暂无数据..");
            #endregion

            #region 一周之星
            ds = new TPR2.BLL.guess.BaPay().GetBaPayList("TOP 1 payusid,sum(p_getMoney-payCent) as payCents", "p_TPRtime>='" + dt3 + "'and p_TPRtime<='" + dt4 + "' and itypes=0 and types = 0 and p_active>0 group by payusid order by sum(p_getMoney-payCent) desc");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                builder.Append("<br /><a href=\"" + Utils.getUrl("topGuess.aspx?act=todaystar2") + "\">一周之星</a>：<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + ds.Tables[0].Rows[0]["payusid"].ToString() + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(int.Parse(ds.Tables[0].Rows[0]["payusid"].ToString())) + "</a>(净赚" + Convert.ToDouble(ds.Tables[0].Rows[0]["payCents"]) + "" + ub.Get("SiteBz") + ")<br />");
            }
            else
                builder.Append("<br />一周之星：暂无数据..<br />");
            #endregion

            builder.Append(Out.Tab("</div>", ""));

            #region 底部公共UBB
            if (ub.GetSub("SiteFoot", xmlPath) != "")
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append(Out.SysUBB(ub.GetSub("SiteFoot", xmlPath)));
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            #endregion
        }
        else
        {
            #region showtype!=0的导航
            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回球彩首页</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("myGuess.aspx?ptype=1") + "\">未开投注</a> ");
            builder.Append("<a href=\"" + Utils.getUrl("myGuess.aspx?ptype=2") + "\">历史投注</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            #endregion
        }
        #endregion

        #region 底部友情链接
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/game/default.aspx") + "\">游戏中心</a>");
        if (Utils.GetDomain().Contains("lt388")){
            builder.Append("-<a href=\"" + Utils.getUrl("/bbs/guess2/gz/default.aspx") + "\">管</a>");
        
        }
        builder.Append(Out.Tab("</div>", ""));
        //-----------友链链入开始
        int kid = int.Parse(Utils.GetRequest("kid", "get", 1, @"^[0-9]\d*$", "0"));
        if (kid != 0)
        {
            if (new BCW.BLL.Link().Exists(kid))
            {
                //统计链入
                string xmlPath2 = "/Controls/link.xml";
                if (ub.GetSub("LinkIsPc", xmlPath2) == "0")
                {
                    if (Utils.IsMobileUa())
                    {
                        new BCW.BLL.Link().UpdateLinkIn(kid);
                        if (ub.GetSub("LinkGoUrl", xmlPath2) != "")
                        {
                            Response.Redirect(ub.GetSub("LinkGoUrl", xmlPath2));
                        }
                    }
                }
                else
                {
                    new BCW.BLL.Link().UpdateLinkIn(kid);
                    if (ub.GetSub("LinkGoUrl", xmlPath2) != "")
                    {
                        Response.Redirect(ub.GetSub("LinkGoUrl", xmlPath2));
                    }
                }
            }
        }
        //-----------友链链入结束
        #endregion
    }

    #region 比赛状态 Convertp_once
    /// <summary>
    /// 比赛状态
    /// </summary>
    /// <param name="p_once"></param>
    /// <returns></returns>
    private string Convertp_once(string p_once)
    {
        //string once = "";
        //if (!string.IsNullOrEmpty(p_once))
        //{
        //    if (p_once.Contains("'") && !p_once.Contains("+"))
        //    {
        //        try
        //        {
        //            int min = Convert.ToInt32(p_once.Replace("'", ""));
        //            if (min > 5)
        //                once = (min - 5) + "'";
        //            else
        //                once = p_once;
        //        }
        //        catch
        //        {
        //            once = p_once;
        //        }
        //    }
        //    else
        //    {
        //        once = p_once;
        //    }
        //}
        //return once;

        return p_once;
    }
    #endregion
}
