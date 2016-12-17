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


public partial class API_PrefabPage_content : System.Web.UI.Page
{
    public int threadId;
    public BCW.Model.Text tbText;
    

    protected void Page_Load( object sender, EventArgs e )
    {
        threadId = Utils.ParseInt(Utils.GetRequest( "threadId", "get", 1, @"^\d*$", "" ));
        tbText = new BCW.BLL.Text().GetText( threadId ); 
    }

    public string getPhoto(int usId)
    {
        string _imgPath = new BCW.BLL.User().GetPhoto( usId );
        if( _imgPath == "" )
            _imgPath = "/images/weixi.png";
        return "http://"+Utils.GetDomain()+ _imgPath;
    }

    public string getContent()
    {
        return tbText != null ? Out.SysUBB( tbText.Content ) : "";
    }

}
