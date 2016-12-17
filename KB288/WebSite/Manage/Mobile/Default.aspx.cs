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

public partial class Manage_MobileDefault : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder( "" );

    protected void Page_Load( object sender, EventArgs e )
    {
        //int meid = new BCW.User.Users().GetUsId();
        //if( meid == 0 )
        //    Utils.Login();

        Master.Title = "App管理中心";
        builder.Append( Out.Tab( "<div class=\"title\">", "" ) );
        builder.Append("App管理中心");
        builder.Append( Out.Tab( "</div>", "" ) );

        builder.Append( Out.Tab( "<div class=\"Text\">", "" ) );
        builder.Append( "<a href=\"" + Utils.getUrl( "Slider.aspx" ) + "\">轮播管理</a><br />" );
        builder.Append( Out.Tab( "</div>", "" ) );

        builder.Append( Out.Tab( "</div><div class=\"title\"><a href=\"" + Utils.getUrl( "../default.aspx" ) + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl( "default.aspx" ) + "\">返回管理中心</a>" ) );
        builder.Append( Out.Tab( "</div>", "<br />" ) );

    }
}
