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
using BCW.Data;

public partial class bbs_game_iosinterface : System.Web.UI.Page
{
    protected void Page_Load( object sender, EventArgs e )
    {

         
        DataSet ds = BCW.Data.SqlHelper.Query( "select id,title,braga,bragb,usname from dbo.tb_Brag" );
        if( ds.Tables[ 0 ].Rows.Count > 0 )
        {
            for( int i = 0; i < ds.Tables[ 0 ].Rows.Count - 1;i++ )
                Response.Write( string.Format( "{0},{1},{2},{3},{4}<br />", 
                    ds.Tables[ 0 ].Rows[ i ][ "id" ].ToString(),
                    ds.Tables[ 0 ].Rows[ i ][ "title" ].ToString(), 
                    ds.Tables[ 0 ].Rows[ i ][ "braga" ].ToString(),
                    ds.Tables[ 0 ].Rows[ i ][ "bragb" ].ToString(),
                    ds.Tables[ 0 ].Rows[ i ][ "usname" ].ToString() ));
        }

    }
}
