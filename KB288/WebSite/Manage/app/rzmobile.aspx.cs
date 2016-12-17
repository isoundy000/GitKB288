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
using BCW.Files;

public partial class Manage_app_rzmobile : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "认证手机号列表";
        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId != 1)
        {
            Utils.Error("权限不足", "");
        }
        int ptype= int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-3]$", "0"));
        if (ptype > 0)
        {
            DataSet ds = new BCW.BLL.User().GetList("mobile", "IsVerify=1 and mobile<>'15107583999' and mobile<>'15107583888' and mobile<>'15107582999' order by regtime desc");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string Mobile = ds.Tables[0].Rows[i]["mobile"].ToString();

                    if (ptype == 1)
                        builder.Append(Mobile + "<br />");
                    else if (ptype == 2)
                        builder.Append(Mobile + "#");
                    else if (ptype == 3)
                        builder.Append(Mobile + ",");
                }
            }
        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("使用IE浏览器查看才可以浏览完全");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("rzmobile.aspx?ptype=1") + "\">换行查看</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("rzmobile.aspx?ptype=2") + "\">#分开查看</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("rzmobile.aspx?ptype=3") + "\">,分开查看</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">系统服务中心</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
}