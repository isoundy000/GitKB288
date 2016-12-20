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

public partial class gourl : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int UsId = meid;

        BCW.Model.User model = new BCW.Model.User();
        model = new BCW.BLL.User().GetKey(UsId);

        string UsKey = model.UsKey;
        string UsPwd = model.UsPwd;

        //加密用户密码
        string strPwd = Utils.MD5Str(UsPwd.Trim());
        //设置keys
        string keys = "";
        keys = BCW.User.Users.SetUserKeys(UsId, UsPwd, UsKey);
        string bUrl = string.Empty;
        if (Utils.getPage(1) != "")
        {
            bUrl = Utils.getUrl(Utils.removeUVe(Utils.getPage(1)));
        }
        else
        {
            bUrl = Utils.getUrl("/default.aspx");
        }
        //更新识别串
        string SID = ConfigHelper.GetConfigString("SID");
        bUrl = UrlOper.UpdateParam(bUrl, SID, keys);


        Master.Title = "登录成功";
        builder.Append(Out.Tab("<div class=\"title\">登录成功</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("保存以下书签即可<br />");
        builder.Append("<a href=\"" + Utils.getUrl("" + bUrl + "") + "\">进可保存书签</a>-");

        builder.Append(Out.Tab("</div>", ""));
    }
}