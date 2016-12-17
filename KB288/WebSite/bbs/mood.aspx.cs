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
/// 修改人 陈志基 2016/4/9
/// 切换心情图片
/// 
/// </summary>
public partial class bbs_mood : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "帖子心情";
        int forumid = int.Parse(Utils.GetRequest("forumid", "all", 2, @"^[0-9]\d*$", "论坛ID错误"));
        int bid = int.Parse(Utils.GetRequest("bid", "all", 2, @"^[0-9]\d*$", "帖子ID错误"));
        if (!new BCW.BLL.Forum().Exists2(forumid))
        {
            Utils.Success("访问论坛", "该论坛不存在或已暂停使用", Utils.getUrl("forum.aspx"), "1");
        }
        if (!new BCW.BLL.Text().Exists2(bid, forumid))
        {
            Utils.Error("帖子不存在或已被删除", "");
        }
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("看了本帖的心情");
        builder.Append(Out.Tab("</div>", ""));
        DataSet ds = new BCW.BLL.Text().GetList("ReStats,ReList", "ID=" + bid + "");
        string ReStats = ds.Tables[0].Rows[0]["ReStats"].ToString();
        string ReList = "|" + ds.Tables[0].Rows[0]["ReList"].ToString() + "|";
        int v = int.Parse(Utils.GetRequest("v", "get", 1, @"^[0-9]\d*$", "0"));//4
        //  int v = int.Parse(Utils.GetRequest("v", "get", 1, @"^[0-9]\d*$", "0"));//  2
        if (string.IsNullOrEmpty(ReStats))
            ReStats = "0|0|0|0|0";
        string sStats = string.Empty;
        //
        #region  修改
        bool temp = false;  //判断修改标志  默认不可以修改
        string jungle = meid.ToString();
        string[] list = ds.Tables[0].Rows[0]["ReList"].ToString().Split('|');
        for (int i = 0; i < list.Length; i++)
        {
            // builder.Append("list:"+jungle + "-");
            if (jungle.Trim() == list[i].Trim())//如果包含meid
            {
                temp = false;  //设置为不可以修改
            }
            else//如果不包含meid
            {
                temp = true;  //设置为可以修改
            }
        }
        bool test = !("|" + ReList + "|").Contains("|" + meid + "|");

        #endregion

        //结束
        if (v != 0 && temp && test)//不包含这个ID
        {
            if (v >= 1 && v <= 5)  //限定 V 的取值在 1-5之间才能修改数据
            {
                string[] arrReStats = ReStats.Split("|".ToCharArray());
                for (int i = 0; i < arrReStats.Length; i++) //5
                {
                    if ((v - 1) == i)
                    {
                        sStats += "|" + Convert.ToInt32(Convert.ToInt32(arrReStats[i]) + 1);//重写数据
                    }
                    else
                    {
                        sStats += "|" + arrReStats[i];//原有数据
                    }
                }
                sStats = Utils.Mid(sStats, 1, sStats.Length);
                string result = string.Empty;
                if (string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ReList"].ToString()))//判断是否为空
                {
                    result = ds.Tables[0].Rows[0]["ReList"].ToString() + meid;
                }
                else
                {
                    result = ds.Tables[0].Rows[0]["ReList"].ToString() + "|" + meid;
                }
                //builder.Append(ds.Tables[0].Rows[0]["ReList"].ToString() + "<br/>");
                //builder.Append(result + "  写入数据库的数据result");
                // ReList = Utils.Mid(ReList + "|" + meid, 1, (ReList + "|" + meid).Length);
                new BCW.BLL.Text().UpdateReStats(bid, sStats, result);

            }
        }
        else
        {
            sStats = ReStats;

        }

        string ReText = "快乐|伤心|幽默|好帖";
        string[] arrText = ReText.Split("|".ToCharArray());
        string[] arrsStats = sStats.Split("|".ToCharArray());
        for (int i = 0; i < arrsStats.Length-1; i++)
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            // builder.Append(i+"<br/>");
            builder.Append("<a href=\"" + Utils.getUrl("mood.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;v=" + (i + 1) + "&amp;backurl=" + Utils.getPage(0) + "") + "\"><img src=\"/files/face/em" + i + ".gif\" alt=\"load\"/>" + arrText[i] + "(" + arrsStats[i] + ")</a>");
            builder.Append(Out.Tab("</div>", ""));
        }

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("topic.aspx?forumid=" + forumid + "&amp;bid=" + bid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;返回主题帖</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("注:同心情超5人次,帖子将会出现心情图标");
        builder.Append(Out.Tab("</div>", ""));
    }
}