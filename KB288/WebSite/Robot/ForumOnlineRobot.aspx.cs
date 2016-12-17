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
/// 邵广林 20161114
/// 增加try
/// </summary>

public partial class Robot_ForumOnlineRobot : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        //得到在线的小号集合
        DataSet ds = new BCW.BLL.User().GetList("ID", "IsSpier=1 and EndTime>='" + DateTime.Now.AddMinutes(-Convert.ToInt32(ub.Get("SiteExTime"))) + "' ");
        try
        {
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                int max = ds.Tables[0].Rows.Count;
                string[] maxArray = new string[max];
                for (int i = 0; i < max; i++)
                {
                    maxArray[i] = ds.Tables[0].Rows[i]["ID"].ToString();
                }

                //新手22/交易2/足篮3/游戏15/音乐11/图吧8/笑话7/财经12/彩票28/杂谈6/灌水101/电脑30/心灵栖息10/幽兰雅轩9
                //奋斗人生21/蓝雨之恋19/天山情缘102/小说专区103


                string[] ForumStrID = { "22", "2", "3", "15", "11", "8", "7", "12", "28", "6", "101", "30", "10", "9", "21", "19", "102", "103", "4", "23" };

                string[] ForumCount = { "60", "140", "140", "60", "40", "100", "100", "50", "16", "120", "176", "10", "60", "30", "16", "16", "16", "16", "32", "30" };

                for (int k = 0; k < ForumStrID.Length; k++)
                {

                    int forumid = Utils.ParseInt(ForumStrID[k]);
                    //执行前先去掉之前的小号在线
                    BCW.Data.SqlHelper.ExecuteSql("update tb_user set EndForumID=0 where EndForumID=" + forumid + " and IsSpier=1");

                    int count = Utils.ParseInt(ForumCount[k]) * 2;
                    if (count > max)
                    {
                        count = max;
                    }
                    int hour = DateTime.Now.Hour;
                    if (hour > 2 && hour < 7)
                    {
                        count = Utils.ParseInt((count / 2).ToString());
                    }
                    string[] arrayStr = Utils.GetRandom(maxArray, count);
                    for (int i = 0; i < arrayStr.Length; i++)
                    {
                        int uid = Utils.ParseInt(arrayStr[i]);
                        BCW.Data.SqlHelper.ExecuteSql("update tb_user set EndForumID=" + forumid + " where id=" + uid + "");
                    }
                }
            }
        }
        catch { }

        Response.Write("ok!");
    }
}
