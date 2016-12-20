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

/// <summary>
/// 已结贴不能添加本期 20160422
/// 黄国军
/// </summary>
public partial class bbs_Gsedit : System.Web.UI.Page
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

        int forumid = int.Parse(Utils.GetRequest("forumid", "all", 2, @"^[0-9]\d*$", "论坛ID错误"));
        int bid = int.Parse(Utils.GetRequest("bid", "all", 2, @"^[0-9]\d*$", "帖子ID错误"));
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[0-9]\d*$", "0"));
        if (!new BCW.BLL.Forum().Exists2(forumid))
        {
            Utils.Success("访问论坛", "该论坛不存在或已暂停使用", Utils.getUrl("forum.aspx"), "1");
        }
        if (!new BCW.BLL.Text().Exists2(bid, forumid))
        {
            Utils.Error("帖子不存在或已被删除", "");
        }
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "save":
                SavePage(forumid, bid, id);
                break;
            default:
                ReloadPage(forumid, bid, id);
                break;
        }
    }

    private void ReloadPage(int forumid, int bid, int id)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string GsDemoID = ub.GetSub("BbsGsDemoID", xmlPath);
        if (GsDemoID != "")
        {
            if (!("#" + GsDemoID + "#").Contains("#" + meid + "#"))
            {
                Utils.Error("此功能内测中，您的ID不在内测ID里", "");
            }
        }
        BCW.Model.Text modelT = new BCW.BLL.Text().GetText(bid);

        if (modelT.IsOver == 1)
        {
            Utils.Error("本帖已结,请另开新帖增加期数", "");
        }

        if (id > 0)
        {
            BCW.Model.Forumvote model = new BCW.BLL.Forumvote().GetForumvote(id);
            if (model == null)
            {
                Utils.Error("不存在的记录", "");
            }
            if (model.UsID != meid)
            {
                Utils.Error("不存在的记录", "");
            }
        }
        else
        {
            if (modelT == null)
            {
                Utils.Error("不存在的记录", "");
            }
            if (modelT.UsID != meid)
            {
                Utils.Error("不存在的记录", "");
            }

        }
        if (new BCW.User.ForumInc().IsForumGSIDS(forumid) == true)
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

            builder.Append(Out.Tab("<div class=\"title\">", ""));
            if (id > 0)
            {
                Master.Title = "修改参赛内容";
                builder.Append("修改参赛内容");
            }
            else
            {
                Master.Title = "添加参赛内容";
                builder.Append("添加参赛内容");
            }
            builder.Append(Out.Tab("</div>", "<br />"));

            string ac = Utils.GetRequest("ac", "post", 1, "", "");
            string Choose = Utils.GetRequest("Choose", "post", 1, "", "");
            string Choose2 = Utils.GetRequest("Choose2", "post", 1, "", "");

            if (new BCW.User.ForumInc().IsForum113(forumid) == true || new BCW.User.ForumInc().IsForum116(forumid) == true)
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
                strName = "Choose,forumid,bid,id,act,info";
                strValu = "" + Choose + "'" + forumid + "'" + bid + "'" + id + "'view'ok";
                strOthe = "" + sx + ",Gsedit.aspx,post,3,other|other|other|other|other|other|other|other|other|other|other|other";
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
                        strName = "Choose,forumid,bid,id,act,info";
                        if (i == 0)
                            strValu = "" + Choose.Replace(cTemp[i] + ",", "").Replace(cTemp[i], "") + "'" + forumid + "'" + bid + "'" + id + "'view'ok";
                        else
                            strValu = "" + Choose.Replace("," + cTemp[i], "").Replace(cTemp[i], "") + "'" + forumid + "'" + bid + "'" + id + "'view'ok";

                        strOthe = "[删],Gsedit.aspx,post,3,other";
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
                strName = "Choose,forumid,bid,id,act,info";
                strValu = "" + Choose + "'" + forumid + "'" + bid + "'" + id + "'view'ok";
                strOthe = "" + bs + ",Gsedit.aspx,post,3,other|other|other";
                builder.Append(Out.wapform(strName, strValu, strOthe));
                builder.Append(Out.Tab("</div>", ""));

                builder.Append(Out.Tab("<div>", Out.Hr()));
                builder.Append("=选择要推荐的半波=");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                strName = "Choose2,forumid,bid,id,act,info";
                strValu = "" + Choose2 + "'" + forumid + "'" + bid + "'" + id + "'view'ok";
                strOthe = "" + bs2 + ",Gsedit.aspx,post,3,other|other|other|other|other|other";
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
                        strName = "Choose,forumid,bid,id,act,info";
                        if (i == 0)
                            strValu = "" + Choose.Replace(cTemp[i] + ",", "").Replace(cTemp[i], "") + "'" + forumid + "'" + bid + "'" + id + "'view'ok";
                        else
                            strValu = "" + Choose.Replace("," + cTemp[i], "").Replace(cTemp[i], "") + "'" + forumid + "'" + bid + "'" + id + "'view'ok";

                        strOthe = "[删],Gsedit.aspx,post,3,other";
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
                        strName = "Choose2,forumid,bid,id,act,info";
                        if (i == 0)
                            strValu = "" + Choose2.Replace(cTemp[i] + ",", "").Replace(cTemp[i], "") + "'" + forumid + "'" + bid + "'" + id + "'view'ok";
                        else
                            strValu = "" + Choose2.Replace("," + cTemp[i], "").Replace(cTemp[i], "") + "'" + forumid + "'" + bid + "'" + id + "'view'ok";

                        strOthe = "[删],Gsedit.aspx,post,3,other";
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
                strName = "Choose,forumid,bid,id,act,info";
                strValu = "" + Choose + "'" + forumid + "'" + bid + "'" + id + "'view'ok";
                strOthe = "" + sNum + ",Gsedit.aspx,post,3,other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other|other";
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
                        strName = "Choose,forumid,bid,id,act,info";
                        if (i == 0)
                            strValu = "" + Choose.Replace(cTemp[i] + ",", "").Replace(cTemp[i], "") + "'" + forumid + "'" + bid + "'" + id + "'view'ok";
                        else
                            strValu = "" + Choose.Replace("," + cTemp[i], "").Replace(cTemp[i], "") + "'" + forumid + "'" + bid + "'" + id + "'view'ok";

                        strOthe = "[删],Gsedit.aspx,post,3,other";
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
                strName = "Choose,forumid,bid,id,act,info";
                strValu = "" + Choose + "'" + forumid + "'" + bid + "'" + id + "'view'ok";
                strOthe = "" + ws + ",Gsedit.aspx,post,3,other|other|other|other|other|other|other|other|other|other";
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
                        strName = "Choose,forumid,bid,id,act,info";
                        if (i == 0)
                            strValu = "" + Choose.Replace(cTemp[i] + ",", "").Replace(cTemp[i], "") + "'" + forumid + "'" + bid + "'" + id + "'view'ok";
                        else
                            strValu = "" + Choose.Replace("," + cTemp[i], "").Replace(cTemp[i], "") + "'" + forumid + "'" + bid + "'" + id + "'view'ok";

                        strOthe = "[删],Gsedit.aspx,post,3,other";
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
                strName = "Choose,forumid,bid,id,act,info";
                strValu = "" + Choose + "'" + forumid + "'" + bid + "'" + id + "'view'ok";
                strOthe = "" + acType + ",Gsedit.aspx,post,3,other|other|other|other|other|other|other|other|other|other";
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
                        strName = "Choose,forumid,bid,id,act,info";
                        if (i == 0)
                            strValu = "" + Choose.Replace(cTemp[i] + ",", "").Replace(cTemp[i], "") + "'" + forumid + "'" + bid + "'" + id + "'view'ok";
                        else
                            strValu = "" + Choose.Replace("," + cTemp[i], "").Replace(cTemp[i], "") + "'" + forumid + "'" + bid + "'" + id + "'view'ok";

                        strOthe = "[删],Gsedit.aspx,post,3,other";
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
                strName = "Choose,forumid,bid,id,act,info";
                strValu = "" + Choose + "'" + forumid + "'" + bid + "'" + id + "'view'ok";
                strOthe = "" + acType + ",Gsedit.aspx,post,3,other|other|other|other|other|other|other|other|other|other";
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
                        strName = "Choose,forumid,bid,id,act,info";
                        if (i == 0)
                            strValu = "" + Choose.Replace(cTemp[i] + ",", "").Replace(cTemp[i], "") + "'" + forumid + "'" + bid + "'" + id + "'view'ok";
                        else
                            strValu = "" + Choose.Replace("," + cTemp[i], "").Replace(cTemp[i], "") + "'" + forumid + "'" + bid + "'" + id + "'view'ok";

                        strOthe = "[删],Gsedit.aspx,post,3,other";
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
                strName = "Choose,forumid,bid,id,act,info";
                strValu = "" + Choose + "'" + forumid + "'" + bid + "'" + id + "'view'ok";
                strOthe = "" + acType + ",Gsedit.aspx,post,3,other|other|other|other|other|other|other|other|other|other";
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
                        strName = "Choose,forumid,bid,id,act,info";
                        if (i == 0)
                            strValu = "" + Choose.Replace(cTemp[i] + ",", "").Replace(cTemp[i], "") + "'" + forumid + "'" + bid + "'" + id + "'view'ok";
                        else
                            strValu = "" + Choose.Replace("," + cTemp[i], "").Replace(cTemp[i], "") + "'" + forumid + "'" + bid + "'" + id + "'view'ok";

                        strOthe = "[删],Gsedit.aspx,post,3,other";
                        builder.Append(Out.wapform(strName, strValu, strOthe));
                    }
                    builder.Append(Out.Tab("</div>", ""));
                }
            }

            strText = ",,,,";
            strName = "Choose,forumid,bid,id,act";
            strType = "hidden,hidden,hidden,hidden,hidden";
            strValu = "" + Choose + "'" + forumid + "'" + bid + "'" + id + "'save";
            strEmpt = "false,false,false,false,false";
            if (id > 0)
                strOthe = "/&gt;确定修改,Gsedit.aspx,post,0,red";
            else
                strOthe = "/&gt;确定添加,Gsedit.aspx,post,0,red";

            if (id > 0)
            {
                strText += ",主题(" + ub.GetSub("BbsThreadMax", xmlPath) + "字内):,内容(" + ub.GetSub("BbsContentMax", xmlPath) + "字内):/";
                strName += ",Title,Content";
                strType += ",text,big";
                strValu += "'" + modelT.Title + "'" + modelT.Content + "";
                strEmpt += ",false,false";
            }
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "") + "\">返回上一级</a>");
            builder.Append(Out.Tab("</div>", ""));

        }
    }


    private void SavePage(int forumid, int bid, int id)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

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
        if (id > 0)
        {
            BCW.Model.Forumvote model = new BCW.BLL.Forumvote().GetForumvote(id);
            if (model == null)
            {
                Utils.Error("不存在的记录", "");
            }
            if (model.UsID != meid)
            {
                Utils.Error("不存在的记录", "");
            }
        }
        else
        {
            BCW.Model.Text modelT = new BCW.BLL.Text().GetText(bid);
            if (modelT == null)
            {
                Utils.Error("不存在的记录", "");
            }
            if (modelT.UsID != meid)
            {
                Utils.Error("不存在的记录", "");
            }
            if (GsqiNum == modelT.Gqinum)
            {
                Utils.Error("本期已参赛过了", "");
            }

        }
        if (new BCW.User.ForumInc().IsForumGSIDS(forumid) == true)
        {

            string Choose = "";
            Choose = Utils.GetRequest("Choose", "post", 2, @"^[^\,]{1,50}(?:\,[^\,]{1,50}){0,500}$", "参赛内容选择出错");

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
            string mename = new BCW.BLL.User().GetUsName(meid);
            if (id > 0)
            {
                string Title = Utils.GetRequest("Title", "post", 2, @"^[\s\S]{1," + ub.GetSub("BbsThreadMax", xmlPath) + "}$", "标题限1-50字");
                string Content = Utils.GetRequest("Content", "post", 2, @"^[\s\S]{1," + ub.GetSub("BbsContentMax", xmlPath) + "}$", "请输入不超" + ub.GetSub("BbsContentMax", xmlPath) + "字的内容");

                BCW.Model.Text model2 = new BCW.Model.Text();
                model2.ID = bid;
                model2.Title = Title;
                model2.Content = Content;
                new BCW.BLL.Text().Update(model2);

                new BCW.BLL.Forumvote().UpdateNotes(id, Choose);
                //记录日志
                string strLog = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]修改了参赛内容!主题[url=/bbs/topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "]《" + new BCW.BLL.Text().GetTitle(bid) + "》[/url]";
                new BCW.BLL.Forumlog().Add(7, forumid, bid, strLog);
            }
            else
            {
                BCW.Model.Forumvote f = new BCW.Model.Forumvote();
                f.Types = 0;
                f.ForumID = forumid;
                f.BID = bid;
                f.Notes = Choose;
                f.state = 0;
                f.qiNum = GsqiNum;
                f.IsWin = 0;
                f.AddTime = DateTime.Now;
                f.UsID = meid;
                f.UsName = mename;
                new BCW.BLL.Forumvote().Add(f);
                //更新期数进主题记录并加历史次数
                new BCW.BLL.Text().UpdateGqinum(bid, GsqiNum);
            }
            Utils.Success("参赛", "操作成功，正在进入参赛帖...", Utils.getUrl("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + ""), "2");


        }

    }
}