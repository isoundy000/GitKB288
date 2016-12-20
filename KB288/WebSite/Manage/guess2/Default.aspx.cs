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
/// 新增特别检查入口 黄国军 20161128
/// 修改整理抓取页面设置 黄国军 20160929
/// 修改半单抓取设置入口 黄国军 20160922
/// 增加显示全部走地功能
/// 黄国军 20160721
/// </summary>
public partial class Manage_guess2_Default : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "竞猜管理";
        string act = Utils.GetRequest("act", "all", 1, "", "");
        #region 隐藏走地
        if (act == "zddel" || act == "zddelok")
        {
            if (act == "zddel")
            {
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("确定隐藏全部" + ub.Get("SiteGqText") + "吗");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append(Out.waplink(Utils.getUrl("default.aspx?act=zddelok"), "确定隐藏") + "<br />");
                builder.Append(Out.waplink(Utils.getUrl("default.aspx"), "先留着吧.."));
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            else
            {
                BCW.Data.SqlHelper.ExecuteSql("update tb_TBaList set p_del=1 where p_ison=1 and p_active=0");
                Utils.Success("隐藏全部" + ub.Get("SiteGqText"), "隐藏全部" + ub.Get("SiteGqText") + "成功..", Utils.getUrl("default.aspx"), "1");
            }
        }
        if (act == "zddel2" || act == "zddelok2")
        {
            if (act == "zddel2")
            {
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("确定显示全部" + ub.Get("SiteGqText") + "吗");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append(Out.waplink(Utils.getUrl("default.aspx?act=zddelok2"), "确定显示") + "<br />");
                builder.Append(Out.waplink(Utils.getUrl("default.aspx"), "先留着吧.."));
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            else
            {
                BCW.Data.SqlHelper.ExecuteSql("update tb_TBaList set p_del=0 where p_ison=1 and p_active=0");
                Utils.Success("显示全部" + ub.Get("SiteGqText"), "显示全部" + ub.Get("SiteGqText") + "成功..", Utils.getUrl("default.aspx"), "1");
            }
        }
        #endregion

        #region 默认列表
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append(Out.waplink(Utils.getUrl("../game/default.aspx"), "游戏") + "&gt;球彩竞猜");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 1, @"^[0-9]\d*$", "0"));
            int showtype = Utils.ParseInt(Utils.GetRequest("showtype", "get", 1, @"^[0-2]$", "0"));
            string keyword = Out.UBB(Utils.GetRequest("keyword", "all", 1, "", ""));
            string fly = "";
            if (ptype == 4)
                fly = Out.UBB(Utils.GetRequest("fly", "get", 2, @"^.+?$", "请选择联赛"));

            builder.Append(Out.waplink(Utils.getUrl("addGuess.aspx"), "增加赛事") + " ");
            builder.Append(Out.waplink(Utils.getUrl("topGuess.aspx"), "排行榜") + " ");
            builder.Append(Out.waplink(Utils.getUrl("search.aspx"), "搜索") + "<br />");
            builder.Append("赛事分析<a href=\"" + Utils.getUrl("stats.aspx?showtype=2") + "\">未开</a>.");
            builder.Append("<a href=\"" + Utils.getUrl("stats.aspx") + "\">完场</a>.");
            builder.Append("<a href=\"" + Utils.getUrl("caselist.aspx") + "\">未兑奖</a>.");
            builder.Append("<a href=\"" + Utils.getUrl("../uinfo.aspx?ptype=8") + "\">已冻结</a>.");

            builder.Append("<a href=\"" + Utils.getUrl("plGuess.aspx?act=checkdata") + "\">" + MoneyToChinese(ub.GetSub("SiteListCent", "/Controls/guess2.xml")) + "列表</a><br />");

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
                builder.Append(Out.waplink(Utils.getUrl("default.aspx?ptype=3"), "联赛") + " ");

            if (ptype == 5)
                builder.Append("全场" + ub.Get("SiteGqText") + " ");
            else
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx?ptype=5") + "\">全场" + ub.Get("SiteGqText") + "</a> ");

            if (ptype == 6)
                builder.Append("波胆 ");
            else
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx?ptype=6") + "\">波胆</a> ");

            builder.Append(Out.waplink(Utils.getUrl("super.aspx"), "串串") + "");
            builder.Append(" " + Out.waplink(Utils.getUrl("kzlist.aspx"), "个人庄") + "<br />");

            if (ptype == 7)
                builder.Append("半单 ");
            else
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx?ptype=7") + "\">半单</a> ");

            if (ptype == 8)
                builder.Append("足半 ");
            else
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx?ptype=8") + "\">足半</a> ");

            if (ptype == 9)
                builder.Append("篮半 ");
            else
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx?ptype=9") + "\">篮半</a> ");

            if (ptype == 10)
                builder.Append("篮单 ");
            else
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx?ptype=10") + "\">篮单</a> ");

            if (ptype == 11)
                builder.Append("半场" + ub.Get("SiteGqText") + " ");
            else
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx?ptype=11") + "\">半场" + ub.Get("SiteGqText") + "</a> ");

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
                        strWhere = "p_type=" + ptype + " and p_basketve=0";
                    else if (ptype == 4)
                        strWhere = "p_title like '%" + fly + "%'";
                    else if (ptype == 5)
                        strWhere = "p_ison=1 and p_basketve=0";
                    else if (ptype == 6)
                        strWhere = "p_score IS NOT NULL AND p_score<>''";
                    else if (ptype == 7)
                        strWhere = "p_basketve>0";
                    else if (ptype == 8)
                        strWhere = "p_basketve=9";
                    else if (ptype == 9)
                        strWhere = "p_basketve=3";
                    else if (ptype == 10)
                        strWhere = "p_basketve>0 and p_basketve<9 and p_basketve<>3";
                    else if (ptype == 11)
                        strWhere = "p_ison=1 and p_basketve=9";
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
                        if (n.p_ison == 1)
                            Sonce = "(" + ub.Get("SiteGqText") + ")";
                        builder.Append(Out.waplink(Utils.getUrl("editGuess.aspx?gid=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + ""), "[管理]"));
                        builder.AppendFormat(Out.waplink(Utils.getUrl("showGuess.aspx?gid={0}&amp;backurl=" + Utils.PostPage(1) + ""), "{1}[" + n.p_title + "]{2}VS{3}{4}") + "", n.ID, DT.FormatDate(Convert.ToDateTime(n.p_TPRtime), 13), n.p_one, n.p_two, Sonce);
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


                        if (n.xID0 != "" || n.xID1 != "" || n.xID2 != "" || n.xID3 != "" || n.xID4 != "")
                        {
                            builder.Append("(有受限)");
                        }
                        if (n.p_isluck == 1)
                        {
                            builder.Append("(已封)");
                        }

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
                IList<TPR2.Model.guess.BaList> listBaListLX = new TPR2.BLL.guess.BaList().GetBaListLX(pageIndex, pageSize, strLXWhere, out recordCount);
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

                        builder.AppendFormat(Out.waplink(Utils.getUrl("default.aspx?ptype=4&amp;fly={0}"), "-{1}(共{2}场)") + "", n.p_title, n.p_title, new TPR2.BLL.guess.BaList().GetCountByp_title(n.p_title));
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
            string strOthe = "搜赛事," + Utils.getUrl("default.aspx") + ",post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            if (keyword != "")
            {
                builder.Append(Out.Tab("<div>", " "));
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">返回</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=zddel") + "\">[隐藏全部" + ub.Get("SiteGqText") + "]</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx?act=zddel2") + "\">[显示全部" + ub.Get("SiteGqText") + "]</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("fbRobotset.aspx") + "\">机器人设置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("clear.aspx") + "\">清空记录</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("../xml/guessset2.aspx?backurl=" + Utils.PostPage(1) + "&amp;ve=2a") + "\">游戏配置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("../xml/guessset2.aspx?act=guessjc") + "\">抓取配置</a><br />");
            //if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th"))
            //    builder.Append("<a href=\"" + Utils.getUrl("http://jc.tuhao111.com/a/guessset2.aspx?ve=2a") + "\">抓取配置(弃用)</a><br />");
            //else if (Utils.GetTopDomain().Contains("kb288.net"))
            //    builder.Append("<a href=\"" + Utils.getUrl("http://jc.kb288.cn/a/guessset2.aspx?ve=2a") + "\">抓取配置(弃用)</a><br />");
            //else
            //    builder.Append("<a href=\"" + Utils.getUrl("http://jc.kb288.cn/a/guessset2.aspx?ve=2a") + "\">抓取配置(弃用)</a><br />");            
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        #endregion
    }

    public string MoneyToChinese(string LowerMoney)
    {
        string functionReturnValue = null;
        bool IsNegative = false; // 是否是负数
        if (LowerMoney.Trim().Substring(0, 1) == "-")
        {
            // 是负数则先转为正数
            LowerMoney = LowerMoney.Trim().Remove(0, 1);
            IsNegative = true;
        }
        string strLower = null;
        string strUpart = null;
        string strUpper = null;
        int iTemp = 0;
        // 保留两位小数 123.489→123.49　　123.4→123.4
        LowerMoney = Math.Round(double.Parse(LowerMoney), 2).ToString();
        if (LowerMoney.IndexOf(".") > 0)
        {
            if (LowerMoney.IndexOf(".") == LowerMoney.Length - 2)
            {
                LowerMoney = LowerMoney + "0";
            }
        }
        else
        {
            LowerMoney = LowerMoney + ".00";
        }
        strLower = LowerMoney;
        iTemp = 1;
        strUpper = "";
        while (iTemp <= strLower.Length)
        {
            switch (strLower.Substring(strLower.Length - iTemp, 1))
            {
                case ".":
                    strUpart = "圆";
                    break;
                case "0":
                    strUpart = "零";
                    break;
                case "1":
                    strUpart = "壹";
                    break;
                case "2":
                    strUpart = "贰";
                    break;
                case "3":
                    strUpart = "叁";
                    break;
                case "4":
                    strUpart = "肆";
                    break;
                case "5":
                    strUpart = "伍";
                    break;
                case "6":
                    strUpart = "陆";
                    break;
                case "7":
                    strUpart = "柒";
                    break;
                case "8":
                    strUpart = "捌";
                    break;
                case "9":
                    strUpart = "玖";
                    break;
            }

            switch (iTemp)
            {
                case 1:
                    strUpart = strUpart + "分";
                    break;
                case 2:
                    strUpart = strUpart + "角";
                    break;
                case 3:
                    strUpart = strUpart + "";
                    break;
                case 4:
                    strUpart = strUpart + "";
                    break;
                case 5:
                    strUpart = strUpart + "拾";
                    break;
                case 6:
                    strUpart = strUpart + "佰";
                    break;
                case 7:
                    strUpart = strUpart + "仟";
                    break;
                case 8:
                    strUpart = strUpart + "万";
                    break;
                case 9:
                    strUpart = strUpart + "拾";
                    break;
                case 10:
                    strUpart = strUpart + "佰";
                    break;
                case 11:
                    strUpart = strUpart + "仟";
                    break;
                case 12:
                    strUpart = strUpart + "亿";
                    break;
                case 13:
                    strUpart = strUpart + "拾";
                    break;
                case 14:
                    strUpart = strUpart + "佰";
                    break;
                case 15:
                    strUpart = strUpart + "仟";
                    break;
                case 16:
                    strUpart = strUpart + "万";
                    break;
                default:
                    strUpart = strUpart + "";
                    break;
            }

            strUpper = strUpart + strUpper;
            iTemp = iTemp + 1;
        }

        strUpper = strUpper.Replace("零拾", "零");
        strUpper = strUpper.Replace("零佰", "零");
        strUpper = strUpper.Replace("零仟", "零");
        strUpper = strUpper.Replace("零零零", "零");
        strUpper = strUpper.Replace("零零", "零");
        strUpper = strUpper.Replace("零角零分", "整");
        strUpper = strUpper.Replace("零分", "整");
        strUpper = strUpper.Replace("零角", "零");
        strUpper = strUpper.Replace("零亿零万零圆", "亿圆");
        strUpper = strUpper.Replace("亿零万零圆", "亿圆");
        strUpper = strUpper.Replace("零亿零万", "亿");
        strUpper = strUpper.Replace("零万零圆", "万圆");
        strUpper = strUpper.Replace("零亿", "亿");
        strUpper = strUpper.Replace("零万", "万");
        strUpper = strUpper.Replace("零圆", "圆");
        strUpper = strUpper.Replace("零零", "零");

        // 对壹圆以下的金额的处理
        if (strUpper.Substring(0, 1) == "圆")
        {
            strUpper = strUpper.Substring(1, strUpper.Length - 1);
        }
        if (strUpper.Substring(0, 1) == "零")
        {
            strUpper = strUpper.Substring(1, strUpper.Length - 1);
        }
        if (strUpper.Substring(0, 1) == "角")
        {
            strUpper = strUpper.Substring(1, strUpper.Length - 1);
        }
        if (strUpper.Substring(0, 1) == "分")
        {
            strUpper = strUpper.Substring(1, strUpper.Length - 1);
        }
        if (strUpper.Substring(0, 1) == "整")
        {
            strUpper = "零圆整";
        }
        functionReturnValue = strUpper;
        functionReturnValue = functionReturnValue.Replace("圆", "").Replace("整", "");
        if (IsNegative == true)
        {
            return "负" + functionReturnValue;
        }
        else
        {
            return functionReturnValue;
        }
    }

}