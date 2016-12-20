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

public partial class Manage_login : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "后台管理";
        string ac = Utils.GetRequest("ac", "all", 1, "", "");

        if (Utils.ToSChinese(ac) == "登录后台")
        {
            string userName = Utils.GetRequest("userName", "post", 2, @"^[A-Za-z0-9]+$", "请正确输入用户名");
            string userPass = Utils.GetRequest("userPass", "post", 2, @"^[A-Za-z0-9]+$", "请正确输入密码");
            BCW.Model.Manage model = new BCW.Model.Manage();
            model.sUser = userName;
            model.sPwd = Utils.MD5(userPass);
            BCW.BLL.Manage bll = new BCW.BLL.Manage();
            if (bll.GetManageRow(model) > 0)
            {
                BCW.Model.Manage modelManage = bll.GetModelByModel(model.sUser, model.sPwd);
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("登录成功");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("上次登录:" + DT.DateDiff(DateTime.Now, Convert.ToDateTime(modelManage.sTime)) + "前");
                string VE = ConfigHelper.GetConfigString("VE");
                string SID = ConfigHelper.GetConfigString("SID");
                builder.Append("<br /><a href=\"Default.aspx?" + VE + "=" + Utils.getstrVe() + "&amp;" + SID + "=" + modelManage.sKeys + new Rand().RandNume(4) + "\">马上进入后台</a>");
                //更新登录时间
                modelManage.sTime = DateTime.Now;
                bll.UpdateTimeIP(modelManage);
            }
            else
            {
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("登录失败");
                builder.Append(Out.Tab("</div>", ""));
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("login.aspx") + "\">返回继续</a>");
            }
            builder.Append(Out.Tab("</div>", ""));
        }
        else if (ac == "exit")
        {
            string AdminPath = ConfigHelper.GetConfigString("AdminPath");
            //后台管理员权限判断
            int ManageId = new BCW.User.Manage().IsManageLogin();
            if (ManageId == 0)
            {
                Response.Redirect("/" + AdminPath + "/login.aspx");
                Response.End();
            }
            //更新Keys
            BCW.Model.Manage modelkeys = new BCW.Model.Manage();
            modelkeys.ID = BCW.User.Users.GetIDByKeys(Utils.getstrU());
            modelkeys.sKeys = BCW.User.Users.SetUserKeys(modelkeys.ID, "", new Rand().RandNum(10));
            modelkeys.sKeys = Utils.Mid(modelkeys.sKeys, 0, modelkeys.sKeys.Length - 4);
            new BCW.BLL.Manage().UpdateKeys(modelkeys);

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("退出成功<br />");
            builder.Append("<a href=\"" + Utils.getUrl("login.aspx") + "\">继续登录</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("欢迎您进入管理后台");
            builder.Append(Out.Tab("</div>", ""));
            string strText = "用户,密码";
            string strName = "userName,userPass";
            string strType = "text,password";
            string strValu = "''";
            string strEmpt = "false,false";
            string strIdea = "/";
            string strOthe = "登录后台|reset,login.aspx,post,0,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
    }
}
