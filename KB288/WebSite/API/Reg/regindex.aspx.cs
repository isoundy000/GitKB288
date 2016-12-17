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

public partial class API_Reg_regindex : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder( "" );

    protected void Page_Load( object sender, EventArgs e )
    {
        builder.Append( Out.Tab( "<div class=\"title\">免费注册会员</div>", "" ) );

        string strText = "手机号码/,验证码/,*设定密码(6-20字符):/,*确认密码:/,,,";
        string strName = "mobile,verifycode,pwd,pwdr,backurl";
        string strType = "text,text,password,password,hidden";
        string strValu = "'''''" + Utils.getPage( 0 ) + "";
        string strEmpt = "false,false,false,false,false";
        string strIdea = "/";
        string strOthe = "注册,mobileReg.ashx,post,0,red";
        builder.Append( Out.wapform( strText, strName, strType, strValu, strEmpt, strIdea, strOthe ) );
    }

}
