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
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using BCW.Data;


public partial class Sms10086 : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "新消息声音提示（本页20秒刷新一次）";
        //Master.IsFoot = false;
        Master.Refresh = 20;
        Master.Gourl = Utils.getUrl("Sms10086.aspx");

        int uid = new BCW.User.Users().GetUsId();
        if (uid == 0)
            Utils.Login();

        bool Isgo = false;

        if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th"))
        {
            if (uid == 111 || uid == 666 || uid == 888 || uid == 555 || uid == 10086 || uid == 100)
            {
                Isgo = true;
            }
        }
        else
        {
            if (uid == 1119 || uid == 8888 || uid == 666888 || uid == 388 || uid == 7888 || uid == 10086)
            {
                Isgo = true;
            }
        }
        if (Isgo)
        {
            int meid = uid;

            int smsCount = new BCW.BLL.Guest().GetCount(meid);
            int smsXCount = new BCW.BLL.Guest().GetXCount(meid);
            if (smsCount > 0 || smsXCount > 0)
            {

                string GuestText = "<b>新内线(" + smsCount + ")系统(" + smsXCount + ")</b><br />";

                builder.Append(Out.Tab("<div>", ""));
                builder.Append(GuestText);
                builder.Append("<embed src=\"sms.wav\" height=\"0\" width=\"0\"/>使用IE、opera8.54等浏览器可以听到声音提示");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            else
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("暂无新消息");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
    }
}