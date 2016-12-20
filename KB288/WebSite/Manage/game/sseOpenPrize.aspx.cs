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
using BCW.SSE;
using BCW.Common;

public partial class Manage_game_sseOpenPrize : System.Web.UI.Page
{
    private string xmlPath = "/Controls/SSE.xml";

    protected void Page_Load( object sender, EventArgs e )
    {

        HttpContext.Current.Response.Write( string.Format( "值：{0}<br />", ub.GetSub( "SSEPrizeCalcRate", xmlPath ) ) );
        //获取新一期的开奖数据.
        BCW.SSE.SSEDataParseNew _c = new BCW.SSE.SSEDataParseNew();
        _c.autoOpenPrize = true;
        if (_c.GetNewSSEData() == true) 
          SetNextSseTime();
    }

    private void SetNextSseTime()
    {
        DateTime _openTime = DateTime.Parse( DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + " " + ub.GetSub( "SSEBeginDefault", xmlPath ) );


        DateTime _tempcloseTime = _openTime;
        //跳过星期六日.                 
        if( _tempcloseTime.DayOfWeek == DayOfWeek.Friday )
            _tempcloseTime = _tempcloseTime.AddDays( 3 );
        else if( System.DateTime.Now.DayOfWeek == DayOfWeek.Saturday )
            _tempcloseTime = _tempcloseTime.AddDays( 2 );
        else
            _tempcloseTime = _tempcloseTime.AddDays( 1 );
        DateTime _closeTime = DateTime.Parse( _tempcloseTime.Year.ToString() + "-" + _tempcloseTime.Month.ToString() + "-" + _tempcloseTime.Day.ToString() + " " + ub.GetSub( "SSEEndDefault", xmlPath ) );

        DateTime _openPrizeTime = DateTime.Parse( _closeTime.Year.ToString() + "-" + _closeTime.Month.ToString() + "-" + _closeTime.Day.ToString() + " " + ub.GetSub( "SSEOpenDefault", xmlPath ) );

        ub xml = new ub();
        Application.Remove( xmlPath );//清缓存
        xml.ReloadSub( xmlPath );                   //加载配置  

        xml.dss[ "SSEBegin" ] = _openTime.ToString( "yyyy-MM-dd HH:mm:ss" );
        xml.dss[ "SSEEnd" ] = _closeTime.ToString( "yyyy-MM-dd HH:mm:ss" );
        xml.dss[ "SSEOpen" ] = _openPrizeTime.ToString( "yyyy-MM-dd HH:mm:ss" );
        System.IO.File.WriteAllText( Server.MapPath( xmlPath ), xml.Post( xml.dss ), System.Text.Encoding.UTF8 ); 
    }
}
