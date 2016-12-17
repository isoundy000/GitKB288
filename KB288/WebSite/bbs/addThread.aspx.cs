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
/// 陈志基 16-5-17
/// 发帖向某ID提示已发帖
/// </summary>
/// 
/// <summary>
/// 蒙宗将 20160822 撤掉抽奖值生成
/// </summary>
/// 
public partial class bbs_addThread : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/bbs.xml";

    protected string strText = string.Empty;
    protected string strName = string.Empty;
    protected string strType = string.Empty;
    protected string strValu = string.Empty;
    protected string strEmpt = string.Empty;
    protected string strIdea = string.Empty;
    protected string strOthe = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "help":
                HelpPage();
                break;
            case "save":
                SavePage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        Master.Title = "发表话题";

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int forumid = int.Parse(Utils.GetRequest("forumid", "all", 1, @"^[0-9]\d*$", "0"));
        int dd = int.Parse(Utils.GetRequest("dd", "all", 1, @"^[0-9]\d*$", "0"));
        int ptype = 0;
        //if (forumid == 3)
        //    ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-9]\d*$", "6"));
        if (forumid == 69)
            ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-9]\d*$", "7"));
        else
            ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-9]\d*$", "0"));

        int copy = int.Parse(Utils.GetRequest("copy", "all", 1, @"^[0-1]$", "0"));
        int ff = int.Parse(Utils.GetRequest("ff", "all", 1, @"^[0-9]\d*$", "-1"));

        if (forumid == 0)
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("请选择发表到的版块:");
            builder.Append(Out.Tab("</div>", Out.Hr()));
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string strWhere = string.Empty;
            string strOrder = string.Empty;
            string[] pageValUrl = { "dd" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            //查询条件
            strWhere = "IsActive=0";

            // 开始读取论坛
            IList<BCW.Model.Forum> listForum = new BCW.BLL.Forum().GetForums(pageIndex, pageSize, strWhere, out recordCount);
            if (listForum.Count > 0)
            {
                int k = 1;
                foreach (BCW.Model.Forum n in listForum)
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
                    builder.Append("<a href=\"" + Utils.getUrl("addThread.aspx?forumid=" + n.ID + "&amp;dd=" + dd + "") + "\">" + n.Title + "</a>");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));

                }

                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("text", "没有版块记录"));
            }
        }
        else
        {
            if (!new BCW.BLL.Forum().Exists2(forumid))
            {
                Utils.Error("不存在的论坛或此论坛已暂停使用", "");
            }

            //高手统计论坛
            if (string.IsNullOrEmpty(Request["ptype"]))
            {
                if (new BCW.User.ForumInc().IsForumGSIDS(forumid))
                {
                    new Out().head(Utils.ForWordType("发表帖子"));
                    Response.Write(Out.Tab("<div class=\"title\">", ""));
                    Response.Write("<a href=\"" + Utils.getUrl("addThread.aspx?forumid=" + forumid + "&amp;ptype=8") + "\">1.发表参赛帖</a><br />");
                    Response.Write("<a href=\"" + Utils.getUrl("addThread.aspx?forumid=" + forumid + "&amp;ptype=0") + "\">2.发表普通帖</a>");
                    Response.Write(Out.Tab("</div>", ""));
                    Response.Write(Out.Tab("<div class=\"title\">", Out.Hr()));
                    Response.Write("<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "") + "\">返回上级</a>");
                    Response.Write(Out.Tab("</div>", ""));
                    Response.Write(new Out().foot());
                    Response.End();
                }
                else if (new BCW.User.ForumInc().IsForum68(forumid))
                {
                    new Out().head(Utils.ForWordType("发表帖子"));
                    Response.Write(Out.Tab("<div class=\"title\">", ""));
                    Response.Write("<a href=\"" + Utils.getUrl("addThread.aspx?forumid=" + forumid + "&amp;ptype=6") + "\">1.发表竞技帖</a><br />");
                    Response.Write("<a href=\"" + Utils.getUrl("addThread.aspx?forumid=" + forumid + "&amp;ptype=0") + "\">2.发表普通帖</a>");
                    Response.Write(Out.Tab("</div>", ""));
                    Response.Write(Out.Tab("<div class=\"title\">", Out.Hr()));
                    Response.Write("<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "") + "\">返回上级</a>");
                    Response.Write(Out.Tab("</div>", ""));
                    Response.Write(new Out().foot());
                    Response.End();
                }
            }

            BCW.Model.Forum model = new BCW.BLL.Forum().GetForum(forumid);

            if (ptype == 8)
            {
                string GsDemoID = ub.GetSub("BbsGsDemoID", xmlPath);
                if (GsDemoID != "")
                {
                    if (!("#" + GsDemoID + "#").Contains("#" + meid + "#"))
                    {
                        Utils.Error("此功能内测中，您的ID不在内测ID里", "");
                    }
                }
                DateTime GsStopTime = Utils.ParseTime(ub.GetSub("BbsGsStopTime", xmlPath));
                int GsqiNum = Utils.ParseInt(ub.GetSub("BbsGsqiNum", xmlPath));
                if (GsqiNum == 0)
                {
                    Utils.Error("本期尚未设置", "");
                }
                if (GsStopTime < DateTime.Now)
                {
                    Utils.Error("本期第" + GsqiNum + "期已截止发表，截止时间" + GsStopTime + "", "");
                }
                if (new BCW.User.ForumInc().IsForumGSIDS(forumid))
                {
                    builder.Append(Out.Tab("<div class=\"title\">", ""));
                    builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "") + "\">" + model.Title + "</a>");
                    builder.Append("&gt;发表参赛帖");
                    builder.Append(Out.Tab("</div>", "<br />"));

                    string ac = Utils.GetRequest("ac", "post", 1, "", "");
                    string Choose = Utils.GetRequest("Choose", "post", 1, "", "");
                    string Choose2 = Utils.GetRequest("Choose2", "post", 1, "", "");

                  
                            if (new BCW.User.ForumInc().IsForum113(forumid)==true||new BCW.User.ForumInc().IsForum116(forumid))
                {
                        if (ac != "" && ac != "[删]")
                        {
                            if (Choose != "")
                            {
                                Choose += "," + Utils.ToSChinese(ac);
                            }
                            else
                            {
                                Choose = Utils.ToSChinese(ac);
                            }
                        } 
 
                        string show1 = "";
                        string show2 = "";
                        string show3 = "";
                        string show4 = "";
                        string show5 = "";
                        string show6 = "";
                        string show7 = "";
                        string show8 = "";
                        string show9 = "";
                        string show10 = "";
                        string show11 = "";
                        string show12 = "";
                        int cNum = Utils.GetStringNum(Choose, ",");
                        if ((new BCW.User.ForumInc().IsForum113(forumid) == true && cNum == 5) || (new BCW.User.ForumInc().IsForum116(forumid) == true && ac != "" && ac != "[删]"))
                        {
                            show1 = "$";
                            show2 = "$";
                            show3 = "$";
                            show4 = "$";
                            show5 = "$";
                            show6 = "$";
                            show7 = "$";
                            show8 = "$";
                            show9 = "$";
                            show10 = "$";
                            show11 = "$";
                            show12 = "$";
                        }
                        else
                        {
                            if (Choose.Contains("鼠"))
                                show1 = "$";
                            if (Choose.Contains("牛"))
                                show2 = "$";
                            if (Choose.Contains("虎"))
                                show3 = "$";
                            if (Choose.Contains("兔"))
                                show4 = "$";
                            if (Choose.Contains("龙"))
                                show5 = "$";
                            if (Choose.Contains("蛇"))
                                show6 = "$";
                            if (Choose.Contains("马"))
                                show7 = "$";
                            if (Choose.Contains("羊"))
                                show8 = "$";
                            if (Choose.Contains("猴"))
                                show9 = "$";
                            if (Choose.Contains("鸡"))
                                show10 = "$";
                            if (Choose.Contains("狗"))
                                show11 = "$";
                            if (Choose.Contains("猪"))
                                show12 = "$";
                        }


                        string sx = "" + show1 + "鼠|" + show2 + "牛|" + show3 + "虎|" + show4 + "兔|" + show5 + "龙|" + show6 + "蛇/|" + show7 + "马|" + show8 + "羊|" + show9 + "猴|" + show10 + "鸡|" + show11 + "狗|" + show12 + "猪";

                        builder.Append(Out.Tab("<div>", ""));
                        builder.Append("选择要推荐的生肖:");
                        builder.Append(Out.Tab("</div>", Out.Hr()));
                        builder.Append(Out.Tab("<div>", ""));
                        strName = "Choose,forumid,ptype,act,info";
                        strValu = "" + Choose + "'" + forumid + "'" + ptype + "'view'ok";
                        strOthe = "" + sx + ",addThread.aspx,post,3,other|other|other|other|other|other|other|other|other|other|other|other";
                        builder.Append(Out.wapform(strName, strValu, strOthe));
                        builder.Append(Out.Tab("</div>", ""));
                        if (Choose != "")
                        {
                            builder.Append(Out.Tab("<div>", "<br />"));
                            builder.Append("=选中的生肖=<br />");

                            string[] cTemp = Choose.Split(',');

                            for (int i = 0; i < cTemp.Length; i++)
                            {
                                builder.Append("" + cTemp[i] + "");
                                strName = "Choose,forumid,ptype,act,info";
                                if (i == 0)
                                    strValu = "" + Choose.Replace(cTemp[i] + ",", "").Replace(cTemp[i], "") + "'" + forumid + "'" + ptype + "'view'ok";
                                else
                                    strValu = "" + Choose.Replace("," + cTemp[i], "").Replace(cTemp[i], "") + "'" + forumid + "'" + ptype + "'view'ok";

                                strOthe = "[删],addThread.aspx,post,3,other";
                                builder.Append(Out.wapform(strName, strValu, strOthe));
                            }
                            builder.Append(Out.Tab("</div>", ""));
                        }
                    }
                            else if (new BCW.User.ForumInc().IsForum114(forumid) == true)
                    {

                        string showA1 = "";
                        string showA2 = "";
                        string showA3 = "";
                        string showB1 = "";
                        string showB2 = "";
                        string showB3 = "";
                        string showB4 = "";
                        string showB5 = "";
                        string showB6 = "";

                        if (ac != "" && ac != "[删]" && !ac.Contains("单") && !ac.Contains("双"))
                        {
                            if (Choose != "")
                            {
                                Choose += "," + Utils.ToSChinese(ac);
                            }
                            else
                            {
                                Choose = Utils.ToSChinese(ac);
                            }
                            int cNum = Utils.GetStringNum(Choose, ",");
                            if (cNum >= 1)
                            {
                                showA1 = "$";
                                showA2 = "$";
                                showA3 = "$";
                            }
                            showB1 = "$";
                            showB2 = "$";
                            showB3 = "$";
                            showB4 = "$";
                            showB5 = "$";
                            showB6 = "$";
                        }
                        if (ac != "" && ac != "[删]" && (ac.Contains("单") || ac.Contains("双")))
                        {
                            if (Choose2 != "")
                            {
                                Choose2 += "," + Utils.ToSChinese(ac);
                            }
                            else
                            {
                                Choose2 = Utils.ToSChinese(ac);
                            }
                            int cNum = Utils.GetStringNum(Choose2, ",");
                            if (cNum >= 3)
                            {
                                showB1 = "$";
                                showB2 = "$";
                                showB3 = "$";
                                showB4 = "$";
                                showB5 = "$";
                                showB6 = "$";
                            }
                            showA1 = "$";
                            showA2 = "$";
                            showA3 = "$";
                        }

                        if (Choose.Contains("红波"))
                            showA1 = "$";
                        if (Choose.Contains("蓝波"))
                            showA2 = "$";
                        if (Choose.Contains("绿波"))
                            showA3 = "$";

                        if (Choose2.Contains("红单"))
                            showB1 = "$";
                        if (Choose2.Contains("蓝单"))
                            showB2 = "$";
                        if (Choose2.Contains("绿单"))
                            showB3 = "$";
                        if (Choose2.Contains("红双"))
                            showB4 = "$";
                        if (Choose2.Contains("蓝双"))
                            showB5 = "$";
                        if (Choose2.Contains("绿双"))
                            showB6 = "$";

                        string bs = "" + showA1 + "红波|" + showA2 + "蓝波|" + showA3 + "绿波";
                        string bs2 = "" + showB1 + "红单|" + showB2 + "蓝单|" + showB3 + "绿单/|" + showB4 + "红双|" + showB5 + "蓝双|" + showB6 + "绿双";

                        builder.Append(Out.Tab("<div>", Out.RHr()));
                        builder.Append("=选择要推荐的波色=");
                        builder.Append(Out.Tab("</div>", "<br />"));
                        builder.Append(Out.Tab("<div>", ""));
                        strName = "Choose,forumid,ptype,act,info";
                        strValu = "" + Choose + "'" + forumid + "'" + ptype + "'view'ok";
                        strOthe = "" + bs + ",addThread.aspx,post,3,other|other|other";
                        builder.Append(Out.wapform(strName, strValu, strOthe));
                        builder.Append(Out.Tab("</div>", ""));

                        builder.Append(Out.Tab("<div>", Out.Hr()));
                        builder.Append("=选择要推荐的半波=");
                        builder.Append(Out.Tab("</div>", "<br />"));
                        builder.Append(Out.Tab("<div>", ""));
                        strName = "Choose2,forumid,ptype,act,info";
                        strValu = "" + Choose2 + "'" + forumid + "'" + ptype + "'view'ok";
                        strOthe = "" + bs2 + ",addThread.aspx,post,3,other|other|other|other|other|other";
                        builder.Append(Out.wapform(strName, strValu, strOthe));
                        builder.Append(Out.Tab("</div>", ""));
                        if (Choose != "")
                        {
                            builder.Append(Out.Tab("<div>", "<br />"));
                            builder.Append("=选中的波色=<br />");

                            string[] cTemp = Choose.Split(',');

                            for (int i = 0; i < cTemp.Length; i++)
                            {
                                builder.Append("" + cTemp[i] + "");
                                strName = "Choose,forumid,ptype,act,info";
                                if (i == 0)
                                    strValu = "" + Choose.Replace(cTemp[i] + ",", "").Replace(cTemp[i], "") + "'" + forumid + "'" + ptype + "'view'ok";
                                else
                                    strValu = "" + Choose.Replace("," + cTemp[i], "").Replace(cTemp[i], "") + "'" + forumid + "'" + ptype + "'view'ok";

                                strOthe = "[删],addThread.aspx,post,3,other";
                                builder.Append(Out.wapform(strName, strValu, strOthe));
                            }
                            builder.Append(Out.Tab("</div>", ""));
                        }
                        if (Choose2 != "")
                        {
                            builder.Append(Out.Tab("<div>", "<br />"));
                            builder.Append("=选中的半波=<br />");

                            string[] cTemp = Choose2.Split(',');

                            for (int i = 0; i < cTemp.Length; i++)
                            {
                                builder.Append("" + cTemp[i] + "");
                                strName = "Choose2,forumid,ptype,act,info";
                                if (i == 0)
                                    strValu = "" + Choose2.Replace(cTemp[i] + ",", "").Replace(cTemp[i], "") + "'" + forumid + "'" + ptype + "'view'ok";
                                else
                                    strValu = "" + Choose2.Replace("," + cTemp[i], "").Replace(cTemp[i], "") + "'" + forumid + "'" + ptype + "'view'ok";

                                strOthe = "[删],addThread.aspx,post,3,other";
                                builder.Append(Out.wapform(strName, strValu, strOthe));
                            }
                            builder.Append(Out.Tab("</div>", ""));
                            Choose = Choose2;
                        }
                    }
                            else if (new BCW.User.ForumInc().IsForum117(forumid) == true)
                    {
                        if (ac != "" && ac != "[删]")
                        {
                            if (Choose != "")
                            {
                                Choose += "," + Utils.ToSChinese(ac);
                            }
                            else
                            {
                                Choose = Utils.ToSChinese(ac);
                            }
                        }

                        string show1 = "";
                        string show2 = "";
                        string show3 = "";
                        string show4 = "";
                        string show5 = "";
                        string show6 = "";
                        string show7 = "";
                        string show8 = "";
                        string show9 = "";
                        string show10 = "";
                        string show11 = "";
                        string show12 = "";
                        string show13 = "";
                        string show14 = "";
                        string show15 = "";
                        string show16 = "";
                        string show17 = "";
                        string show18 = "";
                        string show19 = "";
                        string show20 = "";
                        string show21 = "";
                        string show22 = "";
                        string show23 = "";
                        string show24 = "";
                        string show25 = "";
                        string show26 = "";
                        string show27 = "";
                        string show28 = "";
                        string show29 = "";
                        string show30 = "";
                        string show31 = "";
                        string show32 = "";
                        string show33 = "";
                        string show34 = "";
                        string show35 = "";
                        string show36 = "";
                        string show37 = "";
                        string show38 = "";
                        string show39 = "";
                        string show40 = "";
                        string show41 = "";
                        string show42 = "";
                        string show43 = "";
                        string show44 = "";
                        string show45 = "";
                        string show46 = "";
                        string show47 = "";
                        string show48 = "";
                        string show49 = "";

                        int cNum = Utils.GetStringNum(Choose, ",");
                        if (cNum == 4)
                        {
                            show1 = "$";
                            show2 = "$";
                            show3 = "$";
                            show4 = "$";
                            show5 = "$";
                            show6 = "$";
                            show7 = "$";
                            show8 = "$";
                            show9 = "$";
                            show10 = "$";
                            show11 = "$";
                            show12 = "$";
                            show13 = "$";
                            show14 = "$";
                            show15 = "$";
                            show16 = "$";
                            show17 = "$";
                            show18 = "$";
                            show19 = "$";
                            show20 = "$";
                            show21 = "$";
                            show22 = "$";
                            show23 = "$";
                            show24 = "$";
                            show25 = "$";
                            show26 = "$";
                            show27 = "$";
                            show28 = "$";
                            show29 = "$";
                            show30 = "$";
                            show31 = "$";
                            show32 = "$";
                            show33 = "$";
                            show34 = "$";
                            show35 = "$";
                            show36 = "$";
                            show37 = "$";
                            show38 = "$";
                            show39 = "$";
                            show40 = "$";
                            show41 = "$";
                            show42 = "$";
                            show43 = "$";
                            show44 = "$";
                            show45 = "$";
                            show46 = "$";
                            show47 = "$";
                            show48 = "$";
                            show49 = "$";
                         
                        }
                        else
                        {
                            if (Choose.Contains("01")) show1 = "$";
                            if (Choose.Contains("02")) show2 = "$";
                            if (Choose.Contains("03")) show3 = "$";
                            if (Choose.Contains("04")) show4 = "$";
                            if (Choose.Contains("05")) show5 = "$";
                            if (Choose.Contains("06")) show6 = "$";
                            if (Choose.Contains("07")) show7 = "$";
                            if (Choose.Contains("08")) show8 = "$";
                            if (Choose.Contains("09")) show9 = "$";
                            if (Choose.Contains("10")) show10 = "$";
                            if (Choose.Contains("11")) show11 = "$";
                            if (Choose.Contains("12")) show12 = "$";
                            if (Choose.Contains("13")) show13 = "$";
                            if (Choose.Contains("14")) show14 = "$";
                            if (Choose.Contains("15")) show15 = "$";
                            if (Choose.Contains("16")) show16 = "$";
                            if (Choose.Contains("17")) show17 = "$";
                            if (Choose.Contains("18")) show18 = "$";
                            if (Choose.Contains("19")) show19 = "$";
                            if (Choose.Contains("20")) show20 = "$";
                            if (Choose.Contains("21")) show21 = "$";
                            if (Choose.Contains("22")) show22 = "$";
                            if (Choose.Contains("23")) show23 = "$";
                            if (Choose.Contains("24")) show24 = "$";
                            if (Choose.Contains("25")) show25 = "$";
                            if (Choose.Contains("26")) show26 = "$";
                            if (Choose.Contains("27")) show27 = "$";
                            if (Choose.Contains("28")) show28 = "$";
                            if (Choose.Contains("29")) show29 = "$";
                            if (Choose.Contains("30")) show30 = "$";
                            if (Choose.Contains("31")) show31 = "$";
                            if (Choose.Contains("32")) show32 = "$";
                            if (Choose.Contains("33")) show33 = "$";
                            if (Choose.Contains("34")) show34 = "$";
                            if (Choose.Contains("35")) show35 = "$";
                            if (Choose.Contains("36")) show36 = "$";
                            if (Choose.Contains("37")) show37 = "$";
                            if (Choose.Contains("38")) show38 = "$";
                            if (Choose.Contains("39")) show39 = "$";
                            if (Choose.Contains("40")) show40 = "$";
                            if (Choose.Contains("41")) show41 = "$";
                            if (Choose.Contains("42")) show42 = "$";
                            if (Choose.Contains("43")) show43 = "$";
                            if (Choose.Contains("44")) show44 = "$";
                            if (Choose.Contains("45")) show45 = "$";
                            if (Choose.Contains("46")) show46 = "$";
                            if (Choose.Contains("47")) show47 = "$";
                            if (Choose.Contains("48")) show48 = "$";
                            if (Choose.Contains("49")) show49 = "$";
                        }

                        string sNum = "" + show1 + "01|" + show2 + "02|" + show3 + "03|" + show4 + "04|" + show5 + "05|" + show6 + "06|" + show7 + "07/|" + show8 + "08|" + show9 + "09|" + show10 + "10|" + show11 + "11|" + show12 + "12|" + show13 + "13|" + show14 + "14/|" + show15 + "15|" + show16 + "16|" + show17 + "17|" + show18 + "18|" + show19 + "19|" + show20 + "20|" + show21 + "21/|" + show22 + "22|" + show23 + "23|" + show24 + "24|" + show25 + "25|" + show26 + "26|" + show27 + "27|" + show28 + "28/|" + show29 + "29|" + show30 + "30|" + show31 + "31|" + show32 + "32|" + show33 + "33|" + show34 + "34|" + show35 + "35/|" + show36 + "36|" + show37 + "37|" + show38 + "38|" + show39 + "39|" + show40 + "40|" + show41 + "41|" + show42 + "42/|" + show43 + "43|" + show44 + "44|" + show45 + "45|" + show46 + "46|" + show47 + "47|" + show48 + "48|" + show49 + "49";
                        builder.Append(Out.Tab("<div>", ""));
                        builder.Append("选择要推荐的5个数字:");
                        builder.Append(Out.Tab("</div>", Out.Hr()));
                        builder.Append(Out.Tab("<div>", ""));
                        strName = "Choose,forumid,ptype,act,info";
                        strValu = "" + Choose + "'" + forumid + "'" + ptype + "'view'ok";
                        strOthe = "" + sNum + ",addThread.aspx,post,3,other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other";
                        builder.Append(Out.wapform(strName, strValu, strOthe));
                        builder.Append(Out.Tab("</div>", ""));
                        if (Choose != "")
                        {
                            builder.Append(Out.Tab("<div>", "<br />"));
                            builder.Append("=选中的数字=<br />");

                            string[] cTemp = Choose.Split(',');

                            for (int i = 0; i < cTemp.Length; i++)
                            {
                                builder.Append("" + cTemp[i] + "");
                                strName = "Choose,forumid,ptype,act,info";
                                if (i == 0)
                                    strValu = "" + Choose.Replace(cTemp[i] + ",", "").Replace(cTemp[i], "") + "'" + forumid + "'" + ptype + "'view'ok";
                                else
                                    strValu = "" + Choose.Replace("," + cTemp[i], "").Replace(cTemp[i], "") + "'" + forumid + "'" + ptype + "'view'ok";

                                strOthe = "[删],addThread.aspx,post,3,other";
                                builder.Append(Out.wapform(strName, strValu, strOthe));
                            }
                            builder.Append(Out.Tab("</div>", ""));
                        }
                    
                    }
                            else if (new BCW.User.ForumInc().IsForum119(forumid) == true)
                    {
                        if (ac != "" && ac != "[删]")
                        {
                            if (Choose != "")
                            {
                                Choose += "," + Utils.ToSChinese(ac);
                            }
                            else
                            {
                                Choose = Utils.ToSChinese(ac);
                            }
                        }

                        string show1 = "";
                        string show2 = "";
                        string show3 = "";
                        string show4 = "";
                        string show5 = "";
                        string show6 = "";
                        string show7 = "";
                        string show8 = "";
                        string show9 = "";
                        string show10 = "";

                        int cNum = Utils.GetStringNum(Choose, ",");
                        if (cNum == 4)
                        {
                            show1 = "$";
                            show2 = "$";
                            show3 = "$";
                            show4 = "$";
                            show5 = "$";
                            show6 = "$";
                            show7 = "$";
                            show8 = "$";
                            show9 = "$";
                            show10 = "$";
                        }
                        else
                        {
                            if (Choose.Contains("0尾"))
                                show1 = "$";
                            if (Choose.Contains("1尾"))
                                show2 = "$";
                            if (Choose.Contains("2尾"))
                                show3 = "$";
                            if (Choose.Contains("3尾"))
                                show4 = "$";
                            if (Choose.Contains("4尾"))
                                show5 = "$";
                            if (Choose.Contains("5尾"))
                                show6 = "$";
                            if (Choose.Contains("6尾"))
                                show7 = "$";
                            if (Choose.Contains("7尾"))
                                show8 = "$";
                            if (Choose.Contains("8尾"))
                                show9 = "$";
                            if (Choose.Contains("9尾"))
                                show10 = "$";

                        }

                        string ws = "" + show1 + "0尾|" + show2 + "1尾|" + show3 + "2尾|" + show4 + "3尾|" + show5 + "4尾/|" + show6 + "5尾|" + show7 + "6尾|" + show8 + "7尾|" + show9 + "8尾|" + show10 + "9尾";

                        builder.Append(Out.Tab("<div>", ""));
                        builder.Append("选择要推荐的5个尾数:");
                        builder.Append(Out.Tab("</div>", Out.Hr()));
                        builder.Append(Out.Tab("<div>", ""));
                        strName = "Choose,forumid,ptype,act,info";
                        strValu = "" + Choose + "'" + forumid + "'" + ptype + "'view'ok";
                        strOthe = "" + ws + ",addThread.aspx,post,3,other|other|other|other|other|other|other|other|other|other";
                        builder.Append(Out.wapform(strName, strValu, strOthe));
                        builder.Append(Out.Tab("</div>", ""));
                        if (Choose != "")
                        {
                            builder.Append(Out.Tab("<div>", "<br />"));
                            builder.Append("=选中的特尾=<br />");

                            string[] cTemp = Choose.Split(',');

                            for (int i = 0; i < cTemp.Length; i++)
                            {
                                builder.Append("" + cTemp[i] + "");
                                strName = "Choose,forumid,ptype,act,info";
                                if (i == 0)
                                    strValu = "" + Choose.Replace(cTemp[i] + ",", "").Replace(cTemp[i], "") + "'" + forumid + "'" + ptype + "'view'ok";
                                else
                                    strValu = "" + Choose.Replace("," + cTemp[i], "").Replace(cTemp[i], "") + "'" + forumid + "'" + ptype + "'view'ok";

                                strOthe = "[删],addThread.aspx,post,3,other";
                                builder.Append(Out.wapform(strName, strValu, strOthe));
                            }
                            builder.Append(Out.Tab("</div>", ""));
                        }

                    }
                            else if (new BCW.User.ForumInc().IsForum115(forumid) == true)
                    {
                        if (ac != "" && ac != "[删]")
                        {
                            if (Choose != "")
                            {
                                Choose += "," + Utils.ToSChinese(ac);
                            }
                            else
                            {
                                Choose = Utils.ToSChinese(ac);
                            }
                        }

                        string show1 = "";
                        string show2 = "";
                        if (ac != "" && ac != "[删]")
                        {
                            show1 = "$";
                            show2 = "$";

                        }
                        else
                        {
                            if (Choose.Contains("大数"))
                                show1 = "$";
                            if (Choose.Contains("小数"))
                                show2 = "$";

                        }

                        string acType = "" + show1 + "大数|" + show2 + "小数";

                        builder.Append(Out.Tab("<div>", ""));
                        builder.Append("选择要推荐的大小:");
                        builder.Append(Out.Tab("</div>", Out.Hr()));
                        builder.Append(Out.Tab("<div>", ""));
                        strName = "Choose,forumid,ptype,act,info";
                        strValu = "" + Choose + "'" + forumid + "'" + ptype + "'view'ok";
                        strOthe = "" + acType + ",addThread.aspx,post,3,other|other|other|other|other|other|other|other|other|other";
                        builder.Append(Out.wapform(strName, strValu, strOthe));
                        builder.Append(Out.Tab("</div>", ""));
                        if (Choose != "")
                        {
                            builder.Append(Out.Tab("<div>", "<br />"));
                            builder.Append("=选中的大小=<br />");

                            string[] cTemp = Choose.Split(',');

                            for (int i = 0; i < cTemp.Length; i++)
                            {
                                builder.Append("" + cTemp[i] + "");
                                strName = "Choose,forumid,ptype,act,info";
                                if (i == 0)
                                    strValu = "" + Choose.Replace(cTemp[i] + ",", "").Replace(cTemp[i], "") + "'" + forumid + "'" + ptype + "'view'ok";
                                else
                                    strValu = "" + Choose.Replace("," + cTemp[i], "").Replace(cTemp[i], "") + "'" + forumid + "'" + ptype + "'view'ok";

                                strOthe = "[删],addThread.aspx,post,3,other";
                                builder.Append(Out.wapform(strName, strValu, strOthe));
                            }
                            builder.Append(Out.Tab("</div>", ""));
                        }
                    }
                            else if (new BCW.User.ForumInc().IsForum121(forumid) == true)
                    {
                        if (ac != "" && ac != "[删]")
                        {
                            if (Choose != "")
                            {
                                Choose += "," + Utils.ToSChinese(ac);
                            }
                            else
                            {
                                Choose = Utils.ToSChinese(ac);
                            }
                        }

                        string show1 = "";
                        string show2 = "";
                        if (ac != "" && ac != "[删]")
                        {
                            show1 = "$";
                            show2 = "$";

                        }
                        else
                        {
                            if (Choose.Contains("单数"))
                                show1 = "$";
                            if (Choose.Contains("双数"))
                                show2 = "$";

                        }

                        string acType = "" + show1 + "单数|" + show2 + "双数";

                        builder.Append(Out.Tab("<div>", ""));
                        builder.Append("选择要推荐的单双:");
                        builder.Append(Out.Tab("</div>", Out.Hr()));
                        builder.Append(Out.Tab("<div>", ""));
                        strName = "Choose,forumid,ptype,act,info";
                        strValu = "" + Choose + "'" + forumid + "'" + ptype + "'view'ok";
                        strOthe = "" + acType + ",addThread.aspx,post,3,other|other|other|other|other|other|other|other|other|other";
                        builder.Append(Out.wapform(strName, strValu, strOthe));
                        builder.Append(Out.Tab("</div>", ""));
                        if (Choose != "")
                        {
                            builder.Append(Out.Tab("<div>", "<br />"));
                            builder.Append("=选中的单双=<br />");

                            string[] cTemp = Choose.Split(',');

                            for (int i = 0; i < cTemp.Length; i++)
                            {
                                builder.Append("" + cTemp[i] + "");
                                strName = "Choose,forumid,ptype,act,info";
                                if (i == 0)
                                    strValu = "" + Choose.Replace(cTemp[i] + ",", "").Replace(cTemp[i], "") + "'" + forumid + "'" + ptype + "'view'ok";
                                else
                                    strValu = "" + Choose.Replace("," + cTemp[i], "").Replace(cTemp[i], "") + "'" + forumid + "'" + ptype + "'view'ok";

                                strOthe = "[删],addThread.aspx,post,3,other";
                                builder.Append(Out.wapform(strName, strValu, strOthe));
                            }
                            builder.Append(Out.Tab("</div>", ""));
                        }
                    }
                            else if (new BCW.User.ForumInc().IsForum122(forumid) == true)
                    {
                        if (ac != "" && ac != "[删]")
                        {
                            if (Choose != "")
                            {
                                Choose += "," + Utils.ToSChinese(ac);
                            }
                            else
                            {
                                Choose = Utils.ToSChinese(ac);
                            }
                        }

                        string show1 = "";
                        string show2 = "";
                        if (ac != "" && ac != "[删]")
                        {
                            show1 = "$";
                            show2 = "$";

                        }
                        else
                        {
                            if (Choose.Contains("家禽"))
                                show1 = "$";
                            if (Choose.Contains("野兽"))
                                show2 = "$";

                        }

                        string acType = "" + show1 + "家禽|" + show2 + "野兽";

                        builder.Append(Out.Tab("<div>", ""));
                        builder.Append("选择要推荐的家野:");
                        builder.Append(Out.Tab("</div>", Out.Hr()));
                        builder.Append(Out.Tab("<div>", ""));
                        strName = "Choose,forumid,ptype,act,info";
                        strValu = "" + Choose + "'" + forumid + "'" + ptype + "'view'ok";
                        strOthe = "" + acType + ",addThread.aspx,post,3,other|other|other|other|other|other|other|other|other|other";
                        builder.Append(Out.wapform(strName, strValu, strOthe));
                        builder.Append(Out.Tab("</div>", ""));
                        if (Choose != "")
                        {
                            builder.Append(Out.Tab("<div>", "<br />"));
                            builder.Append("=选中的家野=<br />");

                            string[] cTemp = Choose.Split(',');

                            for (int i = 0; i < cTemp.Length; i++)
                            {
                                builder.Append("" + cTemp[i] + "");
                                strName = "Choose,forumid,ptype,act,info";
                                if (i == 0)
                                    strValu = "" + Choose.Replace(cTemp[i] + ",", "").Replace(cTemp[i], "") + "'" + forumid + "'" + ptype + "'view'ok";
                                else
                                    strValu = "" + Choose.Replace("," + cTemp[i], "").Replace(cTemp[i], "") + "'" + forumid + "'" + ptype + "'view'ok";

                                strOthe = "[删],addThread.aspx,post,3,other";
                                builder.Append(Out.wapform(strName, strValu, strOthe));
                            }
                            builder.Append(Out.Tab("</div>", ""));
                        }
                    }
                    strText = "主题(" + ub.GetSub("BbsThreadMin", xmlPath) + "-" + ub.GetSub("BbsThreadMax", xmlPath) + "字.格式参照“xxx期xxx”):/,内容(" + ub.GetSub("BbsContentMin", xmlPath) + "-" + ub.GetSub("BbsContentMax", xmlPath) + "字):/,,,,";
                    strName = "Title,Content,Choose,forumid,ptype,act";
                    strType = "text,textarea,hidden,hidden,hidden,hidden";
                    strValu = "''" + Choose + "'" + forumid + "'" + ptype + "'save";
                    strEmpt = "false,false,false,false,false,false";
                    strIdea = "/";
                    strOthe = "&gt;确定发表,addThread.aspx,post,1,red";
                    builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                    builder.Append(Out.Tab("<div>", " "));
                    builder.Append("<a href=\"" + Utils.getUrl("addThread.aspx?forumid=" + forumid + "&amp;ptype=8") + "\">重置</a>");
                    builder.Append(Out.Tab("</div>", ""));
                    builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
                    builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "") + "\">取消发表</a>");
                    builder.Append(Out.Tab("</div>", ""));
                }
            }
            else
            {
                //复制内容
                string Copytitle = string.Empty;
                string Copytemp = string.Empty;
                if (ff >= 0)
                    Copytemp += "[F]" + ff + "[/F]";

                if (dd > 0)
                {
                    Copytitle += new BCW.BLL.Submit().GetTitle(dd, meid);
                    Copytemp += new BCW.BLL.Submit().GetContent(dd, meid);
                }
                if (copy == 1)
                    Copytemp += new BCW.BLL.User().GetCopytemp(meid);

                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "") + "\">" + model.Title + "</a>");
                builder.Append("&gt;发表话题");
                builder.Append(Out.Tab("</div>", ""));

                string sText = string.Empty;
                string sName = string.Empty;
                string sType = string.Empty;
                string sValu = string.Empty;
                string sEmpt = string.Empty;
                if (!string.IsNullOrEmpty(model.Label))
                {
                    sText = "话题类型:,";
                    sName = "LabelId,";
                    sType = "select,";
                    sValu = "0'";
                    sEmpt = "0|综合|" + model.Label + ",";
                }

                if (ptype == 0)
                {
                    strText = "主题(" + ub.GetSub("BbsThreadMin", xmlPath) + "-" + ub.GetSub("BbsThreadMax", xmlPath) + "字):/,内容(" + ub.GetSub("BbsContentMin", xmlPath) + "-" + ub.GetSub("BbsContentMax", xmlPath) + "字):/," + sText + ",,";
                    strName = "Title,Content," + sName + "forumid,ptype,act";
                    strType = "text,big," + sType + "hidden,hidden,hidden";
                    strValu = "" + Copytitle + "'" + Copytemp + "'" + sValu + "" + forumid + "'" + ptype + "'save";
                    strEmpt = "false,false," + sEmpt + "false,false,false";
                }
                else if (ptype == 1)
                {
                    strText = "主题(" + ub.GetSub("BbsThreadMin", xmlPath) + "-" + ub.GetSub("BbsThreadMax", xmlPath) + "字):/,内容(" + ub.GetSub("BbsContentMin", xmlPath) + "-" + ub.GetSub("BbsContentMax", xmlPath) + "字):/,投票选项投票内容(投票选项用“#”隔开):/,投票类型:/,等级要求(级):/,投票性质:/,截止投票时间:/," + sText + ",,";
                    strName = "Title,Content,Vote,VoteType,VoteLeven,VoteTiple,VoteExTime," + sName + "forumid,ptype,act";
                    strType = "text,big,text,select,num,select,date," + sType + "hidden,hidden,hidden";
                    strValu = "" + Copytitle + "'" + Copytemp + "''0'0'0'" + DT.FormatDate(DateTime.Now.AddDays(7), 0) + "'" + sValu + "" + forumid + "'" + ptype + "'save";
                    strEmpt = "false,false,false,0|任何人可见|1|投票后可见,false,0|限制单选|1|允许多选,false," + sEmpt + "false,false,false";

                }
                else if (ptype == 2)
                {
                    strText = "主题(" + ub.GetSub("BbsThreadMin", xmlPath) + "-" + ub.GetSub("BbsThreadMax", xmlPath) + "字):/,内容(" + ub.GetSub("BbsContentMin", xmlPath) + "-" + ub.GetSub("BbsContentMax", xmlPath) + "字):/,描述(所有人可看到):/,收费(0为不收费):/,是否回复后才能浏览:/," + sText + ",,";
                    strName = "Title,Content,Hide,Price,HideType," + sName + "forumid,ptype,act";
                    strType = "text,big,text,num,select," + sType + "hidden,hidden,hidden";
                    strValu = "" + Copytitle + "'" + Copytemp + "''0'0'" + sValu + "" + forumid + "'" + ptype + "'save";
                    strEmpt = "false,false,false,false,0|否|1|是," + sEmpt + "false,false,false";

                }
                else if (ptype == 3)//新功能
                {
                    strText = "主题(" + ub.GetSub("BbsThreadMin", xmlPath) + "-" + ub.GetSub("BbsThreadMax", xmlPath) + "字):/,内容(" + ub.GetSub("BbsContentMin", xmlPath) + "-" + ub.GetSub("BbsContentMax", xmlPath) + "字):/,派币总额:/,最小派币额:/,最大派币额(与最小派币额一样则每人得到金额一样):/,派币附言:（留空则派币时不需要输入附言！）/,派币楼层尾数.多个用#隔开(0为全派但不重复):/,★派币币种:," + sText + ",,";
                    strName = "Title,Content,Prices,Price,Price2,PricesLimit,PayCi,BzType," + sName + "forumid,ptype,act";
                    strType = "text,big,num,num,num,text,text,select," + sType + "hidden,hidden,hidden";
                    strValu = "" + Copytitle + "'" + Copytemp + "'0'0'0''0'0'" + sValu + "" + forumid + "'" + ptype + "'save";
                    strEmpt = "false,false,false,false,false,false,false,0|" + ub.Get("SiteBz") + "|1|" + ub.Get("SiteBz2") + "," + sEmpt + "false,false,false";
                    if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th"))
                    {
                        strIdea = "/提示：派币有效期为15天，系统收取总额" + ub.GetSub("BbsCentThreadTar", xmlPath) + "%的手续费/";
                    }
                    else
                    {
                        strIdea = "/提示：派币有效期为一周，系统收取总额" + ub.GetSub("BbsCentThreadTar", xmlPath) + "%的手续费/";
                    }
                }
                else if (ptype == 4)
                {
                    strText = "主题(" + ub.GetSub("BbsThreadMin", xmlPath) + "-" + ub.GetSub("BbsThreadMax", xmlPath) + "字):/,内容(" + ub.GetSub("BbsContentMin", xmlPath) + "-" + ub.GetSub("BbsContentMax", xmlPath) + "字):/,性质:,等级(0级为不限制):/," + sText + ",,";
                    strName = "Title,Content,IsSeen,Grade," + sName + "forumid,ptype,act";
                    strType = "text,big,select,num," + sType + "hidden,hidden,hidden";
                    strValu = "" + Copytitle + "'" + Copytemp + "'0'0'" + sValu + "" + forumid + "'" + ptype + "'save";
                    strEmpt = "false,false,0|不限制|1|登录可见|2|手机可见,false," + sEmpt + "false,false,false";

                }
                else if (ptype == 6)
                {
                    strText = "主题(" + ub.GetSub("BbsThreadMin", xmlPath) + "-" + ub.GetSub("BbsThreadMax", xmlPath) + "字):/,内容(" + ub.GetSub("BbsContentMin", xmlPath) + "-" + ub.GetSub("BbsContentMax", xmlPath) + "字):/,竞猜" + ub.Get("SiteBz") + "(可续):/,★竞猜币种:," + sText + ",,";
                    strName = "Title,Content,Prices,BzType," + sName + "forumid,ptype,act";
                    strType = "text,big,num,hidden," + sType + "hidden,hidden,hidden";
                    strValu = "" + Copytitle + "'" + Copytemp + "'0'0'" + sValu + "" + forumid + "'" + ptype + "'save";
                    strEmpt = "false,false,false,0|" + ub.Get("SiteBz") + "|1|" + ub.Get("SiteBz2") + "," + sEmpt + "false,false,false";
                    strIdea = "/提示:发表竞猜帖子收取100" + ub.Get("SiteBz") + "手续费./";

                }
                else if (ptype == 7)
                {
                    strText = "主题(" + ub.GetSub("BbsThreadMin", xmlPath) + "-" + ub.GetSub("BbsThreadMax", xmlPath) + "字):/,内容(" + ub.GetSub("BbsContentMin", xmlPath) + "-" + ub.GetSub("BbsContentMax", xmlPath) + "字):/,悬赏金额:/,★悬赏币种:," + sText + ",,";
                    strName = "Title,Content,Prices,BzType," + sName + "forumid,ptype,act";
                    strType = "text,big,num,select," + sType + "hidden,hidden,hidden";
                    strValu = "" + Copytitle + "'" + Copytemp + "'0'0'" + sValu + "" + forumid + "'" + ptype + "'save";
                    strEmpt = "false,false,false,0|" + ub.Get("SiteBz") + "|1|" + ub.Get("SiteBz2") + "," + sEmpt + "false,false,false";
                    strIdea = "/提示:发表悬赏帖子收取100" + ub.Get("SiteBz") + "手续费./";

                }
                strText += ",表情:";
                strName += ",Face";
                strType += ",select";
                strValu += "'0";
                strEmpt += ",0|表情|1|微笑|2|哭泣|3|调皮|4|脸红|5|害羞|6|生气|7|偷笑|8|流汗|9|我倒|10|得意|11|眦牙|12|疑问|13|睡觉|14|好色|15|口水|16|敲头|17|可爱|18|猪头|19|胜利|20|玫瑰|21|吃药|22|白酒|23|可乐|24|囧囧|25|亲亲|26|抱抱";

                if (string.IsNullOrEmpty(strIdea))
                    strIdea = "/";

                strOthe = "&gt;确定发表|保存草稿,addThread.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("插入:<a href=\"" + Utils.getUrl("function.aspx?act=admsub&amp;backurl=" + Utils.PostPage(1) + "") + "\">常用短语</a>|");
                builder.Append("<a href=\"" + Utils.getUrl("function.aspx?act=face&amp;backurl=" + Utils.PostPage(1) + "") + "\">表情</a>|");
                builder.Append("<a href=\"" + Utils.getUrl("function.aspx?act=admtemp&amp;backurl=" + Utils.PostPage(1) + "") + "\">草稿</a>");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("类型:");

                if (new BCW.User.ForumInc().IsForum68(forumid) == true)
                {
                    if (ptype == 6)
                    {
                        //builder.Append("竞猜|");
                    }
                    else
                        builder.Append("<a href=\"" + Utils.getUrl("addThread.aspx?forumid=" + forumid + "&amp;ptype=6") + "\">竞技</a>|");
                }
                if (new BCW.User.ForumInc().IsForum69(forumid) == true)
                {
                    if (ptype == 7)
                    {
                        //builder.Append("悬赏|");
                    }
                    else
                        builder.Append("<a href=\"" + Utils.getUrl("addThread.aspx?forumid=" + forumid + "&amp;ptype=7") + "\">悬赏</a>|");
                }
                if (ptype != 0)
                    builder.Append("<a href=\"" + Utils.getUrl("addThread.aspx?forumid=" + forumid + "&amp;ptype=0") + "\">普通</a>|");

                if (ptype == 1)
                    builder.Append("投票|");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("addThread.aspx?forumid=" + forumid + "&amp;ptype=1") + "\">投票</a>|");
                if (ptype == 2)
                    builder.Append("隐藏|");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("addThread.aspx?forumid=" + forumid + "&amp;ptype=2") + "\">隐藏</a>|");
                if (ptype == 3)
                    builder.Append("派币|");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("addThread.aspx?forumid=" + forumid + "&amp;ptype=3") + "\">派币</a>|");
                if (ptype == 4)
                    builder.Append("限制");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("addThread.aspx?forumid=" + forumid + "&amp;ptype=4") + "\">限制</a>");

                builder.Append("<br /><a href=\"" + Utils.getUrl("addThread20.aspx?forumid=" + forumid + "") + "\">发表文件帖</a> ");
                builder.Append("<br /><a href=\"" + Utils.getUrl("addfilegc.aspx?forumid=" + forumid + "") + "\">发表文件帖(低端机)</a> ");
                builder.Append("<br /><a href=\"" + Utils.getUrl("addThread.aspx?act=help&amp;forumid=" + forumid + "") + "\">发帖UBB帮助</a>");
                builder.Append("<br /><a href=\"" + Utils.getUrl("addThread.aspx?forumid=" + forumid + "&amp;ptype=" + ptype + "&amp;ff=" + ff + "&amp;dd=" + dd + "&amp;copy=1&amp;backurl=" + Utils.getPage(0) + "") + "\">[粘贴]</a>");
                builder.Append(Out.Tab("</div>", ""));
                builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
                builder.Append("<a href=\"" + Utils.getUrl("forum.aspx?forumid=" + forumid + "") + "\">取消发表</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
    }


    private void SavePage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int ptype = int.Parse(Utils.GetRequest("ptype", "post", 2, @"^[0-4]$|^6$|^7$|^8$", "帖子类型错误"));
        int forumid = int.Parse(Utils.GetRequest("forumid", "post", 2, @"^[0-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Forum().Exists2(forumid))
        {
            Utils.Error("不存在的论坛或此论坛已暂停使用", "");
        }

        BCW.User.Users.ShowVerifyRole("a", meid);//非验证会员提示
        new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Text, meid);//会员本身权限
        new BCW.User.FLimits().CheckUserFLimit(BCW.User.FLimits.enumRole.Role_Text, meid, forumid);//版块内权限
        string mename = new BCW.BLL.User().GetUsName(meid);

        BCW.Model.Forum model = new BCW.BLL.Forum().GetForum(forumid);
        //圈子限制性
        BCW.Model.Group modelgr = null;
        if (model.GroupId > 0)
        {
            modelgr = new BCW.BLL.Group().GetGroupMe(model.GroupId);
            if (modelgr == null)
            {
                Utils.Error("不存在的" + ub.GetSub("GroupName", "/Controls/group.xml") + "", "");
            }
            else if (DT.FormatDate(modelgr.ExTime, 0) != "1990-01-01 00:00:00" && modelgr.ExTime < DateTime.Now)
            {
                Utils.Error("" + ub.GetSub("GroupName", "/Controls/group.xml") + "已过期", "");
            }
            if (modelgr.ForumStatus == 2)
            {
                Utils.Error("" + ub.GetSub("GroupName", "/Controls/group.xml") + "论坛已关闭", "");
            }
            if (modelgr.ForumStatus == 1)
            {
                if (meid == 0)
                    Utils.Login();

                string GroupId = new BCW.BLL.User().GetGroupId(meid);
                if (GroupId.IndexOf("#" + model.GroupId + "#") == -1 && IsCTID(meid) == false)
                {
                    Utils.Error("非成员不能访问" + ub.GetSub("GroupName", "/Controls/group.xml") + "论坛！<br /><a href=\"" + Utils.getUrl("/bbs/group.aspx?act=addin&amp;id=" + model.GroupId + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">加入本" + ub.GetSub("GroupName", "/Controls/group.xml") + "</a>", "");
                }
            }
        }
        //论坛限制性
        BCW.User.Users.ShowForumLimit(meid, model.Gradelt, model.Visitlt, model.VisitId, model.IsPc);
        BCW.User.Users.ShowAddThread(meid, model.Postlt);
        string Title = Utils.GetRequest("Title", "post", 2, @"^[\s\S]{" + ub.GetSub("BbsThreadMin", xmlPath) + "," + ub.GetSub("BbsThreadMax", xmlPath) + "}$", "标题限" + ub.GetSub("BbsThreadMin", xmlPath) + "-" + ub.GetSub("BbsThreadMax", xmlPath) + "字");
        Title = Title.Replace(char.ConvertFromUtf32(10), "").Replace(char.ConvertFromUtf32(13), "");
        string Content = Utils.GetRequest("Content", "post", 2, @"^[\s\S]{" + ub.GetSub("BbsContentMin", xmlPath) + "," + ub.GetSub("BbsContentMax", xmlPath) + "}$", "请输入" + ub.GetSub("BbsContentMin", xmlPath) + "-" + ub.GetSub("BbsContentMax", xmlPath) + "字的内容");
        int Face = int.Parse(Utils.GetRequest("Face", "post", 1, @"^[0-9]\d*$", "0"));
       // Limit
       // int Limit = int.Parse(Utils.GetRequest("Limit", "post", 2, @"^[0-1]\d*$", "选择是否附言出错"));
        string PricesLimit = Utils.GetRequest("PricesLimit", "all", 1, @"^[^\^]{1,1000}$", "");//派币附言
        if (PricesLimit.Length > 30)
            Utils.Error("请输入附言为1到30字！", "");
        
        if (Face > 0 & Face < 27) 
            Content = "[F]" + Face + "[/F]" + Content;

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        if (Utils.ToSChinese(ac) == "保存草稿")
        {
            //是否刷屏
            string appName = "LIGHT_THREAD";
            int Expir = Convert.ToInt32(ub.GetSub("BbsThreadExpir", xmlPath));
            BCW.User.Users.IsFresh(appName, Expir);

            BCW.Model.Submit admodel = new BCW.Model.Submit();
            admodel.Types = 1;
            admodel.Title = Title;
            admodel.Content = Content;
            admodel.UsID = meid;
            admodel.AddTime = DateTime.Now;
            new BCW.BLL.Submit().Add(admodel);
            Utils.Success("保存草稿", "保存草稿成功，正在返回..", Utils.getUrl("addThread.aspx?forumid=" + forumid + ""), "2");
        }
        else
        {
            if (ptype != 3 && ptype != 6 && ptype != 8)
            {
                //是否刷屏
                string appName = "LIGHT_THREAD";
                int Expir = Convert.ToInt32(ub.GetSub("BbsThreadExpir", xmlPath));
                BCW.User.Users.IsFresh(appName, Expir);
            }
            int ThreadNum = Utils.ParseInt(ub.GetSub("BbsThreadNum", xmlPath));
            if (ThreadNum > 0)
            {
                int ToDayCount = new BCW.BLL.Forumstat().GetCount(meid, 1);//今天发布帖子数
                if (ToDayCount >= ThreadNum)
                {
                    Utils.Error("系统限每天每ID限发帖子" + ThreadNum + "帖", "");
                }
            }
            string Hide = string.Empty;
            int Price = 0;
            int Price2 = 0;
            long Prices = 0;
            int BzType = 0;

            int HideType = 0;
            int IsSeen = 0;
            string PayCi = string.Empty;
            string Vote = string.Empty;
            int VoteType = 0;
            int VoteLeven = 0;
            int VoteTiple = 0;

            DateTime VoteExTime = DateTime.Now;
            string Choose = "";
            int ChooseTypes = 0;

            if (ptype == 1)
            {
                Vote = Utils.GetRequest("Vote", "post", 2, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){1,500}$", "投票选项必须用#隔开、选项两个以上");
                VoteType = int.Parse(Utils.GetRequest("VoteType", "post", 2, @"^[0-1]$", "投票类型选择错误"));
                VoteLeven = int.Parse(Utils.GetRequest("VoteLeven", "post", 2, @"^[0-9]\d*$", "等级数填写错误"));
                VoteTiple = int.Parse(Utils.GetRequest("VoteTiple", "post", 2, @"^[0-1]$", "投票性质选择错误"));
                VoteExTime = Utils.ParseTime(Utils.GetRequest("VoteExTime", "post", 2, DT.RegexTime, "截止投票时间格式填写出错,正确格式如" + DateTime.Now.ToString("yyy-MM-dd HH:mm:ss") + ""));
            }
            else if (ptype == 2)
            {
                Hide = Utils.GetRequest("Hide", "post", 2, @"^[^\^]{1,300}$", "请输入1-300字的描述");
                Price = int.Parse(Utils.GetRequest("Price", "post", 2, @"^[0-9]\d*$", "收费填写错误"));
                HideType = int.Parse(Utils.GetRequest("HideType", "post", 2, @"^[0-1]$", "浏览性质选择错误"));
                if (Price == 0 && HideType == 0)
                {
                    Utils.Error("收费与隐藏至少要匹配一项", "");
                }
            }
            else if (ptype == 3)
            {
                Prices = Int64.Parse(Utils.GetRequest("Prices", "post", 4, @"^[1-9]\d*$", "派币总额填写错误"));
                Price = int.Parse(Utils.GetRequest("Price", "post", 2, @"^[1-9]\d*$", "最小派币额填写错误"));
                Price2 = int.Parse(Utils.GetRequest("Price2", "post", 2, @"^[1-9]\d*$", "最大派币额填写错误"));
                BzType = int.Parse(Utils.GetRequest("BzType", "post", 2, @"^[0-1]$", "派币币种选择错误"));
            

                if (Convert.ToInt64(Price) > Prices || Convert.ToInt64(Price2) > Prices)
                {
                    Utils.Error("最小派币额与最大派币额不能大于派币总额", "");
                }
                if (Price2 > 0 && Price > Price2)
                {
                    Utils.Error("最大派币额非0时，必须大于最小派币额", "");
                }
                PayCi = Utils.GetRequest("PayCi", "post", 1, @"", "");
                if (PayCi != "0")
                {
                    if (!Utils.IsRegex(PayCi, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$"))
                    {
                        Utils.Error("楼层尾数填写错误，正确格式如1#3#5", "");
                    }
                    string[] sPayCi = PayCi.Split("#".ToCharArray());
                    for (int i = 0; i < sPayCi.Length; i++)
                    {
                        if (sPayCi[i].Length > 1)
                        {
                            Utils.Error("楼层尾数填写错误，正确格式如1#3#5", "");
                        }
                        if (!Utils.IsRegex(sPayCi[i].ToString(), @"^[0-9]$"))
                        {
                            Utils.Error("楼层尾数填写错误，正确格式如1#3#5", "");
                        }
                        //出现的次数
                        int GetNum = Utils.GetStringNum(PayCi, sPayCi[i].ToString());
                        if (GetNum > 1)
                        {
                            Utils.Error("楼层尾数必须唯一，不能重复填写，正确格式如1#3#5" + PayCi[i].ToString() + "", "");
                        }
                    }
                }
                if (BzType == 0)
                {
                    string SysID = ub.GetSub("FinanceSysID", "/Controls/finance.xml");
                    if (("#" + SysID + "#").Contains("#" + meid + "#"))
                    {
                        Utils.Error("你的权限不足", "");
                    }
                    //内部ID过户软禁
                    string SysID2 = ub.GetSub("FinanceSysID2", xmlPath);
                    SysID2 += "#" + ub.GetSub("XiaoHao", "/Controls/xiaohao.xml");

                    if (("#" + SysID2 + "#").Contains("#" + meid + "#"))
                    {
                        Utils.Error("过户权限不足，请联系客服！", "");
                    }

                    long mecent = new BCW.BLL.User().GetGold(meid);
                    if (mecent < Prices)
                    {
                        Utils.Error("你的" + ub.Get("SiteBz") + "不足", "");
                    }

                    //支付安全提示
                    string[] p_pageArr = { "act", "forumid", "ptype", "LabelId", "Title", "Content", "Hide", "Price", "Price2", "Prices", "BzType", "HideType", "PayCi", "IsSeen", "PricesLimit" };
                    BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr);

                    //是否刷屏
                    string appName = "LIGHT_THREAD";
                    int Expir = Convert.ToInt32(ub.GetSub("BbsThreadExpir", xmlPath));
                    BCW.User.Users.IsFresh(appName, Expir);

                    //扣币
                    new BCW.BLL.User().UpdateiGold(meid, mename, -Prices, "发派币帖子");
                }
                else
                {
                    //是否刷屏
                    string appName = "LIGHT_THREAD";
                    int Expir = Convert.ToInt32(ub.GetSub("BbsThreadExpir", xmlPath));
                    BCW.User.Users.IsFresh(appName, Expir);

                    long mecent = new BCW.BLL.User().GetMoney(meid);
                    if (mecent < Prices)
                    {
                        Utils.Error("你的" + ub.Get("SiteBz2") + "不足", "");
                    }
                    //扣币
                    new BCW.BLL.User().UpdateiMoney(meid, mename, -Prices, "发派币帖子");
                }
                //扣取手续费得到最后的派币总额
                long Tar = Convert.ToInt64(ub.GetSub("BbsCentThreadTar", xmlPath));
                long TarCent = Convert.ToInt64(Tar * 0.01 * Prices);
                Prices = Prices - TarCent;


            }
            else if (ptype == 4)
            {
                IsSeen = int.Parse(Utils.GetRequest("IsSeen", "post", 2, @"^[0-2]$", "浏览性质选择错误"));
                Price = int.Parse(Utils.GetRequest("Grade", "post", 2, @"^[0-9]\d*$", "等级填写错误"));
            }
            else if (ptype == 6 || ptype == 7)
            {
                if (ptype == 6 && new BCW.User.ForumInc().IsForum68(forumid) == false)
                {
                    Utils.Error("论坛选择错误", "");
                }
                if (ptype == 7 && new BCW.User.ForumInc().IsForum69(forumid) == false)
                {
                    Utils.Error("论坛选择错误", "");
                }
                Prices = Int64.Parse(Utils.GetRequest("Prices", "post", 4, @"^[1-9]\d*$", "总额填写错误"));
                BzType = int.Parse(Utils.GetRequest("BzType", "post", 2, @"^[0-1]$", "币种选择错误"));
                string SysID = ub.GetSub("FinanceSysID", "/Controls/finance.xml");
                if (("#" + SysID + "#").Contains("#" + meid + "#"))
                {
                    Utils.Error("你的权限不足", "");
                }
                //内部ID过户软禁
                string SysID2 = ub.GetSub("FinanceSysID2", "/Controls/finance.xml");
                SysID2 += "#" + ub.GetSub("XiaoHao", "/Controls/xiaohao.xml");

                if (("#" + SysID2 + "#").Contains("#" + meid + "#"))
                {
                    Utils.Error("过户权限不足，请联系客服！", "");
                }
                if (BzType == 0)
                {
                    long mecent = new BCW.BLL.User().GetGold(meid);
                    if (mecent < (Prices + 100))
                    {
                        Utils.Error("你的" + ub.Get("SiteBz") + "不足" + (Prices + 100) + "(含手续费)", "");
                    }

                    //支付安全提示
                    string[] p_pageArr = { "act", "forumid", "ptype", "LabelId", "Title", "Content", "Hide", "Price", "Price2", "Prices", "BzType", "HideType", "PayCi", "IsSeen", "PricesLimit" };
                    BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr);

                    //扣币
                    if (ptype == 6)
                        new BCW.BLL.User().UpdateiGold(meid, mename, -(Prices + 100), "发竞猜帖子");
                    else
                        new BCW.BLL.User().UpdateiGold(meid, mename, -(Prices + 100), "发悬赏帖子");
                }
                else
                {
                    long mecent = new BCW.BLL.User().GetMoney(meid);
                    if (mecent < Prices)
                    {
                        Utils.Error("你的" + ub.Get("SiteBz2") + "不足", "");
                    }
                    //扣币
                    if (ptype == 6)
                        new BCW.BLL.User().UpdateiMoney(meid, mename, -Prices, "发竞猜帖子");
                    else
                        new BCW.BLL.User().UpdateiMoney(meid, mename, -Prices, "发悬赏帖子");
                }
            }
            else if (ptype == 8)
            {
                if (new BCW.User.ForumInc().IsForumGSIDS(forumid))
                {

                    DateTime GsStopTime = Utils.ParseTime(ub.GetSub("BbsGsStopTime", xmlPath));
                    int GsqiNum = Utils.ParseInt(ub.GetSub("BbsGsqiNum", xmlPath));
                    if (GsqiNum == 0)
                    {
                        Utils.Error("本期尚未设置", "");
                    }
                    if (GsStopTime < DateTime.Now)
                    {
                        Utils.Error("本期第" + GsqiNum + "期已截止发表，截止时间" + GsStopTime + "", "");
                    }
                    Choose = Utils.GetRequest("Choose", "post", 2, @"^[^\,]{1,50}(?:\,[^\,]{1,50}){0,500}$", "参赛内容选择出错");
                    int RaceBID = new BCW.BLL.Text().GetRaceBID(forumid, meid);
                    if (RaceBID > 0)
                    {
                        Utils.Success("重复参赛帖", "您在本论坛已发表一张参赛帖，正在进入参赛帖...", Utils.getUrl("topic.aspx?forumid=" + forumid + "&amp;bid=" + RaceBID + ""), "2");
                    }

                    int cNum = Utils.GetStringNum(Choose, ",");
                    if (new BCW.User.ForumInc().IsForum113(forumid) == true)
                    {
                        if (cNum != 5)
                        {
                            Utils.Error("请选择6个生肖", "");
                        }
                    }
                    else if (new BCW.User.ForumInc().IsForum114(forumid) == true)
                    {
                        if (Choose.Contains("单") || Choose.Contains("双"))
                        {
                            if (cNum != 3)
                            {
                                Utils.Error("选择半波参赛时，请选择4个半波", "");
                            }
                        }
                        else
                        {
                            if (cNum != 1)
                            {
                                Utils.Error("选择波色参赛时，请选择2个波色", "");
                            }
                        }
                    }
                    else if (new BCW.User.ForumInc().IsForum116(forumid) == true)
                    {
                        if (cNum != 0)
                        {
                            Utils.Error("请选择1个生肖", "");
                        }

                    }
                    if (new BCW.User.ForumInc().IsForum117(forumid) == true)
                    {
                        if (cNum != 4)
                        {
                            Utils.Error("请选择5个数字", "");
                        }
                    }
                    if (new BCW.User.ForumInc().IsForum119(forumid) == true)
                    {
                        if (cNum != 4)
                        {
                            Utils.Error("请选择5个尾数", "");
                        }
                    }
                    if (new BCW.User.ForumInc().IsForum115(forumid) == true || new BCW.User.ForumInc().IsForum121(forumid) == true || new BCW.User.ForumInc().IsForum122(forumid) == true)
                    {
                        if (cNum != 0)
                        {
                            Utils.Error("参数选择错误", "");
                        }
                    }
                }
            
            }


            int LabelId = int.Parse(Utils.GetRequest("LabelId", "post", 1, @"^[0-9]\d*$", "0"));

            BCW.Model.Text addmodel = new BCW.Model.Text();
            addmodel.ForumId = forumid;
            addmodel.Types = ptype;
            addmodel.LabelId = LabelId;
            addmodel.Title = Title;
            addmodel.Content = Content;
            addmodel.HideContent = Hide;
            addmodel.UsID = meid;
            addmodel.UsName = mename;
            addmodel.Price = Price;
            addmodel.Price2 = Price2;
            addmodel.Prices = Prices;
            addmodel.HideType = HideType;
            addmodel.BzType = BzType;
            addmodel.PayCi = PayCi;
            addmodel.IsSeen = IsSeen;
            addmodel.IsDel = 0;
            addmodel.AddTime = DateTime.Now;
            addmodel.ReTime = DateTime.Now;
            addmodel.PricesLimit = PricesLimit;

            if (ptype == 8 && Choose != "" && (new BCW.User.ForumInc().IsForumGSIDS(forumid) == true))
            {
                addmodel.Gaddnum = 1;
                addmodel.Gqinum = Utils.ParseInt(ub.GetSub("BbsGsqiNum", xmlPath));
            }
            else
            {
                addmodel.Gaddnum = 0;
                addmodel.Gqinum = 0;
            }
            int k = 0;
            if (ptype == 3)
            {
                k = new BCW.BLL.Text().AddPricesLimit(addmodel);
            }
            else
            {
                k = new BCW.BLL.Text().Add(addmodel);
            }

            //投票帖子
            if (ptype == 1)
            {
                string[] sVote = Vote.Split("#".ToCharArray());
                string sAddVote = string.Empty;
                for (int i = 0; i < sVote.Length; i++)
                {
                    sAddVote += "#0";
                }
                sAddVote = Utils.Mid(sAddVote, 1, sAddVote.Length);

                BCW.Model.Votes votemodel = new BCW.Model.Votes();
                votemodel.Types = k;//帖子ID
                votemodel.UsID = meid;
                votemodel.Title = "";
                votemodel.Content = "";
                votemodel.Vote = Vote;
                votemodel.AddVote = sAddVote;
                votemodel.VoteType = VoteType;
                votemodel.VoteLeven = VoteLeven;
                votemodel.VoteTiple = VoteTiple;
                votemodel.Readcount = 0;
                votemodel.VoteExTime = VoteExTime;
                votemodel.AddTime = DateTime.Now;
                new BCW.BLL.Votes().Add(votemodel);
            }
            else if (ptype == 6)//竞猜帖子
            {
                BCW.Model.Textdc dc = new BCW.Model.Textdc();
                dc.BID = k;
                dc.UsID = meid;
                dc.IsZtid = 0;
                dc.OutCent = Prices;
                dc.AddTime = DateTime.Now;
                dc.BzType = BzType;
                string bzText = string.Empty;
                if (BzType == 0)
                    bzText = ub.Get("SiteBz");
                else
                    bzText = ub.Get("SiteBz2");

                dc.LogText = "" + mename + "(" + meid + ")使用" + Prices + "" + bzText + "开庄(" + DT.FormatDate(DateTime.Now, 1) + ")";
                new BCW.BLL.Textdc().Add(dc);
            }
            else if (ptype == 8)//高手论坛帖子
            {
                if (new BCW.User.ForumInc().IsForumGSIDS(forumid) == true)
                {
                    int GsqiNum = Utils.ParseInt(ub.GetSub("BbsGsqiNum", xmlPath));
        
                    BCW.Model.Forumvote f = new BCW.Model.Forumvote();
                    f.Types = ChooseTypes;
                    f.ForumID = forumid;
                    f.BID = k;
                    f.Notes = Choose;
                    f.state = 0;
                    f.qiNum = GsqiNum;
                    f.IsWin = 0;
                    f.AddTime = DateTime.Now;
                    f.UsID = meid;
                    f.UsName = mename;
                    new BCW.BLL.Forumvote().Add(f);
                }

            }
            //论坛统计
            BCW.User.Users.UpdateForumStat(1, meid, mename, forumid);
            //动态记录
            if (model.GroupId > 0)
            {
                new BCW.BLL.Action().Add(-2, 0, meid, mename, "在圈坛-" + model.Title + "发表了[URL=/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + k + "]" + Title + "[/URL]的帖子");
            }
            else
            {
                new BCW.BLL.Action().Add(-1, 0, meid, mename, "在" + model.Title + "发表了[URL=/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + k + "]" + Title + "[/URL]的帖子");
            }
            //积分操作/论坛统计/圈子论坛不进行任何奖励
            int GroupId = new BCW.BLL.Forum().GetGroupId(forumid);
            int IsAcc = -1;
            if (GroupId == 0)
            {
                IsAcc = new BCW.User.Cent().UpdateCent2(BCW.User.Cent.enumRole.Cent_Text, meid, true);
            }
            else
            {
                if (!Utils.GetDomain().Contains("th"))
                    IsAcc = new BCW.User.Cent().UpdateCent2(BCW.User.Cent.enumRole.Cent_Text, meid, false);
            }
            #region  这里开始修改提醒ID 发内线
            string remind = ub.GetSub("remindid" + forumid, "/Controls/bbs.xml");//获取XML的值
            if (remind != "")  //如果有提醒ID
            {
                string[] IDS = remind.Split('#');
                for (int i = 0; i < IDS.Length; i++)
                {
                    if (model.GroupId > 0)
                    {
                        new BCW.BLL.Guest().Add(0, int.Parse(IDS[i]), new BCW.BLL.User().GetUsName(int.Parse(IDS[i])), "请注意!用户[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "(" + meid + ")[/url]在圈坛-" + model.Title + "发表了[URL=/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + k + "]" + Title + "[/URL]的帖子");
                    }
                    else
                    {
                        new BCW.BLL.Guest().Add(0, int.Parse(IDS[i]), new BCW.BLL.User().GetUsName(int.Parse(IDS[i])), "请注意!用户[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "(" + meid + ")[/url]在" + model.Title + "发表了[URL=/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + k + "]" + Title + "[/URL]的帖子");
                    }
                }
            }
            #endregion
            if (IsAcc >= 0)
                Utils.Success("发表成功", "发表成功！恭喜您获得" + BCW.User.Users.GetWinCent(0, meid) + "", Utils.getUrl("forum.aspx?forumid=" + forumid + ""), "2");
            else
                Utils.Success("发表成功", "发表成功！", Utils.getUrl("forum.aspx?forumid=" + forumid + ""), "2");
        }

    }

    private void HelpPage()
    {
        int forumid = int.Parse(Utils.GetRequest("forumid", "get", 1, @"^[0-9]\d*$", "0"));
        Master.Title = "发表UBB帮助";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("发表UBB帮助:");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("换行：[br]<br />");
        builder.Append("空格：[n]<br />");
        builder.Append("手工分页：##<br />");
        builder.Append("页下居左：[left]<br />");
        builder.Append("页下居中：[center]<br />");
        builder.Append("页下居右：[right]<br />");
        builder.Append("文字加粗：[B]加粗[/B]<br />");
        builder.Append("文字斜体：[I]斜体[/I]<br />");
        builder.Append("文字下划线：[U]下划线[/U]<br />");
        builder.Append("大字体：[BIG]大字体[/BIG]<br />");
        builder.Append("小字体：[SMALL]小字体[/SMALL]<br />");
        builder.Append("强调字体：[STRONG]小字体[/STRONG]<br />");
        builder.Append("打电话：[C]电话号码[/C]<br />");
        builder.Append("打电话2：[C=电话号码]说明[/C]<br />");
        builder.Append("帖图：[IMG]图片地址[/IMG]<br />");
        builder.Append("超链：[URL]网页地址[/URL]<br />");
        builder.Append("超链2：[URL=网页地址]说明[/URL]<br />");
        builder.Append("<b>=彩色字体(帖子标题也支持哦)=</b><br />");
        builder.Append("[红]红色字体[/红]<br />");
        builder.Append("[橙]橙色字体[/橙]<br />");
        //builder.Append("[黄]黄色字体[/黄]<br />");
        builder.Append("[绿]绿色字体[/绿]<br />");
        builder.Append("[青]青色字体[/青]<br />");
        builder.Append("[蓝]蓝色字体[/蓝]<br />");
        builder.Append("[紫]紫色字体[/紫]");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("addThread.aspx?forumid=" + forumid + "") + "\">看明白了,继续发帖</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    /// <summary>
    /// 穿透圈子限制ID
    /// </summary>
    private bool IsCTID(int meid)
    {
        bool Isvi = false;
        //能够穿透的ID
        string CTID = "#" + ub.GetSub("GroupCTID", "/Controls/group.xml") + "#";
        if (CTID.IndexOf("#" + meid + "#") != -1)
        {
            Isvi = true;
        }

        return Isvi;
    }
}