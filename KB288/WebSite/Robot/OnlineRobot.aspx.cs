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

public partial class Robot_OnlineRobot : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string IDs = ub.GetSub("XiaoHao", "/Controls/xiaohao.xml");
        if (IDs != "")
        {

            int maxNum = 6000;//取小号在线人数

            //按时间段抽取在线
            int hour = DateTime.Now.Hour;
            if (hour > 2 && hour < 7)
                maxNum = new Random().Next(2200, 2500);
            else
                maxNum = new Random().Next(3800, 4100);

            maxNum=maxNum*2;
            int OnLineNum = new BCW.BLL.User().GetNum(3);
            if (OnLineNum < maxNum)
            {
                int cNum = maxNum - OnLineNum;
                string[] array = IDs.Split("#".ToCharArray());

                string[] arrayStr = Utils.GetRandom(array, cNum);
                for (int i = 0; i < arrayStr.Length; i++)
                {
                    int uid = Utils.ParseInt(arrayStr[i]);
                    //if (BCW.User.Users.UserOnline(uid) == "离线")
                    //{
                        new BCW.BLL.User().UpdateTime(Utils.ParseInt(arrayStr[i]), 5);
                    //}
                    
                }
            }

            Response.Write("ok!");
        }
    }
}
